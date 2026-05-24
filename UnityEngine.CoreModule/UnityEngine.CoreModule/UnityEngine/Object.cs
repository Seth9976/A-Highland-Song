using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security;
using UnityEngine.Bindings;
using UnityEngine.Internal;
using UnityEngine.Scripting;
using UnityEngineInternal;

namespace UnityEngine
{
	// Token: 0x02000227 RID: 551
	[NativeHeader("Runtime/SceneManager/SceneManager.h")]
	[NativeHeader("Runtime/Export/Scripting/UnityEngineObject.bindings.h")]
	[RequiredByNativeCode(GenerateProxy = true)]
	[NativeHeader("Runtime/GameCode/CloneObject.h")]
	[StructLayout(0)]
	public class Object
	{
		// Token: 0x0600178E RID: 6030 RVA: 0x000261E0 File Offset: 0x000243E0
		[SecuritySafeCritical]
		public unsafe int GetInstanceID()
		{
			bool flag = this.m_CachedPtr == IntPtr.Zero;
			int num;
			if (flag)
			{
				num = 0;
			}
			else
			{
				bool flag2 = Object.OffsetOfInstanceIDInCPlusPlusObject == -1;
				if (flag2)
				{
					Object.OffsetOfInstanceIDInCPlusPlusObject = Object.GetOffsetOfInstanceIDInCPlusPlusObject();
				}
				num = *(int*)(void*)new IntPtr(this.m_CachedPtr.ToInt64() + (long)Object.OffsetOfInstanceIDInCPlusPlusObject);
			}
			return num;
		}

		// Token: 0x0600178F RID: 6031 RVA: 0x00026240 File Offset: 0x00024440
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		// Token: 0x06001790 RID: 6032 RVA: 0x00026258 File Offset: 0x00024458
		public override bool Equals(object other)
		{
			Object @object = other as Object;
			bool flag = @object == null && other != null && !(other is Object);
			return !flag && Object.CompareBaseObjects(this, @object);
		}

		// Token: 0x06001791 RID: 6033 RVA: 0x0002629C File Offset: 0x0002449C
		public static implicit operator bool(Object exists)
		{
			return !Object.CompareBaseObjects(exists, null);
		}

		// Token: 0x06001792 RID: 6034 RVA: 0x000262B8 File Offset: 0x000244B8
		private static bool CompareBaseObjects(Object lhs, Object rhs)
		{
			bool flag = lhs == null;
			bool flag2 = rhs == null;
			bool flag3 = flag2 && flag;
			bool flag4;
			if (flag3)
			{
				flag4 = true;
			}
			else
			{
				bool flag5 = flag2;
				if (flag5)
				{
					flag4 = !Object.IsNativeObjectAlive(lhs);
				}
				else
				{
					bool flag6 = flag;
					if (flag6)
					{
						flag4 = !Object.IsNativeObjectAlive(rhs);
					}
					else
					{
						flag4 = lhs == rhs;
					}
				}
			}
			return flag4;
		}

		// Token: 0x06001793 RID: 6035 RVA: 0x0002630C File Offset: 0x0002450C
		private void EnsureRunningOnMainThread()
		{
			bool flag = !Object.CurrentThreadIsMainThread();
			if (flag)
			{
				throw new InvalidOperationException("EnsureRunningOnMainThread can only be called from the main thread");
			}
		}

		// Token: 0x06001794 RID: 6036 RVA: 0x00026334 File Offset: 0x00024534
		private static bool IsNativeObjectAlive(Object o)
		{
			return o.GetCachedPtr() != IntPtr.Zero;
		}

		// Token: 0x06001795 RID: 6037 RVA: 0x00026358 File Offset: 0x00024558
		private IntPtr GetCachedPtr()
		{
			return this.m_CachedPtr;
		}

		// Token: 0x1700048B RID: 1163
		// (get) Token: 0x06001796 RID: 6038 RVA: 0x00026370 File Offset: 0x00024570
		// (set) Token: 0x06001797 RID: 6039 RVA: 0x00026388 File Offset: 0x00024588
		public string name
		{
			get
			{
				return Object.GetName(this);
			}
			set
			{
				Object.SetName(this, value);
			}
		}

