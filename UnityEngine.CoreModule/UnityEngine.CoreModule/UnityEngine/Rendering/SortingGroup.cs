using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;

namespace UnityEngine.Rendering
{
	// Token: 0x02000427 RID: 1063
	[NativeType(Header = "Runtime/2D/Sorting/SortingGroup.h")]
	[RequireComponent(typeof(Transform))]
	public sealed class SortingGroup : Behaviour
	{
		// Token: 0x170006F7 RID: 1783
		// (get) Token: 0x0600250F RID: 9487
		[StaticAccessor("SortingGroup", StaticAccessorType.DoubleColon)]
		internal static extern int invalidSortingGroupID
		{
			[MethodImpl(4096)]
			get;
		}

		// Token: 0x06002510 RID: 9488
		[StaticAccessor("SortingGroup", StaticAccessorType.DoubleColon)]
		[MethodImpl(4096)]
		public static extern void UpdateAllSortingGroups();

		// Token: 0x06002511 RID: 9489
		[StaticAccessor("SortingGroup", StaticAccessorType.DoubleColon)]
		[MethodImpl(4096)]
		internal static extern SortingGroup GetSortingGroupByIndex(int index);

		// Token: 0x170006F8 RID: 1784
		// (get) Token: 0x06002512 RID: 9490
		// (set) Token: 0x06002513 RID: 9491
		public extern string sortingLayerName
		{
			[MethodImpl(4096)]
			get;
			[MethodImpl(4096)]
			set;
		}

		// Token: 0x170006F9 RID: 1785
		// (get) Token: 0x06002514 RID: 9492
		// (set) Token: 0x06002515 RID: 9493
		public extern int sortingLayerID
		{
			[MethodImpl(4096)]
			get;
			[MethodImpl(4096)]
			set;
		}

		// Token: 0x170006FA RID: 1786
		// (get) Token: 0x06002516 RID: 9494
		// (set) Token: 0x06002517 RID: 9495
		public extern int sortingOrder
		{
			[MethodImpl(4096)]
			get;
			[MethodImpl(4096)]
			set;
		}

		// Token: 0x170006FB RID: 1787
		// (get) Token: 0x06002518 RID: 9496
		internal extern int sortingGroupID
		{
			[MethodImpl(4096)]
			get;
		}

		// Token: 0x170006FC RID: 1788
		// (get) Token: 0x06002519 RID: 9497
		internal extern int sortingGroupOrder
		{
			[MethodImpl(4096)]
			get;
		}

		// Token: 0x170006FD RID: 1789
		// (get) Token: 0x0600251A RID: 9498
		internal extern int index
		{
			[MethodImpl(4096)]
			get;
		}
	}
}
