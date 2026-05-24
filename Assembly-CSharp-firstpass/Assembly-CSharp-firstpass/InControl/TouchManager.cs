using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace InControl
{
	// Token: 0x02000062 RID: 98
	[ExecuteInEditMode]
	public class TouchManager : SingletonMonoBehavior<TouchManager>
	{
		// Token: 0x1400000F RID: 15
		// (add) Token: 0x0600046B RID: 1131 RVA: 0x00010874 File Offset: 0x0000EA74
		// (remove) Token: 0x0600046C RID: 1132 RVA: 0x000108A8 File Offset: 0x0000EAA8
		public static event Action OnSetup;

		// Token: 0x0600046D RID: 1133 RVA: 0x000108DB File Offset: 0x0000EADB
		protected TouchManager()
		{
		}

		// Token: 0x0600046E RID: 1134 RVA: 0x00010904 File Offset: 0x0000EB04
		private void OnEnable()
		{
			if (base.GetComponent<InControlManager>() == null)
			{
				Logger.LogError("Touch Manager component can only be added to the InControl Manager object.");
				Object.DestroyImmediate(this);
				return;
			}
			if (base.EnforceSingleton)
			{
				return;
			}
			this.touchControls = base.GetComponentsInChildren<TouchControl>(true);
			if (Application.isPlaying)
			{
				InputManager.OnSetup += this.Setup;
				InputManager.OnUpdateDevices += this.UpdateDevice;
				InputManager.OnCommitDevices += this.CommitDevice;
			}
		}

		// Token: 0x0600046F RID: 1135 RVA: 0x00010980 File Offset: 0x0000EB80
		private void OnDisable()
		{
			if (Application.isPlaying)
			{
				InputManager.OnSetup -= this.Setup;
				InputManager.OnUpdateDevices -= this.UpdateDevice;
				InputManager.OnCommitDevices -= this.CommitDevice;
			}
			this.Reset();
		}

		// Token: 0x06000470 RID: 1136 RVA: 0x000109CD File Offset: 0x0000EBCD
		private void Setup()
		{
			this.UpdateScreenSize(this.GetCurrentScreenSize());
			this.CreateDevice();
			this.CreateTouches();
			if (TouchManager.OnSetup != null)
			{
				TouchManager.OnSetup();
				TouchManager.OnSetup = null;
			}
		}

		// Token: 0x06000471 RID: 1137 RVA: 0x00010A00 File Offset: 0x0000EC00
		private void Reset()
		{
			this.device = null;
			for (int i = 0; i < 3; i++)
			{
				this.mouseTouches[i] = null;
			}
			this.cachedTouches = null;
			this.activeTouches = null;
			this.readOnlyActiveTouches = null;
			this.touchControls = null;
			TouchManager.OnSetup = null;
		}

		// Token: 0x06000472 RID: 1138 RVA: 0x00010A4B File Offset: 0x0000EC4B
		private IEnumerator UpdateScreenSizeAtEndOfFrame()
		{
			yield return new WaitForEndOfFrame();
			this.UpdateScreenSize(this.GetCurrentScreenSize());
			yield return null;
			yield break;
		}

		// Token: 0x06000473 RID: 1139 RVA: 0x00010A5C File Offset: 0x0000EC5C
		private void Update()
		{
			Vector2 currentScreenSize = this.GetCurrentScreenSize();
			if (!this.isReady)
			{
				base.StartCoroutine(this.UpdateScreenSizeAtEndOfFrame());
				this.UpdateScreenSize(currentScreenSize);
				this.isReady = true;
				return;
			}
			if (this.screenSize != currentScreenSize)
			{
				this.UpdateScreenSize(currentScreenSize);
			}
			if (TouchManager.OnSetup != null)
			{
				TouchManager.OnSetup();
				TouchManager.OnSetup = null;
			}
		}

		// Token: 0x06000474 RID: 1140 RVA: 0x00010AC0 File Offset: 0x0000ECC0
		private void CreateDevice()
		{
			this.device = new TouchInputDevice();
			this.device.AddControl(InputControlType.LeftStickLeft, "LeftStickLeft");
			this.device.AddControl(InputControlType.LeftStickRight, "LeftStickRight");
			this.device.AddControl(InputControlType.LeftStickUp, "LeftStickUp");
			this.device.AddControl(InputControlType.LeftStickDown, "LeftStickDown");
			this.device.AddControl(InputControlType.RightStickLeft, "RightStickLeft");
			this.device.AddControl(InputControlType.RightStickRight, "RightStickRight");
			this.device.AddControl(InputControlType.RightStickUp, "RightStickUp");
			this.device.AddControl(InputControlType.RightStickDown, "RightStickDown");
			this.device.AddControl(InputControlType.DPadUp, "DPadUp");
			this.device.AddControl(InputControlType.DPadDown, "DPadDown");
			this.device.AddControl(InputControlType.DPadLeft, "DPadLeft");
			this.device.AddControl(InputControlType.DPadRight, "DPadRight");
			this.device.AddControl(InputControlType.LeftTrigger, "LeftTrigger");
			this.device.AddControl(InputControlType.RightTrigger, "RightTrigger");
			this.device.AddControl(InputControlType.LeftBumper, "LeftBumper");
			this.device.AddControl(InputControlType.RightBumper, "RightBumper");
			for (InputControlType inputControlType = InputControlType.Action1; inputControlType <= InputControlType.Action12; inputControlType++)
			{
				this.device.AddControl(inputControlType, inputControlType.ToString());
			}
			this.device.AddControl(InputControlType.Menu, "Menu");
			for (InputControlType inputControlType2 = InputControlType.Button0; inputControlType2 <= InputControlType.Button19; inputControlType2++)
			{
				this.device.AddControl(inputControlType2, inputControlType2.ToString());
			}
			InputManager.AttachDevice(this.device);
		}

		// Token: 0x06000475 RID: 1141 RVA: 0x00010C75 File Offset: 0x0000EE75
		private void UpdateDevice(ulong updateTick, float deltaTime)
		{
			this.UpdateTouches(updateTick, deltaTime);
			this.SubmitControlStates(updateTick, deltaTime);
		}

		// Token: 0x06000476 RID: 1142 RVA: 0x00010C87 File Offset: 0x0000EE87
		private void CommitDevice(ulong updateTick, float deltaTime)
		{
			this.CommitControlStates(updateTick, deltaTime);
		}

		// Token: 0x06000477 RID: 1143 RVA: 0x00010C94 File Offset: 0x0000EE94
		private void SubmitControlStates(ulong updateTick, float deltaTime)
		{
			int num = this.touchControls.Length;
			for (int i = 0; i < num; i++)
			{
				TouchControl touchControl = this.touchControls[i];
				if (touchControl.enabled && touchControl.gameObject.activeInHierarchy)
				{
					touchControl.SubmitControlState(updateTick, deltaTime);
				}
			}
		}

		// Token: 0x06000478 RID: 1144 RVA: 0x00010CDC File Offset: 0x0000EEDC
		private void CommitControlStates(ulong updateTick, float deltaTime)
		{
			int num = this.touchControls.Length;
			for (int i = 0; i < num; i++)
			{
				TouchControl touchControl = this.touchControls[i];
				if (touchControl.enabled && touchControl.gameObject.activeInHierarchy)
				{
					touchControl.CommitControlState(updateTick, deltaTime);
				}
			}
		}

		// Token: 0x06000479 RID: 1145 RVA: 0x00010D24 File Offset: 0x0000EF24
		private void UpdateScreenSize(Vector2 currentScreenSize)
		{
			this.touchCamera.rect = new Rect(0f, 0f, 0.99f, 1f);
			this.touchCamera.rect = new Rect(0f, 0f, 1f, 1f);
			this.screenSize = currentScreenSize;
			this.halfScreenSize = this.screenSize / 2f;
			this.viewSize = this.ConvertViewToWorldPoint(Vector2.one) * 0.02f;
			this.percentToWorld = Mathf.Min(this.viewSize.x, this.viewSize.y);
			this.halfPercentToWorld = this.percentToWorld / 2f;
			if (this.touchCamera != null)
			{
				this.halfPixelToWorld = this.touchCamera.orthographicSize / this.screenSize.y;
				this.pixelToWorld = this.halfPixelToWorld * 2f;
			}
			if (this.touchControls != null)
			{
				int num = this.touchControls.Length;
				for (int i = 0; i < num; i++)
				{
					this.touchControls[i].ConfigureControl();
				}
			}
		}

		// Token: 0x0600047A RID: 1146 RVA: 0x00010E4C File Offset: 0x0000F04C
		private void CreateTouches()
		{
			this.cachedTouches = new TouchPool();
			for (int i = 0; i < 3; i++)
			{
				this.mouseTouches[i] = new Touch();
				this.mouseTouches[i].fingerId = -2;
			}
			this.activeTouches = new List<Touch>(32);
			this.readOnlyActiveTouches = new ReadOnlyCollection<Touch>(this.activeTouches);
		}

		// Token: 0x0600047B RID: 1147 RVA: 0x00010EAC File Offset: 0x0000F0AC
		private void UpdateTouches(ulong updateTick, float deltaTime)
		{
			this.activeTouches.Clear();
			this.cachedTouches.FreeEndedTouches();
			for (int i = 0; i < 3; i++)
			{
				if (this.mouseTouches[i].SetWithMouseData(i, updateTick, deltaTime))
				{
					this.activeTouches.Add(this.mouseTouches[i]);
				}
			}
			for (int j = 0; j < Input.touchCount; j++)
			{
				Touch touch = Input.GetTouch(j);
				Touch touch2 = this.cachedTouches.FindOrCreateTouch(touch.fingerId);
				touch2.SetWithTouchData(touch, updateTick, deltaTime);
				this.activeTouches.Add(touch2);
			}
			int count = this.cachedTouches.Touches.Count;
			for (int k = 0; k < count; k++)
			{
				Touch touch3 = this.cachedTouches.Touches[k];
				if (touch3.phase != TouchPhase.Ended && touch3.updateTick != updateTick)
				{
					touch3.phase = TouchPhase.Ended;
					this.activeTouches.Add(touch3);
				}
			}
			this.InvokeTouchEvents();
		}

		// Token: 0x0600047C RID: 1148 RVA: 0x00010FA8 File Offset: 0x0000F1A8
		private void SendTouchBegan(Touch touch)
		{
			int num = this.touchControls.Length;
			for (int i = 0; i < num; i++)
			{
				TouchControl touchControl = this.touchControls[i];
				if (touchControl.enabled && touchControl.gameObject.activeInHierarchy)
				{
					touchControl.TouchBegan(touch);
				}
			}
		}

		// Token: 0x0600047D RID: 1149 RVA: 0x00010FF0 File Offset: 0x0000F1F0
		private void SendTouchMoved(Touch touch)
		{
			int num = this.touchControls.Length;
			for (int i = 0; i < num; i++)
			{
				TouchControl touchControl = this.touchControls[i];
				if (touchControl.enabled && touchControl.gameObject.activeInHierarchy)
				{
					touchControl.TouchMoved(touch);
				}
			}
		}

		// Token: 0x0600047E RID: 1150 RVA: 0x00011038 File Offset: 0x0000F238
		private void SendTouchEnded(Touch touch)
		{
			int num = this.touchControls.Length;
			for (int i = 0; i < num; i++)
			{
				TouchControl touchControl = this.touchControls[i];
				if (touchControl.enabled && touchControl.gameObject.activeInHierarchy)
				{
					touchControl.TouchEnded(touch);
				}
			}
		}

		// Token: 0x0600047F RID: 1151 RVA: 0x00011080 File Offset: 0x0000F280
		private void InvokeTouchEvents()
		{
			int count = this.activeTouches.Count;
			if (this.enableControlsOnTouch && count > 0 && !this.controlsEnabled)
			{
				TouchManager.Device.RequestActivation();
				this.controlsEnabled = true;
			}
			for (int i = 0; i < count; i++)
			{
				Touch touch = this.activeTouches[i];
				switch (touch.phase)
				{
				case TouchPhase.Began:
					this.SendTouchBegan(touch);
					break;
				case TouchPhase.Moved:
					this.SendTouchMoved(touch);
					break;
				case TouchPhase.Stationary:
					break;
				case TouchPhase.Ended:
					this.SendTouchEnded(touch);
					break;
				case TouchPhase.Canceled:
					this.SendTouchEnded(touch);
					break;
				default:
					throw new ArgumentOutOfRangeException();
				}
			}
		}

		// Token: 0x06000480 RID: 1152 RVA: 0x00011124 File Offset: 0x0000F324
		private bool TouchCameraIsValid()
		{
			return !(this.touchCamera == null) && !Utility.IsZero(this.touchCamera.orthographicSize) && (!Utility.IsZero(this.touchCamera.rect.width) || !Utility.IsZero(this.touchCamera.rect.height)) && (!Utility.IsZero(this.touchCamera.pixelRect.width) || !Utility.IsZero(this.touchCamera.pixelRect.height));
		}

		// Token: 0x06000481 RID: 1153 RVA: 0x000111C4 File Offset: 0x0000F3C4
		private Vector3 ConvertScreenToWorldPoint(Vector2 point)
		{
			if (this.TouchCameraIsValid())
			{
				return this.touchCamera.ScreenToWorldPoint(new Vector3(point.x, point.y, -this.touchCamera.transform.position.z));
			}
			return Vector3.zero;
		}

		// Token: 0x06000482 RID: 1154 RVA: 0x00011214 File Offset: 0x0000F414
		private Vector3 ConvertViewToWorldPoint(Vector2 point)
		{
			if (this.TouchCameraIsValid())
			{
				return this.touchCamera.ViewportToWorldPoint(new Vector3(point.x, point.y, -this.touchCamera.transform.position.z));
			}
			return Vector3.zero;
		}

		// Token: 0x06000483 RID: 1155 RVA: 0x00011264 File Offset: 0x0000F464
		private Vector3 ConvertScreenToViewPoint(Vector2 point)
		{
			if (this.TouchCameraIsValid())
			{
				return this.touchCamera.ScreenToViewportPoint(new Vector3(point.x, point.y, -this.touchCamera.transform.position.z));
			}
			return Vector3.zero;
		}

		// Token: 0x06000484 RID: 1156 RVA: 0x000112B1 File Offset: 0x0000F4B1
		private Vector2 GetCurrentScreenSize()
		{
			if (this.TouchCameraIsValid())
			{
				return new Vector2((float)this.touchCamera.pixelWidth, (float)this.touchCamera.pixelHeight);
			}
			return new Vector2((float)Screen.width, (float)Screen.height);
		}

		// Token: 0x17000147 RID: 327
		// (get) Token: 0x06000485 RID: 1157 RVA: 0x000112EA File Offset: 0x0000F4EA
		// (set) Token: 0x06000486 RID: 1158 RVA: 0x000112F4 File Offset: 0x0000F4F4
		public bool controlsEnabled
		{
			get
			{
				return this._controlsEnabled;
			}
			set
			{
				if (this._controlsEnabled != value)
				{
					int num = this.touchControls.Length;
					for (int i = 0; i < num; i++)
					{
						this.touchControls[i].enabled = value;
					}
					this._controlsEnabled = value;
				}
			}
		}

		// Token: 0x17000148 RID: 328
		// (get) Token: 0x06000487 RID: 1159 RVA: 0x00011334 File Offset: 0x0000F534
		public static ReadOnlyCollection<Touch> Touches
		{
			get
			{
				return SingletonMonoBehavior<TouchManager>.Instance.readOnlyActiveTouches;
			}
		}

		// Token: 0x17000149 RID: 329
		// (get) Token: 0x06000488 RID: 1160 RVA: 0x00011340 File Offset: 0x0000F540
		public static int TouchCount
		{
			get
			{
				return SingletonMonoBehavior<TouchManager>.Instance.activeTouches.Count;
			}
		}

		// Token: 0x06000489 RID: 1161 RVA: 0x00011351 File Offset: 0x0000F551
		public static Touch GetTouch(int touchIndex)
		{
			return SingletonMonoBehavior<TouchManager>.Instance.activeTouches[touchIndex];
		}

		// Token: 0x0600048A RID: 1162 RVA: 0x00011363 File Offset: 0x0000F563
		public static Touch GetTouchByFingerId(int fingerId)
		{
			return SingletonMonoBehavior<TouchManager>.Instance.cachedTouches.FindTouch(fingerId);
		}

		// Token: 0x0600048B RID: 1163 RVA: 0x00011375 File Offset: 0x0000F575
		public static Vector3 ScreenToWorldPoint(Vector2 point)
		{
			return SingletonMonoBehavior<TouchManager>.Instance.ConvertScreenToWorldPoint(point);
		}

		// Token: 0x0600048C RID: 1164 RVA: 0x00011382 File Offset: 0x0000F582
		public static Vector3 ViewToWorldPoint(Vector2 point)
		{
			return SingletonMonoBehavior<TouchManager>.Instance.ConvertViewToWorldPoint(point);
		}

		// Token: 0x0600048D RID: 1165 RVA: 0x0001138F File Offset: 0x0000F58F
		public static Vector3 ScreenToViewPoint(Vector2 point)
		{
			return SingletonMonoBehavior<TouchManager>.Instance.ConvertScreenToViewPoint(point);
		}

		// Token: 0x0600048E RID: 1166 RVA: 0x0001139C File Offset: 0x0000F59C
		public static float ConvertToWorld(float value, TouchUnitType unitType)
		{
			return value * ((unitType == TouchUnitType.Pixels) ? TouchManager.PixelToWorld : TouchManager.PercentToWorld);
		}

		// Token: 0x0600048F RID: 1167 RVA: 0x000113B0 File Offset: 0x0000F5B0
		public static Rect PercentToWorldRect(Rect rect)
		{
			return new Rect((rect.xMin - 50f) * TouchManager.ViewSize.x, (rect.yMin - 50f) * TouchManager.ViewSize.y, rect.width * TouchManager.ViewSize.x, rect.height * TouchManager.ViewSize.y);
		}

		// Token: 0x06000490 RID: 1168 RVA: 0x00011418 File Offset: 0x0000F618
		public static Rect PixelToWorldRect(Rect rect)
		{
			return new Rect(Mathf.Round(rect.xMin - TouchManager.HalfScreenSize.x) * TouchManager.PixelToWorld, Mathf.Round(rect.yMin - TouchManager.HalfScreenSize.y) * TouchManager.PixelToWorld, Mathf.Round(rect.width) * TouchManager.PixelToWorld, Mathf.Round(rect.height) * TouchManager.PixelToWorld);
		}

		// Token: 0x06000491 RID: 1169 RVA: 0x00011488 File Offset: 0x0000F688
		public static Rect ConvertToWorld(Rect rect, TouchUnitType unitType)
		{
			if (unitType != TouchUnitType.Pixels)
			{
				return TouchManager.PercentToWorldRect(rect);
			}
			return TouchManager.PixelToWorldRect(rect);
		}

		// Token: 0x1700014A RID: 330
		// (get) Token: 0x06000492 RID: 1170 RVA: 0x0001149B File Offset: 0x0000F69B
		public static Camera Camera
		{
			get
			{
				return SingletonMonoBehavior<TouchManager>.Instance.touchCamera;
			}
		}

		// Token: 0x1700014B RID: 331
		// (get) Token: 0x06000493 RID: 1171 RVA: 0x000114A7 File Offset: 0x0000F6A7
		public static InputDevice Device
		{
			get
			{
				return SingletonMonoBehavior<TouchManager>.Instance.device;
			}
		}

		// Token: 0x1700014C RID: 332
		// (get) Token: 0x06000494 RID: 1172 RVA: 0x000114B3 File Offset: 0x0000F6B3
		public static Vector3 ViewSize
		{
			get
			{
				return SingletonMonoBehavior<TouchManager>.Instance.viewSize;
			}
		}

		// Token: 0x1700014D RID: 333
		// (get) Token: 0x06000495 RID: 1173 RVA: 0x000114BF File Offset: 0x0000F6BF
		public static float PercentToWorld
		{
			get
			{
				return SingletonMonoBehavior<TouchManager>.Instance.percentToWorld;
			}
		}

		// Token: 0x1700014E RID: 334
		// (get) Token: 0x06000496 RID: 1174 RVA: 0x000114CB File Offset: 0x0000F6CB
		public static float HalfPercentToWorld
		{
			get
			{
				return SingletonMonoBehavior<TouchManager>.Instance.halfPercentToWorld;
			}
		}

		// Token: 0x1700014F RID: 335
		// (get) Token: 0x06000497 RID: 1175 RVA: 0x000114D7 File Offset: 0x0000F6D7
		public static float PixelToWorld
		{
			get
			{
				return SingletonMonoBehavior<TouchManager>.Instance.pixelToWorld;
			}
		}

		// Token: 0x17000150 RID: 336
		// (get) Token: 0x06000498 RID: 1176 RVA: 0x000114E3 File Offset: 0x0000F6E3
		public static float HalfPixelToWorld
		{
			get
			{
				return SingletonMonoBehavior<TouchManager>.Instance.halfPixelToWorld;
			}
		}

		// Token: 0x17000151 RID: 337
		// (get) Token: 0x06000499 RID: 1177 RVA: 0x000114EF File Offset: 0x0000F6EF
		public static Vector2 ScreenSize
		{
			get
			{
				return SingletonMonoBehavior<TouchManager>.Instance.screenSize;
			}
		}

		// Token: 0x17000152 RID: 338
		// (get) Token: 0x0600049A RID: 1178 RVA: 0x000114FB File Offset: 0x0000F6FB
		public static Vector2 HalfScreenSize
		{
			get
			{
				return SingletonMonoBehavior<TouchManager>.Instance.halfScreenSize;
			}
		}

		// Token: 0x17000153 RID: 339
		// (get) Token: 0x0600049B RID: 1179 RVA: 0x00011507 File Offset: 0x0000F707
		public static TouchManager.GizmoShowOption ControlsShowGizmos
		{
			get
			{
				return SingletonMonoBehavior<TouchManager>.Instance.controlsShowGizmos;
			}
		}

		// Token: 0x17000154 RID: 340
		// (get) Token: 0x0600049C RID: 1180 RVA: 0x00011513 File Offset: 0x0000F713
		// (set) Token: 0x0600049D RID: 1181 RVA: 0x0001151F File Offset: 0x0000F71F
		public static bool ControlsEnabled
		{
			get
			{
				return SingletonMonoBehavior<TouchManager>.Instance.controlsEnabled;
			}
			set
			{
				SingletonMonoBehavior<TouchManager>.Instance.controlsEnabled = value;
			}
		}

		// Token: 0x0600049E RID: 1182 RVA: 0x0001152C File Offset: 0x0000F72C
		public static implicit operator bool(TouchManager instance)
		{
			return instance != null;
		}

		// Token: 0x04000426 RID: 1062
		[Space(10f)]
		public Camera touchCamera;

		// Token: 0x04000427 RID: 1063
		public TouchManager.GizmoShowOption controlsShowGizmos = TouchManager.GizmoShowOption.Always;

		// Token: 0x04000428 RID: 1064
		[HideInInspector]
		public bool enableControlsOnTouch;

		// Token: 0x04000429 RID: 1065
		[SerializeField]
		[HideInInspector]
		private bool _controlsEnabled = true;

		// Token: 0x0400042A RID: 1066
		[HideInInspector]
		public int controlsLayer = 5;

		// Token: 0x0400042C RID: 1068
		private InputDevice device;

		// Token: 0x0400042D RID: 1069
		private Vector3 viewSize;

		// Token: 0x0400042E RID: 1070
		private Vector2 screenSize;

		// Token: 0x0400042F RID: 1071
		private Vector2 halfScreenSize;

		// Token: 0x04000430 RID: 1072
		private float percentToWorld;

		// Token: 0x04000431 RID: 1073
		private float halfPercentToWorld;

		// Token: 0x04000432 RID: 1074
		private float pixelToWorld;

		// Token: 0x04000433 RID: 1075
		private float halfPixelToWorld;

		// Token: 0x04000434 RID: 1076
		private TouchControl[] touchControls;

		// Token: 0x04000435 RID: 1077
		private TouchPool cachedTouches;

		// Token: 0x04000436 RID: 1078
		private List<Touch> activeTouches;

		// Token: 0x04000437 RID: 1079
		private ReadOnlyCollection<Touch> readOnlyActiveTouches;

		// Token: 0x04000438 RID: 1080
		private bool isReady;

		// Token: 0x04000439 RID: 1081
		private readonly Touch[] mouseTouches = new Touch[3];

		// Token: 0x02000222 RID: 546
		public enum GizmoShowOption
		{
			// Token: 0x04000503 RID: 1283
			Never,
			// Token: 0x04000504 RID: 1284
			WhenSelected,
			// Token: 0x04000505 RID: 1285
			UnlessPlaying,
			// Token: 0x04000506 RID: 1286
			Always
		}
	}
}
