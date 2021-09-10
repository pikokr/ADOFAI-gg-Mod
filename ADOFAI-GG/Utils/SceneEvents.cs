using System;
using ADOFAI_GG.Scenes;
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
        }
        
        internal static void OnLoadScene(string scene, Action<GameObject> action)
        {
            SceneManager.sceneLoaded += (scn, mode) =>
            {
                MelonLogger.Msg($"{scn.name} ${scene}");
                if (scn.name == scene)
                {
                    try
                    {
                        action(GameObject.Find("Root"));
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