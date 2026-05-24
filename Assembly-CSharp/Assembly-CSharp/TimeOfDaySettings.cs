using System;
using UnityEngine;

// Token: 0x0200004D RID: 77
public class TimeOfDaySettings : ScriptableObject
{
	// Token: 0x0400033D RID: 829
	public TimeOfDayTheme clear;

	// Token: 0x0400033E RID: 830
	public TimeOfDayTheme cloudy;

	// Token: 0x0400033F RID: 831
	public TimeOfDayTheme raining;

	// Token: 0x04000340 RID: 832
	public TimeOfDayTheme storm;

	// Token: 0x04000341 RID: 833
	public TimeOfDayTheme snow;

	// Token: 0x04000342 RID: 834
	public TimeOfDaySettings.FogStop[] foggyModifierStops;

	// Token: 0x04000343 RID: 835
	public Gradient horizonColour;

	// Token: 0x04000344 RID: 836
	public Gradient horizonColourCloudy;

	// Token: 0x04000345 RID: 837
	public float caveBrightness;

	// Token: 0x04000346 RID: 838
	public Range caveFogRamp = new Range(50f, 300f);

	// Token: 0x04000347 RID: 839
	public float caveLightingRampUpDuration = 5f;

	// Token: 0x04000348 RID: 840
	public float caveLightingRampDownDuration = 1f;

	// Token: 0x04000349 RID: 841
	public float caveLightingEndRampSplitPoint = 0.8f;

	// Token: 0x0400034A RID: 842
	public float cloudscapeMultiplySkyLowMix = 0.2f;

	// Token: 0x0400034B RID: 843
	public float cloudscapeMultiplyBrightened = 1.2f;

	// Token: 0x0400034C RID: 844
	public TimeOfDaySettings.StormCloudStop[] stormCloudMultiplyColouring;

	// Token: 0x0400034D RID: 845
	public static Color defaultFarFogColour = new Color(0.45f, 0.75f, 0.94f, 0.6f);

	// Token: 0x0200027B RID: 635
	[Serializable]
	public struct FogStop
	{
		// Token: 0x06001570 RID: 5488 RVA: 0x00093BC8 File Offset: 0x00091DC8
		public static TimeOfDaySettings.FogStop Lerp(TimeOfDaySettings.FogStop stop1, TimeOfDaySettings.FogStop stop2, float l)
		{
			return new TimeOfDaySettings.FogStop
			{
				fogNearColour = Color.Lerp(stop1.fogNearColour, stop2.fogNearColour, l),
				fogFarColour = Color.Lerp(stop1.fogFarColour, stop2.fogFarColour, l),
				fogMainRamp = new Range(Mathf.Lerp(stop1.fogMainRamp.min, stop2.fogMainRamp.min, l), Mathf.Lerp(stop1.fogMainRamp.max, stop2.fogMainRamp.max, l))
			};
		}

		// Token: 0x040014DC RID: 5340
		public float hour;

		// Token: 0x040014DD RID: 5341
		public Color fogNearColour;

		// Token: 0x040014DE RID: 5342
		public Color fogFarColour;

		// Token: 0x040014DF RID: 5343
		public Range fogMainRamp;
	}

	// Token: 0x0200027C RID: 636
	[Serializable]
	public struct StormCloudStop
	{
		// Token: 0x06001571 RID: 5489 RVA: 0x00093C54 File Offset: 0x00091E54
		public static TimeOfDaySettings.StormCloudStop Lerp(TimeOfDaySettings.StormCloudStop stop1, TimeOfDaySettings.StormCloudStop stop2, float l)
		{
			return new TimeOfDaySettings.StormCloudStop
			{
				colour = Color.Lerp(stop1.colour, stop2.colour, l)
			};
		}

		// Token: 0x040014E0 RID: 5344
		public string name;

		// Token: 0x040014E1 RID: 5345
		public float hour;

		// Token: 0x040014E2 RID: 5346
		public Color colour;
	}
}
