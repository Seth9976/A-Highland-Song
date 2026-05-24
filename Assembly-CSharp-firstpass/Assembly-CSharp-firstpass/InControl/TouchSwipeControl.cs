using System;
using UnityEngine;

namespace InControl
{
	// Token: 0x0200005C RID: 92
	public class TouchSwipeControl : TouchControl
	{
		// Token: 0x0600042B RID: 1067 RVA: 0x0000F9D6 File Offset: 0x0000DBD6
		public override void CreateControl()
		{
		}

		// Token: 0x0600042C RID: 1068 RVA: 0x0000F9D8 File Offset: 0x0000DBD8
		public override void DestroyControl()
		{
			if (this.currentTouch != null)
			{
				this.TouchEnded(this.currentTouch);
				this.currentTouch = null;
			}
		}

		// Token: 0x0600042D RID: 1069 RVA: 0x0000F9F5 File Offset: 0x0000DBF5
		public override void ConfigureControl()
		{
			this.worldActiveArea = TouchManager.ConvertToWorld(this.activeArea, this.areaUnitType);
		}

		// Token: 0x0600042E RID: 1070 RVA: 0x0000FA0E File Offset: 0x0000DC0E
		public override void DrawGizmos()
		{
			Utility.DrawRectGizmo(this.worldActiveArea, Color.yellow);
		}

		// Token: 0x0600042F RID: 1071 RVA: 0x0000FA20 File Offset: 0x0000DC20
		private void Update()
		{
			if (this.dirty)
			{
				this.ConfigureControl();
				this.dirty = false;
			}
		}

		// Token: 0x06000430 RID: 1072 RVA: 0x0000FA38 File Offset: 0x0000DC38
		public override void SubmitControlState(ulong updateTick, float deltaTime)
		{
			Vector3 vector = TouchControl.SnapTo(this.currentVector, this.snapAngles);
			base.SubmitAnalogValue(this.target, vector, 0f, 1f, updateTick, deltaTime);
			base.SubmitButtonState(this.upTarget, this.fireButtonTarget && this.nextButtonTarget == this.upTarget, updateTick, deltaTime);
			base.SubmitButtonState(this.downTarget, this.fireButtonTarget && this.nextButtonTarget == this.downTarget, updateTick, deltaTime);
			base.SubmitButtonState(this.leftTarget, this.fireButtonTarget && this.nextButtonTarget == this.leftTarget, updateTick, deltaTime);
			base.SubmitButtonState(this.rightTarget, this.fireButtonTarget && this.nextButtonTarget == this.rightTarget, updateTick, deltaTime);
			base.SubmitButtonState(this.tapTarget, this.fireButtonTarget && this.nextButtonTarget == this.tapTarget, updateTick, deltaTime);
			if (this.fireButtonTarget && this.nextButtonTarget != TouchControl.ButtonTarget.None)
			{
				this.fireButtonTarget = !this.oneSwipePerTouch;
				this.lastButtonTarget = this.nextButtonTarget;
				this.nextButtonTarget = TouchControl.ButtonTarget.None;
			}
		}

		// Token: 0x06000431 RID: 1073 RVA: 0x0000FB70 File Offset: 0x0000DD70
		public override void CommitControlState(ulong updateTick, float deltaTime)
		{
			base.CommitAnalog(this.target);
			base.CommitButton(this.upTarget);
			base.CommitButton(this.downTarget);
			base.CommitButton(this.leftTarget);
			base.CommitButton(this.rightTarget);
			base.CommitButton(this.tapTarget);
		}

		// Token: 0x06000432 RID: 1074 RVA: 0x0000FBC8 File Offset: 0x0000DDC8
		public override void TouchBegan(Touch touch)
		{
			if (this.currentTouch != null)
			{
				return;
			}
			this.beganPosition = TouchManager.ScreenToWorldPoint(touch.position);
			if (this.worldActiveArea.Contains(this.beganPosition))
			{
				this.lastPosition = this.beganPosition;
				this.currentTouch = touch;
				this.currentVector = Vector2.zero;
				this.currentVectorIsSet = false;
				this.fireButtonTarget = true;
				this.nextButtonTarget = TouchControl.ButtonTarget.None;
				this.lastButtonTarget = TouchControl.ButtonTarget.None;
			}
		}

		// Token: 0x06000433 RID: 1075 RVA: 0x0000FC44 File Offset: 0x0000DE44
		public override void TouchMoved(Touch touch)
		{
			if (this.currentTouch != touch)
			{
				return;
			}
			Vector3 vector = TouchManager.ScreenToWorldPoint(touch.position);
			Vector3 vector2 = vector - this.lastPosition;
			if (vector2.magnitude >= this.sensitivity)
			{
				this.lastPosition = vector;
				if (!this.oneSwipePerTouch || !this.currentVectorIsSet)
				{
					this.currentVector = vector2.normalized;
					this.currentVectorIsSet = true;
				}
				if (this.fireButtonTarget)
				{
					TouchControl.ButtonTarget buttonTargetForVector = this.GetButtonTargetForVector(this.currentVector);
					if (buttonTargetForVector != this.lastButtonTarget)
					{
						this.nextButtonTarget = buttonTargetForVector;
					}
				}
			}
		}

