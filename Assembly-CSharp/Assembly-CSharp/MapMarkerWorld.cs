using System;
using UnityEngine;

// Token: 0x02000124 RID: 292
public class MapMarkerWorld : MonoBehaviour
{
	// Token: 0x17000269 RID: 617
	// (get) Token: 0x060009B9 RID: 2489 RVA: 0x000517E7 File Offset: 0x0004F9E7
	private Prop prop
	{
		get
		{
			if (this._prop == null)
			{
				this._prop = base.GetComponentInChildren<Prop>();
			}
			return this._prop;
		}
	}

	// Token: 0x1700026A RID: 618
	// (get) Token: 0x060009BA RID: 2490 RVA: 0x00051809 File Offset: 0x0004FA09
	private Prototype prototype
	{
		get
		{
			if (this._prototype == null)
			{
				this._prototype = base.GetComponent<Prototype>();
			}
			return this._prototype;
		}
	}

	// Token: 0x1700026B RID: 619
	// (get) Token: 0x060009BB RID: 2491 RVA: 0x0005182B File Offset: 0x0004FA2B
	private ParticleSystem particles
	{
		get
		{
			if (this._particles == null)
			{
				this._particles = base.GetComponent<ParticleSystem>();
			}
			return this._particles;
		}
	}

	// Token: 0x060009BC RID: 2492 RVA: 0x00051850 File Offset: 0x0004FA50
	public void Setup(Vector3 targetPos, Map map)
	{
		this.map = map;
		this._parentLevel = WorldManager.instance.GetLevelAtDepth(targetPos.z);
		if (this._parentLevel.isSetup)
		{
			this._parentLevel.props.AddDynamic(this.prop);
			this._parentLevel.triggerZones.AddDynamic(this.prop.triggerZone);
		}
		else
		{
			Debug.LogError(string.Format("MapMarkerWorld was created for a Level idx {0} but it wasn't set up, presumably because it's not loaded (yet?)", this._parentLevel.levelIdx), this);
		}
		base.transform.position = targetPos + 0.5f * Vector3.forward;
		ParticleSystem.EmitParams emitParams = default(ParticleSystem.EmitParams);
		emitParams.startSize = 3f;
		this.particles.Emit(emitParams, 30);
		this.particles.Play();
	}

	// Token: 0x060009BD RID: 2493 RVA: 0x00051928 File Offset: 0x0004FB28
	public void ReturnToPool(bool immediate)
	{
		if (this._parentLevel != null && this._parentLevel.isSetup)
		{
			this._parentLevel.props.RemoveDynamic(this.prop);
			this._parentLevel.triggerZones.RemoveDynamic(this.prop.triggerZone);
		}
		this._parentLevel = null;
		if (immediate)
		{
			this.prototype.ReturnToPool();
			return;
		}
		ParticlesX.StopAndReturnToPool(this.particles);
	}

	// Token: 0x04000BCB RID: 3019
	[Disable]
	public Map map;

	// Token: 0x04000BCC RID: 3020
	private Prop _prop;

	// Token: 0x04000BCD RID: 3021
	private Prototype _prototype;

	// Token: 0x04000BCE RID: 3022
	private ParticleSystem _particles;

	// Token: 0x04000BCF RID: 3023
	private Level _parentLevel;
}
