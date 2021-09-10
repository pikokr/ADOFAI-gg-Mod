using MelonLoader;
using UnityEngine;
using UnityEngine.UI;

namespace ADOFAI_GG.Scenes
{
    public class MainScene
    {
        public static void init(GameObject root)
        {
            var t = root.transform;
            var levelsBtn = t.GetChild(1).GetChild(0).gameObject.GetComponent<Button>();
            levelsBtn.onClick.AddListener(() =>
            {
                MelonLogger.Msg("levels");
            });
        }
    }
}