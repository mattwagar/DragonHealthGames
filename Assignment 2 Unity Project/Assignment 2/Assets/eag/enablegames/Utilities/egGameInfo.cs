using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace enableGame
{
    public class egGameInfo : MonoBehaviour
    {

        [SerializeField]
        private string _GameName;
        [SerializeField]
        private string _VersionNumber;

        private static egGameInfo instance;
        public static egGameInfo Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new egGameInfo();
                }
                return instance;
            }
        }

        private string gameName;
        public string GameName
        {
            get
            {
                return gameName;
            }
        }

        private string versionNumber;
        public string VersionNumber
        {
            get
            {
                return versionNumber;
            }
        }

        // Use this for initialization
        void Awake()
        {
            gameName = _GameName;
            versionNumber = _VersionNumber;
        }
    }
}
