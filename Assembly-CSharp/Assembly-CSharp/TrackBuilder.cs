using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

// Token: 0x02000082 RID: 130
[NullableContext(1)]
[Nullable(new byte[] { 0, 1 })]
public class TrackBuilder : MonoSingleton<TrackBuilder>
{
	// Token: 0x060003AA RID: 938 RVA: 0x0001C9CC File Offset: 0x0001ABCC
	private TrackBuilder.MatchOption TryChunkMatchOption(Chunk chunkProto, TrackBuilder.MatchParams matchParams, TrackBuilder.MatchFallbackAttempt fallback = TrackBuilder.MatchFallbackAttempt.None)
	{
		TrackBuilder.MatchOption matchOption = new TrackBuilder.MatchOption
		{
			chunkProto = chunkProto,
			matchParams = matchParams,
			fallback = fallback,
			rejectReason = TrackBuilder.MatchRejectReason.None
		};
		if (chunkProto.debugFallbackOnly && !DebugOptions.opts.allowDebugSmallRockChunks)
		{
			return matchOption.Reject(TrackBuilder.MatchRejectReason.DebugFallbackOnly);
		}
		if (chunkProto.dontPatternMatch)
		{
			return matchOption.Reject(TrackBuilder.MatchRejectReason.NeverPatternMatch);
		}
		if ((!matchParams.eastwards || !chunkProto.canBeUsedEastwards) && (matchParams.eastwards || !chunkProto.canBeUsedWestwards))
		{
			return matchOption.Reject(TrackBuilder.MatchRejectReason.Direction);
		}
		if (!chunkProto.validSlopeAbsAngleRange.Contains(matchParams.requiredAbsSlopeRange))
		{
			return matchOption.Reject(TrackBuilder.MatchRejectReason.AngleRange);
		}
		if (!chunkProto.CanMatchObstacles(matchOption.matchParams.obstacles, matchParams.beatCount))
		{
			return matchOption.Reject(TrackBuilder.MatchRejectReason.Obstacles);
		}
		matchOption.scoreWeight = chunkProto.weight;
		if (matchOption.chunkProto.originalPrototypeRefCount > 1)
		{
			matchOption.scoreWeight *= 0.01f;
		}
		else if (matchOption.chunkProto.originalPrototypeRefCount > 0)
		{
			matchOption.scoreWeight *= 0.1f;
		}
		else if (chunkProto.debugSuperHighWeight)
		{
			matchOption.scoreWeight *= 100f;
		}
		if (fallback != TrackBuilder.MatchFallbackAttempt.None)
		{
			float num = this._fallbackScoreWeightScalars[(int)fallback];
			if (num <= 0f)
			{
				Debug.LogError(string.Format("Fallback score scalar was {0} for {1}", num, fallback));
			}
			matchOption.scoreWeight *= num;
		}
		return matchOption;
	}

	// Token: 0x060003AB RID: 939 RVA: 0x0001CB40 File Offset: 0x0001AD40
	[NullableContext(0)]
	[return: TupleElementNames(new string[] { "hadSpecials", "obsWithoutSlides" })]
	private static ValueTuple<bool, Obstacles> ObstaclesWithoutSpecials(Obstacles obs, int beatCount)
	{
		bool flag = false;
		for (int i = 0; i < beatCount * 2; i++)
		{
			if (obs.Has(i, ObstacleType.Special))
			{
				obs.Remove(i, ObstacleType.Special);
				flag = true;
			}
		}
		return new ValueTuple<bool, Obstacles>(flag, obs);
	}

	// Token: 0x060003AC RID: 940 RVA: 0x0001CB7C File Offset: 0x0001AD7C
	[NullableContext(0)]
	[return: TupleElementNames(new string[] { "hadTricky", "obsWithoutTrickys" })]
	private static ValueTuple<bool, Obstacles> ObstaclesAfterChoosingEasyMode(bool easyMode, Obstacles obs, int beatCount)
	{
		bool flag = false;
		for (int i = 0; i < beatCount * 2; i++)
		{
			if (obs.Has(i, ObstacleType.Tricky))
			{
				obs.Remove(i, ObstacleType.Tricky);
				if (!easyMode)
				{
					obs.Add(i, ObstacleType.Hop);
				}
				flag = true;
			}
		}
		return new ValueTuple<bool, Obstacles>(flag, obs);
	}

	// Token: 0x060003AD RID: 941 RVA: 0x0001CBC4 File Offset: 0x0001ADC4
	[NullableContext(0)]
	[return: TupleElementNames(new string[] { "hadHalfBeatHops", "obsWithoutHalfBeatHops" })]
	private static ValueTuple<bool, Obstacles> ObstaclesWithoutHalfBeatHops(Obstacles obs, int beatCount)
	{
		ObstacleType obstacleType = ObstacleType.Hop | ObstacleType.Special | ObstacleType.Tricky;
		bool flag = false;
		bool flag2 = false;
		for (int i = 0; i < 2 * beatCount; i++)
		{
			bool flag3 = obs.HasAny(i, obstacleType);
			if (flag3 && flag2)
			{
				obs.Remove(i, obstacleType);
				flag = true;
			}
			flag2 = flag3;
		}
		return new ValueTuple<bool, Obstacles>(flag, obs);
	}

	// Token: 0x060003AE RID: 942 RVA: 0x0001CC08 File Offset: 0x0001AE08
	private TrackBuilder.MatchDecision ChooseChunkPrototypeWithObstacles(TrackBuilder.MatchParams matchParams)
	{
		string text = (matchParams.eastwards ? "east" : "west");
		TrackBuilder.MatchDecision matchDecision = new TrackBuilder.MatchDecision(matchParams);
		for (int i = 0; i < this._chunkPrototypes.Count; i++)
		{
			Chunk chunk = this._chunkPrototypes[i];
			TrackBuilder.MatchParams matchParams2 = matchParams;
			TrackBuilder.MatchOption matchOption = this.TryChunkMatchOption(chunk, matchParams2, TrackBuilder.MatchFallbackAttempt.None);
			matchDecision.options.Add(matchOption);
			if (matchOption.rejectReason == TrackBuilder.MatchRejectReason.Obstacles)
			{
				ValueTuple<bool, Obstacles> valueTuple = TrackBuilder.ObstaclesAfterChoosingEasyMode(true, matchParams2.obstacles, matchParams.beatCount);
				bool item = valueTuple.Item1;
				Obstacles item2 = valueTuple.Item2;
				if (item)
				{
					matchParams2.obstacles = item2;
					matchOption = this.TryChunkMatchOption(chunk, matchParams2, TrackBuilder.MatchFallbackAttempt.RemoveTricky);
					matchDecision.options.Add(matchOption);
				}
			}
			if (matchOption.rejectReason == TrackBuilder.MatchRejectReason.Obstacles)
			{
				ValueTuple<bool, Obstacles> valueTuple2 = TrackBuilder.ObstaclesWithoutHalfBeatHops(matchParams2.obstacles, matchParams.beatCount);
				bool item3 = valueTuple2.Item1;
				Obstacles item4 = valueTuple2.Item2;
				if (item3)
				{
					matchParams2.obstacles = item4;
					matchOption = this.TryChunkMatchOption(chunk, matchParams2, TrackBuilder.MatchFallbackAttempt.RemoveHalfBeatHops);
					matchDecision.options.Add(matchOption);
				}
			}
			if (matchOption.rejectReason == TrackBuilder.MatchRejectReason.Obstacles)
			{
				matchParams2.obstacles = Obstacles.empty;
				matchOption = this.TryChunkMatchOption(chunk, matchParams2, TrackBuilder.MatchFallbackAttempt.RemoveAllObstacles);
				matchDecision.options.Add(matchOption);
			}
		}
		float num = 0f;
		float num2 = 0f;
		TrackBuilder._matchingOptionsScratch.Clear();
		TrackBuilder._matchingFallbackOptionsScratch.Clear();
		foreach (TrackBuilder.MatchOption matchOption2 in matchDecision.options)
		{
			if (!matchOption2.isReject)
			{
				if (matchOption2.isSuccess)
				{
					TrackBuilder._matchingOptionsScratch.Add(matchOption2);
					num += matchOption2.scoreWeight;
				}
				else
				{
					TrackBuilder._matchingFallbackOptionsScratch.Add(matchOption2);
					num2 += matchOption2.scoreWeight;
				}
			}
		}
		List<TrackBuilder.MatchOption> list = TrackBuilder._matchingOptionsScratch;
		if (list.Count == 0)
		{
			list = TrackBuilder._matchingFallbackOptionsScratch;
			num = num2;
		}
		if (list.Count > 0)
		{
			float num3 = Random.Range(0f, num);
			float num4 = 0f;
			for (int j = 0; j < list.Count; j++)
			{
				TrackBuilder.MatchOption matchOption3 = list[j];
				num4 += matchOption3.scoreWeight;
				if (num3 < num4)
				{
					TrackBuilder._matchingFallbackOptionsScratch.Clear();
					TrackBuilder._matchingOptionsScratch.Clear();
					matchDecision.chosenOption = matchOption3;
					return matchDecision;
				}
			}
			throw new Exception(string.Format("Unexpectedly didn't find choiceVal={0} in totalWeight={1} from {2} chunks", num3, num, list.Count));
		}
		Debug.LogError(new Exception(string.Concat(new string[]
		{
			"No matching chunks for pattern '",
			matchParams.obstacles.ToString(2 * matchParams.beatCount),
			"' [",
			text,
			"], even the fallback pattern isn't matching!"
		})));
		float num5 = 0.5f * (float)matchParams.beatCount;
		matchDecision.chosenOption = new TrackBuilder.MatchOption
		{
			chunkProto = this.ChooseSpacerChunkPrototype(num5),
			fallback = TrackBuilder.MatchFallbackAttempt.ErrorForceEmpty,
			matchParams = matchParams
		};
		return matchDecision;
	}

