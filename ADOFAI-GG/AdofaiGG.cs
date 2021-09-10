using System;
using ADOFAI_GG.Utils;
using MelonLoader;
using UnityEngine.SceneManagement;

namespace ADOFAI_GG
{
    public class AdofaiGG: MelonMod
    {
        public override void OnApplicationStart()
        {
            Assets.Init();
            SceneEvents.Init();
        }
    }
}