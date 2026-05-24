using System;
using Ink.Runtime;

// Token: 0x02000092 RID: 146
[Serializable]
public class GenericChoiceInstruction : BaseChoiceInstruction
{
	// Token: 0x060004E7 RID: 1255 RVA: 0x00027DE8 File Offset: 0x00025FE8
	public static GenericChoiceInstruction TryParse(Choice choice)
	{
		if (string.IsNullOrWhiteSpace(choice.text))
		{
			return null;
		}
		InkInstructionParserUtility.ParsedArgumentCollection parsedArgumentCollection = InkInstructionParserUtility.ParseArguments(choice.text);
		return new GenericChoiceInstruction(choice)
		{
			text = InkStylingUtility.ParseStyling(InkStylingUtility.ProcessText(parsedArgumentCollection.textBeforeArguments, false, false), true, "#FFE473")
		};
	}

	// Token: 0x060004E8 RID: 1256 RVA: 0x00027E34 File Offset: 0x00026034
	public GenericChoiceInstruction(Choice storyChoice)
		: base(storyChoice)
	{
	}

	// Token: 0x060004E9 RID: 1257 RVA: 0x00027E3D File Offset: 0x0002603D
	public override string ToString()
	{
		return string.Format("[" + base.GetType().Name + "] text {0}", this.text);
	}

	// Token: 0x040005C1 RID: 1473
	public string text;
}
