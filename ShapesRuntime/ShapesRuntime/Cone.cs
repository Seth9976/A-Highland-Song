using System;
using UnityEngine;

namespace Shapes
{
	// Token: 0x02000002 RID: 2
	[ExecuteAlways]
	[AddComponentMenu("Shapes/Cone")]
	public class Cone : ShapeRenderer
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		// (set) Token: 0x06000002 RID: 2 RVA: 0x00002058 File Offset: 0x00000258
		public float Radius
		{
			get
			{
				return this.radius;
			}
			set
			{
				base.SetFloatNow(ShapesMaterialUtils.propRadius, this.radius = Mathf.Max(0f, value));
			}
		}

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000003 RID: 3 RVA: 0x00002084 File Offset: 0x00000284
		// (set) Token: 0x06000004 RID: 4 RVA: 0x0000208C File Offset: 0x0000028C
		public float Length
		{
			get
			{
				return this.length;
			}
			set
			{
				base.SetFloatNow(ShapesMaterialUtils.propLength, this.length = Mathf.Max(0f, value));
			}
		}

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000005 RID: 5 RVA: 0x000020B8 File Offset: 0x000002B8
		// (set) Token: 0x06000006 RID: 6 RVA: 0x000020C0 File Offset: 0x000002C0
		[Obsolete("this property is obsolete I'm sorry! this was a typo, please use SizeSpace instead!", true)]
		public ThicknessSpace RadiusSpace
		{
			get
			{
				return this.SizeSpace;
			}
			set
			{
				this.SizeSpace = value;
			}
		}

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000007 RID: 7 RVA: 0x000020C9 File Offset: 0x000002C9
		// (set) Token: 0x06000008 RID: 8 RVA: 0x000020D4 File Offset: 0x000002D4
		public ThicknessSpace SizeSpace
		{
			get
			{
				return this.sizeSpace;
			}
			set
			{
				int propSizeSpace = ShapesMaterialUtils.propSizeSpace;
				this.sizeSpace = value;
				base.SetIntNow(propSizeSpace, (int)value);
			}
		}

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x06000009 RID: 9 RVA: 0x000020F6 File Offset: 0x000002F6
		// (set) Token: 0x0600000A RID: 10 RVA: 0x000020FE File Offset: 0x000002FE
		public bool FillCap
		{
			get
			{
				return this.fillCap;
			}
			set
			{
				this.fillCap = value;
				base.UpdateMesh(true);
			}
		}

		// Token: 0x0600000B RID: 11 RVA: 0x0000210E File Offset: 0x0000030E
		private protected override void SetAllMaterialProperties()
		{
			base.SetFloat(ShapesMaterialUtils.propRadius, this.radius);
			base.SetFloat(ShapesMaterialUtils.propLength, this.length);
			base.SetInt(ShapesMaterialUtils.propSizeSpace, (int)this.sizeSpace);
		}

		// Token: 0x0600000C RID: 12 RVA: 0x00002143 File Offset: 0x00000343
		private protected override void ShapeClampRanges()
		{
			this.radius = Mathf.Max(0f, this.radius);
			this.length = Mathf.Max(0f, this.length);
		}

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x0600000D RID: 13 RVA: 0x00002171 File Offset: 0x00000371
		internal override bool HasDetailLevels
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x0600000E RID: 14 RVA: 0x00002174 File Offset: 0x00000374
		internal override bool HasScaleModes
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600000F RID: 15 RVA: 0x00002177 File Offset: 0x00000377
		private protected override Material[] GetMaterials()
		{
			return new Material[] { ShapesMaterialUtils.matCone[base.BlendMode] };
		}

		// Token: 0x06000010 RID: 16 RVA: 0x00002192 File Offset: 0x00000392
		private protected override Mesh GetInitialMeshAsset()
		{
			if (!this.fillCap)
			{
				return ShapesMeshUtils.ConeMeshUncapped[(int)this.detailLevel];
			}
			return ShapesMeshUtils.ConeMesh[(int)this.detailLevel];
		}

		// Token: 0x06000011 RID: 17 RVA: 0x000021B8 File Offset: 0x000003B8
		private protected override Bounds GetBounds_Internal()
		{
			if (this.sizeSpace != ThicknessSpace.Meters)
			{
				return new Bounds(Vector3.zero, Vector3.one);
			}
			return new Bounds(Vector3.zero, new Vector3(this.radius * 2f, this.radius * 2f, this.length));
		}

		// Token: 0x04000001 RID: 1
		[SerializeField]
		private float radius = 1f;

		// Token: 0x04000002 RID: 2
		[SerializeField]
		private float length = 1.5f;

		// Token: 0x04000003 RID: 3
		[SerializeField]
		private ThicknessSpace sizeSpace;

		// Token: 0x04000004 RID: 4
		[SerializeField]
		private bool fillCap = true;
	}
}
