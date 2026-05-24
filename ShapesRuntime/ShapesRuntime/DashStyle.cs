using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Shapes
{
	// Token: 0x02000039 RID: 57
	[Serializable]
	public class DashStyle
	{
		// Token: 0x1700012E RID: 302
		// (get) Token: 0x060009E6 RID: 2534 RVA: 0x00023943 File Offset: 0x00021B43
		public static DashStyle DefaultDashStyleRing
		{
			get
			{
				return new DashStyle(16f)
				{
					spacing = 0.5f,
					snap = DashSnapping.Tiling,
					space = DashSpace.FixedCount
				};
			}
		}

		// Token: 0x1700012F RID: 303
		// (get) Token: 0x060009E7 RID: 2535 RVA: 0x00023969 File Offset: 0x00021B69
		public static DashStyle DefaultDashStyleLine
		{
			get
			{
				return new DashStyle(4f);
			}
		}

		// Token: 0x060009E8 RID: 2536 RVA: 0x00023975 File Offset: 0x00021B75
		private float GetNet(float v, float thickness)
		{
			if (this.space != DashSpace.Relative)
			{
				return v;
			}
			return thickness * v;
		}

		// Token: 0x060009E9 RID: 2537 RVA: 0x00023985 File Offset: 0x00021B85
		public float GetNetAbsoluteSize(bool dashed, float thickness)
		{
			if (!dashed)
			{
				return 0f;
			}
			return this.GetNet(this.size, thickness);
		}

		// Token: 0x060009EA RID: 2538 RVA: 0x0002399D File Offset: 0x00021B9D
		public float GetNetAbsoluteSpacing(bool dashed, float thickness)
		{
			if (!dashed)
			{
				return 0f;
			}
			return this.GetNet(this.spacing, thickness);
		}

		// Token: 0x17000130 RID: 304
		// (get) Token: 0x060009EB RID: 2539 RVA: 0x000239B5 File Offset: 0x00021BB5
		// (set) Token: 0x060009EC RID: 2540 RVA: 0x000239BD File Offset: 0x00021BBD
		public float UniformSize
		{
			get
			{
				return this.size;
			}
			set
			{
				this.size = value;
				if (this.space == DashSpace.FixedCount)
				{
					this.spacing = 0.5f;
					return;
				}
				this.spacing = this.size;
			}
		}

		// Token: 0x060009ED RID: 2541 RVA: 0x000239E8 File Offset: 0x00021BE8
		public DashStyle()
		{
		}

		// Token: 0x060009EE RID: 2542 RVA: 0x00023A18 File Offset: 0x00021C18
		public DashStyle(float size)
		{
			this.size = size;
			this.spacing = size;
		}

		// Token: 0x060009EF RID: 2543 RVA: 0x00023A58 File Offset: 0x00021C58
		public DashStyle(float size, DashType type)
		{
			this.size = size;
			this.spacing = size;
			this.type = type;
		}

		// Token: 0x060009F0 RID: 2544 RVA: 0x00023AA8 File Offset: 0x00021CA8
		public DashStyle(float size, float spacing, DashType type)
		{
			this.size = size;
			this.spacing = spacing;
			this.type = type;
		}

		// Token: 0x060009F1 RID: 2545 RVA: 0x00023AF8 File Offset: 0x00021CF8
		public DashStyle(float size, float spacing)
		{
			this.size = size;
			this.spacing = spacing;
		}

		// Token: 0x060009F2 RID: 2546 RVA: 0x00023B38 File Offset: 0x00021D38
		public DashStyle(float size, float spacing, float offset)
		{
			this.size = size;
			this.spacing = spacing;
			this.offset = offset;
		}

		// Token: 0x060009F3 RID: 2547 RVA: 0x00023B88 File Offset: 0x00021D88
		public static implicit operator DashStyle(float dashSize)
		{
			return new DashStyle(dashSize);
		}

		// Token: 0x060009F4 RID: 2548 RVA: 0x00023B90 File Offset: 0x00021D90
		public static implicit operator DashStyle(int dashSize)
		{
			return new DashStyle((float)dashSize);
		}

		// Token: 0x060009F5 RID: 2549 RVA: 0x00023B99 File Offset: 0x00021D99
		public static implicit operator DashStyle([TupleElementNames(new string[] { "size", "spacing" })] ValueTuple<float, float> t)
		{
			return new DashStyle(t.Item1, t.Item2);
		}

		// Token: 0x060009F6 RID: 2550 RVA: 0x00023BAC File Offset: 0x00021DAC
		public static implicit operator DashStyle([TupleElementNames(new string[] { "size", "spacing", "offset" })] ValueTuple<float, float, float> t)
		{
			return new DashStyle(t.Item1, t.Item2, t.Item3);
		}

		// Token: 0x04000155 RID: 341
		public DashType type;

		// Token: 0x04000156 RID: 342
		public DashSpace space = DashSpace.Relative;

		// Token: 0x04000157 RID: 343
		public DashSnapping snap;

		// Token: 0x04000158 RID: 344
		public float size = 1f;

		// Token: 0x04000159 RID: 345
		public float offset;

		// Token: 0x0400015A RID: 346
		public float spacing = 1f;

		// Token: 0x0400015B RID: 347
		[Range(-1f, 1f)]
		public float shapeModifier = 1f;
	}
}
