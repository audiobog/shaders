// original by NickWest https://www.shadertoy.com/view/lslGRr

/*{
    "CREDIT": "NickWest",
    "TAGS": ["graphics"],
    "VSN": "1.1",
    "DESCRIPTION": "Simple 4-tap Sharpen filter",
    "MEDIA": {
        "REQUIRES_TEXTURE": true,
    },
    "INPUTS": [
        { "LABEL": "Sharpness", "NAME": "fx_sharp", "TYPE": "float", "DEFAULT": 1., "MIN": 0.0, "MAX": 10.0 },
    ],

}*/

vec4 fxColorForPixel(vec2 mm_FragNormCoord)
{
	vec2 uv = mm_FragNormCoord;
	vec2 res = FX_IMG_SIZE();
    
	vec2 step = 1.0 / res;
	
	vec3 texA = FX_NORM_PIXEL( uv + vec2(-step.x, -step.y) * 1.5 ).rgb;
	vec3 texB = FX_NORM_PIXEL( uv + vec2( step.x, -step.y) * 1.5 ).rgb;
	vec3 texC = FX_NORM_PIXEL( uv + vec2(-step.x,  step.y) * 1.5 ).rgb;
	vec3 texD = FX_NORM_PIXEL( uv + vec2( step.x,  step.y) * 1.5 ).rgb;
   
    vec3 around = 0.25 * (texA + texB + texC + texD);
	vec4 center  = FX_NORM_PIXEL( uv );
	
	float sharpness = fx_sharp;
  
	vec3 col = center.rgb + (center.rgb - around) * sharpness;
    return vec4(col,center.a);
}