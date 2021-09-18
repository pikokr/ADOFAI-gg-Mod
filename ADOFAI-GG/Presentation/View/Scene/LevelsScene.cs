using System;
using System.Collections.Generic;
using System.Globalization;
using ADOFAI_GG.Domain.Model.Levels;
using ADOFAI_GG.Presentation.ViewModel.Scene;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using UniRx;

namespace ADOFAI_GG.Presentation.View.Scene
{
    public class LevelsScene : SceneBase
    {

        private LevelsViewModel viewModel;

        private List<GameObject> levelObjectList = new List<GameObject>();

        private Button prevButton;
        private Button nextButton;
        private Text paginationNumber;

        private Transform content => root.transform.GetChild(1);

        private Transform levelList;
        private GameObject levelToInstantiate;

        private GameObject loadingMessage;

        private Image searchButton;

        public static void Init(UnityEngine.SceneManagement.Scene scn, GameObject root)
        {
            var obj = new GameObject("LevelsScene");
            obj.GetOrAddComponent<LevelsScene>().root = root;
        }

        private void Start()
        {
            viewModel = LevelsViewModel.getInstance();

            var t = root.transform;
            var buttons = t.GetChild(0).GetChild(1);
            var exitButton = buttons.GetChild(0).gameObject.GetComponent<Button>();
            var serachButton = buttons.GetChild(1).gameObject.GetComponent<Button>();
            exitButton.onClick.AddListener(() => { SceneManager.LoadScene("ADOFAIGG_MAIN"); });


            var searchT = content.GetChild(2);

            var searchOverlay = searchT.GetChild(0);

            var searchInput = searchT.GetChild(1).GetComponent<InputField>();

            searchInput.onEndEdit.AddListener(query =>
            {
                viewModel.GetSearchQuery().Value = query;
                searchT.gameObject.SetActive(false);
                UpdateLevels();
            });

            searchOverlay.GetComponent<Button>().onClick.AddListener(() => { searchT.gameObject.SetActive(false); });

            serachButton.onClick.AddListener(() => { searchT.gameObject.SetActive(true); });

            var pagination = content.GetChild(1);

            prevButton = pagination.GetChild(0).GetComponent<Button>();
            nextButton = pagination.GetChild(2).GetComponent<Button>();
            paginationNumber = pagination.GetChild(1).GetChild(0).GetComponent<Text>();

            prevButton.interactable = false;
            nextButton.interactable = false;


            prevButton.onClick.AddListener(() =>
            {
                prevButton.interactable = false;
                nextButton.interactable = false;
                viewModel.GetPage().Value -= 1;
                UpdateLevels();
            });

            nextButton.onClick.AddListener(() =>
            {
                prevButton.interactable = false;
                nextButton.interactable = false;
                viewModel.GetPage().Value += 1;
                UpdateLevels();
            });

            searchButton = root.transform.GetChild(0).GetChild(1).GetChild(1).GetChild(1).GetComponent<Image>();


            viewModel.GetSearchQuery().ToObservable().Subscribe(value =>
            {
                searchButton.color = value.Length == 0 ? Color.white : new Color(0, 125, 255, 255);
            });

            levelList = content.GetChild(0);
            levelToInstantiate = levelList.GetChild(0).gameObject;
            
            viewModel.GetCurrentPageInfo().Subscribe(pageInfo =>
            {
                int page = pageInfo.Item1;
                int maxPage = pageInfo.Item2;

                paginationNumber.text = $"{page + 1} / {maxPage}";
                prevButton.interactable = page != 0;
                nextButton.interactable = page != maxPage - 1;
            });


            loadingMessage = content.GetChild(0).GetChild(1).gameObject;

            viewModel.GetLevels().ObserveOnMainThread().Subscribe(levels =>
            {
                loadingMessage.SetActive(false);
                if (levels == null) return;
                foreach (Level level in levels)
                {
                    instantiateLevel(level);
                }
                
            });
            
            UpdateLevels();
        }
        
        private void UpdateLevels()
        {
            foreach (var o in levelObjectList)
            {
                GameObject.Destroy(o);
            }
            levelObjectList.Clear();

            loadingMessage.SetActive(true);
            viewModel.FetchLevels();
        }

        private void instantiateLevel(Level level)
        {
            var o = GameObject.Instantiate(levelToInstantiate, levelList);
            var t = o.transform;
            var icon = t.GetChild(0);

            var iconSprite =
                Assets.Bundle.LoadAsset<Sprite>($"Assets/Images/difficultyIcons/{level.difficulty}.png");

            icon.gameObject.GetComponent<Image>().sprite = iconSprite;

            t.GetChild(1).GetComponent<Text>().text = String.Join(" & ", level.artists);
            t.GetChild(2).GetComponent<Text>().text = level.song;
            t.GetChild(4).GetComponent<Text>().text = String.Join(" & ", level.creators);
            t.GetChild(6).GetComponent<Text>().text =
                level.minBpm.ToString(CultureInfo.InvariantCulture) ==
                level.maxBpm.ToString(CultureInfo.InvariantCulture)
                    ? $"{level.minBpm}"
                    : $"{level.minBpm}-{level.maxBpm}";
            t.GetChild(8).GetComponent<Text>().text = level.tiles.ToString();
            t.GetChild(10).GetComponent<Text>().text = level.comments.ToString();
            t.GetChild(12).GetComponent<Text>().text = level.comments.ToString();
            levelObjectList.Add(o);
            o.SetActive(true);
        }

    }
}