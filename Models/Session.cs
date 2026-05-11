namespace ClinicaVeterinaria.Models;

using System.Collections.Generic;

public enum SessionType { Morning, Afternoon }

public class Session
{
    public SessionType Type { get; }
    public int StartHour { get; }
    public int StartMinute { get; }
    public int EndHour { get; }
    public int EndMinute { get; }
    public int MaxDuration { get; }
    public List<(Appointment appointment, string startTime)> Appointments { get; } = [];
    public int UsedTime { get; private set; } = 0;

    public Session(SessionType type)
    {
        Type = type;

        if (type == SessionType.Morning)
        {
            StartHour = 8;
            StartMinute = 0;
            EndHour = 11;
            EndMinute = 30;
        }
        else
        {
            StartHour = 13;
            StartMinute = 30;
            EndHour = 17;
            EndMinute = 0;
        }

        MaxDuration = CalculateMaxDuration();
    }

    private int CalculateMaxDuration()
    {
        var startMinutes = StartHour * 60 + StartMinute;
        var endMinutes = EndHour * 60 + EndMinute;
        return endMinutes - startMinutes;
    }

    public bool CanFit(int duration) => UsedTime + duration <= MaxDuration;

    public bool AddAppointment(Appointment appointment)
    {
        if (!CanFit(appointment.Duration))
            return false;

        var startTimeStr = GetTimeString(UsedTime);
        Appointments.Add((appointment, startTimeStr));
        UsedTime += appointment.Duration;
        return true;
    }

    private string GetTimeString(int minutes)
    {
        var totalMinutes = StartHour * 60 + StartMinute + minutes;
        var hours = totalMinutes / 60;
        var mins = totalMinutes % 60;
        return $"{hours:D2}:{mins:D2}";
    }

    public string GetEndTime() => GetTimeString(UsedTime);

    public int GetRemainingTime() => MaxDuration - UsedTime;
}
