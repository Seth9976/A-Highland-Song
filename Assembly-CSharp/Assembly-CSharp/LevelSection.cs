using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000191 RID: 401
[ExecuteInEditMode]
public class LevelSection : MonoInstancer<LevelSection>
{
	// Token: 0x1700030D RID: 781
	// (get) Token: 0x06000D32 RID: 3378 RVA: 0x00069AF0 File Offset: 0x00067CF0
	public int levelIdx
	{
		get
		{
			return Level.DepthToIndex(base.transform.position.z);
		}
	}

	// Token: 0x06000D33 RID: 3379 RVA: 0x00069B07 File Offset: 0x00067D07
	protected override void OnEnable()
	{
		base.OnEnable();
		this.level.loadedLevelSections.Add(this);
	}

	// Token: 0x06000D34 RID: 3380 RVA: 0x00069B20 File Offset: 0x00067D20
	protected override void OnDisable()
	{
		base.OnDisable();
		this.level.loadedLevelSections.Remove(this);
		this._level = null;
	}

	// Token: 0x1700030E RID: 782
	// (get) Token: 0x06000D35 RID: 3381 RVA: 0x00069B44 File Offset: 0x00067D44
	public Level level
	{
		get
		{
			if ((this._level == null || !Application.isPlaying) && this.world != null)
			{
				int levelIdx = this.levelIdx;
				if (levelIdx >= 0 && levelIdx < this.world.levels.Count)
				{
					this._level = this.world.levels[levelIdx];
				}
				else
				{
					Debug.LogError("LevelSection " + base.name + " had a levelIdx that was out of range for the number of Levels in the World. levelIdx is auto-generated from the Z position, so perhaps it's positioned wrong?");
				}
			}
			return this._level;
		}
	}

	// Token: 0x04001009 RID: 4105
	public World world;

	// Token: 0x0400100A RID: 4106
	public List<Slope> crossReferencedSlopes = new List<Slope>();

	// Token: 0x0400100B RID: 4107
	[NonSerialized]
	public bool crossReferencedSlopesHaveBeenConnected;

	// Token: 0x0400100C RID: 4108
	[NonSerialized]
	public List<Splat> unflattenedSplats;

	// Token: 0x0400100D RID: 4109
	[NonSerialized]
	public List<Splat> unflattenedSplatsThatJustNeedRefresh;

	// Token: 0x0400100E RID: 4110
	private Level _level;
}
