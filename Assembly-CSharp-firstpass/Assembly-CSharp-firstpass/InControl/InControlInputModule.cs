using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

namespace InControl
{
	// Token: 0x0200004B RID: 75
	[AddComponentMenu("Event/InControl Input Module")]
	public class InControlInputModule : PointerInputModule
	{
		// Token: 0x17000105 RID: 261
		// (get) Token: 0x06000320 RID: 800 RVA: 0x0000AC6A File Offset: 0x00008E6A
		// (set) Token: 0x06000321 RID: 801 RVA: 0x0000AC72 File Offset: 0x00008E72
		public PlayerAction SubmitAction { get; set; }

		// Token: 0x17000106 RID: 262
		// (get) Token: 0x06000322 RID: 802 RVA: 0x0000AC7B File Offset: 0x00008E7B
		// (set) Token: 0x06000323 RID: 803 RVA: 0x0000AC83 File Offset: 0x00008E83
		public PlayerAction CancelAction { get; set; }

		// Token: 0x17000107 RID: 263
		// (get) Token: 0x06000324 RID: 804 RVA: 0x0000AC8C File Offset: 0x00008E8C
		// (set) Token: 0x06000325 RID: 805 RVA: 0x0000AC94 File Offset: 0x00008E94
		public PlayerTwoAxisAction MoveAction { get; set; }

		// Token: 0x06000326 RID: 806 RVA: 0x0000ACA0 File Offset: 0x00008EA0
		protected InControlInputModule()
		{
			this.direction = new TwoAxisInputControl();
			this.direction.StateThreshold = this.analogMoveThreshold;
		}

		// Token: 0x06000327 RID: 807 RVA: 0x0000AD0E File Offset: 0x00008F0E
		public override void UpdateModule()
		{
			this.lastMousePosition = this.thisMousePosition;
			this.thisMousePosition = InputManager.MouseProvider.GetPosition();
		}

		// Token: 0x06000328 RID: 808 RVA: 0x0000AD31 File Offset: 0x00008F31
		public override bool IsModuleSupported()
		{
			return this.forceModuleActive || InputManager.MouseProvider.HasMousePresent() || Input.touchSupported;
		}

		// Token: 0x06000329 RID: 809 RVA: 0x0000AD54 File Offset: 0x00008F54
		public override bool ShouldActivateModule()
		{
			if (!base.enabled || !base.gameObject.activeInHierarchy)
			{
				return false;
			}
			this.UpdateInputState();
			bool flag = false;
			flag |= this.SubmitWasPressed;
			flag |= this.CancelWasPressed;
			flag |= this.VectorWasPressed;
			if (this.allowMouseInput)
			{
				flag |= this.MouseHasMoved;
				flag |= InControlInputModule.MouseButtonWasPressed;
			}
			if (this.allowTouchInput)
			{
				flag |= Input.touchCount > 0;
			}
			return flag;
		}

		// Token: 0x0600032A RID: 810 RVA: 0x0000ADC8 File Offset: 0x00008FC8
		public override void ActivateModule()
		{
			base.ActivateModule();
			this.thisMousePosition = InputManager.MouseProvider.GetPosition();
			this.lastMousePosition = this.thisMousePosition;
			GameObject gameObject = base.eventSystem.currentSelectedGameObject;
			if (gameObject == null)
			{
				gameObject = base.eventSystem.firstSelectedGameObject;
			}
			base.eventSystem.SetSelectedGameObject(gameObject, this.GetBaseEventData());
		}

		// Token: 0x0600032B RID: 811 RVA: 0x0000AE30 File Offset: 0x00009030
		public override void Process()
		{
			bool flag = this.SendUpdateEventToSelectedObject();
			if (base.eventSystem.sendNavigationEvents)
			{
				if (!flag)
				{
					flag = this.SendVectorEventToSelectedObject();
				}
				if (!flag)
				{
					this.SendButtonEventToSelectedObject();
				}
			}
			if (this.allowTouchInput && this.ProcessTouchEvents())
			{
				return;
			}
			if (this.allowMouseInput)
			{
				this.ProcessMouseEvent();
			}
		}

