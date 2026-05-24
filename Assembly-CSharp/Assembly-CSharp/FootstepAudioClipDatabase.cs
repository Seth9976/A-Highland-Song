using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200000C RID: 12
public class FootstepAudioClipDatabase : ScriptableObject
{
	// Token: 0x17000012 RID: 18
	// (get) Token: 0x06000046 RID: 70 RVA: 0x00005AC0 File Offset: 0x00003CC0
	public Dictionary<SurfaceType, FootstepAudioClipSet> surfaces
	{
		get
		{
			if (this._surfaces == null)
			{
				this._surfaces = new Dictionary<SurfaceType, FootstepAudioClipSet>
				{
					{
						SurfaceType.Grass,
						this.grass
					},
					{
						SurfaceType.Water,
						this.waterShallow
					},
					{
						SurfaceType.Forest,
						this.forest
					},
					{
						SurfaceType.GrassTall,
						this.grassTall
					},
					{
						SurfaceType.Mud,
						this.mud
					},
					{
						SurfaceType.Rock,
						this.rock
					},
					{
						SurfaceType.Sand,
						this.sandShallow
					},
					{
						SurfaceType.Dirt,
						this.dirt
					},
					{
						SurfaceType.Gravel,
						this.gravel
					},
					{
						SurfaceType.Wood,
						this.wood
					}
				};
			}
			return this._surfaces;
		}
	}

	// Token: 0x06000047 RID: 71 RVA: 0x00005B70 File Offset: 0x00003D70
	public FootstepAudioClipSet TryGetClipSetFromSurfaceType(SurfaceType surfaceType)
	{
		if (surfaceType == SurfaceType.NONE)
		{
			return null;
		}
		FootstepAudioClipSet footstepAudioClipSet = null;
		if (this.surfaces.TryGetValue(surfaceType, out footstepAudioClipSet))
		{
			return footstepAudioClipSet;
		}
		if (this.@default != null)
		{
			return this.@default;
		}
		Debug.LogWarning("No footstep sounds for " + surfaceType.ToString() + " and no fallback exists. Current valid types are " + DebugX.ListAsString<SurfaceType>(this.surfaces.Keys, null, true, true));
		return null;
	}

	// Token: 0x0400004E RID: 78
	[SerializeField]
	private FootstepAudioClipSet @default;

	// Token: 0x0400004F RID: 79
	[Space]
	[SerializeField]
	private FootstepAudioClipSet dirt;

	// Token: 0x04000050 RID: 80
	[SerializeField]
	private FootstepAudioClipSet gravel;

	// Token: 0x04000051 RID: 81
	[SerializeField]
	private FootstepAudioClipSet forest;

	// Token: 0x04000052 RID: 82
	[SerializeField]
	private FootstepAudioClipSet grass;

	// Token: 0x04000053 RID: 83
	[SerializeField]
	private FootstepAudioClipSet grassTall;

	// Token: 0x04000054 RID: 84
	[SerializeField]
	private FootstepAudioClipSet mud;

	// Token: 0x04000055 RID: 85
	[SerializeField]
	private FootstepAudioClipSet rock;

	// Token: 0x04000056 RID: 86
	[SerializeField]
	private FootstepAudioClipSet sandShallow;

	// Token: 0x04000057 RID: 87
	[SerializeField]
	private FootstepAudioClipSet waterShallow;

	// Token: 0x04000058 RID: 88
	[SerializeField]
	private FootstepAudioClipSet wood;

	// Token: 0x04000059 RID: 89
	private Dictionary<SurfaceType, FootstepAudioClipSet> _surfaces;
}
