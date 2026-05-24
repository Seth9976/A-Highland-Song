using System;
using UnityEngine;

// Token: 0x02000223 RID: 547
public class ParticleSystemAutoDestroy : MonoBehaviour
{
	// Token: 0x060013BB RID: 5051 RVA: 0x00089EC3 File Offset: 0x000880C3
	public void Start()
	{
		this.ps = base.GetComponent<ParticleSystem>();
	}

	// Token: 0x060013BC RID: 5052 RVA: 0x00089ED4 File Offset: 0x000880D4
	public void Update()
	{
		if (this.ps && (!this.ps.IsAlive() || (!this.ps.emission.enabled && this.ps.particleCount == 0)))
		{
			Prototype component = base.GetComponent<Prototype>();
			if (component != null)
			{
				if (component.isOriginalPrototype)
				{
					return;
				}
				component.ReturnToPool();
				return;
			}
			else
			{
				Object.Destroy(base.gameObject);
			}
		}
	}

	// Token: 0x040012E8 RID: 4840
	private ParticleSystem ps;
}
