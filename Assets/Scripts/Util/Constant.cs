using System;

public class Constant
{
    public const string PP_FIRST_TIEM = "PP_FIRST_TIEM";
    public const string PP_BGM_VALUE = "PP_BGM_VALUE";
    public const string PP_SFX_VALUE = "PP_SFX_VALUE";
    public const string PP_BGM_MUTE = "PP_BGM_MUTE";
    public const string PP_SFX_MUTE = "PP_SFX_MUTE";
    public const string PP_DEFAULT_REMOTE_CONFIG_room_default_value = "PP_DEFAULT_REMOTE_CONFIG_room_default_value";

    public const string PP_TUTORIAL_TEXT = "PP_TUTORIAL_TEXT";
    public const string PP_STORY = "PP_STORY";
    public const string PP_BUY_NUMBER = "PP_BUY_NUMBER";
    public const string PP_DEFAULT_REMOTE_CONFIG_remote_config_data = "PP_DEFAULT_REMOTE_CONFIG_remote_config_data";
    public const string PP_DEFAULT_REMOTE_CONFIG_remote_config_data_CurrentDefaultTower = "PP_DEFAULT_REMOTE_CONFIG_remote_config_data_CurrentDefaultTower";

    public static string[] TextSelect = { "+1", "+10", "+100", "Max", "Max" };
}

[Serializable]
public class KeyValue
{
    public string Key;
    public string Value;

    public int KeyInt()
    {
        return int.Parse(Key);
    }
    public int ValueInt()
    {
        if (Value != "")
            return int.Parse(Value);
        return 0;
    }
    public float KeyFloat()
    {
        return float.Parse(Key);
    }
    public float ValueFloat()
    {
        if (Value != "")
            return float.Parse(Value);
        return 0;
    }
    public double ValueDouble()
    {
        if (Value != "")
            return double.Parse(Value);
        return 0;
    }

    public KeyValue(string key, string value)
    {
        this.Key = key;
        this.Value = value;
    }

    public KeyValue()
    {
    }

    public override string ToString()
    {
        return "Key: " + Key + "___Value: " + Value;
    }
}