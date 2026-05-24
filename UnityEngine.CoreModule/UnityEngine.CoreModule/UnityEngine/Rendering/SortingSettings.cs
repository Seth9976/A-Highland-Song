using System;

namespace UnityEngine.Rendering
{
	// Token: 0x02000415 RID: 1045
	public struct SortingSettings : IEquatable<SortingSettings>
	{
		// Token: 0x060023FB RID: 9211 RVA: 0x0003CCD4 File Offset: 0x0003AED4
		public SortingSettings(Camera camera)
		{
			ScriptableRenderContext.InitializeSortSettings(camera, out this);
			this.m_Criteria = this.criteria;
		}

		// Token: 0x170006AA RID: 1706
		// (get) Token: 0x060023FC RID: 9212 RVA: 0x0003CCEC File Offset: 0x0003AEEC
		// (set) Token: 0x060023FD RID: 9213 RVA: 0x0003CD04 File Offset: 0x0003AF04
		public Matrix4x4 worldToCameraMatrix
		{
			get
			{
				return this.m_WorldToCameraMatrix;
			}
			set
			{
				this.m_WorldToCameraMatrix = value;
			}
		}

		// Token: 0x170006AB RID: 1707
		// (get) Token: 0x060023FE RID: 9214 RVA: 0x0003CD10 File Offset: 0x0003AF10
		// (set) Token: 0x060023FF RID: 9215 RVA: 0x0003CD28 File Offset: 0x0003AF28
		public Vector3 cameraPosition
		{
			get
			{
				return this.m_CameraPosition;
			}
			set
			{
				this.m_CameraPosition = value;
			}
		}

		// Token: 0x170006AC RID: 1708
		// (get) Token: 0x06002400 RID: 9216 RVA: 0x0003CD34 File Offset: 0x0003AF34
		// (set) Token: 0x06002401 RID: 9217 RVA: 0x0003CD4C File Offset: 0x0003AF4C
		public Vector3 customAxis
		{
			get
			{
				return this.m_CustomAxis;
			}
			set
			{
				this.m_CustomAxis = value;
			}
		}

		// Token: 0x170006AD RID: 1709
		// (get) Token: 0x06002402 RID: 9218 RVA: 0x0003CD58 File Offset: 0x0003AF58
		// (set) Token: 0x06002403 RID: 9219 RVA: 0x0003CD70 File Offset: 0x0003AF70
		public SortingCriteria criteria
		{
			get
			{
				return this.m_Criteria;
			}
			set
			{
				this.m_Criteria = value;
			}
		}

		// Token: 0x170006AE RID: 1710
		// (get) Token: 0x06002404 RID: 9220 RVA: 0x0003CD7C File Offset: 0x0003AF7C
		// (set) Token: 0x06002405 RID: 9221 RVA: 0x0003CD94 File Offset: 0x0003AF94
		public DistanceMetric distanceMetric
		{
			get
			{
				return this.m_DistanceMetric;
			}
			set
			{
				this.m_DistanceMetric = value;
			}
		}

		// Token: 0x06002406 RID: 9222 RVA: 0x0003CDA0 File Offset: 0x0003AFA0
		public bool Equals(SortingSettings other)
		{
			return this.m_WorldToCameraMatrix.Equals(other.m_WorldToCameraMatrix) && this.m_CameraPosition.Equals(other.m_CameraPosition) && this.m_CustomAxis.Equals(other.m_CustomAxis) && this.m_Criteria == other.m_Criteria && this.m_DistanceMetric == other.m_DistanceMetric && this.m_PreviousVPMatrix.Equals(other.m_PreviousVPMatrix) && this.m_NonJitteredVPMatrix.Equals(other.m_NonJitteredVPMatrix);
		}

		// Token: 0x06002407 RID: 9223 RVA: 0x0003CE30 File Offset: 0x0003B030
		public override bool Equals(object obj)
		{
			bool flag = obj == null;
			return !flag && obj is SortingSettings && this.Equals((SortingSettings)obj);
		}

		// Token: 0x06002408 RID: 9224 RVA: 0x0003CE68 File Offset: 0x0003B068
		public override int GetHashCode()
		{
			int num = this.m_WorldToCameraMatrix.GetHashCode();
			num = (num * 397) ^ this.m_CameraPosition.GetHashCode();
			num = (num * 397) ^ this.m_CustomAxis.GetHashCode();
			num = (num * 397) ^ (int)this.m_Criteria;
			num = (num * 397) ^ (int)this.m_DistanceMetric;
			num = (num * 397) ^ this.m_PreviousVPMatrix.GetHashCode();
			return (num * 397) ^ this.m_NonJitteredVPMatrix.GetHashCode();
		}

		// Token: 0x06002409 RID: 9225 RVA: 0x0003CF14 File Offset: 0x0003B114
		public static bool operator ==(SortingSettings left, SortingSettings right)
		{
			return left.Equals(right);
		}

		// Token: 0x0600240A RID: 9226 RVA: 0x0003CF30 File Offset: 0x0003B130
		public static bool operator !=(SortingSettings left, SortingSettings right)
		{
			return !left.Equals(right);
		}

		// Token: 0x04000D53 RID: 3411
		private Matrix4x4 m_WorldToCameraMatrix;

		// Token: 0x04000D54 RID: 3412
		private Vector3 m_CameraPosition;

		// Token: 0x04000D55 RID: 3413
		private Vector3 m_CustomAxis;

		// Token: 0x04000D56 RID: 3414
		private SortingCriteria m_Criteria;

		// Token: 0x04000D57 RID: 3415
		private DistanceMetric m_DistanceMetric;

		// Token: 0x04000D58 RID: 3416
		private Matrix4x4 m_PreviousVPMatrix;

		// Token: 0x04000D59 RID: 3417
		private Matrix4x4 m_NonJitteredVPMatrix;
	}
}
