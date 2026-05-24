using System;
using System.Globalization;

namespace UnityEngine.UIElements
{
	// Token: 0x02000276 RID: 630
	public struct Length : IEquatable<Length>
	{
		// Token: 0x0600142B RID: 5163 RVA: 0x000570B8 File Offset: 0x000552B8
		public static Length Percent(float value)
		{
			return new Length(value, LengthUnit.Percent);
		}

		// Token: 0x0600142C RID: 5164 RVA: 0x000570D4 File Offset: 0x000552D4
		internal static Length Auto()
		{
			return new Length(0f, Length.Unit.Auto);
		}

		// Token: 0x0600142D RID: 5165 RVA: 0x000570F4 File Offset: 0x000552F4
		internal static Length None()
		{
			return new Length(0f, Length.Unit.None);
		}

		// Token: 0x170004C4 RID: 1220
		// (get) Token: 0x0600142E RID: 5166 RVA: 0x00057111 File Offset: 0x00055311
		// (set) Token: 0x0600142F RID: 5167 RVA: 0x00057119 File Offset: 0x00055319
		public float value
		{
			get
			{
				return this.m_Value;
			}
			set
			{
				this.m_Value = Mathf.Clamp(value, -8388608f, 8388608f);
			}
		}

		// Token: 0x170004C5 RID: 1221
		// (get) Token: 0x06001430 RID: 5168 RVA: 0x00057131 File Offset: 0x00055331
		// (set) Token: 0x06001431 RID: 5169 RVA: 0x00057139 File Offset: 0x00055339
		public LengthUnit unit
		{
			get
			{
				return (LengthUnit)this.m_Unit;
			}
			set
			{
				this.m_Unit = (Length.Unit)value;
			}
		}

		// Token: 0x06001432 RID: 5170 RVA: 0x00057142 File Offset: 0x00055342
		internal bool IsAuto()
		{
			return this.m_Unit == Length.Unit.Auto;
		}

		// Token: 0x06001433 RID: 5171 RVA: 0x0005714D File Offset: 0x0005534D
		internal bool IsNone()
		{
			return this.m_Unit == Length.Unit.None;
		}

		// Token: 0x06001434 RID: 5172 RVA: 0x00057158 File Offset: 0x00055358
		public Length(float value)
		{
			this = new Length(value, Length.Unit.Pixel);
		}

		// Token: 0x06001435 RID: 5173 RVA: 0x00057164 File Offset: 0x00055364
		public Length(float value, LengthUnit unit)
		{
			this = new Length(value, (Length.Unit)unit);
		}

		// Token: 0x06001436 RID: 5174 RVA: 0x00057170 File Offset: 0x00055370
		private Length(float value, Length.Unit unit)
		{
			this = default(Length);
			this.value = value;
			this.m_Unit = unit;
		}

		// Token: 0x06001437 RID: 5175 RVA: 0x0005718C File Offset: 0x0005538C
		public static implicit operator Length(float value)
		{
			return new Length(value, LengthUnit.Pixel);
		}

		// Token: 0x06001438 RID: 5176 RVA: 0x000571A8 File Offset: 0x000553A8
		public static bool operator ==(Length lhs, Length rhs)
		{
			return lhs.m_Value == rhs.m_Value && lhs.m_Unit == rhs.m_Unit;
		}

		// Token: 0x06001439 RID: 5177 RVA: 0x000571DC File Offset: 0x000553DC
		public static bool operator !=(Length lhs, Length rhs)
		{
			return !(lhs == rhs);
		}

		// Token: 0x0600143A RID: 5178 RVA: 0x000571F8 File Offset: 0x000553F8
		public bool Equals(Length other)
		{
			return other == this;
		}

		// Token: 0x0600143B RID: 5179 RVA: 0x00057218 File Offset: 0x00055418
		public override bool Equals(object obj)
		{
			bool flag;
			if (obj is Length)
			{
				Length length = (Length)obj;
				flag = this.Equals(length);
			}
			else
			{
				flag = false;
			}
			return flag;
		}

		// Token: 0x0600143C RID: 5180 RVA: 0x00057244 File Offset: 0x00055444
		public override int GetHashCode()
		{
			return (this.m_Value.GetHashCode() * 397) ^ (int)this.m_Unit;
		}

		// Token: 0x0600143D RID: 5181 RVA: 0x00057270 File Offset: 0x00055470
		public override string ToString()
		{
			string text = this.value.ToString(CultureInfo.InvariantCulture.NumberFormat);
			string text2 = string.Empty;
			switch (this.m_Unit)
			{
			case Length.Unit.Pixel:
			{
				bool flag = !Mathf.Approximately(0f, this.value);
				if (flag)
				{
					text2 = "px";
				}
				break;
			}
			case Length.Unit.Percent:
				text2 = "%";
				break;
			case Length.Unit.Auto:
				text = "auto";
				break;
			case Length.Unit.None:
				text = "none";
				break;
			}
			return text + text2;
		}

		// Token: 0x040008EA RID: 2282
		private const float k_MaxValue = 8388608f;

		// Token: 0x040008EB RID: 2283
		private float m_Value;

		// Token: 0x040008EC RID: 2284
		private Length.Unit m_Unit;

		// Token: 0x02000277 RID: 631
		private enum Unit
		{
			// Token: 0x040008EE RID: 2286
			Pixel,
			// Token: 0x040008EF RID: 2287
			Percent,
			// Token: 0x040008F0 RID: 2288
			Auto,
			// Token: 0x040008F1 RID: 2289
			None
		}
	}
}
