using System;
using System.Collections.Generic;

// Token: 0x020001CE RID: 462
public class SAnimatedProperty<T> : SAnimatedProperty
{
	// Token: 0x06000F78 RID: 3960 RVA: 0x0007701B File Offset: 0x0007521B
	public override void Start()
	{
		this.end = this.property.getter();
		this.property.setter(this.start);
	}

	// Token: 0x06000F79 RID: 3961 RVA: 0x00077049 File Offset: 0x00075249
	public override void Remove()
	{
		this.property.animatedProperty = null;
		this.property = null;
		this.animation = null;
		this.start = default(T);
		this.end = default(T);
		SAnimatedProperty<T>._reusePool.Push(this);
	}

	// Token: 0x06000F7A RID: 3962 RVA: 0x00077088 File Offset: 0x00075288
	public override void Animate(float lerpValue)
	{
		this.property.setter(this.property.Lerp(this.start, this.end, lerpValue));
	}

	// Token: 0x06000F7B RID: 3963 RVA: 0x000770B4 File Offset: 0x000752B4
	public static SAnimatedProperty<T> Create(float duration, float delay, SLayoutProperty<T> layoutProperty, SLayoutAnimation anim)
	{
		SAnimatedProperty<T> sanimatedProperty;
		if (SAnimatedProperty<T>._reusePool.Count > 0)
		{
			sanimatedProperty = SAnimatedProperty<T>._reusePool.Pop();
		}
		else
		{
			sanimatedProperty = new SAnimatedProperty<T>();
		}
		sanimatedProperty.duration = duration;
		sanimatedProperty.delay = delay;
		sanimatedProperty.animation = anim;
		sanimatedProperty.property = layoutProperty;
		layoutProperty.animatedProperty = sanimatedProperty;
		return sanimatedProperty;
	}

	// Token: 0x040011E8 RID: 4584
	public SLayoutProperty<T> property;

	// Token: 0x040011E9 RID: 4585
	public SLayoutAnimation animation;

	// Token: 0x040011EA RID: 4586
	public T start;

	// Token: 0x040011EB RID: 4587
	public T end;

	// Token: 0x040011EC RID: 4588
	private static Stack<SAnimatedProperty<T>> _reusePool = new Stack<SAnimatedProperty<T>>();
}
