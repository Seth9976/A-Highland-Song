using System;
using UnityEngine;

// Token: 0x0200011E RID: 286
public class LitPeakRing : MonoBehaviour
{
	// Token: 0x060009A3 RID: 2467 RVA: 0x00051439 File Offset: 0x0004F639
	private void Update()
	{
		base.transform.Rotate(0f, 0f, this.speed * Time.unscaledDeltaTime);
	}

	// Token: 0x04000BB5 RID: 2997
	public float speed = 1f;
}
