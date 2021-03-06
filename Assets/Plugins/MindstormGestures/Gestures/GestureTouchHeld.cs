/*
Unity3d-TUIO connects touch tracking from a TUIO to objects in Unity3d.

Copyright 2011 - Mindstorm Limited (reg. 05071596)

Author - Simon Lerpiniere

This file is part of Unity3d-TUIO.

Unity3d-TUIO is free software: you can redistribute it and/or modify
it under the terms of the GNU Lesser Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

Unity3d-TUIO is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU Lesser Public License for more details.

You should have received a copy of the GNU Lesser Public License
along with Unity3d-TUIO.  If not, see <http://www.gnu.org/licenses/>.

If you have any questions regarding this library, or would like to purchase 
a commercial licence, please contact Mindstorm via www.mindstorm.com.
*/

using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

using Mindstorm.Gesture;

/// <summary>
/// Detects when a touch has been held down on this object for a certain amount of time.
/// Very useful for push buttons where they must be pushed all the way down.
/// </summary>
[RequireComponent(typeof(CountdownTimer))]
public class GestureTouchHeld : GestureTouch
{
	public bool RelaxTime = true;
	
	/// <summary>
	/// How long the touch must be held for before the HeldMessage is sent.
	/// </summary>
	public float HoldTime = 1.0f;
	
	/// <summary>
	/// Message to send once the touch has been held for the specified time.
	/// </summary>
	public string HeldMessage;
	
	/// <summary>
	/// Message to be sent when a touch is added.
	/// </summary>
	public string TouchStartMessege;
	
	/// <summary>
	/// Message to be sent when touch is removed.
	/// </summary>
	public string CancelMessage;
	
	CountdownTimer heldTimer = null;
	
	public override void Start()
	{
		base.Start();
		
		heldTimer = GetComponent<CountdownTimer>();
	}
	
	public override void AddTouch(Touch t, RaycastHit hit, Camera hitOn)
	{
		base.AddTouch(t, hit, hitOn);
		
		if(curTouch.fingerId == t.fingerId)
		{
			if (TouchStartMessege != string.Empty) BroadcastTouchMessage(TouchStartMessege, hit);
			
			heldTimer.StartCountdown(HoldTime);
		}
	}
	
	public override void RemoveTouch(Touch t)
	{
		base.RemoveTouch(t);
		
		if(curTouch.fingerId == t.fingerId)
		{
			CancelHeld();
		}
	}
	
	public override void UpdateTouch(Touch t)
	{
		base.UpdateTouch(t);
		
		if(curTouch.fingerId != t.fingerId || !touchSet) return;
		
		if(heldTimer.RemainingTime > 0.0f) return;
		
		RaycastHit h = new RaycastHit();
		if(!HitsOrigCollider(t, out h)) return;
		
		BroadcastTouchMessage(HeldMessage, h);
		EndHeld();
	}
	
	void CancelHeld()
	{
		ClearCurTouch();
		
		heldTimer.ResetCountdown(RelaxTime?
								CountdownTimer.CountdownStateEnum.Relaxing:
								CountdownTimer.CountdownStateEnum.Paused);
		
		if (CancelMessage != string.Empty) BroadcastTouchMessage(CancelMessage, new RaycastHit());
	}
	
	void EndHeld()
	{
		heldTimer.ResetCountdown(CountdownTimer.CountdownStateEnum.Finished);
	}
}