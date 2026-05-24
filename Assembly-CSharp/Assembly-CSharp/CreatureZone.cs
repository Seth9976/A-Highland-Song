using System;
using UnityEngine;

// Token: 0x02000182 RID: 386
public class CreatureZone : MonoBehaviour
{
	// Token: 0x170002E7 RID: 743
	// (get) Token: 0x06000CB0 RID: 3248 RVA: 0x00066106 File Offset: 0x00064306
	public Vector3 entryPos
	{
		get
		{
			return base.transform.position + this.entry.localEntryPos;
		}
	}

	// Token: 0x170002E8 RID: 744
	// (get) Token: 0x06000CB1 RID: 3249 RVA: 0x00066123 File Offset: 0x00064323
	public Vector3 exitPos
	{
		get
		{
			return base.transform.position + this.exit.localExitPos;
		}
	}

	// Token: 0x170002E9 RID: 745
	// (get) Token: 0x06000CB2 RID: 3250 RVA: 0x00066140 File Offset: 0x00064340
	public Range range
	{
		get
		{
			return Range.Centered(base.transform.position.x, 2f * this.radius);
		}
	}

	// Token: 0x06000CB3 RID: 3251 RVA: 0x00066163 File Offset: 0x00064363
	public bool Contains(Vector2 pos)
	{
		return Vector2.Distance(base.transform.position, pos) < this.radius;
	}

	// Token: 0x06000CB4 RID: 3252 RVA: 0x00066184 File Offset: 0x00064384
	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.white;
		Gizmos.DrawWireSphere(base.transform.position, this.radius);
		if (this.entry.entryType != CreatureEntryType.None)
		{
			Gizmos.color = Color.green;
			Gizmos.DrawSphere(this.entryPos, 1f);
		}
		if (this.exit.condition != CreatureExitCondition.None)
		{
			Gizmos.color = Color.red;
			Gizmos.DrawSphere(this.exitPos, 1f);
		}
	}

	// Token: 0x04000F78 RID: 3960
	public float radius = 10f;

	// Token: 0x04000F79 RID: 3961
	public Creature.State state;

	// Token: 0x04000F7A RID: 3962
	public CreatureEntry entry;

	// Token: 0x04000F7B RID: 3963
	public CreatureExit exit;
}
