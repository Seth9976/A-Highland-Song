using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ink.Parsed;
using Ink.Runtime;

namespace Ink
{
	// Token: 0x02000009 RID: 9
	public class InkParser : StringParser
	{
		// Token: 0x06000023 RID: 35 RVA: 0x00002802 File Offset: 0x00000A02
		public InkParser(string str, string filenameForMetadata = null, Ink.ErrorHandler externalErrorHandler = null, IFileHandler fileHandler = null)
			: this(str, filenameForMetadata, externalErrorHandler, null, fileHandler)
		{
		}

		// Token: 0x06000024 RID: 36 RVA: 0x00002810 File Offset: 0x00000A10
		private InkParser(string str, string inkFilename = null, Ink.ErrorHandler externalErrorHandler = null, InkParser rootParser = null, IFileHandler fileHandler = null)
			: base(str)
		{
			this._filename = inkFilename;
			this.RegisterExpressionOperators();
			this.GenerateStatementLevelRules();
			base.errorHandler = new StringParser.ErrorHandler(this.OnStringParserError);
			this._externalErrorHandler = externalErrorHandler;
			this._fileHandler = fileHandler ?? new DefaultFileHandler();
			if (rootParser == null)
			{
				this._rootParser = this;
				this._openFilenames = new HashSet<string>();
				if (inkFilename != null)
				{
					string text = this._fileHandler.ResolveInkFilename(inkFilename);
					this._openFilenames.Add(text);
					return;
				}
			}
			else
			{
				this._rootParser = rootParser;
			}
		}

		// Token: 0x06000025 RID: 37 RVA: 0x000028CD File Offset: 0x00000ACD
		public Ink.Parsed.Story Parse()
		{
			return new Ink.Parsed.Story(this.StatementsAtLevel(InkParser.StatementLevel.Top), this._rootParser != this);
		}

		// Token: 0x06000026 RID: 38 RVA: 0x000028E8 File Offset: 0x00000AE8
		protected List<T> SeparatedList<T>(StringParser.SpecificParseRule<T> mainRule, StringParser.ParseRule separatorRule) where T : class
		{
			T t = base.Parse<T>(mainRule);
			if (t == null)
			{
				return null;
			}
			List<T> list = new List<T>();
			list.Add(t);
			int num;
			for (;;)
			{
				num = base.BeginRule();
				if (separatorRule() == null)
				{
					break;
				}
				T t2 = base.Parse<T>(mainRule);
				if (t2 == null)
				{
					goto Block_3;
				}
				base.SucceedRule(num, null);
				list.Add(t2);
			}
			base.FailRule(num);
			return list;
			Block_3:
			base.FailRule(num);
			return list;
		}

		// Token: 0x06000027 RID: 39 RVA: 0x0000295A File Offset: 0x00000B5A
		protected override string PreProcessInputString(string str)
		{
			return new CommentEliminator(str).Process();
		}

		// Token: 0x06000028 RID: 40 RVA: 0x00002968 File Offset: 0x00000B68
		protected DebugMetadata CreateDebugMetadata(StringParserState.Element stateAtStart, StringParserState.Element stateAtEnd)
		{
			return new DebugMetadata
			{
				startLineNumber = stateAtStart.lineIndex + 1,
				endLineNumber = stateAtEnd.lineIndex + 1,
				startCharacterNumber = stateAtStart.characterInLineIndex + 1,
				endCharacterNumber = stateAtEnd.characterInLineIndex + 1,
				fileName = this._filename
			};
		}

		// Token: 0x06000029 RID: 41 RVA: 0x000029C0 File Offset: 0x00000BC0
		protected override void RuleDidSucceed(object result, StringParserState.Element stateAtStart, StringParserState.Element stateAtEnd)
		{
			Ink.Parsed.Object @object = result as Ink.Parsed.Object;
			if (@object)
			{
				@object.debugMetadata = this.CreateDebugMetadata(stateAtStart, stateAtEnd);
				return;
			}
			List<Ink.Parsed.Object> list = result as List<Ink.Parsed.Object>;
			if (list != null)
			{
				foreach (Ink.Parsed.Object object2 in list)
				{
					if (!object2.hasOwnDebugMetadata)
					{
						object2.debugMetadata = this.CreateDebugMetadata(stateAtStart, stateAtEnd);
					}
				}
			}
			Identifier identifier = result as Identifier;
			if (identifier != null)
			{
				identifier.debugMetadata = this.CreateDebugMetadata(stateAtStart, stateAtEnd);
			}
		}

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x0600002A RID: 42 RVA: 0x00002A60 File Offset: 0x00000C60
		// (set) Token: 0x0600002B RID: 43 RVA: 0x00002A69 File Offset: 0x00000C69
		protected bool parsingStringExpression
		{
			get
			{
				return base.GetFlag(1U);
			}
			set
			{
				base.SetFlag(1U, value);
			}
		}

		// Token: 0x0600002C RID: 44 RVA: 0x00002A74 File Offset: 0x00000C74
		private void OnStringParserError(string message, int index, int lineIndex, bool isWarning)
		{
			string text = (isWarning ? "WARNING:" : "ERROR:");
			string text2;
			if (this._filename != null)
			{
				text2 = string.Format(text + " '{0}' line {1}: {2}", this._filename, lineIndex + 1, message);
			}
			else
			{
				text2 = string.Format(text + " line {0}: {1}", lineIndex + 1, message);
			}
			if (this._externalErrorHandler != null)
			{
				this._externalErrorHandler(text2, isWarning ? ErrorType.Warning : ErrorType.Error);
				return;
			}
			throw new Exception(text2);
		}

		// Token: 0x0600002D RID: 45 RVA: 0x00002AFC File Offset: 0x00000CFC
		protected AuthorWarning AuthorWarning()
		{
			this.Whitespace();
			Identifier identifier = base.Parse<Identifier>(new StringParser.SpecificParseRule<Identifier>(this.IdentifierWithMetadata));
			if (identifier == null || identifier.name != "TODO")
			{
				return null;
			}
			this.Whitespace();
			base.ParseString(":");
			this.Whitespace();
			return new AuthorWarning(base.ParseUntilCharactersFromString("\n\r", -1));
		}

		// Token: 0x0600002E RID: 46 RVA: 0x00002B68 File Offset: 0x00000D68
		private void ExtendIdentifierCharacterRanges(CharacterSet identifierCharSet)
		{
			foreach (CharacterRange characterRange in InkParser.ListAllCharacterRanges())
			{
				identifierCharSet.AddCharacters(characterRange.ToCharacterSet());
			}
		}

		// Token: 0x0600002F RID: 47 RVA: 0x00002B9C File Offset: 0x00000D9C
		public static CharacterRange[] ListAllCharacterRanges()
		{
			return new CharacterRange[]
			{
				InkParser.LatinBasic,
				InkParser.LatinExtendedA,
				InkParser.LatinExtendedB,
				InkParser.Arabic,
				InkParser.Armenian,
				InkParser.Cyrillic,
				InkParser.Greek,
				InkParser.Hebrew,
				InkParser.Korean,
				InkParser.Latin1Supplement
			};
		}

		// Token: 0x06000030 RID: 48 RVA: 0x00002C04 File Offset: 0x00000E04
		protected Ink.Parsed.Choice Choice()
		{
			bool flag = true;
			List<string> list = base.Interleave<string>(base.OptionalExclude(new StringParser.ParseRule(this.Whitespace)), base.String("*"), null, true);
			if (list == null)
			{
				list = base.Interleave<string>(base.OptionalExclude(new StringParser.ParseRule(this.Whitespace)), base.String("+"), null, true);
				if (list == null)
				{
					return null;
				}
				flag = false;
			}
			Identifier identifier = base.Parse<Identifier>(new StringParser.SpecificParseRule<Identifier>(this.BracketedName));
			this.Whitespace();
			Expression expression = base.Parse<Expression>(new StringParser.SpecificParseRule<Expression>(this.ChoiceCondition));
			this.Whitespace();
			this._parsingChoice = true;
			ContentList contentList = null;
			List<Ink.Parsed.Object> list2 = base.Parse<List<Ink.Parsed.Object>>(new StringParser.SpecificParseRule<List<Ink.Parsed.Object>>(this.MixedTextAndLogic));
			if (list2 != null)
			{
				contentList = new ContentList(list2);
			}
			ContentList contentList2 = null;
			ContentList contentList3 = null;
			bool flag2 = base.ParseString("[") != null;
			if (flag2)
			{
				List<Ink.Parsed.Object> list3 = base.Parse<List<Ink.Parsed.Object>>(new StringParser.SpecificParseRule<List<Ink.Parsed.Object>>(this.MixedTextAndLogic));
				if (list3 != null)
				{
					contentList2 = new ContentList(list3);
				}
				base.Expect(base.String("]"), "closing ']' for weave-style option", null);
				List<Ink.Parsed.Object> list4 = base.Parse<List<Ink.Parsed.Object>>(new StringParser.SpecificParseRule<List<Ink.Parsed.Object>>(this.MixedTextAndLogic));
				if (list4 != null)
				{
					contentList3 = new ContentList(list4);
				}
			}
			this.Whitespace();
			List<Ink.Parsed.Object> list5 = base.Parse<List<Ink.Parsed.Object>>(new StringParser.SpecificParseRule<List<Ink.Parsed.Object>>(this.MultiDivert));
			this._parsingChoice = false;
			this.Whitespace();
			bool flag3 = !contentList && !contentList3 && !contentList2;
			if (flag3 && list5 == null)
			{
				base.Warning("Choice is completely empty. Interpretting as a default fallback choice. Add a divert arrow to remove this warning: * ->");
			}
			else if (!contentList && flag2 && !contentList2)
			{
				base.Warning("Blank choice - if you intended a default fallback choice, use the `* ->` syntax");
			}
			if (!contentList3)
			{
				contentList3 = new ContentList();
			}
			List<Ink.Parsed.Tag> list6 = base.Parse<List<Ink.Parsed.Tag>>(new StringParser.SpecificParseRule<List<Ink.Parsed.Tag>>(this.Tags));
			if (list6 != null)
			{
				contentList3.AddContent<Ink.Parsed.Tag>(list6);
			}
			if (list5 != null)
			{
				foreach (Ink.Parsed.Object @object in list5)
				{
					Ink.Parsed.Divert divert = @object as Ink.Parsed.Divert;
					if (!divert || !divert.isEmpty)
					{
						contentList3.AddContent<Ink.Parsed.Object>(@object);
					}
				}
			}
			contentList3.AddContent<Text>(new Text("\n"));
			return new Ink.Parsed.Choice(contentList, contentList2, contentList3)
			{
				identifier = identifier,
				indentationDepth = list.Count,
				hasWeaveStyleInlineBrackets = flag2,
				condition = expression,
				onceOnly = flag,
				isInvisibleDefault = flag3
			};
		}

		// Token: 0x06000031 RID: 49 RVA: 0x00002EA4 File Offset: 0x000010A4
		protected Expression ChoiceCondition()
		{
			List<Expression> list = base.Interleave<Expression>(new StringParser.ParseRule(this.ChoiceSingleCondition), new StringParser.ParseRule(this.ChoiceConditionsSpace), null, true);
			if (list == null)
			{
				return null;
			}
			if (list.Count == 1)
			{
				return list[0];
			}
			return new MultipleConditionExpression(list);
		}

		// Token: 0x06000032 RID: 50 RVA: 0x00002EEE File Offset: 0x000010EE
		protected object ChoiceConditionsSpace()
		{
			this.Newline();
			this.Whitespace();
			return StringParser.ParseSuccess;
		}

		// Token: 0x06000033 RID: 51 RVA: 0x00002F04 File Offset: 0x00001104
		protected Expression ChoiceSingleCondition()
		{
			if (base.ParseString("{") == null)
			{
				return null;
			}
			Expression expression = base.Expect(new StringParser.ParseRule(this.Expression), "choice condition inside { }", null) as Expression;
			this.DisallowIncrement(expression);
			base.Expect(base.String("}"), "closing '}' for choice condition", null);
			return expression;
		}

