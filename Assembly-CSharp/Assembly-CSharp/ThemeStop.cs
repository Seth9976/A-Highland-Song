using System;
using UnityEngine;

// Token: 0x0200004A RID: 74
[Serializable]
public struct ThemeStop
{
	// Token: 0x0600020F RID: 527 RVA: 0x000120F0 File Offset: 0x000102F0
	public static ThemeStop Lerp(ThemeStop a, ThemeStop b, float l)
	{
		return new ThemeStop
		{
			name = "Lerped",
			multiplyColour = Color.Lerp(a.multiplyColour, b.multiplyColour, l),
			brightness = Mathf.Lerp(a.brightness, b.brightness, l),
			sky = SkyColourStop.Lerp(a.sky, b.sky, l),
			cloudsBrightness = Mathf.Lerp(a.cloudsBrightness, b.cloudsBrightness, l),
			cloudsColour = Color.Lerp(a.cloudsColour, b.cloudsColour, l),
			fogNearColour = Color.Lerp(a.fogNearColour, b.fogNearColour, l),
			fogFarColour = Color.Lerp(a.fogFarColour, b.fogFarColour, l),
			fogMainRamp = new Range(Mathf.Lerp(a.fogMainRamp.min, b.fogMainRamp.min, l), Mathf.Lerp(a.fogMainRamp.max, b.fogMainRamp.max, l)),
			starsOpacity = Mathf.Lerp(a.starsOpacity, b.starsOpacity, l),
			sunBacklightSize = Mathf.Lerp(a.sunBacklightSize, b.sunBacklightSize, l),
			moonBacklightSize = Mathf.Lerp(a.moonBacklightSize, b.moonBacklightSize, l)
		};
	}

	// Token: 0x040002FA RID: 762
	public string name;

	// Token: 0x040002FB RID: 763
	public float hour;

	// Token: 0x040002FC RID: 764
	public Color multiplyColour;

	// Token: 0x040002FD RID: 765
	public float brightness;

	// Token: 0x040002FE RID: 766
	public float cloudsBrightness;

	// Token: 0x040002FF RID: 767
	public Color cloudsColour;

	// Token: 0x04000300 RID: 768
	public Color fogNearColour;

	// Token: 0x04000301 RID: 769
	public Color fogFarColour;

	// Token: 0x04000302 RID: 770
	public Range fogMainRamp;

	// Token: 0x04000303 RID: 771
	public float starsOpacity;

	// Token: 0x04000304 RID: 772
	public float sunBacklightSize;

	// Token: 0x04000305 RID: 773
	public float moonBacklightSize;

	// Token: 0x04000306 RID: 774
	public SkyColourStop sky;
}
