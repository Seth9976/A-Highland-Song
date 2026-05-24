using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

// Token: 0x02000200 RID: 512
public static class CameraX
{
	// Token: 0x060012C9 RID: 4809 RVA: 0x0008662C File Offset: 0x0008482C
	public static Rect ViewportToScreenRect(this Camera camera, Rect rect)
	{
		Vector3 vector = camera.ViewportToScreenPoint(rect.min);
		Vector3 vector2 = camera.ViewportToScreenPoint(rect.max);
		return Rect.MinMaxRect(vector.x, vector.y, vector2.x, vector2.y);
	}

	// Token: 0x060012CA RID: 4810 RVA: 0x0008667C File Offset: 0x0008487C
	public static Vector3 ViewportToScreenVector(this Camera camera, Vector2 vector)
	{
		return camera.ViewportToScreenPoint(vector) - camera.ViewportToScreenPoint(Vector2.zero);
	}

	// Token: 0x060012CB RID: 4811 RVA: 0x0008669F File Offset: 0x0008489F
	public static Vector3 ViewportToWorldVector(this Camera camera, Vector2 vector, float distance)
	{
		return camera.ViewportToWorldPoint(new Vector3(0f, 0f, distance)) - camera.ViewportToWorldPoint(new Vector3(vector.x, vector.y, distance));
	}

	// Token: 0x060012CC RID: 4812 RVA: 0x000866D4 File Offset: 0x000848D4
	public static Rect ScreenToViewportRect(this Camera camera, Rect rect)
	{
		Vector3 vector = camera.ScreenToViewportPoint(rect.min);
		Vector3 vector2 = camera.ScreenToViewportPoint(rect.max);
		return Rect.MinMaxRect(vector.x, vector.y, vector2.x, vector2.y);
	}

	// Token: 0x060012CD RID: 4813 RVA: 0x00086724 File Offset: 0x00084924
	public static Vector3 ScreenToWorldVector(this Camera camera, Vector2 vector, float distance)
	{
		return camera.ScreenToWorldPoint(new Vector3(0f, 0f, distance)) - camera.ScreenToWorldPoint(new Vector3(vector.x, vector.y, distance));
	}

	// Token: 0x060012CE RID: 4814 RVA: 0x00086759 File Offset: 0x00084959
	public static Vector3 ScreenToViewportVector(this Camera camera, Vector2 vector)
	{
		return camera.ScreenToViewportPoint(vector) - camera.ScreenToViewportPoint(Vector2.zero);
	}

	// Token: 0x060012CF RID: 4815 RVA: 0x0008677C File Offset: 0x0008497C
	public static Vector2 WorldToViewportVector(this Camera camera, Vector3 vector)
	{
		return camera.WorldToViewportPoint(Vector2.zero) - camera.WorldToViewportPoint(vector);
	}

	// Token: 0x060012D0 RID: 4816 RVA: 0x000867A0 File Offset: 0x000849A0
	public static Rect WorldToViewportRect(this Camera camera, Rect worldRect, float distance)
	{
		Vector3 vector = camera.WorldToViewportPoint(new Vector3(worldRect.x, worldRect.y, distance));
		Vector3 vector2 = camera.WorldToViewportPoint(new Vector3(worldRect.x + worldRect.width, worldRect.y + worldRect.height, distance));
		return Rect.MinMaxRect(vector.x, vector.y, vector2.x, vector2.y);
	}

