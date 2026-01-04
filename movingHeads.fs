/*{
    "CREDIT": "Bent Gjersvik/11lyd.no through Gemini AI",
    "DESCRIPTION": "Moving Head Extreme with Gobos, Tilt, and Chase",
    "TAGS": "lighting, stage, beams, gobo, dmx",
    "INPUTS": [
        {
            "NAME": "movementSpeed",
            "TYPE": "float",
            "LABEL": "Pan Speed",
            "MIN": 0.0,
            "MAX": 4.0,
            "DEFAULT": 1.0
        },
        {
            "NAME": "pattern",
            "TYPE": "float",
            "LABEL": "Pan Pattern",
            "MIN": 0.0,
            "MAX": 3.0,
            "DEFAULT": 1.0,
            "VALUES": [0.0, 1.0, 2.0, 3.0],
            "LABELS": ["Static", "Sweep", "Sine Wave", "Random Search"]
        },
        {
            "NAME": "tilt",
            "TYPE": "float",
            "LABEL": "Tilt (Forward/Back)",
            "MIN": -0.8,
            "MAX": 0.8,
            "DEFAULT": 0.0
        },
        {
            "NAME": "beamAngleMult",
            "TYPE": "float",
            "LABEL": "Beam Zoom",
            "MIN": 0.02,
            "MAX": 0.4,
            "DEFAULT": 0.1
        },
        {
            "NAME": "dimmer",
            "TYPE": "float",
            "LABEL": "Master Intensity",
            "MIN": 0.0,
            "MAX": 1.0,
            "DEFAULT": 1.0
        },
        {
            "NAME": "sepGobo",
            "TYPE": "float",
            "LABEL": "--- Gobo Settings ---",
            "DEFAULT": 0.0
        },
        {
            "NAME": "goboType",
            "TYPE": "float",
            "LABEL": "Gobo Wheel",
            "MIN": 0.0,
            "MAX": 3.0,
            "DEFAULT": 0.0,
            "VALUES": [0.0, 1.0, 2.0, 3.0],
            "LABELS": ["Open", "Dots", "Star", "Stripes"]
        },
        {
            "NAME": "goboRotation",
            "TYPE": "float",
            "LABEL": "Gobo Rotation",
            "MIN": 0.0,
            "MAX": 10.0,
            "DEFAULT": 2.0
        },
        {
            "NAME": "sepChase",
            "TYPE": "float",
            "LABEL": "--- Chase & Color ---",
            "DEFAULT": 0.0
        },
        {
            "NAME": "chaseMode",
            "TYPE": "float",
            "LABEL": "Chase Mode",
            "MIN": 0.0,
            "MAX": 3.0,
            "DEFAULT": 1.0,
            "VALUES": [0.0, 1.0, 2.0, 3.0],
            "LABELS": ["None", "Sequential", "Ping-Pong", "Random"]
        },
        {
            "NAME": "chaseSpeed",
            "TYPE": "float",
            "LABEL": "Chase Speed",
            "MIN": 0.0,
            "MAX": 10.0,
            "DEFAULT": 4.0
        },
        {
            "NAME": "manualColor",
            "TYPE": "color",
            "LABEL": "Color",
            "DEFAULT": [1.0, 1.0, 1.0, 1.0]
        },
        {
            "NAME": "autoColor",
            "TYPE": "bool",
            "LABEL": "Auto-Color Cycle",
            "DEFAULT": true
        }
    ]
}*/

#define PI 3.14159265359

// --- Math Helpers ---
float hash(float n) { return fract(sin(n) * 43758.5453123); }

float noise(vec2 p) {
    vec2 i = floor(p);
    vec2 f = fract(p);
    f = f*f*(3.0-2.0*f);
    float n = i.x + i.y*57.0;
    return mix(mix(hash(n+0.0), hash(n+1.0),f.x), mix(hash(n+57.0), hash(n+58.0),f.x),f.y);
}

vec3 hsv2rgb(vec3 c) {
    vec4 K = vec4(1.0, 2.0 / 3.0, 1.0 / 3.0, 3.0);
    vec3 p = abs(fract(c.xxx + K.xyz) * 6.0 - K.www);
    return c.z * mix(K.xxx, clamp(p - K.xxx, 0.0, 1.0), c.y);
}

