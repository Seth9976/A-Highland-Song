using System;

namespace UnityEngine.Rendering.PostProcessing
{
	// Token: 0x02000007 RID: 7
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
	public sealed class TrackballAttribute : Attribute
	{
		// Token: 0x06000007 RID: 7 RVA: 0x000020E3 File Offset: 0x000002E3
		public TrackballAttribute(TrackballAttribute.Mode mode)
		{
			this.mode = mode;
		}

		// Token: 0x0400000B RID: 11
		public readonly TrackballAttribute.Mode mode;

		// Token: 0x02000069 RID: 105
		public enum Mode
		{
			// Token: 0x0400024A RID: 586
			None,
			// Token: 0x0400024B RID: 587
			Lift,
			// Token: 0x0400024C RID: 588
			Gamma,
			// Token: 0x0400024D RID: 589
			Gain
		}
	}
}