	// Token: 0x060003AF RID: 943 RVA: 0x0001CF34 File Offset: 0x0001B134
	public Chunk ChooseSpacerChunkPrototype(float time)
	{
		Chunk chunk = null;
		float num = float.MaxValue;
		foreach (Chunk chunk2 in this.spacerChunkProtos)
		{
			float num2 = Mathf.Abs(1f - time / chunk2.authoredDuration);
			if (chunk == null || num2 < num)
			{
				chunk = chunk2;
				num = num2;
			}
		}
		return chunk;
	}

	// Token: 0x1700010A RID: 266
	// (get) Token: 0x060003B0 RID: 944 RVA: 0x0001CF8E File Offset: 0x0001B18E
	// (set) Token: 0x060003B1 RID: 945 RVA: 0x0001CF9E File Offset: 0x0001B19E
	public static bool easyMode
	{
		get
		{
			return PlayerPrefsX.GetInt("MusicRunningEasyMode", 0) != 0;
		}
		set
		{
			PlayerPrefsX.SetInt("MusicRunningEasyMode", value ? 1 : 0);
			Runner.RefreshSingleJumpButtonCache();
		}
	}

	// Token: 0x1700010B RID: 267
	// (get) Token: 0x060003B2 RID: 946 RVA: 0x0001CFB6 File Offset: 0x0001B1B6
	public static bool specialJumpsAvailable
	{
		get
		{
			return !Runner.singleJumpButtonOnly && (Level.currentIndex >= 2 || Game.instance.playthroughIdx > 0 || DebugOptions.opts.forceSpecialJumpsAlwaysAvailable) && MonoSingleton<Tutorial>.instance.HasDone(TutorialId.MusicRunFirstJump);
		}
	}

	// Token: 0x060003B3 RID: 947 RVA: 0x0001CFF0 File Offset: 0x0001B1F0
	private TrackPosition GetExternalEdge(MusicRun musicRun, bool east)
	{
		TrackPosition trackPosition = new TrackPosition
		{
			slope = (east ? musicRun.eastSlopeToJoin : musicRun.westSlopeToJoin)
		};
		if (trackPosition.slope == null)
		{
			trackPosition.x = (east ? musicRun.range.max : musicRun.range.min);
			return trackPosition;
		}
		trackPosition.x = (east ? trackPosition.slope.leftEdge : trackPosition.slope.rightEdge);
		return trackPosition;
	}

	// Token: 0x060003B4 RID: 948 RVA: 0x0001D074 File Offset: 0x0001B274
	private TrackPosition GetExistingEdge(MusicRun musicRun, bool east)
	{
		if (musicRun.chunks.Count == 0)
		{
			return this.GetExternalEdge(musicRun, !east);
		}
		TrackPosition trackPosition = new TrackPosition
		{
			chunk = (east ? musicRun.eastChunk : musicRun.westChunk)
		};
		Chunk.Connector connectorEast = trackPosition.chunk.GetConnectorEast(east);
		trackPosition.slope = connectorEast.slope;
		trackPosition.x = connectorEast.point.x;
		return trackPosition;
	}

	// Token: 0x060003B5 RID: 949 RVA: 0x0001D0E7 File Offset: 0x0001B2E7
	private bool CanAddChunk(MusicRun musicRun, bool east)
	{
		if (musicRun.chunks.Count == 0)
		{
			return true;
		}
		if (!east)
		{
			return !musicRun.connectedToTrackWest;
		}
		return !musicRun.connectedToTrackEast;
	}

	// Token: 0x1700010C RID: 268
	// (get) Token: 0x060003B6 RID: 950 RVA: 0x0001D10E File Offset: 0x0001B30E
	private Runner runner
	{
		get
		{
			return GSR.Runner;
		}
	}

	// Token: 0x1700010D RID: 269
	// (get) Token: 0x060003B7 RID: 951 RVA: 0x0001D115 File Offset: 0x0001B315
	private RunTrack runTrack
	{
		get
		{
			return MonoSingleton<RunTrack>.instance;
		}
	}

	// Token: 0x1700010E RID: 270
	// (get) Token: 0x060003B8 RID: 952 RVA: 0x0001D11C File Offset: 0x0001B31C
	private Range screenZone
	{
		get
		{
			return this.MakeZone(0f, float.MaxValue);
		}
	}

	// Token: 0x1700010F RID: 271
	// (get) Token: 0x060003B9 RID: 953 RVA: 0x0001D12E File Offset: 0x0001B32E
	private Range createZone
	{
		get
		{
			return this.MakeZone(this.settings.creationZoneWidth, this.settings.maxTotalCreationZoneWidth);
		}
	}

	// Token: 0x17000110 RID: 272
	// (get) Token: 0x060003BA RID: 954 RVA: 0x0001D14C File Offset: 0x0001B34C
	private Range keepAliveZone
	{
		get
		{
			return this.MakeZone(this.settings.keepAliveZoneWidth, this.settings.maxTotalKeepAliveZoneWidth);
		}
	}

	// Token: 0x17000111 RID: 273
	// (get) Token: 0x060003BB RID: 955 RVA: 0x0001D16C File Offset: 0x0001B36C
	[Nullable(2)]
	public RhythmActionMarker nearestMarker
	{
		[NullableContext(2)]
		get
		{
			float x = Runner.instance.position.x;
			float num = float.MaxValue;
			RhythmActionMarker rhythmActionMarker = null;
			foreach (RhythmActionMarker rhythmActionMarker2 in this._markers)
			{
				float num2 = Mathf.Abs(x - rhythmActionMarker2.transform.position.x);
				if (num2 < num)
				{
					rhythmActionMarker = rhythmActionMarker2;
					num = num2;
				}
			}
			return rhythmActionMarker;
		}
	}

	// Token: 0x060003BC RID: 956 RVA: 0x0001D1F8 File Offset: 0x0001B3F8
	[NullableContext(2)]
	public RhythmActionMarker MarkerForObstacle(BeatTrack.ObstacleRef obsRef)
	{
		foreach (RhythmActionMarker rhythmActionMarker in this._markers)
		{
			if (rhythmActionMarker.obstacleRef.IsSameTimeAs(obsRef))
			{
				return rhythmActionMarker;
			}
		}
		return null;
	}

	// Token: 0x17000112 RID: 274
	// (get) Token: 0x060003BD RID: 957 RVA: 0x0001D25C File Offset: 0x0001B45C
	[Nullable(2)]
	public RhythmActionMarker nextUpcomingMarker
	{
		[NullableContext(2)]
		get
		{
			float currentMusicTime = MonoSingleton<RunTrack>.instance.currentMusicTime;
			float x = Runner.instance.position.x;
			float direction = Runner.instance.direction;
			float num = float.MaxValue;
			RhythmActionMarker rhythmActionMarker = null;
			foreach (RhythmActionMarker rhythmActionMarker2 in this._markers)
			{
				float num2 = direction * (rhythmActionMarker2.transform.position.x - x);
				if (num2 <= num && rhythmActionMarker2.obstacleRef.time > currentMusicTime + 0.1f && num2 > 2f)
				{
					num = num2;
					rhythmActionMarker = rhythmActionMarker2;
				}
			}
			return rhythmActionMarker;
		}
	}

	// Token: 0x060003BE RID: 958 RVA: 0x0001D31C File Offset: 0x0001B51C
	private Range MakeZone(float edgeWidth, float maxTotalWidth)
	{
		float num = GameCamera.ViewWidth(this.runner.visualDepth) + 2f * edgeWidth;
		if (num > maxTotalWidth)
		{
			num = maxTotalWidth;
		}
		return Range.Centered(Runner.instance.position.x, num);
	}

	// Token: 0x060003BF RID: 959 RVA: 0x0001D360 File Offset: 0x0001B560
	private void Awake()
	{
		this.library.GetComponentsInChildren<Chunk>(this._chunkPrototypes);
		this._chunkPrototypes.RemoveAll((Chunk c) => c.prototype == null);
		this.GenerateWestwardChunks();
		Runner.onDidJumpObstacle = (Runner.OnDidJumpObstacleDelegate)Delegate.Combine(Runner.onDidJumpObstacle, new Runner.OnDidJumpObstacleDelegate(this.OnRunnerJumpedMusicRunObstacle));
	}

	// Token: 0x060003C0 RID: 960 RVA: 0x0001D3CF File Offset: 0x0001B5CF
	private void OnEnable()
	{
		Game.onUIPositionUpdate += this.OnUIPositionUpdate;
	}

	// Token: 0x060003C1 RID: 961 RVA: 0x0001D3E2 File Offset: 0x0001B5E2
	private void OnDisable()
	{
		Game.onUIPositionUpdate -= this.OnUIPositionUpdate;
		this.ClearPrompts();
	}

	// Token: 0x060003C2 RID: 962 RVA: 0x0001D3FC File Offset: 0x0001B5FC
	private void ClearPrompts()
	{
		foreach (RhythmActionMarkerPrompt rhythmActionMarkerPrompt in this._prompts)
		{
			if (rhythmActionMarkerPrompt.marker != null)
			{
				rhythmActionMarkerPrompt.marker.buttonPrompt = null;
				rhythmActionMarkerPrompt.marker = null;
			}
			rhythmActionMarkerPrompt.prototype.ReturnToPool();
		}
		this._prompts.Clear();
	}

