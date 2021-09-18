using UnityEngine;

namespace ADOFAI_GG {
    public class ConstObject: MonoBehaviour {
        private static ConstObject _instance;

        public static ConstObject Instance {
            get {
                if (_instance == null) {
                    var obj = new GameObject();
                    DontDestroyOnLoad(obj);
                    _instance = obj.AddComponent<ConstObject>();
                }

                return _instance;
            }
        }
    }
}