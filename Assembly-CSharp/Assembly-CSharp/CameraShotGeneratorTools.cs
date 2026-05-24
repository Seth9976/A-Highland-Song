using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Token: 0x020001E7 RID: 487
public static class CameraShotGeneratorTools
{
	// Token: 0x06001136 RID: 4406 RVA: 0x0007F87C File Offset: 0x0007DA7C
	public static List<Vector3> GetPointCloudFromTargets(params GameObject[] targets)
	{
		return CameraShotGeneratorTools.GetPointCloudFromTargetList(targets);
	}

	// Token: 0x06001137 RID: 4407 RVA: 0x0007F884 File Offset: 0x0007DA84
	public static List<Vector3> GetPointCloudFromTargetList(IList<GameObject> targets)
	{
		List<Vector3> list = new List<Vector3>();
		if (targets == null || targets.Count == 0)
		{
			Debug.LogWarning("Point cloud could not be created because Targets list is null or empty!");
		}
		else
		{
			for (int i = 0; i < targets.Count; i++)
			{
				if (!(targets[i] == null))
				{
					list.AddRange(CameraShotGeneratorTools.GetVerticesFromGameObject(targets[i]));
				}
			}
		}
		return list;
	}

	// Token: 0x06001138 RID: 4408 RVA: 0x0007F8E1 File Offset: 0x0007DAE1
	public static Vector3[] GetVerticesFromGameObject(GameObject go)
	{
		Vector3[] array;
		if ((array = CameraShotGeneratorTools.TryGetVerticesFromMeshFilter(go)) == null)
		{
			array = CameraShotGeneratorTools.TryGetVerticesFromSpriteRenderer(go) ?? CameraShotGeneratorTools.GetVerticesFromTransform(go.transform);
		}
		return array;
	}

	// Token: 0x06001139 RID: 4409 RVA: 0x0007F904 File Offset: 0x0007DB04
	private static Vector3[] TryGetVerticesFromMeshFilter(GameObject go)
	{
		MeshFilter component = go.GetComponent<MeshFilter>();
		if (component != null && component.sharedMesh != null)
		{
			return CameraShotGeneratorTools.GetVerticesFromMeshFilter(component);
		}
		return null;
	}

	// Token: 0x0600113A RID: 4410 RVA: 0x0007F938 File Offset: 0x0007DB38
	public static Vector3[] GetVerticesFromMeshFilter(MeshFilter meshFilter)
	{
		Vector3[] vertices = meshFilter.sharedMesh.vertices;
		for (int i = 0; i < vertices.Length; i++)
		{
			vertices[i] = meshFilter.transform.TransformPoint(vertices[i]);
		}
		return vertices;
	}

	// Token: 0x0600113B RID: 4411 RVA: 0x0007F97C File Offset: 0x0007DB7C
	private static Vector3[] TryGetVerticesFromSpriteRenderer(GameObject go)
	{
		SpriteRenderer component = go.GetComponent<SpriteRenderer>();
		if (component != null && component.enabled && component.sprite != null)
		{
			return CameraShotGeneratorTools.GetVerticesFromSpriteRenderer(component);
		}
		return null;
	}

	// Token: 0x0600113C RID: 4412 RVA: 0x0007F9B8 File Offset: 0x0007DBB8
	public static Vector3[] GetVerticesFromSpriteRenderer(SpriteRenderer spriteRenderer)
	{
		Bounds bounds = spriteRenderer.sprite.bounds;
		bounds.extents = new Vector3(bounds.extents.x, bounds.extents.y, 0f);
		Vector3[] verticesFromBounds = CameraShotGeneratorTools.GetVerticesFromBounds(bounds);
		for (int i = 0; i < verticesFromBounds.Length; i++)
		{
			verticesFromBounds[i] = spriteRenderer.transform.TransformPoint(verticesFromBounds[i]);
		}
		return verticesFromBounds;
	}

	// Token: 0x0600113D RID: 4413 RVA: 0x0007FA2C File Offset: 0x0007DC2C
	public static Vector3[] GetVerticesFromBounds(Bounds bounds)
	{
		Vector3 min = bounds.min;
		Vector3 max = bounds.max;
		return new Vector3[]
		{
			min,
			max,
			new Vector3(min.x, min.y, max.z),
			new Vector3(min.x, max.y, min.z),
			new Vector3(max.x, min.y, min.z),
			new Vector3(min.x, max.y, max.z),
			new Vector3(max.x, min.y, max.z),
			new Vector3(max.x, max.y, min.z)
		};
	}

