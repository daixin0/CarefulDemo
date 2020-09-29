

namespace Careful.Core
{
    public interface IView
    {
        object DataContext { get; set; }
        bool? ShowDialog();
        void Show();

    }
}
