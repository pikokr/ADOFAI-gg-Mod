using ADOFAI_GG.Domain.Model.Levels;
using System;
using System.Collections.Generic;

namespace ADOFAI_GG.Data.Entity.Remote
{
    [Serializable]
    public class LevelSearchResult
    {
        public List<Level> results;
        public int count;
    }

}
