using System;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;
using UnityEngine.Internal;
using UnityEngine.Scripting;

namespace UnityEngine
{
	// Token: 0x0200020E RID: 526
	[ExtensionOfNativeClass]
	[NativeHeader("Runtime/Scripting/DelayedCallUtility.h")]
	[RequiredByNativeCode]
	[NativeHeader("Runtime/Mono/MonoBehaviour.h")]
	public class MonoBehaviour : Behaviour
	{
		// Token: 0x0600171F RID: 5919 RVA: 0x00025388 File Offset: 0x00023588
		public bool IsInvoking()
		{
			return MonoBehaviour.Internal_IsInvokingAll(this);
		}

		// Token: 0x06001720 RID: 5920 RVA: 0x000253A0 File Offset: 0x000235A0
		public void CancelInvoke()
		{
			MonoBehaviour.Internal_CancelInvokeAll(this);
		}

		// Token: 0x06001721 RID: 5921 RVA: 0x000253AA File Offset: 0x000235AA
		public void Invoke(string methodName, float time)
		{
			MonoBehaviour.InvokeDelayed(this, methodName, time, 0f);
		}

		// Token: 0x06001722 RID: 5922 RVA: 0x000253BC File Offset: 0x000235BC
		public void InvokeRepeating(string methodName, float time, float repeatRate)
		{
			bool flag = repeatRate <= 1E-05f && repeatRate != 0f;
			if (flag)
			{
				throw new UnityException("Invoke repeat rate has to be larger than 0.00001F)");
			}
			MonoBehaviour.InvokeDelayed(this, methodName, time, repeatRate);
		}

		// Token: 0x06001723 RID: 5923 RVA: 0x000253F9 File Offset: 0x000235F9
		public void CancelInvoke(string methodName)
		{
			MonoBehaviour.CancelInvoke(this, methodName);
		}

		// Token: 0x06001724 RID: 5924 RVA: 0x00025404 File Offset: 0x00023604
		public bool IsInvoking(string methodName)
		{
			return MonoBehaviour.IsInvoking(this, methodName);
		}

		// Token: 0x06001725 RID: 5925 RVA: 0x00025420 File Offset: 0x00023620
		[ExcludeFromDocs]
		public Coroutine StartCoroutine(string methodName)
		{
			object obj = null;
			return this.StartCoroutine(methodName, obj);
		}

		// Token: 0x06001726 RID: 5926 RVA: 0x0002543C File Offset: 0x0002363C
		public Coroutine StartCoroutine(string methodName, [DefaultValue("null")] object value)
		{
			bool flag = string.IsNullOrEmpty(methodName);
			if (flag)
			{
				throw new NullReferenceException("methodName is null or empty");
			}
			bool flag2 = !MonoBehaviour.IsObjectMonoBehaviour(this);
			if (flag2)
			{
				throw new ArgumentException("Coroutines can only be stopped on a MonoBehaviour");
			}
			return this.StartCoroutineManaged(methodName, value);
		}

		// Token: 0x06001727 RID: 5927 RVA: 0x00025484 File Offset: 0x00023684
		public Coroutine StartCoroutine(IEnumerator routine)
		{
			bool flag = routine == null;
			if (flag)
			{
				throw new NullReferenceException("routine is null");
			}
			bool flag2 = !MonoBehaviour.IsObjectMonoBehaviour(this);
			if (flag2)
			{
				throw new ArgumentException("Coroutines can only be stopped on a MonoBehaviour");
			}
			return this.StartCoroutineManaged2(routine);
		}

		// Token: 0x06001728 RID: 5928 RVA: 0x000254C8 File Offset: 0x000236C8
		[Obsolete("StartCoroutine_Auto has been deprecated. Use StartCoroutine instead (UnityUpgradable) -> StartCoroutine([mscorlib] System.Collections.IEnumerator)", false)]
		public Coroutine StartCoroutine_Auto(IEnumerator routine)
		{
			return this.StartCoroutine(routine);
		}

