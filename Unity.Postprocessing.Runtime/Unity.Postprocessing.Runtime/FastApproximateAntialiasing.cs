using System;
using UnityEngine.Scripting;
using UnityEngine.Serialization;

namespace UnityEngine.Rendering.PostProcessing
{
	// Token: 0x02000022 RID: 34
	[Preserve]
	[Serializable]
	public sealed class FastApproximateAntialiasing
	{
		// Token: 0x04000087 RID: 135
		[FormerlySerializedAs("mobileOptimized")]
		[Tooltip("Boost performances by lowering the effect quality. This setting is meant to be used on mobile and other low-end platforms but can also provide a nice performance boost on desktops and consoles.")]
		public bool fastMode;

		// Token: 0x04000088 RID: 136
		[Tooltip("Keep alpha channel. This will slightly lower the effect quality but allows rendering against a transparent background.\nThis setting has no effect if the camera render target has no alpha channel.")]
		public bool keepAlpha;
	}
}
