using System;
using System.Collections.Generic;
using UnityEngine;

namespace SplineSystem
{
	// Token: 0x02000225 RID: 549
	[Serializable]
	public class Spline
	{
		// Token: 0x060013C2 RID: 5058 RVA: 0x00089F50 File Offset: 0x00088150
		public Vector3 GetPointAtArcLength(float arcLength)
		{
			return this.GetCurveAtArcLength(arcLength).GetPointAtArcLength(arcLength);
		}

		// Token: 0x060013C3 RID: 5059 RVA: 0x00089F5F File Offset: 0x0008815F
		public Vector3 GetPointAtArcLength(float arcLength, Matrix4x4 localToWorldMatrix)
		{
			return localToWorldMatrix.MultiplyPoint3x4(this.GetPointAtArcLength(arcLength));
		}

		// Token: 0x060013C4 RID: 5060 RVA: 0x00089F6F File Offset: 0x0008816F
		public Quaternion GetRotationAtArcLength(float arcLength)
		{
			return this.GetCurveAtArcLength(arcLength).GetRotationAtArcLength(arcLength);
		}

		// Token: 0x060013C5 RID: 5061 RVA: 0x00089F80 File Offset: 0x00088180
		public Quaternion GetRotationAtArcLength(float arcLength, Matrix4x4 localToWorldMatrix)
		{
			Quaternion rotationAtArcLength = this.GetRotationAtArcLength(arcLength);
			return localToWorldMatrix.rotation * rotationAtArcLength;
		}

		// Token: 0x060013C6 RID: 5062 RVA: 0x00089FA2 File Offset: 0x000881A2
		public Vector3 GetDirectionAtArcLength(float arcLength)
		{
			return this.GetCurveAtArcLength(arcLength).GetDirectionAtArcLength(arcLength);
		}

		// Token: 0x060013C7 RID: 5063 RVA: 0x00089FB1 File Offset: 0x000881B1
		public Vector3 GetDirectionAtArcLength(float arcLength, Matrix4x4 localToWorldMatrix)
		{
			return localToWorldMatrix.MultiplyVector(this.GetDirectionAtArcLength(arcLength));
		}

		// Token: 0x1700045A RID: 1114
		// (get) Token: 0x060013C8 RID: 5064 RVA: 0x00089FC1 File Offset: 0x000881C1
		public float length
		{
			get
			{
				return this.curves[this.curves.Length - 1].endArcLength;
			}
		}

		// Token: 0x060013C9 RID: 5065 RVA: 0x00089FD9 File Offset: 0x000881D9
		public Spline(params SplineBezierPoint[] bezierPoints)
		{
			this.bezierPoints = bezierPoints;
			this.RefreshCurveData();
		}