	// Token: 0x0600113E RID: 4414 RVA: 0x0007FB14 File Offset: 0x0007DD14
	public static Vector3[] GetVerticesFromTransform(Transform transform)
	{
		Vector3 vector = transform.localScale * 0.5f;
		Vector3 vector2 = transform.position + transform.rotation * new Vector3(-vector.x, -vector.y, vector.z);
		Vector3 vector3 = transform.position + transform.rotation * new Vector3(vector.x, -vector.y, vector.z);
		Vector3 vector4 = transform.position + transform.rotation * new Vector3(-vector.x, -vector.y, -vector.z);
		Vector3 vector5 = transform.position + transform.rotation * new Vector3(vector.x, -vector.y, -vector.z);
		Vector3 vector6 = transform.position + transform.rotation * new Vector3(-vector.x, vector.y, vector.z);
		Vector3 vector7 = transform.position + transform.rotation * new Vector3(vector.x, vector.y, vector.z);
		Vector3 vector8 = transform.position + transform.rotation * new Vector3(-vector.x, vector.y, -vector.z);
		Vector3 vector9 = transform.position + transform.rotation * new Vector3(vector.x, vector.y, -vector.z);
		return new Vector3[] { vector2, vector3, vector4, vector5, vector6, vector7, vector8, vector9 };
	}

	// Token: 0x0600113F RID: 4415 RVA: 0x0007FCFE File Offset: 0x0007DEFE
	public static Rect GetViewportRectFromPointCloud(SerializableCamera camera, IList<Vector3> pointCloud)
	{
		return CameraShotGeneratorTools.CreateEncapsulating((from x in camera.WorldToViewportPoints(pointCloud)
			select (x)).ToArray<Vector2>());
	}

	// Token: 0x06001140 RID: 4416 RVA: 0x0007FD38 File Offset: 0x0007DF38
	public static bool CreateCameraShot(CameraShotGeneratorProperties shotGeneratorProperties, ref SerializableCamera camera)
	{
		float num;
		return CameraShotGeneratorTools.CreateCameraShot(shotGeneratorProperties, ref camera, out num);
	}

	// Token: 0x06001141 RID: 4417 RVA: 0x0007FD50 File Offset: 0x0007DF50
	public static bool CreateCameraShot(CameraShotGeneratorProperties shotGeneratorProperties, ref SerializableCamera camera, out float distanceFromTarget)
	{
		distanceFromTarget = 0f;
		if (!shotGeneratorProperties.isValid)
		{
			shotGeneratorProperties.fitHorizontally = true;
			shotGeneratorProperties.rotation = Quaternion.identity;
			if (!shotGeneratorProperties.isValid)
			{
				return false;
			}
			Debug.LogError("Shot generator properties are invalid. Using default values.");
		}
		camera.transform.position = Vector3.zero;
		camera.rect = shotGeneratorProperties.viewportRect;
		if (camera.rect.width == 0f || camera.rect.height == 0f)
		{
			Debug.LogError("Camera rect " + camera.rect.ToString() + " is invalid");
			return false;
		}
		if (camera.farClipPlane <= camera.nearClipPlane)
		{
			Debug.LogWarning("Camera far clip plane " + camera.farClipPlane.ToString() + " is invalid");
			return false;
		}
		camera.fieldOfView = shotGeneratorProperties.fieldOfView;
		camera.transform.rotation = shotGeneratorProperties.rotation;
		camera.orthographic = shotGeneratorProperties.orthographic;
		Vector3 vector = CameraShotGeneratorTools.ClosestTargetInDirection(shotGeneratorProperties.rotation * Vector3.forward, shotGeneratorProperties.pointCloud);
		CameraShotGeneratorTools.FrameOrthographic(ref camera, vector, shotGeneratorProperties);
		if (!shotGeneratorProperties.orthographic)
		{
			for (int i = 0; i < 10; i++)
			{
				CameraShotGeneratorTools.FramePerspective(ref camera, vector, shotGeneratorProperties, out distanceFromTarget);
			}
		}
		return true;
	}

