using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200001E RID: 30
public class CameraShakeDatabase : ScriptableObject
{
	// Token: 0x060000B1 RID: 177 RVA: 0x00009F44 File Offset: 0x00008144
	public CameraShake FindWithName(CameraShakeName shakeName)
	{
		foreach (CameraShake cameraShake in this.shakes)
		{
			if (cameraShake.shakeName == shakeName)
			{
				return cameraShake;
			}
		}
		Debug.LogWarning("No shake with name " + shakeName.ToString() + " found!");
		return null;
	}

	// Token: 0x0400010B RID: 267
	public List<CameraShake> shakes = new List<CameraShake>();
}
