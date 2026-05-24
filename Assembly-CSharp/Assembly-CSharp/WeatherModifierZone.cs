using System;
using UnityEngine;

// Token: 0x020000F8 RID: 248
public class WeatherModifierZone : MonoBehaviour
{
	// Token: 0x060007F7 RID: 2039 RVA: 0x000463BA File Offset: 0x000445BA
	public bool ContainsPoint(Vector3 pos)
	{
		if (this.region != null)
		{
			return this.region.ContainsPoint(pos);
		}
		return this.triggerZone.InsideTriggerDist(pos, pos.z, 1f);
	}

	// Token: 0x1700020B RID: 523
	// (get) Token: 0x060007F8 RID: 2040 RVA: 0x000463F4 File Offset: 0x000445F4
	public float area
	{
		get
		{
			if (this.region != null)
			{
				Vector3 size = this.region.bounds.size;
				return size.x * size.y;
			}
			float triggerRadius = this.triggerZone.triggerRadius;
			return 3.1415927f * triggerRadius * triggerRadius;
		}
	}

	// Token: 0x1700020C RID: 524
	// (get) Token: 0x060007F9 RID: 2041 RVA: 0x00046448 File Offset: 0x00044648
	public Range rangeX
	{
		get
		{
			if (this.region != null)
			{
				Bounds bounds = this.region.bounds;
				return new Range(bounds.min.x, bounds.max.x);
			}
			return Range.Centered(base.transform.position.x, 2f * this.triggerZone.triggerRadius);
		}
	}

	// Token: 0x1700020D RID: 525
	// (get) Token: 0x060007FA RID: 2042 RVA: 0x000464B3 File Offset: 0x000446B3
	private TriggerZone triggerZone
	{
		get
		{
			if (this._triggerZone == null)
			{
				this._triggerZone = base.GetComponent<TriggerZone>();
			}
			return this._triggerZone;
		}
	}

	// Token: 0x1700020E RID: 526
	// (get) Token: 0x060007FB RID: 2043 RVA: 0x000464D5 File Offset: 0x000446D5
	private Region region
	{
		get
		{
			if (!this._searchedRegion)
			{
				this._region = base.GetComponent<Region>();
				this._searchedRegion = true;
			}
			return this._region;
		}
	}

	// Token: 0x060007FC RID: 2044 RVA: 0x000464F8 File Offset: 0x000446F8
	private void OnEnable()
	{
	}

	// Token: 0x040009E2 RID: 2530
	[EnumButtonGroup]
	public ShelterProtectionStrength protectionFromWeather = ShelterProtectionStrength.Full;

	// Token: 0x040009E3 RID: 2531
	[EnumButtonGroup]
	public ShelterProtectionStrength protectionFromWind = ShelterProtectionStrength.Full;

	// Token: 0x040009E4 RID: 2532
	[EnumButtonGroup]
	public ShelterProtectionStrength protectionFromTemperature;

	// Token: 0x040009E5 RID: 2533
	public bool affectsActualWeather;

	// Token: 0x040009E6 RID: 2534
	public bool provideMaxHealthRefreshOnRestingWithin;

	// Token: 0x040009E7 RID: 2535
	public string optionalInkName;

	// Token: 0x040009E8 RID: 2536
	private TriggerZone _triggerZone;

	// Token: 0x040009E9 RID: 2537
	private Region _region;

	// Token: 0x040009EA RID: 2538
	private bool _searchedRegion;
}
