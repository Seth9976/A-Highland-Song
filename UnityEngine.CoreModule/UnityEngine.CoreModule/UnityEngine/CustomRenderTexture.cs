using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Internal;
using UnityEngine.Scripting;

namespace UnityEngine
{
	// Token: 0x020001AB RID: 427
	[NativeHeader("Runtime/Graphics/CustomRenderTexture.h")]
	[UsedByNativeCode]
	public sealed class CustomRenderTexture : RenderTexture
	{
		// Token: 0x06001285 RID: 4741
		[FreeFunction(Name = "CustomRenderTextureScripting::Create")]
		[MethodImpl(4096)]
		private static extern void Internal_CreateCustomRenderTexture([Writable] CustomRenderTexture rt);

		// Token: 0x06001286 RID: 4742
		[NativeName("TriggerUpdate")]
		[MethodImpl(4096)]
		private extern void TriggerUpdate(int count);

		// Token: 0x06001287 RID: 4743 RVA: 0x00018FD7 File Offset: 0x000171D7
		public void Update(int count)
		{
			CustomRenderTextureManager.InvokeTriggerUpdate(this, count);
			this.TriggerUpdate(count);
		}

		// Token: 0x06001288 RID: 4744 RVA: 0x00018FEA File Offset: 0x000171EA
		public void Update()
		{
			this.Update(1);
		}

		// Token: 0x06001289 RID: 4745
		[NativeName("TriggerInitialization")]
		[MethodImpl(4096)]
		private extern void TriggerInitialization();

		// Token: 0x0600128A RID: 4746 RVA: 0x00018FF5 File Offset: 0x000171F5
		public void Initialize()
		{
			this.TriggerInitialization();
			CustomRenderTextureManager.InvokeTriggerInitialize(this);
		}

		// Token: 0x0600128B RID: 4747
		[MethodImpl(4096)]
		public extern void ClearUpdateZones();

		// Token: 0x170003C5 RID: 965
		// (get) Token: 0x0600128C RID: 4748
		// (set) Token: 0x0600128D RID: 4749
		public extern Material material
		{
			[MethodImpl(4096)]
			get;
			[MethodImpl(4096)]
			set;
		}

		// Token: 0x170003C6 RID: 966
		// (get) Token: 0x0600128E RID: 4750
		// (set) Token: 0x0600128F RID: 4751
		public extern Material initializationMaterial
		{
			[MethodImpl(4096)]
			get;
			[MethodImpl(4096)]
			set;
		}

		// Token: 0x170003C7 RID: 967
		// (get) Token: 0x06001290 RID: 4752
		// (set) Token: 0x06001291 RID: 4753
		public extern Texture initializationTexture
		{
			[MethodImpl(4096)]
			get;
			[MethodImpl(4096)]
			set;
		}

		// Token: 0x06001292 RID: 4754
		[FreeFunction(Name = "CustomRenderTextureScripting::GetUpdateZonesInternal", HasExplicitThis = true)]
		[MethodImpl(4096)]
		internal extern void GetUpdateZonesInternal([NotNull("ArgumentNullException")] object updateZones);

		// Token: 0x06001293 RID: 4755 RVA: 0x00019006 File Offset: 0x00017206
		public void GetUpdateZones(List<CustomRenderTextureUpdateZone> updateZones)
		{
			this.GetUpdateZonesInternal(updateZones);
		}

		// Token: 0x06001294 RID: 4756
		[FreeFunction(Name = "CustomRenderTextureScripting::SetUpdateZonesInternal", HasExplicitThis = true)]
		[MethodImpl(4096)]
		private extern void SetUpdateZonesInternal(CustomRenderTextureUpdateZone[] updateZones);

		// Token: 0x06001295 RID: 4757
		[FreeFunction(Name = "CustomRenderTextureScripting::GetDoubleBufferRenderTexture", HasExplicitThis = true)]
		[MethodImpl(4096)]
		public extern RenderTexture GetDoubleBufferRenderTexture();

		// Token: 0x06001296 RID: 4758
		[MethodImpl(4096)]
		public extern void EnsureDoubleBufferConsistency();

		// Token: 0x06001297 RID: 4759 RVA: 0x00019014 File Offset: 0x00017214
		public void SetUpdateZones(CustomRenderTextureUpdateZone[] updateZones)
		{
			bool flag = updateZones == null;
			if (flag)
			{
				throw new ArgumentNullException("updateZones");
			}
			this.SetUpdateZonesInternal(updateZones);
		}

		// Token: 0x170003C8 RID: 968
		// (get) Token: 0x06001298 RID: 4760
		// (set) Token: 0x06001299 RID: 4761
		public extern CustomRenderTextureInitializationSource initializationSource
		{
			[MethodImpl(4096)]
			get;
			[MethodImpl(4096)]
			set;
		}

