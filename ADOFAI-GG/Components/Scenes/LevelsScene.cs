using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using ADOFAI_GG.API;
using ADOFAI_GG.API.Filters;
using ADOFAI_GG.API.SortOrder;
using ADOFAI_GG.API.Types;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace ADOFAI_GG.Components.Scenes {
    public class LevelsScene : SceneBase {
        public static bool OpenedByThisScene;
        public static int SelectedLevelID;
        
        private List<GameObject> _objects = new List<GameObject>();

        public Transform levelsParent;
        public ScrollRect scrollRect;
        public RectTransform handle;

        private Transform Content => root.transform.GetChild(1);

        public static LevelsScene Instance => _instance;
        private static LevelsScene _instance;

        private void Awake() {

            if (Instance != null) {
                Destroy(gameObject);
                return;
            }

            _instance = this;
        }

        private void Start() {
            var t = root.transform;
            var buttons = t.GetChild(0).GetChild(1);
            var exitButton = buttons.GetChild(0).gameObject.GetComponent<Button>();
            var serachButton = buttons.GetChild(1).gameObject.GetComponent<Button>();
            exitButton.onClick.AddListener(() => { SceneManager.LoadScene("ADOFAIGG_MAIN"); });

            var searchT = Content.GetChild(3);

            var searchOverlay = searchT.GetChild(0);

            var searchInput = searchT.GetChild(1).GetComponent<InputField>();

            searchInput.onEndEdit.AddListener(query => {
                this._search = query;
                searchT.gameObject.SetActive(false);
                StartCoroutine(Fetch());
            });

            searchOverlay.GetComponent<Button>().onClick.AddListener(() => { searchT.gameObject.SetActive(false); });

            serachButton.onClick.AddListener(() => { searchT.gameObject.SetActive(true); });

            var pagination = Content.GetChild(1);
            scrollRect = GameObject.Find("InfinityScroll").GetComponent<ScrollRect>();
            levelsParent = scrollRect.transform.GetChild(0).GetChild(0);
            handle = scrollRect.transform.GetChild(2).GetChild(0).GetChild(0).GetComponent<RectTransform>();

            /*
            _prevButton = pagination.GetChild(0).GetComponent<Button>();
            _nextButton = pagination.GetChild(2).GetComponent<Button>();
            _paginationNumber = pagination.GetChild(1).GetChild(0).GetComponent<Text>();

            _prevButton.interactable = false;
            _nextButton.interactable = false;
            _prevButton.onClick.AddListener(() => {
                _prevButton.interactable = false;
                _page -= 1;
                StartCoroutine(Fetch());
            });

            _nextButton.onClick.AddListener(() => {
                _nextButton.interactable = false;
                _page += 1;
                StartCoroutine(Fetch());
            });
            */

            StartCoroutine(Fetch());
        }
        
        private string _search = string.Empty;
        /*
        private int _page;
        private double _count;
        private int MAXPage => (int) Math.Ceiling(_count / 5);
        */
        public int loadedLevels = 0;

        private void Update() {
            var searchButton = root.transform.GetChild(0).GetChild(1).GetChild(1).GetChild(1).GetComponent<Image>();
            searchButton.color = _search.Length == 0 ? Color.white : new Color(0, 125, 255, 255);
        }

        public IEnumerator Fetch() {
            //StopAllCoroutines();
            foreach (var o in _objects) {
                Destroy(o);
            }

            loadedLevels = 0;
            var filter = new LevelFilter(0, 50)
                .QueryTitle(_search)
                .QueryCreator(_search)
                .QueryArtist(_search)
                .Sort(LevelsSortOrder.RECENT_DESC);
            UpdateLevels(filter);
            yield return CheckUpdateLevels(filter);
        }
        public IEnumerator Refresh() {
            foreach (object objects in levelsParent) {
                yield return null;
            }
        }

        public List<int> downloading;

        public void AddLevel(Level level) {
            var o = Instantiate(Assets.LevelPrefab, levelsParent);
            var t = o.transform;
            var icon = t.GetChild(0);

            var iconSprite =
                Assets.Bundle.LoadAsset<Sprite>($"Assets/Images/difficultyIcons/{level.Difficulty}.png");

            icon.gameObject.GetComponent<Image>().sprite = iconSprite;

            t.GetChild(1).GetComponent<Text>().text = string.Join(" & ", level.Artists);
            t.GetChild(2).GetComponent<Text>().text = level.Song;
            t.GetChild(4).GetComponent<Text>().text = string.Join(" & ", level.Creators);
            t.GetChild(6).GetComponent<Text>().text =
                level.MinBPM.ToString(CultureInfo.InvariantCulture) ==
                level.MaxBPM.ToString(CultureInfo.InvariantCulture)
                    ? $"{level.MinBPM}"
                    : $"{level.MinBPM}-{level.MaxBPM}";
            t.GetChild(8).GetComponent<Text>().text = level.Tiles.ToString();
            t.GetChild(10).GetComponent<Text>().text = level.Comments.ToString();
            t.GetChild(12).GetComponent<Text>().text = level.Comments.ToString();
            var buttons = t.GetChild(13);
            var downloadOrPlay = buttons.GetChild(0);
            var playWorkshop = buttons.GetChild(1);
            var btnDownload = downloadOrPlay.GetComponent<Button>();
            btnDownload.interactable = true;
            if (level.CheckLevelExists()) {
                downloadOrPlay.GetChild(0).gameObject.SetActive(false);
                downloadOrPlay.GetChild(1).gameObject.SetActive(true);
                btnDownload.onClick.AddListener(() => {
                    OpenedByThisScene = true;
                    ConstObject.Instance.StartCoroutine(level.LoadLevel());
                });
                buttons.GetChild(2).gameObject.SetActive(true);
                buttons.GetChild(2).GetComponent<Button>().onClick.AddListener(level.DeleteLevel);
            } else {
                downloadOrPlay.GetChild(0).gameObject.SetActive(true);
                downloadOrPlay.GetChild(1).gameObject.SetActive(false);

                btnDownload.onClick.AddListener(() => {
                    ConstObject.Instance.StartCoroutine(level.DownloadLevel());
                    var img = downloadOrPlay.GetComponent<Image>();
                    btnDownload.interactable = false;
                });
                if (level.Download == null || 
                    level.Download.Contains("cdn.discordapp.com/attachments") ||
                    level.Download.Contains("www.mediafire.com/file/")) {
                    btnDownload.interactable = false;
                }

                buttons.GetChild(2).gameObject.SetActive(false);
            }

            _objects.Add(o);
            var btn = o.GetComponent<Button>();
            btn.onClick ??= new Button.ButtonClickedEvent();
            o.SetActive(true);
        }

        public IEnumerator CheckUpdateLevels(LevelFilter filter) {
            while (true) {
                if (handle.anchorMin.y <= 0.1f * 50 / loadedLevels) {
                    filter.Offset(loadedLevels);
                    yield return UpdateLevelsCo(filter);
                }

                yield return null;
            }
        }

        public IEnumerator UpdateLevelsCo(LevelFilter filter) {
            yield return RequestCo.RequestLevels(filter, (levels, i) => {
                foreach (var level in levels) {
                    AddLevel(level);
                    loadedLevels += 1;
                }

                return RequestCo.None;
            });
        }
        
        public void UpdateLevels(LevelFilter filter) {
            filter.Offset(loadedLevels);
            var tuple = Request.RequestLevels(filter).Result;
            if (!tuple.HasValue) return;
            var levels = tuple.Value.Item1;
            foreach (var level in levels) {
                AddLevel(level);
                loadedLevels += 1;
            }
        }


        public static void Init(Scene scn, GameObject root) {
            var obj = new GameObject("LevelsScene");
            obj.GetOrAddComponent<LevelsScene>().root = root;
        }

    }
}