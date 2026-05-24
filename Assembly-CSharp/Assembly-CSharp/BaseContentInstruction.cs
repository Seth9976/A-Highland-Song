using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using UnityEngine;

// Token: 0x0200009B RID: 155
[Serializable]
public abstract class BaseContentInstruction : BaseInstruction
{
	// Token: 0x06000503 RID: 1283 RVA: 0x00028265 File Offset: 0x00026465
	public BaseContentInstruction()
	{
	}

	// Token: 0x06000504 RID: 1284 RVA: 0x00028270 File Offset: 0x00026470
	public override string ToString()
	{
		string text = "[";
		Type type = base.GetType();
		StringBuilder stringBuilder = new StringBuilder(text + ((type != null) ? type.ToString() : null) + "]\n");
		foreach (FieldInfo fieldInfo in base.GetType().GetFields())
		{
			object value = fieldInfo.GetValue(this);
			stringBuilder.Append(" ");
			stringBuilder.Append(fieldInfo.Name);
			stringBuilder.Append(":");
			stringBuilder.Append((value == null) ? "NULL" : value.ToString());
		}
		return stringBuilder.ToString();
	}

	// Token: 0x06000505 RID: 1285 RVA: 0x00028310 File Offset: 0x00026510
	public static bool TryParse(string rawContent, List<string> tags, out BaseContentInstruction scriptContent)
	{
		scriptContent = null;
		if (rawContent == null)
		{
			Debug.LogError("rawContent is null. This is not allowed.");
			return false;
		}
		scriptContent = DebuggingInstruction.TryParse(rawContent);
		if (scriptContent != null)
		{
			return true;
		}
		scriptContent = PauseInstruction.TryParse(rawContent);
		if (scriptContent != null)
		{
			return true;
		}
		scriptContent = AudioInstruction.TryParse(rawContent);
		if (scriptContent != null)
		{
			return true;
		}
		scriptContent = HealthInstruction.TryParse(rawContent);
		if (scriptContent != null)
		{
			return true;
		}
		scriptContent = DebugLogInstruction.TryParse(rawContent);
		if (scriptContent != null)
		{
			return true;
		}
		scriptContent = LegacyInstruction.TryParse(rawContent, tags);
		if (scriptContent != null)
		{
			return true;
		}
		scriptContent = ContentInstruction.TryParse(rawContent, tags);
		if (scriptContent != null)
		{
			return true;
		}
		Debug.LogWarning("Unexpected text in ink choice with no particular markup: " + rawContent);
		return false;
	}
}
