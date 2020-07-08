using System;
using Discord;

public static class DiscordActivities
{
    public static readonly Activity MainMenuActivity = new Activity
    {
        State = "In the Main Menu",
        Assets =
        {
            LargeImage = "blank"
        },
        Instance = false
    };

    public static Activity StartGameActivity()
    {
        return new Activity {
            State = "In game",
            Timestamps =
            {
                Start = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()
            },
            Assets =
            {
                LargeImage = "blank",
                LargeText = "Blank."
            },
            Instance = false
        };
    }
}