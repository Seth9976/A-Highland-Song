using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Ink.Parsed;

namespace Ink
{
	// Token: 0x0200000E RID: 14
	public class StringParser
	{
		// Token: 0x060000AD RID: 173 RVA: 0x00006820 File Offset: 0x00004A20
		public StringParser(string str)
		{
			str = this.PreProcessInputString(str);
			this.state = new StringParserState();
			if (str != null)
			{
				this._chars = str.ToCharArray();
			}
			else
			{
				this._chars = new char[0];
			}
			this.inputString = str;
		}

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x060000AE RID: 174 RVA: 0x00006860 File Offset: 0x00004A60
		// (set) Token: 0x060000AF RID: 175 RVA: 0x00006868 File Offset: 0x00004A68
		protected StringParser.ErrorHandler errorHandler { get; set; }

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x060000B0 RID: 176 RVA: 0x00006871 File Offset: 0x00004A71
		public char currentCharacter
		{
			get
			{
				if (this.index >= 0 && this.remainingLength > 0)
				{
					return this._chars[this.index];
				}
				return '\0';
			}
		}

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x060000B1 RID: 177 RVA: 0x00006894 File Offset: 0x00004A94
		// (set) Token: 0x060000B2 RID: 178 RVA: 0x0000689C File Offset: 0x00004A9C
		public StringParserState state { get; private set; }

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x060000B3 RID: 179 RVA: 0x000068A5 File Offset: 0x00004AA5
		// (set) Token: 0x060000B4 RID: 180 RVA: 0x000068AD File Offset: 0x00004AAD
		public bool hadError { get; protected set; }

		// Token: 0x060000B5 RID: 181 RVA: 0x000068B6 File Offset: 0x00004AB6
		protected virtual string PreProcessInputString(string str)
		{
			return str;
		}

		// Token: 0x060000B6 RID: 182 RVA: 0x000068B9 File Offset: 0x00004AB9
		protected int BeginRule()
		{
			return this.state.Push();
		}

		// Token: 0x060000B7 RID: 183 RVA: 0x000068C6 File Offset: 0x00004AC6
		protected object FailRule(int expectedRuleId)
		{
			this.state.Pop(expectedRuleId);
			return null;
		}

		// Token: 0x060000B8 RID: 184 RVA: 0x000068D5 File Offset: 0x00004AD5
		protected void CancelRule(int expectedRuleId)
		{
			this.state.Pop(expectedRuleId);
		}

		// Token: 0x060000B9 RID: 185 RVA: 0x000068E4 File Offset: 0x00004AE4
		protected object SucceedRule(int expectedRuleId, object result = null)
		{
			StringParserState.Element element = this.state.Peek(expectedRuleId);
			StringParserState.Element element2 = this.state.PeekPenultimate();
			this.RuleDidSucceed(result, element2, element);
			this.state.Squash();
			if (result == null)
			{
				result = StringParser.ParseSuccess;
			}
			return result;
		}

		// Token: 0x060000BA RID: 186 RVA: 0x00006929 File Offset: 0x00004B29
		protected virtual void RuleDidSucceed(object result, StringParserState.Element startState, StringParserState.Element endState)
		{
		}

		// Token: 0x060000BB RID: 187 RVA: 0x0000692C File Offset: 0x00004B2C
		protected object Expect(StringParser.ParseRule rule, string message = null, StringParser.ParseRule recoveryRule = null)
		{
			object obj = this.ParseObject(rule);
			if (obj == null)
			{
				if (message == null)
				{
					message = rule.Method.Name;
				}
				string text = this.LineRemainder();
				string text2;
				if (text == null || text.Length == 0)
				{
					text2 = "end of line";
				}
				else
				{
					text2 = "'" + text + "'";
				}
				this.Error("Expected " + message + " but saw " + text2, false);
				if (recoveryRule != null)
				{
					obj = recoveryRule();
				}
			}
			return obj;
		}

		// Token: 0x060000BC RID: 188 RVA: 0x000069A2 File Offset: 0x00004BA2
		protected void Error(string message, bool isWarning = false)
		{
			this.ErrorOnLine(message, this.lineIndex + 1, isWarning);
		}

