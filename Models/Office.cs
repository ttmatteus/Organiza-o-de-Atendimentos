namespace ClinicaVeterinaria.Models;

public class Office
{
    public int Id { get; }
    public Session MorningSession { get; }
    public Session AfternoonSession { get; }

    public Office(int id)
    {
        Id = id;
        MorningSession = new Session(SessionType.Morning);
        AfternoonSession = new Session(SessionType.Afternoon);
    }

    public bool CanFitAppointment(Appointment appointment) =>
        MorningSession.CanFit(appointment.Duration) ||
        AfternoonSession.CanFit(appointment.Duration);

    public bool AddAppointment(Appointment appointment)
    {
        if (MorningSession.CanFit(appointment.Duration))
            return MorningSession.AddAppointment(appointment);

        if (AfternoonSession.CanFit(appointment.Duration))
            return AfternoonSession.AddAppointment(appointment);

        return false;
    }

    public override string ToString()
    {
        var sb = new System.Text.StringBuilder();
        sb.AppendLine($"Consultório {Id}:");

        sb.AppendLine("Turno da manhã (08:00 - 11:30):");
        if (MorningSession.Appointments.Count == 0)
            sb.AppendLine("  (Vazio)");
        else
            foreach (var (appointment, startTime) in MorningSession.Appointments)
            {
                var durationDisplay = appointment.IsExpressed ? "expresso" : $"{appointment.Duration}min";
                sb.AppendLine($"  {startTime} {appointment.Name} ({durationDisplay})");
            }
        sb.AppendLine($"  {MorningSession.GetEndTime()} Higienização");

        sb.AppendLine("\nTurno da tarde (13:30 - 17:00):");
        if (AfternoonSession.Appointments.Count == 0)
            sb.AppendLine("  (Vazio)");
        else
            foreach (var (appointment, startTime) in AfternoonSession.Appointments)
            {
                var durationDisplay = appointment.IsExpressed ? "expresso" : $"{appointment.Duration}min";
                sb.AppendLine($"  {startTime} {appointment.Name} ({durationDisplay})");
            }
        sb.AppendLine($"  {AfternoonSession.GetEndTime()} Reunião de encerramento\n");

        return sb.ToString();
    }
}
