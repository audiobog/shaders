/*{
    "CREDIT": "Beatline",
    "DESCRIPTION": "Controls Pan, Tilt and Intensity of any number of identical movers via DMX pixel mapping. Pixel map your mover ficture to this material and map as follows: Red=Tilt, Green=Tilt, Blue=Brightness",
    "TAGS": "movers, pan, tilt, intensity, dmx",
    "VSN": "1.4",
    "INPUTS": [
        { "LABEL": "Mover Count", "NAME": "mat_mover_count", "TYPE": "int", "DEFAULT": 6, "MIN": 1, "MAX": 32, "DESCRIPTION": "Number of movers (1–32)" },
		  { "LABEL": "Cycle offset", "NAME": "cycle_offset", "TYPE": "float", "MIN": 0.0, "MAX": 1.0, "DEFAULT": 0.0, "DESCRIPTION": "Position in the wave cycle (0–1)" },
	     { "LABEL": "Pan and Tilt fine adjust", "NAME": "mat_pan_tilt", "TYPE": "point2D", "MIN": [-1.0, -1.0], "MAX": [1.0, 1.0], "DEFAULT": [0.0, 0.0], "DESCRIPTION": "Fine adjustment for Pan and Tilt. Can be used as follow spot." },
        { "LABEL": "X sensitivity", "NAME": "mat_x_sens", "TYPE": "float", "MIN": -1.0, "MAX": 1.0, "DEFAULT": 0.1, "DESCRIPTION": "Pan fine adjust" },
        { "LABEL": "Y sensitivity", "NAME": "mat_y_sens", "TYPE": "float", "MIN": -1.0, "MAX": 1.0, "DEFAULT": 0.2, "DESCRIPTION": "Tilt fine adjust" },


        { "LABEL": "Pan control/Center", "NAME": "mat_pan_center", "TYPE": "float", "MIN": 0.0, "MAX": 1.0, "DEFAULT": 0.5, "DESCRIPTION": "Pan" },
        { "LABEL": "Pan control/Spread", "NAME": "mat_pan_spread", "TYPE": "float", "MIN": -0.5, "MAX": 0.5, "DEFAULT": 0.0, "DESCRIPTION": "Spread pan to go from parallel to fan." },
        { "LABEL": "Pan control/Amplitude", "NAME": "mat_pan_amp", "TYPE": "float", "MIN": 0.0, "MAX": 0.5, "DEFAULT": 0.0, "DESCRIPTION": "Pan wave amplitude" },
        { "LABEL": "Pan control/Phase spacing", "NAME": "mat_pan_spacing", "TYPE": "float", "MIN": -2.0, "MAX": 2.0, "DEFAULT": 0.0, "DESCRIPTION": "Spacing of pan animation between movers." },
        { "LABEL": "Pan control/Animation speed", "NAME": "mat_pan_speed", "TYPE": "float", "MIN": 0.0, "MAX": 4.0, "DEFAULT": 1.0, "DESCRIPTION": "Speed multiplier to BPM" },
        { "LABEL": "Pan control/BPM Sync", "NAME": "mat_pan_bpmsync", "TYPE": "bool", "DEFAULT": true, "FLAGS": "button" },

        { "LABEL": "Tilt control/Center", "NAME": "mat_tilt_center", "TYPE": "float", "MIN": 0.0, "MAX": 1.0, "DEFAULT": 0.5, "DESCRIPTION": "Tilt" },
        { "LABEL": "Tilt control/Spread", "NAME": "mat_tilt_spread", "TYPE": "float", "MIN": -1.0, "MAX": 1.0, "DEFAULT": 0.0, "DESCRIPTION": "Spread tilt to go from parallel to fan." },
        { "LABEL": "Tilt control/Amplitude", "NAME": "mat_tilt_amp", "TYPE": "float", "MIN": 0.0, "MAX": 0.5, "DEFAULT": 0.0, "DESCRIPTION": "Tilt wave amplitude" },
        { "LABEL": "Tilt control/Phase spacing", "NAME": "mat_tilt_spacing", "TYPE": "float", "MIN": -2.0, "MAX": 2.0, "DEFAULT": 0.0, "DESCRIPTION": "Spacing of pan animation between movers." },
        { "LABEL": "Tilt control/Animation speed", "NAME": "mat_tilt_speed", "TYPE": "float", "MIN": 0.0, "MAX": 4.0, "DEFAULT": 1.0, "DESCRIPTION": "Speed multiplier to BPM" },
        { "LABEL": "Tilt control/BPM Sync", "NAME": "mat_tilt_bpmsync", "TYPE": "bool", "DEFAULT": true, "FLAGS": "button" },

		  { "LABEL": "Intensity control/Base", "NAME": "mat_intensity_base", "TYPE": "float", "MIN": 0.0, "MAX": 1.0, "DEFAULT": 0.5, "DESCRIPTION": "Intensity" },
        { "LABEL": "Intensity control/Amplitude", "NAME": "mat_intensity_amp", "TYPE": "float", "MIN": 0.0, "MAX": 0.5, "DEFAULT": 0.2, "DESCRIPTION": "Intensity wave amplitude" },
        { "LABEL": "Intensity control/Phase spacing", "NAME": "mat_intensity_spacing", "TYPE": "float", "MIN": -3.0, "MAX": 3.0, "DEFAULT": 0.0, "DESCRIPTION": "Spacing of Intensity animation between movers." },
        { "LABEL": "Intensity control/Animation speed", "NAME": "mat_intensity_speed", "TYPE": "float", "MIN": 0.0, "MAX": 4.0, "DEFAULT": 1.0, "DESCRIPTION": "Speed multiplier to BPM" },
        { "LABEL": "Intensity control/BPM Sync", "NAME": "mat_intensity_bpmsync", "TYPE": "bool", "DEFAULT": true, "FLAGS": "button" },
        { "Label": "Intensity control/Strobe Frequency", "NAME": "mat_strobe_frequency", "TYPE": "float", "MIN": 0.0, "MAX": 8.0, "DEFAULT": 1.0 },
        { "Label": "Intensity control/Strobe Duty", "NAME": "mat_strobe_duty", "TYPE": "float", "MIN": 0.0, "MAX": 1.0, "DEFAULT": 0.0 },
   ],
    "GENERATORS": [
        {"NAME": "pan_animation_time", "TYPE": "time_base", "PARAMS": {"speed": "mat_pan_speed", "bpm_sync": "mat_pan_bpmsync", "speed_curve": 2, "link_speed_to_global_bpm":true}},
        {"NAME": "tilt_animation_time", "TYPE": "time_base", "PARAMS": {"speed": "mat_tilt_speed", "bpm_sync": "mat_tilt_bpmsync", "speed_curve": 2, "link_speed_to_global_bpm":true}},
        {"NAME": "intensity_animation_time", "TYPE": "time_base", "PARAMS": {"speed": "mat_intensity_speed", "bpm_sync": "mat_intensity_bpmsync", "speed_curve": 2, "link_speed_to_global_bpm":true}},
        {"NAME": "strob_position", "TYPE": "time_base", "PARAMS": {"speed": "mat_speed", "strob": 0, "bpm_sync": "mat_intensity_bpmsync", "speed_curve":2, "link_speed_to_global_bpm":true}},
    ]
}*/

