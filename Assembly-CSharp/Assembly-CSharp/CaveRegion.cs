using System;
using UnityEngine;

// Token: 0x02000173 RID: 371
[RequireComponent(typeof(TriggerZone))]
public class CaveRegion : MonoBehaviour
{
	// Token: 0x170002D8 RID: 728
	// (get) Token: 0x06000C4E RID: 3150 RVA: 0x000626F2 File Offset: 0x000608F2
	public static bool inCave
	{
		get
		{
			return CaveRegion.inCaveNorm > 0.2f;
		}
	}

	// Token: 0x06000C4F RID: 3151 RVA: 0x00062700 File Offset: 0x00060900
	public static void UpdateAll()
	{
		Vector2 position = Runner.instance.position;
		int physicalDepthLayerIdx = Runner.instance.physicalDepthLayerIdx;
		CaveRegion.inCaveNorm = 0f;
		foreach (TriggerZone triggerZone in Level.current.triggerZones.Nearby(position, Range.Centered(0f, 100000f), 0f, null))
		{
			if (triggerZone.InsideTriggerDist(position, (float)physicalDepthLayerIdx, 1f) && triggerZone.GetComponent<CaveRegion>() != null)
			{
				float num = Vector2.Distance(position, triggerZone.transform.position);
				float num2 = 1f - Mathf.InverseLerp(triggerZone.triggerRadius - 8f, triggerZone.triggerRadius, num);
				CaveRegion.inCaveNorm = Mathf.Clamp01(CaveRegion.inCaveNorm + num2);
			}
		}
		if (Game.instance.inTitleScreenAndIntroState)
		{
			if (GameCamera.instance.introCameraState.introInProgress)
			{
				float num3 = GameCamera.instance.transform.position.z - Runner.instance.transform.position.z;
				float num4 = Mathf.InverseLerp(0f, 500f, num3);
				CaveRegion.inCaveNorm *= 1f - num4;
				return;
			}
			CaveRegion.inCaveNorm = 0f;
		}
	}

	// Token: 0x06000C50 RID: 3152 RVA: 0x00062870 File Offset: 0x00060A70
	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
	private static void ResetStatics()
	{
		CaveRegion.inCaveNorm = 0f;
	}

	// Token: 0x04000EBC RID: 3772
	public static float inCaveNorm;
}
