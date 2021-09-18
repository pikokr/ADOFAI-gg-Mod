using ADOFAI_GG.Utils;
using Cysharp.Threading.Tasks;
using MelonLoader;
using UnityEngine.LowLevel;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ADOFAI_GG
{
    public class AdofaiGG: MelonMod
    {
        public override void OnApplicationStart()
        {
            Initalizer.Init();
            PlayerLoopSystem playerLoopSystem = PlayerLoop.GetCurrentPlayerLoop();
            PlayerLoopHelper.Initialize(ref playerLoopSystem);
            PlayerLoop.SetPlayerLoop(playerLoopSystem);
        }
    }
}