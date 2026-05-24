using System;
using UnityEngine;

// Token: 0x02000144 RID: 324
public abstract class SettingControl : MonoBehaviour
{
	// Token: 0x170002A7 RID: 679
	// (get) Token: 0x06000AEE RID: 2798 RVA: 0x000590D9 File Offset: 0x000572D9
	// (set) Token: 0x06000AEF RID: 2799 RVA: 0x000590E1 File Offset: 0x000572E1
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
				this.RefreshLayout();
			}
		}
	}

	// Token: 0x170002A8 RID: 680
	// (get) Token: 0x06000AF0 RID: 2800 RVA: 0x000590F9 File Offset: 0x000572F9
	// (set) Token: 0x06000AF1 RID: 2801 RVA: 0x00059101 File Offset: 0x00057301
	public bool disabled
	{
		get
		{
			return this._disabled;
		}
		set
		{
			if (this._disabled != value)
			{
				this._disabled = value;
				this.RefreshLayout();
			}
		}
	}

	// Token: 0x170002A9 RID: 681
	// (get) Token: 0x06000AF2 RID: 2802 RVA: 0x00059119 File Offset: 0x00057319
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

	// Token: 0x170002AA RID: 682
	// (get) Token: 0x06000AF3 RID: 2803 RVA: 0x0005913B File Offset: 0x0005733B
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

	// Token: 0x06000AF4 RID: 2804
	public abstract void Trigger();

	// Token: 0x06000AF5 RID: 2805
	public abstract void LeftRight(int amount);

	// Token: 0x06000AF6 RID: 2806
	public abstract void RefreshLayout();

	// Token: 0x04000D14 RID: 3348
	protected bool _highlighted;

	// Token: 0x04000D15 RID: 3349
	protected bool _disabled;

	// Token: 0x04000D16 RID: 3350
	public SettingControlSettings settings;

	// Token: 0x04000D17 RID: 3351
	private SLayout _layout;

	// Token: 0x04000D18 RID: 3352
	private Prototype _prototype;
}
