using System;

namespace UnityEngine.UIElements
{
	// Token: 0x02000298 RID: 664
	internal struct RareData : IStyleDataGroup<RareData>, IEquatable<RareData>
	{
		// Token: 0x0600169D RID: 5789 RVA: 0x0005CAAC File Offset: 0x0005ACAC
		public RareData Copy()
		{
			return this;
		}

		// Token: 0x0600169E RID: 5790 RVA: 0x0005CAC4 File Offset: 0x0005ACC4
		public void CopyFrom(ref RareData other)
		{
			this = other;
		}

		// Token: 0x0600169F RID: 5791 RVA: 0x0005CAD4 File Offset: 0x0005ACD4
		public static bool operator ==(RareData lhs, RareData rhs)
		{
			return lhs.cursor == rhs.cursor && lhs.textOverflow == rhs.textOverflow && lhs.unityBackgroundImageTintColor == rhs.unityBackgroundImageTintColor && lhs.unityBackgroundScaleMode == rhs.unityBackgroundScaleMode && lhs.unityOverflowClipBox == rhs.unityOverflowClipBox && lhs.unitySliceBottom == rhs.unitySliceBottom && lhs.unitySliceLeft == rhs.unitySliceLeft && lhs.unitySliceRight == rhs.unitySliceRight && lhs.unitySliceTop == rhs.unitySliceTop && lhs.unityTextOverflowPosition == rhs.unityTextOverflowPosition;
		}

		// Token: 0x060016A0 RID: 5792 RVA: 0x0005CB84 File Offset: 0x0005AD84
		public static bool operator !=(RareData lhs, RareData rhs)
		{
			return !(lhs == rhs);
		}

		// Token: 0x060016A1 RID: 5793 RVA: 0x0005CBA0 File Offset: 0x0005ADA0
		public bool Equals(RareData other)
		{
			return other == this;
		}

		// Token: 0x060016A2 RID: 5794 RVA: 0x0005CBC0 File Offset: 0x0005ADC0
		public override bool Equals(object obj)
		{
			bool flag = obj == null;
			return !flag && obj is RareData && this.Equals((RareData)obj);
		}

		// Token: 0x060016A3 RID: 5795 RVA: 0x0005CBF8 File Offset: 0x0005ADF8
		public override int GetHashCode()
		{
			int num = this.cursor.GetHashCode();
			num = (num * 397) ^ (int)this.textOverflow;
			num = (num * 397) ^ this.unityBackgroundImageTintColor.GetHashCode();
			num = (num * 397) ^ (int)this.unityBackgroundScaleMode;
			num = (num * 397) ^ (int)this.unityOverflowClipBox;
			num = (num * 397) ^ this.unitySliceBottom;
			num = (num * 397) ^ this.unitySliceLeft;
			num = (num * 397) ^ this.unitySliceRight;
			num = (num * 397) ^ this.unitySliceTop;
			return (num * 397) ^ (int)this.unityTextOverflowPosition;
		}

		// Token: 0x0400095F RID: 2399
		public Cursor cursor;

		// Token: 0x04000960 RID: 2400
		public TextOverflow textOverflow;

		// Token: 0x04000961 RID: 2401
		public Color unityBackgroundImageTintColor;

		// Token: 0x04000962 RID: 2402
		public ScaleMode unityBackgroundScaleMode;

		// Token: 0x04000963 RID: 2403
		public OverflowClipBox unityOverflowClipBox;

		// Token: 0x04000964 RID: 2404
		public int unitySliceBottom;

		// Token: 0x04000965 RID: 2405
		public int unitySliceLeft;

		// Token: 0x04000966 RID: 2406
		public int unitySliceRight;

		// Token: 0x04000967 RID: 2407
		public int unitySliceTop;

		// Token: 0x04000968 RID: 2408
		public TextOverflowPosition unityTextOverflowPosition;
	}
}
