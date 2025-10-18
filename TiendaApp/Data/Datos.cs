using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;      // Paquete Npgsql (de NuGet)
using System;
using System.Data;

namespace TiendaApp.Data
{
    public class Datos
    {
        // 🔗 Cadena de conexión (ajusta el puerto si usaste 5433)
        private readonly string _cs =
            "Host=localhost;Port=5432;Username=postgres;Password=postgres;Database=tienda_db";

        // 👉 Método para INSERT, UPDATE o DELETE
        public bool ExecuteQuery(string sql)
        {
            try
            {
                using (var conn = new NpgsqlConnection(_cs))
                {
                    conn.Open();
                    using (var cmd = new NpgsqlCommand(sql, conn))
                    {
                        int rows = cmd.ExecuteNonQuery();
                        return rows > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                // En "Octubre" solo devolvía false, así que no mostramos detalles
                return false;
            }
        }

        // 👉 Método para SELECT (retorna un DataTable)
        public DataTable GetDataTable(string sql)
        {
            var dt = new DataTable();

            using (var conn = new NpgsqlConnection(_cs))
            {
                conn.Open();
                using (var da = new NpgsqlDataAdapter(sql, conn))
                {
                    da.Fill(dt);
                }
            }

            return dt;
        }
    }
}

