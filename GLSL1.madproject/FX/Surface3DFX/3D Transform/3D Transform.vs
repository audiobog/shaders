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
  "CREDIT": "mad-matt / MM team",
  "DESCRIPTION": "3D Transform",
  "TAGS": "geometry",
  "VSN": "1.0",
  "INPUTS": [
    { "LABEL": "Global/Translate X", "NAME": "fx_base_transition_x", "TYPE": "float", "DEFAULT": 0.0, "MIN": -5.0, "MAX": 5.0 },
    { "LABEL": "Global/Translate Y", "NAME": "fx_base_transition_y", "TYPE": "float", "DEFAULT": 0.0, "MIN": -5.0, "MAX": 5.0 },
    { "LABEL": "Global/Translate Z", "NAME": "fx_base_transition_z", "TYPE": "float", "DEFAULT": 0.0, "MIN": -1.0, "MAX": 1.0 },
    { "LABEL": "Global/Rotation X", "NAME": "fx_base_rotation_x", "TYPE": "float", "DEFAULT": 0.0, "MIN": 0.0, "MAX": 360.0 },
    { "LABEL": "Global/Rotation Y", "NAME": "fx_base_rotation_y", "TYPE": "float", "DEFAULT": 0.0, "MIN": 0.0, "MAX": 360.0 },
    { "LABEL": "Global/Rotation Z", "NAME": "fx_base_rotation_z", "TYPE": "float", "DEFAULT": 0.0, "MIN": 0.0, "MAX": 360.0 },
    { "LABEL": "Global/Scale", "NAME": "fx_base_scale", "TYPE": "float", "DEFAULT": 1.0, "MIN": 0.0, "MAX": 2.0 },
    { "LABEL": "Global/Z Fade", "NAME": "fx_z_fade", "TYPE": "float", "DEFAULT": 0.25, "MIN": 0.0, "MAX": 1.0 },
    { "LABEL": "Global/BPM Sync", "NAME": "fx_bpmsync", "TYPE": "bool", "DEFAULT": false, "FLAGS": "button" },

    { "LABEL": "Auto Move/Active", "NAME": "fx_automoveactive", "TYPE": "bool", "DEFAULT": false, "FLAGS": "button" },
    { "LABEL": "Auto Move/Size X", "NAME": "fx_automovesize_x", "TYPE": "float", "DEFAULT": 1.0, "MIN": 0.0, "MAX": 2.0 },
    { "LABEL": "Auto Move/Size Y", "NAME": "fx_automovesize_y", "TYPE": "float", "DEFAULT": 0.0, "MIN": 0.0, "MAX": 2.0 },
    { "LABEL": "Auto Move/Size Z", "NAME": "fx_automovesize_z", "TYPE": "float", "DEFAULT": 0.0, "MIN": 0.0, "MAX": 2.0 },
    { "LABEL": "Auto Move/Speed", "NAME": "fx_automovespeed", "TYPE": "float", "DEFAULT": 1, "MIN": 0.0, "MAX": 3.0 },
    { "LABEL": "Auto Move/Reverse", "NAME": "fx_automovereverse", "TYPE": "bool", "DEFAULT": false, "FLAGS": "button" },
    { "LABEL": "Auto Move/Shape", "NAME": "fx_automoveshape", "TYPE": "long", "VALUES": ["Smooth","In","Linear","Cut","Noise","Smooth In"], "DEFAULT": "Smooth" },

    { "LABEL": "Auto Rotate/Active", "NAME": "fx_autorotateactive", "TYPE": "bool", "DEFAULT": true, "FLAGS": "button" },
    { "LABEL": "Auto Rotate/Speed", "NAME": "fx_autorotatespeed", "TYPE": "float", "DEFAULT": 1.0, "MIN": 0.0, "MAX": 3.0 },
    { "LABEL": "Auto Rotate/Reverse", "NAME": "fx_autorotatereverse", "TYPE": "bool", "DEFAULT": false, "FLAGS": "button" },
    { "LABEL": "Auto Rotate/Axis", "NAME": "fx_autorotateaxis", "TYPE": "float", "DEFAULT": 0.0, "MIN": 0.0, "MAX": 3.0 },
    { "LABEL": "Auto Rotate/Auto Axis", "NAME": "fx_autorotateaxisactive", "TYPE": "bool", "DEFAULT": false, "FLAGS": "button" },

    { "LABEL": "Auto Scale/Active", "NAME": "fx_autoscaleactive", "TYPE": "bool", "DEFAULT": false, "FLAGS": "button" },
    { "LABEL": "Auto Scale/Speed", "NAME": "fx_autoscalespeed", "TYPE": "float", "DEFAULT": 1.0, "MIN": 0.0, "MAX": 3.0 },
    { "LABEL": "Auto Scale/Level", "NAME": "fx_autoscalelevel", "TYPE": "float", "DEFAULT": 1.0, "MIN": 0.0, "MAX": 1.0 },
  ],
  "GENERATORS": [
    {
      "NAME": "fx_move_position",
      "TYPE": "time_base",
      "PARAMS": {"speed": "fx_automovespeed","reverse": "fx_automovereverse", "bpm_sync": "fx_bpmsync", "speed_curve":2, "link_speed_to_global_bpm":true, "max_value":10000 }
    },
    {
      "NAME": "fx_rot_position",
      "TYPE": "time_base",
      "PARAMS": {"speed": "fx_autorotatespeed","reverse": "fx_autorotatereverse", "bpm_sync": "fx_bpmsync", "speed_curve":2, "link_speed_to_global_bpm":true, "max_value":10000 }
    },
    {
      "NAME": "fx_scale_position",
      "TYPE": "time_base",
      "PARAMS": {"speed": "fx_autoscalespeed", "bpm_sync": "fx_bpmsync", "speed_curve":2, "link_speed_to_global_bpm":true, "max_value":10000 }
    },
  ]
}*/


