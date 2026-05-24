using System;
using UnityEngine;

namespace SplineSystem
{
	// Token: 0x02000227 RID: 551
	[Serializable]
	public class SplineBezierCurve
	{
		// Token: 0x060013F1 RID: 5105 RVA: 0x0008B248 File Offset: 0x00089448
		public SplineBezierCurve(SplineBezierPoint startPoint, SplineBezierPoint endPoint)
		{
			this.startPoint = startPoint;
			this.endPoint = endPoint;
		}

		// Token: 0x060013F2 RID: 5106 RVA: 0x0008B298 File Offset: 0x00089498
		public void CacheData()
		{
			this.p0 = this.startPoint.position;
			this.p1 = this.startPoint.outControlPoint.GetPosition(this.startPoint);
			this.p2 = this.endPoint.inControlPoint.GetPosition(this.endPoint);
			this.p3 = this.endPoint.position;
		}

		// Token: 0x060013F3 RID: 5107 RVA: 0x0008B2FF File Offset: 0x000894FF
		public float GetRoughLength()
		{
			return Bezier.GetRoughLength(this.p0, this.p1, this.p2, this.p3);
		}

		// Token: 0x060013F4 RID: 5108 RVA: 0x0008B320 File Offset: 0x00089520
		public void SetLength()
		{
			Bezier.GetRoughLength(this.p0, this.p1, this.p2, this.p3);
			float num = 1f / (float)(this.numArcLengthsForArcLengthToTCalculation - 1);
			Vector3 vector = this.GetPointAtT(0f);
			this.length = 0f;
			for (int i = 1; i < this.numArcLengthsForArcLengthToTCalculation; i++)
			{
				Vector3 pointAtT = this.GetPointAtT(num * (float)i);
				this.length += (pointAtT - vector).magnitude;
				vector = pointAtT;
			}
		}

		// Token: 0x060013F5 RID: 5109 RVA: 0x0008B3B0 File Offset: 0x000895B0
		public void SetArcLengths()
		{
			this.numArcLengthsForArcLengthToTCalculationReciprocal = 1f / (float)(this.numArcLengthsForArcLengthToTCalculation - 1);
			if (this._arcLengths == null || this._arcLengths.Length != this.numArcLengthsForArcLengthToTCalculation)
			{
				this._arcLengths = new float[this.numArcLengthsForArcLengthToTCalculation];
			}
			if (this._points == null || this._points.Length != this.numArcLengthsForArcLengthToTCalculation)
			{
				this._points = new Vector3[this.numArcLengthsForArcLengthToTCalculation];
			}
			Vector3 vector = this.GetPointAtT(0f);
			this._arcLengths[0] = 0f;
			this._points[0] = vector;
			float num = 0f;
			for (int i = 1; i < this.numArcLengthsForArcLengthToTCalculation; i++)
			{
				Vector3 pointAtT = this.GetPointAtT(this.numArcLengthsForArcLengthToTCalculationReciprocal * (float)i);
				num += (pointAtT - vector).magnitude;
				this._arcLengths[i] = num;
				this._points[i] = pointAtT;
				vector = pointAtT;
			}
		}

		// Token: 0x060013F6 RID: 5110 RVA: 0x0008B49C File Offset: 0x0008969C
		public void SetBounds()
		{
			int num = 8;
			float num2 = 1f / (float)(num - 1);
			Vector3 vector = this.GetPointAtNormalizedLocalArcLength(0f);
			this.bounds = new Bounds(vector, Vector3.zero);
			for (int i = 1; i < num; i++)
			{
				vector = this.GetPointAtNormalizedLocalArcLength(num2 * (float)i);
				this.bounds.Encapsulate(vector);
			}
		}

		// Token: 0x060013F7 RID: 5111 RVA: 0x0008B4F6 File Offset: 0x000896F6
		public float GetTAtBezierArcLength(float bezierArcLength)
		{
			return this.GetTAtNormalizedBezierArcLength(this.NormalizeBezierArcLength(bezierArcLength));
		}

