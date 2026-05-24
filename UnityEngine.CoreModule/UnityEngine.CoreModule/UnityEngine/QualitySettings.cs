using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;
using UnityEngine.Internal;
using UnityEngine.Rendering;

namespace UnityEngine
{
	// Token: 0x0200013C RID: 316
	[StaticAccessor("GetQualitySettings()", StaticAccessorType.Dot)]
	[NativeHeader("Runtime/Misc/PlayerSettings.h")]
	[NativeHeader("Runtime/Graphics/QualitySettings.h")]
	public sealed class QualitySettings : Object
	{
		// Token: 0x06000A11 RID: 2577 RVA: 0x0000F228 File Offset: 0x0000D428
		public static void IncreaseLevel([DefaultValue("false")] bool applyExpensiveChanges)
		{
			QualitySettings.SetQualityLevel(QualitySettings.GetQualityLevel() + 1, applyExpensiveChanges);
		}

		// Token: 0x06000A12 RID: 2578 RVA: 0x0000F239 File Offset: 0x0000D439
		public static void DecreaseLevel([DefaultValue("false")] bool applyExpensiveChanges)
		{
			QualitySettings.SetQualityLevel(QualitySettings.GetQualityLevel() - 1, applyExpensiveChanges);
		}

		// Token: 0x06000A13 RID: 2579 RVA: 0x0000F24A File Offset: 0x0000D44A
		public static void SetQualityLevel(int index)
		{
			QualitySettings.SetQualityLevel(index, true);
		}

		// Token: 0x06000A14 RID: 2580 RVA: 0x0000F255 File Offset: 0x0000D455
		public static void IncreaseLevel()
		{
			QualitySettings.IncreaseLevel(false);
		}

		// Token: 0x06000A15 RID: 2581 RVA: 0x0000F25F File Offset: 0x0000D45F
		public static void DecreaseLevel()
		{
			QualitySettings.DecreaseLevel(false);
		}

		// Token: 0x17000221 RID: 545
		// (get) Token: 0x06000A16 RID: 2582 RVA: 0x0000F26C File Offset: 0x0000D46C
		// (set) Token: 0x06000A17 RID: 2583 RVA: 0x0000F24A File Offset: 0x0000D44A
		[Obsolete("Use GetQualityLevel and SetQualityLevel", false)]
		public static QualityLevel currentLevel
		{
			get
			{
				return (QualityLevel)QualitySettings.GetQualityLevel();
			}
			set
			{
				QualitySettings.SetQualityLevel((int)value, true);
			}
		}

		// Token: 0x06000A18 RID: 2584 RVA: 0x0000E87A File Offset: 0x0000CA7A
		private QualitySettings()
		{
		}

		// Token: 0x17000222 RID: 546
		// (get) Token: 0x06000A19 RID: 2585
		// (set) Token: 0x06000A1A RID: 2586
		public static extern int pixelLightCount
		{
			[MethodImpl(4096)]
			get;
			[MethodImpl(4096)]
			set;
		}

		// Token: 0x17000223 RID: 547
		// (get) Token: 0x06000A1B RID: 2587
		// (set) Token: 0x06000A1C RID: 2588
		[NativeProperty("ShadowQuality")]
		public static extern ShadowQuality shadows
		{
			[MethodImpl(4096)]
			get;
			[MethodImpl(4096)]
			set;
		}

		// Token: 0x17000224 RID: 548
		// (get) Token: 0x06000A1D RID: 2589
		// (set) Token: 0x06000A1E RID: 2590
		public static extern ShadowProjection shadowProjection
		{
			[MethodImpl(4096)]
			get;
			[MethodImpl(4096)]
			set;
		}

		// Token: 0x17000225 RID: 549
		// (get) Token: 0x06000A1F RID: 2591
		// (set) Token: 0x06000A20 RID: 2592
		public static extern int shadowCascades
		{
			[MethodImpl(4096)]
			get;
			[MethodImpl(4096)]
			set;
		}

		// Token: 0x17000226 RID: 550
		// (get) Token: 0x06000A21 RID: 2593
		// (set) Token: 0x06000A22 RID: 2594
		public static extern float shadowDistance
		{
			[MethodImpl(4096)]
			get;
			[MethodImpl(4096)]
			set;
		}

		// Token: 0x17000227 RID: 551
		// (get) Token: 0x06000A23 RID: 2595
		// (set) Token: 0x06000A24 RID: 2596
		[NativeProperty("ShadowResolution")]
		public static extern ShadowResolution shadowResolution
		{
			[MethodImpl(4096)]
			get;
			[MethodImpl(4096)]
			set;
		}

