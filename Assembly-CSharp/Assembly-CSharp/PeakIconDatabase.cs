using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000136 RID: 310
[CreateAssetMenu]
public class PeakIconDatabase : ScriptableObject
{
	// Token: 0x06000A6D RID: 2669 RVA: 0x00056828 File Offset: 0x00054A28
	public Sprite SpriteWithName(string peakInkName)
	{
		if (this._all.Count == 0)
		{
			this.LoadAll();
		}
		Sprite sprite;
		if (this._all.TryGetValue(peakInkName, out sprite))
		{
			return sprite;
		}
		return this.fallbackIcon;
	}

	// Token: 0x06000A6E RID: 2670 RVA: 0x00056860 File Offset: 0x00054A60
	public void LoadAll()
	{
		foreach (Sprite sprite in Resources.LoadAll<Sprite>("PeakIcons"))
		{
			this._all[sprite.name] = sprite;
		}
	}

	// Token: 0x04000C9F RID: 3231
	public Sprite fallbackIcon;

	// Token: 0x04000CA0 RID: 3232
	private Dictionary<string, Sprite> _all = new Dictionary<string, Sprite>();
}
