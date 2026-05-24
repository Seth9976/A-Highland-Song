using System;
using System.Collections.Generic;

namespace UnityEngine.UIElements
{
	// Token: 0x020000D4 RID: 212
	public struct UQueryBuilder<T> : IEquatable<UQueryBuilder<T>> where T : VisualElement
	{
		// Token: 0x1700017B RID: 379
		// (get) Token: 0x060006E5 RID: 1765 RVA: 0x000190EC File Offset: 0x000172EC
		private List<StyleSelector> styleSelectors
		{
			get
			{
				List<StyleSelector> list;
				if ((list = this.m_StyleSelectors) == null)
				{
					list = (this.m_StyleSelectors = new List<StyleSelector>());
				}
				return list;
			}
		}

		// Token: 0x1700017C RID: 380
		// (get) Token: 0x060006E6 RID: 1766 RVA: 0x00019118 File Offset: 0x00017318
		private List<StyleSelectorPart> parts
		{
			get
			{
				List<StyleSelectorPart> list;
				if ((list = this.m_Parts) == null)
				{
					list = (this.m_Parts = new List<StyleSelectorPart>());
				}
				return list;
			}
		}

		// Token: 0x060006E7 RID: 1767 RVA: 0x00019144 File Offset: 0x00017344
		public UQueryBuilder(VisualElement visualElement)
		{
			this = default(UQueryBuilder<T>);
			this.m_Element = visualElement;
			this.m_Parts = null;
			this.m_StyleSelectors = null;
			this.m_Relationship = StyleSelectorRelationship.None;
			this.m_Matchers = new List<RuleMatcher>();
			this.pseudoStatesMask = (this.negatedPseudoStatesMask = 0);
		}

		// Token: 0x060006E8 RID: 1768 RVA: 0x00019190 File Offset: 0x00017390
		public UQueryBuilder<T> Class(string classname)
		{
			this.AddClass(classname);
			return this;
		}

		// Token: 0x060006E9 RID: 1769 RVA: 0x000191B0 File Offset: 0x000173B0
		public UQueryBuilder<T> Name(string id)
		{
			this.AddName(id);
			return this;
		}

		// Token: 0x060006EA RID: 1770 RVA: 0x000191D0 File Offset: 0x000173D0
		public UQueryBuilder<T2> Descendents<T2>(string name = null, params string[] classNames) where T2 : VisualElement
		{
			this.FinishCurrentSelector();
			this.AddType<T2>();
			this.AddName(name);
			this.AddClasses(classNames);
			return this.AddRelationship<T2>(StyleSelectorRelationship.Descendent);
		}

		// Token: 0x060006EB RID: 1771 RVA: 0x00019208 File Offset: 0x00017408
		public UQueryBuilder<T2> Descendents<T2>(string name = null, string classname = null) where T2 : VisualElement
		{
			this.FinishCurrentSelector();
			this.AddType<T2>();
			this.AddName(name);
			this.AddClass(classname);
			return this.AddRelationship<T2>(StyleSelectorRelationship.Descendent);
		}

		// Token: 0x060006EC RID: 1772 RVA: 0x00019240 File Offset: 0x00017440
		public UQueryBuilder<T2> Children<T2>(string name = null, params string[] classes) where T2 : VisualElement
		{
			this.FinishCurrentSelector();
			this.AddType<T2>();
			this.AddName(name);
			this.AddClasses(classes);
			return this.AddRelationship<T2>(StyleSelectorRelationship.Child);
		}

		// Token: 0x060006ED RID: 1773 RVA: 0x00019278 File Offset: 0x00017478
		public UQueryBuilder<T2> Children<T2>(string name = null, string className = null) where T2 : VisualElement
		{
			this.FinishCurrentSelector();
			this.AddType<T2>();
			this.AddName(name);
			this.AddClass(className);
			return this.AddRelationship<T2>(StyleSelectorRelationship.Child);
		}

