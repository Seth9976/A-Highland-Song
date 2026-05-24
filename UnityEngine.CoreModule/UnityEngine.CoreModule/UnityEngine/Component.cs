using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Security;
using UnityEngine.Bindings;
using UnityEngine.Internal;
using UnityEngine.Scripting;
using UnityEngineInternal;

namespace UnityEngine
{
	// Token: 0x020001FD RID: 509
	[NativeClass("Unity::Component")]
	[RequiredByNativeCode]
	[NativeHeader("Runtime/Export/Scripting/Component.bindings.h")]
	public class Component : Object
	{
		// Token: 0x1700046E RID: 1134
		// (get) Token: 0x0600166B RID: 5739
		public extern Transform transform
		{
			[FreeFunction("GetTransform", HasExplicitThis = true, ThrowsException = true)]
			[MethodImpl(4096)]
			get;
		}

		// Token: 0x1700046F RID: 1135
		// (get) Token: 0x0600166C RID: 5740
		public extern GameObject gameObject
		{
			[FreeFunction("GetGameObject", HasExplicitThis = true)]
			[MethodImpl(4096)]
			get;
		}

		// Token: 0x0600166D RID: 5741 RVA: 0x00023E00 File Offset: 0x00022000
		[TypeInferenceRule(TypeInferenceRules.TypeReferencedByFirstArgument)]
		public Component GetComponent(Type type)
		{
			return this.gameObject.GetComponent(type);
		}

		// Token: 0x0600166E RID: 5742
		[FreeFunction(HasExplicitThis = true, ThrowsException = true)]
		[MethodImpl(4096)]
		internal extern void GetComponentFastPath(Type type, IntPtr oneFurtherThanResultValue);

		// Token: 0x0600166F RID: 5743 RVA: 0x00023E20 File Offset: 0x00022020
		[SecuritySafeCritical]
		public unsafe T GetComponent<T>()
		{
			CastHelper<T> castHelper = default(CastHelper<T>);
			this.GetComponentFastPath(typeof(T), new IntPtr((void*)(&castHelper.onePointerFurtherThanT)));
			return castHelper.t;
		}

		// Token: 0x06001670 RID: 5744 RVA: 0x00023E60 File Offset: 0x00022060
		[TypeInferenceRule(TypeInferenceRules.TypeReferencedByFirstArgument)]
		public bool TryGetComponent(Type type, out Component component)
		{
			return this.gameObject.TryGetComponent(type, out component);
		}

		// Token: 0x06001671 RID: 5745 RVA: 0x00023E80 File Offset: 0x00022080
		[SecuritySafeCritical]
		public bool TryGetComponent<T>(out T component)
		{
			return this.gameObject.TryGetComponent<T>(out component);
		}

		// Token: 0x06001672 RID: 5746
		[FreeFunction(HasExplicitThis = true)]
		[MethodImpl(4096)]
		public extern Component GetComponent(string type);

		// Token: 0x06001673 RID: 5747 RVA: 0x00023EA0 File Offset: 0x000220A0
		[TypeInferenceRule(TypeInferenceRules.TypeReferencedByFirstArgument)]
		public Component GetComponentInChildren(Type t, bool includeInactive)
		{
			return this.gameObject.GetComponentInChildren(t, includeInactive);
		}

		// Token: 0x06001674 RID: 5748 RVA: 0x00023EC0 File Offset: 0x000220C0
		[TypeInferenceRule(TypeInferenceRules.TypeReferencedByFirstArgument)]
		public Component GetComponentInChildren(Type t)
		{
			return this.GetComponentInChildren(t, false);
		}

		// Token: 0x06001675 RID: 5749 RVA: 0x00023EDC File Offset: 0x000220DC
		public T GetComponentInChildren<T>([DefaultValue("false")] bool includeInactive)
		{
			return (T)((object)this.GetComponentInChildren(typeof(T), includeInactive));
		}

		// Token: 0x06001676 RID: 5750 RVA: 0x00023F04 File Offset: 0x00022104
		[ExcludeFromDocs]
		public T GetComponentInChildren<T>()
		{
			return (T)((object)this.GetComponentInChildren(typeof(T), false));
		}

		// Token: 0x06001677 RID: 5751 RVA: 0x00023F2C File Offset: 0x0002212C
		public Component[] GetComponentsInChildren(Type t, bool includeInactive)
		{
			return this.gameObject.GetComponentsInChildren(t, includeInactive);
		}

