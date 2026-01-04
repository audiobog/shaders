/*{
"CREDIT": "Elver Gonzales",
"DESCRIPTION": "Senoides moduladas por luminancia, con animación, contraste e inversión",
"TAGS": "sine,wave,animated,contrast,invert,luminance,effect",
"VSN": "1.0",
"INPUTS": [
    { "LABEL": "Lines", "NAME": "fx_line_count", "TYPE": "float", "MIN": 10.0, "MAX": 300.0, "DEFAULT": 45.0 },
    { "LABEL": "Amp", "NAME": "fx_amp", "TYPE": "float", "MIN": 0.0, "MAX": 0.2, "DEFAULT": 0.035 },
    { "LABEL": "Base Freq", "NAME": "fx_freq", "TYPE": "float", "MIN": 1.0, "MAX": 40.0, "DEFAULT": 10.0 },
    { "LABEL": "Variation", "NAME": "fx_freq_variation", "TYPE": "float", "MIN": 0.0, "MAX": 2.0, "DEFAULT": 1.0 },
    { "LABEL": "Line Width", "NAME": "fx_thickness", "TYPE": "float", "MIN": 0.001, "MAX": 0.03, "DEFAULT": 0.01 },
    { "LABEL": "Velocity", "NAME": "fx_speed", "TYPE": "float", "MIN": -10.0, "MAX": 10.0, "DEFAULT": 1.0 },
    { "LABEL": "Luma Boost", "NAME": "fx_luma_boost", "TYPE": "float", "MIN": 0.0, "MAX": 2.0, "DEFAULT": 1.0 },
    { "LABEL": "Contrast", "NAME": "fx_luma_contrast", "TYPE": "float", "MIN": 0.0, "MAX": 5.0, "DEFAULT": 1.0 },
    { "LABEL": "Invert", "NAME": "fx_invert_luma", "TYPE": "bool", "DEFAULT": 0, "FLAGS": "button" },
    { "LABEL": "Color", "NAME": "fx_color", "TYPE": "color", "DEFAULT": [0.0, 1.0, 0.0, 1.0] }
],
"GENERATORS": [
    {
        "NAME": "fx_animation_time",
        "TYPE": "time_base",
        "PARAMS": { "speed": "fx_speed", "reverse": false, "link_speed_to_global_bpm": true }
    }
]
}*/

float getAdjustedLuminance(vec3 color) {
    float luma = dot(color, vec3(0.299, 0.587, 0.114));
    luma *= fx_luma_boost;
    luma = clamp(luma, 0.0, 1.0);
    luma = 0.5 + (luma - 0.5) * fx_luma_contrast;
    luma = clamp(luma, 0.0, 1.0);
    if (fx_invert_luma) {
        luma = 1.0 - luma;
    }
    return luma;
}

vec4 fxColorForPixel(vec2 texCoord)
{
    float lines = fx_line_count;
    float result = 0.0;
    const int smoothRange = 4;

    for (int i = -smoothRange; i <= smoothRange; i++) {
        float currentLineY = floor(texCoord.y * lines) + float(i);
        float yLine = (currentLineY + 0.5) / lines;
        if (yLine < 0.0 || yLine > 1.0) continue;

        vec3 colorSample = FX_NORM_PIXEL(vec2(texCoord.x, yLine)).rgb;
        float luminance = getAdjustedLuminance(colorSample);

        float localFreq = fx_freq * (1.0 + fx_freq_variation * luminance);
        float phase = fx_animation_time * 2.0;
        float sineY = yLine + fx_amp * luminance * sin(texCoord.x * localFreq * 6.2831 + phase);

        float weight = 1.0 - abs(yLine - texCoord.y) * lines;
        weight = clamp(weight, 0.0, 1.0);

        float dist = abs(texCoord.y - sineY);
        float lineAlpha = smoothstep(fx_thickness, 0.0, dist);

        result += weight * lineAlpha;
    }

    result = clamp(result, 0.0, 1.0);
    return vec4(fx_color.rgb * result, result);
}
