using System;
using UnityEngine.Scripting;

namespace UnityEngine
{
	// Token: 0x02000206 RID: 518
	internal class EnumInfo
	{
		// Token: 0x060016B8 RID: 5816 RVA: 0x00024AC8 File Offset: 0x00022CC8
		[UsedByNativeCode]
		internal static EnumInfo CreateEnumInfoFromNativeEnum(string[] names, int[] values, string[] annotations, bool isFlags)
		{
			return new EnumInfo
			{
				names = names,
				values = values,
				annotations = annotations,
				isFlags = isFlags
			};
		}

		// Token: 0x040007F1 RID: 2033
		public string[] names;

		// Token: 0x040007F2 RID: 2034
		public int[] values;

		// Token: 0x040007F3 RID: 2035
		public string[] annotations;

		// Token: 0x040007F4 RID: 2036
		public bool isFlags;
	}
}
