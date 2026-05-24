using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;

namespace InControl
{
	// Token: 0x02000026 RID: 38
	public abstract class PlayerActionSet
	{
		// Token: 0x17000047 RID: 71
		// (get) Token: 0x06000123 RID: 291 RVA: 0x00005DAE File Offset: 0x00003FAE
		// (set) Token: 0x06000124 RID: 292 RVA: 0x00005DB6 File Offset: 0x00003FB6
		public InputDevice Device { get; set; }

		// Token: 0x17000048 RID: 72
		// (get) Token: 0x06000125 RID: 293 RVA: 0x00005DBF File Offset: 0x00003FBF
		// (set) Token: 0x06000126 RID: 294 RVA: 0x00005DC7 File Offset: 0x00003FC7
		public List<InputDevice> IncludeDevices { get; private set; }

		// Token: 0x17000049 RID: 73
		// (get) Token: 0x06000127 RID: 295 RVA: 0x00005DD0 File Offset: 0x00003FD0
		// (set) Token: 0x06000128 RID: 296 RVA: 0x00005DD8 File Offset: 0x00003FD8
		public List<InputDevice> ExcludeDevices { get; private set; }

		// Token: 0x1700004A RID: 74
		// (get) Token: 0x06000129 RID: 297 RVA: 0x00005DE1 File Offset: 0x00003FE1
		// (set) Token: 0x0600012A RID: 298 RVA: 0x00005DE9 File Offset: 0x00003FE9
		public ReadOnlyCollection<PlayerAction> Actions { get; private set; }

		// Token: 0x1700004B RID: 75
		// (get) Token: 0x0600012B RID: 299 RVA: 0x00005DF2 File Offset: 0x00003FF2
		// (set) Token: 0x0600012C RID: 300 RVA: 0x00005DFA File Offset: 0x00003FFA
		public ulong UpdateTick { get; protected set; }

		// Token: 0x14000003 RID: 3
		// (add) Token: 0x0600012D RID: 301 RVA: 0x00005E04 File Offset: 0x00004004
		// (remove) Token: 0x0600012E RID: 302 RVA: 0x00005E3C File Offset: 0x0000403C
		public event Action<BindingSourceType> OnLastInputTypeChanged;

		// Token: 0x1700004C RID: 76
		// (get) Token: 0x0600012F RID: 303 RVA: 0x00005E71 File Offset: 0x00004071
		// (set) Token: 0x06000130 RID: 304 RVA: 0x00005E79 File Offset: 0x00004079
		public bool Enabled { get; set; }

		// Token: 0x1700004D RID: 77
		// (get) Token: 0x06000131 RID: 305 RVA: 0x00005E82 File Offset: 0x00004082
		// (set) Token: 0x06000132 RID: 306 RVA: 0x00005E8A File Offset: 0x0000408A
		public bool PreventInputWhileListeningForBinding { get; set; }

		// Token: 0x1700004E RID: 78
		// (get) Token: 0x06000133 RID: 307 RVA: 0x00005E93 File Offset: 0x00004093
		// (set) Token: 0x06000134 RID: 308 RVA: 0x00005E9B File Offset: 0x0000409B
		public object UserData { get; set; }

		// Token: 0x06000135 RID: 309 RVA: 0x00005EA4 File Offset: 0x000040A4
		protected PlayerActionSet()
		{
			this.Enabled = true;
			this.PreventInputWhileListeningForBinding = true;
			this.Device = null;
			this.IncludeDevices = new List<InputDevice>();
			this.ExcludeDevices = new List<InputDevice>();
			this.Actions = new ReadOnlyCollection<PlayerAction>(this.actions);
			InputManager.AttachPlayerActionSet(this);
		}

		// Token: 0x06000136 RID: 310 RVA: 0x00005F30 File Offset: 0x00004130
		public void Destroy()
		{
			this.OnLastInputTypeChanged = null;
			InputManager.DetachPlayerActionSet(this);
		}

