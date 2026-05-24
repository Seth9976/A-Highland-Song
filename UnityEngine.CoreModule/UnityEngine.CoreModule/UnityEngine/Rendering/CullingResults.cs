using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine.Bindings;

namespace UnityEngine.Rendering
{
	// Token: 0x020003F7 RID: 1015
	[NativeHeader("Runtime/Scripting/ScriptingCommonStructDefinitions.h")]
	[NativeHeader("Runtime/Graphics/ScriptableRenderLoop/ScriptableCulling.h")]
	[NativeHeader("Runtime/Export/RenderPipeline/ScriptableRenderPipeline.bindings.h")]
	public struct CullingResults : IEquatable<CullingResults>
	{
		// Token: 0x0600226E RID: 8814
		[FreeFunction("ScriptableRenderPipeline_Bindings::GetLightIndexCount")]
		[MethodImpl(4096)]
		private static extern int GetLightIndexCount(IntPtr cullingResultsPtr);

		// Token: 0x0600226F RID: 8815
		[FreeFunction("ScriptableRenderPipeline_Bindings::GetReflectionProbeIndexCount")]
		[MethodImpl(4096)]
		private static extern int GetReflectionProbeIndexCount(IntPtr cullingResultsPtr);

		// Token: 0x06002270 RID: 8816
		[FreeFunction("FillLightAndReflectionProbeIndices")]
		[MethodImpl(4096)]
		private static extern void FillLightAndReflectionProbeIndices(IntPtr cullingResultsPtr, ComputeBuffer computeBuffer);

		// Token: 0x06002271 RID: 8817
		[FreeFunction("FillLightAndReflectionProbeIndices")]
		[MethodImpl(4096)]
		private static extern void FillLightAndReflectionProbeIndicesGraphicsBuffer(IntPtr cullingResultsPtr, GraphicsBuffer buffer);

		// Token: 0x06002272 RID: 8818
		[FreeFunction("GetLightIndexMapSize")]
		[MethodImpl(4096)]
		private static extern int GetLightIndexMapSize(IntPtr cullingResultsPtr);

		// Token: 0x06002273 RID: 8819
		[FreeFunction("GetReflectionProbeIndexMapSize")]
		[MethodImpl(4096)]
		private static extern int GetReflectionProbeIndexMapSize(IntPtr cullingResultsPtr);

		// Token: 0x06002274 RID: 8820
		[FreeFunction("FillLightIndexMapScriptable")]
		[MethodImpl(4096)]
		private static extern void FillLightIndexMap(IntPtr cullingResultsPtr, IntPtr indexMapPtr, int indexMapSize);

		// Token: 0x06002275 RID: 8821
		[FreeFunction("FillReflectionProbeIndexMapScriptable")]
		[MethodImpl(4096)]
		private static extern void FillReflectionProbeIndexMap(IntPtr cullingResultsPtr, IntPtr indexMapPtr, int indexMapSize);

		// Token: 0x06002276 RID: 8822
		[FreeFunction("SetLightIndexMapScriptable")]
		[MethodImpl(4096)]
		private static extern void SetLightIndexMap(IntPtr cullingResultsPtr, IntPtr indexMapPtr, int indexMapSize);

		// Token: 0x06002277 RID: 8823
		[FreeFunction("SetReflectionProbeIndexMapScriptable")]
		[MethodImpl(4096)]
		private static extern void SetReflectionProbeIndexMap(IntPtr cullingResultsPtr, IntPtr indexMapPtr, int indexMapSize);

		// Token: 0x06002278 RID: 8824
		[FreeFunction("ScriptableRenderPipeline_Bindings::GetShadowCasterBounds")]
		[MethodImpl(4096)]
		private static extern bool GetShadowCasterBounds(IntPtr cullingResultsPtr, int lightIndex, out Bounds bounds);

