using System;
using System.Collections.Generic;
using UnityEngine.Yoga;

namespace UnityEngine.UIElements
{
	// Token: 0x0200028E RID: 654
	internal static class StyleValueExtensions
	{
		// Token: 0x06001576 RID: 5494 RVA: 0x0005B974 File Offset: 0x00059B74
		internal static string DebugString<T>(this IStyleValue<T> styleValue)
		{
			return (styleValue.keyword != StyleKeyword.Undefined) ? string.Format("{0}", styleValue.keyword) : string.Format("{0}", styleValue.value);
		}

		// Token: 0x06001577 RID: 5495 RVA: 0x0005B9BC File Offset: 0x00059BBC
		internal static YogaValue ToYogaValue(this Length length)
		{
			bool flag = length.IsAuto();
			YogaValue yogaValue;
			if (flag)
			{
				yogaValue = YogaValue.Auto();
			}
			else
			{
				bool flag2 = length.IsNone();
				if (flag2)
				{
					yogaValue = float.NaN;
				}
				else
				{
					LengthUnit unit = length.unit;
					LengthUnit lengthUnit = unit;
					if (lengthUnit != LengthUnit.Pixel)
					{
						if (lengthUnit != LengthUnit.Percent)
						{
							Debug.LogAssertion(string.Format("Unexpected unit '{0}'", length.unit));
							yogaValue = float.NaN;
						}
						else
						{
							yogaValue = YogaValue.Percent(length.value);
						}
					}
					else
					{
						yogaValue = YogaValue.Point(length.value);
					}
				}
			}
			return yogaValue;
		}

		// Token: 0x06001578 RID: 5496 RVA: 0x0005BA54 File Offset: 0x00059C54
		internal static Length ToLength(this StyleKeyword keyword)
		{
			StyleKeyword styleKeyword = keyword;
			StyleKeyword styleKeyword2 = styleKeyword;
			Length length;
			if (styleKeyword2 != StyleKeyword.Auto)
			{
				if (styleKeyword2 != StyleKeyword.None)
				{
					Debug.LogAssertion("Unexpected StyleKeyword '" + keyword.ToString() + "'");
					length = default(Length);
				}
				else
				{
					length = Length.None();
				}
			}
			else
			{
				length = Length.Auto();
			}
			return length;
		}

		// Token: 0x06001579 RID: 5497 RVA: 0x0005BAB4 File Offset: 0x00059CB4
		internal static Rotate ToRotate(this StyleKeyword keyword)
		{
			StyleKeyword styleKeyword = keyword;
			StyleKeyword styleKeyword2 = styleKeyword;
			Rotate rotate;
			if (styleKeyword2 != StyleKeyword.None)
			{
				Debug.LogAssertion("Unexpected StyleKeyword '" + keyword.ToString() + "'");
				rotate = default(Rotate);
			}
			else
			{
				rotate = Rotate.None();
			}
			return rotate;
		}

		// Token: 0x0600157A RID: 5498 RVA: 0x0005BB04 File Offset: 0x00059D04
		internal static Scale ToScale(this StyleKeyword keyword)
		{
			StyleKeyword styleKeyword = keyword;
			StyleKeyword styleKeyword2 = styleKeyword;
			Scale scale;
			if (styleKeyword2 != StyleKeyword.None)
			{
				Debug.LogAssertion("Unexpected StyleKeyword '" + keyword.ToString() + "'");
				scale = default(Scale);
			}
			else
			{
				scale = Scale.None();
			}
			return scale;
		}

		// Token: 0x0600157B RID: 5499 RVA: 0x0005BB54 File Offset: 0x00059D54
		internal static Translate ToTranslate(this StyleKeyword keyword)
		{
			StyleKeyword styleKeyword = keyword;
			StyleKeyword styleKeyword2 = styleKeyword;
			Translate translate;
			if (styleKeyword2 != StyleKeyword.None)
			{
				Debug.LogAssertion("Unexpected StyleKeyword '" + keyword.ToString() + "'");
				translate = default(Translate);
			}
			else
			{
				translate = Translate.None();
			}
			return translate;
		}

		// Token: 0x0600157C RID: 5500 RVA: 0x0005BBA4 File Offset: 0x00059DA4
		internal static Length ToLength(this StyleLength styleLength)
		{
			StyleKeyword keyword = styleLength.keyword;
			StyleKeyword styleKeyword = keyword;
			Length length;
			if (styleKeyword - StyleKeyword.Auto > 1)
			{
				length = styleLength.value;
			}
			else
			{
				length = styleLength.keyword.ToLength();
			}
			return length;
		}

		// Token: 0x0600157D RID: 5501 RVA: 0x0005BBDE File Offset: 0x00059DDE
		internal static void CopyFrom<T>(this List<T> list, List<T> other)
		{
			list.Clear();
			list.AddRange(other);
		}
	}
}
