/*{
    "CREDIT": "frz / 1024 architecture",
    "CATEGORIES": ["graphic"],
    "VSN": "1.1",
    "INPUTS": [

	{ "LABEL": "Speed", "NAME": "fx_speed", "TYPE": "float", "DEFAULT": 1.0, "MIN": 0.0, "MAX": 10.0 },
	{ "LABEL": "Slices X", "NAME": "fx_x", "TYPE": "float", "DEFAULT": 10.0, "MIN": 1.0, "MAX": 100.0 },
	{ "LABEL": "Slices Y", "NAME": "fx_y", "TYPE": "float", "DEFAULT": 1.0, "MIN": 1.0, "MAX": 100.0 },

	{ "LABEL": "Amplitude X", "NAME": "fx_ampx", "TYPE": "float", "DEFAULT": 0.2, "MIN": 0.0, "MAX": 1.0 },
	{ "LABEL": "Amplitude Y", "NAME": "fx_ampy", "TYPE": "float", "DEFAULT": 0.0, "MIN": 0.0, "MAX": 1.0 },
    ],
    "GENERATORS": [

	{ "NAME": "fx_time", "TYPE": "time_base", "PARAMS": {"speed": "fx_speed", } }

    ]
}*/

#include "MadNoise.glsl"

vec4 fxColorForPixel(vec2 mm_FragNormCoord)
{
	vec2 uv = mm_FragNormCoord;

	float X = fx_x;
	float Y = fx_y;

	uv *= vec2(X,Y);

	float n_x = vnoise(vec3(floor(uv),fx_time))-0.5;
	float n_y = vnoise(vec3(floor(uv+2.),fx_time+ 0.345))-0.5;

	uv.x += n_y*fx_ampy;
	uv.y += n_x*fx_ampx;
	uv /= vec2(X,Y);
	
    return FX_NORM_PIXEL(uv);
}
