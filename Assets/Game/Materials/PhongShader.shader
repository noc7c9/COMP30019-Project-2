Shader "Custom/PhongShader" {
		SubShader
	{
		Pass
	{
		CGPROGRAM
#pragma vertex vert
#pragma fragment frag

#include "UnityCG.cginc"

	#define MAX_CORES 10

	uniform float3 _FloorColour;

	uniform float3 _CoreColours[MAX_CORES];
	uniform float3 _CorePositions[MAX_CORES];
	uniform int _NumCores;

	uniform float _AmbientCoeff;
	uniform float _DiffuseCoeff;
	uniform float _SpecularCoeff;
	uniform float _SpecularPower;

	struct vertIn
	{
		float4 vertex : POSITION;
		float4 normal : NORMAL;
		float4 color : COLOR;
	};

	struct vertOut
	{
		float4 vertex : SV_POSITION;
		float4 color : COLOR;
		float4 worldVertex : TEXCOORD0;
		float3 worldNormal : TEXCOORD1;
	};

	// Implementation of the vertex shader
	vertOut vert(vertIn v)
	{
		vertOut o;
		o.color = v.color;

		// Convert Vertex position and corresponding normal into world coords
		// Note that we have to multiply the normal by the transposed inverse of the world 
		// transformation matrix (for cases where we have non-uniform scaling; we also don't
		// care about the "fourth" dimension, because translations don't affect the normal) 
		o.worldVertex = mul(_Object2World, v.vertex);
		o.worldNormal = normalize(mul(transpose((float3x3)_World2Object), v.normal.xyz));

		// Transform vertex in world coordinates to camera coordinates
		o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);

		return o;
	}

	// Implementation of the fragment shader
	fixed4 frag(vertOut v) : SV_Target
	{
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