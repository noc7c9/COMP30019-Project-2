Shader "Custom/PhongShader" {
	Properties{
		_AttenuationFactor("Attenuation Factor", Range(0, 1)) = 0.5
		_AmbientCoeff("Ambient Coefficient", Range(0, 1)) = 0.5
		_DiffuseCoeff("Diffuse Coefficient", Range(0, 1)) = 0.5
		_SpecularCoeff("Specular Coefficient", Range(0, 1)) = 0.5
		_SpecularPower("Specular Power", Range(0, 100)) = 1
	}
	SubShader {
		Pass {
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#define MAX_LIGHTS 20

			#include "UnityShaderVariables.cginc"
			#include "Assets/Helpers/PhongShader.cginc"

			uniform float _AttenuationFactor;
			uniform float _AmbientCoeff;
			uniform float _DiffuseCoeff;
			uniform float _SpecularCoeff;
			uniform float _SpecularPower;

			uniform float4 _CoreColours[MAX_LIGHTS];
			uniform float3 _CorePositions[MAX_LIGHTS];
			uniform int _NumCores;

			struct vertIn {
				float4 vertex : POSITION;
				float4 color : COLOR;
			};

			struct vertOut {
				float4 vertex : SV_POSITION;
				float4 color : COLOR;
				float4 worldVertex : TEXCOORD0;
				float3 worldNormal : TEXCOORD1;
			};

			vertOut vert(vertIn v) {
				vertOut o;

				o.worldVertex = worldVertex(v.vertex);
				o.worldNormal = worldNormal(v.vertex);
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.color = v.color;
				
				return o;
			}

			fixed4 frag(vertOut v) : SV_Target {
				v.color.rgb = phongLighting(
					v.worldVertex, v.worldNormal, v.color,
					_AttenuationFactor,
					_AmbientCoeff,
					_DiffuseCoeff,
					_SpecularCoeff, _SpecularPower,
					_CorePositions, _CoreColours, _NumCores
				);

				return v.color;
			}

			ENDCG
		}
	}
	Fallback "Diffuse"
}
