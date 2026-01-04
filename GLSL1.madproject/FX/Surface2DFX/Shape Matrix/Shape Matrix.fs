/*{
    "CREDIT": "frz / 1024 architecture",
    "TAGS": ["graphics"],
    "VSN": 1.0,
    "DESCRIPTION": "Old school dot printer matrix look, with different shapes options, and proper antialiasing",
    "INPUTS": [
{ "NAME": "fx_shape", "Label": "Shape", "TYPE": "long", "DEFAULT": "Dot", 
	"VALUES": ["Dot", "Rectangle", "Rounded Rectangle", "Triangle", "Hexagon"] },
{ "LABEL": "Scale", "NAME": "fx_scale", "TYPE": "int", "DEFAULT": 10, "MIN": 1, "MAX": 20 },
{ "LABEL": "Size", "NAME": "fx_size", "TYPE": "float", "DEFAULT": 1., "MIN": 0.0, "MAX": 4.0 },
{ "LABEL": "Smoothness", "NAME": "fx_smooth", "TYPE": "float", "DEFAULT": 0.1, "MIN": 0.0, "MAX": 1.0 },
    ],
}*/

#include "MadSDF.glsl"

vec4 fxColorForPixel(vec2 mm_FragNormCoord)
{
	vec2 uv = mm_FragNormCoord;
	vec2 ruv = uv;
	vec2 res = FX_IMG_SIZE();
	float ratio = res.x/res.y;

	ruv = ruv *2.-1.;

	float sc = float(fx_scale*2.);
	vec2 scale = vec2(sc,sc);
	ruv *= scale*vec2(0.5*ratio,0.5);
	ruv = fract(ruv);

	vec3 targetcol = FX_NORM_PIXEL(floor( uv * scale)/scale ).rgb;
	float luma = max(0.,dot(targetcol, vec3(0.3,0.6,0.1)) );

	float c =-1.;
	float s = (fx_size*0.1)*fx_scale;

	if(fx_shape == 0) c = circle(ruv,vec2(0.5),(1./sc*8.)*luma*s );
	if(fx_shape == 1) c = rectangle(ruv,vec2(0.5),vec2((1./sc*8.)*luma*s ));
	if(fx_shape == 2) c = roundedRectangle(ruv,vec2(0.5),vec2((1./sc*8.)*luma*s ), 0.25);
	if(fx_shape == 3) c = triangle(ruv,vec2(0.5),((1./sc*8.)*luma*s ));
	if(fx_shape == 4) c = hexagon(ruv,vec2(0.5),((1./sc*8.)*luma*s ));

	c += 0.1;
	c = 1. - smoothstep(0.,max(0.02,fx_smooth*0.3),c);

	vec3 col = vec3(1.)*c*targetcol;

	return vec4(col,1.);

    return FX_NORM_PIXEL(uv);
}
