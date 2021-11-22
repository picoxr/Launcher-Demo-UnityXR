using UnityEngine;
using System.Collections;

public class Constant
{
	public const bool IS_DEBUG = true;
	public const string SP_IF_CAN_UPDATE = "ifCanUpdate";
	public const int CAN_UPDATE = 1;
	public const int CANNOT_UPDATE = 0;

	public const string SYSTEM_UPDATE_PACKAGE_NAME = "com.picovr.updatesystem";
	public const string SYSTEM_UPDATE_ACTIVITY_NAME = "com.picovr.updatesystem.MainActivity";
	public const string USERCENTER_PACKAGE_NAME = "com.picovr.vrusercenter";
	public const string USERCENTER_ACTIVITY_NAME = "com.picovr.vrusercenter.MainActivity";
	public const string SETTINGS_PACKAGE_NAME = "com.picovr.settings";
	public const string SETTINGS_ACTIVITY_NAME = "com.picovr.vrsettingslib.UnityActivity";
	public const string SETTINGS_WIFI_ACTION = "pui.settings.action.WIFI_SETTINGS";
	public const string SETTINGS_BLUETOOTH_ACTION = "pui.settings.action.BLUETOOTH_SETTINGS";
	public const string SETTINGS_HANDLE_ACTION = "pui.settings.action.CONTROLLER_SETTINGS";
	public const string STORE_PACKAGE_NAME = "com.picovr.store";
	public const string STORE_ACTIVITY_NAME = "com.picovr.store.UnityActivity";
	public const string VIVEPORT_PACKAGE_NAME = "com.htc.viveport.store";
	public const string FILE_MANAGER_PACKAGE_NAME = "com.picovr.filemanager";
	public const string FILE_MANAGER2_PACKAGE_NAME = "com.pvr.filemanager";
	public const string X5_PACKAGE_NAME = "com.pvr.innerweb";
	
	public static string BASE_URL = "http://pui.picovr.com/api/";
	public static string API_KEY = "6u2qBI2DF26a6tA6FbCm3vlA8azxAUl6";
	
	public const string API_HOME = "pui3/home";
	public const string API_NOTICE = "announcement";
	public const string API_APP = "pui3/home/quick";
	
	public const int MSG_NORMAL = 0;
	public const int MSG_NONETWORK = -102;
	public const int MSG_ABORTED = -117;
	public const int MSG_CONNECT_TIMEOUT = -118;
	public const int MSG_TIMEOUT = -119;
	public const int MSG_ERROR = -10000;
	
	public const string DB_HOME_TABLE = "T_Config";
	
	public const string PREFS_KEY = "history_records";
	
	public const string FROM_SCENE_APP = "scene_app";
	
	public const string SP_SCENE_INDEX = "scene_index";
	
	public const string SP_TMALL_PROTOCOL_KEY = "tmall_protocol";
	public const int SP_TMALL_PROTOCOL_DEF_VALUE = 0;
	public const int SP_TMALL_PROTOCOL_VALUE = 1;
	
	public const string SP_DB_VERSION = "dbVersion";
	public const int SP_DB_VERSION_DEF_VALUE = 1;
	
	public const string SP_SUN_TIP = "sunTip";
	public const int SP_SUN_TIP_DEF_VALUE = 0;
	public const int SP_SUN_TIP_VALUE = 1;
	
	public const int HAS_SYSTEM_UPDATE = 1;
	
	public const int HAS_APP_UPDATE = 1;
}


public enum DataType
{
	UNKNOWN = 0,
	
	VIDEO = 1,
	
	GAME = 2,

	URL = 3,
	
	GALLERY_CATEGORY = 4,
	
	GALLERY_IMAGE = 5,

	EPISODE = 6,
	
	GALLERY_THEME = 7,
	
	WING_CATEGORY = 8,
	
	WING_THEME = 9,
	
	APP = 10,

	RECENT = 11,

	SYSTEM = 12,
	
	STORE_CATEGORY = 13,
}


public enum VideoType
{
	
	VIDOE_2D = 0,
	
	VIDOE_3D_LR = 1,
	
	VIDOE_360_2D = 2,
	
	VIDOE_360_3D_TB = 3,
	
	VIDOE_360_3D_BT = 4,
	
	VIDOE_360_3D_LR = 5,
	
	VIDOE_360_3D_RL = 6,
	
	VIDOE_3D_TB = 7,
	
	VIDOE_3D_BT = 8,
	
	VIDOE_3D_RL = 9,
	
	VIDOE_180_2D = 10,
	
	VIDOE_180_3D_TB = 11,
	
	VIDOE_180_3D_BT = 12,
	
	VIDOE_180_3D_LR = 13,
	
	VIDOE_180_3D_RL = 14,
}


public enum ScreenId{
	HOME = 1,
	APP = 2,
	SCENE = 3,
}


public enum ScreenState{
	UNKNOWN,
	ACTIVE,
	HIDDEN
}

public enum AppListType{
	APP = 0,
	RECENT = 1 
}