	// Token: 0x060003C3 RID: 963 RVA: 0x0001D480 File Offset: 0x0001B680
	protected override void OnDestroy()
	{
		base.OnDestroy();
		Runner.onDidJumpObstacle = (Runner.OnDidJumpObstacleDelegate)Delegate.Remove(Runner.onDidJumpObstacle, new Runner.OnDidJumpObstacleDelegate(this.OnRunnerJumpedMusicRunObstacle));
	}

	// Token: 0x060003C4 RID: 964 RVA: 0x0001D4A8 File Offset: 0x0001B6A8
	private void GenerateWestwardChunks()
	{
		List<Chunk> list = new List<Chunk>();
		foreach (Chunk chunk in this._chunkPrototypes)
		{
			if (chunk.direction == Chunk.Direction.Eastwards && !chunk.departsMusicRun && !chunk.dontPatternMatch && chunk.matchType == Chunk.CanMatch.Specific)
			{
				Chunk chunk2 = Object.Instantiate<Chunk>(chunk, chunk.transform.position - 30f * Vector3.up, Quaternion.identity, this.library);
				foreach (Splat splat in chunk2.splats)
				{
					splat.customMesh = null;
				}
				chunk2.name = "AUTO-WEST " + chunk2.name;
				Bake.Flip(chunk2.transform, null);
				list.Add(chunk2);
				chunk2.flippedPrototype = chunk;
				chunk.flippedPrototype = chunk2;
				foreach (Splat splat2 in chunk2.splats)
				{
					if (splat2.splatMode == Splat.SplatMode.Surface)
					{
						splat2.Refresh();
					}
				}
			}
		}
		this._chunkPrototypes.AddRange(list);
	}

	// Token: 0x060003C5 RID: 965 RVA: 0x0001D658 File Offset: 0x0001B858
	private void Update()
	{
		if (!Game.loaded)
		{
			return;
		}
		if (MonoSingleton<RunTrack>.instance.paused)
		{
			return;
		}
		if (!DebugOptions.opts.dontCreateTrack && DebugOptions.opts.drawTrackCreationZoneDebugLines)
		{
			Debug.DrawLine(new Vector3(this.screenZone.min, -1000f, 0f), new Vector3(this.screenZone.min, 1000f, 0f), Color.green);
			Debug.DrawLine(new Vector3(this.screenZone.max, -1000f, 0f), new Vector3(this.screenZone.max, 1000f, 0f), Color.green);
			Debug.DrawLine(new Vector3(this.createZone.min, -1000f, 0f), new Vector3(this.createZone.min, 1000f, 0f), Color.blue);
			Debug.DrawLine(new Vector3(this.createZone.max, -1000f, 0f), new Vector3(this.createZone.max, 1000f, 0f), Color.blue);
			Debug.DrawLine(new Vector3(this.keepAliveZone.min, -1000f, 0f), new Vector3(this.keepAliveZone.min, 1000f, 0f), Color.red);
			Debug.DrawLine(new Vector3(this.keepAliveZone.max, -1000f, 0f), new Vector3(this.keepAliveZone.max, 1000f, 0f), Color.red);
		}
		this.UpdateBuilding();
		this.UpdateObstacleMarkers();
	}

	// Token: 0x060003C6 RID: 966 RVA: 0x0001D818 File Offset: 0x0001BA18
	public void UpdateBuilding()
	{
		this.runTrack.worldPosToStartMusic = TrackPosition.none;
		if (DebugOptions.opts.dontCreateTrack)
		{
			return;
		}
		foreach (MusicRun musicRun in Level.current.musicRuns)
		{
			if (!musicRun.chunkTestingMode)
			{
				this.GenerateOnMusicRun(musicRun);
				if (!DebugOptions.opts.dontDestroyOutOfBoundsTrack)
				{
					if (musicRun.chunks.Count > 0 && musicRun.eastChunk.boundsRange.min > this.keepAliveZone.max)
					{
						this.TryRemoveChunk(musicRun, true);
					}
					if (musicRun.chunks.Count > 0 && musicRun.westChunk.boundsRange.max < this.keepAliveZone.min)
					{
						this.TryRemoveChunk(musicRun, false);
					}
					if (musicRun != this.runner.currentMusicRun)
					{
						Range range = Range.Centered(this.runner.position.y, this.settings.keepAliveZoneHeight);
						if (musicRun.chunks.Count > 0 && !range.Contains(musicRun.eastChunk.GetConnectorEast(true).point.y))
						{
							this.TryRemoveChunk(musicRun, true);
						}
						if (musicRun.chunks.Count > 0 && !range.Contains(musicRun.westChunk.GetConnectorEast(false).point.y))
						{
							this.TryRemoveChunk(musicRun, false);
						}
					}
				}
				if (!this.runTrack.playing && this.runner.currentMusicRun == musicRun && ((this.runner.runningEastwards && !musicRun.connectedToTrackEast) || (!this.runner.runningEastwards && !musicRun.connectedToTrackWest)))
				{
					this.runTrack.worldPosToStartMusic = this.GetExistingEdge(musicRun, this.runner.runningEastwards);
				}
			}
		}
	}

	// Token: 0x060003C7 RID: 967 RVA: 0x0001DA28 File Offset: 0x0001BC28
	public void AddTestChunk(MusicRun musicRun, Chunk chunkProto, bool east)
	{
		this.AddChunkWithProto(musicRun, chunkProto, chunkProto.validatedAuthoredDuration, east, default(Obstacles), -1, 0f, false);
	}

	// Token: 0x060003C8 RID: 968 RVA: 0x0001DA58 File Offset: 0x0001BC58
	public void AddTestChunkInMiddle(MusicRun musicRun, Chunk chunkProto, float creationPosNorm)
	{
		musicRun.range.Lerp(creationPosNorm);
		Vector3 vector = musicRun.FindPositionOnSurfaceNorm(creationPosNorm);
		this.AddChunkWithProtoInTrackMid(musicRun, chunkProto, vector);
	}

	// Token: 0x060003C9 RID: 969 RVA: 0x0001DA86 File Offset: 0x0001BC86
	public void RemoveTestChunk(MusicRun musicRun, bool east)
	{
		this.TryRemoveChunk(musicRun, east);
	}

	// Token: 0x060003CA RID: 970 RVA: 0x0001DA90 File Offset: 0x0001BC90
	private void AddChunkWithProtoInTrackMid(MusicRun musicRun, Chunk chunkProto, Vector3 creationPos)
	{
		Chunk chunk = chunkProto.prototype.Instantiate<Chunk>(null);
		chunk.transform.parent = musicRun.transform;
		chunk.SetupWithObstacles(Obstacles.empty, 4);
		chunk.instanceCreatedEastwards = true;
		Chunk.Connector connectorEast = chunk.GetConnectorEast(true);
		Vector3 vector = Vector3.Lerp(chunk.GetConnectorEast(false).point, connectorEast.point, 0.5f);
		this.MoveChunkToAlignWorldPoints(chunk, vector, creationPos);
		Alignment alignment = musicRun.slope.AlignmentForRange(chunk.range);
		chunk.AlignTo(alignment);
		Build.BuildAllUnder(chunk.transform);
		this.FinaliseAddNewChunk(musicRun, chunk, true);
	}

	// Token: 0x060003CB RID: 971 RVA: 0x0001DB2C File Offset: 0x0001BD2C
	private void GenerateOnMusicRun(MusicRun musicRun)
	{
		float x = this.runner.position.x;
		if (!this.createZone.Intersects(musicRun.range) && !musicRun.range.Contains(x))
		{
			musicRun.MarkAllChunksNonLive();
			return;
		}
		if (this.runner.caught)
		{
			musicRun.MarkAllChunksNonLive();
			return;
		}
		if (!this.runTrack.playingOrAboutTo)
		{
			musicRun.MarkAllChunksNonLive();
		}
		float num = musicRun.range.Clamp(x);
		if (!Range.Centered(musicRun.slope.SampleAt(num, false).point.y, this.settings.creationZoneHeight).Contains(this.runner.position.y))
		{
			return;
		}
		if (musicRun.chunks.Count <= 0)
		{
			if (musicRun.range.Contains(x))
			{
				Range range = musicRun.range;
				range.min += this.settings.startWithCentreIslandMargin;
				range.max -= this.settings.startWithCentreIslandMargin;
				if (range.Contains(x))
				{
					TrackBuilder.MatchParams matchParams = new TrackBuilder.MatchParams
					{
						obstacles = Obstacles.empty,
						beatCount = 4,
						eastwards = true,
						requiredAbsSlopeRange = new Range(-30f, 30f)
					};
					Chunk chunkProto = this.ChooseChunkPrototypeWithObstacles(matchParams).chosenOption.chunkProto;
					this.AddChunkWithProtoInTrackMid(musicRun, chunkProto, musicRun.slope.SampleAt(x, false).point3d);
				}
				else
				{
					bool flag = x - musicRun.range.min < musicRun.range.max - x;
					do
					{
						this.ChooseAndAddChunk(musicRun, flag);
						if (this.GetExistingEdge(musicRun, flag).chunk.range.Contains(x))
						{
							break;
						}
					}
					while (this.CanAddChunk(musicRun, flag));
				}
			}
			else
			{
				bool flag2 = x < musicRun.range.min;
				this.ChooseAndAddChunk(musicRun, flag2);
			}
		}
		float x2 = this.GetExistingEdge(musicRun, true).x;
		float x3 = this.GetExistingEdge(musicRun, false).x;
		if (this.CanAddChunk(musicRun, true) && this.createZone.Contains(x2))
		{
			if (this.GetExternalEdge(musicRun, true).x - x2 > this.settings.proximityToConnectIslandToTrack)
			{
				this.ChooseAndAddChunk(musicRun, true);
			}
			else
			{
				this.InsertFinalChunk(musicRun, true);
			}
		}
		if (this.CanAddChunk(musicRun, false) && this.createZone.Contains(x3))
		{
			float x4 = this.GetExternalEdge(musicRun, false).x;
			if (x3 - x4 > this.settings.proximityToConnectIslandToTrack)
			{
				this.ChooseAndAddChunk(musicRun, false);
				return;
			}
			this.InsertFinalChunk(musicRun, false);
		}
	}

