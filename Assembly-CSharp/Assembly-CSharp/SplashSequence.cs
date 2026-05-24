using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200014D RID: 333
public class SplashSequence : MonoSingleton<SplashSequence>
{
	// Token: 0x170002AE RID: 686
	// (get) Token: 0x06000B47 RID: 2887 RVA: 0x0005B4D9 File Offset: 0x000596D9
	// (set) Token: 0x06000B48 RID: 2888 RVA: 0x0005B4E1 File Offset: 0x000596E1
	public bool visible { get; private set; }

	// Token: 0x06000B49 RID: 2889 RVA: 0x0005B4EA File Offset: 0x000596EA
	public void Begin()
	{
		base.StartCoroutine(this.SplashSequence_Coroutine());
	}

	// Token: 0x06000B4A RID: 2890 RVA: 0x0005B4F9 File Offset: 0x000596F9
	public void BeginLoadingAnimOnly()
	{
		this._animOnlyVisible = true;
		base.StartCoroutine(this.LoadingAnimOnly_Coroutine());
	}

	// Token: 0x06000B4B RID: 2891 RVA: 0x0005B50F File Offset: 0x0005970F
	public void CompleteLoadingAnimOnly()
	{
		this._animOnlyVisible = false;
	}

	// Token: 0x06000B4C RID: 2892 RVA: 0x0005B518 File Offset: 0x00059718
	private void Awake()
	{
		this._inkleLogo.alpha = 0f;
		this._calibrationReminder.groupAlpha = 0f;
		this._continuePrompt.groupAlpha = 0f;
		SplashSequence.preventTitleMusic = false;
		this._loadingImage.SetActive(false);
	}

	// Token: 0x06000B4D RID: 2893 RVA: 0x0005B567 File Offset: 0x00059767
	private IEnumerator SplashSequence_Coroutine()
	{
		this.visible = true;
		SplashSequence.wantsGameplayPaused = true;
		SplashSequence.preventTitleMusic = true;
		WorldManager.allowSceneActivations = false;
		this._loadingImage.SetActive(true);
		while (Launcher.launching)
		{
			yield return null;
		}
		base.StartCoroutine(this.FadeOutLoadingImage(3f));
		if (LatencyCalibrator.shouldPerformLaunchCalibration)
		{
			while (MonoSingleton<LatencyCalibrator>.instance == null || !MonoSingleton<LatencyCalibrator>.instance.readyToShow)
			{
				yield return null;
			}
			MonoSingleton<LatencyCalibrator>.instance.Show(LatencyCalibrator.Context.SplashScreenAuto);
			while (MonoSingleton<LatencyCalibrator>.instance.visibleAtAll || GameInput.HasControl(MonoSingleton<LatencyCalibrator>.instance))
			{
				yield return null;
			}
		}
		if (WorldManager.instance != null)
		{
			yield return WorldManager.instance.ActivateLoadedScenes();
		}
		SplashSequence.preventTitleMusic = false;
		this._calibrationReminder.Animate(this.calibrationReminderFadeTime, delegate
		{
			this._calibrationReminder.groupAlpha = 1f;
		});
		yield return this.WaitAndAllowSkip(this.calibrationReminderFadeTime + this.calibrationReminderReadTime, true, 1f, 1f);
		this._calibrationReminder.CancelAnimations();
		this._calibrationReminder.Animate(this.calibrationReminderFadeTime, delegate
		{
			this._calibrationReminder.groupAlpha = 0f;
		});
		yield return this.ActivateScenesOrPause(this.calibrateToInkleLogoTime);
		this._inkleLogo.Animate(this.logoFadeInTime, delegate
		{
			this._inkleLogo.alpha = 1f;
		});
		yield return this.WaitAndAllowSkip(this.logoFadeInTime + this.logoPauseTime, false, 0f, 1f);
		this._inkleLogo.CancelAnimations();
		this._inkleLogo.Animate(this.logoFadeOutTime, delegate
		{
			this._inkleLogo.alpha = 0f;
		});
		yield return this.ActivateScenesOrPause(this.logoToQuotePause);
		MonoSingleton<OpeningQuote>.instance.Show();
		yield return new WaitForSeconds(this.openingQuoteMinimumWaitTime);
		yield return this.WaitAndAllowSkip(this.openingQuoteMaximumWaitTime - this.openingQuoteMinimumWaitTime, true, 2f, 6f);
		MonoSingleton<OpeningQuote>.instance.Hide();
		WorldManager.allowSceneActivations = true;
		yield return this.ActivateScenesOrPause(this.pauseBeforeBlackoutFade);
		if (WorldManager.instance.loading)
		{
			this._loadingImage.SetActive(true);
			while (WorldManager.instance.loading)
			{
				yield return null;
			}
			base.StartCoroutine(this.FadeOutLoadingImage(3f));
		}
		Blackout.FadeIn(0f, null);
		this.visible = false;
		SplashSequence.wantsGameplayPaused = false;
		yield break;
	}

