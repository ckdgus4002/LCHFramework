Shader "Custom/LCHFramework/Unlit"
{
    Properties
    {
        _Color ("Base Color", Color) = (1, 1, 1, 1)
    }

    SubShader
    {
        Pass
        {
            CGPROGRAM
                #include "UnityCG.cginc"

                #pragma vertex vert
                #pragma fragment frag

                fixed4 _Color;

                struct vertextInput {
                    float3 positionOnObjectSpace : POSITION;
                };

                struct fragmentInput {
                    float4 positionOnClipSpace : SV_POSITION;
                };

                fragmentInput vert(vertextInput input) {
                    float4 positionOnClipSpace = UnityObjectToClipPos(input.positionOnObjectSpace);

                    fragmentInput output;
                    output.positionOnClipSpace = positionOnClipSpace;

                    return output;
                }

                fixed4 frag(fragmentInput input) : SV_TARGET {
                    return _Color;
                }

            ENDCG
        }
    }
}