		// Token: 0x06002279 RID: 8825
		[FreeFunction("ScriptableRenderPipeline_Bindings::ComputeSpotShadowMatricesAndCullingPrimitives")]
		[MethodImpl(4096)]
		private static extern bool ComputeSpotShadowMatricesAndCullingPrimitives(IntPtr cullingResultsPtr, int activeLightIndex, out Matrix4x4 viewMatrix, out Matrix4x4 projMatrix, out ShadowSplitData shadowSplitData);

		// Token: 0x0600227A RID: 8826
		[FreeFunction("ScriptableRenderPipeline_Bindings::ComputePointShadowMatricesAndCullingPrimitives")]
		[MethodImpl(4096)]
		private static extern bool ComputePointShadowMatricesAndCullingPrimitives(IntPtr cullingResultsPtr, int activeLightIndex, CubemapFace cubemapFace, float fovBias, out Matrix4x4 viewMatrix, out Matrix4x4 projMatrix, out ShadowSplitData shadowSplitData);

		// Token: 0x0600227B RID: 8827 RVA: 0x00039C90 File Offset: 0x00037E90
		[FreeFunction("ScriptableRenderPipeline_Bindings::ComputeDirectionalShadowMatricesAndCullingPrimitives")]
		private static bool ComputeDirectionalShadowMatricesAndCullingPrimitives(IntPtr cullingResultsPtr, int activeLightIndex, int splitIndex, int splitCount, Vector3 splitRatio, int shadowResolution, float shadowNearPlaneOffset, out Matrix4x4 viewMatrix, out Matrix4x4 projMatrix, out ShadowSplitData shadowSplitData)
		{
			return CullingResults.ComputeDirectionalShadowMatricesAndCullingPrimitives_Injected(cullingResultsPtr, activeLightIndex, splitIndex, splitCount, ref splitRatio, shadowResolution, shadowNearPlaneOffset, out viewMatrix, out projMatrix, out shadowSplitData);
		}

		// Token: 0x17000651 RID: 1617
		// (get) Token: 0x0600227C RID: 8828 RVA: 0x00039CB2 File Offset: 0x00037EB2
		public unsafe NativeArray<VisibleLight> visibleLights
		{
			get
			{
				return this.GetNativeArray<VisibleLight>((void*)this.m_AllocationInfo->visibleLightsPtr, this.m_AllocationInfo->visibleLightCount);
			}
		}

		// Token: 0x17000652 RID: 1618
		// (get) Token: 0x0600227D RID: 8829 RVA: 0x00039CD0 File Offset: 0x00037ED0
		public unsafe NativeArray<VisibleLight> visibleOffscreenVertexLights
		{
			get
			{
				return this.GetNativeArray<VisibleLight>((void*)this.m_AllocationInfo->visibleOffscreenVertexLightsPtr, this.m_AllocationInfo->visibleOffscreenVertexLightCount);
			}
		}

		// Token: 0x17000653 RID: 1619
		// (get) Token: 0x0600227E RID: 8830 RVA: 0x00039CEE File Offset: 0x00037EEE
		public unsafe NativeArray<VisibleReflectionProbe> visibleReflectionProbes
		{
			get
			{
				return this.GetNativeArray<VisibleReflectionProbe>((void*)this.m_AllocationInfo->visibleReflectionProbesPtr, this.m_AllocationInfo->visibleReflectionProbeCount);
			}
		}

		// Token: 0x0600227F RID: 8831 RVA: 0x00039D0C File Offset: 0x00037F0C
		private unsafe NativeArray<T> GetNativeArray<T>(void* dataPointer, int length) where T : struct
		{
			return NativeArrayUnsafeUtility.ConvertExistingDataToNativeArray<T>(dataPointer, length, Allocator.Invalid);
		}

		// Token: 0x17000654 RID: 1620
		// (get) Token: 0x06002280 RID: 8832 RVA: 0x00039D28 File Offset: 0x00037F28
		public int lightIndexCount
		{
			get
			{
				return CullingResults.GetLightIndexCount(this.ptr);
			}
		}

