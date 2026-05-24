using System;

// Token: 0x020001CF RID: 463
public class SAnimatedCustomProperty : SAnimatedProperty
{
	// Token: 0x06000F7E RID: 3966 RVA: 0x0007711B File Offset: 0x0007531B
	public SAnimatedCustomProperty(Action<float> customAnim, float duration, float delay)
	{
		this._customAnim = customAnim;
		this.duration = duration;
		this.delay = delay;
	}

	// Token: 0x06000F7F RID: 3967 RVA: 0x00077138 File Offset: 0x00075338
	public override void Start()
	{
	}

	// Token: 0x06000F80 RID: 3968 RVA: 0x0007713A File Offset: 0x0007533A
	public override void Remove()
	{
	}

	// Token: 0x06000F81 RID: 3969 RVA: 0x0007713C File Offset: 0x0007533C
	public override void Animate(float lerpValue)
	{
		this._customAnim(lerpValue);
	}

	// Token: 0x040011ED RID: 4589
	private Action<float> _customAnim;
}