	// Token: 0x060003CC RID: 972 RVA: 0x0001DDF4 File Offset: 0x0001BFF4
	private Alignment CreateAlignmentTowards(MusicRun musicRun, Range range, Vector2 startingPoint, bool east)
	{
		Alignment alignment = musicRun.slope.AlignmentForRange(range);
		Vector2 vector = alignment.vector;
		Vector2 vector2 = (east ? alignment.right : alignment.left);
		float num = Mathf.Abs(Mathf.Tan(0.017453292f * this.settings.maxAngleReturnToGradient) * vector.x);
		Range range2 = new Range(Mathf.Min(vector.y, -num), Mathf.Max(vector.y, num));
		float num2 = range2.Clamp(vector2.y - startingPoint.y);
		Vector2 vector3 = new Vector2(vector2.x, startingPoint.y + num2);
		if (east)
		{
			return new Alignment
			{
				left = startingPoint,
				right = vector3
			};
		}
		return new Alignment
		{
			left = vector3,
			right = startingPoint
		};
	}

	// Token: 0x060003CD RID: 973 RVA: 0x0001DED4 File Offset: 0x0001C0D4
	private void AddNonMusicalChunk(MusicRun musicRun, bool east, Range requiredAbsSlopeRange, bool empty)
	{
		Obstacles obstacles = Obstacles.empty;
		if (!empty)
		{
			float value = Random.value;
			if (value < 0.3f)
			{
				obstacles = new Obstacles("h_______");
			}
			else if (value < 0.6f)
			{
				obstacles = new Obstacles("h_h_____");
			}
			else if (value < 0.9f)
			{
				obstacles = new Obstacles("h___h___");
			}
			else
			{
				obstacles = Obstacles.empty;
			}
		}
		TrackBuilder.MatchParams matchParams = new TrackBuilder.MatchParams
		{
			obstacles = obstacles,
			beatCount = 4,
			eastwards = east,
			requiredAbsSlopeRange = requiredAbsSlopeRange
		};
		TrackBuilder.MatchDecision matchDecision = this.ChooseChunkPrototypeWithObstacles(matchParams);
		Chunk chunkProto = matchDecision.chosenOption.chunkProto;
		this.AddChunkWithProto(musicRun, chunkProto, chunkProto.validatedAuthoredDuration, east, default(Obstacles), -1, 0f, false).decision = matchDecision;
	}

	// Token: 0x060003CE RID: 974 RVA: 0x0001DFA4 File Offset: 0x0001C1A4
	private void ChooseAndAddChunk(MusicRun musicRun, bool east)
	{
		TrackPosition existingEdge = this.GetExistingEdge(musicRun, east);
		Vector2 vector;
		if (existingEdge.slope != null)
		{
			vector = existingEdge.slope.SampleAt(existingEdge.x, false).point;
		}
		else
		{
			vector = (east ? musicRun.slope.leftPoint : musicRun.slope.rightPoint);
		}
		float num = existingEdge.x + (east ? this.settings.maxChunkWidthRequirement : (-this.settings.maxChunkWidthRequirement));
		Range range = Range.Auto(existingEdge.x, num);
		float num2 = Slope.AngleWithVector(this.CreateAlignmentTowards(musicRun, range, vector, east).vector, false, musicRun);
		Range range2 = new Range(num2, num2);
		if (!this.runTrack.playingOrAboutTo || east != this.runner.runningEastwards || musicRun != this.runner.currentMusicRun)
		{
			this.AddNonMusicalChunk(musicRun, east, range2, true);
			return;
		}
		Simulate.FindOptions standardPredict = Simulate.FindOptions.standardPredict;
		standardPredict.SetDirectionEastwards(east);
		Simulate.FindResult findResult = Simulate.FindTimeFromTo(this.runner.lastRunPos, existingEdge, this.runner.direction, standardPredict, this.runner.settings);
		if (!findResult.foundRequestedX)
		{
			standardPredict.traceCallback = delegate(Simulate.TraceState state)
			{
				Debug.DrawLine(state.point, state.point + 3f * Vector3.up, Color.red, 20f);
			};
			findResult = Simulate.FindTimeFromTo(this.runner.lastRunPos, existingEdge, this.runner.direction, standardPredict, this.runner.settings);
			Debug.DrawLine(vector + 0.5f * Vector2.right, vector + 3f * Vector2.up + 0.5f * Vector2.right, Color.blue, 20f);
			Debug.DrawLine(findResult.sample.point + 0.75f * Vector2.right, findResult.sample.point + 3f * Vector2.up + 0.75f * Vector2.right, Color.magenta, 20f);
			Debug.LogError(string.Format("Couldn't reach edge of created space? Dist from target={0} What's the circumstance?", findResult.distFromRequestedX));
			if (DebugOptions.opts.pauseWhenEdgeOfSpaceNotFound)
			{
				Debug.Break();
			}
			return;
		}
		float num3 = this.runner.lastRunSlopeTime + findResult.duration;
		if (num3 >= this.runTrack.track.lastBeatEndTime)
		{
			this.AddNonMusicalChunk(musicRun, east, range2, true);
			return;
		}
		ValueTuple<int, float> valueTuple = this.runTrack.track.FindNearestBarStart(num3);
		int item = valueTuple.Item1;
		float item2 = valueTuple.Item2;
		BeatTrack.Bar bar = this.runTrack.track.bars[item];
		float num4 = num3 - item2;
		if (!this.settings.allowLargeResyncChunks || num4 <= this.settings.largeResyncMinError)
		{
			TrackBuilder.MatchParams matchParams = new TrackBuilder.MatchParams
			{
				obstacles = bar.obstacles,
				beatCount = this.runTrack.track.BeatCountInBar(item),
				eastwards = east,
				requiredAbsSlopeRange = range2
			};
			bool flag = (Level.currentIndex == 0 && !Game.instance.newGamePlus) || PlayerPrefsX.GetInt("MusicRunningEasyMode", 0) == 1;
			matchParams.obstacles = TrackBuilder.ObstaclesAfterChoosingEasyMode(flag, matchParams.obstacles, matchParams.beatCount).Item2;
			float num5 = 1f;
			float num6 = 0f;
			TrackBuilder.ChosenSwitchback chosenSwitchback = default(TrackBuilder.ChosenSwitchback);
			bool flag2 = false;
			if (!DebugOptions.opts.disableSwitchbacks)
			{
				chosenSwitchback = this.ChooseSwitchbackIfAny(east, item);
				num5 = chosenSwitchback.barDurationScalar;
				num6 = chosenSwitchback.extensionDuration;
				if (chosenSwitchback.chunk != null)
				{
					flag2 = true;
				}
			}
			TrackBuilder.MatchDecision matchDecision = default(TrackBuilder.MatchDecision);
			if (chosenSwitchback.chunk == null)
			{
				matchDecision = this.ChooseChunkPrototypeWithObstacles(matchParams);
			}
			else
			{
				matchDecision.chosenOption.chunkProto = chosenSwitchback.chunk;
			}
			if (matchDecision.chosenOption.isFallback)
			{
				Debug.Log(string.Format("Chunk needs to be authored for pattern: {0}-beat {1} / angle range {2} in bar idx {3} of {4}", new object[]
				{
					matchParams.beatCount,
					matchParams.obstacles.ToString(matchParams.beatCount * 2),
					matchParams.requiredAbsSlopeRange,
					item,
					this.runTrack.track.name
				}));
			}
			if (num4 < -0.1f)
			{
				Chunk chunk = this.ChooseSpacerChunkPrototype(-num4);
				this.AddChunkWithProto(musicRun, chunk, -num4, east, default(Obstacles), -1, 0f, false).liveForMusic = true;
				num4 = 0f;
			}
			if (!east)
			{
				Chunk westChunk = musicRun.westChunk;
			}
			else
			{
				Chunk eastChunk = musicRun.eastChunk;
			}
			Chunk chunk2 = this.AddChunkWithProto(musicRun, matchDecision.chosenOption.chunkProto, num5 * this.runTrack.track.BarDuration(item), east, matchDecision.chosenOption.matchParams.obstacles, matchParams.beatCount, flag2 ? 0f : num4, matchDecision.chosenOption.chunkProto.departsMusicRun);
			chunk2.liveForMusic = true;
			this.PlaceObstacleMarkers(chunk2, item);
			chunk2.decision = matchDecision;
			chunk2.decision.barIdx = item;
			chunk2.decision.track = this.runTrack.track;
			if (num6 > 0f)
			{
				Chunk chunk3 = this.ChooseSpacerChunkPrototype(num6);
				chunk2 = this.AddChunkWithProto(musicRun, chunk3, num6, east, default(Obstacles), -1, 0f, true);
				chunk2.liveForMusic = true;
			}
			return;
		}
		float num7 = this.runTrack.track.BarDuration(item);
		if (num4 > num7)
		{
			Debug.LogError(string.Format("Extremely large time error of {0} - surely FindNearestBarStart should've detected an entirely different bar?", num4));
			return;
		}
		float num8 = num7 - num4;
		Chunk chunk4 = this.ChooseSpacerChunkPrototype(num8);
		Chunk chunk5 = this.AddChunkWithProto(musicRun, chunk4, num8, east, default(Obstacles), -1, 0f, false);
		chunk5.liveForMusic = true;
		Debug.Log(string.Format("Significant time error of {0}. Recalibrating using spacer chunk with duration {1}", num4, num8), chunk5);
	}

