using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEngine.Windows.Speech
{
	// Token: 0x0200028C RID: 652
	public abstract class PhraseRecognizer : IDisposable
	{
		// Token: 0x06001C32 RID: 7218
		[NativeHeader("PlatformDependent/Win/Bindings/SpeechBindings.h")]
		[NativeThrows]
		[MethodImpl(4096)]
		protected static extern IntPtr CreateFromKeywords(object self, string[] keywords, ConfidenceLevel minimumConfidence);

		// Token: 0x06001C33 RID: 7219
		[NativeHeader("PlatformDependent/Win/Bindings/SpeechBindings.h")]
		[NativeThrows]
		[MethodImpl(4096)]
		protected static extern IntPtr CreateFromGrammarFile(object self, string grammarFilePath, ConfidenceLevel minimumConfidence);

		// Token: 0x06001C34 RID: 7220
		[NativeThrows]
		[NativeHeader("PlatformDependent/Win/Bindings/SpeechBindings.h")]
		[MethodImpl(4096)]
		private static extern void Start_Internal(IntPtr recognizer);

		// Token: 0x06001C35 RID: 7221
		[NativeHeader("PlatformDependent/Win/Bindings/SpeechBindings.h")]
		[MethodImpl(4096)]
		private static extern void Stop_Internal(IntPtr recognizer);

		// Token: 0x06001C36 RID: 7222
		[NativeHeader("PlatformDependent/Win/Bindings/SpeechBindings.h")]
		[MethodImpl(4096)]
		private static extern bool IsRunning_Internal(IntPtr recognizer);

		// Token: 0x06001C37 RID: 7223
		[NativeHeader("PlatformDependent/Win/Bindings/SpeechBindings.h")]
		[MethodImpl(4096)]
		private static extern void Destroy(IntPtr recognizer);

		// Token: 0x06001C38 RID: 7224
		[ThreadSafe]
		[NativeHeader("PlatformDependent/Win/Bindings/SpeechBindings.h")]
		[MethodImpl(4096)]
		private static extern void DestroyThreaded(IntPtr recognizer);

		// Token: 0x1400001D RID: 29
		// (add) Token: 0x06001C39 RID: 7225 RVA: 0x0002D310 File Offset: 0x0002B510
		// (remove) Token: 0x06001C3A RID: 7226 RVA: 0x0002D348 File Offset: 0x0002B548
		[field: DebuggerBrowsable(0)]
		public event PhraseRecognizer.PhraseRecognizedDelegate OnPhraseRecognized;

		// Token: 0x06001C3B RID: 7227 RVA: 0x00008CAF File Offset: 0x00006EAF
		internal PhraseRecognizer()
		{
		}

		// Token: 0x06001C3C RID: 7228 RVA: 0x0002D380 File Offset: 0x0002B580
		protected override void Finalize()
		{
			try
			{
				bool flag = this.m_Recognizer != IntPtr.Zero;
				if (flag)
				{
					PhraseRecognizer.DestroyThreaded(this.m_Recognizer);
					this.m_Recognizer = IntPtr.Zero;
					GC.SuppressFinalize(this);
				}
			}
			finally
			{
				base.Finalize();
			}
		}

		// Token: 0x06001C3D RID: 7229 RVA: 0x0002D3E0 File Offset: 0x0002B5E0
		public void Start()
		{
			bool flag = this.m_Recognizer == IntPtr.Zero;
			if (!flag)
			{
				PhraseRecognizer.Start_Internal(this.m_Recognizer);
			}
		}

		// Token: 0x06001C3E RID: 7230 RVA: 0x0002D410 File Offset: 0x0002B610
		public void Stop()
		{
			bool flag = this.m_Recognizer == IntPtr.Zero;
			if (!flag)
			{
				PhraseRecognizer.Stop_Internal(this.m_Recognizer);
			}
		}

		// Token: 0x06001C3F RID: 7231 RVA: 0x0002D440 File Offset: 0x0002B640
		public void Dispose()
		{
			bool flag = this.m_Recognizer != IntPtr.Zero;
			if (flag)
			{
				PhraseRecognizer.Destroy(this.m_Recognizer);
				this.m_Recognizer = IntPtr.Zero;
			}
			GC.SuppressFinalize(this);
		}

		// Token: 0x170005A6 RID: 1446
		// (get) Token: 0x06001C40 RID: 7232 RVA: 0x0002D484 File Offset: 0x0002B684
		public bool IsRunning
		{
			get
			{
				return this.m_Recognizer != IntPtr.Zero && PhraseRecognizer.IsRunning_Internal(this.m_Recognizer);
			}
		}

		// Token: 0x06001C41 RID: 7233 RVA: 0x0002D4B8 File Offset: 0x0002B6B8
		[RequiredByNativeCode]
		private void InvokePhraseRecognizedEvent(string text, ConfidenceLevel confidence, SemanticMeaning[] semanticMeanings, long phraseStartFileTime, long phraseDurationTicks)
		{
			PhraseRecognizer.PhraseRecognizedDelegate onPhraseRecognized = this.OnPhraseRecognized;
			bool flag = onPhraseRecognized != null;
			if (flag)
			{
				onPhraseRecognized(new PhraseRecognizedEventArgs(text, confidence, semanticMeanings, DateTime.FromFileTime(phraseStartFileTime), TimeSpan.FromTicks(phraseDurationTicks)));
			}
		}

		// Token: 0x06001C42 RID: 7234 RVA: 0x0002D4F4 File Offset: 0x0002B6F4
		[RequiredByNativeCode]
		private unsafe static SemanticMeaning[] MarshalSemanticMeaning(IntPtr keys, IntPtr values, IntPtr valueSizes, int valueCount)
		{
			SemanticMeaning[] array = new SemanticMeaning[valueCount];
			int num = 0;
			for (int i = 0; i < valueCount; i++)
			{
				uint num2 = *(uint*)((byte*)(void*)valueSizes + (IntPtr)i * 4);
				SemanticMeaning semanticMeaning = new SemanticMeaning
				{
					key = new string(*(IntPtr*)((byte*)(void*)keys + (IntPtr)i * (IntPtr)sizeof(char*))),
					values = new string[num2]
				};
				int num3 = 0;
				while ((long)num3 < (long)((ulong)num2))
				{
					semanticMeaning.values[num3] = new string(*(IntPtr*)((byte*)(void*)values + (IntPtr)(num + num3) * (IntPtr)sizeof(char*)));
					num3++;
				}
				array[i] = semanticMeaning;
				num += (int)num2;
			}
			return array;
		}

		// Token: 0x04000927 RID: 2343
		protected IntPtr m_Recognizer;

		// Token: 0x0200028D RID: 653
		// (Invoke) Token: 0x06001C44 RID: 7236
		public delegate void PhraseRecognizedDelegate(PhraseRecognizedEventArgs args);
	}
}
