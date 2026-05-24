using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020001D1 RID: 465
public class SLayoutAnimation
{
	// Token: 0x170003DA RID: 986
	// (get) Token: 0x0600101A RID: 4122 RVA: 0x00078A07 File Offset: 0x00076C07
	public float duration
	{
		get
		{
			return this._duration;
		}
	}

	// Token: 0x170003DB RID: 987
	// (get) Token: 0x0600101B RID: 4123 RVA: 0x00078A0F File Offset: 0x00076C0F
	public float delay
	{
		get
		{
			return this._delay;
		}
	}

	// Token: 0x0600101C RID: 4124 RVA: 0x00078A18 File Offset: 0x00076C18
	public SLayoutAnimation(float duration, float delay, AnimationCurve customCurve, Action animAction, Action nonAnimatedAction, SLayout owner)
	{
		this._maxDuration = duration;
		this._duration = duration;
		this._maxDelay = delay;
		this._delay = delay;
		this._animAction = animAction;
		this._nonAnimatedAction = nonAnimatedAction;
		this._customCurve = customCurve;
		this._owner = owner;
		this.time = 0f;
	}

	// Token: 0x0600101D RID: 4125 RVA: 0x00078A78 File Offset: 0x00076C78
	public void Start()
	{
		this.time = 0f;
		if (SLayoutAnimation._animationsBeingDefined == null)
		{
			SLayoutAnimation._animationsBeingDefined = new List<SLayoutAnimation>();
		}
		SLayoutAnimation._animationsBeingDefined.Add(this);
		bool flag = this._delay + this._duration <= 0f;
		if (this._animAction != null)
		{
			this._properties = new List<SAnimatedProperty>();
			this._animAction();
			if (!flag)
			{
				foreach (SAnimatedProperty sanimatedProperty in this._properties)
				{
					sanimatedProperty.Start();
				}
			}
		}
		SLayoutAnimation._animationsBeingDefined.RemoveAt(SLayoutAnimation._animationsBeingDefined.Count - 1);
		if (flag)
		{
			this.Done();
		}
	}

	// Token: 0x0600101E RID: 4126 RVA: 0x00078B48 File Offset: 0x00076D48
	public SLayoutAnimation Then(Action action)
	{
		return this.ThenAfter(0f, action);
	}

	// Token: 0x0600101F RID: 4127 RVA: 0x00078B56 File Offset: 0x00076D56
	public SLayoutAnimation ThenAfter(float delay, Action action)
	{
		return this.ThenAnimateInternal(0f, delay, null, null, action);
	}

	// Token: 0x06001020 RID: 4128 RVA: 0x00078B67 File Offset: 0x00076D67
	public SLayoutAnimation ThenAnimate(float duration, Action animAction)
	{
		return this.ThenAnimate(duration, 0f, animAction);
	}

	// Token: 0x06001021 RID: 4129 RVA: 0x00078B76 File Offset: 0x00076D76
	public SLayoutAnimation ThenAnimate(float duration, float delay, Action animAction)
	{
		return this.ThenAnimateInternal(duration, delay, null, animAction, null);
	}

	// Token: 0x06001022 RID: 4130 RVA: 0x00078B83 File Offset: 0x00076D83
	public SLayoutAnimation ThenAnimate(float duration, float delay, AnimationCurve curve, Action animAction)
	{
		return this.ThenAnimateInternal(duration, delay, curve, animAction, null);
	}

	// Token: 0x06001023 RID: 4131 RVA: 0x00078B94 File Offset: 0x00076D94
	public SLayoutAnimation ThenAnimateCustom(float duration, Action<float> customAnimAction)
	{
		return this.ThenAnimateInternal(duration, 0f, null, delegate
		{
			SLayout.Animatable(customAnimAction);
		}, null);
	}

	// Token: 0x06001024 RID: 4132 RVA: 0x00078BC8 File Offset: 0x00076DC8
	public SLayoutAnimation ThenAnimateCustom(float duration, float delay, Action<float> customAnimAction)
	{
		return this.ThenAnimateInternal(duration, delay, null, delegate
		{
			SLayout.Animatable(customAnimAction);
		}, null);
	}

