using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewModdingAPI.Utilities;
using StardewValley;
using ExpShare.Domain;
using ExpShare.Service;

namespace ExpShare.Main
{
    internal class ModEntry : Mod
	{
        private ModConfig Config;
        private FarmerListService farmerListService;
        private ExperienceService experienceService;
        private bool ModEnabled;

		public override void Entry(IModHelper helper)
		{
            this.farmerListService = new FarmerListService();
            this.experienceService = new ExperienceService(this.farmerListService);

            this.ModEnabled = false;
            this.Config = Helper.ReadConfig<ModConfig>();
            this.Helper.Events.GameLoop.SaveLoaded += EnableExpShare;
            this.Helper.Events.GameLoop.ReturnedToTitle += DisableExpShare;
		}

        private void EnableExpShare(object sender, EventArgs e)
        {
            if (this.Config.Enabled && Context.IsMultiplayer && Game1.IsServer)
            {
                this.Helper.Events.GameLoop.DayEnding += this.ShareExpAtDayEnding;
                this.Helper.Events.GameLoop.DayStarted += this.ResetFarmerList;
                this.Helper.Events.Multiplayer.PeerConnected += this.AddToFarmerList;
                this.ModEnabled = true;
                this.Monitor.Log($"ExpShare Enabled", LogLevel.Info);
            }
        }

        private void DisableExpShare(object sender, EventArgs e)
        {
            if (ModEnabled)
            {
                this.Helper.Events.GameLoop.DayEnding -= this.ShareExpAtDayEnding;
                this.Helper.Events.GameLoop.DayStarted -= this.ResetFarmerList;
                this.Helper.Events.Multiplayer.PeerConnected -= this.AddToFarmerList;
                this.ModEnabled = false;
                this.Monitor.Log($"ExpShare Disabled", LogLevel.Info);
            }
        }

        private void ResetFarmerList(object sender, DayStartedEventArgs e)
        {
            this.farmerListService.ResetEntries();
            LogPlayersAndExperience();
        }

        private void AddToFarmerList(object sender, PeerConnectedEventArgs e)
        {
            this.farmerListService.AddDifferentEntries();
            LogPlayersAndExperience();
        }

        private void ShareExpAtDayEnding(object sender, DayEndingEventArgs e)
        {
            this.experienceService.UpdateExperience();
        }

        private void LogPlayersAndExperience()
        {            
            this.Monitor.Log($"Experience:\n" + string.Join("\n", this.farmerListService.Farmers.Select(c => c.Name + " -> (" + string.Join("-", c.Experience.ToArray<int>()) + ")").ToList()), LogLevel.Info);
        }
	}
}