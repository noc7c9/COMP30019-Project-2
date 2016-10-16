Shader "Custom/LaserShader" {
	Properties{
	}
	SubShader {
		Pass {
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityShaderVariables.cginc"

			struct vertIn {
				float4 vertex : POSITION;
				float4 color : COLOR;
			};

			struct vertOut {
				float4 vertex : SV_POSITION;
				float4 color : COLOR;
			};

			vertOut vert(vertIn v) {
				vertOut o;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.color = v.color;
				return o;
			}

			fixed4 frag(vertOut v) : SV_Target {
				return v.color;
			}

			ENDCG
		}
	}
}
