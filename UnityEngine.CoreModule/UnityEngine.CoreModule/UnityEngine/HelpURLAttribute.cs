using System;
using UnityEngine.Scripting;

namespace UnityEngine
{
	// Token: 0x020001F6 RID: 502
	[UsedByNativeCode]
	[AttributeUsage(4, AllowMultiple = false)]
	public class HelpURLAttribute : Attribute
	{
		// Token: 0x0600165F RID: 5727 RVA: 0x00023D77 File Offset: 0x00021F77
		public HelpURLAttribute(string url)
		{
			this.m_Url = url;
			this.m_DispatchingFieldName = "";
			this.m_Dispatcher = false;
		}

		// Token: 0x06001660 RID: 5728 RVA: 0x00023D9A File Offset: 0x00021F9A
		internal HelpURLAttribute(string defaultURL, string dispatchingFieldName)
		{
			this.m_Url = defaultURL;
			this.m_DispatchingFieldName = dispatchingFieldName;
			this.m_Dispatcher = !string.IsNullOrEmpty(dispatchingFieldName);
		}

		// Token: 0x1700046A RID: 1130
		// (get) Token: 0x06001661 RID: 5729 RVA: 0x00023DC1 File Offset: 0x00021FC1
		public string URL
		{
			get
			{
				return this.m_Url;
			}
		}

		// Token: 0x040007D6 RID: 2006
		internal readonly string m_Url;

		// Token: 0x040007D7 RID: 2007
		internal readonly bool m_Dispatcher;

		// Token: 0x040007D8 RID: 2008
		internal readonly string m_DispatchingFieldName;
	}
}
