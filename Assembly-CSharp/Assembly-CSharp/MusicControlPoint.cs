using System;
using UnityEngine;

// Token: 0x02000197 RID: 407
[RequireComponent(typeof(TriggerZone))]
public class MusicControlPoint : MonoBehaviour
{
	// Token: 0x04001026 RID: 4134
	[Tooltip("Force the grace period (e.g. 5+ seconds) to end where the music carries on playing in a non-music area if the player is just passes on, so we don't lose musical momentum.")]
	public bool forceStopMusic;

	// Token: 0x04001027 RID: 4135
	[Tooltip("If music is already playing it'll ramp down and ramp up again. If this is null it'll be ignored.")]
	public BeatTrack changeTrack;

	// Token: 0x04001028 RID: 4136
	[Tooltip("-1 = ignore, otherwise change to the specific bar either in the specified track or the current one. e.g. can pass 0 to restart any music track to start. ")]
	public int barIdx = -1;
}
