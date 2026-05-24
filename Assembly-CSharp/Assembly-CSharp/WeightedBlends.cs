using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Token: 0x020001EA RID: 490
public static class WeightedBlends
{
	// Token: 0x06001156 RID: 4438 RVA: 0x00080760 File Offset: 0x0007E960
	public static bool WeightedBlend(IEnumerable<bool> values, IList<float> weights)
	{
		float num = 0f;
		float num2 = 0f;
		int num3 = 0;
		foreach (bool flag in values)
		{
			num2 += weights[num3];
			if (flag)
			{
				num += weights[num3];
			}
			num3++;
		}
		return num / num2 > 0.5f;
	}

	// Token: 0x06001157 RID: 4439 RVA: 0x000807D4 File Offset: 0x0007E9D4
	public static float WeightedBlend(IEnumerable<float> values, IList<float> weights)
	{
		return WeightedBlends.WeightedBlend<float>(values, (float f) => f, weights);
	}

	// Token: 0x06001158 RID: 4440 RVA: 0x000807FC File Offset: 0x0007E9FC
	public static float WeightedBlend<T>(IEnumerable<T> values, Func<T, float> selector, IList<float> weights)
	{
		return WeightedBlends.WeightedBlend<T, float, float>(values, selector, weights, (float total, float val, float weight) => total + weight * val, (float total) => total);
	}

	// Token: 0x06001159 RID: 4441 RVA: 0x0008084F File Offset: 0x0007EA4F
	public static Vector3 WeightedBlend(IEnumerable<Vector3> values, IList<float> weights)
	{
		return WeightedBlends.WeightedBlend<Vector3>(values, (Vector3 v) => v, weights);
	}

	// Token: 0x0600115A RID: 4442 RVA: 0x00080878 File Offset: 0x0007EA78
	public static Vector3 WeightedBlend<T>(IEnumerable<T> values, Func<T, Vector3> selector, IList<float> weights)
	{
		return WeightedBlends.WeightedBlend<T, Vector3, Vector3>(values, selector, weights, (Vector3 total, Vector3 val, float weight) => total + weight * val, (Vector3 total) => total);
	}

	// Token: 0x0600115B RID: 4443 RVA: 0x000808CC File Offset: 0x0007EACC
	public static SerializableTransform WeightedBlend(IEnumerable<SerializableTransform> values, IList<float> weights)
	{
		return new SerializableTransform(WeightedBlends.WeightedBlend(values.Select((SerializableTransform x) => x.position), weights), WeightedBlends.WeightedBlend(values.Select((SerializableTransform x) => x.rotation), weights), Vector3.one);
	}

	// Token: 0x0600115C RID: 4444 RVA: 0x0008093C File Offset: 0x0007EB3C
	public static float WeightedBlendAngle(IEnumerable<float> values, IList<float> weights)
	{
		return WeightedBlends.WeightedBlend<float, float, Vector2>(values, (float v) => v, weights, (Vector2 totalDirection, float angle, float weight) => totalDirection + weight * WeightedBlends.WithDegrees(angle), (Vector2 totalDirection) => Mathf.Atan2(-totalDirection.y, totalDirection.x) * 57.29578f);
	}

	// Token: 0x0600115D RID: 4445 RVA: 0x000809B0 File Offset: 0x0007EBB0
	public static float WeightedBlendAngle<T>(IEnumerable<T> values, Func<T, float> selector, IList<float> weights)
	{
		return WeightedBlends.WeightedBlend<T, float, Vector2>(values, selector, weights, (Vector2 totalDirection, float angle, float weight) => totalDirection + weight * WeightedBlends.WithDegrees(angle), (Vector2 totalDirection) => Mathf.Atan2(-totalDirection.y, totalDirection.x) * 57.29578f);
	}

	// Token: 0x0600115E RID: 4446 RVA: 0x00080A04 File Offset: 0x0007EC04
	public static Quaternion WeightedBlend<T>(IEnumerable<T> values, Func<T, Quaternion> selector, IList<float> weights)
	{
		return WeightedBlends.WeightedBlend<T, Quaternion, WeightedBlends.WeightedAxes>(values, selector, weights, delegate(WeightedBlends.WeightedAxes axes, Quaternion q, float weight)
		{
			if (weight == 0f)
			{
				return axes;
			}
			Vector3 vector = q * Vector3.forward;
			Vector3 vector2 = q * Vector3.up;
			axes.forward += weight * vector;
			axes.up += weight * vector2;
			return axes;
		}, delegate(WeightedBlends.WeightedAxes axes)
		{
			if (axes.forward == Vector3.zero || axes.up == Vector3.zero)
			{
				return Quaternion.identity;
			}
			return Quaternion.LookRotation(axes.forward.normalized, axes.up.normalized);
		});
	}

	// Token: 0x0600115F RID: 4447 RVA: 0x00080A57 File Offset: 0x0007EC57
	public static Quaternion WeightedBlend(IEnumerable<Quaternion> values, IList<float> weights)
	{
		return WeightedBlends.WeightedBlend<Quaternion>(values, (Quaternion q) => q, weights);
	}

	// Token: 0x06001160 RID: 4448 RVA: 0x00080A80 File Offset: 0x0007EC80
	private static U WeightedBlend<T, U, TAccum>(IEnumerable<T> values, Func<T, U> selector, IList<float> weights, Func<TAccum, U, float, TAccum> accumFunc, Func<TAccum, U> resultFunc)
	{
		TAccum taccum = default(TAccum);
		int num = 0;
		foreach (T t in values)
		{
			float num2 = weights[num];
			U u = selector(t);
			taccum = accumFunc(taccum, u, num2);
			num++;
		}
		return resultFunc(taccum);
	}

	// Token: 0x06001161 RID: 4449 RVA: 0x00080AF8 File Offset: 0x0007ECF8
	private static T Identity<T>(T t)
	{
		return t;
	}

	// Token: 0x06001162 RID: 4450 RVA: 0x00080AFC File Offset: 0x0007ECFC
	private static Vector2 WithDegrees(float degrees)
	{
		float num = degrees * 0.017453292f;
		return new Vector2(Mathf.Sin(num), Mathf.Cos(num));
	}

	// Token: 0x020003FC RID: 1020
	private struct WeightedAxes
	{
		// Token: 0x04001ABD RID: 6845
		public Vector3 forward;

		// Token: 0x04001ABE RID: 6846
		public Vector3 up;
	}
}
