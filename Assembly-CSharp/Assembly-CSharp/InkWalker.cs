using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200018E RID: 398
[RequireComponent(typeof(GuidComponent))]
public class InkWalker : MonoInstancer<InkWalker>
{
	// Token: 0x17000300 RID: 768
	// (get) Token: 0x06000CF7 RID: 3319 RVA: 0x00067C68 File Offset: 0x00065E68
	public string guid
	{
		get
		{
			if (string.IsNullOrEmpty(this._guid))
			{
				GuidComponent component = base.GetComponent<GuidComponent>();
				this._guid = component.GetGuid().ToString();
			}
			return this._guid;
		}
	}

	// Token: 0x17000301 RID: 769
	// (get) Token: 0x06000CF8 RID: 3320 RVA: 0x00067CA9 File Offset: 0x00065EA9
	public static bool canSave
	{
		get
		{
			return !MonoInstancer<InkWalker>.all.Exists((InkWalker c) => c._walking);
		}
	}

	// Token: 0x17000302 RID: 770
	// (get) Token: 0x06000CF9 RID: 3321 RVA: 0x00067CD7 File Offset: 0x00065ED7
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

	// Token: 0x17000303 RID: 771
	// (get) Token: 0x06000CFA RID: 3322 RVA: 0x00067CF9 File Offset: 0x00065EF9
	// (set) Token: 0x06000CFB RID: 3323 RVA: 0x00067D01 File Offset: 0x00065F01
	private float alpha
	{
		get
		{
			return this._alpha;
		}
		set
		{
			if (this._alpha != value)
			{
				this._alpha = value;
				this.animator.spriteRenderer.color = this._originalColor.WithAlpha(this._alpha);
			}
		}
	}

	// Token: 0x17000304 RID: 772
	// (get) Token: 0x06000CFC RID: 3324 RVA: 0x00067D34 File Offset: 0x00065F34
	// (set) Token: 0x06000CFD RID: 3325 RVA: 0x00067D3C File Offset: 0x00065F3C
	private float rotation
	{
		get
		{
			return this._rotation;
		}
		set
		{
			if (this._rotation != value)
			{
				this._rotation = value;
				base.transform.localRotation = Quaternion.Euler(0f, 0f, this._rotation);
			}
		}
	}

	// Token: 0x06000CFE RID: 3326 RVA: 0x00067D6E File Offset: 0x00065F6E
	private void Awake()
	{
		this.SetupOriginalPropertiesIfNecessary();
		this._facePlayer = false;
	}

	// Token: 0x06000CFF RID: 3327 RVA: 0x00067D80 File Offset: 0x00065F80
	private void SetupOriginalPropertiesIfNecessary()
	{
		if (this._setupOriginalProperties)
		{
			return;
		}
		this._originalPos = base.transform.position;
		this._originalDirection = Mathf.Sign(base.transform.localScale.x);
		this._originalColor = this.animator.spriteRenderer.color;
		this._setupOriginalProperties = true;
	}

	// Token: 0x06000D00 RID: 3328 RVA: 0x00067DDF File Offset: 0x00065FDF
	protected override void OnEnable()
	{
		base.OnEnable();
		Narrative.onEventDidFire += this.OnInkEventDidFire;
		this.animator.SetAnimation(this.idleAnim, 0, FrameAnimator.PosMatch.None);
	}

	// Token: 0x06000D01 RID: 3329 RVA: 0x00067E0B File Offset: 0x0006600B
	protected override void OnDisable()
	{
		base.OnDisable();
		Narrative.onEventDidFire -= this.OnInkEventDidFire;
	}

	// Token: 0x06000D02 RID: 3330 RVA: 0x00067E24 File Offset: 0x00066024
	public static void ResetAll()
	{
		InkWalker[] array = Object.FindObjectsOfType<InkWalker>(true);
		for (int i = 0; i < array.Length; i++)
		{
			array[i].ResetState();
		}
	}