		// Token: 0x0600032C RID: 812 RVA: 0x0000AE84 File Offset: 0x00009084
		private bool ProcessTouchEvents()
		{
			int touchCount = Input.touchCount;
			for (int i = 0; i < touchCount; i++)
			{
				Touch touch = Input.GetTouch(i);
				if (touch.type != TouchType.Indirect)
				{
					bool flag;
					bool flag2;
					PointerEventData touchPointerEventData = base.GetTouchPointerEventData(touch, out flag, out flag2);
					this.ProcessTouchPress(touchPointerEventData, flag, flag2);
					if (!flag2)
					{
						this.ProcessMove(touchPointerEventData);
						this.ProcessDrag(touchPointerEventData);
					}
					else
					{
						base.RemovePointerData(touchPointerEventData);
					}
				}
			}
			return touchCount > 0;
		}

		// Token: 0x0600032D RID: 813 RVA: 0x0000AEF0 File Offset: 0x000090F0
		private bool SendButtonEventToSelectedObject()
		{
			if (base.eventSystem.currentSelectedGameObject == null)
			{
				return false;
			}
			BaseEventData baseEventData = this.GetBaseEventData();
			if (this.SubmitWasPressed)
			{
				ExecuteEvents.Execute<ISubmitHandler>(base.eventSystem.currentSelectedGameObject, baseEventData, ExecuteEvents.submitHandler);
			}
			else
			{
				bool submitWasReleased = this.SubmitWasReleased;
			}
			if (this.CancelWasPressed)
			{
				ExecuteEvents.Execute<ICancelHandler>(base.eventSystem.currentSelectedGameObject, baseEventData, ExecuteEvents.cancelHandler);
			}
			return baseEventData.used;
		}

		// Token: 0x0600032E RID: 814 RVA: 0x0000AF68 File Offset: 0x00009168
		private bool SendVectorEventToSelectedObject()
		{
			if (!this.VectorWasPressed)
			{
				return false;
			}
			AxisEventData axisEventData = this.GetAxisEventData(this.thisVectorState.x, this.thisVectorState.y, 0.5f);
			if (axisEventData.moveDir != MoveDirection.None)
			{
				if (base.eventSystem.currentSelectedGameObject == null)
				{
					base.eventSystem.SetSelectedGameObject(base.eventSystem.firstSelectedGameObject, this.GetBaseEventData());
				}
				else
				{
					ExecuteEvents.Execute<IMoveHandler>(base.eventSystem.currentSelectedGameObject, axisEventData, ExecuteEvents.moveHandler);
				}
				this.SetVectorRepeatTimer();
			}
			return axisEventData.used;
		}

		// Token: 0x0600032F RID: 815 RVA: 0x0000B000 File Offset: 0x00009200
		protected override void ProcessMove(PointerEventData pointerEvent)
		{
			GameObject pointerEnter = pointerEvent.pointerEnter;
			base.ProcessMove(pointerEvent);
			if (this.focusOnMouseHover && pointerEnter != pointerEvent.pointerEnter)
			{
				GameObject eventHandler = ExecuteEvents.GetEventHandler<ISelectHandler>(pointerEvent.pointerEnter);
				base.eventSystem.SetSelectedGameObject(eventHandler, pointerEvent);
			}
		}

		// Token: 0x06000330 RID: 816 RVA: 0x0000B04A File Offset: 0x0000924A
		private void Update()
		{
			this.direction.Filter(this.Device.Direction, Time.deltaTime);
		}

		// Token: 0x06000331 RID: 817 RVA: 0x0000B068 File Offset: 0x00009268
		private void UpdateInputState()
		{
			this.lastVectorState = this.thisVectorState;
			this.thisVectorState = Vector2.zero;
			TwoAxisInputControl twoAxisInputControl = this.MoveAction ?? this.direction;
			if (Utility.AbsoluteIsOverThreshold(twoAxisInputControl.X, this.analogMoveThreshold))
			{
				this.thisVectorState.x = Mathf.Sign(twoAxisInputControl.X);
			}
			if (Utility.AbsoluteIsOverThreshold(twoAxisInputControl.Y, this.analogMoveThreshold))
			{
				this.thisVectorState.y = Mathf.Sign(twoAxisInputControl.Y);
			}
			this.moveWasRepeated = false;
			if (this.VectorIsReleased)
			{
				this.nextMoveRepeatTime = 0f;
			}
			else if (this.VectorIsPressed)
			{
				float realtimeSinceStartup = Time.realtimeSinceStartup;
				if (this.lastVectorState == Vector2.zero)
				{
					this.nextMoveRepeatTime = realtimeSinceStartup + this.moveRepeatFirstDuration;
				}
				else if (realtimeSinceStartup >= this.nextMoveRepeatTime)
				{
					this.moveWasRepeated = true;
					this.nextMoveRepeatTime = realtimeSinceStartup + this.moveRepeatDelayDuration;
				}
			}
			this.lastSubmitState = this.thisSubmitState;
			this.thisSubmitState = ((this.SubmitAction == null) ? this.SubmitButton.IsPressed : this.SubmitAction.IsPressed);
			this.lastCancelState = this.thisCancelState;
			this.thisCancelState = ((this.CancelAction == null) ? this.CancelButton.IsPressed : this.CancelAction.IsPressed);
		}

