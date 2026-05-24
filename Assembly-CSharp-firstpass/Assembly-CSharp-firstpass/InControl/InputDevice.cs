using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace InControl
{
	// Token: 0x0200003F RID: 63
	public class InputDevice
	{
		// Token: 0x170000A5 RID: 165
		// (get) Token: 0x0600023B RID: 571 RVA: 0x00008641 File Offset: 0x00006841
		// (set) Token: 0x0600023C RID: 572 RVA: 0x00008649 File Offset: 0x00006849
		public string Name { get; protected set; }

		// Token: 0x170000A6 RID: 166
		// (get) Token: 0x0600023D RID: 573 RVA: 0x00008652 File Offset: 0x00006852
		// (set) Token: 0x0600023E RID: 574 RVA: 0x0000865A File Offset: 0x0000685A
		public string Meta { get; protected set; }

		// Token: 0x170000A7 RID: 167
		// (get) Token: 0x0600023F RID: 575 RVA: 0x00008663 File Offset: 0x00006863
		// (set) Token: 0x06000240 RID: 576 RVA: 0x0000866B File Offset: 0x0000686B
		public int SortOrder { get; protected set; }

		// Token: 0x170000A8 RID: 168
		// (get) Token: 0x06000241 RID: 577 RVA: 0x00008674 File Offset: 0x00006874
		// (set) Token: 0x06000242 RID: 578 RVA: 0x0000867C File Offset: 0x0000687C
		public InputDeviceClass DeviceClass { get; protected set; }

		// Token: 0x170000A9 RID: 169
		// (get) Token: 0x06000243 RID: 579 RVA: 0x00008685 File Offset: 0x00006885
		// (set) Token: 0x06000244 RID: 580 RVA: 0x0000868D File Offset: 0x0000688D
		public InputDeviceStyle DeviceStyle { get; protected set; }

		// Token: 0x170000AA RID: 170
		// (get) Token: 0x06000245 RID: 581 RVA: 0x00008696 File Offset: 0x00006896
		// (set) Token: 0x06000246 RID: 582 RVA: 0x0000869E File Offset: 0x0000689E
		public Guid GUID { get; private set; }

		// Token: 0x170000AB RID: 171
		// (get) Token: 0x06000247 RID: 583 RVA: 0x000086A7 File Offset: 0x000068A7
		// (set) Token: 0x06000248 RID: 584 RVA: 0x000086AF File Offset: 0x000068AF
		public ulong LastInputTick { get; private set; }

		// Token: 0x170000AC RID: 172
		// (get) Token: 0x06000249 RID: 585 RVA: 0x000086B8 File Offset: 0x000068B8
		// (set) Token: 0x0600024A RID: 586 RVA: 0x000086C0 File Offset: 0x000068C0
		public bool IsActive { get; private set; }

		// Token: 0x170000AD RID: 173
		// (get) Token: 0x0600024B RID: 587 RVA: 0x000086C9 File Offset: 0x000068C9
		// (set) Token: 0x0600024C RID: 588 RVA: 0x000086D1 File Offset: 0x000068D1
		public bool IsAttached { get; private set; }

		// Token: 0x170000AE RID: 174
		// (get) Token: 0x0600024D RID: 589 RVA: 0x000086DA File Offset: 0x000068DA
		// (set) Token: 0x0600024E RID: 590 RVA: 0x000086E2 File Offset: 0x000068E2
		private protected bool RawSticks { protected get; private set; }

		// Token: 0x170000AF RID: 175
		// (get) Token: 0x0600024F RID: 591 RVA: 0x000086EB File Offset: 0x000068EB
		// (set) Token: 0x06000250 RID: 592 RVA: 0x000086F3 File Offset: 0x000068F3
		public ReadOnlyCollection<InputControl> Controls { get; protected set; }

		// Token: 0x170000B0 RID: 176
		// (get) Token: 0x06000251 RID: 593 RVA: 0x000086FC File Offset: 0x000068FC
		// (set) Token: 0x06000252 RID: 594 RVA: 0x00008704 File Offset: 0x00006904
		private protected InputControl[] ControlsByTarget { protected get; private set; }

		// Token: 0x170000B1 RID: 177
		// (get) Token: 0x06000253 RID: 595 RVA: 0x0000870D File Offset: 0x0000690D
		// (set) Token: 0x06000254 RID: 596 RVA: 0x00008715 File Offset: 0x00006915
		public TwoAxisInputControl LeftStick { get; private set; }

		// Token: 0x170000B2 RID: 178
		// (get) Token: 0x06000255 RID: 597 RVA: 0x0000871E File Offset: 0x0000691E
		// (set) Token: 0x06000256 RID: 598 RVA: 0x00008726 File Offset: 0x00006926
		public TwoAxisInputControl RightStick { get; private set; }

		// Token: 0x170000B3 RID: 179
		// (get) Token: 0x06000257 RID: 599 RVA: 0x0000872F File Offset: 0x0000692F
		// (set) Token: 0x06000258 RID: 600 RVA: 0x00008737 File Offset: 0x00006937
		public TwoAxisInputControl DPad { get; private set; }

		// Token: 0x170000B4 RID: 180
		// (get) Token: 0x06000259 RID: 601 RVA: 0x00008740 File Offset: 0x00006940
		// (set) Token: 0x0600025A RID: 602 RVA: 0x00008748 File Offset: 0x00006948
		public InputControlType LeftCommandControl { get; private set; }

		// Token: 0x170000B5 RID: 181
		// (get) Token: 0x0600025B RID: 603 RVA: 0x00008751 File Offset: 0x00006951
		// (set) Token: 0x0600025C RID: 604 RVA: 0x00008759 File Offset: 0x00006959
		public InputControlType RightCommandControl { get; private set; }

		// Token: 0x170000B6 RID: 182
		// (get) Token: 0x0600025D RID: 605 RVA: 0x00008762 File Offset: 0x00006962
		// (set) Token: 0x0600025E RID: 606 RVA: 0x0000876A File Offset: 0x0000696A
		protected InputDevice.AnalogSnapshotEntry[] AnalogSnapshot { get; set; }

		// Token: 0x0600025F RID: 607 RVA: 0x00008773 File Offset: 0x00006973
		public InputDevice()
			: this("")
		{
		}

		// Token: 0x06000260 RID: 608 RVA: 0x00008780 File Offset: 0x00006980
		public InputDevice(string name)
			: this(name, false)
		{
		}

		// Token: 0x06000261 RID: 609 RVA: 0x0000878C File Offset: 0x0000698C
		public InputDevice(string name, bool rawSticks)
		{
			this.Name = name;
			this.RawSticks = rawSticks;
			this.Meta = "";
			this.GUID = Guid.NewGuid();
			this.LastInputTick = 0UL;
			this.SortOrder = int.MaxValue;
			this.DeviceClass = InputDeviceClass.Unknown;
			this.DeviceStyle = InputDeviceStyle.Unknown;
			this.Passive = false;
			this.ControlsByTarget = new InputControl[531];
			this.controls = new List<InputControl>(32);
			this.Controls = new ReadOnlyCollection<InputControl>(this.controls);
			this.RemoveAliasControls();
		}

		// Token: 0x06000262 RID: 610 RVA: 0x0000881F File Offset: 0x00006A1F
		internal void OnAttached()
		{
			this.IsAttached = true;
			this.AddAliasControls();
		}

		// Token: 0x06000263 RID: 611 RVA: 0x0000882E File Offset: 0x00006A2E
		internal void OnDetached()
		{
			this.IsAttached = false;
			this.StopVibration();
			this.RemoveAliasControls();
		}

		// Token: 0x06000264 RID: 612 RVA: 0x00008844 File Offset: 0x00006A44
		private void AddAliasControls()
		{
			this.RemoveAliasControls();
			if (this.IsKnown)
			{
				this.LeftStick = new TwoAxisInputControl();
				this.RightStick = new TwoAxisInputControl();
				this.DPad = new TwoAxisInputControl();
				this.DPad.DeadZoneFunc = new DeadZoneFunc(DeadZone.Separate);
				this.AddControl(InputControlType.LeftStickX, "Left Stick X");
				this.AddControl(InputControlType.LeftStickY, "Left Stick Y");
				this.AddControl(InputControlType.RightStickX, "Right Stick X");
				this.AddControl(InputControlType.RightStickY, "Right Stick Y");
				this.AddControl(InputControlType.DPadX, "DPad X");
				this.AddControl(InputControlType.DPadY, "DPad Y");
				this.AddControl(InputControlType.Command, "Command");
				this.LeftCommandControl = this.DeviceStyle.LeftCommandControl();
				this.leftCommandSource = this.GetControl(this.LeftCommandControl);
				this.hasLeftCommandControl = !this.leftCommandSource.IsNullControl;
				if (this.hasLeftCommandControl)
				{
					this.AddControl(InputControlType.LeftCommand, this.leftCommandSource.Handle);
				}
				this.RightCommandControl = this.DeviceStyle.RightCommandControl();
				this.rightCommandSource = this.GetControl(this.RightCommandControl);
				this.hasRightCommandControl = !this.rightCommandSource.IsNullControl;
				if (this.hasRightCommandControl)
				{
					this.AddControl(InputControlType.RightCommand, this.rightCommandSource.Handle);
				}
				this.ExpireControlCache();
			}
		}

		// Token: 0x06000265 RID: 613 RVA: 0x000089C4 File Offset: 0x00006BC4
		private void RemoveAliasControls()
		{
			this.LeftStick = TwoAxisInputControl.Null;
			this.RightStick = TwoAxisInputControl.Null;
			this.DPad = TwoAxisInputControl.Null;
			this.RemoveControl(InputControlType.LeftStickX);
			this.RemoveControl(InputControlType.LeftStickY);
			this.RemoveControl(InputControlType.RightStickX);
			this.RemoveControl(InputControlType.RightStickY);
			this.RemoveControl(InputControlType.DPadX);
			this.RemoveControl(InputControlType.DPadY);
			this.RemoveControl(InputControlType.Command);
			this.RemoveControl(InputControlType.LeftCommand);
			this.RemoveControl(InputControlType.RightCommand);
			this.leftCommandSource = null;
			this.hasLeftCommandControl = false;
			this.rightCommandSource = null;
			this.hasRightCommandControl = false;
			this.ExpireControlCache();
		}

		// Token: 0x06000266 RID: 614 RVA: 0x00008A77 File Offset: 0x00006C77
		protected void ClearControls()
		{
			Array.Clear(this.ControlsByTarget, 0, this.ControlsByTarget.Length);
			this.controls.Clear();
			this.ExpireControlCache();
		}

		// Token: 0x06000267 RID: 615 RVA: 0x00008A9E File Offset: 0x00006C9E
		public bool HasControl(InputControlType controlType)
		{
			return this.ControlsByTarget[(int)controlType] != null;
		}

		// Token: 0x06000268 RID: 616 RVA: 0x00008AAB File Offset: 0x00006CAB
		public InputControl GetControl(InputControlType controlType)
		{
			return this.ControlsByTarget[(int)controlType] ?? InputControl.Null;
		}

		// Token: 0x170000B7 RID: 183
		public InputControl this[InputControlType controlType]
		{
			get
			{
				return this.GetControl(controlType);
			}
		}

		// Token: 0x0600026A RID: 618 RVA: 0x00008AC7 File Offset: 0x00006CC7
		public static InputControlType GetInputControlTypeByName(string inputControlName)
		{
			return (InputControlType)Enum.Parse(typeof(InputControlType), inputControlName);
		}

		// Token: 0x0600026B RID: 619 RVA: 0x00008AE0 File Offset: 0x00006CE0
		public InputControl GetControlByName(string controlName)
		{
			InputControlType inputControlTypeByName = InputDevice.GetInputControlTypeByName(controlName);
			return this.GetControl(inputControlTypeByName);
		}

		// Token: 0x0600026C RID: 620 RVA: 0x00008AFC File Offset: 0x00006CFC
		public InputControl AddControl(InputControlType controlType, string handle)
		{
			InputControl inputControl = this.ControlsByTarget[(int)controlType];
			if (inputControl == null)
			{
				inputControl = new InputControl(handle, controlType);
				this.ControlsByTarget[(int)controlType] = inputControl;
				this.controls.Add(inputControl);
				this.ExpireControlCache();
			}
			return inputControl;
		}

		// Token: 0x0600026D RID: 621 RVA: 0x00008B39 File Offset: 0x00006D39
		public InputControl AddControl(InputControlType controlType, string handle, float lowerDeadZone, float upperDeadZone)
		{
			InputControl inputControl = this.AddControl(controlType, handle);
			inputControl.LowerDeadZone = lowerDeadZone;
			inputControl.UpperDeadZone = upperDeadZone;
			return inputControl;
		}

		// Token: 0x0600026E RID: 622 RVA: 0x00008B54 File Offset: 0x00006D54
		private void RemoveControl(InputControlType controlType)
		{
			InputControl inputControl = this.ControlsByTarget[(int)controlType];
			if (inputControl != null)
			{
				this.ControlsByTarget[(int)controlType] = null;
				this.controls.Remove(inputControl);
				this.ExpireControlCache();
			}
		}

		// Token: 0x0600026F RID: 623 RVA: 0x00008B8C File Offset: 0x00006D8C
		public void ClearInputState()
		{
			this.LeftStick.ClearInputState();
			this.RightStick.ClearInputState();
			this.DPad.ClearInputState();
			int count = this.Controls.Count;
			for (int i = 0; i < count; i++)
			{
				InputControl inputControl = this.Controls[i];
				if (inputControl != null)
				{
					inputControl.ClearInputState();
				}
			}
		}

		// Token: 0x06000270 RID: 624 RVA: 0x00008BE8 File Offset: 0x00006DE8
		protected void UpdateWithState(InputControlType controlType, bool state, ulong updateTick, float deltaTime)
		{
			this.GetControl(controlType).UpdateWithState(state, updateTick, deltaTime);
		}

		// Token: 0x06000271 RID: 625 RVA: 0x00008BFB File Offset: 0x00006DFB
		protected void UpdateWithValue(InputControlType controlType, float value, ulong updateTick, float deltaTime)
		{
			this.GetControl(controlType).UpdateWithValue(value, updateTick, deltaTime);
		}

		// Token: 0x06000272 RID: 626 RVA: 0x00008C0E File Offset: 0x00006E0E
		protected void UpdateWithRawValue(InputControlType controlType, float value, ulong updateTick, float deltaTime)
		{
			this.GetControl(controlType).UpdateWithRawValue(value, updateTick, deltaTime);
		}

		// Token: 0x06000273 RID: 627 RVA: 0x00008C24 File Offset: 0x00006E24
		public void UpdateLeftStickWithValue(Vector2 value, ulong updateTick, float deltaTime)
		{
			this.LeftStickLeft.UpdateWithValue(Mathf.Max(0f, -value.x), updateTick, deltaTime);
			this.LeftStickRight.UpdateWithValue(Mathf.Max(0f, value.x), updateTick, deltaTime);
			if (InputManager.InvertYAxis)
			{
				this.LeftStickUp.UpdateWithValue(Mathf.Max(0f, -value.y), updateTick, deltaTime);
				this.LeftStickDown.UpdateWithValue(Mathf.Max(0f, value.y), updateTick, deltaTime);
				return;
			}
			this.LeftStickUp.UpdateWithValue(Mathf.Max(0f, value.y), updateTick, deltaTime);
			this.LeftStickDown.UpdateWithValue(Mathf.Max(0f, -value.y), updateTick, deltaTime);
		}

		// Token: 0x06000274 RID: 628 RVA: 0x00008CF0 File Offset: 0x00006EF0
		public void UpdateLeftStickWithRawValue(Vector2 value, ulong updateTick, float deltaTime)
		{
			this.LeftStickLeft.UpdateWithRawValue(Mathf.Max(0f, -value.x), updateTick, deltaTime);
			this.LeftStickRight.UpdateWithRawValue(Mathf.Max(0f, value.x), updateTick, deltaTime);
			if (InputManager.InvertYAxis)
			{
				this.LeftStickUp.UpdateWithRawValue(Mathf.Max(0f, -value.y), updateTick, deltaTime);
				this.LeftStickDown.UpdateWithRawValue(Mathf.Max(0f, value.y), updateTick, deltaTime);
				return;
			}
			this.LeftStickUp.UpdateWithRawValue(Mathf.Max(0f, value.y), updateTick, deltaTime);
			this.LeftStickDown.UpdateWithRawValue(Mathf.Max(0f, -value.y), updateTick, deltaTime);
		}

		// Token: 0x06000275 RID: 629 RVA: 0x00008DBC File Offset: 0x00006FBC
		public void CommitLeftStick()
		{
			this.LeftStickUp.Commit();
			this.LeftStickDown.Commit();
			this.LeftStickLeft.Commit();
			this.LeftStickRight.Commit();
		}

		// Token: 0x06000276 RID: 630 RVA: 0x00008DEC File Offset: 0x00006FEC
		public void UpdateRightStickWithValue(Vector2 value, ulong updateTick, float deltaTime)
		{
			this.RightStickLeft.UpdateWithValue(Mathf.Max(0f, -value.x), updateTick, deltaTime);
			this.RightStickRight.UpdateWithValue(Mathf.Max(0f, value.x), updateTick, deltaTime);
			if (InputManager.InvertYAxis)
			{
				this.RightStickUp.UpdateWithValue(Mathf.Max(0f, -value.y), updateTick, deltaTime);
				this.RightStickDown.UpdateWithValue(Mathf.Max(0f, value.y), updateTick, deltaTime);
				return;
			}
			this.RightStickUp.UpdateWithValue(Mathf.Max(0f, value.y), updateTick, deltaTime);
			this.RightStickDown.UpdateWithValue(Mathf.Max(0f, -value.y), updateTick, deltaTime);
		}

		// Token: 0x06000277 RID: 631 RVA: 0x00008EB8 File Offset: 0x000070B8
		public void UpdateRightStickWithRawValue(Vector2 value, ulong updateTick, float deltaTime)
		{
			this.RightStickLeft.UpdateWithRawValue(Mathf.Max(0f, -value.x), updateTick, deltaTime);
			this.RightStickRight.UpdateWithRawValue(Mathf.Max(0f, value.x), updateTick, deltaTime);
			if (InputManager.InvertYAxis)
			{
				this.RightStickUp.UpdateWithRawValue(Mathf.Max(0f, -value.y), updateTick, deltaTime);
				this.RightStickDown.UpdateWithRawValue(Mathf.Max(0f, value.y), updateTick, deltaTime);
				return;
			}
			this.RightStickUp.UpdateWithRawValue(Mathf.Max(0f, value.y), updateTick, deltaTime);
			this.RightStickDown.UpdateWithRawValue(Mathf.Max(0f, -value.y), updateTick, deltaTime);
		}

		// Token: 0x06000278 RID: 632 RVA: 0x00008F84 File Offset: 0x00007184
		public void CommitRightStick()
		{
			this.RightStickUp.Commit();
			this.RightStickDown.Commit();
			this.RightStickLeft.Commit();
			this.RightStickRight.Commit();
		}

		// Token: 0x06000279 RID: 633 RVA: 0x00008FB2 File Offset: 0x000071B2
		public virtual void Update(ulong updateTick, float deltaTime)
		{
		}

		// Token: 0x0600027A RID: 634 RVA: 0x00008FB4 File Offset: 0x000071B4
		private void ProcessLeftStick(ulong updateTick, float deltaTime)
		{
			float num = Utility.ValueFromSides(this.LeftStickLeft.NextRawValue, this.LeftStickRight.NextRawValue);
			float num2 = Utility.ValueFromSides(this.LeftStickDown.NextRawValue, this.LeftStickUp.NextRawValue, InputManager.InvertYAxis);
			Vector2 vector;
			if (this.RawSticks || this.LeftStickLeft.Raw || this.LeftStickRight.Raw || this.LeftStickUp.Raw || this.LeftStickDown.Raw)
			{
				vector = new Vector2(num, num2);
			}
			else
			{
				float num3 = Utility.Max(this.LeftStickLeft.LowerDeadZone, this.LeftStickRight.LowerDeadZone, this.LeftStickUp.LowerDeadZone, this.LeftStickDown.LowerDeadZone);
				float num4 = Utility.Min(this.LeftStickLeft.UpperDeadZone, this.LeftStickRight.UpperDeadZone, this.LeftStickUp.UpperDeadZone, this.LeftStickDown.UpperDeadZone);
				vector = this.LeftStick.DeadZoneFunc(num, num2, num3, num4);
			}
			this.LeftStick.Raw = true;
			this.LeftStick.UpdateWithAxes(vector.x, vector.y, updateTick, deltaTime);
			this.LeftStickX.Raw = true;
			this.LeftStickX.CommitWithValue(vector.x, updateTick, deltaTime);
			this.LeftStickY.Raw = true;
			this.LeftStickY.CommitWithValue(vector.y, updateTick, deltaTime);
			this.LeftStickLeft.SetValue(this.LeftStick.Left.Value, updateTick);
			this.LeftStickRight.SetValue(this.LeftStick.Right.Value, updateTick);
			this.LeftStickUp.SetValue(this.LeftStick.Up.Value, updateTick);
			this.LeftStickDown.SetValue(this.LeftStick.Down.Value, updateTick);
		}

		// Token: 0x0600027B RID: 635 RVA: 0x00009194 File Offset: 0x00007394
		private void ProcessRightStick(ulong updateTick, float deltaTime)
		{
			float num = Utility.ValueFromSides(this.RightStickLeft.NextRawValue, this.RightStickRight.NextRawValue);
			float num2 = Utility.ValueFromSides(this.RightStickDown.NextRawValue, this.RightStickUp.NextRawValue, InputManager.InvertYAxis);
			Vector2 vector;
			if (this.RawSticks || this.RightStickLeft.Raw || this.RightStickRight.Raw || this.RightStickUp.Raw || this.RightStickDown.Raw)
			{
				vector = new Vector2(num, num2);
			}
			else
			{
				float num3 = Utility.Max(this.RightStickLeft.LowerDeadZone, this.RightStickRight.LowerDeadZone, this.RightStickUp.LowerDeadZone, this.RightStickDown.LowerDeadZone);
				float num4 = Utility.Min(this.RightStickLeft.UpperDeadZone, this.RightStickRight.UpperDeadZone, this.RightStickUp.UpperDeadZone, this.RightStickDown.UpperDeadZone);
				vector = this.RightStick.DeadZoneFunc(num, num2, num3, num4);
			}
			this.RightStick.Raw = true;
			this.RightStick.UpdateWithAxes(vector.x, vector.y, updateTick, deltaTime);
			this.RightStickX.Raw = true;
			this.RightStickX.CommitWithValue(vector.x, updateTick, deltaTime);
			this.RightStickY.Raw = true;
			this.RightStickY.CommitWithValue(vector.y, updateTick, deltaTime);
			this.RightStickLeft.SetValue(this.RightStick.Left.Value, updateTick);
			this.RightStickRight.SetValue(this.RightStick.Right.Value, updateTick);
			this.RightStickUp.SetValue(this.RightStick.Up.Value, updateTick);
			this.RightStickDown.SetValue(this.RightStick.Down.Value, updateTick);
		}

		// Token: 0x0600027C RID: 636 RVA: 0x00009374 File Offset: 0x00007574
		private void ProcessDPad(ulong updateTick, float deltaTime)
		{
			float num = Utility.ValueFromSides(this.DPadLeft.NextRawValue, this.DPadRight.NextRawValue);
			float num2 = Utility.ValueFromSides(this.DPadDown.NextRawValue, this.DPadUp.NextRawValue, InputManager.InvertYAxis);
			Vector2 vector;
			if (this.RawSticks || this.DPadLeft.Raw || this.DPadRight.Raw || this.DPadUp.Raw || this.DPadDown.Raw)
			{
				vector = new Vector2(num, num2);
			}
			else
			{
				float num3 = Utility.Max(this.DPadLeft.LowerDeadZone, this.DPadRight.LowerDeadZone, this.DPadUp.LowerDeadZone, this.DPadDown.LowerDeadZone);
				float num4 = Utility.Min(this.DPadLeft.UpperDeadZone, this.DPadRight.UpperDeadZone, this.DPadUp.UpperDeadZone, this.DPadDown.UpperDeadZone);
				vector = this.DPad.DeadZoneFunc(num, num2, num3, num4);
			}
			this.DPad.Raw = true;
			this.DPad.UpdateWithAxes(vector.x, vector.y, updateTick, deltaTime);
			this.DPadX.Raw = true;
			this.DPadX.CommitWithValue(vector.x, updateTick, deltaTime);
			this.DPadY.Raw = true;
			this.DPadY.CommitWithValue(vector.y, updateTick, deltaTime);
			this.DPadLeft.SetValue(this.DPad.Left.Value, updateTick);
			this.DPadRight.SetValue(this.DPad.Right.Value, updateTick);
			this.DPadUp.SetValue(this.DPad.Up.Value, updateTick);
			this.DPadDown.SetValue(this.DPad.Down.Value, updateTick);
		}

		// Token: 0x0600027D RID: 637 RVA: 0x00009554 File Offset: 0x00007754
		public void Commit(ulong updateTick, float deltaTime)
		{
			if (this.IsKnown)
			{
				this.ProcessLeftStick(updateTick, deltaTime);
				this.ProcessRightStick(updateTick, deltaTime);
				this.ProcessDPad(updateTick, deltaTime);
			}
			int count = this.Controls.Count;
			for (int i = 0; i < count; i++)
			{
				InputControl inputControl = this.Controls[i];
				if (inputControl != null)
				{
					inputControl.Commit();
				}
			}
			if (this.IsKnown)
			{
				bool flag = true;
				bool flag2 = false;
				for (int j = 100; j <= 116; j++)
				{
					InputControl inputControl2 = this.ControlsByTarget[j];
					if (inputControl2 != null && inputControl2.IsPressed)
					{
						flag2 = true;
						if (!inputControl2.Passive)
						{
							flag = false;
						}
					}
				}
				this.Command.Passive = flag;
				this.Command.CommitWithState(flag2, updateTick, deltaTime);
				if (this.hasLeftCommandControl)
				{
					this.LeftCommand.Passive = this.leftCommandSource.Passive;
					this.LeftCommand.CommitWithState(this.leftCommandSource.IsPressed, updateTick, deltaTime);
				}
				if (this.hasRightCommandControl)
				{
					this.RightCommand.Passive = this.rightCommandSource.Passive;
					this.RightCommand.CommitWithState(this.rightCommandSource.IsPressed, updateTick, deltaTime);
				}
			}
			this.IsActive = false;
			for (int k = 0; k < count; k++)
			{
				InputControl inputControl3 = this.Controls[k];
				if (inputControl3 != null && inputControl3.HasInput && !inputControl3.Passive)
				{
					this.LastInputTick = updateTick;
					this.IsActive = true;
				}
			}
		}

		// Token: 0x0600027E RID: 638 RVA: 0x000096C8 File Offset: 0x000078C8
		public bool LastInputAfter(InputDevice device)
		{
			return device == null || this.LastInputTick > device.LastInputTick;
		}

		// Token: 0x0600027F RID: 639 RVA: 0x000096DD File Offset: 0x000078DD
		public void RequestActivation()
		{
			this.LastInputTick = InputManager.CurrentTick;
			this.IsActive = true;
		}

		// Token: 0x06000280 RID: 640 RVA: 0x000096F1 File Offset: 0x000078F1
		public virtual void Vibrate(float leftSpeed, float rightSpeed)
		{
		}

		// Token: 0x06000281 RID: 641 RVA: 0x000096F3 File Offset: 0x000078F3
		public void Vibrate(float intensity)
		{
			this.Vibrate(intensity, intensity);
		}

		// Token: 0x06000282 RID: 642 RVA: 0x000096FD File Offset: 0x000078FD
		public virtual void VibrateTriggers(float leftTriggerSpeed, float rightTriggerSpeed)
		{
		}

		// Token: 0x06000283 RID: 643 RVA: 0x000096FF File Offset: 0x000078FF
		public void StopVibration()
		{
			this.Vibrate(0f);
		}

		// Token: 0x06000284 RID: 644 RVA: 0x0000970C File Offset: 0x0000790C
		public virtual void SetLightColor(float red, float green, float blue)
		{
		}

		// Token: 0x06000285 RID: 645 RVA: 0x0000970E File Offset: 0x0000790E
		public void SetLightColor(Color color)
		{
			this.SetLightColor(color.r * color.a, color.g * color.a, color.b * color.a);
		}

		// Token: 0x06000286 RID: 646 RVA: 0x0000973D File Offset: 0x0000793D
		public virtual void SetLightFlash(float flashOnDuration, float flashOffDuration)
		{
		}

		// Token: 0x06000287 RID: 647 RVA: 0x0000973F File Offset: 0x0000793F
		public void StopLightFlash()
		{
			this.SetLightFlash(1f, 0f);
		}

		// Token: 0x170000B8 RID: 184
		// (get) Token: 0x06000288 RID: 648 RVA: 0x00009751 File Offset: 0x00007951
		public virtual bool IsSupportedOnThisPlatform
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170000B9 RID: 185
		// (get) Token: 0x06000289 RID: 649 RVA: 0x00009754 File Offset: 0x00007954
		public virtual bool IsKnown
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170000BA RID: 186
		// (get) Token: 0x0600028A RID: 650 RVA: 0x00009757 File Offset: 0x00007957
		public bool IsUnknown
		{
			get
			{
				return !this.IsKnown;
			}
		}

		// Token: 0x170000BB RID: 187
		// (get) Token: 0x0600028B RID: 651 RVA: 0x00009762 File Offset: 0x00007962
		[Obsolete("Use InputDevice.CommandIsPressed instead.", false)]
		public bool MenuIsPressed
		{
			get
			{
				return this.IsKnown && this.Command.IsPressed;
			}
		}

		// Token: 0x170000BC RID: 188
		// (get) Token: 0x0600028C RID: 652 RVA: 0x00009779 File Offset: 0x00007979
		[Obsolete("Use InputDevice.CommandWasPressed instead.", false)]
		public bool MenuWasPressed
		{
			get
			{
				return this.IsKnown && this.Command.WasPressed;
			}
		}

		// Token: 0x170000BD RID: 189
		// (get) Token: 0x0600028D RID: 653 RVA: 0x00009790 File Offset: 0x00007990
		[Obsolete("Use InputDevice.CommandWasReleased instead.", false)]
		public bool MenuWasReleased
		{
			get
			{
				return this.IsKnown && this.Command.WasReleased;
			}
		}

		// Token: 0x170000BE RID: 190
		// (get) Token: 0x0600028E RID: 654 RVA: 0x000097A7 File Offset: 0x000079A7
		public bool CommandIsPressed
		{
			get
			{
				return this.IsKnown && this.Command.IsPressed;
			}
		}

		// Token: 0x170000BF RID: 191
		// (get) Token: 0x0600028F RID: 655 RVA: 0x000097BE File Offset: 0x000079BE
		public bool CommandWasPressed
		{
			get
			{
				return this.IsKnown && this.Command.WasPressed;
			}
		}

		// Token: 0x170000C0 RID: 192
		// (get) Token: 0x06000290 RID: 656 RVA: 0x000097D5 File Offset: 0x000079D5
		public bool CommandWasReleased
		{
			get
			{
				return this.IsKnown && this.Command.WasReleased;
			}
		}

		// Token: 0x170000C1 RID: 193
		// (get) Token: 0x06000291 RID: 657 RVA: 0x000097EC File Offset: 0x000079EC
		public InputControl AnyButton
		{
			get
			{
				int count = this.Controls.Count;
				for (int i = 0; i < count; i++)
				{
					InputControl inputControl = this.Controls[i];
					if (inputControl != null && inputControl.IsButton && inputControl.IsPressed)
					{
						return inputControl;
					}
				}
				return InputControl.Null;
			}
		}

		// Token: 0x170000C2 RID: 194
		// (get) Token: 0x06000292 RID: 658 RVA: 0x00009838 File Offset: 0x00007A38
		public bool AnyButtonIsPressed
		{
			get
			{
				int count = this.Controls.Count;
				for (int i = 0; i < count; i++)
				{
					InputControl inputControl = this.Controls[i];
					if (inputControl != null && inputControl.IsButton && inputControl.IsPressed)
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x170000C3 RID: 195
		// (get) Token: 0x06000293 RID: 659 RVA: 0x00009880 File Offset: 0x00007A80
		public bool AnyButtonWasPressed
		{
			get
			{
				int count = this.Controls.Count;
				for (int i = 0; i < count; i++)
				{
					InputControl inputControl = this.Controls[i];
					if (inputControl != null && inputControl.IsButton && inputControl.WasPressed)
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x170000C4 RID: 196
		// (get) Token: 0x06000294 RID: 660 RVA: 0x000098C8 File Offset: 0x00007AC8
		public bool AnyButtonWasReleased
		{
			get
			{
				int count = this.Controls.Count;
				for (int i = 0; i < count; i++)
				{
					InputControl inputControl = this.Controls[i];
					if (inputControl != null && inputControl.IsButton && inputControl.WasReleased)
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x170000C5 RID: 197
		// (get) Token: 0x06000295 RID: 661 RVA: 0x00009910 File Offset: 0x00007B10
		public TwoAxisInputControl Direction
		{
			get
			{
				if (this.DPad.UpdateTick <= this.LeftStick.UpdateTick)
				{
					return this.LeftStick;
				}
				return this.DPad;
			}
		}

		// Token: 0x170000C6 RID: 198
		// (get) Token: 0x06000296 RID: 662 RVA: 0x00009938 File Offset: 0x00007B38
		public InputControl LeftStickUp
		{
			get
			{
				InputControl inputControl;
				if ((inputControl = this.cachedLeftStickUp) == null)
				{
					inputControl = (this.cachedLeftStickUp = this.GetControl(InputControlType.LeftStickUp));
				}
				return inputControl;
			}
		}

		// Token: 0x170000C7 RID: 199
		// (get) Token: 0x06000297 RID: 663 RVA: 0x00009960 File Offset: 0x00007B60
		public InputControl LeftStickDown
		{
			get
			{
				InputControl inputControl;
				if ((inputControl = this.cachedLeftStickDown) == null)
				{
					inputControl = (this.cachedLeftStickDown = this.GetControl(InputControlType.LeftStickDown));
				}
				return inputControl;
			}
		}

		// Token: 0x170000C8 RID: 200
		// (get) Token: 0x06000298 RID: 664 RVA: 0x00009988 File Offset: 0x00007B88
		public InputControl LeftStickLeft
		{
			get
			{
				InputControl inputControl;
				if ((inputControl = this.cachedLeftStickLeft) == null)
				{
					inputControl = (this.cachedLeftStickLeft = this.GetControl(InputControlType.LeftStickLeft));
				}
				return inputControl;
			}
		}

		// Token: 0x170000C9 RID: 201
		// (get) Token: 0x06000299 RID: 665 RVA: 0x000099B0 File Offset: 0x00007BB0
		public InputControl LeftStickRight
		{
			get
			{
				InputControl inputControl;
				if ((inputControl = this.cachedLeftStickRight) == null)
				{
					inputControl = (this.cachedLeftStickRight = this.GetControl(InputControlType.LeftStickRight));
				}
				return inputControl;
			}
		}

		// Token: 0x170000CA RID: 202
		// (get) Token: 0x0600029A RID: 666 RVA: 0x000099D8 File Offset: 0x00007BD8
		public InputControl RightStickUp
		{
			get
			{
				InputControl inputControl;
				if ((inputControl = this.cachedRightStickUp) == null)
				{
					inputControl = (this.cachedRightStickUp = this.GetControl(InputControlType.RightStickUp));
				}
				return inputControl;
			}
		}

		// Token: 0x170000CB RID: 203
		// (get) Token: 0x0600029B RID: 667 RVA: 0x00009A00 File Offset: 0x00007C00
		public InputControl RightStickDown
		{
			get
			{
				InputControl inputControl;
				if ((inputControl = this.cachedRightStickDown) == null)
				{
					inputControl = (this.cachedRightStickDown = this.GetControl(InputControlType.RightStickDown));
				}
				return inputControl;
			}
		}

		// Token: 0x170000CC RID: 204
		// (get) Token: 0x0600029C RID: 668 RVA: 0x00009A28 File Offset: 0x00007C28
		public InputControl RightStickLeft
		{
			get
			{
				InputControl inputControl;
				if ((inputControl = this.cachedRightStickLeft) == null)
				{
					inputControl = (this.cachedRightStickLeft = this.GetControl(InputControlType.RightStickLeft));
				}
				return inputControl;
			}
		}

		// Token: 0x170000CD RID: 205
		// (get) Token: 0x0600029D RID: 669 RVA: 0x00009A50 File Offset: 0x00007C50
		public InputControl RightStickRight
		{
			get
			{
				InputControl inputControl;
				if ((inputControl = this.cachedRightStickRight) == null)
				{
					inputControl = (this.cachedRightStickRight = this.GetControl(InputControlType.RightStickRight));
				}
				return inputControl;
			}
		}

		// Token: 0x170000CE RID: 206
		// (get) Token: 0x0600029E RID: 670 RVA: 0x00009A78 File Offset: 0x00007C78
		public InputControl DPadUp
		{
			get
			{
				InputControl inputControl;
				if ((inputControl = this.cachedDPadUp) == null)
				{
					inputControl = (this.cachedDPadUp = this.GetControl(InputControlType.DPadUp));
				}
				return inputControl;
			}
		}

		// Token: 0x170000CF RID: 207
		// (get) Token: 0x0600029F RID: 671 RVA: 0x00009AA0 File Offset: 0x00007CA0
		public InputControl DPadDown
		{
			get
			{
				InputControl inputControl;
				if ((inputControl = this.cachedDPadDown) == null)
				{
					inputControl = (this.cachedDPadDown = this.GetControl(InputControlType.DPadDown));
				}
				return inputControl;
			}
		}

		// Token: 0x170000D0 RID: 208
		// (get) Token: 0x060002A0 RID: 672 RVA: 0x00009AC8 File Offset: 0x00007CC8
		public InputControl DPadLeft
		{
			get
			{
				InputControl inputControl;
				if ((inputControl = this.cachedDPadLeft) == null)
				{
					inputControl = (this.cachedDPadLeft = this.GetControl(InputControlType.DPadLeft));
				}
				return inputControl;
			}
		}

		// Token: 0x170000D1 RID: 209
		// (get) Token: 0x060002A1 RID: 673 RVA: 0x00009AF0 File Offset: 0x00007CF0
		public InputControl DPadRight
		{
			get
			{
				InputControl inputControl;
				if ((inputControl = this.cachedDPadRight) == null)
				{
					inputControl = (this.cachedDPadRight = this.GetControl(InputControlType.DPadRight));
				}
				return inputControl;
			}
		}

		// Token: 0x170000D2 RID: 210
		// (get) Token: 0x060002A2 RID: 674 RVA: 0x00009B18 File Offset: 0x00007D18
		public InputControl Action1
		{
			get
			{
				InputControl inputControl;
				if ((inputControl = this.cachedAction1) == null)
				{
					inputControl = (this.cachedAction1 = this.GetControl(InputControlType.Action1));
				}
				return inputControl;
			}
		}

		// Token: 0x170000D3 RID: 211
		// (get) Token: 0x060002A3 RID: 675 RVA: 0x00009B40 File Offset: 0x00007D40
		public InputControl Action2
		{
			get
			{
				InputControl inputControl;
				if ((inputControl = this.cachedAction2) == null)
				{
					inputControl = (this.cachedAction2 = this.GetControl(InputControlType.Action2));
				}
				return inputControl;
			}
		}

		// Token: 0x170000D4 RID: 212
		// (get) Token: 0x060002A4 RID: 676 RVA: 0x00009B68 File Offset: 0x00007D68
		public InputControl Action3
		{
			get
			{
				InputControl inputControl;
				if ((inputControl = this.cachedAction3) == null)
				{
					inputControl = (this.cachedAction3 = this.GetControl(InputControlType.Action3));
				}
				return inputControl;
			}
		}

		// Token: 0x170000D5 RID: 213
		// (get) Token: 0x060002A5 RID: 677 RVA: 0x00009B90 File Offset: 0x00007D90
		public InputControl Action4
		{
			get
			{
				InputControl inputControl;
				if ((inputControl = this.cachedAction4) == null)
				{
					inputControl = (this.cachedAction4 = this.GetControl(InputControlType.Action4));
				}
				return inputControl;
			}
		}

		// Token: 0x170000D6 RID: 214
		// (get) Token: 0x060002A6 RID: 678 RVA: 0x00009BB8 File Offset: 0x00007DB8
		public InputControl LeftTrigger
		{
			get
			{
				InputControl inputControl;
				if ((inputControl = this.cachedLeftTrigger) == null)
				{
					inputControl = (this.cachedLeftTrigger = this.GetControl(InputControlType.LeftTrigger));
				}
				return inputControl;
			}
		}

		// Token: 0x170000D7 RID: 215
		// (get) Token: 0x060002A7 RID: 679 RVA: 0x00009BE0 File Offset: 0x00007DE0
		public InputControl RightTrigger
		{
			get
			{
				InputControl inputControl;
				if ((inputControl = this.cachedRightTrigger) == null)
				{
					inputControl = (this.cachedRightTrigger = this.GetControl(InputControlType.RightTrigger));
				}
				return inputControl;
			}
		}

		// Token: 0x170000D8 RID: 216
		// (get) Token: 0x060002A8 RID: 680 RVA: 0x00009C08 File Offset: 0x00007E08
		public InputControl LeftBumper
		{
			get
			{
				InputControl inputControl;
				if ((inputControl = this.cachedLeftBumper) == null)
				{
					inputControl = (this.cachedLeftBumper = this.GetControl(InputControlType.LeftBumper));
				}
				return inputControl;
			}
		}

		// Token: 0x170000D9 RID: 217
		// (get) Token: 0x060002A9 RID: 681 RVA: 0x00009C30 File Offset: 0x00007E30
		public InputControl RightBumper
		{
			get
			{
				InputControl inputControl;
				if ((inputControl = this.cachedRightBumper) == null)
				{
					inputControl = (this.cachedRightBumper = this.GetControl(InputControlType.RightBumper));
				}
				return inputControl;
			}
		}

		// Token: 0x170000DA RID: 218
		// (get) Token: 0x060002AA RID: 682 RVA: 0x00009C58 File Offset: 0x00007E58
		public InputControl LeftStickButton
		{
			get
			{
				InputControl inputControl;
				if ((inputControl = this.cachedLeftStickButton) == null)
				{
					inputControl = (this.cachedLeftStickButton = this.GetControl(InputControlType.LeftStickButton));
				}
				return inputControl;
			}
		}

		// Token: 0x170000DB RID: 219
		// (get) Token: 0x060002AB RID: 683 RVA: 0x00009C80 File Offset: 0x00007E80
		public InputControl RightStickButton
		{
			get
			{
				InputControl inputControl;
				if ((inputControl = this.cachedRightStickButton) == null)
				{
					inputControl = (this.cachedRightStickButton = this.GetControl(InputControlType.RightStickButton));
				}
				return inputControl;
			}
		}

		// Token: 0x170000DC RID: 220
		// (get) Token: 0x060002AC RID: 684 RVA: 0x00009CA8 File Offset: 0x00007EA8
		public InputControl LeftStickX
		{
			get
			{
				InputControl inputControl;
				if ((inputControl = this.cachedLeftStickX) == null)
				{
					inputControl = (this.cachedLeftStickX = this.GetControl(InputControlType.LeftStickX));
				}
				return inputControl;
			}
		}

		// Token: 0x170000DD RID: 221
		// (get) Token: 0x060002AD RID: 685 RVA: 0x00009CD4 File Offset: 0x00007ED4
		public InputControl LeftStickY
		{
			get
			{
				InputControl inputControl;
				if ((inputControl = this.cachedLeftStickY) == null)
				{
					inputControl = (this.cachedLeftStickY = this.GetControl(InputControlType.LeftStickY));
				}
				return inputControl;
			}
		}

		// Token: 0x170000DE RID: 222
		// (get) Token: 0x060002AE RID: 686 RVA: 0x00009D00 File Offset: 0x00007F00
		public InputControl RightStickX
		{
			get
			{
				InputControl inputControl;
				if ((inputControl = this.cachedRightStickX) == null)
				{
					inputControl = (this.cachedRightStickX = this.GetControl(InputControlType.RightStickX));
				}
				return inputControl;
			}
		}

		// Token: 0x170000DF RID: 223
		// (get) Token: 0x060002AF RID: 687 RVA: 0x00009D2C File Offset: 0x00007F2C
		public InputControl RightStickY
		{
			get
			{
				InputControl inputControl;
				if ((inputControl = this.cachedRightStickY) == null)
				{
					inputControl = (this.cachedRightStickY = this.GetControl(InputControlType.RightStickY));
				}
				return inputControl;
			}
		}

		// Token: 0x170000E0 RID: 224
		// (get) Token: 0x060002B0 RID: 688 RVA: 0x00009D58 File Offset: 0x00007F58
		public InputControl DPadX
		{
			get
			{
				InputControl inputControl;
				if ((inputControl = this.cachedDPadX) == null)
				{
					inputControl = (this.cachedDPadX = this.GetControl(InputControlType.DPadX));
				}
				return inputControl;
			}
		}

		// Token: 0x170000E1 RID: 225
		// (get) Token: 0x060002B1 RID: 689 RVA: 0x00009D84 File Offset: 0x00007F84
		public InputControl DPadY
		{
			get
			{
				InputControl inputControl;
				if ((inputControl = this.cachedDPadY) == null)
				{
					inputControl = (this.cachedDPadY = this.GetControl(InputControlType.DPadY));
				}
				return inputControl;
			}
		}

		// Token: 0x170000E2 RID: 226
		// (get) Token: 0x060002B2 RID: 690 RVA: 0x00009DB0 File Offset: 0x00007FB0
		public InputControl Command
		{
			get
			{
				InputControl inputControl;
				if ((inputControl = this.cachedCommand) == null)
				{
					inputControl = (this.cachedCommand = this.GetControl(InputControlType.Command));
				}
				return inputControl;
			}
		}

		// Token: 0x170000E3 RID: 227
		// (get) Token: 0x060002B3 RID: 691 RVA: 0x00009DDC File Offset: 0x00007FDC
		public InputControl LeftCommand
		{
			get
			{
				InputControl inputControl;
				if ((inputControl = this.cachedLeftCommand) == null)
				{
					inputControl = (this.cachedLeftCommand = this.GetControl(InputControlType.LeftCommand));
				}
				return inputControl;
			}
		}

		// Token: 0x170000E4 RID: 228
		// (get) Token: 0x060002B4 RID: 692 RVA: 0x00009E08 File Offset: 0x00008008
		public InputControl RightCommand
		{
			get
			{
				InputControl inputControl;
				if ((inputControl = this.cachedRightCommand) == null)
				{
					inputControl = (this.cachedRightCommand = this.GetControl(InputControlType.RightCommand));
				}
				return inputControl;
			}
		}

		// Token: 0x060002B5 RID: 693 RVA: 0x00009E34 File Offset: 0x00008034
		private void ExpireControlCache()
		{
			this.cachedLeftStickUp = null;
			this.cachedLeftStickDown = null;
			this.cachedLeftStickLeft = null;
			this.cachedLeftStickRight = null;
			this.cachedRightStickUp = null;
			this.cachedRightStickDown = null;
			this.cachedRightStickLeft = null;
			this.cachedRightStickRight = null;
			this.cachedDPadUp = null;
			this.cachedDPadDown = null;
			this.cachedDPadLeft = null;
			this.cachedDPadRight = null;
			this.cachedAction1 = null;
			this.cachedAction2 = null;
			this.cachedAction3 = null;
			this.cachedAction4 = null;
			this.cachedLeftTrigger = null;
			this.cachedRightTrigger = null;
			this.cachedLeftBumper = null;
			this.cachedRightBumper = null;
			this.cachedLeftStickButton = null;
			this.cachedRightStickButton = null;
			this.cachedLeftStickX = null;
			this.cachedLeftStickY = null;
			this.cachedRightStickX = null;
			this.cachedRightStickY = null;
			this.cachedDPadX = null;
			this.cachedDPadY = null;
			this.cachedCommand = null;
			this.cachedLeftCommand = null;
			this.cachedRightCommand = null;
		}

		// Token: 0x170000E5 RID: 229
		// (get) Token: 0x060002B6 RID: 694 RVA: 0x00009F1A File Offset: 0x0000811A
		public virtual int NumUnknownAnalogs
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x170000E6 RID: 230
		// (get) Token: 0x060002B7 RID: 695 RVA: 0x00009F1D File Offset: 0x0000811D
		public virtual int NumUnknownButtons
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x060002B8 RID: 696 RVA: 0x00009F20 File Offset: 0x00008120
		public virtual bool ReadRawButtonState(int index)
		{
			return false;
		}

		// Token: 0x060002B9 RID: 697 RVA: 0x00009F23 File Offset: 0x00008123
		public virtual float ReadRawAnalogValue(int index)
		{
			return 0f;
		}

		// Token: 0x060002BA RID: 698 RVA: 0x00009F2C File Offset: 0x0000812C
		public void TakeSnapshot()
		{
			if (this.AnalogSnapshot == null)
			{
				this.AnalogSnapshot = new InputDevice.AnalogSnapshotEntry[this.NumUnknownAnalogs];
			}
			for (int i = 0; i < this.NumUnknownAnalogs; i++)
			{
				float num = Utility.ApplySnapping(this.ReadRawAnalogValue(i), 0.5f);
				this.AnalogSnapshot[i].value = num;
			}
		}

		// Token: 0x060002BB RID: 699 RVA: 0x00009F88 File Offset: 0x00008188
		public UnknownDeviceControl GetFirstPressedAnalog()
		{
			if (this.AnalogSnapshot != null)
			{
				for (int i = 0; i < this.NumUnknownAnalogs; i++)
				{
					InputControlType inputControlType = InputControlType.Analog0 + i;
					float num = Utility.ApplySnapping(this.ReadRawAnalogValue(i), 0.5f);
					float num2 = num - this.AnalogSnapshot[i].value;
					this.AnalogSnapshot[i].TrackMinMaxValue(num);
					if (num2 > 0.1f)
					{
						num2 = this.AnalogSnapshot[i].maxValue - this.AnalogSnapshot[i].value;
					}
					if (num2 < -0.1f)
					{
						num2 = this.AnalogSnapshot[i].minValue - this.AnalogSnapshot[i].value;
					}
					if (num2 > 1.9f)
					{
						return new UnknownDeviceControl(inputControlType, InputRangeType.MinusOneToOne);
					}
					if (num2 < -0.9f)
					{
						return new UnknownDeviceControl(inputControlType, InputRangeType.ZeroToMinusOne);
					}
					if (num2 > 0.9f)
					{
						return new UnknownDeviceControl(inputControlType, InputRangeType.ZeroToOne);
					}
				}
			}
			return UnknownDeviceControl.None;
		}

		// Token: 0x060002BC RID: 700 RVA: 0x0000A084 File Offset: 0x00008284
		public UnknownDeviceControl GetFirstPressedButton()
		{
			for (int i = 0; i < this.NumUnknownButtons; i++)
			{
				if (this.ReadRawButtonState(i))
				{
					return new UnknownDeviceControl(InputControlType.Button0 + i, InputRangeType.ZeroToOne);
				}
			}
			return UnknownDeviceControl.None;
		}

		// Token: 0x040002A3 RID: 675
		public static readonly InputDevice Null = new InputDevice("None");

		// Token: 0x040002AE RID: 686
		private readonly List<InputControl> controls;

		// Token: 0x040002B4 RID: 692
		private bool hasLeftCommandControl;

		// Token: 0x040002B5 RID: 693
		private InputControl leftCommandSource;

		// Token: 0x040002B7 RID: 695
		private bool hasRightCommandControl;

		// Token: 0x040002B8 RID: 696
		private InputControl rightCommandSource;

		// Token: 0x040002BA RID: 698
		public bool Passive;

		// Token: 0x040002BC RID: 700
		private InputControl cachedLeftStickUp;

		// Token: 0x040002BD RID: 701
		private InputControl cachedLeftStickDown;

		// Token: 0x040002BE RID: 702
		private InputControl cachedLeftStickLeft;

		// Token: 0x040002BF RID: 703
		private InputControl cachedLeftStickRight;

		// Token: 0x040002C0 RID: 704
		private InputControl cachedRightStickUp;

		// Token: 0x040002C1 RID: 705
		private InputControl cachedRightStickDown;

		// Token: 0x040002C2 RID: 706
		private InputControl cachedRightStickLeft;

		// Token: 0x040002C3 RID: 707
		private InputControl cachedRightStickRight;

		// Token: 0x040002C4 RID: 708
		private InputControl cachedDPadUp;

		// Token: 0x040002C5 RID: 709
		private InputControl cachedDPadDown;

		// Token: 0x040002C6 RID: 710
		private InputControl cachedDPadLeft;

		// Token: 0x040002C7 RID: 711
		private InputControl cachedDPadRight;

		// Token: 0x040002C8 RID: 712
		private InputControl cachedAction1;

		// Token: 0x040002C9 RID: 713
		private InputControl cachedAction2;

		// Token: 0x040002CA RID: 714
		private InputControl cachedAction3;

		// Token: 0x040002CB RID: 715
		private InputControl cachedAction4;

		// Token: 0x040002CC RID: 716
		private InputControl cachedLeftTrigger;

		// Token: 0x040002CD RID: 717
		private InputControl cachedRightTrigger;

		// Token: 0x040002CE RID: 718
		private InputControl cachedLeftBumper;

		// Token: 0x040002CF RID: 719
		private InputControl cachedRightBumper;

		// Token: 0x040002D0 RID: 720
		private InputControl cachedLeftStickButton;

		// Token: 0x040002D1 RID: 721
		private InputControl cachedRightStickButton;

		// Token: 0x040002D2 RID: 722
		private InputControl cachedLeftStickX;

		// Token: 0x040002D3 RID: 723
		private InputControl cachedLeftStickY;

		// Token: 0x040002D4 RID: 724
		private InputControl cachedRightStickX;

		// Token: 0x040002D5 RID: 725
		private InputControl cachedRightStickY;

		// Token: 0x040002D6 RID: 726
		private InputControl cachedDPadX;

		// Token: 0x040002D7 RID: 727
		private InputControl cachedDPadY;

		// Token: 0x040002D8 RID: 728
		private InputControl cachedCommand;

		// Token: 0x040002D9 RID: 729
		private InputControl cachedLeftCommand;

		// Token: 0x040002DA RID: 730
		private InputControl cachedRightCommand;

		// Token: 0x0200021A RID: 538
		protected struct AnalogSnapshotEntry
		{
			// Token: 0x06000950 RID: 2384 RVA: 0x00053266 File Offset: 0x00051466
			public void TrackMinMaxValue(float currentValue)
			{
				this.maxValue = Mathf.Max(this.maxValue, currentValue);
				this.minValue = Mathf.Min(this.minValue, currentValue);
			}

			// Token: 0x040004BD RID: 1213
			public float value;

			// Token: 0x040004BE RID: 1214
			public float maxValue;

			// Token: 0x040004BF RID: 1215
			public float minValue;
		}
	}
}