		// Token: 0x060000BD RID: 189 RVA: 0x000069B4 File Offset: 0x00004BB4
		protected void ErrorWithParsedObject(string message, Object result, bool isWarning = false)
		{
			this.ErrorOnLine(message, result.debugMetadata.startLineNumber, isWarning);
		}

		// Token: 0x060000BE RID: 190 RVA: 0x000069CC File Offset: 0x00004BCC
		protected void ErrorOnLine(string message, int lineNumber, bool isWarning)
		{
			if (!this.state.errorReportedAlreadyInScope)
			{
				string text = (isWarning ? "Warning" : "Error");
				if (this.errorHandler == null)
				{
					throw new Exception(string.Concat(new string[]
					{
						text,
						" on line ",
						lineNumber.ToString(),
						": ",
						message
					}));
				}
				this.errorHandler(message, this.index, lineNumber - 1, isWarning);
				this.state.NoteErrorReported();
			}
			if (!isWarning)
			{
				this.hadError = true;
			}
		}

		// Token: 0x060000BF RID: 191 RVA: 0x00006A5C File Offset: 0x00004C5C
		protected void Warning(string message)
		{
			this.Error(message, true);
		}

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x060000C0 RID: 192 RVA: 0x00006A66 File Offset: 0x00004C66
		public bool endOfInput
		{
			get
			{
				return this.index >= this._chars.Length;
			}
		}

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x060000C1 RID: 193 RVA: 0x00006A7B File Offset: 0x00004C7B
		public string remainingString
		{
			get
			{
				return new string(this._chars, this.index, this.remainingLength);
			}
		}

		// Token: 0x060000C2 RID: 194 RVA: 0x00006A94 File Offset: 0x00004C94
		public string LineRemainder()
		{
			return (string)this.Peek(() => this.ParseUntilCharactersFromString("\n\r", -1));
		}

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x060000C3 RID: 195 RVA: 0x00006AAD File Offset: 0x00004CAD
		public int remainingLength
		{
			get
			{
				return this._chars.Length - this.index;
			}
		}

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x060000C4 RID: 196 RVA: 0x00006ABE File Offset: 0x00004CBE
		// (set) Token: 0x060000C5 RID: 197 RVA: 0x00006AC6 File Offset: 0x00004CC6
		public string inputString { get; private set; }

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x060000C7 RID: 199 RVA: 0x00006ADD File Offset: 0x00004CDD
		// (set) Token: 0x060000C6 RID: 198 RVA: 0x00006ACF File Offset: 0x00004CCF
		public int lineIndex
		{
			get
			{
				return this.state.lineIndex;
			}
			set
			{
				this.state.lineIndex = value;
			}
		}

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x060000C9 RID: 201 RVA: 0x00006AF8 File Offset: 0x00004CF8
		// (set) Token: 0x060000C8 RID: 200 RVA: 0x00006AEA File Offset: 0x00004CEA
		public int characterInLineIndex
		{
			get
			{
				return this.state.characterInLineIndex;
			}
			set
			{
				this.state.characterInLineIndex = value;
			}
		}

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x060000CB RID: 203 RVA: 0x00006B13 File Offset: 0x00004D13
		// (set) Token: 0x060000CA RID: 202 RVA: 0x00006B05 File Offset: 0x00004D05
		public int index
		{
			get
			{
				return this.state.characterIndex;
			}
			private set
			{
				this.state.characterIndex = value;
			}
		}

		// Token: 0x060000CC RID: 204 RVA: 0x00006B20 File Offset: 0x00004D20
		public void SetFlag(uint flag, bool trueOrFalse)
		{
			if (trueOrFalse)
			{
				this.state.customFlags |= flag;
				return;
			}
			this.state.customFlags &= ~flag;
		}

		// Token: 0x060000CD RID: 205 RVA: 0x00006B4D File Offset: 0x00004D4D
		public bool GetFlag(uint flag)
		{
			return (this.state.customFlags & flag) > 0U;
		}

		// Token: 0x060000CE RID: 206 RVA: 0x00006B60 File Offset: 0x00004D60
		public object ParseObject(StringParser.ParseRule rule)
		{
			int num = this.BeginRule();
			int stackHeight = this.state.stackHeight;
			object obj = rule();
			if (stackHeight != this.state.stackHeight)
			{
				throw new Exception("Mismatched Begin/Fail/Succeed rules");
			}
			if (obj == null)
			{
				return this.FailRule(num);
			}
			this.SucceedRule(num, obj);
			return obj;
		}

