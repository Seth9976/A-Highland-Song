using System;
using System.Runtime.CompilerServices;
using SplineSystem;
using UnityEngine;

// Token: 0x02000185 RID: 389
public class Eagle : MonoSingleton<Eagle>
{
	// Token: 0x170002ED RID: 749
	// (get) Token: 0x06000CC2 RID: 3266 RVA: 0x0006656C File Offset: 0x0006476C
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

	// Token: 0x170002EE RID: 750
	// (get) Token: 0x06000CC3 RID: 3267 RVA: 0x0006658E File Offset: 0x0006478E
	public bool complete
	{
		get
		{
			return this.state == Eagle.State.Completing || this.state == Eagle.State.None;
		}
	}

	// Token: 0x170002EF RID: 751
	// (get) Token: 0x06000CC4 RID: 3268 RVA: 0x000665A4 File Offset: 0x000647A4
	public bool hasSpline
	{
		get
		{
			return this._flySpline != null && this._flySpline.curves != null && this._flySpline.curves.Length != 0;
		}
	}

	// Token: 0x170002F0 RID: 752
	// (get) Token: 0x06000CC5 RID: 3269 RVA: 0x000665CC File Offset: 0x000647CC
	public bool carryingToTarget
	{
		get
		{
			return this.state == Eagle.State.CarryingToTarget;
		}
	}

	// Token: 0x170002F1 RID: 753
	// (get) Token: 0x06000CC6 RID: 3270 RVA: 0x000665D7 File Offset: 0x000647D7
	public float distToDropoff
	{
		get
		{
			return this._flySpline.curves[1].endArcLength - this._arcLengthOnSpline;
		}
	}

	// Token: 0x170002F2 RID: 754
	// (get) Token: 0x06000CC7 RID: 3271 RVA: 0x000665F4 File Offset: 0x000647F4
	public float distFromPickup
	{
		get
		{
			SplineBezierCurve splineBezierCurve = this._flySpline.curves[1];
			return this._arcLengthOnSpline - splineBezierCurve.startArcLength;
		}
	}

	// Token: 0x170002F3 RID: 755
	// (get) Token: 0x06000CC8 RID: 3272 RVA: 0x0006661C File Offset: 0x0006481C
	public Vector3 splinePos
	{
		get
		{
			return this._flySpline.GetPointAtArcLength(this._arcLengthOnSpline);
		}
	}

	// Token: 0x170002F4 RID: 756
	// (get) Token: 0x06000CC9 RID: 3273 RVA: 0x0006662F File Offset: 0x0006482F
	// (set) Token: 0x06000CCA RID: 3274 RVA: 0x00066648 File Offset: 0x00064848
	private float direction
	{
		get
		{
			return Mathf.Sign(base.transform.localScale.x);
		}
		set
		{
			Vector3 localScale = base.transform.localScale;
			localScale.x = Mathf.Abs(localScale.x) * Mathf.Sign(value);
			base.transform.localScale = localScale;
		}
	}

	// Token: 0x06000CCB RID: 3275 RVA: 0x00066688 File Offset: 0x00064888
	public void StartPickupAndFlyTo(string targetPropName)
	{
		this._audioSource.time = 0f;
		this._audioSource.clip = this._eagleCry;
		this._audioSource.Play();
		this._targetProp = Prop.FindNearestByInkName(base.transform.position, targetPropName);
		this.animator.SetAnimation("EagleFlying", FrameAnimator.PosMatch.None);
		this.state = Eagle.State.Catching;
		GameCamera.instance.caughtCameraState.Begin();
		float num = -Mathf.Sign(this._originalPos.x - Runner.instance.transform.position.x);
		Runner.instance.PrepareForEagleCatch(num);
		this.CalculateSpline();
		AudioController.instance.PlaySting(Sting.Night, 1);
	}