	// Token: 0x06001025 RID: 4133 RVA: 0x00078BF8 File Offset: 0x00076DF8
	private SLayoutAnimation ThenAnimateInternal(float duration, float delay, AnimationCurve customCurve, Action animAction, Action nonAnimatedAction)
	{
		SLayoutAnimation slayoutAnimation = new SLayoutAnimation(duration, delay, customCurve, animAction, nonAnimatedAction, this._owner);
		if (this.isComplete)
		{
			SLayoutAnimator.instance.StartAnimation(slayoutAnimation);
		}
		else
		{
			this._chainedAnim = slayoutAnimation;
		}
		return slayoutAnimation;
	}

	// Token: 0x170003DC RID: 988
	// (get) Token: 0x06001026 RID: 4134 RVA: 0x00078C35 File Offset: 0x00076E35
	public bool canAnimate
	{
		get
		{
			return !(this.owner == null) && !this.owner.Equals(null);
		}
	}

	// Token: 0x06001027 RID: 4135 RVA: 0x00078C56 File Offset: 0x00076E56
	public static SLayoutAnimation AnimationUnderDefinition()
	{
		if (SLayoutAnimation._preventingAnim)
		{
			return null;
		}
		if (SLayoutAnimation._animationsBeingDefined != null && SLayoutAnimation._animationsBeingDefined.Count > 0)
		{
			return SLayoutAnimation._animationsBeingDefined[SLayoutAnimation._animationsBeingDefined.Count - 1];
		}
		return null;
	}

	// Token: 0x06001028 RID: 4136 RVA: 0x00078C90 File Offset: 0x00076E90
	public void SetupPropertyAnim<T>(SLayoutProperty<T> layoutProperty)
	{
		SAnimatedProperty<T> sanimatedProperty = layoutProperty.animatedProperty;
		if (sanimatedProperty != null)
		{
			SLayoutAnimation animation = sanimatedProperty.animation;
			if (animation != this)
			{
				animation.RemovePropertyAnim(sanimatedProperty);
				sanimatedProperty = null;
			}
		}
		if (sanimatedProperty == null)
		{
			sanimatedProperty = SAnimatedProperty<T>.Create(this._duration, this._delay, layoutProperty, this);
			this._properties.Add(sanimatedProperty);
			sanimatedProperty.start = layoutProperty.getter();
		}
	}

	// Token: 0x06001029 RID: 4137 RVA: 0x00078CEF File Offset: 0x00076EEF
	public void Cancel()
	{
		this.RemoveAnimFromAllProperties();
		this._nonAnimatedAction = null;
		this._chainedAnim = null;
	}

	// Token: 0x0600102A RID: 4138 RVA: 0x00078D08 File Offset: 0x00076F08
	private void RemoveAnimFromAllProperties()
	{
		if (this._properties == null)
		{
			return;
		}
		foreach (SAnimatedProperty sanimatedProperty in this._properties)
		{
			sanimatedProperty.Remove();
		}
		this._properties.Clear();
	}

	// Token: 0x0600102B RID: 4139 RVA: 0x00078D6C File Offset: 0x00076F6C
	private void RemovePropertyAnim(SAnimatedProperty animProperty)
	{
		animProperty.Remove();
		if (this._properties != null)
		{
			this._properties.Remove(animProperty);
		}
	}

	// Token: 0x170003DD RID: 989
	// (get) Token: 0x0600102C RID: 4140 RVA: 0x00078D89 File Offset: 0x00076F89
	public bool isComplete
	{
		get
		{
			return this._completed;
		}
	}

	// Token: 0x170003DE RID: 990
	// (get) Token: 0x0600102D RID: 4141 RVA: 0x00078D91 File Offset: 0x00076F91
	public SLayout owner
	{
		get
		{
			return this._owner;
		}
	}

	// Token: 0x0600102E RID: 4142 RVA: 0x00078D99 File Offset: 0x00076F99
	public void AddDelay(float extraDelay)
	{
		this._delay += extraDelay;
		this._maxDelay = Mathf.Max(this._delay, this._maxDelay);
	}

