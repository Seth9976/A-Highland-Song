using System;
using System.Runtime.CompilerServices;
using UnityEngine;

// Token: 0x02000074 RID: 116
[NullableContext(1)]
[Nullable(new byte[] { 0, 1 })]
public class MusicLibrary : MonoSingleton<MusicLibrary>
{
	// Token: 0x0600034C RID: 844 RVA: 0x0001A51C File Offset: 0x0001871C
	public BeatTrackSection GetBeatTrack(string runInkName, float trackDuration)
	{
		float num = trackDuration / 60f;
		string text = Narrative.instance.ChooseBeatTrack(runInkName, num);
		foreach (BeatTrack beatTrack in this.trackLibrary)
		{
			foreach (BeatTrack.Section section in beatTrack.sections)
			{
				if (section.inkName == text)
				{
					return new BeatTrackSection
					{
						track = beatTrack,
						section = section
					};
				}
			}
		}
		Debug.LogWarning("No beat track section found in library with BeatTrack.inkName " + text);
		BeatTrack beatTrack2 = this.trackLibrary.Random<BeatTrack>();
		BeatTrack.Section section2 = beatTrack2.sections.Random<BeatTrack.Section>();
		return new BeatTrackSection
		{
			track = beatTrack2,
			section = section2
		};
	}

	// Token: 0x04000483 RID: 1155
	public BeatTrack[] trackLibrary = new BeatTrack[0];
}
