using UnityEngine;
using UnityEngine.UI;

namespace Igumania
{
    public interface ITabPageData
    {
        Button TabButton { get; set; }

        GameObject TabPagePanelGameObject { get; set; }
    }
}
