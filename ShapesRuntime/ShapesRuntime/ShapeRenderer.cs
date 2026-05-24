using System;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Rendering;

namespace Shapes
{
	// Token: 0x0200000C RID: 12
	[DisallowMultipleComponent]
	public abstract class ShapeRenderer : MonoBehaviour
	{
		// Token: 0x1700007D RID: 125
		// (get) Token: 0x06000134 RID: 308 RVA: 0x00004A04 File Offset: 0x00002C04
		private MaterialPropertyBlock Mpb
		{
			get
			{
				MaterialPropertyBlock materialPropertyBlock;
				if ((materialPropertyBlock = this.mpb) == null)
				{
					materialPropertyBlock = (this.mpb = new MaterialPropertyBlock());
				}
				return materialPropertyBlock;
			}
		}

		// Token: 0x1700007E RID: 126
		// (get) Token: 0x06000135 RID: 309 RVA: 0x00004A29 File Offset: 0x00002C29
		internal bool IsUsingUniqueMaterials
		{
			get
			{
				return !this.IsInstanced;
			}
		}

		// Token: 0x1700007F RID: 127
		// (get) Token: 0x06000136 RID: 310 RVA: 0x00004A34 File Offset: 0x00002C34
		// (set) Token: 0x06000137 RID: 311 RVA: 0x00004A41 File Offset: 0x00002C41
		public Mesh Mesh
		{
			get
			{
				return this.mf.sharedMesh;
			}
			private set
			{
				this.mf.sharedMesh = value;
			}
		}

		// Token: 0x17000080 RID: 128
		// (get) Token: 0x06000138 RID: 312 RVA: 0x00004A50 File Offset: 0x00002C50
		// (set) Token: 0x06000139 RID: 313 RVA: 0x00004A70 File Offset: 0x00002C70
		public int SortingLayerID
		{
			get
			{
				bool flag;
				return this.MakeSureComponentExists<MeshRenderer>(ref this.rnd, out flag).sortingLayerID;
			}
			set
			{
				bool flag;
				this.MakeSureComponentExists<MeshRenderer>(ref this.rnd, out flag).sortingLayerID = value;
			}
		}

		// Token: 0x17000081 RID: 129
		// (get) Token: 0x0600013A RID: 314 RVA: 0x00004A94 File Offset: 0x00002C94
		// (set) Token: 0x0600013B RID: 315 RVA: 0x00004AB4 File Offset: 0x00002CB4
		public int SortingOrder
		{
			get
			{
				bool flag;
				return this.MakeSureComponentExists<MeshRenderer>(ref this.rnd, out flag).sortingOrder;
			}
			set
			{
				bool flag;
				this.MakeSureComponentExists<MeshRenderer>(ref this.rnd, out flag).sortingOrder = value;
			}
		}

		// Token: 0x17000082 RID: 130
		// (get) Token: 0x0600013C RID: 316 RVA: 0x00004AD5 File Offset: 0x00002CD5
		public string SortingLayerName
		{
			get
			{
				return SortingLayer.IDToName(this.SortingLayerID);
			}
		}

		// Token: 0x17000083 RID: 131
		// (get) Token: 0x0600013D RID: 317 RVA: 0x00004AE2 File Offset: 0x00002CE2
		// (set) Token: 0x0600013E RID: 318 RVA: 0x00004AEA File Offset: 0x00002CEA
		public ShapesBlendMode BlendMode
		{
			get
			{
				return this.blendMode;
			}
			set
			{
				this.blendMode = value;
				this.UpdateMaterial();
			}
		}

		// Token: 0x17000084 RID: 132
		// (get) Token: 0x0600013F RID: 319 RVA: 0x00004AF9 File Offset: 0x00002CF9
		// (set) Token: 0x06000140 RID: 320 RVA: 0x00004B04 File Offset: 0x00002D04
		public ScaleMode ScaleMode
		{
			get
			{
				return this.scaleMode;
			}
			set
			{
				int propScaleMode = ShapesMaterialUtils.propScaleMode;
				this.scaleMode = value;
				this.SetIntNow(propScaleMode, (int)value);
			}
		}