#include "MadNoise.glsl"
#include "MadCommon.glsl"

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

mat4 make3dTransformMatrix()
{
    // Center
    mat4 linePatternsMatrix = mat4(1,0,0,0,
                                   0,1,0,0,
                                   0,0,1,0,
                                   0,0,0,1);

    // Rotate
    if (fx_base_rotation_x!=0) {
      float angle = fx_base_rotation_x * 2*PI / 360;
      vec3 axis = vec3(1,0,0);
      float s = sin(angle);
      float c = cos(angle);
      float oc = 1.0 - c;
      linePatternsMatrix *= 
             mat4(oc * axis.x * axis.x + c,           oc * axis.x * axis.y - axis.z * s,  oc * axis.z * axis.x + axis.y * s,  0.0,
                  oc * axis.x * axis.y + axis.z * s,  oc * axis.y * axis.y + c,           oc * axis.y * axis.z - axis.x * s,  0.0,
                  oc * axis.z * axis.x - axis.y * s,  oc * axis.y * axis.z + axis.x * s,  oc * axis.z * axis.z + c,           0.0,
                  0.0,                                0.0,                                0.0,                                1.0);
    }
    if (fx_base_rotation_y!=0) {
      float angle = fx_base_rotation_y * 2*PI / 360;
      vec3 axis = vec3(0,1,0);
      float s = sin(angle);
      float c = cos(angle);
      float oc = 1.0 - c;
      linePatternsMatrix *= 
             mat4(oc * axis.x * axis.x + c,           oc * axis.x * axis.y - axis.z * s,  oc * axis.z * axis.x + axis.y * s,  0.0,
                  oc * axis.x * axis.y + axis.z * s,  oc * axis.y * axis.y + c,           oc * axis.y * axis.z - axis.x * s,  0.0,
                  oc * axis.z * axis.x - axis.y * s,  oc * axis.y * axis.z + axis.x * s,  oc * axis.z * axis.z + c,           0.0,
                  0.0,                                0.0,                                0.0,                                1.0);
    }
    if (fx_base_rotation_z!=0) {
      float angle = fx_base_rotation_z * 2*PI / 360;
      vec3 axis = vec3(0,0,1);
      float s = sin(angle);
      float c = cos(angle);
      float oc = 1.0 - c;
      linePatternsMatrix *= 
             mat4(oc * axis.x * axis.x + c,           oc * axis.x * axis.y - axis.z * s,  oc * axis.z * axis.x + axis.y * s,  0.0,
                  oc * axis.x * axis.y + axis.z * s,  oc * axis.y * axis.y + c,           oc * axis.y * axis.z - axis.x * s,  0.0,
                  oc * axis.z * axis.x - axis.y * s,  oc * axis.y * axis.z + axis.x * s,  oc * axis.z * axis.z + c,           0.0,
                  0.0,                                0.0,                                0.0,                                1.0);
    }

    // Auto Rotate
    if (fx_autorotateactive) {
      float angle = fract(fx_rot_position/5) * 2*PI;

      // fx_autorotateaxis from 0-3
      // 0 => Y axis
      // 1 => Z axis
      // 2 => X axis
      // 3 => Y axis again

      vec3 axis;

      float axisValue = mod(fx_autorotateaxis + (fx_autorotateaxisactive?fx_rot_position/7.654:0),3);

      if (axisValue < 1) {
        axis = mix(vec3(0,1,0),vec3(1,0,0),axisValue-0);
      } else if (axisValue < 2) {
        axis = mix(vec3(1,0,0),vec3(0,0,1),axisValue-1);
      } else {
        axis = mix(vec3(0,0,1),vec3(0,1,0),axisValue-2);
      }

      axis = normalize(axis);
      float s = sin(angle);
      float c = cos(angle);
      float oc = 1.0 - c;
      
      linePatternsMatrix *= 
             mat4(oc * axis.x * axis.x + c,           oc * axis.x * axis.y - axis.z * s,  oc * axis.z * axis.x + axis.y * s,  0.0,
                  oc * axis.x * axis.y + axis.z * s,  oc * axis.y * axis.y + c,           oc * axis.y * axis.z - axis.x * s,  0.0,
                  oc * axis.z * axis.x - axis.y * s,  oc * axis.y * axis.z + axis.x * s,  oc * axis.z * axis.z + c,           0.0,
                  0.0,                                0.0,                                0.0,                                1.0);
    }

    // Move
    float translate = 0;
    if (fx_automoveactive) {
      // "Smooth"=0,"In"=1,"Linear"=2,"Cut"=3,"Noise"=4,"Smooth In"=5
      if (fx_automoveshape == 0) {
        translate = sin((fx_move_position)*2*PI) / 2;
      } else if (fx_automoveshape == 1) {
        translate = (0.5-mod((fx_move_position),1));
      } else if (fx_automoveshape == 2) {
        translate = (0.5-abs(mod((fx_move_position)*2+1,2)-1));
      } else if (fx_automoveshape == 3) {
        translate = (0.5-step(0.5,mod((fx_move_position),1)));
      } else if (fx_automoveshape == 4) {
        translate = (0.5*noise(vec2(fx_move_position,0)));
      } else {
        translate = (-0.5 * sin(-PI/2 + mod(fx_move_position,1)*PI));
      }
    }
    linePatternsMatrix *= mat4(1,0,0,translate*fx_automovesize_x+10*fx_base_transition_x,
                               0,1,0,translate*fx_automovesize_y+10*fx_base_transition_y,
                               0,0,1,translate*fx_automovesize_z+10*fx_base_transition_z,
                               0,0,0,1);


    return linePatternsMatrix;
}


out float zPos;

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

	vec4 finalPos = mm_Vertex * make3dTransformMatrix();

	float scale = fx_base_scale;
	if (fx_autoscaleactive) {
		scale *= 1 - (0.5+0.5*sin(fx_scale_position*2*PI)) * fx_autoscalelevel;
	}
	finalPos.xyz *= scale;

	zPos = finalPos.z;

	// Tells OpenGL where this vertex should be on the output view
	gl_Position = mm_ModelViewProjectionMatrix * finalPos;
}