	// Token: 0x060003CF RID: 975 RVA: 0x0001E5F8 File Offset: 0x0001C7F8
	private void InsertFinalChunk(MusicRun musicRun, bool east)
	{
		TrackPosition existingEdge = this.GetExistingEdge(musicRun, east);
		TrackPosition externalEdge = this.GetExternalEdge(musicRun, east);
		Range range = Range.Auto(existingEdge.x, externalEdge.x);
		float num = range.length * Simulate.SignedSpeedOnGround(1f, 0f, false, this.runner.settings.run);
		Chunk chunk = this.ChooseSpacerChunkPrototype(num).prototype.Instantiate<Chunk>(null);
		chunk.transform.parent = musicRun.transform;
		chunk.instanceCreatedEastwards = east;
		Vector3 vector = (east ? existingEdge.slope.rightPoint : existingEdge.slope.leftPoint);
		this.MoveChunkToAlignWorldPoints(chunk, chunk.GetConnectorEast(!east).point, vector);
		Alignment alignment = this.CreateAlignmentTowards(musicRun, range, vector, east);
		chunk.AlignTo(alignment);
		Build.BuildAllUnder(chunk.transform);
		if (east)
		{
			this.Connect(existingEdge.chunk, chunk);
		}
		else
		{
			this.Connect(chunk, existingEdge.chunk);
		}
		if (externalEdge.slope != null)
		{
			this.JoinToTrack(musicRun, chunk, east);
		}
		else if (east)
		{
			musicRun.connectedToTrackEast = true;
		}
		else
		{
			musicRun.connectedToTrackWest = true;
		}
		this.FinaliseAddNewChunk(musicRun, chunk, east);
	}

	// Token: 0x060003D0 RID: 976 RVA: 0x0001E73C File Offset: 0x0001C93C
	private TrackBuilder.ChosenSwitchback ChooseSwitchbackIfAny(bool east, int barIdx)
	{
		BeatTrack track = this.runTrack.track;
		TrackBuilder.ChosenSwitchback chosenSwitchback = new TrackBuilder.ChosenSwitchback
		{
			chunk = null,
			barDurationScalar = 1f,
			extensionDuration = 0f
		};
		if (this._activeSwitchbacksStartBarIdx == -1)
		{
			if (barIdx < track.bars.Length - 1 && track.bars[barIdx + 1].hasSwitchback)
			{
				this._activeSwitchbacksStartBarIdx = barIdx;
				float num = (float)track.BeatCountInBar(barIdx);
				chosenSwitchback.barDurationScalar = (num - 0.5f) / num;
			}
			return chosenSwitchback;
		}
		if (!track.bars[barIdx].hasSwitchback)
		{
			this._activeSwitchbacksStartBarIdx = -1;
			return chosenSwitchback;
		}
		if (track.bars[barIdx].switchbackLegBeats == 4)
		{
			bool flag = (this._activeSwitchbacksStartBarIdx - barIdx) % 2 == 0;
			chosenSwitchback.chunk = ((east ^ flag) ? this.switchbackLeftPrototype : this.switchbackRightPrototype);
		}
		else
		{
			chosenSwitchback.chunk = (east ? this.switchbackDoubleRightPrototype : this.switchbackDoubleLeftPrototype);
		}
		if (barIdx + 1 >= track.bars.Length || !track.bars[barIdx + 1].hasSwitchback)
		{
			BeatTrack.Bar bar = track.bars[barIdx];
			float num2 = track.BeatDuration(bar.firstBeatIdx);
			chosenSwitchback.extensionDuration = 0.5f * num2;
		}
		return chosenSwitchback;
	}

	// Token: 0x060003D1 RID: 977 RVA: 0x0001E894 File Offset: 0x0001CA94
	private void AborbErrorBetweenChunks(Chunk leftChunk, Chunk rightChunk, float timeError, bool rightwards)
	{
		if (leftChunk.rightSlope.numberOfPoints < 2)
		{
			Debug.LogError("Expected right margin of chunk to have at least 2 vertices", leftChunk);
			return;
		}
		if (rightChunk.leftSlope.numberOfPoints < 2)
		{
			Debug.LogError("Expected left margin of chunk to have at least 2 vertices", rightChunk);
			return;
		}
		float num = (float)(rightwards ? 1 : (-1));
		float num2 = Simulate.TimeSlopeEdge(leftChunk.rightSlope, rightwards, num, this.runner.settings.run);
		float num3 = Simulate.TimeSlopeEdge(rightChunk.leftSlope, !rightwards, num, this.runner.settings.run);
		float num4 = Mathf.Max(num2 - 0.05f, 0f);
		float num5 = Mathf.Max(num3 - 0.05f, 0f);
		if (timeError > num4 + num5)
		{
			Debug.LogWarning(string.Format("Too much time error to absorb: {0}s, for margins of {1} and {2}. Click for left chunk.", timeError, num2, num3), leftChunk);
			timeError = num4 + num5;
		}
		float num6 = timeError / 2f;
		float num7 = timeError / 2f;
		if (num6 > num4)
		{
			num6 = num4;
			num7 = timeError - num6;
		}
		else if (num7 > num5)
		{
			num7 = num5;
			num6 = timeError - num7;
		}
		float num8 = (num2 - num6) / num2;
		float num9 = (num3 - num7) / num3;
		Vector2 vector = leftChunk.rightSlope.localPoints[1] - leftChunk.rightSlope.localPoints[0];
		leftChunk.rightSlope.localPoints[1] = leftChunk.rightSlope.localPoints[0] + num8 * vector;
		leftChunk.rightSlope.ReCalculateWorldPoints();
		Vector2 vector2 = rightChunk.leftSlope.localPoints[1] - rightChunk.leftSlope.localPoints[0];
		rightChunk.leftSlope.localPoints[0] = rightChunk.leftSlope.localPoints[1] - num9 * vector2;
		rightChunk.leftSlope.ReCalculateWorldPoints();
		if (rightwards)
		{
			this.MoveChunkToAlignWorldPoints(rightChunk, rightChunk.leftSlope.leftPoint, leftChunk.rightSlope.rightPoint);
			return;
		}
		this.MoveChunkToAlignWorldPoints(leftChunk, leftChunk.rightSlope.rightPoint, rightChunk.leftSlope.leftPoint);
	}

	// Token: 0x060003D2 RID: 978 RVA: 0x0001EAC0 File Offset: 0x0001CCC0
	private void MoveChunkToAlignWorldPoints(Chunk chunk, Vector3 worldPointOnChunk, Vector3 toTargetWorldPoint)
	{
		Vector3 vector = toTargetWorldPoint - worldPointOnChunk;
		chunk.transform.position += vector;
		foreach (Slope slope in chunk.slopes)
		{
			slope.transform.position += vector;
			slope.ReCalculateWorldPoints();
		}
	}

