using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

// Token: 0x020000A1 RID: 161
public static class InkStylingUtility
{
	// Token: 0x06000532 RID: 1330 RVA: 0x00028FF0 File Offset: 0x000271F0
	private static string OpenColor(string hexColor)
	{
		return string.Format("<color={0}>", hexColor);
	}

	// Token: 0x06000533 RID: 1331 RVA: 0x00028FFD File Offset: 0x000271FD
	public static bool StartsWithVowel(string s)
	{
		return s.Length > 0 && "aeiou".IndexOf(s.Substring(0, 1).ToLower()) > -1;
	}

	// Token: 0x06000534 RID: 1332 RVA: 0x00029028 File Offset: 0x00027228
	public static string UppercaseFirstCharacterHandlingQuotesAndOtherMarkers(string s)
	{
		if (string.IsNullOrEmpty(s))
		{
			return string.Empty;
		}
		char[] array = s.ToCharArray();
		bool flag = true;
		for (int i = 0; i < array.Length; i++)
		{
			char c = array[i];
			if (c != '"' && c != '\'' && c != '_' && c != '‘' && c != '’' && c != '“' && c != '”' && c != '§' && c != ' ')
			{
				if (flag)
				{
					array[i] = char.ToUpperInvariant(c);
				}
				flag = c.isTerminator();
				if (flag && i < array.Length - 1 && (array[i] == '!' || array[i] == '?') && (array[i + 1] == '"' || array[i + 1] == '\'' || array[i + 1] == '’' || array[i + 1] == '”'))
				{
					flag = false;
				}
				if (i > 2 && c == '.' && array[i - 1] == '.' && array[i - 2] == '.')
				{
					flag = false;
				}
			}
		}
		return new string(array);
	}

	// Token: 0x06000535 RID: 1333 RVA: 0x00029130 File Offset: 0x00027330
	public static string ParseStyling(string dialogueText, bool colourise = true, string hexColor = "#FFE473")
	{
		InkStylingUtility.sb.Clear();
		bool flag = false;
		bool flag2 = false;
		for (int i = 0; i < dialogueText.Length; i++)
		{
			if (dialogueText[i] == '_')
			{
				flag = !flag;
				InkStylingUtility.sb.Append(flag ? "<space=-0.05em><i>" : "</i><space=0.2em>");
			}
			else if (dialogueText[i] == '§')
			{
				if (colourise)
				{
					flag2 = !flag2;
					InkStylingUtility.sb.Append(flag2 ? InkStylingUtility.OpenColor(hexColor) : "</color>");
				}
			}
			else
			{
				if (dialogueText[i] == ' ')
				{
					if (flag2)
					{
						InkStylingUtility.sb.Append("</color>");
					}
					if (flag)
					{
						InkStylingUtility.sb.Append("</i><space=0.2em>");
					}
				}
				InkStylingUtility.sb.Append(dialogueText[i]);
				if (dialogueText[i] == ' ')
				{
					if (flag2)
					{
						InkStylingUtility.sb.Append(InkStylingUtility.OpenColor(hexColor));
					}
					if (flag)
					{
						InkStylingUtility.sb.Append("<space=-0.05em><i>");
					}
				}
			}
		}
		return InkStylingUtility.sb.ToString();
	}

	// Token: 0x06000536 RID: 1334 RVA: 0x00029248 File Offset: 0x00027448
	public static string ExhaustifyDialogueText(string dialogueLine, bool tired, bool veryTired = false, bool cold = false)
	{
		List<string> list = new List<string> { "i", "you", "i'm", "but", "if" };
		List<string> list2 = new List<string>
		{
			"ch", "sh", "th", "ph", "dr", "cl", "cr", "c", "kn", "k",
			"m", "p", "tr", "t", "fr", "f", "br", "bl", "b"
		};
		List<char> list3 = new List<char> { '.', ',', ';', ':', ')', '-', '\'', '"', '>' };
		bool flag = false;
		int num = 0;
		int num2 = Random.Range(2, 5);
		StringBuilder stringBuilder = new StringBuilder();
		foreach (string text in dialogueLine.Split(' ', StringSplitOptions.None))
		{
			string text2 = text;
			if (tired && !flag && list.Contains(text.ToLower()))
			{
				stringBuilder.Append(text + "... ");
				flag = true;
			}
			else if (cold && text.Length >= 2 && Random.value < 1f - (float)num * 0.33f)
			{
				for (int j = 2; j > 0; j--)
				{
					string text3 = text.Substring(0, j);
					if (list2.Contains(text3.ToLower()))
					{
						stringBuilder.Append(text3 + "-" + text3.ToLower());
						if (Random.value < 0.2f)
						{
							stringBuilder.Append("-" + text3.ToLower());
						}
						text2 = text.Substring(j);
						num++;
						break;
					}
				}
			}
			num2--;
			stringBuilder.Append(text2);
			if (text.Length > 0)
			{
				char c = text[text.Length - 1];
				if (num2 <= 0 && !list3.Contains(c) && (tired || cold))
				{
					stringBuilder.Append("...");
					num2 = Random.Range(2, 5) + (veryTired ? 0 : 2);
				}
				stringBuilder.Append(" ");
			}
		}
		return stringBuilder.ToString();
	}

