using System;

namespace InControl
{
	// Token: 0x02000036 RID: 54
	public enum InputControlType
	{
		// Token: 0x040001C9 RID: 457
		None,
		// Token: 0x040001CA RID: 458
		LeftStickUp,
		// Token: 0x040001CB RID: 459
		LeftStickDown,
		// Token: 0x040001CC RID: 460
		LeftStickLeft,
		// Token: 0x040001CD RID: 461
		LeftStickRight,
		// Token: 0x040001CE RID: 462
		LeftStickButton,
		// Token: 0x040001CF RID: 463
		RightStickUp,
		// Token: 0x040001D0 RID: 464
		RightStickDown,
		// Token: 0x040001D1 RID: 465
		RightStickLeft,
		// Token: 0x040001D2 RID: 466
		RightStickRight,
		// Token: 0x040001D3 RID: 467
		RightStickButton,
		// Token: 0x040001D4 RID: 468
		DPadUp,
		// Token: 0x040001D5 RID: 469
		DPadDown,
		// Token: 0x040001D6 RID: 470
		DPadLeft,
		// Token: 0x040001D7 RID: 471
		DPadRight,
		// Token: 0x040001D8 RID: 472
		LeftTrigger,
		// Token: 0x040001D9 RID: 473
		RightTrigger,
		// Token: 0x040001DA RID: 474
		LeftBumper,
		// Token: 0x040001DB RID: 475
		RightBumper,
		// Token: 0x040001DC RID: 476
		Action1,
		// Token: 0x040001DD RID: 477
		Action2,
		// Token: 0x040001DE RID: 478
		Action3,
		// Token: 0x040001DF RID: 479
		Action4,
		// Token: 0x040001E0 RID: 480
		Action5,
		// Token: 0x040001E1 RID: 481
		Action6,
		// Token: 0x040001E2 RID: 482
		Action7,
		// Token: 0x040001E3 RID: 483
		Action8,
		// Token: 0x040001E4 RID: 484
		Action9,
		// Token: 0x040001E5 RID: 485
		Action10,
		// Token: 0x040001E6 RID: 486
		Action11,
		// Token: 0x040001E7 RID: 487
		Action12,
		// Token: 0x040001E8 RID: 488
		Back = 100,
		// Token: 0x040001E9 RID: 489
		Start,
		// Token: 0x040001EA RID: 490
		Select,
		// Token: 0x040001EB RID: 491
		System,
		// Token: 0x040001EC RID: 492
		Options,
		// Token: 0x040001ED RID: 493
		Pause,
		// Token: 0x040001EE RID: 494
		Menu,
		// Token: 0x040001EF RID: 495
		Share,
		// Token: 0x040001F0 RID: 496
		Home,
		// Token: 0x040001F1 RID: 497
		View,
		// Token: 0x040001F2 RID: 498
		Power,
		// Token: 0x040001F3 RID: 499
		Capture,
		// Token: 0x040001F4 RID: 500
		Assistant,
		// Token: 0x040001F5 RID: 501
		Plus,
		// Token: 0x040001F6 RID: 502
		Minus,
		// Token: 0x040001F7 RID: 503
		Create,
		// Token: 0x040001F8 RID: 504
		Mute,
		// Token: 0x040001F9 RID: 505
		Guide,
		// Token: 0x040001FA RID: 506
		PedalLeft = 150,
		// Token: 0x040001FB RID: 507
		PedalRight,
		// Token: 0x040001FC RID: 508
		PedalMiddle,
		// Token: 0x040001FD RID: 509
		GearUp,
		// Token: 0x040001FE RID: 510
		GearDown,
		// Token: 0x040001FF RID: 511
		Pitch = 200,
		// Token: 0x04000200 RID: 512
		Roll,
		// Token: 0x04000201 RID: 513
		Yaw,
		// Token: 0x04000202 RID: 514
		PitchUp,
		// Token: 0x04000203 RID: 515
		PitchDown,
		// Token: 0x04000204 RID: 516
		RollLeft,
		// Token: 0x04000205 RID: 517
		RollRight,
		// Token: 0x04000206 RID: 518
		YawLeft,
		// Token: 0x04000207 RID: 519
		YawRight,
		// Token: 0x04000208 RID: 520
		ThrottleUp,
		// Token: 0x04000209 RID: 521
		ThrottleDown,
		// Token: 0x0400020A RID: 522
		ThrottleLeft,
		// Token: 0x0400020B RID: 523
		ThrottleRight,
		// Token: 0x0400020C RID: 524
		POVUp,
		// Token: 0x0400020D RID: 525
		POVDown,
		// Token: 0x0400020E RID: 526
		POVLeft,
		// Token: 0x0400020F RID: 527
		POVRight,
		// Token: 0x04000210 RID: 528
		TiltX = 250,
		// Token: 0x04000211 RID: 529
		TiltY,
		// Token: 0x04000212 RID: 530
		TiltZ,
		// Token: 0x04000213 RID: 531
		GyroscopeX = 250,
		// Token: 0x04000214 RID: 532
		GyroscopeY,
		// Token: 0x04000215 RID: 533
		GyroscopeZ,
		// Token: 0x04000216 RID: 534
		AccelerometerX,
		// Token: 0x04000217 RID: 535
		AccelerometerY,
		// Token: 0x04000218 RID: 536
		AccelerometerZ,
		// Token: 0x04000219 RID: 537
		ScrollWheel,
		// Token: 0x0400021A RID: 538
		[Obsolete("Use InputControlType.TouchPadButton instead.", true)]
		TouchPadTap,
		// Token: 0x0400021B RID: 539
		TouchPadButton,
		// Token: 0x0400021C RID: 540
		TouchPadXAxis,
		// Token: 0x0400021D RID: 541
		TouchPadYAxis,
		// Token: 0x0400021E RID: 542
		LeftSL,
		// Token: 0x0400021F RID: 543
		LeftSR,
		// Token: 0x04000220 RID: 544
		RightSL,
		// Token: 0x04000221 RID: 545
		RightSR,
		// Token: 0x04000222 RID: 546
		Paddle1,
		// Token: 0x04000223 RID: 547
		Paddle2,
		// Token: 0x04000224 RID: 548
		Paddle3,
		// Token: 0x04000225 RID: 549
		Paddle4,
		// Token: 0x04000226 RID: 550
		Command = 300,
		// Token: 0x04000227 RID: 551
		LeftStickX,
		// Token: 0x04000228 RID: 552
		LeftStickY,
		// Token: 0x04000229 RID: 553
		RightStickX,
		// Token: 0x0400022A RID: 554
		RightStickY,
		// Token: 0x0400022B RID: 555
		DPadX,
		// Token: 0x0400022C RID: 556
		DPadY,
		// Token: 0x0400022D RID: 557
		LeftCommand,
		// Token: 0x0400022E RID: 558
		RightCommand,
		// Token: 0x0400022F RID: 559
		Analog0 = 400,
		// Token: 0x04000230 RID: 560
		Analog1,
		// Token: 0x04000231 RID: 561
		Analog2,
		// Token: 0x04000232 RID: 562
		Analog3,
		// Token: 0x04000233 RID: 563
		Analog4,
		// Token: 0x04000234 RID: 564
		Analog5,
		// Token: 0x04000235 RID: 565
		Analog6,
		// Token: 0x04000236 RID: 566
		Analog7,
		// Token: 0x04000237 RID: 567
		Analog8,
		// Token: 0x04000238 RID: 568
		Analog9,
		// Token: 0x04000239 RID: 569
		Analog10,
		// Token: 0x0400023A RID: 570
		Analog11,
		// Token: 0x0400023B RID: 571
		Analog12,
		// Token: 0x0400023C RID: 572
		Analog13,
		// Token: 0x0400023D RID: 573
		Analog14,
		// Token: 0x0400023E RID: 574
		Analog15,
		// Token: 0x0400023F RID: 575
		Analog16,
		// Token: 0x04000240 RID: 576
		Analog17,
		// Token: 0x04000241 RID: 577
		Analog18,
		// Token: 0x04000242 RID: 578
		Analog19,
		// Token: 0x04000243 RID: 579
		Button0 = 500,
		// Token: 0x04000244 RID: 580
		Button1,
		// Token: 0x04000245 RID: 581
		Button2,
		// Token: 0x04000246 RID: 582
		Button3,
		// Token: 0x04000247 RID: 583
		Button4,
		// Token: 0x04000248 RID: 584
		Button5,
		// Token: 0x04000249 RID: 585
		Button6,
		// Token: 0x0400024A RID: 586
		Button7,
		// Token: 0x0400024B RID: 587
		Button8,
		// Token: 0x0400024C RID: 588
		Button9,
		// Token: 0x0400024D RID: 589
		Button10,
		// Token: 0x0400024E RID: 590
		Button11,
		// Token: 0x0400024F RID: 591
		Button12,
		// Token: 0x04000250 RID: 592
		Button13,
		// Token: 0x04000251 RID: 593
		Button14,
		// Token: 0x04000252 RID: 594
		Button15,
		// Token: 0x04000253 RID: 595
		Button16,
		// Token: 0x04000254 RID: 596
		Button17,
		// Token: 0x04000255 RID: 597
		Button18,
		// Token: 0x04000256 RID: 598
		Button19,
		// Token: 0x04000257 RID: 599
		Button20,
		// Token: 0x04000258 RID: 600
		Button21,
		// Token: 0x04000259 RID: 601
		Button22,
		// Token: 0x0400025A RID: 602
		Button23,
		// Token: 0x0400025B RID: 603
		Button24,
		// Token: 0x0400025C RID: 604
		Button25,
		// Token: 0x0400025D RID: 605
		Button26,
		// Token: 0x0400025E RID: 606
		Button27,
		// Token: 0x0400025F RID: 607
		Button28,
		// Token: 0x04000260 RID: 608
		Button29,
		// Token: 0x04000261 RID: 609
		Count
	}
}
