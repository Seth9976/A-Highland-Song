using System;
using UnityEngine;

// Token: 0x0200003D RID: 61
public class RouteVisualiser : MonoBehaviour
{
	// Token: 0x170000AC RID: 172
	// (get) Token: 0x060001E8 RID: 488 RVA: 0x00011618 File Offset: 0x0000F818
	public static bool highlightRoutesSettingEnabled
	{
		get
		{
			return PlayerPrefsX.GetInt(RouteVisualiser.highlightRoutesPrefName, 1) != 0;
		}
	}

	// Token: 0x060001E9 RID: 489 RVA: 0x00011628 File Offset: 0x0000F828
	public static void SetFlattened(bool flattened)
	{
		if (RouteVisualiser._Global_RoutesFlattenedId == 0)
		{
			RouteVisualiser._Global_RoutesFlattenedId = Shader.PropertyToID("_Global_RoutesFlattened");
		}
		Shader.SetGlobalFloat(RouteVisualiser._Global_RoutesFlattenedId, (float)(flattened ? 1 : 0));
	}

	// Token: 0x060001EA RID: 490 RVA: 0x00011652 File Offset: 0x0000F852
	private void OnEnable()
	{
		this._routesFadeId = Shader.PropertyToID("_Global_RoutesFade");
		this._playerPosId = Shader.PropertyToID("_Global_PlayerPos");
	}

	// Token: 0x060001EB RID: 491 RVA: 0x00011674 File Offset: 0x0000F874
	private void Update()
	{
		if (!Game.loaded)
		{
			return;
		}
		if (WorldManager.instance.loading)
		{
			return;
		}
		if (this._currentLevelIdx != Level.currentIndex)
		{
			this._currentLevelIdx = Level.currentIndex;
		}
		bool flag = Game.instance.lookingFurther && RouteVisualiser.highlightRoutesSettingEnabled;
		if (flag != this.active)
		{
			this.active = flag;
			this.meshFilter.mesh = Level.current.routesMesh;
		}
		this._alpha = Mathf.MoveTowards(this._alpha, (float)(this.active ? 1 : 0), Time.deltaTime);
		Shader.SetGlobalFloat(this._routesFadeId, 1f - this._alpha);
		Vector4 vector = Runner.instance.physicalPosition3d;
		vector.w = 1f;
		Shader.SetGlobalVector(this._playerPosId, vector);
	}

	// Token: 0x060001EC RID: 492 RVA: 0x0001174A File Offset: 0x0000F94A
	private void OnDrawGizmos()
	{
	}

	// Token: 0x040002A9 RID: 681
	[Disable]
	public bool active;

	// Token: 0x040002AA RID: 682
	public MeshFilter meshFilter;

	// Token: 0x040002AB RID: 683
	public static string highlightRoutesPrefName = "highlightRoutesEnabled";

	// Token: 0x040002AC RID: 684
	private static int _Global_RoutesFlattenedId;

	// Token: 0x040002AD RID: 685
	private int _currentLevelIdx = -1;

	// Token: 0x040002AE RID: 686
	private float _alpha;

	// Token: 0x040002AF RID: 687
	private int _routesFadeId;

	// Token: 0x040002B0 RID: 688
	private int _playerPosId;

	// Token: 0x040002B1 RID: 689
	private static RouteEditHint[] _hints;

	// Token: 0x040002B2 RID: 690
	[SerializeField]
	private RouteVisualiserSettings _settings;
}
