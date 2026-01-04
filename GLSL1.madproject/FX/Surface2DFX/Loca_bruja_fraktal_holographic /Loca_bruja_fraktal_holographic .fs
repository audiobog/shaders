/*{
  "RESOURCE_TYPE": "Fractlas_loca_bruja",
  "CREDIT": "La_loca_bruja",
  "DESCRIPTION": "Stereographic projection with audio-reactive warping",
  "TAGS": "distortion,audio,spherical,warping,fractal",
  "VSN": "1.0",
  "INPUTS": [
    { "LABEL": "Distortion/Amount", "NAME": "fx_distortion", "TYPE": "float", "DEFAULT": 1.2, "MIN": 0.0, "MAX": 3.0, "DESCRIPTION": "Warp intensity" },
    { "LABEL": "Animation/Speed", "NAME": "fx_speed", "TYPE": "float", "DEFAULT": 1.0, "MIN": 0.0, "MAX": 3.0, "DESCRIPTION": "Animation speed" },
    { "LABEL": "Audio/Bass React", "NAME": "fx_bass_react", "TYPE": "float", "DEFAULT": 1.0, "MIN": 0.0, "MAX": 2.0, "DESCRIPTION": "Bass reactivity" },
    { "LABEL": "Audio/Mid React", "NAME": "fx_mid_react", "TYPE": "float", "DEFAULT": 1.0, "MIN": 0.0, "MAX": 2.0, "DESCRIPTION": "Mid reactivity" },
    { "LABEL": "Audio/Treble React", "NAME": "fx_treble_react", "TYPE": "float", "DEFAULT": 1.0, "MIN": 0.0, "MAX": 2.0, "DESCRIPTION": "Treble reactivity" },
    { "LABEL": "Warp/Fractal Depth", "NAME": "fx_fractal_depth", "TYPE": "float", "DEFAULT": 2.0, "MIN": 0.5, "MAX": 5.0, "DESCRIPTION": "Fractal iteration depth" },
    { "LABEL": "Projection/Radius", "NAME": "fx_proj_radius", "TYPE": "float", "DEFAULT": 1.5, "MIN": 0.5, "MAX": 3.0, "DESCRIPTION": "Sphere projection radius" },
    { "NAME": "fx_waveformFFT", "TYPE": "audioFFT", "SIZE": 3, "RELEASE": 0.1, "ATTACK": 0 }
  ],
  "GENERATORS": [
    { "NAME": "fx_anim_time", "TYPE": "time_base", "PARAMS": {"speed": "fx_speed", "bpm_sync": false} }
  ]
}*/

#ifdef GL_ES
precision mediump float;
#endif

#ifndef PI
#define PI 3.14159265359
#endif

float f_hash(vec2 p_pos) {
  return fract(sin(dot(p_pos, vec2(12.9898, 78.233))) * 43758.5453);
}

float f_noise(vec2 p_pos) {
  vec2 v_i = floor(p_pos);
  vec2 v_f = fract(p_pos);
  vec2 v_u = v_f * v_f * (3.0 - 2.0 * v_f);
  
  float v_n00 = f_hash(v_i + vec2(0.0, 0.0));
  float v_n10 = f_hash(v_i + vec2(1.0, 0.0));
  float v_n01 = f_hash(v_i + vec2(0.0, 1.0));
  float v_n11 = f_hash(v_i + vec2(1.0, 1.0));
  
  float v_nx0 = mix(v_n00, v_n10, v_u.x);
  float v_nx1 = mix(v_n01, v_n11, v_u.x);
  return mix(v_nx0, v_nx1, v_u.y);
}

float f_fbm(vec2 p_pos, int p_octaves) {
  float v_value = 0.0;
  float v_amplitude = 1.0;
  float v_frequency = 1.0;
  float v_max_value = 0.0;
  
  for (int i = 0; i < 8; i++) {
    if (i >= p_octaves) break;
    v_value += v_amplitude * f_noise(p_pos * v_frequency);
    v_max_value += v_amplitude;
    v_amplitude *= 0.5;
    v_frequency *= 2.0;
  }
  
  return v_value / v_max_value;
}

vec2 f_stereographic_project(vec2 p_uv, float p_radius) {
  vec2 v_centered = p_uv * 2.0 - 1.0;
  float v_len = length(v_centered);
  
  if (v_len > p_radius) {
    return p_uv;
  }
  
  float v_angle = atan(v_centered.y, v_centered.x);
  float v_r = v_len / p_radius;
  float v_theta = v_r * PI;
  
  float v_new_r = tan(v_theta / 2.0) * 2.0;
  
  vec2 v_projected = vec2(cos(v_angle), sin(v_angle)) * v_new_r;
  return v_projected * 0.5 + 0.5;
}