	// Token: 0x06000CCC RID: 3276 RVA: 0x00066744 File Offset: 0x00064944
	private void CalculateSpline()
	{
		Vector3 vector = Runner.instance.catchAimBasePos;
		Vector3 vector2 = this._settings.catchOffset;
		vector2.x *= this.direction;
		vector += vector2;
		Vector3 vector3 = this._targetProp.transform.position + 10f * Vector3.up;
		float num = Mathf.Sign(vector3.x - vector.x);
		Vector3 vector4 = new Vector3(100f * num, 50f, 0f);
		Vector3 vector5 = vector3 + vector4;
		this._flySpline = new Spline(new SplineBezierPoint[]
		{
			new SplineBezierPoint(this._originalPos, Eagle.<CalculateSpline>g__FaceDir|21_0(vector.x - this._originalPos.x), this._settings.splineTangentDist, this._settings.splineTangentDist),
			new SplineBezierPoint(vector, Eagle.<CalculateSpline>g__FaceDir|21_0(vector3.x - vector.x), this._settings.splineTangentDist, this._settings.splineTangentDist),
			new SplineBezierPoint(vector3, Eagle.<CalculateSpline>g__FaceDir|21_0(vector3.x - vector.x), this._settings.splineTangentDist, this._settings.splineTangentDist),
			new SplineBezierPoint(vector5, Eagle.<CalculateSpline>g__FaceDir|21_0(vector5.x - vector3.x), this._settings.splineTangentDist, this._settings.splineTangentDist)
		});
	}

	// Token: 0x06000CCD RID: 3277 RVA: 0x000668D1 File Offset: 0x00064AD1
	private void Awake()
	{
		this._originalPos = base.transform.position;
		FrameAnimator animator = this.animator;
		animator.onRootMotion = (FrameAnimator.OnRootMotionDelegate)Delegate.Combine(animator.onRootMotion, new FrameAnimator.OnRootMotionDelegate(this.OnRootMotion));
	}

	// Token: 0x06000CCE RID: 3278 RVA: 0x0006690B File Offset: 0x00064B0B
	private void OnRootMotion(Vector2 motion, bool worldSpace)
	{
		if (!worldSpace)
		{
			motion.x *= this.direction;
		}
		this._rootMotionOffset += motion;
	}

	// Token: 0x06000CCF RID: 3279 RVA: 0x00066938 File Offset: 0x00064B38
	public void Clear()
	{
		Game.instance.RemoveTimeScalar(Game.TimeScalar.EagleCatch);
		this.state = Eagle.State.None;
		base.transform.position = this._originalPos;
		this._rootMotionOffset = Vector3.zero;
		this._flySpline = null;
		this._arcLengthOnSpline = 0f;
		this._catchTime = 0f;
		this._targetProp = null;
		this._speed = 0f;
		this.animator.SetAnimation(null, 0, FrameAnimator.PosMatch.None);
	}

