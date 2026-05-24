using System;
using System.Collections.Generic;
using System.Reflection;

namespace InControl
{
	// Token: 0x02000078 RID: 120
	public static class Reflector
	{
		// Token: 0x1700017D RID: 381
		// (get) Token: 0x06000584 RID: 1412 RVA: 0x00013CE7 File Offset: 0x00011EE7
		public static IEnumerable<Type> AllAssemblyTypes
		{
			get
			{
				IEnumerable<Type> enumerable;
				if ((enumerable = Reflector.assemblyTypes) == null)
				{
					enumerable = (Reflector.assemblyTypes = Reflector.GetAllAssemblyTypes());
				}
				return enumerable;
			}
		}

		// Token: 0x06000585 RID: 1413 RVA: 0x00013D00 File Offset: 0x00011F00
		private static bool IgnoreAssemblyWithName(string assemblyName)
		{
			foreach (string text in Reflector.ignoreAssemblies)
			{
				if (assemblyName.StartsWith(text))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000586 RID: 1414 RVA: 0x00013D34 File Offset: 0x00011F34
		private static IEnumerable<Type> GetAllAssemblyTypes()
		{
			List<Type> list = new List<Type>();
			foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
			{
				if (!Reflector.IgnoreAssemblyWithName(assembly.GetName().Name))
				{
					Type[] array = null;
					try
					{
						array = assembly.GetTypes();
					}
					catch
					{
					}
					if (array != null)
					{
						list.AddRange(array);
					}
				}
			}
			return list;
		}

		// Token: 0x0400047A RID: 1146
		private static readonly string[] ignoreAssemblies = new string[]
		{
			"Unity", "UnityEngine", "UnityEditor", "mscorlib", "Microsoft", "System", "Mono", "JetBrains", "nunit", "ExCSS",
			"ICSharpCode", "AssetStoreTools"
		};

		// Token: 0x0400047B RID: 1147
		private static IEnumerable<Type> assemblyTypes;
	}
}
