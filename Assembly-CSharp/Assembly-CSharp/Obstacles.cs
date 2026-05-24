using System;

// Token: 0x0200007A RID: 122
[Serializable]
public struct Obstacles
{
	// Token: 0x170000EB RID: 235
	// (get) Token: 0x0600035F RID: 863 RVA: 0x0001A9FF File Offset: 0x00018BFF
	public bool isEmpty
	{
		get
		{
			return this.bitmask == 0UL;
		}
	}

	// Token: 0x06000360 RID: 864 RVA: 0x0001AA0B File Offset: 0x00018C0B
	private Obstacles(ulong bitmask)
	{
		this.bitmask = bitmask;
	}

	// Token: 0x06000361 RID: 865 RVA: 0x0001AA14 File Offset: 0x00018C14
	public Obstacles(string beatPattern)
	{
		this.bitmask = 0UL;
		int num = 0;
		foreach (char c in beatPattern)
		{
			if (c != ' ')
			{
				if (c == 'h')
				{
					this[num] = ObstacleType.Hop;
				}
				else if (c == 't')
				{
					this[num] = ObstacleType.Tricky;
				}
				else if (c == 's')
				{
					this[num] = ObstacleType.Special;
				}
				else if (c != '_')
				{
					throw new Exception(string.Format("Unexpected character '{0}' in beat pattern '{1}", c, beatPattern));
				}
				num++;
			}
		}
	}

	// Token: 0x06000362 RID: 866 RVA: 0x0001AA98 File Offset: 0x00018C98
	public bool Is(string beatPattern)
	{
		return this.Equals(new Obstacles(beatPattern));
	}

	// Token: 0x170000EC RID: 236
	public ObstacleType this[int halfBeatIdx]
	{
		get
		{
			if (halfBeatIdx < 0 || halfBeatIdx >= 16)
			{
				throw new IndexOutOfRangeException();
			}
			int num = halfBeatIdx * 4;
			ulong num2 = 15UL << num;
			return (ObstacleType)((this.bitmask & num2) >> num);
		}
		set
		{
			if (halfBeatIdx < 0 || halfBeatIdx >= 16)
			{
				throw new IndexOutOfRangeException();
			}
			int num = halfBeatIdx * 4;
			ulong num2 = 15UL << num;
			ulong num3 = this.bitmask & ~num2;
			this.bitmask = num3 | ((ulong)value << num);
		}
	}

	// Token: 0x06000365 RID: 869 RVA: 0x0001AB30 File Offset: 0x00018D30
	public void Add(int halfBeatIdx, ObstacleType typeToAdd)
	{
		if (halfBeatIdx < 0 || halfBeatIdx >= 16)
		{
			throw new IndexOutOfRangeException(halfBeatIdx.ToString());
		}
		int num = halfBeatIdx * 4;
		this.bitmask |= (ulong)typeToAdd << num;
	}

	// Token: 0x06000366 RID: 870 RVA: 0x0001AB6C File Offset: 0x00018D6C
	public void Remove(int halfBeatIdx, ObstacleType typeToRemove)
	{
		if (halfBeatIdx < 0 || halfBeatIdx >= 16)
		{
			throw new IndexOutOfRangeException(halfBeatIdx.ToString());
		}
		int num = halfBeatIdx * 4;
		this.bitmask &= ~((ulong)typeToRemove << num);
	}

	// Token: 0x06000367 RID: 871 RVA: 0x0001ABA8 File Offset: 0x00018DA8
	public bool Has(int halfBeatIdx, ObstacleType typeToCheck)
	{
		if (halfBeatIdx < 0 || halfBeatIdx >= 16)
		{
			throw new IndexOutOfRangeException(halfBeatIdx.ToString());
		}
		int num = halfBeatIdx * 4;
		ulong num2 = (ulong)typeToCheck << num;
		return (this.bitmask & num2) == num2;
	}

	// Token: 0x06000368 RID: 872 RVA: 0x0001ABE4 File Offset: 0x00018DE4
	public bool HasAny(int halfBeatIdx, ObstacleType typeToCheck)
	{
		if (halfBeatIdx < 0 || halfBeatIdx >= 16)
		{
			throw new IndexOutOfRangeException(halfBeatIdx.ToString());
		}
		int num = halfBeatIdx * 4;
		ulong num2 = (ulong)typeToCheck << num;
		return (this.bitmask & num2) > 0UL;
	}

