using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine.Scripting;

namespace UnityEngine.Rendering
{
	// Token: 0x020003EF RID: 1007
	[UsedByNativeCode]
	public struct CameraProperties : IEquatable<CameraProperties>
	{
		// Token: 0x06002233 RID: 8755 RVA: 0x00038A60 File Offset: 0x00036C60
		public unsafe Plane GetShadowCullingPlane(int index)
		{
			bool flag = index < 0 || index >= 6;
			if (flag)
			{
				throw new ArgumentOutOfRangeException(string.Format("{0} was {1}, but must be at least 0 and less than {2}", "index", index, 6));
			}
			fixed (byte* ptr = &this.m_ShadowCullPlanes.FixedElementField)
			{
				byte* ptr2 = ptr;
				Plane* ptr3 = (Plane*)ptr2;
				return ptr3[index];
			}
		}

		// Token: 0x06002234 RID: 8756 RVA: 0x00038ACC File Offset: 0x00036CCC
		public unsafe void SetShadowCullingPlane(int index, Plane plane)
		{
			bool flag = index < 0 || index >= 6;
			if (flag)
			{
				throw new ArgumentOutOfRangeException(string.Format("{0} was {1}, but must be at least 0 and less than {2}", "index", index, 6));
			}
			fixed (byte* ptr = &this.m_ShadowCullPlanes.FixedElementField)
			{
				byte* ptr2 = ptr;
				Plane* ptr3 = (Plane*)ptr2;
				ptr3[index] = plane;
			}
		}

		// Token: 0x06002235 RID: 8757 RVA: 0x00038B38 File Offset: 0x00036D38
		public unsafe Plane GetCameraCullingPlane(int index)
		{
			bool flag = index < 0 || index >= 6;
			if (flag)
			{
				throw new ArgumentOutOfRangeException(string.Format("{0} was {1}, but must be at least 0 and less than {2}", "index", index, 6));
			}
			fixed (byte* ptr = &this.m_CameraCullPlanes.FixedElementField)
			{
				byte* ptr2 = ptr;
				Plane* ptr3 = (Plane*)ptr2;
				return ptr3[index];
			}
		}

		// Token: 0x06002236 RID: 8758 RVA: 0x00038BA4 File Offset: 0x00036DA4
		public unsafe void SetCameraCullingPlane(int index, Plane plane)
		{
			bool flag = index < 0 || index >= 6;
			if (flag)
			{
				throw new ArgumentOutOfRangeException(string.Format("{0} was {1}, but must be at least 0 and less than {2}", "index", index, 6));
			}
			fixed (byte* ptr = &this.m_CameraCullPlanes.FixedElementField)
			{
				byte* ptr2 = ptr;
				Plane* ptr3 = (Plane*)ptr2;
				ptr3[index] = plane;
			}
		}

		// Token: 0x06002237 RID: 8759 RVA: 0x00038C10 File Offset: 0x00036E10
		public unsafe bool Equals(CameraProperties other)
		{
			for (int i = 0; i < 6; i++)
			{
				bool flag = !this.GetShadowCullingPlane(i).Equals(other.GetShadowCullingPlane(i));
				if (flag)
				{
					return false;
				}
			}
			for (int j = 0; j < 6; j++)
			{
				bool flag2 = !this.GetCameraCullingPlane(j).Equals(other.GetCameraCullingPlane(j));
				if (flag2)
				{
					return false;
				}
			}
			fixed (float* ptr = &this.layerCullDistances.FixedElementField)
			{
				float* ptr2 = ptr;
				for (int k = 0; k < 32; k++)
				{
					bool flag3 = ptr2[k] != *((ref other.layerCullDistances.FixedElementField) + (IntPtr)k * 4);
					if (flag3)
					{
						return false;
					}
				}
			}
			return this.screenRect.Equals(other.screenRect) && this.viewDir.Equals(other.viewDir) && this.projectionNear.Equals(other.projectionNear) && this.projectionFar.Equals(other.projectionFar) && this.cameraNear.Equals(other.cameraNear) && this.cameraFar.Equals(other.cameraFar) && this.cameraAspect.Equals(other.cameraAspect) && this.cameraToWorld.Equals(other.cameraToWorld) && this.actualWorldToClip.Equals(other.actualWorldToClip) && this.cameraClipToWorld.Equals(other.cameraClipToWorld) && this.cameraWorldToClip.Equals(other.cameraWorldToClip) && this.implicitProjection.Equals(other.implicitProjection) && this.stereoWorldToClipLeft.Equals(other.stereoWorldToClipLeft) && this.stereoWorldToClipRight.Equals(other.stereoWorldToClipRight) && this.worldToCamera.Equals(other.worldToCamera) && this.up.Equals(other.up) && this.right.Equals(other.right) && this.transformDirection.Equals(other.transformDirection) && this.cameraEuler.Equals(other.cameraEuler) && this.velocity.Equals(other.velocity) && this.farPlaneWorldSpaceLength.Equals(other.farPlaneWorldSpaceLength) && this.rendererCount == other.rendererCount && this.baseFarDistance.Equals(other.baseFarDistance) && this.shadowCullCenter.Equals(other.shadowCullCenter) && this.layerCullSpherical == other.layerCullSpherical && this.coreCameraValues.Equals(other.coreCameraValues) && this.cameraType == other.cameraType && this.projectionIsOblique == other.projectionIsOblique && this.isImplicitProjectionMatrix == other.isImplicitProjectionMatrix;
		}

