using System;
using UnityEngine;

// Token: 0x0200003A RID: 58
public class RestStateController : MonoSingleton<RestStateController>
{
	// Token: 0x170000A4 RID: 164
	// (get) Token: 0x060001D4 RID: 468 RVA: 0x00010E75 File Offset: 0x0000F075
	public bool active
	{
		get
		{
			return this.state > RestStateController.State.None;
		}
	}

	// Token: 0x170000A5 RID: 165
	// (get) Token: 0x060001D5 RID: 469 RVA: 0x00010E80 File Offset: 0x0000F080
	public bool sitting
	{
		get
		{
			return this.state == RestStateController.State.Sitting;
		}
	}

	// Token: 0x170000A6 RID: 166
	// (get) Token: 0x060001D6 RID: 470 RVA: 0x00010E8B File Offset: 0x0000F08B
	public bool resting
	{
		get
		{
			return this.state == RestStateController.State.Resting;
		}
	}

	// Token: 0x170000A7 RID: 167
	// (get) Token: 0x060001D7 RID: 471 RVA: 0x00010E96 File Offset: 0x0000F096
	public bool restingOrSitting
	{
		get
		{
			return this.state == RestStateController.State.Resting || this.state == RestStateController.State.Sitting;
		}
	}

	// Token: 0x170000A8 RID: 168
	// (get) Token: 0x060001D8 RID: 472 RVA: 0x00010EAC File Offset: 0x0000F0AC
	public bool sleeping
	{
		get
		{
			return this.state == RestStateController.State.Sleeping;
		}
	}

	// Token: 0x170000A9 RID: 169
	// (get) Token: 0x060001D9 RID: 473 RVA: 0x00010EB7 File Offset: 0x0000F0B7
	public bool sleepPending
	{
		get
		{
			return this._sleepPending;
		}
	}

	// Token: 0x170000AA RID: 170
	// (get) Token: 0x060001DA RID: 474 RVA: 0x00010EBF File Offset: 0x0000F0BF
	public bool wantsSleepNotRest
	{
		get
		{
			return GameClock.instance.isLate;
		}
	}

	// Token: 0x170000AB RID: 171
	// (get) Token: 0x060001DB RID: 475 RVA: 0x00010ECC File Offset: 0x0000F0CC
	public static RestAvailability restAvailability
	{
		get
		{
			Runner instance = Runner.instance;
			if (MonoSingleton<RestStateController>.instance.state == RestStateController.State.Sleeping)
			{
				return RestAvailability.FullyAvailable;
			}
			if (instance.health.currentHealth <= 0f)
			{
				return RestAvailability.None;
			}
			if (!instance.gameObject.activeInHierarchy)
			{
				return RestAvailability.None;
			}
			if (MonoSingleton<BlackBars>.instance.visible)
			{
				return RestAvailability.None;
			}
			if (!Game.instance.inActiveGameplay)
			{
				return RestAvailability.None;
			}
			if (instance.isMusicRunning)
			{
				return RestAvailability.None;
			}
			if (AudioController.instance.playingFinalJumpMusic)
			{
				return RestAvailability.None;
			}
			if (FinalJumpZone.activeZone != null || instance.inFinalJumpAndLeftLand)
			{
				return RestAvailability.None;
			}
			if (GameCamera.instance.playerZoomState.zooming || MonoSingleton<MapsViewController>.instance.isBusy)
			{
				return RestAvailability.None;
			}
			if ((PropsController.instance.triggerFlags & TriggerFlags.DisableSitting) > TriggerFlags.None)
			{
				return RestAvailability.None;
			}
			if (MonoSingleton<JournalController>.instance.visible)
			{
				return RestAvailability.None;
			}
			if (MonoSingleton<TitleScreen>.instance.visible)
			{
				return RestAvailability.None;
			}
			if (instance.transform.position.y <= 0f)
			{
				return RestAvailability.None;
			}
			if (instance.stoneSkimming)
			{
				return RestAvailability.None;
			}
			if (PhotoMode.visible)
			{
				return RestAvailability.None;
			}
			if (!instance.running && !instance.sitting && !instance.jumping)
			{
				return RestAvailability.PromptDisabled;
			}
			if (instance.playerControlDisabled != PlayerControlDisableReason.None)
			{
				return RestAvailability.PromptDisabled;
			}
			if (NarrativePresenter.instance.showingNarrativeChoiceWidget && !NarrativePresenter.hasRestChoice)
			{
				return RestAvailability.PromptDisabled;
			}
			if (PropsController.instance.inRestPreventionZone)
			{
				return RestAvailability.PromptDisabled;
			}
			if (instance.jumping)
			{
				return RestAvailability.PromptEnabled;
			}
			return RestAvailability.FullyAvailable;
		}
	}

	// Token: 0x060001DC RID: 476 RVA: 0x00011026 File Offset: 0x0000F226
	private void Start()
	{
		GameClock.onTimeDidPass = (Action<float>)Delegate.Combine(GameClock.onTimeDidPass, new Action<float>(this.OnTimeDidPass));
		this._prevWantsSleepNotRest = this.wantsSleepNotRest;
	}

