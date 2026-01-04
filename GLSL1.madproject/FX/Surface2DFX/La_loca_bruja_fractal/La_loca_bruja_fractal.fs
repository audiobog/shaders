/*{
  "RESOURCE_TYPE": "Surface 2D FX For MadMapper",
  "CREDIT": "Made with MadAI",
  "DESCRIPTION": "3D holographic effect with audio reactivity - bass deforms, mids rotate, treble shimmers",
  "TAGS": "hologram,3D,audio,glossy,sci-fi,material",
  "VSN": "1.0",
  "INPUTS": [
    { "LABEL": "Hologram/Scale", "NAME": "fx_holo_scale", "TYPE": "float", "DEFAULT": 1.0, "MIN": 0.5, "MAX": 3.0, "DESCRIPTION": "Hologram size" },
    { "LABEL": "Hologram/Layers", "NAME": "fx_holo_layers", "TYPE": "float", "DEFAULT": 3.0, "MIN": 1.0, "MAX": 8.0, "DESCRIPTION": "Number of layers" },
    { "LABEL": "Hologram/Depth", "NAME": "fx_holo_depth", "TYPE": "float", "DEFAULT": 1.5, "MIN": 0.5, "MAX": 3.0, "DESCRIPTION": "Depth intensity" },
    { "LABEL": "Hologram/Glossiness", "NAME": "fx_glossiness", "TYPE": "float", "DEFAULT": 0.8, "MIN": 0.0, "MAX": 1.0, "DESCRIPTION": "Glossy reflection" },
    { "LABEL": "Hologram/Opacity", "NAME": "fx_opacity", "TYPE": "float", "DEFAULT": 0.7, "MIN": 0.0, "MAX": 1.0, "DESCRIPTION": "Overall opacity" },
    { "LABEL": "Color/Primary", "NAME": "fx_color_primary", "TYPE": "color", "DEFAULT": [0.0, 1.0, 0.8, 1.0], "FLAGS": ["no_alpha"], "DESCRIPTION": "Primary holo color" },
    { "LABEL": "Color/Secondary", "NAME": "fx_color_secondary", "TYPE": "color", "DEFAULT": [0.2, 0.8, 1.0, 1.0], "FLAGS": ["no_alpha"], "DESCRIPTION": "Secondary holo color" },
    { "LABEL": "Color/Accent", "NAME": "fx_color_accent", "TYPE": "color", "DEFAULT": [1.0, 0.4, 0.8, 1.0], "FLAGS": ["no_alpha"], "DESCRIPTION": "Accent shimmer color" },
    { "LABEL": "Audio/Bass Strength", "NAME": "fx_bass_strength", "TYPE": "float", "DEFAULT": 0.6, "MIN": 0.0, "MAX": 1.0, "DESCRIPTION": "Bass deformation" },
    { "LABEL": "Audio/Mid Strength", "NAME": "fx_mid_strength", "TYPE": "float", "DEFAULT": 0.5, "MIN": 0.0, "MAX": 1.0, "DESCRIPTION": "Mid rotation" },
    { "LABEL": "Audio/Treble Strength", "NAME": "fx_treble_strength", "TYPE": "float", "DEFAULT": 0.8, "MIN": 0.0, "MAX": 1.0, "DESCRIPTION": "Treble shimmer" },
    { "LABEL": "Audio/Smoothness", "NAME": "fx_audio_smooth", "TYPE": "float", "DEFAULT": 0.15, "MIN": 0.0, "MAX": 0.5, "DESCRIPTION": "Audio smoothing" },
    { "NAME": "fx_waveformFFT", "TYPE": "audioFFT", "SIZE": 64, "RELEASE": "fx_audio_smooth", "ATTACK": 0 }
  ],
  "GENERATORS": [
    { "NAME": "fx_time_base", "TYPE": "time_base", "PARAMS": {"speed": 1.0, "bpm_sync": false} }
  ]
}*/

#ifndef PI
#define PI 3.14159265359
#endif

float f_hash(vec3 p_pos) {
  float v_h = dot(p_pos, vec3(127.1, 311.7, 74.7));
  return fract(sin(v_h) * 43758.5453123);
}

