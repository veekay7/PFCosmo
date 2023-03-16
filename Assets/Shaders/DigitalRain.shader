// Copyright 2018 VinTK. All Rights Reserved.
// Author: VinTK
Shader "Special/DigitalRain"
{
	Properties
	{
		_CloudTex0("Texture 1", 2D) = "white" {}
		_CloudTex1("Texture 2", 2D) = "white" {}
		_CloudTex2("Texture 3", 2D) = "white" {}
		_CloudTex3("Texture 4", 2D) = "white" {}
		_ScrollSpd0("Texture 1 Scroll Speed", Float) = 1.0
		_ScrollSpd1("Texture 2 Scroll Speed", Float) = 1.0
		_ScrollSpd2("Texture 3 Scroll Speed", Float) = 1.0
		_ScrollSpd3("Texture 4 Scroll Speed", Float) = 1.0
	}
	SubShader
	{
		Tags
		{ 
			"Queue"="Transparent" 
			"RenderType" = "Transparent" 
		}

		LOD 200

		Pass
		{
			Cull Front
			Blend SrcAlpha OneMinusSrcAlpha
			/*ZWrite Off*/
			Fog { Mode Off }

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_fog
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
				float2 uv0 : TEXCOORD0;
				float2 uv1 : TEXCOORD1;
				float2 uv2 : TEXCOORD2;
				float2 uv3 : TEXCOORD3;
				//UNITY_FOG_COORDS(1)
			};

			sampler2D _CloudTex0;
			sampler2D _CloudTex1;
			sampler2D _CloudTex2;
			sampler2D _CloudTex3;
			float4 _CloudTex0_ST;
			float4 _CloudTex1_ST;
			float4 _CloudTex2_ST;
			float4 _CloudTex3_ST;
			float _ScrollSpd0;
			float _ScrollSpd1;
			float _ScrollSpd2;
			float _ScrollSpd3;
			

			v2f vert (appdata v)
			{
				v2f o;

				o.vertex = UnityObjectToClipPos(v.vertex);

				o.uv0 = TRANSFORM_TEX(v.uv, _CloudTex0);
				o.uv1 = TRANSFORM_TEX(v.uv, _CloudTex1);
				o.uv2 = TRANSFORM_TEX(v.uv, _CloudTex2);
				o.uv3 = TRANSFORM_TEX(v.uv, _CloudTex3);
				
				//UNITY_TRANSFER_FOG(o, o.vertex);

				return o;
			}
			
			float4 frag (v2f i) : SV_Target
			{
				float scroll0 = -_ScrollSpd0 * _Time.x;
				float scroll1 = -_ScrollSpd1 * _Time.x;
				float scroll2 = -_ScrollSpd2 * _Time.x;
				float scroll3 = -_ScrollSpd3 * _Time.x;

				i.uv0 += float2(0.0, scroll0);
				i.uv1 += float2(0.0, scroll1);
				i.uv2 += float2(0.0, scroll2);
				i.uv3 += float2(0.0, scroll3);

				// sample the textures
				float4 col0 = tex2D(_CloudTex0, i.uv0);
				float4 col1 = tex2D(_CloudTex1, i.uv1);
				float4 col2 = tex2D(_CloudTex2, i.uv2);
				float4 col3 = tex2D(_CloudTex3, i.uv3);

				float3 temp1 = lerp(col0.rgb, col1.rgb, col1.a * 1.0);
				float3 temp2 = lerp(temp1, col2.rgb, col2.a * 1.0);
				float3 finalLerpColor = lerp(temp2, col3.rgb, col3.a * 1.0);
				float4 finalColor = fixed4(finalLerpColor.rgb, col3.a);

				//// apply fog
				//UNITY_APPLY_FOG(i.fogCoord, finalColor);
				
				return finalColor;
			}
			ENDCG
		}
	}
}
