using System;

namespace UnityEngine.Lumin
{
	// Token: 0x02000394 RID: 916
	[AttributeUsage(4, AllowMultiple = false)]
	public sealed class UsesLuminPlatformLevelAttribute : Attribute
	{
		// Token: 0x06001EEE RID: 7918 RVA: 0x000325CF File Offset: 0x000307CF
		public UsesLuminPlatformLevelAttribute(uint platformLevel)
		{
			this.m_PlatformLevel = platformLevel;
		}

		// Token: 0x170005F1 RID: 1521
		// (get) Token: 0x06001EEF RID: 7919 RVA: 0x000325E0 File Offset: 0x000307E0
		public uint platformLevel
		{
			get
			{
				return this.m_PlatformLevel;
			}
		}

		// Token: 0x04000A30 RID: 2608
		private readonly uint m_PlatformLevel;
	}
}
