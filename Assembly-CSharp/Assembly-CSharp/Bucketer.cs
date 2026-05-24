using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000170 RID: 368
public class Bucketer<T> where T : Behaviour
{
	// Token: 0x170002D5 RID: 725
	// (get) Token: 0x06000C38 RID: 3128 RVA: 0x00061D76 File Offset: 0x0005FF76
	public List<T> all
	{
		get
		{
			return this._all;
		}
	}

	// Token: 0x170002D6 RID: 726
	// (get) Token: 0x06000C39 RID: 3129 RVA: 0x00061D7E File Offset: 0x0005FF7E
	public int count
	{
		get
		{
			return this._count;
		}
	}

	// Token: 0x06000C3A RID: 3130 RVA: 0x00061D88 File Offset: 0x0005FF88
	public Bucketer(float bucketWidth, bool canAccessAll = false, bool returnsOnlyActive = false)
	{
		this._canAccessAll = canAccessAll;
		this._returnsOnlyActive = returnsOnlyActive;
		this.bucketWidth = bucketWidth;
	}

	// Token: 0x06000C3B RID: 3131 RVA: 0x00061E08 File Offset: 0x00060008
	public List<T> Nearby(Vector2 position, Range depthRange, float searchRadius = 0f, List<T> customScratchList = null)
	{
		if (customScratchList == null)
		{
			customScratchList = this._scratchList;
		}
		customScratchList.Clear();
		foreach (Bucketer<T>.Item item in this._dynamicItems)
		{
			if (depthRange.Contains(item.depth) && (!this._returnsOnlyActive || item.obj.isActiveAndEnabled))
			{
				customScratchList.Add(item.obj);
			}
		}
		int num = Mathf.RoundToInt((position.x - searchRadius) / this.bucketWidth) - this._leftIdx;
		int num2 = Mathf.RoundToInt((position.x + searchRadius) / this.bucketWidth) - this._leftIdx;
		if (num2 < 0 || num >= this._buckets.Count)
		{
			return customScratchList;
		}
		num = Mathf.Max(num, 0);
		num2 = Mathf.Min(num2, this._buckets.Count - 1);
		for (int i = num; i <= num2; i++)
		{
			foreach (Bucketer<T>.Item item2 in this._buckets[i])
			{
				if (depthRange.Contains(item2.depth) && (!this._returnsOnlyActive || item2.obj.isActiveAndEnabled))
				{
					customScratchList.Add(item2.obj);
				}
			}
		}
		return customScratchList;
	}

	// Token: 0x06000C3C RID: 3132 RVA: 0x00061F94 File Offset: 0x00060194
	public void Clear()
	{
		foreach (List<Bucketer<T>.Item> list in this._buckets)
		{
			list.Clear();
			Bucketer<T>._bucketPool.Push(list);
		}
		this._buckets.Clear();
		this._all.Clear();
		this._dynamicItems.Clear();
		this._leftIdx = int.MaxValue;
		this.zRange = new Range(float.MaxValue, float.MinValue);
	}

	// Token: 0x06000C3D RID: 3133 RVA: 0x00062034 File Offset: 0x00060234
	private List<Bucketer<T>.Item> NewBucket()
	{
		if (Bucketer<T>._bucketPool.Count > 0)
		{
			return Bucketer<T>._bucketPool.Pop();
		}
		return new List<Bucketer<T>.Item>();
	}

	// Token: 0x06000C3E RID: 3134 RVA: 0x00062053 File Offset: 0x00060253
	public void AddAll(Transform root, Func<T, Range> getXRange, Func<T, bool> filter = null)
	{
		this._scratchList.Clear();
		root.GetComponentsInChildren<T>(true, this._scratchList);
		this.AddAll(this._scratchList, getXRange, filter);
		this._scratchList.Clear();
	}

