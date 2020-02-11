using InviGiant.Tools;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Notifications.Android;
using Firebase.Extensions;
public enum NotificationType
{
    None,
    Package_Outdate, Package_active,
    Event_Start, Event_End,
    Energy,
    Daily_Morning, Daily_Night, Daily_Login,
    Next24h, Next48h, Next72h, next96h
}
public class NotificationController : Singleton<NotificationController>
{
    AndroidNotification androidNotification;
    int identifier;
    AndroidNotificationChannel notificationChannel;
    Firebase.DependencyStatus dependencyStatus = Firebase.DependencyStatus.UnavailableOther;
    // Start is called before the first frame update
    void Start()
    {
        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            dependencyStatus = task.Result;
            if (dependencyStatus == Firebase.DependencyStatus.Available)
            {
                InitializeFirebase();
            }
            else
            {
                Debug.LogError(
                  "Could not resolve all Firebase dependencies: " + dependencyStatus);
            }
        });
        //Firebase.Messaging.FirebaseMessaging.TokenRegistrationOnInitEnabled = true;
        //Firebase.Messaging.FirebaseMessaging.TokenReceived += OnTokenReceived;
        //Firebase.Messaging.FirebaseMessaging.MessageReceived += OnMessageReceived;
        AndroidNotificationCenter.OnNotificationReceived += receivedNotificationHandler;
        RegisterChannel();
        //TimeSpan time = new TimeSpan(20, 00, 00);
        //ShowNotification("Test", "Test Notification!!", DateTime.Today+time);

    }

    void InitializeFirebase()
    {
        Firebase.Messaging.FirebaseMessaging.MessageReceived += OnMessageReceived;
        Firebase.Messaging.FirebaseMessaging.TokenReceived += OnTokenReceived;
    }
    private void OnTokenReceived(object sender, Firebase.Messaging.TokenReceivedEventArgs token)
    {
        Debug.Log("Received Registration Token : " + token.Token);
    }
    private void OnMessageReceived(object sender, Firebase.Messaging.MessageReceivedEventArgs e)
    {
        Debug.Log("Received a new message from : " + e.Message.From);
    }
    void RegisterChannel()
    {
        notificationChannel = new AndroidNotificationChannel()
        {
            Id = "channel_id",
            Name = "Default Channel",
            Importance = Importance.High,
            Description = "Generic notifications",
        };
        AndroidNotificationCenter.RegisterNotificationChannel(notificationChannel);
    }
    public void ShowNotification(string _title, string _message, DateTime _time)
    {
        androidNotification = new AndroidNotification();
        androidNotification.Title = _title;
        androidNotification.Text = _message;
        androidNotification.SmallIcon = "small_icon";
        androidNotification.LargeIcon = "icon_large";
        androidNotification.FireTime = _time;
        identifier = AndroidNotificationCenter.SendNotification(androidNotification, "channel_id");
    }

    public void CancelDisplayedNotification()
    {
        AndroidNotificationCenter.CancelAllDisplayedNotifications();
    }

    private void receivedNotificationHandler(AndroidNotificationIntentData data)
    {
        var msg = "Notification received : " + data.Id + "\n";
        msg += "\n Notification received: ";
        msg += "\n .Title: " + data.Notification.Title;
        msg += "\n .Body: " + data.Notification.Text;
        msg += "\n .Channel: " + data.Channel;
        Debug.Log(msg);
    }

    public void OnApplicationPause(bool pause)
    {

    }
}
