using System;
using UnityEngine;

// Token: 0x0200005C RID: 92
public class Vocalisations : ScriptableObject
{
	// Token: 0x0600027D RID: 637 RVA: 0x00015305 File Offset: 0x00013505
	public AudioClip GetVolcalisationClip(Vocalisation voc)
	{
		return this.vocalisations[(int)voc].clips.Random<AudioClip>();
	}

	// Token: 0x0600027E RID: 638 RVA: 0x00015320 File Offset: 0x00013520
	private void OnValidate()
	{
		int length = Enum.GetValues(typeof(Vocalisation)).Length;
		if (this.vocalisations != null && length == this.vocalisations.Length)
		{
			for (int i = 0; i < length; i++)
			{
				if (this.vocalisations[i].vocalisation != (Vocalisation)i)
				{
					goto IL_0049;
				}
			}
			return;
		}
		IL_0049:
		Vocalisations.VocalisationSet[] array = new Vocalisations.VocalisationSet[length];
		for (int j = 0; j < length; j++)
		{
			Vocalisation vocalisation = (Vocalisation)j;
			if (this.vocalisations != null)
			{
				foreach (Vocalisations.VocalisationSet vocalisationSet in this.vocalisations)
				{
					if (vocalisationSet.vocalisation == vocalisation)
					{
						array[j] = vocalisationSet;
						break;
					}
				}
			}
			if (array[j].vocalisation != vocalisation)
			{
				array[j].vocalisation = vocalisation;
				array[j].clips = new AudioClip[0];
			}
		}
		this.vocalisations = array;
	}

	// Token: 0x040003C5 RID: 965
	public Vocalisations.VocalisationSet[] vocalisations;

	// Token: 0x02000284 RID: 644
	[Serializable]
	public struct VocalisationSet
	{
		// Token: 0x040014EF RID: 5359
		public Vocalisation vocalisation;

		// Token: 0x040014F0 RID: 5360
		public int priority;

		// Token: 0x040014F1 RID: 5361
		public AudioClip[] clips;
	}
}
