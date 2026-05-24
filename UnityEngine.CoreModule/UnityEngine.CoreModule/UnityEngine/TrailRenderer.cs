using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine.Bindings;

namespace UnityEngine
{
	// Token: 0x02000144 RID: 324
	[NativeHeader("Runtime/Graphics/GraphicsScriptBindings.h")]
	[NativeHeader("Runtime/Graphics/TrailRenderer.h")]
	public sealed class TrailRenderer : Renderer
	{
		// Token: 0x17000249 RID: 585
		// (get) Token: 0x06000A73 RID: 2675 RVA: 0x0000F32C File Offset: 0x0000D52C
		[Obsolete("Use positionCount instead (UnityUpgradable) -> positionCount", false)]
		public int numPositions
		{
			get
			{
				return this.positionCount;
			}
		}

		// Token: 0x1700024A RID: 586
		// (get) Token: 0x06000A74 RID: 2676
		// (set) Token: 0x06000A75 RID: 2677
		public extern float time
		{
			[MethodImpl(4096)]
			get;
			[MethodImpl(4096)]
			set;
		}

		// Token: 0x1700024B RID: 587
		// (get) Token: 0x06000A76 RID: 2678
		// (set) Token: 0x06000A77 RID: 2679
		public extern float startWidth
		{
			[MethodImpl(4096)]
			get;
			[MethodImpl(4096)]
			set;
		}

		// Token: 0x1700024C RID: 588
		// (get) Token: 0x06000A78 RID: 2680
		// (set) Token: 0x06000A79 RID: 2681
		public extern float endWidth
		{
			[MethodImpl(4096)]
			get;
			[MethodImpl(4096)]
			set;
		}

		// Token: 0x1700024D RID: 589
		// (get) Token: 0x06000A7A RID: 2682
		// (set) Token: 0x06000A7B RID: 2683
		public extern float widthMultiplier
		{
			[MethodImpl(4096)]
			get;
			[MethodImpl(4096)]
			set;
		}

		// Token: 0x1700024E RID: 590
		// (get) Token: 0x06000A7C RID: 2684
		// (set) Token: 0x06000A7D RID: 2685
		public extern bool autodestruct
		{
			[MethodImpl(4096)]
			get;
			[MethodImpl(4096)]
			set;
		}

		// Token: 0x1700024F RID: 591
		// (get) Token: 0x06000A7E RID: 2686
		// (set) Token: 0x06000A7F RID: 2687
		public extern bool emitting
		{
			[MethodImpl(4096)]
			get;
			[MethodImpl(4096)]
			set;
		}

		// Token: 0x17000250 RID: 592
		// (get) Token: 0x06000A80 RID: 2688
		// (set) Token: 0x06000A81 RID: 2689
		public extern int numCornerVertices
		{
			[MethodImpl(4096)]
			get;
			[MethodImpl(4096)]
			set;
		}

		// Token: 0x17000251 RID: 593
		// (get) Token: 0x06000A82 RID: 2690
		// (set) Token: 0x06000A83 RID: 2691
		public extern int numCapVertices
		{
			[MethodImpl(4096)]
			get;
			[MethodImpl(4096)]
			set;
		}

		// Token: 0x17000252 RID: 594
		// (get) Token: 0x06000A84 RID: 2692
		// (set) Token: 0x06000A85 RID: 2693
		public extern float minVertexDistance
		{
			[MethodImpl(4096)]
			get;
			[MethodImpl(4096)]
			set;
		}

		// Token: 0x17000253 RID: 595
		// (get) Token: 0x06000A86 RID: 2694 RVA: 0x0000F344 File Offset: 0x0000D544
		// (set) Token: 0x06000A87 RID: 2695 RVA: 0x0000F35A File Offset: 0x0000D55A
		public Color startColor
		{
			get
			{
				Color color;
				this.get_startColor_Injected(out color);
				return color;
			}
			set
			{
				this.set_startColor_Injected(ref value);
			}
		}

		// Token: 0x17000254 RID: 596
		// (get) Token: 0x06000A88 RID: 2696 RVA: 0x0000F364 File Offset: 0x0000D564
		// (set) Token: 0x06000A89 RID: 2697 RVA: 0x0000F37A File Offset: 0x0000D57A
		public Color endColor
		{
			get
			{
				Color color;
				this.get_endColor_Injected(out color);
				return color;
			}
			set
			{
				this.set_endColor_Injected(ref value);
			}
		}

		// Token: 0x17000255 RID: 597
		// (get) Token: 0x06000A8A RID: 2698
		[NativeProperty("PositionsCount")]
		public extern int positionCount
		{
			[MethodImpl(4096)]
			get;
		}

		// Token: 0x06000A8B RID: 2699 RVA: 0x0000F384 File Offset: 0x0000D584
		public void SetPosition(int index, Vector3 position)
		{
			this.SetPosition_Injected(index, ref position);
		}

