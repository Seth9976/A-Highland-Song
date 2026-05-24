using System;
using UnityEngine;

// Token: 0x0200017B RID: 379
[RequireComponent(typeof(GuidComponent))]
public class Creature : MonoBehaviour
{
	// Token: 0x170002E0 RID: 736
	// (get) Token: 0x06000C87 RID: 3207 RVA: 0x00064086 File Offset: 0x00062286
	// (set) Token: 0x06000C88 RID: 3208 RVA: 0x00064098 File Offset: 0x00062298
	public Vector2 position
	{
		get
		{
			return base.transform.position;
		}
		set
		{
			Vector3 position = base.transform.position;
			position.x = value.x;
			position.y = value.y;
			base.transform.position = position;
		}
	}

	// Token: 0x170002E1 RID: 737
	// (get) Token: 0x06000C89 RID: 3209 RVA: 0x000640D7 File Offset: 0x000622D7
	// (set) Token: 0x06000C8A RID: 3210 RVA: 0x000640DF File Offset: 0x000622DF
	public float alpha
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
				this.RefreshAlpha();
			}
		}
	}

	// Token: 0x170002E2 RID: 738
	// (get) Token: 0x06000C8B RID: 3211 RVA: 0x000640F7 File Offset: 0x000622F7
	public string currentZoneName
	{
		get
		{
			if (!(this._zone != null))
			{
				return "NONE";
			}
			return this._zone.name;
		}
	}

	// Token: 0x170002E3 RID: 739
	// (get) Token: 0x06000C8C RID: 3212 RVA: 0x00064118 File Offset: 0x00062318
	private GuidComponent guidComponent
	{
		get
		{
			if (this._guidComponent == null)
			{
				this._guidComponent = base.GetComponent<GuidComponent>();
			}
			return this._guidComponent;
		}
	}

	// Token: 0x170002E4 RID: 740
	// (get) Token: 0x06000C8D RID: 3213 RVA: 0x0006413C File Offset: 0x0006233C
	private string guid
	{
		get
		{
			if (this._guid == null)
			{
				this._guid = this.guidComponent.GetGuid().ToString();
			}
			return this._guid;
		}
	}

	// Token: 0x170002E5 RID: 741
	// (get) Token: 0x06000C8E RID: 3214 RVA: 0x00064176 File Offset: 0x00062376
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

	// Token: 0x170002E6 RID: 742
	// (get) Token: 0x06000C8F RID: 3215 RVA: 0x00064198 File Offset: 0x00062398
	// (set) Token: 0x06000C90 RID: 3216 RVA: 0x000641A0 File Offset: 0x000623A0
	public Creature.State state
	{
		get
		{
			return this._state;
		}
		set
		{
			if (this._state == value)
			{
				return;
			}
			this.RunStateFunc(this._state, false, true);
			this._prevPrevState = this._prevState;
			this._prevState = this._state;
			this._state = value;
			this._stateTimer = 0f;
			this.RunStateFunc(this._state, true, false);
		}
	}

	// Token: 0x06000C91 RID: 3217 RVA: 0x000641FD File Offset: 0x000623FD
	private Creature.State ConsumeNextState()
	{
		Creature.State nextState = this._nextState;
		this._nextState = Creature.State.None;
		return nextState;
	}

	// Token: 0x06000C92 RID: 3218 RVA: 0x0006420C File Offset: 0x0006240C
	private void EnterDestinationZone()
	{
		this._zone = this._zone.exit.destinationZone;
		this._exiting = false;
		if (this._zone != null)
		{
			this.state = Creature.State.Enter;
			return;
		}
		this.state = Creature.State.None;
		this.alpha = 0f;
		this.TryRunInkIfNearby("GONE_FOR_GOOD");
	}

	// Token: 0x06000C93 RID: 3219 RVA: 0x0006426C File Offset: 0x0006246C
	private void Awake()
	{
		this.state = Creature.State.None;
		this.animator.SetAnimation(null, 0, FrameAnimator.PosMatch.None);
		this.alpha = (this.targetAlpha = 0f);
		this._hasRunGrazeKnot = false;
	}

	// Token: 0x06000C94 RID: 3220 RVA: 0x000642AC File Offset: 0x000624AC
	public void Begin()
	{
		this._zone = this.startZone;
		this.DropOnSlope(false);
		this.alpha = (this.targetAlpha = 1f);
		this.state = this._zone.state;
		this._hasRunGrazeKnot = false;
		if (this.shouldSaveState && this.guidComponent != null)
		{
			this.lastValidSaveState.name = base.name;
			this.lastValidSaveState.guid = this.guid;
		}
	}

	// Token: 0x06000C95 RID: 3221 RVA: 0x00064330 File Offset: 0x00062530
	public void StopAndHide()
	{
		if (this.state == Creature.State.None)
		{
			return;
		}
		this.animator.SetAnimation(null, 0, FrameAnimator.PosMatch.None);
		this.alpha = (this.targetAlpha = 0f);
		this.state = Creature.State.None;
		this._zone = null;
		this._hasRunGrazeKnot = false;
		this._currentChunk = null;
		this._currentSlope = null;
	}

	// Token: 0x06000C96 RID: 3222 RVA: 0x0006438C File Offset: 0x0006258C
	public void ManualUpdate(float dt)
	{
		this.UpdateCurrentState(dt);
		this.UpdateBehaviour(dt);
		this.alpha = Mathf.MoveTowards(this.alpha, this.targetAlpha, dt);
		float num = this.direction;
		if (this.animator.inSecondHalfOfReversal)
		{
			num *= -1f;
		}
		base.transform.localScale = new Vector3(num, 1f, 1f);
		float num2 = 0f;
		if (this.state == Creature.State.Jump)
		{
			float num3 = Mathf.Clamp01(this._stateTimer / this._jumpDuration);
			this._rotation = this._jumpBaseAngle + -this.direction * this.settings.jumpAngleOffset.Lerp(num3);
		}
		else if (!this._stateControllingRotation)
		{
			if (this._currentSlope != null)
			{
				num2 = this.settings.slopeAngleFollowScalar * this._currentSlope.SampleAt(this.position.x, false).angle;
			}
			this._rotation = Mathf.MoveTowardsAngle(this._rotation, num2, 120f * Time.deltaTime);
		}
		base.transform.rotation = Quaternion.Euler(0f, 0f, this._rotation);
		if (this.shouldSaveState && (this.state == Creature.State.Graze || this.state == Creature.State.Alert || this.state == Creature.State.MusicRun || (this.state == Creature.State.Enter && this._hasWalkRunTarget) || (this.state == Creature.State.Exit && this._hasWalkRunTarget)) && this.shouldSaveState)
		{
			this.lastValidSaveState = new SaveState.CreatureState
			{
				name = this.lastValidSaveState.name,
				guid = this.lastValidSaveState.guid,
				pos = base.transform.position,
				dir = this.direction,
				zoneName = ((this._zone == null) ? "NONE" : this._zone.name),
				state = this.state,
				walkRunTarget = (this._hasWalkRunTarget ? this._walkRunTarget : 0f),
				hasRunGrazeKnot = this._hasRunGrazeKnot,
				exiting = this._exiting
			};
		}
		Chunk chunk = ((this._currentSlope != null) ? this._currentSlope.chunk : null);
		if (chunk != this._currentChunk)
		{
			if (this._currentChunk != null)
			{
				Chunk currentChunk = this._currentChunk;
				currentChunk.onRecycle = (Action<Chunk>)Delegate.Remove(currentChunk.onRecycle, new Action<Chunk>(this.OnCurrentChunkRecycled));
			}
			this._currentChunk = chunk;
			if (this._currentChunk != null)
			{
				if (!this._currentChunk.gameObject.activeSelf)
				{
					this.OnCurrentChunkRecycled(this._currentChunk);
					return;
				}
				Chunk currentChunk2 = this._currentChunk;
				currentChunk2.onRecycle = (Action<Chunk>)Delegate.Combine(currentChunk2.onRecycle, new Action<Chunk>(this.OnCurrentChunkRecycled));
			}
		}
	}

	// Token: 0x06000C97 RID: 3223 RVA: 0x00064694 File Offset: 0x00062894
	public void OnCurrentChunkRecycled(Chunk chunk)
	{
		if (chunk != this._currentChunk)
		{
			Debug.LogWarning("Got a Chunk recycle notification although we weren't actually on that chunk (anymore)");
			return;
		}
		chunk.onRecycle = (Action<Chunk>)Delegate.Remove(chunk.onRecycle, new Action<Chunk>(this.OnCurrentChunkRecycled));
		this.StopAndHide();
	}

	// Token: 0x06000C98 RID: 3224 RVA: 0x000646E2 File Offset: 0x000628E2
	private void RefreshAlpha()
	{
		this.animator.spriteRenderer.color = Color.white.WithAlpha(this._alpha);
	}

	// Token: 0x06000C99 RID: 3225 RVA: 0x00064704 File Offset: 0x00062904
	private void UpdateCurrentState(float dt)
	{
		this._dt = dt;
		this._stateTimer += this._dt;
		this.RunStateFunc(this.state, false, false);
	}

	// Token: 0x06000C9A RID: 3226 RVA: 0x00064730 File Offset: 0x00062930
	public void UpdateHat()
	{
		bool flag = Game.isChristmas && this.settings.christmasHat != null;
		if (flag && this._hat == null)
		{
			GameObject gameObject = new GameObject("hat", new Type[] { typeof(SpriteRenderer) });
			this._hat = gameObject.GetComponent<SpriteRenderer>();
			this._hat.transform.SetParent(this.animator.transform, false);
			this._hat.sprite = this.settings.christmasHat;
			this._hat.sharedMaterial = this.animator.spriteRenderer.sharedMaterial;
		}
		else if (!flag && this._hat != null)
		{
			Object.Destroy(this._hat.gameObject);
			this._hat = null;
		}
		if (this._hat == null)
		{
			return;
		}
		this._hat.transform.localRotation = Quaternion.Euler(0f, 0f, -this.animator.headTorchAngle + this.settings.hatAngleOffset);
		Vector3 vector = this.animator.headTorchPos;
		vector.z -= 0.1f;
		vector += this.settings.hatOffset.x * this.direction * this._hat.transform.right + this.settings.hatOffset.y * this._hat.transform.up;
		this._hat.transform.position = vector;
	}

	// Token: 0x06000C9B RID: 3227 RVA: 0x000648E0 File Offset: 0x00062AE0
	private void RunStateFunc(Creature.State stateToRun, bool enter, bool exit)
	{
		switch (stateToRun)
		{
		case Creature.State.Graze:
			this.State_Graze(enter, exit);
			return;
		case Creature.State.Alert:
			this.State_Alert(enter, exit);
			return;
		case Creature.State.MusicRun:
			this.State_MusicRun(enter, exit);
			return;
		case Creature.State.Jump:
			this.State_Jump(enter, exit);
			return;
		case Creature.State.Enter:
			this.State_Enter(enter, exit);
			return;
		case Creature.State.Exit:
			this.State_Exit(enter, exit);
			return;
		case Creature.State.BetweenZones:
			this.State_BetweenZones(enter, exit);
			return;
		case Creature.State.EmergencyExit:
			this.State_EmergencyExit(enter, exit);
			return;
		default:
			return;
		}
	}

	// Token: 0x06000C9C RID: 3228 RVA: 0x00064960 File Offset: 0x00062B60
	public void ApplySaveState(SaveState.CreatureState saveState)
	{
		this.lastValidSaveState = saveState;
		base.transform.position = saveState.pos;
		this.direction = (float)((saveState.dir < 0f) ? (-1) : 1);
		if (saveState.walkRunTarget != 0f)
		{
			this._walkRunTarget = saveState.walkRunTarget;
			this._hasWalkRunTarget = true;
		}
		else
		{
			this._hasWalkRunTarget = false;
		}
		this._hasRunGrazeKnot = saveState.hasRunGrazeKnot;
		if (saveState.zoneName == "NONE")
		{
			this._zone = null;
		}
		else
		{
			CreatureZone destinationZone = this.startZone;
			while (destinationZone != null && destinationZone.name != saveState.zoneName)
			{
				destinationZone = destinationZone.exit.destinationZone;
			}
			if (destinationZone == null)
			{
				Debug.LogError("Zone with name " + saveState.zoneName + " couldn't be found for creature " + base.name, this);
			}
			else
			{
				this._zone = destinationZone;
			}
		}
		this.state = saveState.state;
		this.targetAlpha = (this.alpha = (float)((this.state == Creature.State.None || this._zone == null) ? 0 : 1));
		this._exiting = saveState.exiting;
		this.DropOnSlope(true);
	}

	// Token: 0x06000C9D RID: 3229 RVA: 0x00064A9C File Offset: 0x00062C9C
	private bool IsOrWillBeState(Creature.State s1, Creature.State s2 = Creature.State.None, Creature.State s3 = Creature.State.None, Creature.State s4 = Creature.State.None)
	{
		return this.state == s1 || this._nextState == s1 || (s2 != Creature.State.None && (this.state == s2 || this._nextState == s2)) || (s3 != Creature.State.None && (this.state == s3 || this._nextState == s3)) || (s4 != Creature.State.None && (this.state == s4 || this._nextState == s4));
	}

	// Token: 0x06000C9E RID: 3230 RVA: 0x00064B08 File Offset: 0x00062D08
	private void UpdateBehaviour(float dt)
	{
		if (this._zone == null)
		{
			return;
		}
		if (this.IsOrWillBeState(Creature.State.MusicRun, Creature.State.None, Creature.State.None, Creature.State.None) && this._exiting)
		{
			if (this._zone.exit.destinationZone != null && this._zone.exit.destinationZone.Contains(this.position))
			{
				this.EnterDestinationZone();
				return;
			}
		}
		else if (!this.IsOrWillBeState(Creature.State.Enter, Creature.State.Exit, Creature.State.BetweenZones, Creature.State.EmergencyExit) && this.state != Creature.State.None)
		{
			if (this._zone.exit.condition == CreatureExitCondition.Always)
			{
				this._exiting = true;
				this.state = Creature.State.Exit;
				return;
			}
			if (this._zone.exit.condition == CreatureExitCondition.PlayerTooClose)
			{
				Vector3 physicalPosition3d = Runner.instance.physicalPosition3d;
				if (Range.Centered(base.transform.position.z, 2.5f).Contains(physicalPosition3d.z) && Vector2.Distance(physicalPosition3d, base.transform.position) < this.settings.playerTooCloseProximity)
				{
					this.TryRunInkIfNearby("STARTLED");
					this._exiting = true;
					this.state = Creature.State.Exit;
					return;
				}
			}
		}
	}

	// Token: 0x06000C9F RID: 3231 RVA: 0x00064C44 File Offset: 0x00062E44
	private bool TryRunInkIfNearby(string stateName)
	{
		if (string.IsNullOrWhiteSpace(this.inkName))
		{
			return false;
		}
		Vector3 position = Runner.instance.transform.position;
		if (Mathf.Abs(base.transform.position.z - position.z) > 6f)
		{
			return false;
		}
		float num = Vector2.Distance(position, base.transform.position);
		if (num > 30f)
		{
			return false;
		}
		Range range = new Range(0.1f, 0.9f);
		Vector3 vector = GameCamera.instance.camera.WorldToViewportPoint(base.transform.position);
		if ((!range.Contains(vector.x) || !range.Contains(vector.y) || vector.z < 0f) && num > 15f)
		{
			return false;
		}
		if (Narrative.instance.knotQueueCount > 0)
		{
			return false;
		}
		Narrative.instance.RunCreatureKnot(this.inkName, stateName);
		return true;
	}

	// Token: 0x06000CA0 RID: 3232 RVA: 0x00064D44 File Offset: 0x00062F44
	private Creature.WalkRunResult UpdateWalkRunToTarget(bool run)
	{
		if (!this._hasWalkRunTarget)
		{
			Debug.LogError("Creature " + base.name + " called UpdateWalk but _hasWalkTarget == false", this);
			return Creature.WalkRunResult.Complete;
		}
		if (this.animator.HasAnimation("GrazeToIdle") || this.animator.HasAnimation("Idle"))
		{
			if (this.animator.IsAnimation("Graze", null, null, null))
			{
				this.animator.SetAnimationWithTransition("GrazeToIdle", "Idle", 0, false, false, FrameAnimator.PosMatch.None);
				return Creature.WalkRunResult.NotComplete;
			}
			if (this.animator.IsAnimation("GrazeToIdle", null, null, null))
			{
				return Creature.WalkRunResult.NotComplete;
			}
		}
		else if (this.animator.HasAnimation("GrazeToWalk"))
		{
			if (this.animator.IsAnimation("Graze", null, null, null))
			{
				this.animator.SetAnimationWithTransition("GrazeToWalk", "Walk", 0, false, false, FrameAnimator.PosMatch.None);
				return Creature.WalkRunResult.NotComplete;
			}
			if (this.animator.IsAnimation("GrazeToWalk", null, null, null))
			{
				return Creature.WalkRunResult.NotComplete;
			}
		}
		string text = ((run && this.animator.HasAnimation("Run")) ? "Run" : "Walk");
		if (this.animator.HasAnimation(text))
		{
			this.animator.SetAnimation(text, FrameAnimator.PosMatch.None);
		}
		this.direction = Mathf.Sign(this._walkRunTarget - this.position.x);
		float num = (run ? this.settings.runSpeed : this.settings.walkSpeed) * Time.deltaTime;
		Simulate.FindResult findResult = Simulate.FindGroundPositionAtDistance(new TrackPosition
		{
			slope = this._currentSlope,
			x = this.position.x
		}, this.direction * num, Simulate.FindOptions.standardPredict, this.runnerSettings);
		if (!findResult.foundRequestedX)
		{
			return Creature.WalkRunResult.Blocked;
		}
		Vector2 position = this.position;
		this.position = findResult.sample.point;
		this._currentSlope = findResult.sample.slope;
		if (Mathf.Sign(this._walkRunTarget - position.x) != Mathf.Sign(this._walkRunTarget - this.position.x))
		{
			this._hasWalkRunTarget = false;
			return Creature.WalkRunResult.Complete;
		}
		return Creature.WalkRunResult.NotComplete;
	}

	// Token: 0x06000CA1 RID: 3233 RVA: 0x00064F60 File Offset: 0x00063160
	private void State_Graze(bool start, bool end)
	{
		if (start)
		{
			if (this.animator.HasAnimation("IdleToGraze"))
			{
				this.animator.SetAnimationWithTransition("IdleToGraze", "Graze", 0, false, false, FrameAnimator.PosMatch.None);
			}
			else if (this.animator.IsAnimation("Walk", null, null, null) && this.animator.HasAnimation("WalkToGraze"))
			{
				this.animator.SetAnimationWithTransition("WalkToGraze", "Graze", 0, false, false, FrameAnimator.PosMatch.None);
			}
			this._scheduledStateChangeTime = this.settings.grazeEatTime.Random();
			this._hasRunGrazeKnot = false;
			return;
		}
		if (end)
		{
			if (this.animator.HasAnimation("Idle"))
			{
				if (this.animator.IsAnimation("Graze", null, null, null) && this.animator.HasAnimation("GrazeToIdle"))
				{
					this.animator.SetAnimationWithTransition("GrazeToIdle", "Idle", 0, false, false, FrameAnimator.PosMatch.None);
					return;
				}
				this.animator.SetAnimation("Idle", FrameAnimator.PosMatch.None);
			}
			return;
		}
		if (this._hasWalkRunTarget)
		{
			Creature.WalkRunResult walkRunResult = this.UpdateWalkRunToTarget(false);
			if (walkRunResult == Creature.WalkRunResult.Complete || walkRunResult == Creature.WalkRunResult.Blocked)
			{
				this._scheduledStateChangeTime = this._stateTimer + this.settings.grazeEatTime.Random();
				if (this.animator.HasAnimation("IdleToGraze"))
				{
					this.animator.SetAnimationWithTransition("IdleToGraze", "Graze", 0, false, false, FrameAnimator.PosMatch.None);
				}
				else if (this.animator.HasAnimation("Graze"))
				{
					if (this.animator.HasAnimation("WalkToGraze"))
					{
						this.animator.SetAnimationWithTransition("WalkToGraze", "Graze", 0, false, false, FrameAnimator.PosMatch.None);
					}
					else
					{
						this.animator.SetAnimation("Graze", FrameAnimator.PosMatch.None);
					}
				}
			}
		}
		else
		{
			if (this.animator.HasAnimation("Graze"))
			{
				this.animator.SetAnimation("Graze", FrameAnimator.PosMatch.None);
			}
			if (this._stateTimer > this._scheduledStateChangeTime)
			{
				if (this.animator.HasAnimation("GrazeToWalk"))
				{
					this.animator.SetAnimationWithTransition("GrazeToWalk", "Walk", 0, false, false, FrameAnimator.PosMatch.None);
				}
				else if (this.animator.HasAnimation("GrazeToIdle"))
				{
					this.animator.SetAnimationWithTransition("GrazeToIdle", "Idle", 0, false, false, FrameAnimator.PosMatch.None);
				}
				else if (this.animator.HasAnimation("Idle"))
				{
					this.animator.SetAnimation("Idle", FrameAnimator.PosMatch.None);
				}
				this._walkRunTarget = this._zone.range.Random();
				this._hasWalkRunTarget = true;
			}
		}
		if (!this._hasRunGrazeKnot && this.TryRunInkIfNearby("GRAZE_AMBIENT"))
		{
			this._hasRunGrazeKnot = true;
		}
	}

	// Token: 0x06000CA2 RID: 3234 RVA: 0x0006521C File Offset: 0x0006341C
	private void State_Alert(bool start, bool end)
	{
		if (start)
		{
			if (this.animator.IsAnimation("Walk", null, null, null) && this.animator.HasAnimation("WalkToLook"))
			{
				this.animator.SetAnimationWithTransition("WalkToLook", "Look", 0, false, false, FrameAnimator.PosMatch.None);
				return;
			}
			this.animator.SetAnimation("Look", FrameAnimator.PosMatch.None);
			return;
		}
		else
		{
			if (end)
			{
				if (this.animator.HasAnimation("LookToWalk"))
				{
					this.animator.SetAnimationWithTransition("LookToWalk", "Walk", 0, false, false, FrameAnimator.PosMatch.None);
				}
				return;
			}
			if (this._stateTimer > 15f)
			{
				this.state = Creature.State.Graze;
				return;
			}
			return;
		}
	}

	// Token: 0x06000CA3 RID: 3235 RVA: 0x000652C4 File Offset: 0x000634C4
	private void State_MusicRun(bool start, bool end)
	{
		if (start)
		{
			this._blockedWhileMusicRunningFrames = 0;
			return;
		}
		if (end)
		{
			this.animator.speed = 1f;
			return;
		}
		float num = 1f;
		float num2 = this.direction * (this.position.x - Runner.instance.position.x);
		if (!this._waitingForPlayerOnMusicRun && num2 > this.settings.targetMusicRunPlayerOffsetRange.max)
		{
			this._waitingForPlayerOnMusicRun = true;
			this.animator.SetAnimation("Idle", FrameAnimator.PosMatch.None);
			return;
		}
		if (this._waitingForPlayerOnMusicRun)
		{
			if (num2 >= this.settings.targetMusicRunPlayerOffsetRange.Lerp(0.15f))
			{
				return;
			}
			this._waitingForPlayerOnMusicRun = false;
		}
		float num3 = 0.5f * (Mathf.Sin(6.2831855f * Time.time / this.settings.targetMusicRunPlayerOffsetRangePeriod) + 1f);
		float num4 = this.settings.targetMusicRunPlayerOffsetRange.Lerp(num3);
		if (this.direction * Runner.instance.momentum >= 0.7f)
		{
			if (num2 > num4)
			{
				float num5 = Mathf.InverseLerp(num4, 1.5f * num4, num2);
				num = Mathf.Lerp(1f, this.settings.walkSpeed / this.settings.runSpeed, num5);
			}
			else
			{
				float num6 = Mathf.InverseLerp(0.5f * num4, num4, num2);
				num = Mathf.Lerp(2f, 1f, num6);
			}
		}
		if (this._prevState != Creature.State.Jump)
		{
			num *= Mathf.Lerp(0.3f, 1f, Mathf.InverseLerp(0f, 2f, this._stateTimer));
		}
		Simulate.FindOptions standardPredict = Simulate.FindOptions.standardPredict;
		float x = this.position.x;
		if (num * this.settings.runSpeed < Mathf.Lerp(this.settings.walkSpeed, this.settings.runSpeed, 0.5f) || !this.animator.HasAnimation("Run"))
		{
			this.animator.SetAnimation("Walk", FrameAnimator.PosMatch.None);
		}
		else
		{
			this.animator.SetAnimation("Run", FrameAnimator.PosMatch.None);
		}
		float num7 = this.direction * num * this.settings.runSpeed * Time.deltaTime;
		Simulate.FindResult findResult = Simulate.FindGroundPositionAtDistance(new TrackPosition
		{
			slope = this._currentSlope,
			x = this.position.x
		}, num7, standardPredict, this.runnerSettings);
		if (!findResult.foundRequestedX)
		{
			this._blockedWhileMusicRunningFrames++;
			if (this._blockedWhileMusicRunningFrames > 5)
			{
				this.state = Creature.State.EmergencyExit;
				return;
			}
		}
		else
		{
			this._blockedWhileMusicRunningFrames = 0;
		}
		this.position = findResult.sample.point;
		this._currentSlope = findResult.sample.slope;
		if (this._currentSlope.chunk != null)
		{
			Range range = Range.Auto(x, this.position.x);
			ObstaclePlacement[] obstaclePlacements = this._currentSlope.chunk.obstaclePlacements;
			for (int i = 0; i < obstaclePlacements.Length; i++)
			{
				float x2 = obstaclePlacements[i].transform.position.x;
				if (range.Contains(x2))
				{
					this._nextState = Creature.State.MusicRun;
					this._jumpContext = Creature.JumpContext.MusicRun;
					this.state = Creature.State.Jump;
					return;
				}
			}
		}
	}

	// Token: 0x06000CA4 RID: 3236 RVA: 0x000655FC File Offset: 0x000637FC
	private void State_Jump(bool start, bool end)
	{
		if (start)
		{
			this.animator.SetAnimation("Jump", FrameAnimator.PosMatch.None);
			this._jumpBaseAngle = this._rotation;
			this._jumpStartPos = base.transform.position;
			if (MonoSingleton<RunTrack>.instance.playingOrRampingUp)
			{
				this._jumpDuration = Runner.instance.musicRunningJumpDurationFromBeat;
				this._jumpGravity = this.settings.musicRunJumpGravity;
			}
			else
			{
				this._jumpDuration = this.settings.jumpDuration;
				this._jumpGravity = this.settings.jumpGravity;
			}
			if (this._jumpContext == Creature.JumpContext.Exit)
			{
				this._jumpEndPos = new Vector3(this.position.x + 3f * this.direction, this.position.y - 2.5f);
				this._jumpEndSlope = null;
				this._jumpEndPos.z = base.transform.position.z + (float)this._jumpZDir;
			}
			else if (this._jumpContext == Creature.JumpContext.Entry)
			{
				this._jumpEndPos = base.transform.position;
				this._jumpEndSlope = this._currentSlope;
				base.transform.position = (this._jumpStartPos = this._jumpEndPos + new Vector3(-5f * this.direction, -5f) - (float)this._jumpZDir * Vector3.forward);
			}
			else
			{
				if (this._jumpContext != Creature.JumpContext.MusicRun)
				{
					Debug.LogError("Unhandled Creature.JumpContext: " + this._jumpContext.ToString());
				}
				float num = this.direction;
				float runSpeed = this.settings.runSpeed;
				float jumpDuration = this._jumpDuration;
				Simulate.FindResult findResult = Simulate.FindGroundPositionAtTime(new TrackPosition
				{
					slope = this._currentSlope,
					x = this.position.x
				}, this._jumpDuration, this.direction, Simulate.FindOptions.standardPredict, this.runnerSettings);
				this._jumpEndPos = findResult.sample.point3d;
				this._jumpEndSlope = findResult.sample.slope;
			}
			this._jumpVelocity.x = (this._jumpEndPos.x - this._jumpStartPos.x) / this._jumpDuration;
			this._jumpVelocity.y = Parabola.CalcJumpSpeedYFromGravity(this._jumpGravity, this._jumpEndPos.y - this._jumpStartPos.y, this._jumpDuration);
		}
		if (end)
		{
			float num2 = 0f;
			if (this._currentSlope != null)
			{
				num2 = this.settings.slopeAngleFollowScalar * this._currentSlope.SampleAt(this.position.x, false).angle;
			}
			this._rotation = Mathf.LerpAngle(this._rotation, num2, 0.7f);
			return;
		}
		Vector3 vector = base.transform.position + this._jumpVelocity * Time.deltaTime;
		float num3 = Mathf.InverseLerp(0f, this._jumpDuration, this._stateTimer);
		vector.z = this._jumpStartPos.z + num3 * (float)this._jumpZDir;
		bool flag = false;
		if (this._jumpContext != Creature.JumpContext.Entry && this._jumpContext != Creature.JumpContext.Exit)
		{
			SlopeSample slopeSample;
			flag = Raycast.SampleWithDepthRange(this.position, vector, base.transform.position.z, this.runnerSettings.layer.layerCollideNearbyRange + base.transform.position.z, out slopeSample, default(Color)).didHit;
		}
		if (flag || num3 >= 1f)
		{
			base.transform.position = this._jumpEndPos;
			this._currentSlope = this._jumpEndSlope;
			this.state = this.ConsumeNextState();
			return;
		}
		base.transform.position = vector;
		this._jumpVelocity.y = this._jumpVelocity.y + this._jumpGravity * Time.deltaTime;
	}

	// Token: 0x06000CA5 RID: 3237 RVA: 0x000659F5 File Offset: 0x00063BF5
	private static bool ExitTypeIsSafeWhenImprecise(CreatureExitType exitType)
	{
		return exitType == CreatureExitType.JumpDownBehind || exitType == CreatureExitType.JumpDownFront || exitType == CreatureExitType.Fade || exitType == CreatureExitType.RunDownBehind || exitType == CreatureExitType.RunDownFront;
	}

	// Token: 0x06000CA6 RID: 3238 RVA: 0x00065A10 File Offset: 0x00063C10
	private void State_Exit(bool start, bool end)
	{
		if (start)
		{
			if (!this._reachedExitPos || this._prevState != Creature.State.Jump || this._prevPrevState != Creature.State.Exit)
			{
				this._reachedExitPos = false;
			}
			return;
		}
		if (end)
		{
			this._stateControllingRotation = false;
			return;
		}
		if (!this._reachedExitPos)
		{
			this._hasWalkRunTarget = true;
			this._walkRunTarget = this._zone.exitPos.x;
			CreatureExitType exitType = this._zone.exit.exitType;
			Creature.WalkRunResult walkRunResult = this.UpdateWalkRunToTarget(true);
			if (walkRunResult == Creature.WalkRunResult.Complete || (walkRunResult == Creature.WalkRunResult.Blocked && Creature.ExitTypeIsSafeWhenImprecise(exitType)))
			{
				this._reachedExitPos = true;
				if (exitType == CreatureExitType.MusicRunLeft || exitType == CreatureExitType.MusicRunRight)
				{
					this.TryRunInkIfNearby("ENTER_MUSIC_RUN");
				}
				if (exitType == CreatureExitType.MusicRunLeft)
				{
					this.direction = -1f;
					this.state = Creature.State.MusicRun;
					return;
				}
				if (exitType == CreatureExitType.MusicRunRight)
				{
					this.direction = 1f;
					this.state = Creature.State.MusicRun;
					return;
				}
				if (exitType == CreatureExitType.RunToZone)
				{
					this.EnterDestinationZone();
					return;
				}
				if (this._zone.exit.exitType == CreatureExitType.JumpDownBehind || this._zone.exit.exitType == CreatureExitType.JumpDownFront)
				{
					this._jumpZDir = ((this._zone.exit.exitType == CreatureExitType.JumpDownBehind) ? 1 : (-1));
					this.direction = (float)this._zone.exit.direction;
					this._jumpContext = Creature.JumpContext.Exit;
					this._nextState = Creature.State.Exit;
					this.state = Creature.State.Jump;
					return;
				}
				if (this._zone.exit.exitType == CreatureExitType.Fade)
				{
					this.targetAlpha = 0f;
				}
				else if (this._zone.exit.exitType != CreatureExitType.RunDownBehind && this._zone.exit.exitType != CreatureExitType.RunDownFront)
				{
					Debug.LogWarning("Unhandled exit type: " + exitType.ToString());
				}
			}
			else if (walkRunResult == Creature.WalkRunResult.Blocked)
			{
				this.state = Creature.State.EmergencyExit;
				return;
			}
		}
		if (this._reachedExitPos && this._zone.exit.exitType == CreatureExitType.Fade)
		{
			if (this.alpha == 0f)
			{
				this.state = Creature.State.BetweenZones;
				return;
			}
		}
		else if (this._reachedExitPos && (this._zone.exit.exitType == CreatureExitType.RunDownBehind || this._zone.exit.exitType == CreatureExitType.RunDownFront || this._zone.exit.exitType == CreatureExitType.JumpDownBehind || this._zone.exit.exitType == CreatureExitType.JumpDownFront) && this.UpdateRunDownBehindOrFront((float)this._zone.exit.direction, this._zone.exit.exitType == CreatureExitType.RunDownBehind))
		{
			this.state = Creature.State.BetweenZones;
			return;
		}
	}

	// Token: 0x06000CA7 RID: 3239 RVA: 0x00065C98 File Offset: 0x00063E98
	private bool UpdateRunDownBehindOrFront(float dir, bool behind)
	{
		this.targetAlpha = 0f;
		this._stateControllingRotation = true;
		if (this.animator.HasAnimation("Run"))
		{
			this.animator.SetAnimation("Run", FrameAnimator.PosMatch.None);
		}
		else
		{
			this.animator.SetAnimation("Walk", FrameAnimator.PosMatch.None);
		}
		this.direction = dir;
		Vector2 vector = Slope.VectorWithAngle(-this.settings.exitRunDownAngle, this.direction);
		Vector3 vector2 = base.transform.position;
		vector2 += vector * this.settings.runSpeed * Time.deltaTime;
		int num = (behind ? 1 : (-1));
		vector2.z += 0.5f * (float)num * Time.deltaTime;
		base.transform.position = vector2;
		this._rotation = Mathf.MoveTowardsAngle(this._rotation, -this.direction * this.settings.slopeAngleFollowScalar * this.settings.exitRunDownAngle, 120f * Time.deltaTime);
		return this.alpha == 0f;
	}

	// Token: 0x06000CA8 RID: 3240 RVA: 0x00065DB4 File Offset: 0x00063FB4
	private void State_EmergencyExit(bool start, bool end)
	{
		if (start)
		{
			return;
		}
		if (end)
		{
			return;
		}
		if (this.UpdateRunDownBehindOrFront(this.direction, true))
		{
			this.TryRunInkIfNearby("GONE_FOR_GOOD");
			this.state = Creature.State.None;
			return;
		}
	}

	// Token: 0x06000CA9 RID: 3241 RVA: 0x00065DE4 File Offset: 0x00063FE4
	private void DropOnSlope(bool loadingSave)
	{
		Vector3 position = base.transform.position;
		SlopeSample slopeSample = Raycast.FindBestNearbySlopeSample(Level.GetForTransform(base.transform), position, false, (float)(loadingSave ? 1000 : 3));
		this._currentSlope = slopeSample.slope;
		base.transform.position = slopeSample.point3d;
	}

	// Token: 0x06000CAA RID: 3242 RVA: 0x00065E3C File Offset: 0x0006403C
	private void State_Enter(bool start, bool end)
	{
		if (start)
		{
			if (this.alpha == 0f || this._prevState == Creature.State.BetweenZones || this._prevState == Creature.State.None)
			{
				this._rotation = 0f;
				base.transform.position = this._zone.entryPos;
				this.DropOnSlope(false);
			}
			this.targetAlpha = 1f;
			if ((this._zone.entry.entryType == CreatureEntryType.JumpFromBehind || this._zone.entry.entryType == CreatureEntryType.JumpFromFront) && this._prevState != Creature.State.Jump)
			{
				this._jumpZDir = ((this._zone.entry.entryType == CreatureEntryType.JumpFromBehind) ? (-1) : 1);
				this.direction = (float)this._zone.entry.direction;
				this._jumpContext = Creature.JumpContext.Entry;
				this._nextState = Creature.State.Enter;
				this.state = Creature.State.Jump;
				return;
			}
			if (this._zone.entry.entryType == CreatureEntryType.None)
			{
				this.alpha = 1f;
			}
			if (!this._zone.Contains(base.transform.position))
			{
				this._hasWalkRunTarget = true;
				this._walkRunTarget = this._zone.transform.position.x;
				return;
			}
			if (this._zone.state == Creature.State.None)
			{
				this.state = Creature.State.EmergencyExit;
				return;
			}
			this.state = this._zone.state;
			return;
		}
		else
		{
			if (end)
			{
				return;
			}
			Creature.WalkRunResult walkRunResult = this.UpdateWalkRunToTarget(true);
			if (walkRunResult == Creature.WalkRunResult.Complete)
			{
				if (this._zone.state == Creature.State.None)
				{
					this.state = Creature.State.EmergencyExit;
					return;
				}
				this.state = this._zone.state;
				return;
			}
			else
			{
				if (walkRunResult == Creature.WalkRunResult.Blocked)
				{
					this.state = Creature.State.EmergencyExit;
					return;
				}
				return;
			}
		}
	}

	// Token: 0x06000CAB RID: 3243 RVA: 0x00065FE0 File Offset: 0x000641E0
	private void State_BetweenZones(bool start, bool end)
	{
		if (start)
		{
			this.alpha = 0f;
			this.targetAlpha = 0f;
			this._rotation = 0f;
			return;
		}
		if (end)
		{
			return;
		}
		if (this._stateTimer > 1f)
		{
			this.EnterDestinationZone();
		}
	}

	// Token: 0x04000F18 RID: 3864
	private const string idleAnimName = "Idle";

	// Token: 0x04000F19 RID: 3865
	private const string grazeAnimName = "Graze";

	// Token: 0x04000F1A RID: 3866
	private const string idleToGrazeAnimName = "IdleToGraze";

	// Token: 0x04000F1B RID: 3867
	private const string grazeToIdleAnimName = "GrazeToIdle";

	// Token: 0x04000F1C RID: 3868
	private const string grazeToWalkAnimName = "GrazeToWalk";

	// Token: 0x04000F1D RID: 3869
	private const string walkToGrazeAnimName = "WalkToGraze";

	// Token: 0x04000F1E RID: 3870
	private const string walkToLookAnimName = "WalkToLook";

	// Token: 0x04000F1F RID: 3871
	private const string lookToWalkAnimName = "LookToWalk";

	// Token: 0x04000F20 RID: 3872
	private const string lookAnimName = "Look";

	// Token: 0x04000F21 RID: 3873
	private const string walkAnimName = "Walk";

	// Token: 0x04000F22 RID: 3874
	private const string runAnimName = "Run";

	// Token: 0x04000F23 RID: 3875
	private const string jumpAnimName = "Jump";

	// Token: 0x04000F24 RID: 3876
	public string inkName;

	// Token: 0x04000F25 RID: 3877
	public bool shouldSaveState;

	// Token: 0x04000F26 RID: 3878
	public CreatureSettings settings;

	// Token: 0x04000F27 RID: 3879
	public CreatureZone startZone;

	// Token: 0x04000F28 RID: 3880
	public RunnerSettings runnerSettings;

	// Token: 0x04000F29 RID: 3881
	[Disable]
	public float direction = 1f;

	// Token: 0x04000F2A RID: 3882
	private float _alpha = 1f;

	// Token: 0x04000F2B RID: 3883
	[Disable]
	public float targetAlpha = 1f;

	// Token: 0x04000F2C RID: 3884
	private GuidComponent _guidComponent;

	// Token: 0x04000F2D RID: 3885
	private string _guid;

	// Token: 0x04000F2E RID: 3886
	[NonSerialized]
	public SaveState.CreatureState lastValidSaveState;

	// Token: 0x04000F2F RID: 3887
	private FrameAnimator _animator;

	// Token: 0x04000F30 RID: 3888
	private Creature.State _state;

	// Token: 0x04000F31 RID: 3889
	private Creature.State _prevPrevState;

	// Token: 0x04000F32 RID: 3890
	private Creature.State _prevState;

	// Token: 0x04000F33 RID: 3891
	private float _stateTimer;

	// Token: 0x04000F34 RID: 3892
	private float _dt;

	// Token: 0x04000F35 RID: 3893
	private Slope _currentSlope;

	// Token: 0x04000F36 RID: 3894
	private Chunk _currentChunk;

	// Token: 0x04000F37 RID: 3895
	private float _rotation;

	// Token: 0x04000F38 RID: 3896
	private bool _stateControllingRotation;

	// Token: 0x04000F39 RID: 3897
	private bool _hasWalkRunTarget;

	// Token: 0x04000F3A RID: 3898
	private float _walkRunTarget;

	// Token: 0x04000F3B RID: 3899
	private float _scheduledStateChangeTime;

	// Token: 0x04000F3C RID: 3900
	private bool _exiting;

	// Token: 0x04000F3D RID: 3901
	private bool _reachedExitPos;

	// Token: 0x04000F3E RID: 3902
	private Vector3 _jumpStartPos;

	// Token: 0x04000F3F RID: 3903
	private Vector3 _jumpEndPos;

	// Token: 0x04000F40 RID: 3904
	private int _jumpZDir;

	// Token: 0x04000F41 RID: 3905
	private Slope _jumpEndSlope;

	// Token: 0x04000F42 RID: 3906
	private bool _jumpingOffLevel;

	// Token: 0x04000F43 RID: 3907
	private Vector2 _jumpVelocity;

	// Token: 0x04000F44 RID: 3908
	private float _jumpBaseAngle;

	// Token: 0x04000F45 RID: 3909
	private float _jumpDuration;

	// Token: 0x04000F46 RID: 3910
	private float _jumpGravity;

	// Token: 0x04000F47 RID: 3911
	private Creature.JumpContext _jumpContext;

	// Token: 0x04000F48 RID: 3912
	private Creature.State _nextState;

	// Token: 0x04000F49 RID: 3913
	private bool _waitingForPlayerOnMusicRun;

	// Token: 0x04000F4A RID: 3914
	private bool _hasRunGrazeKnot;

	// Token: 0x04000F4B RID: 3915
	private int _blockedWhileMusicRunningFrames;

	// Token: 0x04000F4C RID: 3916
	private SpriteRenderer _hat;

	// Token: 0x04000F4D RID: 3917
	[Disable]
	[SerializeField]
	private CreatureZone _zone;

	// Token: 0x020003A2 RID: 930
	public enum State
	{
		// Token: 0x0400197D RID: 6525
		None,
		// Token: 0x0400197E RID: 6526
		Graze,
		// Token: 0x0400197F RID: 6527
		Alert,
		// Token: 0x04001980 RID: 6528
		MusicRun,
		// Token: 0x04001981 RID: 6529
		Jump,
		// Token: 0x04001982 RID: 6530
		Enter,
		// Token: 0x04001983 RID: 6531
		Exit,
		// Token: 0x04001984 RID: 6532
		BetweenZones,
		// Token: 0x04001985 RID: 6533
		EmergencyExit
	}

	// Token: 0x020003A3 RID: 931
	private enum WalkRunResult
	{
		// Token: 0x04001987 RID: 6535
		NotComplete,
		// Token: 0x04001988 RID: 6536
		Complete,
		// Token: 0x04001989 RID: 6537
		Blocked
	}

	// Token: 0x020003A4 RID: 932
	private enum JumpContext
	{
		// Token: 0x0400198B RID: 6539
		MusicRun,
		// Token: 0x0400198C RID: 6540
		Exit,
		// Token: 0x0400198D RID: 6541
		Entry
	}
}
