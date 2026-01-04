/*{
    "CREDIT": "1024 architecture",
    "DESCRIPTION": "Vignette",
    "VSN": "1.1",
    "TAGS": "filmic",
    "INPUTS": [
	{ "LABEL": "Amount", "NAME": "fx_amount", "TYPE": "float", "MIN": 0., "MAX": 1.0, "DEFAULT": 1. }, 
	{ "LABEL": "Size", "NAME": "fx_size", "TYPE": "float", "MIN": 0., "MAX": 1.0, "DEFAULT": 0.5 }, 
	{ "LABEL": "Anamorphic", "NAME": "fx_anamorphic", "TYPE": "float", "MIN": -1., "MAX": 1.0, "DEFAULT": 0.0 }, 
    ],
}*/

vec4 fxColorForPixel(vec2 mm_FragNormCoord)
{
	vec4 col = FX_NORM_PIXEL(mm_FragNormCoord); 

	vec2 uv = mm_SurfaceCoord.xy*2.-1.;
	uv.x *= 1. - max(-fx_anamorphic,0.);
	uv.y *= 1. - max(fx_anamorphic,0.);

	float l = max(0.,1. -length(uv*fx_size));
	l = smoothstep(0.,1.,l);

	col.rgb *= mix(1.,l,fx_amount);
    return col;
}