		// Token: 0x06000137 RID: 311 RVA: 0x00005F3F File Offset: 0x0000413F
		protected PlayerAction CreatePlayerAction(string name)
		{
			return new PlayerAction(name, this);
		}

		// Token: 0x06000138 RID: 312 RVA: 0x00005F48 File Offset: 0x00004148
		internal void AddPlayerAction(PlayerAction action)
		{
			action.Device = this.FindActiveDevice();
			if (this.actionsByName.ContainsKey(action.Name))
			{
				throw new InControlException("Action '" + action.Name + "' already exists in this set.");
			}
			this.actions.Add(action);
			this.actionsByName.Add(action.Name, action);
		}

		// Token: 0x06000139 RID: 313 RVA: 0x00005FB0 File Offset: 0x000041B0
		protected PlayerOneAxisAction CreateOneAxisPlayerAction(PlayerAction negativeAction, PlayerAction positiveAction)
		{
			PlayerOneAxisAction playerOneAxisAction = new PlayerOneAxisAction(negativeAction, positiveAction);
			this.oneAxisActions.Add(playerOneAxisAction);
			return playerOneAxisAction;
		}

		// Token: 0x0600013A RID: 314 RVA: 0x00005FD4 File Offset: 0x000041D4
		protected PlayerTwoAxisAction CreateTwoAxisPlayerAction(PlayerAction negativeXAction, PlayerAction positiveXAction, PlayerAction negativeYAction, PlayerAction positiveYAction)
		{
			PlayerTwoAxisAction playerTwoAxisAction = new PlayerTwoAxisAction(negativeXAction, positiveXAction, negativeYAction, positiveYAction);
			this.twoAxisActions.Add(playerTwoAxisAction);
			return playerTwoAxisAction;
		}

		// Token: 0x1700004F RID: 79
		public PlayerAction this[string actionName]
		{
			get
			{
				PlayerAction playerAction;
				if (this.actionsByName.TryGetValue(actionName, out playerAction))
				{
					return playerAction;
				}
				throw new KeyNotFoundException("Action '" + actionName + "' does not exist in this action set.");
			}
		}

		// Token: 0x0600013C RID: 316 RVA: 0x00006030 File Offset: 0x00004230
		public PlayerAction GetPlayerActionByName(string actionName)
		{
			PlayerAction playerAction;
			if (this.actionsByName.TryGetValue(actionName, out playerAction))
			{
				return playerAction;
			}
			return null;
		}

		// Token: 0x0600013D RID: 317 RVA: 0x00006050 File Offset: 0x00004250
		internal void Update(ulong updateTick, float deltaTime)
		{
			InputDevice inputDevice = this.Device ?? this.FindActiveDevice();
			BindingSourceType bindingSourceType = this.LastInputType;
			ulong num = this.LastInputTypeChangedTick;
			InputDeviceClass inputDeviceClass = this.LastDeviceClass;
			InputDeviceStyle inputDeviceStyle = this.LastDeviceStyle;
			int count = this.actions.Count;
			for (int i = 0; i < count; i++)
			{
				PlayerAction playerAction = this.actions[i];
				playerAction.Update(updateTick, deltaTime, inputDevice);
				if (playerAction.UpdateTick > this.UpdateTick)
				{
					this.UpdateTick = playerAction.UpdateTick;
					this.activeDevice = playerAction.ActiveDevice;
				}
				if (playerAction.LastInputTypeChangedTick > num)
				{
					bindingSourceType = playerAction.LastInputType;
					num = playerAction.LastInputTypeChangedTick;
					inputDeviceClass = playerAction.LastDeviceClass;
					inputDeviceStyle = playerAction.LastDeviceStyle;
				}
			}
			int count2 = this.oneAxisActions.Count;
			for (int j = 0; j < count2; j++)
			{
				this.oneAxisActions[j].Update(updateTick, deltaTime);
			}
			int count3 = this.twoAxisActions.Count;
			for (int k = 0; k < count3; k++)
			{
				this.twoAxisActions[k].Update(updateTick, deltaTime);
			}
			if (num > this.LastInputTypeChangedTick)
			{
				bool flag = bindingSourceType != this.LastInputType;
				this.LastInputType = bindingSourceType;
				this.LastInputTypeChangedTick = num;
				this.LastDeviceClass = inputDeviceClass;
				this.LastDeviceStyle = inputDeviceStyle;
				if (this.OnLastInputTypeChanged != null && flag)
				{
					this.OnLastInputTypeChanged(bindingSourceType);
				}
			}
		}

