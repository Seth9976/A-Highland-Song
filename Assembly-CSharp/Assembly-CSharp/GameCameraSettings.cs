using System;
using UnityEngine;

// Token: 0x02000022 RID: 34
public class GameCameraSettings : ScriptableObject
{
	// Token: 0x0400012C RID: 300
	public float zoomToDistance = 8f;

	// Token: 0x0400012D RID: 301
	public float zoomToDistanceLate = 24f;

	// Token: 0x0400012E RID: 302
	public float cameraSmoothTime = 0.2f;

	// Token: 0x0400012F RID: 303
	public float fallingSmoothTime = 0.2f;

	// Token: 0x04000130 RID: 304
	public float cameraOffsetMaxSpeed = 100f;

	// Token: 0x04000131 RID: 305
	public float cameraOffsetYScalar = 0.6f;

	// Token: 0x04000132 RID: 306
	public float cameraLeadScalar = 1f;

	// Token: 0x04000133 RID: 307
	public float cameraLeadScalarSliding = 1f;

	// Token: 0x04000134 RID: 308
	public float cameraLeadScalarMusicRun;

	// Token: 0x04000135 RID: 309
	public float cameraMaxLead = 30f;

	// Token: 0x04000136 RID: 310
	public float cameraMaxLeadMusicRun = 30f;

	// Token: 0x04000137 RID: 311
	public float cameraMaxLeadSliding = 30f;

	// Token: 0x04000138 RID: 312
	public Range goingOffscreenRange = new Range(0.7f, 0.9f);

	// Token: 0x04000139 RID: 313
	public float outOfRangeClampPow = 2f;

	// Token: 0x0400013A RID: 314
	public float outOfRangeClampScalar = 2f;

	// Token: 0x0400013B RID: 315
	public float leadSmoothTime = 0.2f;

	// Token: 0x0400013C RID: 316
	public float leadSmoothTimeStopped = 0.8f;

	// Token: 0x0400013D RID: 317
	public float cameraRunZoomMax = 2f;

	// Token: 0x0400013E RID: 318
	public float cameraMusicRunAreaWideZoom = 10f;

	// Token: 0x0400013F RID: 319
	public float cameraMusicRunReadyToSprint = 1f;

	// Token: 0x04000140 RID: 320
	public float cameraMusicRunReadyToSprintSmoothTime = 0.1f;

	// Token: 0x04000141 RID: 321
	public float cameraMusicRunZoom1 = 1f;

	// Token: 0x04000142 RID: 322
	public float cameraMusicRunZoom2 = 2.5f;

	// Token: 0x04000143 RID: 323
	public float cameraMusicRunZoom3 = 1.5f;

	// Token: 0x04000144 RID: 324
	public float cameraMusicRunInitialBigZoomDuration = 2f;

	// Token: 0x04000145 RID: 325
	public float cameraMusicRunInitialLeadInTime = 0.5f;

	// Token: 0x04000146 RID: 326
	public float cameraRunZoomTimeToFullZoom = 10f;

	// Token: 0x04000147 RID: 327
	public float cameraSlideZoomMax = 3f;

	// Token: 0x04000148 RID: 328
	public float cameraZoomOutSmoothTime = 2f;

	// Token: 0x04000149 RID: 329
	public float cameraZoomInSmoothTime = 2f;

	// Token: 0x0400014A RID: 330
	public float cameraMusicRunSnappySmoothTime = 0.1f;

	// Token: 0x0400014B RID: 331
	public Range maxRelativeZoomCountSpeed = new Range(1f, 10f);

	// Token: 0x0400014C RID: 332
	public float initialMusicRunLeadOffetScalar = 3f;

	// Token: 0x0400014D RID: 333
	[Header("Zoom")]
	public float limitedZoomDistance = 20f;

	// Token: 0x0400014E RID: 334
	public float caveZoomDistance = 10f;

	// Token: 0x0400014F RID: 335
	public float ridgeZoomDistance = 40f;

	// Token: 0x04000150 RID: 336
	public float lookFurtherMultiplier = 2f;

	// Token: 0x04000151 RID: 337
	public float zoomInDistance = 5f;

	// Token: 0x04000152 RID: 338
	public Range zoomInSourceDistanceRange = new Range(10f, 100f);

	// Token: 0x04000153 RID: 339
	public Range zoomInTargetDistanceRange = new Range(5f, 20f);

	// Token: 0x04000154 RID: 340
	public float minPlayerZoom = 1.5f;

	// Token: 0x04000155 RID: 341
	public float playerZoomSmoothTime = 0.5f;

	// Token: 0x04000156 RID: 342
	public float restingZoom = 4f;

	// Token: 0x04000157 RID: 343
	public float restingZoomSmoothTime;

	// Token: 0x04000158 RID: 344
	public float bellyWriggleZoom = 0.3f;

	// Token: 0x04000159 RID: 345
	public float bellyWriggleStuckZoom = 0.2f;

	// Token: 0x0400015A RID: 346
	public float playerPanSpeed = 1f;

	// Token: 0x0400015B RID: 347
	public float playerPanSmoothTime = 0.2f;

	// Token: 0x0400015C RID: 348
	public float playerPanLimit = 1f;

	// Token: 0x0400015D RID: 349
	public float deadZoomOutPerSecond = 2f;

	// Token: 0x0400015E RID: 350
	[Header("Falling")]
	public float cameraLeadScalarFalling = 1f;

	// Token: 0x0400015F RID: 351
	public float cameraMaxLeadFalling = 10f;

	// Token: 0x04000160 RID: 352
	public float fallingZoomMax = 0.2f;