		// Token: 0x060006EE RID: 1774 RVA: 0x000192B0 File Offset: 0x000174B0
		public UQueryBuilder<T2> OfType<T2>(string name = null, params string[] classes) where T2 : VisualElement
		{
			this.AddType<T2>();
			this.AddName(name);
			this.AddClasses(classes);
			return this.AddRelationship<T2>(StyleSelectorRelationship.None);
		}

		// Token: 0x060006EF RID: 1775 RVA: 0x000192E0 File Offset: 0x000174E0
		public UQueryBuilder<T2> OfType<T2>(string name = null, string className = null) where T2 : VisualElement
		{
			this.AddType<T2>();
			this.AddName(name);
			this.AddClass(className);
			return this.AddRelationship<T2>(StyleSelectorRelationship.None);
		}

		// Token: 0x060006F0 RID: 1776 RVA: 0x00019310 File Offset: 0x00017510
		internal UQueryBuilder<T> SingleBaseType()
		{
			this.parts.Add(StyleSelectorPart.CreatePredicate(UQuery.IsOfType<T>.s_Instance));
			return this;
		}

		// Token: 0x060006F1 RID: 1777 RVA: 0x00019340 File Offset: 0x00017540
		public UQueryBuilder<T> Where(Func<T, bool> selectorPredicate)
		{
			this.parts.Add(StyleSelectorPart.CreatePredicate(new UQuery.PredicateWrapper<T>(selectorPredicate)));
			return this;
		}

		// Token: 0x060006F2 RID: 1778 RVA: 0x00019370 File Offset: 0x00017570
		private void AddClass(string c)
		{
			bool flag = c != null;
			if (flag)
			{
				this.parts.Add(StyleSelectorPart.CreateClass(c));
			}
		}

		// Token: 0x060006F3 RID: 1779 RVA: 0x00019398 File Offset: 0x00017598
		private void AddClasses(params string[] classes)
		{
			bool flag = classes != null;
			if (flag)
			{
				for (int i = 0; i < classes.Length; i++)
				{
					this.AddClass(classes[i]);
				}
			}
		}

		// Token: 0x060006F4 RID: 1780 RVA: 0x000193CC File Offset: 0x000175CC
		private void AddName(string id)
		{
			bool flag = id != null;
			if (flag)
			{
				this.parts.Add(StyleSelectorPart.CreateId(id));
			}
		}

		// Token: 0x060006F5 RID: 1781 RVA: 0x000193F4 File Offset: 0x000175F4
		private void AddType<T2>() where T2 : VisualElement
		{
			bool flag = typeof(T2) != typeof(VisualElement);
			if (flag)
			{
				this.parts.Add(StyleSelectorPart.CreatePredicate(UQuery.IsOfType<T2>.s_Instance));
			}
		}

		// Token: 0x060006F6 RID: 1782 RVA: 0x00019438 File Offset: 0x00017638
		private UQueryBuilder<T> AddPseudoState(PseudoStates s)
		{
			this.pseudoStatesMask |= (int)s;
			return this;
		}

		// Token: 0x060006F7 RID: 1783 RVA: 0x00019460 File Offset: 0x00017660
		private UQueryBuilder<T> AddNegativePseudoState(PseudoStates s)
		{
			this.negatedPseudoStatesMask |= (int)s;
			return this;
		}

		// Token: 0x060006F8 RID: 1784 RVA: 0x00019488 File Offset: 0x00017688
		public UQueryBuilder<T> Active()
		{
			return this.AddPseudoState(PseudoStates.Active);
		}

		// Token: 0x060006F9 RID: 1785 RVA: 0x000194A4 File Offset: 0x000176A4
		public UQueryBuilder<T> NotActive()
		{
			return this.AddNegativePseudoState(PseudoStates.Active);
		}

		// Token: 0x060006FA RID: 1786 RVA: 0x000194C0 File Offset: 0x000176C0
		public UQueryBuilder<T> Visible()
		{
			return this.Where((T e) => e.visible);
		}

