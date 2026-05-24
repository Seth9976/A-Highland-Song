using System;

namespace UnityEngine.Rendering.PostProcessing
{
	// Token: 0x02000063 RID: 99
	internal static class ShaderIDs
	{
		// Token: 0x040001B3 RID: 435
		internal static readonly int MainTex = Shader.PropertyToID("_MainTex");

		// Token: 0x040001B4 RID: 436
		internal static readonly int Jitter = Shader.PropertyToID("_Jitter");

		// Token: 0x040001B5 RID: 437
		internal static readonly int Sharpness = Shader.PropertyToID("_Sharpness");

		// Token: 0x040001B6 RID: 438
		internal static readonly int FinalBlendParameters = Shader.PropertyToID("_FinalBlendParameters");

		// Token: 0x040001B7 RID: 439
		internal static readonly int HistoryTex = Shader.PropertyToID("_HistoryTex");

		// Token: 0x040001B8 RID: 440
		internal static readonly int SMAA_Flip = Shader.PropertyToID("_SMAA_Flip");

		// Token: 0x040001B9 RID: 441
		internal static readonly int SMAA_Flop = Shader.PropertyToID("_SMAA_Flop");

		// Token: 0x040001BA RID: 442
		internal static readonly int AOParams = Shader.PropertyToID("_AOParams");

		// Token: 0x040001BB RID: 443
		internal static readonly int AOColor = Shader.PropertyToID("_AOColor");

		// Token: 0x040001BC RID: 444
		internal static readonly int OcclusionTexture1 = Shader.PropertyToID("_OcclusionTexture1");

		// Token: 0x040001BD RID: 445
		internal static readonly int OcclusionTexture2 = Shader.PropertyToID("_OcclusionTexture2");

		// Token: 0x040001BE RID: 446
		internal static readonly int SAOcclusionTexture = Shader.PropertyToID("_SAOcclusionTexture");

		// Token: 0x040001BF RID: 447
		internal static readonly int MSVOcclusionTexture = Shader.PropertyToID("_MSVOcclusionTexture");

		// Token: 0x040001C0 RID: 448
		internal static readonly int DepthCopy = Shader.PropertyToID("DepthCopy");

		// Token: 0x040001C1 RID: 449
		internal static readonly int LinearDepth = Shader.PropertyToID("LinearDepth");

		// Token: 0x040001C2 RID: 450
		internal static readonly int LowDepth1 = Shader.PropertyToID("LowDepth1");

		// Token: 0x040001C3 RID: 451
		internal static readonly int LowDepth2 = Shader.PropertyToID("LowDepth2");

		// Token: 0x040001C4 RID: 452
		internal static readonly int LowDepth3 = Shader.PropertyToID("LowDepth3");

		// Token: 0x040001C5 RID: 453
		internal static readonly int LowDepth4 = Shader.PropertyToID("LowDepth4");

		// Token: 0x040001C6 RID: 454
		internal static readonly int TiledDepth1 = Shader.PropertyToID("TiledDepth1");

		// Token: 0x040001C7 RID: 455
		internal static readonly int TiledDepth2 = Shader.PropertyToID("TiledDepth2");

		// Token: 0x040001C8 RID: 456
		internal static readonly int TiledDepth3 = Shader.PropertyToID("TiledDepth3");

		// Token: 0x040001C9 RID: 457
		internal static readonly int TiledDepth4 = Shader.PropertyToID("TiledDepth4");

		// Token: 0x040001CA RID: 458
		internal static readonly int Occlusion1 = Shader.PropertyToID("Occlusion1");

		// Token: 0x040001CB RID: 459
		internal static readonly int Occlusion2 = Shader.PropertyToID("Occlusion2");

		// Token: 0x040001CC RID: 460
		internal static readonly int Occlusion3 = Shader.PropertyToID("Occlusion3");

		// Token: 0x040001CD RID: 461
		internal static readonly int Occlusion4 = Shader.PropertyToID("Occlusion4");

		// Token: 0x040001CE RID: 462
		internal static readonly int Combined1 = Shader.PropertyToID("Combined1");

		// Token: 0x040001CF RID: 463
		internal static readonly int Combined2 = Shader.PropertyToID("Combined2");

		// Token: 0x040001D0 RID: 464
		internal static readonly int Combined3 = Shader.PropertyToID("Combined3");

		// Token: 0x040001D1 RID: 465
		internal static readonly int SSRResolveTemp = Shader.PropertyToID("_SSRResolveTemp");

