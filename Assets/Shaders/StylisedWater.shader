Shader "Water/Stylised"
{
    Properties
    {
        _Color("Color", Color) = (1.0, 1.0, 1.0, 1.0)
        _UDepth("Depth U", Range(0, 30)) = 18
        _VDepth("Depth V", Range(0, 30)) = 15
        _TimeMultU("Time U", Range(0, 10)) = 18
        _TimeMultV("Time V", Range(0, 10)) = 18
        _UScaler("U Scale", Range(0, 1)) = 0.1
        _VScaler("V Scale", Range(0, 1)) = 0.2
        _UMovement("U Movement", Range(0, 5)) = 2
        _VMovement("V Movement", Range(0, 5)) = 5
        _BlendColor("Blend Color", Color) = (1.0, 1.0, 1.0, 1.0)
        _Softness("Soft Factor", Range(0.01, 3.0)) = 1.0
        _FadeLimit("Fade Limit", Range(0.0, 1.0)) = 0.3
        _WaveSpeed("Wave Speed", Range(0.0, 100)) = 100
        _WaveOffset("Wave Offset", Range(0.0, 5)) = 2
        _MainTex("Texture", 2D) = "white" {}
    }

    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        LOD 200

        CGPROGRAM
        #pragma surface surf Standard vertex:vert alpha:fade nolightmap
        #pragma target 3.0

        struct Input
        {
            float2 uv_MainTex;
            float4 screenPos;
            float eyeDepth;
        };

        sampler2D_float _CameraDepthTexture;
        sampler2D _MainTex;

        fixed4 _Color;
        float _UDepth;
        float _VDepth;
        float _UScaler;
        float _VScaler;
        float _TimeMultU;
        float _TimeMultV;
        float _UMovement;
        float _VMovement;
        fixed4 _BlendColor;
        float _FadeLimit;
        float _Softness;
        float _WaveSpeed;
        float _WaveAmp;
        float _WaveOffset;

        void vert(inout appdata_full v, out Input o)
        {
            UNITY_INITIALIZE_OUTPUT(Input, o);
            COMPUTE_EYEDEPTH(o.eyeDepth);

            float3 v0 = mul(unity_ObjectToWorld, v.vertex).xyz;
            float phase0 = (_WaveAmp)* sin((_Time.y * _WaveSpeed) + (v0.x * _WaveOffset) + (v0.z * _WaveOffset));
            float phase0_1 = (_WaveAmp)* cos((_Time.y * _WaveSpeed) - (v0.x * -_WaveOffset) - (v0.z * -_WaveOffset));

            v.vertex.y += (phase0 + phase0_1) * _WaveAmp;
            v.vertex.x -= (phase0_1 * 1.75) * _WaveAmp;
            v.vertex.z += (phase0_1 * 3.5) * _WaveAmp;
        }


        void surf(Input IN, inout SurfaceOutputStandard o)
        {
            float u = IN.uv_MainTex.x + sin(_Time * _UMovement);
            float v = IN.uv_MainTex.y + sin(_Time * _VMovement);

            float newU = sin((_UDepth * u + (_Time.y * _TimeMultU)) * 0.25) * _UScaler + v;
            float newV = sin((_VDepth * v + (_Time.y * _TimeMultV)) * 0.25) * _VScaler + u;

            o.Albedo = tex2D(_MainTex, float2(newU, newV)) * _Color;
            o.Alpha = _Color.a;
            o.Metallic = 0;
            o.Smoothness = 0;

            //// Add a normal map to the properties/variable declaration and uncomment these lines for animated normals
            //float2 vTextureOffset1 = IN.uv_BumpMap / 4.0 + float2(_Time.y * _NormalMovement, _Time.y * //_NormalMovement);
            //float2 vTextureOffset2 = IN.uv_BumpMap / 4.0 + float2(sin(_Time.y * _NormalMovement), cos(_Time.y * //_NormalMovement));
            //o.Normal = UnpackNormal(tex2D(_BumpMap, vTextureOffset1)) + UnpackNormal(tex2D(_BumpMap, vTextureOffset2));

            float rawZ = SAMPLE_DEPTH_TEXTURE_PROJ(_CameraDepthTexture, UNITY_PROJ_COORD(IN.screenPos));
            float sceneZ = LinearEyeDepth(rawZ);
            float partZ = IN.eyeDepth;
            float fade = 1.0;
            if (rawZ > 0.0f)
                fade = saturate(_Softness * (sceneZ - partZ));

            if (fade < _FadeLimit)
                o.Albedo = o.Albedo.rgb * fade + _BlendColor.rgb * (1.0 - fade);
            }

        ENDCG
    }

    Fallback "Diffuse"
}