		// Token: 0x17000085 RID: 133
		// (get) Token: 0x06000141 RID: 321 RVA: 0x00004B26 File Offset: 0x00002D26
		// (set) Token: 0x06000142 RID: 322 RVA: 0x00004B30 File Offset: 0x00002D30
		public virtual Color Color
		{
			get
			{
				return this.color;
			}
			set
			{
				int propColor = ShapesMaterialUtils.propColor;
				this.color = value;
				this.SetColorNow(propColor, value);
			}
		}

		// Token: 0x17000086 RID: 134
		// (get) Token: 0x06000143 RID: 323 RVA: 0x00004B52 File Offset: 0x00002D52
		// (set) Token: 0x06000144 RID: 324 RVA: 0x00004B5A File Offset: 0x00002D5A
		public virtual DetailLevel DetailLevel
		{
			get
			{
				return this.detailLevel;
			}
			set
			{
				this.detailLevel = value;
				this.UpdateMesh(true);
			}
		}

		// Token: 0x17000087 RID: 135
		// (get) Token: 0x06000145 RID: 325 RVA: 0x00004B6A File Offset: 0x00002D6A
		private bool IsInstanced
		{
			get
			{
				return this.UsingDefaultZTests && this.UsingDefaultStencil;
			}
		}

		// Token: 0x17000088 RID: 136
		// (get) Token: 0x06000146 RID: 326 RVA: 0x00004B7C File Offset: 0x00002D7C
		private bool UsingDefaultZTests
		{
			get
			{
				return this.zTest == CompareFunction.LessEqual && this.zOffsetFactor == 0f && this.zOffsetUnits == 0;
			}
		}

		// Token: 0x17000089 RID: 137
		// (get) Token: 0x06000147 RID: 327 RVA: 0x00004B9F File Offset: 0x00002D9F
		// (set) Token: 0x06000148 RID: 328 RVA: 0x00004BA8 File Offset: 0x00002DA8
		public CompareFunction ZTest
		{
			get
			{
				return this.zTest;
			}
			set
			{
				int propZTest = ShapesMaterialUtils.propZTest;
				this.zTest = value;
				this.SetIntOnAllInstancedMaterials(propZTest, (int)value);
			}
		}

		// Token: 0x1700008A RID: 138
		// (get) Token: 0x06000149 RID: 329 RVA: 0x00004BCA File Offset: 0x00002DCA
		// (set) Token: 0x0600014A RID: 330 RVA: 0x00004BD4 File Offset: 0x00002DD4
		public float ZOffsetFactor
		{
			get
			{
				return this.zOffsetFactor;
			}
			set
			{
				int propZOffsetFactor = ShapesMaterialUtils.propZOffsetFactor;
				this.zOffsetFactor = value;
				this.SetFloatOnAllInstancedMaterials(propZOffsetFactor, value);
			}
		}

		// Token: 0x1700008B RID: 139
		// (get) Token: 0x0600014B RID: 331 RVA: 0x00004BF6 File Offset: 0x00002DF6
		// (set) Token: 0x0600014C RID: 332 RVA: 0x00004C00 File Offset: 0x00002E00
		public int ZOffsetUnits
		{
			get
			{
				return this.zOffsetUnits;
			}
			set
			{
				int propZOffsetUnits = ShapesMaterialUtils.propZOffsetUnits;
				this.zOffsetUnits = value;
				this.SetIntOnAllInstancedMaterials(propZOffsetUnits, value);
			}
		}

		// Token: 0x1700008C RID: 140
		// (get) Token: 0x0600014D RID: 333 RVA: 0x00004C22 File Offset: 0x00002E22
		private bool UsingDefaultStencil
		{
			get
			{
				return this.stencilComp == CompareFunction.Always && this.stencilOpPass == StencilOp.Keep && this.stencilRefID == 0 && this.stencilReadMask == byte.MaxValue && this.stencilWriteMask == byte.MaxValue;
			}
		}

		// Token: 0x1700008D RID: 141
		// (get) Token: 0x0600014E RID: 334 RVA: 0x00004C59 File Offset: 0x00002E59
		// (set) Token: 0x0600014F RID: 335 RVA: 0x00004C64 File Offset: 0x00002E64
		public CompareFunction StencilComp
		{
			get
			{
				return this.stencilComp;
			}
			set
			{
				int propStencilComp = ShapesMaterialUtils.propStencilComp;
				this.stencilComp = value;
				this.SetIntOnAllInstancedMaterials(propStencilComp, (int)value);
			}
		}

