namespace ClinicaVeterinaria.Tests;

using System;
using System.Collections.Generic;
using System.Linq;
using ClinicaVeterinaria.Models;
using ClinicaVeterinaria.Scheduler;
using Xunit;

public class AppointmentTests
{
    [Fact]
    public void ParseExpressoAppointment_ShouldBe10Minutes()
    {
        var apt = new Appointment("Aplicação de vacina", "expresso");
        Assert.Equal(10, apt.Duration);
        Assert.True(apt.IsExpressed);
    }

    [Fact]
    public void ParseTimedAppointment_ShouldExtractDuration()
    {
        var apt = new Appointment("Castração de gato adulto", "90min");
        Assert.Equal(90, apt.Duration);
        Assert.False(apt.IsExpressed);
    }

    [Fact]
    public void ParseTimedAppointment_WithoutMinSuffix()
    {
        var apt = new Appointment("Consulta de rotina", "30");
        Assert.Equal(30, apt.Duration);
    }
}

public class SessionTests
{
    [Fact]
    public void MorningSession_ShouldHaveCorrectTimes()
    {
        var session = new Session(SessionType.Morning);
        Assert.Equal(8, session.StartHour);
        Assert.Equal(0, session.StartMinute);
        Assert.Equal(11, session.EndHour);
        Assert.Equal(30, session.EndMinute);
    }

    [Fact]
    public void AfternoonSession_ShouldHaveCorrectTimes()
    {
        var session = new Session(SessionType.Afternoon);
        Assert.Equal(13, session.StartHour);
        Assert.Equal(30, session.StartMinute);
        Assert.Equal(17, session.EndHour);
        Assert.Equal(0, session.EndMinute);
    }

    [Fact]
    public void MorningSession_MaxDurationShouldBe210Minutes()
    {
        var session = new Session(SessionType.Morning);
        Assert.Equal(210, session.MaxDuration);
    }

    [Fact]
    public void AfternoonSession_MaxDurationShouldBe210Minutes()
    {
        var session = new Session(SessionType.Afternoon);
        Assert.Equal(210, session.MaxDuration);
    }

    [Fact]
    public void AddAppointment_ShouldSucceedIfItFits()
    {
        var session = new Session(SessionType.Morning);
        var apt = new Appointment("Consulta", "60min");
        Assert.True(session.AddAppointment(apt));
        Assert.Single(session.Appointments);
        Assert.Equal(60, session.UsedTime);
    }

    [Fact]
    public void AddAppointment_ShouldFailIfItDoesNotFit()
    {
        var session = new Session(SessionType.Morning);
        var apt = new Appointment("Cirurgia longa", "300min");
        Assert.False(session.AddAppointment(apt));
        Assert.Empty(session.Appointments);
    }

    [Fact]
    public void GetRemainingTime_ShouldTrackCorrectly()
    {
        var session = new Session(SessionType.Morning);
        var apt1 = new Appointment("Consulta 1", "60min");
        var apt2 = new Appointment("Consulta 2", "90min");

        session.AddAppointment(apt1);
        Assert.Equal(150, session.GetRemainingTime());

        session.AddAppointment(apt2);
        Assert.Equal(60, session.GetRemainingTime());
    }

    [Fact]
    public void GetEndTime_ShouldCalculateCorrectly()
    {
        var session = new Session(SessionType.Morning);
        var apt = new Appointment("Consulta", "90min");
        session.AddAppointment(apt);

        Assert.Equal("09:30", session.GetEndTime());
    }
}

public class OfficeTests
{
    [Fact]
    public void CreateOffice_ShouldHaveCorrectId()
    {
        var office = new Office(1);
        Assert.Equal(1, office.Id);
    }

    [Fact]
    public void AddAppointment_ToMorningSession_IfItFits()
    {
        var office = new Office(1);
        var apt = new Appointment("Consulta", "60min");
        Assert.True(office.AddAppointment(apt));
        Assert.Single(office.MorningSession.Appointments);
    }

    [Fact]
    public void AddAppointment_ToAfternoon_IfMorningFull()
    {
        var office = new Office(1);
        var apt1 = new Appointment("Cirurgia", "150min");
        var apt2 = new Appointment("Consulta", "70min");

        office.AddAppointment(apt1);  // Morning: 150/210
        office.AddAppointment(apt2);  // Won't fit in morning (150+70=220), goes to afternoon

        Assert.Single(office.MorningSession.Appointments);
        Assert.Single(office.AfternoonSession.Appointments);
    }

