using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

// Token: 0x02000178 RID: 376
[RequireComponent(typeof(GuidComponent))]
[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[ExecuteInEditMode]
[DisallowMultipleComponent]
public class Cloud : MonoBehaviour
{
	// Token: 0x170002D9 RID: 729
	// (get) Token: 0x06000C58 RID: 3160 RVA: 0x00062A1E File Offset: 0x00060C1E
	public Vector3 animationExtent
	{
		get
		{
			return new Vector3(this.driftDistance * Mathf.Cos(0.017453292f * this.driftAngle), this.driftDistance * Mathf.Sin(0.017453292f * this.driftAngle), 0f);
		}
	}

	// Token: 0x170002DA RID: 730
	// (get) Token: 0x06000C59 RID: 3161 RVA: 0x00062A5A File Offset: 0x00060C5A
	private static Material materialPrefab
	{
		get
		{
			if (Cloud._materialPrefab == null && Application.isPlaying)
			{
				Cloud._materialPrefab = Resources.Load<Material>("CloudMaterial");
			}
			return Cloud._materialPrefab;
		}
	}

	// Token: 0x170002DB RID: 731
	// (get) Token: 0x06000C5A RID: 3162 RVA: 0x00062A84 File Offset: 0x00060C84
	private static Mesh quad
	{
		get
		{
			if (Cloud._quad == null && Application.isPlaying)
			{
				Cloud._quad = Resources.Load<Mesh>("Quad");
			}
			return Cloud._quad;
		}
	}

	// Token: 0x06000C5B RID: 3163 RVA: 0x00062AB0 File Offset: 0x00060CB0
	private void OnEnable()
	{
		this.ValidateGUID();
		this.meshRenderer.enabled = true;
		if (this.spriteSet == null)
		{
			Debug.LogWarning("Cloud missing sprite set: " + base.name, this);
			this.meshRenderer.enabled = false;
		}
		else if (!this.spriteSet.isValid)
		{
			Debug.LogWarning("Cloud sprite set invalid: " + base.name, this.spriteSet);
			this.meshRenderer.enabled = false;
		}
		this.cachedAnimationExtent = this.animationExtent;
		if (this.isDirty)
		{
			this.isDirty = false;
			this.Refresh();
			return;
		}
		this.LoadAndApplyQuadMesh();
		this.LoadAndApplyMaterial();
		this.UpdateVisibility();
		this.RefreshPropertyBlock();
	}

	// Token: 0x06000C5C RID: 3164 RVA: 0x00062B70 File Offset: 0x00060D70
	private void OnDisable()
	{
		this.meshRenderer.enabled = false;
	}

	// Token: 0x06000C5D RID: 3165 RVA: 0x00062B80 File Offset: 0x00060D80
	private void ValidateGUID()
	{
		bool flag = false;
		if (this.guidComponent == null)
		{
			flag = true;
			this.guidComponent = base.GetComponent<GuidComponent>();
		}
		if (this.guidComponent.didJustAssignNewGUID || flag)
		{
			this.OnSetGUID();
			return;
		}
		GuidComponent guidComponent = this.guidComponent;
		guidComponent.OnSetGUID = (Action)Delegate.Combine(guidComponent.OnSetGUID, new Action(this.OnSetGUID));
	}

	// Token: 0x06000C5E RID: 3166 RVA: 0x00062BE8 File Offset: 0x00060DE8
	private void OnSetGUID()
	{
		this.InitialSetUp();
	}

	// Token: 0x06000C5F RID: 3167 RVA: 0x00062BF0 File Offset: 0x00060DF0
	private void InitialSetUp()
	{
		this.guidComponent.hideFlags = HideFlags.HideInInspector;
		this.meshFilter = base.GetComponent<MeshFilter>();
		this.meshFilter.hideFlags = HideFlags.HideInHierarchy | HideFlags.HideInInspector;
		this.meshRenderer = base.GetComponent<MeshRenderer>();
		this.meshRenderer.hideFlags = HideFlags.HideInHierarchy | HideFlags.HideInInspector;
	}

	// Token: 0x06000C60 RID: 3168 RVA: 0x00062C2E File Offset: 0x00060E2E
	public void RefreshAfterInstantiation()
	{
		this.Refresh();
	}

	// Token: 0x06000C61 RID: 3169 RVA: 0x00062C38 File Offset: 0x00060E38
	public void ApplySpriteAspectRatioToTransform()
	{
		if (this.spriteSet == null || !this.spriteSet.isValid)
		{
			return;
		}
		float num = this.spriteSet.main.textureRect.width / this.spriteSet.main.textureRect.height;
		base.transform.localScale = new Vector3(base.transform.localScale.x, base.transform.localScale.x / num, 1f);
	}

	// Token: 0x06000C62 RID: 3170 RVA: 0x00062CCB File Offset: 0x00060ECB
	private void ManualUpdate()
	{
		if (this.UpdateVisibility())
		{
			this.RefreshPropertyBlock();
		}
		if (this.isDirty)
		{
			this.Refresh();
		}
	}

	// Token: 0x06000C63 RID: 3171 RVA: 0x00062CE9 File Offset: 0x00060EE9
	public void Refresh()
	{
		this.isDirty = false;
		this.LoadAndApplyQuadMesh();
		this.LoadAndApplyMaterial();
		this.EnforcePositiveScale();
		this.UpdateVisibility();
		this.RefreshPropertyBlock();
	}

	// Token: 0x06000C64 RID: 3172 RVA: 0x00062D11 File Offset: 0x00060F11
	private void LoadAndApplyQuadMesh()
	{
		this.meshFilter.mesh = Cloud.quad;
	}

	// Token: 0x06000C65 RID: 3173 RVA: 0x00062D24 File Offset: 0x00060F24
	private void LoadAndApplyMaterial()
	{
		if (this.spriteSet != null && this.spriteSet.isValid)
		{
			Material material;
			if (!Cloud.loadedMaterialsDictionary.TryGetValue(this.spriteSet.main.texture.name, out material) || material == null)
			{
				this.SetShaderPropertyIDs();
				material = new Material(Cloud.materialPrefab);
				material.name = this.spriteSet.main.texture.name;
				material.SetTexture(Cloud._MainTexID, this.spriteSet.main.texture);
				Cloud.loadedMaterialsDictionary[this.spriteSet.main.texture.name] = material;
			}
			this.meshRenderer.material = material;
			return;
		}
		Debug.LogWarning("Cloud " + base.gameObject.name + " with has no sprite", base.gameObject);
	}

	// Token: 0x06000C66 RID: 3174 RVA: 0x00062E18 File Offset: 0x00061018
	private void EnforcePositiveScale()
	{
		Vector3 localScale = base.transform.localScale;
		base.transform.localScale = new Vector3(Mathf.Abs(localScale.x), Mathf.Abs(localScale.y), Mathf.Abs(localScale.z));
	}

	// Token: 0x06000C67 RID: 3175 RVA: 0x00062E64 File Offset: 0x00061064
	public bool UpdateVisibility()
	{
		bool flag = true;
		if (this.color.a * this.tint.a * this.fadeAlpha == 0f)
		{
			flag = false;
		}
		if (this.meshRenderer.enabled != flag)
		{
			this.meshRenderer.enabled = flag;
			return flag;
		}
		return false;
	}

	// Token: 0x06000C68 RID: 3176 RVA: 0x00062EB7 File Offset: 0x000610B7
	private void SetMaterialPropertyBlock()
	{
		if (this.spriteSet == null)
		{
			if (this._properties != null)
			{
				this._properties.Clear();
			}
			return;
		}
		if (this._properties == null)
		{
			this._properties = new MaterialPropertyBlock();
		}
	}

	// Token: 0x06000C69 RID: 3177 RVA: 0x00062EF0 File Offset: 0x000610F0
	private void SetShaderPropertyIDs()
	{
		if (!Cloud._generatedIDs)
		{
			Cloud._MainTexID = Shader.PropertyToID("_MainTex");
			Cloud._ColorID = Shader.PropertyToID("_Color");
			Cloud._HueShiftID = Shader.PropertyToID("_HueShift");
			Cloud._SaturationID = Shader.PropertyToID("_Saturation");
			Cloud._BrightnessID = Shader.PropertyToID("_Brightness");
			Cloud._ContrastID = Shader.PropertyToID("_Contrast");
			Cloud._SizeID = Shader.PropertyToID("_Size");
			Cloud._FogStrengthID = Shader.PropertyToID("_FogStrength");
			Cloud._MainTexUVID = Shader.PropertyToID("_MainTexUV");
			Cloud._HighlightTexUVID = Shader.PropertyToID("_HighlightTexUV");
			Cloud._SoftTexUVID = Shader.PropertyToID("_SoftTexUV");
			Cloud._SoftHighlightTexUVID = Shader.PropertyToID("_SoftHighlightTexUV");
			Cloud._generatedIDs = true;
		}
	}

	// Token: 0x170002DC RID: 732
	// (get) Token: 0x06000C6A RID: 3178 RVA: 0x00062FC1 File Offset: 0x000611C1
	private Color finalColor
	{
		get
		{
			return (this.color * this.tint).WithAlpha(this.color.a * this.tint.a * this.fadeAlpha);
		}
	}

	// Token: 0x06000C6B RID: 3179 RVA: 0x00062FF8 File Offset: 0x000611F8
	public void RefreshPropertyBlock()
	{
		if (!this.PrePropertyBlockRefresh() && this._doneInitialPropertySetup)
		{
			return;
		}
		this.SetMaterialPropertyBlock();
		this._properties.SetColor(Cloud._ColorID, this.finalColor);
		this._properties.SetFloat(Cloud._HueShiftID, this.hueShift * 2f * 3.1415927f);
		this._properties.SetFloat(Cloud._SaturationID, this.saturation);
		this._properties.SetFloat(Cloud._BrightnessID, this.brightness);
		this._properties.SetFloat(Cloud._ContrastID, this.contrast);
		this._properties.SetFloat(Cloud._SizeID, base.transform.lossyScale.y);
		this._properties.SetFloat(Cloud._FogStrengthID, this.fogStrength);
		Vector2 vector = new Vector2((float)this.spriteSet.main.texture.width, (float)this.spriteSet.main.texture.height);
		Rect textureRect = this.spriteSet.main.textureRect;
		Rect textureRect2 = this.spriteSet.highlight.textureRect;
		Rect textureRect3 = this.spriteSet.softMain.textureRect;
		Rect textureRect4 = this.spriteSet.softHighlight.textureRect;
		Vector4 vector2 = Cloud.<RefreshPropertyBlock>g__NormalisedUV|54_0(textureRect, vector);
		Vector4 vector3 = Cloud.<RefreshPropertyBlock>g__NormalisedUV|54_0(textureRect2, vector);
		Vector4 vector4 = Cloud.<RefreshPropertyBlock>g__NormalisedUV|54_0(textureRect3, vector);
		Vector4 vector5 = Cloud.<RefreshPropertyBlock>g__NormalisedUV|54_0(textureRect4, vector);
		if (this.flipX)
		{
			vector2 = Cloud.<RefreshPropertyBlock>g__FlipUV|54_1(vector2);
			vector3 = Cloud.<RefreshPropertyBlock>g__FlipUV|54_1(vector3);
			vector4 = Cloud.<RefreshPropertyBlock>g__FlipUV|54_1(vector4);
			vector5 = Cloud.<RefreshPropertyBlock>g__FlipUV|54_1(vector5);
		}
		this._properties.SetVector(Cloud._MainTexUVID, vector2);
		this._properties.SetVector(Cloud._HighlightTexUVID, vector3);
		this._properties.SetVector(Cloud._SoftTexUVID, vector4);
		this._properties.SetVector(Cloud._SoftHighlightTexUVID, vector5);
		this.meshRenderer.SetPropertyBlock(this._properties);
		this._doneInitialPropertySetup = true;
	}

	// Token: 0x06000C6C RID: 3180 RVA: 0x000631F1 File Offset: 0x000613F1
	public void RefreshPropertyBlockColorOnly()
	{
		this._properties.SetColor(Cloud._ColorID, this.finalColor);
		this.meshRenderer.SetPropertyBlock(this._properties);
	}

	// Token: 0x06000C6D RID: 3181 RVA: 0x0006321C File Offset: 0x0006141C
	private bool PrePropertyBlockRefresh()
	{
		if (!this.meshRenderer.enabled)
		{
			return false;
		}
		this.SetMaterialPropertyBlock();
		if (this._properties == null || this.spriteSet == null || !this.spriteSet.isValid)
		{
			return false;
		}
		this.SetShaderPropertyIDs();
		return true;
	}

	// Token: 0x06000C6E RID: 3182 RVA: 0x0006326C File Offset: 0x0006146C
	public bool ContainsPoint(Vector3 point)
	{
		Range range = new Range(-0.4f, 0.4f);
		Vector3 vector = base.transform.InverseTransformPoint(point);
		return range.Contains(vector.x) && range.Contains(vector.y);
	}

	// Token: 0x06000C71 RID: 3185 RVA: 0x0006335C File Offset: 0x0006155C
	[CompilerGenerated]
	internal static Vector4 <RefreshPropertyBlock>g__NormalisedUV|54_0(Rect spriteRect, Vector2 textureSize)
	{
		return new Vector4(spriteRect.x / textureSize.x, spriteRect.y / textureSize.y, spriteRect.width / textureSize.x, spriteRect.height / textureSize.y);
	}

	// Token: 0x06000C72 RID: 3186 RVA: 0x000633AA File Offset: 0x000615AA
	[CompilerGenerated]
	internal static Vector4 <RefreshPropertyBlock>g__FlipUV|54_1(Vector4 uv)
	{
		return new Vector4(uv.x + uv.z, uv.y, -uv.z, uv.w);
	}

	// Token: 0x04000ECE RID: 3790
	public CloudSpriteSet spriteSet;

	// Token: 0x04000ECF RID: 3791
	public Color color = Color.white;

	// Token: 0x04000ED0 RID: 3792
	[Range(0f, 1f)]
	public float hueShift;

	// Token: 0x04000ED1 RID: 3793
	[Range(0f, 2f)]
	public float saturation = 1f;

	// Token: 0x04000ED2 RID: 3794
	[Range(0f, 3f)]
	public float brightness = 1f;

	// Token: 0x04000ED3 RID: 3795
	[Range(0f, 5f)]
	public float contrast = 1f;

	// Token: 0x04000ED4 RID: 3796
	[Range(0f, 1f)]
	public float fogStrength = 1f;

	// Token: 0x04000ED5 RID: 3797
	public bool flipX;

	// Token: 0x04000ED6 RID: 3798
	public float driftAngle;

	// Token: 0x04000ED7 RID: 3799
	public float driftDistance = 100f;

	// Token: 0x04000ED8 RID: 3800
	public float driftSpeed = 1f;

	// Token: 0x04000ED9 RID: 3801
	[Range(0f, 10f)]
	public float xVariation = 0.2f;

	// Token: 0x04000EDA RID: 3802
	[Range(0f, 4f)]
	public float scaleVariation = 0.2f;

	// Token: 0x04000EDB RID: 3803
	[NonSerialized]
	public bool visible;

	// Token: 0x04000EDC RID: 3804
	[NonSerialized]
	public float posNorm;

	// Token: 0x04000EDD RID: 3805
	[NonSerialized]
	public Vector3 basePosition;

	// Token: 0x04000EDE RID: 3806
	[NonSerialized]
	public Vector3 baseScale;

	// Token: 0x04000EDF RID: 3807
	[NonSerialized]
	public float fadeAlpha = 1f;

	// Token: 0x04000EE0 RID: 3808
	[NonSerialized]
	public bool earlyFadeOut;

	// Token: 0x04000EE1 RID: 3809
	[NonSerialized]
	public Vector3 currentCycleOffset;

	// Token: 0x04000EE2 RID: 3810
	[NonSerialized]
	public float speedVariationScalar;

	// Token: 0x04000EE3 RID: 3811
	[NonSerialized]
	public Color tint = Color.white;

	// Token: 0x04000EE4 RID: 3812
	[NonSerialized]
	public Vector3 cachedAnimationExtent;

	// Token: 0x04000EE5 RID: 3813
	[SerializeField]
	private GuidComponent guidComponent;

	// Token: 0x04000EE6 RID: 3814
	[SerializeField]
	private MeshFilter meshFilter;

	// Token: 0x04000EE7 RID: 3815
	public MeshRenderer meshRenderer;

	// Token: 0x04000EE8 RID: 3816
	private static Dictionary<string, Mesh> loadedMeshesDictionary = new Dictionary<string, Mesh>();

	// Token: 0x04000EE9 RID: 3817
	private static Material _materialPrefab;

	// Token: 0x04000EEA RID: 3818
	private static Dictionary<string, Material> loadedMaterialsDictionary = new Dictionary<string, Material>();

	// Token: 0x04000EEB RID: 3819
	[NonSerialized]
	private bool isDirty;

	// Token: 0x04000EEC RID: 3820
	private static Mesh _quad;

	// Token: 0x04000EED RID: 3821
	private MaterialPropertyBlock _properties;

	// Token: 0x04000EEE RID: 3822
	private bool _doneInitialPropertySetup;

	// Token: 0x04000EEF RID: 3823
	private static bool _generatedIDs;

	// Token: 0x04000EF0 RID: 3824
	private static int _MainTexID;

	// Token: 0x04000EF1 RID: 3825
	private static int _ColorID;

	// Token: 0x04000EF2 RID: 3826
	private static int _HueShiftID;

	// Token: 0x04000EF3 RID: 3827
	private static int _SaturationID;

	// Token: 0x04000EF4 RID: 3828
	private static int _BrightnessID;

	// Token: 0x04000EF5 RID: 3829
	private static int _ContrastID;

	// Token: 0x04000EF6 RID: 3830
	private static int _SizeID;

	// Token: 0x04000EF7 RID: 3831
	private static int _FogStrengthID;

	// Token: 0x04000EF8 RID: 3832
	private static int _MainTexUVID;

	// Token: 0x04000EF9 RID: 3833
	private static int _HighlightTexUVID;

	// Token: 0x04000EFA RID: 3834
	private static int _SoftTexUVID;

	// Token: 0x04000EFB RID: 3835
	private static int _SoftHighlightTexUVID;
}
