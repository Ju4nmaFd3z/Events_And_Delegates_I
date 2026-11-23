using System;

namespace Ejercicios_Eventos.Ejercicio2
{
    // Temperatura actual y umbral que viajan con el evento
    public class TemperaturaEventArgs : EventArgs
    {
        public double Temperatura { get; set; }
        public double Umbral { get; set; }

        public TemperaturaEventArgs(double temperatura, double umbral)
        {
            Temperatura = temperatura;
            Umbral = umbral;
        }
    }

    // Emisor: vigila la temperatura del invernadero
    public class ControlTemperatura
    {
        // El delegado fija la firma de los receptores
        public delegate void GestorEventoTemperatura(object fuente, TemperaturaEventArgs args);

        // El ? evita el aviso de nulos, puede no haber suscriptores
        public event GestorEventoTemperatura? TemperaturaAlta;

        private double umbral;

        public ControlTemperatura(double umbral)
        {
            this.umbral = umbral;
        }

        // Lee la temperatura y avisa si pasa el umbral
        public void LeerTemperatura(double temperatura)
        {
            Console.WriteLine($"Temperatura actual: {temperatura:F1} °C");

            if (temperatura > umbral)
            {
                EnTemperaturaAlta(temperatura);
            }
        }

        // Dispara el evento si hay alguien suscrito
        protected virtual void EnTemperaturaAlta(double temperatura)
        {
            if (TemperaturaAlta != null)
            {
                TemperaturaAlta(this, new TemperaturaEventArgs(temperatura, umbral));
            }
        }
    }

    // Receptor: muestra una alerta en consola
    public class ServicioAlerta
    {
        public void EnviarAlerta(object fuente, TemperaturaEventArgs e)
        {
            Console.WriteLine($"¡ALERTA! Temperatura alta en el invernadero: {e.Temperatura:F1} °C (umbral {e.Umbral:F1} °C)");
        }
    }

    // Receptor: registra la temperatura en consola
    public class ServicioRegistroTemperatura
    {
        public void RegistrarTemperatura(object fuente, TemperaturaEventArgs e)
        {
            Console.WriteLine($"Registro: temperatura de {e.Temperatura:F1} °C guardada (umbral {e.Umbral:F1} °C)");
        }
    }

    // Programa de prueba del ejercicio
    public static class Prueba
    {
        public static void Ejecutar()
        {
            ControlTemperatura control = new ControlTemperatura(30);
            ServicioAlerta servicioAlerta = new ServicioAlerta();
            ServicioRegistroTemperatura servicioRegistro = new ServicioRegistroTemperatura();

            // Suscribimos los dos receptores al evento
            control.TemperaturaAlta += servicioAlerta.EnviarAlerta;
            control.TemperaturaAlta += servicioRegistro.RegistrarTemperatura;

            control.LeerTemperatura(24.5);
            control.LeerTemperatura(29);
            control.LeerTemperatura(31.5);
            control.LeerTemperatura(35.2);
        }
    }
}
