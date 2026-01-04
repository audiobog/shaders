vec4 applyLighting(vec4 media_color)
{
    // Lighting mode enabled
    if (mm_LightingEnabled)
    {
        vec4 final_color = mm_AmbientColor * media_color;

        vec3 L = normalize(mm_LightDir);

        vec3 N = normalize(mm_vNormal);
        float lambertTerm = dot(N,L);

        if(lambertTerm > 0.0)
        {
            float attenuation = 1.;
            if (mm_LightAttenuated) {
                attenuation = mm_Attenuation;
            }

            if (mm_LightSpot)
            {
                float clampedCosine = max(0.0, dot(-L, mm_LightSpotDir));
                float maxCosine = cos(radians(mm_LightSpotCutoff));
                if (clampedCosine < maxCosine) { // outside of spotlight cone
                    attenuation = 0.0;
                } else {
                    attenuation = attenuation * pow(1.-((1.-clampedCosine) / (1.-maxCosine)), mm_LightSpotExponent);
                }

                if (mm_HasShadowMap)
                {
                    if (attenuation > 0.0 && mm_VertexToLightPosition.w > 0.0)
                    {
                        vec4 shadowCoordinateWdivide = mm_VertexToLightPosition / mm_VertexToLightPosition.w;

                        // Used to lower moire pattern and self-shadowing
                        shadowCoordinateWdivide.z -= mm_LightShadowMapOffset;

                        attenuation = attenuation * mm_antialiasedShadowMap(mm_LightShadowMap, 1./mm_LightShadowMapSize, shadowCoordinateWdivide, mm_LightShadowMapSamples);
                    }
                }
            }

            vec4 light_color = mm_LightColor * lambertTerm * media_color;
            final_color += light_color * attenuation;
        }

        return final_color;
    } else {
        return media_color;
    }
}

vec4 fxColorForPixel(vec2 mm_FragNormCoord)
{
    vec4 media_color = FX_NORM_PIXEL(mm_FragNormCoord);
    media_color.rgb *= mm_ModulationColor.rgb;
    return applyLighting(media_color);
}
