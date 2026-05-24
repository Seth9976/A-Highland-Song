using System;
using System.Runtime.CompilerServices;
using UnityEngine;

// Token: 0x020000F3 RID: 243
public static class BezierSimple
{
	// Token: 0x060007DB RID: 2011 RVA: 0x00045DB8 File Offset: 0x00043FB8
	private static Vector2 HandleVec(BezierSimple.Point b)
	{
		float num = 0.017453292f * b.angle;
		return b.strength * new Vector2(Mathf.Sin(num), Mathf.Cos(num));
	}

	// Token: 0x060007DC RID: 2012 RVA: 0x00045DF0 File Offset: 0x00043FF0
	public static Vector2 Evaluate(BezierSimple.Point[] bezier, float t)
	{
		if (t < 0f || t > (float)(bezier.Length - 1))
		{
			Debug.LogError("Out of range: " + t.ToString());
			return Vector2.zero;
		}
		int num = Mathf.FloorToInt(t);
		int num2 = Mathf.CeilToInt(t) % bezier.Length;
		float num3 = t - (float)num;
		BezierSimple._bezierEvalPoints[0] = bezier[num].pos;
		BezierSimple._bezierEvalPoints[1] = bezier[num].pos + BezierSimple.HandleVec(bezier[num]);
		BezierSimple._bezierEvalPoints[2] = bezier[num2].pos - BezierSimple.HandleVec(bezier[num2]);
		BezierSimple._bezierEvalPoints[3] = bezier[num2].pos;
		for (int i = BezierSimple._bezierEvalPoints.Length; i > 1; i--)
		{
			Vector2 vector = BezierSimple._bezierEvalPoints[0];
			for (int j = 0; j < i - 1; j++)
			{
				Vector2 vector2 = BezierSimple._bezierEvalPoints[j + 1];
				BezierSimple._bezierEvalPoints[j] = Vector2.Lerp(vector, vector2, num3);
				vector = vector2;
			}
		}
		return BezierSimple._bezierEvalPoints[0];
	}

	// Token: 0x060007DD RID: 2013 RVA: 0x00045F28 File Offset: 0x00044128
	[return: TupleElementNames(new string[] { "point", "newT" })]
	public static ValueTuple<Vector2, float> EvaluateStep(BezierSimple.Point[] bezier, float t, float distanceDelta)
	{
		Vector2 vector = BezierSimple.Evaluate(bezier, t);
		float num = ((t - Mathf.Floor(t) > 0.5f) ? (t - 0.0001f) : (t + 0.0001f));
		float num2 = Vector2.Distance(BezierSimple.Evaluate(bezier, num), vector) / 0.0001f;
		float num3 = distanceDelta / num2;
		int num4 = Mathf.FloorToInt(t) % bezier.Length;
		int num5 = Mathf.CeilToInt(t) % bezier.Length;
		float num6 = 0.5f;
		float num7 = Vector2.Distance(bezier[num4].pos, bezier[num5].pos);
		if (num7 > 2f * distanceDelta)
		{
			num6 = 2f * distanceDelta / num7;
		}
		if (num3 > num6)
		{
			num3 = num6;
		}
		float num8 = t + num3;
		if (num8 >= (float)(bezier.Length - 1))
		{
			num8 = (float)(bezier.Length - 1);
		}
		return new ValueTuple<Vector2, float>(BezierSimple.Evaluate(bezier, num8), num8);
	}

	// Token: 0x060007DE RID: 2014 RVA: 0x00045FFC File Offset: 0x000441FC
	public static void DrawGizmo(BezierSimple.Point[] points, float iterateDist)
	{
		ValueTuple<Vector2, float> valueTuple = new ValueTuple<Vector2, float>(points[0].pos, 0f);
		while (valueTuple.Item2 < (float)(points.Length - 1))
		{
			ValueTuple<Vector2, float> valueTuple2 = BezierSimple.EvaluateStep(points, valueTuple.Item2, iterateDist);
			Gizmos.DrawLine(valueTuple.Item1, valueTuple2.Item1);
			valueTuple = valueTuple2;
		}
	}

	// Token: 0x040009D5 RID: 2517
	private static Vector2[] _bezierEvalPoints = new Vector2[4];

	// Token: 0x02000318 RID: 792
	public struct Point
	{
		// Token: 0x040017C0 RID: 6080
		public Vector2 pos;

		// Token: 0x040017C1 RID: 6081
		public float angle;

		// Token: 0x040017C2 RID: 6082
		public float strength;
	}
}
