using System.IO;
using System.Linq;
using System.Reflection;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using UnityEngine;

namespace NoCultivatorDust
{
    [BepInPlugin(ModGUID, ModName, ModVersion)]
    public class NoCultivatorDustPlugin : BaseUnityPlugin
    {
        internal const string ModName = "NoCultivatorDust";
        internal const string ModVersion = "1.0.3";
        internal const string Author = "Azumatt";
        private const string ModGUID = Author + "." + ModName;

        private readonly Harmony _harmony = new(ModGUID);

        public static readonly ManualLogSource NoCultivatorDustLogger =
            BepInEx.Logging.Logger.CreateLogSource(ModName);

        public void Awake()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            _harmony.PatchAll(assembly);
        }

    }
}