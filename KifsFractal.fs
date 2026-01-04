/*{
    "CREDIT": "Original by coyote, restored by Gemini",
    "DESCRIPTION": "Restored architectural Menger fractal with fly-through movement.",
    "CATEGORIES": ["Generative", "Fractal"],
    "INPUTS": [
        {
            "NAME": "speed",
            "TYPE": "float",
            "MIN": 0.0,
            "MAX": 2.0,
            "DEFAULT": 0.4,
            "LABEL": "Movement Speed"
        },
        {
            "NAME": "detail",
            "TYPE": "float",
            "MIN": 0.1,
            "MAX": 1.0,
            "DEFAULT": 0.29,
            "LABEL": "Room Structure"
        },
        {
            "NAME": "brightness",
            "TYPE": "float",
            "MIN": 0.0,
            "MAX": 1.0,
            "DEFAULT": 0.8
        }
    ]
}*/

precision highp float;

// Standard Rotation Matrix
mat3 getRotYMat(float a) {
    return mat3(cos(a), 0.0, sin(a), 0.0, 1.0, 0.0, -sin(a), 0.0, cos(a));
}

void main() {
    vec2 res = RENDERSIZE.xy;
    float t = TIME * speed;
    
    // UV Setup: Matches the original Shadertoy aspect ratio correction
    vec3 p = vec3((2.0 * gl_FragCoord.xy - res) / res.x, 1.0);
    
    // Camera Rotation (The Fly-through angle)
    p *= getRotYMat(-t);
    
    vec3 r = vec3(0.0);
    vec3 q = vec3(0.0);
    
    // The "Flight Path" - This is what takes you through the rooms
    q.zx += 10.0 + vec2(sin(t), cos(t)) * 3.0;
    
    float c = 0.0;
    float d = 0.0;
    float m = 1.0;

    // Raymarching Loop - High step count for sharp edges
    for (float i = 1.0; i > 0.0; i -= 0.01) {
        c = i;
        d = 0.0;
        m = 1.0;
        
        // This inner loop creates the recursive "Menger" rooms
        for (int j = 0; j < 3; j++) {
            // Restore the specific 'Coyote' fold: Space folding/repetition
            r = mod(q * m + 1.0, 2.0) - 1.0;
            
            // The magic line that creates the rooms: Maximize the box dimensions
            r = abs(r);
            r = max(r, r.yzx);
            
            // Distance Estimator (using the 'detail' parameter for room shape)
            d = max(d, (detail - length(r) * 0.6) / m) * 0.8;
            m *= 1.1; // Iterative scaling
        }

        q += p * d; // Move ray forward
        
        // Surface hit threshold - must be very small for sharp geometry
        if (d < 1e-5) break;
    }
    
    // Restore the original "Neon-on-Dark" coloring
    float k = dot(r, r + 0.15);
    vec3 color = vec3(1.0, k, k / c) - (1.0 - brightness);
    
    gl_FragColor = vec4(color, 1.0);
}