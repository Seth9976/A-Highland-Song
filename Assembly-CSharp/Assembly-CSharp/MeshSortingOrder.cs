using System;
using UnityEngine;

// Token: 0x020000AE RID: 174
[ExecuteInEditMode]
public class MeshSortingOrder : MonoBehaviour
{
	// Token: 0x17000167 RID: 359
	// (get) Token: 0x06000588 RID: 1416 RVA: 0x0002BE25 File Offset: 0x0002A025
	private MeshRenderer meshRenderer
	{
		get
		{
			if (this._meshRenderer == null)
			{
				this._meshRenderer = base.GetComponent<MeshRenderer>();
			}
			return this._meshRenderer;
		}
	}

	// Token: 0x06000589 RID: 1417 RVA: 0x0002BE47 File Offset: 0x0002A047
	private void Update()
	{
		this.meshRenderer.sortingOrder = this.sortingOrder;
	}

	// Token: 0x0600058A RID: 1418 RVA: 0x0002BE5A File Offset: 0x0002A05A
	private void OnDisable()
	{
		this.meshRenderer.sortingOrder = 0;
		this._meshRenderer = null;
	}

	// Token: 0x04000659 RID: 1625
	public int sortingOrder;

	// Token: 0x0400065A RID: 1626
	private MeshRenderer _meshRenderer;
}
