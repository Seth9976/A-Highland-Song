using System;
using UnityEngine;

// Token: 0x02000036 RID: 54
public class PeakStateSettings : ScriptableObject
{
	// Token: 0x04000273 RID: 627
	public PeakStateSettings.Props props;

	// Token: 0x04000274 RID: 628
	public PeakStateSettings.Camera camera;

	// Token: 0x04000275 RID: 629
	public PeakStateSettings.Vignette vignette;

	// Token: 0x04000276 RID: 630
	public float directionalArrowsScaleWithMap = 0.6f;

	// Token: 0x04000277 RID: 631
	public float directionalArrowsPulseSpeed = 2f;

	// Token: 0x04000278 RID: 632
	public float maxAssumedSpottableDistance = 500f;

	// Token: 0x04000279 RID: 633
	public float maxAssumedSpottableDistanceMinor = 250f;

	// Token: 0x0400027A RID: 634
	public float cloudsFadeStartZoom = 0.3f;

	// Token: 0x0400027B RID: 635
	public float minPeakCloudsAlpha = 0.7f;

	// Token: 0x0200026F RID: 623
	[Serializable]
	public class Props
	{
		// Token: 0x040014AD RID: 5293
		public PeakStateSettings.Props.PropFocusing propFocusing;

		// Token: 0x040014AE RID: 5294
		public PeakStateSettings.Props.Snapping magneticSnapping;

		// Token: 0x02000432 RID: 1074
		[Serializable]
		public class PropFocusing
		{
			// Token: 0x04001B88 RID: 7048
			public float distantFocusModifier = 0.5f;

			// Token: 0x04001B89 RID: 7049
			public float focusRadius = 300f;

			// Token: 0x04001B8A RID: 7050
			public float focusDiscoveryTime = 1f;
		}

		// Token: 0x02000433 RID: 1075
		[Serializable]
		public class Snapping
		{
			// Token: 0x04001B8B RID: 7051
			public float snapDistance = 70f;

			// Token: 0x04001B8C RID: 7052
			public float snapDistanceWhenZoomed = 70f;

			// Token: 0x04001B8D RID: 7053
			public float snappingScale = 4f;

			// Token: 0x04001B8E RID: 7054
			public float snappingScaleWhenZoomed = 4f;

			// Token: 0x04001B8F RID: 7055
			public AnimationCurve smoothingTimeOverDistance;
		}
	}

	// Token: 0x02000270 RID: 624
	[Serializable]
	public class Camera
	{
		// Token: 0x040014AF RID: 5295
		public Range playerScreenDistRange = new Range(200f, 1200f);

		// Token: 0x040014B0 RID: 5296
		[Space]
		public float zoomTransitionDuration = 1f;

		// Token: 0x040014B1 RID: 5297
		public float zoomSpeed = 1f;

		// Token: 0x040014B2 RID: 5298
		public float zoomNormToHideCurrentLevel = 0.7f;

		// Token: 0x040014B3 RID: 5299
		public float cameraMaxSlowdownAtSwitchPoint = 0.05f;

		// Token: 0x040014B4 RID: 5300
		[Space]
		public float fieldOfView = 18f;

		// Token: 0x040014B5 RID: 5301
		public float fieldOfViewFirstMiniPeak = 30f;

		// Token: 0x040014B6 RID: 5302
		[Space]
		public float reticuleViewportOffsetScale = 2.166667f;
	}

	// Token: 0x02000271 RID: 625
	[Serializable]
	public class Vignette
	{
		// Token: 0x040014B7 RID: 5303
		public float inactiveScale = 1.5f;
	}
}