	// Token: 0x06000D03 RID: 3331 RVA: 0x00067E50 File Offset: 0x00066050
	private void ResetState()
	{
		this.SetupOriginalPropertiesIfNecessary();
		base.transform.position = this._originalPos;
		this.FaceDir(this._originalDirection);
		this.animator.spriteRenderer.color = this._originalColor;
		this._walking = false;
		this._waypointIdx = -1;
		this._facePlayer = false;
		this._currentTrackPos = default(TrackPosition);
		this._transitioningOut = false;
	}

	// Token: 0x06000D04 RID: 3332 RVA: 0x00067EC0 File Offset: 0x000660C0
	public void FaceDir(float dir)
	{
		if (dir == 0f)
		{
			return;
		}
		Transform transform = base.transform;
		Vector3 localScale = transform.localScale;
		localScale.x = Mathf.Abs(localScale.x) * Mathf.Sign(dir);
		transform.localScale = localScale;
	}

	// Token: 0x06000D05 RID: 3333 RVA: 0x00067F04 File Offset: 0x00066104
	private void Update()
	{
		if (!this.animator.gameObject.activeSelf)
		{
			return;
		}
		if (this._facePlayer)
		{
			this.FaceDir(Runner.instance.position.x - base.transform.position.x);
		}
		if (this._walking)
		{
			InkWalker.Waypoint waypoint = this.waypoints[this._waypointIdx];
			Vector3 position = waypoint.target.position;
			FrameAnimation frameAnimation = waypoint.anim ?? this.walkAnim;
			if (waypoint.transitionIn)
			{
				this.animator.SetAnimationWithTransition(waypoint.transitionIn, frameAnimation, 0, false, false, FrameAnimator.PosMatch.None);
			}
			else
			{
				this.animator.SetAnimation(frameAnimation, 0, FrameAnimator.PosMatch.None);
			}
			float num = position.x - base.transform.position.x;
			this.FaceDir(num);
			bool flag = false;
			if (waypoint.moveType == InkWalker.MoveType.Air)
			{
				base.transform.position = Vector3.MoveTowards(base.transform.position, position, this.walkSpeed * Time.deltaTime);
				this._currentTrackPos.slope = null;
				flag = Vector3.Distance(base.transform.position, position) < 0.1f;
			}
			else if (waypoint.moveType == InkWalker.MoveType.Slopes)
			{
				if (this._currentTrackPos.slope == null)
				{
					this._currentTrackPos.slope = Raycast.FindBestNearbySlopeSample(Level.current, base.transform.position, false, 3f).slope;
					this._currentTrackPos.x = base.transform.position.x;
				}
				float num2 = Mathf.Sign(num) * this.walkSpeed * Time.deltaTime;
				num2 = Mathf.Clamp(num2, -Mathf.Abs(num), Mathf.Abs(num));
				Simulate.FindResult findResult = Simulate.FindGroundPositionAtDistance(this._currentTrackPos, num2, Simulate.FindOptions.standardSimulate, Runner.instance.settings);
				base.transform.position = findResult.sample.point3d;
				this._currentTrackPos.slope = findResult.sample.slope;
				this._currentTrackPos.x = findResult.sample.point.x;
				flag = Mathf.Abs(this._currentTrackPos.x - position.x) < 0.5f;
			}
			else if (waypoint.moveType == InkWalker.MoveType.Teleport)
			{
				base.transform.position = position;
				this._currentTrackPos.slope = null;
				flag = true;
			}
			else if (waypoint.moveType == InkWalker.MoveType.TransitionOut)
			{
				flag = !this.animator.IsAnimation(waypoint.transitionIn, null, null, null);
				if (!flag)
				{
					this.alpha = 1f - this.animator.normalizedTime;
				}
			}
			if (!this._transitioningOut)
			{
				this.alpha = Mathf.MoveTowards(this.alpha, 1f, Time.deltaTime / 0.5f);
			}
			if (flag)
			{
				this._walking = false;
				this.animator.SetAnimation(this.idleAnim, 0, FrameAnimator.PosMatch.None);
				if (waypoint.turnToFace == InkWalker.TurnToFace.Left)
				{
					this.FaceDir(-1f);
				}
				else if (waypoint.turnToFace == InkWalker.TurnToFace.Right)
				{
					this.FaceDir(1f);
				}
				else if (waypoint.turnToFace == InkWalker.TurnToFace.FacePlayer)
				{
					this._facePlayer = true;
				}
				Narrative.instance.EndEvent(waypoint.inkEventName);
				this._waypointIdx = -1;
			}
		}
		if (this.rotateToSlope > 0f && this.alpha > 0f)
		{
			float num3 = 0f;
			if (this._currentTrackPos.slope != null)
			{
				num3 = this._currentTrackPos.slope.SampleAt(this._currentTrackPos.x, false).angle;
			}
			float num4 = Mathf.LerpAngle(0f, num3, this.rotateToSlope);
			this.rotation = Mathf.LerpAngle(this.rotation, num4, 0.2f);
			if (Mathf.DeltaAngle(this.rotation, num4) < 1f)
			{
				this.rotation = num4;
			}
		}
	}

