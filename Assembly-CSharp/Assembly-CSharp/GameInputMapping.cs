using System;
using InControl;

// Token: 0x02000031 RID: 49
public class GameInputMapping : PlayerActionSet
{
	// Token: 0x06000197 RID: 407 RVA: 0x0000E218 File Offset: 0x0000C418
	public GameInputMapping(bool alt, bool wasd)
	{
		InputControlType inputControlType = InputControlType.Action1;
		InputControlType inputControlType2 = InputControlType.Action2;
		this.moveLeft = base.CreatePlayerAction("Move Left");
		this.moveLeft.AddDefaultBinding(new Key[] { wasd ? Key.A : Key.LeftArrow });
		this.moveLeft.AddDefaultBinding(InputControlType.LeftStickLeft);
		this.moveLeft.AddDefaultBinding(InputControlType.DPadLeft);
		this.moveRight = base.CreatePlayerAction("Move Right");
		this.moveRight.AddDefaultBinding(new Key[] { wasd ? Key.D : Key.RightArrow });
		this.moveRight.AddDefaultBinding(InputControlType.LeftStickRight);
		this.moveRight.AddDefaultBinding(InputControlType.DPadRight);
		this.moveDown = base.CreatePlayerAction("Move Down");
		this.moveDown.AddDefaultBinding(new Key[] { wasd ? Key.S : Key.DownArrow });
		this.moveDown.AddDefaultBinding(InputControlType.LeftStickDown);
		this.moveDown.AddDefaultBinding(InputControlType.DPadDown);
		this.moveUp = base.CreatePlayerAction("Move Up");
		this.moveUp.AddDefaultBinding(new Key[] { wasd ? Key.W : Key.UpArrow });
		this.moveUp.AddDefaultBinding(InputControlType.LeftStickUp);
		this.moveUp.AddDefaultBinding(InputControlType.DPadUp);
		this.moveLeftRight = base.CreateOneAxisPlayerAction(this.moveLeft, this.moveRight);
		this.moveUpDown = base.CreateOneAxisPlayerAction(this.moveDown, this.moveUp);
		this.move = base.CreateTwoAxisPlayerAction(this.moveLeft, this.moveRight, this.moveDown, this.moveUp);
		this.jump = base.CreatePlayerAction("Jump");
		this.jump.AddDefaultBinding(new Key[] { Key.Space });
		this.jump.AddDefaultBinding(alt ? inputControlType : InputControlType.Action4);
		this.jumpSpecial = base.CreatePlayerAction("Special jump");
		this.jumpSpecial.AddDefaultBinding(new Key[] { Key.UpArrow });
		if (wasd)
		{
			this.jumpSpecial.AddDefaultBinding(new Key[] { Key.W });
		}
		this.jumpSpecial.AddDefaultBinding(alt ? InputControlType.Action4 : InputControlType.Action3);
		this.sprint = base.CreatePlayerAction("Sprint");
		this.sprint.AddDefaultBinding(new Key[] { Key.Shift });
		this.sprint.AddDefaultBinding(inputControlType2);
		this.rest = base.CreatePlayerAction("Rest");
		this.rest.AddDefaultBinding(new Key[] { Key.R });
		this.rest.AddDefaultBinding(alt ? InputControlType.Action4 : InputControlType.Action3);
		this.slipRecover = base.CreatePlayerAction("Slip Recover");
		this.slipRecover.AddDefaultBinding(new Key[] { Key.Space });
		this.slipRecover.AddDefaultBinding(alt ? InputControlType.Action3 : InputControlType.Action4);
		this.zoomOut = base.CreatePlayerAction("Zoom out");
		this.zoomOut.AddDefaultBinding(new Key[] { wasd ? Key.DownArrow : Key.Z });
		this.zoomOut.AddDefaultBinding(InputControlType.LeftTrigger);
		this.zoomIn = base.CreatePlayerAction("Zoom in");
		this.zoomIn.AddDefaultBinding(new Key[] { wasd ? Key.UpArrow : Key.A });
		this.zoomIn.AddDefaultBinding(InputControlType.RightTrigger);
		this.lookFurther = base.CreatePlayerAction("Look further");
		if (wasd)
		{
			this.lookFurther.AddDefaultBinding(new Key[] { Key.Shift });
		}
		this.lookFurther.AddDefaultBinding(new Key[] { Key.Return });
		this.lookFurther.AddDefaultBinding(inputControlType);
		this.telescope = base.CreatePlayerAction("Telescope");
		this.telescope.AddDefaultBinding(new Key[] { Key.Return });
		this.telescope.AddDefaultBinding(inputControlType);
		this.selectChoice = base.CreatePlayerAction("Select choice");
		this.selectChoice.AddDefaultBinding(new Key[] { Key.Return });
		this.selectChoice.AddDefaultBinding(new Key[] { Key.F });
		this.selectChoice.AddDefaultBinding(alt ? InputControlType.Action3 : inputControlType);
		this.skipDialogue = base.CreatePlayerAction("Skip dialogue");
		this.skipDialogue.AddDefaultBinding(new Key[] { Key.Return });
		this.skipDialogue.AddDefaultBinding(inputControlType2);
		this.hideChoices = base.CreatePlayerAction("Hide choices");
		this.hideChoices.AddDefaultBinding(new Key[] { Key.Escape });
		this.hideChoices.AddDefaultBinding(new Key[] { Key.Delete });
		this.hideChoices.AddDefaultBinding(inputControlType2);
		this.selectMenuItem = base.CreatePlayerAction("Select");
		this.selectMenuItem.AddDefaultBinding(new Key[] { Key.Return });
		this.selectMenuItem.AddDefaultBinding(inputControlType);
		this.throwStone = base.CreatePlayerAction("Throw Stone");
		this.throwStone.AddDefaultBinding(new Key[] { Key.Return });
		this.throwStone.AddDefaultBinding(new Key[] { Key.F });
		this.throwStone.AddDefaultBinding(inputControlType);
		this.back = base.CreatePlayerAction("Back");
		this.back.AddDefaultBinding(new Key[] { Key.Escape });
		this.back.AddDefaultBinding(inputControlType2);
		this.panLeft = base.CreatePlayerAction("Pan Left");
		this.panLeft.AddDefaultBinding(new Key[] { wasd ? Key.A : Key.LeftArrow });
		this.panLeft.AddDefaultBinding(InputControlType.LeftStickLeft);
		this.panRight = base.CreatePlayerAction("Pan Right");
		this.panRight.AddDefaultBinding(new Key[] { wasd ? Key.D : Key.RightArrow });
		this.panRight.AddDefaultBinding(InputControlType.LeftStickRight);
		this.panDown = base.CreatePlayerAction("Pan Down");
		this.panDown.AddDefaultBinding(new Key[] { wasd ? Key.S : Key.DownArrow });
		this.panDown.AddDefaultBinding(InputControlType.LeftStickDown);
		this.panUp = base.CreatePlayerAction("Pan Up");
		this.panUp.AddDefaultBinding(new Key[] { wasd ? Key.W : Key.UpArrow });
		this.panUp.AddDefaultBinding(InputControlType.LeftStickUp);
		this.pan = base.CreateTwoAxisPlayerAction(this.panLeft, this.panRight, this.panDown, this.panUp);
		this.resetCameraToPlayer = base.CreatePlayerAction("Reset view on peak");
		this.resetCameraToPlayer.AddDefaultBinding(InputControlType.Action3);
		this.resetCameraToPlayer.AddDefaultBinding(new Key[] { Key.T });
		this.selectUp = base.CreatePlayerAction("Select Up");
		this.selectUp.AddDefaultBinding(new Key[] { Key.UpArrow });
		if (wasd)
		{
			this.selectUp.AddDefaultBinding(new Key[] { Key.W });
		}
		this.selectUp.AddDefaultBinding(InputControlType.DPadUp);
		this.selectUp.AddDefaultBinding(InputControlType.LeftStickUp);
		this.selectUp.StateThreshold = 0.7f;
		this.selectDown = base.CreatePlayerAction("Select Down");
		this.selectDown.AddDefaultBinding(new Key[] { Key.DownArrow });
		if (wasd)
		{
			this.selectDown.AddDefaultBinding(new Key[] { Key.S });
		}
		this.selectDown.AddDefaultBinding(InputControlType.DPadDown);
		this.selectDown.AddDefaultBinding(InputControlType.LeftStickDown);
		this.selectDown.StateThreshold = 0.7f;
		this.selectLeft = base.CreatePlayerAction("Select Left");
		this.selectLeft.AddDefaultBinding(new Key[] { Key.LeftArrow });
		if (wasd)
		{
			this.selectLeft.AddDefaultBinding(new Key[] { Key.A });
		}
		this.selectLeft.AddDefaultBinding(InputControlType.DPadLeft);
		this.selectLeft.AddDefaultBinding(InputControlType.LeftStickLeft);
		this.selectLeft.StateThreshold = 0.7f;
		this.selectRight = base.CreatePlayerAction("Select right");
		this.selectRight.AddDefaultBinding(new Key[] { Key.RightArrow });
		if (wasd)
		{
			this.selectRight.AddDefaultBinding(new Key[] { Key.D });
		}
		this.selectRight.AddDefaultBinding(InputControlType.DPadRight);
		this.selectRight.AddDefaultBinding(InputControlType.LeftStickRight);
		this.selectRight.StateThreshold = 0.7f;
		this.showMaps = base.CreatePlayerAction("Show Maps");
		this.showMaps.AddDefaultBinding(new Key[] { Key.M });
		this.showMaps.AddDefaultBinding(InputControlType.Action4);
		this.prevMap = base.CreatePlayerAction("Previous Map");
		this.prevMap.AddDefaultBinding(new Key[] { Key.Q });
		this.prevMap.AddDefaultBinding(InputControlType.LeftBumper);
		this.nextMap = base.CreatePlayerAction("Next Map");
		this.nextMap.AddDefaultBinding(new Key[] { Key.E });
		this.nextMap.AddDefaultBinding(InputControlType.RightBumper);
		this.showJournal = base.CreatePlayerAction("Show Journal");
		this.showJournal.AddDefaultBinding(new Key[] { Key.Tab });
		this.showJournal.AddDefaultBinding(InputControlType.Plus);
		this.showJournal.AddDefaultBinding(InputControlType.Start);
		this.showJournal.AddDefaultBinding(InputControlType.Options);
		this.showJournal.AddDefaultBinding(InputControlType.Menu);
		this.showJournal.AddDefaultBinding(InputControlType.Pause);
		this.togglePhotoMode = base.CreatePlayerAction("Enter photo mode");
		this.takePhoto = base.CreatePlayerAction("Take photo");
		this.takePhoto.AddDefaultBinding(new Key[] { Key.Space });
		this.takePhoto.AddDefaultBinding(InputControlType.Action1);
		this.photoModeOptions = base.CreatePlayerAction("Toggle photo mode options");
		this.photoModeOptions.AddDefaultBinding(new Key[] { Key.Shift });
		this.photoModeOptions.AddDefaultBinding(InputControlType.Action4);
		this.revealPhoto = base.CreatePlayerAction("Reveal photo in explorer");
		this.revealPhoto.AddDefaultBinding(new Key[] { Key.Return });
		this.revealPhoto.AddDefaultBinding(InputControlType.Action3);
		this.photoModeToggleUI = base.CreatePlayerAction("Toggle photo mode UI");
		this.photoModeToggleUI.AddDefaultBinding(new Key[] { Key.Tab });
		this.photoModeToggleUI.AddDefaultBinding(InputControlType.RightBumper);
		this.prevSection = base.CreatePlayerAction("Previous Section");
		this.prevSection.AddDefaultBinding(new Key[] { Key.Q });
		this.prevSection.AddDefaultBinding(InputControlType.LeftBumper);
		this.nextSection = base.CreatePlayerAction("Next Section");
		this.nextSection.AddDefaultBinding(new Key[] { Key.E });
		this.nextSection.AddDefaultBinding(InputControlType.RightBumper);
		this.nums = new PlayerAction[5];
		this.nums[0] = base.CreatePlayerAction("Select One");
		this.nums[0].AddDefaultBinding(new Key[] { Key.Key1 });
		this.nums[1] = base.CreatePlayerAction("Select Two");
		this.nums[1].AddDefaultBinding(new Key[] { Key.Key2 });
		this.nums[2] = base.CreatePlayerAction("Select Three");
		this.nums[2].AddDefaultBinding(new Key[] { Key.Key3 });
		this.nums[3] = base.CreatePlayerAction("Select Four");
		this.nums[3].AddDefaultBinding(new Key[] { Key.Key4 });
		this.nums[4] = base.CreatePlayerAction("Select Five");
		this.nums[4].AddDefaultBinding(new Key[] { Key.Key5 });
		this.debugFlyToggle = base.CreatePlayerAction("debugFlyToggle");
		this.debugFlyToggle.AddDefaultBinding(new Key[] { Key.Y });
		this.debugFlyToggle.AddDefaultBinding(InputControlType.TouchPadButton);
		PlayerAction playerAction = base.CreatePlayerAction("Debug Fly Move Left");
		playerAction.AddDefaultBinding(new Key[] { Key.LeftArrow });
		playerAction.AddDefaultBinding(InputControlType.LeftStickLeft);
		PlayerAction playerAction2 = base.CreatePlayerAction("Debug Fly Move Right");
		playerAction2.AddDefaultBinding(new Key[] { Key.RightArrow });
		playerAction2.AddDefaultBinding(InputControlType.LeftStickRight);
		PlayerAction playerAction3 = base.CreatePlayerAction("Debug Fly Move Down");
		playerAction3.AddDefaultBinding(new Key[] { Key.DownArrow });
		playerAction3.AddDefaultBinding(InputControlType.LeftStickDown);
		PlayerAction playerAction4 = base.CreatePlayerAction("Debug Fly Move Up");
		playerAction4.AddDefaultBinding(new Key[] { Key.UpArrow });
		playerAction4.AddDefaultBinding(InputControlType.LeftStickUp);
		this.debugFlyMove = base.CreateTwoAxisPlayerAction(playerAction, playerAction2, playerAction3, playerAction4);
		PlayerAction playerAction5 = base.CreatePlayerAction("debugFlyIn");
		playerAction5.AddDefaultBinding(new Key[] { Key.A });
		playerAction5.AddDefaultBinding(InputControlType.LeftTrigger);
		PlayerAction playerAction6 = base.CreatePlayerAction("debugFlyOut");
		playerAction6.AddDefaultBinding(new Key[] { Key.Z });
		playerAction6.AddDefaultBinding(InputControlType.RightTrigger);
		this.debugFlyInOut = base.CreateOneAxisPlayerAction(playerAction5, playerAction6);
		this.debugFast = base.CreatePlayerAction("debugFast");
		this.debugFast.AddDefaultBinding(new Key[] { Key.Slash });
		this.debugFast.AddDefaultBinding(new Key[] { Key.QuestionMark });
		this.debugReset = base.CreatePlayerAction("debugReset");
		this.debugReset.AddDefaultBinding(new Key[] { Key.Hash });
		this.debugReset.AddDefaultBinding(new Key[] { Key.K });
		this.debugTestMenu = base.CreatePlayerAction("debug test menu");
		this.debugTestMenu.AddDefaultBinding(new Key[] { Key.Backslash });
		this.debugTestMenu.AddDefaultBinding(InputControlType.RightStickButton);
	}

