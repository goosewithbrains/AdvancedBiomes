﻿using System;
using Verse;

namespace ActiveTerrain
{
	// Token: 0x02000016 RID: 22
	public class TerrainComp_PowerTrader : TerrainComp
	{
		// Token: 0x17000015 RID: 21
		// (get) Token: 0x06000066 RID: 102 RVA: 0x000036D4 File Offset: 0x000018D4
		// (set) Token: 0x06000067 RID: 103 RVA: 0x000036EC File Offset: 0x000018EC
		public CompPowerTraderFloor ConnectParent
		{
			get
			{
				return this.connectParentInt;
			}
			set
			{
				CompPowerTraderFloor compPowerTraderFloor = this.connectParentInt;
				if (compPowerTraderFloor != null)
				{
					compPowerTraderFloor.Notify_TerrainCompRemoved(this);
				}
				this.connectParentInt = value;
				if (value != null)
				{
					value.ReceiveTerrainComp(this);
				}
			}
		}

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x06000068 RID: 104 RVA: 0x00003718 File Offset: 0x00001918
		public TerrainCompProperties_PowerTrader Props
		{
			get
			{
				return (TerrainCompProperties_PowerTrader)this.props;
			}
		}

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x06000069 RID: 105 RVA: 0x00003738 File Offset: 0x00001938
		public bool PowerOn
		{
			get
			{
				return this.ConnectParent != null && this.ConnectParent.PowerOn;
			}
		}

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x0600006A RID: 106 RVA: 0x00003760 File Offset: 0x00001960
		// (set) Token: 0x0600006B RID: 107 RVA: 0x00003778 File Offset: 0x00001978
		public virtual float PowerOutput
		{
			get
			{
				return this.powerOutputInt;
			}
			set
			{
				this.powerOutputInt = value;
				CompPowerTraderFloor connectParent = this.ConnectParent;
				if (connectParent != null)
				{
					connectParent.UpdatePowerOutput();
				}
			}
		}

		// Token: 0x0600006C RID: 108 RVA: 0x00003794 File Offset: 0x00001994
		public override void CompUpdate()
		{
			bool flag = !this.PowerOn && !this.Props.ignoreNeedsPower;
			if (flag)
			{
				ActiveTerrainUtility.RenderPulsingNeedsPowerOverlay(this.parent.Position);
			}
		}

		// Token: 0x0600006D RID: 109 RVA: 0x000037D4 File Offset: 0x000019D4
		public override void CompTick()
		{
			bool flag = this.PowerOn != this.curSignal;
			if (flag)
			{
				this.parent.BroadcastCompSignal(this.PowerOn ? CompSignals.PowerTurnedOn : CompSignals.PowerTurnedOff);
				this.curSignal = this.PowerOn;
			}
			bool flag2 = !this.PowerOn && Find.TickManager.TicksGame % this.tickInterval == this.HashCodeToMod(this.tickInterval);
			if (flag2)
			{
				CompPowerTraderFloor compPowerTraderFloor = ActiveTerrainUtility.TryFindNearestPowerConduitFloor(this.parent.Position, this.parent.Map);
				bool flag3 = compPowerTraderFloor != null;
				if (flag3)
				{
					this.ConnectParent = compPowerTraderFloor;
				}
			}
		}

		// Token: 0x0600006E RID: 110 RVA: 0x00003883 File Offset: 0x00001A83
		public override void Initialize(TerrainCompProperties props)
		{
			base.Initialize(props);
			this.powerOutputInt = -this.Props.basePowerConsumption;
		}

		// Token: 0x0600006F RID: 111 RVA: 0x000038A0 File Offset: 0x00001AA0
		public override void PostRemove()
		{
			this.ConnectParent = null;
		}

		// Token: 0x06000070 RID: 112 RVA: 0x000038AC File Offset: 0x00001AAC
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<bool>(ref this.curSignal, "curCompSignal", false, false);
			Thing thing = null;
			bool flag = Scribe.mode == LoadSaveMode.Saving && this.ConnectParent != null;
			if (flag)
			{
				thing = this.ConnectParent.parent;
			}
			Scribe_References.Look<Thing>(ref thing, "parentThing", false);
			bool flag2 = thing != null;
			if (flag2)
			{
				this.ConnectParent = ((ThingWithComps)thing).GetComp<CompPowerTraderFloor>();
			}
		}

		// Token: 0x04000029 RID: 41
		public readonly int tickInterval = 50;

		// Token: 0x0400002A RID: 42
		private float powerOutputInt;

		// Token: 0x0400002B RID: 43
		private CompPowerTraderFloor connectParentInt;

		// Token: 0x0400002C RID: 44
		private bool curSignal;
	}
}