	// Token: 0x06000537 RID: 1335 RVA: 0x00029544 File Offset: 0x00027744
	public static string SentenceCaseWithoutArticles(string s)
	{
		if (s.IndexOf("a ", StringComparison.OrdinalIgnoreCase) == 0)
		{
			s = s.Remove(0, 2);
		}
		if (s.IndexOf("the ", StringComparison.OrdinalIgnoreCase) == 0)
		{
			s = s.Remove(0, 4);
		}
		return InkStylingUtility.UppercaseFirstCharacter(s);
	}

	// Token: 0x06000538 RID: 1336 RVA: 0x0002957C File Offset: 0x0002777C
	public static string UppercaseFirstCharacter(string s)
	{
		if (string.IsNullOrEmpty(s))
		{
			return string.Empty;
		}
		char[] array = s.ToCharArray();
		array[0] = char.ToUpperInvariant(array[0]);
		return new string(array);
	}

	// Token: 0x06000539 RID: 1337 RVA: 0x000295B0 File Offset: 0x000277B0
	public static string ResolveArticleAlteringMarkers(string s)
	{
		bool flag = false;
		StringBuilder stringBuilder = new StringBuilder(s.Length);
		for (int i = 0; i < s.Length - 1; i++)
		{
			if (!flag && (s[i] == 'n' || s[i] == 'N') && s[i + 1] == ' ')
			{
				if (i < 2 || s[i - 1] != ' ' || (s[i - 2] != 'a' && s[i - 2] != 'A'))
				{
					if (i >= 1)
					{
						stringBuilder.Append(s[i - 1]);
					}
					i += 2;
				}
			}
			else
			{
				flag = char.IsLetter(s[i]);
				if (i > 0)
				{
					stringBuilder.Append(s[i - 1]);
				}
			}
		}
		if (s.Length >= 2)
		{
			stringBuilder.Append(s[s.Length - 2]);
		}
		if (s.Length >= 1)
		{
			stringBuilder.Append(s[s.Length - 1]);
		}
		return stringBuilder.ToString();
	}

