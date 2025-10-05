using Object = UnityEngine.Object;

namespace UI.ViewControllerBase
{
    public class ViewControllerBase<T> where T : ViewBase
    {
        protected T _view;
        
        public ViewControllerBase(T view)
        {
            _view = view;
            _view.OnDestroyAction += Dispose;
        }

        public virtual void Dispose()
        {
            if (_view != null)
            {
                if (_view.gameObject != null)
                {
                    Object.Destroy(_view.gameObject);
                }
            }

            _view = null;
        }
    }
}