		// Token: 0x06000034 RID: 52 RVA: 0x00002F60 File Offset: 0x00001160
		protected Gather Gather()
		{
			object obj = base.Parse<object>(new StringParser.SpecificParseRule<object>(this.GatherDashes));
			if (obj == null)
			{
				return null;
			}
			int num = (int)obj;
			Gather gather = new Gather(base.Parse<Identifier>(new StringParser.SpecificParseRule<Identifier>(this.BracketedName)), num);
			this.Newline();
			return gather;
		}

		// Token: 0x06000035 RID: 53 RVA: 0x00002FAC File Offset: 0x000011AC
		protected object GatherDashes()
		{
			this.Whitespace();
			int num = 0;
			while (this.ParseDashNotArrow() != null)
			{
				num++;
				this.Whitespace();
			}
			if (num == 0)
			{
				return null;
			}
			return num;
		}

		// Token: 0x06000036 RID: 54 RVA: 0x00002FE4 File Offset: 0x000011E4
		protected object ParseDashNotArrow()
		{
			int num = base.BeginRule();
			if (base.ParseString("->") == null && base.ParseSingleCharacter() == '-')
			{
				return base.SucceedRule(num, null);
			}
			return base.FailRule(num);
		}

		// Token: 0x06000037 RID: 55 RVA: 0x00003020 File Offset: 0x00001220
		protected Identifier BracketedName()
		{
			if (base.ParseString("(") == null)
			{
				return null;
			}
			this.Whitespace();
			Identifier identifier = base.Parse<Identifier>(new StringParser.SpecificParseRule<Identifier>(this.IdentifierWithMetadata));
			if (identifier == null)
			{
				return null;
			}
			this.Whitespace();
			base.Expect(base.String(")"), "closing ')' for bracketed name", null);
			return identifier;
		}

		// Token: 0x06000038 RID: 56 RVA: 0x0000307C File Offset: 0x0000127C
		public CommandLineInput CommandLineUserInput()
		{
			CommandLineInput commandLineInput = new CommandLineInput();
			this.Whitespace();
			if (base.ParseString("help") != null)
			{
				commandLineInput.isHelp = true;
				return commandLineInput;
			}
			if (base.ParseString("exit") != null || base.ParseString("quit") != null)
			{
				commandLineInput.isExit = true;
				return commandLineInput;
			}
			return (CommandLineInput)base.OneOf(new StringParser.ParseRule[]
			{
				new StringParser.ParseRule(this.DebugSource),
				new StringParser.ParseRule(this.DebugPathLookup),
				new StringParser.ParseRule(this.UserChoiceNumber),
				new StringParser.ParseRule(this.UserImmediateModeStatement)
			});
		}

		// Token: 0x06000039 RID: 57 RVA: 0x0000311C File Offset: 0x0000131C
		private CommandLineInput DebugSource()
		{
			this.Whitespace();
			if (base.ParseString("DebugSource") == null)
			{
				return null;
			}
			this.Whitespace();
			string text = "character offset in parentheses, e.g. DebugSource(5)";
			if (base.Expect(base.String("("), text, null) == null)
			{
				return null;
			}
			this.Whitespace();
			int? num = base.ParseInt();
			if (num == null)
			{
				base.Error(text, false);
				return null;
			}
			this.Whitespace();
			base.Expect(base.String(")"), "closing parenthesis", null);
			return new CommandLineInput
			{
				debugSource = num
			};
		}

		// Token: 0x0600003A RID: 58 RVA: 0x000031B0 File Offset: 0x000013B0
		private CommandLineInput DebugPathLookup()
		{
			this.Whitespace();
			if (base.ParseString("DebugPath") == null)
			{
				return null;
			}
			if (this.Whitespace() == null)
			{
				return null;
			}
			string text = base.Expect(new StringParser.ParseRule(this.RuntimePath), "path", null) as string;
			return new CommandLineInput
			{
				debugPathLookup = text
			};
		}

		// Token: 0x0600003B RID: 59 RVA: 0x00003208 File Offset: 0x00001408
		private string RuntimePath()
		{
			if (this._runtimePathCharacterSet == null)
			{
				this._runtimePathCharacterSet = new CharacterSet(this.identifierCharSet);
				this._runtimePathCharacterSet.Add('-');
				this._runtimePathCharacterSet.Add('.');
			}
			return base.ParseCharactersFromCharSet(this._runtimePathCharacterSet, true, -1);
		}

		// Token: 0x0600003C RID: 60 RVA: 0x00003258 File Offset: 0x00001458
		private CommandLineInput UserChoiceNumber()
		{
			this.Whitespace();
			int? num = base.ParseInt();
			if (num == null)
			{
				return null;
			}
			this.Whitespace();
			if (base.Parse<object>(new StringParser.SpecificParseRule<object>(this.EndOfLine)) == null)
			{
				return null;
			}
			return new CommandLineInput
			{
				choiceInput = num
			};
		}

		// Token: 0x0600003D RID: 61 RVA: 0x000032A8 File Offset: 0x000014A8
		private CommandLineInput UserImmediateModeStatement()
		{
			object obj = base.OneOf(new StringParser.ParseRule[]
			{
				new StringParser.ParseRule(this.SingleDivert),
				new StringParser.ParseRule(this.TempDeclarationOrAssignment),
				new StringParser.ParseRule(this.Expression)
			});
			return new CommandLineInput
			{
				userImmediateModeStatement = obj
			};
		}

		// Token: 0x0600003E RID: 62 RVA: 0x000032FC File Offset: 0x000014FC
		protected Conditional InnerConditionalContent()
		{
			Expression initialQueryExpression = base.Parse<Expression>(new StringParser.SpecificParseRule<Expression>(this.ConditionExpression));
			Conditional conditional = base.Parse<Conditional>(() => this.InnerConditionalContent(initialQueryExpression));
			if (conditional == null)
			{
				return null;
			}
			return conditional;
		}

		// Token: 0x0600003F RID: 63 RVA: 0x00003350 File Offset: 0x00001550
		protected Conditional InnerConditionalContent(Expression initialQueryExpression)
		{
			bool flag = initialQueryExpression != null;
			bool flag2 = base.Parse<object>(new StringParser.SpecificParseRule<object>(this.Newline)) == null;
			if (flag2 && !flag)
			{
				return null;
			}
			List<ConditionalSingleBranch> list;
			if (flag2)
			{
				list = this.InlineConditionalBranches();
			}
			else
			{
				list = this.MultilineConditionalBranches();
				if (list == null)
				{
					if (initialQueryExpression)
					{
						List<Ink.Parsed.Object> list2 = this.StatementsAtLevel(InkParser.StatementLevel.InnerBlock);
						if (list2 != null)
						{
							ConditionalSingleBranch conditionalSingleBranch = new ConditionalSingleBranch(list2);
							list = new List<ConditionalSingleBranch>();
							list.Add(conditionalSingleBranch);
							ConditionalSingleBranch conditionalSingleBranch2 = base.Parse<ConditionalSingleBranch>(new StringParser.SpecificParseRule<ConditionalSingleBranch>(this.SingleMultilineCondition));
							if (conditionalSingleBranch2)
							{
								if (!conditionalSingleBranch2.isElse)
								{
									base.ErrorWithParsedObject("Expected an '- else:' clause here rather than an extra condition", conditionalSingleBranch2, false);
									conditionalSingleBranch2.isElse = true;
								}
								list.Add(conditionalSingleBranch2);
							}
						}
					}
					if (list == null)
					{
						return null;
					}
				}
				else if (list.Count == 1 && list[0].isElse && initialQueryExpression)
				{
					list.Insert(0, new ConditionalSingleBranch(null)
					{
						isTrueBranch = true
					});
				}
				if (initialQueryExpression)
				{
					bool flag3 = false;
					for (int i = 0; i < list.Count; i++)
					{
						ConditionalSingleBranch conditionalSingleBranch3 = list[i];
						bool flag4 = i == list.Count - 1;
						if (conditionalSingleBranch3.ownExpression)
						{
							conditionalSingleBranch3.matchingEquality = true;
							flag3 = true;
						}
						else if (flag3 && flag4)
						{
							conditionalSingleBranch3.matchingEquality = true;
							conditionalSingleBranch3.isElse = true;
						}
						else if (!flag4 && list.Count > 2)
						{
							base.ErrorWithParsedObject("Only final branch can be an 'else'. Did you miss a ':'?", conditionalSingleBranch3, false);
						}
						else if (i == 0)
						{
							conditionalSingleBranch3.isTrueBranch = true;
						}
						else
						{
							conditionalSingleBranch3.isElse = true;
						}
					}
				}
				else
				{
					for (int j = 0; j < list.Count; j++)
					{
						ConditionalSingleBranch conditionalSingleBranch4 = list[j];
						bool flag5 = j == list.Count - 1;
						if (conditionalSingleBranch4.ownExpression == null)
						{
							if (flag5)
							{
								conditionalSingleBranch4.isElse = true;
							}
							else if (conditionalSingleBranch4.isElse)
							{
								ConditionalSingleBranch conditionalSingleBranch5 = list[list.Count - 1];
								if (conditionalSingleBranch5.isElse)
								{
									base.ErrorWithParsedObject("Multiple 'else' cases. Can have a maximum of one, at the end.", conditionalSingleBranch5, false);
								}
								else
								{
									base.ErrorWithParsedObject("'else' case in conditional should always be the final one", conditionalSingleBranch4, false);
								}
							}
							else
							{
								base.ErrorWithParsedObject("Branch doesn't have condition. Are you missing a ':'? ", conditionalSingleBranch4, false);
							}
						}
					}
					if (list.Count == 1 && list[0].ownExpression == null)
					{
						base.ErrorWithParsedObject("Condition block with no conditions", list[0], false);
					}
				}
			}
			if (list == null)
			{
				return null;
			}
			foreach (ConditionalSingleBranch conditionalSingleBranch6 in list)
			{
				conditionalSingleBranch6.isInline = flag2;
			}
			return new Conditional(initialQueryExpression, list);
		}

		// Token: 0x06000040 RID: 64 RVA: 0x00003610 File Offset: 0x00001810
		protected List<ConditionalSingleBranch> InlineConditionalBranches()
		{
			List<List<Ink.Parsed.Object>> list = base.Interleave<List<Ink.Parsed.Object>>(new StringParser.ParseRule(this.MixedTextAndLogic), base.Exclude(base.String("|")), null, false);
			if (list == null || list.Count == 0)
			{
				return null;
			}
			List<ConditionalSingleBranch> list2 = new List<ConditionalSingleBranch>();
			if (list.Count > 2)
			{
				base.Error("Expected one or two alternatives separated by '|' in inline conditional", false);
			}
			else
			{
				list2.Add(new ConditionalSingleBranch(list[0])
				{
					isTrueBranch = true
				});
				if (list.Count > 1)
				{
					list2.Add(new ConditionalSingleBranch(list[1])
					{
						isElse = true
					});
				}
			}
			return list2;
		}

		// Token: 0x06000041 RID: 65 RVA: 0x000036B0 File Offset: 0x000018B0
		protected List<ConditionalSingleBranch> MultilineConditionalBranches()
		{
			this.MultilineWhitespace();
			List<object> list = base.OneOrMore(new StringParser.ParseRule(this.SingleMultilineCondition));
			if (list == null)
			{
				return null;
			}
			this.MultilineWhitespace();
			return list.Cast<ConditionalSingleBranch>().ToList<ConditionalSingleBranch>();
		}

		// Token: 0x06000042 RID: 66 RVA: 0x000036F0 File Offset: 0x000018F0
		protected ConditionalSingleBranch SingleMultilineCondition()
		{
			this.Whitespace();
			if (base.ParseString("->") != null)
			{
				return null;
			}
			if (base.ParseString("-") == null)
			{
				return null;
			}
			this.Whitespace();
			Expression expression = null;
			bool flag = base.Parse<object>(new StringParser.SpecificParseRule<object>(this.ElseExpression)) != null;
			if (!flag)
			{
				expression = base.Parse<Expression>(new StringParser.SpecificParseRule<Expression>(this.ConditionExpression));
			}
			List<Ink.Parsed.Object> list = this.StatementsAtLevel(InkParser.StatementLevel.InnerBlock);
			if (expression == null && list == null)
			{
				base.Error("expected content for the conditional branch following '-'", false);
				list = new List<Ink.Parsed.Object>();
				list.Add(new Text(""));
			}
			this.MultilineWhitespace();
			return new ConditionalSingleBranch(list)
			{
				ownExpression = expression,
				isElse = flag
			};
		}

