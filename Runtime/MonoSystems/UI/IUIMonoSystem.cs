using PlazmaGames.Core.MonoSystem;

namespace PlazmaGames.UI
{
    public interface IUIMonoSystem : IMonoSystem
    {
        /// <summary>
        /// Checks if current View is of type T
        /// </summary>
        public bool GetCurrentViewIs<T>() where T : View;

        /// <summary>
        /// Gets the current view if the view is of type T
        /// </summary>
        public T GetCurrentView<T>() where T : View;

        /// <summary>
        /// Gets a View of type T from the master view list
        /// </summary>
        public T GetView<T>() where T : View;

        /// <summary>
        /// Displays a view of type T. Remeber parameter indicates if to add the view
        /// to the history or not. 
        /// </summary>
        public void Show<T>(bool remeber = true) where T : View;

        /// <summary>
        /// Displays the last view in the history
        /// </summary>
        public void ShowLast();

        /// <summary>
        /// Hides all Views.
        /// </summary>
        public void HideAllViews();

        /// <summary>
        /// Shows all Views.
        /// </summary>
        public void ShowAllViews();
    }
}
