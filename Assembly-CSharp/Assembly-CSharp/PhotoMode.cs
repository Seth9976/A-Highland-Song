using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Steamworks;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

// Token: 0x02000137 RID: 311
public class PhotoMode : MonoSingleton<PhotoMode>
{
	// Token: 0x06000A70 RID: 2672 RVA: 0x000568AF File Offset: 0x00054AAF
	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
	private static void ResetStatics()
	{
		PhotoMode.wantsGameplayPaused = false;
		PhotoMode.visible = false;
	}

	// Token: 0x17000297 RID: 663
	// (get) Token: 0x06000A71 RID: 2673 RVA: 0x000568BD File Offset: 0x00054ABD
	// (set) Token: 0x06000A72 RID: 2674 RVA: 0x000568C4 File Offset: 0x00054AC4
	public static bool visible { get; private set; }

	// Token: 0x17000298 RID: 664
	// (get) Token: 0x06000A73 RID: 2675 RVA: 0x000568CC File Offset: 0x00054ACC
	private static string screenshotsDirectoryPath
	{
		get
		{
			return Path.Combine(Application.persistentDataPath, "Screenshots");
		}
	}

	// Token: 0x17000299 RID: 665
	// (get) Token: 0x06000A74 RID: 2676 RVA: 0x000568DD File Offset: 0x00054ADD
	private bool deck
	{
		get
		{
			return SteamManager.Initialized && SteamUtils.IsSteamRunningOnSteamDeck();
		}
	}

	// Token: 0x1700029A RID: 666
	// (get) Token: 0x06000A75 RID: 2677 RVA: 0x000568ED File Offset: 0x00054AED
	private GameCamera.FreeCameraState freeCam
	{
		get
		{
			return GameCamera.instance.freeCameraState;
		}
	}

	// Token: 0x06000A76 RID: 2678 RVA: 0x000568FC File Offset: 0x00054AFC
	public void Show()
	{
		if (PhotoMode.visible)
		{
			Debug.LogError("PhotoMode was already visible");
			return;
		}
		if (this._showingHiding)
		{
			Debug.LogError("PhotoMode was already hiding or showing");
			return;
		}
		PhotoMode.wantsGameplayPaused = true;
		PhotoMode.visible = true;
		this._showingHiding = true;
		if (MonoSingleton<RunTrack>.instance.playingOrAboutTo)
		{
			MonoSingleton<RunTrack>.instance.SetPaused(true, RunTrack.PauseReason.PhotoMode);
		}
		Game.instance.SetTimeScalar(Game.TimeScalar.PhotoMode, 0f);
		Narrative.instance.SetPaused(Narrative.PauseReason.PhotoMode, true);
		this._canvas.gameObject.SetActive(true);
		this._layout.Animate(0.5f, delegate
		{
			this._layout.groupAlpha = 1f;
		}).Then(delegate
		{
			this._showingHiding = false;
		});
		GameInput.PushControlStack(this);
		MonoSingleton<GameUI>.instance.GetComponent<Canvas>().enabled = false;
		this.freeCam.active = true;
		this.freeCam.restricted = true;
		this.freeCam.allowInput = true;
		this._selectedOptionIdx = 0;
		this._originalTimeOfDayNorm = GameClock.instance.timeOfDayNorm;
		this._weatherOverride = (this._originalWeather = WeatherSystem.instance.currentWeather);
		PostProcessLayer component = GameCamera.instance.GetComponent<PostProcessLayer>();
		if (this._colorGradingOverride == null)
		{
			Vector4 value = PostProcessManager.instance.GetHighestPriorityVolume(component).profile.GetSetting<ColorGrading>().gamma.value;
			this._colorGradingOverride = ScriptableObject.CreateInstance<ColorGrading>();
			this._colorGradingOverride.enabled.Override(true);
			this._colorGradingOverride.gamma.Override(value);
		}
		this._postProcessingVolume = PostProcessManager.instance.QuickVolume(LayerMask.NameToLayer("PostProcessingVolumes"), 100f, new PostProcessEffectSettings[] { this._colorGradingOverride });
	}

