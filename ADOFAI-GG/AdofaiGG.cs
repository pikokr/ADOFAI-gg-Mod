using System;
using ADOFAI_GG.Utils;
using MelonLoader;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ADOFAI_GG
{
    public class AdofaiGG: MelonMod {
        public override void OnApplicationLateStart() {
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