		// Token: 0x1700008E RID: 142
		// (get) Token: 0x06000150 RID: 336 RVA: 0x00004C86 File Offset: 0x00002E86
		// (set) Token: 0x06000151 RID: 337 RVA: 0x00004C90 File Offset: 0x00002E90
		public StencilOp StencilOpPass
		{
			get
			{
				return this.stencilOpPass;
			}
			set
			{
				int propStencilOpPass = ShapesMaterialUtils.propStencilOpPass;
				this.stencilOpPass = value;
				this.SetIntOnAllInstancedMaterials(propStencilOpPass, (int)value);
			}
		}

		// Token: 0x1700008F RID: 143
		// (get) Token: 0x06000152 RID: 338 RVA: 0x00004CB2 File Offset: 0x00002EB2
		// (set) Token: 0x06000153 RID: 339 RVA: 0x00004CBC File Offset: 0x00002EBC
		public byte StencilRefID
		{
			get
			{
				return this.stencilRefID;
			}
			set
			{
				int propStencilID = ShapesMaterialUtils.propStencilID;
				this.stencilRefID = value;
				this.SetIntOnAllInstancedMaterials(propStencilID, (int)value);
			}
		}

		// Token: 0x17000090 RID: 144
		// (get) Token: 0x06000154 RID: 340 RVA: 0x00004CDE File Offset: 0x00002EDE
		// (set) Token: 0x06000155 RID: 341 RVA: 0x00004CE8 File Offset: 0x00002EE8
		public byte StencilReadMask
		{
			get
			{
				return this.stencilReadMask;
			}
			set
			{
				int propStencilReadMask = ShapesMaterialUtils.propStencilReadMask;
				this.stencilReadMask = value;
				this.SetIntOnAllInstancedMaterials(propStencilReadMask, (int)value);
			}
		}

		// Token: 0x17000091 RID: 145
		// (get) Token: 0x06000156 RID: 342 RVA: 0x00004D0A File Offset: 0x00002F0A
		// (set) Token: 0x06000157 RID: 343 RVA: 0x00004D14 File Offset: 0x00002F14
		public byte StencilWriteMask
		{
			get
			{
				return this.stencilWriteMask;
			}
			set
			{
				int propStencilWriteMask = ShapesMaterialUtils.propStencilWriteMask;
				this.stencilWriteMask = value;
				this.SetIntOnAllInstancedMaterials(propStencilWriteMask, (int)value);
			}
		}

		// Token: 0x06000158 RID: 344 RVA: 0x00004D38 File Offset: 0x00002F38
		private T MakeSureComponentExists<T>(ref T field, out bool created) where T : Component
		{
			if (field == null)
			{
				field = base.GetComponent<T>();
				if (field == null)
				{
					field = base.gameObject.AddComponent<T>();
					created = true;
				}
				field.hideFlags = HideFlags.HideInInspector;
			}
			created = false;
			return field;
		}

		// Token: 0x06000159 RID: 345 RVA: 0x00004DA4 File Offset: 0x00002FA4
		private void VerifyComponents()
		{
			if (!this.initializedComponents)
			{
				this.initializedComponents = true;
				bool flag;
				this.MakeSureComponentExists<MeshFilter>(ref this.mf, out flag);
				bool flag2;
				this.MakeSureComponentExists<MeshRenderer>(ref this.rnd, out flag2);
				if (flag2)
				{
					this.rnd.receiveShadows = false;
					this.rnd.shadowCastingMode = ShadowCastingMode.Off;
					this.rnd.lightProbeUsage = LightProbeUsage.Off;
					this.rnd.reflectionProbeUsage = ReflectionProbeUsage.Off;
				}
			}
		}

		// Token: 0x0600015A RID: 346 RVA: 0x00004E11 File Offset: 0x00003011
		public virtual void Awake()
		{
			this.VerifyComponents();
			this.UpdateMaterial();
			this.UpdateMesh(false);
			this.UpdateAllMaterialProperties();
		}

		// Token: 0x17000092 RID: 146
		// (get) Token: 0x0600015B RID: 347 RVA: 0x00004E2C File Offset: 0x0000302C
		private bool HasGeneratedOrCopyOfMesh
		{
			get
			{
				return this.MeshUpdateMode == MeshUpdateMode.SelfGenerated || this.MeshUpdateMode == MeshUpdateMode.UseAssetCopy;
			}
		}

