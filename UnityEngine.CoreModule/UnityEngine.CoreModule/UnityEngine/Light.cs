using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;
using UnityEngine.Rendering;

namespace UnityEngine
{
	// Token: 0x02000156 RID: 342
	[RequireComponent(typeof(Transform))]
	[NativeHeader("Runtime/Export/Graphics/Light.bindings.h")]
	[RequireComponent(typeof(Transform))]
	[NativeHeader("Runtime/Camera/Light.h")]
	public sealed class Light : Behaviour
	{
		// Token: 0x170002E4 RID: 740
		// (get) Token: 0x06000E3E RID: 3646
		// (set) Token: 0x06000E3F RID: 3647
		[NativeProperty("LightType")]
		public extern LightType type
		{
			[MethodImpl(4096)]
			get;
			[MethodImpl(4096)]
			set;
		}

		// Token: 0x170002E5 RID: 741
		// (get) Token: 0x06000E40 RID: 3648
		// (set) Token: 0x06000E41 RID: 3649
		[NativeProperty("LightShape")]
		public extern LightShape shape
		{
			[MethodImpl(4096)]
			get;
			[MethodImpl(4096)]
			set;
		}

		// Token: 0x170002E6 RID: 742
		// (get) Token: 0x06000E42 RID: 3650
		// (set) Token: 0x06000E43 RID: 3651
		public extern float spotAngle
		{
			[MethodImpl(4096)]
			get;
			[MethodImpl(4096)]
			set;
		}

		// Token: 0x170002E7 RID: 743
		// (get) Token: 0x06000E44 RID: 3652
		// (set) Token: 0x06000E45 RID: 3653
		public extern float innerSpotAngle
		{
			[MethodImpl(4096)]
			get;
			[MethodImpl(4096)]
			set;
		}

		// Token: 0x170002E8 RID: 744
		// (get) Token: 0x06000E46 RID: 3654 RVA: 0x00012B90 File Offset: 0x00010D90
		// (set) Token: 0x06000E47 RID: 3655 RVA: 0x00012BA6 File Offset: 0x00010DA6
		public Color color
		{
			get
			{
				Color color;
				this.get_color_Injected(out color);
				return color;
			}
			set
			{
				this.set_color_Injected(ref value);
			}
		}

		// Token: 0x170002E9 RID: 745
		// (get) Token: 0x06000E48 RID: 3656
		// (set) Token: 0x06000E49 RID: 3657
		public extern float colorTemperature
		{
			[MethodImpl(4096)]
			get;
			[MethodImpl(4096)]
			set;
		}

		// Token: 0x170002EA RID: 746
		// (get) Token: 0x06000E4A RID: 3658
		// (set) Token: 0x06000E4B RID: 3659
		public extern bool useColorTemperature
		{
			[MethodImpl(4096)]
			get;
			[MethodImpl(4096)]
			set;
		}

		// Token: 0x170002EB RID: 747
		// (get) Token: 0x06000E4C RID: 3660
		// (set) Token: 0x06000E4D RID: 3661
		public extern float intensity
		{
			[MethodImpl(4096)]
			get;
			[MethodImpl(4096)]
			set;
		}

		// Token: 0x170002EC RID: 748
		// (get) Token: 0x06000E4E RID: 3662
		// (set) Token: 0x06000E4F RID: 3663
		public extern float bounceIntensity
		{
			[MethodImpl(4096)]
			get;
			[MethodImpl(4096)]
			set;
		}

		// Token: 0x170002ED RID: 749
		// (get) Token: 0x06000E50 RID: 3664
		// (set) Token: 0x06000E51 RID: 3665
		public extern bool useBoundingSphereOverride
		{
			[MethodImpl(4096)]
			get;
			[MethodImpl(4096)]
			set;
		}

		// Token: 0x170002EE RID: 750
		// (get) Token: 0x06000E52 RID: 3666 RVA: 0x00012BB0 File Offset: 0x00010DB0
		// (set) Token: 0x06000E53 RID: 3667 RVA: 0x00012BC6 File Offset: 0x00010DC6
		public Vector4 boundingSphereOverride
		{
			get
			{
				Vector4 vector;
				this.get_boundingSphereOverride_Injected(out vector);
				return vector;
			}
			set
			{
				this.set_boundingSphereOverride_Injected(ref value);
			}
		}

		// Token: 0x170002EF RID: 751
		// (get) Token: 0x06000E54 RID: 3668
		// (set) Token: 0x06000E55 RID: 3669
		public extern bool useViewFrustumForShadowCasterCull
		{
			[MethodImpl(4096)]
			get;
			[MethodImpl(4096)]
			set;
		}

		// Token: 0x170002F0 RID: 752
		// (get) Token: 0x06000E56 RID: 3670
		// (set) Token: 0x06000E57 RID: 3671
		public extern int shadowCustomResolution
		{
			[MethodImpl(4096)]
			get;
			[MethodImpl(4096)]
			set;
		}

