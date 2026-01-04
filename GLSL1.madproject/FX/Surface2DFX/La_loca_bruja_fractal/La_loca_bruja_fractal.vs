void fxVsFunc() {
    mm_FragNormCoord = 
        (mm_TextureMatrix * vec4(mm_TexCoord0.xy, 0.0, 1.0)).xy;

    gl_Position = 
        mm_ModelViewProjectionMatrix * vec4(mm_Vertex.xy, 0.0, 1.0);
}
