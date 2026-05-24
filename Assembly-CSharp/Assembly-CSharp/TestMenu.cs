using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using ActionIcon;
using UnityEngine;

// Token: 0x020000FA RID: 250
public class TestMenu : MonoSingleton<TestMenu>
{
	// Token: 0x1700020F RID: 527
	// (get) Token: 0x06000801 RID: 2049 RVA: 0x00046535 File Offset: 0x00044735
	// (set) Token: 0x06000802 RID: 2050 RVA: 0x00046540 File Offset: 0x00044740
	private bool hudEnabled
	{
		get
		{
			return this._hudEnabled;
		}
		set
		{
			if (this._hudEnabled != value)
			{
				GameObject[] hudItems = this._hudItems;
				for (int i = 0; i < hudItems.Length; i++)
				{
					hudItems[i].SetActive(value);
				}
				this._hudEnabled = value;
			}
		}
	}

	// Token: 0x17000210 RID: 528
	// (get) Token: 0x06000803 RID: 2051 RVA: 0x0004657B File Offset: 0x0004477B
	// (set) Token: 0x06000804 RID: 2052 RVA: 0x00046584 File Offset: 0x00044784
	private bool allUIEnabled
	{
		get
		{
			return this._allUIEnabled;
		}
		set
		{
			if (this._allUIEnabled != value)
			{
				this._allUIEnabled = value;
				if (this._gameUICamera == null)
				{
					GameObject gameObject = GameObject.Find("GameUICamera");
					if (gameObject == null)
					{
						return;
					}
					this._gameUICamera = gameObject.GetComponent<Camera>();
					if (this._gameUICamera == null)
					{
						return;
					}
				}
				this._gameUICamera.enabled = value;
			}
		}
	}

	// Token: 0x06000805 RID: 2053 RVA: 0x000465EB File Offset: 0x000447EB
	private void Start()
	{
		this._allGameSetups = Resources.FindObjectsOfTypeAll<GameSetup>();
	}

	// Token: 0x06000806 RID: 2054 RVA: 0x000465F8 File Offset: 0x000447F8
	private void AddToggleableGameObject(GameObject go)
	{
		List<TestMenu.Option> currOptions = this.currOptions;
		TestMenu.Option option = new TestMenu.Option();
		option.context = go;
		option.label = go.name;
		option.valueStringFunc = delegate(TestMenu.Option opt)
		{
			if (!((GameObject)opt.context).activeSelf)
			{
				return "OFF";
			}
			return "ON";
		};
		option.action = delegate(TestMenu.Option obj)
		{
			((GameObject)obj.context).SetActive(!((GameObject)obj.context).activeSelf);
		};
		currOptions.Add(option);
	}