	// Token: 0x04000221 RID: 545
	public PlayerTwoAxisAction move;

	// Token: 0x04000222 RID: 546
	public PlayerOneAxisAction moveLeftRight;

	// Token: 0x04000223 RID: 547
	public PlayerOneAxisAction moveUpDown;

	// Token: 0x04000224 RID: 548
	public PlayerAction moveLeft;

	// Token: 0x04000225 RID: 549
	public PlayerAction moveRight;

	// Token: 0x04000226 RID: 550
	public PlayerAction moveUp;

	// Token: 0x04000227 RID: 551
	public PlayerAction moveDown;

	// Token: 0x04000228 RID: 552
	public PlayerAction jump;

	// Token: 0x04000229 RID: 553
	public PlayerAction jumpSpecial;

	// Token: 0x0400022A RID: 554
	public PlayerAction sprint;

	// Token: 0x0400022B RID: 555
	public PlayerAction rest;

	// Token: 0x0400022C RID: 556
	public PlayerAction slipRecover;

	// Token: 0x0400022D RID: 557
	public PlayerAction zoomOut;

	// Token: 0x0400022E RID: 558
	public PlayerAction zoomIn;

	// Token: 0x0400022F RID: 559
	public PlayerAction lookFurther;

	// Token: 0x04000230 RID: 560
	public PlayerAction telescope;

