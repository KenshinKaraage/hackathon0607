public enum GameState
{
    READY,
    ROLE_DISTRIBUTION,
    QUESTION,
    ANSWER,
    VOTE,
    RESULT,
}

public enum Role
{
    None = 0,       // 未割り当て
    Representative = 1, // 代表者 (人間)
    Werewolf = 2,       // 人狼 (人間)
    VillagerAI = 3      // 村人 (AI) - AIプレイヤーの場合
}