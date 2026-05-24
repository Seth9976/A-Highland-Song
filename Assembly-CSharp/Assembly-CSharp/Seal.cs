using System;
using UnityEngine;

// Token: 0x020001A3 RID: 419
public class Seal : MonoBehaviour
{
	// Token: 0x1700034F RID: 847
	// (get) Token: 0x06000DDC RID: 3548 RVA: 0x0006D219 File Offset: 0x0006B419
	private FrameAnimator animator
	{
		get
		{
			if (this._animator == null)
			{
				this._animator = base.GetComponentInChildren<FrameAnimator>();
			}
			return this._animator;
		}
	}

	// Token: 0x17000350 RID: 848
	// (get) Token: 0x06000DDD RID: 3549 RVA: 0x0006D23B File Offset: 0x0006B43B
	private SpriteRenderer spriteRenderer
	{
		get
		{
			if (this._spriteRenderer == null)
			{
				this._spriteRenderer = base.GetComponent<SpriteRenderer>();
			}
			return this._spriteRenderer;
		}
	}

	// Token: 0x06000DDE RID: 3550 RVA: 0x0006D260 File Offset: 0x0006B460
	private void OnEnable()
	{
		this._inkWantsHide = false;
		this._appearTime = 0f;
		this._turnedTime = 0f;
		this._animDir = 1;
		this._basePos = base.transform.position;
		this._splashParticles.Stop();
		this._splashParticles.Clear();
		this.SetState(Seal.State.Hidden);
		Narrative.onEventDidFire += this.OnEventFired;
	}

	// Token: 0x06000DDF RID: 3551 RVA: 0x0006D2D0 File Offset: 0x0006B4D0
	private void OnDisable()
	{
		this.SetState(Seal.State.None);
		this._splashParticles.Stop();
		Narrative.onEventDidFire -= this.OnEventFired;
	}

	// Token: 0x06000DE0 RID: 3552 RVA: 0x0006D2F8 File Offset: 0x0006B4F8
	private void Update()
	{
		this.UpdateState();
		this.stateTimer += Time.deltaTime;
		if (this.state != Seal.State.Hidden && this.state != Seal.State.None && Time.frameCount % this.rippleMax == this.rippleIdx % this.rippleMax)
		{
			MonoSingleton<WaterRippleManager>.instance.CreateRipple(base.transform.position + this._settings.rippleOffsetZ * Vector3.forward);
		}
		if ((this.state == Seal.State.Diving && this.stateTimer > 0.5f * this._settings.diveDuration) || this.animator.IsAnimation(this._settings.hideAnim, null, null, null))
		{
			this._splashParticles.Play();
			return;
		}
		this._splashParticles.Stop();
	}

	// Token: 0x06000DE1 RID: 3553 RVA: 0x0006D3CC File Offset: 0x0006B5CC
	private void SetState(Seal.State newState)
	{
		Seal.State state = this.state;
		this.animator.speed = 1f;
		if (state == Seal.State.Hidden)
		{
			this.spriteRenderer.color = Color.Lerp(this._settings.colorRange1, this._settings.colorRange2, Random.value);
		}
		if (newState == Seal.State.None)
		{
			this.animator.SetAnimation(null, 0, FrameAnimator.PosMatch.None);
		}
		else if (newState == Seal.State.Hidden)
		{
			if (state == Seal.State.None)
			{
				this.animator.SetAnimation(null, 0, FrameAnimator.PosMatch.None);
			}
			else
			{
				this.animator.SetAnimation(this._settings.hideAnim, 0, FrameAnimator.PosMatch.None);
			}
			this._appearIsDive = !this._inkWantsHide && Random.value < this._settings.diveVsBobProbability;
			this._appearTime = this._settings.hiddenTime.Random();
		}
		else if (newState == Seal.State.Turning)
		{
			this.animator.SetAnimation(this._settings.turnAnim, 0, FrameAnimator.PosMatch.None);
			if (state == Seal.State.Turned)
			{
				this._animDir = -1;
				this.animator.normalizedTime = 1f;
				this.animator.speed = -1f;
			}
			else
			{
				this._animDir = 1;
				this.animator.normalizedTime = 0f;
				this.animator.speed = 1f;
			}
		}
		else if (newState == Seal.State.Turned)
		{
			this.animator.speed = 1f;
			this.animator.SetAnimation(this._settings.turnedAnim, 0, FrameAnimator.PosMatch.None);
			this._turnedTime = this._settings.turnedTime.Random();
		}
		else if (newState == Seal.State.Bobbing)
		{
			if (state == Seal.State.Hidden)
			{
				this.animator.SetAnimationWithTransition(this._settings.appearAnim, this._settings.bobbingAnim, 0, false, false, FrameAnimator.PosMatch.None);
			}
			else
			{
				this.animator.SetAnimation(this._settings.bobbingAnim, 0, FrameAnimator.PosMatch.None);
			}
			if (state == Seal.State.Turning)
			{
				this.animator.normalizedTime = 1f;
				this.animator.speed = -1f;
				this._animDir = -1;
			}
			else
			{
				this._animDir = 1;
			}
		}
		else if (newState == Seal.State.Diving)
		{
			this._animDir = ((Random.value < 0.5f) ? (-1) : 1);
			this._diveOffset = new Vector3(this._settings.diveXOffsetRange.Random(), this._settings.diveYOffsetRange.Random(), 0f);
			this._diveRotationOffset = this._settings.driveRotationOffsetRange.Random();
			this.animator.SetAnimation(this._settings.divingAnim, 0, FrameAnimator.PosMatch.None);
		}
		this.state = newState;
		this.stateTimer = 0f;
		if (newState != Seal.State.Hidden && newState != Seal.State.None)
		{
			MonoSingleton<WaterRippleManager>.instance.CreateRipple(base.transform.position + this._settings.rippleOffsetZ * Vector3.forward);
		}
		if (newState == Seal.State.Diving)
		{
			this.UpdateState();
		}
	}

