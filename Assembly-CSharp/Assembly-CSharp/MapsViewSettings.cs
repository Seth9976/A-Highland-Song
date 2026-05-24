using System;
using UnityEngine;

// Token: 0x02000126 RID: 294
public class MapsViewSettings : ScriptableObject
{
	// Token: 0x04000C06 RID: 3078
	public Vector2 mapAreaSize = new Vector2(800f, 800f);

	// Token: 0x04000C07 RID: 3079
	public Vector2 mapAreaOffsetFromTopRight;

	// Token: 0x04000C08 RID: 3080
	public Vector2 minimisedMapOffsetFromTopRight;

	// Token: 0x04000C09 RID: 3081
	public float minimisedMapAngle;

	// Token: 0x04000C0A RID: 3082
	public float fanAnglePerMap = 5f;

	// Token: 0x04000C0B RID: 3083
	public float fanScalePerMap = 0.9f;

	// Token: 0x04000C0C RID: 3084
	public Vector2 leftFanOffsetStart = new Vector2(-200f, -100f);

	// Token: 0x04000C0D RID: 3085
	public Vector2 leftFanOffsetEnd = new Vector2(-300f, -150f);

	// Token: 0x04000C0E RID: 3086
	public Vector2 rightFanOffsetStart = new Vector2(100f, 50f);

	// Token: 0x04000C0F RID: 3087
	public Vector2 rightFanOffsetEnd = new Vector2(150f, 80f);

	// Token: 0x04000C10 RID: 3088
	public Color colourTintStart = Color.white;

	// Token: 0x04000C11 RID: 3089
	public Color colourTintEnd = Color.white;

	// Token: 0x04000C12 RID: 3090
	public Vector2 tuckOffset = new Vector2(0f, 300f);

	// Token: 0x04000C13 RID: 3091
	public AnimationCurve tuckCurve = AnimationCurve.Linear(0f, 0f, 1f, 0f);

	// Token: 0x04000C14 RID: 3092
	public float mapLayeringZIndexOffset = -0.4f;

	// Token: 0x04000C15 RID: 3093
	public float hiddenScale = 0.3f;

	// Token: 0x04000C16 RID: 3094
	public float hiddenMapAngle = 50f;

	// Token: 0x04000C17 RID: 3095
	public float deselectedScalar = 0.3f;

	// Token: 0x04000C18 RID: 3096
	public Vector2 deselectedOffset = new Vector2(200f, -200f);

	// Token: 0x04000C19 RID: 3097
	public float deselectedRotation = 10f;

	// Token: 0x04000C1A RID: 3098
	public float smoothTime = 0.5f;

	// Token: 0x04000C1B RID: 3099
	public float maxSpeed = 10000f;

	// Token: 0x04000C1C RID: 3100
	public float onOffScreenDuration = 0.3f;

	// Token: 0x04000C1D RID: 3101
	public float offscreenOffset = -800f;

	// Token: 0x04000C1E RID: 3102
	public Vector2 confirmMapPosFromTopRight = new Vector2(-200f, -300f);

	// Token: 0x04000C1F RID: 3103
	public float getMapStartScale = 0.1f;

	// Token: 0x04000C20 RID: 3104
	public float getMapStartOffsetY = -300f;

	// Token: 0x04000C21 RID: 3105
	public float getMapAppearDuration = 2f;

	// Token: 0x04000C22 RID: 3106
	public float getMapPauseDuration = 3f;

	// Token: 0x04000C23 RID: 3107
	public AnimationCurve getMapAppearCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);

	// Token: 0x04000C24 RID: 3108
	public float getMapBGDarkness = 0.5f;

	// Token: 0x04000C25 RID: 3109
	public float markerSelectedBobSpeed = 4f;

	// Token: 0x04000C26 RID: 3110
	public float markerSelectedBobScale = 1.1f;

	// Token: 0x04000C27 RID: 3111
	public float markerProjectionMaxRelativeRadius = 0.8f;

	// Token: 0x04000C28 RID: 3112
	public float markerProjectionZWeight = 0.1f;

	// Token: 0x04000C29 RID: 3113
	public float glowPulseAlphaAmount = 0.1f;

	// Token: 0x04000C2A RID: 3114
	public float glowPulseScaleAmount = 0.1f;

	// Token: 0x04000C2B RID: 3115
	public float glowPulseSpeed = 3f;

	// Token: 0x04000C2C RID: 3116
	public float glowBaseAlpha = 0.8f;

	// Token: 0x04000C2D RID: 3117
	public float minimumSquareAspectScale = 0.7f;

	// Token: 0x04000C2E RID: 3118
	public float visiblePromptsContainerWidth = 800f;

	// Token: 0x04000C2F RID: 3119
	public float visiblePromptsOffsetFromRight;

	// Token: 0x04000C30 RID: 3120
	public float hiddenPromptsContainerWidth = 250f;

	// Token: 0x04000C31 RID: 3121
	public float hiddenPromptsOffsetFromRight;

	// Token: 0x04000C32 RID: 3122
	public Range previewMarkerAppearZoomRange = new Range(0f, 0.5f);

	// Token: 0x04000C33 RID: 3123
	public float prevNextPulseSpeed = 1f;

	// Token: 0x04000C34 RID: 3124
	public Range prevNextPulseScale = new Range(0.9f, 1.1f);
}
