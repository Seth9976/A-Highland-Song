using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000C0 RID: 192
public class BreathingSFXSettings : ScriptableObject
{
	// Token: 0x04000700 RID: 1792
	public float runningGameplayVolume = 1f;

	// Token: 0x04000701 RID: 1793
	[Space]
	public AnimationCurve targetExhaustionOverStaminaRecovering;

	// Token: 0x04000702 RID: 1794
	public AnimationCurve exhaustionDeltaOverStaminaRecovering;

	// Token: 0x04000703 RID: 1795
	public AnimationCurve targetExhaustionOverStaminaIdle;

	// Token: 0x04000704 RID: 1796
	public AnimationCurve exhaustionDeltaOverStaminaIdle;

	// Token: 0x04000705 RID: 1797
	public AnimationCurve targetExhaustionOverStaminaRunning;

	// Token: 0x04000706 RID: 1798
	public AnimationCurve exhaustionDeltaOverStaminaRunning;

	// Token: 0x04000707 RID: 1799
	public AnimationCurve targetExhaustionOverStaminaClimbing;

	// Token: 0x04000708 RID: 1800
	public AnimationCurve exhaustionDeltaOverStaminaClimbing;

	// Token: 0x04000709 RID: 1801
	[Space]
	public AnimationCurve breathVolumeOverExhaustion;

	// Token: 0x0400070A RID: 1802
	public AnimationCurve breathTimeOverExhaustion;

	// Token: 0x0400070B RID: 1803
	[Space]
	public List<MusicGameAudioClip> lightBreathingClips;

	// Token: 0x0400070C RID: 1804
	public List<MusicGameAudioClip> regularBreathingClips;

	// Token: 0x0400070D RID: 1805
	public List<MusicGameAudioClip> heavyBreathingClips;

	// Token: 0x0400070E RID: 1806
	public List<MusicGameAudioClip> heaviestBreathingClips;

	// Token: 0x0400070F RID: 1807
	public List<MusicGameAudioClip> recoveryPantingClips;
}
