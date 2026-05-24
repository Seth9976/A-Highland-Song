using System;
using UnityEngine.Scripting;

namespace UnityEngine
{
	// Token: 0x020000EF RID: 239
	[UsedByNativeCode]
	public struct CachedAssetBundle
	{
		// Token: 0x06000448 RID: 1096 RVA: 0x00006FA2 File Offset: 0x000051A2
		public CachedAssetBundle(string name, Hash128 hash)
		{
			this.m_Name = name;
			this.m_Hash = hash;
		}

		// Token: 0x170000D1 RID: 209
		// (get) Token: 0x06000449 RID: 1097 RVA: 0x00006FB4 File Offset: 0x000051B4
		// (set) Token: 0x0600044A RID: 1098 RVA: 0x00006FCC File Offset: 0x000051CC
		public string name
		{
			get
			{
				return this.m_Name;
			}
			set
			{
				this.m_Name = value;
			}
		}

		// Token: 0x170000D2 RID: 210
		// (get) Token: 0x0600044B RID: 1099 RVA: 0x00006FD8 File Offset: 0x000051D8
		// (set) Token: 0x0600044C RID: 1100 RVA: 0x00006FF0 File Offset: 0x000051F0
		public Hash128 hash
		{
			get
			{
				return this.m_Hash;
			}
			set
			{
				this.m_Hash = value;
			}
		}

		// Token: 0x04000326 RID: 806
		private string m_Name;

		// Token: 0x04000327 RID: 807
		private Hash128 m_Hash;
	}
}
