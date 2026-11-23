using System;

namespace Ejercicios_Eventos.Ejercicio3
{
    // Nombre del archivo y fecha que viajan con el evento
    public class BackupEventArgs : EventArgs
    {
        public string NombreArchivo { get; set; }
        public DateTime Fecha { get; set; }

        public BackupEventArgs(string nombreArchivo, DateTime fecha)
        {
            NombreArchivo = nombreArchivo;
            Fecha = fecha;
        }
    }

    // Emisor: gestiona las copias de seguridad
    public class GestorBackups
    {
        // El delegado fija la firma de los receptores
        public delegate void GestorEventoBackup(object fuente, BackupEventArgs args);

        // El ? evita el aviso de nulos, puede no haber suscriptores
        public event GestorEventoBackup? BackupCompletado;

        // Simula la copia de seguridad de un archivo
        public void RealizarBackup(string nombreArchivo)
        {
            Console.WriteLine($"Haciendo backup de {nombreArchivo}...");
            EnBackupCompletado(nombreArchivo);
        }

        // Dispara el evento si hay alguien suscrito
        protected virtual void EnBackupCompletado(string nombreArchivo)
        {
            if (BackupCompletado != null)
            {
                BackupCompletado(this, new BackupEventArgs(nombreArchivo, DateTime.Now));
            }
        }
    }

    // Receptor: envía una notificación del backup
    public class ServicioNotificacion
    {
        public void EnviarNotificacionBackup(object fuente, BackupEventArgs e)
        {
            Console.WriteLine($"Notificación: backup de {e.NombreArchivo} completado el {e.Fecha:dd/MM/yyyy HH:mm}");
        }
    }

    // Receptor: registra la operación en el log
    public class ServicioLog
    {
        public void RegistrarBackup(object fuente, BackupEventArgs e)
        {
            Console.WriteLine($"Log: copia de {e.NombreArchivo} realizada el {e.Fecha:dd/MM/yyyy HH:mm:ss}");
        }
    }

    // Programa de prueba del ejercicio
    public static class Prueba
    {
        public static void Ejecutar()
        {
            GestorBackups gestor = new GestorBackups();
            ServicioNotificacion servicioNotificacion = new ServicioNotificacion();
            ServicioLog servicioLog = new ServicioLog();

            // Suscribimos los dos receptores al evento
            gestor.BackupCompletado += servicioNotificacion.EnviarNotificacionBackup;
            gestor.BackupCompletado += servicioLog.RegistrarBackup;

            gestor.RealizarBackup("informe.docx");
            gestor.RealizarBackup("fotos.zip");
            gestor.RealizarBackup("base_datos.sql");
        }
    }
}
