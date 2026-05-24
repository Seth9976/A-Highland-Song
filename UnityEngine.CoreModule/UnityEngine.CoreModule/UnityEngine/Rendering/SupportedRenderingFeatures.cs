using System;
using UnityEngine.Scripting;

namespace UnityEngine.Rendering
{
	// Token: 0x02000417 RID: 1047
	public class SupportedRenderingFeatures
	{
		// Token: 0x170006BB RID: 1723
		// (get) Token: 0x0600242D RID: 9261 RVA: 0x0003D40C File Offset: 0x0003B60C
		// (set) Token: 0x0600242E RID: 9262 RVA: 0x0003D439 File Offset: 0x0003B639
		public static SupportedRenderingFeatures active
		{
			get
			{
				bool flag = SupportedRenderingFeatures.s_Active == null;
				if (flag)
				{
					SupportedRenderingFeatures.s_Active = new SupportedRenderingFeatures();
				}
				return SupportedRenderingFeatures.s_Active;
			}
			set
			{
				SupportedRenderingFeatures.s_Active = value;
			}
		}

		// Token: 0x170006BC RID: 1724
		// (get) Token: 0x0600242F RID: 9263 RVA: 0x0003D442 File Offset: 0x0003B642
		// (set) Token: 0x06002430 RID: 9264 RVA: 0x0003D44A File Offset: 0x0003B64A
		public SupportedRenderingFeatures.ReflectionProbeModes reflectionProbeModes { get; set; } = SupportedRenderingFeatures.ReflectionProbeModes.None;

		// Token: 0x170006BD RID: 1725
		// (get) Token: 0x06002431 RID: 9265 RVA: 0x0003D453 File Offset: 0x0003B653
		// (set) Token: 0x06002432 RID: 9266 RVA: 0x0003D45B File Offset: 0x0003B65B
		public SupportedRenderingFeatures.LightmapMixedBakeModes defaultMixedLightingModes { get; set; } = SupportedRenderingFeatures.LightmapMixedBakeModes.None;

		// Token: 0x170006BE RID: 1726
		// (get) Token: 0x06002433 RID: 9267 RVA: 0x0003D464 File Offset: 0x0003B664
		// (set) Token: 0x06002434 RID: 9268 RVA: 0x0003D46C File Offset: 0x0003B66C
		public SupportedRenderingFeatures.LightmapMixedBakeModes mixedLightingModes { get; set; } = SupportedRenderingFeatures.LightmapMixedBakeModes.IndirectOnly | SupportedRenderingFeatures.LightmapMixedBakeModes.Subtractive | SupportedRenderingFeatures.LightmapMixedBakeModes.Shadowmask;

		// Token: 0x170006BF RID: 1727
		// (get) Token: 0x06002435 RID: 9269 RVA: 0x0003D475 File Offset: 0x0003B675
		// (set) Token: 0x06002436 RID: 9270 RVA: 0x0003D47D File Offset: 0x0003B67D
		public LightmapBakeType lightmapBakeTypes { get; set; } = LightmapBakeType.Realtime | LightmapBakeType.Baked | LightmapBakeType.Mixed;

		// Token: 0x170006C0 RID: 1728
		// (get) Token: 0x06002437 RID: 9271 RVA: 0x0003D486 File Offset: 0x0003B686
		// (set) Token: 0x06002438 RID: 9272 RVA: 0x0003D48E File Offset: 0x0003B68E
		public LightmapsMode lightmapsModes { get; set; } = LightmapsMode.CombinedDirectional;

		// Token: 0x170006C1 RID: 1729
		// (get) Token: 0x06002439 RID: 9273 RVA: 0x0003D497 File Offset: 0x0003B697
		// (set) Token: 0x0600243A RID: 9274 RVA: 0x0003D49F File Offset: 0x0003B69F
		public bool enlightenLightmapper { get; set; } = true;

		// Token: 0x170006C2 RID: 1730
		// (get) Token: 0x0600243B RID: 9275 RVA: 0x0003D4A8 File Offset: 0x0003B6A8
		// (set) Token: 0x0600243C RID: 9276 RVA: 0x0003D4B0 File Offset: 0x0003B6B0
		public bool enlighten { get; set; } = true;

		// Token: 0x170006C3 RID: 1731
		// (get) Token: 0x0600243D RID: 9277 RVA: 0x0003D4B9 File Offset: 0x0003B6B9
		// (set) Token: 0x0600243E RID: 9278 RVA: 0x0003D4C1 File Offset: 0x0003B6C1
		public bool lightProbeProxyVolumes { get; set; } = true;

