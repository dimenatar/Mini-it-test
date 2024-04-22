using System.Collections.Generic;

[System.Serializable]
public class UserData
{
    public UserData()
    {
        CurrentLevelOrder = 1;
        CurrentSceneName = "Level 1";
        IsCompletedAllLevels = false;
        Keys = new List<string>();
    }

    public int CurrentLevelOrder;
    public string CurrentSceneName;
    public bool IsCompletedAllLevels;
    public List<string> Keys;

    public override string ToString()
    {
        return $"CurrentLevelOrder {CurrentLevelOrder} Scene {CurrentSceneName} Completed {IsCompletedAllLevels}";
    }
}
