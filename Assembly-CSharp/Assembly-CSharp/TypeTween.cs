using System;
using UnityEngine;

// Token: 0x020001BF RID: 447
public abstract class TypeTween<T>
{
	// Token: 0x17000375 RID: 885
	// (get) Token: 0x06000EB2 RID: 3762 RVA: 0x00072DA0 File Offset: 0x00070FA0
	// (set) Token: 0x06000EB3 RID: 3763 RVA: 0x00072DA8 File Offset: 0x00070FA8
	public T currentValue
	{
		get
		{
			return this._currentValue;
		}
		set
		{
			this._currentValue = value;
			this.ChangedCurrentValue();
		}
	}

	// Token: 0x17000376 RID: 886
	// (get) Token: 0x06000EB4 RID: 3764 RVA: 0x00072DB7 File Offset: 0x00070FB7
	// (set) Token: 0x06000EB5 RID: 3765 RVA: 0x00072DBF File Offset: 0x00070FBF
	public AnimationCurve easingCurve
	{
		get
		{
			return this._easingCurve;
		}
		protected set
		{
			this._easingCurve = value;
		}
	}

	// Token: 0x14000012 RID: 18
	// (add) Token: 0x06000EB6 RID: 3766 RVA: 0x00072DC8 File Offset: 0x00070FC8
	// (remove) Token: 0x06000EB7 RID: 3767 RVA: 0x00072E00 File Offset: 0x00071000
	public event TypeTween<T>.OnChangeEvent OnChange;

	// Token: 0x14000013 RID: 19
	// (add) Token: 0x06000EB8 RID: 3768 RVA: 0x00072E38 File Offset: 0x00071038
	// (remove) Token: 0x06000EB9 RID: 3769 RVA: 0x00072E70 File Offset: 0x00071070
	public event TypeTween<T>.OnInterruptEvent OnInterrupt;

	// Token: 0x14000014 RID: 20
	// (add) Token: 0x06000EBA RID: 3770 RVA: 0x00072EA8 File Offset: 0x000710A8
	// (remove) Token: 0x06000EBB RID: 3771 RVA: 0x00072EE0 File Offset: 0x000710E0
	public event TypeTween<T>.OnStopEvent OnStop;

	// Token: 0x14000015 RID: 21
	// (add) Token: 0x06000EBC RID: 3772 RVA: 0x00072F18 File Offset: 0x00071118
	// (remove) Token: 0x06000EBD RID: 3773 RVA: 0x00072F50 File Offset: 0x00071150
	public event TypeTween<T>.OnStartEvent OnStart;

	// Token: 0x14000016 RID: 22
	// (add) Token: 0x06000EBE RID: 3774 RVA: 0x00072F88 File Offset: 0x00071188
	// (remove) Token: 0x06000EBF RID: 3775 RVA: 0x00072FC0 File Offset: 0x000711C0
	public event TypeTween<T>.OnCompleteEvent OnComplete;

	// Token: 0x06000EC0 RID: 3776 RVA: 0x00072FF5 File Offset: 0x000711F5
	public TypeTween()
	{
		this.Init();
	}

	// Token: 0x06000EC1 RID: 3777 RVA: 0x00073004 File Offset: 0x00071204
	public TypeTween(T myCurrentValue)
	{
		this.Init();
		this.currentValue = myCurrentValue;
		this.targetValue = myCurrentValue;
	}

	// Token: 0x06000EC2 RID: 3778 RVA: 0x0007302D File Offset: 0x0007122D
	public TypeTween(T myStartValue, T myTargetValue, float myTweenTime)
	{
		this.Init();
		this.Tween(myStartValue, myTargetValue, myTweenTime);
	}

	// Token: 0x06000EC3 RID: 3779 RVA: 0x00073044 File Offset: 0x00071244
	public TypeTween(T myStartValue, T myTargetValue, float myTweenTime, TypeTween<T>.LerpFunction lerpFunction)
	{
		this.Init();
		this.Tween(myStartValue, myTargetValue, myTweenTime, lerpFunction);
	}

