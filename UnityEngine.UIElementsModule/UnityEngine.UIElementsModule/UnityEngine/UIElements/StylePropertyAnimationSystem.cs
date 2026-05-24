using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine.Assertions;
using UnityEngine.Pool;
using UnityEngine.UIElements.StyleSheets;

namespace UnityEngine.UIElements
{
	// Token: 0x02000096 RID: 150
	internal class StylePropertyAnimationSystem : IStylePropertyAnimationSystem
	{
		// Token: 0x06000527 RID: 1319 RVA: 0x000133EF File Offset: 0x000115EF
		public StylePropertyAnimationSystem()
		{
			this.m_CurrentTimeMs = Panel.TimeSinceStartupMs();
		}

		// Token: 0x06000528 RID: 1320 RVA: 0x00013424 File Offset: 0x00011624
		private T GetOrCreate<T>(ref T values) where T : new()
		{
			T t = values;
			return (t != null) ? t : (values = new T());
		}

		// Token: 0x06000529 RID: 1321 RVA: 0x00013458 File Offset: 0x00011658
		private bool StartTransition<T>(VisualElement owner, StylePropertyId prop, T startValue, T endValue, int durationMs, int delayMs, Func<float, float> easingCurve, StylePropertyAnimationSystem.Values<T> values)
		{
			this.m_PropertyToValues[prop] = values;
			bool flag = values.StartTransition(owner, prop, startValue, endValue, durationMs, delayMs, easingCurve, this.CurrentTimeMs());
			this.UpdateTracking<T>(values);
			return flag;
		}

		// Token: 0x0600052A RID: 1322 RVA: 0x0001349C File Offset: 0x0001169C
		public bool StartTransition(VisualElement owner, StylePropertyId prop, float startValue, float endValue, int durationMs, int delayMs, [NotNull] Func<float, float> easingCurve)
		{
			return this.StartTransition<float>(owner, prop, startValue, endValue, durationMs, delayMs, easingCurve, this.GetOrCreate<StylePropertyAnimationSystem.ValuesFloat>(ref this.m_Floats));
		}

		// Token: 0x0600052B RID: 1323 RVA: 0x000134CC File Offset: 0x000116CC
		public bool StartTransition(VisualElement owner, StylePropertyId prop, int startValue, int endValue, int durationMs, int delayMs, [NotNull] Func<float, float> easingCurve)
		{
			return this.StartTransition<int>(owner, prop, startValue, endValue, durationMs, delayMs, easingCurve, this.GetOrCreate<StylePropertyAnimationSystem.ValuesInt>(ref this.m_Ints));
		}

		// Token: 0x0600052C RID: 1324 RVA: 0x000134FC File Offset: 0x000116FC
		public bool StartTransition(VisualElement owner, StylePropertyId prop, Length startValue, Length endValue, int durationMs, int delayMs, [NotNull] Func<float, float> easingCurve)
		{
			return this.StartTransition<Length>(owner, prop, startValue, endValue, durationMs, delayMs, easingCurve, this.GetOrCreate<StylePropertyAnimationSystem.ValuesLength>(ref this.m_Lengths));
		}

		// Token: 0x0600052D RID: 1325 RVA: 0x0001352C File Offset: 0x0001172C
		public bool StartTransition(VisualElement owner, StylePropertyId prop, Color startValue, Color endValue, int durationMs, int delayMs, [NotNull] Func<float, float> easingCurve)
		{
			return this.StartTransition<Color>(owner, prop, startValue, endValue, durationMs, delayMs, easingCurve, this.GetOrCreate<StylePropertyAnimationSystem.ValuesColor>(ref this.m_Colors));
		}

		// Token: 0x0600052E RID: 1326 RVA: 0x0001355C File Offset: 0x0001175C
		public bool StartAnimationEnum(VisualElement owner, StylePropertyId prop, int startValue, int endValue, int durationMs, int delayMs, [NotNull] Func<float, float> easingCurve)
		{
			return this.StartTransition<int>(owner, prop, startValue, endValue, durationMs, delayMs, easingCurve, this.GetOrCreate<StylePropertyAnimationSystem.ValuesEnum>(ref this.m_Enums));
		}

		// Token: 0x0600052F RID: 1327 RVA: 0x0001358C File Offset: 0x0001178C
		public bool StartTransition(VisualElement owner, StylePropertyId prop, Background startValue, Background endValue, int durationMs, int delayMs, [NotNull] Func<float, float> easingCurve)
		{
			return this.StartTransition<Background>(owner, prop, startValue, endValue, durationMs, delayMs, easingCurve, this.GetOrCreate<StylePropertyAnimationSystem.ValuesBackground>(ref this.m_Backgrounds));
		}

		// Token: 0x06000530 RID: 1328 RVA: 0x000135BC File Offset: 0x000117BC
		public bool StartTransition(VisualElement owner, StylePropertyId prop, FontDefinition startValue, FontDefinition endValue, int durationMs, int delayMs, [NotNull] Func<float, float> easingCurve)
		{
			return this.StartTransition<FontDefinition>(owner, prop, startValue, endValue, durationMs, delayMs, easingCurve, this.GetOrCreate<StylePropertyAnimationSystem.ValuesFontDefinition>(ref this.m_FontDefinitions));
		}

		// Token: 0x06000531 RID: 1329 RVA: 0x000135EC File Offset: 0x000117EC
		public bool StartTransition(VisualElement owner, StylePropertyId prop, Font startValue, Font endValue, int durationMs, int delayMs, [NotNull] Func<float, float> easingCurve)
		{
			return this.StartTransition<Font>(owner, prop, startValue, endValue, durationMs, delayMs, easingCurve, this.GetOrCreate<StylePropertyAnimationSystem.ValuesFont>(ref this.m_Fonts));
		}

		// Token: 0x06000532 RID: 1330 RVA: 0x0001361C File Offset: 0x0001181C
		public bool StartTransition(VisualElement owner, StylePropertyId prop, TextShadow startValue, TextShadow endValue, int durationMs, int delayMs, [NotNull] Func<float, float> easingCurve)
		{
			return this.StartTransition<TextShadow>(owner, prop, startValue, endValue, durationMs, delayMs, easingCurve, this.GetOrCreate<StylePropertyAnimationSystem.ValuesTextShadow>(ref this.m_TextShadows));
		}

		// Token: 0x06000533 RID: 1331 RVA: 0x0001364C File Offset: 0x0001184C
		public bool StartTransition(VisualElement owner, StylePropertyId prop, Scale startValue, Scale endValue, int durationMs, int delayMs, [NotNull] Func<float, float> easingCurve)
		{
			return this.StartTransition<Scale>(owner, prop, startValue, endValue, durationMs, delayMs, easingCurve, this.GetOrCreate<StylePropertyAnimationSystem.ValuesScale>(ref this.m_Scale));
		}

		// Token: 0x06000534 RID: 1332 RVA: 0x0001367C File Offset: 0x0001187C
		public bool StartTransition(VisualElement owner, StylePropertyId prop, Rotate startValue, Rotate endValue, int durationMs, int delayMs, [NotNull] Func<float, float> easingCurve)
		{
			return this.StartTransition<Rotate>(owner, prop, startValue, endValue, durationMs, delayMs, easingCurve, this.GetOrCreate<StylePropertyAnimationSystem.ValuesRotate>(ref this.m_Rotate));
		}

		// Token: 0x06000535 RID: 1333 RVA: 0x000136AC File Offset: 0x000118AC
		public bool StartTransition(VisualElement owner, StylePropertyId prop, Translate startValue, Translate endValue, int durationMs, int delayMs, [NotNull] Func<float, float> easingCurve)
		{
			return this.StartTransition<Translate>(owner, prop, startValue, endValue, durationMs, delayMs, easingCurve, this.GetOrCreate<StylePropertyAnimationSystem.ValuesTranslate>(ref this.m_Translate));
		}

		// Token: 0x06000536 RID: 1334 RVA: 0x000136DC File Offset: 0x000118DC
		public bool StartTransition(VisualElement owner, StylePropertyId prop, TransformOrigin startValue, TransformOrigin endValue, int durationMs, int delayMs, [NotNull] Func<float, float> easingCurve)
		{
			return this.StartTransition<TransformOrigin>(owner, prop, startValue, endValue, durationMs, delayMs, easingCurve, this.GetOrCreate<StylePropertyAnimationSystem.ValuesTransformOrigin>(ref this.m_TransformOrigin));
		}

		// Token: 0x06000537 RID: 1335 RVA: 0x0001370C File Offset: 0x0001190C
		public void CancelAllAnimations()
		{
			foreach (StylePropertyAnimationSystem.Values values in this.m_AllValues)
			{
				values.CancelAllAnimations();
			}
		}

		// Token: 0x06000538 RID: 1336 RVA: 0x00013764 File Offset: 0x00011964
		public void CancelAllAnimations(VisualElement owner)
		{
			foreach (StylePropertyAnimationSystem.Values values in this.m_AllValues)
			{
				values.CancelAllAnimations(owner);
			}
			Assert.AreEqual(0, owner.styleAnimation.runningAnimationCount);
			Assert.AreEqual(0, owner.styleAnimation.completedAnimationCount);
		}

		// Token: 0x06000539 RID: 1337 RVA: 0x000137E4 File Offset: 0x000119E4
		public void CancelAnimation(VisualElement owner, StylePropertyId id)
		{
			StylePropertyAnimationSystem.Values values;
			bool flag = this.m_PropertyToValues.TryGetValue(id, ref values);
			if (flag)
			{
				values.CancelAnimation(owner, id);
			}
		}

		// Token: 0x0600053A RID: 1338 RVA: 0x00013810 File Offset: 0x00011A10
		public bool HasRunningAnimation(VisualElement owner, StylePropertyId id)
		{
			StylePropertyAnimationSystem.Values values;
			return this.m_PropertyToValues.TryGetValue(id, ref values) && values.HasRunningAnimation(owner, id);
		}

