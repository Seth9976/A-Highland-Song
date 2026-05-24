using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200002B RID: 43
[CreateAssetMenu]
public class GameSetup : ScriptableObject
{
	// Token: 0x06000124 RID: 292 RVA: 0x0000C6AB File Offset: 0x0000A8AB
	public void Run(Action onComplete = null)
	{
		Game.instance.StartCoroutine(this.RunCR(onComplete, true));
	}

	// Token: 0x06000125 RID: 293 RVA: 0x0000C6C0 File Offset: 0x0000A8C0
	public IEnumerator RunCR(Action onComplete = null, bool allowSetClockTime = true)
	{
		if (Runner.instance == null)
		{
			Debug.LogError("GameSetup failed to find Runner to position them");
			yield break;
		}
		if (this.setsClockTime && allowSetClockTime)
		{
			GameClock.instance.SetDayAndHourWithoutTimePassing(this.clockDay, this.clockTimeHour);
		}
		yield return Game.instance.TeleportPlayerTo3DCR(this.playerPos, "Game setup", this.lookDirection, false, this.world, false, false, null, null);
		if (onComplete != null)
		{
			onComplete();
		}
		yield break;
	}

	// Token: 0x040001E5 RID: 485
	public static Action<GameSetup> onRunInEditor;

	// Token: 0x040001E6 RID: 486
	public World world;

	// Token: 0x040001E7 RID: 487
	public bool showInPauseMenu = true;

	// Token: 0x040001E8 RID: 488
	public Vector3 playerPos;

	// Token: 0x040001E9 RID: 489
	public int lookDirection;

	// Token: 0x040001EA RID: 490
	public bool setsClockTime;

	// Token: 0x040001EB RID: 491
	public int clockDay;

	// Token: 0x040001EC RID: 492
	public float clockTimeHour = 15f;

	// Token: 0x040001ED RID: 493
	public bool resetsTutorial;
}
