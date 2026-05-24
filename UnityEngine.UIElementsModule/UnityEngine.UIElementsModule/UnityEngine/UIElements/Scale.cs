using System;

namespace UnityEngine.UIElements
{
	// Token: 0x02000279 RID: 633
	public struct Scale : IEquatable<Scale>
	{
		// Token: 0x0600144E RID: 5198 RVA: 0x0005751C File Offset: 0x0005571C
		public Scale(Vector3 scale)
		{
			this.m_Scale = new Vector3(scale.x, scale.y, 1f);
			this.m_IsNone = false;
		}

		// Token: 0x0600144F RID: 5199 RVA: 0x00057544 File Offset: 0x00055744
		internal static Scale Initial()
		{
			return new Scale(Vector3.one);
		}

		// Token: 0x06001450 RID: 5200 RVA: 0x00057560 File Offset: 0x00055760
		public static Scale None()
		{
			Scale scale = Scale.Initial();
			scale.m_IsNone = true;
			return scale;
		}

		// Token: 0x170004C8 RID: 1224
		// (get) Token: 0x06001451 RID: 5201 RVA: 0x00057581 File Offset: 0x00055781
		// (set) Token: 0x06001452 RID: 5202 RVA: 0x00057589 File Offset: 0x00055789
		public Vector3 value
		{
			get
			{
				return this.m_Scale;
			}
			set
			{
				this.m_Scale = new Vector3(value.x, value.y, 1f);
			}
		}

		// Token: 0x06001453 RID: 5203 RVA: 0x000575A7 File Offset: 0x000557A7
		internal bool IsNone()
		{
			return this.m_IsNone;
		}

		// Token: 0x06001454 RID: 5204 RVA: 0x000575B0 File Offset: 0x000557B0
		public static bool operator ==(Scale lhs, Scale rhs)
		{
			return lhs.m_Scale == rhs.m_Scale;
		}

		// Token: 0x06001455 RID: 5205 RVA: 0x000575D4 File Offset: 0x000557D4
		public static bool operator !=(Scale lhs, Scale rhs)
		{
			return !(lhs == rhs);
		}

		// Token: 0x06001456 RID: 5206 RVA: 0x000575F0 File Offset: 0x000557F0
		public bool Equals(Scale other)
		{
			return other == this;
		}

		// Token: 0x06001457 RID: 5207 RVA: 0x00057610 File Offset: 0x00055810
		public override bool Equals(object obj)
		{
			bool flag;
			if (obj is Scale)
			{
				Scale scale = (Scale)obj;
				flag = this.Equals(scale);
			}
			else
			{
				flag = false;
			}
			return flag;
		}

		// Token: 0x06001458 RID: 5208 RVA: 0x0005763C File Offset: 0x0005583C
		public override int GetHashCode()
		{
			return this.m_Scale.GetHashCode() * 793;
		}

		// Token: 0x06001459 RID: 5209 RVA: 0x00057668 File Offset: 0x00055868
		public override string ToString()
		{
			return this.m_Scale.ToString();
		}

		// Token: 0x040008F5 RID: 2293
		private Vector3 m_Scale;

		// Token: 0x040008F6 RID: 2294
		private bool m_IsNone;
	}
}
