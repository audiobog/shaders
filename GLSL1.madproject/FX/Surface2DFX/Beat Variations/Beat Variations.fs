/*{
    "RESOURCE_TYPE": "Surface 2D FX For MadMapper",
    "CREDIT": "Made with MadAI",
    "DESCRIPTION": "Beat Split Transition with Random Variations",
    "INPUTS": [
        { "LABEL": "Beats", "NAME": "fx_beats", "TYPE": "float", "DEFAULT": 4.0, "MIN": 1.0, "MAX": 16.0 },
        { "LABEL": "Variations", "NAME": "fx_variations", "TYPE": "float", "DEFAULT": 6.0, "MIN": 3.0, "MAX": 10.0 },
        { "LABEL": "Random Seed", "NAME": "fx_seed", "TYPE": "float", "DEFAULT": 1.0, "MIN": 0.0, "MAX": 100.0 }
    ],
    "GENERATORS": [
		{ "NAME": "fx_bpm_position", "TYPE": "time_base", "PARAMS": {"speed": 1, "bpm_sync": true} }  
    ]
}*/

float fx_mod(float x, float y) {
    return x - y * floor(x/y);
}

// Hash function for random values
float fx_hash(vec2 p) {
    p = fract(p * vec2(123.4, 789.1));
    p += dot(p, p + 33.33);
    return fract(p.x * p.y);
}

// Noise generator based on hash
float fx_noise(vec2 p, float seed) {
    vec2 i = floor(p);
    vec2 f = fract(p);
    f = f * f * (3.0 - 2.0 * f); // Smoothstep
    
    float a = fx_hash(i + vec2(0.0, 0.0) + seed);
    float b = fx_hash(i + vec2(1.0, 0.0) + seed);
    float c = fx_hash(i + vec2(0.0, 1.0) + seed);
    float d = fx_hash(i + vec2(1.0, 1.0) + seed);
    
    return mix(mix(a, b, f.x), mix(c, d, f.x), f.y);
}

// Warp function for distortion effects
vec2 fx_warp(vec2 uv, float amount, float seed) {
    vec2 offset = vec2(
        fx_noise(uv * 5.0 + 0.2, seed),
        fx_noise(uv * 5.0 + 10.0, seed + 1.0)
    );
    return uv + (offset * 2.0 - 1.0) * amount;
}

vec4 fxColorForPixel(vec2 mm_FragNormCoord) {
    vec2 uv = mm_FragNormCoord;
    float beatPhase = fx_mod(fx_bpm_position, fx_beats);
    
    // Random value based on beats and seed
    float randomSeed = floor(fx_bpm_position / fx_beats) + fx_seed;
    float random1 = fx_hash(vec2(randomSeed, 123.45));
    float random2 = fx_hash(vec2(randomSeed, 678.91));
    float random3 = fx_hash(vec2(randomSeed, 246.80));
    
    // Select a transition type based on random and variations parameter
    int transitionType = int(floor(random1 * fx_variations));
    float transition = smoothstep(0.0, 0.5, beatPhase);
    
    // Based on the transition type, apply different effects
    if (transitionType == 0) {
        // Scroll from top
        uv.y = (uv.y - 1.0 + transition);
        uv.y = fx_mod(uv.y, 1.0);
    } 
    else if (transitionType == 1) {
        // Scroll from right
        uv.x = (uv.x - 1.0 + transition);
        uv.x = fx_mod(uv.x, 1.0);
    }
    else if (transitionType == 2) {
        // Scroll from bottom
        uv.y = (uv.y + 1.0 - transition);
        uv.y = fx_mod(uv.y, 1.0);
    }
    else if (transitionType == 3) {
        // Scroll from left
        uv.x = (uv.x + 1.0 - transition);
        uv.x = fx_mod(uv.x, 1.0);
    }
    else if (transitionType == 4) {
        // Diamond transition
        float dist = abs(uv.x - 0.5) + abs(uv.y - 0.5);
        float diamondSize = transition * 1.5;
        if (dist > diamondSize) {
            return FX_NORM_PIXEL(vec2(fx_mod(uv.x + random2, 1.0), fx_mod(uv.y + random3, 1.0)));
        }
    }
    else if (transitionType == 5) {
        // Split into grid
        int gridSize = 2 + int(random2 * 3.0);
        vec2 gridUV = uv * float(gridSize);
        vec2 cell = floor(gridUV);
        gridUV = fract(gridUV);
        
        // Each cell gets a different delay
        float cellDelay = fx_hash(cell + randomSeed);
        float cellTransition = smoothstep(0.0, 0.5, beatPhase - cellDelay * 0.5);
        
        if (cellTransition < 1.0) {
            gridUV = (gridUV - 0.5) * (1.0 / max(0.01, cellTransition)) + 0.5;
            if (gridUV.x < 0.0 || gridUV.x > 1.0 || gridUV.y < 0.0 || gridUV.y > 1.0) {
                // Wrapping behavior for out-of-bounds
                gridUV = fract(gridUV);
            }
        }
        uv = gridUV;
    }
    else if (transitionType == 6) {
        // Spiral
        vec2 center = vec2(0.5, 0.5);
        vec2 toCenter = uv - center;
        float angle = atan(toCenter.y, toCenter.x);
        float dist = length(toCenter);
        float newAngle = angle + transition * 10.0 * random2;
        uv = center + dist * vec2(cos(newAngle), sin(newAngle));
    }
    else if (transitionType == 7) {
        // Zoom in/out
        vec2 center = vec2(0.5 + (random2 - 0.5) * 0.4, 0.5 + (random3 - 0.5) * 0.4);
        uv = center + (uv - center) * (1.0 + transition * (random1 > 0.5 ? 1.0 : -0.8));
    }
    else if (transitionType == 8) {
        // Warp effect
        uv = fx_warp(uv, transition * 0.3, randomSeed);
    }
    else {
        // Diagonal swipe
        float diag = (uv.x + uv.y) * 0.5;
        diag = (diag - 1.0 + transition);
        if (diag < 0.0) {
            uv = vec2(fx_mod(uv.x + random2, 1.0), fx_mod(uv.y + random3, 1.0));
        }
    }
    
    // Apply a subtle color shift based on beat phase for some transitions
    vec4 color = FX_NORM_PIXEL(uv);
    if (transitionType == 4 || transitionType == 6 || transitionType == 8) {
        float colorShift = sin(beatPhase * 3.14159) * 0.05;
        color.r = FX_NORM_PIXEL(vec2(fx_mod(uv.x + colorShift, 1.0), fx_mod(uv.y, 1.0))).r;
        color.b = FX_NORM_PIXEL(vec2(fx_mod(uv.x - colorShift, 1.0), fx_mod(uv.y, 1.0))).b;
    }
    
    return color;
}