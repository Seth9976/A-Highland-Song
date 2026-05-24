using System;
using System.Collections.Generic;
using UnityEngine;

namespace Shapes
{
	// Token: 0x0200001C RID: 28
	internal abstract class MetaMpb : IDisposable
	{
		// Token: 0x17000100 RID: 256
		// (get) Token: 0x0600093E RID: 2366 RVA: 0x00021C24 File Offset: 0x0001FE24
		public bool HasContent
		{
			get
			{
				return this.initialized;
			}
		}

		// Token: 0x17000101 RID: 257
		// (get) Token: 0x0600093F RID: 2367 RVA: 0x00021C2C File Offset: 0x0001FE2C
		private bool HasMultipleInstances
		{
			get
			{
				return this.instanceCount > 1;
			}
		}

		// Token: 0x06000940 RID: 2368 RVA: 0x00021C38 File Offset: 0x0001FE38
		internal static void ApplyColorOrFill<T>(T fillable, ShapeFill fill, Color baseColor) where T : MetaMpb, IFillable
		{
			bool flag = fill != null;
			fillable.color.Add((flag ? fill.colorStart : baseColor).ColorSpaceAdjusted());
			fillable.fillType.Add((float)fill.GetShaderFillModeInt());
			fillable.fillSpace.Add(flag ? ((float)fill.space) : 0f);
			fillable.fillStart.Add(flag ? fill.GetShaderStartVector() : default(Vector4));
			fillable.fillColorEnd.Add(flag ? fill.colorEnd.ColorSpaceAdjusted() : default(Color));
			fillable.fillEnd.Add(flag ? fill.linearEnd : default(Vector3));
		}

		// Token: 0x06000941 RID: 2369 RVA: 0x00021D24 File Offset: 0x0001FF24
		internal static void ApplyDashSettings<T>(T dashable, DashStyle style, float thickness) where T : MetaMpb, IDashable
		{
			bool flag = style != null && style.size > 0f;
			dashable.dashSize.Add(flag ? style.GetNetAbsoluteSize(true, thickness) : 0f);
			dashable.dashType.Add(flag ? ((float)style.type) : 0f);
			dashable.dashShapeModifier.Add(flag ? style.shapeModifier : 0f);
			dashable.dashSpace.Add(flag ? ((float)style.space) : 0f);
			dashable.dashSnap.Add((float)(flag ? style.snap : DashSnapping.Off));
			dashable.dashOffset.Add(flag ? style.offset : 0f);
			dashable.dashSpacing.Add(flag ? style.GetNetAbsoluteSpacing(true, thickness) : 0f);
		}

		// Token: 0x06000942 RID: 2370 RVA: 0x00021E28 File Offset: 0x00020028
		internal static List<T> InitList<T>()
		{
			return new List<T>(1023);
		}

		// Token: 0x06000943 RID: 2371
		protected abstract void TransferShapeProperties();

		// Token: 0x06000944 RID: 2372 RVA: 0x00021E34 File Offset: 0x00020034
		protected void Transfer(int propertyID, List<Vector4> listVec)
		{
			if (this.directMaterialApply)
			{
				this.drawState.mat.SetVector(propertyID, listVec[0]);
			}
			else if (this.HasMultipleInstances)
			{
				this.sdc.mpb.SetVectorArray(propertyID, listVec);
			}
			else
			{
				this.sdc.mpb.SetVector(propertyID, listVec[0]);
			}
			listVec.Clear();
		}

		// Token: 0x06000945 RID: 2373 RVA: 0x00021EA0 File Offset: 0x000200A0
		protected void Transfer(int propertyID, List<float> listFloat)
		{
			if (this.directMaterialApply)
			{
				this.drawState.mat.SetFloat(propertyID, listFloat[0]);
			}
			else if (this.HasMultipleInstances)
			{
				this.sdc.mpb.SetFloatArray(propertyID, listFloat);
			}
			else
			{
				this.sdc.mpb.SetFloat(propertyID, listFloat[0]);
			}
			listFloat.Clear();
		}

