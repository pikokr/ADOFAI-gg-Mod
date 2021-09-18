using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace ADOFAI_GG.Components.Scenes {
    public class MainScene : SceneBase {
        private void Start() {
            var t = root.transform;
            var levelsBtn = t.GetChild(1).GetChild(0).gameObject.GetComponent<Button>();
            levelsBtn.onClick.AddListener(() => { SceneManager.LoadScene("ADOFAIGG_LEVELS"); });
            var exitButton = t.GetChild(2).GetChild(0).gameObject.GetComponent<Button>();
            exitButton.onClick.AddListener(() => { SceneManager.LoadScene(GCNS.sceneLevelSelect); });
        }

        public static void Init(Scene scn, GameObject root) {
            var obj = new GameObject("MainScene");
            obj.GetOrAddComponent<MainScene>().root = root;
            LevelsScene.OpenedByThisScene = false;
        }
    }
}