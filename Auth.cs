using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionValesRdz
{
    public static class Auth
    {
        public static int Id { get; set; }
        public static string Usuario { get; set; }
        public static string Nombre { get; set; }

        public static List<permisos> Permisos { get; set; }

        public static bool TienePermiso(string permiso)
        {

            foreach (var priv in Permisos)
            {
                if (priv.permiso == permiso)
                    return true;
            }

            return false;
        }
    }
}