		// Token: 0x06001678 RID: 5752 RVA: 0x00023F4C File Offset: 0x0002214C
		[ExcludeFromDocs]
		public Component[] GetComponentsInChildren(Type t)
		{
			return this.gameObject.GetComponentsInChildren(t, false);
		}

		// Token: 0x06001679 RID: 5753 RVA: 0x00023F6C File Offset: 0x0002216C
		public T[] GetComponentsInChildren<T>(bool includeInactive)
		{
			return this.gameObject.GetComponentsInChildren<T>(includeInactive);
		}

		// Token: 0x0600167A RID: 5754 RVA: 0x00023F8A File Offset: 0x0002218A
		public void GetComponentsInChildren<T>(bool includeInactive, List<T> result)
		{
			this.gameObject.GetComponentsInChildren<T>(includeInactive, result);
		}

		// Token: 0x0600167B RID: 5755 RVA: 0x00023F9C File Offset: 0x0002219C
		public T[] GetComponentsInChildren<T>()
		{
			return this.GetComponentsInChildren<T>(false);
		}

		// Token: 0x0600167C RID: 5756 RVA: 0x00023FB5 File Offset: 0x000221B5
		public void GetComponentsInChildren<T>(List<T> results)
		{
			this.GetComponentsInChildren<T>(false, results);
		}

		// Token: 0x0600167D RID: 5757 RVA: 0x00023FC4 File Offset: 0x000221C4
		[TypeInferenceRule(TypeInferenceRules.TypeReferencedByFirstArgument)]
		public Component GetComponentInParent(Type t, bool includeInactive)
		{
			return this.gameObject.GetComponentInParent(t, includeInactive);
		}

		// Token: 0x0600167E RID: 5758 RVA: 0x00023FE4 File Offset: 0x000221E4
		[TypeInferenceRule(TypeInferenceRules.TypeReferencedByFirstArgument)]
		public Component GetComponentInParent(Type t)
		{
			return this.gameObject.GetComponentInParent(t, false);
		}

		// Token: 0x0600167F RID: 5759 RVA: 0x00024004 File Offset: 0x00022204
		public T GetComponentInParent<T>([DefaultValue("false")] bool includeInactive)
		{
			return (T)((object)this.GetComponentInParent(typeof(T), includeInactive));
		}

		// Token: 0x06001680 RID: 5760 RVA: 0x0002402C File Offset: 0x0002222C
		public T GetComponentInParent<T>()
		{
			return (T)((object)this.GetComponentInParent(typeof(T), false));
		}

		// Token: 0x06001681 RID: 5761 RVA: 0x00024054 File Offset: 0x00022254
		public Component[] GetComponentsInParent(Type t, [DefaultValue("false")] bool includeInactive)
		{
			return this.gameObject.GetComponentsInParent(t, includeInactive);
		}

		// Token: 0x06001682 RID: 5762 RVA: 0x00024074 File Offset: 0x00022274
		[ExcludeFromDocs]
		public Component[] GetComponentsInParent(Type t)
		{
			return this.GetComponentsInParent(t, false);
		}

		// Token: 0x06001683 RID: 5763 RVA: 0x00024090 File Offset: 0x00022290
		public T[] GetComponentsInParent<T>(bool includeInactive)
		{
			return this.gameObject.GetComponentsInParent<T>(includeInactive);
		}

		// Token: 0x06001684 RID: 5764 RVA: 0x000240AE File Offset: 0x000222AE
		public void GetComponentsInParent<T>(bool includeInactive, List<T> results)
		{
			this.gameObject.GetComponentsInParent<T>(includeInactive, results);
		}

		// Token: 0x06001685 RID: 5765 RVA: 0x000240C0 File Offset: 0x000222C0
		public T[] GetComponentsInParent<T>()
		{
			return this.GetComponentsInParent<T>(false);
		}

		// Token: 0x06001686 RID: 5766 RVA: 0x000240DC File Offset: 0x000222DC
		public Component[] GetComponents(Type type)
		{
			return this.gameObject.GetComponents(type);
		}

		// Token: 0x06001687 RID: 5767
		[FreeFunction(HasExplicitThis = true, ThrowsException = true)]
		[MethodImpl(4096)]
		private extern void GetComponentsForListInternal(Type searchType, object resultList);

		// Token: 0x06001688 RID: 5768 RVA: 0x000240FA File Offset: 0x000222FA
		public void GetComponents(Type type, List<Component> results)
		{
			this.GetComponentsForListInternal(type, results);
		}

