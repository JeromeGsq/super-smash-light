using UnityEngine;
using Root.DesignPatterns;

namespace Root.Modules.Save {

    public class SaveModule : GlobalSingleton<SaveModule> {

        #region Fields

        [SerializeField]
        SaveConf saveConf;

        private ES2Settings es2Settings;

        private string globalSavePath;

        private string default_EasySaveFileName;
        private string fileNameExtensions;

        private bool encryptsData;
        private string encryptionPassword;


        #endregion

        #region API's

        protected override void Init() {
            base.Init();

            if(saveConf != null) {
                default_EasySaveFileName = saveConf.Default_EasySaveFileName;
                fileNameExtensions = saveConf.FileNameExtensions;

                es2Settings = new ES2Settings();

                es2Settings.encrypt = saveConf.EncryptsData;
                es2Settings.encryptionPassword = saveConf.EncryptionPassword;
                es2Settings.saveLocation = saveConf.SaveLocation;

            }

#if UNITY_EDITOR || UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX
            globalSavePath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/" + Application.companyName + "/" + Application.productName;
#endif

#if UNITY_IPHONE || UNITY_ANDROID
		globalSavePath = Application.persistentDataPath;
#endif

            Debug.Log(globalSavePath);
        }


        /// <summary>
        /// Save a generic Param of T type with a tag
        /// </summary>
        /// <typeparam name="T">Type to be saved</typeparam>
        /// <param name="_key">The tag associated with the value to be saved</param>
        /// <param name="_param">The value of the param to save</param>
        /// <param name="_saveFile">The specific filename in which EasySave will write. Uses a default file if empty.</param>
        public void Save<T>(string _key, T _param, string _saveFile = "") {
            Debug.Log("Want to save " + _param + " as " + _key + " in " + _saveFile + " //// " + es2Settings.saveLocation);
            ES2.Save(_param, globalSavePath + "/" + GetFileSave_Name(_saveFile) + "?tag=" + _key, es2Settings);

        }


        /// <summary>
        /// Load a generic saved param with its tag
        /// </summary>
        /// <typeparam name="T">The generic Type.</typeparam>
        /// <param name="_key">The tag under which a value is saved</param>
        /// <param name="_defaultValue">Its default value if not existing. The type default value if nothing specified</param>
        /// <param name="_saveFile">The specific filename in which EasySave will read from. Uses a default file if empty.</param>
        /// <returns>A saved value if saving system has the key</returns>
        public T Load<T>(string _key, T _defaultValue = default(T), string _saveFile = "") {

            if(!HasKey(_key, _saveFile)) {
                return _defaultValue;
            }

            return ES2.Load<T>(globalSavePath + "/" + GetFileSave_Name(_saveFile) + "?tag=" + _key, es2Settings);
        }




        /// <summary>
        /// Checks the presence of a key in the current saving system. 
        /// </summary>
        /// <param name="_key">The key to look for</param>
        /// <param name="_saveFile">The specific filename in which to check. Uses a default file if empty.</param>
        /// <returns>true if the current saving system has the key already saved in</returns>
        public bool HasKey(string _key, string _saveFile = "") {

            return ES2.Exists(globalSavePath + "/" + GetFileSave_Name(_saveFile) + "?tag=" + _key, es2Settings);

        }


        /// <summary>
        /// Deletes an entry from the saving system.
        /// </summary>
        /// <param name="_entry">Name of the registry entry</param>
        public void DeleteEntry(string _key, string _saveFile = "") {

            ES2.Delete(globalSavePath + "/" + GetFileSave_Name(_saveFile) + "?tag=" + _key, es2Settings);

        }


        /// <summary>
        /// Deletes en entire file.
        /// </summary>
        /// <param name="_entry">Name of the file to delete. Delete the default save file if empty.</param>
        public void DeleteFile(string _saveFileName = "") {

            ES2.Delete(globalSavePath + "/" + GetFileSave_Name(_saveFileName), es2Settings);

        }

        /// <summary>
        /// Deletes all the registry entries with PlayerPrefs or Deletes the default folder with EasySave.
        /// </summary>
        public void DeleteEverything() {

            ES2.DeleteDefaultFolder();

        }




        /// <summary>
        /// Formats the correct fileName with the correct extension.
        /// Takes the default file name if _saveFile is empty
        /// </summary>
        /// <param name="_saveFile">The given fileName without extension</param>
        /// <returns>The fileName with the correct extension.</returns>
        private string GetFileSave_Name(string _saveFile) {

            string fileName = _saveFile;
            if(string.IsNullOrEmpty(fileName)) {
                fileName = default_EasySaveFileName;
            }

            fileName += fileNameExtensions;

            return fileName;
        }


        #endregion

    }
}