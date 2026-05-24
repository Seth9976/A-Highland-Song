using System;

namespace UnityEngine
{
	// Token: 0x02000256 RID: 598
	[Serializable]
	public struct Pose : IEquatable<Pose>
	{
		// Token: 0x060019E4 RID: 6628 RVA: 0x00029D18 File Offset: 0x00027F18
		public Pose(Vector3 position, Quaternion rotation)
		{
			this.position = position;
			this.rotation = rotation;
		}

		// Token: 0x060019E5 RID: 6629 RVA: 0x00029D2C File Offset: 0x00027F2C
		public override string ToString()
		{
			return UnityString.Format("({0}, {1})", new object[]
			{
				this.position.ToString(),
				this.rotation.ToString()
			});
		}

		// Token: 0x060019E6 RID: 6630 RVA: 0x00029D78 File Offset: 0x00027F78
		public string ToString(string format)
		{
			return UnityString.Format("({0}, {1})", new object[]
			{
				this.position.ToString(format),
				this.rotation.ToString(format)
			});
		}

		// Token: 0x060019E7 RID: 6631 RVA: 0x00029DB8 File Offset: 0x00027FB8
		public Pose GetTransformedBy(Pose lhs)
		{
			return new Pose
			{
				position = lhs.position + lhs.rotation * this.position,
				rotation = lhs.rotation * this.rotation
			};
		}

		// Token: 0x060019E8 RID: 6632 RVA: 0x00029E10 File Offset: 0x00028010
		public Pose GetTransformedBy(Transform lhs)
		{
			return new Pose
			{
				position = lhs.TransformPoint(this.position),
				rotation = lhs.rotation * this.rotation
			};
		}

		// Token: 0x17000530 RID: 1328
		// (get) Token: 0x060019E9 RID: 6633 RVA: 0x00029E58 File Offset: 0x00028058
		public Vector3 forward
		{
			get
			{
				return this.rotation * Vector3.forward;
			}
		}

		// Token: 0x17000531 RID: 1329
		// (get) Token: 0x060019EA RID: 6634 RVA: 0x00029E7C File Offset: 0x0002807C
		public Vector3 right
		{
			get
			{
				return this.rotation * Vector3.right;
			}
		}

		// Token: 0x17000532 RID: 1330
		// (get) Token: 0x060019EB RID: 6635 RVA: 0x00029EA0 File Offset: 0x000280A0
		public Vector3 up
		{
			get
			{
				return this.rotation * Vector3.up;
			}
		}

		// Token: 0x17000533 RID: 1331
		// (get) Token: 0x060019EC RID: 6636 RVA: 0x00029EC4 File Offset: 0x000280C4
		public static Pose identity
		{
			get
			{
				return Pose.k_Identity;
			}
		}

		// Token: 0x060019ED RID: 6637 RVA: 0x00029EDC File Offset: 0x000280DC
		public override bool Equals(object obj)
		{
			bool flag = !(obj is Pose);
			return !flag && this.Equals((Pose)obj);
		}

		// Token: 0x060019EE RID: 6638 RVA: 0x00029F10 File Offset: 0x00028110
		public bool Equals(Pose other)
		{
			return this.position == other.position && this.rotation == other.rotation;
		}

		// Token: 0x060019EF RID: 6639 RVA: 0x00029F4C File Offset: 0x0002814C
		public override int GetHashCode()
		{
			return this.position.GetHashCode() ^ (this.rotation.GetHashCode() << 1);
		}

		// Token: 0x060019F0 RID: 6640 RVA: 0x00029F84 File Offset: 0x00028184
		public static bool operator ==(Pose a, Pose b)
		{
			return a.Equals(b);
		}

		// Token: 0x060019F1 RID: 6641 RVA: 0x00029FA0 File Offset: 0x000281A0
		public static bool operator !=(Pose a, Pose b)
		{
			return !(a == b);
		}

		// Token: 0x0400088F RID: 2191
		public Vector3 position;

		// Token: 0x04000890 RID: 2192
		public Quaternion rotation;

		// Token: 0x04000891 RID: 2193
		private static readonly Pose k_Identity = new Pose(Vector3.zero, Quaternion.identity);
	}
}
