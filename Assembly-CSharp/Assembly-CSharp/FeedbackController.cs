using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using UnityEngine.UI;

// Token: 0x0200010E RID: 270
public class FeedbackController : MonoSingleton<FeedbackController>
{
	// Token: 0x17000248 RID: 584
	// (get) Token: 0x060008FC RID: 2300 RVA: 0x0004B64A File Offset: 0x0004984A
	// (set) Token: 0x060008FD RID: 2301 RVA: 0x0004B652 File Offset: 0x00049852
	public bool visible { get; private set; }

	// Token: 0x060008FE RID: 2302 RVA: 0x0004B65C File Offset: 0x0004985C
	public void Show(float delay = 0f)
	{
		if (this.visible || this._layout.isAnimating)
		{
			return;
		}
		if (MonoSingleton<RunTrack>.instance.playingOrAboutTo)
		{
			MonoSingleton<RunTrack>.instance.SetPaused(true, RunTrack.PauseReason.Feedback);
		}
		Game.instance.SetTimeScalar(Game.TimeScalar.Feedback, 0f);
		Runner.instance.playerControlDisabled |= PlayerControlDisableReason.Feedback;
		this._layout.gameObject.SetActive(true);
		this._layout.groupAlpha = 0f;
		this._panel.scale = this._settings.hiddenScale;
		this.ResetSendButton();
		this._layout.Animate(0.3f, delay + 0.03f, delegate
		{
			this._layout.groupAlpha = 1f;
			this._panel.scale = 1f;
		}).Then(delegate
		{
			string @string = PlayerPrefsX.GetString("emailAddressForFeedback", "");
			this._emailField.text = @string;
			if (string.IsNullOrWhiteSpace(@string))
			{
				this._emailField.ActivateInputField();
				return;
			}
			this._inputField.ActivateInputField();
		});
		this._layout.After(delay, delegate
		{
			this.CaptureScreenshotAndGameState();
		});
		GameInput.PushControlStack(this);
		this.visible = true;
		FeedbackController.wantsGameplayPaused = true;
		Cursor.visible = true;
		Cursor.lockState = CursorLockMode.None;
		EventSystem.current.SetSelectedGameObject(this._panel.gameObject);
	}

	// Token: 0x060008FF RID: 2303 RVA: 0x0004B77C File Offset: 0x0004997C
	public void SetFeedbackTarget(string targetName)
	{
		this._emailToggle.SetIsOnWithoutNotify(targetName == "email");
		this._jonToggle.SetIsOnWithoutNotify(targetName == "jon");
		this._joeToggle.SetIsOnWithoutNotify(targetName == "joe");
		PlayerPrefsX.SetString("feedbackTarget", targetName);
	}

	// Token: 0x06000900 RID: 2304 RVA: 0x0004B7D8 File Offset: 0x000499D8
	private void Hide()
	{
		if (!this.visible || this._layout.isAnimating)
		{
			return;
		}
		Game.instance.RemoveTimeScalar(Game.TimeScalar.Feedback);
		this._hiding = true;
		this._layout.Animate(0.3f, delegate
		{
			this._layout.groupAlpha = 0f;
			this._panel.scale = this._settings.hiddenScale;
		}).Then(delegate
		{
			this._layout.gameObject.SetActive(false);
			GameInput.PopControlStack(this, true);
			Runner.instance.playerControlDisabled &= ~PlayerControlDisableReason.Feedback;
			this.ResetSendButton();
			this.Clear();
			this._hiding = false;
		});
		if (MonoSingleton<RunTrack>.instance.playingOrAboutTo)
		{
			MonoSingleton<RunTrack>.instance.SetPaused(false, RunTrack.PauseReason.Feedback);
		}
		this.visible = false;
		FeedbackController.wantsGameplayPaused = false;
		Cursor.visible = false;
		EventSystem.current.SetSelectedGameObject(null);
	}

	// Token: 0x06000901 RID: 2305 RVA: 0x0004B874 File Offset: 0x00049A74
	private void Clear()
	{
		this._inputField.text = "";
		this._screenshotImage.texture = null;
		if (this._screenshotTex != null)
		{
			Object.DestroyImmediate(this._screenshotTex);
			this._screenshotTex = null;
		}
		this._screenshotBytes = null;
		this._zipBytes = null;
		this._historyJson = null;
		this._attachSaveToggle.isOn = true;
		this._attachScreenshotToggle.isOn = true;
	}

