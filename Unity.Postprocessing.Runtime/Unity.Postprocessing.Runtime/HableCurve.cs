using System;

namespace UnityEngine.Rendering.PostProcessing
{
	// Token: 0x0200005B RID: 91
	public class HableCurve
	{
		// Token: 0x17000027 RID: 39
		// (get) Token: 0x060001AD RID: 429 RVA: 0x0000E022 File Offset: 0x0000C222
		// (set) Token: 0x060001AE RID: 430 RVA: 0x0000E02A File Offset: 0x0000C22A
		public float whitePoint { get; private set; }

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x060001AF RID: 431 RVA: 0x0000E033 File Offset: 0x0000C233
		// (set) Token: 0x060001B0 RID: 432 RVA: 0x0000E03B File Offset: 0x0000C23B
		public float inverseWhitePoint { get; private set; }

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x060001B1 RID: 433 RVA: 0x0000E044 File Offset: 0x0000C244
		// (set) Token: 0x060001B2 RID: 434 RVA: 0x0000E04C File Offset: 0x0000C24C
		internal float x0 { get; private set; }

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x060001B3 RID: 435 RVA: 0x0000E055 File Offset: 0x0000C255
		// (set) Token: 0x060001B4 RID: 436 RVA: 0x0000E05D File Offset: 0x0000C25D
		internal float x1 { get; private set; }

		// Token: 0x060001B5 RID: 437 RVA: 0x0000E068 File Offset: 0x0000C268
		public HableCurve()
		{
			for (int i = 0; i < 3; i++)
			{
				this.m_Segments[i] = new HableCurve.Segment();
			}
			this.uniforms = new HableCurve.Uniforms(this);
		}

		// Token: 0x060001B6 RID: 438 RVA: 0x0000E0AC File Offset: 0x0000C2AC
		public float Eval(float x)
		{
			float num = x * this.inverseWhitePoint;
			int num2 = ((num < this.x0) ? 0 : ((num < this.x1) ? 1 : 2));
			return this.m_Segments[num2].Eval(num);
		}

		// Token: 0x060001B7 RID: 439 RVA: 0x0000E0EC File Offset: 0x0000C2EC
		public void Init(float toeStrength, float toeLength, float shoulderStrength, float shoulderLength, float shoulderAngle, float gamma)
		{
			HableCurve.DirectParams directParams = default(HableCurve.DirectParams);
			toeLength = Mathf.Pow(Mathf.Clamp01(toeLength), 2.2f);
			toeStrength = Mathf.Clamp01(toeStrength);
			shoulderAngle = Mathf.Clamp01(shoulderAngle);
			shoulderStrength = Mathf.Clamp(shoulderStrength, 1E-05f, 0.99999f);
			shoulderLength = Mathf.Max(0f, shoulderLength);
			gamma = Mathf.Max(1E-05f, gamma);
			float num = toeLength * 0.5f;
			float num2 = (1f - toeStrength) * num;
			float num3 = 1f - num2;
			float num4 = num + num3;
			float num5 = (1f - shoulderStrength) * num3;
			float num6 = num + num5;
			float num7 = num2 + num5;
			float num8 = RuntimeUtilities.Exp2(shoulderLength) - 1f;
			float num9 = num4 + num8;
			directParams.x0 = num;
			directParams.y0 = num2;
			directParams.x1 = num6;
			directParams.y1 = num7;
			directParams.W = num9;
			directParams.gamma = gamma;
			directParams.overshootX = directParams.W * 2f * shoulderAngle * shoulderLength;
			directParams.overshootY = 0.5f * shoulderAngle * shoulderLength;
			this.InitSegments(directParams);
		}

