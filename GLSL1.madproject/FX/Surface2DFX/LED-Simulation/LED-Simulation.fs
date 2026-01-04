/*{
  "RESOURCE_TYPE": "Surface 2D FX For MadMapper",
  "CREDIT": "MadTeam",
  "DESCRIPTION": "LED Grid with RGB sub-pixels",
  "TAGS": "graphics,led,rgb",
  "VSN": "1.0",
  "INPUTS": [
    { "LABEL": "Grid/Width", "NAME": "fx_grid_width", "TYPE": "int", "MIN": 1, "MAX": 3500, "DEFAULT": 32 },
    { "LABEL": "Grid/Height", "NAME": "fx_grid_height", "TYPE": "int", "MIN": 1, "MAX": 3500, "DEFAULT": 32 },
    { "LABEL": "LED/Size", "NAME": "fx_led_size", "TYPE": "float", "MIN": 0, "MAX": 1, "DEFAULT": 0.8 },
    { "LABEL": "LED/Feather", "NAME": "fx_led_feather", "TYPE": "float", "MIN": 0, "MAX": 0.5, "DEFAULT": 0.1 },
    { "LABEL": "LED/Brightness", "NAME": "fx_led_brightness", "TYPE": "float", "MIN": 0.0, "MAX": 3.0, "DEFAULT": 1.0 },
    { "LABEL": "LED/Contrast", "NAME": "fx_led_contrast", "TYPE": "float", "MIN": 0.0, "MAX": 3.0, "DEFAULT": 1.0 },
    { "LABEL": "LED/Gap Size", "NAME": "fx_gap_size", "TYPE": "float", "MIN": 0.0, "MAX": 0.5, "DEFAULT": 0.05 },
    { "LABEL": "LED/Shape", "NAME": "fx_led_shape", "TYPE": "long", "DEFAULT": "0", "VALUES": [0,1,2], "LABELS": ["Square", "Round", "Hexagon"] },
    { "LABEL": "RGB/Red X", "NAME": "fx_red_x", "TYPE": "float", "MIN": -0.01, "MAX": 0.01, "DEFAULT": -0.005 },
    { "LABEL": "RGB/Red Y", "NAME": "fx_red_y", "TYPE": "float", "MIN": -0.01, "MAX": 0.01, "DEFAULT": 0.0 },
    { "LABEL": "RGB/Green X", "NAME": "fx_green_x", "TYPE": "float", "MIN": -0.01, "MAX": 0.01, "DEFAULT": 0.0 },
    { "LABEL": "RGB/Green Y", "NAME": "fx_green_y", "TYPE": "float", "MIN": -0.01, "MAX": 0.01, "DEFAULT": 0.0 },
    { "LABEL": "RGB/Blue X", "NAME": "fx_blue_x", "TYPE": "float", "MIN": -0.01, "MAX": 0.01, "DEFAULT": 0.005 },
    { "LABEL": "RGB/Blue Y", "NAME": "fx_blue_y", "TYPE": "float", "MIN": -0.01, "MAX": 0.01, "DEFAULT": 0.0 },
    { "LABEL": "RGB/Intensity", "NAME": "fx_rgb_intensity", "TYPE": "float", "MIN": 0.0, "MAX": 2.0, "DEFAULT": 1.0 },
    { "LABEL": "RGB/R Balance", "NAME": "fx_red_balance", "TYPE": "float", "MIN": 0.0, "MAX": 2.0, "DEFAULT": 1.0 },
    { "LABEL": "RGB/G Balance", "NAME": "fx_green_balance", "TYPE": "float", "MIN": 0.0, "MAX": 2.0, "DEFAULT": 1.0 },
    { "LABEL": "RGB/B Balance", "NAME": "fx_blue_balance", "TYPE": "float", "MIN": 0.0, "MAX": 2.0, "DEFAULT": 1.0 },
    { "LABEL": "Color/Saturation", "NAME": "fx_saturation", "TYPE": "float", "MIN": 0.0, "MAX": 3.0, "DEFAULT": 1.0 },
    { "LABEL": "Color/Temperature", "NAME": "fx_temperature", "TYPE": "float", "MIN": -1.0, "MAX": 1.0, "DEFAULT": 0.0 },
    { "LABEL": "Color/Gamma", "NAME": "fx_gamma", "TYPE": "float", "MIN": 0.1, "MAX": 3.0, "DEFAULT": 1.0 },
    { "LABEL": "Quality/Antialias", "NAME": "fx_antialias", "TYPE": "float", "MIN": 0.0, "MAX": 1.0, "DEFAULT": 0.5 },
    { "LABEL": "Quality/Dithering", "NAME": "fx_dithering", "TYPE": "float", "MIN": 0.0, "MAX": 1.0, "DEFAULT": 0.0 }
  ]
}*/

float f_hash(vec2 p_pos)
{
  return fract(sin(dot(p_pos, vec2(12.9898, 78.233))) * 43758.5453);
}

float f_smoothstep_square(vec2 p_pos, float p_size, float p_feather)
{
  vec2 v_d = abs(p_pos) - vec2(p_size);
  float v_dist = length(max(v_d, 0.0)) + min(max(v_d.x, v_d.y), 0.0);
  return 1.0 - smoothstep(-p_feather, p_feather, v_dist);
}

float f_smoothstep_circle(vec2 p_pos, float p_size, float p_feather)
{
  float v_dist = length(p_pos) - p_size;
  return 1.0 - smoothstep(-p_feather, p_feather, v_dist);
}