		// Token: 0x06000946 RID: 2374 RVA: 0x00021F0C File Offset: 0x0002010C
		public bool PreAppendCheck(ShapeDrawState additionDrawState, Matrix4x4 mtx)
		{
			bool flag = false;
			if (!this.initialized)
			{
				this.initialized = true;
				this.drawState = additionDrawState;
				flag = true;
			}
			else if (this.instanceCount < 1023 && this.drawState.CompatibleWith(additionDrawState))
			{
				flag = true;
			}
			if (flag)
			{
				Matrix4x4[] array = this.matrices;
				int num = this.instanceCount;
				this.instanceCount = num + 1;
				array[num] = mtx;
			}
			return flag;
		}

		// Token: 0x06000947 RID: 2375 RVA: 0x00021F74 File Offset: 0x00020174
		public ShapeDrawCall ExtractDrawCall()
		{
			if (this.HasMultipleInstances)
			{
				this.sdc = new ShapeDrawCall(this.drawState, this.instanceCount, this.matrices);
				this.matrices = ArrayPool<Matrix4x4>.Alloc(1023);
			}
			else
			{
				this.sdc = new ShapeDrawCall(this.drawState, this.matrices[0]);
			}
			this.TransferAllProperties();
			this.Dispose();
			return this.sdc;
		}

		// Token: 0x06000948 RID: 2376 RVA: 0x00021FE7 File Offset: 0x000201E7
		public void ApplyDirectlyToMaterial()
		{
			this.directMaterialApply = true;
			this.TransferAllProperties();
			this.directMaterialApply = false;
			this.Dispose();
		}

		// Token: 0x06000949 RID: 2377 RVA: 0x00022004 File Offset: 0x00020204
		internal void TransferAllProperties()
		{
			if (!(this is MpbText))
			{
				this.Transfer(ShapesMaterialUtils.propColor, this.color);
			}
			IFillable fillable = this as IFillable;
			if (fillable != null)
			{
				this.Transfer(ShapesMaterialUtils.propFillType, fillable.fillType);
				this.Transfer(ShapesMaterialUtils.propFillSpace, fillable.fillSpace);
				this.Transfer(ShapesMaterialUtils.propFillStart, fillable.fillStart);
				this.Transfer(ShapesMaterialUtils.propColorEnd, fillable.fillColorEnd);
				this.Transfer(ShapesMaterialUtils.propFillEnd, fillable.fillEnd);
			}
			IDashable dashable = this as IDashable;
			if (dashable != null)
			{
				this.Transfer(ShapesMaterialUtils.propDashSize, dashable.dashSize);
				this.Transfer(ShapesMaterialUtils.propDashType, dashable.dashType);
				this.Transfer(ShapesMaterialUtils.propDashShapeModifier, dashable.dashShapeModifier);
				this.Transfer(ShapesMaterialUtils.propDashSpace, dashable.dashSpace);
				this.Transfer(ShapesMaterialUtils.propDashSnap, dashable.dashSnap);
				this.Transfer(ShapesMaterialUtils.propDashOffset, dashable.dashOffset);
				this.Transfer(ShapesMaterialUtils.propDashSpacing, dashable.dashSpacing);
			}
			this.TransferShapeProperties();
		}

		// Token: 0x0600094A RID: 2378 RVA: 0x00022110 File Offset: 0x00020310
		public void Dispose()
		{
			this.initialized = false;
			this.drawState = default(ShapeDrawState);
			this.instanceCount = 0;
		}

		// Token: 0x040000CD RID: 205
		private bool initialized;

		// Token: 0x040000CE RID: 206
		private int instanceCount;

		// Token: 0x040000CF RID: 207
		private ShapeDrawState drawState;

		// Token: 0x040000D0 RID: 208
		private Matrix4x4[] matrices = ArrayPool<Matrix4x4>.Alloc(1023);

		// Token: 0x040000D1 RID: 209
		private bool directMaterialApply;

		// Token: 0x040000D2 RID: 210
		internal List<Vector4> color = MetaMpb.InitList<Vector4>();

		// Token: 0x040000D3 RID: 211
		private ShapeDrawCall sdc;
	}
}
