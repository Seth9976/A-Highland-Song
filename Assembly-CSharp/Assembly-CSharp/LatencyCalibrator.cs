using System;
using System.Collections.Generic;
using EasyButtons;
using TMPro;
using UnityEngine;

// Token: 0x02000063 RID: 99
public class LatencyCalibrator : MonoSingleton<LatencyCalibrator>
{
	// Token: 0x170000C3 RID: 195
	// (get) Token: 0x060002AC RID: 684 RVA: 0x0001634F File Offset: 0x0001454F
	public bool visibleAtAll
	{
		get
		{
			return this._layout.gameObject.activeSelf && this._layout.groupAlpha > 0f;
		}
	}

	// Token: 0x170000C4 RID: 196
	// (get) Token: 0x060002AD RID: 685 RVA: 0x00016377 File Offset: 0x00014577
	public bool readyToShow
	{
		get
		{
			return this._layout != null && this._layout.gameObject != null;
		}
	}

	// Token: 0x170000C5 RID: 197
	// (get) Token: 0x060002AE RID: 686 RVA: 0x0001639A File Offset: 0x0001459A
	public static bool shouldPerformLaunchCalibration
	{
		get
		{
			return SystemInfo.deviceType == DeviceType.Desktop || PlayerPrefsX.GetInt("PerformedInitialCalibration", 0) != 1;
		}
	}

	// Token: 0x170000C6 RID: 198
	// (get) Token: 0x060002AF RID: 687 RVA: 0x000163B9 File Offset: 0x000145B9
	// (set) Token: 0x060002B0 RID: 688 RVA: 0x000163D6 File Offset: 0x000145D6
	public static float latency
	{
		get
		{
			if (LatencyCalibrator._latency == -1f)
			{
				LatencyCalibrator._latency = LatencyCalibrator.latencyPrefValue;
			}
			return LatencyCalibrator._latency;
		}
		set
		{
			LatencyCalibrator._latency = value;
			LatencyCalibrator.latencyPrefValue = value;
		}
	}

	// Token: 0x170000C7 RID: 199
	// (get) Token: 0x060002B1 RID: 689 RVA: 0x000163E4 File Offset: 0x000145E4
	// (set) Token: 0x060002B2 RID: 690 RVA: 0x00016407 File Offset: 0x00014607
	public static float latencyPrefValue
	{
		get
		{
			if (PlayerPrefsX.HasKey("Latency"))
			{
				return PlayerPrefsX.GetFloat("Latency", 0f);
			}
			return 0.03f;
		}
		set
		{
			PlayerPrefsX.SetFloat("Latency", value);
		}
	}

	// Token: 0x060002B3 RID: 691 RVA: 0x00016414 File Offset: 0x00014614
	[Button("Reset latency value to zero")]
	private void ResetToZero()
	{
		LatencyCalibrator.latency = 0f;
	}

	// Token: 0x060002B4 RID: 692 RVA: 0x00016420 File Offset: 0x00014620
	public void Show(LatencyCalibrator.Context context)
	{
		this._context = context;
		this._layout.gameObject.SetActive(true);
		this._animator.gameObject.SetActive(true);
		this._animator.SetAnimation("Run", FrameAnimator.PosMatch.None);
		this._animatorSpeedY = 0f;
		this._jumping = false;
		this._playerCompleteWillAutoContinue = false;
		this._doneTick.groupAlpha = 0f;
		this._continueButton.groupAlpha = 1f;
		this._continueButtonText.text = ((context == LatencyCalibrator.Context.SplashScreenAuto) ? "Skip" : "Continue");
		if (this._layout.groupAlpha == 0f)
		{
			this._layout.y = -400f;
		}
		this._layout.Animate(0.5f, delegate
		{
			this._layout.groupAlpha = 1f;
			this._layout.y = 0f;
		});
		GameInput.PushControlStack(this);
		this.Reset();
		this.visible = true;
	}

	// Token: 0x060002B5 RID: 693 RVA: 0x0001650C File Offset: 0x0001470C
	public void Hide()
	{
		this.Reset();
		this._audioSource.Stop();
		PlayerPrefsX.Save();
		this.visible = false;
		this._layout.Animate(0.2f, delegate
		{
			this._layout.groupAlpha = 0f;
		}).Then(delegate
		{
			GameInput.PopControlStack(this, true);
			this._layout.gameObject.SetActive(false);
			this._playerCompleteWillAutoContinue = false;
			this._animator.gameObject.SetActive(false);
			if (this._animatorFloorY != 0f)
			{
				this._animatorLayout.y = this._animatorFloorY;
			}
			this._animatorSpeedY = 0f;
			this._jumping = false;
		});
	}

