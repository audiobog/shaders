/*{
"CREDIT": "Video Wave Lines Effect x R3S3T30",
"DESCRIPTION": "Creates animated sine wave lines based on video content luminance",
"TAGS": "video,waves,lines,effect",
"VSN": "1.0",
"INPUTS": [
{ "LABEL": "Speed", "NAME": "fx_speed", "TYPE": "float", "MIN" : 0.0, "MAX" : 5.0, "DEFAULT": 1.0 },
{ "LABEL": "Reverse", "NAME": "fx_reverse", "TYPE": "bool", "DEFAULT": 0, "FLAGS": "button" },
{ "LABEL": "Wave Frequency", "NAME": "fx_wave_freq", "TYPE": "float", "MIN" : 0.1, "MAX" : 20.0, "DEFAULT": 5.0 },
{ "LABEL": "Wave Amplitude", "NAME": "fx_wave_amp", "TYPE": "float", "MIN" : 0.0, "MAX" : 1.0, "DEFAULT": 0.3 },
{ "LABEL": "Line Thickness", "NAME": "fx_line_thickness", "TYPE": "float", "MIN" : 0.001, "MAX" : 0.1, "DEFAULT": 0.01 },
{ "LABEL": "Sensitivity", "NAME": "fx_sensitivity", "TYPE": "float", "MIN" : 0.1, "MAX" : 5.0, "DEFAULT": 1.0 },
{ "LABEL": "Video Mix", "NAME": "fx_video_mix", "TYPE": "float", "MIN" : 0.0, "MAX" : 1.0, "DEFAULT": 0.3 },
{ "LABEL": "Mode", "NAME": "fx_mode", "TYPE": "float", "MIN" : 0.0, "MAX" : 3.0, "DEFAULT": 0.0 },
{ "LABEL": "Color/Line Color", "NAME": "fx_line_color", "TYPE": "color", "DEFAULT": [ 1.0, 1.0, 1.0, 1.0 ] },
{ "LABEL": "Color/Background", "NAME": "fx_bg_color", "TYPE": "color", "DEFAULT": [ 0.0, 0.0, 0.0, 1.0 ] }
],
"GENERATORS": [
{
"NAME": "fx_animation_time",
"TYPE": "time_base",
"PARAMS": {"speed": "fx_speed", "reverse": "fx_reverse", "link_speed_to_global_bpm":true }
}
]
}*/

vec4 fxColorForPixel(vec2 texCoord)
{
    // Obtener el color del video de entrada usando la función correcta de MadMapper
    vec4 inputVideoColor = FX_NORM_PIXEL(texCoord);
    
    // Calcular luminancia del video
    float videoLuminance = dot(inputVideoColor.rgb, vec3(0.299, 0.587, 0.114));
    
    float mode = floor(fx_mode);
    
    if (mode == 0.0) {
        // Modo 1: Ondas horizontales basadas en luminancia del video
        float phase = videoLuminance * fx_sensitivity * 6.28 + fx_animation_time * fx_wave_freq;
        float waveOffset = fx_wave_amp * sin(phase);
        float linePos = 0.5 + waveOffset;
        float distance = abs(texCoord.y - linePos);
        float lineStrength = 1.0 - smoothstep(0.0, fx_line_thickness, distance);
        
        vec4 lineColor = mix(fx_bg_color, fx_line_color, lineStrength);
        return mix(lineColor, inputVideoColor, fx_video_mix);
        
    } else if (mode == 1.0) {
        // Modo 2: Múltiples ondas
        float wave1 = fx_wave_amp * sin((videoLuminance * fx_sensitivity + texCoord.x * 5.0) * 6.28 + fx_animation_time * fx_wave_freq);
        float wave2 = fx_wave_amp * 0.5 * sin((videoLuminance * fx_sensitivity * 2.0 + texCoord.x * 8.0) * 6.28 + fx_animation_time * fx_wave_freq * 1.5);
        float combinedWave = wave1 + wave2;
        float distance = abs(texCoord.y - (0.5 + combinedWave));
        float lineStrength = 1.0 - smoothstep(0.0, fx_line_thickness, distance);
        
        vec4 lineColor = mix(fx_bg_color, fx_line_color, lineStrength);
        return mix(lineColor, inputVideoColor, fx_video_mix);
        
    } else if (mode == 2.0) {
        // Modo 3: Ondas verticales
        float phase = videoLuminance * fx_sensitivity * 6.28 + fx_animation_time * fx_wave_freq;
        float waveOffset = fx_wave_amp * sin(phase);
        float distance = abs(texCoord.x - (0.5 + waveOffset));
        float lineStrength = 1.0 - smoothstep(0.0, fx_line_thickness, distance);
        
        vec4 lineColor = mix(fx_bg_color, fx_line_color, lineStrength);
        return mix(lineColor, inputVideoColor, fx_video_mix);
        
    } else {
        // Modo 4: Ripples radiales
        float centerDist = length(texCoord - vec2(0.5));
        float phase = (videoLuminance * fx_sensitivity + centerDist * 10.0) * 6.28 + fx_animation_time * fx_wave_freq;
        float ripple = fx_wave_amp * sin(phase);
        float targetRadius = 0.3 + ripple;
        float distance = abs(centerDist - targetRadius);
        float lineStrength = 1.0 - smoothstep(0.0, fx_line_thickness * 2.0, distance);
        
        vec4 lineColor = mix(fx_bg_color, fx_line_color, lineStrength);
        return mix(lineColor, inputVideoColor, fx_video_mix);
    }
}