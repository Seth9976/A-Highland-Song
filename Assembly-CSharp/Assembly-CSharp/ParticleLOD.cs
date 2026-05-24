using System;

// Token: 0x020001C5 RID: 453
[Serializable]
public struct ParticleLOD
{
	// Token: 0x06000EFA RID: 3834 RVA: 0x000739B1 File Offset: 0x00071BB1
	public ParticleLOD(float normalizedTransitionDistance, ParticleSystemAlphaController[] renderers)
	{
		this.normalizedTransitionDistance = normalizedTransitionDistance;
		this.fadeTransitionWidth = 0f;
		this.renderers = renderers;
	}

	// Token: 0x040011AF RID: 4527
	public float normalizedTransitionDistance;

	// Token: 0x040011B0 RID: 4528
	public float fadeTransitionWidth;

	// Token: 0x040011B1 RID: 4529
	public ParticleSystemAlphaController[] renderers;
}
