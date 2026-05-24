using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Serialization;
using UnityX.MeshBuilder;

// Token: 0x020001AE RID: 430
[RequireComponent(typeof(GuidComponent))]
[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[ExecuteInEditMode]
[DisallowMultipleComponent]
public class Splat : MonoBehaviour
{
	// Token: 0x17000362 RID: 866
	// (get) Token: 0x06000E26 RID: 3622 RVA: 0x0006F473 File Offset: 0x0006D673
	public Splat.SplatMode renderedSplatMode
	{
		get
		{
			if (!this.hasParentPoly)
			{
				return Splat.SplatMode.Normal;
			}
			return this.splatMode;
		}
	}

	// Token: 0x17000363 RID: 867
	// (get) Token: 0x06000E27 RID: 3623 RVA: 0x0006F485 File Offset: 0x0006D685
	// (set) Token: 0x06000E28 RID: 3624 RVA: 0x0006F48D File Offset: 0x0006D68D
	public Poly parentPoly
	{
		get
		{
			return this._parentPoly;
		}
		set
		{
			if (this._parentPoly == value)
			{
				return;
			}
			this._parentPoly = value;
			this._hasParentPoly = this._parentPoly != null;
			this.UpdateVisibility();
		}
	}

	// Token: 0x17000364 RID: 868
	// (get) Token: 0x06000E29 RID: 3625 RVA: 0x0006F4BE File Offset: 0x0006D6BE
	private bool hasParentPoly
	{
		get
		{
			return this._hasParentPoly;
		}
	}

	// Token: 0x17000365 RID: 869
	// (get) Token: 0x06000E2A RID: 3626 RVA: 0x0006F4C6 File Offset: 0x0006D6C6
	public static Material material
	{
		get
		{
			if (Splat._material == null && Application.isPlaying)
			{
				Splat._material = Resources.Load<Material>("SplatterMaterial");
			}
			return Splat._material;
		}
	}

	// Token: 0x17000366 RID: 870
	// (get) Token: 0x06000E2B RID: 3627 RVA: 0x0006F4F0 File Offset: 0x0006D6F0
	public static Material simpleMaterialVariant
	{
		get
		{
			if (Splat._simpleMaterialVariant == null && Application.isPlaying)
			{
				Splat._simpleMaterialVariant = Resources.Load<Material>("SplatterMaterialSimplified");
			}
			return Splat._simpleMaterialVariant;
		}
	}

	// Token: 0x17000367 RID: 871
	// (get) Token: 0x06000E2C RID: 3628 RVA: 0x0006F51A File Offset: 0x0006D71A
	public SurfaceType surfaceType
	{
		get
		{
			if (this.surfaceTypeOverride != SurfaceType.NONE)
			{
				return this.surfaceTypeOverride;
			}
			return this.spriteSurfaceType;
		}
	}

	// Token: 0x17000368 RID: 872
	// (get) Token: 0x06000E2D RID: 3629 RVA: 0x0006F531 File Offset: 0x0006D731
	public bool IgnoreSurfaceType
	{
		get
		{
			return this.surfaceTypePassthrough || this.surfaceType == SurfaceType.NONE;
		}
	}

	// Token: 0x17000369 RID: 873
	// (get) Token: 0x06000E2E RID: 3630 RVA: 0x0006F546 File Offset: 0x0006D746
	// (set) Token: 0x06000E2F RID: 3631 RVA: 0x0006F54E File Offset: 0x0006D74E
	public bool flattenedAndHidden
	{
		get
		{
			return this._flattendAndHidden;
		}
		set
		{
			if (this._flattendAndHidden != value)
			{
				this._flattendAndHidden = value;
				this.UpdateVisibility();
			}
		}
	}

	// Token: 0x06000E30 RID: 3632 RVA: 0x0006F568 File Offset: 0x0006D768
	private void OnEnable()
	{
		this.ValidateGUID();
		if (Application.isPlaying && this.customMesh == null)
		{
			this.customMesh = null;
			this.isDirty = true;
		}
		this.meshRenderer.enabled = true;
		if (this.sprite == null)
		{
			Debug.LogWarning("Splat missing sprite: " + base.name, this);
		}
		if (this.isDirty)
		{
			this.isDirty = false;
			this.Refresh();
			return;
		}
		if (Application.isPlaying && this.customMesh.vertexCount > 0)
		{
			this.UpdateMeshUVsForSpriteAtlas();
		}
		this.UpdateVisibility();
		this.RefreshPropertyBlock();
		this.ApplySortingOrder();
	}

	// Token: 0x06000E31 RID: 3633 RVA: 0x0006F612 File Offset: 0x0006D812
	private void OnDisable()
	{
		this.SetHighlightProperty(0f);
		this.meshRenderer.enabled = false;
		this._properties = null;
	}

	// Token: 0x06000E32 RID: 3634 RVA: 0x0006F634 File Offset: 0x0006D834
	private void OnDestroy()
	{
		if (this.hasParentPoly)
		{
			this.parentPoly.splats.Remove(this);
			this.OnRemovedFromPoly();
		}
		if (this.customMesh != null)
		{
			Splat.DestroyAutomatic(this.customMesh);
			this.customMesh = null;
		}
	}

	// Token: 0x06000E33 RID: 3635 RVA: 0x0006F681 File Offset: 0x0006D881
	private static void DestroyAutomatic(Object o)
	{
		Object.Destroy(o);
	}

	// Token: 0x06000E34 RID: 3636 RVA: 0x0006F68C File Offset: 0x0006D88C
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

	// Token: 0x06000E35 RID: 3637 RVA: 0x0006F6F4 File Offset: 0x0006D8F4
	private void OnSetGUID()
	{
		this.InitialSetUp();
	}

	// Token: 0x06000E36 RID: 3638 RVA: 0x0006F6FC File Offset: 0x0006D8FC
	private void InitialSetUp()
	{
		this.guidComponent.hideFlags = HideFlags.HideInInspector;
		this.meshFilter = base.GetComponent<MeshFilter>();
		this.meshFilter.hideFlags = HideFlags.HideInHierarchy | HideFlags.HideInInspector;
		this.meshRenderer = base.GetComponent<MeshRenderer>();
		this.meshRenderer.hideFlags = HideFlags.HideInHierarchy | HideFlags.HideInInspector;
		this.parentPoly = base.GetComponentInParent<Poly>();
		if (this.hasParentPoly && !this.parentPoly.splats.Contains(this))
		{
			this.parentPoly.splats.Add(this);
			this.OnAddedToPoly(this.parentPoly);
		}
	}

	// Token: 0x06000E37 RID: 3639 RVA: 0x0006F789 File Offset: 0x0006D989
	public void OnAddedToLevel(bool isStatic)
	{
		this.isStatic = isStatic;
		this.UpdateVisibility();
	}

	// Token: 0x06000E38 RID: 3640 RVA: 0x0006F799 File Offset: 0x0006D999
	public void OnAddedToPoly(Poly poly)
	{
		this.parentPoly = poly;
	}

	// Token: 0x06000E39 RID: 3641 RVA: 0x0006F7A2 File Offset: 0x0006D9A2
	public void OnRemovedFromPoly()
	{
		this.parentPoly = null;
	}

	// Token: 0x06000E3A RID: 3642 RVA: 0x0006F7AB File Offset: 0x0006D9AB
	public void RefreshAfterInstantiation()
	{
		this.Refresh();
	}

	// Token: 0x06000E3B RID: 3643 RVA: 0x0006F7B3 File Offset: 0x0006D9B3
	public void OnPolyRefresh()
	{
		this.Refresh();
	}

	// Token: 0x06000E3C RID: 3644 RVA: 0x0006F7BB File Offset: 0x0006D9BB
	public void RefreshFadeDueToPolyCutaway()
	{
		this.UpdateVisibility();
		this.RefreshPropertyBlockFadeOnly();
	}

	// Token: 0x06000E3D RID: 3645 RVA: 0x0006F7CC File Offset: 0x0006D9CC
	public void RefreshMusicRunSplatAlpha(float cameraZ, bool isStaticSplat, Range musicRunGeneratedRange = default(Range))
	{
		bool flag = cameraZ < base.transform.position.z - 70f;
		if (!isStaticSplat)
		{
			flag = !flag;
		}
		if (isStaticSplat && !flag && !musicRunGeneratedRange.Intersects(this.musicRunWorldRangeX))
		{
			flag = true;
		}
		float num = (float)(flag ? 1 : 0);
		if (this.musicRunSplatAlpha != num)
		{
			this.musicRunSplatAlpha = Mathf.MoveTowards(this.musicRunSplatAlpha, num, Time.deltaTime / 1f);
			if (this.UpdateVisibility())
			{
				this.RefreshPropertyBlock();
				return;
			}
			this.RefreshPropertyBlockFadeOnly();
		}
	}

	// Token: 0x06000E3E RID: 3646 RVA: 0x0006F858 File Offset: 0x0006DA58
	public void CalculateXRangeForMusicRunFading()
	{
		Bounds bounds = this.meshRenderer.bounds;
		Vector3 center = bounds.center;
		Vector3 size = bounds.size;
		this.musicRunWorldRangeX = Range.Centered(center.x, size.x);
	}

	// Token: 0x06000E3F RID: 3647 RVA: 0x0006F898 File Offset: 0x0006DA98
	public void Refresh()
	{
		this.isDirty = false;
		this.BuildMesh();
		this.ApplyMesh();
		this.ApplyMaterial();
		this.EnforcePositiveScale();
		this.UpdateVisibility();
		this.RefreshPropertyBlock();
		this.ApplySortingOrder();
	}

	// Token: 0x06000E40 RID: 3648 RVA: 0x0006F8CC File Offset: 0x0006DACC
	private void BuildMesh()
	{
		if (this.renderedSplatMode == Splat.SplatMode.Normal)
		{
			if (this.windObjectType == Splat.WindObjectType.Tree)
			{
				this.CreateQuadMesh(5, 5);
				return;
			}
			this.CreateQuadMesh(1, 1);
			return;
		}
		else
		{
			if (this.renderedSplatMode == Splat.SplatMode.Surface)
			{
				this.CreateSurfaceFollowMesh();
				return;
			}
			if (this.renderedSplatMode == Splat.SplatMode.ClipToPoly)
			{
				this.CreateClipToPolyMesh();
				return;
			}
			Debug.LogWarning("Unrecognised splat mode " + this.renderedSplatMode.ToString());
			if (this.customMesh != null)
			{
				this.customMesh.Clear();
			}
			return;
		}
	}

	// Token: 0x06000E41 RID: 3649 RVA: 0x0006F958 File Offset: 0x0006DB58
	private void UpdateMeshUVsForSpriteAtlas()
	{
		int width = this.sprite.texture.width;
		int height = this.sprite.texture.height;
		Rect textureRect = this.sprite.textureRect;
		Vector2 min = textureRect.min;
		Vector2 max = textureRect.max;
		float num = min.x / (float)width;
		float num2 = min.y / (float)height;
		float num3 = max.x / (float)width;
		float num4 = max.y / (float)height;
		if (this.uvs.Length != this.rawUVs.Length)
		{
			Array.Resize<Vector2>(ref this.uvs, this.rawUVs.Length);
		}
		for (int i = 0; i < this.rawUVs.Length; i++)
		{
			Vector2 vector = this.rawUVs[i];
			this.uvs[i] = new Vector2(Mathf.Lerp(num, num3, vector.x), Mathf.Lerp(num2, num4, vector.y));
		}
		this.customMesh.uv = this.uvs;
		if (this.fastOpaqueRenderMode)
		{
			this.customMesh.SetColors(new Color[] { this.color, this.color, this.color, this.color });
			Vector4 vector2 = new Vector4(this.brightness, this.contrast, this.saturation, this.hueShift);
			this.customMesh.SetUVs(1, new Vector4[] { vector2, vector2, vector2, vector2 });
		}
	}

	// Token: 0x06000E42 RID: 3650 RVA: 0x0006FB00 File Offset: 0x0006DD00
	private void ApplyMesh()
	{
		this.meshFilter.mesh = this.customMesh;
	}

	// Token: 0x06000E43 RID: 3651 RVA: 0x0006FB14 File Offset: 0x0006DD14
	public void ApplyMaterial()
	{
		Material sharedMaterial = this.meshRenderer.sharedMaterial;
		if (sharedMaterial != null && sharedMaterial != Splat.simpleMaterialVariant && sharedMaterial != Splat.material)
		{
			return;
		}
		if (this.simplifiedColouring)
		{
			this.meshRenderer.material = Splat.simpleMaterialVariant;
			return;
		}
		this.meshRenderer.material = Splat.material;
	}

	// Token: 0x06000E44 RID: 3652 RVA: 0x0006FB7C File Offset: 0x0006DD7C
	private void EnforcePositiveScale()
	{
		Vector3 localScale = base.transform.localScale;
		base.transform.localScale = new Vector3(Mathf.Abs(localScale.x), Mathf.Abs(localScale.y), Mathf.Abs(localScale.z));
	}

	// Token: 0x06000E45 RID: 3653 RVA: 0x0006FBC8 File Offset: 0x0006DDC8
	public bool UpdateVisibility()
	{
		bool flag = true;
		if (this._flattendAndHidden)
		{
			flag = false;
		}
		else if (this.hasParentPoly && this._parentPoly.cutawayWallFullyHidden)
		{
			flag = false;
		}
		else if (this.musicRunSplatAlpha == 0f)
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

	// Token: 0x06000E46 RID: 3654 RVA: 0x0006FC28 File Offset: 0x0006DE28
	private void SetMaterialPropertyBlock()
	{
		if (this.fastOpaqueRenderMode)
		{
			this._properties = null;
			return;
		}
		if (this.sprite == null)
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

	// Token: 0x06000E47 RID: 3655 RVA: 0x0006FC7C File Offset: 0x0006DE7C
	private static void SetShaderPropertyIDs()
	{
		if (Splat._generatedIDs)
		{
			return;
		}
		Splat._MainTexID = Shader.PropertyToID("_MainTex");
		Splat._UVScaleID = Shader.PropertyToID("_UVScale");
		Splat._ColorID = Shader.PropertyToID("_Color");
		Splat._HueShiftID = Shader.PropertyToID("_HueShift");
		Splat._SaturationID = Shader.PropertyToID("_Saturation");
		Splat._BrightnessID = Shader.PropertyToID("_Brightness");
		Splat._ContrastID = Shader.PropertyToID("_Contrast");
		Splat._BackLitContributionID = Shader.PropertyToID("_BackLitContribution");
		Splat._ShadowID = Shader.PropertyToID("_Shadow");
		Splat._fadeId = Shader.PropertyToID("_Fade");
		Splat._highlightId = Shader.PropertyToID("_HighlightOpacity");
		Splat._WindEffectStrengthId = Shader.PropertyToID("_WindEffectStrength");
		Splat._WindObjectTypeId = Shader.PropertyToID("_WindObjectType");
		Splat._WindEffectPivotId = Shader.PropertyToID("_WindEffectPivot");
		Splat._generatedIDs = true;
	}

	// Token: 0x1700036A RID: 874
	// (get) Token: 0x06000E48 RID: 3656 RVA: 0x0006FD6C File Offset: 0x0006DF6C
	public float finalRenderAlpha
	{
		get
		{
			float num = Mathf.InverseLerp(0f, 0.65f, this.musicRunSplatAlpha);
			return this.color.a * (this.hasParentPoly ? this._parentPoly.cutawayAlpha : 1f) * num;
		}
	}

	// Token: 0x06000E49 RID: 3657 RVA: 0x0006FDB8 File Offset: 0x0006DFB8
	public void RefreshPropertyBlock()
	{
		if (!this.PrePropertyBlockRefresh())
		{
			return;
		}
		Rect textureRect = this.sprite.textureRect;
		int width = this.sprite.texture.width;
		int height = this.sprite.texture.height;
		Splat.SetMaterialBlockProperties(this._properties, this.sprite, new Vector2(this.sprite.textureRect.width / (float)this.sprite.texture.width, this.sprite.textureRect.height / (float)this.sprite.texture.height), this.color.WithAlpha(this.finalRenderAlpha), this.hueShift, this.saturation, this.brightness, this.contrast, this.backLitContribution, this.isShadow, 1f - this.finalRenderAlpha, this.windEffect, (int)this.windObjectType, new Vector2(Mathf.Lerp(textureRect.xMin / (float)width, textureRect.xMax / (float)width, this.windEffectPivot.x), Mathf.Lerp(textureRect.yMin / (float)height, textureRect.yMax / (float)height, this.windEffectPivot.y)));
		this.meshRenderer.SetPropertyBlock(this._properties);
		this.isPropertyBlockDirty = false;
	}

	// Token: 0x06000E4A RID: 3658 RVA: 0x0006FF10 File Offset: 0x0006E110
	public static void SetMaterialBlockProperties(MaterialPropertyBlock props, Sprite sprite, Vector2 uvScale, Color color, float hueShiftNorm = 0f, float saturation = 1f, float brightness = 1f, float contrast = 1f, float backLitContribution = 0f, bool isShadow = false, float fade = 0f, float windEffect = 0f, int windObjectType = 0, Vector2 windPivot = default(Vector2))
	{
		props.SetTexture(Splat._MainTexID, sprite.texture);
		props.SetVector(Splat._UVScaleID, uvScale);
		props.SetColor(Splat._ColorID, color);
		props.SetFloat(Splat._HueShiftID, hueShiftNorm * 2f * 3.1415927f);
		props.SetFloat(Splat._SaturationID, saturation);
		props.SetFloat(Splat._BrightnessID, brightness);
		props.SetFloat(Splat._ContrastID, contrast);
		props.SetFloat(Splat._BackLitContributionID, backLitContribution);
		props.SetFloat(Splat._ShadowID, (float)(isShadow ? 1 : 0));
		props.SetFloat(Splat._fadeId, fade);
		props.SetFloat(Splat._WindEffectStrengthId, windEffect);
		props.SetInt(Splat._WindObjectTypeId, windObjectType);
		props.SetVector(Splat._WindEffectPivotId, windPivot);
	}

	// Token: 0x06000E4B RID: 3659 RVA: 0x0006FFE5 File Offset: 0x0006E1E5
	public void SetHighlightProperty(float highlightOpacity)
	{
		this.SetMaterialPropertyBlock();
		if (this._properties == null)
		{
			return;
		}
		this._properties.SetFloat(Splat._highlightId, highlightOpacity);
		this.meshRenderer.SetPropertyBlock(this._properties);
	}

	// Token: 0x06000E4C RID: 3660 RVA: 0x00070018 File Offset: 0x0006E218
	public void RefreshPropertyBlockFadeOnly()
	{
		if (!this.PrePropertyBlockRefresh())
		{
			return;
		}
		float finalRenderAlpha = this.finalRenderAlpha;
		this._properties.SetColor(Splat._ColorID, this.color.WithAlpha(finalRenderAlpha));
		this._properties.SetFloat(Splat._fadeId, 1f - finalRenderAlpha);
		this.meshRenderer.SetPropertyBlock(this._properties);
	}

	// Token: 0x06000E4D RID: 3661 RVA: 0x00070079 File Offset: 0x0006E279
	private bool PrePropertyBlockRefresh()
	{
		if (!this.meshRenderer.enabled)
		{
			return false;
		}
		if (this.fastOpaqueRenderMode)
		{
			return false;
		}
		this.SetMaterialPropertyBlock();
		if (this._properties == null || this.sprite == null)
		{
			return false;
		}
		Splat.SetShaderPropertyIDs();
		return true;
	}

	// Token: 0x06000E4E RID: 3662 RVA: 0x000700B8 File Offset: 0x0006E2B8
	public bool ContainsPoint(Vector3 point)
	{
		Vector2 vector = point;
		Bounds bounds = this.meshRenderer.bounds;
		Vector3 center = bounds.center;
		Vector3 size = bounds.size;
		Rect rect = new Rect(center.x - size.x * 0.5f, center.y - size.y * 0.5f, size.x, size.y + 0.01f);
		if (!rect.Contains(vector))
		{
			return false;
		}
		if (this.renderedSplatMode == Splat.SplatMode.ClipToPoly && this.hasParentPoly)
		{
			Splat.containsPointCheckComplexPolygon.Clear();
			this.customMesh.GetVertices(Splat.vertices);
			foreach (Vector3 vector2 in Splat.vertices)
			{
				Splat.containsPointCheckComplexPolygon.Add(vector2);
			}
			return Polygon.ContainsPoint(Splat.containsPointCheckComplexPolygon, base.transform.InverseTransformPoint(vector + Vector2.up * -0.01f));
		}
		Matrix4x4 localToWorldMatrix = base.transform.localToWorldMatrix;
		Splat.containsPointCheckBoundsPolygon[0] = localToWorldMatrix.MultiplyPoint3x4(new Vector3(-0.5f, 0.5f, 0f));
		Splat.containsPointCheckBoundsPolygon[1] = localToWorldMatrix.MultiplyPoint3x4(new Vector3(0.5f, 0.5f, 0f));
		Splat.containsPointCheckBoundsPolygon[2] = localToWorldMatrix.MultiplyPoint3x4(new Vector3(0.5f, -0.5f, 0f));
		Splat.containsPointCheckBoundsPolygon[3] = localToWorldMatrix.MultiplyPoint3x4(new Vector3(-0.5f, -0.5f, 0f));
		return Polygon.ContainsPoint(Splat.containsPointCheckBoundsPolygon, vector);
	}

	// Token: 0x06000E4F RID: 3663 RVA: 0x000702B0 File Offset: 0x0006E4B0
	private void ApplySortingOrder()
	{
	}

	// Token: 0x06000E50 RID: 3664 RVA: 0x000702B4 File Offset: 0x0006E4B4
	private void CreateQuadMesh(int numQuadsX, int numQuadsY)
	{
		if (this.customMesh == null)
		{
			this.customMesh = new Mesh();
		}
		this.customMesh.name = base.gameObject.name;
		float num = this.sprite.textureRect.width / this.sprite.textureRect.height;
		float num2 = ((num > 1f) ? (-0.5f) : (-num * 0.5f));
		float num3 = ((num > 1f) ? 0.5f : (num * 0.5f));
		float num4 = ((num < 1f) ? (-0.5f) : (0.5f / -num));
		float num5 = ((num < 1f) ? 0.5f : (0.5f / num));
		Vector2 vector = new Vector2(1f / (float)numQuadsX, 1f / (float)numQuadsY);
		Vector3[] array = new Vector3[(numQuadsX + 1) * (numQuadsY + 1)];
		if (this.rawUVs.Length != array.Length)
		{
			Array.Resize<Vector2>(ref this.rawUVs, array.Length);
		}
		int num6 = 0;
		for (int i = 0; i <= numQuadsY; i++)
		{
			int j = 0;
			while (j <= numQuadsX)
			{
				array[num6] = new Vector3(Mathf.Lerp(num2, num3, (float)j * vector.x), Mathf.Lerp(num4, num5, (float)i * vector.y), 0f);
				this.rawUVs[num6] = new Vector2((float)j / (float)numQuadsX, (float)i / (float)numQuadsY);
				if (this.flipX)
				{
					this.rawUVs[num6].x = 1f - this.rawUVs[num6].x;
				}
				j++;
				num6++;
			}
		}
		if (this.customMesh.vertexCount != array.Length)
		{
			this.customMesh.Clear();
		}
		this.customMesh.vertices = array;
		this.customMesh.uv = this.rawUVs;
		int[] array2 = new int[numQuadsX * numQuadsY * 6];
		int num7 = 0;
		int num8 = 0;
		int k = 0;
		while (k < numQuadsY)
		{
			int l = 0;
			while (l < numQuadsX)
			{
				array2[num7] = num8;
				array2[num7 + 3] = (array2[num7 + 2] = num8 + 1);
				array2[num7 + 4] = (array2[num7 + 1] = num8 + numQuadsX + 1);
				array2[num7 + 5] = num8 + numQuadsX + 2;
				l++;
				num7 += 6;
				num8++;
			}
			k++;
			num8++;
		}
		this.customMesh.triangles = array2;
		if (Splat.meshBakeParams.recalculateNormals)
		{
			this.customMesh.RecalculateNormals();
		}
		if (Splat.meshBakeParams.recalculateBounds)
		{
			this.customMesh.RecalculateBounds();
		}
		this.UpdateMeshUVsForSpriteAtlas();
	}

	// Token: 0x06000E51 RID: 3665 RVA: 0x0007057C File Offset: 0x0006E77C
	private void CreateClipToPolyMesh()
	{
		if (this.customMesh == null)
		{
			this.customMesh = new Mesh();
		}
		this.customMesh.name = base.gameObject.name;
		if (!this.hasParentPoly)
		{
			return;
		}
		MeshFilter meshFilter = this.parentPoly.MeshFilter;
		if (meshFilter == null || meshFilter.sharedMesh == null)
		{
			return;
		}
		if (Splat.meshBuilder == null)
		{
			Splat.meshBuilder = new MeshBuilder();
		}
		else
		{
			Splat.meshBuilder.Clear();
		}
		if (this.sprite == null)
		{
			this.customMesh.Clear();
			return;
		}
		Vector2[] array = new Vector2[meshFilter.sharedMesh.vertexCount];
		Vector3[] array2 = meshFilter.sharedMesh.vertices;
		for (int i = 0; i < array2.Length; i++)
		{
			Vector3 vector = meshFilter.transform.TransformPoint(array2[i]);
			array[i] = base.transform.InverseTransformPoint(vector);
		}
		float num = this.sprite.textureRect.width / this.sprite.textureRect.height;
		float num2 = ((num > 1f) ? (-0.5f) : (-num * 0.5f));
		float num3 = ((num > 1f) ? 0.5f : (num * 0.5f));
		float num4 = ((num < 1f) ? (-0.5f) : (0.5f / -num));
		float num5 = ((num < 1f) ? 0.5f : (0.5f / num));
		Rect rect = Rect.MinMaxRect(num2, num4, num3, num5);
		Vector2[] array3 = new Vector2[]
		{
			new Vector2(num2, num5),
			new Vector2(num3, num5),
			new Vector2(num3, num4),
			new Vector2(num2, num4)
		};
		List<Vector2> list = new List<Vector2>();
		Polygon.SutherlandHodgman.GetIntersectedPolygon(array, array3, ref list);
		List<int> list2 = new List<int>();
		Triangulator.GenerateIndices(list, list2);
		Vector3[] array4 = new Vector3[list.Count];
		Vector2[] array5 = new Vector2[list.Count];
		for (int j = 0; j < list.Count; j++)
		{
			array4[j] = list[j];
			array5[j].x = Mathf.InverseLerp(rect.xMin, rect.xMax, list[j].x);
			array5[j].y = Mathf.InverseLerp(rect.yMin, rect.yMax, list[j].y);
			if (this.flipX)
			{
				array5[j].x = 1f - array5[j].x;
			}
		}
		if (this.customMesh.vertexCount != array4.Length)
		{
			this.customMesh.Clear();
		}
		this.customMesh.vertices = array4;
		this.customMesh.SetTriangles(list2, 0);
		this.rawUVs = (this.customMesh.uv = array5);
		if (Splat.meshBakeParams.recalculateNormals)
		{
			this.customMesh.RecalculateNormals();
		}
		if (Splat.meshBakeParams.recalculateBounds)
		{
			this.customMesh.RecalculateBounds();
		}
		this.UpdateMeshUVsForSpriteAtlas();
	}

	// Token: 0x06000E52 RID: 3666 RVA: 0x000708D8 File Offset: 0x0006EAD8
	private void CreateSurfaceFollowMesh()
	{
		if (this.customMesh == null)
		{
			this.customMesh = new Mesh();
		}
		this.customMesh.name = base.gameObject.name;
		if (!this.hasParentPoly)
		{
			return;
		}
		MeshFilter meshFilter = this.parentPoly.MeshFilter;
		if (meshFilter == null || meshFilter.sharedMesh == null)
		{
			return;
		}
		Vector3[] surfaceWorldVerts = this.GetSurfaceWorldVerts(meshFilter);
		if (surfaceWorldVerts == null)
		{
			this.customMesh.Clear();
			return;
		}
		if (Splat.meshBuilder == null)
		{
			Splat.meshBuilder = new MeshBuilder();
		}
		else
		{
			Splat.meshBuilder.Clear();
		}
		if (this.sprite == null)
		{
			this.customMesh.Clear();
			return;
		}
		float num = this.sprite.textureRect.width / this.sprite.textureRect.height;
		float num2 = -((Mathf.InverseLerp(this.sprite.textureRectOffset.y, this.sprite.textureRectOffset.y + this.sprite.textureRect.height, this.sprite.pivot.y) - 0.5f) / num);
		float num3 = 0.5f / num + num2;
		float num4 = -0.5f / num + num2;
		Rect rect = new Rect(0f, 0f, 1f, 1f);
		Vector3[] array = Splat.<CreateSurfaceFollowMesh>g__GetExtruded|100_0(surfaceWorldVerts, num4 * base.transform.localScale.x);
		Vector3[] array2 = Splat.<CreateSurfaceFollowMesh>g__GetExtruded|100_0(surfaceWorldVerts, num3 * base.transform.localScale.x);
		float yMax = rect.yMax;
		float yMin = rect.yMin;
		for (int i = 0; i < surfaceWorldVerts.Length - 1; i++)
		{
			Vector3 vector = array[i] - array[i + 1];
			Vector3 vector2 = array2[i] - array2[i + 1];
			if (Vector3.Dot(vector.normalized, vector2.normalized) < 0f)
			{
				array[i + 1] = array[i];
			}
		}
		Vector3[] array3 = new Vector3[array.Length];
		for (int j = 0; j < array3.Length; j++)
		{
			array3[j] = meshFilter.transform.InverseTransformPoint(array[j]);
			array3[j].z = 0f;
			array3[j] = meshFilter.transform.TransformPoint(array3[j]);
			array3[j] = base.transform.InverseTransformPoint(array3[j]);
		}
		Vector3[] array4 = new Vector3[array2.Length];
		for (int k = 0; k < array2.Length; k++)
		{
			array4[k] = meshFilter.transform.InverseTransformPoint(array2[k]);
			array4[k].z = 0f;
			array4[k] = meshFilter.transform.TransformPoint(array4[k]);
			array4[k] = base.transform.InverseTransformPoint(array4[k]);
		}
		float num5 = 0f;
		for (int l = 0; l < array3.Length - 1; l++)
		{
			if (l < array3.Length - 1)
			{
				num5 += Vector2.Distance(array3[l], array3[l + 1]);
			}
		}
		float num6 = 0f;
		for (int m = 0; m < array4.Length - 1; m++)
		{
			if (m < array4.Length - 1)
			{
				num6 += Vector2.Distance(array4[m], array4[m + 1]);
			}
		}
		float num7 = 0f;
		float num8 = ((num5 == 0f) ? 0f : (1f / num5));
		float num9 = 0f;
		float num10 = ((num6 == 0f) ? 0f : (1f / num6));
		int num11 = array3.Length - 1;
		int num12 = 1;
		Vector3[] array5 = new Vector3[(num11 + 1) * (num12 + 1)];
		if (this.customMesh.vertexCount != array5.Length)
		{
			this.customMesh.Clear();
		}
		if (num11 > 0)
		{
			Vector2 vector3 = new Vector2(1f / (float)num11, 1f / (float)num12);
			Vector2[] array6 = new Vector2[array5.Length];
			int num13 = 0;
			for (int n = 0; n <= num11; n++)
			{
				Vector3 vector4 = array3[n];
				Vector3 vector5 = array4[n];
				float num14 = num8 * num7;
				float num15 = num10 * num9;
				Vector2 vector6 = new Vector2(num14, yMin);
				Vector2 vector7 = new Vector2(num15, yMax);
				if (this.flipX)
				{
					vector6 = new Vector2(1f - vector6.x, vector6.y);
					vector7 = new Vector2(1f - vector7.x, vector7.y);
				}
				int num16 = num12;
				while (num16 >= 0)
				{
					float num17 = (float)num16 * vector3.y;
					array5[num13] = new Vector3(Mathf.Lerp(vector4.x, vector5.x, num17), Mathf.Lerp(vector4.y, vector5.y, num17), 0f);
					array6[num13] = new Vector2(Mathf.Lerp(vector6.x, vector7.x, num17), Mathf.Lerp(vector6.y, vector7.y, num17));
					num16--;
					num13++;
				}
				if (n < array3.Length - 1)
				{
					float num18 = Vector2.Distance(array3[n], array3[n + 1]);
					float num19 = Vector2.Distance(array4[n], array4[n + 1]);
					num7 += num18;
					num9 += num19;
				}
			}
			this.customMesh.vertices = array5;
			this.rawUVs = (this.customMesh.uv = array6);
			int[] array7 = new int[num11 * num12 * 6];
			int num20 = 0;
			int num21 = 0;
			int num22 = 0;
			while (num22 < num11)
			{
				int num23 = 0;
				while (num23 < num12)
				{
					array7[num20] = num21;
					array7[num20 + 3] = (array7[num20 + 2] = num21 + 1);
					array7[num20 + 4] = (array7[num20 + 1] = num21 + num12 + 1);
					array7[num20 + 5] = num21 + num12 + 2;
					num23++;
					num20 += 6;
					num21++;
				}
				num22++;
				num21++;
			}
			this.customMesh.triangles = array7;
			if (Splat.meshBakeParams.recalculateNormals)
			{
				this.customMesh.RecalculateNormals();
			}
			if (Splat.meshBakeParams.recalculateBounds)
			{
				this.customMesh.RecalculateBounds();
			}
		}
		this.rawUVs = this.customMesh.uv;
		this.UpdateMeshUVsForSpriteAtlas();
	}

	// Token: 0x06000E53 RID: 3667 RVA: 0x00070FE8 File Offset: 0x0006F1E8
	private Vector3[] GetSurfaceWorldVerts(MeshFilter parentMeshFilter)
	{
		Splat.<>c__DisplayClass101_0 CS$<>8__locals1;
		CS$<>8__locals1.<>4__this = this;
		Rect rect = Rect.MinMaxRect(-0.5f, -0.5f, 0.5f, float.PositiveInfinity);
		CS$<>8__locals1.rectBottomLine = new Line(new Vector2(rect.xMin, rect.yMin), new Vector2(rect.xMax, rect.yMin));
		CS$<>8__locals1.rectLeftLine = new Line(new Vector2(rect.xMin, rect.yMin), new Vector2(rect.xMin, 0.5f));
		CS$<>8__locals1.rectRightLine = new Line(new Vector2(rect.xMax, rect.yMin), new Vector2(rect.xMax, 0.5f));
		Matrix4x4 matrix4x = base.transform.worldToLocalMatrix * parentMeshFilter.transform.localToWorldMatrix;
		Vector3[] array = parentMeshFilter.sharedMesh.vertices;
		int num = -1;
		for (int i = 0; i < array.Length; i++)
		{
			Vector3 vector = matrix4x.MultiplyPoint3x4(array[i]);
			if (rect.Contains(vector))
			{
				num = i;
				break;
			}
		}
		if (num == -1)
		{
			Vector3[] array2 = new Vector3[2];
			int num2 = 0;
			for (int j = 0; j < array.Length; j++)
			{
				Line line = new Line(matrix4x.MultiplyPoint3x4(array.GetRepeating(j)), matrix4x.MultiplyPoint3x4(array.GetRepeating(j + 1)));
				Vector2 vector2;
				if (Line.LineIntersectionPoint(line, CS$<>8__locals1.rectLeftLine, out vector2, true, true))
				{
					array2[num2] = base.transform.TransformPoint(vector2);
					num2++;
					if (num2 == 2)
					{
						break;
					}
				}
				if (Line.LineIntersectionPoint(line, CS$<>8__locals1.rectBottomLine, out vector2, true, true))
				{
					array2[num2] = base.transform.TransformPoint(vector2);
					num2++;
					if (num2 == 2)
					{
						break;
					}
				}
				if (Line.LineIntersectionPoint(line, CS$<>8__locals1.rectRightLine, out vector2, true, true))
				{
					array2[num2] = base.transform.TransformPoint(vector2);
					num2++;
					if (num2 == 2)
					{
						break;
					}
				}
			}
			if (num2 == 2)
			{
				for (int k = 0; k < array2.Length; k++)
				{
					array2[k] = base.transform.InverseTransformPoint(array2[k]);
					array2[k].z = 0f;
					array2[k] = base.transform.TransformPoint(array2[k]);
				}
				return array2;
			}
			Debug.LogWarning("Splat could not intersect with poly. No verts intersect, and no edges intersect without extending infinitely.", this);
			return null;
		}
		else
		{
			int num3 = num;
			for (int l = 0; l < array.Length; l++)
			{
				int num4 = num - l;
				int repeatingIndex = array.GetRepeatingIndex(num4);
				if (!rect.Contains(matrix4x.MultiplyPoint3x4(array[repeatingIndex])))
				{
					num3 = num4 + 1;
					break;
				}
			}
			int num5 = num;
			for (int m = 0; m < array.Length; m++)
			{
				int num6 = num + m;
				int repeatingIndex2 = array.GetRepeatingIndex(num6);
				if (!rect.Contains(matrix4x.MultiplyPoint3x4(array[repeatingIndex2])))
				{
					num5 = num6 - 1;
					break;
				}
			}
			bool flag = this.<GetSurfaceWorldVerts>g__GetIsClockwise|101_2(array, ref CS$<>8__locals1);
			int num7 = Mathf.Abs(num3 - num5) + 1;
			Vector3[] array3 = new Vector3[num7 + 2];
			for (int n = 0; n < num7; n++)
			{
				int repeatingIndex3 = array.GetRepeatingIndex(num3 + n);
				Vector3 vector3 = array[repeatingIndex3];
				Vector3 vector4 = parentMeshFilter.transform.TransformPoint(vector3);
				int num8 = (flag ? (n + 1) : (num7 - n));
				array3[num8] = vector4;
			}
			Vector3 vector5;
			Vector3 vector6;
			Vector3 vector7;
			Vector3 vector8;
			if (flag)
			{
				vector5 = array.GetRepeating(num3);
				vector6 = array.GetRepeating(num5);
				vector7 = array.GetRepeating(num3 - 1);
				vector8 = array.GetRepeating(num5 + 1);
			}
			else
			{
				vector5 = array.GetRepeating(num5);
				vector6 = array.GetRepeating(num3);
				vector8 = array.GetRepeating(num3 - 1);
				vector7 = array.GetRepeating(num5 + 1);
			}
			Line line2 = new Line(matrix4x.MultiplyPoint3x4(vector5), matrix4x.MultiplyPoint3x4(vector7));
			array3[0] = base.transform.TransformPoint(this.<GetSurfaceWorldVerts>g__GetPointOnRectEdge|101_1(line2, ref CS$<>8__locals1));
			Line line3 = new Line(matrix4x.MultiplyPoint3x4(vector6), matrix4x.MultiplyPoint3x4(vector8));
			array3[array3.Length - 1] = base.transform.TransformPoint(this.<GetSurfaceWorldVerts>g__GetPointOnRectEdge|101_1(line3, ref CS$<>8__locals1));
			List<Vector3> list = new List<Vector3>(array3);
			for (int num9 = 0; num9 < list.Count; num9++)
			{
				list[num9] = base.transform.InverseTransformPoint(list[num9]);
				list[num9] = new Vector3(list[num9].x, list[num9].y, 0f);
			}
			float num10 = float.PositiveInfinity;
			Vector3 vector9 = Vector3.zero;
			Vector2 vector10 = Vector3.zero;
			int num11 = -1;
			Line line4 = new Line(Vector2.zero - Vector2.up * 10000f, Vector2.zero + Vector2.up * 10000f);
			for (int num12 = 0; num12 < list.Count - 1; num12++)
			{
				if (Line.LineIntersectionPoint(new Line(list[num12], list[num12 + 1]), line4, out vector10, true, true))
				{
					float magnitude = vector10.magnitude;
					if (magnitude < num10)
					{
						num10 = magnitude;
						vector9 = vector10;
						num11 = num12 + 1;
					}
				}
			}
			if (num11 == -1)
			{
				return new Vector3[0];
			}
			vector9 = base.transform.TransformPoint(vector9);
			for (int num13 = 0; num13 < list.Count; num13++)
			{
				list[num13] = base.transform.TransformPoint(list[num13]);
			}
			list.Insert(num11, vector9);
			Splat.<>c__DisplayClass101_1 CS$<>8__locals2;
			CS$<>8__locals2.minAllowedDot = 0.6f;
			List<Vector3> list2 = this.<GetSurfaceWorldVerts>g__GetVertsInDirection|101_3(list, num11, ref CS$<>8__locals1, ref CS$<>8__locals2);
			list.Reverse();
			List<Vector3> list3 = this.<GetSurfaceWorldVerts>g__GetVertsInDirection|101_3(list, list.Count - 1 - num11, ref CS$<>8__locals1, ref CS$<>8__locals2);
			list3.Reverse();
			return list3.Concat(list2).Distinct<Vector3>().ToArray<Vector3>();
		}
	}

	// Token: 0x06000E56 RID: 3670 RVA: 0x00071710 File Offset: 0x0006F910
	[CompilerGenerated]
	internal static Vector3[] <CreateSurfaceFollowMesh>g__GetExtruded|100_0(Vector3[] vertices, float extrusion)
	{
		Splat.<>c__DisplayClass100_0 CS$<>8__locals1;
		CS$<>8__locals1.vertices = vertices;
		CS$<>8__locals1.clockwise = true;
		Vector3[] array = new Vector3[CS$<>8__locals1.vertices.Length];
		Vector2 zero = Vector2.zero;
		Vector2 zero2 = Vector2.zero;
		for (int i = 0; i < CS$<>8__locals1.vertices.Length; i++)
		{
			Vector2 vector = CS$<>8__locals1.vertices[i];
			array[i] = vector + Splat.<CreateSurfaceFollowMesh>g__TangentToNormal|100_2(Splat.<CreateSurfaceFollowMesh>g__GetTangent|100_1(i, ref CS$<>8__locals1), ref CS$<>8__locals1) * extrusion;
			array[i] = new Vector2(array[i].x, array[i].y);
		}
		return array;
	}

	// Token: 0x06000E57 RID: 3671 RVA: 0x000717C0 File Offset: 0x0006F9C0
	[CompilerGenerated]
	internal static Vector2 <CreateSurfaceFollowMesh>g__GetTangent|100_1(int i, ref Splat.<>c__DisplayClass100_0 A_1)
	{
		Vector2 vector = Vector2.right;
		if (i > 0 && i < A_1.vertices.Length - 1)
		{
			vector = Vector3.Slerp((A_1.vertices[i] - A_1.vertices[i - 1]).normalized, (A_1.vertices[i + 1] - A_1.vertices[i]).normalized, 0.5f);
		}
		else if (i > 0)
		{
			vector = (A_1.vertices[i] - A_1.vertices[i - 1]).normalized;
		}
		else
		{
			vector = (A_1.vertices[i + 1] - A_1.vertices[i]).normalized;
		}
		return vector;
	}

	// Token: 0x06000E58 RID: 3672 RVA: 0x000718A8 File Offset: 0x0006FAA8
	[CompilerGenerated]
	internal static Vector2 <CreateSurfaceFollowMesh>g__TangentToNormal|100_2(Vector2 tangent, ref Splat.<>c__DisplayClass100_0 A_1)
	{
		Vector2 vector = new Vector2(-tangent.y, tangent.x);
		if (!A_1.clockwise)
		{
			vector = -vector;
		}
		return vector;
	}

	// Token: 0x06000E59 RID: 3673 RVA: 0x000718DC File Offset: 0x0006FADC
	[CompilerGenerated]
	private bool <GetSurfaceWorldVerts>g__TryGetPointOnRectEdge|101_0(Line line, out Vector2 intersectionPoint, ref Splat.<>c__DisplayClass101_0 A_3)
	{
		intersectionPoint = Vector2.zero;
		return Line.LineIntersectionPoint(line, A_3.rectBottomLine, out intersectionPoint, true, true) || Line.LineIntersectionPoint(line, A_3.rectLeftLine, out intersectionPoint, true, false) || Line.LineIntersectionPoint(line, A_3.rectRightLine, out intersectionPoint, true, false);
	}

	// Token: 0x06000E5A RID: 3674 RVA: 0x00071930 File Offset: 0x0006FB30
	[CompilerGenerated]
	private Vector2 <GetSurfaceWorldVerts>g__GetPointOnRectEdge|101_1(Line line, ref Splat.<>c__DisplayClass101_0 A_2)
	{
		Vector2 vector;
		if (this.<GetSurfaceWorldVerts>g__TryGetPointOnRectEdge|101_0(line, out vector, ref A_2))
		{
			return vector;
		}
		return Vector2.zero;
	}

	// Token: 0x06000E5B RID: 3675 RVA: 0x00071950 File Offset: 0x0006FB50
	[CompilerGenerated]
	private bool <GetSurfaceWorldVerts>g__GetIsClockwise|101_2(Vector3[] verts, ref Splat.<>c__DisplayClass101_0 A_2)
	{
		float num = 0f;
		Vector2 vector = verts[verts.Length - 1];
		Vector2 vector2 = verts[0];
		num += (vector2.x - vector.x) * (vector2.y + vector.y);
		for (int i = 1; i < verts.Length; i++)
		{
			vector = vector2;
			vector2 = verts[i];
			num += (vector2.x - vector.x) * (vector2.y + vector.y);
		}
		return (num >= 0f) ^ (base.transform.worldToLocalMatrix.m00 < 0f);
	}

	// Token: 0x06000E5C RID: 3676 RVA: 0x00071A00 File Offset: 0x0006FC00
	[CompilerGenerated]
	private List<Vector3> <GetSurfaceWorldVerts>g__GetVertsInDirection|101_3(List<Vector3> verts, int startIndex, ref Splat.<>c__DisplayClass101_0 A_3, ref Splat.<>c__DisplayClass101_1 A_4)
	{
		List<Vector3> list = new List<Vector3>();
		int i = startIndex;
		while (i < verts.Count - 1)
		{
			if (Mathf.Abs(Vector3.Dot((verts[i + 1] - verts[i]).normalized, base.transform.right)) > A_4.minAllowedDot)
			{
				if (i != startIndex && list.Count == 0)
				{
					Line line = new Line(base.transform.InverseTransformPoint(Vector3.Lerp(verts[i + 1], verts[i], 0.5f)), base.transform.InverseTransformPoint(verts[i]));
					list.Add(base.transform.TransformPoint(this.<GetSurfaceWorldVerts>g__GetPointOnRectEdge|101_1(line, ref A_3)));
				}
				if (!list.Contains(verts[i]))
				{
					list.Add(verts[i]);
				}
				list.Add(verts[i + 1]);
				i++;
			}
			else
			{
				if (list.Count == 1)
				{
					Line line2 = new Line(base.transform.InverseTransformPoint(list[list.Count - 1]), base.transform.right);
					list.Add(base.transform.TransformPoint(this.<GetSurfaceWorldVerts>g__GetPointOnRectEdge|101_1(line2, ref A_3)));
					break;
				}
				if (list.Count > 1)
				{
					Line line3 = new Line(base.transform.InverseTransformPoint(Vector3.Lerp(list[list.Count - 2], list[list.Count - 1], 0.5f)), base.transform.InverseTransformPoint(list[list.Count - 1]));
					list.Add(base.transform.TransformPoint(this.<GetSurfaceWorldVerts>g__GetPointOnRectEdge|101_1(line3, ref A_3)));
					break;
				}
				break;
			}
		}
		return list;
	}

	// Token: 0x040010FB RID: 4347
	[NonSerialized]
	private bool isStatic;

	// Token: 0x040010FC RID: 4348
	private LevelSection level;

	// Token: 0x040010FD RID: 4349
	private bool _hasLevel;

	// Token: 0x040010FE RID: 4350
	[FormerlySerializedAs("cameraDistanceBasedLODs")]
	public bool musicRunFadeSplat;

	// Token: 0x040010FF RID: 4351
	[NonSerialized]
	public float musicRunSplatAlpha = 1f;

	// Token: 0x04001100 RID: 4352
	[NonSerialized]
	public Range musicRunWorldRangeX;

	// Token: 0x04001101 RID: 4353
	public Sprite sprite;

	// Token: 0x04001102 RID: 4354
	public bool simplifiedColouring;

	// Token: 0x04001103 RID: 4355
	public Color color = Color.white;

	// Token: 0x04001104 RID: 4356
	[Range(0f, 1f)]
	public float hueShift;

	// Token: 0x04001105 RID: 4357
	[Range(0f, 2f)]
	public float saturation = 1f;

	// Token: 0x04001106 RID: 4358
	[Range(0f, 5f)]
	public float brightness = 1f;

	// Token: 0x04001107 RID: 4359
	[Range(0f, 5f)]
	public float contrast = 1f;

	// Token: 0x04001108 RID: 4360
	public Splat.SplatMode splatMode;

	// Token: 0x04001109 RID: 4361
	public bool flipX;

	// Token: 0x0400110A RID: 4362
	[Range(0f, 1f)]
	public float backLitContribution;

	// Token: 0x0400110B RID: 4363
	public bool isShadow;

	// Token: 0x0400110C RID: 4364
	[Range(0f, 1f)]
	public float windEffect;

	// Token: 0x0400110D RID: 4365
	public Splat.WindObjectType windObjectType;

	// Token: 0x0400110E RID: 4366
	public Vector2 windEffectPivot = new Vector2(0.5f, 0.5f);

	// Token: 0x0400110F RID: 4367
	[SerializeField]
	private GuidComponent guidComponent;

	// Token: 0x04001110 RID: 4368
	[SerializeField]
	private MeshFilter meshFilter;

	// Token: 0x04001111 RID: 4369
	public bool fastOpaqueRenderMode;

	// Token: 0x04001112 RID: 4370
	public MeshRenderer meshRenderer;

	// Token: 0x04001113 RID: 4371
	[NonSerialized]
	private Poly _parentPoly;

	// Token: 0x04001114 RID: 4372
	[NonSerialized]
	private bool _hasParentPoly;

	// Token: 0x04001115 RID: 4373
	[SerializeField]
	public Mesh customMesh;

	// Token: 0x04001116 RID: 4374
	[SerializeField]
	private Vector2[] rawUVs = new Vector2[0];

	// Token: 0x04001117 RID: 4375
	[SerializeField]
	private Vector2[] uvs = new Vector2[0];

	// Token: 0x04001118 RID: 4376
	private bool isDirty;

	// Token: 0x04001119 RID: 4377
	[NonSerialized]
	public bool isPropertyBlockDirty;

	// Token: 0x0400111A RID: 4378
	private static MeshBuilder meshBuilder = new MeshBuilder();

	// Token: 0x0400111B RID: 4379
	private static MeshBakeParams meshBakeParams = new MeshBakeParams(true, true);

	// Token: 0x0400111C RID: 4380
	private static Material _material;

	// Token: 0x0400111D RID: 4381
	private static Material _simpleMaterialVariant;

	// Token: 0x0400111E RID: 4382
	[SerializeField]
	private SurfaceType spriteSurfaceType;

	// Token: 0x0400111F RID: 4383
	[SerializeField]
	private bool surfaceTypePassthrough;

	// Token: 0x04001120 RID: 4384
	[SerializeField]
	private SurfaceType surfaceTypeOverride;

	// Token: 0x04001121 RID: 4385
	private bool _flattendAndHidden;

	// Token: 0x04001122 RID: 4386
	[SerializeField]
	private Transform parent;

	// Token: 0x04001123 RID: 4387
	[SerializeField]
	private int siblingIndex = -1;

	// Token: 0x04001124 RID: 4388
	[SerializeField]
	private Matrix4x4 transformMatrix = Matrix4x4.identity;

	// Token: 0x04001125 RID: 4389
	private static Vector2[] containsPointCheckBoundsPolygon = new Vector2[4];

	// Token: 0x04001126 RID: 4390
	private static List<Vector2> containsPointCheckComplexPolygon = new List<Vector2>();

	// Token: 0x04001127 RID: 4391
	private static List<Vector3> vertices = new List<Vector3>();

	// Token: 0x04001128 RID: 4392
	private MaterialPropertyBlock _properties;

	// Token: 0x04001129 RID: 4393
	private static bool _generatedIDs;

	// Token: 0x0400112A RID: 4394
	private static int _MainTexID;

	// Token: 0x0400112B RID: 4395
	private static int _UVScaleID;

	// Token: 0x0400112C RID: 4396
	private static int _ColorID;

	// Token: 0x0400112D RID: 4397
	private static int _HueShiftID;

	// Token: 0x0400112E RID: 4398
	private static int _SaturationID;

	// Token: 0x0400112F RID: 4399
	private static int _BrightnessID;

	// Token: 0x04001130 RID: 4400
	private static int _ContrastID;

	// Token: 0x04001131 RID: 4401
	private static int _BackLitContributionID;

	// Token: 0x04001132 RID: 4402
	private static int _ShadowID;

	// Token: 0x04001133 RID: 4403
	private static int _fadeId;

	// Token: 0x04001134 RID: 4404
	private static int _highlightId;

	// Token: 0x04001135 RID: 4405
	private static int _WindEffectStrengthId;

	// Token: 0x04001136 RID: 4406
	private static int _WindObjectTypeId;

	// Token: 0x04001137 RID: 4407
	private static int _WindEffectPivotId;

	// Token: 0x020003C0 RID: 960
	public enum SplatMode
	{
		// Token: 0x04001A0D RID: 6669
		Normal,
		// Token: 0x04001A0E RID: 6670
		ClipToPoly,
		// Token: 0x04001A0F RID: 6671
		Surface
	}

	// Token: 0x020003C1 RID: 961
	public enum WindObjectType
	{
		// Token: 0x04001A11 RID: 6673
		None,
		// Token: 0x04001A12 RID: 6674
		Foliage,
		// Token: 0x04001A13 RID: 6675
		Grass,
		// Token: 0x04001A14 RID: 6676
		Tree
	}
}