		// Token: 0x060013CA RID: 5066 RVA: 0x00089FFC File Offset: 0x000881FC
		public static Spline CreateFromPoints(IList<Vector3> points, Vector3 upVector, float normalizedBezierPositionBetweenPoints = 0.5f, float normalizedControlPointDistance = 0.25f)
		{
			if (points.Count <= 1)
			{
				Debug.LogError("Can't create spline because points array only has " + points.Count.ToString() + " items");
				return null;
			}
			bool flag = normalizedBezierPositionBetweenPoints < 0.5f;
			int num = (flag ? (points.Count * 2 - 2) : (points.Count + 1));
			SplineBezierPoint[] array = new SplineBezierPoint[num];
			int num2 = 0;
			if (flag)
			{
				for (int i = 0; i < points.Count; i++)
				{
					if (i > 0)
					{
						Vector3 vector = points[i] - points[i - 1];
						float magnitude = vector.magnitude;
						Quaternion quaternion = Quaternion.LookRotation(vector, upVector);
						Vector3 vector2 = points[i];
						if (i < points.Count - 1)
						{
							vector2 -= vector * normalizedBezierPositionBetweenPoints;
						}
						array[num2] = new SplineBezierPoint(vector2, quaternion, magnitude * normalizedControlPointDistance, magnitude * normalizedControlPointDistance);
						num2++;
					}
					if (i < points.Count - 1)
					{
						Vector3 vector3 = points[i + 1] - points[i];
						float magnitude2 = vector3.magnitude;
						Quaternion quaternion2 = Quaternion.LookRotation(vector3, upVector);
						Vector3 vector4 = points[i];
						if (i > 0)
						{
							vector4 += vector3 * normalizedBezierPositionBetweenPoints;
						}
						array[num2] = new SplineBezierPoint(vector4, quaternion2, magnitude2 * normalizedControlPointDistance, magnitude2 * normalizedControlPointDistance);
						num2++;
					}
				}
			}
			else
			{
				for (int j = 0; j < num; j++)
				{
					Vector3 vector5 = points[num2];
					Quaternion quaternion3 = Quaternion.identity;
					float num3;
					if (j == 0)
					{
						Vector3 vector6 = points[j + 1] - points[j];
						num3 = vector6.magnitude;
						quaternion3 = Quaternion.LookRotation(vector6, upVector);
					}
					else if (j == num - 1)
					{
						vector5 = points[num2];
						Vector3 vector7 = points[num2] - points[num2 - 1];
						num3 = vector7.magnitude;
						quaternion3 = Quaternion.LookRotation(vector7, upVector);
						num2++;
					}
					else
					{
						vector5 = Vector3.Lerp(points[j - 1], points[j], 0.5f);
						Vector3 vector8 = points[j] - points[j - 1];
						num3 = vector8.magnitude;
						quaternion3 = Quaternion.LookRotation(vector8, upVector);
						num2++;
					}
					array[j] = new SplineBezierPoint(vector5, quaternion3, num3 * normalizedControlPointDistance, num3 * normalizedControlPointDistance);
				}
			}
			return new Spline(array);
		}

		// Token: 0x060013CB RID: 5067 RVA: 0x0008A27C File Offset: 0x0008847C
		public static Spline CreateFromPoints(IList<Vector3> points, IList<Quaternion> rotations, float normalizedControlPointDistance = 0.25f)
		{
			int count = points.Count;
			SplineBezierPoint[] array = new SplineBezierPoint[count];
			for (int i = 0; i < count; i++)
			{
				Vector3 vector = points[i];
				Quaternion quaternion = rotations[i];
				float num2;
				float num;
				if (i == 0)
				{
					num = (num2 = (points[i + 1] - points[i]).magnitude);
				}
				else if (i == count - 1)
				{
					num = (num2 = (points[i] - points[i - 1]).magnitude);
				}
				else
				{
					num = (points[i + 1] - points[i]).magnitude;
					num2 = (points[i] - points[i - 1]).magnitude;
				}
				array[i] = new SplineBezierPoint(vector, quaternion, num2 * normalizedControlPointDistance, num * normalizedControlPointDistance);
			}
			return new Spline(array);
		}

		// Token: 0x060013CC RID: 5068 RVA: 0x0008A378 File Offset: 0x00088578
		public void RefreshCurveData()
		{
			if (this.bezierPoints == null || this.bezierPoints.Length <= 1)
			{
				return;
			}
			if (this.curves == null || this.curves.Length != this.bezierPoints.Length - 1)
			{
				this.curves = new SplineBezierCurve[this.bezierPoints.Length - 1];
			}
			for (int i = 0; i < this.bezierPoints.Length - 1; i++)
			{
				this.curves[i] = new SplineBezierCurve(this.bezierPoints[i], this.bezierPoints[i + 1]);
			}
			float num = 0f;
			foreach (SplineBezierCurve splineBezierCurve in this.curves)
			{
				splineBezierCurve.CacheData();
				splineBezierCurve.numArcLengthsForArcLengthToTCalculation = Mathf.Max(2, Mathf.CeilToInt(this.quality));
				splineBezierCurve.SetLength();
				splineBezierCurve.SetArcLengths();
				splineBezierCurve.startArcLength = num;
				num += splineBezierCurve.length;
				splineBezierCurve.endArcLength = num;
			}
			for (int k = 0; k < this.curves.Length; k++)
			{
				this.curves[k].SetBounds();
				if (k == 0)
				{
					this.bounds = new Bounds(this.curves[k].bounds.center, this.curves[k].bounds.size);
				}
				else
				{
					this.bounds.Encapsulate(this.curves[k].bounds);
				}
			}
		}