		// Token: 0x040001D2 RID: 466
		internal static readonly int Noise = Shader.PropertyToID("_Noise");

		// Token: 0x040001D3 RID: 467
		internal static readonly int Test = Shader.PropertyToID("_Test");

		// Token: 0x040001D4 RID: 468
		internal static readonly int Resolve = Shader.PropertyToID("_Resolve");

		// Token: 0x040001D5 RID: 469
		internal static readonly int History = Shader.PropertyToID("_History");

		// Token: 0x040001D6 RID: 470
		internal static readonly int ViewMatrix = Shader.PropertyToID("_ViewMatrix");

		// Token: 0x040001D7 RID: 471
		internal static readonly int InverseViewMatrix = Shader.PropertyToID("_InverseViewMatrix");

		// Token: 0x040001D8 RID: 472
		internal static readonly int ScreenSpaceProjectionMatrix = Shader.PropertyToID("_ScreenSpaceProjectionMatrix");

		// Token: 0x040001D9 RID: 473
		internal static readonly int Params2 = Shader.PropertyToID("_Params2");

		// Token: 0x040001DA RID: 474
		internal static readonly int FogColor = Shader.PropertyToID("_FogColor");

		// Token: 0x040001DB RID: 475
		internal static readonly int FogParams = Shader.PropertyToID("_FogParams");

		// Token: 0x040001DC RID: 476
		internal static readonly int VelocityScale = Shader.PropertyToID("_VelocityScale");

		// Token: 0x040001DD RID: 477
		internal static readonly int MaxBlurRadius = Shader.PropertyToID("_MaxBlurRadius");

		// Token: 0x040001DE RID: 478
		internal static readonly int RcpMaxBlurRadius = Shader.PropertyToID("_RcpMaxBlurRadius");

		// Token: 0x040001DF RID: 479
		internal static readonly int VelocityTex = Shader.PropertyToID("_VelocityTex");

		// Token: 0x040001E0 RID: 480
		internal static readonly int Tile2RT = Shader.PropertyToID("_Tile2RT");

		// Token: 0x040001E1 RID: 481
		internal static readonly int Tile4RT = Shader.PropertyToID("_Tile4RT");

		// Token: 0x040001E2 RID: 482
		internal static readonly int Tile8RT = Shader.PropertyToID("_Tile8RT");

		// Token: 0x040001E3 RID: 483
		internal static readonly int TileMaxOffs = Shader.PropertyToID("_TileMaxOffs");

		// Token: 0x040001E4 RID: 484
		internal static readonly int TileMaxLoop = Shader.PropertyToID("_TileMaxLoop");

		// Token: 0x040001E5 RID: 485
		internal static readonly int TileVRT = Shader.PropertyToID("_TileVRT");

		// Token: 0x040001E6 RID: 486
		internal static readonly int NeighborMaxTex = Shader.PropertyToID("_NeighborMaxTex");

		// Token: 0x040001E7 RID: 487
		internal static readonly int LoopCount = Shader.PropertyToID("_LoopCount");

		// Token: 0x040001E8 RID: 488
		internal static readonly int DepthOfFieldTemp = Shader.PropertyToID("_DepthOfFieldTemp");

		// Token: 0x040001E9 RID: 489
		internal static readonly int DepthOfFieldTex = Shader.PropertyToID("_DepthOfFieldTex");

		// Token: 0x040001EA RID: 490
		internal static readonly int Distance = Shader.PropertyToID("_Distance");

		// Token: 0x040001EB RID: 491
		internal static readonly int LensCoeff = Shader.PropertyToID("_LensCoeff");

		// Token: 0x040001EC RID: 492
		internal static readonly int MaxCoC = Shader.PropertyToID("_MaxCoC");

		// Token: 0x040001ED RID: 493
		internal static readonly int RcpMaxCoC = Shader.PropertyToID("_RcpMaxCoC");

		// Token: 0x040001EE RID: 494
		internal static readonly int RcpAspect = Shader.PropertyToID("_RcpAspect");

		// Token: 0x040001EF RID: 495
		internal static readonly int CoCTex = Shader.PropertyToID("_CoCTex");

		// Token: 0x040001F0 RID: 496
		internal static readonly int TaaParams = Shader.PropertyToID("_TaaParams");

		// Token: 0x040001F1 RID: 497
		internal static readonly int AutoExposureTex = Shader.PropertyToID("_AutoExposureTex");

		// Token: 0x040001F2 RID: 498
		internal static readonly int HistogramBuffer = Shader.PropertyToID("_HistogramBuffer");