	// Token: 0x04000231 RID: 561
	public PlayerAction selectChoice;

	// Token: 0x04000232 RID: 562
	public PlayerAction skipDialogue;

	// Token: 0x04000233 RID: 563
	public PlayerAction hideChoices;

	// Token: 0x04000234 RID: 564
	public PlayerAction selectMenuItem;

	// Token: 0x04000235 RID: 565
	public PlayerAction back;

	// Token: 0x04000236 RID: 566
	public PlayerAction panLeft;

	// Token: 0x04000237 RID: 567
	public PlayerAction panRight;

	// Token: 0x04000238 RID: 568
	public PlayerAction panDown;

	// Token: 0x04000239 RID: 569
	public PlayerAction panUp;

	// Token: 0x0400023A RID: 570
	public PlayerTwoAxisAction pan;

	// Token: 0x0400023B RID: 571
	public PlayerAction resetCameraToPlayer;

	// Token: 0x0400023C RID: 572
	public PlayerAction throwStone;

	// Token: 0x0400023D RID: 573
	public PlayerAction selectUp;

	// Token: 0x0400023E RID: 574
	public PlayerAction selectDown;

	// Token: 0x0400023F RID: 575
	public PlayerAction selectLeft;

	// Token: 0x04000240 RID: 576
	public PlayerAction selectRight;

