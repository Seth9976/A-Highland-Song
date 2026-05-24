using System;
using UnityEngine;

// Token: 0x02000158 RID: 344
[CreateAssetMenu]
public class CloudSpriteSet : ScriptableObject
{
	// Token: 0x170002BC RID: 700
	// (get) Token: 0x06000BB6 RID: 2998 RVA: 0x0005E57F File Offset: 0x0005C77F
	public bool isValid
	{
		get
		{
			return this.main != null;
		}
	}

	// Token: 0x04000DD7 RID: 3543
	public Sprite main;

	// Token: 0x04000DD8 RID: 3544
	public Sprite highlight;

	// Token: 0x04000DD9 RID: 3545
	public Sprite softMain;

	// Token: 0x04000DDA RID: 3546
	public Sprite softHighlight;
}