		// Token: 0x06000043 RID: 67 RVA: 0x000037A8 File Offset: 0x000019A8
		protected Expression ConditionExpression()
		{
			Expression expression = base.Parse<Expression>(new StringParser.SpecificParseRule<Expression>(this.Expression));
			if (expression == null)
			{
				return null;
			}
			this.DisallowIncrement(expression);
			this.Whitespace();
			if (base.ParseString(":") == null)
			{
				return null;
			}
			return expression;
		}

		// Token: 0x06000044 RID: 68 RVA: 0x000037F1 File Offset: 0x000019F1
		protected object ElseExpression()
		{
			if (base.ParseString("else") == null)
			{
				return null;
			}
			this.Whitespace();
			if (base.ParseString(":") == null)
			{
				return null;
			}
			return StringParser.ParseSuccess;
		}

		// Token: 0x06000045 RID: 69 RVA: 0x00003820 File Offset: 0x00001A20
		private void TrimEndWhitespace(List<Ink.Parsed.Object> mixedTextAndLogicResults, bool terminateWithSpace)
		{
			if (mixedTextAndLogicResults.Count > 0)
			{
				int num = mixedTextAndLogicResults.Count - 1;
				Ink.Parsed.Object @object = mixedTextAndLogicResults[num];
				if (@object is Text)
				{
					Text text = (Text)@object;
					text.text = text.text.TrimEnd(new char[] { ' ', '\t' });
					if (terminateWithSpace)
					{
						Text text2 = text;
						text2.text += " ";
						return;
					}
					if (text.text.Length == 0)
					{
						mixedTextAndLogicResults.RemoveAt(num);
						this.TrimEndWhitespace(mixedTextAndLogicResults, false);
					}
				}
			}
		}

		// Token: 0x06000046 RID: 70 RVA: 0x000038B0 File Offset: 0x00001AB0
		protected List<Ink.Parsed.Object> LineOfMixedTextAndLogic()
		{
			base.Parse<object>(new StringParser.SpecificParseRule<object>(this.Whitespace));
			List<Ink.Parsed.Object> list = base.Parse<List<Ink.Parsed.Object>>(new StringParser.SpecificParseRule<List<Ink.Parsed.Object>>(this.MixedTextAndLogic));
			bool flag = false;
			List<Ink.Parsed.Tag> list2 = base.Parse<List<Ink.Parsed.Tag>>(new StringParser.SpecificParseRule<List<Ink.Parsed.Tag>>(this.Tags));
			if (list2 != null)
			{
				if (list == null)
				{
					list = list2.Cast<Ink.Parsed.Object>().ToList<Ink.Parsed.Object>();
					flag = true;
				}
				else
				{
					foreach (Ink.Parsed.Tag tag in list2)
					{
						list.Add(tag);
					}
				}
			}
			if (list == null || list.Count == 0)
			{
				return null;
			}
			Text text = list[0] as Text;
			if (text && text.text.StartsWith("return"))
			{
				base.Warning("Do you need a '~' before 'return'? If not, perhaps use a glue: <> (since it's lowercase) or rewrite somehow?");
			}
			if (list.Count == 0)
			{
				return null;
			}
			if (!(list[list.Count - 1] is Ink.Parsed.Divert))
			{
				this.TrimEndWhitespace(list, false);
			}
			if (!flag)
			{
				list.Add(new Text("\n"));
			}
			base.Expect(new StringParser.ParseRule(this.EndOfLine), "end of line", new StringParser.ParseRule(this.SkipToNextLine));
			return list;
		}

		// Token: 0x06000047 RID: 71 RVA: 0x000039F0 File Offset: 0x00001BF0
		protected List<Ink.Parsed.Object> MixedTextAndLogic()
		{
			if (base.ParseObject(this.Spaced(base.String("~"))) != null)
			{
				base.Error("You shouldn't use a '~' here - tildas are for logic that's on its own line. To do inline logic, use { curly braces } instead", false);
			}
			List<Ink.Parsed.Object> list = base.Interleave<Ink.Parsed.Object>(base.Optional(new StringParser.ParseRule(this.ContentText)), base.Optional(new StringParser.ParseRule(this.InlineLogicOrGlue)), null, true);
			if (!this._parsingChoice)
			{
				List<Ink.Parsed.Object> list2 = base.Parse<List<Ink.Parsed.Object>>(new StringParser.SpecificParseRule<List<Ink.Parsed.Object>>(this.MultiDivert));
				if (list2 != null)
				{
					if (list == null)
					{
						list = new List<Ink.Parsed.Object>();
					}
					this.TrimEndWhitespace(list, true);
					list.AddRange(list2);
				}
			}
			if (list == null)
			{
				return null;
			}
			return list;
		}

		// Token: 0x06000048 RID: 72 RVA: 0x00003A8B File Offset: 0x00001C8B
		protected Text ContentText()
		{
			return this.ContentTextAllowingEcapeChar();
		}

		// Token: 0x06000049 RID: 73 RVA: 0x00003A94 File Offset: 0x00001C94
		protected Text ContentTextAllowingEcapeChar()
		{
			StringBuilder stringBuilder = null;
			for (;;)
			{
				string text = base.Parse<string>(new StringParser.SpecificParseRule<string>(this.ContentTextNoEscape));
				bool flag = base.ParseString("\\") != null;
				if (!flag && text == null)
				{
					break;
				}
				if (stringBuilder == null)
				{
					stringBuilder = new StringBuilder();
				}
				if (text != null)
				{
					stringBuilder.Append(text);
				}
				if (flag)
				{
					char c = base.ParseSingleCharacter();
					stringBuilder.Append(c);
				}
			}
			if (stringBuilder != null)
			{
				return new Text(stringBuilder.ToString());
			}
			return null;
		}

		// Token: 0x0600004A RID: 74 RVA: 0x00003B04 File Offset: 0x00001D04
		protected string ContentTextNoEscape()
		{
			if (this._nonTextPauseCharacters == null)
			{
				this._nonTextPauseCharacters = new CharacterSet("-<");
			}
			if (this._nonTextEndCharacters == null)
			{
				this._nonTextEndCharacters = new CharacterSet("{}|\n\r\\#");
				this._notTextEndCharactersChoice = new CharacterSet(this._nonTextEndCharacters);
				this._notTextEndCharactersChoice.AddCharacters("[]");
				this._notTextEndCharactersString = new CharacterSet(this._nonTextEndCharacters);
				this._notTextEndCharactersString.AddCharacters("\"");
			}
			StringParser.ParseRule parseRule = () => base.OneOf(new StringParser.ParseRule[]
			{
				new StringParser.ParseRule(this.ParseDivertArrow),
				new StringParser.ParseRule(this.ParseThreadArrow),
				new StringParser.ParseRule(this.EndOfLine),
				new StringParser.ParseRule(this.Glue)
			});
			CharacterSet characterSet;
			if (this.parsingStringExpression)
			{
				characterSet = this._notTextEndCharactersString;
			}
			else if (this._parsingChoice)
			{
				characterSet = this._notTextEndCharactersChoice;
			}
			else
			{
				characterSet = this._nonTextEndCharacters;
			}
			string text = base.ParseUntil(parseRule, this._nonTextPauseCharacters, characterSet);
			if (text != null)
			{
				return text;
			}
			return null;
		}

		// Token: 0x0600004B RID: 75 RVA: 0x00003BD4 File Offset: 0x00001DD4
		protected List<Ink.Parsed.Object> MultiDivert()
		{
			this.Whitespace();
			Ink.Parsed.Divert divert = base.Parse<Ink.Parsed.Divert>(new StringParser.SpecificParseRule<Ink.Parsed.Divert>(this.StartThread));
			if (divert)
			{
				return new List<Ink.Parsed.Object> { divert };
			}
			List<object> list = base.Interleave<object>(new StringParser.ParseRule(this.ParseDivertArrowOrTunnelOnwards), new StringParser.ParseRule(this.DivertIdentifierWithArguments), null, true);
			if (list == null)
			{
				return null;
			}
			List<Ink.Parsed.Object> list2 = new List<Ink.Parsed.Object>();
			for (int i = 0; i < list.Count; i++)
			{
				if (i % 2 == 0)
				{
					if ((string)list[i] == "->->")
					{
						if (i != 0 && i != list.Count - 1 && i != list.Count - 2)
						{
							base.Error("Tunnel onwards '->->' must only come at the begining or the start of a divert", false);
						}
						TunnelOnwards tunnelOnwards = new TunnelOnwards();
						if (i < list.Count - 1)
						{
							Ink.Parsed.Divert divert2 = list[i + 1] as Ink.Parsed.Divert;
							tunnelOnwards.divertAfter = divert2;
						}
						list2.Add(tunnelOnwards);
						break;
					}
				}
				else
				{
					Ink.Parsed.Divert divert3 = list[i] as Ink.Parsed.Divert;
					if (i < list.Count - 1)
					{
						divert3.isTunnel = true;
					}
					list2.Add(divert3);
				}
			}
			if (list2.Count == 0 && list.Count == 1)
			{
				list2.Add(new Ink.Parsed.Divert(null)
				{
					isEmpty = true
				});
				if (!this._parsingChoice)
				{
					base.Error("Empty diverts (->) are only valid on choices", false);
				}
			}
			return list2;
		}

		// Token: 0x0600004C RID: 76 RVA: 0x00003D40 File Offset: 0x00001F40
		protected Ink.Parsed.Divert StartThread()
		{
			this.Whitespace();
			if (this.ParseThreadArrow() == null)
			{
				return null;
			}
			this.Whitespace();
			Ink.Parsed.Divert divert = base.Expect(new StringParser.ParseRule(this.DivertIdentifierWithArguments), "target for new thread", () => new Ink.Parsed.Divert(null)) as Ink.Parsed.Divert;
			divert.isThread = true;
			return divert;
		}

		// Token: 0x0600004D RID: 77 RVA: 0x00003DA8 File Offset: 0x00001FA8
		protected Ink.Parsed.Divert DivertIdentifierWithArguments()
		{
			this.Whitespace();
			List<Identifier> list = base.Parse<List<Identifier>>(new StringParser.SpecificParseRule<List<Identifier>>(this.DotSeparatedDivertPathComponents));
			if (list == null)
			{
				return null;
			}
			this.Whitespace();
			List<Expression> list2 = base.Parse<List<Expression>>(new StringParser.SpecificParseRule<List<Expression>>(this.ExpressionFunctionCallArguments));
			this.Whitespace();
			return new Ink.Parsed.Divert(new Ink.Parsed.Path(list), list2);
		}

		// Token: 0x0600004E RID: 78 RVA: 0x00003E04 File Offset: 0x00002004
		protected Ink.Parsed.Divert SingleDivert()
		{
			List<Ink.Parsed.Object> list = base.Parse<List<Ink.Parsed.Object>>(new StringParser.SpecificParseRule<List<Ink.Parsed.Object>>(this.MultiDivert));
			if (list == null)
			{
				return null;
			}
			if (list.Count != 1)
			{
				return null;
			}
			if (list[0] is TunnelOnwards)
			{
				return null;
			}
			Ink.Parsed.Divert divert = list[0] as Ink.Parsed.Divert;
			if (divert.isTunnel)
			{
				return null;
			}
			return divert;
		}

		// Token: 0x0600004F RID: 79 RVA: 0x00003E5C File Offset: 0x0000205C
		private List<Identifier> DotSeparatedDivertPathComponents()
		{
			return base.Interleave<Identifier>(this.Spaced(new StringParser.ParseRule(this.IdentifierWithMetadata)), base.Exclude(base.String(".")), null, true);
		}

		// Token: 0x06000050 RID: 80 RVA: 0x00003E8C File Offset: 0x0000208C
		protected string ParseDivertArrowOrTunnelOnwards()
		{
			int num = 0;
			while (base.ParseString("->") != null)
			{
				num++;
			}
			if (num == 0)
			{
				return null;
			}
			if (num == 1)
			{
				return "->";
			}
			if (num == 2)
			{
				return "->->";
			}
			base.Error("Unexpected number of arrows in divert. Should only have '->' or '->->'", false);
			return "->->";
		}