		// Token: 0x17000228 RID: 552
		// (get) Token: 0x06000A25 RID: 2597
		// (set) Token: 0x06000A26 RID: 2598
		[NativeProperty("ShadowmaskMode")]
		public static extern ShadowmaskMode shadowmaskMode
		{
			[MethodImpl(4096)]
			get;
			[MethodImpl(4096)]
			set;
		}

		// Token: 0x17000229 RID: 553
		// (get) Token: 0x06000A27 RID: 2599
		// (set) Token: 0x06000A28 RID: 2600
		public static extern float shadowNearPlaneOffset
		{
			[MethodImpl(4096)]
			get;
			[MethodImpl(4096)]
			set;
		}

		// Token: 0x1700022A RID: 554
		// (get) Token: 0x06000A29 RID: 2601
		// (set) Token: 0x06000A2A RID: 2602
		public static extern float shadowCascade2Split
		{
			[MethodImpl(4096)]
			get;
			[MethodImpl(4096)]
			set;
		}

		// Token: 0x1700022B RID: 555
		// (get) Token: 0x06000A2B RID: 2603 RVA: 0x0000F284 File Offset: 0x0000D484
		// (set) Token: 0x06000A2C RID: 2604 RVA: 0x0000F299 File Offset: 0x0000D499
		public static Vector3 shadowCascade4Split
		{
			get
			{
				Vector3 vector;
				QualitySettings.get_shadowCascade4Split_Injected(out vector);
				return vector;
			}
			set
			{
				QualitySettings.set_shadowCascade4Split_Injected(ref value);
			}
		}

		// Token: 0x1700022C RID: 556
		// (get) Token: 0x06000A2D RID: 2605
		// (set) Token: 0x06000A2E RID: 2606
		[NativeProperty("LODBias")]
		public static extern float lodBias
		{
			[MethodImpl(4096)]
			get;
			[MethodImpl(4096)]
			set;
		}

		// Token: 0x1700022D RID: 557
		// (get) Token: 0x06000A2F RID: 2607
		// (set) Token: 0x06000A30 RID: 2608
		[NativeProperty("AnisotropicTextures")]
		public static extern AnisotropicFiltering anisotropicFiltering
		{
			[MethodImpl(4096)]
			get;
			[MethodImpl(4096)]
			set;
		}

		// Token: 0x1700022E RID: 558
		// (get) Token: 0x06000A31 RID: 2609
		// (set) Token: 0x06000A32 RID: 2610
		public static extern int masterTextureLimit
		{
			[MethodImpl(4096)]
			get;
			[MethodImpl(4096)]
			set;
		}

		// Token: 0x1700022F RID: 559
		// (get) Token: 0x06000A33 RID: 2611
		// (set) Token: 0x06000A34 RID: 2612
		public static extern int maximumLODLevel
		{
			[MethodImpl(4096)]
			get;
			[MethodImpl(4096)]
			set;
		}

		// Token: 0x17000230 RID: 560
		// (get) Token: 0x06000A35 RID: 2613
		// (set) Token: 0x06000A36 RID: 2614
		public static extern int particleRaycastBudget
		{
			[MethodImpl(4096)]
			get;
			[MethodImpl(4096)]
			set;
		}

		// Token: 0x17000231 RID: 561
		// (get) Token: 0x06000A37 RID: 2615
		// (set) Token: 0x06000A38 RID: 2616
		public static extern bool softParticles
		{
			[MethodImpl(4096)]
			get;
			[MethodImpl(4096)]
			set;
		}

		// Token: 0x17000232 RID: 562
		// (get) Token: 0x06000A39 RID: 2617
		// (set) Token: 0x06000A3A RID: 2618
		public static extern bool softVegetation
		{
			[MethodImpl(4096)]
			get;
			[MethodImpl(4096)]
			set;
		}

		// Token: 0x17000233 RID: 563
		// (get) Token: 0x06000A3B RID: 2619
		// (set) Token: 0x06000A3C RID: 2620
		public static extern int vSyncCount
		{
			[MethodImpl(4096)]
			get;
			[MethodImpl(4096)]
			set;
		}

		// Token: 0x17000234 RID: 564
		// (get) Token: 0x06000A3D RID: 2621
		// (set) Token: 0x06000A3E RID: 2622
		public static extern int antiAliasing
		{
			[MethodImpl(4096)]
			get;
			[MethodImpl(4096)]
			set;
		}

