Shader "Custom/LCHFramework/Dissolve"
{
    Properties
    {
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _NoiseTex ("NoiseTex", 2D) = "white" {}
        _Cut ("Alpha Cut", Range(0,1)) = 0
        [HDR]_OutColor ("OutColor", Color) = (1,1,1,1)
        _OutThinkness ("_OutThinkness", Range(1,1.5)) = 1.15
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent"}

        CGPROGRAM
        #pragma surface surf Lambert alpha:fade

        sampler2D _MainTex;
        sampler2D _NoiseTex;
        float _Cut;
        float4 _OutColor;
        float _OutThinkness;
        
        struct Input
        {
            float2 uv_MainTex;
            float2 uv_NoiseTex;
        };

        void surf (Input IN, inout SurfaceOutput o)
        {
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex);
            float4 noise = tex2D (_NoiseTex, IN.uv_NoiseTex);
            o.Albedo = c.rgb;

            float alpha = _Cut <= noise.r ? 1 : 0;
            float outline = _Cut * _OutThinkness <= noise.r ? 0 : 1;
            o.Emission = outline * _OutColor.rgb;
            o.Alpha = alpha;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
