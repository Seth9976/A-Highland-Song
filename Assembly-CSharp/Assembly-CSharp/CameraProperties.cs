using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

// Token: 0x020001E3 RID: 483
[Serializable]
public struct CameraProperties
{
	// Token: 0x170003FC RID: 1020
	// (get) Token: 0x060010F3 RID: 4339 RVA: 0x0007DB05 File Offset: 0x0007BD05
	public static CameraProperties @default
	{
		get
		{
			return new CameraProperties(Vector3.zero);
		}
	}

	// Token: 0x170003FD RID: 1021
	// (get) Token: 0x060010F4 RID: 4340 RVA: 0x0007DB11 File Offset: 0x0007BD11
	// (set) Token: 0x060010F5 RID: 4341 RVA: 0x0007DB1E File Offset: 0x0007BD1E
	public float yaw
	{
		get
		{
			return this.worldEulerAngles.y;
		}
		set
		{
			this.worldEulerAngles.y = value;
		}
	}

	// Token: 0x170003FE RID: 1022
	// (get) Token: 0x060010F6 RID: 4342 RVA: 0x0007DB2C File Offset: 0x0007BD2C
	// (set) Token: 0x060010F7 RID: 4343 RVA: 0x0007DB39 File Offset: 0x0007BD39
	public float pitch
	{
		get
		{
			return this.worldEulerAngles.x;
		}
		set
		{
			this.worldEulerAngles.x = value;
		}
	}

	// Token: 0x170003FF RID: 1023
	// (get) Token: 0x060010F8 RID: 4344 RVA: 0x0007DB47 File Offset: 0x0007BD47
	// (set) Token: 0x060010F9 RID: 4345 RVA: 0x0007DB54 File Offset: 0x0007BD54
	public float localYaw
	{
		get
		{
			return this.localEulerAngles.y;
		}
		set
		{
			this.localEulerAngles.y = value;
		}
	}

	// Token: 0x17000400 RID: 1024
	// (get) Token: 0x060010FA RID: 4346 RVA: 0x0007DB62 File Offset: 0x0007BD62
	// (set) Token: 0x060010FB RID: 4347 RVA: 0x0007DB6F File Offset: 0x0007BD6F
	public float localPitch
	{
		get
		{
			return this.localEulerAngles.x;
		}
		set
		{
			this.localEulerAngles.x = value;
		}
	}

	// Token: 0x17000401 RID: 1025
	// (get) Token: 0x060010FC RID: 4348 RVA: 0x0007DB7D File Offset: 0x0007BD7D
	// (set) Token: 0x060010FD RID: 4349 RVA: 0x0007DB8A File Offset: 0x0007BD8A
	public float localRoll
	{
		get
		{
			return this.localEulerAngles.z;
		}
		set
		{
			this.localEulerAngles.z = value;
		}
	}

	// Token: 0x17000402 RID: 1026
	// (get) Token: 0x060010FE RID: 4350 RVA: 0x0007DB98 File Offset: 0x0007BD98
	// (set) Token: 0x060010FF RID: 4351 RVA: 0x0007DBA5 File Offset: 0x0007BDA5
	public float horizontalViewportOffset
	{
		get
		{
			return this.viewportOffset.x;
		}
		set
		{
			this.viewportOffset.x = value;
		}
	}

	// Token: 0x17000403 RID: 1027
	// (get) Token: 0x06001100 RID: 4352 RVA: 0x0007DBB3 File Offset: 0x0007BDB3
	// (set) Token: 0x06001101 RID: 4353 RVA: 0x0007DBC0 File Offset: 0x0007BDC0
	public float verticalViewportOffset
	{
		get
		{
			return this.viewportOffset.y;
		}
		set
		{
			this.viewportOffset.y = value;
		}
	}

	// Token: 0x17000404 RID: 1028
	// (get) Token: 0x06001102 RID: 4354 RVA: 0x0007DBD0 File Offset: 0x0007BDD0
	public Quaternion rotation
	{
		get
		{
			Vector3 vector = this.worldEulerAngles + this.localEulerAngles;
			if (vector == Vector3.zero)
			{
				return this.axis;
			}
			return this.axis * Quaternion.Euler(vector);
		}
	}