	// Token: 0x06000DE2 RID: 3554 RVA: 0x0006D6AC File Offset: 0x0006B8AC
	private void UpdateState()
	{
		if (this.state == Seal.State.Hidden)
		{
			if (this.animator.IsAnimation(this._settings.hideAnim, null, null, null) && this.animator.normalizedTime == 1f)
			{
				this.animator.SetAnimation(null, 0, FrameAnimator.PosMatch.None);
			}
			if (this.stateTimer > this._appearTime && !this._inkWantsHide)
			{
				if (this._appearIsDive)
				{
					this.SetState(Seal.State.Diving);
					return;
				}
				this.SetState(Seal.State.Bobbing);
				return;
			}
		}
		else if (this.state == Seal.State.Bobbing)
		{
			if (this.animator.IsAnimation(this._settings.bobbingAnim, null, null, null))
			{
				if (this.animator.normalizedTime == 1f && this._animDir == 1)
				{
					this.animator.speed = -1f;
					this._animDir = -1;
					if (Random.value < this._settings.turnProbability)
					{
						this.SetState(Seal.State.Turning);
						return;
					}
				}
				else if (this.animator.normalizedTime == 0f && this._animDir == -1)
				{
					this.animator.speed = 1f;
					this._animDir = 1;
					if (Random.value < this._settings.hideProbability || this._inkWantsHide)
					{
						this.SetState(Seal.State.Hidden);
						return;
					}
				}
			}
		}
		else if (this.state == Seal.State.Turning)
		{
			if (this._animDir == 1 && this.animator.normalizedTime == 1f)
			{
				this.SetState(Seal.State.Turned);
				return;
			}
			if (this._animDir == -1 && this.animator.normalizedTime == 0f)
			{
				this.SetState(Seal.State.Bobbing);
				return;
			}
		}
		else if (this.state == Seal.State.Turned)
		{
			if (this.stateTimer > this._turnedTime)
			{
				this.SetState(Seal.State.Turning);
				return;
			}
		}
		else if (this.state == Seal.State.Diving)
		{
			Transform transform = base.transform;
			transform.position = new Vector3(this._basePos.x + (float)this._animDir * this._settings.diveAnimXCurve.Evaluate(this.stateTimer / this._settings.diveDuration) + this._diveOffset.x, this._basePos.y + this._settings.diveAnimYCurve.Evaluate(this.stateTimer / this._settings.diveDuration) + this._diveOffset.y, this._basePos.z);
			transform.rotation = Quaternion.Euler(0f, 0f, (float)this._animDir * (this._diveRotationOffset + this._settings.diveAnimRotationCurve.Evaluate(this.stateTimer / this._settings.diveDuration)));
			transform.localScale = new Vector3((float)this._animDir, 1f, 1f);
			if (this.stateTimer > this._settings.diveDuration)
			{
				transform.position = this._basePos;
				transform.rotation = Quaternion.identity;
				transform.localScale = Vector3.one;
				this.SetState(Seal.State.Hidden);
			}
		}
	}

	// Token: 0x06000DE3 RID: 3555 RVA: 0x0006D9C1 File Offset: 0x0006BBC1
	private void OnEventFired(string eventName)
	{
		if (eventName == "HideSeals")
		{
			this._inkWantsHide = true;
		}
	}

	// Token: 0x0400109B RID: 4251
	public int rippleIdx;

	// Token: 0x0400109C RID: 4252
	public int rippleMax = 8;

	// Token: 0x0400109D RID: 4253
	public Seal.State state;

	// Token: 0x0400109E RID: 4254
	public float stateTimer;

	// Token: 0x0400109F RID: 4255
	private FrameAnimator _animator;

	// Token: 0x040010A0 RID: 4256
	private SpriteRenderer _spriteRenderer;

	// Token: 0x040010A1 RID: 4257
	private float _appearTime;

	// Token: 0x040010A2 RID: 4258
	private bool _appearIsDive;

	// Token: 0x040010A3 RID: 4259
	private float _turnedTime;

	// Token: 0x040010A4 RID: 4260
	private int _animDir;

	// Token: 0x040010A5 RID: 4261
	private bool _inkWantsHide;

	// Token: 0x040010A6 RID: 4262
	private Vector3 _basePos;

	// Token: 0x040010A7 RID: 4263
	private Vector3 _diveOffset;

	// Token: 0x040010A8 RID: 4264
	private float _diveRotationOffset;

	// Token: 0x040010A9 RID: 4265
	[SerializeField]
	private SealSettings _settings;

	// Token: 0x040010AA RID: 4266
	[SerializeField]
	private ParticleSystem _splashParticles;

	// Token: 0x020003BA RID: 954
	public enum State
	{
		// Token: 0x040019F4 RID: 6644
		None,
		// Token: 0x040019F5 RID: 6645
		Hidden,
		// Token: 0x040019F6 RID: 6646
		Bobbing,
		// Token: 0x040019F7 RID: 6647
		Turning,
		// Token: 0x040019F8 RID: 6648
		Turned,
		// Token: 0x040019F9 RID: 6649
		Diving
	}
}
