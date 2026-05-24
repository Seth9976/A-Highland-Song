using System;
using System.ComponentModel;
using UnityEngine.Networking.PlayerConnection;

namespace UnityEngine.Diagnostics
{
	// Token: 0x0200044E RID: 1102
	public static class PlayerConnection
	{
		// Token: 0x17000730 RID: 1840
		// (get) Token: 0x060026E1 RID: 9953 RVA: 0x00040DCC File Offset: 0x0003EFCC
		[Obsolete("Use UnityEngine.Networking.PlayerConnection.PlayerConnection.instance.isConnected instead.")]
		public static bool connected
		{
			get
			{
				return PlayerConnection.instance.isConnected;
			}
		}

		// Token: 0x060026E2 RID: 9954 RVA: 0x00004557 File Offset: 0x00002757
		[EditorBrowsable(1)]
		[Obsolete("PlayerConnection.SendFile is no longer supported.", true)]
		public static void SendFile(string remoteFilePath, byte[] data)
		{
		}
	}
}
