// Created by inigo quilez - iq/2013
// License Creative Commons Attribution-NonCommercial-ShareAlike 3.0 Unported License.
// https://www.shadertoy.com/view/Ms2SWW
// modded by frz to fit madmapper and add a few external controls to the original shader.

/*{
    "CREDIT": "iq, adapted by frz",
    "TAGS": ["graphics"],
    "VSN": 1.0,
    "DESCRIPTION": "Transforms the input source into a neverending tunnel",
    "INPUTS": [
{ "LABEL": "Speed", "NAME": "fx_speed", "TYPE": "float", "DEFAULT": 1., "MIN": -4.0, "MAX": 4.0 },
{ "LABEL": "Angle", "NAME": "fx_angle", "TYPE": "float", "DEFAULT": 0., "MIN": 0.0, "MAX": 1.0 },
{ "LABEL": "Repeat", "NAME": "fx_repeat", "TYPE": "float", "DEFAULT": 1., "MIN": 0.0, "MAX": 10.0 },
{ "LABEL": "Scale", "NAME": "fx_scale", "TYPE": "float", "DEFAULT": 0.2, "MIN": 0.0, "MAX": 1.0 },
{ "LABEL": "Cylinder", "NAME": "fx_cyl", "TYPE": "float", "DEFAULT": 0., "MIN": 0.0, "MAX": 1.0 },
    ],
    "GENERATORS": [
        {"NAME": "fx_time", "TYPE": "time_base", "PARAMS": {"speed": "fx_speed", "speed_curve": 2, "link_speed_to_global_bpm":true}},
    ]
}*/

vec4 fxColorForPixel(vec2 mm_FragNormCoord)
{
	vec2 p = mm_FragNormCoord *2. -1.;
    float a = atan(p.y,p.x);
	a += 3.14156*2*fx_angle;
    float cr = length(p);
    float sr = pow( pow(p.x*p.x,4.0) + pow(p.y*p.y,4.0), 1.0/8.0 );
	
	float r = mix(sr,cr,fx_cyl);
    vec2 uv = vec2( fx_scale/r + 0.3*fx_time, a/(3.1415927*(1./fx_repeat)) );
	uv = fract(uv);

    return FX_NORM_PIXEL(abs(uv))*vec4(vec3(r),1.);
}
