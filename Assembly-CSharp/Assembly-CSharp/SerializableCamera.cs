using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

// Token: 0x020001F0 RID: 496
[Serializable]
public struct SerializableCamera
{
	// Token: 0x1700041F RID: 1055
	// (get) Token: 0x06001207 RID: 4615 RVA: 0x00083567 File Offset: 0x00081767
	// (set) Token: 0x06001208 RID: 4616 RVA: 0x00083574 File Offset: 0x00081774
	public Vector3 position
	{
		get
		{
			return this.transform.position;
		}
		set
		{
			this.transform.position = value;
		}
	}

	// Token: 0x17000420 RID: 1056
	// (get) Token: 0x06001209 RID: 4617 RVA: 0x00083582 File Offset: 0x00081782
	// (set) Token: 0x0600120A RID: 4618 RVA: 0x0008358F File Offset: 0x0008178F
	public Quaternion rotation
	{
		get
		{
			return this.transform.rotation;
		}
		set
		{
			this.transform.rotation = value;
		}
	}

	// Token: 0x17000421 RID: 1057
	// (get) Token: 0x0600120B RID: 4619 RVA: 0x000835A0 File Offset: 0x000817A0
	// (set) Token: 0x0600120C RID: 4620 RVA: 0x0008362E File Offset: 0x0008182E
	public Matrix4x4 projectionMatrix
	{
		get
		{
			if (!this._projectionMatrixSet)
			{
				this._projectionMatrixSet = true;
				if (this.orthographic)
				{
					this._projectionMatrix = Matrix4x4.Ortho(-this.aspect * this.orthographicSize, this.aspect * this.orthographicSize, -this.orthographicSize, this.orthographicSize, this.nearClipPlane, this.farClipPlane);
				}
				else
				{
					this._projectionMatrix = Matrix4x4.Perspective(this.fieldOfView, this.aspect, this.nearClipPlane, this.farClipPlane);
				}
			}
			return this._projectionMatrix;
		}
		set
		{
			this._projectionMatrixSet = true;
			this._projectionMatrix = value;
		}
	}

	// Token: 0x17000422 RID: 1058
	// (get) Token: 0x0600120D RID: 4621 RVA: 0x00083640 File Offset: 0x00081840
	public Matrix4x4 inverseProjectionMatrix
	{
		get
		{
			if (!this._inverseProjectionMatrixSet)
			{
				this._inverseProjectionMatrixSet = true;
				this._inverseProjectionMatrix = this.projectionMatrix.inverse;
			}
			return this._inverseProjectionMatrix;
		}
	}

	// Token: 0x17000423 RID: 1059
	// (get) Token: 0x0600120E RID: 4622 RVA: 0x00083676 File Offset: 0x00081876
	// (set) Token: 0x0600120F RID: 4623 RVA: 0x0008367E File Offset: 0x0008187E
	public bool orthographic
	{
		get
		{
			return this._orthographic;
		}
		set
		{
			this._orthographic = value;
			this._projectionMatrixSet = false;
			this._inverseProjectionMatrixSet = false;
		}
	}

	// Token: 0x17000424 RID: 1060
	// (get) Token: 0x06001210 RID: 4624 RVA: 0x00083695 File Offset: 0x00081895
	// (set) Token: 0x06001211 RID: 4625 RVA: 0x0008369D File Offset: 0x0008189D
	public float orthographicSize
	{
		get
		{
			return this._orthographicSize;
		}
		set
		{
			this._orthographicSize = value;
			this._projectionMatrixSet = false;
			this._inverseProjectionMatrixSet = false;
		}
	}

	// Token: 0x17000425 RID: 1061
	// (get) Token: 0x06001212 RID: 4626 RVA: 0x000836B4 File Offset: 0x000818B4
	// (set) Token: 0x06001213 RID: 4627 RVA: 0x000836BC File Offset: 0x000818BC
	public float fieldOfView
	{
		get
		{
			return this._fieldOfView;
		}
		set
		{
			this._fieldOfView = value;
			this._projectionMatrixSet = false;
			this._inverseProjectionMatrixSet = false;
		}
	}