	// Token: 0x17000405 RID: 1029
	// (get) Token: 0x06001103 RID: 4355 RVA: 0x0007DC19 File Offset: 0x0007BE19
	public Vector3 forward
	{
		get
		{
			return this.rotation * Vector3.forward;
		}
	}

	// Token: 0x17000406 RID: 1030
	// (get) Token: 0x06001104 RID: 4356 RVA: 0x0007DC2B File Offset: 0x0007BE2B
	public Vector3 right
	{
		get
		{
			return this.rotation * Vector3.right;
		}
	}

	// Token: 0x17000407 RID: 1031
	// (get) Token: 0x06001105 RID: 4357 RVA: 0x0007DC3D File Offset: 0x0007BE3D
	public Vector3 up
	{
		get
		{
			return this.rotation * Vector3.up;
		}
	}

	// Token: 0x17000408 RID: 1032
	// (get) Token: 0x06001106 RID: 4358 RVA: 0x0007DC50 File Offset: 0x0007BE50
	// (set) Token: 0x06001107 RID: 4359 RVA: 0x0007DCA0 File Offset: 0x0007BEA0
	public Vector3 basePosition
	{
		get
		{
			Quaternion quaternion = this.axis * Quaternion.Euler(this.worldEulerAngles);
			Vector3 vector = -this.distance * (quaternion * Vector3.forward);
			return this.targetPoint + vector;
		}
		set
		{
			Vector3 vector = this.targetPoint - value;
			Vector3 vector2 = Quaternion.Inverse(this.axis) * vector;
			this.worldEulerAngles = this.GetPitchAndYaw(vector2);
			this.distance = vector.magnitude;
		}
	}

	// Token: 0x06001108 RID: 4360 RVA: 0x0007DCE8 File Offset: 0x0007BEE8
	public CameraProperties(Vector3 targetPoint)
	{
		this.axis = Quaternion.identity;
		this.targetPoint = targetPoint;
		this.distance = 10f;
		this.worldEulerAngles = Vector2.zero;
		this.localEulerAngles = Vector3.zero;
		this.viewportOffset = Vector2.zero;
		this.fieldOfView = 60f;
		this.orthographic = false;
		this.orthographicSize = 10f;
	}

	// Token: 0x06001109 RID: 4361 RVA: 0x0007DD50 File Offset: 0x0007BF50
	public Vector3 PositionWithViewportAspectRatio(float aspect)
	{
		float num;
		if (this.orthographic)
		{
			num = this.orthographicSize * 2f;
		}
		else
		{
			num = this.distance * Mathf.Tan(this.fieldOfView * 0.5f * 0.017453292f) * 2f;
		}
		float num2 = aspect * num;
		Vector3 vector = new Vector3(num2 * this.viewportOffset.x, num * this.viewportOffset.y, 0f);
		Quaternion rotation = this.rotation;
		Vector3 vector2 = rotation * Vector3.up;
		Vector3 vector3 = rotation * Vector3.right;
		Vector3 vector4 = vector.x * vector3 + vector.y * vector2;
		return this.basePosition + vector4;
	}

	// Token: 0x0600110A RID: 4362 RVA: 0x0007DE14 File Offset: 0x0007C014
	public static CameraProperties OrbitingPoint(Vector3 targetPoint, Quaternion axis, Vector2 worldEulerAngles, float distance)
	{
		return new CameraProperties
		{
			targetPoint = targetPoint,
			axis = axis,
			worldEulerAngles = worldEulerAngles,
			distance = distance
		};
	}

	// Token: 0x0600110B RID: 4363 RVA: 0x0007DE4C File Offset: 0x0007C04C
	public static CameraProperties FromTo(Vector3 originPoint, Vector3 targetPoint)
	{
		return new CameraProperties
		{
			targetPoint = targetPoint,
			basePosition = originPoint
		};
	}

	// Token: 0x0600110C RID: 4364 RVA: 0x0007DE74 File Offset: 0x0007C074
	public static CameraProperties FromTo(Vector3 originPoint, Vector3 targetPoint, Quaternion axis)
	{
		return new CameraProperties
		{
			targetPoint = targetPoint,
			axis = axis,
			basePosition = originPoint
		};
	}

	// Token: 0x0600110D RID: 4365 RVA: 0x0007DEA2 File Offset: 0x0007C0A2
	public static CameraProperties Lerp(CameraProperties start, CameraProperties end, float lerp)
	{
		if (lerp <= 0f)
		{
			return start;
		}
		if (lerp >= 1f)
		{
			return end;
		}
		return CameraProperties.LerpUnclamped(start, end, lerp);
	}