		// Token: 0x17000655 RID: 1621
		// (get) Token: 0x06002281 RID: 8833 RVA: 0x00039D48 File Offset: 0x00037F48
		public int reflectionProbeIndexCount
		{
			get
			{
				return CullingResults.GetReflectionProbeIndexCount(this.ptr);
			}
		}

		// Token: 0x17000656 RID: 1622
		// (get) Token: 0x06002282 RID: 8834 RVA: 0x00039D68 File Offset: 0x00037F68
		public int lightAndReflectionProbeIndexCount
		{
			get
			{
				return CullingResults.GetLightIndexCount(this.ptr) + CullingResults.GetReflectionProbeIndexCount(this.ptr);
			}
		}

		// Token: 0x06002283 RID: 8835 RVA: 0x00039D91 File Offset: 0x00037F91
		public void FillLightAndReflectionProbeIndices(ComputeBuffer computeBuffer)
		{
			CullingResults.FillLightAndReflectionProbeIndices(this.ptr, computeBuffer);
		}

		// Token: 0x06002284 RID: 8836 RVA: 0x00039DA1 File Offset: 0x00037FA1
		public void FillLightAndReflectionProbeIndices(GraphicsBuffer buffer)
		{
			CullingResults.FillLightAndReflectionProbeIndicesGraphicsBuffer(this.ptr, buffer);
		}

		// Token: 0x06002285 RID: 8837 RVA: 0x00039DB4 File Offset: 0x00037FB4
		public NativeArray<int> GetLightIndexMap(Allocator allocator)
		{
			int lightIndexMapSize = CullingResults.GetLightIndexMapSize(this.ptr);
			NativeArray<int> nativeArray = new NativeArray<int>(lightIndexMapSize, allocator, NativeArrayOptions.UninitializedMemory);
			CullingResults.FillLightIndexMap(this.ptr, (IntPtr)nativeArray.GetUnsafePtr<int>(), lightIndexMapSize);
			return nativeArray;
		}

		// Token: 0x06002286 RID: 8838 RVA: 0x00039DF5 File Offset: 0x00037FF5
		public void SetLightIndexMap(NativeArray<int> lightIndexMap)
		{
			CullingResults.SetLightIndexMap(this.ptr, (IntPtr)lightIndexMap.GetUnsafeReadOnlyPtr<int>(), lightIndexMap.Length);
		}

		// Token: 0x06002287 RID: 8839 RVA: 0x00039E18 File Offset: 0x00038018
		public NativeArray<int> GetReflectionProbeIndexMap(Allocator allocator)
		{
			int reflectionProbeIndexMapSize = CullingResults.GetReflectionProbeIndexMapSize(this.ptr);
			NativeArray<int> nativeArray = new NativeArray<int>(reflectionProbeIndexMapSize, allocator, NativeArrayOptions.UninitializedMemory);
			CullingResults.FillReflectionProbeIndexMap(this.ptr, (IntPtr)nativeArray.GetUnsafePtr<int>(), reflectionProbeIndexMapSize);
			return nativeArray;
		}

		// Token: 0x06002288 RID: 8840 RVA: 0x00039E59 File Offset: 0x00038059
		public void SetReflectionProbeIndexMap(NativeArray<int> lightIndexMap)
		{
			CullingResults.SetReflectionProbeIndexMap(this.ptr, (IntPtr)lightIndexMap.GetUnsafeReadOnlyPtr<int>(), lightIndexMap.Length);
		}

		// Token: 0x06002289 RID: 8841 RVA: 0x00039E7C File Offset: 0x0003807C
		public bool GetShadowCasterBounds(int lightIndex, out Bounds outBounds)
		{
			return CullingResults.GetShadowCasterBounds(this.ptr, lightIndex, out outBounds);
		}

