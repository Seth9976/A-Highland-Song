using System;

namespace Ink
{
	// Token: 0x0200000F RID: 15
	public class StringParserState
	{
		// Token: 0x17000011 RID: 17
		// (get) Token: 0x060000E6 RID: 230 RVA: 0x00007304 File Offset: 0x00005504
		// (set) Token: 0x060000E7 RID: 231 RVA: 0x00007311 File Offset: 0x00005511
		public int lineIndex
		{
			get
			{
				return this.currentElement.lineIndex;
			}
			set
			{
				this.currentElement.lineIndex = value;
			}
		}

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x060000E8 RID: 232 RVA: 0x0000731F File Offset: 0x0000551F
		// (set) Token: 0x060000E9 RID: 233 RVA: 0x0000732C File Offset: 0x0000552C
		public int characterIndex
		{
			get
			{
				return this.currentElement.characterIndex;
			}
			set
			{
				this.currentElement.characterIndex = value;
			}
		}

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x060000EA RID: 234 RVA: 0x0000733A File Offset: 0x0000553A
		// (set) Token: 0x060000EB RID: 235 RVA: 0x00007347 File Offset: 0x00005547
		public int characterInLineIndex
		{
			get
			{
				return this.currentElement.characterInLineIndex;
			}
			set
			{
				this.currentElement.characterInLineIndex = value;
			}
		}

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x060000EC RID: 236 RVA: 0x00007355 File Offset: 0x00005555
		// (set) Token: 0x060000ED RID: 237 RVA: 0x00007362 File Offset: 0x00005562
		public uint customFlags
		{
			get
			{
				return this.currentElement.customFlags;
			}
			set
			{
				this.currentElement.customFlags = value;
			}
		}

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x060000EE RID: 238 RVA: 0x00007370 File Offset: 0x00005570
		public bool errorReportedAlreadyInScope
		{
			get
			{
				return this.currentElement.reportedErrorInScope;
			}
		}

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x060000EF RID: 239 RVA: 0x0000737D File Offset: 0x0000557D
		public int stackHeight
		{
			get
			{
				return this._numElements;
			}
		}

		// Token: 0x060000F0 RID: 240 RVA: 0x00007388 File Offset: 0x00005588
		public StringParserState()
		{
			this._stack = new StringParserState.Element[200];
			for (int i = 0; i < 200; i++)
			{
				this._stack[i] = new StringParserState.Element();
			}
			this._numElements = 1;
		}

		// Token: 0x060000F1 RID: 241 RVA: 0x000073D0 File Offset: 0x000055D0
		public int Push()
		{
			if (this._numElements >= this._stack.Length)
			{
				throw new Exception("Stack overflow in parser state");
			}
			StringParserState.Element element = this._stack[this._numElements - 1];
			StringParserState.Element element2 = this._stack[this._numElements];
			this._numElements++;
			element2.CopyFrom(element);
			return element2.uniqueId;
		}

		// Token: 0x060000F2 RID: 242 RVA: 0x0000742F File Offset: 0x0000562F
		public void Pop(int expectedRuleId)
		{
			if (this._numElements == 1)
			{
				throw new Exception("Attempting to remove final stack element is illegal! Mismatched Begin/Succceed/Fail?");
			}
			if (this.currentElement.uniqueId != expectedRuleId)
			{
				throw new Exception("Mismatched rule IDs - do you have mismatched Begin/Succeed/Fail?");
			}
			this._numElements--;
		}

		// Token: 0x060000F3 RID: 243 RVA: 0x0000746C File Offset: 0x0000566C
		public StringParserState.Element Peek(int expectedRuleId)
		{
			if (this.currentElement.uniqueId != expectedRuleId)
			{
				throw new Exception("Mismatched rule IDs - do you have mismatched Begin/Succeed/Fail?");
			}
			return this._stack[this._numElements - 1];
		}

		// Token: 0x060000F4 RID: 244 RVA: 0x00007496 File Offset: 0x00005696
		public StringParserState.Element PeekPenultimate()
		{
			if (this._numElements >= 2)
			{
				return this._stack[this._numElements - 2];
			}
			return null;
		}

		// Token: 0x060000F5 RID: 245 RVA: 0x000074B4 File Offset: 0x000056B4
		public void Squash()
		{
			if (this._numElements < 2)
			{
				throw new Exception("Attempting to remove final stack element is illegal! Mismatched Begin/Succceed/Fail?");
			}
			StringParserState.Element element = this._stack[this._numElements - 2];
			StringParserState.Element element2 = this._stack[this._numElements - 1];
			element.SquashFrom(element2);
			this._numElements--;
		}

		// Token: 0x060000F6 RID: 246 RVA: 0x00007508 File Offset: 0x00005708
		public void NoteErrorReported()
		{
			StringParserState.Element[] stack = this._stack;
			for (int i = 0; i < stack.Length; i++)
			{
				stack[i].reportedErrorInScope = true;
			}
		}

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x060000F7 RID: 247 RVA: 0x00007533 File Offset: 0x00005733
		protected StringParserState.Element currentElement
		{
			get
			{
				return this._stack[this._numElements - 1];
			}
		}

		// Token: 0x04000042 RID: 66
		private StringParserState.Element[] _stack;

		// Token: 0x04000043 RID: 67
		private int _numElements;

		// Token: 0x02000085 RID: 133
		public class Element
		{
			// Token: 0x06000537 RID: 1335 RVA: 0x0001B20C File Offset: 0x0001940C
			public void CopyFrom(StringParserState.Element fromElement)
			{
				StringParserState.Element._uniqueIdCounter++;
				this.uniqueId = StringParserState.Element._uniqueIdCounter;
				this.characterIndex = fromElement.characterIndex;
				this.characterInLineIndex = fromElement.characterInLineIndex;
				this.lineIndex = fromElement.lineIndex;
				this.customFlags = fromElement.customFlags;
				this.reportedErrorInScope = false;
			}

			// Token: 0x06000538 RID: 1336 RVA: 0x0001B267 File Offset: 0x00019467
			public void SquashFrom(StringParserState.Element fromElement)
			{
				this.characterIndex = fromElement.characterIndex;
				this.characterInLineIndex = fromElement.characterInLineIndex;
				this.lineIndex = fromElement.lineIndex;
				this.reportedErrorInScope = fromElement.reportedErrorInScope;
			}

			// Token: 0x040001E0 RID: 480
			public int characterIndex;

			// Token: 0x040001E1 RID: 481
			public int characterInLineIndex;

			// Token: 0x040001E2 RID: 482
			public int lineIndex;

			// Token: 0x040001E3 RID: 483
			public bool reportedErrorInScope;

			// Token: 0x040001E4 RID: 484
			public int uniqueId;

			// Token: 0x040001E5 RID: 485
			public uint customFlags;

			// Token: 0x040001E6 RID: 486
			private static int _uniqueIdCounter;
		}
	}
}
