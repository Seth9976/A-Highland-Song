using System;
using UnityEngine;

// Token: 0x02000121 RID: 289
public class LookPromptUI : MonoBehaviour
{
	// Token: 0x17000265 RID: 613
	// (get) Token: 0x060009AC RID: 2476 RVA: 0x000515CE File Offset: 0x0004F7CE
	// (set) Token: 0x060009AD RID: 2477 RVA: 0x000515D6 File Offset: 0x0004F7D6
	public LookPromptUI.State state
	{
		get
		{
			return this._state;
		}
		set
		{
			if (this._state == value)
			{
				return;
			}
			this._state = value;
			this.layout.Animate(0.4f, new Action(this.RefreshAlpha));
		}
	}

	// Token: 0x17000266 RID: 614
	// (get) Token: 0x060009AE RID: 2478 RVA: 0x00051606 File Offset: 0x0004F806
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

	// Token: 0x060009AF RID: 2479 RVA: 0x00051628 File Offset: 0x0004F828
	private void OnEnable()
	{
		this._state = LookPromptUI.State.Hidden;
		this.RefreshAlpha();
	}

	// Token: 0x060009B0 RID: 2480 RVA: 0x00051638 File Offset: 0x0004F838
	private void Update()
	{
		if (Game.instance.inPeakState || GameCamera.instance.playerZoomState.zooming || MonoSingleton<MapsViewController>.instance.isBusy || Game.gameplayPaused || MonoSingleton<BlackBars>.instance.visible || Runner.instance.inFinalJumpAndLeftLand || PhotoMode.visible)
		{
			this.state = LookPromptUI.State.Hidden;
			return;
		}
		if (!Game.instance.inActiveGameplay)
		{
			this.state = LookPromptUI.State.Hidden;
			return;
		}
		if (Game.instance.maxPlayerZoomOut == MaxZoom.None)
		{
			this.state = LookPromptUI.State.Hidden;
			return;
		}
		if (Game.instance.maxPlayerZoomOut == MaxZoom.Limited)
		{
			this.state = LookPromptUI.State.GameLimitedZoom;
			return;
		}
		if (Game.instance.maxPlayerZoomOut == MaxZoom.OnRidge)
		{
			this.state = LookPromptUI.State.GameRidgeZoom;
			return;
		}
		this.state = LookPromptUI.State.Hidden;
	}

	// Token: 0x060009B1 RID: 2481 RVA: 0x000516F5 File Offset: 0x0004F8F5
	private void RefreshAlpha()
	{
		this.layout.groupAlpha = (float)((this.state == LookPromptUI.State.Hidden) ? 0 : 1);
	}

	// Token: 0x04000BBC RID: 3004
	[SerializeField]
	[Disable]
	private LookPromptUI.State _state;

	// Token: 0x04000BBD RID: 3005
	private SLayout _layout;

	// Token: 0x0200035A RID: 858
	public enum State
	{
		// Token: 0x0400189A RID: 6298
		Hidden,
		// Token: 0x0400189B RID: 6299
		GameLimitedZoom,
		// Token: 0x0400189C RID: 6300
		GameRidgeZoom
	}
}
