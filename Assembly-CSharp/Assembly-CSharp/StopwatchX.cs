using System;
using System.Diagnostics;
using UnityEngine;

// Token: 0x020001F4 RID: 500
public static class StopwatchX
{
	// Token: 0x0600129A RID: 4762 RVA: 0x000855AC File Offset: 0x000837AC
	public static void Start(int skip = 0)
	{
		if (StopwatchX._activeInfo == null)
		{
			StopwatchX._activeInfo = new StopwatchX.TimeInfo();
			StopwatchX._activeInfo.skip = skip;
		}
		StopwatchX._sw = new Stopwatch();
		StopwatchX._sw.Start();
	}

	// Token: 0x0600129B RID: 4763 RVA: 0x000855E0 File Offset: 0x000837E0
	public static float End()
	{
		if (StopwatchX._sw == null)
		{
			Debug.LogError("No Stopwatch started?");
			return 0f;
		}
		StopwatchX._sw.Stop();
		if (StopwatchX._activeInfo.skip > 0)
		{
			StopwatchX._activeInfo.skip--;
		}
		else
		{
			StopwatchX._activeInfo.totalRuns++;
			StopwatchX._activeInfo.totalMillisecs += StopwatchX._sw.Millisecs();
		}
		if (StopwatchX._activeInfo.totalRuns == 0)
		{
			return 0f;
		}
		return StopwatchX._activeInfo.totalMillisecs / (float)StopwatchX._activeInfo.totalRuns;
	}

	// Token: 0x0600129C RID: 4764 RVA: 0x00085684 File Offset: 0x00083884
	public static float Millisecs(this Stopwatch sw)
	{
		return (float)(1000.0 * ((double)sw.ElapsedTicks / (double)Stopwatch.Frequency));
	}

	// Token: 0x0400129C RID: 4764
	private static Stopwatch _sw;

	// Token: 0x0400129D RID: 4765
	private static StopwatchX.TimeInfo _activeInfo;

	// Token: 0x02000409 RID: 1033
	private class TimeInfo
	{
		// Token: 0x04001AE5 RID: 6885
		public float totalMillisecs;

		// Token: 0x04001AE6 RID: 6886
		public int totalRuns;

		// Token: 0x04001AE7 RID: 6887
		public int skip;
	}
}
