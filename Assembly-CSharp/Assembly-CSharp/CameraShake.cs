using System;
using UnityEngine;

// Token: 0x0200001D RID: 29
public class CameraShake : ScriptableObject
{
	// Token: 0x060000AC RID: 172 RVA: 0x00009E9D File Offset: 0x0000809D
	public float GetRawStrengthAtTime(float time)
	{
		return this.shakeStrengthOverTime.Evaluate(time);
	}

	// Token: 0x060000AD RID: 173 RVA: 0x00009EAB File Offset: 0x000080AB
	public Vector2 GetViewportOffsetAtTime(float time)
	{
		return this.RandomOnUnitCircle() * this.viewportStrength * this.GetRawStrengthAtTime(time);
	}

	// Token: 0x060000AE RID: 174 RVA: 0x00009ECA File Offset: 0x000080CA
	public bool IsCompleteAtTime(float time)
	{
		return time >= this.shakeStrengthOverTime.keys[this.shakeStrengthOverTime.keys.Length - 1].time;
	}

	// Token: 0x060000AF RID: 175 RVA: 0x00009EF8 File Offset: 0x000080F8
	private Vector2 RandomOnUnitCircle()
	{
		float num = Random.value * 360f;
		return new Vector2(Mathf.Sin(num * 0.017453292f), Mathf.Cos(num * 0.017453292f));
	}

	// Token: 0x04000108 RID: 264
	public CameraShakeName shakeName;

	// Token: 0x04000109 RID: 265
	public AnimationCurve shakeStrengthOverTime;

	// Token: 0x0400010A RID: 266
	public float viewportStrength = 0.02f;
}
