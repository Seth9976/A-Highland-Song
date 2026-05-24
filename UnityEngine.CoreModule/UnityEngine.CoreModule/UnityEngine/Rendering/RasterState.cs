using System;

namespace UnityEngine.Rendering
{
	// Token: 0x02000401 RID: 1025
	public struct RasterState : IEquatable<RasterState>
	{
		// Token: 0x060022D8 RID: 8920 RVA: 0x0003AA7D File Offset: 0x00038C7D
		public RasterState(CullMode cullingMode = CullMode.Back, int offsetUnits = 0, float offsetFactor = 0f, bool depthClip = true)
		{
			this.m_CullingMode = cullingMode;
			this.m_OffsetUnits = offsetUnits;
			this.m_OffsetFactor = offsetFactor;
			this.m_DepthClip = Convert.ToByte(depthClip);
			this.m_Conservative = Convert.ToByte(false);
			this.m_Padding1 = 0;
			this.m_Padding2 = 0;
		}

		// Token: 0x1700066D RID: 1645
		// (get) Token: 0x060022D9 RID: 8921 RVA: 0x0003AABC File Offset: 0x00038CBC
		// (set) Token: 0x060022DA RID: 8922 RVA: 0x0003AAD4 File Offset: 0x00038CD4
		public CullMode cullingMode
		{
			get
			{
				return this.m_CullingMode;
			}
			set
			{
				this.m_CullingMode = value;
			}
		}

		// Token: 0x1700066E RID: 1646
		// (get) Token: 0x060022DB RID: 8923 RVA: 0x0003AAE0 File Offset: 0x00038CE0
		// (set) Token: 0x060022DC RID: 8924 RVA: 0x0003AAFD File Offset: 0x00038CFD
		public bool depthClip
		{
			get
			{
				return Convert.ToBoolean(this.m_DepthClip);
			}
			set
			{
				this.m_DepthClip = Convert.ToByte(value);
			}
		}

		// Token: 0x1700066F RID: 1647
		// (get) Token: 0x060022DD RID: 8925 RVA: 0x0003AB0C File Offset: 0x00038D0C
		// (set) Token: 0x060022DE RID: 8926 RVA: 0x0003AB29 File Offset: 0x00038D29
		public bool conservative
		{
			get
			{
				return Convert.ToBoolean(this.m_Conservative);
			}
			set
			{
				this.m_Conservative = Convert.ToByte(value);
			}
		}

		// Token: 0x17000670 RID: 1648
		// (get) Token: 0x060022DF RID: 8927 RVA: 0x0003AB38 File Offset: 0x00038D38
		// (set) Token: 0x060022E0 RID: 8928 RVA: 0x0003AB50 File Offset: 0x00038D50
		public int offsetUnits
		{
			get
			{
				return this.m_OffsetUnits;
			}
			set
			{
				this.m_OffsetUnits = value;
			}
		}

		// Token: 0x17000671 RID: 1649
		// (get) Token: 0x060022E1 RID: 8929 RVA: 0x0003AB5C File Offset: 0x00038D5C
		// (set) Token: 0x060022E2 RID: 8930 RVA: 0x0003AB74 File Offset: 0x00038D74
		public float offsetFactor
		{
			get
			{
				return this.m_OffsetFactor;
			}
			set
			{
				this.m_OffsetFactor = value;
			}
		}

		// Token: 0x060022E3 RID: 8931 RVA: 0x0003AB80 File Offset: 0x00038D80
		public bool Equals(RasterState other)
		{
			return this.m_CullingMode == other.m_CullingMode && this.m_OffsetUnits == other.m_OffsetUnits && this.m_OffsetFactor.Equals(other.m_OffsetFactor) && this.m_DepthClip == other.m_DepthClip && this.m_Conservative == other.m_Conservative;
		}

		// Token: 0x060022E4 RID: 8932 RVA: 0x0003ABE0 File Offset: 0x00038DE0
		public override bool Equals(object obj)
		{
			bool flag = obj == null;
			return !flag && obj is RasterState && this.Equals((RasterState)obj);
		}

		// Token: 0x060022E5 RID: 8933 RVA: 0x0003AC18 File Offset: 0x00038E18
		public override int GetHashCode()
		{
			int num = (int)this.m_CullingMode;
			num = (num * 397) ^ this.m_OffsetUnits;
			num = (num * 397) ^ this.m_OffsetFactor.GetHashCode();
			num = (num * 397) ^ this.m_DepthClip.GetHashCode();
			return (num * 397) ^ this.m_Conservative.GetHashCode();
		}

		// Token: 0x060022E6 RID: 8934 RVA: 0x0003AC80 File Offset: 0x00038E80
		public static bool operator ==(RasterState left, RasterState right)
		{
			return left.Equals(right);
		}

		// Token: 0x060022E7 RID: 8935 RVA: 0x0003AC9C File Offset: 0x00038E9C
		public static bool operator !=(RasterState left, RasterState right)
		{
			return !left.Equals(right);
		}

		// Token: 0x04000CFA RID: 3322
		public static readonly RasterState defaultValue = new RasterState(CullMode.Back, 0, 0f, true);

		// Token: 0x04000CFB RID: 3323
		private CullMode m_CullingMode;

		// Token: 0x04000CFC RID: 3324
		private int m_OffsetUnits;

		// Token: 0x04000CFD RID: 3325
		private float m_OffsetFactor;

		// Token: 0x04000CFE RID: 3326
		private byte m_DepthClip;

		// Token: 0x04000CFF RID: 3327
		private byte m_Conservative;

		// Token: 0x04000D00 RID: 3328
		private byte m_Padding1;

		// Token: 0x04000D01 RID: 3329
		private byte m_Padding2;
	}
}
