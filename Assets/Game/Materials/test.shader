Shader "Custom/test" {
	SubShader
	{
		Pass
	{
		CGPROGRAM
#pragma vertex vert
#pragma fragment frag

#include "UnityCG.cginc"

#define MAX_CORES 20

		uniform float3 _CoreColours[MAX_CORES];
	uniform float3 _CorePositions[MAX_CORES];
	uniform int _NumCores;

	uniform float _AmbientCoeff;
	uniform float _DiffuseCoeff;
	uniform float _SpecularCoeff;
	uniform float _SpecularPower;

	uniform sampler2D _MainTex;
	uniform sampler2D _NormalMapTex;

	struct vertIn
	{
		float4 vertex : POSITION;
		float4 normal : NORMAL;
		float4 tangent : TANGENT;
		float2 uv : TEXCOORD0;
	};

	struct vertOut
	{
		float4 vertex : SV_POSITION;
		float2 uv : TEXCOORD0;
		float4 worldVertex : TEXCOORD1;
		float3 worldNormal : TEXCOORD2;
		float3 worldTangent : TEXCOORD3;
		float3 worldBinormal : TEXCOORD4;
	};

	// Implementation of the vertex shader
	vertOut vert(vertIn v)
	{
		vertOut o;

		// Convert Vertex position and corresponding normal into world coords
		float4 worldVertex = mul(_Object2World, v.vertex);
		float3 worldNormal = normalize(mul(transpose((float3x3)_World2Object), v.normal.xyz));
		float3 worldTangent = normalize(mul(transpose((float3x3)_World2Object), v.tangent.xyz));
		float3 worldBinormal = normalize(cross(worldTangent, worldNormal));

		// Transform vertex in world coordinates to camera coordinates, and pass colour
		o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
		o.uv = v.uv;

		// Pass out the world vertex position and world normal to be interpolated
		// in the fragment shader (and utilised)
		o.worldVertex = worldVertex;
		o.worldNormal = worldNormal;
		o.worldTangent = worldTangent;
		o.worldBinormal = worldBinormal;

		return o;
	}

	// Implementation of the fragment shader
	fixed4 frag(vertOut v) : SV_Target
	{
		// Sample colour from texture (i.e. pixel colour before lighting applied)
		float4 surfaceColor = tex2D(_MainTex, v.uv);

		// Modify normal based on normal map (and bring into range -1 to 1)
		float3 bump = (tex2D(_NormalMapTex, v.uv) - float3(0.5, 0.5, 0.5)) * 2.0;
		float3 bumpNormal = (bump.x * normalize(v.worldTangent)) +
			(bump.y * normalize(v.worldBinormal)) +
			(bump.z * normalize(v.worldNormal));
		bumpNormal = normalize(bumpNormal);

		// Calculate ambient RGB intensities
		float Ka = _AmbientCoeff; // (May seem inefficient, but compiler will optimise)
		float3 amb = surfaceColor * UNITY_LIGHTMODEL_AMBIENT.rgb * Ka;

		// Sum up lighting calculations for each light (only diffuse/specular; ambient does not depend on the individual lights)
		float3 dif_and_spe_sum = float3(0.0, 0.0, 0.0);
		for (int i = 0; i < _NumCores; i++)
		{
			// Calculate diffuse RBG reflections, we save the results of L.N because we will use it again
			// (when calculating the reflected ray in our specular component)
			float fAtt = 1;
			float Kd = _DiffuseCoeff;
			float3 L = normalize(_CorePositions[i] - v.worldVertex.xyz);
			float LdotN = dot(L, bumpNormal);
			float3 dif = fAtt * _CoreColours[i].rgb * Kd * surfaceColor * saturate(LdotN);

			// Calculate specular reflections
			float Ks = _SpecularCoeff;
			float specN = _SpecularPower;
			float3 V = normalize(_WorldSpaceCameraPos - v.worldVertex.xyz);
			// Using Blinn-Phong approximation (note, this is a modification of normal Phong illumination):
			float3 H = normalize(V + L);
			float3 spe = fAtt * _CoreColours[i].rgb * Ks * pow(saturate(dot(bumpNormal, H)), specN);

			dif_and_spe_sum += dif + spe;
		}

		// Combine Phong illumination model components
		float4 returnColor = float4(0.0f, 0.0f, 0.0f, 0.0f);
		returnColor.rgb = amb.rgb + dif_and_spe_sum.rgb;
		returnColor.a = surfaceColor.a;

		return returnColor;
	}
		ENDCG
	}
	}
	FallBack "Diffuse"
}