	// Token: 0x04000241 RID: 577
	public PlayerAction showMaps;

	// Token: 0x04000242 RID: 578
	public PlayerAction prevMap;

	// Token: 0x04000243 RID: 579
	public PlayerAction nextMap;

	// Token: 0x04000244 RID: 580
	public PlayerAction prevSection;

	// Token: 0x04000245 RID: 581
	public PlayerAction nextSection;

	// Token: 0x04000246 RID: 582
	public PlayerAction showJournal;

	// Token: 0x04000247 RID: 583
	public PlayerAction[] nums;

	// Token: 0x04000248 RID: 584
	public PlayerAction togglePhotoMode;

	// Token: 0x04000249 RID: 585
	public PlayerAction takePhoto;

	// Token: 0x0400024A RID: 586
	public PlayerAction photoModeOptions;

	// Token: 0x0400024B RID: 587
	public PlayerAction revealPhoto;

	// Token: 0x0400024C RID: 588
	public PlayerAction photoModeToggleUI;

	// Token: 0x0400024D RID: 589
	public PlayerAction debugFlyToggle;

	// Token: 0x0400024E RID: 590
	public PlayerTwoAxisAction debugFlyMove;

	// Token: 0x0400024F RID: 591
	public PlayerOneAxisAction debugFlyInOut;

	// Token: 0x04000250 RID: 592
	public PlayerAction debugFast;

	// Token: 0x04000251 RID: 593
	public PlayerAction debugReset;

	// Token: 0x04000252 RID: 594
	public PlayerAction debugTestMenu;
}
