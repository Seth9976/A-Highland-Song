using System;

// Token: 0x020001CD RID: 461
public abstract class SAnimatedProperty
{
	// Token: 0x06000F74 RID: 3956
	public abstract void Start();

	// Token: 0x06000F75 RID: 3957
	public abstract void Remove();

	// Token: 0x06000F76 RID: 3958
	public abstract void Animate(float lerpValue);

	// Token: 0x040011E6 RID: 4582
	public float delay;

	// Token: 0x040011E7 RID: 4583
	public float duration;
}
