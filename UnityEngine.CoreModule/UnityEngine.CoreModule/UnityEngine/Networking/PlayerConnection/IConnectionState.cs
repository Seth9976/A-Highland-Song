using System;
using UnityEngine.Scripting.APIUpdating;

namespace UnityEngine.Networking.PlayerConnection
{
	// Token: 0x02000386 RID: 902
	[MovedFrom("UnityEngine.Experimental.Networking.PlayerConnection")]
	public interface IConnectionState : IDisposable
	{
		// Token: 0x170005EC RID: 1516
		// (get) Token: 0x06001EBA RID: 7866
		ConnectionTarget connectedToTarget { get; }

		// Token: 0x170005ED RID: 1517
		// (get) Token: 0x06001EBB RID: 7867
		string connectionName { get; }
	}
}
