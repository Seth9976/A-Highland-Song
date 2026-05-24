using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine.Bindings;
using UnityEngineInternal;

namespace UnityEngine
{
	// Token: 0x020001EA RID: 490
	[NativeHeader("Runtime/Misc/ResourceManagerUtility.h")]
	[NativeHeader("Runtime/Export/Resources/Resources.bindings.h")]
	public sealed class Resources
	{
		// Token: 0x06001621 RID: 5665 RVA: 0x0002365C File Offset: 0x0002185C
		internal static T[] ConvertObjects<T>(Object[] rawObjects) where T : Object
		{
			bool flag = rawObjects == null;
			T[] array;
			if (flag)
			{
				array = null;
			}
			else
			{
				T[] array2 = new T[rawObjects.Length];
				for (int i = 0; i < array2.Length; i++)
				{
					array2[i] = (T)((object)rawObjects[i]);
				}
				array = array2;
			}
			return array;
		}

		// Token: 0x06001622 RID: 5666 RVA: 0x000236A8 File Offset: 0x000218A8
		public static Object[] FindObjectsOfTypeAll(Type type)
		{
			return ResourcesAPI.ActiveAPI.FindObjectsOfTypeAll(type);
		}

		// Token: 0x06001623 RID: 5667 RVA: 0x000236C8 File Offset: 0x000218C8
		public static T[] FindObjectsOfTypeAll<T>() where T : Object
		{
			return Resources.ConvertObjects<T>(Resources.FindObjectsOfTypeAll(typeof(T)));
		}

		// Token: 0x06001624 RID: 5668 RVA: 0x000236F0 File Offset: 0x000218F0
		public static Object Load(string path)
		{
			return Resources.Load(path, typeof(Object));
		}

		// Token: 0x06001625 RID: 5669 RVA: 0x00023714 File Offset: 0x00021914
		public static T Load<T>(string path) where T : Object
		{
			return (T)((object)Resources.Load(path, typeof(T)));
		}

		// Token: 0x06001626 RID: 5670 RVA: 0x0002373C File Offset: 0x0002193C
		public static Object Load(string path, Type systemTypeInstance)
		{
			return ResourcesAPI.ActiveAPI.Load(path, systemTypeInstance);
		}

		// Token: 0x06001627 RID: 5671 RVA: 0x0002375C File Offset: 0x0002195C
		public static ResourceRequest LoadAsync(string path)
		{
			return Resources.LoadAsync(path, typeof(Object));
		}

		// Token: 0x06001628 RID: 5672 RVA: 0x00023780 File Offset: 0x00021980
		public static ResourceRequest LoadAsync<T>(string path) where T : Object
		{
			return Resources.LoadAsync(path, typeof(T));
		}

		// Token: 0x06001629 RID: 5673 RVA: 0x000237A4 File Offset: 0x000219A4
		public static ResourceRequest LoadAsync(string path, Type type)
		{
			return ResourcesAPI.ActiveAPI.LoadAsync(path, type);
		}

		// Token: 0x0600162A RID: 5674 RVA: 0x000237C4 File Offset: 0x000219C4
		public static Object[] LoadAll(string path, Type systemTypeInstance)
		{
			return ResourcesAPI.ActiveAPI.LoadAll(path, systemTypeInstance);
		}

		// Token: 0x0600162B RID: 5675 RVA: 0x000237E4 File Offset: 0x000219E4
		public static Object[] LoadAll(string path)
		{
			return Resources.LoadAll(path, typeof(Object));
		}

		// Token: 0x0600162C RID: 5676 RVA: 0x00023808 File Offset: 0x00021A08
		public static T[] LoadAll<T>(string path) where T : Object
		{
			return Resources.ConvertObjects<T>(Resources.LoadAll(path, typeof(T)));
		}

		// Token: 0x0600162D RID: 5677
		[FreeFunction("GetScriptingBuiltinResource", ThrowsException = true)]
		[TypeInferenceRule(TypeInferenceRules.TypeReferencedByFirstArgument)]
		[MethodImpl(4096)]
		public static extern Object GetBuiltinResource([NotNull("ArgumentNullException")] Type type, string path);

		// Token: 0x0600162E RID: 5678 RVA: 0x00023830 File Offset: 0x00021A30
		public static T GetBuiltinResource<T>(string path) where T : Object
		{
			return (T)((object)Resources.GetBuiltinResource(typeof(T), path));
		}

		// Token: 0x0600162F RID: 5679 RVA: 0x00023857 File Offset: 0x00021A57
		public static void UnloadAsset(Object assetToUnload)
		{
			ResourcesAPI.ActiveAPI.UnloadAsset(assetToUnload);
		}

		// Token: 0x06001630 RID: 5680
		[FreeFunction("Scripting::UnloadAssetFromScripting")]
		[MethodImpl(4096)]
		private static extern void UnloadAssetImplResourceManager(Object assetToUnload);

		// Token: 0x06001631 RID: 5681
		[FreeFunction("Resources_Bindings::UnloadUnusedAssets")]
		[MethodImpl(4096)]
		public static extern AsyncOperation UnloadUnusedAssets();

		// Token: 0x06001632 RID: 5682
		[FreeFunction("Resources_Bindings::InstanceIDToObject")]
		[MethodImpl(4096)]
		public static extern Object InstanceIDToObject(int instanceID);

		// Token: 0x06001633 RID: 5683
		[FreeFunction("Resources_Bindings::InstanceIDToObjectList")]
		[MethodImpl(4096)]
		private static extern void InstanceIDToObjectList(IntPtr instanceIDs, int instanceCount, List<Object> objects);

		// Token: 0x06001634 RID: 5684 RVA: 0x00023868 File Offset: 0x00021A68
		public static void InstanceIDToObjectList(NativeArray<int> instanceIDs, List<Object> objects)
		{
			bool flag = !instanceIDs.IsCreated;
			if (flag)
			{
				throw new ArgumentException("NativeArray is uninitialized", "instanceIDs");
			}
			bool flag2 = objects == null;
			if (flag2)
			{
				throw new ArgumentNullException("objects");
			}
			bool flag3 = instanceIDs.Length == 0;
			if (flag3)
			{
				objects.Clear();
			}
			else
			{
				Resources.InstanceIDToObjectList((IntPtr)instanceIDs.GetUnsafeReadOnlyPtr<int>(), instanceIDs.Length, objects);
			}
		}
	}
}
