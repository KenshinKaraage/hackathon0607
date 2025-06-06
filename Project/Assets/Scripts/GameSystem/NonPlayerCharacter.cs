[System.Serializable]
public class NonPlayerCharacter : IPlayerCharacter
{
    private int _uniqueID;
    private string _nickname;
    private Role _job;
    //コンストラクタ
    public NonPlayerCharacter(int id, string name)
    {
        this._uniqueID = id;
        this._nickname = name;
    }

    public int UniqueID { get => _uniqueID; }
    public string Nickname { get => _nickname; }
    public Role Job { get; set; }         // 役職
    public bool IsAlive { get; set; }

    public bool IsNPC => true;
    public string Answer { get; set; }
    public bool IsAnswered { get; set; }

    public int ID => UniqueID;

    public string Displayname => _nickname;
}
