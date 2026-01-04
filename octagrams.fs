/*{
    "CREDIT": "Original by ShaderToy, converted by Gemini",
    "DESCRIPTION": "A glowing geometric raymarched lattice (Octgrams).",
    "CATEGORIES": [
        "Generative",
        "3D"
    ],
    "INPUTS": [
        {
            "NAME": "speed",
            "TYPE": "float",
            "MIN": 0.1,
            "MAX": 2.0,
            "DEFAULT": 1.0,
            "LABEL": "Animation Speed"
        },
        {
            "NAME": "shapeScale",
            "TYPE": "float",
            "MIN": 0.5,
            "MAX": 3.0,
            "DEFAULT": 1.5,
            "LABEL": "Shape Scale"
        },
        {
            "NAME": "blueShift",
            "TYPE": "float",
            "MIN": 0.0,
            "MAX": 1.0,
            "DEFAULT": 0.5,
            "LABEL": "Blue Intensity"
        },
        {
            "NAME": "rotationStrength",
            "TYPE": "float",
            "MIN": 0.0,
            "MAX": 1.0,
            "DEFAULT": 0.5,
            "LABEL": "Camera Twist"
        }
    ]
}*/

precision highp float;

// Global variable used for the tail/blur effect
float gTime = 0.0;

mat2 rot(float a) {
    float c = cos(a), s = sin(a);
    return mat2(c, s, -s, c);
}

float sdBox(vec3 p, vec3 b) {
    vec3 q = abs(p) - b;
    return length(max(q, 0.0)) + min(max(q.x, max(q.y, q.z)), 0.0);
}

float box(vec3 pos, float scale) {
    pos *= scale;
    float base = sdBox(pos, vec3(0.4, 0.4, 0.1)) / 1.5;
    // The original shader has some logic that results in a negative base
    return -base;
}

float box_set(vec3 pos) {
    vec3 pos_origin = pos;
    float t_mod = gTime * 0.4;
    float pulse = abs(sin(t_mod)) * shapeScale;
    
    // Box 1 & 2 (Y-axis movement)
    vec3 p1 = pos_origin; p1.y += sin(t_mod) * 2.5; p1.xy *= rot(0.8);
    float box1 = box(p1, 2.0 - pulse);
    
    vec3 p2 = pos_origin; p2.y -= sin(t_mod) * 2.5; p2.xy *= rot(0.8);
    float box2 = box(p2, 2.0 - pulse);
    
    // Box 3 & 4 (X-axis movement)
    vec3 p3 = pos_origin; p3.x += sin(t_mod) * 2.5; p3.xy *= rot(0.8);
    float box3 = box(p3, 2.0 - pulse);
    
    vec3 p4 = pos_origin; p4.x -= sin(t_mod) * 2.5; p4.xy *= rot(0.8);
    float box4 = box(p4, 2.0 - pulse);
    
    // Central boxes
    vec3 p5 = pos_origin; p5.xy *= rot(0.8);
    float box5 = box(p5, 0.5) * 6.0;
    
    float box6 = box(pos_origin, 0.5) * 6.0;
    
    return max(max(max(max(max(box1, box2), box3), box4), box5), box6);
}

float map(vec3 pos) {
    return box_set(pos);
}

void main() {
    // 1. Setup Coordinates
    vec2 uv = (gl_FragCoord.xy * 2.0 - RENDERSIZE.xy) / min(RENDERSIZE.x, RENDERSIZE.y);
    
    // 2. Setup Camera and Ray
    float t_scaled = TIME * speed;
    vec3 ro = vec3(0.0, -0.2, t_scaled * 4.0);
    vec3 ray = normalize(vec3(uv, 1.5));
    
    // Camera Rotation / Twist
    ray.xy = ray.xy * rot(sin(t_scaled * 0.03) * (5.0 * rotationStrength));
    ray.yz = ray.yz * rot(sin(t_scaled * 0.05) * 0.2);
    
    float t = 0.1;
    vec3 col = vec3(0.0);
    float ac = 0.0;

    // 3. Raymarching Loop
    for (int i = 0; i < 80; i++) { // Reduced iterations slightly for better performance in MadMapper
        vec3 pos = ro + ray * t;
        
        // Infinite repetition logic
        pos = mod(pos - 2.0, 4.0) - 2.0;
        
        // This creates the motion trail effect
        gTime = t_scaled - float(i) * 0.01;
        
        float d = map(pos);

        d = max(abs(d), 0.01);
        ac += exp(-d * 23.0);

        t += d * 0.55;
    }

    // 4. Coloring
    col = vec3(ac * 0.02);
    
    // Add the glowing blue/purple filmic tint
    float colorPulse = abs(sin(t_scaled));
    col += vec3(0.0, 0.2 * colorPulse, blueShift + colorPulse * 0.2);

    // Fog/Distance Fade
    float alpha = 1.0 - t * (0.02 + 0.02 * sin(t_scaled));
    
    gl_FragColor = vec4(col, alpha);
}