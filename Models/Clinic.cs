namespace ClinicaVeterinaria.Models;

using System.Collections.Generic;
using System.Linq;

public class Clinic
{
    public List<Office> Offices { get; } = [];

    public Office AddOffice()
    {
        var office = new Office(Offices.Count + 1);
        Offices.Add(office);
        return office;
    }

    public Office? FindBestOffice(Appointment appointment)
    {
        foreach (var office in Offices)
        {
            if (office.CanFitAppointment(appointment))
                return office;
        }
        return null;
    }

    public bool ScheduleAppointment(Appointment appointment)
    {
        var office = FindBestOffice(appointment) ?? AddOffice();
        return office.AddAppointment(appointment);
    }

    public override string ToString() =>
        string.Join("\n", Offices.Select(o => o.ToString()));

    public int GetTotalOffices() => Offices.Count;
}
