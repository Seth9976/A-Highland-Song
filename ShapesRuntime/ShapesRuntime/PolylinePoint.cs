using System;
using UnityEngine;

namespace Shapes
{
	// Token: 0x0200004D RID: 77
	[Serializable]
	public struct PolylinePoint
	{
		// Token: 0x06000A06 RID: 2566 RVA: 0x00023D00 File Offset: 0x00021F00
		public static PolylinePoint Lerp(PolylinePoint a, PolylinePoint b, float t)
		{
			return new PolylinePoint
			{
				point = Vector3.LerpUnclamped(a.point, b.point, t),
				color = Color.LerpUnclamped(a.color, b.color, t),
				thickness = Mathf.LerpUnclamped(a.thickness, b.thickness, t)
			};
		}

		// Token: 0x06000A07 RID: 2567 RVA: 0x00023D61 File Offset: 0x00021F61
		public PolylinePoint(Vector3 point)
		{
			this.point = point;
			this.color = Color.white;
			this.thickness = 1f;
		}

		// Token: 0x06000A08 RID: 2568 RVA: 0x00023D80 File Offset: 0x00021F80
		public PolylinePoint(Vector2 point)
		{
			this.point = point;
			this.color = Color.white;
			this.thickness = 1f;
		}

		// Token: 0x06000A09 RID: 2569 RVA: 0x00023DA4 File Offset: 0x00021FA4
		public PolylinePoint(Vector3 point, Color color)
		{
			this.point = point;
			this.color = color;
			this.thickness = 1f;
		}

		// Token: 0x06000A0A RID: 2570 RVA: 0x00023DBF File Offset: 0x00021FBF
		public PolylinePoint(Vector2 point, Color color)
		{
			this.point = point;
			this.color = color;
			this.thickness = 1f;
		}

		// Token: 0x06000A0B RID: 2571 RVA: 0x00023DDF File Offset: 0x00021FDF
		public PolylinePoint(Vector3 point, Color color, float thickness)
		{
			this.point = point;
			this.color = color;
			this.thickness = thickness;
		}

		// Token: 0x06000A0C RID: 2572 RVA: 0x00023DF6 File Offset: 0x00021FF6
		public PolylinePoint(Vector2 point, Color color, float thickness)
		{
			this.point = point;
			this.color = color;
			this.thickness = thickness;
		}

		// Token: 0x0400018E RID: 398
		public Vector3 point;

		// Token: 0x0400018F RID: 399
		[ShapesColorField(true)]
		public Color color;

		// Token: 0x04000190 RID: 400
		public float thickness;
	}
}
