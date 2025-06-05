using UnityEngine;

namespace Test
{
    using Photon.Pun;
    using Photon.Realtime;
    using UnityEngine;

    public interface Test_IPlayerCharacter
    {
        int ID { get; }         // プレイヤーまたはNPCの一意なID (ActorNumberやNPC ID)
        string Displayname { get; }      // 表示名
        Role Job { get; set; }         // 役職
        bool IsAlive { get; set; }       // 生存状態
        bool IsNPC { get; }           // NPCであるかどうかのフラグ
        string Answer { get; set; }
        bool IsAnswered { get; set; }
    }

    public class Test_HumanPlayerCharacter : Test_IPlayerCharacter
    {
        private Player photonPlayer; //PUN2のPlayer型のフィールドを宣言

        //コンストラクタ（インスタンス生成の際に呼び出される）
        public Test_HumanPlayerCharacter(Player player)
        {
            this.photonPlayer = player;
        }

        //IPlayerCharacterで宣言したプロパティを実装
        public int ID => photonPlayer.ActorNumber;
        public string Displayname => photonPlayer.NickName;

        //----ここからカスタムプロパティに役職と生存の情報を格納している前提-------

        //プレイヤーのカスタムプロパティからJobを取得する
        public Role Job
        {
            get => photonPlayer.CustomProperties.TryGetValue("Job", out object job) ? (Role)job : Role.None;
            set => UpdateCustomProperty("Job", value);
        }
        //IsAliveを取得する
        public bool IsAlive
        {
            get => photonPlayer.CustomProperties.TryGetValue("IsAlive", out object alive) ? (bool)alive : true;
            set => UpdateCustomProperty("IsAlive", value);
        }
        public bool IsNPC => false; //プレイヤーなので

        //IsAnsweredを取得する
        public string Answer
        {
            get => photonPlayer.CustomProperties.TryGetValue("Answer", out object answer) ? (string)answer : "";
            set => UpdateCustomProperty("Answer", value);
        }

        //IsAnsweredを取得する
        public bool IsAnswered
        {
            get => photonPlayer.CustomProperties.TryGetValue("IsAnswered", out object value) ? (bool)value : true;
            set => UpdateCustomProperty("IsAnswered", value);
        }

        // Photon Playerオブジェクト（ラップしていない生データ）へ直接アクセスが必要な場合
        public Player GetPhotonPlayer() => photonPlayer;

        //カスタムプロパティを変更する場合 HumanplayerCharacter型.UpdateCustomProperty("Job", 村人) みたいにする
        private void UpdateCustomProperty(string key, object value)
        {
            if (photonPlayer.IsLocal || PhotonNetwork.IsMasterClient) // 基本的にカスタムプロパティの変更はローカルプレイヤーが行う
            {
                ExitGames.Client.Photon.Hashtable props = new ExitGames.Client.Photon.Hashtable();
                props[key] = value;
                photonPlayer.SetCustomProperties(props);
            }
            else
            {
                // 他のプレイヤーのプロパティを直接変更すべきではない。
                // 変更が必要な場合はマスタークライアント経由でRPC等を使用する。
                UnityEngine.Debug.LogWarning($"Cannot directly modify custom properties of non-local player: {photonPlayer.NickName}");
            }
        }

    }

    [System.Serializable]
    public class Test_NonPlayerCharacter : Test_IPlayerCharacter
    {
        private int _uniqueID;
        private string _nickname;
        private Role _job;
        //コンストラクタ
        public Test_NonPlayerCharacter(int id, string name)
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
}
