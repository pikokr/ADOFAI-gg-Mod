using System;
using HarmonyLib;
using MelonLoader;
using UnityEngine;
using UnityEngine.SceneManagement;
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
                var floor = obj.GetComponent<scrFloor>();
                obj.transform.position = new Vector3(2, 3);
                floor.levelnumber = -12345678;
            }
        }

        [HarmonyPatch(typeof(scrController), "OnLandOnPortal")]
        private static class ScrControllerPortalTravelAction
        {
            private static bool Prefix(int portalDestination)
            {
                if (portalDestination == -12345678)
                {
                    try
                    {
                        SceneManager.LoadScene("ADOFAIGG_NewCLS");
                    }
                    catch (Exception e)
                    {
                        Debug.LogError(e);
                        throw;
                    }
                    return false;
                }

                return true;
            }
        }
    }
}