	// Token: 0x06000902 RID: 2306 RVA: 0x0004B8EC File Offset: 0x00049AEC
	private void CaptureScreenshotAndGameState()
	{
		base.StartCoroutine(this.CaptureScreenshotAtEndOfFrame());
		this._historyJson = null;
		if (History.frames.Count > 0)
		{
			SerializedHistory serializedHistory = new SerializedHistory(History.frames);
			this._historyJson = JsonUtility.ToJson(serializedHistory);
		}
		this._zipBytes = this.CreateZipWithGameStateFiles();
	}

	// Token: 0x06000903 RID: 2307 RVA: 0x0004B943 File Offset: 0x00049B43
	private IEnumerator CaptureScreenshotAtEndOfFrame()
	{
		yield return new WaitForEndOfFrame();
		this._screenshotTex = ScreenCapture.CaptureScreenshotAsTexture();
		this._screenshotImage.texture = this._screenshotTex;
		this._screenshotBytes = this._screenshotTex.EncodeToJPG();
		yield break;
	}

	// Token: 0x06000904 RID: 2308 RVA: 0x0004B954 File Offset: 0x00049B54
	private void Start()
	{
		this._layout.groupAlpha = 0f;
		this._panel.scale = this._settings.hiddenScale;
		this._layout.gameObject.SetActive(false);
		this._emailToggle.gameObject.SetActive(false);
		this._jonToggle.gameObject.SetActive(false);
		this._joeToggle.gameObject.SetActive(false);
	}

	// Token: 0x06000905 RID: 2309 RVA: 0x0004B9CB File Offset: 0x00049BCB
	private void OnEnable()
	{
		Application.logMessageReceived += this.OnUnityDebugLog;
	}

	// Token: 0x06000906 RID: 2310 RVA: 0x0004B9DE File Offset: 0x00049BDE
	private void OnDisable()
	{
		Application.logMessageReceived -= this.OnUnityDebugLog;
	}

	// Token: 0x06000907 RID: 2311 RVA: 0x0004B9F1 File Offset: 0x00049BF1
	private void OnUnityDebugLog(string condition, string stackTrace, LogType type)
	{
		this._lastNUnityLogs.Enqueue(condition + "\n" + stackTrace);
		while (this._lastNUnityLogs.Count > 20)
		{
			this._lastNUnityLogs.Dequeue();
		}
	}

	// Token: 0x06000908 RID: 2312 RVA: 0x0004BA28 File Offset: 0x00049C28
	private void Update()
	{
		if (GameInput.Back(this) && this.visible)
		{
			this.Hide();
		}
		bool flag = Input.GetKey(KeyCode.LeftMeta) || Input.GetKey(KeyCode.RightMeta);
		if (this.visible && (Input.GetKeyDown(KeyCode.KeypadEnter) || (flag && Input.GetKeyDown(KeyCode.Return))))
		{
			this.OnSendClicked();
		}
	}

	// Token: 0x06000909 RID: 2313 RVA: 0x0004BA8B File Offset: 0x00049C8B
	public void OnSendClicked()
	{
		base.StartCoroutine(this.UploadCR());
	}

	// Token: 0x0600090A RID: 2314 RVA: 0x0004BA9A File Offset: 0x00049C9A
	public void OnCancelClicked()
	{
		this.Hide();
	}

	// Token: 0x0600090B RID: 2315 RVA: 0x0004BAA2 File Offset: 0x00049CA2
	public void OnToggleScreenshotValueChanged()
	{
		this._screenshotImage.color = Color.white.WithAlpha(this._attachScreenshotToggle.isOn ? 1f : 0.2f);
	}

	// Token: 0x0600090C RID: 2316 RVA: 0x0004BAD2 File Offset: 0x00049CD2
	public void OnToggleSaveValueChanged()
	{
	}

	// Token: 0x0600090D RID: 2317 RVA: 0x0004BAD4 File Offset: 0x00049CD4
	public void OnEmailValueChanged()
	{
		bool flag = string.IsNullOrWhiteSpace(this._emailField.text) || Regex.IsMatch(this._emailField.text, "^[A-Za-z0-9._+\\-\\']+@[A-Za-z0-9.\\-]+\\.[A-Za-z]{2,}$");
		this._emailField.textComponent.color = (flag ? Color.white : Color.red);
		PlayerPrefsX.SetString("emailAddressForFeedback", this._emailField.text);
	}

