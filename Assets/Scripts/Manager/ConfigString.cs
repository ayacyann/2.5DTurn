
public static class ConfigString//无需实例化即可获取类中的信息
{
    #region   场景名称
    public const string OVERWORLD_SCENE = "OverworldScene";
    public const string MENU_SCENE = "MenuScene";
    public const string BATTLE_SCENE = "BattleScene";
    #endregion

    #region 默认玩家名称
    public const string DEFAULT_PLAYER_NAME = "Player";
    #endregion

    #region 保存内容的相关字符串
    public const string BACKPACK_ITEM = "Backpack";
    public const string PLAYER_DATA = "PlayerData";
    #endregion
    
    
    public const string PARTY_JOINED_MESSAGE = " joined The Party!";
    public const string NPC_JOINABLE_TAG = "NPC Joinable";

    public const string IS_WALK_PARAM="IsWalk";//引用isWalk参数
    public const float TIME_PER_STEP = 0.5f;//在草丛中行走时需要花的时间
    public const string AI_NAME = "NPC";
}
