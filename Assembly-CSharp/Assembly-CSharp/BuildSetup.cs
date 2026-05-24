using System;
using UnityEngine;

// Token: 0x02000015 RID: 21
public class BuildSetup : ScriptableObject
{
	// Token: 0x040000A6 RID: 166
	public bool steamEnabled;

	// Token: 0x040000A7 RID: 167
	public string visibleNameOnTitleScreen;

	// Token: 0x040000A8 RID: 168
	public bool inkTribecaExhibitionSetup;

	// Token: 0x040000A9 RID: 169
	public bool fastIntro;

	// Token: 0x040000AA RID: 170
	public bool demoEndGameSequence;

	// Token: 0x040000AB RID: 171
	public int levelLimit;

	// Token: 0x040000AC RID: 172
	public bool demoNeverLoadOnTitleScreen;

	// Token: 0x040000AD RID: 173
	public bool preventQuit;

	// Token: 0x040000AE RID: 174
	[Space]
	public bool timeoutAfterInactivity;

	// Token: 0x040000AF RID: 175
	public float inactivityTimeoutSeconds = 60f;

	// Token: 0x040000B0 RID: 176
	public float dialogueSecondsAfterTimeout = 30f;

	// Token: 0x040000B1 RID: 177
	[Space]
	public bool hasTestMenu;

	// Token: 0x040000B2 RID: 178
	public bool trailerFeatures;

	// Token: 0x040000B3 RID: 179
	public GameSetup newGameSetup;
}