		// Token: 0x06001798 RID: 6040 RVA: 0x00026394 File Offset: 0x00024594
		[TypeInferenceRule(TypeInferenceRules.TypeOfFirstArgument)]
		public static Object Instantiate(Object original, Vector3 position, Quaternion rotation)
		{
			Object.CheckNullArgument(original, "The Object you want to instantiate is null.");
			bool flag = original is ScriptableObject;
			if (flag)
			{
				throw new ArgumentException("Cannot instantiate a ScriptableObject with a position and rotation");
			}
			Object @object = Object.Internal_InstantiateSingle(original, position, rotation);
			bool flag2 = @object == null;
			if (flag2)
			{
				throw new UnityException("Instantiate failed because the clone was destroyed during creation. This can happen if DestroyImmediate is called in MonoBehaviour.Awake.");
			}
			return @object;
		}

		// Token: 0x06001799 RID: 6041 RVA: 0x000263EC File Offset: 0x000245EC
		[TypeInferenceRule(TypeInferenceRules.TypeOfFirstArgument)]
		public static Object Instantiate(Object original, Vector3 position, Quaternion rotation, Transform parent)
		{
			bool flag = parent == null;
			Object @object;
			if (flag)
			{
				@object = Object.Instantiate(original, position, rotation);
			}
			else
			{
				Object.CheckNullArgument(original, "The Object you want to instantiate is null.");
				Object object2 = Object.Internal_InstantiateSingleWithParent(original, parent, position, rotation);
				bool flag2 = object2 == null;
				if (flag2)
				{
					throw new UnityException("Instantiate failed because the clone was destroyed during creation. This can happen if DestroyImmediate is called in MonoBehaviour.Awake.");
				}
				@object = object2;
			}
			return @object;
		}

		// Token: 0x0600179A RID: 6042 RVA: 0x00026444 File Offset: 0x00024644
		[TypeInferenceRule(TypeInferenceRules.TypeOfFirstArgument)]
		public static Object Instantiate(Object original)
		{
			Object.CheckNullArgument(original, "The Object you want to instantiate is null.");
			Object @object = Object.Internal_CloneSingle(original);
			bool flag = @object == null;
			if (flag)
			{
				throw new UnityException("Instantiate failed because the clone was destroyed during creation. This can happen if DestroyImmediate is called in MonoBehaviour.Awake.");
			}
			return @object;
		}

		// Token: 0x0600179B RID: 6043 RVA: 0x00026480 File Offset: 0x00024680
		[TypeInferenceRule(TypeInferenceRules.TypeOfFirstArgument)]
		public static Object Instantiate(Object original, Transform parent)
		{
			return Object.Instantiate(original, parent, false);
		}

		// Token: 0x0600179C RID: 6044 RVA: 0x0002649C File Offset: 0x0002469C
		[TypeInferenceRule(TypeInferenceRules.TypeOfFirstArgument)]
		public static Object Instantiate(Object original, Transform parent, bool instantiateInWorldSpace)
		{
			bool flag = parent == null;
			Object @object;
			if (flag)
			{
				@object = Object.Instantiate(original);
			}
			else
			{
				Object.CheckNullArgument(original, "The Object you want to instantiate is null.");
				Object object2 = Object.Internal_CloneSingleWithParent(original, parent, instantiateInWorldSpace);
				bool flag2 = object2 == null;
				if (flag2)
				{
					throw new UnityException("Instantiate failed because the clone was destroyed during creation. This can happen if DestroyImmediate is called in MonoBehaviour.Awake.");
				}
				@object = object2;
			}
			return @object;
		}

		// Token: 0x0600179D RID: 6045 RVA: 0x000264F0 File Offset: 0x000246F0
		public static T Instantiate<T>(T original) where T : Object
		{
			Object.CheckNullArgument(original, "The Object you want to instantiate is null.");
			T t = (T)((object)Object.Internal_CloneSingle(original));
			bool flag = t == null;
			if (flag)
			{
				throw new UnityException("Instantiate failed because the clone was destroyed during creation. This can happen if DestroyImmediate is called in MonoBehaviour.Awake.");
			}
			return t;
		}

		// Token: 0x0600179E RID: 6046 RVA: 0x00026540 File Offset: 0x00024740
		public static T Instantiate<T>(T original, Vector3 position, Quaternion rotation) where T : Object
		{
			return (T)((object)Object.Instantiate(original, position, rotation));
		}

