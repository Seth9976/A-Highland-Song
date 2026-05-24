using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using UnityEngine;

// Token: 0x0200009D RID: 157
public static class InkInstructionParserUtility
{
	// Token: 0x17000149 RID: 329
	// (get) Token: 0x06000507 RID: 1287 RVA: 0x000283AC File Offset: 0x000265AC
	public static Regex digitalTimeRegexParser
	{
		get
		{
			if (InkInstructionParserUtility._digitalTimeRegexParser == null)
			{
				InkInstructionParserUtility._digitalTimeRegexParser = new Regex("([0-1]?[0-9]|2[0-3]):[0-5][0-9]");
			}
			return InkInstructionParserUtility._digitalTimeRegexParser;
		}
	}

	// Token: 0x1700014A RID: 330
	// (get) Token: 0x06000508 RID: 1288 RVA: 0x000283C9 File Offset: 0x000265C9
	public static Regex dialogueRegexParser
	{
		get
		{
			if (InkInstructionParserUtility._dialogueRegexParser == null)
			{
				InkInstructionParserUtility._dialogueRegexParser = new Regex("([^:]*)(?:\\s*:?\\s*)(.*)");
			}
			return InkInstructionParserUtility._dialogueRegexParser;
		}
	}

	// Token: 0x06000509 RID: 1289 RVA: 0x000283E8 File Offset: 0x000265E8
	public static bool TryParseDialogue(string rawContent, out string speaker, out string content)
	{
		speaker = null;
		content = null;
		if (rawContent == null)
		{
			return false;
		}
		Match match = InkInstructionParserUtility.dialogueRegexParser.Match(rawContent);
		if (!match.Success)
		{
			return false;
		}
		if (InkInstructionParserUtility.digitalTimeRegexParser.Match(rawContent).Success)
		{
			return false;
		}
		speaker = match.Groups[1].Value.Trim();
		content = match.Groups[2].Value.Trim();
		return true;
	}

	// Token: 0x1700014B RID: 331
	// (get) Token: 0x0600050A RID: 1290 RVA: 0x0002845B File Offset: 0x0002665B
	public static Regex instructionPrefixRegexParser
	{
		get
		{
			if (InkInstructionParserUtility._instructionPrefixRegexParser == null)
			{
				InkInstructionParserUtility._instructionPrefixRegexParser = new Regex("\\s*>{2,}\\s*");
			}
			return InkInstructionParserUtility._instructionPrefixRegexParser;
		}
	}

	// Token: 0x0600050B RID: 1291 RVA: 0x00028478 File Offset: 0x00026678
	public static string BuildInstructionPrefixRegex(string instructionName)
	{
		return string.Format("{0}(?:{1}\\s*:?\\s*)(.*)", "\\s*>{2,}\\s*", instructionName);
	}

	// Token: 0x0600050C RID: 1292 RVA: 0x0002848A File Offset: 0x0002668A
	public static string BuildColonPrefixedInstructionRegex(string instructionName)
	{
		return string.Format("(?:{0}\\s*:\\s*)(.*)", instructionName);
	}

	// Token: 0x1700014C RID: 332
	// (get) Token: 0x0600050D RID: 1293 RVA: 0x00028497 File Offset: 0x00026697
	public static Regex argumentAndValueParser
	{
		get
		{
			if (InkInstructionParserUtility._argumentAndValueParser == null)
			{
				InkInstructionParserUtility._argumentAndValueParser = new Regex("\\((\\w*?)\\s*(\\w*?)\\s*[:=]\\s*(.*?\\s*)\\)");
			}
			return InkInstructionParserUtility._argumentAndValueParser;
		}
	}

	// Token: 0x0600050E RID: 1294 RVA: 0x000284B4 File Offset: 0x000266B4
	public static bool TryParseEnum(Type enumType, string value, bool ignoreCase, out object enumValue)
	{
		enumValue = Enum.Parse(enumType, value, ignoreCase);
		return Enum.IsDefined(enumType, enumValue);
	}