		// Token: 0x06000051 RID: 81 RVA: 0x00003ED8 File Offset: 0x000020D8
		protected string ParseDivertArrow()
		{
			return base.ParseString("->");
		}

		// Token: 0x06000052 RID: 82 RVA: 0x00003EE5 File Offset: 0x000020E5
		protected string ParseThreadArrow()
		{
			return base.ParseString("<-");
		}

		// Token: 0x06000053 RID: 83 RVA: 0x00003EF4 File Offset: 0x000020F4
		protected Ink.Parsed.Object TempDeclarationOrAssignment()
		{
			this.Whitespace();
			bool flag = this.ParseTempKeyword();
			this.Whitespace();
			Identifier identifier;
			if (flag)
			{
				identifier = (Identifier)base.Expect(new StringParser.ParseRule(this.IdentifierWithMetadata), "variable name", null);
			}
			else
			{
				identifier = base.Parse<Identifier>(new StringParser.SpecificParseRule<Identifier>(this.IdentifierWithMetadata));
			}
			if (identifier == null)
			{
				return null;
			}
			this.Whitespace();
			bool flag2 = base.ParseString("+") != null;
			bool flag3 = base.ParseString("-") != null;
			if (flag2 && flag3)
			{
				base.Error("Unexpected sequence '+-'", false);
			}
			if (base.ParseString("=") == null)
			{
				if (flag)
				{
					base.Error("Expected '='", false);
				}
				return null;
			}
			Expression expression = (Expression)base.Expect(new StringParser.ParseRule(this.Expression), "value expression to be assigned", null);
			if (flag2 || flag3)
			{
				return new IncDecExpression(identifier, expression, flag2);
			}
			return new Ink.Parsed.VariableAssignment(identifier, expression)
			{
				isNewTemporaryDeclaration = flag
			};
		}

		// Token: 0x06000054 RID: 84 RVA: 0x00003FE4 File Offset: 0x000021E4
		protected void DisallowIncrement(Ink.Parsed.Object expr)
		{
			if (expr is IncDecExpression)
			{
				base.Error("Can't use increment/decrement here. It can only be used on a ~ line", false);
			}
		}

		// Token: 0x06000055 RID: 85 RVA: 0x00003FFC File Offset: 0x000021FC
		protected bool ParseTempKeyword()
		{
			int num = base.BeginRule();
			if (base.Parse<string>(new StringParser.SpecificParseRule<string>(this.Identifier)) == "temp")
			{
				base.SucceedRule(num, null);
				return true;
			}
			base.FailRule(num);
			return false;
		}

		// Token: 0x06000056 RID: 86 RVA: 0x00004044 File Offset: 0x00002244
		protected Return ReturnStatement()
		{
			this.Whitespace();
			if (base.Parse<string>(new StringParser.SpecificParseRule<string>(this.Identifier)) != "return")
			{
				return null;
			}
			this.Whitespace();
			return new Return(base.Parse<Expression>(new StringParser.SpecificParseRule<Expression>(this.Expression)));
		}

		// Token: 0x06000057 RID: 87 RVA: 0x00004096 File Offset: 0x00002296
		protected Expression Expression()
		{
			return this.Expression(0);
		}

		// Token: 0x06000058 RID: 88 RVA: 0x000040A0 File Offset: 0x000022A0
		protected Expression Expression(int minimumPrecedence)
		{
			this.Whitespace();
			Expression expr = this.ExpressionUnary();
			if (expr == null)
			{
				return null;
			}
			this.Whitespace();
			int num;
			for (;;)
			{
				num = base.BeginRule();
				InkParser.InfixOperator infixOp = this.ParseInfixOperator();
				if (infixOp == null || infixOp.precedence <= minimumPrecedence)
				{
					goto IL_00C5;
				}
				string text = string.Format("right side of '{0}' expression", infixOp.type);
				object obj = base.Expect(() => this.ExpressionInfixRight(expr, infixOp), text, null);
				if (obj == null)
				{
					break;
				}
				expr = base.SucceedRule(num, obj) as Expression;
			}
			base.FailRule(num);
			return null;
			IL_00C5:
			base.FailRule(num);
			this.Whitespace();
			return expr;
		}

		// Token: 0x06000059 RID: 89 RVA: 0x00004188 File Offset: 0x00002388
		protected Expression ExpressionUnary()
		{
			Expression expression = base.Parse<Expression>(new StringParser.SpecificParseRule<Expression>(this.ExpressionDivertTarget));
			if (expression != null)
			{
				return expression;
			}
			string text = (string)base.OneOf(new StringParser.ParseRule[]
			{
				base.String("-"),
				base.String("!")
			});
			if (text == null)
			{
				text = base.Parse<string>(new StringParser.SpecificParseRule<string>(this.ExpressionNot));
			}
			this.Whitespace();
			Expression expression2 = base.OneOf(new StringParser.ParseRule[]
			{
				new StringParser.ParseRule(this.ExpressionList),
				new StringParser.ParseRule(this.ExpressionParen),
				new StringParser.ParseRule(this.ExpressionFunctionCall),
				new StringParser.ParseRule(this.ExpressionVariableName),
				new StringParser.ParseRule(this.ExpressionLiteral)
			}) as Expression;
			if (expression2 == null && text != null)
			{
				expression2 = this.ExpressionUnary();
			}
			if (expression2 == null)
			{
				return null;
			}
			if (text != null)
			{
				expression2 = UnaryExpression.WithInner(expression2, text);
			}
			this.Whitespace();
			string text2 = (string)base.OneOf(new StringParser.ParseRule[]
			{
				base.String("++"),
				base.String("--")
			});
			if (text2 != null)
			{
				bool flag = text2 == "++";
				if (!(expression2 is Ink.Parsed.VariableReference))
				{
					string text3 = "can only increment and decrement variables, but saw '";
					Expression expression3 = expression2;
					base.Error(text3 + ((expression3 != null) ? expression3.ToString() : null) + "'", false);
				}
				else
				{
					expression2 = new IncDecExpression(((Ink.Parsed.VariableReference)expression2).identifier, flag);
				}
			}
			return expression2;
		}

		// Token: 0x0600005A RID: 90 RVA: 0x0000430C File Offset: 0x0000250C
		protected string ExpressionNot()
		{
			string text = this.Identifier();
			if (text == "not")
			{
				return text;
			}
			return null;
		}

		// Token: 0x0600005B RID: 91 RVA: 0x00004330 File Offset: 0x00002530
		protected Expression ExpressionLiteral()
		{
			return (Expression)base.OneOf(new StringParser.ParseRule[]
			{
				new StringParser.ParseRule(this.ExpressionFloat),
				new StringParser.ParseRule(this.ExpressionInt),
				new StringParser.ParseRule(this.ExpressionBool),
				new StringParser.ParseRule(this.ExpressionString)
			});
		}

		// Token: 0x0600005C RID: 92 RVA: 0x0000438C File Offset: 0x0000258C
		protected Expression ExpressionDivertTarget()
		{
			this.Whitespace();
			Ink.Parsed.Divert divert = base.Parse<Ink.Parsed.Divert>(new StringParser.SpecificParseRule<Ink.Parsed.Divert>(this.SingleDivert));
			if (divert == null)
			{
				return null;
			}
			if (divert.isThread)
			{
				return null;
			}
			this.Whitespace();
			return new DivertTarget(divert);
		}

		// Token: 0x0600005D RID: 93 RVA: 0x000043D8 File Offset: 0x000025D8
		protected Number ExpressionInt()
		{
			int? num = base.ParseInt();
			if (num == null)
			{
				return null;
			}
			return new Number(num.Value);
		}

		// Token: 0x0600005E RID: 94 RVA: 0x00004408 File Offset: 0x00002608
		protected Number ExpressionFloat()
		{
			float? num = base.ParseFloat();
			if (num == null)
			{
				return null;
			}
			return new Number(num.Value);
		}

		// Token: 0x0600005F RID: 95 RVA: 0x00004438 File Offset: 0x00002638
		protected StringExpression ExpressionString()
		{
			if (base.ParseString("\"") == null)
			{
				return null;
			}
			this.parsingStringExpression = true;
			List<Ink.Parsed.Object> list = base.Parse<List<Ink.Parsed.Object>>(new StringParser.SpecificParseRule<List<Ink.Parsed.Object>>(this.MixedTextAndLogic));
			base.Expect(base.String("\""), "close quote for string expression", null);
			this.parsingStringExpression = false;
			if (list == null)
			{
				list = new List<Ink.Parsed.Object>();
				list.Add(new Text(""));
			}
			else if (list.Exists((Ink.Parsed.Object c) => c is Ink.Parsed.Divert))
			{
				base.Error("String expressions cannot contain diverts (->)", false);
			}
			return new StringExpression(list);
		}

		// Token: 0x06000060 RID: 96 RVA: 0x000044E4 File Offset: 0x000026E4
		protected Number ExpressionBool()
		{
			string text = base.Parse<string>(new StringParser.SpecificParseRule<string>(this.Identifier));
			if (text == "true")
			{
				return new Number(true);
			}
			if (text == "false")
			{
				return new Number(false);
			}
			return null;
		}

		// Token: 0x06000061 RID: 97 RVA: 0x00004538 File Offset: 0x00002738
		protected Expression ExpressionFunctionCall()
		{
			Identifier identifier = base.Parse<Identifier>(new StringParser.SpecificParseRule<Identifier>(this.IdentifierWithMetadata));
			if (identifier == null)
			{
				return null;
			}
			this.Whitespace();
			List<Expression> list = base.Parse<List<Expression>>(new StringParser.SpecificParseRule<List<Expression>>(this.ExpressionFunctionCallArguments));
			if (list == null)
			{
				return null;
			}
			return new FunctionCall(identifier, list);
		}

		// Token: 0x06000062 RID: 98 RVA: 0x00004584 File Offset: 0x00002784
		protected List<Expression> ExpressionFunctionCallArguments()
		{
			if (base.ParseString("(") == null)
			{
				return null;
			}
			StringParser.ParseRule parseRule = base.Exclude(base.String(","));
			List<Expression> list = base.Interleave<Expression>(new StringParser.ParseRule(this.Expression), parseRule, null, true);
			if (list == null)
			{
				list = new List<Expression>();
			}
			this.Whitespace();
			base.Expect(base.String(")"), "closing ')' for function call", null);
			return list;
		}

		// Token: 0x06000063 RID: 99 RVA: 0x000045F4 File Offset: 0x000027F4
		protected Expression ExpressionVariableName()
		{
			List<Identifier> list = base.Interleave<Identifier>(new StringParser.ParseRule(this.IdentifierWithMetadata), base.Exclude(this.Spaced(base.String("."))), null, true);
			if (list == null || Ink.Parsed.Story.IsReservedKeyword(list[0].name))
			{
				return null;
			}
			return new Ink.Parsed.VariableReference(list);
		}

		// Token: 0x06000064 RID: 100 RVA: 0x0000464C File Offset: 0x0000284C
		protected Expression ExpressionParen()
		{
			if (base.ParseString("(") == null)
			{
				return null;
			}
			Expression expression = base.Parse<Expression>(new StringParser.SpecificParseRule<Expression>(this.Expression));
			if (expression == null)
			{
				return null;
			}
			this.Whitespace();
			base.Expect(base.String(")"), "closing parenthesis ')' for expression", null);
			return expression;
		}

		// Token: 0x06000065 RID: 101 RVA: 0x000046A8 File Offset: 0x000028A8
		protected Expression ExpressionInfixRight(Expression left, InkParser.InfixOperator op)
		{
			this.Whitespace();
			Expression expression = base.Parse<Expression>(() => this.Expression(op.precedence));
			if (expression)
			{
				return new BinaryExpression(left, expression, op.type);
			}
			return null;
		}

		// Token: 0x06000066 RID: 102 RVA: 0x00004700 File Offset: 0x00002900
		private InkParser.InfixOperator ParseInfixOperator()
		{
			foreach (InkParser.InfixOperator infixOperator in this._binaryOperators)
			{
				int num = base.BeginRule();
				if (base.ParseString(infixOperator.type) != null)
				{
					if (!infixOperator.requireWhitespace || this.Whitespace() != null)
					{
						return (InkParser.InfixOperator)base.SucceedRule(num, infixOperator);
					}
					base.FailRule(num);
				}
				else
				{
					base.FailRule(num);
				}
			}
			return null;
		}

