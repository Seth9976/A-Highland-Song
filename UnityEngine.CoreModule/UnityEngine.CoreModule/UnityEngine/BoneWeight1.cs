using System;
using UnityEngine.Scripting;

namespace UnityEngine
{
	// Token: 0x0200019F RID: 415
	[UsedByNativeCode]
	[Serializable]
	public struct BoneWeight1 : IEquatable<BoneWeight1>
	{
		// Token: 0x17000359 RID: 857
		// (get) Token: 0x060010A4 RID: 4260 RVA: 0x00015F34 File Offset: 0x00014134
		// (set) Token: 0x060010A5 RID: 4261 RVA: 0x00015F4C File Offset: 0x0001414C
		public float weight
		{
			get
			{
				return this.m_Weight;
			}
			set
			{
				this.m_Weight = value;
			}
		}

		// Token: 0x1700035A RID: 858
		// (get) Token: 0x060010A6 RID: 4262 RVA: 0x00015F58 File Offset: 0x00014158
		// (set) Token: 0x060010A7 RID: 4263 RVA: 0x00015F70 File Offset: 0x00014170
		public int boneIndex
		{
			get
			{
				return this.m_BoneIndex;
			}
			set
			{
				this.m_BoneIndex = value;
			}
		}

		// Token: 0x060010A8 RID: 4264 RVA: 0x00015F7C File Offset: 0x0001417C
		public override bool Equals(object other)
		{
			return other is BoneWeight1 && this.Equals((BoneWeight1)other);
		}

		// Token: 0x060010A9 RID: 4265 RVA: 0x00015FA8 File Offset: 0x000141A8
		public bool Equals(BoneWeight1 other)
		{
			return this.boneIndex.Equals(other.boneIndex) && this.weight.Equals(other.weight);
		}

		// Token: 0x060010AA RID: 4266 RVA: 0x00015FEC File Offset: 0x000141EC
		public override int GetHashCode()
		{
			return this.boneIndex.GetHashCode() ^ this.weight.GetHashCode();
		}

		// Token: 0x060010AB RID: 4267 RVA: 0x0001601C File Offset: 0x0001421C
		public static bool operator ==(BoneWeight1 lhs, BoneWeight1 rhs)
		{
			return lhs.boneIndex == rhs.boneIndex && lhs.weight == rhs.weight;
		}

		// Token: 0x060010AC RID: 4268 RVA: 0x00016054 File Offset: 0x00014254
		public static bool operator !=(BoneWeight1 lhs, BoneWeight1 rhs)
		{
			return !(lhs == rhs);
		}

		// Token: 0x040005B2 RID: 1458
		[SerializeField]
		private float m_Weight;

		// Token: 0x040005B3 RID: 1459
		[SerializeField]
		private int m_BoneIndex;
	}
}