	// Token: 0x06000369 RID: 873 RVA: 0x0001AC1F File Offset: 0x00018E1F
	public void Set(int halfBeatIdx, ObstacleType typeToSet, bool enabled)
	{
		if (enabled)
		{
			this.Add(halfBeatIdx, typeToSet);
			return;
		}
		this.Remove(halfBeatIdx, typeToSet);
	}

	// Token: 0x0600036A RID: 874 RVA: 0x0001AC38 File Offset: 0x00018E38
	public ValueTuple<Obstacles, Obstacles> Split(int halfBeatIdx)
	{
		ulong num = this.bitmask >> 4 * halfBeatIdx;
		return new ValueTuple<Obstacles, Obstacles>(new Obstacles(this.bitmask & ~(ulong.MaxValue << 4 * halfBeatIdx)), new Obstacles(num));
	}

	// Token: 0x0600036B RID: 875 RVA: 0x0001AC74 File Offset: 0x00018E74
	public Obstacles Merge(Obstacles withObs, int fromHalfBeatIdx)
	{
		ulong num = withObs.bitmask << 4 * fromHalfBeatIdx;
		return new Obstacles((this.bitmask & ~(ulong.MaxValue << 4 * fromHalfBeatIdx)) | num);
	}

	// Token: 0x0600036C RID: 876 RVA: 0x0001ACA7 File Offset: 0x00018EA7
	public override string ToString()
	{
		return this.ToString(16);
	}

	// Token: 0x0600036D RID: 877 RVA: 0x0001ACB4 File Offset: 0x00018EB4
	public string ToString(int halfBeatCount)
	{
		if (halfBeatCount < 0 || halfBeatCount > 16)
		{
			throw new ArgumentOutOfRangeException();
		}
		int num = halfBeatCount / 2;
		int num2 = halfBeatCount + num;
		for (int i = 0; i < num2; i++)
		{
			int num3 = i / 3;
			int num4 = i % 3;
			if (num4 == 2)
			{
				Obstacles._charArrayScratch[i] = ' ';
			}
			else
			{
				int num5 = 2 * num3 + num4;
				if (this.Has(num5, ObstacleType.Tricky))
				{
					Obstacles._charArrayScratch[i] = 't';
				}
				else if (this.Has(num5, ObstacleType.Hop))
				{
					Obstacles._charArrayScratch[i] = 'h';
				}
				else if (this.Has(num5, ObstacleType.Special))
				{
					Obstacles._charArrayScratch[i] = 's';
				}
				else
				{
					Obstacles._charArrayScratch[i] = '_';
				}
			}
		}
		return new string(Obstacles._charArrayScratch, 0, num2);
	}

	// Token: 0x0600036E RID: 878 RVA: 0x0001AD60 File Offset: 0x00018F60
	public bool Equals(Obstacles obs2, int halfBeatCount)
	{
		if (halfBeatCount < 0 || halfBeatCount > 16)
		{
			throw new ArgumentOutOfRangeException();
		}
		int num = 16 - halfBeatCount;
		ulong num2 = ulong.MaxValue >> num * 4;
		return (this.bitmask & num2) == (obs2.bitmask & num2);
	}

	// Token: 0x0600036F RID: 879 RVA: 0x0001ADA0 File Offset: 0x00018FA0
	public bool HasHopPattern(Obstacles patternObs, int halfBeatCount)
	{
		if (halfBeatCount < 0 || halfBeatCount > 16)
		{
			throw new ArgumentOutOfRangeException();
		}
		for (int i = 0; i < halfBeatCount; i++)
		{
			bool flag = patternObs.Has(i, ObstacleType.Hop);
			if (this.HasAny(i, ObstacleType.Hop | ObstacleType.Special) != flag)
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x040004AB RID: 1195
	public ulong bitmask;

	// Token: 0x040004AC RID: 1196
	public static readonly Obstacles empty = default(Obstacles);

	// Token: 0x040004AD RID: 1197
	private const ulong allMask = 18446744073709551615UL;

	// Token: 0x040004AE RID: 1198
	private static char[] _charArrayScratch = new char[10000];
}
