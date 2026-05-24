using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Shapes
{
	// Token: 0x02000009 RID: 9
	public class Crosshair : MonoBehaviour
	{
		// Token: 0x06000036 RID: 54 RVA: 0x00003166 File Offset: 0x00001366
		public void Fire()
		{
			this.fireDecayer.SetT(1f);
		}

		// Token: 0x06000037 RID: 55 RVA: 0x00003178 File Offset: 0x00001378
		public void FireHit()
		{
			this.hitDecayer.SetT(1f);
		}

		// Token: 0x06000038 RID: 56 RVA: 0x0000318A File Offset: 0x0000138A
		public void UpdateCrosshairDecay()
		{
			this.fireDecayer.Update();
			this.hitDecayer.Update();
		}

		// Token: 0x06000039 RID: 57 RVA: 0x000031A4 File Offset: 0x000013A4
		public void DrawCrosshair()
		{
			Vector2[] array = new Vector2[]
			{
				Vector2.up,
				Vector2.right,
				Vector2.down,
				Vector2.left
			};
			Vector2[] array2 = new Vector2[]
			{
				(Vector2.up + Vector2.right).normalized,
				(Vector2.right + Vector2.down).normalized,
				(Vector2.down + Vector2.left).normalized,
				(Vector2.left + Vector2.up).normalized
			};
			float num = this.crosshairCrossThickness * Mathf.Lerp(1f, this.scaleFire, this.fireDecayer.t);
			Crosshair.<DrawCrosshair>g__DrawCross|12_0(array, this.crosshairCrossInnerRad, this.crosshairCrossOuterRad, num, this.fireDecayer.value, Color.white);
			Crosshair.<DrawCrosshair>g__DrawCross|12_0(array2, this.crosshairHitCrossInnerRad, this.crosshairHitCrossOuterRad, this.crosshairHitCrossThickness, this.hitDecayer.valueInv, new Color(1f, 0f, 0f, this.hitDecayer.t));
		}

		// Token: 0x0600003B RID: 59 RVA: 0x00003368 File Offset: 0x00001568
		[CompilerGenerated]
		internal static void <DrawCrosshair>g__DrawCross|12_0(Vector2[] dirs, float radInner, float radOuter, float thickness, float radialOffset, Color color)
		{
			foreach (Vector2 vector in dirs)
			{
				Vector2 vector2 = vector * (radInner + radialOffset);
				Vector2 vector3 = vector * (radOuter + radialOffset);
				Draw.Line(vector2, vector3, thickness, LineEndCap.Round, color);
			}
		}

		// Token: 0x0400003B RID: 59
		[Header("Style")]
		[Range(0f, 0.05f)]
		public float crosshairCrossInnerRad = 0.1f;

		// Token: 0x0400003C RID: 60
		[Range(0f, 0.05f)]
		public float crosshairCrossOuterRad = 0.3f;

		// Token: 0x0400003D RID: 61
		[Range(0f, 0.05f)]
		public float crosshairCrossThickness = 0.2f;

		// Token: 0x0400003E RID: 62
		[Range(0f, 0.05f)]
		public float crosshairHitCrossInnerRad = 0.1f;

		// Token: 0x0400003F RID: 63
		[Range(0f, 0.05f)]
		public float crosshairHitCrossOuterRad = 0.3f;

		// Token: 0x04000040 RID: 64
		[Range(0f, 0.05f)]
		public float crosshairHitCrossThickness = 0.2f;

		// Token: 0x04000041 RID: 65
		[Header("Animation")]
		[Range(0f, 1f)]
		public float scaleFire = 0.1f;

		// Token: 0x04000042 RID: 66
		public Decayer fireDecayer = new Decayer();

		// Token: 0x04000043 RID: 67
		public Decayer hitDecayer = new Decayer();
	}
}
