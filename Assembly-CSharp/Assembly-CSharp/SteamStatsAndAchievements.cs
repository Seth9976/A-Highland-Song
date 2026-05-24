using System;
using System.Collections;
using Steamworks;
using UnityEngine;

// Token: 0x020000ED RID: 237
public class SteamStatsAndAchievements : MonoSingleton<SteamStatsAndAchievements>
{
	// Token: 0x060007C5 RID: 1989 RVA: 0x000457C4 File Offset: 0x000439C4
	private void OnEnable()
	{
		if (!SteamManager.Initialized)
		{
			return;
		}
		this.m_GameID = new CGameID(SteamUtils.GetAppID());
		this.m_UserStatsReceived = Callback<UserStatsReceived_t>.Create(new Callback<UserStatsReceived_t>.DispatchDelegate(this.OnUserStatsReceived));
		this.m_UserAchievementStored = Callback<UserAchievementStored_t>.Create(new Callback<UserAchievementStored_t>.DispatchDelegate(this.OnAchievementStored));
	}

	// Token: 0x060007C6 RID: 1990 RVA: 0x00045818 File Offset: 0x00043A18
	private void Update()
	{
		if (!SteamManager.Initialized)
		{
			return;
		}
		if (!this.m_bRequestedStats)
		{
			if (!SteamManager.Initialized)
			{
				this.m_bRequestedStats = true;
				return;
			}
			bool flag = SteamUserStats.RequestCurrentStats();
			this.m_bRequestedStats = flag;
		}
		bool bStatsValid = this.m_bStatsValid;
	}

	// Token: 0x060007C7 RID: 1991 RVA: 0x00045858 File Offset: 0x00043A58
	private void OnUserStatsReceived(UserStatsReceived_t pCallback)
	{
		if (!SteamManager.Initialized)
		{
			return;
		}
		if ((ulong)this.m_GameID == pCallback.m_nGameID)
		{
			if (EResult.k_EResultOK == pCallback.m_eResult)
			{
				Debug.Log("Received stats and achievements from Steam\n");
				this.m_bStatsValid = true;
				using (IEnumerator enumerator = MonoSingleton<AchievementsManager>.instance.achievements.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						object obj = enumerator.Current;
						Achievement achievement = (Achievement)obj;
						if (!SteamUserStats.GetAchievement(achievement.ID, out achievement.unlocked))
						{
							Debug.LogWarning("SteamUserStats.GetAchievement failed for Achievement " + achievement.ID + "\nIs it registered in the Steam Partner site?");
						}
					}
					return;
				}
			}
			Debug.Log("RequestStats - failed, " + pCallback.m_eResult.ToString());
		}
	}

	// Token: 0x060007C8 RID: 1992 RVA: 0x00045934 File Offset: 0x00043B34
	public void UnlockAchievement(string achievementID)
	{
		if (!SteamManager.Initialized)
		{
			return;
		}
		SteamUserStats.SetAchievement(achievementID);
		SteamUserStats.StoreStats();
	}

	// Token: 0x060007C9 RID: 1993 RVA: 0x0004594C File Offset: 0x00043B4C
	private void OnAchievementStored(UserAchievementStored_t pCallback)
	{
		if ((ulong)this.m_GameID == pCallback.m_nGameID)
		{
			if (pCallback.m_nMaxProgress == 0U)
			{
				Debug.Log("Achievement '" + pCallback.m_rgchAchievementName + "' unlocked!");
				return;
			}
			Debug.Log(string.Concat(new string[]
			{
				"Achievement '",
				pCallback.m_rgchAchievementName,
				"' progress callback, (",
				pCallback.m_nCurProgress.ToString(),
				",",
				pCallback.m_nMaxProgress.ToString(),
				")"
			}));
		}
	}

	// Token: 0x040009A8 RID: 2472
	private CGameID m_GameID;

	// Token: 0x040009A9 RID: 2473
	private bool m_bRequestedStats;

	// Token: 0x040009AA RID: 2474
	private bool m_bStatsValid;

	// Token: 0x040009AB RID: 2475
	protected Callback<UserStatsReceived_t> m_UserStatsReceived;

	// Token: 0x040009AC RID: 2476
	protected Callback<UserAchievementStored_t> m_UserAchievementStored;
}
