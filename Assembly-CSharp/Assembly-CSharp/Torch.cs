using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000E0 RID: 224
[RequireComponent(typeof(AudioSource))]
public class Torch : MonoSingleton<Torch>
{
	// Token: 0x170001FA RID: 506
	// (get) Token: 0x06000786 RID: 1926 RVA: 0x000445D4 File Offset: 0x000427D4
	public bool shouldBeOn
	{
		get
		{
			return (this.hasTorch || this.forceAlwaysHaveTorchWithFullBattery) && (GameClock.instance.isLate || CaveRegion.inCave) && !this.runner.hidden && !MonoSingleton<RestStateController>.instance.sleeping && this.batteryLevel > 0f && (Game.instance.inActiveGameplay || Game.instance.isPathFollowing) && !this.runner.inFinalJump;
		}
	}

	// Token: 0x06000787 RID: 1927 RVA: 0x00044652 File Offset: 0x00042852
	public void SetAlpha(float alpha)
	{
		this._overallAlpha = alpha;
	}

	// Token: 0x06000788 RID: 1928 RVA: 0x0004465B File Offset: 0x0004285B
	public void ResetForGameStart()
	{
		this.batteryLevel = this._settings.gameStartIntialBatteryHours / this._settings.batteryHours;
		this.hasTorch = true;
	}

	// Token: 0x170001FB RID: 507
	// (get) Token: 0x06000789 RID: 1929 RVA: 0x00044681 File Offset: 0x00042881
	private AudioSource audioSource
	{
		get
		{
			if (this._audioSource == null)
			{
				this._audioSource = base.GetComponentInChildren<AudioSource>();
			}
			return this._audioSource;
		}
	}

	// Token: 0x170001FC RID: 508
	// (get) Token: 0x0600078A RID: 1930 RVA: 0x000446A3 File Offset: 0x000428A3
	private Runner runner
	{
		get
		{
			if (this._runner == null)
			{
				this._runner = base.GetComponentInParent<Runner>();
			}
			return this._runner;
		}
	}

	// Token: 0x0600078B RID: 1931 RVA: 0x000446C8 File Offset: 0x000428C8
	private void SetSpritesActive(bool active)
	{
		foreach (Torch.SpriteWithAlpha spriteWithAlpha in this._sprites)
		{
			spriteWithAlpha.sprite.gameObject.SetActive(active);
		}
	}

	// Token: 0x0600078C RID: 1932 RVA: 0x00044724 File Offset: 0x00042924
	private void Start()
	{
		this.SetSpritesActive(false);
	}

	// Token: 0x0600078D RID: 1933 RVA: 0x0004472D File Offset: 0x0004292D
	private void OnDisable()
	{
		GameClock.onTimeDidPass = (Action<float>)Delegate.Remove(GameClock.onTimeDidPass, new Action<float>(this.OnTimeDidPass));
		this.SetSpritesActive(false);
	}

	// Token: 0x0600078E RID: 1934 RVA: 0x00044758 File Offset: 0x00042958
	private void Update()
	{
		if (this.forceAlwaysHaveTorchWithFullBattery)
		{
			this.batteryLevel = 1f;
		}
		if (this._sprites[0].sprite.gameObject.activeSelf != this.shouldBeOn)
		{
			this.SetSpritesActive(this.shouldBeOn);
			if (!Runner.instance.dead && !Blackout.isFullyVisible && Game.instance.inActiveGameplay)
			{
				this.audioSource.Play();
			}
			if (this.shouldBeOn)
			{
				GameClock.onTimeDidPass = (Action<float>)Delegate.Combine(GameClock.onTimeDidPass, new Action<float>(this.OnTimeDidPass));
			}
			else
			{
				GameClock.onTimeDidPass = (Action<float>)Delegate.Remove(GameClock.onTimeDidPass, new Action<float>(this.OnTimeDidPass));
			}
		}
		if (this.shouldBeOn)
		{
			this.UpdateAlphaFlickerAndScale();
		}
	}

