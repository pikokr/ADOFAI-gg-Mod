using MelonLoader;
using UnityEngine;
using UnityEngine.SceneManagement;
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
            var exitButton = t.GetChild(2).GetChild(0).gameObject.GetComponent<Button>();
            exitButton.onClick.AddListener(() =>
            {
                SceneManager.LoadScene("scnNewIntro");
            });
        }
    }
}