		// Token: 0x0600179F RID: 6047 RVA: 0x00026564 File Offset: 0x00024764
		public static T Instantiate<T>(T original, Vector3 position, Quaternion rotation, Transform parent) where T : Object
		{
			return (T)((object)Object.Instantiate(original, position, rotation, parent));
		}

		// Token: 0x060017A0 RID: 6048 RVA: 0x0002658C File Offset: 0x0002478C
		public static T Instantiate<T>(T original, Transform parent) where T : Object
		{
			return Object.Instantiate<T>(original, parent, false);
		}

		// Token: 0x060017A1 RID: 6049 RVA: 0x000265A8 File Offset: 0x000247A8
		public static T Instantiate<T>(T original, Transform parent, bool worldPositionStays) where T : Object
		{
			return (T)((object)Object.Instantiate(original, parent, worldPositionStays));
		}

		// Token: 0x060017A2 RID: 6050
		[NativeMethod(Name = "Scripting::DestroyObjectFromScripting", IsFreeFunction = true, ThrowsException = true)]
		[MethodImpl(4096)]
		public static extern void Destroy(Object obj, [DefaultValue("0.0F")] float t);

		// Token: 0x060017A3 RID: 6051 RVA: 0x000265CC File Offset: 0x000247CC
		[ExcludeFromDocs]
		public static void Destroy(Object obj)
		{
			float num = 0f;
			Object.Destroy(obj, num);
		}

		// Token: 0x060017A4 RID: 6052
		[NativeMethod(Name = "Scripting::DestroyObjectFromScriptingImmediate", IsFreeFunction = true, ThrowsException = true)]
		[MethodImpl(4096)]
		public static extern void DestroyImmediate(Object obj, [DefaultValue("false")] bool allowDestroyingAssets);

		// Token: 0x060017A5 RID: 6053 RVA: 0x000265E8 File Offset: 0x000247E8
		[ExcludeFromDocs]
		public static void DestroyImmediate(Object obj)
		{
			bool flag = false;
			Object.DestroyImmediate(obj, flag);
		}

		// Token: 0x060017A6 RID: 6054 RVA: 0x00026600 File Offset: 0x00024800
		public static Object[] FindObjectsOfType(Type type)
		{
			return Object.FindObjectsOfType(type, false);
		}

		// Token: 0x060017A7 RID: 6055
		[FreeFunction("UnityEngineObjectBindings::FindObjectsOfType")]
		[TypeInferenceRule(TypeInferenceRules.ArrayOfTypeReferencedByFirstArgument)]
		[MethodImpl(4096)]
		public static extern Object[] FindObjectsOfType(Type type, bool includeInactive);

		// Token: 0x060017A8 RID: 6056 RVA: 0x0002661C File Offset: 0x0002481C
		public static Object[] FindObjectsByType(Type type, FindObjectsSortMode sortMode)
		{
			return Object.FindObjectsByType(type, FindObjectsInactive.Exclude, sortMode);
		}

		// Token: 0x060017A9 RID: 6057
		[TypeInferenceRule(TypeInferenceRules.ArrayOfTypeReferencedByFirstArgument)]
		[FreeFunction("UnityEngineObjectBindings::FindObjectsByType")]
		[MethodImpl(4096)]
		public static extern Object[] FindObjectsByType(Type type, FindObjectsInactive findObjectsInactive, FindObjectsSortMode sortMode);

		// Token: 0x060017AA RID: 6058
		[FreeFunction("GetSceneManager().DontDestroyOnLoad", ThrowsException = true)]
		[MethodImpl(4096)]
		public static extern void DontDestroyOnLoad([NotNull("NullExceptionObject")] Object target);

		// Token: 0x1700048C RID: 1164
		// (get) Token: 0x060017AB RID: 6059
		// (set) Token: 0x060017AC RID: 6060
		public extern HideFlags hideFlags
		{
			[MethodImpl(4096)]
			get;
			[MethodImpl(4096)]
			set;
		}

		// Token: 0x060017AD RID: 6061 RVA: 0x00026636 File Offset: 0x00024836
		[Obsolete("use Object.Destroy instead.")]
		public static void DestroyObject(Object obj, [DefaultValue("0.0F")] float t)
		{
			Object.Destroy(obj, t);
		}