float f_smoothstep_hexagon(vec2 p_pos, float p_size, float p_feather)
{
  const vec3 v_k = vec3(-0.866025404, 0.5, 0.577350269);
  p_pos = abs(p_pos);
  p_pos -= 2.0 * min(dot(v_k.xy, p_pos), 0.0) * v_k.xy;
  p_pos -= vec2(clamp(p_pos.x, -v_k.z * p_size, v_k.z * p_size), p_size);
  float v_dist = length(p_pos) * sign(p_pos.y);
  return 1.0 - smoothstep(-p_feather, p_feather, v_dist);
}

vec3 f_adjust_saturation(vec3 p_color, float p_saturation)
{
  float v_gray = dot(p_color, vec3(0.299, 0.587, 0.114));
  return mix(vec3(v_gray), p_color, p_saturation);
}

vec3 f_adjust_temperature(vec3 p_color, float p_temperature)
{
  vec3 v_warm = p_color * vec3(1.0, 0.9, 0.8);
  vec3 v_cool = p_color * vec3(0.8, 0.9, 1.0);
  return mix(p_color, mix(v_cool, v_warm, p_temperature * 0.5 + 0.5), abs(p_temperature));
}

vec3 f_adjust_contrast(vec3 p_color, float p_contrast)
{
  return (p_color - 0.5) * p_contrast + 0.5;
}

vec3 f_apply_gamma(vec3 p_color, float p_gamma)
{
  return pow(max(p_color, vec3(0.0)), vec3(1.0 / p_gamma));
}

vec3 f_apply_dithering(vec3 p_color, vec2 p_pos, float p_amount)
{
  if (p_amount <= 0.0) return p_color;
  
  float v_noise = f_hash(p_pos) - 0.5;
  return p_color + v_noise * p_amount * 0.1;
}

vec4 fxColorForPixel(vec2 mm_FragNormCoord)
{
  float v_square_width = 1.0 / float(fx_grid_width);
  float v_square_height = 1.0 / float(fx_grid_height);
  float v_square_h_num = floor(mm_FragNormCoord.x / v_square_width);
  float v_square_v_num = floor(mm_FragNormCoord.y / v_square_height);
  vec2 v_square_center = vec2((0.5 + v_square_h_num) * v_square_width, (0.5 + v_square_v_num) * v_square_height);
  
  vec2 v_local_pos = (mm_FragNormCoord - v_square_center) / vec2(v_square_width, v_square_height);
  
  float v_led_size = (fx_led_size - fx_gap_size) * 0.5;
  float v_feather = fx_led_feather * (1.0 + fx_antialias);
  
  vec2 v_red_offset = vec2(fx_red_x, fx_red_y);
  vec2 v_green_offset = vec2(fx_green_x, fx_green_y);
  vec2 v_blue_offset = vec2(fx_blue_x, fx_blue_y);
  
  vec2 v_base_uv = v_square_center;
  vec4 v_original_color = FX_NORM_PIXEL(v_base_uv);
  
  v_original_color.rgb = f_adjust_saturation(v_original_color.rgb, fx_saturation);
  v_original_color.rgb = f_adjust_temperature(v_original_color.rgb, fx_temperature);
  v_original_color.rgb = f_adjust_contrast(v_original_color.rgb, fx_led_contrast);
  v_original_color.rgb = f_apply_gamma(v_original_color.rgb, fx_gamma);
  v_original_color.rgb *= fx_led_brightness;
  
  vec2 v_red_pos = v_local_pos - v_red_offset / vec2(v_square_width, v_square_height);
  vec2 v_green_pos = v_local_pos - v_green_offset / vec2(v_square_width, v_square_height);
  vec2 v_blue_pos = v_local_pos - v_blue_offset / vec2(v_square_width, v_square_height);
  
  float v_red_mask, v_green_mask, v_blue_mask;
  
  if (fx_led_shape == 0) {
    v_red_mask = f_smoothstep_square(v_red_pos, v_led_size, v_feather);
    v_green_mask = f_smoothstep_square(v_green_pos, v_led_size, v_feather);
    v_blue_mask = f_smoothstep_square(v_blue_pos, v_led_size, v_feather);
  } else if (fx_led_shape == 1) {
    v_red_mask = f_smoothstep_circle(v_red_pos, v_led_size, v_feather);
    v_green_mask = f_smoothstep_circle(v_green_pos, v_led_size, v_feather);
    v_blue_mask = f_smoothstep_circle(v_blue_pos, v_led_size, v_feather);
  } else {
    v_red_mask = f_smoothstep_hexagon(v_red_pos, v_led_size, v_feather);
    v_green_mask = f_smoothstep_hexagon(v_green_pos, v_led_size, v_feather);
    v_blue_mask = f_smoothstep_hexagon(v_blue_pos, v_led_size, v_feather);
  }
  
  vec3 v_final_color = vec3(0.0);
  v_final_color += vec3(v_original_color.r * v_red_mask * fx_rgb_intensity * fx_red_balance, 0.0, 0.0);
  v_final_color += vec3(0.0, v_original_color.g * v_green_mask * fx_rgb_intensity * fx_green_balance, 0.0);
  v_final_color += vec3(0.0, 0.0, v_original_color.b * v_blue_mask * fx_rgb_intensity * fx_blue_balance);
  
  v_final_color = f_apply_dithering(v_final_color, mm_FragNormCoord * vec2(fx_grid_width, fx_grid_height), fx_dithering);
  
  return vec4(v_final_color, 1.0);
}