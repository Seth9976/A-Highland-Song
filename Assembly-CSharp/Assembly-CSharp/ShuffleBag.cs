using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

// Token: 0x020001EB RID: 491
[Serializable]
public class ShuffleBag<T>
{
	// Token: 0x1700040C RID: 1036
	// (get) Token: 0x06001163 RID: 4451 RVA: 0x00080B22 File Offset: 0x0007ED22
	public ReadOnlyCollection<T> sourceItems
	{
		get
		{
			return this._sourceItems.AsReadOnly();
		}
	}

	// Token: 0x1700040D RID: 1037
	// (get) Token: 0x06001164 RID: 4452 RVA: 0x00080B2F File Offset: 0x0007ED2F
	public ReadOnlyCollection<T> items
	{
		get
		{
			return this._items.AsReadOnly();
		}
	}

	// Token: 0x06001165 RID: 4453 RVA: 0x00080B3C File Offset: 0x0007ED3C
	public ShuffleBag(List<T> sourceItems)
	{
		this._sourceItems = sourceItems;
		this.RefreshBag(true);
	}

	// Token: 0x06001166 RID: 4454 RVA: 0x00080B68 File Offset: 0x0007ED68
	public ShuffleBag(List<T> sourceItems, List<T> items)
	{
		this._sourceItems = sourceItems;
		this._items = items;
	}

	// Token: 0x06001167 RID: 4455 RVA: 0x00080B94 File Offset: 0x0007ED94
	public ShuffleBag(ShuffleBag<T> otherBag)
	{
		this._sourceItems = new List<T>(otherBag.sourceItems);
		this._items = new List<T>(otherBag.items);
	}

	// Token: 0x06001168 RID: 4456 RVA: 0x00080BD4 File Offset: 0x0007EDD4
	private void RefreshBag(bool clearBeforeAdding = true)
	{
		if (clearBeforeAdding)
		{
			this._items.Clear();
		}
		foreach (T t in this._sourceItems)
		{
			this._items.Add(t);
		}
		ShuffleBag<T>.Shuffle(this._items);
	}

	// Token: 0x06001169 RID: 4457 RVA: 0x00080C48 File Offset: 0x0007EE48
	public T PeekAhead()
	{
		return this._items[0];
	}

	// Token: 0x0600116A RID: 4458 RVA: 0x00080C58 File Offset: 0x0007EE58
	public T TakeNext()
	{
		T t = this._items[0];
		this.Remove(t);
		return t;
	}

	// Token: 0x0600116B RID: 4459 RVA: 0x00080C7C File Offset: 0x0007EE7C
	public bool Remove(T item)
	{
		int num = this._items.IndexOf(item);
		return this.RemoveAt(num);
	}

	// Token: 0x0600116C RID: 4460 RVA: 0x00080C9D File Offset: 0x0007EE9D
	public bool RemoveAt(int index)
	{
		bool flag = this._items.ContainsIndex(index);
		if (flag)
		{
			this._items.RemoveAt(index);
			if (this._items.Count == 0)
			{
				this.RefreshBag(true);
			}
		}
		return flag;
	}

	// Token: 0x0600116D RID: 4461 RVA: 0x00080CD0 File Offset: 0x0007EED0
	public static void Shuffle(IList<T> list)
	{
		int i = list.Count;
		while (i > 1)
		{
			i--;
			int num = Random.Range(0, i + 1);
			T t = list[num];
			list[num] = list[i];
			list[i] = t;
		}
	}

	// Token: 0x0600116E RID: 4462 RVA: 0x00080D18 File Offset: 0x0007EF18
	public static void Shuffle(IList<T> list, int seed)
	{
		Random.State state = Random.state;
		Random.InitState(seed);
		int i = list.Count;
		while (i > 1)
		{
			i--;
			int num = Random.Range(0, i + 1);
			T t = list[num];
			list[num] = list[i];
			list[i] = t;
		}
		Random.state = state;
	}

	// Token: 0x0400126F RID: 4719
	[SerializeField]
	private List<T> _sourceItems = new List<T>();

	// Token: 0x04001270 RID: 4720
	[SerializeField]
	private List<T> _items = new List<T>();
}
