// mix of https://www.shadertoy.com/view/4dXBW2 by Coolok
// and
// https://www.shadertoy.com/view/MtXBDs by airtight

/*{
    "CREDIT": "Coolok + airtight",
    "TAGS": ["graphics"],
    "VSN": 1.1,
    "DESCRIPTION": "Yet another glitch effect",
    "MEDIA": {
        "REQUIRES_TEXTURE": true,
    },
    "INPUTS": [
{ "LABEL": "Amount", "NAME": "fx_amount", "TYPE": "float", "DEFAULT": 1., "MIN": 0.0, "MAX": 2.0 },
{ "LABEL": "Speed", "NAME": "fx_speed", "TYPE": "float", "DEFAULT": 1., "MIN": 0.0, "MAX": 1.0 },
    ],
    "GENERATORS": [
        {"NAME": "fx_time", "TYPE": "time_base", "PARAMS": {"speed": "fx_speed", "speed_curve": 2, "link_speed_to_global_bpm":true}},
    ]
}*/

#include "MadNoise.glsl"

float sat( float t ) {
	return clamp( t, 0.0, 1.0 );
}

vec2 sat( vec2 t ) {
	return clamp( t, 0.0, 1.0 );
}

//remaps inteval [a;b] to [0;1]
float remap  ( float t, float a, float b ) {
	return sat( (t - a) / (b - a) );
}

//note: /\ t=[0;0.5;1], y=[0;1;0]
float linterp( float t ) {
	return sat( 1.0 - abs( 2.0*t - 1.0 ) );
}

vec3 spectrum_offset( float t ) {
	vec3 ret;
	float lo = step(t,0.5);
	float hi = 1.0-lo;
	float w = linterp( remap( t, 1.0/6.0, 5.0/6.0 ) );
	float neg_w = 1.0-w;
	ret = vec3(lo,1.0,hi) * vec3(neg_w, w, neg_w);
	return pow( ret, vec3(1.0/2.2) );
}

//note: [0;1]
float rand( vec2 n ) {
  return fract(sin(dot(n.xy, vec2(12.9898, 78.233)))* 43758.5453);
}

//note: [-1;1]
float srand( vec2 n ) {
	return rand(n) * 2.0 - 1.0;
}

float mytrunc( float x, float num_levels )
{
	return floor(x*num_levels) / num_levels;
}
vec2 mytrunc( vec2 x, float num_levels )
{
	return floor(x*num_levels) / num_levels;
}

//2D (returns 0 - 1)
float random2d(vec2 n) { 
    return fract(sin(dot(n, vec2(12.9898, 4.1414))) * 43758.5453);
}

float randomRange (in vec2 seed, in float min, in float max) {
		return min + random2d(seed) * (max - min);
}

// return 1 if v inside 1d range
float insideRange(float v, float bottom, float top) {
   return step(bottom, v) - step(top, v);
}

//inputs
float AMT = 0.2; //0 - 1 glitch amount
float SPEED = 0.6; //0 - 1 speed

vec4 fxColorForPixel(vec2 mm_FragNormCoord)
{
	vec2 uv = mm_FragNormCoord;

	float time = mod(fx_time*100.0, 32.0)/110.0; // + modelmat[0].x + modelmat[0].z;

	float GLITCH = 0.1 * fx_amount;
	
	float gnm = sat( GLITCH );
	float rnd0 = rand( mytrunc( vec2(time, time), 6.0 ) );
	float r0 = sat((1.0-gnm)*0.7 + rnd0);
	float rnd1 = rand( vec2(mytrunc( uv.x, 10.0*r0 ), time) ); //horz
	//float r1 = 1.0f - sat( (1.0f-gnm)*0.5f + rnd1 );
	float r1 = 0.5 - 0.5 * gnm + rnd1;
	r1 = 1.0 - max( 0.0, ((r1<1.0) ? r1 : 0.9999999) ); //note: weird ass bug on old drivers
	float rnd2 = rand( vec2(mytrunc( uv.y, 40.0*r1 ), time) ); //vert
	float r2 = sat( rnd2 );

	float rnd3 = rand( vec2(mytrunc( uv.y, 10.0*r0 ), time) );
	float r3 = (1.0-sat(rnd3+0.8)) - 0.1;

	float pxrnd = rand( uv + time );

	float ofs = 0.5 * r2 * GLITCH * ( rnd0 > 0.5 ? 1.0 : -1.0 );
	ofs += 0.5 * pxrnd * ofs;

	uv.y += 0.5 * r3 * GLITCH;

    const int NUM_SAMPLES = 4;
    const float RCP_NUM_SAMPLES_F = 1.0 / float(NUM_SAMPLES);
    
	vec4 sum = vec4(0.0);
	vec3 wsum = vec3(0.0);
	for( int i=0; i<NUM_SAMPLES; ++i )
	{
		float t = float(i) * RCP_NUM_SAMPLES_F;
		uv.x = sat( uv.x + ofs * t );
		vec4 samplecol = FX_NORM_PIXEL( uv );
		vec3 s = spectrum_offset( t );
		samplecol.rgb = samplecol.rgb * s;
		sum += samplecol;
		wsum += s;
	}
	sum.rgb /= wsum;
	sum.a *= RCP_NUM_SAMPLES_F;

    vec3 outCol = FX_NORM_PIXEL( mm_FragNormCoord).rgb;
	vec3 ori = FX_NORM_PIXEL(mix(mm_FragNormCoord,uv,0.5)).rgb;
    
    //randomly offset slices horizontally
    float maxOffset = AMT/2.0*fx_amount;
    for (float i = 0.0; i <  AMT; i += 1.0) {
        float sliceY = random2d(vec2(time , 2345.0 + float(i)));
        float sliceH = random2d(vec2(time , 9035.0 + float(i))) * 0.25;
        float hOffset = randomRange(vec2(time , 9625.0 + float(i)), -maxOffset, maxOffset);
        vec2 uvOff = uv;
        uvOff.x += hOffset;
        if (insideRange(uv.y, sliceY, fract(sliceY+sliceH)) == 1.0 ){
        	outCol = FX_NORM_PIXEL( uvOff).rgb;
        }
    }
    
    //do slight offset on one entire channel
    float maxColOffset = AMT/6.0;
    float rnd = random2d(vec2(time , 9545.0));
    vec2 colOffset = vec2(randomRange(vec2(time , 9545.0),-maxColOffset,maxColOffset), 
                       randomRange(vec2(time , 7205.0),-maxColOffset,maxColOffset));
    if (rnd < 0.33){
        outCol.r =FX_NORM_PIXEL( uv + colOffset).r;
        
    }else if (rnd < 0.66){
        outCol.g = FX_NORM_PIXEL( uv + colOffset).g;
        
    } else{
        outCol.b = FX_NORM_PIXEL( uv + colOffset).b;  
    }
       
	float msk = vnoise(vec3(floor(mm_FragNormCoord*100.)*0.1,fx_time));
	msk = pow(msk,1.3);
	msk = step(0.5,msk);

    return vec4( mix(ori,outCol.rgb,msk*fx_amount)*0.5+ sum.rgb*0.5,1.);
}
