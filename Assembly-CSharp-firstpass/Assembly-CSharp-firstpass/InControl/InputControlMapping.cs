using System;
using UnityEngine;

namespace InControl
{
	// Token: 0x02000032 RID: 50
	[Serializable]
	public class InputControlMapping
	{
		// Token: 0x1700006D RID: 109
		// (get) Token: 0x060001AE RID: 430 RVA: 0x000073BC File Offset: 0x000055BC
		// (set) Token: 0x060001AF RID: 431 RVA: 0x000073F1 File Offset: 0x000055F1
		public string Name
		{
			get
			{
				if (!string.IsNullOrEmpty(this.name))
				{
					return this.name;
				}
				return this.Target.ToString();
			}
			set
			{
				this.name = value;
			}
		}

		// Token: 0x1700006E RID: 110
		// (get) Token: 0x060001B0 RID: 432 RVA: 0x000073FA File Offset: 0x000055FA
		// (set) Token: 0x060001B1 RID: 433 RVA: 0x00007402 File Offset: 0x00005602
		public bool Invert
		{
			get
			{
				return this.invert;
			}
			set
			{
				this.invert = value;
			}
		}

		// Token: 0x1700006F RID: 111
		// (get) Token: 0x060001B2 RID: 434 RVA: 0x0000740B File Offset: 0x0000560B
		// (set) Token: 0x060001B3 RID: 435 RVA: 0x00007413 File Offset: 0x00005613
		public float Scale
		{
			get
			{
				return this.scale;
			}
			set
			{
				this.scale = value;
			}
		}

		// Token: 0x17000070 RID: 112
		// (get) Token: 0x060001B4 RID: 436 RVA: 0x0000741C File Offset: 0x0000561C
		// (set) Token: 0x060001B5 RID: 437 RVA: 0x00007424 File Offset: 0x00005624
		public bool Raw
		{
			get
			{
				return this.raw;
			}
			set
			{
				this.raw = value;
			}
		}

		// Token: 0x17000071 RID: 113
		// (get) Token: 0x060001B6 RID: 438 RVA: 0x0000742D File Offset: 0x0000562D
		// (set) Token: 0x060001B7 RID: 439 RVA: 0x00007435 File Offset: 0x00005635
		public bool Passive
		{
			get
			{
				return this.passive;
			}
			set
			{
				this.passive = value;
			}
		}

		// Token: 0x17000072 RID: 114
		// (get) Token: 0x060001B8 RID: 440 RVA: 0x0000743E File Offset: 0x0000563E
		// (set) Token: 0x060001B9 RID: 441 RVA: 0x00007446 File Offset: 0x00005646
		public bool IgnoreInitialZeroValue
		{
			get
			{
				return this.ignoreInitialZeroValue;
			}
			set
			{
				this.ignoreInitialZeroValue = value;
			}
		}

		// Token: 0x17000073 RID: 115
		// (get) Token: 0x060001BA RID: 442 RVA: 0x0000744F File Offset: 0x0000564F
		// (set) Token: 0x060001BB RID: 443 RVA: 0x00007457 File Offset: 0x00005657
		public float Sensitivity
		{
			get
			{
				return this.sensitivity;
			}
			set
			{
				this.sensitivity = Mathf.Clamp01(value);
			}
		}

		// Token: 0x17000074 RID: 116
		// (get) Token: 0x060001BC RID: 444 RVA: 0x00007465 File Offset: 0x00005665
		// (set) Token: 0x060001BD RID: 445 RVA: 0x0000746D File Offset: 0x0000566D
		public float LowerDeadZone
		{
			get
			{
				return this.lowerDeadZone;
			}
			set
			{
				this.lowerDeadZone = Mathf.Clamp01(value);
			}
		}