		// Token: 0x17000235 RID: 565
		// (get) Token: 0x06000A3F RID: 2623
		// (set) Token: 0x06000A40 RID: 2624
		public static extern int asyncUploadTimeSlice
		{
			[MethodImpl(4096)]
			get;
			[MethodImpl(4096)]
			set;
		}

		// Token: 0x17000236 RID: 566
		// (get) Token: 0x06000A41 RID: 2625
		// (set) Token: 0x06000A42 RID: 2626
		public static extern int asyncUploadBufferSize
		{
			[MethodImpl(4096)]
			get;
			[MethodImpl(4096)]
			set;
		}

		// Token: 0x17000237 RID: 567
		// (get) Token: 0x06000A43 RID: 2627
		// (set) Token: 0x06000A44 RID: 2628
		public static extern bool asyncUploadPersistentBuffer
		{
			[MethodImpl(4096)]
			get;
			[MethodImpl(4096)]
			set;
		}

		// Token: 0x06000A45 RID: 2629
		[NativeName("SetLODSettings")]
		[MethodImpl(4096)]
		public static extern void SetLODSettings(float lodBias, int maximumLODLevel, bool setDirty = true);

		// Token: 0x17000238 RID: 568
		// (get) Token: 0x06000A46 RID: 2630
		// (set) Token: 0x06000A47 RID: 2631
		public static extern bool realtimeReflectionProbes
		{
			[MethodImpl(4096)]
			get;
			[MethodImpl(4096)]
			set;
		}

		// Token: 0x17000239 RID: 569
		// (get) Token: 0x06000A48 RID: 2632
		// (set) Token: 0x06000A49 RID: 2633
		public static extern bool billboardsFaceCameraPosition
		{
			[MethodImpl(4096)]
			get;
			[MethodImpl(4096)]
			set;
		}

		// Token: 0x1700023A RID: 570
		// (get) Token: 0x06000A4A RID: 2634
		// (set) Token: 0x06000A4B RID: 2635
		public static extern float resolutionScalingFixedDPIFactor
		{
			[MethodImpl(4096)]
			get;
			[MethodImpl(4096)]
			set;
		}

		// Token: 0x1700023B RID: 571
		// (get) Token: 0x06000A4C RID: 2636
		// (set) Token: 0x06000A4D RID: 2637
		[NativeName("RenderPipeline")]
		private static extern ScriptableObject INTERNAL_renderPipeline
		{
			[MethodImpl(4096)]
			get;
			[MethodImpl(4096)]
			set;
		}

		// Token: 0x1700023C RID: 572
		// (get) Token: 0x06000A4E RID: 2638 RVA: 0x0000F2A4 File Offset: 0x0000D4A4
		// (set) Token: 0x06000A4F RID: 2639 RVA: 0x0000F2C0 File Offset: 0x0000D4C0
		public static RenderPipelineAsset renderPipeline
		{
			get
			{
				return QualitySettings.INTERNAL_renderPipeline as RenderPipelineAsset;
			}
			set
			{
				QualitySettings.INTERNAL_renderPipeline = value;
			}
		}

		// Token: 0x06000A50 RID: 2640
		[NativeName("GetRenderPipelineAssetAt")]
		[MethodImpl(4096)]
		internal static extern ScriptableObject InternalGetRenderPipelineAssetAt(int index);

		// Token: 0x06000A51 RID: 2641 RVA: 0x0000F2CC File Offset: 0x0000D4CC
		public static RenderPipelineAsset GetRenderPipelineAssetAt(int index)
		{
			bool flag = index < 0 || index >= QualitySettings.names.Length;
			if (flag)
			{
				throw new IndexOutOfRangeException(string.Format("{0} is out of range [0..{1}[", "index", QualitySettings.names.Length));
			}
			return QualitySettings.InternalGetRenderPipelineAssetAt(index) as RenderPipelineAsset;
		}

		// Token: 0x1700023D RID: 573
		// (get) Token: 0x06000A52 RID: 2642
		// (set) Token: 0x06000A53 RID: 2643
		[Obsolete("blendWeights is obsolete. Use skinWeights instead (UnityUpgradable) -> skinWeights", true)]
		public static extern BlendWeights blendWeights
		{
			[NativeName("GetSkinWeights")]
			[MethodImpl(4096)]
			get;
			[NativeName("SetSkinWeights")]
			[MethodImpl(4096)]
			set;
		}

