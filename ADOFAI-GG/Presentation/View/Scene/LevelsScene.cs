using System;
using System.Collections.Generic;
using ADOFAI_GG.Presentation.ViewModel.Scene;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using UniRx;
using System.Collections;
using ADOFAI_GG.Data.Repository;
using ADOFAI_GG.Presentation.Model;

namespace ADOFAI_GG.Presentation.View.Scene {
    public class LevelsScene : SceneBase {
        public static bool OpenedByThisScene;

        public static LevelsScene Instance => _instance;
        private static LevelsScene _instance;

        private LevelsViewModel _viewModel;

        private List<GameObject> _levelObjectList = new List<GameObject>();

        public Transform levelsParent;
        public ScrollRect levelScrollRect;
        public RectTransform levelScrollHandle;

        private Image _searchButtonImage;

        public static void Init(UnityEngine.SceneManagement.Scene scn, GameObject root) {
            var obj = new GameObject("LevelsScene");
            obj.GetOrAddComponent<LevelsScene>().root = root;
        }

        private void Awake() {
            if (Instance != null) {
                Destroy(gameObject);
            }

            _instance = this;
        }

        private void Start() {
            _viewModel = LevelsViewModel.Instance;

            var buttons = root.transform.GetChild(0).GetChild(1);
            var exitButton = buttons.GetChild(0).gameObject.GetComponent<Button>();
            var searchButton = buttons.GetChild(1).gameObject.GetComponent<Button>();

            var content = root.transform.GetChild(1);

            var searchTransform = content.GetChild(3);
            var searchOverlay = searchTransform.GetChild(0);
            var searchOverlayButton = searchOverlay.GetComponent<Button>();
            var searchInput = searchTransform.GetChild(1).GetComponent<InputField>();

            _searchButtonImage = root.transform.GetChild(0).GetChild(1).GetChild(1).GetChild(1).GetComponent<Image>();

            var pagination = content.GetChild(1);
            levelScrollRect = GameObject.Find("InfinityScroll").GetComponent<ScrollRect>();
            levelsParent = levelScrollRect.transform.GetChild(0).GetChild(0);
            levelScrollHandle = levelScrollRect.transform.GetChild(2).GetChild(0).GetChild(0)
                .GetComponent<RectTransform>();


            exitButton.onClick.AddListener(() => { SceneManager.LoadScene("ADOFAIGG_MAIN"); });

            searchInput.onEndEdit.AddListener(query => {
                _viewModel.SearchQuery.Value = query;
                searchTransform.gameObject.SetActive(false);
                _viewModel.ClearLevels();
            });

            searchOverlayButton.onClick.AddListener(() => { searchTransform.gameObject.SetActive(false); });
            searchButton.onClick.AddListener(() => { searchTransform.gameObject.SetActive(true); });


            _viewModel.SearchQuery.ToObservable().Subscribe(value => {
                _searchButtonImage.color = value.Length == 0 ? Color.white : new Color(0, 125, 255, 255);
            });


            _viewModel.Levels.ObserveAdd().Subscribe(addEvent => { InstantiateLevel(addEvent.Value, addEvent.Index); });
            _viewModel.Levels.ObserveReplace().Subscribe(replaceEvent => {
                UpdateLevel(_levelObjectList[replaceEvent.Index], replaceEvent.NewValue, replaceEvent.Index);
            });
            _viewModel.Levels.ObserveRemove().Subscribe(removeEvent => {
                Destroy(_levelObjectList[removeEvent.Index]);
                _levelObjectList.RemoveAt(removeEvent.Index);
            });

            _viewModel.FetchNextPage();
        }

        public void Update() {
            if (levelScrollHandle.anchorMin.y <= 5.0f / _viewModel.LoadedLevelAmount) {
                _viewModel.FetchNextPage();
            }
        }

        public void MoveToEditor(int id) {
            OpenedByThisScene = true;
            bool success = LevelFileRepository.Instance.LoadLevel(id).Result;
        }

        private void InstantiateLevel(LevelModel levelModel, int idx) {
            var o = Instantiate(Assets.LevelPrefab, levelsParent);
            UpdateLevel(o, levelModel, idx);
            _levelObjectList.Add(o);
            o.SetActive(true);
        }

        private void UpdateLevel(GameObject o, LevelModel levelModel, int idx) {
            var t = o.transform;
            var icon = t.GetChild(0);

            var level = levelModel.Level;

            var iconSprite =
                Assets.Bundle.LoadAsset<Sprite>($"Assets/Images/difficultyIcons/{level.Difficulty}.png");

            icon.gameObject.GetComponent<Image>().sprite = iconSprite;

            t.GetChild(1).GetComponent<Text>().text = String.Join(" & ", level.Artists);
            t.GetChild(2).GetComponent<Text>().text = level.Song;
            t.GetChild(4).GetComponent<Text>().text = String.Join(" & ", level.Creators);
            t.GetChild(6).GetComponent<Text>().text =
                level.MinBpm.Equals(level.MaxBpm) ? $"{level.MinBpm}" : $"{level.MinBpm}-{level.MaxBpm}";
            t.GetChild(8).GetComponent<Text>().text = level.Tiles.ToString();
            t.GetChild(10).GetComponent<Text>().text = level.Comments.ToString();
            t.GetChild(12).GetComponent<Text>().text = level.Comments.ToString();

            var buttons = t.GetChild(13);
            var downloadOrPlay = buttons.GetChild(0);
            var playWorkshop = buttons.GetChild(1);
            var btnDownload = downloadOrPlay.GetComponent<Button>();
            btnDownload.interactable = true;

            if (levelModel.LevelExists) {
                downloadOrPlay.GetChild(0).gameObject.SetActive(false);
                downloadOrPlay.GetChild(1).gameObject.SetActive(true);
                buttons.GetChild(2).gameObject.SetActive(true);
                buttons.GetChild(2).GetComponent<Button>().onClick.AddListener(() => _viewModel.DeleteLevel(idx));

                btnDownload.onClick.AddListener(() => MoveToEditor(level.Id));
            } else {
                downloadOrPlay.GetChild(0).gameObject.SetActive(true);
                downloadOrPlay.GetChild(1).gameObject.SetActive(false);
                buttons.GetChild(2).gameObject.SetActive(false);

                btnDownload.onClick.AddListener(() => _viewModel.DownloadLevel(idx));

                btnDownload.interactable = levelModel.CanDownload;

            }
        }
    }
}