	// Token: 0x06000EC4 RID: 3780 RVA: 0x0007305D File Offset: 0x0007125D
	public TypeTween(T myStartValue, T myTargetValue, float myTweenTime, AnimationCurve _easingCurve)
	{
		this.Init();
		this.Tween(myStartValue, myTargetValue, myTweenTime, _easingCurve);
	}

	// Token: 0x06000EC5 RID: 3781 RVA: 0x00073076 File Offset: 0x00071276
	public TypeTween(T myStartValue, T myTargetValue, float myTweenTime, AnimationCurve _easingCurve, TypeTween<T>.LerpFunction lerpFunction)
	{
		this.Init();
		this.Tween(myStartValue, myTargetValue, myTweenTime, _easingCurve, lerpFunction);
	}

	// Token: 0x06000EC6 RID: 3782 RVA: 0x00073091 File Offset: 0x00071291
	public TypeTween(TweenProperties<T> tweenProperties)
	{
		this.Init();
		this.Tween(tweenProperties);
	}

	// Token: 0x06000EC7 RID: 3783 RVA: 0x000730A6 File Offset: 0x000712A6
	protected virtual void Init()
	{
	}

	// Token: 0x06000EC8 RID: 3784 RVA: 0x000730A8 File Offset: 0x000712A8
	public void Reset()
	{
		this.Reset(default(T));
	}

	// Token: 0x06000EC9 RID: 3785 RVA: 0x000730C4 File Offset: 0x000712C4
	public void Reset(T myCurrentValue)
	{
		this.Stop();
		this.currentValue = myCurrentValue;
	}

	// Token: 0x06000ECA RID: 3786 RVA: 0x000730D3 File Offset: 0x000712D3
	public void Reset(T myStartValue, T myTargetValue, float myTweenTime)
	{
		this.Stop();
		this.Tween(myStartValue, myTargetValue, myTweenTime);
	}

	// Token: 0x06000ECB RID: 3787 RVA: 0x000730E4 File Offset: 0x000712E4
	public void Reset(T myStartValue, T myTargetValue, float myTweenTime, AnimationCurve _easingCurve)
	{
		this.Stop();
		this.Tween(myStartValue, myTargetValue, myTweenTime, _easingCurve);
	}

	// Token: 0x06000ECC RID: 3788 RVA: 0x000730F7 File Offset: 0x000712F7
	public virtual void Tween(T myTargetValue, float myTweenTime)
	{
		this.Tween(this.currentValue, myTargetValue, myTweenTime);
	}

	// Token: 0x06000ECD RID: 3789 RVA: 0x00073107 File Offset: 0x00071307
	public virtual void Tween(T myTargetValue, float myTweenTime, AnimationCurve myLerpCurve)
	{
		this.Tween(this.currentValue, myTargetValue, myTweenTime, myLerpCurve);
	}

	// Token: 0x06000ECE RID: 3790 RVA: 0x00073118 File Offset: 0x00071318
	public virtual void Tween(T myTargetValue, float myTweenTime, TypeTween<T>.LerpFunction lerpFunction)
	{
		this.Tween(this.currentValue, myTargetValue, myTweenTime, lerpFunction);
	}

	// Token: 0x06000ECF RID: 3791 RVA: 0x00073129 File Offset: 0x00071329
	public virtual void Tween(T myTargetValue, float myTweenTime, AnimationCurve myLerpCurve, TypeTween<T>.LerpFunction lerpFunction)
	{
		this.Tween(this.currentValue, myTargetValue, myTweenTime, myLerpCurve, lerpFunction);
	}

	// Token: 0x06000ED0 RID: 3792 RVA: 0x0007313C File Offset: 0x0007133C
	public virtual void Tween(T myStartValue, T myTargetValue, float myTweenTime)
	{
		this.Tween(myStartValue, myTargetValue, myTweenTime, AnimationCurve.Linear(0f, 0f, 1f, 1f));
	}

	// Token: 0x06000ED1 RID: 3793 RVA: 0x00073160 File Offset: 0x00071360
	public virtual void Tween(T myStartValue, T myTargetValue, float myTweenTime, AnimationCurve myLerpCurve)
	{
		this.Tween(myStartValue, myTargetValue, myTweenTime, myLerpCurve, this.lerpFunction);
	}