		// Token: 0x1700023E RID: 574
		// (get) Token: 0x06000A54 RID: 2644
		// (set) Token: 0x06000A55 RID: 2645
		public static extern SkinWeights skinWeights
		{
			[MethodImpl(4096)]
			get;
			[MethodImpl(4096)]
			set;
		}

		// Token: 0x1700023F RID: 575
		// (get) Token: 0x06000A56 RID: 2646
		// (set) Token: 0x06000A57 RID: 2647
		public static extern bool streamingMipmapsActive
		{
			[MethodImpl(4096)]
			get;
			[MethodImpl(4096)]
			set;
		}

		// Token: 0x17000240 RID: 576
		// (get) Token: 0x06000A58 RID: 2648
		// (set) Token: 0x06000A59 RID: 2649
		public static extern float streamingMipmapsMemoryBudget
		{
			[MethodImpl(4096)]
			get;
			[MethodImpl(4096)]
			set;
		}

		// Token: 0x17000241 RID: 577
		// (get) Token: 0x06000A5A RID: 2650
		// (set) Token: 0x06000A5B RID: 2651
		public static extern int streamingMipmapsRenderersPerFrame
		{
			[MethodImpl(4096)]
			get;
			[MethodImpl(4096)]
			set;
		}

		// Token: 0x17000242 RID: 578
		// (get) Token: 0x06000A5C RID: 2652
		// (set) Token: 0x06000A5D RID: 2653
		public static extern int streamingMipmapsMaxLevelReduction
		{
			[MethodImpl(4096)]
			get;
			[MethodImpl(4096)]
			set;
		}

		// Token: 0x17000243 RID: 579
		// (get) Token: 0x06000A5E RID: 2654
		// (set) Token: 0x06000A5F RID: 2655
		public static extern bool streamingMipmapsAddAllCameras
		{
			[MethodImpl(4096)]
			get;
			[MethodImpl(4096)]
			set;
		}

		// Token: 0x17000244 RID: 580
		// (get) Token: 0x06000A60 RID: 2656
		// (set) Token: 0x06000A61 RID: 2657
		public static extern int streamingMipmapsMaxFileIORequests
		{
			[MethodImpl(4096)]
			get;
			[MethodImpl(4096)]
			set;
		}

		// Token: 0x17000245 RID: 581
		// (get) Token: 0x06000A62 RID: 2658
		// (set) Token: 0x06000A63 RID: 2659
		[StaticAccessor("QualitySettingsScripting", StaticAccessorType.DoubleColon)]
		public static extern int maxQueuedFrames
		{
			[MethodImpl(4096)]
			get;
			[MethodImpl(4096)]
			set;
		}

		// Token: 0x06000A64 RID: 2660
		[NativeName("GetCurrentIndex")]
		[MethodImpl(4096)]
		public static extern int GetQualityLevel();

		// Token: 0x06000A65 RID: 2661
		[FreeFunction]
		[MethodImpl(4096)]
		public static extern Object GetQualitySettings();

		// Token: 0x06000A66 RID: 2662
		[NativeName("SetCurrentIndex")]
		[MethodImpl(4096)]
		public static extern void SetQualityLevel(int index, [DefaultValue("true")] bool applyExpensiveChanges);

		// Token: 0x17000246 RID: 582
		// (get) Token: 0x06000A67 RID: 2663
		[NativeProperty("QualitySettingsNames")]
		public static extern string[] names
		{
			[MethodImpl(4096)]
			get;
		}

		// Token: 0x17000247 RID: 583
		// (get) Token: 0x06000A68 RID: 2664
		public static extern ColorSpace desiredColorSpace
		{
			[NativeName("GetColorSpace")]
			[StaticAccessor("GetPlayerSettings()", StaticAccessorType.Dot)]
			[MethodImpl(4096)]
			get;
		}

		// Token: 0x17000248 RID: 584
		// (get) Token: 0x06000A69 RID: 2665
		public static extern ColorSpace activeColorSpace
		{
			[StaticAccessor("GetPlayerSettings()", StaticAccessorType.Dot)]
			[NativeName("GetColorSpace")]
			[MethodImpl(4096)]
			get;
		}

		// Token: 0x06000A6A RID: 2666
		[MethodImpl(4096)]
		private static extern void get_shadowCascade4Split_Injected(out Vector3 ret);

		// Token: 0x06000A6B RID: 2667
		[MethodImpl(4096)]
		private static extern void set_shadowCascade4Split_Injected(ref Vector3 value);
	}
}
