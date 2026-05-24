using System;
using UnityEngine;

// Token: 0x02000221 RID: 545
public class AudioSourceAutoDestroy : MonoBehaviour
{
	// Token: 0x060013A0 RID: 5024 RVA: 0x00089B4B File Offset: 0x00087D4B
	public void Start()
	{
		this.audioSource = base.GetComponent<AudioSource>();
	}

	// Token: 0x060013A1 RID: 5025 RVA: 0x00089B5C File Offset: 0x00087D5C
	public void Update()
	{
		if (this.audioSource && !this.audioSource.isPlaying)
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

	// Token: 0x040012D3 RID: 4819
	private AudioSource audioSource;
}
