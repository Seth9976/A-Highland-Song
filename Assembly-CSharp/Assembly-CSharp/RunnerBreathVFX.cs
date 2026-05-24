using System;
using UnityEngine;

// Token: 0x020000C6 RID: 198
public class RunnerBreathVFX : MonoBehaviour
{
	// Token: 0x06000629 RID: 1577 RVA: 0x00031753 File Offset: 0x0002F953
	private void OnEnable()
	{
		RunnerAudioManager runnerAudioManager = this.runnerAudioManager;
		runnerAudioManager.onBreatheOut = (Action<float>)Delegate.Combine(runnerAudioManager.onBreatheOut, new Action<float>(this.OnBreatheOut));
	}

	// Token: 0x0600062A RID: 1578 RVA: 0x0003177C File Offset: 0x0002F97C
	private void OnDisable()
	{
		RunnerAudioManager runnerAudioManager = this.runnerAudioManager;
		runnerAudioManager.onBreatheOut = (Action<float>)Delegate.Combine(runnerAudioManager.onBreatheOut, new Action<float>(this.OnBreatheOut));
	}

	// Token: 0x0600062B RID: 1579 RVA: 0x000317A5 File Offset: 0x0002F9A5
	private void Update()
	{
		base.transform.position = this.mouth.position;
	}

	// Token: 0x0600062C RID: 1580 RVA: 0x000317BD File Offset: 0x0002F9BD
	private void OnBreatheOut(float exhaustion)
	{
		this.prototype.Instantiate<ParticleSystem>(null);
	}

	// Token: 0x04000732 RID: 1842
	[Header("The audio script controls breathing")]
	public RunnerAudioManager runnerAudioManager;

	// Token: 0x04000733 RID: 1843
	public Prototype prototype;

	// Token: 0x04000734 RID: 1844
	public Transform mouth;
}
