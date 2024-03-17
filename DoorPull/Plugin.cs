using System.Reflection;
using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using UnityEngine;

namespace DoorPull {
    [BepInPlugin(ModGuid, ModName, ModVersion)]
    public class Plugin : BaseUnityPlugin {
        public const string ModName = "DoorPull";
        public const string ModGuid = "com.maxsch.valheim.DoorPull";
        public const string ModVersion = "0.1.0";

        public static Plugin Instance { get; private set; }

        public static ConfigEntry<KeyboardShortcut> PullKey { get; private set; }

        private Harmony harmony;

        private void Awake() {
            Instance = this;

            harmony = new Harmony(ModGuid);
            harmony.PatchAll();

            PullKey = Config.Bind("General", "Pull Key", new KeyboardShortcut(KeyCode.E, KeyCode.LeftControl));
        }
    }
}