		// Token: 0x17000108 RID: 264
		// (get) Token: 0x06000333 RID: 819 RVA: 0x0000B1C6 File Offset: 0x000093C6
		// (set) Token: 0x06000332 RID: 818 RVA: 0x0000B1BD File Offset: 0x000093BD
		public InputDevice Device
		{
			get
			{
				return this.inputDevice ?? InputManager.ActiveDevice;
			}
			set
			{
				this.inputDevice = value;
			}
		}

		// Token: 0x17000109 RID: 265
		// (get) Token: 0x06000334 RID: 820 RVA: 0x0000B1D7 File Offset: 0x000093D7
		private InputControl SubmitButton
		{
			get
			{
				return this.Device.GetControl((InputControlType)this.submitButton);
			}
		}

		// Token: 0x1700010A RID: 266
		// (get) Token: 0x06000335 RID: 821 RVA: 0x0000B1EA File Offset: 0x000093EA
		private InputControl CancelButton
		{
			get
			{
				return this.Device.GetControl((InputControlType)this.cancelButton);
			}
		}

		// Token: 0x06000336 RID: 822 RVA: 0x0000B1FD File Offset: 0x000093FD
		private void SetVectorRepeatTimer()
		{
			this.nextMoveRepeatTime = Mathf.Max(this.nextMoveRepeatTime, Time.realtimeSinceStartup + this.moveRepeatDelayDuration);
		}

		// Token: 0x1700010B RID: 267
		// (get) Token: 0x06000337 RID: 823 RVA: 0x0000B21C File Offset: 0x0000941C
		private bool VectorIsPressed
		{
			get
			{
				return this.thisVectorState != Vector2.zero;
			}
		}

		// Token: 0x1700010C RID: 268
		// (get) Token: 0x06000338 RID: 824 RVA: 0x0000B22E File Offset: 0x0000942E
		private bool VectorIsReleased
		{
			get
			{
				return this.thisVectorState == Vector2.zero;
			}
		}

		// Token: 0x1700010D RID: 269
		// (get) Token: 0x06000339 RID: 825 RVA: 0x0000B240 File Offset: 0x00009440
		private bool VectorHasChanged
		{
			get
			{
				return this.thisVectorState != this.lastVectorState;
			}
		}

		// Token: 0x1700010E RID: 270
		// (get) Token: 0x0600033A RID: 826 RVA: 0x0000B253 File Offset: 0x00009453
		private bool VectorWasPressed
		{
			get
			{
				return this.moveWasRepeated || (this.VectorIsPressed && this.lastVectorState == Vector2.zero);
			}
		}

		// Token: 0x1700010F RID: 271
		// (get) Token: 0x0600033B RID: 827 RVA: 0x0000B279 File Offset: 0x00009479
		private bool SubmitWasPressed
		{
			get
			{
				return this.thisSubmitState && this.thisSubmitState != this.lastSubmitState;
			}
		}

		// Token: 0x17000110 RID: 272
		// (get) Token: 0x0600033C RID: 828 RVA: 0x0000B296 File Offset: 0x00009496
		private bool SubmitWasReleased
		{
			get
			{
				return !this.thisSubmitState && this.thisSubmitState != this.lastSubmitState;
			}
		}

		// Token: 0x17000111 RID: 273
		// (get) Token: 0x0600033D RID: 829 RVA: 0x0000B2B3 File Offset: 0x000094B3
		private bool CancelWasPressed
		{
			get
			{
				return this.thisCancelState && this.thisCancelState != this.lastCancelState;
			}
		}

