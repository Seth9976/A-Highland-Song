using System;
using UnityEngine;

// Token: 0x02000153 RID: 339
public class ThrowPrompt : PromptWithVisibility
{
	// Token: 0x06000B7E RID: 2942 RVA: 0x0005C90A File Offset: 0x0005AB0A
	protected override bool ShouldShow()
	{
		return Runner.instance != null && Runner.instance.stoneSkimming && Runner.instance.stoneSkimSubState == Runner.SkimStoneSubState.Ready;
	}

	// Token: 0x170002B3 RID: 691
	// (get) Token: 0x06000B7F RID: 2943 RVA: 0x0005C934 File Offset: 0x0005AB34
	private RoundRect roundRect
	{
		get
		{
			if (this._roundRect == null)
			{
				this._roundRect = base.GetComponent<RoundRect>();
			}
			return this._roundRect;
		}
	}

	// Token: 0x06000B80 RID: 2944 RVA: 0x0005C956 File Offset: 0x0005AB56
	protected override void OnEnable()
	{
		base.OnEnable();
		Game.onUIPositionUpdate += this.OnUIPositionUpdate;
	}

	// Token: 0x06000B81 RID: 2945 RVA: 0x0005C96F File Offset: 0x0005AB6F
	private void OnDisable()
	{
		Game.onUIPositionUpdate -= this.OnUIPositionUpdate;
	}

	// Token: 0x06000B82 RID: 2946 RVA: 0x0005C984 File Offset: 0x0005AB84
	private void OnUIPositionUpdate(GameUI ui)
	{
		if (base.layout.groupAlpha == 0f)
		{
			return;
		}
		Vector2 vector = ui.WorldToCanvas(Runner.instance.transform.position + this.worldOffsetY * Vector3.up, default(Vector2));
		base.layout.origin = vector;
		if (Runner.instance.stoneSkimSubState == Runner.SkimStoneSubState.WindingThrow)
		{
			this.roundRect.fillColor = this.windingFillColor;
			return;
		}
		this.roundRect.fillColor = this.defaultFillColor;
	}

	// Token: 0x04000DB3 RID: 3507
	public float worldOffsetY = -5f;

	// Token: 0x04000DB4 RID: 3508
	public Color windingFillColor = Color.blue.WithAlpha(0.5f);

	// Token: 0x04000DB5 RID: 3509
	public Color defaultFillColor = Color.black.WithAlpha(0.5f);

	// Token: 0x04000DB6 RID: 3510
	private RoundRect _roundRect;
}
