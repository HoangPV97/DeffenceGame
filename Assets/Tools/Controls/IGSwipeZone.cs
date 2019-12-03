using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace InviGiant.Tools
{	
	/// <summary>
	/// The possible directions a swipe can have
	/// </summary>
	public enum IGPossibleSwipeDirections { Up, Down, Left, Right }


	[System.Serializable]
	public class SwipeEvent : UnityEvent<IGSwipeEvent> {}

	/// <summary>
	/// An event usually triggered when a swipe happens. It contains the swipe "base" direction, and detailed information if needed (angle, length, origin and destination
	/// </summary>
	public struct IGSwipeEvent
	{
		public IGPossibleSwipeDirections SwipeDirection;
		public float SwipeAngle;
		public float SwipeLength;
		public Vector2 SwipeOrigin;
		public Vector2 SwipeDestination;

		/// <summary>
		/// Initializes a new instance of the <see cref="MoreMountains.Tools.MMSwipeEvent"/> struct.
		/// </summary>
		/// <param name="direction">Direction.</param>
		/// <param name="angle">Angle.</param>
		/// <param name="length">Length.</param>
		/// <param name="origin">Origin.</param>
		/// <param name="destination">Destination.</param>
		public IGSwipeEvent(IGPossibleSwipeDirections direction, float angle, float length, Vector2 origin, Vector2 destination)
		{
			SwipeDirection = direction;
			SwipeAngle = angle;
			SwipeLength = length;
			SwipeOrigin = origin;
			SwipeDestination = destination;
		}
	}

	[RequireComponent(typeof(RectTransform))]
	/// <summary>
	/// Add a swipe manager to your scene, and it'll trigger MMSwipeEvents everytime a swipe happens. From its inspector you can determine the minimal length of a swipe. Shorter swipes will be ignored
	/// </summary>
	public class MMSwipeZone : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler, IPointerEnterHandler
	{
		/// the minimal length of a swipe
		public float MinimalSwipeLength = 50f;
		/// the maximum press length of a swipe
		public float MaximumPressLength = 10f;

		/// The method(s) to call when the zone is swiped
		public SwipeEvent ZoneSwiped;
		/// The method(s) to call while the zone is being pressed
		public UnityEvent ZonePressed;

		[Header("Mouse Mode")]	
		/// If you set this to true, you'll need to actually press the button for it to be triggered, otherwise a simple hover will trigger it (better for touch input).
		public bool MouseMode = false;

		protected Vector2 _firstTouchPosition;
		protected float _angle;
		protected float _length;
		protected Vector2 _destination;
		protected Vector2 _deltaSwipe;
		protected IGPossibleSwipeDirections _swipeDirection;

		protected virtual void Swipe()
		{
			IGSwipeEvent swipeEvent = new IGSwipeEvent (_swipeDirection, _angle, _length, _firstTouchPosition, _destination);
			IGEventManager.TriggerEvent(swipeEvent);
			if (ZoneSwiped != null)
			{
				ZoneSwiped.Invoke (swipeEvent);
			}
		}

		protected virtual void Press()
		{
			if (ZonePressed != null)
			{
				ZonePressed.Invoke ();
			}
		}

		/// <summary>
		/// Triggers the bound pointer down action
		/// </summary>
		public virtual void OnPointerDown(PointerEventData data)
		{
			_firstTouchPosition = Input.mousePosition;
		}

		/// <summary>
		/// Triggers the bound pointer up action
		/// </summary>
		public virtual void OnPointerUp(PointerEventData data)
		{
			_destination = Input.mousePosition;
			_deltaSwipe = _destination - _firstTouchPosition;
			_length = _deltaSwipe.magnitude;

			// if the swipe has been long enough
			if (_length > MinimalSwipeLength)
			{
				_angle = IGMaths.AngleBetween (_deltaSwipe, Vector2.right);
				_swipeDirection = AngleToSwipeDirection (_angle);
				Swipe ();
			}

			// if it's just a press
			if (_deltaSwipe.magnitude < MaximumPressLength)
			{
				Press ();
			}
		}

		/// <summary>
		/// Triggers the bound pointer enter action when touch enters zone
		/// </summary>
		public void OnPointerEnter(PointerEventData data)
		{
			if (!MouseMode)
			{
				OnPointerDown (data);
			}
		}

		/// <summary>
		/// Triggers the bound pointer exit action when touch is out of zone
		/// </summary>
		public void OnPointerExit(PointerEventData data)
		{
			if (!MouseMode)
			{
				OnPointerUp(data);	
			}
		}

		/// <summary>
		/// Determines a MMPossibleSwipeDirection out of an angle in degrees. 
		/// </summary>
		/// <returns>The to swipe direction.</returns>
		/// <param name="angle">Angle in degrees.</param>
		protected virtual IGPossibleSwipeDirections AngleToSwipeDirection(float angle)
		{
			if ((angle < 45) || (angle >= 315))
			{
				return IGPossibleSwipeDirections.Right;
			}
			if ((angle >= 45) && (angle < 135))
			{
				return IGPossibleSwipeDirections.Up;
			}
			if ((angle >= 135) && (angle < 225))
			{
				return IGPossibleSwipeDirections.Left;
			}
			if ((angle >= 225) && (angle < 315))
			{
				return IGPossibleSwipeDirections.Down;
			}
			return IGPossibleSwipeDirections.Right;
		}
	}
}