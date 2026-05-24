using System;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x020001DD RID: 477
[ExecuteInEditMode]
public class WorldSpaceUIElement : UIBehaviour
{
	// Token: 0x170003F4 RID: 1012
	// (get) Token: 0x06001085 RID: 4229 RVA: 0x0007A96C File Offset: 0x00078B6C
	// (set) Token: 0x06001086 RID: 4230 RVA: 0x0007A9C8 File Offset: 0x00078BC8
	public Camera worldCamera
	{
		get
		{
			if (this._worldCamera == null)
			{
				this._worldCamera = Camera.main;
				Debug.Log("No camera specified. Setting to current value of Camera.main: " + ((this._worldCamera == null) ? "Null" : this._worldCamera.name), this);
			}
			return this._worldCamera;
		}
		set
		{
			if (this._worldCamera == value)
			{
				return;
			}
			this._worldCamera = value;
			this.Refresh();
		}
	}

	// Token: 0x170003F5 RID: 1013
	// (get) Token: 0x06001087 RID: 4231 RVA: 0x0007A9E6 File Offset: 0x00078BE6
	// (set) Token: 0x06001088 RID: 4232 RVA: 0x0007A9EE File Offset: 0x00078BEE
	public Transform target
	{
		get
		{
			return this._target;
		}
		set
		{
			if (this._target == value)
			{
				return;
			}
			this._target = value;
			this.Refresh();
		}
	}

	// Token: 0x170003F6 RID: 1014
	// (get) Token: 0x06001089 RID: 4233 RVA: 0x0007AA0C File Offset: 0x00078C0C
	private Vector3 targetPositionInternal
	{
		get
		{
			if (!(this.target == null))
			{
				return this.target.position;
			}
			return this.worldPosition;
		}
	}

	// Token: 0x170003F7 RID: 1015
	// (get) Token: 0x0600108A RID: 4234 RVA: 0x0007AA2E File Offset: 0x00078C2E
	private Quaternion targetRotationInternal
	{
		get
		{
			if (!(this.target == null))
			{
				return this.target.rotation;
			}
			return this.worldRotation;
		}
	}

	// Token: 0x170003F8 RID: 1016
	// (get) Token: 0x0600108B RID: 4235 RVA: 0x0007AA50 File Offset: 0x00078C50
	public RectTransform rectTransform
	{
		get
		{
			if (!this._rectTransformSet)
			{
				if (base.transform is RectTransform)
				{
					this._rectTransform = base.transform as RectTransform;
					this._rectTransformSet = true;
				}
				else
				{
					Debug.LogWarning(base.gameObject.name + " is not a rect transform!", this);
				}
			}
			return this._rectTransform;
		}
	}

	// Token: 0x170003F9 RID: 1017
	// (get) Token: 0x0600108C RID: 4236 RVA: 0x0007AAAD File Offset: 0x00078CAD
	public Canvas rootCanvas
	{
		get
		{
			if (!this._rootCanvasSet)
			{
				this.SetRootCanvas();
			}
			return this._rootCanvas;
		}
	}

	// Token: 0x170003FA RID: 1018
	// (get) Token: 0x0600108D RID: 4237 RVA: 0x0007AAC3 File Offset: 0x00078CC3
	public RectTransform rootCanvasRT
	{
		get
		{
			if (!this._rootCanvasSet)
			{
				this.SetRootCanvas();
			}
			return this._rootCanvasRT;
		}
	}

	// Token: 0x170003FB RID: 1019
	// (get) Token: 0x0600108E RID: 4238 RVA: 0x0007AADC File Offset: 0x00078CDC
	private RectTransform parentRT
	{
		get
		{
			Transform parent = base.transform.parent;
			if (!(parent != null))
			{
				return this.rectTransform;
			}
			if (parent is RectTransform)
			{
				return (RectTransform)parent;
			}
			Debug.LogWarning("Parent of " + base.gameObject.name + " is not a rect transform!", this);
			return null;
		}
	}

