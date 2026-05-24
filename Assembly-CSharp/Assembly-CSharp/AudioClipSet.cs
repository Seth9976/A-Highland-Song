using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Token: 0x02000009 RID: 9
public class AudioClipSet : ScriptableObject
{
	// Token: 0x06000033 RID: 51 RVA: 0x00005474 File Offset: 0x00003674
	public AudioClip GetRandomClip()
	{
		if (this.audioClips.IsEmpty<AudioClip>())
		{
			Debug.LogWarning("No AudioClips in set " + base.name, this);
			return null;
		}
		int num = Mathf.FloorToInt((float)this.audioClips.Count * 0.5f);
		if (this.lastUsedClips == null)
		{
			this.lastUsedClips = new Queue<AudioClip>(num);
		}
		if (this.lastUsedClips.Count > 0 && this.lastUsedClips.Count >= num)
		{
			this.lastUsedClips.Dequeue();
		}
		AudioClip audioClip = this.audioClips.Except(this.lastUsedClips).Random<AudioClip>();
		this.lastUsedClips.Enqueue(audioClip);
		return audioClip;
	}

	// Token: 0x0400003F RID: 63
	public List<AudioClip> audioClips;

	// Token: 0x04000040 RID: 64
	private Queue<AudioClip> lastUsedClips;
}
