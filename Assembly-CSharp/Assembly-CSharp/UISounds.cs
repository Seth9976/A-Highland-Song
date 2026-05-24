using System;
using UnityEngine;

// Token: 0x02000053 RID: 83
public class UISounds : ScriptableObject
{
	// Token: 0x0600024D RID: 589 RVA: 0x00014A3C File Offset: 0x00012C3C
	public AudioClip GetClip(UISound sound)
	{
		foreach (UISounds.UISoundClip uisoundClip in this.clips)
		{
			if (uisoundClip.sound == sound)
			{
				return uisoundClip.clip;
			}
		}
		return null;
	}

	// Token: 0x0400038A RID: 906
	public UISounds.UISoundClip[] clips;

	// Token: 0x02000281 RID: 641
	[Serializable]
	public struct UISoundClip
	{
		// Token: 0x040014EB RID: 5355
		public UISound sound;

		// Token: 0x040014EC RID: 5356
		public AudioClip clip;
	}
}
