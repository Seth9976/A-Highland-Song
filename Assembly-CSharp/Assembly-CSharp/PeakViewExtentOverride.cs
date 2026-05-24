using System;
using UnityEngine;

// Token: 0x0200019B RID: 411
public class PeakViewExtentOverride : MonoBehaviour
{
	// Token: 0x04001043 RID: 4163
	[Info("Zero in min or max means don't override. Default is currently -3000 to +3000 for major peaks, -800 to +800 for minor")]
	public Range extent;
}