    [Fact]
    public void AddAppointment_ShouldFailIfBothSessionsFull()
    {
        var office = new Office(1);
        var apt1 = new Appointment("Cirurgia 1", "180min");
        var apt2 = new Appointment("Cirurgia 2", "180min");
        var apt3 = new Appointment("Cirurgia 3", "100min");

        office.AddAppointment(apt1);
        office.AddAppointment(apt2);
        Assert.False(office.AddAppointment(apt3));
    }
}

public class ClinicTests
{
    [Fact]
    public void NewClinic_ShouldHaveNoOffices()
    {
        var clinic = new Clinic();
        Assert.Equal(0, clinic.GetTotalOffices());
    }

    [Fact]
    public void AddOffice_ShouldIncrementCount()
    {
        var clinic = new Clinic();
        clinic.AddOffice();
        Assert.Equal(1, clinic.GetTotalOffices());
    }

    [Fact]
    public void ScheduleAppointment_ShouldUseExistingOffice()
    {
        var clinic = new Clinic();
        var apt = new Appointment("Consulta", "60min");
        clinic.ScheduleAppointment(apt);
        Assert.Equal(1, clinic.GetTotalOffices());
    }

    [Fact]
    public void ScheduleAppointment_ShouldCreateNewOfficeWhenNeeded()
    {
        var clinic = new Clinic();
        var apt1 = new Appointment("Cirurgia 1", "180min");
        var apt2 = new Appointment("Cirurgia 2", "180min");
        var apt3 = new Appointment("Consulta", "60min");

        clinic.ScheduleAppointment(apt1);
        clinic.ScheduleAppointment(apt2);
        clinic.ScheduleAppointment(apt3);

        Assert.Equal(2, clinic.GetTotalOffices());
    }
}

public class SchedulingAlgorithmTests
{
    [Fact]
    public void Schedule_ShouldAllocateAllAppointments()
    {
        var appointments = new List<Appointment>
        {
            new("Consulta", "30min"),
            new("Cirurgia", "120min"),
            new("Vacina", "expresso"),
        };

        var clinic = SchedulingAlgorithm.Schedule(appointments);
        var totalScheduled = clinic.Offices.Sum(o =>
            o.MorningSession.Appointments.Count + o.AfternoonSession.Appointments.Count);

        Assert.Equal(3, totalScheduled);
    }

    [Fact]
    public void Schedule_ShouldSortByDurationDescending()
    {
        var appointments = new List<Appointment>
        {
            new("Consulta", "30min"),
            new("Cirurgia", "120min"),
            new("Exame", "60min"),
        };

        var clinic = SchedulingAlgorithm.Schedule(appointments);
        var firstAppointment = clinic.Offices[0].MorningSession.Appointments[0];
        Assert.Equal(120, firstAppointment.appointment.Duration);
    }

    [Fact]
    public void Schedule_ShouldPackEfficientlyWithMixedDurations()
    {
        var appointments = new List<Appointment>
        {
            new("A", "100min"),
            new("B", "100min"),
            new("C", "50min"),
            new("D", "30min"),
            new("E", "30min"),
        };

        var clinic = SchedulingAlgorithm.Schedule(appointments);
        Assert.True(clinic.GetTotalOffices() <= 2);
    }

    [Fact]
    public void Schedule_ShouldHandleAllExpressoAppointments()
    {
        var appointments = new List<Appointment>
        {
            new("Vacina 1", "expresso"),
            new("Vacina 2", "expresso"),
            new("Vacina 3", "expresso"),
        };

        var clinic = SchedulingAlgorithm.Schedule(appointments);
        Assert.Equal(1, clinic.GetTotalOffices());
    }

    [Fact]
    public void Schedule_ShouldRespectTimeConstraints()
    {
        var appointments = new List<Appointment>
        {
            new("Consulta", "30min"),
            new("Exame", "60min"),
        };

        var clinic = SchedulingAlgorithm.Schedule(appointments);

        foreach (var office in clinic.Offices)
        {
            Assert.True(office.MorningSession.UsedTime <= 210);
            Assert.True(office.AfternoonSession.UsedTime <= 210);
        }
    }
}
