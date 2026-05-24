using System;
using System.Collections.Generic;
using System.Text;
using Ink.Runtime;

namespace Ink.Parsed
{
	// Token: 0x02000060 RID: 96
	public class StringExpression : Expression
	{
		// Token: 0x1700012A RID: 298
		// (get) Token: 0x060004B7 RID: 1207 RVA: 0x00019808 File Offset: 0x00017A08
		public bool isSingleString
		{
			get
			{
				return base.content.Count == 1 && base.content[0] is Text;
			}
		}

		// Token: 0x060004B8 RID: 1208 RVA: 0x00019830 File Offset: 0x00017A30
		public StringExpression(List<Object> content)
		{
			base.AddContent<Object>(content);
		}

		// Token: 0x060004B9 RID: 1209 RVA: 0x00019840 File Offset: 0x00017A40
		public override void GenerateIntoContainer(Container container)
		{
			container.AddContent(ControlCommand.BeginString());
			foreach (Object @object in base.content)
			{
				container.AddContent(@object.runtimeObject);
			}
			container.AddContent(ControlCommand.EndString());
		}

		// Token: 0x060004BA RID: 1210 RVA: 0x000198B0 File Offset: 0x00017AB0
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (Object @object in base.content)
			{
				stringBuilder.Append(@object.ToString());
			}
			return stringBuilder.ToString();
		}

		// Token: 0x060004BB RID: 1211 RVA: 0x00019918 File Offset: 0x00017B18
		public override bool Equals(object obj)
		{
			StringExpression stringExpression = obj as StringExpression;
			if (stringExpression == null)
			{
				return false;
			}
			if (!this.isSingleString || !stringExpression.isSingleString)
			{
				return false;
			}
			string text = this.ToString();
			string text2 = stringExpression.ToString();
			return text.Equals(text2);
		}

		// Token: 0x060004BC RID: 1212 RVA: 0x0001995C File Offset: 0x00017B5C
		public override int GetHashCode()
		{
			return this.ToString().GetHashCode();
		}
	}
}