float f_noise3d(vec3 p_pos) {
  vec3 v_i = floor(p_pos);
  vec3 v_f = fract(p_pos);
  vec3 v_u = v_f * v_f * (3.0 - 2.0 * v_f);
  
  float v_n0 = mix(mix(mix(f_hash(v_i + vec3(0.0, 0.0, 0.0)), f_hash(v_i + vec3(1.0, 0.0, 0.0)), v_u.x),
                        mix(f_hash(v_i + vec3(0.0, 1.0, 0.0)), f_hash(v_i + vec3(1.0, 1.0, 0.0)), v_u.x), v_u.y),
                   mix(mix(f_hash(v_i + vec3(0.0, 0.0, 1.0)), f_hash(v_i + vec3(1.0, 0.0, 1.0)), v_u.x),
                        mix(f_hash(v_i + vec3(0.0, 1.0, 1.0)), f_hash(v_i + vec3(1.0, 1.0, 1.0)), v_u.x), v_u.y), v_u.z);
  return v_n0;
}

float f_fbm(vec3 p_pos, int p_octaves) {
  float v_value = 0.0;
  float v_amplitude = 1.0;
  float v_frequency = 1.0;
  float v_max = 0.0;
  
  for (int i = 0; i < 8; i++) {
    if (i >= p_octaves) break;
    v_value += v_amplitude * f_noise3d(p_pos * v_frequency);
    v_max += v_amplitude;
    v_amplitude *= 0.5;
    v_frequency *= 2.0;
  }
  return v_value / v_max;
}

vec3 f_spherical_distortion(vec3 p_pos, float p_bass, float p_mid) {
  float v_r = length(p_pos);
  float v_bass_distort = 1.0 + p_bass * 0.5;
  float v_theta = atan(p_pos.y, p_pos.x) + p_mid * PI;
  float v_phi = acos(p_pos.z / (v_r + 0.001));
  
  vec3 v_distorted = vec3(
    v_r * v_bass_distort * sin(v_phi) * cos(v_theta),
    v_r * v_bass_distort * sin(v_phi) * sin(v_theta),
    v_r * v_bass_distort * cos(v_phi)
  );
  
  return v_distorted;
}

float f_hologram_sdf(vec3 p_pos, float p_bass, float p_mid, float p_scale) {
  vec3 v_distorted = f_spherical_distortion(p_pos, p_bass, p_mid);
  float v_sphere = length(v_distorted) - p_scale;
  
  float v_wave = sin(v_distorted.x * 8.0 + fx_time_base * 2.0) * 0.1;
  v_wave += sin(v_distorted.y * 8.0 + fx_time_base * 2.5) * 0.1;
  v_wave += sin(v_distorted.z * 8.0 + fx_time_base * 2.2) * 0.1;
  
  float v_noise = f_fbm(v_distorted * 3.0 + vec3(fx_time_base), 3) * 0.15;
  
  return v_sphere + v_wave + v_noise;
}

vec3 f_hologram_normal(vec3 p_pos, float p_bass, float p_mid, float p_scale) {
  float v_eps = 0.001;
  float v_dx = f_hologram_sdf(p_pos + vec3(v_eps, 0.0, 0.0), p_bass, p_mid, p_scale) - f_hologram_sdf(p_pos - vec3(v_eps, 0.0, 0.0), p_bass, p_mid, p_scale);
  float v_dy = f_hologram_sdf(p_pos + vec3(0.0, v_eps, 0.0), p_bass, p_mid, p_scale) - f_hologram_sdf(p_pos - vec3(0.0, v_eps, 0.0), p_bass, p_mid, p_scale);
  float v_dz = f_hologram_sdf(p_pos + vec3(0.0, 0.0, v_eps), p_bass, p_mid, p_scale) - f_hologram_sdf(p_pos - vec3(0.0, 0.0, v_eps), p_bass, p_mid, p_scale);
  return normalize(vec3(v_dx, v_dy, v_dz));
}

