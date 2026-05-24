using System;

namespace UnityEngine.Rendering
{
	// Token: 0x020003FF RID: 1023
	public struct LODParameters : IEquatable<LODParameters>
	{
		// Token: 0x17000668 RID: 1640
		// (get) Token: 0x060022C9 RID: 8905 RVA: 0x0003A878 File Offset: 0x00038A78
		// (set) Token: 0x060022CA RID: 8906 RVA: 0x0003A895 File Offset: 0x00038A95
		public bool isOrthographic
		{
			get
			{
				return Convert.ToBoolean(this.m_IsOrthographic);
			}
			set
			{
				this.m_IsOrthographic = Convert.ToInt32(value);
			}
		}

		// Token: 0x17000669 RID: 1641
		// (get) Token: 0x060022CB RID: 8907 RVA: 0x0003A8A4 File Offset: 0x00038AA4
		// (set) Token: 0x060022CC RID: 8908 RVA: 0x0003A8BC File Offset: 0x00038ABC
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

		// Token: 0x1700066A RID: 1642
		// (get) Token: 0x060022CD RID: 8909 RVA: 0x0003A8C8 File Offset: 0x00038AC8
		// (set) Token: 0x060022CE RID: 8910 RVA: 0x0003A8E0 File Offset: 0x00038AE0
		public float fieldOfView
		{
			get
			{
				return this.m_FieldOfView;
			}
			set
			{
				this.m_FieldOfView = value;
			}
		}

		// Token: 0x1700066B RID: 1643
		// (get) Token: 0x060022CF RID: 8911 RVA: 0x0003A8EC File Offset: 0x00038AEC
		// (set) Token: 0x060022D0 RID: 8912 RVA: 0x0003A904 File Offset: 0x00038B04
		public float orthoSize
		{
			get
			{
				return this.m_OrthoSize;
			}
			set
			{
				this.m_OrthoSize = value;
			}
		}

		// Token: 0x1700066C RID: 1644
		// (get) Token: 0x060022D1 RID: 8913 RVA: 0x0003A910 File Offset: 0x00038B10
		// (set) Token: 0x060022D2 RID: 8914 RVA: 0x0003A928 File Offset: 0x00038B28
		public int cameraPixelHeight
		{
			get
			{
				return this.m_CameraPixelHeight;
			}
			set
			{
				this.m_CameraPixelHeight = value;
			}
		}

		// Token: 0x060022D3 RID: 8915 RVA: 0x0003A934 File Offset: 0x00038B34
		public bool Equals(LODParameters other)
		{
			return this.m_IsOrthographic == other.m_IsOrthographic && this.m_CameraPosition.Equals(other.m_CameraPosition) && this.m_FieldOfView.Equals(other.m_FieldOfView) && this.m_OrthoSize.Equals(other.m_OrthoSize) && this.m_CameraPixelHeight == other.m_CameraPixelHeight;
		}

		// Token: 0x060022D4 RID: 8916 RVA: 0x0003A9A0 File Offset: 0x00038BA0
		public override bool Equals(object obj)
		{
			bool flag = obj == null;
			return !flag && obj is LODParameters && this.Equals((LODParameters)obj);
		}

		// Token: 0x060022D5 RID: 8917 RVA: 0x0003A9D8 File Offset: 0x00038BD8
		public override int GetHashCode()
		{
			int num = this.m_IsOrthographic;
			num = (num * 397) ^ this.m_CameraPosition.GetHashCode();
			num = (num * 397) ^ this.m_FieldOfView.GetHashCode();
			num = (num * 397) ^ this.m_OrthoSize.GetHashCode();
			return (num * 397) ^ this.m_CameraPixelHeight;
		}

		// Token: 0x060022D6 RID: 8918 RVA: 0x0003AA44 File Offset: 0x00038C44
		public static bool operator ==(LODParameters left, LODParameters right)
		{
			return left.Equals(right);
		}

		// Token: 0x060022D7 RID: 8919 RVA: 0x0003AA60 File Offset: 0x00038C60
		public static bool operator !=(LODParameters left, LODParameters right)
		{
			return !left.Equals(right);
		}

		// Token: 0x04000CE8 RID: 3304
		private int m_IsOrthographic;

		// Token: 0x04000CE9 RID: 3305
		private Vector3 m_CameraPosition;

		// Token: 0x04000CEA RID: 3306
		private float m_FieldOfView;

		// Token: 0x04000CEB RID: 3307
		private float m_OrthoSize;

		// Token: 0x04000CEC RID: 3308
		private int m_CameraPixelHeight;
	}
}
