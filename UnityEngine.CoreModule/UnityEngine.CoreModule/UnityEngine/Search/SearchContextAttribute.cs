using System;

namespace UnityEngine.Search
{
	// Token: 0x020002D2 RID: 722
	[AttributeUsage(256)]
	public class SearchContextAttribute : PropertyAttribute
	{
		// Token: 0x170005C9 RID: 1481
		// (get) Token: 0x06001DD2 RID: 7634 RVA: 0x00030926 File Offset: 0x0002EB26
		// (set) Token: 0x06001DD3 RID: 7635 RVA: 0x0003092E File Offset: 0x0002EB2E
		public string query { get; private set; }

		// Token: 0x170005CA RID: 1482
		// (get) Token: 0x06001DD4 RID: 7636 RVA: 0x00030937 File Offset: 0x0002EB37
		// (set) Token: 0x06001DD5 RID: 7637 RVA: 0x0003093F File Offset: 0x0002EB3F
		public string[] providerIds { get; private set; }

		// Token: 0x170005CB RID: 1483
		// (get) Token: 0x06001DD6 RID: 7638 RVA: 0x00030948 File Offset: 0x0002EB48
		// (set) Token: 0x06001DD7 RID: 7639 RVA: 0x00030950 File Offset: 0x0002EB50
		public Type[] instantiableProviders { get; private set; }

		// Token: 0x170005CC RID: 1484
		// (get) Token: 0x06001DD8 RID: 7640 RVA: 0x00030959 File Offset: 0x0002EB59
		// (set) Token: 0x06001DD9 RID: 7641 RVA: 0x00030961 File Offset: 0x0002EB61
		public SearchViewFlags flags { get; private set; }

		// Token: 0x06001DDA RID: 7642 RVA: 0x0003096A File Offset: 0x0002EB6A
		public SearchContextAttribute(string query)
			: this(query, null, SearchViewFlags.None)
		{
		}

		// Token: 0x06001DDB RID: 7643 RVA: 0x00030977 File Offset: 0x0002EB77
		public SearchContextAttribute(string query, SearchViewFlags flags)
			: this(query, null, flags)
		{
		}

		// Token: 0x06001DDC RID: 7644 RVA: 0x00030984 File Offset: 0x0002EB84
		public SearchContextAttribute(string query, string providerIdsCommaSeparated)
			: this(query, providerIdsCommaSeparated, SearchViewFlags.None)
		{
		}

		// Token: 0x06001DDD RID: 7645 RVA: 0x00030991 File Offset: 0x0002EB91
		public SearchContextAttribute(string query, string providerIdsCommaSeparated, SearchViewFlags flags)
			: this(query, flags, providerIdsCommaSeparated, null)
		{
		}

		// Token: 0x06001DDE RID: 7646 RVA: 0x0003099F File Offset: 0x0002EB9F
		public SearchContextAttribute(string query, params Type[] instantiableProviders)
			: this(query, SearchViewFlags.None, null, instantiableProviders)
		{
		}

		// Token: 0x06001DDF RID: 7647 RVA: 0x000309AD File Offset: 0x0002EBAD
		public SearchContextAttribute(string query, SearchViewFlags flags, params Type[] instantiableProviders)
			: this(query, flags, null, instantiableProviders)
		{
		}

		// Token: 0x06001DE0 RID: 7648 RVA: 0x000309BC File Offset: 0x0002EBBC
		public SearchContextAttribute(string query, SearchViewFlags flags, string providerIdsCommaSeparated, params Type[] instantiableProviders)
		{
			this.query = ((string.IsNullOrEmpty(query) || query.EndsWith(" ")) ? query : (query + " "));
			this.providerIds = ((providerIdsCommaSeparated != null) ? providerIdsCommaSeparated.Split(new char[] { ',', ';' }) : null) ?? new string[0];
			this.instantiableProviders = instantiableProviders ?? new Type[0];
			this.flags = flags;
		}
	}
}