	// Token: 0x060002B6 RID: 694 RVA: 0x00016564 File Offset: 0x00014764
	private void Awake()
	{
		this._layout.groupAlpha = 0f;
		this._layout.gameObject.SetActive(false);
		this._jumping = false;
		this._doneTick.groupAlpha = 0f;
	}

	// Token: 0x060002B7 RID: 695 RVA: 0x0001659E File Offset: 0x0001479E
	private void Reset()
	{
		this._samples.Clear();
		this._beatIndex = -1;
		this._currentBeatScheduledDSPTime = -1.0;
		this._lastInputTime = -1f;
		this._standardDeviation = float.MaxValue;
	}

	// Token: 0x060002B8 RID: 696 RVA: 0x000165D8 File Offset: 0x000147D8
	private void Update()
	{
		if (!this.visible)
		{
			return;
		}
		if (this._jumping)
		{
			this._animatorSpeedY += this.gravity * Time.unscaledDeltaTime;
			float num = this._animatorLayout.y + this._animatorSpeedY * Time.unscaledDeltaTime;
			if (num < this._animatorFloorY)
			{
				num = this._animatorFloorY;
				this._animatorFloorY = 0f;
				this._jumping = false;
				this._animator.SetAnimation("Run", FrameAnimator.PosMatch.None);
			}
			this._animatorLayout.y = num;
		}
		float num2 = Time.unscaledTime - this._lastInputTime;
		float num3 = Mathf.InverseLerp(Mathf.Pow(0.2f, 2f), 0f, Mathf.Pow(num2, 2f));
		this._tapShape.alpha = num3;
		if (this._playerCompleteWillAutoContinue)
		{
			return;
		}
		if (GameInput.Back(this))
		{
			GameInput.ClearInputState();
			this.Hide();
			return;
		}
		if (this._currentBeatScheduledDSPTime == -1.0)
		{
			this._currentBeatScheduledDSPTime = AudioSettings.dspTime;
			this._beatIndex = -1;
		}
		if (AudioSettings.dspTime >= this._currentBeatScheduledDSPTime + (double)(this._secondsPerBeat * 0.5f))
		{
			this._currentBeatScheduledDSPTime += (double)this._secondsPerBeat;
			this._audioSource.PlayScheduled(this._currentBeatScheduledDSPTime);
			this._beatIndex++;
		}
		if (this._beatIndex >= 0 && GameInput.jumped)
		{
			double num4 = AudioSettings.dspTime - this._currentBeatScheduledDSPTime;
			this._lastInputTime = Time.unscaledTime;
			Debug.Log("Hit beat index " + this._beatIndex.ToString() + " with delta " + num4.ToString());
			while (this._samples.Count >= Mathf.Max(this.settings.numSamples, 1))
			{
				this._samples.RemoveAt(0);
			}
			if (this.settings.numSamples > 0)
			{
				this._samples.Add((float)num4);
				this.CalculateLatencyFromSamples();
			}
			if (!this._jumping)
			{
				if (this._animatorFloorY == 0f)
				{
					this._animatorFloorY = this._animatorLayout.y;
				}
				this._animator.SetAnimation("Jump", FrameAnimator.PosMatch.None);
				this._animatorSpeedY = this.jumpSpeed;
				this._jumping = true;
			}
		}
		if (this._standardDeviation < this.settings.standardDeviationMax)
		{
			if (this._doneTick.targetGroupAlpha == 0f)
			{
				this._tingSource.Play();
				if (this._doneTick.groupAlpha == 0f)
				{
					this._doneTick.scale = 0.3f;
				}
				this._doneTick.Animate(0.3f, 0f, SLayout.popCurve, delegate
				{
					this._doneTick.scale = 1f;
					this._doneTick.groupAlpha = 1f;
				});
				if (this._context == LatencyCalibrator.Context.SplashScreenAuto)
				{
					this._playerCompleteWillAutoContinue = true;
					this._continueButton.Animate(0.3f, delegate
					{
						this._continueButton.groupAlpha = 0f;
					});
					this._layout.After(2f, delegate
					{
						this.Hide();
					});
				}
			}
		}
		else if (this._doneTick.targetGroupAlpha > 0f)
		{
			this._doneTick.Animate(0.3f, delegate
			{
				this._doneTick.groupAlpha = 0f;
			});
		}
		string text = string.Format("{0}ms", Mathf.RoundToInt(LatencyCalibrator.latency * 1000f));
		this._descriptionLabel.text = text;
	}