	// Token: 0x060012D1 RID: 4817 RVA: 0x00086810 File Offset: 0x00084A10
	public static Vector2[] ScreenToViewportPoints(this Camera camera, Vector2[] screenPoints)
	{
		Vector2[] array = new Vector2[screenPoints.Length];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = camera.ScreenToViewportPoint(screenPoints[i]);
		}
		return array;
	}

	// Token: 0x060012D2 RID: 4818 RVA: 0x00086854 File Offset: 0x00084A54
	public static Vector2[] WorldToScreenPoints(this Camera camera, Vector3[] worldPoints)
	{
		Vector2[] array = new Vector2[worldPoints.Length];
		camera.WorldToScreenPoints(worldPoints, ref array);
		return array;
	}

	// Token: 0x060012D3 RID: 4819 RVA: 0x00086874 File Offset: 0x00084A74
	public static void WorldToScreenPoints(this Camera camera, Vector3[] worldPoints, ref Vector2[] screenPoints)
	{
		if (screenPoints == null || screenPoints.Length != worldPoints.Length)
		{
			screenPoints = new Vector2[worldPoints.Length];
		}
		for (int i = 0; i < screenPoints.Length; i++)
		{
			screenPoints[i] = camera.WorldToScreenPoint(worldPoints[i]);
		}
	}

	// Token: 0x060012D4 RID: 4820 RVA: 0x000868C4 File Offset: 0x00084AC4
	public static Rect WorldToScreenRect(this Camera camera, Bounds worldBounds)
	{
		Vector2[] array = camera.WorldToScreenPoints(CameraX.GetVerticesFromBounds(worldBounds));
		Rect rect = new Rect(array[0].x, array[0].y, 0f, 0f);
		for (int i = 1; i < array.Length; i++)
		{
			float num = Mathf.Min(rect.xMin, array[i].x);
			float num2 = Mathf.Max(rect.xMax, array[i].x);
			float num3 = Mathf.Min(rect.yMin, array[i].y);
			float num4 = Mathf.Max(rect.yMax, array[i].y);
			rect = Rect.MinMaxRect(num, num3, num2, num4);
		}
		return rect;
	}

	// Token: 0x060012D5 RID: 4821 RVA: 0x00086988 File Offset: 0x00084B88
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

	// Token: 0x060012D6 RID: 4822 RVA: 0x00086A70 File Offset: 0x00084C70
	public static Rect WorldToScreenRect(this Camera camera, Rect worldRect, float distance)
	{
		Vector3 vector = camera.WorldToScreenPoint(new Vector3(worldRect.x, worldRect.y, distance));
		Vector3 vector2 = camera.WorldToScreenPoint(new Vector3(worldRect.x + worldRect.width, worldRect.y + worldRect.height, distance));
		return Rect.MinMaxRect(vector.x, vector.y, vector2.x, vector2.y);
	}

	// Token: 0x060012D7 RID: 4823 RVA: 0x00086AE0 File Offset: 0x00084CE0
	public static Vector2[] WorldToViewportPoints(this Camera camera, IList<Vector3> input)
	{
		Vector2[] array = new Vector2[input.Count];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = camera.WorldToViewportPoint(input[i]);
		}
		return array;
	}

	// Token: 0x060012D8 RID: 4824 RVA: 0x00086B24 File Offset: 0x00084D24
	public static float CalculateOrthographicSize(float cameraAspect, Rect boundingBox, bool fitHorizontally = true, bool hitVertically = true)
	{
		if (!fitHorizontally && !hitVertically)
		{
			return float.PositiveInfinity;
		}
		float num = boundingBox.width / boundingBox.height;
		float num2 = cameraAspect / num;
		if (!hitVertically || num2 < 1f)
		{
			return Mathf.Abs(boundingBox.width) / cameraAspect / 2f;
		}
		return Mathf.Abs(boundingBox.height) / 2f;
	}

	// Token: 0x060012D9 RID: 4825 RVA: 0x00086B84 File Offset: 0x00084D84
	public static void FocusOnBounds(this Camera camera, Bounds bounds)
	{
		float num = bounds.size.magnitude / 2f;
		float num2 = Mathf.Min(camera.fieldOfView, camera.GetHorizontalFieldOfView());
		float num3 = num / Mathf.Sin(num2 * 0.5f * 0.017453292f);
		camera.transform.localPosition = new Vector3(camera.transform.localPosition.x, camera.transform.localPosition.y, num3);
		if (camera.orthographic)
		{
			camera.orthographicSize = num;
		}
		camera.transform.LookAt(bounds.center);
	}

	// Token: 0x060012DA RID: 4826 RVA: 0x00086C20 File Offset: 0x00084E20
	public static float GetHorizontalFieldOfView(this Camera camera)
	{
		return CameraX.GetHorizontalFieldOfView(camera.fieldOfView, camera.aspect);
	}

	// Token: 0x060012DB RID: 4827 RVA: 0x00086C33 File Offset: 0x00084E33
	public static float GetFrustrumHeightAtDistance(this Camera camera, float distance)
	{
		return CameraX.GetFrustrumHeightAtDistance(distance, camera.fieldOfView);
	}

	// Token: 0x060012DC RID: 4828 RVA: 0x00086C41 File Offset: 0x00084E41
	public static float GetFrustrumWidthAtDistance(this Camera camera, float distance)
	{
		return CameraX.GetFrustrumWidthAtDistance(distance, camera.fieldOfView, camera.aspect);
	}

	// Token: 0x060012DD RID: 4829 RVA: 0x00086C55 File Offset: 0x00084E55
	public static float GetDistanceAtFrustrumHeight(this Camera camera, float frustumHeight)
	{
		return CameraX.GetDistanceAtFrustrumHeight(frustumHeight, camera.fieldOfView);
	}

	// Token: 0x060012DE RID: 4830 RVA: 0x00086C63 File Offset: 0x00084E63
	public static float GetDistanceAtFrustrumWidth(this Camera camera, float frustumWidth)
	{
		return CameraX.GetDistanceAtFrustrumWidth(frustumWidth, camera.fieldOfView, camera.aspect);
	}

	// Token: 0x060012DF RID: 4831 RVA: 0x00086C77 File Offset: 0x00084E77
	public static float GetFOVAngleAtWidthAndDistance(this Camera camera, float frustumWidth, float distance)
	{
		return CameraX.GetFOVAngleAtWidthAndDistance(frustumWidth, distance, camera.aspect);
	}

	// Token: 0x060012E0 RID: 4832 RVA: 0x00086C86 File Offset: 0x00084E86
	public static float ConvertFrustumWidthToFrustumHeight(this Camera camera, float frustumWidth)
	{
		return CameraX.ConvertFrustumWidthToFrustumHeight(frustumWidth, camera.aspect);
	}

	// Token: 0x060012E1 RID: 4833 RVA: 0x00086C94 File Offset: 0x00084E94
	public static float ConvertFrustumHeightToFrustumWidth(this Camera camera, float frustumHeight)
	{
		return CameraX.ConvertFrustumHeightToFrustumWidth(frustumHeight, camera.aspect);
	}

	// Token: 0x060012E2 RID: 4834 RVA: 0x00086CA2 File Offset: 0x00084EA2
	public static float GetHorizontalFieldOfView(float verticalFieldOfView, float aspectRatio)
	{
		return 2f * Mathf.Atan(Mathf.Tan(verticalFieldOfView * 0.017453292f * 0.5f) * aspectRatio) * 57.29578f;
	}

	// Token: 0x060012E3 RID: 4835 RVA: 0x00086CC9 File Offset: 0x00084EC9
	public static float GetFrustrumHeightAtDistance(float distance, float fieldOfView)
	{
		return 2f * distance * Mathf.Tan(fieldOfView * 0.5f * 0.017453292f);
	}

	// Token: 0x060012E4 RID: 4836 RVA: 0x00086CE5 File Offset: 0x00084EE5
	public static float GetFrustrumWidthAtDistance(float distance, float fieldOfView, float aspectRatio)
	{
		return CameraX.ConvertFrustumHeightToFrustumWidth(CameraX.GetFrustrumHeightAtDistance(distance, fieldOfView), aspectRatio);
	}

	// Token: 0x060012E5 RID: 4837 RVA: 0x00086CF4 File Offset: 0x00084EF4
	public static float GetDistanceAtFrustrumHeight(float frustumHeight, float fieldOfView)
	{
		return frustumHeight * 0.5f / Mathf.Tan(fieldOfView * 0.5f * 0.017453292f);
	}

	// Token: 0x060012E6 RID: 4838 RVA: 0x00086D10 File Offset: 0x00084F10
	public static float GetDistanceAtFrustrumWidth(float frustumWidth, float fieldOfView, float aspectRatio)
	{
		return CameraX.GetDistanceAtFrustrumHeight(CameraX.ConvertFrustumWidthToFrustumHeight(frustumWidth, aspectRatio), fieldOfView);
	}

	// Token: 0x060012E7 RID: 4839 RVA: 0x00086D1F File Offset: 0x00084F1F
	public static float GetFOVAngleAtHeightAndDistance(float frustumHeight, float distance)
	{
		return 2f * Mathf.Atan(frustumHeight * 0.5f / distance) * 57.29578f;
	}

	// Token: 0x060012E8 RID: 4840 RVA: 0x00086D3B File Offset: 0x00084F3B
	public static float GetFOVAngleAtWidthAndDistance(float frustumWidth, float distance, float aspectRatio)
	{
		return CameraX.GetFOVAngleAtHeightAndDistance(CameraX.ConvertFrustumWidthToFrustumHeight(frustumWidth, aspectRatio), distance);
	}

	// Token: 0x060012E9 RID: 4841 RVA: 0x00086D4A File Offset: 0x00084F4A
	public static float ConvertFrustumWidthToFrustumHeight(float frustumWidth, float aspectRatio)
	{
		return frustumWidth / aspectRatio;
	}

	// Token: 0x060012EA RID: 4842 RVA: 0x00086D4F File Offset: 0x00084F4F
	public static float ConvertFrustumHeightToFrustumWidth(float frustumHeight, float aspectRatio)
	{
		return frustumHeight * aspectRatio;
	}

	// Token: 0x060012EB RID: 4843 RVA: 0x00086D54 File Offset: 0x00084F54
	public static Vector3 WorldToViewportPointClamped(this Camera camera, Vector3 worldPosition, bool allowOffscreenY = true, float margin = -0.05f)
	{
		Vector3 vector = camera.WorldToViewportPoint(worldPosition);
		Rect rect = new Rect(margin, margin, 1f - 2f * margin, 1f - 2f * margin);
		if (vector.z < 0f)
		{
			vector.x = 1f - vector.x;
			vector.y = 1f - vector.y;
		}
		Vector2 vector2 = new Vector2(0.5f, 0.5f);
		if (!rect.Contains(new Vector2(vector.x, vector.y)) || vector.z < 0f)
		{
			Vector2 vector3 = new Vector2(vector.x, vector.y) - vector2;
			if (!allowOffscreenY)
			{
				vector3.y = 0f;
			}
			Vector2 vector4 = CameraX.<WorldToViewportPointClamped>g__SplatVector|34_0(rect, vector3);
			vector.x = vector4.x;
			vector.y = vector4.y;
		}
		return vector;
	}

	// Token: 0x060012EC RID: 4844 RVA: 0x00086E44 File Offset: 0x00085044
	[CompilerGenerated]
	internal static Vector2 <WorldToViewportPointClamped>g__SplatVector|34_0(Rect rect, Vector2 vector)
	{
		if (vector == Vector2.zero)
		{
			return rect.center;
		}
		float num = Mathf.Abs(vector.x / vector.y);
		float num2 = rect.size.x / rect.size.y;
		float num3;
		if (num > num2)
		{
			num3 = Mathf.Abs(0.5f * rect.size.x / vector.x);
		}
		else
		{
			num3 = Mathf.Abs(0.5f * rect.size.y / vector.y);
		}
		return num3 * vector + rect.center;
	}
}
