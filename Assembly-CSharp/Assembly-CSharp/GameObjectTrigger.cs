using System;
using UnityEngine;

// Token: 0x0200018A RID: 394
public class GameObjectTrigger : MonoBehaviour
{
	// Token: 0x06000CEA RID: 3306 RVA: 0x0006794C File Offset: 0x00065B4C
	private void Update()
	{
		this._triggered = this.zone.triggering || (this._triggered && this.stayTriggered);
		bool flag = this._triggered ^ this.invert;
		if (this.target.activeSelf != flag)
		{
			this.target.SetActive(flag);
		}
	}

	// Token: 0x04000FBD RID: 4029
	public GameObject target;

	// Token: 0x04000FBE RID: 4030
	public TriggerZone zone;

	// Token: 0x04000FBF RID: 4031
	public bool invert;

	// Token: 0x04000FC0 RID: 4032
	public bool stayTriggered;

	// Token: 0x04000FC1 RID: 4033
	private bool _triggered;
}