		// Token: 0x170006C4 RID: 1732
		// (get) Token: 0x0600243F RID: 9279 RVA: 0x0003D4CA File Offset: 0x0003B6CA
		// (set) Token: 0x06002440 RID: 9280 RVA: 0x0003D4D2 File Offset: 0x0003B6D2
		public bool motionVectors { get; set; } = true;

		// Token: 0x170006C5 RID: 1733
		// (get) Token: 0x06002441 RID: 9281 RVA: 0x0003D4DB File Offset: 0x0003B6DB
		// (set) Token: 0x06002442 RID: 9282 RVA: 0x0003D4E3 File Offset: 0x0003B6E3
		public bool receiveShadows { get; set; } = true;

		// Token: 0x170006C6 RID: 1734
		// (get) Token: 0x06002443 RID: 9283 RVA: 0x0003D4EC File Offset: 0x0003B6EC
		// (set) Token: 0x06002444 RID: 9284 RVA: 0x0003D4F4 File Offset: 0x0003B6F4
		public bool reflectionProbes { get; set; } = true;

		// Token: 0x170006C7 RID: 1735
		// (get) Token: 0x06002445 RID: 9285 RVA: 0x0003D4FD File Offset: 0x0003B6FD
		// (set) Token: 0x06002446 RID: 9286 RVA: 0x0003D505 File Offset: 0x0003B705
		public bool reflectionProbesBlendDistance { get; set; } = true;

		// Token: 0x170006C8 RID: 1736
		// (get) Token: 0x06002447 RID: 9287 RVA: 0x0003D50E File Offset: 0x0003B70E
		// (set) Token: 0x06002448 RID: 9288 RVA: 0x0003D516 File Offset: 0x0003B716
		public bool rendererPriority { get; set; } = false;

		// Token: 0x170006C9 RID: 1737
		// (get) Token: 0x06002449 RID: 9289 RVA: 0x0003D51F File Offset: 0x0003B71F
		// (set) Token: 0x0600244A RID: 9290 RVA: 0x0003D527 File Offset: 0x0003B727
		public bool rendersUIOverlay { get; set; }

		// Token: 0x170006CA RID: 1738
		// (get) Token: 0x0600244B RID: 9291 RVA: 0x0003D530 File Offset: 0x0003B730
		// (set) Token: 0x0600244C RID: 9292 RVA: 0x0003D538 File Offset: 0x0003B738
		public bool overridesEnvironmentLighting { get; set; } = false;

		// Token: 0x170006CB RID: 1739
		// (get) Token: 0x0600244D RID: 9293 RVA: 0x0003D541 File Offset: 0x0003B741
		// (set) Token: 0x0600244E RID: 9294 RVA: 0x0003D549 File Offset: 0x0003B749
		public bool overridesFog { get; set; } = false;

		// Token: 0x170006CC RID: 1740
		// (get) Token: 0x0600244F RID: 9295 RVA: 0x0003D552 File Offset: 0x0003B752
		// (set) Token: 0x06002450 RID: 9296 RVA: 0x0003D55A File Offset: 0x0003B75A
		public bool overridesRealtimeReflectionProbes { get; set; } = false;

		// Token: 0x170006CD RID: 1741
		// (get) Token: 0x06002451 RID: 9297 RVA: 0x0003D563 File Offset: 0x0003B763
		// (set) Token: 0x06002452 RID: 9298 RVA: 0x0003D56B File Offset: 0x0003B76B
		public bool overridesOtherLightingSettings { get; set; } = false;

		// Token: 0x170006CE RID: 1742
		// (get) Token: 0x06002453 RID: 9299 RVA: 0x0003D574 File Offset: 0x0003B774
		// (set) Token: 0x06002454 RID: 9300 RVA: 0x0003D57C File Offset: 0x0003B77C
		public bool editableMaterialRenderQueue { get; set; } = true;

		// Token: 0x170006CF RID: 1743
		// (get) Token: 0x06002455 RID: 9301 RVA: 0x0003D585 File Offset: 0x0003B785
		// (set) Token: 0x06002456 RID: 9302 RVA: 0x0003D58D File Offset: 0x0003B78D
		public bool overridesLODBias { get; set; } = false;

		// Token: 0x170006D0 RID: 1744
		// (get) Token: 0x06002457 RID: 9303 RVA: 0x0003D596 File Offset: 0x0003B796
		// (set) Token: 0x06002458 RID: 9304 RVA: 0x0003D59E File Offset: 0x0003B79E
		public bool overridesMaximumLODLevel { get; set; } = false;

