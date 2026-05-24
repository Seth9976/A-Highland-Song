using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000029 RID: 41
public class GameClock : MonoBehaviour
{
	// Token: 0x17000047 RID: 71
	// (get) Token: 0x0600010E RID: 270 RVA: 0x0000BFBD File Offset: 0x0000A1BD
	public static GameClock instance
	{
		get
		{
			return GSR.Clock;
		}
	}

	// Token: 0x17000048 RID: 72
	// (get) Token: 0x0600010F RID: 271 RVA: 0x0000BFC4 File Offset: 0x0000A1C4
	public float minuteOfDay
	{
		get
		{
			return this.hourOfDay % 1f * 60f;
		}
	}

	// Token: 0x17000049 RID: 73
	// (get) Token: 0x06000110 RID: 272 RVA: 0x0000BFD8 File Offset: 0x0000A1D8
	// (set) Token: 0x06000111 RID: 273 RVA: 0x0000BFE8 File Offset: 0x0000A1E8
	public float daysNorm
	{
		get
		{
			return (float)this.dayIdx + this.timeOfDayNorm;
		}
		set
		{
			this.dayIdx = Mathf.FloorToInt(value);
			this.timeOfDayNorm = value - (float)this.dayIdx;
		}
	}

	// Token: 0x1700004A RID: 74
	// (get) Token: 0x06000112 RID: 274 RVA: 0x0000C005 File Offset: 0x0000A205
	// (set) Token: 0x06000113 RID: 275 RVA: 0x0000C01D File Offset: 0x0000A21D
	public float timeOfDayNorm
	{
		get
		{
			return Mathf.Repeat(this.hourOfDay, 24f) / 24f;
		}
		set
		{
			this.hourOfDay = Mathf.Clamp01(value) * 24f;
		}
	}

	// Token: 0x1700004B RID: 75
	// (get) Token: 0x06000114 RID: 276 RVA: 0x0000C031 File Offset: 0x0000A231
	public int dayNumber
	{
		get
		{
			return GameClock.instance.dayIdx + 1;
		}
	}

	// Token: 0x1700004C RID: 76
	// (get) Token: 0x06000115 RID: 277 RVA: 0x0000C03F File Offset: 0x0000A23F
	public bool isMorning
	{
		get
		{
			return this.hourOfDay >= 6f && this.hourOfDay < 9f;
		}
	}

	// Token: 0x1700004D RID: 77
	// (get) Token: 0x06000116 RID: 278 RVA: 0x0000C05D File Offset: 0x0000A25D
	public bool isNight
	{
		get
		{
			return this.hourOfDay >= 22f || this.hourOfDay < 6f;
		}
	}

	// Token: 0x1700004E RID: 78
	// (get) Token: 0x06000117 RID: 279 RVA: 0x0000C07B File Offset: 0x0000A27B
	public bool isLate
	{
		get
		{
			return this.hourOfDay >= 19f || this.hourOfDay < 6f;
		}
	}

	// Token: 0x1700004F RID: 79
	// (get) Token: 0x06000118 RID: 280 RVA: 0x0000C099 File Offset: 0x0000A299
	public bool isWaitingForTimeToPass
	{
		get
		{
			return this._targetWaitEndDaysNorm > 0f && this.daysNorm < this._targetWaitEndDaysNorm;
		}
	}

	// Token: 0x17000050 RID: 80
	// (get) Token: 0x06000119 RID: 281 RVA: 0x0000C0B8 File Offset: 0x0000A2B8
	private float standardTimePassSpeed_daysPerRealSec
	{
		get
		{
			return 1f / (1440f * this.settings.gameRealSecondsPerGameMinute);
		}
	}

	// Token: 0x17000051 RID: 81
	// (get) Token: 0x0600011A RID: 282 RVA: 0x0000C0D1 File Offset: 0x0000A2D1
	private float engagedInNarrativeContentTimePassSpeed_daysPerRealSec
	{
		get
		{
			return 1f / (1440f * this.settings.gameRealSecondsPerGameMinuteWhileNarrating);
		}
	}

	// Token: 0x0600011B RID: 283 RVA: 0x0000C0EA File Offset: 0x0000A2EA
	public void SetDayAndHourWithoutTimePassing(int dayIdx, float hourOfDay)
	{
		this.dayIdx = dayIdx;
		this.hourOfDay = hourOfDay;
		this.ResetPrevDaysForTimeDelta();
	}

	// Token: 0x0600011C RID: 284 RVA: 0x0000C100 File Offset: 0x0000A300
	private void Start()
	{
		this._timePassSpeed_daysPerRealSec = this.standardTimePassSpeed_daysPerRealSec;
		this.ResetPrevDaysForTimeDelta();
	}

