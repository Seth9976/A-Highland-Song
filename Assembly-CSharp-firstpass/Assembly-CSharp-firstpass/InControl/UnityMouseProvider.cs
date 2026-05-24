using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace InControl
{
	// Token: 0x02000050 RID: 80
	public class UnityMouseProvider : IMouseProvider
	{
		// Token: 0x060003B5 RID: 949 RVA: 0x0000D229 File Offset: 0x0000B429
		public void Setup()
		{
			this.ClearState();
		}

		// Token: 0x060003B6 RID: 950 RVA: 0x0000D231 File Offset: 0x0000B431
		public void Reset()
		{
			this.ClearState();
		}

		// Token: 0x060003B7 RID: 951 RVA: 0x0000D23C File Offset: 0x0000B43C
		public void Update()
		{
			if (Input.mousePresent)
			{
				Array.Copy(this.buttonPressed, this.lastButtonPressed, this.buttonPressed.Length);
				this.buttonPressed[1] = UnityMouseProvider.SafeGetMouseButton(0);
				this.buttonPressed[2] = UnityMouseProvider.SafeGetMouseButton(1);
				this.buttonPressed[3] = UnityMouseProvider.SafeGetMouseButton(2);
				this.buttonPressed[10] = UnityMouseProvider.SafeGetMouseButton(3);
				this.buttonPressed[11] = UnityMouseProvider.SafeGetMouseButton(4);
				this.buttonPressed[12] = UnityMouseProvider.SafeGetMouseButton(5);
				this.buttonPressed[13] = UnityMouseProvider.SafeGetMouseButton(6);
				this.lastPosition = this.position;
				this.position = Input.mousePosition;
				this.delta = new Vector2(Input.GetAxisRaw("mouse x"), Input.GetAxisRaw("mouse y"));
				this.scroll = Input.mouseScrollDelta;
				return;
			}
			this.ClearState();
		}

		// Token: 0x060003B8 RID: 952 RVA: 0x0000D320 File Offset: 0x0000B520
		private static bool SafeGetMouseButton(int button)
		{
			if (button < UnityMouseProvider.maxSafeMouseButton)
			{
				try
				{
					return Input.GetMouseButton(button);
				}
				catch (ArgumentException)
				{
					UnityMouseProvider.maxSafeMouseButton = Mathf.Min(button, UnityMouseProvider.maxSafeMouseButton);
				}
				return false;
			}
			return false;
		}

		// Token: 0x060003B9 RID: 953 RVA: 0x0000D364 File Offset: 0x0000B564
		private void ClearState()
		{
			Array.Clear(this.lastButtonPressed, 0, this.lastButtonPressed.Length);
			Array.Clear(this.buttonPressed, 0, this.buttonPressed.Length);
			this.lastPosition = Vector2.zero;
			this.position = Vector2.zero;
			this.delta = Vector2.zero;
			this.scroll = Vector2.zero;
		}

		// Token: 0x060003BA RID: 954 RVA: 0x0000D3C5 File Offset: 0x0000B5C5
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Vector2 GetPosition()
		{
			return this.position;
		}

		// Token: 0x060003BB RID: 955 RVA: 0x0000D3CD File Offset: 0x0000B5CD
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public float GetDeltaX()
		{
			return this.delta.x;
		}

		// Token: 0x060003BC RID: 956 RVA: 0x0000D3DA File Offset: 0x0000B5DA
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public float GetDeltaY()
		{
			return this.delta.y;
		}

		// Token: 0x060003BD RID: 957 RVA: 0x0000D3E7 File Offset: 0x0000B5E7
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public float GetDeltaScroll()
		{
			if (Utility.Abs(this.scroll.x) <= Utility.Abs(this.scroll.y))
			{
				return this.scroll.y;
			}
			return this.scroll.x;
		}

		// Token: 0x060003BE RID: 958 RVA: 0x0000D422 File Offset: 0x0000B622
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool GetButtonIsPressed(Mouse control)
		{
			return this.buttonPressed[(int)control];
		}

		// Token: 0x060003BF RID: 959 RVA: 0x0000D42C File Offset: 0x0000B62C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool GetButtonWasPressed(Mouse control)
		{
			return this.buttonPressed[(int)control] && !this.lastButtonPressed[(int)control];
		}

		// Token: 0x060003C0 RID: 960 RVA: 0x0000D445 File Offset: 0x0000B645
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool GetButtonWasReleased(Mouse control)
		{
			return !this.buttonPressed[(int)control] && this.lastButtonPressed[(int)control];
		}

		// Token: 0x060003C1 RID: 961 RVA: 0x0000D45B File Offset: 0x0000B65B
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool HasMousePresent()
		{
			return Input.mousePresent;
		}

		// Token: 0x04000381 RID: 897
		private const string mouseXAxis = "mouse x";

		// Token: 0x04000382 RID: 898
		private const string mouseYAxis = "mouse y";

		// Token: 0x04000383 RID: 899
		private readonly bool[] lastButtonPressed = new bool[16];

		// Token: 0x04000384 RID: 900
		private readonly bool[] buttonPressed = new bool[16];

		// Token: 0x04000385 RID: 901
		private Vector2 lastPosition;

		// Token: 0x04000386 RID: 902
		private Vector2 position;

		// Token: 0x04000387 RID: 903
		private Vector2 delta;

		// Token: 0x04000388 RID: 904
		private Vector2 scroll;

		// Token: 0x04000389 RID: 905
		private static int maxSafeMouseButton = int.MaxValue;
	}
}
