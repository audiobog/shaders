/*{
  "DESCRIPTION": "Dynamic fractal raymarch with two mirrored light sources",
  "CREDIT": "Shader Genius (based on @XorDev) + dual-light mod",
  "ISFVSN": "2",
  "INPUTS": [
    { "NAME": "iResolution", "TYPE": "point2D", "DEFAULT": [0.5, 0.5] },
    { "NAME": "xyControl", "TYPE": "point2D", "DEFAULT": [0.5, 0.5] },

    { "NAME": "colorPulse",  "TYPE": "float", "DEFAULT": 1.0, "MIN": 0.1, "MAX": 5.0 },
    { "NAME": "zoom",        "TYPE": "float", "DEFAULT": 9.0, "MIN": 1.0, "MAX": 20.0 },
    { "NAME": "speed",       "TYPE": "float", "DEFAULT": 1.0, "MIN": 0.1, "MAX": 5.0 },
    { "NAME": "morph",       "TYPE": "float", "DEFAULT": 1.0, "MIN": 0.1, "MAX": 3.0 },

    { "NAME": "brightness",  "TYPE": "float", "DEFAULT": 0.001, "MIN": 0.0001, "MAX": 0.10 },
    { "NAME": "saturation",  "TYPE": "float", "DEFAULT": 1.0, "MIN": 0.0, "MAX": 2.0 },
    { "NAME": "contrast",    "TYPE": "float", "DEFAULT": 1.0, "MIN": 0.5, "MAX": 2.0 },

    { "NAME": "paletteIndex", "TYPE": "long", "DEFAULT": 0,
      "VALUES": [0,1,2,3,4,5,6],
      "LABELS": ["Trippy", "Cosmic", "Neon", "Lava", "Aurora", "Spiral", "PsyStorm"]
    }
  ]
}*/

vec3 getPaletteColor(float t, int index) {
    if (index == 0) return 0.5 + 0.5 * cos(6.2831 * (t + vec3(0.0, 0.33, 0.67)));
    if (index == 1) return 0.5 + 0.5 * cos(6.2831 * (t + vec3(0.1, 0.3, 0.6)));
    if (index == 2) return vec3(sin(t * 2.0), sin(t * 1.5), cos(t * 3.0));
    if (index == 3) return vec3(sin(t + 0.3), sin(t + 1.0), cos(t + 1.5));
    if (index == 4) return vec3(0.4 + 0.6 * sin(t * 2.0), 0.3 + 0.6 * sin(t), 1.0);
    if (index == 5) return vec3(cos(t * 3.0), sin(t * 2.0), 0.5 + 0.5 * sin(t));
    if (index == 6) return vec3(sin(t * 5.0), cos(t * 3.0), sin(t * 7.0));
    return vec3(1.0);
}

vec3 renderLight(vec2 uv, vec2 mouse, float time) {
    vec4 O = vec4(0.0);
    float z = 0.0;

    for (float i = 0.0; i < 100.0; i++) {
        vec3 p = z * normalize(vec3(uv + mouse, 0.0));
        p.z += zoom;

        vec3 axis = normalize(cos(vec3(0.0, 1.0, 0.0) + time * speed - 0.4 * z));
        p = axis * dot(axis, p) - cross(axis, p);

        float s = 0.0;
        float jf = 0.0;
        for (float j = 1.0; j < 6.0; j++) {
            s = length(p += cos(p * j * morph + time * speed).yzx / j);
            jf = j;
        }

        float d = 0.1 * (abs(sin(s - time * speed)) + abs(p.y) / jf);
        z += d;

        float pt = s * colorPulse - time * speed;
        vec3 col = getPaletteColor(pt, int(paletteIndex));

        O.rgb += (col + 1.1) / (d + 0.001);
    }

    return O.rgb;
}

void main() {
    vec2 fragCoord = gl_FragCoord.xy;
    vec2 res = iResolution;

    vec2 uv = (fragCoord / res) * 2.0 - 1.0;
    uv.x *= res.x / res.y;

    vec2 mouse = xyControl - 0.5;
    float time = TIME;

    // --- LEFT LIGHT (original) ---
    vec3 leftLight = renderLight(uv, mouse, time);

    // --- RIGHT LIGHT (mirrored UV) ---
    vec2 uv2 = uv;
    uv2.x = -uv.x;   // mirror horizontally
    vec3 rightLight = renderLight(uv2, mouse, time);

    // Combine both lights
    vec3 col = leftLight + rightLight;

    // Post-processing
    col *= 0.015 * brightness;
    col = pow(col, vec3(1.0));
    col = clamp(col, 0.0, 10.0);
    col = mix(vec3(dot(col, vec3(0.333))), col, saturation);
    col = (col - 0.5) * contrast + 0.5;
    col = clamp(col, 0.0, 1.0);

    gl_FragColor = vec4(col, 1.0);
}
