using System;
using UnityEngine;

// Token: 0x02000083 RID: 131
public class TrackBuilderSettings : ScriptableObject
{
	// Token: 0x04000508 RID: 1288
	public float creationZoneWidth = 25f;

	// Token: 0x04000509 RID: 1289
	public float keepAliveZoneWidth = 30f;

	// Token: 0x0400050A RID: 1290
	public float creationZoneHeight = 25f;

	// Token: 0x0400050B RID: 1291
	public float keepAliveZoneHeight = 30f;

	// Token: 0x0400050C RID: 1292
	public float maxTotalCreationZoneWidth = 150f;

	// Token: 0x0400050D RID: 1293
	public float maxTotalKeepAliveZoneWidth = 200f;

	// Token: 0x0400050E RID: 1294
	public float maxAngleReturnToGradient = 30f;

	// Token: 0x0400050F RID: 1295
	public float startWithCentreIslandMargin = 50f;

	// Token: 0x04000510 RID: 1296
	public float proximityToConnectIslandToTrack = 50f;

	// Token: 0x04000511 RID: 1297
	public float maxChunkWidthRequirement = 50f;

	// Token: 0x04000512 RID: 1298
	public float minimumChunkLife = 4f;

	// Token: 0x04000513 RID: 1299
	public bool allowLargeResyncChunks;

	// Token: 0x04000514 RID: 1300
	public float largeResyncMinError = 0.1f;

	// Token: 0x04000515 RID: 1301
	public bool removeAllMarkersWhenOutOfSync;

	// Token: 0x04000516 RID: 1302
	public float removeAllMarkersErrorThreshold = 0.07f;

	// Token: 0x04000517 RID: 1303
	public float promptOffsetWorldY = -2f;

	// Token: 0x04000518 RID: 1304
	public float promptAlpha = 0.4f;
}