		// Token: 0x170002F1 RID: 753
		// (get) Token: 0x06000E58 RID: 3672
		// (set) Token: 0x06000E59 RID: 3673
		public extern float shadowBias
		{
			[MethodImpl(4096)]
			get;
			[MethodImpl(4096)]
			set;
		}

		// Token: 0x170002F2 RID: 754
		// (get) Token: 0x06000E5A RID: 3674
		// (set) Token: 0x06000E5B RID: 3675
		public extern float shadowNormalBias
		{
			[MethodImpl(4096)]
			get;
			[MethodImpl(4096)]
			set;
		}

		// Token: 0x170002F3 RID: 755
		// (get) Token: 0x06000E5C RID: 3676
		// (set) Token: 0x06000E5D RID: 3677
		public extern float shadowNearPlane
		{
			[MethodImpl(4096)]
			get;
			[MethodImpl(4096)]
			set;
		}

		// Token: 0x170002F4 RID: 756
		// (get) Token: 0x06000E5E RID: 3678
		// (set) Token: 0x06000E5F RID: 3679
		public extern bool useShadowMatrixOverride
		{
			[MethodImpl(4096)]
			get;
			[MethodImpl(4096)]
			set;
		}

		// Token: 0x170002F5 RID: 757
		// (get) Token: 0x06000E60 RID: 3680 RVA: 0x00012BD0 File Offset: 0x00010DD0
		// (set) Token: 0x06000E61 RID: 3681 RVA: 0x00012BE6 File Offset: 0x00010DE6
		public Matrix4x4 shadowMatrixOverride
		{
			get
			{
				Matrix4x4 matrix4x;
				this.get_shadowMatrixOverride_Injected(out matrix4x);
				return matrix4x;
			}
			set
			{
				this.set_shadowMatrixOverride_Injected(ref value);
			}
		}

		// Token: 0x170002F6 RID: 758
		// (get) Token: 0x06000E62 RID: 3682
		// (set) Token: 0x06000E63 RID: 3683
		public extern float range
		{
			[MethodImpl(4096)]
			get;
			[MethodImpl(4096)]
			set;
		}

		// Token: 0x170002F7 RID: 759
		// (get) Token: 0x06000E64 RID: 3684
		// (set) Token: 0x06000E65 RID: 3685
		public extern Flare flare
		{
			[MethodImpl(4096)]
			get;
			[MethodImpl(4096)]
			set;
		}

		// Token: 0x170002F8 RID: 760
		// (get) Token: 0x06000E66 RID: 3686 RVA: 0x00012BF0 File Offset: 0x00010DF0
		// (set) Token: 0x06000E67 RID: 3687 RVA: 0x00012C06 File Offset: 0x00010E06
		public LightBakingOutput bakingOutput
		{
			get
			{
				LightBakingOutput lightBakingOutput;
				this.get_bakingOutput_Injected(out lightBakingOutput);
				return lightBakingOutput;
			}
			set
			{
				this.set_bakingOutput_Injected(ref value);
			}
		}

		// Token: 0x170002F9 RID: 761
		// (get) Token: 0x06000E68 RID: 3688
		// (set) Token: 0x06000E69 RID: 3689
		public extern int cullingMask
		{
			[MethodImpl(4096)]
			get;
			[MethodImpl(4096)]
			set;
		}

		// Token: 0x170002FA RID: 762
		// (get) Token: 0x06000E6A RID: 3690
		// (set) Token: 0x06000E6B RID: 3691
		public extern int renderingLayerMask
		{
			[MethodImpl(4096)]
			get;
			[MethodImpl(4096)]
			set;
		}

		// Token: 0x170002FB RID: 763
		// (get) Token: 0x06000E6C RID: 3692
		// (set) Token: 0x06000E6D RID: 3693
		public extern LightShadowCasterMode lightShadowCasterMode
		{
			[MethodImpl(4096)]
			get;
			[MethodImpl(4096)]
			set;
		}

		// Token: 0x06000E6E RID: 3694
		[MethodImpl(4096)]
		public extern void Reset();

		// Token: 0x170002FC RID: 764
		// (get) Token: 0x06000E6F RID: 3695
		// (set) Token: 0x06000E70 RID: 3696
		public extern LightShadows shadows
		{
			[NativeMethod("GetShadowType")]
			[MethodImpl(4096)]
			get;
			[FreeFunction("Light_Bindings::SetShadowType", HasExplicitThis = true, ThrowsException = true)]
			[MethodImpl(4096)]
			set;
		}