		// Token: 0x0600015C RID: 348 RVA: 0x00004E42 File Offset: 0x00003042
		public virtual void OnEnable()
		{
			this.UpdateMesh(false);
			this.rnd.enabled = true;
			if (this.UseCamOnPreCull)
			{
				this.SubscribeCamPreCull();
			}
		}

		// Token: 0x0600015D RID: 349 RVA: 0x00004E65 File Offset: 0x00003065
		private void OnDisable()
		{
			if (this.rnd != null)
			{
				this.rnd.enabled = false;
			}
			if (this.UseCamOnPreCull)
			{
				this.UnsubscribeCamPreCull();
			}
		}

		// Token: 0x0600015E RID: 350 RVA: 0x00004E8F File Offset: 0x0000308F
		private void OnPreCamCullWithCam(Camera cam)
		{
			this.CamOnPreCull();
		}

		// Token: 0x0600015F RID: 351 RVA: 0x00004E97 File Offset: 0x00003097
		private void OnPreCamCullWithCam(ScriptableRenderContext ctx, Camera cam)
		{
			this.CamOnPreCull();
		}

		// Token: 0x06000160 RID: 352 RVA: 0x00004E9F File Offset: 0x0000309F
		private void SubscribeCamPreCull()
		{
			if (UnityInfo.UsingSRP)
			{
				RenderPipelineManager.beginCameraRendering += this.OnPreCamCullWithCam;
				return;
			}
			Camera.onPreCull = (Camera.CameraCallback)Delegate.Combine(Camera.onPreCull, new Camera.CameraCallback(this.OnPreCamCullWithCam));
		}

		// Token: 0x06000161 RID: 353 RVA: 0x00004EDA File Offset: 0x000030DA
		private void UnsubscribeCamPreCull()
		{
			if (UnityInfo.UsingSRP)
			{
				RenderPipelineManager.beginCameraRendering -= this.OnPreCamCullWithCam;
				return;
			}
			Camera.onPreCull = (Camera.CameraCallback)Delegate.Remove(Camera.onPreCull, new Camera.CameraCallback(this.OnPreCamCullWithCam));
		}

		// Token: 0x06000162 RID: 354 RVA: 0x00004F15 File Offset: 0x00003115
		private void Reset()
		{
			this.UpdateAllMaterialProperties();
			this.UpdateMesh(true);
		}

		// Token: 0x06000163 RID: 355 RVA: 0x00004F24 File Offset: 0x00003124
		private void OnDestroy()
		{
			if (this.HasGeneratedOrCopyOfMesh && this.Mesh != null)
			{
				Object.DestroyImmediate(this.Mesh);
			}
			this.TryDestroyInOnDestroy(this.rnd);
			this.TryDestroyInOnDestroy(this.mf);
			this.TryDestroyInstancedMaterials(true);
		}

		// Token: 0x06000164 RID: 356
		private protected abstract Bounds GetBounds_Internal();

		// Token: 0x06000165 RID: 357
		private protected abstract void SetAllMaterialProperties();

		// Token: 0x06000166 RID: 358 RVA: 0x00004F71 File Offset: 0x00003171
		private protected virtual void ShapeClampRanges()
		{
		}

		// Token: 0x06000167 RID: 359
		private protected abstract Material[] GetMaterials();

		// Token: 0x06000168 RID: 360 RVA: 0x00004F73 File Offset: 0x00003173
		private protected virtual void GenerateMesh()
		{
		}

		// Token: 0x06000169 RID: 361 RVA: 0x00004F75 File Offset: 0x00003175
		private protected virtual Mesh GetInitialMeshAsset()
		{
			return ShapesMeshUtils.QuadMesh[this.HasDetailLevels ? 2 : 0];
		}

		// Token: 0x17000093 RID: 147
		// (get) Token: 0x0600016A RID: 362 RVA: 0x00004F89 File Offset: 0x00003189
		private protected virtual MeshUpdateMode MeshUpdateMode
		{
			get
			{
				return MeshUpdateMode.UseAsset;
			}
		}

		// Token: 0x17000094 RID: 148
		// (get) Token: 0x0600016B RID: 363 RVA: 0x00004F8C File Offset: 0x0000318C
		internal virtual bool HasScaleModes
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000095 RID: 149
		// (get) Token: 0x0600016C RID: 364 RVA: 0x00004F8F File Offset: 0x0000318F
		internal virtual bool HasDetailLevels
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000096 RID: 150
		// (get) Token: 0x0600016D RID: 365 RVA: 0x00004F92 File Offset: 0x00003192
		private protected virtual bool UseCamOnPreCull
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600016E RID: 366 RVA: 0x00004F95 File Offset: 0x00003195
		internal virtual void CamOnPreCull()
		{
		}