		// Token: 0x0600053B RID: 1339 RVA: 0x00013840 File Offset: 0x00011A40
		public void UpdateAnimation(VisualElement owner, StylePropertyId id)
		{
			StylePropertyAnimationSystem.Values values;
			bool flag = this.m_PropertyToValues.TryGetValue(id, ref values);
			if (flag)
			{
				values.UpdateAnimation(owner, id);
			}
		}

		// Token: 0x0600053C RID: 1340 RVA: 0x0001386C File Offset: 0x00011A6C
		public void GetAllAnimations(VisualElement owner, List<StylePropertyId> propertyIds)
		{
			foreach (StylePropertyAnimationSystem.Values values in this.m_AllValues)
			{
				values.GetAllAnimations(owner, propertyIds);
			}
		}

		// Token: 0x0600053D RID: 1341 RVA: 0x000138C4 File Offset: 0x00011AC4
		private void UpdateTracking<T>(StylePropertyAnimationSystem.Values<T> values)
		{
			bool flag = !values.isEmpty && !this.m_AllValues.Contains(values);
			if (flag)
			{
				this.m_AllValues.Add(values);
			}
		}

		// Token: 0x0600053E RID: 1342 RVA: 0x00013900 File Offset: 0x00011B00
		private long CurrentTimeMs()
		{
			return this.m_CurrentTimeMs;
		}

		// Token: 0x0600053F RID: 1343 RVA: 0x00013918 File Offset: 0x00011B18
		public void Update()
		{
			this.m_CurrentTimeMs = Panel.TimeSinceStartupMs();
			int count = this.m_AllValues.Count;
			for (int i = 0; i < count; i++)
			{
				this.m_AllValues[i].Update(this.m_CurrentTimeMs);
			}
		}

		// Token: 0x04000218 RID: 536
		private long m_CurrentTimeMs = 0L;

		// Token: 0x04000219 RID: 537
		private StylePropertyAnimationSystem.ValuesFloat m_Floats;

		// Token: 0x0400021A RID: 538
		private StylePropertyAnimationSystem.ValuesInt m_Ints;

		// Token: 0x0400021B RID: 539
		private StylePropertyAnimationSystem.ValuesLength m_Lengths;

		// Token: 0x0400021C RID: 540
		private StylePropertyAnimationSystem.ValuesColor m_Colors;

		// Token: 0x0400021D RID: 541
		private StylePropertyAnimationSystem.ValuesEnum m_Enums;

		// Token: 0x0400021E RID: 542
		private StylePropertyAnimationSystem.ValuesBackground m_Backgrounds;

		// Token: 0x0400021F RID: 543
		private StylePropertyAnimationSystem.ValuesFontDefinition m_FontDefinitions;

		// Token: 0x04000220 RID: 544
		private StylePropertyAnimationSystem.ValuesFont m_Fonts;

		// Token: 0x04000221 RID: 545
		private StylePropertyAnimationSystem.ValuesTextShadow m_TextShadows;

		// Token: 0x04000222 RID: 546
		private StylePropertyAnimationSystem.ValuesScale m_Scale;

		// Token: 0x04000223 RID: 547
		private StylePropertyAnimationSystem.ValuesRotate m_Rotate;

		// Token: 0x04000224 RID: 548
		private StylePropertyAnimationSystem.ValuesTranslate m_Translate;

		// Token: 0x04000225 RID: 549
		private StylePropertyAnimationSystem.ValuesTransformOrigin m_TransformOrigin;

		// Token: 0x04000226 RID: 550
		private readonly List<StylePropertyAnimationSystem.Values> m_AllValues = new List<StylePropertyAnimationSystem.Values>();

		// Token: 0x04000227 RID: 551
		private readonly Dictionary<StylePropertyId, StylePropertyAnimationSystem.Values> m_PropertyToValues = new Dictionary<StylePropertyId, StylePropertyAnimationSystem.Values>();

		// Token: 0x02000097 RID: 151
		[Flags]
		private enum TransitionState
		{
			// Token: 0x04000229 RID: 553
			None = 0,
			// Token: 0x0400022A RID: 554
			Running = 1,
			// Token: 0x0400022B RID: 555
			Started = 2,
			// Token: 0x0400022C RID: 556
			Ended = 4,
			// Token: 0x0400022D RID: 557
			Canceled = 8
		}

		// Token: 0x02000098 RID: 152
		private struct AnimationDataSet<TTimingData, TStyleData>
		{
			// Token: 0x1700014F RID: 335
			// (get) Token: 0x06000540 RID: 1344 RVA: 0x00013967 File Offset: 0x00011B67
			// (set) Token: 0x06000541 RID: 1345 RVA: 0x00013971 File Offset: 0x00011B71
			private int capacity
			{
				get
				{
					return this.elements.Length;
				}
				set
				{
					Array.Resize<VisualElement>(ref this.elements, value);
					Array.Resize<StylePropertyId>(ref this.properties, value);
					Array.Resize<TTimingData>(ref this.timing, value);
					Array.Resize<TStyleData>(ref this.style, value);
				}
			}

			// Token: 0x06000542 RID: 1346 RVA: 0x000139A8 File Offset: 0x00011BA8
			private void LocalInit()
			{
				this.elements = new VisualElement[2];
				this.properties = new StylePropertyId[2];
				this.timing = new TTimingData[2];
				this.style = new TStyleData[2];
				this.indices = new Dictionary<StylePropertyAnimationSystem.ElementPropertyPair, int>(StylePropertyAnimationSystem.ElementPropertyPair.Comparer);
			}

			// Token: 0x06000543 RID: 1347 RVA: 0x000139F8 File Offset: 0x00011BF8
			public static StylePropertyAnimationSystem.AnimationDataSet<TTimingData, TStyleData> Create()
			{
				StylePropertyAnimationSystem.AnimationDataSet<TTimingData, TStyleData> animationDataSet = default(StylePropertyAnimationSystem.AnimationDataSet<TTimingData, TStyleData>);
				animationDataSet.LocalInit();
				return animationDataSet;
			}

			// Token: 0x06000544 RID: 1348 RVA: 0x00013A1C File Offset: 0x00011C1C
			public bool IndexOf(VisualElement ve, StylePropertyId prop, out int index)
			{
				return this.indices.TryGetValue(new StylePropertyAnimationSystem.ElementPropertyPair(ve, prop), ref index);
			}

			// Token: 0x06000545 RID: 1349 RVA: 0x00013A44 File Offset: 0x00011C44
			public void Add(VisualElement owner, StylePropertyId prop, TTimingData timingData, TStyleData styleData)
			{
				bool flag = this.count >= this.capacity;
				if (flag)
				{
					this.capacity *= 2;
				}
				int num = this.count;
				this.count = num + 1;
				int num2 = num;
				this.elements[num2] = owner;
				this.properties[num2] = prop;
				this.timing[num2] = timingData;
				this.style[num2] = styleData;
				this.indices.Add(new StylePropertyAnimationSystem.ElementPropertyPair(owner, prop), num2);
			}

			// Token: 0x06000546 RID: 1350 RVA: 0x00013ACC File Offset: 0x00011CCC
			public void Remove(int cancelledIndex)
			{
				int num = this.count - 1;
				this.count = num;
				int num2 = num;
				this.indices.Remove(new StylePropertyAnimationSystem.ElementPropertyPair(this.elements[cancelledIndex], this.properties[cancelledIndex]));
				bool flag = cancelledIndex != num2;
				if (flag)
				{
					VisualElement visualElement = (this.elements[cancelledIndex] = this.elements[num2]);
					StylePropertyId stylePropertyId = (this.properties[cancelledIndex] = this.properties[num2]);
					this.timing[cancelledIndex] = this.timing[num2];
					this.style[cancelledIndex] = this.style[num2];
					this.indices[new StylePropertyAnimationSystem.ElementPropertyPair(visualElement, stylePropertyId)] = cancelledIndex;
				}
				this.elements[num2] = null;
				this.properties[num2] = StylePropertyId.Unknown;
				this.timing[num2] = default(TTimingData);
				this.style[num2] = default(TStyleData);
			}

			// Token: 0x06000547 RID: 1351 RVA: 0x00013BCA File Offset: 0x00011DCA
			public void Replace(int index, TTimingData timingData, TStyleData styleData)
			{
				this.timing[index] = timingData;
				this.style[index] = styleData;
			}

			// Token: 0x06000548 RID: 1352 RVA: 0x00013BE8 File Offset: 0x00011DE8
			public void RemoveAll(VisualElement ve)
			{
				int num = this.count;
				for (int i = num - 1; i >= 0; i--)
				{
					bool flag = this.elements[i] == ve;
					if (flag)
					{
						this.Remove(i);
					}
				}
			}

			// Token: 0x06000549 RID: 1353 RVA: 0x00013C2C File Offset: 0x00011E2C
			public void RemoveAll()
			{
				this.capacity = 2;
				int num = Mathf.Min(this.count, this.capacity);
				Array.Clear(this.elements, 0, num);
				Array.Clear(this.properties, 0, num);
				Array.Clear(this.timing, 0, num);
				Array.Clear(this.style, 0, num);
				this.count = 0;
				this.indices.Clear();
			}

			// Token: 0x0600054A RID: 1354 RVA: 0x00013CA0 File Offset: 0x00011EA0
			public void GetActivePropertiesForElement(VisualElement ve, List<StylePropertyId> outProperties)
			{
				int num = this.count;
				for (int i = num - 1; i >= 0; i--)
				{
					bool flag = this.elements[i] == ve;
					if (flag)
					{
						outProperties.Add(this.properties[i]);
					}
				}
			}

			// Token: 0x0400022E RID: 558
			private const int InitialSize = 2;

			// Token: 0x0400022F RID: 559
			public VisualElement[] elements;

			// Token: 0x04000230 RID: 560
			public StylePropertyId[] properties;

			// Token: 0x04000231 RID: 561
			public TTimingData[] timing;

			// Token: 0x04000232 RID: 562
			public TStyleData[] style;

			// Token: 0x04000233 RID: 563
			public int count;

