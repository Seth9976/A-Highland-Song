using System;
using System.Runtime.CompilerServices;
using UnityEngine;

// Token: 0x020000CE RID: 206
[NullableContext(1)]
[Nullable(0)]
public class AnimSettings : ScriptableObject
{
	// Token: 0x04000842 RID: 2114
	public float speedExpectedForAnim = 10f;

	// Token: 0x04000843 RID: 2115
	public float speedExpectedUphillForAnim = 10f;

	// Token: 0x04000844 RID: 2116
	public float speedExpectedDownhillForAnim = 10f;

	// Token: 0x04000845 RID: 2117
	public AnimationClip runClipForDuration = Presume<AnimationClip>.NonNull("C:\\Users\\josep\\Desktop\\highland-song\\Highland Run\\Assets\\Scripts\\Runner\\Settings\\AnimSettings.cs", 15);

	// Token: 0x04000846 RID: 2118
	public AnimationClip readyCrouchClipForDuration = Presume<AnimationClip>.NonNull("C:\\Users\\josep\\Desktop\\highland-song\\Highland Run\\Assets\\Scripts\\Runner\\Settings\\AnimSettings.cs", 16);
}