		// Token: 0x0600013E RID: 318 RVA: 0x000061C8 File Offset: 0x000043C8
		public void Reset()
		{
			int count = this.actions.Count;
			for (int i = 0; i < count; i++)
			{
				this.actions[i].ResetBindings();
			}
		}

		// Token: 0x0600013F RID: 319 RVA: 0x00006200 File Offset: 0x00004400
		private InputDevice FindActiveDevice()
		{
			bool flag = this.IncludeDevices.Count > 0;
			bool flag2 = this.ExcludeDevices.Count > 0;
			if (flag || flag2)
			{
				InputDevice inputDevice = InputDevice.Null;
				int count = InputManager.Devices.Count;
				for (int i = 0; i < count; i++)
				{
					InputDevice inputDevice2 = InputManager.Devices[i];
					if (inputDevice2 != inputDevice && inputDevice2.LastInputAfter(inputDevice) && !inputDevice2.Passive && (!flag2 || !this.ExcludeDevices.Contains(inputDevice2)) && (!flag || this.IncludeDevices.Contains(inputDevice2)))
					{
						inputDevice = inputDevice2;
					}
				}
				return inputDevice;
			}
			return InputManager.ActiveDevice;
		}

		// Token: 0x06000140 RID: 320 RVA: 0x000062A8 File Offset: 0x000044A8
		public void ClearInputState()
		{
			int count = this.actions.Count;
			for (int i = 0; i < count; i++)
			{
				this.actions[i].ClearInputState();
			}
			int count2 = this.oneAxisActions.Count;
			for (int j = 0; j < count2; j++)
			{
				this.oneAxisActions[j].ClearInputState();
			}
			int count3 = this.twoAxisActions.Count;
			for (int k = 0; k < count3; k++)
			{
				this.twoAxisActions[k].ClearInputState();
			}
		}

