using System;
using System.Collections.Generic;

// Token: 0x020001E4 RID: 484
[Serializable]
public class CameraPropertiesBuilderQueue
{
	// Token: 0x0600111B RID: 4379 RVA: 0x0007E98C File Offset: 0x0007CB8C
	public void Add(CameraPropertiesBuilderQueue.UpdateCameraPropertiesDelegate updateCameraPropertiesDelegate, CameraPropertiesBuilderQueue.ModifyCameraPropertiesDelegate setCameraPropertiesDelegate)
	{
		this.modifiers.Add(new CameraPropertiesBuilderQueue.SetCameraPropertiesDelegateQueueItem(this.modifiers.Count, updateCameraPropertiesDelegate, setCameraPropertiesDelegate));
	}

	// Token: 0x0600111C RID: 4380 RVA: 0x0007E9AB File Offset: 0x0007CBAB
	public void Add(CameraPropertiesBuilderQueue.UpdateCameraPropertiesDelegate updateCameraPropertiesDelegate, CameraPropertiesBuilderQueue.ModifyCameraPropertiesDelegate setCameraPropertiesDelegate, int sortIndex)
	{
		this.modifiers.Add(new CameraPropertiesBuilderQueue.SetCameraPropertiesDelegateQueueItem(sortIndex, updateCameraPropertiesDelegate, setCameraPropertiesDelegate));
		this.modifiers.Sort((CameraPropertiesBuilderQueue.SetCameraPropertiesDelegateQueueItem x, CameraPropertiesBuilderQueue.SetCameraPropertiesDelegateQueueItem y) => x.sortIndex.CompareTo(y.sortIndex));
	}

	// Token: 0x0600111D RID: 4381 RVA: 0x0007E9EC File Offset: 0x0007CBEC
	public void Add(CameraPropertiesBuilderQueue.UpdateCameraPropertiesDelegate updateCameraPropertiesDelegate, CameraPropertiesBuilderQueue.ModifyCameraPropertiesDelegate setCameraPropertiesDelegate, int sortIndex, string name)
	{
		CameraPropertiesBuilderQueue.SetCameraPropertiesDelegateQueueItem setCameraPropertiesDelegateQueueItem = new CameraPropertiesBuilderQueue.SetCameraPropertiesDelegateQueueItem(sortIndex, updateCameraPropertiesDelegate, setCameraPropertiesDelegate);
		setCameraPropertiesDelegateQueueItem.name = name;
		this.modifiers.Add(setCameraPropertiesDelegateQueueItem);
		this.modifiers.Sort((CameraPropertiesBuilderQueue.SetCameraPropertiesDelegateQueueItem x, CameraPropertiesBuilderQueue.SetCameraPropertiesDelegateQueueItem y) => x.sortIndex.CompareTo(y.sortIndex));
	}

	// Token: 0x0600111E RID: 4382 RVA: 0x0007EA40 File Offset: 0x0007CC40
	public bool Remove(CameraPropertiesBuilderQueue.ModifyCameraPropertiesDelegate setCameraPropertiesDelegate)
	{
		for (int i = this.modifiers.Count - 1; i >= 0; i--)
		{
			if (this.modifiers[i].setCameraPropertiesDelegate == setCameraPropertiesDelegate)
			{
				this.modifiers.RemoveAt(i);
				return true;
			}
		}
		return false;
	}

	// Token: 0x0600111F RID: 4383 RVA: 0x0007EA90 File Offset: 0x0007CC90
	public void Update(float deltaTime)
	{
		foreach (CameraPropertiesBuilderQueue.SetCameraPropertiesDelegateQueueItem setCameraPropertiesDelegateQueueItem in this.modifiers)
		{
			setCameraPropertiesDelegateQueueItem.updateCameraPropertiesDelegate(deltaTime);
		}
	}

	// Token: 0x06001120 RID: 4384 RVA: 0x0007EAE8 File Offset: 0x0007CCE8
	public void Generate(ref CameraProperties properties)
	{
		foreach (CameraPropertiesBuilderQueue.SetCameraPropertiesDelegateQueueItem setCameraPropertiesDelegateQueueItem in this.modifiers)
		{
			setCameraPropertiesDelegateQueueItem.setCameraPropertiesDelegate(ref properties);
		}
	}

	// Token: 0x0400125F RID: 4703
	private List<CameraPropertiesBuilderQueue.SetCameraPropertiesDelegateQueueItem> modifiers = new List<CameraPropertiesBuilderQueue.SetCameraPropertiesDelegateQueueItem>();

	// Token: 0x020003F6 RID: 1014
	// (Invoke) Token: 0x060018C6 RID: 6342
	public delegate void UpdateCameraPropertiesDelegate(float deltaTime);

	// Token: 0x020003F7 RID: 1015
	// (Invoke) Token: 0x060018CA RID: 6346
	public delegate void ModifyCameraPropertiesDelegate(ref CameraProperties SpaceCameraProperties);

	// Token: 0x020003F8 RID: 1016
	[Serializable]
	private class SetCameraPropertiesDelegateQueueItem
	{
		// Token: 0x17000507 RID: 1287
		// (get) Token: 0x060018CD RID: 6349 RVA: 0x0009F23C File Offset: 0x0009D43C
		// (set) Token: 0x060018CE RID: 6350 RVA: 0x0009F244 File Offset: 0x0009D444
		public int sortIndex { get; private set; }

		// Token: 0x17000508 RID: 1288
		// (get) Token: 0x060018CF RID: 6351 RVA: 0x0009F24D File Offset: 0x0009D44D
		// (set) Token: 0x060018D0 RID: 6352 RVA: 0x0009F255 File Offset: 0x0009D455
		public CameraPropertiesBuilderQueue.UpdateCameraPropertiesDelegate updateCameraPropertiesDelegate { get; private set; }

		// Token: 0x17000509 RID: 1289
		// (get) Token: 0x060018D1 RID: 6353 RVA: 0x0009F25E File Offset: 0x0009D45E
		// (set) Token: 0x060018D2 RID: 6354 RVA: 0x0009F266 File Offset: 0x0009D466
		public CameraPropertiesBuilderQueue.ModifyCameraPropertiesDelegate setCameraPropertiesDelegate { get; private set; }

		// Token: 0x060018D3 RID: 6355 RVA: 0x0009F26F File Offset: 0x0009D46F
		public SetCameraPropertiesDelegateQueueItem(int sortIndex, CameraPropertiesBuilderQueue.UpdateCameraPropertiesDelegate updateCameraPropertiesDelegate, CameraPropertiesBuilderQueue.ModifyCameraPropertiesDelegate setCameraPropertiesDelegate)
		{
			this.sortIndex = sortIndex;
			this.updateCameraPropertiesDelegate = updateCameraPropertiesDelegate;
			this.setCameraPropertiesDelegate = setCameraPropertiesDelegate;
		}

		// Token: 0x04001AB0 RID: 6832
		public string name;
	}
}
