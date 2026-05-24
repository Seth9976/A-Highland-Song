using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

// Token: 0x020000E7 RID: 231
[Serializable]
public class SaveState
{
	// Token: 0x060007AA RID: 1962 RVA: 0x00045293 File Offset: 0x00043493
	public static SaveState Create()
	{
		return new SaveState
		{
			saveMetaInfo = new SaveMetaInformation(),
			saveMetaInfo = 
			{
				saveVersion = new SaveVersion(1),
				saveDateTime = DateTime.Now
			}
		};
	}

	// Token: 0x060007AB RID: 1963 RVA: 0x000452C8 File Offset: 0x000434C8
	private SaveState()
	{
	}

	// Token: 0x04000979 RID: 2425
	public SaveMetaInformation saveMetaInfo;

	// Token: 0x0400097A RID: 2426
	[Space]
	public float playTime;

	// Token: 0x0400097B RID: 2427
	public float lastBackupTime;

	// Token: 0x0400097C RID: 2428
	[Space]
	public int playthroughIdx;

	// Token: 0x0400097D RID: 2429
	[Space]
	public Vector3 runnerPosition;

	// Token: 0x0400097E RID: 2430
	public int levelIdx = -1;

	// Token: 0x0400097F RID: 2431
	public float runnerHealth;

	// Token: 0x04000980 RID: 2432
	public float runnerMaxHealth;

	// Token: 0x04000981 RID: 2433
	public float runnerMaxStamina;

	// Token: 0x04000982 RID: 2434
	public Range runnerXRange;

	// Token: 0x04000983 RID: 2435
	public int lastMusicRunHealthBoostDayIdx = -1;

	// Token: 0x04000984 RID: 2436
	public Damage lastWeatherHealthImpact;

	// Token: 0x04000985 RID: 2437
	public float weatherRecoverTimeRemaining;

	// Token: 0x04000986 RID: 2438
	public int lastNightfallRemarkDayIdx = -1;

	// Token: 0x04000987 RID: 2439
	public string activeAutoCutZoneName;

	// Token: 0x04000988 RID: 2440
	public float hourOfDay = 12f;

	// Token: 0x04000989 RID: 2441
	public int dayIdx;

	// Token: 0x0400098A RID: 2442
	public List<string> inventoryMapNames = new List<string>(128);

	// Token: 0x0400098B RID: 2443
	public int selectedMapIdx;

	// Token: 0x0400098C RID: 2444
	public bool firstDayPauseEnabled;

	// Token: 0x0400098D RID: 2445
	public SaveState.WeatherSystemSaveState weatherSystemSaveState = new SaveState.WeatherSystemSaveState();

	// Token: 0x0400098E RID: 2446
	public List<string> passedProps = new List<string>();

	// Token: 0x0400098F RID: 2447
	public List<string> disabledProps = new List<string>();

	// Token: 0x04000990 RID: 2448
	public List<string> enabledProps = new List<string>();

	// Token: 0x04000991 RID: 2449
	public List<string> completedAutoRunZones = new List<string>();

	// Token: 0x04000992 RID: 2450
	public List<string> visitedPeaks = new List<string>();

	// Token: 0x04000993 RID: 2451
	public List<string> foundPaths = new List<string>();

	// Token: 0x04000994 RID: 2452
	public List<SaveState.OrderOfAquisition> orderOfAquisitionIdx = new List<SaveState.OrderOfAquisition>();

	// Token: 0x04000995 RID: 2453
	public int nextAquisitionIdx;

	// Token: 0x04000996 RID: 2454
	public List<SaveState.MapMarkerPos> mapMarkerPositions = new List<SaveState.MapMarkerPos>();

	// Token: 0x04000997 RID: 2455
	public List<SaveState.CreatureState> activeCreatures = new List<SaveState.CreatureState>();

