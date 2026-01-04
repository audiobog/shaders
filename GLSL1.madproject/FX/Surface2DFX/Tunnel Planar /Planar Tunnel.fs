/*{
    "CREDIT": "frz / 1024 architecture",
    "TAGS": ["graphics"],
    "VSN": 1.0,
    "DESCRIPTION": "Old school TRON like tunnel",
    "INPUTS": [
{ "LABEL": "Speed", "NAME": "fx_speed", "TYPE": "float", "DEFAULT": 1., "MIN": -4., "MAX": 4.0 },
{ "LABEL": "Scale", "NAME": "fx_scale", "TYPE": "float", "DEFAULT": 1., "MIN": 0.1, "MAX": 2.0 },
{ "LABEL": "Rotation", "NAME": "fx_rot", "TYPE": "float", "DEFAULT": 0., "MIN": 0.0, "MAX": 1.0 },
    ],
    "GENERATORS": [
{"NAME": "fx_time", "TYPE": "time_base", "PARAMS": {"speed": "fx_speed", "speed_curve": 2, "link_speed_to_global_bpm":true}},
    ]
}*/

// TODO: should enable mipmapping

vec4 fxColorForPixel(vec2 mm_FragNormCoord)
{
	vec2 uv = mm_FragNormCoord;
	uv = uv *2. -1.;
	uv *= 1./fx_scale;

	float ro = fx_rot * 2. * 3.14156;
	mat2 r = mat2(cos(ro),-sin(ro),sin(ro),cos(ro));
	uv *= r;
	
	uv =  vec2( uv.x, 1.0f ) / abs( uv.y ) + vec2( 0, fx_time );
	uv = fract(uv);
	
	vec2 ruv = (mm_FragNormCoord*2.-1.) * r;
	float fog =  abs(ruv.y);

    return FX_NORM_PIXEL(uv)*vec4(vec3(fog),1.);
}