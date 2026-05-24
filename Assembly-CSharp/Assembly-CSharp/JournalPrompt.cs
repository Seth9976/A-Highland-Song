using System;
using UnityEngine;

// Token: 0x0200011B RID: 283
public class JournalPrompt : PromptWithVisibility
{
	// Token: 0x0600099F RID: 2463 RVA: 0x000511DF File Offset: 0x0004F3DF
	protected override bool ShouldShow()
	{
		return MonoSingleton<JournalController>.instance.canShowAndNotAlreadyVisible && !MonoSingleton<BlackBars>.instance.visible && !MonoSingleton<PeakStateController>.instance.active && !PhotoMode.visible;
	}

	// Token: 0x060009A0 RID: 2464 RVA: 0x00051210 File Offset: 0x0004F410
	protected override void Update()
	{
		base.Update();
		if (this._shownOrShowing)
		{
			if (MonoSingleton<MapsViewController>.instance.inVicinityPromptableMap != null && MonoSingleton<MapsViewController>.instance.PlayerIsInCorrectPrimaryLocationForMap(MonoSingleton<MapsViewController>.instance.inVicinityPromptableMap) && (!Narrative.instance.isBusy || Narrative.instance.canInterrupt))
			{
				float num = 0.5f * (Mathf.Sin(this.pulseSpeed * Time.time) + 1f);
				this._icon.scale = this.pulseSizeRange.Lerp(num);
				return;
			}
			this._icon.scale = 1f;
		}
	}

	// Token: 0x04000B89 RID: 2953
	public float pulseSpeed = 1f;

	// Token: 0x04000B8A RID: 2954
	public Range pulseSizeRange = new Range(0.9f, 1.2f);

	// Token: 0x04000B8B RID: 2955
	[SerializeField]
	private SLayout _icon;
}
