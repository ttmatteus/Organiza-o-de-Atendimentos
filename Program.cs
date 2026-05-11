using System;
using System.Collections.Generic;
using ClinicaVeterinaria.Models;
using ClinicaVeterinaria.Scheduler;
using ClinicaVeterinaria.Utils;

var filePath = args.Length > 0 ? args[0] : "atendimentos.txt";

try
{
    Console.WriteLine($"Lendo atendimentos de: {filePath}\n");
    var appointments = FileParser.ParseAppointmentsFile(filePath);

    Console.WriteLine($"Total de atendimentos a agendar: {appointments.Count}\n");
    Console.WriteLine("Atendimentos:");
    foreach (var apt in appointments)
    {
        Console.WriteLine($"  - {apt}");
    }
    Console.WriteLine("\n" + new string('=', 80) + "\n");

    var clinic = SchedulingAlgorithm.Schedule(appointments);

    Console.WriteLine($"Consultórios necessários: {clinic.GetTotalOffices()}\n");
    Console.WriteLine(clinic);
}
catch (Exception ex)
{
    Console.Error.WriteLine($"Erro: {ex.Message}");
    Environment.Exit(1);
}
