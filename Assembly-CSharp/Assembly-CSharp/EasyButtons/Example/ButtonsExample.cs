using System;
using UnityEngine;

namespace EasyButtons.Example
{
	// Token: 0x02000232 RID: 562
	public class ButtonsExample : MonoBehaviour
	{
		// Token: 0x0600143C RID: 5180 RVA: 0x0008CAB0 File Offset: 0x0008ACB0
		[Button]
		public void SayMyName()
		{
			Debug.Log(base.name);
		}

		// Token: 0x0600143D RID: 5181 RVA: 0x0008CABD File Offset: 0x0008ACBD
		[Button(Mode = ButtonMode.DisabledInPlayMode)]
		protected void SayHelloEditor()
		{
			Debug.Log("Hello from edit mode");
		}

		// Token: 0x0600143E RID: 5182 RVA: 0x0008CAC9 File Offset: 0x0008ACC9
		[Button(Mode = ButtonMode.EnabledInPlayMode)]
		private void SayHelloInRuntime()
		{
			Debug.Log("Hello from play mode");
		}

		// Token: 0x0600143F RID: 5183 RVA: 0x0008CAD5 File Offset: 0x0008ACD5
		[Button("Special Name", Spacing = ButtonSpacing.Before)]
		private void TestButtonName()
		{
			Debug.Log("Hello from special name button");
		}

		// Token: 0x06001440 RID: 5184 RVA: 0x0008CAE1 File Offset: 0x0008ACE1
		[Button("Special Name Editor Only", Mode = ButtonMode.DisabledInPlayMode)]
		private void TestButtonNameEditorOnly()
		{
			Debug.Log("Hello from special name button for editor only");
		}

		// Token: 0x06001441 RID: 5185 RVA: 0x0008CAED File Offset: 0x0008ACED
		[Button]
		private static void TestStaticMethod()
		{
			Debug.Log("Hello from static method");
		}

		// Token: 0x06001442 RID: 5186 RVA: 0x0008CAF9 File Offset: 0x0008ACF9
		[Button("Space Before and After", Spacing = ButtonSpacing.Before | ButtonSpacing.After)]
		private void TestButtonSpaceBoth()
		{
			Debug.Log("Hello from a button surround by spaces");
		}

		// Token: 0x06001443 RID: 5187 RVA: 0x0008CB05 File Offset: 0x0008AD05
		[Button("Button With Parameters")]
		private void TestButtonWithParams(string message, int number)
		{
			Debug.Log(string.Format("Your message #{0}: \"{1}\"", number, message));
		}

		// Token: 0x06001444 RID: 5188 RVA: 0x0008CB1D File Offset: 0x0008AD1D
		[Button("Expanded Button Example", Expanded = true)]
		private void TestExpandedButton(string message)
		{
			Debug.Log(message);
		}
	}
}
