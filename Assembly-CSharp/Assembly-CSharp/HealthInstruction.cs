using System;
using System.Text.RegularExpressions;
using UnityEngine;

// Token: 0x02000098 RID: 152
[Serializable]
public class HealthInstruction : BaseContentInstruction
{
	// Token: 0x17000147 RID: 327
	// (get) Token: 0x060004F8 RID: 1272 RVA: 0x0002807B File Offset: 0x0002627B
	private static Regex parserRegex
	{
		get
		{
			if (HealthInstruction._parserRegex == null)
			{
				HealthInstruction._parserRegex = new Regex(InkInstructionParserUtility.BuildInstructionPrefixRegex("HEALTH"), RegexOptions.IgnoreCase);
			}
			return HealthInstruction._parserRegex;
		}
	}

	// Token: 0x060004F9 RID: 1273 RVA: 0x000280A0 File Offset: 0x000262A0
	public static HealthInstruction TryParse(string rawContent)
	{
		Match match = HealthInstruction.parserRegex.Match(rawContent);
		if (!match.Success)
		{
			return null;
		}
		HealthInstruction healthInstruction = new HealthInstruction();
		InkInstructionParserUtility.ParsedArgumentCollection parsedArgumentCollection = InkInstructionParserUtility.ParseArguments(match.Groups[1].Value);
		parsedArgumentCollection.TryGetValue<DamageType>("damageType", ref healthInstruction.damageType);
		parsedArgumentCollection.TryGetValue<Damage>("damage", ref healthInstruction.damage);
		if (parsedArgumentCollection.TryGetValue<int>("healing", ref healthInstruction.healing) && healthInstruction.healing <= 0)
		{
			Debug.LogWarning("Healing was " + healthInstruction.healing.ToString() + ". Healing should always be a positive number");
		}
		return healthInstruction;
	}

	// Token: 0x040005CF RID: 1487
	private static Regex _parserRegex;

	// Token: 0x040005D0 RID: 1488
	public DamageType damageType;

	// Token: 0x040005D1 RID: 1489
	public Damage damage;

	// Token: 0x040005D2 RID: 1490
	public int healing = -1;
}
