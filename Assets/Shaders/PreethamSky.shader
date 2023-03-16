// Copyright 2018 VinTK. All Rights Reserved.
// Author: VinTK
Shader "Sky/PreethamSky"
{
	Properties
	{
		fRayleighCoefficient("Rayleigh Coefficient", Range(0.0, 4.0)) = 2.0
		fMieCoefficient("Mie Coefficient", Range(0.0, 1.0)) = 0.1
		fMieDirectionalG("Mie Directional G", Range(0.0, 1.0)) = 0.7
		fTurbidity("Turbidity", Range(1.0, 2.0)) = 1.5
		fSunIntensity("Sun Intensity", Float) = 1000.0
		fLuminance("Luminance", Float) = 0.05
	}

	SubShader
	{
		Tags { "RenderType" = "Opaque" "Queue" = "Geometry" }
		LOD 200

		Pass
		{
			Blend Off
			//ZWrite Off
			Cull Front
			Fog { Mode Off }

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"
			#include "PreethamCommon.cginc"

			struct v2f
			{
				float4 Pos : SV_POSITION;
				float3 WPos : TEXCOORD0;
				float3 WNormal : NORMAL;
				float3 WTangent : TANGENT;
				float3 WBinorm : BINORMAL;
				float3 SunPos : TEXCOORD1;
				float3 SunDir : TEXCOORD2;
				float Luminance : PSIZE;
			};

			uniform float3 v3LightPos;
			uniform float3 v3LightDir;
			uniform float fRayleighCoefficient;
			uniform float fMieCoefficient;
			uniform float fMieDirectionalG;
			uniform float fTurbidity;
			uniform float fSunIntensity;
			uniform float fLuminance;


			v2f vert(appdata_full IN)
			{
				v2f OUT = (v2f)0;

				OUT.Pos = UnityObjectToClipPos(IN.vertex);

				OUT.WPos = mul(unity_ObjectToWorld, IN.vertex.xyz);
				OUT.WNormal = mul(unity_ObjectToWorld, IN.normal.xyz);
				OUT.WTangent = mul(unity_ObjectToWorld, IN.tangent.xyz);
				OUT.WBinorm = normalize(cross(OUT.WNormal, OUT.WTangent) * IN.tangent.w);	// tangent.w is specific to Unity

				//// Calculates the fog coordinates and stores them back to the v2f struct
				//UNITY_TRANSFER_FOG(OUT, OUT.vertex);

				OUT.SunPos = v3LightPos;
				OUT.SunDir = v3LightDir;

				OUT.Luminance = IN.color.r;

				return OUT;
			}


			float4 frag(v2f IN) : SV_Target
			{
				float3 vPosDirection = normalize(IN.WPos - _WorldSpaceCameraPos);

				float sunFade = 1.0 - clamp(1.0 - exp(-(IN.SunPos.z / 500.0)), 0.0, 1.0);

				// Extinction (absorbtrion + out scattering)
				// Rayleigh coefficients
				float3 betaR = (8.0 * pow(M_PI, 3.0) * pow(pow(g_RefractiveAirIndex, 2.0) - 1.0, 2.0) * (6.0 + 3.0 * g_DepolatizationFactor)) /
					(3.0f * g_MoleculesPerUnit * pow(g_Lambda, float3(4.0, 4.0, 4.0)) * (6.0 - 7.0 * g_DepolatizationFactor)) * fRayleighCoefficient;

				// Mie coefficients
				float c = (0.2 * fTurbidity) * 10E-18;
				float3 betaM = (0.434 * c * M_PI * pow((2.0 * M_PI) / g_Lambda, (float3)(g_MieVCoefficient - 2.0)) * g_MieKCoefficient) * fMieCoefficient;

				// Optical length (Transmittance AKA Optical depth)
				// Cutoff angle at 90 degrees to avoid singularity in next formula
				float zenithAngle = acos(max(0.0, dot(g_GlobalUp, vPosDirection)));
				float sR = g_RayleighZenithLength / (cos(zenithAngle) + 0.15 * pow(93.885 - ((zenithAngle * 180.0) / M_PI), -1.253));
				float sM = g_MieZenithLength / (cos(zenithAngle) + 0.15 * pow(93.885 - ((zenithAngle * 180.0) / M_PI), -1.253));

				// Combined extinction factor
				float3 Fex = exp(-(betaR * sR + betaM * sM));

				// In scattering
				float cosTheta = dot(vPosDirection, IN.SunDir);

				float rPhase = RayleighPhase(cosTheta * 0.5 + 0.5);
				float3 betaRTheta = betaR * rPhase;

				float mPhase = HGPhase(cosTheta, fMieDirectionalG);
				float3 betaMTheta = betaM * mPhase;

				float sunE = SunIntensity(dot(IN.SunDir, g_GlobalUp), fSunIntensity);
				float3 Lin = pow(sunE * ((betaRTheta + betaMTheta) / (betaR + betaM)) * (1.0 - Fex), float3(1.5, 1.5, 1.5));
				Lin *= lerp(float3(1.0, 1.0, 1.0), pow(sunE * ((betaRTheta + betaMTheta) / (betaR + betaM)) * Fex, float3(1.0 / 2.0, 1.0 / 2.0, 1.0 / 2.0)), clamp(pow(1.0 - dot(g_GlobalUp, IN.SunDir), 5.0), 0.0, 1.0));

				// Night sky
				float theta = acos(vPosDirection.y);	// elevation --> y-axis, [-pi/2, pi/2]
				float phi = atan2(vPosDirection.y, vPosDirection.x);	// azimuth --> x-axis, [-pi/2, pi/2]
				float2 uv = float2(phi, theta) / float2(2.0 * M_PI, M_PI) + float2(0.5, 0.0);
				float3 L0 = float3(0.1, 0.1, 0.1) * Fex;
				//float3 L0 = tex2D(skySampler, uv).rgb + 0.1 * Fex;

				// Composition  + solar disc
				/*if (cosTheta > sunAngularDiameterCos)*/
				float sunDisk = smoothstep(g_SunAngularDiameterCos, g_SunAngularDiameterCos + 0.00002, cosTheta);
				if (vPosDirection.y > 0.0)
					L0 += (sunE * 19000.0 * Fex) * sunDisk;

				// Composition
				float3 scatteringColor = (Lin + L0) * 0.04;
				scatteringColor += float3(0.0, 0.001, 0.0025) * 0.3;

				// Tonemapping
				float3 whiteScale = 1.0 / Uncharted2Tonemap(float3(g_W, g_W, g_W));
				float3 tonemappedColor = Uncharted2Tonemap((log2(2.0 / pow(IN.Luminance, 4.0))) * scatteringColor);
				//float3 tonemappedColor = Uncharted2Tonemap((log2(2.0 / pow(fLuminance, 4.0))) * scatteringColor);
				float3 adjustedTonemappedColor = tonemappedColor * whiteScale;

				float3 color = (float3)0;
				color = pow(adjustedTonemappedColor, (float3)(1.0 / (1.2 + (1.2 * sunFade))));
				//color = pow(adjustedTonemappedColor, (float3)(1.0 / 1.2));

				float4 finalColor = float4(color.rgb, 1.0);
				return finalColor;
			}
			ENDCG
		}
	}
}
