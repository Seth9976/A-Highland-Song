using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace UnityEngine.UIElements.Experimental
{
	// Token: 0x02000386 RID: 902
	internal class TreeView : BaseVerticalCollectionView
	{
		// Token: 0x170006F0 RID: 1776
		// (get) Token: 0x06001D03 RID: 7427 RVA: 0x0008661D File Offset: 0x0008481D
		// (set) Token: 0x06001D04 RID: 7428 RVA: 0x00024BEA File Offset: 0x00022DEA
		public new IList itemsSource
		{
			get
			{
				return this.viewController.itemsSource;
			}
			internal set
			{
				base.GetOrCreateViewController().itemsSource = value;
			}
		}

		// Token: 0x06001D05 RID: 7429 RVA: 0x0008662C File Offset: 0x0008482C
		public void SetRootItems<T>(IList<TreeViewItemData<T>> rootItems)
		{
			DefaultTreeViewController<T> defaultTreeViewController = base.viewController as DefaultTreeViewController<T>;
			bool flag = defaultTreeViewController != null;
			if (flag)
			{
				defaultTreeViewController.SetRootItems(rootItems);
			}
			else
			{
				DefaultTreeViewController<T> defaultTreeViewController2 = new DefaultTreeViewController<T>();
				this.SetViewController(defaultTreeViewController2);
				defaultTreeViewController2.SetRootItems(rootItems);
			}
		}

		// Token: 0x06001D06 RID: 7430 RVA: 0x00086674 File Offset: 0x00084874
		public IEnumerable<int> GetRootIds()
		{
			return this.viewController.GetRootItemIds();
		}

		// Token: 0x06001D07 RID: 7431 RVA: 0x00086694 File Offset: 0x00084894
		public int GetTreeCount()
		{
			return this.viewController.GetTreeCount();
		}

		// Token: 0x170006F1 RID: 1777
		// (get) Token: 0x06001D08 RID: 7432 RVA: 0x000866B1 File Offset: 0x000848B1
		internal new TreeViewController viewController
		{
			get
			{
				return base.viewController as TreeViewController;
			}
		}

		// Token: 0x06001D09 RID: 7433 RVA: 0x000866BE File Offset: 0x000848BE
		private protected override void CreateVirtualizationController()
		{
			base.CreateVirtualizationController<ReusableTreeViewItem>();
		}

		// Token: 0x06001D0A RID: 7434 RVA: 0x000866C8 File Offset: 0x000848C8
		private protected override void CreateViewController()
		{
			this.SetViewController(new DefaultTreeViewController<object>());
		}

		// Token: 0x06001D0B RID: 7435 RVA: 0x000866D8 File Offset: 0x000848D8
		internal void SetViewController(TreeViewController controller)
		{
			bool flag = this.viewController != null;
			if (flag)
			{
				controller.itemIndexChanged -= new Action<int, int>(this.OnItemIndexChanged);
			}
			base.SetViewController(controller);
			base.RefreshItems();
			bool flag2 = controller != null;
			if (flag2)
			{
				controller.itemIndexChanged += new Action<int, int>(this.OnItemIndexChanged);
			}
		}

		// Token: 0x06001D0C RID: 7436 RVA: 0x000267BF File Offset: 0x000249BF
		private void OnItemIndexChanged(int srcIndex, int dstIndex)
		{
			base.RefreshItems();
		}

		// Token: 0x06001D0D RID: 7437 RVA: 0x00086734 File Offset: 0x00084934
		internal override ICollectionDragAndDropController CreateDragAndDropController()
		{
			return new TreeViewReorderableDragAndDropController(this);
		}

		// Token: 0x170006F2 RID: 1778
		// (get) Token: 0x06001D0E RID: 7438 RVA: 0x0008673C File Offset: 0x0008493C
		// (set) Token: 0x06001D0F RID: 7439 RVA: 0x00086744 File Offset: 0x00084944
		public bool autoExpand
		{
			get
			{
				return this.m_AutoExpand;
			}
			set
			{
				this.m_AutoExpand = value;
				TreeViewController viewController = this.viewController;
				if (viewController != null)
				{
					viewController.RegenerateWrappers();
				}
				base.RefreshItems();
			}
		}

		// Token: 0x170006F3 RID: 1779
		// (get) Token: 0x06001D10 RID: 7440 RVA: 0x00086767 File Offset: 0x00084967
		// (set) Token: 0x06001D11 RID: 7441 RVA: 0x0008676F File Offset: 0x0008496F
		internal List<int> expandedItemIds
		{
			get
			{
				return this.m_ExpandedItemIds;
			}
			set
			{
				this.m_ExpandedItemIds = value;
			}
		}

		// Token: 0x06001D12 RID: 7442 RVA: 0x00086778 File Offset: 0x00084978
		public TreeView()
		{
			this.m_ExpandedItemIds = new List<int>();
			base.name = TreeView.ussClassName;
			base.viewDataKey = TreeView.ussClassName;
			base.AddToClassList(TreeView.ussClassName);
			base.scrollView.contentContainer.RegisterCallback<KeyDownEvent>(new EventCallback<KeyDownEvent>(this.OnScrollViewKeyDown), TrickleDown.NoTrickleDown);
			base.RegisterCallback<MouseUpEvent>(new EventCallback<MouseUpEvent>(this.OnTreeViewMouseUp), TrickleDown.TrickleDown);
		}

		// Token: 0x06001D13 RID: 7443 RVA: 0x000867F0 File Offset: 0x000849F0
		public int GetIdForIndex(int index)
		{
			return this.viewController.GetIdForIndex(index);
		}

		// Token: 0x06001D14 RID: 7444 RVA: 0x00086810 File Offset: 0x00084A10
		public int GetParentIdForIndex(int index)
		{
			return this.viewController.GetParentId(this.GetIdForIndex(index));
		}

		// Token: 0x06001D15 RID: 7445 RVA: 0x00086834 File Offset: 0x00084A34
		public IEnumerable<int> GetChildrenIdsForIndex(int index)
		{
			return this.viewController.GetChildrenIdsByIndex(this.GetIdForIndex(index));
		}

		// Token: 0x06001D16 RID: 7446 RVA: 0x00086858 File Offset: 0x00084A58
		public T GetItemDataForIndex<T>(int index)
		{
			DefaultTreeViewController<T> defaultTreeViewController = this.viewController as DefaultTreeViewController<T>;
			bool flag = defaultTreeViewController != null;
			T t;
			if (flag)
			{
				t = defaultTreeViewController.GetDataForIndex(index);
			}
			else
			{
				TreeViewController viewController = this.viewController;
				object obj = ((viewController != null) ? viewController.GetItemForIndex(index) : null);
				Type type = ((obj != null) ? obj.GetType() : null);
				bool flag2 = type == typeof(T);
				if (!flag2)
				{
					bool flag3;
					if (type == null)
					{
						TreeViewController viewController2 = this.viewController;
						flag3 = ((viewController2 != null) ? viewController2.GetType().GetGenericTypeDefinition() : null) == typeof(DefaultTreeViewController<>);
					}
					else
					{
						flag3 = false;
					}
					bool flag4 = flag3;
					if (flag4)
					{
						type = this.viewController.GetType().GetGenericArguments()[0];
					}
					throw new ArgumentException(string.Format("Type parameter ({0}) differs from data source ({1}) and is not recognized by the controller.", typeof(T), type));
				}
				t = (T)((object)obj);
			}
			return t;
		}

		// Token: 0x06001D17 RID: 7447 RVA: 0x00086928 File Offset: 0x00084B28
		public T GetItemDataForId<T>(int id)
		{
			DefaultTreeViewController<T> defaultTreeViewController = this.viewController as DefaultTreeViewController<T>;
			bool flag = defaultTreeViewController != null;
			T t;
			if (flag)
			{
				t = defaultTreeViewController.GetDataForId(id);
			}
			else
			{
				TreeViewController viewController = this.viewController;
				object obj = ((viewController != null) ? viewController.GetItemForIndex(this.viewController.GetIndexForId(id)) : null);
				Type type = ((obj != null) ? obj.GetType() : null);
				bool flag2 = type == typeof(T);
				if (!flag2)
				{
					bool flag3;
					if (type == null)
					{
						TreeViewController viewController2 = this.viewController;
						flag3 = ((viewController2 != null) ? viewController2.GetType().GetGenericTypeDefinition() : null) == typeof(DefaultTreeViewController<>);
					}
					else
					{
						flag3 = false;
					}
					bool flag4 = flag3;
					if (flag4)
					{
						type = this.viewController.GetType().GetGenericArguments()[0];
					}
					throw new ArgumentException(string.Format("Type parameter ({0}) differs from data source ({1}) and is not recognized by the controller.", typeof(T), type));
				}
				t = (T)((object)obj);
			}
			return t;
		}

		// Token: 0x06001D18 RID: 7448 RVA: 0x00086A04 File Offset: 0x00084C04
		public void AddItem<T>(TreeViewItemData<T> item, int parentId = -1, int childIndex = -1)
		{
			DefaultTreeViewController<T> defaultTreeViewController = this.viewController as DefaultTreeViewController<T>;
			bool flag = defaultTreeViewController != null;
			if (flag)
			{
				defaultTreeViewController.AddItem(in item, parentId, childIndex);
				base.RefreshItems();
			}
			Type type = null;
			TreeViewController viewController = this.viewController;
			bool flag2 = ((viewController != null) ? viewController.GetType().GetGenericTypeDefinition() : null) == typeof(DefaultTreeViewController<>);
			if (flag2)
			{
				type = this.viewController.GetType().GetGenericArguments()[0];
			}
			throw new ArgumentException(string.Format("Type parameter ({0}) differs from data source ({1})and is not recognized by the controller.", typeof(T), type));
		}

		// Token: 0x06001D19 RID: 7449 RVA: 0x00086A94 File Offset: 0x00084C94
		public bool TryRemoveItem(int id)
		{
			bool flag = this.viewController.TryRemoveItem(id);
			bool flag2;
			if (flag)
			{
				base.RefreshItems();
				flag2 = true;
			}
			else
			{
				flag2 = false;
			}
			return flag2;
		}

		// Token: 0x06001D1A RID: 7450 RVA: 0x00086AC4 File Offset: 0x00084CC4
		internal override void OnViewDataReady()
		{
			base.OnViewDataReady();
			bool flag = this.viewController != null;
			if (flag)
			{
				this.viewController.RebuildTree();
				base.RefreshItems();
			}
		}

		// Token: 0x06001D1B RID: 7451 RVA: 0x00086AFC File Offset: 0x00084CFC
		private void OnScrollViewKeyDown(KeyDownEvent evt)
		{
			int selectedIndex = base.selectedIndex;
			bool flag = true;
			KeyCode keyCode = evt.keyCode;
			KeyCode keyCode2 = keyCode;
			if (keyCode2 != KeyCode.RightArrow)
			{
				if (keyCode2 != KeyCode.LeftArrow)
				{
					flag = false;
				}
				else
				{
					bool flag2 = evt.altKey || this.IsExpandedByIndex(selectedIndex);
					if (flag2)
					{
						this.CollapseItemByIndex(selectedIndex, evt.altKey);
					}
				}
			}
			else
			{
				bool flag3 = evt.altKey || !this.IsExpandedByIndex(selectedIndex);
				if (flag3)
				{
					this.ExpandItemByIndex(selectedIndex, evt.altKey);
				}
			}
			bool flag4 = flag;
			if (flag4)
			{
				evt.StopPropagation();
			}
		}

		// Token: 0x06001D1C RID: 7452 RVA: 0x00086B95 File Offset: 0x00084D95
		public void SetSelectionById(int id)
		{
			this.SetSelectionById(new int[] { id });
		}

		// Token: 0x06001D1D RID: 7453 RVA: 0x00086BA9 File Offset: 0x00084DA9
		public void SetSelectionById(IEnumerable<int> ids)
		{
			this.SetSelectionInternalById(ids, true);
		}

		// Token: 0x06001D1E RID: 7454 RVA: 0x00086BB5 File Offset: 0x00084DB5
		public void SetSelectionByIdWithoutNotify(IEnumerable<int> ids)
		{
			this.SetSelectionInternalById(ids, false);
		}

		// Token: 0x06001D1F RID: 7455 RVA: 0x00086BC4 File Offset: 0x00084DC4
		internal void SetSelectionInternalById(IEnumerable<int> ids, bool sendNotification)
		{
			bool flag = ids == null;
			if (!flag)
			{
				List<int> list = Enumerable.ToList<int>(Enumerable.Select<int, int>(ids, delegate(int id)
				{
					this.viewController.ExpandItem(id, false);
					return this.viewController.GetIndexForId(id);
				}));
				base.SetSelectionInternal(list, sendNotification);
			}
		}

		// Token: 0x06001D20 RID: 7456 RVA: 0x00086C00 File Offset: 0x00084E00
		internal void CopyExpandedStates(ITreeViewItem source, ITreeViewItem target)
		{
			bool flag = this.IsExpanded(source.id);
			if (flag)
			{
				this.ExpandItem(target.id, false);
				bool flag2 = source.children != null && Enumerable.Count<ITreeViewItem>(source.children) > 0;
				if (flag2)
				{
					bool flag3 = target.children == null || Enumerable.Count<ITreeViewItem>(source.children) != Enumerable.Count<ITreeViewItem>(target.children);
					if (flag3)
					{
						Debug.LogWarning("Source and target hierarchies are not the same");
					}
					else
					{
						for (int i = 0; i < Enumerable.Count<ITreeViewItem>(source.children); i++)
						{
							ITreeViewItem treeViewItem = Enumerable.ElementAt<ITreeViewItem>(source.children, i);
							ITreeViewItem treeViewItem2 = Enumerable.ElementAt<ITreeViewItem>(target.children, i);
							this.CopyExpandedStates(treeViewItem, treeViewItem2);
						}
					}
				}
			}
			else
			{
				this.CollapseItem(target.id, false);
			}
		}

		// Token: 0x06001D21 RID: 7457 RVA: 0x00086CE0 File Offset: 0x00084EE0
		public bool IsExpanded(int id)
		{
			return this.viewController.IsExpanded(id);
		}

		// Token: 0x06001D22 RID: 7458 RVA: 0x00086CFE File Offset: 0x00084EFE
		public void CollapseItem(int id, bool collapseAllChildren = false)
		{
			this.viewController.CollapseItem(id, collapseAllChildren);
			base.RefreshItems();
		}

		// Token: 0x06001D23 RID: 7459 RVA: 0x00086D16 File Offset: 0x00084F16
		public void ExpandItem(int id, bool expandAllChildren = false)
		{
			this.viewController.ExpandItem(id, expandAllChildren);
			base.RefreshItems();
		}

		// Token: 0x06001D24 RID: 7460 RVA: 0x00086D30 File Offset: 0x00084F30
		public void ExpandRootItems()
		{
			foreach (int num in this.viewController.GetRootItemIds())
			{
				this.viewController.ExpandItem(num, false);
			}
			base.RefreshItems();
		}

		// Token: 0x06001D25 RID: 7461 RVA: 0x00086D94 File Offset: 0x00084F94
		public void ExpandAll()
		{
			this.viewController.ExpandAll();
		}

		// Token: 0x06001D26 RID: 7462 RVA: 0x00086DA3 File Offset: 0x00084FA3
		public void CollapseAll()
		{
			this.viewController.CollapseAll();
		}

		// Token: 0x06001D27 RID: 7463 RVA: 0x00086DB2 File Offset: 0x00084FB2
		private void OnTreeViewMouseUp(MouseUpEvent evt)
		{
			base.scrollView.contentContainer.Focus();
		}

		// Token: 0x06001D28 RID: 7464 RVA: 0x00086DC8 File Offset: 0x00084FC8
		private bool IsExpandedByIndex(int index)
		{
			return this.viewController.IsExpandedByIndex(index);
		}

		// Token: 0x06001D29 RID: 7465 RVA: 0x00086DE8 File Offset: 0x00084FE8
		private void CollapseItemByIndex(int index, bool collapseAll)
		{
			bool flag = !this.viewController.HasChildrenByIndex(index);
			if (!flag)
			{
				this.viewController.CollapseItemByIndex(index, collapseAll);
				base.RefreshItems();
				base.SaveViewData();
			}
		}

		// Token: 0x06001D2A RID: 7466 RVA: 0x00086E28 File Offset: 0x00085028
		private void ExpandItemByIndex(int index, bool expandAll)
		{
			bool flag = !this.viewController.HasChildrenByIndex(index);
			if (!flag)
			{
				this.viewController.ExpandItemByIndex(index, expandAll, true);
				base.RefreshItems();
				base.SaveViewData();
			}
		}

		// Token: 0x04000E6C RID: 3692
		public new static readonly string ussClassName = "unity-tree-view";

		// Token: 0x04000E6D RID: 3693
		public new static readonly string itemUssClassName = TreeView.ussClassName + "__item";

		// Token: 0x04000E6E RID: 3694
		public static readonly string itemToggleUssClassName = TreeView.ussClassName + "__item-toggle";

		// Token: 0x04000E6F RID: 3695
		public static readonly string itemIndentsContainerUssClassName = TreeView.ussClassName + "__item-indents";

		// Token: 0x04000E70 RID: 3696
		public static readonly string itemIndentUssClassName = TreeView.ussClassName + "__item-indent";

		// Token: 0x04000E71 RID: 3697
		public static readonly string itemContentContainerUssClassName = TreeView.ussClassName + "__item-content";

		// Token: 0x04000E72 RID: 3698
		private bool m_AutoExpand;

		// Token: 0x04000E73 RID: 3699
		[SerializeField]
		private List<int> m_ExpandedItemIds;

		// Token: 0x02000387 RID: 903
		public new class UxmlFactory : UxmlFactory<TreeView, TreeView.UxmlTraits>
		{
		}

		// Token: 0x02000388 RID: 904
		public new class UxmlTraits : VisualElement.UxmlTraits
		{
			// Token: 0x170006F4 RID: 1780
			// (get) Token: 0x06001D2E RID: 7470 RVA: 0x00086F1C File Offset: 0x0008511C
			public override IEnumerable<UxmlChildElementDescription> uxmlChildElementsDescription
			{
				get
				{
					yield break;
				}
			}

			// Token: 0x06001D2F RID: 7471 RVA: 0x00086F3C File Offset: 0x0008513C
			public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
			{
				base.Init(ve, bag, cc);
				int num = 0;
				bool flag = this.m_FixedItemHeight.TryGetValueFromBag(bag, cc, ref num);
				if (flag)
				{
					((TreeView)ve).fixedItemHeight = (float)num;
				}
				((TreeView)ve).virtualizationMethod = this.m_VirtualizationMethod.GetValueFromBag(bag, cc);
				((TreeView)ve).autoExpand = this.m_AutoExpand.GetValueFromBag(bag, cc);
				((TreeView)ve).showBorder = this.m_ShowBorder.GetValueFromBag(bag, cc);
				((TreeView)ve).selectionType = this.m_SelectionType.GetValueFromBag(bag, cc);
				((TreeView)ve).showAlternatingRowBackgrounds = this.m_ShowAlternatingRowBackgrounds.GetValueFromBag(bag, cc);
			}

			// Token: 0x04000E74 RID: 3700
			private readonly UxmlIntAttributeDescription m_FixedItemHeight = new UxmlIntAttributeDescription
			{
				name = "fixed-item-height",
				obsoleteNames = new string[] { "item-height" },
				defaultValue = BaseVerticalCollectionView.s_DefaultItemHeight
			};

			// Token: 0x04000E75 RID: 3701
			private readonly UxmlEnumAttributeDescription<CollectionVirtualizationMethod> m_VirtualizationMethod = new UxmlEnumAttributeDescription<CollectionVirtualizationMethod>
			{
				name = "virtualization-method",
				defaultValue = CollectionVirtualizationMethod.FixedHeight
			};

			// Token: 0x04000E76 RID: 3702
			private readonly UxmlBoolAttributeDescription m_ShowBorder = new UxmlBoolAttributeDescription
			{
				name = "show-border",
				defaultValue = false
			};

			// Token: 0x04000E77 RID: 3703
			private readonly UxmlBoolAttributeDescription m_AutoExpand = new UxmlBoolAttributeDescription
			{
				name = "auto-expand",
				defaultValue = false
			};

			// Token: 0x04000E78 RID: 3704
			private readonly UxmlEnumAttributeDescription<SelectionType> m_SelectionType = new UxmlEnumAttributeDescription<SelectionType>
			{
				name = "selection-type",
				defaultValue = SelectionType.Single
			};

			// Token: 0x04000E79 RID: 3705
			private readonly UxmlEnumAttributeDescription<AlternatingRowBackground> m_ShowAlternatingRowBackgrounds = new UxmlEnumAttributeDescription<AlternatingRowBackground>
			{
				name = "show-alternating-row-backgrounds",
				defaultValue = AlternatingRowBackground.None
			};
		}
	}
}
