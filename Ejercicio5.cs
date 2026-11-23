using System;

namespace Ejercicios_Eventos.Ejercicio5
{
    // Consumo actual y umbral que viajan con el evento
    public class EnergiaEventArgs : EventArgs
    {
        public double ConsumoActual { get; set; }
        public double Umbral { get; set; }

        public EnergiaEventArgs(double consumoActual, double umbral)
        {
            ConsumoActual = consumoActual;
            Umbral = umbral;
        }
    }

    // Emisor: controla el consumo de energía
    public class MonitorEnergia
    {
        // El delegado fija la firma de los receptores
        public delegate void GestorEventoEnergia(object fuente, EnergiaEventArgs args);

        // El ? evita el aviso de nulos, puede no haber suscriptores
        public event GestorEventoEnergia? ConsumoExcesivoDetectado;

        private double umbral;

        public MonitorEnergia(double umbral)
        {
            this.umbral = umbral;
        }

        // Registra el consumo y avisa si pasa el umbral
        public void RegistrarConsumo(double consumo)
        {
            Console.WriteLine($"Consumo actual: {consumo:F1} kWh");

            if (consumo > umbral)
            {
                EnConsumoExcesivoDetectado(consumo);
            }
        }

        // Dispara el evento si hay alguien suscrito
        protected virtual void EnConsumoExcesivoDetectado(double consumo)
        {
            if (ConsumoExcesivoDetectado != null)
            {
                ConsumoExcesivoDetectado(this, new EnergiaEventArgs(consumo, umbral));
            }
        }
    }

    // Receptor: envía una advertencia al usuario
    public class ServicioNotificacion
    {
        public void EnviarAdvertencia(object fuente, EnergiaEventArgs e)
        {
            Console.WriteLine($"Advertencia: el consumo de {e.ConsumoActual:F1} kWh supera el umbral de {e.Umbral:F1} kWh");
        }
    }

    // Receptor: ajusta los dispositivos para gastar menos
    public class ServicioAjusteAutomatizado
    {
        public void AjustarDispositivos(object fuente, EnergiaEventArgs e)
        {
            double exceso = e.ConsumoActual - e.Umbral;
            Console.WriteLine($"Ajuste automático: se apagan luces y se baja la calefacción (exceso de {exceso:F1} kWh)");
        }
    }

    // Programa de prueba del ejercicio
    public static class Prueba
    {
        public static void Ejecutar()
        {
            MonitorEnergia monitor = new MonitorEnergia(5);
            ServicioNotificacion servicioNotificacion = new ServicioNotificacion();
            ServicioAjusteAutomatizado servicioAjuste = new ServicioAjusteAutomatizado();

            // Suscribimos los dos receptores al evento
            monitor.ConsumoExcesivoDetectado += servicioNotificacion.EnviarAdvertencia;
            monitor.ConsumoExcesivoDetectado += servicioAjuste.AjustarDispositivos;

            monitor.RegistrarConsumo(3.8);
            monitor.RegistrarConsumo(4.9);
            monitor.RegistrarConsumo(5.6);
            monitor.RegistrarConsumo(7.3);
        }
    }
}
