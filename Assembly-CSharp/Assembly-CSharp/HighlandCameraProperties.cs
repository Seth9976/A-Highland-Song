using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Token: 0x0200002E RID: 46
[Serializable]
public struct HighlandCameraProperties
{
	// Token: 0x17000052 RID: 82
	// (get) Token: 0x0600012D RID: 301 RVA: 0x0000C903 File Offset: 0x0000AB03
	public static Quaternion rotation
	{
		get
		{
			return Quaternion.identity;
		}
	}

	// Token: 0x17000053 RID: 83
	// (get) Token: 0x0600012E RID: 302 RVA: 0x0000C90C File Offset: 0x0000AB0C
	public static HighlandCameraProperties @default
	{
		get
		{
			return new HighlandCameraProperties
			{
				distance = 10f,
				fieldOfView = 60f,
				depthOfFieldStrength = 1f,
				clipPlaneSplitPoint = 150f,
				viewportScale = 1f
			};
		}
	}

	// Token: 0x17000054 RID: 84
	// (get) Token: 0x0600012F RID: 303 RVA: 0x0000C95E File Offset: 0x0000AB5E
	public Vector3 position
	{
		get
		{
			return this.targetPoint + Vector3.back * this.distance;
		}
	}

	// Token: 0x06000130 RID: 304 RVA: 0x0000C97B File Offset: 0x0000AB7B
	public void SetPositionAndTargetZ(Vector3 newPosition, float targetZ)
	{
		this.targetPoint.z = targetZ;
		this.SetPositionMaintainingTargetZ(newPosition);
	}

	// Token: 0x06000131 RID: 305 RVA: 0x0000C990 File Offset: 0x0000AB90
	public void SetPositionMaintainingTargetZ(Vector3 newPosition)
	{
		float z = this.targetPoint.z;
		this.targetPoint = new Vector3(newPosition.x, newPosition.y, z);
		this.distance = z - newPosition.z;
	}

	// Token: 0x06000132 RID: 306 RVA: 0x0000C9CF File Offset: 0x0000ABCF
	public void SetPositionMaintainingDistance(Vector3 newPosition)
	{
		this.targetPoint = newPosition + Vector3.forward * this.distance;
	}

	// Token: 0x06000133 RID: 307 RVA: 0x0000C9F0 File Offset: 0x0000ABF0
	public Matrix4x4 CreateProjectionMatrix(float aspect, float nearClipPlane, float farClipPlane, bool flipX = false)
	{
		Matrix4x4 matrix4x = this.CreateViewportAdjustmentMatrix(flipX);
		Matrix4x4 matrix4x2 = Matrix4x4.Perspective(this.fieldOfView, aspect, nearClipPlane, farClipPlane);
		Matrix4x4 matrix4x3 = this.CreateShearingMatrix();
		return matrix4x * matrix4x2 * matrix4x3;
	}

	// Token: 0x06000134 RID: 308 RVA: 0x0000CA27 File Offset: 0x0000AC27
	public Matrix4x4 CreateShearingMatrix()
	{
		return HighlandCameraProperties.CreateShearingMatrix(this.shearFactor, this.distance);
	}

	// Token: 0x06000135 RID: 309 RVA: 0x0000CA3A File Offset: 0x0000AC3A
	public Matrix4x4 CreateViewportAdjustmentMatrix(bool flipX = false)
	{
		return HighlandCameraProperties.CreateViewportAdjustmentMatrix(this.viewport, this.viewportScale, flipX);
	}

	// Token: 0x06000136 RID: 310 RVA: 0x0000CA50 File Offset: 0x0000AC50
	public static Matrix4x4 CreateShearingMatrix(float shearFactor, float distanceFromTarget)
	{
		Matrix4x4 identity = Matrix4x4.identity;
		identity.m12 = -shearFactor;
		Vector3 vector = new Vector3(0f, 0f, -distanceFromTarget);
		Vector3 vector2 = identity.MultiplyPoint(vector);
		identity.m13 = -vector2.y;
		return identity;
	}

	// Token: 0x06000137 RID: 311 RVA: 0x0000CA98 File Offset: 0x0000AC98
	public static Matrix4x4 CreateViewportAdjustmentMatrix(Vector2 viewport, float viewportScale, bool flipX = false)
	{
		if (flipX)
		{
			return Matrix4x4.Translate(new Vector2(-viewport.x, viewport.y)) * Matrix4x4.Scale(new Vector3(viewportScale, viewportScale, 1f));
		}
		return Matrix4x4.Translate(viewport) * Matrix4x4.Scale(new Vector3(viewportScale, viewportScale, 1f));
	}

	// Token: 0x06000138 RID: 312 RVA: 0x0000CAFC File Offset: 0x0000ACFC
	public void ApplyTo(Camera camera, bool flipX = false)
	{
		camera.transform.position = this.position;
		camera.transform.rotation = HighlandCameraProperties.rotation;
		this.ApplyNonTransformPropertiesTo(camera, flipX);
	}