		// Token: 0x060013CD RID: 5069 RVA: 0x0008A4E4 File Offset: 0x000886E4
		public int GetCurveIndexAtArcLength(float targetDistance)
		{
			int i = 0;
			int num = this.curves.Length - 1;
			if (num < 0)
			{
				Debug.LogError("The array cannot be empty");
			}
			while (i <= num)
			{
				int num2 = (int)((uint)(i + num) >> 1);
				float startArcLength = this.curves[num2].startArcLength;
				if (startArcLength < targetDistance)
				{
					i = num2 + 1;
				}
				else
				{
					if (startArcLength <= targetDistance)
					{
						return num2;
					}
					num = num2 - 1;
				}
			}
			if (num < 0)
			{
				num = 0;
			}
			return num;
		}

		// Token: 0x060013CE RID: 5070 RVA: 0x0008A543 File Offset: 0x00088743
		public SplineBezierCurve GetCurveAtArcLength(float arcLength)
		{
			return this.curves[this.GetCurveIndexAtArcLength(arcLength)];
		}

		// Token: 0x060013CF RID: 5071 RVA: 0x0008A554 File Offset: 0x00088754
		public float GetTAtArcLength(float arcLength)
		{
			int curveIndexAtArcLength = this.GetCurveIndexAtArcLength(arcLength);
			return this.curves[curveIndexAtArcLength].GetTAtBezierArcLength(arcLength);
		}

		// Token: 0x060013D0 RID: 5072 RVA: 0x0008A577 File Offset: 0x00088777
		public float ClampArcLength(float targetArcLength)
		{
			return Mathf.Clamp(targetArcLength, 0f, this.length);
		}

		// Token: 0x060013D1 RID: 5073 RVA: 0x0008A58C File Offset: 0x0008878C
		private static void RemoveCurves(Vector3 position, ref List<SplineBezierCurve> curvesToTry, int numSamples, ref float smallestSqrDistance)
		{
			if (curvesToTry.Count <= 1)
			{
				return;
			}
			Spline.sqrDistances.Clear();
			for (int i = 0; i < curvesToTry.Count; i++)
			{
				SplineBezierCurve splineBezierCurve = curvesToTry[i];
				Spline.sqrDistances.Add(Spline.SqrDistance(position, splineBezierCurve.bounds.ClosestPoint(position)));
				smallestSqrDistance = Mathf.Min(smallestSqrDistance, Spline.sqrDistances[i]);
			}
			float num = 1f + 1f / (float)numSamples * 2f;
			float num2 = Mathf.Max(400f, smallestSqrDistance * num);
			for (int j = curvesToTry.Count - 1; j >= 0; j--)
			{
				if (Spline.sqrDistances[j] > num2)
				{
					curvesToTry.RemoveAt(j);
				}
			}
		}

		// Token: 0x060013D2 RID: 5074 RVA: 0x0008A650 File Offset: 0x00088850
		public bool RoughEstimateBestCurveT(Func<Vector3, float> scoringFunction, ref SplineBezierCurve bestCurve, ref float bestT, ref float bestScore)
		{
			bool flag = false;
			foreach (SplineBezierCurve splineBezierCurve in this.curves)
			{
				for (int j = 0; j < splineBezierCurve._points.Length; j++)
				{
					Vector3 vector = splineBezierCurve._points[j];
					float num = scoringFunction(vector);
					if (num > bestScore)
					{
						bestScore = num;
						bestCurve = splineBezierCurve;
						bestT = (float)j * splineBezierCurve.numArcLengthsForArcLengthToTCalculationReciprocal;
						flag = true;
					}
				}
			}
			return flag;
		}

