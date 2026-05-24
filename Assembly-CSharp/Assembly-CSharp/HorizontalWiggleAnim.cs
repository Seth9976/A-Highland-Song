using System;
using UnityEngine;

// Token: 0x02000114 RID: 276
public class HorizontalWiggleAnim : MonoBehaviour
{
	// Token: 0x17000251 RID: 593
	// (get) Token: 0x0600093A RID: 2362 RVA: 0x0004DA29 File Offset: 0x0004BC29
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

	// Token: 0x0600093B RID: 2363 RVA: 0x0004DA4B File Offset: 0x0004BC4B
	private void Start()
	{
		this._baseX = this.layout.originX;
	}

	// Token: 0x0600093C RID: 2364 RVA: 0x0004DA5E File Offset: 0x0004BC5E
	private void Update()
	{
		this.layout.originX = this._baseX + this.amount * Mathf.Sin(this.speed * Time.unscaledTime);
	}

	// Token: 0x04000B1E RID: 2846
	public float speed = 10f;

	// Token: 0x04000B1F RID: 2847
	public float amount = 10f;

	// Token: 0x04000B20 RID: 2848
	private SLayout _layout;

	// Token: 0x04000B21 RID: 2849
	private float _baseX;
}
