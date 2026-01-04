/*{
  "CREDIT": "Gemini for MadMapper",
  "DESCRIPTION": "Auto-drifting camera that mathematically stays inside image bounds.",
  "CATEGORIES": [
    "Distortion",
    "Geometry"
  ],
  "INPUTS": [
    {
      "NAME": "inputImage",
      "TYPE": "image"
    },
    {
      "NAME": "zoom_level",
      "TYPE": "float",
      "DEFAULT": 1.5,
      "MIN": 1.0,
      "MAX": 5.0,
      "LABEL": "Zoom Level"
    },
    {
      "NAME": "speed_x",
      "TYPE": "float",
      "DEFAULT": 0.1,
      "MIN": 0.0,
      "MAX": 1.0,
      "LABEL": "Drift Speed X"
    },
    {
      "NAME": "speed_y",
      "TYPE": "float",
      "DEFAULT": 0.13,
      "MIN": 0.0,
      "MAX": 1.0,
      "LABEL": "Drift Speed Y"
    },
    {
      "NAME": "phase_offset",
      "TYPE": "float",
      "DEFAULT": 0.0,
      "MIN": 0.0,
      "MAX": 10.0,
      "LABEL": "Pattern Offset"
    }
  ]
}*/

void main() {
    // 1. Calculate the size of the "Visible Window"
    // If zoom is 2.0, we only see 0.5 (half) of the texture width
    float windowSize = 1.0 / zoom_level;
    
    // 2. Calculate the "Playable Area" (The Safe Zone)
    // The available space to move is the remaining area.
    // e.g. If we see 0.5, we have 0.5 remaining space to slide around in.
    float playableArea = 1.0 - windowSize;
    
    // 3. Generate Auto-Movement Signals
    // We map a Sine wave (-1 to 1) to a 0 to 1 range
    // We use slightly different frequencies for X and Y so the movement isn't a perfect circle
    
    float autoX = sin((TIME * speed_x) + phase_offset); // -1 to 1
    float autoY = cos((TIME * speed_y) + phase_offset); // -1 to 1 (using cos creates circular motion)
    
    // Normalize to 0.0 -> 1.0
    float normX = (autoX * 0.5) + 0.5;
    float normY = (autoY * 0.5) + 0.5;
    
    // 4. Calculate the Offset
    // We multiply the normalized movement by the playable area.
    // This ensures that even at max movement (1.0), we only move as far as the playable area allows.
    vec2 offset = vec2(normX * playableArea, normY * playableArea);
    
    // 5. Apply the Zoom and Offset
    // Standard UV coordinates
    vec2 uv = gl_FragCoord.xy / RENDERSIZE.xy;
    
    // Scale the UVs to create the zoom
    vec2 zoomedUV = uv * windowSize;
    
    // Add the offset to move the window
    vec2 finalUV = zoomedUV + offset;
    
    gl_FragColor = IMG_NORM_PIXEL(inputImage, finalUV);
}
