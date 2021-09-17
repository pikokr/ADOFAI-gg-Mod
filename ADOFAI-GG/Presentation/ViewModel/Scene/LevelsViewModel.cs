using ADOFAI_GG.Data.Entity.Remote;
using ADOFAI_GG.Data.Repository;
using ADOFAI_GG.Domain.Model.Levels;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using MelonLoader;
using System;
using System.Collections.Generic;
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
                    LevelRepository.GetInstance());
            }
            return instance;
        }

        private const int PAGE_SIZE = 5;


        private readonly LevelRepository levelRepository;

        private readonly AsyncReactiveProperty<List<Level>> levels;
        private readonly AsyncReactiveProperty<string> searchQuery;
        private readonly AsyncReactiveProperty<int> page;
        private readonly AsyncReactiveProperty<int> count;
        private readonly IUniTaskAsyncEnumerable<int> maxPage;
        private readonly IUniTaskAsyncEnumerable<Tuple<int, int>> currentPageInfo;

        protected LevelsViewModel(LevelRepository levelRepository)
        {
            this.levelRepository = levelRepository;
            
            this.levels = new AsyncReactiveProperty<List<Level>>(null);
            this.searchQuery = new AsyncReactiveProperty<string>("");
            this.page = new AsyncReactiveProperty<int>(0);
            this.count = new AsyncReactiveProperty<int>(0);
            // when count updated, maxPage updated
            this.maxPage = this.count.Select(count => count / PAGE_SIZE + (count % PAGE_SIZE != 0 ? 1 : 0));

            // when page or maxPage changed, currentPageInfo updated
            this.currentPageInfo = this.page.CombineLatest(maxPage, Tuple.Create);
        }
        
        public void FetchLevels()
        {
            levels.Value = null;
            UniTask task = UniTask.RunOnThreadPool(async () =>
            {
                // Worker thread
                LevelSearchResult result = await levelRepository.Search(page.Value, PAGE_SIZE, searchQuery.Value, "RECENT_DESC");
                
                // Main thread
                await UniTask.SwitchToMainThread();

                // update count & levels value
                count.Value = result.count;
                levels.Value = result.results;
            });
        }

        public IObservable<List<Level>> GetLevels()
        {
            return levels.ToObservable();
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