	// Token: 0x0600110E RID: 4366 RVA: 0x0007DEC0 File Offset: 0x0007C0C0
	public static CameraProperties LerpUnclamped(CameraProperties start, CameraProperties end, float lerp)
	{
		CameraProperties cameraProperties = default(CameraProperties);
		cameraProperties.axis = Quaternion.SlerpUnclamped(start.axis, end.axis, lerp);
		cameraProperties.targetPoint = Vector3.LerpUnclamped(start.targetPoint, end.targetPoint, lerp);
		cameraProperties.basePosition = Vector3.LerpUnclamped(start.basePosition, end.basePosition, lerp);
		cameraProperties.localEulerAngles.x = CameraProperties.<LerpUnclamped>g__LerpAngleUnclamped|51_0(start.localEulerAngles.x, end.localEulerAngles.x, lerp);
		cameraProperties.localEulerAngles.y = CameraProperties.<LerpUnclamped>g__LerpAngleUnclamped|51_0(start.localEulerAngles.y, end.localEulerAngles.y, lerp);
		cameraProperties.localEulerAngles.z = CameraProperties.<LerpUnclamped>g__LerpAngleUnclamped|51_0(start.localEulerAngles.z, end.localEulerAngles.z, lerp);
		cameraProperties.viewportOffset.x = Mathf.LerpUnclamped(start.viewportOffset.x, end.viewportOffset.x, lerp);
		cameraProperties.viewportOffset.y = Mathf.LerpUnclamped(start.viewportOffset.y, end.viewportOffset.y, lerp);
		cameraProperties.fieldOfView = Mathf.LerpUnclamped(start.fieldOfView, end.fieldOfView, lerp);
		cameraProperties.orthographic = (((double)lerp < 0.5) ? start.orthographic : end.orthographic);
		cameraProperties.orthographicSize = Mathf.LerpUnclamped(start.orthographicSize, end.orthographicSize, lerp);
		return cameraProperties;
	}

	// Token: 0x0600110F RID: 4367 RVA: 0x0007E03F File Offset: 0x0007C23F
	public static CameraProperties SmoothDamp(CameraProperties start, CameraProperties end, ref CameraProperties velocity, float smoothTime, float deltaTime)
	{
		return CameraProperties.SmoothDamp(start, end, ref velocity, smoothTime, CameraProperties.CameraPropertiesMaxSpeed.Infinity, deltaTime);
	}

	// Token: 0x06001110 RID: 4368 RVA: 0x0007E054 File Offset: 0x0007C254
	public static CameraProperties SmoothDamp(CameraProperties start, CameraProperties end, ref CameraProperties velocity, float smoothTime, CameraProperties.CameraPropertiesMaxSpeed maxSpeed, float deltaTime)
	{
		if (deltaTime <= 0f)
		{
			return start;
		}
		CameraProperties cameraProperties = default(CameraProperties);
		cameraProperties.axis = CameraProperties.QuaternionSmoothDamp(start.axis, end.axis, ref velocity.axis, smoothTime, maxSpeed.axis, deltaTime);
		cameraProperties.targetPoint = Vector3.SmoothDamp(start.targetPoint, end.targetPoint, ref velocity.targetPoint, smoothTime, maxSpeed.targetPoint, deltaTime);
		cameraProperties.distance = Mathf.SmoothDamp(start.distance, end.distance, ref velocity.distance, smoothTime, maxSpeed.distance, deltaTime);
		cameraProperties.worldEulerAngles.x = Mathf.SmoothDampAngle(start.worldEulerAngles.x, end.worldEulerAngles.x, ref velocity.worldEulerAngles.x, smoothTime, maxSpeed.worldEulerAngles.x, deltaTime);
		cameraProperties.worldEulerAngles.y = Mathf.SmoothDampAngle(start.worldEulerAngles.y, end.worldEulerAngles.y, ref velocity.worldEulerAngles.y, smoothTime, maxSpeed.worldEulerAngles.y, deltaTime);
		cameraProperties.localEulerAngles.x = Mathf.SmoothDampAngle(start.localEulerAngles.x, end.localEulerAngles.x, ref velocity.localEulerAngles.x, smoothTime, maxSpeed.localEulerAngles.x, deltaTime);
		cameraProperties.localEulerAngles.y = Mathf.SmoothDampAngle(start.localEulerAngles.y, end.localEulerAngles.y, ref velocity.localEulerAngles.y, smoothTime, maxSpeed.localEulerAngles.y, deltaTime);
		cameraProperties.localEulerAngles.z = Mathf.SmoothDampAngle(start.localEulerAngles.z, end.localEulerAngles.z, ref velocity.localEulerAngles.z, smoothTime, maxSpeed.localEulerAngles.z, deltaTime);
		cameraProperties.viewportOffset.x = Mathf.SmoothDamp(start.viewportOffset.x, end.viewportOffset.x, ref velocity.viewportOffset.x, smoothTime, maxSpeed.viewportOffset.x, deltaTime);
		cameraProperties.viewportOffset.y = Mathf.SmoothDamp(start.viewportOffset.y, end.viewportOffset.y, ref velocity.viewportOffset.y, smoothTime, maxSpeed.viewportOffset.y, deltaTime);
		cameraProperties.fieldOfView = Mathf.SmoothDamp(start.fieldOfView, end.fieldOfView, ref velocity.fieldOfView, smoothTime, maxSpeed.fieldOfView, deltaTime);
		cameraProperties.orthographic = end.orthographic;
		cameraProperties.orthographicSize = Mathf.SmoothDamp(start.orthographicSize, end.orthographicSize, ref velocity.orthographicSize, smoothTime, maxSpeed.orthographicSize, deltaTime);
		return cameraProperties;
	}

