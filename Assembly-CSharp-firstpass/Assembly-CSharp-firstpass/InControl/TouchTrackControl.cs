using System;
using UnityEngine;

namespace InControl
{
	// Token: 0x0200005D RID: 93
	public class TouchTrackControl : TouchControl
	{
		// Token: 0x0600043B RID: 1083 RVA: 0x0000FE56 File Offset: 0x0000E056
		public override void CreateControl()
		{
			this.ConfigureControl();
		}

		// Token: 0x0600043C RID: 1084 RVA: 0x0000FE5E File Offset: 0x0000E05E
		public override void DestroyControl()
		{
			if (this.currentTouch != null)
			{
				this.TouchEnded(this.currentTouch);
				this.currentTouch = null;
			}
		}

		// Token: 0x0600043D RID: 1085 RVA: 0x0000FE7B File Offset: 0x0000E07B
		public override void ConfigureControl()
		{
			this.worldActiveArea = TouchManager.ConvertToWorld(this.activeArea, this.areaUnitType);
		}

		// Token: 0x0600043E RID: 1086 RVA: 0x0000FE94 File Offset: 0x0000E094
		public override void DrawGizmos()
		{
			Utility.DrawRectGizmo(this.worldActiveArea, Color.yellow);
		}

		// Token: 0x0600043F RID: 1087 RVA: 0x0000FEA6 File Offset: 0x0000E0A6
		private void OnValidate()
		{
			if (this.maxTapDuration < 0f)
			{
				this.maxTapDuration = 0f;
			}
		}

		// Token: 0x06000440 RID: 1088 RVA: 0x0000FEC0 File Offset: 0x0000E0C0
		private void Update()
		{
			if (this.dirty)
			{
				this.ConfigureControl();
				this.dirty = false;
			}
		}

		// Token: 0x06000441 RID: 1089 RVA: 0x0000FED8 File Offset: 0x0000E0D8
		public override void SubmitControlState(ulong updateTick, float deltaTime)
		{
			Vector3 vector = this.thisPosition - this.lastPosition;
			base.SubmitRawAnalogValue(this.target, vector * this.scale, updateTick, deltaTime);
			this.lastPosition = this.thisPosition;
			base.SubmitButtonState(this.tapTarget, this.fireButtonTarget, updateTick, deltaTime);
			this.fireButtonTarget = false;
		}

		// Token: 0x06000442 RID: 1090 RVA: 0x0000FF3D File Offset: 0x0000E13D
		public override void CommitControlState(ulong updateTick, float deltaTime)
		{
			base.CommitAnalog(this.target);
			base.CommitButton(this.tapTarget);
		}

		// Token: 0x06000443 RID: 1091 RVA: 0x0000FF58 File Offset: 0x0000E158
		public override void TouchBegan(Touch touch)
		{
			if (this.currentTouch != null)
			{
				return;
			}
			this.beganPosition = TouchManager.ScreenToWorldPoint(touch.position);
			if (this.worldActiveArea.Contains(this.beganPosition))
			{
				this.thisPosition = TouchManager.ScreenToViewPoint(touch.position * 100f);
				this.lastPosition = this.thisPosition;
				this.currentTouch = touch;
				this.beganTime = Time.realtimeSinceStartup;
			}
		}

		// Token: 0x06000444 RID: 1092 RVA: 0x0000FFCB File Offset: 0x0000E1CB
		public override void TouchMoved(Touch touch)
		{
			if (this.currentTouch != touch)
			{
				return;
			}
			this.thisPosition = TouchManager.ScreenToViewPoint(touch.position * 100f);
		}

		// Token: 0x06000445 RID: 1093 RVA: 0x0000FFF4 File Offset: 0x0000E1F4
		public override void TouchEnded(Touch touch)
		{
			if (this.currentTouch != touch)
			{
				return;
			}
			Vector3 vector = TouchManager.ScreenToWorldPoint(touch.position) - this.beganPosition;
			float num = Time.realtimeSinceStartup - this.beganTime;
			if (vector.magnitude <= this.maxTapMovement && num <= this.maxTapDuration && this.tapTarget != TouchControl.ButtonTarget.None)
			{
				this.fireButtonTarget = true;
			}
			this.thisPosition = Vector3.zero;
			this.lastPosition = Vector3.zero;
			this.currentTouch = null;
		}

		// Token: 0x17000142 RID: 322
		// (get) Token: 0x06000446 RID: 1094 RVA: 0x00010073 File Offset: 0x0000E273
		// (set) Token: 0x06000447 RID: 1095 RVA: 0x0001007B File Offset: 0x0000E27B
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

		// Token: 0x17000143 RID: 323
		// (get) Token: 0x06000448 RID: 1096 RVA: 0x00010099 File Offset: 0x0000E299
		// (set) Token: 0x06000449 RID: 1097 RVA: 0x000100A1 File Offset: 0x0000E2A1
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

		// Token: 0x040003FA RID: 1018
		[Header("Dimensions")]
		[SerializeField]
		private TouchUnitType areaUnitType;

		// Token: 0x040003FB RID: 1019
		[SerializeField]
		private Rect activeArea = new Rect(25f, 25f, 50f, 50f);

		// Token: 0x040003FC RID: 1020
		[Header("Analog Target")]
		public TouchControl.AnalogTarget target = TouchControl.AnalogTarget.LeftStick;

		// Token: 0x040003FD RID: 1021
		public float scale = 1f;

		// Token: 0x040003FE RID: 1022
		[Header("Button Target")]
		public TouchControl.ButtonTarget tapTarget;

		// Token: 0x040003FF RID: 1023
		public float maxTapDuration = 0.5f;

		// Token: 0x04000400 RID: 1024
		public float maxTapMovement = 1f;

		// Token: 0x04000401 RID: 1025
		private Rect worldActiveArea;

		// Token: 0x04000402 RID: 1026
		private Vector3 lastPosition;

		// Token: 0x04000403 RID: 1027
		private Vector3 thisPosition;

		// Token: 0x04000404 RID: 1028
		private Touch currentTouch;

		// Token: 0x04000405 RID: 1029
		private bool dirty;

		// Token: 0x04000406 RID: 1030
		private bool fireButtonTarget;

		// Token: 0x04000407 RID: 1031
		private float beganTime;

		// Token: 0x04000408 RID: 1032
		private Vector3 beganPosition;
	}
}
