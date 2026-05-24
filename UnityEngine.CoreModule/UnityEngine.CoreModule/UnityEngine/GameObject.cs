using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Security;
using UnityEngine.Bindings;
using UnityEngine.Internal;
using UnityEngine.SceneManagement;
using UnityEngine.Scripting;
using UnityEngineInternal;

namespace UnityEngine
{
	// Token: 0x0200020A RID: 522
	[ExcludeFromPreset]
	[NativeHeader("Runtime/Export/Scripting/GameObject.bindings.h")]
	[UsedByNativeCode]
	public sealed class GameObject : Object
	{
		// Token: 0x060016BD RID: 5821
		[FreeFunction("GameObjectBindings::CreatePrimitive")]
		[MethodImpl(4096)]
		public static extern GameObject CreatePrimitive(PrimitiveType type);

		// Token: 0x060016BE RID: 5822 RVA: 0x00024B00 File Offset: 0x00022D00
		[SecuritySafeCritical]
		public unsafe T GetComponent<T>()
		{
			CastHelper<T> castHelper = default(CastHelper<T>);
			this.GetComponentFastPath(typeof(T), new IntPtr((void*)(&castHelper.onePointerFurtherThanT)));
			return castHelper.t;
		}

		// Token: 0x060016BF RID: 5823
		[TypeInferenceRule(TypeInferenceRules.TypeReferencedByFirstArgument)]
		[FreeFunction(Name = "GameObjectBindings::GetComponentFromType", HasExplicitThis = true, ThrowsException = true)]
		[MethodImpl(4096)]
		public extern Component GetComponent(Type type);

		// Token: 0x060016C0 RID: 5824
		[FreeFunction(Name = "GameObjectBindings::GetComponentFastPath", HasExplicitThis = true, ThrowsException = true)]
		[NativeWritableSelf]
		[MethodImpl(4096)]
		internal extern void GetComponentFastPath(Type type, IntPtr oneFurtherThanResultValue);

		// Token: 0x060016C1 RID: 5825
		[FreeFunction(Name = "Scripting::GetScriptingWrapperOfComponentOfGameObject", HasExplicitThis = true)]
		[MethodImpl(4096)]
		internal extern Component GetComponentByName(string type);

		// Token: 0x060016C2 RID: 5826 RVA: 0x00024B40 File Offset: 0x00022D40
		public Component GetComponent(string type)
		{
			return this.GetComponentByName(type);
		}

		// Token: 0x060016C3 RID: 5827
		[FreeFunction(Name = "GameObjectBindings::GetComponentInChildren", HasExplicitThis = true, ThrowsException = true)]
		[TypeInferenceRule(TypeInferenceRules.TypeReferencedByFirstArgument)]
		[MethodImpl(4096)]
		public extern Component GetComponentInChildren(Type type, bool includeInactive);

		// Token: 0x060016C4 RID: 5828 RVA: 0x00024B5C File Offset: 0x00022D5C
		[TypeInferenceRule(TypeInferenceRules.TypeReferencedByFirstArgument)]
		public Component GetComponentInChildren(Type type)
		{
			return this.GetComponentInChildren(type, false);
		}

		// Token: 0x060016C5 RID: 5829 RVA: 0x00024B78 File Offset: 0x00022D78
		[ExcludeFromDocs]
		public T GetComponentInChildren<T>()
		{
			bool flag = false;
			return this.GetComponentInChildren<T>(flag);
		}

		// Token: 0x060016C6 RID: 5830 RVA: 0x00024B94 File Offset: 0x00022D94
		public T GetComponentInChildren<T>([DefaultValue("false")] bool includeInactive)
		{
			return (T)((object)this.GetComponentInChildren(typeof(T), includeInactive));
		}

		// Token: 0x060016C7 RID: 5831
		[FreeFunction(Name = "GameObjectBindings::GetComponentInParent", HasExplicitThis = true, ThrowsException = true)]
		[TypeInferenceRule(TypeInferenceRules.TypeReferencedByFirstArgument)]
		[MethodImpl(4096)]
		public extern Component GetComponentInParent(Type type, bool includeInactive);

		// Token: 0x060016C8 RID: 5832 RVA: 0x00024BBC File Offset: 0x00022DBC
		[TypeInferenceRule(TypeInferenceRules.TypeReferencedByFirstArgument)]
		public Component GetComponentInParent(Type type)
		{
			return this.GetComponentInParent(type, false);
		}

