using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Token: 0x02000190 RID: 400
[Serializable]
public class Level
{
	// Token: 0x17000307 RID: 775
	// (get) Token: 0x06000D0F RID: 3343 RVA: 0x000688DF File Offset: 0x00066ADF
	// (set) Token: 0x06000D10 RID: 3344 RVA: 0x000688E6 File Offset: 0x00066AE6
	public static Level current { get; private set; }

	// Token: 0x17000308 RID: 776
	// (get) Token: 0x06000D11 RID: 3345 RVA: 0x000688EE File Offset: 0x00066AEE
	public static float currentZ
	{
		get
		{
			return (float)Level.currentIndex * 500f;
		}
	}

	// Token: 0x17000309 RID: 777
	// (get) Token: 0x06000D12 RID: 3346 RVA: 0x000688FC File Offset: 0x00066AFC
	public static List<Level> activeLevels
	{
		get
		{
			return Level._setupLevels;
		}
	}

	// Token: 0x1700030A RID: 778
	// (get) Token: 0x06000D13 RID: 3347 RVA: 0x00068903 File Offset: 0x00066B03
	public bool isSetup
	{
		get
		{
			return this._isSetup;
		}
	}

	// Token: 0x1700030B RID: 779
	// (get) Token: 0x06000D14 RID: 3348 RVA: 0x0006890B File Offset: 0x00066B0B
	// (set) Token: 0x06000D15 RID: 3349 RVA: 0x00068913 File Offset: 0x00066B13
	public float fadeAlpha { get; private set; } = 1f;

	// Token: 0x1700030C RID: 780
	// (get) Token: 0x06000D16 RID: 3350 RVA: 0x0006891C File Offset: 0x00066B1C
	public IEnumerable<Slope> slopesAndMusicRunSlopes
	{
		get
		{
			foreach (Slope slope in this.slopes.all)
			{
				yield return slope;
			}
			List<Slope>.Enumerator enumerator = default(List<Slope>.Enumerator);
			foreach (MusicRun musicRun in this.musicRuns)
			{
				yield return musicRun.slope;
			}
			List<MusicRun>.Enumerator enumerator2 = default(List<MusicRun>.Enumerator);
			yield break;
			yield break;
		}
	}

	// Token: 0x06000D17 RID: 3351 RVA: 0x0006892C File Offset: 0x00066B2C
	public static int DepthToIndex(float positionZ)
	{
		return Mathf.RoundToInt(positionZ / 500f);
	}

	// Token: 0x06000D18 RID: 3352 RVA: 0x0006893A File Offset: 0x00066B3A
	public static float DepthToIndexFloat(float positionZ)
	{
		return positionZ / 500f;
	}

	// Token: 0x06000D19 RID: 3353 RVA: 0x00068943 File Offset: 0x00066B43
	public static float IndexToDepth(int index)
	{
		return (float)index * 500f;
	}

	// Token: 0x06000D1A RID: 3354 RVA: 0x0006894D File Offset: 0x00066B4D
	public static Level GetForTransform(Transform trans)
	{
		LevelSection component = trans.root.GetComponent<LevelSection>();
		if (component == null)
		{
			return null;
		}
		return component.level;
	}

	// Token: 0x06000D1B RID: 3355 RVA: 0x00068968 File Offset: 0x00066B68
	public List<Prop> PropsWithName(string name)
	{
		List<Prop> list;
		if (!this._propsByName.TryGetValue(name, out list))
		{
			return Level._emptyPropList;
		}
		return list;
	}

	// Token: 0x06000D1C RID: 3356 RVA: 0x0006898C File Offset: 0x00066B8C
	public static void ClearAll()
	{
		Level.currentIndex = -1;
		for (int i = Level._setupLevels.Count - 1; i >= 0; i--)
		{
			Level._setupLevels[i].Clear();
		}
		Level._setupLevels.Clear();
		Level.ResetLevelFadeAlpha();
	}

	// Token: 0x06000D1D RID: 3357 RVA: 0x000689D8 File Offset: 0x00066BD8
	public static void ClearUnused(World world, int firstLevelIdx, int lastLevelIdx)
	{
		for (int i = Level._setupLevels.Count - 1; i >= 0; i--)
		{
			Level level = Level._setupLevels[i];
			if (!world.levels.Contains(level) || level.levelIdx < firstLevelIdx || level.levelIdx > lastLevelIdx)
			{
				level.Clear();
			}
		}
	}

