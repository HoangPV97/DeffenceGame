
using System;

public interface INotification
{
   void Notify(string p_event_path, Object p_target, params object[] p_data);
}
