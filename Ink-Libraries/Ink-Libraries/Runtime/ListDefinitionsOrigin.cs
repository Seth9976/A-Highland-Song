using System;
using System.Collections.Generic;

namespace Ink.Runtime
{
	// Token: 0x02000020 RID: 32
	public class ListDefinitionsOrigin
	{
		// Token: 0x17000053 RID: 83
		// (get) Token: 0x060001D8 RID: 472 RVA: 0x0000B188 File Offset: 0x00009388
		public List<ListDefinition> lists
		{
			get
			{
				List<ListDefinition> list = new List<ListDefinition>();
				foreach (KeyValuePair<string, ListDefinition> keyValuePair in this._lists)
				{
					list.Add(keyValuePair.Value);
				}
				return list;
			}
		}

		// Token: 0x060001D9 RID: 473 RVA: 0x0000B1E8 File Offset: 0x000093E8
		public ListDefinitionsOrigin(List<ListDefinition> lists)
		{
			this._lists = new Dictionary<string, ListDefinition>();
			this._allUnambiguousListValueCache = new Dictionary<string, ListValue>();
			foreach (ListDefinition listDefinition in lists)
			{
				this._lists[listDefinition.name] = listDefinition;
				foreach (KeyValuePair<InkListItem, int> keyValuePair in listDefinition.items)
				{
					InkListItem key = keyValuePair.Key;
					int value = keyValuePair.Value;
					ListValue listValue = new ListValue(key, value);
					this._allUnambiguousListValueCache[key.itemName] = listValue;
					this._allUnambiguousListValueCache[key.fullName] = listValue;
				}
			}
		}

		// Token: 0x060001DA RID: 474 RVA: 0x0000B2E4 File Offset: 0x000094E4
		public bool TryListGetDefinition(string name, out ListDefinition def)
		{
			return this._lists.TryGetValue(name, out def);
		}

		// Token: 0x060001DB RID: 475 RVA: 0x0000B2F4 File Offset: 0x000094F4
		public ListValue FindSingleItemListWithName(string name)
		{
			ListValue listValue = null;
			if (!string.IsNullOrWhiteSpace(name))
			{
				this._allUnambiguousListValueCache.TryGetValue(name, out listValue);
			}
			return listValue;
		}

		// Token: 0x0400007A RID: 122
		private Dictionary<string, ListDefinition> _lists;

		// Token: 0x0400007B RID: 123
		private Dictionary<string, ListValue> _allUnambiguousListValueCache;
	}
}