	// Token: 0x06000A77 RID: 2679 RVA: 0x00056AB4 File Offset: 0x00054CB4
	public void Hide()
	{
		if (this._showingHiding)
		{
			Debug.LogError("PhotoMode was already hiding or showing");
			return;
		}
		if (!PhotoMode.visible)
		{
			Debug.LogError("PhotoMode was already hidden");
			return;
		}
		this._showingHiding = true;
		this.freeCam.active = false;
		MonoSingleton<GameUI>.instance.GetComponent<Canvas>().enabled = true;
		GameClock.instance.timeOfDayNorm = this._originalTimeOfDayNorm;
		this._originalTimeOfDayNorm = GameClock.instance.timeOfDayNorm;
		WeatherSystem.instance.EndPhotoOverride(this._originalWeather);
		if (this._postProcessingVolume != null)
		{
			RuntimeUtilities.DestroyVolume(this._postProcessingVolume, true, true);
			this._postProcessingVolume = null;
		}
		if (this._dialogueBubble != null)
		{
			this._dialogueBubble.HideAndMarkForDestroy(0f);
			this._dialogueBubble = null;
		}
		this.ClearOptions();
		this._dialogueLineOptionNum = 0;
		this._layout.Animate(0.5f, delegate
		{
			this._layout.groupAlpha = 0f;
		}).Then(delegate
		{
			this._showingHiding = false;
			GameInput.PopControlStack(this, true);
			PhotoMode.wantsGameplayPaused = false;
			PhotoMode.visible = false;
			Game.instance.RemoveTimeScalar(Game.TimeScalar.PhotoMode);
			Narrative.instance.SetPaused(Narrative.PauseReason.PhotoMode, false);
			if (MonoSingleton<RunTrack>.instance.playingOrAboutTo)
			{
				MonoSingleton<RunTrack>.instance.SetPaused(false, RunTrack.PauseReason.PhotoMode);
			}
		});
	}

	// Token: 0x06000A78 RID: 2680 RVA: 0x00056BBC File Offset: 0x00054DBC
	private void Start()
	{
		this._canvas.gameObject.SetActive(false);
		this._layout.groupAlpha = 0f;
		this._whiteout.gameObject.SetActive(false);
		this._options.groupAlpha = 0f;
		this._options.scale = this._settings.optionsHiddenScale;
		this._notification.groupAlpha = 0f;
		this._notification.scale = this._settings.notificationHiddenScale;
		this._toggleUISwitch.gameObject.SetActive(false);
		this._takePhotoSwitch.gameObject.SetActive(false);
	}

	// Token: 0x06000A79 RID: 2681 RVA: 0x00056C69 File Offset: 0x00054E69
	protected override void OnDestroy()
	{
		base.OnDestroy();
		if (this._postProcessingVolume != null)
		{
			RuntimeUtilities.DestroyVolume(this._postProcessingVolume, true, true);
			this._postProcessingVolume = null;
		}
	}

	// Token: 0x06000A7A RID: 2682 RVA: 0x00056C94 File Offset: 0x00054E94
	private void Update()
	{
		if (!Game.loaded)
		{
			return;
		}
		if (!PhotoMode.visible && !this._showingHiding && !this._layout.isAnimating && MonoSingleton<JournalController>.instance.canShowAndNotAlreadyVisible && GameInput.togglePhotoMode)
		{
			GameInput.ClearInputState();
			this.Show();
		}
		if (!PhotoMode.visible || this._showingHiding || this._takingScreenshot || this._layout.isAnimating)
		{
			return;
		}
		if ((GameInput.Back(this) || (GameInput.togglePhotoMode && PhotoMode.visible && GameInput.HasControl(this))) && !this._takingScreenshot && this._layout.targetGroupAlpha > 0f)
		{
			GameInput.ClearInputState();
			this.Hide();
			return;
		}
		if (GameInput.takePhoto && GameInput.HasControl(this) && !this._takingScreenshot)
		{
			base.StartCoroutine(this.TakeScreenshot_Coroutine());
		}
		if (GameInput.revealPhoto && !this.deck)
		{
			string screenshotsDirectoryPath = PhotoMode.screenshotsDirectoryPath;
			if (this._lastSavedScreenshotPath != null && Directory.Exists(screenshotsDirectoryPath))
			{
				SystemX.OpenInFileBrowser((this._lastSavedScreenshotPath != null) ? this._lastSavedScreenshotPath : screenshotsDirectoryPath);
			}
		}
		if (GameInput.HasControl(this) && (GameInput.photoModeToggleUI || (GameInput.Back(this) && this._layout.targetGroupAlpha == 0f)))
		{
			this._layout.Animate(0.2f, delegate
			{
				this._layout.groupAlpha = (float)((this._layout.targetGroupAlpha > 0f) ? 0 : 1);
			});
		}
		if (GameInput.photoModeOptions || GameInput.Back(this._options))
		{
			if (this._layout.targetGroupAlpha == 0f)
			{
				this._layout.Animate(0.2f, delegate
				{
					this._layout.groupAlpha = 1f;
				});
			}
			this.ToggleOptions();
		}
		if (GameInput.HasControl(this._options))
		{
			if (GameInput.selectUp)
			{
				GameInput.ClearInputState();
				this.SelectOption(-1);
			}
			if (GameInput.selectDown)
			{
				GameInput.ClearInputState();
				this.SelectOption(1);
			}
			if (GameInput.selectRight)
			{
				GameInput.ClearInputState();
				this._optionControls[this._selectedOptionIdx].LeftRight(1);
			}
			if (GameInput.selectLeft)
			{
				GameInput.ClearInputState();
				this._optionControls[this._selectedOptionIdx].LeftRight(-1);
			}
			if (GameInput.selectMenuItem)
			{
				GameInput.ClearInputState();
				this._optionControls[this._selectedOptionIdx].Trigger();
			}
		}
		WeatherSystem.instance.SetPhotoOverride(this._weatherOverride);
	}

