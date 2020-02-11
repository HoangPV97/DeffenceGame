using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.SimpleAndroidNotifications;
using System;

using NotificationServices = UnityEngine.iOS.NotificationServices;
using NotificationType = UnityEngine.iOS.NotificationType;

public class PushNotification : MonoBehaviour
{
    List<PushNotificationType> pushNotificationTypes;
    int countID = 0;
    void Start()
    {
#if UNITY_IOS
        NotificationServices.RegisterForNotifications(
            NotificationType.Alert |
            NotificationType.Badge |
            NotificationType.Sound);
#endif
    }
    public void SetUpNotification()
    {
        countID = 0;
#if UNITY_ANDROID
        NotificationManager.CancelAll();
#elif UNITY_IOS
        NotificationManager.CancelAllLocalNotifications();
#endif
        SetUpAndroidNotification();
    }

    void SetUpAndroidNotification()
    {
        pushNotificationTypes = new List<PushNotificationType>() { PushNotificationType. package, PushNotificationType.Energ, PushNotificationType.IceCream,
                                                                   PushNotificationType.DailyMorning, PushNotificationType.DailyNight, PushNotificationType.DailyLogin,};

        SetPush(PushNotificationType.DailyMorning, 24 + 8, 8, 8);
        SetPush(PushNotificationType.DailyLogin, 24 + 12, 12, 12);
        SetPush(PushNotificationType.DailyNight, 24 + 19, 19, 19);
        SetPush(PushNotificationType.Next12hour, 24 + 8 + 24, 8 + 24, 8);
        SetPush(PushNotificationType.Next24Hour, 24 + 12 + 48, 12 + 48, 12);
        SetPush(PushNotificationType.Next48hour, 24 + 8 + 72, 8 + 72, 8);
    }

    public void SetPush(PushNotificationType pDefault, int hour1, int hour2, int pushHour)
    {
        countID++;
        var dateTimeTmp = DateTime.Now;
        var dateTime2Tmp = dateTimeTmp.Date.AddHours((dateTimeTmp.Hour > pushHour) ? hour1 : hour2);
        var diff = dateTime2Tmp - dateTimeTmp;
        if (!AddPush(diff.TotalSeconds))
        {
            SetPushNotification((int)diff.TotalSeconds, countID, LocalNotificationType.Normal, pDefault);
            pushNotificationTypes.Remove(pDefault);
        }
    }

    bool AddPush(double time)
    {
        countID++;
        int deltaTime = 0;
        for (int i = 0; i < pushNotificationTypes.Count; i++)
        {
            switch (pushNotificationTypes[i])
            {
                case PushNotificationType.None:
                    break;
                case PushNotificationType.package:
                    float starterPackTimeStamp = PlayerPrefs.GetFloat(MainMenuController.STARTER_PACK_HASH, 0);
                    bool canDisplayStarterPack = DataController.Instance.GetLevelState(1, 7) > 0
                    && (DataController.ConvertToUnixTime(System.DateTime.UtcNow) - starterPackTimeStamp < 3 * 24 * 60 * 60)
                    && PlayerPrefs.GetInt("has_buy_starter_pack", 0) == 0;
                    if (canDisplayStarterPack)
                    {
                        deltaTime = (int)(3 * 24 * 60 * 60 - DataController.ConvertToUnixTime(System.DateTime.UtcNow) + starterPackTimeStamp);
                        if (Math.Abs(deltaTime - time) < 30 * 60)
                        {
                            SetPushNotification(deltaTime, countID, LocalNotificationType.Normal, pushNotificationTypes[i]);
                            pushNotificationTypes.Remove(pushNotificationTypes[i]);
                            return true;
                        }
                    }
                    else
                    {

                    }
                    break;
                case PushNotificationType.Energ:
                    if (DataController.Instance.Energy >= 3)
                    { }
                    else
                    {
                        int maxEn = 5 - DataController.Instance.Energy;
                        deltaTime = maxEn * 30 * 60;
                        if (Math.Abs(deltaTime - time) < 30 * 60)
                        {
                            SetPushNotification(deltaTime, countID, LocalNotificationType.Normal, pushNotificationTypes[i]);
                            pushNotificationTypes.Remove(pushNotificationTypes[i]);
                            return true;
                        }
                    }
                    break;
                case PushNotificationType.IceCream:
                    deltaTime = DataController.Instance.IceCreamDuration;
                    bool canDisplayIceCreamTruck = DataController.Instance.IsItemUnlocked((int)ItemType.IceCream);
                    if (Math.Abs(deltaTime - time) < 30 * 60 && canDisplayIceCreamTruck)
                    {
                        SetPushNotification(deltaTime, countID, LocalNotificationType.Normal, pushNotificationTypes[i]);
                        pushNotificationTypes.Remove(pushNotificationTypes[i]);
                        return true;
                    }
                    break;
            }
        }
        return false;
    }

    void SetPushNotification(int dateTime, int CountID, LocalNotificationType localNotificationType, PushNotificationType type)
    {
#if UNITY_ANDROID
        GetTitleAndDescreptionSimple(dateTime, CountID, localNotificationType, type);
#elif UNITY_IOS
        var notif = new LocalNotification();
        notif.fireDate = DateTime.Now.AddSeconds(dateTime);
		notif.alertBody =  Lean.Localization.LeanLocalization.GetTranslationText("PushNotificationType_" + type.ToString() + "_des");;
		NotificationServices.ScheduleLocalNotification(notif);
#endif
    }

    public void GetTitleAndDescreptionSimple(int secondPush, int id, LocalNotificationType localNotificationType, PushNotificationType pushNotificationType)
    {
        string title = Lean.Localization.LeanLocalization.GetTranslationText("PushNotificationType_" + pushNotificationType.ToString() + "_title");
        string descreption = Lean.Localization.LeanLocalization.GetTranslationText("PushNotificationType_" + pushNotificationType.ToString() + "_des");
        if (localNotificationType == LocalNotificationType.Simple)
            NotificationManager.Send(TimeSpan.FromSeconds(secondPush), title, descreption, new Color(1, 0.3f, 0.15f));
        else if (localNotificationType == LocalNotificationType.Normal)
            NotificationManager.SendWithAppIcon(TimeSpan.FromSeconds(secondPush), title, descreption, new Color(0, 0.6f, 1), NotificationIcon.Message);
        else if (localNotificationType == LocalNotificationType.Custom)
        {
            var notificationParams = new NotificationParams
            {
                Id = id,
                Delay = TimeSpan.FromSeconds(secondPush),
                Title = title,
                Message = descreption,
                Ticker = "Ticker",
                Sound = true,
                Vibrate = true,
                Light = true,
                SmallIcon = NotificationIcon.Message,
                SmallIconColor = new Color(0, 0.6f, 0),
                LargeIcon = "app_icon"
            };
            NotificationManager.SendCustom(notificationParams);
        }
    }
}
public enum PushNotificationType
{
    None,
    package,
    Energ,
    IceCream,
    DailyMorning,
    DailyNight,
    DailyLogin,
    Next12hour,
    Next24Hour,
    Next72hour,
    Next48hour,
}

public enum LocalNotificationType
{
    None,
    Simple,
    Normal,
    Custom,
}