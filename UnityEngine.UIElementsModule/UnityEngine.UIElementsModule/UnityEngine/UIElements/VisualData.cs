using System;

namespace UnityEngine.UIElements
{
	// Token: 0x0200029B RID: 667
	internal struct VisualData : IStyleDataGroup<VisualData>, IEquatable<VisualData>
	{
		// Token: 0x060016B2 RID: 5810 RVA: 0x0005D088 File Offset: 0x0005B288
		public VisualData Copy()
		{
			return this;
		}

		// Token: 0x060016B3 RID: 5811 RVA: 0x0005D0A0 File Offset: 0x0005B2A0
		public void CopyFrom(ref VisualData other)
		{
			this = other;
		}

		// Token: 0x060016B4 RID: 5812 RVA: 0x0005D0B0 File Offset: 0x0005B2B0
		public static bool operator ==(VisualData lhs, VisualData rhs)
		{
			return lhs.backgroundColor == rhs.backgroundColor && lhs.backgroundImage == rhs.backgroundImage && lhs.borderBottomColor == rhs.borderBottomColor && lhs.borderBottomLeftRadius == rhs.borderBottomLeftRadius && lhs.borderBottomRightRadius == rhs.borderBottomRightRadius && lhs.borderLeftColor == rhs.borderLeftColor && lhs.borderRightColor == rhs.borderRightColor && lhs.borderTopColor == rhs.borderTopColor && lhs.borderTopLeftRadius == rhs.borderTopLeftRadius && lhs.borderTopRightRadius == rhs.borderTopRightRadius && lhs.opacity == rhs.opacity && lhs.overflow == rhs.overflow;
		}

		// Token: 0x060016B5 RID: 5813 RVA: 0x0005D1AC File Offset: 0x0005B3AC
		public static bool operator !=(VisualData lhs, VisualData rhs)
		{
			return !(lhs == rhs);
		}

		// Token: 0x060016B6 RID: 5814 RVA: 0x0005D1C8 File Offset: 0x0005B3C8
		public bool Equals(VisualData other)
		{
			return other == this;
		}

		// Token: 0x060016B7 RID: 5815 RVA: 0x0005D1E8 File Offset: 0x0005B3E8
		public override bool Equals(object obj)
		{
			bool flag = obj == null;
			return !flag && obj is VisualData && this.Equals((VisualData)obj);
		}

		// Token: 0x060016B8 RID: 5816 RVA: 0x0005D220 File Offset: 0x0005B420
		public override int GetHashCode()
		{
			int num = this.backgroundColor.GetHashCode();
			num = (num * 397) ^ this.backgroundImage.GetHashCode();
			num = (num * 397) ^ this.borderBottomColor.GetHashCode();
			num = (num * 397) ^ this.borderBottomLeftRadius.GetHashCode();
			num = (num * 397) ^ this.borderBottomRightRadius.GetHashCode();
			num = (num * 397) ^ this.borderLeftColor.GetHashCode();
			num = (num * 397) ^ this.borderRightColor.GetHashCode();
			num = (num * 397) ^ this.borderTopColor.GetHashCode();
			num = (num * 397) ^ this.borderTopLeftRadius.GetHashCode();
			num = (num * 397) ^ this.borderTopRightRadius.GetHashCode();
			num = (num * 397) ^ this.opacity.GetHashCode();
			return (num * 397) ^ (int)this.overflow;
		}

		// Token: 0x04000971 RID: 2417
		public Color backgroundColor;

		// Token: 0x04000972 RID: 2418
		public Background backgroundImage;

		// Token: 0x04000973 RID: 2419
		public Color borderBottomColor;

		// Token: 0x04000974 RID: 2420
		public Length borderBottomLeftRadius;

		// Token: 0x04000975 RID: 2421
		public Length borderBottomRightRadius;

		// Token: 0x04000976 RID: 2422
		public Color borderLeftColor;

		// Token: 0x04000977 RID: 2423
		public Color borderRightColor;

		// Token: 0x04000978 RID: 2424
		public Color borderTopColor;

		// Token: 0x04000979 RID: 2425
		public Length borderTopLeftRadius;

		// Token: 0x0400097A RID: 2426
		public Length borderTopRightRadius;

		// Token: 0x0400097B RID: 2427
		public float opacity;

		// Token: 0x0400097C RID: 2428
		public OverflowInternal overflow;
	}
}
