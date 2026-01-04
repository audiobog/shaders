/*{
  "RESOURCE_TYPE": "Surface 2D FX For MadMapper",
  "CREDIT": "Made with MadAI",
  "DESCRIPTION": "Desaturate with brightness and contrast, then choose output colour using RGB with hue shift",
  "TAGS": "color,desaturate,brightness,contrast,hue",
  "VSN": "1.0",
  "INPUTS": [
    { "LABEL": "Desaturate/Amount", "NAME": "fx_desaturate", "TYPE": "float", "DEFAULT": 0.0, "MIN": 0.0, "MAX": 1.0, "DESCRIPTION": "Amount of desaturation" },
    { "LABEL": "Desaturate/Brightness", "NAME": "fx_brightness", "TYPE": "float", "DEFAULT": 0.0, "MIN": -1.0, "MAX": 1.0, "DESCRIPTION": "Brightness adjustment" },
    { "LABEL": "Desaturate/Contrast", "NAME": "fx_contrast", "TYPE": "float", "DEFAULT": 1.0, "MIN": 0.0, "MAX": 3.0, "DESCRIPTION": "Contrast adjustment" },
    { "LABEL": "Desaturate/Hue Shift", "NAME": "fx_hue_shift", "TYPE": "float", "DEFAULT": 0.0, "MIN": -180.0, "MAX": 180.0, "DESCRIPTION": "Hue shift in degrees" },
    { "LABEL": "Output Hue/Red Amount", "NAME": "fx_hue_red", "TYPE": "float", "DEFAULT": 1.0, "MIN": 0.0, "MAX": 1.0, "DESCRIPTION": "Red color amount" },
    { "LABEL": "Output Hue/Green Amount", "NAME": "fx_hue_green", "TYPE": "float", "DEFAULT": 1.0, "MIN": 0.0, "MAX": 1.0, "DESCRIPTION": "Green color amount" },
    { "LABEL": "Output Hue/Blue Amount", "NAME": "fx_hue_blue", "TYPE": "float", "DEFAULT": 1.0, "MIN": 0.0, "MAX": 1.0, "DESCRIPTION": "Blue color amount" },
    { "LABEL": "Output Hue/Hue Shift", "NAME": "fx_output_hue_shift", "TYPE": "float", "DEFAULT": 0.0, "MIN": -180.0, "MAX": 180.0, "DESCRIPTION": "Output hue shift in degrees" }
  ]
}*/

#ifndef PI
#define PI 3.14159265359
#endif

vec3 f_rgb_to_hsv(vec3 p_c) {
    vec4 v_k = vec4(0.0, -1.0 / 3.0, 2.0 / 3.0, -1.0);
    vec4 v_p = mix(vec4(p_c.bg, v_k.wz), vec4(p_c.gb, v_k.xy), step(p_c.b, p_c.g));
    vec4 v_q = mix(vec4(v_p.xyw, p_c.r), vec4(p_c.r, v_p.yzx), step(v_p.x, p_c.r));
    float v_d = v_q.x - min(v_q.w, v_q.y);
    float v_e = 1.0e-10;
    return vec3(abs(v_q.z + (v_q.w - v_q.y) / (6.0 * v_d + v_e)), v_d / (v_q.x + v_e), v_q.x);
}

vec3 f_hsv_to_rgb(vec3 p_c) {
    vec4 v_k = vec4(1.0, 2.0 / 3.0, 1.0 / 3.0, 3.0);
    vec3 v_p = abs(fract(p_c.xxx + v_k.xyz) * 6.0 - v_k.www);
    return p_c.z * mix(v_k.xxx, clamp(v_p - v_k.xxx, 0.0, 1.0), p_c.y);
}

vec4 fxColorForPixel(vec2 mm_FragNormCoord) {
    vec4 v_input_color = FX_NORM_PIXEL(mm_FragNormCoord);
    
    // Apply brightness
    vec3 v_bright_color = v_input_color.rgb + fx_brightness;
    
    // Apply contrast
    vec3 v_contrast_color = (v_bright_color - 0.5) * fx_contrast + 0.5;
    
    // Apply hue shift
    vec3 v_hsv = f_rgb_to_hsv(v_contrast_color);
    v_hsv.x = mod(v_hsv.x + fx_hue_shift / 360.0, 1.0);
    vec3 v_hue_shifted = f_hsv_to_rgb(v_hsv);
    
    // Calculate luminance for desaturation
    float v_luminance = dot(v_hue_shifted, vec3(0.299, 0.587, 0.114));
    vec3 v_gray = vec3(v_luminance);
    vec3 v_desaturated = mix(v_hue_shifted, v_gray, fx_desaturate);
    
    // Create target color from hue controls
    vec3 v_target_color = vec3(fx_hue_red, fx_hue_green, fx_hue_blue);
    
    // Mix the desaturated image with the target color based on the original luminance
    vec3 v_final_color = v_target_color * v_luminance;
    
    // Blend between original processed color and target color based on desaturation amount
    v_final_color = mix(v_desaturated, v_final_color, fx_desaturate);
    
    // Apply output hue shift
    vec3 v_output_hsv = f_rgb_to_hsv(v_final_color);
    v_output_hsv.x = mod(v_output_hsv.x + fx_output_hue_shift / 360.0, 1.0);
    v_final_color = f_hsv_to_rgb(v_output_hsv);
    
    return vec4(clamp(v_final_color, 0.0, 1.0), v_input_color.a);
}