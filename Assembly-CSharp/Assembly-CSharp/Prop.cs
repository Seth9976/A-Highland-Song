using System;
using System.Collections.Generic;
using Ink.Runtime;
using UnityEngine;
using UnityEngine.Serialization;

// Token: 0x0200019E RID: 414
[RequireComponent(typeof(GuidComponent))]
[RequireComponent(typeof(TriggerZone))]
[DisallowMultipleComponent]
public class Prop : MonoInstancer<Prop>
{
	// Token: 0x1700032C RID: 812
	// (get) Token: 0x06000D9C RID: 3484 RVA: 0x0006C086 File Offset: 0x0006A286
	public PeakStateSettings peakStateSettings
	{
		get
		{
			return MonoSingleton<PeakStateController>.instance.settings;
		}
	}

	// Token: 0x1700032D RID: 813
	// (get) Token: 0x06000D9D RID: 3485 RVA: 0x0006C092 File Offset: 0x0006A292
	public bool interactive
	{
		get
		{
			return base.gameObject.activeInHierarchy && !this._deactivationPending;
		}
	}

	// Token: 0x1700032E RID: 814
	// (get) Token: 0x06000D9E RID: 3486 RVA: 0x0006C0AC File Offset: 0x0006A2AC
	public bool isPeak
	{
		get
		{
			return this.type == Prop.Type.Peak || this.type == Prop.Type.MinorPeak;
		}
	}

	// Token: 0x1700032F RID: 815
	// (get) Token: 0x06000D9F RID: 3487 RVA: 0x0006C0C2 File Offset: 0x0006A2C2
	public bool isMinorPeak
	{
		get
		{
			return this.type == Prop.Type.MinorPeak;
		}
	}

	// Token: 0x17000330 RID: 816
	// (get) Token: 0x06000DA0 RID: 3488 RVA: 0x0006C0CD File Offset: 0x0006A2CD
	public bool isNamedMinorPeak
	{
		get
		{
			return this.isMinorPeak && this.inkListItemName != "";
		}
	}

	// Token: 0x17000331 RID: 817
	// (get) Token: 0x06000DA1 RID: 3489 RVA: 0x0006C0E9 File Offset: 0x0006A2E9
	public bool isMajorPeak
	{
		get
		{
			return this.type == Prop.Type.Peak;
		}
	}

	// Token: 0x17000332 RID: 818
	// (get) Token: 0x06000DA2 RID: 3490 RVA: 0x0006C0F4 File Offset: 0x0006A2F4
	public bool isShelter
	{
		get
		{
			return this.type == Prop.Type.Shelter;
		}
	}

	// Token: 0x17000333 RID: 819
	// (get) Token: 0x06000DA3 RID: 3491 RVA: 0x0006C0FF File Offset: 0x0006A2FF
	public bool isPathOut
	{
		get
		{
			return this.type == Prop.Type.PathOut;
		}
	}

	// Token: 0x17000334 RID: 820
	// (get) Token: 0x06000DA4 RID: 3492 RVA: 0x0006C10A File Offset: 0x0006A30A
	public bool isInkInteractable
	{
		get
		{
			return this.type == Prop.Type.InkInteractable;
		}
	}

	// Token: 0x17000335 RID: 821
	// (get) Token: 0x06000DA5 RID: 3493 RVA: 0x0006C116 File Offset: 0x0006A316
	public bool isInkInteractableLike
	{
		get
		{
			return this.isInkInteractable || this.isShelter || this.isPathOut || this.isMajorPeak || this.isNamedMinorPeak;
		}
	}

	// Token: 0x17000336 RID: 822
	// (get) Token: 0x06000DA6 RID: 3494 RVA: 0x0006C140 File Offset: 0x0006A340
	public bool isInkZone
	{
		get
		{
			return this.type == Prop.Type.InkZone;
		}
	}

	// Token: 0x17000337 RID: 823
	// (get) Token: 0x06000DA7 RID: 3495 RVA: 0x0006C14C File Offset: 0x0006A34C
	public Vector3 widgetAttachPos
	{
		get
		{
			if (!(this.customWidgetAttach != null))
			{
				return base.transform.position;
			}
			return this.customWidgetAttach.position;
		}
	}

