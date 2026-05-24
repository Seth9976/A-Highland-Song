using System;

// Token: 0x02000130 RID: 304
public class OnwardPromptUI : PromptWithVisibility
{
	// Token: 0x06000A44 RID: 2628 RVA: 0x00055CAD File Offset: 0x00053EAD
	protected override bool ShouldShow()
	{
		return Game.instance.inPeakState && MonoSingleton<PeakStateController>.instance.allowExit;
	}
}
