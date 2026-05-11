namespace ClinicaVeterinaria.Models;

public class Appointment
{
    public string Name { get; }
    public int Duration { get; }
    public bool IsExpressed { get; }

    public Appointment(string name, string durationStr)
    {
        Name = name;
        IsExpressed = durationStr == "expresso";
        Duration = IsExpressed ? 10 : int.Parse(durationStr.Replace("min", ""));
    }

    public override string ToString()
    {
        var durationDisplay = IsExpressed ? "expresso" : $"{Duration}min";
        return $"{Name} {durationDisplay}";
    }
}
