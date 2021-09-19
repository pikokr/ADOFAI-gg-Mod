using ADOFAI_GG.Data.Entity.Remote.Types;

namespace ADOFAI_GG.Presentation.Model
{
    class LevelModel
    {

        public readonly Level level;
        public readonly bool isLevelExists;
        public readonly bool canDownload;

        public LevelModel(Level level, bool isLevelExists, bool canDownload)
        {
            this.level = level;
            this.isLevelExists = isLevelExists;
            this.canDownload = canDownload;
        }

    }



}