		// Token: 0x0600228A RID: 8842 RVA: 0x00039E9C File Offset: 0x0003809C
		public bool ComputeSpotShadowMatricesAndCullingPrimitives(int activeLightIndex, out Matrix4x4 viewMatrix, out Matrix4x4 projMatrix, out ShadowSplitData shadowSplitData)
		{
			return CullingResults.ComputeSpotShadowMatricesAndCullingPrimitives(this.ptr, activeLightIndex, out viewMatrix, out projMatrix, out shadowSplitData);
		}

		// Token: 0x0600228B RID: 8843 RVA: 0x00039EC0 File Offset: 0x000380C0
		public bool ComputePointShadowMatricesAndCullingPrimitives(int activeLightIndex, CubemapFace cubemapFace, float fovBias, out Matrix4x4 viewMatrix, out Matrix4x4 projMatrix, out ShadowSplitData shadowSplitData)
		{
			return CullingResults.ComputePointShadowMatricesAndCullingPrimitives(this.ptr, activeLightIndex, cubemapFace, fovBias, out viewMatrix, out projMatrix, out shadowSplitData);
		}

		// Token: 0x0600228C RID: 8844 RVA: 0x00039EE8 File Offset: 0x000380E8
		public bool ComputeDirectionalShadowMatricesAndCullingPrimitives(int activeLightIndex, int splitIndex, int splitCount, Vector3 splitRatio, int shadowResolution, float shadowNearPlaneOffset, out Matrix4x4 viewMatrix, out Matrix4x4 projMatrix, out ShadowSplitData shadowSplitData)
		{
			return CullingResults.ComputeDirectionalShadowMatricesAndCullingPrimitives(this.ptr, activeLightIndex, splitIndex, splitCount, splitRatio, shadowResolution, shadowNearPlaneOffset, out viewMatrix, out projMatrix, out shadowSplitData);
		}

		// Token: 0x0600228D RID: 8845 RVA: 0x00004557 File Offset: 0x00002757
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		internal void Validate()
		{
		}

		// Token: 0x0600228E RID: 8846 RVA: 0x00039F14 File Offset: 0x00038114
		public bool Equals(CullingResults other)
		{
			return this.ptr.Equals(other.ptr) && this.m_AllocationInfo == other.m_AllocationInfo;
		}

		// Token: 0x0600228F RID: 8847 RVA: 0x00039F50 File Offset: 0x00038150
		public override bool Equals(object obj)
		{
			bool flag = obj == null;
			return !flag && obj is CullingResults && this.Equals((CullingResults)obj);
		}

		// Token: 0x06002290 RID: 8848 RVA: 0x00039F88 File Offset: 0x00038188
		public override int GetHashCode()
		{
			int hashCode = this.ptr.GetHashCode();
			return (hashCode * 397) ^ this.m_AllocationInfo;
		}

		// Token: 0x06002291 RID: 8849 RVA: 0x00039FBC File Offset: 0x000381BC
		public static bool operator ==(CullingResults left, CullingResults right)
		{
			return left.Equals(right);
		}

		// Token: 0x06002292 RID: 8850 RVA: 0x00039FD8 File Offset: 0x000381D8
		public static bool operator !=(CullingResults left, CullingResults right)
		{
			return !left.Equals(right);
		}

		// Token: 0x06002293 RID: 8851
		[MethodImpl(4096)]
		private static extern bool ComputeDirectionalShadowMatricesAndCullingPrimitives_Injected(IntPtr cullingResultsPtr, int activeLightIndex, int splitIndex, int splitCount, ref Vector3 splitRatio, int shadowResolution, float shadowNearPlaneOffset, out Matrix4x4 viewMatrix, out Matrix4x4 projMatrix, out ShadowSplitData shadowSplitData);

		// Token: 0x04000CC6 RID: 3270
		internal IntPtr ptr;

		// Token: 0x04000CC7 RID: 3271
		private unsafe CullingAllocationInfo* m_AllocationInfo;
	}
}