#include "MadCommon.glsl"

in float adjusted_pos;

const float TWO_PI = 6.28318530718;

float cospos(float position){
   return cos(TWO_PI * position);
}

float safeSpacing(float v, float count) {
    return (abs(v) < 0.0001) ? 1e9 : (count * v);
}

vec4 materialColorForPixel(vec2 texCoord)
{
    int count = clamp(mat_mover_count, 1, 32);
    float moverIndex = floor(texCoord.x * float(count));
    moverIndex = clamp(moverIndex, 0.0, float(count - 1));
	 float panSpacing = safeSpacing(mat_pan_spacing, count);
	 float tiltSpacing = safeSpacing(mat_tilt_spacing, count);
	 float intensitySpacing = safeSpacing(mat_intensity_spacing, count);

    float sym = (texCoord.x * 2.0 - 1.0); // -1 left → 0 center → +1 right

    float panSpread = sym * mat_pan_spread;
    float panAdjust = clamp(mat_pan_center + (mat_pan_tilt.x * mat_x_sens) + panSpread, 0.0, 1.0);
    float panAmp = min(min(panAdjust, 1 - panAdjust), mat_pan_amp);
	 float pan  =  panAdjust + cospos(cycle_offset + pan_animation_time  + (moverIndex /  panSpacing )) * panAmp;

	 float tiltSpread = sym * mat_tilt_spread;
    float tiltAdjust = clamp(mat_tilt_center + (mat_pan_tilt.y * mat_y_sens) + tiltSpread, 0.0, 1.0);
    float tiltAmp = min(min(tiltAdjust, 1 - tiltAdjust), mat_tilt_amp);
    float tilt = tiltAdjust + cospos(cycle_offset + tilt_animation_time + (moverIndex / tiltSpacing )) * tiltAmp;

    float intensityAmp = min(min(mat_intensity_base, 1 - mat_intensity_base), mat_intensity_amp);
    float intensity = mat_intensity_base + cospos(cycle_offset + intensity_animation_time + (moverIndex / intensitySpacing )) * intensityAmp;
  
    float phase = fract(TIME * mat_strobe_frequency);
    float v = phase < mat_strobe_duty ? 1.0 : 0.0;

	 return vec4(pan, tilt, intensity * v, 1.0);
}






