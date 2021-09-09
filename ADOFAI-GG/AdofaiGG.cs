using System;
using MelonLoader;
using UnityEngine.SceneManagement;

namespace ADOFAI_GG
{
    public class AdofaiGG: MelonMod
    {
        public override void OnApplicationStart()
        {
            Assets.Init();
        }
    }
}