		// Token: 0x060013D3 RID: 5075 RVA: 0x0008A6C8 File Offset: 0x000888C8
		private void GetBestCurve(Vector3 position, SplineBezierCurve curve, ref SplineBezierCurve bestCurve, ref float bestDistance, ref Vector3 bestPoint, ref float bestT)
		{
			for (int i = 0; i < curve.numArcLengthsForArcLengthToTCalculation; i++)
			{
				Vector3 vector = curve._points[i];
				float num = Spline.SqrDistance(position, vector);
				if (num == bestDistance)
				{
					float num2 = (float)i * curve.numArcLengthsForArcLengthToTCalculationReciprocal;
					Vector3 directionAtT = curve.GetDirectionAtT(num2);
					if (Vector3.Dot(position - vector, directionAtT) > 0f)
					{
						bestCurve = curve;
						bestDistance = num;
						bestPoint = vector;
						bestT = num2;
					}
				}
				else if (num < bestDistance)
				{
					bestCurve = curve;
					bestDistance = num;
					bestPoint = vector;
					bestT = (float)i * curve.numArcLengthsForArcLengthToTCalculationReciprocal;
				}
			}
		}

		// Token: 0x060013D4 RID: 5076 RVA: 0x0008A764 File Offset: 0x00088964
		public float EstimateArcLengthAlongCurve(Vector3 position, bool clampAtStart = false, bool clampAtEnd = false)
		{
			if (this.curves == null || this.curves.Length == 0)
			{
				Debug.LogError("No curves in spline");
				return 0f;
			}
			Spline.curvesToTry.Clear();
			Spline.curvesToTry.AddRange(this.curves);
			float positiveInfinity = float.PositiveInfinity;
			Spline.RemoveCurves(position, ref Spline.curvesToTry, 3, ref positiveInfinity);
			SplineBezierCurve splineBezierCurve = null;
			float positiveInfinity2 = float.PositiveInfinity;
			Vector3 zero = Vector3.zero;
			float num = 0f;
			foreach (SplineBezierCurve splineBezierCurve2 in Spline.curvesToTry)
			{
				this.GetBestCurve(position, splineBezierCurve2, ref splineBezierCurve, ref positiveInfinity2, ref zero, ref num);
			}
			return this.EstimateArcLengthAlongCurve(position, splineBezierCurve, num, clampAtStart, clampAtEnd);
		}

		// Token: 0x060013D5 RID: 5077 RVA: 0x0008A834 File Offset: 0x00088A34
		public float EstimateArcLengthAlongCurve(Vector3 position, SplineBezierCurve curve, bool clampAtStart = false, bool clampAtEnd = false)
		{
			SplineBezierCurve splineBezierCurve = curve;
			float positiveInfinity = float.PositiveInfinity;
			Vector3 zero = Vector3.zero;
			float num = 0f;
			this.GetBestCurve(position, curve, ref splineBezierCurve, ref positiveInfinity, ref zero, ref num);
			return this.EstimateArcLengthAlongCurve(position, splineBezierCurve, num, clampAtStart, clampAtEnd);
		}

