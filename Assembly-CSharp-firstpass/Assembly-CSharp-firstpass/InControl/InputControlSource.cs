using System;
using UnityEngine;

namespace InControl
{
	// Token: 0x02000034 RID: 52
	[Serializable]
	public struct InputControlSource
	{
		// Token: 0x1700007A RID: 122
		// (get) Token: 0x060001CA RID: 458 RVA: 0x00007599 File Offset: 0x00005799
		// (set) Token: 0x060001CB RID: 459 RVA: 0x000075A1 File Offset: 0x000057A1
		public InputControlSourceType SourceType
		{
			get
			{
				return this.sourceType;
			}
			set
			{
				this.sourceType = value;
			}
		}

		// Token: 0x1700007B RID: 123
		// (get) Token: 0x060001CC RID: 460 RVA: 0x000075AA File Offset: 0x000057AA
		// (set) Token: 0x060001CD RID: 461 RVA: 0x000075B2 File Offset: 0x000057B2
		public int Index
		{
			get
			{
				return this.index;
			}
			set
			{
				this.index = value;
			}
		}

		// Token: 0x060001CE RID: 462 RVA: 0x000075BB File Offset: 0x000057BB
		public InputControlSource(InputControlSourceType sourceType, int index)
		{
			this.sourceType = sourceType;
			this.index = index;
		}

		// Token: 0x060001CF RID: 463 RVA: 0x000075CB File Offset: 0x000057CB
		public InputControlSource(KeyCode keyCode)
		{
			this = new InputControlSource(InputControlSourceType.KeyCode, (int)keyCode);
		}

		// Token: 0x060001D0 RID: 464 RVA: 0x000075D8 File Offset: 0x000057D8
		public float GetValue(InputDevice inputDevice)
		{
			switch (this.SourceType)
			{
			case InputControlSourceType.None:
				return 0f;
			case InputControlSourceType.Button:
				if (!this.GetState(inputDevice))
				{
					return 0f;
				}
				return 1f;
			case InputControlSourceType.Analog:
				return inputDevice.ReadRawAnalogValue(this.Index);
			case InputControlSourceType.KeyCode:
				if (!this.GetState(inputDevice))
				{
					return 0f;
				}
				return 1f;
			default:
				throw new ArgumentOutOfRangeException();
			}
		}

		// Token: 0x060001D1 RID: 465 RVA: 0x00007648 File Offset: 0x00005848
		public bool GetState(InputDevice inputDevice)
		{
			switch (this.SourceType)
			{
			case InputControlSourceType.None:
				return false;
			case InputControlSourceType.Button:
				return inputDevice.ReadRawButtonState(this.Index);
			case InputControlSourceType.Analog:
				return Utility.IsNotZero(this.GetValue(inputDevice));
			case InputControlSourceType.KeyCode:
				return Input.GetKey((KeyCode)this.Index);
			default:
				throw new ArgumentOutOfRangeException();
			}
		}

		// Token: 0x060001D2 RID: 466 RVA: 0x000076A4 File Offset: 0x000058A4
		public string ToCode()
		{
			return string.Concat(new string[]
			{
				"new InputControlSource( InputControlSourceType.",
				this.SourceType.ToString(),
				", ",
				this.Index.ToString(),
				" )"
			});
		}

		// Token: 0x040001C3 RID: 451
		[SerializeField]
		private InputControlSourceType sourceType;

		// Token: 0x040001C4 RID: 452
		[SerializeField]
		private int index;
	}
}
