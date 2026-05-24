using System;

namespace UnityEngine.Rendering
{
	// Token: 0x020003C3 RID: 963
	public struct RenderTargetBinding
	{
		// Token: 0x17000607 RID: 1543
		// (get) Token: 0x06001F7F RID: 8063 RVA: 0x00033874 File Offset: 0x00031A74
		// (set) Token: 0x06001F80 RID: 8064 RVA: 0x0003388C File Offset: 0x00031A8C
		public RenderTargetIdentifier[] colorRenderTargets
		{
			get
			{
				return this.m_ColorRenderTargets;
			}
			set
			{
				this.m_ColorRenderTargets = value;
			}
		}

		// Token: 0x17000608 RID: 1544
		// (get) Token: 0x06001F81 RID: 8065 RVA: 0x00033898 File Offset: 0x00031A98
		// (set) Token: 0x06001F82 RID: 8066 RVA: 0x000338B0 File Offset: 0x00031AB0
		public RenderTargetIdentifier depthRenderTarget
		{
			get
			{
				return this.m_DepthRenderTarget;
			}
			set
			{
				this.m_DepthRenderTarget = value;
			}
		}

		// Token: 0x17000609 RID: 1545
		// (get) Token: 0x06001F83 RID: 8067 RVA: 0x000338BC File Offset: 0x00031ABC
		// (set) Token: 0x06001F84 RID: 8068 RVA: 0x000338D4 File Offset: 0x00031AD4
		public RenderBufferLoadAction[] colorLoadActions
		{
			get
			{
				return this.m_ColorLoadActions;
			}
			set
			{
				this.m_ColorLoadActions = value;
			}
		}

		// Token: 0x1700060A RID: 1546
		// (get) Token: 0x06001F85 RID: 8069 RVA: 0x000338E0 File Offset: 0x00031AE0
		// (set) Token: 0x06001F86 RID: 8070 RVA: 0x000338F8 File Offset: 0x00031AF8
		public RenderBufferStoreAction[] colorStoreActions
		{
			get
			{
				return this.m_ColorStoreActions;
			}
			set
			{
				this.m_ColorStoreActions = value;
			}
		}

		// Token: 0x1700060B RID: 1547
		// (get) Token: 0x06001F87 RID: 8071 RVA: 0x00033904 File Offset: 0x00031B04
		// (set) Token: 0x06001F88 RID: 8072 RVA: 0x0003391C File Offset: 0x00031B1C
		public RenderBufferLoadAction depthLoadAction
		{
			get
			{
				return this.m_DepthLoadAction;
			}
			set
			{
				this.m_DepthLoadAction = value;
			}
		}

		// Token: 0x1700060C RID: 1548
		// (get) Token: 0x06001F89 RID: 8073 RVA: 0x00033928 File Offset: 0x00031B28
		// (set) Token: 0x06001F8A RID: 8074 RVA: 0x00033940 File Offset: 0x00031B40
		public RenderBufferStoreAction depthStoreAction
		{
			get
			{
				return this.m_DepthStoreAction;
			}
			set
			{
				this.m_DepthStoreAction = value;
			}
		}

		// Token: 0x1700060D RID: 1549
		// (get) Token: 0x06001F8B RID: 8075 RVA: 0x0003394C File Offset: 0x00031B4C
		// (set) Token: 0x06001F8C RID: 8076 RVA: 0x00033964 File Offset: 0x00031B64
		public RenderTargetFlags flags
		{
			get
			{
				return this.m_Flags;
			}
			set
			{
				this.m_Flags = value;
			}
		}

		// Token: 0x06001F8D RID: 8077 RVA: 0x0003396E File Offset: 0x00031B6E
		public RenderTargetBinding(RenderTargetIdentifier[] colorRenderTargets, RenderBufferLoadAction[] colorLoadActions, RenderBufferStoreAction[] colorStoreActions, RenderTargetIdentifier depthRenderTarget, RenderBufferLoadAction depthLoadAction, RenderBufferStoreAction depthStoreAction)
		{
			this.m_ColorRenderTargets = colorRenderTargets;
			this.m_DepthRenderTarget = depthRenderTarget;
			this.m_ColorLoadActions = colorLoadActions;
			this.m_ColorStoreActions = colorStoreActions;
			this.m_DepthLoadAction = depthLoadAction;
			this.m_DepthStoreAction = depthStoreAction;
			this.m_Flags = RenderTargetFlags.None;
		}

		// Token: 0x06001F8E RID: 8078 RVA: 0x000339A5 File Offset: 0x00031BA5
		public RenderTargetBinding(RenderTargetIdentifier colorRenderTarget, RenderBufferLoadAction colorLoadAction, RenderBufferStoreAction colorStoreAction, RenderTargetIdentifier depthRenderTarget, RenderBufferLoadAction depthLoadAction, RenderBufferStoreAction depthStoreAction)
		{
			this = new RenderTargetBinding(new RenderTargetIdentifier[] { colorRenderTarget }, new RenderBufferLoadAction[] { colorLoadAction }, new RenderBufferStoreAction[] { colorStoreAction }, depthRenderTarget, depthLoadAction, depthStoreAction);
		}

		// Token: 0x06001F8F RID: 8079 RVA: 0x000339D8 File Offset: 0x00031BD8
		public RenderTargetBinding(RenderTargetSetup setup)
		{
			this.m_ColorRenderTargets = new RenderTargetIdentifier[setup.color.Length];
			for (int i = 0; i < this.m_ColorRenderTargets.Length; i++)
			{
				this.m_ColorRenderTargets[i] = new RenderTargetIdentifier(setup.color[i], setup.mipLevel, setup.cubemapFace, setup.depthSlice);
			}
			this.m_DepthRenderTarget = setup.depth;
			this.m_ColorLoadActions = (RenderBufferLoadAction[])setup.colorLoad.Clone();
			this.m_ColorStoreActions = (RenderBufferStoreAction[])setup.colorStore.Clone();
			this.m_DepthLoadAction = setup.depthLoad;
			this.m_DepthStoreAction = setup.depthStore;
			this.m_Flags = RenderTargetFlags.None;
		}

		// Token: 0x04000B81 RID: 2945
		private RenderTargetIdentifier[] m_ColorRenderTargets;

		// Token: 0x04000B82 RID: 2946
		private RenderTargetIdentifier m_DepthRenderTarget;

		// Token: 0x04000B83 RID: 2947
		private RenderBufferLoadAction[] m_ColorLoadActions;

		// Token: 0x04000B84 RID: 2948
		private RenderBufferStoreAction[] m_ColorStoreActions;

		// Token: 0x04000B85 RID: 2949
		private RenderBufferLoadAction m_DepthLoadAction;

		// Token: 0x04000B86 RID: 2950
		private RenderBufferStoreAction m_DepthStoreAction;

		// Token: 0x04000B87 RID: 2951
		private RenderTargetFlags m_Flags;
	}
}
