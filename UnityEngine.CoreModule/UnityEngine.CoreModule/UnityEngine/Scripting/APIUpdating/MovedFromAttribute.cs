using System;

namespace UnityEngine.Scripting.APIUpdating
{
	// Token: 0x020002DF RID: 735
	[AttributeUsage(5148)]
	public class MovedFromAttribute : Attribute
	{
		// Token: 0x06001DF9 RID: 7673 RVA: 0x00030BB0 File Offset: 0x0002EDB0
		public MovedFromAttribute(bool autoUpdateAPI, string sourceNamespace = null, string sourceAssembly = null, string sourceClassName = null)
		{
			this.data.Set(autoUpdateAPI, sourceNamespace, sourceAssembly, sourceClassName);
		}

		// Token: 0x06001DFA RID: 7674 RVA: 0x00030BCB File Offset: 0x0002EDCB
		public MovedFromAttribute(string sourceNamespace)
		{
			this.data.Set(true, sourceNamespace, null, null);
		}

		// Token: 0x170005D3 RID: 1491
		// (get) Token: 0x06001DFB RID: 7675 RVA: 0x00030BE8 File Offset: 0x0002EDE8
		internal bool AffectsAPIUpdater
		{
			get
			{
				return !this.data.classHasChanged && !this.data.assemblyHasChanged;
			}
		}

		// Token: 0x170005D4 RID: 1492
		// (get) Token: 0x06001DFC RID: 7676 RVA: 0x00030C18 File Offset: 0x0002EE18
		public bool IsInDifferentAssembly
		{
			get
			{
				return this.data.assemblyHasChanged;
			}
		}

		// Token: 0x040009DA RID: 2522
		internal MovedFromAttributeData data;
	}
}
