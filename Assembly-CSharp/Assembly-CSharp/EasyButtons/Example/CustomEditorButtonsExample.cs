using System;
using UnityEngine;

namespace EasyButtons.Example
{
	// Token: 0x02000233 RID: 563
	public class CustomEditorButtonsExample : MonoBehaviour
	{
		// Token: 0x06001446 RID: 5190 RVA: 0x0008CB2D File Offset: 0x0008AD2D
		[Button("Custom Editor Example")]
		private void SayHello()
		{
			Debug.Log("Hello from custom editor");
		}

		// Token: 0x06001447 RID: 5191 RVA: 0x0008CB39 File Offset: 0x0008AD39
		[Button]
		private void SecondButton()
		{
			Debug.Log("Second button of the custom editor.");
		}
	}
}