	// Token: 0x06000D1E RID: 3358 RVA: 0x00068A38 File Offset: 0x00066C38
	public static void SetCurrentLevelIdx(int idx)
	{
		Level.currentIndex = idx;
		if (Level.currentIndex >= 0 && Level.currentIndex < WorldManager.instance.currentWorld.levels.Count)
		{
			Level.current = WorldManager.instance.currentWorld.levels[Level.currentIndex];
		}
		else if (Level.currentIndex != -1)
		{
			Debug.LogError(string.Format("Level.SetActiveLevelIdx tried to set current index to {0} which is outside of the range of world {1}", Level.currentIndex, WorldManager.instance.currentWorld.name));
		}
		if (Level.current != null)
		{
			Level.current.SetupIfNecessary();
		}
		foreach (Level level in WorldManager.instance.currentWorld.levels)
		{
			if (level.isSetup)
			{
				level.SetCurrent(level == Level.current);
			}
		}
	}

	// Token: 0x06000D1F RID: 3359 RVA: 0x00068B30 File Offset: 0x00066D30
	public static void SetupLevelRange(World world, int firstLevelIdx, int lastLevelIdx)
	{
		for (int i = firstLevelIdx; i <= lastLevelIdx; i++)
		{
			if (i >= 0 && i < world.levels.Count)
			{
				world.levels[i].SetupIfNecessary();
			}
		}
	}

	// Token: 0x06000D20 RID: 3360 RVA: 0x00068B6C File Offset: 0x00066D6C
	public static void SetupLODs(World world, int highLODLevelIdx)
	{
		for (int i = 0; i < world.levels.Count; i++)
		{
			if (world.levels[i].isSetup)
			{
				LODLevel lodlevel;
				if (i == highLODLevelIdx)
				{
					lodlevel = LODLevel.High;
				}
				else if (i >= highLODLevelIdx + 1 && i <= highLODLevelIdx + 2)
				{
					lodlevel = LODLevel.Medium;
				}
				else
				{
					lodlevel = LODLevel.Low;
				}
				world.levels[i].SetFlattenedLODLevel(lodlevel);
				world.levels[i].SetLODForLevel(highLODLevelIdx);
			}
		}
	}

	// Token: 0x06000D21 RID: 3361 RVA: 0x00068BE0 File Offset: 0x00066DE0
	public static void SetAllHighLOD()
	{
		World currentWorld = WorldManager.instance.currentWorld;
		for (int i = 0; i < currentWorld.levels.Count; i++)
		{
			if (currentWorld.levels[i].isSetup)
			{
				currentWorld.levels[i].SetFlattenedLODLevel(LODLevel.High);
				currentWorld.levels[i].SetLODForLevel(i);
			}
		}
	}

	// Token: 0x06000D22 RID: 3362 RVA: 0x00068C48 File Offset: 0x00066E48
	public void SetFlattenedLODLevel(LODLevel lodLevel)
	{
		foreach (FlattenSplats flattenSplats in this.flattenSplats)
		{
			flattenSplats.SetFlattenedLOD(lodLevel);
		}
		if (this.levelIdx == Level.currentIndex)
		{
			RouteVisualiser.SetFlattened(lodLevel != LODLevel.High);
		}
	}

	// Token: 0x06000D23 RID: 3363 RVA: 0x00068CB4 File Offset: 0x00066EB4
	public void SetDynamicMediumAndHighLODLevel()
	{
		Vector3 physicalPosition3d = Runner.instance.physicalPosition3d;
		foreach (FlattenSplats flattenSplats in this.flattenSplats)
		{
			if (flattenSplats.RequireDynamicHighLODForPos(physicalPosition3d))
			{
				flattenSplats.SetFlattenedLOD(LODLevel.High);
			}
			else
			{
				flattenSplats.SetFlattenedLOD(LODLevel.Medium);
			}
		}
	}

