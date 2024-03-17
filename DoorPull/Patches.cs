using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;
using UnityEngine;

namespace DoorPull {
    [HarmonyPatch]
    public class Patches {
        private static float lastDoorInteractTime;

        [HarmonyPatch(typeof(Door), nameof(Door.GetHoverText)), HarmonyPostfix]
        public static void GetHoverText(Door __instance, ref string __result) {
            if (!__instance.m_nview.IsValid() || __instance.m_canNotBeClosed && !__instance.CanInteract()) {
                return;
            }

            if (__instance.m_checkGuardStone && !PrivateArea.CheckAccess(__instance.transform.position, flash: false)) {
                return;
            }

            if (!__instance.CanInteract()) {
                return;
            }

            bool isOpen = __instance.m_nview.GetZDO().GetInt(ZDOVars.s_state) != 0;
            string openText = isOpen && !__instance.m_invertedOpenClosedText ? "$piece_door_close" : "Pull $piece_door_open";
            __result += Localization.instance.Localize($"\n[<color=yellow><b>{Plugin.PullKey.Value.Serialize()}</b></color>] {openText}");
        }

        [HarmonyPatch(typeof(Door), nameof(Door.Interact)), HarmonyTranspiler]
        public static IEnumerable<CodeInstruction> InteractTranspiler(IEnumerable<CodeInstruction> instructions) {
            MethodInfo openCall = AccessTools.Method(typeof(Door), nameof(Door.Open));

            return new CodeMatcher(instructions)
                .MatchForward(false, new CodeMatch(i => i.Calls(openCall)))
                .Insert(
                    new CodeInstruction(OpCodes.Ldarg_0), // this (Door)
                    new CodeInstruction(OpCodes.Ldarg_1), // character
                    new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(Patches), nameof(GetPullDirection)))
                )
                .InstructionEnumeration();
        }

        [HarmonyPatch(typeof(Player), nameof(Player.Update)), HarmonyPostfix]
        public static void UpdatePostfix(Player __instance) {
            if (!__instance.m_hovering || !Plugin.PullKey.Value.IsKeyPressed() || !__instance.TakeInput()) {
                return;
            }

            if (Time.time - lastDoorInteractTime < 0.2 || !__instance.m_hovering.GetComponentInParent<Door>()) {
                return;
            }

            // this allows other keys then E to be used for interaction with doors
            __instance.Interact(__instance.m_hovering, false, true);
        }

        private static Vector3 GetPullDirection(Vector3 originalDirection, Door door, Humanoid character) {
            lastDoorInteractTime = Time.time;

            if (!Plugin.PullKey.Value.IsKeyPressed()) {
                return originalDirection;
            }

            return (door.transform.position - character.transform.position).normalized;
        }
    }
}
