using System;
using UnityEngine;

namespace Shapes
{
	// Token: 0x02000006 RID: 6
	public class AmmoBar : MonoBehaviour
	{
		// Token: 0x06000029 RID: 41 RVA: 0x00002690 File Offset: 0x00000890
		private Vector2 GetBulletEjectPos(Vector2 origin, float t)
		{
			Vector2 vector = new Vector2(this.bulletEjectX.Evaluate(t), this.bulletEjectY.Evaluate(t));
			return origin + vector * this.bulletEjectScale;
		}

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x0600002A RID: 42 RVA: 0x000026CE File Offset: 0x000008CE
		public bool HasBulletsLeft
		{
			get
			{
				return this.bullets > 0;
			}
		}

		// Token: 0x0600002B RID: 43 RVA: 0x000026DC File Offset: 0x000008DC
		public void Fire()
		{
			float[] array = this.bulletFireTimes;
			int num = this.bullets - 1;
			this.bullets = num;
			array[num] = Time.time;
		}

		// Token: 0x0600002C RID: 44 RVA: 0x00002706 File Offset: 0x00000906
		public void Reload()
		{
			this.bullets = this.totalBullets;
		}

		// Token: 0x0600002D RID: 45 RVA: 0x00002714 File Offset: 0x00000914
		private void Awake()
		{
			this.bulletFireTimes = new float[this.totalBullets];
		}

		// Token: 0x0600002E RID: 46 RVA: 0x00002728 File Offset: 0x00000928
		public void DrawBar(FpsController fpsController, float barRadius)
		{
			float ammoBarThickness = fpsController.ammoBarThickness;
			float ammoBarOutlineThickness = fpsController.ammoBarOutlineThickness;
			float num = -fpsController.ammoBarAngularSpanRad / 2f;
			float num2 = fpsController.ammoBarAngularSpanRad / 2f;
			Draw.LineEndCaps = LineEndCap.Round;
			float num3 = (barRadius - ammoBarThickness / 2f) * fpsController.ammoBarAngularSpanRad / (float)this.totalBullets * this.bulletThicknessScale;
			for (int i = 0; i < this.totalBullets; i++)
			{
				float num4 = (float)i / ((float)this.totalBullets - 1f);
				Vector2 vector = ShapesMath.AngToDir(Mathf.Lerp(num, num2, num4));
				Vector2 vector2 = vector * barRadius;
				Vector2 vector3 = vector * (ammoBarThickness / 2f - ammoBarOutlineThickness * 1.5f);
				float num5 = 1f;
				if (i >= this.bullets && Application.isPlaying)
				{
					float num6 = Time.time - this.bulletFireTimes[i];
					float num7 = Mathf.Clamp01(num6 / this.bulletDisappearTime);
					num5 = 1f - num7;
					vector2 = this.GetBulletEjectPos(vector2, num7);
					float num8 = num6 * (this.bulletEjectAngSpeed + Mathf.Cos((float)i * 92372.8f) * this.ejectRotSpeedVariance);
					vector3 = ShapesMath.Rotate(vector3, num8);
				}
				Vector2 vector4 = vector2 + vector3;
				Vector2 vector5 = vector2 - vector3;
				Draw.Line(vector4, vector5, num3, new Color(1f, 1f, 1f, num5));
			}
			FpsController.DrawRoundedArcOutline(Vector2.zero, barRadius, ammoBarThickness, ammoBarOutlineThickness, num, num2);
		}

		// Token: 0x04000012 RID: 18
		public int totalBullets = 20;

		// Token: 0x04000013 RID: 19
		public int bullets = 15;

		// Token: 0x04000014 RID: 20
		[Header("Style")]
		[Range(0f, 1f)]
		public float bulletThicknessScale = 1f;

		// Token: 0x04000015 RID: 21
		[Range(0f, 0.5f)]
		public float bulletEjectScale = 0.5f;

		// Token: 0x04000016 RID: 22
		[Header("Animation")]
		public float bulletDisappearTime = 1f;

		// Token: 0x04000017 RID: 23
		[Range(0f, 6.2831855f)]
		public float bulletEjectAngSpeed = 0.5f;

		// Token: 0x04000018 RID: 24
		[Range(0f, 6.2831855f)]
		public float ejectRotSpeedVariance = 1f;

		// Token: 0x04000019 RID: 25
		public AnimationCurve bulletEjectX = AnimationCurve.Constant(0f, 1f, 0f);

		// Token: 0x0400001A RID: 26
		public AnimationCurve bulletEjectY = AnimationCurve.Constant(0f, 1f, 0f);

		// Token: 0x0400001B RID: 27
		private float[] bulletFireTimes;
	}
}
