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

namespace ADOFAI_GG.Presentation.ViewModel.Scene
{
    class LevelsViewModel
    {
        private static LevelsViewModel instance;
        
        public static LevelsViewModel getInstance()
        {
            if (instance == null)
            {
                instance = new LevelsViewModel(
                    LevelRepository.GetInstance(),
                    LevelFileRepository.GetInstance());
            }
            return instance;
        }

        private const int PAGE_SIZE = 50;


        private readonly LevelRepository levelRepository;
        private readonly LevelFileRepository levelFileRepository;

        private readonly ReactiveCollection<LevelModel> levels;
        private readonly AsyncReactiveProperty<string> searchQuery;
        private readonly AsyncReactiveProperty<int> page;
        private readonly AsyncReactiveProperty<int> count;
        private readonly IUniTaskAsyncEnumerable<int> maxPage;
        private readonly IUniTaskAsyncEnumerable<Tuple<int, int>> currentPageInfo;

        private bool blockLoading = false;

        protected LevelsViewModel(LevelRepository levelRepository, LevelFileRepository levelFileRepository)
        {
            this.levelRepository = levelRepository;
            this.levelFileRepository = levelFileRepository;
            
            this.levels = new ReactiveCollection<LevelModel>();
            this.searchQuery = new AsyncReactiveProperty<string>("");
            this.page = new AsyncReactiveProperty<int>(0);
            this.count = new AsyncReactiveProperty<int>(0);
            // when count updated, maxPage updated
            this.maxPage = this.count.Select(count => count / PAGE_SIZE + (count % PAGE_SIZE != 0 ? 1 : 0));

            // when page or maxPage changed, currentPageInfo updated
            this.currentPageInfo = this.page.CombineLatest(maxPage, Tuple.Create);
        }
        
        public void ClearLevels()
        {
            while (levels.Count > 0)
            {
                levels.RemoveAt(0);
            }
            page.Value = 0;
        }

        public void FetchNextPage()
        {
            if (blockLoading) return;
            blockLoading = true;

            UniTask task = UniTask.RunOnThreadPool(async () =>
            {
                // Worker thread
                LevelFilter filter = new LevelFilter(page.Value * PAGE_SIZE, PAGE_SIZE)
                    .QueryTitle(searchQuery.Value)
                    .QueryCreator(searchQuery.Value)
                    .QueryArtist(searchQuery.Value)
                    .Sort(LevelsSortOrder.RECENT_DESC);

                var tuple = await levelRepository.RequestLevels(filter);
                if (!tuple.HasValue)
                {
                    blockLoading = false;
                    return;
                }

                await UniTask.SwitchToMainThread();

                page.Value++;

                count.Value = tuple.Value.Item2;
                foreach (Level level in tuple.Value.Item1)
                {
                    levels.Add(GetLevelModel(level));
                }

                blockLoading = false;
            });
        }

        public void DeleteLevel(int idx)
        {
            levelFileRepository.DeleteLevel(levels[idx].level.Id);
            levels[idx] = GetLevelModel(levels[idx].level);
        }

        public void DownloadLevel(int idx)
        {
            UniTask.RunOnThreadPool(async () =>
            {
                // warning : this can cause problem in future.
                // if level list is reset while downloading level,
                // that can cause data mismatch or ArgumentOutOfRangeException
                bool success = await levelFileRepository.DownloadLevel(levels[idx].level);
                await UniTask.SwitchToMainThread();
                levels[idx] = GetLevelModel(levels[idx].level);
            });
        }

        private LevelModel GetLevelModel(Level level)
        {
            return new LevelModel(level, 
                levelFileRepository.IsLevelExists(level.Id),
                levelFileRepository.isDownloadable(level.Download));
        }

        public int GetLoadedLevelAmount()
        {
            return levels.Count;
        }

        public ReactiveCollection<LevelModel> GetLevels()
        {
            return levels;
        }

        public AsyncReactiveProperty<string> GetSearchQuery()
        {
            return searchQuery;
        }

        public AsyncReactiveProperty<int> GetPage()
        {
            return page;
        }

        public IObservable<int> GetCount()
        {
            return count.ToObservable();
        }
        
        public IUniTaskAsyncEnumerable<Tuple<int, int>> GetCurrentPageInfo()
        {
            return currentPageInfo;
        }

    }
}
