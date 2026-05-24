using System;
using System.Collections.Generic;

namespace Ink
{
	// Token: 0x02000008 RID: 8
	public class CommentEliminator : StringParser
	{
		// Token: 0x0600001C RID: 28 RVA: 0x00002654 File Offset: 0x00000854
		public CommentEliminator(string input)
			: base(input)
		{
		}

		// Token: 0x0600001D RID: 29 RVA: 0x00002690 File Offset: 0x00000890
		public string Process()
		{
			List<string> list = base.Interleave<string>(base.Optional(new StringParser.ParseRule(this.CommentsAndNewlines)), base.Optional(new StringParser.ParseRule(this.MainInk)), null, true);
			if (list != null)
			{
				return string.Join("", list.ToArray());
			}
			return null;
		}

		// Token: 0x0600001E RID: 30 RVA: 0x000026DF File Offset: 0x000008DF
		private string MainInk()
		{
			return base.ParseUntil(new StringParser.ParseRule(this.CommentsAndNewlines), this._commentOrNewlineStartCharacter, null);
		}

		// Token: 0x0600001F RID: 31 RVA: 0x000026FC File Offset: 0x000008FC
		private string CommentsAndNewlines()
		{
			List<string> list = base.Interleave<string>(base.Optional(new StringParser.ParseRule(base.ParseNewline)), base.Optional(new StringParser.ParseRule(this.ParseSingleComment)), null, true);
			if (list != null)
			{
				return string.Join("", list.ToArray());
			}
			return null;
		}

		// Token: 0x06000020 RID: 32 RVA: 0x0000274B File Offset: 0x0000094B
		private string ParseSingleComment()
		{
			return (string)base.OneOf(new StringParser.ParseRule[]
			{
				new StringParser.ParseRule(this.EndOfLineComment),
				new StringParser.ParseRule(this.BlockComment)
			});
		}

		// Token: 0x06000021 RID: 33 RVA: 0x0000277C File Offset: 0x0000097C
		private string EndOfLineComment()
		{
			if (base.ParseString("//") == null)
			{
				return null;
			}
			base.ParseUntilCharactersFromCharSet(this._newlineCharacters, -1);
			return "";
		}

		// Token: 0x06000022 RID: 34 RVA: 0x000027A0 File Offset: 0x000009A0
		private string BlockComment()
		{
			if (base.ParseString("/*") == null)
			{
				return null;
			}
			int lineIndex = base.lineIndex;
			bool flag = base.ParseUntil(base.String("*/"), this._commentBlockEndCharacter, null) != null;
			if (!base.endOfInput)
			{
				base.ParseString("*/");
			}
			if (flag)
			{
				return new string('\n', base.lineIndex - lineIndex);
			}
			return null;
		}

		// Token: 0x04000013 RID: 19
		private CharacterSet _commentOrNewlineStartCharacter = new CharacterSet("/\r\n");

		// Token: 0x04000014 RID: 20
		private CharacterSet _commentBlockEndCharacter = new CharacterSet("*");

		// Token: 0x04000015 RID: 21
		private CharacterSet _newlineCharacters = new CharacterSet("\n\r");
	}
}