	// Token: 0x060002B9 RID: 697 RVA: 0x00016948 File Offset: 0x00014B48
	private void CalculateLatencyFromSamples()
	{
		float num = 0f;
		float num2 = 0f;
		for (int i = 0; i < this._samples.Count; i++)
		{
			float num3 = this._samples[i];
			float num4 = Mathf.Lerp(1f - this.settings.sampleFalloff, 1f, Mathf.InverseLerp((float)(this.settings.numSamples - 1), 0f, (float)i));
			num += num3 * num4;
			num2 += num4;
		}
		LatencyCalibrator.latency = num / num2;
		this._standardDeviation = float.MaxValue;
		if (this._samples.Count == this.settings.numSamples)
		{
			float num5 = 0f;
			for (int j = 0; j < this._samples.Count; j++)
			{
				float num6 = this._samples[j] - LatencyCalibrator.latency;
				float num7 = num6 * num6;
				num5 += num7;
			}
			this._standardDeviation = Mathf.Sqrt(num5 / (float)this._samples.Count);
			Debug.Log("Standard deviation: " + this._standardDeviation.ToString());
		}
		if (!LatencyCalibrator._recordedInitialCalibration)
		{
			PlayerPrefsX.SetInt("PerformedInitialCalibration", 1);
			LatencyCalibrator._recordedInitialCalibration = true;
		}
	}

	// Token: 0x060002BA RID: 698 RVA: 0x00016A7D File Offset: 0x00014C7D
	private void OnSliderChange(float val)
	{
		LatencyCalibrator.latency = val;
	}

	// Token: 0x170000C8 RID: 200
	// (get) Token: 0x060002BB RID: 699 RVA: 0x00016A85 File Offset: 0x00014C85
	private float _secondsPerBeat
	{
		get
		{
			return 1f / this.settings.bpm * 60f;
		}
	}

	// Token: 0x040003DF RID: 991
	public float jumpSpeed = 100f;

	// Token: 0x040003E0 RID: 992
	public float gravity = -100f;

	// Token: 0x040003E1 RID: 993
	[Disable]
	public bool visible;

	// Token: 0x040003E2 RID: 994
	private const string performedInitialCalibrationPrefKey = "PerformedInitialCalibration";

	// Token: 0x040003E3 RID: 995
	public const float defaultLatency = 0.03f;

	// Token: 0x040003E4 RID: 996
	public static float _latency = -1f;

	// Token: 0x040003E5 RID: 997
	private List<float> _samples = new List<float>();

	// Token: 0x040003E6 RID: 998
	private int _beatIndex = -1;

	// Token: 0x040003E7 RID: 999
	private double _currentBeatScheduledDSPTime = -1.0;

	// Token: 0x040003E8 RID: 1000
	private float _lastInputTime = -1f;

	// Token: 0x040003E9 RID: 1001
	private float _standardDeviation = float.MaxValue;

	// Token: 0x040003EA RID: 1002
	private float _animatorFloorY;

	// Token: 0x040003EB RID: 1003
	private float _animatorSpeedY;

	// Token: 0x040003EC RID: 1004
	private bool _jumping;

	// Token: 0x040003ED RID: 1005
	private bool _playerCompleteWillAutoContinue;

	// Token: 0x040003EE RID: 1006
	private LatencyCalibrator.Context _context;

	// Token: 0x040003EF RID: 1007
	[SerializeField]
	private SLayout _layout;

	// Token: 0x040003F0 RID: 1008
	[SerializeField]
	private AudioSource _audioSource;

	// Token: 0x040003F1 RID: 1009
	[SerializeField]
	private AudioSource _tingSource;

	// Token: 0x040003F2 RID: 1010
	[SerializeField]
	private TextMeshProUGUI _descriptionLabel;

	// Token: 0x040003F3 RID: 1011
	[SerializeField]
	private SLayout _tapShape;

	// Token: 0x040003F4 RID: 1012
	[SerializeField]
	private SLayout _doneTick;

	// Token: 0x040003F5 RID: 1013
	[SerializeField]
	private SLayout _continueButton;

	// Token: 0x040003F6 RID: 1014
	[SerializeField]
	private TextMeshProUGUI _continueButtonText;

	// Token: 0x040003F7 RID: 1015
	[SerializeField]
	private FrameAnimator _animator;

	// Token: 0x040003F8 RID: 1016
	[SerializeField]
	private SLayout _animatorLayout;

	// Token: 0x040003F9 RID: 1017
	[SerializeField]
	private LatencyCalibratorSettings settings;

	// Token: 0x040003FA RID: 1018
	private static bool _recordedInitialCalibration = false;

	// Token: 0x02000288 RID: 648
	public enum Context
	{
		// Token: 0x040014FB RID: 5371
		SplashScreenAuto,
		// Token: 0x040014FC RID: 5372
		Manual
	}
}
