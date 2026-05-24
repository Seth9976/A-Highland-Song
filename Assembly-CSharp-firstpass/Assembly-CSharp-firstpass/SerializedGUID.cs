using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000004 RID: 4
[Serializable]
public class SerializedGUID : ISerializationCallbackReceiver
{
	// Token: 0x17000001 RID: 1
	// (get) Token: 0x06000015 RID: 21 RVA: 0x0000239F File Offset: 0x0000059F
	// (set) Token: 0x06000016 RID: 22 RVA: 0x000023BF File Offset: 0x000005BF
	public Guid guid
	{
		get
		{
			if (this._guid == Guid.Empty)
			{
				this.LoadOrCreateGuid();
			}
			return this._guid;
		}
		private set
		{
			this._guid = value;
		}
	}

	// Token: 0x06000017 RID: 23 RVA: 0x000023C8 File Offset: 0x000005C8
	public bool Validate()
	{
		if (this.hasBeenValidated)
		{
			return true;
		}
		bool flag = true;
		if (SerializedGUID.guids.Contains(this.guid))
		{
			flag = false;
			this.Clear();
		}
		SerializedGUID.guids.Add(this.guid);
		this.hasBeenValidated = true;
		return flag;
	}

	// Token: 0x06000018 RID: 24 RVA: 0x00002414 File Offset: 0x00000614
	public void Destroy()
	{
		SerializedGUID.guids.Remove(this.guid);
		this.hasBeenValidated = false;
	}

	// Token: 0x06000019 RID: 25 RVA: 0x0000242E File Offset: 0x0000062E
	private void Clear()
	{
		this.serializedGuid = null;
		this.guid = Guid.Empty;
		this.hasBeenValidated = false;
	}

	// Token: 0x0600001A RID: 26 RVA: 0x0000244C File Offset: 0x0000064C
	private void LoadOrCreateGuid()
	{
		if (this.serializedGuid == null || this.serializedGuid.Length != 16)
		{
			this.guid = Guid.NewGuid();
			this.serializedGuid = this.guid.ToByteArray();
			if (this.OnSetGUID != null)
			{
				this.OnSetGUID();
				return;
			}
		}
		else if (this._guid == Guid.Empty)
		{
			this.guid = new Guid(this.serializedGuid);
		}
	}

	// Token: 0x0600001B RID: 27 RVA: 0x000024C4 File Offset: 0x000006C4
	public void OnBeforeSerialize()
	{
		if (this.guid != Guid.Empty)
		{
			this.serializedGuid = this.guid.ToByteArray();
		}
	}

	// Token: 0x0600001C RID: 28 RVA: 0x000024F7 File Offset: 0x000006F7
	public void OnAfterDeserialize()
	{
		if (this.serializedGuid != null && this.serializedGuid.Length == 16)
		{
			this.guid = new Guid(this.serializedGuid);
		}
	}

	// Token: 0x0600001D RID: 29 RVA: 0x00002520 File Offset: 0x00000720
	public override string ToString()
	{
		return this.guid.ToString();
	}

	// Token: 0x04000007 RID: 7
	private static HashSet<Guid> guids = new HashSet<Guid>();

	// Token: 0x04000008 RID: 8
	private Guid _guid = Guid.Empty;

	// Token: 0x04000009 RID: 9
	public Action OnSetGUID;

	// Token: 0x0400000A RID: 10
	[SerializeField]
	[HideInInspector]
	private byte[] serializedGuid;

	// Token: 0x0400000B RID: 11
	[NonSerialized]
	private bool hasBeenValidated;
}
