using System;
using UnityEngine;

// Token: 0x0200004E RID: 78
public class TimeOfDayTheme : ScriptableObject
{
	// Token: 0x06000229 RID: 553 RVA: 0x00013713 File Offset: 0x00011913
	public ThemeStop Evaluate(float timeOfDayNorm)
	{
		return TimeOfDayEffects.EvaluateStops<ThemeStop>(this.stops, (ThemeStop stop) => stop.hour, new Func<ThemeStop, ThemeStop, float, ThemeStop>(ThemeStop.Lerp), timeOfDayNorm);
	}

	// Token: 0x0400034E RID: 846
	public ThemeStop[] stops;
}