	// Token: 0x0600053A RID: 1338 RVA: 0x000296B4 File Offset: 0x000278B4
	public static string CorrectSpacingErrors(string s)
	{
		if (string.IsNullOrEmpty(s))
		{
			return string.Empty;
		}
		StringBuilder stringBuilder = new StringBuilder(s.Length * 2);
		bool flag = false;
		bool flag2 = false;
		bool flag3 = false;
		bool flag4 = false;
		char c = '^';
		int num = 0;
		bool flag5 = false;
		for (int i = 0; i < s.Length; i++)
		{
			char c2 = s[i];
			if (c2 == '\'')
			{
				if ((c == '^' || c == ' ' || (flag && !flag3)) && (!flag3 || flag4))
				{
					c2 = '‘';
					flag3 = true;
					flag4 = false;
				}
				else
				{
					if (flag3 && (c.isPunctuation() || (c == ' ' && stringBuilder[stringBuilder.Length - 2].isPunctuation())))
					{
						flag4 = true;
					}
					if (flag3 && flag)
					{
						flag3 = false;
					}
					c2 = '’';
				}
			}
			if (c2 == '"')
			{
				if (flag2)
				{
					flag2 = false;
					c2 = '”';
				}
				else
				{
					c2 = '“';
					flag2 = true;
				}
			}
			if ((c != '“' && c != '‘') || c2 != ' ')
			{
				if (flag5 && !c2.isPunctuation() && c2 != ' ')
				{
					flag5 = false;
					stringBuilder.Append('_');
				}
				if (c2 == '_')
				{
					flag5 = true;
				}
				else
				{
					if (c2.isPunctuation() || c2.isQuoteMark())
					{
						if (c2 != '’')
						{
							flag = true;
							if (c == '’')
							{
								flag3 = false;
							}
						}
						if (c == ' ' && stringBuilder.Length > 0 && (!c2.isQuoteMark() || flag4))
						{
							stringBuilder.Remove(stringBuilder.Length - 1, 1);
							c = stringBuilder[stringBuilder.Length - 1];
							if (!flag5 && c == '_' && stringBuilder.Length > 0)
							{
								flag5 = true;
								stringBuilder.Remove(stringBuilder.Length - 1, 1);
								c = stringBuilder[stringBuilder.Length - 1];
							}
							if (flag4)
							{
								flag3 = false;
								if (c.isPunctuation())
								{
									flag = true;
								}
							}
						}
					}
					else
					{
						if (flag && c2 != ' ' && c2 != '_')
						{
							stringBuilder.Append(' ');
						}
						flag = false;
					}
					if ((c2 != c || (c2 != ' ' && c2 != ',' && c2 != ':' && c2 != ';' && c2 != ')' && c2 != '(')) && (stringBuilder.Length != 0 || (c2 != ' ' && c2 != ',' && c2 != ':' && c2 != ';' && c2 != ')')))
					{
						bool flag6 = false;
						if (c2.isTerminator())
						{
							if (c == '.')
							{
								num++;
								if ((i != s.Length - 1 && (s[i + 1] == '.' || s[i + 1] == '!' || s[i + 1] == '?')) || num < 3)
								{
									goto IL_037E;
								}
								stringBuilder.Append('.');
								flag6 = true;
							}
							else
							{
								num = 1;
							}
						}
						if (!flag6 && c != c2 && c.isPunctuation() && c2.isPunctuation() && (!c.isSpecialTerminator() || !c2.isSpecialTerminator()))
						{
							if (c.isTerminator() == c2.isTerminator() || num > 1)
							{
								goto IL_037E;
							}
							num = 0;
							stringBuilder.Remove(stringBuilder.Length - 1, 1);
							if (stringBuilder.Length >= 2)
							{
								c = stringBuilder[stringBuilder.Length - 1];
							}
							else
							{
								c = '^';
							}
						}
						if (flag5 && c2 == ' ')
						{
							flag5 = false;
							stringBuilder.Append('_');
						}
						stringBuilder.Append(c2);
						if (flag3 && c == '’')
						{
							if (c2 != ' ' && !flag)
							{
								flag4 = false;
							}
							else if (c2 == ' ')
							{
								flag4 = true;
							}
						}
						c = c2;
					}
				}
			}
			IL_037E:;
		}
		if (flag5)
		{
			stringBuilder.Append('_');
		}
		return stringBuilder.ToString().Trim();
	}

	// Token: 0x0600053B RID: 1339 RVA: 0x00029A6A File Offset: 0x00027C6A
	public static bool isPunctuation(this char thisChar)
	{
		return InkStylingUtility.punctuation.Contains(thisChar);
	}

	// Token: 0x0600053C RID: 1340 RVA: 0x00029A77 File Offset: 0x00027C77
	public static bool isTerminator(this char thisChar)
	{
		return InkStylingUtility.terminators.Contains(thisChar);
	}

	// Token: 0x0600053D RID: 1341 RVA: 0x00029A84 File Offset: 0x00027C84
	public static bool isSpecialTerminator(this char thisChar)
	{
		return InkStylingUtility.specialTerminators.Contains(thisChar);
	}

	// Token: 0x0600053E RID: 1342 RVA: 0x00029A91 File Offset: 0x00027C91
	public static bool isQuoteMark(this char thisChar)
	{
		return thisChar == '”' || thisChar == '’' || thisChar == '\'' || thisChar == '"';
	}

	// Token: 0x0600053F RID: 1343 RVA: 0x00029AAF File Offset: 0x00027CAF
	public static string Possessive(this string txt)
	{
		if (string.IsNullOrEmpty(txt))
		{
			return "";
		}
		if (txt[txt.Length - 1] == 's')
		{
			return txt + "'";
		}
		return txt + "'s";
	}

