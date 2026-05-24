using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200015A RID: 346
public class GodRayParticlesManager : MonoSingleton<GodRayParticlesManager>
{
	// Token: 0x06000BBA RID: 3002 RVA: 0x0005E5BC File Offset: 0x0005C7BC
	public void SetPeakGodRayActive(string peakId, bool newActive, Vector3 worldPos)
	{
		if (this._activeGodRays.ContainsKey(peakId) == newActive)
		{
			return;
		}
		if (newActive)
		{
			GodRayParticles godRayParticles = this._godRayParticlesProto.Instantiate<GodRayParticles>(null);
			godRayParticles.transform.position = worldPos + 20f * Vector3.up;
			this.StartGodRay(godRayParticles);
			this._activeGodRays[peakId] = godRayParticles;
			return;
		}
		GodRayParticles godRayParticles2;
		if (this._activeGodRays.TryGetValue(peakId, out godRayParticles2))
		{
			if (godRayParticles2 != null)
			{
				this.FadeOutGodRay(godRayParticles2);
			}
			this._activeGodRays.Remove(peakId);
		}
	}

	// Token: 0x06000BBB RID: 3003 RVA: 0x0005E64C File Offset: 0x0005C84C
	private void StartGodRay(GodRayParticles godRay)
	{
		ParticleSystem[] particles = godRay.particles;
		for (int i = 0; i < particles.Length; i++)
		{
			particles[i].Play();
		}
	}

	// Token: 0x06000BBC RID: 3004 RVA: 0x0005E678 File Offset: 0x0005C878
	private void FadeOutGodRay(GodRayParticles godRay)
	{
		ParticleSystem[] particles = godRay.particles;
		for (int i = 0; i < particles.Length; i++)
		{
			particles[i].Stop();
		}
		this._fadingOutGodRays.Add(godRay);
	}

	// Token: 0x06000BBD RID: 3005 RVA: 0x0005E6B0 File Offset: 0x0005C8B0
	public void FadeOutGodRaysBeforeLevelIndex(int firstValidLevelIndex)
	{
		this._expiredPeakNamesScratch.Clear();
		foreach (KeyValuePair<string, GodRayParticles> keyValuePair in this._activeGodRays)
		{
			GodRayParticles value = keyValuePair.Value;
			if (Level.DepthToIndex(value.transform.position.z) < firstValidLevelIndex)
			{
				this.FadeOutGodRay(value);
				this._expiredPeakNamesScratch.Add(keyValuePair.Key);
			}
		}
		foreach (string text in this._expiredPeakNamesScratch)
		{
			this._activeGodRays.Remove(text);
		}
	}

	// Token: 0x06000BBE RID: 3006 RVA: 0x0005E78C File Offset: 0x0005C98C
	public void Clear()
	{
		foreach (GodRayParticles godRayParticles in this._fadingOutGodRays)
		{
			if (!(godRayParticles == null))
			{
				godRayParticles.GetComponent<Prototype>().ReturnToPool();
			}
		}
		this._fadingOutGodRays.Clear();
		foreach (GodRayParticles godRayParticles2 in this._activeGodRays.Values)
		{
			if (!(godRayParticles2 == null))
			{
				godRayParticles2.GetComponent<Prototype>().ReturnToPool();
			}
		}
		this._activeGodRays.Clear();
	}

	// Token: 0x06000BBF RID: 3007 RVA: 0x0005E858 File Offset: 0x0005CA58
	private void Update()
	{
		bool flag = !GameClock.instance.isNight;
		foreach (GodRayParticles godRayParticles in this._activeGodRays.Values)
		{
			if (!(godRayParticles == null))
			{
				ParticleSystem[] particles = godRayParticles.particles;
				for (int i = 0; i < particles.Length; i++)
				{
					ParticlesX.SetActive(particles[i], flag);
				}
			}
		}
		this._fadingOutGodRays.UpdateAndRemoveIf(delegate(GodRayParticles godRay)
		{
			bool flag2 = false;
			ParticleSystem[] particles2 = godRay.particles;
			for (int j = 0; j < particles2.Length; j++)
			{
				if (particles2[j].particleCount > 0)
				{
					flag2 = true;
				}
			}
			if (!flag2)
			{
				godRay.GetComponent<Prototype>().ReturnToPool();
				return true;
			}
			return false;
		});
	}

	// Token: 0x04000DDC RID: 3548
	private List<string> _expiredPeakNamesScratch = new List<string>(64);

	// Token: 0x04000DDD RID: 3549
	private Dictionary<string, GodRayParticles> _activeGodRays = new Dictionary<string, GodRayParticles>();

	// Token: 0x04000DDE RID: 3550
	private List<GodRayParticles> _fadingOutGodRays = new List<GodRayParticles>();

	// Token: 0x04000DDF RID: 3551
	[SerializeField]
	private Prototype _godRayParticlesProto;
}