	// Token: 0x06000ED2 RID: 3794 RVA: 0x00073173 File Offset: 0x00071373
	public virtual void Tween(T myStartValue, T myTargetValue, float myTweenTime, TypeTween<T>.LerpFunction lerpFunction)
	{
		this.Tween(myStartValue, myTargetValue, myTweenTime, AnimationCurve.Linear(0f, 0f, 1f, 1f), lerpFunction);
	}

	// Token: 0x06000ED3 RID: 3795 RVA: 0x0007319C File Offset: 0x0007139C
	public virtual void Tween(TweenProperties<T> tweenProperties)
	{
		if (tweenProperties.setStartValue)
		{
			if (tweenProperties.setEasingCurve)
			{
				this.Tween(tweenProperties.startValue, tweenProperties.targetValue, tweenProperties.tweenTime, tweenProperties.easingCurve, tweenProperties.lerpFunction);
				return;
			}
			this.Tween(tweenProperties.startValue, tweenProperties.targetValue, tweenProperties.tweenTime, tweenProperties.lerpFunction);
			return;
		}
		else
		{
			if (tweenProperties.setEasingCurve)
			{
				this.Tween(tweenProperties.targetValue, tweenProperties.tweenTime, tweenProperties.easingCurve, tweenProperties.lerpFunction);
				return;
			}
			this.Tween(tweenProperties.targetValue, tweenProperties.tweenTime, tweenProperties.lerpFunction);
			return;
		}
	}

	// Token: 0x06000ED4 RID: 3796 RVA: 0x0007323C File Offset: 0x0007143C
	public virtual void Tween(T myStartValue, T myTargetValue, float myTweenTime, AnimationCurve myLerpCurve, TypeTween<T>.LerpFunction lerpFunction)
	{
		if (this.tweening)
		{
			this.Interrupt();
		}
		this.SetEasingCurve(myLerpCurve);
		if (lerpFunction == null)
		{
			this.SetDefaultLerpFunction();
		}
		else
		{
			this.lerpFunction = lerpFunction;
		}
		if (myTweenTime > 0f)
		{
			this.tweenTimer = new Timer(myTweenTime);
			this.tweenTimer.OnComplete += this.TweenComplete;
			this.tweenTimer.Start();
			this.deltaValue = default(T);
			this.currentValue = myStartValue;
			this.startValue = myStartValue;
			this.targetValue = myTargetValue;
			this.tweening = true;
		}
		else
		{
			this.currentValue = myTargetValue;
			this.startValue = myStartValue;
			this.targetValue = myTargetValue;
		}
		this.TweenStart();
		if (!this.tweening)
		{
			this.TweenComplete();
		}
	}

	// Token: 0x06000ED5 RID: 3797 RVA: 0x000732FD File Offset: 0x000714FD
	protected virtual void SetEasingCurve(AnimationCurve easingCurve)
	{
		this.easingCurve = easingCurve;
	}

	// Token: 0x06000ED6 RID: 3798 RVA: 0x00073306 File Offset: 0x00071506
	public virtual T Update()
	{
		return this.Update(Time.deltaTime);
	}

	// Token: 0x06000ED7 RID: 3799 RVA: 0x00073313 File Offset: 0x00071513
	public virtual T Update(float myDeltaTime)
	{
		if (this.tweening)
		{
			this.SetValue(this.GetValueAtNormalizedTime(this.tweenTimer.GetNormalizedTime()));
			this.tweenTimer.Update(myDeltaTime);
		}
		return this.currentValue;
	}

	// Token: 0x06000ED8 RID: 3800 RVA: 0x00073346 File Offset: 0x00071546
	protected virtual void TweenStart()
	{
		if (this.OnStart != null)
		{
			this.OnStart();
		}
	}

	// Token: 0x06000ED9 RID: 3801 RVA: 0x0007335B File Offset: 0x0007155B
	protected virtual void TweenComplete()
	{
		this.SetValue(this.GetValueAtNormalizedTime(1f));
		this.deltaValue = default(T);
		this.Stop();
		if (this.OnComplete != null)
		{
			this.OnComplete();
		}
	}

	// Token: 0x06000EDA RID: 3802 RVA: 0x00073393 File Offset: 0x00071593
	public virtual void Stop()
	{
		this.tweening = false;
		if (this.OnStop != null)
		{
			this.OnStop();
		}
	}

