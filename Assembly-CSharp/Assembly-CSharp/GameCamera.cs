using System;
using System.Collections;
using System.Collections.Generic;
using SplineSystem;
using UnityEngine;

// Token: 0x0200001A RID: 26
[ExecuteInEditMode]
public class GameCamera : MonoBehaviour
{
	// Token: 0x17000022 RID: 34
	// (get) Token: 0x0600008A RID: 138 RVA: 0x00009168 File Offset: 0x00007368
	public static GameCamera instance
	{
		get
		{
			return GSR.GameCam;
		}
	}

	// Token: 0x17000023 RID: 35
	// (get) Token: 0x0600008B RID: 139 RVA: 0x0000916F File Offset: 0x0000736F
	// (set) Token: 0x0600008C RID: 140 RVA: 0x00009176 File Offset: 0x00007376
	public static float cameraZ
	{
		get
		{
			return GameCamera._cameraZ;
		}
		set
		{
			if (GameCamera._cameraZ == value)
			{
				return;
			}
			GameCamera._cameraZ = value;
			if (GameCamera.onChangeCameraZ != null)
			{
				GameCamera.onChangeCameraZ();
			}
		}
	}

	// Token: 0x17000024 RID: 36
	// (get) Token: 0x0600008D RID: 141 RVA: 0x00009198 File Offset: 0x00007398
	// (set) Token: 0x0600008E RID: 142 RVA: 0x000091A5 File Offset: 0x000073A5
	public Color backgroundColour
	{
		get
		{
			return this.camera.backgroundColor;
		}
		set
		{
			this.camera.backgroundColor = value;
			this.backgroundCamera.backgroundColor = value;
		}
	}

	// Token: 0x17000025 RID: 37
	// (get) Token: 0x0600008F RID: 143 RVA: 0x000091BF File Offset: 0x000073BF
	public WaterReflectionGrab waterReflectionGrab
	{
		get
		{
			if (this._waterReflectionGrab == null)
			{
				this._waterReflectionGrab = base.GetComponent<WaterReflectionGrab>();
			}
			return this._waterReflectionGrab;
		}
	}

	// Token: 0x17000026 RID: 38
	// (get) Token: 0x06000090 RID: 144 RVA: 0x000091E1 File Offset: 0x000073E1
	private BlurEffect blurEffect
	{
		get
		{
			if (this._blurEffect == null)
			{
				this._blurEffect = base.GetComponentInChildren<BlurEffect>();
			}
			return this._blurEffect;
		}
	}

	// Token: 0x17000027 RID: 39
	// (get) Token: 0x06000091 RID: 145 RVA: 0x00009203 File Offset: 0x00007403
	private Camera backgroundCamera
	{
		get
		{
			if (this._backgroundCamera == null)
			{
				this._backgroundCamera = this.blurEffect.GetComponent<Camera>();
			}
			return this._backgroundCamera;
		}
	}

	// Token: 0x17000028 RID: 40
	// (get) Token: 0x06000092 RID: 146 RVA: 0x0000922C File Offset: 0x0000742C
	private List<Camera> allSubCameras
	{
		get
		{
			if (!this._allSubCamerasSet || !Application.isPlaying)
			{
				this._allSubCameras.Clear();
				base.GetComponentsInChildren<Camera>(this._allSubCameras);
				this._allSubCameras.Remove(this.camera);
				this._allSubCamerasSet = true;
			}
			return this._allSubCameras;
		}
	}

	// Token: 0x17000029 RID: 41
	// (get) Token: 0x06000093 RID: 147 RVA: 0x0000927E File Offset: 0x0000747E
	public float fov
	{
		get
		{
			return this.camera.fieldOfView / this.cameraProperties.viewportScale;
		}
	}

