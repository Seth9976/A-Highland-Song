using System;
using TMPro;
using UnityEngine;

// Token: 0x02000152 RID: 338
[ExecuteInEditMode]
public static class TextMeshProUtils
{
	// Token: 0x06000B76 RID: 2934 RVA: 0x0005C588 File Offset: 0x0005A788
	public static string ReplaceUnsupportedQuoteMarks(string textString, TMP_FontAsset font)
	{
		if (!font.glyphLookupTable.ContainsKey(8221U) || !font.glyphLookupTable.ContainsKey(8220U))
		{
			textString = textString.Replace('”', '"');
			textString = textString.Replace('“', '"');
		}
		if (!font.glyphLookupTable.ContainsKey(8217U))
		{
			textString = textString.Replace('’', '\'');
		}
		return textString;
	}

	// Token: 0x06000B77 RID: 2935 RVA: 0x0005C5FC File Offset: 0x0005A7FC
	public static float GetTotalWidth(TMP_TextInfo textInfo, bool visibleOnly)
	{
		float num = 0f;
		for (int i = 0; i < textInfo.characterInfo.Length; i++)
		{
			TMP_CharacterInfo tmp_CharacterInfo = textInfo.characterInfo[i];
			if (!visibleOnly || tmp_CharacterInfo.isVisible)
			{
				float characterWidth = TextMeshProUtils.GetCharacterWidth(tmp_CharacterInfo);
				num += characterWidth;
			}
		}
		return num;
	}

	// Token: 0x06000B78 RID: 2936 RVA: 0x0005C646 File Offset: 0x0005A846
	public static float GetCharacterWidth(TMP_CharacterInfo characterInfo)
	{
		return characterInfo.topRight.x - characterInfo.topLeft.x;
	}

	// Token: 0x06000B79 RID: 2937 RVA: 0x0005C660 File Offset: 0x0005A860
	public static int GetNumCharacters(TMP_TextInfo textInfo, bool visibleOnly)
	{
		if (!visibleOnly)
		{
			return textInfo.characterInfo.Length;
		}
		int num = 0;
		for (int i = 0; i < textInfo.characterInfo.Length; i++)
		{
			if (textInfo.characterInfo[i].isVisible)
			{
				num++;
			}
		}
		return num;
	}

	// Token: 0x06000B7A RID: 2938 RVA: 0x0005C6A8 File Offset: 0x0005A8A8
	public static bool IsAnyWordSplit(TMP_TextInfo textInfo)
	{
		for (int i = 0; i < textInfo.characterInfo.Length - 1; i++)
		{
			if (textInfo.characterInfo[i].lineNumber != textInfo.characterInfo[i + 1].lineNumber)
			{
				char character = textInfo.characterInfo[i].character;
				if (!char.IsSeparator(character) && !char.IsPunctuation(character))
				{
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x06000B7B RID: 2939 RVA: 0x0005C718 File Offset: 0x0005A918
	public static Vector2 GetRenderedValues(this TMP_Text textMeshPro, string text, float maxWidth, float maxHeight, bool onlyVisibleCharacters = true)
	{
		if (string.IsNullOrEmpty(text))
		{
			return Vector2.zero;
		}
		TextRenderFlags renderMode = textMeshPro.renderMode;
		string text2 = textMeshPro.text;
		Vector2 sizeDelta = textMeshPro.rectTransform.sizeDelta;
		textMeshPro.renderMode = TextRenderFlags.DontRender;
		textMeshPro.text = text;
		textMeshPro.rectTransform.sizeDelta = new Vector2(maxWidth, maxHeight);
		textMeshPro.ForceMeshUpdate(true, false);
		if (text.Length == 0)
		{
			return Vector2.zero;
		}
		Vector2 renderedValues = textMeshPro.GetRenderedValues(onlyVisibleCharacters);
		if (float.IsNaN(renderedValues.x) || float.IsNaN(renderedValues.y))
		{
			Vector2 preferredValues = textMeshPro.GetPreferredValues(text, maxWidth, maxHeight);
			if (float.IsNaN(renderedValues.x))
			{
				renderedValues.x = preferredValues.x;
			}
			if (float.IsNaN(renderedValues.y))
			{
				renderedValues.y = preferredValues.y;
			}
		}
		textMeshPro.renderMode = renderMode;
		textMeshPro.text = text2;
		textMeshPro.rectTransform.sizeDelta = sizeDelta;
		textMeshPro.ForceMeshUpdate(true, false);
		return renderedValues;
	}

	// Token: 0x06000B7C RID: 2940 RVA: 0x0005C80C File Offset: 0x0005AA0C
	public static Vector2 GetTightPreferredValues(this TMP_Text textMeshPro, string text, float maxWidth, float maxHeight)
	{
		Vector2 preferredValues = textMeshPro.GetPreferredValues(text, maxWidth, maxHeight);
		preferredValues.x = Mathf.Min(preferredValues.x, maxWidth);
		preferredValues.y = Mathf.Min(preferredValues.y, maxHeight);
		return preferredValues;
	}

	// Token: 0x06000B7D RID: 2941 RVA: 0x0005C84C File Offset: 0x0005AA4C
	public static Vector2 GetBestFitWidth(this TMP_Text textMeshPro, float targetHeight, float widthStep)
	{
		textMeshPro.renderMode = TextRenderFlags.DontRender;
		Vector2 sizeDelta = textMeshPro.rectTransform.sizeDelta;
		float num = 0f;
		textMeshPro.rectTransform.sizeDelta = new Vector2(num, targetHeight);
		textMeshPro.ForceMeshUpdate(true, false);
		int num2 = 0;
		while (textMeshPro.isTextOverflowing)
		{
			num += widthStep;
			textMeshPro.rectTransform.sizeDelta = new Vector2(num, targetHeight);
			textMeshPro.ForceMeshUpdate(true, false);
			num2++;
			if (num2 > 50)
			{
				Debug.LogError("Max num iterations reached for GetBestFitWidth with targetHeight " + targetHeight.ToString() + " and widthStep " + widthStep.ToString());
			}
		}
		ref Vector2 renderedValues = textMeshPro.GetRenderedValues(true);
		textMeshPro.renderMode = TextRenderFlags.Render;
		textMeshPro.rectTransform.sizeDelta = sizeDelta;
		return new Vector2(renderedValues.x, targetHeight);
	}
}
