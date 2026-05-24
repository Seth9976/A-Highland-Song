using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

// Token: 0x0200006D RID: 109
[CreateAssetMenu]
public class BeatTrack : ScriptableObject
{
	// Token: 0x170000CD RID: 205
	// (get) Token: 0x060002FD RID: 765 RVA: 0x000187D0 File Offset: 0x000169D0
	public float lastBeatEndTime
	{
		get
		{
			if (this.beats.Length == 0)
			{
				return 0f;
			}
			int num = this.beats.Length - 1;
			return this.beats[num].time + this.BeatDuration(num);
		}
	}

	// Token: 0x170000CE RID: 206
	// (get) Token: 0x060002FE RID: 766 RVA: 0x00018810 File Offset: 0x00016A10
	public BeatTrack.ObstacleRef firstObstacleRef
	{
		get
		{
			int num = Mathf.Min(16, this.bars.Length);
			for (int i = 0; i < num; i++)
			{
				Obstacles obstacles = this.bars[i].obstacles;
				if (!obstacles.isEmpty)
				{
					for (int j = 0; j < 16; j++)
					{
						if (obstacles[j] != ObstacleType.None)
						{
							return new BeatTrack.ObstacleRef
							{
								barIdx = i,
								halfBeatIdx = j
							};
						}
					}
				}
			}
			return BeatTrack.ObstacleRef.invalid;
		}
	}

	// Token: 0x060002FF RID: 767 RVA: 0x0001888C File Offset: 0x00016A8C
	public float BeatDuration(int beatIdx)
	{
		if (this.beats.Length < 2)
		{
			return 0.5f;
		}
		if (beatIdx == this.beats.Length - 1)
		{
			beatIdx--;
		}
		return this.beats[beatIdx + 1].time - this.beats[beatIdx].time;
	}

	// Token: 0x06000300 RID: 768 RVA: 0x000188E4 File Offset: 0x00016AE4
	public float BeatDurationAtTimeWhenPlaying(float t)
	{
		int num = this.BeatIndexAtTime(t + 0.0001f);
		return this.BeatDuration(num);
	}

	// Token: 0x06000301 RID: 769 RVA: 0x00018906 File Offset: 0x00016B06
	public int BeatIndexAtTime(float t)
	{
		return BinarySearch.SearchRoundDown<BeatTrack.Beat>(this.beats, t, (BeatTrack.Beat b) => b.time);
	}

	// Token: 0x06000302 RID: 770 RVA: 0x00018933 File Offset: 0x00016B33
	public int NearestBeatIndexAtTime(float t)
	{
		return BinarySearch.SearchNearest<BeatTrack.Beat>(this.beats, t, (BeatTrack.Beat b) => b.time);
	}

	// Token: 0x06000303 RID: 771 RVA: 0x00018960 File Offset: 0x00016B60
	public int BarIndexAtTime(float t)
	{
		if (t < this.beats[this.bars[0].firstBeatIdx].time)
		{
			return 0;
		}
		return BinarySearch.SearchRoundDown<BeatTrack.Bar>(this.bars, t, (BeatTrack.Bar bar) => this.beats[bar.firstBeatIdx].time);
	}

	// Token: 0x06000304 RID: 772 RVA: 0x000189A0 File Offset: 0x00016BA0
	public float FractionalIndexAtTime(float t)
	{
		int num = this.BeatIndexAtTime(t);
		float time = this.beats[num].time;
		float num2 = this.BeatDuration(num);
		if (num2 > 0f)
		{
			return (float)num + (t - time) / num2;
		}
		return (float)num;
	}

	// Token: 0x06000305 RID: 773 RVA: 0x000189E4 File Offset: 0x00016BE4
	public int BarIdxWithBeatIdx(int beatIdx)
	{
		if (this.bars.Length == 0 || beatIdx < this.bars[0].firstBeatIdx)
		{
			return -1;
		}
		return BinarySearch.SearchRoundDown<BeatTrack.Bar>(this.bars, (float)beatIdx, (BeatTrack.Bar bar) => (float)bar.firstBeatIdx);
	}

	// Token: 0x06000306 RID: 774 RVA: 0x00018A3C File Offset: 0x00016C3C
	public int NextBarIndexAtTime(float musicTime)
	{
		return BinarySearch.SearchNext<BeatTrack.Bar>(this.bars, musicTime, (BeatTrack.Bar bar) => this.beats[bar.firstBeatIdx].time);
	}

	// Token: 0x06000307 RID: 775 RVA: 0x00018A56 File Offset: 0x00016C56
	public float BarStartTime(int barIdx)
	{
		return this.beats[this.bars[barIdx].firstBeatIdx].time;
	}

