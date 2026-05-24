using System;

namespace UnityEngine.Events
{
	// Token: 0x020002B5 RID: 693
	internal class UnityEventTools
	{
		// Token: 0x06001D0F RID: 7439 RVA: 0x0002E7CC File Offset: 0x0002C9CC
		internal static string TidyAssemblyTypeName(string assemblyTypeName)
		{
			bool flag = string.IsNullOrEmpty(assemblyTypeName);
			string text;
			if (flag)
			{
				text = assemblyTypeName;
			}
			else
			{
				int num = int.MaxValue;
				int num2 = assemblyTypeName.IndexOf(", Version=");
				bool flag2 = num2 != -1;
				if (flag2)
				{
					num = Math.Min(num2, num);
				}
				num2 = assemblyTypeName.IndexOf(", Culture=");
				bool flag3 = num2 != -1;
				if (flag3)
				{
					num = Math.Min(num2, num);
				}
				num2 = assemblyTypeName.IndexOf(", PublicKeyToken=");
				bool flag4 = num2 != -1;
				if (flag4)
				{
					num = Math.Min(num2, num);
				}
				bool flag5 = num != int.MaxValue;
				if (flag5)
				{
					assemblyTypeName = assemblyTypeName.Substring(0, num);
				}
				num2 = assemblyTypeName.IndexOf(", UnityEngine.");
				bool flag6 = num2 != -1 && assemblyTypeName.EndsWith("Module");
				if (flag6)
				{
					assemblyTypeName = assemblyTypeName.Substring(0, num2) + ", UnityEngine";
				}
				text = assemblyTypeName;
			}
			return text;
		}
	}
}