		// Token: 0x060006FB RID: 1787 RVA: 0x000194F8 File Offset: 0x000176F8
		public UQueryBuilder<T> NotVisible()
		{
			return this.Where((T e) => !e.visible);
		}

		// Token: 0x060006FC RID: 1788 RVA: 0x00019530 File Offset: 0x00017730
		public UQueryBuilder<T> Hovered()
		{
			return this.AddPseudoState(PseudoStates.Hover);
		}

		// Token: 0x060006FD RID: 1789 RVA: 0x0001954C File Offset: 0x0001774C
		public UQueryBuilder<T> NotHovered()
		{
			return this.AddNegativePseudoState(PseudoStates.Hover);
		}

		// Token: 0x060006FE RID: 1790 RVA: 0x00019568 File Offset: 0x00017768
		public UQueryBuilder<T> Checked()
		{
			return this.AddPseudoState(PseudoStates.Checked);
		}

		// Token: 0x060006FF RID: 1791 RVA: 0x00019584 File Offset: 0x00017784
		public UQueryBuilder<T> NotChecked()
		{
			return this.AddNegativePseudoState(PseudoStates.Checked);
		}

		// Token: 0x06000700 RID: 1792 RVA: 0x000195A0 File Offset: 0x000177A0
		[Obsolete("Use Checked() instead")]
		public UQueryBuilder<T> Selected()
		{
			return this.AddPseudoState(PseudoStates.Checked);
		}

		// Token: 0x06000701 RID: 1793 RVA: 0x000195BC File Offset: 0x000177BC
		[Obsolete("Use NotChecked() instead")]
		public UQueryBuilder<T> NotSelected()
		{
			return this.AddNegativePseudoState(PseudoStates.Checked);
		}

		// Token: 0x06000702 RID: 1794 RVA: 0x000195D8 File Offset: 0x000177D8
		public UQueryBuilder<T> Enabled()
		{
			return this.AddNegativePseudoState(PseudoStates.Disabled);
		}

		// Token: 0x06000703 RID: 1795 RVA: 0x000195F4 File Offset: 0x000177F4
		public UQueryBuilder<T> NotEnabled()
		{
			return this.AddPseudoState(PseudoStates.Disabled);
		}

		// Token: 0x06000704 RID: 1796 RVA: 0x00019610 File Offset: 0x00017810
		public UQueryBuilder<T> Focused()
		{
			return this.AddPseudoState(PseudoStates.Focus);
		}

		// Token: 0x06000705 RID: 1797 RVA: 0x0001962C File Offset: 0x0001782C
		public UQueryBuilder<T> NotFocused()
		{
			return this.AddNegativePseudoState(PseudoStates.Focus);
		}

		// Token: 0x06000706 RID: 1798 RVA: 0x00019648 File Offset: 0x00017848
		private UQueryBuilder<T2> AddRelationship<T2>(StyleSelectorRelationship relationship) where T2 : VisualElement
		{
			return new UQueryBuilder<T2>(this.m_Element)
			{
				m_Matchers = this.m_Matchers,
				m_Parts = this.m_Parts,
				m_StyleSelectors = this.m_StyleSelectors,
				m_Relationship = ((relationship == StyleSelectorRelationship.None) ? this.m_Relationship : relationship),
				pseudoStatesMask = this.pseudoStatesMask,
				negatedPseudoStatesMask = this.negatedPseudoStatesMask
			};
		}

		// Token: 0x06000707 RID: 1799 RVA: 0x000196BC File Offset: 0x000178BC
		private void AddPseudoStatesRuleIfNecessasy()
		{
			bool flag = this.pseudoStatesMask != 0 || this.negatedPseudoStatesMask != 0;
			if (flag)
			{
				this.parts.Add(new StyleSelectorPart
				{
					type = StyleSelectorType.PseudoClass
				});
			}
		}

