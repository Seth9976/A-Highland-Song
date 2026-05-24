using System;
using System.Collections.Generic;
using Ink.Runtime;
using UnityEngine;

// Token: 0x020000A0 RID: 160
[Serializable]
public class InkListChangeHandler
{
	// Token: 0x17000150 RID: 336
	// (get) Token: 0x06000529 RID: 1321 RVA: 0x00028E15 File Offset: 0x00027015
	public string variableName
	{
		get
		{
			return this._variableName;
		}
	}

	// Token: 0x17000151 RID: 337
	// (get) Token: 0x0600052A RID: 1322 RVA: 0x00028E1D File Offset: 0x0002701D
	public InkList inkList
	{
		get
		{
			return this._inkList;
		}
	}

	// Token: 0x17000152 RID: 338
	// (get) Token: 0x0600052B RID: 1323 RVA: 0x00028E25 File Offset: 0x00027025
	public IReadOnlyList<InkListItem> currentListItems
	{
		get
		{
			return this._currentListItems;
		}
	}

	// Token: 0x0600052C RID: 1324 RVA: 0x00028E2D File Offset: 0x0002702D
	public InkListChangeHandler(string variableName)
	{
		this._variableName = variableName;
	}

	// Token: 0x0600052D RID: 1325 RVA: 0x00028E68 File Offset: 0x00027068
	public void RefreshValue(Story story, bool silently)
	{
		this.OnInkVarChanged(this.variableName, story.variablesState[this.variableName], silently);
	}

	// Token: 0x0600052E RID: 1326 RVA: 0x00028E88 File Offset: 0x00027088
	public void AddVariableObserver(Story story)
	{
		if (this.observing)
		{
			Debug.LogWarning("Tried observing story for variable with name " + this._variableName + " but we're already observing. Please call RemoveVariableObserver first!");
			return;
		}
		story.ObserveVariable(this.variableName, new Story.VariableObserver(this.OnInkVarChanged));
		this.observing = true;
	}

	// Token: 0x0600052F RID: 1327 RVA: 0x00028ED7 File Offset: 0x000270D7
	public void RemoveVariableObserver(Story story)
	{
		if (!this.observing)
		{
			return;
		}
		if (story != null)
		{
			story.RemoveVariableObserver(new Story.VariableObserver(this.OnInkVarChanged), this._variableName);
		}
		this.observing = false;
	}

	// Token: 0x06000530 RID: 1328 RVA: 0x00028F0A File Offset: 0x0002710A
	private void OnInkVarChanged(string variableName, object newValue)
	{
		this.OnInkVarChanged(variableName, newValue, false);
	}

	// Token: 0x06000531 RID: 1329 RVA: 0x00028F18 File Offset: 0x00027118
	private void OnInkVarChanged(string variableName, object newValue, bool silently)
	{
		this._inkList = (InkList)newValue;
		this.prevListItems.Clear();
		this.prevListItems.AddRange(this._currentListItems);
		this._currentListItems.Clear();
		foreach (KeyValuePair<InkListItem, int> keyValuePair in this.inkList)
		{
			this._currentListItems.Add(keyValuePair.Key);
		}
		if (IEnumerableX.GetChanges<InkListItem>(this.prevListItems, this._currentListItems, ref this.itemsRemoved, ref this.itemsAdded) && !silently && this.onChange != null)
		{
			this.onChange(this._currentListItems, this.itemsAdded, this.itemsRemoved);
		}
	}

	// Token: 0x040005F4 RID: 1524
	[SerializeField]
	private string _variableName;

	// Token: 0x040005F5 RID: 1525
	[SerializeField]
	private bool observing;

	// Token: 0x040005F6 RID: 1526
	private InkList _inkList;

	// Token: 0x040005F7 RID: 1527
	private List<InkListItem> prevListItems = new List<InkListItem>();

	// Token: 0x040005F8 RID: 1528
	[SerializeField]
	private List<InkListItem> _currentListItems = new List<InkListItem>();

	// Token: 0x040005F9 RID: 1529
	private List<InkListItem> itemsAdded = new List<InkListItem>();

	// Token: 0x040005FA RID: 1530
	private List<InkListItem> itemsRemoved = new List<InkListItem>();

	// Token: 0x040005FB RID: 1531
	public InkListChangeHandler.OnChangeDelegate onChange;

	// Token: 0x020002CD RID: 717
	// (Invoke) Token: 0x06001643 RID: 5699
	public delegate void OnChangeDelegate(IReadOnlyList<InkListItem> currentListItems, IReadOnlyList<InkListItem> itemsAdded, IReadOnlyList<InkListItem> itemsRemoved);
}