	// Token: 0x0600090E RID: 2318 RVA: 0x0004BB40 File Offset: 0x00049D40
	private byte[] CreateZipWithGameStateFiles()
	{
		byte[] array = null;
		try
		{
			string text = Path.Combine(Application.temporaryCachePath, "highland_feedback");
			if (Directory.Exists(text))
			{
				Directory.Delete(text, true);
			}
			Directory.CreateDirectory(text);
			bool flag = SaveLoadManager.TryCopySave(SaveLoadType.Latest, Path.Combine(text, "latest_save.json"), false);
			bool flag2 = SaveLoadManager.TryCopySave(SaveLoadType.Backup1, Path.Combine(text, "backup_save.json"), false);
			bool flag3 = false;
			if (this._historyJson != null)
			{
				File.WriteAllText(Path.Combine(text, "history.json"), this._historyJson);
				flag3 = true;
			}
			bool flag4 = false;
			string consoleLogPath = Application.consoleLogPath;
			if (!string.IsNullOrWhiteSpace(consoleLogPath))
			{
				if (File.Exists(consoleLogPath))
				{
					File.Copy(consoleLogPath, Path.Combine(text, "unity_log.txt"));
					flag4 = true;
				}
				string directoryName = Path.GetDirectoryName(consoleLogPath);
				string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(consoleLogPath);
				string extension = Path.GetExtension(consoleLogPath);
				string text2 = Path.Combine(directoryName, fileNameWithoutExtension + "-prev" + extension);
				if (File.Exists(text2))
				{
					File.Copy(text2, Path.Combine(text, "unity_log-prev.txt"));
					flag4 = true;
				}
			}
			bool flag5 = false;
			if (Narrative.instance.debugLog.Count > 0)
			{
				File.WriteAllText(Path.Combine(text, "ink_log.txt"), string.Join("\n", Narrative.instance.debugLog));
				flag5 = true;
			}
			if (!flag && !flag2 && !flag3 && !flag4 && !flag5)
			{
				return null;
			}
			string text3 = Path.Combine(Application.temporaryCachePath, "highland_feedback.zip");
			if (File.Exists(text3))
			{
				File.Delete(text3);
			}
			ZipFile.CreateFromDirectory(text, text3);
			array = File.ReadAllBytes(text3);
			File.Delete(text3);
		}
		catch (Exception ex)
		{
			Debug.LogError("Error when creating zip file of save files: " + ex.Message);
		}
		return array;
	}