		// Token: 0x060016C9 RID: 5833 RVA: 0x00024BD8 File Offset: 0x00022DD8
		[ExcludeFromDocs]
		public T GetComponentInParent<T>()
		{
			bool flag = false;
			return this.GetComponentInParent<T>(flag);
		}

		// Token: 0x060016CA RID: 5834 RVA: 0x00024BF4 File Offset: 0x00022DF4
		public T GetComponentInParent<T>([DefaultValue("false")] bool includeInactive)
		{
			return (T)((object)this.GetComponentInParent(typeof(T), includeInactive));
		}

		// Token: 0x060016CB RID: 5835
		[FreeFunction(Name = "GameObjectBindings::GetComponentsInternal", HasExplicitThis = true, ThrowsException = true)]
		[MethodImpl(4096)]
		private extern Array GetComponentsInternal(Type type, bool useSearchTypeAsArrayReturnType, bool recursive, bool includeInactive, bool reverse, object resultList);

		// Token: 0x060016CC RID: 5836 RVA: 0x00024C1C File Offset: 0x00022E1C
		public Component[] GetComponents(Type type)
		{
			return (Component[])this.GetComponentsInternal(type, false, false, true, false, null);
		}

		// Token: 0x060016CD RID: 5837 RVA: 0x00024C40 File Offset: 0x00022E40
		public T[] GetComponents<T>()
		{
			return (T[])this.GetComponentsInternal(typeof(T), true, false, true, false, null);
		}

		// Token: 0x060016CE RID: 5838 RVA: 0x00024C6C File Offset: 0x00022E6C
		public void GetComponents(Type type, List<Component> results)
		{
			this.GetComponentsInternal(type, false, false, true, false, results);
		}

		// Token: 0x060016CF RID: 5839 RVA: 0x00024C7C File Offset: 0x00022E7C
		public void GetComponents<T>(List<T> results)
		{
			this.GetComponentsInternal(typeof(T), true, false, true, false, results);
		}

		// Token: 0x060016D0 RID: 5840 RVA: 0x00024C98 File Offset: 0x00022E98
		[ExcludeFromDocs]
		public Component[] GetComponentsInChildren(Type type)
		{
			bool flag = false;
			return this.GetComponentsInChildren(type, flag);
		}

		// Token: 0x060016D1 RID: 5841 RVA: 0x00024CB4 File Offset: 0x00022EB4
		public Component[] GetComponentsInChildren(Type type, [DefaultValue("false")] bool includeInactive)
		{
			return (Component[])this.GetComponentsInternal(type, false, true, includeInactive, false, null);
		}

		// Token: 0x060016D2 RID: 5842 RVA: 0x00024CD8 File Offset: 0x00022ED8
		public T[] GetComponentsInChildren<T>(bool includeInactive)
		{
			return (T[])this.GetComponentsInternal(typeof(T), true, true, includeInactive, false, null);
		}

		// Token: 0x060016D3 RID: 5843 RVA: 0x00024D04 File Offset: 0x00022F04
		public void GetComponentsInChildren<T>(bool includeInactive, List<T> results)
		{
			this.GetComponentsInternal(typeof(T), true, true, includeInactive, false, results);
		}

		// Token: 0x060016D4 RID: 5844 RVA: 0x00024D20 File Offset: 0x00022F20
		public T[] GetComponentsInChildren<T>()
		{
			return this.GetComponentsInChildren<T>(false);
		}

		// Token: 0x060016D5 RID: 5845 RVA: 0x00024D39 File Offset: 0x00022F39
		public void GetComponentsInChildren<T>(List<T> results)
		{
			this.GetComponentsInChildren<T>(false, results);
		}

		// Token: 0x060016D6 RID: 5846 RVA: 0x00024D48 File Offset: 0x00022F48
		[ExcludeFromDocs]
		public Component[] GetComponentsInParent(Type type)
		{
			bool flag = false;
			return this.GetComponentsInParent(type, flag);
		}

		// Token: 0x060016D7 RID: 5847 RVA: 0x00024D64 File Offset: 0x00022F64
		public Component[] GetComponentsInParent(Type type, [DefaultValue("false")] bool includeInactive)
		{
			return (Component[])this.GetComponentsInternal(type, false, true, includeInactive, true, null);
		}

		// Token: 0x060016D8 RID: 5848 RVA: 0x00024D87 File Offset: 0x00022F87
		public void GetComponentsInParent<T>(bool includeInactive, List<T> results)
		{
			this.GetComponentsInternal(typeof(T), true, true, includeInactive, true, results);
		}

