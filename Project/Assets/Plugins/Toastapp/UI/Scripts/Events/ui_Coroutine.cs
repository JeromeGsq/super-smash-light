using UnityEngine;
using System.Collections;

namespace Root.Modules.UI {
    [System.Serializable]
    public abstract class ui_Coroutine : MonoBehaviour  {

        public abstract IEnumerator Run();
        public abstract IEnumerator Stop();
    }
}