using UnityEngine;

namespace Root.Modules.Save {

    public class SaveConf : ScriptableObject {

        [SerializeField]
        [Tooltip("The Default name of the save file")]
        private string default_EasySaveFileName = "saves";

        [SerializeField]
        [Tooltip("The default file extensions for save files")]
        private string fileNameExtensions = "";

        [SerializeField]
        [Tooltip("Enables encryption with save data ?")]
        private bool encryptsData = false;

        [SerializeField]
        [Tooltip("Encryption password for save files")]
        private string encryptionPassword;

        [SerializeField]
        [Tooltip("Save Location")]
        private ES2Settings.SaveLocation saveLocation = ES2Settings.SaveLocation.File;

        /// <summary>
        /// File name by default when using EasySave
        /// </summary>
        public string Default_EasySaveFileName {
            get {
                return default_EasySaveFileName;
            }

            set {
                default_EasySaveFileName = value;
            }
        }

        /// <summary>
        /// File name Extension by efault, for all save file with EasySave
        /// </summary>
        public string FileNameExtensions {
            get {
                return fileNameExtensions;
            }

            set {
                fileNameExtensions = value;
            }
        }

        /// <summary>
        /// Activates data Encryption  with EasySave
        /// </summary>
        public bool EncryptsData {
            get {
                return encryptsData;
            }

            set {
                encryptsData = value;
            }
        }

        /// <summary>
        /// Password for encryption when encryption is enabled with EasySave
        /// </summary>
        public string EncryptionPassword {
            get {
                return encryptionPassword;
            }

            set {
                encryptionPassword = value;
            }
        }

        /// <summary>
        /// Where data should be saved
        /// </summary>
        public ES2Settings.SaveLocation SaveLocation {
            get {
                return saveLocation;
            }

            set {
                saveLocation = value;
            }
        }
    }
}