		// Token: 0x06000434 RID: 1076 RVA: 0x0000FCD8 File Offset: 0x0000DED8
		public override void TouchEnded(Touch touch)
		{
			if (this.currentTouch != touch)
			{
				return;
			}
			this.currentTouch = null;
			this.currentVector = Vector2.zero;
			this.currentVectorIsSet = false;
			Vector3 vector = TouchManager.ScreenToWorldPoint(touch.position);
			if ((this.beganPosition - vector).magnitude < this.sensitivity)
			{
				this.fireButtonTarget = true;
				this.nextButtonTarget = this.tapTarget;
				this.lastButtonTarget = TouchControl.ButtonTarget.None;
				return;
			}
			this.fireButtonTarget = false;
			this.nextButtonTarget = TouchControl.ButtonTarget.None;
			this.lastButtonTarget = TouchControl.ButtonTarget.None;
		}

		// Token: 0x06000435 RID: 1077 RVA: 0x0000FD68 File Offset: 0x0000DF68
		private TouchControl.ButtonTarget GetButtonTargetForVector(Vector2 vector)
		{
			Vector2 vector2 = TouchControl.SnapTo(vector, TouchControl.SnapAngles.Four);
			if (vector2 == Vector2.up)
			{
				return this.upTarget;
			}
			if (vector2 == Vector2.right)
			{
				return this.rightTarget;
			}
			if (vector2 == -Vector2.up)
			{
				return this.downTarget;
			}
			if (vector2 == -Vector2.right)
			{
				return this.leftTarget;
			}
			return TouchControl.ButtonTarget.None;
		}

		// Token: 0x17000140 RID: 320
		// (get) Token: 0x06000436 RID: 1078 RVA: 0x0000FDDD File Offset: 0x0000DFDD
		// (set) Token: 0x06000437 RID: 1079 RVA: 0x0000FDE5 File Offset: 0x0000DFE5
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

		// Token: 0x17000141 RID: 321
		// (get) Token: 0x06000438 RID: 1080 RVA: 0x0000FE03 File Offset: 0x0000E003
		// (set) Token: 0x06000439 RID: 1081 RVA: 0x0000FE0B File Offset: 0x0000E00B
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

		// Token: 0x040003E5 RID: 997
		[Header("Position")]
		[SerializeField]
		private TouchUnitType areaUnitType;

		// Token: 0x040003E6 RID: 998
		[SerializeField]
		private Rect activeArea = new Rect(25f, 25f, 50f, 50f);

		// Token: 0x040003E7 RID: 999
		[Header("Options")]
		[Range(0f, 1f)]
		public float sensitivity = 0.1f;

		// Token: 0x040003E8 RID: 1000
		public bool oneSwipePerTouch;

		// Token: 0x040003E9 RID: 1001
		[Header("Analog Target")]
		public TouchControl.AnalogTarget target;

		// Token: 0x040003EA RID: 1002
		public TouchControl.SnapAngles snapAngles;

		// Token: 0x040003EB RID: 1003
		[Header("Button Targets")]
		public TouchControl.ButtonTarget upTarget;

		// Token: 0x040003EC RID: 1004
		public TouchControl.ButtonTarget downTarget;

		// Token: 0x040003ED RID: 1005
		public TouchControl.ButtonTarget leftTarget;

		// Token: 0x040003EE RID: 1006
		public TouchControl.ButtonTarget rightTarget;

		// Token: 0x040003EF RID: 1007
		public TouchControl.ButtonTarget tapTarget;

		// Token: 0x040003F0 RID: 1008
		private Rect worldActiveArea;

		// Token: 0x040003F1 RID: 1009
		private Vector3 currentVector;

		// Token: 0x040003F2 RID: 1010
		private bool currentVectorIsSet;

		// Token: 0x040003F3 RID: 1011
		private Vector3 beganPosition;

		// Token: 0x040003F4 RID: 1012
		private Vector3 lastPosition;

		// Token: 0x040003F5 RID: 1013
		private Touch currentTouch;

		// Token: 0x040003F6 RID: 1014
		private bool fireButtonTarget;

		// Token: 0x040003F7 RID: 1015
		private TouchControl.ButtonTarget nextButtonTarget;

		// Token: 0x040003F8 RID: 1016
		private TouchControl.ButtonTarget lastButtonTarget;

		// Token: 0x040003F9 RID: 1017
		private bool dirty;
	}
}
