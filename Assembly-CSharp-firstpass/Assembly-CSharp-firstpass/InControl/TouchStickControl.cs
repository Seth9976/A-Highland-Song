using System;
using UnityEngine;

namespace InControl
{
	// Token: 0x0200005B RID: 91
	public class TouchStickControl : TouchControl
	{
		// Token: 0x06000410 RID: 1040 RVA: 0x0000F179 File Offset: 0x0000D379
		public override void CreateControl()
		{
			this.ring.Create("Ring", base.transform, 1000);
			this.knob.Create("Knob", base.transform, 1001);
		}

		// Token: 0x06000411 RID: 1041 RVA: 0x0000F1B1 File Offset: 0x0000D3B1
		public override void DestroyControl()
		{
			this.ring.Delete();
			this.knob.Delete();
			if (this.currentTouch != null)
			{
				this.TouchEnded(this.currentTouch);
				this.currentTouch = null;
			}
		}

		// Token: 0x06000412 RID: 1042 RVA: 0x0000F1E4 File Offset: 0x0000D3E4
		public override void ConfigureControl()
		{
			this.resetPosition = base.OffsetToWorldPosition(this.anchor, this.offset, this.offsetUnitType, true);
			base.transform.position = this.resetPosition;
			this.ring.Update(true);
			this.knob.Update(true);
			this.worldActiveArea = TouchManager.ConvertToWorld(this.activeArea, this.areaUnitType);
			this.worldKnobRange = TouchManager.ConvertToWorld(this.knobRange, this.knob.SizeUnitType);
		}

		// Token: 0x06000413 RID: 1043 RVA: 0x0000F26C File Offset: 0x0000D46C
		public override void DrawGizmos()
		{
			this.ring.DrawGizmos(this.RingPosition, Color.yellow);
			this.knob.DrawGizmos(this.KnobPosition, Color.yellow);
			Utility.DrawCircleGizmo(this.RingPosition, this.worldKnobRange, Color.red);
			Utility.DrawRectGizmo(this.worldActiveArea, Color.green);
		}

		// Token: 0x06000414 RID: 1044 RVA: 0x0000F2D0 File Offset: 0x0000D4D0
		private void Update()
		{
			if (this.dirty)
			{
				this.ConfigureControl();
				this.dirty = false;
			}
			else
			{
				this.ring.Update();
				this.knob.Update();
			}
			if (this.IsNotActive)
			{
				if (this.resetWhenDone && this.KnobPosition != this.resetPosition)
				{
					Vector3 vector = this.KnobPosition - this.RingPosition;
					this.RingPosition = Vector3.MoveTowards(this.RingPosition, this.resetPosition, this.ringResetSpeed * Time.unscaledDeltaTime);
					this.KnobPosition = this.RingPosition + vector;
				}
				if (this.KnobPosition != this.RingPosition)
				{
					this.KnobPosition = Vector3.MoveTowards(this.KnobPosition, this.RingPosition, this.knobResetSpeed * Time.unscaledDeltaTime);
				}
			}
		}

		// Token: 0x06000415 RID: 1045 RVA: 0x0000F3AD File Offset: 0x0000D5AD
		public override void SubmitControlState(ulong updateTick, float deltaTime)
		{
			base.SubmitAnalogValue(this.target, this.value, this.lowerDeadZone, this.upperDeadZone, updateTick, deltaTime);
		}

		// Token: 0x06000416 RID: 1046 RVA: 0x0000F3D4 File Offset: 0x0000D5D4
		public override void CommitControlState(ulong updateTick, float deltaTime)
		{
			base.CommitAnalog(this.target);
		}

		// Token: 0x06000417 RID: 1047 RVA: 0x0000F3E4 File Offset: 0x0000D5E4
		public override void TouchBegan(Touch touch)
		{
			if (this.IsActive)
			{
				return;
			}
			this.beganPosition = TouchManager.ScreenToWorldPoint(touch.position);
			bool flag = this.worldActiveArea.Contains(this.beganPosition);
			bool flag2 = this.ring.Contains(this.beganPosition);
			if (this.snapToInitialTouch && (flag || flag2))
			{
				this.RingPosition = this.beganPosition;
				this.KnobPosition = this.beganPosition;
				this.currentTouch = touch;
			}
			else if (flag2)
			{
				this.KnobPosition = this.beganPosition;
				this.beganPosition = this.RingPosition;
				this.currentTouch = touch;
			}
			if (this.IsActive)
			{
				this.TouchMoved(touch);
				this.ring.State = true;
				this.knob.State = true;
			}
		}