		// Token: 0x060013D6 RID: 5078 RVA: 0x0008A874 File Offset: 0x00088A74
		private float EstimateArcLengthAlongCurve(Vector3 position, SplineBezierCurve bestCurve, float bestT, bool clampAtStart = false, bool clampAtEnd = false)
		{
			float num = bestCurve.numArcLengthsForArcLengthToTCalculationReciprocal;
			float num2 = Mathf.Clamp01(bestT - num / 2f);
			float num3 = Mathf.Clamp01(bestT + num / 2f);
			float num4 = this.SqrDistanceAtTValue(bestCurve, num2, position);
			float num5 = this.SqrDistanceAtTValue(bestCurve, num3, position);
			int num6 = 0;
			bool flag = false;
			while (!flag && num6 < 20)
			{
				if (num4 < num5)
				{
					num3 = (num2 + num3) / 2f;
					num5 = this.SqrDistanceAtTValue(bestCurve, num3, position);
				}
				else
				{
					num2 = (num2 + num3) / 2f;
					num4 = this.SqrDistanceAtTValue(bestCurve, num2, position);
				}
				num6++;
				flag = (num3 - num2) * bestCurve.length < 0.05f || num4 < 0.01f;
			}
			num = num3 - num2;
			bestT = Mathf.Lerp(num2, num3, 0.5f);
			float num7 = ((bestT == num3) ? num2 : (bestT - num * 0.5f));
			float num8 = ((bestT == num2) ? num3 : (bestT + num * 0.5f));
			Vector3 pointAtT = bestCurve.GetPointAtT(bestT);
			Vector3 pointAtT2 = bestCurve.GetPointAtT(num7);
			Vector3 pointAtT3 = bestCurve.GetPointAtT(num8);
			float normalizedDistanceOnLine = Spline.GetNormalizedDistanceOnLine(pointAtT2, pointAtT, position, clampAtStart);
			float normalizedDistanceOnLine2 = Spline.GetNormalizedDistanceOnLine(pointAtT, pointAtT3, position, clampAtEnd);
			float num9 = Spline.SqrDistance(position, Vector3.LerpUnclamped(pointAtT2, pointAtT, normalizedDistanceOnLine));
			float num10 = Spline.SqrDistance(position, Vector3.LerpUnclamped(pointAtT, pointAtT3, normalizedDistanceOnLine2));
			float num11;
			float num12;
			if (num9 < num10)
			{
				num11 = Vector3.Distance(pointAtT2, pointAtT);
				num12 = normalizedDistanceOnLine - 1f;
			}
			else
			{
				num11 = Vector3.Distance(pointAtT, pointAtT3);
				num12 = normalizedDistanceOnLine2;
			}
			num11 *= num12;
			float num13 = bestCurve.EstimateBezierArcLengthAtT(bestT) + num11;
			if (clampAtStart)
			{
				num13 = Mathf.Max(num13, 0f);
			}
			if (clampAtEnd)
			{
				num13 = Mathf.Min(num13, bestCurve.length);
			}
			return bestCurve.startArcLength + num13;
		}

		// Token: 0x060013D7 RID: 5079 RVA: 0x0008AA34 File Offset: 0x00088C34
		private float SqrDistanceAtTValue(SplineBezierCurve curve, float t, Vector3 samplePosition)
		{
			Vector3 pointAtT = curve.GetPointAtT(t);
			return Spline.SqrDistance(samplePosition, pointAtT);
		}

		// Token: 0x060013D8 RID: 5080 RVA: 0x0008AA50 File Offset: 0x00088C50
		private void SubdivideInCurve(SplineBezierCurve curve, Vector3 samplePosition, ref float bestT, ref float tRange, int numSamples)
		{
			float num = bestT - tRange * 0.5f;
			float num2 = 1f / (float)(numSamples - 1);
			num2 *= tRange;
			tRange = num2;
			float num3 = float.PositiveInfinity;
			for (int i = 0; i < numSamples; i++)
			{
				float num4 = num + (float)i * num2;
				Vector3 pointAtT = curve.GetPointAtT(num4);
				float num5 = Spline.SqrDistance(samplePosition, pointAtT);
				if (num5 < num3)
				{
					num3 = num5;
					bestT = num4;
				}
			}
		}

		// Token: 0x060013D9 RID: 5081 RVA: 0x0008AABB File Offset: 0x00088CBB
		public IEnumerable<Vector3> GetVerts(int numPoints)
		{
			float r = this.length / (float)(numPoints - 1);
			int num;
			for (int i = 0; i < numPoints; i = num + 1)
			{
				yield return this.GetPointAtArcLength(r * (float)i);
				num = i;
			}
			yield break;
		}

		// Token: 0x060013DA RID: 5082 RVA: 0x0008AAD2 File Offset: 0x00088CD2
		public IEnumerable<Vector3> GetVerts(int numPoints, Matrix4x4 localToWorldMatrix)
		{
			float r = this.length / (float)(numPoints - 1);
			int num;
			for (int i = 0; i < numPoints; i = num + 1)
			{
				yield return this.GetPointAtArcLength(r * (float)i, localToWorldMatrix);
				num = i;
			}
			yield break;
		}

