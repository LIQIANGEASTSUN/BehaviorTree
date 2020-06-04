using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CommonUtils {

    public static class FileUtils {

        public static string CombinePath(params string[] args) {
            return string.Join(Separator + "", args);
        }

        public static char Separator {
            get {
#if UNITY_EDITOR_WIN
                return '/';
#else
                return System.IO.Path.DirectorySeparatorChar;
#endif
            }
        }

        public static string GetStreamingAssetsFilePath(string subfoldername, string filename) {
            string sub = "";
            if (!string.IsNullOrEmpty(filename)) {
                sub = filename + "/";
            }
            switch (Application.platform) {
                case RuntimePlatform.Android:
                    return "jar:file://" + Application.dataPath + "!/assets/" + sub + subfoldername;
                case RuntimePlatform.IPhonePlayer:
                    return "file://" + Application.dataPath + "/Raw/" + sub + subfoldername;
                default:
                    return "file://" + Application.streamingAssetsPath + "/" + sub + subfoldername;
            }
        }
    }

}