		// Token: 0x060000CF RID: 207 RVA: 0x00006BB4 File Offset: 0x00004DB4
		public T Parse<T>(StringParser.SpecificParseRule<T> rule) where T : class
		{
			int num = this.BeginRule();
			T t = rule();
			if (t == null)
			{
				this.FailRule(num);
				return default(T);
			}
			this.SucceedRule(num, t);
			return t;
		}

		// Token: 0x060000D0 RID: 208 RVA: 0x00006BF8 File Offset: 0x00004DF8
		public object OneOf(params StringParser.ParseRule[] array)
		{
			foreach (StringParser.ParseRule parseRule in array)
			{
				object obj = this.ParseObject(parseRule);
				if (obj != null)
				{
					return obj;
				}
			}
			return null;
		}

		// Token: 0x060000D1 RID: 209 RVA: 0x00006C28 File Offset: 0x00004E28
		public List<object> OneOrMore(StringParser.ParseRule rule)
		{
			List<object> list = new List<object>();
			object obj;
			do
			{
				obj = this.ParseObject(rule);
				if (obj != null)
				{
					list.Add(obj);
				}
			}
			while (obj != null);
			if (list.Count > 0)
			{
				return list;
			}
			return null;
		}

		// Token: 0x060000D2 RID: 210 RVA: 0x00006C5E File Offset: 0x00004E5E
		public StringParser.ParseRule Optional(StringParser.ParseRule rule)
		{
			return delegate
			{
				object obj = this.ParseObject(rule);
				if (obj == null)
				{
					obj = StringParser.ParseSuccess;
				}
				return obj;
			};
		}

		// Token: 0x060000D3 RID: 211 RVA: 0x00006C7E File Offset: 0x00004E7E
		public StringParser.ParseRule Exclude(StringParser.ParseRule rule)
		{
			return delegate
			{
				if (this.ParseObject(rule) == null)
				{
					return null;
				}
				return StringParser.ParseSuccess;
			};
		}

		// Token: 0x060000D4 RID: 212 RVA: 0x00006C9E File Offset: 0x00004E9E
		public StringParser.ParseRule OptionalExclude(StringParser.ParseRule rule)
		{
			return delegate
			{
				this.ParseObject(rule);
				return StringParser.ParseSuccess;
			};
		}

		// Token: 0x060000D5 RID: 213 RVA: 0x00006CBE File Offset: 0x00004EBE
		protected StringParser.ParseRule String(string str)
		{
			return () => this.ParseString(str);
		}

		// Token: 0x060000D6 RID: 214 RVA: 0x00006CE0 File Offset: 0x00004EE0
		private void TryAddResultToList<T>(object result, List<T> list, bool flatten = true)
		{
			if (result == StringParser.ParseSuccess)
			{
				return;
			}
			if (flatten)
			{
				ICollection collection = result as ICollection;
				if (collection != null)
				{
					foreach (object obj in collection)
					{
						list.Add((T)((object)obj));
					}
					return;
				}
			}
			list.Add((T)((object)result));
		}

		// Token: 0x060000D7 RID: 215 RVA: 0x00006D58 File Offset: 0x00004F58
		public List<T> Interleave<T>(StringParser.ParseRule ruleA, StringParser.ParseRule ruleB, StringParser.ParseRule untilTerminator = null, bool flatten = true)
		{
			int num = this.BeginRule();
			List<T> list = new List<T>();
			object obj = this.ParseObject(ruleA);
			if (obj == null)
			{
				return (List<T>)this.FailRule(num);
			}
			this.TryAddResultToList<T>(obj, list, flatten);
			while (untilTerminator == null || this.Peek(untilTerminator) == null)
			{
				object obj2 = this.ParseObject(ruleB);
				if (obj2 == null)
				{
					break;
				}
				this.TryAddResultToList<T>(obj2, list, flatten);
				object obj3 = null;
				if (obj2 != null)
				{
					obj3 = this.ParseObject(ruleA);
					if (obj3 == null)
					{
						break;
					}
					this.TryAddResultToList<T>(obj3, list, flatten);
				}
				if ((obj2 == null && obj3 == null) || (obj2 == StringParser.ParseSuccess && obj3 == StringParser.ParseSuccess) || this.remainingLength <= 0)
				{
					break;
				}
			}
			if (list.Count == 0)
			{
				return (List<T>)this.FailRule(num);
			}
			return (List<T>)this.SucceedRule(num, list);
		}

