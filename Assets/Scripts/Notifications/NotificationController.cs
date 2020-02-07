using InviGiant.Tools;
using System;
using System.Collections;
using System.Collections.Generic;
using Firebase.Messaging;
using UnityEngine;
public class NotificationController : Singleton<NotificationController>
{
    // Start is called before the first frame update
    void Start()
    {
        FirebaseMessaging.TokenReceived += OnTokenReceived;
        FirebaseMessaging.MessageReceived += OnMessageReceived;
    }
    private void OnTokenReceived(object sender, TokenReceivedEventArgs token)
    {
        Debug.Log("Received Registration Token : " + token.Token);
    }
    private void OnMessageReceived(object sender, MessageReceivedEventArgs e)
    {
        Debug.Log("Received a new message from : " + e.Message.From);
    }
}
