using System;
using UnityEngine;

// Token: 0x02000172 RID: 370
[ExecuteInEditMode]
public class CaveLighting : MonoInstancer<CaveLighting>
{
	// Token: 0x170002D7 RID: 727
	// (get) Token: 0x06000C4C RID: 3148 RVA: 0x000626C8 File Offset: 0x000608C8
	public SpriteRenderer sprite
	{
		get
		{
			if (this._sprite == null)
			{
				this._sprite = base.GetComponent<SpriteRenderer>();
			}
			return this._sprite;
		}
	}

	// Token: 0x04000EBB RID: 3771
	private SpriteRenderer _sprite;
}
