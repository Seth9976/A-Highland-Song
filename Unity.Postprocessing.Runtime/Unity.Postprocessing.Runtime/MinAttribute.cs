using System;

namespace UnityEngine.Rendering.PostProcessing
{
	// Token: 0x02000004 RID: 4
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
	public sealed class MinAttribute : Attribute
	{
		// Token: 0x06000003 RID: 3 RVA: 0x0000206E File Offset: 0x0000026E
		public MinAttribute(float min)
		{
			this.min = min;
		}

		// Token: 0x04000003 RID: 3
		public readonly float min;
	}
}