		// Token: 0x060017AE RID: 6062 RVA: 0x00026644 File Offset: 0x00024844
		[ExcludeFromDocs]
		[Obsolete("use Object.Destroy instead.")]
		public static void DestroyObject(Object obj)
		{
			float num = 0f;
			Object.Destroy(obj, num);
		}

		// Token: 0x060017AF RID: 6063 RVA: 0x00026660 File Offset: 0x00024860
		[Obsolete("warning use Object.FindObjectsByType instead.")]
		public static Object[] FindSceneObjectsOfType(Type type)
		{
			return Object.FindObjectsOfType(type);
		}

		// Token: 0x060017B0 RID: 6064
		[FreeFunction("UnityEngineObjectBindings::FindObjectsOfTypeIncludingAssets")]
		[Obsolete("use Resources.FindObjectsOfTypeAll instead.")]
		[MethodImpl(4096)]
		public static extern Object[] FindObjectsOfTypeIncludingAssets(Type type);

		// Token: 0x060017B1 RID: 6065 RVA: 0x00026678 File Offset: 0x00024878
		public static T[] FindObjectsOfType<T>() where T : Object
		{
			return Resources.ConvertObjects<T>(Object.FindObjectsOfType(typeof(T), false));
		}

		// Token: 0x060017B2 RID: 6066 RVA: 0x000266A0 File Offset: 0x000248A0
		public static T[] FindObjectsByType<T>(FindObjectsSortMode sortMode) where T : Object
		{
			return Resources.ConvertObjects<T>(Object.FindObjectsByType(typeof(T), FindObjectsInactive.Exclude, sortMode));
		}

		// Token: 0x060017B3 RID: 6067 RVA: 0x000266C8 File Offset: 0x000248C8
		public static T[] FindObjectsOfType<T>(bool includeInactive) where T : Object
		{
			return Resources.ConvertObjects<T>(Object.FindObjectsOfType(typeof(T), includeInactive));
		}

		// Token: 0x060017B4 RID: 6068 RVA: 0x000266F0 File Offset: 0x000248F0
		public static T[] FindObjectsByType<T>(FindObjectsInactive findObjectsInactive, FindObjectsSortMode sortMode) where T : Object
		{
			return Resources.ConvertObjects<T>(Object.FindObjectsByType(typeof(T), findObjectsInactive, sortMode));
		}

		// Token: 0x060017B5 RID: 6069 RVA: 0x00026718 File Offset: 0x00024918
		public static T FindObjectOfType<T>() where T : Object
		{
			return (T)((object)Object.FindObjectOfType(typeof(T), false));
		}

		// Token: 0x060017B6 RID: 6070 RVA: 0x00026740 File Offset: 0x00024940
		public static T FindObjectOfType<T>(bool includeInactive) where T : Object
		{
			return (T)((object)Object.FindObjectOfType(typeof(T), includeInactive));
		}

		// Token: 0x060017B7 RID: 6071 RVA: 0x00026768 File Offset: 0x00024968
		public static T FindFirstObjectByType<T>() where T : Object
		{
			return (T)((object)Object.FindFirstObjectByType(typeof(T), FindObjectsInactive.Exclude));
		}

		// Token: 0x060017B8 RID: 6072 RVA: 0x00026790 File Offset: 0x00024990
		public static T FindAnyObjectByType<T>() where T : Object
		{
			return (T)((object)Object.FindAnyObjectByType(typeof(T), FindObjectsInactive.Exclude));
		}

		// Token: 0x060017B9 RID: 6073 RVA: 0x000267B8 File Offset: 0x000249B8
		public static T FindFirstObjectByType<T>(FindObjectsInactive findObjectsInactive) where T : Object
		{
			return (T)((object)Object.FindFirstObjectByType(typeof(T), findObjectsInactive));
		}

		// Token: 0x060017BA RID: 6074 RVA: 0x000267E0 File Offset: 0x000249E0
		public static T FindAnyObjectByType<T>(FindObjectsInactive findObjectsInactive) where T : Object
		{
			return (T)((object)Object.FindAnyObjectByType(typeof(T), findObjectsInactive));
		}

