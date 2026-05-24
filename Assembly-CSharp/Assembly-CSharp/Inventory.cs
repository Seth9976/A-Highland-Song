using System;
using System.Collections.Generic;

// Token: 0x02000032 RID: 50
public class Inventory : MonoSingleton<Inventory>
{
	// Token: 0x1700008D RID: 141
	// (get) Token: 0x06000198 RID: 408 RVA: 0x0000EF4C File Offset: 0x0000D14C
	public List<Map> incompleteForwardFacingMaps
	{
		get
		{
			this._incompleteForwardFacingMaps.Clear();
			foreach (Map map in this.maps)
			{
				if (!Narrative.instance.PlayerHasCompletedMapWithPropName(map.targetInkPropName))
				{
					bool flag = false;
					using (List<Prop>.Enumerator enumerator2 = Prop.GetLoadedPropsByInkName(map.targetInkPropName).GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							if (enumerator2.Current.levelIdx >= Level.currentIndex)
							{
								flag = true;
								break;
							}
						}
					}
					if (flag)
					{
						this._incompleteForwardFacingMaps.Add(map);
					}
				}
			}
			return this._incompleteForwardFacingMaps;
		}
	}

	// Token: 0x06000199 RID: 409 RVA: 0x0000F01C File Offset: 0x0000D21C
	public void ClearAndGiveInitialMaps()
	{
		this.maps.Clear();
		this.incompleteForwardFacingMaps.Clear();
		foreach (Map map in Map.all)
		{
			if ((map.unlockedAtGameStart || DebugOptions.opts.unlockAllMaps) && !this.maps.Contains(map))
			{
				this.maps.Add(map);
			}
		}
	}

	// Token: 0x04000253 RID: 595
	public List<Map> maps = new List<Map>(256);

	// Token: 0x04000254 RID: 596
	private List<Map> _incompleteForwardFacingMaps = new List<Map>(256);
}