		// Token: 0x0600016F RID: 367 RVA: 0x00004F97 File Offset: 0x00003197
		private void UpdateMeshBounds()
		{
			this.Mesh.bounds = this.GetBounds_Internal();
		}

		// Token: 0x06000170 RID: 368 RVA: 0x00004FAC File Offset: 0x000031AC
		private void TryDestroyInstancedMaterials(bool inOnDestroy = false)
		{
			if (this.instancedMaterials != null)
			{
				for (int i = 0; i < this.instancedMaterials.Length; i++)
				{
					if (this.instancedMaterials[i] != null)
					{
						if (inOnDestroy)
						{
							this.TryDestroyInOnDestroy(this.instancedMaterials[i]);
						}
						else
						{
							this.instancedMaterials[i].DestroyBranched();
						}
					}
				}
			}
		}

		// Token: 0x06000171 RID: 369 RVA: 0x00005004 File Offset: 0x00003204
		private void MakeSureMaterialInstancesAreGood(Material[] sourceMats)
		{
			ShapeRenderer.<>c__DisplayClass113_0 CS$<>8__locals1;
			CS$<>8__locals1.sourceMats = sourceMats;
			CS$<>8__locals1.<>4__this = this;
			if (this.instancedMaterials == null)
			{
				this.<MakeSureMaterialInstancesAreGood>g__PopulateAll|113_1(ref CS$<>8__locals1);
				return;
			}
			if (this.instancedMaterials.Length != CS$<>8__locals1.sourceMats.Length)
			{
				this.TryDestroyInstancedMaterials(false);
				this.<MakeSureMaterialInstancesAreGood>g__PopulateAll|113_1(ref CS$<>8__locals1);
				return;
			}
			for (int i = 0; i < CS$<>8__locals1.sourceMats.Length; i++)
			{
				if (this.instancedMaterials[i] == null)
				{
					this.instancedMaterials[i] = this.<MakeSureMaterialInstancesAreGood>g__InstantiateMaterial|113_0(i, ref CS$<>8__locals1);
				}
				else if (this.instancedMaterials[i].shader != CS$<>8__locals1.sourceMats[i].shader)
				{
					this.instancedMaterials[i].DestroyBranched();
					this.instancedMaterials[i] = this.<MakeSureMaterialInstancesAreGood>g__InstantiateMaterial|113_0(i, ref CS$<>8__locals1);
				}
				else
				{
					this.instancedMaterials[i].shaderKeywords = CS$<>8__locals1.sourceMats[i].shaderKeywords;
				}
			}
		}

		// Token: 0x06000172 RID: 370 RVA: 0x000050EC File Offset: 0x000032EC
		private protected void UpdateMaterial()
		{
			Material[] materials = this.GetMaterials();
			if (this.IsUsingUniqueMaterials)
			{
				this.MakeSureMaterialInstancesAreGood(materials);
				materials = this.instancedMaterials;
			}
			this.VerifyComponents();
			this.rnd.sharedMaterials = materials;
		}

		// Token: 0x06000173 RID: 371 RVA: 0x00005128 File Offset: 0x00003328
		public void UpdateMesh(bool force = false)
		{
			MeshUpdateMode meshUpdateMode = this.MeshUpdateMode;
			if (meshUpdateMode == MeshUpdateMode.UseAsset && (this.Mesh == null || this.Mesh != this.GetInitialMeshAsset()))
			{
				this.Mesh = this.GetInitialMeshAsset();
				return;
			}
			int instanceID = base.gameObject.GetInstanceID();
			if (this.Mesh == null || this.meshOwnerID != instanceID)
			{
				this.meshOwnerID = instanceID;
				if (meshUpdateMode == MeshUpdateMode.UseAssetCopy)
				{
					this.Mesh = Object.Instantiate<Mesh>(this.GetInitialMeshAsset());
					this.Mesh.hideFlags = HideFlags.HideAndDontSave;
					this.Mesh.MarkDynamic();
					return;
				}
				if (meshUpdateMode == MeshUpdateMode.SelfGenerated)
				{
					this.Mesh = new Mesh
					{
						hideFlags = HideFlags.HideAndDontSave
					};
					this.Mesh.MarkDynamic();
					this.GenerateMesh();
					return;
				}
			}
			else if (force && meshUpdateMode == MeshUpdateMode.SelfGenerated)
			{
				this.GenerateMesh();
			}
		}