		// Token: 0x060017BB RID: 6075 RVA: 0x00026808 File Offset: 0x00024A08
		[Obsolete("Please use Resources.FindObjectsOfTypeAll instead")]
		public static Object[] FindObjectsOfTypeAll(Type type)
		{
			return Resources.FindObjectsOfTypeAll(type);
		}

		// Token: 0x060017BC RID: 6076 RVA: 0x00026820 File Offset: 0x00024A20
		private static void CheckNullArgument(object arg, string message)
		{
			bool flag = arg == null;
			if (flag)
			{
				throw new ArgumentException(message);
			}
		}

		// Token: 0x060017BD RID: 6077 RVA: 0x00026840 File Offset: 0x00024A40
		[TypeInferenceRule(TypeInferenceRules.TypeReferencedByFirstArgument)]
		public static Object FindObjectOfType(Type type)
		{
			Object[] array = Object.FindObjectsOfType(type, false);
			bool flag = array.Length != 0;
			Object @object;
			if (flag)
			{
				@object = array[0];
			}
			else
			{
				@object = null;
			}
			return @object;
		}

		// Token: 0x060017BE RID: 6078 RVA: 0x0002686C File Offset: 0x00024A6C
		public static Object FindFirstObjectByType(Type type)
		{
			Object[] array = Object.FindObjectsByType(type, FindObjectsInactive.Exclude, FindObjectsSortMode.InstanceID);
			return (array.Length != 0) ? array[0] : null;
		}

		// Token: 0x060017BF RID: 6079 RVA: 0x00026894 File Offset: 0x00024A94
		public static Object FindAnyObjectByType(Type type)
		{
			Object[] array = Object.FindObjectsByType(type, FindObjectsInactive.Exclude, FindObjectsSortMode.None);
			return (array.Length != 0) ? array[0] : null;
		}

		// Token: 0x060017C0 RID: 6080 RVA: 0x000268BC File Offset: 0x00024ABC
		[TypeInferenceRule(TypeInferenceRules.TypeReferencedByFirstArgument)]
		public static Object FindObjectOfType(Type type, bool includeInactive)
		{
			Object[] array = Object.FindObjectsOfType(type, includeInactive);
			bool flag = array.Length != 0;
			Object @object;
			if (flag)
			{
				@object = array[0];
			}
			else
			{
				@object = null;
			}
			return @object;
		}

		// Token: 0x060017C1 RID: 6081 RVA: 0x000268E8 File Offset: 0x00024AE8
		public static Object FindFirstObjectByType(Type type, FindObjectsInactive findObjectsInactive)
		{
			Object[] array = Object.FindObjectsByType(type, findObjectsInactive, FindObjectsSortMode.InstanceID);
			return (array.Length != 0) ? array[0] : null;
		}

		// Token: 0x060017C2 RID: 6082 RVA: 0x00026910 File Offset: 0x00024B10
		public static Object FindAnyObjectByType(Type type, FindObjectsInactive findObjectsInactive)
		{
			Object[] array = Object.FindObjectsByType(type, findObjectsInactive, FindObjectsSortMode.None);
			return (array.Length != 0) ? array[0] : null;
		}

		// Token: 0x060017C3 RID: 6083 RVA: 0x00026938 File Offset: 0x00024B38
		public override string ToString()
		{
			return Object.ToString(this);
		}

		// Token: 0x060017C4 RID: 6084 RVA: 0x00026950 File Offset: 0x00024B50
		public static bool operator ==(Object x, Object y)
		{
			return Object.CompareBaseObjects(x, y);
		}

		// Token: 0x060017C5 RID: 6085 RVA: 0x0002696C File Offset: 0x00024B6C
		public static bool operator !=(Object x, Object y)
		{
			return !Object.CompareBaseObjects(x, y);
		}

		// Token: 0x060017C6 RID: 6086
		[NativeMethod(Name = "Object::GetOffsetOfInstanceIdMember", IsFreeFunction = true, IsThreadSafe = true)]
		[MethodImpl(4096)]
		private static extern int GetOffsetOfInstanceIDInCPlusPlusObject();

		// Token: 0x060017C7 RID: 6087
		[NativeMethod(Name = "CurrentThreadIsMainThread", IsFreeFunction = true, IsThreadSafe = true)]
		[MethodImpl(4096)]
		private static extern bool CurrentThreadIsMainThread();

