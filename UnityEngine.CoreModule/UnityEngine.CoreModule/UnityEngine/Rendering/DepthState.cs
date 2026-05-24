using System;

namespace UnityEngine.Rendering
{
	// Token: 0x020003F9 RID: 1017
	public struct DepthState : IEquatable<DepthState>
	{
		// Token: 0x17000657 RID: 1623
		// (get) Token: 0x06002294 RID: 8852 RVA: 0x00039FF8 File Offset: 0x000381F8
		public static DepthState defaultValue
		{
			get
			{
				return new DepthState(true, CompareFunction.Less);
			}
		}

		// Token: 0x06002295 RID: 8853 RVA: 0x0003A011 File Offset: 0x00038211
		public DepthState(bool writeEnabled = true, CompareFunction compareFunction = CompareFunction.Less)
		{
			this.m_WriteEnabled = Convert.ToByte(writeEnabled);
			this.m_CompareFunction = (sbyte)compareFunction;
		}

		// Token: 0x17000658 RID: 1624
		// (get) Token: 0x06002296 RID: 8854 RVA: 0x0003A028 File Offset: 0x00038228
		// (set) Token: 0x06002297 RID: 8855 RVA: 0x0003A045 File Offset: 0x00038245
		public bool writeEnabled
		{
			get
			{
				return Convert.ToBoolean(this.m_WriteEnabled);
			}
			set
			{
				this.m_WriteEnabled = Convert.ToByte(value);
			}
		}

		// Token: 0x17000659 RID: 1625
		// (get) Token: 0x06002298 RID: 8856 RVA: 0x0003A054 File Offset: 0x00038254
		// (set) Token: 0x06002299 RID: 8857 RVA: 0x0003A06C File Offset: 0x0003826C
		public CompareFunction compareFunction
		{
			get
			{
				return (CompareFunction)this.m_CompareFunction;
			}
			set
			{
				this.m_CompareFunction = (sbyte)value;
			}
		}

		// Token: 0x0600229A RID: 8858 RVA: 0x0003A078 File Offset: 0x00038278
		public bool Equals(DepthState other)
		{
			return this.m_WriteEnabled == other.m_WriteEnabled && this.m_CompareFunction == other.m_CompareFunction;
		}

		// Token: 0x0600229B RID: 8859 RVA: 0x0003A0AC File Offset: 0x000382AC
		public override bool Equals(object obj)
		{
			bool flag = obj == null;
			return !flag && obj is DepthState && this.Equals((DepthState)obj);
		}

		// Token: 0x0600229C RID: 8860 RVA: 0x0003A0E4 File Offset: 0x000382E4
		public override int GetHashCode()
		{
			return (this.m_WriteEnabled.GetHashCode() * 397) ^ this.m_CompareFunction.GetHashCode();
		}

		// Token: 0x0600229D RID: 8861 RVA: 0x0003A114 File Offset: 0x00038314
		public static bool operator ==(DepthState left, DepthState right)
		{
			return left.Equals(right);
		}

		// Token: 0x0600229E RID: 8862 RVA: 0x0003A130 File Offset: 0x00038330
		public static bool operator !=(DepthState left, DepthState right)
		{
			return !left.Equals(right);
		}

		// Token: 0x04000CCE RID: 3278
		private byte m_WriteEnabled;

		// Token: 0x04000CCF RID: 3279
		private sbyte m_CompareFunction;
	}
}