		// Token: 0x06000174 RID: 372 RVA: 0x00005203 File Offset: 0x00003403
		public Bounds GetBounds()
		{
			return this.GetBounds_Internal();
		}

		// Token: 0x06000175 RID: 373 RVA: 0x0000520C File Offset: 0x0000340C
		public Bounds GetWorldBounds()
		{
			Bounds bounds_Internal = this.GetBounds_Internal();
			Vector3 vector = Vector3.one * float.MaxValue;
			Vector3 vector2 = Vector3.one * float.MinValue;
			Transform transform = base.transform;
			for (int i = -1; i <= 1; i += 2)
			{
				for (int j = -1; j <= 1; j += 2)
				{
					for (int k = -1; k <= 1; k += 2)
					{
						Vector3 vector3 = transform.TransformPoint(bounds_Internal.center + Vector3.Scale(bounds_Internal.extents, new Vector3((float)i, (float)j, (float)k)));
						vector = Vector3.Min(vector, vector3);
						vector2 = Vector3.Max(vector2, vector3);
					}
				}
			}
			return new Bounds((vector2 + vector) / 2f, vector2 - vector);
		}

		// Token: 0x06000176 RID: 374 RVA: 0x000052D4 File Offset: 0x000034D4
		private void OnDidApplyAnimationProperties()
		{
			this.UpdateAllMaterialProperties();
		}

		// Token: 0x06000177 RID: 375 RVA: 0x000052DC File Offset: 0x000034DC
		private void SetIntOnAllInstancedMaterials(int property, int value)
		{
			if (this.IsUsingUniqueMaterials)
			{
				this.UpdateMaterial();
				Material[] array = this.instancedMaterials;
				for (int i = 0; i < array.Length; i++)
				{
					array[i].SetInt(property, value);
				}
			}
		}

		// Token: 0x06000178 RID: 376 RVA: 0x00005318 File Offset: 0x00003518
		private void SetFloatOnAllInstancedMaterials(int property, float value)
		{
			if (this.IsUsingUniqueMaterials)
			{
				this.UpdateMaterial();
				Material[] array = this.instancedMaterials;
				for (int i = 0; i < array.Length; i++)
				{
					array[i].SetFloat(property, value);
				}
			}
		}

		// Token: 0x06000179 RID: 377 RVA: 0x00005354 File Offset: 0x00003554
		internal void UpdateAllMaterialProperties()
		{
			if (!base.gameObject.scene.IsValid())
			{
				return;
			}
			this.UpdateMaterial();
			if (this.IsUsingUniqueMaterials)
			{
				foreach (Material material in this.instancedMaterials)
				{
					material.SetInt(ShapesMaterialUtils.propZTest, (int)this.zTest);
					material.SetFloat(ShapesMaterialUtils.propZOffsetFactor, this.zOffsetFactor);
					material.SetInt(ShapesMaterialUtils.propZOffsetUnits, this.zOffsetUnits);
					material.SetInt(ShapesMaterialUtils.propStencilComp, (int)this.stencilComp);
					material.SetInt(ShapesMaterialUtils.propStencilOpPass, (int)this.stencilOpPass);
					material.SetInt(ShapesMaterialUtils.propStencilID, (int)this.stencilRefID);
					material.SetInt(ShapesMaterialUtils.propStencilReadMask, (int)this.stencilReadMask);
					material.SetInt(ShapesMaterialUtils.propStencilWriteMask, (int)this.stencilWriteMask);
				}
			}
			this.SetColor(ShapesMaterialUtils.propColor, this.color);
			if (this.HasScaleModes)
			{
				this.SetInt(ShapesMaterialUtils.propScaleMode, (int)this.scaleMode);
			}
			this.SetAllMaterialProperties();
			this.ApplyProperties();
		}

		// Token: 0x0600017A RID: 378 RVA: 0x00005463 File Offset: 0x00003663
		private protected void ApplyProperties()
		{
			this.VerifyComponents();
			this.rnd.SetPropertyBlock(this.Mpb);
			if (this.MeshUpdateMode == MeshUpdateMode.UseAssetCopy)
			{
				this.UpdateMeshBounds();
			}
		}

