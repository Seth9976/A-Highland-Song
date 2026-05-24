using System;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine.Scripting;

namespace UnityEngine.Experimental.GlobalIllumination
{
	// Token: 0x02000463 RID: 1123
	public static class Lightmapping
	{
		// Token: 0x060027D1 RID: 10193 RVA: 0x00042450 File Offset: 0x00040650
		[RequiredByNativeCode]
		public static void SetDelegate(Lightmapping.RequestLightsDelegate del)
		{
			Lightmapping.s_RequestLightsDelegate = ((del != null) ? del : Lightmapping.s_DefaultDelegate);
		}

		// Token: 0x060027D2 RID: 10194 RVA: 0x00042464 File Offset: 0x00040664
		[RequiredByNativeCode]
		public static Lightmapping.RequestLightsDelegate GetDelegate()
		{
			return Lightmapping.s_RequestLightsDelegate;
		}

		// Token: 0x060027D3 RID: 10195 RVA: 0x0004247B File Offset: 0x0004067B
		[RequiredByNativeCode]
		public static void ResetDelegate()
		{
			Lightmapping.s_RequestLightsDelegate = Lightmapping.s_DefaultDelegate;
		}

		// Token: 0x060027D4 RID: 10196 RVA: 0x00042488 File Offset: 0x00040688
		[RequiredByNativeCode]
		internal unsafe static void RequestLights(Light[] lights, IntPtr outLightsPtr, int outLightsCount)
		{
			NativeArray<LightDataGI> nativeArray = NativeArrayUnsafeUtility.ConvertExistingDataToNativeArray<LightDataGI>((void*)outLightsPtr, outLightsCount, Allocator.None);
			Lightmapping.s_RequestLightsDelegate(lights, nativeArray);
		}

		// Token: 0x04000EBD RID: 3773
		[RequiredByNativeCode]
		private static readonly Lightmapping.RequestLightsDelegate s_DefaultDelegate = delegate(Light[] requests, NativeArray<LightDataGI> lightsOutput)
		{
			DirectionalLight directionalLight = default(DirectionalLight);
			PointLight pointLight = default(PointLight);
			SpotLight spotLight = default(SpotLight);
			RectangleLight rectangleLight = default(RectangleLight);
			DiscLight discLight = default(DiscLight);
			Cookie cookie = default(Cookie);
			LightDataGI lightDataGI = default(LightDataGI);
			for (int i = 0; i < requests.Length; i++)
			{
				Light light = requests[i];
				switch (light.type)
				{
				case LightType.Spot:
					LightmapperUtils.Extract(light, ref spotLight);
					LightmapperUtils.Extract(light, out cookie);
					lightDataGI.Init(ref spotLight, ref cookie);
					break;
				case LightType.Directional:
					LightmapperUtils.Extract(light, ref directionalLight);
					LightmapperUtils.Extract(light, out cookie);
					lightDataGI.Init(ref directionalLight, ref cookie);
					break;
				case LightType.Point:
					LightmapperUtils.Extract(light, ref pointLight);
					LightmapperUtils.Extract(light, out cookie);
					lightDataGI.Init(ref pointLight, ref cookie);
					break;
				case LightType.Area:
					LightmapperUtils.Extract(light, ref rectangleLight);
					LightmapperUtils.Extract(light, out cookie);
					lightDataGI.Init(ref rectangleLight, ref cookie);
					break;
				case LightType.Disc:
					LightmapperUtils.Extract(light, ref discLight);
					LightmapperUtils.Extract(light, out cookie);
					lightDataGI.Init(ref discLight, ref cookie);
					break;
				default:
					lightDataGI.InitNoBake(light.GetInstanceID());
					break;
				}
				lightsOutput[i] = lightDataGI;
			}
		};

		// Token: 0x04000EBE RID: 3774
		[RequiredByNativeCode]
		private static Lightmapping.RequestLightsDelegate s_RequestLightsDelegate = Lightmapping.s_DefaultDelegate;

		// Token: 0x02000464 RID: 1124
		// (Invoke) Token: 0x060027D7 RID: 10199
		public delegate void RequestLightsDelegate(Light[] requests, NativeArray<LightDataGI> lightsOutput);
	}
}