		// Token: 0x060000D8 RID: 216 RVA: 0x00006E1C File Offset: 0x0000501C
		public string ParseString(string str)
		{
			if (str.Length > this.remainingLength)
			{
				return null;
			}
			int num = this.BeginRule();
			int num2 = this.index;
			int num3 = this.characterInLineIndex;
			int num4 = this.lineIndex;
			bool flag = true;
			foreach (char c in str)
			{
				if (this._chars[num2] != c)
				{
					flag = false;
					break;
				}
				if (c == '\n')
				{
					num4++;
					num3 = -1;
				}
				num2++;
				num3++;
			}
			this.index = num2;
			this.characterInLineIndex = num3;
			this.lineIndex = num4;
			if (flag)
			{
				return (string)this.SucceedRule(num, str);
			}
			return (string)this.FailRule(num);
		}

		// Token: 0x060000D9 RID: 217 RVA: 0x00006ED4 File Offset: 0x000050D4
		public char ParseSingleCharacter()
		{
			if (this.remainingLength > 0)
			{
				char c = this._chars[this.index];
				int num;
				if (c == '\n')
				{
					num = this.lineIndex;
					this.lineIndex = num + 1;
					this.characterInLineIndex = -1;
				}
				num = this.index;
				this.index = num + 1;
				num = this.characterInLineIndex;
				this.characterInLineIndex = num + 1;
				return c;
			}
			return '\0';
		}

		// Token: 0x060000DA RID: 218 RVA: 0x00006F35 File Offset: 0x00005135
		public string ParseUntilCharactersFromString(string str, int maxCount = -1)
		{
			return this.ParseCharactersFromString(str, false, maxCount);
		}

		// Token: 0x060000DB RID: 219 RVA: 0x00006F40 File Offset: 0x00005140
		public string ParseUntilCharactersFromCharSet(CharacterSet charSet, int maxCount = -1)
		{
			return this.ParseCharactersFromCharSet(charSet, false, maxCount);
		}

		// Token: 0x060000DC RID: 220 RVA: 0x00006F4B File Offset: 0x0000514B
		public string ParseCharactersFromString(string str, int maxCount = -1)
		{
			return this.ParseCharactersFromString(str, true, maxCount);
		}

		// Token: 0x060000DD RID: 221 RVA: 0x00006F56 File Offset: 0x00005156
		public string ParseCharactersFromString(string str, bool shouldIncludeStrChars, int maxCount = -1)
		{
			return this.ParseCharactersFromCharSet(new CharacterSet(str), shouldIncludeStrChars, -1);
		}

		// Token: 0x060000DE RID: 222 RVA: 0x00006F68 File Offset: 0x00005168
		public string ParseCharactersFromCharSet(CharacterSet charSet, bool shouldIncludeChars = true, int maxCount = -1)
		{
			if (maxCount == -1)
			{
				maxCount = int.MaxValue;
			}
			int index = this.index;
			int num = this.index;
			int num2 = this.characterInLineIndex;
			int num3 = this.lineIndex;
			int num4 = 0;
			while (num < this._chars.Length && charSet.Contains(this._chars[num]) == shouldIncludeChars && num4 < maxCount)
			{
				if (this._chars[num] == '\n')
				{
					num3++;
					num2 = -1;
				}
				num++;
				num2++;
				num4++;
			}
			this.index = num;
			this.characterInLineIndex = num2;
			this.lineIndex = num3;
			if (this.index > index)
			{
				return new string(this._chars, index, this.index - index);
			}
			return null;
		}

		// Token: 0x060000DF RID: 223 RVA: 0x00007018 File Offset: 0x00005218
		public object Peek(StringParser.ParseRule rule)
		{
			int num = this.BeginRule();
			object obj = rule();
			this.CancelRule(num);
			return obj;
		}

