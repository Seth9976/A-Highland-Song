using System;

namespace UnityEngine.SceneManagement
{
	// Token: 0x020002E8 RID: 744
	[Serializable]
	public struct CreateSceneParameters
	{
		// Token: 0x170005E6 RID: 1510
		// (get) Token: 0x06001E72 RID: 7794 RVA: 0x00031660 File Offset: 0x0002F860
		// (set) Token: 0x06001E73 RID: 7795 RVA: 0x00031678 File Offset: 0x0002F878
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

		// Token: 0x06001E74 RID: 7796 RVA: 0x00031678 File Offset: 0x0002F878
		public CreateSceneParameters(LocalPhysicsMode physicsMode)
		{
			this.m_LocalPhysicsMode = physicsMode;
		}

		// Token: 0x040009F0 RID: 2544
		[SerializeField]
		private LocalPhysicsMode m_LocalPhysicsMode;
	}
}