		// Token: 0x17000112 RID: 274
		// (get) Token: 0x0600033E RID: 830 RVA: 0x0000B2D0 File Offset: 0x000094D0
		private bool MouseHasMoved
		{
			get
			{
				return (this.thisMousePosition - this.lastMousePosition).sqrMagnitude > 0f;
			}
		}

		// Token: 0x17000113 RID: 275
		// (get) Token: 0x0600033F RID: 831 RVA: 0x0000B2FD File Offset: 0x000094FD
		private static bool MouseButtonWasPressed
		{
			get
			{
				return InputManager.MouseProvider.GetButtonWasPressed(Mouse.LeftButton);
			}
		}

		// Token: 0x06000340 RID: 832 RVA: 0x0000B30C File Offset: 0x0000950C
		protected bool SendUpdateEventToSelectedObject()
		{
			if (base.eventSystem.currentSelectedGameObject == null)
			{
				return false;
			}
			BaseEventData baseEventData = this.GetBaseEventData();
			ExecuteEvents.Execute<IUpdateSelectedHandler>(base.eventSystem.currentSelectedGameObject, baseEventData, ExecuteEvents.updateSelectedHandler);
			return baseEventData.used;
		}

		// Token: 0x06000341 RID: 833 RVA: 0x0000B352 File Offset: 0x00009552
		protected void ProcessMouseEvent()
		{
			this.ProcessMouseEvent(0);
		}

		// Token: 0x06000342 RID: 834 RVA: 0x0000B35C File Offset: 0x0000955C
		protected void ProcessMouseEvent(int id)
		{
			PointerInputModule.MouseState mousePointerEventData = this.GetMousePointerEventData(id);
			PointerInputModule.MouseButtonEventData eventData = mousePointerEventData.GetButtonState(PointerEventData.InputButton.Left).eventData;
			this.ProcessMousePress(eventData);
			this.ProcessMove(eventData.buttonData);
			this.ProcessDrag(eventData.buttonData);
			this.ProcessMousePress(mousePointerEventData.GetButtonState(PointerEventData.InputButton.Right).eventData);
			this.ProcessDrag(mousePointerEventData.GetButtonState(PointerEventData.InputButton.Right).eventData.buttonData);
			this.ProcessMousePress(mousePointerEventData.GetButtonState(PointerEventData.InputButton.Middle).eventData);
			this.ProcessDrag(mousePointerEventData.GetButtonState(PointerEventData.InputButton.Middle).eventData.buttonData);
			if (!Mathf.Approximately(eventData.buttonData.scrollDelta.sqrMagnitude, 0f))
			{
				ExecuteEvents.ExecuteHierarchy<IScrollHandler>(ExecuteEvents.GetEventHandler<IScrollHandler>(eventData.buttonData.pointerCurrentRaycast.gameObject), eventData.buttonData, ExecuteEvents.scrollHandler);
			}
		}

