using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;
using UnityEngine.Rendering;

namespace UnityEngine
{
	// Token: 0x02000148 RID: 328
	[NativeHeader("Runtime/Graphics/QualitySettingsTypes.h")]
	[NativeHeader("Runtime/Camera/RenderSettings.h")]
	[StaticAccessor("GetRenderSettings()", StaticAccessorType.Dot)]
	public sealed class RenderSettings : Object
	{
		// Token: 0x17000294 RID: 660
		// (get) Token: 0x06000C06 RID: 3078 RVA: 0x00010724 File Offset: 0x0000E924
		// (set) Token: 0x06000C07 RID: 3079 RVA: 0x0001073B File Offset: 0x0000E93B
		[Obsolete("Use RenderSettings.ambientIntensity instead (UnityUpgradable) -> ambientIntensity", false)]
		public static float ambientSkyboxAmount
		{
			get
			{
				return RenderSettings.ambientIntensity;
			}
			set
			{
				RenderSettings.ambientIntensity = value;
			}
		}

		// Token: 0x06000C08 RID: 3080 RVA: 0x0000E87A File Offset: 0x0000CA7A
		private RenderSettings()
		{
		}

		// Token: 0x17000295 RID: 661
		// (get) Token: 0x06000C09 RID: 3081
		// (set) Token: 0x06000C0A RID: 3082
		[NativeProperty("UseFog")]
		public static extern bool fog
		{
			[MethodImpl(4096)]
			get;
			[MethodImpl(4096)]
			set;
		}

		// Token: 0x17000296 RID: 662
		// (get) Token: 0x06000C0B RID: 3083
		// (set) Token: 0x06000C0C RID: 3084
		[NativeProperty("LinearFogStart")]
		public static extern float fogStartDistance
		{
			[MethodImpl(4096)]
			get;
			[MethodImpl(4096)]
			set;
		}

		// Token: 0x17000297 RID: 663
		// (get) Token: 0x06000C0D RID: 3085
		// (set) Token: 0x06000C0E RID: 3086
		[NativeProperty("LinearFogEnd")]
		public static extern float fogEndDistance
		{
			[MethodImpl(4096)]
			get;
			[MethodImpl(4096)]
			set;
		}

		// Token: 0x17000298 RID: 664
		// (get) Token: 0x06000C0F RID: 3087
		// (set) Token: 0x06000C10 RID: 3088
		public static extern FogMode fogMode
		{
			[MethodImpl(4096)]
			get;
			[MethodImpl(4096)]
			set;
		}

		// Token: 0x17000299 RID: 665
		// (get) Token: 0x06000C11 RID: 3089 RVA: 0x00010748 File Offset: 0x0000E948
		// (set) Token: 0x06000C12 RID: 3090 RVA: 0x0001075D File Offset: 0x0000E95D
		public static Color fogColor
		{
			get
			{
				Color color;
				RenderSettings.get_fogColor_Injected(out color);
				return color;
			}
			set
			{
				RenderSettings.set_fogColor_Injected(ref value);
			}
		}

		// Token: 0x1700029A RID: 666
		// (get) Token: 0x06000C13 RID: 3091
		// (set) Token: 0x06000C14 RID: 3092
		public static extern float fogDensity
		{
			[MethodImpl(4096)]
			get;
			[MethodImpl(4096)]
			set;
		}

		// Token: 0x1700029B RID: 667
		// (get) Token: 0x06000C15 RID: 3093
		// (set) Token: 0x06000C16 RID: 3094
		public static extern AmbientMode ambientMode
		{
			[MethodImpl(4096)]
			get;
			[MethodImpl(4096)]
			set;
		}

		// Token: 0x1700029C RID: 668
		// (get) Token: 0x06000C17 RID: 3095 RVA: 0x00010768 File Offset: 0x0000E968
		// (set) Token: 0x06000C18 RID: 3096 RVA: 0x0001077D File Offset: 0x0000E97D
		public static Color ambientSkyColor
		{
			get
			{
				Color color;
				RenderSettings.get_ambientSkyColor_Injected(out color);
				return color;
			}
			set
			{
				RenderSettings.set_ambientSkyColor_Injected(ref value);
			}
		}

		// Token: 0x1700029D RID: 669
		// (get) Token: 0x06000C19 RID: 3097 RVA: 0x00010788 File Offset: 0x0000E988
		// (set) Token: 0x06000C1A RID: 3098 RVA: 0x0001079D File Offset: 0x0000E99D
		public static Color ambientEquatorColor
		{
			get
			{
				Color color;
				RenderSettings.get_ambientEquatorColor_Injected(out color);
				return color;
			}
			set
			{
				RenderSettings.set_ambientEquatorColor_Injected(ref value);
			}
		}

