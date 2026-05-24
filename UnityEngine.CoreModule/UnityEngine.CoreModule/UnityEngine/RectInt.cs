using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine.Scripting;

namespace UnityEngine
{
	// Token: 0x02000117 RID: 279
	[UsedByNativeCode]
	public struct RectInt : IEquatable<RectInt>, IFormattable
	{
		// Token: 0x17000199 RID: 409
		// (get) Token: 0x0600073A RID: 1850 RVA: 0x0000AE5C File Offset: 0x0000905C
		// (set) Token: 0x0600073B RID: 1851 RVA: 0x0000AE74 File Offset: 0x00009074
		public int x
		{
			get
			{
				return this.m_XMin;
			}
			set
			{
				this.m_XMin = value;
			}
		}

		// Token: 0x1700019A RID: 410
		// (get) Token: 0x0600073C RID: 1852 RVA: 0x0000AE80 File Offset: 0x00009080
		// (set) Token: 0x0600073D RID: 1853 RVA: 0x0000AE98 File Offset: 0x00009098
		public int y
		{
			get
			{
				return this.m_YMin;
			}
			set
			{
				this.m_YMin = value;
			}
		}

		// Token: 0x1700019B RID: 411
		// (get) Token: 0x0600073E RID: 1854 RVA: 0x0000AEA4 File Offset: 0x000090A4
		public Vector2 center
		{
			get
			{
				return new Vector2((float)this.x + (float)this.m_Width / 2f, (float)this.y + (float)this.m_Height / 2f);
			}
		}

		// Token: 0x1700019C RID: 412
		// (get) Token: 0x0600073F RID: 1855 RVA: 0x0000AEE8 File Offset: 0x000090E8
		// (set) Token: 0x06000740 RID: 1856 RVA: 0x0000AF0B File Offset: 0x0000910B
		public Vector2Int min
		{
			get
			{
				return new Vector2Int(this.xMin, this.yMin);
			}
			set
			{
				this.xMin = value.x;
				this.yMin = value.y;
			}
		}

		// Token: 0x1700019D RID: 413
		// (get) Token: 0x06000741 RID: 1857 RVA: 0x0000AF2C File Offset: 0x0000912C
		// (set) Token: 0x06000742 RID: 1858 RVA: 0x0000AF4F File Offset: 0x0000914F
		public Vector2Int max
		{
			get
			{
				return new Vector2Int(this.xMax, this.yMax);
			}
			set
			{
				this.xMax = value.x;
				this.yMax = value.y;
			}
		}

		// Token: 0x1700019E RID: 414
		// (get) Token: 0x06000743 RID: 1859 RVA: 0x0000AF70 File Offset: 0x00009170
		// (set) Token: 0x06000744 RID: 1860 RVA: 0x0000AF88 File Offset: 0x00009188
		public int width
		{
			get
			{
				return this.m_Width;
			}
			set
			{
				this.m_Width = value;
			}
		}

		// Token: 0x1700019F RID: 415
		// (get) Token: 0x06000745 RID: 1861 RVA: 0x0000AF94 File Offset: 0x00009194
		// (set) Token: 0x06000746 RID: 1862 RVA: 0x0000AFAC File Offset: 0x000091AC
		public int height
		{
			get
			{
				return this.m_Height;
			}
			set
			{
				this.m_Height = value;
			}
		}

		// Token: 0x170001A0 RID: 416
		// (get) Token: 0x06000747 RID: 1863 RVA: 0x0000AFB8 File Offset: 0x000091B8
		// (set) Token: 0x06000748 RID: 1864 RVA: 0x0000AFE4 File Offset: 0x000091E4
		public int xMin
		{
			get
			{
				return Math.Min(this.m_XMin, this.m_XMin + this.m_Width);
			}
			set
			{
				int xMax = this.xMax;
				this.m_XMin = value;
				this.m_Width = xMax - this.m_XMin;
			}
		}

		// Token: 0x170001A1 RID: 417
		// (get) Token: 0x06000749 RID: 1865 RVA: 0x0000B010 File Offset: 0x00009210
		// (set) Token: 0x0600074A RID: 1866 RVA: 0x0000B03C File Offset: 0x0000923C
		public int yMin
		{
			get
			{
				return Math.Min(this.m_YMin, this.m_YMin + this.m_Height);
			}
			set
			{
				int yMax = this.yMax;
				this.m_YMin = value;
				this.m_Height = yMax - this.m_YMin;
			}
		}