	// Token: 0x06000540 RID: 1344 RVA: 0x00029AE8 File Offset: 0x00027CE8
	public static string ProcessText(string text, bool terminatingPunctuation = true, bool stripSquareBrackets = false)
	{
		if (string.IsNullOrEmpty(text))
		{
			return string.Empty;
		}
		string text2 = text;
		if (stripSquareBrackets)
		{
			text2 = InkStylingUtility.StripSquareBrackets(text2).Trim();
		}
		text2 = InkStylingUtility.CorrectSpacingErrors(text2);
		text2 = InkStylingUtility.ResolveArticleAlteringMarkers(text2);
		text2 = InkStylingUtility.UppercaseFirstCharacterHandlingQuotesAndOtherMarkers(text2);
		return InkStylingUtility.EnforceTerminatingPunctuation(text2, terminatingPunctuation);
	}

	// Token: 0x06000541 RID: 1345 RVA: 0x00029B34 File Offset: 0x00027D34
	public static string StripSquareBrackets(string s)
	{
		StringBuilder stringBuilder = new StringBuilder(s.Length);
		bool flag = false;
		int i = 0;
		while (i < s.Length)
		{
			char c = s[i];
			if (c == '[')
			{
				flag = true;
				goto IL_002C;
			}
			if (c != ']')
			{
				goto IL_002C;
			}
			flag = false;
			IL_0037:
			i++;
			continue;
			IL_002C:
			if (!flag)
			{
				stringBuilder.Append(c);
				goto IL_0037;
			}
			goto IL_0037;
		}
		return stringBuilder.ToString();
	}

	// Token: 0x06000542 RID: 1346 RVA: 0x00029B8C File Offset: 0x00027D8C
	public static string AppendEllipsis(string text)
	{
		text = text.Trim();
		char c = text[text.Length - 1];
		if (c == '_')
		{
			return InkStylingUtility.AppendEllipsis(text.Substring(0, text.Length - 1)) + "_";
		}
		if (text.Length == 0)
		{
			return text;
		}
		if (!c.isTerminator())
		{
			if (!c.isPunctuation())
			{
				return text + "...";
			}
			return text.Substring(0, text.Length - 1) + "...";
		}
		else
		{
			if (c.isTerminator() && text[text.Length - 2].isTerminator())
			{
				return text;
			}
			StringBuilder stringBuilder = new StringBuilder();
			char c2 = c;
			stringBuilder.Append(text.Substring(0, text.Length - 1));
			stringBuilder.Append("..");
			stringBuilder.Append(c2);
			return stringBuilder.ToString();
		}
	}

	// Token: 0x06000543 RID: 1347 RVA: 0x00029C6C File Offset: 0x00027E6C
	public static string EnforceTerminatingPunctuation(string s, bool requireTerminatingPunctuation)
	{
		if (string.IsNullOrEmpty(s))
		{
			return string.Empty;
		}
		StringBuilder stringBuilder = new StringBuilder(s.Length + 1);
		int i;
		for (i = s.Length - 1; i >= 0; i--)
		{
			if (InkStylingUtility.nonTerminatorsWhichThenRequireTerminators.Contains(s[i]))
			{
				requireTerminatingPunctuation = true;
			}
			else if (!InkStylingUtility.nonTerminators.Contains(s[i]))
			{
				break;
			}
		}
		if (i < 0)
		{
			return s;
		}
		if (requireTerminatingPunctuation)
		{
			if (InkStylingUtility.terminators.Contains(s[i]))
			{
				return s;
			}
			stringBuilder.Append(s.Substring(0, i + 1));
			stringBuilder.Append('.');
		}
		else
		{
			if (s[i] != '.')
			{
				return s;
			}
			if (i >= 3 && s[i - 1] == '.' && s[i - 2] == '.')
			{
				return s;
			}
			stringBuilder.Append(s.Substring(0, i));
		}
		if (i < s.Length - 1)
		{
			stringBuilder.Append(s.Substring(i + 1));
		}
		return stringBuilder.ToString();
	}