	// Token: 0x17000426 RID: 1062
	// (get) Token: 0x06001214 RID: 4628 RVA: 0x000836D3 File Offset: 0x000818D3
	// (set) Token: 0x06001215 RID: 4629 RVA: 0x000836DB File Offset: 0x000818DB
	public float nearClipPlane
	{
		get
		{
			return this._nearClipPlane;
		}
		set
		{
			this._nearClipPlane = value;
			this._projectionMatrixSet = false;
			this._inverseProjectionMatrixSet = false;
		}
	}

	// Token: 0x17000427 RID: 1063
	// (get) Token: 0x06001216 RID: 4630 RVA: 0x000836F2 File Offset: 0x000818F2
	// (set) Token: 0x06001217 RID: 4631 RVA: 0x000836FA File Offset: 0x000818FA
	public float farClipPlane
	{
		get
		{
			return this._farClipPlane;
		}
		set
		{
			this._farClipPlane = value;
			this._projectionMatrixSet = false;
			this._inverseProjectionMatrixSet = false;
		}
	}

	// Token: 0x17000428 RID: 1064
	// (get) Token: 0x06001218 RID: 4632 RVA: 0x00083711 File Offset: 0x00081911
	// (set) Token: 0x06001219 RID: 4633 RVA: 0x00083719 File Offset: 0x00081919
	public Rect rect
	{
		get
		{
			return this._rect;
		}
		set
		{
			this._rect = value;
			this._projectionMatrixSet = false;
			this._inverseProjectionMatrixSet = false;
		}
	}

	// Token: 0x17000429 RID: 1065
	// (get) Token: 0x0600121A RID: 4634 RVA: 0x00083730 File Offset: 0x00081930
	private Rect clampedRect
	{
		get
		{
			return Rect.MinMaxRect(Mathf.Clamp01(this.rect.xMin), Mathf.Clamp01(this.rect.yMin), Mathf.Clamp01(this.rect.xMax), Mathf.Clamp01(this.rect.yMax));
		}
	}

	// Token: 0x1700042A RID: 1066
	// (get) Token: 0x0600121B RID: 4635 RVA: 0x00083790 File Offset: 0x00081990
	public float aspect
	{
		get
		{
			return (float)SerializableCamera.screenWidth * this.clampedRect.width / ((float)SerializableCamera.screenHeight * this.clampedRect.height);
		}
	}

	// Token: 0x1700042B RID: 1067
	// (get) Token: 0x0600121C RID: 4636 RVA: 0x000837C8 File Offset: 0x000819C8
	public int pixelWidth
	{
		get
		{
			return Mathf.RoundToInt((float)SerializableCamera.screenWidth * this.clampedRect.width);
		}
	}

	// Token: 0x1700042C RID: 1068
	// (get) Token: 0x0600121D RID: 4637 RVA: 0x000837F0 File Offset: 0x000819F0
	public int pixelHeight
	{
		get
		{
			return Mathf.RoundToInt((float)SerializableCamera.screenHeight * this.clampedRect.height);
		}
	}

	// Token: 0x1700042D RID: 1069
	// (get) Token: 0x0600121E RID: 4638 RVA: 0x00083817 File Offset: 0x00081A17
	private static int screenWidth
	{
		get
		{
			return Screen.width;
		}
	}

	// Token: 0x1700042E RID: 1070
	// (get) Token: 0x0600121F RID: 4639 RVA: 0x0008381E File Offset: 0x00081A1E
	private static int screenHeight
	{
		get
		{
			return Screen.height;
		}
	}

