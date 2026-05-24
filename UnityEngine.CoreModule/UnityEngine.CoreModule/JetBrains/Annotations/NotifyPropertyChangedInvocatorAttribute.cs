using System;

namespace JetBrains.Annotations
{
	// Token: 0x020000BC RID: 188
	[AttributeUsage(64)]
	public sealed class NotifyPropertyChangedInvocatorAttribute : Attribute
	{
		// Token: 0x06000345 RID: 837 RVA: 0x00002059 File Offset: 0x00000259
		public NotifyPropertyChangedInvocatorAttribute()
		{
		}

		// Token: 0x06000346 RID: 838 RVA: 0x00005CBC File Offset: 0x00003EBC
		public NotifyPropertyChangedInvocatorAttribute([NotNull] string parameterName)
		{
			this.ParameterName = parameterName;
		}

		// Token: 0x17000081 RID: 129
		// (get) Token: 0x06000347 RID: 839 RVA: 0x00005CCD File Offset: 0x00003ECD
		[CanBeNull]
		public string ParameterName { get; }
	}
}