		// Token: 0x06000708 RID: 1800 RVA: 0x00019704 File Offset: 0x00017904
		private void FinishSelector()
		{
			this.FinishCurrentSelector();
			bool flag = this.styleSelectors.Count > 0;
			if (flag)
			{
				StyleComplexSelector styleComplexSelector = new StyleComplexSelector();
				styleComplexSelector.selectors = this.styleSelectors.ToArray();
				this.styleSelectors.Clear();
				this.m_Matchers.Add(new RuleMatcher
				{
					complexSelector = styleComplexSelector
				});
			}
		}

		// Token: 0x06000709 RID: 1801 RVA: 0x00019770 File Offset: 0x00017970
		private bool CurrentSelectorEmpty()
		{
			return this.parts.Count == 0 && this.m_Relationship == StyleSelectorRelationship.None && this.pseudoStatesMask == 0 && this.negatedPseudoStatesMask == 0;
		}

		// Token: 0x0600070A RID: 1802 RVA: 0x000197AC File Offset: 0x000179AC
		private void FinishCurrentSelector()
		{
			bool flag = !this.CurrentSelectorEmpty();
			if (flag)
			{
				StyleSelector styleSelector = new StyleSelector();
				styleSelector.previousRelationship = this.m_Relationship;
				this.AddPseudoStatesRuleIfNecessasy();
				styleSelector.parts = this.m_Parts.ToArray();
				styleSelector.pseudoStateMask = this.pseudoStatesMask;
				styleSelector.negatedPseudoStateMask = this.negatedPseudoStatesMask;
				this.styleSelectors.Add(styleSelector);
				this.m_Parts.Clear();
				this.pseudoStatesMask = (this.negatedPseudoStatesMask = 0);
			}
		}

		// Token: 0x0600070B RID: 1803 RVA: 0x00019838 File Offset: 0x00017A38
		public UQueryState<T> Build()
		{
			this.FinishSelector();
			bool flag = this.m_Matchers.Count == 0;
			if (flag)
			{
				this.parts.Add(new StyleSelectorPart
				{
					type = StyleSelectorType.Wildcard
				});
				this.FinishSelector();
			}
			return new UQueryState<T>(this.m_Element, this.m_Matchers);
		}

		// Token: 0x0600070C RID: 1804 RVA: 0x0001989C File Offset: 0x00017A9C
		public static implicit operator T(UQueryBuilder<T> s)
		{
			return s.First();
		}

		// Token: 0x0600070D RID: 1805 RVA: 0x000198B8 File Offset: 0x00017AB8
		public static bool operator ==(UQueryBuilder<T> builder1, UQueryBuilder<T> builder2)
		{
			return builder1.Equals(builder2);
		}

		// Token: 0x0600070E RID: 1806 RVA: 0x000198D4 File Offset: 0x00017AD4
		public static bool operator !=(UQueryBuilder<T> builder1, UQueryBuilder<T> builder2)
		{
			return !(builder1 == builder2);
		}

		// Token: 0x0600070F RID: 1807 RVA: 0x000198F0 File Offset: 0x00017AF0
		public T First()
		{
			return this.Build().First();
		}

		// Token: 0x06000710 RID: 1808 RVA: 0x00019910 File Offset: 0x00017B10
		public T Last()
		{
			return this.Build().Last();
		}

		// Token: 0x06000711 RID: 1809 RVA: 0x00019930 File Offset: 0x00017B30
		public List<T> ToList()
		{
			return this.Build().ToList();
		}

		// Token: 0x06000712 RID: 1810 RVA: 0x00019950 File Offset: 0x00017B50
		public void ToList(List<T> results)
		{
			this.Build().ToList(results);
		}

		// Token: 0x06000713 RID: 1811 RVA: 0x00019970 File Offset: 0x00017B70
		public T AtIndex(int index)
		{
			return this.Build().AtIndex(index);
		}