	// Token: 0x0600108F RID: 4239 RVA: 0x0007AB38 File Offset: 0x00078D38
	private void SetRootCanvas()
	{
		Canvas componentInParent = base.transform.GetComponentInParent<Canvas>();
		this._rootCanvas = componentInParent.rootCanvas;
		this._rootCanvasRT = this._rootCanvas.GetComponent<RectTransform>();
		this._rootCanvasSet = true;
	}

	// Token: 0x06001090 RID: 4240 RVA: 0x0007AB78 File Offset: 0x00078D78
	public Vector3 GetLocalScale()
	{
		if (this.rootCanvas.renderMode == RenderMode.WorldSpace && this.worldCamera.orthographic)
		{
			float num = this.worldCamera.orthographicSize;
			num *= this.scaleMultiplier;
			return Vector3.one * Mathf.Clamp(num, this.minScale, this.maxScale);
		}
		return Vector3.one * WorldSpaceUIElement.GetClampedViewportScale(this.worldCamera, this.targetPositionInternal, this.scaleMultiplier, this.minScale, this.maxScale);
	}

	// Token: 0x06001091 RID: 4241 RVA: 0x0007AC00 File Offset: 0x00078E00
	public static float GetClampedViewportScale(Camera camera, Vector3 targetPoint, float targetScale, float minViewportScale, float maxViewportScale)
	{
		float num;
		if (camera.orthographic)
		{
			num = Mathf.Abs(Vector3.Dot(camera.transform.position - targetPoint, camera.transform.forward));
		}
		else
		{
			num = Vector3.Distance(targetPoint, camera.transform.position);
		}
		float num2 = camera.ViewportToWorldPoint(new Vector3(0f, 1f, num)).y - camera.ViewportToWorldPoint(new Vector3(1f, 0f, num)).y;
		return Mathf.Clamp(1f / num2 * targetScale, minViewportScale, maxViewportScale);
	}

	// Token: 0x06001092 RID: 4242 RVA: 0x0007ACA0 File Offset: 0x00078EA0
	protected override void Awake()
	{
		this._rectTransform = base.transform as RectTransform;
		this._rectTransformSet = this._rectTransform != null;
		if (this.worldCamera == null)
		{
			this.worldCamera = Camera.main;
		}
		this.SetRootCanvas();
	}

	// Token: 0x06001093 RID: 4243 RVA: 0x0007ACEF File Offset: 0x00078EEF
	protected override void OnEnable()
	{
		this.Refresh();
	}

	// Token: 0x06001094 RID: 4244 RVA: 0x0007ACF7 File Offset: 0x00078EF7
	protected override void OnTransformParentChanged()
	{
		this.SetRootCanvas();
		base.OnTransformParentChanged();
	}

	// Token: 0x06001095 RID: 4245 RVA: 0x0007AD05 File Offset: 0x00078F05
	private void LateUpdate()
	{
		this.Refresh();
	}

	// Token: 0x06001096 RID: 4246 RVA: 0x0007AD10 File Offset: 0x00078F10
	public void Refresh()
	{
		if (this.worldCamera == null || this.rootCanvas == null)
		{
			return;
		}
		if (this.updateScale)
		{
			this.ScaleFromDistance();
		}
		if (this.updatePosition)
		{
			this.SetPositionFromWorldPosition();
		}
		if (this.updateRotation == WorldSpaceUIElement.RotationMode.Rotation)
		{
			this.SetAngleFromRotation();
		}
		else if (this.updateRotation == WorldSpaceUIElement.RotationMode.RotationZ)
		{
			this.SetAngleFromRotationZ();
		}
		if (this.updateOcclusion)
		{
			this.CheckOcclusion();
		}
	}

	// Token: 0x06001097 RID: 4247 RVA: 0x0007AD84 File Offset: 0x00078F84
	public void ScaleFromDistance()
	{
		this.rectTransform.localScale = this.GetLocalScale();
	}