	// Token: 0x06000CD0 RID: 3280 RVA: 0x000669B0 File Offset: 0x00064BB0
	private void Update()
	{
		if (this.state == Eagle.State.None)
		{
			return;
		}
		if (this.state == Eagle.State.Catching)
		{
			this._settings.catchOffset.x *= this.direction;
			float num = this.UpdateFlyOnSpline(1, this._settings.speed, this._settings.catchSpeed);
			if (num <= 0f)
			{
				this.state = Eagle.State.CarryingToTarget;
				this._catchTime = Time.time;
				AudioController.instance.PlayVocalisation(Vocalisation.EagleCatch, 0f);
				return;
			}
			float num2 = 0.5f * (this._speed + this._settings.catchSpeed);
			float num3 = num / num2;
			float catchAnimTimeToGrab = Runner.instance.catchAnimTimeToGrab;
			if (num3 < catchAnimTimeToGrab && !Runner.instance.inEagleCatchAnim)
			{
				AudioController.instance.PlayVocalisation(Vocalisation.EagleImminent, 0f);
				Runner.instance.ReactToEagleCatch();
			}
			FrameAnimation frameAnimation = this.animator.TryGetAnim("EagleCatch");
			if (num3 < frameAnimation.duration * 0.4f && !this.animator.IsAnimation("EagleCatch", null, null, null))
			{
				this.animator.SetAnimation("EagleCatch", FrameAnimator.PosMatch.Mouth);
				InputVibration.Large();
				this._audioSource.time = 0f;
				this._audioSource.clip = this._whoosh;
				this._audioSource.Play();
			}
			if (num3 < this._settings.slowMoTimeToArrival)
			{
				Game.instance.SetTimeScalar(Game.TimeScalar.EagleCatch, this._settings.slowMoScalar);
				return;
			}
		}
		else if (this.state == Eagle.State.CarryingToTarget)
		{
			if (this.animator.IsAnimation("EagleCatch", null, null, null) && this.animator.normalizedTime >= 1f)
			{
				this.animator.SetAnimation("EagleFlying", FrameAnimator.PosMatch.Mouth);
			}
			float num4 = Mathf.InverseLerp(this._catchTime, this._catchTime + this._settings.slowMoRecoverTime, Time.time);
			if (num4 >= 1f)
			{
				Game.instance.RemoveTimeScalar(Game.TimeScalar.EagleCatch);
			}
			else
			{
				float num5 = Mathf.Lerp(this._settings.slowMoScalar, 1f, num4);
				Game.instance.SetTimeScalar(Game.TimeScalar.EagleCatch, num5);
			}
			Level forTransform = Level.GetForTransform(this._targetProp.transform);
			if (this._flySpline.GetTAtArcLength(this._arcLengthOnSpline) > 0.9f && Level.currentIndex != forTransform.levelIdx)
			{
				WorldManager.instance.LoadLevelsFrom(forTransform.levelIdx, null);
			}
			bool flag = this.UpdateFlyOnSpline(2, this._settings.catchSpeed, this._settings.dropOffSpeed) <= 0f;
			Vector3 headTorchPos = this.animator.headTorchPos;
			Runner.instance.transform.position = headTorchPos + new Vector3(this.direction * this._settings.playerHangOffset.x, this._settings.playerHangOffset.y, 0f);
			Runner.instance.direction = this.direction;
			if (this.animator.IsAnimation("EagleFlying", null, null, null))
			{
				float normalizedTime = this.animator.normalizedTime;
				if (this._lastFlyingAnimTime > normalizedTime)
				{
					this._lastFlyingAnimTime -= 1f;
				}
				if (normalizedTime >= this._settings.wingBeatSoundTime && this._lastFlyingAnimTime < this._settings.wingBeatSoundTime)
				{
					this._wingBeatSource.clip = this._wingBeatClips[this._wingBeatIdx % this._wingBeatClips.Length];
					this._wingBeatSource.time = 0f;
					this._wingBeatSource.Play();
					this._wingBeatIdx++;
				}
				this._lastFlyingAnimTime = normalizedTime;
			}
			if (flag)
			{
				Vector3 position = Runner.instance.transform.position;
				position.z = this._targetProp.transform.position.z;
				Runner.instance.transform.position = position;
				this._audioSource.time = 0f;
				this._audioSource.clip = this._eagleCry;
				this._audioSource.Play();
				Runner.instance.Drop();
				this.state = Eagle.State.Completing;
				GameCamera.instance.caughtCameraState.End();
				return;
			}
		}
		else if (this.state == Eagle.State.Completing)
		{
			AudioController.instance.PlayVocalisation(Vocalisation.EagleComplete, 0f);
			float num6 = Mathf.Sign(base.transform.localScale.x);
			this._targetProp.transform.position + 100f * Vector3.up + 100f * num6 * Vector3.right;
			if (this.UpdateFlyOnSpline(3, this._settings.dropOffSpeed, this._settings.speed) <= 0f)
			{
				this.animator.SetAnimation(null, 0, FrameAnimator.PosMatch.None);
				this.state = Eagle.State.None;
			}
		}
	}