	// Token: 0x060003D3 RID: 979 RVA: 0x0001EB48 File Offset: 0x0001CD48
	private Chunk AddChunkWithProto(MusicRun musicRun, Chunk chunkProto, float duration, bool east, Obstacles obstacles = default(Obstacles), int beatCount = -1, float timeError = 0f, bool departsMusicRun = false)
	{
		Chunk chunk = chunkProto.prototype.Instantiate<Chunk>(null);
		chunk.transform.parent = musicRun.transform;
		chunk.SetupWithObstacles(obstacles, beatCount);
		Chunk chunk2;
		Vector3 vector;
		if (musicRun.chunks.Count == 0)
		{
			chunk2 = null;
			if (east)
			{
				if (musicRun.westSlopeToJoin != null)
				{
					vector = musicRun.westSlopeToJoin.rightPoint;
				}
				else
				{
					vector = musicRun.slope.leftPoint;
				}
			}
			else if (musicRun.eastSlopeToJoin != null)
			{
				vector = musicRun.eastSlopeToJoin.leftPoint;
			}
			else
			{
				vector = musicRun.slope.rightPoint;
			}
		}
		else
		{
			chunk2 = (east ? musicRun.eastChunk : musicRun.westChunk);
			vector = chunk2.GetConnectorEast(east).point;
		}
		chunk.instanceCreatedEastwards = east;
		this.MoveChunkToAlignWorldPoints(chunk, chunk.GetConnector(true).point, vector);
		float validatedAuthoredDuration = chunkProto.validatedAuthoredDuration;
		float num = duration / validatedAuthoredDuration;
		Range range = Range.zero;
		if (!departsMusicRun)
		{
			Vector3 position = chunk.transform.position;
			float z = musicRun.transform.position.z;
			position.z = Mathf.Clamp(z, position.z - 1f, position.z + 1f);
			chunk.transform.position = position;
			float num2 = (chunkProto.range * num).length;
			if (!east)
			{
				num2 = -num2;
			}
			range = Range.Auto(vector.x, vector.x + num2);
			Alignment alignment = this.CreateAlignmentTowards(musicRun, range, vector, east);
			chunk.AlignTo(alignment);
		}
		Build.BuildAllUnder(chunk.transform);
		if (chunk.leftSlope == null)
		{
			Debug.Log("Chunk " + chunk.name + " has no left slope, iteration may fail", chunk);
		}
		if (chunk.rightSlope == null)
		{
			Debug.Log("Chunk " + chunk.name + " has no right slope, iteration may fail", chunk);
		}
		Slope prevSlope = null;
		Simulate.FindResult findResult = Simulate.IterateForwardOverChunk(chunk, delegate(Simulate.TraceState state)
		{
			if (prevSlope != state.slope)
			{
				if (prevSlope != null)
				{
					if (east)
					{
						state.slope.nextWestSlope = prevSlope;
						prevSlope.nextEastSlope = state.slope;
					}
					else
					{
						state.slope.nextEastSlope = prevSlope;
						prevSlope.nextWestSlope = state.slope;
					}
				}
				prevSlope = state.slope;
				prevSlope.reverseFlow = state.rightwards != east;
			}
		}, this.runner.settings);
		if (findResult.foundRequestedX && Mathf.Abs(findResult.duration - duration) > 0.01f)
		{
			float num3 = duration / findResult.duration;
			if (!departsMusicRun)
			{
				Range range2 = (range - vector.x) * num3 + vector.x;
				Alignment alignment2 = this.CreateAlignmentTowards(musicRun, range2, vector, east);
				chunk.AlignTo(alignment2);
			}
			else
			{
				chunk.RealignUsingScale(vector, num3);
			}
			Build.BuildAllUnder(chunk.transform);
		}
		this.MoveChunkToAlignWorldPoints(chunk, chunk.GetConnector(true).point, vector);
		if (chunk2 == null)
		{
			this.JoinToTrack(musicRun, chunk, !east);
		}
		else
		{
			if (east)
			{
				this.Connect(chunk2, chunk);
			}
			else
			{
				this.Connect(chunk, chunk2);
			}
			if (timeError != 0f && !chunk.dontAbsorbErrorInMargins && !DebugOptions.opts.dontAbsorbErrorInMargins)
			{
				if (east)
				{
					this.AborbErrorBetweenChunks(chunk2, chunk, timeError, east);
				}
				else
				{
					this.AborbErrorBetweenChunks(chunk, chunk2, timeError, east);
				}
			}
		}
		this.FinaliseAddNewChunk(musicRun, chunk, east);
		if (musicRun.musicRunFlocks != null)
		{
			foreach (MusicRunFlock musicRunFlock in musicRun.musicRunFlocks)
			{
				if (!musicRunFlock.spawned && chunk.range.Contains(musicRunFlock.transform.position.x))
				{
					musicRunFlock.Spawn(chunk, (float)(east ? 1 : (-1)));
				}
			}
		}
		return chunk;
	}

	// Token: 0x060003D4 RID: 980 RVA: 0x0001EF1C File Offset: 0x0001D11C
	private void JoinToTrack(MusicRun musicRun, Chunk chunk, bool eastSideOfMusicRun)
	{
		Slope slope = (eastSideOfMusicRun ? musicRun.eastSlopeToJoin : musicRun.westSlopeToJoin);
		Chunk.Connector connectorEast = chunk.GetConnectorEast(eastSideOfMusicRun);
		if (eastSideOfMusicRun)
		{
			musicRun.connectedToTrackEast = true;
		}
		else
		{
			musicRun.connectedToTrackWest = true;
		}
		if (slope == null)
		{
			return;
		}
		List<Slope> list = (eastSideOfMusicRun ? connectorEast.slope.rightSlopes : connectorEast.slope.leftSlopes);
		List<Slope> list2 = (eastSideOfMusicRun ? slope.leftSlopes : slope.rightSlopes);
		if (list.Count > 0)
		{
			string text = (eastSideOfMusicRun ? "left" : "right");
			Debug.LogError(string.Format("Expected chunk '{0}' to have a simple connection on {1} side without any existing slopes to connect to external track, but it already had {2}. Click for slope.", chunk.name, text, list.Count), connectorEast.slope);
		}
		if (list2.Count > 0)
		{
			string text2 = (eastSideOfMusicRun ? "east" : "west");
			Debug.LogError(string.Format("Expected {0} side of track to have no existing slopes when joining chunk but it already had {1}. Click for slope.", text2, list2.Count), slope);
		}
		list.Add(slope);
		list2.Add(connectorEast.slope);
		connectorEast.slope.SetPointIdx(eastSideOfMusicRun ? (connectorEast.slope.numberOfPoints - 1) : 0, slope.PointIdx(eastSideOfMusicRun ? 0 : (slope.numberOfPoints - 1)));
	}

	// Token: 0x060003D5 RID: 981 RVA: 0x0001F04C File Offset: 0x0001D24C
	private void FinaliseAddNewChunk(MusicRun musicRun, Chunk chunk, bool east)
	{
		Level.current.slopes.AddDynamic(chunk.slopes);
		Level.current.polys.AddDynamic(chunk.polys);
		Level.current.trips.AddDynamic(chunk.trips);
		Level.current.balancePoints.AddDynamic(chunk.balancePoints);
		chunk.musicRun = musicRun;
		if (east)
		{
			musicRun.chunks.Add(chunk);
		}
		else
		{
			musicRun.chunks.Insert(0, chunk);
		}
		chunk.instantiatedTime = Time.time;
		if (chunk.floorPoly != null)
		{
			chunk.floorPoly.useDebugTexture = musicRun.poly.useDebugTexture;
			if (!musicRun.poly.useDebugTexture)
			{
				chunk.floorPoly.useDebugTexture = false;
				chunk.floorPoly.baseTexture = musicRun.poly.baseTexture;
				chunk.floorPoly.textureScale = musicRun.poly.textureScale;
				chunk.floorPoly.textureOffset = musicRun.poly.textureOffset;
			}
			else
			{
				chunk.floorPoly.baseTexture = null;
			}
			chunk.floorPoly.color = musicRun.poly.color;
		}
		musicRun.RefreshPolyAroundChunks();
	}

	// Token: 0x060003D6 RID: 982 RVA: 0x0001F18C File Offset: 0x0001D38C
	private void TryRemoveChunk(MusicRun musicRun, bool east)
	{
		if (musicRun.chunks.Count == 0)
		{
			return;
		}
		Chunk chunk = (east ? musicRun.eastChunk : musicRun.westChunk);
		if (Time.time - chunk.instantiatedTime < this.settings.minimumChunkLife)
		{
			return;
		}
		musicRun.chunks.RemoveAt(east ? (musicRun.chunks.Count - 1) : 0);
		if ((east || musicRun.chunks.Count == 0) && musicRun.connectedToTrackEast)
		{
			musicRun.connectedToTrackEast = false;
			if (musicRun.eastSlopeToJoin != null)
			{
				this.DisconnectSimple(chunk.GetConnectorEast(true).slope, musicRun.eastSlopeToJoin);
			}
		}
		if ((!east || musicRun.chunks.Count == 0) && musicRun.connectedToTrackWest)
		{
			musicRun.connectedToTrackWest = false;
			if (musicRun.westSlopeToJoin != null)
			{
				this.DisconnectSimple(musicRun.westSlopeToJoin, chunk.GetConnectorEast(false).slope);
			}
		}
		if (musicRun.chunks.Count > 0)
		{
			if (east)
			{
				this.Disconnect(musicRun.eastChunk, chunk);
			}
			else
			{
				this.Disconnect(chunk, musicRun.westChunk);
			}
		}
		Level.current.slopes.RemoveDynamic(chunk.slopes);
		Level.current.polys.RemoveDynamic(chunk.polys);
		Level.current.trips.RemoveDynamic(chunk.trips);
		Level.current.balancePoints.RemoveDynamic(chunk.balancePoints);
		chunk.prototype.ReturnToPool();
		chunk.transform.parent = base.transform;
		musicRun.RefreshPolyAroundChunks();
	}

	// Token: 0x060003D7 RID: 983 RVA: 0x0001F31E File Offset: 0x0001D51E
	private void ConnectSimple(Slope slopeLeft, Slope slopeRight)
	{
		slopeLeft.rightSlopes.Add(slopeRight);
		slopeRight.leftSlopes.Add(slopeLeft);
	}

	// Token: 0x060003D8 RID: 984 RVA: 0x0001F338 File Offset: 0x0001D538
	private void DisconnectSimple(Slope slopeLeft, Slope slopeRight)
	{
		slopeLeft.rightSlopes.Remove(slopeRight);
		slopeRight.leftSlopes.Remove(slopeLeft);
	}

