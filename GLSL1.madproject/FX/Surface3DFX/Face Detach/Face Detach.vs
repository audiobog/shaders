/*
    Available uniforms

        // mm_Vertex.xy comes in (-1,-1)-(1,1), mm_ModelViewProjectionMatrix is a combination of the surface
        // perspective transform and the view transform
        uniform mat4 mm_ModelViewProjectionMatrix;

        // Model View matrix, independant of the projection, used to compute normals & lighting
        uniform mat4 mm_ModelViewMatrix;

        // Normal matrix (normal matrix of mm_ModelViewMatrix)
        uniform mat3 mm_NormalMatrix;

        // Light matrix (computed as translate(0.5, 0.5, 0.5) * scale(0.5,0.5,0.5) * projectionMatrix * modelViewMatrix)
        uniform mat4 mm_LightMatrix;

        // mm_TexCoord0.xy is the texture coordinate relative to the input rectangle bounding rectangle (0,0)-(1,1),
        // this matrix transforms from coordinates in input rectangle bounding rectangle to coordinates in the media
        // (it handles scaling and rotation of the uv rectangle)
        uniform mat4 mm_TextureMatrix;

        // Light Psosition
        uniform vec3 mm_LightPosition;

        // Light Color
        uniform vec4 mm_LightColor;

        // Light attenuation (.x=constant, .y=linear, .z=quadratic)
        uniform vec3 mm_LightAttenuation;

        // True if Spot Light has been enabled
        uniform bool mm_HasShadowMap;

        // Spot direction
        uniform vec3 mm_LightSpotDirection;

        // Spot Exponent
        uniform float mm_LightSpotExponent;

        // Spot Cutoff
        uniform float mm_LightSpotCutoff;

        // Texturing mode:
        //  0 = UVs from 3D Object file
        //  1 = Triplanar mapping
        //  2 = Planar mapping
        //  3 = Spherical mapping
        uniform int mm_TexturingMode;

        // Texture Wrapping Mode (this is handled at shader level so it handles rectangle texture (for Syphon & AVFoundation movies / cameras):
        //  0 = Clamp To Edge (default)
        //  1 = Clamp To Border (border is set to black)
        //  2 = Repeat
        uniform int mm_TextureWrappingMode;

        // Texturing rotation X/Y/Z (ignored if texturing mode == 0 / UVs from 3D Object file)
        uniform vec3 mm_TexturingRotation;
        
        // Texturing smoothness (ignored if texturing mode == 0 / UVs from 3D Object file)
        uniform float mm_TexturingSmoothness;

        // Uv Matrix (depending on surface rectangle in Input / UV Preview)
        uniform mat4 mm_UvRectMatrix;

        // mm_ModulationColor contains red, green, blue levels & surface opacity in alpha
        // The multiplication in the fragment shader is done by MadMapper, so you shouldn't need that
        uniform vec4 mm_ModulationColor;

        // Wireframe Color
        uniform vec4 mm_WireframeColor;

        // Ambient Color
        uniform vec4 mm_AmbientColor;

        // Blend Mode, see possible values in Definitions below. This is handled by default by MadMapper so you should need it
        uniform int mm_BlendMode;

        // Used to know if we're rendering to the preview or to an output fullscreen / desktop window / syphon-spout-NDI
        uniform bool mm_IsRenderingPreview;

        // The index of the surface in the project (not recommended to use that)
        uniform int mm_SurfaceIndex;

        // Shadow map uniforms, shouldn't be used
        uniform sampler2D mm_LightShadowMap;    // Shadow map sampler
        uniform float mm_LightShadowMapSize;    // = 32 * 2^"Quality" (set in surface parameters)

    All Vertex Shader outputs should be initialized:

        out vec2 mm_FragNormCoord;          // Texture coordinate for vertex
        out vec3 mm_vNormal;                // = mm_NormalMatrix * mm_Normal
        out vec4 mm_vNormalForTexturing;    // = mm_Normal, might be altered by texturing rotation
        out vec4 mm_vVertex;                // = mm_Vertex
        out vec4 mm_VertexToLightPosition;  // = mm_LightMatrix * mm_Vertex
        out vec2 mm_SurfaceCoord;           // = mm_TexCoord0
        out vec3 mm_LightDir;               // Direction from vertex to light, = vec3(mm_ModelViewMatrix * (vec4(mm_LightPosition,1) - mm_Vertex))
        out vec3 mm_LightSpotDir;           // = normalize(mm_NormalMatrix * mm_LightSpotDirection)
        out float mm_Attenuation;           // Light attenuation computed by distance from vertex to light

    Definitions & Macros:

        // Blend modes
        #define BlendModeIgnoreAlpha 0
        #define BlendModeAdd 1
        #define BlendModeOver 2
        #define BlendModeOverPremultiplied 3
        #define BlendModeMultiply 4
        #define BlendModeSubtract 5
        #define BlendModeStencilLuma 6
        #define BlendModeSilhouetteLuma 7
        #define BlendModeSilhouetteAlpha 8

        vec4 FX_NORM_PIXEL(vec2 uv) => returns the color of the pixel at normalized position uv (0,0-1,1),
                                        whether it's a texture or a material & handling texturing mode

        vec2 FX_IMG_SIZE() => returns the dimensions of the input media or 1024x1024 if a Material  is being used

        #define IS_MATERIAL => defined only if a Material is being used as input media (the FX & the Material shaders are combined into a single Shader Program)

*/