		// Token: 0x060017C8 RID: 6088
		[NativeMethod(Name = "CloneObject", IsFreeFunction = true, ThrowsException = true)]
		[MethodImpl(4096)]
		private static extern Object Internal_CloneSingle([NotNull("NullExceptionObject")] Object data);

		// Token: 0x060017C9 RID: 6089
		[FreeFunction("CloneObject")]
		[MethodImpl(4096)]
		private static extern Object Internal_CloneSingleWithParent([NotNull("NullExceptionObject")] Object data, [NotNull("NullExceptionObject")] Transform parent, bool worldPositionStays);

		// Token: 0x060017CA RID: 6090 RVA: 0x00026988 File Offset: 0x00024B88
		[FreeFunction("InstantiateObject")]
		private static Object Internal_InstantiateSingle([NotNull("NullExceptionObject")] Object data, Vector3 pos, Quaternion rot)
		{
			return Object.Internal_InstantiateSingle_Injected(data, ref pos, ref rot);
		}

		// Token: 0x060017CB RID: 6091 RVA: 0x00026994 File Offset: 0x00024B94
		[FreeFunction("InstantiateObject")]
		private static Object Internal_InstantiateSingleWithParent([NotNull("NullExceptionObject")] Object data, [NotNull("NullExceptionObject")] Transform parent, Vector3 pos, Quaternion rot)
		{
			return Object.Internal_InstantiateSingleWithParent_Injected(data, parent, ref pos, ref rot);
		}

		// Token: 0x060017CC RID: 6092
		[FreeFunction("UnityEngineObjectBindings::ToString")]
		[MethodImpl(4096)]
		private static extern string ToString(Object obj);

		// Token: 0x060017CD RID: 6093
		[FreeFunction("UnityEngineObjectBindings::GetName")]
		[MethodImpl(4096)]
		private static extern string GetName([NotNull("NullExceptionObject")] Object obj);

		// Token: 0x060017CE RID: 6094
		[FreeFunction("UnityEngineObjectBindings::IsPersistent")]
		[MethodImpl(4096)]
		internal static extern bool IsPersistent([NotNull("NullExceptionObject")] Object obj);

		// Token: 0x060017CF RID: 6095
		[FreeFunction("UnityEngineObjectBindings::SetName")]
		[MethodImpl(4096)]
		private static extern void SetName([NotNull("NullExceptionObject")] Object obj, string name);

		// Token: 0x060017D0 RID: 6096
		[NativeMethod(Name = "UnityEngineObjectBindings::DoesObjectWithInstanceIDExist", IsFreeFunction = true, IsThreadSafe = true)]
		[MethodImpl(4096)]
		internal static extern bool DoesObjectWithInstanceIDExist(int instanceID);

		// Token: 0x060017D1 RID: 6097
		[FreeFunction("UnityEngineObjectBindings::FindObjectFromInstanceID")]
		[VisibleToOtherModules]
		[MethodImpl(4096)]
		internal static extern Object FindObjectFromInstanceID(int instanceID);

		// Token: 0x060017D2 RID: 6098
		[FreeFunction("UnityEngineObjectBindings::ForceLoadFromInstanceID")]
		[VisibleToOtherModules]
		[MethodImpl(4096)]
		internal static extern Object ForceLoadFromInstanceID(int instanceID);

		// Token: 0x060017D5 RID: 6101
		[MethodImpl(4096)]
		private static extern Object Internal_InstantiateSingle_Injected(Object data, ref Vector3 pos, ref Quaternion rot);

		// Token: 0x060017D6 RID: 6102
		[MethodImpl(4096)]
		private static extern Object Internal_InstantiateSingleWithParent_Injected(Object data, Transform parent, ref Vector3 pos, ref Quaternion rot);

		// Token: 0x04000825 RID: 2085
		private IntPtr m_CachedPtr;

		// Token: 0x04000826 RID: 2086
		internal static int OffsetOfInstanceIDInCPlusPlusObject = -1;

		// Token: 0x04000827 RID: 2087
		private const string objectIsNullMessage = "The Object you want to instantiate is null.";

		// Token: 0x04000828 RID: 2088
		private const string cloneDestroyedMessage = "Instantiate failed because the clone was destroyed during creation. This can happen if DestroyImmediate is called in MonoBehaviour.Awake.";
	}
}
