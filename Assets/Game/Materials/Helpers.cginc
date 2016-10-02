﻿// Note that we have to multiply the normal by the transposed inverse of the
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
