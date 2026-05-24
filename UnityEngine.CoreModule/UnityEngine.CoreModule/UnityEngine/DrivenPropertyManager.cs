using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;

namespace UnityEngine
{
	// Token: 0x020001D3 RID: 467
	[NativeHeader("Editor/Src/Properties/DrivenPropertyManager.h")]
	internal class DrivenPropertyManager
	{
		// Token: 0x060015C1 RID: 5569 RVA: 0x00022F4D File Offset: 0x0002114D
		[Conditional("UNITY_EDITOR")]
		public static void RegisterProperty(Object driver, Object target, string propertyPath)
		{
			DrivenPropertyManager.RegisterPropertyPartial(driver, target, propertyPath);
		}

		// Token: 0x060015C2 RID: 5570 RVA: 0x00022F59 File Offset: 0x00021159
		[Conditional("UNITY_EDITOR")]
		public static void TryRegisterProperty(Object driver, Object target, string propertyPath)
		{
			DrivenPropertyManager.TryRegisterPropertyPartial(driver, target, propertyPath);
		}

		// Token: 0x060015C3 RID: 5571 RVA: 0x00022F65 File Offset: 0x00021165
		[Conditional("UNITY_EDITOR")]
		public static void UnregisterProperty(Object driver, Object target, string propertyPath)
		{
			DrivenPropertyManager.UnregisterPropertyPartial(driver, target, propertyPath);
		}

		// Token: 0x060015C4 RID: 5572
		[NativeConditional("UNITY_EDITOR")]
		[Conditional("UNITY_EDITOR")]
		[StaticAccessor("GetDrivenPropertyManager()", StaticAccessorType.Dot)]
		[MethodImpl(4096)]
		public static extern void UnregisterProperties([NotNull("ArgumentNullException")] Object driver);

		// Token: 0x060015C5 RID: 5573
		[StaticAccessor("GetDrivenPropertyManager()", StaticAccessorType.Dot)]
		[NativeConditional("UNITY_EDITOR")]
		[MethodImpl(4096)]
		private static extern void RegisterPropertyPartial([NotNull("ArgumentNullException")] Object driver, [NotNull("ArgumentNullException")] Object target, [NotNull("ArgumentNullException")] string propertyPath);

		// Token: 0x060015C6 RID: 5574
		[NativeConditional("UNITY_EDITOR")]
		[StaticAccessor("GetDrivenPropertyManager()", StaticAccessorType.Dot)]
		[MethodImpl(4096)]
		private static extern void TryRegisterPropertyPartial([NotNull("ArgumentNullException")] Object driver, [NotNull("ArgumentNullException")] Object target, [NotNull("ArgumentNullException")] string propertyPath);

		// Token: 0x060015C7 RID: 5575
		[NativeConditional("UNITY_EDITOR")]
		[StaticAccessor("GetDrivenPropertyManager()", StaticAccessorType.Dot)]
		[MethodImpl(4096)]
		private static extern void UnregisterPropertyPartial([NotNull("ArgumentNullException")] Object driver, [NotNull("ArgumentNullException")] Object target, [NotNull("ArgumentNullException")] string propertyPath);
	}
}