	// Token: 0x06000A7B RID: 2683 RVA: 0x00056EEC File Offset: 0x000550EC
	private void SelectOption(int dir)
	{
		this._selectedOptionIdx = Mathf.Clamp(this._selectedOptionIdx + dir, 0, this._optionControls.Count - 1);
		for (int i = 0; i < this._optionControls.Count; i++)
		{
			this._optionControls[i].highlighted = i == this._selectedOptionIdx;
		}
	}

	// Token: 0x06000A7C RID: 2684 RVA: 0x00056F4C File Offset: 0x0005514C
	private void ToggleOptions()
	{
		if (GameInput.HasControl(this._options))
		{
			GameInput.PopControlStack(this._options, true);
			this._layout.Animate(0.2f, delegate
			{
				this._options.groupAlpha = 0f;
				this._options.scale = this._settings.optionsHiddenScale;
				this._nonOptions.groupAlpha = 1f;
			}).Then(delegate
			{
			});
			this.freeCam.allowInput = true;
			return;
		}
		this._layout.Animate(0.2f, delegate
		{
			this._options.groupAlpha = 1f;
			this._options.scale = 1f;
			this._nonOptions.groupAlpha = 0f;
		});
		GameInput.PushControlStack(this._options);
		this.freeCam.allowInput = false;
		this.CreateOptionsIfNecessary();
	}