	// Token: 0x06000EDB RID: 3803 RVA: 0x000733AF File Offset: 0x000715AF
	public virtual void Interrupt()
	{
		this.Stop();
		if (this.OnInterrupt != null)
		{
			this.OnInterrupt();
		}
	}

	// Token: 0x06000EDC RID: 3804 RVA: 0x000733CA File Offset: 0x000715CA
	public T GetValueAtTime(float myTime)
	{
		return this.GetValueAtNormalizedTime(myTime / this.tweenTimer.targetTime);
	}

	// Token: 0x06000EDD RID: 3805 RVA: 0x000733DF File Offset: 0x000715DF
	public T GetValueAtNormalizedTime(float myNormalizedTime)
	{
		return this.lerpFunction(this.startValue, this.targetValue, myNormalizedTime);
	}

	// Token: 0x06000EDE RID: 3806 RVA: 0x000733FC File Offset: 0x000715FC
	protected virtual void SetValue(T myValue)
	{
		T currentValue = this.currentValue;
		this.currentValue = myValue;
		this.SetDeltaValue(currentValue, this.currentValue);
	}

	// Token: 0x06000EDF RID: 3807
	protected abstract void SetDeltaValue(T myLastValue, T myCurrentValue);

	// Token: 0x06000EE0 RID: 3808 RVA: 0x00073424 File Offset: 0x00071624
	protected virtual void ChangedCurrentValue()
	{
		if (this.OnChange != null)
		{
			this.OnChange(this.currentValue);
		}
	}

	// Token: 0x06000EE1 RID: 3809
	protected abstract void SetDefaultLerpFunction();

	// Token: 0x06000EE2 RID: 3810 RVA: 0x00073440 File Offset: 0x00071640
	public override string ToString()
	{
		string[] array = new string[10];
		array[0] = "[";
		array[1] = base.GetType().Name;
		array[2] = "] tweening ";
		array[3] = this.tweening.ToString();
		array[4] = ", currentValue ";
		int num = 5;
		T currentValue = this.currentValue;
		array[num] = ((currentValue != null) ? currentValue.ToString() : null);
		array[6] = ", targetValue ";
		int num2 = 7;
		currentValue = this.targetValue;
		array[num2] = ((currentValue != null) ? currentValue.ToString() : null);
		array[8] = ", timer norm time ";
		array[9] = ((this.tweenTimer == null) ? "" : this.tweenTimer.GetNormalizedTime().ToString());
		return string.Concat(array);
	}

	// Token: 0x04001192 RID: 4498
	[SerializeField]
	private T _currentValue;

	// Token: 0x04001193 RID: 4499
	public T deltaValue;

	// Token: 0x04001194 RID: 4500
	public bool tweening;

	// Token: 0x04001195 RID: 4501
	public T targetValue;

	// Token: 0x04001196 RID: 4502
	public T startValue;

	// Token: 0x04001197 RID: 4503
	public Timer tweenTimer;

	// Token: 0x04001198 RID: 4504
	[SerializeField]
	private AnimationCurve _easingCurve;

	// Token: 0x0400119E RID: 4510
	public TypeTween<T>.LerpFunction lerpFunction;

	// Token: 0x020003CE RID: 974
	// (Invoke) Token: 0x0600187C RID: 6268
	public delegate void OnChangeEvent(T currentValue);

	// Token: 0x020003CF RID: 975
	// (Invoke) Token: 0x06001880 RID: 6272
	public delegate void OnInterruptEvent();

	// Token: 0x020003D0 RID: 976
	// (Invoke) Token: 0x06001884 RID: 6276
	public delegate void OnStopEvent();

	// Token: 0x020003D1 RID: 977
	// (Invoke) Token: 0x06001888 RID: 6280
	public delegate void OnStartEvent();

	// Token: 0x020003D2 RID: 978
	// (Invoke) Token: 0x0600188C RID: 6284
	public delegate void OnCompleteEvent();

	// Token: 0x020003D3 RID: 979
	// (Invoke) Token: 0x06001890 RID: 6288
	public delegate T LerpFunction(T start, T end, float lerp);
}