	// Token: 0x17000338 RID: 824
	// (get) Token: 0x06000DA8 RID: 3496 RVA: 0x0006C174 File Offset: 0x0006A374
	public InkList inkListVar
	{
		get
		{
			if (Application.isPlaying)
			{
				if (this._inkListVar == null)
				{
					if (string.IsNullOrWhiteSpace(this._inkListItemName))
					{
						return null;
					}
					if (Narrative.instance.inkStory == null)
					{
						return null;
					}
					ListValue listValue = Narrative.instance.inkStory.listDefinitions.FindSingleItemListWithName(this.inkListItemName);
					if (listValue == null)
					{
						return null;
					}
					this._inkListVar = listValue.value;
				}
				return this._inkListVar;
			}
			return null;
		}
	}

	// Token: 0x17000339 RID: 825
	// (get) Token: 0x06000DA9 RID: 3497 RVA: 0x0006C1F0 File Offset: 0x0006A3F0
	public string uniqueID
	{
		get
		{
			return this._inkListItemName + " " + this.guid.ToString();
		}
	}

	// Token: 0x1700033A RID: 826
	// (get) Token: 0x06000DAA RID: 3498 RVA: 0x0006C221 File Offset: 0x0006A421
	public string inkListItemName
	{
		get
		{
			if (this.isPathOut && string.IsNullOrWhiteSpace(this._inkListItemName))
			{
				return "GENERIC_PATH";
			}
			return this._inkListItemName;
		}
	}

	// Token: 0x1700033B RID: 827
	// (get) Token: 0x06000DAB RID: 3499 RVA: 0x0006C244 File Offset: 0x0006A444
	public WeatherModifierZone weatherModifierZone
	{
		get
		{
			if (this.hasNoWeatherModifierZone)
			{
				return null;
			}
			if (this._weatherModifierZone == null)
			{
				this._weatherModifierZone = base.GetComponent<WeatherModifierZone>();
			}
			if (this._weatherModifierZone == null)
			{
				this.hasNoWeatherModifierZone = true;
			}
			return this._weatherModifierZone;
		}
	}

	// Token: 0x1700033C RID: 828
	// (get) Token: 0x06000DAC RID: 3500 RVA: 0x0006C290 File Offset: 0x0006A490
	public bool autoRunToZone
	{
		get
		{
			return this.isPeak || (this.isInkZone && this._autoRunToZone);
		}
	}

	// Token: 0x1700033D RID: 829
	// (get) Token: 0x06000DAD RID: 3501 RVA: 0x0006C2AC File Offset: 0x0006A4AC
	// (set) Token: 0x06000DAE RID: 3502 RVA: 0x0006C2B4 File Offset: 0x0006A4B4
	public bool highlight
	{
		get
		{
			return this._highlight;
		}
		set
		{
			if (this._highlight != value)
			{
				this._highlight = value;
			}
		}
	}

	// Token: 0x1700033E RID: 830
	// (get) Token: 0x06000DAF RID: 3503 RVA: 0x0006C2C8 File Offset: 0x0006A4C8
	public int pathLevelsChange
	{
		get
		{
			GameObject gameObject = this.pathDestination.gameObject;
			if (!(gameObject == null))
			{
				return Level.GetForTransform(gameObject.transform).levelIdx - Level.GetForTransform(base.transform).levelIdx;
			}
			if (this.pathDestination.hasAssignedGUID)
			{
				Debug.LogError("Returning arbitrary pathLayersChange number (10) for Prop path " + base.name + " since destination is not loaded, so change is unknown", this);
				return 10;
			}
			return 0;
		}
	}

	// Token: 0x1700033F RID: 831
	// (get) Token: 0x06000DB0 RID: 3504 RVA: 0x0006C338 File Offset: 0x0006A538
	public float pathCrowFliesLength
	{
		get
		{
			GameObject gameObject = this.pathDestination.gameObject;
			if (gameObject == null)
			{
				return 0f;
			}
			return Vector3.Distance(base.transform.position, gameObject.transform.position);
		}
	}

	// Token: 0x17000340 RID: 832
	// (get) Token: 0x06000DB1 RID: 3505 RVA: 0x0006C37B File Offset: 0x0006A57B
	public bool pathDestinationIsLoaded
	{
		get
		{
			return this.pathDestination.gameObject != null;
		}
	}

