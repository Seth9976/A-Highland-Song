using System;

namespace UnityEngine.UIElements
{
	// Token: 0x020002AE RID: 686
	internal struct StyleVariable
	{
		// Token: 0x06001717 RID: 5911 RVA: 0x0005E489 File Offset: 0x0005C689
		public StyleVariable(string name, StyleSheet sheet, StyleValueHandle[] handles)
		{
			this.name = name;
			this.sheet = sheet;
			this.handles = handles;
		}

		// Token: 0x06001718 RID: 5912 RVA: 0x0005E4A4 File Offset: 0x0005C6A4
		public override int GetHashCode()
		{
			int num = this.name.GetHashCode();
			num = (num * 397) ^ this.sheet.GetHashCode();
			return (num * 397) ^ this.handles.GetHashCode();
		}

		// Token: 0x040009DB RID: 2523
		public readonly string name;

		// Token: 0x040009DC RID: 2524
		public readonly StyleSheet sheet;

		// Token: 0x040009DD RID: 2525
		public readonly StyleValueHandle[] handles;
	}
}
