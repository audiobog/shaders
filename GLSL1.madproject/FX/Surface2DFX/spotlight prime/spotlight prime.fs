/*{
  "RESOURCE_TYPE": "Surface 2D FX For MadMapper",
  "CREDIT": "Made with MadAI",
  "DESCRIPTION": "Searching spotlight effect",
  "TAGS": "spotlight,reveal",
  "VSN": "1.0",
  "INPUTS": [
    { "LABEL": "Size", "NAME": "fx_size", "TYPE": "float", "DEFAULT": 0.2, "MIN": 0.0, "MAX": 1.0 },
    { "LABEL": "Glow", "NAME": "fx_glow", "TYPE": "float", "DEFAULT": 0.3, "MIN": 0.0, "MAX": 1.0 },
    { "LABEL": "Speed", "NAME": "fx_speed", "TYPE": "float", "DEFAULT": 0.5, "MIN": 0.0, "MAX": 2.0 }
  ],
  "GENERATORS": [
    { "NAME": "fx_time", "TYPE": "time_base", "PARAMS": {"speed": "fx_speed"} }
  ]
}*/

float f_rand(vec2 p_co) {
    return fract(sin(dot(p_co.xy ,vec2(12.9898,78.233))) * 43758.5453);
}

vec2 f_random_pos(float p_time) {
    float v_t = p_time * 0.5;
    float v_x = 0.5 + 0.3 * sin(v_t * 0.7) * cos(v_t * 0.3);
    float v_y = 0.5 + 0.3 * cos(v_t * 0.5) * sin(v_t * 0.4);
    return vec2(v_x, v_y);
}

vec4 fxColorForPixel(vec2 mm_FragNormCoord) {
    vec2 v_pos = f_random_pos(fx_time);
    
    float v_dist = distance(mm_FragNormCoord, v_pos);
    float v_spotlight = 1.0 - smoothstep(fx_size * 0.5, fx_size * 0.5 + fx_glow, v_dist);
    
    vec4 v_source = FX_NORM_PIXEL(mm_FragNormCoord);
    return v_source * v_spotlight;
}