	// Token: 0x06000CD1 RID: 3281 RVA: 0x00066E98 File Offset: 0x00065098
	private float UpdateFlyOnSpline(int targetControlPoint, float departSpeed, float arriveSpeed)
	{
		SplineBezierCurve splineBezierCurve = this._flySpline.curves[targetControlPoint - 1];
		float startArcLength = splineBezierCurve.startArcLength;
		float endArcLength = splineBezierCurve.endArcLength;
		if (this._arcLengthOnSpline < startArcLength + this._settings.arrivalRadius)
		{
			float num = Mathf.InverseLerp(startArcLength, startArcLength + this._settings.arrivalRadius, this._arcLengthOnSpline);
			this._speed = Mathf.Lerp(departSpeed, this._settings.speed, num);
		}
		else if (this._arcLengthOnSpline > endArcLength - this._settings.arrivalRadius)
		{
			float num2 = Mathf.InverseLerp(endArcLength - this._settings.arrivalRadius, endArcLength, this._arcLengthOnSpline);
			this._speed = Mathf.Lerp(this._settings.speed, arriveSpeed, num2);
		}
		this._arcLengthOnSpline += this._speed * Time.deltaTime;
		base.transform.position = this._flySpline.GetPointAtArcLength(this._arcLengthOnSpline) + this._rootMotionOffset;
		this.direction = Mathf.Sign(this._flySpline.GetDirectionAtArcLength(this._arcLengthOnSpline).x);
		return endArcLength - this._arcLengthOnSpline;
	}

	// Token: 0x06000CD3 RID: 3283 RVA: 0x00066FC2 File Offset: 0x000651C2
	[CompilerGenerated]
	internal static Quaternion <CalculateSpline>g__FaceDir|21_0(float dir)
	{
		return Quaternion.LookRotation(new Vector3(Mathf.Sign(dir), 0f, 0f), Vector3.up);
	}

	// Token: 0x04000F86 RID: 3974
	public Eagle.State state;

	// Token: 0x04000F87 RID: 3975
	private FrameAnimator _animator;

	// Token: 0x04000F88 RID: 3976
	[SerializeField]
	private float _speed;

	// Token: 0x04000F89 RID: 3977
	private Prop _targetProp;

	// Token: 0x04000F8A RID: 3978
	private Vector3 _originalPos;

	// Token: 0x04000F8B RID: 3979
	private float _catchTime;

	// Token: 0x04000F8C RID: 3980
	private Spline _flySpline;

	// Token: 0x04000F8D RID: 3981
	private float _arcLengthOnSpline;

	// Token: 0x04000F8E RID: 3982
	private Vector3 _rootMotionOffset;

	// Token: 0x04000F8F RID: 3983
	private int _wingBeatIdx;

	// Token: 0x04000F90 RID: 3984
	private float _lastFlyingAnimTime;

	// Token: 0x04000F91 RID: 3985
	[SerializeField]
	private EagleSettings _settings;

	// Token: 0x04000F92 RID: 3986
	[SerializeField]
	private AudioClip _eagleCry;

	// Token: 0x04000F93 RID: 3987
	[SerializeField]
	private AudioClip _whoosh;

	// Token: 0x04000F94 RID: 3988
	[SerializeField]
	private AudioClip[] _wingBeatClips;

	// Token: 0x04000F95 RID: 3989
	[SerializeField]
	private AudioSource _audioSource;

	// Token: 0x04000F96 RID: 3990
	[SerializeField]
	private AudioSource _wingBeatSource;

	// Token: 0x020003A5 RID: 933
	public enum State
	{
		// Token: 0x0400198F RID: 6543
		None,
		// Token: 0x04001990 RID: 6544
		Catching,
		// Token: 0x04001991 RID: 6545
		CarryingToTarget,
		// Token: 0x04001992 RID: 6546
		Completing
	}
}