		// Token: 0x170006D1 RID: 1745
		// (get) Token: 0x06002459 RID: 9305 RVA: 0x0003D5A7 File Offset: 0x0003B7A7
		// (set) Token: 0x0600245A RID: 9306 RVA: 0x0003D5AF File Offset: 0x0003B7AF
		public bool rendererProbes { get; set; } = true;

		// Token: 0x170006D2 RID: 1746
		// (get) Token: 0x0600245B RID: 9307 RVA: 0x0003D5B8 File Offset: 0x0003B7B8
		// (set) Token: 0x0600245C RID: 9308 RVA: 0x0003D5C0 File Offset: 0x0003B7C0
		public bool particleSystemInstancing { get; set; } = true;

		// Token: 0x170006D3 RID: 1747
		// (get) Token: 0x0600245D RID: 9309 RVA: 0x0003D5C9 File Offset: 0x0003B7C9
		// (set) Token: 0x0600245E RID: 9310 RVA: 0x0003D5D1 File Offset: 0x0003B7D1
		public bool autoAmbientProbeBaking { get; set; } = true;

		// Token: 0x170006D4 RID: 1748
		// (get) Token: 0x0600245F RID: 9311 RVA: 0x0003D5DA File Offset: 0x0003B7DA
		// (set) Token: 0x06002460 RID: 9312 RVA: 0x0003D5E2 File Offset: 0x0003B7E2
		public bool autoDefaultReflectionProbeBaking { get; set; } = true;

		// Token: 0x170006D5 RID: 1749
		// (get) Token: 0x06002461 RID: 9313 RVA: 0x0003D5EB File Offset: 0x0003B7EB
		// (set) Token: 0x06002462 RID: 9314 RVA: 0x0003D5F3 File Offset: 0x0003B7F3
		public bool overridesShadowmask { get; set; } = false;

		// Token: 0x170006D6 RID: 1750
		// (get) Token: 0x06002463 RID: 9315 RVA: 0x0003D5FC File Offset: 0x0003B7FC
		// (set) Token: 0x06002464 RID: 9316 RVA: 0x0003D604 File Offset: 0x0003B804
		public string overrideShadowmaskMessage { get; set; } = "";

		// Token: 0x170006D7 RID: 1751
		// (get) Token: 0x06002465 RID: 9317 RVA: 0x0003D610 File Offset: 0x0003B810
		public string shadowmaskMessage
		{
			get
			{
				bool flag = !this.overridesShadowmask;
				string text;
				if (flag)
				{
					text = "The Shadowmask Mode used at run time can be set in the Quality Settings panel.";
				}
				else
				{
					text = this.overrideShadowmaskMessage;
				}
				return text;
			}
		}

		// Token: 0x06002466 RID: 9318 RVA: 0x0003D640 File Offset: 0x0003B840
		internal unsafe static MixedLightingMode FallbackMixedLightingMode()
		{
			MixedLightingMode mixedLightingMode;
			SupportedRenderingFeatures.FallbackMixedLightingModeByRef(new IntPtr((void*)(&mixedLightingMode)));
			return mixedLightingMode;
		}

		// Token: 0x06002467 RID: 9319 RVA: 0x0003D664 File Offset: 0x0003B864
		[RequiredByNativeCode]
		internal unsafe static void FallbackMixedLightingModeByRef(IntPtr fallbackModePtr)
		{
			MixedLightingMode* ptr = (MixedLightingMode*)(void*)fallbackModePtr;
			bool flag = SupportedRenderingFeatures.active.defaultMixedLightingModes != SupportedRenderingFeatures.LightmapMixedBakeModes.None && (SupportedRenderingFeatures.active.mixedLightingModes & SupportedRenderingFeatures.active.defaultMixedLightingModes) == SupportedRenderingFeatures.active.defaultMixedLightingModes;
			if (flag)
			{
				SupportedRenderingFeatures.LightmapMixedBakeModes defaultMixedLightingModes = SupportedRenderingFeatures.active.defaultMixedLightingModes;
				SupportedRenderingFeatures.LightmapMixedBakeModes lightmapMixedBakeModes = defaultMixedLightingModes;
				if (lightmapMixedBakeModes != SupportedRenderingFeatures.LightmapMixedBakeModes.Subtractive)
				{
					if (lightmapMixedBakeModes != SupportedRenderingFeatures.LightmapMixedBakeModes.Shadowmask)
					{
						*ptr = MixedLightingMode.IndirectOnly;
					}
					else
					{
						*ptr = MixedLightingMode.Shadowmask;
					}
				}
				else
				{
					*ptr = MixedLightingMode.Subtractive;
				}
			}
			else
			{
				bool flag2 = SupportedRenderingFeatures.IsMixedLightingModeSupported(MixedLightingMode.Shadowmask);
				if (flag2)
				{
					*ptr = MixedLightingMode.Shadowmask;
				}
				else
				{
					bool flag3 = SupportedRenderingFeatures.IsMixedLightingModeSupported(MixedLightingMode.Subtractive);
					if (flag3)
					{
						*ptr = MixedLightingMode.Subtractive;
					}
					else
					{
						*ptr = MixedLightingMode.IndirectOnly;
					}
				}
			}
		}