	// Token: 0x17000341 RID: 833
	// (get) Token: 0x06000DB2 RID: 3506 RVA: 0x0006C390 File Offset: 0x0006A590
	public float pathTravelTimeHours
	{
		get
		{
			bool flag = this.pathLevelsChange > 0;
			Vector2 vector = (flag ? GameClock.instance.settings.betweenLevelPathLengthBounds : GameClock.instance.settings.inLevelPathLengthBounds);
			Vector2 vector2 = (flag ? GameClock.instance.settings.betweenLevelPathDurationBoundsInMinutes : GameClock.instance.settings.inLevelPathDurationBoundsInMinutes);
			float num = Mathf.InverseLerp(vector[0], vector[1], this.pathCrowFliesLength);
			if (Application.isPlaying && flag && Narrative.instance.PlayerHasCompletedMapWithPropName(this.inkListItemName) && Narrative.instance.MapIsEasterEgg(this.inkListItemName))
			{
				num *= GameClock.instance.settings.easterEggPathTimeScalar;
			}
			num = Mathf.Pow(num, flag ? 0.8f : 1.2f);
			return Mathf.Lerp(vector2[0], vector2[1], num) / 60f;
		}
	}

	// Token: 0x17000342 RID: 834
	// (get) Token: 0x06000DB3 RID: 3507 RVA: 0x0006C47C File Offset: 0x0006A67C
	public bool pathIsLocal
	{
		get
		{
			return !(this.pathDestination.gameObject == null) && this.pathLevelsChange == 0;
		}
	}

	// Token: 0x17000343 RID: 835
	// (get) Token: 0x06000DB4 RID: 3508 RVA: 0x0006C49C File Offset: 0x0006A69C
	public Prop nearestPathPropEnd
	{
		get
		{
			GameObject gameObject = this.pathDestination.gameObject;
			if (gameObject != null)
			{
				Prop component = gameObject.GetComponent<Prop>();
				if (component != null && component.transform.position.z < base.transform.position.z)
				{
					return component;
				}
			}
			return this;
		}
	}

	// Token: 0x06000DB5 RID: 3509 RVA: 0x0006C4F4 File Offset: 0x0006A6F4
	public void SetEnabledByInk(bool inkEnabled, bool immediate = false)
	{
		if (inkEnabled)
		{
			base.gameObject.SetActive(true);
			return;
		}
		InkAnimation componentInChildren = base.GetComponentInChildren<InkAnimation>();
		if (componentInChildren != null && componentInChildren.isPlaying && !immediate)
		{
			this._deactivationPending = true;
			NarrativePresenter.instance.DeactivatePropWhenAnimationComplete(this, componentInChildren);
			return;
		}
		base.gameObject.SetActive(false);
	}

	// Token: 0x17000344 RID: 836
	// (get) Token: 0x06000DB6 RID: 3510 RVA: 0x0006C54C File Offset: 0x0006A74C
	public static List<Prop> allLoadedProps
	{
		get
		{
			return Prop._allLoadedProps;
		}
	}

	// Token: 0x17000345 RID: 837
	// (get) Token: 0x06000DB7 RID: 3511 RVA: 0x0006C553 File Offset: 0x0006A753
	public static List<Prop> allLoadedPeaks
	{
		get
		{
			return Prop._allLoadedPeaks;
		}
	}

	// Token: 0x06000DB8 RID: 3512 RVA: 0x0006C55C File Offset: 0x0006A75C
	public static void RefreshLoadedProps()
	{
		Prop._allLoadedProps.Clear();
		Prop._allLoadedPeaks.Clear();
		Prop._allPropsByInkName.Clear();
		foreach (Component component in MonoInstancer<LevelSection>.all)
		{
			Prop._levelSectionPropsScratch.Clear();
			component.GetComponentsInChildren<Prop>(true, Prop._levelSectionPropsScratch);
			Prop._allLoadedProps.AddRange(Prop._levelSectionPropsScratch);
		}
		foreach (Prop prop in Prop._allLoadedProps)
		{
			if (prop.inkListItemName != null)
			{
				if (!Prop._allPropsByInkName.ContainsKey(prop.inkListItemName))
				{
					Prop._allPropsByInkName[prop.inkListItemName] = new List<Prop>(1);
				}
				Prop._allPropsByInkName[prop.inkListItemName].Add(prop);
			}
			else
			{
				Debug.LogWarning("Prop '" + prop.name + "' doesn't have an ink name?", prop);
			}
		}
		foreach (Prop prop2 in Prop._allLoadedProps)
		{
			if (prop2.isPeak)
			{
				Prop._allLoadedPeaks.Add(prop2);
			}
		}
	}

