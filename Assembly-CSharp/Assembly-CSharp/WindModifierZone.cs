using System;
using UnityEngine;

// Token: 0x02000166 RID: 358
public class WindModifierZone : MonoInstancer<WindModifierZone>
{
	// Token: 0x170002CA RID: 714
	// (get) Token: 0x06000C09 RID: 3081 RVA: 0x00060421 File Offset: 0x0005E621
	public Bounds bounds
	{
		get
		{
			return new Bounds(base.transform.position, base.transform.localScale);
		}
	}

	// Token: 0x06000C0A RID: 3082 RVA: 0x0006043E File Offset: 0x0005E63E
	public float GetWindVelocity()
	{
		if (this.mode == WindModifierZone.Mode.Constant)
		{
			return this.constantWindVelocity;
		}
		return 0f;
	}

	// Token: 0x04000E49 RID: 3657
	public int layer;

	// Token: 0x04000E4A RID: 3658
	public WindModifierZone.Mode mode;

	// Token: 0x04000E4B RID: 3659
	[SerializeField]
	[Range(-1f, 1f)]
	private float constantWindVelocity;

	// Token: 0x02000399 RID: 921
	public enum Mode
	{
		// Token: 0x04001967 RID: 6503
		Constant
	}
}
