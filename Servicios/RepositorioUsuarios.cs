using Microsoft.Data.SqlClient;
using Dapper;
using PruebaFederacion.Models;
using System.Runtime.InteropServices;

namespace PruebaFederacion.Servicios
{
    public interface IRepositorioUsuarios
    {
        Task<Usuario> BuscarUsuarioPorEmail(string email);
        Task<Usuario> BuscarUsuarioPorId(int id);
        Task<int> CrearUsuario(Usuario usuario);
    }

    public class RepositorioUsuarios: IRepositorioUsuarios
    {
        private readonly string connectionstring;
        public RepositorioUsuarios(IConfiguration configuration) {
            connectionstring = configuration.GetConnectionString("DefaulConnection");
        }
        public async Task<int> CrearUsuario(Usuario usuario)
        {
            ////polo op a la interfas ctrl+.
            //using var connection = new SqlConnection(connectionstring);
            //var id = await connection.QuerySingleAsync<string>(@"Insert into Usuarios(numtrabajador,nombre,email,clavehash)
            //                                                    values (@numtrabajador,@nombre,@email, @clavehash) SELECT SCOPE_IDENTITY()", usuario);
            int id = 1;

            return id;

        }
        public async Task<Usuario> BuscarUsuarioPorId(int numTrabajador) { 

            //using var connection = new SqlConnection(connectionstring);
            //return await connection.QuerySingleOrDefaultAsync<Usuario>("Select * from usuarios where id=@id", new { numTrabajador });
          
            return  null;
        }

        public async Task<Usuario> BuscarUsuarioPorEmail(string email)
        {

            //using var connection = new SqlConnection(connectionstring);
            // return await connection.QuerySingleOrDefaultAsync<Usuario>("Select * from usuarios where email=@email", new { email });
            var usua = new Usuario();
            usua.Id = 1;
            usua.Nombre = "Tania";
            usua.numTrabajador = "8811";
            usua.Email = "Tania";
            usua.ClaveHash = "AQAAAAEAACcQAAAAEFFulJjKcnXyLIxSJZBzcEr70cijbBZ8D6sIwD915Dgs3RKiYi0gMiQKfzuvGguC7w==";

            return usua;


        }
    }
}
