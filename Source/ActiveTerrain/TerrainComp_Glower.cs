﻿using System;
using RimWorld;
using Verse;

namespace ActiveTerrain
{
	// Token: 0x02000015 RID: 21
	public class TerrainComp_Glower : TerrainComp
	{
		// Token: 0x1700000F RID: 15
		// (get) Token: 0x06000057 RID: 87 RVA: 0x0000342C File Offset: 0x0000162C
		public CompGlower AsThingComp
		{
			get
			{
				return (this.instanceGlowerComp == null) ? (this.instanceGlowerComp = (CompGlower)this) : this.instanceGlowerComp;
			}
		}

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x06000058 RID: 88 RVA: 0x00003460 File Offset: 0x00001660
		public TerrainCompProperties_Glower Props
		{
			get
			{
				return (TerrainCompProperties_Glower)this.props;
			}
		}

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x06000059 RID: 89 RVA: 0x00003480 File Offset: 0x00001680
		public virtual bool ShouldBeLitNow
		{
			get
			{
				TerrainComp_PowerTrader comp = this.parent.GetComp<TerrainComp_PowerTrader>();
				return comp == null || comp.PowerOn || !this.Props.powered;
			}
		}

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x0600005A RID: 90 RVA: 0x000034BC File Offset: 0x000016BC
		// (set) Token: 0x0600005B RID: 91 RVA: 0x000034C4 File Offset: 0x000016C4
		public float OverlightRadius
		{
			get
			{
				return this.overlightRadius;
			}
			set
			{
				this.overlightRadius = value;
			}
		}

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x0600005C RID: 92 RVA: 0x000034CD File Offset: 0x000016CD
		// (set) Token: 0x0600005D RID: 93 RVA: 0x000034D5 File Offset: 0x000016D5
		public float GlowRadius
		{
			get
			{
				return this.glowRadius;
			}
			set
			{
				this.glowRadius = value;
			}
		}

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x0600005E RID: 94 RVA: 0x000034DE File Offset: 0x000016DE
		// (set) Token: 0x0600005F RID: 95 RVA: 0x000034E6 File Offset: 0x000016E6
		public ColorInt Color
		{
			get
			{
				return this.colorInt;
			}
			set
			{
				this.colorInt = value;
			}
		}

		// Token: 0x06000060 RID: 96 RVA: 0x000034F0 File Offset: 0x000016F0
		public void UpdateLit()
		{
			bool shouldBeLitNow = this.ShouldBeLitNow;
			bool flag = this.currentlyOn == shouldBeLitNow;
			if (!flag)
			{
				this.currentlyOn = shouldBeLitNow;
				this.parent.Map.mapDrawer.MapMeshDirty(this.parent.Position, MapMeshFlag.Things);
				(this.currentlyOn ? new Action<CompGlower>(this.parent.Map.glowGrid.RegisterGlower) : new Action<CompGlower>(this.parent.Map.glowGrid.DeRegisterGlower))(this.AsThingComp);
			}
		}

		// Token: 0x06000061 RID: 97 RVA: 0x0000358C File Offset: 0x0000178C
		public override void ReceiveCompSignal(string sig)
		{
			base.ReceiveCompSignal(sig);
			bool flag = sig == CompSignals.PowerTurnedOff || sig == CompSignals.PowerTurnedOn;
			if (flag)
			{
				this.UpdateLit();
			}
		}

		// Token: 0x06000062 RID: 98 RVA: 0x000035CC File Offset: 0x000017CC
		public override void PostPostLoad()
		{
			this.UpdateLit();
			bool shouldBeLitNow = this.ShouldBeLitNow;
			if (shouldBeLitNow)
			{
				this.parent.Map.glowGrid.RegisterGlower(this.AsThingComp);
			}
		}

		// Token: 0x06000063 RID: 99 RVA: 0x0000360C File Offset: 0x0000180C
		public override void Initialize(TerrainCompProperties props)
		{
			base.Initialize(props);
			this.Color = this.Props.glowColor;
			this.GlowRadius = this.Props.glowRadius;
			this.OverlightRadius = this.Props.overlightRadius;
		}

		// Token: 0x06000064 RID: 100 RVA: 0x00003658 File Offset: 0x00001858
		public static explicit operator CompGlower(TerrainComp_Glower inst)
		{
			CompGlower compGlower = new CompGlower
			{
				parent = (ThingWithComps)ThingMaker.MakeThing(ThingDefOf.Wall, ThingDefOf.Steel)
			};
			compGlower.parent.SetPositionDirect(inst.parent.Position);
			compGlower.Initialize(new CompProperties_Glower
			{
				glowColor = inst.Color,
				glowRadius = inst.GlowRadius,
				overlightRadius = inst.OverlightRadius
			});
			return compGlower;
		}

		// Token: 0x04000024 RID: 36
		[Unsaved]
		protected bool currentlyOn;

		// Token: 0x04000025 RID: 37
		[Unsaved]
		private CompGlower instanceGlowerComp;

		// Token: 0x04000026 RID: 38
		private ColorInt colorInt;

		// Token: 0x04000027 RID: 39
		private float glowRadius;

		// Token: 0x04000028 RID: 40
		private float overlightRadius;
	}
}