		// Token: 0x06000343 RID: 835 RVA: 0x0000B438 File Offset: 0x00009638
		protected void ProcessMousePress(PointerInputModule.MouseButtonEventData data)
		{
			PointerEventData buttonData = data.buttonData;
			GameObject gameObject = buttonData.pointerCurrentRaycast.gameObject;
			if (data.PressedThisFrame())
			{
				buttonData.eligibleForClick = true;
				buttonData.delta = Vector2.zero;
				buttonData.dragging = false;
				buttonData.useDragThreshold = true;
				buttonData.pressPosition = buttonData.position;
				buttonData.pointerPressRaycast = buttonData.pointerCurrentRaycast;
				base.DeselectIfSelectionChanged(gameObject, buttonData);
				GameObject gameObject2 = ExecuteEvents.ExecuteHierarchy<IPointerDownHandler>(gameObject, buttonData, ExecuteEvents.pointerDownHandler);
				if (gameObject2 == null)
				{
					gameObject2 = ExecuteEvents.GetEventHandler<IPointerClickHandler>(gameObject);
				}
				float unscaledTime = Time.unscaledTime;
				if (gameObject2 == buttonData.lastPress)
				{
					if (unscaledTime - buttonData.clickTime < 0.3f)
					{
						PointerEventData pointerEventData = buttonData;
						int num = pointerEventData.clickCount + 1;
						pointerEventData.clickCount = num;
					}
					else
					{
						buttonData.clickCount = 1;
					}
					buttonData.clickTime = unscaledTime;
				}
				else
				{
					buttonData.clickCount = 1;
				}
				buttonData.pointerPress = gameObject2;
				buttonData.rawPointerPress = gameObject;
				buttonData.clickTime = unscaledTime;
				buttonData.pointerDrag = ExecuteEvents.GetEventHandler<IDragHandler>(gameObject);
				if (buttonData.pointerDrag != null)
				{
					ExecuteEvents.Execute<IInitializePotentialDragHandler>(buttonData.pointerDrag, buttonData, ExecuteEvents.initializePotentialDrag);
				}
			}
			if (data.ReleasedThisFrame())
			{
				ExecuteEvents.Execute<IPointerUpHandler>(buttonData.pointerPress, buttonData, ExecuteEvents.pointerUpHandler);
				GameObject eventHandler = ExecuteEvents.GetEventHandler<IPointerClickHandler>(gameObject);
				if (buttonData.pointerPress == eventHandler && buttonData.eligibleForClick)
				{
					ExecuteEvents.Execute<IPointerClickHandler>(buttonData.pointerPress, buttonData, ExecuteEvents.pointerClickHandler);
				}
				else if (buttonData.pointerDrag != null && buttonData.dragging)
				{
					ExecuteEvents.ExecuteHierarchy<IDropHandler>(gameObject, buttonData, ExecuteEvents.dropHandler);
				}
				buttonData.eligibleForClick = false;
				buttonData.pointerPress = null;
				buttonData.rawPointerPress = null;
				if (buttonData.pointerDrag != null && buttonData.dragging)
				{
					ExecuteEvents.Execute<IEndDragHandler>(buttonData.pointerDrag, buttonData, ExecuteEvents.endDragHandler);
				}
				buttonData.dragging = false;
				buttonData.pointerDrag = null;
				if (gameObject != buttonData.pointerEnter)
				{
					base.HandlePointerExitAndEnter(buttonData, null);
					base.HandlePointerExitAndEnter(buttonData, gameObject);
				}
			}
		}

		// Token: 0x06000344 RID: 836 RVA: 0x0000B634 File Offset: 0x00009834
		protected void ProcessTouchPress(PointerEventData pointerEvent, bool pressed, bool released)
		{
			GameObject gameObject = pointerEvent.pointerCurrentRaycast.gameObject;
			if (pressed)
			{
				pointerEvent.eligibleForClick = true;
				pointerEvent.delta = Vector2.zero;
				pointerEvent.dragging = false;
				pointerEvent.useDragThreshold = true;
				pointerEvent.pressPosition = pointerEvent.position;
				pointerEvent.pointerPressRaycast = pointerEvent.pointerCurrentRaycast;
				base.DeselectIfSelectionChanged(gameObject, pointerEvent);
				if (pointerEvent.pointerEnter != gameObject)
				{
					base.HandlePointerExitAndEnter(pointerEvent, gameObject);
					pointerEvent.pointerEnter = gameObject;
				}
				GameObject gameObject2 = ExecuteEvents.ExecuteHierarchy<IPointerDownHandler>(gameObject, pointerEvent, ExecuteEvents.pointerDownHandler);
				if (gameObject2 == null)
				{
					gameObject2 = ExecuteEvents.GetEventHandler<IPointerClickHandler>(gameObject);
				}
				float unscaledTime = Time.unscaledTime;
				if (gameObject2 == pointerEvent.lastPress)
				{
					if (unscaledTime - pointerEvent.clickTime < 0.3f)
					{
						int num = pointerEvent.clickCount + 1;
						pointerEvent.clickCount = num;
					}
					else
					{
						pointerEvent.clickCount = 1;
					}
					pointerEvent.clickTime = unscaledTime;
				}
				else
				{
					pointerEvent.clickCount = 1;
				}
				pointerEvent.pointerPress = gameObject2;
				pointerEvent.rawPointerPress = gameObject;
				pointerEvent.clickTime = unscaledTime;
				pointerEvent.pointerDrag = ExecuteEvents.GetEventHandler<IDragHandler>(gameObject);
				if (pointerEvent.pointerDrag != null)
				{
					ExecuteEvents.Execute<IInitializePotentialDragHandler>(pointerEvent.pointerDrag, pointerEvent, ExecuteEvents.initializePotentialDrag);
				}
			}
			if (released)
			{
				ExecuteEvents.Execute<IPointerUpHandler>(pointerEvent.pointerPress, pointerEvent, ExecuteEvents.pointerUpHandler);
				GameObject eventHandler = ExecuteEvents.GetEventHandler<IPointerClickHandler>(gameObject);
				if (pointerEvent.pointerPress == eventHandler && pointerEvent.eligibleForClick)
				{
					ExecuteEvents.Execute<IPointerClickHandler>(pointerEvent.pointerPress, pointerEvent, ExecuteEvents.pointerClickHandler);
				}
				else if (pointerEvent.pointerDrag != null && pointerEvent.dragging)
				{
					ExecuteEvents.ExecuteHierarchy<IDropHandler>(gameObject, pointerEvent, ExecuteEvents.dropHandler);
				}
				pointerEvent.eligibleForClick = false;
				pointerEvent.pointerPress = null;
				pointerEvent.rawPointerPress = null;
				if (pointerEvent.pointerDrag != null && pointerEvent.dragging)
				{
					ExecuteEvents.Execute<IEndDragHandler>(pointerEvent.pointerDrag, pointerEvent, ExecuteEvents.endDragHandler);
				}
				pointerEvent.dragging = false;
				pointerEvent.pointerDrag = null;
				if (pointerEvent.pointerDrag != null)
				{
					ExecuteEvents.Execute<IEndDragHandler>(pointerEvent.pointerDrag, pointerEvent, ExecuteEvents.endDragHandler);
				}
				pointerEvent.pointerDrag = null;
				ExecuteEvents.ExecuteHierarchy<IPointerExitHandler>(pointerEvent.pointerEnter, pointerEvent, ExecuteEvents.pointerExitHandler);
				pointerEvent.pointerEnter = null;
			}
		}