			// Token: 0x04000234 RID: 564
			private Dictionary<StylePropertyAnimationSystem.ElementPropertyPair, int> indices;
		}

		// Token: 0x02000099 RID: 153
		private struct ElementPropertyPair
		{
			// Token: 0x0600054B RID: 1355 RVA: 0x00013CEA File Offset: 0x00011EEA
			public ElementPropertyPair(VisualElement element, StylePropertyId property)
			{
				this.element = element;
				this.property = property;
			}

			// Token: 0x04000235 RID: 565
			public static readonly IEqualityComparer<StylePropertyAnimationSystem.ElementPropertyPair> Comparer = new StylePropertyAnimationSystem.ElementPropertyPair.EqualityComparer();

			// Token: 0x04000236 RID: 566
			public readonly VisualElement element;

			// Token: 0x04000237 RID: 567
			public readonly StylePropertyId property;

			// Token: 0x0200009A RID: 154
			private class EqualityComparer : IEqualityComparer<StylePropertyAnimationSystem.ElementPropertyPair>
			{
				// Token: 0x0600054D RID: 1357 RVA: 0x00013D08 File Offset: 0x00011F08
				public bool Equals(StylePropertyAnimationSystem.ElementPropertyPair x, StylePropertyAnimationSystem.ElementPropertyPair y)
				{
					return x.element == y.element && x.property == y.property;
				}

				// Token: 0x0600054E RID: 1358 RVA: 0x00013D3C File Offset: 0x00011F3C
				public int GetHashCode(StylePropertyAnimationSystem.ElementPropertyPair obj)
				{
					return (obj.element.GetHashCode() * 397) ^ (int)obj.property;
				}
			}
		}

		// Token: 0x0200009B RID: 155
		private abstract class Values
		{
			// Token: 0x06000550 RID: 1360
			public abstract void CancelAllAnimations();

			// Token: 0x06000551 RID: 1361
			public abstract void CancelAllAnimations(VisualElement ve);

			// Token: 0x06000552 RID: 1362
			public abstract void CancelAnimation(VisualElement ve, StylePropertyId id);

			// Token: 0x06000553 RID: 1363
			public abstract bool HasRunningAnimation(VisualElement ve, StylePropertyId id);

			// Token: 0x06000554 RID: 1364
			public abstract void UpdateAnimation(VisualElement ve, StylePropertyId id);

			// Token: 0x06000555 RID: 1365
			public abstract void GetAllAnimations(VisualElement ve, List<StylePropertyId> outPropertyIds);

			// Token: 0x06000556 RID: 1366
			public abstract void Update(long currentTimeMs);

			// Token: 0x06000557 RID: 1367
			protected abstract void UpdateValues();

			// Token: 0x06000558 RID: 1368
			protected abstract void UpdateComputedStyle();

			// Token: 0x06000559 RID: 1369
			protected abstract void UpdateComputedStyle(int i);
		}

		// Token: 0x0200009C RID: 156
		private abstract class Values<T> : StylePropertyAnimationSystem.Values
		{
			// Token: 0x17000150 RID: 336
			// (get) Token: 0x0600055B RID: 1371 RVA: 0x00013D67 File Offset: 0x00011F67
			public bool isEmpty
			{
				get
				{
					return this.running.count + this.completed.count == 0;
				}
			}

			// Token: 0x17000151 RID: 337
			// (get) Token: 0x0600055C RID: 1372
			public abstract Func<T, T, bool> SameFunc { get; }

			// Token: 0x0600055D RID: 1373 RVA: 0x00013D84 File Offset: 0x00011F84
			protected Values()
			{
				this.running = StylePropertyAnimationSystem.AnimationDataSet<StylePropertyAnimationSystem.Values<T>.TimingData, StylePropertyAnimationSystem.Values<T>.StyleData>.Create();
				this.completed = StylePropertyAnimationSystem.AnimationDataSet<StylePropertyAnimationSystem.Values<T>.EmptyData, T>.Create();
				this.m_CurrentTimeMs = Panel.TimeSinceStartupMs();
			}

			// Token: 0x0600055E RID: 1374 RVA: 0x00013DD8 File Offset: 0x00011FD8
			private void SwapFrameStates()
			{
				StylePropertyAnimationSystem.Values<T>.TransitionEventsFrameState currentFrameEventsState = this.m_CurrentFrameEventsState;
				this.m_CurrentFrameEventsState = this.m_NextFrameEventsState;
				this.m_NextFrameEventsState = currentFrameEventsState;
			}

			// Token: 0x0600055F RID: 1375 RVA: 0x00013E00 File Offset: 0x00012000
			private void QueueEvent(EventBase evt, StylePropertyAnimationSystem.ElementPropertyPair epp)
			{
				evt.target = epp.element;
				Queue<EventBase> pooledQueue;
				bool flag = !this.m_NextFrameEventsState.elementPropertyQueuedEvents.TryGetValue(epp, ref pooledQueue);
				if (flag)
				{
					pooledQueue = StylePropertyAnimationSystem.Values<T>.TransitionEventsFrameState.GetPooledQueue();
					this.m_NextFrameEventsState.elementPropertyQueuedEvents.Add(epp, pooledQueue);
				}
				pooledQueue.Enqueue(evt);
				bool flag2 = this.m_NextFrameEventsState.panel == null;
				if (flag2)
				{
					this.m_NextFrameEventsState.panel = epp.element.panel;
				}
				this.m_NextFrameEventsState.RegisterChange();
			}

			// Token: 0x06000560 RID: 1376 RVA: 0x00013E8C File Offset: 0x0001208C
			private void ClearEventQueue(StylePropertyAnimationSystem.ElementPropertyPair epp)
			{
				Queue<EventBase> queue;
				bool flag = this.m_NextFrameEventsState.elementPropertyQueuedEvents.TryGetValue(epp, ref queue);
				if (flag)
				{
					while (queue.Count > 0)
					{
						queue.Dequeue().Dispose();
						this.m_NextFrameEventsState.UnregisterChange();
					}
				}
			}

			// Token: 0x06000561 RID: 1377 RVA: 0x00013EDC File Offset: 0x000120DC
			private void QueueTransitionRunEvent(VisualElement ve, int runningIndex)
			{
				ref StylePropertyAnimationSystem.Values<T>.TimingData ptr = ref this.running.timing[runningIndex];
				StylePropertyId stylePropertyId = this.running.properties[runningIndex];
				int num = ((ptr.delayMs < 0) ? Mathf.Min(Mathf.Max(-ptr.delayMs, 0), ptr.durationMs) : 0);
				StylePropertyAnimationSystem.ElementPropertyPair elementPropertyPair = new StylePropertyAnimationSystem.ElementPropertyPair(ve, stylePropertyId);
				TransitionRunEvent pooled = TransitionEventBase<TransitionRunEvent>.GetPooled(new StylePropertyName(stylePropertyId), (double)((float)num / 1000f));
				bool flag = this.m_NextFrameEventsState.elementPropertyStateDelta.ContainsKey(elementPropertyPair);
				if (flag)
				{
					Dictionary<StylePropertyAnimationSystem.ElementPropertyPair, StylePropertyAnimationSystem.TransitionState> elementPropertyStateDelta = this.m_NextFrameEventsState.elementPropertyStateDelta;
					StylePropertyAnimationSystem.ElementPropertyPair elementPropertyPair2 = elementPropertyPair;
					elementPropertyStateDelta[elementPropertyPair2] |= StylePropertyAnimationSystem.TransitionState.Running;
				}
				else
				{
					this.m_NextFrameEventsState.elementPropertyStateDelta.Add(elementPropertyPair, StylePropertyAnimationSystem.TransitionState.Running);
				}
				this.QueueEvent(pooled, elementPropertyPair);
			}

			// Token: 0x06000562 RID: 1378 RVA: 0x00013FAC File Offset: 0x000121AC
			private void QueueTransitionStartEvent(VisualElement ve, int runningIndex)
			{
				ref StylePropertyAnimationSystem.Values<T>.TimingData ptr = ref this.running.timing[runningIndex];
				StylePropertyId stylePropertyId = this.running.properties[runningIndex];
				int num = ((ptr.delayMs < 0) ? Mathf.Min(Mathf.Max(-ptr.delayMs, 0), ptr.durationMs) : 0);
				StylePropertyAnimationSystem.ElementPropertyPair elementPropertyPair = new StylePropertyAnimationSystem.ElementPropertyPair(ve, stylePropertyId);
				TransitionStartEvent pooled = TransitionEventBase<TransitionStartEvent>.GetPooled(new StylePropertyName(stylePropertyId), (double)((float)num / 1000f));
				bool flag = this.m_NextFrameEventsState.elementPropertyStateDelta.ContainsKey(elementPropertyPair);
				if (flag)
				{
					Dictionary<StylePropertyAnimationSystem.ElementPropertyPair, StylePropertyAnimationSystem.TransitionState> elementPropertyStateDelta = this.m_NextFrameEventsState.elementPropertyStateDelta;
					StylePropertyAnimationSystem.ElementPropertyPair elementPropertyPair2 = elementPropertyPair;
					elementPropertyStateDelta[elementPropertyPair2] |= StylePropertyAnimationSystem.TransitionState.Started;
				}
				else
				{
					this.m_NextFrameEventsState.elementPropertyStateDelta.Add(elementPropertyPair, StylePropertyAnimationSystem.TransitionState.Started);
				}
				this.QueueEvent(pooled, elementPropertyPair);
			}