	// Token: 0x060003D9 RID: 985 RVA: 0x0001F354 File Offset: 0x0001D554
	private void Connect(Chunk westChunk, Chunk eastChunk)
	{
		Chunk.Connector connectorEast = westChunk.GetConnectorEast(true);
		Chunk.Connector connectorEast2 = eastChunk.GetConnectorEast(false);
		connectorEast2.slope.nextWestSlope = connectorEast.slope;
		connectorEast.slope.nextEastSlope = connectorEast2.slope;
		TrackBuilder._sharedHub.Clear();
		TrackBuilder._sharedHub[connectorEast.slope] = connectorEast.isOnLeft;
		TrackBuilder._sharedHub[connectorEast2.slope] = connectorEast2.isOnLeft;
		for (int i = 0; i < 2; i++)
		{
			Chunk.Connector connector = ((i == 0) ? connectorEast : connectorEast2);
			foreach (Slope slope in (connector.isOnLeft ? connector.slope.leftSlopes : connector.slope.rightSlopes))
			{
				if (!TrackBuilder._sharedHub.ContainsKey(slope))
				{
					if (slope.leftSlopes.Contains(connector.slope))
					{
						TrackBuilder._sharedHub[slope] = true;
					}
					else
					{
						TrackBuilder._sharedHub[slope] = false;
					}
				}
			}
		}
		Vector3 vector = Vector3.zero;
		foreach (KeyValuePair<Slope, bool> keyValuePair in TrackBuilder._sharedHub)
		{
			Slope key = keyValuePair.Key;
			bool value = keyValuePair.Value;
			vector += (value ? key.leftPoint : key.rightPoint);
		}
		vector /= (float)TrackBuilder._sharedHub.Count;
		foreach (KeyValuePair<Slope, bool> keyValuePair2 in TrackBuilder._sharedHub)
		{
			Slope key2 = keyValuePair2.Key;
			bool value2 = keyValuePair2.Value;
			if (value2)
			{
				key2.leftPoint = vector;
			}
			else
			{
				key2.rightPoint = vector;
			}
			List<Slope> list = (value2 ? key2.leftSlopes : key2.rightSlopes);
			foreach (Slope slope2 in TrackBuilder._sharedHub.Keys)
			{
				if (key2 != slope2 && !list.Contains(slope2))
				{
					list.Add(slope2);
				}
			}
		}
		TrackBuilder._sharedHub.Clear();
	}

	// Token: 0x060003DA RID: 986 RVA: 0x0001F5E8 File Offset: 0x0001D7E8
	private void Disconnect(Chunk westChunk, Chunk eastChunk)
	{
		Chunk.Connector connectorEast = westChunk.GetConnectorEast(true);
		Chunk.Connector connectorEast2 = eastChunk.GetConnectorEast(false);
		List<Slope> list = (connectorEast.isOnLeft ? connectorEast.slope.leftSlopes : connectorEast.slope.rightSlopes);
		List<Slope> list2 = (connectorEast2.isOnLeft ? connectorEast2.slope.leftSlopes : connectorEast2.slope.rightSlopes);
		list.Remove(connectorEast2.slope);
		list2.Remove(connectorEast.slope);
		foreach (Slope slope in list)
		{
			slope.leftSlopes.Remove(connectorEast2.slope);
			slope.rightSlopes.Remove(connectorEast2.slope);
		}
		foreach (Slope slope2 in list2)
		{
			slope2.leftSlopes.Remove(connectorEast.slope);
			slope2.rightSlopes.Remove(connectorEast.slope);
		}
	}

	// Token: 0x060003DB RID: 987 RVA: 0x0001F718 File Offset: 0x0001D918
	private void PlaceObstacleMarkers(Chunk newMusicChunk, int barIdx)
	{
		BeatTrack.Bar bar = this.runTrack.track.bars[barIdx];
		foreach (ObstaclePlacement obstaclePlacement in newMusicChunk.obstaclePlacements)
		{
			int num = bar.firstBeatIdx + obstaclePlacement.halfBeatIdx / 2;
			float num2 = this.runTrack.track.beats[num].time;
			if ((obstaclePlacement.halfBeatIdx & 1) == 1)
			{
				num2 += 0.5f * this.runTrack.track.BeatDuration(num);
			}
			BeatTrack.ObstacleRef obstacleRef = new BeatTrack.ObstacleRef
			{
				barIdx = barIdx,
				halfBeatIdx = obstaclePlacement.halfBeatIdx,
				special = (TrackBuilder.specialJumpsAvailable && bar.obstacles.Has(obstaclePlacement.halfBeatIdx, ObstacleType.Special)),
				time = num2
			};
			Vector3 position = obstaclePlacement.transform.position;
			RhythmActionMarker rhythmActionMarker = this.obstacleMarkerPrototype.Instantiate<RhythmActionMarker>(null);
			Vector3 vector = 0.8f * Vector3.down;
			Poly poly = Raycast.AnyPolyOccludes(position + vector, new Range(position.z - 2f, position.z), null, null);
			if (poly != null)
			{
				position.z = poly.transform.position.z - 0.1f;
			}
			rhythmActionMarker.Setup(position, obstacleRef);
			this._markers.Add(rhythmActionMarker);
			if (rhythmActionMarker.isSpecialJump || (GameInput.activeInputType == GameInput.InputType.Gamepad && TrackBuilder.specialJumpsAvailable))
			{
				RhythmActionMarkerPrompt rhythmActionMarkerPrompt = MonoSingleton<GameUI>.instance.rhythmActionMarkerPromptPrototype.Instantiate<RhythmActionMarkerPrompt>(null);
				rhythmActionMarkerPrompt.Setup(rhythmActionMarker.isSpecialJump);
				rhythmActionMarker.buttonPrompt = rhythmActionMarkerPrompt;
				rhythmActionMarkerPrompt.marker = rhythmActionMarker;
				this._prompts.Add(rhythmActionMarkerPrompt);
			}
		}
	}

	// Token: 0x060003DC RID: 988 RVA: 0x0001F8F4 File Offset: 0x0001DAF4
	private void UpdateObstacleMarkers()
	{
		if (this.runner == null || this.runTrack == null)
		{
			return;
		}
		if ((this.runTrack.state != RunTrack.State.Playing && this.runTrack.state != RunTrack.State.RampingUp && this.runTrack.state != RunTrack.State.Scheduled) || !this.runner.lastRunPos.isValid || float.IsNegativeInfinity(this.runner.lastRunSlopeTime))
		{
			foreach (RhythmActionMarker rhythmActionMarker in this._markers)
			{
				rhythmActionMarker.Hide(RhythmActionMarker.HideReason.NotMusicRunning, false, false, 0f);
			}
			this._markers.Clear();
			return;
		}
		this._markers.UpdateAndRemoveIf(delegate(RhythmActionMarker marker)
		{
			if (this.runTrack.currentMusicTime - 0.5f > marker.obstacleRef.time)
			{
				marker.Hide(RhythmActionMarker.HideReason.Passed, false, false, 0f);
				return true;
			}
			return false;
		});
		if (this.settings.removeAllMarkersWhenOutOfSync && Runner.instance.running && this._markers.Count > 0)
		{
			RhythmActionMarker rhythmActionMarker2 = null;
			foreach (RhythmActionMarker rhythmActionMarker3 in this._markers)
			{
				if (rhythmActionMarker2 == null || rhythmActionMarker3.obstacleRef.time < rhythmActionMarker2.obstacleRef.time)
				{
					rhythmActionMarker2 = rhythmActionMarker3;
				}
			}
			if (rhythmActionMarker2 != null)
			{
				Simulate.FindResult findResult = Simulate.FindTimeFromTo(this.runner.trackPosition, new TrackPosition
				{
					x = rhythmActionMarker2.targetPos.x
				}, this.runner.direction, Simulate.FindOptions.standardPredict, Runner.instance.settings);
				if (findResult.foundRequestedX)
				{
					float num = this.runTrack.currentMusicTime + findResult.duration;
					float num2 = rhythmActionMarker2.obstacleRef.time - num;
					if (Mathf.Abs(num2) > Time.deltaTime + this.settings.removeAllMarkersErrorThreshold)
					{
						Debug.LogError("Music went significantly out of sync with markers. Removing them all to recalibrate." + num2.ToString());
						foreach (RhythmActionMarker rhythmActionMarker4 in this._markers)
						{
							rhythmActionMarker4.Hide(RhythmActionMarker.HideReason.OutOfSync, false, false, 0f);
						}
						this._markers.Clear();
					}
				}
			}
		}
	}

	// Token: 0x060003DD RID: 989 RVA: 0x0001FB80 File Offset: 0x0001DD80
	private void OnRunnerJumpedMusicRunObstacle(BeatTrack.ObstacleRef obstacle, bool success, bool nailedIt, float timeToObs)
	{
		int num = this._markers.FindIndex((RhythmActionMarker marker) => marker.obstacleRef.IsSameTimeAs(obstacle));
		if (num != -1)
		{
			this._markers[num].Hide(RhythmActionMarker.HideReason.Jump, success, nailedIt, timeToObs);
			this._markers.RemoveAt(num);
		}
	}

	// Token: 0x060003DE RID: 990 RVA: 0x0001FBD8 File Offset: 0x0001DDD8
	private void OnUIPositionUpdate(GameUI ui)
	{
		this._prompts.UpdateAndRemoveIf(delegate(RhythmActionMarkerPrompt prompt)
		{
			if (prompt.marker != null)
			{
				prompt.worldPos = prompt.marker.transform.position + this.settings.promptOffsetWorldY * Vector3.up;
			}
			else
			{
				prompt.visibility -= 3f * Time.deltaTime;
				if (prompt.visibility <= 0f)
				{
					prompt.visibility = 1f;
					prompt.layout.groupAlpha = this.settings.promptAlpha;
					prompt.prototype.ReturnToPool();
					return true;
				}
			}
			prompt.layout.groupAlpha = prompt.visibility * this.settings.promptAlpha;
			prompt.layout.origin = ui.WorldToCanvas(prompt.worldPos, default(Vector2));
			return false;
		});
	}