		// Token: 0x06000A8C RID: 2700 RVA: 0x0000F390 File Offset: 0x0000D590
		public Vector3 GetPosition(int index)
		{
			Vector3 vector;
			this.GetPosition_Injected(index, out vector);
			return vector;
		}

		// Token: 0x17000256 RID: 598
		// (get) Token: 0x06000A8D RID: 2701
		// (set) Token: 0x06000A8E RID: 2702
		public extern float shadowBias
		{
			[MethodImpl(4096)]
			get;
			[MethodImpl(4096)]
			set;
		}

		// Token: 0x17000257 RID: 599
		// (get) Token: 0x06000A8F RID: 2703
		// (set) Token: 0x06000A90 RID: 2704
		public extern bool generateLightingData
		{
			[MethodImpl(4096)]
			get;
			[MethodImpl(4096)]
			set;
		}

		// Token: 0x17000258 RID: 600
		// (get) Token: 0x06000A91 RID: 2705
		// (set) Token: 0x06000A92 RID: 2706
		public extern LineTextureMode textureMode
		{
			[MethodImpl(4096)]
			get;
			[MethodImpl(4096)]
			set;
		}

		// Token: 0x17000259 RID: 601
		// (get) Token: 0x06000A93 RID: 2707
		// (set) Token: 0x06000A94 RID: 2708
		public extern LineAlignment alignment
		{
			[MethodImpl(4096)]
			get;
			[MethodImpl(4096)]
			set;
		}

		// Token: 0x06000A95 RID: 2709
		[MethodImpl(4096)]
		public extern void Clear();

		// Token: 0x06000A96 RID: 2710 RVA: 0x0000F3A7 File Offset: 0x0000D5A7
		public void BakeMesh(Mesh mesh, bool useTransform = false)
		{
			this.BakeMesh(mesh, Camera.main, useTransform);
		}

		// Token: 0x06000A97 RID: 2711
		[MethodImpl(4096)]
		public extern void BakeMesh([NotNull("ArgumentNullException")] Mesh mesh, [NotNull("ArgumentNullException")] Camera camera, bool useTransform = false);

		// Token: 0x1700025A RID: 602
		// (get) Token: 0x06000A98 RID: 2712 RVA: 0x0000F3B8 File Offset: 0x0000D5B8
		// (set) Token: 0x06000A99 RID: 2713 RVA: 0x0000F3D0 File Offset: 0x0000D5D0
		public AnimationCurve widthCurve
		{
			get
			{
				return this.GetWidthCurveCopy();
			}
			set
			{
				this.SetWidthCurve(value);
			}
		}

		// Token: 0x1700025B RID: 603
		// (get) Token: 0x06000A9A RID: 2714 RVA: 0x0000F3DC File Offset: 0x0000D5DC
		// (set) Token: 0x06000A9B RID: 2715 RVA: 0x0000F3F4 File Offset: 0x0000D5F4
		public Gradient colorGradient
		{
			get
			{
				return this.GetColorGradientCopy();
			}
			set
			{
				this.SetColorGradient(value);
			}
		}

		// Token: 0x06000A9C RID: 2716
		[MethodImpl(4096)]
		private extern AnimationCurve GetWidthCurveCopy();

		// Token: 0x06000A9D RID: 2717
		[MethodImpl(4096)]
		private extern void SetWidthCurve([NotNull("ArgumentNullException")] AnimationCurve curve);

		// Token: 0x06000A9E RID: 2718
		[MethodImpl(4096)]
		private extern Gradient GetColorGradientCopy();

		// Token: 0x06000A9F RID: 2719
		[MethodImpl(4096)]
		private extern void SetColorGradient([NotNull("ArgumentNullException")] Gradient curve);

		// Token: 0x06000AA0 RID: 2720
		[FreeFunction(Name = "TrailRendererScripting::GetPositions", HasExplicitThis = true)]
		[MethodImpl(4096)]
		public extern int GetPositions([NotNull("ArgumentNullException")] [Out] Vector3[] positions);

		// Token: 0x06000AA1 RID: 2721
		[FreeFunction(Name = "TrailRendererScripting::GetVisiblePositions", HasExplicitThis = true)]
		[MethodImpl(4096)]
		public extern int GetVisiblePositions([NotNull("ArgumentNullException")] [Out] Vector3[] positions);

		// Token: 0x06000AA2 RID: 2722
		[FreeFunction(Name = "TrailRendererScripting::SetPositions", HasExplicitThis = true)]
		[MethodImpl(4096)]
		public extern void SetPositions([NotNull("ArgumentNullException")] Vector3[] positions);

		// Token: 0x06000AA3 RID: 2723 RVA: 0x0000F3FF File Offset: 0x0000D5FF
		[FreeFunction(Name = "TrailRendererScripting::AddPosition", HasExplicitThis = true)]
		public void AddPosition(Vector3 position)
		{
			this.AddPosition_Injected(ref position);
		}

		// Token: 0x06000AA4 RID: 2724
		[FreeFunction(Name = "TrailRendererScripting::AddPositions", HasExplicitThis = true)]
		[MethodImpl(4096)]
		public extern void AddPositions([NotNull("ArgumentNullException")] Vector3[] positions);