			// Token: 0x06000563 RID: 1379 RVA: 0x0001407C File Offset: 0x0001227C
			private void QueueTransitionEndEvent(VisualElement ve, int runningIndex)
			{
				ref StylePropertyAnimationSystem.Values<T>.TimingData ptr = ref this.running.timing[runningIndex];
				StylePropertyId stylePropertyId = this.running.properties[runningIndex];
				StylePropertyAnimationSystem.ElementPropertyPair elementPropertyPair = new StylePropertyAnimationSystem.ElementPropertyPair(ve, stylePropertyId);
				TransitionEndEvent pooled = TransitionEventBase<TransitionEndEvent>.GetPooled(new StylePropertyName(stylePropertyId), (double)((float)ptr.durationMs / 1000f));
				bool flag = this.m_NextFrameEventsState.elementPropertyStateDelta.ContainsKey(elementPropertyPair);
				if (flag)
				{
					Dictionary<StylePropertyAnimationSystem.ElementPropertyPair, StylePropertyAnimationSystem.TransitionState> elementPropertyStateDelta = this.m_NextFrameEventsState.elementPropertyStateDelta;
					StylePropertyAnimationSystem.ElementPropertyPair elementPropertyPair2 = elementPropertyPair;
					elementPropertyStateDelta[elementPropertyPair2] |= StylePropertyAnimationSystem.TransitionState.Ended;
				}
				else
				{
					this.m_NextFrameEventsState.elementPropertyStateDelta.Add(elementPropertyPair, StylePropertyAnimationSystem.TransitionState.Ended);
				}
				this.QueueEvent(pooled, elementPropertyPair);
			}

			// Token: 0x06000564 RID: 1380 RVA: 0x00014128 File Offset: 0x00012328
			private void QueueTransitionCancelEvent(VisualElement ve, int runningIndex, long panelElapsedMs)
			{
				ref StylePropertyAnimationSystem.Values<T>.TimingData ptr = ref this.running.timing[runningIndex];
				StylePropertyId stylePropertyId = this.running.properties[runningIndex];
				long num = (ptr.isStarted ? (panelElapsedMs - ptr.startTimeMs) : 0L);
				StylePropertyAnimationSystem.ElementPropertyPair elementPropertyPair = new StylePropertyAnimationSystem.ElementPropertyPair(ve, stylePropertyId);
				bool flag = ptr.delayMs < 0;
				if (flag)
				{
					num = (long)(-(long)ptr.delayMs) + num;
				}
				TransitionCancelEvent pooled = TransitionEventBase<TransitionCancelEvent>.GetPooled(new StylePropertyName(stylePropertyId), (double)((float)num / 1000f));
				bool flag2 = this.m_NextFrameEventsState.elementPropertyStateDelta.ContainsKey(elementPropertyPair);
				if (flag2)
				{
					bool flag3 = this.m_NextFrameEventsState.elementPropertyStateDelta[elementPropertyPair] == StylePropertyAnimationSystem.TransitionState.None || (this.m_NextFrameEventsState.elementPropertyStateDelta[elementPropertyPair] & StylePropertyAnimationSystem.TransitionState.Canceled) == StylePropertyAnimationSystem.TransitionState.Canceled;
					if (flag3)
					{
						this.m_NextFrameEventsState.elementPropertyStateDelta[elementPropertyPair] = StylePropertyAnimationSystem.TransitionState.Canceled;
						this.ClearEventQueue(elementPropertyPair);
						this.QueueEvent(pooled, elementPropertyPair);
					}
					else
					{
						this.m_NextFrameEventsState.elementPropertyStateDelta[elementPropertyPair] = StylePropertyAnimationSystem.TransitionState.None;
						this.ClearEventQueue(elementPropertyPair);
					}
				}
				else
				{
					this.m_NextFrameEventsState.elementPropertyStateDelta.Add(elementPropertyPair, StylePropertyAnimationSystem.TransitionState.Canceled);
					this.QueueEvent(pooled, elementPropertyPair);
				}
			}

			// Token: 0x06000565 RID: 1381 RVA: 0x00014258 File Offset: 0x00012458
			private void SendTransitionCancelEvent(VisualElement ve, int runningIndex, long panelElapsedMs)
			{
				ref StylePropertyAnimationSystem.Values<T>.TimingData ptr = ref this.running.timing[runningIndex];
				StylePropertyId stylePropertyId = this.running.properties[runningIndex];
				long num = (ptr.isStarted ? (panelElapsedMs - ptr.startTimeMs) : 0L);
				bool flag = ptr.delayMs < 0;
				if (flag)
				{
					num = (long)(-(long)ptr.delayMs) + num;
				}
				using (TransitionCancelEvent pooled = TransitionEventBase<TransitionCancelEvent>.GetPooled(new StylePropertyName(stylePropertyId), (double)((float)num / 1000f)))
				{
					pooled.target = ve;
					ve.SendEvent(pooled);
				}
			}

			// Token: 0x06000566 RID: 1382 RVA: 0x00014300 File Offset: 0x00012500
			public sealed override void CancelAllAnimations()
			{
				int count = this.running.count;
				bool flag = count > 0;
				if (flag)
				{
					using (new EventDispatcherGate(this.running.elements[0].panel.dispatcher))
					{
						for (int i = 0; i < count; i++)
						{
							VisualElement visualElement = this.running.elements[i];
							this.SendTransitionCancelEvent(visualElement, i, this.m_CurrentTimeMs);
							this.ForceComputedStyleEndValue(i);
							IStylePropertyAnimations styleAnimation = visualElement.styleAnimation;
							int num = styleAnimation.runningAnimationCount;
							styleAnimation.runningAnimationCount = num - 1;
						}
					}
					this.running.RemoveAll();
				}
				int count2 = this.completed.count;
				for (int j = 0; j < count2; j++)
				{
					VisualElement visualElement2 = this.completed.elements[j];
					IStylePropertyAnimations styleAnimation2 = visualElement2.styleAnimation;
					int num = styleAnimation2.completedAnimationCount;
					styleAnimation2.completedAnimationCount = num - 1;
				}
				this.completed.RemoveAll();
			}

			// Token: 0x06000567 RID: 1383 RVA: 0x00014428 File Offset: 0x00012628
			public sealed override void CancelAllAnimations(VisualElement ve)
			{
				int count = this.running.count;
				bool flag = count > 0;
				if (flag)
				{
					using (new EventDispatcherGate(this.running.elements[0].panel.dispatcher))
					{
						for (int i = 0; i < count; i++)
						{
							bool flag2 = this.running.elements[i] == ve;
							if (flag2)
							{
								this.SendTransitionCancelEvent(ve, i, this.m_CurrentTimeMs);
								this.ForceComputedStyleEndValue(i);
								IStylePropertyAnimations styleAnimation = this.running.elements[i].styleAnimation;
								int num = styleAnimation.runningAnimationCount;
								styleAnimation.runningAnimationCount = num - 1;
							}
						}
					}
				}
				this.running.RemoveAll(ve);
				int count2 = this.completed.count;
				for (int j = 0; j < count2; j++)
				{
					bool flag3 = this.completed.elements[j] == ve;
					if (flag3)
					{
						IStylePropertyAnimations styleAnimation2 = this.completed.elements[j].styleAnimation;
						int num = styleAnimation2.completedAnimationCount;
						styleAnimation2.completedAnimationCount = num - 1;
					}
				}
				this.completed.RemoveAll(ve);
			}

			// Token: 0x06000568 RID: 1384 RVA: 0x0001457C File Offset: 0x0001277C
			public sealed override void CancelAnimation(VisualElement ve, StylePropertyId id)
			{
				int num;
				bool flag = this.running.IndexOf(ve, id, out num);
				if (flag)
				{
					this.QueueTransitionCancelEvent(ve, num, this.m_CurrentTimeMs);
					this.ForceComputedStyleEndValue(num);
					this.running.Remove(num);
					IStylePropertyAnimations styleAnimation = ve.styleAnimation;
					int num2 = styleAnimation.runningAnimationCount;
					styleAnimation.runningAnimationCount = num2 - 1;
				}
				int num3;
				bool flag2 = this.completed.IndexOf(ve, id, out num3);
				if (flag2)
				{
					this.completed.Remove(num3);
					IStylePropertyAnimations styleAnimation2 = ve.styleAnimation;
					int num2 = styleAnimation2.completedAnimationCount;
					styleAnimation2.completedAnimationCount = num2 - 1;
				}
			}

			// Token: 0x06000569 RID: 1385 RVA: 0x00014614 File Offset: 0x00012814
			public sealed override bool HasRunningAnimation(VisualElement ve, StylePropertyId id)
			{
				int num;
				return this.running.IndexOf(ve, id, out num);
			}

			// Token: 0x0600056A RID: 1386 RVA: 0x00014638 File Offset: 0x00012838
			public sealed override void UpdateAnimation(VisualElement ve, StylePropertyId id)
			{
				int num;
				bool flag = this.running.IndexOf(ve, id, out num);
				if (flag)
				{
					this.UpdateComputedStyle(num);
				}
			}

			// Token: 0x0600056B RID: 1387 RVA: 0x00014661 File Offset: 0x00012861
			public sealed override void GetAllAnimations(VisualElement ve, List<StylePropertyId> outPropertyIds)
			{
				this.running.GetActivePropertiesForElement(ve, outPropertyIds);
				this.completed.GetActivePropertiesForElement(ve, outPropertyIds);
			}

			// Token: 0x0600056C RID: 1388 RVA: 0x00014680 File Offset: 0x00012880
			private float ComputeReversingShorteningFactor(int oldIndex)
			{
				ref StylePropertyAnimationSystem.Values<T>.TimingData ptr = ref this.running.timing[oldIndex];
				return Mathf.Clamp01(Mathf.Abs(1f - (1f - ptr.easedProgress) * ptr.reversingShorteningFactor));
			}

			// Token: 0x0600056D RID: 1389 RVA: 0x000146C8 File Offset: 0x000128C8
			private int ComputeReversingDuration(int newTransitionDurationMs, float newReversingShorteningFactor)
			{
				return Mathf.RoundToInt((float)newTransitionDurationMs * newReversingShorteningFactor);
			}

			// Token: 0x0600056E RID: 1390 RVA: 0x000146E4 File Offset: 0x000128E4
			private int ComputeReversingDelay(int delayMs, float newReversingShorteningFactor)
			{
				return (delayMs < 0) ? Mathf.RoundToInt((float)delayMs * newReversingShorteningFactor) : delayMs;
			}

