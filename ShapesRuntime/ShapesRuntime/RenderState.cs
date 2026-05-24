using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

namespace Shapes
{
	// Token: 0x02000052 RID: 82
	internal struct RenderState : IEquatable<RenderState>
	{
		// Token: 0x06000A0F RID: 2575 RVA: 0x00023E54 File Offset: 0x00022054
		public RenderState(Material mat)
		{
			this.shader = mat.shader;
			this.keywords = mat.shaderKeywords;
			this.zTest = (CompareFunction)mat.GetInt(ShapesMaterialUtils.propZTest);
			this.zOffsetFactor = mat.GetFloat(ShapesMaterialUtils.propZOffsetFactor);
			this.zOffsetUnits = mat.GetInt(ShapesMaterialUtils.propZOffsetUnits);
			this.stencilComp = (CompareFunction)mat.GetInt(ShapesMaterialUtils.propStencilComp);
			this.stencilOpPass = (StencilOp)mat.GetInt(ShapesMaterialUtils.propStencilOpPass);
			this.stencilRefID = (byte)mat.GetInt(ShapesMaterialUtils.propStencilID);
			this.stencilReadMask = (byte)mat.GetInt(ShapesMaterialUtils.propStencilReadMask);
			this.stencilWriteMask = (byte)mat.GetInt(ShapesMaterialUtils.propStencilWriteMask);
		}

		// Token: 0x06000A10 RID: 2576 RVA: 0x00023F04 File Offset: 0x00022104
		public Material CreateMaterial()
		{
			Material material = new Material(this.shader);
			material.shaderKeywords = this.keywords;
			material.SetInt(ShapesMaterialUtils.propZTest, (int)this.zTest);
			material.SetFloat(ShapesMaterialUtils.propZOffsetFactor, this.zOffsetFactor);
			material.SetInt(ShapesMaterialUtils.propZOffsetUnits, this.zOffsetUnits);
			material.SetInt(ShapesMaterialUtils.propStencilComp, (int)this.stencilComp);
			material.SetInt(ShapesMaterialUtils.propStencilOpPass, (int)this.stencilOpPass);
			material.SetInt(ShapesMaterialUtils.propStencilID, (int)this.stencilRefID);
			material.SetInt(ShapesMaterialUtils.propStencilReadMask, (int)this.stencilReadMask);
			material.SetInt(ShapesMaterialUtils.propStencilWriteMask, (int)this.stencilWriteMask);
			material.enableInstancing = true;
			Object.DontDestroyOnLoad(material);
			return material;
		}

		// Token: 0x06000A11 RID: 2577 RVA: 0x00023FBD File Offset: 0x000221BD
		private static bool StrArrEquals(string[] a, string[] b)
		{
			if (a == null || b == null)
			{
				return a == b;
			}
			return a.Length == b.Length && a.SequenceEqual(b);
		}

		// Token: 0x06000A12 RID: 2578 RVA: 0x00023FDC File Offset: 0x000221DC
		public bool Equals(RenderState other)
		{
			return object.Equals(this.shader, other.shader) && RenderState.StrArrEquals(this.keywords, other.keywords) && this.zTest == other.zTest && this.zOffsetFactor.Equals(other.zOffsetFactor) && this.zOffsetUnits == other.zOffsetUnits && this.stencilComp == other.stencilComp && this.stencilOpPass == other.stencilOpPass && this.stencilRefID == other.stencilRefID && this.stencilReadMask == other.stencilReadMask && this.stencilWriteMask == other.stencilWriteMask;
		}

		// Token: 0x06000A13 RID: 2579 RVA: 0x0002408C File Offset: 0x0002228C
		public override bool Equals(object obj)
		{
			if (obj is RenderState)
			{
				RenderState renderState = (RenderState)obj;
				return this.Equals(renderState);
			}
			return false;
		}

		// Token: 0x06000A14 RID: 2580 RVA: 0x000240B4 File Offset: 0x000222B4
		public override int GetHashCode()
		{
			int num = ((this.shader != null) ? this.shader.GetHashCode() : 0);
			if (this.keywords != null)
			{
				foreach (string text in this.keywords)
				{
					num = (num * 397) ^ ((text != null) ? text.GetHashCode() : 0);
				}
			}
			num = (num * 397) ^ (int)this.zTest;
			num = (num * 397) ^ this.zOffsetFactor.GetHashCode();
			num = (num * 397) ^ this.zOffsetUnits;
			num = (num * 397) ^ (int)this.stencilComp;
			num = (num * 397) ^ (int)this.stencilOpPass;
			num = (num * 397) ^ this.stencilRefID.GetHashCode();
			num = (num * 397) ^ this.stencilReadMask.GetHashCode();
			return (num * 397) ^ this.stencilWriteMask.GetHashCode();
		}

		// Token: 0x0400019B RID: 411
		public Shader shader;

		// Token: 0x0400019C RID: 412
		public string[] keywords;

		// Token: 0x0400019D RID: 413
		public CompareFunction zTest;

		// Token: 0x0400019E RID: 414
		public float zOffsetFactor;

		// Token: 0x0400019F RID: 415
		public int zOffsetUnits;

		// Token: 0x040001A0 RID: 416
		public CompareFunction stencilComp;

		// Token: 0x040001A1 RID: 417
		public StencilOp stencilOpPass;

		// Token: 0x040001A2 RID: 418
		public byte stencilRefID;

		// Token: 0x040001A3 RID: 419
		public byte stencilReadMask;

		// Token: 0x040001A4 RID: 420
		public byte stencilWriteMask;
	}
}
