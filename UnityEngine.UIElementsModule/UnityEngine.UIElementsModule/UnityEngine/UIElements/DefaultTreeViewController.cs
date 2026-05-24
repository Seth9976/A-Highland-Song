using System;
using System.Collections.Generic;

namespace UnityEngine.UIElements
{
	// Token: 0x0200010A RID: 266
	internal sealed class DefaultTreeViewController<T> : TreeViewController
	{
		// Token: 0x06000861 RID: 2145 RVA: 0x0001F30E File Offset: 0x0001D50E
		public void SetRootItems(IList<TreeViewItemData<T>> items)
		{
			this.m_TreeData = new TreeData<T>(items);
			base.RebuildTree();
		}

		// Token: 0x06000862 RID: 2146 RVA: 0x0001F324 File Offset: 0x0001D524
		public void AddItem(in TreeViewItemData<T> item, int parentId, int childIndex)
		{
			this.m_TreeData.AddItem(item, parentId, childIndex);
			base.RebuildTree();
		}

		// Token: 0x06000863 RID: 2147 RVA: 0x0001F344 File Offset: 0x0001D544
		public override bool TryRemoveItem(int id)
		{
			bool flag = this.m_TreeData.TryRemove(id);
			bool flag2;
			if (flag)
			{
				base.RebuildTree();
				flag2 = true;
			}
			else
			{
				flag2 = false;
			}
			return flag2;
		}

		// Token: 0x06000864 RID: 2148 RVA: 0x0001F374 File Offset: 0x0001D574
		public T GetDataForId(int id)
		{
			return this.m_TreeData.GetDataForId(id).data;
		}

		// Token: 0x06000865 RID: 2149 RVA: 0x0001F39C File Offset: 0x0001D59C
		public T GetDataForIndex(int index)
		{
			int idForIndex = this.GetIdForIndex(index);
			return this.GetDataForId(idForIndex);
		}

		// Token: 0x06000866 RID: 2150 RVA: 0x0001F3C0 File Offset: 0x0001D5C0
		public override object GetItemForIndex(int index)
		{
			return this.GetDataForIndex(index);
		}

		// Token: 0x06000867 RID: 2151 RVA: 0x0001F3E0 File Offset: 0x0001D5E0
		public override int GetParentId(int id)
		{
			return this.m_TreeData.GetParentId(id);
		}

		// Token: 0x06000868 RID: 2152 RVA: 0x0001F400 File Offset: 0x0001D600
		public override bool HasChildren(int id)
		{
			return this.m_TreeData.GetDataForId(id).hasChildren;
		}

		// Token: 0x06000869 RID: 2153 RVA: 0x0001F426 File Offset: 0x0001D626
		private static IEnumerable<int> GetItemIds(IEnumerable<TreeViewItemData<T>> items)
		{
			bool flag = items == null;
			if (flag)
			{
				yield break;
			}
			foreach (TreeViewItemData<T> item in items)
			{
				yield return item.id;
				item = default(TreeViewItemData<T>);
			}
			IEnumerator<TreeViewItemData<T>> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x0600086A RID: 2154 RVA: 0x0001F438 File Offset: 0x0001D638
		public override IEnumerable<int> GetChildrenIds(int id)
		{
			return DefaultTreeViewController<T>.GetItemIds(this.m_TreeData.GetDataForId(id).children);
		}

		// Token: 0x0600086B RID: 2155 RVA: 0x0001F464 File Offset: 0x0001D664
		public override void Move(int id, int newParentId, int childIndex = -1)
		{
			bool flag = id == newParentId;
			if (!flag)
			{
				bool flag2 = this.IsChildOf(newParentId, id);
				if (!flag2)
				{
					this.m_TreeData.Move(id, newParentId, childIndex);
					base.RaiseItemIndexChanged(this.GetIndexForId(id), this.GetIndexForId(newParentId) + childIndex + 1);
				}
			}
		}

		// Token: 0x0600086C RID: 2156 RVA: 0x0001F4B4 File Offset: 0x0001D6B4
		private bool IsChildOf(int childId, int id)
		{
			return this.m_TreeData.GetDataForId(id).HasChildRecursive(childId);
		}

		// Token: 0x0600086D RID: 2157 RVA: 0x0001F4E4 File Offset: 0x0001D6E4
		public override IEnumerable<int> GetAllItemIds(IEnumerable<int> rootIds = null)
		{
			bool flag = rootIds == null;
			if (flag)
			{
				bool flag2 = this.m_TreeData.rootItemIds == null;
				if (flag2)
				{
					yield break;
				}
				rootIds = this.m_TreeData.rootItemIds;
			}
			IEnumerator<int> currentIterator = rootIds.GetEnumerator();
			for (;;)
			{
				bool hasNext = currentIterator.MoveNext();
				bool flag3 = !hasNext;
				if (flag3)
				{
					bool flag4 = this.m_IteratorStack.Count > 0;
					if (!flag4)
					{
						break;
					}
					currentIterator = this.m_IteratorStack.Pop();
				}
				else
				{
					int currentItemId = currentIterator.Current;
					yield return currentItemId;
					bool flag5 = this.HasChildren(currentItemId);
					if (flag5)
					{
						this.m_IteratorStack.Push(currentIterator);
						currentIterator = this.GetChildrenIds(currentItemId).GetEnumerator();
					}
				}
			}
			yield break;
		}

		// Token: 0x0400036B RID: 875
		private TreeData<T> m_TreeData;

		// Token: 0x0400036C RID: 876
		private Stack<IEnumerator<int>> m_IteratorStack = new Stack<IEnumerator<int>>();
	}
}
