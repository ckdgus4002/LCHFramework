Shader "Custom/LCHFramework/Cloaking"
{
    Properties
    {
        _MainTex("Albedo Texture", 2D) = "white" {}

        // 빛을 굴절시키는데 사용되는 노말맵. 기본 텍스처로는 범프맵을 할당한다.
        _NormalMap("Normal Map", 2D) = "bump" {}

        _Opacity("Opacity", Range(0, 1)) = 0.1

        // 빛의 굴절 효과를 얼마나 할 것인지를 설정.
        _DeformIntensity("Deform by Normal Intensity", Range(0, 3)) = 1

        // 림라이팅 사용.
        _RimPow("Rim Power", int) = 3

        // 외각선의 색깔 효과.
        _RimColor("Rim Color", Color) = (0, 1, 1, 1)
    }

    SubShader
    {
        // Queue: 뒷 배경을 투명하게 함.
        Tags { "Queue" = "Transparent" "RenderType"="Opaque" }

        // 카메라의 zBuffer 를 덮어쓰지 않게 함.
        zwrite off

        // GrabPass 를 사용하면 모델이 그려지기 전의 모습을 가져올 수 있다.
        GrabPass {}

        CGPROGRAM

        // surface surf {커스라이팅모델}
        // CloakingLight: 클로킹을 사용할 땐 빛의 용량을 안받는게 편함. 그림자가 져야하는지? 주변 빛의 영향을 받아야하는지? 를 계산 안하기 위해서.
        // noambient: 엠비언트 라이팅에 영향을 받지 않게 함.
        // novertexlights noforwardadd: 포워드 라이팅에서 라이트 프로브의 영향을 받지 않게 됨.
        #pragma surface surf CloakingLight noambient novertexlights noforwardadd
        #pragma target 3.0

        // GrabPass를 사용하기 때문에 예약된 변수 _GrabTexture 를 사용해야함.
        sampler2D _GrabTexture;

        sampler2D _MainTex;
        sampler2D _NormalMap;

        float _Opacity;
        float _DeformIntensity;
        float _RimPow;
        float3 _RimColor;

        // Surface 쉐이더의 입력으로 들어갈 데이터 구조
        struct Input
        {
            // GrabPass 를 통해 잡은 Texture 를 샘플링 하기 위해 사용.
            // 모델을 그리기 전에, 그 모델이 있던 위치가 배경 상에 어떤 위치인지 알고 그 곳에 있던 픽셀을 가져와서 모델에 사용.
            float4 screenPos;

            float2 uv_MainTex;
            float2 uv_NormalMap;

            // 뷰의 방향(=카메라의 방향)
            float3 viewDir;
        };

        void surf (Input IN, inout SurfaceOutput o)
        {
            o.Normal = UnpackNormal(tex2D(_NormalMap, IN.uv_NormalMap));

            float4 color = tex2D(_MainTex, IN.uv_MainTex);

            // 현재 버텍스가 스크린 텍스처의 어떤 uv 좌에 대응되는지를 가져옴.
            // 호모지니어스 좌표계에서 xyzw 에서 w 가 1 호모지니어스 좌표계로 변경함.
            float2 uv_screen = IN.screenPos.xyz / IN.screenPos.w;

            // 변환된 좌표계에 대응되는 Color를 가져옴.
            // o.Normal.xyz * _DeformIntensity: 노말에 의해 Color를 어긋나게 만듬.
            fixed3 mappingScreenColor = tex2D(_GrabTexture, uv_screen + o.Normal.xyz * _DeformIntensity);

            // saturate: 0 ~ 1 사이의 값으로 잘림.
            // saturate(dot(IN.viewDir, o.Normal)): 보는 사람의 방향과 표면의 방향이 엇나갈수록 0 에 가까워짐.
            // 물체의 외각선이 밝아지고 아닌 부분은 0이 됨.
            float rimBrightness = 1 - saturate(dot(IN.viewDir, o.Normal));

            // pow: 하면 할수록 작아지는 특성이 있음.
            rimBrightness = pow(rimBrightness, _RimPow);

            // Opacity값이 1에 가까울수록 mappingScreenColor는 0에 수렴.
            // o.Emission: 스스로 빛을 냄(=주변이 어두우면 안보임, 광원이 없으면 뒷배경을 보여줌.)
            // _RimColor * rimBrightness: 외각선으로 갈 수록 값이 큼. (=외각선을 따라서 빛을 내는 효과.)
            o.Emission = mappingScreenColor * (1 - _Opacity) + _RimColor * rimBrightness;
            o.Albedo = color.rgb;
        }

        // s: Surface Shading을 통해 처리를 끝낸 라이팅을 제외하고 다 그려진 픽셀.
        // lightDir: 라이팅이 오는 방향.
        // atten: 명도(=빛의 세기).
        fixed4 LightingCloakingLight(SurfaceOutput s, float3 lightDir, float atten) {
            // _LightColor0: 예약어. 씬 상에 존재하는 첫번째 라이트의 컬러(=주변 라이트의 컬러).
            // 1: 알파값은 가져온 뒤의 배경을 다시 배경과 섞는건 말이 안됨.
            return fixed4(s.Albedo * _Opacity * _LightColor0, 1);
        }

        ENDCG
    }
    FallBack "Diffuse"
}
