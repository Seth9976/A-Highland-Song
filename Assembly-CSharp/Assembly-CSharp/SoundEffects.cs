using System;
using UnityEngine;

// Token: 0x02000041 RID: 65
public class SoundEffects : ScriptableObject
{
	// Token: 0x060001F6 RID: 502 RVA: 0x0001187C File Offset: 0x0000FA7C
	public AudioClip GetClip(SoundEffect sound, int cycleIdx = 0)
	{
		SoundEffects.SFXClip[] array = this.clips;
		int i = 0;
		while (i < array.Length)
		{
			SoundEffects.SFXClip sfxclip = array[i];
			if (sfxclip.sound == sound)
			{
				if (sfxclip.clips == null || sfxclip.clips.Length == 0)
				{
					return null;
				}
				return sfxclip.clips[cycleIdx % sfxclip.clips.Length];
			}
			else
			{
				i++;
			}
		}
		return null;
	}

	// Token: 0x040002C1 RID: 705
	public SoundEffects.SFXClip[] clips;

	// Token: 0x02000275 RID: 629
	[Serializable]
	public struct SFXClip
	{
		// Token: 0x040014C3 RID: 5315
		public SoundEffect sound;

		// Token: 0x040014C4 RID: 5316
		public AudioClip[] clips;
	}
}
