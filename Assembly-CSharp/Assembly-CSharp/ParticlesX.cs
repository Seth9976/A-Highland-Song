using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020001EE RID: 494
public static class ParticlesX
{
	// Token: 0x060011E6 RID: 4582 RVA: 0x000831A2 File Offset: 0x000813A2
	public static void SetActive(ParticleSystem particles, bool active)
	{
		if (active)
		{
			if (!particles.IsAlive())
			{
				particles.Clear();
			}
			if (!particles.isPlaying)
			{
				particles.Play();
				return;
			}
		}
		else
		{
			particles.Stop();
		}
	}

	// Token: 0x060011E7 RID: 4583 RVA: 0x000831CA File Offset: 0x000813CA
	public static void StopAndReturnToPool(ParticleSystem particles)
	{
		particles.Stop();
		if (!ParticlesX._awaitingReturnToPool.Contains(particles))
		{
			ParticlesX._awaitingReturnToPool.Add(particles);
		}
	}

	// Token: 0x060011E8 RID: 4584 RVA: 0x000831EA File Offset: 0x000813EA
	public static void UpdateParticlesAwaitingReturnToPool()
	{
		ParticlesX._awaitingReturnToPool.UpdateAndRemoveIf(delegate(ParticleSystem particles)
		{
			particles.Stop();
			if (!particles.IsAlive())
			{
				particles.GetComponent<Prototype>().ReturnToPool();
				return true;
			}
			return false;
		});
	}

	// Token: 0x04001277 RID: 4727
	private static List<ParticleSystem> _awaitingReturnToPool = new List<ParticleSystem>();
}
