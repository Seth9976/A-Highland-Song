using System;
using System.Collections.Generic;
using EasyButtons;
using UnityEngine;

// Token: 0x02000154 RID: 340
public class TitleScreen : MonoSingleton<TitleScreen>
{
	// Token: 0x170002B4 RID: 692
	// (get) Token: 0x06000B84 RID: 2948 RVA: 0x0005CA51 File Offset: 0x0005AC51
	public bool allowInput
	{
		get
		{
			return this.visible && !this._appearingOrHiding && GameInput.HasControl(this) && this._options.Count > 0;
		}
	}

	// Token: 0x06000B85 RID: 2949 RVA: 0x0005CA7B File Offset: 0x0005AC7B
	private void Start()
	{
		this._canvas.gameObject.SetActive(false);
		this.visible = false;
		TitleScreen.wantsGameplayPaused = false;
	}

	// Token: 0x06000B86 RID: 2950 RVA: 0x0005CA9C File Offset: 0x0005AC9C
	public void Show(bool forNewGamePlus = false)
	{
		if (this.visible)
		{
			return;
		}
		this._canvas.gameObject.SetActive(true);
		this._layout.groupAlpha = 0f;
		this._appearingOrHiding = true;
		MonoSingleton<BlackBars>.instance.SetVisible(true, BlackBarsReason.TitleScreen);
		this.CreateOptions(forNewGamePlus);
		if (this._logoDefaultX == 0f)
		{
			this._logoDefaultX = this._logo.x;
		}
		this._logo.x = this._logoDefaultX - 150f;
		this._layout.Animate(1f, delegate
		{
			this._logo.x = this._logoDefaultX;
			this._layout.groupAlpha = 1f;
		}).Then(delegate
		{
			if (this.visible)
			{
				this._appearingOrHiding = false;
			}
		});
		string text = CurrentVersionSO.Instance.version.ToBasicWithSHAString();
		string visibleNameOnTitleScreen = MonoSingleton<BuildSetupManager>.instance.setup.visibleNameOnTitleScreen;
		if (visibleNameOnTitleScreen != null && visibleNameOnTitleScreen.Length > 0)
		{
			text = text + " - " + visibleNameOnTitleScreen;
		}
		this._versionText.textMeshPro.text = text;
		this._optionsGroupLayout.groupAlpha = 1f;
		GameInput.PushControlStack(this);
		this.visible = true;
		TitleScreen.wantsGameplayPaused = true;
	}

	// Token: 0x06000B87 RID: 2951 RVA: 0x0005CBC0 File Offset: 0x0005ADC0
	public void Hide()
	{
		if (!this.visible)
		{
			return;
		}
		this._appearingOrHiding = true;
		this._layout.Animate(0.5f, delegate
		{
			foreach (SettingButton settingButton in this._options)
			{
				settingButton.layout.groupAlpha = 0f;
				this._layout.AddDelay(0.3f);
			}
			this._logo.x = this._logoDefaultX - 150f;
			this._logo.alpha = 0f;
		});
		this._layout.Animate(1f, 0.25f, delegate
		{
			this._layout.groupAlpha = 0f;
		}).Then(delegate
		{
			if (!this.visible)
			{
				this._canvas.gameObject.SetActive(false);
				this._logo.alpha = 1f;
				this.RemoveAllOptions();
				MonoSingleton<BlackBars>.instance.SetVisible(false, BlackBarsReason.TitleScreen);
				this._appearingOrHiding = false;
			}
		});
		GameInput.PopControlStack(this, true);
		this.visible = false;
		TitleScreen.wantsGameplayPaused = false;
	}

	// Token: 0x06000B88 RID: 2952 RVA: 0x0005CC41 File Offset: 0x0005AE41
	public void RefreshOptions(bool forNewGamePlus)
	{
		this.RemoveAllOptions();
		this.CreateOptions(forNewGamePlus);
	}

	// Token: 0x06000B89 RID: 2953 RVA: 0x0005CC50 File Offset: 0x0005AE50
	private void Update()
	{
		if (this.allowInput)
		{
			if (GameInput.selectDown)
			{
				GameInput.ClearInputState();
				this.SelectNextPrev(1);
			}
			if (GameInput.selectUp)
			{
				GameInput.ClearInputState();
				this.SelectNextPrev(-1);
			}
			if (GameInput.selectMenuItem)
			{
				GameInput.ClearInputState();
				this._currentOption.Trigger();
			}
		}
		if (this.visible)
		{
			int num = (MonoSingleton<SettingsScreen>.instance.visible ? 0 : 1);
			this._optionsGroupLayout.groupAlpha = Mathf.MoveTowards(this._optionsGroupLayout.groupAlpha, (float)num, 2f * Time.unscaledDeltaTime);
		}
	}

	// Token: 0x06000B8A RID: 2954 RVA: 0x0005CCE4 File Offset: 0x0005AEE4
	private void SelectNextPrev(int dir)
	{
		int num = this._options.IndexOf(this._currentOption);
		int num2 = Mathf.Clamp(num + dir, 0, this._options.Count - 1);
		if (num != num2)
		{
			this._currentOption.highlighted = false;
			this._currentOption = this._options[num2];
			this._currentOption.highlighted = true;
			this.RefreshLayout();
		}
	}

