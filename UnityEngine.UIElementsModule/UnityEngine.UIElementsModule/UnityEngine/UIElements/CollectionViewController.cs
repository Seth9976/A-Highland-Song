using System;
using System.Collections;
using System.Diagnostics;
using System.Linq;
using UnityEngine.Assertions;

namespace UnityEngine.UIElements
{
	// Token: 0x02000105 RID: 261
	internal class CollectionViewController
	{
		// Token: 0x14000011 RID: 17
		// (add) Token: 0x0600080D RID: 2061 RVA: 0x0001DBA4 File Offset: 0x0001BDA4
		// (remove) Token: 0x0600080E RID: 2062 RVA: 0x0001DBDC File Offset: 0x0001BDDC
		[field: DebuggerBrowsable(0)]
		public event Action itemsSourceChanged;

		// Token: 0x14000012 RID: 18
		// (add) Token: 0x0600080F RID: 2063 RVA: 0x0001DC14 File Offset: 0x0001BE14
		// (remove) Token: 0x06000810 RID: 2064 RVA: 0x0001DC4C File Offset: 0x0001BE4C
		[field: DebuggerBrowsable(0)]
		public event Action<int, int> itemIndexChanged;

		// Token: 0x1700019E RID: 414
		// (get) Token: 0x06000811 RID: 2065 RVA: 0x0001DC81 File Offset: 0x0001BE81
		// (set) Token: 0x06000812 RID: 2066 RVA: 0x0001DC8C File Offset: 0x0001BE8C
		public IList itemsSource
		{
			get
			{
				return this.m_ItemsSource;
			}
			set
			{
				bool flag = this.m_ItemsSource == value;
				if (!flag)
				{
					this.m_ItemsSource = value;
					this.RaiseItemsSourceChanged();
				}
			}
		}

		// Token: 0x06000813 RID: 2067 RVA: 0x0001DCB7 File Offset: 0x0001BEB7
		protected void SetItemsSourceWithoutNotify(IList source)
		{
			this.m_ItemsSource = source;
		}

		// Token: 0x1700019F RID: 415
		// (get) Token: 0x06000814 RID: 2068 RVA: 0x0001DCC1 File Offset: 0x0001BEC1
		protected BaseVerticalCollectionView view
		{
			get
			{
				return this.m_View;
			}
		}

		// Token: 0x06000815 RID: 2069 RVA: 0x0001DCC9 File Offset: 0x0001BEC9
		public void SetView(BaseVerticalCollectionView view)
		{
			this.m_View = view;
			Assert.IsNotNull<BaseVerticalCollectionView>(this.m_View, "View must not be null.");
		}

		// Token: 0x06000816 RID: 2070 RVA: 0x0001DCE4 File Offset: 0x0001BEE4
		public virtual int GetItemCount()
		{
			IList itemsSource = this.m_ItemsSource;
			return (itemsSource != null) ? itemsSource.Count : 0;
		}

		// Token: 0x06000817 RID: 2071 RVA: 0x0001DD08 File Offset: 0x0001BF08
		public virtual int GetIndexForId(int id)
		{
			return id;
		}

		// Token: 0x06000818 RID: 2072 RVA: 0x0001DD1C File Offset: 0x0001BF1C
		public virtual int GetIdForIndex(int index)
		{
			Func<int, int> getItemId = this.m_View.getItemId;
			return (getItemId != null) ? getItemId.Invoke(index) : index;
		}

		// Token: 0x06000819 RID: 2073 RVA: 0x0001DD48 File Offset: 0x0001BF48
		public virtual object GetItemForIndex(int index)
		{
			bool flag = index < 0 || index >= this.m_ItemsSource.Count;
			object obj;
			if (flag)
			{
				obj = null;
			}
			else
			{
				obj = this.m_ItemsSource[index];
			}
			return obj;
		}

		// Token: 0x0600081A RID: 2074 RVA: 0x0001DD86 File Offset: 0x0001BF86
		internal virtual void InvokeMakeItem(ReusableCollectionItem reusableItem)
		{
			reusableItem.Init(this.MakeItem());
		}

