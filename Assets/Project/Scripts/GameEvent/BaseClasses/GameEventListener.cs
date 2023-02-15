using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameEventListener<T> : MonoBehaviour
{
	public GameEvent<T> Event;
	public UnityEvent<T> Response;

	public virtual void OnEnable() => Event.RegisterListener(this);
	public virtual void OnDisable() => Event.UnregisterListener(this);
	
	public virtual void OnEventRaised(T t)
	{
		Response.Invoke(t);
	}
}
