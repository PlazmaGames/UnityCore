using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using PlazmaGames.Attribute;
using UnityEngine.Events;

namespace PlazmaGames.UI
{
    public sealed class UIMonoSystem : MonoBehaviour, IUIMonoSystem
    {
        [SerializeField, View] private string _startingView;
        [SerializeField] private View[] _views;

        private View _currentView;
        private readonly Stack<View> _history = new();

        private bool _inTransition;
        private Func<bool> _canTransitionCallback;
        private System.Type _nextViewAfterTransition;
        private View _viewBeforeTransition;

        public bool GetCurrentViewIs<T>() where T : View
        {
            return _currentView is T;
        }

        public T GetCurrentView<T>() where T : View
        {
            if (GetCurrentViewIs<T>()) return _currentView as T;
            else return null;
        }

        public T GetView<T>() where T : View
        {
            foreach (View view in _views)
            {
                if (view is T viewOfType)
                {
                    return viewOfType;
                }
            }

            return null;
        }

        private void Show(System.Type type, bool remeber = true, bool hideLastView = true)
        {
            if (_inTransition)
            {
                Debug.LogWarning("Cannot Change View During a Transition.");
                return;
            }

            foreach (View view in _views)
            {
                if (view.GetType().Equals(type))
                {
                    if (_currentView != null)
                    {
                        if (remeber) _history.Push(_currentView);
                        if (hideLastView) _currentView.Hide();
                    }

                    view.Show();
                    _currentView = view;
                }
            }
        }

        public void ShowWithTransition<TNext, TIntermediate>(Func<bool> canTransitionCallback, bool remeber = true, bool hideLastView = true) where TNext : View where TIntermediate : View
        {
            if (_inTransition) return;

            _viewBeforeTransition = (hideLastView) ? _currentView : null;
            Show<TIntermediate>(remeber, false);
            _inTransition = true;
            _canTransitionCallback = canTransitionCallback;
            _nextViewAfterTransition = typeof(TNext);
        }

        public void ShowLastWithTransition<TIntermediate>(Func<bool> canTransitionCallback, bool hideLastView = true) where TIntermediate : View
        {
            if (_inTransition) return;

            _viewBeforeTransition = (hideLastView) ? _currentView : null;
            Show<TIntermediate>(false, false);
            _inTransition = true;
            _canTransitionCallback = canTransitionCallback;
            _nextViewAfterTransition = null;
        }

        public void Show<T>(bool remeber = true, bool hideLastView = true) where T : View
        {
            Show(typeof(T), remeber, hideLastView);
        }

        /// <summary>
        /// Displays a view given the view as an input. Remeber parameter indicates
        /// if to add the view to the history or not. 
        /// </summary>
        private void Show(View view, bool remeber = true)
        {
            if (view != null)
            {
                if (_currentView != null)
                {
                    if (remeber) _history.Push(_currentView);
                    _currentView.Hide();
                }

                view.Show();
                _currentView = view;
            }
        }

        public void ShowLast()
        {
            if (_history.Count != 0)
            {
                Show(_history.Pop(), false);
            }
        }

        public void HideAllViews()
        {
            foreach (View view in _views) view.Hide();
     
        }

        public void ShowAllViews()
        {
            foreach (View view in _views) view.Show();
        }

        public bool IsInTransition()
        {
            return _inTransition;
        }

        public void ClearHistory()
        {
            _history.Clear();
        }

        private void Init()
        {
            View[] viewStilActive = _views.Where(e => e != null).ToArray();
            _views =  viewStilActive.Concat(FindObjectsOfType<MonoBehaviour>().OfType<View>()).ToArray();
    
            foreach (View view in _views)
            {
                view.Init();
                view.Hide();
            }

            Type startingViewType = Type.GetType(_startingView);
            if (startingViewType != null) Show(startingViewType, true);
        }

        private void Start()
        {
            Init();
        }

        private void Update()
        {
            if (_inTransition  && (_canTransitionCallback == null || _canTransitionCallback.Invoke()))
            {
                _inTransition = false;
                HideAllViews();
                if (_viewBeforeTransition != null) _viewBeforeTransition.Hide();
                if (_nextViewAfterTransition != null) Show(_nextViewAfterTransition, false, true);
                else ShowLast();
                _viewBeforeTransition = null;
                _canTransitionCallback = null;
                _nextViewAfterTransition = null;
            }
        }
    }
}
