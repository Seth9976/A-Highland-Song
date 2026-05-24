using System;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Internal;
using UnityEngine.Rendering;

namespace UnityEngine
{
	// Token: 0x020001AC RID: 428
	public struct RenderTextureDescriptor
	{
		// Token: 0x170003D2 RID: 978
		// (get) Token: 0x060012B3 RID: 4787 RVA: 0x000190FE File Offset: 0x000172FE
		// (set) Token: 0x060012B4 RID: 4788 RVA: 0x00019106 File Offset: 0x00017306
		public int width { readonly get; set; }

		// Token: 0x170003D3 RID: 979
		// (get) Token: 0x060012B5 RID: 4789 RVA: 0x0001910F File Offset: 0x0001730F
		// (set) Token: 0x060012B6 RID: 4790 RVA: 0x00019117 File Offset: 0x00017317
		public int height { readonly get; set; }

		// Token: 0x170003D4 RID: 980
		// (get) Token: 0x060012B7 RID: 4791 RVA: 0x00019120 File Offset: 0x00017320
		// (set) Token: 0x060012B8 RID: 4792 RVA: 0x00019128 File Offset: 0x00017328
		public int msaaSamples { readonly get; set; }

		// Token: 0x170003D5 RID: 981
		// (get) Token: 0x060012B9 RID: 4793 RVA: 0x00019131 File Offset: 0x00017331
		// (set) Token: 0x060012BA RID: 4794 RVA: 0x00019139 File Offset: 0x00017339
		public int volumeDepth { readonly get; set; }

		// Token: 0x170003D6 RID: 982
		// (get) Token: 0x060012BB RID: 4795 RVA: 0x00019142 File Offset: 0x00017342
		// (set) Token: 0x060012BC RID: 4796 RVA: 0x0001914A File Offset: 0x0001734A
		public int mipCount { readonly get; set; }

		// Token: 0x170003D7 RID: 983
		// (get) Token: 0x060012BD RID: 4797 RVA: 0x00019154 File Offset: 0x00017354
		// (set) Token: 0x060012BE RID: 4798 RVA: 0x0001916C File Offset: 0x0001736C
		public GraphicsFormat graphicsFormat
		{
			get
			{
				return this._graphicsFormat;
			}
			set
			{
				this._graphicsFormat = value;
				this.SetOrClearRenderTextureCreationFlag(GraphicsFormatUtility.IsSRGBFormat(value), RenderTextureCreationFlags.SRGB);
				this.depthBufferBits = this.depthBufferBits;
			}
		}

		// Token: 0x170003D8 RID: 984
		// (get) Token: 0x060012BF RID: 4799 RVA: 0x00019191 File Offset: 0x00017391
		// (set) Token: 0x060012C0 RID: 4800 RVA: 0x00019199 File Offset: 0x00017399
		public GraphicsFormat stencilFormat { readonly get; set; }

		// Token: 0x170003D9 RID: 985
		// (get) Token: 0x060012C1 RID: 4801 RVA: 0x000191A2 File Offset: 0x000173A2
		// (set) Token: 0x060012C2 RID: 4802 RVA: 0x000191AA File Offset: 0x000173AA
		public GraphicsFormat depthStencilFormat { readonly get; set; }

		// Token: 0x170003DA RID: 986
		// (get) Token: 0x060012C3 RID: 4803 RVA: 0x000191B4 File Offset: 0x000173B4
		// (set) Token: 0x060012C4 RID: 4804 RVA: 0x000191D4 File Offset: 0x000173D4
		public RenderTextureFormat colorFormat
		{
			get
			{
				return GraphicsFormatUtility.GetRenderTextureFormat(this.graphicsFormat);
			}
			set
			{
				GraphicsFormat graphicsFormat = GraphicsFormatUtility.GetGraphicsFormat(value, this.sRGB);
				this.graphicsFormat = SystemInfo.GetCompatibleFormat(graphicsFormat, FormatUsage.Render);
			}
		}

		// Token: 0x170003DB RID: 987
		// (get) Token: 0x060012C5 RID: 4805 RVA: 0x00019200 File Offset: 0x00017400
		// (set) Token: 0x060012C6 RID: 4806 RVA: 0x0001921D File Offset: 0x0001741D
		public bool sRGB
		{
			get
			{
				return GraphicsFormatUtility.IsSRGBFormat(this.graphicsFormat);
			}
			set
			{
				this.graphicsFormat = GraphicsFormatUtility.GetGraphicsFormat(this.colorFormat, value);
			}
		}

		// Token: 0x170003DC RID: 988
		// (get) Token: 0x060012C7 RID: 4807 RVA: 0x00019234 File Offset: 0x00017434
		// (set) Token: 0x060012C8 RID: 4808 RVA: 0x00019251 File Offset: 0x00017451
		public int depthBufferBits
		{
			get
			{
				return GraphicsFormatUtility.GetDepthBits(this.depthStencilFormat);
			}
			set
			{
				this.depthStencilFormat = RenderTexture.GetDepthStencilFormatLegacy(value, this.graphicsFormat);
			}
		}