	// Token: 0x06000DB9 RID: 3513 RVA: 0x0006C6D4 File Offset: 0x0006A8D4
	public static List<Prop> GetLoadedPropsByInkName(string inkName)
	{
		List<Prop> list = null;
		Prop._allPropsByInkName.TryGetValue(inkName, out list);
		if (list == null)
		{
			list = Prop._emptyList;
		}
		return list;
	}

	// Token: 0x06000DBA RID: 3514 RVA: 0x0006C6FB File Offset: 0x0006A8FB
	public static Prop NearestMajorOrMinorPeak(Vector3 point)
	{
		return Prop.NearestMajorOrMinorPeak(point, true);
	}

	// Token: 0x06000DBB RID: 3515 RVA: 0x0006C704 File Offset: 0x0006A904
	public static Prop NearestMajorPeak(Vector3 point)
	{
		return Prop.NearestMajorOrMinorPeak(point, false);
	}

	// Token: 0x06000DBC RID: 3516 RVA: 0x0006C710 File Offset: 0x0006A910
	private static Prop NearestMajorOrMinorPeak(Vector3 point, bool allowMinorPeaks)
	{
		float num = float.PositiveInfinity;
		Prop prop = null;
		foreach (Prop prop2 in Level.current.peaks)
		{
			if (allowMinorPeaks || prop2.type == Prop.Type.Peak)
			{
				float num2 = Vector3.Distance(point, prop2.transform.position);
				if (num2 < num)
				{
					num = num2;
					prop = prop2;
				}
			}
		}
		return prop;
	}

	// Token: 0x06000DBD RID: 3517 RVA: 0x0006C794 File Offset: 0x0006A994
	public static Prop FindNearestByInkName(Vector3 point, string propName)
	{
		List<Prop> loadedPropsByInkName = Prop.GetLoadedPropsByInkName(propName);
		if (loadedPropsByInkName.Count == 0)
		{
			return null;
		}
		return loadedPropsByInkName.WithMin((Prop t) => Vector3.Distance(point, t.transform.position));
	}

	// Token: 0x06000DBE RID: 3518 RVA: 0x0006C7D1 File Offset: 0x0006A9D1
	protected override void OnDisable()
	{
		base.OnDisable();
		this._deactivationPending = false;
	}

	// Token: 0x17000346 RID: 838
	// (get) Token: 0x06000DBF RID: 3519 RVA: 0x0006C7E0 File Offset: 0x0006A9E0
	private IEnumerable<Splat> highlightSplats
	{
		get
		{
			if (this._customHighlights != null && this._customHighlights.Count > 0)
			{
				foreach (Splat splat in this._customHighlights)
				{
					yield return splat;
				}
				List<Splat>.Enumerator enumerator = default(List<Splat>.Enumerator);
			}
			else
			{
				Splat component = base.GetComponent<Splat>();
				if (component != null)
				{
					yield return component;
				}
			}
			yield break;
			yield break;
		}
	}

	// Token: 0x06000DC0 RID: 3520 RVA: 0x0006C7F0 File Offset: 0x0006A9F0
	private void UpdateHighlight()
	{
		float num = (this.highlight ? 1f : 0f);
		if (this.highlightOpacity != num)
		{
			float num2 = ((num > this.highlightOpacity) ? 0.2f : 0.5f);
			this.highlightOpacity = Mathf.MoveTowards(this.highlightOpacity, num, Time.deltaTime / num2);
			foreach (Splat splat in this.highlightSplats)
			{
				if (!(splat == null))
				{
					splat.SetHighlightProperty(this.highlightOpacity);
				}
			}
		}
	}

	// Token: 0x17000347 RID: 839
	// (get) Token: 0x06000DC1 RID: 3521 RVA: 0x0006C898 File Offset: 0x0006AA98
	public TriggerZone triggerZone
	{
		get
		{
			if (this._triggerZone == null)
			{
				this._triggerZone = base.GetComponent<TriggerZone>();
			}
			return this._triggerZone;
		}
	}

	// Token: 0x17000348 RID: 840
	// (get) Token: 0x06000DC2 RID: 3522 RVA: 0x0006C8BA File Offset: 0x0006AABA
	public int levelIdx
	{
		get
		{
			return Level.GetForTransform(base.transform).levelIdx;
		}
	}

