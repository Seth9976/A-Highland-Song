using System;
using UnityEngine;

// Token: 0x020001BD RID: 445
[Serializable]
public class Timer
{
	// Token: 0x17000372 RID: 882
	// (get) Token: 0x06000E8B RID: 3723 RVA: 0x000726CC File Offset: 0x000708CC
	// (set) Token: 0x06000E8C RID: 3724 RVA: 0x000726D4 File Offset: 0x000708D4
	public float targetTime
	{
		get
		{
			return this._targetTime;
		}
		set
		{
			this._targetTime = value;
			this._targetTimeReciprocal = null;
			if (this.OnSetTargetTime != null)
			{
				this.OnSetTargetTime();
			}
		}
	}

	// Token: 0x17000373 RID: 883
	// (get) Token: 0x06000E8D RID: 3725 RVA: 0x000726FC File Offset: 0x000708FC
	public float remainingTime
	{
		get
		{
			return this.targetTime - this.currentTime;
		}
	}

	// Token: 0x17000374 RID: 884
	// (get) Token: 0x06000E8E RID: 3726 RVA: 0x0007270B File Offset: 0x0007090B
	public bool isComplete
	{
		get
		{
			return this.remainingTime <= 0f;
		}
	}

	// Token: 0x1400000C RID: 12
	// (add) Token: 0x06000E8F RID: 3727 RVA: 0x00072720 File Offset: 0x00070920
	// (remove) Token: 0x06000E90 RID: 3728 RVA: 0x00072758 File Offset: 0x00070958
	public event Action OnStart;

	// Token: 0x1400000D RID: 13
	// (add) Token: 0x06000E91 RID: 3729 RVA: 0x00072790 File Offset: 0x00070990
	// (remove) Token: 0x06000E92 RID: 3730 RVA: 0x000727C8 File Offset: 0x000709C8
	public event Action OnStop;

	// Token: 0x1400000E RID: 14
	// (add) Token: 0x06000E93 RID: 3731 RVA: 0x00072800 File Offset: 0x00070A00
	// (remove) Token: 0x06000E94 RID: 3732 RVA: 0x00072838 File Offset: 0x00070A38
	public event Action OnReset;

	// Token: 0x1400000F RID: 15
	// (add) Token: 0x06000E95 RID: 3733 RVA: 0x00072870 File Offset: 0x00070A70
	// (remove) Token: 0x06000E96 RID: 3734 RVA: 0x000728A8 File Offset: 0x00070AA8
	public event Action OnRepeat;

	// Token: 0x14000010 RID: 16
	// (add) Token: 0x06000E97 RID: 3735 RVA: 0x000728E0 File Offset: 0x00070AE0
	// (remove) Token: 0x06000E98 RID: 3736 RVA: 0x00072918 File Offset: 0x00070B18
	public event Action OnComplete;

	// Token: 0x14000011 RID: 17
	// (add) Token: 0x06000E99 RID: 3737 RVA: 0x00072950 File Offset: 0x00070B50
	// (remove) Token: 0x06000E9A RID: 3738 RVA: 0x00072988 File Offset: 0x00070B88
	public event Action OnSetTargetTime;

	// Token: 0x06000E9B RID: 3739 RVA: 0x000729C0 File Offset: 0x00070BC0
	public static Timer StartNew(float targetTime, Action onComplete = null)
	{
		Timer timer = new Timer(targetTime);
		if (onComplete != null)
		{
			timer.OnComplete += onComplete;
		}
		timer.Start();
		return timer;
	}

	// Token: 0x06000E9C RID: 3740 RVA: 0x000729E5 File Offset: 0x00070BE5
	public Timer()
	{
		this.useTargetTime = false;
	}

	// Token: 0x06000E9D RID: 3741 RVA: 0x00072A09 File Offset: 0x00070C09
	public Timer(float myTargetTime)
	{
		this.Set(myTargetTime, 1);
	}

	// Token: 0x06000E9E RID: 3742 RVA: 0x00072A2E File Offset: 0x00070C2E
	public Timer(float myTargetTime, int myTargetRepeats)
	{
		this.Set(myTargetTime, myTargetRepeats);
	}

	// Token: 0x06000E9F RID: 3743 RVA: 0x00072A53 File Offset: 0x00070C53
	public Timer(float myTargetTime, bool myRepeatForever)
	{
		this.Set(myTargetTime, myRepeatForever);
	}

	// Token: 0x06000EA0 RID: 3744 RVA: 0x00072A78 File Offset: 0x00070C78
	public virtual void Set(float myTargetTime, int myTargetRepeats = 1)
	{
		this.targetTime = myTargetTime;
		this.targetRepeats = myTargetRepeats;
		this.useTargetTime = this.targetTime >= 0f && this.targetRepeats > 0;
	}

	// Token: 0x06000EA1 RID: 3745 RVA: 0x00072AA7 File Offset: 0x00070CA7
	public virtual void Set(float myTargetTime, bool myRepeatForever)
	{
		this.targetTime = myTargetTime;
		this.repeatForever = myRepeatForever;
		this.useTargetTime = this.targetTime > 0f;
	}

	// Token: 0x06000EA2 RID: 3746 RVA: 0x00072ACA File Offset: 0x00070CCA
	public virtual void Start()
	{
		if (this.state == Timer.State.Playing)
		{
			return;
		}
		this.state = Timer.State.Playing;
		if (this.OnStart != null)
		{
			this.OnStart();
		}
	}

