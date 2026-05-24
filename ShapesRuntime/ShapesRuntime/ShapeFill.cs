using System;
using UnityEngine;

namespace Shapes
{
	// Token: 0x02000055 RID: 85
	[Serializable]
	public class ShapeFill
	{
		// Token: 0x06000A16 RID: 2582 RVA: 0x000241AE File Offset: 0x000223AE
		public static ShapeFill CreateLinear(Vector3 start, Vector3 end, Color colorStart, Color colorEnd, FillSpace space)
		{
			return new ShapeFill
			{
				type = FillType.LinearGradient,
				linearStart = start,
				linearEnd = end,
				colorStart = colorStart,
				colorEnd = colorEnd,
				space = space
			};
		}

		// Token: 0x06000A17 RID: 2583 RVA: 0x000241E0 File Offset: 0x000223E0
		public static ShapeFill CreateRadial(Vector3 origin, float radius, Color colorInner, Color colorOuter, FillSpace space)
		{
			return new ShapeFill
			{
				type = FillType.RadialGradient,
				radialOrigin = origin,
				radialRadius = radius,
				colorStart = colorInner,
				colorEnd = colorOuter,
				space = space
			};
		}

		// Token: 0x06000A18 RID: 2584 RVA: 0x00024214 File Offset: 0x00022414
		internal Vector4 GetShaderStartVector()
		{
			if (this.type == FillType.LinearGradient)
			{
				return this.linearStart;
			}
			return new Vector4(this.radialOrigin.x, this.radialOrigin.y, this.radialOrigin.z, this.radialRadius);
		}

		// Token: 0x040001A8 RID: 424
		public const int FILL_NONE = -1;

		// Token: 0x040001A9 RID: 425
		public FillType type;

		// Token: 0x040001AA RID: 426
		public FillSpace space;

		// Token: 0x040001AB RID: 427
		public Color colorStart = Color.black;

		// Token: 0x040001AC RID: 428
		public Color colorEnd = Color.white;

		// Token: 0x040001AD RID: 429
		public Vector3 linearStart = Vector3.zero;

		// Token: 0x040001AE RID: 430
		public Vector3 linearEnd = Vector3.up;

		// Token: 0x040001AF RID: 431
		public Vector3 radialOrigin = Vector3.zero;

		// Token: 0x040001B0 RID: 432
		public float radialRadius = 1f;
	}
}