		// Token: 0x1700029E RID: 670
		// (get) Token: 0x06000C1B RID: 3099 RVA: 0x000107A8 File Offset: 0x0000E9A8
		// (set) Token: 0x06000C1C RID: 3100 RVA: 0x000107BD File Offset: 0x0000E9BD
		public static Color ambientGroundColor
		{
			get
			{
				Color color;
				RenderSettings.get_ambientGroundColor_Injected(out color);
				return color;
			}
			set
			{
				RenderSettings.set_ambientGroundColor_Injected(ref value);
			}
		}

		// Token: 0x1700029F RID: 671
		// (get) Token: 0x06000C1D RID: 3101
		// (set) Token: 0x06000C1E RID: 3102
		public static extern float ambientIntensity
		{
			[MethodImpl(4096)]
			get;
			[MethodImpl(4096)]
			set;
		}

		// Token: 0x170002A0 RID: 672
		// (get) Token: 0x06000C1F RID: 3103 RVA: 0x000107C8 File Offset: 0x0000E9C8
		// (set) Token: 0x06000C20 RID: 3104 RVA: 0x000107DD File Offset: 0x0000E9DD
		[NativeProperty("AmbientSkyColor")]
		public static Color ambientLight
		{
			get
			{
				Color color;
				RenderSettings.get_ambientLight_Injected(out color);
				return color;
			}
			set
			{
				RenderSettings.set_ambientLight_Injected(ref value);
			}
		}

		// Token: 0x170002A1 RID: 673
		// (get) Token: 0x06000C21 RID: 3105 RVA: 0x000107E8 File Offset: 0x0000E9E8
		// (set) Token: 0x06000C22 RID: 3106 RVA: 0x000107FD File Offset: 0x0000E9FD
		public static Color subtractiveShadowColor
		{
			get
			{
				Color color;
				RenderSettings.get_subtractiveShadowColor_Injected(out color);
				return color;
			}
			set
			{
				RenderSettings.set_subtractiveShadowColor_Injected(ref value);
			}
		}

		// Token: 0x170002A2 RID: 674
		// (get) Token: 0x06000C23 RID: 3107
		// (set) Token: 0x06000C24 RID: 3108
		[NativeProperty("SkyboxMaterial")]
		public static extern Material skybox
		{
			[MethodImpl(4096)]
			get;
			[MethodImpl(4096)]
			set;
		}

		// Token: 0x170002A3 RID: 675
		// (get) Token: 0x06000C25 RID: 3109
		// (set) Token: 0x06000C26 RID: 3110
		public static extern Light sun
		{
			[MethodImpl(4096)]
			get;
			[MethodImpl(4096)]
			set;
		}

		// Token: 0x170002A4 RID: 676
		// (get) Token: 0x06000C27 RID: 3111 RVA: 0x00010808 File Offset: 0x0000EA08
		// (set) Token: 0x06000C28 RID: 3112 RVA: 0x0001081D File Offset: 0x0000EA1D
		public static SphericalHarmonicsL2 ambientProbe
		{
			[NativeMethod("GetFinalAmbientProbe")]
			get
			{
				SphericalHarmonicsL2 sphericalHarmonicsL;
				RenderSettings.get_ambientProbe_Injected(out sphericalHarmonicsL);
				return sphericalHarmonicsL;
			}
			set
			{
				RenderSettings.set_ambientProbe_Injected(ref value);
			}
		}

		// Token: 0x170002A5 RID: 677
		// (get) Token: 0x06000C29 RID: 3113
		// (set) Token: 0x06000C2A RID: 3114
		public static extern Texture customReflection
		{
			[MethodImpl(4096)]
			get;
			[NativeThrows]
			[MethodImpl(4096)]
			set;
		}

		// Token: 0x170002A6 RID: 678
		// (get) Token: 0x06000C2B RID: 3115
		// (set) Token: 0x06000C2C RID: 3116
		public static extern float reflectionIntensity
		{
			[MethodImpl(4096)]
			get;
			[MethodImpl(4096)]
			set;
		}

		// Token: 0x170002A7 RID: 679
		// (get) Token: 0x06000C2D RID: 3117
		// (set) Token: 0x06000C2E RID: 3118
		public static extern int reflectionBounces
		{
			[MethodImpl(4096)]
			get;
			[MethodImpl(4096)]
			set;
		}

