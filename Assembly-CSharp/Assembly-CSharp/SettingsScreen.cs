using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ActionIcon;
using EasyButtons;
using InControl;
using Steamworks;
using TMPro;
using UnityEngine;

// Token: 0x0200014A RID: 330
public class SettingsScreen : MonoSingleton<SettingsScreen>
{
	// Token: 0x170002AC RID: 684
	// (get) Token: 0x06000B17 RID: 2839 RVA: 0x000599D0 File Offset: 0x00057BD0
	private bool allowInput
	{
		get
		{
			return GameInput.HasControl(this) || GameInput.HasControl(this._remapControlsPanelBackItem);
		}
	}

	// Token: 0x170002AD RID: 685
	// (get) Token: 0x06000B18 RID: 2840 RVA: 0x000599E7 File Offset: 0x00057BE7
	public static SettingsScreenSettings settings
	{
		get
		{
			return MonoSingleton<SettingsScreen>.instance._settings;
		}
	}

	// Token: 0x06000B19 RID: 2841 RVA: 0x000599F4 File Offset: 0x00057BF4
	public void Show()
	{
		if (this.visible)
		{
			return;
		}
		this._canvas.gameObject.SetActive(true);
		this._layout.Animate(0.2f, delegate
		{
			this._layout.groupAlpha = 1f;
		});
		this._panelSlideSpeed = 0f;
		if (this._panelDefaultY == 0f)
		{
			this._panelDefaultY = this._panel.topY;
		}
		if (this._layout.groupAlpha == 0f)
		{
			this._panel.topY = this._panelDefaultY + 400f;
		}
		this._layout.Animate(0.3f, delegate
		{
			this._panel.topY = this._panelDefaultY;
			this._panelTargetY = this._panel.y;
		});
		GameInput.PushControlStack(this);
		this.CreateContent();
		this.visible = true;
	}

	// Token: 0x06000B1A RID: 2842 RVA: 0x00059ABC File Offset: 0x00057CBC
	public void Hide()
	{
		if (!this.visible)
		{
			return;
		}
		this.visible = false;
		PlayerPrefsX.Save();
		GameInput.PopControlStack(this, true);
		this._layout.Animate(0.2f, delegate
		{
			this._layout.groupAlpha = 0f;
			this._panel.topY += 400f;
		}).Then(delegate
		{
			this.RemoveContent();
			this._canvas.gameObject.SetActive(false);
		});
	}

	// Token: 0x06000B1B RID: 2843 RVA: 0x00059B14 File Offset: 0x00057D14
	public static void SetupDefaultSettingsIfNecessary()
	{
		if (!PlayerPrefsX.HasKey("AutoContinue"))
		{
			PlayerPrefsX.SetInt("AutoContinue", 1);
		}
		if (!PlayerPrefsX.HasKey("dontMusicRunTrip"))
		{
			PlayerPrefsX.SetInt("dontMusicRunTrip", 0);
		}
		if (!PlayerPrefsX.HasKey("neverSlip"))
		{
			PlayerPrefsX.SetInt("neverSlip", 0);
		}
		if (!PlayerPrefsX.HasKey("highQualityWater"))
		{
			PlayerPrefsX.SetInt("highQualityWater", 1);
		}
		if (!PlayerPrefsX.HasKey("readingSpeed"))
		{
			PlayerPrefsX.SetFloat("readingSpeed", 0.5f);
		}
		if (!PlayerPrefsX.HasKey(Health.weatherDifficultyPrefName))
		{
			Health.weatherDifficulty = Health.WeatherDifficulty.Moderate;
		}
		if (!PlayerPrefsX.HasKey("MusicRunningEasyMode"))
		{
			PlayerPrefsX.SetInt("MusicRunningEasyMode", 0);
		}
		if (!PlayerPrefsX.HasKey("singleJumpButton"))
		{
			PlayerPrefsX.SetInt("singleJumpButton", 0);
		}
		if (!PlayerPrefsX.HasKey("altControls"))
		{
			PlayerPrefsX.SetInt("altControls", 0);
		}
		if (!PlayerPrefsX.HasKey(InputVibration.vibrationPrefName))
		{
			PlayerPrefsX.SetInt(InputVibration.vibrationPrefName, 1);
		}
		if (!PlayerPrefsX.HasKey(WeatherSystem.lightingSettingPrefName))
		{
			PlayerPrefsX.SetInt(WeatherSystem.lightingSettingPrefName, 1);
		}
		if (!PlayerPrefsX.HasKey(RouteVisualiser.highlightRoutesPrefName))
		{
			PlayerPrefsX.SetInt(RouteVisualiser.highlightRoutesPrefName, 1);
		}
		PlayerPrefsX.Save();
	}

	// Token: 0x06000B1C RID: 2844 RVA: 0x00059C3C File Offset: 0x00057E3C
	private void Start()
	{
		this._canvas = this._layout.canvas;
		this._canvas.gameObject.SetActive(false);
		this._layout.groupAlpha = 0f;
		if (PlayerPrefsX.HasKey("vSyncCount"))
		{
			QualitySettings.vSyncCount = PlayerPrefsX.GetInt("vSyncCount", 0);
		}
	}

