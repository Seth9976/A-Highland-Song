using System;

namespace Ink.Runtime
{
	// Token: 0x02000014 RID: 20
	public class ChoicePoint : Object
	{
		// Token: 0x17000026 RID: 38
		// (get) Token: 0x06000121 RID: 289 RVA: 0x00007C6C File Offset: 0x00005E6C
		// (set) Token: 0x06000122 RID: 290 RVA: 0x00007CAF File Offset: 0x00005EAF
		public Path pathOnChoice
		{
			get
			{
				if (this._pathOnChoice != null && this._pathOnChoice.isRelative)
				{
					Container choiceTarget = this.choiceTarget;
					if (choiceTarget)
					{
						this._pathOnChoice = choiceTarget.path;
					}
				}
				return this._pathOnChoice;
			}
			set
			{
				this._pathOnChoice = value;
			}
		}

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x06000123 RID: 291 RVA: 0x00007CB8 File Offset: 0x00005EB8
		public Container choiceTarget
		{
			get
			{
				return base.ResolvePath(this._pathOnChoice).container;
			}
		}

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x06000124 RID: 292 RVA: 0x00007CD9 File Offset: 0x00005ED9
		// (set) Token: 0x06000125 RID: 293 RVA: 0x00007CE7 File Offset: 0x00005EE7
		public string pathStringOnChoice
		{
			get
			{
				return base.CompactPathString(this.pathOnChoice);
			}
			set
			{
				this.pathOnChoice = new Path(value);
			}
		}

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x06000126 RID: 294 RVA: 0x00007CF5 File Offset: 0x00005EF5
		// (set) Token: 0x06000127 RID: 295 RVA: 0x00007CFD File Offset: 0x00005EFD
		public bool hasCondition { get; set; }

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x06000128 RID: 296 RVA: 0x00007D06 File Offset: 0x00005F06
		// (set) Token: 0x06000129 RID: 297 RVA: 0x00007D0E File Offset: 0x00005F0E
		public bool hasStartContent { get; set; }

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x0600012A RID: 298 RVA: 0x00007D17 File Offset: 0x00005F17
		// (set) Token: 0x0600012B RID: 299 RVA: 0x00007D1F File Offset: 0x00005F1F
		public bool hasChoiceOnlyContent { get; set; }

		// Token: 0x1700002C RID: 44
		// (get) Token: 0x0600012C RID: 300 RVA: 0x00007D28 File Offset: 0x00005F28
		// (set) Token: 0x0600012D RID: 301 RVA: 0x00007D30 File Offset: 0x00005F30
		public bool onceOnly { get; set; }

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x0600012E RID: 302 RVA: 0x00007D39 File Offset: 0x00005F39
		// (set) Token: 0x0600012F RID: 303 RVA: 0x00007D41 File Offset: 0x00005F41
		public bool isInvisibleDefault { get; set; }

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x06000130 RID: 304 RVA: 0x00007D4C File Offset: 0x00005F4C
		// (set) Token: 0x06000131 RID: 305 RVA: 0x00007D99 File Offset: 0x00005F99
		public int flags
		{
			get
			{
				int num = 0;
				if (this.hasCondition)
				{
					num |= 1;
				}
				if (this.hasStartContent)
				{
					num |= 2;
				}
				if (this.hasChoiceOnlyContent)
				{
					num |= 4;
				}
				if (this.isInvisibleDefault)
				{
					num |= 8;
				}
				if (this.onceOnly)
				{
					num |= 16;
				}
				return num;
			}
			set
			{
				this.hasCondition = (value & 1) > 0;
				this.hasStartContent = (value & 2) > 0;
				this.hasChoiceOnlyContent = (value & 4) > 0;
				this.isInvisibleDefault = (value & 8) > 0;
				this.onceOnly = (value & 16) > 0;
			}
		}

		// Token: 0x06000132 RID: 306 RVA: 0x00007DD8 File Offset: 0x00005FD8
		public ChoicePoint(bool onceOnly)
		{
			this.onceOnly = onceOnly;
		}

		// Token: 0x06000133 RID: 307 RVA: 0x00007DE7 File Offset: 0x00005FE7
		public ChoicePoint()
			: this(true)
		{
		}

		// Token: 0x06000134 RID: 308 RVA: 0x00007DF0 File Offset: 0x00005FF0
		public override string ToString()
		{
			int? num = base.DebugLineNumberOfPath(this.pathOnChoice);
			string text = this.pathOnChoice.ToString();
			if (num != null)
			{
				string[] array = new string[5];
				array[0] = " line ";
				int num2 = 1;
				int? num3 = num;
				array[num2] = num3.ToString();
				array[2] = "(";
				array[3] = text;
				array[4] = ")";
				text = string.Concat(array);
			}
			return "Choice: -> " + text;
		}

		// Token: 0x04000052 RID: 82
		private Path _pathOnChoice;
	}
}