		// Token: 0x060013DB RID: 5083 RVA: 0x0008AAF0 File Offset: 0x00088CF0
		public IEnumerable<Vector3> GetVerts(int numPoints, float startArcLength, float endArcLength)
		{
			float num = endArcLength - startArcLength;
			float r = num / (float)(numPoints - 1);
			int num2;
			for (int i = 0; i < numPoints; i = num2 + 1)
			{
				yield return this.GetPointAtArcLength(startArcLength + r * (float)i);
				num2 = i;
			}
			yield break;
		}

		// Token: 0x060013DC RID: 5084 RVA: 0x0008AB15 File Offset: 0x00088D15
		public IEnumerable<Vector3> GetVerts(int numPoints, float startArcLength, float endArcLength, Matrix4x4 localToWorldMatrix)
		{
			float num = endArcLength - startArcLength;
			float r = num / (float)(numPoints - 1);
			int num2;
			for (int i = 0; i < numPoints; i = num2 + 1)
			{
				yield return this.GetPointAtArcLength(startArcLength + r * (float)i, localToWorldMatrix);
				num2 = i;
			}
			yield break;
		}

		// Token: 0x060013DD RID: 5085 RVA: 0x0008AB44 File Offset: 0x00088D44
		public IEnumerable<Vector3> GetVertsWithPointsPerMeter(float pointsPerMeter)
		{
			int num = Mathf.Max(Mathf.CeilToInt(this.length * pointsPerMeter), 2);
			float num2 = this.length / (float)(num - 1);
			return this.GetVerts(num);
		}

		// Token: 0x060013DE RID: 5086 RVA: 0x0008AB78 File Offset: 0x00088D78
		public IEnumerable<Vector3> GetVertsWithPointsPerMeter(float pointsPerMeter, Matrix4x4 localToWorldMatrix)
		{
			int num = Mathf.Max(Mathf.CeilToInt(this.length * pointsPerMeter), 2);
			float num2 = this.length / (float)(num - 1);
			return this.GetVerts(num, localToWorldMatrix);
		}

		// Token: 0x060013DF RID: 5087 RVA: 0x0008ABB0 File Offset: 0x00088DB0
		public IEnumerable<Vector3> GetVertsWithPointsPerMeter(float pointsPerMeter, float startArcLength, float endArcLength)
		{
			int num = Mathf.Max(Mathf.CeilToInt((endArcLength - startArcLength) * pointsPerMeter), 2);
			return this.GetVerts(num, startArcLength, endArcLength);
		}

		// Token: 0x060013E0 RID: 5088 RVA: 0x0008ABD8 File Offset: 0x00088DD8
		public IEnumerable<Vector3> GetVertsWithPointsPerMeter(float pointsPerMeter, float startArcLength, float endArcLength, Matrix4x4 localToWorldMatrix)
		{
			int num = Mathf.Max(Mathf.CeilToInt((endArcLength - startArcLength) * pointsPerMeter), 2);
			return this.GetVerts(num, startArcLength, endArcLength, localToWorldMatrix);
		}

		// Token: 0x060013E1 RID: 5089 RVA: 0x0008AC01 File Offset: 0x00088E01
		public static IEnumerable<Vector3> GetCurveVerts(Spline spline, SplineBezierCurve curve, int numPoints)
		{
			numPoints = Mathf.Min(numPoints, curve.numArcLengthsForArcLengthToTCalculation);
			float r = 1f / (float)(numPoints - 1);
			int num2;
			for (int i = 0; i < numPoints; i = num2 + 1)
			{
				float num = Mathf.Lerp(curve.startArcLength, curve.endArcLength, (float)i * r);
				yield return spline.GetPointAtArcLength(num);
				num2 = i;
			}
			yield break;
		}