		// Token: 0x06000067 RID: 103 RVA: 0x00004798 File Offset: 0x00002998
		protected List ExpressionList()
		{
			this.Whitespace();
			if (base.ParseString("(") == null)
			{
				return null;
			}
			this.Whitespace();
			List<Identifier> list = this.SeparatedList<Identifier>(new StringParser.SpecificParseRule<Identifier>(this.ListMember), this.Spaced(base.String(",")));
			this.Whitespace();
			if (base.ParseString(")") == null)
			{
				return null;
			}
			return new List(list);
		}

		// Token: 0x06000068 RID: 104 RVA: 0x00004804 File Offset: 0x00002A04
		protected Identifier ListMember()
		{
			this.Whitespace();
			Identifier identifier = base.Parse<Identifier>(new StringParser.SpecificParseRule<Identifier>(this.IdentifierWithMetadata));
			if (identifier == null)
			{
				return null;
			}
			if (base.ParseString(".") != null)
			{
				StringParser.ParseRule parseRule = new StringParser.ParseRule(this.IdentifierWithMetadata);
				string text = "element name within the set ";
				Identifier identifier2 = identifier;
				Identifier identifier3 = base.Expect(parseRule, text + ((identifier2 != null) ? identifier2.ToString() : null), null) as Identifier;
				identifier.name = identifier.name + "." + ((identifier3 != null) ? identifier3.name : null);
			}
			this.Whitespace();
			return identifier;
		}

		// Token: 0x06000069 RID: 105 RVA: 0x00004898 File Offset: 0x00002A98
		private void RegisterExpressionOperators()
		{
			this._maxBinaryOpLength = 0;
			this._binaryOperators = new List<InkParser.InfixOperator>();
			this.RegisterBinaryOperator("&&", 1, false);
			this.RegisterBinaryOperator("||", 1, false);
			this.RegisterBinaryOperator("and", 1, true);
			this.RegisterBinaryOperator("or", 1, true);
			this.RegisterBinaryOperator("==", 2, false);
			this.RegisterBinaryOperator(">=", 2, false);
			this.RegisterBinaryOperator("<=", 2, false);
			this.RegisterBinaryOperator("<", 2, false);
			this.RegisterBinaryOperator(">", 2, false);
			this.RegisterBinaryOperator("!=", 2, false);
			this.RegisterBinaryOperator("?", 3, false);
			this.RegisterBinaryOperator("has", 3, true);
			this.RegisterBinaryOperator("!?", 3, false);
			this.RegisterBinaryOperator("hasnt", 3, true);
			this.RegisterBinaryOperator("^", 3, false);
			this.RegisterBinaryOperator("+", 4, false);
			this.RegisterBinaryOperator("-", 5, false);
			this.RegisterBinaryOperator("*", 6, false);
			this.RegisterBinaryOperator("/", 7, false);
			this.RegisterBinaryOperator("%", 8, false);
			this.RegisterBinaryOperator("mod", 8, true);
		}

		// Token: 0x0600006A RID: 106 RVA: 0x000049C8 File Offset: 0x00002BC8
		private void RegisterBinaryOperator(string op, int precedence, bool requireWhitespace = false)
		{
			this._binaryOperators.Add(new InkParser.InfixOperator(op, precedence, requireWhitespace));
			this._maxBinaryOpLength = Math.Max(this._maxBinaryOpLength, op.Length);
		}

		// Token: 0x0600006B RID: 107 RVA: 0x000049F4 File Offset: 0x00002BF4
		protected object IncludeStatement()
		{
			this.Whitespace();
			if (base.ParseString("INCLUDE") == null)
			{
				return null;
			}
			this.Whitespace();
			string text = (string)base.Expect(() => base.ParseUntilCharactersFromString("\n\r", -1), "filename for include statement", null);
			text = text.TrimEnd(new char[] { ' ', '\t' });
			string text2 = this._rootParser._fileHandler.ResolveInkFilename(text);
			if (this.FilenameIsAlreadyOpen(text2))
			{
				base.Error("Recursive INCLUDE detected: '" + text2 + "' is already open.", false);
				base.ParseUntilCharactersFromString("\r\n", -1);
				return new IncludedFile(null);
			}
			this.AddOpenFilename(text2);
			Ink.Parsed.Story story = null;
			string text3 = null;
			try
			{
				text3 = this._rootParser._fileHandler.LoadInkFileContents(text2);
			}
			catch
			{
				base.Error("Failed to load: '" + text + "'", false);
			}
			if (text3 != null)
			{
				story = new InkParser(text3, text, this._externalErrorHandler, this._rootParser, null).Parse();
			}
			this.RemoveOpenFilename(text2);
			return new IncludedFile(story);
		}

		// Token: 0x0600006C RID: 108 RVA: 0x00004B0C File Offset: 0x00002D0C
		private bool FilenameIsAlreadyOpen(string fullFilename)
		{
			return this._rootParser._openFilenames.Contains(fullFilename);
		}

		// Token: 0x0600006D RID: 109 RVA: 0x00004B1F File Offset: 0x00002D1F
		private void AddOpenFilename(string fullFilename)
		{
			this._rootParser._openFilenames.Add(fullFilename);
		}

		// Token: 0x0600006E RID: 110 RVA: 0x00004B33 File Offset: 0x00002D33
		private void RemoveOpenFilename(string fullFilename)
		{
			this._rootParser._openFilenames.Remove(fullFilename);
		}

		// Token: 0x0600006F RID: 111 RVA: 0x00004B48 File Offset: 0x00002D48
		protected Knot KnotDefinition()
		{
			InkParser.FlowDecl flowDecl = base.Parse<InkParser.FlowDecl>(new StringParser.SpecificParseRule<InkParser.FlowDecl>(this.KnotDeclaration));
			if (flowDecl == null)
			{
				return null;
			}
			base.Expect(new StringParser.ParseRule(this.EndOfLine), "end of line after knot name definition", new StringParser.ParseRule(this.SkipToNextLine));
			StringParser.ParseRule parseRule = () => this.StatementsAtLevel(InkParser.StatementLevel.Knot);
			List<Ink.Parsed.Object> list = base.Expect(parseRule, "at least one line within the knot", new StringParser.ParseRule(this.KnotStitchNoContentRecoveryRule)) as List<Ink.Parsed.Object>;
			return new Knot(flowDecl.name, list, flowDecl.arguments, flowDecl.isFunction);
		}

		// Token: 0x06000070 RID: 112 RVA: 0x00004BD4 File Offset: 0x00002DD4
		protected InkParser.FlowDecl KnotDeclaration()
		{
			this.Whitespace();
			if (this.KnotTitleEquals() == null)
			{
				return null;
			}
			this.Whitespace();
			Identifier identifier = base.Parse<Identifier>(new StringParser.SpecificParseRule<Identifier>(this.IdentifierWithMetadata));
			bool flag = ((identifier != null) ? identifier.name : null) == "function";
			Identifier identifier2;
			if (flag)
			{
				base.Expect(new StringParser.ParseRule(this.Whitespace), "whitespace after the 'function' keyword", null);
				identifier2 = base.Parse<Identifier>(new StringParser.SpecificParseRule<Identifier>(this.IdentifierWithMetadata));
			}
			else
			{
				identifier2 = identifier;
			}
			if (identifier2 == null)
			{
				base.Error("Expected the name of the " + (flag ? "function" : "knot"), false);
				identifier2 = new Identifier
				{
					name = ""
				};
			}
			this.Whitespace();
			List<FlowBase.Argument> list = base.Parse<List<FlowBase.Argument>>(new StringParser.SpecificParseRule<List<FlowBase.Argument>>(this.BracketedKnotDeclArguments));
			this.Whitespace();
			base.Parse<string>(new StringParser.SpecificParseRule<string>(this.KnotTitleEquals));
			return new InkParser.FlowDecl
			{
				name = identifier2,
				arguments = list,
				isFunction = flag
			};
		}

		// Token: 0x06000071 RID: 113 RVA: 0x00004CD8 File Offset: 0x00002ED8
		protected string KnotTitleEquals()
		{
			string text = base.ParseCharactersFromString("=", -1);
			if (text == null || text.Length <= 1)
			{
				return null;
			}
			return text;
		}

		// Token: 0x06000072 RID: 114 RVA: 0x00004D04 File Offset: 0x00002F04
		protected object StitchDefinition()
		{
			InkParser.FlowDecl flowDecl = base.Parse<InkParser.FlowDecl>(new StringParser.SpecificParseRule<InkParser.FlowDecl>(this.StitchDeclaration));
			if (flowDecl == null)
			{
				return null;
			}
			base.Expect(new StringParser.ParseRule(this.EndOfLine), "end of line after stitch name", new StringParser.ParseRule(this.SkipToNextLine));
			StringParser.ParseRule parseRule = () => this.StatementsAtLevel(InkParser.StatementLevel.Stitch);
			List<Ink.Parsed.Object> list = base.Expect(parseRule, "at least one line within the stitch", new StringParser.ParseRule(this.KnotStitchNoContentRecoveryRule)) as List<Ink.Parsed.Object>;
			return new Stitch(flowDecl.name, list, flowDecl.arguments, flowDecl.isFunction);
		}

		// Token: 0x06000073 RID: 115 RVA: 0x00004D90 File Offset: 0x00002F90
		protected InkParser.FlowDecl StitchDeclaration()
		{
			this.Whitespace();
			if (base.ParseString("=") == null)
			{
				return null;
			}
			if (base.ParseString("=") != null)
			{
				return null;
			}
			this.Whitespace();
			bool flag = base.ParseString("function") != null;
			if (flag)
			{
				this.Whitespace();
			}
			Identifier identifier = base.Parse<Identifier>(new StringParser.SpecificParseRule<Identifier>(this.IdentifierWithMetadata));
			if (identifier == null)
			{
				return null;
			}
			this.Whitespace();
			List<FlowBase.Argument> list = base.Parse<List<FlowBase.Argument>>(new StringParser.SpecificParseRule<List<FlowBase.Argument>>(this.BracketedKnotDeclArguments));
			this.Whitespace();
			return new InkParser.FlowDecl
			{
				name = identifier,
				arguments = list,
				isFunction = flag
			};
		}

		// Token: 0x06000074 RID: 116 RVA: 0x00004E35 File Offset: 0x00003035
		protected object KnotStitchNoContentRecoveryRule()
		{
			base.ParseUntil(new StringParser.ParseRule(this.KnotDeclaration), new CharacterSet("="), null);
			return new List<Ink.Parsed.Object>
			{
				new Text("<ERROR IN FLOW>")
			};
		}

		// Token: 0x06000075 RID: 117 RVA: 0x00004E6C File Offset: 0x0000306C
		protected List<FlowBase.Argument> BracketedKnotDeclArguments()
		{
			if (base.ParseString("(") == null)
			{
				return null;
			}
			List<FlowBase.Argument> list = base.Interleave<FlowBase.Argument>(this.Spaced(new StringParser.ParseRule(this.FlowDeclArgument)), base.Exclude(base.String(",")), null, true);
			base.Expect(base.String(")"), "closing ')' for parameter list", null);
			if (list == null)
			{
				list = new List<FlowBase.Argument>();
			}
			return list;
		}

		// Token: 0x06000076 RID: 118 RVA: 0x00004ED8 File Offset: 0x000030D8
		protected FlowBase.Argument FlowDeclArgument()
		{
			Identifier identifier = base.Parse<Identifier>(new StringParser.SpecificParseRule<Identifier>(this.IdentifierWithMetadata));
			this.Whitespace();
			string text = this.ParseDivertArrow();
			this.Whitespace();
			Identifier identifier2 = base.Parse<Identifier>(new StringParser.SpecificParseRule<Identifier>(this.IdentifierWithMetadata));
			if (identifier == null && identifier2 == null)
			{
				return null;
			}
			FlowBase.Argument argument = new FlowBase.Argument();
			if (text != null)
			{
				argument.isDivertTarget = true;
			}
			if (identifier != null && identifier.name == "ref")
			{
				if (identifier2 == null)
				{
					base.Error("Expected an parameter name after 'ref'", false);
				}
				argument.identifier = identifier2;
				argument.isByReference = true;
			}
			else
			{
				if (argument.isDivertTarget)
				{
					argument.identifier = identifier2;
				}
				else
				{
					argument.identifier = identifier;
				}
				if (argument.identifier == null)
				{
					base.Error("Expected an parameter name", false);
				}
				argument.isByReference = false;
			}
			return argument;
		}