		// Token: 0x170003DD RID: 989
		// (get) Token: 0x060012C9 RID: 4809 RVA: 0x00019267 File Offset: 0x00017467
		// (set) Token: 0x060012CA RID: 4810 RVA: 0x0001926F File Offset: 0x0001746F
		public TextureDimension dimension { readonly get; set; }

		// Token: 0x170003DE RID: 990
		// (get) Token: 0x060012CB RID: 4811 RVA: 0x00019278 File Offset: 0x00017478
		// (set) Token: 0x060012CC RID: 4812 RVA: 0x00019280 File Offset: 0x00017480
		public ShadowSamplingMode shadowSamplingMode { readonly get; set; }

		// Token: 0x170003DF RID: 991
		// (get) Token: 0x060012CD RID: 4813 RVA: 0x00019289 File Offset: 0x00017489
		// (set) Token: 0x060012CE RID: 4814 RVA: 0x00019291 File Offset: 0x00017491
		public VRTextureUsage vrUsage { readonly get; set; }

		// Token: 0x170003E0 RID: 992
		// (get) Token: 0x060012CF RID: 4815 RVA: 0x0001929C File Offset: 0x0001749C
		public RenderTextureCreationFlags flags
		{
			get
			{
				return this._flags;
			}
		}

		// Token: 0x170003E1 RID: 993
		// (get) Token: 0x060012D0 RID: 4816 RVA: 0x000192B4 File Offset: 0x000174B4
		// (set) Token: 0x060012D1 RID: 4817 RVA: 0x000192BC File Offset: 0x000174BC
		public RenderTextureMemoryless memoryless { readonly get; set; }

		// Token: 0x060012D2 RID: 4818 RVA: 0x000192C5 File Offset: 0x000174C5
		public RenderTextureDescriptor(int width, int height)
		{
			this = new RenderTextureDescriptor(width, height, RenderTextureFormat.Default);
		}

		// Token: 0x060012D3 RID: 4819 RVA: 0x000192D2 File Offset: 0x000174D2
		public RenderTextureDescriptor(int width, int height, RenderTextureFormat colorFormat)
		{
			this = new RenderTextureDescriptor(width, height, colorFormat, 0);
		}

		// Token: 0x060012D4 RID: 4820 RVA: 0x000192E0 File Offset: 0x000174E0
		public RenderTextureDescriptor(int width, int height, RenderTextureFormat colorFormat, int depthBufferBits)
		{
			this = new RenderTextureDescriptor(width, height, colorFormat, depthBufferBits, Texture.GenerateAllMips);
		}

		// Token: 0x060012D5 RID: 4821 RVA: 0x000192F4 File Offset: 0x000174F4
		[ExcludeFromDocs]
		public RenderTextureDescriptor(int width, int height, GraphicsFormat colorFormat, int depthBufferBits)
		{
			this = new RenderTextureDescriptor(width, height, colorFormat, depthBufferBits, Texture.GenerateAllMips);
		}

		// Token: 0x060012D6 RID: 4822 RVA: 0x00019308 File Offset: 0x00017508
		public RenderTextureDescriptor(int width, int height, RenderTextureFormat colorFormat, int depthBufferBits, int mipCount)
		{
			GraphicsFormat graphicsFormat = GraphicsFormatUtility.GetGraphicsFormat(colorFormat, false);
			this = new RenderTextureDescriptor(width, height, SystemInfo.GetCompatibleFormat(graphicsFormat, FormatUsage.Render), depthBufferBits, mipCount);
		}

		// Token: 0x060012D7 RID: 4823 RVA: 0x00019338 File Offset: 0x00017538
		[ExcludeFromDocs]
		public RenderTextureDescriptor(int width, int height, GraphicsFormat colorFormat, int depthBufferBits, int mipCount)
		{
			this = default(RenderTextureDescriptor);
			this._flags = RenderTextureCreationFlags.AutoGenerateMips | RenderTextureCreationFlags.AllowVerticalFlip;
			this.width = width;
			this.height = height;
			this.volumeDepth = 1;
			this.msaaSamples = 1;
			this.graphicsFormat = colorFormat;
			this.depthStencilFormat = RenderTexture.GetDepthStencilFormatLegacy(depthBufferBits, colorFormat);
			this.mipCount = mipCount;
			this.dimension = TextureDimension.Tex2D;
			this.shadowSamplingMode = ShadowSamplingMode.None;
			this.vrUsage = VRTextureUsage.None;
			this.memoryless = RenderTextureMemoryless.None;
		}

		// Token: 0x060012D8 RID: 4824 RVA: 0x000193B8 File Offset: 0x000175B8
		[ExcludeFromDocs]
		public RenderTextureDescriptor(int width, int height, GraphicsFormat colorFormat, GraphicsFormat depthStencilFormat)
		{
			this = new RenderTextureDescriptor(width, height, colorFormat, depthStencilFormat, Texture.GenerateAllMips);
		}

