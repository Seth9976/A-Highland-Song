using System;
using System.Globalization;

namespace UnityEngine.UIElements
{
	// Token: 0x02000291 RID: 657
	public struct TransformOrigin : IEquatable<TransformOrigin>
	{
		// Token: 0x0600158B RID: 5515 RVA: 0x0005BD74 File Offset: 0x00059F74
		public TransformOrigin(Length x, Length y, float z)
		{
			this.m_X = x;
			this.m_Y = y;
			this.m_Z = z;
		}

		// Token: 0x0600158C RID: 5516 RVA: 0x0005BD8C File Offset: 0x00059F8C
		public TransformOrigin(Length x, Length y)
		{
			this = new TransformOrigin(x, y, 0f);
		}

		// Token: 0x0600158D RID: 5517 RVA: 0x0005BDA0 File Offset: 0x00059FA0
		public static TransformOrigin Initial()
		{
			return new TransformOrigin(Length.Percent(50f), Length.Percent(50f), 0f);
		}

		// Token: 0x170004EF RID: 1263
		// (get) Token: 0x0600158E RID: 5518 RVA: 0x0005BDD0 File Offset: 0x00059FD0
		// (set) Token: 0x0600158F RID: 5519 RVA: 0x0005BDD8 File Offset: 0x00059FD8
		public Length x
		{
			get
			{
				return this.m_X;
			}
			set
			{
				this.m_X = value;
			}
		}

		// Token: 0x170004F0 RID: 1264
		// (get) Token: 0x06001590 RID: 5520 RVA: 0x0005BDE1 File Offset: 0x00059FE1
		// (set) Token: 0x06001591 RID: 5521 RVA: 0x0005BDE9 File Offset: 0x00059FE9
		public Length y
		{
			get
			{
				return this.m_Y;
			}
			set
			{
				this.m_Y = value;
			}
		}

		// Token: 0x170004F1 RID: 1265
		// (get) Token: 0x06001592 RID: 5522 RVA: 0x0005BDF2 File Offset: 0x00059FF2
		// (set) Token: 0x06001593 RID: 5523 RVA: 0x0005BDFA File Offset: 0x00059FFA
		public float z
		{
			get
			{
				return this.m_Z;
			}
			set
			{
				this.m_Z = value;
			}
		}

		// Token: 0x06001594 RID: 5524 RVA: 0x0005BE04 File Offset: 0x0005A004
		public static bool operator ==(TransformOrigin lhs, TransformOrigin rhs)
		{
			return lhs.m_X == rhs.m_X && lhs.m_Y == rhs.m_Y && lhs.m_Z == rhs.m_Z;
		}

		// Token: 0x06001595 RID: 5525 RVA: 0x0005BE50 File Offset: 0x0005A050
		public static bool operator !=(TransformOrigin lhs, TransformOrigin rhs)
		{
			return !(lhs == rhs);
		}

		// Token: 0x06001596 RID: 5526 RVA: 0x0005BE6C File Offset: 0x0005A06C
		public bool Equals(TransformOrigin other)
		{
			return other == this;
		}

		// Token: 0x06001597 RID: 5527 RVA: 0x0005BE8C File Offset: 0x0005A08C
		public override bool Equals(object obj)
		{
			bool flag;
			if (obj is TransformOrigin)
			{
				TransformOrigin transformOrigin = (TransformOrigin)obj;
				flag = this.Equals(transformOrigin);
			}
			else
			{
				flag = false;
			}
			return flag;
		}

		// Token: 0x06001598 RID: 5528 RVA: 0x0005BEB8 File Offset: 0x0005A0B8
		public override int GetHashCode()
		{
			return (this.m_X.GetHashCode() * 793) ^ (this.m_Y.GetHashCode() * 791) ^ (this.m_Z.GetHashCode() * 571);
		}

		// Token: 0x06001599 RID: 5529 RVA: 0x0005BF0C File Offset: 0x0005A10C
		public override string ToString()
		{
			string text = this.m_Z.ToString(CultureInfo.InvariantCulture.NumberFormat);
			return string.Concat(new string[]
			{
				this.m_X.ToString(),
				" ",
				this.m_Y.ToString(),
				" ",
				text
			});
		}

		// Token: 0x04000929 RID: 2345
		private Length m_X;

		// Token: 0x0400092A RID: 2346
		private Length m_Y;

		// Token: 0x0400092B RID: 2347
		private float m_Z;
	}
}