		// Token: 0x060013E2 RID: 5090 RVA: 0x0008AC20 File Offset: 0x00088E20
		public static Spline Transform(Spline spline, Matrix4x4 matrix)
		{
			SplineBezierPoint[] array = new SplineBezierPoint[spline.bezierPoints.Length];
			for (int i = 0; i < spline.bezierPoints.Length; i++)
			{
				SplineBezierPoint splineBezierPoint = spline.bezierPoints[i];
				Vector3 vector = matrix.MultiplyPoint3x4(splineBezierPoint.position);
				Quaternion quaternion = Quaternion.LookRotation(matrix.MultiplyVector(splineBezierPoint.rotation * Vector3.forward), matrix.MultiplyVector(splineBezierPoint.rotation * Vector3.up));
				Vector3 vector2 = matrix.MultiplyPoint3x4(splineBezierPoint.inControlPoint.GetPosition(splineBezierPoint));
				Vector3 normalized = matrix.MultiplyVector(splineBezierPoint.inControlPoint.GetDirection(splineBezierPoint)).normalized;
				float num = Vector3.Dot(vector2 - vector, normalized);
				Vector3 vector3 = matrix.MultiplyPoint3x4(splineBezierPoint.outControlPoint.GetPosition(splineBezierPoint));
				Vector3 normalized2 = matrix.MultiplyVector(splineBezierPoint.outControlPoint.GetDirection(splineBezierPoint)).normalized;
				float num2 = Vector3.Dot(vector3 - vector, normalized2);
				array[i] = new SplineBezierPoint(vector, quaternion, num, num2);
			}
			return new Spline(array);
		}

		// Token: 0x060013E3 RID: 5091 RVA: 0x0008AD44 File Offset: 0x00088F44
		public bool Validate()
		{
			bool flag = false;
			if (this.quality <= 0f)
			{
				this.quality = 25f;
				flag = true;
			}
			if (this.bezierPoints == null || this.bezierPoints.Length < 2)
			{
				this.bezierPoints = new SplineBezierPoint[]
				{
					new SplineBezierPoint(new Vector3(-1f, 1f, 0f), Quaternion.LookRotation(Vector3.right, Vector3.forward), 1f, 1f),
					new SplineBezierPoint(new Vector3(1f, -1f, 0f), Quaternion.LookRotation(Vector3.right, Vector3.forward), 1f, 1f)
				};
				flag = true;
			}
			else
			{
				for (int i = 0; i < this.bezierPoints.Length; i++)
				{
					if (this.bezierPoints[i].rotation.x == 0f && this.bezierPoints[i].rotation.y == 0f && this.bezierPoints[i].rotation.z == 0f && this.bezierPoints[i].rotation.w == 0f)
					{
						this.bezierPoints[i].rotation = Quaternion.identity;
						flag = true;
					}
					if (this.bezierPoints[i].inControlPoint.directionSign != -1)
					{
						this.bezierPoints[i].inControlPoint.directionSign = -1;
						flag = true;
					}
					if (this.bezierPoints[i].inControlPoint.distance < 0f)
					{
						this.bezierPoints[i].inControlPoint.distance = 0f;
						flag = true;
					}
					if (this.bezierPoints[i].outControlPoint.directionSign != 1)
					{
						this.bezierPoints[i].outControlPoint.directionSign = 1;
						flag = true;
					}
					if (this.bezierPoints[i].outControlPoint.distance < 0f)
					{
						this.bezierPoints[i].outControlPoint.distance = 0f;
						flag = true;
					}
				}
			}
			if (flag)
			{
				this.RefreshCurveData();
			}
			return flag;
		}

		// Token: 0x060013E4 RID: 5092 RVA: 0x0008AF90 File Offset: 0x00089190
		public static void DrawSplineGizmos(Spline spline, int numPoints)
		{
			for (int i = 0; i < spline.curves.Length; i++)
			{
				Spline.DrawCurveLineGizmos(spline, spline.curves[i], Matrix4x4.identity, numPoints);
			}
		}

		// Token: 0x060013E5 RID: 5093 RVA: 0x0008AFC4 File Offset: 0x000891C4
		public static void DrawSplineGizmos(Spline spline, Matrix4x4 localToWorldMatrix, int numPoints)
		{
			for (int i = 0; i < spline.curves.Length; i++)
			{
				Spline.DrawCurveLineGizmos(spline, spline.curves[i], localToWorldMatrix, numPoints);
			}
		}