		// Token: 0x060001B8 RID: 440 RVA: 0x0000E200 File Offset: 0x0000C400
		private void InitSegments(HableCurve.DirectParams srcParams)
		{
			HableCurve.DirectParams directParams = srcParams;
			this.whitePoint = srcParams.W;
			this.inverseWhitePoint = 1f / srcParams.W;
			directParams.W = 1f;
			directParams.x0 /= srcParams.W;
			directParams.x1 /= srcParams.W;
			directParams.overshootX = srcParams.overshootX / srcParams.W;
			float num;
			float num2;
			this.AsSlopeIntercept(out num, out num2, directParams.x0, directParams.x1, directParams.y0, directParams.y1);
			float gamma = srcParams.gamma;
			HableCurve.Segment segment = this.m_Segments[1];
			segment.offsetX = -(num2 / num);
			segment.offsetY = 0f;
			segment.scaleX = 1f;
			segment.scaleY = 1f;
			segment.lnA = gamma * Mathf.Log(num);
			segment.B = gamma;
			float num3 = this.EvalDerivativeLinearGamma(num, num2, gamma, directParams.x0);
			float num4 = this.EvalDerivativeLinearGamma(num, num2, gamma, directParams.x1);
			directParams.y0 = Mathf.Max(1E-05f, Mathf.Pow(directParams.y0, directParams.gamma));
			directParams.y1 = Mathf.Max(1E-05f, Mathf.Pow(directParams.y1, directParams.gamma));
			directParams.overshootY = Mathf.Pow(1f + directParams.overshootY, directParams.gamma) - 1f;
			this.x0 = directParams.x0;
			this.x1 = directParams.x1;
			HableCurve.Segment segment2 = this.m_Segments[0];
			segment2.offsetX = 0f;
			segment2.offsetY = 0f;
			segment2.scaleX = 1f;
			segment2.scaleY = 1f;
			float num5;
			float num6;
			this.SolveAB(out num5, out num6, directParams.x0, directParams.y0, num3);
			segment2.lnA = num5;
			segment2.B = num6;
			HableCurve.Segment segment3 = this.m_Segments[2];
			float num7 = 1f + directParams.overshootX - directParams.x1;
			float num8 = 1f + directParams.overshootY - directParams.y1;
			float num9;
			float num10;
			this.SolveAB(out num9, out num10, num7, num8, num4);
			segment3.offsetX = 1f + directParams.overshootX;
			segment3.offsetY = 1f + directParams.overshootY;
			segment3.scaleX = -1f;
			segment3.scaleY = -1f;
			segment3.lnA = num9;
			segment3.B = num10;
			float num11 = this.m_Segments[2].Eval(1f);
			float num12 = 1f / num11;
			this.m_Segments[0].offsetY *= num12;
			this.m_Segments[0].scaleY *= num12;
			this.m_Segments[1].offsetY *= num12;
			this.m_Segments[1].scaleY *= num12;
			this.m_Segments[2].offsetY *= num12;
			this.m_Segments[2].scaleY *= num12;
		}

		// Token: 0x060001B9 RID: 441 RVA: 0x0000E519 File Offset: 0x0000C719
		private void SolveAB(out float lnA, out float B, float x0, float y0, float m)
		{
			B = m * x0 / y0;
			lnA = Mathf.Log(y0) - B * Mathf.Log(x0);
		}

		// Token: 0x060001BA RID: 442 RVA: 0x0000E538 File Offset: 0x0000C738
		private void AsSlopeIntercept(out float m, out float b, float x0, float x1, float y0, float y1)
		{
			float num = y1 - y0;
			float num2 = x1 - x0;
			if (num2 == 0f)
			{
				m = 1f;
			}
			else
			{
				m = num / num2;
			}
			b = y0 - x0 * m;
		}

		// Token: 0x060001BB RID: 443 RVA: 0x0000E56F File Offset: 0x0000C76F
		private float EvalDerivativeLinearGamma(float m, float b, float g, float x)
		{
			return g * m * Mathf.Pow(m * x + b, g - 1f);
		}

		// Token: 0x04000198 RID: 408
		private readonly HableCurve.Segment[] m_Segments = new HableCurve.Segment[3];

		// Token: 0x04000199 RID: 409
		public readonly HableCurve.Uniforms uniforms;

		// Token: 0x02000087 RID: 135
		private class Segment
		{
			// Token: 0x06000245 RID: 581 RVA: 0x00010F68 File Offset: 0x0000F168
			public float Eval(float x)
			{
				float num = (x - this.offsetX) * this.scaleX;
				float num2 = 0f;
				if (num > 0f)
				{
					num2 = Mathf.Exp(this.lnA + this.B * Mathf.Log(num));
				}
				return num2 * this.scaleY + this.offsetY;
			}

