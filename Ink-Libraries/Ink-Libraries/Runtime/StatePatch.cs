using System;
using System.Collections.Generic;

namespace Ink.Runtime
{
	// Token: 0x0200002A RID: 42
	public class StatePatch
	{
		// Token: 0x1700006C RID: 108
		// (get) Token: 0x06000245 RID: 581 RVA: 0x0000D822 File Offset: 0x0000BA22
		public Dictionary<string, Object> globals
		{
			get
			{
				return this._globals;
			}
		}

		// Token: 0x1700006D RID: 109
		// (get) Token: 0x06000246 RID: 582 RVA: 0x0000D82A File Offset: 0x0000BA2A
		public HashSet<string> changedVariables
		{
			get
			{
				return this._changedVariables;
			}
		}

		// Token: 0x1700006E RID: 110
		// (get) Token: 0x06000247 RID: 583 RVA: 0x0000D832 File Offset: 0x0000BA32
		public Dictionary<Container, int> visitCounts
		{
			get
			{
				return this._visitCounts;
			}
		}

		// Token: 0x1700006F RID: 111
		// (get) Token: 0x06000248 RID: 584 RVA: 0x0000D83A File Offset: 0x0000BA3A
		public Dictionary<Container, int> turnIndices
		{
			get
			{
				return this._turnIndices;
			}
		}

		// Token: 0x06000249 RID: 585 RVA: 0x0000D844 File Offset: 0x0000BA44
		public StatePatch(StatePatch toCopy)
		{
			if (toCopy != null)
			{
				this._globals = new Dictionary<string, Object>(toCopy._globals);
				this._changedVariables = new HashSet<string>(toCopy._changedVariables);
				this._visitCounts = new Dictionary<Container, int>(toCopy._visitCounts);
				this._turnIndices = new Dictionary<Container, int>(toCopy._turnIndices);
				return;
			}
			this._globals = new Dictionary<string, Object>();
			this._changedVariables = new HashSet<string>();
			this._visitCounts = new Dictionary<Container, int>();
			this._turnIndices = new Dictionary<Container, int>();
		}

		// Token: 0x0600024A RID: 586 RVA: 0x0000D8EC File Offset: 0x0000BAEC
		public bool TryGetGlobal(string name, out Object value)
		{
			return this._globals.TryGetValue(name, out value);
		}

		// Token: 0x0600024B RID: 587 RVA: 0x0000D8FB File Offset: 0x0000BAFB
		public void SetGlobal(string name, Object value)
		{
			this._globals[name] = value;
		}

		// Token: 0x0600024C RID: 588 RVA: 0x0000D90A File Offset: 0x0000BB0A
		public void AddChangedVariable(string name)
		{
			this._changedVariables.Add(name);
		}

		// Token: 0x0600024D RID: 589 RVA: 0x0000D919 File Offset: 0x0000BB19
		public bool TryGetVisitCount(Container container, out int count)
		{
			return this._visitCounts.TryGetValue(container, out count);
		}

		// Token: 0x0600024E RID: 590 RVA: 0x0000D928 File Offset: 0x0000BB28
		public void SetVisitCount(Container container, int count)
		{
			this._visitCounts[container] = count;
		}

		// Token: 0x0600024F RID: 591 RVA: 0x0000D937 File Offset: 0x0000BB37
		public void SetTurnIndex(Container container, int index)
		{
			this._turnIndices[container] = index;
		}

		// Token: 0x06000250 RID: 592 RVA: 0x0000D946 File Offset: 0x0000BB46
		public bool TryGetTurnIndex(Container container, out int index)
		{
			return this._turnIndices.TryGetValue(container, out index);
		}

		// Token: 0x040000C4 RID: 196
		private Dictionary<string, Object> _globals;

		// Token: 0x040000C5 RID: 197
		private HashSet<string> _changedVariables = new HashSet<string>();

		// Token: 0x040000C6 RID: 198
		private Dictionary<Container, int> _visitCounts = new Dictionary<Container, int>();

		// Token: 0x040000C7 RID: 199
		private Dictionary<Container, int> _turnIndices = new Dictionary<Container, int>();
	}
}
