/*{
    "CREDIT": "Gemini AI",
    "DESCRIPTION": "A halftone/dot screen effect with controls for distortion, dot size, and density.",
    "CATEGORIES": [
        "Filter"
    ],
    "INPUTS": [
        {
            "NAME": "inputImage",
            "TYPE": "image"
        },
        {
            "NAME": "dotSize",
            "TYPE": "float",
            "DEFAULT": 10.0,
            "MIN": 1.0,
            "MAX": 50.0
        },
        {
            "NAME": "density",
            "TYPE": "float",
            "DEFAULT": 50.0,
            "MIN": 10.0,
            "MAX": 200.0
        },
        {
            "NAME": "distortion",
            "TYPE": "float",
            "DEFAULT": 0.0,
            "MIN": 0.0,
            "MAX": 1.0
        },
        {
            "NAME": "colorMode",
            "TYPE": "bool",
            "DEFAULT": 1.0
        }
    ]
}*/

void main() {
    // 1. Get the current texture coordinate
    vec2 uv = gl_FragCoord.xy / RENDERSIZE.xy;

    // 2. Introduce distortion (using sine waves for a subtle, wavy effect)
    // The distortion uniform controls the strength of the distortion
    uv.x += sin(uv.y * density * 0.1 + TIME * 1.0) * 0.01 * distortion;
    uv.y += cos(uv.x * density * 0.05 + TIME * 1.5) * 0.01 * distortion;
    
    // 3. Define the grid/cell based on the density parameter
    vec2 cell = floor(uv * density);
    vec2 grid_uv = fract(uv * density); // Coordinates within the cell (0.0 to 1.0)

    // 4. Sample the input video/image at the center of the cell
    // We use the center of the cell to get the average color for the dot
    vec2 center_uv = (cell + 0.5) / density;
    vec4 videoColor = texture(inputImage, center_uv);
    
    // 5. Calculate the luminosity (brightness) of the sampled color
    // This will determine the size of the dot (brighter = bigger dot)
    float luma = dot(videoColor.rgb, vec3(0.2126, 0.7152, 0.0722)); // Standard sRGB Luma calculation
    
    // 6. Calculate the size of the dot for this cell
    // The dot's radius is based on its luminosity, scaled by the dotSize uniform
    float radius = (1.0 - luma) * dotSize * 0.01;
    
    // 7. Determine the dot shape (distance from the center of the cell)
    // grid_uv - 0.5 moves the origin to the center of the cell (-0.5 to 0.5)
    float dist = length(grid_uv - 0.5);
    
    // 8. Output the final color
    
    // Check if the current pixel is inside the dot's radius
    if (dist < radius) {
        if (colorMode) {
            // Color Mode (Dot is the color of the video)
            gl_FragColor = videoColor;
        } else {
            // Grayscale/Black & White Mode (Dot is black/white based on luma)
            // The background is the opposite color of the dot
            gl_FragColor = vec4(vec3(1.0 - luma), videoColor.a);
        }
    } else {
        // Pixel is outside the dot (the background)
        gl_FragColor = vec4(0.0, 0.0, 0.0, 0.0); // Transparent background
    }
}