	// Token: 0x06000308 RID: 776 RVA: 0x00018A7C File Offset: 0x00016C7C
	public int BeatCountInBar(int barIdx)
	{
		int num;
		if (barIdx < this.bars.Length - 1)
		{
			num = this.bars[barIdx + 1].firstBeatIdx;
		}
		else
		{
			num = this.beats.Length;
		}
		return num - this.bars[barIdx].firstBeatIdx;
	}

	// Token: 0x06000309 RID: 777 RVA: 0x00018AC8 File Offset: 0x00016CC8
	public float BarDuration(int barIdx)
	{
		float num = this.BarStartTime(barIdx);
		float num2;
		if (barIdx < this.bars.Length - 1)
		{
			num2 = this.BarStartTime(barIdx + 1) - num;
		}
		else
		{
			num2 = this.lastBeatEndTime - num;
		}
		return num2;
	}

	// Token: 0x0600030A RID: 778 RVA: 0x00018B04 File Offset: 0x00016D04
	[return: TupleElementNames(new string[] { "barIdx", "barStartTime" })]
	public ValueTuple<int, float> FindNearestBarStart(float timeToFind)
	{
		int num = BinarySearch.SearchRoundDown<BeatTrack.Bar>(this.bars, timeToFind, (BeatTrack.Bar b) => this.beats[b.firstBeatIdx].time);
		float num2 = this.BarStartTime(num);
		float num3 = num2 - timeToFind;
		float num4 = float.MaxValue;
		if (num < this.bars.Length - 1)
		{
			num4 = this.BarStartTime(num + 1);
		}
		float num5 = num4 - timeToFind;
		if (Mathf.Abs(num3) > Mathf.Abs(num5))
		{
			num++;
			num2 = num4;
		}
		return new ValueTuple<int, float>(num, num2);
	}

	// Token: 0x0600030B RID: 779 RVA: 0x00018B70 File Offset: 0x00016D70
	public bool HasObstacleAt(int beatIndex)
	{
		int num = this.BarIdxWithBeatIdx(beatIndex);
		BeatTrack.Bar bar = this.bars[num];
		int num2 = 2 * this.BeatIndexInBar(beatIndex);
		return bar.obstacles.HasAny(num2, ObstacleType.Hop | ObstacleType.Special);
	}

	// Token: 0x0600030C RID: 780 RVA: 0x00018BAC File Offset: 0x00016DAC
	public int BeatIndexInBar(int beatIndex)
	{
		int num = this.BarIdxWithBeatIdx(beatIndex);
		BeatTrack.Bar bar = this.bars[num];
		return beatIndex - bar.firstBeatIdx;
	}

	// Token: 0x0600030D RID: 781 RVA: 0x00018BD8 File Offset: 0x00016DD8
	public BeatTrack.ObstacleRef FindNearestObstacle(float time)
	{
		int num = this.BarIndexAtTime(time);
		BeatTrack.Bar bar = this.bars[num];
		float time2 = this.beats[bar.firstBeatIdx].time;
		float num2 = this.BarDuration(num);
		int num3 = num;
		if (time < time2 + 0.5f * num2)
		{
			if (num > 0)
			{
				num3 = num - 1;
			}
		}
		else if (num < this.bars.Length - 1)
		{
			num3 = num + 1;
		}
		BeatTrack.ObstacleRef obstacleRef = this.NearestObstacleInBar(num, time);
		if (num == num3)
		{
			return obstacleRef;
		}
		BeatTrack.ObstacleRef obstacleRef2 = this.NearestObstacleInBar(num3, time);
		if (Mathf.Abs(obstacleRef.time - time) < Mathf.Abs(obstacleRef2.time - time))
		{
			return obstacleRef;
		}
		return obstacleRef2;
	}

	// Token: 0x0600030E RID: 782 RVA: 0x00018C88 File Offset: 0x00016E88
	public BeatTrack.ObstacleRef NearestObstacleInBar(int barIdx, float time)
	{
		BeatTrack.Bar bar = this.bars[barIdx];
		int num = this.BeatCountInBar(barIdx);
		BeatTrack.ObstacleRef obstacleRef = new BeatTrack.ObstacleRef
		{
			time = float.MaxValue,
			barIdx = -1,
			halfBeatIdx = -1,
			special = false
		};
		float num2 = float.MaxValue;
		for (int i = 0; i < 2 * num; i++)
		{
			int num3 = bar.firstBeatIdx + i / 2;
			float num4 = this.beats[num3].time;
			if (i % 2 == 1)
			{
				num4 += 0.5f * this.BeatDuration(num3);
			}
			if (bar.obstacles.HasAny(i, ObstacleType.Hop | ObstacleType.Special | ObstacleType.Tricky))
			{
				float num5 = Mathf.Abs(num4 - time);
				if (num5 < num2)
				{
					num2 = num5;
					obstacleRef.time = num4;
					obstacleRef.barIdx = barIdx;
					obstacleRef.halfBeatIdx = i;
					obstacleRef.special = bar.obstacles.Has(i, ObstacleType.Special);
				}
			}
		}
		return obstacleRef;
	}