		// Token: 0x040001F3 RID: 499
		internal static readonly int Params = Shader.PropertyToID("_Params");

		// Token: 0x040001F4 RID: 500
		internal static readonly int ScaleOffsetRes = Shader.PropertyToID("_ScaleOffsetRes");

		// Token: 0x040001F5 RID: 501
		internal static readonly int BloomTex = Shader.PropertyToID("_BloomTex");

		// Token: 0x040001F6 RID: 502
		internal static readonly int SampleScale = Shader.PropertyToID("_SampleScale");

		// Token: 0x040001F7 RID: 503
		internal static readonly int Threshold = Shader.PropertyToID("_Threshold");

		// Token: 0x040001F8 RID: 504
		internal static readonly int ColorIntensity = Shader.PropertyToID("_ColorIntensity");

		// Token: 0x040001F9 RID: 505
		internal static readonly int Bloom_DirtTex = Shader.PropertyToID("_Bloom_DirtTex");

		// Token: 0x040001FA RID: 506
		internal static readonly int Bloom_Settings = Shader.PropertyToID("_Bloom_Settings");

		// Token: 0x040001FB RID: 507
		internal static readonly int Bloom_Color = Shader.PropertyToID("_Bloom_Color");

		// Token: 0x040001FC RID: 508
		internal static readonly int Bloom_DirtTileOffset = Shader.PropertyToID("_Bloom_DirtTileOffset");

		// Token: 0x040001FD RID: 509
		internal static readonly int ChromaticAberration_Amount = Shader.PropertyToID("_ChromaticAberration_Amount");

		// Token: 0x040001FE RID: 510
		internal static readonly int ChromaticAberration_SpectralLut = Shader.PropertyToID("_ChromaticAberration_SpectralLut");

		// Token: 0x040001FF RID: 511
		internal static readonly int Distortion_CenterScale = Shader.PropertyToID("_Distortion_CenterScale");

		// Token: 0x04000200 RID: 512
		internal static readonly int Distortion_Amount = Shader.PropertyToID("_Distortion_Amount");

		// Token: 0x04000201 RID: 513
		internal static readonly int Lut2D = Shader.PropertyToID("_Lut2D");

		// Token: 0x04000202 RID: 514
		internal static readonly int Lut3D = Shader.PropertyToID("_Lut3D");

		// Token: 0x04000203 RID: 515
		internal static readonly int Lut3D_Params = Shader.PropertyToID("_Lut3D_Params");

		// Token: 0x04000204 RID: 516
		internal static readonly int Lut2D_Params = Shader.PropertyToID("_Lut2D_Params");

		// Token: 0x04000205 RID: 517
		internal static readonly int UserLut2D_Params = Shader.PropertyToID("_UserLut2D_Params");

		// Token: 0x04000206 RID: 518
		internal static readonly int PostExposure = Shader.PropertyToID("_PostExposure");

		// Token: 0x04000207 RID: 519
		internal static readonly int ColorBalance = Shader.PropertyToID("_ColorBalance");

		// Token: 0x04000208 RID: 520
		internal static readonly int ColorFilter = Shader.PropertyToID("_ColorFilter");

		// Token: 0x04000209 RID: 521
		internal static readonly int HueSatCon = Shader.PropertyToID("_HueSatCon");

		// Token: 0x0400020A RID: 522
		internal static readonly int Brightness = Shader.PropertyToID("_Brightness");

		// Token: 0x0400020B RID: 523
		internal static readonly int ChannelMixerRed = Shader.PropertyToID("_ChannelMixerRed");

		// Token: 0x0400020C RID: 524
		internal static readonly int ChannelMixerGreen = Shader.PropertyToID("_ChannelMixerGreen");

		// Token: 0x0400020D RID: 525
		internal static readonly int ChannelMixerBlue = Shader.PropertyToID("_ChannelMixerBlue");

		// Token: 0x0400020E RID: 526
		internal static readonly int Lift = Shader.PropertyToID("_Lift");

		// Token: 0x0400020F RID: 527
		internal static readonly int InvGamma = Shader.PropertyToID("_InvGamma");

		// Token: 0x04000210 RID: 528
		internal static readonly int Gain = Shader.PropertyToID("_Gain");

		// Token: 0x04000211 RID: 529
		internal static readonly int Curves = Shader.PropertyToID("_Curves");

		// Token: 0x04000212 RID: 530
		internal static readonly int CustomToneCurve = Shader.PropertyToID("_CustomToneCurve");

