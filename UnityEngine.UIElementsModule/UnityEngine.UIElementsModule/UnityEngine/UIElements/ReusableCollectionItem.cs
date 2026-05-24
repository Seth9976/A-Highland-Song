using System;
using System.Diagnostics;
using UnityEngine.UIElements.Experimental;

namespace UnityEngine.UIElements
{
	// Token: 0x02000110 RID: 272
	internal class ReusableCollectionItem
	{
		// Token: 0x170001AB RID: 427
		// (get) Token: 0x060008B0 RID: 2224 RVA: 0x00021397 File Offset: 0x0001F597
		public virtual VisualElement rootElement
		{
			get
			{
				return this.bindableElement;
			}
		}

		// Token: 0x170001AC RID: 428
		// (get) Token: 0x060008B1 RID: 2225 RVA: 0x0002139F File Offset: 0x0001F59F
		// (set) Token: 0x060008B2 RID: 2226 RVA: 0x000213A7 File Offset: 0x0001F5A7
		public VisualElement bindableElement { get; protected set; }

		// Token: 0x170001AD RID: 429
		// (get) Token: 0x060008B3 RID: 2227 RVA: 0x000213B0 File Offset: 0x0001F5B0
		// (set) Token: 0x060008B4 RID: 2228 RVA: 0x000213B8 File Offset: 0x0001F5B8
		public ValueAnimation<StyleValues> animator { get; set; }

		// Token: 0x170001AE RID: 430
		// (get) Token: 0x060008B5 RID: 2229 RVA: 0x000213C1 File Offset: 0x0001F5C1
		// (set) Token: 0x060008B6 RID: 2230 RVA: 0x000213C9 File Offset: 0x0001F5C9
		public int index { get; set; }

		// Token: 0x170001AF RID: 431
		// (get) Token: 0x060008B7 RID: 2231 RVA: 0x000213D2 File Offset: 0x0001F5D2
		// (set) Token: 0x060008B8 RID: 2232 RVA: 0x000213DA File Offset: 0x0001F5DA
		public int id { get; set; }

		// Token: 0x14000016 RID: 22
		// (add) Token: 0x060008B9 RID: 2233 RVA: 0x000213E4 File Offset: 0x0001F5E4
		// (remove) Token: 0x060008BA RID: 2234 RVA: 0x0002141C File Offset: 0x0001F61C
		[field: DebuggerBrowsable(0)]
		public event Action<ReusableCollectionItem> onGeometryChanged;

		// Token: 0x060008BB RID: 2235 RVA: 0x00021454 File Offset: 0x0001F654
		public ReusableCollectionItem()
		{
			this.index = (this.id = -1);
			this.m_GeometryChangedEventCallback = new EventCallback<GeometryChangedEvent>(this.OnGeometryChanged);
		}

		// Token: 0x060008BC RID: 2236 RVA: 0x0002148D File Offset: 0x0001F68D
		public virtual void Init(VisualElement item)
		{
			this.bindableElement = item;
		}

		// Token: 0x060008BD RID: 2237 RVA: 0x00021498 File Offset: 0x0001F698
		public virtual void PreAttachElement()
		{
			this.rootElement.AddToClassList(BaseVerticalCollectionView.itemUssClassName);
			this.rootElement.RegisterCallback<GeometryChangedEvent>(this.m_GeometryChangedEventCallback, TrickleDown.NoTrickleDown);
		}

		// Token: 0x060008BE RID: 2238 RVA: 0x000214C0 File Offset: 0x0001F6C0
		public virtual void DetachElement()
		{
			this.rootElement.RemoveFromClassList(BaseVerticalCollectionView.itemUssClassName);
			this.rootElement.UnregisterCallback<GeometryChangedEvent>(this.m_GeometryChangedEventCallback, TrickleDown.NoTrickleDown);
			VisualElement rootElement = this.rootElement;
			if (rootElement != null)
			{
				rootElement.RemoveFromHierarchy();
			}
			this.SetSelected(false);
			this.index = (this.id = -1);
		}

		// Token: 0x060008BF RID: 2239 RVA: 0x00021520 File Offset: 0x0001F720
		public virtual void SetSelected(bool selected)
		{
			if (selected)
			{
				this.rootElement.AddToClassList(BaseVerticalCollectionView.itemSelectedVariantUssClassName);
				this.rootElement.pseudoStates |= PseudoStates.Checked;
			}
			else
			{
				this.rootElement.RemoveFromClassList(BaseVerticalCollectionView.itemSelectedVariantUssClassName);
				this.rootElement.pseudoStates &= ~PseudoStates.Checked;
			}
		}

		// Token: 0x060008C0 RID: 2240 RVA: 0x00021584 File Offset: 0x0001F784
		protected void OnGeometryChanged(GeometryChangedEvent evt)
		{
			Action<ReusableCollectionItem> action = this.onGeometryChanged;
			if (action != null)
			{
				action.Invoke(this);
			}
		}

		// Token: 0x060008C1 RID: 2241 RVA: 0x0002159A File Offset: 0x0001F79A
		protected internal VisualElement GetRootElement()
		{
			return this.rootElement;
		}

		// Token: 0x0400038B RID: 907
		public const int UndefinedIndex = -1;

		// Token: 0x04000391 RID: 913
		protected EventCallback<GeometryChangedEvent> m_GeometryChangedEventCallback;
	}
}