		// Token: 0x0600017B RID: 379 RVA: 0x0000548C File Offset: 0x0000368C
		private protected void SetAllDashValues(DashStyle style, bool dashed, bool matchSpacingToSize, float thickness, bool setType, bool now)
		{
			float netAbsoluteSize = style.GetNetAbsoluteSize(dashed, thickness);
			if (dashed)
			{
				this.SetFloat(ShapesMaterialUtils.propDashSpacing, this.GetNetDashSpacing(style, true, matchSpacingToSize, thickness));
				this.SetFloat(ShapesMaterialUtils.propDashOffset, style.offset);
				this.SetInt(ShapesMaterialUtils.propDashSpace, (int)style.space);
				this.SetInt(ShapesMaterialUtils.propDashSnap, (int)style.snap);
				if (setType)
				{
					this.SetInt(ShapesMaterialUtils.propDashType, (int)style.type);
					if (style.type.HasModifier())
					{
						this.SetFloat(ShapesMaterialUtils.propDashShapeModifier, style.shapeModifier);
					}
				}
			}
			if (now)
			{
				this.SetFloatNow(ShapesMaterialUtils.propDashSize, netAbsoluteSize);
				return;
			}
			this.SetFloat(ShapesMaterialUtils.propDashSize, netAbsoluteSize);
		}

		// Token: 0x0600017C RID: 380 RVA: 0x0000553F File Offset: 0x0000373F
		private protected float GetNetDashSpacing(DashStyle style, bool dashed, bool matchSpacingToSize, float thickness)
		{
			if (matchSpacingToSize && style.space == DashSpace.FixedCount)
			{
				return 0.5f;
			}
			if (!matchSpacingToSize)
			{
				return style.GetNetAbsoluteSpacing(dashed, thickness);
			}
			return style.GetNetAbsoluteSize(dashed, thickness);
		}

		// Token: 0x0600017D RID: 381 RVA: 0x0000556C File Offset: 0x0000376C
		private protected void SetColor(int prop, Color value)
		{
			if (ShapeGroup.shapeGroupsInScene > 0)
			{
				ShapeGroup[] componentsInParent = base.GetComponentsInParent<ShapeGroup>();
				if (componentsInParent != null)
				{
					foreach (ShapeGroup shapeGroup in componentsInParent.Where((ShapeGroup g) => g.IsEnabled))
					{
						value *= shapeGroup.Color;
					}
				}
			}
			this.Mpb.SetColor(prop, value);
		}

		// Token: 0x0600017E RID: 382 RVA: 0x00005600 File Offset: 0x00003800
		private protected void SetFloat(int prop, float value)
		{
			this.Mpb.SetFloat(prop, value);
		}

		// Token: 0x0600017F RID: 383 RVA: 0x0000560F File Offset: 0x0000380F
		private protected void SetInt(int prop, int value)
		{
			this.Mpb.SetInt(prop, value);
		}

		// Token: 0x06000180 RID: 384 RVA: 0x0000561E File Offset: 0x0000381E
		private protected void SetVector3(int prop, Vector3 value)
		{
			this.Mpb.SetVector(prop, value);
		}

		// Token: 0x06000181 RID: 385 RVA: 0x00005632 File Offset: 0x00003832
		private protected void SetVector4(int prop, Vector4 value)
		{
			this.Mpb.SetVector(prop, value);
		}

		// Token: 0x06000182 RID: 386 RVA: 0x00005641 File Offset: 0x00003841
		private protected void SetColorNow(int prop, Color value)
		{
			this.SetColor(prop, value);
			this.ApplyProperties();
		}

		// Token: 0x06000183 RID: 387 RVA: 0x00005651 File Offset: 0x00003851
		private protected void SetFloatNow(int prop, float value)
		{
			this.Mpb.SetFloat(prop, value);
			this.ApplyProperties();
		}

		// Token: 0x06000184 RID: 388 RVA: 0x00005666 File Offset: 0x00003866
		private protected void SetIntNow(int prop, int value)
		{
			this.Mpb.SetInt(prop, value);
			this.ApplyProperties();
		}

		// Token: 0x06000185 RID: 389 RVA: 0x0000567B File Offset: 0x0000387B
		private protected void SetVector3Now(int prop, Vector3 value)
		{
			this.Mpb.SetVector(prop, value);
			this.ApplyProperties();
		}

