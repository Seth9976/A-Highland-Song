using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000060 RID: 96
[ExecuteInEditMode]
public class WaterRippleManager : MonoSingleton<WaterRippleManager>
{
	// Token: 0x060002A0 RID: 672 RVA: 0x00015D9C File Offset: 0x00013F9C
	public void CreateRipple(Vector3 position)
	{
		this._ripples.Enqueue(new WaterRippleManager.Ripple
		{
			pos = position,
			creationTime = Time.time
		});
		while (this._ripples.Count > 64)
		{
			this._ripples.Dequeue();
		}
	}

	// Token: 0x060002A1 RID: 673 RVA: 0x00015DEE File Offset: 0x00013FEE
	private void OnEnable()
	{
		this._ripples.Clear();
		this.Refresh();
	}

	// Token: 0x060002A2 RID: 674 RVA: 0x00015E01 File Offset: 0x00014001
	private void OnDisable()
	{
		this._ripples.Clear();
		this.Refresh();
	}

	// Token: 0x060002A3 RID: 675 RVA: 0x00015E14 File Offset: 0x00014014
	private void Update()
	{
		if (Application.isPlaying)
		{
			while (this._ripples.Count > 0 && Time.time > this._ripples.Peek().creationTime + 5f)
			{
				this._ripples.Dequeue();
			}
		}
		this.Refresh();
	}

	// Token: 0x060002A4 RID: 676 RVA: 0x00015E68 File Offset: 0x00014068
	private void Refresh()
	{
		int num = 0;
		float time = Time.time;
		foreach (WaterRippleManager.Ripple ripple in this._ripples)
		{
			float num2 = time - ripple.creationTime;
			this._shaderRipples[num] = new Vector4(ripple.pos.x, ripple.pos.y, ripple.pos.z, num2);
			num++;
		}
		Shader.SetGlobalInt("_RipplesCount", Mathf.Min(this._ripples.Count, 64));
		Shader.SetGlobalVectorArray("_RipplePositionsAndTimes", this._shaderRipples);
	}

	// Token: 0x040003D9 RID: 985
	private const int maxRippleCount = 64;

	// Token: 0x040003DA RID: 986
	private Queue<WaterRippleManager.Ripple> _ripples = new Queue<WaterRippleManager.Ripple>();

	// Token: 0x040003DB RID: 987
	private Vector4[] _shaderRipples = new Vector4[64];

	// Token: 0x02000285 RID: 645
	private struct Ripple
	{
		// Token: 0x040014F2 RID: 5362
		public Vector3 pos;

		// Token: 0x040014F3 RID: 5363
		public float creationTime;
	}
}
