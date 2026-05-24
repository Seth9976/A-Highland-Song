using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine.Pool;

namespace UnityEngine.UIElements
{
	// Token: 0x02000106 RID: 262
	internal class ListViewController : CollectionViewController
	{
		// Token: 0x14000013 RID: 19
		// (add) Token: 0x06000825 RID: 2085 RVA: 0x0001DF3C File Offset: 0x0001C13C
		// (remove) Token: 0x06000826 RID: 2086 RVA: 0x0001DF74 File Offset: 0x0001C174
		[field: DebuggerBrowsable(0)]
		public event Action itemsSourceSizeChanged;

		// Token: 0x14000014 RID: 20
		// (add) Token: 0x06000827 RID: 2087 RVA: 0x0001DFAC File Offset: 0x0001C1AC
		// (remove) Token: 0x06000828 RID: 2088 RVA: 0x0001DFE4 File Offset: 0x0001C1E4
		[field: DebuggerBrowsable(0)]
		public event Action<IEnumerable<int>> itemsAdded;

		// Token: 0x14000015 RID: 21
		// (add) Token: 0x06000829 RID: 2089 RVA: 0x0001E01C File Offset: 0x0001C21C
		// (remove) Token: 0x0600082A RID: 2090 RVA: 0x0001E054 File Offset: 0x0001C254
		[field: DebuggerBrowsable(0)]
		public event Action<IEnumerable<int>> itemsRemoved;

		// Token: 0x170001A0 RID: 416
		// (get) Token: 0x0600082B RID: 2091 RVA: 0x0001E089 File Offset: 0x0001C289
		private ListView listView
		{
			get
			{
				return base.view as ListView;
			}
		}

		// Token: 0x0600082C RID: 2092 RVA: 0x0001E098 File Offset: 0x0001C298
		internal override void InvokeMakeItem(ReusableCollectionItem reusableItem)
		{
			ReusableListViewItem reusableListViewItem = reusableItem as ReusableListViewItem;
			bool flag = reusableListViewItem != null;
			if (flag)
			{
				reusableListViewItem.Init(this.MakeItem(), this.listView.reorderable && this.listView.reorderMode == ListViewReorderMode.Animated);
				reusableListViewItem.bindableElement.style.position = Position.Relative;
				reusableListViewItem.bindableElement.style.flexBasis = StyleKeyword.Initial;
				reusableListViewItem.bindableElement.style.marginTop = 0f;
				reusableListViewItem.bindableElement.style.marginBottom = 0f;
				reusableListViewItem.bindableElement.style.flexGrow = 0f;
				reusableListViewItem.bindableElement.style.flexShrink = 0f;
			}
		}

		// Token: 0x0600082D RID: 2093 RVA: 0x0001E180 File Offset: 0x0001C380
		internal override void InvokeBindItem(ReusableCollectionItem reusableItem, int index)
		{
			ReusableListViewItem reusableListViewItem = reusableItem as ReusableListViewItem;
			bool flag = reusableListViewItem != null;
			if (flag)
			{
				bool flag2 = this.listView.reorderable && this.listView.reorderMode == ListViewReorderMode.Animated;
				reusableListViewItem.UpdateDragHandle(flag2 && this.NeedsDragHandle(index));
			}
			base.InvokeBindItem(reusableItem, index);
		}

		// Token: 0x0600082E RID: 2094 RVA: 0x0001E1DC File Offset: 0x0001C3DC
		public virtual bool NeedsDragHandle(int index)
		{
			return !this.listView.sourceIncludesArraySize || index != 0;
		}

		// Token: 0x0600082F RID: 2095 RVA: 0x0001E204 File Offset: 0x0001C404
		public virtual void AddItems(int itemCount)
		{
			this.EnsureItemSourceCanBeResized();
			int count = base.itemsSource.Count;
			List<int> list = CollectionPool<List<int>, int>.Get();
			try
			{
				bool isFixedSize = base.itemsSource.IsFixedSize;
				if (isFixedSize)
				{
					base.itemsSource = ListViewController.AddToArray((Array)base.itemsSource, itemCount);
					for (int i = 0; i < itemCount; i++)
					{
						list.Add(count + i);
					}
				}
				else
				{
					for (int j = 0; j < itemCount; j++)
					{
						list.Add(count + j);
						base.itemsSource.Add(null);
					}
				}
				this.RaiseItemsAdded(list);
			}
			finally
			{
				CollectionPool<List<int>, int>.Release(list);
			}
			this.RaiseOnSizeChanged();
			bool isFixedSize2 = base.itemsSource.IsFixedSize;
			if (isFixedSize2)
			{
				this.listView.Rebuild();
			}
		}

