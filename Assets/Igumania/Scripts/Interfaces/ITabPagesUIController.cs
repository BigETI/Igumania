using Igumania.Data;

namespace Igumania
{
    public interface ITabPagesUIController : IBehaviour
    {
        int SelectedTabPageIndex { get; set; }

        TabPageData[] TabPages { get; set; }
    }
}
