using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Shapes
{
	// Token: 0x02000008 RID: 8
	public class Compass : MonoBehaviour
	{
		// Token: 0x06000033 RID: 51 RVA: 0x00002CE8 File Offset: 0x00000EE8
		public void DrawCompass(Vector3 worldDir)
		{
			Compass.<>c__DisplayClass13_0 CS$<>8__locals1;
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.compArcOrigin = this.position + Vector2.down * this.bendRadius;
			CS$<>8__locals1.angUiMin = 1.5707964f - this.width / 2f / this.bendRadius;
			CS$<>8__locals1.angUiMax = 1.5707964f + this.width / 2f / this.bendRadius;
			float num = ShapesMath.DirToAng(new Vector2(worldDir.x, worldDir.z).normalized);
			CS$<>8__locals1.angWorldMin = num + this.fieldOfView / 2f;
			CS$<>8__locals1.angWorldMax = num - this.fieldOfView / 2f;
			Draw.Arc(CS$<>8__locals1.compArcOrigin, this.bendRadius, this.lineThickness, CS$<>8__locals1.angUiMin, CS$<>8__locals1.angUiMax, ArcEndCap.Round);
			Draw.LineEndCaps = LineEndCap.Square;
			Draw.LineThickness = this.lineThickness;
			Vector2 vector = CS$<>8__locals1.compArcOrigin + Vector2.up * (this.bendRadius + 0.01f);
			Vector2 vector2 = CS$<>8__locals1.compArcOrigin + Vector2.up * this.bendRadius + this.lookAngLabelOffset * 0.1f;
			string text = Mathf.RoundToInt(-num * 57.29578f + 180f).ToString() + "°";
			Draw.FontSize = this.fontSizeLookLabel;
			Draw.Text(vector2, 0f, text, TextAlign.Center);
			Vector2 vector3 = vector + ShapesMath.AngToDir(-1.5707964f) * this.triangleNootSize;
			Vector2 vector4 = vector + ShapesMath.AngToDir(0.5235988f) * this.triangleNootSize;
			Vector2 vector5 = vector + ShapesMath.AngToDir(2.6179938f) * this.triangleNootSize;
			Draw.Triangle(vector3, vector4, vector5);
			int num2 = (this.ticksPerQuarterTurn - 1) * 4;
			for (int i = 0; i < num2; i++)
			{
				float num3 = (float)i / (float)num2;
				float num4 = 6.2831855f * num3;
				bool flag = i % (num2 / 4) == 0;
				string text2 = null;
				if (flag)
				{
					switch (Mathf.RoundToInt((1f - num3) * 4f))
					{
					case 0:
					case 4:
						text2 = "S";
						break;
					case 1:
						text2 = "W";
						break;
					case 2:
						text2 = "N";
						break;
					case 3:
						text2 = "E";
						break;
					}
				}
				float num5 = ShapesMath.InverseLerpAngleRad(CS$<>8__locals1.angWorldMax, CS$<>8__locals1.angWorldMin, num4);
				if (num5 < 1f && num5 > 0f)
				{
					this.<DrawCompass>g__CompassArcNoot|13_0(num4, flag ? 0.8f : 0.5f, text2, ref CS$<>8__locals1);
				}
			}
		}

		// Token: 0x06000035 RID: 53 RVA: 0x00003054 File Offset: 0x00001254
		[CompilerGenerated]
		private void <DrawCompass>g__CompassArcNoot|13_0(float worldAng, float size, string label, ref Compass.<>c__DisplayClass13_0 A_4)
		{
			float num = ShapesMath.InverseLerpAngleRad(A_4.angWorldMax, A_4.angWorldMin, worldAng);
			float num2 = Mathf.Lerp(A_4.angUiMin, A_4.angUiMax, num);
			Vector2 vector = ShapesMath.AngToDir(num2);
			Vector2 vector2 = A_4.compArcOrigin + vector * this.bendRadius;
			Vector2 vector3 = A_4.compArcOrigin + vector * (this.bendRadius - size * this.tickSize);
			float num3 = Mathf.InverseLerp(0f, this.tickEdgeFadeFraction, 1f - Mathf.Abs(num * 2f - 1f));
			Draw.Line(vector2, vector3, LineEndCap.None, new Color(1f, 1f, 1f, num3));
			if (label != null)
			{
				Draw.FontSize = this.fontSizeTickLabel;
				Draw.Text(vector3 - vector * this.tickLabelOffset, num2 - 1.5707964f, label, TextAlign.Center, new Color(1f, 1f, 1f, num3));
			}
		}

		// Token: 0x0400002E RID: 46
		public Vector2 position;

		// Token: 0x0400002F RID: 47
		public float width = 1f;

		// Token: 0x04000030 RID: 48
		[Range(0f, 0.01f)]
		public float lineThickness = 0.1f;

		// Token: 0x04000031 RID: 49
		[Range(0.1f, 2f)]
		public float bendRadius = 1f;

		// Token: 0x04000032 RID: 50
		[Range(0.05f, 3.0787609f)]
		public float fieldOfView = 1.5707964f;

		// Token: 0x04000033 RID: 51
		[Header("Ticks")]
		public int ticksPerQuarterTurn = 12;

		// Token: 0x04000034 RID: 52
		[Range(0f, 0.2f)]
		public float tickSize = 0.1f;

		// Token: 0x04000035 RID: 53
		[Range(0f, 1f)]
		public float tickEdgeFadeFraction = 0.1f;

		// Token: 0x04000036 RID: 54
		[Range(0.01f, 0.26f)]
		public float fontSizeTickLabel = 1f;

		// Token: 0x04000037 RID: 55
		[Range(0f, 0.1f)]
		public float tickLabelOffset = 0.01f;

		// Token: 0x04000038 RID: 56
		[Header("Degree Marker")]
		[Range(0.01f, 0.26f)]
		public float fontSizeLookLabel = 1f;

		// Token: 0x04000039 RID: 57
		public Vector2 lookAngLabelOffset;

		// Token: 0x0400003A RID: 58
		[Range(0f, 0.05f)]
		public float triangleNootSize = 0.1f;
	}
}