			// Token: 0x040002E3 RID: 739
			public float offsetX;

			// Token: 0x040002E4 RID: 740
			public float offsetY;

			// Token: 0x040002E5 RID: 741
			public float scaleX;

			// Token: 0x040002E6 RID: 742
			public float scaleY;

			// Token: 0x040002E7 RID: 743
			public float lnA;

			// Token: 0x040002E8 RID: 744
			public float B;
		}

		// Token: 0x02000088 RID: 136
		private struct DirectParams
		{
			// Token: 0x040002E9 RID: 745
			internal float x0;

			// Token: 0x040002EA RID: 746
			internal float y0;

			// Token: 0x040002EB RID: 747
			internal float x1;

			// Token: 0x040002EC RID: 748
			internal float y1;

			// Token: 0x040002ED RID: 749
			internal float W;

			// Token: 0x040002EE RID: 750
			internal float overshootX;

			// Token: 0x040002EF RID: 751
			internal float overshootY;

			// Token: 0x040002F0 RID: 752
			internal float gamma;
		}

		// Token: 0x02000089 RID: 137
		public class Uniforms
		{
			// Token: 0x06000247 RID: 583 RVA: 0x00010FC4 File Offset: 0x0000F1C4
			internal Uniforms(HableCurve parent)
			{
				this.parent = parent;
			}

			// Token: 0x17000044 RID: 68
			// (get) Token: 0x06000248 RID: 584 RVA: 0x00010FD3 File Offset: 0x0000F1D3
			public Vector4 curve
			{
				get
				{
					return new Vector4(this.parent.inverseWhitePoint, this.parent.x0, this.parent.x1, 0f);
				}
			}

			// Token: 0x17000045 RID: 69
			// (get) Token: 0x06000249 RID: 585 RVA: 0x00011000 File Offset: 0x0000F200
			public Vector4 toeSegmentA
			{
				get
				{
					HableCurve.Segment segment = this.parent.m_Segments[0];
					return new Vector4(segment.offsetX, segment.offsetY, segment.scaleX, segment.scaleY);
				}
			}

			// Token: 0x17000046 RID: 70
			// (get) Token: 0x0600024A RID: 586 RVA: 0x00011038 File Offset: 0x0000F238
			public Vector4 toeSegmentB
			{
				get
				{
					HableCurve.Segment segment = this.parent.m_Segments[0];
					return new Vector4(segment.lnA, segment.B, 0f, 0f);
				}
			}

			// Token: 0x17000047 RID: 71
			// (get) Token: 0x0600024B RID: 587 RVA: 0x00011070 File Offset: 0x0000F270
			public Vector4 midSegmentA
			{
				get
				{
					HableCurve.Segment segment = this.parent.m_Segments[1];
					return new Vector4(segment.offsetX, segment.offsetY, segment.scaleX, segment.scaleY);
				}
			}

			// Token: 0x17000048 RID: 72
			// (get) Token: 0x0600024C RID: 588 RVA: 0x000110A8 File Offset: 0x0000F2A8
			public Vector4 midSegmentB
			{
				get
				{
					HableCurve.Segment segment = this.parent.m_Segments[1];
					return new Vector4(segment.lnA, segment.B, 0f, 0f);
				}
			}

			// Token: 0x17000049 RID: 73
			// (get) Token: 0x0600024D RID: 589 RVA: 0x000110E0 File Offset: 0x0000F2E0
			public Vector4 shoSegmentA
			{
				get
				{
					HableCurve.Segment segment = this.parent.m_Segments[2];
					return new Vector4(segment.offsetX, segment.offsetY, segment.scaleX, segment.scaleY);
				}
			}

			// Token: 0x1700004A RID: 74
			// (get) Token: 0x0600024E RID: 590 RVA: 0x00011118 File Offset: 0x0000F318
			public Vector4 shoSegmentB
			{
				get
				{
					HableCurve.Segment segment = this.parent.m_Segments[2];
					return new Vector4(segment.lnA, segment.B, 0f, 0f);
				}
			}

			// Token: 0x040002F1 RID: 753
			private HableCurve parent;
		}
	}
}
