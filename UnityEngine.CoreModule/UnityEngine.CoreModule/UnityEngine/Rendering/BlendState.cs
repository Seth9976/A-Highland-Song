using System;

namespace UnityEngine.Rendering
{
	// Token: 0x020003ED RID: 1005
	public struct BlendState : IEquatable<BlendState>
	{
		// Token: 0x17000631 RID: 1585
		// (get) Token: 0x06002213 RID: 8723 RVA: 0x0003850C File Offset: 0x0003670C
		public static BlendState defaultValue
		{
			get
			{
				return new BlendState(false, false);
			}
		}

		// Token: 0x06002214 RID: 8724 RVA: 0x00038528 File Offset: 0x00036728
		public BlendState(bool separateMRTBlend = false, bool alphaToMask = false)
		{
			this.m_BlendState0 = RenderTargetBlendState.defaultValue;
			this.m_BlendState1 = RenderTargetBlendState.defaultValue;
			this.m_BlendState2 = RenderTargetBlendState.defaultValue;
			this.m_BlendState3 = RenderTargetBlendState.defaultValue;
			this.m_BlendState4 = RenderTargetBlendState.defaultValue;
			this.m_BlendState5 = RenderTargetBlendState.defaultValue;
			this.m_BlendState6 = RenderTargetBlendState.defaultValue;
			this.m_BlendState7 = RenderTargetBlendState.defaultValue;
			this.m_SeparateMRTBlendStates = Convert.ToByte(separateMRTBlend);
			this.m_AlphaToMask = Convert.ToByte(alphaToMask);
			this.m_Padding = 0;
		}

		// Token: 0x17000632 RID: 1586
		// (get) Token: 0x06002215 RID: 8725 RVA: 0x000385B0 File Offset: 0x000367B0
		// (set) Token: 0x06002216 RID: 8726 RVA: 0x000385CD File Offset: 0x000367CD
		public bool separateMRTBlendStates
		{
			get
			{
				return Convert.ToBoolean(this.m_SeparateMRTBlendStates);
			}
			set
			{
				this.m_SeparateMRTBlendStates = Convert.ToByte(value);
			}
		}

		// Token: 0x17000633 RID: 1587
		// (get) Token: 0x06002217 RID: 8727 RVA: 0x000385DC File Offset: 0x000367DC
		// (set) Token: 0x06002218 RID: 8728 RVA: 0x000385F9 File Offset: 0x000367F9
		public bool alphaToMask
		{
			get
			{
				return Convert.ToBoolean(this.m_AlphaToMask);
			}
			set
			{
				this.m_AlphaToMask = Convert.ToByte(value);
			}
		}

		// Token: 0x17000634 RID: 1588
		// (get) Token: 0x06002219 RID: 8729 RVA: 0x00038608 File Offset: 0x00036808
		// (set) Token: 0x0600221A RID: 8730 RVA: 0x00038620 File Offset: 0x00036820
		public RenderTargetBlendState blendState0
		{
			get
			{
				return this.m_BlendState0;
			}
			set
			{
				this.m_BlendState0 = value;
			}
		}

		// Token: 0x17000635 RID: 1589
		// (get) Token: 0x0600221B RID: 8731 RVA: 0x0003862C File Offset: 0x0003682C
		// (set) Token: 0x0600221C RID: 8732 RVA: 0x00038644 File Offset: 0x00036844
		public RenderTargetBlendState blendState1
		{
			get
			{
				return this.m_BlendState1;
			}
			set
			{
				this.m_BlendState1 = value;
			}
		}

		// Token: 0x17000636 RID: 1590
		// (get) Token: 0x0600221D RID: 8733 RVA: 0x00038650 File Offset: 0x00036850
		// (set) Token: 0x0600221E RID: 8734 RVA: 0x00038668 File Offset: 0x00036868
		public RenderTargetBlendState blendState2
		{
			get
			{
				return this.m_BlendState2;
			}
			set
			{
				this.m_BlendState2 = value;
			}
		}

		// Token: 0x17000637 RID: 1591
		// (get) Token: 0x0600221F RID: 8735 RVA: 0x00038674 File Offset: 0x00036874
		// (set) Token: 0x06002220 RID: 8736 RVA: 0x0003868C File Offset: 0x0003688C
		public RenderTargetBlendState blendState3
		{
			get
			{
				return this.m_BlendState3;
			}
			set
			{
				this.m_BlendState3 = value;
			}
		}

		// Token: 0x17000638 RID: 1592
		// (get) Token: 0x06002221 RID: 8737 RVA: 0x00038698 File Offset: 0x00036898
		// (set) Token: 0x06002222 RID: 8738 RVA: 0x000386B0 File Offset: 0x000368B0
		public RenderTargetBlendState blendState4
		{
			get
			{
				return this.m_BlendState4;
			}
			set
			{
				this.m_BlendState4 = value;
			}
		}