		// Token: 0x170002FD RID: 765
		// (get) Token: 0x06000E71 RID: 3697
		// (set) Token: 0x06000E72 RID: 3698
		public extern float shadowStrength
		{
			[MethodImpl(4096)]
			get;
			[FreeFunction("Light_Bindings::SetShadowStrength", HasExplicitThis = true)]
			[MethodImpl(4096)]
			set;
		}

		// Token: 0x170002FE RID: 766
		// (get) Token: 0x06000E73 RID: 3699
		// (set) Token: 0x06000E74 RID: 3700
		public extern LightShadowResolution shadowResolution
		{
			[MethodImpl(4096)]
			get;
			[FreeFunction("Light_Bindings::SetShadowResolution", HasExplicitThis = true, ThrowsException = true)]
			[MethodImpl(4096)]
			set;
		}

		// Token: 0x170002FF RID: 767
		// (get) Token: 0x06000E75 RID: 3701 RVA: 0x00012C10 File Offset: 0x00010E10
		// (set) Token: 0x06000E76 RID: 3702 RVA: 0x00004557 File Offset: 0x00002757
		[Obsolete("Shadow softness is removed in Unity 5.0+", true)]
		[EditorBrowsable(1)]
		public float shadowSoftness
		{
			get
			{
				return 4f;
			}
			set
			{
			}
		}

		// Token: 0x17000300 RID: 768
		// (get) Token: 0x06000E77 RID: 3703 RVA: 0x00012C28 File Offset: 0x00010E28
		// (set) Token: 0x06000E78 RID: 3704 RVA: 0x00004557 File Offset: 0x00002757
		[Obsolete("Shadow softness is removed in Unity 5.0+", true)]
		[EditorBrowsable(1)]
		public float shadowSoftnessFade
		{
			get
			{
				return 1f;
			}
			set
			{
			}
		}

		// Token: 0x17000301 RID: 769
		// (get) Token: 0x06000E79 RID: 3705
		// (set) Token: 0x06000E7A RID: 3706
		public extern float[] layerShadowCullDistances
		{
			[FreeFunction("Light_Bindings::GetLayerShadowCullDistances", HasExplicitThis = true, ThrowsException = false)]
			[MethodImpl(4096)]
			get;
			[FreeFunction("Light_Bindings::SetLayerShadowCullDistances", HasExplicitThis = true, ThrowsException = true)]
			[MethodImpl(4096)]
			set;
		}

		// Token: 0x17000302 RID: 770
		// (get) Token: 0x06000E7B RID: 3707
		// (set) Token: 0x06000E7C RID: 3708
		public extern float cookieSize
		{
			[MethodImpl(4096)]
			get;
			[MethodImpl(4096)]
			set;
		}

		// Token: 0x17000303 RID: 771
		// (get) Token: 0x06000E7D RID: 3709
		// (set) Token: 0x06000E7E RID: 3710
		public extern Texture cookie
		{
			[MethodImpl(4096)]
			get;
			[MethodImpl(4096)]
			set;
		}

		// Token: 0x17000304 RID: 772
		// (get) Token: 0x06000E7F RID: 3711
		// (set) Token: 0x06000E80 RID: 3712
		public extern LightRenderMode renderMode
		{
			[MethodImpl(4096)]
			get;
			[FreeFunction("Light_Bindings::SetRenderMode", HasExplicitThis = true, ThrowsException = true)]
			[MethodImpl(4096)]
			set;
		}

		// Token: 0x17000305 RID: 773
		// (get) Token: 0x06000E81 RID: 3713 RVA: 0x00012C40 File Offset: 0x00010E40
		// (set) Token: 0x06000E82 RID: 3714 RVA: 0x00012C58 File Offset: 0x00010E58
		[Obsolete("warning bakedIndex has been removed please use bakingOutput.isBaked instead.", true)]
		[EditorBrowsable(1)]
		public int bakedIndex
		{
			get
			{
				return this.m_BakedIndex;
			}
			set
			{
				this.m_BakedIndex = value;
			}
		}

		// Token: 0x06000E83 RID: 3715 RVA: 0x00012C62 File Offset: 0x00010E62
		public void AddCommandBuffer(LightEvent evt, CommandBuffer buffer)
		{
			this.AddCommandBuffer(evt, buffer, ShadowMapPass.All);
		}

		// Token: 0x06000E84 RID: 3716
		[FreeFunction("Light_Bindings::AddCommandBuffer", HasExplicitThis = true)]
		[MethodImpl(4096)]
		public extern void AddCommandBuffer(LightEvent evt, CommandBuffer buffer, ShadowMapPass shadowPassMask);

		// Token: 0x06000E85 RID: 3717 RVA: 0x00012C73 File Offset: 0x00010E73
		public void AddCommandBufferAsync(LightEvent evt, CommandBuffer buffer, ComputeQueueType queueType)
		{
			this.AddCommandBufferAsync(evt, buffer, ShadowMapPass.All, queueType);
		}

