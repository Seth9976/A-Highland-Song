using System;
using Ink.Runtime;

// Token: 0x0200009A RID: 154
[Serializable]
public abstract class BaseChoiceInstruction : BaseInstruction
{
	// Token: 0x060004FF RID: 1279 RVA: 0x000281F8 File Offset: 0x000263F8
	public BaseChoiceInstruction(Choice storyChoice)
	{
		this.storyChoice = storyChoice;
	}

	// Token: 0x06000500 RID: 1280 RVA: 0x00028207 File Offset: 0x00026407
	public override string ToString()
	{
		string text = "[";
		Type type = base.GetType();
		return text + ((type != null) ? type.ToString() : null) + "]\n";
	}

	// Token: 0x06000501 RID: 1281 RVA: 0x0002822C File Offset: 0x0002642C
	public static BaseChoiceInstruction ParseChoice(Choice choice)
	{
		BaseChoiceInstruction baseChoiceInstruction = DialogueChoiceInstruction.TryParse(choice);
		if (baseChoiceInstruction != null)
		{
			return baseChoiceInstruction;
		}
		baseChoiceInstruction = GenericChoiceInstruction.TryParse(choice);
		if (baseChoiceInstruction != null)
		{
			return baseChoiceInstruction;
		}
		return null;
	}

	// Token: 0x06000502 RID: 1282 RVA: 0x00028254 File Offset: 0x00026454
	public static bool TryParse(Choice choice, out BaseChoiceInstruction scriptChoice)
	{
		scriptChoice = BaseChoiceInstruction.ParseChoice(choice);
		return scriptChoice != null;
	}

	// Token: 0x040005D5 RID: 1493
	public Choice storyChoice;
}
