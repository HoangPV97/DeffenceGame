#define EVENTROUTER_THROWEXCEPTIONS 
#if EVENTROUTER_THROWEXCEPTIONS
//#define EVENTROUTER_REQUIRELISTENER // Uncomment this if you want listeners to be required for sending events.
#endif

using System;
using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

namespace InviGiant.Tools
{	
	/// <summary>
	/// IGGameEvents are used throughout the game for general game events (game started, game ended, life lost, etc.)
	/// </summary>
	public struct IGGameEvent
	{
		public string EventName;
		public IGGameEvent(string newName)
		{
			EventName = newName;
		}
	}

	public struct IGSfxEvent
	{
		public AudioClip ClipToPlay;
		public IGSfxEvent(AudioClip clipToPlay)
		{
			ClipToPlay = clipToPlay;
		}
	}

	/// <summary>
	/// This class handles event management, and can be used to broadcast events throughout the game, to tell one class (or many) that something's happened.
	/// Events are structs, you can define any kind of events you want. This manager comes with IGGameEvents, which are 
	/// basically just made of a string, but you can work with more complex ones if you want.
	/// 
	/// To trigger a new event, from anywhere, just call IGEventManager.TriggerEvent(YOUR_EVENT);
	/// For example : IGEventManager.TriggerEvent(new IGGameEvents("GameStart")); will broadcast an IGGameEvents named GameStart to all listeners.
	///
	/// To start listening to an event from any class, there are 3 things you must do : 
	///
	/// 1 - tell that your class implements the IGEventListener interface for that kind of event.
	/// For example: public class GUIManager : Singleton<GUIManager>, IGEventListener<IGGameEvents>
	/// You can have more than one of these (one per event type).
	///
	/// 2 - On Enable and Disable, respectively start and stop listening to the event :
	/// void OnEnable()
	/// {
	/// 	this.MMEventStartListening<IGGameEvents>();
	/// }
	/// void OnDisable()
	/// {
	/// 	this.MMEventStopListening<IGGameEvents>();
	/// }
	/// 
	/// 3 - Implement the IGEventListener interface for that event. For example :
	/// public void OnMMEvent(IGGameEvents gameEvent)
	/// {
	/// 	if (gameEvent.eventName == "GameOver")
	///		{
	///			// DO SOMETHING
	///		}
	/// } 
	/// will catch all events of type IGGameEvents emitted from anywhere in the game, and do something if it's named GameOver
	/// </summary>
	public static class IGEventManager 
	{
	    private static Dictionary<Type, List<IGEventListenerBase>> _subscribersList;

		static IGEventManager()
	    {
	        _subscribersList = new Dictionary<Type, List<IGEventListenerBase>>();
	    }

	    /// <summary>
	    /// Adds a new subscriber to a certain event.
	    /// </summary>
		/// <param name="listener">listener.</param>
	    /// <typeparam name="MMEvent">The event type.</typeparam>
	    public static void AddListener<MMEvent>( IGEventListener<MMEvent> listener ) where MMEvent : struct
	    {
	        Type eventType = typeof( MMEvent );

	        if( !_subscribersList.ContainsKey( eventType ) )
	            _subscribersList[eventType] = new List<IGEventListenerBase>();

	        if( !SubscriptionExists( eventType, listener ) )
	            _subscribersList[eventType].Add( listener );
	    }

	    /// <summary>
	    /// Removes a subscriber from a certain event.
	    /// </summary>
		/// <param name="listener">listener.</param>
	    /// <typeparam name="MMEvent">The event type.</typeparam>
	    public static void RemoveListener<MMEvent>( IGEventListener<MMEvent> listener ) where MMEvent : struct
	    {
	        Type eventType = typeof( MMEvent );

	        if( !_subscribersList.ContainsKey( eventType ) )
	        {
				#if EVENTROUTER_THROWEXCEPTIONS
					throw new ArgumentException( string.Format( "Removing listener \"{0}\", but the event type \"{1}\" isn't registered.", listener, eventType.ToString() ) );
				#else
					return;
				#endif
	        }

			List<IGEventListenerBase> subscriberList = _subscribersList[eventType];
	        bool listenerFound = false;

			foreach(IGEventListenerBase subscriber in subscriberList )
	        {
	            if( subscriber == listener )
	            {
	                subscriberList.Remove( subscriber );
	                listenerFound = true;

	                if( subscriberList.Count == 0 )
	                    _subscribersList.Remove( eventType );

	                return;
	            }
	        }

			#if EVENTROUTER_THROWEXCEPTIONS
		        if( !listenerFound )
		        {
					throw new ArgumentException( string.Format( "Removing listener, but the supplied receiver isn't subscribed to event type \"{0}\".", eventType.ToString() ) );
		        }
			#endif
	    }

	    /// <summary>
	    /// Triggers an event. All instances that are subscribed to it will receive it (and will potentially act on it).
	    /// </summary>
		/// <param name="newEvent">The event to trigger.</param>
	    /// <typeparam name="IGEvent">The 1st type parameter.</typeparam>
	    public static void TriggerEvent<IGEvent>( IGEvent newEvent ) where IGEvent : struct
	    {
	        List<IGEventListenerBase> list;
	        if( !_subscribersList.TryGetValue( typeof( IGEvent ), out list ) )
			#if EVENTROUTER_REQUIRELISTENER
			            throw new ArgumentException( string.Format( "Attempting to send event of type \"{0}\", but no listener for this type has been found. Make sure this.Subscribe<{0}>(EventRouter) has been called, or that all listeners to this event haven't been unsubscribed.", typeof( MMEvent ).ToString() ) );
			#else
			                return;
			#endif

	        foreach( IGEventListenerBase b in list )
	        {
	            ( b as IGEventListener<IGEvent> ).OnMMEvent( newEvent );
	        }
	    }

	    /// <summary>
	    /// Checks if there are subscribers for a certain type of events
	    /// </summary>
	    /// <returns><c>true</c>, if exists was subscriptioned, <c>false</c> otherwise.</returns>
	    /// <param name="type">Type.</param>
	    /// <param name="receiver">Receiver.</param>
	    private static bool SubscriptionExists( Type type, IGEventListenerBase receiver )
	    {
	        List<IGEventListenerBase> receivers;

	        if( !_subscribersList.TryGetValue( type, out receivers ) ) return false;

	        bool exists = false;

			foreach( IGEventListenerBase subscription in receivers )
	        {
	            if( subscription == receiver )
	            {
	                exists = true;
	                break;
	            }
	        }

	        return exists;
	    }
	}

	/// <summary>
	/// Static class that allows any class to start or stop listening to events
	/// </summary>
	public static class EventRegister
	{
	    public delegate void Delegate<T>( T eventType );

	    public static void MMEventStartListening<EventType>( this IGEventListener<EventType> caller ) where EventType : struct
	    {
			IGEventManager.AddListener<EventType>( caller );
	    }

		public static void MMEventStopListening<EventType>( this IGEventListener<EventType> caller ) where EventType : struct
	    {
			IGEventManager.RemoveListener<EventType>( caller );
	    }
	}

	/// <summary>
	/// Event listener basic interface
	/// </summary>
	public interface IGEventListenerBase { };

	/// <summary>
	/// A public interface you'll need to implement for each type of event you want to listen to.
	/// </summary>
	public interface IGEventListener<T> : IGEventListenerBase
	{
	    void OnMMEvent( T eventType );
	}
}