			// Token: 0x0600056F RID: 1391 RVA: 0x00014708 File Offset: 0x00012908
			public bool StartTransition(VisualElement owner, StylePropertyId prop, T startValue, T endValue, int durationMs, int delayMs, Func<float, float> easingCurve, long currentTimeMs)
			{
				long num = currentTimeMs + (long)delayMs;
				StylePropertyAnimationSystem.Values<T>.TimingData timingData = new StylePropertyAnimationSystem.Values<T>.TimingData
				{
					startTimeMs = num,
					durationMs = durationMs,
					easingCurve = easingCurve,
					reversingShorteningFactor = 1f,
					delayMs = delayMs
				};
				StylePropertyAnimationSystem.Values<T>.StyleData styleData = new StylePropertyAnimationSystem.Values<T>.StyleData
				{
					startValue = startValue,
					endValue = endValue,
					currentValue = startValue,
					reversingAdjustedStartValue = startValue
				};
				int num2 = Mathf.Max(0, durationMs) + delayMs;
				int num3;
				bool flag = this.completed.IndexOf(owner, prop, out num3);
				if (flag)
				{
					bool flag2 = this.SameFunc.Invoke(endValue, this.completed.style[num3]);
					if (flag2)
					{
						return false;
					}
					bool flag3 = num2 <= 0;
					if (flag3)
					{
						return false;
					}
					this.completed.Remove(num3);
					IStylePropertyAnimations styleAnimation = owner.styleAnimation;
					int num4 = styleAnimation.completedAnimationCount;
					styleAnimation.completedAnimationCount = num4 - 1;
				}
				int num5;
				bool flag4 = this.running.IndexOf(owner, prop, out num5);
				bool flag6;
				if (flag4)
				{
					bool flag5 = this.SameFunc.Invoke(endValue, this.running.style[num5].endValue);
					if (flag5)
					{
						flag6 = false;
					}
					else
					{
						bool flag7 = this.SameFunc.Invoke(endValue, this.running.style[num5].currentValue);
						if (flag7)
						{
							this.QueueTransitionCancelEvent(owner, num5, currentTimeMs);
							this.running.Remove(num5);
							IStylePropertyAnimations styleAnimation2 = owner.styleAnimation;
							int num4 = styleAnimation2.runningAnimationCount;
							styleAnimation2.runningAnimationCount = num4 - 1;
							flag6 = false;
						}
						else
						{
							bool flag8 = num2 <= 0;
							if (flag8)
							{
								this.QueueTransitionCancelEvent(owner, num5, currentTimeMs);
								this.running.Remove(num5);
								IStylePropertyAnimations styleAnimation3 = owner.styleAnimation;
								int num4 = styleAnimation3.runningAnimationCount;
								styleAnimation3.runningAnimationCount = num4 - 1;
								flag6 = false;
							}
							else
							{
								styleData.startValue = this.running.style[num5].currentValue;
								styleData.currentValue = styleData.startValue;
								bool flag9 = this.SameFunc.Invoke(endValue, this.running.style[num5].startValue);
								if (flag9)
								{
									float num6 = (timingData.reversingShorteningFactor = this.ComputeReversingShorteningFactor(num5));
									timingData.startTimeMs = currentTimeMs + (long)this.ComputeReversingDelay(delayMs, num6);
									timingData.durationMs = this.ComputeReversingDuration(durationMs, num6);
									styleData.reversingAdjustedStartValue = this.running.style[num5].endValue;
								}
								this.running.timing[num5].isStarted = false;
								this.QueueTransitionCancelEvent(owner, num5, currentTimeMs);
								this.QueueTransitionRunEvent(owner, num5);
								this.running.Replace(num5, timingData, styleData);
								flag6 = true;
							}
						}
					}
				}
				else
				{
					bool flag10 = num2 <= 0;
					if (flag10)
					{
						flag6 = false;
					}
					else
					{
						bool flag11 = this.SameFunc.Invoke(startValue, endValue);
						if (flag11)
						{
							flag6 = false;
						}
						else
						{
							this.running.Add(owner, prop, timingData, styleData);
							IStylePropertyAnimations styleAnimation4 = owner.styleAnimation;
							int num4 = styleAnimation4.runningAnimationCount;
							styleAnimation4.runningAnimationCount = num4 + 1;
							this.QueueTransitionRunEvent(owner, this.running.count - 1);
							flag6 = true;
						}
					}
				}
				return flag6;
			}

			// Token: 0x06000570 RID: 1392 RVA: 0x00014A6C File Offset: 0x00012C6C
			private void ForceComputedStyleEndValue(int runningIndex)
			{
				ref StylePropertyAnimationSystem.Values<T>.StyleData ptr = ref this.running.style[runningIndex];
				ptr.currentValue = ptr.endValue;
				this.UpdateComputedStyle(runningIndex);
			}

			// Token: 0x06000571 RID: 1393 RVA: 0x00014AA0 File Offset: 0x00012CA0
			public sealed override void Update(long currentTimeMs)
			{
				this.m_CurrentTimeMs = currentTimeMs;
				this.UpdateProgress(currentTimeMs);
				this.UpdateValues();
				this.UpdateComputedStyle();
				bool flag = this.m_NextFrameEventsState.StateChanged();
				if (flag)
				{
					this.ProcessEventQueue();
				}
			}

			// Token: 0x06000572 RID: 1394 RVA: 0x00014AE4 File Offset: 0x00012CE4
			private void ProcessEventQueue()
			{
				this.SwapFrameStates();
				IPanel panel = this.m_CurrentFrameEventsState.panel;
				EventDispatcher eventDispatcher = ((panel != null) ? panel.dispatcher : null);
				using (new EventDispatcherGate(eventDispatcher))
				{
					foreach (KeyValuePair<StylePropertyAnimationSystem.ElementPropertyPair, Queue<EventBase>> keyValuePair in this.m_CurrentFrameEventsState.elementPropertyQueuedEvents)
					{
						StylePropertyAnimationSystem.ElementPropertyPair key = keyValuePair.Key;
						Queue<EventBase> value = keyValuePair.Value;
						VisualElement element = keyValuePair.Key.element;
						while (value.Count > 0)
						{
							EventBase eventBase = value.Dequeue();
							element.SendEvent(eventBase);
							eventBase.Dispose();
						}
					}
					this.m_CurrentFrameEventsState.Clear();
				}
			}

			// Token: 0x06000573 RID: 1395 RVA: 0x00014BDC File Offset: 0x00012DDC
			private void UpdateProgress(long currentTimeMs)
			{
				int num = this.running.count;
				bool flag = num > 0;
				if (flag)
				{
					for (int i = 0; i < num; i++)
					{
						ref StylePropertyAnimationSystem.Values<T>.TimingData ptr = ref this.running.timing[i];
						bool flag2 = currentTimeMs < ptr.startTimeMs;
						if (flag2)
						{
							ptr.easedProgress = 0f;
						}
						else
						{
							bool flag3 = currentTimeMs >= ptr.startTimeMs + (long)ptr.durationMs;
							if (flag3)
							{
								ref StylePropertyAnimationSystem.Values<T>.StyleData ptr2 = ref this.running.style[i];
								ref VisualElement ptr3 = ref this.running.elements[i];
								ptr2.currentValue = ptr2.endValue;
								this.UpdateComputedStyle(i);
								this.completed.Add(ptr3, this.running.properties[i], StylePropertyAnimationSystem.Values<T>.EmptyData.Default, ptr2.endValue);
								IStylePropertyAnimations styleAnimation = ptr3.styleAnimation;
								int num2 = styleAnimation.runningAnimationCount;
								styleAnimation.runningAnimationCount = num2 - 1;
								IStylePropertyAnimations styleAnimation2 = ptr3.styleAnimation;
								num2 = styleAnimation2.completedAnimationCount;
								styleAnimation2.completedAnimationCount = num2 + 1;
								this.QueueTransitionEndEvent(ptr3, i);
								this.running.Remove(i);
								i--;
								num--;
							}
							else
							{
								bool flag4 = !ptr.isStarted;
								if (flag4)
								{
									ptr.isStarted = true;
									this.QueueTransitionStartEvent(this.running.elements[i], i);
								}
								float num3 = (float)(currentTimeMs - ptr.startTimeMs) / (float)ptr.durationMs;
								ptr.easedProgress = ptr.easingCurve.Invoke(num3);
							}
						}
					}
				}
			}

			// Token: 0x04000238 RID: 568
			private long m_CurrentTimeMs = 0L;

			// Token: 0x04000239 RID: 569
			private StylePropertyAnimationSystem.Values<T>.TransitionEventsFrameState m_CurrentFrameEventsState = new StylePropertyAnimationSystem.Values<T>.TransitionEventsFrameState();

			// Token: 0x0400023A RID: 570
			private StylePropertyAnimationSystem.Values<T>.TransitionEventsFrameState m_NextFrameEventsState = new StylePropertyAnimationSystem.Values<T>.TransitionEventsFrameState();

			// Token: 0x0400023B RID: 571
			public StylePropertyAnimationSystem.AnimationDataSet<StylePropertyAnimationSystem.Values<T>.TimingData, StylePropertyAnimationSystem.Values<T>.StyleData> running;

			// Token: 0x0400023C RID: 572
			public StylePropertyAnimationSystem.AnimationDataSet<StylePropertyAnimationSystem.Values<T>.EmptyData, T> completed;

			// Token: 0x0200009D RID: 157
			private class TransitionEventsFrameState
			{
				// Token: 0x06000574 RID: 1396 RVA: 0x00014D80 File Offset: 0x00012F80
				public static Queue<EventBase> GetPooledQueue()
				{
					return StylePropertyAnimationSystem.Values<T>.TransitionEventsFrameState.k_EventQueuePool.Get();
				}

				// Token: 0x06000575 RID: 1397 RVA: 0x00014D9C File Offset: 0x00012F9C
				public void RegisterChange()
				{
					this.m_ChangesCount++;
				}

				// Token: 0x06000576 RID: 1398 RVA: 0x00014DAD File Offset: 0x00012FAD
				public void UnregisterChange()
				{
					this.m_ChangesCount--;
				}

