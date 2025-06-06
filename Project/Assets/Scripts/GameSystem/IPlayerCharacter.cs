using UnityEngine;

public interface IPlayerCharacter
{
    int ID { get; }         // プレイヤーまたはNPCの一意なID (ActorNumberやNPC ID)
    string Displayname { get; }      // 表示名
    Role Job { get; set; }         // 役職
    bool IsAlive { get; set; }       // 生存状態
    bool IsNPC { get; }           // NPCであるかどうかのフラグ
    string Answer { get; set; }
    bool IsAnswered { get; set; }
}