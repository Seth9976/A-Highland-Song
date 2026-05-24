using System;
using System.Collections;
using System.Collections.Generic;

namespace UnityEngine.UIElements
{
	// Token: 0x020000CF RID: 207
	public struct UQueryState<T> : IEnumerable<T>, IEnumerable, IEquatable<UQueryState<T>> where T : VisualElement
	{
		// Token: 0x060006BB RID: 1723 RVA: 0x00018BC8 File Offset: 0x00016DC8
		internal UQueryState(VisualElement element, List<RuleMatcher> matchers)
		{
			this.m_Element = element;
			this.m_Matchers = matchers;
		}

		// Token: 0x060006BC RID: 1724 RVA: 0x00018BDC File Offset: 0x00016DDC
		public UQueryState<T> RebuildOn(VisualElement element)
		{
			return new UQueryState<T>(element, this.m_Matchers);
		}

		// Token: 0x060006BD RID: 1725 RVA: 0x00018BFC File Offset: 0x00016DFC
		private T Single(UQuery.SingleQueryMatcher matcher)
		{
			bool flag = matcher.IsInUse();
			if (flag)
			{
				matcher = matcher.CreateNew();
			}
			matcher.Run(this.m_Element, this.m_Matchers);
			T t = matcher.match as T;
			matcher.match = null;
			return t;
		}

		// Token: 0x060006BE RID: 1726 RVA: 0x00018C4F File Offset: 0x00016E4F
		public T First()
		{
			return this.Single(UQuery.FirstQueryMatcher.Instance);
		}

		// Token: 0x060006BF RID: 1727 RVA: 0x00018C5C File Offset: 0x00016E5C
		public T Last()
		{
			return this.Single(UQuery.LastQueryMatcher.Instance);
		}

		// Token: 0x060006C0 RID: 1728 RVA: 0x00018C69 File Offset: 0x00016E69
		public void ToList(List<T> results)
		{
			UQueryState<T>.s_List.matches = results;
			UQueryState<T>.s_List.Run(this.m_Element, this.m_Matchers);
			UQueryState<T>.s_List.Reset();
		}

		// Token: 0x060006C1 RID: 1729 RVA: 0x00018C9C File Offset: 0x00016E9C
		public List<T> ToList()
		{
			List<T> list = new List<T>();
			this.ToList(list);
			return list;
		}

		// Token: 0x060006C2 RID: 1730 RVA: 0x00018CC0 File Offset: 0x00016EC0
		public T AtIndex(int index)
		{
			UQuery.IndexQueryMatcher instance = UQuery.IndexQueryMatcher.Instance;
			instance.matchIndex = index;
			return this.Single(instance);
		}

		// Token: 0x060006C3 RID: 1731 RVA: 0x00018CE8 File Offset: 0x00016EE8
		public void ForEach(Action<T> funcCall)
		{
			UQueryState<T>.ActionQueryMatcher actionQueryMatcher = UQueryState<T>.s_Action;
			bool flag = actionQueryMatcher.callBack != null;
			if (flag)
			{
				actionQueryMatcher = new UQueryState<T>.ActionQueryMatcher();
			}
			try
			{
				actionQueryMatcher.callBack = funcCall;
				actionQueryMatcher.Run(this.m_Element, this.m_Matchers);
			}
			finally
			{
				actionQueryMatcher.callBack = null;
			}
		}

		// Token: 0x060006C4 RID: 1732 RVA: 0x00018D4C File Offset: 0x00016F4C
		public void ForEach<T2>(List<T2> result, Func<T, T2> funcCall)
		{
			UQueryState<T>.DelegateQueryMatcher<T2> delegateQueryMatcher = UQueryState<T>.DelegateQueryMatcher<T2>.s_Instance;
			bool flag = delegateQueryMatcher.callBack != null;
			if (flag)
			{
				delegateQueryMatcher = new UQueryState<T>.DelegateQueryMatcher<T2>();
			}
			try
			{
				delegateQueryMatcher.callBack = funcCall;
				delegateQueryMatcher.result = result;
				delegateQueryMatcher.Run(this.m_Element, this.m_Matchers);
			}
			finally
			{
				delegateQueryMatcher.callBack = null;
				delegateQueryMatcher.result = null;
			}
		}

