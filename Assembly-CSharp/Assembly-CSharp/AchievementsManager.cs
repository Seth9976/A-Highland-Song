using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Ink.Runtime;
using UnityEngine;

// Token: 0x020000EB RID: 235
public class AchievementsManager : MonoSingleton<AchievementsManager>
{
	// Token: 0x17000202 RID: 514
	// (get) Token: 0x060007B3 RID: 1971 RVA: 0x0004541E File Offset: 0x0004361E
	public IEnumerable achievements
	{
		get
		{
			return this._achievements;
		}
	}

	// Token: 0x060007B4 RID: 1972 RVA: 0x00045426 File Offset: 0x00043626
	private void Awake()
	{
		this._achievements = new List<Achievement>();
	}

	// Token: 0x060007B5 RID: 1973 RVA: 0x00045434 File Offset: 0x00043634
	public void SetupAchievementsFromInk(ListDefinition listDef)
	{
		this._achievements.Clear();
		foreach (InkListItem inkListItem in listDef.items.Keys)
		{
			this._achievements.Add(new Achievement(inkListItem.itemName));
		}
	}

	// Token: 0x060007B6 RID: 1974 RVA: 0x000454A8 File Offset: 0x000436A8
	public Achievement GetAchievement(string achievementID)
	{
		return this._achievements.FirstOrDefault((Achievement x) => x.ID == achievementID);
	}

	// Token: 0x060007B7 RID: 1975 RVA: 0x000454DC File Offset: 0x000436DC
	public bool UnlockRandom()
	{
		Achievement achievement = this._achievements.Where((Achievement x) => !x.unlocked).Random<Achievement>();
		if (achievement == null)
		{
			Debug.Log("Could not find any locked achievements!");
			return false;
		}
		this.UnlockWithInkID(achievement.ID);
		return true;
	}

	// Token: 0x060007B8 RID: 1976 RVA: 0x00045538 File Offset: 0x00043738
	public void UnlockWithInkID(string inkID)
	{
		Achievement achievement = this.GetAchievement(inkID);
		if (achievement == null)
		{
			return;
		}
		if (achievement.unlocked)
		{
			Debug.Log("Tried to unlock previously unlocked achievement " + achievement.ID + ". Nothing wrong with this unless it's not visibly unlocked.");
			return;
		}
		Debug.Log("UNLOCK ACHIEVEMENT! " + achievement.ID);
		achievement.unlocked = true;
		this.UnlockOnCurrentPlatformInternal(achievement);
	}

	// Token: 0x060007B9 RID: 1977 RVA: 0x00045597 File Offset: 0x00043797
	private void UnlockOnCurrentPlatformInternal(Achievement achievement)
	{
		if (MonoSingleton<SteamStatsAndAchievements>.instance != null)
		{
			MonoSingleton<SteamStatsAndAchievements>.instance.UnlockAchievement(achievement.ID);
		}
	}

	// Token: 0x060007BA RID: 1978 RVA: 0x000455B6 File Offset: 0x000437B6
	[ContextMenu("Print all")]
	public void PrintAll()
	{
		DebugX.LogList<Achievement>("All achievements", this._achievements, null, true, true);
	}

	// Token: 0x040009A3 RID: 2467
	[SerializeField]
	[Disable]
	private List<Achievement> _achievements = new List<Achievement>();
}