		// Token: 0x06000AA5 RID: 2725 RVA: 0x0000F409 File Offset: 0x0000D609
		public void SetPositions(NativeArray<Vector3> positions)
		{
			this.SetPositionsWithNativeContainer((IntPtr)positions.GetUnsafeReadOnlyPtr<Vector3>(), positions.Length);
		}

		// Token: 0x06000AA6 RID: 2726 RVA: 0x0000F427 File Offset: 0x0000D627
		public void SetPositions(NativeSlice<Vector3> positions)
		{
			this.SetPositionsWithNativeContainer((IntPtr)positions.GetUnsafeReadOnlyPtr<Vector3>(), positions.Length);
		}

		// Token: 0x06000AA7 RID: 2727 RVA: 0x0000F448 File Offset: 0x0000D648
		public int GetPositions([Out] NativeArray<Vector3> positions)
		{
			return this.GetPositionsWithNativeContainer((IntPtr)positions.GetUnsafePtr<Vector3>(), positions.Length);
		}

		// Token: 0x06000AA8 RID: 2728 RVA: 0x0000F474 File Offset: 0x0000D674
		public int GetPositions([Out] NativeSlice<Vector3> positions)
		{
			return this.GetPositionsWithNativeContainer((IntPtr)positions.GetUnsafePtr<Vector3>(), positions.Length);
		}

		// Token: 0x06000AA9 RID: 2729 RVA: 0x0000F4A0 File Offset: 0x0000D6A0
		public int GetVisiblePositions([Out] NativeArray<Vector3> positions)
		{
			return this.GetVisiblePositionsWithNativeContainer((IntPtr)positions.GetUnsafePtr<Vector3>(), positions.Length);
		}

		// Token: 0x06000AAA RID: 2730 RVA: 0x0000F4CC File Offset: 0x0000D6CC
		public int GetVisiblePositions([Out] NativeSlice<Vector3> positions)
		{
			return this.GetVisiblePositionsWithNativeContainer((IntPtr)positions.GetUnsafePtr<Vector3>(), positions.Length);
		}

		// Token: 0x06000AAB RID: 2731 RVA: 0x0000F4F7 File Offset: 0x0000D6F7
		public void AddPositions([Out] NativeArray<Vector3> positions)
		{
			this.AddPositionsWithNativeContainer((IntPtr)positions.GetUnsafePtr<Vector3>(), positions.Length);
		}

		// Token: 0x06000AAC RID: 2732 RVA: 0x0000F515 File Offset: 0x0000D715
		public void AddPositions([Out] NativeSlice<Vector3> positions)
		{
			this.AddPositionsWithNativeContainer((IntPtr)positions.GetUnsafePtr<Vector3>(), positions.Length);
		}

		// Token: 0x06000AAD RID: 2733
		[FreeFunction(Name = "TrailRendererScripting::SetPositionsWithNativeContainer", HasExplicitThis = true)]
		[MethodImpl(4096)]
		private extern void SetPositionsWithNativeContainer(IntPtr positions, int count);

		// Token: 0x06000AAE RID: 2734
		[FreeFunction(Name = "TrailRendererScripting::GetPositionsWithNativeContainer", HasExplicitThis = true)]
		[MethodImpl(4096)]
		private extern int GetPositionsWithNativeContainer(IntPtr positions, int length);

		// Token: 0x06000AAF RID: 2735
		[FreeFunction(Name = "TrailRendererScripting::GetVisiblePositionsWithNativeContainer", HasExplicitThis = true)]
		[MethodImpl(4096)]
		private extern int GetVisiblePositionsWithNativeContainer(IntPtr positions, int length);

		// Token: 0x06000AB0 RID: 2736
		[FreeFunction(Name = "TrailRendererScripting::AddPositionsWithNativeContainer", HasExplicitThis = true)]
		[MethodImpl(4096)]
		private extern void AddPositionsWithNativeContainer(IntPtr positions, int length);

		// Token: 0x06000AB2 RID: 2738
		[MethodImpl(4096)]
		private extern void get_startColor_Injected(out Color ret);

		// Token: 0x06000AB3 RID: 2739
		[MethodImpl(4096)]
		private extern void set_startColor_Injected(ref Color value);

		// Token: 0x06000AB4 RID: 2740
		[MethodImpl(4096)]
		private extern void get_endColor_Injected(out Color ret);

		// Token: 0x06000AB5 RID: 2741
		[MethodImpl(4096)]
		private extern void set_endColor_Injected(ref Color value);

		// Token: 0x06000AB6 RID: 2742
		[MethodImpl(4096)]
		private extern void SetPosition_Injected(int index, ref Vector3 position);

		// Token: 0x06000AB7 RID: 2743
		[MethodImpl(4096)]
		private extern void GetPosition_Injected(int index, out Vector3 ret);

		// Token: 0x06000AB8 RID: 2744
		[MethodImpl(4096)]
		private extern void AddPosition_Injected(ref Vector3 position);
	}
}
