using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using ADOFAI_GG.Levels;
using MelonLoader;
using MelonLoader.TinyJSON;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace ADOFAI_GG.Scenes
{
    public class LevelsScene : SceneBase
    {
        private List<GameObject> objects = new List<GameObject>();

        private Button prevButton;
        private Button nextButton;
        private Text paginationNumber;

            private void Start()
        {
            var t = root.transform;
            var exitButton = t.GetChild(1).GetChild(0).gameObject.GetComponent<Button>();
            exitButton.onClick.AddListener(() => { SceneManager.LoadScene("ADOFAIGG_MAIN"); });

            var pagination = root.transform.GetChild(0).GetChild(1);

            prevButton = pagination.GetChild(0).GetComponent<Button>();
            nextButton = pagination.GetChild(2).GetComponent<Button>();
            paginationNumber = pagination.GetChild(1).GetChild(0).GetComponent<Text>();

            prevButton.interactable = false;
            nextButton.interactable = false;
            
            prevButton.onClick.AddListener(() =>
            {
                prevButton.interactable = false;
                page -= 1;
                StartCoroutine(fetch());
            });
            
            nextButton.onClick.AddListener(() =>
            {
                nextButton.interactable = false;
                page += 1;
                StartCoroutine(fetch());
            });

            StartCoroutine(fetch());
        }

        private int page;
        private double count;
        private int maxPage => (int)Math.Ceiling(count / 5);

        private IEnumerator fetch()
        {
            foreach (var o in objects)
            {
                GameObject.Destroy(o);
            }

            objects = new List<GameObject>();
            
            paginationNumber.text = $"{page+1}";

            root.transform.GetChild(0).GetChild(0).GetChild(1).gameObject.SetActive(true);

            
            var www = new UnityWebRequest($"https://api.adofai.gg:9200/api/v1/levels?offset={5*page}&amount=5");

            www.downloadHandler = new DownloadHandlerBuffer();

            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                MelonLogger.Error(www.error);
                yield break;
            }

            var res = www.downloadHandler.text;

            var data = JSON.Load(res);

            var list = data["results"] as ProxyArray;
            if (list == null) yield break;
            var levels = new List<Level>();
            foreach (var i in list)
            {
                levels.Add(JsonUtility.FromJson<Level>(i.ToJSON()));
            }

            count = data["count"];

            root.transform.GetChild(0).GetChild(0).GetChild(1).gameObject.SetActive(false);

            setLevels(levels);

            prevButton.interactable = page != 0;
            nextButton.interactable = page < maxPage;
            paginationNumber.text = $"{page+1} / {maxPage}";
        }

        private void setLevels(List<Level> levels)
        {
            var list = root.transform.GetChild(0).GetChild(0);
            var toInstantiate = list.GetChild(0).gameObject;
            foreach (var level in levels)
            {
                var o = GameObject.Instantiate(toInstantiate, list);
                var t = o.transform;
                var icon = t.GetChild(0);

                var iconSprite = Assets.Bundle.LoadAsset<Sprite>($"Assets/Images/difficultyIcons/{level.difficulty}.png");
                
                icon.gameObject.GetComponent<Image>().sprite = iconSprite;
                
                t.GetChild(1).GetComponent<Text>().text = String.Join(" & ", level.artists);
                t.GetChild(2).GetComponent<Text>().text = level.song;
                t.GetChild(4).GetComponent<Text>().text = String.Join(" & ", level.creators);
                t.GetChild(6).GetComponent<Text>().text = level.minBpm.ToString(CultureInfo.InvariantCulture) == level.maxBpm.ToString(CultureInfo.InvariantCulture) ? $"{level.minBpm}" : $"{level.minBpm}-{level.maxBpm}";
                t.GetChild(8).GetComponent<Text>().text = level.tiles.ToString();
                t.GetChild(10).GetComponent<Text>().text = level.comments.ToString();
                t.GetChild(12).GetComponent<Text>().text = level.comments.ToString();
                objects.Add(o);
                o.SetActive(true);
            }
        }

        public static void init(Scene scn, GameObject root)
        {
            var obj = new GameObject("LevelsScene");
            obj.GetOrAddComponent<LevelsScene>().root = root;
        }
    }
}