		// Token: 0x060013F8 RID: 5112 RVA: 0x0008B508 File Offset: 0x00089708
		public float GetTAtNormalizedBezierArcLength(float u)
		{
			if (u <= 0f)
			{
				return 0f;
			}
			if (u >= 1f)
			{
				return 1f;
			}
			int num = 0;
			int i = 0;
			int num2 = this._arcLengths.Length - 1;
			float num3 = u * this.length;
			float num4 = -1f;
			while (i < num2)
			{
				num = (int)((uint)(i + num2) >> 1);
				num4 = this._arcLengths[num];
				if (num4 < num3)
				{
					i = num + 1;
				}
				else
				{
					num2 = num;
				}
			}
			if (num4 > num3)
			{
				num--;
			}
			if (num < 0)
			{
				return 0f;
			}
			return ((float)num + Mathf.InverseLerp(this._arcLengths[num], this._arcLengths[num + 1], num3)) * this.numArcLengthsForArcLengthToTCalculationReciprocal;
		}

		// Token: 0x060013F9 RID: 5113 RVA: 0x0008B5A8 File Offset: 0x000897A8
		public float EstimateNormalizedBezierArcLengthAtT(float t)
		{
			return this.EstimateBezierArcLengthAtT(t) / this.length;
		}

		// Token: 0x060013FA RID: 5114 RVA: 0x0008B5B8 File Offset: 0x000897B8
		public float EstimateBezierArcLengthAtT(float t)
		{
			if (t <= 0f)
			{
				return 0f;
			}
			float num = 0f;
			float num2 = 0f;
			float num3 = this._arcLengths[0];
			for (int i = 1; i < this.numArcLengthsForArcLengthToTCalculation; i++)
			{
				num += this.numArcLengthsForArcLengthToTCalculationReciprocal;
				float num4 = this._arcLengths[i];
				float num5 = num4 - num3;
				if (num > t)
				{
					float num6 = Mathf.InverseLerp(num - this.numArcLengthsForArcLengthToTCalculationReciprocal, num, t);
					return num2 + num5 * num6;
				}
				num2 += num5;
				num3 = num4;
			}
			return this.length;
		}

		// Token: 0x060013FB RID: 5115 RVA: 0x0008B645 File Offset: 0x00089845
		public float NormalizeBezierArcLength(float bezierArcLength)
		{
			return (bezierArcLength - this.startArcLength) / (this.endArcLength - this.startArcLength);
		}

		// Token: 0x060013FC RID: 5116 RVA: 0x0008B65D File Offset: 0x0008985D
		public Vector3 GetPointAtArcLength(float distance)
		{
			return this.GetPointAtNormalizedLocalArcLength(this.NormalizeBezierArcLength(distance));
		}

		// Token: 0x060013FD RID: 5117 RVA: 0x0008B66C File Offset: 0x0008986C
		public Vector3 GetPointAtNormalizedLocalArcLength(float normalizedDistance)
		{
			float tatNormalizedBezierArcLength = this.GetTAtNormalizedBezierArcLength(normalizedDistance);
			return this.GetPointAtT(tatNormalizedBezierArcLength, normalizedDistance);
		}

		// Token: 0x060013FE RID: 5118 RVA: 0x0008B689 File Offset: 0x00089889
		public Vector3 GetPointAtT(float t)
		{
			return Bezier.GetPoint(this.p0, this.p1, this.p2, this.p3, t);
		}

		// Token: 0x060013FF RID: 5119 RVA: 0x0008B6AC File Offset: 0x000898AC
		public Vector3 GetPointAtT(float t, float normalizedDistance)
		{
			Vector3 vector = this.GetPointAtT(t);
			if (normalizedDistance < 0f)
			{
				vector += this.GetDirectionAtT(t) * normalizedDistance * this.length;
			}
			else if (normalizedDistance > 1f)
			{
				vector += this.GetDirectionAtT(t) * (normalizedDistance - 1f) * this.length;
			}
			return vector;
		}

