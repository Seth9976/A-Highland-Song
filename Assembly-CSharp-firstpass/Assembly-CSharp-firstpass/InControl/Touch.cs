using System;
using UnityEngine;

namespace InControl
{
	// Token: 0x0200005E RID: 94
	public class Touch
	{
		// Token: 0x0600044B RID: 1099 RVA: 0x00010116 File Offset: 0x0000E316
		internal Touch()
		{
			this.fingerId = -1;
			this.phase = TouchPhase.Ended;
		}

		// Token: 0x0600044C RID: 1100 RVA: 0x0001012C File Offset: 0x0000E32C
		internal void Reset()
		{
			this.fingerId = -1;
			this.mouseButton = 0;
			this.phase = TouchPhase.Ended;
			this.tapCount = 0;
			this.position = Vector2.zero;
			this.startPosition = Vector2.zero;
			this.deltaPosition = Vector2.zero;
			this.lastPosition = Vector2.zero;
			this.deltaTime = 0f;
			this.updateTick = 0UL;
			this.type = TouchType.Direct;
			this.altitudeAngle = 0f;
			this.azimuthAngle = 0f;
			this.maximumPossiblePressure = 1f;
			this.pressure = 0f;
			this.radius = 0f;
			this.radiusVariance = 0f;
		}

		// Token: 0x17000144 RID: 324
		// (get) Token: 0x0600044D RID: 1101 RVA: 0x000101DD File Offset: 0x0000E3DD
		[Obsolete("normalizedPressure is deprecated, please use NormalizedPressure instead.")]
		public float normalizedPressure
		{
			get
			{
				return Mathf.Clamp(this.pressure / this.maximumPossiblePressure, 0.001f, 1f);
			}
		}

		// Token: 0x17000145 RID: 325
		// (get) Token: 0x0600044E RID: 1102 RVA: 0x000101FB File Offset: 0x0000E3FB
		public float NormalizedPressure
		{
			get
			{
				return Mathf.Clamp(this.pressure / this.maximumPossiblePressure, 0.001f, 1f);
			}
		}

		// Token: 0x17000146 RID: 326
		// (get) Token: 0x0600044F RID: 1103 RVA: 0x00010219 File Offset: 0x0000E419
		public bool IsMouse
		{
			get
			{
				return this.type == TouchType.Mouse;
			}
		}

		// Token: 0x06000450 RID: 1104 RVA: 0x00010224 File Offset: 0x0000E424
		internal void SetWithTouchData(Touch touch, ulong updateTick, float deltaTime)
		{
			this.phase = touch.phase;
			this.tapCount = touch.tapCount;
			this.mouseButton = 0;
			this.altitudeAngle = touch.altitudeAngle;
			this.azimuthAngle = touch.azimuthAngle;
			this.maximumPossiblePressure = touch.maximumPossiblePressure;
			this.pressure = touch.pressure;
			this.radius = touch.radius;
			this.radiusVariance = touch.radiusVariance;
			Vector2 vector = touch.position;
			vector.x = Mathf.Clamp(vector.x, 0f, (float)Screen.width);
			vector.y = Mathf.Clamp(vector.y, 0f, (float)Screen.height);
			if (this.phase == TouchPhase.Began)
			{
				this.startPosition = vector;
				this.deltaPosition = Vector2.zero;
				this.lastPosition = vector;
				this.position = vector;
			}
			else
			{
				if (this.phase == TouchPhase.Stationary)
				{
					this.phase = TouchPhase.Moved;
				}
				this.deltaPosition = vector - this.lastPosition;
				this.lastPosition = this.position;
				this.position = vector;
			}
			this.deltaTime = deltaTime;
			this.updateTick = updateTick;
		}

		// Token: 0x06000451 RID: 1105 RVA: 0x00010350 File Offset: 0x0000E550
		internal bool SetWithMouseData(int button, ulong updateTick, float deltaTime)
		{
			if (Input.touchCount > 0)
			{
				return false;
			}
			if (button < 0 || button > 2)
			{
				return false;
			}
			Vector2 vector = InputManager.MouseProvider.GetPosition();
			Vector2 vector2 = new Vector2(Mathf.Round(vector.x), Mathf.Round(vector.y));
			Mouse mouse = Mouse.LeftButton + button;
			if (InputManager.MouseProvider.GetButtonWasPressed(mouse))
			{
				this.phase = TouchPhase.Began;
				this.pressure = 1f;
				this.maximumPossiblePressure = 1f;
				this.tapCount = 1;
				this.type = TouchType.Mouse;
				this.mouseButton = button;
				this.startPosition = vector2;
				this.deltaPosition = Vector2.zero;
				this.lastPosition = vector2;
				this.position = vector2;
				this.deltaTime = deltaTime;
				this.updateTick = updateTick;
				return true;
			}
			if (InputManager.MouseProvider.GetButtonWasReleased(mouse))
			{
				this.phase = TouchPhase.Ended;
				this.pressure = 0f;
				this.maximumPossiblePressure = 1f;
				this.tapCount = 1;
				this.type = TouchType.Mouse;
				this.mouseButton = button;
				this.deltaPosition = vector2 - this.lastPosition;
				this.lastPosition = this.position;
				this.position = vector2;
				this.deltaTime = deltaTime;
				this.updateTick = updateTick;
				return true;
			}
			if (InputManager.MouseProvider.GetButtonIsPressed(mouse))
			{
				this.phase = TouchPhase.Moved;
				this.pressure = 1f;
				this.maximumPossiblePressure = 1f;
				this.tapCount = 1;
				this.type = TouchType.Mouse;
				this.mouseButton = button;
				this.deltaPosition = vector2 - this.lastPosition;
				this.lastPosition = this.position;
				this.position = vector2;
				this.deltaTime = deltaTime;
				this.updateTick = updateTick;
				return true;
			}
			return false;
		}

		// Token: 0x04000409 RID: 1033
		public const int FingerID_None = -1;

		// Token: 0x0400040A RID: 1034
		public const int FingerID_Mouse = -2;

		// Token: 0x0400040B RID: 1035
		public int fingerId;

		// Token: 0x0400040C RID: 1036
		public int mouseButton;

		// Token: 0x0400040D RID: 1037
		public TouchPhase phase;

		// Token: 0x0400040E RID: 1038
		public int tapCount;

		// Token: 0x0400040F RID: 1039
		public Vector2 position;

		// Token: 0x04000410 RID: 1040
		public Vector2 startPosition;

		// Token: 0x04000411 RID: 1041
		public Vector2 deltaPosition;

		// Token: 0x04000412 RID: 1042
		public Vector2 lastPosition;

		// Token: 0x04000413 RID: 1043
		public float deltaTime;

		// Token: 0x04000414 RID: 1044
		public ulong updateTick;

		// Token: 0x04000415 RID: 1045
		public TouchType type;

		// Token: 0x04000416 RID: 1046
		public float altitudeAngle;

		// Token: 0x04000417 RID: 1047
		public float azimuthAngle;

		// Token: 0x04000418 RID: 1048
		public float maximumPossiblePressure;

		// Token: 0x04000419 RID: 1049
		public float pressure;

		// Token: 0x0400041A RID: 1050
		public float radius;

		// Token: 0x0400041B RID: 1051
		public float radiusVariance;
	}
}
