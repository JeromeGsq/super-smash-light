using UnityEngine;
using System.Collections;

namespace Root.DesignPatterns {

    public abstract class SceneSingleton<T> : MonoBehaviour where T : Component {
        
        private static T mInstance;

        public static T Get {
            get {
                if(mInstance == null) {
                    mInstance = GameObject.FindObjectOfType<T>();                    
                }
                return mInstance;
            }
        }

        public static bool Instantiated {
            get {
                return mInstance != null;
            }
        }
    }

}