namespace ClinicaVeterinaria.Utils;

using System.Collections.Generic;
using System.IO;
using System.Linq;
using ClinicaVeterinaria.Models;

public static class FileParser
{
    public static List<Appointment> ParseAppointmentsFile(string filePath)
    {
        var appointments = new List<Appointment>();
        var lines = File.ReadAllLines(filePath);

        foreach (var line in lines)
        {
            var trimmed = line.Trim();
            if (string.IsNullOrEmpty(trimmed))
                continue;

            var parts = trimmed.Split(' ');
            var durationStr = parts[^1];
            var name = string.Join(" ", parts[..^1]);

            appointments.Add(new Appointment(name, durationStr));
        }

        return appointments;
    }
}
