using System;
using UnityEngine;

namespace SplineSystem
{
	// Token: 0x02000226 RID: 550
	[Serializable]
	public struct SplineBezierControlPoint
	{
		// Token: 0x060013ED RID: 5101 RVA: 0x0008B1EF File Offset: 0x000893EF
		public Vector3 GetPosition(SplineBezierPoint bezierPoint)
		{
			return bezierPoint.position + this.GetDirection(bezierPoint) * this.distance;
		}

		// Token: 0x060013EE RID: 5102 RVA: 0x0008B20E File Offset: 0x0008940E
		public Vector3 GetDirection(SplineBezierPoint bezierPoint)
		{
			return bezierPoint.forward * (float)this.directionSign;
		}

		// Token: 0x060013EF RID: 5103 RVA: 0x0008B223 File Offset: 0x00089423
		public Quaternion GetRotation(SplineBezierPoint bezierPoint)
		{
			return Quaternion.LookRotation(this.GetDirection(bezierPoint), bezierPoint.normal);
		}

		// Token: 0x060013F0 RID: 5104 RVA: 0x0008B238 File Offset: 0x00089438
		public SplineBezierControlPoint(int directionSign, float distance)
		{
			this.directionSign = directionSign;
			this.distance = distance;
		}

		// Token: 0x040012F0 RID: 4848
		public int directionSign;

		// Token: 0x040012F1 RID: 4849
		public float distance;
	}
}
