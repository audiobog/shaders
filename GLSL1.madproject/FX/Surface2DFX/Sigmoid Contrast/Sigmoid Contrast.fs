// from Loadus https://www.shadertoy.com/view/MlXGRf
/*{
    "CREDIT": "Loadus",
    "TAGS": ["filmic"],
    "VSN": 1.0,
    "DESCRIPTION": "Sigmoid Contrast filter",
    "MEDIA": {
        "REQUIRES_TEXTURE": false,
        "GL_TEXTURE_MIN_FILTER": "LINEAR",
        "GL_TEXTURE_MAG_FILTER": "LINEAR",
        "GL_TEXTURE_WRAP": "CLAMP_TO_EDGE",
    },
    "INPUTS": [
{ "LABEL": "Amount", "NAME": "fx_amount", "TYPE": "float", "DEFAULT": 1., "MIN": 0.0, "MAX": 4.0 },
{ "LABEL": "Correction", "NAME": "fx_correction", "TYPE": "float", "DEFAULT": 1., "MIN": 0.0, "MAX": 4.0 },
    ],

}*/

vec3 SCurve (vec3 value, float amount, float correction) {

	vec3 curve = vec3(1.0); 

    if (length(value) < 0.5)
    {
        curve = pow(value, vec3(amount)) * pow(2.0, amount) * 0.5; 
    }       
    else
    { 	
    	curve = 1.0 - pow(vec3(1.0) - value, vec3(amount)) * pow(2.0, amount) * 0.5; 
    }

    return pow(curve, vec3(correction));
}

vec4 fxColorForPixel(vec2 mm_FragNormCoord)
{
	vec2 uv = mm_FragNormCoord.xy;
	vec4 col = FX_NORM_PIXEL(uv);
	col.rgb = SCurve(col.rgb, fx_amount,fx_correction); 
    return col;
}