		// Token: 0x06000418 RID: 1048 RVA: 0x0000F4AC File Offset: 0x0000D6AC
		public override void TouchMoved(Touch touch)
		{
			if (this.currentTouch != touch)
			{
				return;
			}
			this.movedPosition = TouchManager.ScreenToWorldPoint(touch.position);
			if (this.lockToAxis == LockAxis.Horizontal && this.allowDraggingAxis == DragAxis.Horizontal)
			{
				this.movedPosition.y = this.beganPosition.y;
			}
			else if (this.lockToAxis == LockAxis.Vertical && this.allowDraggingAxis == DragAxis.Vertical)
			{
				this.movedPosition.x = this.beganPosition.x;
			}
			Vector3 vector = this.movedPosition - this.beganPosition;
			Vector3 normalized = vector.normalized;
			float magnitude = vector.magnitude;
			if (this.allowDragging)
			{
				float num = magnitude - this.worldKnobRange;
				if (num < 0f)
				{
					num = 0f;
				}
				Vector3 vector2 = num * normalized;
				if (this.allowDraggingAxis == DragAxis.Horizontal)
				{
					vector2.y = 0f;
				}
				else if (this.allowDraggingAxis == DragAxis.Vertical)
				{
					vector2.x = 0f;
				}
				this.beganPosition += vector2;
				this.RingPosition = this.beganPosition;
			}
			this.movedPosition = this.beganPosition + Mathf.Clamp(magnitude, 0f, this.worldKnobRange) * normalized;
			if (this.lockToAxis == LockAxis.Horizontal)
			{
				this.movedPosition.y = this.beganPosition.y;
			}
			else if (this.lockToAxis == LockAxis.Vertical)
			{
				this.movedPosition.x = this.beganPosition.x;
			}
			if (this.snapAngles != TouchControl.SnapAngles.None)
			{
				this.movedPosition = TouchControl.SnapTo(this.movedPosition - this.beganPosition, this.snapAngles) + this.beganPosition;
			}
			this.RingPosition = this.beganPosition;
			this.KnobPosition = this.movedPosition;
			this.value = (this.movedPosition - this.beganPosition) / this.worldKnobRange;
			this.value.x = this.inputCurve.Evaluate(Utility.Abs(this.value.x)) * Mathf.Sign(this.value.x);
			this.value.y = this.inputCurve.Evaluate(Utility.Abs(this.value.y)) * Mathf.Sign(this.value.y);
		}

		// Token: 0x06000419 RID: 1049 RVA: 0x0000F708 File Offset: 0x0000D908
		public override void TouchEnded(Touch touch)
		{
			if (this.currentTouch != touch)
			{
				return;
			}
			this.value = Vector3.zero;
			float magnitude = (this.resetPosition - this.RingPosition).magnitude;
			this.ringResetSpeed = (Utility.IsZero(this.resetDuration) ? magnitude : (magnitude / this.resetDuration));
			float magnitude2 = (this.RingPosition - this.KnobPosition).magnitude;
			this.knobResetSpeed = (Utility.IsZero(this.resetDuration) ? this.knobRange : (magnitude2 / this.resetDuration));
			this.currentTouch = null;
			this.ring.State = false;
			this.knob.State = false;
		}

		// Token: 0x17000137 RID: 311
		// (get) Token: 0x0600041A RID: 1050 RVA: 0x0000F7BE File Offset: 0x0000D9BE
		public bool IsActive
		{
			get
			{
				return this.currentTouch != null;
			}
		}

		// Token: 0x17000138 RID: 312
		// (get) Token: 0x0600041B RID: 1051 RVA: 0x0000F7C9 File Offset: 0x0000D9C9
		public bool IsNotActive
		{
			get
			{
				return this.currentTouch == null;
			}
		}

		// Token: 0x17000139 RID: 313
		// (get) Token: 0x0600041C RID: 1052 RVA: 0x0000F7D4 File Offset: 0x0000D9D4
		// (set) Token: 0x0600041D RID: 1053 RVA: 0x0000F7FA File Offset: 0x0000D9FA
		public Vector3 RingPosition
		{
			get
			{
				if (!this.ring.Ready)
				{
					return base.transform.position;
				}
				return this.ring.Position;
			}
			set
			{
				if (this.ring.Ready)
				{
					this.ring.Position = value;
				}
			}
		}

		// Token: 0x1700013A RID: 314
		// (get) Token: 0x0600041E RID: 1054 RVA: 0x0000F815 File Offset: 0x0000DA15
		// (set) Token: 0x0600041F RID: 1055 RVA: 0x0000F83B File Offset: 0x0000DA3B
		public Vector3 KnobPosition
		{
			get
			{
				if (!this.knob.Ready)
				{
					return base.transform.position;
				}
				return this.knob.Position;
			}
			set
			{
				if (this.knob.Ready)
				{
					this.knob.Position = value;
				}
			}
		}

		// Token: 0x1700013B RID: 315
		// (get) Token: 0x06000420 RID: 1056 RVA: 0x0000F856 File Offset: 0x0000DA56
		// (set) Token: 0x06000421 RID: 1057 RVA: 0x0000F85E File Offset: 0x0000DA5E
		public TouchControlAnchor Anchor
		{
			get
			{
				return this.anchor;
			}
			set
			{
				if (this.anchor != value)
				{
					this.anchor = value;
					this.dirty = true;
				}
			}
		}