	// Token: 0x17000349 RID: 841
	// (get) Token: 0x06000DC3 RID: 3523 RVA: 0x0006C8CC File Offset: 0x0006AACC
	public Vector2 highlightMid
	{
		get
		{
			if (this._customHighlights == null || this._customHighlights.Count == 0)
			{
				return base.transform.position;
			}
			Vector2 vector = default(Vector2);
			foreach (Splat splat in this._customHighlights)
			{
				vector += splat.transform.position;
			}
			vector /= (float)this._customHighlights.Count;
			return vector;
		}
	}

	// Token: 0x1700034A RID: 842
	// (get) Token: 0x06000DC4 RID: 3524 RVA: 0x0006C974 File Offset: 0x0006AB74
	private Poly ownPoly
	{
		get
		{
			if (this._ownPoly == null)
			{
				this._ownPoly = base.GetComponent<Poly>();
			}
			return this._ownPoly;
		}
	}

	// Token: 0x1700034B RID: 843
	// (get) Token: 0x06000DC5 RID: 3525 RVA: 0x0006C998 File Offset: 0x0006AB98
	public Transform waterPlane
	{
		get
		{
			WaterPlane waterPlane = MonoInstancer<WaterPlane>.all.Best((WaterPlane x) => x.bounds.SignedDistanceFromPoint(base.transform.position), (float other, float currentBest) => other < currentBest, float.PositiveInfinity, null);
			if (waterPlane != null)
			{
				return waterPlane.transform;
			}
			return base.transform;
		}
	}

	// Token: 0x1700034C RID: 844
	// (get) Token: 0x06000DC6 RID: 3526 RVA: 0x0006C9F7 File Offset: 0x0006ABF7
	public Guid guid
	{
		get
		{
			return this.guidComponent.GetGuid();
		}
	}

	// Token: 0x06000DC7 RID: 3527 RVA: 0x0006CA04 File Offset: 0x0006AC04
	protected override void OnEnable()
	{
		base.OnEnable();
		this.ValidateGUID();
	}

	// Token: 0x06000DC8 RID: 3528 RVA: 0x0006CA14 File Offset: 0x0006AC14
	private void ValidateGUID()
	{
		bool flag = false;
		if (this.guidComponent == null)
		{
			flag = true;
			this.guidComponent = base.GetComponent<GuidComponent>();
		}
		if (this.guidComponent.didJustAssignNewGUID || flag)
		{
			this.OnSetGUID();
			return;
		}
		GuidComponent guidComponent = this.guidComponent;
		guidComponent.OnSetGUID = (Action)Delegate.Combine(guidComponent.OnSetGUID, new Action(this.OnSetGUID));
	}

	// Token: 0x06000DC9 RID: 3529 RVA: 0x0006CA7C File Offset: 0x0006AC7C
	private void OnSetGUID()
	{
		this.InitialSetUp();
	}

	// Token: 0x06000DCA RID: 3530 RVA: 0x0006CA84 File Offset: 0x0006AC84
	private void InitialSetUp()
	{
		this.guidComponent.hideFlags = HideFlags.HideInInspector;
	}

	// Token: 0x06000DCB RID: 3531 RVA: 0x0006CA92 File Offset: 0x0006AC92
	public bool IsCurrentlyAvailableAsNarrativeChoice()
	{
		return this.isInkInteractableLike || this.isInkZone || this.isMajorPeak;
	}

	// Token: 0x0400106D RID: 4205
	public Prop.Type type = Prop.Type.InkInteractable;

	// Token: 0x0400106E RID: 4206
	public bool isDynamic;

	// Token: 0x0400106F RID: 4207
	public Transform customWidgetAttach;

	// Token: 0x04001070 RID: 4208
	[NonSerialized]
	public bool hasRestChoice;

	// Token: 0x04001071 RID: 4209
	[NonSerialized]
	public bool hasTakePathChoice;

	// Token: 0x04001072 RID: 4210
	private InkList _inkListVar;

	// Token: 0x04001073 RID: 4211
	[SerializeField]
	private string _inkListItemName;

	// Token: 0x04001074 RID: 4212
	public bool disabledAtStart;

	// Token: 0x04001075 RID: 4213
	private bool hasNoWeatherModifierZone;

	// Token: 0x04001076 RID: 4214
	private WeatherModifierZone _weatherModifierZone;

