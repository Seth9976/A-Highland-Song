using System;

namespace UnityEngine
{
	// Token: 0x020001E9 RID: 489
	public class ResourcesAPI
	{
		// Token: 0x1700045F RID: 1119
		// (get) Token: 0x06001616 RID: 5654 RVA: 0x000235D9 File Offset: 0x000217D9
		internal static ResourcesAPI ActiveAPI
		{
			get
			{
				return ResourcesAPI.overrideAPI ?? ResourcesAPI.s_DefaultAPI;
			}
		}

		// Token: 0x17000460 RID: 1120
		// (get) Token: 0x06001617 RID: 5655 RVA: 0x000235E9 File Offset: 0x000217E9
		// (set) Token: 0x06001618 RID: 5656 RVA: 0x000235F0 File Offset: 0x000217F0
		public static ResourcesAPI overrideAPI { get; set; }

		// Token: 0x06001619 RID: 5657 RVA: 0x00008CAF File Offset: 0x00006EAF
		protected internal ResourcesAPI()
		{
		}

		// Token: 0x0600161A RID: 5658 RVA: 0x000235F8 File Offset: 0x000217F8
		protected internal virtual Object[] FindObjectsOfTypeAll(Type systemTypeInstance)
		{
			return ResourcesAPIInternal.FindObjectsOfTypeAll(systemTypeInstance);
		}

		// Token: 0x0600161B RID: 5659 RVA: 0x00023600 File Offset: 0x00021800
		protected internal virtual Shader FindShaderByName(string name)
		{
			return ResourcesAPIInternal.FindShaderByName(name);
		}

		// Token: 0x0600161C RID: 5660 RVA: 0x00023608 File Offset: 0x00021808
		protected internal virtual Object Load(string path, Type systemTypeInstance)
		{
			return ResourcesAPIInternal.Load(path, systemTypeInstance);
		}

		// Token: 0x0600161D RID: 5661 RVA: 0x00023611 File Offset: 0x00021811
		protected internal virtual Object[] LoadAll(string path, Type systemTypeInstance)
		{
			return ResourcesAPIInternal.LoadAll(path, systemTypeInstance);
		}

		// Token: 0x0600161E RID: 5662 RVA: 0x0002361C File Offset: 0x0002181C
		protected internal virtual ResourceRequest LoadAsync(string path, Type systemTypeInstance)
		{
			ResourceRequest resourceRequest = ResourcesAPIInternal.LoadAsyncInternal(path, systemTypeInstance);
			resourceRequest.m_Path = path;
			resourceRequest.m_Type = systemTypeInstance;
			return resourceRequest;
		}

		// Token: 0x0600161F RID: 5663 RVA: 0x00023645 File Offset: 0x00021845
		protected internal virtual void UnloadAsset(Object assetToUnload)
		{
			ResourcesAPIInternal.UnloadAsset(assetToUnload);
		}

		// Token: 0x040007C7 RID: 1991
		private static ResourcesAPI s_DefaultAPI = new ResourcesAPI();
	}
}
