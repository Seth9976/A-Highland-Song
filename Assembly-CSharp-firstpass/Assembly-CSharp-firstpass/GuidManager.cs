using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000003 RID: 3
public class GuidManager
{
	// Token: 0x0600000C RID: 12 RVA: 0x000021C3 File Offset: 0x000003C3
	public static bool Add(GuidComponent guidComponent)
	{
		if (GuidManager.Instance == null)
		{
			GuidManager.Instance = new GuidManager();
		}
		return GuidManager.Instance.InternalAdd(guidComponent);
	}

	// Token: 0x0600000D RID: 13 RVA: 0x000021E1 File Offset: 0x000003E1
	public static void Remove(Guid guid)
	{
		if (GuidManager.Instance == null)
		{
			GuidManager.Instance = new GuidManager();
		}
		GuidManager.Instance.InternalRemove(guid);
	}

	// Token: 0x0600000E RID: 14 RVA: 0x000021FF File Offset: 0x000003FF
	public static GameObject ResolveGuid(Guid guid, Action<GameObject> onAddCallback, Action onRemoveCallback)
	{
		if (GuidManager.Instance == null)
		{
			GuidManager.Instance = new GuidManager();
		}
		return GuidManager.Instance.ResolveGuidInternal(guid, onAddCallback, onRemoveCallback);
	}

	// Token: 0x0600000F RID: 15 RVA: 0x0000221F File Offset: 0x0000041F
	public static GameObject ResolveGuid(Guid guid, Action onDestroyCallback)
	{
		if (GuidManager.Instance == null)
		{
			GuidManager.Instance = new GuidManager();
		}
		return GuidManager.Instance.ResolveGuidInternal(guid, null, onDestroyCallback);
	}

	// Token: 0x06000010 RID: 16 RVA: 0x0000223F File Offset: 0x0000043F
	public static GameObject ResolveGuid(Guid guid)
	{
		if (GuidManager.Instance == null)
		{
			GuidManager.Instance = new GuidManager();
		}
		return GuidManager.Instance.ResolveGuidInternal(guid, null, null);
	}

	// Token: 0x06000011 RID: 17 RVA: 0x0000225F File Offset: 0x0000045F
	private GuidManager()
	{
		this.guidToObjectMap = new Dictionary<Guid, GuidManager.GuidInfo>();
	}

	// Token: 0x06000012 RID: 18 RVA: 0x00002274 File Offset: 0x00000474
	private bool InternalAdd(GuidComponent guidComponent)
	{
		Guid guid = guidComponent.GetGuid();
		GuidManager.GuidInfo guidInfo = new GuidManager.GuidInfo(guidComponent);
		if (!this.guidToObjectMap.ContainsKey(guid))
		{
			this.guidToObjectMap.Add(guid, guidInfo);
			return true;
		}
		GuidManager.GuidInfo guidInfo2 = this.guidToObjectMap[guid];
		if (guidInfo2.go != null && guidInfo2.go != guidComponent.gameObject)
		{
			return false;
		}
		guidInfo2.go = guidInfo.go;
		guidInfo2.HandleAddCallback();
		this.guidToObjectMap[guid] = guidInfo2;
		return true;
	}

	// Token: 0x06000013 RID: 19 RVA: 0x00002300 File Offset: 0x00000500
	private void InternalRemove(Guid guid)
	{
		GuidManager.GuidInfo guidInfo;
		if (this.guidToObjectMap.TryGetValue(guid, out guidInfo))
		{
			guidInfo.HandleRemoveCallback();
		}
		this.guidToObjectMap.Remove(guid);
	}

	// Token: 0x06000014 RID: 20 RVA: 0x00002334 File Offset: 0x00000534
	private GameObject ResolveGuidInternal(Guid guid, Action<GameObject> onAddCallback, Action onRemoveCallback)
	{
		GuidManager.GuidInfo guidInfo;
		if (this.guidToObjectMap.TryGetValue(guid, out guidInfo))
		{
			if (onAddCallback != null)
			{
				guidInfo.OnAdd += onAddCallback;
			}
			if (onRemoveCallback != null)
			{
				guidInfo.OnRemove += onRemoveCallback;
			}
			this.guidToObjectMap[guid] = guidInfo;
			return guidInfo.go;
		}
		if (onAddCallback != null)
		{
			guidInfo.OnAdd += onAddCallback;
		}
		if (onRemoveCallback != null)
		{
			guidInfo.OnRemove += onRemoveCallback;
		}
		this.guidToObjectMap.Add(guid, guidInfo);
		return null;
	}

	// Token: 0x04000005 RID: 5
	private static GuidManager Instance;

	// Token: 0x04000006 RID: 6
	private Dictionary<Guid, GuidManager.GuidInfo> guidToObjectMap;

	// Token: 0x02000213 RID: 531
	private struct GuidInfo
	{
		// Token: 0x14000010 RID: 16
		// (add) Token: 0x0600093F RID: 2367 RVA: 0x00053090 File Offset: 0x00051290
		// (remove) Token: 0x06000940 RID: 2368 RVA: 0x000530C8 File Offset: 0x000512C8
		public event Action<GameObject> OnAdd;

		// Token: 0x14000011 RID: 17
		// (add) Token: 0x06000941 RID: 2369 RVA: 0x00053100 File Offset: 0x00051300
		// (remove) Token: 0x06000942 RID: 2370 RVA: 0x00053138 File Offset: 0x00051338
		public event Action OnRemove;

		// Token: 0x06000943 RID: 2371 RVA: 0x0005316D File Offset: 0x0005136D
		public GuidInfo(GuidComponent comp)
		{
			this.go = comp.gameObject;
			this.OnRemove = null;
			this.OnAdd = null;
		}

		// Token: 0x06000944 RID: 2372 RVA: 0x00053189 File Offset: 0x00051389
		public void HandleAddCallback()
		{
			if (this.OnAdd != null)
			{
				this.OnAdd(this.go);
			}
		}

		// Token: 0x06000945 RID: 2373 RVA: 0x000531A4 File Offset: 0x000513A4
		public void HandleRemoveCallback()
		{
			if (this.OnRemove != null)
			{
				this.OnRemove();
			}
		}

		// Token: 0x0400049B RID: 1179
		public GameObject go;
	}
}
