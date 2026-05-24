using System;
using System.Collections.Generic;
using InControl;
using UnityEngine;

// Token: 0x020001B8 RID: 440
public static class ActionSetsManager
{
	// Token: 0x17000371 RID: 881
	// (get) Token: 0x06000E81 RID: 3713 RVA: 0x00072460 File Offset: 0x00070660
	private static IDictionary<ActionSetsManager.ActionSetType, PlayerActionSet> actionSets
	{
		get
		{
			if (ActionSetsManager._actionSets == null && MonoSingleton<GameInput>.instance != null)
			{
				ActionSetsManager._actionSets = new Dictionary<ActionSetsManager.ActionSetType, PlayerActionSet>
				{
					{
						ActionSetsManager.ActionSetType.Menu,
						MonoSingleton<GameInput>.instance.mapping
					},
					{
						ActionSetsManager.ActionSetType.Game,
						MonoSingleton<GameInput>.instance.mapping
					}
				};
			}
			return ActionSetsManager._actionSets;
		}
	}

	// Token: 0x06000E82 RID: 3714 RVA: 0x000724B2 File Offset: 0x000706B2
	public static void ResetActionSets()
	{
		ActionSetsManager._actionSets = null;
	}

	// Token: 0x06000E83 RID: 3715 RVA: 0x000724BC File Offset: 0x000706BC
	public static PlayerAction GetAction(ActionSetsManager.ActionSetType actionSetType, string actionName)
	{
		PlayerActionSet actionSet = ActionSetsManager.GetActionSet(actionSetType);
		if (actionSet == null)
		{
			Debug.LogError("No value for " + actionSetType.ToString());
			return null;
		}
		return actionSet.GetPlayerActionByName(actionName);
	}

	// Token: 0x06000E84 RID: 3716 RVA: 0x000724F8 File Offset: 0x000706F8
	public static PlayerActionSet GetActionSet(ActionSetsManager.ActionSetType actionSetType)
	{
		PlayerActionSet playerActionSet = null;
		if (!ActionSetsManager.actionSets.TryGetValue(actionSetType, out playerActionSet))
		{
			Debug.LogError("No value for " + actionSetType.ToString());
			return null;
		}
		return playerActionSet;
	}

	// Token: 0x04001157 RID: 4439
	private static Dictionary<ActionSetsManager.ActionSetType, PlayerActionSet> _actionSets;

	// Token: 0x020003CC RID: 972
	public enum ActionSetType
	{
		// Token: 0x04001A37 RID: 6711
		Menu,
		// Token: 0x04001A38 RID: 6712
		Game
	}
}
