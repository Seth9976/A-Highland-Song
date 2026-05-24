using System;
using UnityEngine;

// Token: 0x0200018B RID: 395
public class GeneralSplashParticles : MonoSingleton<GeneralSplashParticles>
{
	// Token: 0x06000CEC RID: 3308 RVA: 0x000679B0 File Offset: 0x00065BB0
	public static void Emit(int count, Vector3 pos)
	{
		ParticleSystem particles = MonoSingleton<GeneralSplashParticles>.instance.particles;
		particles.transform.position = pos;
		particles.Emit(count);
	}

	// Token: 0x170002FB RID: 763
	// (get) Token: 0x06000CED RID: 3309 RVA: 0x000679CE File Offset: 0x00065BCE
	private ParticleSystem particles
	{
		get
		{
			if (this._particles == null)
			{
				this._particles = base.GetComponent<ParticleSystem>();
			}
			return this._particles;
		}
	}

	// Token: 0x04000FC2 RID: 4034
	private ParticleSystem _particles;
}
