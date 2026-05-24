using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020001E9 RID: 489
[Serializable]
public class CameraShotGeneratorProperties
{
	// Token: 0x1700040B RID: 1035
	// (get) Token: 0x06001151 RID: 4433 RVA: 0x000805C0 File Offset: 0x0007E7C0
	public bool isValid
	{
		get
		{
			return this.pointCloud.Count != 0 && (this.fitHorizontally || this.fitVertically) && this.fieldOfView > 0f && (this.rotation.x != 0f || this.rotation.y != 0f || this.rotation.z != 0f || this.rotation.w != 0f);
		}
	}

	// Token: 0x06001153 RID: 4435 RVA: 0x000806B4 File Offset: 0x0007E8B4
	public SerializableCamera ToShot()
	{
		return this.ToShot(Camera.main);
	}

	// Token: 0x06001154 RID: 4436 RVA: 0x000806C4 File Offset: 0x0007E8C4
	public SerializableCamera ToShot(Camera camera)
	{
		SerializableCamera serializableCamera = new SerializableCamera(camera);
		serializableCamera.orthographic = false;
		CameraShotGeneratorTools.CreateCameraShot(this, ref serializableCamera);
		return serializableCamera;
	}

	// Token: 0x06001155 RID: 4437 RVA: 0x000806EC File Offset: 0x0007E8EC
	public override string ToString()
	{
		return string.Format("[CameraShotGeneratorProperties: fieldOfView={0}, zoom={1}, rotation={2}, fitHorizontally={3}, fitVertically={4}, isValid={5}]", new object[] { this.fieldOfView, this.zoom, this.rotation, this.fitHorizontally, this.fitVertically, this.isValid });
	}

	// Token: 0x04001267 RID: 4711
	public Rect viewportRect = new Rect(0f, 0f, 1f, 1f);

	// Token: 0x04001268 RID: 4712
	public List<Vector3> pointCloud = new List<Vector3>();

	// Token: 0x04001269 RID: 4713
	public Quaternion rotation = Quaternion.identity;

	// Token: 0x0400126A RID: 4714
	public bool orthographic;

	// Token: 0x0400126B RID: 4715
	public float fieldOfView = 60f;

	// Token: 0x0400126C RID: 4716
	public float zoom = 1f;

	// Token: 0x0400126D RID: 4717
	public bool fitHorizontally = true;

	// Token: 0x0400126E RID: 4718
	public bool fitVertically = true;
}
