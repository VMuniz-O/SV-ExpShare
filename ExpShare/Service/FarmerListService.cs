using System.Collections.Generic;
using System.Linq;
using StardewValley;
using ExpShare.Domain;

namespace ExpShare.Service
{
    public class FarmerListService
    {
        public List<FarmerInfo> Farmers;

        public FarmerListService()
        {
            this.Farmers = new List<FarmerInfo>();
        }

        public void ResetEntries()
        {
            this.Farmers = this.GetCurrentPlayers().Select(c => new FarmerInfo(c.UniqueMultiplayerID, c.Name, c.experiencePoints.ToArray<int>())).ToList();
        }

        public void AddDifferentEntries()
        {
            foreach (var f in this.GetCurrentPlayers().Where(c => !this.Farmers.Any(x => x.Id == c.UniqueMultiplayerID)))
            {
                this.Farmers.Add(new FarmerInfo(f.UniqueMultiplayerID, f.Name, f.experiencePoints.ToArray<int>()));
            }
        }

        public List<Farmer> GetCurrentPlayers()
        {
            return Game1.getOnlineFarmers().ToList();
        }
    }
}