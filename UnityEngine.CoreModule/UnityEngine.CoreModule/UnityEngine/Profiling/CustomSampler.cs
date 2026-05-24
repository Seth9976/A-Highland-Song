using System;
using System.Diagnostics;
using Unity.Profiling.LowLevel;
using Unity.Profiling.LowLevel.Unsafe;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEngine.Profiling
{
	// Token: 0x0200027B RID: 635
	[UsedByNativeCode]
	[NativeHeader("Runtime/Profiler/Marker.h")]
	[NativeHeader("Runtime/Profiler/ScriptBindings/Sampler.bindings.h")]
	public sealed class CustomSampler : Sampler
	{
		// Token: 0x06001BB3 RID: 7091 RVA: 0x0002C785 File Offset: 0x0002A985
		internal CustomSampler()
		{
		}

		// Token: 0x06001BB4 RID: 7092 RVA: 0x0002C78F File Offset: 0x0002A98F
		internal CustomSampler(IntPtr ptr)
		{
			this.m_Ptr = ptr;
		}

		// Token: 0x06001BB5 RID: 7093 RVA: 0x0002C7A0 File Offset: 0x0002A9A0
		public static CustomSampler Create(string name, bool collectGpuData = false)
		{
			IntPtr intPtr = ProfilerUnsafeUtility.CreateMarker(name, 1, MarkerFlags.AvailabilityNonDevelopment | (collectGpuData ? MarkerFlags.SampleGPU : MarkerFlags.Default), 0);
			bool flag = intPtr == IntPtr.Zero;
			CustomSampler customSampler;
			if (flag)
			{
				customSampler = CustomSampler.s_InvalidCustomSampler;
			}
			else
			{
				customSampler = new CustomSampler(intPtr);
			}
			return customSampler;
		}

		// Token: 0x06001BB6 RID: 7094 RVA: 0x0002C7E5 File Offset: 0x0002A9E5
		[Conditional("ENABLE_PROFILER")]
		public void Begin()
		{
			ProfilerUnsafeUtility.BeginSample(this.m_Ptr);
		}

		// Token: 0x06001BB7 RID: 7095 RVA: 0x0002C7F4 File Offset: 0x0002A9F4
		[Conditional("ENABLE_PROFILER")]
		public void Begin(Object targetObject)
		{
			ProfilerUnsafeUtility.Internal_BeginWithObject(this.m_Ptr, targetObject);
		}

		// Token: 0x06001BB8 RID: 7096 RVA: 0x0002C804 File Offset: 0x0002AA04
		[Conditional("ENABLE_PROFILER")]
		public void End()
		{
			ProfilerUnsafeUtility.EndSample(this.m_Ptr);
		}

		// Token: 0x0400090F RID: 2319
		internal static CustomSampler s_InvalidCustomSampler = new CustomSampler();
	}
}
