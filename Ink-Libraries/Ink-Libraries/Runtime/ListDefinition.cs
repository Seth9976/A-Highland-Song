using System;
using System.Collections.Generic;

namespace Ink.Runtime
{
	// Token: 0x0200001F RID: 31
	public class ListDefinition
	{
		// Token: 0x17000051 RID: 81
		// (get) Token: 0x060001D0 RID: 464 RVA: 0x0000AFE9 File Offset: 0x000091E9
		public string name
		{
			get
			{
				return this._name;
			}
		}

		// Token: 0x17000052 RID: 82
		// (get) Token: 0x060001D1 RID: 465 RVA: 0x0000AFF4 File Offset: 0x000091F4
		public Dictionary<InkListItem, int> items
		{
			get
			{
				if (this._items == null)
				{
					this._items = new Dictionary<InkListItem, int>();
					foreach (KeyValuePair<string, int> keyValuePair in this._itemNameToValues)
					{
						InkListItem inkListItem = new InkListItem(this.name, keyValuePair.Key);
						this._items[inkListItem] = keyValuePair.Value;
					}
				}
				return this._items;
			}
		}

		// Token: 0x060001D2 RID: 466 RVA: 0x0000B080 File Offset: 0x00009280
		public int ValueForItem(InkListItem item)
		{
			int num;
			if (this._itemNameToValues.TryGetValue(item.itemName, out num))
			{
				return num;
			}
			return 0;
		}

		// Token: 0x060001D3 RID: 467 RVA: 0x0000B0A5 File Offset: 0x000092A5
		public bool ContainsItem(InkListItem item)
		{
			return !(item.originName != this.name) && this._itemNameToValues.ContainsKey(item.itemName);
		}

		// Token: 0x060001D4 RID: 468 RVA: 0x0000B0CD File Offset: 0x000092CD
		public bool ContainsItemWithName(string itemName)
		{
			return this._itemNameToValues.ContainsKey(itemName);
		}

		// Token: 0x060001D5 RID: 469 RVA: 0x0000B0DC File Offset: 0x000092DC
		public bool TryGetItemWithValue(int val, out InkListItem item)
		{
			foreach (KeyValuePair<string, int> keyValuePair in this._itemNameToValues)
			{
				if (keyValuePair.Value == val)
				{
					item = new InkListItem(this.name, keyValuePair.Key);
					return true;
				}
			}
			item = InkListItem.Null;
			return false;
		}

		// Token: 0x060001D6 RID: 470 RVA: 0x0000B15C File Offset: 0x0000935C
		public bool TryGetValueForItem(InkListItem item, out int intVal)
		{
			return this._itemNameToValues.TryGetValue(item.itemName, out intVal);
		}

		// Token: 0x060001D7 RID: 471 RVA: 0x0000B170 File Offset: 0x00009370
		public ListDefinition(string name, Dictionary<string, int> items)
		{
			this._name = name;
			this._itemNameToValues = items;
		}

		// Token: 0x04000077 RID: 119
		private Dictionary<InkListItem, int> _items;

		// Token: 0x04000078 RID: 120
		private string _name;

		// Token: 0x04000079 RID: 121
		private Dictionary<string, int> _itemNameToValues;
	}
}