	// Token: 0x06000D24 RID: 3364 RVA: 0x00068D24 File Offset: 0x00066F24
	private static void SetGlobalLevelFade(float alpha, float cutoffDepth = -1000f)
	{
		if (!Level._hasSetGlobalShaderIds)
		{
			Level._Global_LevelFadeId = Shader.PropertyToID("_Global_LevelFade");
			Level._Global_LevelFadeCutoffId = Shader.PropertyToID("_Global_LevelFadeCutoff");
			Level._hasSetGlobalShaderIds = true;
		}
		Shader.SetGlobalFloat(Level._Global_LevelFadeId, alpha);
		Shader.SetGlobalFloat(Level._Global_LevelFadeCutoffId, cutoffDepth);
	}

	// Token: 0x06000D25 RID: 3365 RVA: 0x00068D72 File Offset: 0x00066F72
	public static void ResetLevelFadeAlpha()
	{
		Level.SetGlobalLevelFade(1f, -1000f);
	}

	// Token: 0x06000D26 RID: 3366 RVA: 0x00068D84 File Offset: 0x00066F84
	public void SetLevelFadeAlpha(float alpha)
	{
		if (this.fadeAlpha == alpha)
		{
			return;
		}
		this.fadeAlpha = alpha;
		Level.SetGlobalLevelFade(this.fadeAlpha, Level.currentZ + 250f);
		foreach (LitParticles litParticles in this.litParticles)
		{
			litParticles.SetLevelFadeAlpha(alpha);
		}
	}

	// Token: 0x06000D27 RID: 3367 RVA: 0x00068DFC File Offset: 0x00066FFC
	public void SetLODForLevel(int levelIdx)
	{
		foreach (LODItem loditem in this.lodItems)
		{
			bool flag = Level.DepthToIndex(loditem.transform.position.z) - levelIdx <= loditem.levelVisibilityRange;
			loditem.gameObject.SetActive(flag);
		}
		foreach (CloudGroup cloudGroup in this.cloudGroups)
		{
			bool flag2 = Level.DepthToIndex(cloudGroup.transform.position.z) - levelIdx <= cloudGroup.levelVisibilityRange;
			cloudGroup.SetActiveWithFade(flag2);
		}
	}

	// Token: 0x06000D28 RID: 3368 RVA: 0x00068EE4 File Offset: 0x000670E4
	public static void ResetCutawayAndMusicRunSplats(World world, int currentLevelIdx)
	{
		for (int i = 0; i < world.levels.Count; i++)
		{
			if (i != currentLevelIdx)
			{
				Level level = world.levels[i];
				if (level.isSetup)
				{
					foreach (Poly poly in level.cutawayPolys)
					{
						poly.cutawayAlpha = 1f;
					}
					foreach (MusicRun musicRun in level.musicRuns)
					{
						foreach (Splat splat in musicRun.staticFadingSplats)
						{
							splat.musicRunSplatAlpha = 1f;
						}
					}
				}
			}
		}
	}

