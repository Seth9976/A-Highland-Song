using System;
using UnityEngine;

// Token: 0x0200011F RID: 287
public class LookFurtherPrompt : PromptWithVisibility
{
	// Token: 0x060009A5 RID: 2469 RVA: 0x0005146F File Offset: 0x0004F66F
	protected override bool ShouldShow()
	{
		return this._wantsShowTimer > this.delay;
	}

	// Token: 0x17000263 RID: 611
	// (get) Token: 0x060009A6 RID: 2470 RVA: 0x0005147F File Offset: 0x0004F67F
	private bool wantsShow
	{
		get
		{
			return Game.instance.looking && !Game.instance.lookingFurther && Game.instance.canLookFurther && GameCamera.instance.playerZoomState.activeZoomDir == -1;
		}
	}

	// Token: 0x060009A7 RID: 2471 RVA: 0x000514B9 File Offset: 0x0004F6B9
	protected override void Update()
	{
		base.Update();
		if (this.wantsShow)
		{
			this._wantsShowTimer += Time.unscaledDeltaTime;
			return;
		}
		this._wantsShowTimer = 0f;
	}

	// Token: 0x04000BB6 RID: 2998
	public float delay = 2f;

	// Token: 0x04000BB7 RID: 2999
	private float _wantsShowTimer;
}