		// Token: 0x06000077 RID: 119 RVA: 0x00004FA0 File Offset: 0x000031A0
		protected ExternalDeclaration ExternalDeclaration()
		{
			this.Whitespace();
			Identifier identifier = base.Parse<Identifier>(new StringParser.SpecificParseRule<Identifier>(this.IdentifierWithMetadata));
			if (identifier == null || identifier.name != "EXTERNAL")
			{
				return null;
			}
			this.Whitespace();
			Identifier identifier2 = (base.Expect(new StringParser.ParseRule(this.IdentifierWithMetadata), "name of external function", null) as Identifier) ?? new Identifier();
			this.Whitespace();
			StringParser.ParseRule parseRule = new StringParser.ParseRule(this.BracketedKnotDeclArguments);
			string text = "declaration of arguments for EXTERNAL, even if empty, i.e. 'EXTERNAL ";
			Identifier identifier3 = identifier2;
			List<FlowBase.Argument> list = base.Expect(parseRule, text + ((identifier3 != null) ? identifier3.ToString() : null) + "()'", null) as List<FlowBase.Argument>;
			if (list == null)
			{
				list = new List<FlowBase.Argument>();
			}
			List<string> list2 = list.Select(delegate(FlowBase.Argument arg)
			{
				Identifier identifier4 = arg.identifier;
				if (identifier4 == null)
				{
					return null;
				}
				return identifier4.name;
			}).ToList<string>();
			return new ExternalDeclaration(identifier2, list2);
		}

		// Token: 0x06000078 RID: 120 RVA: 0x00005084 File Offset: 0x00003284
		protected Ink.Parsed.Object LogicLine()
		{
			this.Whitespace();
			if (base.ParseString("~") == null)
			{
				return null;
			}
			this.Whitespace();
			StringParser.ParseRule parseRule = () => base.OneOf(new StringParser.ParseRule[]
			{
				new StringParser.ParseRule(this.ReturnStatement),
				new StringParser.ParseRule(this.TempDeclarationOrAssignment),
				new StringParser.ParseRule(this.Expression)
			});
			Ink.Parsed.Object @object = base.Expect(parseRule, "expression after '~'", new StringParser.ParseRule(this.SkipToNextLine)) as Ink.Parsed.Object;
			if (@object == null)
			{
				return new ContentList();
			}
			if (@object is Expression && !(@object is FunctionCall) && !(@object is IncDecExpression))
			{
				Ink.Parsed.VariableReference variableReference = @object as Ink.Parsed.VariableReference;
				if (variableReference && variableReference.name == "include")
				{
					base.Error("'~ include' is no longer the correct syntax - please use 'INCLUDE your_filename.ink', without the tilda, and in block capitals.", false);
				}
				else
				{
					base.Error("Logic following a '~' can't be that type of expression. It can only be something like:\n\t~ return\n\t~ var x = blah\n\t~ x++\n\t~ myFunction()", false);
				}
			}
			FunctionCall functionCall = @object as FunctionCall;
			if (functionCall)
			{
				functionCall.shouldPopReturnedValue = true;
			}
			if (@object.Find<FunctionCall>(null) != null)
			{
				@object = new ContentList(new Ink.Parsed.Object[]
				{
					@object,
					new Text("\n")
				});
			}
			base.Expect(new StringParser.ParseRule(this.EndOfLine), "end of line", new StringParser.ParseRule(this.SkipToNextLine));
			return @object;
		}

		// Token: 0x06000079 RID: 121 RVA: 0x000051A4 File Offset: 0x000033A4
		protected Ink.Parsed.Object VariableDeclaration()
		{
			this.Whitespace();
			if (base.Parse<string>(new StringParser.SpecificParseRule<string>(this.Identifier)) != "VAR")
			{
				return null;
			}
			this.Whitespace();
			Identifier identifier = base.Expect(new StringParser.ParseRule(this.IdentifierWithMetadata), "variable name", null) as Identifier;
			this.Whitespace();
			base.Expect(base.String("="), "the '=' for an assignment of a value, e.g. '= 5' (initial values are mandatory)", null);
			this.Whitespace();
			Expression expression = base.Expect(new StringParser.ParseRule(this.Expression), "initial value for ", null) as Expression;
			if (expression)
			{
				if (!(expression is Number) && !(expression is StringExpression) && !(expression is DivertTarget) && !(expression is Ink.Parsed.VariableReference) && !(expression is List))
				{
					base.Error("initial value for a variable must be a number, constant, list or divert target", false);
				}
				if (base.Parse<string>(new StringParser.SpecificParseRule<string>(this.ListElementDefinitionSeparator)) != null)
				{
					base.Error("Unexpected ','. If you're trying to declare a new list, use the LIST keyword, not VAR", false);
				}
				else if (expression is StringExpression && !(expression as StringExpression).isSingleString)
				{
					base.Error("Constant strings cannot contain any logic.", false);
				}
				return new Ink.Parsed.VariableAssignment(identifier, expression)
				{
					isGlobalDeclaration = true
				};
			}
			return null;
		}

		// Token: 0x0600007A RID: 122 RVA: 0x000052D4 File Offset: 0x000034D4
		protected Ink.Parsed.VariableAssignment ListDeclaration()
		{
			this.Whitespace();
			if (base.Parse<string>(new StringParser.SpecificParseRule<string>(this.Identifier)) != "LIST")
			{
				return null;
			}
			this.Whitespace();
			Identifier identifier = base.Expect(new StringParser.ParseRule(this.IdentifierWithMetadata), "list name", null) as Identifier;
			this.Whitespace();
			base.Expect(base.String("="), "the '=' for an assignment of the list definition", null);
			this.Whitespace();
			Ink.Parsed.ListDefinition listDefinition = base.Expect(new StringParser.ParseRule(this.ListDefinition), "list item names", null) as Ink.Parsed.ListDefinition;
			if (listDefinition)
			{
				listDefinition.identifier = identifier;
				return new Ink.Parsed.VariableAssignment(identifier, listDefinition);
			}
			return null;
		}

		// Token: 0x0600007B RID: 123 RVA: 0x0000538C File Offset: 0x0000358C
		protected Ink.Parsed.ListDefinition ListDefinition()
		{
			this.AnyWhitespace();
			List<ListElementDefinition> list = this.SeparatedList<ListElementDefinition>(new StringParser.SpecificParseRule<ListElementDefinition>(this.ListElementDefinition), new StringParser.ParseRule(this.ListElementDefinitionSeparator));
			if (list == null)
			{
				return null;
			}
			return new Ink.Parsed.ListDefinition(list);
		}

		// Token: 0x0600007C RID: 124 RVA: 0x000053CA File Offset: 0x000035CA
		protected string ListElementDefinitionSeparator()
		{
			this.AnyWhitespace();
			if (base.ParseString(",") == null)
			{
				return null;
			}
			this.AnyWhitespace();
			return ",";
		}

		// Token: 0x0600007D RID: 125 RVA: 0x000053F0 File Offset: 0x000035F0
		protected ListElementDefinition ListElementDefinition()
		{
			bool flag = base.ParseString("(") != null;
			bool flag2 = flag;
			this.Whitespace();
			Identifier identifier = base.Parse<Identifier>(new StringParser.SpecificParseRule<Identifier>(this.IdentifierWithMetadata));
			if (identifier == null)
			{
				return null;
			}
			this.Whitespace();
			if (flag && base.ParseString(")") != null)
			{
				flag2 = false;
				this.Whitespace();
			}
			int? num = null;
			if (base.ParseString("=") != null)
			{
				this.Whitespace();
				Number number = base.Expect(new StringParser.ParseRule(this.ExpressionInt), "value to be assigned to list item", null) as Number;
				if (number != null)
				{
					num = new int?((int)number.value);
				}
				if (flag2)
				{
					this.Whitespace();
					if (base.ParseString(")") != null)
					{
						flag2 = false;
					}
				}
			}
			if (flag2)
			{
				base.Error("Expected closing ')'", false);
			}
			return new ListElementDefinition(identifier, flag, num);
		}

		// Token: 0x0600007E RID: 126 RVA: 0x000054D8 File Offset: 0x000036D8
		protected Ink.Parsed.Object ConstDeclaration()
		{
			this.Whitespace();
			if (base.Parse<string>(new StringParser.SpecificParseRule<string>(this.Identifier)) != "CONST")
			{
				return null;
			}
			this.Whitespace();
			Identifier identifier = base.Expect(new StringParser.ParseRule(this.IdentifierWithMetadata), "constant name", null) as Identifier;
			this.Whitespace();
			base.Expect(base.String("="), "the '=' for an assignment of a value, e.g. '= 5' (initial values are mandatory)", null);
			this.Whitespace();
			Expression expression = base.Expect(new StringParser.ParseRule(this.Expression), "initial value for ", null) as Expression;
			if (!(expression is Number) && !(expression is DivertTarget) && !(expression is StringExpression))
			{
				base.Error("initial value for a constant must be a number or divert target", false);
			}
			else if (expression is StringExpression && !(expression as StringExpression).isSingleString)
			{
				base.Error("Constant strings cannot contain any logic.", false);
			}
			return new ConstantDeclaration(identifier, expression);
		}

		// Token: 0x0600007F RID: 127 RVA: 0x000055C1 File Offset: 0x000037C1
		protected Ink.Parsed.Object InlineLogicOrGlue()
		{
			return (Ink.Parsed.Object)base.OneOf(new StringParser.ParseRule[]
			{
				new StringParser.ParseRule(this.InlineLogic),
				new StringParser.ParseRule(this.Glue)
			});
		}

		// Token: 0x06000080 RID: 128 RVA: 0x000055F2 File Offset: 0x000037F2
		protected Ink.Parsed.Glue Glue()
		{
			if (base.ParseString("<>") != null)
			{
				return new Ink.Parsed.Glue(new Ink.Runtime.Glue());
			}
			return null;
		}

		// Token: 0x06000081 RID: 129 RVA: 0x00005610 File Offset: 0x00003810
		protected Ink.Parsed.Object InlineLogic()
		{
			if (base.ParseString("{") == null)
			{
				return null;
			}
			this.Whitespace();
			Ink.Parsed.Object @object = (Ink.Parsed.Object)base.Expect(new StringParser.ParseRule(this.InnerLogic), "some kind of logic, conditional or sequence within braces: { ... }", null);
			if (@object == null)
			{
				return null;
			}
			this.DisallowIncrement(@object);
			ContentList contentList = @object as ContentList;
			if (!contentList)
			{
				contentList = new ContentList(new Ink.Parsed.Object[] { @object });
			}
			this.Whitespace();
			base.Expect(base.String("}"), "closing brace '}' for inline logic", null);
			return contentList;
		}

		// Token: 0x06000082 RID: 130 RVA: 0x000056A4 File Offset: 0x000038A4
		protected Ink.Parsed.Object InnerLogic()
		{
			this.Whitespace();
			SequenceType? sequenceType = (SequenceType?)base.ParseObject(new StringParser.ParseRule(this.SequenceTypeAnnotation));
			if (sequenceType != null)
			{
				List<ContentList> list = (List<ContentList>)base.Expect(new StringParser.ParseRule(this.InnerSequenceObjects), "sequence elements (for cycle/stoping etc)", null);
				if (list == null)
				{
					return null;
				}
				return new Sequence(list, sequenceType.Value);
			}
			else
			{
				Expression initialQueryExpression = base.Parse<Expression>(new StringParser.SpecificParseRule<Expression>(this.ConditionExpression));
				if (initialQueryExpression)
				{
					return (Conditional)base.Expect(() => this.InnerConditionalContent(initialQueryExpression), "conditional content following query", null);
				}
				foreach (StringParser.ParseRule parseRule in new StringParser.ParseRule[]
				{
					new StringParser.ParseRule(this.InnerConditionalContent),
					new StringParser.ParseRule(this.InnerSequence),
					new StringParser.ParseRule(this.InnerExpression)
				})
				{
					int num = base.BeginRule();
					Ink.Parsed.Object @object = base.ParseObject(parseRule) as Ink.Parsed.Object;
					if (@object)
					{
						if (base.Peek(this.Spaced(base.String("}"))) != null)
						{
							return (Ink.Parsed.Object)base.SucceedRule(num, @object);
						}
						base.FailRule(num);
					}
					else
					{
						base.FailRule(num);
					}
				}
				return null;
			}
		}

