using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace Shapes
{
	// Token: 0x0200000C RID: 12
	[ExecuteAlways]
	public class ProceduralTree : ImmediateModeShapeDrawer
	{
		// Token: 0x0600004A RID: 74 RVA: 0x0000397C File Offset: 0x00001B7C
		public override void DrawShapes(Camera cam)
		{
			using (Draw.Command(cam, CameraEvent.BeforeImageEffects))
			{
				Draw.ResetAllDrawStates();
				Draw.BlendMode = ShapesBlendMode.Additive;
				Draw.LineThickness = this.lineThickness;
				Draw.LineGeometry = (this.use3D ? LineGeometry.Volumetric3D : LineGeometry.Flat2D);
				Draw.LineThicknessSpace = ThicknessSpace.Meters;
				Draw.Color = this.lineColor;
				Random.InitState(this.seed);
				this.currentLineCount = 0;
				this.BranchFrom(Draw.Matrix);
			}
		}

		// Token: 0x0600004B RID: 75 RVA: 0x00003A04 File Offset: 0x00001C04
		private void BranchFrom(Matrix4x4 mtx)
		{
			int num = this.currentLineCount;
			this.currentLineCount = num + 1;
			if (num >= this.lineCount)
			{
				return;
			}
			Draw.Matrix = mtx;
			float num2 = Mathf.Lerp(this.branchLengthMin, this.branchLengthMax, Random.value);
			Vector3 vector = new Vector3(0f, num2, 0f);
			Draw.Line(Vector3.zero, vector);
			Draw.Translate(vector);
			int num3 = Random.Range(this.branchesMin, this.branchesMax + 1);
			for (int i = 0; i < num3; i++)
			{
				using (Draw.MatrixScope)
				{
					float num4 = Mathf.Lerp(-this.maxAngDeviation, this.maxAngDeviation, ShapesMath.RandomGaussian(0f, 1f));
					if (this.use3D)
					{
						Draw.Rotate(num4, ShapesMath.GetRandomPerpendicularVector(Vector3.up));
					}
					else
					{
						Draw.Rotate(num4);
					}
					this.mtxQueue.Enqueue(Draw.Matrix);
				}
			}
			while (this.mtxQueue.Count > 0)
			{
				this.BranchFrom(this.mtxQueue.Dequeue());
			}
		}

		// Token: 0x0400005F RID: 95
		[Header("Line Style")]
		[Range(0f, 0.1f)]
		public float lineThickness = 0.1f;

		// Token: 0x04000060 RID: 96
		public Color lineColor = Color.white;

		// Token: 0x04000061 RID: 97
		[Header("Tree shape")]
		public int seed;

		// Token: 0x04000062 RID: 98
		[Range(1f, 2000f)]
		public int lineCount = 100;

		// Token: 0x04000063 RID: 99
		[Range(0f, 4f)]
		public int branchesMin = 1;

		// Token: 0x04000064 RID: 100
		[Range(1f, 5f)]
		public int branchesMax = 5;

		// Token: 0x04000065 RID: 101
		[Range(0f, 1f)]
		public float branchLengthMin = 0.25f;

		// Token: 0x04000066 RID: 102
		[Range(0f, 1f)]
		public float branchLengthMax = 1f;

		// Token: 0x04000067 RID: 103
		[Range(0f, 3.1415927f)]
		public float maxAngDeviation = 1.0471976f;

		// Token: 0x04000068 RID: 104
		public bool use3D;

		// Token: 0x04000069 RID: 105
		private int currentLineCount;

		// Token: 0x0400006A RID: 106
		private readonly Queue<Matrix4x4> mtxQueue = new Queue<Matrix4x4>();
	}
}
