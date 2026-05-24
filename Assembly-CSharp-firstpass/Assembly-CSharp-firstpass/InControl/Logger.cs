using System;

namespace InControl
{
	// Token: 0x0200003E RID: 62
	public static class Logger
	{
		// Token: 0x14000006 RID: 6
		// (add) Token: 0x06000236 RID: 566 RVA: 0x00008528 File Offset: 0x00006728
		// (remove) Token: 0x06000237 RID: 567 RVA: 0x0000855C File Offset: 0x0000675C
		public static event Action<LogMessage> OnLogMessage;

		// Token: 0x06000238 RID: 568 RVA: 0x00008590 File Offset: 0x00006790
		public static void LogInfo(string text)
		{
			if (Logger.OnLogMessage != null)
			{
				LogMessage logMessage = new LogMessage
				{
					Text = text,
					Type = LogMessageType.Info
				};
				Logger.OnLogMessage(logMessage);
			}
		}

		// Token: 0x06000239 RID: 569 RVA: 0x000085CC File Offset: 0x000067CC
		public static void LogWarning(string text)
		{
			if (Logger.OnLogMessage != null)
			{
				LogMessage logMessage = new LogMessage
				{
					Text = text,
					Type = LogMessageType.Warning
				};
				Logger.OnLogMessage(logMessage);
			}
		}

		// Token: 0x0600023A RID: 570 RVA: 0x00008608 File Offset: 0x00006808
		public static void LogError(string text)
		{
			if (Logger.OnLogMessage != null)
			{
				LogMessage logMessage = new LogMessage
				{
					Text = text,
					Type = LogMessageType.Error
				};
				Logger.OnLogMessage(logMessage);
			}
		}
	}
}
