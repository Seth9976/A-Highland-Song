using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000134 RID: 308
public class PeakClimbedBanner : MonoSingleton<PeakClimbedBanner>
{
	// Token: 0x17000296 RID: 662
	// (get) Token: 0x06000A61 RID: 2657 RVA: 0x00056678 File Offset: 0x00054878
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

	// Token: 0x06000A62 RID: 2658 RVA: 0x0005669A File Offset: 0x0005489A
	private void Awake()
	{
		this.layout.groupAlpha = 0f;
		base.gameObject.SetActive(false);
	}

	// Token: 0x06000A63 RID: 2659 RVA: 0x000566B8 File Offset: 0x000548B8
	public void LoadAllIcons()
	{
		this._settings.peakIconDatabase.LoadAll();
	}

	// Token: 0x06000A64 RID: 2660 RVA: 0x000566CA File Offset: 0x000548CA
	public IEnumerator Play_Coroutine(string peakInkId)
	{
		base.gameObject.SetActive(true);
		this._peakBadgeImage.sprite = this._settings.peakIconDatabase.SpriteWithName(peakInkId);
		this.layout.Animate(this._settings.fadeInTime, delegate
		{
			this.layout.groupAlpha = 1f;
		});
		if (this.layout.groupAlpha == 0f)
		{
			this.layout.scale = this._settings.startScale;
		}
		this.layout.Animate(this._settings.totalTime, 0f, this._settings.scaleCurve, delegate
		{
			this.layout.scale = 1f;
		});
		this._glow.alpha = 0f;
		this._glow.scale = this._settings.glowStartScale;
		this.layout.Animate(this._settings.glowFadeInTime, this._settings.glowDelay, delegate
		{
			this._glow.alpha = 1f;
			this._glow.scale = this._settings.glowVisibleScale;
		}).ThenAnimate(this._settings.glowFadeOutTime, this._settings.glowVisibleTime, delegate
		{
			this._glow.alpha = 0f;
			this._glow.scale = this._settings.glowEndScale;
		});
		yield return new WaitForSeconds(this._settings.totalTime - this._settings.fadeOutTime);
		this.layout.Animate(this._settings.fadeOutTime, delegate
		{
			this.layout.groupAlpha = 0f;
		}).Then(delegate
		{
			base.gameObject.SetActive(false);
		});
		yield return new WaitForSeconds(0.6f * this._settings.fadeOutTime);
		yield break;
	}

	// Token: 0x04000C8E RID: 3214
	private SLayout _layout;

	// Token: 0x04000C8F RID: 3215
	[SerializeField]
	private SLayout _glow;

	// Token: 0x04000C90 RID: 3216
	[SerializeField]
	private Image _peakBadgeImage;

	// Token: 0x04000C91 RID: 3217
	[SerializeField]
	private PeakClimbedBannerSettings _settings;
}
