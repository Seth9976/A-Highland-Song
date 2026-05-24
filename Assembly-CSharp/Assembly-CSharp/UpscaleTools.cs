using System;
using System.Runtime.CompilerServices;
using UnityEngine;

// Token: 0x020001E1 RID: 481
public static class UpscaleTools
{
	// Token: 0x060010EB RID: 4331 RVA: 0x0007CB40 File Offset: 0x0007AD40
	public static Color[] UpscaleBoolMap(Vector2Int sourceMapSize, bool[] sourceMap, bool defaultIsTrue)
	{
		UpscaleTools.<>c__DisplayClass0_0 CS$<>8__locals1;
		CS$<>8__locals1.sourceMap = sourceMap;
		CS$<>8__locals1.sourceMapSize = sourceMapSize;
		Color black = Color.black;
		Color white = Color.white;
		Color grey = Color.grey;
		Vector2Int vector2Int = new Vector2Int(CS$<>8__locals1.sourceMapSize.x * 4, CS$<>8__locals1.sourceMapSize.y * 4);
		Color[] array = new Color[vector2Int.x * vector2Int.y];
		for (int i = 0; i < CS$<>8__locals1.sourceMapSize.x; i++)
		{
			for (int j = 0; j < CS$<>8__locals1.sourceMapSize.y; j++)
			{
				bool flag = UpscaleTools.<UpscaleBoolMap>g__GetValueAtGridPoint|0_0(i, j, ref CS$<>8__locals1);
				if (i == 0 && j == 0)
				{
					Color color = (flag ? white : black);
					RectInt rectInt = new RectInt(i * 4, j * 4, 4, 4);
					UpscaleTools.SetMapValues(array, vector2Int.x, rectInt, color);
				}
				if (i == 0 && j == CS$<>8__locals1.sourceMapSize.y - 1)
				{
					Color color2 = (flag ? white : black);
					RectInt rectInt2 = new RectInt(i * 4, j * 4, 4, 4);
					UpscaleTools.SetMapValues(array, vector2Int.x, rectInt2, color2);
				}
				if (i == CS$<>8__locals1.sourceMapSize.x - 1 && j == 0)
				{
					Color color3 = (flag ? white : black);
					RectInt rectInt3 = new RectInt(i * 4, j * 4, 4, 4);
					UpscaleTools.SetMapValues(array, vector2Int.x, rectInt3, color3);
				}
				if (i == CS$<>8__locals1.sourceMapSize.x - 1 && j == CS$<>8__locals1.sourceMapSize.y - 1)
				{
					Color color4 = (flag ? white : black);
					RectInt rectInt4 = new RectInt(i * 4, j * 4, 4, 4);
					UpscaleTools.SetMapValues(array, vector2Int.x, rectInt4, color4);
				}
				if (i == 0 && j > 0 && j < CS$<>8__locals1.sourceMapSize.y - 1)
				{
					Color color5 = (flag ? white : black);
					RectInt rectInt5 = new RectInt(i * 2, j * 4, 2, 4);
					UpscaleTools.SetMapValues(array, vector2Int.x, rectInt5, color5);
				}
				if (i == CS$<>8__locals1.sourceMapSize.x - 1 && j > 0 && j < CS$<>8__locals1.sourceMapSize.y - 1)
				{
					Color color6 = (flag ? white : black);
					RectInt rectInt6 = new RectInt(2 + i * 4, j * 4, 2, 4);
					UpscaleTools.SetMapValues(array, vector2Int.x, rectInt6, color6);
				}
				if (j == 0 && i > 0 && i < CS$<>8__locals1.sourceMapSize.x - 1)
				{
					Color color7 = (flag ? white : black);
					RectInt rectInt7 = new RectInt(i * 4, j * 2, 4, 2);
					UpscaleTools.SetMapValues(array, vector2Int.x, rectInt7, color7);
				}
				if (j == CS$<>8__locals1.sourceMapSize.y - 1 && i > 0 && i < CS$<>8__locals1.sourceMapSize.x - 1)
				{
					Color color8 = (flag ? white : black);
					RectInt rectInt8 = new RectInt(i * 4, 2 + j * 4, 4, 2);
					UpscaleTools.SetMapValues(array, vector2Int.x, rectInt8, color8);
				}
				if (i < CS$<>8__locals1.sourceMapSize.x - 1)
				{
					bool flag2 = j < CS$<>8__locals1.sourceMapSize.y - 1;
				}
				RectInt rectInt9 = new RectInt(2 + i * 4, 2 + j * 4, 4, 4);
				bool flag3 = (UpscaleTools.<UpscaleBoolMap>g__IsOnVisibilityMap|0_1(i + 1, j) ? UpscaleTools.<UpscaleBoolMap>g__GetValueAtGridPoint|0_0(i + 1, j, ref CS$<>8__locals1) : flag);
				bool flag4 = (UpscaleTools.<UpscaleBoolMap>g__IsOnVisibilityMap|0_1(i, j + 1) ? UpscaleTools.<UpscaleBoolMap>g__GetValueAtGridPoint|0_0(i, j + 1, ref CS$<>8__locals1) : flag);
				bool flag5 = (UpscaleTools.<UpscaleBoolMap>g__IsOnVisibilityMap|0_1(i + 1, j + 1) ? UpscaleTools.<UpscaleBoolMap>g__GetValueAtGridPoint|0_0(i + 1, j + 1, ref CS$<>8__locals1) : flag);
				int num = (flag ? 0 : 1);
				num += (flag3 ? 0 : 2);
				num += (flag4 ? 0 : 4);
				num += (flag5 ? 0 : 8);
				if (num == 0)
				{
					UpscaleTools.SetMapValues(array, vector2Int.x, rectInt9, new Color[]
					{
						white, white, white, white, white, white, white, white, white, white,
						white, white, white, white, white, white
					});
				}
				else if (num == 1)
				{
					UpscaleTools.SetMapValues(array, vector2Int.x, rectInt9, new Color[]
					{
						black, grey, white, white, grey, white, white, white, white, white,
						white, white, white, white, white, white
					});
				}
				else if (num == 2)
				{
					UpscaleTools.SetMapValues(array, vector2Int.x, rectInt9, new Color[]
					{
						white, white, grey, black, white, white, white, grey, white, white,
						white, white, white, white, white, white
					});
				}
				else if (num == 3)
				{
					UpscaleTools.SetMapValues(array, vector2Int.x, rectInt9, new Color[]
					{
						black, black, black, black, black, black, black, black, white, white,
						white, white, white, white, white, white
					});
				}
				else if (num == 4)
				{
					UpscaleTools.SetMapValues(array, vector2Int.x, rectInt9, new Color[]
					{
						white, white, white, white, white, white, white, white, grey, white,
						white, white, black, grey, white, white
					});
				}
				else if (num == 5)
				{
					UpscaleTools.SetMapValues(array, vector2Int.x, rectInt9, new Color[]
					{
						black, black, white, white, black, black, white, white, black, black,
						white, white, black, black, white, white
					});
				}
				else if (num == 6)
				{
					UpscaleTools.SetMapValues(array, vector2Int.x, rectInt9, new Color[]
					{
						white, white, grey, black, white, white, white, grey, grey, white,
						white, white, black, grey, white, white
					});
				}
				else if (num == 7)
				{
					UpscaleTools.SetMapValues(array, vector2Int.x, rectInt9, new Color[]
					{
						black, black, black, black, black, black, black, black, black, black,
						black, grey, black, black, grey, white
					});
				}
				else if (num == 8)
				{
					UpscaleTools.SetMapValues(array, vector2Int.x, rectInt9, new Color[]
					{
						white, white, white, white, white, white, white, white, white, white,
						white, grey, white, white, grey, black
					});
				}
				else if (num == 9)
				{
					UpscaleTools.SetMapValues(array, vector2Int.x, rectInt9, new Color[]
					{
						black, grey, white, white, grey, white, white, white, white, white,
						white, grey, white, white, grey, black
					});
				}
				else if (num == 10)
				{
					UpscaleTools.SetMapValues(array, vector2Int.x, rectInt9, new Color[]
					{
						white, white, black, black, white, white, black, black, white, white,
						black, black, white, white, black, black
					});
				}
				else if (num == 11)
				{
					UpscaleTools.SetMapValues(array, vector2Int.x, rectInt9, new Color[]
					{
						black, black, black, black, black, black, black, black, grey, black,
						black, black, white, grey, black, black
					});
				}
				else if (num == 12)
				{
					UpscaleTools.SetMapValues(array, vector2Int.x, rectInt9, new Color[]
					{
						white, white, white, white, white, white, white, white, black, black,
						black, black, black, black, black, black
					});
				}
				else if (num == 13)
				{
					UpscaleTools.SetMapValues(array, vector2Int.x, rectInt9, new Color[]
					{
						black, black, grey, white, black, black, black, grey, black, black,
						black, black, black, black, black, black
					});
				}
				else if (num == 14)
				{
					UpscaleTools.SetMapValues(array, vector2Int.x, rectInt9, new Color[]
					{
						white, grey, black, black, grey, black, black, black, black, black,
						black, black, black, black, black, black
					});
				}
				else if (num == 15)
				{
					UpscaleTools.SetMapValues(array, vector2Int.x, rectInt9, new Color[]
					{
						black, black, black, black, black, black, black, black, black, black,
						black, black, black, black, black, black
					});
				}
			}
		}
		return array;
	}