	// Token: 0x06000A7D RID: 2685 RVA: 0x00056FFC File Offset: 0x000551FC
	private void CreateOptionsIfNecessary()
	{
		if (this._optionControls.Count > 0)
		{
			return;
		}
		float num = this._settings.optionsStartY;
		SettingSlider settingSlider = this._optionSliderPrototype.Instantiate<SettingSlider>(null);
		settingSlider.leftRightIncrements = 0.01f;
		settingSlider.Setup("Time of day", GameClock.instance.timeOfDayNorm, delegate(float newT)
		{
			GameClock.instance.timeOfDayNorm = newT;
		}, null);
		settingSlider.layout.topY = num;
		this._optionControls.Add(settingSlider);
		num += this._settings.optionsControlHeight;
		SettingPicker settingPicker = this._optionPickerPrototype.Instantiate<SettingPicker>(null);
		settingPicker.Setup("Weather", PhotoMode._weatherOptions, PhotoMode.WeatherTypeToString(this._weatherOverride), delegate(string newWeatherStr)
		{
			this._weatherOverride = PhotoMode.WeatherStringToType(newWeatherStr);
			WeatherSystem.instance.SetPhotoOverride(this._weatherOverride);
		}, null);
		settingPicker.layout.topY = num;
		this._optionControls.Add(settingPicker);
		num += this._settings.optionsControlHeight;
		SettingSlider settingSlider2 = this._optionSliderPrototype.Instantiate<SettingSlider>(null);
		settingSlider2.leftRightIncrements = 0.05f;
		settingSlider2.Setup("Colour grade", this.GammaToHueNorm(this._colorGradingOverride.gamma.value), delegate(float newValue)
		{
			this._colorGradingOverride.gamma.Override(this.ModifyGammaHue(this._colorGradingOverride.gamma.value, newValue));
		}, null);
		settingSlider2.layout.topY = num;
		this._optionControls.Add(settingSlider2);
		num += this._settings.optionsControlHeight;
		SettingSlider settingSlider3 = this._optionSliderPrototype.Instantiate<SettingSlider>(null);
		settingSlider3.leftRightIncrements = 0.05f;
		settingSlider3.Setup("Exposure", this.GammaToExposureNorm(this._colorGradingOverride.gamma.value), delegate(float newValue)
		{
			this._colorGradingOverride.gamma.Override(this.ModifyGammaExposure(this._colorGradingOverride.gamma.value, newValue));
		}, null);
		settingSlider3.layout.topY = num;
		this._optionControls.Add(settingSlider3);
		num += this._settings.optionsControlHeight;
		this.SelectOption(this._selectedOptionIdx);
		List<string> recentDialogue = NarrativePresenter.instance.recentMoiraDialogue;
		if (recentDialogue != null && recentDialogue.Count > 0)
		{
			string[] array = new string[recentDialogue.Count + 1];
			array[0] = "NONE";
			for (int i = 0; i < recentDialogue.Count; i++)
			{
				array[i + 1] = "LINE " + (i + 1).ToString();
			}
			SettingPicker settingPicker2 = this._optionPickerPrototype.Instantiate<SettingPicker>(null);
			settingPicker2.Setup("Recent dialogue", array, array[this._dialogueLineOptionNum], delegate(string newLineStr)
			{
				if (this._dialogueBubble != null)
				{
					this._dialogueBubble.HideAndMarkForDestroy(0f);
					this._dialogueBubble = null;
				}
				if (newLineStr == "NONE")
				{
					this._dialogueLineOptionNum = 0;
					return;
				}
				if (newLineStr != "NONE")
				{
					this._dialogueLineOptionNum = int.Parse(newLineStr.Substring("LINE ".Length));
					this._dialogueBubble = this._dialoguePrototype.Instantiate<DialogueBubbleView>(null);
					this._dialogueBubble.Setup(StoryCharacter.WithName("MOIRA"), recentDialogue[this._dialogueLineOptionNum - 1]);
					this._dialogueBubble.animTextView.SkipToShown();
					this._dialogueBubble.transform.SetSiblingIndex(0);
				}
			}, null);
			settingPicker2.layout.topY = num;
			this._optionControls.Add(settingPicker2);
			num += this._settings.optionsControlHeight;
		}
		this._options.height = num + this._settings.optionBottomMargin;
	}

	// Token: 0x06000A7E RID: 2686 RVA: 0x000572E0 File Offset: 0x000554E0
	private float GammaToHueNorm(Vector4 gammaToHue)
	{
		float num;
		float num2;
		float num3;
		Color.RGBToHSV(new Color(gammaToHue.x, gammaToHue.y, gammaToHue.z, 1f), out num, out num2, out num3);
		return Mathf.Repeat(num + 0.5f, 1f);
	}

	// Token: 0x06000A7F RID: 2687 RVA: 0x00057325 File Offset: 0x00055525
	private float GammaToExposureNorm(Vector4 gammaToHue)
	{
		return Mathf.InverseLerp(-1f, 1f, gammaToHue.w);
	}

	// Token: 0x06000A80 RID: 2688 RVA: 0x0005733C File Offset: 0x0005553C
	private Vector4 ModifyGammaHue(Vector4 originalGamma, float newHue)
	{
		float num;
		float num2;
		float num3;
		Color.RGBToHSV(new Color(originalGamma.x, originalGamma.y, originalGamma.z, 1f), out num, out num2, out num3);
		Color color = Color.HSVToRGB(Mathf.Repeat(newHue - 0.5f, 1f), num2, num3);
		return new Vector4(color.r, color.g, color.b, originalGamma.w);
	}

	// Token: 0x06000A81 RID: 2689 RVA: 0x000573A8 File Offset: 0x000555A8
	private Vector4 ModifyGammaExposure(Vector4 originalGamma, float newExposure)
	{
		Vector4 vector = originalGamma;
		vector.w = Mathf.Lerp(-1f, 1f, newExposure);
		return vector;
	}