// --- Beam Rendering with Gobos and Tilt ---
float drawBeam(vec2 uv, vec2 anchor, float pan, float tiltVal, float coneWidth, float gType, float gRot) {
    // 1. Perspective Tilt
    // We adjust the Y scale of the UV to simulate the beam leaning toward/away from us
    float tiltFactor = 1.0 + tiltVal;
    vec2 p = uv - anchor;
    p.y /= tiltFactor; 

    // 2. Pan Rotation
    float s = sin(pan);
    float c = cos(pan);
    vec2 rotatedP = vec2(p.x * c - p.y * s, p.x * s + p.y * c);

    // 3. Basic Cone Logic
    float forwardClip = smoothstep(0.0, 0.02, rotatedP.y);
    float angleFromCenter = atan(rotatedP.x, max(rotatedP.y, 0.001));
    float beamRadial = smoothstep(coneWidth, coneWidth * 0.5, abs(angleFromCenter));

    // 4. Gobo Implementation
    float goboMask = 1.0;
    float rAngle = angleFromCenter * 10.0 + TIME * gRot; // Animated rotation

    if (gType > 0.5 && gType < 1.5) { // Dots
        goboMask = step(0.5, sin(angleFromCenter * 40.0)) * step(0.5, sin(rotatedP.y * 20.0 - TIME * gRot));
    } else if (gType > 1.5 && gType < 2.5) { // Star
        goboMask = 0.5 + 0.5 * sin(angleFromCenter * 12.0 + TIME * gRot);
    } else if (gType > 2.5) { // Stripes
        goboMask = step(0.3, fract(angleFromCenter * 8.0 + TIME * gRot * 0.2));
    }

    // 5. Falloff
    // As tilt increases, the beam should appear shorter or longer
    float distanceFalloff = smoothstep(2.5 * tiltFactor, 0.0, rotatedP.y); 
    
    return beamRadial * forwardClip * distanceFalloff * goboMask;
}

void main() {
    vec2 uv = gl_FragCoord.xy / RENDERSIZE.xy;
    uv = uv * 2.0 - 1.0;
    uv.x *= RENDERSIZE.x / RENDERSIZE.y;
    uv.y += 0.7;

    vec3 finalOutput = vec3(0.0);
    float tMove = TIME * movementSpeed;
    float tChase = TIME * chaseSpeed;
    
    // Chase Pointer Logic
    float chasePointer = 0.0;
    if (chaseMode > 0.5 && chaseMode < 1.5) chasePointer = mod(tChase, 8.0);
    else if (chaseMode > 1.5 && chaseMode < 2.5) chasePointer = abs(7.0 - mod(tChase, 14.0));
    else if (chaseMode > 2.5) chasePointer = floor(hash(floor(tChase)) * 8.0);

    for (int i = 0; i < 8; i++) {
        float fi = float(i);
        float xOffset = -1.4 + (fi / 7.0) * 2.8;
        vec2 anchor = vec2(xOffset, 0.0);
        
        // Intensity
        float iLevel = 1.0;
        if (chaseMode > 0.5) {
            float d = abs(fi - chasePointer);
            if (chaseMode < 1.5) d = min(d, abs(fi - (chasePointer - 8.0)));
            iLevel = exp(-d * 2.0);
        }

        // Pan Position
        float pan = 0.0;
        if (pattern < 0.5) pan = 0.0;
        else if (pattern < 1.5) pan = sin(tMove) * 0.7;
        else if (pattern < 2.5) pan = sin(tMove + fi * 0.5) * 0.6;
        else pan = sin(tMove * (0.5 + hash(fi)*0.5)) * 0.5;

        // Color
        vec3 c = autoColor ? hsv2rgb(vec3(TIME * 0.1 + fi * 0.1, 0.8, 1.0)) : manualColor.rgb;

        // Render
        float beam = drawBeam(uv, anchor, pan, tilt, beamAngleMult, goboType, goboRotation);
        float glow = smoothstep(0.1, 0.0, length(uv - anchor)) * 0.5;
        
        finalOutput += (beam + glow) * c * iLevel;
    }

    // Atmosphere
    float haze = 0.85 + 0.15 * noise(uv * 2.0 + TIME * 0.1);
    gl_FragColor = vec4(finalOutput * dimmer * haze, 1.0);
}
