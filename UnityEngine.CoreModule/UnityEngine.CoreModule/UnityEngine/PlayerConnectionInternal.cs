using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;

namespace UnityEngine
{
	// Token: 0x020001CF RID: 463
	[NativeHeader("Runtime/Export/PlayerConnection/PlayerConnectionInternal.bindings.h")]
	internal class PlayerConnectionInternal : IPlayerEditorConnectionNative
	{
		// Token: 0x0600159E RID: 5534 RVA: 0x00022D94 File Offset: 0x00020F94
		void IPlayerEditorConnectionNative.SendMessage(Guid messageId, byte[] data, int playerId)
		{
			bool flag = messageId == Guid.Empty;
			if (flag)
			{
				throw new ArgumentException("messageId must not be empty");
			}
			PlayerConnectionInternal.SendMessage(messageId.ToString("N"), data, playerId);
		}

		// Token: 0x0600159F RID: 5535 RVA: 0x00022DD4 File Offset: 0x00020FD4
		bool IPlayerEditorConnectionNative.TrySendMessage(Guid messageId, byte[] data, int playerId)
		{
			bool flag = messageId == Guid.Empty;
			if (flag)
			{
				throw new ArgumentException("messageId must not be empty");
			}
			return PlayerConnectionInternal.TrySendMessage(messageId.ToString("N"), data, playerId);
		}

		// Token: 0x060015A0 RID: 5536 RVA: 0x00022E14 File Offset: 0x00021014
		void IPlayerEditorConnectionNative.Poll()
		{
			PlayerConnectionInternal.PollInternal();
		}

		// Token: 0x060015A1 RID: 5537 RVA: 0x00022E1D File Offset: 0x0002101D
		void IPlayerEditorConnectionNative.RegisterInternal(Guid messageId)
		{
			PlayerConnectionInternal.RegisterInternal(messageId.ToString("N"));
		}

		// Token: 0x060015A2 RID: 5538 RVA: 0x00022E32 File Offset: 0x00021032
		void IPlayerEditorConnectionNative.UnregisterInternal(Guid messageId)
		{
			PlayerConnectionInternal.UnregisterInternal(messageId.ToString("N"));
		}

		// Token: 0x060015A3 RID: 5539 RVA: 0x00022E47 File Offset: 0x00021047
		void IPlayerEditorConnectionNative.Initialize()
		{
			PlayerConnectionInternal.Initialize();
		}

		// Token: 0x060015A4 RID: 5540 RVA: 0x00022E50 File Offset: 0x00021050
		bool IPlayerEditorConnectionNative.IsConnected()
		{
			return PlayerConnectionInternal.IsConnected();
		}

		// Token: 0x060015A5 RID: 5541 RVA: 0x00022E67 File Offset: 0x00021067
		void IPlayerEditorConnectionNative.DisconnectAll()
		{
			PlayerConnectionInternal.DisconnectAll();
		}

		// Token: 0x060015A6 RID: 5542
		[FreeFunction("PlayerConnection_Bindings::IsConnected")]
		[MethodImpl(4096)]
		private static extern bool IsConnected();

		// Token: 0x060015A7 RID: 5543
		[FreeFunction("PlayerConnection_Bindings::Initialize")]
		[MethodImpl(4096)]
		private static extern void Initialize();

		// Token: 0x060015A8 RID: 5544
		[FreeFunction("PlayerConnection_Bindings::RegisterInternal")]
		[MethodImpl(4096)]
		private static extern void RegisterInternal(string messageId);

		// Token: 0x060015A9 RID: 5545
		[FreeFunction("PlayerConnection_Bindings::UnregisterInternal")]
		[MethodImpl(4096)]
		private static extern void UnregisterInternal(string messageId);

		// Token: 0x060015AA RID: 5546
		[FreeFunction("PlayerConnection_Bindings::SendMessage")]
		[MethodImpl(4096)]
		private static extern void SendMessage(string messageId, byte[] data, int playerId);

		// Token: 0x060015AB RID: 5547
		[FreeFunction("PlayerConnection_Bindings::TrySendMessage")]
		[MethodImpl(4096)]
		private static extern bool TrySendMessage(string messageId, byte[] data, int playerId);

		// Token: 0x060015AC RID: 5548
		[FreeFunction("PlayerConnection_Bindings::PollInternal")]
		[MethodImpl(4096)]
		private static extern void PollInternal();

		// Token: 0x060015AD RID: 5549
		[FreeFunction("PlayerConnection_Bindings::DisconnectAll")]
		[MethodImpl(4096)]
		private static extern void DisconnectAll();

		// Token: 0x020001D0 RID: 464
		[Flags]
		public enum MulticastFlags
		{
			// Token: 0x040007A6 RID: 1958
			kRequestImmediateConnect = 1,
			// Token: 0x040007A7 RID: 1959
			kSupportsProfile = 2,
			// Token: 0x040007A8 RID: 1960
			kCustomMessage = 4,
			// Token: 0x040007A9 RID: 1961
			kUseAlternateIP = 8
		}
	}
}
