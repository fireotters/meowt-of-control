﻿using System;
using Discord;
using UnityEngine;

public class DiscordManager : MonoBehaviour
{
    public static DiscordManager Instance { get; private set; }
    private Discord.Discord discord;
    private ActivityManager activityManager;
    private static readonly byte[] ObfsId = {}; //TODO fill
    private const string Key = ""; //TODO fill
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            var sec = new SimpleEncrypt(Key);
            discord = new Discord.Discord(Convert.ToInt64(sec.Decrypt(ObfsId)), (ulong) CreateFlags.NoRequireDiscord);
            activityManager = discord.GetActivityManager();
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    private void Update()
    {
        discord.RunCallbacks();
    }

    /// <summary>
    /// Updates current activity in Rich Presence
    /// </summary>
    /// <param name="activity">Activity definition</param>
    /// <returns>Status flag</returns>
    public bool UpdateDiscordRp(Activity activity)
    {
        var operationResult = false;

        if (activityManager != null)
        {
            activityManager.UpdateActivity(activity, result =>
            {
                if (result == Result.Ok)
                    operationResult = true;
                else 
                    Debug.LogError("Uh oh something failed while updating rich presence.\nResult code: " + result);
            });
        }
        else
        {
            Debug.LogError("activityManager is null");
        }
        return operationResult;
    }

    private void OnApplicationQuit()
    {
        activityManager.ClearActivity(result =>
        {
            Debug.Log(result == Result.Ok
                ? "Presence cleared successfully!"
                : "uh oh how did you debug this thing again? Result: " + result);
        });
        
        discord.Dispose();
    }
}