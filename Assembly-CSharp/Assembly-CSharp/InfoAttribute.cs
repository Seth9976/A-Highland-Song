using System;
using UnityEngine;

// Token: 0x0200021F RID: 543
public class InfoAttribute : PropertyAttribute
{
	// Token: 0x0600139C RID: 5020 RVA: 0x00089AB5 File Offset: 0x00087CB5
	public InfoAttribute(string info)
	{
		this.info = info;
	}

	// Token: 0x040012CF RID: 4815
	public string info;
}