	// Token: 0x04000161 RID: 353
	public float fallingZoomSmoothTime = 0.5f;

	// Token: 0x04000162 RID: 354
	[Header("Peak")]
	public Range peakElevationRange = new Range(100f, 6000f);

	// Token: 0x04000163 RID: 355
	public Range peakZOffset = new Range(20f, 100f);

	// Token: 0x04000164 RID: 356
	public Range peakZOffsetMinor = new Range(20f, 100f);

	// Token: 0x04000165 RID: 357
	public Range peakFOV = new Range(100f, 120f);

	// Token: 0x04000166 RID: 358
	public float peakMinimumCameraDistanceForSpeed = 200f;

	// Token: 0x04000167 RID: 359
	public Vector2 peakPlayerPanSpeed = new Vector2(1.25f, 1.25f);

	// Token: 0x04000168 RID: 360
	public float peakViewZoomedSpeedScalar = 2f;

	// Token: 0x04000169 RID: 361
	public Vector2 peakViewExtent = new Vector2(3000f, 1200f);

	// Token: 0x0400016A RID: 362
	public Vector2 minorPeakViewExtent = new Vector2(800f, 1000f);

	// Token: 0x0400016B RID: 363
	public Range peakWorldAbsoluteMaxClampX = new Range(-5000f, 5000f);

	// Token: 0x0400016C RID: 364
	public float peakViewPanSmoothTime = 0.1f;

	// Token: 0x0400016D RID: 365
	public Range lookUpDownRange = new Range(-200f, 1000f);

	// Token: 0x0400016E RID: 366
	public Vector2 viewportMoveWhenLookingUp = new Vector2(0.5f, 1.6f);

	// Token: 0x0400016F RID: 367
	public Vector2 translateMoveWhenLookingUp = new Vector2(0.4f, 0f);

	// Token: 0x04000170 RID: 368
	public Vector2 viewportMoveWhenLookingDown = new Vector2(0.5f, 0.4f);

	// Token: 0x04000171 RID: 369
	public Vector2 translateMoveWhenLookingDown = new Vector2(0.4f, 0.6f);

	// Token: 0x04000172 RID: 370
	public Range peakViewOffset = new Range(50f, -200f);

	// Token: 0x04000173 RID: 371
	public float peakShear = 0.5f;

	// Token: 0x04000174 RID: 372
	public float peakAutomaticSmoothTime = 0.2f;

	// Token: 0x04000175 RID: 373
	public float peakAutomaticMaxSpeed = 1000f;

	// Token: 0x04000176 RID: 374
	public float peakAutomaticArrivalDist = 0.05f;

	// Token: 0x04000177 RID: 375
	public float peakAutomaticArrivalMaxSpeed = 1f;

	// Token: 0x04000178 RID: 376
	[Header("Shelter")]
	public float shelterZoom = 10f;

	// Token: 0x04000179 RID: 377
	public float shelterYOffset = 100f;

	// Token: 0x0400017A RID: 378
	[Header("Caught")]
	public float caughtZoom = 10f;

	// Token: 0x0400017B RID: 379
	public float caughtZoomSmoothTime = 10f;

	// Token: 0x0400017C RID: 380
	public float caughtLerp = 0.5f;

	// Token: 0x0400017D RID: 381
	public float caughtStartLerp = 0.9f;

	// Token: 0x0400017E RID: 382
	public Range caughtStartTimeRange = new Range(2f, 4f);

	// Token: 0x0400017F RID: 383
	[Space]
	public float minDistanceForZoomModePeakView = 40f;

	// Token: 0x04000180 RID: 384
	[Space]
	[Header("Stone Skimming")]
	public GameCameraSettings.StoneSkimmingSettings stoneSkimmingSettings;

	// Token: 0x04000181 RID: 385
	[Space]
	[Header("Path Trigger")]
	public GameCameraSettings.PathTriggerSettings pathTriggerSettings;

	// Token: 0x0200025D RID: 605
	[Serializable]
	public class StoneSkimmingSettings
	{
		// Token: 0x04001431 RID: 5169
		public float stateEnterSpeed = 1f;

		// Token: 0x04001432 RID: 5170
		public float stateExitSpeed = 2f;

		// Token: 0x04001433 RID: 5171
		[Space]
		public float zoom = 0.75f;

		// Token: 0x04001434 RID: 5172
		public Vector2 viewportOffset;

		// Token: 0x04001435 RID: 5173
		[Space]
		[Header("Skimming State Settings")]
		public float modeSwitchSmoothTime = 0.1f;

		// Token: 0x04001436 RID: 5174
		[Space]
		[Header("Idle/Winding")]
		public float triggerZoneFramingFrontMargin = 40f;

		// Token: 0x04001437 RID: 5175
		public float triggerZoneFramingBackMargin = 10f;

		// Token: 0x04001438 RID: 5176
		public AnimationCurve extraDistanceOverTimeHeld;

		// Token: 0x04001439 RID: 5177
		[Space]
		[Header("Stone Follow")]
		public float stoneFrontMargin = 10f;

		// Token: 0x0400143A RID: 5178
		public float stoneBackMargin = 10f;

		// Token: 0x0400143B RID: 5179
		public AnimationCurve thrownStateStrengthOverStoneDistance;
	}

	// Token: 0x0200025E RID: 606
	[Serializable]
	public class PathTriggerSettings
	{
		// Token: 0x0400143C RID: 5180
		public float strengthSmoothTime = 1f;

		// Token: 0x0400143D RID: 5181
		[Space]
		public float distance = 0.75f;

		// Token: 0x0400143E RID: 5182
		public AnimationCurve shearFactorOverDegreesToTarget;
	}
}
