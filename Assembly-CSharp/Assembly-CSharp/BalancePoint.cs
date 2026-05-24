using System;
using UnityEngine;

// Token: 0x02000169 RID: 361
public class BalancePoint : MonoBehaviour
{
	// Token: 0x170002CE RID: 718
	// (get) Token: 0x06000C15 RID: 3093 RVA: 0x00060880 File Offset: 0x0005EA80
	public Slope slopeUnder
	{
		get
		{
			if (this._slopeUnder == null)
			{
				Debug.LogWarning("BalancePoint didn't have a valid _slopeUnder reference! Try rebuilding the level? This was supposed to have be found at build time for efficiency.", this);
				SlopeSample slopeSample;
				if (Raycast.SampleWithDepthRange(base.transform.position + 0.2f * Vector3.up, base.transform.position - Vector3.up, base.transform.position.z, Range.Centered(base.transform.position.z, 0.8f), out slopeSample, default(Color)).didHit)
				{
					this._slopeUnder = slopeSample.slope;
				}
				else
				{
					Debug.LogError("BalancePoint couldn't find any valid Slope underneath at runtime either!", this);
				}
			}
			return this._slopeUnder;
		}
	}

	// Token: 0x04000E5D RID: 3677
	public bool isWallClimb;

	// Token: 0x04000E5E RID: 3678
	public bool exitToSlope;

	// Token: 0x04000E5F RID: 3679
	[SerializeField]
	private Slope _slopeUnder;

	// Token: 0x04000E60 RID: 3680
	private const float gizmosJumpRadius = 5f;
}