	// Token: 0x06000D06 RID: 3334 RVA: 0x000682F8 File Offset: 0x000664F8
	private void OnInkEventDidFire(string eventName)
	{
		int num = this.waypoints.FindIndex((InkWalker.Waypoint w) => w.inkEventName == eventName);
		if (num == -1)
		{
			return;
		}
		this._waypointIdx = num;
		this._walking = true;
		this._facePlayer = false;
		this._transitioningOut = this.waypoints[this._waypointIdx].moveType == InkWalker.MoveType.TransitionOut;
	}

	// Token: 0x04000FCA RID: 4042
	public List<InkWalker.Waypoint> waypoints;

	// Token: 0x04000FCB RID: 4043
	public FrameAnimation idleAnim;

	// Token: 0x04000FCC RID: 4044
	public FrameAnimation walkAnim;

	// Token: 0x04000FCD RID: 4045
	public float walkSpeed = 4f;

	// Token: 0x04000FCE RID: 4046
	[Range(0f, 1f)]
	public float rotateToSlope;

	// Token: 0x04000FCF RID: 4047
	private string _guid;

	// Token: 0x04000FD0 RID: 4048
	private FrameAnimator _animator;

	// Token: 0x04000FD1 RID: 4049
	private float _alpha = 1f;

	// Token: 0x04000FD2 RID: 4050
	private float _rotation;

	// Token: 0x04000FD3 RID: 4051
	private bool _walking;

	// Token: 0x04000FD4 RID: 4052
	private int _waypointIdx = -1;

	// Token: 0x04000FD5 RID: 4053
	private TrackPosition _currentTrackPos;

	// Token: 0x04000FD6 RID: 4054
	private bool _setupOriginalProperties;

	// Token: 0x04000FD7 RID: 4055
	private Vector3 _originalPos;

	// Token: 0x04000FD8 RID: 4056
	private float _originalDirection;

	// Token: 0x04000FD9 RID: 4057
	private Color _originalColor;

	// Token: 0x04000FDA RID: 4058
	private bool _facePlayer;

	// Token: 0x04000FDB RID: 4059
	private bool _transitioningOut;

	// Token: 0x020003A6 RID: 934
	public enum TurnToFace
	{
		// Token: 0x04001994 RID: 6548
		None,
		// Token: 0x04001995 RID: 6549
		Left,
		// Token: 0x04001996 RID: 6550
		Right,
		// Token: 0x04001997 RID: 6551
		FacePlayer
	}

	// Token: 0x020003A7 RID: 935
	public enum MoveType
	{
		// Token: 0x04001999 RID: 6553
		Slopes,
		// Token: 0x0400199A RID: 6554
		Air,
		// Token: 0x0400199B RID: 6555
		Teleport,
		// Token: 0x0400199C RID: 6556
		TransitionOut
	}

	// Token: 0x020003A8 RID: 936
	[Serializable]
	public struct Waypoint
	{
		// Token: 0x0400199D RID: 6557
		public string inkEventName;

		// Token: 0x0400199E RID: 6558
		public Transform target;

		// Token: 0x0400199F RID: 6559
		public InkWalker.TurnToFace turnToFace;

		// Token: 0x040019A0 RID: 6560
		public InkWalker.MoveType moveType;

		// Token: 0x040019A1 RID: 6561
		public FrameAnimation transitionIn;

		// Token: 0x040019A2 RID: 6562
		public FrameAnimation anim;
	}
}