	// Token: 0x1700042F RID: 1071
	// (get) Token: 0x06001220 RID: 4640 RVA: 0x00083825 File Offset: 0x00081A25
	public Matrix4x4 cameraToWorldMatrix
	{
		get
		{
			return Matrix4x4.TRS(this.position, this.rotation, new Vector3(1f, 1f, -1f));
		}
	}

	// Token: 0x17000430 RID: 1072
	// (get) Token: 0x06001221 RID: 4641 RVA: 0x0008384C File Offset: 0x00081A4C
	public Matrix4x4 worldToCameraMatrix
	{
		get
		{
			return this.cameraToWorldMatrix.inverse;
		}
	}

	// Token: 0x17000431 RID: 1073
	// (get) Token: 0x06001222 RID: 4642 RVA: 0x00083868 File Offset: 0x00081A68
	public Matrix4x4 worldToCameraViewportMatrix
	{
		get
		{
			return Matrix4x4.TRS(new Vector3(0.5f, 0.5f, 0f), Quaternion.identity, new Vector3(0.5f, 0.5f, 1f)) * this.projectionMatrix * this.worldToCameraMatrix;
		}
	}

	// Token: 0x17000432 RID: 1074
	// (get) Token: 0x06001223 RID: 4643 RVA: 0x000838BD File Offset: 0x00081ABD
	public Matrix4x4 worldToCameraPixelRectMatrix
	{
		get
		{
			return Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3((float)SerializableCamera.screenWidth, (float)SerializableCamera.screenHeight, 1f)) * this.worldToCameraViewportMatrix;
		}
	}

	// Token: 0x17000433 RID: 1075
	// (get) Token: 0x06001224 RID: 4644 RVA: 0x000838F0 File Offset: 0x00081AF0
	public Matrix4x4 worldToScreenViewportMatrix
	{
		get
		{
			return Matrix4x4.TRS(new Vector3(this.clampedRect.x, this.clampedRect.y, 0f), Quaternion.identity, new Vector3(this.clampedRect.width, this.clampedRect.height, 1f)) * this.worldToCameraViewportMatrix;
		}
	}

	// Token: 0x17000434 RID: 1076
	// (get) Token: 0x06001225 RID: 4645 RVA: 0x0008395E File Offset: 0x00081B5E
	public Matrix4x4 worldToScreenMatrix
	{
		get
		{
			return Matrix4x4.TRS(Vector3.one, Quaternion.identity, new Vector3((float)SerializableCamera.screenWidth, (float)SerializableCamera.screenHeight, 1f)) * this.worldToScreenViewportMatrix;
		}
	}

	// Token: 0x17000435 RID: 1077
	// (get) Token: 0x06001226 RID: 4646 RVA: 0x00083990 File Offset: 0x00081B90
	public static SerializableCamera identity
	{
		get
		{
			return new SerializableCamera
			{
				transform = SerializableTransform.identity,
				fieldOfView = 60f,
				nearClipPlane = 0.01f,
				farClipPlane = 1000f,
				orthographic = false,
				orthographicSize = 10f,
				rect = SerializableCamera.defaultRect
			};
		}
	}

	// Token: 0x06001227 RID: 4647 RVA: 0x000839F8 File Offset: 0x00081BF8
	public SerializableCamera(SerializableTransform transform)
	{
		this.transform = transform;
		this._fieldOfView = 60f;
		this._nearClipPlane = 0.01f;
		this._farClipPlane = 1000f;
		this._orthographic = false;
		this._orthographicSize = 10f;
		this._rect = SerializableCamera.defaultRect;
		this._projectionMatrix = Matrix4x4.identity;
		this._projectionMatrixSet = false;
		this._inverseProjectionMatrix = Matrix4x4.identity;
		this._inverseProjectionMatrixSet = false;
	}

	// Token: 0x06001228 RID: 4648 RVA: 0x00083A70 File Offset: 0x00081C70
	public SerializableCamera(Camera camera)
	{
		this.transform = new SerializableTransform(camera.transform);
		this._fieldOfView = camera.fieldOfView;
		this._nearClipPlane = camera.nearClipPlane;
		this._farClipPlane = camera.farClipPlane;
		this._orthographic = camera.orthographic;
		this._orthographicSize = camera.orthographicSize;
		this._rect = camera.rect;
		this._projectionMatrix = Matrix4x4.identity;
		this._projectionMatrixSet = false;
		this._inverseProjectionMatrix = Matrix4x4.identity;
		this._inverseProjectionMatrixSet = false;
	}

	// Token: 0x06001229 RID: 4649 RVA: 0x00083AFC File Offset: 0x00081CFC
	public SerializableCamera(Vector3 position, Quaternion rotation, float fieldOfView, float aspectRatio)
	{
		this.transform = new SerializableTransform(position, rotation);
		this._fieldOfView = fieldOfView;
		this._nearClipPlane = 0.01f;
		this._farClipPlane = 1000f;
		this._orthographic = false;
		this._orthographicSize = 10f;
		this._rect = SerializableCamera.defaultRect;
		this._projectionMatrix = Matrix4x4.identity;
		this._projectionMatrixSet = false;
		this._inverseProjectionMatrix = Matrix4x4.identity;
		this._inverseProjectionMatrixSet = false;
	}

	// Token: 0x0600122A RID: 4650 RVA: 0x00083B74 File Offset: 0x00081D74
	public SerializableCamera(Vector3 position, Quaternion rotation, float fieldOfView, float aspectRatio, float nearClipPlane, float farClipPlane)
	{
		this.transform = new SerializableTransform(position, rotation);
		this._fieldOfView = fieldOfView;
		this._nearClipPlane = nearClipPlane;
		this._farClipPlane = farClipPlane;
		this._orthographic = false;
		this._orthographicSize = 10f;
		this._rect = SerializableCamera.defaultRect;
		this._projectionMatrix = Matrix4x4.identity;
		this._projectionMatrixSet = false;
		this._inverseProjectionMatrix = Matrix4x4.identity;
		this._inverseProjectionMatrixSet = false;
	}

	// Token: 0x0600122B RID: 4651 RVA: 0x00083BE8 File Offset: 0x00081DE8
	public void ApplyTo(Camera camera)
	{
		camera.transform.position = this.position;
		camera.transform.rotation = this.rotation;
		camera.fieldOfView = this.fieldOfView;
		camera.nearClipPlane = this.nearClipPlane;
		camera.farClipPlane = this.farClipPlane;
		camera.orthographic = this.orthographic;
		camera.orthographicSize = this.orthographicSize;
		camera.rect = this.rect;
	}

	// Token: 0x0600122C RID: 4652 RVA: 0x00083C60 File Offset: 0x00081E60
	public void ApplyFrom(Camera camera)
	{
		this.transform.ApplyFrom(camera.transform);
		this.fieldOfView = camera.fieldOfView;
		this.nearClipPlane = camera.nearClipPlane;
		this.farClipPlane = camera.farClipPlane;
		this.orthographic = camera.orthographic;
		this.orthographicSize = camera.orthographicSize;
		this.rect = camera.rect;
	}

	// Token: 0x0600122D RID: 4653 RVA: 0x00083CC8 File Offset: 0x00081EC8
	public void ApplyFrom(SerializableCamera camera)
	{
		this.transform.ApplyFrom(camera.transform);
		this.fieldOfView = camera.fieldOfView;
		this.nearClipPlane = camera.nearClipPlane;
		this.farClipPlane = camera.farClipPlane;
		this.orthographic = camera.orthographic;
		this.orthographicSize = camera.orthographicSize;
		this.rect = camera.rect;
	}

	// Token: 0x0600122E RID: 4654 RVA: 0x00083D34 File Offset: 0x00081F34
	public Vector3 WorldToViewportPoint(Vector3 worldPoint)
	{
		Vector3 vector = (this.projectionMatrix * this.worldToCameraMatrix).MultiplyPoint(worldPoint);
		return new Vector3(0.5f + vector.x * 0.5f, 0.5f + vector.y * 0.5f, this.transform.worldToLocalDirectionMatrix.MultiplyPoint(worldPoint).z);
	}

	// Token: 0x0600122F RID: 4655 RVA: 0x00083DA0 File Offset: 0x00081FA0
	public Vector2 WorldToScreenPoint(Vector3 worldPoint)
	{
		Vector3 vector = this.WorldToViewportPoint(worldPoint);
		return this.ViewportToScreenPoint(vector);
	}

	// Token: 0x06001230 RID: 4656 RVA: 0x00083DC4 File Offset: 0x00081FC4
	public Vector3 ViewportToWorldPoint(Vector3 viewportPoint)
	{
		viewportPoint.x = (viewportPoint.x - 0.5f) * 2f;
		viewportPoint.y = (viewportPoint.y - 0.5f) * 2f;
		Vector3 vector = new Vector3(viewportPoint.x, viewportPoint.y, -1f);
		Vector3 vector2 = new Vector3(viewportPoint.x, viewportPoint.y, 1f);
		Vector3 vector3 = this.inverseProjectionMatrix.MultiplyPoint(vector);
		Vector3 vector4 = this.inverseProjectionMatrix.MultiplyPoint(vector2);
		vector3.z = -vector3.z;
		vector4.z = -vector4.z;
		Vector3 normalized = (vector4 - vector3).normalized;
		float num = Vector3.Dot(Vector3.forward, normalized);
		Vector3 vector5 = normalized * viewportPoint.z / num;
		return this.transform.localToWorldDirectionMatrix.MultiplyPoint(vector5);
	}

	// Token: 0x06001231 RID: 4657 RVA: 0x00083EC4 File Offset: 0x000820C4
	public Ray ViewportPointToRay(Vector3 viewportPoint)
	{
		viewportPoint.x = (viewportPoint.x - 0.5f) * 2f;
		viewportPoint.y = (viewportPoint.y - 0.5f) * 2f;
		Vector3 vector = new Vector3(viewportPoint.x, viewportPoint.y, -1f);
		Vector3 vector2 = new Vector3(viewportPoint.x, viewportPoint.y, 1f);
		Vector3 vector3 = this.inverseProjectionMatrix.MultiplyPoint(vector);
		Vector3 vector4 = this.inverseProjectionMatrix.MultiplyPoint(vector2);
		vector3.z = -vector3.z;
		vector4.z = -vector4.z;
		Vector3 normalized = (vector4 - vector3).normalized;
		return new Ray(this.transform.localToWorldDirectionMatrix.MultiplyPoint(vector3), this.transform.localToWorldDirectionMatrix.MultiplyVector(normalized));
	}

	// Token: 0x06001232 RID: 4658 RVA: 0x00083FB3 File Offset: 0x000821B3
	public Vector2 ScreenToWorldPoint(Vector3 screenPoint)
	{
		return this.ViewportToWorldPoint(Vector2.Scale(screenPoint, new Vector2(1f / (float)SerializableCamera.screenWidth, 1f / (float)SerializableCamera.screenHeight)));
	}

	// Token: 0x06001233 RID: 4659 RVA: 0x00083FED File Offset: 0x000821ED
	public Ray ScreenPointToRay(Vector3 screenPoint)
	{
		return this.ViewportPointToRay(Vector2.Scale(screenPoint, new Vector2(1f / (float)SerializableCamera.screenWidth, 1f / (float)SerializableCamera.screenHeight)));
	}

	// Token: 0x06001234 RID: 4660 RVA: 0x00084024 File Offset: 0x00082224
	public Vector3 ScreenToViewportPoint(Vector3 screenPoint)
	{
		float num = SerializableCamera.<ScreenToViewportPoint>g__InverseLerpUnclamped|86_0((float)SerializableCamera.screenWidth * this.clampedRect.xMin, (float)SerializableCamera.screenWidth * this.clampedRect.xMax, screenPoint.x);
		float num2 = SerializableCamera.<ScreenToViewportPoint>g__InverseLerpUnclamped|86_0((float)SerializableCamera.screenHeight * this.clampedRect.yMin, (float)SerializableCamera.screenHeight * this.clampedRect.yMax, screenPoint.y);
		return new Vector3(num, num2, screenPoint.z);
	}

	// Token: 0x06001235 RID: 4661 RVA: 0x000840A8 File Offset: 0x000822A8
	public Vector3 ViewportToScreenPoint(Vector3 viewportPoint)
	{
		float num = Mathf.LerpUnclamped((float)SerializableCamera.screenWidth * this.clampedRect.xMin, (float)SerializableCamera.screenWidth * this.clampedRect.xMax, viewportPoint.x);
		float num2 = Mathf.LerpUnclamped((float)SerializableCamera.screenHeight * this.clampedRect.yMin, (float)SerializableCamera.screenHeight * this.clampedRect.yMax, viewportPoint.y);
		return new Vector3(num, num2, viewportPoint.z);
	}

	// Token: 0x06001236 RID: 4662 RVA: 0x0008412C File Offset: 0x0008232C
	public Vector3[] WorldToViewportPoints(params Vector3[] input)
	{
		Vector3[] array = new Vector3[input.Length];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = this.WorldToViewportPoint(input[i]);
		}
		return array;
	}

	// Token: 0x06001237 RID: 4663 RVA: 0x00084168 File Offset: 0x00082368
	public Vector3[] WorldToViewportPoints(IList<Vector3> input)
	{
		Vector3[] array = new Vector3[input.Count];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = this.WorldToViewportPoint(input[i]);
		}
		return array;
	}

	// Token: 0x06001238 RID: 4664 RVA: 0x000841A4 File Offset: 0x000823A4
	public Vector2[] WorldToScreenPoints(params Vector3[] input)
	{
		Vector2[] array = new Vector2[input.Length];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = this.WorldToScreenPoint(input[i]);
		}
		return array;
	}

	// Token: 0x06001239 RID: 4665 RVA: 0x000841E0 File Offset: 0x000823E0
	public Vector2[] WorldToScreenPoints(IList<Vector3> input)
	{
		Vector2[] array = new Vector2[input.Count];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = this.WorldToScreenPoint(input[i]);
		}
		return array;
	}

	// Token: 0x0600123A RID: 4666 RVA: 0x0008421C File Offset: 0x0008241C
	public Vector3[] ViewportToWorldPoints(params Vector3[] input)
	{
		Vector3[] array = new Vector3[input.Length];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = this.ViewportToWorldPoint(input[i]);
		}
		return array;
	}

	// Token: 0x0600123B RID: 4667 RVA: 0x00084258 File Offset: 0x00082458
	public Vector3[] ViewportToWorldPoints(IList<Vector3> input)
	{
		Vector3[] array = new Vector3[input.Count];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = this.ViewportToWorldPoint(input[i]);
		}
		return array;
	}

	// Token: 0x0600123C RID: 4668 RVA: 0x00084294 File Offset: 0x00082494
	public Rect ViewportToScreenRect(Rect rect)
	{
		Vector3 vector = this.ViewportToScreenPoint(rect.min);
		Vector3 vector2 = this.ViewportToScreenPoint(rect.max);
		return Rect.MinMaxRect(vector.x, vector.y, vector2.x, vector2.y);
	}

	// Token: 0x0600123D RID: 4669 RVA: 0x000842E4 File Offset: 0x000824E4
	public Vector3 ViewportToScreenVector(Vector2 vector)
	{
		return this.ViewportToScreenPoint(vector) - this.ViewportToScreenPoint(Vector2.zero);
	}

	// Token: 0x0600123E RID: 4670 RVA: 0x00084307 File Offset: 0x00082507
	public Vector3 ViewportToWorldVector(Vector2 vector, float distance)
	{
		return this.ViewportToWorldPoint(new Vector3(0f, 0f, distance)) - this.ViewportToWorldPoint(new Vector3(vector.x, vector.y, distance));
	}

	// Token: 0x0600123F RID: 4671 RVA: 0x0008433C File Offset: 0x0008253C
	public Rect ScreenToViewportRect(Rect rect)
	{
		Vector3 vector = this.ScreenToViewportPoint(rect.min);
		Vector3 vector2 = this.ScreenToViewportPoint(rect.max);
		return Rect.MinMaxRect(vector.x, vector.y, vector2.x, vector2.y);
	}

	// Token: 0x06001240 RID: 4672 RVA: 0x0008438C File Offset: 0x0008258C
	public Vector3 ScreenToWorldVector(Vector2 vector, float distance)
	{
		return this.ScreenToWorldPoint(new Vector3(0f, 0f, distance)) - this.ScreenToWorldPoint(new Vector3(vector.x, vector.y, distance));
	}

	// Token: 0x06001241 RID: 4673 RVA: 0x000843C6 File Offset: 0x000825C6
	public Vector3 ScreenToViewportVector(Vector2 vector)
	{
		return this.ScreenToViewportPoint(vector) - this.ScreenToViewportPoint(Vector2.zero);
	}

	// Token: 0x06001242 RID: 4674 RVA: 0x000843EC File Offset: 0x000825EC
	public Vector2[] ScreenToViewportPoints(Vector2[] screenPoints)
	{
		Vector2[] array = new Vector2[screenPoints.Length];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = this.ScreenToViewportPoint(screenPoints[i]);
		}
		return array;
	}

	// Token: 0x06001243 RID: 4675 RVA: 0x0008442F File Offset: 0x0008262F
	public float GetHorizontalFieldOfView()
	{
		return CameraX.GetHorizontalFieldOfView(this.fieldOfView, this.aspect);
	}

	// Token: 0x06001244 RID: 4676 RVA: 0x00084442 File Offset: 0x00082642
	public float GetFrustrumHeightAtDistance(float distance)
	{
		if (this.orthographic)
		{
			return this.orthographicSize;
		}
		return CameraX.GetFrustrumHeightAtDistance(distance, this.fieldOfView);
	}

	// Token: 0x06001245 RID: 4677 RVA: 0x0008445F File Offset: 0x0008265F
	public float GetFrustrumWidthAtDistance(float distance)
	{
		if (this.orthographic)
		{
			return this.aspect * this.orthographicSize;
		}
		return CameraX.GetFrustrumWidthAtDistance(distance, this.fieldOfView, this.aspect);
	}

	// Token: 0x06001246 RID: 4678 RVA: 0x00084489 File Offset: 0x00082689
	public float GetDistanceAtFrustrumHeight(float frustumHeight)
	{
		return CameraX.GetDistanceAtFrustrumHeight(frustumHeight, this.fieldOfView);
	}

	// Token: 0x06001247 RID: 4679 RVA: 0x00084497 File Offset: 0x00082697
	public float GetDistanceAtFrustrumWidth(float frustumWidth)
	{
		return CameraX.GetDistanceAtFrustrumWidth(frustumWidth, this.fieldOfView, this.aspect);
	}

	// Token: 0x06001248 RID: 4680 RVA: 0x000844AB File Offset: 0x000826AB
	public float GetFOVAngleAtWidthAndDistance(float frustumWidth, float distance)
	{
		return CameraX.GetFOVAngleAtWidthAndDistance(frustumWidth, distance, this.aspect);
	}

	// Token: 0x06001249 RID: 4681 RVA: 0x000844BA File Offset: 0x000826BA
	public float ConvertFrustumWidthToFrustumHeight(float frustumWidth)
	{
		return CameraX.ConvertFrustumWidthToFrustumHeight(frustumWidth, this.aspect);
	}

	// Token: 0x0600124A RID: 4682 RVA: 0x000844C8 File Offset: 0x000826C8
	public float ConvertFrustumHeightToFrustumWidth(float frustumHeight)
	{
		return CameraX.ConvertFrustumHeightToFrustumWidth(frustumHeight, this.aspect);
	}

	// Token: 0x0600124B RID: 4683 RVA: 0x000844D8 File Offset: 0x000826D8
	public void DrawGizmos()
	{
		Matrix4x4 matrix = Gizmos.matrix;
		Gizmos.matrix = Matrix4x4.TRS(this.position, this.rotation, Vector3.one);
		if (this.orthographic)
		{
			float num = this.farClipPlane - this.nearClipPlane;
			float num2 = (this.farClipPlane + this.nearClipPlane) * 0.5f;
			Gizmos.DrawWireCube(new Vector3(0f, 0f, num2), new Vector3(this.orthographicSize * 2f * this.aspect, this.orthographicSize * 2f, num));
		}
		else
		{
			Gizmos.DrawFrustum(Vector3.zero, this.fieldOfView, this.farClipPlane, this.nearClipPlane, this.aspect);
		}
		Gizmos.matrix = matrix;
	}

	// Token: 0x0600124C RID: 4684 RVA: 0x00084594 File Offset: 0x00082794
	public override string ToString()
	{
		return string.Format("[SerializableCamera: position={0}, rotation={1}, field of view={2}, aspect={3}, near={4}, far={5}, rect={6}, orthSize={7}]", new object[] { this.position, this.rotation, this.fieldOfView, this.aspect, this.nearClipPlane, this.farClipPlane, this.rect, this.orthographicSize });
	}

	// Token: 0x0600124E RID: 4686 RVA: 0x00084641 File Offset: 0x00082841
	[CompilerGenerated]
	internal static float <ScreenToViewportPoint>g__InverseLerpUnclamped|86_0(float from, float to, float value)
	{
		return (value - from) / (to - from);
	}

	// Token: 0x0400127C RID: 4732
	public const bool defaultOrthographic = false;

	// Token: 0x0400127D RID: 4733
	public const float defaultFieldOfView = 60f;

	// Token: 0x0400127E RID: 4734
	public const float defaultOrthographicSize = 10f;

	// Token: 0x0400127F RID: 4735
	public const float defaultAspectRatio = 1f;

	// Token: 0x04001280 RID: 4736
	public const float defaultNearClipPlane = 0.01f;

	// Token: 0x04001281 RID: 4737
	public const float defaultFarClipPlane = 1000f;

	// Token: 0x04001282 RID: 4738
	public static Rect defaultRect = new Rect(0f, 0f, 1f, 1f);

	// Token: 0x04001283 RID: 4739
	public SerializableTransform transform;

	// Token: 0x04001284 RID: 4740
	private bool _projectionMatrixSet;

	// Token: 0x04001285 RID: 4741
	private Matrix4x4 _projectionMatrix;

	// Token: 0x04001286 RID: 4742
	private bool _inverseProjectionMatrixSet;

	// Token: 0x04001287 RID: 4743
	private Matrix4x4 _inverseProjectionMatrix;

	// Token: 0x04001288 RID: 4744
	[SerializeField]
	private bool _orthographic;

	// Token: 0x04001289 RID: 4745
	[SerializeField]
	private float _orthographicSize;

	// Token: 0x0400128A RID: 4746
	[SerializeField]
	private float _fieldOfView;

	// Token: 0x0400128B RID: 4747
	[SerializeField]
	private float _nearClipPlane;

	// Token: 0x0400128C RID: 4748
	[SerializeField]
	private float _farClipPlane;

	// Token: 0x0400128D RID: 4749
	private Rect _rect;
}