		// Token: 0x060016D9 RID: 5849 RVA: 0x00024DA0 File Offset: 0x00022FA0
		public T[] GetComponentsInParent<T>(bool includeInactive)
		{
			return (T[])this.GetComponentsInternal(typeof(T), true, true, includeInactive, true, null);
		}

		// Token: 0x060016DA RID: 5850 RVA: 0x00024DCC File Offset: 0x00022FCC
		public T[] GetComponentsInParent<T>()
		{
			return this.GetComponentsInParent<T>(false);
		}

		// Token: 0x060016DB RID: 5851 RVA: 0x00024DE8 File Offset: 0x00022FE8
		[SecuritySafeCritical]
		public unsafe bool TryGetComponent<T>(out T component)
		{
			CastHelper<T> castHelper = default(CastHelper<T>);
			this.TryGetComponentFastPath(typeof(T), new IntPtr((void*)(&castHelper.onePointerFurtherThanT)));
			component = castHelper.t;
			return castHelper.t != null;
		}

		// Token: 0x060016DC RID: 5852 RVA: 0x00024E3C File Offset: 0x0002303C
		public bool TryGetComponent(Type type, out Component component)
		{
			component = this.TryGetComponentInternal(type);
			return component != null;
		}

		// Token: 0x060016DD RID: 5853
		[TypeInferenceRule(TypeInferenceRules.TypeReferencedByFirstArgument)]
		[FreeFunction(Name = "GameObjectBindings::TryGetComponentFromType", HasExplicitThis = true, ThrowsException = true)]
		[MethodImpl(4096)]
		internal extern Component TryGetComponentInternal(Type type);

		// Token: 0x060016DE RID: 5854
		[FreeFunction(Name = "GameObjectBindings::TryGetComponentFastPath", HasExplicitThis = true, ThrowsException = true)]
		[NativeWritableSelf]
		[MethodImpl(4096)]
		internal extern void TryGetComponentFastPath(Type type, IntPtr oneFurtherThanResultValue);

		// Token: 0x060016DF RID: 5855 RVA: 0x00024E60 File Offset: 0x00023060
		public static GameObject FindWithTag(string tag)
		{
			return GameObject.FindGameObjectWithTag(tag);
		}

		// Token: 0x060016E0 RID: 5856 RVA: 0x00024E78 File Offset: 0x00023078
		public void SendMessageUpwards(string methodName, SendMessageOptions options)
		{
			this.SendMessageUpwards(methodName, null, options);
		}

		// Token: 0x060016E1 RID: 5857 RVA: 0x00024E85 File Offset: 0x00023085
		public void SendMessage(string methodName, SendMessageOptions options)
		{
			this.SendMessage(methodName, null, options);
		}

		// Token: 0x060016E2 RID: 5858 RVA: 0x00024E92 File Offset: 0x00023092
		public void BroadcastMessage(string methodName, SendMessageOptions options)
		{
			this.BroadcastMessage(methodName, null, options);
		}

		// Token: 0x060016E3 RID: 5859
		[FreeFunction(Name = "MonoAddComponent", HasExplicitThis = true)]
		[MethodImpl(4096)]
		internal extern Component AddComponentInternal(string className);

		// Token: 0x060016E4 RID: 5860
		[FreeFunction(Name = "MonoAddComponentWithType", HasExplicitThis = true)]
		[MethodImpl(4096)]
		private extern Component Internal_AddComponentWithType(Type componentType);

		// Token: 0x060016E5 RID: 5861 RVA: 0x00024EA0 File Offset: 0x000230A0
		[TypeInferenceRule(TypeInferenceRules.TypeReferencedByFirstArgument)]
		public Component AddComponent(Type componentType)
		{
			return this.Internal_AddComponentWithType(componentType);
		}

		// Token: 0x060016E6 RID: 5862 RVA: 0x00024EBC File Offset: 0x000230BC
		public T AddComponent<T>() where T : Component
		{
			return this.AddComponent(typeof(T)) as T;
		}

		// Token: 0x17000473 RID: 1139
		// (get) Token: 0x060016E7 RID: 5863
		public extern Transform transform
		{
			[FreeFunction("GameObjectBindings::GetTransform", HasExplicitThis = true)]
			[MethodImpl(4096)]
			get;
		}

		// Token: 0x17000474 RID: 1140
		// (get) Token: 0x060016E8 RID: 5864
		// (set) Token: 0x060016E9 RID: 5865
		public extern int layer
		{
			[MethodImpl(4096)]
			get;
			[MethodImpl(4096)]
			set;
		}

