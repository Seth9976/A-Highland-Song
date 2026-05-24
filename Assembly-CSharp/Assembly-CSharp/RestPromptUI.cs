using System;
using UnityEngine;

// Token: 0x02000140 RID: 320
public class RestPromptUI : MonoSingleton<RestPromptUI>
{
	// Token: 0x170002A4 RID: 676
	// (get) Token: 0x06000AD8 RID: 2776 RVA: 0x000587FC File Offset: 0x000569FC
	// (set) Token: 0x06000AD9 RID: 2777 RVA: 0x00058804 File Offset: 0x00056A04
	public bool visibleAndEnabled { get; private set; }

	// Token: 0x170002A5 RID: 677
	// (get) Token: 0x06000ADA RID: 2778 RVA: 0x0005880D File Offset: 0x00056A0D
	public Vector2 leftMidEdgeInCanvasSpace
	{
		get
		{
			return this._layout.ConvertPositionToTarget(new Vector2(0f, this._layout.middleY), null);
		}
	}

	// Token: 0x170002A6 RID: 678
	// (get) Token: 0x06000ADB RID: 2779 RVA: 0x00058830 File Offset: 0x00056A30
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

	// Token: 0x06000ADC RID: 2780 RVA: 0x00058852 File Offset: 0x00056A52
	private void Start()
	{
		this.layout.groupAlpha = 0f;
	}

	// Token: 0x06000ADD RID: 2781 RVA: 0x00058864 File Offset: 0x00056A64
	private void Update()
	{
		if (!Game.loaded)
		{
			return;
		}
		this.UpdateRestPrompt();
	}

	// Token: 0x06000ADE RID: 2782 RVA: 0x00058874 File Offset: 0x00056A74
	private void UpdateRestPrompt()
	{
		bool flag = RestStateController.restAvailability >= RestAvailability.PromptDisabled && !MonoSingleton<RestStateController>.instance.active && !Game.gameplayPaused && !PhotoMode.visible;
		bool flag2 = RestStateController.restAvailability >= RestAvailability.PromptEnabled;
		float targetAlpha = (flag ? (flag2 ? 1f : 0.3f) : 0f);
		if (this.layout.targetGroupAlpha != targetAlpha)
		{
			this.layout.Animate(0.4f, delegate
			{
				this.layout.groupAlpha = targetAlpha;
			});
		}
		this.visibleAndEnabled = flag && flag2;
	}

	// Token: 0x04000D03 RID: 3331
	private SLayout _layout;
}
