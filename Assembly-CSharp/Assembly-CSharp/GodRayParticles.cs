using System;
using UnityEngine;

// Token: 0x02000159 RID: 345
public class GodRayParticles : MonoBehaviour
{
	// Token: 0x170002BD RID: 701
	// (get) Token: 0x06000BB8 RID: 3000 RVA: 0x0005E595 File Offset: 0x0005C795
	public ParticleSystem[] particles
	{
		get
		{
			if (this._particles == null)
			{
				this._particles = base.GetComponentsInChildren<ParticleSystem>();
			}
			return this._particles;
		}
	}

	// Token: 0x04000DDB RID: 3547
	private ParticleSystem[] _particles;
}
