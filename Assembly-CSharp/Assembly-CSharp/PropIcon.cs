using System;
using System.Runtime.CompilerServices;
using UnityEngine;

// Token: 0x0200013C RID: 316
[NullableContext(1)]
[Nullable(0)]
public class PropIcon : MonoBehaviour
{
	// Token: 0x06000AB0 RID: 2736 RVA: 0x0005809D File Offset: 0x0005629D
	private void Start()
	{
		base.GetComponent<Prototype>().OnReturnToPool += this.OnReturnToPool;
	}

	// Token: 0x06000AB1 RID: 2737 RVA: 0x000580B6 File Offset: 0x000562B6
	private void OnDestroy()
	{
		base.GetComponent<Prototype>().OnReturnToPool -= this.OnReturnToPool;
	}

	// Token: 0x06000AB2 RID: 2738 RVA: 0x000580CF File Offset: 0x000562CF
	private void OnEnable()
	{
		Game.onUIPositionUpdate += this.OnUIPositionUpdate;
	}

	// Token: 0x06000AB3 RID: 2739 RVA: 0x000580E2 File Offset: 0x000562E2
	private void OnDisable()
	{
		Game.onUIPositionUpdate -= this.OnUIPositionUpdate;
	}

	// Token: 0x06000AB4 RID: 2740 RVA: 0x000580F5 File Offset: 0x000562F5
	private void OnReturnToPool()
	{
		this.prop = null;
	}

	// Token: 0x1700029F RID: 671
	// (get) Token: 0x06000AB5 RID: 2741 RVA: 0x000580FE File Offset: 0x000562FE
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

	// Token: 0x06000AB6 RID: 2742 RVA: 0x00058120 File Offset: 0x00056320
	private void OnUIPositionUpdate(GameUI ui)
	{
		if (this.prop == null)
		{
			return;
		}
		this.layout.origin = ui.WorldToCanvas(this.prop.transform.position, default(Vector2));
	}

	// Token: 0x04000CEB RID: 3307
	[Nullable(2)]
	[NonSerialized]
	public Prop prop;

	// Token: 0x04000CEC RID: 3308
	[Nullable(2)]
	private SLayout _layout;
}