		// Token: 0x04000340 RID: 832
		public InControlInputModule.Button submitButton = InControlInputModule.Button.Action1;

		// Token: 0x04000341 RID: 833
		public InControlInputModule.Button cancelButton = InControlInputModule.Button.Action2;

		// Token: 0x04000342 RID: 834
		[Range(0.1f, 0.9f)]
		public float analogMoveThreshold = 0.5f;

		// Token: 0x04000343 RID: 835
		public float moveRepeatFirstDuration = 0.8f;

		// Token: 0x04000344 RID: 836
		public float moveRepeatDelayDuration = 0.1f;

		// Token: 0x04000345 RID: 837
		[FormerlySerializedAs("allowMobileDevice")]
		public bool forceModuleActive;

		// Token: 0x04000346 RID: 838
		public bool allowMouseInput = true;

		// Token: 0x04000347 RID: 839
		public bool focusOnMouseHover;

		// Token: 0x04000348 RID: 840
		public bool allowTouchInput = true;

		// Token: 0x04000349 RID: 841
		private InputDevice inputDevice;

		// Token: 0x0400034A RID: 842
		private Vector3 thisMousePosition;

		// Token: 0x0400034B RID: 843
		private Vector3 lastMousePosition;

		// Token: 0x0400034C RID: 844
		private Vector2 thisVectorState;

		// Token: 0x0400034D RID: 845
		private Vector2 lastVectorState;

		// Token: 0x0400034E RID: 846
		private bool thisSubmitState;

		// Token: 0x0400034F RID: 847
		private bool lastSubmitState;

		// Token: 0x04000350 RID: 848
		private bool thisCancelState;

		// Token: 0x04000351 RID: 849
		private bool lastCancelState;

		// Token: 0x04000352 RID: 850
		private bool moveWasRepeated;

		// Token: 0x04000353 RID: 851
		private float nextMoveRepeatTime;

		// Token: 0x04000354 RID: 852
		private TwoAxisInputControl direction;

		// Token: 0x0200021B RID: 539
		public enum Button
		{
			// Token: 0x040004C1 RID: 1217
			Action1 = 19,
			// Token: 0x040004C2 RID: 1218
			Action2,
			// Token: 0x040004C3 RID: 1219
			Action3,
			// Token: 0x040004C4 RID: 1220
			Action4
		}
	}
}