	// Token: 0x06001142 RID: 4418 RVA: 0x0007FEA0 File Offset: 0x0007E0A0
	public static void FrameOrthographic(ref SerializableCamera camera, Vector3 closestTargetInDirection, CameraShotGeneratorProperties shotGeneratorProperties)
	{
		global::BoundingSphere boundingSphere = new global::BoundingSphere();
		boundingSphere.CreateFromPoints(shotGeneratorProperties.pointCloud.ToArray());
		float num = CameraShotGeneratorTools.DistanceInDirection(boundingSphere.center - closestTargetInDirection, shotGeneratorProperties.rotation * Vector3.forward);
		Rect pointCloudRectRelativeToDirection = CameraShotGeneratorTools.GetPointCloudRectRelativeToDirection(shotGeneratorProperties.pointCloud.ToArray(), shotGeneratorProperties.rotation * Vector3.forward);
		float distanceFromTarget = CameraShotGeneratorTools.GetDistanceFromTarget(camera, shotGeneratorProperties, pointCloudRectRelativeToDirection.width, pointCloudRectRelativeToDirection.height);
		camera.transform.position = boundingSphere.center;
		camera.transform.Translate(-Vector3.forward * distanceFromTarget, Space.Self);
		camera.transform.Translate(-Vector3.forward * num, Space.Self);
		if (shotGeneratorProperties.orthographic)
		{
			camera.orthographicSize = CameraX.CalculateOrthographicSize(camera.aspect, pointCloudRectRelativeToDirection, shotGeneratorProperties.fitHorizontally, shotGeneratorProperties.fitVertically) * (1f / shotGeneratorProperties.zoom);
		}
	}

	// Token: 0x06001143 RID: 4419 RVA: 0x0007FFA0 File Offset: 0x0007E1A0
	private static void FramePerspective(ref SerializableCamera camera, Vector3 closestTargetInDirection, CameraShotGeneratorProperties shotGeneratorProperties, out float distanceFromTarget)
	{
		float num = CameraShotGeneratorTools.DistanceInDirection(closestTargetInDirection - camera.transform.position, camera.transform.forward);
		Vector2[] rectVertices = CameraShotGeneratorTools.GetRectVertices(CameraShotGeneratorTools.GetViewportRectFromPointCloud(camera, shotGeneratorProperties.pointCloud));
		Vector2 vector = Vector2.Lerp(rectVertices[0], rectVertices[2], 0.5f);
		distanceFromTarget = CameraShotGeneratorTools.GetDistanceFromTarget(camera, shotGeneratorProperties, num, rectVertices);
		Vector3 vector2 = camera.ViewportToWorldPoint(new Vector3(vector.x, vector.y, num));
		camera.transform.position = vector2;
		camera.transform.Translate(-Vector3.forward * distanceFromTarget, Space.Self);
	}

	// Token: 0x06001144 RID: 4420 RVA: 0x00080054 File Offset: 0x0007E254
	private static void OffsetShot(ref SerializableCamera camera, float distanceFromTarget, Vector2 offset)
	{
		Vector3 vector = camera.ViewportToWorldPoint(new Vector3(1f, 1f, distanceFromTarget)) - camera.ViewportToWorldPoint(new Vector3(0f, 0f, distanceFromTarget));
		Vector2 vector2 = Vector3.Scale(camera.transform.InverseTransformDirection(vector), -offset);
		camera.transform.Translate(vector2, Space.Self);
	}

	// Token: 0x06001145 RID: 4421 RVA: 0x000800D4 File Offset: 0x0007E2D4
	public static float GetDistanceFromTarget(SerializableCamera camera, CameraShotGeneratorProperties shotGeneratorProperties, float closestPointCameraDistance, Vector2[] viewportRectVertices)
	{
		Rect rect = CameraShotGeneratorTools.CreateEncapsulating(CameraShotGeneratorTools.GetFlattenedTargetRectVertices(camera, closestPointCameraDistance, viewportRectVertices));
		return CameraShotGeneratorTools.GetDistanceFromTarget(camera, shotGeneratorProperties, rect.size.x, rect.size.y);
	}

