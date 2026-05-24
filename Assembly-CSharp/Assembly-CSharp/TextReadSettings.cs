using System;

// Token: 0x020000A3 RID: 163
[Serializable]
public class TextReadSettings
{
	// Token: 0x04000607 RID: 1543
	public float extraTimeConstant = 0.5f;

	// Token: 0x04000608 RID: 1544
	public float characterReadDuration = 0.045f;

	// Token: 0x04000609 RID: 1545
	public float terminatorDuration = 0.1f;

	// Token: 0x0400060A RID: 1546
	public float minDuration = 1f;

	// Token: 0x0400060B RID: 1547
	public float maxDuration = 4f;
}
