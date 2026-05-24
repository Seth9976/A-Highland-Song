using System;
using UnityEngine;

// Token: 0x02000193 RID: 403
[CreateAssetMenu]
public class LighthouseLightSettings : ScriptableObject
{
	// Token: 0x04001015 RID: 4117
	public AnimationCurve alpha = AnimationCurve.Constant(0f, 1f, 1f);

	// Token: 0x04001016 RID: 4118
	public AnimationCurve alphaNight = AnimationCurve.Constant(0f, 1f, 1f);

	// Token: 0x04001017 RID: 4119
	public AnimationCurve scaleX = AnimationCurve.Constant(0f, 1f, 1f);

	// Token: 0x04001018 RID: 4120
	public AnimationCurve scaleY = AnimationCurve.Constant(0f, 1f, 1f);

	// Token: 0x04001019 RID: 4121
	public float tNormOffset;
}
