using ADOFAI_GG.Utils;
using Cysharp.Threading.Tasks;
using MelonLoader;
using UnityEngine.LowLevel;

namespace ADOFAI_GG
{
    public class AdofaiGG: MelonMod
    {
        public override void OnApplicationStart()
        {
            Assets.Init();
            SceneEvents.Init();

            PlayerLoopSystem playerLoopSystem = PlayerLoop.GetCurrentPlayerLoop();
            PlayerLoopHelper.Initialize(ref playerLoopSystem);
            PlayerLoop.SetPlayerLoop(playerLoopSystem);

        }

    }
}