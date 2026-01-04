/*{
  "RESOURCE_TYPE": "Surface 2D FX For MadMapper",
  "CREDIT": "Made with MadAI",
  "DESCRIPTION": "Simple checker board pattern",
  "TAGS": "pattern,geometric",
  "VSN": "1.0",
  "INPUTS": [
    { "LABEL": "Size/Scale", "NAME": "fx_scale", "TYPE": "float", "DEFAULT": 20.0, "MIN": -100.0, "MAX": 100.0 }
  ]
}*/

vec4 fxColorForPixel(vec2 mm_FragNormCoord) {
    vec2 v_coord = mm_FragNormCoord * FX_IMG_SIZE() * fx_scale;
    float v_pattern = mod(floor(v_coord.x) + floor(v_coord.y), 2.0);
    return vec4(v_pattern, v_pattern, v_pattern, 1.0);
}