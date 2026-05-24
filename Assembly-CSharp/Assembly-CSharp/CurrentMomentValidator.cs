using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using UnityEngine;

// Token: 0x0200009F RID: 159
[Serializable]
public class CurrentMomentValidator
{
	// Token: 0x06000521 RID: 1313 RVA: 0x00028C8C File Offset: 0x00026E8C
	public int GetAdvanceLockID()
	{
		return this.advanceLockID;
	}

	// Token: 0x06000522 RID: 1314 RVA: 0x00028C94 File Offset: 0x00026E94
	public bool ValidateKey(int advanceKeyID)
	{
		return this.enabled && this.advanceLockID == advanceKeyID;
	}

	// Token: 0x06000523 RID: 1315 RVA: 0x00028CA9 File Offset: 0x00026EA9
	public bool AssertValidKey(int advanceKeyID)
	{
		if (!this.ValidateKey(advanceKeyID))
		{
			this.LogWrongKey(advanceKeyID);
			return false;
		}
		return true;
	}

	// Token: 0x06000524 RID: 1316 RVA: 0x00028CBE File Offset: 0x00026EBE
	public void AdvanceLock()
	{
		this.advanceLockID++;
	}

	// Token: 0x06000525 RID: 1317 RVA: 0x00028CCE File Offset: 0x00026ECE
	public void Reset()
	{
		this.enabled = false;
		this.advanceLockID = 0;
		this.advanceCallstack.Clear();
	}

	// Token: 0x06000526 RID: 1318 RVA: 0x00028CEC File Offset: 0x00026EEC
	public void StoreStackTrace(int advanceKeyID)
	{
		StackFrame[] frames = new StackTrace().GetFrames();
		StringBuilder stringBuilder = new StringBuilder();
		foreach (StackFrame stackFrame in frames)
		{
			stringBuilder.AppendLine(stackFrame.ToString());
		}
		this.advanceCallstack[advanceKeyID] = stringBuilder.ToString();
	}

	// Token: 0x06000527 RID: 1319 RVA: 0x00028D3C File Offset: 0x00026F3C
	private void LogWrongKey(int advanceKeyID)
	{
		string text = string.Concat(new string[]
		{
			"CurrentMomentValidator: Tried AdvanceViewEventQueue with keyID ",
			advanceKeyID.ToString(),
			" but lockID is ",
			this.advanceLockID.ToString(),
			". Validator enabled state is ",
			this.enabled.ToString()
		});
		if (advanceKeyID != this.advanceLockID)
		{
			text += "\nThis is probably because the story shouldn't have been advanced while someone held a lock! Why did this happen?";
		}
		else
		{
			text += "\nThis is probably because the story was interacted with before it was ready.";
		}
		if (this.advanceCallstack.ContainsKey(advanceKeyID))
		{
			text = text + "\nkeyID was already triggered by\n" + this.advanceCallstack[advanceKeyID];
		}
		else
		{
			text += "\nkeyID has not yet been triggered.";
		}
		Debug.LogWarning(text);
		DebugX.LogDictionary<int, string>("Full advance callstack:", this.advanceCallstack, null, true);
	}

	// Token: 0x040005F1 RID: 1521
	public bool enabled;

	// Token: 0x040005F2 RID: 1522
	[SerializeField]
	[Disable]
	private int advanceLockID;

	// Token: 0x040005F3 RID: 1523
	private Dictionary<int, string> advanceCallstack = new Dictionary<int, string>();
}