vec4 f_raymarch_hologram(vec3 p_origin, vec3 p_dir, float p_bass, float p_mid, float p_treble, float p_scale, float p_depth, float p_glossiness) {
  float v_t = 0.0;
  vec3 v_pos = p_origin;
  vec4 v_result = vec4(0.0);
  
  for (int i = 0; i < 64; i++) {
    v_pos = p_origin + p_dir * v_t;
    float v_dist = f_hologram_sdf(v_pos, p_bass, p_mid, p_scale);
    
    if (abs(v_dist) < 0.01) {
      vec3 v_normal = f_hologram_normal(v_pos, p_bass, p_mid, p_scale);
      vec3 v_view_dir = -p_dir;
      
      float v_fresnel = pow(1.0 - abs(dot(v_normal, v_view_dir)), 2.0);
      float v_diffuse = max(0.0, dot(v_normal, normalize(vec3(1.0, 1.0, 1.0)))) * 0.5 + 0.5;
      
      vec3 v_reflect = reflect(p_dir, v_normal);
      float v_spec = pow(max(0.0, dot(v_reflect, v_view_dir)), 32.0 * p_glossiness) * p_glossiness;
      
      float v_shimmer = sin(v_pos.x * 20.0 + fx_time_base * 5.0) * 0.5 + 0.5;
      v_shimmer *= sin(v_pos.y * 20.0 + fx_time_base * 5.5) * 0.5 + 0.5;
      v_shimmer *= p_treble;
      
      vec3 v_base_color = mix(fx_color_primary.rgb, fx_color_secondary.rgb, 0.5 + 0.5 * sin(fx_time_base + length(v_pos)));
      vec3 v_accent_color = fx_color_accent.rgb * v_shimmer * 2.0;
      
      v_result.rgb = v_base_color * (v_diffuse + v_fresnel * 0.5) + v_spec * vec3(1.0) + v_accent_color;
      v_result.a = 1.0;
      break;
    }
    
    v_t += max(v_dist * 0.5, 0.02);
    
    if (v_t > 10.0) break;
  }
  
  return v_result;
}

vec4 fxColorForPixel(vec2 p_uv) {
  vec2 v_uv = p_uv * 2.0 - 1.0;
  float v_aspect = FX_IMG_SIZE().x / FX_IMG_SIZE().y;
  v_uv.x *= v_aspect;
  
  float v_bass = IMG_NORM_PIXEL(fx_waveformFFT, vec2(0.05, 0.0)).x;
  float v_mid = IMG_NORM_PIXEL(fx_waveformFFT, vec2(0.5, 0.0)).x;
  float v_treble = IMG_NORM_PIXEL(fx_waveformFFT, vec2(0.95, 0.0)).x;
  
  v_bass *= fx_bass_strength;
  v_mid *= fx_mid_strength;
  v_treble *= fx_treble_strength;
  
  vec3 v_ray_dir = normalize(vec3(v_uv, 1.5));
  vec3 v_ray_origin = vec3(0.0, 0.0, -3.0);
  
  v_ray_origin.z -= v_bass * fx_holo_depth * 0.5;
  v_ray_origin.x += sin(fx_time_base + v_mid * PI) * 0.5;
  v_ray_origin.y += cos(fx_time_base + v_mid * PI) * 0.5;
  
  vec4 v_hologram = f_raymarch_hologram(v_ray_origin, v_ray_dir, v_bass, v_mid, v_treble, fx_holo_scale, fx_holo_depth, fx_glossiness);
  
  vec4 v_input = FX_NORM_PIXEL(p_uv);
  
  vec3 v_background = mix(v_input.rgb, vec3(0.05, 0.1, 0.15), 0.7);
  
  float v_glow = exp(-length(v_uv) * 0.5) * 0.3;
  v_background += vec3(0.0, 0.3, 0.4) * v_glow;
  
  vec4 v_final = vec4(mix(v_background, v_hologram.rgb, v_hologram.a * fx_opacity), 1.0);
  
  float v_scan = sin(p_uv.y * 50.0 + fx_time_base * 10.0) * 0.05 + 0.95;
  v_final.rgb *= v_scan;
  
  v_final.rgb += vec3(0.0, 0.2, 0.3) * v_treble * 0.5;
  
  return v_final;
}