
using System.Collections.Generic;

namespace Becerra.Carder.Card
{
    public static class Icons
    {
        private static Dictionary<string, int> IndexByIcon = new Dictionary<string, int>
        {
            {"1", 0},
            {"2", 1},
            {"3", 2},
            {"R", 8},
            {"1-3", 19},
            {"1-2", 11},
            {"2-3", 3},
            {"P", 9},
            {"F", 10},
            {"critical_success", 16},
            {"success", 17},
            {"miss", 18},
            {"critical_miss", 24},
            {"d20", 25},
            {"d4", 26},
            {"power_up", 27},
            {"time", 4},
            {"cone", 5},
            {"saving", 6},
            {"target", 12},
            {"line", 13},
            {"ranged", 21},
            {"emanation", 21},
            {"range", 28},
            {"melee", 29},
            {"somatic", 14},
            {"verbal", 22},
            {"material", 30}
        };

        public static string GetRichtextForIcon(string iconName)
        {
            if (IndexByIcon.TryGetValue(iconName, out int index))
            {
                return $"<sprite={index}>";
            }

            return "ICON_NOT_FOUND";
        }

        public static string ApplyIcons(string text)
        {
            foreach (var icon in IndexByIcon)
            {
                string iconText = GetRichtextForIcon(icon.Key);
                string key = "{" + icon.Key + "}";

                text = text.Replace(key, iconText);
            }

            return text;
        }
    }
}