	// Token: 0x06001098 RID: 4248 RVA: 0x0007AD98 File Offset: 0x00078F98
	private bool TryProjectWorldPoint(Vector3 worldPosition, out Vector3 projectedCanvasPosition)
	{
		RectTransform parentRT = this.parentRT;
		if (parentRT == null)
		{
			projectedCanvasPosition = Vector3.zero;
			return false;
		}
		Vector3? vector = WorldSpaceUIElement.WorldPointToLocalPointInRectangle(this.rootCanvas, this.worldCamera, parentRT, worldPosition);
		if (vector == null)
		{
			projectedCanvasPosition = Vector3.zero;
			return false;
		}
		projectedCanvasPosition = vector.Value;
		if (this.rootCanvas.renderMode == RenderMode.WorldSpace)
		{
			projectedCanvasPosition = base.transform.parent.InverseTransformPoint(worldPosition);
			projectedCanvasPosition.z = 0f;
		}
		return true;
	}

	// Token: 0x06001099 RID: 4249 RVA: 0x0007AE2C File Offset: 0x0007902C
	public void SetPositionFromWorldPosition()
	{
		Vector3 vector;
		if (!this.TryProjectWorldPoint(this.targetPositionInternal, out vector))
		{
			this.onScreen = false;
			return;
		}
		this.rectTransform.localPosition = vector;
		if (this.clampToScreen)
		{
			Rect screenRect = WorldSpaceUIElement.GetScreenRect(this.rectTransform, this.rootCanvas);
			Rect screenRect2 = WorldSpaceUIElement.GetScreenRect(this.rootCanvasRT, this.rootCanvas);
			Rect rect = WorldSpaceUIElement.ClampInsideKeepSize(screenRect, screenRect2);
			Vector3? vector2 = WorldSpaceUIElement.ScreenPointToCanvasSpace(this.rootCanvas, rect.position + screenRect.size * 0.5f);
			if (vector2 != null)
			{
				this.rectTransform.position = vector2.Value;
			}
		}
		this.onScreen = this.rootCanvasRT.rect.Contains(vector);
	}

	// Token: 0x0600109A RID: 4250 RVA: 0x0007AEF6 File Offset: 0x000790F6
	public void SetAngleFromRotation()
	{
		this.rectTransform.rotation = Quaternion.Inverse(this.worldCamera.transform.rotation) * this.targetRotationInternal;
	}

	// Token: 0x0600109B RID: 4251 RVA: 0x0007AF24 File Offset: 0x00079124
	public void SetAngleFromRotationZ()
	{
		Vector3 vector;
		if (!this.TryProjectWorldPoint(this.targetPositionInternal, out vector))
		{
			return;
		}
		Vector3 vector2;
		if (!this.TryProjectWorldPoint(this.targetPositionInternal + this.worldPointingVectorForZRotation, out vector2))
		{
			this.onScreen = false;
			return;
		}
		float num = Vector2.SignedAngle(vector2 - vector, Vector2.up);
		this.rectTransform.localRotation = Quaternion.Euler(0f, 0f, -num);
	}

	// Token: 0x0600109C RID: 4252 RVA: 0x0007AF98 File Offset: 0x00079198
	private void CheckOcclusion()
	{
		Vector3 vector = this.targetPositionInternal - this.worldCamera.transform.position;
		float magnitude = vector.magnitude;
		this.occluded = false;
		if (magnitude > 0f)
		{
			Vector3 vector2 = vector / magnitude;
			if (this.occlusionMask != 0)
			{
				RaycastHit raycastHit;
				this.occluded = Physics.Raycast(this.worldCamera.transform.position, vector2, out raycastHit, magnitude, this.occlusionMask);
			}
		}
	}

	// Token: 0x0600109D RID: 4253 RVA: 0x0007B010 File Offset: 0x00079210
	private void _OnDrawGizmos()
	{
		if (WorldSpaceUIElement.WorldPointToLocalPointInRectangle(this.rootCanvas, this.worldCamera, this.targetPositionInternal) == null)
		{
			return;
		}
		Rect screenRect = WorldSpaceUIElement.GetScreenRect(this.rectTransform, this.rootCanvas);
		Rect screenRect2 = WorldSpaceUIElement.GetScreenRect(this.rootCanvasRT, this.rootCanvas);
		Gizmos.color = Color.yellow;
		Gizmos.DrawWireCube(screenRect.center, screenRect.size);
		Gizmos.color = Color.blue;
		Gizmos.DrawWireCube(screenRect2.center, screenRect2.size);
		Gizmos.color = Color.red;
		Rect rect = WorldSpaceUIElement.ClampInsideKeepSize(screenRect, screenRect2);
		Gizmos.DrawWireCube(rect.center, rect.size);
	}

