using System;
using System.Runtime.CompilerServices;
using UnityEngine;

// Token: 0x02000150 RID: 336
public class SunClock : MonoBehaviour
{
	// Token: 0x170002B1 RID: 689
	// (get) Token: 0x06000B6C RID: 2924 RVA: 0x0005BA4C File Offset: 0x00059C4C
	private bool shouldBeVisible
	{
		get
		{
			return (Game.instance.inActiveGameplay || Game.instance.isPathFollowing) && !MonoSingleton<JournalController>.instance.visible && (!MonoSingleton<BlackBars>.instance.visible || Game.instance.isPathFollowing) && !MonoSingleton<TitleScreen>.instance.visible && !Blackout.showing && !MonoSingleton<PeakStateController>.instance.active && (!GameClock.instance.isNight || !GameClock.instance.isWaitingForTimeToPass) && !Runner.instance.inFinalJump && !MonoSingleton<Credits>.instance.visible && Level.currentIndex < 8 && !PhotoMode.visible;
		}
	}

	// Token: 0x170002B2 RID: 690
	// (get) Token: 0x06000B6D RID: 2925 RVA: 0x0005BAF9 File Offset: 0x00059CF9
	private SLayout layout
	{
		get
		{
			if (this._layout == null)
			{
				this._layout = base.GetComponent<SLayout>();
			}
			return this._layout;
		}
	}

	// Token: 0x06000B6E RID: 2926 RVA: 0x0005BB1B File Offset: 0x00059D1B
	private void Start()
	{
		this.layout.groupAlpha = 0f;
	}

	// Token: 0x06000B6F RID: 2927 RVA: 0x0005BB30 File Offset: 0x00059D30
	private void Update()
	{
		if (this.layout.targetGroupAlpha == 1f != this.shouldBeVisible)
		{
			if (this.shouldBeVisible)
			{
				if (this.layout.groupAlpha == 0f)
				{
					this.layout.scale = 0.5f;
				}
				this.layout.Animate(0.5f, 0f, SLayout.popCurve, delegate
				{
					this.layout.groupAlpha = 1f;
					this.layout.scale = 1f;
				});
			}
			else
			{
				this.layout.Animate(0.5f, 1f, SLayout.reversePopCurve, delegate
				{
					this.layout.groupAlpha = 0f;
					this.layout.scale = 0.5f;
				});
			}
		}
		if (this.layout.groupAlpha > 0f)
		{
			this.UpdateLayout();
		}
	}

