in vec2 posInQuad;

vec4 fxColorForPixel(vec2 mm_FragNormCoord)
{
    if (fx_restrict) {
        #ifdef SURFACE_IS_quad
            if (posInQuad.x<-1 || posInQuad.x>1  || posInQuad.y<-1 || posInQuad.y>1) {
                return vec4(0,0,0,0);
            }
        #endif
        #ifdef SURFACE_IS_triangle
            if (posInQuad.y<-1 || posInQuad.y>1) {
                return vec4(0,0,0,0);
            } else {
                // Triangle width at Y==-1 => 1, Triangle width at Y==1 => 0
                float triangleWidthAtY = (1 - posInQuad.y)/2;
                if (posInQuad.x<-triangleWidthAtY || posInQuad.x>triangleWidthAtY) {
                    return vec4(0,0,0,0);
                }
            }
        #endif
        #ifdef SURFACE_IS_circle
            if (length(posInQuad)>sqrt(2)) {
                return vec4(0,0,0,0);
            }
        #endif
    } 

    vec4 out_color = FX_NORM_PIXEL(mm_FragNormCoord);
    return out_color;
}
