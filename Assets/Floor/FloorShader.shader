Shader "Custom/FloorShader" {
	Properties{
		_GridBrightness("Grid Brightness", Range(0.01, 1)) = 0.15
		_TerritoryBrightness("Territory Brightness", Range(0, 1)) = 0.75
		_ShadowBrightness("Shadow Brightness", Range(0, 1)) = 0.1

		_CoreTerritoryRange("Core Territory Range", Float) = 15
	}
	SubShader {
		Pass {
			Tags{ "LightMode" = "ForwardBase" }

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_fwdbase
			
			#define MAX_CORES 20

			#include "UnityShaderVariables.cginc"
			#include "AutoLight.cginc"
			#include "Assets/Helpers/Helpers.cginc"

			uniform float _GridBrightness;
			uniform float _TerritoryBrightness;
			uniform float _ShadowBrightness;

			uniform float _CoreTerritoryRange;

			uniform float4 _CoreColours[MAX_CORES];
			uniform float3 _CorePositions[MAX_CORES];
			uniform int _NumCores;

			struct vertIn {
				float4 vertex : POSITION;
			};

			struct vertOut {
				float4 vertex : SV_POSITION;
				float4 worldVertex : TEXCOORD0;
				LIGHTING_COORDS(1, 2)
			};

			// source: https://www.opengl.org/sdk/docs/man4/html/fract.xhtml
			float2 fract(float2 v) {
				return v - floor(v);
			}

			// source: https://www.opengl.org/sdk/docs/man4/html/fwidth.xhtml
			float2 fwidth(float2 v) {
				return abs(ddx(v)) + abs(ddy(v));
			}

			// source: http://www.madebyevan.com/shaders/grid
			float4 grid(float2 coord, float scale) {
				coord = coord * 1 / scale;
				float2 g = abs(fract(coord - 0.5) - 0.5) / fwidth(coord);
				float l = min(g.x, g.y);
				float v = 1.0 - min(l, 1.0);
				return float4(v, v, v, 1);
			}

			vertOut vert(vertIn v) {
				vertOut o;

				o.worldVertex = worldVertex(v.vertex);
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				
				TRANSFER_VERTEX_TO_FRAGMENT(o);
				
				return o;
			}

			fixed4 frag(vertOut v) : SV_Target {
				// generate color based on grid
				float4 color = grid(v.worldVertex.xz, 4) * _GridBrightness;

				// find closest core (within territory range)
				// and tint the grid with that core's color
				float minDist = _CoreTerritoryRange;
				float3 tint = float3(0, 0, 0); // no tint by default
				for (int i = 0; i < _NumCores; i++) {
					// ignore y coord for distance
					float dist = length(v.worldVertex.xz - _CorePositions[i].xz);
					if (dist < minDist) {
						minDist = dist;

						// note: tint ignores grid brightness
						tint = _CoreColours[i] * (1 / _GridBrightness) * _TerritoryBrightness;
					}
				}
				float distFactor = minDist / _CoreTerritoryRange; // used for edge fading

				// fade out edges of tint
				float3 shadowTint = tint * (1 - distFactor); // untinted regions are black
				tint = 1 - (-tint + 1) * (1 - pow(distFactor, 5)); // untinted regions are white

				// apply tint
				color.rgb *= tint;

				// add shadows
				color.rgb += shadowTint * _ShadowBrightness * (1 - LIGHT_ATTENUATION(v));

				return color;
			}

			ENDCG
		}
	}
}