		// Token: 0x060006C5 RID: 1733 RVA: 0x00018DC0 File Offset: 0x00016FC0
		public List<T2> ForEach<T2>(Func<T, T2> funcCall)
		{
			List<T2> list = new List<T2>();
			this.ForEach<T2>(list, funcCall);
			return list;
		}

		// Token: 0x060006C6 RID: 1734 RVA: 0x00018DE2 File Offset: 0x00016FE2
		public UQueryState<T>.Enumerator GetEnumerator()
		{
			return new UQueryState<T>.Enumerator(this);
		}

		// Token: 0x060006C7 RID: 1735 RVA: 0x00018DEF File Offset: 0x00016FEF
		IEnumerator<T> IEnumerable<T>.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x060006C8 RID: 1736 RVA: 0x00018DEF File Offset: 0x00016FEF
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x060006C9 RID: 1737 RVA: 0x00018DFC File Offset: 0x00016FFC
		public bool Equals(UQueryState<T> other)
		{
			return this.m_Element == other.m_Element && EqualityComparer<List<RuleMatcher>>.Default.Equals(this.m_Matchers, other.m_Matchers);
		}

		// Token: 0x060006CA RID: 1738 RVA: 0x00018E38 File Offset: 0x00017038
		public override bool Equals(object obj)
		{
			bool flag = !(obj is UQueryState<T>);
			return !flag && this.Equals((UQueryState<T>)obj);
		}

		// Token: 0x060006CB RID: 1739 RVA: 0x00018E6C File Offset: 0x0001706C
		public override int GetHashCode()
		{
			int num = 488160421;
			num = num * -1521134295 + EqualityComparer<VisualElement>.Default.GetHashCode(this.m_Element);
			return num * -1521134295 + EqualityComparer<List<RuleMatcher>>.Default.GetHashCode(this.m_Matchers);
		}

		// Token: 0x060006CC RID: 1740 RVA: 0x00018EB8 File Offset: 0x000170B8
		public static bool operator ==(UQueryState<T> state1, UQueryState<T> state2)
		{
			return state1.Equals(state2);
		}

		// Token: 0x060006CD RID: 1741 RVA: 0x00018ED4 File Offset: 0x000170D4
		public static bool operator !=(UQueryState<T> state1, UQueryState<T> state2)
		{
			return !(state1 == state2);
		}

		// Token: 0x040002A4 RID: 676
		private static UQueryState<T>.ActionQueryMatcher s_Action = new UQueryState<T>.ActionQueryMatcher();

		// Token: 0x040002A5 RID: 677
		private readonly VisualElement m_Element;

		// Token: 0x040002A6 RID: 678
		internal readonly List<RuleMatcher> m_Matchers;

		// Token: 0x040002A7 RID: 679
		private static readonly UQueryState<T>.ListQueryMatcher<T> s_List = new UQueryState<T>.ListQueryMatcher<T>();

		// Token: 0x040002A8 RID: 680
		private static readonly UQueryState<T>.ListQueryMatcher<VisualElement> s_EnumerationList = new UQueryState<T>.ListQueryMatcher<VisualElement>();

		// Token: 0x020000D0 RID: 208
		private class ListQueryMatcher<TElement> : UQuery.UQueryMatcher where TElement : VisualElement
		{
			// Token: 0x17000175 RID: 373
			// (get) Token: 0x060006CF RID: 1743 RVA: 0x00018F10 File Offset: 0x00017110
			// (set) Token: 0x060006D0 RID: 1744 RVA: 0x00018F18 File Offset: 0x00017118
			public List<TElement> matches { get; set; }

			// Token: 0x060006D1 RID: 1745 RVA: 0x00018F24 File Offset: 0x00017124
			protected override bool OnRuleMatchedElement(RuleMatcher matcher, VisualElement element)
			{
				this.matches.Add(element as TElement);
				return false;
			}

			// Token: 0x060006D2 RID: 1746 RVA: 0x00018F4E File Offset: 0x0001714E
			public void Reset()
			{
				this.matches = null;
			}
		}