		// Token: 0x170001A2 RID: 418
		// (get) Token: 0x0600074B RID: 1867 RVA: 0x0000B068 File Offset: 0x00009268
		// (set) Token: 0x0600074C RID: 1868 RVA: 0x0000B092 File Offset: 0x00009292
		public int xMax
		{
			get
			{
				return Math.Max(this.m_XMin, this.m_XMin + this.m_Width);
			}
			set
			{
				this.m_Width = value - this.m_XMin;
			}
		}

		// Token: 0x170001A3 RID: 419
		// (get) Token: 0x0600074D RID: 1869 RVA: 0x0000B0A4 File Offset: 0x000092A4
		// (set) Token: 0x0600074E RID: 1870 RVA: 0x0000B0CE File Offset: 0x000092CE
		public int yMax
		{
			get
			{
				return Math.Max(this.m_YMin, this.m_YMin + this.m_Height);
			}
			set
			{
				this.m_Height = value - this.m_YMin;
			}
		}

		// Token: 0x170001A4 RID: 420
		// (get) Token: 0x0600074F RID: 1871 RVA: 0x0000B0E0 File Offset: 0x000092E0
		// (set) Token: 0x06000750 RID: 1872 RVA: 0x0000B103 File Offset: 0x00009303
		public Vector2Int position
		{
			get
			{
				return new Vector2Int(this.m_XMin, this.m_YMin);
			}
			set
			{
				this.m_XMin = value.x;
				this.m_YMin = value.y;
			}
		}

		// Token: 0x170001A5 RID: 421
		// (get) Token: 0x06000751 RID: 1873 RVA: 0x0000B120 File Offset: 0x00009320
		// (set) Token: 0x06000752 RID: 1874 RVA: 0x0000B143 File Offset: 0x00009343
		public Vector2Int size
		{
			get
			{
				return new Vector2Int(this.m_Width, this.m_Height);
			}
			set
			{
				this.m_Width = value.x;
				this.m_Height = value.y;
			}
		}

		// Token: 0x06000753 RID: 1875 RVA: 0x0000B160 File Offset: 0x00009360
		public void SetMinMax(Vector2Int minPosition, Vector2Int maxPosition)
		{
			this.min = minPosition;
			this.max = maxPosition;
		}

		// Token: 0x06000754 RID: 1876 RVA: 0x0000B173 File Offset: 0x00009373
		public RectInt(int xMin, int yMin, int width, int height)
		{
			this.m_XMin = xMin;
			this.m_YMin = yMin;
			this.m_Width = width;
			this.m_Height = height;
		}

		// Token: 0x06000755 RID: 1877 RVA: 0x0000B193 File Offset: 0x00009393
		public RectInt(Vector2Int position, Vector2Int size)
		{
			this.m_XMin = position.x;
			this.m_YMin = position.y;
			this.m_Width = size.x;
			this.m_Height = size.y;
		}

		// Token: 0x06000756 RID: 1878 RVA: 0x0000B1CC File Offset: 0x000093CC
		public void ClampToBounds(RectInt bounds)
		{
			this.position = new Vector2Int(Math.Max(Math.Min(bounds.xMax, this.position.x), bounds.xMin), Math.Max(Math.Min(bounds.yMax, this.position.y), bounds.yMin));
			this.size = new Vector2Int(Math.Min(bounds.xMax - this.position.x, this.size.x), Math.Min(bounds.yMax - this.position.y, this.size.y));
		}

		// Token: 0x06000757 RID: 1879 RVA: 0x0000B290 File Offset: 0x00009490
		public bool Contains(Vector2Int position)
		{
			return position.x >= this.xMin && position.y >= this.yMin && position.x < this.xMax && position.y < this.yMax;
		}

		// Token: 0x06000758 RID: 1880 RVA: 0x0000B2E4 File Offset: 0x000094E4
		public bool Overlaps(RectInt other)
		{
			return other.xMin < this.xMax && other.xMax > this.xMin && other.yMin < this.yMax && other.yMax > this.yMin;
		}

		// Token: 0x06000759 RID: 1881 RVA: 0x0000B338 File Offset: 0x00009538
		public override string ToString()
		{
			return this.ToString(null, null);
		}

