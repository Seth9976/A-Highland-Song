using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ink.Runtime
{
	// Token: 0x02000026 RID: 38
	public class ProfileNode
	{
		// Token: 0x17000066 RID: 102
		// (get) Token: 0x06000235 RID: 565 RVA: 0x0000D54D File Offset: 0x0000B74D
		public bool hasChildren
		{
			get
			{
				return this._nodes != null && this._nodes.Count > 0;
			}
		}

		// Token: 0x17000067 RID: 103
		// (get) Token: 0x06000236 RID: 566 RVA: 0x0000D567 File Offset: 0x0000B767
		public int totalMillisecs
		{
			get
			{
				return (int)this._totalMillisecs;
			}
		}

		// Token: 0x06000237 RID: 567 RVA: 0x0000D570 File Offset: 0x0000B770
		public ProfileNode()
		{
		}

		// Token: 0x06000238 RID: 568 RVA: 0x0000D578 File Offset: 0x0000B778
		public ProfileNode(string key)
		{
			this.key = key;
		}

		// Token: 0x06000239 RID: 569 RVA: 0x0000D587 File Offset: 0x0000B787
		public void AddSample(string[] stack, double duration)
		{
			this.AddSample(stack, -1, duration);
		}

		// Token: 0x0600023A RID: 570 RVA: 0x0000D594 File Offset: 0x0000B794
		private void AddSample(string[] stack, int stackIdx, double duration)
		{
			this._totalSampleCount++;
			this._totalMillisecs += duration;
			if (stackIdx == stack.Length - 1)
			{
				this._selfSampleCount++;
				this._selfMillisecs += duration;
			}
			if (stackIdx + 1 < stack.Length)
			{
				this.AddSampleToNode(stack, stackIdx + 1, duration);
			}
		}

		// Token: 0x0600023B RID: 571 RVA: 0x0000D5F4 File Offset: 0x0000B7F4
		private void AddSampleToNode(string[] stack, int stackIdx, double duration)
		{
			string text = stack[stackIdx];
			if (this._nodes == null)
			{
				this._nodes = new Dictionary<string, ProfileNode>();
			}
			ProfileNode profileNode;
			if (!this._nodes.TryGetValue(text, out profileNode))
			{
				profileNode = new ProfileNode(text);
				this._nodes[text] = profileNode;
			}
			profileNode.AddSample(stack, stackIdx, duration);
		}

		// Token: 0x17000068 RID: 104
		// (get) Token: 0x0600023C RID: 572 RVA: 0x0000D645 File Offset: 0x0000B845
		public IEnumerable<KeyValuePair<string, ProfileNode>> descendingOrderedNodes
		{
			get
			{
				if (this._nodes == null)
				{
					return null;
				}
				return this._nodes.OrderByDescending((KeyValuePair<string, ProfileNode> keyNode) => keyNode.Value._totalMillisecs);
			}
		}

		// Token: 0x0600023D RID: 573 RVA: 0x0000D67C File Offset: 0x0000B87C
		private void PrintHierarchy(StringBuilder sb, int indent)
		{
			this.Pad(sb, indent);
			sb.Append(this.key);
			sb.Append(": ");
			sb.AppendLine(this.ownReport);
			if (this._nodes == null)
			{
				return;
			}
			foreach (KeyValuePair<string, ProfileNode> keyValuePair in this.descendingOrderedNodes)
			{
				keyValuePair.Value.PrintHierarchy(sb, indent + 1);
			}
		}

		// Token: 0x17000069 RID: 105
		// (get) Token: 0x0600023E RID: 574 RVA: 0x0000D70C File Offset: 0x0000B90C
		public string ownReport
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append("total ");
				stringBuilder.Append(Profiler.FormatMillisecs(this._totalMillisecs));
				stringBuilder.Append(", self ");
				stringBuilder.Append(Profiler.FormatMillisecs(this._selfMillisecs));
				stringBuilder.Append(" (");
				stringBuilder.Append(this._selfSampleCount);
				stringBuilder.Append(" self samples, ");
				stringBuilder.Append(this._totalSampleCount);
				stringBuilder.Append(" total)");
				return stringBuilder.ToString();
			}
		}

		// Token: 0x0600023F RID: 575 RVA: 0x0000D7A0 File Offset: 0x0000B9A0
		private void Pad(StringBuilder sb, int spaces)
		{
			for (int i = 0; i < spaces; i++)
			{
				sb.Append("   ");
			}
		}

		// Token: 0x06000240 RID: 576 RVA: 0x0000D7C8 File Offset: 0x0000B9C8
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			this.PrintHierarchy(stringBuilder, 0);
			return stringBuilder.ToString();
		}

		// Token: 0x040000B7 RID: 183
		public readonly string key;

		// Token: 0x040000B8 RID: 184
		public bool openInUI;

		// Token: 0x040000B9 RID: 185
		private Dictionary<string, ProfileNode> _nodes;

		// Token: 0x040000BA RID: 186
		private double _selfMillisecs;

		// Token: 0x040000BB RID: 187
		private double _totalMillisecs;

		// Token: 0x040000BC RID: 188
		private int _selfSampleCount;

		// Token: 0x040000BD RID: 189
		private int _totalSampleCount;
	}
}
