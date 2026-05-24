using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;

// Token: 0x02000216 RID: 534
public static class LineBreaker
{
	// Token: 0x06001381 RID: 4993 RVA: 0x000892E8 File Offset: 0x000874E8
	public static Vector2 Layout(List<float> wordWidths, float maxWidth, float lineHeight, float spaceWidth, List<Vector2> wordPositionsOut, TextAlignment align)
	{
		ValueTuple<List<LineBreaker.Line>, float> valueTuple = LineBreaker.ChooseLineBreaks(wordWidths, spaceWidth, maxWidth);
		List<LineBreaker.Line> item = valueTuple.Item1;
		float item2 = valueTuple.Item2;
		return LineBreaker.PositionWords(item, wordWidths, spaceWidth, lineHeight, item2, wordPositionsOut, align);
	}

	// Token: 0x06001382 RID: 4994 RVA: 0x00089318 File Offset: 0x00087518
	public static string InsertNewlines(string text, float maxCharactersPerLine)
	{
		string[] array = text.Split(' ', StringSplitOptions.None);
		LineBreaker._widths.Clear();
		foreach (string text2 in array)
		{
			LineBreaker._widths.Add((float)text2.Length);
		}
		List<LineBreaker.Line> item = LineBreaker.ChooseLineBreaks(LineBreaker._widths, 1f, maxCharactersPerLine).Item1;
		LineBreaker._widths.Clear();
		StringBuilder stringBuilder = new StringBuilder();
		for (int j = 0; j < item.Count; j++)
		{
			LineBreaker.Line line = item[j];
			for (int k = line.firstWordIdx; k <= line.lastWordIdx; k++)
			{
				stringBuilder.Append(array[k]);
				if (k < line.lastWordIdx)
				{
					stringBuilder.Append(" ");
				}
			}
			if (j < item.Count - 1)
			{
				stringBuilder.Append("\n");
			}
		}
		return stringBuilder.ToString();
	}

	// Token: 0x06001383 RID: 4995 RVA: 0x00089408 File Offset: 0x00087608
	[return: TupleElementNames(new string[] { "lines", "maxLineWidth" })]
	public static ValueTuple<List<LineBreaker.Line>, float> ChooseLineBreaks(List<float> wordWidths, float spaceWidth, float maxWidth)
	{
		LineBreaker._availableSpacePositions.Clear();
		float num = 0f;
		for (int i = 0; i < wordWidths.Count; i++)
		{
			num += wordWidths[i];
			if (i < wordWidths.Count - 1)
			{
				LineBreaker._availableSpacePositions.Add(new LineBreaker.AvailableSpace
				{
					beforeWordIdx = i + 1,
					posInSingleLine = num
				});
				num += spaceWidth;
			}
		}
		int num2 = Mathf.CeilToInt(num / maxWidth);
		int num3 = num2 - 1;
		LineBreaker._lineBreakOrder.Clear();
		if (num3 > 0)
		{
			LineBreaker._minMaxStack.Clear();
			LineBreaker._minMaxStack.Push(new ValueTuple<int, int>(0, num3 - 1));
			while (LineBreaker._minMaxStack.Count > 0)
			{
				ValueTuple<int, int> valueTuple = LineBreaker._minMaxStack.Pop();
				int item = valueTuple.Item1;
				int item2 = valueTuple.Item2;
				if (item == item2)
				{
					LineBreaker._lineBreakOrder.Add(item);
				}
				else if (item + 1 == item2)
				{
					LineBreaker._lineBreakOrder.Add(item);
					LineBreaker._lineBreakOrder.Add(item2);
				}
				else
				{
					int num4 = (item2 + item) / 2;
					LineBreaker._lineBreakOrder.Add(num4);
					if (item <= num4 - 1)
					{
						LineBreaker._minMaxStack.Push(new ValueTuple<int, int>(item, num4 - 1));
					}
					if (num4 + 1 <= item2)
					{
						LineBreaker._minMaxStack.Push(new ValueTuple<int, int>(num4 + 1, item2));
					}
				}
			}
			LineBreaker._minMaxStack.Clear();
		}
		LineBreaker._lines.Clear();
		LineBreaker._lines.Add(new LineBreaker.Line
		{
			firstWordIdx = 0
		});
		foreach (int num5 in LineBreaker._lineBreakOrder)
		{
			if (LineBreaker._availableSpacePositions.Count == 0)
			{
				break;
			}
			float num6 = ((float)num5 + 1f) / (float)num2 * num;
			int num7 = 0;
			if (LineBreaker._availableSpacePositions.Count > 0)
			{
				int num8 = LineBreaker._availableSpacePositions.Count - 1;
				if (num6 < LineBreaker._availableSpacePositions[0].posInSingleLine)
				{
					num7 = 0;
				}
				else if (num6 > LineBreaker._availableSpacePositions[num8].posInSingleLine)
				{
					num7 = num8;
				}
				else
				{
					int j = 0;
					int num9 = num8;
					while (j < num9)
					{
						int num10 = (j + num9) / 2 + 1;
						if (num6 < LineBreaker._availableSpacePositions[num10].posInSingleLine)
						{
							num9 = num10 - 1;
						}
						else
						{
							j = num10;
						}
					}
					num7 = j;
				}
			}
			LineBreaker.AvailableSpace availableSpace = LineBreaker._availableSpacePositions[num7];
			LineBreaker._availableSpacePositions.RemoveAt(num7);
			LineBreaker._lines.Add(new LineBreaker.Line
			{
				firstWordIdx = availableSpace.beforeWordIdx
			});
		}
		LineBreaker._lines.Sort((LineBreaker.Line l1, LineBreaker.Line l2) => l1.firstWordIdx - l2.firstWordIdx);
		float num11 = 0f;
		for (int k = 0; k < LineBreaker._lines.Count; k++)
		{
			LineBreaker.Line line = LineBreaker._lines[k];
			line.lastWordIdx = ((k == LineBreaker._lines.Count - 1) ? (wordWidths.Count - 1) : (LineBreaker._lines[k + 1].firstWordIdx - 1));
			for (int l = line.firstWordIdx; l <= line.lastWordIdx; l++)
			{
				line.width += wordWidths[l];
				if (l < line.lastWordIdx)
				{
					line.width += spaceWidth;
				}
			}
			if (line.width > num11)
			{
				num11 = line.width;
			}
			LineBreaker._lines[k] = line;
		}
		return new ValueTuple<List<LineBreaker.Line>, float>(LineBreaker._lines, num11);
	}

