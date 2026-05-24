using System;

namespace UnityEngine.Rendering
{
	// Token: 0x02000408 RID: 1032
	public struct RenderStateBlock : IEquatable<RenderStateBlock>
	{
		// Token: 0x0600233E RID: 9022 RVA: 0x0003B69B File Offset: 0x0003989B
		public RenderStateBlock(RenderStateMask mask)
		{
			this.m_BlendState = BlendState.defaultValue;
			this.m_RasterState = RasterState.defaultValue;
			this.m_DepthState = DepthState.defaultValue;
			this.m_StencilState = StencilState.defaultValue;
			this.m_StencilReference = 0;
			this.m_Mask = mask;
		}

		// Token: 0x1700068F RID: 1679
		// (get) Token: 0x0600233F RID: 9023 RVA: 0x0003B6D8 File Offset: 0x000398D8
		// (set) Token: 0x06002340 RID: 9024 RVA: 0x0003B6F0 File Offset: 0x000398F0
		public BlendState blendState
		{
			get
			{
				return this.m_BlendState;
			}
			set
			{
				this.m_BlendState = value;
			}
		}

		// Token: 0x17000690 RID: 1680
		// (get) Token: 0x06002341 RID: 9025 RVA: 0x0003B6FC File Offset: 0x000398FC
		// (set) Token: 0x06002342 RID: 9026 RVA: 0x0003B714 File Offset: 0x00039914
		public RasterState rasterState
		{
			get
			{
				return this.m_RasterState;
			}
			set
			{
				this.m_RasterState = value;
			}
		}

		// Token: 0x17000691 RID: 1681
		// (get) Token: 0x06002343 RID: 9027 RVA: 0x0003B720 File Offset: 0x00039920
		// (set) Token: 0x06002344 RID: 9028 RVA: 0x0003B738 File Offset: 0x00039938
		public DepthState depthState
		{
			get
			{
				return this.m_DepthState;
			}
			set
			{
				this.m_DepthState = value;
			}
		}

		// Token: 0x17000692 RID: 1682
		// (get) Token: 0x06002345 RID: 9029 RVA: 0x0003B744 File Offset: 0x00039944
		// (set) Token: 0x06002346 RID: 9030 RVA: 0x0003B75C File Offset: 0x0003995C
		public StencilState stencilState
		{
			get
			{
				return this.m_StencilState;
			}
			set
			{
				this.m_StencilState = value;
			}
		}

		// Token: 0x17000693 RID: 1683
		// (get) Token: 0x06002347 RID: 9031 RVA: 0x0003B768 File Offset: 0x00039968
		// (set) Token: 0x06002348 RID: 9032 RVA: 0x0003B780 File Offset: 0x00039980
		public int stencilReference
		{
			get
			{
				return this.m_StencilReference;
			}
			set
			{
				this.m_StencilReference = value;
			}
		}

		// Token: 0x17000694 RID: 1684
		// (get) Token: 0x06002349 RID: 9033 RVA: 0x0003B78C File Offset: 0x0003998C
		// (set) Token: 0x0600234A RID: 9034 RVA: 0x0003B7A4 File Offset: 0x000399A4
		public RenderStateMask mask
		{
			get
			{
				return this.m_Mask;
			}
			set
			{
				this.m_Mask = value;
			}
		}

		// Token: 0x0600234B RID: 9035 RVA: 0x0003B7B0 File Offset: 0x000399B0
		public bool Equals(RenderStateBlock other)
		{
			return this.m_BlendState.Equals(other.m_BlendState) && this.m_RasterState.Equals(other.m_RasterState) && this.m_DepthState.Equals(other.m_DepthState) && this.m_StencilState.Equals(other.m_StencilState) && this.m_StencilReference == other.m_StencilReference && this.m_Mask == other.m_Mask;
		}

		// Token: 0x0600234C RID: 9036 RVA: 0x0003B830 File Offset: 0x00039A30
		public override bool Equals(object obj)
		{
			bool flag = obj == null;
			return !flag && obj is RenderStateBlock && this.Equals((RenderStateBlock)obj);
		}

		// Token: 0x0600234D RID: 9037 RVA: 0x0003B868 File Offset: 0x00039A68
		public override int GetHashCode()
		{
			int num = this.m_BlendState.GetHashCode();
			num = (num * 397) ^ this.m_RasterState.GetHashCode();
			num = (num * 397) ^ this.m_DepthState.GetHashCode();
			num = (num * 397) ^ this.m_StencilState.GetHashCode();
			num = (num * 397) ^ this.m_StencilReference;
			return (num * 397) ^ (int)this.m_Mask;
		}

		// Token: 0x0600234E RID: 9038 RVA: 0x0003B8FC File Offset: 0x00039AFC
		public static bool operator ==(RenderStateBlock left, RenderStateBlock right)
		{
			return left.Equals(right);
		}

		// Token: 0x0600234F RID: 9039 RVA: 0x0003B918 File Offset: 0x00039B18
		public static bool operator !=(RenderStateBlock left, RenderStateBlock right)
		{
			return !left.Equals(right);
		}

		// Token: 0x04000D1A RID: 3354
		private BlendState m_BlendState;

		// Token: 0x04000D1B RID: 3355
		private RasterState m_RasterState;

		// Token: 0x04000D1C RID: 3356
		private DepthState m_DepthState;

		// Token: 0x04000D1D RID: 3357
		private StencilState m_StencilState;

		// Token: 0x04000D1E RID: 3358
		private int m_StencilReference;

		// Token: 0x04000D1F RID: 3359
		private RenderStateMask m_Mask;
	}
}
