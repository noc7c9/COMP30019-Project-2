/**
 * Helpers
 */

// Note that we have to multiply the normal by the transposed inverse of the
// world transformation matrix (for cases where we have non-uniform scaling; we
// also don't care about the "fourth" dimension, because translations don't
// affect the normal)

float4 worldVertex(float4 vertex) {
#if UNITY_VERSION >= 540
	return mul(unity_ObjectToWorld, vertex);
#else
	return mul(_ObjectToWorld, vertex);
#endif
}

float3 worldNormal(float4 normal) {
#if UNITY_VERSION >= 540
	return normalize(mul(transpose((float3x3)unity_WorldToObject), normal.xyz));
#else
	return normalize(mul(transpose((float3x3)_WorldToObject), normal.xyz));
#endif
}

/**
 * Actual Phong Shaders
 */

#if !MAX_LIGHTS
#define MAX_LIGHTS 10
#endif

float3 calcAmbientLighting(float4 colour, float ambientCoeff) {
	return colour.rgb * UNITY_LIGHTMODEL_AMBIENT.rgb * ambientCoeff;
}

float3 calcDiffuseLighting(
		float4 worldVertex, float3 worldNormal, float4 colour,
		float attenuationFactor,
		float diffuseCoeff,
		float3 lightPosition, float4 lightColour) {

	float3 L = normalize(lightPosition - worldVertex.xyz);
	float LdotN = dot(L, worldNormal.xyz);

	return attenuationFactor * lightColour.rgb * diffuseCoeff
		* colour.rgb * saturate(LdotN);
}

float3 calcSpecularLighting(
		float4 worldVertex, float3 worldNormal, float4 colour,
		float attenuationFactor,
		float specularCoeff, float specularPower,
		float3 lightPosition, float4 lightColour) {

	float3 L = normalize(lightPosition - worldVertex.xyz);
	float LdotN = dot(L, worldNormal.xyz);
	float3 V = normalize(_WorldSpaceCameraPos - worldVertex.xyz);
	float3 H = (L + V) / length(L + V);
	float NdotH = dot(worldNormal, H);
	float3 R = (2 * LdotN * worldNormal) - L;

	return attenuationFactor * lightColour.rgb * specularCoeff
		* pow(saturate(NdotH), specularPower);
}

float3 phongLighting(
		float4 worldVertex, float3 worldNormal, float4 colour,
		float attenuationFactor,
		float ambientCoeff,
		float diffuseCoeff,
		float specularCoeff, float specularPower,
		float3 lightPositions[MAX_LIGHTS], float4 lightColours[MAX_LIGHTS], int numOfLights) {

	worldNormal = normalize(worldNormal);

	float3 lighting = calcAmbientLighting(colour, ambientCoeff);

	for (int i = 0; i < numOfLights; i++) {
		float3 diffuse = calcDiffuseLighting(
			worldVertex, worldNormal, colour,
			attenuationFactor,
			diffuseCoeff,
			lightPositions[i], lightColours[i]
		);

		float3 specular = calcSpecularLighting(
			worldVertex, worldNormal, colour,
			attenuationFactor,
			specularCoeff, specularPower,
			lightPositions[i], lightColours[i]
		);

		lighting += diffuse + specular;
	}

	return lighting;
}