		// Token: 0x06000083 RID: 131 RVA: 0x00005804 File Offset: 0x00003A04
		protected Ink.Parsed.Object InnerExpression()
		{
			Expression expression = base.Parse<Expression>(new StringParser.SpecificParseRule<Expression>(this.Expression));
			if (expression)
			{
				expression.outputWhenComplete = true;
			}
			return expression;
		}

		// Token: 0x06000084 RID: 132 RVA: 0x00005834 File Offset: 0x00003A34
		protected Identifier IdentifierWithMetadata()
		{
			string text = this.Identifier();
			if (text == null)
			{
				return null;
			}
			return new Identifier
			{
				name = text,
				debugMetadata = null
			};
		}

		// Token: 0x06000085 RID: 133 RVA: 0x00005860 File Offset: 0x00003A60
		protected string Identifier()
		{
			string text = base.ParseCharactersFromCharSet(this.identifierCharSet, true, -1);
			if (text == null)
			{
				return null;
			}
			bool flag = true;
			foreach (char c in text)
			{
				if (c < '0' || c > '9')
				{
					flag = false;
					break;
				}
			}
			if (flag)
			{
				return null;
			}
			return text;
		}

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x06000086 RID: 134 RVA: 0x000058B8 File Offset: 0x00003AB8
		private CharacterSet identifierCharSet
		{
			get
			{
				if (this._identifierCharSet == null)
				{
					(this._identifierCharSet = new CharacterSet()).AddRange('A', 'Z').AddRange('a', 'z').AddRange('0', '9')
						.Add('_');
					this.ExtendIdentifierCharacterRanges(this._identifierCharSet);
				}
				return this._identifierCharSet;
			}
		}

		// Token: 0x06000087 RID: 135 RVA: 0x00005910 File Offset: 0x00003B10
		protected Sequence InnerSequence()
		{
			this.Whitespace();
			SequenceType sequenceType = SequenceType.Stopping;
			SequenceType? sequenceType2 = (SequenceType?)base.Parse<object>(new StringParser.SpecificParseRule<object>(this.SequenceTypeAnnotation));
			if (sequenceType2 != null)
			{
				sequenceType = sequenceType2.Value;
			}
			List<ContentList> list = base.Parse<List<ContentList>>(new StringParser.SpecificParseRule<List<ContentList>>(this.InnerSequenceObjects));
			if (list == null || list.Count <= 1)
			{
				return null;
			}
			return new Sequence(list, sequenceType);
		}

		// Token: 0x06000088 RID: 136 RVA: 0x00005978 File Offset: 0x00003B78
		protected object SequenceTypeAnnotation()
		{
			SequenceType? sequenceType = (SequenceType?)base.Parse<object>(new StringParser.SpecificParseRule<object>(this.SequenceTypeSymbolAnnotation));
			if (sequenceType == null)
			{
				sequenceType = (SequenceType?)base.Parse<object>(new StringParser.SpecificParseRule<object>(this.SequenceTypeWordAnnotation));
			}
			if (sequenceType == null)
			{
				return null;
			}
			SequenceType value = sequenceType.Value;
			switch (value)
			{
			case SequenceType.Stopping:
			case SequenceType.Cycle:
			case SequenceType.Shuffle:
			case SequenceType.Stopping | SequenceType.Shuffle:
			case SequenceType.Once:
				goto IL_00A6;
			case SequenceType.Stopping | SequenceType.Cycle:
			case SequenceType.Cycle | SequenceType.Shuffle:
			case SequenceType.Stopping | SequenceType.Cycle | SequenceType.Shuffle:
				break;
			default:
				if (value == (SequenceType.Shuffle | SequenceType.Once))
				{
					goto IL_00A6;
				}
				break;
			}
			base.Error("Sequence type combination not supported: " + sequenceType.Value.ToString(), false);
			return SequenceType.Stopping;
			IL_00A6:
			return sequenceType;
		}

		// Token: 0x06000089 RID: 137 RVA: 0x00005A34 File Offset: 0x00003C34
		protected object SequenceTypeSymbolAnnotation()
		{
			if (this._sequenceTypeSymbols == null)
			{
				this._sequenceTypeSymbols = new CharacterSet("!&~$ ");
			}
			SequenceType sequenceType = (SequenceType)0;
			string text = base.ParseCharactersFromCharSet(this._sequenceTypeSymbols, true, -1);
			if (text == null)
			{
				return null;
			}
			foreach (char c in text)
			{
				if (c <= '$')
				{
					if (c != '!')
					{
						if (c == '$')
						{
							sequenceType |= SequenceType.Stopping;
						}
					}
					else
					{
						sequenceType |= SequenceType.Once;
					}
				}
				else if (c != '&')
				{
					if (c == '~')
					{
						sequenceType |= SequenceType.Shuffle;
					}
				}
				else
				{
					sequenceType |= SequenceType.Cycle;
				}
			}
			if (sequenceType == (SequenceType)0)
			{
				return null;
			}
			return sequenceType;
		}

		// Token: 0x0600008A RID: 138 RVA: 0x00005AD0 File Offset: 0x00003CD0
		protected object SequenceTypeWordAnnotation()
		{
			List<SequenceType?> list = base.Interleave<SequenceType?>(new StringParser.ParseRule(this.SequenceTypeSingleWord), base.Exclude(new StringParser.ParseRule(this.Whitespace)), null, true);
			if (list == null || list.Count == 0)
			{
				return null;
			}
			if (base.ParseString(":") == null)
			{
				return null;
			}
			SequenceType sequenceType = (SequenceType)0;
			foreach (SequenceType? sequenceType2 in list)
			{
				sequenceType |= sequenceType2.Value;
			}
			return sequenceType;
		}

		// Token: 0x0600008B RID: 139 RVA: 0x00005B6C File Offset: 0x00003D6C
		protected object SequenceTypeSingleWord()
		{
			SequenceType? sequenceType = null;
			Identifier identifier = base.Parse<Identifier>(new StringParser.SpecificParseRule<Identifier>(this.IdentifierWithMetadata));
			if (identifier != null)
			{
				string name = identifier.name;
				if (!(name == "once"))
				{
					if (!(name == "cycle"))
					{
						if (!(name == "shuffle"))
						{
							if (name == "stopping")
							{
								sequenceType = new SequenceType?(SequenceType.Stopping);
							}
						}
						else
						{
							sequenceType = new SequenceType?(SequenceType.Shuffle);
						}
					}
					else
					{
						sequenceType = new SequenceType?(SequenceType.Cycle);
					}
				}
				else
				{
					sequenceType = new SequenceType?(SequenceType.Once);
				}
			}
			if (sequenceType == null)
			{
				return null;
			}
			return sequenceType;
		}

		// Token: 0x0600008C RID: 140 RVA: 0x00005C0C File Offset: 0x00003E0C
		protected List<ContentList> InnerSequenceObjects()
		{
			List<ContentList> list;
			if (base.Parse<object>(new StringParser.SpecificParseRule<object>(this.Newline)) != null)
			{
				list = base.Parse<List<ContentList>>(new StringParser.SpecificParseRule<List<ContentList>>(this.InnerMultilineSequenceObjects));
			}
			else
			{
				list = base.Parse<List<ContentList>>(new StringParser.SpecificParseRule<List<ContentList>>(this.InnerInlineSequenceObjects));
			}
			return list;
		}

		// Token: 0x0600008D RID: 141 RVA: 0x00005C5C File Offset: 0x00003E5C
		protected List<ContentList> InnerInlineSequenceObjects()
		{
			List<object> list = base.Interleave<object>(base.Optional(new StringParser.ParseRule(this.MixedTextAndLogic)), base.String("|"), null, false);
			if (list == null)
			{
				return null;
			}
			List<ContentList> list2 = new List<ContentList>();
			bool flag = false;
			foreach (object obj in list)
			{
				if (obj as string == "|")
				{
					if (!flag)
					{
						list2.Add(new ContentList());
					}
					flag = false;
				}
				else
				{
					List<Ink.Parsed.Object> list3 = obj as List<Ink.Parsed.Object>;
					if (list3 == null)
					{
						base.Error("Expected content, but got " + ((obj != null) ? obj.ToString() : null) + " (this is an ink compiler bug!)", false);
					}
					else
					{
						list2.Add(new ContentList(list3));
					}
					flag = true;
				}
			}
			if (!flag)
			{
				list2.Add(new ContentList());
			}
			return list2;
		}

		// Token: 0x0600008E RID: 142 RVA: 0x00005D50 File Offset: 0x00003F50
		protected List<ContentList> InnerMultilineSequenceObjects()
		{
			this.MultilineWhitespace();
			List<object> list = base.OneOrMore(new StringParser.ParseRule(this.SingleMultilineSequenceElement));
			if (list == null)
			{
				return null;
			}
			return list.Cast<ContentList>().ToList<ContentList>();
		}

		// Token: 0x0600008F RID: 143 RVA: 0x00005D88 File Offset: 0x00003F88
		protected ContentList SingleMultilineSequenceElement()
		{
			this.Whitespace();
			if (base.ParseString("->") != null)
			{
				return null;
			}
			if (base.ParseString("-") == null)
			{
				return null;
			}
			this.Whitespace();
			List<Ink.Parsed.Object> list = this.StatementsAtLevel(InkParser.StatementLevel.InnerBlock);
			if (list == null)
			{
				this.MultilineWhitespace();
			}
			else
			{
				list.Insert(0, new Text("\n"));
			}
			return new ContentList(list);
		}

		// Token: 0x06000090 RID: 144 RVA: 0x00005DEC File Offset: 0x00003FEC
		protected List<Ink.Parsed.Object> StatementsAtLevel(InkParser.StatementLevel level)
		{
			if (level == InkParser.StatementLevel.InnerBlock && base.Parse<object>(new StringParser.SpecificParseRule<object>(this.GatherDashes)) != null)
			{
				base.Error("You can't use a gather (the dashes) within the { curly braces } context. For multi-line sequences and conditions, you should only use one dash.", false);
			}
			return base.Interleave<Ink.Parsed.Object>(base.Optional(new StringParser.ParseRule(this.MultilineWhitespace)), () => this.StatementAtLevel(level), () => this.StatementsBreakForLevel(level), true);
		}

		// Token: 0x06000091 RID: 145 RVA: 0x00005E68 File Offset: 0x00004068
		protected object StatementAtLevel(InkParser.StatementLevel level)
		{
			StringParser.ParseRule[] array = this._statementRulesAtLevel[(int)level];
			object obj = base.OneOf(array);
			if (level == InkParser.StatementLevel.Top && obj is Return)
			{
				base.Error("should not have return statement outside of a knot", false);
			}
			return obj;
		}

		// Token: 0x06000092 RID: 146 RVA: 0x00005EA0 File Offset: 0x000040A0
		protected object StatementsBreakForLevel(InkParser.StatementLevel level)
		{
			this.Whitespace();
			StringParser.ParseRule[] array = this._statementBreakRulesAtLevel[(int)level];
			object obj = base.OneOf(array);
			if (obj == null)
			{
				return null;
			}
			return obj;
		}

