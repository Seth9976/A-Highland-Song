using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

// Token: 0x02000188 RID: 392
[ExecuteInEditMode]
[SelectionBase]
public class FlattenSplats : MonoBehaviour
{
	// Token: 0x170002F5 RID: 757
	// (get) Token: 0x06000CD6 RID: 3286 RVA: 0x0006708C File Offset: 0x0006528C
	public Rect clipRect
	{
		get
		{
			return this.insets.ApplyToRect(new Rect(0f, 0f, 1f, 1f), false);
		}
	}

	// Token: 0x170002F6 RID: 758
	// (get) Token: 0x06000CD7 RID: 3287 RVA: 0x000670B3 File Offset: 0x000652B3
	public Level level
	{
		get
		{
			return Level.GetForTransform(base.transform);
		}
	}

	// Token: 0x170002F7 RID: 759
	// (get) Token: 0x06000CD8 RID: 3288 RVA: 0x000670C0 File Offset: 0x000652C0
	public string textureFilePath
	{
		get
		{
			string text = Path.Combine("Assets", "FlattenedSplats", base.gameObject.scene.name);
			if (!Directory.Exists(text))
			{
				Directory.CreateDirectory(text);
			}
			return Path.Combine(text, base.name + ".png");
		}
	}

	// Token: 0x170002F8 RID: 760
	// (get) Token: 0x06000CD9 RID: 3289 RVA: 0x00067118 File Offset: 0x00065318
	public Rect clippedWorldRect
	{
		get
		{
			return new Insets
			{
				top = this.insets.top * this.unclippedWorldRect.height,
				bottom = this.insets.bottom * this.unclippedWorldRect.height,
				left = this.insets.left * this.unclippedWorldRect.width,
				right = this.insets.right * this.unclippedWorldRect.height
			}.ApplyToRect(this.unclippedWorldRect, false);
		}
	}

	// Token: 0x170002F9 RID: 761
	// (get) Token: 0x06000CDA RID: 3290 RVA: 0x000671B8 File Offset: 0x000653B8
	public Bounds worldBoundsForDynamicHighLOD
	{
		get
		{
			if (!this._hasCalculatedWorldBoundsForDynamicHighLOD)
			{
				Rect rect = this.clippedWorldRect.Expand(50f);
				Vector3 vector = rect.center;
				vector.z = this.spriteRenderer.transform.position.z;
				Vector3 vector2 = rect.size;
				vector2.z = 20f;
				this._worldBoundsForDynamicHighLOD = new Bounds(vector, vector2);
				this._hasCalculatedWorldBoundsForDynamicHighLOD = true;
			}
			return this._worldBoundsForDynamicHighLOD;
		}
	}

	// Token: 0x06000CDB RID: 3291 RVA: 0x0006723C File Offset: 0x0006543C
	public bool RequireDynamicHighLODForPos(Vector3 playerPos)
	{
		return !this.allowDynamicMediumLOD || this.worldBoundsForDynamicHighLOD.Contains(playerPos);
	}

