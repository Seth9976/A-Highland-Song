using System;
using UnityEngine;

namespace InControl
{
	// Token: 0x02000058 RID: 88
	public class TouchButtonControl : TouchControl
	{
		// Token: 0x060003FB RID: 1019 RVA: 0x0000EE2B File Offset: 0x0000D02B
		public override void CreateControl()
		{
			this.button.Create("Button", base.transform, 1000);
		}

		// Token: 0x060003FC RID: 1020 RVA: 0x0000EE48 File Offset: 0x0000D048
		public override void DestroyControl()
		{
			this.button.Delete();
			if (this.currentTouch != null)
			{
				this.TouchEnded(this.currentTouch);
				this.currentTouch = null;
			}
		}

		// Token: 0x060003FD RID: 1021 RVA: 0x0000EE70 File Offset: 0x0000D070
		public override void ConfigureControl()
		{
			base.transform.position = base.OffsetToWorldPosition(this.anchor, this.offset, this.offsetUnitType, this.lockAspectRatio);
			this.button.Update(true);
		}

		// Token: 0x060003FE RID: 1022 RVA: 0x0000EEA7 File Offset: 0x0000D0A7
		public override void DrawGizmos()
		{
			this.button.DrawGizmos(this.ButtonPosition, Color.yellow);
		}

		// Token: 0x060003FF RID: 1023 RVA: 0x0000EEBF File Offset: 0x0000D0BF
		private void Update()
		{
			if (this.dirty)
			{
				this.ConfigureControl();
				this.dirty = false;
				return;
			}
			this.button.Update();
		}

		// Token: 0x06000400 RID: 1024 RVA: 0x0000EEE4 File Offset: 0x0000D0E4
		public override void SubmitControlState(ulong updateTick, float deltaTime)
		{
			if (this.pressureSensitive)
			{
				float num = 0f;
				if (this.currentTouch == null)
				{
					if (this.allowSlideToggle)
					{
						int touchCount = TouchManager.TouchCount;
						for (int i = 0; i < touchCount; i++)
						{
							Touch touch = TouchManager.GetTouch(i);
							if (this.button.Contains(touch))
							{
								num = Utility.Max(num, touch.NormalizedPressure);
							}
						}
					}
				}
				else
				{
					num = this.currentTouch.NormalizedPressure;
				}
				this.ButtonState = num > 0f;
				base.SubmitButtonValue(this.target, num, updateTick, deltaTime);
				return;
			}
			if (this.currentTouch == null && this.allowSlideToggle)
			{
				this.ButtonState = false;
				int touchCount2 = TouchManager.TouchCount;
				for (int j = 0; j < touchCount2; j++)
				{
					this.ButtonState = this.ButtonState || this.button.Contains(TouchManager.GetTouch(j));
				}
			}
			base.SubmitButtonState(this.target, this.ButtonState, updateTick, deltaTime);
		}

		// Token: 0x06000401 RID: 1025 RVA: 0x0000EFD5 File Offset: 0x0000D1D5
		public override void CommitControlState(ulong updateTick, float deltaTime)
		{
			base.CommitButton(this.target);
		}

		// Token: 0x06000402 RID: 1026 RVA: 0x0000EFE3 File Offset: 0x0000D1E3
		public override void TouchBegan(Touch touch)
		{
			if (this.currentTouch != null)
			{
				return;
			}
			if (this.button.Contains(touch))
			{
				this.ButtonState = true;
				this.currentTouch = touch;
			}
		}

		// Token: 0x06000403 RID: 1027 RVA: 0x0000F00A File Offset: 0x0000D20A
		public override void TouchMoved(Touch touch)
		{
			if (this.currentTouch != touch)
			{
				return;
			}
			if (this.toggleOnLeave && !this.button.Contains(touch))
			{
				this.ButtonState = false;
				this.currentTouch = null;
			}
		}

		// Token: 0x06000404 RID: 1028 RVA: 0x0000F03A File Offset: 0x0000D23A
		public override void TouchEnded(Touch touch)
		{
			if (this.currentTouch != touch)
			{
				return;
			}
			this.ButtonState = false;
			this.currentTouch = null;
		}

		// Token: 0x17000132 RID: 306
		// (get) Token: 0x06000405 RID: 1029 RVA: 0x0000F054 File Offset: 0x0000D254
		// (set) Token: 0x06000406 RID: 1030 RVA: 0x0000F05C File Offset: 0x0000D25C
		private bool ButtonState
		{
			get
			{
				return this.buttonState;
			}
			set
			{
				if (this.buttonState != value)
				{
					this.buttonState = value;
					this.button.State = value;
				}
			}
		}

		// Token: 0x17000133 RID: 307
		// (get) Token: 0x06000407 RID: 1031 RVA: 0x0000F07A File Offset: 0x0000D27A
		// (set) Token: 0x06000408 RID: 1032 RVA: 0x0000F0A0 File Offset: 0x0000D2A0
		public Vector3 ButtonPosition
		{
			get
			{
				if (!this.button.Ready)
				{
					return base.transform.position;
				}
				return this.button.Position;
			}
			set
			{
				if (this.button.Ready)
				{
					this.button.Position = value;
				}
			}
		}

		// Token: 0x17000134 RID: 308
		// (get) Token: 0x06000409 RID: 1033 RVA: 0x0000F0BB File Offset: 0x0000D2BB
		// (set) Token: 0x0600040A RID: 1034 RVA: 0x0000F0C3 File Offset: 0x0000D2C3
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

		// Token: 0x17000135 RID: 309
		// (get) Token: 0x0600040B RID: 1035 RVA: 0x0000F0DC File Offset: 0x0000D2DC
		// (set) Token: 0x0600040C RID: 1036 RVA: 0x0000F0E4 File Offset: 0x0000D2E4
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

		// Token: 0x17000136 RID: 310
		// (get) Token: 0x0600040D RID: 1037 RVA: 0x0000F102 File Offset: 0x0000D302
		// (set) Token: 0x0600040E RID: 1038 RVA: 0x0000F10A File Offset: 0x0000D30A
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

		// Token: 0x040003B4 RID: 948
		[Header("Position")]
		[SerializeField]
		private TouchControlAnchor anchor = TouchControlAnchor.BottomRight;

		// Token: 0x040003B5 RID: 949
		[SerializeField]
		private TouchUnitType offsetUnitType;

		// Token: 0x040003B6 RID: 950
		[SerializeField]
		private Vector2 offset = new Vector2(-10f, 10f);

		// Token: 0x040003B7 RID: 951
		[SerializeField]
		private bool lockAspectRatio = true;

		// Token: 0x040003B8 RID: 952
		[Header("Options")]
		public TouchControl.ButtonTarget target = TouchControl.ButtonTarget.Action1;

		// Token: 0x040003B9 RID: 953
		public bool allowSlideToggle = true;

		// Token: 0x040003BA RID: 954
		public bool toggleOnLeave;

		// Token: 0x040003BB RID: 955
		public bool pressureSensitive;

		// Token: 0x040003BC RID: 956
		[Header("Sprites")]
		public TouchSprite button = new TouchSprite(15f);

		// Token: 0x040003BD RID: 957
		private bool buttonState;

		// Token: 0x040003BE RID: 958
		private Touch currentTouch;

		// Token: 0x040003BF RID: 959
		private bool dirty;
	}
}