	// Token: 0x060001DD RID: 477 RVA: 0x00011054 File Offset: 0x0000F254
	protected override void OnDestroy()
	{
		base.OnDestroy();
		GameClock.onTimeDidPass = (Action<float>)Delegate.Remove(GameClock.onTimeDidPass, new Action<float>(this.OnTimeDidPass));
	}

	// Token: 0x060001DE RID: 478 RVA: 0x0001107C File Offset: 0x0000F27C
	private void Update()
	{
		Runner instance = Runner.instance;
		if (this.active && (NarrativePresenter.instance.showingNarrativeChoiceWidget || RestStateController.restAvailability == RestAvailability.None) && this.state != RestStateController.State.HeaderOnlyDuringWait && this.state != RestStateController.State.Sleeping && GameInput.HasControl(this))
		{
			this.Exit();
			return;
		}
		if (GameInput.restPressed && RestStateController.restAvailability == RestAvailability.FullyAvailable && !this.active)
		{
			MonoSingleton<GameInput>.instance.mapping.rest.ClearInputState();
			this.Enter();
			return;
		}
		if (GameInput.HasControl(this) && (this.state == RestStateController.State.Sitting || this.state == RestStateController.State.Resting) && !this._sleepPending && (GameInput.Back(this) || GameInput.restPressed || (GameInput.moveLeftRight != 0f && Runner.instance.playerControlDisabled == PlayerControlDisableReason.None)))
		{
			MonoSingleton<GameInput>.instance.mapping.back.ClearInputState();
			this.Exit();
			return;
		}
		if (this.active && this._sleepPending && !Narrative.instance.isBusy)
		{
			this._sleepPending = false;
			this.TryChangeState(RestStateController.State.Sleeping);
		}
		if (this.active && !this._sitDownInkOpportunityConsumed)
		{
			float num = Time.time - this._sitStartTime;
			bool flag = this.state == RestStateController.State.Sitting && !Narrative.instance.isBusy;
			if (num > 1f || this.state != RestStateController.State.Sitting)
			{
				this._sitDownInkOpportunityConsumed = true;
			}
			else if (flag)
			{
				this._sitDownInkOpportunityConsumed = true;
				Narrative instance2 = Narrative.instance;
				Prop activeRestProp = PropsController.instance.activeRestProp;
				instance2.SitDown((activeRestProp != null) ? activeRestProp.inkListItemName : null);
			}
		}
		if (this.active && this.state == RestStateController.State.Sitting && GameInput.selectMenuItem)
		{
			if (this.wantsSleepNotRest)
			{
				this.TryChangeState(RestStateController.State.Sleeping);
			}
			else
			{
				this.TryChangeState(RestStateController.State.Resting);
			}
		}
		else if (this.state == RestStateController.State.Resting && !GameInput.selectMenuitemHeld)
		{
			this.state = RestStateController.State.Sitting;
		}
		if (!this._restEndInkOpportunityConsumed && this._firstRestStartDaysNorm > 0f && GameClock.instance.daysNorm > this._firstRestStartDaysNorm + 0.083333336f)
		{
			this._restEndInkOpportunityConsumed = true;
			Narrative instance3 = Narrative.instance;
			Prop activeRestProp2 = PropsController.instance.activeRestProp;
			instance3.MidRest((activeRestProp2 != null) ? activeRestProp2.inkListItemName : null);
		}
		if (this.state == RestStateController.State.Resting && GameClock.instance.firstDayPauseEnabled && GameClock.instance.hourOfDay > GameClock.instance.firstDayPauseHour - 0.1f)
		{
			GameClock.instance.firstDayPauseEnabled = false;
		}
		if (this.state == RestStateController.State.Resting && Time.time - this._restStartRealTime > 0.5f && !this._restStartInkOpportunityConsumed && !Narrative.instance.isBusy)
		{
			this._restStartInkOpportunityConsumed = true;
			Narrative instance4 = Narrative.instance;
			Prop activeRestProp3 = PropsController.instance.activeRestProp;
			instance4.StartRest((activeRestProp3 != null) ? activeRestProp3.inkListItemName : null);
		}
		if (this.state == RestStateController.State.Sleeping && !Narrative.instance.isBusy)
		{
			this.Exit();
		}
	}