	// Token: 0x0600090F RID: 2319 RVA: 0x0004BD00 File Offset: 0x00049F00
	private string CreateStateSummary(bool html)
	{
		FeedbackController.<>c__DisplayClass22_0 CS$<>8__locals1;
		CS$<>8__locals1.html = html;
		CS$<>8__locals1.sb = new StringBuilder();
		if (!this._attachSaveToggle.isOn)
		{
			CS$<>8__locals1.sb.Append("NO SAVE DATA ATTACHED AT PLAYER REQUEST\n");
		}
		FeedbackController.<CreateStateSummary>g__AddLine|22_0("Level number", (Level.currentIndex + 1).ToString(), ref CS$<>8__locals1);
		FeedbackController.<CreateStateSummary>g__AddLine|22_0("Playthrough index", Game.instance.playthroughIdx.ToString(), ref CS$<>8__locals1);
		FeedbackController.<CreateStateSummary>g__AddLine|22_0("Day index (0-based)", GameClock.instance.dayIdx.ToString(), ref CS$<>8__locals1);
		FeedbackController.<CreateStateSummary>g__AddLine|22_0("Hour", GameClock.instance.hourOfDay.ToString("F2"), ref CS$<>8__locals1);
		FeedbackController.<CreateStateSummary>g__AddLine|22_0("Game state", Game.instance.stateName, ref CS$<>8__locals1);
		FeedbackController.<CreateStateSummary>g__AddLine|22_0("Runner state", Runner.instance.state.ToString(), ref CS$<>8__locals1);
		string text = SaveLoadManager.CantSaveReason(false);
		FeedbackController.<CreateStateSummary>g__AddLine|22_0("Can't save reason", (text == null) ? "none" : text, ref CS$<>8__locals1);
		FeedbackController.<CreateStateSummary>g__AddLine|22_0("Runner disable reason", (Runner.instance.playerControlDisabled & ~PlayerControlDisableReason.Feedback).ToString(), ref CS$<>8__locals1);
		FeedbackController.<CreateStateSummary>g__AddLine|22_0("Runner hide reason", Runner.instance.hideReason.ToString(), ref CS$<>8__locals1);
		FeedbackController.<CreateStateSummary>g__AddLine|22_0("Narrative ink busy", Narrative.instance.isInkBusy.ToString(), ref CS$<>8__locals1);
		FeedbackController.<CreateStateSummary>g__AddLine|22_0("NarrativePresenter busy", NarrativePresenter.presenting.ToString(), ref CS$<>8__locals1);
		FeedbackController.<CreateStateSummary>g__AddLine|22_0("Narrative latest start path", Narrative.instance.debugInkEvaluationStartPoint, ref CS$<>8__locals1);
		string text2 = string.Join(", ", PropsController.instance.triggerRangeProps.Select((Prop p) => p.inkListItemName));
		FeedbackController.<CreateStateSummary>g__AddLine|22_0("Trigger-range props", text2, ref CS$<>8__locals1);
		string text3 = "Last interacted prop";
		Prop lastInteractedProp = PropsController.instance.lastInteractedProp;
		FeedbackController.<CreateStateSummary>g__AddLine|22_0(text3, (lastInteractedProp != null) ? lastInteractedProp.inkListItemName : null, ref CS$<>8__locals1);
		string text4 = null;
		List<Prop> all = Level.current.props.all;
		if (all.Count > 0)
		{
			Vector3 runnerPos = Runner.instance.transform.position;
			text4 = all.WithMin((Prop p) => Vector3.Distance(runnerPos, p.transform.position)).inkListItemName;
		}
		FeedbackController.<CreateStateSummary>g__AddLine|22_0("Nearest prop", text4, ref CS$<>8__locals1);
		string text5 = "Last music track";
		BeatTrack track = MonoSingleton<RunTrack>.instance.track;
		FeedbackController.<CreateStateSummary>g__AddLine|22_0(text5, (track != null) ? track.name : null, ref CS$<>8__locals1);
		FeedbackController.<CreateStateSummary>g__AddLine|22_0("RunTrack bar idx", MonoSingleton<RunTrack>.instance.currBarIdx.ToString(), ref CS$<>8__locals1);
		MusicRun musicRun = Runner.instance.currentMusicRun;
		if (musicRun == null)
		{
			musicRun = Runner.instance.prevMusicRun;
		}
		if (musicRun != null)
		{
			string text6 = string.Join(", ", musicRun.chunks.Select((Chunk c) => c.name));
			FeedbackController.<CreateStateSummary>g__AddLine|22_0("Nearby chunks", text6, ref CS$<>8__locals1);
			foreach (Chunk chunk in musicRun.chunks)
			{
				if (chunk.range.Contains(Runner.instance.position.x))
				{
					FeedbackController.<CreateStateSummary>g__AddLine|22_0("On chunk", chunk.name, ref CS$<>8__locals1);
				}
			}
		}
		string text7 = "none";
		List<InkAnimation> activeNonLoopingAnims = InkAnimation.activeNonLoopingAnims;
		if (activeNonLoopingAnims.Count > 0)
		{
			text7 = string.Join<InkAnimation>(", ", activeNonLoopingAnims);
		}
		FeedbackController.<CreateStateSummary>g__AddLine|22_0("Active non-looping ink anims", text7, ref CS$<>8__locals1);
		if (activeNonLoopingAnims.Count > 0)
		{
			foreach (InkAnimation inkAnimation in activeNonLoopingAnims)
			{
				FeedbackController.<CreateStateSummary>g__AddLine|22_0(inkAnimation.name + " debug info", inkAnimation.debugInfo, ref CS$<>8__locals1);
			}
		}
		CS$<>8__locals1.sb.AppendLine("\nEND OF INK LOG: (full log in zip)");
		List<string> debugLog = Narrative.instance.debugLog;
		for (int i = Mathf.Max(0, debugLog.Count - 90); i < debugLog.Count; i++)
		{
			if (CS$<>8__locals1.html)
			{
				CS$<>8__locals1.sb.Append("<p>");
			}
			CS$<>8__locals1.sb.Append(debugLog[i]);
			if (CS$<>8__locals1.html)
			{
				CS$<>8__locals1.sb.Append("</p>");
			}
			CS$<>8__locals1.sb.AppendLine();
		}
		CS$<>8__locals1.sb.AppendLine("\n\n\nEND OF UNITY LOG: (full log in zip)");
		foreach (string text8 in this._lastNUnityLogs)
		{
			CS$<>8__locals1.sb.AppendLine(text8);
		}
		return CS$<>8__locals1.sb.ToString();
	}