	// Token: 0x06000C3F RID: 3135 RVA: 0x00062088 File Offset: 0x00060288
	public void AddAll(IList<T> items, Func<T, Range> getXRange, Func<T, bool> filter = null)
	{
		foreach (T t in items)
		{
			if (filter == null || filter(t))
			{
				Range range = getXRange(t);
				int i = Mathf.RoundToInt(range.min / this.bucketWidth);
				int j = Mathf.RoundToInt(range.max / this.bucketWidth);
				if (this._buckets.Count == 0)
				{
					this._buckets.Add(this.NewBucket());
					this._leftIdx = i;
				}
				while (i < this._leftIdx)
				{
					this._buckets.Insert(0, this.NewBucket());
					this._leftIdx--;
				}
				while (j >= this._leftIdx + this._buckets.Count)
				{
					this._buckets.Add(this.NewBucket());
				}
				for (int k = i; k <= j; k++)
				{
					this._buckets[k - this._leftIdx].Add(new Bucketer<T>.Item
					{
						depth = t.transform.position.z,
						obj = t
					});
				}
				if (this._canAccessAll)
				{
					this._all.Add(t);
				}
				float z = t.transform.position.z;
				if (z > this.zRange.max)
				{
					this.zRange.max = z;
				}
				if (z < this.zRange.min)
				{
					this.zRange.min = z;
				}
				this._count++;
			}
		}
	}

	// Token: 0x06000C40 RID: 3136 RVA: 0x00062258 File Offset: 0x00060458
	public void AddDynamic(IList<T> list)
	{
		foreach (T t in list)
		{
			this._dynamicItems.Add(new Bucketer<T>.Item
			{
				obj = t,
				depth = t.transform.position.z
			});
		}
	}

	// Token: 0x06000C41 RID: 3137 RVA: 0x000622D4 File Offset: 0x000604D4
	public void RemoveDynamic(IList<T> list)
	{
		this._dynamicItems.RemoveAll((Bucketer<T>.Item item) => list.Contains(item.obj));
	}

	// Token: 0x06000C42 RID: 3138 RVA: 0x00062308 File Offset: 0x00060508
	public void AddDynamic(T item)
	{
		this._dynamicItems.Add(new Bucketer<T>.Item
		{
			obj = item,
			depth = item.transform.position.z
		});
	}

	// Token: 0x06000C43 RID: 3139 RVA: 0x00062350 File Offset: 0x00060550
	public void RemoveDynamic(T item)
	{
		for (int i = 0; i < this._dynamicItems.Count; i++)
		{
			if (this._dynamicItems[i].obj == item)
			{
				this._dynamicItems.RemoveAt(i);
				return;
			}
		}
	}

	// Token: 0x04000E9D RID: 3741
	public float bucketWidth = 30f;

	// Token: 0x04000E9E RID: 3742
	public float margin = 15f;

	// Token: 0x04000E9F RID: 3743
	public Range zRange = new Range(float.MaxValue, float.MinValue);

	// Token: 0x04000EA0 RID: 3744
	private int _leftIdx;

	// Token: 0x04000EA1 RID: 3745
	private List<List<Bucketer<T>.Item>> _buckets = new List<List<Bucketer<T>.Item>>();

	// Token: 0x04000EA2 RID: 3746
	private List<Bucketer<T>.Item> _dynamicItems = new List<Bucketer<T>.Item>();

	// Token: 0x04000EA3 RID: 3747
	private List<T> _all = new List<T>();

	// Token: 0x04000EA4 RID: 3748
	private int _count;

	// Token: 0x04000EA5 RID: 3749
	private List<T> _scratchList = new List<T>();

	// Token: 0x04000EA6 RID: 3750
	private bool _canAccessAll;

	// Token: 0x04000EA7 RID: 3751
	private bool _returnsOnlyActive;

	// Token: 0x04000EA8 RID: 3752
	private static Stack<List<Bucketer<T>.Item>> _bucketPool = new Stack<List<Bucketer<T>.Item>>();

	// Token: 0x0200039E RID: 926
	private struct Item
	{
		// Token: 0x04001973 RID: 6515
		public float depth;

		// Token: 0x04001974 RID: 6516
		public T obj;
	}
}
