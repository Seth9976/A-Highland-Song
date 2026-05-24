using System;
using UnityEngine;

namespace EasyButtons.Example
{
	// Token: 0x02000234 RID: 564
	[CreateAssetMenu(fileName = "ScriptableObjectExample.asset", menuName = "EasyButtons/ScriptableObjectExample")]
	public class ScriptableObjectExample : ScriptableObject
	{
		// Token: 0x06001449 RID: 5193 RVA: 0x0008CB4D File Offset: 0x0008AD4D
		[Button]
		public void SayHello()
		{
			Debug.Log("Hello");
		}

		// Token: 0x0600144A RID: 5194 RVA: 0x0008CB59 File Offset: 0x0008AD59
		[Button(Mode = ButtonMode.DisabledInPlayMode)]
		public void SayHelloEditor()
		{
			Debug.Log("Hello from edit mode");
		}

		// Token: 0x0600144B RID: 5195 RVA: 0x0008CB65 File Offset: 0x0008AD65
		[Button(Mode = ButtonMode.EnabledInPlayMode)]
		public void SayHelloPlayMode()
		{
			Debug.Log("Hello from play mode");
		}
	}
}