	// Token: 0x060010EC RID: 4332 RVA: 0x0007D9C8 File Offset: 0x0007BBC8
	private static int GridPointToArrayIndex(int x, int y, int width)
	{
		return y * width + x;
	}

	// Token: 0x060010ED RID: 4333 RVA: 0x0007D9D0 File Offset: 0x0007BBD0
	private static void SetMapValues(Color[] map, int maxWidth, RectInt pointRect, Color[] vals)
	{
		int num = 0;
		for (int i = 0; i < pointRect.height; i++)
		{
			for (int j = 0; j < pointRect.width; j++)
			{
				Vector2Int vector2Int = new Vector2Int(pointRect.x + j, pointRect.y + i);
				int num2 = UpscaleTools.GridPointToArrayIndex(vector2Int.x, vector2Int.y, maxWidth);
				map[num2] = vals[num];
				num++;
			}
		}
	}

	// Token: 0x060010EE RID: 4334 RVA: 0x0007DA48 File Offset: 0x0007BC48
	private static void SetMapValues(Color[] map, int maxWidth, RectInt pointRect, Color val)
	{
		int num = 0;
		for (int i = 0; i < pointRect.height; i++)
		{
			for (int j = 0; j < pointRect.width; j++)
			{
				Vector2Int vector2Int = new Vector2Int(pointRect.x + j, pointRect.y + i);
				int num2 = UpscaleTools.GridPointToArrayIndex(vector2Int.x, vector2Int.y, maxWidth);
				map[num2] = val;
				num++;
			}
		}
	}

	// Token: 0x060010EF RID: 4335 RVA: 0x0007DAB7 File Offset: 0x0007BCB7
	[CompilerGenerated]
	internal static bool <UpscaleBoolMap>g__GetValueAtGridPoint|0_0(int x, int y, ref UpscaleTools.<>c__DisplayClass0_0 A_2)
	{
		return A_2.sourceMap[UpscaleTools.GridPointToArrayIndex(x, y, A_2.sourceMapSize.x)];
	}

	// Token: 0x060010F0 RID: 4336 RVA: 0x0007DAD2 File Offset: 0x0007BCD2
	[CompilerGenerated]
	internal static bool <UpscaleBoolMap>g__IsOnVisibilityMap|0_1(int x, int y)
	{
		return true;
	}
}
