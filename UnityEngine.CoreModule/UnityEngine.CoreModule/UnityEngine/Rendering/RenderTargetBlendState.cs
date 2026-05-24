using System;

namespace UnityEngine.Rendering
{
	// Token: 0x0200040A RID: 1034
	public struct RenderTargetBlendState : IEquatable<RenderTargetBlendState>
	{
		// Token: 0x17000695 RID: 1685
		// (get) Token: 0x06002350 RID: 9040 RVA: 0x0003B938 File Offset: 0x00039B38
		public static RenderTargetBlendState defaultValue
		{
			get
			{
				return new RenderTargetBlendState(ColorWriteMask.All, BlendMode.One, BlendMode.Zero, BlendMode.One, BlendMode.Zero, BlendOp.Add, BlendOp.Add);
			}
		}

		// Token: 0x06002351 RID: 9041 RVA: 0x0003B958 File Offset: 0x00039B58
		public RenderTargetBlendState(ColorWriteMask writeMask = ColorWriteMask.All, BlendMode sourceColorBlendMode = BlendMode.One, BlendMode destinationColorBlendMode = BlendMode.Zero, BlendMode sourceAlphaBlendMode = BlendMode.One, BlendMode destinationAlphaBlendMode = BlendMode.Zero, BlendOp colorBlendOperation = BlendOp.Add, BlendOp alphaBlendOperation = BlendOp.Add)
		{
			this.m_WriteMask = (byte)writeMask;
			this.m_SourceColorBlendMode = (byte)sourceColorBlendMode;
			this.m_DestinationColorBlendMode = (byte)destinationColorBlendMode;
			this.m_SourceAlphaBlendMode = (byte)sourceAlphaBlendMode;
			this.m_DestinationAlphaBlendMode = (byte)destinationAlphaBlendMode;
			this.m_ColorBlendOperation = (byte)colorBlendOperation;
			this.m_AlphaBlendOperation = (byte)alphaBlendOperation;
			this.m_Padding = 0;
		}

		// Token: 0x17000696 RID: 1686
		// (get) Token: 0x06002352 RID: 9042 RVA: 0x0003B9AC File Offset: 0x00039BAC
		// (set) Token: 0x06002353 RID: 9043 RVA: 0x0003B9C4 File Offset: 0x00039BC4
		public ColorWriteMask writeMask
		{
			get
			{
				return (ColorWriteMask)this.m_WriteMask;
			}
			set
			{
				this.m_WriteMask = (byte)value;
			}
		}

		// Token: 0x17000697 RID: 1687
		// (get) Token: 0x06002354 RID: 9044 RVA: 0x0003B9D0 File Offset: 0x00039BD0
		// (set) Token: 0x06002355 RID: 9045 RVA: 0x0003B9E8 File Offset: 0x00039BE8
		public BlendMode sourceColorBlendMode
		{
			get
			{
				return (BlendMode)this.m_SourceColorBlendMode;
			}
			set
			{
				this.m_SourceColorBlendMode = (byte)value;
			}
		}

		// Token: 0x17000698 RID: 1688
		// (get) Token: 0x06002356 RID: 9046 RVA: 0x0003B9F4 File Offset: 0x00039BF4
		// (set) Token: 0x06002357 RID: 9047 RVA: 0x0003BA0C File Offset: 0x00039C0C
		public BlendMode destinationColorBlendMode
		{
			get
			{
				return (BlendMode)this.m_DestinationColorBlendMode;
			}
			set
			{
				this.m_DestinationColorBlendMode = (byte)value;
			}
		}

		// Token: 0x17000699 RID: 1689
		// (get) Token: 0x06002358 RID: 9048 RVA: 0x0003BA18 File Offset: 0x00039C18
		// (set) Token: 0x06002359 RID: 9049 RVA: 0x0003BA30 File Offset: 0x00039C30
		public BlendMode sourceAlphaBlendMode
		{
			get
			{
				return (BlendMode)this.m_SourceAlphaBlendMode;
			}
			set
			{
				this.m_SourceAlphaBlendMode = (byte)value;
			}
		}