				// Token: 0x06000577 RID: 1399 RVA: 0x00014DC0 File Offset: 0x00012FC0
				public bool StateChanged()
				{
					return this.m_ChangesCount > 0;
				}

				// Token: 0x06000578 RID: 1400 RVA: 0x00014DDC File Offset: 0x00012FDC
				public void Clear()
				{
					foreach (KeyValuePair<StylePropertyAnimationSystem.ElementPropertyPair, Queue<EventBase>> keyValuePair in this.elementPropertyQueuedEvents)
					{
						this.elementPropertyStateDelta[keyValuePair.Key] = StylePropertyAnimationSystem.TransitionState.None;
						keyValuePair.Value.Clear();
						StylePropertyAnimationSystem.Values<T>.TransitionEventsFrameState.k_EventQueuePool.Release(keyValuePair.Value);
					}
					this.elementPropertyQueuedEvents.Clear();
					this.panel = null;
					this.m_ChangesCount = 0;
				}

				// Token: 0x0400023D RID: 573
				private static readonly ObjectPool<Queue<EventBase>> k_EventQueuePool = new ObjectPool<Queue<EventBase>>(() => new Queue<EventBase>(4), null, null, null, true, 10, 10000);

				// Token: 0x0400023E RID: 574
				public readonly Dictionary<StylePropertyAnimationSystem.ElementPropertyPair, StylePropertyAnimationSystem.TransitionState> elementPropertyStateDelta = new Dictionary<StylePropertyAnimationSystem.ElementPropertyPair, StylePropertyAnimationSystem.TransitionState>(StylePropertyAnimationSystem.ElementPropertyPair.Comparer);

				// Token: 0x0400023F RID: 575
				public readonly Dictionary<StylePropertyAnimationSystem.ElementPropertyPair, Queue<EventBase>> elementPropertyQueuedEvents = new Dictionary<StylePropertyAnimationSystem.ElementPropertyPair, Queue<EventBase>>(StylePropertyAnimationSystem.ElementPropertyPair.Comparer);

				// Token: 0x04000240 RID: 576
				public IPanel panel;

				// Token: 0x04000241 RID: 577
				private int m_ChangesCount;
			}

			// Token: 0x0200009F RID: 159
			public struct TimingData
			{
				// Token: 0x04000243 RID: 579
				public long startTimeMs;

				// Token: 0x04000244 RID: 580
				public int durationMs;

				// Token: 0x04000245 RID: 581
				public Func<float, float> easingCurve;

				// Token: 0x04000246 RID: 582
				public float easedProgress;

				// Token: 0x04000247 RID: 583
				public float reversingShorteningFactor;

				// Token: 0x04000248 RID: 584
				public bool isStarted;

				// Token: 0x04000249 RID: 585
				public int delayMs;
			}

			// Token: 0x020000A0 RID: 160
			public struct StyleData
			{
				// Token: 0x0400024A RID: 586
				public T startValue;

				// Token: 0x0400024B RID: 587
				public T endValue;

				// Token: 0x0400024C RID: 588
				public T reversingAdjustedStartValue;

				// Token: 0x0400024D RID: 589
				public T currentValue;
			}

			// Token: 0x020000A1 RID: 161
			public struct EmptyData
			{
				// Token: 0x0400024E RID: 590
				public static StylePropertyAnimationSystem.Values<T>.EmptyData Default = default(StylePropertyAnimationSystem.Values<T>.EmptyData);
			}
		}

		// Token: 0x020000A2 RID: 162
		private class ValuesFloat : StylePropertyAnimationSystem.Values<float>
		{
			// Token: 0x17000152 RID: 338
			// (get) Token: 0x0600057F RID: 1407 RVA: 0x00014EED File Offset: 0x000130ED
			public override Func<float, float, bool> SameFunc { get; } = new Func<float, float, bool>(StylePropertyAnimationSystem.ValuesFloat.IsSame);

			// Token: 0x06000580 RID: 1408 RVA: 0x00014EF5 File Offset: 0x000130F5
			private static bool IsSame(float a, float b)
			{
				return Mathf.Approximately(a, b);
			}

			// Token: 0x06000581 RID: 1409 RVA: 0x00014EFE File Offset: 0x000130FE
			private static float Lerp(float a, float b, float t)
			{
				return Mathf.LerpUnclamped(a, b, t);
			}

			// Token: 0x06000582 RID: 1410 RVA: 0x00014F08 File Offset: 0x00013108
			protected sealed override void UpdateValues()
			{
				int count = this.running.count;
				for (int i = 0; i < count; i++)
				{
					ref StylePropertyAnimationSystem.Values<float>.TimingData ptr = ref this.running.timing[i];
					ref StylePropertyAnimationSystem.Values<float>.StyleData ptr2 = ref this.running.style[i];
					ptr2.currentValue = StylePropertyAnimationSystem.ValuesFloat.Lerp(ptr2.startValue, ptr2.endValue, ptr.easedProgress);
				}
			}

			// Token: 0x06000583 RID: 1411 RVA: 0x00014F78 File Offset: 0x00013178
			protected sealed override void UpdateComputedStyle()
			{
				int count = this.running.count;
				for (int i = 0; i < count; i++)
				{
					this.running.elements[i].computedStyle.ApplyPropertyAnimation(this.running.elements[i], this.running.properties[i], this.running.style[i].currentValue);
				}
			}

			// Token: 0x06000584 RID: 1412 RVA: 0x00014FEC File Offset: 0x000131EC
			protected sealed override void UpdateComputedStyle(int i)
			{
				this.running.elements[i].computedStyle.ApplyPropertyAnimation(this.running.elements[i], this.running.properties[i], this.running.style[i].currentValue);
			}
		}

		// Token: 0x020000A3 RID: 163
		private class ValuesInt : StylePropertyAnimationSystem.Values<int>
		{
			// Token: 0x17000153 RID: 339
			// (get) Token: 0x06000586 RID: 1414 RVA: 0x0001505D File Offset: 0x0001325D
			public override Func<int, int, bool> SameFunc { get; } = new Func<int, int, bool>(StylePropertyAnimationSystem.ValuesInt.IsSame);

			// Token: 0x06000587 RID: 1415 RVA: 0x00015065 File Offset: 0x00013265
			private static bool IsSame(int a, int b)
			{
				return a == b;
			}

			// Token: 0x06000588 RID: 1416 RVA: 0x0001506B File Offset: 0x0001326B
			private static int Lerp(int a, int b, float t)
			{
				return Mathf.RoundToInt(Mathf.LerpUnclamped((float)a, (float)b, t));
			}

			// Token: 0x06000589 RID: 1417 RVA: 0x0001507C File Offset: 0x0001327C
			protected sealed override void UpdateValues()
			{
				int count = this.running.count;
				for (int i = 0; i < count; i++)
				{
					ref StylePropertyAnimationSystem.Values<int>.TimingData ptr = ref this.running.timing[i];
					ref StylePropertyAnimationSystem.Values<int>.StyleData ptr2 = ref this.running.style[i];
					ptr2.currentValue = StylePropertyAnimationSystem.ValuesInt.Lerp(ptr2.startValue, ptr2.endValue, ptr.easedProgress);
				}
			}

			// Token: 0x0600058A RID: 1418 RVA: 0x000150EC File Offset: 0x000132EC
			protected sealed override void UpdateComputedStyle()
			{
				int count = this.running.count;
				for (int i = 0; i < count; i++)
				{
					this.running.elements[i].computedStyle.ApplyPropertyAnimation(this.running.elements[i], this.running.properties[i], this.running.style[i].currentValue);
				}
			}

			// Token: 0x0600058B RID: 1419 RVA: 0x00015160 File Offset: 0x00013360
			protected sealed override void UpdateComputedStyle(int i)
			{
				this.running.elements[i].computedStyle.ApplyPropertyAnimation(this.running.elements[i], this.running.properties[i], this.running.style[i].currentValue);
			}
		}

		// Token: 0x020000A4 RID: 164
		private class ValuesLength : StylePropertyAnimationSystem.Values<Length>
		{
			// Token: 0x17000154 RID: 340
			// (get) Token: 0x0600058D RID: 1421 RVA: 0x000151D1 File Offset: 0x000133D1
			public override Func<Length, Length, bool> SameFunc { get; } = new Func<Length, Length, bool>(StylePropertyAnimationSystem.ValuesLength.IsSame);

			// Token: 0x0600058E RID: 1422 RVA: 0x000151D9 File Offset: 0x000133D9
			private static bool IsSame(Length a, Length b)
			{
				return a.unit == b.unit && Mathf.Approximately(a.value, b.value);
			}

			// Token: 0x0600058F RID: 1423 RVA: 0x00015201 File Offset: 0x00013401
			internal static Length Lerp(Length a, Length b, float t)
			{
				return new Length(Mathf.LerpUnclamped(a.value, b.value, t), b.unit);
			}

			// Token: 0x06000590 RID: 1424 RVA: 0x00015224 File Offset: 0x00013424
			protected sealed override void UpdateValues()
			{
				int count = this.running.count;
				for (int i = 0; i < count; i++)
				{
					ref StylePropertyAnimationSystem.Values<Length>.TimingData ptr = ref this.running.timing[i];
					ref StylePropertyAnimationSystem.Values<Length>.StyleData ptr2 = ref this.running.style[i];
					ptr2.currentValue = StylePropertyAnimationSystem.ValuesLength.Lerp(ptr2.startValue, ptr2.endValue, ptr.easedProgress);
				}
			}

			// Token: 0x06000591 RID: 1425 RVA: 0x00015294 File Offset: 0x00013494
			protected sealed override void UpdateComputedStyle()
			{
				int count = this.running.count;
				for (int i = 0; i < count; i++)
				{
					this.running.elements[i].computedStyle.ApplyPropertyAnimation(this.running.elements[i], this.running.properties[i], this.running.style[i].currentValue);
				}
			}