		// Token: 0x06002238 RID: 8760 RVA: 0x00038F64 File Offset: 0x00037164
		public override bool Equals(object obj)
		{
			bool flag = obj == null;
			return !flag && obj is CameraProperties && this.Equals((CameraProperties)obj);
		}

		// Token: 0x06002239 RID: 8761 RVA: 0x00038F9C File Offset: 0x0003719C
		public unsafe override int GetHashCode()
		{
			int num = this.screenRect.GetHashCode();
			num = (num * 397) ^ this.viewDir.GetHashCode();
			num = (num * 397) ^ this.projectionNear.GetHashCode();
			num = (num * 397) ^ this.projectionFar.GetHashCode();
			num = (num * 397) ^ this.cameraNear.GetHashCode();
			num = (num * 397) ^ this.cameraFar.GetHashCode();
			num = (num * 397) ^ this.cameraAspect.GetHashCode();
			num = (num * 397) ^ this.cameraToWorld.GetHashCode();
			num = (num * 397) ^ this.actualWorldToClip.GetHashCode();
			num = (num * 397) ^ this.cameraClipToWorld.GetHashCode();
			num = (num * 397) ^ this.cameraWorldToClip.GetHashCode();
			num = (num * 397) ^ this.implicitProjection.GetHashCode();
			num = (num * 397) ^ this.stereoWorldToClipLeft.GetHashCode();
			num = (num * 397) ^ this.stereoWorldToClipRight.GetHashCode();
			num = (num * 397) ^ this.worldToCamera.GetHashCode();
			num = (num * 397) ^ this.up.GetHashCode();
			num = (num * 397) ^ this.right.GetHashCode();
			num = (num * 397) ^ this.transformDirection.GetHashCode();
			num = (num * 397) ^ this.cameraEuler.GetHashCode();
			num = (num * 397) ^ this.velocity.GetHashCode();
			num = (num * 397) ^ this.farPlaneWorldSpaceLength.GetHashCode();
			num = (num * 397) ^ (int)this.rendererCount;
			for (int i = 0; i < 6; i++)
			{
				num = (num * 397) ^ this.GetShadowCullingPlane(i).GetHashCode();
			}
			for (int j = 0; j < 6; j++)
			{
				num = (num * 397) ^ this.GetCameraCullingPlane(j).GetHashCode();
			}
			num = (num * 397) ^ this.baseFarDistance.GetHashCode();
			num = (num * 397) ^ this.shadowCullCenter.GetHashCode();
			fixed (float* ptr = &this.layerCullDistances.FixedElementField)
			{
				float* ptr2 = ptr;
				for (int k = 0; k < 32; k++)
				{
					num = (num * 397) ^ ptr2[k].GetHashCode();
				}
			}
			num = (num * 397) ^ this.layerCullSpherical;
			num = (num * 397) ^ this.coreCameraValues.GetHashCode();
			num = (num * 397) ^ (int)this.cameraType;
			num = (num * 397) ^ this.projectionIsOblique;
			return (num * 397) ^ this.isImplicitProjectionMatrix;
		}

		// Token: 0x0600223A RID: 8762 RVA: 0x000392EC File Offset: 0x000374EC
		public static bool operator ==(CameraProperties left, CameraProperties right)
		{
			return left.Equals(right);
		}

		// Token: 0x0600223B RID: 8763 RVA: 0x00039308 File Offset: 0x00037508
		public static bool operator !=(CameraProperties left, CameraProperties right)
		{
			return !left.Equals(right);
		}

