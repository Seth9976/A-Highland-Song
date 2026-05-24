using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200013D RID: 317
public class PropWidget : MonoBehaviour
{
	// Token: 0x170002A0 RID: 672
	// (get) Token: 0x06000AB8 RID: 2744 RVA: 0x0005816E File Offset: 0x0005636E
	public SLayout layout
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

	// Token: 0x170002A1 RID: 673
	// (get) Token: 0x06000AB9 RID: 2745 RVA: 0x00058190 File Offset: 0x00056390
	// (set) Token: 0x06000ABA RID: 2746 RVA: 0x00058198 File Offset: 0x00056398
	public PropWidget.State state
	{
		get
		{
			return this._state;
		}
		set
		{
			if (this._state != value)
			{
				this._state = value;
				this.layout.Animate(0.2f, new Action(this.Layout));
			}
		}
	}

	// Token: 0x06000ABB RID: 2747 RVA: 0x000581C8 File Offset: 0x000563C8
	public void Setup(Prop prop, PropWidget.State initialState, List<GameChoice> choices)
	{
		this.layout.groupAlpha = 0f;
		this.prop = prop;
		base.gameObject.name = "Prop Choices Widget UI: " + prop.name;
		this.choicesWidget.Clear();
		if (choices != null)
		{
			this.choicesWidget.SetChoices(choices, 3, false, false);
		}
		this._attractDotTransition = 0f;
		this._timeAlive = 0f;
		this._activeAttractDotSettings = this.attractDotSettings;
		if (prop.hasTakePathChoice)
		{
			this._activeAttractDotSettings = this.pathAttractDotSettings;
		}
		else if (prop.hasRestChoice)
		{
			this._activeAttractDotSettings = this.shelterAttractDotSettings;
		}
		this.attractDotImage.sprite = this._activeAttractDotSettings.sprite;
		this.state = initialState;
		this.Layout();
	}

	// Token: 0x06000ABC RID: 2748 RVA: 0x00058293 File Offset: 0x00056493
	public void FadeOutAndRemove()
	{
		this.layout.Animate(0.2f, delegate
		{
			this.state = PropWidget.State.Hidden;
		}).Then(delegate
		{
			base.GetComponent<Prototype>().ReturnToPool();
		});
	}

	// Token: 0x06000ABD RID: 2749 RVA: 0x000582C4 File Offset: 0x000564C4
	private void Start()
	{
		Prototype component = base.GetComponent<Prototype>();
		if (!component.isOriginalPrototype)
		{
			component.OnReturnToPool += this.OnReturnToPool;
		}
	}

	// Token: 0x06000ABE RID: 2750 RVA: 0x000582F2 File Offset: 0x000564F2
	private void OnDestroy()
	{
		base.GetComponent<Prototype>().OnReturnToPool -= this.OnReturnToPool;
	}

	// Token: 0x06000ABF RID: 2751 RVA: 0x0005830B File Offset: 0x0005650B
	private void OnEnable()
	{
		Game.onUIPositionUpdate += this.OnUIPositionUpdate;
	}

	// Token: 0x06000AC0 RID: 2752 RVA: 0x0005831E File Offset: 0x0005651E
	private void OnDisable()
	{
		Game.onUIPositionUpdate -= this.OnUIPositionUpdate;
	}

	// Token: 0x06000AC1 RID: 2753 RVA: 0x00058331 File Offset: 0x00056531
	private void Update()
	{
		this._timeAlive += Time.deltaTime;
		this.UpdateAttractDot();
	}

	// Token: 0x06000AC2 RID: 2754 RVA: 0x0005834C File Offset: 0x0005654C
	private void UpdateAttractDot()
	{
		ChoicesWidgetSettings settings = this.choicesWidget.settings;
		if (this.state == PropWidget.State.Attract)
		{
			this._attractDotTransition += Time.deltaTime / this._activeAttractDotSettings.initialPulseTime;
			if (this._attractDotTransition > 1f)
			{
				this._attractDotTransition = 1f;
			}
		}
		else
		{
			this._attractDotTransition -= Time.deltaTime / this._activeAttractDotSettings.hideTime;
			if (this._attractDotTransition < 0f)
			{
				this._attractDotTransition = 0f;
			}
		}
		float num = this._activeAttractDotSettings.popOpenCurve.Evaluate(this._attractDotTransition);
		float num2 = 0.5f * (Mathf.Sin(this._timeAlive * this._activeAttractDotSettings.pulseSpeed) + 1f);
		float num3 = this._activeAttractDotSettings.pulseRange.Lerp(num2);
		this.attractLayout.scale = num * num3;
		this.attractDotImage.color = this._activeAttractDotSettings.color;
	}

	// Token: 0x06000AC3 RID: 2755 RVA: 0x0005844D File Offset: 0x0005664D
	private void OnReturnToPool()
	{
		this.layout.CancelAnimations();
		SLayout.WithoutAnimating(delegate
		{
			this.state = PropWidget.State.Hidden;
		});
		this.prop = null;
		this.choicesWidget.Clear();
	}

	// Token: 0x06000AC4 RID: 2756 RVA: 0x00058480 File Offset: 0x00056680
	private void Layout()
	{
		if (this.state == PropWidget.State.Hidden)
		{
			this.layout.groupAlpha = 0f;
		}
		else
		{
			this.layout.groupAlpha = 1f;
		}
		if (this.state == PropWidget.State.Attract)
		{
			this.attractLayout.groupAlpha = 1f;
		}
		else
		{
			this.attractLayout.groupAlpha = 0f;
		}
		this.choicesWidget.layout.groupAlpha = (float)((this.state == PropWidget.State.Normal) ? 1 : 0);
		this.choicesWidget.layout.originX = 0f;
		SLayout.WithoutAnimating(delegate
		{
			this.UpdateAttractDot();
		});
	}

	// Token: 0x06000AC5 RID: 2757 RVA: 0x00058528 File Offset: 0x00056728
	private void OnUIPositionUpdate(GameUI ui)
	{
		if (this.prop == null)
		{
			return;
		}
		this.layout.center = ui.WorldToCanvas(this.prop.widgetAttachPos, default(Vector2));
	}

	// Token: 0x04000CED RID: 3309
	public AttractDotSettings attractDotSettings;

	// Token: 0x04000CEE RID: 3310
	public AttractDotSettings shelterAttractDotSettings;

	// Token: 0x04000CEF RID: 3311
	public AttractDotSettings pathAttractDotSettings;

	// Token: 0x04000CF0 RID: 3312
	private SLayout _layout;

	// Token: 0x04000CF1 RID: 3313
	[NonSerialized]
	public Prop prop;

	// Token: 0x04000CF2 RID: 3314
	public SLayout attractLayout;

	// Token: 0x04000CF3 RID: 3315
	public Image attractDotImage;

	// Token: 0x04000CF4 RID: 3316
	public ChoicesWidget choicesWidget;

	// Token: 0x04000CF5 RID: 3317
	public PropWidget.State _state;

	// Token: 0x04000CF6 RID: 3318
	private float _attractDotTransition;

	// Token: 0x04000CF7 RID: 3319
	private float _timeAlive;

	// Token: 0x04000CF8 RID: 3320
	private AttractDotSettings _activeAttractDotSettings;

	// Token: 0x02000372 RID: 882
	public enum State
	{
		// Token: 0x040018E3 RID: 6371
		Hidden,
		// Token: 0x040018E4 RID: 6372
		Attract,
		// Token: 0x040018E5 RID: 6373
		Normal
	}
}
