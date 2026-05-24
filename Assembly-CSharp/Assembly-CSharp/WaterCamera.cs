using System;
using UnityEngine;

// Token: 0x0200005D RID: 93
[RequireComponent(typeof(Camera))]
public class WaterCamera : MonoSingleton<WaterCamera>
{
	// Token: 0x170000BD RID: 189
	// (get) Token: 0x06000280 RID: 640 RVA: 0x0001540F File Offset: 0x0001360F
	private GameCamera gameCamera
	{
		get
		{
			return GameCamera.instance;
		}
	}

	// Token: 0x06000281 RID: 641 RVA: 0x00015416 File Offset: 0x00013616
	private void OnEnable()
	{
		this.camera = base.GetComponent<Camera>();
	}

	// Token: 0x06000282 RID: 642 RVA: 0x00015424 File Offset: 0x00013624
	private void OnDisable()
	{
		this.camera.ResetProjectionMatrix();
	}

	// Token: 0x06000283 RID: 643 RVA: 0x00015434 File Offset: 0x00013634
	private void LateUpdate()
	{
		this.camera.depthTextureMode = DepthTextureMode.Depth;
		if (this.renderTexture != null)
		{
			foreach (WaterPlane waterPlane in MonoInstancer<WaterPlane>.all)
			{
				waterPlane.SetHighQualityReflectionTex(this.renderTexture);
			}
		}
		this.SetFromGameCamera();
	}

	// Token: 0x06000284 RID: 644 RVA: 0x000154AC File Offset: 0x000136AC
	private Quaternion ReflectRotation(Quaternion source, Vector3 normal)
	{
		Quaternion quaternion = new Quaternion(normal.x, normal.y, normal.z, 0f);
		return quaternion * source * quaternion;
	}

	// Token: 0x06000285 RID: 645 RVA: 0x000154E4 File Offset: 0x000136E4
	private void SetFromGameCamera()
	{
		this.cameraProperties = this.gameCamera.cameraProperties;
		this.cameraProperties.SetPositionMaintainingDistance(new Vector3(this.cameraProperties.position.x, -this.cameraProperties.position.y, this.cameraProperties.position.z));
		this.cameraProperties.ApplyTo(this.camera, true);
		Matrix4x4 matrix4x = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3(-1f, -1f, 1f));
		this.camera.projectionMatrix = this.camera.projectionMatrix * matrix4x;
		base.transform.rotation = this.ReflectRotation(HighlandCameraProperties.rotation, Vector3.up);
		Matrix4x4 matrix4x2 = HighlandCameraProperties.CreateShearingMatrix(this.cameraProperties.shearFactor, this.cameraProperties.distance);
		Shader.SetGlobalMatrix("_CameraShear", matrix4x2);
		base.transform.position = this.cameraProperties.position;
		base.transform.rotation = this.ReflectRotation(HighlandCameraProperties.rotation, Vector3.up);
		this.camera.fieldOfView = this.gameCamera.cameraProperties.fieldOfView;
		this.material.SetMatrix("_ViewProjectInverse", (this.gameCamera.camera.projectionMatrix * this.gameCamera.camera.worldToCameraMatrix).inverse);
		this.camera.enabled = false;
		this.CreateRenderTextureIfNecessary();
		this.camera.targetTexture = this.renderTexture;
		this.camera.Render();
	}

	// Token: 0x06000286 RID: 646 RVA: 0x0001568E File Offset: 0x0001388E
	private void OnRenderImage(RenderTexture src, RenderTexture dest)
	{
		this.CreateRenderTextureIfNecessary();
		if (this.material != null)
		{
			Graphics.Blit(src, dest, this.material);
			return;
		}
		Graphics.Blit(src, dest);
	}

	// Token: 0x06000287 RID: 647 RVA: 0x000156BC File Offset: 0x000138BC
	private void CreateRenderTextureIfNecessary()
	{
		float resolutionScaleDesktop = this.settings.resolutionScaleDesktop;
		int num = Mathf.RoundToInt((float)Screen.width * resolutionScaleDesktop);
		int num2 = Mathf.RoundToInt((float)Screen.height * resolutionScaleDesktop);
		if (this.renderTexture == null || this.renderTexture.width != num || this.renderTexture.height != num2)
		{
			this.DestoryPrevFrameRenderTex();
			this.renderTexture = new RenderTexture(num, num2, 24, RenderTextureFormat.ARGB32);
		}
	}

	// Token: 0x06000288 RID: 648 RVA: 0x00015731 File Offset: 0x00013931
	private void DestoryPrevFrameRenderTex()
	{
		if (this.renderTexture == null)
		{
			return;
		}
		if (Application.isPlaying)
		{
			Object.Destroy(this.renderTexture);
		}
		else
		{
			Object.DestroyImmediate(this.renderTexture);
		}
		this.renderTexture = null;
	}

	// Token: 0x040003C6 RID: 966
	public WaterCameraSettings settings;

	// Token: 0x040003C7 RID: 967
	public Camera camera;

	// Token: 0x040003C8 RID: 968
	public RenderTexture renderTexture;

	// Token: 0x040003C9 RID: 969
	[SerializeField]
	private HighlandCameraProperties cameraProperties;

	// Token: 0x040003CA RID: 970
	public Material material;
}
