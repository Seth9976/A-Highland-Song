using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000069 RID: 105
[CreateAssetMenu]
public class Map : ScriptableObject
{
	// Token: 0x060002F3 RID: 755 RVA: 0x0001848C File Offset: 0x0001668C
	public static void LoadAll()
	{
		Map.all = Resources.LoadAll<Map>("Maps");
		foreach (Map map in Map.all)
		{
			if (!string.IsNullOrEmpty(map.targetInkPropName))
			{
				Map.allByPropName[map.targetInkPropName] = map;
			}
		}
	}

	// Token: 0x04000423 RID: 1059
	public static Map[] all;

	// Token: 0x04000424 RID: 1060
	public static Dictionary<string, Map> allByPropName = new Dictionary<string, Map>();

	// Token: 0x04000425 RID: 1061
	public Sprite sprite;

	// Token: 0x04000426 RID: 1062
	public const float defaultTriggerTradius = 15f;

	// Token: 0x04000427 RID: 1063
	public float triggerRadius = 15f;

	// Token: 0x04000428 RID: 1064
	public const float defaultNearbyRadius = 40f;

	// Token: 0x04000429 RID: 1065
	public float nearbyRadius = 40f;

	// Token: 0x0400042A RID: 1066
	public float nearbyZ = 8f;

	// Token: 0x0400042B RID: 1067
	public Vector2 targetPosOnMap = new Vector2(0.5f, 0.5f);

	// Token: 0x0400042C RID: 1068
	public string targetInkPropName;

	// Token: 0x0400042D RID: 1069
	public string secondaryTargetInkPropName;

	// Token: 0x0400042E RID: 1070
	public bool targetIsPeak;

	// Token: 0x0400042F RID: 1071
	public bool unlockedAtGameStart;

	// Token: 0x04000430 RID: 1072
	public bool highlightLookPromptWithoutSpotting;

	// Token: 0x04000431 RID: 1073
	public bool isFirstTutorialMap;

	// Token: 0x04000432 RID: 1074
	public bool neverPromptDirectly;
}