		// Token: 0x04000213 RID: 531
		internal static readonly int ToeSegmentA = Shader.PropertyToID("_ToeSegmentA");

		// Token: 0x04000214 RID: 532
		internal static readonly int ToeSegmentB = Shader.PropertyToID("_ToeSegmentB");

		// Token: 0x04000215 RID: 533
		internal static readonly int MidSegmentA = Shader.PropertyToID("_MidSegmentA");

		// Token: 0x04000216 RID: 534
		internal static readonly int MidSegmentB = Shader.PropertyToID("_MidSegmentB");

		// Token: 0x04000217 RID: 535
		internal static readonly int ShoSegmentA = Shader.PropertyToID("_ShoSegmentA");

		// Token: 0x04000218 RID: 536
		internal static readonly int ShoSegmentB = Shader.PropertyToID("_ShoSegmentB");

		// Token: 0x04000219 RID: 537
		internal static readonly int Vignette_Color = Shader.PropertyToID("_Vignette_Color");

		// Token: 0x0400021A RID: 538
		internal static readonly int Vignette_Center = Shader.PropertyToID("_Vignette_Center");

		// Token: 0x0400021B RID: 539
		internal static readonly int Vignette_Settings = Shader.PropertyToID("_Vignette_Settings");

		// Token: 0x0400021C RID: 540
		internal static readonly int Vignette_Mask = Shader.PropertyToID("_Vignette_Mask");

		// Token: 0x0400021D RID: 541
		internal static readonly int Vignette_Opacity = Shader.PropertyToID("_Vignette_Opacity");

		// Token: 0x0400021E RID: 542
		internal static readonly int Vignette_Mode = Shader.PropertyToID("_Vignette_Mode");

		// Token: 0x0400021F RID: 543
		internal static readonly int Grain_Params1 = Shader.PropertyToID("_Grain_Params1");

		// Token: 0x04000220 RID: 544
		internal static readonly int Grain_Params2 = Shader.PropertyToID("_Grain_Params2");

		// Token: 0x04000221 RID: 545
		internal static readonly int GrainTex = Shader.PropertyToID("_GrainTex");

		// Token: 0x04000222 RID: 546
		internal static readonly int Phase = Shader.PropertyToID("_Phase");

		// Token: 0x04000223 RID: 547
		internal static readonly int GrainNoiseParameters = Shader.PropertyToID("_NoiseParameters");

		// Token: 0x04000224 RID: 548
		internal static readonly int LumaInAlpha = Shader.PropertyToID("_LumaInAlpha");

		// Token: 0x04000225 RID: 549
		internal static readonly int DitheringTex = Shader.PropertyToID("_DitheringTex");

		// Token: 0x04000226 RID: 550
		internal static readonly int Dithering_Coords = Shader.PropertyToID("_Dithering_Coords");

		// Token: 0x04000227 RID: 551
		internal static readonly int From = Shader.PropertyToID("_From");

		// Token: 0x04000228 RID: 552
		internal static readonly int To = Shader.PropertyToID("_To");

		// Token: 0x04000229 RID: 553
		internal static readonly int Interp = Shader.PropertyToID("_Interp");

		// Token: 0x0400022A RID: 554
		internal static readonly int TargetColor = Shader.PropertyToID("_TargetColor");

		// Token: 0x0400022B RID: 555
		internal static readonly int HalfResFinalCopy = Shader.PropertyToID("_HalfResFinalCopy");

		// Token: 0x0400022C RID: 556
		internal static readonly int WaveformSource = Shader.PropertyToID("_WaveformSource");

		// Token: 0x0400022D RID: 557
		internal static readonly int WaveformBuffer = Shader.PropertyToID("_WaveformBuffer");

		// Token: 0x0400022E RID: 558
		internal static readonly int VectorscopeBuffer = Shader.PropertyToID("_VectorscopeBuffer");

		// Token: 0x0400022F RID: 559
		internal static readonly int RenderViewportScaleFactor = Shader.PropertyToID("_RenderViewportScaleFactor");

		// Token: 0x04000230 RID: 560
		internal static readonly int UVTransform = Shader.PropertyToID("_UVTransform");

		// Token: 0x04000231 RID: 561
		internal static readonly int DepthSlice = Shader.PropertyToID("_DepthSlice");

		// Token: 0x04000232 RID: 562
		internal static readonly int UVScaleOffset = Shader.PropertyToID("_UVScaleOffset");

		// Token: 0x04000233 RID: 563
		internal static readonly int PosScaleOffset = Shader.PropertyToID("_PosScaleOffset");
	}
}
