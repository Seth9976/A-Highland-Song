using System;
using UnityEngine;

namespace SplineSystem
{
	// Token: 0x02000228 RID: 552
	[Serializable]
	public struct SplineBezierPoint
	{
		// Token: 0x1700045B RID: 1115
		// (get) Token: 0x06001406 RID: 5126 RVA: 0x0008B843 File Offset: 0x00089A43
		public Vector3 forward
		{
			get
			{
				return this.rotation * Vector3.forward;
			}
		}

		// Token: 0x1700045C RID: 1116
		// (get) Token: 0x06001407 RID: 5127 RVA: 0x0008B855 File Offset: 0x00089A55
		public Vector3 normal
		{
			get
			{
				return this.rotation * Vector3.up;
			}
		}

		// Token: 0x1700045D RID: 1117
		// (get) Token: 0x06001408 RID: 5128 RVA: 0x0008B867 File Offset: 0x00089A67
		public Vector3 binormal
		{
			get
			{
				return this.rotation * Vector3.right;
			}
		}

		// Token: 0x06001409 RID: 5129 RVA: 0x0008B879 File Offset: 0x00089A79
		public SplineBezierPoint(Vector3 position, Quaternion rotation, float inControlPointDistance, float outControlPointDistance)
		{
			this.position = position;
			this.rotation = rotation;
			this.inControlPoint = new SplineBezierControlPoint(-1, inControlPointDistance);
			this.outControlPoint = new SplineBezierControlPoint(1, outControlPointDistance);
		}

		// Token: 0x04001300 RID: 4864
		[SerializeField]
		public SplineBezierControlPoint inControlPoint;

		// Token: 0x04001301 RID: 4865
		[SerializeField]
		public SplineBezierControlPoint outControlPoint;

		// Token: 0x04001302 RID: 4866
		public Vector3 position;

		// Token: 0x04001303 RID: 4867
		public Quaternion rotation;
	}
}
