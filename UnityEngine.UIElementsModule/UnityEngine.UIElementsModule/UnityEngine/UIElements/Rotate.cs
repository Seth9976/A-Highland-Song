using System;

namespace UnityEngine.UIElements
{
	// Token: 0x02000278 RID: 632
	public struct Rotate : IEquatable<Rotate>
	{
		// Token: 0x0600143E RID: 5182 RVA: 0x00057306 File Offset: 0x00055506
		internal Rotate(Angle angle, Vector3 axis)
		{
			this.m_Angle = angle;
			this.m_Axis = axis;
			this.m_IsNone = false;
		}

		// Token: 0x0600143F RID: 5183 RVA: 0x0005731E File Offset: 0x0005551E
		public Rotate(Angle angle)
		{
			this.m_Angle = angle;
			this.m_Axis = Vector3.forward;
			this.m_IsNone = false;
		}

		// Token: 0x06001440 RID: 5184 RVA: 0x0005733C File Offset: 0x0005553C
		internal static Rotate Initial()
		{
			return new Rotate(0f);
		}

		// Token: 0x06001441 RID: 5185 RVA: 0x00057360 File Offset: 0x00055560
		public static Rotate None()
		{
			Rotate rotate = Rotate.Initial();
			rotate.m_IsNone = true;
			return rotate;
		}

		// Token: 0x170004C6 RID: 1222
		// (get) Token: 0x06001442 RID: 5186 RVA: 0x00057381 File Offset: 0x00055581
		// (set) Token: 0x06001443 RID: 5187 RVA: 0x00057389 File Offset: 0x00055589
		public Angle angle
		{
			get
			{
				return this.m_Angle;
			}
			set
			{
				this.m_Angle = value;
			}
		}

		// Token: 0x170004C7 RID: 1223
		// (get) Token: 0x06001444 RID: 5188 RVA: 0x00057392 File Offset: 0x00055592
		// (set) Token: 0x06001445 RID: 5189 RVA: 0x0005739A File Offset: 0x0005559A
		internal Vector3 axis
		{
			get
			{
				return this.m_Axis;
			}
			set
			{
				this.m_Axis = value;
			}
		}

		// Token: 0x06001446 RID: 5190 RVA: 0x000573A3 File Offset: 0x000555A3
		internal bool IsNone()
		{
			return this.m_IsNone;
		}

		// Token: 0x06001447 RID: 5191 RVA: 0x000573AC File Offset: 0x000555AC
		public static bool operator ==(Rotate lhs, Rotate rhs)
		{
			return lhs.m_Angle == rhs.m_Angle && lhs.m_Axis == rhs.m_Axis && lhs.m_IsNone == rhs.m_IsNone;
		}

		// Token: 0x06001448 RID: 5192 RVA: 0x000573F8 File Offset: 0x000555F8
		public static bool operator !=(Rotate lhs, Rotate rhs)
		{
			return !(lhs == rhs);
		}

		// Token: 0x06001449 RID: 5193 RVA: 0x00057414 File Offset: 0x00055614
		public bool Equals(Rotate other)
		{
			return other == this;
		}

		// Token: 0x0600144A RID: 5194 RVA: 0x00057434 File Offset: 0x00055634
		public override bool Equals(object obj)
		{
			bool flag;
			if (obj is Rotate)
			{
				Rotate rotate = (Rotate)obj;
				flag = this.Equals(rotate);
			}
			else
			{
				flag = false;
			}
			return flag;
		}

		// Token: 0x0600144B RID: 5195 RVA: 0x00057460 File Offset: 0x00055660
		public override int GetHashCode()
		{
			return (this.m_Angle.GetHashCode() * 793) ^ (this.m_Axis.GetHashCode() * 791) ^ (this.m_IsNone.GetHashCode() * 197);
		}

		// Token: 0x0600144C RID: 5196 RVA: 0x000574B4 File Offset: 0x000556B4
		public override string ToString()
		{
			return this.m_Angle.ToString() + " " + this.m_Axis.ToString();
		}

		// Token: 0x0600144D RID: 5197 RVA: 0x000574F4 File Offset: 0x000556F4
		internal Quaternion ToQuaternion()
		{
			return Quaternion.AngleAxis(this.m_Angle.ToDegrees(), this.m_Axis);
		}

		// Token: 0x040008F2 RID: 2290
		private Angle m_Angle;

		// Token: 0x040008F3 RID: 2291
		private Vector3 m_Axis;

		// Token: 0x040008F4 RID: 2292
		private bool m_IsNone;
	}
}
