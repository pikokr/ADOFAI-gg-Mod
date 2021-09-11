using System;
using HarmonyLib;
using MelonLoader;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace ADOFAI_GG.Patches.Intro
{
    internal static class IntroButtonPatches
    {
        [HarmonyPatch(typeof(scnLevelSelect), "Start")]
        private static class ScnLevelSelectStart
        {
            private static void Postfix()
            {
                var parent = GameObject.Find("outer ring");
                var f = parent.transform.Find("FloorCalibration").gameObject;
                var obj = Object.Instantiate(f, parent.transform);
                obj.name = "FloorAdofaiGG";
                var floor = obj.GetComponent<scrFloor>();
                obj.transform.position = new Vector3(2, 3);
                floor.levelnumber = -12345678;
                var textParent = GameObject.Find("Canvas World");
                var textObj = Object.Instantiate(textParent.transform.Find("Calibration"), textParent.transform);
                textObj.position = new Vector3(5.4969f, 3.001f, 72.32f);
                textObj.name = "AdofaiGG";
                textObj.GetComponent<scrTextChanger>().desktopText = "ADOFAI.GG";
                textObj.GetComponent<Text>().alignment = TextAnchor.MiddleLeft;
            }
        }

        [HarmonyPatch(typeof(scrController), "OnLandOnPortal")]
        private static class ScrControllerPortalTravelAction
        {
            private static bool Prefix(int portalDestination)
            {
                if (portalDestination == -12345678)
                {
                    GCS.sceneToLoad = "ADOFAIGG_MAIN";
                    scrController.instance.StartLoadingScene(WipeDirection.StartsFromRight);

                    return false;
                }

                return true;
            }
        }
    }
}