	// Token: 0x1700014D RID: 333
	// (get) Token: 0x0600050F RID: 1295 RVA: 0x000284C8 File Offset: 0x000266C8
	public static Regex vector2Parser
	{
		get
		{
			if (InkInstructionParserUtility._vector2Parser == null)
			{
				InkInstructionParserUtility._vector2Parser = new Regex("(?:x|min)=([+-]?(?:[0-9]*[.])?[0-9]+),\\s*(?:y|max)=([+-]?(?:[0-9]*[.])?[0-9]+)", RegexOptions.IgnoreCase);
			}
			return InkInstructionParserUtility._vector2Parser;
		}
	}

	// Token: 0x06000510 RID: 1296 RVA: 0x000284E8 File Offset: 0x000266E8
	public static bool TryParseVector2(string value, out Vector2 vector2)
	{
		vector2 = Vector2.zero;
		Match match = InkInstructionParserUtility.vector2Parser.Match(value);
		if (!match.Success)
		{
			return false;
		}
		float num;
		if (!float.TryParse(match.Groups[1].Value, NumberStyles.Float, CultureInfo.InvariantCulture, out num))
		{
			return false;
		}
		float num2;
		if (!float.TryParse(match.Groups[2].Value, NumberStyles.Float, CultureInfo.InvariantCulture, out num2))
		{
			return false;
		}
		vector2 = new Vector2(num, num2);
		return true;
	}

	// Token: 0x1700014E RID: 334
	// (get) Token: 0x06000511 RID: 1297 RVA: 0x00028570 File Offset: 0x00026770
	public static Regex rectParser
	{
		get
		{
			if (InkInstructionParserUtility._rectParser == null)
			{
				InkInstructionParserUtility._rectParser = new Regex("x=([+-]?(?:[0-9]*[.])?[0-9]+),\\s*y=([+-]?(?:[0-9]*[.])?[0-9]+),\\s*(?:w|width)=([+-]?(?:[0-9]*[.])?[0-9]+),(?:h|height)=([+-]?(?:[0-9]*[.])?[0-9]+)", RegexOptions.IgnoreCase);
			}
			return InkInstructionParserUtility._rectParser;
		}
	}

	// Token: 0x06000512 RID: 1298 RVA: 0x00028590 File Offset: 0x00026790
	public static bool TryParseRect(string value, out Rect rect)
	{
		rect = Rect.zero;
		Match match = InkInstructionParserUtility.rectParser.Match(value);
		if (!match.Success)
		{
			return false;
		}
		float num;
		if (!float.TryParse(match.Groups[1].Value, NumberStyles.Float, CultureInfo.InvariantCulture, out num))
		{
			return false;
		}
		float num2;
		if (!float.TryParse(match.Groups[2].Value, NumberStyles.Float, CultureInfo.InvariantCulture, out num2))
		{
			return false;
		}
		float num3;
		if (!float.TryParse(match.Groups[3].Value, NumberStyles.Float, CultureInfo.InvariantCulture, out num3))
		{
			return false;
		}
		float num4;
		if (!float.TryParse(match.Groups[4].Value, NumberStyles.Float, CultureInfo.InvariantCulture, out num4))
		{
			return false;
		}
		rect = new Rect(num, num2, num3, num4);
		return true;
	}