		// Token: 0x06002468 RID: 9320 RVA: 0x0003D700 File Offset: 0x0003B900
		internal unsafe static bool IsMixedLightingModeSupported(MixedLightingMode mixedMode)
		{
			bool flag;
			SupportedRenderingFeatures.IsMixedLightingModeSupportedByRef(mixedMode, new IntPtr((void*)(&flag)));
			return flag;
		}

		// Token: 0x06002469 RID: 9321 RVA: 0x0003D724 File Offset: 0x0003B924
		[RequiredByNativeCode]
		internal unsafe static void IsMixedLightingModeSupportedByRef(MixedLightingMode mixedMode, IntPtr isSupportedPtr)
		{
			bool* ptr = (bool*)(void*)isSupportedPtr;
			bool flag = !SupportedRenderingFeatures.IsLightmapBakeTypeSupported(LightmapBakeType.Mixed);
			if (flag)
			{
				*ptr = false;
			}
			else
			{
				*ptr = (mixedMode == MixedLightingMode.IndirectOnly && (SupportedRenderingFeatures.active.mixedLightingModes & SupportedRenderingFeatures.LightmapMixedBakeModes.IndirectOnly) == SupportedRenderingFeatures.LightmapMixedBakeModes.IndirectOnly) || (mixedMode == MixedLightingMode.Subtractive && (SupportedRenderingFeatures.active.mixedLightingModes & SupportedRenderingFeatures.LightmapMixedBakeModes.Subtractive) == SupportedRenderingFeatures.LightmapMixedBakeModes.Subtractive) || (mixedMode == MixedLightingMode.Shadowmask && (SupportedRenderingFeatures.active.mixedLightingModes & SupportedRenderingFeatures.LightmapMixedBakeModes.Shadowmask) == SupportedRenderingFeatures.LightmapMixedBakeModes.Shadowmask);
			}
		}

		// Token: 0x0600246A RID: 9322 RVA: 0x0003D78C File Offset: 0x0003B98C
		internal unsafe static bool IsLightmapBakeTypeSupported(LightmapBakeType bakeType)
		{
			bool flag;
			SupportedRenderingFeatures.IsLightmapBakeTypeSupportedByRef(bakeType, new IntPtr((void*)(&flag)));
			return flag;
		}

		// Token: 0x0600246B RID: 9323 RVA: 0x0003D7B0 File Offset: 0x0003B9B0
		[RequiredByNativeCode]
		internal unsafe static void IsLightmapBakeTypeSupportedByRef(LightmapBakeType bakeType, IntPtr isSupportedPtr)
		{
			bool* ptr = (bool*)(void*)isSupportedPtr;
			bool flag = bakeType == LightmapBakeType.Mixed;
			if (flag)
			{
				bool flag2 = SupportedRenderingFeatures.IsLightmapBakeTypeSupported(LightmapBakeType.Baked);
				bool flag3 = !flag2 || SupportedRenderingFeatures.active.mixedLightingModes == SupportedRenderingFeatures.LightmapMixedBakeModes.None;
				if (flag3)
				{
					*ptr = false;
					return;
				}
			}
			*ptr = (SupportedRenderingFeatures.active.lightmapBakeTypes & bakeType) == bakeType;
			bool flag4 = bakeType == LightmapBakeType.Realtime && !SupportedRenderingFeatures.active.enlighten;
			if (flag4)
			{
				*ptr = false;
			}
		}

		// Token: 0x0600246C RID: 9324 RVA: 0x0003D824 File Offset: 0x0003BA24
		internal unsafe static bool IsLightmapsModeSupported(LightmapsMode mode)
		{
			bool flag;
			SupportedRenderingFeatures.IsLightmapsModeSupportedByRef(mode, new IntPtr((void*)(&flag)));
			return flag;
		}