	// Token: 0x06001111 RID: 4369 RVA: 0x0007E314 File Offset: 0x0007C514
	private static Quaternion QuaternionSmoothDamp(Quaternion rot, Quaternion target, ref Quaternion currentVelocity, float smoothTime, float maxSpeed, float deltaTime)
	{
		if (deltaTime == 0f)
		{
			return rot;
		}
		float num = ((Quaternion.Dot(rot, target) > 0f) ? 1f : (-1f));
		target.x *= num;
		target.y *= num;
		target.z *= num;
		target.w *= num;
		Vector4 normalized = new Vector4(Mathf.SmoothDamp(rot.x, target.x, ref currentVelocity.x, smoothTime, maxSpeed, deltaTime), Mathf.SmoothDamp(rot.y, target.y, ref currentVelocity.y, smoothTime, maxSpeed, deltaTime), Mathf.SmoothDamp(rot.z, target.z, ref currentVelocity.z, smoothTime, maxSpeed, deltaTime), Mathf.SmoothDamp(rot.w, target.w, ref currentVelocity.w, smoothTime, maxSpeed, deltaTime)).normalized;
		float num2 = 1f / deltaTime;
		currentVelocity.x = (normalized.x - rot.x) * num2;
		currentVelocity.y = (normalized.y - rot.y) * num2;
		currentVelocity.z = (normalized.z - rot.z) * num2;
		currentVelocity.w = (normalized.w - rot.w) * num2;
		return new Quaternion(normalized.x, normalized.y, normalized.z, normalized.w);
	}