		// Token: 0x06001400 RID: 5120 RVA: 0x0008B718 File Offset: 0x00089918
		public Vector3 GetDirectionAtArcLength(float distance)
		{
			return this.GetDirectionAtNormalizedLocalArcLength(this.NormalizeBezierArcLength(distance));
		}

		// Token: 0x06001401 RID: 5121 RVA: 0x0008B727 File Offset: 0x00089927
		public Vector3 GetDirectionAtNormalizedLocalArcLength(float normalizedDistance)
		{
			return this.GetDirectionAtT(this.GetTAtNormalizedBezierArcLength(normalizedDistance));
		}

		// Token: 0x06001402 RID: 5122 RVA: 0x0008B738 File Offset: 0x00089938
		public Vector3 GetDirectionAtT(float t)
		{
			if (t <= 0f || this.p0 == this.p1)
			{
				return this.startPoint.forward;
			}
			if (t >= 1f || this.p2 == this.p3)
			{
				return this.endPoint.forward;
			}
			return Bezier.GetFirstDerivative(this.p0, this.p1, this.p2, this.p3, t).normalized;
		}

		// Token: 0x06001403 RID: 5123 RVA: 0x0008B7B9 File Offset: 0x000899B9
		public Quaternion GetRotationAtArcLength(float distance)
		{
			return this.GetRotationAtNormalizedLocalArcLength(this.NormalizeBezierArcLength(distance));
		}

		// Token: 0x06001404 RID: 5124 RVA: 0x0008B7C8 File Offset: 0x000899C8
		public Quaternion GetRotationAtNormalizedLocalArcLength(float normalizedDistance)
		{
			Vector3 directionAtNormalizedLocalArcLength = this.GetDirectionAtNormalizedLocalArcLength(normalizedDistance);
			Vector3 vector = Vector3.Slerp(this.startPoint.normal, this.endPoint.normal, normalizedDistance);
			return Quaternion.LookRotation(directionAtNormalizedLocalArcLength, vector);
		}

		// Token: 0x06001405 RID: 5125 RVA: 0x0008B800 File Offset: 0x00089A00
		public Quaternion GetRotationAtNormalizedLocalArcLength(float normalizedDistance, out Vector3 direction)
		{
			direction = this.GetDirectionAtNormalizedLocalArcLength(normalizedDistance);
			Vector3 vector = Vector3.Slerp(this.startPoint.normal, this.endPoint.normal, normalizedDistance);
			return Quaternion.LookRotation(direction, vector);
		}

		// Token: 0x040012F2 RID: 4850
		[NonSerialized]
		public SplineBezierPoint startPoint;

		// Token: 0x040012F3 RID: 4851
		[NonSerialized]
		public SplineBezierPoint endPoint;

		// Token: 0x040012F4 RID: 4852
		public int numArcLengthsForArcLengthToTCalculation;

		// Token: 0x040012F5 RID: 4853
		public float numArcLengthsForArcLengthToTCalculationReciprocal;

		// Token: 0x040012F6 RID: 4854
		public float[] _arcLengths;

		// Token: 0x040012F7 RID: 4855
		public Vector3[] _points;

		// Token: 0x040012F8 RID: 4856
		public Bounds bounds;

		// Token: 0x040012F9 RID: 4857
		public float length;

		// Token: 0x040012FA RID: 4858
		public float startArcLength;

		// Token: 0x040012FB RID: 4859
		public float endArcLength;

		// Token: 0x040012FC RID: 4860
		[SerializeField]
		private Vector3 p0 = Vector3.zero;

		// Token: 0x040012FD RID: 4861
		[SerializeField]
		private Vector3 p1 = Vector3.zero;

		// Token: 0x040012FE RID: 4862
		[SerializeField]
		private Vector3 p2 = Vector3.zero;

		// Token: 0x040012FF RID: 4863
		[SerializeField]
		private Vector3 p3 = Vector3.zero;
	}
}
