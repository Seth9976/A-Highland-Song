using System;
using UnityEngine;

// Token: 0x0200005A RID: 90
[Serializable]
public struct VibrationMoment
{
	// Token: 0x170000BC RID: 188
	// (get) Token: 0x06000274 RID: 628 RVA: 0x00015180 File Offset: 0x00013380
	public static VibrationMoment zero
	{
		get
		{
			return default(VibrationMoment);
		}
	}

	// Token: 0x06000275 RID: 629 RVA: 0x00015198 File Offset: 0x00013398
	public static VibrationMoment Create(float strength)
	{
		return new VibrationMoment
		{
			rightStrength = strength,
			leftStrength = strength,
			largeMotorStrength = strength,
			smallMotorStrength = strength
		};
	}

	// Token: 0x06000276 RID: 630 RVA: 0x000151D4 File Offset: 0x000133D4
	public static VibrationMoment Create(float leftStrength, float rightStrength)
	{
		VibrationMoment vibrationMoment = default(VibrationMoment);
		vibrationMoment.leftStrength = leftStrength;
		vibrationMoment.rightStrength = rightStrength;
		vibrationMoment.smallMotorStrength = (vibrationMoment.largeMotorStrength = Mathf.Lerp(leftStrength, rightStrength, 0.5f));
		return vibrationMoment;
	}

	// Token: 0x06000277 RID: 631 RVA: 0x00015217 File Offset: 0x00013417
	public static VibrationMoment Max(VibrationMoment a, VibrationMoment b)
	{
		return VibrationMoment.Create(Mathf.Max(a.leftStrength, b.leftStrength), Mathf.Max(a.rightStrength, b.rightStrength));
	}

	// Token: 0x06000278 RID: 632 RVA: 0x00015240 File Offset: 0x00013440
	public override bool Equals(object obj)
	{
		return obj is VibrationMoment && this == (VibrationMoment)obj;
	}

	// Token: 0x06000279 RID: 633 RVA: 0x0001525D File Offset: 0x0001345D
	public bool Equals(VibrationMoment p)
	{
		return this.leftStrength == p.leftStrength && this.rightStrength == p.rightStrength && this.largeMotorStrength == p.largeMotorStrength && this.smallMotorStrength == p.smallMotorStrength;
	}

	// Token: 0x0600027A RID: 634 RVA: 0x00015299 File Offset: 0x00013499
	public override int GetHashCode()
	{
		return 27 * this.leftStrength.GetHashCode() * this.rightStrength.GetHashCode() * this.largeMotorStrength.GetHashCode() * this.smallMotorStrength.GetHashCode();
	}

	// Token: 0x0600027B RID: 635 RVA: 0x000152CD File Offset: 0x000134CD
	public static bool operator ==(VibrationMoment left, VibrationMoment right)
	{
		return left == right || (left != null && right != null && left.Equals(right));
	}

	// Token: 0x0600027C RID: 636 RVA: 0x000152F9 File Offset: 0x000134F9
	public static bool operator !=(VibrationMoment left, VibrationMoment right)
	{
		return !(left == right);
	}

	// Token: 0x0400039E RID: 926
	public float largeMotorStrength;

	// Token: 0x0400039F RID: 927
	public float smallMotorStrength;

	// Token: 0x040003A0 RID: 928
	public float leftStrength;

	// Token: 0x040003A1 RID: 929
	public float rightStrength;
}
