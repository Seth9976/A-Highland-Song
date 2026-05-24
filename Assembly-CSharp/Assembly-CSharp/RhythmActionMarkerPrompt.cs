using System;
using ActionIcon;
using UnityEngine;

// Token: 0x0200007D RID: 125
public class RhythmActionMarkerPrompt : MonoBehaviour
{
	// Token: 0x170000EE RID: 238
	// (get) Token: 0x06000379 RID: 889 RVA: 0x0001B3A0 File Offset: 0x000195A0
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

	// Token: 0x170000EF RID: 239
	// (get) Token: 0x0600037A RID: 890 RVA: 0x0001B3C2 File Offset: 0x000195C2
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

	// Token: 0x170000F0 RID: 240
	// (get) Token: 0x0600037B RID: 891 RVA: 0x0001B3E4 File Offset: 0x000195E4
	private ActionIconView actionIconView
	{
		get
		{
			if (this._actionIconView == null)
			{
				this._actionIconView = base.GetComponentInChildren<ActionIconView>();
			}
			return this._actionIconView;
		}
	}

	// Token: 0x0600037C RID: 892 RVA: 0x0001B406 File Offset: 0x00019606
	public void Setup(bool isSpecialJump)
	{
		if (isSpecialJump)
		{
			this.actionIconView.actionName = "Special jump";
			this.actionIconView.variant = IconVariant.Highlight;
			return;
		}
		this.actionIconView.actionName = "Jump";
		this.actionIconView.variant = IconVariant.Standard;
	}

	// Token: 0x040004BE RID: 1214
	[NonSerialized]
	public RhythmActionMarker marker;

	// Token: 0x040004BF RID: 1215
	[NonSerialized]
	public Vector3 worldPos;

	// Token: 0x040004C0 RID: 1216
	[NonSerialized]
	public float visibility = 1f;

	// Token: 0x040004C1 RID: 1217
	private Prototype _prototype;

	// Token: 0x040004C2 RID: 1218
	private SLayout _layout;

	// Token: 0x040004C3 RID: 1219
	private ActionIconView _actionIconView;
}