	// Token: 0x0600078F RID: 1935 RVA: 0x0004482C File Offset: 0x00042A2C
	private void LateUpdate()
	{
		if (this.shouldBeOn)
		{
			this.UpdatePositioning();
		}
	}

	// Token: 0x06000790 RID: 1936 RVA: 0x0004483C File Offset: 0x00042A3C
	private void UpdatePositioning()
	{
		base.transform.position = this.runner.animator.headTorchPos;
		base.transform.localRotation = Quaternion.Euler(0f, 0f, -this.runner.animator.headTorchAngle);
	}

	// Token: 0x06000791 RID: 1937 RVA: 0x00044890 File Offset: 0x00042A90
	private void UpdateAlphaFlickerAndScale()
	{
		float num = this._overallAlpha;
		float num2 = this._settings.alphaByBatteryLevel.Evaluate(this.batteryLevel);
		num *= num2;
		if (this.batteryLevel < this._settings.flickerBatteryLevel)
		{
			float num3 = this._settings.flickerNoiseActiveScale * Time.time;
			if (Mathf.PerlinNoise(509.13f + num3, 0.765f) > this._settings.flickerNoiseActiveThreshold)
			{
				float num4 = this._settings.flickerNoiseActiveOnScale * Time.time;
				num *= Mathf.Clamp01(Mathf.PerlinNoise(12.34f + num4, 0.123f) + this._settings.flickeringAmplitudeOffset);
			}
		}
		foreach (Torch.SpriteWithAlpha spriteWithAlpha in this._sprites)
		{
			float num5 = 1f;
			if (spriteWithAlpha.caveOnly)
			{
				num5 = CaveRegion.inCaveNorm;
			}
			spriteWithAlpha.sprite.color = spriteWithAlpha.sprite.color.WithAlpha(spriteWithAlpha.baseAlpha * num * num5);
		}
		base.transform.localScale = Vector3.one * Mathf.Lerp(this._settings.minimumScaleLowBattery, 1f, this.batteryLevel);
	}

	// Token: 0x06000792 RID: 1938 RVA: 0x000449EC File Offset: 0x00042BEC
	private void OnTimeDidPass(float daysNormDelta)
	{
		if (this.forceAlwaysHaveTorchWithFullBattery)
		{
			this.batteryLevel = 1f;
			return;
		}
		this.batteryLevel -= daysNormDelta * (24f / this._settings.batteryHours);
		if (this.batteryLevel < 0f)
		{
			this.batteryLevel = 0f;
		}
	}

	// Token: 0x0400094F RID: 2383
	[Range(0f, 1f)]
	public float batteryLevel = 1f;

	// Token: 0x04000950 RID: 2384
	public bool hasTorch = true;

	// Token: 0x04000951 RID: 2385
	public bool forceAlwaysHaveTorchWithFullBattery = true;

	// Token: 0x04000952 RID: 2386
	private float _overallAlpha = 1f;

	// Token: 0x04000953 RID: 2387
	private AudioSource _audioSource;

	// Token: 0x04000954 RID: 2388
	private Runner _runner;

	// Token: 0x04000955 RID: 2389
	[SerializeField]
	private List<Torch.SpriteWithAlpha> _sprites = new List<Torch.SpriteWithAlpha>();

	// Token: 0x04000956 RID: 2390
	[SerializeField]
	private TorchSettings _settings;

	// Token: 0x04000957 RID: 2391
	[SerializeField]
	private Transform _beamTransform;

	// Token: 0x0200030D RID: 781
	[Serializable]
	private struct SpriteWithAlpha
	{
		// Token: 0x0400179E RID: 6046
		public SpriteRenderer sprite;

		// Token: 0x0400179F RID: 6047
		public float baseAlpha;

		// Token: 0x040017A0 RID: 6048
		public bool caveOnly;
	}
}
