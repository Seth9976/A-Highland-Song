using System;

// Token: 0x02000056 RID: 86
public class InputVibrationLibrary : MonoSingleton<InputVibration>
{
	// Token: 0x06000263 RID: 611 RVA: 0x00014E30 File Offset: 0x00013030
	public static void ButtonPressVibrate()
	{
		InputVibration.DoTimedVibration(TimedVibration.VibrateForSeconds(0.5f, 0.125f));
	}
}