		// Token: 0x06000186 RID: 390 RVA: 0x00005695 File Offset: 0x00003895
		private protected void SetVector4Now(int prop, Vector4 value)
		{
			this.Mpb.SetVector(prop, value);
			this.ApplyProperties();
		}

		// Token: 0x06000188 RID: 392 RVA: 0x00005703 File Offset: 0x00003903
		[CompilerGenerated]
		private Material <MakeSureMaterialInstancesAreGood>g__InstantiateMaterial|113_0(int index, ref ShapeRenderer.<>c__DisplayClass113_0 A_2)
		{
			return new Material(A_2.sourceMats[index])
			{
				name = A_2.sourceMats[index].name + " (instance)"
			};
		}

		// Token: 0x06000189 RID: 393 RVA: 0x00005730 File Offset: 0x00003930
		[CompilerGenerated]
		private void <MakeSureMaterialInstancesAreGood>g__PopulateAll|113_1(ref ShapeRenderer.<>c__DisplayClass113_0 A_1)
		{
			this.instancedMaterials = new Material[A_1.sourceMats.Length];
			for (int i = 0; i < A_1.sourceMats.Length; i++)
			{
				this.instancedMaterials[i] = this.<MakeSureMaterialInstancesAreGood>g__InstantiateMaterial|113_0(i, ref A_1);
			}
		}

		// Token: 0x04000049 RID: 73
		private bool initializedComponents;

		// Token: 0x0400004A RID: 74
		private MeshRenderer rnd;

		// Token: 0x0400004B RID: 75
		private MeshFilter mf;

		// Token: 0x0400004C RID: 76
		private int meshOwnerID;

		// Token: 0x0400004D RID: 77
		private MaterialPropertyBlock mpb;

		// Token: 0x0400004E RID: 78
		private Material[] instancedMaterials;

		// Token: 0x0400004F RID: 79
		[NonSerialized]
		public bool meshOutOfDate = true;

		// Token: 0x04000050 RID: 80
		[SerializeField]
		private ShapesBlendMode blendMode = ShapesBlendMode.Transparent;

		// Token: 0x04000051 RID: 81
		[SerializeField]
		private ScaleMode scaleMode;

		// Token: 0x04000052 RID: 82
		[SerializeField]
		[ShapesColorField(true)]
		private protected Color color = Color.white;

		// Token: 0x04000053 RID: 83
		[SerializeField]
		private protected DetailLevel detailLevel = DetailLevel.Medium;

		// Token: 0x04000054 RID: 84
		public const CompareFunction DEFAULT_ZTEST = CompareFunction.LessEqual;

		// Token: 0x04000055 RID: 85
		public const float DEFAULT_ZOFS_FACTOR = 0f;

		// Token: 0x04000056 RID: 86
		public const int DEFAULT_ZOFS_UNITS = 0;

		// Token: 0x04000057 RID: 87
		[SerializeField]
		private CompareFunction zTest = CompareFunction.LessEqual;

		// Token: 0x04000058 RID: 88
		[SerializeField]
		private float zOffsetFactor;

		// Token: 0x04000059 RID: 89
		[SerializeField]
		private int zOffsetUnits;

		// Token: 0x0400005A RID: 90
		public const CompareFunction DEFAULT_STENCIL_COMP = CompareFunction.Always;

		// Token: 0x0400005B RID: 91
		public const StencilOp DEFAULT_STENCIL_OP = StencilOp.Keep;

		// Token: 0x0400005C RID: 92
		public const byte DEFAULT_STENCIL_REF_ID = 0;

		// Token: 0x0400005D RID: 93
		public const byte DEFAULT_STENCIL_MASK = 255;

		// Token: 0x0400005E RID: 94
		[SerializeField]
		private CompareFunction stencilComp = CompareFunction.Always;

		// Token: 0x0400005F RID: 95
		[SerializeField]
		private StencilOp stencilOpPass;

		// Token: 0x04000060 RID: 96
		[SerializeField]
		private byte stencilRefID;

		// Token: 0x04000061 RID: 97
		[SerializeField]
		private byte stencilReadMask = byte.MaxValue;

		// Token: 0x04000062 RID: 98
		[SerializeField]
		private byte stencilWriteMask = byte.MaxValue;
	}
}
