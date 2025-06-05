using UnityEngine;

public interface IPlayerCharacter
{
    int ID { get; }              // プレイヤーまたはNPCの一意なID (ActorNumberやNPC ID)
    string Displayname { get; }     // 表示名
    JobNames Job { get; set; }       // 職業
    bool IsAlive { get; set; }       // 生存状態
    bool IsNPC { get; }             // NPCであるかどうかのフラグ
    // その他、ゲームに必要な共通のプロパティやメソッド
    // void PerformActionX();
}