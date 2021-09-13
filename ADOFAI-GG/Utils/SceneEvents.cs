using System;
using ADOFAI_GG.Presentation.View.Scene;
using MelonLoader;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ADOFAI_GG.Utils
{
    public static class SceneEvents
    {
        internal static void Init()
        {
            OnLoadScene("ADOFAIGG_MAIN", MainScene.init);
            OnLoadScene("ADOFAIGG_LEVELS", LevelsScene.init);
        }
        
        internal static void OnLoadScene(string scene, Action<Scene, GameObject> action)
        {
            SceneManager.sceneLoaded += (scn, mode) =>
            {
                if (scn.name == scene)
                {
                    try
                    {
                        action(scn, GameObject.Find("Root"));
                    }
                    catch (Exception e)
                    {
                        MelonLogger.Error(e);
                        throw;
                    }
                }
            };
        }
    }
}