	// Token: 0x0600011D RID: 285 RVA: 0x0000C114 File Offset: 0x0000A314
	private void ResetPrevDaysForTimeDelta()
	{
		this._prevDaysNorm = this.daysNorm;
	}

	// Token: 0x0600011E RID: 286 RVA: 0x0000C122 File Offset: 0x0000A322
	public IEnumerator WaitUntilDaysNorm(float targetDaysNorm)
	{
		this._targetWaitEndDaysNorm = targetDaysNorm;
		this._waitStartDaysNorm = this.daysNorm;
		this._waitIsCancelled = false;
		while (this.daysNorm < targetDaysNorm && !this._waitIsCancelled)
		{
			yield return null;
		}
		this._targetWaitEndDaysNorm = -1f;
		this._waitStartDaysNorm = -1f;
		this._waitIsCancelled = false;
		if (!Narrative.instance.isBusy && !Game.instance.showingIntro)
		{
			Narrative.instance.RefreshInteractablesChoices(false, false);
		}
		yield break;
	}

	// Token: 0x0600011F RID: 287 RVA: 0x0000C138 File Offset: 0x0000A338
	public void CancelTimePassing()
	{
		this._waitIsCancelled = true;
	}

	// Token: 0x06000120 RID: 288 RVA: 0x0000C144 File Offset: 0x0000A344
	private void Update()
	{
		if (!Game.loaded)
		{
			return;
		}
		int num = 24 * this.inkUpdatesPerHour;
		int num2 = Mathf.FloorToInt(this.timeOfDayNorm * (float)num);
		float standardTimePassSpeed_daysPerRealSec = this.standardTimePassSpeed_daysPerRealSec;
		float num3 = 1f / this.settings.restingRealSecondsPerGameHour / 24f;
		bool flag = false;
		if (this.isWaitingForTimeToPass)
		{
			float num4 = 1f / this.settings.shelterRealSecondsPerGameHour / 24f;
			float num5 = 1f;
			if (MonoSingleton<BuildSetupManager>.instance.setup.fastIntro && Game.instance.inTitleScreenAndIntroState)
			{
				num5 = 2f;
				num4 *= num5;
			}
			float num6 = num5 * (num4 - standardTimePassSpeed_daysPerRealSec) / this.settings.shelterTimeAccelEndTime;
			float num7 = (this._timePassSpeed_daysPerRealSec * this._timePassSpeed_daysPerRealSec - standardTimePassSpeed_daysPerRealSec * standardTimePassSpeed_daysPerRealSec) / (2f * num6);
			float num8 = this._targetWaitEndDaysNorm - num7;
			float num9 = standardTimePassSpeed_daysPerRealSec * 0.033333335f;
			if (this.daysNorm >= num8 - num9)
			{
				this._timePassSpeed_daysPerRealSec = Mathf.MoveTowards(this._timePassSpeed_daysPerRealSec, standardTimePassSpeed_daysPerRealSec, num6 * Time.deltaTime);
			}
			else
			{
				float num10 = num5 * (num4 - standardTimePassSpeed_daysPerRealSec) / this.settings.shelterTimeAccelStartTime;
				this._timePassSpeed_daysPerRealSec = Mathf.MoveTowards(this._timePassSpeed_daysPerRealSec, num4, num10 * Time.deltaTime);
			}
		}
		else if (Runner.instance != null && MonoSingleton<RestStateController>.instance.resting)
		{
			float num11 = (num3 - standardTimePassSpeed_daysPerRealSec) / this.settings.restAccelRealDuration;
			this._timePassSpeed_daysPerRealSec = Mathf.MoveTowards(this._timePassSpeed_daysPerRealSec, num3, num11 * Time.deltaTime);
		}
		else if ((Narrative.instance.isBusy && !MonoSingleton<RestStateController>.instance.active) || Game.instance.inPeakState)
		{
			flag = true;
		}
		else
		{
			float num12 = (num3 - standardTimePassSpeed_daysPerRealSec) / this.settings.restDecelRealDuration;
			this._timePassSpeed_daysPerRealSec = Mathf.MoveTowards(this._timePassSpeed_daysPerRealSec, standardTimePassSpeed_daysPerRealSec, num12 * Time.deltaTime);
		}
		this.normalSpeedMultiplier = this._timePassSpeed_daysPerRealSec / this.standardTimePassSpeed_daysPerRealSec;
		GameClock.dayNormDeltaTime = (flag ? this.engagedInNarrativeContentTimePassSpeed_daysPerRealSec : this._timePassSpeed_daysPerRealSec) * Time.deltaTime;
		GameClock.hourDeltaTime = GameClock.dayNormDeltaTime * 24f;
		GameClock.minuteDeltaTime = GameClock.dayNormDeltaTime * 24f * 60f;
		float num13 = GameClock.dayNormDeltaTime;
		float num14 = this.daysNorm + GameClock.dayNormDeltaTime;
		float num15 = float.MaxValue;
		if (this.firstDayPauseEnabled && Level.currentIndex == 0 && this.dayIdx == 0 && !this.isWaitingForTimeToPass)
		{
			num15 = 0f + this.firstDayPauseHour / 24f;
		}
		else if ((Level.currentIndex == 8 || (Level.currentIndex == 7 && Runner.instance.position.x > 900f)) && !this.isWaitingForTimeToPass)
		{
			num15 = (float)this.dayIdx + this.lastLevelPauseHour / 24f;
		}
		if ((Game.gameplayPaused || Runner.instance.isMusicRunning || num14 > num15) && !this.isWaitingForTimeToPass)
		{
			num14 = this.daysNorm;
			GameClock.dayNormDeltaTime = 0f;
		}
		float num16 = 0f;
		if (CaveRegion.inCave && !this.isWaitingForTimeToPass)
		{
			num16 = GameClock.dayNormDeltaTime;
			num14 = this.daysNorm;
			GameClock.dayNormDeltaTime = 0f;
		}
		this.daysNorm = num14;
		if (GameClock.onTimeDidPass != null && GameClock.dayNormDeltaTime > 0f)
		{
			GameClock.onTimeDidPass(GameClock.dayNormDeltaTime);
		}
		else if (GameClock.onCaveTimeDidNotPass != null && num16 > 0f)
		{
			GameClock.onCaveTimeDidNotPass(num16);
		}
		if (GameClock.onVisualEffectTimeDidPass != null)
		{
			float num17 = this.settings.deltaRangeForVisualEffects.InverseLerp(10000f * num13);
			num17 = Mathf.Pow(num17, this.settings.timeSpeedNormPowerForVisualEffects);
			float num18 = this.settings.visualEffectsAnimSpeedRange.Lerp(num17);
			GameClock.onVisualEffectTimeDidPass(num18);
		}
		int num19 = Mathf.FloorToInt(this.timeOfDayNorm * (float)num);
		if (num2 != num19 && GameClock.onInkIntervalDidPass != null)
		{
			GameClock.onInkIntervalDidPass();
		}
		this.ResetPrevDaysForTimeDelta();
	}