	// Token: 0x06000B70 RID: 2928 RVA: 0x0005BBEC File Offset: 0x00059DEC
	private void UpdateLayout()
	{
		float num = (this._debugOverride ? this._debugOverrideHour : GameClock.instance.hourOfDay);
		float num2 = num / 24f;
		float num3;
		if (num >= 19f)
		{
			num3 = Mathf.InverseLerp(19f, 22f, num);
		}
		else
		{
			num3 = 1f - Mathf.InverseLerp(5f, 7f, num);
		}
		this._background.color = this._settings.backgroundGradient.Evaluate(num3);
		this._lowerHorizon.color = this._settings.belowHorizonGradient.Evaluate(num3);
		float num5;
		if (num >= 5f && num < 22f)
		{
			float num4 = Mathf.InverseLerp(5f, 22f, num);
			num5 = Mathf.Lerp(this._settings.sunriseAngleNorm, this._settings.sunsetAngleNorm, num4) * 2f * 3.1415927f;
		}
		else
		{
			float num6 = num;
			if (num6 < 5f)
			{
				num6 += 24f;
			}
			float num7 = Mathf.InverseLerp(22f, 29f, num6);
			num5 = Mathf.Lerp(this._settings.sunsetAngleNorm, this._settings.sunriseAngleNorm + 1f, num7) * 2f * 3.1415927f;
		}
		this._sun.origin = this._background.middle + this._settings.sunRevolveRadius * new Vector2(Mathf.Sin(num5 + 3.1415927f), Mathf.Cos(num5 + 3.1415927f));
		this._sun.color = this._settings.sunGradient.Evaluate(num3);
		this._horizonLine.color = this._settings.horizonLineGradient.Evaluate(num3);
		float alpha = this._progressRing.alpha;
		this._progressRing.image.fillAmount = 1f - num3;
		Color color = this._settings.progressBarGradient.Evaluate(num3);
		bool flag = num >= 19f && num <= 22f;
		float num8 = (float)(flag ? 1 : 0);
		color.a = Mathf.MoveTowards(alpha, num8, Time.unscaledDeltaTime);
		this._progressRing.color = color;
		this._stars.rotation = -360f * num2;
		this._stars.alpha = this._settings.starsFadeInRange.InverseLerp(num3);
		int num9 = ((num3 > this._settings.flashStart && num3 != 1f) ? 1 : 0);
		this._flashStrength = Mathf.MoveTowards(this._flashStrength, (float)num9, Time.unscaledDeltaTime);
		float num10 = 0.5f * (Mathf.Sin(this._settings.flashSpeed * Time.unscaledTime) + 1f);
		this._outerRing.color = Color.Lerp(Color.black, this._settings.flashColor, this._flashStrength * num10);
		float num11 = this._settings.shadingFadeRange.InverseLerp(num3);
		this._shading.alpha = Mathf.Lerp(1f, this._settings.minShadingAlpha, num11);
		bool flag2 = this._cave.targetAlpha > 0f;
		bool caveShouldBeVisible = CaveRegion.inCave;
		if (flag2 != caveShouldBeVisible)
		{
			this._cave.Animate(0.5f, delegate
			{
				this._cave.alpha = (float)(caveShouldBeVisible ? 1 : 0);
			});
		}
		string text = this.TimeDescriptionAndProgress(num).Item1;
		if ((text != "Nightfall") & caveShouldBeVisible)
		{
			text = "Underground";
		}
		if (this._currentLabel == null || this._currentLabel.textMeshPro.text != text)
		{
			if (this._currentLabel != null)
			{
				SLayout oldLabel = this._currentLabel;
				oldLabel.Animate(1f, delegate
				{
					oldLabel.alpha = 0f;
				}).Then(delegate
				{
					oldLabel.GetComponent<Prototype>().ReturnToPool();
				});
				this._currentLabel = null;
			}
			if (!string.IsNullOrEmpty(text))
			{
				SLayout newLabel = this._labelProto.Instantiate<SLayout>(null);
				newLabel.textMeshPro.text = text;
				if (this._defaultLabelColor.a == 0f)
				{
					this._defaultLabelColor = newLabel.color;
				}
				Color targetColor = (flag ? this._settings.nightfallLabelColor : this._defaultLabelColor);
				newLabel.color = targetColor.WithAlpha(0f);
				newLabel.Animate(1f, delegate
				{
					newLabel.alpha = targetColor.a;
				});
				this._currentLabel = newLabel;
			}
		}
		bool flag3 = GameClock.instance.isWaitingForTimeToPass || MonoSingleton<RestStateController>.instance.resting || this._debugOverrideShowRing;
		float num12 = (float)(flag3 ? 1 : 0);
		float num13 = (flag3 ? 0.3f : 1f);
		this._dottedRing.alpha = Mathf.MoveTowards(this._dottedRing.alpha, num12, Time.unscaledDeltaTime / num13);
		float num14 = 0f;
		if (flag3 || GameClock.instance.isWaitingForTimeToPass)
		{
			num14 = (GameClock.instance.isWaitingForTimeToPass ? this._settings.timeLapseRingSpeed : this._settings.restRingSpeed);
		}
		this._ringSpeed = Mathf.MoveTowards(this._ringSpeed, num14, this._settings.ringAccel * Time.unscaledDeltaTime);
		this._dottedRing.rotation += -this._ringSpeed * Time.unscaledDeltaTime;
	}

	// Token: 0x06000B71 RID: 2929 RVA: 0x0005C1B4 File Offset: 0x0005A3B4
	[return: TupleElementNames(new string[] { "timeDesc", "timePeriodProgress" })]
	private ValueTuple<string, float> TimeDescriptionAndProgress(float hourOfDay)
	{
		foreach (SunClock.TimePeriod timePeriod in this._timePeriods)
		{
			bool flag = false;
			if (timePeriod.start > timePeriod.end && (hourOfDay >= timePeriod.start || hourOfDay < timePeriod.end))
			{
				flag = true;
			}
			else if (timePeriod.start < timePeriod.end && hourOfDay >= timePeriod.start && hourOfDay < timePeriod.end)
			{
				flag = true;
			}
			if (flag)
			{
				float start = timePeriod.start;
				float num = timePeriod.end;
				if (start > num)
				{
					num += 24f;
					if (hourOfDay < start)
					{
						hourOfDay += 24f;
					}
				}
				return new ValueTuple<string, float>(timePeriod.name, Mathf.InverseLerp(start, num, hourOfDay));
			}
		}
		Debug.LogError("Failed to find time period for hour of day: " + hourOfDay.ToString());
		return new ValueTuple<string, float>("Error: Unknown time period", 0f);
	}

