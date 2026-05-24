using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000123 RID: 291
public class MapMarker : MonoBehaviour
{
	// Token: 0x17000267 RID: 615
	// (get) Token: 0x060009B4 RID: 2484 RVA: 0x0005176D File Offset: 0x0004F96D
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

	// Token: 0x17000268 RID: 616
	// (get) Token: 0x060009B5 RID: 2485 RVA: 0x0005178F File Offset: 0x0004F98F
	// (set) Token: 0x060009B6 RID: 2486 RVA: 0x000517A0 File Offset: 0x0004F9A0
	public Color color
	{
		get
		{
			return this._graphicsToColor[0].color;
		}
		set
		{
			Graphic[] graphicsToColor = this._graphicsToColor;
			for (int i = 0; i < graphicsToColor.Length; i++)
			{
				graphicsToColor[i].color = value;
			}
		}
	}

	// Token: 0x060009B7 RID: 2487 RVA: 0x000517CB File Offset: 0x0004F9CB
	public void ReturnToPool()
	{
		this.beingRemoved = false;
		this.prototype.ReturnToPool();
	}

	// Token: 0x04000BC4 RID: 3012
	[Disable]
	public Vector3 worldPos;

	// Token: 0x04000BC5 RID: 3013
	[Disable]
	public bool beingRemoved;

	// Token: 0x04000BC6 RID: 3014
	[Disable]
	public Map map;

	// Token: 0x04000BC7 RID: 3015
	public SLayout layout;

	// Token: 0x04000BC8 RID: 3016
	public SLayout layoutForDistanceScale;

	// Token: 0x04000BC9 RID: 3017
	private Prototype _prototype;

	// Token: 0x04000BCA RID: 3018
	[SerializeField]
	private Graphic[] _graphicsToColor;
}
