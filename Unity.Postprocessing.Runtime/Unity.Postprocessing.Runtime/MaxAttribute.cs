using System;

namespace UnityEngine.Rendering.PostProcessing
{
	// Token: 0x02000003 RID: 3
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
	public sealed class MaxAttribute : Attribute
	{
		// Token: 0x06000002 RID: 2 RVA: 0x0000205F File Offset: 0x0000025F
		public MaxAttribute(float max)
		{
			this.max = max;
		}

		// Token: 0x04000002 RID: 2
		public readonly float max;
	}
}
