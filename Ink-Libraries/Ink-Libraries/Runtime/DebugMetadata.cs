using System;

namespace Ink.Runtime
{
	// Token: 0x02000017 RID: 23
	public class DebugMetadata
	{
		// Token: 0x06000172 RID: 370 RVA: 0x00008850 File Offset: 0x00006A50
		public DebugMetadata Merge(DebugMetadata dm)
		{
			DebugMetadata debugMetadata = new DebugMetadata();
			debugMetadata.fileName = this.fileName;
			debugMetadata.sourceName = this.sourceName;
			if (this.startLineNumber < dm.startLineNumber)
			{
				debugMetadata.startLineNumber = this.startLineNumber;
				debugMetadata.startCharacterNumber = this.startCharacterNumber;
			}
			else if (this.startLineNumber > dm.startLineNumber)
			{
				debugMetadata.startLineNumber = dm.startLineNumber;
				debugMetadata.startCharacterNumber = dm.startCharacterNumber;
			}
			else
			{
				debugMetadata.startLineNumber = this.startLineNumber;
				debugMetadata.startCharacterNumber = Math.Min(this.startCharacterNumber, dm.startCharacterNumber);
			}
			if (this.endLineNumber > dm.endLineNumber)
			{
				debugMetadata.endLineNumber = this.endLineNumber;
				debugMetadata.endCharacterNumber = this.endCharacterNumber;
			}
			else if (this.endLineNumber < dm.endLineNumber)
			{
				debugMetadata.endLineNumber = dm.endLineNumber;
				debugMetadata.endCharacterNumber = dm.endCharacterNumber;
			}
			else
			{
				debugMetadata.endLineNumber = this.endLineNumber;
				debugMetadata.endCharacterNumber = Math.Max(this.endCharacterNumber, dm.endCharacterNumber);
			}
			return debugMetadata;
		}

		// Token: 0x06000173 RID: 371 RVA: 0x00008962 File Offset: 0x00006B62
		public override string ToString()
		{
			if (this.fileName != null)
			{
				return string.Format("line {0} of {1}", this.startLineNumber, this.fileName);
			}
			return "line " + this.startLineNumber.ToString();
		}

		// Token: 0x04000060 RID: 96
		public int startLineNumber;

		// Token: 0x04000061 RID: 97
		public int endLineNumber;

		// Token: 0x04000062 RID: 98
		public int startCharacterNumber;

		// Token: 0x04000063 RID: 99
		public int endCharacterNumber;

		// Token: 0x04000064 RID: 100
		public string fileName;

		// Token: 0x04000065 RID: 101
		public string sourceName;
	}
}