	// Token: 0x06000121 RID: 289 RVA: 0x0000C53B File Offset: 0x0000A73B
	private void OnValidate()
	{
		this.hourOfDay = Mathf.Repeat(this.hourOfDay, 24f);
	}

	// Token: 0x040001BD RID: 445
	public static Action onInkIntervalDidPass;

	// Token: 0x040001BE RID: 446
	public static Action<float> onTimeDidPass;

	// Token: 0x040001BF RID: 447
	public static Action<float> onCaveTimeDidNotPass;

	// Token: 0x040001C0 RID: 448
	public static Action<float> onVisualEffectTimeDidPass;

	// Token: 0x040001C1 RID: 449
	public float hourOfDay = 12f;

	// Token: 0x040001C2 RID: 450
	public int dayIdx;

	// Token: 0x040001C3 RID: 451
	public int inkUpdatesPerHour = 2;

	// Token: 0x040001C4 RID: 452
	public float firstDayPauseHour = 18f;

	// Token: 0x040001C5 RID: 453
	public float lastLevelPauseHour = 20f;

	// Token: 0x040001C6 RID: 454
	public bool firstDayPauseEnabled = true;

	// Token: 0x040001C7 RID: 455
	public GameClockSettings settings;

	// Token: 0x040001C8 RID: 456
	private float _prevDaysNorm;

	// Token: 0x040001C9 RID: 457
	public const float dawnHour = 6f;

	// Token: 0x040001CA RID: 458
	public const float lateHour = 19f;

	// Token: 0x040001CB RID: 459
	public const float duskHour = 22f;

	// Token: 0x040001CC RID: 460
	[SerializeField]
	[Disable]
	private float _dayNormDeltaTime;

	// Token: 0x040001CD RID: 461
	public static float dayNormDeltaTime;

	// Token: 0x040001CE RID: 462
	public static float hourDeltaTime;

	// Token: 0x040001CF RID: 463
	public static float minuteDeltaTime;

	// Token: 0x040001D0 RID: 464
	public float normalSpeedMultiplier;

	// Token: 0x040001D1 RID: 465
	private float _timePassSpeed_daysPerRealSec;

	// Token: 0x040001D2 RID: 466
	private float _waitStartDaysNorm = -1f;

	// Token: 0x040001D3 RID: 467
	private float _targetWaitEndDaysNorm = -1f;

	// Token: 0x040001D4 RID: 468
	private bool _waitIsCancelled;
}
