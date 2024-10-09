Shader "Custom/LCHFramework/Refraction"
{
    Properties
    {
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _RefStrength("Reflection Strength", Range(0, 0.1)) = 0.05
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        zwrite off
        
        GrabPass {}
        
        CGPROGRAM
        #pragma surface surf nolight noambient alpha:fade

        #define EPSILON 1e-5f
        
        sampler2D _GrabTextrue;
        sampler2D _MainTex;
        float _RefStrength;

        struct Input
        {
            float4 color:COLOR;
            float4 screenPos;
            float2 uv_MainTex;
        };

        void surf (Input IN, inout SurfaceOutput o)
        {
            float4 ref = tex2D (_MainTex, IN.uv_MainTex);
            float3 screenUV = IN.screenPos.rgb / max(IN.screenPos.a, EPSILON);
            o.Emission = tex2D (_GrabTextrue, (screenUV.xy + ref.x * _RefStrength));
        }

        float4 Lightingnolight(SurfaceOutput s, float3 lightDir, float atten)
        {
            return float4(0, 0, 0, 1);
        }
        ENDCG
    }
    FallBack "Regacy Shaders/Transparent/Vertexlit"
}
