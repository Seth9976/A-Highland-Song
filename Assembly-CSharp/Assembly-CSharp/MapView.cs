using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000127 RID: 295
public class MapView : MonoBehaviour
{
	// Token: 0x17000280 RID: 640
	// (get) Token: 0x06000A06 RID: 2566 RVA: 0x00054EF5 File Offset: 0x000530F5
	// (set) Token: 0x06000A07 RID: 2567 RVA: 0x00054EFD File Offset: 0x000530FD
	public Map map
	{
		get
		{
			return this._map;
		}
		set
		{
			this._map = value;
			if (this._map != null)
			{
				this._image.sprite = this._map.sprite;
			}
		}
	}

	// Token: 0x17000281 RID: 641
	// (get) Token: 0x06000A08 RID: 2568 RVA: 0x00054F2A File Offset: 0x0005312A
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

	// Token: 0x17000282 RID: 642
	// (get) Token: 0x06000A09 RID: 2569 RVA: 0x00054F4C File Offset: 0x0005314C
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

	// Token: 0x17000283 RID: 643
	// (get) Token: 0x06000A0A RID: 2570 RVA: 0x00054F6E File Offset: 0x0005316E
	// (set) Token: 0x06000A0B RID: 2571 RVA: 0x00054F7B File Offset: 0x0005317B
	public Color tint
	{
		get
		{
			return this._image.color;
		}
		set
		{
			this._image.color = value;
		}
	}

	// Token: 0x06000A0C RID: 2572 RVA: 0x00054F8C File Offset: 0x0005318C
	public void ReturnToPool()
	{
		this.layout.CancelAnimations();
		this.foundTick.groupAlpha = 0f;
		this.foundTick.scale = 1f;
		this.tint = Color.white;
		this.prototype.ReturnToPool();
	}

	// Token: 0x04000C35 RID: 3125
	public SLayout foundTick;

	// Token: 0x04000C36 RID: 3126
	private Map _map;

	// Token: 0x04000C37 RID: 3127
	private SLayout _layout;

	// Token: 0x04000C38 RID: 3128
	private Prototype _prototype;

	// Token: 0x04000C39 RID: 3129
	[SerializeField]
	private Image _image;
}
