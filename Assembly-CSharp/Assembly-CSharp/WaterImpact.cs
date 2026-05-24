using System;
using UnityEngine;

// Token: 0x020001B4 RID: 436
public class WaterImpact : MonoBehaviour
{
	// Token: 0x06000E69 RID: 3689 RVA: 0x000720BD File Offset: 0x000702BD
	private void OnEnable()
	{
		this._lastRippleTime = Time.time;
		this.particles.Clear();
	}

	// Token: 0x06000E6A RID: 3690 RVA: 0x000720D5 File Offset: 0x000702D5
	private void OnDisable()
	{
		this.particles.Stop();
	}

	// Token: 0x06000E6B RID: 3691 RVA: 0x000720E4 File Offset: 0x000702E4
	private void Update()
	{
		if (base.transform.position.y < 0f)
		{
			Vector3 position = this.particles.transform.position;
			position.y = 0f;
			this.particles.transform.position = position;
			if (!this.particles.isPlaying)
			{
				this.particles.Play();
			}
			if (this.ripplesPerSecond > 0f)
			{
				float num = 1f / this.ripplesPerSecond;
				if (Time.time > this._lastRippleTime + num)
				{
					MonoSingleton<WaterRippleManager>.instance.CreateRipple(position);
					this._lastRippleTime = Time.time;
					return;
				}
			}
		}
		else
		{
			this.particles.Stop();
		}
	}

	// Token: 0x0400114B RID: 4427
	public float ripplesPerSecond = 2f;

	// Token: 0x0400114C RID: 4428
	public ParticleSystem particles;

	// Token: 0x0400114D RID: 4429
	private float _lastRippleTime;
}
