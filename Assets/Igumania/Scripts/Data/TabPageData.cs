using System;
using UnityEngine;
using UnityEngine.UI;

namespace Igumania.Data
{
    [Serializable]
    public struct TabPageData : ITabPageData
    {
        [SerializeField]
        private Button tabButton;

        [SerializeField]
        private GameObject tabPagePanelGameObject;

        public Button TabButton
        {
            get => tabButton;
            set => tabButton = value;
        }

        public GameObject TabPagePanelGameObject
        {
            get => tabPagePanelGameObject;
            set => tabPagePanelGameObject = value;
        }
    }
}