		// Token: 0x06001729 RID: 5929 RVA: 0x000254E4 File Offset: 0x000236E4
		public void StopCoroutine(IEnumerator routine)
		{
			bool flag = routine == null;
			if (flag)
			{
				throw new NullReferenceException("routine is null");
			}
			bool flag2 = !MonoBehaviour.IsObjectMonoBehaviour(this);
			if (flag2)
			{
				throw new ArgumentException("Coroutines can only be stopped on a MonoBehaviour");
			}
			this.StopCoroutineFromEnumeratorManaged(routine);
		}

		// Token: 0x0600172A RID: 5930 RVA: 0x00025528 File Offset: 0x00023728
		public void StopCoroutine(Coroutine routine)
		{
			bool flag = routine == null;
			if (flag)
			{
				throw new NullReferenceException("routine is null");
			}
			bool flag2 = !MonoBehaviour.IsObjectMonoBehaviour(this);
			if (flag2)
			{
				throw new ArgumentException("Coroutines can only be stopped on a MonoBehaviour");
			}
			this.StopCoroutineManaged(routine);
		}

		// Token: 0x0600172B RID: 5931
		[MethodImpl(4096)]
		public extern void StopCoroutine(string methodName);

		// Token: 0x0600172C RID: 5932
		[MethodImpl(4096)]
		public extern void StopAllCoroutines();

		// Token: 0x17000483 RID: 1155
		// (get) Token: 0x0600172D RID: 5933
		// (set) Token: 0x0600172E RID: 5934
		public extern bool useGUILayout
		{
			[MethodImpl(4096)]
			get;
			[MethodImpl(4096)]
			set;
		}

		// Token: 0x0600172F RID: 5935 RVA: 0x00025569 File Offset: 0x00023769
		public static void print(object message)
		{
			Debug.Log(message);
		}

		// Token: 0x06001730 RID: 5936
		[FreeFunction("CancelInvoke")]
		[MethodImpl(4096)]
		private static extern void Internal_CancelInvokeAll([NotNull("NullExceptionObject")] MonoBehaviour self);

		// Token: 0x06001731 RID: 5937
		[FreeFunction("IsInvoking")]
		[MethodImpl(4096)]
		private static extern bool Internal_IsInvokingAll([NotNull("NullExceptionObject")] MonoBehaviour self);

		// Token: 0x06001732 RID: 5938
		[FreeFunction]
		[MethodImpl(4096)]
		private static extern void InvokeDelayed([NotNull("NullExceptionObject")] MonoBehaviour self, string methodName, float time, float repeatRate);

		// Token: 0x06001733 RID: 5939
		[FreeFunction]
		[MethodImpl(4096)]
		private static extern void CancelInvoke([NotNull("NullExceptionObject")] MonoBehaviour self, string methodName);

		// Token: 0x06001734 RID: 5940
		[FreeFunction]
		[MethodImpl(4096)]
		private static extern bool IsInvoking([NotNull("NullExceptionObject")] MonoBehaviour self, string methodName);

		// Token: 0x06001735 RID: 5941
		[FreeFunction]
		[MethodImpl(4096)]
		private static extern bool IsObjectMonoBehaviour([NotNull("NullExceptionObject")] Object obj);

		// Token: 0x06001736 RID: 5942
		[MethodImpl(4096)]
		private extern Coroutine StartCoroutineManaged(string methodName, object value);

		// Token: 0x06001737 RID: 5943
		[MethodImpl(4096)]
		private extern Coroutine StartCoroutineManaged2(IEnumerator enumerator);

		// Token: 0x06001738 RID: 5944
		[MethodImpl(4096)]
		private extern void StopCoroutineManaged(Coroutine routine);

		// Token: 0x06001739 RID: 5945
		[MethodImpl(4096)]
		private extern void StopCoroutineFromEnumeratorManaged(IEnumerator routine);

		// Token: 0x0600173A RID: 5946
		[MethodImpl(4096)]
		internal extern string GetScriptClassName();
	}
}