vec2 f_spherical_warp(vec2 p_uv, float p_time, float p_audio_bass, float p_audio_mid, float p_audio_treble) {
  vec2 v_centered = p_uv * 2.0 - 1.0;
  float v_dist = length(v_centered);
  float v_angle = atan(v_centered.y, v_centered.x);
  
  float v_warp = sin(v_angle * 3.0 + p_time) * 0.3;
  v_warp += cos(v_dist * 5.0 - p_time * 1.5) * 0.2;
  v_warp += sin(p_time * 0.7) * p_audio_bass * 0.4;
  v_warp += cos(p_time * 1.2) * p_audio_mid * 0.3;
  v_warp += sin(p_time * 2.0) * p_audio_treble * 0.25;
  
  float v_new_dist = v_dist * (1.0 + v_warp);
  
  vec2 v_warped = vec2(cos(v_angle), sin(v_angle)) * v_new_dist;
  return v_warped * 0.5 + 0.5;
}

vec2 f_fractal_bend(vec2 p_uv, float p_time, float p_depth, float p_audio_bass, float p_audio_mid, float p_audio_treble) {
  vec2 v_pos = p_uv;
  float v_scale = 1.0;
  
  for (float i = 0.0; i < 5.0; i++) {
    if (i >= p_depth) break;
    
    v_pos = abs(v_pos - 0.5) + 0.5;
    
    float v_bend = sin(p_time * (0.5 + i * 0.3) + p_audio_bass * 2.0) * 0.15;
    v_bend += cos(p_time * (0.7 + i * 0.2) + p_audio_mid * 1.5) * 0.1;
    v_bend += sin(p_time * (1.1 + i * 0.4) + p_audio_treble * 2.5) * 0.12;
    
    v_pos += vec2(sin(v_pos.y * 10.0 + p_time), cos(v_pos.x * 10.0 + p_time)) * v_bend;
    
    v_scale *= 0.5;
  }
  
  return v_pos;
}

vec2 f_nonlinear_warp(vec2 p_uv, float p_time, float p_distortion, float p_audio_bass, float p_audio_mid, float p_audio_treble) {
  vec2 v_centered = p_uv * 2.0 - 1.0;
  float v_dist = length(v_centered);
  float v_angle = atan(v_centered.y, v_centered.x);
  
  float v_radial_warp = sin(v_dist * 8.0 - p_time * 2.0) * p_distortion * 0.2;
  v_radial_warp += p_audio_bass * 0.3;
  v_radial_warp += p_audio_mid * 0.2;
  v_radial_warp += p_audio_treble * 0.15;
  
  float v_angular_warp = cos(v_angle * 4.0 + p_time) * p_distortion * 0.15;
  v_angular_warp += sin(p_time * 1.3) * p_audio_mid * 0.2;
  
  float v_new_dist = v_dist * (1.0 + v_radial_warp);
  float v_new_angle = v_angle + v_angular_warp;
  
  vec2 v_warped = vec2(cos(v_new_angle), sin(v_new_angle)) * v_new_dist;
  return v_warped * 0.5 + 0.5;
}

vec4 fxColorForPixel(vec2 mm_FragNormCoord) {
  vec2 v_uv = mm_FragNormCoord;
  float v_aspect = FX_IMG_SIZE().x / FX_IMG_SIZE().y;
  v_uv.x *= v_aspect;
  
  float v_audio_bass = IMG_NORM_PIXEL(fx_waveformFFT, vec2(0.16, 0.0)).x * fx_bass_react;
  float v_audio_mid = IMG_NORM_PIXEL(fx_waveformFFT, vec2(0.5, 0.0)).x * fx_mid_react;
  float v_audio_treble = IMG_NORM_PIXEL(fx_waveformFFT, vec2(0.84, 0.0)).x * fx_treble_react;
  
  vec2 v_warped = v_uv;
  
  v_warped = f_spherical_warp(v_warped, fx_anim_time, v_audio_bass, v_audio_mid, v_audio_treble);
  
  v_warped = f_nonlinear_warp(v_warped, fx_anim_time * 0.5, fx_distortion, v_audio_bass, v_audio_mid, v_audio_treble);
  
  v_warped = f_fractal_bend(v_warped, fx_anim_time * 0.3, fx_fractal_depth, v_audio_bass, v_audio_mid, v_audio_treble);
  
  v_warped = f_stereographic_project(v_warped, fx_proj_radius);
  
  v_warped.x /= v_aspect;
  
  v_warped = clamp(v_warped, 0.0, 1.0);
  
  vec4 v_color = FX_NORM_PIXEL(v_warped);
  
  float v_chromatic = 0.01 * (v_audio_bass + v_audio_mid * 0.5);
  vec4 v_r = FX_NORM_PIXEL(clamp(v_warped + vec2(v_chromatic, 0.0), 0.0, 1.0));
  vec4 v_b = FX_NORM_PIXEL(clamp(v_warped - vec2(v_chromatic, 0.0), 0.0, 1.0));
  
  v_color.r = v_r.r;
  v_color.b = v_b.b;
  
  return v_color;
}