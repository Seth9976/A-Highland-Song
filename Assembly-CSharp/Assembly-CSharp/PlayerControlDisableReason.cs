using System;

// Token: 0x020000CB RID: 203
[Flags]
public enum PlayerControlDisableReason
{
	// Token: 0x04000810 RID: 2064
	None = 0,
	// Token: 0x04000811 RID: 2065
	Ink = 1,
	// Token: 0x04000812 RID: 2066
	InkPose = 2,
	// Token: 0x04000813 RID: 2067
	FollowPath = 4,
	// Token: 0x04000814 RID: 2068
	NarrativeChoicesWithNoExit = 8,
	// Token: 0x04000815 RID: 2069
	Peak = 16,
	// Token: 0x04000816 RID: 2070
	AutoRunToProp = 32,
	// Token: 0x04000817 RID: 2071
	Blackout = 64,
	// Token: 0x04000818 RID: 2072
	MapsTransition = 256,
	// Token: 0x04000819 RID: 2073
	JournalMapConfirm = 512,
	// Token: 0x0400081A RID: 2074
	TitleScreenAndIntro = 1024,
	// Token: 0x0400081B RID: 2075
	EndOfGameSequence = 2048,
	// Token: 0x0400081C RID: 2076
	TestMenu = 4096,
	// Token: 0x0400081D RID: 2077
	Feedback = 8192,
	// Token: 0x0400081E RID: 2078
	FreeCamera = 16384,
	// Token: 0x0400081F RID: 2079
	LookFurther = 32768
}
