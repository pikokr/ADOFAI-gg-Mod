using System;
using ADOFAI_GG.Scenes;
using MelonLoader;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ADOFAI_GG.Utils
{
    public static class SceneEvents
    {
        [Init] internal static void Init()
        {
            OnLoadScene("ADOFAIGG_MAIN", MainScene.init);
            OnLoadScene("ADOFAIGG_LEVELS", LevelsScene.Init);
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