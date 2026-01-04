/*
	Sobel Edge Detection.
	http://en.wikipedia.org/wiki/Sobel_operator

	https://www.shadertoy.com/view/Xdf3Rf  by ForceFlow
	https://www.shadertoy.com/view/Mdf3zr  by jmk
*/

/*{
    "CREDIT": "frz / 1024 architecture",
    "CATEGORIES": ["graphic"],
    "VSN": 1.1,
    "MEDIA": {
        "REQUIRES_TEXTURE": true,
    },
    "INPUTS": [
	{ "LABEL": "Edge Width", "NAME": "fx_w", "TYPE": "float", "DEFAULT": 5.0, "MIN": 0.0, "MAX": 10.0 },

	{ "LABEL": "Edge Color", "NAME": "fx_color", "TYPE": "color", "DEFAULT": [ 1.0, 0.0, 0.0, 1.0 ], "FLAGS": "no_alpha" },
	{ "LABEL": "Original", "NAME": "fx_origin", "TYPE": "float", "DEFAULT": 0.5, "MIN": 0.0, "MAX": 1.0 },
    ],
}*/

float lookup(vec2 p, float dx, float dy, vec2 texel)
{
    vec2 uv = (p.xy + vec2(dx,dy)*texel) ;
    vec4 c = FX_NORM_PIXEL(uv);
	return 0.2126*c.r + 0.7152*c.g + 0.0722*c.b;
}

vec4 fxColorForPixel(vec2 mm_FragNormCoord)
{
	vec2 p = mm_FragNormCoord;
	vec2 texel = mm_FragNormCoord / FX_IMG_SIZE();
	texel *= fx_w;

    float gx, gy = 0.0;

    gx += -1.0 * lookup(p, -1.0, -1.0, texel);
    gx += -2.0 * lookup(p, -1.0,  0.0, texel);
    gx += -1.0 * lookup(p, -1.0,  1.0, texel);
    gx +=  1.0 * lookup(p,  1.0, -1.0, texel);
    gx +=  2.0 * lookup(p,  1.0,  0.0, texel);
    gx +=  1.0 * lookup(p,  1.0,  1.0, texel);
    
    gy += -1.0 * lookup(p, -1.0, -1.0, texel);
    gy += -2.0 * lookup(p,  0.0, -1.0, texel);
    gy += -1.0 * lookup(p,  1.0, -1.0, texel);
    gy +=  1.0 * lookup(p, -1.0,  1.0, texel);
    gy +=  2.0 * lookup(p,  0.0,  1.0, texel);
    gy +=  1.0 * lookup(p,  1.0,  1.0, texel);
    
	// hack: use g^2 to conceal noise in the video
    float g = gx*gx + gy*gy;
    
    vec4 col = FX_NORM_PIXEL(mm_FragNormCoord);
	col.rgb = mix(col.rgb*fx_origin, fx_color.rgb, g);
   
    return col;
}