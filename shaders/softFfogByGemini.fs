/*{
    "CREDIT": "Audiobog/11lyd.no/Gemini AI",
    "DESCRIPTION": "Organic fog with a performance-ready strobe controller. Made for the Soft Ffog band",
    "CATEGORIES": ["Generative", "Performance"],
    "INPUTS": [
        {
            "NAME": "speed",
            "TYPE": "float",
            "MIN": 0.0, "MAX": 2.0, "DEFAULT": 0.4
        },
        {
            "NAME": "brightness",
            "TYPE": "float",
            "MIN": 0.5, "MAX": 5.0, "DEFAULT": 1.5
        },
        {
            "NAME": "zoom",
            "TYPE": "float",
            "MIN": 0.1, "MAX": 5.0, "DEFAULT": 1.2
        },
        {
            "NAME": "color1",
            "TYPE": "color",
            "DEFAULT": [0.1, 0.4, 0.8, 1.0]
        },
        {
            "NAME": "color2",
            "TYPE": "color",
            "DEFAULT": [0.9, 0.2, 0.3, 1.0]
        },
        {
            "NAME": "strobeRate",
            "TYPE": "float",
            "MIN": 0.0, "MAX": 30.0, "DEFAULT": 0.0,
            "LABEL": "Strobe Rate (Hz)"
        },
        {
            "NAME": "strobeAmount",
            "TYPE": "float",
            "MIN": 0.0, "MAX": 1.0, "DEFAULT": 0.0,
            "LABEL": "Strobe Fader"
        },
        {
            "NAME": "whiteStrobe",
            "TYPE": "bool",
            "DEFAULT": false,
            "LABEL": "White Strobe Mode"
        }
    ]
}*/

precision highp float;

float hash(vec2 p) {
    return fract(sin(dot(p, vec2(127.1, 311.7))) * 43758.5453123);
}

float noise(vec2 p) {
    vec2 i = floor(p);
    vec2 f = fract(p);
    f = f*f*(3.0-2.0*f);
    return mix(mix(hash(i + vec2(0.0,0.0)), hash(i + vec2(1.0,0.0)), f.x),
               mix(hash(i + vec2(0.0,1.0)), hash(i + vec2(1.0,1.0)), f.x), f.y);
}

float fbm(vec2 p) {
    float v = 0.0;
    float amp = 0.5;
    for (int i = 0; i < 5; i++) {
        v += amp * noise(p);
        p *= 2.0;
        amp *= 0.5;
    }
    return v;
}

void main() {
    vec2 uv = (gl_FragCoord.xy / RENDERSIZE.xy) * 2.0 - 1.0;
    uv.x *= RENDERSIZE.x / RENDERSIZE.y;
    uv *= zoom;

    float t = TIME * speed;
    vec2 uv1 = uv + vec2(t * 0.2, t * 0.1);
    vec2 uv2 = uv - vec2(t * 0.1, t * 0.3);

    float n1 = fbm(uv1 + fbm(uv1 + t));
    float n2 = fbm(uv2 + fbm(uv2 - t));
    float cloud = smoothstep(0.2, 0.8, n1 * n2);
    
    vec3 finalColor = mix(color1.rgb, color2.rgb, n1);
    finalColor *= cloud * brightness;
    
    // --- STROBE LOGIC ---
    
    // Create a square wave (0 or 1) based on frequency
    // We use fract(TIME * rate) to get a repeating 0-1 ramp
    float strobe = step(0.5, fract(TIME * strobeRate));
    
    // If strobeAmount is 0, this does nothing. 
    // If strobeAmount is 1, it flashes completely.
    if (whiteStrobe) {
        // White Strobe: Flashes to full white/overexposed
        vec3 whiteOut = vec3(brightness);
        finalColor = mix(finalColor, mix(finalColor, whiteOut, strobe), strobeAmount);
    } else {
        // Black Strobe: Flashes to black
        finalColor = mix(finalColor, finalColor * strobe, strobeAmount);
    }
    
    // Vignette
    float vignette = 1.0 - length(uv * 0.3);
    finalColor *= clamp(vignette, 0.0, 1.0);

    gl_FragColor = vec4(finalColor, 1.0);
}