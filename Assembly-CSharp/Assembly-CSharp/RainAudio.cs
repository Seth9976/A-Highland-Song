using System;
using UnityEngine;

// Token: 0x0200015D RID: 349
public class RainAudio : MonoBehaviour
{
	// Token: 0x06000BC5 RID: 3013 RVA: 0x0005EB8C File Offset: 0x0005CD8C
	private void OnEnable()
	{
		this.Refresh();
	}

	// Token: 0x06000BC6 RID: 3014 RVA: 0x0005EB94 File Offset: 0x0005CD94
	private void Update()
	{
		this.strength = Mathf.MoveTowards(this.strength, this.targetStrength, Time.unscaledDeltaTime);
		this.Refresh();
	}

	// Token: 0x06000BC7 RID: 3015 RVA: 0x0005EBB8 File Offset: 0x0005CDB8
	private void Refresh()
	{
		this.audioSource.volume = this.strength;
	}

	// Token: 0x04000DF1 RID: 3569
	public float strength;

	// Token: 0x04000DF2 RID: 3570
	public float targetStrength;

	// Token: 0x04000DF3 RID: 3571
	public AudioSource audioSource;
}
