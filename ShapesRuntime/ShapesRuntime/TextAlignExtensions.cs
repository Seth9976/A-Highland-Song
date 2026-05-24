using System;
using TMPro;
using UnityEngine;

namespace Shapes
{
	// Token: 0x02000058 RID: 88
	public static class TextAlignExtensions
	{
		// Token: 0x06000A1A RID: 2586 RVA: 0x000242BC File Offset: 0x000224BC
		public static Vector2 GetPivot(this TextAlign align)
		{
			switch (align)
			{
			case TextAlign.TopLeft:
				return new Vector2(0f, 1f);
			case TextAlign.Top:
				return new Vector2(0.5f, 1f);
			case TextAlign.TopRight:
				return new Vector2(1f, 1f);
			case TextAlign.Left:
				return new Vector2(0f, 0.5f);
			case TextAlign.Center:
				return new Vector2(0.5f, 0.5f);
			case TextAlign.Right:
				return new Vector2(1f, 0.5f);
			case TextAlign.BottomLeft:
				return new Vector2(0f, 0f);
			case TextAlign.Bottom:
				return new Vector2(0.5f, 0f);
			case TextAlign.BottomRight:
				return new Vector2(1f, 0f);
			default:
				return default(Vector2);
			}
		}

		// Token: 0x06000A1B RID: 2587 RVA: 0x00024394 File Offset: 0x00022594
		public static TextAlignmentOptions GetTMPAlignment(this TextAlign align)
		{
			switch (align)
			{
			case TextAlign.TopLeft:
				return TextAlignmentOptions.TopLeft;
			case TextAlign.Top:
				return TextAlignmentOptions.Top;
			case TextAlign.TopRight:
				return TextAlignmentOptions.TopRight;
			case TextAlign.Left:
				return TextAlignmentOptions.Left;
			case TextAlign.Center:
				return TextAlignmentOptions.Center;
			case TextAlign.Right:
				return TextAlignmentOptions.Right;
			case TextAlign.BottomLeft:
				return TextAlignmentOptions.BottomLeft;
			case TextAlign.Bottom:
				return TextAlignmentOptions.Bottom;
			case TextAlign.BottomRight:
				return TextAlignmentOptions.BottomRight;
			default:
				return (TextAlignmentOptions)0;
			}
		}
	}
}
