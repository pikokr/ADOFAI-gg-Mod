using ADOFAI_GG.Data.Entity.Remote.Types;

namespace ADOFAI_GG.Presentation.Model {
    class LevelModel {
        public readonly Level Level;
        public readonly bool LevelExists;
        public readonly bool CanDownload;

        public LevelModel(Level level, bool levelExists, bool canDownload) {
            Level = level;
            LevelExists = levelExists;
            CanDownload = canDownload;
        }
    }
}