using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.CampaignSystem;
using TaleWorlds.ObjectSystem;

namespace AutoEquipCompanions.Saving
{
    public static class Config
    {
        public static void Initialize()
        {
            CharacterSettings = new Dictionary<string, CharacterSettings>();
        }

        internal static Dictionary<string, CharacterSettings> CharacterSettings;
    }
}
