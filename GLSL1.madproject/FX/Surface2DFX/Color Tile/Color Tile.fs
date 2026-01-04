/*{
  "RESOURCE_TYPE": "Surface 2D FX For MadMapper",
  "CREDIT": "Made by Joe Griffith  with MadAI",
  "DESCRIPTION": "Tile effect with animated RGB color tint variations and shape morphing",
  "TAGS": "tile,grid,color,glitch,animated,morph",
  "VSN": "1.0",
  "INPUTS": [
    { "LABEL": "Horiz", "NAME": "fx_tiles_h", "TYPE": "int", "DEFAULT": 22, "MIN": 1, "MAX": 32, "DESCRIPTION": "Number of horizontal tiles" },
    { "LABEL": "Vert", "NAME": "fx_tiles_v", "TYPE": "int", "DEFAULT": 22, "MIN": 1, "MAX": 32, "DESCRIPTION": "Number of vertical tiles" },
    { "LABEL": "RGB Effect", "NAME": "fx_rgb_amount", "TYPE": "float", "DEFAULT": 1.0, "MIN": 0.0, "MAX": 10.0, "DESCRIPTION": "Amount of random RGB tint per tile" },
    { "LABEL": "Color Speed", "NAME": "fx_color_speed", "TYPE": "float", "DEFAULT": 0.01, "MIN": 0.0, "MAX": 0.1, "DESCRIPTION": "Speed of color animation" },
    { "LABEL": "Morph", "NAME": "fx_shape_morph", "TYPE": "float", "DEFAULT": 0.5, "MIN": 0.0, "MAX": 1.0, "DESCRIPTION": "Amount of tile shape morphing" },
    { "LABEL": "Displacement", "NAME": "fx_displacement", "TYPE": "float", "DEFAULT": 0.0, "MIN": 0.0, "MAX": 10.0, "DESCRIPTION": "Amount of tile displacement" }
  ],
  "GENERATORS": [
    { "NAME": "fx_color_time", "TYPE": "time_base", "PARAMS": {"speed": "fx_color_speed", "bpm_sync": false} }
  ]
}*/

float f_hash(vec2 p_coord) {
    return fract(sin(dot(p_coord, vec2(127.1, 311.7))) * 43758.5453123);
}

vec3 f_randomTint(vec2 p_tileId, float p_time) {
    float v_offset = p_time * 0.0005;
    float v_r = f_hash(p_tileId + vec2(v_offset, 0.0));
    float v_g = f_hash(p_tileId + vec2(v_offset + 1.0, 0.0));
    float v_b = f_hash(p_tileId + vec2(v_offset, 1.0));
    return vec3(v_r, v_g, v_b);
}

vec2 f_displaceTile(vec2 p_tileId, float p_amount, float p_time) {
    float v_offset = p_time * 0.0005;
    float v_randX = f_hash(p_tileId + vec2(v_offset + 5.0, 0.0)) * 2.0 - 1.0;
    float v_randY = f_hash(p_tileId + vec2(0.0, v_offset + 5.0)) * 2.0 - 1.0;
    return vec2(v_randX, v_randY) * p_amount;
}

vec2 f_morphTileShape(vec2 p_uv, vec2 p_tileId, float p_amount, float p_time) {
    float v_offset = p_time * 0.0005;
    float v_randX = f_hash(p_tileId + vec2(3.0, v_offset));
    float v_randY = f_hash(p_tileId + vec2(v_offset, 3.0));
    
    vec2 v_center = vec2(0.5);
    vec2 v_toCenter = p_uv - v_center;
    
    float v_distortion = sin(v_randX * 6.28318) * 0.3 + 0.5;
    float v_angle = v_randY * 6.28318;
    
    mat2 v_rotation = mat2(cos(v_angle), -sin(v_angle), sin(v_angle), cos(v_angle));
    vec2 v_rotated = v_rotation * v_toCenter;
    
    vec2 v_scale = vec2(1.0 + v_distortion * 0.5, 1.0 - v_distortion * 0.5);
    vec2 v_scaled = v_rotated * v_scale;
    
    vec2 v_morphed = v_scaled + v_center;
    
    return mix(p_uv, v_morphed, p_amount);
}

vec4 fxColorForPixel(vec2 mm_FragNormCoord) {
    vec2 v_tileId = floor(mm_FragNormCoord * vec2(float(fx_tiles_h), float(fx_tiles_v)));
    
    vec2 v_tileUV = fract(mm_FragNormCoord * vec2(float(fx_tiles_h), float(fx_tiles_v)));
    
    vec2 v_displacement = f_displaceTile(v_tileId, fx_displacement, fx_color_time);
    
    vec2 v_morphedUV = f_morphTileShape(v_tileUV, v_tileId, fx_shape_morph, fx_color_time);
    
    vec2 v_sampleCoord = (v_tileId + v_morphedUV + v_displacement) / vec2(float(fx_tiles_h), float(fx_tiles_v));
    
    vec4 v_color = FX_NORM_PIXEL(v_sampleCoord);
    
    vec3 v_tint = f_randomTint(v_tileId, fx_color_time);
    v_color.rgb = mix(v_color.rgb, v_color.rgb * v_tint, fx_rgb_amount);
    
    return v_color;
}