/*{
    "CREDIT": "Gemini AI",
    "DESCRIPTION": "Black and white/sepia filter with time-based noise and vertical film scratches.",
    "CATEGORIES": [
        "Filter",
        "Retro"
    ],
    "INPUTS": [
        {
            "NAME": "inputImage",
            "TYPE": "image"
        },
        {
            "NAME": "sepiaAmount",
            "TYPE": "float",
            "DEFAULT": 0.5,
            "MIN": 0.0,
            "MAX": 1.0,
            "LABEL": "Sepia Tone Amount"
        },
        {
            "NAME": "noiseStrength",
            "TYPE": "float",
            "DEFAULT": 0.15,
            "MIN": 0.0,
            "MAX": 0.5,
            "LABEL": "Film Grain Strength"
        },
        {
            "NAME": "scratchIntensity",
            "TYPE": "float",
            "DEFAULT": 0.25,
            "MIN": 0.0,
            "MAX": 1.0,
            "LABEL": "Vertical Scratch Intensity"
        }
    ]
}*/

// Helper function to generate pseudo-random numbers
// This is essential for creating believable noise/grain
float rand(vec2 co) {
    // Generate a pseudo-random value between 0.0 and 1.0
    return fract(sin(dot(co.xy ,vec2(12.9898,78.233))) * 43758.5453);
}

void main() {
    vec2 uv = gl_FragCoord.xy / RENDERSIZE.xy;
    vec4 videoColor = texture(inputImage, uv);

    // --- 1. Grayscale and Sepia Toning ---
    
    // Convert to Luminance (Grayscale)
    // Standard sRGB luminance weights
    float luma = dot(videoColor.rgb, vec3(0.2126, 0.7152, 0.0722)); 
    vec3 grayColor = vec3(luma);

    // Sepia color approximation (richer tone)
    vec3 sepia = vec3(luma * 1.12, luma * 0.66, luma * 0.45);

    // Mix the grayscale and sepia based on the sepiaAmount uniform
    vec3 finalColor = mix(grayColor, sepia, sepiaAmount);

    // --- 2. Time-Based Film Grain (Noise) ---
    
    // Create high-frequency coordinates for the noise function
    vec2 noiseUV = uv * RENDERSIZE.xy / 2.0; // Scale UV for visible noise pattern
    
    // Add TIME to the coordinates to make the noise constantly flicker
    float noise = rand(noiseUV + floor(TIME * 20.0)); // floor(TIME*20) gives 20 changes per second
    
    // Adjust the noise to be centered around zero, then scale by noiseStrength
    float noiseValue = (noise - 0.5) * noiseStrength;

    // Apply the noise to the color channels
    finalColor += noiseValue;

    // --- 3. Vertical Scratches/Stripes ---
    
    // Define scratch width and frequency
    float scratchFrequency = 100.0; // How many potential scratch lines
    
    // Use the x-coordinate to check for a scratch
    float scratchLine = fract(uv.x * scratchFrequency); 
    
    // Use a random value that flickers with time to decide if a scratch is visible
    // The scratch will only appear on certain frame ticks and at specific X positions
    float scratchSeed = rand(vec2(floor(TIME * 15.0), scratchLine * 10.0));
    
    // Define the scratch area (a very narrow vertical line)
    // A small value like 0.01 makes the scratch very thin
    float scratchMask = smoothstep(0.005, 0.015, scratchLine) - smoothstep(0.995, 1.0, scratchLine); 
    
    // If the random seed is low (e.g., < 0.01), make the scratch visible
    float scratch = mix(0.0, 1.0, step(0.995, scratchSeed));
    
    // Combine the mask and the flicker, scaled by the intensity uniform
    float finalScratch = (scratchMask * scratch) * scratchIntensity;
    
    // Apply the scratch (darkening the film)
    finalColor -= finalScratch; 
    
    // --- 4. Final Output ---
    
    // Clamp the colors to ensure they stay within the 0.0 to 1.0 range
    finalColor = clamp(finalColor, 0.0, 1.0);
    
    gl_FragColor = vec4(finalColor, videoColor.a);
}
