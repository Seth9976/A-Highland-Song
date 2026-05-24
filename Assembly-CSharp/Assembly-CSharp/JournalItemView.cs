using System;
using UnityEngine;

// Token: 0x02000119 RID: 281
public class JournalItemView : MonoBehaviour
{
	// Token: 0x1700025F RID: 607
	// (get) Token: 0x06000993 RID: 2451 RVA: 0x00050E30 File Offset: 0x0004F030
	public SLayout layout
	{
		get
		{
			if (this._layout == null)
			{
				this._layout = base.GetComponent<SLayout>();
				this.originalHeight = this._layout.height;
				if (this.text1 != null)
				{
					this.originalText1Color = this.text1.color;
				}
			}
			return this._layout;
		}
	}

	// Token: 0x17000260 RID: 608
	// (get) Token: 0x06000994 RID: 2452 RVA: 0x00050E8D File Offset: 0x0004F08D
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

	// Token: 0x06000995 RID: 2453 RVA: 0x00050EAF File Offset: 0x0004F0AF
	public void SetTicked(bool completed)
	{
		if (this.tick != null)
		{
			this.tick.gameObject.SetActive(completed);
		}
	}

	// Token: 0x06000996 RID: 2454 RVA: 0x00050ED0 File Offset: 0x0004F0D0
	public void SetImageFaded(bool faded)
	{
		if (this.image != null)
		{
			this.image.color = (faded ? this.incompleteTint : Color.white);
		}
	}

	// Token: 0x06000997 RID: 2455 RVA: 0x00050EFB File Offset: 0x0004F0FB
	public void SetLit(bool lit)
	{
		if (this.litIcon != null)
		{
			this.litIcon.gameObject.SetActive(lit);
		}
	}

	// Token: 0x06000998 RID: 2456 RVA: 0x00050F1C File Offset: 0x0004F11C
	public void SetDisabled(bool disabled)
	{
		this.disabled = disabled;
		this.layout.groupAlpha = (disabled ? 0.3f : 1f);
	}

	// Token: 0x04000B76 RID: 2934
	private SLayout _layout;

	// Token: 0x04000B77 RID: 2935
	private Prototype _prototype;

	// Token: 0x04000B78 RID: 2936
	public SLayout text1;

	// Token: 0x04000B79 RID: 2937
	public SLayout text2;

	// Token: 0x04000B7A RID: 2938
	public SLayout image;

	// Token: 0x04000B7B RID: 2939
	public SLayout tick;

	// Token: 0x04000B7C RID: 2940
	public SLayout litIcon;

	// Token: 0x04000B7D RID: 2941
	public SLayout highlight;

	// Token: 0x04000B7E RID: 2942
	public Color incompleteTint = Color.white;

	// Token: 0x04000B7F RID: 2943
	public bool isMap;

	// Token: 0x04000B80 RID: 2944
	public bool disabled;

	// Token: 0x04000B81 RID: 2945
	[NonSerialized]
	public float originalHeight;

	// Token: 0x04000B82 RID: 2946
	[NonSerialized]
	public Color originalText1Color;

	// Token: 0x04000B83 RID: 2947
	[NonSerialized]
	public JournalItemView.OnHighlightChanged onHighlightChanged;

	// Token: 0x04000B84 RID: 2948
	[NonSerialized]
	public Action onSelect;

	// Token: 0x02000359 RID: 857
	// (Invoke) Token: 0x06001748 RID: 5960
	public delegate void OnHighlightChanged(JournalItemView itemView, bool selected);
}
