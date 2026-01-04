/*{
    "CREDIT": "Matt Beghin - MM team",
    "DESCRIPTION": "Rutt Etra FX using instancing",
    "VSN": "1.0",
    "MEDIA": {
        "REQUIRES_TEXTURE": true,
    },
    "INPUTS": [
        {
            "LABEL": "Line Count",
            "NAME": "fx_count",
            "TYPE": "int",
            "MIN": 16,
            "MAX": 128,
            "DEFAULT": 100
        },
        {
            "LABEL": "Precision",
            "NAME": "fx_precision",
            "TYPE": "int",
            "MIN": 64,
            "MAX": 512,
            "DEFAULT": 256
        },
        {
            "LABEL": "Thickness",
            "NAME": "fx_thickness",
            "TYPE": "float",
            "MIN": 0,
            "MAX": 1,
            "DEFAULT": 0.2
        },
        {
            "LABEL": "Depth",
            "NAME": "fx_depth",
            "TYPE": "float",
            "MIN": 0,
            "MAX": 1,
            "DEFAULT": 0.2
        },
        {
            "LABEL": "Rotate",
            "NAME": "fx_rotate",
            "TYPE": "point2D",
            "MIN": [-90,-90],
            "MAX": [90,90],
            "DEFAULT": [0,0]
        },
        {
            "LABEL": "Zoom",
            "NAME": "fx_zoom",
            "TYPE": "float",
            "MIN": 0,
            "MAX": 4,
            "DEFAULT": 1
        },
        {
            "LABEL": "Origin",
            "NAME": "fx_origin",
            "TYPE": "point2D",
            "MIN": [-1,-1],
            "MAX": [1,1],
            "DEFAULT": [0,0]
        },
        {
            "LABEL": "Restrict to Surface",
            "NAME": "fx_restrict",
            "TYPE": "bool",
            "DEFAULT": true,
            "FLAGS": "button"
        }
    ],
    "GENERATORS": [
        {"NAME": "fx_instance_count", "TYPE": "multiplier", "PARAMS": {"value1": "fx_count", "value2": "fx_precision"}},
    ],
    "RENDER_SETTINGS": {
       "INSTANCE_COUNT": "fx_instance_count"
    }
}*/

#include "MadNoise.glsl"
#include "MadCommon.glsl"

out vec2 posInQuad;

mat4 CreatePerspectiveMatrix(in float fov, in float aspect, in float near, in float far)
{
    mat4 m = mat4(0.0);
    float angle = (fov / 180.0) * PI;
    float f = 1. / tan( angle * 0.5 );
    m[0][0] = f / aspect;
    m[1][1] = f;
    m[2][2] = (far + near) / (near - far);
    m[2][3] = -1.;
    m[3][2] = (2. * far*near) / (near - far);
    return m;
}

mat4 CamControl( vec3 eye, float pitch)
{
    float cosPitch = cos(pitch);
    float sinPitch = sin(pitch);
    vec3 xaxis = vec3( 1, 0, 0. );
    vec3 yaxis = vec3( 0., cosPitch, sinPitch );
    vec3 zaxis = vec3( 0., -sinPitch, cosPitch );

    // Create a 4x4 view matrix from the right, up, forward and eye position vectors
    mat4 viewMatrix = mat4(
        vec4(       xaxis.x,            yaxis.x,            zaxis.x,      0 ),
        vec4(       xaxis.y,            yaxis.y,            zaxis.y,      0 ),
        vec4(       xaxis.z,            yaxis.z,            zaxis.z,      0 ),
        vec4( -dot( xaxis, eye ), -dot( yaxis, eye ), -dot( zaxis, eye ), 1 )
    );
    return viewMatrix;
}

mat3 rotateAroundX( in float angle )
{
  float s = sin(angle);
  float c = cos(angle);
  return mat3(1.0,0.0,0.0,
              0.0,  c, -s,
              0.0,  s,  c);
}

mat3 rotateAroundY( in float angle )
{
  float s = sin(angle);
  float c = cos(angle);
  return mat3(  c,0.0,  s,
              0.0,1.0,0.0,
               -s,0.0,  c);
}


void fxVsFunc()
{
	vec4 posInSurface = vec4(mm_Vertex.xy,0,1);

	if (gl_InstanceID%2 != 0) {
		gl_Position = vec4(0);
		//return;
	}

	int lineInstanceId = gl_InstanceID/fx_precision;
	int hPartId = gl_InstanceID%fx_precision;

	float instanceLeft = -1+2*float(hPartId)/fx_precision;
	float instanceRight = -1+2*float(hPartId+1)/fx_precision;
	posInSurface.x = mix(instanceLeft,instanceRight,(1+posInSurface.x)/2);

	float instanceTop = -1+2*float(lineInstanceId) / fx_count;
	float instanceBottom = -1+2*float(lineInstanceId + fx_thickness) / fx_count;
	posInSurface.y = mix(instanceTop,instanceBottom,(1+posInSurface.y)/2);

	// Initialize Surface 2D fragment shader inputs
	mm_FragNormCoord = (mm_TextureMatrix*vec4(0.5+0.5*posInSurface.xy,0,1)).xy;

	vec2 uvAtLineTop = (mm_TextureMatrix*vec4(0.5+0.5*vec2(posInSurface.x,instanceTop),0,1)).xy;

    vec4 mediaColor = IMG_NORM_PIXEL(inputImage,uvAtLineTop);
	float mediaLuma = dot(mediaColor.rgb, vec3(0.299, 0.587, 0.114)) * mediaColor.a;

	//posInSurface.xy += (posInSurface.xy-fx_origin) * (mediaLuma-0.5)*fx_depth;
	
	vec3 depthDir = vec3(-fx_origin,1);
	posInSurface.xyz += depthDir*(mediaLuma-0.5)*fx_depth;

	vec3 eye = vec3(0,0,-1);
    mat4 projmat = CreatePerspectiveMatrix(90., 1, -0.1, 10.);
    mat4 viewmat = CamControl(eye, 0);
    mat4 vpmat = viewmat * projmat;

	mat3 rotMatrixX = rotateAroundX(-fx_rotate.y*PI/180);
	mat3 rotMatrixY = rotateAroundY(-fx_rotate.x*PI/180);
	posInSurface.xyz *= rotMatrixX * rotMatrixY;

	posInSurface.z += -1+eye.z; //mix(eye.z,-1+eye.z,max(abs(fx_rotate.x),abs(fx_rotate.y))/90);

	posInSurface = vpmat * posInSurface; 
	posInSurface.xy /= posInSurface.w;
	posInSurface.z = 1;
	posInSurface.w = 1;
	posInSurface.xy *= 2*fx_zoom;

	posInQuad = posInSurface.xy;

	// Tells OpenGL where this vertex should be on the output view
	gl_Position = mm_ModelViewProjectionMatrix * posInSurface;
}

