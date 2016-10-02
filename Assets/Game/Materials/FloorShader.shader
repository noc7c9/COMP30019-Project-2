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

			#define MAX_CORES 20
			#define MAX_LIGHTS MAX_CORES

			#include "UnityCG.cginc"
			#include "PhongShader.cginc"

			uniform float4 _CoreColours[MAX_CORES];
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

				o.worldVertex = worldVertex(v.vertex);
				o.worldNormal = worldNormal(v.normal);

				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);

				return o;
			}

			fixed4 frag(vertOut v) : SV_Target {
				v.color.rgb = grid(v.worldVertex.xz, 4);

				float fAtt = 1;
				v.color.rgb = phongLighting(
					v.worldVertex, v.worldNormal, v.color,
					fAtt,
					_AmbientCoeff,
					_DiffuseCoeff,
					_SpecularCoeff, _SpecularPower,
					_CorePositions, _CoreColours, _NumCores
				);
				v.color.a = v.color.a;

				return v.color;
			}
			ENDCG
		}
	}
}