	// Token: 0x06000094 RID: 148 RVA: 0x00009298 File Offset: 0x00007498
	private void OnEnable()
	{
		this.camera = base.GetComponent<Camera>();
		this.camera.transparencySortMode = TransparencySortMode.CustomAxis;
		this.camera.transparencySortAxis = Vector3.forward;
		foreach (Camera camera in this.allSubCameras)
		{
			camera.transparencySortMode = TransparencySortMode.CustomAxis;
			camera.transparencySortAxis = Vector3.forward;
		}
		this.cameraStates = new GameCamera.BaseCameraState[]
		{
			this.runningState, this.modifierState, this.inkModifierState, this.playerZoomState, this.mapConfirmState, this.followPathState, this.peakState, this.stoneSkimmingState, this.shelterState, this.caughtCameraState,
			this.cameraShakeState, this.pushVolumeState, this.animatedCameraState, this.introCameraState, this.freeCameraState
		};
		GameCamera.BaseCameraState[] array = this.cameraStates;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].Reset();
		}
		this.backgroundColour = this.camera.backgroundColor;
		this.highQualityWaterReflections = PlayerPrefsX.GetInt("highQualityWater", 1) == 1;
		this.RefreshWaterReflectionQuality();
		if (Application.isPlaying && MonoSingleton<Launcher>.instance != null)
		{
			MonoSingleton<Launcher>.instance.stupidCameraThatFixesCanvasResolution.gameObject.SetActive(false);
		}
	}

	// Token: 0x06000095 RID: 149 RVA: 0x00009428 File Offset: 0x00007628
	private void OnDisable()
	{
		if (Application.isPlaying && MonoSingleton<Launcher>.instance != null && MonoSingleton<Launcher>.instance.stupidCameraThatFixesCanvasResolution != null)
		{
			MonoSingleton<Launcher>.instance.stupidCameraThatFixesCanvasResolution.gameObject.SetActive(true);
		}
	}

	// Token: 0x06000096 RID: 150 RVA: 0x00009468 File Offset: 0x00007668
	public void Clear()
	{
		this.SetAnimationActive(null);
		GameCamera.BaseCameraState[] array = this.cameraStates;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].Reset();
		}
	}

	// Token: 0x06000097 RID: 151 RVA: 0x0000949C File Offset: 0x0000769C
	private void OnDrawGizmos()
	{
		if (this.cameraStates == null)
		{
			return;
		}
		GameCamera.BaseCameraState[] array = this.cameraStates;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].OnDrawGizmos();
		}
	}

	// Token: 0x06000098 RID: 152 RVA: 0x000094D0 File Offset: 0x000076D0
	public HighlandCameraProperties GetCameraProperties(bool immediate = false)
	{
		HighlandCameraProperties @default = HighlandCameraProperties.@default;
		if (!base.enabled)
		{
			return @default;
		}
		GameCamera.BaseCameraState[] array = this.cameraStates;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].Update(immediate, ref @default);
		}
		@default.distance *= this.debugDistanceScalar;
		@default.targetPoint = Vector3.Lerp(@default.targetPoint, this.runningState.cameraProperties.targetPoint, 1f - this.debugDistanceScalar);
		return @default;
	}

	// Token: 0x06000099 RID: 153 RVA: 0x00009550 File Offset: 0x00007750
	public void Refresh(bool immediate = false)
	{
		if (!this.setManually)
		{
			this.cameraProperties = this.GetCameraProperties(immediate);
		}
		if (this.cameraProperties.targetPoint.y < 1.25f)
		{
			this.cameraProperties.targetPoint.y = 1.25f;
		}
		this.blurEffect.strength = this.cameraProperties.depthOfFieldStrength;
		this.backgroundCamera.enabled = this.useBackgroundBlurCam;
		this.blurEffect.enabled = this.useBackgroundBlurCam;
		if (this.useBackgroundBlurCam)
		{
			float num = this.cameraProperties.distance + this.cameraProperties.clipPlaneSplitPoint;
			this.camera.farClipPlane = num;
			this.backgroundCamera.nearClipPlane = num - 20f;
			this.camera.clearFlags = CameraClearFlags.Depth;
			this.camera.backgroundColor = Color.clear;
		}
		else
		{
			this.camera.farClipPlane = this.backgroundCamera.farClipPlane;
			this.camera.clearFlags = CameraClearFlags.Skybox;
		}
		this.camera.transform.position = this.cameraProperties.position;
		this.camera.transform.rotation = HighlandCameraProperties.rotation;
		this.camera.fieldOfView = this.cameraProperties.fieldOfView;
		Matrix4x4 matrix4x = this.cameraProperties.CreateViewportAdjustmentMatrix(false);
		Matrix4x4 matrix4x2 = Matrix4x4.Perspective(this.camera.fieldOfView, this.camera.aspect, this.camera.nearClipPlane, this.camera.farClipPlane);
		Matrix4x4 matrix4x3 = this.cameraProperties.CreateShearingMatrix();
		this.camera.projectionMatrix = matrix4x * matrix4x2 * matrix4x3;
		foreach (Camera camera in this.allSubCameras)
		{
			camera.transform.position = this.cameraProperties.position;
			camera.transform.rotation = HighlandCameraProperties.rotation;
			camera.aspect = this.camera.aspect;
			camera.fieldOfView = this.camera.fieldOfView;
			matrix4x2 = Matrix4x4.Perspective(camera.fieldOfView, camera.aspect, camera.nearClipPlane, camera.farClipPlane);
			camera.projectionMatrix = matrix4x * matrix4x2 * matrix4x3;
		}
		GameCamera.cameraZ = this.camera.transform.position.z;
		if (this.highQualityWaterReflections != !this.waterReflectionGrab.enabled || this.highQualityWaterReflections != (MonoSingleton<WaterCamera>.instance != null && MonoSingleton<WaterCamera>.instance.gameObject.activeSelf))
		{
			this.RefreshWaterReflectionQuality();
		}
		if (Game.instance.inTitleScreenAndIntroState)
		{
			this.waterReflectionReferencePosition = GameCamera.instance.cameraProperties.targetPoint;
			float num2 = Mathf.InverseLerp(20f, 30f, this.introCameraState.time);
			this.waterReflectionReferencePosition.z = Mathf.Lerp(4000f, Runner.instance.transform.position.z, num2);
		}
		else
		{
			this.waterReflectionReferencePosition = Runner.instance.transform.position;
		}
		this.waterReflectionReferencePosition.y = 0f;
		Shader.SetGlobalFloat("_Global_WaterReflectionReferenceZ", this.waterReflectionReferencePosition.z);
		if (this.waterReflectionGrab.enabled)
		{
			this.waterReflectionGrab.RefreshReflectionOffset();
		}
		int num3 = (this.highQualityWaterReflections ? 1 : 0);
		if ((float)num3 != this._waterQualityFade)
		{
			this._waterQualityFade = Mathf.MoveTowards(this._waterQualityFade, (float)num3, Time.unscaledDeltaTime / 0.5f);
			this.SetWaterQualityFade(this._waterQualityFade);
			if (!this.highQualityWaterReflections && this._waterQualityFade == (float)num3)
			{
				this.SetWaterRenderingMode(false);
			}
		}
		Shader.DisableKeyword("HIGHLAND_SEA_LEVEL_CLIP_DISABLED");
		if (DebugOptions.opts.visualiseTexelDensity != GameCamera._debugDrawTexelDensity)
		{
			GameCamera._debugDrawTexelDensity = DebugOptions.opts.visualiseTexelDensity;
			if (GameCamera._debugDrawTexelDensity)
			{
				Shader.EnableKeyword("DEBUG_DRAW_TEXEL_DENSITY");
				return;
			}
			Shader.DisableKeyword("DEBUG_DRAW_TEXEL_DENSITY");
		}
	}

	// Token: 0x0600009A RID: 154 RVA: 0x00009984 File Offset: 0x00007B84
	private void Update()
	{
		if (!Game.loaded || WorldManager.instance.loading)
		{
			return;
		}
		if (DebugOptions.opts.dynamicWaterQuality)
		{
			this.UpdateDynamicWaterQuality();
		}
	}

	// Token: 0x0600009B RID: 155 RVA: 0x000099AC File Offset: 0x00007BAC
	private void UpdateDynamicWaterQuality()
	{
		this._waterQualityTimeSinceLastSwapOrZoneChange += Time.unscaledDeltaTime;
		bool flag = !LowQualityWaterZone.isInside;
		if (flag != this.highQualityWaterReflections)
		{
			if (this._waterQualityTimeSinceLastSwapOrZoneChange < 8f)
			{
				return;
			}
			this._waterQualityTimeSinceLastSwapOrZoneChange = 0f;
			this.highQualityWaterReflections = flag;
		}
		if (this._wantedHighQualityLastFrame != flag)
		{
			this._waterQualityTimeSinceLastSwapOrZoneChange = 0f;
			this._wantedHighQualityLastFrame = flag;
		}
	}

	// Token: 0x0600009C RID: 156 RVA: 0x00009A18 File Offset: 0x00007C18
	public void ResetDynamicWaterQuality()
	{
		this.highQualityWaterReflections = true;
		this._waterQualityTimeSinceLastSwapOrZoneChange = 1000f;
	}

	// Token: 0x0600009D RID: 157 RVA: 0x00009A2C File Offset: 0x00007C2C
	private void RefreshWaterReflectionQuality()
	{
		if (this._highQualityWaterReflections == this.highQualityWaterReflections && this._doneFirstWaterQualitySetup)
		{
			return;
		}
		this._highQualityWaterReflections = this.highQualityWaterReflections;
		if (!this._doneFirstWaterQualitySetup)
		{
			this.SetWaterRenderingMode(this.highQualityWaterReflections);
			this._waterQualityFade = (float)(this.highQualityWaterReflections ? 1 : 0);
			this.SetWaterQualityFade(this._waterQualityFade);
			this._doneFirstWaterQualitySetup = true;
			return;
		}
		if (this.highQualityWaterReflections)
		{
			this.SetWaterRenderingMode(true);
		}
	}

	// Token: 0x0600009E RID: 158 RVA: 0x00009AA8 File Offset: 0x00007CA8
	private void SetWaterRenderingMode(bool high)
	{
		if (high)
		{
			this.waterReflectionGrab.enabled = false;
			MonoSingleton<WaterCamera>.instance.gameObject.SetActive(true);
			using (List<WaterPlane>.Enumerator enumerator = MonoInstancer<WaterPlane>.all.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					WaterPlane waterPlane = enumerator.Current;
					if (waterPlane != null)
					{
						waterPlane.RemoveLowQualityReflectionProperties();
					}
				}
				return;
			}
		}
		this.waterReflectionGrab.enabled = true;
		MonoSingleton<WaterCamera>.instance.gameObject.SetActive(false);
	}

	// Token: 0x0600009F RID: 159 RVA: 0x00009B40 File Offset: 0x00007D40
	private void SetWaterQualityFade(float lowToHighNorm)
	{
		foreach (WaterPlane waterPlane in MonoInstancer<WaterPlane>.all)
		{
			waterPlane.SetDistanceFadeScalar(1f - lowToHighNorm);
		}
	}

	// Token: 0x060000A0 RID: 160 RVA: 0x00009B98 File Offset: 0x00007D98
	public void SetAnimationActive(InkAnimation inkAnim)
	{
		if (inkAnim != null)
		{
			this.animatedCameraState.anim = inkAnim;
			this.animatedCameraState.Enter();
			return;
		}
		this.animatedCameraState.anim = null;
		this.animatedCameraState.Exit();
	}

	// Token: 0x060000A1 RID: 161 RVA: 0x00009BD2 File Offset: 0x00007DD2
	private void SetBackgroundBlurCamEnabled(bool blurCamEnabled)
	{
		this.useBackgroundBlurCam = blurCamEnabled;
		this.backgroundCamera.enabled = blurCamEnabled;
		this.blurEffect.enabled = blurCamEnabled;
	}

	// Token: 0x060000A2 RID: 162 RVA: 0x00009BF4 File Offset: 0x00007DF4
	public static Vector2 SmoothDampXYScreenSpace(Vector2 currentPos, Vector2 targetPos, float zoom, ref Vector2 screenspaceVelocityToViewpoint, float smoothTime, float maxSpeed)
	{
		Vector2 vector = currentPos / zoom;
		Vector2 vector2 = targetPos / zoom;
		return Vector2.SmoothDamp(vector, vector2, ref screenspaceVelocityToViewpoint, smoothTime, maxSpeed) * zoom;
	}

	// Token: 0x060000A3 RID: 163 RVA: 0x00009C24 File Offset: 0x00007E24
	public static float ViewWidth(float atZ = 0f)
	{
		Camera camera = GameCamera.instance.camera;
		if (camera.orthographic)
		{
			return 2f * camera.aspect * camera.orthographicSize;
		}
		float num = atZ - camera.transform.position.z;
		if (num < 25f)
		{
			num = 25f;
		}
		float num2 = num * Mathf.Tan(0.5f * camera.fieldOfView * 0.017453292f);
		return camera.aspect * 2f * num2;
	}

	// Token: 0x060000A4 RID: 164 RVA: 0x00009CA4 File Offset: 0x00007EA4
	public static SerializableCamera ToShot(HighlandCameraProperties cameraProperties, CameraShotGeneratorProperties shotGeneratorProperties, Camera camera)
	{
		float num = 0f;
		return GameCamera.ToShot(cameraProperties, shotGeneratorProperties, camera, out num);
	}

	// Token: 0x060000A5 RID: 165 RVA: 0x00009CC4 File Offset: 0x00007EC4
	public static SerializableCamera ToShot(HighlandCameraProperties cameraProperties, CameraShotGeneratorProperties shotGeneratorProperties, Camera camera, out float distanceFromTarget)
	{
		SerializableCamera serializableCamera = new SerializableCamera(camera);
		cameraProperties.ApplyTo(serializableCamera, false);
		CameraShotGeneratorTools.CreateCameraShot(shotGeneratorProperties, ref serializableCamera, out distanceFromTarget);
		return serializableCamera;
	}

	// Token: 0x060000A6 RID: 166 RVA: 0x00009CF0 File Offset: 0x00007EF0
	public static Vector3 ViewportOffset(float distanceInDirection, Vector2 viewportOffset, float fieldOfView, float aspectRatio)
	{
		float num = distanceInDirection * Mathf.Tan(fieldOfView * 0.5f * 0.017453292f) * 2f;
		float num2 = aspectRatio * num;
		Vector3 vector = new Vector3(num2 * viewportOffset.x, num * viewportOffset.y, 0f);
		Quaternion rotation = HighlandCameraProperties.rotation;
		Vector3 vector2 = rotation * Vector3.up;
		Vector3 vector3 = rotation * Vector3.right;
		return vector.x * vector3 + vector.y * vector2;
	}

	// Token: 0x060000A7 RID: 167 RVA: 0x00009D74 File Offset: 0x00007F74
	public static float GetShearFactorBetweenPoints(Vector3 entry, Vector3 exit, AnimationCurve shearFactorOverDegreesToTarget)
	{
		Vector3 vector = Vector3.Normalize(exit - entry);
		if (vector.z < 0f)
		{
			vector *= -1f;
		}
		float num = GameCamera.SignedDegreesAgainstDirection(vector, Vector3.forward, Vector3.right);
		return shearFactorOverDegreesToTarget.Evaluate(num);
	}

	// Token: 0x060000A8 RID: 168 RVA: 0x00009DC0 File Offset: 0x00007FC0
	public static float SignedDegreesAgainstDirection(Vector3 a, Vector3 b, Vector3 direction)
	{
		Vector3 vector = ((direction.sqrMagnitude == 1f) ? direction : direction.normalized);
		Vector3 normalized = Vector3.ProjectOnPlane(a, vector).normalized;
		Vector3 normalized2 = Vector3.ProjectOnPlane(b, vector).normalized;
		return Vector3.SignedAngle(normalized, normalized2, direction);
	}

	// Token: 0x040000DB RID: 219
	[Range(0.1f, 1f)]
	public float debugDistanceScalar = 1f;

	// Token: 0x040000DC RID: 220
	public GameCameraSettings settings;

	// Token: 0x040000DD RID: 221
	public Prop propForPeakZoomMode;

	// Token: 0x040000DE RID: 222
	public bool setManually;

	// Token: 0x040000DF RID: 223
	public HighlandCameraProperties cameraProperties = HighlandCameraProperties.@default;

	// Token: 0x040000E0 RID: 224
	public bool useBackgroundBlurCam;

	// Token: 0x040000E1 RID: 225
	private static float _cameraZ;

	// Token: 0x040000E2 RID: 226
	public static Action onChangeCameraZ;

	// Token: 0x040000E3 RID: 227
	public Camera camera;

	// Token: 0x040000E4 RID: 228
	private WaterReflectionGrab _waterReflectionGrab;

	// Token: 0x040000E5 RID: 229
	private BlurEffect _blurEffect;

	// Token: 0x040000E6 RID: 230
	private Camera _backgroundCamera;

	// Token: 0x040000E7 RID: 231
	private List<Camera> _allSubCameras = new List<Camera>();

	// Token: 0x040000E8 RID: 232
	private bool _allSubCamerasSet;

	// Token: 0x040000E9 RID: 233
	public const string highQualityWaterPrefName = "highQualityWater";

	// Token: 0x040000EA RID: 234
	public bool highQualityWaterReflections = true;

	// Token: 0x040000EB RID: 235
	private GameCamera.BaseCameraState[] cameraStates;

	// Token: 0x040000EC RID: 236
	public GameCamera.RunningCameraState runningState;

	// Token: 0x040000ED RID: 237
	public GameCamera.CameraModifierState modifierState;

	// Token: 0x040000EE RID: 238
	public GameCamera.InkModifierState inkModifierState;

	// Token: 0x040000EF RID: 239
	public GameCamera.PlayerZoomCameraState playerZoomState;

	// Token: 0x040000F0 RID: 240
	public GameCamera.MapConfirmCameraState mapConfirmState;

	// Token: 0x040000F1 RID: 241
	public GameCamera.FollowPathCameraState followPathState;

	// Token: 0x040000F2 RID: 242
	public GameCamera.PeakCameraState peakState;

	// Token: 0x040000F3 RID: 243
	public GameCamera.StoneSkimmingCameraState stoneSkimmingState;

	// Token: 0x040000F4 RID: 244
	public GameCamera.ShelterCameraState shelterState;

	// Token: 0x040000F5 RID: 245
	public GameCamera.CaughtCameraState caughtCameraState;

	// Token: 0x040000F6 RID: 246
	public GameCamera.CameraShakeCameraState cameraShakeState;

	// Token: 0x040000F7 RID: 247
	public GameCamera.PushVolumeCameraState pushVolumeState;

	// Token: 0x040000F8 RID: 248
	public GameCamera.AnimatedCameraState animatedCameraState;

	// Token: 0x040000F9 RID: 249
	public GameCamera.IntroCameraState introCameraState;

	// Token: 0x040000FA RID: 250
	public GameCamera.FreeCameraState freeCameraState;

	// Token: 0x040000FB RID: 251
	public Vector3 waterReflectionReferencePosition;

	// Token: 0x040000FC RID: 252
	private bool _highQualityWaterReflections;

	// Token: 0x040000FD RID: 253
	private bool _doneFirstWaterQualitySetup;

	// Token: 0x040000FE RID: 254
	private float _waterQualityFade;

	// Token: 0x040000FF RID: 255
	private float _waterQualityTimeSinceLastSwapOrZoneChange;

	// Token: 0x04000100 RID: 256
	private bool _wantedHighQualityLastFrame;

	// Token: 0x04000101 RID: 257
	private static bool _debugDrawTexelDensity;

	// Token: 0x0200024D RID: 589
	[Serializable]
	public class AnimatedCameraState : GameCamera.BaseCameraState
	{
		// Token: 0x060014C1 RID: 5313 RVA: 0x0008EBC5 File Offset: 0x0008CDC5
		public void Enter()
		{
		}

		// Token: 0x060014C2 RID: 5314 RVA: 0x0008EBC7 File Offset: 0x0008CDC7
		public void Exit()
		{
			base.strength = 0f;
		}

		// Token: 0x060014C3 RID: 5315 RVA: 0x0008EBD4 File Offset: 0x0008CDD4
		public override void Reset()
		{
			base.strength = 0f;
		}

		// Token: 0x060014C4 RID: 5316 RVA: 0x0008EBE4 File Offset: 0x0008CDE4
		public override void Update(bool immediate, ref HighlandCameraProperties masterCameraProperties)
		{
			if (this.anim == null)
			{
				return;
			}
			Animation component = this.anim.GetComponent<Animation>();
			if (Application.isPlaying && !component.isPlaying)
			{
				this.Exit();
				return;
			}
			AnimationState animationState = null;
			using (IEnumerator enumerator = component.GetEnumerator())
			{
				if (enumerator.MoveNext())
				{
					animationState = (AnimationState)enumerator.Current;
				}
			}
			float time = animationState.time;
			float num = ((this.anim.cameraBlendInTime > 0f) ? Mathf.InverseLerp(0f, this.anim.cameraBlendInTime, animationState.time) : 1f);
			float num2 = ((this.anim.cameraBlendOutTime > 0f) ? (1f - Mathf.InverseLerp(animationState.length - this.anim.cameraBlendOutTime, animationState.length, animationState.time)) : 1f);
			base.strength = Mathf.SmoothStep(0f, 1f, Mathf.Min(num, num2));
			if (!component.isPlaying)
			{
				base.strength = 1f;
			}
			float num3 = masterCameraProperties.targetPoint.z - masterCameraProperties.distance + masterCameraProperties.clipPlaneSplitPoint;
			this.cameraProperties.targetPoint = this.anim.transform.position;
			float num4 = this.cameraProperties.targetPoint.z - this.cameraProperties.distance;
			this.cameraProperties.clipPlaneSplitPoint = num3 - num4;
			masterCameraProperties = HighlandCameraProperties.Lerp(masterCameraProperties, this.cameraProperties, base.strength);
		}

		// Token: 0x040013D9 RID: 5081
		[NonSerialized]
		public InkAnimation anim;
	}

	// Token: 0x0200024E RID: 590
	public abstract class BaseCameraState
	{
		// Token: 0x17000486 RID: 1158
		// (get) Token: 0x060014C6 RID: 5318 RVA: 0x0008EDA4 File Offset: 0x0008CFA4
		// (set) Token: 0x060014C7 RID: 5319 RVA: 0x0008EDAC File Offset: 0x0008CFAC
		public float strength
		{
			get
			{
				return this._strength;
			}
			set
			{
				this._strength = value;
				if (this._strength < 0.001f)
				{
					this._strength = 0f;
					return;
				}
				if (this._strength > 0.999f)
				{
					this._strength = 1f;
				}
			}
		}

		// Token: 0x17000487 RID: 1159
		// (get) Token: 0x060014C8 RID: 5320 RVA: 0x0008EDE6 File Offset: 0x0008CFE6
		public static GameCamera gameCamera
		{
			get
			{
				return GSR.GameCam;
			}
		}

		// Token: 0x17000488 RID: 1160
		// (get) Token: 0x060014C9 RID: 5321 RVA: 0x0008EDED File Offset: 0x0008CFED
		public static Runner runner
		{
			get
			{
				return GSR.Runner;
			}
		}

		// Token: 0x17000489 RID: 1161
		// (get) Token: 0x060014CA RID: 5322 RVA: 0x0008EDF4 File Offset: 0x0008CFF4
		public GameCameraSettings settings
		{
			get
			{
				return GameCamera.BaseCameraState.gameCamera.settings;
			}
		}

		// Token: 0x060014CB RID: 5323 RVA: 0x0008EE00 File Offset: 0x0008D000
		public virtual void Reset()
		{
		}

		// Token: 0x060014CC RID: 5324 RVA: 0x0008EE02 File Offset: 0x0008D002
		public virtual void Update(bool immediate, ref HighlandCameraProperties masterCameraProperties)
		{
		}

		// Token: 0x060014CD RID: 5325 RVA: 0x0008EE04 File Offset: 0x0008D004
		public virtual void OnDrawGizmos()
		{
		}

		// Token: 0x060014CE RID: 5326 RVA: 0x0008EE08 File Offset: 0x0008D008
		public static float UpdateZoom(float zoom, float targetZoom, ref float zoomCountVelocity, float zoomSmoothTime, Range maxSpeedRange = default(Range))
		{
			if (zoom == targetZoom)
			{
				zoomCountVelocity = 0f;
				return targetZoom;
			}
			float num = Mathf.InverseLerp(0.5f, 10f, 1f / zoomSmoothTime);
			if (maxSpeedRange == default(Range))
			{
				maxSpeedRange = new Range(float.MaxValue, float.MaxValue);
			}
			float num2 = maxSpeedRange.Lerp(num);
			zoomCountVelocity = Mathf.Clamp(zoomCountVelocity, -num2, num2);
			float num3 = Mathf.Log(zoom);
			float num4 = Mathf.Log(targetZoom);
			zoom = Mathf.Exp(Mathf.SmoothDamp(num3, num4, ref zoomCountVelocity, zoomSmoothTime));
			return zoom;
		}

		// Token: 0x040013DA RID: 5082
		[SerializeField]
		private float _strength;

		// Token: 0x040013DB RID: 5083
		public HighlandCameraProperties cameraProperties = HighlandCameraProperties.@default;
	}

	// Token: 0x0200024F RID: 591
	[Serializable]
	public class CameraModifierState : GameCamera.BaseCameraState
	{
		// Token: 0x060014D0 RID: 5328 RVA: 0x0008EEA4 File Offset: 0x0008D0A4
		public override void Reset()
		{
			foreach (CameraVolume cameraVolume in this._activeCameraVolumes)
			{
				cameraVolume.activeStrength = 0f;
				cameraVolume.activeStrengthSpeed = 0f;
			}
			this._activeCameraVolumes.Clear();
			this.SetupForCurrentLevel();
		}

		// Token: 0x060014D1 RID: 5329 RVA: 0x0008EF18 File Offset: 0x0008D118
		public void SetupForCurrentLevel()
		{
			if (Level.current == null)
			{
				return;
			}
			this._activeCameraVolumes.Clear();
			this._activeCameraVolumes.AddRange(Level.current.cameraVolumes);
			this._activeCameraVolumes.AddRange(Runner.instance.cameraVolumes);
			this.SortActiveCameraVolumes();
		}

		// Token: 0x060014D2 RID: 5330 RVA: 0x0008EF68 File Offset: 0x0008D168
		public void ClearCameraVolumes()
		{
			this._activeCameraVolumes.Clear();
		}

		// Token: 0x060014D3 RID: 5331 RVA: 0x0008EF75 File Offset: 0x0008D175
		private void SortActiveCameraVolumes()
		{
			this._activeCameraVolumes.Sort(delegate(CameraVolume c1, CameraVolume c2)
			{
				int num = c1.priority - c2.priority;
				if (num != 0)
				{
					return num;
				}
				return c1.GetInstanceID().CompareTo(c2.GetInstanceID());
			});
		}

		// Token: 0x060014D4 RID: 5332 RVA: 0x0008EFA4 File Offset: 0x0008D1A4
		public override void Update(bool immediate, ref HighlandCameraProperties masterCameraProperties)
		{
			if (base.strength == 0f)
			{
				return;
			}
			if (GameCamera.BaseCameraState.gameCamera.peakState.strength == 1f)
			{
				return;
			}
			if (this.debugDisable)
			{
				return;
			}
			if (!Application.isPlaying)
			{
				this._activeCameraVolumes.Clear();
				this._activeCameraVolumes.AddRange(MonoInstancer<CameraVolume>.all);
				this.SortActiveCameraVolumes();
			}
			Vector3 vector = GameCamera.BaseCameraState.runner.focus;
			if (GameCamera.BaseCameraState.gameCamera.followPathState.active)
			{
				vector = GameCamera.BaseCameraState.gameCamera.followPathState.runnerFocusOverride;
			}
			Vector3 targetPoint = masterCameraProperties.targetPoint;
			bool flag = false;
			foreach (CameraVolume cameraVolume in this._activeCameraVolumes)
			{
				if (!(cameraVolume == null))
				{
					bool flag2 = cameraVolume.inkDrivenOnly && cameraVolume.inkHasEnabled;
					cameraVolume.UpdateActiveStrength(vector, immediate, flag && !flag2);
					cameraVolume.ApplyTo(targetPoint, ref masterCameraProperties, base.strength);
					flag = flag2;
				}
			}
		}

		// Token: 0x040013DC RID: 5084
		public bool debugDisable;

		// Token: 0x040013DD RID: 5085
		private List<CameraVolume> _activeCameraVolumes = new List<CameraVolume>();
	}

	// Token: 0x02000250 RID: 592
	[Serializable]
	public class CameraShakeCameraState : GameCamera.BaseCameraState
	{
		// Token: 0x060014D6 RID: 5334 RVA: 0x0008F0D7 File Offset: 0x0008D2D7
		public override void Reset()
		{
			this.Clear();
		}

		// Token: 0x060014D7 RID: 5335 RVA: 0x0008F0DF File Offset: 0x0008D2DF
		public void StartShake(CameraShakeName shakeName)
		{
			this.currentShake = this.shakeDatabase.FindWithName(shakeName);
			this.currentTime = 0f;
		}

		// Token: 0x060014D8 RID: 5336 RVA: 0x0008F0FE File Offset: 0x0008D2FE
		public void Clear()
		{
			this.currentShake = null;
			this.currentTime = 0f;
		}

		// Token: 0x060014D9 RID: 5337 RVA: 0x0008F114 File Offset: 0x0008D314
		private VibrationMoment GetVibration()
		{
			if (this.currentShake == null)
			{
				return VibrationMoment.zero;
			}
			return VibrationMoment.Create(this.currentShake.GetViewportOffsetAtTime(this.currentTime).magnitude * 30f);
		}

		// Token: 0x060014DA RID: 5338 RVA: 0x0008F15C File Offset: 0x0008D35C
		public override void Update(bool immediate, ref HighlandCameraProperties masterCameraProperties)
		{
			if (!this.subscribed)
			{
				this.subscribed = true;
				InputVibration.AddDelegate(new InputVibration.GetVibrationDelegate(this.GetVibration));
			}
			if (this.currentShake != null)
			{
				this.currentTime += Time.unscaledDeltaTime;
				if (this.currentShake.IsCompleteAtTime(this.currentTime))
				{
					this.Clear();
				}
			}
			this.ApplyTo(ref masterCameraProperties);
		}

		// Token: 0x060014DB RID: 5339 RVA: 0x0008F1C9 File Offset: 0x0008D3C9
		public void ApplyTo(ref HighlandCameraProperties cameraProperties)
		{
			if (this.currentShake == null)
			{
				return;
			}
			cameraProperties.viewport += this.currentShake.GetViewportOffsetAtTime(this.currentTime);
		}

		// Token: 0x040013DE RID: 5086
		public CameraShakeDatabase shakeDatabase;

		// Token: 0x040013DF RID: 5087
		public CameraShake currentShake;

		// Token: 0x040013E0 RID: 5088
		public float currentTime;

		// Token: 0x040013E1 RID: 5089
		private bool subscribed;
	}

	// Token: 0x02000251 RID: 593
	[Serializable]
	public class CaughtCameraState : GameCamera.BaseCameraState
	{
		// Token: 0x060014DD RID: 5341 RVA: 0x0008F20C File Offset: 0x0008D40C
		public void Begin()
		{
			this.stateTimer = 0f;
			this._active = true;
			base.strength = 1f;
			this.cameraProperties = HighlandCameraProperties.@default;
			this.cameraProperties.distance = this._settings.startDistance;
			this._intialZoomOutProgress = 0f;
			this._carryTimer = 0f;
		}

		// Token: 0x060014DE RID: 5342 RVA: 0x0008F26D File Offset: 0x0008D46D
		public void End()
		{
			this._active = false;
		}

		// Token: 0x060014DF RID: 5343 RVA: 0x0008F276 File Offset: 0x0008D476
		public override void Reset()
		{
			base.strength = 0f;
			this._active = false;
			this._carryTimer = 0f;
		}

		// Token: 0x060014E0 RID: 5344 RVA: 0x0008F298 File Offset: 0x0008D498
		public override void Update(bool immediate, ref HighlandCameraProperties masterCameraProperties)
		{
			this.stateTimer += Time.deltaTime;
			base.strength = Mathf.MoveTowards(base.strength, (float)(this._active ? 1 : 0), Time.deltaTime);
			if (base.strength == 0f)
			{
				return;
			}
			if (!MonoSingleton<Eagle>.instance.complete && MonoSingleton<Eagle>.instance.hasSpline)
			{
				if (this.stateTimer > this._settings.ealgeToPlayerInitialPause)
				{
					this._intialZoomOutProgress = Mathf.MoveTowards(this._intialZoomOutProgress, 1f, Time.deltaTime / this._settings.eagleToPlayerLerpTime);
				}
				Vector3 splinePos = MonoSingleton<Eagle>.instance.splinePos;
				Vector3 vector = (MonoSingleton<Eagle>.instance.complete ? Runner.instance.transform.position : Runner.instance.catchAimBasePos);
				float num = Mathf.SmoothStep(0f, 1f, this._intialZoomOutProgress);
				Vector3 vector2 = Vector3.Lerp(splinePos, vector, 0.5f);
				this.cameraProperties.targetPoint = Vector3.Lerp(splinePos, vector2, num);
				this.cameraProperties.distance = Mathf.Lerp(this._settings.startDistance, this._settings.zoomedOutDistance, num);
				float num2 = Vector2.Distance(splinePos, vector);
				float num3 = this._settings.approachDistRange.InverseLerp(num2);
				this.cameraProperties.distance = Mathf.Lerp(this.cameraProperties.distance, this._settings.catchDistance, Mathf.SmoothStep(0f, 1f, 1f - num3));
				if (MonoSingleton<Eagle>.instance.carryingToTarget)
				{
					float num4 = Mathf.InverseLerp(0f, this._settings.flyDistForZoomOutAfterPickup, MonoSingleton<Eagle>.instance.distFromPickup);
					this.cameraProperties.targetPoint = Vector3.Lerp(this.cameraProperties.targetPoint, splinePos, num4);
					this.cameraProperties.distance = Mathf.Lerp(this.cameraProperties.distance, this._settings.zoomedOutForCarryDistance, Mathf.SmoothStep(0f, 1f, num4));
					float num5 = Mathf.Pow(1f - Mathf.InverseLerp(0f, this._settings.flyDistForZoomInBeforeDropOff, MonoSingleton<Eagle>.instance.distToDropoff), this._settings.zoomBackInCurvePower);
					this.cameraProperties.distance = Mathf.Lerp(this.cameraProperties.distance, this._settings.zoomedInForDropOffDistance, Mathf.SmoothStep(0f, 1f, num5));
					this._carryTimer += Time.deltaTime;
					if (this._settings.cutToZoomedInDuringFlightTimeRange.Contains(this._carryTimer))
					{
						this.cameraProperties.distance = this._settings.startDistance;
					}
				}
			}
			masterCameraProperties = HighlandCameraProperties.Lerp(masterCameraProperties, this.cameraProperties, base.strength);
		}

		// Token: 0x040013E2 RID: 5090
		private float stateTimer;

		// Token: 0x040013E3 RID: 5091
		private bool _active;

		// Token: 0x040013E4 RID: 5092
		private float _intialZoomOutProgress;

		// Token: 0x040013E5 RID: 5093
		private float _carryTimer;

		// Token: 0x040013E6 RID: 5094
		[SerializeField]
		private CaughtCameraSettings _settings;
	}

	// Token: 0x02000252 RID: 594
	[Serializable]
	public class FollowPathCameraState : GameCamera.BaseCameraState
	{
		// Token: 0x1700048A RID: 1162
		// (get) Token: 0x060014E2 RID: 5346 RVA: 0x0008F58C File Offset: 0x0008D78C
		public float pathChangeDuration
		{
			get
			{
				float num;
				switch (this.lengthType)
				{
				case GameCamera.FollowPathCameraState.PathLength.Local:
					num = this.settings.durationLocal;
					break;
				case GameCamera.FollowPathCameraState.PathLength.Standard:
					num = this.settings.durationStandard;
					break;
				case GameCamera.FollowPathCameraState.PathLength.Long:
					num = this.settings.durationLong;
					break;
				default:
					num = this.settings.durationStandard;
					break;
				}
				return num;
			}
		}

		// Token: 0x1700048B RID: 1163
		// (get) Token: 0x060014E3 RID: 5347 RVA: 0x0008F5EB File Offset: 0x0008D7EB
		public float progress
		{
			get
			{
				return Mathf.InverseLerp(this._startTime, this._startTime + this.pathChangeDuration, Time.time);
			}
		}

		// Token: 0x1700048C RID: 1164
		// (get) Token: 0x060014E4 RID: 5348 RVA: 0x0008F60A File Offset: 0x0008D80A
		public Vector3 runnerFocusOverride
		{
			get
			{
				return (this.blendingToExit ? this.exit : this.entry) + 4f * Vector3.up;
			}
		}

		// Token: 0x060014E5 RID: 5349 RVA: 0x0008F638 File Offset: 0x0008D838
		public void Enter(Vector3 pathEntry, Vector3 pathExit, GameCamera.FollowPathCameraState.PathLength lengthType)
		{
			this.lengthType = lengthType;
			this._startTime = Time.time;
			this.entry = pathEntry;
			this.exit = pathExit;
			this.active = true;
			this._startProperties = GameCamera.BaseCameraState.gameCamera.cameraProperties;
			this.blendingToExit = false;
			this._endProperties = HighlandCameraProperties.@default;
		}

		// Token: 0x060014E6 RID: 5350 RVA: 0x0008F68E File Offset: 0x0008D88E
		public override void Reset()
		{
			this.active = false;
			base.strength = 0f;
		}

		// Token: 0x060014E7 RID: 5351 RVA: 0x0008F6A4 File Offset: 0x0008D8A4
		public void ReadyForBlendToExit()
		{
			this.blendingToExit = true;
			GameCamera.BaseCameraState.gameCamera.runningState.Update(true, ref this._endProperties);
			GameCamera.BaseCameraState.gameCamera.modifierState.Update(true, ref this._endProperties);
			this._blendToExitStartProgress = this.progress;
		}

		// Token: 0x060014E8 RID: 5352 RVA: 0x0008F6F0 File Offset: 0x0008D8F0
		public override void Update(bool immediate, ref HighlandCameraProperties masterCameraProperties)
		{
			if (!Application.isPlaying)
			{
				return;
			}
			if (!this.active)
			{
				return;
			}
			if (this.progress >= 1f)
			{
				base.strength = 0f;
				this.active = false;
				return;
			}
			if (this.lengthType == GameCamera.FollowPathCameraState.PathLength.Local)
			{
				base.strength = this.settings.strengthOverProgressLocal.Evaluate(this.progress);
			}
			else
			{
				base.strength = this.settings.strengthOverProgress.Evaluate(this.progress);
			}
			float num = Mathf.SmoothStep(0f, 1f, Mathf.InverseLerp(this._blendToExitStartProgress, 1f, this.progress));
			Vector3 vector = this.exit;
			if (this.blendingToExit)
			{
				vector = Vector3.Lerp(this.exit, this._endProperties.targetPoint, num);
			}
			this.cameraProperties.targetPoint = Vector3.Lerp(this._startProperties.targetPoint, vector, Mathf.SmoothStep(0f, 1f, this.progress));
			float num2 = 1f;
			if (this.lengthType == GameCamera.FollowPathCameraState.PathLength.Local)
			{
				num2 = this.settings.maxLocalCamZoomStrength * this.settings.localPathZDistRange.InverseLerp(Mathf.Abs(this.entry.z - this.exit.z));
			}
			this.cameraProperties.targetPoint = this.cameraProperties.targetPoint + Vector3.up * this.settings.extraHeightOverProgress.Evaluate(this.progress) * num2;
			float num3 = this._startProperties.distance;
			if (this.blendingToExit)
			{
				num3 = Mathf.Lerp(num3, this._endProperties.distance, num);
			}
			float num4 = 0.5f * (Mathf.Cos(this.progress * 2f * 3.1415927f) + 1f);
			if (this.settings.minDistance > num3)
			{
				num3 = Mathf.Lerp(num3, this.settings.minDistance, num4);
			}
			this.cameraProperties.distance = num3 + this.settings.distanceFromStartOverProgress.Evaluate(this.progress) * num2;
			this.cameraProperties.shearFactor = this.settings.shearOverProgress.Evaluate(this.progress) * num2;
			masterCameraProperties = HighlandCameraProperties.Lerp(masterCameraProperties, this.cameraProperties, base.strength);
		}

		// Token: 0x040013E7 RID: 5095
		public new FollowPathCameraSettings settings;

		// Token: 0x040013E8 RID: 5096
		public Vector3 entry;

		// Token: 0x040013E9 RID: 5097
		public Vector3 exit;

		// Token: 0x040013EA RID: 5098
		public bool active;

		// Token: 0x040013EB RID: 5099
		public bool blendingToExit;

		// Token: 0x040013EC RID: 5100
		public GameCamera.FollowPathCameraState.PathLength lengthType;

		// Token: 0x040013ED RID: 5101
		private HighlandCameraProperties _startProperties;

		// Token: 0x040013EE RID: 5102
		private HighlandCameraProperties _endProperties;

		// Token: 0x040013EF RID: 5103
		private float _startTime;

		// Token: 0x040013F0 RID: 5104
		private float _blendToExitStartProgress;

		// Token: 0x02000431 RID: 1073
		public enum PathLength
		{
			// Token: 0x04001B85 RID: 7045
			Local,
			// Token: 0x04001B86 RID: 7046
			Standard,
			// Token: 0x04001B87 RID: 7047
			Long
		}
	}

	// Token: 0x02000253 RID: 595
	[Serializable]
	public class FreeCameraState : GameCamera.BaseCameraState
	{
		// Token: 0x060014EA RID: 5354 RVA: 0x0008F954 File Offset: 0x0008DB54
		public override void Update(bool immediate, ref HighlandCameraProperties masterCameraProperties)
		{
			if (!this.active)
			{
				this.cameraProperties = masterCameraProperties;
				this.cameraProperties.viewport = HighlandCameraProperties.@default.viewport;
				this.cameraProperties.viewportScale = HighlandCameraProperties.@default.viewportScale;
				this.cameraProperties.shearFactor = HighlandCameraProperties.@default.shearFactor;
				this.cameraProperties.fieldOfView = HighlandCameraProperties.@default.fieldOfView;
				this._speedScalar = 1f;
				return;
			}
			Vector3 vector = Vector3.zero;
			if (this.allowInput)
			{
				vector = GameInput.move2d;
				if (GameInput.zoomInHeld)
				{
					vector.z += 1f;
				}
				if (GameInput.zoomOutHeld)
				{
					vector.z -= 1f;
				}
			}
			FreeCameraStateSettings freeCameraStateSettings = (this.restricted ? this._settings : this._unrestrictedSettings);
			Vector3 vector2 = this._speedScalar * new Vector3(freeCameraStateSettings.speed * vector.x, freeCameraStateSettings.speed * vector.y, freeCameraStateSettings.zSpeed * vector.z);
			float num = ((this._velocity.magnitude < vector2.magnitude) ? freeCameraStateSettings.velocityLerpAccel : freeCameraStateSettings.velocityLerpDecel);
			this._velocity = Vector3.Lerp(this._velocity, vector2, TimeX.Lerping(num, Time.unscaledDeltaTime));
			Vector3 vector3 = this.cameraProperties.targetPoint + this._velocity * Time.unscaledDeltaTime;
			if (this.restricted)
			{
				Vector3 vector4 = Runner.instance.physicalPosition3d + 2.5f * Vector3.up;
				Vector3 position = this.cameraProperties.position;
				float num2 = vector4.z - position.z;
				float num3 = 0.008726646f * this.cameraProperties.fieldOfView;
				float num4 = num2 * Mathf.Tan(num3) + freeCameraStateSettings.bonusPanExtent;
				float num5 = (float)Screen.width / (float)Screen.height * num4 + freeCameraStateSettings.bonusPanExtent;
				Range range = Range.Centered(vector4.x, freeCameraStateSettings.panRestrictionScreenProportion * 2f * num5);
				Range range2 = Range.Centered(vector4.y, freeCameraStateSettings.panRestrictionScreenProportion * 2f * num4);
				if (range2.min < 1.25f)
				{
					range2.min = 1.25f;
				}
				if (!range.Contains(vector3.x))
				{
					vector3.x = range.Clamp(vector3.x);
					if ((vector3.x > vector4.x && this._velocity.x > 0f) || (vector3.x < vector4.x && this._velocity.x < 0f))
					{
						this._velocity.x = 0f;
					}
				}
				if (!range2.Contains(vector3.y))
				{
					vector3.y = range2.Clamp(vector3.y);
					if ((vector3.y > vector4.y && this._velocity.y > 0f) || (vector3.y < vector4.y && this._velocity.y < 0f))
					{
						this._velocity.y = 0f;
					}
				}
				MaxZoom maxZoom = (GameCamera.BaseCameraState.runner.isOnRidge ? MaxZoom.OnRidge : MaxZoom.Limited);
				float num6 = GameCamera.BaseCameraState.gameCamera.playerZoomState.CalculateTargetDistance(GameCamera.BaseCameraState.gameCamera.runningState.cameraProperties.distance, -1, maxZoom);
				Prop nearbyPeakProp = PropsController.instance.nearbyPeakProp;
				if (nearbyPeakProp != null && Vector2.Distance(nearbyPeakProp.transform.position, GameCamera.BaseCameraState.runner.position) < 30f)
				{
					num6 = GameCamera.BaseCameraState.gameCamera.peakState.CameraDistanceForPeak(nearbyPeakProp.isMinorPeak, GameCamera.BaseCameraState.runner.position.y);
				}
				float num7 = vector4.z - 1f;
				float num8 = vector4.z - num6;
				if (position.z > num7)
				{
					vector3.z = num7 + this.cameraProperties.distance;
					if (this._velocity.z > 0f)
					{
						this._velocity.z = 0f;
					}
				}
				else if (position.z < num8)
				{
					vector3.z = num8 + this.cameraProperties.distance;
					if (this._velocity.z < 0f)
					{
						this._velocity.z = 0f;
					}
				}
				if (this._velocity.z > 0f)
				{
					Vector3 vector5 = vector3 + this.cameraProperties.distance * Vector3.back;
					Poly poly = Raycast.AnyPolyOccludes(vector5, new Range(position.z - 2f, vector5.z + 2f), null, null);
					if (poly != null)
					{
						float num9 = poly.transform.position.z - 1f;
						if (vector5.z > num9)
						{
							vector3.z = num9 + this.cameraProperties.distance;
							if (this._velocity.z > 0f)
							{
								this._velocity.z = 0f;
							}
						}
					}
				}
			}
			this.cameraProperties.targetPoint = vector3;
			float num10;
			float num11;
			if (vector.magnitude > 0.5f)
			{
				num10 = freeCameraStateSettings.maxSpeedScalar;
				num11 = freeCameraStateSettings.acceleration;
			}
			else
			{
				num10 = 1f;
				num11 = freeCameraStateSettings.speedScalarFalloffSpeed;
			}
			this._speedScalar = Mathf.MoveTowards(this._speedScalar, num10, num11 * Time.unscaledDeltaTime);
			masterCameraProperties = this.cameraProperties;
		}

		// Token: 0x040013F1 RID: 5105
		public bool active;

		// Token: 0x040013F2 RID: 5106
		public bool restricted = true;

		// Token: 0x040013F3 RID: 5107
		public bool allowInput = true;

		// Token: 0x040013F4 RID: 5108
		private Vector3 _velocity;

		// Token: 0x040013F5 RID: 5109
		private Vector3 _accel;

		// Token: 0x040013F6 RID: 5110
		[SerializeField]
		[Disable]
		private float _speedScalar = 1f;

		// Token: 0x040013F7 RID: 5111
		[SerializeField]
		private FreeCameraStateSettings _settings;

		// Token: 0x040013F8 RID: 5112
		[SerializeField]
		private FreeCameraStateSettings _unrestrictedSettings;
	}

	// Token: 0x02000254 RID: 596
	[Serializable]
	public class InkModifierState : GameCamera.BaseCameraState
	{
		// Token: 0x060014EC RID: 5356 RVA: 0x0008FF24 File Offset: 0x0008E124
		public override void Update(bool immediate, ref HighlandCameraProperties masterCameraProperties)
		{
			if (this.inkBlendTime > 0f)
			{
				base.strength = Mathf.MoveTowards(base.strength, (float)(this.inkEnabled ? 1 : 0), Time.deltaTime / this.inkBlendTime);
			}
			else
			{
				base.strength = (float)(this.inkEnabled ? 1 : 0);
			}
			if (base.strength == 0f)
			{
				this._distance = masterCameraProperties.distance;
			}
			else
			{
				this._distance = Mathf.SmoothDamp(this._distance, this.inkTargetDistance, ref this._speed, 0.5f * this.inkBlendTime, float.MaxValue, Time.deltaTime);
			}
			masterCameraProperties.distance = Mathf.Lerp(masterCameraProperties.distance, this._distance, Mathf.SmoothStep(0f, 1f, base.strength));
		}

		// Token: 0x060014ED RID: 5357 RVA: 0x0008FFF8 File Offset: 0x0008E1F8
		public override void Reset()
		{
			this.inkEnabled = false;
			base.strength = 0f;
			this._distance = (this.inkTargetDistance = 40f);
			this._speed = 0f;
			this.inkBlendTime = 1f;
		}

		// Token: 0x040013F9 RID: 5113
		private const float defaultBlend = 1f;

		// Token: 0x040013FA RID: 5114
		private const float defaultInkDistance = 40f;

		// Token: 0x040013FB RID: 5115
		public bool inkEnabled;

		// Token: 0x040013FC RID: 5116
		public float inkBlendTime = 1f;

		// Token: 0x040013FD RID: 5117
		public float inkTargetDistance = 40f;

		// Token: 0x040013FE RID: 5118
		private float _distance;

		// Token: 0x040013FF RID: 5119
		private float _speed;
	}

	// Token: 0x02000255 RID: 597
	[Serializable]
	public class IntroCameraState : GameCamera.BaseCameraState
	{
		// Token: 0x1700048D RID: 1165
		// (get) Token: 0x060014EF RID: 5359 RVA: 0x0009005F File Offset: 0x0008E25F
		public float time
		{
			get
			{
				return this._time;
			}
		}

		// Token: 0x060014F0 RID: 5360 RVA: 0x00090068 File Offset: 0x0008E268
		public void BeginIntro(bool fastSwoop)
		{
			this.active = true;
			base.strength = 1f;
			this._time = 0f;
			this._distance = 0f;
			this._speed = 0f;
			this._fastSwooop = fastSwoop;
			this._reversedForTrailer = false;
			this._trailerSpeedScalar = 1f;
			this.introInProgress = true;
			this.CreateSpline();
		}

		// Token: 0x060014F1 RID: 5361 RVA: 0x000900D0 File Offset: 0x0008E2D0
		public void BeginTrailerSwoop(float speedScalar)
		{
			this.active = true;
			base.strength = 1f;
			this._time = 0f;
			this._distance = 0f;
			this._speed = 0f;
			this._fastSwooop = false;
			this._reversedForTrailer = true;
			this._trailerSpeedScalar = speedScalar;
			this.introInProgress = true;
			this.CreateSpline();
		}

		// Token: 0x060014F2 RID: 5362 RVA: 0x00090132 File Offset: 0x0008E332
		public override void Reset()
		{
			base.Reset();
			this.active = false;
			this.introInProgress = false;
			this.spline = null;
			this._reversedForTrailer = false;
			this._trailerSpeedScalar = 1f;
		}

		// Token: 0x060014F3 RID: 5363 RVA: 0x00090164 File Offset: 0x0008E364
		public void CreateSpline()
		{
			Vector3 targetPoint = this._settings.startProps.targetPoint;
			Quaternion quaternion = Quaternion.LookRotation(Vector3.forward, Vector3.up);
			Quaternion quaternion2 = Quaternion.LookRotation(Vector3.back, Vector3.up);
			List<SplineBezierPoint> list = new List<SplineBezierPoint>();
			SplineBezierPoint splineBezierPoint = new SplineBezierPoint(targetPoint, this._reversedForTrailer ? quaternion : quaternion2, this._settings.firstSplineControlDist, this._settings.firstSplineControlDist);
			list.Add(splineBezierPoint);
			Vector3 vector = Runner.instance.transform.position - targetPoint;
			Ray ray = new Ray(targetPoint, vector);
			int currentIndex = Level.currentIndex;
			for (int i = 8; i >= Level.currentIndex; i--)
			{
				float num = Level.IndexToDepth(i);
				Vector3 vector2 = Raycast.RayIntersectionWithZPlane(ray, num);
				List<IntroCameraKeyframe> introCameraKeyframes = WorldManager.instance.currentWorld.levels[i].introCameraKeyframes;
				if (introCameraKeyframes.Count != 0)
				{
					IntroCameraKeyframe introCameraKeyframe = null;
					float num2 = float.MaxValue;
					foreach (IntroCameraKeyframe introCameraKeyframe2 in introCameraKeyframes)
					{
						float num3 = Vector3.Distance(introCameraKeyframe2.transform.position, vector2);
						if (num3 < num2)
						{
							num2 = num3;
							introCameraKeyframe = introCameraKeyframe2;
						}
					}
					SplineBezierPoint splineBezierPoint2 = new SplineBezierPoint(introCameraKeyframe.transform.position, this._reversedForTrailer ? quaternion : quaternion2, this._settings.intermediateSplineControlDist, this._settings.intermediateSplineControlDist);
					list.Add(splineBezierPoint2);
				}
			}
			Vector3 position = Runner.instance.transform.position;
			Quaternion quaternion3 = Quaternion.LookRotation(this._reversedForTrailer ? (-this._settings.penultimateBezierPointDir) : this._settings.penultimateBezierPointDir, Vector3.up);
			SplineBezierPoint splineBezierPoint3 = new SplineBezierPoint(position + this._settings.penultimateBezierPointOffset, quaternion3, this._settings.penultimateSplineControlDist, this._settings.penultimateSplineControlDist);
			list.Add(splineBezierPoint3);
			SplineBezierPoint splineBezierPoint4 = new SplineBezierPoint(position, quaternion, this._settings.finalSplineControlDist, this._settings.finalSplineControlDist);
			list.Add(splineBezierPoint4);
			if (this._reversedForTrailer)
			{
				list.Reverse();
			}
			this.spline = new Spline(list.ToArray());
		}

		// Token: 0x060014F4 RID: 5364 RVA: 0x000903C4 File Offset: 0x0008E5C4
		public override void Update(bool immediate, ref HighlandCameraProperties masterCameraProperties)
		{
			if (!this.active)
			{
				return;
			}
			if (!this.introInProgress)
			{
				masterCameraProperties = (this.cameraProperties = this._settings.startProps);
				return;
			}
			float length = this.spline.length;
			if (!this._fastSwooop)
			{
				float slowdownDistance = this._settings.slowdownDistance;
			}
			else
			{
				float slowdownDistanceFast = this._settings.slowdownDistanceFast;
			}
			float num = Mathf.InverseLerp(length - this._settings.slowdownDistance, length, this._distance);
			float num2 = (this._fastSwooop ? this._settings.continueExistingGameSpeedScalar : 1f);
			num2 = Mathf.Lerp(num2, 1f, num);
			this._time += num2 * this._trailerSpeedScalar * Time.deltaTime;
			float num3 = (this._fastSwooop ? this._settings.initialAccelDurationFast : this._settings.initialAccelDuration);
			float num4 = this._time / num3;
			float num5 = Mathf.Lerp(0f, this._settings.maxSpeed, num4);
			float num6 = Mathf.Lerp(1f, this._settings.endingSlowdown, num);
			num5 *= num6;
			Vector3 directionAtArcLength = this.spline.GetDirectionAtArcLength(this._distance);
			float num7 = Mathf.Atan(Mathf.Abs(directionAtArcLength.x) / Mathf.Abs(directionAtArcLength.z)) * 57.29578f;
			float num8 = this._settings.sidewaysAngleRange.InverseLerp(num7);
			num5 *= Mathf.Lerp(1f, this._settings.sidewaysSpeedup, num8);
			num5 *= num2;
			float num9 = (this._fastSwooop ? this._settings.speedSmoothTimeFast : this._settings.speedSmoothTime);
			this._distance = Mathf.SmoothDamp(this._distance, length, ref this._speed, num9, num5, Time.deltaTime);
			Vector3 pointAtArcLength = this.spline.GetPointAtArcLength(this._distance);
			float num10 = this._distance / length;
			this.cameraProperties = HighlandCameraProperties.Lerp(this._settings.startProps, masterCameraProperties, num10);
			this.cameraProperties.targetPoint = pointAtArcLength;
			if (this._reversedForTrailer)
			{
				float num11 = Mathf.InverseLerp(5f, this._settings.blendOutDistance, this._distance);
				masterCameraProperties = HighlandCameraProperties.Lerp(masterCameraProperties, this.cameraProperties, Mathf.SmoothStep(0f, 1f, num11));
				float num12 = Mathf.InverseLerp(length - 1000f, length - 400f, this._distance);
				masterCameraProperties = HighlandCameraProperties.Lerp(masterCameraProperties, this._settings.startProps, Mathf.SmoothStep(0f, 1f, num12));
			}
			if (!this._reversedForTrailer)
			{
				float num13 = Mathf.InverseLerp(length - this._settings.blendOutDistance, length - 5f, this._distance);
				masterCameraProperties = HighlandCameraProperties.Lerp(this.cameraProperties, masterCameraProperties, Mathf.SmoothStep(0f, 1f, num13));
			}
			if (this._distance > length - 5f)
			{
				this.active = false;
				this.introInProgress = false;
				this.spline = null;
			}
		}

		// Token: 0x04001400 RID: 5120
		public bool active;

		// Token: 0x04001401 RID: 5121
		public bool introInProgress;

		// Token: 0x04001402 RID: 5122
		public Spline spline;

		// Token: 0x04001403 RID: 5123
		[SerializeField]
		private IntroCameraSettings _settings;

		// Token: 0x04001404 RID: 5124
		private float _distance;

		// Token: 0x04001405 RID: 5125
		private float _speed;

		// Token: 0x04001406 RID: 5126
		private float _time;

		// Token: 0x04001407 RID: 5127
		private bool _fastSwooop;

		// Token: 0x04001408 RID: 5128
		private float _trailerSpeedScalar = 1f;

		// Token: 0x04001409 RID: 5129
		private bool _reversedForTrailer;
	}

	// Token: 0x02000256 RID: 598
	[Serializable]
	public class MapConfirmCameraState : GameCamera.BaseCameraState
	{
		// Token: 0x060014F6 RID: 5366 RVA: 0x00090708 File Offset: 0x0008E908
		public override void Update(bool immediate, ref HighlandCameraProperties masterCameraProperties)
		{
			float cameraViewportOffsetNorm = MonoSingleton<MapsViewController>.instance.cameraViewportOffsetNorm;
			float num = 0f;
			if (cameraViewportOffsetNorm > 0f)
			{
				float num2 = 2f * (MonoSingleton<MapsViewController>.instance.reticulePosNorm.x - 0.5f);
				masterCameraProperties.viewport.x = Mathf.Lerp(masterCameraProperties.viewport.x, num2, MonoSingleton<MapsViewController>.instance.cameraViewportOffsetNorm);
				num = cameraViewportOffsetNorm;
			}
			else if (MonoSingleton<JournalController>.instance.slideOverNorm != 0f)
			{
				num = Mathf.Abs(MonoSingleton<JournalController>.instance.slideOverNorm);
				float num3 = -Mathf.Sign(MonoSingleton<JournalController>.instance.slideOverNorm);
				masterCameraProperties.viewport.x = Mathf.Lerp(masterCameraProperties.viewport.x, num3 * 0.25f, num);
				if (masterCameraProperties.distance < 40f)
				{
					masterCameraProperties.distance = Mathf.Lerp(masterCameraProperties.distance, 40f, num);
				}
			}
			masterCameraProperties.targetPoint.x = Mathf.Lerp(masterCameraProperties.targetPoint.x, Runner.instance.position.x, num);
		}
	}

	// Token: 0x02000257 RID: 599
	[Serializable]
	public class PeakCameraState : GameCamera.BaseCameraState
	{
		// Token: 0x1700048E RID: 1166
		// (get) Token: 0x060014F8 RID: 5368 RVA: 0x00090820 File Offset: 0x0008EA20
		public float panningSpeed
		{
			get
			{
				return this._peakTargetOffsetVelocity.magnitude;
			}
		}

		// Token: 0x060014F9 RID: 5369 RVA: 0x00090830 File Offset: 0x0008EA30
		public void Enter()
		{
			if (this.active)
			{
				return;
			}
			this.stateTimer = 0f;
			this.targetStrength = 1f;
			this._currentViewOffsetTarget = (this._currentViewOffset = (this._peakTargetOffsetVelocity = default(Vector2)));
			this.active = true;
		}

		// Token: 0x060014FA RID: 5370 RVA: 0x00090884 File Offset: 0x0008EA84
		public void Exit()
		{
			this.targetStrength = 0f;
			this.active = false;
		}

		// Token: 0x060014FB RID: 5371 RVA: 0x00090898 File Offset: 0x0008EA98
		public override void Reset()
		{
			this.targetStrength = 0f;
			this.active = false;
		}

		// Token: 0x060014FC RID: 5372 RVA: 0x000908AC File Offset: 0x0008EAAC
		public void ResetViewToPlayer()
		{
			this._resettingViewToPlayer = true;
		}

		// Token: 0x060014FD RID: 5373 RVA: 0x000908B8 File Offset: 0x0008EAB8
		public float CameraDistanceForPeak(bool minorPeak, float yElevation)
		{
			Range range = (minorPeak ? base.settings.peakZOffsetMinor : base.settings.peakZOffset);
			float num = GameCamera.BaseCameraState.gameCamera.settings.peakElevationRange.InverseLerp(yElevation);
			return range.Lerp(num);
		}

		// Token: 0x060014FE RID: 5374 RVA: 0x00090900 File Offset: 0x0008EB00
		public override void Update(bool immediate, ref HighlandCameraProperties masterCameraProperties)
		{
			base.strength = Mathf.SmoothDamp(base.strength, this.targetStrength, ref this.strengthVelocity, this.strengthSmoothTime, float.PositiveInfinity, Time.deltaTime);
			if (base.strength < 0.001f)
			{
				base.strength = 0f;
			}
			if (base.strength == 0f)
			{
				return;
			}
			this.stateTimer += Time.deltaTime;
			Vector2 vector = (this.isMinorPeak ? base.settings.minorPeakViewExtent : base.settings.peakViewExtent);
			Range range = new Range(-vector.x, vector.x);
			Range range2 = new Range(-vector.y, vector.y);
			if (this.peakViewExtentOverride != null)
			{
				if (this.peakViewExtentOverride.extent.min != 0f)
				{
					range.min = this.peakViewExtentOverride.extent.min;
				}
				if (this.peakViewExtentOverride.extent.max != 0f)
				{
					range.max = this.peakViewExtentOverride.extent.max;
				}
			}
			Range range3 = new Range(base.settings.peakWorldAbsoluteMaxClampX.min - GameCamera.BaseCameraState.runner.position.x, base.settings.peakWorldAbsoluteMaxClampX.max - GameCamera.BaseCameraState.runner.position.x);
			if (this.peakViewExtentOverride == null)
			{
				range.min = Mathf.Max(range.min, range3.min);
				range.max = Mathf.Min(range.max, range3.max);
			}
			if (this._resettingViewToPlayer)
			{
				this._currentViewOffsetTarget = Vector2.zero;
				this._currentViewOffset = Vector2.SmoothDamp(this._currentViewOffset, this._currentViewOffsetTarget, ref this._peakTargetOffsetVelocity, base.settings.peakAutomaticSmoothTime, base.settings.peakAutomaticMaxSpeed);
				if (Vector2.Distance(this._currentViewOffset, this._currentViewOffsetTarget) < base.settings.peakAutomaticArrivalDist && this._peakTargetOffsetVelocity.magnitude < base.settings.peakAutomaticMaxSpeed)
				{
					this._resettingViewToPlayer = false;
					this._peakTargetOffsetVelocity = Vector2.zero;
				}
			}
			if (!this._resettingViewToPlayer)
			{
				float num = 1f / this.cameraProperties.viewportScale;
				float num2 = Mathf.Max(this.cameraProperties.distance, base.settings.peakMinimumCameraDistanceForSpeed);
				float num3 = Mathf.Lerp(1f, base.settings.peakViewZoomedSpeedScalar, MonoSingleton<PeakStateController>.instance.zoomTransitionSmoothed);
				this._currentViewOffsetTarget += num2 * num * Time.deltaTime * Vector2.Scale(base.settings.peakPlayerPanSpeed * num3, this.playerPanInput);
				this._currentViewOffsetTarget.x = range.Clamp(this._currentViewOffsetTarget.x);
				this._currentViewOffsetTarget.y = range2.Clamp(this._currentViewOffsetTarget.y);
				float num4 = this.stateTimer / 2f;
				float num5 = Mathf.Lerp(2f, base.settings.peakViewPanSmoothTime, num4);
				this._currentViewOffset = Vector2.SmoothDamp(this._currentViewOffset, this._currentViewOffsetTarget, ref this._peakTargetOffsetVelocity, num5);
			}
			float num6 = GameCamera.BaseCameraState.gameCamera.settings.peakElevationRange.InverseLerp(GameCamera.BaseCameraState.runner.position.y);
			this.zoomedCameraProperties.distance = (this.unzoomedCameraProperties.distance = this.CameraDistanceForPeak(this.isMinorPeak, GameCamera.BaseCameraState.runner.position.y));
			this.zoomedCameraProperties.clipPlaneSplitPoint = (this.unzoomedCameraProperties.clipPlaneSplitPoint = 500f);
			this.zoomedCameraProperties.depthOfFieldStrength = (this.unzoomedCameraProperties.depthOfFieldStrength = 0.5f);
			float num7 = base.settings.lookUpDownRange.InverseLerp(this._currentViewOffset.y);
			Vector2 vector2 = Vector2.Lerp(base.settings.translateMoveWhenLookingDown, base.settings.translateMoveWhenLookingUp, num7);
			Vector2 vector3 = Vector2.Scale(this._currentViewOffset, vector2);
			float num8 = base.settings.peakViewOffset.Lerp(num6);
			Vector2 vector4 = new Vector2(GameCamera.BaseCameraState.runner.position.x, num8 + GameCamera.BaseCameraState.runner.position.y) + vector3;
			if (vector4.y < 20f)
			{
				vector4.y = 20f;
			}
			Vector3 vector5 = new Vector3(vector4.x, vector4.y, GameCamera.BaseCameraState.runner.visualDepth);
			this.unzoomedCameraProperties.targetPoint = (this.zoomedCameraProperties.targetPoint = vector5);
			float num9 = base.settings.peakFOV.Lerp(num6);
			this.unzoomedCameraProperties.fieldOfView = (this.zoomedCameraProperties.fieldOfView = num9);
			this.unzoomedCameraProperties.viewportScale = 1f;
			this.zoomedCameraProperties.viewportScale = num9 / MonoSingleton<PeakStateController>.instance.targetFieldOfView;
			Vector2 vector6 = Vector2.Lerp(base.settings.viewportMoveWhenLookingDown, base.settings.viewportMoveWhenLookingUp, num7);
			this.unzoomedCameraProperties.viewport = (this.zoomedCameraProperties.viewport = -0.001f * Vector2.Scale(this._currentViewOffset, vector6));
			this.zoomedCameraProperties.viewport = this.unzoomedCameraProperties.viewport * this.zoomedCameraProperties.viewportScale;
			this.zoomedCameraProperties.shearFactor = (this.unzoomedCameraProperties.shearFactor = base.settings.peakShear);
			this.cameraProperties = HighlandCameraProperties.Lerp(this.unzoomedCameraProperties, this.zoomedCameraProperties, this.playerZoomInput);
			masterCameraProperties = HighlandCameraProperties.Lerp(masterCameraProperties, this.cameraProperties, base.strength);
			float num10 = 2f * (MonoSingleton<MapsViewController>.instance.reticulePosNorm.x - 0.5f);
			masterCameraProperties.viewport.x = masterCameraProperties.viewport.x + num10 * MonoSingleton<MapsViewController>.instance.cameraViewportOffsetNorm;
		}

		// Token: 0x0400140A RID: 5130
		public HighlandCameraProperties unzoomedCameraProperties = HighlandCameraProperties.@default;

		// Token: 0x0400140B RID: 5131
		public HighlandCameraProperties zoomedCameraProperties = HighlandCameraProperties.@default;

		// Token: 0x0400140C RID: 5132
		public HighlandCameraProperties protagHeadCameraProperties = HighlandCameraProperties.@default;

		// Token: 0x0400140D RID: 5133
		[Space]
		private float stateTimer;

		// Token: 0x0400140E RID: 5134
		public bool active;

		// Token: 0x0400140F RID: 5135
		public float targetStrength;

		// Token: 0x04001410 RID: 5136
		public float strengthSmoothTime = 1f;

		// Token: 0x04001411 RID: 5137
		public float strengthVelocity;

		// Token: 0x04001412 RID: 5138
		[Space]
		public bool isMinorPeak;

		// Token: 0x04001413 RID: 5139
		public bool isTutorialPeak;

		// Token: 0x04001414 RID: 5140
		public PeakViewExtentOverride peakViewExtentOverride;

		// Token: 0x04001415 RID: 5141
		public Vector2 playerPanInput;

		// Token: 0x04001416 RID: 5142
		public float playerZoomInput;

		// Token: 0x04001417 RID: 5143
		private Vector2 _currentViewOffsetTarget;

		// Token: 0x04001418 RID: 5144
		private Vector2 _currentViewOffset;

		// Token: 0x04001419 RID: 5145
		private Vector2 _peakTargetOffsetVelocity;

		// Token: 0x0400141A RID: 5146
		private bool _resettingViewToPlayer;
	}

	// Token: 0x02000258 RID: 600
	[Serializable]
	public class PlayerZoomCameraState : GameCamera.BaseCameraState
	{
		// Token: 0x1700048F RID: 1167
		// (get) Token: 0x06001500 RID: 5376 RVA: 0x00090F71 File Offset: 0x0008F171
		public bool zooming
		{
			get
			{
				return this.activeZoomDir != 0;
			}
		}

		// Token: 0x06001501 RID: 5377 RVA: 0x00090F7C File Offset: 0x0008F17C
		public float CalculateTargetDistance(float baseDistance, int zoomDir, MaxZoom maxZoom)
		{
			float num = baseDistance;
			if (zoomDir == -1 && maxZoom == MaxZoom.Limited)
			{
				num = base.settings.limitedZoomDistance;
			}
			else if (zoomDir == -1 && maxZoom == MaxZoom.OnRidge)
			{
				num = base.settings.ridgeZoomDistance;
			}
			if (zoomDir == -1 && this.lookFurther)
			{
				num *= base.settings.lookFurtherMultiplier;
			}
			else if (zoomDir == 1)
			{
				float num2 = base.settings.zoomInSourceDistanceRange.InverseLerp(baseDistance);
				num = base.settings.zoomInTargetDistanceRange.Lerp(num2);
			}
			if (zoomDir < 0)
			{
				num = Mathf.Lerp(num, base.settings.caveZoomDistance, CaveRegion.inCaveNorm);
			}
			return num;
		}

		// Token: 0x06001502 RID: 5378 RVA: 0x00091018 File Offset: 0x0008F218
		public override void Update(bool immediate, ref HighlandCameraProperties masterCameraProperties)
		{
			GameCamera instance = GameCamera.instance;
			float num = this.CalculateTargetDistance(masterCameraProperties.distance, this.activeZoomDir, this.maxZoom) / masterCameraProperties.distance;
			if (this.activeZoomDir < -1 && num < base.settings.minPlayerZoom)
			{
				num = base.settings.minPlayerZoom;
			}
			TriggerFlags triggerFlags = PropsController.instance.triggerFlags & (TriggerFlags.VeryShortZoomOut | TriggerFlags.ShortZoomOut | TriggerFlags.MediumZoomOut);
			if ((triggerFlags & TriggerFlags.VeryShortZoomOut) != TriggerFlags.None)
			{
				num = Mathf.Lerp(1f, num, 0.1f);
			}
			else if ((triggerFlags & TriggerFlags.ShortZoomOut) != TriggerFlags.None)
			{
				num = Mathf.Lerp(1f, num, 0.25f);
			}
			else if ((triggerFlags & TriggerFlags.MediumZoomOut) != TriggerFlags.None)
			{
				num = Mathf.Lerp(1f, num, 0.5f);
			}
			if (immediate)
			{
				this._zoom = num;
			}
			else
			{
				this._zoom = GameCamera.BaseCameraState.UpdateZoom(this._zoom, num, ref this._zoomCountVelocity, base.settings.playerZoomSmoothTime, default(Range));
			}
			if (MonoSingleton<MapsViewController>.instance.cameraViewportOffsetNorm > 0f)
			{
				masterCameraProperties.viewport = new Vector2(-this.maxViewportOffset * MonoSingleton<MapsViewController>.instance.cameraViewportOffsetNorm, 0f);
			}
			masterCameraProperties.distance *= this._zoom;
			Vector2 vector = this._playerViewOffsetNorm;
			float num2;
			if (this.lookFurther)
			{
				num2 = base.settings.playerPanSmoothTime;
				float playerPanLimit = base.settings.playerPanLimit;
				float num3 = playerPanLimit * ((float)Screen.height / (float)Screen.width);
				vector.x = Mathf.Clamp(vector.x + base.settings.playerPanSpeed * GameInput.moveLeftRight * Time.deltaTime, -playerPanLimit, playerPanLimit);
				vector.y = Mathf.Clamp(vector.y + base.settings.playerPanSpeed * GameInput.moveUpDown * Time.deltaTime, -num3, num3);
			}
			else
			{
				num2 = 0.5f;
				vector = Vector2.zero;
			}
			if ((masterCameraProperties.targetPoint + masterCameraProperties.distance * vector).y < 1f)
			{
				float num4 = 1f - masterCameraProperties.targetPoint.y;
				vector.y = num4 / masterCameraProperties.distance;
			}
			this._playerViewOffsetNorm = Vector2.SmoothDamp(this._playerViewOffsetNorm, vector, ref this._playerViewOffsetNormSpeed, num2, float.MaxValue);
			masterCameraProperties.targetPoint += masterCameraProperties.distance * this._playerViewOffsetNorm;
			if (this._zoom < 1f)
			{
				masterCameraProperties.targetPoint = Vector3.Lerp(masterCameraProperties.targetPoint, Runner.instance.focus, 1f - this._zoom);
			}
		}

		// Token: 0x0400141B RID: 5147
		public int activeZoomDir;

		// Token: 0x0400141C RID: 5148
		public MaxZoom maxZoom;

		// Token: 0x0400141D RID: 5149
		public bool lookFurther;

		// Token: 0x0400141E RID: 5150
		public Vector2 playerViewOffset;

		// Token: 0x0400141F RID: 5151
		public float maxViewportOffset = 1f;

		// Token: 0x04001420 RID: 5152
		private float _zoom;

		// Token: 0x04001421 RID: 5153
		private float _zoomCountVelocity;

		// Token: 0x04001422 RID: 5154
		private Vector2 _playerViewOffsetNorm;

		// Token: 0x04001423 RID: 5155
		private Vector2 _playerViewOffsetNormSpeed;
	}

	// Token: 0x02000259 RID: 601
	[Serializable]
	public class PushVolumeCameraState : GameCamera.BaseCameraState
	{
		// Token: 0x06001504 RID: 5380 RVA: 0x000912C8 File Offset: 0x0008F4C8
		public override void Reset()
		{
			this._pushOffset = (this._pushSpeed = Vector3.zero);
		}

		// Token: 0x06001505 RID: 5381 RVA: 0x000912EC File Offset: 0x0008F4EC
		public override void Update(bool immediate, ref HighlandCameraProperties masterCameraProperties)
		{
			if (!Game.loaded || Level.current == null)
			{
				return;
			}
			Vector3 vector = Vector3.zero;
			Vector3 vector2 = masterCameraProperties.position;
			foreach (CameraPushVolume cameraPushVolume in Level.current.cameraPushVolumes)
			{
				Vector3 vector3 = cameraPushVolume.transform.InverseTransformPoint(vector2);
				if (Mathf.Abs(vector3.x) <= 1f && Mathf.Abs(vector3.y) <= 1f && Mathf.Abs(vector3.z) <= 1f)
				{
					Vector3 vector4 = (1f - vector3.y) * Vector3.up;
					Vector3 vector5 = vector3;
					vector5.y = 0f;
					float magnitude = vector5.magnitude;
					float num = 1f - Mathf.InverseLerp(0.7f, 1f, magnitude);
					vector += num * cameraPushVolume.transform.TransformVector(vector4);
					vector2 += vector;
				}
			}
			this._pushOffset = Vector3.SmoothDamp(this._pushOffset, vector, ref this._pushSpeed, 0.1f, float.MaxValue);
			masterCameraProperties.targetPoint += this._pushOffset;
		}

		// Token: 0x04001424 RID: 5156
		private Vector3 _pushSpeed;

		// Token: 0x04001425 RID: 5157
		private Vector3 _pushOffset;
	}

	// Token: 0x0200025A RID: 602
	[Serializable]
	public class RunningCameraState : GameCamera.BaseCameraState
	{
		// Token: 0x06001507 RID: 5383 RVA: 0x00091460 File Offset: 0x0008F660
		public override void Update(bool immediate, ref HighlandCameraProperties masterCameraProperties)
		{
			if (GameCamera.BaseCameraState.gameCamera.peakState.strength == 1f)
			{
				return;
			}
			Vector2 groundVelocity = GameCamera.BaseCameraState.runner.GetGroundVelocity(GameCamera.BaseCameraState.runner.velocity);
			bool slidingOrSlideJumping = GameCamera.BaseCameraState.runner.slidingOrSlideJumping;
			bool flag = GameCamera.BaseCameraState.runner.falling && GameCamera.BaseCameraState.runner.stateTimer > 0.4f;
			bool bellyWriggling = GameCamera.BaseCameraState.runner.bellyWriggling;
			bool flag2 = bellyWriggling && (!Narrative.instance.CanBellyWriggle(true) || !Narrative.instance.CanBellyWriggle(false));
			bool dead = GameCamera.BaseCameraState.runner.dead;
			bool caught = GameCamera.BaseCameraState.runner.caught;
			float signedTimeSinceMusicStart = MonoSingleton<RunTrack>.instance.signedTimeSinceMusicStart;
			bool flag3 = signedTimeSinceMusicStart > -base.settings.cameraMusicRunInitialLeadInTime && signedTimeSinceMusicStart < base.settings.cameraMusicRunInitialBigZoomDuration;
			float num;
			float num2;
			if (slidingOrSlideJumping)
			{
				num = base.settings.cameraLeadScalarSliding;
				num2 = base.settings.cameraMaxLeadSliding;
			}
			else if (flag)
			{
				num = base.settings.cameraLeadScalarFalling;
				num2 = base.settings.cameraMaxLeadFalling;
			}
			else if (caught)
			{
				num = 0f;
				num2 = 0f;
			}
			else if (GameCamera.BaseCameraState.runner.isMusicRunning && !flag3)
			{
				num = base.settings.cameraLeadScalarMusicRun;
				num2 = base.settings.cameraMaxLeadMusicRun;
			}
			else
			{
				num = base.settings.cameraLeadScalar;
				num2 = base.settings.cameraMaxLead;
			}
			float num3 = Mathf.InverseLerp(0f, GameCamera.BaseCameraState.runner.settings.run.maxStandardSpeed, GameCamera.BaseCameraState.runner.speed);
			if ((GameCamera.BaseCameraState.runner.falling && !flag) || GameCamera.BaseCameraState.runner.jumping || GameCamera.BaseCameraState.runner.transitionIsActive)
			{
				num3 = 0f;
			}
			float num4 = Mathf.Lerp(base.settings.leadSmoothTimeStopped, base.settings.leadSmoothTime, num3);
			Vector2 vector = num * groundVelocity;
			float magnitude = vector.magnitude;
			if (magnitude > num2)
			{
				float num5 = num2 / magnitude;
				vector *= num5;
			}
			if (flag3)
			{
				float num6 = 1f - Mathf.InverseLerp(-base.settings.cameraMusicRunInitialLeadInTime, base.settings.cameraMusicRunInitialBigZoomDuration, signedTimeSinceMusicStart);
				vector = Vector2.Lerp(vector, vector * base.settings.initialMusicRunLeadOffetScalar, num6);
			}
			if (immediate)
			{
				this._leadAheadOfRunOffset = vector;
				this._leadAheadOfRunVelocity = Vector2.zero;
			}
			else
			{
				this._leadAheadOfRunOffset = Vector2.SmoothDamp(this._leadAheadOfRunOffset, vector, ref this._leadAheadOfRunVelocity, num4, base.settings.cameraOffsetMaxSpeed);
			}
			float num7 = base.settings.cameraZoomInSmoothTime;
			float num8 = base.settings.cameraZoomOutSmoothTime;
			if (!slidingOrSlideJumping)
			{
				if (!GameCamera.BaseCameraState.runner.inMusicRunningArea)
				{
					float num9 = Mathf.Clamp01(GameCamera.BaseCameraState.runner.speed / GameCamera.BaseCameraState.runner.settings.run.maxStandardSpeed);
					this.targetZoom = Mathf.Lerp(1f, base.settings.cameraRunZoomMax, num9);
				}
				else if (MonoSingleton<RunTrack>.instance.readyToSprint)
				{
					this.targetZoom = base.settings.cameraMusicRunReadyToSprint;
				}
				else if (!MonoSingleton<RunTrack>.instance.playingOrRampingUp)
				{
					this.targetZoom = base.settings.cameraMusicRunAreaWideZoom;
				}
				else if (MonoSingleton<RunTrack>.instance.signedTimeSinceMusicStart < base.settings.cameraMusicRunInitialBigZoomDuration)
				{
					float num10 = MonoSingleton<RunTrack>.instance.signedTimeSinceMusicStart / base.settings.cameraMusicRunInitialBigZoomDuration;
					this.targetZoom = Mathf.Lerp(base.settings.cameraMusicRunZoom1, base.settings.cameraMusicRunZoom2, num10);
					num8 = (num7 = base.settings.cameraMusicRunSnappySmoothTime);
				}
				else
				{
					float num11 = MonoSingleton<RunTrack>.instance.signedTimeSinceMusicStart / base.settings.cameraRunZoomTimeToFullZoom;
					this.targetZoom = Mathf.Lerp(base.settings.cameraMusicRunZoom2, base.settings.cameraMusicRunZoom3, num11);
				}
			}
			else
			{
				float num12 = Mathf.Clamp01(GameCamera.BaseCameraState.runner.speed / GameCamera.BaseCameraState.runner.settings.run.maxStandardSpeed);
				this.targetZoom = Mathf.Lerp(1f, base.settings.cameraSlideZoomMax, num12);
			}
			if (flag && base.settings.fallingZoomMax > this.targetZoom)
			{
				float num13 = Mathf.InverseLerp(0.5f, 2f, GameCamera.BaseCameraState.runner.stateTimer);
				this.targetZoom = Mathf.Lerp(this.targetZoom, base.settings.fallingZoomMax, num13);
				num8 = base.settings.fallingZoomSmoothTime;
			}
			if (bellyWriggling)
			{
				this.targetZoom = (flag2 ? base.settings.bellyWriggleStuckZoom : base.settings.bellyWriggleZoom);
			}
			if (dead)
			{
				float num14 = Mathf.Max(GameCamera.BaseCameraState.runner.stateTimer - 1.5f, 0f);
				this.targetZoom += num14 * base.settings.deadZoomOutPerSecond;
			}
			float num15 = Mathf.InverseLerp(19f, 22f, GameClock.instance.hourOfDay);
			if (GameClock.instance.hourOfDay > 22f)
			{
				num15 = 1f - Mathf.InverseLerp(22f, 24f, GameClock.instance.hourOfDay);
			}
			float num16 = Mathf.Lerp(base.settings.zoomToDistance, base.settings.zoomToDistanceLate, num15);
			float num17 = ((this.targetZoom * num16 > this._zoom) ? num8 : num7);
			if (MonoSingleton<RunTrack>.instance.readyToSprint)
			{
				num17 = base.settings.cameraMusicRunReadyToSprintSmoothTime;
			}
			Vector2 vector2 = GameCamera.BaseCameraState.runner.focus;
			if (GameCamera.BaseCameraState.gameCamera.followPathState.active)
			{
				vector2 = GameCamera.BaseCameraState.gameCamera.followPathState.runnerFocusOverride;
			}
			Vector2 vector3 = vector2 + new Vector2(this._leadAheadOfRunOffset.x, this._leadAheadOfRunOffset.y * base.settings.cameraOffsetYScalar) * GameCamera.instance.debugDistanceScalar;
			if (MonoSingleton<RestStateController>.instance.active)
			{
				vector3.y -= 3f;
			}
			vector3.y = Mathf.Max(vector3.y, 0.1f);
			if (immediate)
			{
				this.cameraProperties.targetPoint = vector3;
				this.cameraProperties.targetPoint.z = GameCamera.BaseCameraState.runner.visualDepth;
				this._velocity = Vector2.zero;
				this._depthChangeSpeed = 0f;
			}
			else
			{
				float num18 = (flag ? base.settings.fallingSmoothTime : base.settings.cameraSmoothTime);
				float num19 = base.settings.cameraOffsetMaxSpeed;
				num18 *= GameCamera.instance.debugDistanceScalar;
				if (GameCamera.BaseCameraState.runner.debugFlying)
				{
					num18 = 0.05f;
					num19 = 10000f;
				}
				Vector3 vector4 = Vector2.SmoothDamp(this.cameraProperties.targetPoint, vector3, ref this._velocity, num18, Mathf.Max(this._zoom, 1f) * num19);
				vector4.z = Mathf.SmoothDamp(this.cameraProperties.targetPoint.z, GameCamera.BaseCameraState.runner.visualDepth, ref this._depthChangeSpeed, num18, float.MaxValue);
				this.cameraProperties.targetPoint = vector4;
			}
			float num20 = this.cameraProperties.distance * Mathf.Tan(0.008726646f * GameCamera.BaseCameraState.gameCamera.camera.fieldOfView);
			float num21 = (float)Screen.width / (float)Screen.height * num20;
			Vector2 vector5 = vector2 - this.cameraProperties.targetPoint;
			Vector2 vector6 = new Vector2(Mathf.Abs(vector5.x), Mathf.Abs(vector5.y));
			Range range = base.settings.goingOffscreenRange * num21;
			if (vector6.x > range.min)
			{
				float num22 = Mathf.InverseLerp(range.min, range.min + base.settings.outOfRangeClampScalar * range.length, vector5.x);
				this.cameraProperties.targetPoint.x = vector2.x - Mathf.Sign(vector5.x) * range.Lerp(Mathf.Pow(num22, base.settings.outOfRangeClampPow));
			}
			Range range2 = base.settings.goingOffscreenRange * num20;
			if (vector6.y > range2.min)
			{
				float num23 = Mathf.InverseLerp(range2.min, range2.min + base.settings.outOfRangeClampScalar * range2.length, vector5.y);
				this.cameraProperties.targetPoint.y = vector2.y - Mathf.Sign(vector5.y) * range2.Lerp(num23);
			}
			if (MonoSingleton<RestStateController>.instance.active)
			{
				this.targetZoom = base.settings.restingZoom;
				num17 = base.settings.restingZoomSmoothTime;
			}
			if (immediate)
			{
				this._zoom = this.targetZoom;
			}
			else
			{
				this._zoom = GameCamera.BaseCameraState.UpdateZoom(this._zoom, this.targetZoom, ref this._zoomCountVelocity, num17, base.settings.maxRelativeZoomCountSpeed);
			}
			this.cameraProperties.distance = num16 * this._zoom;
			masterCameraProperties = HighlandCameraProperties.Lerp(masterCameraProperties, this.cameraProperties, base.strength);
		}

		// Token: 0x04001426 RID: 5158
		private Vector2 _leadAheadOfRunOffset;

		// Token: 0x04001427 RID: 5159
		private Vector2 _leadAheadOfRunVelocity;

		// Token: 0x04001428 RID: 5160
		private float targetZoom;

		// Token: 0x04001429 RID: 5161
		private Vector2 _velocity;

		// Token: 0x0400142A RID: 5162
		private float _depthChangeSpeed;

		// Token: 0x0400142B RID: 5163
		public float _zoomCountVelocity;

		// Token: 0x0400142C RID: 5164
		public float _zoom = 1f;
	}

	// Token: 0x0200025B RID: 603
	[Serializable]
	public class ShelterCameraState : GameCamera.BaseCameraState
	{
		// Token: 0x06001509 RID: 5385 RVA: 0x00091DDD File Offset: 0x0008FFDD
		public void Enter()
		{
			this.targetStrength = 1f;
		}

		// Token: 0x0600150A RID: 5386 RVA: 0x00091DEA File Offset: 0x0008FFEA
		public void Exit()
		{
			this.targetStrength = 0f;
		}

		// Token: 0x0600150B RID: 5387 RVA: 0x00091DF8 File Offset: 0x0008FFF8
		public override void Reset()
		{
			base.strength = (this.targetStrength = 0f);
		}

		// Token: 0x0600150C RID: 5388 RVA: 0x00091E1C File Offset: 0x0009001C
		public override void Update(bool immediate, ref HighlandCameraProperties masterCameraProperties)
		{
			base.strength = Mathf.MoveTowards(base.strength, this.targetStrength, Time.deltaTime);
			this.cameraProperties.distance = base.settings.shelterZoom;
			Vector2 vector = GameCamera.BaseCameraState.runner.position + base.settings.shelterYOffset * Vector2.up;
			this.cameraProperties.targetPoint = new Vector3(vector.x, vector.y, GameCamera.BaseCameraState.runner.visualDepth);
			masterCameraProperties = HighlandCameraProperties.Lerp(masterCameraProperties, this.cameraProperties, base.strength);
		}

		// Token: 0x0400142D RID: 5165
		public float targetStrength;
	}

	// Token: 0x0200025C RID: 604
	[Serializable]
	public class StoneSkimmingCameraState : GameCamera.BaseCameraState
	{
		// Token: 0x17000490 RID: 1168
		// (get) Token: 0x0600150E RID: 5390 RVA: 0x00091ECB File Offset: 0x000900CB
		private GameCameraSettings.StoneSkimmingSettings stoneSkimmingSettings
		{
			get
			{
				return GameCamera.BaseCameraState.gameCamera.settings.stoneSkimmingSettings;
			}
		}

		// Token: 0x0600150F RID: 5391 RVA: 0x00091EDC File Offset: 0x000900DC
		public override void Reset()
		{
			base.strength = 0f;
			this.rawStrength = 0f;
		}

		// Token: 0x06001510 RID: 5392 RVA: 0x00091EF4 File Offset: 0x000900F4
		public override void Update(bool immediate, ref HighlandCameraProperties masterCameraProperties)
		{
			if (GameCamera.BaseCameraState.runner.stoneSkimming && GameCamera.BaseCameraState.runner.stoneProp != null)
			{
				TriggerZone triggerZone = GameCamera.BaseCameraState.runner.stoneProp.triggerZone;
				CameraShotGeneratorProperties cameraShotGeneratorProperties = new CameraShotGeneratorProperties();
				cameraShotGeneratorProperties.fitHorizontally = true;
				cameraShotGeneratorProperties.fitVertically = true;
				cameraShotGeneratorProperties.rotation = HighlandCameraProperties.rotation;
				cameraShotGeneratorProperties.orthographic = false;
				cameraShotGeneratorProperties.fieldOfView = this.cameraProperties.fieldOfView;
				cameraShotGeneratorProperties.zoom = this.stoneSkimmingSettings.zoom;
				float num = triggerZone.triggerRadius + ((GameCamera.BaseCameraState.runner.direction == -1f) ? this.stoneSkimmingSettings.triggerZoneFramingFrontMargin : this.stoneSkimmingSettings.triggerZoneFramingBackMargin);
				float num2 = triggerZone.triggerRadius + ((GameCamera.BaseCameraState.runner.direction == 1f) ? this.stoneSkimmingSettings.triggerZoneFramingFrontMargin : this.stoneSkimmingSettings.triggerZoneFramingBackMargin);
				cameraShotGeneratorProperties.pointCloud.Add(triggerZone.transform.position + Vector3.left * num);
				cameraShotGeneratorProperties.pointCloud.Add(triggerZone.transform.position + Vector3.right * num2);
				Vector3 vector = GameCamera.ToShot(this.cameraProperties, cameraShotGeneratorProperties, GameCamera.BaseCameraState.gameCamera.camera).position;
				float num3 = Vector3.Dot(GameCamera.BaseCameraState.runner.transform.position - vector, HighlandCameraProperties.rotation * Vector3.forward);
				vector += GameCamera.ViewportOffset(num3, this.stoneSkimmingSettings.viewportOffset, this.cameraProperties.fieldOfView, GameCamera.BaseCameraState.gameCamera.camera.aspect);
				float num4 = triggerZone.triggerRadius + ((GameCamera.BaseCameraState.runner.direction == -1f) ? this.stoneSkimmingSettings.triggerZoneFramingFrontMargin : this.stoneSkimmingSettings.triggerZoneFramingBackMargin);
				float num5 = triggerZone.triggerRadius + ((GameCamera.BaseCameraState.runner.direction == 1f) ? this.stoneSkimmingSettings.triggerZoneFramingFrontMargin : this.stoneSkimmingSettings.triggerZoneFramingBackMargin);
				if (GameCamera.BaseCameraState.runner.stoneSkimSubState == Runner.SkimStoneSubState.WindingThrow || GameCamera.BaseCameraState.runner.stoneSkimSubState == Runner.SkimStoneSubState.Throw || GameCamera.BaseCameraState.runner.stoneSkimSubState == Runner.SkimStoneSubState.FollowStone)
				{
					this._stoneWindingLerp = this.stoneSkimmingSettings.extraDistanceOverTimeHeld.Evaluate(GameCamera.BaseCameraState.runner.stoneSkimWindingTime);
				}
				else
				{
					this._stoneWindingLerp = Mathf.MoveTowards(this._stoneWindingLerp, 0f, Time.deltaTime);
				}
				if (GameCamera.BaseCameraState.runner.direction == 1f)
				{
					num5 += this._stoneWindingLerp;
				}
				else if (GameCamera.BaseCameraState.runner.direction == -1f)
				{
					num4 += this._stoneWindingLerp;
				}
				cameraShotGeneratorProperties.pointCloud.Add(triggerZone.transform.position + Vector3.left * num4);
				cameraShotGeneratorProperties.pointCloud.Add(triggerZone.transform.position + Vector3.right * num5);
				Vector3 vector2 = GameCamera.ToShot(this.cameraProperties, cameraShotGeneratorProperties, GameCamera.BaseCameraState.gameCamera.camera).position;
				float num6 = Vector3.Dot(GameCamera.BaseCameraState.runner.transform.position - vector2, HighlandCameraProperties.rotation * Vector3.forward);
				vector2 += GameCamera.ViewportOffset(num6, this.stoneSkimmingSettings.viewportOffset, this.cameraProperties.fieldOfView, GameCamera.BaseCameraState.gameCamera.camera.aspect);
				cameraShotGeneratorProperties.pointCloud.Clear();
				float num7 = triggerZone.triggerRadius + ((GameCamera.BaseCameraState.runner.direction == -1f) ? this.stoneSkimmingSettings.stoneFrontMargin : this.stoneSkimmingSettings.stoneBackMargin);
				float num8 = triggerZone.triggerRadius + ((GameCamera.BaseCameraState.runner.direction == 1f) ? this.stoneSkimmingSettings.stoneFrontMargin : this.stoneSkimmingSettings.stoneBackMargin);
				cameraShotGeneratorProperties.pointCloud.Add(GameCamera.BaseCameraState.runner.stonePosition + Vector3.left * num7);
				cameraShotGeneratorProperties.pointCloud.Add(GameCamera.BaseCameraState.runner.stonePosition + Vector3.right * num8);
				Vector3 vector3 = GameCamera.ToShot(this.cameraProperties, cameraShotGeneratorProperties, GameCamera.BaseCameraState.gameCamera.camera).position;
				float num9 = Vector3.Dot(GameCamera.BaseCameraState.runner.transform.position - vector3, HighlandCameraProperties.rotation * Vector3.forward);
				vector3 += GameCamera.ViewportOffset(num9, this.stoneSkimmingSettings.viewportOffset, this.cameraProperties.fieldOfView, GameCamera.BaseCameraState.gameCamera.camera.aspect);
				vector3 = Vector3.Lerp(vector2, vector3, this.stoneSkimmingSettings.thrownStateStrengthOverStoneDistance.Evaluate(GameCamera.BaseCameraState.runner.stoneDistance));
				Vector3 vector4 = Vector3.zero;
				if (GameCamera.BaseCameraState.runner.stoneSkimSubState == Runner.SkimStoneSubState.WindingThrow)
				{
					vector4 = vector2;
				}
				else if (GameCamera.BaseCameraState.runner.stoneSkimSubState == Runner.SkimStoneSubState.Throw || GameCamera.BaseCameraState.runner.stoneSkimSubState == Runner.SkimStoneSubState.FollowStone || GameCamera.BaseCameraState.runner.stoneSkimSubState == Runner.SkimStoneSubState.Sink)
				{
					vector4 = vector3;
				}
				else
				{
					vector4 = vector;
				}
				Vector3 vector5 = this.cameraProperties.position;
				if (base.strength > 0f)
				{
					vector5 = Vector3.SmoothDamp(this.cameraProperties.position, vector4, ref this.stoneSkimmingStateChangeVelocity, this.stoneSkimmingSettings.modeSwitchSmoothTime, float.PositiveInfinity, immediate ? 1f : Time.deltaTime);
				}
				else
				{
					vector5 = vector4;
				}
				this.cameraProperties.SetPositionAndTargetZ(vector5, GameCamera.BaseCameraState.runner.visualDepth);
			}
			if (GameCamera.BaseCameraState.runner.stoneSkimming && GameCamera.BaseCameraState.runner.stoneProp != null)
			{
				this.rawStrength = Mathf.MoveTowards(this.rawStrength, 1f, Time.deltaTime / base.settings.stoneSkimmingSettings.stateEnterSpeed);
			}
			else
			{
				this.rawStrength = Mathf.MoveTowards(this.rawStrength, 0f, Time.deltaTime / base.settings.stoneSkimmingSettings.stateExitSpeed);
			}
			base.strength = Mathf.SmoothStep(0f, 1f, this.rawStrength);
			masterCameraProperties = HighlandCameraProperties.Lerp(masterCameraProperties, this.cameraProperties, base.strength);
		}

		// Token: 0x0400142E RID: 5166
		[SerializeField]
		private float rawStrength;

		// Token: 0x0400142F RID: 5167
		public Vector3 stoneSkimmingStateChangeVelocity;

		// Token: 0x04001430 RID: 5168
		private float _stoneWindingLerp;
	}
}
