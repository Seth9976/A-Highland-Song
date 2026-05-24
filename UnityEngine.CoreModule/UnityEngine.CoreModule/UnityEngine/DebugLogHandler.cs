using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;

namespace UnityEngine
{
	// Token: 0x02000108 RID: 264
	[NativeHeader("Runtime/Export/Debug/Debug.bindings.h")]
	internal sealed class DebugLogHandler : ILogHandler
	{
		// Token: 0x0600062B RID: 1579
		[ThreadAndSerializationSafe]
		[MethodImpl(4096)]
		internal static extern void Internal_Log(LogType level, LogOption options, string msg, Object obj);

		// Token: 0x0600062C RID: 1580
		[ThreadAndSerializationSafe]
		[MethodImpl(4096)]
		internal static extern void Internal_LogException(Exception ex, Object obj);

		// Token: 0x0600062D RID: 1581 RVA: 0x00008744 File Offset: 0x00006944
		public void LogFormat(LogType logType, Object context, string format, params object[] args)
		{
			DebugLogHandler.Internal_Log(logType, LogOption.None, string.Format(format, args), context);
		}

		// Token: 0x0600062E RID: 1582 RVA: 0x00008758 File Offset: 0x00006958
		public void LogFormat(LogType logType, LogOption logOptions, Object context, string format, params object[] args)
		{
			DebugLogHandler.Internal_Log(logType, logOptions, string.Format(format, args), context);
		}

		// Token: 0x0600062F RID: 1583 RVA: 0x00008770 File Offset: 0x00006970
		public void LogException(Exception exception, Object context)
		{
			bool flag = exception == null;
			if (flag)
			{
				throw new ArgumentNullException("exception");
			}
			DebugLogHandler.Internal_LogException(exception, context);
		}
	}
}
