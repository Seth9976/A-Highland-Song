using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;

namespace UnityEngine
{
	// Token: 0x02000133 RID: 307
	[StaticAccessor("GetLightmapSettings()")]
	[NativeHeader("Runtime/Graphics/LightmapSettings.h")]
	public sealed class LightmapSettings : Object
	{
		// Token: 0x0600099C RID: 2460 RVA: 0x0000E87A File Offset: 0x0000CA7A
		private LightmapSettings()
		{
		}

		// Token: 0x170001F7 RID: 503
		// (get) Token: 0x0600099D RID: 2461
		// (set) Token: 0x0600099E RID: 2462
		public static extern LightmapData[] lightmaps
		{
			[FreeFunction]
			[MethodImpl(4096)]
			get;
			[FreeFunction(ThrowsException = true)]
			[MethodImpl(4096)]
			set;
		}

		// Token: 0x170001F8 RID: 504
		// (get) Token: 0x0600099F RID: 2463
		// (set) Token: 0x060009A0 RID: 2464
		public static extern LightmapsMode lightmapsMode
		{
			[MethodImpl(4096)]
			get;
			[FreeFunction(ThrowsException = true)]
			[MethodImpl(4096)]
			set;
		}

		// Token: 0x170001F9 RID: 505
		// (get) Token: 0x060009A1 RID: 2465
		// (set) Token: 0x060009A2 RID: 2466
		public static extern LightProbes lightProbes
		{
			[MethodImpl(4096)]
			get;
			[FreeFunction]
			[NativeName("SetLightProbes")]
			[MethodImpl(4096)]
			set;
		}

		// Token: 0x060009A3 RID: 2467
		[NativeName("ResetAndAwakeFromLoad")]
		[MethodImpl(4096)]
		internal static extern void Reset();

		// Token: 0x170001FA RID: 506
		// (get) Token: 0x060009A4 RID: 2468 RVA: 0x0000E884 File Offset: 0x0000CA84
		// (set) Token: 0x060009A5 RID: 2469 RVA: 0x00004557 File Offset: 0x00002757
		[Obsolete("Use lightmapsMode instead.", false)]
		public static LightmapsModeLegacy lightmapsModeLegacy
		{
			get
			{
				return LightmapsModeLegacy.Single;
			}
			set
			{
			}
		}

		// Token: 0x170001FB RID: 507
		// (get) Token: 0x060009A6 RID: 2470 RVA: 0x0000E898 File Offset: 0x0000CA98
		// (set) Token: 0x060009A7 RID: 2471 RVA: 0x00004557 File Offset: 0x00002757
		[Obsolete("Use QualitySettings.desiredColorSpace instead.", false)]
		public static ColorSpace bakedColorSpace
		{
			get
			{
				return QualitySettings.desiredColorSpace;
			}
			set
			{
			}
		}
	}
}