	// Token: 0x04001077 RID: 4215
	[FormerlySerializedAs("_autoRunToSpot")]
	[SerializeField]
	private bool _autoRunToZone;

	// Token: 0x04001078 RID: 4216
	public bool preventRest;

	// Token: 0x04001079 RID: 4217
	public int preferredLookDirection;

	// Token: 0x0400107A RID: 4218
	public GuidReference pathDestination = new GuidReference();

	// Token: 0x0400107B RID: 4219
	public Prop.PathExitDirection pathDestinationExitDir;

	// Token: 0x0400107C RID: 4220
	public Prop.PathAnimType pathAnimType;

	// Token: 0x0400107D RID: 4221
	private bool _highlight;

	// Token: 0x0400107E RID: 4222
	private float highlightOpacity;

	// Token: 0x0400107F RID: 4223
	private const float highlightFadeUpDuration = 0.2f;

	// Token: 0x04001080 RID: 4224
	private const float highlightFadeDownDuration = 0.5f;

	// Token: 0x04001081 RID: 4225
	private static List<Prop> _levelSectionPropsScratch = new List<Prop>();

	// Token: 0x04001082 RID: 4226
	private TriggerZone _triggerZone;

	// Token: 0x04001083 RID: 4227
	private Poly _ownPoly;

	// Token: 0x04001084 RID: 4228
	[SerializeField]
	private List<Splat> _customHighlights;

	// Token: 0x04001085 RID: 4229
	[SerializeField]
	private GuidComponent guidComponent;

	// Token: 0x04001086 RID: 4230
	private bool _deactivationPending;

	// Token: 0x04001087 RID: 4231
	private static List<Prop> _allLoadedProps = new List<Prop>(512);

	// Token: 0x04001088 RID: 4232
	private static List<Prop> _allLoadedPeaks = new List<Prop>(128);

	// Token: 0x04001089 RID: 4233
	private static Dictionary<string, List<Prop>> _allPropsByInkName = new Dictionary<string, List<Prop>>(StringComparer.OrdinalIgnoreCase);

	// Token: 0x0400108A RID: 4234
	private static List<Prop> _emptyList = new List<Prop>(0);

	// Token: 0x020003B2 RID: 946
	public enum Type
	{
		// Token: 0x040019CD RID: 6605
		Peak = 1,
		// Token: 0x040019CE RID: 6606
		Shelter,
		// Token: 0x040019CF RID: 6607
		PathOut,
		// Token: 0x040019D0 RID: 6608
		MinorPeak,
		// Token: 0x040019D1 RID: 6609
		InkInteractable = 100,
		// Token: 0x040019D2 RID: 6610
		InkZone
	}

	// Token: 0x020003B3 RID: 947
	public enum PathExitDirection
	{
		// Token: 0x040019D4 RID: 6612
		Left,
		// Token: 0x040019D5 RID: 6613
		Right
	}

	// Token: 0x020003B4 RID: 948
	public enum PathAnimType
	{
		// Token: 0x040019D7 RID: 6615
		Normal,
		// Token: 0x040019D8 RID: 6616
		RopeClimbDown,
		// Token: 0x040019D9 RID: 6617
		RopeClimbUp,
		// Token: 0x040019DA RID: 6618
		StairClimbDown,
		// Token: 0x040019DB RID: 6619
		StairClimbUp,
		// Token: 0x040019DC RID: 6620
		BellyWriggle
	}

	// Token: 0x020003B5 RID: 949
	[Flags]
	public enum PropUIStateFlags
	{
		// Token: 0x040019DE RID: 6622
		ShowLabelWhenNear = 1,
		// Token: 0x040019DF RID: 6623
		ShowLabelWhenFocused = 2,
		// Token: 0x040019E0 RID: 6624
		ShowTwinkliesWhenZoomedOut = 4,
		// Token: 0x040019E1 RID: 6625
		CanSetAsWaypointWhenZoomedOut = 8,
		// Token: 0x040019E2 RID: 6626
		CanSetAsWaypointWhenZoomedIn = 16,
		// Token: 0x040019E3 RID: 6627
		MagneticWhenZoomedOut = 32,
		// Token: 0x040019E4 RID: 6628
		MagneticWhenZoomedIn = 64,
		// Token: 0x040019E5 RID: 6629
		CanCommentOnProp = 128
	}
}