		// Token: 0x020000D1 RID: 209
		private class ActionQueryMatcher : UQuery.UQueryMatcher
		{
			// Token: 0x17000176 RID: 374
			// (get) Token: 0x060006D4 RID: 1748 RVA: 0x00018F59 File Offset: 0x00017159
			// (set) Token: 0x060006D5 RID: 1749 RVA: 0x00018F61 File Offset: 0x00017161
			internal Action<T> callBack { get; set; }

			// Token: 0x060006D6 RID: 1750 RVA: 0x00018F6C File Offset: 0x0001716C
			protected override bool OnRuleMatchedElement(RuleMatcher matcher, VisualElement element)
			{
				T t = element as T;
				bool flag = t != null;
				if (flag)
				{
					this.callBack.Invoke(t);
				}
				return false;
			}
		}

		// Token: 0x020000D2 RID: 210
		private class DelegateQueryMatcher<TReturnType> : UQuery.UQueryMatcher
		{
			// Token: 0x17000177 RID: 375
			// (get) Token: 0x060006D8 RID: 1752 RVA: 0x00018FA7 File Offset: 0x000171A7
			// (set) Token: 0x060006D9 RID: 1753 RVA: 0x00018FAF File Offset: 0x000171AF
			public Func<T, TReturnType> callBack { get; set; }

			// Token: 0x17000178 RID: 376
			// (get) Token: 0x060006DA RID: 1754 RVA: 0x00018FB8 File Offset: 0x000171B8
			// (set) Token: 0x060006DB RID: 1755 RVA: 0x00018FC0 File Offset: 0x000171C0
			public List<TReturnType> result { get; set; }

			// Token: 0x060006DC RID: 1756 RVA: 0x00018FCC File Offset: 0x000171CC
			protected override bool OnRuleMatchedElement(RuleMatcher matcher, VisualElement element)
			{
				T t = element as T;
				bool flag = t != null;
				if (flag)
				{
					this.result.Add(this.callBack.Invoke(t));
				}
				return false;
			}

			// Token: 0x040002AD RID: 685
			public static UQueryState<T>.DelegateQueryMatcher<TReturnType> s_Instance = new UQueryState<T>.DelegateQueryMatcher<TReturnType>();
		}

		// Token: 0x020000D3 RID: 211
		public struct Enumerator : IEnumerator<T>, IEnumerator, IDisposable
		{
			// Token: 0x060006DF RID: 1759 RVA: 0x00019020 File Offset: 0x00017220
			internal Enumerator(UQueryState<T> queryState)
			{
				this.iterationList = VisualElementListPool.Get(0);
				UQueryState<T>.s_EnumerationList.matches = this.iterationList;
				UQueryState<T>.s_EnumerationList.Run(queryState.m_Element, queryState.m_Matchers);
				UQueryState<T>.s_EnumerationList.Reset();
				this.currentIndex = -1;
			}

			// Token: 0x17000179 RID: 377
			// (get) Token: 0x060006E0 RID: 1760 RVA: 0x00019074 File Offset: 0x00017274
			public T Current
			{
				get
				{
					return (T)((object)this.iterationList[this.currentIndex]);
				}
			}

			// Token: 0x1700017A RID: 378
			// (get) Token: 0x060006E1 RID: 1761 RVA: 0x0001908C File Offset: 0x0001728C
			object IEnumerator.Current
			{
				get
				{
					return this.Current;
				}
			}

			// Token: 0x060006E2 RID: 1762 RVA: 0x0001909C File Offset: 0x0001729C
			public bool MoveNext()
			{
				int num = this.currentIndex + 1;
				this.currentIndex = num;
				return num < this.iterationList.Count;
			}

			// Token: 0x060006E3 RID: 1763 RVA: 0x000190CC File Offset: 0x000172CC
			public void Reset()
			{
				this.currentIndex = -1;
			}

			// Token: 0x060006E4 RID: 1764 RVA: 0x000190D6 File Offset: 0x000172D6
			public void Dispose()
			{
				VisualElementListPool.Release(this.iterationList);
				this.iterationList = null;
			}

			// Token: 0x040002AE RID: 686
			private List<VisualElement> iterationList;

			// Token: 0x040002AF RID: 687
			private int currentIndex;
		}
	}
}
