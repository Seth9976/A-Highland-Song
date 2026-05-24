using System;
using UnityEngine;

namespace Shapes
{
	// Token: 0x02000007 RID: 7
	public class ChargeBar : MonoBehaviour
	{
		// Token: 0x06000030 RID: 48 RVA: 0x00002938 File Offset: 0x00000B38
		public void UpdateCharge()
		{
			if (this.isCharging)
			{
				this.charge += this.chargeSpeed * Time.deltaTime;
			}
			else
			{
				this.charge -= this.chargeDecaySpeed * Time.deltaTime;
			}
			this.charge = Mathf.Clamp01(this.charge);
		}

		// Token: 0x06000031 RID: 49 RVA: 0x00002994 File Offset: 0x00000B94
		public void DrawBar(FpsController fpsController, float barRadius)
		{
			float ammoBarThickness = fpsController.ammoBarThickness;
			float ammoBarOutlineThickness = fpsController.ammoBarOutlineThickness;
			float num = -fpsController.ammoBarAngularSpanRad / 2f;
			float num2 = fpsController.ammoBarAngularSpanRad / 2f;
			float num3 = num + 3.1415927f;
			float num4 = num2 + 3.1415927f;
			float num5 = barRadius + ammoBarThickness / 2f;
			float num6 = this.chargeFillCurve.Evaluate(this.charge);
			float num7 = this.animChargeShakeMagnitude.Evaluate(num6) * this.chargeShakeMagnitude;
			Vector2 shake = fpsController.GetShake(this.chargeShakeSpeed, num7);
			float num8 = Mathf.Lerp(num4, num3, num6);
			Color color = this.chargeFillGradient.Evaluate(num6);
			Draw.Arc(shake, fpsController.ammoBarRadius, ammoBarThickness, num4, num8, color);
			Vector2 vector = shake + ShapesMath.AngToDir(num8) * barRadius;
			Draw.Disc(shake + ShapesMath.AngToDir(num4) * barRadius, ammoBarThickness / 2f, color);
			Draw.LineEndCaps = LineEndCap.None;
			for (int i = 0; i < 7; i++)
			{
				float num9 = (float)i / 6f;
				float num10 = Mathf.Lerp(num4, num3, num9);
				Vector2 vector2 = ShapesMath.AngToDir(num10);
				Vector2 vector3 = shake + vector2 * num5;
				bool flag = i % 3 == 0;
				Vector2 vector4 = vector3 + vector2 * (flag ? this.tickSizeLorge : this.tickSizeSmol);
				Draw.Line(vector3, vector4, this.tickTickness, this.tickColor);
				float num11 = num9 - num6;
				float num12 = ((num11 < 0f) ? this.fontGrowRangePrev : this.fontGrowRangeNext);
				float num13 = 1f - ShapesMath.SmoothCos01(Mathf.Clamp01(Mathf.Abs(num11) / num12));
				Draw.FontSize = ShapesMath.Eerp(this.fontSize, this.fontSizeLorge, num13);
				Vector2 vector5 = vector3 + vector2 * this.percentLabelOffset;
				string text = Mathf.RoundToInt(num9 * 100f).ToString() + "%";
				Draw.Text(vector5, num10 + 3.1415927f, text, TextAlign.Right);
			}
			Draw.Disc(vector, ammoBarThickness / 2f + ammoBarOutlineThickness / 2f);
			Draw.Disc(vector, ammoBarThickness / 2f - ammoBarOutlineThickness / 2f, color);
			FpsController.DrawRoundedArcOutline(shake, barRadius, ammoBarThickness, ammoBarOutlineThickness, num3, num4);
			Draw.LineEndCaps = LineEndCap.Round;
			Draw.BlendMode = ShapesBlendMode.Additive;
			Draw.DiscGradientRadial(vector, ammoBarThickness * 2f, color, Color.clear);
			Draw.BlendMode = ShapesBlendMode.Transparent;
		}

		// Token: 0x0400001C RID: 28
		[Header("Gameplay")]
		[SerializeField]
		private float chargeSpeed = 1f;

		// Token: 0x0400001D RID: 29
		[SerializeField]
		private float chargeDecaySpeed = 1f;

		// Token: 0x0400001E RID: 30
		[NonSerialized]
		public bool isCharging;

		// Token: 0x0400001F RID: 31
		private float charge;

		// Token: 0x04000020 RID: 32
		[Header("Style")]
		public Color tickColor = Color.white;

		// Token: 0x04000021 RID: 33
		public Gradient chargeFillGradient;

		// Token: 0x04000022 RID: 34
		[Range(0f, 0.1f)]
		public float tickSizeSmol = 0.1f;

		// Token: 0x04000023 RID: 35
		[Range(0f, 0.1f)]
		public float tickSizeLorge = 0.1f;

		// Token: 0x04000024 RID: 36
		[Range(0f, 0.05f)]
		public float tickTickness;

		// Token: 0x04000025 RID: 37
		[Range(0f, 0.5f)]
		public float fontSize = 0.1f;

		// Token: 0x04000026 RID: 38
		[Range(0f, 0.5f)]
		public float fontSizeLorge = 0.1f;

		// Token: 0x04000027 RID: 39
		[Range(0f, 0.1f)]
		public float percentLabelOffset = 0.1f;

		// Token: 0x04000028 RID: 40
		[Range(0f, 0.4f)]
		public float fontGrowRangePrev = 0.1f;

		// Token: 0x04000029 RID: 41
		[Range(0f, 0.4f)]
		public float fontGrowRangeNext = 0.1f;

		// Token: 0x0400002A RID: 42
		[Header("Animation")]
		public AnimationCurve chargeFillCurve;

		// Token: 0x0400002B RID: 43
		public AnimationCurve animChargeShakeMagnitude = AnimationCurve.Linear(0f, 0f, 1f, 1f);

		// Token: 0x0400002C RID: 44
		[Range(0f, 0.05f)]
		public float chargeShakeMagnitude = 0.1f;

		// Token: 0x0400002D RID: 45
		public float chargeShakeSpeed = 1f;
	}
}
