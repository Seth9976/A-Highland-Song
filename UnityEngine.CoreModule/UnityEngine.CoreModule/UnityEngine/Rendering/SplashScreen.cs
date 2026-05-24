using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;

namespace UnityEngine.Rendering
{
	// Token: 0x020003E4 RID: 996
	[NativeHeader("Runtime/Graphics/DrawSplashScreenAndWatermarks.h")]
	public class SplashScreen
	{
		// Token: 0x17000626 RID: 1574
		// (get) Token: 0x060021B1 RID: 8625
		public static extern bool isFinished
		{
			[FreeFunction("IsSplashScreenFinished")]
			[MethodImpl(4096)]
			get;
		}

		// Token: 0x060021B2 RID: 8626
		[FreeFunction]
		[MethodImpl(4096)]
		private static extern void CancelSplashScreen();

		// Token: 0x060021B3 RID: 8627
		[FreeFunction]
		[MethodImpl(4096)]
		private static extern void BeginSplashScreenFade();

		// Token: 0x060021B4 RID: 8628
		[FreeFunction("BeginSplashScreen_Binding")]
		[MethodImpl(4096)]
		public static extern void Begin();

		// Token: 0x060021B5 RID: 8629 RVA: 0x00036ED0 File Offset: 0x000350D0
		public static void Stop(SplashScreen.StopBehavior stopBehavior)
		{
			bool flag = stopBehavior == SplashScreen.StopBehavior.FadeOut;
			if (flag)
			{
				SplashScreen.BeginSplashScreenFade();
			}
			else
			{
				SplashScreen.CancelSplashScreen();
			}
		}

		// Token: 0x060021B6 RID: 8630
		[FreeFunction("DrawSplashScreen_Binding")]
		[MethodImpl(4096)]
		public static extern void Draw();

		// Token: 0x060021B7 RID: 8631
		[FreeFunction("SetSplashScreenTime")]
		[MethodImpl(4096)]
		internal static extern void SetTime(float time);

		// Token: 0x020003E5 RID: 997
		public enum StopBehavior
		{
			// Token: 0x04000C30 RID: 3120
			StopImmediate,
			// Token: 0x04000C31 RID: 3121
			FadeOut
		}
	}
}
