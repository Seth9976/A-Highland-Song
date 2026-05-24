using System;
using UnityEngine;

// Token: 0x020001B1 RID: 433
public class TriggerZone : MonoInstancer<TriggerZone>
{
	// Token: 0x1700036B RID: 875
	// (get) Token: 0x06000E60 RID: 3680 RVA: 0x00071F8C File Offset: 0x0007018C
	public float attractRadius
	{
		get
		{
			return this.triggerRadius * 2.5f;
		}
	}

	// Token: 0x1700036C RID: 876
	// (get) Token: 0x06000E61 RID: 3681 RVA: 0x00071F9A File Offset: 0x0007019A
	public Prop prop
	{
		get
		{
			if (!this._searchedForProp)
			{
				this._searchedForProp = true;
				this._prop = base.GetComponent<Prop>();
			}
			return this._prop;
		}
	}

	// Token: 0x06000E62 RID: 3682 RVA: 0x00071FC0 File Offset: 0x000701C0
	public bool InsideTriggerDist(Vector2 position, float depth, float scalarMargin = 1f)
	{
		if (!base.gameObject.activeInHierarchy)
		{
			return false;
		}
		Vector3 position2 = base.transform.position;
		float num = Vector2.Distance(position, position2);
		float num2 = Mathf.Abs(position2.z - depth);
		return num < this.triggerRadius * scalarMargin && num2 < this.triggerDepthDiff * scalarMargin;
	}

	// Token: 0x06000E63 RID: 3683 RVA: 0x00072019 File Offset: 0x00070219
	public bool InsideAttractDist(Vector2 position, float depth)
	{
		return this.InsideRadiusOf(position, depth, this.attractRadius, this.triggerDepthDiff);
	}

	// Token: 0x06000E64 RID: 3684 RVA: 0x00072030 File Offset: 0x00070230
	public bool InsideRadiusOf(Vector2 position, float depth, float fixedRadius, float fixedDepth)
	{
		Vector3 position2 = base.transform.position;
		float num = Vector2.Distance(position, position2);
		float num2 = Mathf.Abs(position2.z - depth);
		return num < fixedRadius && num2 < fixedDepth;
	}

	// Token: 0x04001143 RID: 4419
	public float triggerRadius = 10f;

	// Token: 0x04001144 RID: 4420
	public float triggerDepthDiff = 5f;

	// Token: 0x04001145 RID: 4421
	public TriggerFlags flags;

	// Token: 0x04001146 RID: 4422
	private Prop _prop;

	// Token: 0x04001147 RID: 4423
	private bool _searchedForProp;

	// Token: 0x04001148 RID: 4424
	[NonSerialized]
	public bool triggering;
}
