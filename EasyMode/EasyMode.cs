using HarmonyLib;
using MelonLoader;

namespace EasyMode
{
    public class ModTest : MelonMod
    {
        //Redefines Backpack tile gain to be faster in the early game
        [HarmonyPatch(typeof(Player), "Start")]
        class TileLevelUpCountPatch
        {
            static void Postfix(ref Player __instance)
            { //Standard Level Rewards: 4, 4, 3, 3, 3, 3, 3, 2, 2, 2, 2, 1, 1, 1, 1, 1
                int[] newRewards =
                {
                    6, 5, 5, 5, 5, 5, 5, 1, 1, 1, 1, 1, 1, 1, 1, 1
                };
                for (int i = 0; i < __instance.chosenCharacter.levelUps.Count; ++i)
                {
                    __instance.chosenCharacter.levelUps[i].rewards[0].rewardValue = newRewards[i];
                }
            }
        }

        //Bumps the the amount of items you can take to 5 making them unlimited
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

        //Bumps your AP to 5
        [HarmonyPatch(typeof(Player), "Start")]
        class ApChangePatch
        {
            static void Postfix(ref Player __instance)
            {
                __instance.APperTurn = 5;
            }
        }

        //Checks if you are starting a new game
        public static bool startNewOrMatt = false;
        [HarmonyPatch(typeof(MenuManager), nameof(MenuManager.LoadGame))]
        class StartNewGameCheckPatch
        {
            static void Postfix()
            {
                startNewOrMatt = true;
            }
        }

        //Checks if you are starting a matthew game
        [HarmonyPatch(typeof(MenuManager), nameof(MenuManager.LoadMatt))]
        class StartMattGameCheckPatch
        {
            static void Postfix()
            {
                startNewOrMatt = true;
            }
        }

        //Sets Max health to 50 if starting a new run
        [HarmonyPatch(typeof(Status), "Start")]
        class MaxHpPatch
        {
            static void Postfix(ref Status __instance)
            {
                if (!startNewOrMatt) return;
                UnityEngine.Object.FindObjectOfType<Player>().stats.maxHealth = 50;
                startNewOrMatt = false;
                __instance.ClampHealth();
            }
        }
    }
}
