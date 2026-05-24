using System;
using System.Collections.Generic;

namespace UnityEngine.UIElements
{
	// Token: 0x020002AF RID: 687
	internal class StyleVariableContext
	{
		// Token: 0x06001719 RID: 5913 RVA: 0x0005E4EC File Offset: 0x0005C6EC
		public void Add(StyleVariable sv)
		{
			int hashCode = sv.GetHashCode();
			int num = this.m_SortedHash.BinarySearch(hashCode);
			bool flag = num >= 0;
			if (!flag)
			{
				this.m_SortedHash.Insert(~num, hashCode);
				this.m_Variables.Add(sv);
				this.m_VariableHash = ((this.m_Variables.Count == 0) ? sv.GetHashCode() : ((this.m_VariableHash * 397) ^ sv.GetHashCode()));
			}
		}

		// Token: 0x0600171A RID: 5914 RVA: 0x0005E57C File Offset: 0x0005C77C
		public void AddInitialRange(StyleVariableContext other)
		{
			bool flag = other.m_Variables.Count > 0;
			if (flag)
			{
				Debug.Assert(this.m_Variables.Count == 0);
				this.m_VariableHash = other.m_VariableHash;
				this.m_Variables.AddRange(other.m_Variables);
				this.m_SortedHash.AddRange(other.m_SortedHash);
			}
		}

		// Token: 0x0600171B RID: 5915 RVA: 0x0005E5E4 File Offset: 0x0005C7E4
		public void Clear()
		{
			bool flag = this.m_Variables.Count > 0;
			if (flag)
			{
				this.m_Variables.Clear();
				this.m_VariableHash = 0;
				this.m_SortedHash.Clear();
			}
		}

		// Token: 0x0600171C RID: 5916 RVA: 0x0005E625 File Offset: 0x0005C825
		public StyleVariableContext()
		{
			this.m_Variables = new List<StyleVariable>();
			this.m_VariableHash = 0;
			this.m_SortedHash = new List<int>();
		}

		// Token: 0x0600171D RID: 5917 RVA: 0x0005E64C File Offset: 0x0005C84C
		public StyleVariableContext(StyleVariableContext other)
		{
			this.m_Variables = new List<StyleVariable>(other.m_Variables);
			this.m_VariableHash = other.m_VariableHash;
			this.m_SortedHash = new List<int>(other.m_SortedHash);
		}

		// Token: 0x0600171E RID: 5918 RVA: 0x0005E684 File Offset: 0x0005C884
		public bool TryFindVariable(string name, out StyleVariable v)
		{
			for (int i = this.m_Variables.Count - 1; i >= 0; i--)
			{
				bool flag = this.m_Variables[i].name == name;
				if (flag)
				{
					v = this.m_Variables[i];
					return true;
				}
			}
			v = default(StyleVariable);
			return false;
		}

		// Token: 0x0600171F RID: 5919 RVA: 0x0005E6F4 File Offset: 0x0005C8F4
		public int GetVariableHash()
		{
			return this.m_VariableHash;
		}

		// Token: 0x040009DE RID: 2526
		public static readonly StyleVariableContext none = new StyleVariableContext();

		// Token: 0x040009DF RID: 2527
		private int m_VariableHash;

		// Token: 0x040009E0 RID: 2528
		private List<StyleVariable> m_Variables;

		// Token: 0x040009E1 RID: 2529
		private List<int> m_SortedHash;
	}
}
