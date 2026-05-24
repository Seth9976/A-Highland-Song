using System;
using UnityEngine;

// Token: 0x02000043 RID: 67
public class Stings : ScriptableObject
{
	// Token: 0x060001F8 RID: 504 RVA: 0x000118E0 File Offset: 0x0000FAE0
	public AudioClip GetStingClip(Sting voc, int cycleSeed = 0)
	{
		AudioClip[] clips = this.stings[(int)voc].clips;
		if (clips.Length == 0)
		{
			return null;
		}
		return clips[cycleSeed % clips.Length];
	}

	// Token: 0x060001F9 RID: 505 RVA: 0x0001190C File Offset: 0x0000FB0C
	private void OnValidate()
	{
		int length = Enum.GetValues(typeof(Sting)).Length;
		if (this.stings != null && length == this.stings.Length)
		{
			for (int i = 0; i < length; i++)
			{
				if (this.stings[i].sting != (Sting)i)
				{
					goto IL_0049;
				}
			}
			return;
		}
		IL_0049:
		Stings.StingSet[] array = new Stings.StingSet[length];
		for (int j = 0; j < length; j++)
		{
			Sting sting = (Sting)j;
			if (this.stings != null)
			{
				foreach (Stings.StingSet stingSet in this.stings)
				{
					if (stingSet.sting == sting)
					{
						array[j] = stingSet;
						break;
					}
				}
			}
			if (array[j].sting != sting)
			{
				array[j].sting = sting;
				array[j].clips = new AudioClip[0];
			}
		}
		this.stings = array;
	}

	// Token: 0x040002CB RID: 715
	public Stings.StingSet[] stings;

	// Token: 0x02000276 RID: 630
	[Serializable]
	public struct StingSet
	{
		// Token: 0x040014C5 RID: 5317
		public Sting sting;

		// Token: 0x040014C6 RID: 5318
		public int priority;

		// Token: 0x040014C7 RID: 5319
		public AudioClip[] clips;
	}
}
