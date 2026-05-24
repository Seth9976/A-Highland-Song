using System;

namespace UnityEngine.Scripting.APIUpdating
{
	// Token: 0x020002DE RID: 734
	internal struct MovedFromAttributeData
	{
		// Token: 0x06001DF8 RID: 7672 RVA: 0x00030B58 File Offset: 0x0002ED58
		public void Set(bool autoUpdateAPI, string sourceNamespace = null, string sourceAssembly = null, string sourceClassName = null)
		{
			this.className = sourceClassName;
			this.classHasChanged = this.className != null;
			this.nameSpace = sourceNamespace;
			this.nameSpaceHasChanged = this.nameSpace != null;
			this.assembly = sourceAssembly;
			this.assemblyHasChanged = this.assembly != null;
			this.autoUdpateAPI = autoUpdateAPI;
		}

		// Token: 0x040009D3 RID: 2515
		public string className;

		// Token: 0x040009D4 RID: 2516
		public string nameSpace;

		// Token: 0x040009D5 RID: 2517
		public string assembly;

		// Token: 0x040009D6 RID: 2518
		public bool classHasChanged;

		// Token: 0x040009D7 RID: 2519
		public bool nameSpaceHasChanged;

		// Token: 0x040009D8 RID: 2520
		public bool assemblyHasChanged;

		// Token: 0x040009D9 RID: 2521
		public bool autoUdpateAPI;
	}
}
