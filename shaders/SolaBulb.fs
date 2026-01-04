/*{
  "CREDIT": "Gemini for MadMapper",
  "DESCRIPTION": "Emulates Astera SolaBulb Profile 13 (DIM RGBAWS) with separate channels for DMX mapping.",
  "CATEGORIES": [
    "Generator",
    "Light Simulation"
  ],
  "INPUTS": [
    {
      "NAME": "dimmer",
      "TYPE": "float",
      "DEFAULT": 1.0,
      "MIN": 0.0,
      "MAX": 1.0,
      "LABEL": "1. Dimmer"
    },
    {
      "NAME": "ch_red",
      "TYPE": "float",
      "DEFAULT": 0.0,
      "MIN": 0.0,
      "MAX": 1.0,
      "LABEL": "2. Red"
    },
    {
      "NAME": "ch_green",
      "TYPE": "float",
      "DEFAULT": 0.0,
      "MIN": 0.0,
      "MAX": 1.0,
      "LABEL": "3. Green"
    },
    {
      "NAME": "ch_blue",
      "TYPE": "float",
      "DEFAULT": 0.0,
      "MIN": 0.0,
      "MAX": 1.0,
      "LABEL": "4. Blue"
    },
    {
      "NAME": "ch_amber",
      "TYPE": "float",
      "DEFAULT": 0.0,
      "MIN": 0.0,
      "MAX": 1.0,
      "LABEL": "5. Amber"
    },
    {
      "NAME": "ch_mint",
      "TYPE": "float",
      "DEFAULT": 0.0,
      "MIN": 0.0,
      "MAX": 1.0,
      "LABEL": "6. White/Mint"
    },
    {
      "NAME": "ch_strobe",
      "TYPE": "float",
      "DEFAULT": 0.0,
      "MIN": 0.0,
      "MAX": 1.0,
      "LABEL": "7. Strobe Rate"
    },
    {
      "NAME": "zoom",
      "TYPE": "float",
      "DEFAULT": 0.5,
      "MIN": 0.0,
      "MAX": 1.0,
      "LABEL": "Zoom"
    },
    {
      "NAME": "target_x",
      "TYPE": "float",
      "DEFAULT": 0.5,
      "MIN": 0.0,
      "MAX": 1.0,
      "LABEL": "Target X"
    },
    {
      "NAME": "target_y",
      "TYPE": "float",
      "DEFAULT": 0.5,
      "MIN": 0.0,
      "MAX": 1.0,
      "LABEL": "Target Y"
    }
  ]
}*/

void main() {
    // 1. Construct Color from individual channels
    // We simulate the specific diode colors found in Astera engines
    
    vec3 c_r = vec3(1.0, 0.0, 0.0) * ch_red;
    vec3 c_g = vec3(0.0, 1.0, 0.0) * ch_green;
    vec3 c_b = vec3(0.0, 0.0, 1.0) * ch_blue;
    
    // Amber is usually a warm yellow-orange
    vec3 c_a = vec3(1.0, 0.65, 0.0) * ch_amber; 
    
    // Astera "White" is often a Mint LED for higher CRI, slightly greenish-cyan white
    vec3 c_m = vec3(0.85, 1.0, 0.95) * ch_mint; 
    
    // Combine all
    vec3 mixedColor = c_r + c_g + c_b + c_a + c_m;

    // 2. Strobe Logic
    float strobeVal = 1.0;
    if (ch_strobe > 0.05) {
        // Map input 0.0-1.0 to a strobe frequency (e.g. 0 to 30Hz)
        float speed = ch_strobe * 30.0;
        // Basic square wave strobe
        strobeVal = step(0.5, sin(TIME * speed * 6.28));
    }

    // 3. Beam Geometry (Cone Spot)
    vec2 uv = gl_FragCoord.xy / RENDERSIZE.xy;
    vec2 target = vec2(target_x, target_y);
    float dist = distance(uv, target);
    
    // Map zoom to beam size (0.0 = Wide, 1.0 = Tight)
    float beamRadius = mix(0.4, 0.04, zoom); 
    float softness = beamRadius * 0.3; // Edge softness relative to size
    
    // Create the spot
    float spot = 1.0 - smoothstep(beamRadius - softness, beamRadius + softness, dist);
    
    // Add hot spot intensity
    float hotspot = 1.0 - smoothstep(0.0, beamRadius * 1.5, dist);
    spot = mix(spot, hotspot, 0.5);

    // 4. Final Output
    // Apply Dimmer, Strobe, and calculated Spot shape
    float finalAlpha = spot * dimmer * strobeVal;
    
    // Ensure we don't exceed 1.0 color values for display purposes
    vec3 finalRGB = clamp(mixedColor, 0.0, 1.0);
    
    gl_FragColor = vec4(finalRGB * finalAlpha, finalAlpha);
}