		// Token: 0x1700069A RID: 1690
		// (get) Token: 0x0600235A RID: 9050 RVA: 0x0003BA3C File Offset: 0x00039C3C
		// (set) Token: 0x0600235B RID: 9051 RVA: 0x0003BA54 File Offset: 0x00039C54
		public BlendMode destinationAlphaBlendMode
		{
			get
			{
				return (BlendMode)this.m_DestinationAlphaBlendMode;
			}
			set
			{
				this.m_DestinationAlphaBlendMode = (byte)value;
			}
		}

		// Token: 0x1700069B RID: 1691
		// (get) Token: 0x0600235C RID: 9052 RVA: 0x0003BA60 File Offset: 0x00039C60
		// (set) Token: 0x0600235D RID: 9053 RVA: 0x0003BA78 File Offset: 0x00039C78
		public BlendOp colorBlendOperation
		{
			get
			{
				return (BlendOp)this.m_ColorBlendOperation;
			}
			set
			{
				this.m_ColorBlendOperation = (byte)value;
			}
		}

		// Token: 0x1700069C RID: 1692
		// (get) Token: 0x0600235E RID: 9054 RVA: 0x0003BA84 File Offset: 0x00039C84
		// (set) Token: 0x0600235F RID: 9055 RVA: 0x0003BA9C File Offset: 0x00039C9C
		public BlendOp alphaBlendOperation
		{
			get
			{
				return (BlendOp)this.m_AlphaBlendOperation;
			}
			set
			{
				this.m_AlphaBlendOperation = (byte)value;
			}
		}

		// Token: 0x06002360 RID: 9056 RVA: 0x0003BAA8 File Offset: 0x00039CA8
		public bool Equals(RenderTargetBlendState other)
		{
			return this.m_WriteMask == other.m_WriteMask && this.m_SourceColorBlendMode == other.m_SourceColorBlendMode && this.m_DestinationColorBlendMode == other.m_DestinationColorBlendMode && this.m_SourceAlphaBlendMode == other.m_SourceAlphaBlendMode && this.m_DestinationAlphaBlendMode == other.m_DestinationAlphaBlendMode && this.m_ColorBlendOperation == other.m_ColorBlendOperation && this.m_AlphaBlendOperation == other.m_AlphaBlendOperation;
		}

		// Token: 0x06002361 RID: 9057 RVA: 0x0003BB20 File Offset: 0x00039D20
		public override bool Equals(object obj)
		{
			bool flag = obj == null;
			return !flag && obj is RenderTargetBlendState && this.Equals((RenderTargetBlendState)obj);
		}

		// Token: 0x06002362 RID: 9058 RVA: 0x0003BB58 File Offset: 0x00039D58
		public override int GetHashCode()
		{
			int num = this.m_WriteMask.GetHashCode();
			num = (num * 397) ^ this.m_SourceColorBlendMode.GetHashCode();
			num = (num * 397) ^ this.m_DestinationColorBlendMode.GetHashCode();
			num = (num * 397) ^ this.m_SourceAlphaBlendMode.GetHashCode();
			num = (num * 397) ^ this.m_DestinationAlphaBlendMode.GetHashCode();
			num = (num * 397) ^ this.m_ColorBlendOperation.GetHashCode();
			return (num * 397) ^ this.m_AlphaBlendOperation.GetHashCode();
		}

		// Token: 0x06002363 RID: 9059 RVA: 0x0003BBF0 File Offset: 0x00039DF0
		public static bool operator ==(RenderTargetBlendState left, RenderTargetBlendState right)
		{
			return left.Equals(right);
		}

		// Token: 0x06002364 RID: 9060 RVA: 0x0003BC0C File Offset: 0x00039E0C
		public static bool operator !=(RenderTargetBlendState left, RenderTargetBlendState right)
		{
			return !left.Equals(right);
		}

		// Token: 0x04000D27 RID: 3367
		private byte m_WriteMask;

		// Token: 0x04000D28 RID: 3368
		private byte m_SourceColorBlendMode;

		// Token: 0x04000D29 RID: 3369
		private byte m_DestinationColorBlendMode;

		// Token: 0x04000D2A RID: 3370
		private byte m_SourceAlphaBlendMode;

		// Token: 0x04000D2B RID: 3371
		private byte m_DestinationAlphaBlendMode;

		// Token: 0x04000D2C RID: 3372
		private byte m_ColorBlendOperation;

		// Token: 0x04000D2D RID: 3373
		private byte m_AlphaBlendOperation;

		// Token: 0x04000D2E RID: 3374
		private byte m_Padding;
	}
}
