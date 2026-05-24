using System;
using System.Collections.Generic;
using System.Text;

namespace Ink.Runtime
{
	// Token: 0x0200001D RID: 29
	public class InkList : Dictionary<InkListItem, int>
	{
		// Token: 0x0600019E RID: 414 RVA: 0x000090BE File Offset: 0x000072BE
		public InkList()
		{
		}

		// Token: 0x0600019F RID: 415 RVA: 0x000090C8 File Offset: 0x000072C8
		public InkList(InkList otherList)
			: base(otherList)
		{
			List<string> originNames = otherList.originNames;
			if (originNames != null)
			{
				this._originNames = new List<string>(originNames);
			}
			if (otherList.origins != null)
			{
				this.origins = new List<ListDefinition>(otherList.origins);
			}
		}

		// Token: 0x060001A0 RID: 416 RVA: 0x0000910C File Offset: 0x0000730C
		public InkList(string singleOriginListName, Story originStory)
		{
			this.SetInitialOriginName(singleOriginListName);
			ListDefinition listDefinition;
			if (originStory.listDefinitions.TryListGetDefinition(singleOriginListName, out listDefinition))
			{
				this.origins = new List<ListDefinition> { listDefinition };
				return;
			}
			throw new Exception("InkList origin could not be found in story when constructing new list: " + singleOriginListName);
		}

		// Token: 0x060001A1 RID: 417 RVA: 0x00009159 File Offset: 0x00007359
		public InkList(InkListItem inkListItem, Story originStory)
			: this(inkListItem.originName, originStory)
		{
			this.AddItem(inkListItem);
		}

		// Token: 0x060001A2 RID: 418 RVA: 0x0000916F File Offset: 0x0000736F
		public InkList(KeyValuePair<InkListItem, int> singleElement)
		{
			base.Add(singleElement.Key, singleElement.Value);
		}

		// Token: 0x060001A3 RID: 419 RVA: 0x0000918C File Offset: 0x0000738C
		public static InkList FromString(string myListItem, Story originStory)
		{
			if (myListItem == "")
			{
				return new InkList();
			}
			ListValue listValue = originStory.listDefinitions.FindSingleItemListWithName(myListItem);
			if (listValue)
			{
				return new InkList(listValue.value);
			}
			throw new Exception("Could not find the InkListItem from the string '" + myListItem + "' to create an InkList because it doesn't exist in the original list definition in ink.");
		}

		// Token: 0x060001A4 RID: 420 RVA: 0x000091E4 File Offset: 0x000073E4
		public void AddItem(InkListItem item)
		{
			if (item.originName == null)
			{
				this.AddItem(item.itemName, null);
				return;
			}
			foreach (ListDefinition listDefinition in this.origins)
			{
				if (listDefinition.name == item.originName)
				{
					int num;
					if (listDefinition.TryGetValueForItem(item, out num))
					{
						base[item] = num;
						return;
					}
					string text = "Could not add the item ";
					InkListItem inkListItem = item;
					throw new Exception(text + inkListItem.ToString() + " to this list because it doesn't exist in the original list definition in ink.");
				}
			}
			throw new Exception("Failed to add item to list because the item was from a new list definition that wasn't previously known to this list. Only items from previously known lists can be used, so that the int value can be found.");
		}

		// Token: 0x060001A5 RID: 421 RVA: 0x000092A0 File Offset: 0x000074A0
		public void AddItem(string itemName, Story storyObject = null)
		{
			ListDefinition listDefinition = null;
			if (this.origins != null)
			{
				foreach (ListDefinition listDefinition2 in this.origins)
				{
					if (listDefinition2.ContainsItemWithName(itemName))
					{
						if (listDefinition != null)
						{
							throw new Exception(string.Concat(new string[] { "Could not add the item ", itemName, " to this list because it could come from either ", listDefinition2.name, " or ", listDefinition.name }));
						}
						listDefinition = listDefinition2;
					}
				}
			}
			if (listDefinition != null)
			{
				InkListItem inkListItem = new InkListItem(listDefinition.name, itemName);
				int num = listDefinition.ValueForItem(inkListItem);
				base[inkListItem] = num;
				return;
			}
			if (storyObject == null)
			{
				throw new Exception("Could not add the item " + itemName + " to this list because it isn't known to any list definitions previously associated with this list, and no ink Story object was provided to create it from.");
			}
			KeyValuePair<InkListItem, int> keyValuePair = InkList.FromString(itemName, storyObject).orderedItems[0];
			base[keyValuePair.Key] = keyValuePair.Value;
		}

