namespace ClinicaVeterinaria.Scheduler;

using System.Collections.Generic;
using System.Linq;
using ClinicaVeterinaria.Models;

public static class SchedulingAlgorithm
{
    public static Clinic Schedule(List<Appointment> appointments)
    {
        var clinic = new Clinic();

        var sortedAppointments = appointments
            .OrderByDescending(a => a.Duration)
            .ToList();

        foreach (var appointment in sortedAppointments)
        {
            clinic.ScheduleAppointment(appointment);
        }

        return clinic;
    }
}