		// Token: 0x17000639 RID: 1593
		// (get) Token: 0x06002223 RID: 8739 RVA: 0x000386BC File Offset: 0x000368BC
		// (set) Token: 0x06002224 RID: 8740 RVA: 0x000386D4 File Offset: 0x000368D4
		public RenderTargetBlendState blendState5
		{
			get
			{
				return this.m_BlendState5;
			}
			set
			{
				this.m_BlendState5 = value;
			}
		}

		// Token: 0x1700063A RID: 1594
		// (get) Token: 0x06002225 RID: 8741 RVA: 0x000386E0 File Offset: 0x000368E0
		// (set) Token: 0x06002226 RID: 8742 RVA: 0x000386F8 File Offset: 0x000368F8
		public RenderTargetBlendState blendState6
		{
			get
			{
				return this.m_BlendState6;
			}
			set
			{
				this.m_BlendState6 = value;
			}
		}

		// Token: 0x1700063B RID: 1595
		// (get) Token: 0x06002227 RID: 8743 RVA: 0x00038704 File Offset: 0x00036904
		// (set) Token: 0x06002228 RID: 8744 RVA: 0x0003871C File Offset: 0x0003691C
		public RenderTargetBlendState blendState7
		{
			get
			{
				return this.m_BlendState7;
			}
			set
			{
				this.m_BlendState7 = value;
			}
		}

		// Token: 0x06002229 RID: 8745 RVA: 0x00038728 File Offset: 0x00036928
		public bool Equals(BlendState other)
		{
			return this.m_BlendState0.Equals(other.m_BlendState0) && this.m_BlendState1.Equals(other.m_BlendState1) && this.m_BlendState2.Equals(other.m_BlendState2) && this.m_BlendState3.Equals(other.m_BlendState3) && this.m_BlendState4.Equals(other.m_BlendState4) && this.m_BlendState5.Equals(other.m_BlendState5) && this.m_BlendState6.Equals(other.m_BlendState6) && this.m_BlendState7.Equals(other.m_BlendState7) && this.m_SeparateMRTBlendStates == other.m_SeparateMRTBlendStates && this.m_AlphaToMask == other.m_AlphaToMask;
		}

		// Token: 0x0600222A RID: 8746 RVA: 0x000387F8 File Offset: 0x000369F8
		public override bool Equals(object obj)
		{
			bool flag = obj == null;
			return !flag && obj is BlendState && this.Equals((BlendState)obj);
		}

		// Token: 0x0600222B RID: 8747 RVA: 0x00038830 File Offset: 0x00036A30
		public override int GetHashCode()
		{
			int num = this.m_BlendState0.GetHashCode();
			num = (num * 397) ^ this.m_BlendState1.GetHashCode();
			num = (num * 397) ^ this.m_BlendState2.GetHashCode();
			num = (num * 397) ^ this.m_BlendState3.GetHashCode();
			num = (num * 397) ^ this.m_BlendState4.GetHashCode();
			num = (num * 397) ^ this.m_BlendState5.GetHashCode();
			num = (num * 397) ^ this.m_BlendState6.GetHashCode();
			num = (num * 397) ^ this.m_BlendState7.GetHashCode();
			num = (num * 397) ^ this.m_SeparateMRTBlendStates.GetHashCode();
			return (num * 397) ^ this.m_AlphaToMask.GetHashCode();
		}

		// Token: 0x0600222C RID: 8748 RVA: 0x00038934 File Offset: 0x00036B34
		public static bool operator ==(BlendState left, BlendState right)
		{
			return left.Equals(right);
		}

		// Token: 0x0600222D RID: 8749 RVA: 0x00038950 File Offset: 0x00036B50
		public static bool operator !=(BlendState left, BlendState right)
		{
			return !left.Equals(right);
		}

		// Token: 0x04000C6B RID: 3179
		private RenderTargetBlendState m_BlendState0;

		// Token: 0x04000C6C RID: 3180
		private RenderTargetBlendState m_BlendState1;

		// Token: 0x04000C6D RID: 3181
		private RenderTargetBlendState m_BlendState2;

		// Token: 0x04000C6E RID: 3182
		private RenderTargetBlendState m_BlendState3;

		// Token: 0x04000C6F RID: 3183
		private RenderTargetBlendState m_BlendState4;

		// Token: 0x04000C70 RID: 3184
		private RenderTargetBlendState m_BlendState5;

		// Token: 0x04000C71 RID: 3185
		private RenderTargetBlendState m_BlendState6;

		// Token: 0x04000C72 RID: 3186
		private RenderTargetBlendState m_BlendState7;

		// Token: 0x04000C73 RID: 3187
		private byte m_SeparateMRTBlendStates;

		// Token: 0x04000C74 RID: 3188
		private byte m_AlphaToMask;

		// Token: 0x04000C75 RID: 3189
		private short m_Padding;
	}
}
