using System;
using UnityEngine;

// Token: 0x02000195 RID: 405
public class LODItem : MonoBehaviour
{
	// Token: 0x04001020 RID: 4128
	[Info("0: only visible on current level\n1: only visible from curr + next level\n2: visible on curr level+1 and curr level+2")]
	public int levelVisibilityRange = 1;
}
