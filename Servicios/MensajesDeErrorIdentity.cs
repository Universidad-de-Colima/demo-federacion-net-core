using Microsoft.AspNetCore.Identity;

namespace PruebaFederacion.Servicios
{
    public class MensajesDeErrorIdentity:IdentityErrorDescriber
    {
        public override IdentityError DefaultError()
        {
            return  new IdentityError { Code=nameof(DefaultError),Description=$"Ha ocurrio un error."};
        }

    }
}
