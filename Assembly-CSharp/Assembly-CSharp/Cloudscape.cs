using System;
using UnityEngine;

// Token: 0x02000046 RID: 70
public class Cloudscape : MonoSingleton<Cloudscape>
{
	// Token: 0x170000B3 RID: 179
	// (get) Token: 0x06000202 RID: 514 RVA: 0x00011B03 File Offset: 0x0000FD03
	// (set) Token: 0x06000203 RID: 515 RVA: 0x00011B0B File Offset: 0x0000FD0B
	public Color multiplyColor { get; set; } = Color.white;

	// Token: 0x06000204 RID: 516 RVA: 0x00011B14 File Offset: 0x0000FD14
	private void OnEnable()
	{
		if (Application.isPlaying)
		{
			this.Refresh(0f, true);
			this._mutableCloudscapeMaterial = new Material(this.cloudscapeMaterial);
			this._mutableCloudscapeMaterial.hideFlags = HideFlags.DontSave;
			MeshRenderer[] array = this.cloudscapeQuads;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].sharedMaterial = this._mutableCloudscapeMaterial;
			}
		}
	}

	// Token: 0x06000205 RID: 517 RVA: 0x00011B75 File Offset: 0x0000FD75
	private void OnDisable()
	{
		if (this._mutableCloudscapeMaterial != null)
		{
			Object.Destroy(this._mutableCloudscapeMaterial);
			this._mutableCloudscapeMaterial = null;
		}
	}

	// Token: 0x06000206 RID: 518 RVA: 0x00011B98 File Offset: 0x0000FD98
	private void LateUpdate()
	{
		Vector3 position = base.transform.position;
		position.y = 0f;
		base.transform.position = position;
	}

	// Token: 0x06000207 RID: 519 RVA: 0x00011BCC File Offset: 0x0000FDCC
	public void Refresh(float visualEffectTimeDelta = 0f, bool immediate = false)
	{
		if (PhotoMode.visible)
		{
			visualEffectTimeDelta *= 4f;
		}
		WeatherType weatherType = WeatherSystem.instance.currentWeather & (WeatherType.Cloudy | WeatherType.VeryCloudy | WeatherType.Raining | WeatherType.Storm | WeatherType.Snow);
		Cloudscape.Type type;
		if (weatherType == WeatherType.Clear)
		{
			type = Cloudscape.Type.LightClouds;
		}
		else if ((weatherType & WeatherType.Storm) > WeatherType.Clear)
		{
			type = Cloudscape.Type.Storm;
		}
		else
		{
			type = Cloudscape.Type.HeavyClouds;
		}
		if (immediate)
		{
			this._cloudAToBLerp = 0f;
			this._destinationType = type;
			this._cloudTexA = this.ChooseCloudTexture(this._destinationType);
			this._cloudTexB = this._cloudTexA;
			this.RefreshMaterialParams();
			return;
		}
		if (this.debugTargetType != Cloudscape.Type.NotSet)
		{
			type = this.debugTargetType;
		}
		if (type != this._destinationType || this.debugTargetCloudscapeIdx != -1)
		{
			this._destinationTex = this.ChooseCloudTexture(type);
			this._destinationType = type;
		}
		if (this._destinationTex != null && this._destinationTex == this._cloudTexB)
		{
			float num = 1f;
			if (this.debugTargetType != Cloudscape.Type.NotSet || this.debugTargetCloudscapeIdx != -1)
			{
				num = 10f;
			}
			this._cloudAToBLerp = Mathf.MoveTowards(this._cloudAToBLerp, 1f, num * 0.033333335f * visualEffectTimeDelta * this._settings.crossFadeSpeed);
			if (this._cloudAToBLerp == 1f)
			{
				this._destinationTex = null;
				this._cloudTexA = this._cloudTexB;
				this._cloudAToBLerp = 0f;
				this._cloudsAX = this._cloudsBX;
			}
		}
		else if (this._destinationTex != null && this._destinationTex != this._cloudTexB)
		{
			this._cloudAToBLerp = Mathf.MoveTowards(this._cloudAToBLerp, 0f, 0.033333335f * visualEffectTimeDelta * this._settings.crossFadeSpeed);
			if (this._cloudAToBLerp == 0f)
			{
				this._cloudTexB = this._destinationTex;
				this._cloudsBX = Random.value;
			}
		}
		float windVelocity = WeatherSystem.instance.windVelocity;
		this._cloudsAX += 0.01f * this._settings.windEffectSpeed * windVelocity * visualEffectTimeDelta;
		this._cloudsBX += 0.01f * this._settings.windEffectSpeed * windVelocity * visualEffectTimeDelta;
		this.RefreshMaterialParams();
	}

	// Token: 0x06000208 RID: 520 RVA: 0x00011DD4 File Offset: 0x0000FFD4
	private void RefreshMaterialParams()
	{
		if (this._mutableCloudscapeMaterial == null)
		{
			return;
		}
		if (!Cloudscape._generatedIds)
		{
			Cloudscape._cloudsAId = Shader.PropertyToID("_CloudsA");
			Cloudscape._cloudsBId = Shader.PropertyToID("_CloudsB");
			Cloudscape._cloudsAAlphaId = Shader.PropertyToID("_CloudsAAlpha");
			Cloudscape._cloudsBAlphaId = Shader.PropertyToID("_CloudsBAlpha");
			Cloudscape._cloudsAXId = Shader.PropertyToID("_CloudsAX");
			Cloudscape._cloudsBXId = Shader.PropertyToID("_CloudsBX");
			Cloudscape._multiplyColorId = Shader.PropertyToID("_Multiply");
			Cloudscape._generatedIds = true;
		}
		if (this._cloudTexA != null)
		{
			this._mutableCloudscapeMaterial.SetTexture(Cloudscape._cloudsAId, this._cloudTexA.texture);
		}
		if (this._cloudTexB != null)
		{
			this._mutableCloudscapeMaterial.SetTexture(Cloudscape._cloudsBId, this._cloudTexB.texture);
		}
		float opacity = this._cloudTexA.opacity;
		this._mutableCloudscapeMaterial.SetFloat(Cloudscape._cloudsAAlphaId, opacity * (1f - this._cloudAToBLerp));
		float opacity2 = this._cloudTexB.opacity;
		this._mutableCloudscapeMaterial.SetFloat(Cloudscape._cloudsBAlphaId, opacity2 * this._cloudAToBLerp);
		this._mutableCloudscapeMaterial.SetFloat(Cloudscape._cloudsAXId, this._cloudsAX + this.debugOffsetX);
		this._mutableCloudscapeMaterial.SetFloat(Cloudscape._cloudsBXId, this._cloudsBX + this.debugOffsetX);
		this._mutableCloudscapeMaterial.SetColor(Cloudscape._multiplyColorId, this.multiplyColor);
	}

	// Token: 0x06000209 RID: 521 RVA: 0x00011F4A File Offset: 0x0001014A
	private CloudscapeTexture GetRandomCloudTexture(CloudscapeTexture[] textures)
	{
		if (this.debugTargetCloudscapeIdx != -1 && textures.Length != 0)
		{
			return textures[Mathf.Clamp(this.debugTargetCloudscapeIdx, 0, textures.Length - 1)];
		}
		return textures.Random<CloudscapeTexture>();
	}

	// Token: 0x0600020A RID: 522 RVA: 0x00011F74 File Offset: 0x00010174
	private CloudscapeTexture ChooseCloudTexture(Cloudscape.Type type)
	{
		switch (type)
		{
		case Cloudscape.Type.LightClouds:
			return this.GetRandomCloudTexture(this._settings.lightCloudTextures);
		case Cloudscape.Type.HeavyClouds:
			return this.GetRandomCloudTexture(this._settings.heavyCloudTextures);
		case Cloudscape.Type.Storm:
			return this.GetRandomCloudTexture(this._settings.stormTextures);
		default:
			Debug.LogError("Unhandled cloud texture type: " + type.ToString());
			return this.GetRandomCloudTexture(this._settings.lightCloudTextures);
		}
	}

	// Token: 0x040002D6 RID: 726
	public Material cloudscapeMaterial;

	// Token: 0x040002D7 RID: 727
	public MeshRenderer[] cloudscapeQuads;

	// Token: 0x040002D8 RID: 728
	[Range(-1f, 1f)]
	public float debugOffsetX;

	// Token: 0x040002D9 RID: 729
	public Cloudscape.Type debugTargetType;

	// Token: 0x040002DA RID: 730
	public int debugTargetCloudscapeIdx = -1;

	// Token: 0x040002DB RID: 731
	private Cloudscape.Type _destinationType;

	// Token: 0x040002DC RID: 732
	private CloudscapeTexture _destinationTex;

	// Token: 0x040002DD RID: 733
	private float _cloudAToBLerp;

	// Token: 0x040002DE RID: 734
	private float _cloudsAX;

	// Token: 0x040002DF RID: 735
	private float _cloudsBX;

	// Token: 0x040002E0 RID: 736
	[SerializeField]
	[Disable]
	private CloudscapeTexture _cloudTexA;

	// Token: 0x040002E1 RID: 737
	private CloudscapeTexture _cloudTexB;

	// Token: 0x040002E2 RID: 738
	[SerializeField]
	private CloudscapeSettings _settings;

	// Token: 0x040002E3 RID: 739
	[Range(0f, 1f)]
	public float _cloudTexAAlpha = 1f;

	// Token: 0x040002E4 RID: 740
	[Range(0f, 1f)]
	public float _cloudTexBAlpha = 1f;

	// Token: 0x040002E5 RID: 741
	private Material _mutableCloudscapeMaterial;

	// Token: 0x040002E6 RID: 742
	private static int _cloudsAId;

	// Token: 0x040002E7 RID: 743
	private static int _cloudsBId;

	// Token: 0x040002E8 RID: 744
	private static int _cloudsAAlphaId;

	// Token: 0x040002E9 RID: 745
	private static int _cloudsBAlphaId;

	// Token: 0x040002EA RID: 746
	private static int _cloudsAXId;

	// Token: 0x040002EB RID: 747
	private static int _cloudsBXId;

	// Token: 0x040002EC RID: 748
	private static int _multiplyColorId;

	// Token: 0x040002ED RID: 749
	private static bool _generatedIds;

	// Token: 0x02000277 RID: 631
	public enum Type
	{
		// Token: 0x040014C9 RID: 5321
		NotSet,
		// Token: 0x040014CA RID: 5322
		LightClouds,
		// Token: 0x040014CB RID: 5323
		HeavyClouds,
		// Token: 0x040014CC RID: 5324
		Storm
	}
}