	// Token: 0x06000910 RID: 2320 RVA: 0x0004C234 File Offset: 0x0004A434
	private void UpdateProgressBar(int taskIdx, int totalTasks, float currTaskProgress)
	{
		this._sendButtonProgressBar.alpha = 1f;
		float width = this._sendButtonProgressBar.parentRect.width;
		float num = Mathf.Lerp((float)taskIdx / (float)totalTasks, (float)(taskIdx + 1) / (float)totalTasks, currTaskProgress);
		this._sendButtonProgressBar.width = Mathf.Lerp(0.25f * width, width, num);
	}

	// Token: 0x06000911 RID: 2321 RVA: 0x0004C291 File Offset: 0x0004A491
	private IEnumerator UploadCR()
	{
		string text = this._emailField.text;
		string text2 = this._inputField.text.Replace("\n", "<br/>");
		this._sendButton.interactable = false;
		this._sendButtonText.textMeshPro.text = "Sending...";
		this._sendButtonText.textMeshPro.alpha = 0.5f;
		string text3 = CurrentVersionSO.Instance.version.ToBasicWithSHAString();
		string text4 = this.CreateStateSummary(true);
		text2 = string.Concat(new string[] { text2, "<br/><br/><br/> ---------------- <br/><br/>", text3, "<br/>", text4 });
		Debug.Log("Sending feedback: " + text2);
		List<IMultipartFormSection> list = new List<IMultipartFormSection>();
		if (!string.IsNullOrWhiteSpace(text))
		{
			list.Add(new MultipartFormDataSection("from", text));
		}
		if (!string.IsNullOrWhiteSpace(text2))
		{
			list.Add(new MultipartFormDataSection("body", text2));
		}
		else
		{
			list.Add(new MultipartFormDataSection("body", "No description given"));
		}
		if (this._zipBytes != null && this._attachSaveToggle.isOn)
		{
			list.Add(new MultipartFormFileSection("attachment", this._zipBytes, "highland_feedback.zip", "application/zip"));
		}
		if (this._screenshotBytes != null && this._attachScreenshotToggle.isOn)
		{
			list.Add(new MultipartFormFileSection("attachedScreenshot", this._screenshotBytes, "screenshot.jpg", "image/jpeg"));
		}
		using (UnityWebRequest req = UnityWebRequest.Post(this._settings.url, list))
		{
			req.timeout = this._settings.timeout;
			UnityWebRequestAsyncOperation request = req.SendWebRequest();
			while (!request.isDone)
			{
				this.UpdateProgressBar(0, 1, request.progress);
				if (!this.visible || this._hiding)
				{
					yield break;
				}
				yield return null;
			}
			if (request.webRequest.result == UnityWebRequest.Result.ConnectionError || request.webRequest.result == UnityWebRequest.Result.ProtocolError)
			{
				Debug.LogError(req.error + "\n" + req.downloadHandler.text);
				MonoSingleton<Dialogue>.instance.Show("Failed to send", "Sorry, feedback couldn't be sent because an error occurred: " + req.error + "\n" + req.downloadHandler.text, "Okay", null, false, null);
				this.ResetSendButton();
			}
			else
			{
				this._sendButtonProgressBar.width = this._sendButtonProgressBar.parentRect.width;
				this._sendButtonText.textMeshPro.text = "Sent!";
				yield return new WaitForSecondsRealtime(0.5f);
				this.Hide();
			}
			request = null;
		}
		UnityWebRequest req = null;
		yield break;
		yield break;
	}

