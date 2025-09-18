using System.Data.Entity;

namespace GestionValesRdz
{
    public partial class ValesRdzDatosEntities : DbContext
    {
        public ValesRdzDatosEntities(string cs) 
            : base(cs)
        {

        }
    }
}