		// Token: 0x170003C9 RID: 969
		// (get) Token: 0x0600129A RID: 4762 RVA: 0x00019040 File Offset: 0x00017240
		// (set) Token: 0x0600129B RID: 4763 RVA: 0x00019056 File Offset: 0x00017256
		public Color initializationColor
		{
			get
			{
				Color color;
				this.get_initializationColor_Injected(out color);
				return color;
			}
			set
			{
				this.set_initializationColor_Injected(ref value);
			}
		}

		// Token: 0x170003CA RID: 970
		// (get) Token: 0x0600129C RID: 4764
		// (set) Token: 0x0600129D RID: 4765
		public extern CustomRenderTextureUpdateMode updateMode
		{
			[MethodImpl(4096)]
			get;
			[MethodImpl(4096)]
			set;
		}

		// Token: 0x170003CB RID: 971
		// (get) Token: 0x0600129E RID: 4766
		// (set) Token: 0x0600129F RID: 4767
		public extern CustomRenderTextureUpdateMode initializationMode
		{
			[MethodImpl(4096)]
			get;
			[MethodImpl(4096)]
			set;
		}

		// Token: 0x170003CC RID: 972
		// (get) Token: 0x060012A0 RID: 4768
		// (set) Token: 0x060012A1 RID: 4769
		public extern CustomRenderTextureUpdateZoneSpace updateZoneSpace
		{
			[MethodImpl(4096)]
			get;
			[MethodImpl(4096)]
			set;
		}

		// Token: 0x170003CD RID: 973
		// (get) Token: 0x060012A2 RID: 4770
		// (set) Token: 0x060012A3 RID: 4771
		public extern int shaderPass
		{
			[MethodImpl(4096)]
			get;
			[MethodImpl(4096)]
			set;
		}

		// Token: 0x170003CE RID: 974
		// (get) Token: 0x060012A4 RID: 4772
		// (set) Token: 0x060012A5 RID: 4773
		public extern uint cubemapFaceMask
		{
			[MethodImpl(4096)]
			get;
			[MethodImpl(4096)]
			set;
		}

		// Token: 0x170003CF RID: 975
		// (get) Token: 0x060012A6 RID: 4774
		// (set) Token: 0x060012A7 RID: 4775
		public extern bool doubleBuffered
		{
			[MethodImpl(4096)]
			get;
			[MethodImpl(4096)]
			set;
		}

		// Token: 0x170003D0 RID: 976
		// (get) Token: 0x060012A8 RID: 4776
		// (set) Token: 0x060012A9 RID: 4777
		public extern bool wrapUpdateZones
		{
			[MethodImpl(4096)]
			get;
			[MethodImpl(4096)]
			set;
		}

		// Token: 0x170003D1 RID: 977
		// (get) Token: 0x060012AA RID: 4778
		// (set) Token: 0x060012AB RID: 4779
		public extern float updatePeriod
		{
			[MethodImpl(4096)]
			get;
			[MethodImpl(4096)]
			set;
		}

		// Token: 0x060012AC RID: 4780 RVA: 0x00019060 File Offset: 0x00017260
		public CustomRenderTexture(int width, int height, RenderTextureFormat format, [DefaultValue("RenderTextureReadWrite.Default")] RenderTextureReadWrite readWrite)
			: this(width, height, RenderTexture.GetCompatibleFormat(format, readWrite))
		{
		}

		// Token: 0x060012AD RID: 4781 RVA: 0x00019074 File Offset: 0x00017274
		[ExcludeFromDocs]
		public CustomRenderTexture(int width, int height, RenderTextureFormat format)
			: this(width, height, RenderTexture.GetCompatibleFormat(format, RenderTextureReadWrite.Default))
		{
		}

		// Token: 0x060012AE RID: 4782 RVA: 0x00019087 File Offset: 0x00017287
		[ExcludeFromDocs]
		public CustomRenderTexture(int width, int height)
			: this(width, height, SystemInfo.GetGraphicsFormat(DefaultFormat.LDR))
		{
		}

		// Token: 0x060012AF RID: 4783 RVA: 0x00019099 File Offset: 0x00017299
		[ExcludeFromDocs]
		public CustomRenderTexture(int width, int height, [DefaultValue("DefaultFormat.LDR")] DefaultFormat defaultFormat)
			: this(width, height, SystemInfo.GetGraphicsFormat(defaultFormat))
		{
		}

		// Token: 0x060012B0 RID: 4784 RVA: 0x000190AC File Offset: 0x000172AC
		[ExcludeFromDocs]
		public CustomRenderTexture(int width, int height, GraphicsFormat format)
		{
			bool flag = !base.ValidateFormat(format, FormatUsage.Render);
			if (!flag)
			{
				CustomRenderTexture.Internal_CreateCustomRenderTexture(this);
				this.width = width;
				this.height = height;
				base.graphicsFormat = format;
				base.SetSRGBReadWrite(GraphicsFormatUtility.IsSRGBFormat(format));
			}
		}

		// Token: 0x060012B1 RID: 4785
		[MethodImpl(4096)]
		private extern void get_initializationColor_Injected(out Color ret);

		// Token: 0x060012B2 RID: 4786
		[MethodImpl(4096)]
		private extern void set_initializationColor_Injected(ref Color value);
	}
}
