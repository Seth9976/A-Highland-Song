using System;
using System.Collections.Generic;

// Token: 0x02000005 RID: 5
public class GameSetupManager : MonoSingleton<GameSetupManager>
{
	// Token: 0x04000003 RID: 3
	public GameSetup initialGameSetup;

	// Token: 0x04000004 RID: 4
	[Disable]
	public List<GameSetup> gameSetups = new List<GameSetup>();
}