		// Token: 0x06000830 RID: 2096 RVA: 0x0001E2F0 File Offset: 0x0001C4F0
		public virtual void Move(int index, int newIndex)
		{
			int num = newIndex;
			int num2 = ((newIndex < index) ? 1 : (-1));
			while (Mathf.Min(index, newIndex) < Mathf.Max(index, newIndex))
			{
				this.Swap(index, newIndex);
				newIndex += num2;
			}
			base.RaiseItemIndexChanged(index, num);
		}

		// Token: 0x06000831 RID: 2097 RVA: 0x0001E338 File Offset: 0x0001C538
		public virtual void RemoveItem(int index)
		{
			List<int> list = CollectionPool<List<int>, int>.Get();
			try
			{
				list.Add(index);
				this.RemoveItems(list);
			}
			finally
			{
				CollectionPool<List<int>, int>.Release(list);
			}
		}

		// Token: 0x06000832 RID: 2098 RVA: 0x0001E37C File Offset: 0x0001C57C
		public virtual void RemoveItems(List<int> indices)
		{
			this.EnsureItemSourceCanBeResized();
			indices.Sort();
			bool isFixedSize = base.itemsSource.IsFixedSize;
			if (isFixedSize)
			{
				base.itemsSource = ListViewController.RemoveFromArray((Array)base.itemsSource, indices);
			}
			else
			{
				for (int i = indices.Count - 1; i >= 0; i--)
				{
					base.itemsSource.RemoveAt(indices[i]);
				}
			}
			this.RaiseItemsRemoved(indices);
			this.RaiseOnSizeChanged();
		}

		// Token: 0x06000833 RID: 2099 RVA: 0x0001E403 File Offset: 0x0001C603
		protected void RaiseOnSizeChanged()
		{
			Action action = this.itemsSourceSizeChanged;
			if (action != null)
			{
				action.Invoke();
			}
		}

		// Token: 0x06000834 RID: 2100 RVA: 0x0001E418 File Offset: 0x0001C618
		protected void RaiseItemsAdded(IEnumerable<int> indices)
		{
			Action<IEnumerable<int>> action = this.itemsAdded;
			if (action != null)
			{
				action.Invoke(indices);
			}
		}

		// Token: 0x06000835 RID: 2101 RVA: 0x0001E42E File Offset: 0x0001C62E
		protected void RaiseItemsRemoved(IEnumerable<int> indices)
		{
			Action<IEnumerable<int>> action = this.itemsRemoved;
			if (action != null)
			{
				action.Invoke(indices);
			}
		}

		// Token: 0x06000836 RID: 2102 RVA: 0x0001E444 File Offset: 0x0001C644
		private static Array AddToArray(Array source, int itemCount)
		{
			Type elementType = source.GetType().GetElementType();
			bool flag = elementType == null;
			if (flag)
			{
				throw new InvalidOperationException("Cannot resize source, because its size is fixed.");
			}
			Array array = Array.CreateInstance(elementType, source.Length + itemCount);
			Array.Copy(source, array, source.Length);
			return array;
		}

		// Token: 0x06000837 RID: 2103 RVA: 0x0001E494 File Offset: 0x0001C694
		private static Array RemoveFromArray(Array source, List<int> indicesToRemove)
		{
			int length = source.Length;
			int num = length - indicesToRemove.Count;
			bool flag = num < 0;
			if (flag)
			{
				throw new InvalidOperationException("Cannot remove more items than the current count from source.");
			}
			Type elementType = source.GetType().GetElementType();
			bool flag2 = num == 0;
			Array array;
			if (flag2)
			{
				array = Array.CreateInstance(elementType, 0);
			}
			else
			{
				Array array2 = Array.CreateInstance(elementType, num);
				int num2 = 0;
				int num3 = 0;
				for (int i = 0; i < source.Length; i++)
				{
					bool flag3 = num3 < indicesToRemove.Count && indicesToRemove[num3] == i;
					if (flag3)
					{
						num3++;
					}
					else
					{
						array2.SetValue(source.GetValue(i), num2);
						num2++;
					}
				}
				array = array2;
			}
			return array;
		}

		// Token: 0x06000838 RID: 2104 RVA: 0x0001E55C File Offset: 0x0001C75C
		private void Swap(int lhs, int rhs)
		{
			object obj = base.itemsSource[lhs];
			base.itemsSource[lhs] = base.itemsSource[rhs];
			base.itemsSource[rhs] = obj;
		}

		// Token: 0x06000839 RID: 2105 RVA: 0x0001E5A0 File Offset: 0x0001C7A0
		private void EnsureItemSourceCanBeResized()
		{
			bool flag = base.itemsSource.IsFixedSize && !base.itemsSource.GetType().IsArray;
			if (flag)
			{
				throw new InvalidOperationException("Cannot add or remove items from source, because its size is fixed.");
			}
		}
	}
}
