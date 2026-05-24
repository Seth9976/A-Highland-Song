using System;
using UnityEngine;

// Token: 0x020001B3 RID: 435
[RequireComponent(typeof(TriggerZone))]
public class TutorialTrigger : MonoBehaviour
{
	// Token: 0x1700036D RID: 877
	// (get) Token: 0x06000E67 RID: 3687 RVA: 0x00072093 File Offset: 0x00070293
	public TriggerZone zone
	{
		get
		{
			if (this._zone == null)
			{
				this._zone = base.GetComponent<TriggerZone>();
			}
			return this._zone;
		}
	}

	// Token: 0x04001149 RID: 4425
	public TutorialId tutorialId;

	// Token: 0x0400114A RID: 4426
	private TriggerZone _zone;
}