		// Token: 0x06000093 RID: 147 RVA: 0x00005ECC File Offset: 0x000040CC
		private void GenerateStatementLevelRules()
		{
			List<InkParser.StatementLevel> list = Enum.GetValues(typeof(InkParser.StatementLevel)).Cast<InkParser.StatementLevel>().ToList<InkParser.StatementLevel>();
			this._statementRulesAtLevel = new StringParser.ParseRule[list.Count][];
			this._statementBreakRulesAtLevel = new StringParser.ParseRule[list.Count][];
			foreach (InkParser.StatementLevel statementLevel in list)
			{
				List<StringParser.ParseRule> list2 = new List<StringParser.ParseRule>();
				List<StringParser.ParseRule> list3 = new List<StringParser.ParseRule>();
				list2.Add(this.Line(new StringParser.ParseRule(this.MultiDivert)));
				if (statementLevel >= InkParser.StatementLevel.Top)
				{
					list2.Add(new StringParser.ParseRule(this.KnotDefinition));
				}
				list2.Add(this.Line(new StringParser.ParseRule(this.Choice)));
				list2.Add(this.Line(new StringParser.ParseRule(this.AuthorWarning)));
				if (statementLevel > InkParser.StatementLevel.InnerBlock)
				{
					list2.Add(new StringParser.ParseRule(this.Gather));
				}
				if (statementLevel >= InkParser.StatementLevel.Knot)
				{
					list2.Add(new StringParser.ParseRule(this.StitchDefinition));
				}
				list2.Add(this.Line(new StringParser.ParseRule(this.ListDeclaration)));
				list2.Add(this.Line(new StringParser.ParseRule(this.VariableDeclaration)));
				list2.Add(this.Line(new StringParser.ParseRule(this.ConstDeclaration)));
				list2.Add(this.Line(new StringParser.ParseRule(this.ExternalDeclaration)));
				list2.Add(this.Line(new StringParser.ParseRule(this.IncludeStatement)));
				list2.Add(new StringParser.ParseRule(this.LogicLine));
				list2.Add(new StringParser.ParseRule(this.LineOfMixedTextAndLogic));
				if (statementLevel <= InkParser.StatementLevel.Knot)
				{
					list3.Add(new StringParser.ParseRule(this.KnotDeclaration));
				}
				if (statementLevel <= InkParser.StatementLevel.Stitch)
				{
					list3.Add(new StringParser.ParseRule(this.StitchDeclaration));
				}
				if (statementLevel <= InkParser.StatementLevel.InnerBlock)
				{
					list3.Add(new StringParser.ParseRule(this.ParseDashNotArrow));
					list3.Add(base.String("}"));
				}
				this._statementRulesAtLevel[(int)statementLevel] = list2.ToArray();
				this._statementBreakRulesAtLevel[(int)statementLevel] = list3.ToArray();
			}
		}

		// Token: 0x06000094 RID: 148 RVA: 0x00006108 File Offset: 0x00004308
		protected object SkipToNextLine()
		{
			base.ParseUntilCharactersFromString("\n\r", -1);
			base.ParseNewline();
			return StringParser.ParseSuccess;
		}

		// Token: 0x06000095 RID: 149 RVA: 0x00006123 File Offset: 0x00004323
		protected StringParser.ParseRule Line(StringParser.ParseRule inlineRule)
		{
			return delegate
			{
				object obj = this.ParseObject(inlineRule);
				if (obj == null)
				{
					return null;
				}
				this.Expect(new StringParser.ParseRule(this.EndOfLine), "end of line", new StringParser.ParseRule(this.SkipToNextLine));
				return obj;
			};
		}

		// Token: 0x06000096 RID: 150 RVA: 0x00006144 File Offset: 0x00004344
		protected Ink.Parsed.Tag Tag()
		{
			this.Whitespace();
			if (base.ParseString("#") == null)
			{
				return null;
			}
			this.Whitespace();
			StringBuilder stringBuilder = new StringBuilder();
			for (;;)
			{
				string text = base.ParseUntilCharactersFromCharSet(this._endOfTagCharSet, -1);
				stringBuilder.Append(text);
				if (base.ParseString("\\") == null)
				{
					break;
				}
				char c = base.ParseSingleCharacter();
				if (c != '\0')
				{
					stringBuilder.Append(c);
				}
			}
			return new Ink.Parsed.Tag(new Ink.Runtime.Tag(stringBuilder.ToString().Trim()));
		}

		// Token: 0x06000097 RID: 151 RVA: 0x000061C0 File Offset: 0x000043C0
		protected List<Ink.Parsed.Tag> Tags()
		{
			List<object> list = base.OneOrMore(new StringParser.ParseRule(this.Tag));
			if (list == null)
			{
				return null;
			}
			return list.Cast<Ink.Parsed.Tag>().ToList<Ink.Parsed.Tag>();
		}

		// Token: 0x06000098 RID: 152 RVA: 0x000061F0 File Offset: 0x000043F0
		protected object EndOfLine()
		{
			return base.OneOf(new StringParser.ParseRule[]
			{
				new StringParser.ParseRule(this.Newline),
				new StringParser.ParseRule(this.EndOfFile)
			});
		}

		// Token: 0x06000099 RID: 153 RVA: 0x0000621C File Offset: 0x0000441C
		protected object Newline()
		{
			this.Whitespace();
			if (base.ParseNewline() == null)
			{
				return null;
			}
			return StringParser.ParseSuccess;
		}

		// Token: 0x0600009A RID: 154 RVA: 0x00006237 File Offset: 0x00004437
		protected object EndOfFile()
		{
			this.Whitespace();
			if (!base.endOfInput)
			{
				return null;
			}
			return StringParser.ParseSuccess;
		}

		// Token: 0x0600009B RID: 155 RVA: 0x00006250 File Offset: 0x00004450
		protected object MultilineWhitespace()
		{
			List<object> list = base.OneOrMore(new StringParser.ParseRule(this.Newline));
			if (list == null)
			{
				return null;
			}
			if (list.Count >= 1)
			{
				return StringParser.ParseSuccess;
			}
			return null;
		}

		// Token: 0x0600009C RID: 156 RVA: 0x00006285 File Offset: 0x00004485
		protected object Whitespace()
		{
			if (base.ParseCharactersFromCharSet(this._inlineWhitespaceChars, true, -1) != null)
			{
				return StringParser.ParseSuccess;
			}
			return null;
		}

		// Token: 0x0600009D RID: 157 RVA: 0x0000629E File Offset: 0x0000449E
		protected StringParser.ParseRule Spaced(StringParser.ParseRule rule)
		{
			return delegate
			{
				this.Whitespace();
				object obj = this.ParseObject(rule);
				if (obj == null)
				{
					return null;
				}
				this.Whitespace();
				return obj;
			};
		}

		// Token: 0x0600009E RID: 158 RVA: 0x000062C0 File Offset: 0x000044C0
		protected object AnyWhitespace()
		{
			bool flag = false;
			while (base.OneOf(new StringParser.ParseRule[]
			{
				new StringParser.ParseRule(this.Whitespace),
				new StringParser.ParseRule(this.MultilineWhitespace)
			}) != null)
			{
				flag = true;
			}
			if (!flag)
			{
				return null;
			}
			return StringParser.ParseSuccess;
		}

		// Token: 0x0600009F RID: 159 RVA: 0x00006309 File Offset: 0x00004509
		protected StringParser.ParseRule MultiSpaced(StringParser.ParseRule rule)
		{
			return delegate
			{
				this.AnyWhitespace();
				object obj = this.ParseObject(rule);
				if (obj == null)
				{
					return null;
				}
				this.AnyWhitespace();
				return obj;
			};
		}

		// Token: 0x04000016 RID: 22
		private IFileHandler _fileHandler;

		// Token: 0x04000017 RID: 23
		private Ink.ErrorHandler _externalErrorHandler;

		// Token: 0x04000018 RID: 24
		private string _filename;

		// Token: 0x04000019 RID: 25
		public static readonly CharacterRange LatinBasic = CharacterRange.Define('A', 'z', new CharacterSet().AddRange('[', '`'));

		// Token: 0x0400001A RID: 26
		public static readonly CharacterRange LatinExtendedA = CharacterRange.Define('Ā', 'ſ', null);

		// Token: 0x0400001B RID: 27
		public static readonly CharacterRange LatinExtendedB = CharacterRange.Define('ƀ', 'ɏ', null);

		// Token: 0x0400001C RID: 28
		public static readonly CharacterRange Greek = CharacterRange.Define('Ͱ', 'Ͽ', new CharacterSet().AddRange('\u0378', '\u0385').AddCharacters("ʹ\u0375\u0378·\u038b\u038d\u03a2"));

		// Token: 0x0400001D RID: 29
		public static readonly CharacterRange Cyrillic = CharacterRange.Define('Ѐ', 'ӿ', new CharacterSet().AddRange('҂', '\u0489'));

		// Token: 0x0400001E RID: 30
		public static readonly CharacterRange Armenian = CharacterRange.Define('\u0530', '֏', new CharacterSet().AddCharacters("\u0530").AddRange('\u0557', 'ՠ').AddRange('ֈ', '֎'));

		// Token: 0x0400001F RID: 31
		public static readonly CharacterRange Hebrew = CharacterRange.Define('\u0590', '\u05ff', new CharacterSet());

		// Token: 0x04000020 RID: 32
		public static readonly CharacterRange Arabic = CharacterRange.Define('\u0600', 'ۿ', new CharacterSet());

		// Token: 0x04000021 RID: 33
		public static readonly CharacterRange Korean = CharacterRange.Define('가', '\ud7af', new CharacterSet());

		// Token: 0x04000022 RID: 34
		public static readonly CharacterRange Latin1Supplement = CharacterRange.Define('\u0080', 'ÿ', new CharacterSet());

		// Token: 0x04000023 RID: 35
		private bool _parsingChoice;

		// Token: 0x04000024 RID: 36
		private CharacterSet _runtimePathCharacterSet;

		// Token: 0x04000025 RID: 37
		private CharacterSet _nonTextPauseCharacters;

		// Token: 0x04000026 RID: 38
		private CharacterSet _nonTextEndCharacters;

		// Token: 0x04000027 RID: 39
		private CharacterSet _notTextEndCharactersChoice;

		// Token: 0x04000028 RID: 40
		private CharacterSet _notTextEndCharactersString;

		// Token: 0x04000029 RID: 41
		private List<InkParser.InfixOperator> _binaryOperators;

		// Token: 0x0400002A RID: 42
		private int _maxBinaryOpLength;

		// Token: 0x0400002B RID: 43
		private InkParser _rootParser;

		// Token: 0x0400002C RID: 44
		private HashSet<string> _openFilenames;

		// Token: 0x0400002D RID: 45
		private CharacterSet _identifierCharSet;

		// Token: 0x0400002E RID: 46
		private CharacterSet _sequenceTypeSymbols = new CharacterSet("!&~$");

		// Token: 0x0400002F RID: 47
		private StringParser.ParseRule[][] _statementRulesAtLevel;

		// Token: 0x04000030 RID: 48
		private StringParser.ParseRule[][] _statementBreakRulesAtLevel;

		// Token: 0x04000031 RID: 49
		private CharacterSet _endOfTagCharSet = new CharacterSet("#\n\r\\");

		// Token: 0x04000032 RID: 50
		private CharacterSet _inlineWhitespaceChars = new CharacterSet(" \t");

		// Token: 0x0200006D RID: 109
		protected enum CustomFlags
		{
			// Token: 0x040001B2 RID: 434
			ParsingString = 1
		}

		// Token: 0x0200006E RID: 110
		protected class InfixOperator
		{
			// Token: 0x06000503 RID: 1283 RVA: 0x0001AF33 File Offset: 0x00019133
			public InfixOperator(string type, int precedence, bool requireWhitespace)
			{
				this.type = type;
				this.precedence = precedence;
				this.requireWhitespace = requireWhitespace;
			}

			// Token: 0x06000504 RID: 1284 RVA: 0x0001AF50 File Offset: 0x00019150
			public override string ToString()
			{
				return this.type;
			}

			// Token: 0x040001B3 RID: 435
			public string type;

			// Token: 0x040001B4 RID: 436
			public int precedence;

			// Token: 0x040001B5 RID: 437
			public bool requireWhitespace;
		}

		// Token: 0x0200006F RID: 111
		protected class NameWithMetadata
		{
			// Token: 0x040001B6 RID: 438
			public string name;

			// Token: 0x040001B7 RID: 439
			public DebugMetadata metadata;
		}

		// Token: 0x02000070 RID: 112
		protected class FlowDecl
		{
			// Token: 0x040001B8 RID: 440
			public Identifier name;

			// Token: 0x040001B9 RID: 441
			public List<FlowBase.Argument> arguments;

			// Token: 0x040001BA RID: 442
			public bool isFunction;
		}

		// Token: 0x02000071 RID: 113
		protected enum StatementLevel
		{
			// Token: 0x040001BC RID: 444
			InnerBlock,
			// Token: 0x040001BD RID: 445
			Stitch,
			// Token: 0x040001BE RID: 446
			Knot,
			// Token: 0x040001BF RID: 447
			Top
		}
	}
}