	// Token: 0x06001146 RID: 4422 RVA: 0x00080110 File Offset: 0x0007E310
	public static float GetDistanceFromTarget(SerializableCamera camera, CameraShotGeneratorProperties shotGeneratorProperties, float width, float height)
	{
		float num = 0f;
		if (shotGeneratorProperties.fitHorizontally)
		{
			if (camera.orthographic)
			{
				num = width;
			}
			else
			{
				num = camera.GetDistanceAtFrustrumWidth(width * (1f / shotGeneratorProperties.zoom));
			}
		}
		float num2 = 0f;
		if (shotGeneratorProperties.fitVertically)
		{
			if (camera.orthographic)
			{
				num2 = height;
			}
			else
			{
				num2 = camera.GetDistanceAtFrustrumHeight(height * (1f / shotGeneratorProperties.zoom));
			}
		}
		return Mathf.Max(num, num2);
	}

	// Token: 0x06001147 RID: 4423 RVA: 0x00080188 File Offset: 0x0007E388
	private static Vector3 ClosestTargetInDirection(Vector3 forwardDirection, List<Vector3> points)
	{
		int num = 0;
		float num2 = CameraShotGeneratorTools.DistanceInDirection(points[num], forwardDirection);
		for (int i = 1; i < points.Count; i++)
		{
			float num3 = CameraShotGeneratorTools.DistanceInDirection(points[i], forwardDirection);
			if (num3 < num2)
			{
				num2 = num3;
				num = i;
			}
		}
		return points[num];
	}

	// Token: 0x06001148 RID: 4424 RVA: 0x000801D3 File Offset: 0x0007E3D3
	private static float DistanceInDirection(Vector3 vectorToPoint, Vector3 direction)
	{
		return Vector3.Dot(direction.normalized, vectorToPoint);
	}

	// Token: 0x06001149 RID: 4425 RVA: 0x000801E4 File Offset: 0x0007E3E4
	private static Rect GetPointCloudRectRelativeToDirection(Vector3[] pointCloud, Vector3 forward)
	{
		Matrix4x4 inverse = Matrix4x4.TRS(Vector3.zero, Quaternion.LookRotation(forward), Vector3.one).inverse;
		Vector2[] array = new Vector2[pointCloud.Length];
		for (int i = 0; i < pointCloud.Length; i++)
		{
			Vector3 vector = inverse.MultiplyPoint(pointCloud[i]);
			array[i] = vector;
		}
		return CameraShotGeneratorTools.CreateEncapsulating(array);
	}

	// Token: 0x0600114A RID: 4426 RVA: 0x0008024C File Offset: 0x0007E44C
	private static Vector2[] GetFlattenedTargetRectVertices(SerializableCamera camera, float closestPointCameraDistance, Vector2[] viewportRectVertices)
	{
		Vector2[] array = new Vector2[viewportRectVertices.Length];
		for (int i = 0; i < array.Length; i++)
		{
			Vector3 vector = camera.ViewportToWorldPoint(new Vector3(viewportRectVertices[i].x, viewportRectVertices[i].y, closestPointCameraDistance));
			array[i] = camera.transform.InverseTransformDirection(vector);
		}
		return array;
	}

	// Token: 0x0600114B RID: 4427 RVA: 0x000802B0 File Offset: 0x0007E4B0
	public static Rect CreateEncapsulating(params Vector2[] vectors)
	{
		Rect rect = new Rect(vectors[0].x, vectors[0].y, 0f, 0f);
		for (int i = 1; i < vectors.Length; i++)
		{
			float num = Mathf.Min(rect.xMin, vectors[i].x);
			float num2 = Mathf.Max(rect.xMax, vectors[i].x);
			float num3 = Mathf.Min(rect.yMin, vectors[i].y);
			float num4 = Mathf.Max(rect.yMax, vectors[i].y);
			rect = Rect.MinMaxRect(num, num3, num2, num4);
		}
		return rect;
	}

	// Token: 0x0600114C RID: 4428 RVA: 0x00080364 File Offset: 0x0007E564
	public static Vector2[] GetRectVertices(Rect rect)
	{
		Vector2[] array = new Vector2[4];
		Vector2 max = rect.max;
		array[0] = rect.min;
		array[1] = new Vector2(max.x, array[0].y);
		array[2] = max;
		array[3] = new Vector2(array[0].x, max.y);
		return array;
	}

	// Token: 0x04001266 RID: 4710
	private const int numOrthographicIterations = 10;
}
