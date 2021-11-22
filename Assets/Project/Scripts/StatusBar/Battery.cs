using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battery  {

	private BatteryState batteryState = BatteryState.BATTERY_STATUS_UNKNOWN;

	public BatteryState BatteryState {
		get {
			return batteryState;
		}
		set {
			batteryState = value;
		}
	}

	private string level;
	public string Level {
		get {
			return level;
		}
		set {
			level = value;
		}
	}
}


public enum BatteryState
{
	BATTERY_STATUS_UNKNOWN = 1,
	BATTERY_STATUS_CHARGING = 2,
	BATTERY_STATUS_DISCHARGING = 3,
	BATTERY_STATUS_NOT_CHARGING = 4,
	BATTERY_STATUS_FULL = 5
}
