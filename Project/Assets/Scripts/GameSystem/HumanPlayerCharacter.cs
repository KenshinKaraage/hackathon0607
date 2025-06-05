using Photon.Realtime; // Player型を使用するために必要

public class HumanPlayerCharacter : IPlayerCharacter
{
    private Player photonPlayer; //PUN2のPlayer型のフィールドを宣言

    //コンストラクタ（インスタンス生成時に呼び出される）
    public HumanPlayerCharacter(Player player)
    {
        this.photonPlayer = player;
    }

    //IPlayerCharacterで定義されたプロパティを実装
    public int ID => photonPlayer.ActorNumber;
    public string Displayname => photonPlayer.NickName;

    //----ここから下はカスタムプロパティで定義・同期する情報（ルームにいる間）-------

    //プレイヤーのカスタムプロパティからJobを取得する
    public JobNames Job
    {
        get => photonPlayer.CustomProperties.TryGetValue("Job", out object job) ? (JobNames)job : JobNames.UNKNOWN;
        set => UpdateCustomProperty("Job", value);
    }

    //IsAliveを取得する
    public bool IsAlive
    {
        get => photonPlayer.CustomProperties.TryGetValue("IsAlive", out object alive) ? (bool)alive : true;
        set => UpdateCustomProperty("IsAlive", value);
    }

    public bool IsNPC => false; //プレイヤーなので

    // Photon Playerオブジェクト（ラップしてない生データ）へのアクセスが必要な場合
    public Player GetPhotonPlayer() => photonPlayer;

    //カスタムプロパティを変更する場合 HumanPlayerCharacter型.UpdateCustomProperty("Job", 新しい値) のようにする
    private void UpdateCustomProperty(string key, object value)
    {
        if (photonPlayer.IsLocal) // 基本的にカスタムプロパティの変更はローカルプレイヤーが行う
        {
            ExitGames.Client.Photon.Hashtable props = new ExitGames.Client.Photon.Hashtable();
            props[key] = value;
            photonPlayer.SetCustomProperties(props);
        }
        else
        {
            // 他のプレイヤーのプロパティを直接変更すべきではない。
            // 変更が必要な場合はマスタークライアント経由のRPCなどを使用する。
            UnityEngine.Debug.LogWarning($"Cannot directly modify custom properties of non-local player: {photonPlayer.NickName}");
        }
    }
}