using System;
using UnityEngine;

// Token: 0x0200003F RID: 63
public class SleepGlow : MonoBehaviour
{
	// Token: 0x170000AD RID: 173
	// (get) Token: 0x060001F0 RID: 496 RVA: 0x00011785 File Offset: 0x0000F985
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

	// Token: 0x060001F1 RID: 497 RVA: 0x000117A7 File Offset: 0x0000F9A7
	private void OnEnable()
	{
		if (this._maxAlpha == 0f)
		{
			this._maxAlpha = this.layout.alpha;
		}
		this.layout.alpha = 0f;
	}

	// Token: 0x060001F2 RID: 498 RVA: 0x000117D8 File Offset: 0x0000F9D8
	private void Update()
	{
		bool sleeping = MonoSingleton<RestStateController>.instance.sleeping;
		bool flag = this.layout.targetAlpha > 0f;
		if (sleeping && !flag)
		{
			this.layout.Animate(2f, delegate
			{
				this.layout.alpha = this._maxAlpha;
			});
			return;
		}
		if (!sleeping && flag)
		{
			this.layout.Animate(2f, delegate
			{
				this.layout.alpha = 0f;
			});
		}
	}

	// Token: 0x040002B5 RID: 693
	private SLayout _layout;

	// Token: 0x040002B6 RID: 694
	private float _maxAlpha;
}
