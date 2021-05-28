using Igumania.Data;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Igumania.Controllers
{
    public class TabPagesUIControllerScript : MonoBehaviour, ITabPagesUIController
    {
        private static readonly int isSelectedHash = Animator.StringToHash("isSelected");

        [SerializeField]
        private int selectedTabPageIndex = -1;

        [SerializeField]
        private TabPageData[] tabPages = Array.Empty<TabPageData>();

        private Button selectedTabButton;

        private GameObject selectedTabPagePanelGameObject;

        private int lastSelectedTabPageIndex = -1;

        public int SelectedTabPageIndex
        {
            get => (selectedTabPageIndex < 0) ? -1 : ((selectedTabPageIndex < TabPages.Length) ? selectedTabPageIndex : TabPages.Length - 1);
            set
            {
                selectedTabPageIndex = (value < 0) ? -1 : ((value < TabPages.Length) ? value : TabPages.Length - 1);
                if (lastSelectedTabPageIndex != selectedTabPageIndex)
                {
                    lastSelectedTabPageIndex = selectedTabPageIndex;
                    UpdateVisuals();
                }
            }
        }

        public TabPageData[] TabPages
        {
            get => tabPages ?? Array.Empty<TabPageData>();
            set => tabPages = value ?? throw new ArgumentNullException(nameof(value));
        }

        private void HideSelectedTab()
        {
            if (selectedTabButton)
            {
                if (Application.isPlaying && selectedTabButton.TryGetComponent(out Animator selected_tab_button_animator))
                {
                    selected_tab_button_animator.SetBool(isSelectedHash, false);
                }
                selectedTabButton = null;
            }
            if (selectedTabPagePanelGameObject)
            {
                selectedTabPagePanelGameObject.SetActive(false);
                selectedTabPagePanelGameObject = null;
            }
        }

        private void UpdateVisuals()
        {
            TabPageData[] tab_pages = TabPages;
            HideSelectedTab();
            if ((selectedTabPageIndex >= 0) && (selectedTabPageIndex < tab_pages.Length))
            {
                TabPageData tab_page = tab_pages[selectedTabPageIndex];
                if (tab_page.TabPagePanelGameObject)
                {
                    selectedTabButton = tab_page.TabButton;
                    if (Application.isPlaying && selectedTabButton && selectedTabButton.TryGetComponent(out Animator selected_tab_button_animator))
                    {
                        selected_tab_button_animator.SetBool(isSelectedHash, true);
                    }
                    selectedTabPagePanelGameObject = tab_page.TabPagePanelGameObject;
                    if (selectedTabPagePanelGameObject)
                    {
                        selectedTabPagePanelGameObject.SetActive(true);
                    }
                }
            }
        }

        private void Start()
        {
            TabPageData[] tab_pages = TabPages;
            for (int tab_page_index = 0; tab_page_index < tab_pages.Length; tab_page_index++)
            {
                TabPageData tab_page = tab_pages[tab_page_index];
                if (tab_page.TabButton)
                {
                    int index = tab_page_index;
                    tab_page.TabButton.onClick.AddListener(() => SelectedTabPageIndex = index);
                }
            }
            lastSelectedTabPageIndex = selectedTabPageIndex;
            UpdateVisuals();
        }

        private void Update()
        {
            if (lastSelectedTabPageIndex != selectedTabPageIndex)
            {
                lastSelectedTabPageIndex = selectedTabPageIndex;
                UpdateVisuals();
            }
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            selectedTabPageIndex = (selectedTabPageIndex < 0) ? -1 : ((selectedTabPageIndex < TabPages.Length) ? selectedTabPageIndex : TabPages.Length - 1);
            UpdateVisuals();
        }
#endif
    }
}