	// Token: 0x06001112 RID: 4370 RVA: 0x0007E470 File Offset: 0x0007C670
	public static CameraProperties WeightedBlend(IEnumerable<CameraProperties> allProperties, IList<float> weights)
	{
		float num = weights.Sum();
		for (int i = 0; i < weights.Count; i++)
		{
			weights[i] /= num;
		}
		CameraProperties cameraProperties = default(CameraProperties);
		cameraProperties.axis = WeightedBlends.WeightedBlend<CameraProperties>(allProperties, (CameraProperties p) => p.axis, weights);
		cameraProperties.targetPoint = WeightedBlends.WeightedBlend<CameraProperties>(allProperties, (CameraProperties p) => p.targetPoint, weights);
		cameraProperties.distance = WeightedBlends.WeightedBlend<CameraProperties>(allProperties, (CameraProperties p) => p.distance, weights);
		cameraProperties.worldEulerAngles.x = WeightedBlends.WeightedBlendAngle<CameraProperties>(allProperties, (CameraProperties p) => p.worldEulerAngles.x, weights);
		cameraProperties.worldEulerAngles.y = WeightedBlends.WeightedBlendAngle<CameraProperties>(allProperties, (CameraProperties p) => p.worldEulerAngles.y, weights);
		cameraProperties.localEulerAngles.x = WeightedBlends.WeightedBlendAngle<CameraProperties>(allProperties, (CameraProperties p) => p.localEulerAngles.x, weights);
		cameraProperties.localEulerAngles.y = WeightedBlends.WeightedBlendAngle<CameraProperties>(allProperties, (CameraProperties p) => p.localEulerAngles.y, weights);
		cameraProperties.localEulerAngles.z = WeightedBlends.WeightedBlendAngle<CameraProperties>(allProperties, (CameraProperties p) => p.localEulerAngles.z, weights);
		cameraProperties.viewportOffset.x = WeightedBlends.WeightedBlend<CameraProperties>(allProperties, (CameraProperties p) => p.viewportOffset.x, weights);
		cameraProperties.viewportOffset.y = WeightedBlends.WeightedBlend<CameraProperties>(allProperties, (CameraProperties p) => p.viewportOffset.y, weights);
		cameraProperties.fieldOfView = WeightedBlends.WeightedBlend<CameraProperties>(allProperties, (CameraProperties p) => p.fieldOfView, weights);
		cameraProperties.orthographic = WeightedBlends.WeightedBlend(allProperties.Select((CameraProperties p) => p.orthographic), weights);
		cameraProperties.orthographicSize = WeightedBlends.WeightedBlend<CameraProperties>(allProperties, (CameraProperties p) => p.orthographicSize, weights);
		return cameraProperties;
	}

	// Token: 0x06001113 RID: 4371 RVA: 0x0007E720 File Offset: 0x0007C920
	public void Reset()
	{
		this.targetPoint = Vector3.zero;
		this.distance = 1f;
		this.viewportOffset.x = (this.viewportOffset.y = (this.worldEulerAngles.x = (this.worldEulerAngles.y = (this.localEulerAngles.x = (this.localEulerAngles.y = (this.localEulerAngles.z = 0f))))));
		this.fieldOfView = 60f;
		this.orthographicSize = 10f;
	}

	// Token: 0x06001114 RID: 4372 RVA: 0x0007E7C0 File Offset: 0x0007C9C0
	public void ApplyTo(ref SerializableCamera camera)
	{
		Quaternion rotation = this.rotation;
		camera.rotation = rotation;
		camera.fieldOfView = this.fieldOfView;
		camera.orthographic = this.orthographic;
		camera.orthographicSize = this.orthographicSize;
		camera.position = this.PositionWithViewportAspectRatio(camera.aspect);
	}

	// Token: 0x06001115 RID: 4373 RVA: 0x0007E814 File Offset: 0x0007CA14
	public void ApplyTo(Camera camera)
	{
		SerializableCamera serializableCamera = new SerializableCamera(camera);
		this.ApplyTo(ref serializableCamera);
		serializableCamera.ApplyTo(camera);
	}

	// Token: 0x06001116 RID: 4374 RVA: 0x0007E83C File Offset: 0x0007CA3C
	private float GetPitch(Vector3 v)
	{
		float num = Mathf.Sqrt(v.x * v.x + v.z * v.z);
		return -Mathf.Atan2(v.y, num) * 57.29578f;
	}

	// Token: 0x06001117 RID: 4375 RVA: 0x0007E87D File Offset: 0x0007CA7D
	private float GetYaw(Vector3 v)
	{
		return Mathf.Atan2(v.x, v.z) * 57.29578f;
	}

	// Token: 0x06001118 RID: 4376 RVA: 0x0007E896 File Offset: 0x0007CA96
	private Vector2 GetPitchAndYaw(Vector3 v)
	{
		return new Vector2(this.GetPitch(v), this.GetYaw(v));
	}

	// Token: 0x06001119 RID: 4377 RVA: 0x0007E8AC File Offset: 0x0007CAAC
	public override string ToString()
	{
		return string.Format("{0}: targetPoint: {1}, axis: {2}, distance: {3}, worldEulerAngles: {4}, localEulerAngles: {5}, viewportOffset: {6}, fieldOfView: {7}, orthographic: {8}, orthographicSize: {9}", new object[]
		{
			base.GetType(),
			this.targetPoint,
			this.axis,
			this.distance,
			this.worldEulerAngles,
			this.localEulerAngles,
			this.viewportOffset,
			this.fieldOfView,
			this.orthographic,
			this.orthographicSize
		});
	}