		// Token: 0x06000714 RID: 1812 RVA: 0x00019994 File Offset: 0x00017B94
		public void ForEach<T2>(List<T2> result, Func<T, T2> funcCall)
		{
			this.Build().ForEach<T2>(result, funcCall);
		}

		// Token: 0x06000715 RID: 1813 RVA: 0x000199B4 File Offset: 0x00017BB4
		public List<T2> ForEach<T2>(Func<T, T2> funcCall)
		{
			return this.Build().ForEach<T2>(funcCall);
		}

		// Token: 0x06000716 RID: 1814 RVA: 0x000199D8 File Offset: 0x00017BD8
		public void ForEach(Action<T> funcCall)
		{
			this.Build().ForEach(funcCall);
		}

		// Token: 0x06000717 RID: 1815 RVA: 0x000199F8 File Offset: 0x00017BF8
		public bool Equals(UQueryBuilder<T> other)
		{
			return EqualityComparer<List<StyleSelector>>.Default.Equals(this.m_StyleSelectors, other.m_StyleSelectors) && EqualityComparer<List<StyleSelector>>.Default.Equals(this.styleSelectors, other.styleSelectors) && EqualityComparer<List<StyleSelectorPart>>.Default.Equals(this.m_Parts, other.m_Parts) && EqualityComparer<List<StyleSelectorPart>>.Default.Equals(this.parts, other.parts) && this.m_Element == other.m_Element && EqualityComparer<List<RuleMatcher>>.Default.Equals(this.m_Matchers, other.m_Matchers) && this.m_Relationship == other.m_Relationship && this.pseudoStatesMask == other.pseudoStatesMask && this.negatedPseudoStatesMask == other.negatedPseudoStatesMask;
		}

		// Token: 0x06000718 RID: 1816 RVA: 0x00019AC8 File Offset: 0x00017CC8
		public override bool Equals(object obj)
		{
			bool flag = !(obj is UQueryBuilder<T>);
			return !flag && this.Equals((UQueryBuilder<T>)obj);
		}

		// Token: 0x06000719 RID: 1817 RVA: 0x00019AFC File Offset: 0x00017CFC
		public override int GetHashCode()
		{
			int num = -949812380;
			num = num * -1521134295 + EqualityComparer<List<StyleSelector>>.Default.GetHashCode(this.m_StyleSelectors);
			num = num * -1521134295 + EqualityComparer<List<StyleSelector>>.Default.GetHashCode(this.styleSelectors);
			num = num * -1521134295 + EqualityComparer<List<StyleSelectorPart>>.Default.GetHashCode(this.m_Parts);
			num = num * -1521134295 + EqualityComparer<List<StyleSelectorPart>>.Default.GetHashCode(this.parts);
			num = num * -1521134295 + EqualityComparer<VisualElement>.Default.GetHashCode(this.m_Element);
			num = num * -1521134295 + EqualityComparer<List<RuleMatcher>>.Default.GetHashCode(this.m_Matchers);
			num = num * -1521134295 + this.m_Relationship.GetHashCode();
			num = num * -1521134295 + this.pseudoStatesMask.GetHashCode();
			return num * -1521134295 + this.negatedPseudoStatesMask.GetHashCode();
		}

		// Token: 0x040002B0 RID: 688
		private List<StyleSelector> m_StyleSelectors;

		// Token: 0x040002B1 RID: 689
		private List<StyleSelectorPart> m_Parts;

		// Token: 0x040002B2 RID: 690
		private VisualElement m_Element;

		// Token: 0x040002B3 RID: 691
		private List<RuleMatcher> m_Matchers;

		// Token: 0x040002B4 RID: 692
		private StyleSelectorRelationship m_Relationship;

		// Token: 0x040002B5 RID: 693
		private int pseudoStatesMask;

		// Token: 0x040002B6 RID: 694
		private int negatedPseudoStatesMask;
	}
}