	// Token: 0x06000544 RID: 1348 RVA: 0x00029D6C File Offset: 0x00027F6C
	public static string NumberAsWords(int number, bool withLeadingConjunction = false)
	{
		StringBuilder stringBuilder = new StringBuilder();
		if (number > 0)
		{
			if (withLeadingConjunction)
			{
				if (number < 100)
				{
					stringBuilder.Append(" and ");
				}
				else
				{
					stringBuilder.Append(", ");
				}
			}
			if (number >= 1000000)
			{
				stringBuilder.Append(InkStylingUtility.NumberAsWords(Mathf.FloorToInt((float)(number / 1000000)), false));
				stringBuilder.Append(" million");
				if (number % 1000 > 0)
				{
					stringBuilder.Append(InkStylingUtility.NumberAsWords(number % 1000000, true));
				}
			}
			else if (number >= 1000)
			{
				stringBuilder.Append(InkStylingUtility.NumberAsWords(Mathf.FloorToInt((float)(number / 1000)), false));
				stringBuilder.Append(" thousand");
				if (number % 1000 > 0)
				{
					stringBuilder.Append(InkStylingUtility.NumberAsWords(number % 1000, true));
				}
			}
			else if (number >= 100)
			{
				stringBuilder.Append(InkStylingUtility.NumberAsWords(Mathf.FloorToInt((float)(number / 100)), false));
				stringBuilder.Append(" hundred");
				if (number % 100 > 0)
				{
					stringBuilder.Append(InkStylingUtility.NumberAsWords(number % 100, true));
				}
			}
			else
			{
				if (number >= 20)
				{
					stringBuilder.Append(InkStylingUtility.Tens[Mathf.FloorToInt((float)(number / 10))]);
					if (number % 10 > 0)
					{
						stringBuilder.Append("-");
					}
				}
				if (number >= 10 && number < 20)
				{
					stringBuilder.Append(InkStylingUtility.Units[number]);
				}
				else if (number % 10 > 0)
				{
					stringBuilder.Append(InkStylingUtility.Units[number % 10]);
				}
			}
		}
		else if (!withLeadingConjunction)
		{
			stringBuilder.Append("zero");
		}
		return stringBuilder.ToString();
	}

	// Token: 0x06000545 RID: 1349 RVA: 0x00029F10 File Offset: 0x00028110
	public static string StringWithInsertedNewline(string str, int maxSingleLineCharacters)
	{
		if (str == null)
		{
			return null;
		}
		int length = str.Length;
		if (length <= maxSingleLineCharacters)
		{
			return str;
		}
		int num = length / 2;
		for (int i = 0; i < length - 2; i++)
		{
			int num2 = i / 2;
			if (i % 2 == 1)
			{
				num2 = -num2 - 1;
			}
			int num3 = num + num2;
			if (str[num3] == ' ')
			{
				return str.Substring(0, num3) + "\n" + str.Substring(num3 + 1);
			}
		}
		return str;
	}

	// Token: 0x040005FC RID: 1532
	private const string openItalics = "<space=-0.05em><i>";

	// Token: 0x040005FD RID: 1533
	private const string closeItalics = "</i><space=0.2em>";

	// Token: 0x040005FE RID: 1534
	private const string closeColor = "</color>";

	// Token: 0x040005FF RID: 1535
	private static char[] nonTerminatorsWhichThenRequireTerminators = new char[] { '"', '”' };

	// Token: 0x04000600 RID: 1536
	private static char[] nonTerminators = new char[] { ')', ']', '_', ' ', '\'', '’' };

	// Token: 0x04000601 RID: 1537
	private static char[] terminators = new char[] { '?', '!', '.' };

	// Token: 0x04000602 RID: 1538
	private static char[] specialTerminators = new char[] { '?', '!' };

	// Token: 0x04000603 RID: 1539
	private static char[] punctuation = new char[] { '?', '!', '.', ',', ';', ':' };

	// Token: 0x04000604 RID: 1540
	private static StringBuilder sb = new StringBuilder();

	// Token: 0x04000605 RID: 1541
	private static readonly string[] Tens = new string[] { "zero", "ten", "twenty", "thirty", "forty", "fifty", "sixty", "seventy", "eighty", "ninety" };

	// Token: 0x04000606 RID: 1542
	private static readonly string[] Units = new string[]
	{
		"zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine",
		"ten", "eleven", "twelve", "thirteen", "fourteen", "fifteen", "sixteen", "seventeen", "eighteen", "nineteen"
	};
}
