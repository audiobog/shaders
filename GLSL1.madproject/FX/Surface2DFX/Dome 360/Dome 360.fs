/*{
    "CREDIT": "kellyegan",
    "TAGS": ["utility"],
    "VSN": 1.0,
    "DESCRIPTION": "Remaps UVs to 360 dome projection",
    "MEDIA": {
        "REQUIRES_TEXTURE": false,
        "GL_TEXTURE_MIN_FILTER": "LINEAR",
        "GL_TEXTURE_MAG_FILTER": "LINEAR",
        "GL_TEXTURE_WRAP": "CLAMP_TO_EDGE",
    },
    "INPUTS": [
        { "LABEL": "Inner Radius", "NAME": "fx_in", "TYPE": "float", "DEFAULT": 0.1, "MIN": 0.0, "MAX": 1. },
		{ "LABEL": "Outer Radius", "NAME": "fx_out", "TYPE": "float", "DEFAULT": 0.5, "MIN": 0.5, "MAX": 2.0 },
    ],

}*/
// https://www.shadertoy.com/view/4ttXRH

#define FXPI 3.14159265359

vec4 fxColorForPixel(vec2 mm_FragNormCoord)
{
	vec2 uv = mm_FragNormCoord * 2. -1.;

    float theta = atan( uv.y, uv.x );
    float dist = length( uv );

   	float a = smoothstep( fx_in - 0.001, fx_in + 0.001, dist );
    a *= 1.0 - smoothstep( fx_out - 0.001, fx_out + 0.001, dist );
    
    uv = vec2((theta + FXPI) / (FXPI * 2.0),(dist - fx_in) / (fx_out - fx_in));

    return FX_NORM_PIXEL(uv);
}