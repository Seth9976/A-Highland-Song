using System;
using UnityEngine.Events;

namespace UnityEngine.Networking.PlayerConnection
{
	// Token: 0x02000388 RID: 904
	public interface IEditorPlayerConnection
	{
		// Token: 0x06001EBD RID: 7869
		void Register(Guid messageId, UnityAction<MessageEventArgs> callback);

		// Token: 0x06001EBE RID: 7870
		void Unregister(Guid messageId, UnityAction<MessageEventArgs> callback);

		// Token: 0x06001EBF RID: 7871
		void DisconnectAll();

		// Token: 0x06001EC0 RID: 7872
		void RegisterConnection(UnityAction<int> callback);

		// Token: 0x06001EC1 RID: 7873
		void RegisterDisconnection(UnityAction<int> callback);

		// Token: 0x06001EC2 RID: 7874
		void UnregisterConnection(UnityAction<int> callback);

		// Token: 0x06001EC3 RID: 7875
		void UnregisterDisconnection(UnityAction<int> callback);

		// Token: 0x06001EC4 RID: 7876
		void Send(Guid messageId, byte[] data);

		// Token: 0x06001EC5 RID: 7877
		bool TrySend(Guid messageId, byte[] data);
	}
}
