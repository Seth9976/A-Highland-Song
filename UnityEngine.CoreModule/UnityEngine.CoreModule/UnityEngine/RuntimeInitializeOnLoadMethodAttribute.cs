using System;
using UnityEngine.Scripting;

namespace UnityEngine
{
	// Token: 0x02000212 RID: 530
	[AttributeUsage(64, AllowMultiple = false)]
	[RequiredByNativeCode]
	public class RuntimeInitializeOnLoadMethodAttribute : PreserveAttribute
	{
		// Token: 0x06001746 RID: 5958 RVA: 0x00025698 File Offset: 0x00023898
		public RuntimeInitializeOnLoadMethodAttribute()
		{
			this.loadType = RuntimeInitializeLoadType.AfterSceneLoad;
		}

		// Token: 0x06001747 RID: 5959 RVA: 0x000256AA File Offset: 0x000238AA
		public RuntimeInitializeOnLoadMethodAttribute(RuntimeInitializeLoadType loadType)
		{
			this.loadType = loadType;
		}

		// Token: 0x17000485 RID: 1157
		// (get) Token: 0x06001748 RID: 5960 RVA: 0x000256BC File Offset: 0x000238BC
		// (set) Token: 0x06001749 RID: 5961 RVA: 0x000256D4 File Offset: 0x000238D4
		public RuntimeInitializeLoadType loadType
		{
			get
			{
				return this.m_LoadType;
			}
			private set
			{
				this.m_LoadType = value;
			}
		}

		// Token: 0x04000800 RID: 2048
		private RuntimeInitializeLoadType m_LoadType;
	}
}