	// Token: 0x06000912 RID: 2322 RVA: 0x0004C2A0 File Offset: 0x0004A4A0
	private void ResetSendButton()
	{
		this._sendButtonText.textMeshPro.text = "Send";
		this._sendButtonText.textMeshPro.alpha = 1f;
		this._sendButtonProgressBar.alpha = 0f;
		this._sendButton.interactable = true;
	}

	// Token: 0x06000919 RID: 2329 RVA: 0x0004C3F0 File Offset: 0x0004A5F0
	[CompilerGenerated]
	internal static void <CreateStateSummary>g__AddLine|22_0(string property, string val, ref FeedbackController.<>c__DisplayClass22_0 A_2)
	{
		if (A_2.html)
		{
			A_2.sb.Append("<p>");
		}
		A_2.sb.Append(property);
		A_2.sb.Append(": ");
		A_2.sb.Append((val == null) ? "NONE" : val);
		if (A_2.html)
		{
			A_2.sb.Append("</p>");
		}
		A_2.sb.Append("\n");
	}

	// Token: 0x04000AD1 RID: 2769
	public static bool wantsGameplayPaused;

	// Token: 0x04000AD2 RID: 2770
	private const string missingChunksTaskId = "1205464104665369";

	// Token: 0x04000AD3 RID: 2771
	private List<FeedbackController.AsanaSubtask> _cachedMissingChunkTasks;

	// Token: 0x04000AD4 RID: 2772
	private bool _hiding;

	// Token: 0x04000AD5 RID: 2773
	private byte[] _screenshotBytes;

	// Token: 0x04000AD6 RID: 2774
	private Texture2D _screenshotTex;

	// Token: 0x04000AD7 RID: 2775
	private byte[] _zipBytes;

	// Token: 0x04000AD8 RID: 2776
	private string _historyJson;

	// Token: 0x04000AD9 RID: 2777
	private Queue<string> _lastNUnityLogs = new Queue<string>();

	// Token: 0x04000ADA RID: 2778
	[SerializeField]
	private SLayout _layout;

	// Token: 0x04000ADB RID: 2779
	[SerializeField]
	private SLayout _panel;

	// Token: 0x04000ADC RID: 2780
	[SerializeField]
	private Button _sendButton;

	// Token: 0x04000ADD RID: 2781
	[SerializeField]
	private SLayout _sendButtonProgressBar;

	// Token: 0x04000ADE RID: 2782
	[SerializeField]
	private SLayout _sendButtonText;

	// Token: 0x04000ADF RID: 2783
	[SerializeField]
	private RawImage _screenshotImage;

	// Token: 0x04000AE0 RID: 2784
	[SerializeField]
	private Toggle _attachScreenshotToggle;

	// Token: 0x04000AE1 RID: 2785
	[SerializeField]
	private Toggle _attachSaveToggle;

	// Token: 0x04000AE2 RID: 2786
	[SerializeField]
	private Toggle _emailToggle;

	// Token: 0x04000AE3 RID: 2787
	[SerializeField]
	private Toggle _jonToggle;

	// Token: 0x04000AE4 RID: 2788
	[SerializeField]
	private Toggle _joeToggle;

	// Token: 0x04000AE5 RID: 2789
	[SerializeField]
	private TMP_InputField _emailField;

	// Token: 0x04000AE6 RID: 2790
	[SerializeField]
	private TMP_InputField _inputField;

	// Token: 0x04000AE7 RID: 2791
	[SerializeField]
	private FeedbackSettings _settings;

	// Token: 0x02000330 RID: 816
	[Serializable]
	private struct AsanaResponse
	{
		// Token: 0x04001821 RID: 6177
		public FeedbackController.AsanaResponseData data;
	}

	// Token: 0x02000331 RID: 817
	[Serializable]
	private struct AsanaResponseData
	{
		// Token: 0x04001822 RID: 6178
		public string gid;
	}

	// Token: 0x02000332 RID: 818
	[Serializable]
	public struct AsanaSubtasksResult
	{
		// Token: 0x04001823 RID: 6179
		public List<FeedbackController.AsanaSubtask> data;
	}

	// Token: 0x02000333 RID: 819
	[Serializable]
	public struct AsanaSubtask
	{
		// Token: 0x04001824 RID: 6180
		public string name;
	}
}
