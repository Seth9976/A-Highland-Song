using System;

namespace Ink.Runtime
{
	// Token: 0x0200001C RID: 28
	public struct InkListItem
	{
		// Token: 0x06000193 RID: 403 RVA: 0x00008FB3 File Offset: 0x000071B3
		public InkListItem(string originName, string itemName)
		{
			this.originName = originName;
			this.itemName = itemName;
		}

		// Token: 0x06000194 RID: 404 RVA: 0x00008FC4 File Offset: 0x000071C4
		public InkListItem(string fullName)
		{
			string[] array = fullName.Split('.', StringSplitOptions.None);
			this.originName = array[0];
			this.itemName = array[1];
		}

		// Token: 0x17000046 RID: 70
		// (get) Token: 0x06000195 RID: 405 RVA: 0x00008FED File Offset: 0x000071ED
		public static InkListItem Null
		{
			get
			{
				return new InkListItem(null, null);
			}
		}

		// Token: 0x17000047 RID: 71
		// (get) Token: 0x06000196 RID: 406 RVA: 0x00008FF6 File Offset: 0x000071F6
		public bool isNull
		{
			get
			{
				return this.originName == null && this.itemName == null;
			}
		}

		// Token: 0x17000048 RID: 72
		// (get) Token: 0x06000197 RID: 407 RVA: 0x0000900B File Offset: 0x0000720B
		public string fullName
		{
			get
			{
				return (this.originName ?? "?") + "." + this.itemName;
			}
		}

		// Token: 0x06000198 RID: 408 RVA: 0x0000902C File Offset: 0x0000722C
		public override string ToString()
		{
			return this.fullName;
		}

		// Token: 0x06000199 RID: 409 RVA: 0x00009034 File Offset: 0x00007234
		public override bool Equals(object obj)
		{
			return obj is InkListItem && this.Equals((InkListItem)obj);
		}

		// Token: 0x0600019A RID: 410 RVA: 0x0000904C File Offset: 0x0000724C
		public bool Equals(InkListItem otherItem)
		{
			return otherItem.itemName == this.itemName && otherItem.originName == this.originName;
		}

		// Token: 0x0600019B RID: 411 RVA: 0x00009074 File Offset: 0x00007274
		public static bool operator ==(InkListItem left, InkListItem right)
		{
			return left.Equals(right);
		}

		// Token: 0x0600019C RID: 412 RVA: 0x0000907E File Offset: 0x0000727E
		public static bool operator !=(InkListItem left, InkListItem right)
		{
			return !(left == right);
		}

		// Token: 0x0600019D RID: 413 RVA: 0x0000908C File Offset: 0x0000728C
		public override int GetHashCode()
		{
			int num = 0;
			int hashCode = this.itemName.GetHashCode();
			if (this.originName != null)
			{
				num = this.originName.GetHashCode();
			}
			return num + hashCode;
		}

		// Token: 0x04000072 RID: 114
		public readonly string originName;

		// Token: 0x04000073 RID: 115
		public readonly string itemName;
	}
}