		// Token: 0x060001A6 RID: 422 RVA: 0x000093B0 File Offset: 0x000075B0
		public bool ContainsItemNamed(string itemName)
		{
			foreach (KeyValuePair<InkListItem, int> keyValuePair in this)
			{
				if (keyValuePair.Key.itemName == itemName)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x17000049 RID: 73
		// (get) Token: 0x060001A7 RID: 423 RVA: 0x00009414 File Offset: 0x00007614
		public ListDefinition originOfMaxItem
		{
			get
			{
				if (this.origins == null)
				{
					return null;
				}
				string originName = this.maxItem.Key.originName;
				foreach (ListDefinition listDefinition in this.origins)
				{
					if (listDefinition.name == originName)
					{
						return listDefinition;
					}
				}
				return null;
			}
		}

		// Token: 0x1700004A RID: 74
		// (get) Token: 0x060001A8 RID: 424 RVA: 0x00009498 File Offset: 0x00007698
		public List<string> originNames
		{
			get
			{
				if (base.Count > 0)
				{
					if (this._originNames == null && base.Count > 0)
					{
						this._originNames = new List<string>();
					}
					else
					{
						this._originNames.Clear();
					}
					foreach (KeyValuePair<InkListItem, int> keyValuePair in this)
					{
						this._originNames.Add(keyValuePair.Key.originName);
					}
				}
				return this._originNames;
			}
		}

		// Token: 0x060001A9 RID: 425 RVA: 0x00009530 File Offset: 0x00007730
		public void SetInitialOriginName(string initialOriginName)
		{
			this._originNames = new List<string> { initialOriginName };
		}

		// Token: 0x060001AA RID: 426 RVA: 0x00009544 File Offset: 0x00007744
		public void SetInitialOriginNames(List<string> initialOriginNames)
		{
			if (initialOriginNames == null)
			{
				this._originNames = null;
				return;
			}
			this._originNames = new List<string>(initialOriginNames);
		}

		// Token: 0x1700004B RID: 75
		// (get) Token: 0x060001AB RID: 427 RVA: 0x00009560 File Offset: 0x00007760
		public KeyValuePair<InkListItem, int> maxItem
		{
			get
			{
				KeyValuePair<InkListItem, int> keyValuePair = default(KeyValuePair<InkListItem, int>);
				foreach (KeyValuePair<InkListItem, int> keyValuePair2 in this)
				{
					if (keyValuePair.Key.isNull || keyValuePair2.Value > keyValuePair.Value)
					{
						keyValuePair = keyValuePair2;
					}
				}
				return keyValuePair;
			}
		}

		// Token: 0x1700004C RID: 76
		// (get) Token: 0x060001AC RID: 428 RVA: 0x000095D4 File Offset: 0x000077D4
		public KeyValuePair<InkListItem, int> minItem
		{
			get
			{
				KeyValuePair<InkListItem, int> keyValuePair = default(KeyValuePair<InkListItem, int>);
				foreach (KeyValuePair<InkListItem, int> keyValuePair2 in this)
				{
					if (keyValuePair.Key.isNull || keyValuePair2.Value < keyValuePair.Value)
					{
						keyValuePair = keyValuePair2;
					}
				}
				return keyValuePair;
			}
		}

		// Token: 0x1700004D RID: 77
		// (get) Token: 0x060001AD RID: 429 RVA: 0x00009648 File Offset: 0x00007848
		public InkList inverse
		{
			get
			{
				InkList inkList = new InkList();
				if (this.origins != null)
				{
					foreach (ListDefinition listDefinition in this.origins)
					{
						foreach (KeyValuePair<InkListItem, int> keyValuePair in listDefinition.items)
						{
							if (!base.ContainsKey(keyValuePair.Key))
							{
								inkList.Add(keyValuePair.Key, keyValuePair.Value);
							}
						}
					}
				}
				return inkList;
			}
		}

		// Token: 0x1700004E RID: 78
		// (get) Token: 0x060001AE RID: 430 RVA: 0x00009700 File Offset: 0x00007900
		public InkList all
		{
			get
			{
				InkList inkList = new InkList();
				if (this.origins != null)
				{
					foreach (ListDefinition listDefinition in this.origins)
					{
						foreach (KeyValuePair<InkListItem, int> keyValuePair in listDefinition.items)
						{
							inkList[keyValuePair.Key] = keyValuePair.Value;
						}
					}
				}
				return inkList;
			}
		}

		// Token: 0x060001AF RID: 431 RVA: 0x000097A8 File Offset: 0x000079A8
		public InkList Union(InkList otherList)
		{
			InkList inkList = new InkList(this);
			foreach (KeyValuePair<InkListItem, int> keyValuePair in otherList)
			{
				inkList[keyValuePair.Key] = keyValuePair.Value;
			}
			return inkList;
		}

		// Token: 0x060001B0 RID: 432 RVA: 0x0000980C File Offset: 0x00007A0C
		public InkList Intersect(InkList otherList)
		{
			InkList inkList = new InkList();
			foreach (KeyValuePair<InkListItem, int> keyValuePair in this)
			{
				if (otherList.ContainsKey(keyValuePair.Key))
				{
					inkList.Add(keyValuePair.Key, keyValuePair.Value);
				}
			}
			return inkList;
		}

		// Token: 0x060001B1 RID: 433 RVA: 0x00009880 File Offset: 0x00007A80
		public bool HasIntersection(InkList otherList)
		{
			foreach (KeyValuePair<InkListItem, int> keyValuePair in this)
			{
				if (otherList.ContainsKey(keyValuePair.Key))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060001B2 RID: 434 RVA: 0x000098E0 File Offset: 0x00007AE0
		public InkList Without(InkList listToRemove)
		{
			InkList inkList = new InkList(this);
			foreach (KeyValuePair<InkListItem, int> keyValuePair in listToRemove)
			{
				inkList.Remove(keyValuePair.Key);
			}
			return inkList;
		}

		// Token: 0x060001B3 RID: 435 RVA: 0x00009940 File Offset: 0x00007B40
		public bool Contains(InkList otherList)
		{
			if (otherList.Count == 0 || base.Count == 0)
			{
				return false;
			}
			foreach (KeyValuePair<InkListItem, int> keyValuePair in otherList)
			{
				if (!base.ContainsKey(keyValuePair.Key))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x060001B4 RID: 436 RVA: 0x000099B0 File Offset: 0x00007BB0
		public bool GreaterThan(InkList otherList)
		{
			return base.Count != 0 && (otherList.Count == 0 || this.minItem.Value > otherList.maxItem.Value);
		}

		// Token: 0x060001B5 RID: 437 RVA: 0x000099F0 File Offset: 0x00007BF0
		public bool GreaterThanOrEquals(InkList otherList)
		{
			return base.Count != 0 && (otherList.Count == 0 || (this.minItem.Value >= otherList.minItem.Value && this.maxItem.Value >= otherList.maxItem.Value));
		}

		// Token: 0x060001B6 RID: 438 RVA: 0x00009A54 File Offset: 0x00007C54
		public bool LessThan(InkList otherList)
		{
			return otherList.Count != 0 && (base.Count == 0 || this.maxItem.Value < otherList.minItem.Value);
		}

		// Token: 0x060001B7 RID: 439 RVA: 0x00009A94 File Offset: 0x00007C94
		public bool LessThanOrEquals(InkList otherList)
		{
			return otherList.Count != 0 && (base.Count == 0 || (this.maxItem.Value <= otherList.maxItem.Value && this.minItem.Value <= otherList.minItem.Value));
		}

		// Token: 0x060001B8 RID: 440 RVA: 0x00009AF6 File Offset: 0x00007CF6
		public InkList MaxAsList()
		{
			if (base.Count > 0)
			{
				return new InkList(this.maxItem);
			}
			return new InkList();
		}

		// Token: 0x060001B9 RID: 441 RVA: 0x00009B12 File Offset: 0x00007D12
		public InkList MinAsList()
		{
			if (base.Count > 0)
			{
				return new InkList(this.minItem);
			}
			return new InkList();
		}

		// Token: 0x060001BA RID: 442 RVA: 0x00009B30 File Offset: 0x00007D30
		public InkList ListWithSubRange(object minBound, object maxBound)
		{
			if (base.Count == 0)
			{
				return new InkList();
			}
			List<KeyValuePair<InkListItem, int>> orderedItems = this.orderedItems;
			int num = 0;
			int num2 = int.MaxValue;
			if (minBound is int)
			{
				num = (int)minBound;
			}
			else if (minBound is InkList && ((InkList)minBound).Count > 0)
			{
				num = ((InkList)minBound).minItem.Value;
			}
			if (maxBound is int)
			{
				num2 = (int)maxBound;
			}
			else if (minBound is InkList && ((InkList)minBound).Count > 0)
			{
				num2 = ((InkList)maxBound).maxItem.Value;
			}
			InkList inkList = new InkList();
			inkList.SetInitialOriginNames(this.originNames);
			foreach (KeyValuePair<InkListItem, int> keyValuePair in orderedItems)
			{
				if (keyValuePair.Value >= num && keyValuePair.Value <= num2)
				{
					inkList.Add(keyValuePair.Key, keyValuePair.Value);
				}
			}
			return inkList;
		}

		// Token: 0x060001BB RID: 443 RVA: 0x00009C48 File Offset: 0x00007E48
		public override bool Equals(object other)
		{
			InkList inkList = other as InkList;
			if (inkList == null)
			{
				return false;
			}
			if (inkList.Count != base.Count)
			{
				return false;
			}
			foreach (KeyValuePair<InkListItem, int> keyValuePair in this)
			{
				if (!inkList.ContainsKey(keyValuePair.Key))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x060001BC RID: 444 RVA: 0x00009CC4 File Offset: 0x00007EC4
		public override int GetHashCode()
		{
			int num = 0;
			foreach (KeyValuePair<InkListItem, int> keyValuePair in this)
			{
				num += keyValuePair.Key.GetHashCode();
			}
			return num;
		}

		// Token: 0x1700004F RID: 79
		// (get) Token: 0x060001BD RID: 445 RVA: 0x00009D28 File Offset: 0x00007F28
		private List<KeyValuePair<InkListItem, int>> orderedItems
		{
			get
			{
				List<KeyValuePair<InkListItem, int>> list = new List<KeyValuePair<InkListItem, int>>();
				list.AddRange(this);
				list.Sort(delegate(KeyValuePair<InkListItem, int> x, KeyValuePair<InkListItem, int> y)
				{
					if (x.Value == y.Value)
					{
						return x.Key.originName.CompareTo(y.Key.originName);
					}
					return x.Value.CompareTo(y.Value);
				});
				return list;
			}
		}

		// Token: 0x17000050 RID: 80
		// (get) Token: 0x060001BE RID: 446 RVA: 0x00009D5C File Offset: 0x00007F5C
		public InkListItem singleItem
		{
			get
			{
				using (Dictionary<InkListItem, int>.Enumerator enumerator = base.GetEnumerator())
				{
					if (enumerator.MoveNext())
					{
						KeyValuePair<InkListItem, int> keyValuePair = enumerator.Current;
						return keyValuePair.Key;
					}
				}
				return default(InkListItem);
			}
		}

		// Token: 0x060001BF RID: 447 RVA: 0x00009DB8 File Offset: 0x00007FB8
		public override string ToString()
		{
			List<KeyValuePair<InkListItem, int>> orderedItems = this.orderedItems;
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < orderedItems.Count; i++)
			{
				if (i > 0)
				{
					stringBuilder.Append(", ");
				}
				InkListItem key = orderedItems[i].Key;
				stringBuilder.Append(key.itemName);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x04000074 RID: 116
		public List<ListDefinition> origins;

		// Token: 0x04000075 RID: 117
		private List<string> _originNames;
	}
}