		// Token: 0x0600246D RID: 9325 RVA: 0x0003D848 File Offset: 0x0003BA48
		[RequiredByNativeCode]
		internal unsafe static void IsLightmapsModeSupportedByRef(LightmapsMode mode, IntPtr isSupportedPtr)
		{
			bool* ptr = (bool*)(void*)isSupportedPtr;
			*ptr = (SupportedRenderingFeatures.active.lightmapsModes & mode) == mode;
		}

		// Token: 0x0600246E RID: 9326 RVA: 0x0003D870 File Offset: 0x0003BA70
		internal unsafe static bool IsLightmapperSupported(int lightmapper)
		{
			bool flag;
			SupportedRenderingFeatures.IsLightmapperSupportedByRef(lightmapper, new IntPtr((void*)(&flag)));
			return flag;
		}

		// Token: 0x0600246F RID: 9327 RVA: 0x0003D894 File Offset: 0x0003BA94
		[RequiredByNativeCode]
		internal unsafe static void IsLightmapperSupportedByRef(int lightmapper, IntPtr isSupportedPtr)
		{
			bool* ptr = (bool*)(void*)isSupportedPtr;
			*ptr = lightmapper != 0 || SupportedRenderingFeatures.active.enlightenLightmapper;
		}

		// Token: 0x06002470 RID: 9328 RVA: 0x0003D8BC File Offset: 0x0003BABC
		[RequiredByNativeCode]
		internal unsafe static void IsUIOverlayRenderedBySRP(IntPtr isSupportedPtr)
		{
			bool* ptr = (bool*)(void*)isSupportedPtr;
			*ptr = SupportedRenderingFeatures.active.rendersUIOverlay;
		}

		// Token: 0x06002471 RID: 9329 RVA: 0x0003D8E0 File Offset: 0x0003BAE0
		[RequiredByNativeCode]
		internal unsafe static void IsAutoAmbientProbeBakingSupported(IntPtr isSupportedPtr)
		{
			bool* ptr = (bool*)(void*)isSupportedPtr;
			*ptr = SupportedRenderingFeatures.active.autoAmbientProbeBaking;
		}

		// Token: 0x06002472 RID: 9330 RVA: 0x0003D904 File Offset: 0x0003BB04
		[RequiredByNativeCode]
		internal unsafe static void IsAutoDefaultReflectionProbeBakingSupported(IntPtr isSupportedPtr)
		{
			bool* ptr = (bool*)(void*)isSupportedPtr;
			*ptr = SupportedRenderingFeatures.active.autoDefaultReflectionProbeBaking;
		}

		// Token: 0x06002473 RID: 9331 RVA: 0x0003D928 File Offset: 0x0003BB28
		internal unsafe static int FallbackLightmapper()
		{
			int num;
			SupportedRenderingFeatures.FallbackLightmapperByRef(new IntPtr((void*)(&num)));
			return num;
		}

		// Token: 0x06002474 RID: 9332 RVA: 0x0003D94C File Offset: 0x0003BB4C
		[RequiredByNativeCode]
		internal unsafe static void FallbackLightmapperByRef(IntPtr lightmapperPtr)
		{
			int* ptr = (int*)(void*)lightmapperPtr;
			*ptr = 1;
		}

		// Token: 0x170006D8 RID: 1752
		// (get) Token: 0x06002475 RID: 9333 RVA: 0x0003D964 File Offset: 0x0003BB64
		// (set) Token: 0x06002476 RID: 9334 RVA: 0x00004557 File Offset: 0x00002757
		[Obsolete("terrainDetailUnsupported is deprecated.")]
		public bool terrainDetailUnsupported
		{
			get
			{
				return true;
			}
			set
			{
			}
		}

		// Token: 0x04000D66 RID: 3430
		private static SupportedRenderingFeatures s_Active = new SupportedRenderingFeatures();

		// Token: 0x02000418 RID: 1048
		[Flags]
		public enum ReflectionProbeModes
		{
			// Token: 0x04000D83 RID: 3459
			None = 0,
			// Token: 0x04000D84 RID: 3460
			Rotation = 1
		}

		// Token: 0x02000419 RID: 1049
		[Flags]
		public enum LightmapMixedBakeModes
		{
			// Token: 0x04000D86 RID: 3462
			None = 0,
			// Token: 0x04000D87 RID: 3463
			IndirectOnly = 1,
			// Token: 0x04000D88 RID: 3464
			Subtractive = 2,
			// Token: 0x04000D89 RID: 3465
			Shadowmask = 4
		}
	}
}
