[System.Serializable]
public class NonPlayerCharacter : IPlayerCharacter
{
    private int _ID;
    private string _displayname;
    private JobNames _job;
    private bool _isAlive;

    //コンストラクタ
    public NonPlayerCharacter(int id, string name, JobNames jobTitle, bool isAlive = true)
    {
        this._ID = id;
        this._displayname = name;
        this._job = jobTitle;
        this._isAlive = isAlive;
    }

    public int ID { get => _ID; }
    public string Displayname { get => _displayname; }
    public JobNames Job { get => _job; set => _job = value; }
    public bool IsAlive { get => _isAlive; set => _isAlive = value; }
    public bool IsNPC => true;
}
