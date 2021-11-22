public class BaseModel
{

	public int mid = -1;
	public DataType dataType = DataType.UNKNOWN;
	public string title = "pico";
	public string imageUrl = "";
	public string url = "";
	public int videoType = -1;
	public string packageName = "";
	public int pid = -1;
	public string content = "";
	public string date = "";
	public string button = "";
	//ClassName
	public string className = "";
	public string ToString ()
	{
		return this.mid + "," + this.dataType + "," + this.title + "," +
		this.imageUrl + "," + this.url + "," + this.videoType + "," +
		this.packageName + this.className;
	}

	public bool Equals (BaseModel otherModel)
	{
		return this.ToString ().Equals (otherModel.ToString ());
	}

}