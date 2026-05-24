using System;
using UnityEngine;

// Token: 0x020001C6 RID: 454
[ExecuteInEditMode]
public class ParticleSystemAlphaController : MonoBehaviour
{
	// Token: 0x1700037C RID: 892
	// (get) Token: 0x06000EFB RID: 3835 RVA: 0x000739CC File Offset: 0x00071BCC
	public ParticleSystem particleSystem
	{
		get
		{
			return base.GetComponent<ParticleSystem>();
		}
	}

	// Token: 0x1700037D RID: 893
	// (get) Token: 0x06000EFC RID: 3836 RVA: 0x000739D4 File Offset: 0x00071BD4
	public bool shouldBePaused
	{
		get
		{
			return this.alphaMultiplier == 0f;
		}
	}

	// Token: 0x06000EFD RID: 3837 RVA: 0x000739E3 File Offset: 0x00071BE3
	private void OnEnable()
	{
		this.Initialize();
	}

	// Token: 0x06000EFE RID: 3838 RVA: 0x000739EC File Offset: 0x00071BEC
	private void Initialize()
	{
		ParticleSystemRenderer component = this.particleSystem.GetComponent<ParticleSystemRenderer>();
		if (component == null)
		{
			base.enabled = false;
			return;
		}
		if (this.originalMaterial == null && component.sharedMaterial == null)
		{
			base.enabled = false;
			Debug.LogWarning("ParticleSystemRenderer has no material, so ParticleSystemAlphaController cannot be initialised.");
			return;
		}
		this.initialMaterialAlpha = this.originalMaterial.GetColor(this.colorParamName).a;
		this.material = new Material(this.originalMaterial);
		this.material.name = this.material.name + " (ParticleSystemAlphaController Clone)";
		component.material = this.material;
		this.initialized = true;
	}

	// Token: 0x06000EFF RID: 3839 RVA: 0x00073AA4 File Offset: 0x00071CA4
	private void OnDisable()
	{
		if (!this.initialized)
		{
			return;
		}
		if (this.material != null)
		{
			if (Application.isPlaying)
			{
				Object.Destroy(this.material);
			}
			else
			{
				Object.DestroyImmediate(this.material);
			}
			this.material = null;
		}
		ParticleSystemRenderer component = this.particleSystem.GetComponent<ParticleSystemRenderer>();
		if (component != null)
		{
			component.material = this.originalMaterial;
		}
		else
		{
			string text = "ParticleSystemAlphaController lost reference to particleSystemRenderer and can't re-set its prefab material to ";
			Material material = this.originalMaterial;
			Debug.LogWarning(text + ((material != null) ? material.ToString() : null) + "!");
		}
		this.initialized = false;
	}

	// Token: 0x06000F00 RID: 3840 RVA: 0x00073B40 File Offset: 0x00071D40
	private void Update()
	{
		if (!this.initialized)
		{
			this.Initialize();
			if (!this.initialized)
			{
				return;
			}
		}
		ParticleSystemRenderer component = this.particleSystem.GetComponent<ParticleSystemRenderer>();
		if (component != null)
		{
			component.material = this.material;
			Color color = this.material.GetColor(this.colorParamName);
			color.a = this.alphaMultiplier * this.initialMaterialAlpha;
			this.material.SetColor(this.colorParamName, color);
			if (this.shouldBePaused && !this.particleSystem.isPaused)
			{
				this.particleSystem.Pause();
				return;
			}
			if (!this.shouldBePaused && this.particleSystem.isPaused)
			{
				this.particleSystem.Play();
			}
		}
	}

	// Token: 0x040011B2 RID: 4530
	public string colorParamName = "_Color";

	// Token: 0x040011B3 RID: 4531
	[Range(0f, 1f)]
	public float alphaMultiplier = 1f;

	// Token: 0x040011B4 RID: 4532
	public bool pauseWhenAlphaIsZero = true;

	// Token: 0x040011B5 RID: 4533
	public Material originalMaterial;

	// Token: 0x040011B6 RID: 4534
	[NonSerialized]
	private Material material;

	// Token: 0x040011B7 RID: 4535
	[Disable]
	public float initialMaterialAlpha;

	// Token: 0x040011B8 RID: 4536
	private bool initialized;
}