	// Token: 0x06000A82 RID: 2690 RVA: 0x000573D0 File Offset: 0x000555D0
	private static string WeatherTypeToString(WeatherType weatherType)
	{
		if ((weatherType & WeatherType.Snow) > WeatherType.Clear)
		{
			return "Snowy";
		}
		if ((weatherType & WeatherType.Storm) > WeatherType.Clear)
		{
			return "Stormy";
		}
		if ((weatherType & WeatherType.Raining) > WeatherType.Clear)
		{
			return "Rainy";
		}
		if ((weatherType & WeatherType.VeryCloudy) > WeatherType.Clear)
		{
			return "Very Cloudy";
		}
		if ((weatherType & WeatherType.Cloudy) > WeatherType.Clear || (weatherType & WeatherType.Foggy) > WeatherType.Clear)
		{
			return "Cloudy";
		}
		return "Clear";
	}

	// Token: 0x06000A83 RID: 2691 RVA: 0x0005742C File Offset: 0x0005562C
	private static WeatherType WeatherStringToType(string weather)
	{
		if (weather == "Snowy")
		{
			return WeatherType.Snow;
		}
		if (weather == "Stormy")
		{
			return WeatherType.Storm;
		}
		if (weather == "Rainy")
		{
			return WeatherType.Raining;
		}
		if (weather == "Very Cloudy")
		{
			return WeatherType.VeryCloudy;
		}
		if (weather == "Cloudy")
		{
			return WeatherType.Cloudy;
		}
		return WeatherType.Clear;
	}

	// Token: 0x06000A84 RID: 2692 RVA: 0x00057488 File Offset: 0x00055688
	private void ClearOptions()
	{
		foreach (SettingControl settingControl in this._optionControls)
		{
			settingControl.GetComponent<Prototype>().ReturnToPool();
		}
		this._optionControls.Clear();
	}

	// Token: 0x06000A85 RID: 2693 RVA: 0x000574E8 File Offset: 0x000556E8
	private IEnumerator TakeScreenshot_Coroutine()
	{
		this._takingScreenshot = true;
		this._layout.gameObject.SetActive(false);
		AudioController.instance.PlaySoundEffect(SoundEffect.CameraShutter);
		yield return new WaitForEndOfFrame();
		int superSize = 1;
		if (Screen.width <= 1920 && Screen.height <= 1080)
		{
			superSize = 2;
		}
		this._lastSavedScreenshotPath = PhotoMode.NextScreenshotFilePath();
		ScreenCapture.CaptureScreenshot(this._lastSavedScreenshotPath, superSize);
		if (SteamManager.Initialized)
		{
			float timeout = Time.unscaledTime + 5f;
			while (!File.Exists(this._lastSavedScreenshotPath) && Time.unscaledTime < timeout)
			{
				yield return new WaitForEndOfFrame();
			}
			if (File.Exists(this._lastSavedScreenshotPath))
			{
				SteamScreenshots.AddScreenshotToLibrary(this._lastSavedScreenshotPath, null, Screen.width * superSize, Screen.height * superSize);
				CallResult<ScreenshotReady_t>.Create(delegate(ScreenshotReady_t ready, bool success)
				{
					Debug.Log(string.Format("ready={0} success={1}", ready, success));
				});
			}
		}
		yield return new WaitForEndOfFrame();
		this._whiteout.gameObject.SetActive(true);
		yield return new WaitForEndOfFrame();
		yield return new WaitForEndOfFrame();
		this._whiteout.gameObject.SetActive(false);
		this._layout.gameObject.SetActive(true);
		this._takingScreenshot = false;
		if (!this.deck)
		{
			this._notification.CancelAnimations();
			this._notification.Animate(0.3f, 0f, SLayout.popCurve, delegate
			{
				this._notification.scale = 1f;
				this._notification.groupAlpha = 1f;
			}).ThenAnimate(0.3f, 5f, SLayout.reversePopCurve, delegate
			{
				this._notification.scale = this._settings.notificationHiddenScale;
				this._notification.groupAlpha = 0f;
			});
		}
		yield break;
	}