		// Token: 0x17000475 RID: 1141
		// (get) Token: 0x060016EA RID: 5866
		// (set) Token: 0x060016EB RID: 5867
		[Obsolete("GameObject.active is obsolete. Use GameObject.SetActive(), GameObject.activeSelf or GameObject.activeInHierarchy.")]
		public extern bool active
		{
			[NativeMethod(Name = "IsActive")]
			[MethodImpl(4096)]
			get;
			[NativeMethod(Name = "SetSelfActive")]
			[MethodImpl(4096)]
			set;
		}

		// Token: 0x060016EC RID: 5868
		[NativeMethod(Name = "SetSelfActive")]
		[MethodImpl(4096)]
		public extern void SetActive(bool value);

		// Token: 0x17000476 RID: 1142
		// (get) Token: 0x060016ED RID: 5869
		public extern bool activeSelf
		{
			[NativeMethod(Name = "IsSelfActive")]
			[MethodImpl(4096)]
			get;
		}

		// Token: 0x17000477 RID: 1143
		// (get) Token: 0x060016EE RID: 5870
		public extern bool activeInHierarchy
		{
			[NativeMethod(Name = "IsActive")]
			[MethodImpl(4096)]
			get;
		}

		// Token: 0x060016EF RID: 5871
		[NativeMethod(Name = "SetActiveRecursivelyDeprecated")]
		[Obsolete("gameObject.SetActiveRecursively() is obsolete. Use GameObject.SetActive(), which is now inherited by children.")]
		[MethodImpl(4096)]
		public extern void SetActiveRecursively(bool state);

		// Token: 0x17000478 RID: 1144
		// (get) Token: 0x060016F0 RID: 5872
		// (set) Token: 0x060016F1 RID: 5873
		public extern bool isStatic
		{
			[NativeMethod(Name = "GetIsStaticDeprecated")]
			[MethodImpl(4096)]
			get;
			[NativeMethod(Name = "SetIsStaticDeprecated")]
			[MethodImpl(4096)]
			set;
		}

		// Token: 0x17000479 RID: 1145
		// (get) Token: 0x060016F2 RID: 5874
		internal extern bool isStaticBatchable
		{
			[NativeMethod(Name = "IsStaticBatchable")]
			[MethodImpl(4096)]
			get;
		}

		// Token: 0x1700047A RID: 1146
		// (get) Token: 0x060016F3 RID: 5875
		// (set) Token: 0x060016F4 RID: 5876
		public extern string tag
		{
			[FreeFunction("GameObjectBindings::GetTag", HasExplicitThis = true)]
			[MethodImpl(4096)]
			get;
			[FreeFunction("GameObjectBindings::SetTag", HasExplicitThis = true)]
			[MethodImpl(4096)]
			set;
		}

		// Token: 0x060016F5 RID: 5877
		[FreeFunction(Name = "GameObjectBindings::CompareTag", HasExplicitThis = true)]
		[MethodImpl(4096)]
		public extern bool CompareTag(string tag);

		// Token: 0x060016F6 RID: 5878
		[FreeFunction(Name = "GameObjectBindings::FindGameObjectWithTag", ThrowsException = true)]
		[MethodImpl(4096)]
		public static extern GameObject FindGameObjectWithTag(string tag);

		// Token: 0x060016F7 RID: 5879
		[FreeFunction(Name = "GameObjectBindings::FindGameObjectsWithTag", ThrowsException = true)]
		[MethodImpl(4096)]
		public static extern GameObject[] FindGameObjectsWithTag(string tag);

		// Token: 0x060016F8 RID: 5880
		[FreeFunction(Name = "Scripting::SendScriptingMessageUpwards", HasExplicitThis = true)]
		[MethodImpl(4096)]
		public extern void SendMessageUpwards(string methodName, [DefaultValue("null")] object value, [DefaultValue("SendMessageOptions.RequireReceiver")] SendMessageOptions options);

		// Token: 0x060016F9 RID: 5881 RVA: 0x00024EE8 File Offset: 0x000230E8
		[ExcludeFromDocs]
		public void SendMessageUpwards(string methodName, object value)
		{
			SendMessageOptions sendMessageOptions = SendMessageOptions.RequireReceiver;
			this.SendMessageUpwards(methodName, value, sendMessageOptions);
		}

		// Token: 0x060016FA RID: 5882 RVA: 0x00024F04 File Offset: 0x00023104
		[ExcludeFromDocs]
		public void SendMessageUpwards(string methodName)
		{
			SendMessageOptions sendMessageOptions = SendMessageOptions.RequireReceiver;
			object obj = null;
			this.SendMessageUpwards(methodName, obj, sendMessageOptions);
		}