	// Token: 0x06000B8B RID: 2955 RVA: 0x0005CD4C File Offset: 0x0005AF4C
	public void CreateOptions(bool forNewGamePlus)
	{
		bool isAtStart = MonoSingleton<Main>.instance.isAtStart;
		string text = (isAtStart ? "Begin" : "Continue");
		if (forNewGamePlus)
		{
			text = "Begin again";
		}
		this.CreateOption(text, delegate
		{
			this.Hide();
			Game.instance.IntroSwoopAfterTitleScreen();
		});
		if (!isAtStart)
		{
			this.CreateOption("Restart Game", delegate
			{
				MonoSingleton<Dialogue>.instance.Show("Restart game from beginning?", "Are you sure you wish restart the game? Your progress through the current game will be lost.", "Restart", "Cancel", true, delegate(Dialogue.Result result)
				{
					if (result == Dialogue.Result.Primary)
					{
						MonoSingleton<BlackBars>.instance.SetVisible(true, BlackBarsReason.Intro);
						this.Hide();
						MonoSingleton<Launcher>.instance.RestartAndBegin();
					}
				});
			});
		}
		this.CreateOption("Settings", delegate
		{
			MonoSingleton<SettingsScreen>.instance.Show();
		});
		this.CreateOption("Credits", delegate
		{
			this.Hide();
			MonoSingleton<Credits>.instance.Show(CreditsContext.TitleScreen, delegate
			{
				MonoSingleton<TitleScreen>.instance.Show(false);
			});
		});
		if (SystemInfo.deviceType == DeviceType.Desktop && !MonoSingleton<BuildSetupManager>.instance.setup.preventQuit)
		{
			this.CreateOption("Quit", delegate
			{
				Application.Quit();
			});
		}
		this._currentOption = this._options[0];
		this._currentOption.highlighted = true;
		this.RefreshLayout();
		for (int i = this._options.Count - 1; i >= 0; i--)
		{
			this._options[i].layout.groupAlpha = 0f;
		}
		this._layout.Animate(0.8f, delegate
		{
			foreach (SettingButton settingButton in this._options)
			{
				settingButton.layout.groupAlpha = 1f;
				this._layout.AddDelay(0.3f);
			}
		}).Then(delegate
		{
			this._appearingOrHiding = false;
		});
	}

	// Token: 0x06000B8C RID: 2956 RVA: 0x0005CEB4 File Offset: 0x0005B0B4
	public void RemoveAllOptions()
	{
		foreach (SettingButton settingButton in this._options)
		{
			settingButton.prototype.ReturnToPool();
		}
		this._options.Clear();
		this._currentOption = null;
	}

	// Token: 0x06000B8D RID: 2957 RVA: 0x0005CF1C File Offset: 0x0005B11C
	[Button]
	private void RefreshLayout()
	{
		float num = 0.5f * ((float)this._options.Count * this._settings.heightPerOption);
		float num2 = this._settings.optionsBottomToMidOptionsY - num;
		for (int i = this._options.Count - 1; i >= 0; i--)
		{
			this._options[i].layout.bottomY = num2;
			num2 += this._settings.heightPerOption;
			this._options[i].layout.scale = ((this._options[i] == this._currentOption) ? this._settings.highlightedScale : this._settings.nonHighlightedScale);
		}
	}

	// Token: 0x06000B8E RID: 2958 RVA: 0x0005CFDC File Offset: 0x0005B1DC
	private void CreateOption(string label, Action onTrigger)
	{
		SettingButton settingButton = this._menuOptionPrototype.Instantiate<SettingButton>(null);
		settingButton.Setup(label, onTrigger);
		this._options.Add(settingButton);
	}

	// Token: 0x04000DB7 RID: 3511
	public static bool wantsGameplayPaused;

	// Token: 0x04000DB8 RID: 3512
	public bool visible;

	// Token: 0x04000DB9 RID: 3513
	private SettingButton _currentOption;

	// Token: 0x04000DBA RID: 3514
	private List<SettingButton> _options = new List<SettingButton>();

	// Token: 0x04000DBB RID: 3515
	private float _logoDefaultX;

	// Token: 0x04000DBC RID: 3516
	private bool _appearingOrHiding;

	// Token: 0x04000DBD RID: 3517
	[SerializeField]
	private Canvas _canvas;

	// Token: 0x04000DBE RID: 3518
	[SerializeField]
	private SLayout _layout;

	// Token: 0x04000DBF RID: 3519
	[SerializeField]
	private SLayout _logo;

	// Token: 0x04000DC0 RID: 3520
	[SerializeField]
	private SLayout _optionsGroupLayout;

	// Token: 0x04000DC1 RID: 3521
	[SerializeField]
	private SLayout _versionText;

	// Token: 0x04000DC2 RID: 3522
	[SerializeField]
	private Prototype _menuOptionPrototype;

	// Token: 0x04000DC3 RID: 3523
	[SerializeField]
	private TitleScreenSettings _settings;
}
