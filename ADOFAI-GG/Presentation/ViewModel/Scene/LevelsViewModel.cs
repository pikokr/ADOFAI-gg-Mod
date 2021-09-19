using ADOFAI_GG.Data.Entity.Remote;
using ADOFAI_GG.Data.Entity.Remote.Filters;
using ADOFAI_GG.Data.Entity.Remote.SortOrder;
using ADOFAI_GG.Data.Entity.Remote.Types;
using ADOFAI_GG.Data.Repository;
using ADOFAI_GG.Presentation.Model;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using System;
using UniRx;

namespace ADOFAI_GG.Presentation.ViewModel.Scene {
    class LevelsViewModel {
        private static LevelsViewModel _instance;

        public static LevelsViewModel Instance =>
            _instance ??= new LevelsViewModel(LevelRepository.Instance, LevelFileRepository.Instance);

        private const int PAGE_SIZE = 50;


        private readonly LevelRepository _levelRepository;
        private readonly LevelFileRepository _levelFileRepository;

        private readonly AsyncReactiveProperty<int> _count;
        private readonly IUniTaskAsyncEnumerable<int> _maxPage;

        private bool _blockLoading = false;

        public int LoadedLevelAmount => Levels.Count;
        public ReactiveCollection<LevelModel> Levels { get; }
        public AsyncReactiveProperty<string> SearchQuery { get; }
        public AsyncReactiveProperty<int> Page { get; }
        public IObservable<int> Count => _count.ToObservable();
        public IUniTaskAsyncEnumerable<Tuple<int, int>> CurrentPageInfo { get; }

        protected LevelsViewModel(LevelRepository levelRepository, LevelFileRepository levelFileRepository) {
            _levelRepository = levelRepository;
            _levelFileRepository = levelFileRepository;

            Levels = new ReactiveCollection<LevelModel>();
            SearchQuery = new AsyncReactiveProperty<string>("");
            Page = new AsyncReactiveProperty<int>(0);
            _count = new AsyncReactiveProperty<int>(0);
            // when count updated, maxPage updated
            _maxPage = _count.Select(count => count / PAGE_SIZE + (count % PAGE_SIZE != 0 ? 1 : 0));

            // when page or maxPage changed, currentPageInfo updated
            CurrentPageInfo = Page.CombineLatest(_maxPage, Tuple.Create);
        }

        public void ClearLevels() {
            while (Levels.Count > 0) {
                Levels.RemoveAt(0);
            }

            Page.Value = 0;
        }

        public void FetchNextPage() {
            if (_blockLoading) return;
            _blockLoading = true;

            var task = UniTask.RunOnThreadPool(async () => {
                // Worker thread
                var filter = new LevelFilter(Page.Value * PAGE_SIZE, PAGE_SIZE) {
                    query = SearchQuery.Value,
                    sort = LevelsSortOrder.RECENT_DESC,
                };

                var tuple = await _levelRepository.RequestLevels(filter);
                if (!tuple.HasValue) {
                    _blockLoading = false;
                    return;
                }

                await UniTask.SwitchToMainThread();

                Page.Value++;

                _count.Value = tuple.Value.Item2;
                foreach (Level level in tuple.Value.Item1) {
                    Levels.Add(GetLevelModel(level));
                }

                _blockLoading = false;
            });
        }

        public void DeleteLevel(int idx) {
            _levelFileRepository.DeleteLevel(Levels[idx].Level.Id);
            Levels[idx] = GetLevelModel(Levels[idx].Level);
        }

        public void DownloadLevel(int idx) {
            UniTask.RunOnThreadPool(async () => {
                // warning : this can cause problem in future.
                // if level list is reset while downloading level,
                // that can cause data mismatch or ArgumentOutOfRangeException
                bool success = await _levelFileRepository.DownloadLevel(Levels[idx].Level);
                await UniTask.SwitchToMainThread();
                Levels[idx] = GetLevelModel(Levels[idx].Level);
            });
        }

        private LevelModel GetLevelModel(Level level) {
            return new LevelModel(level,
                _levelFileRepository.IsLevelExists(level.Id),
                LevelFileRepository.IsDownloadable(level.Download));
        }
    }
}