	// Token: 0x040004F3 RID: 1267
	[SerializeField]
	private float[] _fallbackScoreWeightScalars = new float[] { 0f, 0.5f, 0.6f, 0.05f, 0.001f };

	// Token: 0x040004F4 RID: 1268
	public const string musicRunningEasyModePrefName = "MusicRunningEasyMode";

	// Token: 0x040004F5 RID: 1269
	public TrackBuilderSettings settings = Presume<TrackBuilderSettings>.NonNull("C:\\Users\\josep\\Desktop\\highland-song\\Highland Run\\Assets\\Scripts\\MusicRunning\\TrackBuilder.cs", 44);

	// Token: 0x040004F6 RID: 1270
	public Transform library = Presume<Transform>.NonNull("C:\\Users\\josep\\Desktop\\highland-song\\Highland Run\\Assets\\Scripts\\MusicRunning\\TrackBuilder.cs", 45);

	// Token: 0x040004F7 RID: 1271
	public Chunk[] spacerChunkProtos = new Chunk[0];

	// Token: 0x040004F8 RID: 1272
	public Prototype obstacleMarkerPrototype = Presume<Prototype>.NonNull("C:\\Users\\josep\\Desktop\\highland-song\\Highland Run\\Assets\\Scripts\\MusicRunning\\TrackBuilder.cs", 48);

	// Token: 0x040004F9 RID: 1273
	[Header("Switchbacks")]
	public Chunk switchbackLeftPrototype = Presume<Chunk>.NonNull("C:\\Users\\josep\\Desktop\\highland-song\\Highland Run\\Assets\\Scripts\\MusicRunning\\TrackBuilder.cs", 52);

	// Token: 0x040004FA RID: 1274
	public Chunk switchbackRightPrototype = Presume<Chunk>.NonNull("C:\\Users\\josep\\Desktop\\highland-song\\Highland Run\\Assets\\Scripts\\MusicRunning\\TrackBuilder.cs", 53);

	// Token: 0x040004FB RID: 1275
	public Chunk switchbackDoubleRightPrototype = Presume<Chunk>.NonNull("C:\\Users\\josep\\Desktop\\highland-song\\Highland Run\\Assets\\Scripts\\MusicRunning\\TrackBuilder.cs", 54);

	// Token: 0x040004FC RID: 1276
	public Chunk switchbackDoubleLeftPrototype = Presume<Chunk>.NonNull("C:\\Users\\josep\\Desktop\\highland-song\\Highland Run\\Assets\\Scripts\\MusicRunning\\TrackBuilder.cs", 55);

	// Token: 0x040004FD RID: 1277
	[Nullable(2)]
	public Chunk debugForceChunk;

	// Token: 0x040004FE RID: 1278
	public int sleepTestMillisecs = 250;

	// Token: 0x040004FF RID: 1279
	private static Dictionary<Slope, bool> _sharedHub = new Dictionary<Slope, bool>();

	// Token: 0x04000500 RID: 1280
	private List<Chunk> _chunkPrototypes = new List<Chunk>(128);

	// Token: 0x04000501 RID: 1281
	private int _activeSwitchbacksStartBarIdx = -1;

	// Token: 0x04000502 RID: 1282
	private List<RhythmActionMarker> _markers = new List<RhythmActionMarker>(64);

	// Token: 0x04000503 RID: 1283
	private List<RhythmActionMarkerPrompt> _prompts = new List<RhythmActionMarkerPrompt>(64);

	// Token: 0x04000504 RID: 1284
	private static List<Vector2> _vertExtractionListScratch = new List<Vector2>(64);

	// Token: 0x04000505 RID: 1285
	private static List<TrackBuilder.MatchOption> _matchingOptionsScratch = new List<TrackBuilder.MatchOption>(128);

	// Token: 0x04000506 RID: 1286
	private static List<TrackBuilder.MatchOption> _matchingFallbackOptionsScratch = new List<TrackBuilder.MatchOption>(128);

	// Token: 0x04000507 RID: 1287
	private static List<Vector2> guidePolyVerticesScratch = new List<Vector2>(1024);

	// Token: 0x0200029C RID: 668
	[NullableContext(0)]
	public enum MatchRejectReason
	{
		// Token: 0x0400154B RID: 5451
		None,
		// Token: 0x0400154C RID: 5452
		NeverPatternMatch,
		// Token: 0x0400154D RID: 5453
		Direction,
		// Token: 0x0400154E RID: 5454
		Obstacles,
		// Token: 0x0400154F RID: 5455
		AngleRange,
		// Token: 0x04001550 RID: 5456
		DebugFallbackOnly
	}

	// Token: 0x0200029D RID: 669
	[NullableContext(0)]
	public enum MatchFallbackAttempt
	{
		// Token: 0x04001552 RID: 5458
		None,
		// Token: 0x04001553 RID: 5459
		RemoveTricky,
		// Token: 0x04001554 RID: 5460
		RemoveHalfBeatHops,
		// Token: 0x04001555 RID: 5461
		RemoveAllObstacles,
		// Token: 0x04001556 RID: 5462
		ErrorForceEmpty,
		// Token: 0x04001557 RID: 5463
		MAX
	}

	// Token: 0x0200029E RID: 670
	[NullableContext(0)]
	public struct MatchOption
	{
		// Token: 0x170004AF RID: 1199
		// (get) Token: 0x060015A9 RID: 5545 RVA: 0x00094770 File Offset: 0x00092970
		public bool isSuccess
		{
			get
			{
				return this.rejectReason == TrackBuilder.MatchRejectReason.None && this.fallback == TrackBuilder.MatchFallbackAttempt.None && !this.chunkProto.debugFallbackOnly;
			}
		}

		// Token: 0x170004B0 RID: 1200
		// (get) Token: 0x060015AA RID: 5546 RVA: 0x00094792 File Offset: 0x00092992
		public bool isFallback
		{
			get
			{
				return (this.rejectReason == TrackBuilder.MatchRejectReason.None && this.fallback != TrackBuilder.MatchFallbackAttempt.None) || this.chunkProto.debugFallbackOnly;
			}
		}

		// Token: 0x170004B1 RID: 1201
		// (get) Token: 0x060015AB RID: 5547 RVA: 0x000947B1 File Offset: 0x000929B1
		public bool isReject
		{
			get
			{
				return this.rejectReason > TrackBuilder.MatchRejectReason.None;
			}
		}

		// Token: 0x060015AC RID: 5548 RVA: 0x000947BC File Offset: 0x000929BC
		public TrackBuilder.MatchOption Reject(TrackBuilder.MatchRejectReason reason)
		{
			this.rejectReason = reason;
			return this;
		}

		// Token: 0x04001558 RID: 5464
		[Nullable(1)]
		public Chunk chunkProto;

		// Token: 0x04001559 RID: 5465
		public TrackBuilder.MatchParams matchParams;

		// Token: 0x0400155A RID: 5466
		public float scoreWeight;

		// Token: 0x0400155B RID: 5467
		public TrackBuilder.MatchRejectReason rejectReason;

		// Token: 0x0400155C RID: 5468
		public TrackBuilder.MatchFallbackAttempt fallback;
	}

	// Token: 0x0200029F RID: 671
	[NullableContext(0)]
	public struct MatchParams
	{
		// Token: 0x0400155D RID: 5469
		public Obstacles obstacles;

		// Token: 0x0400155E RID: 5470
		public int beatCount;

		// Token: 0x0400155F RID: 5471
		public bool eastwards;

		// Token: 0x04001560 RID: 5472
		public Range requiredAbsSlopeRange;
	}

	// Token: 0x020002A0 RID: 672
	[Nullable(0)]
	public struct MatchDecision
	{
		// Token: 0x060015AD RID: 5549 RVA: 0x000947CB File Offset: 0x000929CB
		public void ReturnToPool()
		{
			if (this.options != null)
			{
				this.options.Clear();
				TrackBuilder.MatchDecision._listPool.Push(this.options);
			}
		}

		// Token: 0x060015AE RID: 5550 RVA: 0x000947F0 File Offset: 0x000929F0
		public MatchDecision(TrackBuilder.MatchParams matchParams)
		{
			this = default(TrackBuilder.MatchDecision);
			this.originalMatchParams = matchParams;
			this.options = ((TrackBuilder.MatchDecision._listPool.Count == 0) ? new List<TrackBuilder.MatchOption>() : TrackBuilder.MatchDecision._listPool.Pop());
		}

		// Token: 0x04001561 RID: 5473
		public TrackBuilder.MatchParams originalMatchParams;

		// Token: 0x04001562 RID: 5474
		public TrackBuilder.MatchOption chosenOption;

		// Token: 0x04001563 RID: 5475
		public List<TrackBuilder.MatchOption> options;

		// Token: 0x04001564 RID: 5476
		[Nullable(2)]
		public BeatTrack track;

		// Token: 0x04001565 RID: 5477
		public int barIdx;

		// Token: 0x04001566 RID: 5478
		private static Stack<List<TrackBuilder.MatchOption>> _listPool = new Stack<List<TrackBuilder.MatchOption>>();
	}

	// Token: 0x020002A1 RID: 673
	[NullableContext(0)]
	private struct ChosenSwitchback
	{
		// Token: 0x04001567 RID: 5479
		[Nullable(2)]
		public Chunk chunk;

		// Token: 0x04001568 RID: 5480
		public float barDurationScalar;

		// Token: 0x04001569 RID: 5481
		public float extensionDuration;
	}
}