		// Token: 0x170002A8 RID: 680
		// (get) Token: 0x06000C2F RID: 3119
		[NativeProperty("GeneratedSkyboxReflection")]
		internal static extern Cubemap defaultReflection
		{
			[MethodImpl(4096)]
			get;
		}

		// Token: 0x170002A9 RID: 681
		// (get) Token: 0x06000C30 RID: 3120
		// (set) Token: 0x06000C31 RID: 3121
		public static extern DefaultReflectionMode defaultReflectionMode
		{
			[MethodImpl(4096)]
			get;
			[MethodImpl(4096)]
			set;
		}

		// Token: 0x170002AA RID: 682
		// (get) Token: 0x06000C32 RID: 3122
		// (set) Token: 0x06000C33 RID: 3123
		public static extern int defaultReflectionResolution
		{
			[MethodImpl(4096)]
			get;
			[MethodImpl(4096)]
			set;
		}

		// Token: 0x170002AB RID: 683
		// (get) Token: 0x06000C34 RID: 3124
		// (set) Token: 0x06000C35 RID: 3125
		public static extern float haloStrength
		{
			[MethodImpl(4096)]
			get;
			[MethodImpl(4096)]
			set;
		}

		// Token: 0x170002AC RID: 684
		// (get) Token: 0x06000C36 RID: 3126
		// (set) Token: 0x06000C37 RID: 3127
		public static extern float flareStrength
		{
			[MethodImpl(4096)]
			get;
			[MethodImpl(4096)]
			set;
		}

		// Token: 0x170002AD RID: 685
		// (get) Token: 0x06000C38 RID: 3128
		// (set) Token: 0x06000C39 RID: 3129
		public static extern float flareFadeSpeed
		{
			[MethodImpl(4096)]
			get;
			[MethodImpl(4096)]
			set;
		}

		// Token: 0x06000C3A RID: 3130
		[FreeFunction("GetRenderSettings")]
		[MethodImpl(4096)]
		internal static extern Object GetRenderSettings();

		// Token: 0x06000C3B RID: 3131
		[StaticAccessor("RenderSettingsScripting", StaticAccessorType.DoubleColon)]
		[MethodImpl(4096)]
		internal static extern void Reset();

		// Token: 0x06000C3C RID: 3132
		[MethodImpl(4096)]
		private static extern void get_fogColor_Injected(out Color ret);

		// Token: 0x06000C3D RID: 3133
		[MethodImpl(4096)]
		private static extern void set_fogColor_Injected(ref Color value);

		// Token: 0x06000C3E RID: 3134
		[MethodImpl(4096)]
		private static extern void get_ambientSkyColor_Injected(out Color ret);

		// Token: 0x06000C3F RID: 3135
		[MethodImpl(4096)]
		private static extern void set_ambientSkyColor_Injected(ref Color value);

		// Token: 0x06000C40 RID: 3136
		[MethodImpl(4096)]
		private static extern void get_ambientEquatorColor_Injected(out Color ret);

		// Token: 0x06000C41 RID: 3137
		[MethodImpl(4096)]
		private static extern void set_ambientEquatorColor_Injected(ref Color value);

		// Token: 0x06000C42 RID: 3138
		[MethodImpl(4096)]
		private static extern void get_ambientGroundColor_Injected(out Color ret);

		// Token: 0x06000C43 RID: 3139
		[MethodImpl(4096)]
		private static extern void set_ambientGroundColor_Injected(ref Color value);

		// Token: 0x06000C44 RID: 3140
		[MethodImpl(4096)]
		private static extern void get_ambientLight_Injected(out Color ret);

		// Token: 0x06000C45 RID: 3141
		[MethodImpl(4096)]
		private static extern void set_ambientLight_Injected(ref Color value);

		// Token: 0x06000C46 RID: 3142
		[MethodImpl(4096)]
		private static extern void get_subtractiveShadowColor_Injected(out Color ret);

		// Token: 0x06000C47 RID: 3143
		[MethodImpl(4096)]
		private static extern void set_subtractiveShadowColor_Injected(ref Color value);

		// Token: 0x06000C48 RID: 3144
		[MethodImpl(4096)]
		private static extern void get_ambientProbe_Injected(out SphericalHarmonicsL2 ret);

		// Token: 0x06000C49 RID: 3145
		[MethodImpl(4096)]
		private static extern void set_ambientProbe_Injected(ref SphericalHarmonicsL2 value);
	}
}