		// Token: 0x060000E0 RID: 224 RVA: 0x0000703C File Offset: 0x0000523C
		public string ParseUntil(StringParser.ParseRule stopRule, CharacterSet pauseCharacters = null, CharacterSet endCharacters = null)
		{
			int num = this.BeginRule();
			CharacterSet characterSet = new CharacterSet();
			if (pauseCharacters != null)
			{
				characterSet.UnionWith(pauseCharacters);
			}
			if (endCharacters != null)
			{
				characterSet.UnionWith(endCharacters);
			}
			StringBuilder stringBuilder = new StringBuilder();
			for (;;)
			{
				string text = this.ParseUntilCharactersFromCharSet(characterSet, -1);
				if (text != null)
				{
					stringBuilder.Append(text);
				}
				if (this.Peek(stopRule) != null || this.endOfInput)
				{
					break;
				}
				char currentCharacter = this.currentCharacter;
				if (pauseCharacters == null || !pauseCharacters.Contains(currentCharacter))
				{
					break;
				}
				stringBuilder.Append(currentCharacter);
				int num2;
				if (currentCharacter == '\n')
				{
					num2 = this.lineIndex;
					this.lineIndex = num2 + 1;
					this.characterInLineIndex = -1;
				}
				num2 = this.index;
				this.index = num2 + 1;
				num2 = this.characterInLineIndex;
				this.characterInLineIndex = num2 + 1;
			}
			if (stringBuilder.Length > 0)
			{
				return (string)this.SucceedRule(num, stringBuilder.ToString());
			}
			return (string)this.FailRule(num);
		}

		// Token: 0x060000E1 RID: 225 RVA: 0x00007124 File Offset: 0x00005324
		public int? ParseInt()
		{
			int index = this.index;
			int characterInLineIndex = this.characterInLineIndex;
			bool flag = this.ParseString("-") != null;
			this.ParseCharactersFromString(" \t", -1);
			string text = this.ParseCharactersFromCharSet(StringParser.numbersCharacterSet, true, -1);
			if (text == null)
			{
				this.index = index;
				this.characterInLineIndex = characterInLineIndex;
				return null;
			}
			int num;
			if (int.TryParse(text, out num))
			{
				return new int?(flag ? (-num) : num);
			}
			this.Error(string.Concat(new string[]
			{
				"Failed to read integer value: ",
				text,
				". Perhaps it's out of the range of acceptable numbers ink supports? (",
				int.MinValue.ToString(),
				" to ",
				int.MaxValue.ToString(),
				")"
			}), false);
			return null;
		}

		// Token: 0x060000E2 RID: 226 RVA: 0x00007204 File Offset: 0x00005404
		public float? ParseFloat()
		{
			int index = this.index;
			int characterInLineIndex = this.characterInLineIndex;
			int? num = this.ParseInt();
			if (num != null && this.ParseString(".") != null)
			{
				string text = this.ParseCharactersFromCharSet(StringParser.numbersCharacterSet, true, -1);
				int? num2 = num;
				return new float?(float.Parse(num2.ToString() + "." + text, CultureInfo.InvariantCulture));
			}
			this.index = index;
			this.characterInLineIndex = characterInLineIndex;
			return null;
		}

		// Token: 0x060000E3 RID: 227 RVA: 0x00007290 File Offset: 0x00005490
		protected string ParseNewline()
		{
			int num = this.BeginRule();
			this.ParseString("\r");
			if (this.ParseString("\n") == null)
			{
				return (string)this.FailRule(num);
			}
			return (string)this.SucceedRule(num, "\n");
		}

		// Token: 0x0400003B RID: 59
		public static StringParser.ParseSuccessStruct ParseSuccess = new StringParser.ParseSuccessStruct();

		// Token: 0x0400003C RID: 60
		public static CharacterSet numbersCharacterSet = new CharacterSet("0123456789");

		// Token: 0x04000041 RID: 65
		private char[] _chars;

		// Token: 0x0200007D RID: 125
		// (Invoke) Token: 0x06000522 RID: 1314
		public delegate object ParseRule();

		// Token: 0x0200007E RID: 126
		// (Invoke) Token: 0x06000526 RID: 1318
		public delegate T SpecificParseRule<T>() where T : class;

		// Token: 0x0200007F RID: 127
		// (Invoke) Token: 0x0600052A RID: 1322
		public delegate void ErrorHandler(string message, int index, int lineIndex, bool isWarning);

		// Token: 0x02000080 RID: 128
		public class ParseSuccessStruct
		{
		}
	}
}
