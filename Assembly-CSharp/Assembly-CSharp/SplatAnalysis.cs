using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x020001AF RID: 431
public static class SplatAnalysis
{
	// Token: 0x06000E5D RID: 3677 RVA: 0x00071BF8 File Offset: 0x0006FDF8
	public static void FindUnflattenedLevelSplats()
	{
		foreach (Level level in WorldManager.instance.currentWorld.levels)
		{
			SplatAnalysis._splatsInAnyFlattenSplatsCache.Clear();
			foreach (LevelSection levelSection in level.loadedLevelSections)
			{
				FlattenSplats[] componentsInChildren = levelSection.GetComponentsInChildren<FlattenSplats>();
				for (int i = 0; i < componentsInChildren.Length; i++)
				{
					foreach (Splat splat in componentsInChildren[i].splats)
					{
						SplatAnalysis._splatsInAnyFlattenSplatsCache.Add(splat);
					}
				}
			}
			foreach (LevelSection levelSection2 in level.loadedLevelSections)
			{
				if (levelSection2.unflattenedSplats == null)
				{
					levelSection2.unflattenedSplats = new List<Splat>();
				}
				else
				{
					levelSection2.unflattenedSplats.Clear();
				}
				if (levelSection2.unflattenedSplatsThatJustNeedRefresh == null)
				{
					levelSection2.unflattenedSplatsThatJustNeedRefresh = new List<Splat>();
				}
				else
				{
					levelSection2.unflattenedSplatsThatJustNeedRefresh.Clear();
				}
				GameObject[] rootGameObjects = levelSection2.gameObject.scene.GetRootGameObjects();
				for (int i = 0; i < rootGameObjects.Length; i++)
				{
					rootGameObjects[i].GetComponentsInChildren<Splat>(true, SplatAnalysis._tempSplats);
					SplatAnalysis._tempSplats.RemoveAllOrderedAnd((Splat s) => SplatAnalysis._splatsInAnyFlattenSplatsCache.Contains(s) || !FlattenSplats.SplatShouldBeFlattened(s), null);
					foreach (Splat splat2 in SplatAnalysis._tempSplats)
					{
						FlattenSplats componentInParent = splat2.GetComponentInParent<FlattenSplats>();
						if (componentInParent != null && !componentInParent.isLowLODParent)
						{
							levelSection2.unflattenedSplatsThatJustNeedRefresh.Add(splat2);
						}
						else
						{
							levelSection2.unflattenedSplats.Add(splat2);
						}
					}
					SplatAnalysis._tempSplats.Clear();
				}
			}
		}
		SplatAnalysis._splatsInAnyFlattenSplatsCache.Clear();
	}

	// Token: 0x06000E5E RID: 3678 RVA: 0x00071EB4 File Offset: 0x000700B4
	public static List<Splat> FindGlobalSplats()
	{
		List<Splat> list = new List<Splat>();
		List<GameObject> list2 = new List<GameObject>();
		list2.AddRange(SceneManager.GetSceneByName("Game").GetRootGameObjects());
		list2.AddRange(SceneManager.GetSceneByName("GlobalWorld").GetRootGameObjects());
		foreach (GameObject gameObject in list2)
		{
			foreach (Splat splat in gameObject.transform.GetComponentsInChildren<Splat>())
			{
				if (FlattenSplats.SplatShouldBeFlattened(splat))
				{
					list.Add(splat);
				}
			}
		}
		return list;
	}

	// Token: 0x04001138 RID: 4408
	private static HashSet<Splat> _splatsInAnyFlattenSplatsCache = new HashSet<Splat>(2048);

	// Token: 0x04001139 RID: 4409
	private static List<Splat> _tempSplats = new List<Splat>(2048);
}
