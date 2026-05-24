using System;
using System.Collections.Generic;

namespace UnityEngine.Rendering.PostProcessing
{
	// Token: 0x02000059 RID: 89
	[ExecuteAlways]
	[AddComponentMenu("Rendering/Post-process Volume", 1001)]
	public sealed class PostProcessVolume : MonoBehaviour
	{
		// Token: 0x17000024 RID: 36
		// (get) Token: 0x06000199 RID: 409 RVA: 0x0000D86C File Offset: 0x0000BA6C
		// (set) Token: 0x0600019A RID: 410 RVA: 0x0000D900 File Offset: 0x0000BB00
		public PostProcessProfile profile
		{
			get
			{
				if (this.m_InternalProfile == null)
				{
					this.m_InternalProfile = ScriptableObject.CreateInstance<PostProcessProfile>();
					if (this.sharedProfile != null)
					{
						foreach (PostProcessEffectSettings postProcessEffectSettings in this.sharedProfile.settings)
						{
							PostProcessEffectSettings postProcessEffectSettings2 = Object.Instantiate<PostProcessEffectSettings>(postProcessEffectSettings);
							this.m_InternalProfile.settings.Add(postProcessEffectSettings2);
						}
					}
				}
				return this.m_InternalProfile;
			}
			set
			{
				this.m_InternalProfile = value;
			}
		}

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x0600019B RID: 411 RVA: 0x0000D909 File Offset: 0x0000BB09
		internal PostProcessProfile profileRef
		{
			get
			{
				if (!(this.m_InternalProfile == null))
				{
					return this.m_InternalProfile;
				}
				return this.sharedProfile;
			}
		}

		// Token: 0x0600019C RID: 412 RVA: 0x0000D926 File Offset: 0x0000BB26
		public bool HasInstantiatedProfile()
		{
			return this.m_InternalProfile != null;
		}

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x0600019D RID: 413 RVA: 0x0000D934 File Offset: 0x0000BB34
		internal int previousLayer
		{
			get
			{
				return this.m_PreviousLayer;
			}
		}

		// Token: 0x0600019E RID: 414 RVA: 0x0000D93C File Offset: 0x0000BB3C
		private void OnEnable()
		{
			PostProcessManager.instance.Register(this);
			this.m_PreviousLayer = base.gameObject.layer;
			this.m_TempColliders = new List<Collider>();
		}

		// Token: 0x0600019F RID: 415 RVA: 0x0000D965 File Offset: 0x0000BB65
		private void OnDisable()
		{
			PostProcessManager.instance.Unregister(this);
		}

		// Token: 0x060001A0 RID: 416 RVA: 0x0000D974 File Offset: 0x0000BB74
		private void Update()
		{
			int layer = base.gameObject.layer;
			if (layer != this.m_PreviousLayer)
			{
				PostProcessManager.instance.UpdateVolumeLayer(this, this.m_PreviousLayer, layer);
				this.m_PreviousLayer = layer;
			}
			if (this.priority != this.m_PreviousPriority)
			{
				PostProcessManager.instance.SetLayerDirty(layer);
				this.m_PreviousPriority = this.priority;
			}
		}

		// Token: 0x060001A1 RID: 417 RVA: 0x0000D9D4 File Offset: 0x0000BBD4
		private void OnDrawGizmos()
		{
			List<Collider> tempColliders = this.m_TempColliders;
			base.GetComponents<Collider>(tempColliders);
			if (this.isGlobal || tempColliders == null)
			{
				return;
			}
			Vector3 lossyScale = base.transform.lossyScale;
			Vector3 vector = new Vector3(1f / lossyScale.x, 1f / lossyScale.y, 1f / lossyScale.z);
			Gizmos.matrix = Matrix4x4.TRS(base.transform.position, base.transform.rotation, lossyScale);
			foreach (Collider collider in tempColliders)
			{
				if (collider.enabled)
				{
					Type type = collider.GetType();
					if (type == typeof(BoxCollider))
					{
						BoxCollider boxCollider = (BoxCollider)collider;
						Gizmos.DrawCube(boxCollider.center, boxCollider.size);
						Gizmos.DrawWireCube(boxCollider.center, boxCollider.size + vector * this.blendDistance * 4f);
					}
					else if (type == typeof(SphereCollider))
					{
						SphereCollider sphereCollider = (SphereCollider)collider;
						Gizmos.DrawSphere(sphereCollider.center, sphereCollider.radius);
						Gizmos.DrawWireSphere(sphereCollider.center, sphereCollider.radius + vector.x * this.blendDistance * 2f);
					}
					else if (type == typeof(MeshCollider))
					{
						MeshCollider meshCollider = (MeshCollider)collider;
						if (!meshCollider.convex)
						{
							meshCollider.convex = true;
						}
						Gizmos.DrawMesh(meshCollider.sharedMesh);
						Gizmos.DrawWireMesh(meshCollider.sharedMesh, Vector3.zero, Quaternion.identity, Vector3.one + vector * this.blendDistance * 4f);
					}
				}
			}
			tempColliders.Clear();
		}

		// Token: 0x04000184 RID: 388
		public PostProcessProfile sharedProfile;

		// Token: 0x04000185 RID: 389
		[Tooltip("Check this box to mark this volume as global. This volume's Profile will be applied to the whole Scene.")]
		public bool isGlobal;

		// Token: 0x04000186 RID: 390
		[Min(0f)]
		[Tooltip("The distance (from the attached Collider) to start blending from. A value of 0 means there will be no blending and the Volume overrides will be applied immediatly upon entry to the attached Collider.")]
		public float blendDistance;

		// Token: 0x04000187 RID: 391
		[Range(0f, 1f)]
		[Tooltip("The total weight of this Volume in the Scene. A value of 0 signifies that it will have no effect, 1 signifies full effect.")]
		public float weight = 1f;

		// Token: 0x04000188 RID: 392
		[Tooltip("The volume priority in the stack. A higher value means higher priority. Negative values are supported.")]
		public float priority;

		// Token: 0x04000189 RID: 393
		private int m_PreviousLayer;

		// Token: 0x0400018A RID: 394
		private float m_PreviousPriority;

		// Token: 0x0400018B RID: 395
		private List<Collider> m_TempColliders;

		// Token: 0x0400018C RID: 396
		private PostProcessProfile m_InternalProfile;
	}
}
