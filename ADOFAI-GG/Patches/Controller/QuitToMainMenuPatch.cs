using ADOFAI_GG.Components.Scenes;
using HarmonyLib;
using UnityEngine.SceneManagement;

namespace ADOFAI_GG.Patches.Controller {
    [HarmonyPatch(typeof(scrController), "QuitToMainMenu")]
    public static class QuitToMainMenuPatch {
        public static bool Prefix() {
            if (!LevelsScene.OpenedByThisScene) return true;
            RDUtils.SetGarbageCollectionEnabled(true);
            scrController.instance.audioManager.StopLoadingMP3File();
            
            scrUIController.instance.WipeToBlack(WipeDirection.StartsFromRight, null);
            GCS.sceneToLoad = "ADOFAIGG_LEVELS";

            scrController.deaths = 0;
            GCS.currentSpeedRun = 1f;
            return false;
        }
    }
}