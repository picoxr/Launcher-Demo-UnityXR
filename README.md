# Description
PICO Enterprise devices offer the possibility of setting installed applications as their main launcher app. This is not only useful when setting up a kiosk mode, but also helps when we want the end user access only a set of different apps and options.

![image](https://github.com/picoxr/Launcher-Demo-UnityXR/assets/15983798/e0bbbeac-a7c6-48be-af88-47c94923b707)

# Demo requirements
- Unity 2022.24.f1
- PICO XR SDK 3.0.0
# Dependencies (UPM)
- XR Interaction Toolkit 2.5.2 + Starter Assets
- Android Logcat
# Example methods
The LauncherUtils.cs class shows how to use several Enterprise SDK methods:
- PXR_Enterprise.StartVrSettingsItem: https://developer-global.pico-interactive.com/reference/unity/latest/PXR_Enterprise/#StartVrSettingsItem
- PXR_Enterprise.StartActivity: https://developer-global.pico-interactive.com/reference/unity/latest/PXR_Enterprise/#StartActivity
- PXR_Enterprise.GotoSeeThroughFloorSetting: https://developer-global.pico-interactive.com/reference/unity/latest/PXR_Enterprise/#bd4f531c
- PXR_Enterprise.ControlSetDeviceAction: https://developer-global.pico-interactive.com/reference/unity/latest/PXR_Enterprise/#ControlSetDeviceAction
- PXR_Enterprise.SetAPPAsHome: https://developer-global.pico-interactive.com/reference/unity/latest/PXR_Enterprise/#SetAPPAsHome

Note: PXR_Enterprise.SetAPPAsHome requires rebooting the device in order to take effect.
# Demo
In this demo, several Enterprise-level APIs are exposed to the user/developer. We encourage developers to take a look at the PXR_Enterprise apps to check how they can leverage the Enteprise SDK for their use cases.
# Setting up kiosk mode/custom launcher without SDK
## Replace launcher under home screen settings 
To provide more intuitive way of configuring Kiosk Mode, we’ve built a functionality from Pico Business Setting to facilitate Kiosk Mode deployment. The method is described as following: 
1. Click Settings > Developer> Business settings>Home Screen. 
2. Select the application that you want to use for Kiosk Mode. 
3. Reboot the device