	// Token: 0x04000439 RID: 1081
	public string displayName;

	// Token: 0x0400043A RID: 1082
	public Sprite bandLogo;

	// Token: 0x0400043B RID: 1083
	public AudioClip clip = Presume<AudioClip>.NonNull("C:\\Users\\josep\\Desktop\\highland-song\\Highland Run\\Assets\\Scripts\\MusicRunning\\BeatTrack.cs", 51);

	// Token: 0x0400043C RID: 1084
	public BeatTrack.Section[] sections = new BeatTrack.Section[0];

	// Token: 0x0400043D RID: 1085
	public BeatTrack.Beat[] beats = new BeatTrack.Beat[0];

	// Token: 0x0400043E RID: 1086
	public BeatTrack.Bar[] bars = new BeatTrack.Bar[0];

	// Token: 0x0200028D RID: 653
	[Serializable]
	public struct Section
	{
		// Token: 0x0400150C RID: 5388
		public int startBarIdx;

		// Token: 0x0400150D RID: 5389
		public string inkName;
	}

	// Token: 0x0200028E RID: 654
	[Flags]
	public enum BeatFlags
	{
		// Token: 0x0400150F RID: 5391
		None = 0,
		// Token: 0x04001510 RID: 5392
		Down = 1,
		// Token: 0x04001511 RID: 5393
		On = 2
	}

	// Token: 0x0200028F RID: 655
	[Serializable]
	public struct Beat : IComparer<BeatTrack.Beat>
	{
		// Token: 0x170004AC RID: 1196
		// (get) Token: 0x06001599 RID: 5529 RVA: 0x00094671 File Offset: 0x00092871
		public bool isDown
		{
			get
			{
				return (this.flags & BeatTrack.BeatFlags.Down) > BeatTrack.BeatFlags.None;
			}
		}

		// Token: 0x0600159A RID: 5530 RVA: 0x0009467E File Offset: 0x0009287E
		public int Compare(BeatTrack.Beat b0, BeatTrack.Beat b1)
		{
			return b0.time.CompareTo(b1.time);
		}

		// Token: 0x04001512 RID: 5394
		public float time;

		// Token: 0x04001513 RID: 5395
		[EnumFlag]
		public BeatTrack.BeatFlags flags;
	}

	// Token: 0x02000290 RID: 656
	[Serializable]
	public struct Bar
	{
		// Token: 0x170004AD RID: 1197
		// (get) Token: 0x0600159B RID: 5531 RVA: 0x00094692 File Offset: 0x00092892
		public bool hasSwitchback
		{
			get
			{
				return this.switchbackLegBeats > 0;
			}
		}

		// Token: 0x04001514 RID: 5396
		public int firstBeatIdx;

		// Token: 0x04001515 RID: 5397
		public Obstacles obstacles;

		// Token: 0x04001516 RID: 5398
		public int switchbackLegBeats;
	}

	// Token: 0x02000291 RID: 657
	[Serializable]
	public struct ObstacleRef
	{
		// Token: 0x170004AE RID: 1198
		// (get) Token: 0x0600159C RID: 5532 RVA: 0x0009469D File Offset: 0x0009289D
		public bool valid
		{
			get
			{
				return this.barIdx >= 0;
			}
		}

		// Token: 0x0600159D RID: 5533 RVA: 0x000946AB File Offset: 0x000928AB
		public bool IsSameTimeAs(BeatTrack.ObstacleRef obs2)
		{
			return this.barIdx == obs2.barIdx && this.halfBeatIdx == obs2.halfBeatIdx;
		}

		// Token: 0x0600159E RID: 5534 RVA: 0x000946CB File Offset: 0x000928CB
		public override string ToString()
		{
			if (!this.valid)
			{
				return "[Obs none/invalid]";
			}
			return string.Format("[Obs bar={0}, half beat={1}]", this.barIdx, this.halfBeatIdx);
		}

		// Token: 0x04001517 RID: 5399
		public int barIdx;

		// Token: 0x04001518 RID: 5400
		public int halfBeatIdx;

		// Token: 0x04001519 RID: 5401
		public float time;

		// Token: 0x0400151A RID: 5402
		public bool special;

		// Token: 0x0400151B RID: 5403
		public static BeatTrack.ObstacleRef invalid = new BeatTrack.ObstacleRef
		{
			barIdx = -1
		};
	}
}
