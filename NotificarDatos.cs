using System;

namespace GestionValesRdz
{
    public static class NotificadorDatos
    {
        // Evento estático al que cualquier formulario se puede suscribir.
        public static event EventHandler DatosCambiados;

        // Método para disparar el evento. Lo llamaremos cuando guardemos cambios.
        public static void AnunciarCambio()
        {
            // Si hay alguien suscrito (escuchando), se ejecuta el evento.
            DatosCambiados?.Invoke(null, EventArgs.Empty);
        }
    }
}