	// Token: 0x06000EA3 RID: 3747 RVA: 0x00072AF0 File Offset: 0x00070CF0
	public virtual void ResetAndStart()
	{
		this.Reset();
		this.Start();
	}

	// Token: 0x06000EA4 RID: 3748 RVA: 0x00072AFE File Offset: 0x00070CFE
	public virtual void ResetAndStart(float myTargetTime)
	{
		this.Reset();
		this.Set(myTargetTime, 1);
		this.Start();
	}

	// Token: 0x06000EA5 RID: 3749 RVA: 0x00072B14 File Offset: 0x00070D14
	public virtual void Stop()
	{
		if (this.state == Timer.State.Stopped)
		{
			return;
		}
		this.state = Timer.State.Stopped;
		if (this.OnStop != null)
		{
			this.OnStop();
		}
	}

	// Token: 0x06000EA6 RID: 3750 RVA: 0x00072B39 File Offset: 0x00070D39
	public virtual void StopAndReset()
	{
		this.Stop();
		this.Reset();
	}

	// Token: 0x06000EA7 RID: 3751 RVA: 0x00072B47 File Offset: 0x00070D47
	public virtual void Reset()
	{
		this.currentTime = 0f;
		this.currentRepeats = 0;
		if (this.OnReset != null)
		{
			this.OnReset();
		}
	}

	// Token: 0x06000EA8 RID: 3752 RVA: 0x00072B70 File Offset: 0x00070D70
	public void CopyFrom(Timer otherTimer)
	{
		this.state = otherTimer.state;
		this.currentTime = otherTimer.currentTime;
		this.useTargetTime = otherTimer.useTargetTime;
		this.stopOnReachingTarget = otherTimer.stopOnReachingTarget;
		this.targetTime = otherTimer.targetTime;
		this.currentRepeats = otherTimer.currentRepeats;
		this.targetRepeats = otherTimer.targetRepeats;
		this.repeatForever = otherTimer.repeatForever;
	}

	// Token: 0x06000EA9 RID: 3753 RVA: 0x00072BDD File Offset: 0x00070DDD
	public virtual void Update()
	{
		this.Update(Time.deltaTime);
	}

	// Token: 0x06000EAA RID: 3754 RVA: 0x00072BEA File Offset: 0x00070DEA
	public virtual void Update(float _deltaTime)
	{
		if (this.state == Timer.State.Playing)
		{
			this.UpdateTimer(_deltaTime);
		}
	}

	// Token: 0x06000EAB RID: 3755 RVA: 0x00072BFC File Offset: 0x00070DFC
	public virtual float GetNormalizedTime()
	{
		if (this.targetTime == 0f)
		{
			return 1f;
		}
		if (this._targetTimeReciprocal == null)
		{
			this._targetTimeReciprocal = new float?(1f / this.targetTime);
		}
		return Mathf.Clamp01(this._targetTimeReciprocal.Value * this.currentTime);
	}

	// Token: 0x06000EAC RID: 3756 RVA: 0x00072C58 File Offset: 0x00070E58
	protected virtual void UpdateTimer(float _deltaTime)
	{
		this.currentTime += _deltaTime;
		if (this.useTargetTime && this.currentTime > this.targetTime)
		{
			this.ReachTargetTime();
		}
	}

	// Token: 0x06000EAD RID: 3757 RVA: 0x00072C84 File Offset: 0x00070E84
	protected virtual void ReachTargetTime()
	{
		this.currentRepeats++;
		if (this.currentRepeats < this.targetRepeats || this.repeatForever)
		{
			this.currentTime = 0f;
			if (this.OnRepeat != null)
			{
				this.OnRepeat();
				return;
			}
		}
		else
		{
			if (this.stopOnReachingTarget)
			{
				this.Stop();
			}
			if (this.OnComplete != null)
			{
				this.OnComplete();
			}
		}
	}

	// Token: 0x06000EAE RID: 3758 RVA: 0x00072CF8 File Offset: 0x00070EF8
	public override string ToString()
	{
		return string.Format("{0}: State: {1}, Time: {2}, Repeats: {3}", new object[]
		{
			base.GetType(),
			this.state,
			this.currentTime,
			this.currentRepeats
		});
	}

	// Token: 0x0400117C RID: 4476
	public Timer.State state;

	// Token: 0x0400117D RID: 4477
	public float currentTime;

	// Token: 0x0400117E RID: 4478
	public bool useTargetTime = true;

	// Token: 0x0400117F RID: 4479
	public bool stopOnReachingTarget = true;

	// Token: 0x04001180 RID: 4480
	[SerializeField]
	[Disable]
	private float _targetTime;

	// Token: 0x04001181 RID: 4481
	public int currentRepeats;

	// Token: 0x04001182 RID: 4482
	public int targetRepeats = 1;

	// Token: 0x04001183 RID: 4483
	public bool repeatForever;

	// Token: 0x04001184 RID: 4484
	private float? _targetTimeReciprocal;

	// Token: 0x020003CD RID: 973
	public enum State
	{
		// Token: 0x04001A3A RID: 6714
		Stopped,
		// Token: 0x04001A3B RID: 6715
		Playing
	}
}