		// Token: 0x04000C79 RID: 3193
		private const int k_NumLayers = 32;

		// Token: 0x04000C7A RID: 3194
		private Rect screenRect;

		// Token: 0x04000C7B RID: 3195
		private Vector3 viewDir;

		// Token: 0x04000C7C RID: 3196
		private float projectionNear;

		// Token: 0x04000C7D RID: 3197
		private float projectionFar;

		// Token: 0x04000C7E RID: 3198
		private float cameraNear;

		// Token: 0x04000C7F RID: 3199
		private float cameraFar;

		// Token: 0x04000C80 RID: 3200
		private float cameraAspect;

		// Token: 0x04000C81 RID: 3201
		private Matrix4x4 cameraToWorld;

		// Token: 0x04000C82 RID: 3202
		private Matrix4x4 actualWorldToClip;

		// Token: 0x04000C83 RID: 3203
		private Matrix4x4 cameraClipToWorld;

		// Token: 0x04000C84 RID: 3204
		private Matrix4x4 cameraWorldToClip;

		// Token: 0x04000C85 RID: 3205
		private Matrix4x4 implicitProjection;

		// Token: 0x04000C86 RID: 3206
		private Matrix4x4 stereoWorldToClipLeft;

		// Token: 0x04000C87 RID: 3207
		private Matrix4x4 stereoWorldToClipRight;

		// Token: 0x04000C88 RID: 3208
		private Matrix4x4 worldToCamera;

		// Token: 0x04000C89 RID: 3209
		private Vector3 up;

		// Token: 0x04000C8A RID: 3210
		private Vector3 right;

		// Token: 0x04000C8B RID: 3211
		private Vector3 transformDirection;

		// Token: 0x04000C8C RID: 3212
		private Vector3 cameraEuler;

		// Token: 0x04000C8D RID: 3213
		private Vector3 velocity;

		// Token: 0x04000C8E RID: 3214
		private float farPlaneWorldSpaceLength;

		// Token: 0x04000C8F RID: 3215
		private uint rendererCount;

		// Token: 0x04000C90 RID: 3216
		private const int k_PlaneCount = 6;

		// Token: 0x04000C91 RID: 3217
		[FixedBuffer(typeof(byte), 96)]
		internal CameraProperties.<m_ShadowCullPlanes>e__FixedBuffer m_ShadowCullPlanes;

		// Token: 0x04000C92 RID: 3218
		[FixedBuffer(typeof(byte), 96)]
		internal CameraProperties.<m_CameraCullPlanes>e__FixedBuffer m_CameraCullPlanes;

		// Token: 0x04000C93 RID: 3219
		private float baseFarDistance;

		// Token: 0x04000C94 RID: 3220
		private Vector3 shadowCullCenter;

		// Token: 0x04000C95 RID: 3221
		[FixedBuffer(typeof(float), 32)]
		internal CameraProperties.<layerCullDistances>e__FixedBuffer layerCullDistances;

		// Token: 0x04000C96 RID: 3222
		private int layerCullSpherical;

		// Token: 0x04000C97 RID: 3223
		private CoreCameraValues coreCameraValues;

		// Token: 0x04000C98 RID: 3224
		private uint cameraType;

		// Token: 0x04000C99 RID: 3225
		private int projectionIsOblique;

		// Token: 0x04000C9A RID: 3226
		private int isImplicitProjectionMatrix;

		// Token: 0x020003F0 RID: 1008
		[CompilerGenerated]
		[UnsafeValueType]
		[StructLayout(0, Size = 96)]
		public struct <m_ShadowCullPlanes>e__FixedBuffer
		{
			// Token: 0x04000C9B RID: 3227
			public byte FixedElementField;
		}

		// Token: 0x020003F1 RID: 1009
		[UnsafeValueType]
		[CompilerGenerated]
		[StructLayout(0, Size = 96)]
		public struct <m_CameraCullPlanes>e__FixedBuffer
		{
			// Token: 0x04000C9C RID: 3228
			public byte FixedElementField;
		}

		// Token: 0x020003F2 RID: 1010
		[CompilerGenerated]
		[UnsafeValueType]
		[StructLayout(0, Size = 128)]
		public struct <layerCullDistances>e__FixedBuffer
		{
			// Token: 0x04000C9D RID: 3229
			public float FixedElementField;
		}
	}
}
