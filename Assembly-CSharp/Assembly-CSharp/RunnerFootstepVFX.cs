using System;
using UnityEngine;

// Token: 0x020000C7 RID: 199
public class RunnerFootstepVFX : MonoBehaviour
{
	// Token: 0x0600062E RID: 1582 RVA: 0x000317D4 File Offset: 0x0002F9D4
	private void OnEnable()
	{
		Runner.onFootstep = (Action<int>)Delegate.Combine(Runner.onFootstep, new Action<int>(this.OnFootstep));
		Runner.onJumpStart = (Action)Delegate.Combine(Runner.onJumpStart, new Action(this.OnJumpStart));
		Runner.onJumpEnd = (Action)Delegate.Combine(Runner.onJumpEnd, new Action(this.OnJumpEnd));
	}

	// Token: 0x0600062F RID: 1583 RVA: 0x00031844 File Offset: 0x0002FA44
	private void OnDisable()
	{
		Runner.onFootstep = (Action<int>)Delegate.Remove(Runner.onFootstep, new Action<int>(this.OnFootstep));
		Runner.onJumpStart = (Action)Delegate.Remove(Runner.onJumpStart, new Action(this.OnJumpStart));
		Runner.onJumpEnd = (Action)Delegate.Remove(Runner.onJumpEnd, new Action(this.OnJumpEnd));
	}

	// Token: 0x06000630 RID: 1584 RVA: 0x000318B1 File Offset: 0x0002FAB1
	private void OnFootstep(int footstep)
	{
		this.CreateParticles();
	}

	// Token: 0x06000631 RID: 1585 RVA: 0x000318B9 File Offset: 0x0002FAB9
	private void OnJumpStart()
	{
		this.CreateParticles();
	}

	// Token: 0x06000632 RID: 1586 RVA: 0x000318C1 File Offset: 0x0002FAC1
	private void OnJumpEnd()
	{
		this.CreateParticles();
	}

	// Token: 0x06000633 RID: 1587 RVA: 0x000318CC File Offset: 0x0002FACC
	private void CreateParticles()
	{
		Vector3 position = Runner.instance.transform.position;
		position.y = 0f;
		base.transform.position = position;
		Prototype prototypeForSurface = this.GetPrototypeForSurface(Runner.instance.surfaceTypeSampler.surfaceType);
		if (prototypeForSurface == null)
		{
			return;
		}
		Component component = prototypeForSurface.Instantiate<ParticleSystem>(null);
		float num = Runner.instance.relativeSlopeAngle;
		if (prototypeForSurface != this.grassPrototype && prototypeForSurface != this.tallGrassPrototype)
		{
			num += this.tiltAngleFromFloor * Mathf.Sign(num);
		}
		component.transform.rotation = Quaternion.AngleAxis(num, Vector3.forward);
	}

	// Token: 0x06000634 RID: 1588 RVA: 0x00031974 File Offset: 0x0002FB74
	private Prototype GetPrototypeForSurface(SurfaceType surface)
	{
		switch (surface)
		{
		case SurfaceType.Grass:
		case SurfaceType.Forest:
			return this.grassPrototype;
		case SurfaceType.GrassTall:
			return this.tallGrassPrototype;
		case SurfaceType.Mud:
			return this.mudPrototype;
		case SurfaceType.Rock:
			return this.rockPrototype;
		case SurfaceType.Water:
			return this.waterPrototype;
		case SurfaceType.Sand:
		case SurfaceType.SandWet:
			return this.sandPrototype;
		case SurfaceType.Dirt:
			return this.dirtPrototype;
		case SurfaceType.Gravel:
			return this.gravelPrototype;
		default:
			return null;
		}
	}

	// Token: 0x04000735 RID: 1845
	public Prototype sandPrototype;

	// Token: 0x04000736 RID: 1846
	public Prototype rockPrototype;

	// Token: 0x04000737 RID: 1847
	public Prototype grassPrototype;

	// Token: 0x04000738 RID: 1848
	public Prototype tallGrassPrototype;

	// Token: 0x04000739 RID: 1849
	public Prototype dirtPrototype;

	// Token: 0x0400073A RID: 1850
	public Prototype gravelPrototype;

	// Token: 0x0400073B RID: 1851
	public Prototype waterPrototype;

	// Token: 0x0400073C RID: 1852
	public Prototype mudPrototype;

	// Token: 0x0400073D RID: 1853
	public float tiltAngleFromFloor = 30f;
}