		// Token: 0x060013E6 RID: 5094 RVA: 0x0008AFF4 File Offset: 0x000891F4
		public static void DrawSplineGizmosWithPointsPerMeter(Spline spline, Matrix4x4 localToWorldMatrix, int pointsPerMeter)
		{
			for (int i = 0; i < spline.curves.Length; i++)
			{
				int num = Mathf.Max(Mathf.CeilToInt(spline.curves[i].length * (float)pointsPerMeter), 2);
				Spline.DrawCurveLineGizmos(spline, spline.curves[i], localToWorldMatrix, num);
			}
		}

		// Token: 0x060013E7 RID: 5095 RVA: 0x0008B040 File Offset: 0x00089240
		public static void DrawCurveLineGizmos(Spline spline, SplineBezierCurve curve, Matrix4x4 localToWorldMatrix, int numPoints)
		{
			numPoints = Mathf.Min(numPoints, curve.numArcLengthsForArcLengthToTCalculation);
			float num = 1f / (float)(numPoints - 1);
			Vector3 vector = localToWorldMatrix.MultiplyPoint3x4(spline.GetPointAtArcLength(curve.startArcLength));
			for (int i = 1; i < numPoints; i++)
			{
				float num2 = Mathf.Lerp(curve.startArcLength, curve.endArcLength, (float)i * num);
				Vector3 vector2 = localToWorldMatrix.MultiplyPoint3x4(spline.GetPointAtArcLength(num2));
				Gizmos.DrawLine(vector, vector2);
				vector = vector2;
			}
		}

		// Token: 0x060013E8 RID: 5096 RVA: 0x0008B0B8 File Offset: 0x000892B8
		public static void DrawSplineGizmos(Spline spline, Matrix4x4 localToWorldMatrix, float startArcLength, float endArcLength, int numPoints)
		{
			IEnumerable<Vector3> vertsWithPointsPerMeter = spline.GetVertsWithPointsPerMeter((float)numPoints, startArcLength, endArcLength, localToWorldMatrix);
			Vector3 vector = Vector3.zero;
			bool flag = true;
			foreach (Vector3 vector2 in vertsWithPointsPerMeter)
			{
				if (flag)
				{
					flag = false;
				}
				else
				{
					Gizmos.DrawLine(vector, vector2);
				}
				vector = vector2;
			}
		}

		// Token: 0x060013E9 RID: 5097 RVA: 0x0008B11C File Offset: 0x0008931C
		private static float SqrDistance(Vector3 a, Vector3 b)
		{
			return (a.x - b.x) * (a.x - b.x) + (a.y - b.y) * (a.y - b.y) + (a.z - b.z) * (a.z - b.z);
		}

		// Token: 0x060013EA RID: 5098 RVA: 0x0008B17C File Offset: 0x0008937C
		private static float GetNormalizedDistanceOnLine(Vector3 start, Vector3 end, Vector3 p, bool clamped = true)
		{
			float num = Spline.SqrDistance(start, end);
			return Spline.GetNormalizedDistanceOnLineInternal(start, end, p, num, clamped);
		}

		// Token: 0x060013EB RID: 5099 RVA: 0x0008B19C File Offset: 0x0008939C
		private static float GetNormalizedDistanceOnLineInternal(Vector3 start, Vector3 end, Vector3 p, float sqrLength, bool clamped = true)
		{
			if (sqrLength == 0f)
			{
				return 0f;
			}
			float num = Vector3.Dot(p - start, end - start) / sqrLength;
			if (!clamped)
			{
				return num;
			}
			return Mathf.Clamp01(num);
		}

		// Token: 0x040012E9 RID: 4841
		private const float defaultQuality = 25f;

		// Token: 0x040012EA RID: 4842
		public float quality = 25f;

		// Token: 0x040012EB RID: 4843
		public SplineBezierPoint[] bezierPoints;

		// Token: 0x040012EC RID: 4844
		public SplineBezierCurve[] curves;

		// Token: 0x040012ED RID: 4845
		public Bounds bounds;

		// Token: 0x040012EE RID: 4846
		private static List<float> sqrDistances = new List<float>();

		// Token: 0x040012EF RID: 4847
		private static List<SplineBezierCurve> curvesToTry = new List<SplineBezierCurve>();
	}
}
