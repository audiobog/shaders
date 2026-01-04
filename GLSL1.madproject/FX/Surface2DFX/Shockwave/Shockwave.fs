// original by niu2x https://www.shadertoy.com/view/ssBXzK

/*{
    "CREDIT": "niu2x",
    "TAGS": ["graphics"],
    "VSN": "1.1",
    "DESCRIPTION": "Generates a triggerable shockwave deformation over the source input",
    "INPUTS": [
{ "LABEL": "GO", 	"NAME": "fx_restart", "TYPE": "event","DEFAULT": false},
{ "LABEL": "Auto", 	"NAME": "fx_auto", "TYPE": "bool","DEFAULT": false,"FLAGS": "button"},
{ "LABEL": "Speed", "NAME": "fx_speed", "TYPE": "float", "DEFAULT": 1., "MIN": 0.0, "MAX": 2.0 },
{ "LABEL": "Spread","NAME": "fx_spread", "TYPE": "float", "DEFAULT": 0.2, "MIN": 0.0, "MAX": 1.0 },
{ "LABEL": "Amplitude", "NAME": "fx_amp", "TYPE": "float", "DEFAULT": 0.2, "MIN": 0.0, "MAX": 2.0 },
{ "LABEL": "Center","NAME": "fx_center", "TYPE": "point2D", "MAX": [ 1.0, 1.0 ], "MIN": [ 0.0, 0.0 ], "DEFAULT": [ 0.5, 0.5 ] },
{ "LABEL": "Light", "NAME": "fx_light", "TYPE": "float", "DEFAULT": 0.5, "MIN": 0.0, "MAX": 1.0 },
{ "LABEL": "Light Color", "NAME": "fx_color", "TYPE": "color", "DEFAULT": [ 1.0, 1.0, 1.0, 1.0 ], "FLAGS": "no_alpha" }, 
    ],
    "GENERATORS": [
        {"NAME": "fx_time", "TYPE": "time_base", "PARAMS": {"speed": "fx_speed", "speed_curve": 2, "link_speed_to_global_bpm":true, "reset": "fx_restart"}},
   		{"NAME": "fx_goCount", "TYPE": "incrementer", "PARAMS": {"increment": "fx_restart"} }
    ]
}*/

vec4 fxColorForPixel(vec2 mm_FragNormCoord)
{
	vec2 res = FX_IMG_SIZE();
	float ratio = res.x/res.y;
	vec2 uv = mm_FragNormCoord;
	vec2 dir = (uv - vec2(fx_center.x, 1.-fx_center.y));
	dir.x *= ratio;
	float d = length(dir);

	float time = fx_time;
	float goCount = fx_goCount;
	vec2 delta = vec2(0.);

	if (fx_auto) time = mod(fx_time, 2.0);
	if (time < 2 && goCount>0) {

    float pos = mod(time, 2.0);
    float deform = smoothstep(pos-fx_spread, pos+fx_spread, d);
    delta = normalize(dir) * sin(deform*3.1415926) * (deform-0.5);
	delta *= fx_amp;

	}

	vec4 col = FX_NORM_PIXEL(uv + delta);
	col.rgb += vec3(length(delta))*30.*fx_light*col.rgb*fx_color.rgb;

    return col;
}