	// Token: 0x06000807 RID: 2055 RVA: 0x00046674 File Offset: 0x00044874
	private void AddToggleableCategoryOfComponent<T>(string label) where T : Component
	{
		T[] allT = Resources.FindObjectsOfTypeAll<T>();
		List<TestMenu.Option> currOptions = this.currOptions;
		TestMenu.Option option = new TestMenu.Option();
		option.label = label;
		option.context = allT;
		option.enabled = true;
		option.valueStringFunc = delegate(TestMenu.Option opt)
		{
			if (!opt.enabled)
			{
				return "OFF";
			}
			return "ON";
		};
		option.action = delegate(TestMenu.Option opt)
		{
			if (allT == null)
			{
				return;
			}
			opt.enabled = !opt.enabled;
			T[] array = (T[])opt.context;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].gameObject.SetActive(opt.enabled);
			}
		};
		currOptions.Add(option);
	}

	// Token: 0x06000808 RID: 2056 RVA: 0x000466F4 File Offset: 0x000448F4
	public void SetupOptions()
	{
		this._optionMenusStack.Clear();
		this._optionMenusStack.Push(new TestMenu.MenuState
		{
			options = new List<TestMenu.Option>(),
			idx = 0
		});
		if (MonoSingleton<BuildSetupManager>.instance.setup.trailerFeatures)
		{
			this.AddTrailerOptions();
		}
		else
		{
			this.AddTestAndPerfOptions();
		}
		this.AddGameSetups();
	}

	// Token: 0x06000809 RID: 2057 RVA: 0x00046754 File Offset: 0x00044954
	private void AddTrailerOptions()
	{
		this.currOptions.Add(new TestMenu.Option
		{
			label = "HUD",
			valueStringFunc = delegate(TestMenu.Option opt)
			{
				if (!this.hudEnabled)
				{
					return "OFF";
				}
				return "ON";
			},
			action = delegate(TestMenu.Option opt)
			{
				this.hudEnabled = !this.hudEnabled;
			}
		});
		this.currOptions.Add(new TestMenu.Option
		{
			label = "All UI",
			valueStringFunc = delegate(TestMenu.Option opt)
			{
				if (!this.allUIEnabled)
				{
					return "OFF";
				}
				return "ON";
			},
			action = delegate(TestMenu.Option opt)
			{
				this.allUIEnabled = !this.allUIEnabled;
			}
		});
		List<TestMenu.Option> currOptions = this.currOptions;
		TestMenu.Option option = new TestMenu.Option();
		option.label = "Latest save";
		option.valueStringFunc = (TestMenu.Option opt) => "RELOAD";
		option.action = delegate(TestMenu.Option opt)
		{
			this.SetVisible(false);
			MonoSingleton<Launcher>.instance.LoadSaveType(SaveLoadType.Latest);
		};
		currOptions.Add(option);
		this.currOptions.Add(new TestMenu.Option
		{
			label = "Nintendo Switch UI",
			valueStringFunc = delegate(TestMenu.Option opt)
			{
				if (!this.forceSwitchController)
				{
					return "OFF";
				}
				return "ON";
			},
			action = delegate(TestMenu.Option opt)
			{
				this.forceSwitchController = !this.forceSwitchController;
				ActionIconUtils.debugFakeSwitchController = this.forceSwitchController;
				GameInput.RefreshActiveDevice(true);
			}
		});
		List<TestMenu.Option> currOptions2 = this.currOptions;
		TestMenu.Option option2 = new TestMenu.Option();
		option2.label = "Cam dist";
		option2.valueStringFunc = (TestMenu.Option opt) => Mathf.RoundToInt(GameCamera.instance.debugDistanceScalar * 100f).ToString() + "%";
		option2.leftRightAction = delegate(int dir, TestMenu.Option opt)
		{
			GameCamera instance = GameCamera.instance;
			instance.debugDistanceScalar = Mathf.Clamp(instance.debugDistanceScalar + 0.1f * (float)dir, 0.1f, 3f);
		};
		currOptions2.Add(option2);
		this.currOptions.Add(new TestMenu.Option
		{
			label = "Game speed",
			valueStringFunc = (TestMenu.Option opt) => this._gameSpeedPercent.ToString() + "%",
			leftRightAction = delegate(int dir, TestMenu.Option opt)
			{
				this._gameSpeedPercent = Mathf.Clamp(this._gameSpeedPercent + 10 * dir, 10, 200);
				if (this._gameSpeedPercent == 100)
				{
					Game.instance.RemoveTimeScalar(Game.TimeScalar.TestMenuGameSpeed);
					return;
				}
				Game.instance.SetTimeScalar(Game.TimeScalar.TestMenuGameSpeed, (float)this._gameSpeedPercent / 100f);
			}
		});
		List<TestMenu.Option> list = new List<TestMenu.Option>();
		list.Add(new TestMenu.Option
		{
			label = "Start hour",
			valueStringFunc = (TestMenu.Option opt) => this._timelapseStartHour.ToString(),
			leftRightAction = delegate(int dir, TestMenu.Option opt)
			{
				this._timelapseStartHour = (this._timelapseStartHour + dir + 24) % 24;
			}
		});
		list.Add(new TestMenu.Option
		{
			label = "End hour",
			valueStringFunc = (TestMenu.Option opt) => this._timelapseEndHour.ToString(),
			leftRightAction = delegate(int dir, TestMenu.Option opt)
			{
				this._timelapseEndHour = (this._timelapseEndHour + dir + 24) % 24;
			}
		});
		list.Add(new TestMenu.Option
		{
			label = "Duration",
			valueStringFunc = (TestMenu.Option opt) => this._timelapseDuration.ToString("F1") + " seconds",
			leftRightAction = delegate(int dir, TestMenu.Option opt)
			{
				this._timelapseDuration = Mathf.Max(this._timelapseDuration + 0.5f * (float)dir, 0f);
			}
		});
		List<TestMenu.Option> list2 = list;
		TestMenu.Option option3 = new TestMenu.Option();
		option3.label = "RUN";
		option3.valueStringFunc = (TestMenu.Option opt) => ">>>";
		option3.action = delegate(TestMenu.Option opt)
		{
			this.SetVisible(false);
			base.StartCoroutine(this.TrailerTimelapseCoroutine());
		};
		list2.Add(option3);
		List<TestMenu.Option> currOptions3 = this.currOptions;
		TestMenu.Option option4 = new TestMenu.Option();
		option4.label = "Timelapse";
		option4.valueStringFunc = (TestMenu.Option opt) => ">";
		option4.submenu = list;
		currOptions3.Add(option4);
		List<TestMenu.Option> list3 = new List<TestMenu.Option>();
		list3.Add(new TestMenu.Option
		{
			label = "Time scalar",
			valueStringFunc = (TestMenu.Option opt) => this._cameraMoveTimeScalar.ToString("F1") + "x",
			leftRightAction = delegate(int dir, TestMenu.Option opt)
			{
				this._cameraMoveTimeScalar = Mathf.Clamp(this._cameraMoveTimeScalar + (float)dir * 0.1f, 0.1f, 2f);
			}
		});
		list3.Add(new TestMenu.Option
		{
			label = "Speed scalar",
			valueStringFunc = (TestMenu.Option opt) => this._cameraMoveSpeedScalar.ToString(),
			leftRightAction = delegate(int dir, TestMenu.Option opt)
			{
				this._cameraMoveSpeedScalar = Mathf.Clamp(this._cameraMoveSpeedScalar + (float)dir * 0.1f, 0.1f, 5f);
			}
		});
		list3.Add(new TestMenu.Option
		{
			label = "Timelapse",
			valueStringFunc = delegate(TestMenu.Option opt)
			{
				if (!this._cameraMoveHasTimelapse)
				{
					return "OFF";
				}
				return "ON";
			},
			leftRightAction = delegate(int dir, TestMenu.Option opt)
			{
				this._cameraMoveHasTimelapse = !this._cameraMoveHasTimelapse;
			}
		});
		list3.Add(new TestMenu.Option
		{
			label = "Timelapse speed",
			valueStringFunc = (TestMenu.Option opt) => this._cameraMoveTimelapseSpeed.ToString("F1"),
			leftRightAction = delegate(int dir, TestMenu.Option opt)
			{
				this._cameraMoveTimelapseSpeed = Mathf.Clamp(this._cameraMoveTimelapseSpeed + (float)dir * 0.1f, 0.1f, 10f);
			}
		});
		list3.Add(new TestMenu.Option
		{
			label = "All levels high LOD",
			valueStringFunc = delegate(TestMenu.Option opt)
			{
				if (!this.allHighLOD)
				{
					return "OFF";
				}
				return "ON";
			},
			leftRightAction = delegate(int dir, TestMenu.Option opt)
			{
				this.allHighLOD = !this.allHighLOD;
			}
		});
		List<TestMenu.Option> list4 = list3;
		TestMenu.Option option5 = new TestMenu.Option();
		option5.label = "GO";
		option5.valueStringFunc = (TestMenu.Option opt) => ">>>";
		option5.action = delegate(TestMenu.Option opt)
		{
			this.SetVisible(false);
			base.StartCoroutine(this.CameraSwoopCoroutine());
		};
		list4.Add(option5);
		List<TestMenu.Option> currOptions4 = this.currOptions;
		TestMenu.Option option6 = new TestMenu.Option();
		option6.label = "Camera swoop";
		option6.valueStringFunc = (TestMenu.Option opt) => ">";
		option6.submenu = list3;
		currOptions4.Add(option6);
		List<TestMenu.Option> list5 = new List<TestMenu.Option>();
		list5.Add(new TestMenu.Option
		{
			label = "X start offset",
			valueStringFunc = (TestMenu.Option opt) => this._cameraMoveStart.x.ToString("F1"),
			smoothLeftRightAction = delegate(float dir, TestMenu.Option opt)
			{
				this.<AddTrailerOptions>g__MoveCameraSetup|22_34(dir * Vector3.right);
			}
		});
		list5.Add(new TestMenu.Option
		{
			label = "Y start offset",
			valueStringFunc = (TestMenu.Option opt) => this._cameraMoveStart.y.ToString("F1"),
			smoothLeftRightAction = delegate(float dir, TestMenu.Option opt)
			{
				this.<AddTrailerOptions>g__MoveCameraSetup|22_34(dir * Vector3.up);
			}
		});
		list5.Add(new TestMenu.Option
		{
			label = "Z start offset",
			valueStringFunc = (TestMenu.Option opt) => this._cameraMoveStart.y.ToString("F1"),
			smoothLeftRightAction = delegate(float dir, TestMenu.Option opt)
			{
				this.<AddTrailerOptions>g__MoveCameraSetup|22_34(dir * Vector3.forward);
			}
		});
		list5.Add(new TestMenu.Option
		{
			label = "X track dist",
			valueStringFunc = (TestMenu.Option opt) => this._cameraTrackVec.x.ToString("F1"),
			smoothLeftRightAction = delegate(float dir, TestMenu.Option opt)
			{
				this._cameraTrackVec.x = this._cameraTrackVec.x + 3f * this.cameraMoveSpeed * dir * Time.unscaledDeltaTime;
			}
		});
		list5.Add(new TestMenu.Option
		{
			label = "Y track dist",
			valueStringFunc = (TestMenu.Option opt) => this._cameraTrackVec.y.ToString("F1"),
			smoothLeftRightAction = delegate(float dir, TestMenu.Option opt)
			{
				this._cameraTrackVec.y = this._cameraTrackVec.y + 3f * this.cameraMoveSpeed * dir * Time.unscaledDeltaTime;
			}
		});
		list5.Add(new TestMenu.Option
		{
			label = "Z track dist",
			valueStringFunc = (TestMenu.Option opt) => this._cameraTrackVec.z.ToString("F1"),
			smoothLeftRightAction = delegate(float dir, TestMenu.Option opt)
			{
				this._cameraTrackVec.z = this._cameraTrackVec.z + 3f * this.cameraMoveSpeed * dir * Time.unscaledDeltaTime;
			}
		});
		List<TestMenu.Option> list6 = list5;
		TestMenu.Option option7 = new TestMenu.Option();
		option7.label = "Reset";
		option7.valueStringFunc = (TestMenu.Option opt) => ">>>";
		option7.action = delegate(TestMenu.Option opt)
		{
			GameCamera.instance.setManually = false;
			GameCamera.instance.Refresh(true);
			this.ResetCameraMoveStartPoint();
			GameCamera.instance.setManually = true;
			this._cameraTrackVec = Vector3.zero;
		};
		list6.Add(option7);
		list5.Add(new TestMenu.Option
		{
			label = "Time scalar",
			valueStringFunc = (TestMenu.Option opt) => this._cameraMoveTimeScalar.ToString("F1") + "x",
			leftRightAction = delegate(int dir, TestMenu.Option opt)
			{
				this._cameraMoveTimeScalar = Mathf.Clamp(this._cameraMoveTimeScalar + (float)dir * 0.1f, 0.1f, 2f);
			}
		});
		list5.Add(new TestMenu.Option
		{
			label = "Speed scalar",
			valueStringFunc = (TestMenu.Option opt) => this._cameraMoveSpeedScalar.ToString(),
			leftRightAction = delegate(int dir, TestMenu.Option opt)
			{
				this._cameraMoveSpeedScalar = Mathf.Clamp(this._cameraMoveSpeedScalar + (float)dir * 0.1f, 0.1f, 5f);
			}
		});
		list5.Add(new TestMenu.Option
		{
			label = "Timelapse",
			valueStringFunc = delegate(TestMenu.Option opt)
			{
				if (!this._cameraMoveHasTimelapse)
				{
					return "OFF";
				}
				return "ON";
			},
			leftRightAction = delegate(int dir, TestMenu.Option opt)
			{
				this._cameraMoveHasTimelapse = !this._cameraMoveHasTimelapse;
			}
		});
		list5.Add(new TestMenu.Option
		{
			label = "Timelapse speed",
			valueStringFunc = (TestMenu.Option opt) => this._cameraMoveTimelapseSpeed.ToString("F1"),
			leftRightAction = delegate(int dir, TestMenu.Option opt)
			{
				this._cameraMoveTimelapseSpeed = Mathf.Clamp(this._cameraMoveTimelapseSpeed + (float)dir * 0.1f, 0.1f, 10f);
			}
		});
		list5.Add(new TestMenu.Option
		{
			label = "All levels high LOD",
			valueStringFunc = delegate(TestMenu.Option opt)
			{
				if (!this.allHighLOD)
				{
					return "OFF";
				}
				return "ON";
			},
			leftRightAction = delegate(int dir, TestMenu.Option opt)
			{
				this.allHighLOD = !this.allHighLOD;
			}
		});
		List<TestMenu.Option> list7 = list5;
		TestMenu.Option option8 = new TestMenu.Option();
		option8.label = "GO";
		option8.valueStringFunc = (TestMenu.Option opt) => ">>>";
		option8.action = delegate(TestMenu.Option opt)
		{
			this.SetVisible(false);
			base.StartCoroutine(this.CameraTrackCoroutine());
		};
		list7.Add(option8);
		List<TestMenu.Option> currOptions5 = this.currOptions;
		TestMenu.Option option9 = new TestMenu.Option();
		option9.label = "Camera track";
		option9.valueStringFunc = (TestMenu.Option opt) => ">";
		option9.submenu = list5;
		option9.action = delegate(TestMenu.Option opt)
		{
			if (!this._hasCameraMoveStart)
			{
				this.ResetCameraMoveStartPoint();
			}
			GameCamera.instance.setManually = true;
			this.<AddTrailerOptions>g__MoveCameraSetup|22_34(Vector3.zero);
		};
		currOptions5.Add(option9);
		List<TestMenu.Option> currOptions6 = this.currOptions;
		TestMenu.Option option10 = new TestMenu.Option();
		option10.label = "Time";
		option10.valueStringFunc = delegate(TestMenu.Option opt)
		{
			float hourOfDay = GameClock.instance.hourOfDay;
			int num = Mathf.FloorToInt(hourOfDay);
			int num2 = Mathf.RoundToInt((hourOfDay - (float)num) * 60f);
			if (num2 == 60)
			{
				num++;
				num2 = 0;
			}
			return string.Format("{0}:{1}", num, num2.ToString("D2"));
		};
		option10.leftRightAction = delegate(int dir, TestMenu.Option opt)
		{
			float num3 = GameClock.instance.daysNorm * 96f;
			int num4 = ((dir > 0) ? Mathf.CeilToInt(num3) : Mathf.FloorToInt(num3));
			num4 += dir;
			GameClock.instance.daysNorm = (float)num4 / 96f;
		};
		currOptions6.Add(option10);
		this.currOptions.Add(new TestMenu.Option
		{
			label = "Weather override",
			valueStringFunc = (TestMenu.Option opt) => this._weatherOverrideTypes[this._selectedWeatherOverrideIdx].ToString() + " (press enter to apply)",
			leftRightAction = delegate(int dir, TestMenu.Option opt)
			{
				this._selectedWeatherOverrideIdx = (this._selectedWeatherOverrideIdx + dir + this._weatherOverrideTypes.Length) % this._weatherOverrideTypes.Length;
			},
			action = delegate(TestMenu.Option opt)
			{
				WeatherType weatherType = this._weatherOverrideTypes[this._selectedWeatherOverrideIdx];
				WeatherSystem.instance.BeginGlobalInkOverride(weatherType);
			}
		});
		this.currOptions.Add(new TestMenu.Option
		{
			label = "Music track",
			valueStringFunc = delegate(TestMenu.Option opt)
			{
				if (this._musicTrackIdx == -1)
				{
					return "DEFAULT GAME CHOICE";
				}
				return MonoSingleton<MusicLibrary>.instance.trackLibrary[this._musicTrackIdx].displayName;
			},
			leftRightAction = delegate(int dir, TestMenu.Option opt)
			{
				BeatTrack[] trackLibrary = MonoSingleton<MusicLibrary>.instance.trackLibrary;
				this._musicTrackIdx += dir;
				if (this._musicTrackIdx >= trackLibrary.Length)
				{
					this._musicTrackIdx = -1;
				}
				if (this._musicTrackIdx < -1)
				{
					this._musicTrackIdx = trackLibrary.Length - 1;
				}
				if (this._musicTrackIdx == -1)
				{
					DebugOptions.opts.dontChooseNewMusic = false;
					return;
				}
				DebugOptions.opts.dontChooseNewMusic = true;
				MonoSingleton<RunTrack>.instance.ChangeTrack(trackLibrary[this._musicTrackIdx], true, -1);
				MonoSingleton<RunTrack>.instance.musicStartBarIdx = 0;
			}
		});
		List<TestMenu.Option> currOptions7 = this.currOptions;
		TestMenu.Option option11 = new TestMenu.Option();
		option11.label = "Music start bar";
		option11.valueStringFunc = (TestMenu.Option opt) => MonoSingleton<RunTrack>.instance.musicStartBarIdx.ToString();
		option11.leftRightAction = delegate(int dir, TestMenu.Option opt)
		{
			MonoSingleton<RunTrack>.instance.musicStartBarIdx = Mathf.Clamp(MonoSingleton<RunTrack>.instance.musicStartBarIdx + dir, 0, MonoSingleton<RunTrack>.instance.track.bars.Length - 1);
		};
		currOptions7.Add(option11);
	}

	// Token: 0x0600080A RID: 2058 RVA: 0x000470E9 File Offset: 0x000452E9
	private void ResetCameraMoveStartPoint()
	{
		this._cameraMoveStart = GameCamera.instance.cameraProperties.targetPoint;
		this._hasCameraMoveStart = true;
	}

	// Token: 0x0600080B RID: 2059 RVA: 0x00047108 File Offset: 0x00045308
	private void AddTestAndPerfOptions()
	{
		this.AddToggleableGameObject(Runner.instance.gameObject);
		List<TestMenu.Option> currOptions = this.currOptions;
		TestMenu.Option option = new TestMenu.Option();
		option.label = "High quality water";
		option.valueStringFunc = delegate(TestMenu.Option opt)
		{
			if (!GameCamera.instance.highQualityWaterReflections)
			{
				return "OFF";
			}
			return "ON";
		};
		option.action = delegate(TestMenu.Option opt)
		{
			GameCamera.instance.highQualityWaterReflections = !GameCamera.instance.highQualityWaterReflections;
		};
		currOptions.Add(option);
		List<TestMenu.Option> currOptions2 = this.currOptions;
		TestMenu.Option option2 = new TestMenu.Option();
		option2.label = "Visualise texel density";
		option2.valueStringFunc = delegate(TestMenu.Option opt)
		{
			if (!DebugOptions.opts.visualiseTexelDensity)
			{
				return "OFF";
			}
			return "ON";
		};
		option2.action = delegate(TestMenu.Option opt)
		{
			DebugOptions.opts.visualiseTexelDensity = !DebugOptions.opts.visualiseTexelDensity;
		};
		currOptions2.Add(option2);
		List<LevelSection> list = new List<LevelSection>();
		List<Level> levels = WorldManager.instance.currentWorld.levels;
		for (int i = Level.currentIndex + 1; i < levels.Count; i++)
		{
			list.AddRange(levels[i].loadedLevelSections);
		}
		List<TestMenu.Option> currOptions3 = this.currOptions;
		TestMenu.Option option3 = new TestMenu.Option();
		option3.label = "Future Levels";
		option3.enabled = true;
		option3.context = list;
		option3.valueStringFunc = delegate(TestMenu.Option opt)
		{
			if (!opt.enabled)
			{
				return "OFF";
			}
			return "ON";
		};
		option3.action = delegate(TestMenu.Option opt)
		{
			opt.enabled = !opt.enabled;
			foreach (LevelSection levelSection in ((List<LevelSection>)opt.context))
			{
				levelSection.gameObject.SetActive(opt.enabled);
			}
		};
		currOptions3.Add(option3);
		List<TestMenu.Option> currOptions4 = this.currOptions;
		TestMenu.Option option4 = new TestMenu.Option();
		option4.label = "Free camera";
		option4.enabled = false;
		option4.valueStringFunc = delegate(TestMenu.Option opt)
		{
			if (GameCamera.instance.freeCameraState.active)
			{
				return "END";
			}
			return "START";
		};
		option4.action = delegate(TestMenu.Option opt)
		{
			GameCamera.FreeCameraState freeCameraState = GameCamera.instance.freeCameraState;
			freeCameraState.active = !freeCameraState.active;
			if (freeCameraState.active)
			{
				freeCameraState.restricted = false;
				freeCameraState.allowInput = true;
				GameInput.PushControlStack(this._freeCamControlObj);
				Runner.instance.playerControlDisabled |= PlayerControlDisableReason.FreeCamera;
				this.SetVisible(false);
				return;
			}
			Runner.instance.playerControlDisabled &= ~PlayerControlDisableReason.FreeCamera;
			GameInput.PopControlStack(this._freeCamControlObj, true);
		};
		currOptions4.Add(option4);
	}

	// Token: 0x0600080C RID: 2060 RVA: 0x000472FC File Offset: 0x000454FC
	private void AddGameSetups()
	{
		List<TestMenu.Option> list = new List<TestMenu.Option>();
		foreach (GameSetup gameSetup in this._allGameSetups)
		{
			if (!(gameSetup.world != WorldManager.instance.currentWorld))
			{
				List<TestMenu.Option> list2 = list;
				TestMenu.Option option = new TestMenu.Option();
				option.label = gameSetup.name;
				option.context = gameSetup;
				option.valueStringFunc = (TestMenu.Option opt) => "RUN";
				option.action = delegate(TestMenu.Option opt)
				{
					((GameSetup)opt.context).Run(null);
					this.SetVisible(false);
				};
				list2.Add(option);
			}
		}
		List<TestMenu.Option> currOptions = this.currOptions;
		TestMenu.Option option2 = new TestMenu.Option();
		option2.label = "Game Setups";
		option2.valueStringFunc = (TestMenu.Option opt) => ">";
		option2.submenu = list;
		currOptions.Add(option2);
	}

	// Token: 0x0600080D RID: 2061 RVA: 0x000473DC File Offset: 0x000455DC
	private void SetVisible(bool shouldBeVisible)
	{
		this.visible = shouldBeVisible;
		TestMenu.wantsGameplayPaused = shouldBeVisible;
		if (this.visible)
		{
			GameInput.PushControlStack(this);
		}
		else
		{
			GameInput.PopControlStack(this, true);
		}
		if (this.disablesPlayerMovementWhenVisible)
		{
			if (this.visible)
			{
				Runner.instance.playerControlDisabled |= PlayerControlDisableReason.TestMenu;
			}
			else
			{
				Runner.instance.playerControlDisabled &= ~PlayerControlDisableReason.TestMenu;
			}
		}
		if (!this.visible)
		{
			while (this._optionMenusStack.Count > 1)
			{
				this._optionMenusStack.Pop();
			}
		}
		if (!this.visible)
		{
			GameCamera.instance.setManually = false;
		}
	}

	// Token: 0x0600080E RID: 2062 RVA: 0x00047484 File Offset: 0x00045684
	private void Update()
	{
		if (!MonoSingleton<BuildSetupManager>.instance.setup.hasTestMenu)
		{
			return;
		}
		if (!this.visible && GameInput.debugToggleTestMenu)
		{
			GameInput.ClearInputState();
			this.SetVisible(true);
		}
		if (this.visible && (GameInput.debugToggleTestMenu || (this._optionMenusStack.Count == 1 && GameInput.Back(this))))
		{
			GameInput.ClearInputState();
			this.SetVisible(false);
		}
		else if (this._optionMenusStack.Count > 1 && GameInput.Back(this))
		{
			GameInput.ClearInputState();
			this._optionMenusStack.Pop();
		}
		else if (GameCamera.instance.freeCameraState.active && !PhotoMode.visible && !this.visible && GameInput.selectMenuItem)
		{
			this.SetVisible(false);
			Runner.instance.playerControlDisabled &= ~PlayerControlDisableReason.FreeCamera;
			GameInput.PopControlStack(this._freeCamControlObj, true);
			GameCamera.instance.freeCameraState.active = false;
			base.StartCoroutine(Game.instance.TeleportPlayerTo3DCR(GameCamera.instance.cameraProperties.targetPoint, "Free cam teleport", 0, false, null, false, false, null, null));
		}
		if (this.visible)
		{
			if (GameInput.selectDown)
			{
				this.idx = (this.idx + 1) % this.currOptions.Count;
			}
			if (GameInput.selectUp)
			{
				this.idx = (this.idx - 1 + this.currOptions.Count) % this.currOptions.Count;
			}
			if (this.currOptions[this.idx].smoothLeftRightAction != null)
			{
				float moveLeftRight = GameInput.moveLeftRight;
				if (moveLeftRight != 0f)
				{
					this.currOptions[this.idx].smoothLeftRightAction(moveLeftRight, this.currOptions[this.idx]);
				}
			}
			else if (this.currOptions[this.idx].leftRightAction != null)
			{
				if (GameInput.selectLeft)
				{
					this.currOptions[this.idx].leftRightAction(-1, this.currOptions[this.idx]);
				}
				if (GameInput.selectRight)
				{
					this.currOptions[this.idx].leftRightAction(1, this.currOptions[this.idx]);
				}
			}
			if (GameInput.selectMenuItem)
			{
				MonoSingleton<GameInput>.instance.mapping.selectMenuItem.ClearInputState();
				TestMenu.Option option = this.currOptions[this.idx];
				if (option.submenu != null)
				{
					this._optionMenusStack.Push(new TestMenu.MenuState
					{
						options = option.submenu,
						idx = 0
					});
				}
				if (option.action != null)
				{
					option.action(option);
					return;
				}
				if (option.leftRightAction != null)
				{
					option.leftRightAction(1, option);
				}
			}
		}
	}

	// Token: 0x0600080F RID: 2063 RVA: 0x00047762 File Offset: 0x00045962
	private IEnumerator TrailerTimelapseCoroutine()
	{
		float t = 0f;
		GameClock clock = GameClock.instance;
		float startDaysNorm = (float)clock.dayIdx + (float)this._timelapseStartHour / 24f;
		float endDaysNorm = (float)clock.dayIdx + (float)this._timelapseEndHour / 24f;
		bool passesDay = false;
		if (endDaysNorm < startDaysNorm)
		{
			endDaysNorm += 1f;
			passesDay = true;
		}
		while (t < this._timelapseDuration)
		{
			t += Time.unscaledDeltaTime;
			float num = Mathf.Clamp01(t / this._timelapseDuration);
			clock.daysNorm = Mathf.Lerp(startDaysNorm, endDaysNorm, num);
			yield return null;
		}
		if (passesDay)
		{
			clock.dayIdx--;
		}
		yield break;
	}

	// Token: 0x06000810 RID: 2064 RVA: 0x00047771 File Offset: 0x00045971
	private IEnumerator CameraSwoopCoroutine()
	{
		this.cameraSwoopOrMoveActive = true;
		GameInput.PushControlStack(this);
		Game.instance.SetTimeScalar(Game.TimeScalar.TestMenuSequence, this._cameraMoveTimeScalar);
		if (this.allHighLOD)
		{
			Level.SetAllHighLOD();
		}
		GameCamera.IntroCameraState introCam = GameCamera.instance.introCameraState;
		introCam.BeginTrailerSwoop(this._cameraMoveSpeedScalar);
		GameClock clock = GameClock.instance;
		float originalDaysNorm = clock.daysNorm;
		while (introCam.active)
		{
			if (this._cameraMoveHasTimelapse)
			{
				clock.daysNorm += 0.1f * this._cameraMoveTimelapseSpeed * Time.deltaTime;
			}
			if (GameInput.Back(this))
			{
				introCam.Reset();
				break;
			}
			yield return null;
		}
		if (this._cameraMoveHasTimelapse)
		{
			clock.daysNorm = originalDaysNorm;
		}
		Game.instance.RemoveTimeScalar(Game.TimeScalar.TestMenuSequence);
		GameInput.PopControlStack(this, true);
		int num = Level.DepthToIndex((float)Runner.instance.physicalDepthLayerIdx);
		Level.SetupLODs(WorldManager.instance.currentWorld, num);
		this.cameraSwoopOrMoveActive = false;
		yield break;
	}

	// Token: 0x06000811 RID: 2065 RVA: 0x00047780 File Offset: 0x00045980
	private IEnumerator CameraTrackCoroutine()
	{
		this.cameraSwoopOrMoveActive = true;
		GameCamera.instance.setManually = true;
		GameInput.PushControlStack(this);
		Game.instance.SetTimeScalar(Game.TimeScalar.TestMenuSequence, this._cameraMoveTimeScalar);
		if (this.allHighLOD)
		{
			Level.SetAllHighLOD();
		}
		GameClock clock = GameClock.instance;
		float originalDaysNorm = clock.daysNorm;
		float trackDist = 0f;
		while (trackDist < this._cameraTrackVec.magnitude)
		{
			trackDist += 20f * this._cameraMoveSpeedScalar * Time.deltaTime;
			HighlandCameraProperties cameraProperties = GameCamera.instance.cameraProperties;
			cameraProperties.targetPoint = this._cameraMoveStart + trackDist * this._cameraTrackVec.normalized;
			GameCamera.instance.cameraProperties = cameraProperties;
			if (this._cameraMoveHasTimelapse)
			{
				clock.daysNorm += 0.1f * this._cameraMoveTimelapseSpeed * Time.deltaTime;
			}
			if (GameInput.Back(this))
			{
				break;
			}
			yield return null;
		}
		if (this._cameraMoveHasTimelapse)
		{
			clock.daysNorm = originalDaysNorm;
		}
		Game.instance.RemoveTimeScalar(Game.TimeScalar.TestMenuSequence);
		GameInput.PopControlStack(this, true);
		int num = Level.DepthToIndex((float)Runner.instance.physicalDepthLayerIdx);
		Level.SetupLODs(WorldManager.instance.currentWorld, num);
		GameCamera.instance.setManually = false;
		this.cameraSwoopOrMoveActive = false;
		yield break;
	}

	// Token: 0x06000812 RID: 2066 RVA: 0x00047790 File Offset: 0x00045990
	private void OnGUI()
	{
		if (!this.visible)
		{
			return;
		}
		float num = 200f;
		GUI.color = Color.black.WithAlpha(0.5f);
		GUI.DrawTexture(new Rect(0f, num, 400f, 25f * (float)this.currOptions.Count), Texture2D.whiteTexture);
		GUI.color = Color.white;
		for (int i = 0; i < this.currOptions.Count; i++)
		{
			GUI.color = ((this.idx == i) ? Color.white : Color.white.WithAlpha(0.5f));
			Rect rect = new Rect(20f, num, 200f, 25f);
			GUI.Label(rect, this.currOptions[i].label);
			rect.x += 200f;
			GUI.Label(rect, this.currOptions[i].valueStringFunc(this.currOptions[i]));
			num += 25f;
		}
	}

	// Token: 0x17000211 RID: 529
	// (get) Token: 0x06000813 RID: 2067 RVA: 0x000478A7 File Offset: 0x00045AA7
	private List<TestMenu.Option> currOptions
	{
		get
		{
			return this._optionMenusStack.Peek().options;
		}
	}

	// Token: 0x17000212 RID: 530
	// (get) Token: 0x06000814 RID: 2068 RVA: 0x000478B9 File Offset: 0x00045AB9
	// (set) Token: 0x06000815 RID: 2069 RVA: 0x000478CB File Offset: 0x00045ACB
	private int idx
	{
		get
		{
			return this._optionMenusStack.Peek().idx;
		}
		set
		{
			this._optionMenusStack.Peek().idx = value;
		}
	}

	// Token: 0x06000832 RID: 2098 RVA: 0x00047C38 File Offset: 0x00045E38
	[CompilerGenerated]
	private void <AddTrailerOptions>g__MoveCameraSetup|22_34(Vector3 dir)
	{
		GameCamera.instance.setManually = true;
		this._cameraMoveStart += this.cameraMoveSpeed * dir * Time.unscaledDeltaTime;
		HighlandCameraProperties cameraProperties = GameCamera.instance.cameraProperties;
		cameraProperties.targetPoint = this._cameraMoveStart;
		GameCamera.instance.cameraProperties = cameraProperties;
	}

	// Token: 0x040009ED RID: 2541
	public bool visible;

	// Token: 0x040009EE RID: 2542
	public bool disablesPlayerMovementWhenVisible;

	// Token: 0x040009EF RID: 2543
	public bool forceSwitchController;

	// Token: 0x040009F0 RID: 2544
	public float cameraMoveSpeed = 30f;

	// Token: 0x040009F1 RID: 2545
	public static bool wantsGameplayPaused;

	// Token: 0x040009F2 RID: 2546
	private bool _hudEnabled = true;

	// Token: 0x040009F3 RID: 2547
	private bool _allUIEnabled = true;

	// Token: 0x040009F4 RID: 2548
	private Camera _gameUICamera;

	// Token: 0x040009F5 RID: 2549
	public bool cameraSwoopOrMoveActive;

	// Token: 0x040009F6 RID: 2550
	public bool allHighLOD;

	// Token: 0x040009F7 RID: 2551
	private bool _cheapCloudShader;

	// Token: 0x040009F8 RID: 2552
	private int _gameSpeedPercent = 100;

	// Token: 0x040009F9 RID: 2553
	private Stack<TestMenu.MenuState> _optionMenusStack = new Stack<TestMenu.MenuState>();

	// Token: 0x040009FA RID: 2554
	private GameSetup[] _allGameSetups;

	// Token: 0x040009FB RID: 2555
	private int _timelapseStartHour = 10;

	// Token: 0x040009FC RID: 2556
	private Vector3 _cameraMoveStart;

	// Token: 0x040009FD RID: 2557
	private Vector3 _cameraTrackVec;

	// Token: 0x040009FE RID: 2558
	private bool _hasCameraMoveStart;

	// Token: 0x040009FF RID: 2559
	private float _cameraMoveTimeScalar = 1f;

	// Token: 0x04000A00 RID: 2560
	private float _cameraMoveSpeedScalar = 1f;

	// Token: 0x04000A01 RID: 2561
	private bool _cameraMoveHasTimelapse;

	// Token: 0x04000A02 RID: 2562
	private float _cameraMoveTimelapseSpeed = 1f;

	// Token: 0x04000A03 RID: 2563
	private int _timelapseEndHour = 19;

	// Token: 0x04000A04 RID: 2564
	private float _timelapseDuration = 10f;

	// Token: 0x04000A05 RID: 2565
	private int _musicTrackIdx = -1;

	// Token: 0x04000A06 RID: 2566
	private string _freeCamControlObj = "Free cam";

	// Token: 0x04000A07 RID: 2567
	private WeatherType[] _weatherOverrideTypes = new WeatherType[]
	{
		WeatherType.Clear,
		WeatherType.Cloudy,
		WeatherType.VeryCloudy,
		WeatherType.Raining,
		WeatherType.Storm
	};

	// Token: 0x04000A08 RID: 2568
	private int _selectedWeatherOverrideIdx;

	// Token: 0x04000A09 RID: 2569
	[SerializeField]
	private GameObject[] _hudItems;

	// Token: 0x0200031A RID: 794
	private class Option
	{
		// Token: 0x040017C3 RID: 6083
		public object context;

		// Token: 0x040017C4 RID: 6084
		public bool enabled;

		// Token: 0x040017C5 RID: 6085
		public string label;

		// Token: 0x040017C6 RID: 6086
		public Func<TestMenu.Option, string> valueStringFunc;

		// Token: 0x040017C7 RID: 6087
		public Action<TestMenu.Option> action;

		// Token: 0x040017C8 RID: 6088
		public Action<int, TestMenu.Option> leftRightAction;

		// Token: 0x040017C9 RID: 6089
		public Action<float, TestMenu.Option> smoothLeftRightAction;

		// Token: 0x040017CA RID: 6090
		public List<TestMenu.Option> submenu;
	}

	// Token: 0x0200031B RID: 795
	private class MenuState
	{
		// Token: 0x040017CB RID: 6091
		public List<TestMenu.Option> options;

		// Token: 0x040017CC RID: 6092
		public int idx;
	}
}