		// Token: 0x06000141 RID: 321 RVA: 0x0000633C File Offset: 0x0000453C
		public bool HasBinding(BindingSource binding)
		{
			if (binding == null)
			{
				return false;
			}
			int count = this.actions.Count;
			for (int i = 0; i < count; i++)
			{
				if (this.actions[i].HasBinding(binding))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000142 RID: 322 RVA: 0x00006384 File Offset: 0x00004584
		public void RemoveBinding(BindingSource binding)
		{
			if (binding == null)
			{
				return;
			}
			int count = this.actions.Count;
			for (int i = 0; i < count; i++)
			{
				this.actions[i].RemoveBinding(binding);
			}
		}

		// Token: 0x17000050 RID: 80
		// (get) Token: 0x06000143 RID: 323 RVA: 0x000063C5 File Offset: 0x000045C5
		public bool IsListeningForBinding
		{
			get
			{
				return this.listenWithAction != null;
			}
		}

		// Token: 0x17000051 RID: 81
		// (get) Token: 0x06000144 RID: 324 RVA: 0x000063D0 File Offset: 0x000045D0
		// (set) Token: 0x06000145 RID: 325 RVA: 0x000063D8 File Offset: 0x000045D8
		public BindingListenOptions ListenOptions
		{
			get
			{
				return this.listenOptions;
			}
			set
			{
				this.listenOptions = value ?? new BindingListenOptions();
			}
		}

		// Token: 0x17000052 RID: 82
		// (get) Token: 0x06000146 RID: 326 RVA: 0x000063EA File Offset: 0x000045EA
		public InputDevice ActiveDevice
		{
			get
			{
				return this.activeDevice ?? InputDevice.Null;
			}
		}

		// Token: 0x06000147 RID: 327 RVA: 0x000063FC File Offset: 0x000045FC
		public byte[] SaveData()
		{
			byte[] array;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				using (BinaryWriter binaryWriter = new BinaryWriter(memoryStream, Encoding.UTF8))
				{
					binaryWriter.Write(66);
					binaryWriter.Write(73);
					binaryWriter.Write(78);
					binaryWriter.Write(68);
					binaryWriter.Write(2);
					int count = this.actions.Count;
					binaryWriter.Write(count);
					for (int i = 0; i < count; i++)
					{
						this.actions[i].Save(binaryWriter);
					}
				}
				array = memoryStream.ToArray();
			}
			return array;
		}

		// Token: 0x06000148 RID: 328 RVA: 0x000064B4 File Offset: 0x000046B4
		public void LoadData(byte[] data)
		{
			if (data == null)
			{
				return;
			}
			try
			{
				using (MemoryStream memoryStream = new MemoryStream(data))
				{
					using (BinaryReader binaryReader = new BinaryReader(memoryStream))
					{
						if (binaryReader.ReadUInt32() != 1145981250U)
						{
							throw new Exception("Unknown data format.");
						}
						ushort num = binaryReader.ReadUInt16();
						if (num < 1 || num > 2)
						{
							throw new Exception("Unknown data format version: " + num.ToString());
						}
						int num2 = binaryReader.ReadInt32();
						for (int i = 0; i < num2; i++)
						{
							PlayerAction playerAction;
							if (this.actionsByName.TryGetValue(binaryReader.ReadString(), out playerAction))
							{
								playerAction.Load(binaryReader, num);
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				Logger.LogError("Provided state could not be loaded:\n" + ex.Message);
				this.Reset();
			}
		}

		// Token: 0x06000149 RID: 329 RVA: 0x000065AC File Offset: 0x000047AC
		public string Save()
		{
			return Convert.ToBase64String(this.SaveData());
		}

		// Token: 0x0600014A RID: 330 RVA: 0x000065BC File Offset: 0x000047BC
		public void Load(string data)
		{
			if (data == null)
			{
				return;
			}
			try
			{
				this.LoadData(Convert.FromBase64String(data));
			}
			catch (Exception ex)
			{
				Logger.LogError("Provided state could not be loaded:\n" + ex.Message);
				this.Reset();
			}
		}

		// Token: 0x0400016D RID: 365
		public BindingSourceType LastInputType;

		// Token: 0x0400016F RID: 367
		public ulong LastInputTypeChangedTick;

		// Token: 0x04000170 RID: 368
		public InputDeviceClass LastDeviceClass;

		// Token: 0x04000171 RID: 369
		public InputDeviceStyle LastDeviceStyle;

		// Token: 0x04000175 RID: 373
		private List<PlayerAction> actions = new List<PlayerAction>();

		// Token: 0x04000176 RID: 374
		private List<PlayerOneAxisAction> oneAxisActions = new List<PlayerOneAxisAction>();

		// Token: 0x04000177 RID: 375
		private List<PlayerTwoAxisAction> twoAxisActions = new List<PlayerTwoAxisAction>();

		// Token: 0x04000178 RID: 376
		private Dictionary<string, PlayerAction> actionsByName = new Dictionary<string, PlayerAction>();

		// Token: 0x04000179 RID: 377
		private BindingListenOptions listenOptions = new BindingListenOptions();

		// Token: 0x0400017A RID: 378
		internal PlayerAction listenWithAction;

		// Token: 0x0400017B RID: 379
		private InputDevice activeDevice;

		// Token: 0x0400017C RID: 380
		private const ushort currentDataFormatVersion = 2;
	}
}
