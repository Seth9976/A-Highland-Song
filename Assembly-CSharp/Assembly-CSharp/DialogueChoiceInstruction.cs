using System;
using Ink.Runtime;

// Token: 0x02000091 RID: 145
public class DialogueChoiceInstruction : BaseChoiceInstruction
{
	// Token: 0x060004E5 RID: 1253 RVA: 0x00027D88 File Offset: 0x00025F88
	public static DialogueChoiceInstruction TryParse(Choice choice)
	{
		string text;
		string text2;
		if (!InkInstructionParserUtility.TryParseDialogue(choice.text, out text, out text2))
		{
			return null;
		}
		text2 = InkStylingUtility.ProcessText(text2, true, false);
		text2 = InkStylingUtility.ParseStyling(text2, true, "#FFE473");
		if (string.IsNullOrEmpty(text2))
		{
			return null;
		}
		return new DialogueChoiceInstruction(choice, text, text2);
	}

	// Token: 0x060004E6 RID: 1254 RVA: 0x00027DD0 File Offset: 0x00025FD0
	public DialogueChoiceInstruction(Choice storyChoice, string speaker, string text)
		: base(storyChoice)
	{
		this.speaker = speaker;
		this.text = text;
	}

	// Token: 0x040005BF RID: 1471
	public string speaker;

	// Token: 0x040005C0 RID: 1472
	public string text;
}
