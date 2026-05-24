using System;
using UnityEngine;
using UnityEngine.Serialization;

// Token: 0x0200011D RID: 285
public class JournalSettings : ScriptableObject
{
	// Token: 0x04000B92 RID: 2962
	public float maxOpenCloseHingeAngle = 60f;

	// Token: 0x04000B93 RID: 2963
	public float openCloseMinScale = 0.7f;

	// Token: 0x04000B94 RID: 2964
	public float closeOffsetY = -200f;

	// Token: 0x04000B95 RID: 2965
	public float pageTopMargin = 80f;

	// Token: 0x04000B96 RID: 2966
	public float pageBottomMargin = 80f;

	// Token: 0x04000B97 RID: 2967
	public float pageLeftMargin = 30f;

	// Token: 0x04000B98 RID: 2968
	public float pageRightMargin = 50f;

	// Token: 0x04000B99 RID: 2969
	public float titleYOffset = -10f;

	// Token: 0x04000B9A RID: 2970
	public float titleHeight = 150f;

	// Token: 0x04000B9B RID: 2971
	public float discoveryBetweenPadding = 10f;

	// Token: 0x04000B9C RID: 2972
	public float peaksStartYFirstPage = 150f;

	// Token: 0x04000B9D RID: 2973
	public float peaksStartYSubsequentPages = 100f;

	// Token: 0x04000B9E RID: 2974
	public float peaksItemHeight = 150f;

	// Token: 0x04000B9F RID: 2975
	public float inventoryItemHeight = 140f;

	// Token: 0x04000BA0 RID: 2976
	public float mapsFirstPageY = 150f;

	// Token: 0x04000BA1 RID: 2977
	public float mapsY = 150f;

	// Token: 0x04000BA2 RID: 2978
	public float systemOptionsStartY = 150f;

	// Token: 0x04000BA3 RID: 2979
	public float systemOptionHeight = 100f;

	// Token: 0x04000BA4 RID: 2980
	public float systemOptionsStartYSwitch = 150f;

	// Token: 0x04000BA5 RID: 2981
	public float systemOptionHeightSwitch = 100f;

	// Token: 0x04000BA6 RID: 2982
	public Color systemItemHighlightColor = Color.red;

	// Token: 0x04000BA7 RID: 2983
	public Material systemItemHighlightedDefaultFontMaterial;

	// Token: 0x04000BA8 RID: 2984
	public Material systemItemHighlightedFontMaterial;

	// Token: 0x04000BA9 RID: 2985
	public float logoY = 200f;

	// Token: 0x04000BAA RID: 2986
	public float lastSavedY = 400f;

	// Token: 0x04000BAB RID: 2987
	public Sprite[] discoveriesSprites;

	// Token: 0x04000BAC RID: 2988
	[FormerlySerializedAs("testInventoryIcon")]
	public Sprite fallbackInventoryIcon;

	// Token: 0x04000BAD RID: 2989
	public PeakIconDatabase peakIconDatabase;

	// Token: 0x04000BAE RID: 2990
	public float slideOverDistFromEdge = 80f;

	// Token: 0x04000BAF RID: 2991
	public float mapZoomLeftX = -200f;

	// Token: 0x04000BB0 RID: 2992
	public float mapZoomLeftFirstY = -200f;

	// Token: 0x04000BB1 RID: 2993
	public float mapZoomRightX = -200f;

	// Token: 0x04000BB2 RID: 2994
	public float mapZoomStartY = -200f;

	// Token: 0x04000BB3 RID: 2995
	public float mapZoomY = -200f;

	// Token: 0x04000BB4 RID: 2996
	public float mapZoomOffsetX = 170f;
}
