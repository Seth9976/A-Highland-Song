using System;

namespace UnityEngine.Rendering.PostProcessing
{
	// Token: 0x02000002 RID: 2
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
	public sealed class DisplayNameAttribute : Attribute
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		public DisplayNameAttribute(string displayName)
		{
			this.displayName = displayName;
		}

		// Token: 0x04000001 RID: 1
		public readonly string displayName;
	}
}
