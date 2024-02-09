using System.Collections.Generic;
using System.Linq;
using StardewValley;
using ExpShare.Domain;

namespace ExpShare.Service
{
    public class ExperienceService
    {        
        private FarmerListService farmerListService;
        
        public ExperienceService(FarmerListService _farmerListService)
        {
            this.farmerListService = _farmerListService;
        }

        public void UpdateExperience()
        {
            var allGainedExperience = new List<FarmerInfo>();
            var currentFarmers = this.farmerListService.GetCurrentPlayers();
            var controlFarmers = this.farmerListService.Farmers;
            foreach (var r in currentFarmers)
            {
                int[] gainedExperience = r.experiencePoints.ToArray<int>();
                int[] controlExperience = controlFarmers.FirstOrDefault(c => c.Id == r.UniqueMultiplayerID).Experience;
                for (int i = 0; i < gainedExperience.Length; i++)
                {
                    gainedExperience[i] -= controlExperience[i];
                }
                allGainedExperience.Add(new FarmerInfo(r.UniqueMultiplayerID, r.Name, gainedExperience));
            }
            foreach (var r in currentFarmers)
            {
                for (int i = 0; i < r.experiencePoints.Length; i++)
                {
                    r.experiencePoints[i] += allGainedExperience.Where(c => c.Id != r.UniqueMultiplayerID).Select(c => c.Experience).Sum(c => c[i]);
                }
            }
        }
    }
}