			// Token: 0x06000592 RID: 1426 RVA: 0x00015308 File Offset: 0x00013508
			protected sealed override void UpdateComputedStyle(int i)
			{
				this.running.elements[i].computedStyle.ApplyPropertyAnimation(this.running.elements[i], this.running.properties[i], this.running.style[i].currentValue);
			}
		}

		// Token: 0x020000A5 RID: 165
		private class ValuesColor : StylePropertyAnimationSystem.Values<Color>
		{
			// Token: 0x17000155 RID: 341
			// (get) Token: 0x06000594 RID: 1428 RVA: 0x00015379 File Offset: 0x00013579
			public override Func<Color, Color, bool> SameFunc { get; } = new Func<Color, Color, bool>(StylePropertyAnimationSystem.ValuesColor.IsSame);

			// Token: 0x06000595 RID: 1429 RVA: 0x00015384 File Offset: 0x00013584
			private static bool IsSame(Color c, Color d)
			{
				return Mathf.Approximately(c.r, d.r) && Mathf.Approximately(c.g, d.g) && Mathf.Approximately(c.b, d.b) && Mathf.Approximately(c.a, d.a);
			}

			// Token: 0x06000596 RID: 1430 RVA: 0x000153DE File Offset: 0x000135DE
			private static Color Lerp(Color a, Color b, float t)
			{
				return Color.LerpUnclamped(a, b, t);
			}

			// Token: 0x06000597 RID: 1431 RVA: 0x000153E8 File Offset: 0x000135E8
			protected sealed override void UpdateValues()
			{
				int count = this.running.count;
				for (int i = 0; i < count; i++)
				{
					ref StylePropertyAnimationSystem.Values<Color>.TimingData ptr = ref this.running.timing[i];
					ref StylePropertyAnimationSystem.Values<Color>.StyleData ptr2 = ref this.running.style[i];
					ptr2.currentValue = StylePropertyAnimationSystem.ValuesColor.Lerp(ptr2.startValue, ptr2.endValue, ptr.easedProgress);
				}
			}

			// Token: 0x06000598 RID: 1432 RVA: 0x00015458 File Offset: 0x00013658
			protected sealed override void UpdateComputedStyle()
			{
				int count = this.running.count;
				for (int i = 0; i < count; i++)
				{
					this.running.elements[i].computedStyle.ApplyPropertyAnimation(this.running.elements[i], this.running.properties[i], this.running.style[i].currentValue);
				}
			}

			// Token: 0x06000599 RID: 1433 RVA: 0x000154CC File Offset: 0x000136CC
			protected sealed override void UpdateComputedStyle(int i)
			{
				this.running.elements[i].computedStyle.ApplyPropertyAnimation(this.running.elements[i], this.running.properties[i], this.running.style[i].currentValue);
			}
		}

		// Token: 0x020000A6 RID: 166
		private abstract class ValuesDiscrete<T> : StylePropertyAnimationSystem.Values<T>
		{
			// Token: 0x17000156 RID: 342
			// (get) Token: 0x0600059B RID: 1435 RVA: 0x0001553D File Offset: 0x0001373D
			public override Func<T, T, bool> SameFunc { get; } = new Func<T, T, bool>(StylePropertyAnimationSystem.ValuesDiscrete<T>.IsSame);

			// Token: 0x0600059C RID: 1436 RVA: 0x00015545 File Offset: 0x00013745
			private static bool IsSame(T a, T b)
			{
				return EqualityComparer<T>.Default.Equals(a, b);
			}

			// Token: 0x0600059D RID: 1437 RVA: 0x00015553 File Offset: 0x00013753
			private static T Lerp(T a, T b, float t)
			{
				return (t < 0.5f) ? a : b;
			}

			// Token: 0x0600059E RID: 1438 RVA: 0x00015564 File Offset: 0x00013764
			protected sealed override void UpdateValues()
			{
				int count = this.running.count;
				for (int i = 0; i < count; i++)
				{
					ref StylePropertyAnimationSystem.Values<T>.TimingData ptr = ref this.running.timing[i];
					ref StylePropertyAnimationSystem.Values<T>.StyleData ptr2 = ref this.running.style[i];
					ptr2.currentValue = StylePropertyAnimationSystem.ValuesDiscrete<T>.Lerp(ptr2.startValue, ptr2.endValue, ptr.easedProgress);
				}
			}
		}

		// Token: 0x020000A7 RID: 167
		private class ValuesEnum : StylePropertyAnimationSystem.ValuesDiscrete<int>
		{
			// Token: 0x060005A0 RID: 1440 RVA: 0x000155F0 File Offset: 0x000137F0
			protected sealed override void UpdateComputedStyle()
			{
				int count = this.running.count;
				for (int i = 0; i < count; i++)
				{
					this.running.elements[i].computedStyle.ApplyPropertyAnimation(this.running.elements[i], this.running.properties[i], this.running.style[i].currentValue);
				}
			}

			// Token: 0x060005A1 RID: 1441 RVA: 0x00015664 File Offset: 0x00013864
			protected sealed override void UpdateComputedStyle(int i)
			{
				this.running.elements[i].computedStyle.ApplyPropertyAnimation(this.running.elements[i], this.running.properties[i], this.running.style[i].currentValue);
			}
		}

		// Token: 0x020000A8 RID: 168
		private class ValuesBackground : StylePropertyAnimationSystem.ValuesDiscrete<Background>
		{
			// Token: 0x060005A3 RID: 1443 RVA: 0x000156C4 File Offset: 0x000138C4
			protected sealed override void UpdateComputedStyle()
			{
				int count = this.running.count;
				for (int i = 0; i < count; i++)
				{
					this.running.elements[i].computedStyle.ApplyPropertyAnimation(this.running.elements[i], this.running.properties[i], this.running.style[i].currentValue);
				}
			}

			// Token: 0x060005A4 RID: 1444 RVA: 0x00015738 File Offset: 0x00013938
			protected sealed override void UpdateComputedStyle(int i)
			{
				this.running.elements[i].computedStyle.ApplyPropertyAnimation(this.running.elements[i], this.running.properties[i], this.running.style[i].currentValue);
			}
		}

		// Token: 0x020000A9 RID: 169
		private class ValuesFontDefinition : StylePropertyAnimationSystem.ValuesDiscrete<FontDefinition>
		{
			// Token: 0x060005A6 RID: 1446 RVA: 0x00015798 File Offset: 0x00013998
			protected sealed override void UpdateComputedStyle()
			{
				int count = this.running.count;
				for (int i = 0; i < count; i++)
				{
					this.running.elements[i].computedStyle.ApplyPropertyAnimation(this.running.elements[i], this.running.properties[i], this.running.style[i].currentValue);
				}
			}

			// Token: 0x060005A7 RID: 1447 RVA: 0x0001580C File Offset: 0x00013A0C
			protected sealed override void UpdateComputedStyle(int i)
			{
				this.running.elements[i].computedStyle.ApplyPropertyAnimation(this.running.elements[i], this.running.properties[i], this.running.style[i].currentValue);
			}
		}

		// Token: 0x020000AA RID: 170
		private class ValuesFont : StylePropertyAnimationSystem.ValuesDiscrete<Font>
		{
			// Token: 0x060005A9 RID: 1449 RVA: 0x0001586C File Offset: 0x00013A6C
			protected sealed override void UpdateComputedStyle()
			{
				int count = this.running.count;
				for (int i = 0; i < count; i++)
				{
					this.running.elements[i].computedStyle.ApplyPropertyAnimation(this.running.elements[i], this.running.properties[i], this.running.style[i].currentValue);
				}
			}

			// Token: 0x060005AA RID: 1450 RVA: 0x000158E0 File Offset: 0x00013AE0
			protected sealed override void UpdateComputedStyle(int i)
			{
				this.running.elements[i].computedStyle.ApplyPropertyAnimation(this.running.elements[i], this.running.properties[i], this.running.style[i].currentValue);
			}
		}

		// Token: 0x020000AB RID: 171
		private class ValuesTextShadow : StylePropertyAnimationSystem.Values<TextShadow>
		{
			// Token: 0x17000157 RID: 343
			// (get) Token: 0x060005AC RID: 1452 RVA: 0x0001593F File Offset: 0x00013B3F
			public override Func<TextShadow, TextShadow, bool> SameFunc { get; } = new Func<TextShadow, TextShadow, bool>(StylePropertyAnimationSystem.ValuesTextShadow.IsSame);

			// Token: 0x060005AD RID: 1453 RVA: 0x00015947 File Offset: 0x00013B47
			private static bool IsSame(TextShadow a, TextShadow b)
			{
				return a == b;
			}

			// Token: 0x060005AE RID: 1454 RVA: 0x00015950 File Offset: 0x00013B50
			private static TextShadow Lerp(TextShadow a, TextShadow b, float t)
			{
				return TextShadow.LerpUnclamped(a, b, t);
			}

			// Token: 0x060005AF RID: 1455 RVA: 0x0001595C File Offset: 0x00013B5C
			protected sealed override void UpdateValues()
			{
				int count = this.running.count;
				for (int i = 0; i < count; i++)
				{
					ref StylePropertyAnimationSystem.Values<TextShadow>.TimingData ptr = ref this.running.timing[i];
					ref StylePropertyAnimationSystem.Values<TextShadow>.StyleData ptr2 = ref this.running.style[i];
					ptr2.currentValue = StylePropertyAnimationSystem.ValuesTextShadow.Lerp(ptr2.startValue, ptr2.endValue, ptr.easedProgress);
				}
			}

			// Token: 0x060005B0 RID: 1456 RVA: 0x000159CC File Offset: 0x00013BCC
			protected sealed override void UpdateComputedStyle()
			{
				int count = this.running.count;
				for (int i = 0; i < count; i++)
				{
					this.running.elements[i].computedStyle.ApplyPropertyAnimation(this.running.elements[i], this.running.properties[i], this.running.style[i].currentValue);
				}
			}

			// Token: 0x060005B1 RID: 1457 RVA: 0x00015A40 File Offset: 0x00013C40
			protected sealed override void UpdateComputedStyle(int i)
			{
				this.running.elements[i].computedStyle.ApplyPropertyAnimation(this.running.elements[i], this.running.properties[i], this.running.style[i].currentValue);
			}
		}

