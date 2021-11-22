using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bluetooth {

	
	private BluetoothState bluetoothState = BluetoothState.STATE_OFF;
	public BluetoothState BluetoothState {
		get {
			return bluetoothState;
		}
		set {
			bluetoothState = value;
		}
	}

	
	private BluetoothProfile bluetoothProfile = BluetoothProfile.STATE_DISCONNECTED;
	public BluetoothProfile BluetoothProfile {
		get {
			return bluetoothProfile;
		}
		set {
			bluetoothProfile = value;
		}
	}


}


public enum BluetoothState{
	STATE_OFF = 10,
	STATE_TURNING_ON = 11,
	STATE_ON = 12,
	STATE_TURNING_OFF = 13
}


public enum BluetoothProfile{
	STATE_DISCONNECTED = 0,
	STATE_CONNECTING = 1,
	STATE_CONNECTED = 2,
	STATE_DISCONNECTING = 3
}
