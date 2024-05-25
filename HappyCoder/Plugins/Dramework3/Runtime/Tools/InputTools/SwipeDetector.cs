using System;

using UnityEngine;


namespace IG.HappyCoder.Dramework3.Runtime.Tools.InputTools
{
    public class SwipeDetector
    {
        #region ================================ FIELDS

        private Vector2 _fingerDownPosition;
        private Vector2 _fingerUpPosition;
        private readonly float _swipeMinDistance;
        private readonly bool _detectSwipeOnUp;
        private SwipeData _swipeData;

        #endregion

        #region ================================ CONSTRUCTORS AND DESTRUCTOR

        public SwipeDetector(float swipeMinDistance, bool detectSwipeOnUp)
        {
            _swipeMinDistance = swipeMinDistance;
            _detectSwipeOnUp = detectSwipeOnUp;
        }

        #endregion

        #region ================================ EVENTS AND DELEGATES

        public event Action<SwipeData> onSwipe;

        #endregion

        #region ================================ METHODS

        public void Update()
        {
#if UNITY_EDITOR

            if (Input.GetMouseButtonDown(0))
            {
                _fingerDownPosition = Input.mousePosition;
                _fingerUpPosition = Input.mousePosition;
            }

            if (_detectSwipeOnUp == false && Input.GetMouseButton(0))
            {
                _fingerDownPosition = Input.mousePosition;
                DetectSwipe();
            }

            if (Input.GetMouseButtonUp(0) == false) return;
            _fingerDownPosition = Input.mousePosition;
            DetectSwipe();

#else
            foreach (var touch in Input.touches)
            {
                if (touch.phase == TouchPhase.Began)
                {
                    _fingerDownPosition = touch.position;
                    _fingerUpPosition = touch.position;
                }

                if (_detectSwipeOnUp == false && touch.phase == TouchPhase.Moved)
                {
                    _fingerDownPosition = touch.position;
                    DetectSwipe();
                }

                if (touch.phase != TouchPhase.Ended) continue;
                _fingerDownPosition = touch.position;
                DetectSwipe();
            }
#endif
        }

        private void DetectSwipe()
        {
            if (SwipeDistanceCheckMet() == false) return;

            if (IsVerticalSwipe())
            {
                var direction = _fingerDownPosition.y - _fingerUpPosition.y > 0 ? SwipeDirection.Up : SwipeDirection.Down;
                SendSwipe(direction);
            }
            else
            {
                var direction = _fingerDownPosition.x - _fingerUpPosition.x > 0 ? SwipeDirection.Right : SwipeDirection.Left;
                SendSwipe(direction);
            }

            _fingerUpPosition = _fingerDownPosition;
        }

        private float HorizontalMovementDistance()
        {
            return Mathf.Abs(_fingerDownPosition.x - _fingerUpPosition.x);
        }

        private bool IsVerticalSwipe()
        {
            return VerticalMovementDistance() > HorizontalMovementDistance();
        }

        private void SendSwipe(SwipeDirection direction)
        {
            _swipeData.Direction = direction;
            _swipeData.StartPosition = _fingerDownPosition;
            _swipeData.EndPosition = _fingerUpPosition;
            onSwipe?.Invoke(_swipeData);
        }

        private bool SwipeDistanceCheckMet()
        {
            return VerticalMovementDistance() > _swipeMinDistance || HorizontalMovementDistance() > _swipeMinDistance;
        }

        private float VerticalMovementDistance()
        {
            return Mathf.Abs(_fingerDownPosition.y - _fingerUpPosition.y);
        }

        #endregion
    }
}