	// Token: 0x06000D29 RID: 3369 RVA: 0x00068FF4 File Offset: 0x000671F4
	public void SetCurrent(bool isCurrent)
	{
		if (this._isCurrent != isCurrent)
		{
			this._isCurrent = isCurrent;
			if (Application.isPlaying)
			{
				if (isCurrent)
				{
					foreach (Flock flock in this.flocks)
					{
						flock.Begin();
					}
					using (List<Creature>.Enumerator enumerator2 = this.creatures.GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							Creature creature = enumerator2.Current;
							creature.Begin();
						}
						return;
					}
				}
				foreach (Flock flock2 in this.flocks)
				{
					flock2.Clear();
				}
				foreach (Creature creature2 in this.creatures)
				{
					creature2.StopAndHide();
				}
			}
		}
	}

	// Token: 0x06000D2A RID: 3370 RVA: 0x00069124 File Offset: 0x00067324
	public static void SetupAllLoaded()
	{
		foreach (LevelSection levelSection in Resources.FindObjectsOfTypeAll<LevelSection>())
		{
			if (levelSection.level != null)
			{
				levelSection.level.SetupIfNecessary();
			}
		}
	}

	// Token: 0x06000D2B RID: 3371 RVA: 0x0006915C File Offset: 0x0006735C
	public void Clear()
	{
		if (!this._isSetup)
		{
			return;
		}
		this.polys.Clear();
		this.slopes.Clear();
		this.triggerZones.Clear();
		this.props.Clear();
		this.balancePoints.Clear();
		this.invisibleCollision.Clear();
		this.trips.Clear();
		this.weatherModifierZones.Clear();
		this.fadeSeparators.Clear();
		this.cameraVolumes.Clear();
		this.musicRuns.Clear();
		this.flattenSplats.Clear();
		this.lodItems.Clear();
		this.cutawayPolys.Clear();
		this.bellyWriggleEnds.Clear();
		this.introCameraKeyframes.Clear();
		this.cameraPushVolumes.Clear();
		this.peaks.Clear();
		this.cloudGroups.Clear();
		foreach (Flock flock in this.flocks)
		{
			flock.Clear();
		}
		this.flocks.Clear();
		this.creatures.Clear();
		this.characters.Clear();
		this.nonFlockBirds.Clear();
		this.walkers.Clear();
		this.litParticles.Clear();
		this.ropes.Clear();
		this.skiLift = null;
		this._isSetup = false;
		this._isCurrent = false;
		Level._setupLevels.Remove(this);
	}

	// Token: 0x06000D2C RID: 3372 RVA: 0x000692F4 File Offset: 0x000674F4
	public void SetupIfNecessary()
	{
		if (this._isSetup)
		{
			return;
		}
		bool flag = false;
		foreach (LevelSection levelSection in MonoInstancer<LevelSection>.all)
		{
			if (levelSection.levelIdx == this.levelIdx)
			{
				flag = true;
				Transform transform = levelSection.transform;
				Slope[] componentsInChildren = transform.GetComponentsInChildren<Slope>();
				Poly[] componentsInChildren2 = transform.GetComponentsInChildren<Poly>();
				Splat[] componentsInChildren3 = transform.GetComponentsInChildren<Splat>();
				this.slopes.AddAll(componentsInChildren, (Slope slope) => slope.range, (Slope slope) => slope.GetComponent<MusicRun>() == null);
				this.polys.AddAll(componentsInChildren2, (Poly poly) => poly.xRange, (Poly poly) => !poly.sceneryOnly);
				this.props.AddAll(transform, (Prop prop) => Range.Centered(prop.transform.position.x, 2f * prop.triggerZone.attractRadius), delegate(Prop prop)
				{
					if (prop.isDynamic)
					{
						this.props.AddDynamic(prop);
						return false;
					}
					return true;
				});
				this.triggerZones.AddAll(transform, (TriggerZone zone) => Range.Centered(zone.transform.position.x, 2f * zone.attractRadius), delegate(TriggerZone triggerZone)
				{
					if (triggerZone.prop != null && triggerZone.prop.isDynamic)
					{
						this.triggerZones.AddDynamic(triggerZone);
						return false;
					}
					return true;
				});
				this.invisibleCollision.AddAll(transform, (InvisibleCollision invis) => invis.worldXRange, null);
				this.fadeSeparators.AddAll(transform, (FadeSeparator sep) => Range.Centered(sep.transform.position.x, 2f * sep.radius), null);
				this.balancePoints.AddAll(transform, (BalancePoint balancePoint) => Range.Centered(balancePoint.transform.position.x, 40f), null);
				this.trips.AddAll(transform, (TripHazard trip) => Range.Centered(trip.transform.position.x, 2f), null);
				this.weatherModifierZones.AddAll(transform, (WeatherModifierZone weatherModifierZone) => weatherModifierZone.rangeX, null);
				if (Application.isPlaying)
				{
					Poly[] array = componentsInChildren2;
					for (int i = 0; i < array.Length; i++)
					{
						array[i].OnAddedToLevel(this, true);
					}
					Slope[] array2 = componentsInChildren;
					for (int i = 0; i < array2.Length; i++)
					{
						array2[i].OnAddedToLevel(true);
					}
					Splat[] array3 = componentsInChildren3;
					for (int i = 0; i < array3.Length; i++)
					{
						array3[i].OnAddedToLevel(true);
					}
				}
				foreach (Poly poly2 in componentsInChildren2)
				{
					if (poly2.isCutawayWall)
					{
						this.cutawayPolys.Add(poly2);
					}
				}
				this.cameraVolumes.AddRange(transform.GetComponentsInChildren<CameraVolume>());
				this.flattenSplats.AddRange(transform.GetComponentsInChildren<FlattenSplats>());
				this.lodItems.AddRange(transform.GetComponentsInChildren<LODItem>());
				this.bellyWriggleEnds.AddRange(transform.GetComponentsInChildren<BellyWriggleEnd>());
				this.flocks.AddRange(transform.GetComponentsInChildren<Flock>());
				this.creatures.AddRange(transform.GetComponentsInChildren<Creature>());
				this.cameraPushVolumes.AddRange(transform.GetComponentsInChildren<CameraPushVolume>());
				this.cloudGroups.AddRange(transform.GetComponentsInChildren<CloudGroup>());
				this.characters.AddRange(transform.GetComponentsInChildren<StoryCharacter>());
				this.walkers.AddRange(transform.GetComponentsInChildren<InkWalker>());
				this.litParticles.AddRange(transform.GetComponentsInChildren<LitParticles>());
				this.ropes.AddRange(transform.GetComponentsInChildren<Rope>());
				this.skiLift = transform.GetComponentInChildren<SkiLift>();
				this.musicRuns.AddRange(transform.GetComponentsInChildren<MusicRun>());
				foreach (MusicRun musicRun in this.musicRuns)
				{
					musicRun.FindStaticFadingSplatsIfNecessary();
					musicRun.poly.isStatic = false;
				}
				this.introCameraKeyframes.AddRange(MonoInstancer<IntroCameraKeyframe>.all.Where((IntroCameraKeyframe k) => Level.DepthToIndex(k.transform.position.z) == this.levelIdx));
				this.nonFlockBirds.AddRange(from b in transform.GetComponentsInChildren<Bird>()
					where b.GetComponentInChildren<Flock>() == null
					select b);
				this._propsByName.Clear();
				foreach (Prop prop2 in this.props.all)
				{
					List<Prop> list;
					this._propsByName.TryGetValue(prop2.inkListItemName, out list);
					if (list == null)
					{
						list = new List<Prop>();
						this._propsByName[prop2.inkListItemName] = list;
					}
					list.Add(prop2);
					if (prop2.isPeak)
					{
						this.peaks.Add(prop2);
					}
				}
			}
		}
		if (!flag)
		{
			string text = string.Join(", ", MonoInstancer<LevelSection>.all.Select((LevelSection s) => s.name));
			Debug.LogError(string.Format("Level.SetActiveLevelIdx didn't find any LevelSections at index {0}! Loaded sections: {1}", this.levelIdx, text));
		}
		this._isSetup = true;
		Level._setupLevels.Add(this);
	}

	// Token: 0x04000FDF RID: 4063
	public static int currentIndex = -1;

	// Token: 0x04000FE0 RID: 4064
	public const float levelDepth = 500f;

	// Token: 0x04000FE1 RID: 4065
	[NonSerialized]
	public List<LevelSection> loadedLevelSections = new List<LevelSection>();

	// Token: 0x04000FE2 RID: 4066
	[ScenePath(ScenePathAttribute.SceneFindMethod.AllInProject, true)]
	public List<string> scenePaths = new List<string>();

	// Token: 0x04000FE3 RID: 4067
	public int levelIdx;

	// Token: 0x04000FE5 RID: 4069
	public Mesh routesMesh;

	// Token: 0x04000FE6 RID: 4070
	[NonSerialized]
	public Bucketer<Poly> polys = new Bucketer<Poly>(30f, false, false);

	// Token: 0x04000FE7 RID: 4071
	[NonSerialized]
	public Bucketer<Slope> slopes = new Bucketer<Slope>(30f, true, true);

	// Token: 0x04000FE8 RID: 4072
	[NonSerialized]
	public Bucketer<TriggerZone> triggerZones = new Bucketer<TriggerZone>(30f, false, true);

	// Token: 0x04000FE9 RID: 4073
	[NonSerialized]
	public Bucketer<Prop> props = new Bucketer<Prop>(30f, true, false);

	// Token: 0x04000FEA RID: 4074
	[NonSerialized]
	public Bucketer<BalancePoint> balancePoints = new Bucketer<BalancePoint>(100f, false, true);

	// Token: 0x04000FEB RID: 4075
	[NonSerialized]
	public Bucketer<InvisibleCollision> invisibleCollision = new Bucketer<InvisibleCollision>(200f, true, true);

	// Token: 0x04000FEC RID: 4076
	[NonSerialized]
	public Bucketer<TripHazard> trips = new Bucketer<TripHazard>(200f, false, true);

	// Token: 0x04000FED RID: 4077
	[NonSerialized]
	public Bucketer<WeatherModifierZone> weatherModifierZones = new Bucketer<WeatherModifierZone>(200f, false, true);

	// Token: 0x04000FEE RID: 4078
	[NonSerialized]
	public Bucketer<FadeSeparator> fadeSeparators = new Bucketer<FadeSeparator>(100f, false, true);

	// Token: 0x04000FEF RID: 4079
	[NonSerialized]
	public List<CameraVolume> cameraVolumes = new List<CameraVolume>();

	// Token: 0x04000FF0 RID: 4080
	[NonSerialized]
	public List<MusicRun> musicRuns = new List<MusicRun>();

	// Token: 0x04000FF1 RID: 4081
	[NonSerialized]
	public List<FlattenSplats> flattenSplats = new List<FlattenSplats>();

	// Token: 0x04000FF2 RID: 4082
	[NonSerialized]
	public List<LODItem> lodItems = new List<LODItem>();

	// Token: 0x04000FF3 RID: 4083
	[NonSerialized]
	public List<Poly> cutawayPolys = new List<Poly>(32);

	// Token: 0x04000FF4 RID: 4084
	[NonSerialized]
	public List<BellyWriggleEnd> bellyWriggleEnds = new List<BellyWriggleEnd>(32);

	// Token: 0x04000FF5 RID: 4085
	[NonSerialized]
	public List<Flock> flocks = new List<Flock>(32);

	// Token: 0x04000FF6 RID: 4086
	[NonSerialized]
	public List<Creature> creatures = new List<Creature>(32);

	// Token: 0x04000FF7 RID: 4087
	[NonSerialized]
	public List<Bird> nonFlockBirds = new List<Bird>(64);

	// Token: 0x04000FF8 RID: 4088
	[NonSerialized]
	public List<CameraPushVolume> cameraPushVolumes = new List<CameraPushVolume>(32);

	// Token: 0x04000FF9 RID: 4089
	[NonSerialized]
	public List<Prop> peaks = new List<Prop>(32);

	// Token: 0x04000FFA RID: 4090
	[NonSerialized]
	public List<CloudGroup> cloudGroups = new List<CloudGroup>(64);

	// Token: 0x04000FFB RID: 4091
	[NonSerialized]
	public List<StoryCharacter> characters = new List<StoryCharacter>(32);

	// Token: 0x04000FFC RID: 4092
	[NonSerialized]
	public List<InkWalker> walkers = new List<InkWalker>(32);

	// Token: 0x04000FFD RID: 4093
	[NonSerialized]
	public List<LitParticles> litParticles = new List<LitParticles>(64);

	// Token: 0x04000FFE RID: 4094
	[NonSerialized]
	public List<Rope> ropes = new List<Rope>(64);

	// Token: 0x04000FFF RID: 4095
	[NonSerialized]
	public SkiLift skiLift;

	// Token: 0x04001000 RID: 4096
	[NonSerialized]
	public List<IntroCameraKeyframe> introCameraKeyframes = new List<IntroCameraKeyframe>();

	// Token: 0x04001001 RID: 4097
	[NonSerialized]
	private bool _isSetup;

	// Token: 0x04001002 RID: 4098
	[NonSerialized]
	private bool _isCurrent;

	// Token: 0x04001003 RID: 4099
	private Dictionary<string, List<Prop>> _propsByName = new Dictionary<string, List<Prop>>();

	// Token: 0x04001004 RID: 4100
	private static List<Prop> _emptyPropList = new List<Prop>();

	// Token: 0x04001005 RID: 4101
	private static List<Level> _setupLevels = new List<Level>();

	// Token: 0x04001006 RID: 4102
	private static bool _hasSetGlobalShaderIds;

	// Token: 0x04001007 RID: 4103
	private static int _Global_LevelFadeId;

	// Token: 0x04001008 RID: 4104
	private static int _Global_LevelFadeCutoffId;
}
