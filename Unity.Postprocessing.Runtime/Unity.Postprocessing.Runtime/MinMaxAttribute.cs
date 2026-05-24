using System;

namespace UnityEngine.Rendering.PostProcessing
{
	// Token: 0x02000005 RID: 5
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
	public sealed class MinMaxAttribute : Attribute
	{
		// Token: 0x06000004 RID: 4 RVA: 0x0000207D File Offset: 0x0000027D
		public MinMaxAttribute(float min, float max)
		{
			this.min = min;
			this.max = max;
		}

		// Token: 0x04000004 RID: 4
		public readonly float min;

		// Token: 0x04000005 RID: 5
		public readonly float max;
	}
}