/*{
    "CREDIT": "frz / 1024 architecture",
    "DESCRIPTION": "Move faces along their normal",
    "TAGS": "geometry",
    "VSN": "1.0",
    "INPUTS": [ 
	{"LABEL": "Distance", "NAME": "fx_distance", "TYPE": "float", "MIN": 0.0, "MAX": 2.0, "DEFAULT": 0.25},
	{ "NAME": "fx_noise_type", "Label": "Noise/Type", "TYPE": "long", "DEFAULT": "Flow", "FLAGS": "generate_as_define", 
	"VALUES": [ "Billowed", "Flow", "Hash"] },
	{"LABEL": "Noise/Amount", "NAME": "fx_noise", "TYPE": "float", "MIN": 0.0, "MAX": 2.0, "DEFAULT": 0.0 }, 
	{"LABEL": "Noise/Speed", "NAME": "fx_noise_speed", "TYPE": "float", "MIN": 0.0, "MAX": 4.0, "DEFAULT": 1. }, 
    ],
	"GENERATORS": [
    {"NAME": "fx_time", "TYPE": "time_base", "PARAMS": {"speed": "fx_noise_speed"} },
    ],
}*/

#include "MadNoise.glsl"

mat4 fx_rotationX( in float angle ) {
	float cosFactor = cos(angle);
	float sinFactor = sin(angle);
    return mat4(1.0,         0,          0, 0,
                  0, cosFactor, -sinFactor, 0,
                  0, sinFactor,  cosFactor, 0,
                  0,         0,          0, 1);
}

mat4 fx_rotationY( in float angle ) {
	float cosFactor = cos(angle);
	float sinFactor = sin(angle);
    return mat4( cosFactor, 0, sinFactor, 0,
                         0, 1,         0, 0,
                -sinFactor, 0, cosFactor, 0,
                         0, 0,         0, 1);
}

mat4 fx_rotationZ( in float angle ) {
	float cosFactor = cos(angle);
	float sinFactor = sin(angle);
    return mat4( cosFactor, -sinFactor, 0, 0,
                 sinFactor,  cosFactor, 0, 0,
                         0,          0, 1, 0,
                         0,          0, 0, 1);
}

float fx_fd_hash13(vec3 p3) {
    p3  = fract(p3 * 1031.1031);
    p3 += dot(p3, p3.yzx + 19.19);
    return fract((p3.x + p3.y) * p3.z);
}

void fxVsFunc()
{
    mm_vNormal = mm_NormalMatrix * mm_Normal;
    mm_vNormalForTexturing =  mm_Normal;
    mm_vVertex = mm_Vertex;
    mm_SurfaceCoord = mm_TexCoord0;

    // Init lighting outs
    vec3 vVertex = vec3(mm_ModelViewMatrix * mm_Vertex);
    vec3 lightPos = vec3(mm_ModelViewMatrix * vec4(mm_LightPosition,1));
    mm_LightSpotDir = normalize(mm_NormalMatrix * mm_LightSpotDirection);
    mm_LightDir = lightPos - vVertex;
    float d = length(mm_LightDir);
    mm_Attenuation = 1.0 / (mm_LightAttenuation.x + 
                     (mm_LightAttenuation.y*d) + 
                     (mm_LightAttenuation.z*d*d) );
    mm_VertexToLightPosition = mm_LightMatrix * mm_Vertex;

    // Handle Texturing Mode
    if (mm_TexturingMode != 0 && length(mm_TexturingRotation) > 0) {
        mat4 rotMatrix = mat4(1.0);
        if (mm_TexturingRotation.x>0) rotMatrix *= fx_rotationX(mm_TexturingRotation.x * 6.283185307179586/360);
        if (mm_TexturingRotation.y>0) rotMatrix *= fx_rotationY(mm_TexturingRotation.y * 6.283185307179586/360);
        if (mm_TexturingRotation.z>0) rotMatrix *= fx_rotationZ(mm_TexturingRotation.z * 6.283185307179586/360);
        mm_vVertex *= rotMatrix;
        mm_vNormalForTexturing = (vec4(mm_vNormalForTexturing,1) * rotMatrix).xyz;
    }

	// mm_FragNormCoord is the real uv position to be used (take care of surface input rectangle)
    mm_FragNormCoord = (mm_TextureMatrix * vec4(mm_TexCoord0,0,1)).xy;

	float n=0.;

	vec3 no = mm_NormalMatrix * mm_Normal;
    #if defined( fx_noise_type_IS_Billowed )
         n = worleyNoise( vec3(no.xy,fx_time) );
    #elif defined( fx_noise_type_IS_Flow )
         n = flowNoise( no.xy,fx_time )*0.5+0.5;
    #elif defined( fx_noise_type_IS_Hash )
         n = fx_fd_hash13( vec3(no.xy,floor(fx_time*60.+1.345))*0.1 );
	#endif

	n = mix(1.,n,fx_noise);

	vec3 v = mm_Vertex.xyz+no*fx_distance*n;

	// vec3 vVertex = vec3(mm_ModelViewMatrix * vec4(v,mm_Vertex.w));
	// vec3 lightPos = vec3(mm_ModelViewMatrix * vec4(mm_LightPosition,1));
	// mm_LightDir = lightPos - vVertex;
	mm_VertexToLightPosition = mm_LightMatrix * vec4(v,mm_Vertex.w);

	// Tells OpenGL where this vertex should be on the output view
	gl_Position = mm_ModelViewProjectionMatrix * vec4(v,mm_Vertex.w);
}
