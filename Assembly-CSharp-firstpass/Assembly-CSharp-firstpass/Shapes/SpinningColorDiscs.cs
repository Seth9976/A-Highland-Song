using System;
using UnityEngine;
using UnityEngine.Rendering;

namespace Shapes
{
	// Token: 0x0200000D RID: 13
	[ExecuteAlways]
	public class SpinningColorDiscs : ImmediateModeShapeDrawer
	{
		// Token: 0x0600004D RID: 77 RVA: 0x00003B98 File Offset: 0x00001D98
		public override void DrawShapes(Camera cam)
		{
			using (Draw.Command(cam, CameraEvent.BeforeImageEffects))
			{
				Draw.ResetAllDrawStates();
				Draw.Matrix = base.transform.localToWorldMatrix;
				for (int i = 0; i < this.discCount; i++)
				{
					float num = (float)i / (float)this.discCount;
					Color color = Color.HSVToRGB(num, 1f, 1f);
					Draw.Disc(this.GetDiscPosition(num), this.discRadius, color);
				}
			}
		}

		// Token: 0x0600004E RID: 78 RVA: 0x00003C24 File Offset: 0x00001E24
		private Vector2 GetDiscPosition(float t)
		{
			float num = t * 6.2831855f + 6.2831855f * Time.time * 0.25f;
			return ShapesMath.AngToDir(num + Mathf.Cos(num * 2f + Time.time * 6.2831855f * 0.5f) * 0.16f);
		}

		// Token: 0x0400006B RID: 107
		[Range(3f, 32f)]
		public int discCount = 24;

		// Token: 0x0400006C RID: 108
		[Range(0f, 1f)]
		public float discRadius = 0.1f;
	}
}
