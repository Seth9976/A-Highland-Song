using System;
using UnityEngine;

// Token: 0x020000CD RID: 205
[ExecuteInEditMode]
public class RunnerSingletonApplier : MonoBehaviour
{
	// Token: 0x06000765 RID: 1893 RVA: 0x0004253E File Offset: 0x0004073E
	private void OnEnable()
	{
		GSR.SetRunner(this.runner);
	}

	// Token: 0x04000841 RID: 2113
	public Runner runner;
}