		// Token: 0x0600081B RID: 2075 RVA: 0x0001DD96 File Offset: 0x0001BF96
		internal virtual void InvokeBindItem(ReusableCollectionItem reusableItem, int index)
		{
			this.BindItem(reusableItem.bindableElement, index);
			reusableItem.SetSelected(Enumerable.Contains<int>(this.m_View.selectedIndices, index));
			reusableItem.rootElement.pseudoStates &= ~PseudoStates.Hover;
		}

		// Token: 0x0600081C RID: 2076 RVA: 0x0001DDD4 File Offset: 0x0001BFD4
		internal virtual void InvokeUnbindItem(ReusableCollectionItem reusableItem, int index)
		{
			this.UnbindItem(reusableItem.bindableElement, index);
		}

		// Token: 0x0600081D RID: 2077 RVA: 0x0001DDE5 File Offset: 0x0001BFE5
		internal virtual void InvokeDestroyItem(ReusableCollectionItem reusableItem)
		{
			this.DestroyItem(reusableItem.bindableElement);
		}

		// Token: 0x0600081E RID: 2078 RVA: 0x0001DDF8 File Offset: 0x0001BFF8
		public virtual VisualElement MakeItem()
		{
			bool flag = this.m_View.makeItem == null;
			VisualElement visualElement;
			if (flag)
			{
				bool flag2 = this.m_View.bindItem != null;
				if (flag2)
				{
					throw new NotImplementedException("You must specify makeItem if bindItem is specified.");
				}
				visualElement = new Label();
			}
			else
			{
				visualElement = this.m_View.makeItem.Invoke();
			}
			return visualElement;
		}

		// Token: 0x0600081F RID: 2079 RVA: 0x0001DE54 File Offset: 0x0001C054
		public virtual void BindItem(VisualElement element, int index)
		{
			bool flag = this.m_View.bindItem == null;
			if (flag)
			{
				bool flag2 = this.m_View.makeItem != null;
				if (flag2)
				{
					throw new NotImplementedException("You must specify bindItem if makeItem is specified.");
				}
				Label label = (Label)element;
				object obj = this.m_ItemsSource[index];
				label.text = ((obj != null) ? obj.ToString() : null) ?? "null";
			}
			else
			{
				this.m_View.bindItem.Invoke(element, index);
			}
		}

		// Token: 0x06000820 RID: 2080 RVA: 0x0001DED7 File Offset: 0x0001C0D7
		public virtual void UnbindItem(VisualElement element, int index)
		{
			Action<VisualElement, int> unbindItem = this.m_View.unbindItem;
			if (unbindItem != null)
			{
				unbindItem.Invoke(element, index);
			}
		}

		// Token: 0x06000821 RID: 2081 RVA: 0x0001DEF3 File Offset: 0x0001C0F3
		public virtual void DestroyItem(VisualElement element)
		{
			Action<VisualElement> destroyItem = this.m_View.destroyItem;
			if (destroyItem != null)
			{
				destroyItem.Invoke(element);
			}
		}

		// Token: 0x06000822 RID: 2082 RVA: 0x0001DF0E File Offset: 0x0001C10E
		protected void RaiseItemsSourceChanged()
		{
			Action action = this.itemsSourceChanged;
			if (action != null)
			{
				action.Invoke();
			}
		}

		// Token: 0x06000823 RID: 2083 RVA: 0x0001DF23 File Offset: 0x0001C123
		protected void RaiseItemIndexChanged(int srcIndex, int dstIndex)
		{
			Action<int, int> action = this.itemIndexChanged;
			if (action != null)
			{
				action.Invoke(srcIndex, dstIndex);
			}
		}

		// Token: 0x0400035D RID: 861
		private BaseVerticalCollectionView m_View;

		// Token: 0x0400035E RID: 862
		private IList m_ItemsSource;
	}
}