	// Token: 0x06000B1D RID: 2845 RVA: 0x00059C97 File Offset: 0x00057E97
	[Button("Refresh Content")]
	private void DebugRefresh()
	{
		this.RemoveContent();
		this.CreateContent();
	}

	// Token: 0x06000B1E RID: 2846 RVA: 0x00059CA8 File Offset: 0x00057EA8
	private void SetVsyncEnabled(bool enabled)
	{
		if (enabled)
		{
			QualitySettings.vSyncCount = Mathf.Clamp(Mathf.RoundToInt((float)Screen.currentResolution.refreshRate / 60f), 1, 4);
		}
		else
		{
			QualitySettings.vSyncCount = 0;
		}
		PlayerPrefsX.SetInt("vSyncCount", QualitySettings.vSyncCount);
	}

	// Token: 0x06000B1F RID: 2847 RVA: 0x00059CF4 File Offset: 0x00057EF4
	private void CreateContent()
	{
		SettingsScreen.<>c__DisplayClass11_0 CS$<>8__locals1 = new SettingsScreen.<>c__DisplayClass11_0();
		CS$<>8__locals1.<>4__this = this;
		this._y = this._settings.topMargin;
		this.AddSubHeader("Graphics");
		this._resolutions.Clear();
		foreach (Resolution resolution in Screen.resolutions)
		{
			this._resolutions[resolution.ToString()] = resolution;
		}
		this.AddPicker("Resolution", this._resolutions.Keys.ToArray<string>(), Screen.currentResolution.ToString(), null, delegate(string resStr)
		{
			Resolution resolution2 = CS$<>8__locals1.<>4__this._resolutions[resStr];
			Screen.SetResolution(resolution2.width, resolution2.height, Screen.fullScreen, resolution2.refreshRate);
			CS$<>8__locals1.<>4__this.SetVsyncEnabled(QualitySettings.vSyncCount > 0);
		});
		this.AddToggle("VSync", QualitySettings.vSyncCount > 0, delegate(bool newVsyncEnabled)
		{
			CS$<>8__locals1.<>4__this.SetVsyncEnabled(newVsyncEnabled);
		});
		this.AddPrefToggle("High quality water", "highQualityWater", delegate(bool newVal)
		{
			GameCamera.instance.highQualityWaterReflections = newVal;
		});
		this.AddSubHeader("Narrative");
		this.AddPrefSlider("Reading speed", "readingSpeed");
		this.AddToggle("Allow mild profanity", Narrative.profanity, delegate(bool profanityEnabled)
		{
			Narrative.profanity = profanityEnabled;
		});
		this.AddSubHeader("Controls");
		string text = "Alternative gamepad controls";
		this.AddPrefToggle(text, "altControls", delegate(bool newUseAltControls)
		{
			MonoSingleton<GameInput>.instance.SetUseAltControls(newUseAltControls);
			CS$<>8__locals1.<>4__this.RefreshControlMappingTexts();
		});
		this.AddControlMappingInfo();
		this._remapControlsButton = this.AddButton("Remap keyboard controls", true, delegate
		{
			CS$<>8__locals1.<>4__this.SetRemapControlsPageVisible(true);
		});
		this.AddSubHeader("Gameplay & Accessibility");
		this.AddPrefToggle("Trip less when running to music", "dontMusicRunTrip", null);
		this.AddPrefToggle("Never flail while climbing", "neverSlip", null);
		this.AddToggle("Easier music rhythms", TrackBuilder.easyMode, delegate(bool easyModeOn)
		{
			CS$<>8__locals1.<>4__this._singleJumpButtonToggle.disabled = easyModeOn;
			TrackBuilder.easyMode = easyModeOn;
			CS$<>8__locals1.<>4__this._singleJumpButtonToggle.currentValue = Runner.singleJumpButtonOnly;
			CS$<>8__locals1.<>4__this._singleJumpButtonToggle.RefreshLayout();
		});
		this._singleJumpButtonToggle = this.AddToggle("Single jump button only", Runner.singleJumpButtonOnly, delegate(bool newVal)
		{
			Runner.singleJumpButtonOnly = newVal;
		});
		this._singleJumpButtonToggle.disabled = TrackBuilder.easyMode;
		this.AddPrefPicker("Weather and Environment", Enum.GetNames(typeof(Health.WeatherDifficulty)), Health.weatherDifficultyPrefName);
		this.AddPrefToggle("Lightning", WeatherSystem.lightingSettingPrefName, null);
		this.AddPrefToggle("Highlight routes when looking further", RouteVisualiser.highlightRoutesPrefName, null);
		this.AddSubHeader("Audio and vibration");
		CS$<>8__locals1.masterRange = new Range(-80f, 0f);
		CS$<>8__locals1.otherRange = new Range(-80f, 2f);
		CS$<>8__locals1.masterSlider = this.AddExposedVolumeSlider("Master Volume", "MasterVolume", CS$<>8__locals1.masterRange);
		CS$<>8__locals1.ambientSlider = this.AddExposedVolumeSlider("Ambient Music Volume", "AmbientMusicVolume", CS$<>8__locals1.otherRange);
		CS$<>8__locals1.rhythmSlider = this.AddExposedVolumeSlider("Rhythm Music Volume", "RhythmMusicVolume", CS$<>8__locals1.otherRange);
		CS$<>8__locals1.speechSlider = this.AddExposedVolumeSlider("Speech Volume", "SpeechVolume", CS$<>8__locals1.otherRange);
		this._y += 10f;
		this.AddButton("Reset volumes", true, delegate
		{
			CS$<>8__locals1.<>4__this.ResetVolume("MasterVolume", CS$<>8__locals1.masterSlider, CS$<>8__locals1.masterRange);
			CS$<>8__locals1.<>4__this.ResetVolume("AmbientMusicVolume", CS$<>8__locals1.ambientSlider, CS$<>8__locals1.otherRange);
			CS$<>8__locals1.<>4__this.ResetVolume("RhythmMusicVolume", CS$<>8__locals1.rhythmSlider, CS$<>8__locals1.otherRange);
			CS$<>8__locals1.<>4__this.ResetVolume("SpeechVolume", CS$<>8__locals1.speechSlider, CS$<>8__locals1.otherRange);
		});
		this.AddPrefToggle("Vibration", InputVibration.vibrationPrefName, null);
		this.AddSubHeader("Backup auto-saves");
		List<SaveInfo> backupSavesInfo = SaveLoadManager.backupSavesInfo;
		using (List<SaveInfo>.Enumerator enumerator = backupSavesInfo.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				SettingsScreen.<>c__DisplayClass11_1 CS$<>8__locals2 = new SettingsScreen.<>c__DisplayClass11_1();
				CS$<>8__locals2.CS$<>8__locals1 = CS$<>8__locals1;
				CS$<>8__locals2.saveInfo = enumerator.Current;
				TimeSpan timeSpan = DateTime.Now - CS$<>8__locals2.saveInfo.dateTime;
				string timeStr;
				if (timeSpan.TotalSeconds < 10.0)
				{
					timeStr = "Just now";
				}
				else if (timeSpan.TotalSeconds < 120.0)
				{
					timeStr = Math.Round(timeSpan.TotalSeconds).ToString() + " seconds ago";
				}
				else if (timeSpan.TotalMinutes < 120.0)
				{
					timeStr = Math.Round(timeSpan.TotalMinutes).ToString() + " minutes ago";
				}
				else
				{
					timeStr = CS$<>8__locals2.saveInfo.dateTime.ToString("h.mmtt \\o\\n MMMM dd, yyyy");
				}
				string text2 = "";
				if (CS$<>8__locals2.saveInfo.type == SaveLoadType.LevelStart)
				{
					text2 = "Arrived at new mountain range";
				}
				this.AddBackupSaveLoadOption(timeStr, text2, delegate
				{
					Dialogue instance = MonoSingleton<Dialogue>.instance;
					string text3 = "Load auto-save?";
					string text4 = "The current game in progress will be lost, are you sure you want to load the save from " + timeStr + "?";
					string text5 = "Continue";
					string text6 = "Cancel";
					bool flag = false;
					Dialogue.OnComplete onComplete;
					if ((onComplete = CS$<>8__locals2.<>9__14) == null)
					{
						onComplete = (CS$<>8__locals2.<>9__14 = delegate(Dialogue.Result result)
						{
							if (result == Dialogue.Result.Primary)
							{
								CS$<>8__locals2.CS$<>8__locals1.<>4__this.Hide();
								if (MonoSingleton<JournalController>.instance.visible)
								{
									MonoSingleton<JournalController>.instance.Close();
								}
								if (MonoSingleton<TitleScreen>.instance.visible)
								{
									MonoSingleton<TitleScreen>.instance.Hide();
								}
								Action action;
								if ((action = CS$<>8__locals2.<>9__15) == null)
								{
									action = (CS$<>8__locals2.<>9__15 = delegate
									{
										MonoSingleton<Launcher>.instance.LoadSaveType(CS$<>8__locals2.saveInfo.type);
									});
								}
								Blackout.FadeOut(action);
							}
						});
					}
					instance.Show(text3, text4, text5, text6, flag, onComplete);
				});
			}
		}
		if (backupSavesInfo.Count == 0)
		{
			this.AddInfoText("No backup auto-saves available");
		}
		if (!SteamUtils.IsSteamRunningOnSteamDeck())
		{
			this.AddSubHeader("Feedback");
			this.AddButton("Send feedback", true, delegate
			{
				CS$<>8__locals1.<>4__this.Hide();
				if (MonoSingleton<JournalController>.instance.visible)
				{
					MonoSingleton<JournalController>.instance.Close();
				}
				MonoSingleton<FeedbackController>.instance.Show(0.5f);
			});
		}
		this.AddSubHeader("Reset");
		this.AddButton("Reset game completely", true, delegate
		{
			Dialogue instance2 = MonoSingleton<Dialogue>.instance;
			string text7 = "Reset game completely?";
			string text8 = "The game in progress will be restarted, tutorial help will be restarted, any New Game+ will be reset. Any custom settings will be kept.";
			string text9 = "Reset";
			string text10 = "Cancel";
			bool flag2 = true;
			Dialogue.OnComplete onComplete2;
			if ((onComplete2 = CS$<>8__locals1.<>9__16) == null)
			{
				onComplete2 = (CS$<>8__locals1.<>9__16 = delegate(Dialogue.Result result)
				{
					if (result == Dialogue.Result.Primary)
					{
						CS$<>8__locals1.<>4__this.Hide();
						if (MonoSingleton<JournalController>.instance.visible)
						{
							MonoSingleton<JournalController>.instance.Close();
						}
						if (MonoSingleton<TitleScreen>.instance.visible)
						{
							MonoSingleton<Launcher>.instance.ResetCompletely();
							return;
						}
						Blackout.FadeOut(delegate
						{
							MonoSingleton<Launcher>.instance.ResetCompletely();
						});
					}
				});
			}
			instance2.Show(text7, text8, text9, text10, flag2, onComplete2);
		});
		this.AddButton("Restart tutorial", true, delegate
		{
			MonoSingleton<Dialogue>.instance.Show("Reset tutorial?", "Any tutorial help will be reset. Recommended when restarting a game to present to a new player, without losing New Game+ benefits.", "Reset", "Cancel", false, delegate(Dialogue.Result result)
			{
				if (result == Dialogue.Result.Primary)
				{
					Tutorial.ResetAll();
				}
			});
		});
		this._y += this._settings.marginAboveDoneButton;
		this.AddButton("Done", false, delegate
		{
			CS$<>8__locals1.<>4__this.Hide();
		});
		this._panel.height = this._y + this._settings.bottomMargin;
		this._currentControl = this._controls[0];
		this._currentControl.highlighted = true;
	}

	// Token: 0x06000B20 RID: 2848 RVA: 0x0005A2E4 File Offset: 0x000584E4
	private void CreateRemapControlsContent()
	{
		this._y = this._settings.topMargin;
		this._y += 20f;
		this.AddKeyAssignControl(MonoSingleton<GameInput>.instance.mapping.moveLeft, null);
		this.AddKeyAssignControl(MonoSingleton<GameInput>.instance.mapping.moveRight, null);
		this.AddKeyAssignControl(MonoSingleton<GameInput>.instance.mapping.moveUp, null);
		this.AddKeyAssignControl(MonoSingleton<GameInput>.instance.mapping.moveDown, null);
		this.AddKeyAssignControl(MonoSingleton<GameInput>.instance.mapping.jump, null);
		this.AddKeyAssignControl(MonoSingleton<GameInput>.instance.mapping.jumpSpecial, "Secondary music jump");
		this.AddKeyAssignControl(MonoSingleton<GameInput>.instance.mapping.sprint, null);
		this.AddKeyAssignControl(MonoSingleton<GameInput>.instance.mapping.rest, null);
		this.AddKeyAssignControl(MonoSingleton<GameInput>.instance.mapping.slipRecover, "Recover from climbing flail");
		this.AddKeyAssignControl(MonoSingleton<GameInput>.instance.mapping.zoomOut, null);
		this.AddKeyAssignControl(MonoSingleton<GameInput>.instance.mapping.zoomIn, null);
		this.AddKeyAssignControl(MonoSingleton<GameInput>.instance.mapping.lookFurther, null);
		this.AddKeyAssignControl(MonoSingleton<GameInput>.instance.mapping.selectChoice, null);
		this.AddKeyAssignControl(MonoSingleton<GameInput>.instance.mapping.panLeft, null);
		this.AddKeyAssignControl(MonoSingleton<GameInput>.instance.mapping.panRight, null);
		this.AddKeyAssignControl(MonoSingleton<GameInput>.instance.mapping.panUp, null);
		this.AddKeyAssignControl(MonoSingleton<GameInput>.instance.mapping.panDown, null);
		this.AddKeyAssignControl(MonoSingleton<GameInput>.instance.mapping.resetCameraToPlayer, null);
		this.AddKeyAssignControl(MonoSingleton<GameInput>.instance.mapping.selectUp, null);
		this.AddKeyAssignControl(MonoSingleton<GameInput>.instance.mapping.selectDown, null);
		this.AddKeyAssignControl(MonoSingleton<GameInput>.instance.mapping.selectLeft, null);
		this.AddKeyAssignControl(MonoSingleton<GameInput>.instance.mapping.selectRight, null);
		this.AddKeyAssignControl(MonoSingleton<GameInput>.instance.mapping.showMaps, "Toggle maps");
		this.AddKeyAssignControl(MonoSingleton<GameInput>.instance.mapping.prevMap, null);
		this.AddKeyAssignControl(MonoSingleton<GameInput>.instance.mapping.nextMap, null);
		this.AddKeyAssignControl(MonoSingleton<GameInput>.instance.mapping.prevSection, null);
		this.AddKeyAssignControl(MonoSingleton<GameInput>.instance.mapping.nextSection, null);
		this.AddKeyAssignControl(MonoSingleton<GameInput>.instance.mapping.togglePhotoMode, null);
		this.AddKeyAssignControl(MonoSingleton<GameInput>.instance.mapping.takePhoto, null);
		this.AddKeyAssignControl(MonoSingleton<GameInput>.instance.mapping.photoModeOptions, null);
		this.AddKeyAssignControl(MonoSingleton<GameInput>.instance.mapping.revealPhoto, null);
		this._y += 20f;
		this._resetAllControlsButton = this.AddButton("Reset all", true, delegate
		{
			MonoSingleton<GameInput>.instance.mapping.Reset();
			ActionIconView.ForceRefreshAll();
			this.RemoveContent();
			this.CreateRemapControlsContent();
			this._resetAllControlsButton.highlighted = true;
			this._currentControl = this._resetAllControlsButton;
			GameInput.SaveCustomMapping();
		});
		this._y += this._settings.marginAboveDoneButton;
		this.AddButton("Done", false, delegate
		{
			this.SetRemapControlsPageVisible(false);
		});
		this._panel.height = this._y + this._settings.bottomMargin;
		this._currentControl = this._controls[0];
		this._currentControl.highlighted = true;
	}

	// Token: 0x06000B21 RID: 2849 RVA: 0x0005A668 File Offset: 0x00058868
	private void RemoveContent()
	{
		foreach (SLayout slayout in this._otherContent)
		{
			slayout.GetComponent<Prototype>().ReturnToPool();
		}
		this._otherContent.Clear();
		foreach (SettingControl settingControl in this._controls)
		{
			settingControl.prototype.ReturnToPool();
		}
		this._controlMappingInfo.gameObject.SetActive(false);
		this._controls.Clear();
		this._resetAllControlsButton = null;
	}

	// Token: 0x06000B22 RID: 2850 RVA: 0x0005A730 File Offset: 0x00058930
	private void SetRemapControlsPageVisible(bool visible)
	{
		this.RemoveContent();
		this._title.textMeshPro.text = (visible ? "Controls" : "Settings");
		this._panelSlideSpeed = 0f;
		this._panel.topY = this._panelDefaultY;
		this._panelTargetY = this._panel.y;
		if (visible)
		{
			this.CreateRemapControlsContent();
			GameInput.PushControlStack(this._remapControlsPanelBackItem);
			return;
		}
		this.CreateContent();
		GameInput.PopControlStack(this._remapControlsPanelBackItem, true);
		this._remapControlsButton.highlighted = true;
		this._currentControl = this._remapControlsButton;
	}

	// Token: 0x06000B23 RID: 2851 RVA: 0x0005A7D0 File Offset: 0x000589D0
	private void AddSubHeader(string text)
	{
		this._y += this._settings.subHeadingMarginAbove;
		SLayout slayout = this._subHeadingPrototype.Instantiate<SLayout>(null);
		slayout.textMeshPro.text = text;
		slayout.topY = this._y;
		slayout.centerX = this._panel.middleX;
		this._otherContent.Add(slayout);
		this._y += this._settings.subHeadingHeight;
	}

	// Token: 0x06000B24 RID: 2852 RVA: 0x0005A850 File Offset: 0x00058A50
	private SLayout AddInfoText(string text)
	{
		this._y += this._settings.infoTextMarginAbove;
		SLayout slayout = this._infoTextPrototype.Instantiate<SLayout>(null);
		slayout.textMeshPro.text = text;
		slayout.topY = this._y;
		slayout.centerX = this._panel.middleX;
		this._otherContent.Add(slayout);
		this._y += this._settings.infoTextHeight;
		return slayout;
	}

	// Token: 0x06000B25 RID: 2853 RVA: 0x0005A8D0 File Offset: 0x00058AD0
	private void AddPicker(string label, string[] options, string initialValue, Action<string> onChange, Action<string> onTrigger)
	{
		SettingPicker settingPicker = this._pickerPrototype.Instantiate<SettingPicker>(null);
		settingPicker.Setup(label, options, initialValue, onChange, onTrigger);
		settingPicker.layout.topY = this._y;
		settingPicker.layout.centerX = this._panel.middleX;
		this._controls.Add(settingPicker);
		this._y += this._settings.controlHeight;
	}

	// Token: 0x06000B26 RID: 2854 RVA: 0x0005A944 File Offset: 0x00058B44
	private void AddPrefPicker(string label, string[] options, string prefName)
	{
		int num = Mathf.Clamp(PlayerPrefsX.GetInt(prefName, 0), 0, options.Length - 1);
		string text = options[num];
		this.AddPicker(label, options, text, delegate(string newValStr)
		{
			int num2 = Mathf.Clamp(Array.IndexOf<string>(options, newValStr), 0, options.Length - 1);
			PlayerPrefsX.SetInt(prefName, num2);
		}, null);
	}

	// Token: 0x06000B27 RID: 2855 RVA: 0x0005A9A8 File Offset: 0x00058BA8
	private SettingToggle AddToggle(string label, bool initialValue, Action<bool> onChange)
	{
		SettingToggle settingToggle = this._togglePrototype.Instantiate<SettingToggle>(null);
		settingToggle.Setup(label, initialValue, onChange, null);
		settingToggle.layout.topY = this._y;
		settingToggle.layout.centerX = this._panel.middleX;
		this._controls.Add(settingToggle);
		this._y += this._settings.controlHeight;
		return settingToggle;
	}

	// Token: 0x06000B28 RID: 2856 RVA: 0x0005AA18 File Offset: 0x00058C18
	private SettingToggle AddPrefToggle(string label, string prefName, Action<bool> onChange = null)
	{
		int @int = PlayerPrefsX.GetInt(prefName, 0);
		return this.AddToggle(label, @int != 0, delegate(bool newVal)
		{
			PlayerPrefsX.SetInt(prefName, newVal ? 1 : 0);
			if (onChange != null)
			{
				onChange(newVal);
			}
		});
	}

	// Token: 0x06000B29 RID: 2857 RVA: 0x0005AA60 File Offset: 0x00058C60
	private SettingSlider AddSlider(string label, float initialValue, Action<float> onChange)
	{
		SettingSlider settingSlider = this._sliderPrototype.Instantiate<SettingSlider>(null);
		settingSlider.Setup(label, initialValue, onChange, null);
		settingSlider.layout.topY = this._y;
		settingSlider.layout.centerX = this._panel.middleX;
		this._controls.Add(settingSlider);
		this._y += this._settings.controlHeight;
		return settingSlider;
	}

	// Token: 0x06000B2A RID: 2858 RVA: 0x0005AAD0 File Offset: 0x00058CD0
	private void AddPrefSlider(string label, string prefName)
	{
		float @float = PlayerPrefsX.GetFloat(prefName, 0.5f);
		this.AddSlider(label, @float, delegate(float newVal)
		{
			PlayerPrefsX.SetFloat(prefName, newVal);
		});
	}

	// Token: 0x06000B2B RID: 2859 RVA: 0x0005AB10 File Offset: 0x00058D10
	private void AddControlMappingInfo()
	{
		this._controlMappingInfo.gameObject.SetActive(true);
		this._controlMappingInfo.topY = this._y;
		this._controlMappingInfo.centerX = this._panel.middleX;
		this._y += this._controlMappingInfo.height;
		this.RefreshControlMappingTexts();
	}

	// Token: 0x06000B2C RID: 2860 RVA: 0x0005AB74 File Offset: 0x00058D74
	private void RefreshControlMappingTexts()
	{
		bool flag = false;
		if (MonoSingleton<TestMenu>.instance != null && MonoSingleton<TestMenu>.instance.forceSwitchController)
		{
			flag = true;
		}
		if (GameInput.altControls)
		{
			if (flag)
			{
				this._jumpControlMappingText.text = "A";
				this._interactControlMappingText.text = "Y";
				this._sprintControlMappingText.text = "B";
				this._restControlMappingText.text = "X";
				return;
			}
			this._jumpControlMappingText.text = "A";
			this._interactControlMappingText.text = "X";
			this._sprintControlMappingText.text = "B";
			this._restControlMappingText.text = "Y";
			return;
		}
		else
		{
			if (flag)
			{
				this._jumpControlMappingText.text = "X";
				this._interactControlMappingText.text = "A";
				this._sprintControlMappingText.text = "B";
				this._restControlMappingText.text = "Y";
				return;
			}
			this._jumpControlMappingText.text = "Y";
			this._interactControlMappingText.text = "A";
			this._sprintControlMappingText.text = "B";
			this._restControlMappingText.text = "X";
			return;
		}
	}

	// Token: 0x06000B2D RID: 2861 RVA: 0x0005ACB4 File Offset: 0x00058EB4
	private SettingSlider AddExposedVolumeSlider(string label, string exposedVolumeName, Range dbRange)
	{
		string prefKey = "AudioExposedPref_" + exposedVolumeName;
		float num;
		if (PlayerPrefsX.HasKey(prefKey))
		{
			float @float = PlayerPrefsX.GetFloat(prefKey, 0f);
			AudioController.instance.audioMixer.SetFloat(exposedVolumeName, @float);
			num = @float;
		}
		else
		{
			AudioController.instance.audioMixer.GetFloat(exposedVolumeName, out num);
		}
		float num2 = this.VolumeDbToNorm(num, dbRange);
		return this.AddSlider(label, num2, delegate(float newValLinearNorm)
		{
			float num3 = this.VolumeNormToDb(newValLinearNorm, dbRange);
			PlayerPrefsX.SetFloat(prefKey, num3);
			AudioController.instance.audioMixer.SetFloat(exposedVolumeName, num3);
		});
	}

	// Token: 0x06000B2E RID: 2862 RVA: 0x0005AD65 File Offset: 0x00058F65
	private float Linearise(float db)
	{
		return Mathf.Pow(Mathf.Abs(db), this.volumePowerCurve) * Mathf.Sign(db);
	}

	// Token: 0x06000B2F RID: 2863 RVA: 0x0005AD80 File Offset: 0x00058F80
	private float VolumeDbToNorm(float db, Range dbRange)
	{
		float num = this.Linearise(dbRange.min);
		float num2 = this.Linearise(dbRange.max);
		float num3 = this.Linearise(db);
		return Mathf.InverseLerp(num, num2, num3);
	}

	// Token: 0x06000B30 RID: 2864 RVA: 0x0005ADB8 File Offset: 0x00058FB8
	private float VolumeNormToDb(float normVal, Range dbRange)
	{
		float num = this.Linearise(dbRange.min);
		float num2 = this.Linearise(dbRange.max);
		float num3 = Mathf.Lerp(num, num2, normVal);
		return Mathf.Pow(Mathf.Abs(num3), 1f / this.volumePowerCurve) * Mathf.Sign(num3);
	}

	// Token: 0x06000B31 RID: 2865 RVA: 0x0005AE04 File Offset: 0x00059004
	private void ResetVolume(string exposedVolumeName, SettingSlider slider, Range dbRange)
	{
		string text = "AudioExposedPref_" + exposedVolumeName;
		AudioController.instance.audioMixer.ClearFloat(exposedVolumeName);
		PlayerPrefsX.DeleteKey(text);
		base.StartCoroutine(this.ResetVolumeWithFrameDelay(exposedVolumeName, slider, dbRange));
	}

	// Token: 0x06000B32 RID: 2866 RVA: 0x0005AE37 File Offset: 0x00059037
	private IEnumerator ResetVolumeWithFrameDelay(string exposedVolumeName, SettingSlider slider, Range dbRange)
	{
		yield return null;
		float num;
		AudioController.instance.audioMixer.GetFloat(exposedVolumeName, out num);
		float num2 = this.Linearise(dbRange.min);
		float num3 = this.Linearise(dbRange.max);
		float num4 = this.Linearise(num);
		float num5 = Mathf.InverseLerp(num2, num3, num4);
		slider.currentValue = num5;
		yield break;
	}

	// Token: 0x06000B33 RID: 2867 RVA: 0x0005AE5C File Offset: 0x0005905C
	private SettingButton AddButton(string label, bool wideAndNarrow, Action onTrigger)
	{
		SettingButton settingButton = this._buttonPrototype.Instantiate<SettingButton>(null);
		settingButton.Setup(label, onTrigger);
		settingButton.layout.size = (wideAndNarrow ? new Vector2(400f, 66f) : new Vector2(200f, 90f));
		settingButton.layout.topY = this._y;
		settingButton.layout.centerX = this._panel.middleX;
		this._controls.Add(settingButton);
		this._y += this._settings.controlHeight;
		return settingButton;
	}

	// Token: 0x06000B34 RID: 2868 RVA: 0x0005AEF8 File Offset: 0x000590F8
	private void AddBackupSaveLoadOption(string label, string extraLabel, Action onTrigger)
	{
		SettingButtonWithLabels settingButtonWithLabels = this._backupSaveLoadOptPrototype.Instantiate<SettingButtonWithLabels>(null);
		settingButtonWithLabels.Setup(label, extraLabel, onTrigger);
		settingButtonWithLabels.layout.topY = this._y;
		settingButtonWithLabels.layout.centerX = this._panel.middleX;
		this._controls.Add(settingButtonWithLabels);
		this._y += this._settings.controlHeight;
	}

	// Token: 0x06000B35 RID: 2869 RVA: 0x0005AF68 File Offset: 0x00059168
	private void AddKeyAssignControl(PlayerAction action, string labelOverride = null)
	{
		SettingKeyAssign settingKeyAssign = this._keyAssignPrototype.Instantiate<SettingKeyAssign>(null);
		settingKeyAssign.Setup(action, labelOverride);
		settingKeyAssign.layout.topY = this._y;
		settingKeyAssign.layout.centerX = this._panel.middleX;
		this._controls.Add(settingKeyAssign);
		this._y += this._settings.controlHeight;
	}

	// Token: 0x06000B36 RID: 2870 RVA: 0x0005AFD8 File Offset: 0x000591D8
	private bool RevertCurrentPickerIfNecessary()
	{
		SettingPicker settingPicker = this._currentControl as SettingPicker;
		if (settingPicker != null && settingPicker.requiresConfirmation)
		{
			settingPicker.Revert();
			return true;
		}
		return false;
	}

	// Token: 0x06000B37 RID: 2871 RVA: 0x0005B00C File Offset: 0x0005920C
	private void Update()
	{
		if (this.allowInput)
		{
			if (GameInput.Back(this))
			{
				GameInput.ClearInputState();
				if (!this.RevertCurrentPickerIfNecessary())
				{
					this.Hide();
				}
			}
			if (GameInput.Back(this._remapControlsPanelBackItem))
			{
				this.SetRemapControlsPageVisible(false);
			}
			if (GameInput.selectDown)
			{
				GameInput.ClearInputState();
				this.SelectPrevNextControl(1);
			}
			if (GameInput.selectUp)
			{
				GameInput.ClearInputState();
				this.SelectPrevNextControl(-1);
			}
			if (GameInput.selectRight)
			{
				this._currentControl.LeftRight(1);
			}
			if (GameInput.selectLeft)
			{
				this._currentControl.LeftRight(-1);
			}
			if (GameInput.selectMenuItem)
			{
				GameInput.ClearInputState();
				this._currentControl.Trigger();
			}
		}
		if (this.visible && !this._layout.isAnimating)
		{
			this._panel.y = Mathf.SmoothDamp(this._panel.y, this._panelTargetY, ref this._panelSlideSpeed, this._settings.panelSlideSmoothTime, float.MaxValue, Time.unscaledDeltaTime);
		}
	}

	// Token: 0x06000B38 RID: 2872 RVA: 0x0005B108 File Offset: 0x00059308
	private void SelectPrevNextControl(int dir)
	{
		int num = this._controls.IndexOf(this._currentControl);
		int num2 = Mathf.Clamp(num + dir, 0, this._controls.Count - 1);
		if (num2 == num)
		{
			return;
		}
		while (this._controls[num2].disabled)
		{
			if (num2 <= 0 || num2 >= this._controls.Count - 1)
			{
				return;
			}
			num2 += dir;
		}
		this.RevertCurrentPickerIfNecessary();
		this._currentControl.highlighted = false;
		this._currentControl = this._controls[num2];
		this._currentControl.highlighted = true;
		float y = this._currentControl.layout.ConvertPositionToTarget(this._currentControl.layout.middle, null).y;
		float num3 = this._settings.controlPosYRangeOnScreen.Clamp(y);
		if (Mathf.RoundToInt(num3) != Mathf.RoundToInt(y))
		{
			float num4 = num3 - y;
			if (Mathf.Sign(num4) == (float)dir)
			{
				this._panelTargetY = this._panel.y - num4;
			}
		}
	}

	// Token: 0x04000D39 RID: 3385
	[Disable]
	public bool visible;

	// Token: 0x04000D3A RID: 3386
	public float volumePowerCurve = 0.5f;

	// Token: 0x04000D3B RID: 3387
	private Canvas _canvas;

	// Token: 0x04000D3C RID: 3388
	private float _y;

	// Token: 0x04000D3D RID: 3389
	private float _panelDefaultY;

	// Token: 0x04000D3E RID: 3390
	private SettingControl _currentControl;

	// Token: 0x04000D3F RID: 3391
	private SettingToggle _singleJumpButtonToggle;

	// Token: 0x04000D40 RID: 3392
	private float _panelTargetY;

	// Token: 0x04000D41 RID: 3393
	private float _panelSlideSpeed;

	// Token: 0x04000D42 RID: 3394
	private List<SettingControl> _controls = new List<SettingControl>();

	// Token: 0x04000D43 RID: 3395
	private List<SLayout> _otherContent = new List<SLayout>();

	// Token: 0x04000D44 RID: 3396
	private Dictionary<string, Resolution> _resolutions = new Dictionary<string, Resolution>();

	// Token: 0x04000D45 RID: 3397
	private string _remapControlsPanelBackItem = "remapControlsPanelBackItem";

	// Token: 0x04000D46 RID: 3398
	private SettingButton _remapControlsButton;

	// Token: 0x04000D47 RID: 3399
	private SettingButton _resetAllControlsButton;

	// Token: 0x04000D48 RID: 3400
	[SerializeField]
	private SLayout _layout;

	// Token: 0x04000D49 RID: 3401
	[SerializeField]
	private SLayout _panel;

	// Token: 0x04000D4A RID: 3402
	[SerializeField]
	private SLayout _title;

	// Token: 0x04000D4B RID: 3403
	[SerializeField]
	private Prototype _subHeadingPrototype;

	// Token: 0x04000D4C RID: 3404
	[SerializeField]
	private Prototype _infoTextPrototype;

	// Token: 0x04000D4D RID: 3405
	[SerializeField]
	private Prototype _pickerPrototype;

	// Token: 0x04000D4E RID: 3406
	[SerializeField]
	private Prototype _togglePrototype;

	// Token: 0x04000D4F RID: 3407
	[SerializeField]
	private Prototype _sliderPrototype;

	// Token: 0x04000D50 RID: 3408
	[SerializeField]
	private Prototype _buttonPrototype;

	// Token: 0x04000D51 RID: 3409
	[SerializeField]
	private Prototype _backupSaveLoadOptPrototype;

	// Token: 0x04000D52 RID: 3410
	[SerializeField]
	private Prototype _keyAssignPrototype;

	// Token: 0x04000D53 RID: 3411
	[SerializeField]
	private SLayout _controlMappingInfo;

	// Token: 0x04000D54 RID: 3412
	[SerializeField]
	private TextMeshProUGUI _jumpControlMappingText;

	// Token: 0x04000D55 RID: 3413
	[SerializeField]
	private TextMeshProUGUI _interactControlMappingText;

	// Token: 0x04000D56 RID: 3414
	[SerializeField]
	private TextMeshProUGUI _sprintControlMappingText;

	// Token: 0x04000D57 RID: 3415
	[SerializeField]
	private TextMeshProUGUI _restControlMappingText;

	// Token: 0x04000D58 RID: 3416
	[SerializeField]
	private SettingsScreenSettings _settings;
}