	// Token: 0x0600102F RID: 4143 RVA: 0x00078DC0 File Offset: 0x00076FC0
	public void AddDuration(float extraDuration)
	{
		this._duration += extraDuration;
		this._maxDuration = Mathf.Max(this._duration, this._maxDuration);
	}

	// Token: 0x06001030 RID: 4144 RVA: 0x00078DE7 File Offset: 0x00076FE7
	public void AddCustomAnim(Action<float> customAnim)
	{
		this._properties.Add(new SAnimatedCustomProperty(customAnim, this._duration, this._delay));
	}

	// Token: 0x170003DF RID: 991
	// (get) Token: 0x06001031 RID: 4145 RVA: 0x00078E06 File Offset: 0x00077006
	private bool timeIsUp
	{
		get
		{
			return this.time >= this._maxDelay + this._maxDuration;
		}
	}

	// Token: 0x06001032 RID: 4146 RVA: 0x00078E20 File Offset: 0x00077020
	public void Update()
	{
		if (Time.frameCount <= 1)
		{
			return;
		}
		this.time += this.owner.timeScale * Time.unscaledDeltaTime;
		if (this.isComplete)
		{
			return;
		}
		if (this._properties != null)
		{
			for (int i = 0; i < this._properties.Count; i++)
			{
				SAnimatedProperty sanimatedProperty = this._properties[i];
				if (this.time > sanimatedProperty.delay)
				{
					float num = Mathf.Clamp01((this.time - sanimatedProperty.delay) / sanimatedProperty.duration);
					if (this._customCurve != null)
					{
						num = this._customCurve.Evaluate(num);
					}
					else
					{
						num = Mathf.SmoothStep(0f, 1f, num);
					}
					sanimatedProperty.Animate(num);
				}
			}
		}
		if (this.timeIsUp)
		{
			this.Done();
		}
	}

	// Token: 0x06001033 RID: 4147 RVA: 0x00078EF8 File Offset: 0x000770F8
	public void CompleteImmediate()
	{
		if (this._properties != null)
		{
			foreach (SAnimatedProperty sanimatedProperty in this._properties)
			{
				sanimatedProperty.Animate(1f);
			}
		}
		this.Done();
	}

	// Token: 0x06001034 RID: 4148 RVA: 0x00078F5C File Offset: 0x0007715C
	public static void StartPreventAnimation()
	{
		SLayoutAnimation._preventingAnim = true;
	}

	// Token: 0x06001035 RID: 4149 RVA: 0x00078F64 File Offset: 0x00077164
	public static void EndPreventAnimation()
	{
		SLayoutAnimation._preventingAnim = false;
	}

	// Token: 0x06001036 RID: 4150 RVA: 0x00078F6C File Offset: 0x0007716C
	private void Done()
	{
		this._completed = true;
		this.RemoveAnimFromAllProperties();
		if (this._nonAnimatedAction != null)
		{
			this._nonAnimatedAction();
		}
		if (this._chainedAnim != null)
		{
			SLayoutAnimation chainedAnim = this._chainedAnim;
			this._chainedAnim = null;
			SLayoutAnimator.instance.StartAnimation(chainedAnim);
		}
	}

	// Token: 0x04001202 RID: 4610
	private List<SAnimatedProperty> _properties;

	// Token: 0x04001203 RID: 4611
	public float time;

	// Token: 0x04001204 RID: 4612
	private float _duration;

	// Token: 0x04001205 RID: 4613
	private float _delay;

	// Token: 0x04001206 RID: 4614
	private float _maxDuration;

	// Token: 0x04001207 RID: 4615
	private float _maxDelay;

	// Token: 0x04001208 RID: 4616
	private AnimationCurve _customCurve;

	// Token: 0x04001209 RID: 4617
	private Action _animAction;

	// Token: 0x0400120A RID: 4618
	private Action _nonAnimatedAction;

	// Token: 0x0400120B RID: 4619
	private SLayoutAnimation _chainedAnim;

	// Token: 0x0400120C RID: 4620
	private bool _completed;

	// Token: 0x0400120D RID: 4621
	private SLayout _owner;

	// Token: 0x0400120E RID: 4622
	private static bool _preventingAnim;

	// Token: 0x0400120F RID: 4623
	private static List<SLayoutAnimation> _animationsBeingDefined;
}
