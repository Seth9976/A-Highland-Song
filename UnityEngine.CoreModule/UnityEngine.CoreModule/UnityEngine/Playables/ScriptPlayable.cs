using System;

namespace UnityEngine.Playables
{
	// Token: 0x02000449 RID: 1097
	public struct ScriptPlayable<T> : IPlayable, IEquatable<ScriptPlayable<T>> where T : class, IPlayableBehaviour, new()
	{
		// Token: 0x1700072E RID: 1838
		// (get) Token: 0x060026C6 RID: 9926 RVA: 0x00040988 File Offset: 0x0003EB88
		public static ScriptPlayable<T> Null
		{
			get
			{
				return ScriptPlayable<T>.m_NullPlayable;
			}
		}

		// Token: 0x060026C7 RID: 9927 RVA: 0x000409A0 File Offset: 0x0003EBA0
		public static ScriptPlayable<T> Create(PlayableGraph graph, int inputCount = 0)
		{
			PlayableHandle playableHandle = ScriptPlayable<T>.CreateHandle(graph, default(T), inputCount);
			return new ScriptPlayable<T>(playableHandle);
		}

		// Token: 0x060026C8 RID: 9928 RVA: 0x000409CC File Offset: 0x0003EBCC
		public static ScriptPlayable<T> Create(PlayableGraph graph, T template, int inputCount = 0)
		{
			PlayableHandle playableHandle = ScriptPlayable<T>.CreateHandle(graph, template, inputCount);
			return new ScriptPlayable<T>(playableHandle);
		}

		// Token: 0x060026C9 RID: 9929 RVA: 0x000409F0 File Offset: 0x0003EBF0
		private static PlayableHandle CreateHandle(PlayableGraph graph, T template, int inputCount)
		{
			bool flag = template == null;
			object obj;
			if (flag)
			{
				obj = ScriptPlayable<T>.CreateScriptInstance();
			}
			else
			{
				obj = ScriptPlayable<T>.CloneScriptInstance(template);
			}
			bool flag2 = obj == null;
			PlayableHandle playableHandle;
			if (flag2)
			{
				string text = "Could not create a ScriptPlayable of Type ";
				Type typeFromHandle = typeof(T);
				Debug.LogError(text + ((typeFromHandle != null) ? typeFromHandle.ToString() : null));
				playableHandle = PlayableHandle.Null;
			}
			else
			{
				PlayableHandle playableHandle2 = graph.CreatePlayableHandle();
				bool flag3 = !playableHandle2.IsValid();
				if (flag3)
				{
					playableHandle = PlayableHandle.Null;
				}
				else
				{
					playableHandle2.SetInputCount(inputCount);
					playableHandle2.SetScriptInstance(obj);
					playableHandle = playableHandle2;
				}
			}
			return playableHandle;
		}

		// Token: 0x060026CA RID: 9930 RVA: 0x00040A98 File Offset: 0x0003EC98
		private static object CreateScriptInstance()
		{
			bool flag = typeof(ScriptableObject).IsAssignableFrom(typeof(T));
			IPlayableBehaviour playableBehaviour;
			if (flag)
			{
				playableBehaviour = ScriptableObject.CreateInstance(typeof(T)) as T;
			}
			else
			{
				playableBehaviour = new T();
			}
			return playableBehaviour;
		}

		// Token: 0x060026CB RID: 9931 RVA: 0x00040AF8 File Offset: 0x0003ECF8
		private static object CloneScriptInstance(IPlayableBehaviour source)
		{
			Object @object = source as Object;
			bool flag = @object != null;
			object obj;
			if (flag)
			{
				obj = ScriptPlayable<T>.CloneScriptInstanceFromEngineObject(@object);
			}
			else
			{
				ICloneable cloneable = source as ICloneable;
				bool flag2 = cloneable != null;
				if (flag2)
				{
					obj = ScriptPlayable<T>.CloneScriptInstanceFromIClonable(cloneable);
				}
				else
				{
					obj = null;
				}
			}
			return obj;
		}

		// Token: 0x060026CC RID: 9932 RVA: 0x00040B40 File Offset: 0x0003ED40
		private static object CloneScriptInstanceFromEngineObject(Object source)
		{
			Object @object = Object.Instantiate(source);
			bool flag = @object != null;
			if (flag)
			{
				@object.hideFlags |= HideFlags.DontSave;
			}
			return @object;
		}

		// Token: 0x060026CD RID: 9933 RVA: 0x00040B78 File Offset: 0x0003ED78
		private static object CloneScriptInstanceFromIClonable(ICloneable source)
		{
			return source.Clone();
		}

		// Token: 0x060026CE RID: 9934 RVA: 0x00040B90 File Offset: 0x0003ED90
		internal ScriptPlayable(PlayableHandle handle)
		{
			bool flag = handle.IsValid();
			if (flag)
			{
				bool flag2 = !typeof(T).IsAssignableFrom(handle.GetPlayableType());
				if (flag2)
				{
					throw new InvalidCastException(string.Format("Incompatible handle: Trying to assign a playable data of type `{0}` that is not compatible with the PlayableBehaviour of type `{1}`.", handle.GetPlayableType(), typeof(T)));
				}
			}
			this.m_Handle = handle;
		}

		// Token: 0x060026CF RID: 9935 RVA: 0x00040BF0 File Offset: 0x0003EDF0
		public PlayableHandle GetHandle()
		{
			return this.m_Handle;
		}

		// Token: 0x060026D0 RID: 9936 RVA: 0x00040C08 File Offset: 0x0003EE08
		public T GetBehaviour()
		{
			return this.m_Handle.GetObject<T>();
		}

		// Token: 0x060026D1 RID: 9937 RVA: 0x00040C28 File Offset: 0x0003EE28
		public static implicit operator Playable(ScriptPlayable<T> playable)
		{
			return new Playable(playable.GetHandle());
		}

		// Token: 0x060026D2 RID: 9938 RVA: 0x00040C48 File Offset: 0x0003EE48
		public static explicit operator ScriptPlayable<T>(Playable playable)
		{
			return new ScriptPlayable<T>(playable.GetHandle());
		}

		// Token: 0x060026D3 RID: 9939 RVA: 0x00040C68 File Offset: 0x0003EE68
		public bool Equals(ScriptPlayable<T> other)
		{
			return this.GetHandle() == other.GetHandle();
		}

		// Token: 0x04000E2D RID: 3629
		private PlayableHandle m_Handle;

		// Token: 0x04000E2E RID: 3630
		private static readonly ScriptPlayable<T> m_NullPlayable = new ScriptPlayable<T>(PlayableHandle.Null);
	}
}
