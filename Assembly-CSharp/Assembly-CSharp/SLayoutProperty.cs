using System;

// Token: 0x020001D4 RID: 468
public abstract class SLayoutProperty<T>
{
	// Token: 0x170003E3 RID: 995
	// (get) Token: 0x06001045 RID: 4165 RVA: 0x00079302 File Offset: 0x00077502
	// (set) Token: 0x06001046 RID: 4166 RVA: 0x00079310 File Offset: 0x00077510
	public T value
	{
		get
		{
			return this.getter();
		}
		set
		{
			SLayoutAnimation slayoutAnimation = SLayoutAnimation.AnimationUnderDefinition();
			if (slayoutAnimation != null)
			{
				slayoutAnimation.SetupPropertyAnim<T>(this);
				this.animatedProperty.end = value;
			}
			this.setter(value);
		}
	}

	// Token: 0x06001047 RID: 4167
	public abstract T Lerp(T v0, T v1, float t);

	// Token: 0x04001215 RID: 4629
	public Func<T> getter;

	// Token: 0x04001216 RID: 4630
	public Action<T> setter;

	// Token: 0x04001217 RID: 4631
	public SAnimatedProperty<T> animatedProperty;
}
