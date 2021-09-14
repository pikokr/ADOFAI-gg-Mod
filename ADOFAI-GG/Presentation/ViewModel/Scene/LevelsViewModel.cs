using ADOFAI_GG.Data.Entity.Remote;
using ADOFAI_GG.Data.Repository;
using ADOFAI_GG.Domain.Model.Levels;
using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        private readonly ReactiveCollection<Level> levels;
        private readonly ReactiveProperty<string> searchQuery;
        private readonly ReactiveProperty<int> page;
        private readonly ReactiveProperty<int> count;
        private readonly IObservable<int> maxPage;

        protected LevelsViewModel(LevelRepository levelRepository)
        {
            this.levelRepository = levelRepository;
            this.levels = new ReactiveCollection<Level>();
            this.searchQuery = new ReactiveProperty<string>("");
            this.page = new ReactiveProperty<int>(0);
            this.count = new ReactiveProperty<int>(0);
            this.maxPage = count.Select(count => count / PAGE_SIZE + (count % PAGE_SIZE != 0 ? 1 : 0));
        }
        
        public void fetchData()
        {

            UniTask.RunOnThreadPool(async () =>
            {
                // Worker thread
                LevelSearchResult result = await levelRepository.Search(page.Value, PAGE_SIZE, searchQuery.Value, "RECENT_DESC");

                await UniTask.SwitchToMainThread();

                // Main thread
                this.count.Value = result.count;
                levels.Clear();
                result.results.ForEach(levels.Add);

            });

        }

        


    }
}
