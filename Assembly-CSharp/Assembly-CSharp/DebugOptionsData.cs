using System;
using UnityEngine;
using UnityEngine.Serialization;

// Token: 0x02000018 RID: 24
public class DebugOptionsData : ScriptableObject
{
	// Token: 0x040000B6 RID: 182
	public bool trackBarAndTimeGUI;

	// Token: 0x040000B7 RID: 183
	public bool forceGamepadUI;

	// Token: 0x040000B8 RID: 184
	public bool allowDebugSmallRockChunks;

	// Token: 0x040000B9 RID: 185
	public bool debugFlyingAllowed = true;

	// Token: 0x040000BA RID: 186
	public bool resetMusic;

	// Token: 0x040000BB RID: 187
	public bool dontChooseNewMusic;

	// Token: 0x040000BC RID: 188
	public bool beatMarkers;

	// Token: 0x040000BD RID: 189
	public bool chunkStartMarkers;

	// Token: 0x040000BE RID: 190
	public bool disableSwitchbacks = true;

	// Token: 0x040000BF RID: 191
	public bool dontCreateTrack;

	// Token: 0x040000C0 RID: 192
	public bool dontDestroyOutOfBoundsTrack;

	// Token: 0x040000C1 RID: 193
	public bool dontAbsorbErrorInMargins;

	// Token: 0x040000C2 RID: 194
	public bool pauseWhenEdgeOfSpaceNotFound;

	// Token: 0x040000C3 RID: 195
	public bool pauseOnFall;

	// Token: 0x040000C4 RID: 196
	public bool drawTrackCreationZoneDebugLines;

	// Token: 0x040000C5 RID: 197
	[FormerlySerializedAs("dontRemark")]
	public bool dontLullRemark;

	// Token: 0x040000C6 RID: 198
	public bool staminaFreeJumps = true;

	// Token: 0x040000C7 RID: 199
	public bool infiniteStamina;

	// Token: 0x040000C8 RID: 200
	public bool neverSlip;

	// Token: 0x040000C9 RID: 201
	public bool fastDeathReset;

	// Token: 0x040000CA RID: 202
	public bool godMode;

	// Token: 0x040000CB RID: 203
	public bool unlockAllMaps;

	// Token: 0x040000CC RID: 204
	public bool keepSceneTextLabelsActiveInPlayMode;

	// Token: 0x040000CD RID: 205
	public bool resetTutorialOnLoad;

	// Token: 0x040000CE RID: 206
	public bool noTutorial;

	// Token: 0x040000CF RID: 207
	public bool fallStuckDetectAndPopOut;

	// Token: 0x040000D0 RID: 208
	public bool originalClimbDownDontAlwaysWallSlide;

	// Token: 0x040000D1 RID: 209
	public bool peakViewStartsWithMinimisedMaps;

	// Token: 0x040000D2 RID: 210
	public bool only3JumpUpPrompts;

	// Token: 0x040000D3 RID: 211
	public bool dontAutoContinue;

	// Token: 0x040000D4 RID: 212
	public bool visualiseTexelDensity;

	// Token: 0x040000D5 RID: 213
	public bool reduceCloudUsageTest;

	// Token: 0x040000D6 RID: 214
	public bool forceSpecialJumpsAlwaysAvailable;

	// Token: 0x040000D7 RID: 215
	public bool autoJump;

	// Token: 0x040000D8 RID: 216
	public bool allowDynamicMediumLODs;

	// Token: 0x040000D9 RID: 217
	public bool dynamicWaterQuality;
}