	// Token: 0x06000513 RID: 1299 RVA: 0x00028668 File Offset: 0x00026868
	public static InkInstructionParserUtility.ParsedArgumentCollection ParseArguments(string text)
	{
		InkInstructionParserUtility.ParsedArgumentCollection parsedArgumentCollection = new InkInstructionParserUtility.ParsedArgumentCollection();
		InkInstructionParserUtility.ParsedArgumentCollection parsedArgumentCollection2 = parsedArgumentCollection;
		parsedArgumentCollection.rawText = text;
		parsedArgumentCollection2.textBeforeArguments = text;
		int num = parsedArgumentCollection.textBeforeArguments.Length;
		foreach (object obj in InkInstructionParserUtility.argumentAndValueParser.Matches(text))
		{
			Match match = (Match)obj;
			num = Mathf.Min(num, match.Index);
			string value = match.Groups[1].Value;
			string value2 = match.Groups[2].Value;
			string text2 = Regex.Unescape(match.Groups[3].Value);
			if (parsedArgumentCollection.ContainsKey(value2))
			{
				Debug.LogWarning("Tried to add key " + value2 + " but key already exists!");
			}
			else
			{
				parsedArgumentCollection.Add(value2, new InkInstructionParserUtility.ParsedArgument(value, value2, text2));
			}
		}
		if (num != parsedArgumentCollection.textBeforeArguments.Length)
		{
			parsedArgumentCollection.textBeforeArguments = parsedArgumentCollection.textBeforeArguments.Substring(0, num).Trim();
		}
		return parsedArgumentCollection;
	}

	// Token: 0x06000514 RID: 1300 RVA: 0x00028798 File Offset: 0x00026998
	public static InkInstructionParserUtility.ParsedArgumentCollection ParseCSSStyleArguments(List<string> stringArguments)
	{
		InkInstructionParserUtility.ParsedArgumentCollection parsedArgumentCollection = new InkInstructionParserUtility.ParsedArgumentCollection();
		if (stringArguments != null)
		{
			foreach (string text in stringArguments)
			{
				int num = text.IndexOf(':');
				if (num == -1)
				{
					num = text.IndexOf('=');
				}
				if (num != -1 && num < text.Length - 1)
				{
					string text2 = text.Substring(0, num).Trim();
					if (text2 != null)
					{
						string text3 = Regex.Unescape(text.Substring(num + 1, text.Length - 1 - num).Trim());
						if (text3 != null)
						{
							if (parsedArgumentCollection.ContainsKey(text2))
							{
								Debug.LogWarning("CSS Style with key " + text2 + " already exists! Full list of arguments: \n" + DebugX.ListAsString<string>(stringArguments, null, true, true));
							}
							else
							{
								parsedArgumentCollection.Add(text2, new InkInstructionParserUtility.ParsedArgument(null, text2, text3));
							}
						}
					}
				}
			}
		}
		return parsedArgumentCollection;
	}

	// Token: 0x06000515 RID: 1301 RVA: 0x0002888C File Offset: 0x00026A8C
	public static bool ParseTag(string content, out string contentWithoutTags, out string tagID)
	{
		contentWithoutTags = content;
		tagID = null;
		if (InkInstructionParserUtility._tagRegex == null)
		{
			InkInstructionParserUtility._tagRegex = new Regex("<(\\w+)>(.+?)<\\/\\w+>", RegexOptions.IgnoreCase);
		}
		Match match = InkInstructionParserUtility._tagRegex.Match(content);
		if (match.Success)
		{
			contentWithoutTags = match.Groups[2].Value;
			tagID = match.Groups[1].Value;
			return true;
		}
		return false;
	}

