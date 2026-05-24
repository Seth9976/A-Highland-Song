using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000113 RID: 275
public class HealthUISettings : ScriptableObject
{
	// Token: 0x04000B15 RID: 2837
	public Color normalHealthBarColour = Color.red;

	// Token: 0x04000B16 RID: 2838
	public Color weatherFreezeHealthBarColour = Color.blue;

	// Token: 0x04000B17 RID: 2839
	public Color weatherFreezeIconColour = Color.blue;

	// Token: 0x04000B18 RID: 2840
	public Color protectedWeatherIconColor = Color.white.WithAlpha(0.3f);

	// Token: 0x04000B19 RID: 2841
	public float healthBarScale = 40f;

	// Token: 0x04000B1A RID: 2842
	public float healthBarScalePower = 1.2f;

	// Token: 0x04000B1B RID: 2843
	public float flashSpeed = 1f;

	// Token: 0x04000B1C RID: 2844
	public float flashAmplitude = 0.2f;

	// Token: 0x04000B1D RID: 2845
	public List<HealthUISettings.Icon> icons;

	// Token: 0x0200033D RID: 829
	[Serializable]
	public struct Icon
	{
		// Token: 0x04001839 RID: 6201
		public WeatherHealthEffect weatherEffect;

		// Token: 0x0400183A RID: 6202
		public Sprite sprite;
	}
}
