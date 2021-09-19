using ADOFAI_GG.Utils.Initializer;
using Cysharp.Threading.Tasks;
using MelonLoader;
using UnityEngine.LowLevel;

namespace ADOFAI_GG
{
    public class AdofaiGG: MelonMod
    {
        public override void OnApplicationStart()
        {
            PlayerLoopSystem playerLoopSystem = PlayerLoop.GetCurrentPlayerLoop();
            PlayerLoopHelper.Initialize(ref playerLoopSystem);
            PlayerLoop.SetPlayerLoop(playerLoopSystem);
            Initalizer.Init();
        }
/*
        private bool Inited = false;
        public override void OnSceneWasLoaded(int buildIndex, string sceneName) {
            if (Inited) return;
            Inited = true;
            Initalizer.LateInit();
        }
        */
    }
}