	// Token: 0x06000CDC RID: 3292 RVA: 0x00067268 File Offset: 0x00065468
	public void SetFlattenedLOD(LODLevel targetLODLevel)
	{
		bool flag;
		bool flag2;
		if (targetLODLevel == LODLevel.Hide)
		{
			flag = false;
			flag2 = true;
		}
		else if (this.alwaysFlattened)
		{
			if (this.isLowLODParent)
			{
				flag = true;
				flag2 = true;
			}
			else
			{
				flag = true;
				if (targetLODLevel == LODLevel.Low && this.hasLowLODParent)
				{
					flag = false;
				}
				flag2 = true;
			}
		}
		else if (targetLODLevel == LODLevel.High)
		{
			flag = false;
			flag2 = false;
		}
		else if (targetLODLevel == LODLevel.Medium)
		{
			if (this.isLowLODParent)
			{
				flag = false;
				flag2 = false;
			}
			else
			{
				flag = true;
				flag2 = true;
			}
		}
		else if (targetLODLevel == LODLevel.Low)
		{
			if (this.isLowLODParent)
			{
				flag = true;
				flag2 = true;
			}
			else
			{
				flag = !this.hasLowLODParent;
				flag2 = true;
			}
		}
		else
		{
			Debug.LogError("Unexpected LODLevel: " + targetLODLevel.ToString());
			flag = false;
			flag2 = false;
		}
		if (this.showingFlattenedSprite == flag && this.hidingSplatsAndPolys == flag2)
		{
			return;
		}
		int num = 0;
		foreach (Splat splat in this.splats)
		{
			if (splat == null)
			{
				num++;
			}
			else
			{
				splat.flattenedAndHidden = flag2;
			}
		}
		int num2 = 0;
		foreach (Poly poly in this.polys)
		{
			if (poly == null)
			{
				num2++;
			}
			else
			{
				poly.flattenedAndHidden = flag2;
			}
		}
		if (flag && this.spriteRenderer == null)
		{
			Debug.LogWarning("FlattenSprites " + base.name + " has no spriteRenderer assigned", this);
		}
		if (flag && this.spriteRenderer.sprite == null)
		{
			Debug.LogWarning("FlattenSplats " + base.name + " being enabled but sprite hasn't been assigned to spriteRenderer", this);
		}
		else
		{
			this.spriteRenderer.enabled = flag;
		}
		this.showingFlattenedSprite = flag;
		this.hidingSplatsAndPolys = flag2;
	}

	// Token: 0x06000CDD RID: 3293 RVA: 0x0006744C File Offset: 0x0006564C
	public static bool SplatShouldBeFlattened(Splat s)
	{
		if (s.fastOpaqueRenderMode)
		{
			return false;
		}
		if (!s.gameObject.activeInHierarchy)
		{
			return false;
		}
		Prop componentInParent = s.GetComponentInParent<Prop>();
		return (!(componentInParent != null) || !componentInParent.disabledAtStart) && !(s.GetComponentInParent<LODItem>() != null) && !(s.GetComponentInParent<DynamicProp>() != null);
	}

	// Token: 0x06000CDE RID: 3294 RVA: 0x000674AD File Offset: 0x000656AD
	private void Awake()
	{
		if (this.spriteRenderer != null)
		{
			this.spriteRenderer.enabled = false;
		}
		this.showingFlattenedSprite = false;
	}

	// Token: 0x06000CDF RID: 3295 RVA: 0x000674D0 File Offset: 0x000656D0
	private void OnDestroy()
	{
	}

	// Token: 0x04000FA3 RID: 4003
	public int maxResolution = 2048;

	// Token: 0x04000FA4 RID: 4004
	public float flattenedSpriteZOffset;

	// Token: 0x04000FA5 RID: 4005
	public bool alwaysFlattened;

	// Token: 0x04000FA6 RID: 4006
	public bool allowDynamicMediumLOD;

	// Token: 0x04000FA7 RID: 4007
	public Insets insets;

	// Token: 0x04000FA8 RID: 4008
	public List<Transform> externalParentsToSearch;

	// Token: 0x04000FA9 RID: 4009
	[Disable]
	public List<Splat> splats;

	// Token: 0x04000FAA RID: 4010
	[Disable]
	public List<Poly> polys;

	// Token: 0x04000FAB RID: 4011
	[Disable]
	public bool isLowLODParent;

	// Token: 0x04000FAC RID: 4012
	[Disable]
	public bool hasLowLODParent;

	// Token: 0x04000FAD RID: 4013
	[Disable]
	public SpriteRenderer spriteRenderer;

	// Token: 0x04000FAE RID: 4014
	[Disable]
	public Rect unclippedWorldRect;

	// Token: 0x04000FAF RID: 4015
	[NonSerialized]
	public bool showingFlattenedSprite;

	// Token: 0x04000FB0 RID: 4016
	[NonSerialized]
	public bool hidingSplatsAndPolys;

	// Token: 0x04000FB1 RID: 4017
	private Bounds _worldBoundsForDynamicHighLOD;

	// Token: 0x04000FB2 RID: 4018
	private bool _hasCalculatedWorldBoundsForDynamicHighLOD;
}