		// Token: 0x0600075A RID: 1882 RVA: 0x0000B354 File Offset: 0x00009554
		public string ToString(string format)
		{
			return this.ToString(format, null);
		}

		// Token: 0x0600075B RID: 1883 RVA: 0x0000B370 File Offset: 0x00009570
		public string ToString(string format, IFormatProvider formatProvider)
		{
			bool flag = formatProvider == null;
			if (flag)
			{
				formatProvider = CultureInfo.InvariantCulture.NumberFormat;
			}
			return UnityString.Format("(x:{0}, y:{1}, width:{2}, height:{3})", new object[]
			{
				this.x.ToString(format, formatProvider),
				this.y.ToString(format, formatProvider),
				this.width.ToString(format, formatProvider),
				this.height.ToString(format, formatProvider)
			});
		}

		// Token: 0x0600075C RID: 1884 RVA: 0x0000B3F4 File Offset: 0x000095F4
		public bool Equals(RectInt other)
		{
			return this.m_XMin == other.m_XMin && this.m_YMin == other.m_YMin && this.m_Width == other.m_Width && this.m_Height == other.m_Height;
		}

		// Token: 0x170001A6 RID: 422
		// (get) Token: 0x0600075D RID: 1885 RVA: 0x0000B444 File Offset: 0x00009644
		public RectInt.PositionEnumerator allPositionsWithin
		{
			get
			{
				return new RectInt.PositionEnumerator(this.min, this.max);
			}
		}

		// Token: 0x04000398 RID: 920
		private int m_XMin;

		// Token: 0x04000399 RID: 921
		private int m_YMin;

		// Token: 0x0400039A RID: 922
		private int m_Width;

		// Token: 0x0400039B RID: 923
		private int m_Height;

		// Token: 0x02000118 RID: 280
		public struct PositionEnumerator : IEnumerator<Vector2Int>, IEnumerator, IDisposable
		{
			// Token: 0x0600075E RID: 1886 RVA: 0x0000B468 File Offset: 0x00009668
			public PositionEnumerator(Vector2Int min, Vector2Int max)
			{
				this._current = min;
				this._min = min;
				this._max = max;
				this.Reset();
			}

			// Token: 0x0600075F RID: 1887 RVA: 0x0000B494 File Offset: 0x00009694
			public RectInt.PositionEnumerator GetEnumerator()
			{
				return this;
			}

			// Token: 0x06000760 RID: 1888 RVA: 0x0000B4AC File Offset: 0x000096AC
			public bool MoveNext()
			{
				bool flag = this._current.y >= this._max.y;
				bool flag2;
				if (flag)
				{
					flag2 = false;
				}
				else
				{
					int num = this._current.x;
					this._current.x = num + 1;
					bool flag3 = this._current.x >= this._max.x;
					if (flag3)
					{
						this._current.x = this._min.x;
						bool flag4 = this._current.x >= this._max.x;
						if (flag4)
						{
							return false;
						}
						num = this._current.y;
						this._current.y = num + 1;
						bool flag5 = this._current.y >= this._max.y;
						if (flag5)
						{
							return false;
						}
					}
					flag2 = true;
				}
				return flag2;
			}

			// Token: 0x06000761 RID: 1889 RVA: 0x0000B5A8 File Offset: 0x000097A8
			public void Reset()
			{
				this._current = this._min;
				int x = this._current.x;
				this._current.x = x - 1;
			}

			// Token: 0x170001A7 RID: 423
			// (get) Token: 0x06000762 RID: 1890 RVA: 0x0000B5D8 File Offset: 0x000097D8
			public Vector2Int Current
			{
				get
				{
					return this._current;
				}
			}

			// Token: 0x170001A8 RID: 424
			// (get) Token: 0x06000763 RID: 1891 RVA: 0x0000B5F0 File Offset: 0x000097F0
			object IEnumerator.Current
			{
				get
				{
					return this.Current;
				}
			}

			// Token: 0x06000764 RID: 1892 RVA: 0x00004557 File Offset: 0x00002757
			void IDisposable.Dispose()
			{
			}

			// Token: 0x0400039C RID: 924
			private readonly Vector2Int _min;

			// Token: 0x0400039D RID: 925
			private readonly Vector2Int _max;

			// Token: 0x0400039E RID: 926
			private Vector2Int _current;
		}
	}
}