		// Token: 0x06001689 RID: 5769 RVA: 0x00024106 File Offset: 0x00022306
		public void GetComponents<T>(List<T> results)
		{
			this.GetComponentsForListInternal(typeof(T), results);
		}

		// Token: 0x17000470 RID: 1136
		// (get) Token: 0x0600168A RID: 5770 RVA: 0x0002411B File Offset: 0x0002231B
		// (set) Token: 0x0600168B RID: 5771 RVA: 0x00024128 File Offset: 0x00022328
		public string tag
		{
			get
			{
				return this.gameObject.tag;
			}
			set
			{
				this.gameObject.tag = value;
			}
		}

		// Token: 0x0600168C RID: 5772 RVA: 0x00024138 File Offset: 0x00022338
		public T[] GetComponents<T>()
		{
			return this.gameObject.GetComponents<T>();
		}

		// Token: 0x0600168D RID: 5773 RVA: 0x00024158 File Offset: 0x00022358
		public bool CompareTag(string tag)
		{
			return this.gameObject.CompareTag(tag);
		}

		// Token: 0x0600168E RID: 5774
		[FreeFunction(HasExplicitThis = true)]
		[MethodImpl(4096)]
		public extern void SendMessageUpwards(string methodName, [DefaultValue("null")] object value, [DefaultValue("SendMessageOptions.RequireReceiver")] SendMessageOptions options);

		// Token: 0x0600168F RID: 5775 RVA: 0x00024176 File Offset: 0x00022376
		[ExcludeFromDocs]
		public void SendMessageUpwards(string methodName, object value)
		{
			this.SendMessageUpwards(methodName, value, SendMessageOptions.RequireReceiver);
		}

		// Token: 0x06001690 RID: 5776 RVA: 0x00024183 File Offset: 0x00022383
		[ExcludeFromDocs]
		public void SendMessageUpwards(string methodName)
		{
			this.SendMessageUpwards(methodName, null, SendMessageOptions.RequireReceiver);
		}

		// Token: 0x06001691 RID: 5777 RVA: 0x00024190 File Offset: 0x00022390
		public void SendMessageUpwards(string methodName, SendMessageOptions options)
		{
			this.SendMessageUpwards(methodName, null, options);
		}

		// Token: 0x06001692 RID: 5778 RVA: 0x0002419D File Offset: 0x0002239D
		public void SendMessage(string methodName, object value)
		{
			this.SendMessage(methodName, value, SendMessageOptions.RequireReceiver);
		}

		// Token: 0x06001693 RID: 5779 RVA: 0x000241AA File Offset: 0x000223AA
		public void SendMessage(string methodName)
		{
			this.SendMessage(methodName, null, SendMessageOptions.RequireReceiver);
		}

		// Token: 0x06001694 RID: 5780
		[FreeFunction("SendMessage", HasExplicitThis = true)]
		[MethodImpl(4096)]
		public extern void SendMessage(string methodName, object value, SendMessageOptions options);

		// Token: 0x06001695 RID: 5781 RVA: 0x000241B7 File Offset: 0x000223B7
		public void SendMessage(string methodName, SendMessageOptions options)
		{
			this.SendMessage(methodName, null, options);
		}

		// Token: 0x06001696 RID: 5782
		[FreeFunction("BroadcastMessage", HasExplicitThis = true)]
		[MethodImpl(4096)]
		public extern void BroadcastMessage(string methodName, [DefaultValue("null")] object parameter, [DefaultValue("SendMessageOptions.RequireReceiver")] SendMessageOptions options);

		// Token: 0x06001697 RID: 5783 RVA: 0x000241C4 File Offset: 0x000223C4
		[ExcludeFromDocs]
		public void BroadcastMessage(string methodName, object parameter)
		{
			this.BroadcastMessage(methodName, parameter, SendMessageOptions.RequireReceiver);
		}

		// Token: 0x06001698 RID: 5784 RVA: 0x000241D1 File Offset: 0x000223D1
		[ExcludeFromDocs]
		public void BroadcastMessage(string methodName)
		{
			this.BroadcastMessage(methodName, null, SendMessageOptions.RequireReceiver);
		}

		// Token: 0x06001699 RID: 5785 RVA: 0x000241DE File Offset: 0x000223DE
		public void BroadcastMessage(string methodName, SendMessageOptions options)
		{
			this.BroadcastMessage(methodName, null, options);
		}
	}
}
