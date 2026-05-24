using System;
using UnityEngine;

// Token: 0x02000180 RID: 384
[Serializable]
public class CreatureExit
{
	// Token: 0x04000F70 RID: 3952
	public Vector3 localExitPos;

	// Token: 0x04000F71 RID: 3953
	public int direction = 1;

	// Token: 0x04000F72 RID: 3954
	public CreatureExitType exitType;

	// Token: 0x04000F73 RID: 3955
	public CreatureExitCondition condition;

	// Token: 0x04000F74 RID: 3956
	public CreatureZone destinationZone;
}
