using System;
using System.Collections.Generic;

namespace Ejercicios_Eventos.Ejercicio4
{
    // Nombre del sensor y hora de detección que viajan con el evento
    public class IntrusoEventArgs : EventArgs
    {
        public string NombreSensor { get; set; }
        public DateTime Hora { get; set; }

        public IntrusoEventArgs(string nombreSensor, DateTime hora)
        {
            NombreSensor = nombreSensor;
            Hora = hora;
        }
    }

    // Emisor: vigila las puertas y ventanas de la casa
    public class SensorMonitoreo
    {
        // El delegado fija la firma de los receptores
        public delegate void GestorEventoIntruso(object fuente, IntrusoEventArgs args);

        // El ? evita el aviso de nulos, puede no haber suscriptores
        public event GestorEventoIntruso? AlertaIntruso;

        // Horario permitido, de 7:00 a 22:00
        private int horaInicio = 7;
        private int horaFin = 22;

        // Detecta una apertura y avisa si es fuera de horario
        public void DetectarApertura(string nombreSensor, DateTime hora)
        {
            Console.WriteLine($"Sensor: {nombreSensor} abierta a las {hora:HH:mm}");

            if (hora.Hour < horaInicio || hora.Hour >= horaFin)
            {
                EnAlertaIntruso(nombreSensor, hora);
            }
            else
            {
                Console.WriteLine("Todo normal: dentro del horario permitido.");
            }
        }

        // Dispara el evento si hay alguien suscrito
        protected virtual void EnAlertaIntruso(string nombreSensor, DateTime hora)
        {
            if (AlertaIntruso != null)
            {
                AlertaIntruso(this, new IntrusoEventArgs(nombreSensor, hora));
            }
        }
    }

    // Receptor: activa la alarma
    public class ServicioAlarma
    {
        public void ActivarAlarma(object fuente, IntrusoEventArgs e)
        {
            Console.WriteLine($"¡ALARMA ACTIVADA! {e.NombreSensor} abierta a las {e.Hora:HH:mm}");
        }
    }

    // Receptor: guarda la incidencia en una "base de datos" simulada
    public class ServicioRegistroIncidencias
    {
        private List<string> incidencias = new List<string>();

        public void GuardarIncidencia(object fuente, IntrusoEventArgs e)
        {
            incidencias.Add($"{e.NombreSensor} - {e.Hora:HH:mm}");
            Console.WriteLine($"Base de datos: incidencia guardada ({e.NombreSensor}, {e.Hora:HH:mm}). Incidencias guardadas: {incidencias.Count}");
        }
    }

    // Programa de prueba del ejercicio
    public static class Prueba
    {
        public static void Ejecutar()
        {
            SensorMonitoreo sensor = new SensorMonitoreo();
            ServicioAlarma servicioAlarma = new ServicioAlarma();
            ServicioRegistroIncidencias servicioRegistro = new ServicioRegistroIncidencias();

            // Suscribimos los dos receptores al evento
            sensor.AlertaIntruso += servicioAlarma.ActivarAlarma;
            sensor.AlertaIntruso += servicioRegistro.GuardarIncidencia;

            sensor.DetectarApertura("Puerta principal", new DateTime(2026, 1, 15, 10, 30, 0));
            sensor.DetectarApertura("Ventana del salón", new DateTime(2026, 1, 15, 23, 45, 0));
            sensor.DetectarApertura("Puerta del garaje", new DateTime(2026, 1, 16, 3, 15, 0));
            sensor.DetectarApertura("Ventana de la cocina", new DateTime(2026, 1, 16, 18, 20, 0));
        }
    }
}