	// Token: 0x0600111A RID: 4378 RVA: 0x0007E95C File Offset: 0x0007CB5C
	[CompilerGenerated]
	internal static float <LerpUnclamped>g__LerpAngleUnclamped|51_0(float a, float b, float t)
	{
		float num = Mathf.Repeat(b - a, 360f);
		if (num > 180f)
		{
			num -= 360f;
		}
		return a + num * t;
	}

	// Token: 0x04001255 RID: 4693
	public const float defaultDistance = 10f;

	// Token: 0x04001256 RID: 4694
	public Vector3 targetPoint;

	// Token: 0x04001257 RID: 4695
	public Quaternion axis;

	// Token: 0x04001258 RID: 4696
	public float distance;

	// Token: 0x04001259 RID: 4697
	public Vector2 worldEulerAngles;

	// Token: 0x0400125A RID: 4698
	public Vector3 localEulerAngles;

	// Token: 0x0400125B RID: 4699
	public Vector2 viewportOffset;

	// Token: 0x0400125C RID: 4700
	public float fieldOfView;

	// Token: 0x0400125D RID: 4701
	public bool orthographic;

	// Token: 0x0400125E RID: 4702
	public float orthographicSize;

	// Token: 0x020003F3 RID: 1011
	[Flags]
	public enum CameraPropertiesAxis
	{
		// Token: 0x04001A8B RID: 6795
		None = 0,
		// Token: 0x04001A8C RID: 6796
		All = -1,
		// Token: 0x04001A8D RID: 6797
		TargetPoint = 1,
		// Token: 0x04001A8E RID: 6798
		Axis = 2,
		// Token: 0x04001A8F RID: 6799
		Distance = 4,
		// Token: 0x04001A90 RID: 6800
		WorldPitch = 8,
		// Token: 0x04001A91 RID: 6801
		WorldYaw = 16,
		// Token: 0x04001A92 RID: 6802
		LocalPitch = 32,
		// Token: 0x04001A93 RID: 6803
		LocalYaw = 64,
		// Token: 0x04001A94 RID: 6804
		LocalRoll = 128,
		// Token: 0x04001A95 RID: 6805
		HorizontalViewportOffset = 256,
		// Token: 0x04001A96 RID: 6806
		VerticalViewportOffset = 512,
		// Token: 0x04001A97 RID: 6807
		FieldOfView = 1024,
		// Token: 0x04001A98 RID: 6808
		Orthographic = 2048,
		// Token: 0x04001A99 RID: 6809
		OrthographicSize = 4096
	}

	// Token: 0x020003F4 RID: 1012
	[Serializable]
	public struct CameraPropertiesMaxSpeed
	{
		// Token: 0x17000506 RID: 1286
		// (get) Token: 0x060018B5 RID: 6325 RVA: 0x0009F104 File Offset: 0x0009D304
		public static CameraProperties.CameraPropertiesMaxSpeed Infinity
		{
			get
			{
				return new CameraProperties.CameraPropertiesMaxSpeed
				{
					axis = float.PositiveInfinity,
					targetPoint = float.PositiveInfinity,
					distance = float.PositiveInfinity,
					worldEulerAngles = new Vector2(float.PositiveInfinity, float.PositiveInfinity),
					localEulerAngles = new Vector3(float.PositiveInfinity, float.PositiveInfinity, float.PositiveInfinity),
					viewportOffset = new Vector2(float.PositiveInfinity, float.PositiveInfinity),
					fieldOfView = float.PositiveInfinity,
					orthographicSize = float.PositiveInfinity
				};
			}
		}

		// Token: 0x04001A9A RID: 6810
		public float axis;

		// Token: 0x04001A9B RID: 6811
		public float targetPoint;

		// Token: 0x04001A9C RID: 6812
		public float distance;

		// Token: 0x04001A9D RID: 6813
		public Vector2 worldEulerAngles;

		// Token: 0x04001A9E RID: 6814
		public Vector3 localEulerAngles;

		// Token: 0x04001A9F RID: 6815
		public Vector2 viewportOffset;

		// Token: 0x04001AA0 RID: 6816
		public float fieldOfView;

		// Token: 0x04001AA1 RID: 6817
		public float orthographicSize;
	}
}