	// Token: 0x060001DF RID: 479 RVA: 0x00011358 File Offset: 0x0000F558
	public void Enter()
	{
		if (this.state != RestStateController.State.None)
		{
			Debug.LogError("RestStateController.Enter() called when already in state " + this.state.ToString());
			return;
		}
		if (RestStateController.restAvailability != RestAvailability.FullyAvailable && !Runner.instance.hasInkPose)
		{
			Debug.LogError("RestStateController.Enter() called when restAvailability wasn't RestAvailability.FullyAvailable but " + RestStateController.restAvailability.ToString());
		}
		if (GameClock.instance.daysNorm > this._lastRestEndDaysNorm + 0.041666668f)
		{
			this._restStartInkOpportunityConsumed = false;
			this._restEndInkOpportunityConsumed = false;
		}
		this._firstRestStartDaysNorm = -1f;
		this._sitDownInkOpportunityConsumed = false;
		this._sitStartTime = Time.time;
		this._sleepPending = false;
		Runner.instance.StartSitting();
		this._prevWantsSleepNotRest = this.wantsSleepNotRest;
		GameInput.PushControlStack(this);
		this.state = RestStateController.State.Sitting;
	}

	// Token: 0x060001E0 RID: 480 RVA: 0x00011431 File Offset: 0x0000F631
	public void Clear()
	{
		this.state = RestStateController.State.None;
		this._restStartInkOpportunityConsumed = false;
		this._restEndInkOpportunityConsumed = false;
		this._sleepPending = false;
		GameInput.RemoveControlStackItemEvenIfNotOnTop(this);
	}

	// Token: 0x060001E1 RID: 481 RVA: 0x00011458 File Offset: 0x0000F658
	public void Exit()
	{
		if (Runner.instance.sitting)
		{
			Runner.instance.StopSitting();
		}
		if (this._restStartInkOpportunityConsumed)
		{
			this._restEndInkOpportunityConsumed = true;
		}
		GameInput.PopControlStack(this, true);
		this.state = RestStateController.State.None;
		this._sleepPending = false;
		if (!Narrative.instance.isBusy)
		{
			Narrative.instance.RefreshInteractablesChoices(false, false);
		}
	}

	// Token: 0x060001E2 RID: 482 RVA: 0x000114B8 File Offset: 0x0000F6B8
	private void TryChangeState(RestStateController.State targetState)
	{
		if (targetState == RestStateController.State.Resting && !this.wantsSleepNotRest && this.state == RestStateController.State.Sitting)
		{
			this._restStartRealTime = Time.time;
			if (this._firstRestStartDaysNorm < 0f)
			{
				this._firstRestStartDaysNorm = GameClock.instance.daysNorm;
			}
			this.state = RestStateController.State.Resting;
			return;
		}
		if (targetState != RestStateController.State.Sleeping || this.state != RestStateController.State.Sitting)
		{
			if (targetState == RestStateController.State.None)
			{
				this.Exit();
			}
			return;
		}
		if (Narrative.instance.isBusy)
		{
			this._sleepPending = true;
			return;
		}
		this.state = RestStateController.State.Sleeping;
		GameClock.instance.firstDayPauseEnabled = false;
		if (this._restStartInkOpportunityConsumed)
		{
			this._restEndInkOpportunityConsumed = true;
		}
		Narrative instance = Narrative.instance;
		Prop activeRestProp = PropsController.instance.activeRestProp;
		instance.Sleep((activeRestProp != null) ? activeRestProp.inkListItemName : null);
	}

	// Token: 0x060001E3 RID: 483 RVA: 0x00011578 File Offset: 0x0000F778
	private void OnTimeDidPass(float timeDelta)
	{
		bool wantsSleepNotRest = this.wantsSleepNotRest;
		if (this._prevWantsSleepNotRest != wantsSleepNotRest)
		{
			if (this.state == RestStateController.State.Resting && wantsSleepNotRest)
			{
				this.state = RestStateController.State.Sitting;
				if (!this._restEndInkOpportunityConsumed)
				{
					this._restEndInkOpportunityConsumed = true;
					Narrative.instance.NightFallsWhileResting();
				}
			}
			this._prevWantsSleepNotRest = wantsSleepNotRest;
		}
	}

	// Token: 0x04000297 RID: 663
	[Disable]
	private RestStateController.State state;

	// Token: 0x04000298 RID: 664
	private bool _prevWantsSleepNotRest;

	// Token: 0x04000299 RID: 665
	private bool _restStartInkOpportunityConsumed;

	// Token: 0x0400029A RID: 666
	private bool _restEndInkOpportunityConsumed;

	// Token: 0x0400029B RID: 667
	private float _lastRestEndDaysNorm;

	// Token: 0x0400029C RID: 668
	private float _restStartRealTime;

	// Token: 0x0400029D RID: 669
	private float _firstRestStartDaysNorm;

	// Token: 0x0400029E RID: 670
	private bool _sitDownInkOpportunityConsumed;

	// Token: 0x0400029F RID: 671
	private float _sitStartTime;

	// Token: 0x040002A0 RID: 672
	private bool _sleepPending;

	// Token: 0x02000274 RID: 628
	public enum State
	{
		// Token: 0x040014BE RID: 5310
		None,
		// Token: 0x040014BF RID: 5311
		Sitting,
		// Token: 0x040014C0 RID: 5312
		Resting,
		// Token: 0x040014C1 RID: 5313
		Sleeping,
		// Token: 0x040014C2 RID: 5314
		HeaderOnlyDuringWait
	}
}
