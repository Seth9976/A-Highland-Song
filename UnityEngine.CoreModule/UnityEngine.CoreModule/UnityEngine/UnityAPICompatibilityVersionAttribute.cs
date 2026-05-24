using System;

namespace UnityEngine
{
	// Token: 0x02000223 RID: 547
	[AttributeUsage(1, AllowMultiple = false)]
	public class UnityAPICompatibilityVersionAttribute : Attribute
	{
		// Token: 0x06001789 RID: 6025 RVA: 0x00026166 File Offset: 0x00024366
		[Obsolete("This overload of the attribute has been deprecated. Use the constructor that takes the version and a boolean", true)]
		public UnityAPICompatibilityVersionAttribute(string version)
		{
			this._version = version;
		}

		// Token: 0x0600178A RID: 6026 RVA: 0x00026178 File Offset: 0x00024378
		public UnityAPICompatibilityVersionAttribute(string version, bool checkOnlyUnityVersion)
		{
			bool flag = !checkOnlyUnityVersion;
			if (flag)
			{
				throw new ArgumentException("You must pass 'true' to checkOnlyUnityVersion parameter.");
			}
			this._version = version;
		}

		// Token: 0x0600178B RID: 6027 RVA: 0x000261A7 File Offset: 0x000243A7
		public UnityAPICompatibilityVersionAttribute(string version, string[] configurationAssembliesHashes)
		{
			this._version = version;
			this._configurationAssembliesHashes = configurationAssembliesHashes;
		}

		// Token: 0x17000489 RID: 1161
		// (get) Token: 0x0600178C RID: 6028 RVA: 0x000261C0 File Offset: 0x000243C0
		public string version
		{
			get
			{
				return this._version;
			}
		}

		// Token: 0x1700048A RID: 1162
		// (get) Token: 0x0600178D RID: 6029 RVA: 0x000261D8 File Offset: 0x000243D8
		internal string[] configurationAssembliesHashes
		{
			get
			{
				return this._configurationAssembliesHashes;
			}
		}

		// Token: 0x04000813 RID: 2067
		private string _version;

		// Token: 0x04000814 RID: 2068
		private string[] _configurationAssembliesHashes;
	}
}
