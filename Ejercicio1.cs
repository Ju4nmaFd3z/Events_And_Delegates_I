using System;
using System.Collections.Generic;

namespace Ejercicios_Eventos.Ejercicio1
{
    // Datos de la venta que viajan con el evento
    public class VentaEventArgs : EventArgs
    {
        public string Producto { get; set; }
        public double Precio { get; set; }

        public VentaEventArgs(string producto, double precio)
        {
            Producto = producto;
            Precio = precio;
        }
    }

    // Emisor: avisa cada vez que se hace una venta
    public class RegistroVentas
    {
        // El delegado fija la firma de los receptores
        public delegate void GestorEventoVenta(object fuente, VentaEventArgs args);

        // El ? evita el aviso de nulos, puede no haber suscriptores
        public event GestorEventoVenta? VentaRealizada;

        // Simula una venta y lanza el evento
        public void ProcesarVenta(string producto, double precio)
        {
            Console.WriteLine($"Procesando la venta de {producto}...");
            EnVentaRealizada(producto, precio);
        }

        // Dispara el evento si hay alguien suscrito
        protected virtual void EnVentaRealizada(string producto, double precio)
        {
            if (VentaRealizada != null)
            {
                VentaRealizada(this, new VentaEventArgs(producto, precio));
            }
        }
    }

    // Receptor: guarda los detalles de cada venta
    public class ServicioRegistro
    {
        private List<string> ventas = new List<string>();

        public void RegistrarVenta(object fuente, VentaEventArgs e)
        {
            ventas.Add($"{e.Producto} - {e.Precio:F2} euros");
            Console.WriteLine($"Registro: venta guardada ({e.Producto}, {e.Precio:F2} euros). Ventas registradas: {ventas.Count}");
        }
    }

    // Receptor: avisa al usuario de la venta
    public class ServicioNotificacion
    {
        public void EnviarNotificacionVenta(object fuente, VentaEventArgs e)
        {
            Console.WriteLine($"Notificación: se ha vendido {e.Producto} por {e.Precio:F2} euros");
        }
    }

    // Programa de prueba del ejercicio
    public static class Prueba
    {
        public static void Ejecutar()
        {
            RegistroVentas registroVentas = new RegistroVentas();
            ServicioRegistro servicioRegistro = new ServicioRegistro();
            ServicioNotificacion servicioNotificacion = new ServicioNotificacion();

            // Suscribimos los dos receptores al evento
            registroVentas.VentaRealizada += servicioRegistro.RegistrarVenta;
            registroVentas.VentaRealizada += servicioNotificacion.EnviarNotificacionVenta;

            registroVentas.ProcesarVenta("Portátil", 899.99);
            registroVentas.ProcesarVenta("Ratón", 15.5);
            registroVentas.ProcesarVenta("Teclado", 49);
        }
    }
}
