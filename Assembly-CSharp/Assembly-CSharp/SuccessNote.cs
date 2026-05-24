using System;
using UnityEngine;

// Token: 0x02000081 RID: 129
public class SuccessNote : MonoBehaviour
{
	// Token: 0x17000108 RID: 264
	// (get) Token: 0x060003A7 RID: 935 RVA: 0x0001C97E File Offset: 0x0001AB7E
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

	// Token: 0x17000109 RID: 265
	// (get) Token: 0x060003A8 RID: 936 RVA: 0x0001C9A0 File Offset: 0x0001ABA0
	public SpriteRenderer spriteRenderer
	{
		get
		{
			if (this._spriteRenderer == null)
			{
				this._spriteRenderer = base.GetComponent<SpriteRenderer>();
			}
			return this._spriteRenderer;
		}
	}

	// Token: 0x040004F0 RID: 1264
	[Disable]
	public float age;

	// Token: 0x040004F1 RID: 1265
	private Prototype _prototype;

	// Token: 0x040004F2 RID: 1266
	private SpriteRenderer _spriteRenderer;
}
