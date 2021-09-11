using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace ADOFAI_GG.Scenes
{
    public class LevelsScene: SceneBase
    {
        private void Start()
        {
            var t = root.transform;
            var exitButton = t.GetChild(0).GetChild(0).gameObject.GetComponent<Button>();
            exitButton.onClick.AddListener(() =>
            {
                SceneManager.LoadScene("ADOFAIGG_MAIN");
            });
        }
        
        public static void init(Scene scn, GameObject root)
        {
            var obj = new GameObject("LevelsScene");
            obj.GetOrAddComponent<LevelsScene>().root = root;
        }
    }
}