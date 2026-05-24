using System;
using TMPro;
using UnityEngine;

// Token: 0x02000105 RID: 261
public class ChoicesWidgetChoice : MonoBehaviour
{
	// Token: 0x1700023A RID: 570
	// (get) Token: 0x060008BD RID: 2237 RVA: 0x0004A012 File Offset: 0x00048212
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

	// Token: 0x1700023B RID: 571
	// (get) Token: 0x060008BE RID: 2238 RVA: 0x0004A034 File Offset: 0x00048234
	// (set) Token: 0x060008BF RID: 2239 RVA: 0x0004A03C File Offset: 0x0004823C
	public bool visible
	{
		get
		{
			return this._visible;
		}
		set
		{
			if (this.visible != value)
			{
				this._visible = value;
				this.PerformLayout();
			}
		}
	}

	// Token: 0x1700023C RID: 572
	// (get) Token: 0x060008C0 RID: 2240 RVA: 0x0004A054 File Offset: 0x00048254
	// (set) Token: 0x060008C1 RID: 2241 RVA: 0x0004A05C File Offset: 0x0004825C
	public bool highlighted
	{
		get
		{
			return this._highlighted;
		}
		set
		{
			if (this._highlighted != value)
			{
				this._highlighted = value;
				this.PerformLayout();
			}
		}
	}

	// Token: 0x1700023D RID: 573
	// (get) Token: 0x060008C2 RID: 2242 RVA: 0x0004A074 File Offset: 0x00048274
	public float preferredWidth
	{
		get
		{
			float x = this._iconWithBg.x;
			float preferredWidth = this.textLayout.textMeshPro.preferredWidth;
			return this.textLayout.rightX + x;
		}
	}

	// Token: 0x1700023E RID: 574
	// (get) Token: 0x060008C3 RID: 2243 RVA: 0x0004A0AB File Offset: 0x000482AB
	private SLayout textLayout
	{
		get
		{
			if (this._textLayout == null)
			{
				this._textLayout = base.GetComponentInChildren<TextMeshProUGUI>().GetComponent<SLayout>();
			}
			return this._textLayout;
		}
	}

	// Token: 0x060008C4 RID: 2244 RVA: 0x0004A0D4 File Offset: 0x000482D4
	public void Setup(GameChoice choice, ChoicesWidgetSettings settings)
	{
		this.choice = choice;
		this._settings = settings;
		this.textLayout.textMeshPro.text = InkStylingUtility.ParseStyling(InkStylingUtility.ProcessText(choice.text, false, false), true, "#FFE473");
		this._highlighted = false;
		this._visible = false;
		ChoiceIcon choiceIcon = choice.icon;
		if (choice.specialType == GameChoiceSpecialType.Rest && choiceIcon == ChoiceIcon.None)
		{
			choiceIcon = ChoiceIcon.Time;
		}
		this._icon.image.sprite = settings.SpriteForIcon(choiceIcon);
		this.PerformLayout();
	}

	// Token: 0x060008C5 RID: 2245 RVA: 0x0004A158 File Offset: 0x00048358
	private void Start()
	{
		Prototype component = base.GetComponent<Prototype>();
		if (!component.isOriginalPrototype)
		{
			component.OnReturnToPool += this.OnReturnToPool;
		}
		this.layout.groupAlpha = 0f;
	}

	// Token: 0x060008C6 RID: 2246 RVA: 0x0004A196 File Offset: 0x00048396
	private void OnDestroy()
	{
		base.GetComponent<Prototype>().OnReturnToPool -= this.OnReturnToPool;
	}

	// Token: 0x060008C7 RID: 2247 RVA: 0x0004A1AF File Offset: 0x000483AF
	private void OnReturnToPool()
	{
		this.layout.CancelAnimations();
		this.choice = default(GameChoice);
		this._highlighted = false;
		this._visible = false;
		this.layout.groupAlpha = 0f;
	}

	// Token: 0x060008C8 RID: 2248 RVA: 0x0004A1E8 File Offset: 0x000483E8
	private void PerformLayout()
	{
		float preferredWidth = this.textLayout.textMeshPro.preferredWidth;
		this.textLayout.width = preferredWidth;
		this.actionIconLayout.groupAlpha = (this.highlighted ? this._settings.actionIconMaxAlpha : 0f);
		if (!this._visible)
		{
			this.layout.groupAlpha = 0f;
		}
		else if (this._highlighted)
		{
			this.layout.groupAlpha = 1f;
		}
		else
		{
			this.layout.groupAlpha = this._settings.unhighlightedAlpha;
		}
		this.layout.width = this.targetWidth;
	}

	// Token: 0x060008C9 RID: 2249 RVA: 0x0004A292 File Offset: 0x00048492
	public void ChooseAnimation()
	{
		this.actionIconLayout.groupAlpha = 0f;
	}

	// Token: 0x04000A6F RID: 2671
	public GameChoice choice;

	// Token: 0x04000A70 RID: 2672
	private SLayout _layout;

	// Token: 0x04000A71 RID: 2673
	[SerializeField]
	[Disable]
	private bool _visible;

	// Token: 0x04000A72 RID: 2674
	[SerializeField]
	[Disable]
	private bool _highlighted;

	// Token: 0x04000A73 RID: 2675
	public float targetWidth;

	// Token: 0x04000A74 RID: 2676
	private SLayout _textLayout;

	// Token: 0x04000A75 RID: 2677
	[SerializeField]
	private SLayout actionIconLayout;

	// Token: 0x04000A76 RID: 2678
	[SerializeField]
	private SLayout _iconWithBg;

	// Token: 0x04000A77 RID: 2679
	[SerializeField]
	private SLayout _icon;

	// Token: 0x04000A78 RID: 2680
	private ChoicesWidgetSettings _settings;
}
