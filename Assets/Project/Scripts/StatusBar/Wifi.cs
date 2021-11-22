using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wifi {

	
	private NetworkState networkState = NetworkState.STATE_DISCONNECTED;
	public NetworkState NetworkState {
		get {
			return networkState;
		}
		set {
			networkState = value;
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

	
	private WifiState wifiState = WifiState.WIFI_STATE_UNKNOWN;
	public WifiState WifiState {
		get {
			return wifiState;
		}
		set {
			wifiState = value;
		}
	}

}


public enum NetworkState
{
	STATE_DISCONNECTED = 0,
	STATE_CONNECTED = 1,
}


public enum WifiState
{
	WIFI_STATE_UNKNOWN = 4,
	WIFI_STATE_ENABLED = 3,
	WIFI_STATE_ENABLING = 2,
	WIFI_STATE_DISABLED = 1,
	WIFI_STATE_DISABLING = 0,
}