	// Token: 0x06000139 RID: 313 RVA: 0x0000CB27 File Offset: 0x0000AD27
	public void ApplyNonTransformPropertiesTo(Camera camera, bool flipX = false)
	{
		camera.fieldOfView = this.fieldOfView;
		camera.projectionMatrix = this.CreateProjectionMatrix(camera.aspect, camera.nearClipPlane, camera.farClipPlane, flipX);
	}

	// Token: 0x0600013A RID: 314 RVA: 0x0000CB54 File Offset: 0x0000AD54
	public void ApplyTo(SerializableCamera camera, bool flipX = false)
	{
		camera.transform.position = this.position;
		camera.transform.rotation = HighlandCameraProperties.rotation;
		this.ApplyNonTransformPropertiesTo(camera, flipX);
	}

	// Token: 0x0600013B RID: 315 RVA: 0x0000CB81 File Offset: 0x0000AD81
	public void ApplyNonTransformPropertiesTo(SerializableCamera camera, bool flipX = false)
	{
		camera.fieldOfView = this.fieldOfView;
		camera.projectionMatrix = this.CreateProjectionMatrix(camera.aspect, camera.nearClipPlane, camera.farClipPlane, flipX);
	}

	// Token: 0x0600013C RID: 316 RVA: 0x0000CBB4 File Offset: 0x0000ADB4
	public static float SmoothDampZoomLog(float zoom, float targetZoom, ref float zoomCountVelocity, float zoomSmoothTime)
	{
		float num = Mathf.Log(zoom);
		float num2 = Mathf.Log(targetZoom);
		zoom = Mathf.Exp(Mathf.SmoothDamp(num, num2, ref zoomCountVelocity, zoomSmoothTime));
		return zoom;
	}

	// Token: 0x0600013D RID: 317 RVA: 0x0000CBE0 File Offset: 0x0000ADE0
	public static float LerpZoomLog(float zoom, float targetZoom, float lerp)
	{
		float num = Mathf.Log(zoom);
		float num2 = Mathf.Log(targetZoom);
		zoom = Mathf.Exp(Mathf.Lerp(num, num2, lerp));
		return zoom;
	}

	// Token: 0x0600013E RID: 318 RVA: 0x0000CC0C File Offset: 0x0000AE0C
	public static HighlandCameraProperties Lerp(HighlandCameraProperties start, HighlandCameraProperties end, float lerp)
	{
		if (lerp <= 0.0001f)
		{
			return start;
		}
		if (lerp >= 0.9999f)
		{
			return end;
		}
		HighlandCameraProperties highlandCameraProperties = default(HighlandCameraProperties);
		highlandCameraProperties.distance = HighlandCameraProperties.LerpZoomLog(start.distance, end.distance, lerp);
		float num = lerp;
		if (Mathf.Abs(start.distance - end.distance) > 0.0001f)
		{
			num = Mathf.InverseLerp(start.distance, end.distance, highlandCameraProperties.distance);
		}
		highlandCameraProperties.targetPoint = Vector3.Lerp(start.targetPoint, end.targetPoint, num);
		highlandCameraProperties.fieldOfView = Mathf.Lerp(start.fieldOfView, end.fieldOfView, num);
		highlandCameraProperties.clipPlaneSplitPoint = Mathf.Lerp(start.clipPlaneSplitPoint, end.clipPlaneSplitPoint, lerp);
		highlandCameraProperties.depthOfFieldStrength = Mathf.Lerp(start.depthOfFieldStrength, end.depthOfFieldStrength, lerp);
		highlandCameraProperties.shearFactor = Mathf.Lerp(start.shearFactor, end.shearFactor, lerp);
		highlandCameraProperties.viewport = Vector2.Lerp(start.viewport, end.viewport, lerp);
		highlandCameraProperties.viewportScale = Mathf.Lerp(start.viewportScale, end.viewportScale, lerp);
		return highlandCameraProperties;
	}

