using System;
using UnityEngine;

// Token: 0x02000005 RID: 5
[Serializable]
public class GuidReference : ISerializationCallbackReceiver
{
	// Token: 0x17000002 RID: 2
	// (get) Token: 0x06000020 RID: 32 RVA: 0x00002560 File Offset: 0x00000760
	public bool hasAssignedGUID
	{
		get
		{
			return this.guid != Guid.Empty;
		}
	}

	// Token: 0x17000003 RID: 3
	// (get) Token: 0x06000021 RID: 33 RVA: 0x00002574 File Offset: 0x00000774
	// (set) Token: 0x06000022 RID: 34 RVA: 0x000025C8 File Offset: 0x000007C8
	public GameObject gameObject
	{
		get
		{
			if (this.isCacheSet && this.cachedReference != null)
			{
				return this.cachedReference;
			}
			this.cachedReference = GuidManager.ResolveGuid(this.guid, this.addDelegate, this.removeDelegate);
			this.isCacheSet = true;
			return this.cachedReference;
		}
		private set
		{
		}
	}

	// Token: 0x06000023 RID: 35 RVA: 0x000025CA File Offset: 0x000007CA
	public GuidReference()
	{
	}

	// Token: 0x06000024 RID: 36 RVA: 0x000025D2 File Offset: 0x000007D2
	public GuidReference(GuidComponent target)
	{
		this.guid = target.GetGuid();
	}

	// Token: 0x06000025 RID: 37 RVA: 0x000025E6 File Offset: 0x000007E6
	private void GuidAdded(GameObject go)
	{
		this.isCacheSet = true;
		this.cachedReference = go;
	}

	// Token: 0x06000026 RID: 38 RVA: 0x000025F6 File Offset: 0x000007F6
	private void GuidRemoved()
	{
		this.cachedReference = null;
		this.isCacheSet = false;
	}

	// Token: 0x06000027 RID: 39 RVA: 0x00002606 File Offset: 0x00000806
	public void OnBeforeSerialize()
	{
		this.serializedGuid = this.guid.ToByteArray();
	}

	// Token: 0x06000028 RID: 40 RVA: 0x0000261C File Offset: 0x0000081C
	public void OnAfterDeserialize()
	{
		this.cachedReference = null;
		this.isCacheSet = false;
		if (this.serializedGuid == null || this.serializedGuid.Length != 16)
		{
			this.serializedGuid = new byte[16];
		}
		this.guid = new Guid(this.serializedGuid);
		this.addDelegate = new Action<GameObject>(this.GuidAdded);
		this.removeDelegate = new Action(this.GuidRemoved);
	}

	// Token: 0x0400000C RID: 12
	private GameObject cachedReference;

	// Token: 0x0400000D RID: 13
	private bool isCacheSet;

	// Token: 0x0400000E RID: 14
	[SerializeField]
	private byte[] serializedGuid;

	// Token: 0x0400000F RID: 15
	private Guid guid;

	// Token: 0x04000010 RID: 16
	private Action<GameObject> addDelegate;

	// Token: 0x04000011 RID: 17
	private Action removeDelegate;
}