		// Token: 0x060012D9 RID: 4825 RVA: 0x000193CC File Offset: 0x000175CC
		[ExcludeFromDocs]
		public RenderTextureDescriptor(int width, int height, GraphicsFormat colorFormat, GraphicsFormat depthStencilFormat, int mipCount)
		{
			this = default(RenderTextureDescriptor);
			this._flags = RenderTextureCreationFlags.AutoGenerateMips | RenderTextureCreationFlags.AllowVerticalFlip;
			this.width = width;
			this.height = height;
			this.volumeDepth = 1;
			this.msaaSamples = 1;
			this.graphicsFormat = colorFormat;
			this.depthStencilFormat = depthStencilFormat;
			this.mipCount = mipCount;
			this.dimension = TextureDimension.Tex2D;
			this.shadowSamplingMode = ShadowSamplingMode.None;
			this.vrUsage = VRTextureUsage.None;
			this.memoryless = RenderTextureMemoryless.None;
		}

		// Token: 0x060012DA RID: 4826 RVA: 0x00019448 File Offset: 0x00017648
		private void SetOrClearRenderTextureCreationFlag(bool value, RenderTextureCreationFlags flag)
		{
			if (value)
			{
				this._flags |= flag;
			}
			else
			{
				this._flags &= ~flag;
			}
		}

		// Token: 0x170003E2 RID: 994
		// (get) Token: 0x060012DB RID: 4827 RVA: 0x00019480 File Offset: 0x00017680
		// (set) Token: 0x060012DC RID: 4828 RVA: 0x0001949D File Offset: 0x0001769D
		public bool useMipMap
		{
			get
			{
				return (this._flags & RenderTextureCreationFlags.MipMap) > (RenderTextureCreationFlags)0;
			}
			set
			{
				this.SetOrClearRenderTextureCreationFlag(value, RenderTextureCreationFlags.MipMap);
			}
		}

		// Token: 0x170003E3 RID: 995
		// (get) Token: 0x060012DD RID: 4829 RVA: 0x000194AC File Offset: 0x000176AC
		// (set) Token: 0x060012DE RID: 4830 RVA: 0x000194C9 File Offset: 0x000176C9
		public bool autoGenerateMips
		{
			get
			{
				return (this._flags & RenderTextureCreationFlags.AutoGenerateMips) > (RenderTextureCreationFlags)0;
			}
			set
			{
				this.SetOrClearRenderTextureCreationFlag(value, RenderTextureCreationFlags.AutoGenerateMips);
			}
		}

		// Token: 0x170003E4 RID: 996
		// (get) Token: 0x060012DF RID: 4831 RVA: 0x000194D8 File Offset: 0x000176D8
		// (set) Token: 0x060012E0 RID: 4832 RVA: 0x000194F6 File Offset: 0x000176F6
		public bool enableRandomWrite
		{
			get
			{
				return (this._flags & RenderTextureCreationFlags.EnableRandomWrite) > (RenderTextureCreationFlags)0;
			}
			set
			{
				this.SetOrClearRenderTextureCreationFlag(value, RenderTextureCreationFlags.EnableRandomWrite);
			}
		}

		// Token: 0x170003E5 RID: 997
		// (get) Token: 0x060012E1 RID: 4833 RVA: 0x00019504 File Offset: 0x00017704
		// (set) Token: 0x060012E2 RID: 4834 RVA: 0x00019525 File Offset: 0x00017725
		public bool bindMS
		{
			get
			{
				return (this._flags & RenderTextureCreationFlags.BindMS) > (RenderTextureCreationFlags)0;
			}
			set
			{
				this.SetOrClearRenderTextureCreationFlag(value, RenderTextureCreationFlags.BindMS);
			}
		}

		// Token: 0x170003E6 RID: 998
		// (get) Token: 0x060012E3 RID: 4835 RVA: 0x00019538 File Offset: 0x00017738
		// (set) Token: 0x060012E4 RID: 4836 RVA: 0x00019556 File Offset: 0x00017756
		internal bool createdFromScript
		{
			get
			{
				return (this._flags & RenderTextureCreationFlags.CreatedFromScript) > (RenderTextureCreationFlags)0;
			}
			set
			{
				this.SetOrClearRenderTextureCreationFlag(value, RenderTextureCreationFlags.CreatedFromScript);
			}
		}

		// Token: 0x170003E7 RID: 999
		// (get) Token: 0x060012E5 RID: 4837 RVA: 0x00019564 File Offset: 0x00017764
		// (set) Token: 0x060012E6 RID: 4838 RVA: 0x00019585 File Offset: 0x00017785
		public bool useDynamicScale
		{
			get
			{
				return (this._flags & RenderTextureCreationFlags.DynamicallyScalable) > (RenderTextureCreationFlags)0;
			}
			set
			{
				this.SetOrClearRenderTextureCreationFlag(value, RenderTextureCreationFlags.DynamicallyScalable);
			}
		}

		// Token: 0x040005CC RID: 1484
		private GraphicsFormat _graphicsFormat;

		// Token: 0x040005D2 RID: 1490
		private RenderTextureCreationFlags _flags;
	}
}
