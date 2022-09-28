using HarmonyLib;
using MelonLoader;

namespace ModerateMode
{
    public class ModTest : MelonMod
    {
        //Redefines Backpack tile gain to be slightly faster in the early game
        [HarmonyPatch(typeof(Player), "Start")]
        class TileLevelUpCountPatch
        {
            static void Postfix(ref Player __instance)
            { //Standard Level Rewards: 4, 4, 3, 3, 3, 3, 3, 2, 2, 2, 2, 1, 1, 1, 1, 1
                int[] newRewards =
                {
                    5, 5, 4, 4, 3, 3, 2, 2, 1, 1, 1, 1, 1, 1, 1, 1
                };
                for (int i = 0; i < __instance.chosenCharacter.levelUps.Count; ++i)
                {
                    __instance.chosenCharacter.levelUps[i].rewards[0].rewardValue = newRewards[i];
                }
            }
        }

        //Bumps the the amount of items you can take to 4 (default 3)
        [HarmonyPatch(typeof(GameManager), nameof(GameManager.ChangeItemsAllowedToTake))]
        class ItemsAllowedToTakePatch
        {
            static bool Prefix(ref GameManager __instance)
            {
                if (__instance.numOfItemsAllowedToTake == 3)
                {
                    __instance.numOfItemsAllowedToTake = 5;
                }
                return true;
            }
        }
    }
}
