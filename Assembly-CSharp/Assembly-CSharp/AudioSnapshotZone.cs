using System;
using UnityEngine;
using UnityEngine.Audio;

// Token: 0x0200000A RID: 10
[RequireComponent(typeof(Region))]
public class AudioSnapshotZone : MonoBehaviour
{
	// Token: 0x1700000B RID: 11
	// (get) Token: 0x06000035 RID: 53 RVA: 0x00005526 File Offset: 0x00003726
	private Region region
	{
		get
		{
			return base.GetComponent<Region>();
		}
	}

	// Token: 0x06000036 RID: 54 RVA: 0x00005530 File Offset: 0x00003730
	private void Update()
	{
		bool flag = this.region.ContainsPoint(Runner.instance.transform.position);
		if (this.active != flag)
		{
			this.active = flag;
			if (this.active)
			{
				this.snapshot.TransitionTo(1f);
				return;
			}
			AudioController.instance.audioMixer.FindSnapshot("Default").TransitionTo(1f);
		}
	}

	// Token: 0x04000041 RID: 65
	public AudioMixerSnapshot snapshot;

	// Token: 0x04000042 RID: 66
	public bool active;
}
