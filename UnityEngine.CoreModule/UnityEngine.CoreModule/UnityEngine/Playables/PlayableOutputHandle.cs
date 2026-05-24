using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEngine.Playables
{
	// Token: 0x02000448 RID: 1096
	[NativeHeader("Runtime/Director/Core/HPlayableOutput.h")]
	[UsedByNativeCode]
	[NativeHeader("Runtime/Export/Director/PlayableOutputHandle.bindings.h")]
	[NativeHeader("Runtime/Director/Core/HPlayable.h")]
	public struct PlayableOutputHandle : IEquatable<PlayableOutputHandle>
	{
		// Token: 0x1700072D RID: 1837
		// (get) Token: 0x0600269D RID: 9885 RVA: 0x000407BC File Offset: 0x0003E9BC
		public static PlayableOutputHandle Null
		{
			get
			{
				return PlayableOutputHandle.m_Null;
			}
		}

		// Token: 0x0600269E RID: 9886 RVA: 0x000407D4 File Offset: 0x0003E9D4
		[VisibleToOtherModules]
		internal bool IsPlayableOutputOfType<T>()
		{
			return this.GetPlayableOutputType() == typeof(T);
		}

		// Token: 0x0600269F RID: 9887 RVA: 0x000407F8 File Offset: 0x0003E9F8
		public override int GetHashCode()
		{
			return this.m_Handle.GetHashCode() ^ this.m_Version.GetHashCode();
		}

		// Token: 0x060026A0 RID: 9888 RVA: 0x00040824 File Offset: 0x0003EA24
		public static bool operator ==(PlayableOutputHandle lhs, PlayableOutputHandle rhs)
		{
			return PlayableOutputHandle.CompareVersion(lhs, rhs);
		}

		// Token: 0x060026A1 RID: 9889 RVA: 0x00040840 File Offset: 0x0003EA40
		public static bool operator !=(PlayableOutputHandle lhs, PlayableOutputHandle rhs)
		{
			return !PlayableOutputHandle.CompareVersion(lhs, rhs);
		}

		// Token: 0x060026A2 RID: 9890 RVA: 0x0004085C File Offset: 0x0003EA5C
		public override bool Equals(object p)
		{
			return p is PlayableOutputHandle && this.Equals((PlayableOutputHandle)p);
		}

		// Token: 0x060026A3 RID: 9891 RVA: 0x00040888 File Offset: 0x0003EA88
		public bool Equals(PlayableOutputHandle other)
		{
			return PlayableOutputHandle.CompareVersion(this, other);
		}

		// Token: 0x060026A4 RID: 9892 RVA: 0x000408A8 File Offset: 0x0003EAA8
		internal static bool CompareVersion(PlayableOutputHandle lhs, PlayableOutputHandle rhs)
		{
			return lhs.m_Handle == rhs.m_Handle && lhs.m_Version == rhs.m_Version;
		}

		// Token: 0x060026A5 RID: 9893 RVA: 0x000408DE File Offset: 0x0003EADE
		[VisibleToOtherModules]
		internal bool IsNull()
		{
			return PlayableOutputHandle.IsNull_Injected(ref this);
		}

		// Token: 0x060026A6 RID: 9894 RVA: 0x000408E6 File Offset: 0x0003EAE6
		[VisibleToOtherModules]
		internal bool IsValid()
		{
			return PlayableOutputHandle.IsValid_Injected(ref this);
		}

		// Token: 0x060026A7 RID: 9895 RVA: 0x000408EE File Offset: 0x0003EAEE
		[FreeFunction("PlayableOutputHandleBindings::GetPlayableOutputType", HasExplicitThis = true, ThrowsException = true)]
		internal Type GetPlayableOutputType()
		{
			return PlayableOutputHandle.GetPlayableOutputType_Injected(ref this);
		}

		// Token: 0x060026A8 RID: 9896 RVA: 0x000408F6 File Offset: 0x0003EAF6
		[FreeFunction("PlayableOutputHandleBindings::GetReferenceObject", HasExplicitThis = true, ThrowsException = true)]
		internal Object GetReferenceObject()
		{
			return PlayableOutputHandle.GetReferenceObject_Injected(ref this);
		}

		// Token: 0x060026A9 RID: 9897 RVA: 0x000408FE File Offset: 0x0003EAFE
		[FreeFunction("PlayableOutputHandleBindings::SetReferenceObject", HasExplicitThis = true, ThrowsException = true)]
		internal void SetReferenceObject(Object target)
		{
			PlayableOutputHandle.SetReferenceObject_Injected(ref this, target);
		}

		// Token: 0x060026AA RID: 9898 RVA: 0x00040907 File Offset: 0x0003EB07
		[FreeFunction("PlayableOutputHandleBindings::GetUserData", HasExplicitThis = true, ThrowsException = true)]
		internal Object GetUserData()
		{
			return PlayableOutputHandle.GetUserData_Injected(ref this);
		}

		// Token: 0x060026AB RID: 9899 RVA: 0x0004090F File Offset: 0x0003EB0F
		[FreeFunction("PlayableOutputHandleBindings::SetUserData", HasExplicitThis = true, ThrowsException = true)]
		internal void SetUserData([Writable] Object target)
		{
			PlayableOutputHandle.SetUserData_Injected(ref this, target);
		}

		// Token: 0x060026AC RID: 9900 RVA: 0x00040918 File Offset: 0x0003EB18
		[FreeFunction("PlayableOutputHandleBindings::GetSourcePlayable", HasExplicitThis = true, ThrowsException = true)]
		internal PlayableHandle GetSourcePlayable()
		{
			PlayableHandle playableHandle;
			PlayableOutputHandle.GetSourcePlayable_Injected(ref this, out playableHandle);
			return playableHandle;
		}

		// Token: 0x060026AD RID: 9901 RVA: 0x0004092E File Offset: 0x0003EB2E
		[FreeFunction("PlayableOutputHandleBindings::SetSourcePlayable", HasExplicitThis = true, ThrowsException = true)]
		internal void SetSourcePlayable(PlayableHandle target, int port)
		{
			PlayableOutputHandle.SetSourcePlayable_Injected(ref this, ref target, port);
		}

		// Token: 0x060026AE RID: 9902 RVA: 0x00040939 File Offset: 0x0003EB39
		[FreeFunction("PlayableOutputHandleBindings::GetSourceOutputPort", HasExplicitThis = true, ThrowsException = true)]
		internal int GetSourceOutputPort()
		{
			return PlayableOutputHandle.GetSourceOutputPort_Injected(ref this);
		}

		// Token: 0x060026AF RID: 9903 RVA: 0x00040941 File Offset: 0x0003EB41
		[FreeFunction("PlayableOutputHandleBindings::GetWeight", HasExplicitThis = true, ThrowsException = true)]
		internal float GetWeight()
		{
			return PlayableOutputHandle.GetWeight_Injected(ref this);
		}

		// Token: 0x060026B0 RID: 9904 RVA: 0x00040949 File Offset: 0x0003EB49
		[FreeFunction("PlayableOutputHandleBindings::SetWeight", HasExplicitThis = true, ThrowsException = true)]
		internal void SetWeight(float weight)
		{
			PlayableOutputHandle.SetWeight_Injected(ref this, weight);
		}

		// Token: 0x060026B1 RID: 9905 RVA: 0x00040952 File Offset: 0x0003EB52
		[FreeFunction("PlayableOutputHandleBindings::PushNotification", HasExplicitThis = true, ThrowsException = true)]
		internal void PushNotification(PlayableHandle origin, INotification notification, object context)
		{
			PlayableOutputHandle.PushNotification_Injected(ref this, ref origin, notification, context);
		}

		// Token: 0x060026B2 RID: 9906 RVA: 0x0004095E File Offset: 0x0003EB5E
		[FreeFunction("PlayableOutputHandleBindings::GetNotificationReceivers", HasExplicitThis = true, ThrowsException = true)]
		internal INotificationReceiver[] GetNotificationReceivers()
		{
			return PlayableOutputHandle.GetNotificationReceivers_Injected(ref this);
		}

		// Token: 0x060026B3 RID: 9907 RVA: 0x00040966 File Offset: 0x0003EB66
		[FreeFunction("PlayableOutputHandleBindings::AddNotificationReceiver", HasExplicitThis = true, ThrowsException = true)]
		internal void AddNotificationReceiver(INotificationReceiver receiver)
		{
			PlayableOutputHandle.AddNotificationReceiver_Injected(ref this, receiver);
		}

		// Token: 0x060026B4 RID: 9908 RVA: 0x0004096F File Offset: 0x0003EB6F
		[FreeFunction("PlayableOutputHandleBindings::RemoveNotificationReceiver", HasExplicitThis = true, ThrowsException = true)]
		internal void RemoveNotificationReceiver(INotificationReceiver receiver)
		{
			PlayableOutputHandle.RemoveNotificationReceiver_Injected(ref this, receiver);
		}

		// Token: 0x060026B6 RID: 9910
		[MethodImpl(4096)]
		private static extern bool IsNull_Injected(ref PlayableOutputHandle _unity_self);

		// Token: 0x060026B7 RID: 9911
		[MethodImpl(4096)]
		private static extern bool IsValid_Injected(ref PlayableOutputHandle _unity_self);

		// Token: 0x060026B8 RID: 9912
		[MethodImpl(4096)]
		private static extern Type GetPlayableOutputType_Injected(ref PlayableOutputHandle _unity_self);

		// Token: 0x060026B9 RID: 9913
		[MethodImpl(4096)]
		private static extern Object GetReferenceObject_Injected(ref PlayableOutputHandle _unity_self);

		// Token: 0x060026BA RID: 9914
		[MethodImpl(4096)]
		private static extern void SetReferenceObject_Injected(ref PlayableOutputHandle _unity_self, Object target);

		// Token: 0x060026BB RID: 9915
		[MethodImpl(4096)]
		private static extern Object GetUserData_Injected(ref PlayableOutputHandle _unity_self);

		// Token: 0x060026BC RID: 9916
		[MethodImpl(4096)]
		private static extern void SetUserData_Injected(ref PlayableOutputHandle _unity_self, [Writable] Object target);

		// Token: 0x060026BD RID: 9917
		[MethodImpl(4096)]
		private static extern void GetSourcePlayable_Injected(ref PlayableOutputHandle _unity_self, out PlayableHandle ret);

		// Token: 0x060026BE RID: 9918
		[MethodImpl(4096)]
		private static extern void SetSourcePlayable_Injected(ref PlayableOutputHandle _unity_self, ref PlayableHandle target, int port);

		// Token: 0x060026BF RID: 9919
		[MethodImpl(4096)]
		private static extern int GetSourceOutputPort_Injected(ref PlayableOutputHandle _unity_self);

		// Token: 0x060026C0 RID: 9920
		[MethodImpl(4096)]
		private static extern float GetWeight_Injected(ref PlayableOutputHandle _unity_self);

		// Token: 0x060026C1 RID: 9921
		[MethodImpl(4096)]
		private static extern void SetWeight_Injected(ref PlayableOutputHandle _unity_self, float weight);

		// Token: 0x060026C2 RID: 9922
		[MethodImpl(4096)]
		private static extern void PushNotification_Injected(ref PlayableOutputHandle _unity_self, ref PlayableHandle origin, INotification notification, object context);

		// Token: 0x060026C3 RID: 9923
		[MethodImpl(4096)]
		private static extern INotificationReceiver[] GetNotificationReceivers_Injected(ref PlayableOutputHandle _unity_self);

		// Token: 0x060026C4 RID: 9924
		[MethodImpl(4096)]
		private static extern void AddNotificationReceiver_Injected(ref PlayableOutputHandle _unity_self, INotificationReceiver receiver);

		// Token: 0x060026C5 RID: 9925
		[MethodImpl(4096)]
		private static extern void RemoveNotificationReceiver_Injected(ref PlayableOutputHandle _unity_self, INotificationReceiver receiver);

		// Token: 0x04000E2A RID: 3626
		internal IntPtr m_Handle;

		// Token: 0x04000E2B RID: 3627
		internal uint m_Version;

		// Token: 0x04000E2C RID: 3628
		private static readonly PlayableOutputHandle m_Null = default(PlayableOutputHandle);
	}
}