		// Token: 0x020000AC RID: 172
		private class ValuesScale : StylePropertyAnimationSystem.Values<Scale>
		{
			// Token: 0x17000158 RID: 344
			// (get) Token: 0x060005B3 RID: 1459 RVA: 0x00015AB1 File Offset: 0x00013CB1
			public override Func<Scale, Scale, bool> SameFunc { get; } = new Func<Scale, Scale, bool>(StylePropertyAnimationSystem.ValuesScale.IsSame);

			// Token: 0x060005B4 RID: 1460 RVA: 0x00015AB9 File Offset: 0x00013CB9
			private static bool IsSame(Scale a, Scale b)
			{
				return a == b;
			}

			// Token: 0x060005B5 RID: 1461 RVA: 0x00015AC4 File Offset: 0x00013CC4
			protected sealed override void UpdateComputedStyle()
			{
				int count = this.running.count;
				for (int i = 0; i < count; i++)
				{
					this.running.elements[i].computedStyle.ApplyPropertyAnimation(this.running.elements[i], this.running.properties[i], this.running.style[i].currentValue);
				}
			}

			// Token: 0x060005B6 RID: 1462 RVA: 0x00015B38 File Offset: 0x00013D38
			protected sealed override void UpdateComputedStyle(int i)
			{
				this.running.elements[i].computedStyle.ApplyPropertyAnimation(this.running.elements[i], this.running.properties[i], this.running.style[i].currentValue);
			}

			// Token: 0x060005B7 RID: 1463 RVA: 0x00015B8E File Offset: 0x00013D8E
			private static Scale Lerp(Scale a, Scale b, float t)
			{
				return new Scale(Vector3.LerpUnclamped(a.value, b.value, t));
			}

			// Token: 0x060005B8 RID: 1464 RVA: 0x00015BAC File Offset: 0x00013DAC
			protected sealed override void UpdateValues()
			{
				int count = this.running.count;
				for (int i = 0; i < count; i++)
				{
					ref StylePropertyAnimationSystem.Values<Scale>.TimingData ptr = ref this.running.timing[i];
					ref StylePropertyAnimationSystem.Values<Scale>.StyleData ptr2 = ref this.running.style[i];
					ptr2.currentValue = StylePropertyAnimationSystem.ValuesScale.Lerp(ptr2.startValue, ptr2.endValue, ptr.easedProgress);
				}
			}
		}

		// Token: 0x020000AD RID: 173
		private class ValuesRotate : StylePropertyAnimationSystem.Values<Rotate>
		{
			// Token: 0x17000159 RID: 345
			// (get) Token: 0x060005BA RID: 1466 RVA: 0x00015C36 File Offset: 0x00013E36
			public override Func<Rotate, Rotate, bool> SameFunc { get; } = new Func<Rotate, Rotate, bool>(StylePropertyAnimationSystem.ValuesRotate.IsSame);

			// Token: 0x060005BB RID: 1467 RVA: 0x00015C3E File Offset: 0x00013E3E
			private static bool IsSame(Rotate a, Rotate b)
			{
				return a == b;
			}

			// Token: 0x060005BC RID: 1468 RVA: 0x00015C48 File Offset: 0x00013E48
			protected sealed override void UpdateComputedStyle()
			{
				int count = this.running.count;
				for (int i = 0; i < count; i++)
				{
					this.running.elements[i].computedStyle.ApplyPropertyAnimation(this.running.elements[i], this.running.properties[i], this.running.style[i].currentValue);
				}
			}

			// Token: 0x060005BD RID: 1469 RVA: 0x00015CBC File Offset: 0x00013EBC
			protected sealed override void UpdateComputedStyle(int i)
			{
				this.running.elements[i].computedStyle.ApplyPropertyAnimation(this.running.elements[i], this.running.properties[i], this.running.style[i].currentValue);
			}

			// Token: 0x060005BE RID: 1470 RVA: 0x00015D14 File Offset: 0x00013F14
			private static Rotate Lerp(Rotate a, Rotate b, float t)
			{
				return new Rotate(Mathf.LerpUnclamped(a.angle.ToDegrees(), b.angle.ToDegrees(), t));
			}

			// Token: 0x060005BF RID: 1471 RVA: 0x00015D50 File Offset: 0x00013F50
			protected sealed override void UpdateValues()
			{
				int count = this.running.count;
				for (int i = 0; i < count; i++)
				{
					ref StylePropertyAnimationSystem.Values<Rotate>.TimingData ptr = ref this.running.timing[i];
					ref StylePropertyAnimationSystem.Values<Rotate>.StyleData ptr2 = ref this.running.style[i];
					ptr2.currentValue = StylePropertyAnimationSystem.ValuesRotate.Lerp(ptr2.startValue, ptr2.endValue, ptr.easedProgress);
				}
			}
		}

		// Token: 0x020000AE RID: 174
		private class ValuesTranslate : StylePropertyAnimationSystem.Values<Translate>
		{
			// Token: 0x1700015A RID: 346
			// (get) Token: 0x060005C1 RID: 1473 RVA: 0x00015DDA File Offset: 0x00013FDA
			public override Func<Translate, Translate, bool> SameFunc { get; } = new Func<Translate, Translate, bool>(StylePropertyAnimationSystem.ValuesTranslate.IsSame);

			// Token: 0x060005C2 RID: 1474 RVA: 0x00015DE2 File Offset: 0x00013FE2
			private static bool IsSame(Translate a, Translate b)
			{
				return a == b;
			}

			// Token: 0x060005C3 RID: 1475 RVA: 0x00015DEC File Offset: 0x00013FEC
			protected sealed override void UpdateComputedStyle()
			{
				int count = this.running.count;
				for (int i = 0; i < count; i++)
				{
					this.running.elements[i].computedStyle.ApplyPropertyAnimation(this.running.elements[i], this.running.properties[i], this.running.style[i].currentValue);
				}
			}

			// Token: 0x060005C4 RID: 1476 RVA: 0x00015E60 File Offset: 0x00014060
			protected sealed override void UpdateComputedStyle(int i)
			{
				this.running.elements[i].computedStyle.ApplyPropertyAnimation(this.running.elements[i], this.running.properties[i], this.running.style[i].currentValue);
			}

			// Token: 0x060005C5 RID: 1477 RVA: 0x00015EB8 File Offset: 0x000140B8
			private static Translate Lerp(Translate a, Translate b, float t)
			{
				return new Translate(StylePropertyAnimationSystem.ValuesLength.Lerp(a.x, b.x, t), StylePropertyAnimationSystem.ValuesLength.Lerp(a.y, b.y, t), Mathf.Lerp(a.z, b.z, t));
			}

			// Token: 0x060005C6 RID: 1478 RVA: 0x00015F08 File Offset: 0x00014108
			protected sealed override void UpdateValues()
			{
				int count = this.running.count;
				for (int i = 0; i < count; i++)
				{
					ref StylePropertyAnimationSystem.Values<Translate>.TimingData ptr = ref this.running.timing[i];
					ref StylePropertyAnimationSystem.Values<Translate>.StyleData ptr2 = ref this.running.style[i];
					ptr2.currentValue = StylePropertyAnimationSystem.ValuesTranslate.Lerp(ptr2.startValue, ptr2.endValue, ptr.easedProgress);
				}
			}
		}

		// Token: 0x020000AF RID: 175
		private class ValuesTransformOrigin : StylePropertyAnimationSystem.Values<TransformOrigin>
		{
			// Token: 0x1700015B RID: 347
			// (get) Token: 0x060005C8 RID: 1480 RVA: 0x00015F92 File Offset: 0x00014192
			public override Func<TransformOrigin, TransformOrigin, bool> SameFunc { get; } = new Func<TransformOrigin, TransformOrigin, bool>(StylePropertyAnimationSystem.ValuesTransformOrigin.IsSame);

			// Token: 0x060005C9 RID: 1481 RVA: 0x00015F9A File Offset: 0x0001419A
			private static bool IsSame(TransformOrigin a, TransformOrigin b)
			{
				return a == b;
			}

			// Token: 0x060005CA RID: 1482 RVA: 0x00015FA4 File Offset: 0x000141A4
			protected sealed override void UpdateComputedStyle()
			{
				int count = this.running.count;
				for (int i = 0; i < count; i++)
				{
					this.running.elements[i].computedStyle.ApplyPropertyAnimation(this.running.elements[i], this.running.properties[i], this.running.style[i].currentValue);
				}
			}

			// Token: 0x060005CB RID: 1483 RVA: 0x00016018 File Offset: 0x00014218
			protected sealed override void UpdateComputedStyle(int i)
			{
				this.running.elements[i].computedStyle.ApplyPropertyAnimation(this.running.elements[i], this.running.properties[i], this.running.style[i].currentValue);
			}

			// Token: 0x060005CC RID: 1484 RVA: 0x00016070 File Offset: 0x00014270
			private static TransformOrigin Lerp(TransformOrigin a, TransformOrigin b, float t)
			{
				return new TransformOrigin(StylePropertyAnimationSystem.ValuesLength.Lerp(a.x, b.x, t), StylePropertyAnimationSystem.ValuesLength.Lerp(a.y, b.y, t), Mathf.Lerp(a.z, b.z, t));
			}

			// Token: 0x060005CD RID: 1485 RVA: 0x000160C0 File Offset: 0x000142C0
			protected sealed override void UpdateValues()
			{
				int count = this.running.count;
				for (int i = 0; i < count; i++)
				{
					ref StylePropertyAnimationSystem.Values<TransformOrigin>.TimingData ptr = ref this.running.timing[i];
					ref StylePropertyAnimationSystem.Values<TransformOrigin>.StyleData ptr2 = ref this.running.style[i];
					ptr2.currentValue = StylePropertyAnimationSystem.ValuesTransformOrigin.Lerp(ptr2.startValue, ptr2.endValue, ptr.easedProgress);
				}
			}
		}
	}
}
