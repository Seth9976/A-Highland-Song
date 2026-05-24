using System;
using UnityEngine;

// Token: 0x02000131 RID: 305
public class PeakWidget : MonoBehaviour
{
	// Token: 0x1700028F RID: 655
	// (get) Token: 0x06000A46 RID: 2630 RVA: 0x00055CCF File Offset: 0x00053ECF
	public bool isDisabled
	{
		get
		{
			return this.prop.isPathOut && this.map == null;
		}
	}

	// Token: 0x17000290 RID: 656
	// (get) Token: 0x06000A47 RID: 2631 RVA: 0x00055CEC File Offset: 0x00053EEC
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

	// Token: 0x17000291 RID: 657
	// (get) Token: 0x06000A48 RID: 2632 RVA: 0x00055D0E File Offset: 0x00053F0E
	public Prototype prototype
	{
		get
		{
			if (this._prototype == null)
			{
				this._prototype = base.GetComponent<Prototype>();
			}
			return this._prototype;
		}
	}

	// Token: 0x06000A49 RID: 2633 RVA: 0x00055D30 File Offset: 0x00053F30
	public void Setup(Prop prop, string name, Map map)
	{
		this.prop = prop;
		this.map = map;
		this.levelIdx = prop.levelIdx;
		this.layout.groupAlpha = 0f;
		this.layout.scale = 1f;
		this._icon.color = Color.white;
		this._iconCircleBg.alpha = 0f;
		this._textWithShadow.groupAlpha = 0f;
		this.maxAlpha = 1f;
		this._icon.image.sprite = (prop.isPeak ? this.peakSprite : this.pathSprite);
		if (name != null && name.Length > 0)
		{
			this._text.textMeshPro.text = name;
			Vector2 preferredValues = this._text.textMeshPro.GetPreferredValues(name, this._text.width, 500f);
			this._textShadow.height = preferredValues.y + 34f;
			this._textWithShadow.gameObject.SetActive(true);
			return;
		}
		this._textWithShadow.gameObject.SetActive(false);
	}

	// Token: 0x06000A4A RID: 2634 RVA: 0x00055E51 File Offset: 0x00054051
	public void Focus()
	{
		this.layout.Animate(0.2f, delegate
		{
			this._iconCircleBg.alpha = 1f;
			this._icon.color = Color.black;
			this._textWithShadow.groupAlpha = 1f;
			this.layout.scale = 1.1f;
		});
	}

	// Token: 0x06000A4B RID: 2635 RVA: 0x00055E70 File Offset: 0x00054070
	public void UnFocus()
	{
		this.layout.Animate(0.5f, delegate
		{
			this._iconCircleBg.alpha = 0f;
			this._icon.color = Color.white;
			this._textWithShadow.groupAlpha = 0f;
			this.layout.scale = 1f;
		});
	}

	// Token: 0x04000C72 RID: 3186
	public Sprite pathSprite;

	// Token: 0x04000C73 RID: 3187
	public Sprite peakSprite;

	// Token: 0x04000C74 RID: 3188
	[Disable]
	public Prop prop;

	// Token: 0x04000C75 RID: 3189
	[Disable]
	public Map map;

	// Token: 0x04000C76 RID: 3190
	[Disable]
	public bool isOnScreen;

	// Token: 0x04000C77 RID: 3191
	[Disable]
	public bool isOccluded;

	// Token: 0x04000C78 RID: 3192
	[Disable]
	public int occlusionIndex;

	// Token: 0x04000C79 RID: 3193
	[Disable]
	public int levelIdx;

	// Token: 0x04000C7A RID: 3194
	[Disable]
	public float maxAlpha = 1f;

	// Token: 0x04000C7B RID: 3195
	[Disable]
	public float distanceForSorting;

	// Token: 0x04000C7C RID: 3196
	[Disable]
	public bool wantsVisible;

	// Token: 0x04000C7D RID: 3197
	private SLayout _layout;

	// Token: 0x04000C7E RID: 3198
	private Prototype _prototype;

	// Token: 0x04000C7F RID: 3199
	[SerializeField]
	private SLayout _text;

	// Token: 0x04000C80 RID: 3200
	[SerializeField]
	private SLayout _icon;

	// Token: 0x04000C81 RID: 3201
	[SerializeField]
	private SLayout _iconCircleBg;

	// Token: 0x04000C82 RID: 3202
	[SerializeField]
	private SLayout _textWithShadow;

	// Token: 0x04000C83 RID: 3203
	[SerializeField]
	private SLayout _textShadow;
}
