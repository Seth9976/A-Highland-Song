using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020001E8 RID: 488
public static class CameraShotTools
{
	// Token: 0x0600114D RID: 4429 RVA: 0x000803D4 File Offset: 0x0007E5D4
	public static List<GameObject> FrustumCast(Camera camera, params GameObject[] gameObjects)
	{
		Plane[] array = GeometryUtility.CalculateFrustumPlanes(camera);
		List<GameObject> list = new List<GameObject>();
		foreach (GameObject gameObject in gameObjects)
		{
			Renderer component = gameObject.GetComponent<Renderer>();
			if (!(component == null) && GeometryUtility.TestPlanesAABB(array, component.bounds))
			{
				list.Add(gameObject);
			}
		}
		return list;
	}

	// Token: 0x0600114E RID: 4430 RVA: 0x0008042E File Offset: 0x0007E62E
	private static Vector3 GetPointInScreenSpaceFromViewportCoord(Camera camera, Vector2 point, float drawDistance)
	{
		return camera.ViewportToWorldPoint(new Vector3(point.x, point.y, drawDistance));
	}

	// Token: 0x0600114F RID: 4431 RVA: 0x00080448 File Offset: 0x0007E648
	public static void DrawLineInScreenSpaceFromViewportCoords(Camera camera, Vector2 a, Vector2 b, float drawDistance)
	{
		Gizmos.DrawLine(CameraShotTools.GetPointInScreenSpaceFromViewportCoord(camera, a, drawDistance), CameraShotTools.GetPointInScreenSpaceFromViewportCoord(camera, b, drawDistance));
	}

	// Token: 0x06001150 RID: 4432 RVA: 0x00080460 File Offset: 0x0007E660
	public static void DrawSquareInScreenSpaceFromViewportRect(Camera camera, Rect rect, float drawDistance)
	{
		Vector3 vector = camera.ViewportToWorldPoint(new Vector3(rect.center.x, rect.center.y, drawDistance));
		Vector3 vector2 = camera.ViewportToWorldPoint(new Vector3(rect.xMin, rect.yMin, drawDistance)) - camera.ViewportToWorldPoint(new Vector3(rect.xMax, rect.yMax, drawDistance));
		Vector2 vector3 = camera.transform.InverseTransformDirection(vector2) * 0.5f;
		Vector3 vector4 = vector + camera.transform.rotation * new Vector3(-vector3.x, vector3.y, 0f);
		Vector3 vector5 = vector + camera.transform.rotation * new Vector3(vector3.x, vector3.y, 0f);
		Vector3 vector6 = vector + camera.transform.rotation * new Vector3(vector3.x, -vector3.y, 0f);
		Vector3 vector7 = vector + camera.transform.rotation * new Vector3(-vector3.x, -vector3.y, 0f);
		Gizmos.DrawLine(vector4, vector5);
		Gizmos.DrawLine(vector5, vector6);
		Gizmos.DrawLine(vector6, vector7);
		Gizmos.DrawLine(vector7, vector4);
	}
}
