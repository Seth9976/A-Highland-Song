using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using InControl;
using UnityEngine;

// Token: 0x020000C8 RID: 200
[SelectionBase]
public class Runner : MonoBehaviour
{
	// Token: 0x06000636 RID: 1590 RVA: 0x00031A00 File Offset: 0x0002FC00
	private void State_Balancing(bool start, bool end)
	{
		if (this._balancePoint == null)
		{
			Debug.LogError("No balance point when in balancing state");
		}
		if (start)
		{
			this.animator.SetAnimation("Teeter", FrameAnimator.PosMatch.None);
			this.currentSlope = null;
			this.physicalDepthLayerIdx = Mathf.RoundToInt(this._balancePoint.transform.position.z);
			this._balanceExitHoldTime = 0f;
			if (!this.isMusicRunning)
			{
				this.momentum = 0f;
			}
			if (this._jumpQueued)
			{
				this.AllowQueuedJump();
				return;
			}
			if (this.isMusicRunning)
			{
				this._jumpDirIntent = this.CreateNormalisedJumpIntent();
				if (this._jumpDirIntent == Vector2.zero)
				{
					this._jumpDirIntent.x = this.direction;
				}
				if (this.TryJump(false))
				{
					this._jump.requireRetroactiveJumpPress = true;
				}
			}
			if (Time.time - this._lastBalanceTime > 2f && Random.value < 0.6f)
			{
				AudioController.instance.PlayVocalisation(Vocalisation.BalanceWobble, 0f);
			}
			this._lastBalanceTime = Time.time;
			return;
		}
		else
		{
			if (end)
			{
				this._prevBalancePoint = this._balancePoint;
				this._balancePoint = null;
				return;
			}
			if (this._leftRightInput * this.direction > 0.5f || this._lookDownTime > 0f)
			{
				this._balanceExitHoldTime += this._dt;
			}
			else
			{
				this._balanceExitHoldTime = 0f;
			}
			bool flag = this._balanceExitHoldTime > 0.5f;
			if (this._leftRightInput * this.direction < 0f)
			{
				this.direction = Mathf.Sign(this._leftRightInput);
			}
			float num = (this.isMusicRunning ? this.settings.balance.balanceMaxTimeMusic : this.settings.balance.balanceMaxTime);
			if (this.stateTimer / num <= 1f && !flag)
			{
				return;
			}
			Vector2 vector = this.position + 0.2f * Vector2.up;
			Vector2 vector2 = this.position - 2f * Vector2.up;
			SlopeSample slopeSample;
			if (!Raycast.SampleWithDepthRange(vector, vector2, (float)this.physicalDepthLayerIdx, this.raycastNearbyRange, out slopeSample, default(Color)).didHit)
			{
				if (!flag)
				{
					this.EscalateFallDamage(Damage.MinorDamage);
					AudioController.instance.PlayVocalisation(Vocalisation.BalanceFall, 0f);
				}
				this._prevVelocity = this.settings.balance.fallVelocity;
				this._prevVelocity.x = this._prevVelocity.x * this.direction;
				this.state = Runner.State.Falling;
				return;
			}
			this.currentSlope = slopeSample.slope;
			if (this.onSlide)
			{
				this.momentum = this.currentSlopeDownDir * 0.7f;
				this.state = Runner.State.Sliding;
				return;
			}
			this.momentum = this.direction * 0.7f;
			this.state = Runner.State.Running;
			return;
		}
	}

	// Token: 0x06000637 RID: 1591 RVA: 0x00031CD4 File Offset: 0x0002FED4
	private void State_BellyWriggle(bool start, bool end)
	{
		if (start)
		{
			string text = (this.animator.IsAnimation("Crouching", null, null, null) ? "BellyWriggleIntoFromCrouch" : "BellyWriggleInto");
			this.animator.SetAnimationWithTransition(text, "BellyWriggle", 0, false, false, FrameAnimator.PosMatch.None);
			this.momentum = 0f;
			this.direction = (float)(this._wriggle.entry.tunnelIsRightwards ? 1 : (-1));
			if (this.currentSlope == null)
			{
				Debug.LogError("BellyWriggle needs to start with a currentSlope but it was null");
				this.state = Runner.State.Running;
				return;
			}
			return;
		}
		else
		{
			if (end)
			{
				this._wriggle = default(Runner.BellyWriggle);
				this.animator.speed = 1f;
				return;
			}
			if (this._wriggle.exiting)
			{
				if (this.animator.normalizedTime > 0.4f)
				{
					this.rotation = Mathf.MoveTowardsAngle(this.rotation, 0f, this.settings.bellyWriggle.transitionRotationSpeed * this._dt);
				}
				if (this.animator.IsAnimation("Idle", null, null, null))
				{
					this.state = Runner.State.Running;
				}
				return;
			}
			TrackPosition trackPosition = this.trackPosition;
			float num = this.currentSlope.SampleAt(trackPosition.x, false).angle;
			Simulate.FindResult findResult = Simulate.FindGroundPositionAtDistance(this.trackPosition, 2f, Simulate.FindOptions.standardSimulate, this.settings);
			Simulate.FindResult findResult2 = Simulate.FindGroundPositionAtDistance(this.trackPosition, -2f, Simulate.FindOptions.standardSimulate, this.settings);
			if (findResult2.foundRequestedX || findResult.foundRequestedX)
			{
				num = Slope.AngleWithVector(findResult.sample.point - findResult2.sample.point, false, null);
			}
			float num2 = num;
			float num3 = this.settings.bellyWriggle.rotationSpeed;
			if (this.animator.IsAnimation("BellyWriggleInto", null, null, null))
			{
				num3 = this.settings.bellyWriggle.transitionRotationSpeed;
			}
			this.rotation = Mathf.MoveTowardsAngle(this.rotation, num2, num3 * this._dt);
			if (this.animator.IsAnimation("BellyWriggle", "BellyWriggleStruggle", null, null))
			{
				float num4;
				if (this._wriggle.entering)
				{
					num4 = this.direction;
				}
				else
				{
					num4 = GameInput.moveLeftRight;
					bool flag = num4 * this.direction > 0f;
					bool flag2 = num4 * this.direction < 0f;
					if ((flag && !Narrative.instance.CanBellyWriggle(true)) || (flag2 && !Narrative.instance.CanBellyWriggle(false)))
					{
						if (Time.time > this._wriggle.lastGruntTime + 1.5f && Random.value < 4f * this._dt)
						{
							AudioController.instance.PlayVocalisation(Vocalisation.BellyWriggleStruggle, 0f);
							this._wriggle.lastGruntTime = Time.time;
						}
						if (((GameInput.pressedLeft && num4 < 0f) || (GameInput.pressedRight && num4 > 0f)) && this.animator.IsAnimation("BellyWriggle", null, null, null))
						{
							this.animator.SetAnimationWithTransition("BellyWriggleStruggle", "BellyWriggle", 0, true, false, FrameAnimator.PosMatch.None);
							if (!this._wriggle.hasVocalisedCurrentStuck)
							{
								AudioController.instance.PlayVocalisation(Vocalisation.BellyWriggleAlarm, 0f);
								this._wriggle.hasVocalisedCurrentStuck = true;
							}
						}
						num4 = 0f;
						this._wriggle.stuckButTriedMoveTime = this._wriggle.stuckButTriedMoveTime + this._dt;
						if (this._wriggle.stuckButTriedMoveTime > this.settings.bellyWriggle.stuckButTryMoveTriggerTime && Time.time > this._wriggle.lastStuckInkCallTime + this.settings.bellyWriggle.minTimeBetweenStuckInkCalls && !Narrative.instance.isBusy)
						{
							Narrative.instance.BellyWriggleTriedMoveButStuck();
							this._wriggle.stuckButTriedMoveTime = 0f;
							this._wriggle.lastStuckInkCallTime = Time.time;
						}
					}
					else if ((flag && !Narrative.instance.CanBellyWriggle(false)) || (flag2 && !Narrative.instance.CanBellyWriggle(true)))
					{
						if (Time.time > this._wriggle.lastStuckInkCallTime + this.settings.bellyWriggle.minTimeBetweenStuckInkCalls && !Narrative.instance.isBusy)
						{
							Narrative.instance.BellyWriggleMovedOtherWayWhenStuck();
							this._wriggle.stuckButTriedMoveTime = 0f;
							this._wriggle.lastStuckInkCallTime = Time.time;
						}
					}
					else if (Mathf.Abs(num4) > 0f && this._wriggle.stuckButTriedMoveTime > 0f)
					{
						this._wriggle.stuckButTriedMoveTime = 0f;
					}
					if (num4 != 0f && this._wriggle.hasVocalisedCurrentStuck)
					{
						this._wriggle.hasVocalisedCurrentStuck = false;
					}
					if (num4 != 0f && Time.time > this._wriggle.lastGruntTime + 2f && Random.value < 2f * this._dt)
					{
						AudioController.instance.PlayVocalisation(Vocalisation.BellyWriggleGrunt, 0f);
						this._wriggle.lastGruntTime = Time.time;
					}
				}
				if (this.animator.IsAnimation("BellyWriggleStruggle", null, null, null))
				{
					this.animator.speed = 1f;
					num4 = 0f;
				}
				else
				{
					this.animator.speed = num4 * this.direction;
				}
				float num5 = num4 * this.settings.bellyWriggle.speed * this._wriggle.entry.speedModifier * Mathf.Lerp(0.3f, 1f, Random.value) * this._dt;
				if (num5 != 0f)
				{
					Simulate.FindOptions standardSimulate = Simulate.FindOptions.standardSimulate;
					Simulate.FindResult findResult3 = Simulate.FindGroundPositionAtDistance(this.trackPosition, num5, standardSimulate, this.settings);
					this.position = findResult3.sample.point;
					this.currentSlope = findResult3.sample.slope;
					InputVibration.LongVerySoft();
				}
				float x = this._wriggle.entry.transform.position.x;
				if ((this._wriggle.entry.tunnelIsRightwards && this.position.x > x + this.settings.bellyWriggle.entryDist) || (!this._wriggle.entry.tunnelIsRightwards && this.position.x < x - this.settings.bellyWriggle.entryDist))
				{
					this._wriggle.entering = false;
				}
				if (!this._wriggle.entering)
				{
					BellyWriggleEnd bellyWriggleEnd = this.NearestBellyWriggleEnd(this.position);
					if (bellyWriggleEnd != null)
					{
						float x2 = bellyWriggleEnd.transform.position.x;
						if ((bellyWriggleEnd.tunnelIsRightwards && this.position.x < x2) || (!bellyWriggleEnd.tunnelIsRightwards && this.position.x > x2))
						{
							this.EndBellyWriggle();
							return;
						}
					}
				}
			}
			return;
		}
	}

	// Token: 0x06000638 RID: 1592 RVA: 0x000323BC File Offset: 0x000305BC
	private BellyWriggleEnd NearestBellyWriggleEnd(Vector2 toPoint)
	{
		BellyWriggleEnd bellyWriggleEnd = null;
		float num = 10f;
		foreach (BellyWriggleEnd bellyWriggleEnd2 in Level.current.bellyWriggleEnds)
		{
			float num2 = Vector2.Distance(bellyWriggleEnd2.transform.position, toPoint);
			if (num2 < num)
			{
				bellyWriggleEnd = bellyWriggleEnd2;
				num = num2;
			}
		}
		return bellyWriggleEnd;
	}

	// Token: 0x06000639 RID: 1593 RVA: 0x00032438 File Offset: 0x00030638
	public void StartBellyWriggle()
	{
		this._wriggle = new Runner.BellyWriggle
		{
			entering = true,
			entry = this.NearestBellyWriggleEnd(this.position)
		};
		if (this._wriggle.entry == null)
		{
			Debug.LogError(string.Format("BellyWriggle couldn't be started because there was no nearby BellyWriggleEnd component in the current level (within {0} units)", 10f));
			this.state = Runner.State.Running;
			return;
		}
		this.state = Runner.State.BellyWriggling;
	}

	// Token: 0x0600063A RID: 1594 RVA: 0x000324AC File Offset: 0x000306AC
	public void EndBellyWriggle()
	{
		if (this.state == Runner.State.BellyWriggling && !this._wriggle.exiting)
		{
			this.animator.SetAnimationWithTransition("BellyWriggleOut", "Idle", 0, true, false, FrameAnimator.PosMatch.None);
			this.animator.speed = 1f;
			this._wriggle.exiting = true;
			Narrative.instance.BellyWriggleDidExit();
		}
	}

	// Token: 0x0600063B RID: 1595 RVA: 0x0003250F File Offset: 0x0003070F
	public void StartBoating()
	{
		this.state = Runner.State.Boat;
	}

	// Token: 0x0600063C RID: 1596 RVA: 0x0003251C File Offset: 0x0003071C
	private void State_Boat(bool start, bool end)
	{
		Boat activeBoat = Game.instance.activeBoat;
		if (start)
		{
			this._currentSlope = null;
			this.MarkCurrentSampleDirty();
			this.rotation = 0f;
			this.direction = activeBoat.sailDirectionX;
			if (activeBoat.moiraAnim != null)
			{
				this.animator.SetAnimation(activeBoat.moiraAnim, 0, FrameAnimator.PosMatch.None);
				return;
			}
			this.animator.SetAnimation("Crouching", FrameAnimator.PosMatch.None);
			return;
		}
		else
		{
			if (end)
			{
				return;
			}
			if (activeBoat.jumpOffBeforeArrival && Vector3.Distance(activeBoat.destination.position, base.transform.position) < 12f)
			{
				SlopeSample landingSlopeSample = activeBoat.landingSlopeSample;
				this.PerformJump_Narrative(landingSlopeSample.point3d, landingSlopeSample.slope);
				return;
			}
			if (activeBoat.arrived)
			{
				Vector3 position = activeBoat.transform.position;
				SlopeSample landingSlopeSample2 = activeBoat.landingSlopeSample;
				base.transform.position = landingSlopeSample2.point3d;
				this.physicalDepthLayerIdx = Mathf.RoundToInt(base.transform.position.z);
				this.currentSlope = landingSlopeSample2.slope;
				this.state = Runner.State.Running;
				return;
			}
			base.transform.position = activeBoat.sitTransform.position;
			base.transform.rotation = activeBoat.sitTransform.rotation;
			this.physicalDepthLayerIdx = Mathf.RoundToInt(base.transform.position.z);
			return;
		}
	}

	// Token: 0x17000184 RID: 388
	// (get) Token: 0x0600063D RID: 1597 RVA: 0x00032680 File Offset: 0x00030880
	public Vector3 catchAimBasePos
	{
		get
		{
			if (!this._hasCapturedAimPos)
			{
				this._catchAimPos = this.currentSample.point3d;
				this._hasCapturedAimPos = true;
			}
			return this._catchAimPos;
		}
	}

	// Token: 0x17000185 RID: 389
	// (get) Token: 0x0600063E RID: 1598 RVA: 0x000326B6 File Offset: 0x000308B6
	public bool inEagleCatchAnim
	{
		get
		{
			return this.animator.IsAnimation("EagleCatch", "EagleCarryLoop", "EagleDrop", null);
		}
	}

	// Token: 0x17000186 RID: 390
	// (get) Token: 0x0600063F RID: 1599 RVA: 0x000326D4 File Offset: 0x000308D4
	public float catchAnimTimeToGrab
	{
		get
		{
			FrameAnimation frameAnimation = this.animator.TryGetAnim("EagleCatch");
			if (frameAnimation == null)
			{
				Debug.LogError("EagleCatch anim missing on player?");
				return float.MaxValue;
			}
			return 0.4f * frameAnimation.duration;
		}
	}

	// Token: 0x06000640 RID: 1600 RVA: 0x00032717 File Offset: 0x00030917
	public void PrepareForEagleCatch(float faceDir)
	{
		this.direction = faceDir;
		this.state = Runner.State.Caught;
	}

	// Token: 0x06000641 RID: 1601 RVA: 0x00032728 File Offset: 0x00030928
	public void ReactToEagleCatch()
	{
		this.animator.SetAnimation("EagleCatch", FrameAnimator.PosMatch.None);
	}

	// Token: 0x06000642 RID: 1602 RVA: 0x0003273B File Offset: 0x0003093B
	public void Drop()
	{
		this.animator.SetAnimationWithTransition("EagleDrop", "Fall", 0, false, false, FrameAnimator.PosMatch.Mouth);
		this.physicalDepthLayerIdx = Mathf.RoundToInt(base.transform.position.z);
		this.state = Runner.State.Falling;
	}

	// Token: 0x06000643 RID: 1603 RVA: 0x00032778 File Offset: 0x00030978
	private void State_Caught(bool start, bool end)
	{
		if (start)
		{
			this._lastCaughtPos = base.transform.position;
			return;
		}
		if (end)
		{
			this.physicalDepthLayerIdx = Mathf.RoundToInt(base.transform.position.z);
			this._hasCapturedAimPos = false;
			return;
		}
		if (this._hasCapturedAimPos)
		{
			this.currentSlope = null;
		}
		if (this.animator.IsAnimation("EagleCatch", null, null, null) && this.animator.normalizedTime == 1f)
		{
			this.animator.SetAnimation("EagleCarryLoop", FrameAnimator.PosMatch.None);
		}
		Vector3 position = base.transform.position;
		this._caughtVelocity = (position - this._lastCaughtPos) / this._dt;
		this._lastCaughtPos = position;
		this.physicalDepthLayerIdx = Mathf.RoundToInt(base.transform.position.z);
	}

	// Token: 0x06000644 RID: 1604 RVA: 0x00032858 File Offset: 0x00030A58
	public bool TryJumpOnChairLift()
	{
		Prop lastInteractedProp = PropsController.instance.lastInteractedProp;
		if (lastInteractedProp == null)
		{
			Debug.LogError("Expected PropsController.instance.lastInteractedProp to be valid");
			return false;
		}
		SkiLiftChair componentInParent = lastInteractedProp.GetComponentInParent<SkiLiftChair>();
		if (componentInParent == null)
		{
			Debug.LogError("Expected PropsController.instance.lastInteractedProp to have a SkiLiftChair in its parent hierarchy");
			return false;
		}
		Vector3 vector = MonoSingleton<SkiLift>.instance.PredictChairMovement(componentInParent, 0.5f);
		Vector2 vector2 = lastInteractedProp.transform.position + vector;
		Vector2 vector3 = vector2 - this.position;
		float num = this.settings.skiLift.maxJumpX / this.settings.skiLift.maxJumpY;
		vector3.y *= num;
		if (vector3.magnitude > this.settings.skiLift.maxJumpX)
		{
			return false;
		}
		this.PerformJump_Narrative(vector2, null);
		this._jumpingOntoChairLift = true;
		this._skiLiftChair = componentInParent;
		CameraVolume cameraVolume = CameraVolume.WithName("Camera Volume follows active ski lift chair");
		if (cameraVolume != null)
		{
			cameraVolume.GetComponent<FollowTransform>().transformToFollow = componentInParent.sitTransform;
		}
		return true;
	}

	// Token: 0x06000645 RID: 1605 RVA: 0x00032964 File Offset: 0x00030B64
	private void State_ChairLift(bool start, bool end)
	{
		if (start)
		{
			this.rotation = 0f;
			this.animator.SetAnimation("ChairLiftSit", FrameAnimator.PosMatch.None);
			return;
		}
		if (end)
		{
			this._skiLiftChair = null;
			this._skiLiftStoppedWhileOnIt = false;
			return;
		}
		base.transform.position = this._skiLiftChair.sitTransform.position;
		base.transform.rotation = this._skiLiftChair.sitTransform.rotation;
		this.physicalDepthLayerIdx = Mathf.RoundToInt(base.transform.position.z);
		if (!this._skiLiftStoppedWhileOnIt && this._skiLiftChair.transform.position.y > MonoSingleton<SkiLift>.instance.breakDownPosition.position.y)
		{
			MonoSingleton<SkiLift>.instance.running = false;
			AudioController.instance.PlayVocalisation(Vocalisation.ClimbingSlip, 0f);
			Narrative.instance.EndEvent("ChairLiftMovingUp");
			this._skiLiftStoppedWhileOnIt = true;
		}
	}

	// Token: 0x06000646 RID: 1606 RVA: 0x00032A5C File Offset: 0x00030C5C
	private void State_CliffEdgeWobble(bool start, bool end)
	{
		if (start)
		{
			this.animator.SetAnimationWithTransition("TeeterInto", "Teeter", 0, false, false, FrameAnimator.PosMatch.None);
			this._cliffEdgeWobbleDirection = this.direction;
			this._cliffEdgeWobbleConfirmHoldDuration = 0f;
			AudioController.instance.PlayVocalisation(Vocalisation.CliffEdgeWobble, 0f);
			this.momentum = 0f;
			return;
		}
		if (end)
		{
			return;
		}
		if (this.animator.IsAnimation("TeeterStepBack", "TeeterStepBackTurn", null, null))
		{
			if (this.animator.normalizedTime == 1f)
			{
				this.state = Runner.State.Running;
			}
			if (this.stateTimer < 2f)
			{
				AudioController.instance.PlayVocalisation(Vocalisation.CliffEdgeWobbleRelease, 0f);
			}
			return;
		}
		if ((this.stateTimer > 2f && this._cliffEdgeWobbleConfirmHoldDuration == 0f) || this._upDownPressed > 0f)
		{
			this.animator.SetAnimationWithTransition("TeeterStepBack", "Idle", 0, false, false, FrameAnimator.PosMatch.None);
			return;
		}
		if (this._leftRightInput * this._cliffEdgeWobbleDirection < 0f)
		{
			this.animator.SetAnimationWithTransition("TeeterStepBackTurn", "Idle", 0, false, false, FrameAnimator.PosMatch.None);
			return;
		}
		if ((this._leftRightInput * this._cliffEdgeWobbleDirection > 0f || this._upDownHeld < -this.settings.climb.inputUpDownThreshold) && this._climbPromptFill == 0f)
		{
			this._cliffEdgeWobbleConfirmHoldDuration += this._dt;
		}
		else
		{
			this._cliffEdgeWobbleConfirmHoldDuration = 0f;
		}
		if (this._cliffEdgeWobbleConfirmHoldDuration >= this.settings.cliffEdgeWobbleConfirmTime)
		{
			float angle = this.currentSample.angle;
			float num = Mathf.Abs(Simulate.SignedSpeedOnGround(0.5f * this._cliffEdgeWobbleDirection, angle, false, this.settings.run));
			Vector2 vector = this.direction * Slope.VectorWithAngle(angle, 1f);
			if (vector.y > 0f)
			{
				vector.y = 0f;
				vector.Normalize();
			}
			this._prevVelocity = num * vector;
			this.position += 1f * vector;
			AudioController.instance.PlayVocalisation(Vocalisation.IntentionalFall, 0f);
			this.state = Runner.State.Falling;
			return;
		}
	}

	// Token: 0x17000187 RID: 391
	// (get) Token: 0x06000647 RID: 1607 RVA: 0x00032C94 File Offset: 0x00030E94
	public float climbSpeed
	{
		get
		{
			return (float)Mathf.Abs(this._climbing.reachDir);
		}
	}

	// Token: 0x17000188 RID: 392
	// (get) Token: 0x06000648 RID: 1608 RVA: 0x00032CA7 File Offset: 0x00030EA7
	public float climbingAngle
	{
		get
		{
			return this._climbing.angle;
		}
	}

	// Token: 0x06000649 RID: 1609 RVA: 0x00032CB4 File Offset: 0x00030EB4
	private void State_Climbing(bool start, bool end)
	{
		if (start)
		{
			this._upDownPressed = 0f;
			this._climbing.reachDir = 0;
			this._climbing.reachProgress = 0f;
			this.currentSlope = null;
			this.momentum = 0f;
			this._climbing.lastUpperHold = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
			this._climbing.lastLowerHold = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
			this._climbing.upperLimitHit = false;
			this._climbing.lowerLimitHit = false;
			this._climbing.canDropDownOrSlide = false;
			this._climbing.dropConfirmHoldDuration = 0f;
			this._climbing.climbDownCouldSlideTimer = 0f;
			this.lastSlopeJumpedFrom = null;
			this.lastBalancePointDroppedFrom = null;
			this.SetupNextSlip();
			return;
		}
		if (end)
		{
			this.animator.speed = 1f;
			ParticlesX.SetActive(this._perspirationParticles, false);
			return;
		}
		bool flag = this.animator.IsAnimation("WallSlide", "WallSlideToClimb", null, null);
		bool flag2 = this._upDownHeld > 0f || (this._leftRightInput * this.direction > 0f && this._climbing.angle < 120f);
		bool flag3 = this._upDownHeld < 0f || (this._leftRightInput * this.direction < 0f && this._climbing.angle < 120f);
		bool flag4 = Mathf.Abs(this._upDownHeld) < 0.5f && this._leftRightInput * this.direction < 0f;
		bool flag5 = this._climbing.reachDir == 0;
		if (!flag)
		{
			if (flag2 && !this._climbing.upperLimitHit)
			{
				this._climbing.reachDir = 1;
			}
			else if (flag3 && !this._climbing.lowerLimitHit)
			{
				this._climbing.reachDir = -1;
			}
		}
		if (flag5 && this._climbing.reachDir != 0)
		{
			if (this._climbing.reachDir > 0)
			{
				this._climbing.reachRightHanded = !this._climbing.reachRightHanded;
				this._climbing.reachProgress = 0f;
			}
			else
			{
				this._climbing.reachProgress = 1f;
			}
			AudioController.instance.PlayVocalisation(Vocalisation.ClimbReach, 0f);
		}
		float num = 0f;
		if (this._climbing.reachDir != 0)
		{
			int num2 = ((this._climbing.reachDir == -1) ? 0 : 1);
			float num3 = (GameInput.sprintHeld ? this.settings.climb.sprintButtonSpeedup : 1f);
			float num4 = ((this._climbing.reachDir < 0) ? this.settings.climb.downwardSpeedup : 1f);
			float num5 = ((this._climbing.reachDir == -1) ? this.settings.climb.climbReachDownDuration : this.settings.climb.climbReachUpDuration);
			float num6 = num3 * num4 / num5 * this._dt;
			this._climbing.reachProgress = Mathf.MoveTowards(this._climbing.reachProgress, (float)num2, num6);
			num = (float)this._climbing.reachDir * num6 * this.settings.climb.climbReachAnimDistance;
			if (this._climbing.reachProgress == (float)num2)
			{
				if (this._climbing.reachDir < 0)
				{
					this._climbing.reachRightHanded = !this._climbing.reachRightHanded;
				}
				this._climbing.reachDir = 0;
			}
		}
		this.UpdateClimbingHolds(!flag);
		if ((num > 0f && !this._climbing.upperLimitHit) || (num < 0f && !this._climbing.lowerLimitHit))
		{
			this.UpdateClimbingMovement(num);
		}
		if (this._climbing.lowerLimitHit && this._climbing.canDropDownOrSlide && this._upDownHeld < -this.settings.climb.inputUpDownThreshold)
		{
			this._climbing.dropConfirmHoldDuration = this._climbing.dropConfirmHoldDuration + this._dt;
			if (this._climbing.dropConfirmHoldDuration > 0.5f)
			{
				this.DropOrSlideFromClimb(true);
				return;
			}
		}
		else
		{
			this._climbing.dropConfirmHoldDuration = 0f;
		}
		if (this._upDownHeld != 0f && this.TryClimbOverTopOrOffBottom(this._upDownHeld))
		{
			return;
		}
		if (flag4 && this.TryClimbOffToLedgeBehind())
		{
			return;
		}
		bool flag6;
		if (DebugOptions.opts.originalClimbDownDontAlwaysWallSlide)
		{
			flag6 = GameInput.sprintHeld;
		}
		else
		{
			bool flag7 = this._climbing.angle < this.settings.wallSlide.maxSlideAngle && this.stateTimer > this.settings.climb.timeBeforeWallSlide1;
			if (flag7 && this._climbing.reachDir != 0)
			{
				this._climbing.climbDownCouldSlideTimer = this._climbing.climbDownCouldSlideTimer + this._dt;
			}
			else
			{
				this._climbing.climbDownCouldSlideTimer = 0f;
			}
			flag6 = flag7 && this._climbing.climbDownCouldSlideTimer > this.settings.climb.timeBeforeWallSlide2;
		}
		if (this._upDownHeld < 0f && flag6 && this.TryStartWallSlideFromClimb(this._climbing.poly, this._climbing.normal, false))
		{
			return;
		}
		if (this.UpdateClimbingStamina())
		{
			return;
		}
		if (this.UpdateClimbingSlip((float)this._climbing.reachDir))
		{
			return;
		}
		if (this.staminaIsVeryLow && Time.time > this._climbing.nextLowStaminaHurtTime)
		{
			this.health.ApplyDamage(DamageType.LowStaminaHurt, Damage.MinorDamage);
			this._climbing.nextLowStaminaHurtTime = Time.time + this.settings.climb.climbingWithLowStaminaInterval;
		}
		this.UpdateClimbingStateAnimations();
	}

	// Token: 0x17000189 RID: 393
	// (get) Token: 0x0600064A RID: 1610 RVA: 0x0003327C File Offset: 0x0003147C
	private PolySimulate.Options climbingPolyIterateOpts
	{
		get
		{
			return new PolySimulate.Options
			{
				preventNonClimbable = true,
				preventDirectionChange = true,
				allowPolyChange = true,
				validAngleRange = new Range(-360f, 360f),
				slopePolyCheckDepthRange = this.raycastCurrentAndNearForeRange,
				runnerSettings = this.settings
			};
		}
	}

	// Token: 0x0600064B RID: 1611 RVA: 0x000332DC File Offset: 0x000314DC
	private void UpdateClimbingHolds(bool allowPositionUpdate)
	{
		PolySimulate.Options climbingPolyIterateOpts = this.climbingPolyIterateOpts;
		PolySimulate.Result result = PolySimulate.AroundEdge(this._climbing.poly, this._climbing.mainPos, base.transform.up, 2f, climbingPolyIterateOpts, null);
		PolySimulate.Result result2 = PolySimulate.AroundEdge(this._climbing.poly, this._climbing.mainPos, -base.transform.up, 2f, climbingPolyIterateOpts, null);
		this._climbing.upperLimitHit = !result.success;
		this._climbing.lowerLimitHit = !result2.success;
		if (result.distanceTravelled > 1f)
		{
			this._climbing.lastUpperHold = result.position3d;
			this._climbing.lastUpperHoldDist = result.distanceTravelled;
		}
		if (result2.distanceTravelled > 1f)
		{
			this._climbing.lastLowerHold = result2.position3d;
			this._climbing.lastLowerHoldDist = result2.distanceTravelled;
		}
		if (allowPositionUpdate)
		{
			if (this._climbing.lastLowerHoldValid && this._climbing.lastUpperHoldValid)
			{
				float num = this._climbing.lastLowerHoldDist / (this._climbing.lastLowerHoldDist + this._climbing.lastUpperHoldDist);
				this.position = Vector2.Lerp(this._climbing.lastLowerHold, this._climbing.lastUpperHold, num);
			}
			else
			{
				this.position = this._climbing.mainPos;
			}
		}
		if (result2.endReason == PolySimulate.EndReason.PreventNonClimbable || result2.endReason == PolySimulate.EndReason.AngleTooHigh || result2.endReason == PolySimulate.EndReason.PreventDirectionChange)
		{
			this._climbing.canDropDownOrSlide = true;
			return;
		}
		this._climbing.canDropDownOrSlide = false;
	}

	// Token: 0x0600064C RID: 1612 RVA: 0x000334AC File Offset: 0x000316AC
	private void UpdateClimbingMovement(float climbDelta)
	{
		Vector3 vector = base.transform.up;
		if (climbDelta != 0f)
		{
			vector = (climbDelta * vector).normalized;
		}
		if (this._climbing.lastUpperHoldValid && this._climbing.lastLowerHoldValid)
		{
			Vector3 vector2 = this._climbing.lastUpperHold - this._climbing.lastLowerHold;
			this._climbing.normal = this.direction * new Vector2(-vector2.y, vector2.x).normalized;
			this._climbing.angle = Util.ClimbAngleFromSurfaceNormal(this._climbing.normal, this.direction);
		}
		float num = this.RotationToClimbOnNormal(this._climbing.normal);
		this.rotation = Mathf.MoveTowardsAngle(this.rotation, num, this.settings.climb.climbRotationSpeed * this._dt);
		PolySimulate.Options climbingPolyIterateOpts = this.climbingPolyIterateOpts;
		PolySimulate.Result result = PolySimulate.AroundEdge(this._climbing.poly, this._climbing.mainPos, vector, Mathf.Abs(climbDelta), climbingPolyIterateOpts, null);
		if (!result.success)
		{
			Debug.LogError(string.Format("Upper/lower hold was found to be valid, how could we fail to find result for climb delta of {0}?", climbDelta));
			return;
		}
		this._climbing.mainPos = result.position3d;
		this._climbing.poly = result.poly;
	}

	// Token: 0x0600064D RID: 1613 RVA: 0x0003361C File Offset: 0x0003181C
	private bool TryClimbOverTopOrOffBottom(float climbDir)
	{
		if (climbDir > 0f)
		{
			Vector2 vector = this.position;
			if (this._climbing.lastUpperHoldValid)
			{
				vector = this._climbing.lastUpperHold;
			}
			if (this.FindClimbUpAndOver(vector, (float)this.physicalDepthLayerIdx, ClimbCheckRange.Climb, this._climbing.poly, out this._climbUpAndOver))
			{
				this._climbUpAndOver.transition = this.settings.transition.climbUpAndOver;
				this.state = Runner.State.ClimbUpAndOver;
				return true;
			}
		}
		if (climbDir < 0f)
		{
			Vector3 vector2 = this.physicalPosition3d;
			if (this._climbing.lastLowerHoldValid)
			{
				vector2 = this._climbing.lastLowerHold;
			}
			if (this.TryClimbDownFrom(vector2))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x0600064E RID: 1614 RVA: 0x000336D4 File Offset: 0x000318D4
	private bool TryClimbDownFrom(Vector3 climbDownFrom)
	{
		Vector3 vector = climbDownFrom + new Vector3(this.direction * this.settings.climb.climbDownRayStart.x, this.settings.climb.climbDownRayStart.y, 0f);
		Vector3 vector2 = vector + new Vector3(this.direction * this.settings.climb.climbDownRay.x, this.settings.climb.climbDownRay.y, 0f);
		Vector3 vector3 = climbDownFrom + new Vector3(this.direction * this.settings.climb.climbDownRayStart2.x, this.settings.climb.climbDownRayStart.y, 0f);
		Vector3 vector4 = vector3 + new Vector3(this.direction * this.settings.climb.climbDownRay2.x, this.settings.climb.climbDownRay2.y, 0f);
		SlopeSample slopeSample;
		if (Raycast.SampleWithDepthRange(vector, vector2, (float)this.physicalDepthLayerIdx, this.raycastCurrentAndSafeForeRange, out slopeSample, default(Color)).didHit || Raycast.SampleWithDepthRange(vector3, vector4, (float)this.physicalDepthLayerIdx, this.raycastCurrentAndSafeForeRange, out slopeSample, default(Color)).didHit)
		{
			this.ClimbOffWallToSlope(slopeSample.slope, slopeSample.point, 1f);
			return true;
		}
		return false;
	}

	// Token: 0x0600064F RID: 1615 RVA: 0x00033864 File Offset: 0x00031A64
	private bool TryClimbOffToLedgeBehind()
	{
		Vector3 vector = this.physicalPosition3d + 1f * Vector3.forward + this.direction * this.settings.climb.climbOffSearchX * Vector3.right;
		Range range = this.settings.climb.climbOffToLedgeRangeX * this.direction + this.physicalPosition3d.x;
		Range range2 = this.settings.climb.climbOffToLedgeRangeY + vector.y;
		List<Slope> list = Level.current.slopes.Nearby(vector, Range.infinity, 5f, null);
		Slope slope = null;
		Vector2 vector2 = default(Vector2);
		float num = float.MaxValue;
		foreach (Slope slope2 in list)
		{
			int num2 = Mathf.RoundToInt(slope2.transform.position.z);
			if (num2 == this.physicalDepthLayerIdx + 1 || num2 == this.physicalDepthLayerIdx - 1)
			{
				SlopeSample slopeSample = slope2.SampleAt(vector.x, false);
				if (slopeSample.outOfRange)
				{
					if (slopeSample.t < 0.5f)
					{
						slopeSample.point = slopeSample.slope.leftPoint;
					}
					else
					{
						slopeSample.point = slopeSample.slope.rightPoint;
					}
				}
				if (range.Contains(slopeSample.point.x) && range2.Contains(slopeSample.point.y))
				{
					float num3 = Vector2.Distance(vector, slopeSample.point);
					if (num3 < num)
					{
						num = num3;
						vector2 = slopeSample.point;
						slope = slopeSample.slope;
					}
				}
			}
		}
		if (slope == null)
		{
			return false;
		}
		this.ClimbOffWallToSlope(slope, vector2, 0.3f);
		return true;
	}

	// Token: 0x06000650 RID: 1616 RVA: 0x00033A7C File Offset: 0x00031C7C
	private void UpdateClimbingStateAnimations()
	{
		if (this.animator.IsAnimation("WallSlide", null, null, null))
		{
			this.animator.SetAnimation("WallSlideToClimb", FrameAnimator.PosMatch.None);
			return;
		}
		if (this.animator.IsAnimation("WallSlideToClimb", null, null, null) && this.animator.normalizedTime < 1f)
		{
			return;
		}
		if (this._climbing.reachDir != 0)
		{
			this.animator.speed = 0f;
			this.animator.SetAnimation(this._climbing.reachRightHanded ? "ClimbReachRight" : "ClimbReachLeft", FrameAnimator.PosMatch.None);
			this.animator.normalizedTime = this._climbing.reachProgress;
			return;
		}
		this.animator.speed = 1f;
		this.animator.SetAnimation(this._climbing.reachRightHanded ? "ClimbIdleRight" : "ClimbIdleLeft", FrameAnimator.PosMatch.None);
	}

	// Token: 0x06000651 RID: 1617 RVA: 0x00033B68 File Offset: 0x00031D68
	private bool UpdateClimbingStamina()
	{
		if (this.playerControlDisabled != PlayerControlDisableReason.None)
		{
			return false;
		}
		float num = 1f / this.settings.stamina.climbTimeLimit;
		if (this._climbing.angle > this.settings.stamina.overhangStartAngle)
		{
			num *= this.settings.stamina.overhangScalar;
		}
		if (GameInput.sprintHeld)
		{
			num *= this.settings.stamina.climbSprintScalar;
		}
		this.stamina -= num * this._dt;
		if (this.stamina <= 0f)
		{
			this.stamina = 0f;
			AudioController.instance.PlayVocalisation(Vocalisation.ClimbSlipOrWallSlideFall, 0f);
			this.DropOrSlideFromClimb(false);
			return true;
		}
		return false;
	}

	// Token: 0x06000652 RID: 1618 RVA: 0x00033C2C File Offset: 0x00031E2C
	private void DropOrSlideFromClimb(bool intentional)
	{
		if (this.TryDropToSlopeFromClimb())
		{
			return;
		}
		if (this.TryStartWallSlideFromClimb(this._climbing.poly, this._climbing.normal, false))
		{
			return;
		}
		this._prevVelocity = new Vector2(-2f * this.direction, 0f);
		this.FallFromClimb(intentional);
	}

	// Token: 0x06000653 RID: 1619 RVA: 0x00033C88 File Offset: 0x00031E88
	private bool TryDropToSlopeFromClimb()
	{
		Vector3 vector = this.physicalPosition3d;
		if (this._climbing.lastLowerHoldValid)
		{
			vector = this._climbing.lastLowerHold;
		}
		return this.TryClimbDownFrom(vector);
	}

	// Token: 0x06000654 RID: 1620 RVA: 0x00033CBC File Offset: 0x00031EBC
	private bool UpdateClimbingSlip(float climbDir)
	{
		if (this.playerControlDisabled != PlayerControlDisableReason.None)
		{
			return false;
		}
		float num = 1f;
		if (climbDir == 0f)
		{
			num *= this.settings.climb.slipTimeScalarStopped;
		}
		num *= Mathf.Lerp(1f, this.settings.climb.slipTimeScalarLowStamina, 1f - this.stamina);
		if ((WeatherSystem.instance.currentWeather & (WeatherType.Raining | WeatherType.Storm | WeatherType.Snow)) > WeatherType.Clear && this.health.shelterProtectionStrength == ShelterProtectionStrength.None)
		{
			num *= this.settings.climb.slipTimeScalarBadWeather;
		}
		num *= Mathf.Lerp(1f, this.settings.climb.slipTimeScalarLowStamina, 1f - this.stamina);
		if (GameInput.sprintHeld)
		{
			num *= this.settings.climb.slipTimeSprintScalar;
		}
		if (GameInput.debugFast)
		{
			num = 0f;
		}
		this._climbing.timeToSlip = this._climbing.timeToSlip - num * this._dt;
		bool flag = (this.prevState == Runner.State.Jumping || this.prevState == Runner.State.Falling) && Random.value < 0.6f;
		if ((this._climbing.timeToSlip <= 0f || flag) && !GameInput.debugFast && PlayerPrefsX.GetInt("neverSlip", 0) == 0 && !DebugOptions.opts.neverSlip)
		{
			this.state = Runner.State.ClimbSlipping;
			return true;
		}
		return false;
	}

	// Token: 0x06000655 RID: 1621 RVA: 0x00033E28 File Offset: 0x00032028
	private Runner.ClimbSetup TrySetupClimbing(ClimbCheckRange climbRangeCheck, bool doStateChange = true, bool transitionFromScramble = false, bool closeOnly = false)
	{
		if (this.staminaIsVeryLow && doStateChange)
		{
			return Runner.ClimbSetup.none;
		}
		Vector2 vector3;
		Vector3 vector4;
		Range range;
		if (transitionFromScramble)
		{
			Vector3 point3d = Simulate.FindGroundPositionAtDistance(this.trackPosition, this.direction * this.settings.climb.climbFromScrambleCheckDist, Simulate.FindOptions.standardPredict, this.settings).sample.point3d;
			Vector3 vector = point3d + 0.2f * Vector3.up;
			Vector2 vector2 = 1f * this.direction * Vector2.right;
			if (!Raycast.CollideWallPolygonsVec3(vector, vector2, this.raycastCurrentRangeONLY, this.settings, false, null, false, default(Color)).didHit || Raycast.InvisibleCollision(this.position, vector + vector2, this.raycastCurrentRangeONLY, out vector3, true))
			{
				return Runner.ClimbSetup.none;
			}
			vector4 = point3d + this.settings.climb.climbStartHeightFromScramble * Vector3.up;
			range = this.raycastCurrentRangeONLY;
		}
		else
		{
			float num = ((climbRangeCheck == ClimbCheckRange.JumpOrFall) ? this.settings.climb.climbStartHeightFromMidAir : this.settings.climb.climbStartHeightFromIdle);
			vector4 = this.physicalPosition3d + num * Vector3.up;
			range = (closeOnly ? this.raycastCurrentAndNearForeRange : this.raycastNearbyRange);
		}
		Vector2 vector5 = (closeOnly ? this.settings.climb.climbAutoReachDist : this.settings.climb.climbMaxReachDist) * this.direction * Vector2.right;
		Raycast.Collision collision = Raycast.CollideWallPolygonsVec3(vector4, vector5, range, this.settings, false, null, false, default(Color));
		if (collision.didHit && Raycast.InvisibleCollision(this.position, collision.pos, Range.Centered(collision.poly.transform.position.z, 1f), out vector3, true))
		{
			return Runner.ClimbSetup.none;
		}
		if (!transitionFromScramble && this.TrySetupGrabLedge(climbRangeCheck, this.position, collision.didHit ? collision.poly : null, doStateChange))
		{
			return new Runner.ClimbSetup
			{
				upAndOver = true
			};
		}
		if (!collision.didHit || collision.unreachable)
		{
			return Runner.ClimbSetup.none;
		}
		if (this.prevState == Runner.State.WallSliding && this._wallSlideFallFromPoly != null && collision.poly == this._wallSlideFallFromPoly && Vector2.Distance(this._wallSlideFallFromPoint, collision.pos) < 5f)
		{
			return Runner.ClimbSetup.none;
		}
		this._climbing.poly = collision.poly;
		this._climbing.mainPos = collision.pos3d;
		this._climbing.normal = collision.normal;
		this._climbing.angle = Util.ClimbAngleFromSurfaceNormal(collision.normal, this.direction);
		this._climbing.reachProgress = 0f;
		if (this.animator.IsAnimation("Scamble", null, null, null))
		{
			this._climbOntoWall.transition = this.settings.transition.scrambleToClimb;
		}
		else if (this.IsGroundBasedState(this.state))
		{
			this._climbOntoWall.transition = this.settings.transition.climbOntoWall;
		}
		else
		{
			this._climbOntoWall.transition = this.settings.transition.climbOntoWallFromMidAir;
		}
		this._climbOntoWall.climbable = collision.climbable;
		if (doStateChange)
		{
			this.state = Runner.State.ClimbOntoWall;
		}
		return new Runner.ClimbSetup
		{
			ontoWall = true,
			physicalDepthLayerIdx = Mathf.RoundToInt(collision.poly.transform.position.z)
		};
	}

	// Token: 0x1700018A RID: 394
	// (get) Token: 0x06000656 RID: 1622 RVA: 0x000341E4 File Offset: 0x000323E4
	private PolySimulate.Options climbingDownIterateOpts
	{
		get
		{
			return new PolySimulate.Options
			{
				preventNonClimbable = false,
				preventDirectionChange = true,
				preventUnreachable = true,
				validAngleRange = new Range(Runner.instance.settings.run.maxGroundAngle, Runner.instance.settings.climb.maxOverhangAngle),
				slopePolyCheckDepthRange = this.raycastCurrentAndNearForeRange,
				runnerSettings = this.settings
			};
		}
	}

	// Token: 0x06000657 RID: 1623 RVA: 0x00034260 File Offset: 0x00032460
	private Runner.ClimbSetup TrySetupClimbingDown()
	{
		Poly poly;
		if (this.currentSlope != null)
		{
			poly = this.currentSlope.originPoly;
		}
		else
		{
			poly = Raycast.AnyPolyOccludes(this.position - 1f * Vector2.up, this.raycastSafeNearbyRange, null, null);
			if (poly == null)
			{
				return Runner.ClimbSetup.none;
			}
		}
		int num = 1;
		while (num == 1 || num == -1)
		{
			float num2 = (float)num * this.direction;
			Vector2 vector;
			if (!(this.currentSlope != null))
			{
				vector = this.position;
				goto IL_00BE;
			}
			Simulate.FindResult findResult = Simulate.FindGroundPositionAtDistance(this.trackPosition, num2 * this.settings.climb.climbDownEdgeDist, Simulate.FindOptions.standardSimulate, this.settings);
			if (!findResult.foundRequestedX)
			{
				vector = findResult.sample.point;
				goto IL_00BE;
			}
			IL_047B:
			num -= 2;
			continue;
			IL_00BE:
			PolySimulate.PointResult pointResult = PolySimulate.FindNearestPointOnPoly(poly, vector + 0.5f * num2 * Vector2.right, new Range(Runner.instance.settings.run.maxGroundAngle, 160f));
			if (!pointResult.found || Vector2.Distance(pointResult.point, this.position) > 3f || pointResult.normal.x * num2 < 0f || Raycast.AnyPolyOccludes(pointResult.point, this.raycastCurrentAndSafeForeRange, pointResult.poly, null) != null)
			{
				goto IL_047B;
			}
			vector = pointResult.point;
			vector.y -= 0.1f;
			PolySimulate.Result result = PolySimulate.AroundEdge(poly, vector, Vector2.down, this.settings.climb.climbDownStraightToGroundDist, this.climbingDownIterateOpts, null);
			Vector2 vector2;
			if (Raycast.InvisibleCollision(this.position, vector, Range.Centered(poly.transform.position.z, 1f), out vector2, true) || Raycast.InvisibleCollision(vector, result.position, Range.Centered(poly.transform.position.z, 1f), out vector2, true))
			{
				goto IL_047B;
			}
			if (result.endReason == PolySimulate.EndReason.SlopeIntersection)
			{
				Simulate.FindResult findResult2 = Simulate.FindGroundPositionAtDistance(new TrackPosition
				{
					slope = result.slopeHit,
					x = result.position.x
				}, 0.5f * num2, Simulate.FindOptions.standardSimulate, this.settings);
				this.ClimbDownToSlopeSetup(findResult2.sample.slope, findResult2.sample.point3d);
				return new Runner.ClimbSetup
				{
					downToSlope = true,
					direction = -num2,
					physicalDepthLayerIdx = Mathf.RoundToInt(findResult2.sample.depth),
					climbDownStartPoint = pointResult.point3d,
					downToSlopeDist = result.distanceTravelled
				};
			}
			if (!result.success)
			{
				Vector2 vector3 = result.position + new Vector2(0.5f * (float)num * this.direction, 0.5f);
				SlopeSample slopeSample;
				if (Raycast.SampleWithDepthRange(vector3, vector3 + 1f * Vector2.down, result.position3d.z, this.raycastNearbyRange, out slopeSample, default(Color)).didHit)
				{
					this.ClimbDownToSlopeSetup(slopeSample.slope, slopeSample.point3d);
					return new Runner.ClimbSetup
					{
						downToSlope = true,
						direction = -num2,
						physicalDepthLayerIdx = Mathf.RoundToInt(slopeSample.depth),
						climbDownStartPoint = pointResult.point3d,
						downToSlopeDist = result.distanceTravelled
					};
				}
				if (result.distanceTravelled < this.settings.climb.climbDownToHoldDist)
				{
					goto IL_047B;
				}
			}
			result = PolySimulate.AroundEdge(poly, vector, Vector2.down, this.settings.climb.climbDownToHoldDist, this.climbingDownIterateOpts, null);
			this._climbing.poly = poly;
			this._climbing.mainPos = result.position3d;
			this._climbing.normal = result.normal;
			this._climbing.angle = Util.ClimbAngleFromSurfaceNormal(result.normal, this.direction);
			this._climbOntoWall.transition = this.settings.transition.climbDownFromLedge;
			this._climbOntoWall.climbable = result.climbable;
			return new Runner.ClimbSetup
			{
				ontoWall = true,
				direction = -num2,
				physicalDepthLayerIdx = Mathf.RoundToInt(poly.transform.position.z)
			};
		}
		return Runner.ClimbSetup.none;
	}

	// Token: 0x06000658 RID: 1624 RVA: 0x000346FF File Offset: 0x000328FF
	private void SetupNextSlip()
	{
		this._climbing.timeToSlip = this.settings.climb.climbTimeToSlip.RandomBell(2);
	}

	// Token: 0x06000659 RID: 1625 RVA: 0x00034722 File Offset: 0x00032922
	private float RotationToClimbOnNormal(Vector2 normal)
	{
		return -this.direction * Mathf.Atan2(normal.y, -this.direction * normal.x) * 57.29578f;
	}

	// Token: 0x0600065A RID: 1626 RVA: 0x0003474C File Offset: 0x0003294C
	private void FallFromClimb(bool intentional)
	{
		AudioController.instance.PlayVocalisation(Vocalisation.ClimbSlipOrWallSlideFall, 0f);
		this._fallPreventsClimb = !intentional;
		Vector3 up = base.transform.up;
		Vector3 vector = this.direction * base.transform.right;
		Vector3 vector2 = this.animator.mouthPosition - 0.3f * vector;
		Vector3 vector3 = -this.settings.climb.headToFootRaycastLength * up;
		Raycast.Collision collision = Raycast.CollideWallPolygonsVec3(vector2, vector3, Range.Centered((float)this.physicalDepthLayerIdx, 2.2f), this.settings, false, null, false, default(Color));
		if (collision.didHit)
		{
			this.position = collision.pos + 0.5f * collision.normal;
			this.animator.SetAnimation("Fall", FrameAnimator.PosMatch.None);
			this.state = Runner.State.Falling;
			return;
		}
		SlopeSample slopeSample;
		if (Raycast.SampleWithDepthRange(vector2, vector2 + vector3, (float)this.physicalDepthLayerIdx, this.raycastNearbyRange, out slopeSample, default(Color)).didHit)
		{
			this.currentSlope = slopeSample.slope;
			this.position = slopeSample.point;
			this.physicalDepthLayerIdx = (int)slopeSample.depth;
			this.momentum = 0.2f * -this.direction;
			this.state = Runner.State.Running;
			return;
		}
		if (this._climbing.angle > this.settings.stamina.overhangStartAngle && !intentional)
		{
			this._fallIsTumble = true;
			this._tumbleIsBackwards = true;
		}
		this.state = Runner.State.Falling;
	}

	// Token: 0x0600065B RID: 1627 RVA: 0x000348F4 File Offset: 0x00032AF4
	private void State_ClimbingJump(bool start, bool end)
	{
		if (start)
		{
			this._climbingJump.startStamina = this.stamina;
			this._climbingJump.endStamina = Mathf.Max(this.stamina - this.settings.climb.climbingJumpStaminaCost, 0f);
			this._climbingJump.upward = this._climbingJump.target3d.y > this.position.y;
			this.StartTransitioningIntoPosition(this._climbingJump.target3d, this.settings.transition.climbingJump, "ClimbIdleRight", false, 1f);
			return;
		}
		if (end)
		{
			this._activeTransition = null;
			return;
		}
		if (!this.UpdateTransitionIntoPosition(this._climbingJump.target3d, this._climbingJump.targetRotation))
		{
			this.stamina = Mathf.Lerp(this._climbingJump.startStamina, this._climbingJump.endStamina, this.animator.normalizedTime);
			return;
		}
		this.stamina = this._climbingJump.endStamina;
		if (this._climbingJump.endsInUpAndOver)
		{
			this.state = Runner.State.ClimbUpAndOver;
			return;
		}
		if (this._climbingJump.endsInNonClimbable)
		{
			if (!this.TryStartWallSlideFromClimb(this._climbingJump.targetPoly, this._climbingJump.targetNormal, true))
			{
				if (!this._climbingJump.upward)
				{
					Vector2 vector = this.position - 1.5f * this.direction * Vector2.right + 1f * Vector2.up;
					SlopeSample slopeSample;
					if (Raycast.SampleWithDepthRange(vector, vector - 6f * Vector2.up, (float)this.physicalDepthLayerIdx, this.raycastSafeNearbyRange, out slopeSample, Color.red).didHit)
					{
						this.currentSlope = slopeSample.slope;
						this.position = slopeSample.point;
						this.physicalDepthLayerIdx = Mathf.RoundToInt(slopeSample.depth);
						this.state = Runner.State.Running;
						return;
					}
				}
				this.state = Runner.State.Falling;
				return;
			}
		}
		else
		{
			this._climbing.poly = this._climbingJump.targetPoly;
			this._climbing.reachRightHanded = true;
			this._climbing.mainPos = this._climbingJump.target3d;
			this.state = Runner.State.Climbing;
		}
	}

	// Token: 0x0600065C RID: 1628 RVA: 0x00034B40 File Offset: 0x00032D40
	private void TryJumpWhileClimbing(Vector2 directionIntentInput)
	{
		bool flag = directionIntentInput != Vector2.zero;
		float num = Vector2.Angle(Vector2.down, directionIntentInput);
		float num2 = Vector2.Angle(-this.direction * Vector2.right, directionIntentInput);
		bool flag2 = flag && num < 45f;
		bool flag3 = flag && num2 < 45f && !flag2;
		bool flag4 = !flag2 && !flag3;
		if (flag3)
		{
			this.TryJump(false);
			return;
		}
		Vector3 vector;
		Runner.ClimbJumpSearchStep climbJumpSearchStep;
		if (this.climbing)
		{
			if (flag4 && !this._climbing.lastUpperHoldValid)
			{
				if (this.FindClimbUpAndOver(this.position, (float)this.physicalDepthLayerIdx, ClimbCheckRange.Climb, this._climbing.poly, out this._climbUpAndOver))
				{
					this.state = Runner.State.ClimbUpAndOver;
					return;
				}
				Debug.LogWarning("Could not jump upward because we don't have a valid upper hold.");
				return;
			}
			else
			{
				if (flag2 && !this._climbing.lastLowerHoldValid)
				{
					Debug.LogWarning("Could not jump downward because we don't have a valid upper hold.");
					return;
				}
				vector = (flag4 ? this._climbing.lastUpperHold : this._climbing.lastLowerHold);
				climbJumpSearchStep = new Runner.ClimbJumpSearchStep
				{
					pos = vector,
					poly = this._climbing.poly,
					norm = this._climbing.normal
				};
			}
		}
		else
		{
			if (!this.wallSliding)
			{
				Debug.LogError("Expected to be climbing or wall sliding");
				return;
			}
			vector = this.position;
			climbJumpSearchStep = new Runner.ClimbJumpSearchStep
			{
				pos = vector,
				poly = this._wallSlide.poly,
				norm = this._wallSlide.normal
			};
		}
		float num3 = 0f;
		this._climbingJump.endsInUpAndOver = false;
		while (num3 < 7f)
		{
			Vector2 vector2 = -climbJumpSearchStep.norm;
			Vector2 vector3 = new Vector2(climbJumpSearchStep.norm.y, Mathf.Abs(climbJumpSearchStep.norm.x));
			if (flag2)
			{
				vector3 = -vector3;
			}
			if (flag4 && this.FindClimbUpAndOver(climbJumpSearchStep.pos, climbJumpSearchStep.poly.transform.position.z, ClimbCheckRange.Climb, climbJumpSearchStep.poly, out this._climbUpAndOver) && num3 < 3.5f)
			{
				this._climbingJump.endsInUpAndOver = true;
				break;
			}
			Vector3 vector4 = climbJumpSearchStep.pos - 0.5f * vector2;
			Vector2 vector5 = 1f * vector3;
			SlopeSample slopeSample;
			if (flag2 && Raycast.SampleWithDepthRange(vector4, vector4 + 3f * vector5, vector4.z, this.raycastNearbyRange, out slopeSample, default(Color)).didHit)
			{
				Vector2 vector6 = this.position - 1f * vector2 + 2f * vector3;
				if (vector6.y - 1f < slopeSample.point.y)
				{
					this.currentSlope = slopeSample.slope;
					this.position = slopeSample.point;
					return;
				}
				Vector2 vector7 = slopeSample.point;
				Slope slope = slopeSample.slope;
				Simulate.FindResult findResult = Simulate.FindGroundPositionAtDistance(new TrackPosition
				{
					slope = slopeSample.slope,
					x = slopeSample.point.x
				}, -2f * this.direction, Simulate.FindOptions.standardPredict, this.settings);
				if (findResult.foundRequestedX)
				{
					vector7 = findResult.sample.point;
					slope = findResult.sample.slope;
				}
				this.position = vector6;
				this.direction = -this.direction;
				this._jumpDirIntent = new Vector2(this.direction, 0f);
				if (vector7.y < this.position.y - 1f)
				{
					this.PerformJump_DropToPoint(vector7, slope, null, 0f);
					return;
				}
				this.PerformJump_Normal(vector7, findResult.sample.slope, null, this.settings.jump.jumpDurationStandard, 0.3f, false);
				return;
			}
			else
			{
				Raycast.Collision collision = Raycast.CollideWallPolygonsVec3(vector4, vector5, this.raycastCurrentAndSafeForeRange, this.settings, false, null, false, default(Color));
				if (collision.didHit)
				{
					if (!collision.climbable)
					{
						break;
					}
					climbJumpSearchStep.pos = collision.pos3d;
					climbJumpSearchStep.poly = collision.poly;
					climbJumpSearchStep.norm = collision.normal;
				}
				else
				{
					Vector3 vector8 = vector4 + vector5;
					Vector2 vector9 = 3f * vector2;
					Raycast.Collision collision2 = Raycast.CollideWallPolygonsVec3(vector8, vector9, this.raycastNearbyRange, this.settings, false, null, false, default(Color));
					if (!collision2.didHit)
					{
						break;
					}
					climbJumpSearchStep.pos = collision2.pos3d;
					climbJumpSearchStep.poly = collision2.poly;
					climbJumpSearchStep.norm = collision2.normal;
				}
				num3 = Vector2.Distance(vector, climbJumpSearchStep.pos);
			}
		}
		if (num3 < 1f)
		{
			if (this._climbingJump.endsInUpAndOver)
			{
				this.state = Runner.State.ClimbUpAndOver;
				return;
			}
			if (flag2)
			{
				this.position += 1f * this._climbing.normal;
				this.PerformJump_WithVelocity(new Vector2(0f, -4f * this.direction));
			}
			return;
		}
		else
		{
			float num4 = (flag4 ? this.settings.climb.climbUpperRayY : (-this.settings.climb.climbLowerRayY));
			Vector2 vector10 = new Vector2(climbJumpSearchStep.norm.y, Mathf.Abs(climbJumpSearchStep.norm.x));
			Raycast.Collision collision3 = Raycast.CollideWallPolygonsVec3(climbJumpSearchStep.pos - num4 * vector10 + 2f * climbJumpSearchStep.norm, -5f * climbJumpSearchStep.norm, this.settings.layer.layerCollideNearbyRange + climbJumpSearchStep.poly.transform.position.z, this.settings, false, null, false, default(Color));
			if (!collision3.didHit)
			{
				Debug.LogWarning("Failed to find main position collision when searching for final ClimbingJump position");
				return;
			}
			this._climbingJump.target3d = collision3.pos3d;
			this._climbingJump.targetPoly = collision3.poly;
			this._climbingJump.targetNormal = collision3.normal;
			this._climbingJump.targetRotation = this.RotationToClimbOnNormal(collision3.normal);
			this._climbingJump.endsInNonClimbable = !collision3.climbable;
			this.state = Runner.State.ClimbingJump;
			return;
		}
	}

	// Token: 0x0600065D RID: 1629 RVA: 0x00035220 File Offset: 0x00033420
	private void State_ClimbOffWall(bool start, bool end)
	{
		if (start)
		{
			this.rotation = Mathf.LerpAngle(this.rotation, 0f, 0.5f);
			this.animator.SetAnimationWithTransition("ClimbTurnToRun", "Run", 0, false, false, FrameAnimator.PosMatch.None);
			this.StartTransitioningIntoPosition(this._climbOffWall.targetPos, this.settings.transition.climbOffWall, "ClimbIdleRight", false, this._climbOffWall.speedScalar);
			return;
		}
		if (end)
		{
			this._activeTransition = null;
			return;
		}
		if (this.UpdateTransitionIntoPosition(this._climbOffWall.targetPos, 0f))
		{
			this.currentSlope = this._climbOffWall.targetSlope;
			if (this.prevState == Runner.State.WallSliding && this._wallSlide.speed > this.settings.wallSlide.speedForDamage)
			{
				this.health.ApplyDamage(DamageType.Fall, Damage.MinorDamage);
				this.state = Runner.State.HardLanding;
				return;
			}
			if (this.onSlide)
			{
				if (this.currentSlopeDownDir * this.direction > 0f)
				{
					this.momentum = 0.7f * this.direction;
				}
				else
				{
					this.momentum = 0f;
				}
				this.state = Runner.State.Sliding;
				return;
			}
			if (Simulate.FindGroundPositionAtDistance(this.trackPosition, 4f * this.direction, Simulate.FindOptions.standardSimulate, this.settings).foundRequestedX)
			{
				this.momentum = 0.2f * this.direction;
			}
			else
			{
				this.momentum = 0f;
			}
			this.state = Runner.State.Running;
		}
	}

	// Token: 0x0600065E RID: 1630 RVA: 0x000353A0 File Offset: 0x000335A0
	private void ClimbOffWallToSlope(Slope targetSlope, Vector2 targetPos, float jumpDistScalar)
	{
		if (this.state != Runner.State.Climbing && this.state != Runner.State.WallSliding)
		{
			Debug.LogError("Expected to be in climbing or wallSlide state when calling ClimbOffWall but we're in " + this.stateName);
		}
		this.direction = -this.direction;
		Range range = targetSlope.range;
		float num = range.Clamp(targetPos.x);
		float num2 = range.Clamp(num + jumpDistScalar * this.direction * this.settings.climb.climbOffRunDistDuringAnim);
		Simulate.FindResult findResult = Simulate.FindGroundPositionAtDistance(new TrackPosition
		{
			slope = targetSlope,
			x = num
		}, num2 - num, Simulate.FindOptions.standardSimulate, this.settings);
		float num3 = Vector2.Distance(this.position, findResult.sample.point3d);
		float num4 = this.settings.climb.climbOffDistRange.InverseLerp(num3);
		this._climbOffWall = new Runner.ClimbOffWall
		{
			targetPos = findResult.sample.point3d,
			targetSlope = targetSlope,
			speedScalar = Mathf.Lerp(1f, this.settings.climb.climbOffMinSpeedScalar, num4)
		};
		this.state = Runner.State.ClimbOffWall;
	}

	// Token: 0x0600065F RID: 1631 RVA: 0x000354D8 File Offset: 0x000336D8
	private void State_ClimbOntoWall(bool start, bool end)
	{
		if (start)
		{
			if (this._climbOntoWall.transition == null)
			{
				Debug.LogError("Expected _climbOntoWall.transition to be set before State_ClimbOntoWall entered");
				this._climbOntoWall.transition = this.settings.transition.climbOntoWall;
			}
			this.StartTransitioningIntoPosition(this._climbing.mainPos, this._climbOntoWall.transition, "ClimbIdleRight", false, 1f);
			return;
		}
		if (end)
		{
			this._activeTransition = null;
			return;
		}
		this.direction = -Mathf.Sign(this._climbing.normal.x);
		float num = this.RotationToClimbOnNormal(this._climbing.normal);
		if (this.UpdateTransitionIntoPosition(this._climbing.mainPos, num))
		{
			this._climbing.reachRightHanded = true;
			if (this._climbOntoWall.climbable)
			{
				this.state = Runner.State.Climbing;
				return;
			}
			if (this.TryStartWallSlideFromClimb(this._climbing.poly, this._climbing.normal, true))
			{
				return;
			}
			this.state = Runner.State.Falling;
		}
	}

	// Token: 0x06000660 RID: 1632 RVA: 0x000355D8 File Offset: 0x000337D8
	private void State_ClimbSlip(bool start, bool end)
	{
		if (start)
		{
			this._slipPrompt = MonoSingleton<GameUI>.instance.CreateQuickButtonPrompt();
			this._climbSlipRotation = this.rotation;
			this.animator.SetAnimationWithTransition("ClimbSlip", "ClimbSlipIdle", 0, false, false, FrameAnimator.PosMatch.None);
			this.animator.speed = 1f;
			AudioController.instance.PlayVocalisation(Vocalisation.ClimbingSlip, 0f);
			InputVibration.Medium();
			return;
		}
		if (end)
		{
			this._slipPrompt.Hide();
			this._slipPrompt = null;
			return;
		}
		if (this.animator.IsAnimation("ClimbSlipRecover", null, null, null))
		{
			float num = this.RotationToClimbOnNormal(this._climbing.normal);
			this.rotation = Mathf.MoveTowardsAngle(this.rotation, num, 180f * this._dt);
			if (this.animator.normalizedTime >= 1f && Mathf.DeltaAngle(this.rotation, num) < 1f)
			{
				this.rotation = num;
				AudioController.instance.PlayVocalisation(Vocalisation.ClimbingSlipRecover, 0f);
				this.state = Runner.State.Climbing;
			}
			return;
		}
		if (this._climbing.overhang)
		{
			float num2 = Mathf.DeltaAngle(this._climbSlipRotation, 0f) * this.settings.climb.slipSwingTorque;
			this._climbSlipRotationSpeed += num2 * this._dt;
			this._climbSlipRotationSpeed *= TimeX.Damping(this.settings.climb.slipSwingDamping, this._dt);
			this._climbSlipRotation += this._climbSlipRotationSpeed * this._dt;
			this.rotation = this._climbSlipRotation;
		}
		if (GameInput.slipRecover)
		{
			this.animator.SetAnimation("ClimbSlipRecover", FrameAnimator.PosMatch.None);
			AudioController.instance.PlayVocalisation(Vocalisation.ClimbEffortMajor, 0f);
			return;
		}
		if (this.stateTimer > this.settings.climb.slipTimeLimit)
		{
			this.FallFromClimb(false);
			return;
		}
	}

	// Token: 0x06000661 RID: 1633 RVA: 0x000357C4 File Offset: 0x000339C4
	private void State_ClimbUpAndOver(bool start, bool end)
	{
		if (start)
		{
			this._climbUpAndOver.startPos = base.transform.position;
			TransitionSettings.Transition transition = this._climbUpAndOver.transition ?? this.settings.transition.climbUpAndOver;
			bool flag = (this.prevState == Runner.State.Running && this.momentumAbs > 0.1f && this._climbUpAndOver.slope != null) || this.prevState == Runner.State.Jumping || this.prevState == Runner.State.Falling;
			if (flag)
			{
				this._climbUpAndOver.momentumAfter = this.momentum;
			}
			else
			{
				this._climbUpAndOver.momentumAfter = 0f;
			}
			if (transition == this.settings.transition.bridgeGap)
			{
				float num = Mathf.Max(this._prevVelocity.magnitude, 0.8f * this.settings.run.maxStandardSpeed);
				this._climbUpAndOver.simpleLerpDuration = (this._climbUpAndOver.targetPos - this._climbUpAndOver.startPos).magnitude / num;
			}
			else
			{
				this._climbUpAndOver.simpleLerpDuration = -1f;
			}
			this.StartTransitioningIntoPosition(this._climbUpAndOver.targetPos, transition, flag ? "Run" : "Idle", flag, 1f);
		}
		if (end)
		{
			this._climbUpAndOver = default(Runner.ClimbUpAndOver);
			this._activeTransition = null;
			this.MarkCurrentSampleDirty();
			return;
		}
		if (this._climbUpAndOver.simpleLerpDuration >= 0f)
		{
			float num2 = ((this._climbUpAndOver.simpleLerpDuration <= 0.02f) ? 1f : ((this.stateTimer + 0.016f) / this._climbUpAndOver.simpleLerpDuration));
			if (num2 > 1f)
			{
				num2 = 1f;
			}
			base.transform.position = Vector3.Lerp(this._climbUpAndOver.startPos, this._climbUpAndOver.targetPos, num2);
			if (num2 >= 1f)
			{
				this.animator.ForceCompleteTransition();
				this.CompleteUpAndOverTransitionToState();
				return;
			}
		}
		else if (this.UpdateTransitionIntoPosition(this._climbUpAndOver.targetPos, 0f))
		{
			this.CompleteUpAndOverTransitionToState();
		}
	}

	// Token: 0x06000662 RID: 1634 RVA: 0x000359F0 File Offset: 0x00033BF0
	private void CompleteUpAndOverTransitionToState()
	{
		History.Log("CompleteUpAndOverTransitionToState");
		if (this._climbUpAndOver.slope != null)
		{
			this.currentSlope = this._climbUpAndOver.slope;
			this.momentum = this._climbUpAndOver.momentumAfter;
			this.state = Runner.State.Running;
			return;
		}
		if (this._climbUpAndOver.balancePoint != null)
		{
			this._balancePoint = this._climbUpAndOver.balancePoint;
			this.state = Runner.State.Balancing;
			return;
		}
		Debug.LogError("No slope or balance point was set for ClimbUpAndOver to transition to!");
		this.state = Runner.State.Falling;
	}

	// Token: 0x06000663 RID: 1635 RVA: 0x00035A84 File Offset: 0x00033C84
	[NullableContext(1)]
	private void ClimbDownToSlopeSetup(Slope slope, Vector3 targetPoint)
	{
		float num = targetPoint.y - this.position.y;
		this._climbUpAndOver.transition = ((num < -5f) ? this.settings.transition.clamberDown2m : this.settings.transition.clamberDown0_5m);
		this._climbUpAndOver.targetPos = targetPoint;
		this._climbUpAndOver.slope = slope;
		this._climbUpAndOver.balancePoint = null;
	}

	// Token: 0x06000664 RID: 1636 RVA: 0x00035B00 File Offset: 0x00033D00
	private bool IsInUpAndOverTrapezoid(Vector2 targetPoint, Vector2 playerPos, Range xRange, Range yRange)
	{
		float num = targetPoint.y - playerPos.y;
		if (!yRange.Contains(num))
		{
			return false;
		}
		float num2 = yRange.InverseLerp(num);
		float num4;
		float num3 = (num4 = xRange.Lerp(num2));
		if (this.direction > 0f)
		{
			num4 *= this.settings.upAndOver.xRangeBackScalar;
		}
		else
		{
			num3 *= this.settings.upAndOver.xRangeBackScalar;
		}
		float num5 = targetPoint.x - playerPos.x;
		if (num5 > 0f)
		{
			if (num5 > num3)
			{
				return false;
			}
		}
		else if (num5 < -num4)
		{
			return false;
		}
		return true;
	}

	// Token: 0x06000665 RID: 1637 RVA: 0x00035B98 File Offset: 0x00033D98
	[NullableContext(2)]
	private bool FindClimbUpAndOver(Vector2 pos, float physicalDepth, ClimbCheckRange climbRange, Poly specificPoly, out Runner.ClimbUpAndOver upAndOver)
	{
		upAndOver = default(Runner.ClimbUpAndOver);
		bool flag = false;
		float num = float.MaxValue;
		Range range = ((climbRange == ClimbCheckRange.GroundAutoClimb) ? this.raycastCurrentAndSafeForeRange : this.raycastNearbyRange);
		bool flag2 = this.direction > 0f;
		Range range2;
		Range range3;
		if (climbRange == ClimbCheckRange.Climb)
		{
			range2 = this.settings.upAndOver.xRangeClimb;
			range3 = this.settings.upAndOver.yRangeClimb;
		}
		else if (climbRange == ClimbCheckRange.JumpOrFall)
		{
			range2 = this.settings.upAndOver.xRangeJumpFall;
			range3 = this.settings.upAndOver.yRangeJumpFall;
		}
		else if (climbRange == ClimbCheckRange.Ground)
		{
			range2 = this.settings.upAndOver.xRangeGround;
			range3 = this.settings.upAndOver.yRangeGround;
		}
		else if (climbRange == ClimbCheckRange.GroundAutoClimb)
		{
			range2 = this.settings.upAndOver.xRangeGroundAuto;
			range3 = this.settings.upAndOver.yRangeGroundAuto;
		}
		else
		{
			Debug.LogError("Undefined climb range enum: " + climbRange.ToString());
			range2 = this.settings.upAndOver.xRangeGround;
			range3 = this.settings.upAndOver.yRangeGround;
		}
		float num2 = this.direction;
		float num3 = -this.direction;
		Vector3 vector = new Vector3(pos.x, pos.y, physicalDepth);
		vector + new Vector3(num2 * range2.min, range3.min, 0f);
		vector + new Vector3(num2 * range2.max, range3.max, 0f);
		vector + new Vector3(num3 * this.settings.upAndOver.xRangeBackScalar * range2.max, range3.max, 0f);
		vector + new Vector3(num3 * this.settings.upAndOver.xRangeBackScalar * range2.min, range3.min, 0f);
		foreach (Slope slope in Level.current.slopes.Nearby(pos, range, 15f, null))
		{
			if ((!(specificPoly != null) || !(slope.originPoly != specificPoly)) && !(slope == this.currentSlope) && !(slope == this.lastSlopeJumpedFrom))
			{
				Vector3 vector2;
				if (flag2)
				{
					if (slope.connectedLeft)
					{
						continue;
					}
					vector2 = slope.leftPoint;
				}
				else
				{
					if (slope.connectedRight)
					{
						continue;
					}
					vector2 = slope.rightPoint;
				}
				if (this.IsInUpAndOverTrapezoid(vector2, pos, range2, range3))
				{
					if (slope.isSlide)
					{
						float num4 = (flag2 ? slope.leftSample.angle : slope.rightSample.angle);
						if (flag2)
						{
							num4 *= -1f;
						}
						if (Mathf.Abs(num4) > this.settings.run.slideMinAngle)
						{
							continue;
						}
					}
					Vector3 vector3 = new Vector3(vector2.x + 0.2f * this.direction, vector2.y + 0.2f, vector2.z);
					Range range4 = this.settings.layer.layerCollideCurrentRange + vector2.z;
					if (!Raycast.CollideWallPolygonsVec3(vector3, new Vector2(-this.direction, 0f), range4, this.settings, false, null, false, default(Color)).didHit)
					{
						Vector3 point3d = slope.SampleAt(vector2.x + 0.5f * this.direction, false).point3d;
						Vector2 vector4;
						if (!Raycast.InvisibleCollision(pos, point3d, range4, out vector4, true))
						{
							float num5 = Vector2.Distance(point3d, pos);
							if (!flag || num5 < num)
							{
								num = num5;
								upAndOver = new Runner.ClimbUpAndOver
								{
									targetPos = point3d,
									slope = slope
								};
								flag = true;
							}
						}
					}
				}
			}
		}
		float num6 = 1.5f * (range2.max / range3.max);
		range3.max += 1.5f;
		range2.max += num6;
		foreach (BalancePoint balancePoint in Level.current.balancePoints.Nearby(pos, range, 20f, null))
		{
			if (!balancePoint.exitToSlope && !(balancePoint == this._prevBalancePoint))
			{
				Vector3 position = balancePoint.transform.position;
				if (this.IsInUpAndOverTrapezoid(position, pos, range2, range3))
				{
					float num7 = Vector2.Distance(position, pos);
					if (!flag || num7 < num + 1.5f)
					{
						num = num7;
						upAndOver = new Runner.ClimbUpAndOver
						{
							targetPos = position,
							balancePoint = balancePoint
						};
						flag = true;
					}
				}
			}
		}
		return flag;
	}

	// Token: 0x06000666 RID: 1638 RVA: 0x000360DC File Offset: 0x000342DC
	[NullableContext(2)]
	private bool TrySetupGrabLedge(ClimbCheckRange climbCheckRange, Vector2 fromPosition, Poly specificPoly = null, bool doStateChange = true)
	{
		if (this.jumping)
		{
			if (this.isMusicRunning && this._jump.targetSlope != null)
			{
				return false;
			}
			bool flag = this.jumping && this.stateTimer < 0.25f * this._jump.expectedDuration;
			if (climbCheckRange == ClimbCheckRange.JumpOrFall && this.velocity.y > 0f && !flag)
			{
				return false;
			}
			if (this._jump.targetBalancePoint != null)
			{
				return false;
			}
		}
		if (!this.FindClimbUpAndOver(fromPosition, (float)this.physicalDepthLayerIdx, climbCheckRange, specificPoly, out this._climbUpAndOver))
		{
			return false;
		}
		float y = (this._climbUpAndOver.targetPos - fromPosition).y;
		if (y < -0.5f)
		{
			return false;
		}
		this._climbUpAndOver.transition = this.settings.transition.climbUpAndOver;
		if (y < 1f)
		{
			this._climbUpAndOver.transition = this.settings.transition.bridgeGap;
		}
		else if (y < 2f)
		{
			this._climbUpAndOver.transition = this.settings.transition.clamber0_5m;
		}
		else if (y < 5f)
		{
			this._climbUpAndOver.transition = this.settings.transition.clamber1m;
		}
		else
		{
			this._climbUpAndOver.transition = this.settings.transition.clamber2m;
		}
		if (doStateChange)
		{
			this.state = Runner.State.ClimbUpAndOver;
		}
		return true;
	}

	// Token: 0x06000667 RID: 1639 RVA: 0x0003625C File Offset: 0x0003445C
	private void State_Death(bool start, bool end)
	{
		if (start)
		{
			if (this.prevState == Runner.State.Falling)
			{
				this.animator.SetAnimationWithTransition("FallToCollapsed", "Collapsed", 0, false, false, FrameAnimator.PosMatch.None);
			}
			else
			{
				this.animator.SetAnimation("Collapsed", FrameAnimator.PosMatch.None);
			}
			this.rotation = this.currentSample.angle;
			this.momentum = 0f;
			return;
		}
	}

	// Token: 0x06000668 RID: 1640 RVA: 0x000362C0 File Offset: 0x000344C0
	public void Die(bool forceFast = false)
	{
		History.Log("DEATH!");
		this.state = Runner.State.Dead;
		bool flag = forceFast || this.health.causeOfDeath == DamageType.Fall;
		Game.instance.StartDeath(flag);
	}

	// Token: 0x06000669 RID: 1641 RVA: 0x000362FF File Offset: 0x000344FF
	public void Resurrect()
	{
		this.health.ResetToMax();
		this.stamina = 1f;
		this.ResetToLastSafePosition(false, false);
		this.state = Runner.State.Running;
	}

	// Token: 0x0600066A RID: 1642 RVA: 0x00036328 File Offset: 0x00034528
	private void State_DebugFlying(bool start, bool end)
	{
		SlopeSample slopeSample;
		Raycast.RaycastResult raycastResult = Raycast.SampleAll(this.position, this.position + 40f * Vector2.down, true, out slopeSample, default(Color));
		if (start)
		{
			this._debugFlyVelocity = 20f * Vector2.up;
			this.currentSlope = null;
			this.momentum = 0f;
			this.visualDepth = (float)this.physicalDepthLayerIdx;
			return;
		}
		if (end)
		{
			this._debugFlyVelocity = Vector2.zero;
			if (raycastResult.didHit)
			{
				this.visualDepth = slopeSample.depth;
			}
			this.physicalDepthLayerIdx = Mathf.RoundToInt(this.visualDepth);
			return;
		}
		if (raycastResult.didHit)
		{
			this.visualDepth = slopeSample.depth;
		}
		this._debugFlyVelocity += new Vector3(this.settings.flySpeed * this._debugFlyInput.x, this.settings.flySpeed * this._debugFlyInput.y, this.settings.flyDepthSpeed * this._debugFlyInput.z) * this._dt;
		this._debugFlyVelocity *= TimeX.Damping(this.settings.flyDamping);
		this.position += this._debugFlyVelocity * this._dt;
		this.visualDepth += this._debugFlyVelocity.z * this._dt;
	}

	// Token: 0x0600066B RID: 1643 RVA: 0x000364C0 File Offset: 0x000346C0
	public void EnterExitDoor(bool enter)
	{
		bool flag = this.state == Runner.State.EnterExitDoor && this._enterExitDoorIsEnter;
		bool flag2 = this.state == Runner.State.EnterExitDoor && !this._enterExitDoorIsEnter;
		if (enter && (this.hidden || flag))
		{
			return;
		}
		if (!enter && (!this.hidden || flag2))
		{
			return;
		}
		this._enterExitDoorIsEnter = enter;
		this.state = Runner.State.EnterExitDoor;
	}

	// Token: 0x0600066C RID: 1644 RVA: 0x00036528 File Offset: 0x00034728
	private void State_EnterExitDoor(bool start, bool end)
	{
		if (start)
		{
			if (!this._enterExitDoorIsEnter)
			{
				this.alpha = 0f;
			}
			this.animator.SetAnimation(this._enterExitDoorIsEnter ? "EnterDoor" : "ExitDoor", FrameAnimator.PosMatch.None);
			return;
		}
		if (end)
		{
			this.alpha = (float)(this._enterExitDoorIsEnter ? 0 : 1);
			return;
		}
		float normalizedTime = this.animator.normalizedTime;
		if (this._enterExitDoorIsEnter)
		{
			this.alpha = 1f - normalizedTime;
		}
		else
		{
			this.alpha = Mathf.InverseLerp(0f, 0.5f, normalizedTime);
		}
		if (normalizedTime >= 1f)
		{
			if (this._enterExitDoorIsEnter)
			{
				this.SetHidden(true, HideReason.Ink);
				return;
			}
			if (this.currentSlope == null)
			{
				Debug.LogError("currentSlope was null when exiting a door. Why?");
				SlopeSample slopeSample = Raycast.FindBestNearbySlopeSample(Level.current, this.physicalPosition3d, false, 3f);
				this.currentSlope = slopeSample.slope;
				this.position = slopeSample.point;
			}
			this.state = Runner.State.Running;
		}
	}

	// Token: 0x0600066D RID: 1645 RVA: 0x00036623 File Offset: 0x00034823
	public void EnterPath(bool isAway, int xDir, Vector3 entryPathOrigin, Prop.PathAnimType pathAnimType)
	{
		this._pathOrigin = entryPathOrigin;
		this._pathExitSlope = null;
		this._pathIsEnter = true;
		this._pathIsAway = isAway;
		this._pathAnimType = pathAnimType;
		this.direction = (float)xDir;
		this.state = Runner.State.EnterExitPath;
	}

	// Token: 0x0600066E RID: 1646 RVA: 0x0003665C File Offset: 0x0003485C
	public void ExitPath(bool isAway, int xDir)
	{
		if (this.state == Runner.State.EnterExitPath)
		{
			this.state = Runner.State.Hidden;
		}
		this._pathExitSlope = this.currentSlope;
		this._pathOrigin = this.physicalPosition3d;
		this._pathIsEnter = false;
		this._pathIsAway = isAway;
		this.direction = (float)xDir;
		this.alpha = 0f;
		this.state = Runner.State.EnterExitPath;
	}

	// Token: 0x0600066F RID: 1647 RVA: 0x000366BC File Offset: 0x000348BC
	private void State_EnterExitPath(bool start, bool end)
	{
		if (start)
		{
			if (this._pathAnimType == Prop.PathAnimType.RopeClimbDown || this._pathAnimType == Prop.PathAnimType.RopeClimbUp || this._pathAnimType == Prop.PathAnimType.StairClimbUp)
			{
				Vector2 position = this.position;
				position.x = this._pathOrigin.x;
				this.position = position;
			}
			this._enterExitPathStartPos = this.physicalPosition3d;
			if (this._pathIsEnter)
			{
				if (this._pathAnimType == Prop.PathAnimType.Normal)
				{
					if (this._pathIsAway)
					{
						this.animator.SetAnimationWithTransition("EnterPathAway", "PathAwayLoop", 0, false, false, FrameAnimator.PosMatch.Mouth);
					}
					else
					{
						this.animator.SetAnimationWithTransition("EnterPathToward", "PathTowardLoop", 0, false, false, FrameAnimator.PosMatch.Mouth);
					}
				}
				else if (this._pathAnimType == Prop.PathAnimType.RopeClimbUp)
				{
					this.animator.SetAnimation("RopeClimbUpEnter", FrameAnimator.PosMatch.None);
				}
				else if (this._pathAnimType == Prop.PathAnimType.RopeClimbDown)
				{
					this.animator.SetAnimation("RopeClimbDown", FrameAnimator.PosMatch.None);
				}
				else if (this._pathAnimType == Prop.PathAnimType.StairClimbDown || this._pathAnimType == Prop.PathAnimType.StairClimbUp)
				{
					this.animator.SetAnimationWithTransition("EnterPathAway", "PathAwayLoop", 0, false, false, FrameAnimator.PosMatch.Mouth);
				}
				else if (this._pathAnimType == Prop.PathAnimType.BellyWriggle)
				{
					this.animator.SetAnimationWithTransition("BellyWriggleInto", "BellyWriggle", 0, false, false, FrameAnimator.PosMatch.None);
				}
				this.currentSlope = null;
			}
			else
			{
				if (this._pathAnimType == Prop.PathAnimType.Normal)
				{
					if (this._pathIsAway)
					{
						this.animator.SetAnimation("ExitPathAway", FrameAnimator.PosMatch.None);
					}
					else
					{
						this.animator.SetAnimation("ExitPathToward", FrameAnimator.PosMatch.None);
					}
				}
				else if (this._pathAnimType == Prop.PathAnimType.RopeClimbUp)
				{
					this.animator.SetAnimation("RopeClimbUpExit", FrameAnimator.PosMatch.None);
				}
				else if (this._pathAnimType == Prop.PathAnimType.RopeClimbDown)
				{
					this.animator.SetAnimationWithTransition("RopeClimbDownToFall", "Fall", 0, false, false, FrameAnimator.PosMatch.None);
				}
				else if (this._pathAnimType == Prop.PathAnimType.StairClimbDown || this._pathAnimType == Prop.PathAnimType.StairClimbUp)
				{
					this.animator.SetAnimation("ExitPathToward", FrameAnimator.PosMatch.None);
				}
				else if (this._pathAnimType == Prop.PathAnimType.BellyWriggle)
				{
					this.animator.SetAnimation("BellyWriggleOut", FrameAnimator.PosMatch.None);
				}
				if (this._pathAnimType == Prop.PathAnimType.RopeClimbDown)
				{
					base.transform.position = this._pathOrigin + 5f * Vector3.up;
				}
				else
				{
					Vector2 remainingRootMotion = this.animator.remainingRootMotion;
					base.transform.position = this._pathOrigin - remainingRootMotion;
				}
				this.MarkCurrentSampleDirty();
				this.physicalDepthLayerIdx = Mathf.RoundToInt(this._pathOrigin.z);
				this.playerControlDisabled |= PlayerControlDisableReason.FollowPath;
			}
			this._animatorExpectedWorldPos = this.position;
			FrameAnimator frameAnimator = this.animator;
			frameAnimator.onRootMotion = (FrameAnimator.OnRootMotionDelegate)Delegate.Combine(frameAnimator.onRootMotion, new FrameAnimator.OnRootMotionDelegate(this.OnAnimationRootMotionFollowPath));
			return;
		}
		if (end)
		{
			if (!this._pathIsEnter)
			{
				this.visualDepth = (float)this.physicalDepthLayerIdx;
				this.playerControlDisabled &= ~PlayerControlDisableReason.FollowPath;
				this.torch.SetAlpha(1f);
			}
			this._pathExitSlope = null;
			FrameAnimator frameAnimator2 = this.animator;
			frameAnimator2.onRootMotion = (FrameAnimator.OnRootMotionDelegate)Delegate.Remove(frameAnimator2.onRootMotion, new FrameAnimator.OnRootMotionDelegate(this.OnAnimationRootMotionFollowPath));
			this.MarkCurrentSampleDirty();
			return;
		}
		if (this._pathIsEnter)
		{
			this.torch.SetAlpha(1f - Mathf.Clamp01(this.stateTimer / 1f));
			if (this.stateTimer > 2f)
			{
				this.SetHidden(true, HideReason.FollowPath);
				return;
			}
		}
		else
		{
			this.torch.SetAlpha(Mathf.Clamp01(this.stateTimer / 1f));
			if (this._pathAnimType == Prop.PathAnimType.RopeClimbDown)
			{
				if (this.animator.IsAnimation("Fall", null, null, null))
				{
					this.alpha = 1f;
					this.state = Runner.State.Falling;
					return;
				}
			}
			else if (this.animator.normalizedTime >= 1f)
			{
				this.currentSlope = this._pathExitSlope;
				base.transform.position = this._pathOrigin;
				this.state = Runner.State.Running;
				return;
			}
		}
		float num = 0f;
		if (this._pathAnimType == Prop.PathAnimType.Normal)
		{
			num = Mathf.Clamp01(this._pathIsEnter ? this.stateTimer : (1f - this.stateTimer)) * ((this._pathIsAway ^ !this._pathIsEnter) ? 1f : (-1f));
		}
		else if (this._pathAnimType == Prop.PathAnimType.StairClimbUp || this._pathAnimType == Prop.PathAnimType.StairClimbDown)
		{
			num = Mathf.Clamp01(this._pathIsEnter ? this.stateTimer : (1f - this.stateTimer)) * 1f;
		}
		if (this._pathAnimType == Prop.PathAnimType.RopeClimbDown)
		{
			float num2 = Mathf.InverseLerp(0f, 1f, this.stateTimer);
			if (this._pathIsEnter)
			{
				num = -0.5f;
			}
			else
			{
				num = Mathf.Lerp(-0.5f, 0f, num2);
			}
		}
		else if (this._pathAnimType == Prop.PathAnimType.RopeClimbUp)
		{
			float num3 = Mathf.InverseLerp(0f, 1f, this.stateTimer);
			if (this._pathIsEnter)
			{
				num = Mathf.Lerp(0f, -0.5f, num3);
			}
			else
			{
				num = -0.5f;
			}
		}
		Vector3 vector = this._animatorExpectedWorldPos;
		vector.z = this._pathOrigin.z + num;
		if (this._pathAnimType == Prop.PathAnimType.StairClimbUp && this._pathIsEnter)
		{
			vector.y = this._enterExitPathStartPos.y + 4f * Mathf.InverseLerp(0.4f, 2f, this.stateTimer);
		}
		else if (this._pathAnimType == Prop.PathAnimType.StairClimbDown && !this._pathIsEnter)
		{
			vector.y = this._pathOrigin.y + 4f * (1f - Mathf.InverseLerp(0f, 1f, this.stateTimer));
		}
		if (vector.y < 0f)
		{
			vector.y = 0f;
		}
		base.transform.position = vector;
		float num4;
		if (this._pathIsEnter)
		{
			num4 = 1f - Mathf.InverseLerp(1f, 2f, this.stateTimer);
		}
		else
		{
			num4 = Mathf.InverseLerp(0f, 1f, this.stateTimer);
		}
		this.alpha = num4;
	}

	// Token: 0x06000670 RID: 1648 RVA: 0x00036CC2 File Offset: 0x00034EC2
	private void OnAnimationRootMotionFollowPath(Vector2 motion, bool worldSpace)
	{
		if (!worldSpace)
		{
			motion.x *= this.direction;
		}
		this._animatorExpectedWorldPos += motion;
	}

	// Token: 0x06000671 RID: 1649 RVA: 0x00036CEC File Offset: 0x00034EEC
	public void Explode(string destinationName)
	{
		this._explosionSource = this.physicalPosition3d;
		this._explosionVelocity = this.settings.explosion.speed * Vector3.up;
		this._explosionDestination = Prop.FindNearestByInkName(this.physicalPosition3d, destinationName);
		if (this._explosionDestination == null)
		{
			Debug.LogError("Explosion destination not found: " + destinationName);
			return;
		}
		this.state = Runner.State.Exploded;
	}

	// Token: 0x06000672 RID: 1650 RVA: 0x00036D60 File Offset: 0x00034F60
	private void State_Exploded(bool start, bool end)
	{
		if (start)
		{
			this._currentSlope = null;
			this.MarkCurrentSampleDirty();
			this.rotation = this.settings.explosion.initialRotation;
			this.animator.SetAnimation("Exploded", FrameAnimator.PosMatch.None);
			AudioController.instance.PlayVocalisation(Vocalisation.AlarmAtFallDistance, 0f);
			InputVibration.Large();
			return;
		}
		if (end)
		{
			this._explosionHasTeleported = false;
			this._explosionDestination = null;
			return;
		}
		if (this.stateTimer < this.settings.explosion.upwardDuration)
		{
			this._explosionVelocity *= TimeX.Damping(this.settings.explosion.deceleration, this._dt);
			base.transform.position += this._explosionVelocity * this._dt;
			this.rotation += this.settings.explosion.rotationSpeed * this._dt;
			return;
		}
		if (!this._explosionHasTeleported)
		{
			this._explosionHasTeleported = true;
			Vector3 destinationPos = this._explosionDestination.transform.position;
			Game.instance.StartCoroutine(Game.instance.TeleportPlayerTo3DCR(this._explosionDestination.transform.position, "Player exploded", 0, true, null, true, false, null, delegate
			{
				this.currentSlope = null;
				this.MarkCurrentSampleDirty();
				this.transform.position = destinationPos + this.settings.explosion.destinationFallHeight * Vector3.up;
				this.UpdateDepth(true);
				this.state = Runner.State.Falling;
				this.EscalateFallDamage(Damage.MajorDamage);
				this.UpdateFocus();
				GameCamera.instance.Refresh(true);
			}));
		}
	}

	// Token: 0x06000673 RID: 1651 RVA: 0x00036ECC File Offset: 0x000350CC
	private void State_Falling(bool start, bool end)
	{
		if (start)
		{
			this._fallVelocity = this._prevVelocity;
			this.currentSlope = null;
			this._fallLastTouchY = this.position.y;
			this._fallStartY = this.position.y;
			if (this.prevState == Runner.State.Jumping)
			{
				this._fallLastTouchY = this._jump.startPos.y;
			}
			FrameAnimator.PosMatch posMatch = FrameAnimator.PosMatch.Mouth;
			if (this.prevState == Runner.State.Jumping || this.prevState == Runner.State.Running || this.prevState == Runner.State.Balancing)
			{
				posMatch = FrameAnimator.PosMatch.None;
			}
			if (!this._fallIsTumble)
			{
				if (this.prevState == Runner.State.Running && !this.stoppedBasically)
				{
					this.animator.SetAnimationWithTransition("RunToFall", "Fall", 0, false, false, FrameAnimator.PosMatch.None);
				}
				else if (this.prevState == Runner.State.Caught)
				{
					this.animator.SetAnimationWithTransition("EagleDrop", "Fall", 0, false, false, FrameAnimator.PosMatch.Mouth);
				}
				else
				{
					this.animator.SetAnimation("Fall", posMatch);
				}
			}
			if (this._fallIsTumble)
			{
				float num = -this.direction;
				if (this._tumbleIsBackwards)
				{
					num = -num;
				}
				this.RandomiseTumbleSpeed(num, 0.3f);
			}
			return;
		}
		if (end)
		{
			this.ResetFallDamage();
			this._fallPreventsClimb = false;
			this._fallIsTumble = false;
			this._fallPosHistory.Clear();
			return;
		}
		if ((double)this.stateTimer > 0.25 && DebugOptions.opts.pauseOnFall)
		{
			Debug.Break();
		}
		if (this._fallIsTumble)
		{
			this.animator.SetAnimation("Tumble", FrameAnimator.PosMatch.Mouth);
		}
		else
		{
			this.animator.SetAnimation("Fall", FrameAnimator.PosMatch.Mouth);
		}
		this._fallVelocity.y = this._fallVelocity.y + this.settings.standardGravity * this._dt;
		float num2 = this._fallVelocity.magnitude;
		if (num2 > this.settings.fall.fallTerminalSpeed)
		{
			float num3 = this.settings.fall.fallTerminalSpeed / num2;
			this._fallVelocity = num3 * this._fallVelocity;
			num2 = this.settings.fall.fallTerminalSpeed;
		}
		float num4 = this._fallLastTouchY - this.position.y;
		Damage damage = Damage.None;
		if (num4 > this.settings.fall.fallHeightDeath)
		{
			damage = Damage.Death;
		}
		else if (num4 > this.settings.fall.fallHeightMajor)
		{
			damage = Damage.MajorDamage;
		}
		else if (num4 > this.settings.fall.fallHeightMinor)
		{
			damage = Damage.MinorDamage;
		}
		if (this.prevState == Runner.State.Caught && damage > Damage.MinorDamage)
		{
			damage = Damage.MinorDamage;
		}
		if (damage > this._currentFallDamage)
		{
			this.EscalateFallDamage(damage);
			this._fallPreventsClimb = true;
			if (this._currentFallDamage == Damage.MinorDamage)
			{
				AudioController.instance.PlayVocalisation(Vocalisation.AlarmAtFallDistance, 0f);
			}
		}
		Vector2 vector = this._fallVelocity * this._dt;
		Vector2 vector2 = this.position + vector;
		if (this._maxFallDamage == Damage.None && !this._fallIsTumble && this.health.currentHealth > 0f)
		{
			if (this.TrySetupGrabLedge(ClimbCheckRange.JumpOrFall, vector2, null, true))
			{
				return;
			}
			if (this.TryLandOnBalancePointOrFall(this.position, vector2))
			{
				return;
			}
		}
		Runner.FallSlopeCollision fallSlopeCollision = this.TryFallCollideWithSlope(this.position + new Vector2(0f, 0.01f), vector);
		if (fallSlopeCollision.changedState)
		{
			return;
		}
		if (fallSlopeCollision.didBounce)
		{
			vector2 = fallSlopeCollision.bouncePositionAfter;
		}
		Vector2 vector3;
		if (Raycast.InvisibleCollision(this.position, vector2, this.raycastCurrentRangeONLY, out vector3, false))
		{
			this._fallVelocity.x = 0f;
			vector2.x = vector3.x;
		}
		Runner.FallCollision fallCollision = this.JumpFallCollideWithPolyWalls(vector2, this.raycastCurrentAndSafeForeRange, true);
		if (fallCollision.collided && this.state != Runner.State.Falling)
		{
			return;
		}
		if (fallCollision.collided && fallCollision.foundGulleySlope != null)
		{
			Slope foundGulleySlope = fallCollision.foundGulleySlope;
			Vector2 point = foundGulleySlope.SampleAt(this.position.x, false).point;
			point.x = foundGulleySlope.range.Clamp(point.x);
			this.FallDidLandOnSlope(foundGulleySlope, point, false);
			return;
		}
		if (fallCollision.collided || fallSlopeCollision.didBounce)
		{
			this._fallLastTouchY = this.position.y;
		}
		if ((fallCollision.collided || fallSlopeCollision.didBounce) && this._fallIsTumble)
		{
			this.RandomiseTumbleSpeed(-Mathf.Sign(fallCollision.normal.x), 1f);
		}
		if (fallCollision.collided || fallSlopeCollision.didBounce)
		{
			if (this._maxFallDamage == Damage.None)
			{
				InputVibration.Small();
			}
			else
			{
				InputVibration.Medium();
			}
		}
		float num5 = Mathf.Sign(this._fallVelocity.x);
		this.momentum = num5 * this.momentumAbs;
		if (this._fallIsTumble && this.animator.IsAnimation("Tumble", null, null, null))
		{
			this.rotation += this._tumbleSpeed * this._dt;
		}
		else
		{
			this.rotation = Mathf.MoveTowardsAngle(this.rotation, 0f, 180f * this._dt / 1f);
		}
		if (!fallCollision.collided)
		{
			this.position = vector2;
		}
		if (DebugOptions.opts.fallStuckDetectAndPopOut)
		{
			this._fallPosHistory.Enqueue(this.position);
			if (this._fallPosHistory.Count > 10)
			{
				Vector2 vector4 = this._fallPosHistory.Dequeue();
				if (fallCollision.collided && this.physicalDepthLayerIdx == Mathf.RoundToInt(fallCollision.collisionDepth))
				{
					bool flag = true;
					using (Queue<Vector2>.Enumerator enumerator = this._fallPosHistory.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							if (Vector2.Distance(enumerator.Current, vector4) > 1.2f)
							{
								flag = false;
							}
						}
					}
					if (flag)
					{
						History.Log("Got stuck while falling? Popping player out");
						Debug.LogWarning("Appeared to get stuck while falling? Popping player out");
						int physicalDepthLayerIdx = this.physicalDepthLayerIdx;
						this.physicalDepthLayerIdx = physicalDepthLayerIdx - 1;
						this._fallPosHistory.Clear();
					}
				}
			}
		}
	}

	// Token: 0x06000674 RID: 1652 RVA: 0x000374C8 File Offset: 0x000356C8
	private Runner.FallSlopeCollision TryFallCollideWithSlope(Vector2 startPos, Vector2 delta)
	{
		bool flag = false;
		SlopeSample slopeSample = default(SlopeSample);
		float num;
		if (this._fallIsTumble)
		{
			Vector2 vector = 1.2f * this.settings.ovalSweptSizeForJump;
			if (this.falling && this._fallIsTumble)
			{
				vector.y = vector.x;
			}
			num = 0.5f * vector.x;
			Raycast.Collision collision = Raycast.OvalSwept<Slope>(startPos, this.rotation, vector, delta, this.raycastSafeNearbyRange, null, true);
			if (collision.didHit)
			{
				flag = true;
				slopeSample = collision.slope.SampleAt(collision.pos.x, false);
			}
		}
		else
		{
			num = 0f;
			flag = Raycast.SampleWithDepthRange(startPos, startPos + delta, (float)this.physicalDepthLayerIdx, this.raycastSafeNearbyRange, out slopeSample, default(Color)).didHit;
		}
		if (flag)
		{
			PropsController.instance.Refresh();
			bool flag2 = (PropsController.instance.triggerFlags & TriggerFlags.SnowSoftLanding) > TriggerFlags.None && this.prevState != Runner.State.DebugFlying;
			if (flag2)
			{
				Debug.Log("Hit soft surface!");
			}
			if (this.falling && this._fallIsTumble && this.speed > this.settings.fall.slopeBounceMinSpeed && !flag2 && Mathf.Abs(slopeSample.angle) > this.settings.fall.slopeBounceMinAngle)
			{
				Runner.BounceResult bounceResult = this.CalculateFallingBounce(startPos, slopeSample.point, slopeSample.normal, num, 0.1f);
				if (bounceResult.shouldBounce)
				{
					this._fallVelocity = bounceResult.velocity;
					return new Runner.FallSlopeCollision
					{
						didBounce = true,
						bouncePositionAfter = bounceResult.position
					};
				}
			}
			this.FallDidLandOnSlope(slopeSample.slope, slopeSample.point, flag2);
		}
		return default(Runner.FallSlopeCollision);
	}

	// Token: 0x06000675 RID: 1653 RVA: 0x000376A0 File Offset: 0x000358A0
	[NullableContext(1)]
	private Runner.FallSlopeCollision FallDidLandOnSlope(Slope slope, Vector2 landingPoint, bool landingOnSnow = false)
	{
		this.currentSlope = slope;
		this.position = landingPoint;
		if (this._currentFallDamage == Damage.MinorDamage)
		{
			GameCamera.instance.cameraShakeState.StartShake(CameraShakeName.Minor);
		}
		else if (this._currentFallDamage == Damage.MajorDamage || this._currentFallDamage == Damage.Death)
		{
			GameCamera.instance.cameraShakeState.StartShake(CameraShakeName.Major);
		}
		if (landingOnSnow)
		{
			bool flag = this._currentFallDamage > Damage.None;
			this.ResetFallDamage();
			this.rotation = this.currentSample.angle;
			this._snowPoofParticles.Emit(this.settings.fall.snowLandingParticleCount);
			this._snowPoofFloatyParticles.Emit(this.settings.fall.snowLandingFloatyParticleCount);
			if (this.shouldDie)
			{
				this.Die(false);
			}
			else if (this.onSlide && !this._fallIsTumble)
			{
				this.state = Runner.State.Sliding;
			}
			else if (flag || this._fallIsTumble)
			{
				this._hardLandingWasSoftenedBySnow = true;
				this.state = Runner.State.HardLanding;
			}
			else
			{
				this.state = Runner.State.Running;
			}
			return new Runner.FallSlopeCollision
			{
				changedState = true
			};
		}
		if (this._fallIsTumble)
		{
			if (this._currentFallDamage == Damage.MinorDamage)
			{
				this.EscalateFallDamage(Damage.MajorDamage);
			}
			else if (this._currentFallDamage == Damage.None)
			{
				this.EscalateFallDamage(Damage.MinorDamage);
			}
		}
		if (this._currentFallDamage > Damage.None)
		{
			this.health.ApplyDamage(DamageType.Fall, this._currentFallDamage);
		}
		if (this.shouldDie)
		{
			this.Die(false);
			return new Runner.FallSlopeCollision
			{
				changedState = true
			};
		}
		if (this._fallIsTumble || (this._currentFallDamage > Damage.None && this.prevState != Runner.State.DebugFlying && !this.onSlide) || this.prevState == Runner.State.Caught)
		{
			if (this._currentFallDamage != Damage.None)
			{
				Narrative.instance.SetFallDamageInInk(this._currentFallDamage.ToString());
			}
			this.state = Runner.State.HardLanding;
		}
		else if (this.onSlide)
		{
			this.state = Runner.State.Sliding;
		}
		else
		{
			this.state = Runner.State.Running;
		}
		return new Runner.FallSlopeCollision
		{
			changedState = true
		};
	}

	// Token: 0x06000676 RID: 1654 RVA: 0x0003789D File Offset: 0x00035A9D
	private void RandomiseTumbleSpeed(float dir, float scalar = 1f)
	{
		this._tumbleSpeed = scalar * dir * this.settings.fall.tumbleSpeedRange.Random();
	}

	// Token: 0x06000677 RID: 1655 RVA: 0x000378BE File Offset: 0x00035ABE
	private void EscalateFallDamage(Damage damage)
	{
		this._currentFallDamage = damage;
		this._maxFallDamage = ((this._currentFallDamage > this._maxFallDamage) ? this._currentFallDamage : this._maxFallDamage);
	}

	// Token: 0x06000678 RID: 1656 RVA: 0x000378E9 File Offset: 0x00035AE9
	private void ResetFallDamage()
	{
		this._currentFallDamage = Damage.None;
		this._maxFallDamage = Damage.None;
	}

	// Token: 0x06000679 RID: 1657 RVA: 0x000378FC File Offset: 0x00035AFC
	private void State_FinalJump(bool start, bool end)
	{
		if (start)
		{
			this.animator.SetAnimation("Idle", FrameAnimator.PosMatch.None);
			this.direction = 1f;
			this.momentum = 0f;
			if (this._finalJumpSubState == Runner.FinalJumpSubState.None)
			{
				this._finalJumpSubState = Runner.FinalJumpSubState.Prep;
			}
			Prop prop = Prop.GetLoadedPropsByInkName("FINAL_JUMP_ZONE")[0];
			this._jumpMarker = prop.GetComponentInChildren<RhythmActionMarker>(true);
			if (!Runner._hasOriginalJumpMarkerPosition)
			{
				Runner._originalJumpMarkerPosition = this._jumpMarker.transform.position;
				Runner._hasOriginalJumpMarkerPosition = true;
				return;
			}
			this._jumpMarker.transform.position = Runner._originalJumpMarkerPosition;
			return;
		}
		else
		{
			if (end)
			{
				this._finalJumpSubState = Runner.FinalJumpSubState.None;
				this._leftRightHoldTime = 0f;
				this.alpha = 1f;
				this.animator.transform.localRotation = Quaternion.identity;
				this.animator.transform.localPosition = Vector3.zero;
				Game.instance.RemoveTimeScalar(Game.TimeScalar.FinalJump);
				this.rotation = 0f;
				this._swanDiveVelocity = default(Vector2);
				this._leftRightHoldTime = 0f;
				this._divingTime = 0f;
				this._pauseTime = 0f;
				this._musicWantedPauseTime = 0f;
				return;
			}
			if (this._finalJumpSubState == Runner.FinalJumpSubState.Prep || this._finalJumpSubState == Runner.FinalJumpSubState.WillJumpImmediate)
			{
				if (GameInput.moveLeftRight != 0f && this._finalJumpSubState == Runner.FinalJumpSubState.Prep)
				{
					this._leftRightHoldTime += this._dt;
					if (this._leftRightHoldTime > this.settings.finalJump.leftRightToCancelHoldTime)
					{
						this.state = Runner.State.Running;
						return;
					}
				}
				else
				{
					this._leftRightHoldTime = 0f;
				}
				if (GameInput.jumped || this._finalJumpSubState == Runner.FinalJumpSubState.WillJumpImmediate)
				{
					this._jumpMarker.Hide(RhythmActionMarker.HideReason.FinalJump, true, true, 0f);
					Narrative.instance.FinalJumpStarted();
					this._finalJumpSubState = Runner.FinalJumpSubState.WaitingForMusicSync;
				}
			}
			if (this._finalJumpSubState == Runner.FinalJumpSubState.WaitingForMusicSync && AudioController.instance.canSyncMusicForFinalJump)
			{
				float num = AudioController.instance.SyncMusicForFinalJump();
				FrameAnimation frameAnimation = this.animator.TryGetAnim("FinalJumpRunUp");
				this._musicWantedPauseTime = num - frameAnimation.duration / this.settings.finalJump.launchTimeScalar + LatencyCalibrator.latency;
				MonoSingleton<BlackBars>.instance.SetVisible(true, BlackBarsReason.FinalJump);
				this._finalJumpSubState = Runner.FinalJumpSubState.PauseAndZoomBeforeLaunch;
			}
			if (this._finalJumpSubState == Runner.FinalJumpSubState.PauseAndZoomBeforeLaunch)
			{
				this._pauseTime += Time.unscaledDeltaTime;
				if (this._pauseTime > this._musicWantedPauseTime - 0.3f && GameCamera.instance.animatedCameraState.anim == null)
				{
					InkAnimation inkAnimation;
					if (InkAnimation.all.TryGetValue("FinalJumpCameraAnim", out inkAnimation))
					{
						inkAnimation.Begin("FinalJumpCameraAnim", false);
					}
					else
					{
						Debug.LogError("Couldn't find FinalJumpCameraAnim InkAnimation object!");
					}
					Game.instance.SetTimeScalar(Game.TimeScalar.FinalJump, this.settings.finalJump.launchTimeScalar);
				}
				if (this._pauseTime > this._musicWantedPauseTime)
				{
					this.animator.SetAnimation("FinalJumpRunUp", FrameAnimator.PosMatch.None);
					this._finalJumpSubState = Runner.FinalJumpSubState.Launching;
				}
			}
			if (this._finalJumpSubState == Runner.FinalJumpSubState.Launching)
			{
				if (this.animator.frameIdx > this.settings.finalJump.runUpStartFrameIdx)
				{
					this.position += this.settings.finalJump.launchRunVelocity * this._dt;
				}
				if (this.animator.normalizedTime == 1f)
				{
					this.currentSlope = null;
					this.animator.SetAnimationWithTransition("FinalJumpSwanInto", "FinalJumpSwanLoop", 0, false, false, FrameAnimator.PosMatch.Frame);
					Game.instance.SetTimeScalar(Game.TimeScalar.FinalJump, this.settings.finalJump.flyTimeScalar);
					this._swanDiveVelocity = this.settings.finalJump.jumpStartVelocity;
					this._finalJumpSubState = Runner.FinalJumpSubState.Diving;
				}
			}
			if (this._finalJumpSubState == Runner.FinalJumpSubState.Diving)
			{
				this._swanDiveVelocity.y = this._swanDiveVelocity.y + this.settings.finalJump.gravity * this._dt;
				this._swanDiveVelocity.x = this._swanDiveVelocity.x * TimeX.Damping(this.settings.finalJump.xDamping, this._dt);
				this.position += this._swanDiveVelocity * this._dt;
				this._divingTime += this._dt;
				if (this._divingTime > this.settings.finalJump.removeTimeScalarTime)
				{
					Game.instance.RemoveTimeScalar(Game.TimeScalar.FinalJump);
				}
				float num2 = Mathf.Clamp01(this._divingTime / this.settings.finalJump.rotationOffsetDuration);
				float num3 = Mathf.Lerp(this.settings.finalJump.rotationOffsetStart, this.settings.finalJump.rotationOffsetEnd, num2);
				this.rotation = 57.29578f * Mathf.Atan2(-this._swanDiveVelocity.x, this._swanDiveVelocity.y) + num3;
				if (this.position.y < -4f)
				{
					InputVibration.Medium();
					this._finalJumpSubState = Runner.FinalJumpSubState.InTheSea;
					this._swanDiveVelocity = Vector2.zero;
					this.alpha = 0f;
					MonoSingleton<WaterRippleManager>.instance.CreateRipple(base.transform.position);
					Vector3 position = base.transform.position;
					position.y = 0f;
					this._splashParticles.transform.position = position;
					this._splashParticles.Emit(2);
					Game.instance.RemoveTimeScalar(Game.TimeScalar.FinalJump);
				}
			}
			Runner.FinalJumpSubState finalJumpSubState = this._finalJumpSubState;
			return;
		}
	}

	// Token: 0x0600067A RID: 1658 RVA: 0x00037E6C File Offset: 0x0003606C
	private void FinalJumpLateUpdate()
	{
		if ((this.animator.IsAnimation("FinalJumpSwanInto", null, null, null) && this.animator.frameIdx >= this.settings.finalJump.firstPreRotatedFrameIdx) || this.animator.IsAnimation("FinalJumpSwanLoop", null, null, null))
		{
			this.animator.transform.localRotation = this.settings.finalJump.finalFramesRotationOffset;
			this.animator.transform.localPosition = this.settings.finalJump.finalFramesPositionOffset;
			return;
		}
		this.animator.transform.localRotation = Quaternion.identity;
		this.animator.transform.localPosition = Vector3.zero;
	}

	// Token: 0x0600067B RID: 1659 RVA: 0x00037F2B File Offset: 0x0003612B
	private void StartFinalJumpImmediate()
	{
		this._finalJumpSubState = Runner.FinalJumpSubState.WillJumpImmediate;
		this.state = Runner.State.FinalJump;
	}

	// Token: 0x0600067C RID: 1660 RVA: 0x00037F3C File Offset: 0x0003613C
	private void TryPrepareForFinalJump()
	{
		if (this.running && FinalJumpZone.activeZone != null)
		{
			float x = FinalJumpZone.activeZone.transform.position.x;
			if (this.stoppedBasically && GameInput.moveLeftRight == 0f)
			{
				if (!this.IsAtSpecificAutoRunTarget(x))
				{
					this.SetAutoRunTarget(FinalJumpZone.activeZone.transform.position, 3f, false);
					return;
				}
				this.playerControlDisabled &= ~PlayerControlDisableReason.AutoRunToProp;
				this.state = Runner.State.FinalJump;
				return;
			}
		}
	}

	// Token: 0x0600067D RID: 1661 RVA: 0x00037FC4 File Offset: 0x000361C4
	private void State_HardLanding(bool start, bool end)
	{
		if (start)
		{
			bool flag = this.animator.IsAnimation("Tumble", null, null, null);
			this.animator.SetAnimationWithTransition("HardLanding", "Idle", 0, true, false, FrameAnimator.PosMatch.None);
			if (flag)
			{
				this.animator.normalizedTime = 0.3f;
			}
			if (this.prevState != Runner.State.ZipLine)
			{
				if (this._hardLandingWasSoftenedBySnow)
				{
					InputVibration.LongSoft();
					AudioController.instance.PlaySoundEffect(SoundEffect.SnowThump);
					Narrative.instance.FallSoftLanding();
				}
				else
				{
					InputVibration.Large();
					Narrative.instance.Fall();
				}
			}
			if (!this._hardLandingWasSoftenedBySnow)
			{
				this.stamina *= this.settings.stamina.staminaEffectOfHardLanding;
			}
			AudioController.instance.PlayVocalisation(Vocalisation.HardLanding, 0f);
			this._hardLandingAngle = (this.rotation = this.currentSample.angle);
			this.momentum = 0f;
			return;
		}
		if (end)
		{
			this._hardLandingWasSoftenedBySnow = false;
			return;
		}
		if (this.animator.IsAnimation("HardLanding", null, null, null))
		{
			this.rotation = Mathf.LerpAngle(this._hardLandingAngle, 0f, this.animator.normalizedTime);
		}
		if (this.animator.IsAnimation("Idle", null, null, null) || (this.animator.IsAnimation("HardLanding", null, null, null) && this.animator.normalizedTime >= 1f))
		{
			this.rotation = 0f;
			this.state = Runner.State.Running;
		}
	}

	// Token: 0x1700018B RID: 395
	// (get) Token: 0x0600067E RID: 1662 RVA: 0x0003813D File Offset: 0x0003633D
	public HideReason hideReason
	{
		get
		{
			return this._hideReasonFlags;
		}
	}

	// Token: 0x0600067F RID: 1663 RVA: 0x00038148 File Offset: 0x00036348
	public void SetHidden(bool hidden, HideReason reason)
	{
		if (hidden)
		{
			this._hideReasonFlags |= reason;
		}
		else
		{
			this._hideReasonFlags &= ~reason;
		}
		if (this._hideReasonFlags > HideReason.None && this.state != Runner.State.Hidden)
		{
			this.state = Runner.State.Hidden;
			return;
		}
		if (this._hideReasonFlags == HideReason.None && this.state == Runner.State.Hidden)
		{
			this.state = Runner.State.Running;
		}
	}

	// Token: 0x06000680 RID: 1664 RVA: 0x000381A9 File Offset: 0x000363A9
	private void State_Hidden(bool start, bool end)
	{
		if (start)
		{
			this.alpha = 0f;
			return;
		}
		if (end)
		{
			this.alpha = 1f;
			return;
		}
	}

	// Token: 0x06000681 RID: 1665 RVA: 0x000381C9 File Offset: 0x000363C9
	private void State_InkPose(bool start, bool end)
	{
		this.PlayAnimation_StateFunc(start, end);
	}

	// Token: 0x06000682 RID: 1666 RVA: 0x000381D4 File Offset: 0x000363D4
	[NullableContext(1)]
	public void SetPoseFromInk(string poseName)
	{
		if (this.running || this.state == Runner.State.InkPose)
		{
			Runner.instance.playerControlDisabled |= PlayerControlDisableReason.InkPose;
			this.PlayAnimation(poseName, Runner.State.InkPose);
			return;
		}
		Debug.LogWarning(string.Concat(new string[] { "Did not play ink's requested '", poseName, "' anim/pose because you weren't in a valid state: ", this.stateName, ". If you want this to be possible, add it above" }));
	}

	// Token: 0x06000683 RID: 1667 RVA: 0x00038244 File Offset: 0x00036444
	public void RemovePose()
	{
		if (this.state == Runner.State.InkPose)
		{
			Runner.instance.playerControlDisabled &= ~PlayerControlDisableReason.InkPose;
			this.ExitAnimation();
			this.state = Runner.State.Running;
		}
	}

	// Token: 0x1700018C RID: 396
	// (get) Token: 0x06000684 RID: 1668 RVA: 0x00038270 File Offset: 0x00036470
	public bool jumpIsSpecial
	{
		get
		{
			return this.jumping && this._jump.special;
		}
	}

	// Token: 0x06000685 RID: 1669 RVA: 0x00038288 File Offset: 0x00036488
	public void TripSpecialMismatch()
	{
		if (this.lastSlopeJumpedFrom == null)
		{
			Debug.LogError("TripSpecialMismatch: lastSlopeJumpedFrom was null somehow");
			return;
		}
		this.currentSlope = this.lastSlopeJumpedFrom;
		if (this._stumbleCount < this.settings.run.stumbleCountMax)
		{
			this.state = Runner.State.Running;
			this.Stumble();
			return;
		}
		this.state = Runner.State.Tripping;
	}

	// Token: 0x06000686 RID: 1670 RVA: 0x000382E8 File Offset: 0x000364E8
	private void State_Jumping(bool start, bool end)
	{
		if (start)
		{
			this.currentSlope = null;
			this._lastJumpTime = Time.time;
			int num = this._jumpIndex % 2;
			if (num == 0 && Random.value > 0.5f)
			{
				num = 2;
			}
			this.animator.SetAnimationVariantNumber(num);
			this._jumpIndex++;
			this.animator.speed = 0f;
			if (this.isMusicRunning)
			{
				this.OnJumpDuringMusicRun();
			}
			if (Runner.onJumpStart != null)
			{
				Runner.onJumpStart();
			}
			return;
		}
		if (end)
		{
			if (!this.onSlide)
			{
				float num2 = this.settings.jump.maxJumpMomentumBonus * this.settings.jump.momentumBonusSteepness.InverseLerp(this.steepness);
				float num3 = (this.inMusicRunningArea ? 1f : this.settings.run.maxSprintMomentum);
				this.momentum = Mathf.Clamp(this.momentum + this.direction * num2, -num3, num3);
			}
			this._jump.targetBalancePoint = null;
			this._jump.requireRetroactiveJumpPress = false;
			if (this.currentSlope != null)
			{
				this.physicalDepthLayerIdx = Mathf.RoundToInt(this.currentSlope.transform.position.z);
			}
			this.animator.speed = 1f;
			if (Runner.onJumpEnd != null)
			{
				Runner.onJumpEnd();
			}
			return;
		}
		if (this._jump.requireRetroactiveJumpPress && this.stateTimer > this.settings.jump.maxRetroactiveJumpDelay)
		{
			this.EscalateFallDamage(Damage.MinorDamage);
			this.state = Runner.State.Falling;
			return;
		}
		if (!GameInput.jumpHeld && this.stateTimer < this.settings.jump.jumpReleaseWindowNorm * this._jump.expectedDuration && !this.isMusicRunning && this._jump.targetBalancePoint == null && !this._jump.specialNarrative)
		{
			float num4 = TimeX.Damping(this.settings.jump.jumpReleaseDamping.x, this._dt);
			this._jump.velocity.x = this._jump.velocity.x * num4;
			this.momentum *= num4;
			this._jump.initialVerticalVelocity = this._jump.initialVerticalVelocity * TimeX.Damping(this.settings.jump.jumpReleaseDamping.y, this._dt);
		}
		if (!this.isMusicRunning && Mathf.Abs(this._leftRightInput) > 0f && !this._jump.specialNarrative)
		{
			if (this.direction * this._leftRightInput < 0f)
			{
				this._jump.targetBalancePoint = null;
			}
			if (this._jump.targetBalancePoint == null)
			{
				this._jump.velocity.x = this._jump.velocity.x + this._leftRightInput * this.settings.jump.airControl * this._dt;
				this._jump.velocity.x = this._jump.airControlSpeedLimit.Clamp(this._jump.velocity.x);
			}
		}
		float num5 = this.stateTimer;
		float num6 = this._jump.initialVerticalVelocity * num5 + 0.5f * this._jump.gravity * num5 * num5;
		Vector2 vector = new Vector2(this.position.x + this._jump.velocity.x * this._dt, this._jump.startPos.y + num6);
		this._jump.velocity.y = (vector.y - this.position.y) / this._dt;
		Vector2 vector2;
		if (Raycast.InvisibleCollision(this.position, vector, this.raycastCurrentRangeONLY, out vector2, false))
		{
			vector.x = vector2.x;
			this._jump.velocity.x = 0f;
			this._jump.targetSlope = null;
			this._jump.targetBalancePoint = null;
		}
		if (this.TryLandOnBalancePointOrFall(this.position, vector))
		{
			return;
		}
		bool flag = false;
		float num7 = 0f;
		SlopeSample slopeSample;
		if (this._jump.targetBalancePoint == null && Raycast.SampleWithDepthRange(this.position, vector, (float)this.physicalDepthLayerIdx, this.raycastSafeNearbyRange, out slopeSample, default(Color)).didHit && (!this._jump.isDropDown || !(slopeSample.slope == this.lastSlopeJumpedFrom)))
		{
			float num8 = Mathf.Clamp01((slopeSample.point - this.position).magnitude / (vector - this.position).magnitude);
			num7 = (1f - num8) * this._dt;
			foreach (BalancePoint balancePoint in Level.current.balancePoints.Nearby(this.position, this.raycastSafeNearbyRange, 2f, null))
			{
				if (!balancePoint.exitToSlope && !(balancePoint == this.lastBalancePointDroppedFrom) && Vector2.Distance(balancePoint.transform.position, slopeSample.point) < 1f)
				{
					this.StartBalancingMaybeFall(balancePoint, 1f - num8);
					return;
				}
			}
			this._prevVelocity = Vector2.zero;
			this.currentSlope = slopeSample.slope;
			this.position = slopeSample.point;
			flag = true;
		}
		float num9 = this.stateTimer / this._jump.expectedDuration;
		if (num9 <= 1f)
		{
			this.animator.normalizedTime = num9;
		}
		else if (this.animator.IsAnimation("Jump", null, null, null))
		{
			this.animator.SetAnimation("JumpToFall", FrameAnimator.PosMatch.None);
		}
		bool flag2 = num9 > 1.1f;
		if (!flag2 && this._jump.targetBalancePoint != null)
		{
			float num10 = this._jump.expectedDuration - this.stateTimer;
			FrameAnimation frameAnimation = this.animator.TryGetAnim("JumpToTeeter");
			if (num10 <= frameAnimation.duration)
			{
				this.animator.SetAnimation("JumpToTeeter", FrameAnimator.PosMatch.None);
			}
		}
		else if (flag2 && this.animator.IsAnimation("JumpToTeeter", null, null, null))
		{
			this.animator.SetAnimation("Fall", FrameAnimator.PosMatch.None);
		}
		if (this.stateTimer >= this._jump.expectedDuration && (this._jump.targetBalancePoint != null || (this.isMusicRunning && this._jump.targetSlope != null) || this._jump.specialNarrative))
		{
			this.position = this._jump.targetPos;
			num7 = this.stateTimer - this._jump.expectedDuration;
			if (this._jump.targetBalancePoint != null)
			{
				this.StartBalancingMaybeFall(this._jump.targetBalancePoint, num7);
				return;
			}
			this.currentSlope = this._jump.targetSlope;
			flag = true;
		}
		if (flag)
		{
			if (!this.isMusicRunning)
			{
				InputVibration.Small();
			}
			if (this._jump.requireRetroactiveJumpPress)
			{
				this.state = Runner.State.Tripping;
				return;
			}
			if (this._jump.specialNarrative && this._jumpingOntoChairLift)
			{
				this._jumpingOntoChairLift = false;
				this.state = Runner.State.ChairLift;
				return;
			}
			if (this._jump.specialNarrative && this._zipLine != null)
			{
				this.state = Runner.State.ZipLine;
				return;
			}
			Runner.State state = Runner.State.Running;
			if (this.onSlide)
			{
				state = Runner.State.Sliding;
			}
			this.state = state;
			if (!this.running && !this.sliding)
			{
				return;
			}
			float dt = this._dt;
			this._dt = num7;
			this.RunState(state, false, false);
			this._dt = dt;
			return;
		}
		else
		{
			if (this.stateTimer > 0.3f * this._jump.expectedDuration && this.TrySetupGrabLedge(ClimbCheckRange.JumpOrFall, vector, null, true))
			{
				return;
			}
			if (!this.isMusicRunning || !(this._jump.targetSlope != null) || this._jump.specialNarrative)
			{
				Runner.FallCollision fallCollision = this.JumpFallCollideWithPolyWalls(vector, this.raycastCurrentAndSafeForeRange, true);
				if (fallCollision.collided)
				{
					if (fallCollision.foundGulleySlope != null)
					{
						this.currentSlope = fallCollision.foundGulleySlope;
						this.state = Runner.State.Running;
						return;
					}
					if (this.state != Runner.State.Jumping)
					{
						return;
					}
					Debug.LogError("Expected to not be in jumping state anymore after JumpFallCollideWithPolyWalls");
				}
			}
			this.position = vector;
			if (this.stateTimer >= this._jump.expectedDuration * 1.2f && !this._jump.specialNarrative)
			{
				this.state = Runner.State.Falling;
			}
			this.rotation = -8f * this.direction;
		}
	}

	// Token: 0x1700018D RID: 397
	// (get) Token: 0x06000687 RID: 1671 RVA: 0x00038BE4 File Offset: 0x00036DE4
	public float musicRunningJumpDurationFromBeat
	{
		get
		{
			float num = (this.isMusicRunning ? this.runTrack.currentBeatDuration : 0.5f);
			float num2 = this.runTrack.currentMusicTime + 0.5f * num;
			return this.runTrack.BeatDurationAtTime(num2) - this.settings.jump.jumpLandingDurationNorm * num;
		}
	}

	// Token: 0x06000688 RID: 1672 RVA: 0x00038C40 File Offset: 0x00036E40
	private bool TryJump(bool special = false)
	{
		this._jumpQueued = false;
		this.lastSlopeJumpedFrom = this.currentSlope;
		Vector2 position = this.position;
		float num = this.momentum;
		if (this.sliding && this._jumpDirIntent.x * this.currentSlopeDownDir < -0.01f)
		{
			this._jumpDirIntent.x = 0.01f * this.currentSlopeDownDir;
			num = 0f;
		}
		bool flag = this._jumpDirIntent != Vector2.zero;
		if (this.isMusicRunning)
		{
			float currentBeatDuration = this.runTrack.currentBeatDuration;
		}
		float num2;
		if (this.isMusicRunning)
		{
			num2 = this.musicRunningJumpDurationFromBeat;
		}
		else
		{
			num2 = this.settings.jump.jumpDurationStandard;
		}
		float num3 = num;
		if (Mathf.Abs(this._jumpDirIntent.x) > 0f && Mathf.Abs(num3) < this.settings.jump.minJumpMomentum)
		{
			num3 = this.settings.jump.minJumpMomentum;
		}
		float num4 = ((this._jumpDirIntent.x == 0f) ? this.direction : Mathf.Sign(this._jumpDirIntent.x));
		num3 = num4 * Mathf.Abs(num3);
		num = num3;
		Simulate.FindOptions findOptions = new Simulate.FindOptions
		{
			takeUphillForks = true,
			stopAtSwitchback = true,
			predictMode = true
		};
		if (this._jumpDirIntent.y < 0f)
		{
			findOptions.takeUphillForks = false;
		}
		TrackPosition trackPosition = new TrackPosition
		{
			slope = this.currentSlope,
			x = position.x
		};
		SlopeSample slopeSample;
		if ((this.balancing || this.falling) && Raycast.SampleWithDepthRange(this.position, this.position - 10f * Vector2.up, (float)this.physicalDepthLayerIdx, this.raycastNearbyRange, out slopeSample, default(Color)).didHit)
		{
			trackPosition.slope = slopeSample.slope;
			trackPosition.x = slopeSample.point.x;
		}
		float num5 = num;
		if (Mathf.Abs(num) < 1f && (!this.stoppedBasically || this._jumpDirIntent.x != 0f) && Mathf.Abs(num) > 0f)
		{
			num5 = Mathf.Sign(num);
		}
		float num6 = num2;
		Vector2 vector = position;
		Slope slope = null;
		if (trackPosition.slope != null)
		{
			Simulate.FindResult findResult = Simulate.FindGroundPositionAtTime(trackPosition, num2, num5, findOptions, this.settings);
			vector = findResult.sample.point;
			slope = findResult.sample.slope;
			num6 = findResult.remainingTime;
			History.Log("Simulated on slope");
			if (!this.isMusicRunning && vector.y - position.y > this.settings.jump.jumpHeightRange.max)
			{
				History.Log("Simulated target slope too high");
				slope = null;
			}
		}
		if (num6 > 0f)
		{
			float angle = this.currentSample.angle;
			Vector2 vector2 = Slope.VectorWithAngle(angle, 1f);
			if (num4 < 0f)
			{
				vector2 = -vector2;
			}
			float num7 = num6 * Simulate.SignedSpeedOnGround(Mathf.Abs(num5), angle, this.sliding, this.settings.run);
			if (this.wallSliding && this.staminaIsVeryLow)
			{
				num7 *= this.settings.wallSlide.jumpDistScalar;
			}
			vector += num7 * vector2;
			if (this.wallSliding && this.staminaIsVeryLow)
			{
				vector.y += this.settings.wallSlide.jumpTargetYOffset;
			}
		}
		bool flag2 = false;
		if (flag)
		{
			float num8 = 0f;
			if (this.currentSample.isValid)
			{
				num8 = this.currentSample.angle;
				num8 = Mathf.Clamp(num8, -25f, 25f);
			}
			float num9 = Vector2.SignedAngle(Vector2.down, this._jumpDirIntent);
			flag2 = Mathf.Abs(num9 - num8) < 30f || Mathf.Abs(num9) < 30f;
		}
		BalancePoint balancePoint;
		if (flag2)
		{
			balancePoint = this.FindNearestBalancePointDropDown(position);
		}
		else
		{
			balancePoint = this.FindNearestBalancePoint(position, vector, num2, this.momentum == 0f, num5);
		}
		if (balancePoint != null)
		{
			vector = balancePoint.transform.position;
			Vector2 vector3 = vector - this.position;
			vector3.y *= this.settings.run.betweenSlopesYScalar;
			float magnitude = vector3.magnitude;
			float num10 = Simulate.SignedSpeedOnGround(Mathf.Abs(num5), 0f, false, this.settings.run);
			if (this.isMusicRunning)
			{
				num2 = this.settings.balance.balanceDistDurationScalar * magnitude / num10;
				num2 = Mathf.Max(num2, 0.2f);
			}
			else
			{
				float minBalancePointJumpDurationNorm = this.settings.jump.minBalancePointJumpDurationNorm;
				float num11 = num10 * num2;
				float num12 = Mathf.InverseLerp(minBalancePointJumpDurationNorm * num11, num11, magnitude);
				num2 = Mathf.Lerp(minBalancePointJumpDurationNorm * num2, num2, num12);
			}
		}
		bool flag3 = false;
		bool flag4 = false;
		Vector3 vector4 = default(Vector3);
		if (flag2)
		{
			if (balancePoint != null)
			{
				flag3 = true;
			}
			else
			{
				if ((PropsController.instance.triggerFlags & TriggerFlags.DisableDropDown) > TriggerFlags.None)
				{
					History.Log("Prevent drop down");
					return false;
				}
				Vector2 vector5 = this.position - 0.2f * Vector2.up;
				Vector2 vector6 = this.position - this.settings.dropDownRayDist * Vector2.up;
				Range range = new Range((float)this.physicalDepthLayerIdx + this.settings.layer.layerDropDownMin, (float)this.physicalDepthLayerIdx + 0.1f);
				SlopeSample slopeSample2;
				if (!Raycast.SampleWithDepthRange(vector5, vector6, (float)this.physicalDepthLayerIdx - 0.1f, range, out slopeSample2, default(Color)).didHit)
				{
					History.Log("Abort jump");
					return false;
				}
				flag3 = true;
				flag4 = true;
				vector4 = slopeSample2.point3d;
			}
		}
		this.momentum = num;
		if (flag3)
		{
			this.position -= 0.3f * Vector2.up;
			if (flag4)
			{
				vector = vector4;
			}
			else
			{
				vector = this.position - 5f * Vector2.up;
			}
			this.lastBalancePointDroppedFrom = this._balancePoint;
			if (balancePoint == this.lastBalancePointDroppedFrom)
			{
				History.Log("lastBalancePointDroppedFrom is null for hop on spot");
				this.lastBalancePointDroppedFrom = null;
			}
			int num13;
			if (this.balancing && this._balancePoint != null)
			{
				num13 = Mathf.RoundToInt(this._balancePoint.transform.position.z);
			}
			else if (this.currentSlope != null)
			{
				num13 = Mathf.RoundToInt(this.currentSlope.transform.position.z);
			}
			else
			{
				num13 = this.physicalDepthLayerIdx;
			}
			this.physicalDepthLayerIdx = num13 - 1;
			slope = null;
		}
		float num14 = 1f;
		if (balancePoint != null)
		{
			num14 = this.settings.jump.balancePointJumpHeightScalar;
			if (!this.isMusicRunning)
			{
				num2 *= this.settings.jump.balancePointDurationScalar;
			}
		}
		else if (this.climbing)
		{
			num14 = this.settings.jump.climbJumpHeightScalar;
		}
		else if (this.isMusicRunning)
		{
			num14 = this.settings.jump.musicRunningJumpHeightScalar;
		}
		if (!flag3 && !this.isMusicRunning && Random.value < 0.5f)
		{
			AudioController.instance.PlayVocalisation(Vocalisation.JumpHuh, 0f);
		}
		if (flag3)
		{
			this.PerformJump_DropToPoint(vector, slope, balancePoint, 0f);
		}
		else if (this.wallSliding && this.staminaIsVeryLow && balancePoint == null)
		{
			this.PerformJump_WithVelocity(new Vector2(-this.direction * this.settings.jump.wallSlideJumpAwaySpeed, 0f));
		}
		else if (this.balancing && flag2 && !flag3)
		{
			this.PerformJump_WithVelocity(new Vector2(this.direction * this.settings.jump.balancePointSideDropXSpeed, 0f));
		}
		else
		{
			this.PerformJump_Normal(vector, slope, balancePoint, num2, num14, special);
		}
		return true;
	}

	// Token: 0x06000689 RID: 1673 RVA: 0x00039478 File Offset: 0x00037678
	[NullableContext(2)]
	private void PerformJump_DropToPoint(Vector2 targetPos, Slope targetSlope = null, BalancePoint targetBalancePoint = null, float initialDropSpeed = 0f)
	{
		this._jump = default(Runner.JumpState);
		History.Log("PerformJump_DropToPoint");
		this.StartJumpAnimation();
		this._jump.startPos = this.position;
		this._jump.targetPos = targetPos;
		this._jump.targetSlope = targetSlope;
		this._jump.targetBalancePoint = targetBalancePoint;
		this._jump.gravity = this.settings.standardGravity;
		this._jump.expectedDuration = Parabola.CalcJumpDurationFromDropHeight(this._jump.startPos.y - targetPos.y, initialDropSpeed, this._jump.gravity);
		this._jump.initialVerticalVelocity = initialDropSpeed;
		this._jump.velocity = new Vector2((targetPos.x - this._jump.startPos.x) / this._jump.expectedDuration, initialDropSpeed);
		this._jump.isDropDown = true;
		this.PerformJump();
	}

	// Token: 0x0600068A RID: 1674 RVA: 0x00039574 File Offset: 0x00037774
	private void PerformJump_WithVelocity(Vector2 initialVelocity)
	{
		this._jump = default(Runner.JumpState);
		History.Log("PerformJump_WithVelocity");
		this.StartJumpAnimation();
		this._jump.startPos = this.position;
		this._jump.gravity = this.settings.standardGravity;
		this._jump.expectedDuration = this.settings.jump.jumpDurationStandard;
		this._jump.velocity = initialVelocity;
		this._jump.initialVerticalVelocity = initialVelocity.y * this._jump.expectedDuration + 0.5f * this._jump.gravity * this._jump.expectedDuration * this._jump.expectedDuration;
		Vector2 vector = new Vector2(initialVelocity.x * this._jump.expectedDuration, this._jump.initialVerticalVelocity);
		this._jump.targetPos = this._jump.startPos + vector;
		this.PerformJump();
	}

	// Token: 0x0600068B RID: 1675 RVA: 0x00039678 File Offset: 0x00037878
	private void StartJumpAnimation()
	{
		FrameAnimator.PosMatch posMatch = FrameAnimator.PosMatch.None;
		if (this.wallSliding || this.climbing)
		{
			posMatch = FrameAnimator.PosMatch.Mouth;
		}
		this.animator.SetAnimation("Jump", posMatch);
	}

	// Token: 0x0600068C RID: 1676 RVA: 0x000396AA File Offset: 0x000378AA
	[NullableContext(2)]
	private void PerformJump_Narrative(Vector2 targetPos, Slope targetSlope)
	{
		this.PerformJump_Normal(targetPos, targetSlope, null, 0.5f, 1f, false);
		this._jump.specialNarrative = true;
	}

	// Token: 0x0600068D RID: 1677 RVA: 0x000396CC File Offset: 0x000378CC
	[NullableContext(2)]
	private void PerformJump_Normal(Vector2 targetPos, Slope targetSlope, BalancePoint targetBalancePoint, float duration, float heightRangeScalar, bool special)
	{
		this._jump = default(Runner.JumpState);
		History.Log("PerformJump_Normal");
		this.StartJumpAnimation();
		this._jump.startPos = this.position;
		this._jump.targetPos = targetPos;
		if (targetBalancePoint != null)
		{
			this._jump.targetBalancePoint = targetBalancePoint;
		}
		else if (targetSlope != null)
		{
			this._jump.targetSlope = targetSlope;
		}
		float num = this._jump.targetPos.y - this._jump.startPos.y;
		Range range = this.settings.jump.jumpHeightRange * heightRangeScalar;
		if (!this.isMusicRunning && num > range.max)
		{
			this._jump.targetSlope = null;
			this._jump.targetBalancePoint = null;
			num = range.max;
			History.Log("stepDiff clamped");
		}
		this._jump.gravity = this.settings.standardGravity;
		this._jump.initialVerticalVelocity = Parabola.CalcJumpSpeedYFromGravity(this.settings.standardGravity, num, duration);
		float num2 = Parabola.CalcJumpHeightFromInitialSpeed(this._jump.initialVerticalVelocity, this.settings.standardGravity);
		if (!range.Contains(num2) && num < range.max)
		{
			float num3 = range.Clamp(num2);
			if (num > num3)
			{
				num = num3;
			}
			this._jump.initialVerticalVelocity = Parabola.CalcJumpSpeedYFromHeight(num3, num, duration);
			this._jump.gravity = Parabola.CalcGravity(this._jump.initialVerticalVelocity, num3);
		}
		this._jump.velocity = new Vector2((this._jump.targetPos.x - this._jump.startPos.x) / duration, this._jump.initialVerticalVelocity);
		this._jump.expectedDuration = duration;
		this._jump.special = special;
		this.PerformJump();
	}

	// Token: 0x0600068E RID: 1678 RVA: 0x000398B4 File Offset: 0x00037AB4
	private void LogHistoryPredictedJumpArc()
	{
		Vector3 vector = this._jump.startPos;
		vector.z = (float)this.physicalDepthLayerIdx;
		Vector2 vector2 = this._jump.velocity;
		for (float num = 0.03f; num < this._jump.expectedDuration; num += 0.03f)
		{
			vector2 += 0.03f * this._jump.gravity * Vector2.up;
			Vector2 vector3 = 0.03f * vector2;
			vector += vector3;
		}
	}

	// Token: 0x0600068F RID: 1679 RVA: 0x00039948 File Offset: 0x00037B48
	private void PerformJump()
	{
		this._jump.velocity.y = this._jump.velocity.y + this._jump.gravity * (0.5f * this._dt);
		if (this._jump.velocity.x != 0f)
		{
			this.direction = Mathf.Sign(this._jump.velocity.x);
		}
		float num = Mathf.Max(this.momentumAbs, this.settings.run.initialMomentum);
		if (num > 1f)
		{
			num = 1f;
		}
		float num2 = Mathf.Abs(Simulate.SignedSpeedOnGround(num, 0f, false, this.settings.run));
		num2 = Mathf.Max(Mathf.Abs(this._jump.velocity.x), num2);
		num2 = Mathf.Max(Mathf.Abs(this._prevVelocity.x), num2);
		this._jump.airControlSpeedLimit = new Range(-num2, num2);
		if (this.sliding)
		{
			if (this.currentSlopeDownDir > 0f)
			{
				this._jump.airControlSpeedLimit.min = 0f;
			}
			else
			{
				this._jump.airControlSpeedLimit.max = 0f;
			}
		}
		if (Time.time - this._lastJumpTime < 2f)
		{
			this._jumpsInARow++;
		}
		else
		{
			this._jumpsInARow = 0;
		}
		if (!this.isMusicRunning && this.stamina > 0.2f && (this._jumpsInARow >= 6 || this.stamina < 1f) && !DebugOptions.opts.staminaFreeJumps)
		{
			float num3 = Mathf.InverseLerp(6f, 10f, (float)this._jumpsInARow);
			float num4 = Mathf.Lerp(0.03f, 0.1f, num3);
			this.stamina -= num4;
		}
		this.state = Runner.State.Jumping;
	}

	// Token: 0x06000690 RID: 1680 RVA: 0x00039B20 File Offset: 0x00037D20
	[NullableContext(2)]
	private BalancePoint FindNearestBalancePoint(Vector2 startPos, Vector2 targetPos, float duration, bool standingJump, float momentumUsedForJump)
	{
		List<BalancePoint> list = Level.current.balancePoints.Nearby(targetPos, this.raycastNearbyRange, 20f, null);
		float num = duration * Simulate.SignedSpeedOnGround(1f, 0f, false, this.settings.run) + this.settings.balance.balanceExtraDistMargin;
		BalancePoint balancePoint = null;
		float num2 = float.MaxValue;
		float num3 = this._jumpDirIntent.y * 0.017453292f * 50f;
		Vector2 vector = new Vector2(this._jumpDirIntent.x * Mathf.Cos(num3), Mathf.Sin(num3));
		bool flag = this._jumpDirIntent != Vector2.zero;
		this._slopesUnderJumpScratch.Clear();
		if (this.currentSlope != null)
		{
			Simulate.FindOptions standardPredict = Simulate.FindOptions.standardPredict;
			standardPredict.traceCallback = delegate(Simulate.TraceState state)
			{
				if (!this._slopesUnderJumpScratch.Contains(state.slope))
				{
					this._slopesUnderJumpScratch.Add(state.slope);
				}
			};
			Simulate.FindGroundPositionAtTime(new TrackPosition
			{
				slope = this.currentSlope,
				x = startPos.x
			}, 1.5f * duration, momentumUsedForJump, standardPredict, this.settings);
		}
		float num4 = ((this._jumpDirIntent.x != 0f) ? Mathf.Sign(this._jumpDirIntent.x) : this.direction);
		if (this.wallSliding || this.climbing)
		{
			num4 = -this.direction;
		}
		foreach (BalancePoint balancePoint2 in list)
		{
			Vector2 vector2 = balancePoint2.transform.position;
			float num5 = vector2.x - this.position.x;
			float num6 = num4 * num5;
			if ((!balancePoint2.exitToSlope || this.balancing || !balancePoint2.exitToSlope || !(balancePoint2.slopeUnder == this.currentSlope)) && num6 >= -0.001f)
			{
				bool flag2 = !this.isMusicRunning && this.balancing && this._jumpDirIntent == Vector2.zero && standingJump;
				if (flag2 || !(balancePoint2 == this._balancePoint))
				{
					Vector2 vector3 = vector2 - this.position;
					float magnitude = vector3.magnitude;
					float num7 = 1f;
					float num8;
					if (flag2)
					{
						num8 = magnitude;
					}
					else if (balancePoint2.isWallClimb)
					{
						if (vector3.x < 0f)
						{
							vector3.x = -vector3.x;
						}
						if (!this.settings.balance.balanceWallClimbXRange.Contains(vector3.x) || !this.settings.balance.balanceWallClimbYRange.Contains(vector3.y))
						{
							continue;
						}
						num7 = 1f - Vector2.Dot(vector, vector3.normalized);
						num7 = Mathf.Lerp(1f, 2f, num7);
						num8 = num7 * this.settings.balance.balanceWallClimbDistDurationScalar * magnitude;
					}
					else
					{
						Vector2 vector4 = vector3;
						Vector2 vector5 = vector3;
						vector5.y /= this.settings.balance.maxRadiusOvalYScale;
						if (vector3.y < 0f)
						{
							float num9 = Mathf.InverseLerp(0f, 8f, -vector3.y);
							vector5.x *= Mathf.Lerp(1f, 0.8f, num9);
						}
						float magnitude2 = vector5.magnitude;
						if (magnitude2 > num)
						{
							if (magnitude2 * 2f < num)
							{
								continue;
							}
							continue;
						}
						else
						{
							if (vector3.y > this.settings.jump.jumpHeightRange.max)
							{
								continue;
							}
							if (vector4.y < 0f)
							{
								vector4.y *= 1.3f;
							}
							if (vector.magnitude > 0.05f)
							{
								num7 = 1f - Vector2.Dot(vector, vector3.normalized);
								num7 = Mathf.Lerp(1f, 2f, num7);
							}
							num8 = num7 * vector4.magnitude;
							Mathf.Atan2(vector3.y, Mathf.Abs(vector3.x));
							if (vector.y <= 0.5f && !flag2 && flag && Mathf.Abs(vector3.x) < this.settings.balance.balancePointMinDist)
							{
								continue;
							}
							float num10 = Slope.AngleWithVector(vector2 - this.position, true, null);
							if (!this.settings.balance.balanceDropGainAngleRange.Contains(num10) || (this.wallSliding && this.staminaIsVeryLow && num10 > 10f))
							{
								continue;
							}
						}
					}
					if (num8 < num2)
					{
						if (this._jumpDirIntent.y >= 0f)
						{
							bool flag3 = false;
							using (List<Slope>.Enumerator enumerator2 = this._slopesUnderJumpScratch.GetEnumerator())
							{
								while (enumerator2.MoveNext())
								{
									if (enumerator2.Current.HeightOfPointAbove(vector2) < 0f)
									{
										flag3 = true;
										break;
									}
								}
							}
							if (flag3)
							{
								continue;
							}
						}
						num2 = num8;
						balancePoint = balancePoint2;
					}
				}
			}
		}
		return balancePoint;
	}

	// Token: 0x06000691 RID: 1681 RVA: 0x0003A078 File Offset: 0x00038278
	[NullableContext(2)]
	private BalancePoint FindNearestBalancePointDropDown(Vector2 startPos)
	{
		List<BalancePoint> list = Level.current.balancePoints.Nearby(startPos, this.raycastNearbyRange, 5f, null);
		BalancePoint balancePoint = null;
		float num = float.MinValue;
		foreach (BalancePoint balancePoint2 in list)
		{
			if (!(balancePoint2 == this.lastBalancePointDroppedFrom))
			{
				Vector2 vector = balancePoint2.transform.position;
				Vector2 vector2 = vector - startPos;
				if (vector2.y <= -1f)
				{
					vector2.x = Mathf.Abs(vector2.x);
					vector2.x = Mathf.Max(vector2.x - 2f, 0f);
					if (vector.y >= startPos.y - this.settings.dropDownRayDist && Vector2.Angle(Vector2.down, vector2) <= 15f && vector.y > num)
					{
						balancePoint = balancePoint2;
						num = vector.y;
					}
				}
			}
		}
		return balancePoint;
	}

	// Token: 0x06000692 RID: 1682 RVA: 0x0003A198 File Offset: 0x00038398
	private bool TryLandOnBalancePointOrFall(Vector2 fallCurr, Vector2 fallNext)
	{
		if (this.falling && this.prevState == Runner.State.Balancing)
		{
			return false;
		}
		foreach (BalancePoint balancePoint in Level.current.balancePoints.Nearby(fallCurr, this.raycastSafeNearbyRange, 5f, null))
		{
			if ((!this.jumping || !(this._jump.targetBalancePoint != null) || !(this._jump.targetBalancePoint != balancePoint)) && !(balancePoint == this.lastBalancePointDroppedFrom))
			{
				Vector2 vector = balancePoint.transform.position;
				if (this.falling && this.prevState == Runner.State.Running && vector.y > this._fallStartY - 0.2f)
				{
					return false;
				}
				float num2;
				if (fallNext.y <= vector.y && fallCurr.y > vector.y)
				{
					if (this.jumping && balancePoint == this._prevBalancePoint && this._jump.targetBalancePoint != null && balancePoint != this._jump.targetBalancePoint)
					{
						continue;
					}
					float num = Mathf.InverseLerp(fallCurr.y, fallNext.y, vector.y);
					if (Mathf.Abs(Mathf.Lerp(fallCurr.x, fallNext.x, num) - vector.x) >= 1f)
					{
						continue;
					}
					num2 = 1f - num;
				}
				else
				{
					if (balancePoint == this._prevBalancePoint)
					{
						continue;
					}
					bool flag = Mathf.Abs(this.velocity.y) < this.settings.jump.balanceSnapLowVelocity;
					Vector2 vector2 = vector - this.position;
					vector2.y *= 2f;
					if (!flag || vector2.magnitude >= 1f)
					{
						continue;
					}
					num2 = 1f;
				}
				this.StartBalancingMaybeFall(balancePoint, num2);
				return true;
			}
		}
		return false;
	}

	// Token: 0x06000693 RID: 1683 RVA: 0x0003A3CC File Offset: 0x000385CC
	[NullableContext(1)]
	private void StartBalancingMaybeFall(BalancePoint balancePoint, float remainingFrameNorm)
	{
		this.position = balancePoint.transform.position;
		if (this.state == Runner.State.Jumping && this._jump.requireRetroactiveJumpPress)
		{
			if (balancePoint.exitToSlope)
			{
				this.state = Runner.State.Tripping;
				return;
			}
			this.EscalateFallDamage(Damage.MinorDamage);
			this.state = Runner.State.Falling;
			return;
		}
		else
		{
			if (balancePoint.exitToSlope)
			{
				this.currentSlope = balancePoint.slopeUnder;
				this.state = Runner.State.Running;
				return;
			}
			this._balancePoint = balancePoint;
			this.state = Runner.State.Balancing;
			if (this.state == Runner.State.Jumping)
			{
				float num = remainingFrameNorm * this._dt;
				float dt = this._dt;
				this._dt = num * this._dt;
				this.State_Jumping(false, false);
				this._dt = dt;
			}
			return;
		}
	}

	// Token: 0x06000694 RID: 1684 RVA: 0x0003A484 File Offset: 0x00038684
	private bool AllowQueuedJump()
	{
		return this._jumpQueued && Time.time - this._jumpQueuedTime < this.settings.jump.jumpQueueGraceTime && this.TryJump(false);
	}

	// Token: 0x06000695 RID: 1685 RVA: 0x0003A4B8 File Offset: 0x000386B8
	private bool JumpWouldBeIntoWall()
	{
		Vector2 vector = 2f * this.direction * Vector2.right;
		return Raycast.CollideWallPolygonsVec3(base.transform.position + 4f * Vector3.up, vector, this.raycastCurrentAndSafeForeRange, this.settings, false, null, false, default(Color)).didHit;
	}

	// Token: 0x06000696 RID: 1686 RVA: 0x0003A524 File Offset: 0x00038724
	private void OnJumpDuringMusicRun()
	{
		RunTrack instance = MonoSingleton<RunTrack>.instance;
		BeatTrack.ObstacleRef obstacleRef = instance.FindNearestObstacle();
		float num = Mathf.Abs(obstacleRef.time - MonoSingleton<RunTrack>.instance.currentMusicTime);
		float currentBeatDuration = instance.currentBeatDuration;
		if (num > 0.45f * currentBeatDuration)
		{
			return;
		}
		RhythmActionMarker rhythmActionMarker = MonoSingleton<TrackBuilder>.instance.MarkerForObstacle(obstacleRef);
		if (rhythmActionMarker == null)
		{
			return;
		}
		bool flag;
		bool flag2;
		if (this.jumpIsSpecial != rhythmActionMarker.isSpecialJump)
		{
			this.TripSpecialMismatch();
			flag = false;
			flag2 = false;
		}
		else
		{
			flag2 = num < 0.1f;
			this.musicalTrail.CreateSuccessNote(flag2, rhythmActionMarker.isSpecialJump);
			flag = true;
		}
		if (flag)
		{
			this._stumbleCountInRow = 0;
		}
		BeatTrack.ObstacleRef firstObstacleRef = instance.track.firstObstacleRef;
		if (obstacleRef.IsSameTimeAs(firstObstacleRef) && Time.time - this._lastWhoopTime > 4f)
		{
			this._lastWhoopTime = Time.time;
		}
		else if (flag && instance.signedTimeSinceMusicStart > 15f && Time.time - this._lastWhoopTime > 6f && Random.value < 0.25f)
		{
			AudioController.instance.PlayVocalisation(Vocalisation.MusicRunningWhoops, 0f);
			this._lastWhoopTime = Time.time;
		}
		if (Runner.onDidJumpObstacle != null)
		{
			Runner.onDidJumpObstacle(obstacleRef, flag, flag2, num);
		}
	}

	// Token: 0x1700018E RID: 398
	// (get) Token: 0x06000697 RID: 1687 RVA: 0x0003A663 File Offset: 0x00038863
	public Runner.JumpState jump
	{
		get
		{
			return this._jump;
		}
	}

	// Token: 0x06000698 RID: 1688 RVA: 0x0003A66C File Offset: 0x0003886C
	private void PlayAnimation_StateFunc(bool start, bool end)
	{
		if (start)
		{
			this._exitingAnimation = false;
			this._activePlayAnim = this._nextPlayAnim;
			this._nextPlayAnim = null;
			return;
		}
		if (end)
		{
			this._activePlayAnim = null;
			this._exitingAnimation = false;
			return;
		}
		if (this._nextPlayAnim != null && this.animator.IsAnimation(this._nextPlayAnim.animName, null, null, null))
		{
			this._activePlayAnim = this._nextPlayAnim;
			this._nextPlayAnim = null;
		}
		if (this.playerControlDisabled == PlayerControlDisableReason.None && Mathf.Abs(GameInput.moveLeftRight) > 0f && this.state == Runner.State.InkPose)
		{
			this.ExitAnimation();
			this.state = Runner.State.Running;
			return;
		}
		if (this.animator.IsAnimation(this._activePlayAnim.animName, null, null, null) && this.animator.currentAnimation.mode == FrameAnimation.Mode.Clamp && this.animator.normalizedTime >= 1f)
		{
			this.ExitAnimation();
		}
		if (this._exitingAnimation && this.animator.IsAnimation("Idle", null, null, null))
		{
			this.state = Runner.State.Running;
			return;
		}
	}

	// Token: 0x06000699 RID: 1689 RVA: 0x0003A77C File Offset: 0x0003897C
	[NullableContext(1)]
	public void PlayAnimation(string inkAnimName, Runner.State animState)
	{
		Runner.AnimWithTransitions animWithTransitions = null;
		foreach (Runner.AnimWithTransitions animWithTransitions2 in Runner._anims)
		{
			if (animWithTransitions2.inkName == inkAnimName)
			{
				animWithTransitions = animWithTransitions2;
				break;
			}
		}
		if (animWithTransitions == null)
		{
			Debug.LogError("Animation for RunAnim not found: " + inkAnimName);
			return;
		}
		this._nextPlayAnim = animWithTransitions;
		if (this._nextPlayAnim.transitionIn != null)
		{
			this.animator.SetAnimationWithTransition(this._nextPlayAnim.transitionIn, this._nextPlayAnim.animName, 0, false, false, FrameAnimator.PosMatch.None);
		}
		else
		{
			this.animator.SetAnimation(this._nextPlayAnim.animName, FrameAnimator.PosMatch.None);
		}
		this._exitingAnimation = false;
		this.state = animState;
	}

	// Token: 0x0600069A RID: 1690 RVA: 0x0003A850 File Offset: 0x00038A50
	public void ExitAnimation()
	{
		if (this._exitingAnimation || this._activePlayAnim == null)
		{
			return;
		}
		if (this._activePlayAnim.transitionOut != null)
		{
			this.animator.SetAnimationWithTransition(this._activePlayAnim.transitionOut, "Idle", 0, false, false, FrameAnimator.PosMatch.None);
		}
		else
		{
			this.animator.SetAnimation("Idle", FrameAnimator.PosMatch.None);
		}
		this._exitingAnimation = true;
	}

	// Token: 0x1700018F RID: 399
	// (get) Token: 0x0600069B RID: 1691 RVA: 0x0003A8B4 File Offset: 0x00038AB4
	// (set) Token: 0x0600069C RID: 1692 RVA: 0x0003A8C7 File Offset: 0x00038AC7
	public static bool singleJumpButtonOnly
	{
		get
		{
			if (!Runner._singleJumpButtonOnlyCached)
			{
				Runner.RefreshSingleJumpButtonCache();
			}
			return Runner._singleJumpButtonOnly;
		}
		set
		{
			Runner._singleJumpButtonOnly = value;
			PlayerPrefsX.SetInt("singleJumpButton", value ? 1 : 0);
			Runner._singleJumpButtonOnlyCached = true;
		}
	}

	// Token: 0x0600069D RID: 1693 RVA: 0x0003A8E6 File Offset: 0x00038AE6
	public static void RefreshSingleJumpButtonCache()
	{
		Runner._singleJumpButtonOnly = PlayerPrefsX.GetInt("singleJumpButton", 0) != 0 || PlayerPrefsX.GetInt("MusicRunningEasyMode", 0) != 0;
		Runner._singleJumpButtonOnlyCached = true;
	}

	// Token: 0x17000190 RID: 400
	// (get) Token: 0x0600069E RID: 1694 RVA: 0x0003A911 File Offset: 0x00038B11
	public int stumbleCountInRow
	{
		get
		{
			return this._stumbleCountInRow;
		}
	}

	// Token: 0x17000191 RID: 401
	// (get) Token: 0x0600069F RID: 1695 RVA: 0x0003A91C File Offset: 0x00038B1C
	private float sprintBoost
	{
		get
		{
			this._sprintTimer = Mathf.Max(this._sprintTimer - this._dt, 0f);
			float num = this._sprintTimer / this.settings.run.sprintBoostTimer;
			return Mathf.Lerp(1f, this.settings.run.sprintBoost, num);
		}
	}

	// Token: 0x060006A0 RID: 1696 RVA: 0x0003A97C File Offset: 0x00038B7C
	private void State_RunningSliding(bool start, bool end)
	{
		if (this.currentSlope == null)
		{
			Debug.LogError("In run state without a currentSlope");
		}
		if (start)
		{
			if (this.sliding && !this.isMusicRunning && this.direction * this.currentSlopeDownDir < 0f && (this.prevState == Runner.State.Jumping || this.prevState == Runner.State.Falling))
			{
				float num = Mathf.InverseLerp(0.2f, 0.6f, Mathf.Abs(this.momentum));
				float num2 = Mathf.Lerp(0f, this.settings.run.slideInitialMaxMomentumLoss, num);
				num2 = Mathf.Clamp(num2, -Mathf.Abs(this.momentum), Mathf.Abs(this.momentum));
				this.momentum -= Mathf.Sign(this.momentum) * num2;
			}
			this.UpdateLastRunPos();
			this.lastSlopeJumpedFrom = null;
			this.lastBalancePointDroppedFrom = null;
			this._runningLeanAngle = 0f;
			this._sprintTimer = 0f;
			this._prevVelocity = this.velocityOnGround;
			if (this.AllowQueuedJump())
			{
				return;
			}
			this.UpdateRunStateAnimations(this._leftRightInput, false, false, false);
			if (this.sliding && Time.time - this._lastSlidingVocalisationTime > 10f)
			{
				AudioController.instance.PlayVocalisation(Vocalisation.SlideStart, 0f);
				this._lastSlidingVocalisationTime = Time.time;
			}
			return;
		}
		else
		{
			if (end)
			{
				if (this.sliding)
				{
					this._slideParticles.Stop();
				}
				this._slowDownParticles.Stop();
				this._lastReverseInterruptTime = -1f;
				this._stumbleTimer = 0f;
				this._sprintTimer = 0f;
				this.animator.speed = 1f;
				return;
			}
			if (this.sliding != this.onSlide)
			{
				if (this.onSlide)
				{
					this.state = Runner.State.Sliding;
					return;
				}
				this.state = Runner.State.Running;
				return;
			}
			else
			{
				if (this.sliding)
				{
					InputVibration.LongSoft();
				}
				if (this.sliding && this.StateTimeJustPassed(2f) && Random.value < 0.6f)
				{
					AudioController.instance.PlayVocalisation(Vocalisation.SlideOrWallSlideMidWobble, 0f);
				}
				float num3 = this._leftRightInput;
				if (NarrativePresenter.activeChoiceCount < 2)
				{
					bool flag = (this._move2dInput.y < 0f && this._climbPromptAvailable && !this._climbPromptIsUp) || (this._move2dInput.y > 0f && this._climbPromptAvailable && this._climbPromptIsUp);
					float magnitude = this._move2dInput.magnitude;
					if (magnitude > 0.9f && Mathf.Abs(magnitude) > num3 && Mathf.Abs(this.currentSample.angle) > 20f && !flag)
					{
						Vector2 normal = this.currentSample.normal;
						Vector2 vector = new Vector2(normal.y, -normal.x);
						float num4 = Vector2.Angle(vector, this._move2dInput);
						if (Vector2.Angle(-vector, this._move2dInput) < 65f)
						{
							num3 = -magnitude;
						}
						else if (num4 < 65f)
						{
							num3 = magnitude;
						}
					}
				}
				bool flag2 = false;
				bool flag3 = false;
				bool flag4 = false;
				bool flag5 = false;
				this.UpdateRunStateMomentum(num3, ref flag5, ref flag2, ref flag3, ref flag4);
				if (flag4 && this.sliding && this.momentumAbs < this.settings.run.slideBackToRunMomentumThreshold)
				{
					this.state = Runner.State.Running;
					return;
				}
				Simulate.FindOptions findOptions = Simulate.FindOptions.standardSimulate;
				findOptions.takeUphillForks = this._useUphillSlopeConnections;
				Simulate.FindOptions findOptions2 = findOptions;
				findOptions2.predictMode = true;
				if (this._stumbleTimer > 0f)
				{
					findOptions = findOptions2;
				}
				Simulate.FindResult findResult = Simulate.FindGroundPositionAtTime(this.trackPosition, this._dt, this.momentum * this.reverseScalar * this.sprintBoost, findOptions, this.settings);
				Vector2 vector2 = findResult.sample.point;
				Slope slope = findResult.sample.slope;
				if (findResult.flipped)
				{
					this.momentum = -this.momentum;
				}
				if (this.TripsBetween(this.position.x, vector2.x))
				{
					if (this._stumbleCount >= this.settings.run.stumbleCountMax && Level.currentIndex != 0 && PlayerPrefsX.GetInt("dontMusicRunTrip", 0) != 1)
					{
						this.state = Runner.State.Tripping;
						return;
					}
					this.Stumble();
				}
				bool flag6 = findResult.sample.angle * Mathf.Sign(this.momentum) > 0f;
				if (!this.onSlide && this.SlidesOnSlope(slope, findResult.sample.angle) && flag6 && this.momentumAbs < 0.25f)
				{
					History.Log("Slowly moved onto slide so clamped at edge");
					this.momentum = 0f;
					slope = this.currentSlope;
					vector2 = this.position;
				}
				if (this.isMusicRunning && this.inMusicRunningArea && findResult.remainingTime > 0f && this.momentum != 0f && this._stumbleTimer == 0f)
				{
					if (this._stumbleCount >= this.settings.run.stumbleCountMax && Level.currentIndex != 0 && PlayerPrefsX.GetInt("dontMusicRunTrip", 0) != 1)
					{
						this.state = Runner.State.Tripping;
						return;
					}
					TrackPosition trackPosition = new TrackPosition
					{
						x = findResult.sample.point.x,
						slope = findResult.sample.slope
					};
					if (Simulate.FindGroundPositionAtTime(trackPosition, 0.1f, this.direction, findOptions2, this.settings).sample.point.y - this.position.y > 0f)
					{
						Simulate.FindResult findResult2 = Simulate.FindGroundPositionAtTime(trackPosition, findResult.remainingTime, this.momentum * this.reverseScalar, findOptions2, this.settings);
						vector2 = findResult2.sample.point;
						slope = findResult2.sample.slope;
						if (findResult2.flipped)
						{
							this.momentum = -this.momentum;
						}
						findResult = findResult2;
						this.Stumble();
					}
				}
				if (this._stumbleTimer > 0f)
				{
					this._stumbleTimer -= this._dt;
					if (this._stumbleTimer < 0f)
					{
						this._stumbleTimer = 0f;
					}
				}
				bool flag7 = this.isOnSteepSlopeUpward && num3 * this.direction > 0f;
				if (flag7 && this.TrySetupClimbing(ClimbCheckRange.GroundAutoClimb, true, true, false).isValid)
				{
					return;
				}
				if ((!this.isMusicRunning || !this.inMusicRunningArea) && this.UpdateRunStatePolyInteractionOval(num3, ref vector2, ref slope))
				{
					return;
				}
				if (this.hasAutoRunTarget && Mathf.Sign(this.autoRunTargetX - this.position.x) != Mathf.Sign(this.autoRunTargetX - vector2.x))
				{
					vector2.x = this.autoRunTargetX;
					this.momentum = 0f;
				}
				Vector2 vector3;
				if (Raycast.InvisibleCollision(this.position, vector2, this.raycastCurrentRangeONLY, out vector3, false))
				{
					vector2 = vector3;
					this.momentum = 0f;
					if (!slope.range.Contains(vector3.x))
					{
						slope = this.currentSlope;
					}
				}
				if (slope != this.currentSlope && !this.currentSlope.isSlide && slope.isSlide && num3 == 0f && this.direction * -findResult.sample.angle < 0f)
				{
					vector2 = this.position;
					slope = this.currentSlope;
					this.momentum = 0f;
				}
				this.position = vector2;
				if (findResult.remainingTime > 0f && this.momentum != 0f && this.UpdateRunStateSlopeEdges(findResult.remainingTime, findResult.sample.slope, ref vector2, ref slope, flag7))
				{
					return;
				}
				this.currentSlope = slope;
				if (this.currentSlope != null)
				{
					this.physicalDepthLayerIdx = Mathf.RoundToInt(this.currentSlope.transform.position.z);
				}
				float num5;
				float num6;
				if (this.animator.IsAnimation("Scramble", null, null, null) && this.currentSample.isValid)
				{
					num5 = this.direction * this.RotationToClimbOnNormal(this.currentSample.normal) + 45f;
					num6 = this.settings.run.rotateSpeed.Lerp(1f);
				}
				else if (this.isSprinting)
				{
					num5 = -this.settings.run.sprintLeanAngle * Mathf.Clamp01(this.momentumAbs);
					num6 = this.settings.run.sprintRotateSpeed;
				}
				else
				{
					num5 = 0f;
					num6 = this.settings.run.rotateSpeed.Lerp(1f);
				}
				this._runningLeanAngle = Mathf.MoveTowardsAngle(this._runningLeanAngle, num5, num6 * this._dt);
				this.rotation = this.direction * this._runningLeanAngle;
				this.UpdateLastRunPos();
				this.UpdateRunStateAnimations(num3, flag2, flag3, flag4 || this.stoppedBasically);
				ParticlesX.SetActive(this._slowDownParticles, flag5 || this.animator.IsAnimation("RunDirectionChange", null, null, null) || this.animator.IsAnimation("RunToIdle", null, null, null) || this.animator.IsAnimation("IdleToRun", null, null, null));
				ParticlesX.SetActive(this._slideParticles, this.sliding);
				if (this._sprintTimer > 0f)
				{
					ParticleSystem.EmitParams emitParams = default(ParticleSystem.EmitParams);
					Vector3 vector4 = Vector3.right;
					Vector3 vector5 = Vector3.up;
					if (this.currentSample.isValid)
					{
						vector5 = this.currentSample.normal;
						vector4 = new Vector2(vector5.y, -vector5.x);
					}
					emitParams.velocity = this.settings.run.sprintParticlesVelocity.x * -this.direction * vector4 + this.settings.run.sprintParticlesVelocity.y * vector5;
					int num7 = Mathf.RoundToInt(this._sprintTimer * (float)this.settings.run.sprintParticleBurstCount);
					this._sprintDustParticles.Emit(emitParams, num7);
				}
				if (this.staminaIsVeryLow && !this.climbing && !this.sliding && !this.isMusicRunning && !this.bellyWriggling && this.momentumAbs > 0.15f && Time.time > this._nextLowStaminaHurtTime)
				{
					this.health.ApplyDamage(DamageType.LowStaminaHurt, Damage.MinorDamage);
					this._nextLowStaminaHurtTime = Time.time + this.settings.run.runningWithLowStaminaInterval;
				}
				return;
			}
		}
	}

	// Token: 0x060006A1 RID: 1697 RVA: 0x0003B430 File Offset: 0x00039630
	private bool TripsBetween(float x0, float x1)
	{
		foreach (TripHazard tripHazard in Level.current.trips.Nearby(new Vector2(x0, 0f), Range.Centered((float)this.physicalDepthLayerIdx, 2f), 0f, null))
		{
			if (tripHazard.gameObject.activeInHierarchy)
			{
				Vector3 position = tripHazard.transform.position;
				if (Mathf.Abs(position.z - (float)this.physicalDepthLayerIdx) <= 0.5f && Vector2.Distance(position, this.position) <= 2f && ((x0 <= position.x && x1 >= position.x) || (x1 <= position.x && x0 >= position.x)))
				{
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x060006A2 RID: 1698 RVA: 0x0003B520 File Offset: 0x00039720
	private void Stumble()
	{
		this._stumbleCount++;
		this._stumbleCountInRow++;
		Debug.Log("Stumble " + this._stumbleCount.ToString());
		if (this.isMusicRunning)
		{
			MonoSingleton<RunTrack>.instance.PlayerDidStumble();
		}
		float num = (this.isMusicRunning ? this.runTrack.currentBeatDuration : 0.5f);
		this._stumbleTimer = 1.5f * num;
		if (this._stumbleCount <= 3 || this._stumbleCount == 5 || this._stumbleCount == 8)
		{
			AudioController.instance.PlayVocalisation(Vocalisation.MusicRunStumble, 0f);
		}
		if (this._stumbleCount <= 3)
		{
			this.health.ApplyDamage(DamageType.Stumble, Damage.MinorDamage);
		}
		if (!this.animator.IsAnimation("Stumble", null, null, null))
		{
			this.animator.SetAnimationWithTransition("Stumble", "Run", 0, true, false, FrameAnimator.PosMatch.None);
		}
	}

	// Token: 0x060006A3 RID: 1699 RVA: 0x0003B60E File Offset: 0x0003980E
	private void UpdateStumbleCount()
	{
		if (!this.isMusicRunning)
		{
			this._stumbleCount = 0;
		}
	}

	// Token: 0x060006A4 RID: 1700 RVA: 0x0003B620 File Offset: 0x00039820
	private void UpdateLastRunPos()
	{
		if (this.currentSlope == null)
		{
			return;
		}
		this.lastRunPos = new TrackPosition
		{
			chunk = this.currentSlope.GetComponentInParent<Chunk>(),
			slope = this.currentSlope,
			x = this.position.x
		};
		if (this.runTrack.playingOrAboutTo)
		{
			this.lastRunSlopeTime = this.runTrack.currentMusicTime;
			return;
		}
		this.lastRunSlopeTime = float.NegativeInfinity;
	}

	// Token: 0x060006A5 RID: 1701 RVA: 0x0003B6A8 File Offset: 0x000398A8
	private void UpdateRunStateMomentum(float runInput, ref bool reversingInput, ref bool justStartedReverse, ref bool accellerating, ref bool decelleratingToStop)
	{
		justStartedReverse = false;
		accellerating = false;
		decelleratingToStop = false;
		if (MonoSingleton<RunTrack>.instance.playerLockedIntoMusicRunStart)
		{
			runInput = Mathf.Sign(this.momentum);
		}
		float num = Mathf.Abs(runInput);
		reversingInput = runInput * this.momentum < 0f;
		bool flag = this.momentumAbs > 0.06f;
		bool flag2 = runInput * this.momentum > 0f;
		if (reversingInput && this._lastReverseInterruptTime < 0f && flag && !this.sliding)
		{
			justStartedReverse = true;
			this._lastReverseInterruptTime = Time.time;
		}
		else if (flag2 || !flag)
		{
			this._lastReverseInterruptTime = -1f;
		}
		bool flag3 = this._lastReverseInterruptTime >= 0f;
		bool flag4 = this.momentumAbs >= 1f && (this.inMusicRunningArea || this.timeOutsideMusicArea < this.settings.run.maxAutoRunOutsideMusicArea);
		bool flag5 = this.currentSlope.chunk != null;
		bool flag6 = flag3 && Time.time - this._lastReverseInterruptTime < this.settings.run.reverseGraceTime && flag4 && !flag5;
		if (flag4)
		{
			this._wasLockedFullSpeedWithoutInput = true;
		}
		if (num > 0f || this.momentum < 0.1f)
		{
			this._wasLockedFullSpeedWithoutInput = false;
		}
		this.isSprinting = false;
		float num3;
		if (this.steepness < 0f)
		{
			float num2 = Mathf.InverseLerp(0.05f, 0.3f, Mathf.Abs(this.steepness));
			num3 = Mathf.Lerp(this.settings.run.maxStandardMomentum, this.settings.run.maxDownhillMomentum, num2);
			this.debugMomentumType = "standard/downhill";
		}
		else
		{
			float num4 = Mathf.InverseLerp(0.1f, 1f, Mathf.Abs(this.steepness));
			num3 = Mathf.Lerp(this.settings.run.maxStandardMomentum, this.settings.run.maxUphillMomentum, num4);
			this.debugMomentumType = "standard/uphill";
		}
		if (this.inSlowdownTrigger)
		{
			num3 = Mathf.Min(num3, this.settings.run.slowdownTriggerSpeedLimit);
			this.debugMomentumType = "slowdown trigger";
		}
		else if (this.sliding)
		{
			num3 = this.settings.run.maxSlideMomentum;
			this.debugMomentumType = "sliding";
		}
		else if (this.running && this.inMusicRunningArea)
		{
			num3 = 1f;
			this.debugMomentumType = "music run";
		}
		else if (this.running && this.staminaIsLow)
		{
			num3 = Mathf.Min(num3, this.settings.run.staminaMomentumMaxSlowdown);
			this.debugMomentumType = "stamina limit";
		}
		else if (this._sprintHeld)
		{
			num3 = this.settings.run.maxSprintMomentum;
			this.debugMomentumType = "sprint";
			this.isSprinting = true;
		}
		float num5 = Mathf.Max(-this.position.y, 0f);
		float num6 = Mathf.InverseLerp(0f, this.bounds.size.y * 0.5f, num5);
		num3 *= Mathf.Lerp(1f, 0.1f, num6);
		if (this._sprintHeld)
		{
			this.stamina -= this.settings.stamina.sprintCost * this._dt;
		}
		this.debugMomentumAccelType = "none";
		this.debugMomentumAccel = float.PositiveInfinity;
		if (this.running && num > 0f && !flag6 && this.momentumAbs < num3)
		{
			if (this._sprintPressed && !this.staminaIsLow)
			{
				if (this.momentumAbs < 1f)
				{
					this.momentum += this.settings.run.sprintStartBonus * Mathf.Sign(runInput);
				}
				this.stamina -= this.settings.stamina.sprintInitialCost;
				this._sprintTimer = this.settings.run.sprintBoostTimer;
			}
			if (this.momentumAbs < this.settings.run.initialMomentum && num3 > this.settings.run.initialMomentum)
			{
				this.debugMomentumAccel = this.settings.run.initialMomentumAccel;
				this.debugMomentumAccelType = "initial";
			}
			else
			{
				float num7 = Mathf.Clamp(this.steepness / 0.5f, -1f, 1f);
				if (num7 < 0f)
				{
					this.debugMomentumAccel = Mathf.Lerp(this.settings.run.downhillMomentumAccel, this.settings.run.standardMomentumAccel, num7 + 1f);
					this.debugMomentumAccelType = "downhill/standard";
				}
				else
				{
					this.debugMomentumAccel = Mathf.Lerp(this.settings.run.standardMomentumAccel, this.settings.run.uphillMomentumAccel, num7);
					this.debugMomentumAccelType = "uphill/standard";
				}
				if (this._sprintHeld)
				{
					this.debugMomentumAccel = this.settings.run.sprintMomentumAccel;
					this.debugMomentumAccelType = "sprint";
				}
				if (this.inMusicRunningArea)
				{
					this.debugMomentumAccel = Mathf.Max(this.debugMomentumAccel, this.settings.run.musicRunMomentumAccel);
					this.debugMomentumAccelType = "music run";
				}
				this.debugMomentumAccel = Mathf.Lerp(this.debugMomentumAccel, this.settings.run.waterMomentumAccelScalar * this.debugMomentumAccel, num6);
			}
			this.momentum += runInput * this.debugMomentumAccel * this._dt;
			this.momentum = Mathf.Clamp(this.momentum, -num3, num3);
			accellerating = true;
		}
		else if (this.sliding && this.currentSample.isValid)
		{
			float num8 = -Mathf.Sign(this.currentSample.angle);
			float num9 = this._leftRightInput * num8;
			bool flag7 = this.momentum * num8 < 0f;
			if (flag7 && runInput * num8 > 0f)
			{
				this.momentum = 0f;
			}
			float num10;
			if (this.isMusicRunning && num9 >= 0f)
			{
				num10 = num8 * num3;
			}
			else
			{
				float num11;
				if (num9 < 0f)
				{
					num11 = Mathf.Lerp(this.settings.run.slideNaturalMomentum, this.settings.run.slideMinMomentum, -num9);
				}
				else
				{
					num11 = Mathf.Lerp(this.settings.run.slideNaturalMomentum, 1f, num9);
				}
				num10 = num11 * num8 * num3;
			}
			float num12;
			if (flag7)
			{
				num12 = this.settings.run.slideDecelUpward;
			}
			else if (Mathf.Abs(num10) > Mathf.Abs(this.momentum) && num10 * this.momentum > 0f)
			{
				num12 = this.settings.run.slideAccelBySteepness.Lerp(Mathf.Clamp01(-this.steepness));
				accellerating = true;
			}
			else
			{
				num12 = this.settings.run.slideDecelBySteepness.Lerp(1f - Mathf.Clamp01(-this.steepness));
				decelleratingToStop = true;
			}
			this.momentum = Mathf.MoveTowards(this.momentum, num10, num12 * this._dt);
			this.debugMomentumAccelType = "sliding";
		}
		if (!this.sliding)
		{
			bool flag8 = num == 0f && !flag4;
			bool flag9 = this.running && (flag8 || flag3) && !flag6;
			bool flag10 = this.momentum * runInput < 0f && (!flag6 || this.sliding);
			bool flag11 = this.momentumAbs > num3;
			if (flag9 || flag10 || flag11)
			{
				decelleratingToStop = false;
				float num13;
				if (this.sliding)
				{
					num13 = this.settings.run.slidingSlowdown;
					this.debugMomentumAccelType = "sliding slowdown";
				}
				else if (this._wasLockedFullSpeedWithoutInput)
				{
					num13 = this.settings.run.slowdownAfterLock;
					this.debugMomentumAccelType = "slowdown after lock";
				}
				else if (flag11 && !flag9)
				{
					float num14 = Mathf.InverseLerp(0.2f, 0.5f, this.steepness);
					num13 = this.LerpLogCurve(this.settings.run.overSpeedLimitSlowdown, this.settings.run.overSpeedLimitSlowdownUphill, num14);
					this.debugMomentumAccelType = "over speed limit slowdown";
				}
				else
				{
					decelleratingToStop = true;
					num13 = this.settings.run.slowdown;
					this.debugMomentumAccelType = "slowdown";
				}
				float num15 = this.momentum * TimeX.Damping(num13);
				if (flag11 && this.isMusicRunning)
				{
					num15 = this.direction * num3;
				}
				this.momentum = num15;
				return;
			}
			this.debugMomentumAccelType = "None";
		}
	}

	// Token: 0x060006A6 RID: 1702 RVA: 0x0003BF8C File Offset: 0x0003A18C
	private float LerpLogCurve(float a, float b, float t)
	{
		float num = Mathf.Log(a);
		float num2 = Mathf.Log(b);
		return Mathf.Exp(Mathf.Lerp(num, num2, t));
	}

	// Token: 0x060006A7 RID: 1703 RVA: 0x0003BFB4 File Offset: 0x0003A1B4
	private void UpdateRunStateAnimations(float runInput, bool justStartedReverse, bool accellerating, bool decelleratingToStop)
	{
		bool flag = this.animator.IsOrWillBeAnimation("Run", "Idle", "Exhaustion", "Scramble", "Sliding", "Stumble");
		if (!this.animator.IsAnimation("Stumble", null, null, null) && !this.animator.IsAnimation("ClimbTurnToRun", null, null, null))
		{
			if (this.sliding && this.relativeSlopeAngle < 0f && (this.stateTimer > 0.5f || this.prevState != Runner.State.Running))
			{
				if (this.animator.IsAnimation("Run", "Scramble", null, null))
				{
					this.animator.SetAnimationWithTransition("RunToSliding", "Sliding", 0, false, false, FrameAnimator.PosMatch.None);
				}
				else if (this.animator.IsAnimation("Jump", "Fall", "JumpToFall", null))
				{
					this.animator.SetAnimationWithTransition("JumpToSliding", "Sliding", 0, false, false, FrameAnimator.PosMatch.None);
				}
				else
				{
					this.animator.SetAnimation("Sliding", FrameAnimator.PosMatch.None);
				}
			}
			else if (this.isOnSteepSlopeUpward && !this.isMusicRunning && Mathf.Abs(this.speed) < this.settings.run.maxSpeedForScambleAnim)
			{
				this.animator.SetAnimationWithTransition("RunToScramble", "Scramble", 0, false, false, FrameAnimator.PosMatch.None);
			}
			else if (justStartedReverse)
			{
				this.animator.SetAnimationWithTransition("RunDirectionChange", "Run", 0, true, false, FrameAnimator.PosMatch.None);
			}
			else if (this.animator.IsAnimation("RunDirectionChange", null, null, null))
			{
				if (this.stoppedBasically && Runner.instance.playerControlDisabled != PlayerControlDisableReason.None)
				{
					this.animator.SetAnimationWithTransition("RunToIdle", "Idle", 0, false, false, FrameAnimator.PosMatch.None);
				}
				else if (this.direction * runInput > 0f)
				{
					this.animator.SetAnimation("Run", FrameAnimator.PosMatch.None);
				}
			}
			else if (this.running && this.animator.IsAnimation("Sliding", null, null, null) && !this.stoppedBasically)
			{
				this.animator.SetAnimationWithTransition("SlidingToRun", "Run", 0, false, false, FrameAnimator.PosMatch.None);
			}
			else if (decelleratingToStop)
			{
				if (!this.stoppedBasically)
				{
					this.animator.SetAnimationWithTransition("RunToIdle", "Idle", 0, false, false, FrameAnimator.PosMatch.None);
				}
				else
				{
					bool hasAnyChoices = NarrativePresenter.instance.hasAnyChoices;
					bool flag2 = (this._lookDownTime > 0f && !hasAnyChoices) || this._lookDownTime > 0.3f;
					bool flag3 = !flag2 && this._restoringStamina && this.stoppedBasically;
					bool flag4 = !flag3 && !flag2 && MonoSingleton<Temperature>.instance.isCold;
					if (!this.animator.IsOrWillBeAnimation("Idle", "Shiver", "Exhaustion", "LookDown", null, null))
					{
						this.animator.SetAnimation("Idle", FrameAnimator.PosMatch.None);
					}
					if (!flag3 && this.animator.IsAnimation("Exhaustion", "ExhaustionToIdle", null, null))
					{
						bool flag5 = !this.animator.IsOrWillBeAnimation("Idle", null, null, null, null, null);
						this.animator.SetAnimationWithTransition("ExhaustionToIdle", "Idle", 0, false, false, FrameAnimator.PosMatch.None);
						if (flag5)
						{
							AudioController.instance.PlayVocalisation(Vocalisation.ExhaustionEnd, 0f);
						}
					}
					else if (!flag4 && this.animator.IsAnimation("Shiver", "ShiverToIdle", null, null))
					{
						this.animator.SetAnimationWithTransition("ShiverToIdle", "Idle", 0, false, false, FrameAnimator.PosMatch.None);
					}
					else if (!flag2 && this.animator.IsAnimation("LookDown", "LookDownToIdle", null, null))
					{
						this.animator.SetAnimationWithTransition("LookDownToIdle", "Idle", 0, false, false, FrameAnimator.PosMatch.None);
					}
					if (this.animator.IsAnimation("Idle", null, null, null))
					{
						if (flag3)
						{
							bool flag6 = !this.animator.IsOrWillBeAnimation("Exhaustion", null, null, null, null, null);
							this.animator.SetAnimationWithTransition("IdleToExhaustion", "Exhaustion", 0, false, false, FrameAnimator.PosMatch.None);
							if (flag6)
							{
								AudioController.instance.PlayVocalisation(Vocalisation.ExhaustionStart, 0f);
							}
						}
						else if (flag4)
						{
							this.animator.SetAnimationWithTransition("IdleToShiver", "Shiver", 0, false, false, FrameAnimator.PosMatch.None);
						}
						else if (flag2)
						{
							this.animator.SetAnimationWithTransition("IdleToLookDown", "LookDown", 0, false, false, FrameAnimator.PosMatch.None);
						}
					}
				}
			}
			else if (accellerating || (this.animator.IsAnimation("Idle", null, null, null) && !this.stoppedBasically))
			{
				if (this.stoppedBasically)
				{
					this.animator.SetAnimationWithTransition("IdleToRun", "Run", 1, false, false, FrameAnimator.PosMatch.None);
				}
				else
				{
					this.animator.SetAnimation("Run", FrameAnimator.PosMatch.None);
				}
			}
			else if (!flag || this.animator.IsAnimation("Scramble", null, null, null))
			{
				if (this.stoppedBasically)
				{
					this.animator.SetAnimation("Idle", FrameAnimator.PosMatch.None);
				}
				else
				{
					this.animator.SetAnimation("Run", FrameAnimator.PosMatch.None);
				}
			}
		}
		if (this.animator.IsAnimation("Run", "Stumble", "RunToSliding", "SlidingToRun"))
		{
			float num = this.settings.anim.speedExpectedForAnim;
			if (this.animator.IsAnimation("Run", null, null, null))
			{
				float relativeSlopeAngle = this.relativeSlopeAngle;
				if (relativeSlopeAngle > 15f)
				{
					this.animator.SetAnimationVariantNumber(1);
					num = this.settings.anim.speedExpectedUphillForAnim;
				}
				else if (relativeSlopeAngle < -15f)
				{
					this.animator.SetAnimationVariantNumber(2);
					num = this.settings.anim.speedExpectedDownhillForAnim;
				}
				else
				{
					this.animator.SetAnimationVariantNumber(0);
				}
			}
			if (this.isMusicRunning)
			{
				FrameAnimation currentAnimationVariant = this.animator.currentAnimationVariant;
				if (currentAnimationVariant != null)
				{
					float currentBeatDuration = this.runTrack.currentBeatDuration;
					float duration = currentAnimationVariant.duration;
					if (duration > 0f && currentBeatDuration > 0f)
					{
						float num2 = duration / currentBeatDuration;
						float num3 = (float)this.animator.frameIdx / (float)currentAnimationVariant.frames.Length;
						float fractionalBeatIdx = this.runTrack.fractionalBeatIdx;
						float num4 = fractionalBeatIdx - (float)Mathf.FloorToInt(fractionalBeatIdx);
						float num5 = num3 - num4 + this.settings.footstepTimeNorm;
						num5 = Mathf.Repeat(num5 + 0.25f, 0.5f) - 0.25f;
						float num6 = num2 - this.settings.footstepCatchupSpeed * num5;
						this.animator.speed = num6;
						return;
					}
				}
			}
			else
			{
				if (this.stoppedBasically)
				{
					this.animator.speed = 1f;
					return;
				}
				this.animator.speed = this.speed / num;
				return;
			}
		}
		else
		{
			if (this.animator.IsAnimation("Scramble", null, null, null))
			{
				this.animator.speed = this.speed / this.settings.run.scrambleExpectedSpeedForAnim;
				return;
			}
			this.animator.speed = 1f;
		}
	}

	// Token: 0x060006A8 RID: 1704 RVA: 0x0003C6BC File Offset: 0x0003A8BC
	[NullableContext(1)]
	private bool UpdateRunStatePolyInteractionOval(float runInput, ref Vector2 nextPosition, ref Slope nextSlope)
	{
		Range raycastCurrentAndSafeForeRange = this.raycastCurrentAndSafeForeRange;
		Vector2 vector = nextPosition - this.position;
		Vector2 vector2 = this.settings.ovalSweptHeightForRun * Vector2.up;
		Raycast.Collision collision = Raycast.OvalSwept<Poly>(this.position + vector2, this.rotation, this.settings.ovalSweptSizeForRun, vector, raycastCurrentAndSafeForeRange, null, true);
		if (collision.didHit && Vector2.Dot(collision.normal, vector) < 0f)
		{
			this.momentum = 0f;
			this._prevVelocity = Vector2.zero;
			nextPosition = this.position + collision.deltaNorm * vector;
			if (this.currentSlope != null && this.currentSlope.range.Contains(nextPosition.x))
			{
				nextSlope = this.currentSlope;
			}
			if (this.TryAutoClimbDueToCollisionWhileSlidingDown(collision))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x060006A9 RID: 1705 RVA: 0x0003C7B4 File Offset: 0x0003A9B4
	private bool TryAutoClimbDueToCollisionWhileSlidingDown(Raycast.Collision collision)
	{
		return this.onSlide && this.currentSlopeDownDir * this.direction > 0f && collision.climbable && this.TrySetupClimbing(ClimbCheckRange.GroundAutoClimb, true, false, false).isValid;
	}

	// Token: 0x060006AA RID: 1706 RVA: 0x0003C804 File Offset: 0x0003AA04
	[NullableContext(1)]
	private bool UpdateRunStateSlopeEdges(float remainingTimeStep, Slope lastSlope, ref Vector2 nextPosition, ref Slope nextSlope, bool canTransitionToClimb)
	{
		if (this.currentSlope == null)
		{
			return false;
		}
		FrameAnimation currentAnimation = this.animator.currentAnimation;
		if (this.direction * this.momentum < 0f || currentAnimation == null || currentAnimation.reversesDirection)
		{
			return false;
		}
		if (this.momentumAbs < this.settings.run.stopAtSafeEdgesMomentum && this._leftRightInput * this.momentum <= 0f)
		{
			this.momentum = 0f;
			this._prevVelocity = Vector2.zero;
			nextPosition = ((this.direction > 0f) ? this.currentSlope.rightPoint : this.currentSlope.leftPoint);
			return false;
		}
		SlopeSample slopeSample;
		if (Raycast.SampleWithDepthRange(this.position + 0.2f * Vector2.up, this.position - 1f * Vector2.up, (float)this.physicalDepthLayerIdx, this.raycastSafeForegroundONLY, out slopeSample, default(Color)).didHit)
		{
			nextPosition = slopeSample.point;
			nextSlope = slopeSample.slope;
			return false;
		}
		Vector3 vector = this.physicalPosition3d + this.settings.run.ledgeFeelerHeight * Vector3.up - this.direction * Vector3.right * this.settings.run.ledgeFeelerPullBack;
		Vector3 vector2 = this.direction * Vector3.right * this.settings.run.ledgeFeelerLength;
		Raycast.Collision collision = Raycast.CollideWallPolygonsVec3(vector, vector2, this.raycastCurrentAndNearForeRange, this.settings, false, null, false, Color.magenta);
		if (collision.didHit)
		{
			this.momentum = 0f;
			this._prevVelocity = Vector2.zero;
			nextPosition = ((this.direction > 0f) ? this.currentSlope.rightPoint : this.currentSlope.leftPoint);
			return this.TryAutoClimbDueToCollisionWhileSlidingDown(collision);
		}
		Simulate.NextSlopeResult nextSlopeResult = Simulate.NextSlope(lastSlope, this.momentum > 0f, Simulate.FindOptions.standardPredict);
		Vector2 vector3 = this.position + this.direction * 0.5f * Vector2.right;
		Vector2 vector4 = vector3 + 8f * Slope.VectorWithAngle(-60f, this.direction);
		SlopeSample slopeSample2;
		Raycast.RaycastResult raycastResult = Raycast.SampleWithDepthRange(vector3, vector4, (float)this.physicalDepthLayerIdx, this.raycastNearbyRange, out slopeSample2, default(Color));
		if (raycastResult.didHit && slopeSample2.point.y < 0f)
		{
			raycastResult.didHit = false;
		}
		bool didHit = raycastResult.didHit;
		bool flag = false;
		if (nextSlopeResult.slope != null)
		{
			Vector3 vector5 = ((this.momentum > 0f) ? nextSlopeResult.slope.leftPoint : nextSlopeResult.slope.rightPoint);
			flag = vector5.y < this.position.y - 0.01f && this.direction * (vector5.x - this.position.x) < 1f;
		}
		float num = Mathf.Sign(this.currentSample.angle);
		bool flag2 = this.sliding && num * this.momentum < 0f;
		if (flag || raycastResult.didHit || this.isMusicRunning || flag2)
		{
			if (flag || raycastResult.didHit || !this.isMusicRunning)
			{
			}
			float num2 = Simulate.SignedSpeedOnGround(this.momentum, 0f, lastSlope.isSlide, this.settings.run) * remainingTimeStep;
			this.position += num2 * Vector2.right;
			this.state = Runner.State.Falling;
			return true;
		}
		this.momentum = 0f;
		this._prevVelocity = Vector2.zero;
		nextPosition = ((this.direction > 0f) ? this.currentSlope.rightPoint : this.currentSlope.leftPoint);
		nextSlope = this.currentSlope;
		History.Log("Stopped at edge");
		this.state = Runner.State.CliffEdgeWobbling;
		return true;
	}

	// Token: 0x060006AB RID: 1707 RVA: 0x0003CC60 File Offset: 0x0003AE60
	private void State_Sitting(bool start, bool end)
	{
		this.PlayAnimation_StateFunc(start, end);
	}

	// Token: 0x060006AC RID: 1708 RVA: 0x0003CC6A File Offset: 0x0003AE6A
	public void StartSitting()
	{
		this.momentum = 0f;
		this.PlayAnimation("Sit", Runner.State.Sitting);
	}

	// Token: 0x060006AD RID: 1709 RVA: 0x0003CC84 File Offset: 0x0003AE84
	public void StopSitting()
	{
		this.ExitAnimation();
	}

	// Token: 0x17000192 RID: 402
	// (get) Token: 0x060006AE RID: 1710 RVA: 0x0003CC8C File Offset: 0x0003AE8C
	public bool hidden
	{
		get
		{
			return this.state == Runner.State.Hidden;
		}
	}

	// Token: 0x17000193 RID: 403
	// (get) Token: 0x060006AF RID: 1711 RVA: 0x0003CC97 File Offset: 0x0003AE97
	public bool running
	{
		get
		{
			return this.state == Runner.State.Running;
		}
	}

	// Token: 0x17000194 RID: 404
	// (get) Token: 0x060006B0 RID: 1712 RVA: 0x0003CCA2 File Offset: 0x0003AEA2
	public bool sliding
	{
		get
		{
			return this.state == Runner.State.Sliding;
		}
	}

	// Token: 0x17000195 RID: 405
	// (get) Token: 0x060006B1 RID: 1713 RVA: 0x0003CCAD File Offset: 0x0003AEAD
	public bool slidingOrSlideJumping
	{
		get
		{
			return this.state == Runner.State.Sliding || (this.state == Runner.State.Jumping && this.prevState == Runner.State.Sliding);
		}
	}

	// Token: 0x17000196 RID: 406
	// (get) Token: 0x060006B2 RID: 1714 RVA: 0x0003CCCE File Offset: 0x0003AECE
	public bool jumping
	{
		get
		{
			return this.state == Runner.State.Jumping;
		}
	}

	// Token: 0x17000197 RID: 407
	// (get) Token: 0x060006B3 RID: 1715 RVA: 0x0003CCD9 File Offset: 0x0003AED9
	public bool droppingDown
	{
		get
		{
			return this.state == Runner.State.Jumping && this._jump.isDropDown;
		}
	}

	// Token: 0x17000198 RID: 408
	// (get) Token: 0x060006B4 RID: 1716 RVA: 0x0003CCF1 File Offset: 0x0003AEF1
	public bool balancing
	{
		get
		{
			return this.state == Runner.State.Balancing;
		}
	}

	// Token: 0x17000199 RID: 409
	// (get) Token: 0x060006B5 RID: 1717 RVA: 0x0003CCFC File Offset: 0x0003AEFC
	public bool cliffEdgeWobbling
	{
		get
		{
			return this.state == Runner.State.CliffEdgeWobbling;
		}
	}

	// Token: 0x1700019A RID: 410
	// (get) Token: 0x060006B6 RID: 1718 RVA: 0x0003CD07 File Offset: 0x0003AF07
	public bool tripping
	{
		get
		{
			return this.state == Runner.State.Tripping;
		}
	}

	// Token: 0x1700019B RID: 411
	// (get) Token: 0x060006B7 RID: 1719 RVA: 0x0003CD12 File Offset: 0x0003AF12
	public bool stumbling
	{
		get
		{
			return this.animator.IsAnimation("Stumble", null, null, null);
		}
	}

	// Token: 0x1700019C RID: 412
	// (get) Token: 0x060006B8 RID: 1720 RVA: 0x0003CD27 File Offset: 0x0003AF27
	public bool falling
	{
		get
		{
			return this.state == Runner.State.Falling;
		}
	}

	// Token: 0x1700019D RID: 413
	// (get) Token: 0x060006B9 RID: 1721 RVA: 0x0003CD32 File Offset: 0x0003AF32
	public bool caught
	{
		get
		{
			return this.state == Runner.State.Caught;
		}
	}

	// Token: 0x1700019E RID: 414
	// (get) Token: 0x060006BA RID: 1722 RVA: 0x0003CD3E File Offset: 0x0003AF3E
	public bool climbing
	{
		get
		{
			return this.state == Runner.State.Climbing;
		}
	}

	// Token: 0x1700019F RID: 415
	// (get) Token: 0x060006BB RID: 1723 RVA: 0x0003CD4A File Offset: 0x0003AF4A
	public bool climbSlipping
	{
		get
		{
			return this.state == Runner.State.ClimbSlipping;
		}
	}

	// Token: 0x170001A0 RID: 416
	// (get) Token: 0x060006BC RID: 1724 RVA: 0x0003CD56 File Offset: 0x0003AF56
	public bool climbingOntoWall
	{
		get
		{
			return this.state == Runner.State.ClimbOntoWall;
		}
	}

	// Token: 0x170001A1 RID: 417
	// (get) Token: 0x060006BD RID: 1725 RVA: 0x0003CD62 File Offset: 0x0003AF62
	public bool climbingUpAndOver
	{
		get
		{
			return this.state == Runner.State.ClimbUpAndOver;
		}
	}

	// Token: 0x170001A2 RID: 418
	// (get) Token: 0x060006BE RID: 1726 RVA: 0x0003CD6E File Offset: 0x0003AF6E
	public bool bridgingGap
	{
		get
		{
			return this.climbingUpAndOver && this._activeTransition == this.settings.transition.bridgeGap;
		}
	}

	// Token: 0x170001A3 RID: 419
	// (get) Token: 0x060006BF RID: 1727 RVA: 0x0003CD92 File Offset: 0x0003AF92
	public bool wallSliding
	{
		get
		{
			return this.state == Runner.State.WallSliding;
		}
	}

	// Token: 0x170001A4 RID: 420
	// (get) Token: 0x060006C0 RID: 1728 RVA: 0x0003CD9E File Offset: 0x0003AF9E
	public bool sitting
	{
		get
		{
			return this.state == Runner.State.Sitting;
		}
	}

	// Token: 0x170001A5 RID: 421
	// (get) Token: 0x060006C1 RID: 1729 RVA: 0x0003CDAA File Offset: 0x0003AFAA
	public bool resting
	{
		get
		{
			return MonoSingleton<RestStateController>.instance.resting;
		}
	}

	// Token: 0x170001A6 RID: 422
	// (get) Token: 0x060006C2 RID: 1730 RVA: 0x0003CDB6 File Offset: 0x0003AFB6
	public bool catchingBreath
	{
		get
		{
			return this.animator.IsAnimation("Exhaustion", null, null, null);
		}
	}

	// Token: 0x170001A7 RID: 423
	// (get) Token: 0x060006C3 RID: 1731 RVA: 0x0003CDCB File Offset: 0x0003AFCB
	public bool hardLanding
	{
		get
		{
			return this.state == Runner.State.HardLanding;
		}
	}

	// Token: 0x170001A8 RID: 424
	// (get) Token: 0x060006C4 RID: 1732 RVA: 0x0003CDD7 File Offset: 0x0003AFD7
	public bool dead
	{
		get
		{
			return this.state == Runner.State.Dead;
		}
	}

	// Token: 0x170001A9 RID: 425
	// (get) Token: 0x060006C5 RID: 1733 RVA: 0x0003CDE3 File Offset: 0x0003AFE3
	public bool debugFlying
	{
		get
		{
			return this.state == Runner.State.DebugFlying;
		}
	}

	// Token: 0x170001AA RID: 426
	// (get) Token: 0x060006C6 RID: 1734 RVA: 0x0003CDEF File Offset: 0x0003AFEF
	public bool stoneSkimming
	{
		get
		{
			return this.state == Runner.State.StoneSkimming;
		}
	}

	// Token: 0x170001AB RID: 427
	// (get) Token: 0x060006C7 RID: 1735 RVA: 0x0003CDFB File Offset: 0x0003AFFB
	public bool bellyWriggling
	{
		get
		{
			return this.state == Runner.State.BellyWriggling;
		}
	}

	// Token: 0x170001AC RID: 428
	// (get) Token: 0x060006C8 RID: 1736 RVA: 0x0003CE07 File Offset: 0x0003B007
	public bool hasInkPose
	{
		get
		{
			return this.state == Runner.State.InkPose;
		}
	}

	// Token: 0x170001AD RID: 429
	// (get) Token: 0x060006C9 RID: 1737 RVA: 0x0003CE13 File Offset: 0x0003B013
	public bool onPath
	{
		get
		{
			return this.state == Runner.State.EnterExitPath;
		}
	}

	// Token: 0x170001AE RID: 430
	// (get) Token: 0x060006CA RID: 1738 RVA: 0x0003CE1F File Offset: 0x0003B01F
	public bool onChairLift
	{
		get
		{
			return this.state == Runner.State.ChairLift;
		}
	}

	// Token: 0x170001AF RID: 431
	// (get) Token: 0x060006CB RID: 1739 RVA: 0x0003CE2B File Offset: 0x0003B02B
	public bool onStoppedChairLift
	{
		get
		{
			return this.state == Runner.State.ChairLift && this._skiLiftStoppedWhileOnIt;
		}
	}

	// Token: 0x170001B0 RID: 432
	// (get) Token: 0x060006CC RID: 1740 RVA: 0x0003CE3F File Offset: 0x0003B03F
	public bool isBoating
	{
		get
		{
			return this.state == Runner.State.Boat;
		}
	}

	// Token: 0x170001B1 RID: 433
	// (get) Token: 0x060006CD RID: 1741 RVA: 0x0003CE4B File Offset: 0x0003B04B
	public bool isZipLining
	{
		get
		{
			return this.state == Runner.State.ZipLine || (this.state == Runner.State.Jumping && this._zipLine != null);
		}
	}

	// Token: 0x170001B2 RID: 434
	// (get) Token: 0x060006CE RID: 1742 RVA: 0x0003CE70 File Offset: 0x0003B070
	public bool inFinalJump
	{
		get
		{
			return this.state == Runner.State.FinalJump;
		}
	}

	// Token: 0x170001B3 RID: 435
	// (get) Token: 0x060006CF RID: 1743 RVA: 0x0003CE7C File Offset: 0x0003B07C
	public bool inFinalJumpAndCommittedToJumping
	{
		get
		{
			return this.state == Runner.State.FinalJump && this._finalJumpSubState >= Runner.FinalJumpSubState.WaitingForMusicSync;
		}
	}

	// Token: 0x170001B4 RID: 436
	// (get) Token: 0x060006D0 RID: 1744 RVA: 0x0003CE96 File Offset: 0x0003B096
	public bool inFinalJumpAndLeftLand
	{
		get
		{
			return this.state == Runner.State.FinalJump && this._finalJumpSubState >= Runner.FinalJumpSubState.Diving;
		}
	}

	// Token: 0x170001B5 RID: 437
	// (get) Token: 0x060006D1 RID: 1745 RVA: 0x0003CEB0 File Offset: 0x0003B0B0
	public bool exploded
	{
		get
		{
			return this.state == Runner.State.Exploded;
		}
	}

	// Token: 0x170001B6 RID: 438
	// (get) Token: 0x060006D2 RID: 1746 RVA: 0x0003CEBC File Offset: 0x0003B0BC
	// (set) Token: 0x060006D3 RID: 1747 RVA: 0x0003CEC4 File Offset: 0x0003B0C4
	public Runner.State state
	{
		get
		{
			return this._state;
		}
		set
		{
			if (this._state != value)
			{
				if (this._state != Runner.State.None)
				{
					this.RunState(this._state, false, true);
				}
				this.prevState = this._state;
				this._state = value;
				this.prevStateTimer = -0.033333335f;
				this.stateTimer = 0f;
				if (this._state != Runner.State.None)
				{
					this.RunState(this._state, true, false);
				}
			}
		}
	}

	// Token: 0x060006D4 RID: 1748 RVA: 0x0003CF30 File Offset: 0x0003B130
	private void RunState(Runner.State state, bool start, bool end)
	{
		switch (state)
		{
		case Runner.State.Hidden:
			this.State_Hidden(start, end);
			return;
		case Runner.State.Running:
			this.State_RunningSliding(start, end);
			return;
		case Runner.State.Sliding:
			this.State_RunningSliding(start, end);
			return;
		case Runner.State.Jumping:
			this.State_Jumping(start, end);
			return;
		case Runner.State.Balancing:
			this.State_Balancing(start, end);
			return;
		case Runner.State.CliffEdgeWobbling:
			this.State_CliffEdgeWobble(start, end);
			return;
		case Runner.State.Tripping:
			this.State_Tripping(start, end);
			return;
		case Runner.State.Falling:
			this.State_Falling(start, end);
			return;
		case Runner.State.Caught:
			this.State_Caught(start, end);
			return;
		case Runner.State.Climbing:
			this.State_Climbing(start, end);
			return;
		case Runner.State.ClimbSlipping:
			this.State_ClimbSlip(start, end);
			return;
		case Runner.State.ClimbOntoWall:
			this.State_ClimbOntoWall(start, end);
			return;
		case Runner.State.ClimbOffWall:
			this.State_ClimbOffWall(start, end);
			return;
		case Runner.State.ClimbingJump:
			this.State_ClimbingJump(start, end);
			return;
		case Runner.State.ClimbUpAndOver:
			this.State_ClimbUpAndOver(start, end);
			return;
		case Runner.State.WallSliding:
			this.State_WallSlide(start, end);
			return;
		case Runner.State.Sitting:
			this.State_Sitting(start, end);
			return;
		case Runner.State.HardLanding:
			this.State_HardLanding(start, end);
			return;
		case Runner.State.Dead:
			this.State_Death(start, end);
			return;
		case Runner.State.DebugFlying:
			this.State_DebugFlying(start, end);
			return;
		case Runner.State.StoneSkimming:
			this.State_StoneSkimming(start, end);
			return;
		case Runner.State.BellyWriggling:
			this.State_BellyWriggle(start, end);
			return;
		case Runner.State.InkPose:
			this.State_InkPose(start, end);
			return;
		case Runner.State.EnterExitPath:
			this.State_EnterExitPath(start, end);
			return;
		case Runner.State.EnterExitDoor:
			this.State_EnterExitDoor(start, end);
			return;
		case Runner.State.ChairLift:
			this.State_ChairLift(start, end);
			return;
		case Runner.State.Boat:
			this.State_Boat(start, end);
			return;
		case Runner.State.ZipLine:
			this.State_ZipLine(start, end);
			return;
		case Runner.State.FinalJump:
			this.State_FinalJump(start, end);
			return;
		case Runner.State.Exploded:
			this.State_Exploded(start, end);
			return;
		default:
			Debug.LogError(string.Format("State {0} doesn't have a state function defined", state));
			return;
		}
	}

	// Token: 0x060006D5 RID: 1749 RVA: 0x0003D0E5 File Offset: 0x0003B2E5
	private void UpdateStateMachine(float dt)
	{
		this.stateTimer += dt;
		if (this._state != Runner.State.None)
		{
			this.RunState(this._state, false, false);
			this.prevStateTimer = this.stateTimer;
		}
	}

	// Token: 0x060006D6 RID: 1750 RVA: 0x0003D117 File Offset: 0x0003B317
	private bool StateTimeJustPassed(float t)
	{
		return this.stateTimer >= t && this.prevStateTimer < t;
	}

	// Token: 0x060006D7 RID: 1751 RVA: 0x0003D130 File Offset: 0x0003B330
	private void State_StoneSkimming(bool start, bool end)
	{
		if (start)
		{
			this.momentum = 0f;
			if (this.stoneProp != null)
			{
				this.direction = (float)Mathf.RoundToInt(Mathf.Sign((float)this.stoneProp.preferredLookDirection));
			}
			else
			{
				if (!(this.stoneThrowTargetProp != null))
				{
					Debug.LogError("Beginning stone skimming but both stoneProp and stoneThrowTargetProp are null! Cancelling");
					this.state = Runner.State.Running;
					return;
				}
				this.direction = (float)Mathf.RoundToInt(Mathf.Sign(this.stoneThrowTargetProp.transform.position.x - this.position.x));
			}
			this.stoneSkimSubState = Runner.SkimStoneSubState.PickingUpRock;
			this.stoneSkim.warmUpSource.Play();
			this.stoneSkim.warmUpSource.volume = 0f;
			InputManager.ClearInputState();
			GameInput.PushControlStack(this);
			return;
		}
		if (end)
		{
			this.stoneProp = null;
			this.stoneThrowTargetProp = null;
			this.stoneSkim.warmUpSource.Stop();
			this.stoneSkim.warmUpSource.volume = 0f;
			this.RemoveStone();
			InputManager.ClearInputState();
			this.animator.SetAnimation("Idle", FrameAnimator.PosMatch.None);
			GameInput.PopControlStack(this, true);
			return;
		}
		this.UpdateStoneSkimmingSubState();
	}

	// Token: 0x060006D8 RID: 1752 RVA: 0x0003D268 File Offset: 0x0003B468
	private void RemoveStone()
	{
		if (this._stoneGO == null)
		{
			return;
		}
		this._stoneGO.SetParent(this.stoneSkim.stoneProto.transform.parent, false);
		this._stoneGO.GetComponent<Prototype>().ReturnToPool();
		this._stoneGO = null;
	}

	// Token: 0x060006D9 RID: 1753 RVA: 0x0003D2BC File Offset: 0x0003B4BC
	[NullableContext(1)]
	public void StartStoneSkimming(Prop prop)
	{
		this.stoneProp = prop;
		this.stoneThrowTargetProp = null;
		this.state = Runner.State.StoneSkimming;
	}

	// Token: 0x060006DA RID: 1754 RVA: 0x0003D2D4 File Offset: 0x0003B4D4
	[NullableContext(1)]
	public void StartStoneThrowAtTarget(Prop targetProp)
	{
		this.stoneProp = null;
		this.stoneThrowTargetProp = targetProp;
		this.state = Runner.State.StoneSkimming;
	}

	// Token: 0x060006DB RID: 1755 RVA: 0x0003D2EC File Offset: 0x0003B4EC
	public void EndStoneSkimming()
	{
		this.state = Runner.State.Running;
	}

	// Token: 0x170001B7 RID: 439
	// (get) Token: 0x060006DC RID: 1756 RVA: 0x0003D2F5 File Offset: 0x0003B4F5
	// (set) Token: 0x060006DD RID: 1757 RVA: 0x0003D2FD File Offset: 0x0003B4FD
	public Runner.SkimStoneSubState stoneSkimSubState
	{
		get
		{
			return this._stoneSkimSubState;
		}
		set
		{
			if (this._stoneSkimSubState == value)
			{
				return;
			}
			Runner.SkimStoneSubState stoneSkimSubState = this._stoneSkimSubState;
			this._stoneSkimSubState = value;
			this.EnterStoneSkimSubState(this.stoneSkimSubState);
		}
	}

	// Token: 0x170001B8 RID: 440
	// (get) Token: 0x060006DE RID: 1758 RVA: 0x0003D323 File Offset: 0x0003B523
	public Vector3 stonePosition
	{
		get
		{
			return this._stonePosition;
		}
	}

	// Token: 0x170001B9 RID: 441
	// (get) Token: 0x060006DF RID: 1759 RVA: 0x0003D32B File Offset: 0x0003B52B
	// (set) Token: 0x060006E0 RID: 1760 RVA: 0x0003D333 File Offset: 0x0003B533
	[Nullable(2)]
	public Prop stoneProp
	{
		[NullableContext(2)]
		get;
		[NullableContext(2)]
		private set;
	}

	// Token: 0x170001BA RID: 442
	// (get) Token: 0x060006E1 RID: 1761 RVA: 0x0003D33C File Offset: 0x0003B53C
	// (set) Token: 0x060006E2 RID: 1762 RVA: 0x0003D344 File Offset: 0x0003B544
	[Nullable(2)]
	public Prop stoneThrowTargetProp
	{
		[NullableContext(2)]
		get;
		[NullableContext(2)]
		private set;
	}

	// Token: 0x170001BB RID: 443
	// (get) Token: 0x060006E3 RID: 1763 RVA: 0x0003D34D File Offset: 0x0003B54D
	public float stoneDistance
	{
		get
		{
			return Mathf.Abs(this.position.x - this._stonePosition.x);
		}
	}

	// Token: 0x060006E4 RID: 1764 RVA: 0x0003D36C File Offset: 0x0003B56C
	private void EnterStoneSkimSubState(Runner.SkimStoneSubState state)
	{
		this.stoneSkim.warmUpSource.volume = 0f;
		if (state == Runner.SkimStoneSubState.PickingUpRock)
		{
			this.animator.SetAnimationWithTransition("PickUp", "SkimStoneReady", 0, false, false, FrameAnimator.PosMatch.None);
		}
		if (state == Runner.SkimStoneSubState.Ready)
		{
			this.animator.SetAnimation("SkimStoneReady", FrameAnimator.PosMatch.None);
			this.stoneSkim.twinkle.gameObject.SetActive(false);
		}
		if (state == Runner.SkimStoneSubState.WindingThrow)
		{
			this.animator.SetAnimation("SkimStonePullBack", FrameAnimator.PosMatch.None);
			this.stoneSkimWindingTime = 0f;
			this.stoneSkim.twinkle.gameObject.SetActive(true);
			this.stoneSkim.twinkle.color = Color.white.WithAlpha(0f);
		}
		if (state == Runner.SkimStoneSubState.Throw)
		{
			this.stoneSkim.twinkle.gameObject.SetActive(false);
		}
		if (state == Runner.SkimStoneSubState.Sink)
		{
			this._stoneSinkTimer = 0f;
			if (!Narrative.instance.isBusy)
			{
				Narrative.instance.RunKnot("CommentOnSkimming", null, false, false, new object[]
				{
					this._stoneSkimBounces,
					Mathf.Clamp01(this.stoneDistance / this.settings.stoneSkim.fullScoreNarrativeCommentDistance)
				});
			}
		}
	}

	// Token: 0x060006E5 RID: 1765 RVA: 0x0003D4AC File Offset: 0x0003B6AC
	private void UpdateStoneSkimmingSubState()
	{
		if (GameInput.Back(this))
		{
			GameInput.ClearInputState();
			if (this.stoneSkimSubState == Runner.SkimStoneSubState.WindingThrow)
			{
				this.stoneSkimSubState = Runner.SkimStoneSubState.Ready;
				return;
			}
			GameInput.PopControlStack(this, true);
			this.state = Runner.State.Running;
			return;
		}
		else
		{
			if (GameInput.moveLeftRight != 0f && this.stoneSkimSubState == Runner.SkimStoneSubState.Ready)
			{
				this.state = Runner.State.Running;
				return;
			}
			if (this.stoneProp != null && !this.stoneProp.triggerZone.triggering)
			{
				this.state = Runner.State.Running;
				return;
			}
			if (this.stoneSkimSubState == Runner.SkimStoneSubState.PickingUpRock)
			{
				if (!this.animator.IsAnimation("PickUp", null, null, null))
				{
					this.stoneSkimSubState = Runner.SkimStoneSubState.Ready;
				}
			}
			else if (this.stoneSkimSubState == Runner.SkimStoneSubState.Ready)
			{
				if (MonoSingleton<GameInput>.instance.mapping.throwStone.WasPressed)
				{
					this.stoneSkimSubState = Runner.SkimStoneSubState.WindingThrow;
				}
			}
			else if (this.stoneSkimSubState == Runner.SkimStoneSubState.WindingThrow)
			{
				this.stoneSkimWindingTime += Time.deltaTime;
				this.stoneSkim.warmUpSource.volume = (float)((this.stoneSkimSubState == Runner.SkimStoneSubState.WindingThrow) ? 1 : 0) * Mathf.InverseLerp(0.6f, 1f, this.settings.stoneSkim.strengthOverTime.Evaluate(this.stoneSkimWindingTime)) * 0.5f;
				float num = 0f;
				float num2 = 0f;
				if (this.stoneSkimWindingTime > this.settings.stoneSkim.minThrowTime)
				{
					num = this.settings.stoneSkim.strengthOverTime.Evaluate(this.stoneSkimWindingTime);
					num2 = this.settings.stoneSkim.twinkleStrengthOverStrength.Evaluate(num);
				}
				this.stoneSkim.twinkle.color = Color.white.WithAlpha(num2);
				if (MonoSingleton<GameInput>.instance.mapping.throwStone.WasReleased)
				{
					if (num == 0f)
					{
						this.stoneSkimSubState = Runner.SkimStoneSubState.Ready;
					}
					else
					{
						float value = Random.value;
						float num3 = Mathf.InverseLerp(0.85f, 1f, num);
						if (num3 > 0f)
						{
							num3 = Mathf.Lerp(0.1f, 0.4f, num3);
							this.stoneSkim.throwSource.PlayOneShot(this.settings.stoneSkim.successfulThrowChime, num3);
						}
						this.stoneSkim.throwSource.PlayOneShot(this.settings.stoneSkim.throwSwooshes.GetRandomClip(), num * 0.2f);
						if (this.stoneThrowTargetProp != null)
						{
							Vector3 position = this.stoneThrowTargetProp.transform.position;
							float num4 = position.y - (this.position.y + this.settings.stoneSkim.stoneReleaseLocalPosition.y);
							float num5 = Mathf.Sqrt(Mathf.Abs(num4 / (0.5f * this.settings.stoneSkim.gravity)));
							float num6 = this.settings.stoneSkim.initialSpeedOverStrength.Evaluate(1f);
							float num7 = Mathf.Max(Mathf.Abs(position.x - this.position.x) / num6, num5);
							float num8 = (position.x - this.position.x) / num7;
							float num9 = (num4 - 0.5f * this.settings.stoneSkim.gravity * num7 * num7) / num7;
							float num10 = Mathf.InverseLerp(0f, 0.5f, num);
							float num11 = Mathf.Lerp(0.5f, 1f, num10);
							float num12 = (position.z - (float)this._physicalDepth) / num5;
							this._stoneVelocity = num11 * new Vector3(num8, num9, num12);
						}
						else
						{
							float num13 = this.settings.stoneSkim.initialSpeedOverStrength.Evaluate(num) * this.settings.stoneSkim.initialSpeedMultiplierOverLuck.Evaluate(value);
							float num14 = this.settings.stoneSkim.throwAngle * 0.017453292f;
							Vector2 vector = new Vector2(Mathf.Sin(num14), Mathf.Cos(num14));
							if (this.direction < 0f)
							{
								vector.x = -vector.x;
							}
							this._stoneVelocity = vector * num13;
						}
						Vector2 stoneReleaseLocalPosition = this.settings.stoneSkim.stoneReleaseLocalPosition;
						stoneReleaseLocalPosition.x *= this.direction;
						this._stonePosition = base.transform.position + stoneReleaseLocalPosition;
						InputVibration.DoTimedVibration(TimedVibration.VibrateForSeconds(this.settings.stoneSkim.vibrationStrengthOverStrength.Evaluate(num), this.settings.stoneSkim.vibrationTime));
						this.animator.SetAnimationWithTransition("SkimStoneThrow", "Idle", 0, false, false, FrameAnimator.PosMatch.None);
						this._stoneSkimBounces = 0;
						this.stoneSkimSubState = Runner.SkimStoneSubState.Throw;
					}
				}
			}
			else if (this.stoneSkimSubState == Runner.SkimStoneSubState.Throw)
			{
				if (this.animator.normalizedTime > this.settings.stoneSkim.stoneReleaseTimeNorm || !this.animator.IsAnimation("SkimStoneThrow", null, null, null))
				{
					this._stoneGO = this.stoneSkim.stoneProto.Instantiate<Transform>(null);
					this._stoneGO.SetParent(null, false);
					this.stoneSkimSubState = Runner.SkimStoneSubState.FollowStone;
				}
			}
			else if (this.stoneSkimSubState == Runner.SkimStoneSubState.FollowStone)
			{
				this._stoneVelocity.y = this._stoneVelocity.y + this.settings.stoneSkim.gravity * Time.deltaTime;
				this._stonePosition += this._stoneVelocity * Time.deltaTime;
				if (this.stoneThrowTargetProp != null)
				{
					if (this._stonePosition.y < 0f || this._stonePosition.y < this.position.y - 5f)
					{
						this.RemoveStone();
						this.stoneSkimSubState = Runner.SkimStoneSubState.PickingUpRock;
						Narrative.instance.RunKnot("CommentOnStoneThrowAtTarget", this.stoneThrowTargetProp.inkListItemName, false, true, Array.Empty<object>());
					}
				}
				else if (this._stonePosition.y < 0f)
				{
					float value2 = Random.value;
					float num15 = this.settings.stoneSkim.skipValueOverSpeed.Evaluate(this._stoneVelocity.magnitude) * this.settings.stoneSkim.skipValueMultiplierOverLuck.Evaluate(value2);
					this._stonePosition.y = 0f;
					this._stoneVelocity.y = Mathf.Abs(this._stoneVelocity.y);
					this._stoneVelocity *= num15;
					AudioSource audioSource = this.stoneSkim.splashAudioSourceProto.Instantiate<AudioSource>(null);
					audioSource.transform.position = this.stonePosition;
					audioSource.clip = this.settings.stoneSkim.skimClips.Random<AudioClip>();
					audioSource.Play();
					this.stoneSkim.splashParticlesProto.Instantiate<ParticleSystem>(null).transform.position = this.stonePosition;
					MonoSingleton<WaterRippleManager>.instance.CreateRipple(this._stonePosition);
					this._stoneSkimBounces++;
					if (Mathf.Abs(this._stoneVelocity.x) < this.settings.stoneSkim.minSinkXSpeed)
					{
						this.stoneSkimSubState = Runner.SkimStoneSubState.Sink;
					}
				}
			}
			else if (this.stoneSkimSubState == Runner.SkimStoneSubState.Sink)
			{
				this._stoneSinkTimer += Time.deltaTime;
				this._stonePosition.y = this._stonePosition.y + this.settings.stoneSkim.sinkSpeed * Time.deltaTime;
				if (this._stoneSinkTimer > 0.5f)
				{
					this.RemoveStone();
					this.stoneSkimSubState = Runner.SkimStoneSubState.PickingUpRock;
				}
			}
			if (this._stoneGO != null)
			{
				this._stoneGO.position = this._stonePosition;
			}
			return;
		}
	}

	// Token: 0x170001BC RID: 444
	// (get) Token: 0x060006E6 RID: 1766 RVA: 0x0003DC61 File Offset: 0x0003BE61
	public bool transitionIsActive
	{
		get
		{
			return this._activeTransition != null && this._activeTransition.anim != null;
		}
	}

	// Token: 0x060006E7 RID: 1767 RVA: 0x0003DC80 File Offset: 0x0003BE80
	[NullableContext(1)]
	private void StartTransitioningIntoPosition(Vector3 targetPosition, TransitionSettings.Transition transition, string completeAnim, bool highMomentum = false, float speedScalar = 1f)
	{
		this.currentSlope = null;
		if (!highMomentum)
		{
			this.momentum = 0f;
		}
		this._activeTransition = transition;
		this._activeTransitionAnimTimeRangeNorm = default(Range);
		this._activeTransitionAnimTimeRangeNorm.max = (highMomentum ? transition.highMomentumAnimRange.max : 1f);
		FrameAnimator.PosMatch posMatch = (this.IsGroundBasedState(this.prevState) ? FrameAnimator.PosMatch.None : FrameAnimator.PosMatch.Mouth);
		this.animator.SetAnimationWithTransition(this._activeTransition.anim.name, completeAnim, 0, true, false, posMatch);
		this.animator.speed = 1f;
		if (highMomentum)
		{
			this.animator.normalizedTime = (this._activeTransitionAnimTimeRangeNorm.min = transition.highMomentumAnimRange.min);
			this.animator.speed = 1.2f;
		}
		this.animator.speed *= speedScalar;
		Vector2 remainingRootMotion = this.animator.remainingRootMotion;
		remainingRootMotion.x *= this.direction;
		Vector3 vector = base.transform.position + remainingRootMotion;
		this._tranitionIntoPositionPositionAdjust = targetPosition - vector;
		this._lastPositionAdjustTimeNorm = this._activeTransition.positionAdjustTimeRange.min;
		this._transitionFocusStartPos = (this._transitionFocusPos = this.focus);
		this._transitionFocusEndPos = targetPosition + 4f * this._activeTransition.endFocusY * Vector3.up;
	}

	// Token: 0x060006E8 RID: 1768 RVA: 0x0003DDFC File Offset: 0x0003BFFC
	private bool UpdateTransitionIntoPosition(Vector3 targetPosition, float targetRotation)
	{
		if (this._activeTransition.vocalisation != Vocalisation.None && this.StateTimeJustPassed(this._activeTransition.vocalisationTime))
		{
			AudioController.instance.PlayVocalisation(this._activeTransition.vocalisation, 0f);
		}
		bool flag = this.animator.IsAnimation(this._activeTransition.anim.name, null, null, null);
		if (flag && this.animator.normalizedTime < this._activeTransitionAnimTimeRangeNorm.max)
		{
			float num = this._activeTransition.positionAdjustTimeRange.InverseLerp(this.animator.normalizedTime);
			if (num > this._lastPositionAdjustTimeNorm)
			{
				if (num >= 1f)
				{
					base.transform.position = targetPosition;
					this.rotation = targetRotation;
					this.MarkCurrentSampleDirty();
				}
				else
				{
					float num2 = num - this._lastPositionAdjustTimeNorm;
					Vector3 vector = num2 * this._tranitionIntoPositionPositionAdjust;
					base.transform.position += vector;
					float num3 = num2 * targetRotation;
					this.rotation += num3;
				}
				this._lastPositionAdjustTimeNorm = num;
			}
			float num4 = this._activeTransitionAnimTimeRangeNorm.InverseLerp(this.animator.normalizedTime);
			this._transitionFocusPos = Vector3.Lerp(this._transitionFocusStartPos, this._transitionFocusEndPos, num4);
			return false;
		}
		if (this._activeTransition.vocalisationEnd != Vocalisation.None)
		{
			AudioController.instance.PlayVocalisation(this._climbUpAndOver.transition.vocalisationEnd, 0f);
		}
		if (flag)
		{
			this.animator.ForceCompleteTransition();
		}
		base.transform.position = targetPosition;
		this.rotation = targetRotation;
		this.MarkCurrentSampleDirty();
		this.animator.speed = 1f;
		this._activeTransition = null;
		return true;
	}

	// Token: 0x060006E9 RID: 1769 RVA: 0x0003DFB0 File Offset: 0x0003C1B0
	private void State_Tripping(bool start, bool end)
	{
		if (start)
		{
			this.animator.SetAnimation("Trip", FrameAnimator.PosMatch.None);
			if (this.isMusicRunning && !Narrative.instance.isBusy)
			{
				Narrative.instance.FailMusicRunning();
			}
			if (Runner.onTrip != null)
			{
				Runner.onTrip(0.4f);
			}
			AudioController.instance.PlayVocalisation(Vocalisation.TripAlarm, 0f);
			InputVibration.Medium();
			return;
		}
		if (end)
		{
			this.momentum = 0f;
			this.rotation = 0f;
			return;
		}
		if (this._tripDirection == 0f)
		{
			this._tripDirection = this.direction;
		}
		this.momentum *= TimeX.Damping(this.settings.tripDamping);
		Simulate.FindOptions standardPredict = Simulate.FindOptions.standardPredict;
		standardPredict.takeUphillForks = this._useUphillSlopeConnections;
		standardPredict.stopAtSwitchback = true;
		SlopeSample sample = Simulate.FindGroundPositionAtTime(this.trackPosition, this._dt, this.momentum, standardPredict, this.settings).sample;
		this.position = sample.point;
		this.currentSlope = sample.slope;
		float num = this.stateTimer / this.animator.currentAnimationVariant.duration;
		float num2 = 1f - Mathf.InverseLerp(0f, 0.2f, Mathf.Abs(num - 0.6f));
		this.rotation = num2 * this._tripDirection * sample.angle;
		if (this.StateTimeJustPassed(0.6f))
		{
			AudioController.instance.PlayVocalisation(Vocalisation.TripPain, 0f);
		}
		if (this.animator.normalizedTime >= 1f)
		{
			this.momentum = 0f;
			this._tripDirection = 0f;
			this.rotation = 0f;
			this.animator.SetAnimation("Idle", FrameAnimator.PosMatch.None);
			this.state = Runner.State.Running;
			return;
		}
	}

	// Token: 0x060006EA RID: 1770 RVA: 0x0003E17C File Offset: 0x0003C37C
	private void State_WallSlide(bool start, bool end)
	{
		if (start)
		{
			if (this.prevState == Runner.State.Climbing)
			{
				this.animator.SetAnimationWithTransition("ClimbToWallSlide", "WallSlide", 0, false, false, FrameAnimator.PosMatch.None);
				AudioController.instance.PlayVocalisation(Vocalisation.WallSlideStartUnexpectedly, 0f);
			}
			else
			{
				this.animator.SetAnimation("WallSlide", FrameAnimator.PosMatch.None);
			}
			this._wallSlide.speed = 0f;
			this.momentum = 0f;
			this._wallSlideFallFromPoly = null;
			return;
		}
		if (end)
		{
			return;
		}
		InputVibration.Small();
		this._wallSlide.speed = this._wallSlide.speed + this.settings.wallSlide.acceleration * this._dt;
		if (this._wallSlide.speed > this.settings.wallSlide.maxSpeed)
		{
			this._wallSlide.speed = this.settings.wallSlide.maxSpeed;
		}
		this.wallSlideStamina -= this._dt / this.settings.wallSlide.staminaDuration;
		if (this.wallSlideStamina <= 0f)
		{
			this.FallFromWallSlide();
			return;
		}
		bool flag = this._upDownHeld > this.settings.climb.inputUpDownThreshold;
		bool flag2 = Mathf.Abs(this._upDownHeld) < 0.5f && this._leftRightInput * this.direction < 0f;
		bool flag3 = flag && this.stamina > 0f && (this.stateTimer > this.settings.wallSlide.minSlideBeforeClimb || this._wallSlide.startedOnClimbable);
		float num = this._wallSlide.speed * this._dt;
		PolySimulate.Options wallSlidePolySimOpts = this.wallSlidePolySimOpts;
		wallSlidePolySimOpts.preventClimbable = flag3;
		PolySimulate.Result result = PolySimulate.AroundEdge(this._wallSlide.poly, this.position, Vector2.down, num, wallSlidePolySimOpts, null);
		this._wallSlide.distanceSinceLastHurt = this._wallSlide.distanceSinceLastHurt + result.distanceTravelled;
		if (this._wallSlide.distanceSinceLastHurt > this.settings.wallSlide.hurtDistance)
		{
			this._wallSlide.distanceSinceLastHurt = this._wallSlide.distanceSinceLastHurt - this.settings.wallSlide.hurtDistance;
			this.health.ApplyDamage(DamageType.WallSlide, Damage.MinorDamage);
		}
		if (this._dt > 0f)
		{
			this._wallSlide.velocity = (result.position - this.position) / this._dt;
		}
		this.position = result.position;
		this._wallSlide.normal = result.normal;
		float num2 = this.RotationToClimbOnNormal(result.normal);
		this.rotation = Mathf.MoveTowardsAngle(this.rotation, num2, this.settings.climb.climbRotationSpeed * this._dt);
		bool flag4 = result.endReason == PolySimulate.EndReason.PreventClimbable;
		bool flag5 = result.endReason == PolySimulate.EndReason.AngleTooHigh && !this.staminaIsVeryLow && this.climbingDownIterateOpts.validAngleRange.Contains(result.invalidAngle);
		if (flag4 || flag5)
		{
			this._climbing.poly = this._wallSlide.poly;
			this._climbing.mainPos = result.position3d + 0.1f * Vector3.down + 0.1f * result.normal;
			this._climbing.normal = result.normal;
			this._climbing.angle = Util.ClimbAngleFromSurfaceNormal(result.normal, this.direction);
			this._climbing.reachProgress = 0f;
			this.state = Runner.State.Climbing;
			return;
		}
		if (result.endReason == PolySimulate.EndReason.AngleTooHigh || result.endReason == PolySimulate.EndReason.PreventUnreachable)
		{
			this._wallSlideFallFromPoly = this._wallSlide.poly;
			this._wallSlideFallFromPoint = this.position;
			this.state = Runner.State.Falling;
			return;
		}
		if (this.TryClimbDownFrom(this.physicalPosition3d))
		{
			return;
		}
		if (flag2 && this.TryClimbOffToLedgeBehind())
		{
			return;
		}
		if (result.endReason == PolySimulate.EndReason.SlopeIntersection)
		{
			this.position = result.position;
			this.currentSlope = result.slopeHit;
			this.state = Runner.State.Running;
			return;
		}
		if (result.endReason != PolySimulate.EndReason.PolyIntersection)
		{
			if (this.StateTimeJustPassed(2f) && Random.value < 0.6f)
			{
				AudioController.instance.PlayVocalisation(Vocalisation.SlideOrWallSlideMidWobble, 0f);
			}
			return;
		}
		if (result.polyHitClimbable)
		{
			this._climbing.poly = result.polyHit;
			this._climbing.mainPos = result.position3d;
			this._climbing.normal = result.normal;
			this._climbing.angle = Util.ClimbAngleFromSurfaceNormal(result.normal, this.direction);
			this._climbing.reachProgress = 0f;
			this.state = Runner.State.Climbing;
			return;
		}
		float num3 = Util.ClimbAngleFromSurfaceNormal(result.normal, this.direction);
		if (wallSlidePolySimOpts.validAngleRange.Contains(num3))
		{
			if (result.normal.x * this.direction < 0f)
			{
				this.direction = -this.direction;
				this._wallSlide.velocity.x = this._wallSlide.velocity.x * -1f;
			}
			this._wallSlide.poly = result.polyHit;
			this._wallSlide.normal = result.normal;
			this.physicalDepthLayerIdx = Mathf.RoundToInt(result.polyHit.transform.position.z);
			this.position -= 0.1f * Vector2.up;
			return;
		}
		this.FallFromWallSlide();
	}

	// Token: 0x170001BD RID: 445
	// (get) Token: 0x060006EB RID: 1771 RVA: 0x0003E724 File Offset: 0x0003C924
	private Range wallSlideValidAngleRange
	{
		get
		{
			return new Range(this.settings.run.maxGroundAngle, this.settings.wallSlide.maxSlideAngle);
		}
	}

	// Token: 0x060006EC RID: 1772 RVA: 0x0003E74C File Offset: 0x0003C94C
	private void FallFromWallSlide()
	{
		AudioController.instance.PlayVocalisation(Vocalisation.ClimbSlipOrWallSlideFall, 0f);
		this.position += 1f * this._wallSlide.normal;
		this._fallIsTumble = true;
		this._tumbleIsBackwards = true;
		this.state = Runner.State.Falling;
	}

	// Token: 0x060006ED RID: 1773 RVA: 0x0003E7A8 File Offset: 0x0003C9A8
	private void WallSlideJumpFail(bool upwardIntent)
	{
		if (this.animator.IsAnimation("WallSlideStruggle", null, null, null))
		{
			return;
		}
		AudioController.instance.PlayVocalisation(Vocalisation.ClimbSlipOrWallSlideFall, 0f);
		this.animator.SetAnimationWithTransition("WallSlideStruggle", "WallSlide", 0, true, false, FrameAnimator.PosMatch.None);
		if (upwardIntent && Mathf.Abs(this._wallSlide.speed) > 0.5f * this.settings.wallSlide.maxSpeed)
		{
			this._wallSlide.speed = this._wallSlide.speed * 0.15f;
			this.wallSlideStamina = Mathf.Clamp01(this.wallSlideStamina - 0.1f);
		}
	}

	// Token: 0x170001BE RID: 446
	// (get) Token: 0x060006EE RID: 1774 RVA: 0x0003E84C File Offset: 0x0003CA4C
	private PolySimulate.Options wallSlidePolySimOpts
	{
		get
		{
			return new PolySimulate.Options
			{
				preventClimbable = false,
				preventUnreachable = true,
				validAngleRange = this.wallSlideValidAngleRange,
				slopePolyCheckDepthRange = this.raycastCurrentAndSafeForeRange,
				runnerSettings = this.settings
			};
		}
	}

	// Token: 0x060006EF RID: 1775 RVA: 0x0003E89C File Offset: 0x0003CA9C
	[NullableContext(1)]
	private bool TryStartWallSlideFromClimb(Poly poly, Vector2 normal, bool allowVeryShortSlide)
	{
		if (this.wallSlideStamina == 0f)
		{
			return false;
		}
		PolySimulate.PointResult pointResult = PolySimulate.FindNearestPointOnPoly(poly, this.position, this.wallSlideValidAngleRange);
		if (!this.CanWallSlideOnNormal(pointResult.normal))
		{
			return false;
		}
		ref PolySimulate.Result ptr = PolySimulate.AroundEdge(poly, pointResult.point, Vector2.down, 5f, this.wallSlidePolySimOpts, null);
		float num = (allowVeryShortSlide ? 0.5f : 4f);
		if (ptr.distanceTravelled < num)
		{
			return false;
		}
		this._wallSlide = new Runner.WallSlide
		{
			poly = poly,
			normal = pointResult.normal,
			startedOnClimbable = true
		};
		this.state = Runner.State.WallSliding;
		return true;
	}

	// Token: 0x060006F0 RID: 1776 RVA: 0x0003E948 File Offset: 0x0003CB48
	private bool TryStartWallSlide()
	{
		if (this.wallSlideStamina == 0f)
		{
			return false;
		}
		if (this._currentFallDamage > Damage.MinorDamage && this.health.ApplyDamage(DamageType.Fall, this._currentFallDamage))
		{
			return true;
		}
		Vector2 vector = base.transform.up;
		Vector2 vector2 = this.direction * base.transform.right;
		Vector3 vector3 = base.transform.position + (2f * vector - 1f * vector2);
		Vector2 vector4 = 4f * vector2;
		Raycast.Collision collision = Raycast.CollideWallPolygonsVec3(vector3, vector4, this.raycastNearbyRange, this.settings, false, null, false, default(Color));
		if (!collision.didHit || collision.unreachable)
		{
			Vector3 vector5 = base.transform.position + (2f * vector + 1f * vector2);
			vector4 = -4f * vector2;
			collision = Raycast.CollideWallPolygonsVec3(vector5, vector4, this.raycastNearbyRange, this.settings, false, null, false, default(Color));
			if (!collision.didHit || collision.unreachable)
			{
				return false;
			}
		}
		if (!this.CanWallSlideOnNormal(collision.normal) || collision.unreachable)
		{
			return false;
		}
		if (!PolySimulate.AroundEdge(collision.poly, collision.pos, Vector2.up, 1.5f, this.wallSlidePolySimOpts, null).success)
		{
			return false;
		}
		if (!PolySimulate.AroundEdge(collision.poly, collision.pos, Vector2.down, 1.5f, this.wallSlidePolySimOpts, null).success)
		{
			return false;
		}
		this.physicalDepthLayerIdx = Mathf.RoundToInt(collision.poly.transform.position.z);
		this.position = collision.pos;
		this._wallSlide.poly = collision.poly;
		this._wallSlide.normal = collision.normal;
		this._wallSlide.startedOnClimbable = collision.climbable;
		this.direction = -Mathf.Sign(collision.normal.x);
		this.state = Runner.State.WallSliding;
		return true;
	}

	// Token: 0x060006F1 RID: 1777 RVA: 0x0003EB81 File Offset: 0x0003CD81
	private bool CanWallSlideOnNormal(Vector2 normal)
	{
		return Vector2.Angle(normal, Vector2.up) < this.settings.wallSlide.maxSlideAngle;
	}

	// Token: 0x060006F2 RID: 1778 RVA: 0x0003EBA0 File Offset: 0x0003CDA0
	public void StartZipLine()
	{
		this._zipLine = PropsController.instance.lastInteractedProp.GetComponentInChildren<ZipLine>();
		if (this._zipLine == null)
		{
			Debug.LogError("Couldn't start zip line beacuse there was no ZipLine on the last interacted prop: " + PropsController.instance.lastInteractedProp.name);
			return;
		}
		Vector3 position = this._zipLine.transform.position;
		Vector3 normalized = (this._zipLine.destination.position - position).normalized;
		Vector3 vector = position + 10f * normalized - 5f * Vector3.up;
		this.PerformJump_Narrative(vector, null);
	}

	// Token: 0x060006F3 RID: 1779 RVA: 0x0003EC54 File Offset: 0x0003CE54
	private void State_ZipLine(bool start, bool end)
	{
		if (start)
		{
			this._currentSlope = null;
			this.MarkCurrentSampleDirty();
			this.rotation = 0f;
			this.direction = Mathf.Sign(this._zipLine.destination.position.x - this._zipLine.transform.position.x);
			this.animator.SetAnimation("ZipLine", FrameAnimator.PosMatch.None);
			this._zipLineSpeed = this.settings.zipLine.startSpeed;
		}
		if (end)
		{
			this._zipLine = null;
			return;
		}
		Vector3 position = this._zipLine.transform.position;
		Vector3 position2 = this._zipLine.destination.position;
		float num = Vector2.Distance(position, position2);
		float num2 = Mathf.InverseLerp(position.x, position2.x, this.position.x) * num;
		if (!start)
		{
			this._zipLineSpeed += this.settings.zipLine.acceleration * this._dt;
			num2 += this._zipLineSpeed * this._dt;
		}
		float num3 = num2 / num;
		base.transform.position = Vector3.Lerp(position, position2, num3);
		if (start)
		{
			return;
		}
		if (num2 > num - 1f)
		{
			SlopeSample slopeSample = Raycast.FindBestNearbySlopeSample(Level.current, this._zipLine.destination.position, false, 3f);
			this.currentSlope = slopeSample.slope;
			this.position = slopeSample.point;
			this.physicalDepthLayerIdx = Mathf.RoundToInt(slopeSample.depth);
			this.MarkCurrentSampleDirty();
			this.state = Runner.State.HardLanding;
			return;
		}
	}

	// Token: 0x060006F4 RID: 1780 RVA: 0x0003EDF2 File Offset: 0x0003CFF2
	public bool IsGroundBasedState(Runner.State s)
	{
		return s == Runner.State.Running || s == Runner.State.Sliding || s == Runner.State.InkPose || s == Runner.State.Balancing || s == Runner.State.CliffEdgeWobbling || s == Runner.State.Sitting || s == Runner.State.StoneSkimming || s == Runner.State.Dead || s == Runner.State.HardLanding || s == Runner.State.StoneSkimming || s == Runner.State.BellyWriggling;
	}

	// Token: 0x170001BF RID: 447
	// (get) Token: 0x060006F5 RID: 1781 RVA: 0x0003EE29 File Offset: 0x0003D029
	public bool inMusicRunningArea
	{
		get
		{
			return this.currentMusicRun != null;
		}
	}

	// Token: 0x170001C0 RID: 448
	// (get) Token: 0x060006F6 RID: 1782 RVA: 0x0003EE37 File Offset: 0x0003D037
	public bool isMusicRunning
	{
		get
		{
			return this.momentumAbs >= 1f && this.runTrack.playingOrAboutTo;
		}
	}

	// Token: 0x170001C1 RID: 449
	// (get) Token: 0x060006F7 RID: 1783 RVA: 0x0003EE53 File Offset: 0x0003D053
	public bool isMusicRunningScheduled
	{
		get
		{
			return this.isMusicRunning && this.runTrack.state == RunTrack.State.Scheduled;
		}
	}

	// Token: 0x170001C2 RID: 450
	// (get) Token: 0x060006F8 RID: 1784 RVA: 0x0003EE6D File Offset: 0x0003D06D
	// (set) Token: 0x060006F9 RID: 1785 RVA: 0x0003EE75 File Offset: 0x0003D075
	public float timeOutsideMusicArea { get; private set; }

	// Token: 0x170001C3 RID: 451
	// (get) Token: 0x060006FA RID: 1786 RVA: 0x0003EE7E File Offset: 0x0003D07E
	public bool isStationary
	{
		get
		{
			return this.stoppedBasically && !this.jumping && !this.climbing && !this.sliding && !this.resting && this._leftRightInput == 0f;
		}
	}

	// Token: 0x170001C4 RID: 452
	// (get) Token: 0x060006FB RID: 1787 RVA: 0x0003EEBC File Offset: 0x0003D0BC
	public bool shouldDie
	{
		get
		{
			return this.health.currentHealth == 0f && this.health.enabled && !this.health.isInvincible && !this.dead;
		}
	}

	// Token: 0x170001C5 RID: 453
	// (get) Token: 0x060006FC RID: 1788 RVA: 0x0003EEF8 File Offset: 0x0003D0F8
	public MusicRun currentMusicRun
	{
		get
		{
			Slope slope = this.currentSlope ?? this.lastRunPos.slope;
			if (slope == null)
			{
				return null;
			}
			if (slope.chunk == null)
			{
				return null;
			}
			return slope.chunk.musicRun;
		}
	}

	// Token: 0x170001C6 RID: 454
	// (get) Token: 0x060006FD RID: 1789 RVA: 0x0003EF41 File Offset: 0x0003D141
	public static Runner instance
	{
		get
		{
			return GSR.Runner;
		}
	}

	// Token: 0x170001C7 RID: 455
	// (get) Token: 0x060006FE RID: 1790 RVA: 0x0003EF48 File Offset: 0x0003D148
	public Health health
	{
		get
		{
			if (this._health == null)
			{
				this._health = base.GetComponent<Health>();
			}
			return this._health;
		}
	}

	// Token: 0x170001C8 RID: 456
	// (get) Token: 0x060006FF RID: 1791 RVA: 0x0003EF6A File Offset: 0x0003D16A
	private SpriteTrail spriteTrail
	{
		get
		{
			if (this._spriteTrail == null)
			{
				this._spriteTrail = this.animator.GetComponent<SpriteTrail>();
			}
			return this._spriteTrail;
		}
	}

	// Token: 0x170001C9 RID: 457
	// (get) Token: 0x06000700 RID: 1792 RVA: 0x0003EF91 File Offset: 0x0003D191
	public Torch torch
	{
		get
		{
			if (this._torch == null)
			{
				this._torch = base.GetComponentInChildren<Torch>();
			}
			return this._torch;
		}
	}

	// Token: 0x170001CA RID: 458
	// (get) Token: 0x06000701 RID: 1793 RVA: 0x0003EFB3 File Offset: 0x0003D1B3
	public CameraVolume[] cameraVolumes
	{
		get
		{
			if (this._cameraVolumes == null || this._cameraVolumes.Length == 0)
			{
				this._cameraVolumes = base.GetComponentsInChildren<CameraVolume>();
			}
			return this._cameraVolumes;
		}
	}

	// Token: 0x170001CB RID: 459
	// (get) Token: 0x06000702 RID: 1794 RVA: 0x0003EFD8 File Offset: 0x0003D1D8
	// (set) Token: 0x06000703 RID: 1795 RVA: 0x0003EFE0 File Offset: 0x0003D1E0
	public float momentum
	{
		get
		{
			return this._momentum;
		}
		set
		{
			if (this._momentum == value)
			{
				return;
			}
			this._momentum = value;
		}
	}

	// Token: 0x170001CC RID: 460
	// (get) Token: 0x06000704 RID: 1796 RVA: 0x0003EFF3 File Offset: 0x0003D1F3
	public float momentumAbs
	{
		get
		{
			return Mathf.Abs(this.momentum);
		}
	}

	// Token: 0x170001CD RID: 461
	// (get) Token: 0x06000705 RID: 1797 RVA: 0x0003F000 File Offset: 0x0003D200
	public bool staminaIsLow
	{
		get
		{
			return (this.stamina < 0.5f || (this.wallSliding && this.wallSlideStamina < 0.5f)) && !this.dead && !this.inFinalJump && !this.hidden;
		}
	}

	// Token: 0x170001CE RID: 462
	// (get) Token: 0x06000706 RID: 1798 RVA: 0x0003F03F File Offset: 0x0003D23F
	public bool staminaIsVeryLow
	{
		get
		{
			return (this.stamina < 0.2f || (this.wallSliding && this.wallSlideStamina < 0.2f)) && !this.dead && !this.inFinalJump && !this.hidden;
		}
	}

	// Token: 0x170001CF RID: 463
	// (get) Token: 0x06000707 RID: 1799 RVA: 0x0003F07E File Offset: 0x0003D27E
	// (set) Token: 0x06000708 RID: 1800 RVA: 0x0003F090 File Offset: 0x0003D290
	public Vector2 position
	{
		get
		{
			return base.transform.position;
		}
		set
		{
			base.transform.position = new Vector3(value.x, value.y, this.visualDepth);
			this.MarkCurrentSampleDirty();
		}
	}

	// Token: 0x170001D0 RID: 464
	// (get) Token: 0x06000709 RID: 1801 RVA: 0x0003F0BA File Offset: 0x0003D2BA
	// (set) Token: 0x0600070A RID: 1802 RVA: 0x0003F0CC File Offset: 0x0003D2CC
	public float visualDepth
	{
		get
		{
			return base.transform.position.z;
		}
		set
		{
			Vector3 position = base.transform.position;
			position.z = value;
			base.transform.position = position;
		}
	}

	// Token: 0x170001D1 RID: 465
	// (get) Token: 0x0600070B RID: 1803 RVA: 0x0003F0F9 File Offset: 0x0003D2F9
	// (set) Token: 0x0600070C RID: 1804 RVA: 0x0003F110 File Offset: 0x0003D310
	public float alpha
	{
		get
		{
			return this.animator.spriteRenderer.color.a;
		}
		set
		{
			Color color = this.animator.spriteRenderer.color;
			color.a = value;
			this.animator.spriteRenderer.color = color;
		}
	}

	// Token: 0x170001D2 RID: 466
	// (get) Token: 0x0600070D RID: 1805 RVA: 0x0003F147 File Offset: 0x0003D347
	public int levelIdx
	{
		get
		{
			return this._levelIdx;
		}
	}

	// Token: 0x170001D3 RID: 467
	// (get) Token: 0x0600070E RID: 1806 RVA: 0x0003F14F File Offset: 0x0003D34F
	// (set) Token: 0x0600070F RID: 1807 RVA: 0x0003F157 File Offset: 0x0003D357
	public int physicalDepthLayerIdx
	{
		get
		{
			return this._physicalDepth;
		}
		set
		{
			if (value != this._physicalDepth)
			{
				this._physicalDepth = value;
				this.UpdateLevelIdx();
			}
		}
	}

	// Token: 0x170001D4 RID: 468
	// (get) Token: 0x06000710 RID: 1808 RVA: 0x0003F16F File Offset: 0x0003D36F
	public Vector3 physicalPosition3d
	{
		get
		{
			return new Vector3(this.position.x, this.position.y, (float)this.physicalDepthLayerIdx);
		}
	}

	// Token: 0x170001D5 RID: 469
	// (get) Token: 0x06000711 RID: 1809 RVA: 0x0003F194 File Offset: 0x0003D394
	// (set) Token: 0x06000712 RID: 1810 RVA: 0x0003F1B9 File Offset: 0x0003D3B9
	public float rotation
	{
		get
		{
			return base.transform.rotation.eulerAngles.z;
		}
		set
		{
			base.transform.rotation = Quaternion.Euler(0f, 0f, value);
		}
	}

	// Token: 0x170001D6 RID: 470
	// (get) Token: 0x06000713 RID: 1811 RVA: 0x0003F1D6 File Offset: 0x0003D3D6
	// (set) Token: 0x06000714 RID: 1812 RVA: 0x0003F1DE File Offset: 0x0003D3DE
	public Slope currentSlope
	{
		get
		{
			return this._currentSlope;
		}
		set
		{
			if (this._currentSlope != value)
			{
				this._currentSlope = value;
				if (this._currentSlope == null && this.running)
				{
					Debug.LogError("Current slope set to null when running! This should never happen (except when exiting state maybe?");
				}
				this.MarkCurrentSampleDirty();
			}
		}
	}

	// Token: 0x170001D7 RID: 471
	// (get) Token: 0x06000715 RID: 1813 RVA: 0x0003F21C File Offset: 0x0003D41C
	public SlopeSample currentSample
	{
		get
		{
			if (this._currentSampleDirty)
			{
				if (this._currentSlope == null)
				{
					this._currentSample = default(SlopeSample);
				}
				else
				{
					this._currentSample = this._currentSlope.SampleAt(this.position.x, false);
				}
				this._currentSampleDirty = false;
			}
			return this._currentSample;
		}
	}

	// Token: 0x06000716 RID: 1814 RVA: 0x0003F271 File Offset: 0x0003D471
	private void MarkCurrentSampleDirty()
	{
		this._currentSampleDirty = true;
	}

	// Token: 0x170001D8 RID: 472
	// (get) Token: 0x06000717 RID: 1815 RVA: 0x0003F27A File Offset: 0x0003D47A
	public bool isOnSteepSlopeUpward
	{
		get
		{
			return this.relativeSlopeAngle > this.settings.run.maxRunGroundAngle;
		}
	}

	// Token: 0x170001D9 RID: 473
	// (get) Token: 0x06000718 RID: 1816 RVA: 0x0003F294 File Offset: 0x0003D494
	public bool isOnSteepSlope
	{
		get
		{
			return Mathf.Abs(this.relativeSlopeAngle) > this.settings.run.maxRunGroundAngle;
		}
	}

	// Token: 0x170001DA RID: 474
	// (get) Token: 0x06000719 RID: 1817 RVA: 0x0003F2B3 File Offset: 0x0003D4B3
	public float movementDirection
	{
		get
		{
			if (Mathf.Abs(this.momentum) >= 0.01f)
			{
				return Mathf.Sign(this.momentum);
			}
			return 0f;
		}
	}

	// Token: 0x170001DB RID: 475
	// (get) Token: 0x0600071A RID: 1818 RVA: 0x0003F2D8 File Offset: 0x0003D4D8
	public bool runningEastwards
	{
		get
		{
			bool flag = this.direction > 0f;
			if (this.lastRunPos.slope != null && this.lastRunPos.slope.reverseFlow)
			{
				flag = !flag;
			}
			return flag;
		}
	}

	// Token: 0x170001DC RID: 476
	// (get) Token: 0x0600071B RID: 1819 RVA: 0x0003F320 File Offset: 0x0003D520
	public TrackPosition trackPosition
	{
		get
		{
			return new TrackPosition
			{
				slope = this.currentSlope,
				x = this.position.x
			};
		}
	}

	// Token: 0x170001DD RID: 477
	// (get) Token: 0x0600071C RID: 1820 RVA: 0x0003F358 File Offset: 0x0003D558
	public float relativeSlopeAngle
	{
		get
		{
			if (!this.currentSample.isValid)
			{
				return 0f;
			}
			return this.direction * this.currentSample.angle;
		}
	}

	// Token: 0x170001DE RID: 478
	// (get) Token: 0x0600071D RID: 1821 RVA: 0x0003F390 File Offset: 0x0003D590
	public float currentSlopeDownDir
	{
		get
		{
			if (!this.currentSample.isValid)
			{
				return 1f;
			}
			return -Mathf.Sign(this.currentSample.angle);
		}
	}

	// Token: 0x170001DF RID: 479
	// (get) Token: 0x0600071E RID: 1822 RVA: 0x0003F3C4 File Offset: 0x0003D5C4
	private float steepness
	{
		get
		{
			return Mathf.Clamp(this.relativeSlopeAngle / this.settings.run.maxGroundAngle, -1f, 1f);
		}
	}

	// Token: 0x170001E0 RID: 480
	// (get) Token: 0x0600071F RID: 1823 RVA: 0x0003F3EC File Offset: 0x0003D5EC
	private bool onSlide
	{
		get
		{
			return this.SlidesOnSlope(this.currentSlope, this.relativeSlopeAngle);
		}
	}

	// Token: 0x06000720 RID: 1824 RVA: 0x0003F400 File Offset: 0x0003D600
	private bool SlidesOnSlope(Slope slope, float angle)
	{
		return slope != null && slope.isSlide && Mathf.Abs(angle) > this.settings.run.slideMinAngle;
	}

	// Token: 0x170001E1 RID: 481
	// (get) Token: 0x06000721 RID: 1825 RVA: 0x0003F430 File Offset: 0x0003D630
	private Vector2 velocityOnGround
	{
		get
		{
			if (!this.currentSample.isValid)
			{
				return Vector2.zero;
			}
			SlopeSample currentSample = this.currentSample;
			Vector2 vector = new Vector2(currentSample.normal.y, -currentSample.normal.x);
			return Simulate.SignedSpeedOnGround(this.momentum, currentSample.angle, this.onSlide, this.settings.run) * this.reverseScalar * vector;
		}
	}

	// Token: 0x170001E2 RID: 482
	// (get) Token: 0x06000722 RID: 1826 RVA: 0x0003F4A8 File Offset: 0x0003D6A8
	private float reverseScalar
	{
		get
		{
			if (this.momentumAbs < 1f)
			{
				return 1f;
			}
			if (this._lastReverseInterruptTime < 0f)
			{
				return 1f;
			}
			if (this._leftRightInput * this.momentum > 0f)
			{
				return 1f;
			}
			float num = Time.time - this._lastReverseInterruptTime;
			float num2 = Mathf.InverseLerp(0f, this.settings.run.reverseGraceTime, num);
			return Mathf.Lerp(1f, this.settings.run.reverseMinSpeedScalar, num2);
		}
	}

	// Token: 0x170001E3 RID: 483
	// (get) Token: 0x06000723 RID: 1827 RVA: 0x0003F53C File Offset: 0x0003D73C
	public Vector2 velocity
	{
		get
		{
			if (!this._inStateUpdate)
			{
				return this._cachedVelocity;
			}
			if (this.jumping)
			{
				return this._jump.velocity;
			}
			if (this.falling)
			{
				return this._fallVelocity;
			}
			if (this.balancing)
			{
				return Vector2.zero;
			}
			if (this.caught)
			{
				return this._caughtVelocity;
			}
			if (this.isBoating)
			{
				return Game.instance.activeBoat.velocity;
			}
			if (this.debugFlying)
			{
				return this._debugFlyVelocity;
			}
			if (this.wallSliding)
			{
				return this._wallSlide.velocity;
			}
			if (this.inFinalJumpAndLeftLand)
			{
				return this._swanDiveVelocity;
			}
			if (this.dead)
			{
				return Vector2.zero;
			}
			return this.velocityOnGround;
		}
	}

	// Token: 0x170001E4 RID: 484
	// (get) Token: 0x06000724 RID: 1828 RVA: 0x0003F600 File Offset: 0x0003D800
	public float speed
	{
		get
		{
			if (!this._inStateUpdate)
			{
				return this._cachedSpeed;
			}
			return this.velocity.magnitude;
		}
	}

	// Token: 0x170001E5 RID: 485
	// (get) Token: 0x06000725 RID: 1829 RVA: 0x0003F62A File Offset: 0x0003D82A
	public Vector2 prevPosition
	{
		get
		{
			return this._prevPosition;
		}
	}

	// Token: 0x06000726 RID: 1830 RVA: 0x0003F634 File Offset: 0x0003D834
	public Vector2 GetGroundVelocity(Vector2 velocity)
	{
		if (this.jumping)
		{
			Vector2 normalized = (this._jump.targetPos - this._jump.startPos).normalized;
			return Vector2.Dot(normalized, velocity) * normalized;
		}
		return velocity;
	}

	// Token: 0x170001E6 RID: 486
	// (get) Token: 0x06000727 RID: 1831 RVA: 0x0003F67E File Offset: 0x0003D87E
	// (set) Token: 0x06000728 RID: 1832 RVA: 0x0003F686 File Offset: 0x0003D886
	public Vector3 focus { get; private set; }

	// Token: 0x170001E7 RID: 487
	// (get) Token: 0x06000729 RID: 1833 RVA: 0x0003F68F File Offset: 0x0003D88F
	public bool isAtAutoRunTarget
	{
		get
		{
			return this.IsAtSpecificAutoRunTarget(this.autoRunTargetX);
		}
	}

	// Token: 0x170001E8 RID: 488
	// (get) Token: 0x0600072A RID: 1834 RVA: 0x0003F69D File Offset: 0x0003D89D
	public bool stoppedBasically
	{
		get
		{
			return this.momentumAbs < this.settings.run.almostStoppedMomentum;
		}
	}

	// Token: 0x170001E9 RID: 489
	// (get) Token: 0x0600072B RID: 1835 RVA: 0x0003F6B7 File Offset: 0x0003D8B7
	public bool stoppedAndPaused
	{
		get
		{
			return this._stoppedAndPausedTimer > 0.7f;
		}
	}

	// Token: 0x170001EA RID: 490
	// (get) Token: 0x0600072C RID: 1836 RVA: 0x0003F6C6 File Offset: 0x0003D8C6
	public bool isAtAutoRunTargetAndStopped
	{
		get
		{
			return this.isAtAutoRunTarget && this.stoppedBasically;
		}
	}

	// Token: 0x170001EB RID: 491
	// (get) Token: 0x0600072D RID: 1837 RVA: 0x0003F6D8 File Offset: 0x0003D8D8
	public bool hasAutoRunTarget
	{
		get
		{
			return this._hasAutoRunTarget;
		}
	}

	// Token: 0x170001EC RID: 492
	// (get) Token: 0x0600072E RID: 1838 RVA: 0x0003F6E0 File Offset: 0x0003D8E0
	public float autoRunTargetX
	{
		get
		{
			return this._autoRunTarget.x;
		}
	}

	// Token: 0x0600072F RID: 1839 RVA: 0x0003F6ED File Offset: 0x0003D8ED
	public void SetAutoRunTarget(Vector3 autoRunTarget, float remainingAutoRunTargetTime, bool precisely = false)
	{
		this._hasAutoRunTarget = true;
		this._autoRunTarget = autoRunTarget;
		this._autoRunTargetNeedsPrecision = precisely;
		this.remainingAutoRunTargetTime = remainingAutoRunTargetTime;
		this.playerControlDisabled |= PlayerControlDisableReason.AutoRunToProp;
	}

	// Token: 0x06000730 RID: 1840 RVA: 0x0003F71A File Offset: 0x0003D91A
	public void ResetAutoRunTarget()
	{
		this._hasAutoRunTarget = false;
		this._autoRunTarget = Vector3.zero;
		this._autoRunTargetNeedsPrecision = false;
		this.remainingAutoRunTargetTime = -1f;
	}

	// Token: 0x06000731 RID: 1841 RVA: 0x0003F740 File Offset: 0x0003D940
	public bool IsAtSpecificAutoRunTarget(float specificAutoRunX)
	{
		return Mathf.Abs(specificAutoRunX - this.position.x) < 0.6f;
	}

	// Token: 0x170001ED RID: 493
	// (get) Token: 0x06000732 RID: 1842 RVA: 0x0003F75B File Offset: 0x0003D95B
	// (set) Token: 0x06000733 RID: 1843 RVA: 0x0003F763 File Offset: 0x0003D963
	public TrackPosition lastRunPos { get; private set; }

	// Token: 0x170001EE RID: 494
	// (get) Token: 0x06000734 RID: 1844 RVA: 0x0003F76C File Offset: 0x0003D96C
	// (set) Token: 0x06000735 RID: 1845 RVA: 0x0003F774 File Offset: 0x0003D974
	public float lastRunSlopeTime { get; private set; }

	// Token: 0x170001EF RID: 495
	// (get) Token: 0x06000736 RID: 1846 RVA: 0x0003F77D File Offset: 0x0003D97D
	public RunTrack runTrack
	{
		get
		{
			return MonoSingleton<RunTrack>.instance;
		}
	}

	// Token: 0x170001F0 RID: 496
	// (get) Token: 0x06000737 RID: 1847 RVA: 0x0003F784 File Offset: 0x0003D984
	public float baseDepthForRaycast
	{
		get
		{
			return (float)this.physicalDepthLayerIdx;
		}
	}

	// Token: 0x170001F1 RID: 497
	// (get) Token: 0x06000738 RID: 1848 RVA: 0x0003F78D File Offset: 0x0003D98D
	private Range raycastCurrentRangeONLY
	{
		get
		{
			return this.settings.layer.layerCollideCurrentRange + this.baseDepthForRaycast;
		}
	}

	// Token: 0x170001F2 RID: 498
	// (get) Token: 0x06000739 RID: 1849 RVA: 0x0003F7AA File Offset: 0x0003D9AA
	public Range raycastNearbyRange
	{
		get
		{
			return this.settings.layer.layerCollideNearbyRange + this.baseDepthForRaycast;
		}
	}

	// Token: 0x170001F3 RID: 499
	// (get) Token: 0x0600073A RID: 1850 RVA: 0x0003F7C7 File Offset: 0x0003D9C7
	public Range raycastSafeNearbyRange
	{
		get
		{
			return new Range(this.settings.layer.layerCollideSafetyMin, this.settings.layer.layerCollideNearbyRange.max) + this.baseDepthForRaycast;
		}
	}

	// Token: 0x170001F4 RID: 500
	// (get) Token: 0x0600073B RID: 1851 RVA: 0x0003F7FE File Offset: 0x0003D9FE
	public Range raycastCurrentAndSafeForeRange
	{
		get
		{
			return new Range(this.settings.layer.layerCollideSafetyMin, this.settings.layer.layerCollideCurrentRange.max) + this.baseDepthForRaycast;
		}
	}

	// Token: 0x170001F5 RID: 501
	// (get) Token: 0x0600073C RID: 1852 RVA: 0x0003F835 File Offset: 0x0003DA35
	private Range raycastCurrentAndNearForeRange
	{
		get
		{
			return new Range(this.settings.layer.layerCollideNearbyRange.min, this.settings.layer.layerCollideCurrentRange.max) + this.baseDepthForRaycast;
		}
	}

	// Token: 0x170001F6 RID: 502
	// (get) Token: 0x0600073D RID: 1853 RVA: 0x0003F871 File Offset: 0x0003DA71
	private Range raycastSafeForegroundONLY
	{
		get
		{
			return new Range(this.settings.layer.layerCollideSafetyMin, this.settings.layer.layerCollideNearbyRange.min) + this.baseDepthForRaycast;
		}
	}

	// Token: 0x170001F7 RID: 503
	// (get) Token: 0x0600073E RID: 1854 RVA: 0x0003F8A8 File Offset: 0x0003DAA8
	public Bounds bounds
	{
		get
		{
			return new Bounds(base.transform.position + Vector3.up * this.settings.worldSize.y * 0.5f, new Vector3(this.settings.worldSize.x, this.settings.worldSize.y, 0.1f));
		}
	}

	// Token: 0x0600073F RID: 1855 RVA: 0x0003F918 File Offset: 0x0003DB18
	public void ResetForGameStart()
	{
		this.stamina = 1f;
		this.maxStamina = 1.01f;
		this.wallSlideStamina = 1f;
		this.momentum = 0f;
		this._lookDownTime = 0f;
		this._sprintPressed = false;
		this.health.ResetToMax();
		this.torch.ResetForGameStart();
	}

	// Token: 0x06000740 RID: 1856 RVA: 0x0003F97C File Offset: 0x0003DB7C
	public void TurnToFace(Prop prop)
	{
		if (prop.preferredLookDirection != 0)
		{
			this.direction = (float)prop.preferredLookDirection;
			return;
		}
		float num = ((prop.customWidgetAttach != null) ? prop.customWidgetAttach.transform.position.x : prop.transform.position.x);
		this.direction = Mathf.Sign(num - this.position.x);
	}

	// Token: 0x06000741 RID: 1857 RVA: 0x0003F9F0 File Offset: 0x0003DBF0
	public void TurnToFace(StoryCharacter character)
	{
		float num = ((character.animator == null) ? character.fallbackMouthTransform.position.x : character.animator.mouthPosition.x);
		this.direction = Mathf.Sign(num - this.position.x);
	}

	// Token: 0x06000742 RID: 1858 RVA: 0x0003FA46 File Offset: 0x0003DC46
	public void RemoveStrongPoses()
	{
	}

	// Token: 0x06000743 RID: 1859 RVA: 0x0003FA48 File Offset: 0x0003DC48
	private void Awake()
	{
		this.musicalTrail.enabled = false;
	}

	// Token: 0x06000744 RID: 1860 RVA: 0x0003FA58 File Offset: 0x0003DC58
	private void Start()
	{
		this.lastRunPos = TrackPosition.none;
		this.lastRunSlopeTime = float.NegativeInfinity;
		FrameAnimator frameAnimator = this.animator;
		frameAnimator.onReverseDirection = (Action)Delegate.Combine(frameAnimator.onReverseDirection, new Action(this.OnAnimationReversedDirection));
		FrameAnimator frameAnimator2 = this.animator;
		frameAnimator2.onRootMotion = (FrameAnimator.OnRootMotionDelegate)Delegate.Combine(frameAnimator2.onRootMotion, new FrameAnimator.OnRootMotionDelegate(this.OnAnimationRootMotion));
	}

	// Token: 0x06000745 RID: 1861 RVA: 0x0003FAC9 File Offset: 0x0003DCC9
	private void OnEnable()
	{
		GSR.SetRunner(this);
		this.SetSafeResetPoint();
		if (Runner.onBecameActive != null)
		{
			Runner.onBecameActive(true);
		}
	}

	// Token: 0x06000746 RID: 1862 RVA: 0x0003FAE9 File Offset: 0x0003DCE9
	private void OnDisable()
	{
		this.ClearSafeResetPoint();
		this.ResetAutoRunTarget();
		if (Runner.onBecameActive != null)
		{
			Runner.onBecameActive(false);
		}
	}

	// Token: 0x06000747 RID: 1863 RVA: 0x0003FB0C File Offset: 0x0003DD0C
	private void Update()
	{
		if (!Game.loaded)
		{
			return;
		}
		if (Game.gameplayPaused)
		{
			return;
		}
		if (Blackout.isShownOrShowing)
		{
			Runner.instance.playerControlDisabled |= PlayerControlDisableReason.Blackout;
		}
		else
		{
			Runner.instance.playerControlDisabled &= ~PlayerControlDisableReason.Blackout;
		}
		this._prevVelocity = this.velocity;
		this._prevSpeed = this._prevVelocity.magnitude;
		this._prevPosition = this.position;
		this.prevMusicRun = this.currentMusicRun;
		bool flag = this.position.y < 0f;
		this._dt = Time.deltaTime;
		if (this.shouldDie && !this.falling && !this.stumbling && !this.tripping)
		{
			if (this.climbing || this.wallSliding || this.balancing || this.climbSlipping || this.climbingUpAndOver)
			{
				this.state = Runner.State.Falling;
			}
			else if (this.IsGroundBasedState(this.state))
			{
				this.Die(false);
			}
		}
		this._inStateUpdate = true;
		if (this._dt > 0f)
		{
			this.UpdateMovement();
		}
		else
		{
			History.Log("Skipped update due to _dt == 0");
		}
		this._cachedVelocity = this.velocity;
		this._cachedSpeed = this.speed;
		this._inStateUpdate = false;
		this.UpdateStamina();
		this._leftRightInput = 0f;
		this._upDownPressed = 0f;
		this._upDownHeld = 0f;
		this._move2dInput = Vector2.zero;
		this._sprintHeld = false;
		this._sprintPressed = false;
		if (!this.balancing && !this.jumping && !this.falling)
		{
			this._prevBalancePoint = null;
		}
		if (this.playerControlDisabled == PlayerControlDisableReason.None && !this.shouldDie)
		{
			this.UpdatePlayerInput();
		}
		else
		{
			this._climbPromptAvailable = false;
			this._climbPromptIsUp = false;
			this._climbPromptFill = 0f;
			MonoSingleton<GameUI>.instance.climbPrompt.Hide(0f);
			if (this.hasAutoRunTarget)
			{
				if (this.isAtAutoRunTarget)
				{
					this._leftRightInput = 0f;
					if (this.isAtAutoRunTargetAndStopped)
					{
						if (this._autoRunTargetNeedsPrecision)
						{
							Game.instance.TeleportPlayerWithoutLevelChange(this._autoRunTarget, "Auto run complete", 0, false, false);
						}
						this.ResetAutoRunTarget();
					}
				}
				else
				{
					this.remainingAutoRunTargetTime -= Time.deltaTime;
					if (this.remainingAutoRunTargetTime < 0f)
					{
						Game.instance.TeleportPlayerWithoutLevelChange(this._autoRunTarget, "Auto run complete", 0, false, false);
						this.ResetAutoRunTarget();
					}
					else
					{
						this._leftRightInput = 0.5f * Mathf.Sign(this.autoRunTargetX - this.position.x);
					}
				}
			}
		}
		if (this.inMusicRunningArea)
		{
			this.timeOutsideMusicArea = 0f;
		}
		else
		{
			this.timeOutsideMusicArea += this._dt;
		}
		this.TryPrepareForFinalJump();
		float num = 0f;
		float num2 = 1f;
		if (MonoSingleton<RunTrack>.instance.scheduled && Time.unscaledTime - MonoSingleton<RunTrack>.instance.initialSprintStartTime < 2f)
		{
			num = Mathf.InverseLerp(MonoSingleton<RunTrack>.instance.initialSprintStartTime, MonoSingleton<RunTrack>.instance.initialSprintStartTime + 2f, Time.unscaledTime);
			num2 = 0.5f;
		}
		else if (this._sprintTimer > 0f)
		{
			float num3 = this._sprintTimer / this.settings.run.sprintBoostTimer;
			num = num3 * 0.8f;
			num2 = 1f - num3;
		}
		this.spriteTrail.visibility = Mathf.MoveTowards(this.spriteTrail.visibility, num, Time.deltaTime / num2);
		bool flag2 = this.animator.currentAnimation != null && this.animator.currentAnimation.reversesDirection;
		if (this.movementDirection != 0f && this.direction * this.movementDirection < 0f && !flag2 && (this.running || this.sliding || this.jumping || this.caught || this.debugFlying || this.isBoating))
		{
			this.direction = this.movementDirection;
		}
		if (!this.isMusicRunning)
		{
			this._stumbleCount = 0;
			this._stumbleCountInRow = 0;
		}
		if (this.direction == 0f)
		{
			Debug.LogWarning("Runner.lookDirection was set to 0! Should always be 1 or -1");
			this.direction = 1f;
		}
		this.UpdateLevelXRange();
		this.UpdateDepth(false);
		this.UpdateAutomaticReset();
		bool flag3 = this.position.y < 0f;
		if (flag3)
		{
			this.surfaceTypeSampler.surfaceType = SurfaceType.Water;
		}
		else if (this.balancing)
		{
			this.surfaceTypeSampler.surfaceType = SurfaceType.Rock;
		}
		else if (Time.time >= this.nextSurfaceSampleTime || (!flag3 && this.surfaceTypeSampler.surfaceType == SurfaceType.Water))
		{
			if (this.currentSlope == null)
			{
				this.surfaceTypeSampler.surfaceType = SurfaceType.Rock;
			}
			else
			{
				this.surfaceTypeSampler.UpdateSurfaceType(this.currentSlope, base.transform.position);
			}
			this.nextSurfaceSampleTime = Time.time + 0.2f;
		}
		bool flag4 = false;
		if (this.running && !this.stoppedBasically)
		{
			int num4 = Mathf.CeilToInt(2f * this.animator.normalizedTime);
			if (num4 != this._lastFootstep)
			{
				this._lastFootstep = num4;
				flag4 = true;
				if (Runner.onFootstep != null)
				{
					Runner.onFootstep(num4);
				}
			}
		}
		else
		{
			this._lastFootstep = -1;
		}
		if ((flag4 && flag3) || (flag3 && !flag))
		{
			Vector3 position = base.transform.position;
			position.y = 0f;
			MonoSingleton<WaterRippleManager>.instance.CreateRipple(position);
		}
		this.musicalTrail.enabled = this.isMusicRunning && !this.isMusicRunningScheduled;
		this._mouth.position = this.animator.mouthPosition;
		this._perspirationParticles.transform.position = this._mouth.position + new Vector3(-this.direction * 0.5f, 0.5f, 0f);
		this.isOnRidge = !Raycast.AnyPolyOccludes(this._mouth.position, new Range(this.visualDepth - 50f, this.visualDepth + 50f), null, null);
		this.UpdateFocus();
	}

	// Token: 0x06000748 RID: 1864 RVA: 0x00040154 File Offset: 0x0003E354
	private void LateUpdate()
	{
		float num = this.direction;
		if (this.animator.inSecondHalfOfReversal)
		{
			num *= -1f;
		}
		base.transform.localScale = new Vector3(num, 1f, 1f);
		if (this.animator.isActiveAndEnabled && this.animator.activeFrame != null)
		{
			Sprite activeFrame = this.animator.activeFrame;
			this.animator.transform.TransformPoint(activeFrame.bounds.center);
		}
		else
		{
			new Vector2(2f, 5f);
		}
		if (this.inFinalJump)
		{
			this.FinalJumpLateUpdate();
		}
	}

	// Token: 0x06000749 RID: 1865 RVA: 0x00040204 File Offset: 0x0003E404
	public void UpdateFocus()
	{
		float num = this.visualDepth;
		float num2 = 4f;
		Vector2 vector;
		if (this.jumping)
		{
			float num3 = Mathf.InverseLerp(this._jump.startPos.x, this._jump.targetPos.x, this.position.x);
			if (num3 >= 1f)
			{
				vector = this.position;
			}
			else
			{
				vector = Vector2.Lerp(this._jump.startPos, this._jump.targetPos, num3);
			}
		}
		else if (this.falling || this.debugFlying)
		{
			vector = this.position;
		}
		else if (this.exploded)
		{
			vector = this._explosionSource;
		}
		else if (this.onChairLift)
		{
			vector = this.position;
		}
		else if (this.transitionIsActive)
		{
			vector = this._transitionFocusPos;
		}
		else if (this.currentSample.isValid)
		{
			vector = this.currentSample.point;
		}
		else
		{
			vector = this.position;
		}
		if (this.climbing)
		{
			num2 = 0f;
		}
		else if (this.transitionIsActive)
		{
			num2 = 0f;
		}
		else if (this.climbSlipping)
		{
			num2 = -4f;
		}
		else if (this.wallSliding)
		{
			num2 = -1f;
		}
		else if (this.bellyWriggling)
		{
			num2 = 1f;
		}
		if (this._lookDownTime > 0f && !NarrativePresenter.instance.hasAnyChoices)
		{
			float num4 = Mathf.InverseLerp(0f, 0.5f, this._lookDownTime);
			num2 = -1f * num4;
			num -= 5f * num4;
		}
		this.focus = new Vector3(vector.x, vector.y + num2, num);
	}

	// Token: 0x0600074A RID: 1866 RVA: 0x000403B4 File Offset: 0x0003E5B4
	public void UpdateLevelXRange()
	{
		this.levelXRange.min = Math.Min(this.levelXRange.min, this.position.x);
		this.levelXRange.max = Math.Max(this.levelXRange.max, this.position.x);
	}

	// Token: 0x0600074B RID: 1867 RVA: 0x0004040D File Offset: 0x0003E60D
	public void UpdateLevelIdx()
	{
		this._levelIdx = Level.DepthToIndex(base.transform.position.z);
	}

	// Token: 0x0600074C RID: 1868 RVA: 0x0004042C File Offset: 0x0003E62C
	public void TryIncreaseMaxStamina()
	{
		if (this.maxStamina < 2f)
		{
			float num = ((this.maxStamina < 1.3f) ? 0.1f : ((this.maxStamina < 1.8f) ? 0.05f : 0.02f));
			this.maxStamina = Mathf.Clamp(this.maxStamina + num, 1f, 2f);
			MonoSingleton<Notifications>.instance.NotifyStaminaIncrease(Mathf.FloorToInt(num * 100f));
		}
	}

	// Token: 0x0600074D RID: 1869 RVA: 0x000404A8 File Offset: 0x0003E6A8
	private void UpdateStamina()
	{
		if (DebugOptions.opts.infiniteStamina)
		{
			this.stamina = this.maxStamina;
		}
		if (Game.instance.inPeakState || Game.instance.isPathFollowingBetweenLevels || Game.instance.inEndGameSequence || Game.instance.inTitleScreenAndIntroState)
		{
			this.stamina = this.maxStamina;
			this.wallSlideStamina = 1f;
		}
		if (this.running)
		{
			this.wallSlideStamina = 1f;
		}
		if ((this.running || this.hasInkPose) && this.stoppedBasically)
		{
			this._stoppedAndPausedTimer += this._dt;
		}
		else
		{
			this._stoppedAndPausedTimer = 0f;
		}
		this._restoringStamina = false;
		if (this.resting || this.sitting || this.hidden || this.bellyWriggling || MonoSingleton<RunTrack>.instance.playingOrAboutTo || this.stoppedAndPaused)
		{
			if (this.stamina < this.maxStamina)
			{
				this._restoringStamina = true;
			}
			this.stamina += this.settings.stamina.staminaRestoreSpeed * this._dt;
			if (this.stamina > this.maxStamina)
			{
				this.stamina = this.maxStamina;
			}
		}
		ParticlesX.SetActive(this._perspirationParticles, this.staminaIsLow);
	}

	// Token: 0x0600074E RID: 1870 RVA: 0x00040601 File Offset: 0x0003E801
	public void ResetToSlopeSample(SlopeSample slopeSample, bool dontChangeState)
	{
		this.currentSlope = slopeSample.slope;
		this.position = slopeSample.point;
		this.momentum = 0f;
		if (!dontChangeState)
		{
			this.state = Runner.State.Running;
		}
	}

	// Token: 0x0600074F RID: 1871 RVA: 0x00040630 File Offset: 0x0003E830
	private Vector2 CreateNormalisedJumpIntent()
	{
		Vector2 vector = GameInput.move2d;
		if (vector.magnitude < 0.2f)
		{
			vector = default(Vector2);
		}
		else
		{
			vector.Normalize();
		}
		return vector;
	}

	// Token: 0x06000750 RID: 1872 RVA: 0x00040664 File Offset: 0x0003E864
	private void UpdatePlayerInput()
	{
		this._leftRightInput += GameInput.moveLeftRight;
		this._move2dInput = GameInput.move2d;
		if (this.running && GameInput.sprintHeld && !NarrativePresenter.instance.hasAnyChoices && (Mathf.Abs(this._leftRightInput) > 0.5f || !this.stoppedBasically) && !this.inMusicRunningArea)
		{
			this._sprintHeld = GameInput.sprintHeld;
			this._sprintPressed = GameInput.sprintPressed;
		}
		bool flag = false;
		if (!flag)
		{
			this._upDownPressed += GameInput.upDownPressed;
			this._upDownHeld += GameInput.moveUpDown;
		}
		this._climbPromptAvailable = false;
		this._climbPromptIsUp = false;
		Vector3 vector = Vector3.zero;
		float num = 0f;
		Runner.ClimbSetup climbSetup = Runner.ClimbSetup.none;
		bool flag2 = this.running || this.sliding || this.jumping || this.balancing || (this.falling && !this._fallIsTumble);
		if (this.isMusicRunning)
		{
			flag2 = false;
		}
		if (this.falling && (this._currentFallDamage != Damage.None || this._fallPreventsClimb))
		{
			flag2 = false;
		}
		if (flag2)
		{
			if (this.running && this._leftRightInput * this.direction > 0f && this.currentSlope != null)
			{
				Runner.ClimbSetup climbSetup2 = this.TrySetupClimbing(ClimbCheckRange.GroundAutoClimb, false, false, true);
				if (climbSetup2.isValid && climbSetup2.upAndOver && this._climbUpAndOver.targetPos.y - this.position.y < this.settings.upAndOver.autoClimbMaxHeight && this._climbUpAndOver.slope != null)
				{
					Slope slope = this._climbUpAndOver.slope;
					Vector2 vector2 = ((this.direction > 0f) ? slope.rightPoint : slope.leftPoint);
					SlopeSample slopeSample;
					if (!Simulate.CanAutoTraverseStep(this.currentSlope.SampleAt(vector2.x, false).point, slope, out slopeSample, -0.1f))
					{
						this.state = Runner.State.ClimbUpAndOver;
						goto IL_0463;
					}
				}
			}
			climbSetup = this.TrySetupClimbing(this.IsGroundBasedState(this.state) ? ClimbCheckRange.Ground : ClimbCheckRange.JumpOrFall, false, false, false);
			bool flag3 = false;
			if (climbSetup.isValid && climbSetup.ontoWall && climbSetup.physicalDepthLayerIdx == this.physicalDepthLayerIdx - 1)
			{
				PolySimulate.Options climbingDownIterateOpts = this.climbingDownIterateOpts;
				climbingDownIterateOpts.slopePolyCheckDepthRange -= 1f;
				flag3 = PolySimulate.AroundEdge(this._climbing.poly, this._climbing.mainPos, Vector2.down, 6f, climbingDownIterateOpts, null).success;
			}
			if (climbSetup.isValid && (this._upDownHeld > this.settings.climb.inputUpDownThreshold || (flag3 && this._upDownHeld < -this.settings.climb.inputUpDownThreshold)))
			{
				this._climbPromptFill = 0f;
				MonoSingleton<GameUI>.instance.climbPrompt.Confirm();
				if (climbSetup.upAndOver)
				{
					this.state = Runner.State.ClimbUpAndOver;
				}
				else if (climbSetup.ontoWall)
				{
					if (this._upDownHeld < 0f)
					{
						PolySimulate.Result result = PolySimulate.AroundEdge(this._climbing.poly, this._climbing.mainPos, Vector2.down, 4f, this.climbingDownIterateOpts, null);
						this._climbing.poly = result.poly;
						this._climbing.mainPos = result.position3d;
						this._climbing.normal = result.normal;
						this._climbing.angle = Util.ClimbAngleFromSurfaceNormal(result.normal, this.direction);
						this.state = Runner.State.Climbing;
						return;
					}
					this.state = Runner.State.ClimbOntoWall;
					ClimbPrompt climbPrompt = MonoSingleton<GameUI>.instance.climbPrompt;
					int upwardClimbsCompleted = climbPrompt.upwardClimbsCompleted;
					climbPrompt.upwardClimbsCompleted = upwardClimbsCompleted + 1;
					return;
				}
			}
			else if (climbSetup.isValid && climbSetup.ontoWall && this._climbOntoWall.climbable && this.running && this.stoppedBasically && (!DebugOptions.opts.only3JumpUpPrompts || MonoSingleton<GameUI>.instance.climbPrompt.upwardClimbsCompleted < 3))
			{
				this._climbPromptAvailable = true;
				vector = this._climbing.mainPos;
				this._climbPromptIsUp = true;
			}
		}
		IL_0463:
		bool flag4 = this.falling && this.stateTimer < this.settings.jump.fallToJumpGracePeriod && (this.prevState == Runner.State.Running || this.prevState == Runner.State.Sliding);
		bool flag5 = this.running || this.sliding || this.balancing || this.climbing || this.wallSliding || flag4 || this.cliffEdgeWobbling || this.onStoppedChairLift;
		bool flag6 = this.jumping || (this.prevState == Runner.State.Jumping && this.climbingUpAndOver && this._climbUpAndOver.transition == this.settings.transition.bridgeGap);
		if (flag5 || flag6)
		{
			bool flag7 = this.isMusicRunning && TrackBuilder.specialJumpsAvailable && GameInput.jumpedSpecial;
			bool flag8 = GameInput.jumped || flag7;
			if (MonoSingleton<Tutorial>.instance.activeTutorial == TutorialId.MusicRunningSpecialJump && !flag7)
			{
				flag8 = false;
			}
			if (Input.GetKeyDown(KeyCode.Space) && !GameInput.jumped)
			{
				flag8 = true;
			}
			if (flag8)
			{
				MonoSingleton<GameInput>.instance.mapping.jump.ClearInputState();
				MonoSingleton<GameInput>.instance.mapping.jumpSpecial.ClearInputState();
				this._jumpDirIntent = this.CreateNormalisedJumpIntent();
				if (FinalJumpZone.activeZone != null && flag5)
				{
					this.StartFinalJumpImmediate();
					return;
				}
				if (this.jumping && this._jump.requireRetroactiveJumpPress)
				{
					this._jump.requireRetroactiveJumpPress = false;
					this._jumpDirIntent = default(Vector2);
					History.Log("Retroactive jump success");
				}
				else if (flag6)
				{
					this._jumpQueued = true;
					this._jumpQueuedTime = Time.time;
					History.Log("Jump queued");
				}
				else
				{
					bool flag9 = false;
					if (this.wallSliding)
					{
						flag9 = PolySimulate.FindNearestPointOnPoly(this._wallSlide.poly, this.position, this.wallSlideValidAngleRange).climbable;
					}
					if (this.climbing || (this.wallSliding && flag9 && !this.staminaIsVeryLow))
					{
						this.TryJumpWhileClimbing(this._jumpDirIntent);
					}
					else if (this.wallSliding && (!flag9 || this.staminaIsVeryLow))
					{
						if (Vector2.Dot(this._wallSlide.normal, this._jumpDirIntent) > 0.5f)
						{
							this.TryJump(false);
						}
						else
						{
							this.WallSlideJumpFail(this._jumpDirIntent.y > 0f);
						}
					}
					else
					{
						this.TryJump(flag7);
					}
				}
			}
			this._useUphillSlopeConnections = false;
			if (GameInput.moveUpDown > 0f)
			{
				this._useUphillSlopeConnections = true;
			}
		}
		bool flag10 = false;
		if (this.momentumAbs <= this.settings.dropDownMaxMomentum && !flag && (this.running || this.balancing || this.cliffEdgeWobbling || this.onStoppedChairLift))
		{
			Runner.ClimbSetup climbSetup3 = this.TrySetupClimbingDown();
			if ((climbSetup3.ontoWall && this._climbOntoWall.climbable) || (climbSetup3.downToSlope && climbSetup3.downToSlopeDist > this.settings.climb.climbDownToSlopeMinDistForPrompt))
			{
				this._climbPromptAvailable = true;
				vector = (climbSetup3.downToSlope ? climbSetup3.climbDownStartPoint : this._climbing.mainPos);
				this._climbPromptIsUp = false;
			}
			if (this._upDownHeld < -this.settings.climb.inputUpDownThreshold && !flag)
			{
				flag10 = true;
				this._lookDownTime += Time.deltaTime;
				this._jumpDirIntent = Vector2.down;
				num = Mathf.Clamp01(this._lookDownTime / this.settings.climb.timeToLookDownBeforeStartClimb);
			}
			if (this._lookDownTime > this.settings.climb.timeToLookDownBeforeStartClimb && (climbSetup3.ontoWall || climbSetup3.downToSlope))
			{
				this._climbPromptFill = 0f;
				MonoSingleton<GameUI>.instance.climbPrompt.Confirm();
				this.direction = climbSetup3.direction;
				this.physicalDepthLayerIdx = climbSetup3.physicalDepthLayerIdx;
				if (climbSetup3.ontoWall)
				{
					this.state = Runner.State.ClimbOntoWall;
					return;
				}
				if (climbSetup3.downToSlope)
				{
					this.state = Runner.State.ClimbUpAndOver;
				}
				return;
			}
		}
		if (!flag10)
		{
			this._lookDownTime = 0f;
			if (!this.jumping)
			{
				this._jumpDirIntent = Vector2.zero;
			}
		}
		if (this._climbPromptAvailable)
		{
			this._climbPromptFill = (this._climbPromptIsUp ? 0f : num);
			MonoSingleton<GameUI>.instance.climbPrompt.ShowAt(vector, this._climbPromptIsUp, this._climbPromptFill, this.staminaIsVeryLow);
			return;
		}
		this._climbPromptFill = 0f;
		MonoSingleton<GameUI>.instance.climbPrompt.Hide(0f);
	}

	// Token: 0x06000751 RID: 1873 RVA: 0x00040F70 File Offset: 0x0003F170
	[return: TupleElementNames(new string[] { "shouldJump", "shouldJumpSpecial" })]
	private ValueTuple<bool, bool> ShouldAutoJumpNow()
	{
		if (!Runner.instance.isMusicRunning)
		{
			return new ValueTuple<bool, bool>(false, false);
		}
		RhythmActionMarker nearestMarker = MonoSingleton<TrackBuilder>.instance.nearestMarker;
		if (nearestMarker == null)
		{
			return new ValueTuple<bool, bool>(false, false);
		}
		if (Range.Auto(this._prevPosition.x, this.position.x).Contains(nearestMarker.transform.position.x))
		{
			return new ValueTuple<bool, bool>(true, nearestMarker.isSpecialJump);
		}
		return new ValueTuple<bool, bool>(false, false);
	}

	// Token: 0x06000752 RID: 1874 RVA: 0x00040FF8 File Offset: 0x0003F1F8
	public void UpdateDepth(bool immediate)
	{
		if (this.caught || this.isBoating || this.onChairLift)
		{
			return;
		}
		Transform transform = null;
		if (this.currentSlope != null)
		{
			transform = this.currentSlope.transform;
		}
		else if (this.balancing)
		{
			transform = this._balancePoint.transform;
		}
		float num = this.settings.layer.runnerDepthChangeSmoothTime;
		if (this.currentSlope != null)
		{
			this.physicalDepthLayerIdx = Mathf.RoundToInt(this.currentSlope.transform.position.z);
		}
		float num2 = (float)this.physicalDepthLayerIdx;
		if (this.jumping)
		{
			if (this._jump.targetSlope != null)
			{
				transform = this._jump.targetSlope.transform;
			}
			else if (this._jump.targetBalancePoint != null)
			{
				transform = this._jump.targetBalancePoint.transform;
			}
			num = this._jump.expectedDuration;
		}
		if (transform != null)
		{
			num2 = transform.transform.position.z;
		}
		if (this.climbing)
		{
			float z = this._climbing.poly.transform.position.z;
			this.physicalDepthLayerIdx = Mathf.RoundToInt(z);
			num2 = z;
		}
		else if (this.wallSliding)
		{
			float z2 = this._wallSlide.poly.transform.position.z;
			this.physicalDepthLayerIdx = Mathf.RoundToInt(z2);
			num2 = z2;
		}
		Vector3 position = base.transform.position;
		if (immediate)
		{
			position.z = num2;
			this._depthChangeSpeed = 0f;
		}
		else
		{
			position.z = Mathf.SmoothDamp(position.z, num2, ref this._depthChangeSpeed, num);
			position.z = Mathf.MoveTowards(position.z, num2, this.settings.layer.fixedZChangeMinSpeed * Time.deltaTime);
			if (Mathf.Abs(position.z - num2) < this.settings.layer.finalSnapZ)
			{
				position.z = num2;
				this._depthChangeSpeed = 0f;
			}
		}
		base.transform.position = position;
	}

	// Token: 0x06000753 RID: 1875 RVA: 0x00041220 File Offset: 0x0003F420
	private void UpdateMovement()
	{
		float dt = this._dt;
		int num = 1;
		if (this._dt > 0.06666667f)
		{
			num = Mathf.Clamp(Mathf.RoundToInt(this._dt / 0.033333335f), 1, 4);
			this._dt /= (float)num;
		}
		for (int i = 0; i < num; i++)
		{
			this.UpdateStateMachine(this._dt);
		}
		this._dt = dt;
		float moveLeftRight = GameInput.moveLeftRight;
		if (GameInput.jumped)
		{
			History.Log("Jump pressed");
		}
		float moveUpDown = GameInput.moveUpDown;
		this.lastSlopeJumpedFrom != null;
		this.lastBalancePointDroppedFrom != null;
		this.animator.currentAnimationVariant != null;
	}

	// Token: 0x06000754 RID: 1876 RVA: 0x000412DF File Offset: 0x0003F4DF
	private void OnAnimationReversedDirection()
	{
		this.direction = -this.direction;
		if (this.momentum * this.direction < 0f)
		{
			this.momentum = -this.momentum;
		}
	}

	// Token: 0x06000755 RID: 1877 RVA: 0x0004130F File Offset: 0x0003F50F
	private void OnAnimationRootMotion(Vector2 motion, bool worldSpace)
	{
		if (!worldSpace)
		{
			motion.x *= this.direction;
		}
		this.position += motion;
	}

	// Token: 0x170001F8 RID: 504
	// (get) Token: 0x06000756 RID: 1878 RVA: 0x00041337 File Offset: 0x0003F537
	public bool healthBeforeFallAtCritical
	{
		get
		{
			return this._lastSafeHealth < 2f;
		}
	}

	// Token: 0x06000757 RID: 1879 RVA: 0x00041348 File Offset: 0x0003F548
	private void UpdateAutomaticReset()
	{
		if (!Game.instance.inActiveGameplay || this.hidden || this.dead || this.onPath || this.hasInkPose || this.inFinalJumpAndLeftLand)
		{
			return;
		}
		bool flag = false;
		if (this.position.y < -10f)
		{
			string text = "Resetting to ";
			Vector3 vector = this._lastSafePosition;
			Debug.LogWarning(text + vector.ToString() + " because y pos < " + (-10f).ToString());
			flag = true;
		}
		else if (GameInput.debugReset)
		{
			string text2 = "Resetting to ";
			Vector3 vector = this._lastSafePosition;
			Debug.LogWarning(text2 + vector.ToString() + " because reset key was pressed");
			flag = true;
		}
		if (flag)
		{
			this.Die(true);
			return;
		}
		if ((this.running || this.resting || this.sitting) && (this.stateTimer > 1f || this._lastSafePosition == Vector3.zero) && this.currentSlope != null && Game.instance.inActiveGameplay && !Narrative.instance.isBusy)
		{
			Vector3 physicalPosition3d = this.physicalPosition3d;
			using (List<InvisibleCollision>.Enumerator enumerator = Level.current.invisibleCollision.Nearby(this.position, Range.infinity, 0f, null).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.Contains(physicalPosition3d, 0f))
					{
						return;
					}
				}
			}
			this.SetSafeResetPoint();
		}
	}

	// Token: 0x06000758 RID: 1880 RVA: 0x000414EC File Offset: 0x0003F6EC
	public void ClearSafeResetPoint()
	{
		this._lastSafePosition = Vector3.zero;
		this._lastSafeDirection = 0;
		this._lastSafeHealth = 0f;
	}

	// Token: 0x06000759 RID: 1881 RVA: 0x0004150B File Offset: 0x0003F70B
	public void SetSafeResetPoint()
	{
		this._lastSafePosition = base.transform.position;
		this._lastSafeDirection = Mathf.RoundToInt(Mathf.Sign(this.direction));
		this._lastSafeHealth = this.health.currentHealth;
	}

	// Token: 0x0600075A RID: 1882 RVA: 0x00041548 File Offset: 0x0003F748
	public void ResetToLastSafePosition(bool dontChangeState = false, bool restoreHealth = false)
	{
		if (this._lastSafePosition != Vector3.zero)
		{
			Game.instance.TeleportPlayerWithoutLevelChange(this._lastSafePosition, "Last safe pos reset", this._lastSafeDirection, dontChangeState, true);
			if (restoreHealth)
			{
				this.health.RestoreAfterFastDeathReset(this._lastSafeHealth);
				return;
			}
		}
		else
		{
			Debug.LogWarning("Can't reset because position has not been set!");
		}
	}

	// Token: 0x0600075B RID: 1883 RVA: 0x000415A3 File Offset: 0x0003F7A3
	public void RefreshImmediate()
	{
		this.UpdateFocus();
		this.UpdateDepth(true);
		this.UpdateLevelIdx();
		this.health.RefreshShelterProtectionStrength();
	}

	// Token: 0x0600075C RID: 1884 RVA: 0x000415C4 File Offset: 0x0003F7C4
	private float CalculateGravity(float jumpHeight, float jumpDuration)
	{
		float num = 0.5f * jumpDuration;
		return -(jumpHeight / (0.5f * num * num));
	}

	// Token: 0x0600075D RID: 1885 RVA: 0x000415E8 File Offset: 0x0003F7E8
	private Runner.FallCollision JumpFallCollideWithPolyWalls(Vector2 nextPosition, Range raycastRange, bool oval = true)
	{
		Vector2 vector = nextPosition - this.position;
		float num = this.settings.ovalSweptHeightForJump;
		if (this.falling && this._fallIsTumble)
		{
			num = 0f;
		}
		Vector3 vector2 = new Vector3(this.position.x, this.position.y + num, (float)this.physicalDepthLayerIdx);
		float num2;
		Raycast.Collision collision;
		if (oval)
		{
			Vector2 ovalSweptSizeForJump = this.settings.ovalSweptSizeForJump;
			if (this.falling && this._fallIsTumble)
			{
				ovalSweptSizeForJump.y = ovalSweptSizeForJump.x;
			}
			num2 = 0.5f * ovalSweptSizeForJump.x;
			collision = Raycast.OvalSwept<Poly>(vector2, this.rotation, ovalSweptSizeForJump, vector, raycastRange, null, true);
		}
		else
		{
			collision = Raycast.CollideWallPolygonsVec3(this.physicalPosition3d, vector, raycastRange, this.settings, false, null, false, Color.cyan);
			num2 = 0f;
		}
		if (collision.didHit && (!oval || Vector2.Dot(collision.normal, this.velocity) < 0f))
		{
			if (collision.unreachable)
			{
				float num3 = collision.poly.transform.position.z - 1f;
				SlopeSample slopeSample;
				if (Raycast.SampleWithDepthRange(collision.pos, collision.pos - 50f * Vector2.up, num3, new Range(num3 + this.settings.layer.layerCollideSafetyMin, num3 + 0.5f), out slopeSample, default(Color)).didHit)
				{
					int num4 = this.physicalDepthLayerIdx;
					this.physicalDepthLayerIdx = num4 - 1;
					return new Runner.FallCollision
					{
						collided = false
					};
				}
				Debug.LogWarning("Unreachable poly wall on " + collision.poly.name + "  wanted to push player forwards a layer but aborted because we couldn't see any slopes below. Should we allow it? Or perhaps you should change unreachable to climbable or non-climbable?", collision.poly);
			}
			Vector3 vector3 = collision.pos3d + 0.1f * collision.normal;
			Vector2 vector4 = 4f * Mathf.Sign(collision.normal.x) * Vector2.right;
			Range raycastCurrentAndNearForeRange = this.raycastCurrentAndNearForeRange;
			if (Raycast.CollideWallPolygonsVec3(vector3, vector4, raycastCurrentAndNearForeRange, this.settings, false, null, false, Color.red).didHit)
			{
				PolySimulate.Options options = new PolySimulate.Options
				{
					preventDirectionChange = true,
					validAngleRange = new Range(-360f, 360f),
					slopePolyCheckDepthRange = this.raycastCurrentAndSafeForeRange,
					runnerSettings = this.settings
				};
				PolySimulate.Result result = PolySimulate.AroundEdge(collision.poly, collision.pos, Vector2.down, 5f, options, null);
				if (result.endReason == PolySimulate.EndReason.PreventDirectionChange)
				{
					int num4 = this.physicalDepthLayerIdx;
					this.physicalDepthLayerIdx = num4 - 1;
					History.Log("Detected thin V-shaped gulley, pushing forward");
					return new Runner.FallCollision
					{
						collided = false
					};
				}
				if (result.endReason == PolySimulate.EndReason.SlopeIntersection)
				{
					History.Log("Detected thin V-shaped gulley, but saw Slope");
					return new Runner.FallCollision
					{
						collided = true,
						foundGulleySlope = result.slopeHit
					};
				}
			}
			if (this._currentFallDamage > Damage.None)
			{
				this.health.ApplyDamage(DamageType.Fall, this._currentFallDamage);
				this._currentFallDamage = Damage.None;
			}
			if (this._maxFallDamage == Damage.None && !this._fallIsTumble && !this.shouldDie && this.TrySetupGrabLedge(ClimbCheckRange.JumpOrFall, nextPosition, collision.poly, true))
			{
				return new Runner.FallCollision
				{
					collided = true,
					normal = collision.normal,
					collisionDepth = collision.pos3d.z
				};
			}
			if (this._maxFallDamage == Damage.None && !this._fallIsTumble && !this.shouldDie && !this._fallPreventsClimb && collision.climbable && this.TrySetupClimbing(ClimbCheckRange.JumpOrFall, true, false, false).isValid)
			{
				return new Runner.FallCollision
				{
					collided = true,
					normal = collision.normal,
					collisionDepth = collision.pos3d.z
				};
			}
			if (!this._fallIsTumble && !this.shouldDie && this.TryStartWallSlide())
			{
				return new Runner.FallCollision
				{
					collided = true,
					normal = collision.normal,
					collisionDepth = collision.pos3d.z
				};
			}
			Vector2 vector5 = this.velocity;
			Vector2 vector6 = this.position;
			Vector2 vector7 = (oval ? (this.position + collision.deltaNorm * vector) : collision.pos);
			float num5 = (oval ? 0.1f : 0.05f);
			Runner.BounceResult bounceResult = this.CalculateFallingBounce(this.position, vector7, collision.normal, num2, num5);
			if (bounceResult.shouldBounce)
			{
				vector5 = bounceResult.velocity;
				vector6 = bounceResult.position;
			}
			if (this.state != Runner.State.Falling)
			{
				this._prevVelocity = vector5;
			}
			this._fallVelocity = vector5;
			Vector2 vector8 = vector6 - this.position;
			if (vector8.sqrMagnitude > 0f)
			{
				Vector2 normalized = vector8.normalized;
				SlopeSample slopeSample2;
				if (Raycast.SampleWithDepthRange(this.position - 0.1f * normalized, vector6, (float)this.physicalDepthLayerIdx, this.raycastNearbyRange, out slopeSample2, Color.magenta).didHit)
				{
					this.currentSlope = slopeSample2.slope;
					this.position = slopeSample2.point;
					this.physicalDepthLayerIdx = Mathf.RoundToInt(slopeSample2.depth);
					this.state = Runner.State.Running;
					return new Runner.FallCollision
					{
						collided = true,
						normal = slopeSample2.normal,
						collisionDepth = slopeSample2.depth
					};
				}
			}
			if (oval)
			{
				if (!this.JumpFallCollideWithPolyWalls(vector6, raycastRange, false).collided)
				{
					this.position = vector6;
				}
			}
			else
			{
				this.position = vector6;
			}
			if (collision.pos3d.z < (float)this.physicalDepthLayerIdx)
			{
				Vector2 vector9 = this.position + 0.5f * Vector2.up + 0.5f * collision.normal;
				SlopeSample slopeSample3;
				if (!Raycast.SampleWithDepthRange(vector9, vector9 - 5f * Vector2.down, (float)this.physicalDepthLayerIdx, this.raycastCurrentRangeONLY, out slopeSample3, default(Color)).didHit)
				{
					this.physicalDepthLayerIdx = Mathf.RoundToInt(collision.pos3d.z);
				}
			}
			if (bounceResult.speedIntoNormal > 6f && Time.time - this._lastWallCollideVocalisationTime > 2f)
			{
				AudioController.instance.PlayVocalisation(Vocalisation.JumpFallHitWall, 0f);
				this._lastWallCollideVocalisationTime = Time.time;
			}
			if (this.speed > this.settings.fall.minCollisionSpeedToStartTumble && this.falling)
			{
				this._fallIsTumble = true;
			}
			this.state = Runner.State.Falling;
			return new Runner.FallCollision
			{
				collided = true,
				normal = collision.normal,
				collisionDepth = collision.pos3d.z
			};
		}
		else
		{
			if (oval)
			{
				return this.JumpFallCollideWithPolyWalls(nextPosition, raycastRange, false);
			}
			return new Runner.FallCollision
			{
				collided = false,
				normal = Vector2.zero
			};
		}
	}

	// Token: 0x0600075E RID: 1886 RVA: 0x00041D24 File Offset: 0x0003FF24
	private Runner.BounceResult CalculateFallingBounce(Vector2 positionBefore, Vector2 collidePos, Vector2 normal, float collisionRadius, float pushBackDist)
	{
		Vector2 vector = this.velocity;
		float num = -Vector2.Dot(this.velocity, normal);
		if (num > 0f)
		{
			if (this._fallIsTumble && Random.value < this.settings.fall.tumbleBigBounceChance)
			{
				vector = this.settings.fall.tumbleBigBounceRestitution * this.velocity.magnitude * normal;
			}
			else
			{
				float num2 = (this._fallIsTumble ? this.settings.fall.fallBounceRestitutionTumble : this.settings.fall.fallBounceRestitution);
				float num3 = (this._fallIsTumble ? this.settings.fall.fallBounceDampingTumble : this.settings.fall.fallBounceDamping);
				vector += (1f + num2) * num * normal;
				vector *= num3;
			}
			if (vector.y > 0f && vector.magnitude > 15f)
			{
				vector = 15f * vector.normalized;
			}
			return new Runner.BounceResult
			{
				shouldBounce = true,
				position = collidePos + (collisionRadius + pushBackDist) * normal,
				velocity = vector,
				speedIntoNormal = num
			};
		}
		return new Runner.BounceResult
		{
			shouldBounce = false
		};
	}

	// Token: 0x0600075F RID: 1887 RVA: 0x00041E88 File Offset: 0x00040088
	private void OnValidate()
	{
		this.direction = (float)((this.direction >= 0f) ? 1 : (-1));
	}

	// Token: 0x0400073E RID: 1854
	private float _balanceExitHoldTime;

	// Token: 0x0400073F RID: 1855
	[Nullable(2)]
	private BalancePoint _balancePoint;

	// Token: 0x04000740 RID: 1856
	[Nullable(2)]
	private BalancePoint _prevBalancePoint;

	// Token: 0x04000741 RID: 1857
	private float _lastBalanceTime;

	// Token: 0x04000742 RID: 1858
	private Runner.BellyWriggle _wriggle;

	// Token: 0x04000743 RID: 1859
	private const float maxEndFindProximity = 10f;

	// Token: 0x04000744 RID: 1860
	private Vector3 _boatVelocity;

	// Token: 0x04000745 RID: 1861
	private Vector3 _catchAimPos;

	// Token: 0x04000746 RID: 1862
	private bool _hasCapturedAimPos;

	// Token: 0x04000747 RID: 1863
	private Vector3 _lastCaughtPos;

	// Token: 0x04000748 RID: 1864
	private Vector2 _caughtVelocity;

	// Token: 0x04000749 RID: 1865
	private SkiLiftChair _skiLiftChair;

	// Token: 0x0400074A RID: 1866
	private bool _skiLiftStoppedWhileOnIt;

	// Token: 0x0400074B RID: 1867
	private bool _jumpingOntoChairLift;

	// Token: 0x0400074C RID: 1868
	private bool _cliffEdgeWobbleDidReleaseInput;

	// Token: 0x0400074D RID: 1869
	private float _cliffEdgeWobbleDirection;

	// Token: 0x0400074E RID: 1870
	private float _cliffEdgeWobbleConfirmHoldDuration;

	// Token: 0x0400074F RID: 1871
	[Nullable(1)]
	public const string neverSlipPrefName = "neverSlip";

	// Token: 0x04000750 RID: 1872
	private Runner.Climbing _climbing;

	// Token: 0x04000751 RID: 1873
	[Nullable(1)]
	[SerializeField]
	private ParticleSystem _perspirationParticles = Presume<ParticleSystem>.NonNull("C:\\Users\\josep\\Desktop\\highland-song\\Highland Run\\Assets\\Scripts\\Runner\\Runner+Climbing.cs", 903);

	// Token: 0x04000752 RID: 1874
	private Runner.ClimbingJump _climbingJump;

	// Token: 0x04000753 RID: 1875
	private Runner.ClimbOffWall _climbOffWall;

	// Token: 0x04000754 RID: 1876
	private Runner.ClimbOntoWall _climbOntoWall;

	// Token: 0x04000755 RID: 1877
	[Nullable(2)]
	private QuickButtonPrompt _slipPrompt;

	// Token: 0x04000756 RID: 1878
	private float _climbSlipRotation;

	// Token: 0x04000757 RID: 1879
	private float _climbSlipRotationSpeed;

	// Token: 0x04000758 RID: 1880
	private Runner.ClimbUpAndOver _climbUpAndOver;

	// Token: 0x04000759 RID: 1881
	private Vector3 _debugFlyInput;

	// Token: 0x0400075A RID: 1882
	private Vector3 _debugFlyVelocity;

	// Token: 0x0400075B RID: 1883
	private bool _enterExitDoorIsEnter;

	// Token: 0x0400075C RID: 1884
	private bool _pathIsEnter;

	// Token: 0x0400075D RID: 1885
	private bool _pathIsAway;

	// Token: 0x0400075E RID: 1886
	private Prop.PathAnimType _pathAnimType;

	// Token: 0x0400075F RID: 1887
	private Vector3 _pathOrigin;

	// Token: 0x04000760 RID: 1888
	private Vector3 _enterExitPathStartPos;

	// Token: 0x04000761 RID: 1889
	[Nullable(2)]
	private Slope _pathExitSlope;

	// Token: 0x04000762 RID: 1890
	private Vector2 _animatorExpectedWorldPos;

	// Token: 0x04000763 RID: 1891
	private Vector3 _explosionSource;

	// Token: 0x04000764 RID: 1892
	private Vector3 _explosionVelocity;

	// Token: 0x04000765 RID: 1893
	private Prop _explosionDestination;

	// Token: 0x04000766 RID: 1894
	private bool _explosionHasTeleported;

	// Token: 0x04000767 RID: 1895
	private Vector2 _fallVelocity;

	// Token: 0x04000768 RID: 1896
	private Damage _currentFallDamage;

	// Token: 0x04000769 RID: 1897
	private Damage _maxFallDamage;

	// Token: 0x0400076A RID: 1898
	private bool _fallHasDamaged;

	// Token: 0x0400076B RID: 1899
	private bool _fallPreventsClimb;

	// Token: 0x0400076C RID: 1900
	private float _fallLastTouchY;

	// Token: 0x0400076D RID: 1901
	private float _fallStartY;

	// Token: 0x0400076E RID: 1902
	private bool _fallIsTumble;

	// Token: 0x0400076F RID: 1903
	private bool _tumbleIsBackwards;

	// Token: 0x04000770 RID: 1904
	private float _tumbleSpeed;

	// Token: 0x04000771 RID: 1905
	private const int fallHistoryMaxSize = 10;

	// Token: 0x04000772 RID: 1906
	[Nullable(1)]
	private Queue<Vector2> _fallPosHistory = new Queue<Vector2>(11);

	// Token: 0x04000773 RID: 1907
	[Nullable(1)]
	[SerializeField]
	private ParticleSystem _snowPoofParticles = Presume<ParticleSystem>.NonNull("C:\\Users\\josep\\Desktop\\highland-song\\Highland Run\\Assets\\Scripts\\Runner\\Runner+Falling.cs", 403);

	// Token: 0x04000774 RID: 1908
	[Nullable(1)]
	[SerializeField]
	private ParticleSystem _snowPoofFloatyParticles = Presume<ParticleSystem>.NonNull("C:\\Users\\josep\\Desktop\\highland-song\\Highland Run\\Assets\\Scripts\\Runner\\Runner+Falling.cs", 404);

	// Token: 0x04000775 RID: 1909
	private Vector2 _swanDiveVelocity;

	// Token: 0x04000776 RID: 1910
	private float _leftRightHoldTime;

	// Token: 0x04000777 RID: 1911
	private float _divingTime;

	// Token: 0x04000778 RID: 1912
	private float _pauseTime;

	// Token: 0x04000779 RID: 1913
	private float _musicWantedPauseTime;

	// Token: 0x0400077A RID: 1914
	private RhythmActionMarker _jumpMarker;

	// Token: 0x0400077B RID: 1915
	private Runner.FinalJumpSubState _finalJumpSubState;

	// Token: 0x0400077C RID: 1916
	[SerializeField]
	private ParticleSystem _splashParticles;

	// Token: 0x0400077D RID: 1917
	private static bool _hasOriginalJumpMarkerPosition;

	// Token: 0x0400077E RID: 1918
	private static Vector3 _originalJumpMarkerPosition;

	// Token: 0x0400077F RID: 1919
	private float _hardLandingAngle;

	// Token: 0x04000780 RID: 1920
	private bool _hardLandingWasSoftenedBySnow;

	// Token: 0x04000781 RID: 1921
	[SerializeField]
	[Disable]
	private HideReason _hideReasonFlags;

	// Token: 0x04000782 RID: 1922
	[Nullable(2)]
	public static Action onJumpStart;

	// Token: 0x04000783 RID: 1923
	[Nullable(2)]
	public static Action onJumpEnd;

	// Token: 0x04000784 RID: 1924
	[Nullable(2)]
	public static Runner.OnDidJumpObstacleDelegate onDidJumpObstacle;

	// Token: 0x04000785 RID: 1925
	private Runner.JumpState _jump;

	// Token: 0x04000786 RID: 1926
	private bool _jumpQueued;

	// Token: 0x04000787 RID: 1927
	private float _jumpQueuedTime;

	// Token: 0x04000788 RID: 1928
	private Vector2 _jumpDirIntent;

	// Token: 0x04000789 RID: 1929
	private float _lastJumpTime = float.MinValue;

	// Token: 0x0400078A RID: 1930
	private int _jumpsInARow;

	// Token: 0x0400078B RID: 1931
	private int _stumbleCountInRow;

	// Token: 0x0400078C RID: 1932
	private int _jumpIndex;

	// Token: 0x0400078D RID: 1933
	private float _lastWhoopTime;

	// Token: 0x0400078E RID: 1934
	[Nullable(1)]
	private List<Slope> _slopesUnderJumpScratch = new List<Slope>();

	// Token: 0x0400078F RID: 1935
	private Runner.AnimWithTransitions _activePlayAnim;

	// Token: 0x04000790 RID: 1936
	private Runner.AnimWithTransitions _nextPlayAnim;

	// Token: 0x04000791 RID: 1937
	private bool _exitingAnimation;

	// Token: 0x04000792 RID: 1938
	private static List<Runner.AnimWithTransitions> _anims = new List<Runner.AnimWithTransitions>
	{
		new Runner.AnimWithTransitions
		{
			inkName = "Sit",
			transitionIn = "IdleToResting",
			animName = "Resting",
			transitionOut = "RestingToIdle"
		},
		new Runner.AnimWithTransitions
		{
			inkName = "Crouch",
			transitionIn = "IdleToCrouching",
			animName = "Crouching",
			transitionOut = "CrouchingToIdle"
		},
		new Runner.AnimWithTransitions
		{
			inkName = "PickUp",
			animName = "PickUp"
		},
		new Runner.AnimWithTransitions
		{
			inkName = "StepBack",
			animName = "StepBack"
		},
		new Runner.AnimWithTransitions
		{
			inkName = "ReachInside",
			animName = "ReachDownInto"
		},
		new Runner.AnimWithTransitions
		{
			inkName = "AirPunch",
			animName = "JumpForJoy"
		},
		new Runner.AnimWithTransitions
		{
			inkName = "FallOver",
			transitionIn = "FallToCollapsed",
			animName = "Collapsed",
			transitionOut = "CrouchingToIdle"
		},
		new Runner.AnimWithTransitions
		{
			inkName = "LookAt",
			transitionIn = "EnterDoor",
			animName = "LookAt",
			transitionOut = "TurnFromBackToSide"
		},
		new Runner.AnimWithTransitions
		{
			inkName = "LookOut",
			transitionIn = "EnterDoor",
			animName = "LookOut",
			transitionOut = "TurnFromBackToSide"
		},
		new Runner.AnimWithTransitions
		{
			inkName = "LookDown",
			transitionIn = "IdleToLookDown",
			animName = "LookDown",
			transitionOut = "LookDownToIdle"
		},
		new Runner.AnimWithTransitions
		{
			inkName = "RopePull",
			transitionIn = "IdleToRopePull",
			animName = "RopePullLoop",
			transitionOut = "RopePullToIdle"
		},
		new Runner.AnimWithTransitions
		{
			inkName = "Push",
			transitionIn = "IdleToPush",
			animName = "Push",
			transitionOut = "PushToIdle"
		}
	};

	// Token: 0x04000793 RID: 1939
	[Nullable(1)]
	public const string dontMusicRunTripPrefName = "dontMusicRunTrip";

	// Token: 0x04000794 RID: 1940
	[Nullable(1)]
	public const string singleJumpButtonPrefName = "singleJumpButton";

	// Token: 0x04000795 RID: 1941
	private static bool _singleJumpButtonOnly;

	// Token: 0x04000796 RID: 1942
	private static bool _singleJumpButtonOnlyCached;

	// Token: 0x04000797 RID: 1943
	public bool isSprinting;

	// Token: 0x04000798 RID: 1944
	private float _sprintTimer;

	// Token: 0x04000799 RID: 1945
	private float _nextLowStaminaHurtTime;

	// Token: 0x0400079A RID: 1946
	private bool _wasLockedFullSpeedWithoutInput;

	// Token: 0x0400079B RID: 1947
	private float _lastReverseInterruptTime = -1f;

	// Token: 0x0400079C RID: 1948
	private float _stumbleTimer;

	// Token: 0x0400079D RID: 1949
	private int _stumbleCount;

	// Token: 0x0400079E RID: 1950
	private float _lastSlidingVocalisationTime;

	// Token: 0x0400079F RID: 1951
	private float _runningLeanAngle;

	// Token: 0x040007A0 RID: 1952
	[Nullable(1)]
	[SerializeField]
	private ParticleSystem _slideParticles = Presume<ParticleSystem>.NonNull("C:\\Users\\josep\\Desktop\\highland-song\\Highland Run\\Assets\\Scripts\\Runner\\Runner+RunningSliding.cs", 1253);

	// Token: 0x040007A1 RID: 1953
	[Nullable(1)]
	[SerializeField]
	private ParticleSystem _slowDownParticles = Presume<ParticleSystem>.NonNull("C:\\Users\\josep\\Desktop\\highland-song\\Highland Run\\Assets\\Scripts\\Runner\\Runner+RunningSliding.cs", 1254);

	// Token: 0x040007A2 RID: 1954
	[Nullable(1)]
	[SerializeField]
	private ParticleSystem _sprintDustParticles = Presume<ParticleSystem>.NonNull("C:\\Users\\josep\\Desktop\\highland-song\\Highland Run\\Assets\\Scripts\\Runner\\Runner+RunningSliding.cs", 1255);

	// Token: 0x040007A3 RID: 1955
	private Runner.State _state;

	// Token: 0x040007A4 RID: 1956
	public Runner.State prevState;

	// Token: 0x040007A5 RID: 1957
	public float stateTimer;

	// Token: 0x040007A6 RID: 1958
	public float prevStateTimer;

	// Token: 0x040007A7 RID: 1959
	public string stateName;

	// Token: 0x040007A8 RID: 1960
	private Runner.SkimStoneSubState _stoneSkimSubState;

	// Token: 0x040007A9 RID: 1961
	public Runner.StoneSkimming stoneSkim;

	// Token: 0x040007AC RID: 1964
	[NonSerialized]
	public float stoneSkimWindingTime;

	// Token: 0x040007AD RID: 1965
	[Nullable(2)]
	private Transform _stoneGO;

	// Token: 0x040007AE RID: 1966
	private float _stoneSinkTimer;

	// Token: 0x040007AF RID: 1967
	private Vector3 _stoneVelocity;

	// Token: 0x040007B0 RID: 1968
	private Vector3 _stonePosition;

	// Token: 0x040007B1 RID: 1969
	private int _stoneSkimBounces;

	// Token: 0x040007B2 RID: 1970
	private Vector3 _tranitionIntoPositionPositionAdjust;

	// Token: 0x040007B3 RID: 1971
	[Nullable(2)]
	private TransitionSettings.Transition _activeTransition;

	// Token: 0x040007B4 RID: 1972
	private Range _activeTransitionAnimTimeRangeNorm;

	// Token: 0x040007B5 RID: 1973
	private Vector3 _transitionFocusStartPos;

	// Token: 0x040007B6 RID: 1974
	private Vector3 _transitionFocusEndPos;

	// Token: 0x040007B7 RID: 1975
	private Vector3 _transitionFocusPos;

	// Token: 0x040007B8 RID: 1976
	private float _lastPositionAdjustTimeNorm;

	// Token: 0x040007B9 RID: 1977
	private float _tripDirection;

	// Token: 0x040007BA RID: 1978
	private Runner.WallSlide _wallSlide;

	// Token: 0x040007BB RID: 1979
	private Vector2 _wallSlideFallFromPoint;

	// Token: 0x040007BC RID: 1980
	[Nullable(2)]
	private Poly _wallSlideFallFromPoly;

	// Token: 0x040007BD RID: 1981
	private ZipLine _zipLine;

	// Token: 0x040007BE RID: 1982
	private float _zipLineSpeed;

	// Token: 0x040007BF RID: 1983
	public static Action<bool> onBecameActive;

	// Token: 0x040007C0 RID: 1984
	public static Action<float> onTrip;

	// Token: 0x040007C1 RID: 1985
	public static Action<int> onFootstep;

	// Token: 0x040007C3 RID: 1987
	public PlayerControlDisableReason playerControlDisabled;

	// Token: 0x040007C4 RID: 1988
	public MusicRun prevMusicRun;

	// Token: 0x040007C5 RID: 1989
	private Health _health;

	// Token: 0x040007C6 RID: 1990
	public FrameAnimator animator;

	// Token: 0x040007C7 RID: 1991
	public MusicalTrail musicalTrail;

	// Token: 0x040007C8 RID: 1992
	private SpriteTrail _spriteTrail;

	// Token: 0x040007C9 RID: 1993
	private Torch _torch;

	// Token: 0x040007CA RID: 1994
	private CameraVolume[] _cameraVolumes;

	// Token: 0x040007CB RID: 1995
	[Range(-2.5f, 2.5f)]
	[SerializeField]
	private float _momentum;

	// Token: 0x040007CC RID: 1996
	public string debugMomentumType = "";

	// Token: 0x040007CD RID: 1997
	public string debugMomentumAccelType = "";

	// Token: 0x040007CE RID: 1998
	public float debugMomentumAccel;

	// Token: 0x040007CF RID: 1999
	public const float initialMaxStamina = 1.01f;

	// Token: 0x040007D0 RID: 2000
	[Range(0f, 2f)]
	[Disable]
	public float stamina = 1f;

	// Token: 0x040007D1 RID: 2001
	public float maxStamina = 1.01f;

	// Token: 0x040007D2 RID: 2002
	[Range(0f, 1f)]
	[Disable]
	public float wallSlideStamina = 1f;

	// Token: 0x040007D3 RID: 2003
	private int _levelIdx = -1;

	// Token: 0x040007D4 RID: 2004
	public Range levelXRange;

	// Token: 0x040007D5 RID: 2005
	private int _physicalDepth;

	// Token: 0x040007D6 RID: 2006
	private Slope _currentSlope;

	// Token: 0x040007D7 RID: 2007
	private bool _currentSampleDirty = true;

	// Token: 0x040007D8 RID: 2008
	private SlopeSample _currentSample;

	// Token: 0x040007D9 RID: 2009
	public Slope lastSlopeJumpedFrom;

	// Token: 0x040007DA RID: 2010
	public BalancePoint lastBalancePointDroppedFrom;

	// Token: 0x040007DB RID: 2011
	public float direction = 1f;

	// Token: 0x040007DC RID: 2012
	private Vector2 _cachedVelocity;

	// Token: 0x040007DD RID: 2013
	private float _cachedSpeed;

	// Token: 0x040007DE RID: 2014
	private bool _inStateUpdate;

	// Token: 0x040007E0 RID: 2016
	public float remainingAutoRunTargetTime = -1f;

	// Token: 0x040007E1 RID: 2017
	private Vector3 _autoRunTarget = Vector3.zero;

	// Token: 0x040007E2 RID: 2018
	private bool _autoRunTargetNeedsPrecision;

	// Token: 0x040007E3 RID: 2019
	private bool _hasAutoRunTarget;

	// Token: 0x040007E4 RID: 2020
	[NonSerialized]
	public bool inSlowdownTrigger;

	// Token: 0x040007E7 RID: 2023
	public RunnerSettings settings;

	// Token: 0x040007E8 RID: 2024
	public SurfaceTypeSampler surfaceTypeSampler = new SurfaceTypeSampler();

	// Token: 0x040007E9 RID: 2025
	public float nextSurfaceSampleTime;

	// Token: 0x040007EA RID: 2026
	[Disable]
	public bool isOnRidge;

	// Token: 0x040007EB RID: 2027
	private BezierSimple.Point[] _trailBezierScratch = new BezierSimple.Point[4];

	// Token: 0x040007EC RID: 2028
	private const float standardFocusHeight = 4f;

	// Token: 0x040007ED RID: 2029
	private Vector3 _lastSafePosition;

	// Token: 0x040007EE RID: 2030
	private int _lastSafeDirection;

	// Token: 0x040007EF RID: 2031
	private float _lastSafeHealth;

	// Token: 0x040007F0 RID: 2032
	private float _dt;

	// Token: 0x040007F1 RID: 2033
	private float _leftRightInput;

	// Token: 0x040007F2 RID: 2034
	private Vector2 _move2dInput;

	// Token: 0x040007F3 RID: 2035
	private bool _sprintPressed;

	// Token: 0x040007F4 RID: 2036
	private bool _sprintHeld;

	// Token: 0x040007F5 RID: 2037
	private float _upDownPressed;

	// Token: 0x040007F6 RID: 2038
	private float _upDownHeld;

	// Token: 0x040007F7 RID: 2039
	private float _depthChangeSpeed;

	// Token: 0x040007F8 RID: 2040
	private bool _useUphillSlopeConnections;

	// Token: 0x040007F9 RID: 2041
	private Vector2 _prevVelocity;

	// Token: 0x040007FA RID: 2042
	private float _prevSpeed;

	// Token: 0x040007FB RID: 2043
	private Vector2 _prevPosition;

	// Token: 0x040007FC RID: 2044
	private float _lookDownTime;

	// Token: 0x040007FD RID: 2045
	private float _stoppedAndPausedTimer;

	// Token: 0x040007FE RID: 2046
	private bool _restoringStamina;

	// Token: 0x040007FF RID: 2047
	private int _lastFootstep = -1;

	// Token: 0x04000800 RID: 2048
	private float _lastWallCollideVocalisationTime;

	// Token: 0x04000801 RID: 2049
	private float _climbPromptFill;

	// Token: 0x04000802 RID: 2050
	private bool _climbPromptAvailable;

	// Token: 0x04000803 RID: 2051
	private bool _climbPromptIsUp;

	// Token: 0x04000804 RID: 2052
	[SerializeField]
	private Transform _mouth;

	// Token: 0x020002EF RID: 751
	private struct BellyWriggle
	{
		// Token: 0x040016DD RID: 5853
		public bool entering;

		// Token: 0x040016DE RID: 5854
		public bool exiting;

		// Token: 0x040016DF RID: 5855
		public BellyWriggleEnd entry;

		// Token: 0x040016E0 RID: 5856
		public float stuckButTriedMoveTime;

		// Token: 0x040016E1 RID: 5857
		public float lastStuckInkCallTime;

		// Token: 0x040016E2 RID: 5858
		public float lastGruntTime;

		// Token: 0x040016E3 RID: 5859
		public bool hasVocalisedCurrentStuck;
	}

	// Token: 0x020002F0 RID: 752
	private struct ClimbSetup
	{
		// Token: 0x170004C9 RID: 1225
		// (get) Token: 0x06001668 RID: 5736 RVA: 0x00098738 File Offset: 0x00096938
		public bool isValid
		{
			get
			{
				return this.ontoWall || this.downToSlope || this.upAndOver;
			}
		}

		// Token: 0x040016E4 RID: 5860
		public bool ontoWall;

		// Token: 0x040016E5 RID: 5861
		public bool downToSlope;

		// Token: 0x040016E6 RID: 5862
		public Vector3 climbDownStartPoint;

		// Token: 0x040016E7 RID: 5863
		public float downToSlopeDist;

		// Token: 0x040016E8 RID: 5864
		public bool upAndOver;

		// Token: 0x040016E9 RID: 5865
		public float direction;

		// Token: 0x040016EA RID: 5866
		public int physicalDepthLayerIdx;

		// Token: 0x040016EB RID: 5867
		public static Runner.ClimbSetup none;
	}

	// Token: 0x020002F1 RID: 753
	private struct Climbing
	{
		// Token: 0x170004CA RID: 1226
		// (get) Token: 0x0600166A RID: 5738 RVA: 0x00098754 File Offset: 0x00096954
		public bool overhang
		{
			get
			{
				return this.angle > 90f;
			}
		}

		// Token: 0x170004CB RID: 1227
		// (get) Token: 0x0600166B RID: 5739 RVA: 0x00098763 File Offset: 0x00096963
		public bool lastUpperHoldValid
		{
			get
			{
				return this.lastUpperHold.x != float.MaxValue;
			}
		}

		// Token: 0x170004CC RID: 1228
		// (get) Token: 0x0600166C RID: 5740 RVA: 0x0009877A File Offset: 0x0009697A
		public bool lastLowerHoldValid
		{
			get
			{
				return this.lastLowerHold.x != float.MaxValue;
			}
		}

		// Token: 0x040016EC RID: 5868
		public int reachDir;

		// Token: 0x040016ED RID: 5869
		public bool reachRightHanded;

		// Token: 0x040016EE RID: 5870
		public float reachProgress;

		// Token: 0x040016EF RID: 5871
		[Nullable(2)]
		public Poly poly;

		// Token: 0x040016F0 RID: 5872
		public Vector3 mainPos;

		// Token: 0x040016F1 RID: 5873
		public Vector2 normal;

		// Token: 0x040016F2 RID: 5874
		public float angle;

		// Token: 0x040016F3 RID: 5875
		public float timeToSlip;

		// Token: 0x040016F4 RID: 5876
		public Vector3 lastUpperHold;

		// Token: 0x040016F5 RID: 5877
		public Vector3 lastLowerHold;

		// Token: 0x040016F6 RID: 5878
		public float lastUpperHoldDist;

		// Token: 0x040016F7 RID: 5879
		public float lastLowerHoldDist;

		// Token: 0x040016F8 RID: 5880
		public bool upperLimitHit;

		// Token: 0x040016F9 RID: 5881
		public bool lowerLimitHit;

		// Token: 0x040016FA RID: 5882
		public bool canDropDownOrSlide;

		// Token: 0x040016FB RID: 5883
		public float dropConfirmHoldDuration;

		// Token: 0x040016FC RID: 5884
		public float nextLowStaminaHurtTime;

		// Token: 0x040016FD RID: 5885
		public float climbDownCouldSlideTimer;
	}

	// Token: 0x020002F2 RID: 754
	private struct ClimbJumpSearchStep
	{
		// Token: 0x040016FE RID: 5886
		public Vector3 pos;

		// Token: 0x040016FF RID: 5887
		[Nullable(1)]
		public Poly poly;

		// Token: 0x04001700 RID: 5888
		public Vector2 norm;
	}

	// Token: 0x020002F3 RID: 755
	private struct ClimbingJump
	{
		// Token: 0x04001701 RID: 5889
		public Vector3 target3d;

		// Token: 0x04001702 RID: 5890
		[Nullable(1)]
		public Poly targetPoly;

		// Token: 0x04001703 RID: 5891
		public Vector2 targetNormal;

		// Token: 0x04001704 RID: 5892
		public float targetRotation;

		// Token: 0x04001705 RID: 5893
		public float startStamina;

		// Token: 0x04001706 RID: 5894
		public float endStamina;

		// Token: 0x04001707 RID: 5895
		public bool upward;

		// Token: 0x04001708 RID: 5896
		public bool endsInUpAndOver;

		// Token: 0x04001709 RID: 5897
		public bool endsInNonClimbable;
	}

	// Token: 0x020002F4 RID: 756
	private struct ClimbOffWall
	{
		// Token: 0x0400170A RID: 5898
		public Vector3 targetPos;

		// Token: 0x0400170B RID: 5899
		public Slope targetSlope;

		// Token: 0x0400170C RID: 5900
		public float speedScalar;
	}

	// Token: 0x020002F5 RID: 757
	private struct ClimbOntoWall
	{
		// Token: 0x0400170D RID: 5901
		public bool climbable;

		// Token: 0x0400170E RID: 5902
		[Nullable(1)]
		public TransitionSettings.Transition transition;
	}

	// Token: 0x020002F6 RID: 758
	[NullableContext(2)]
	[Nullable(0)]
	private struct ClimbUpAndOver
	{
		// Token: 0x0400170F RID: 5903
		public Slope slope;

		// Token: 0x04001710 RID: 5904
		public BalancePoint balancePoint;

		// Token: 0x04001711 RID: 5905
		public Vector3 targetPos;

		// Token: 0x04001712 RID: 5906
		[Nullable(1)]
		public TransitionSettings.Transition transition;

		// Token: 0x04001713 RID: 5907
		public float momentumAfter;

		// Token: 0x04001714 RID: 5908
		public float simpleLerpDuration;

		// Token: 0x04001715 RID: 5909
		public Vector3 startPos;
	}

	// Token: 0x020002F7 RID: 759
	private struct FallSlopeCollision
	{
		// Token: 0x04001716 RID: 5910
		public bool changedState;

		// Token: 0x04001717 RID: 5911
		public bool didBounce;

		// Token: 0x04001718 RID: 5912
		public Vector2 bouncePositionAfter;
	}

	// Token: 0x020002F8 RID: 760
	private enum FinalJumpSubState
	{
		// Token: 0x0400171A RID: 5914
		None,
		// Token: 0x0400171B RID: 5915
		Prep,
		// Token: 0x0400171C RID: 5916
		WillJumpImmediate,
		// Token: 0x0400171D RID: 5917
		WaitingForMusicSync,
		// Token: 0x0400171E RID: 5918
		PauseAndZoomBeforeLaunch,
		// Token: 0x0400171F RID: 5919
		Launching,
		// Token: 0x04001720 RID: 5920
		Diving,
		// Token: 0x04001721 RID: 5921
		InTheSea
	}

	// Token: 0x020002F9 RID: 761
	// (Invoke) Token: 0x0600166E RID: 5742
	public delegate void OnDidJumpObstacleDelegate(BeatTrack.ObstacleRef obsRef, bool success, bool nailedIt, float timeToObs);

	// Token: 0x020002FA RID: 762
	[NullableContext(2)]
	[Nullable(0)]
	[Serializable]
	public struct JumpState
	{
		// Token: 0x04001722 RID: 5922
		public Vector2 startPos;

		// Token: 0x04001723 RID: 5923
		public Vector2 targetPos;

		// Token: 0x04001724 RID: 5924
		public Vector2 velocity;

		// Token: 0x04001725 RID: 5925
		public float initialVerticalVelocity;

		// Token: 0x04001726 RID: 5926
		public float gravity;

		// Token: 0x04001727 RID: 5927
		public BalancePoint targetBalancePoint;

		// Token: 0x04001728 RID: 5928
		public Slope targetSlope;

		// Token: 0x04001729 RID: 5929
		public bool requireRetroactiveJumpPress;

		// Token: 0x0400172A RID: 5930
		public float expectedDuration;

		// Token: 0x0400172B RID: 5931
		public Range airControlSpeedLimit;

		// Token: 0x0400172C RID: 5932
		public bool special;

		// Token: 0x0400172D RID: 5933
		public bool isDropDown;

		// Token: 0x0400172E RID: 5934
		public bool specialNarrative;
	}

	// Token: 0x020002FB RID: 763
	public class AnimWithTransitions
	{
		// Token: 0x0400172F RID: 5935
		public string inkName;

		// Token: 0x04001730 RID: 5936
		public string transitionIn;

		// Token: 0x04001731 RID: 5937
		public string animName;

		// Token: 0x04001732 RID: 5938
		public string transitionOut;
	}

	// Token: 0x020002FC RID: 764
	public enum State
	{
		// Token: 0x04001734 RID: 5940
		None,
		// Token: 0x04001735 RID: 5941
		Hidden,
		// Token: 0x04001736 RID: 5942
		Running,
		// Token: 0x04001737 RID: 5943
		Sliding,
		// Token: 0x04001738 RID: 5944
		Jumping,
		// Token: 0x04001739 RID: 5945
		Balancing,
		// Token: 0x0400173A RID: 5946
		CliffEdgeWobbling,
		// Token: 0x0400173B RID: 5947
		Tripping,
		// Token: 0x0400173C RID: 5948
		Falling,
		// Token: 0x0400173D RID: 5949
		Caught,
		// Token: 0x0400173E RID: 5950
		Climbing,
		// Token: 0x0400173F RID: 5951
		ClimbSlipping,
		// Token: 0x04001740 RID: 5952
		ClimbOntoWall,
		// Token: 0x04001741 RID: 5953
		ClimbOffWall,
		// Token: 0x04001742 RID: 5954
		ClimbingJump,
		// Token: 0x04001743 RID: 5955
		ClimbUpAndOver,
		// Token: 0x04001744 RID: 5956
		WallSliding,
		// Token: 0x04001745 RID: 5957
		Sitting,
		// Token: 0x04001746 RID: 5958
		HardLanding,
		// Token: 0x04001747 RID: 5959
		Dead,
		// Token: 0x04001748 RID: 5960
		DebugFlying,
		// Token: 0x04001749 RID: 5961
		StoneSkimming,
		// Token: 0x0400174A RID: 5962
		BellyWriggling,
		// Token: 0x0400174B RID: 5963
		InkPose,
		// Token: 0x0400174C RID: 5964
		EnterExitPath,
		// Token: 0x0400174D RID: 5965
		EnterExitDoor,
		// Token: 0x0400174E RID: 5966
		ChairLift,
		// Token: 0x0400174F RID: 5967
		Boat,
		// Token: 0x04001750 RID: 5968
		ZipLine,
		// Token: 0x04001751 RID: 5969
		FinalJump,
		// Token: 0x04001752 RID: 5970
		Exploded
	}

	// Token: 0x020002FD RID: 765
	public enum SkimStoneSubState
	{
		// Token: 0x04001754 RID: 5972
		PickingUpRock,
		// Token: 0x04001755 RID: 5973
		Ready,
		// Token: 0x04001756 RID: 5974
		WindingThrow,
		// Token: 0x04001757 RID: 5975
		Throw,
		// Token: 0x04001758 RID: 5976
		FollowStone,
		// Token: 0x04001759 RID: 5977
		Sink
	}

	// Token: 0x020002FE RID: 766
	[NullableContext(2)]
	[Nullable(0)]
	[Serializable]
	public struct StoneSkimming
	{
		// Token: 0x0400175A RID: 5978
		public Prototype stoneProto;

		// Token: 0x0400175B RID: 5979
		public AudioSource warmUpSource;

		// Token: 0x0400175C RID: 5980
		public AudioSource throwSource;

		// Token: 0x0400175D RID: 5981
		public SpriteRenderer twinkle;

		// Token: 0x0400175E RID: 5982
		public Prototype splashParticlesProto;

		// Token: 0x0400175F RID: 5983
		public Prototype splashAudioSourceProto;
	}

	// Token: 0x020002FF RID: 767
	private struct WallSlide
	{
		// Token: 0x04001760 RID: 5984
		[Nullable(1)]
		public Poly poly;

		// Token: 0x04001761 RID: 5985
		public Vector2 normal;

		// Token: 0x04001762 RID: 5986
		public float speed;

		// Token: 0x04001763 RID: 5987
		public float distanceSinceLastHurt;

		// Token: 0x04001764 RID: 5988
		public Vector2 velocity;

		// Token: 0x04001765 RID: 5989
		public bool startedOnClimbable;
	}

	// Token: 0x02000300 RID: 768
	private struct FallCollision
	{
		// Token: 0x04001766 RID: 5990
		public bool collided;

		// Token: 0x04001767 RID: 5991
		public Vector2 normal;

		// Token: 0x04001768 RID: 5992
		public float collisionDepth;

		// Token: 0x04001769 RID: 5993
		public Slope foundGulleySlope;
	}

	// Token: 0x02000301 RID: 769
	private struct BounceResult
	{
		// Token: 0x0400176A RID: 5994
		public bool shouldBounce;

		// Token: 0x0400176B RID: 5995
		public Vector2 position;

		// Token: 0x0400176C RID: 5996
		public Vector2 velocity;

		// Token: 0x0400176D RID: 5997
		public float speedIntoNormal;
	}
}