	// Token: 0x0600109E RID: 4254 RVA: 0x0007B0E0 File Offset: 0x000792E0
	private static void GetScreenCorners(RectTransform rectTransform, Canvas canvas, Vector3[] fourCornersArray)
	{
		rectTransform.GetWorldCorners(WorldSpaceUIElement.corners);
		for (int i = 0; i < 4; i++)
		{
			Camera camera = null;
			if (canvas.renderMode == RenderMode.ScreenSpaceCamera || canvas.renderMode == RenderMode.WorldSpace)
			{
				camera = canvas.worldCamera;
			}
			Vector3 vector = RectTransformUtility.WorldToScreenPoint(camera, WorldSpaceUIElement.corners[i]);
			fourCornersArray[i] = vector;
		}
	}

	// Token: 0x0600109F RID: 4255 RVA: 0x0007B140 File Offset: 0x00079340
	private static Rect GetScreenRect(RectTransform rectTransform, Canvas canvas)
	{
		WorldSpaceUIElement.GetScreenCorners(rectTransform, canvas, WorldSpaceUIElement.corners);
		float num = float.PositiveInfinity;
		float num2 = float.NegativeInfinity;
		float num3 = float.PositiveInfinity;
		float num4 = float.NegativeInfinity;
		for (int i = 0; i < 4; i++)
		{
			Vector3 vector = WorldSpaceUIElement.corners[i];
			if (vector.x < num)
			{
				num = vector.x;
			}
			if (vector.x > num2)
			{
				num2 = vector.x;
			}
			if (vector.y < num3)
			{
				num3 = vector.y;
			}
			if (vector.y > num4)
			{
				num4 = vector.y;
			}
		}
		return new Rect(num, num3, num2 - num, num4 - num3);
	}

	// Token: 0x060010A0 RID: 4256 RVA: 0x0007B1E4 File Offset: 0x000793E4
	public static Rect ClampInsideKeepSize(Rect r, Rect container)
	{
		Rect zero = Rect.zero;
		zero.xMin = Mathf.Max(r.xMin, container.xMin);
		zero.xMax = Mathf.Min(r.xMax, container.xMax);
		zero.yMin = Mathf.Max(r.yMin, container.yMin);
		zero.yMax = Mathf.Min(r.yMax, container.yMax);
		if (r.xMin < container.xMin)
		{
			zero.width += container.xMin - r.xMin;
		}
		if (r.yMin < container.yMin)
		{
			zero.height += container.yMin - r.yMin;
		}
		if (r.xMax > container.xMax)
		{
			zero.x -= r.xMax - container.xMax;
			zero.width += r.xMax - container.xMax;
		}
		if (r.yMax > container.yMax)
		{
			zero.y -= r.yMax - container.yMax;
			zero.height += r.yMax - container.yMax;
		}
		zero.xMin = Mathf.Max(zero.xMin, container.xMin);
		zero.xMax = Mathf.Min(zero.xMax, container.xMax);
		zero.yMin = Mathf.Max(zero.yMin, container.yMin);
		zero.yMax = Mathf.Min(zero.yMax, container.yMax);
		return zero;
	}

	// Token: 0x060010A1 RID: 4257 RVA: 0x0007B3B8 File Offset: 0x000795B8
	private static Vector3? WorldPointToLocalPointInRectangle(Canvas canvas, Camera camera, RectTransform rectTransform, Vector3 worldPosition)
	{
		Vector3 vector = camera.WorldToScreenPoint(worldPosition);
		if (vector.z < 0f)
		{
			return null;
		}
		return WorldSpaceUIElement.ScreenPointToLocalPointInRectangle(canvas, rectTransform, vector);
	}