	// Token: 0x04000D84 RID: 3460
	private const float lateStartHour = 19f;

	// Token: 0x04000D85 RID: 3461
	private const float lateEndHour = 22f;

	// Token: 0x04000D86 RID: 3462
	private const float dawnStartHour = 5f;

	// Token: 0x04000D87 RID: 3463
	private const float dawnEndHour = 7f;

	// Token: 0x04000D88 RID: 3464
	private SLayout _layout;

	// Token: 0x04000D89 RID: 3465
	private SunClock.TimePeriod[] _timePeriods = new SunClock.TimePeriod[]
	{
		new SunClock.TimePeriod
		{
			name = "Dawn",
			start = 6f,
			end = 7f
		},
		new SunClock.TimePeriod
		{
			name = "Early Morning",
			start = 7f,
			end = 9f
		},
		new SunClock.TimePeriod
		{
			name = "Morning",
			start = 9f,
			end = 11f
		},
		new SunClock.TimePeriod
		{
			name = "Midday",
			start = 11f,
			end = 13f
		},
		new SunClock.TimePeriod
		{
			name = "Early Afternoon",
			start = 13f,
			end = 15f
		},
		new SunClock.TimePeriod
		{
			name = "Late Afternoon",
			start = 15f,
			end = 17f
		},
		new SunClock.TimePeriod
		{
			name = "Early Evening",
			start = 17f,
			end = 19f
		},
		new SunClock.TimePeriod
		{
			name = "Nightfall",
			start = 19f,
			end = 22f
		},
		new SunClock.TimePeriod
		{
			name = "Night",
			start = 22f,
			end = 6f
		}
	};

	// Token: 0x04000D8A RID: 3466
	private int _currentTimePeriodIdx;

	// Token: 0x04000D8B RID: 3467
	private SLayout _currentLabel;

	// Token: 0x04000D8C RID: 3468
	[SerializeField]
	private SLayout _background;

	// Token: 0x04000D8D RID: 3469
	[SerializeField]
	private SLayout _lowerHorizon;

	// Token: 0x04000D8E RID: 3470
	[SerializeField]
	private SLayout _horizonLine;

	// Token: 0x04000D8F RID: 3471
	[SerializeField]
	private SLayout _sun;

	// Token: 0x04000D90 RID: 3472
	[SerializeField]
	private SLayout _stars;

	// Token: 0x04000D91 RID: 3473
	[SerializeField]
	private SLayout _progressRing;

	// Token: 0x04000D92 RID: 3474
	[SerializeField]
	private SLayout _outerRing;

	// Token: 0x04000D93 RID: 3475
	[SerializeField]
	private SLayout _shading;

	// Token: 0x04000D94 RID: 3476
	[SerializeField]
	private Prototype _labelProto;

	// Token: 0x04000D95 RID: 3477
	[SerializeField]
	private SLayout _dottedRing;

	// Token: 0x04000D96 RID: 3478
	[SerializeField]
	private SLayout _cave;

	// Token: 0x04000D97 RID: 3479
	[SerializeField]
	private SunClockSettings _settings;

	// Token: 0x04000D98 RID: 3480
	[SerializeField]
	private bool _debugOverride;

	// Token: 0x04000D99 RID: 3481
	[Range(0f, 24f)]
	[SerializeField]
	private float _debugOverrideHour;

	// Token: 0x04000D9A RID: 3482
	[SerializeField]
	private bool _debugOverrideShowRing;

	// Token: 0x04000D9B RID: 3483
	private float _flashStrength;

	// Token: 0x04000D9C RID: 3484
	private Color _defaultLabelColor;

	// Token: 0x04000D9D RID: 3485
	private float _ringSpeed;

	// Token: 0x02000386 RID: 902
	public struct TimePeriod
	{
		// Token: 0x04001928 RID: 6440
		public string name;

		// Token: 0x04001929 RID: 6441
		public float start;

		// Token: 0x0400192A RID: 6442
		public float end;
	}
}
