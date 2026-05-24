using System;
using UnityEngine;

// Token: 0x0200012F RID: 303
public class PanArrowsUI : PromptWithVisibility
{
	// Token: 0x06000A41 RID: 2625 RVA: 0x00055BA4 File Offset: 0x00053DA4
	protected override bool ShouldShow()
	{
		return Game.loaded && (MonoSingleton<PeakStateController>.instance.wantsDirectionalArrows || GameCamera.instance.playerZoomState.lookFurther);
	}

	// Token: 0x06000A42 RID: 2626 RVA: 0x00055BCC File Offset: 0x00053DCC
	protected override void Update()
	{
		base.Update();
		float num = 0.5f * (Mathf.Sin(this.pulseSpeed * Time.unscaledTime) + 1f);
		this._innerGroup.groupAlpha = Mathf.Lerp(0.1f, 0.6f, num);
		base.layout.width = Mathf.Lerp(base.layout.parentRect.width, this.scaleWithMap * base.layout.parentRect.width, MonoSingleton<MapsViewController>.instance.maximisedNorm);
		base.layout.scale = Mathf.Lerp(0.98f, 1f, num);
	}

	// Token: 0x04000C6E RID: 3182
	public float pulseSpeed = 1f;

	// Token: 0x04000C6F RID: 3183
	public Range pulseSizeRange = new Range(0.9f, 1.2f);

	// Token: 0x04000C70 RID: 3184
	public float scaleWithMap = 0.6f;

	// Token: 0x04000C71 RID: 3185
	[SerializeField]
	private SLayout _innerGroup;
}
