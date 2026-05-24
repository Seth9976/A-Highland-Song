using System;
using UnityEngine.Scripting;

namespace UnityEngine.Rendering
{
	// Token: 0x0200041A RID: 1050
	[UsedByNativeCode]
	public struct VisibleLight : IEquatable<VisibleLight>
	{
		// Token: 0x170006D9 RID: 1753
		// (get) Token: 0x06002479 RID: 9337 RVA: 0x0003DA52 File Offset: 0x0003BC52
		public Light light
		{
			get
			{
				return (Light)Object.FindObjectFromInstanceID(this.m_InstanceId);
			}
		}

		// Token: 0x170006DA RID: 1754
		// (get) Token: 0x0600247A RID: 9338 RVA: 0x0003DA64 File Offset: 0x0003BC64
		// (set) Token: 0x0600247B RID: 9339 RVA: 0x0003DA7C File Offset: 0x0003BC7C
		public LightType lightType
		{
			get
			{
				return this.m_LightType;
			}
			set
			{
				this.m_LightType = value;
			}
		}

		// Token: 0x170006DB RID: 1755
		// (get) Token: 0x0600247C RID: 9340 RVA: 0x0003DA88 File Offset: 0x0003BC88
		// (set) Token: 0x0600247D RID: 9341 RVA: 0x0003DAA0 File Offset: 0x0003BCA0
		public Color finalColor
		{
			get
			{
				return this.m_FinalColor;
			}
			set
			{
				this.m_FinalColor = value;
			}
		}

		// Token: 0x170006DC RID: 1756
		// (get) Token: 0x0600247E RID: 9342 RVA: 0x0003DAAC File Offset: 0x0003BCAC
		// (set) Token: 0x0600247F RID: 9343 RVA: 0x0003DAC4 File Offset: 0x0003BCC4
		public Rect screenRect
		{
			get
			{
				return this.m_ScreenRect;
			}
			set
			{
				this.m_ScreenRect = value;
			}
		}

		// Token: 0x170006DD RID: 1757
		// (get) Token: 0x06002480 RID: 9344 RVA: 0x0003DAD0 File Offset: 0x0003BCD0
		// (set) Token: 0x06002481 RID: 9345 RVA: 0x0003DAE8 File Offset: 0x0003BCE8
		public Matrix4x4 localToWorldMatrix
		{
			get
			{
				return this.m_LocalToWorldMatrix;
			}
			set
			{
				this.m_LocalToWorldMatrix = value;
			}
		}

		// Token: 0x170006DE RID: 1758
		// (get) Token: 0x06002482 RID: 9346 RVA: 0x0003DAF4 File Offset: 0x0003BCF4
		// (set) Token: 0x06002483 RID: 9347 RVA: 0x0003DB0C File Offset: 0x0003BD0C
		public float range
		{
			get
			{
				return this.m_Range;
			}
			set
			{
				this.m_Range = value;
			}
		}

		// Token: 0x170006DF RID: 1759
		// (get) Token: 0x06002484 RID: 9348 RVA: 0x0003DB18 File Offset: 0x0003BD18
		// (set) Token: 0x06002485 RID: 9349 RVA: 0x0003DB30 File Offset: 0x0003BD30
		public float spotAngle
		{
			get
			{
				return this.m_SpotAngle;
			}
			set
			{
				this.m_SpotAngle = value;
			}
		}

		// Token: 0x170006E0 RID: 1760
		// (get) Token: 0x06002486 RID: 9350 RVA: 0x0003DB3C File Offset: 0x0003BD3C
		// (set) Token: 0x06002487 RID: 9351 RVA: 0x0003DB5C File Offset: 0x0003BD5C
		public bool intersectsNearPlane
		{
			get
			{
				return (this.m_Flags & VisibleLightFlags.IntersectsNearPlane) > (VisibleLightFlags)0;
			}
			set
			{
				if (value)
				{
					this.m_Flags |= VisibleLightFlags.IntersectsNearPlane;
				}
				else
				{
					this.m_Flags &= ~VisibleLightFlags.IntersectsNearPlane;
				}
			}
		}

		// Token: 0x170006E1 RID: 1761
		// (get) Token: 0x06002488 RID: 9352 RVA: 0x0003DB90 File Offset: 0x0003BD90
		// (set) Token: 0x06002489 RID: 9353 RVA: 0x0003DBB0 File Offset: 0x0003BDB0
		public bool intersectsFarPlane
		{
			get
			{
				return (this.m_Flags & VisibleLightFlags.IntersectsFarPlane) > (VisibleLightFlags)0;
			}
			set
			{
				if (value)
				{
					this.m_Flags |= VisibleLightFlags.IntersectsFarPlane;
				}
				else
				{
					this.m_Flags &= ~VisibleLightFlags.IntersectsFarPlane;
				}
			}
		}

		// Token: 0x0600248A RID: 9354 RVA: 0x0003DBE4 File Offset: 0x0003BDE4
		public bool Equals(VisibleLight other)
		{
			return this.m_LightType == other.m_LightType && this.m_FinalColor.Equals(other.m_FinalColor) && this.m_ScreenRect.Equals(other.m_ScreenRect) && this.m_LocalToWorldMatrix.Equals(other.m_LocalToWorldMatrix) && this.m_Range.Equals(other.m_Range) && this.m_SpotAngle.Equals(other.m_SpotAngle) && this.m_InstanceId == other.m_InstanceId && this.m_Flags == other.m_Flags;
		}

		// Token: 0x0600248B RID: 9355 RVA: 0x0003DC84 File Offset: 0x0003BE84
		public override bool Equals(object obj)
		{
			bool flag = obj == null;
			return !flag && obj is VisibleLight && this.Equals((VisibleLight)obj);
		}

		// Token: 0x0600248C RID: 9356 RVA: 0x0003DCBC File Offset: 0x0003BEBC
		public override int GetHashCode()
		{
			int num = (int)this.m_LightType;
			num = (num * 397) ^ this.m_FinalColor.GetHashCode();
			num = (num * 397) ^ this.m_ScreenRect.GetHashCode();
			num = (num * 397) ^ this.m_LocalToWorldMatrix.GetHashCode();
			num = (num * 397) ^ this.m_Range.GetHashCode();
			num = (num * 397) ^ this.m_SpotAngle.GetHashCode();
			num = (num * 397) ^ this.m_InstanceId;
			return (num * 397) ^ (int)this.m_Flags;
		}

		// Token: 0x0600248D RID: 9357 RVA: 0x0003DD6C File Offset: 0x0003BF6C
		public static bool operator ==(VisibleLight left, VisibleLight right)
		{
			return left.Equals(right);
		}

		// Token: 0x0600248E RID: 9358 RVA: 0x0003DD88 File Offset: 0x0003BF88
		public static bool operator !=(VisibleLight left, VisibleLight right)
		{
			return !left.Equals(right);
		}

		// Token: 0x04000D8A RID: 3466
		private LightType m_LightType;

		// Token: 0x04000D8B RID: 3467
		private Color m_FinalColor;

		// Token: 0x04000D8C RID: 3468
		private Rect m_ScreenRect;

		// Token: 0x04000D8D RID: 3469
		private Matrix4x4 m_LocalToWorldMatrix;

		// Token: 0x04000D8E RID: 3470
		private float m_Range;

		// Token: 0x04000D8F RID: 3471
		private float m_SpotAngle;

		// Token: 0x04000D90 RID: 3472
		private int m_InstanceId;

		// Token: 0x04000D91 RID: 3473
		private VisibleLightFlags m_Flags;
	}
}