	// Token: 0x0600013F RID: 319 RVA: 0x0000CD34 File Offset: 0x0000AF34
	public static HighlandCameraProperties SmoothDamp(HighlandCameraProperties start, HighlandCameraProperties end, ref HighlandCameraProperties velocity, float smoothTime, float deltaTime)
	{
		if (deltaTime <= 0f)
		{
			return start;
		}
		return new HighlandCameraProperties
		{
			targetPoint = Vector3.SmoothDamp(start.targetPoint, end.targetPoint, ref velocity.targetPoint, smoothTime, float.PositiveInfinity, deltaTime),
			distance = Mathf.SmoothDamp(start.distance, end.distance, ref velocity.distance, smoothTime, float.PositiveInfinity, deltaTime),
			fieldOfView = Mathf.SmoothDamp(start.fieldOfView, end.fieldOfView, ref velocity.fieldOfView, smoothTime, float.PositiveInfinity, deltaTime),
			clipPlaneSplitPoint = Mathf.SmoothDamp(start.clipPlaneSplitPoint, end.clipPlaneSplitPoint, ref velocity.clipPlaneSplitPoint, smoothTime, float.PositiveInfinity, deltaTime),
			depthOfFieldStrength = Mathf.SmoothDamp(start.depthOfFieldStrength, end.depthOfFieldStrength, ref velocity.depthOfFieldStrength, smoothTime, float.PositiveInfinity, deltaTime),
			shearFactor = Mathf.SmoothDamp(start.shearFactor, end.shearFactor, ref velocity.shearFactor, smoothTime, float.PositiveInfinity, deltaTime),
			viewport = Vector2.SmoothDamp(start.viewport, end.viewport, ref velocity.viewport, smoothTime, float.PositiveInfinity, deltaTime),
			viewportScale = Mathf.SmoothDamp(start.viewportScale, end.viewportScale, ref velocity.viewportScale, smoothTime, float.PositiveInfinity, deltaTime)
		};
	}

	// Token: 0x06000140 RID: 320 RVA: 0x0000CE88 File Offset: 0x0000B088
	public static HighlandCameraProperties WeightedBlend(IEnumerable<HighlandCameraProperties> allProperties, IList<float> weights)
	{
		float num = weights.Sum();
		for (int i = 0; i < weights.Count; i++)
		{
			weights[i] /= num;
		}
		HighlandCameraProperties highlandCameraProperties = default(HighlandCameraProperties);
		highlandCameraProperties.targetPoint = WeightedBlends.WeightedBlend<HighlandCameraProperties>(allProperties, (HighlandCameraProperties p) => p.targetPoint, weights);
		highlandCameraProperties.distance = WeightedBlends.WeightedBlend<HighlandCameraProperties>(allProperties, (HighlandCameraProperties p) => p.distance, weights);
		highlandCameraProperties.fieldOfView = WeightedBlends.WeightedBlend<HighlandCameraProperties>(allProperties, (HighlandCameraProperties p) => p.fieldOfView, weights);
		highlandCameraProperties.clipPlaneSplitPoint = WeightedBlends.WeightedBlend<HighlandCameraProperties>(allProperties, (HighlandCameraProperties p) => p.clipPlaneSplitPoint, weights);
		highlandCameraProperties.depthOfFieldStrength = WeightedBlends.WeightedBlend<HighlandCameraProperties>(allProperties, (HighlandCameraProperties p) => p.depthOfFieldStrength, weights);
		highlandCameraProperties.shearFactor = WeightedBlends.WeightedBlend<HighlandCameraProperties>(allProperties, (HighlandCameraProperties p) => p.shearFactor, weights);
		highlandCameraProperties.viewport = WeightedBlends.WeightedBlend<HighlandCameraProperties>(allProperties, (HighlandCameraProperties p) => p.viewport, weights);
		highlandCameraProperties.viewportScale = WeightedBlends.WeightedBlend<HighlandCameraProperties>(allProperties, (HighlandCameraProperties p) => p.viewportScale, weights);
		return highlandCameraProperties;
	}

	// Token: 0x04000206 RID: 518
	public Vector3 targetPoint;

	// Token: 0x04000207 RID: 519
	public float distance;

	// Token: 0x04000208 RID: 520
	public float fieldOfView;

	// Token: 0x04000209 RID: 521
	public float clipPlaneSplitPoint;

	// Token: 0x0400020A RID: 522
	public float depthOfFieldStrength;

	// Token: 0x0400020B RID: 523
	public float shearFactor;

	// Token: 0x0400020C RID: 524
	public Vector2 viewport;

	// Token: 0x0400020D RID: 525
	public float viewportScale;

	// Token: 0x0400020E RID: 526
	private const float epsilon = 0.0001f;

	// Token: 0x0200026A RID: 618
	[Flags]
	public enum HighlandCameraPropertiesAxis
	{
		// Token: 0x0400148C RID: 5260
		None = 0,
		// Token: 0x0400148D RID: 5261
		All = -1,
		// Token: 0x0400148E RID: 5262
		TargetPoint = 1,
		// Token: 0x0400148F RID: 5263
		Distance = 2,
		// Token: 0x04001490 RID: 5264
		FieldOfView = 4,
		// Token: 0x04001491 RID: 5265
		ClipPlaneSplitPoint = 8,
		// Token: 0x04001492 RID: 5266
		DepthOfFieldStrength = 16,
		// Token: 0x04001493 RID: 5267
		ShearFactor = 32,
		// Token: 0x04001494 RID: 5268
		Viewport = 64,
		// Token: 0x04001495 RID: 5269
		ViewportScale = 128
	}
}
