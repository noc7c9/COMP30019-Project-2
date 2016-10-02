Shader "Custom/FloorShader" {
	Properties {
		_AmbientCoeff("Ambient Coefficient", Range(0,1)) = 1
		_DiffuseCoeff("Diffuse Coefficient", Range(0,1)) = 0
		_SpecularCoeff("Specular Coefficient", Range(0,1)) = 0
		_SpecularPower("Specular Power", Float) = 200
	}
	SubShader {
		Pass {
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			#define MAX_CORES 20

			uniform float3 _FloorColour;

			uniform float3 _CoreColours[MAX_CORES];
			uniform float3 _CorePositions[MAX_CORES];
			uniform int _NumCores;

			uniform float _AmbientCoeff;
			uniform float _DiffuseCoeff;
			uniform float _SpecularCoeff;
			uniform float _SpecularPower;

			struct vertIn {
				float4 vertex : POSITION;
				float4 normal : NORMAL;
				float4 color : COLOR;
			};

			struct vertOut {
				float4 vertex : SV_POSITION;
				float4 color : COLOR;
				float4 worldVertex : TEXCOORD0;
				float3 worldNormal : TEXCOORD1;
			};

			// source: https://www.opengl.org/sdk/docs/man4/html/fract.xhtml
			float2 fract(float2 v) {
				return v - floor(v);
			}

			// source: https://www.opengl.org/sdk/docs/man4/html/fwidth.xhtml
			float2 fwidth(float2 v) {
				return abs(ddx(v)) + abs(ddy(v));
			}

			// source: // http://www.madebyevan.com/shaders/grid
			float3 grid(float2 coord, float scale) {
				coord = coord * 1 / scale;
				float2 g = abs(fract(coord - 0.5) - 0.5) / fwidth(coord);
				float l = min(g.x, g.y);
				float v = 1.0 - min(l, 1.0);
				return float3(v, v, v);
			}

			vertOut vert(vertIn v) {
				vertOut o;
				o.color = v.color;

				// Convert Vertex position and corresponding normal into world coords
				// Note that we have to multiply the normal by the transposed inverse of the world 
				// transformation matrix (for cases where we have non-uniform scaling; we also don't
				// care about the "fourth" dimension, because translations don't affect the normal) 
				#if UNITY_VERSION >= 540
				float3x3 _WorldToObject = unity_WorldToObject;
				float4x4 _ObjectToWorld = unity_ObjectToWorld;
				#endif
				o.worldVertex = mul(_ObjectToWorld, v.vertex);
				o.worldNormal = normalize(mul(transpose((float3x3)_WorldToObject), v.normal.xyz));

				// Transform vertex in world coordinates to camera coordinates
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);

				return o;
			}

			fixed4 frag(vertOut v) : SV_Target {
				v.color.rgb = grid(v.worldVertex.xz, 4);

				float3 worldNormal = normalize(v.worldNormal);

				// Calculate ambient RGB intensities
				float Ka = _AmbientCoeff;
				float3 amb = v.color.rgb * UNITY_LIGHTMODEL_AMBIENT.rgb * Ka;

				// Sum up lighting calculations for each light (only diffuse/specular; ambient does not depend on the individual lights)
				float3 dif_and_spe_sum = float3(0.0, 0.0, 0.0);
				for (int i = 0; i < _NumCores; i++) {
					// Calculate diffuse RBG reflections, we save the results of L.N because we will use it again
					// (when calculating the reflected ray in our specular component)
					float fAtt = 1;
					float Kd = _DiffuseCoeff;
					float3 L = normalize(_CorePositions[i] - v.worldVertex.xyz);
					float LdotN = dot(L, worldNormal.xyz);
					float3 dif = fAtt * _CoreColours[i].rgb * Kd * v.color.rgb * saturate(LdotN);

					// Calculate specular reflections
					float Ks = _SpecularCoeff;
					float specN = _SpecularPower; // Values>>1 give tighter highlights
					float3 V = normalize(_WorldSpaceCameraPos - v.worldVertex.xyz);
					float3 H = (L + V) / length(L + V);
					float NdotH = dot(worldNormal, H);
					float3 R = (2 * LdotN * worldNormal) - L;
					float3 spe = fAtt * _CoreColours[i].rgb * Ks * pow(saturate(NdotH), specN);

					dif_and_spe_sum += dif + spe;
				}

				// Combine Phong illumination model components
				v.color.rgb = amb.rgb + dif_and_spe_sum;
				v.color.a = v.color.a;

				return v.color;
			}
			ENDCG
		}
	}
}
