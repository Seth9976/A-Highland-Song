using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000025 RID: 37
public class LowQualityWaterZone : MonoInstancer<LowQualityWaterZone>
{
	// Token: 0x1700002A RID: 42
	// (get) Token: 0x060000B9 RID: 185 RVA: 0x0000A708 File Offset: 0x00008908
	public static bool isInside
	{
		get
		{
			Vector3 targetPoint = GameCamera.instance.cameraProperties.targetPoint;
			Vector2 vector = new Vector2(targetPoint.x, targetPoint.z);
			using (List<LowQualityWaterZone>.Enumerator enumerator = MonoInstancer<LowQualityWaterZone>.all.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current._xzWorldRect.Contains(vector))
					{
						return true;
					}
				}
			}
			return false;
		}
	}

	// Token: 0x060000BA RID: 186 RVA: 0x0000A78C File Offset: 0x0000898C
	protected override void OnEnable()
	{
		base.OnEnable();
		Vector3 position = base.transform.position;
		this._xzWorldRect = RectX.CreateFromCenter(new Vector2(position.x, position.z), new Vector2(this.size.x, this.size.z));
	}

	// Token: 0x060000BB RID: 187 RVA: 0x0000A7E2 File Offset: 0x000089E2
	private void OnValidate()
	{
		this.size.y = 100f;
	}

	// Token: 0x060000BC RID: 188 RVA: 0x0000A7F4 File Offset: 0x000089F4
	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.white;
		Gizmos.DrawWireCube(base.transform.position, this.size);
	}

	// Token: 0x04000197 RID: 407
	public Vector3 size;

	// Token: 0x04000198 RID: 408
	private Rect _xzWorldRect;
}
