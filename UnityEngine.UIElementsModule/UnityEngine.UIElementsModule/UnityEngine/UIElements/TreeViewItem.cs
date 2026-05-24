using System;
using System.Collections.Generic;

namespace UnityEngine.UIElements
{
	// Token: 0x020001A1 RID: 417
	internal class TreeViewItem<T> : ITreeViewItem
	{
		// Token: 0x170002CF RID: 719
		// (get) Token: 0x06000D98 RID: 3480 RVA: 0x00037FE0 File Offset: 0x000361E0
		// (set) Token: 0x06000D99 RID: 3481 RVA: 0x00037FE8 File Offset: 0x000361E8
		public int id { get; private set; }

		// Token: 0x170002D0 RID: 720
		// (get) Token: 0x06000D9A RID: 3482 RVA: 0x00037FF1 File Offset: 0x000361F1
		public ITreeViewItem parent
		{
			get
			{
				return this.m_Parent;
			}
		}

		// Token: 0x170002D1 RID: 721
		// (get) Token: 0x06000D9B RID: 3483 RVA: 0x00037FFC File Offset: 0x000361FC
		public IEnumerable<ITreeViewItem> children
		{
			get
			{
				return this.m_Children;
			}
		}

		// Token: 0x170002D2 RID: 722
		// (get) Token: 0x06000D9C RID: 3484 RVA: 0x00038014 File Offset: 0x00036214
		public bool hasChildren
		{
			get
			{
				return this.m_Children != null && this.m_Children.Count > 0;
			}
		}

		// Token: 0x170002D3 RID: 723
		// (get) Token: 0x06000D9D RID: 3485 RVA: 0x0003803F File Offset: 0x0003623F
		// (set) Token: 0x06000D9E RID: 3486 RVA: 0x00038047 File Offset: 0x00036247
		public T data { get; private set; }

		// Token: 0x06000D9F RID: 3487 RVA: 0x00038050 File Offset: 0x00036250
		public TreeViewItem(int id, T data, List<TreeViewItem<T>> children = null)
		{
			this.id = id;
			this.data = data;
			bool flag = children != null;
			if (flag)
			{
				foreach (TreeViewItem<T> treeViewItem in children)
				{
					this.AddChild(treeViewItem);
				}
			}
		}

		// Token: 0x06000DA0 RID: 3488 RVA: 0x000380C4 File Offset: 0x000362C4
		public void AddChild(ITreeViewItem child)
		{
			TreeViewItem<T> treeViewItem = child as TreeViewItem<T>;
			bool flag = treeViewItem == null;
			if (!flag)
			{
				bool flag2 = this.m_Children == null;
				if (flag2)
				{
					this.m_Children = new List<ITreeViewItem>();
				}
				this.m_Children.Add(treeViewItem);
				treeViewItem.m_Parent = this;
			}
		}

		// Token: 0x06000DA1 RID: 3489 RVA: 0x00038110 File Offset: 0x00036310
		public void AddChildren(IList<ITreeViewItem> children)
		{
			foreach (ITreeViewItem treeViewItem in children)
			{
				this.AddChild(treeViewItem);
			}
		}

		// Token: 0x06000DA2 RID: 3490 RVA: 0x0003815C File Offset: 0x0003635C
		public void RemoveChild(ITreeViewItem child)
		{
			bool flag = this.m_Children == null;
			if (!flag)
			{
				TreeViewItem<T> treeViewItem = child as TreeViewItem<T>;
				bool flag2 = treeViewItem == null;
				if (!flag2)
				{
					this.m_Children.Remove(treeViewItem);
				}
			}
		}

		// Token: 0x04000643 RID: 1603
		internal TreeViewItem<T> m_Parent;

		// Token: 0x04000644 RID: 1604
		private List<ITreeViewItem> m_Children;
	}
}
