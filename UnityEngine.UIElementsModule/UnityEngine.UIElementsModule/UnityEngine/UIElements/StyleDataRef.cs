using System;

namespace UnityEngine.UIElements
{
	// Token: 0x0200027D RID: 637
	internal struct StyleDataRef<T> : IEquatable<StyleDataRef<T>> where T : struct, IEquatable<T>, IStyleDataGroup<T>
	{
		// Token: 0x170004CF RID: 1231
		// (get) Token: 0x06001490 RID: 5264 RVA: 0x00057C66 File Offset: 0x00055E66
		public int refCount
		{
			get
			{
				StyleDataRef<T>.RefCounted @ref = this.m_Ref;
				return (@ref != null) ? @ref.refCount : 0;
			}
		}

		// Token: 0x170004D0 RID: 1232
		// (get) Token: 0x06001491 RID: 5265 RVA: 0x00057C7A File Offset: 0x00055E7A
		public uint id
		{
			get
			{
				StyleDataRef<T>.RefCounted @ref = this.m_Ref;
				return (@ref != null) ? @ref.id : 0U;
			}
		}

		// Token: 0x06001492 RID: 5266 RVA: 0x00057C90 File Offset: 0x00055E90
		public StyleDataRef<T> Acquire()
		{
			this.m_Ref.Acquire();
			return this;
		}

		// Token: 0x06001493 RID: 5267 RVA: 0x00057CB4 File Offset: 0x00055EB4
		public void Release()
		{
			this.m_Ref.Release();
			this.m_Ref = null;
		}

		// Token: 0x06001494 RID: 5268 RVA: 0x00057CCC File Offset: 0x00055ECC
		public void CopyFrom(StyleDataRef<T> other)
		{
			bool flag = this.m_Ref.refCount == 1;
			if (flag)
			{
				this.m_Ref.value.CopyFrom(ref other.m_Ref.value);
			}
			else
			{
				this.m_Ref.Release();
				this.m_Ref = other.m_Ref;
				this.m_Ref.Acquire();
			}
		}

		// Token: 0x06001495 RID: 5269 RVA: 0x00057D38 File Offset: 0x00055F38
		public readonly ref T Read()
		{
			return ref this.m_Ref.value;
		}

		// Token: 0x06001496 RID: 5270 RVA: 0x00057D48 File Offset: 0x00055F48
		public ref T Write()
		{
			bool flag = this.m_Ref.refCount == 1;
			ref T ptr;
			if (flag)
			{
				ptr = ref this.m_Ref.value;
			}
			else
			{
				StyleDataRef<T>.RefCounted @ref = this.m_Ref;
				this.m_Ref = this.m_Ref.Copy();
				@ref.Release();
				ptr = ref this.m_Ref.value;
			}
			return ref ptr;
		}

		// Token: 0x06001497 RID: 5271 RVA: 0x00057DA4 File Offset: 0x00055FA4
		public static StyleDataRef<T> Create()
		{
			return new StyleDataRef<T>
			{
				m_Ref = new StyleDataRef<T>.RefCounted()
			};
		}

		// Token: 0x06001498 RID: 5272 RVA: 0x00057DCC File Offset: 0x00055FCC
		public override int GetHashCode()
		{
			return (this.m_Ref != null) ? this.m_Ref.value.GetHashCode() : 0;
		}

		// Token: 0x06001499 RID: 5273 RVA: 0x00057E00 File Offset: 0x00056000
		public static bool operator ==(StyleDataRef<T> lhs, StyleDataRef<T> rhs)
		{
			return lhs.m_Ref == rhs.m_Ref || lhs.m_Ref.value.Equals(rhs.m_Ref.value);
		}

		// Token: 0x0600149A RID: 5274 RVA: 0x00057E44 File Offset: 0x00056044
		public static bool operator !=(StyleDataRef<T> lhs, StyleDataRef<T> rhs)
		{
			return !(lhs == rhs);
		}

		// Token: 0x0600149B RID: 5275 RVA: 0x00057E60 File Offset: 0x00056060
		public bool Equals(StyleDataRef<T> other)
		{
			return other == this;
		}

		// Token: 0x0600149C RID: 5276 RVA: 0x00057E80 File Offset: 0x00056080
		public override bool Equals(object obj)
		{
			bool flag;
			if (obj is StyleDataRef<T>)
			{
				StyleDataRef<T> styleDataRef = (StyleDataRef<T>)obj;
				flag = this.Equals(styleDataRef);
			}
			else
			{
				flag = false;
			}
			return flag;
		}

		// Token: 0x040008FD RID: 2301
		private StyleDataRef<T>.RefCounted m_Ref;

		// Token: 0x0200027E RID: 638
		private class RefCounted
		{
			// Token: 0x170004D1 RID: 1233
			// (get) Token: 0x0600149D RID: 5277 RVA: 0x00057EAB File Offset: 0x000560AB
			public int refCount
			{
				get
				{
					return this.m_RefCount;
				}
			}

			// Token: 0x170004D2 RID: 1234
			// (get) Token: 0x0600149E RID: 5278 RVA: 0x00057EB3 File Offset: 0x000560B3
			public uint id
			{
				get
				{
					return this.m_Id;
				}
			}

			// Token: 0x0600149F RID: 5279 RVA: 0x00057EBB File Offset: 0x000560BB
			public RefCounted()
			{
				this.m_RefCount = 1;
				this.m_Id = (StyleDataRef<T>.RefCounted.m_NextId += 1U);
			}

			// Token: 0x060014A0 RID: 5280 RVA: 0x00057EDF File Offset: 0x000560DF
			public void Acquire()
			{
				this.m_RefCount++;
			}

			// Token: 0x060014A1 RID: 5281 RVA: 0x00057EEF File Offset: 0x000560EF
			public void Release()
			{
				this.m_RefCount--;
			}

			// Token: 0x060014A2 RID: 5282 RVA: 0x00057F00 File Offset: 0x00056100
			public StyleDataRef<T>.RefCounted Copy()
			{
				return new StyleDataRef<T>.RefCounted
				{
					value = this.value.Copy()
				};
			}

			// Token: 0x040008FE RID: 2302
			private static uint m_NextId = 1U;

			// Token: 0x040008FF RID: 2303
			private int m_RefCount;

			// Token: 0x04000900 RID: 2304
			private readonly uint m_Id;

			// Token: 0x04000901 RID: 2305
			public T value;
		}
	}
}