	// Token: 0x04000998 RID: 2456
	public List<SaveState.AnimatorState> animatorStates = new List<SaveState.AnimatorState>();

	// Token: 0x04000999 RID: 2457
	public bool castleCrowsFlown;

	// Token: 0x0400099A RID: 2458
	[FormerlySerializedAs("injuredCrowStates")]
	public List<SaveState.InkWalkerState> walkerStates;

	// Token: 0x0400099B RID: 2459
	public bool chairLiftRunning;

	// Token: 0x0400099C RID: 2460
	public List<string> goshawksFlying = new List<string>();

	// Token: 0x0400099D RID: 2461
	[Space]
	public string storyJSON;

	// Token: 0x0200030F RID: 783
	[Serializable]
	public struct CloudGroupSaveState
	{
		// Token: 0x040017A3 RID: 6051
		public string inkName;

		// Token: 0x040017A4 RID: 6052
		public WeatherOverride weatherOverride;

		// Token: 0x040017A5 RID: 6053
		public float storminess;
	}

	// Token: 0x02000310 RID: 784
	[Serializable]
	public class WeatherSystemSaveState
	{
		// Token: 0x040017A6 RID: 6054
		public WeatherPattern.State weatherPatternState = new WeatherPattern.State();

		// Token: 0x040017A7 RID: 6055
		public List<WeatherSystem.QueuedWeather> queuedWeather = new List<WeatherSystem.QueuedWeather>();

		// Token: 0x040017A8 RID: 6056
		public string weatherPatternName;

		// Token: 0x040017A9 RID: 6057
		public WeatherOverride weatherInkOverride;

		// Token: 0x040017AA RID: 6058
		public List<SaveState.CloudGroupSaveState> cloudGroupStates = new List<SaveState.CloudGroupSaveState>();
	}

	// Token: 0x02000311 RID: 785
	[Serializable]
	public struct OrderOfAquisition
	{
		// Token: 0x040017AB RID: 6059
		public string inkName;

		// Token: 0x040017AC RID: 6060
		public int idx;
	}

	// Token: 0x02000312 RID: 786
	[Serializable]
	public struct MapMarkerPos
	{
		// Token: 0x040017AD RID: 6061
		public string inkName;

		// Token: 0x040017AE RID: 6062
		public Vector3 pos;
	}

	// Token: 0x02000313 RID: 787
	[Serializable]
	public struct CreatureState
	{
		// Token: 0x170004CE RID: 1230
		// (get) Token: 0x06001680 RID: 5760 RVA: 0x0009892F File Offset: 0x00096B2F
		public bool isValid
		{
			get
			{
				return this.guid != null && this.guid.Length > 0;
			}
		}

		// Token: 0x040017AF RID: 6063
		public string name;

		// Token: 0x040017B0 RID: 6064
		public string guid;

		// Token: 0x040017B1 RID: 6065
		public Vector3 pos;

		// Token: 0x040017B2 RID: 6066
		public float dir;

		// Token: 0x040017B3 RID: 6067
		public string zoneName;

		// Token: 0x040017B4 RID: 6068
		public Creature.State state;

		// Token: 0x040017B5 RID: 6069
		public float walkRunTarget;

		// Token: 0x040017B6 RID: 6070
		public bool hasRunGrazeKnot;

		// Token: 0x040017B7 RID: 6071
		public bool exiting;
	}

	// Token: 0x02000314 RID: 788
	[Serializable]
	public struct AnimatorState
	{
		// Token: 0x040017B8 RID: 6072
		public string animatorName;

		// Token: 0x040017B9 RID: 6073
		public string[] animPlayOrder;
	}

	// Token: 0x02000315 RID: 789
	[Serializable]
	public struct InkWalkerState
	{
		// Token: 0x040017BA RID: 6074
		public Vector3 pos;

		// Token: 0x040017BB RID: 6075
		public int dir;

		// Token: 0x040017BC RID: 6076
		public string guid;
	}
}