	// Token: 0x06000B4E RID: 2894 RVA: 0x0005B576 File Offset: 0x00059776
	private IEnumerator LoadingAnimOnly_Coroutine()
	{
		this._loadingImage.SetActive(true);
		while (this._animOnlyVisible)
		{
			yield return null;
		}
		base.StartCoroutine(this.FadeOutLoadingImage(0f));
		yield break;
	}

	// Token: 0x06000B4F RID: 2895 RVA: 0x0005B585 File Offset: 0x00059785
	private IEnumerator ActivateScenesOrPause(float minPauseTime)
	{
		float activateTimeStart = Time.unscaledTime;
		if (WorldManager.instance != null)
		{
			yield return WorldManager.instance.ActivateLoadedScenes();
		}
		float num = Time.unscaledTime - activateTimeStart;
		if (num < minPauseTime)
		{
			yield return new WaitForSeconds(minPauseTime - num);
		}
		yield break;
	}

	// Token: 0x06000B50 RID: 2896 RVA: 0x0005B594 File Offset: 0x00059794
	private IEnumerator FadeOutLoadingImage(float delay = 0f)
	{
		Image img = this._loadingImage.GetComponentInChildren<Image>();
		for (float alphaAndTime = 1f + delay; alphaAndTime > 0f; alphaAndTime -= Time.unscaledDeltaTime)
		{
			img.color = Color.white.WithAlpha(Mathf.Clamp01(alphaAndTime));
			yield return null;
		}
		img.color = Color.white;
		this._loadingImage.SetActive(false);
		yield break;
	}

	// Token: 0x06000B51 RID: 2897 RVA: 0x0005B5AA File Offset: 0x000597AA
	private IEnumerator WaitAndAllowSkip(float duration, bool withContinuePrompt, float continuePromptFadeInDelay = 0f, float continuePromptFadeInTime = 1f)
	{
		float t = 0f;
		GameInput.PushControlStack(this);
		if (withContinuePrompt)
		{
			this._continuePrompt.Animate(continuePromptFadeInTime, continuePromptFadeInDelay, delegate
			{
				this._continuePrompt.groupAlpha = 1f;
			});
		}
		while (t < duration && !GameInput.Back(this) && !GameInput.selectMenuItem && !GameInput.jumped)
		{
			yield return null;
			t += Time.unscaledDeltaTime;
		}
		if (withContinuePrompt)
		{
			this._continuePrompt.Animate(0.2f, delegate
			{
				this._continuePrompt.groupAlpha = 0f;
			});
		}
		GameInput.PopControlStack(this, true);
		yield break;
	}

	// Token: 0x04000D65 RID: 3429
	public static bool preventTitleMusic;

	// Token: 0x04000D66 RID: 3430
	public static bool wantsGameplayPaused;

	// Token: 0x04000D67 RID: 3431
	public float initialGraceTime = 2f;

	// Token: 0x04000D68 RID: 3432
	public float logoFadeInTime = 2f;

	// Token: 0x04000D69 RID: 3433
	public float logoPauseTime = 4f;

	// Token: 0x04000D6A RID: 3434
	public float logoFadeOutTime = 4f;

	// Token: 0x04000D6B RID: 3435
	public float logoToQuotePause = 1f;

	// Token: 0x04000D6C RID: 3436
	public float pauseBeforeBlackoutFade = 2f;

	// Token: 0x04000D6D RID: 3437
	public float calibrationReminderFadeTime = 0.5f;

	// Token: 0x04000D6E RID: 3438
	public float calibrationReminderReadTime = 4f;

	// Token: 0x04000D6F RID: 3439
	public float calibrateToInkleLogoTime = 1f;

	// Token: 0x04000D70 RID: 3440
	public float openingQuoteMinimumWaitTime = 2f;

	// Token: 0x04000D71 RID: 3441
	public float openingQuoteMaximumWaitTime = 12f;

	// Token: 0x04000D73 RID: 3443
	[SerializeField]
	private SLayout _inkleLogo;

	// Token: 0x04000D74 RID: 3444
	[SerializeField]
	private SLayout _calibrationReminder;

	// Token: 0x04000D75 RID: 3445
	[SerializeField]
	private SLayout _continuePrompt;

	// Token: 0x04000D76 RID: 3446
	[SerializeField]
	private GameObject _loadingImage;

	// Token: 0x04000D77 RID: 3447
	private bool _needsSetup;

	// Token: 0x04000D78 RID: 3448
	private bool _animOnlyVisible;
}