		// Token: 0x06000E86 RID: 3718
		[FreeFunction("Light_Bindings::AddCommandBufferAsync", HasExplicitThis = true)]
		[MethodImpl(4096)]
		public extern void AddCommandBufferAsync(LightEvent evt, CommandBuffer buffer, ShadowMapPass shadowPassMask, ComputeQueueType queueType);

		// Token: 0x06000E87 RID: 3719
		[MethodImpl(4096)]
		public extern void RemoveCommandBuffer(LightEvent evt, CommandBuffer buffer);

		// Token: 0x06000E88 RID: 3720
		[MethodImpl(4096)]
		public extern void RemoveCommandBuffers(LightEvent evt);

		// Token: 0x06000E89 RID: 3721
		[MethodImpl(4096)]
		public extern void RemoveAllCommandBuffers();

		// Token: 0x06000E8A RID: 3722
		[FreeFunction("Light_Bindings::GetCommandBuffers", HasExplicitThis = true)]
		[MethodImpl(4096)]
		public extern CommandBuffer[] GetCommandBuffers(LightEvent evt);

		// Token: 0x17000306 RID: 774
		// (get) Token: 0x06000E8B RID: 3723
		public extern int commandBufferCount
		{
			[MethodImpl(4096)]
			get;
		}

		// Token: 0x17000307 RID: 775
		// (get) Token: 0x06000E8C RID: 3724 RVA: 0x00012C88 File Offset: 0x00010E88
		// (set) Token: 0x06000E8D RID: 3725 RVA: 0x00012C9F File Offset: 0x00010E9F
		[Obsolete("Use QualitySettings.pixelLightCount instead.")]
		public static int pixelLightCount
		{
			get
			{
				return QualitySettings.pixelLightCount;
			}
			set
			{
				QualitySettings.pixelLightCount = value;
			}
		}

		// Token: 0x06000E8E RID: 3726
		[FreeFunction("Light_Bindings::GetLights")]
		[MethodImpl(4096)]
		public static extern Light[] GetLights(LightType type, int layer);

		// Token: 0x17000308 RID: 776
		// (get) Token: 0x06000E8F RID: 3727 RVA: 0x00012CAC File Offset: 0x00010EAC
		// (set) Token: 0x06000E90 RID: 3728 RVA: 0x00004557 File Offset: 0x00002757
		[Obsolete("light.shadowConstantBias was removed, use light.shadowBias", true)]
		public float shadowConstantBias
		{
			get
			{
				return 0f;
			}
			set
			{
			}
		}

		// Token: 0x17000309 RID: 777
		// (get) Token: 0x06000E91 RID: 3729 RVA: 0x00012CC4 File Offset: 0x00010EC4
		// (set) Token: 0x06000E92 RID: 3730 RVA: 0x00004557 File Offset: 0x00002757
		[Obsolete("light.shadowObjectSizeBias was removed, use light.shadowBias", true)]
		public float shadowObjectSizeBias
		{
			get
			{
				return 0f;
			}
			set
			{
			}
		}

		// Token: 0x1700030A RID: 778
		// (get) Token: 0x06000E93 RID: 3731 RVA: 0x00012CDC File Offset: 0x00010EDC
		// (set) Token: 0x06000E94 RID: 3732 RVA: 0x00004557 File Offset: 0x00002757
		[Obsolete("light.attenuate was removed; all lights always attenuate now", true)]
		public bool attenuate
		{
			get
			{
				return true;
			}
			set
			{
			}
		}

		// Token: 0x06000E96 RID: 3734
		[MethodImpl(4096)]
		private extern void get_color_Injected(out Color ret);

		// Token: 0x06000E97 RID: 3735
		[MethodImpl(4096)]
		private extern void set_color_Injected(ref Color value);

		// Token: 0x06000E98 RID: 3736
		[MethodImpl(4096)]
		private extern void get_boundingSphereOverride_Injected(out Vector4 ret);

		// Token: 0x06000E99 RID: 3737
		[MethodImpl(4096)]
		private extern void set_boundingSphereOverride_Injected(ref Vector4 value);

		// Token: 0x06000E9A RID: 3738
		[MethodImpl(4096)]
		private extern void get_shadowMatrixOverride_Injected(out Matrix4x4 ret);

		// Token: 0x06000E9B RID: 3739
		[MethodImpl(4096)]
		private extern void set_shadowMatrixOverride_Injected(ref Matrix4x4 value);

		// Token: 0x06000E9C RID: 3740
		[MethodImpl(4096)]
		private extern void get_bakingOutput_Injected(out LightBakingOutput ret);

		// Token: 0x06000E9D RID: 3741
		[MethodImpl(4096)]
		private extern void set_bakingOutput_Injected(ref LightBakingOutput value);

		// Token: 0x04000436 RID: 1078
		private int m_BakedIndex;
	}
}