		// Token: 0x060016FB RID: 5883
		[FreeFunction(Name = "Scripting::SendScriptingMessage", HasExplicitThis = true)]
		[MethodImpl(4096)]
		public extern void SendMessage(string methodName, [DefaultValue("null")] object value, [DefaultValue("SendMessageOptions.RequireReceiver")] SendMessageOptions options);

		// Token: 0x060016FC RID: 5884 RVA: 0x00024F20 File Offset: 0x00023120
		[ExcludeFromDocs]
		public void SendMessage(string methodName, object value)
		{
			SendMessageOptions sendMessageOptions = SendMessageOptions.RequireReceiver;
			this.SendMessage(methodName, value, sendMessageOptions);
		}

		// Token: 0x060016FD RID: 5885 RVA: 0x00024F3C File Offset: 0x0002313C
		[ExcludeFromDocs]
		public void SendMessage(string methodName)
		{
			SendMessageOptions sendMessageOptions = SendMessageOptions.RequireReceiver;
			object obj = null;
			this.SendMessage(methodName, obj, sendMessageOptions);
		}

		// Token: 0x060016FE RID: 5886
		[FreeFunction(Name = "Scripting::BroadcastScriptingMessage", HasExplicitThis = true)]
		[MethodImpl(4096)]
		public extern void BroadcastMessage(string methodName, [DefaultValue("null")] object parameter, [DefaultValue("SendMessageOptions.RequireReceiver")] SendMessageOptions options);

		// Token: 0x060016FF RID: 5887 RVA: 0x00024F58 File Offset: 0x00023158
		[ExcludeFromDocs]
		public void BroadcastMessage(string methodName, object parameter)
		{
			SendMessageOptions sendMessageOptions = SendMessageOptions.RequireReceiver;
			this.BroadcastMessage(methodName, parameter, sendMessageOptions);
		}

		// Token: 0x06001700 RID: 5888 RVA: 0x00024F74 File Offset: 0x00023174
		[ExcludeFromDocs]
		public void BroadcastMessage(string methodName)
		{
			SendMessageOptions sendMessageOptions = SendMessageOptions.RequireReceiver;
			object obj = null;
			this.BroadcastMessage(methodName, obj, sendMessageOptions);
		}

		// Token: 0x06001701 RID: 5889 RVA: 0x00024F90 File Offset: 0x00023190
		public GameObject(string name)
		{
			GameObject.Internal_CreateGameObject(this, name);
		}

		// Token: 0x06001702 RID: 5890 RVA: 0x00024FA2 File Offset: 0x000231A2
		public GameObject()
		{
			GameObject.Internal_CreateGameObject(this, null);
		}

		// Token: 0x06001703 RID: 5891 RVA: 0x00024FB4 File Offset: 0x000231B4
		public GameObject(string name, params Type[] components)
		{
			GameObject.Internal_CreateGameObject(this, name);
			foreach (Type type in components)
			{
				this.AddComponent(type);
			}
		}

		// Token: 0x06001704 RID: 5892
		[FreeFunction(Name = "GameObjectBindings::Internal_CreateGameObject")]
		[MethodImpl(4096)]
		private static extern void Internal_CreateGameObject([Writable] GameObject self, string name);

		// Token: 0x06001705 RID: 5893
		[FreeFunction(Name = "GameObjectBindings::Find")]
		[MethodImpl(4096)]
		public static extern GameObject Find(string name);

		// Token: 0x1700047B RID: 1147
		// (get) Token: 0x06001706 RID: 5894 RVA: 0x00024FF0 File Offset: 0x000231F0
		public Scene scene
		{
			[FreeFunction("GameObjectBindings::GetScene", HasExplicitThis = true)]
			get
			{
				Scene scene;
				this.get_scene_Injected(out scene);
				return scene;
			}
		}

		// Token: 0x1700047C RID: 1148
		// (get) Token: 0x06001707 RID: 5895
		public extern ulong sceneCullingMask
		{
			[FreeFunction(Name = "GameObjectBindings::GetSceneCullingMask", HasExplicitThis = true)]
			[MethodImpl(4096)]
			get;
		}

		// Token: 0x1700047D RID: 1149
		// (get) Token: 0x06001708 RID: 5896 RVA: 0x00025008 File Offset: 0x00023208
		public GameObject gameObject
		{
			get
			{
				return this;
			}
		}

		// Token: 0x06001709 RID: 5897
		[MethodImpl(4096)]
		private extern void get_scene_Injected(out Scene ret);
	}
}