		// Token: 0x1700013C RID: 316
		// (get) Token: 0x06000422 RID: 1058 RVA: 0x0000F877 File Offset: 0x0000DA77
		// (set) Token: 0x06000423 RID: 1059 RVA: 0x0000F87F File Offset: 0x0000DA7F
		public Vector2 Offset
		{
			get
			{
				return this.offset;
			}
			set
			{
				if (this.offset != value)
				{
					this.offset = value;
					this.dirty = true;
				}
			}
		}

		// Token: 0x1700013D RID: 317
		// (get) Token: 0x06000424 RID: 1060 RVA: 0x0000F89D File Offset: 0x0000DA9D
		// (set) Token: 0x06000425 RID: 1061 RVA: 0x0000F8A5 File Offset: 0x0000DAA5
		public TouchUnitType OffsetUnitType
		{
			get
			{
				return this.offsetUnitType;
			}
			set
			{
				if (this.offsetUnitType != value)
				{
					this.offsetUnitType = value;
					this.dirty = true;
				}
			}
		}

		// Token: 0x1700013E RID: 318
		// (get) Token: 0x06000426 RID: 1062 RVA: 0x0000F8BE File Offset: 0x0000DABE
		// (set) Token: 0x06000427 RID: 1063 RVA: 0x0000F8C6 File Offset: 0x0000DAC6
		public Rect ActiveArea
		{
			get
			{
				return this.activeArea;
			}
			set
			{
				if (this.activeArea != value)
				{
					this.activeArea = value;
					this.dirty = true;
				}
			}
		}

		// Token: 0x1700013F RID: 319
		// (get) Token: 0x06000428 RID: 1064 RVA: 0x0000F8E4 File Offset: 0x0000DAE4
		// (set) Token: 0x06000429 RID: 1065 RVA: 0x0000F8EC File Offset: 0x0000DAEC
		public TouchUnitType AreaUnitType
		{
			get
			{
				return this.areaUnitType;
			}
			set
			{
				if (this.areaUnitType != value)
				{
					this.areaUnitType = value;
					this.dirty = true;
				}
			}
		}

		// Token: 0x040003C8 RID: 968
		[Header("Position")]
		[SerializeField]
		private TouchControlAnchor anchor = TouchControlAnchor.BottomLeft;

		// Token: 0x040003C9 RID: 969
		[SerializeField]
		private TouchUnitType offsetUnitType;

		// Token: 0x040003CA RID: 970
		[SerializeField]
		private Vector2 offset = new Vector2(20f, 20f);

		// Token: 0x040003CB RID: 971
		[SerializeField]
		private TouchUnitType areaUnitType;

		// Token: 0x040003CC RID: 972
		[SerializeField]
		private Rect activeArea = new Rect(0f, 0f, 50f, 100f);

		// Token: 0x040003CD RID: 973
		[Header("Options")]
		public TouchControl.AnalogTarget target = TouchControl.AnalogTarget.LeftStick;

		// Token: 0x040003CE RID: 974
		public TouchControl.SnapAngles snapAngles;

		// Token: 0x040003CF RID: 975
		public LockAxis lockToAxis;

		// Token: 0x040003D0 RID: 976
		[Range(0f, 1f)]
		public float lowerDeadZone = 0.1f;

		// Token: 0x040003D1 RID: 977
		[Range(0f, 1f)]
		public float upperDeadZone = 0.9f;

		// Token: 0x040003D2 RID: 978
		public AnimationCurve inputCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);

		// Token: 0x040003D3 RID: 979
		public bool allowDragging;

		// Token: 0x040003D4 RID: 980
		public DragAxis allowDraggingAxis;

		// Token: 0x040003D5 RID: 981
		public bool snapToInitialTouch = true;

		// Token: 0x040003D6 RID: 982
		public bool resetWhenDone = true;

		// Token: 0x040003D7 RID: 983
		public float resetDuration = 0.1f;

		// Token: 0x040003D8 RID: 984
		[Header("Sprites")]
		public TouchSprite ring = new TouchSprite(20f);

		// Token: 0x040003D9 RID: 985
		public TouchSprite knob = new TouchSprite(10f);

		// Token: 0x040003DA RID: 986
		public float knobRange = 7.5f;

		// Token: 0x040003DB RID: 987
		private Vector3 resetPosition;

		// Token: 0x040003DC RID: 988
		private Vector3 beganPosition;

		// Token: 0x040003DD RID: 989
		private Vector3 movedPosition;

		// Token: 0x040003DE RID: 990
		private float ringResetSpeed;

		// Token: 0x040003DF RID: 991
		private float knobResetSpeed;

		// Token: 0x040003E0 RID: 992
		private Rect worldActiveArea;

		// Token: 0x040003E1 RID: 993
		private float worldKnobRange;

		// Token: 0x040003E2 RID: 994
		private Vector3 value;

		// Token: 0x040003E3 RID: 995
		private Touch currentTouch;

		// Token: 0x040003E4 RID: 996
		private bool dirty;
	}
}