	// Token: 0x060010A2 RID: 4258 RVA: 0x0007B3F1 File Offset: 0x000795F1
	private static Vector3? WorldPointToLocalPointInRectangle(Canvas canvas, Camera camera, Vector3 worldPosition)
	{
		return WorldSpaceUIElement.WorldPointToLocalPointInRectangle(canvas, camera, canvas.GetComponent<RectTransform>(), worldPosition);
	}

	// Token: 0x060010A3 RID: 4259 RVA: 0x0007B404 File Offset: 0x00079604
	private static Vector3? ScreenPointToLocalPointInRectangle(Canvas canvas, RectTransform rectTransform, Vector2 screenPoint)
	{
		Camera camera = ((canvas.renderMode == RenderMode.ScreenSpaceCamera) ? canvas.worldCamera : null);
		Vector2 vector;
		if (RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, screenPoint, camera, out vector))
		{
			return new Vector3?(vector);
		}
		return null;
	}

	// Token: 0x060010A4 RID: 4260 RVA: 0x0007B448 File Offset: 0x00079648
	private static Vector3? ScreenPointToCanvasSpace(Canvas canvas, Vector2 screenPoint)
	{
		Camera camera = ((canvas.renderMode == RenderMode.ScreenSpaceCamera) ? canvas.worldCamera : null);
		Vector3 zero = Vector3.zero;
		if (RectTransformUtility.ScreenPointToWorldPointInRectangle(canvas.GetComponent<RectTransform>(), screenPoint, camera, out zero))
		{
			return new Vector3?(zero);
		}
		return null;
	}

	// Token: 0x0400123C RID: 4668
	[SerializeField]
	private bool _updateInEditMode = true;

	// Token: 0x0400123D RID: 4669
	[SerializeField]
	private Camera _worldCamera;

	// Token: 0x0400123E RID: 4670
	[SerializeField]
	private Transform _target;

	// Token: 0x0400123F RID: 4671
	public Vector3 worldPosition = Vector3.zero;

	// Token: 0x04001240 RID: 4672
	public Quaternion worldRotation = Quaternion.identity;

	// Token: 0x04001241 RID: 4673
	public bool updatePosition = true;

	// Token: 0x04001242 RID: 4674
	public WorldSpaceUIElement.RotationMode updateRotation;

	// Token: 0x04001243 RID: 4675
	public Vector3 worldPointingVectorForZRotation = new Vector3(0f, 0f, 1f);

	// Token: 0x04001244 RID: 4676
	public bool updateScale;

	// Token: 0x04001245 RID: 4677
	public float scaleMultiplier = 1f;

	// Token: 0x04001246 RID: 4678
	public float minScale = 0.2f;

	// Token: 0x04001247 RID: 4679
	public float maxScale = 1f;

	// Token: 0x04001248 RID: 4680
	public bool clampToScreen;

	// Token: 0x04001249 RID: 4681
	public bool onScreen;

	// Token: 0x0400124A RID: 4682
	public bool updateOcclusion;

	// Token: 0x0400124B RID: 4683
	public bool occluded;

	// Token: 0x0400124C RID: 4684
	public int occlusionMask = -5;

	// Token: 0x0400124D RID: 4685
	private bool _rectTransformSet;

	// Token: 0x0400124E RID: 4686
	private RectTransform _rectTransform;

	// Token: 0x0400124F RID: 4687
	private bool _rootCanvasSet;

	// Token: 0x04001250 RID: 4688
	private Canvas _rootCanvas;

	// Token: 0x04001251 RID: 4689
	private RectTransform _rootCanvasRT;

	// Token: 0x04001252 RID: 4690
	private static Vector3[] corners = new Vector3[4];

	// Token: 0x020003EF RID: 1007
	public enum RotationMode
	{
		// Token: 0x04001A64 RID: 6756
		None,
		// Token: 0x04001A65 RID: 6757
		Rotation,
		// Token: 0x04001A66 RID: 6758
		RotationZ
	}
}
