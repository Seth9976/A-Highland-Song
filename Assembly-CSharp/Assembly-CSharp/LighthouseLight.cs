using System;
using UnityEngine;

// Token: 0x02000192 RID: 402
public class LighthouseLight : MonoBehaviour
{
	// Token: 0x06000D37 RID: 3383 RVA: 0x00069BD8 File Offset: 0x00067DD8
	private void Update()
	{
		float num = Time.time / this.duration;
		float hourOfDay = GameClock.instance.hourOfDay;
		float num2;
		if (hourOfDay >= 17f)
		{
			num2 = Mathf.InverseLerp(17f, 22f, hourOfDay);
		}
		else
		{
			num2 = 1f - Mathf.InverseLerp(6f, 8f, hourOfDay);
		}
		foreach (LighthouseLight.Light light in this._lights)
		{
			float num3 = (num + light.setting.tNormOffset) % 1f;
			Color color = light.sprite.color;
			float num4 = light.setting.alpha.Evaluate(num3);
			float num5 = light.setting.alphaNight.Evaluate(num2);
			color.a = num4 * num5;
			light.sprite.color = color;
			Vector3 vector = new Vector3(light.setting.scaleX.Evaluate(num3), light.setting.scaleY.Evaluate(num3), 1f);
			light.transform.localScale = vector;
		}
	}

	// Token: 0x0400100F RID: 4111
	public float duration = 6f;

	// Token: 0x04001010 RID: 4112
	public const float lateStartHour = 17f;

	// Token: 0x04001011 RID: 4113
	public const float lateEndHour = 22f;

	// Token: 0x04001012 RID: 4114
	public const float dawnStartHour = 6f;

	// Token: 0x04001013 RID: 4115
	public const float dawnEndHour = 8f;

	// Token: 0x04001014 RID: 4116
	[SerializeField]
	private LighthouseLight.Light[] _lights;

	// Token: 0x020003AD RID: 941
	[Serializable]
	public struct Light
	{
		// Token: 0x040019BA RID: 6586
		public Transform transform;

		// Token: 0x040019BB RID: 6587
		public SpriteRenderer sprite;

		// Token: 0x040019BC RID: 6588
		public LighthouseLightSettings setting;
	}
}
