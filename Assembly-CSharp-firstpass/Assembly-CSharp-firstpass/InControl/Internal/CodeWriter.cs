using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace InControl.Internal
{
	// Token: 0x0200007F RID: 127
	public class CodeWriter
	{
		// Token: 0x060005E9 RID: 1513 RVA: 0x0001583C File Offset: 0x00013A3C
		public CodeWriter()
		{
			this.indent = 0;
			this.stringBuilder = new StringBuilder(4096);
		}

		// Token: 0x060005EA RID: 1514 RVA: 0x0001585B File Offset: 0x00013A5B
		public void IncreaseIndent()
		{
			this.indent++;
		}

		// Token: 0x060005EB RID: 1515 RVA: 0x0001586B File Offset: 0x00013A6B
		public void DecreaseIndent()
		{
			this.indent--;
		}

		// Token: 0x060005EC RID: 1516 RVA: 0x0001587B File Offset: 0x00013A7B
		public void Append(string code)
		{
			this.Append(false, code);
		}

		// Token: 0x060005ED RID: 1517 RVA: 0x00015888 File Offset: 0x00013A88
		public void Append(bool trim, string code)
		{
			if (trim)
			{
				code = code.Trim();
			}
			string[] array = Regex.Split(code, "\\r?\\n|\\n");
			int num = array.Length;
			for (int i = 0; i < num; i++)
			{
				string text = array[i];
				if (!text.All(new Func<char, bool>(char.IsWhiteSpace)))
				{
					this.stringBuilder.Append('\t', this.indent);
					this.stringBuilder.Append(text);
				}
				if (i < num - 1)
				{
					this.stringBuilder.Append('\n');
				}
			}
		}

		// Token: 0x060005EE RID: 1518 RVA: 0x00015909 File Offset: 0x00013B09
		public void AppendLine(string code)
		{
			this.Append(code);
			this.stringBuilder.Append('\n');
		}

		// Token: 0x060005EF RID: 1519 RVA: 0x00015920 File Offset: 0x00013B20
		public void AppendLine(int count)
		{
			this.stringBuilder.Append('\n', count);
		}

		// Token: 0x060005F0 RID: 1520 RVA: 0x00015931 File Offset: 0x00013B31
		public void AppendFormat(string format, params object[] args)
		{
			this.Append(string.Format(format, args));
		}

		// Token: 0x060005F1 RID: 1521 RVA: 0x00015940 File Offset: 0x00013B40
		public void AppendLineFormat(string format, params object[] args)
		{
			this.AppendLine(string.Format(format, args));
		}

		// Token: 0x060005F2 RID: 1522 RVA: 0x0001594F File Offset: 0x00013B4F
		public override string ToString()
		{
			return this.stringBuilder.ToString();
		}

		// Token: 0x04000493 RID: 1171
		private const char newLine = '\n';

		// Token: 0x04000494 RID: 1172
		private int indent;

		// Token: 0x04000495 RID: 1173
		private readonly StringBuilder stringBuilder;
	}
}
