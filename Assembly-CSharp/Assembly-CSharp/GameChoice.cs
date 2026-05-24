using System;
using System.Collections.Generic;

// Token: 0x0200008A RID: 138
[Serializable]
public struct GameChoice
{
	// Token: 0x17000115 RID: 277
	// (get) Token: 0x060003EB RID: 1003 RVA: 0x00020060 File Offset: 0x0001E260
	public bool isMapChoice
	{
		get
		{
			return this.specialType == GameChoiceSpecialType.ConfirmMapSuccess || this.specialType == GameChoiceSpecialType.ConfirmMapFail;
		}
	}

	// Token: 0x17000116 RID: 278
	// (get) Token: 0x060003EC RID: 1004 RVA: 0x00020076 File Offset: 0x0001E276
	public bool isValid
	{
		get
		{
			return this.type > GameChoiceType.None;
		}
	}

	// Token: 0x060003ED RID: 1005 RVA: 0x00020084 File Offset: 0x0001E284
	public bool IsEquivalentTo(GameChoice otherChoice)
	{
		if (this.type != otherChoice.type)
		{
			return false;
		}
		if (this.text != otherChoice.text)
		{
			return false;
		}
		if (this.specialType != otherChoice.specialType)
		{
			return false;
		}
		if (this.interactableNames == null != (otherChoice.interactableNames == null))
		{
			return false;
		}
		if (this.interactableNames != null)
		{
			if (this.interactableNames.Count != otherChoice.interactableNames.Count)
			{
				return false;
			}
			for (int i = 0; i < this.interactableNames.Count; i++)
			{
				if (this.interactableNames[i] != otherChoice.interactableNames[i])
				{
					return false;
				}
			}
		}
		return !this.isMapChoice || !(this.specificMapInkName != otherChoice.specificMapInkName);
	}

	// Token: 0x0400053B RID: 1339
	public GameChoiceType type;

	// Token: 0x0400053C RID: 1340
	public string text;

	// Token: 0x0400053D RID: 1341
	public GameChoiceSpecialType specialType;

	// Token: 0x0400053E RID: 1342
	public int inkChoiceIdx;

	// Token: 0x0400053F RID: 1343
	public List<string> interactableNames;

	// Token: 0x04000540 RID: 1344
	public string specificMapInkName;

	// Token: 0x04000541 RID: 1345
	public ChoiceIcon icon;
}