	// Token: 0x06001384 RID: 4996 RVA: 0x000897E0 File Offset: 0x000879E0
	public static Vector2 PositionWords(List<LineBreaker.Line> lines, List<float> wordWidths, float spaceWidth, float lineHeight, float maxLineWidth, List<Vector2> wordPositionsOut, TextAlignment align)
	{
		float num = 0f;
		for (int i = 0; i < lines.Count; i++)
		{
			LineBreaker.Line line = lines[i];
			float num2 = 0f;
			if (align == TextAlignment.Center)
			{
				num2 = maxLineWidth / 2f - line.width / 2f;
			}
			else if (align == TextAlignment.Right)
			{
				num2 = maxLineWidth - line.width;
			}
			for (int j = line.firstWordIdx; j <= line.lastWordIdx; j++)
			{
				wordPositionsOut.Add(new Vector2(num2, num));
				num2 += wordWidths[j] + spaceWidth;
			}
			num += lineHeight;
		}
		LineBreaker._lines.Clear();
		LineBreaker._availableSpacePositions.Clear();
		LineBreaker._lineBreakOrder.Clear();
		return new Vector2(maxLineWidth, num);
	}

	// Token: 0x040012C4 RID: 4804
	private static List<LineBreaker.AvailableSpace> _availableSpacePositions = new List<LineBreaker.AvailableSpace>(256);

	// Token: 0x040012C5 RID: 4805
	private static List<LineBreaker.Line> _lines = new List<LineBreaker.Line>(64);

	// Token: 0x040012C6 RID: 4806
	private static List<float> _widths = new List<float>(256);

	// Token: 0x040012C7 RID: 4807
	private static List<int> _lineBreakOrder = new List<int>(256);

	// Token: 0x040012C8 RID: 4808
	private static Stack<ValueTuple<int, int>> _minMaxStack = new Stack<ValueTuple<int, int>>();

	// Token: 0x02000415 RID: 1045
	public struct Line
	{
		// Token: 0x04001B10 RID: 6928
		public int firstWordIdx;

		// Token: 0x04001B11 RID: 6929
		public int lastWordIdx;

		// Token: 0x04001B12 RID: 6930
		public float width;
	}

	// Token: 0x02000416 RID: 1046
	private struct AvailableSpace
	{
		// Token: 0x04001B13 RID: 6931
		public int beforeWordIdx;

		// Token: 0x04001B14 RID: 6932
		public float posInSingleLine;
	}
}