		// Token: 0x17000075 RID: 117
		// (get) Token: 0x060001BE RID: 446 RVA: 0x0000747B File Offset: 0x0000567B
		// (set) Token: 0x060001BF RID: 447 RVA: 0x00007483 File Offset: 0x00005683
		public float UpperDeadZone
		{
			get
			{
				return this.upperDeadZone;
			}
			set
			{
				this.upperDeadZone = Mathf.Clamp01(value);
			}
		}

		// Token: 0x17000076 RID: 118
		// (get) Token: 0x060001C0 RID: 448 RVA: 0x00007491 File Offset: 0x00005691
		// (set) Token: 0x060001C1 RID: 449 RVA: 0x00007499 File Offset: 0x00005699
		public InputControlSource Source
		{
			get
			{
				return this.source;
			}
			set
			{
				this.source = value;
			}
		}

		// Token: 0x17000077 RID: 119
		// (get) Token: 0x060001C2 RID: 450 RVA: 0x000074A2 File Offset: 0x000056A2
		// (set) Token: 0x060001C3 RID: 451 RVA: 0x000074AA File Offset: 0x000056AA
		public InputControlType Target
		{
			get
			{
				return this.target;
			}
			set
			{
				this.target = value;
			}
		}

		// Token: 0x17000078 RID: 120
		// (get) Token: 0x060001C4 RID: 452 RVA: 0x000074B3 File Offset: 0x000056B3
		// (set) Token: 0x060001C5 RID: 453 RVA: 0x000074BB File Offset: 0x000056BB
		public InputRangeType SourceRange
		{
			get
			{
				return this.sourceRange;
			}
			set
			{
				this.sourceRange = value;
			}
		}

		// Token: 0x17000079 RID: 121
		// (get) Token: 0x060001C6 RID: 454 RVA: 0x000074C4 File Offset: 0x000056C4
		// (set) Token: 0x060001C7 RID: 455 RVA: 0x000074CC File Offset: 0x000056CC
		public InputRangeType TargetRange
		{
			get
			{
				return this.targetRange;
			}
			set
			{
				this.targetRange = value;
			}
		}

		// Token: 0x060001C8 RID: 456 RVA: 0x000074D8 File Offset: 0x000056D8
		public float ApplyToValue(float value)
		{
			if (this.Raw)
			{
				value *= this.Scale;
				value = (InputRange.Excludes(this.sourceRange, value) ? 0f : value);
			}
			else
			{
				value = Mathf.Clamp(value * this.Scale, -1f, 1f);
				value = InputRange.Remap(value, this.sourceRange, this.targetRange);
			}
			if (this.Invert)
			{
				value = -value;
			}
			return value;
		}

		// Token: 0x040001B1 RID: 433
		[SerializeField]
		private string name = "";

		// Token: 0x040001B2 RID: 434
		[SerializeField]
		private bool invert;

		// Token: 0x040001B3 RID: 435
		[SerializeField]
		private float scale = 1f;

		// Token: 0x040001B4 RID: 436
		[SerializeField]
		private bool raw;

		// Token: 0x040001B5 RID: 437
		[SerializeField]
		private bool passive;

		// Token: 0x040001B6 RID: 438
		[SerializeField]
		private bool ignoreInitialZeroValue;

		// Token: 0x040001B7 RID: 439
		[SerializeField]
		private float sensitivity = 1f;

		// Token: 0x040001B8 RID: 440
		[SerializeField]
		private float lowerDeadZone;

		// Token: 0x040001B9 RID: 441
		[SerializeField]
		private float upperDeadZone = 1f;

		// Token: 0x040001BA RID: 442
		[SerializeField]
		private InputControlSource source;

		// Token: 0x040001BB RID: 443
		[SerializeField]
		private InputControlType target;

		// Token: 0x040001BC RID: 444
		[SerializeField]
		private InputRangeType sourceRange = InputRangeType.MinusOneToOne;

		// Token: 0x040001BD RID: 445
		[SerializeField]
		private InputRangeType targetRange = InputRangeType.MinusOneToOne;
	}
}