	// Token: 0x06000A86 RID: 2694 RVA: 0x000574F8 File Offset: 0x000556F8
	private static string NextScreenshotFilePath()
	{
		string text = "A_Highland_Song";
		int num = 1;
		string screenshotsDirectoryPath = PhotoMode.screenshotsDirectoryPath;
		if (Directory.Exists(screenshotsDirectoryPath))
		{
			string[] files = Directory.GetFiles(screenshotsDirectoryPath);
			for (int i = 0; i < files.Length; i++)
			{
				string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(files[i]);
				if (fileNameWithoutExtension.StartsWith(text))
				{
					string text2 = fileNameWithoutExtension.Substring(text.Length);
					if (text2.Length == 0)
					{
						if (num == 1)
						{
							num = 2;
						}
					}
					else if (text2[0] == '_')
					{
						text2 = text2.TrimStart('_');
						int num2;
						if (int.TryParse(text2, out num2) && num2 >= num)
						{
							num = num2 + 1;
						}
					}
				}
			}
		}
		else
		{
			Directory.CreateDirectory(screenshotsDirectoryPath);
		}
		string text3 = ((num > 1) ? string.Format("{0}_{1}.png", text, num) : (text + ".png"));
		return Path.Combine(screenshotsDirectoryPath, text3);
	}

	// Token: 0x04000CA1 RID: 3233
	public static bool wantsGameplayPaused;

	// Token: 0x04000CA3 RID: 3235
	private static string[] _weatherOptions = new string[] { "Clear", "Cloudy", "Very Cloudy", "Rainy", "Stormy", "Snowy" };

	// Token: 0x04000CA4 RID: 3236
	private bool _showingHiding;

	// Token: 0x04000CA5 RID: 3237
	private bool _takingScreenshot;

	// Token: 0x04000CA6 RID: 3238
	private string _lastSavedScreenshotPath;

	// Token: 0x04000CA7 RID: 3239
	private List<SettingControl> _optionControls = new List<SettingControl>();

	// Token: 0x04000CA8 RID: 3240
	private int _selectedOptionIdx;

	// Token: 0x04000CA9 RID: 3241
	private float _originalTimeOfDayNorm;

	// Token: 0x04000CAA RID: 3242
	private WeatherType _originalWeather;

	// Token: 0x04000CAB RID: 3243
	private WeatherType _weatherOverride;

	// Token: 0x04000CAC RID: 3244
	private ColorGrading _colorGradingOverride;

	// Token: 0x04000CAD RID: 3245
	private PostProcessVolume _postProcessingVolume;

	// Token: 0x04000CAE RID: 3246
	private DialogueBubbleView _dialogueBubble;

	// Token: 0x04000CAF RID: 3247
	private int _dialogueLineOptionNum;

	// Token: 0x04000CB0 RID: 3248
	[SerializeField]
	private Canvas _canvas;

	// Token: 0x04000CB1 RID: 3249
	[SerializeField]
	private SLayout _layout;

	// Token: 0x04000CB2 RID: 3250
	[SerializeField]
	private SLayout _whiteout;

	// Token: 0x04000CB3 RID: 3251
	[SerializeField]
	private SLayout _toggleUIDesktop;

	// Token: 0x04000CB4 RID: 3252
	[SerializeField]
	private SLayout _toggleUISwitch;

	// Token: 0x04000CB5 RID: 3253
	[SerializeField]
	private SLayout _takePhotoDesktop;

	// Token: 0x04000CB6 RID: 3254
	[SerializeField]
	private SLayout _takePhotoSwitch;

	// Token: 0x04000CB7 RID: 3255
	[SerializeField]
	private SLayout _options;

	// Token: 0x04000CB8 RID: 3256
	[SerializeField]
	private SLayout _nonOptions;

	// Token: 0x04000CB9 RID: 3257
	[SerializeField]
	private SLayout _notification;

	// Token: 0x04000CBA RID: 3258
	[SerializeField]
	private Prototype _optionSliderPrototype;

	// Token: 0x04000CBB RID: 3259
	[SerializeField]
	private Prototype _optionPickerPrototype;

	// Token: 0x04000CBC RID: 3260
	[SerializeField]
	private Prototype _optionTogglePrototype;

	// Token: 0x04000CBD RID: 3261
	[SerializeField]
	private Prototype _dialoguePrototype;

	// Token: 0x04000CBE RID: 3262
	[SerializeField]
	private PhotoModeSettings _settings;
}