	// Token: 0x06000516 RID: 1302 RVA: 0x000288F4 File Offset: 0x00026AF4
	public static string[] SplitTextIntoLines(string content)
	{
		content = content.Trim();
		string[] array = content.Split(InkInstructionParserUtility.lineSplitCharacters, StringSplitOptions.RemoveEmptyEntries);
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = array[i].Trim();
		}
		return array;
	}

	// Token: 0x040005D6 RID: 1494
	public const string digitalTimeRegex = "([0-1]?[0-9]|2[0-3]):[0-5][0-9]";

	// Token: 0x040005D7 RID: 1495
	private static Regex _digitalTimeRegexParser;

	// Token: 0x040005D8 RID: 1496
	public const string dialogueRegex = "([^:]*)(?:\\s*:?\\s*)(.*)";

	// Token: 0x040005D9 RID: 1497
	private static Regex _dialogueRegexParser;

	// Token: 0x040005DA RID: 1498
	public const string instructionPrefixRegex = "\\s*>{2,}\\s*";

	// Token: 0x040005DB RID: 1499
	private static Regex _instructionPrefixRegexParser;

	// Token: 0x040005DC RID: 1500
	public const string standardGap = "\\s*-*\\s*";

	// Token: 0x040005DD RID: 1501
	private static Regex _argumentAndValueParser;

	// Token: 0x040005DE RID: 1502
	public const string floatParseString = "([+-]?(?:[0-9]*[.])?[0-9]+)";

	// Token: 0x040005DF RID: 1503
	public const string vector2ParseString = "(?:x|min)=([+-]?(?:[0-9]*[.])?[0-9]+),\\s*(?:y|max)=([+-]?(?:[0-9]*[.])?[0-9]+)";

	// Token: 0x040005E0 RID: 1504
	private static Regex _vector2Parser;

	// Token: 0x040005E1 RID: 1505
	public const string rectParseString = "x=([+-]?(?:[0-9]*[.])?[0-9]+),\\s*y=([+-]?(?:[0-9]*[.])?[0-9]+),\\s*(?:w|width)=([+-]?(?:[0-9]*[.])?[0-9]+),(?:h|height)=([+-]?(?:[0-9]*[.])?[0-9]+)";

	// Token: 0x040005E2 RID: 1506
	private static Regex _rectParser;

	// Token: 0x040005E3 RID: 1507
	private static Regex _tagRegex;

	// Token: 0x040005E4 RID: 1508
	private static char[] lineSplitCharacters = new char[] { '\n' };

	// Token: 0x020002CA RID: 714
	[Serializable]
	public class ParsedArgument
	{
		// Token: 0x0600163D RID: 5693 RVA: 0x00097F70 File Offset: 0x00096170
		public ParsedArgument(string variableStringType, string variableName, string variableStringValue)
		{
			this.variableStringType = variableStringType;
			if (!string.IsNullOrWhiteSpace(this.variableStringType) && !InkInstructionParserUtility.ParsedArgument.primitiveTypes.TryGetValue(this.variableStringType, out this.type))
			{
				this.type = Type.GetType(this.variableStringType, false);
			}
			this.variableName = variableName;
			this.variableStringValue = variableStringValue.Trim();
		}

		// Token: 0x04001631 RID: 5681
		public string variableStringType;

		// Token: 0x04001632 RID: 5682
		public Type type;

		// Token: 0x04001633 RID: 5683
		public string variableName;

		// Token: 0x04001634 RID: 5684
		public string variableStringValue;

		// Token: 0x04001635 RID: 5685
		private static Dictionary<string, Type> primitiveTypes = new Dictionary<string, Type>
		{
			{
				"object",
				typeof(object)
			},
			{
				"bool",
				typeof(bool)
			},
			{
				"int",
				typeof(int)
			},
			{
				"float",
				typeof(float)
			},
			{
				"char",
				typeof(char)
			},
			{
				"string",
				typeof(string)
			}
		};
	}

	// Token: 0x020002CB RID: 715
	public class ParsedArgumentCollection : Dictionary<string, InkInstructionParserUtility.ParsedArgument>
	{
		// Token: 0x0600163F RID: 5695 RVA: 0x00098069 File Offset: 0x00096269
		public ParsedArgumentCollection()
			: base(StringComparer.OrdinalIgnoreCase)
		{
		}

		// Token: 0x06001640 RID: 5696 RVA: 0x00098078 File Offset: 0x00096278
		public bool TryGetValue<T>(string key, ref T val)
		{
			InkInstructionParserUtility.ParsedArgument parsedArgument = null;
			if (!base.TryGetValue(key, out parsedArgument))
			{
				return false;
			}
			Type type = typeof(T);
			type = Nullable.GetUnderlyingType(type) ?? type;
			if (parsedArgument.type != null)
			{
				if (!type.IsAssignableFrom(parsedArgument.type))
				{
					string[] array = new string[6];
					array[0] = "Expected variable type ";
					int num = 1;
					Type type2 = type;
					array[num] = ((type2 != null) ? type2.ToString() : null);
					array[2] = " from key ";
					array[3] = key;
					array[4] = " is not assignable from ink specified type ";
					int num2 = 5;
					Type type3 = parsedArgument.type;
					array[num2] = ((type3 != null) ? type3.ToString() : null);
					Debug.LogError(string.Concat(array));
					return false;
				}
				type = parsedArgument.type;
			}
			bool flag = type == typeof(object);
			if (flag || type == typeof(bool))
			{
				bool flag2 = false;
				if (bool.TryParse(parsedArgument.variableStringValue, out flag2))
				{
					val = (T)((object)flag2);
					return true;
				}
			}
			else if (flag || type == typeof(float))
			{
				float num3 = 0f;
				if (float.TryParse(parsedArgument.variableStringValue, NumberStyles.Float, CultureInfo.InvariantCulture, out num3))
				{
					val = (T)((object)num3);
					return true;
				}
			}
			else if (flag || type == typeof(int))
			{
				int num4 = 0;
				if (int.TryParse(parsedArgument.variableStringValue, NumberStyles.Integer, CultureInfo.InvariantCulture, out num4))
				{
					val = (T)((object)num4);
					return true;
				}
			}
			else if (flag || type == typeof(Color))
			{
				Color color = default(Color);
				if (ColorUtility.TryParseHtmlString((parsedArgument.variableStringValue.StartsWith("#") || parsedArgument.variableStringValue.StartsWith("\\#")) ? parsedArgument.variableStringValue : ("#" + parsedArgument.variableStringValue), out color))
				{
					val = (T)((object)color);
					return true;
				}
			}
			else if (flag || type == typeof(Rect))
			{
				Rect rect = default(Rect);
				if (InkInstructionParserUtility.TryParseRect(parsedArgument.variableStringValue, out rect))
				{
					val = (T)((object)rect);
					return true;
				}
			}
			else if (flag || type == typeof(Vector2))
			{
				Vector2 vector = default(Vector2);
				if (InkInstructionParserUtility.TryParseVector2(parsedArgument.variableStringValue, out vector))
				{
					val = (T)((object)vector);
					return true;
				}
			}
			else if (type.IsEnum)
			{
				object obj = null;
				if (InkInstructionParserUtility.TryParseEnum(type, parsedArgument.variableStringValue, true, out obj))
				{
					val = (T)((object)obj);
					return true;
				}
			}
			else if (flag || type == typeof(string))
			{
				val = (T)((object)parsedArgument.variableStringValue);
				return true;
			}
			return false;
		}

		// Token: 0x06001641 RID: 5697 RVA: 0x00098350 File Offset: 0x00096550
		public bool TryGetRequiredValue<T>(string key, ref T val, Type instructionType)
		{
			if (!this.TryGetValue<T>(key, ref val))
			{
				Debug.LogWarning(string.Concat(new string[] { "Parsed ink content type \"", instructionType.Name, "\" found but required argument \"", key, "\" missing!" }));
				return false;
			}
			return true;
		}

		// Token: 0x04001636 RID: 5686
		public string rawText;

		// Token: 0x04001637 RID: 5687
		public string textBeforeArguments;

		// Token: 0x02000434 RID: 1076
		public enum ArgumentRequirementType
		{
			// Token: 0x04001B91 RID: 7057
			None,
			// Token: 0x04001B92 RID: 7058
			Warn,
			// Token: 0x04001B93 RID: 7059
			Error
		}
	}
}
