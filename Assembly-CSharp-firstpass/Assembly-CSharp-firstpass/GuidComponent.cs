using System;
using UnityEngine;

// Token: 0x02000002 RID: 2
[ExecuteInEditMode]
[DisallowMultipleComponent]
public class GuidComponent : MonoBehaviour, ISerializationCallbackReceiver
{
	// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
	public Guid GetGuid()
	{
		if (this.guid == Guid.Empty && this.serializedGuid != null && this.serializedGuid.Length == 16)
		{
			this.guid = new Guid(this.serializedGuid);
		}
		return this.guid;
	}

	// Token: 0x06000002 RID: 2 RVA: 0x0000208F File Offset: 0x0000028F
	private void OnDisable()
	{
	}

	// Token: 0x06000003 RID: 3 RVA: 0x00002094 File Offset: 0x00000294
	private void CreateGuid()
	{
		bool flag = false;
		if (this.serializedGuid == null || this.serializedGuid.Length != 16)
		{
			this.guid = Guid.NewGuid();
			this.serializedGuid = this.guid.ToByteArray();
			flag = true;
		}
		else if (this.guid == Guid.Empty)
		{
			this.guid = new Guid(this.serializedGuid);
		}
		if (this.guid != Guid.Empty)
		{
			if (!GuidManager.Add(this))
			{
				this.serializedGuid = null;
				this.guid = Guid.Empty;
				this.CreateGuid();
				return;
			}
			if (flag)
			{
				this.didJustAssignNewGUID = true;
			}
		}
	}

	// Token: 0x06000004 RID: 4 RVA: 0x00002137 File Offset: 0x00000337
	public void OnBeforeSerialize()
	{
		if (this.guid != Guid.Empty)
		{
			this.serializedGuid = this.guid.ToByteArray();
		}
	}

	// Token: 0x06000005 RID: 5 RVA: 0x0000215C File Offset: 0x0000035C
	public void OnAfterDeserialize()
	{
		if (this.serializedGuid != null && this.serializedGuid.Length == 16)
		{
			this.guid = new Guid(this.serializedGuid);
		}
	}

	// Token: 0x06000006 RID: 6 RVA: 0x00002183 File Offset: 0x00000383
	private void Awake()
	{
		this.ManualAwake();
	}

	// Token: 0x06000007 RID: 7 RVA: 0x0000218B File Offset: 0x0000038B
	private void OnValidate()
	{
		this.ManualValidate();
	}

	// Token: 0x06000008 RID: 8 RVA: 0x00002193 File Offset: 0x00000393
	private void ManualAwake()
	{
		this.CreateGuid();
	}

	// Token: 0x06000009 RID: 9 RVA: 0x0000219B File Offset: 0x0000039B
	private void ManualValidate()
	{
		this.CreateGuid();
	}

	// Token: 0x0600000A RID: 10 RVA: 0x000021A3 File Offset: 0x000003A3
	private void OnDestroy()
	{
		GuidManager.Remove(this.guid);
	}

	// Token: 0x04000001 RID: 1
	public Action OnSetGUID;

	// Token: 0x04000002 RID: 2
	[NonSerialized]
	public bool didJustAssignNewGUID;

	// Token: 0x04000003 RID: 3
	private Guid guid = Guid.Empty;

	// Token: 0x04000004 RID: 4
	[SerializeField]
	[HideInInspector]
	private byte[] serializedGuid;
}
