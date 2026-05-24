using System;
using UnityEngine;

// Token: 0x0200002C RID: 44
[ExecuteInEditMode]
public class GlobalShaderParamSetter : MonoBehaviour
{
	// Token: 0x06000127 RID: 295 RVA: 0x0000C6F7 File Offset: 0x0000A8F7
	private void Update()
	{
		this.Refresh();
	}

	// Token: 0x06000128 RID: 296 RVA: 0x0000C700 File Offset: 0x0000A900
	public void Refresh()
	{
		if (!Game.loaded)
		{
			return;
		}
		Shader.SetGlobalFloat("_Global_HourOfDay", GameClock.instance.hourOfDay);
		float num = 0f;
		if (Application.isPlaying)
		{
			num = Time.time;
		}
		Shader.SetGlobalFloat("_Global_VisualEffectsTime", num);
	}

	// Token: 0x040001EE RID: 494
	private float _visualEffectsTime;
}
