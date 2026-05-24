using System;

namespace UnityEngine.SceneManagement
{
	// Token: 0x020002E7 RID: 743
	[Serializable]
	public struct LoadSceneParameters
	{
		// Token: 0x170005E4 RID: 1508
		// (get) Token: 0x06001E6C RID: 7788 RVA: 0x000315F8 File Offset: 0x0002F7F8
		// (set) Token: 0x06001E6D RID: 7789 RVA: 0x00031610 File Offset: 0x0002F810
		public LoadSceneMode loadSceneMode
		{
			get
			{
				return this.m_LoadSceneMode;
			}
			set
			{
				this.m_LoadSceneMode = value;
			}
		}

		// Token: 0x170005E5 RID: 1509
		// (get) Token: 0x06001E6E RID: 7790 RVA: 0x0003161C File Offset: 0x0002F81C
		// (set) Token: 0x06001E6F RID: 7791 RVA: 0x00031634 File Offset: 0x0002F834
		public LocalPhysicsMode localPhysicsMode
		{
			get
			{
				return this.m_LocalPhysicsMode;
			}
			set
			{
				this.m_LocalPhysicsMode = value;
			}
		}

		// Token: 0x06001E70 RID: 7792 RVA: 0x0003163E File Offset: 0x0002F83E
		public LoadSceneParameters(LoadSceneMode mode)
		{
			this.m_LoadSceneMode = mode;
			this.m_LocalPhysicsMode = LocalPhysicsMode.None;
		}

		// Token: 0x06001E71 RID: 7793 RVA: 0x0003164F File Offset: 0x0002F84F
		public LoadSceneParameters(LoadSceneMode mode, LocalPhysicsMode physicsMode)
		{
			this.m_LoadSceneMode = mode;
			this.m_LocalPhysicsMode = physicsMode;
		}

		// Token: 0x040009EE RID: 2542
		[SerializeField]
		private LoadSceneMode m_LoadSceneMode;

		// Token: 0x040009EF RID: 2543
		[SerializeField]
		private LocalPhysicsMode m_LocalPhysicsMode;
	}
}
