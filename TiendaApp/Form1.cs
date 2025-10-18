using System;
using System.Data;
using System.Windows.Forms;
using TiendaApp.Data;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TiendaApp
{
    public partial class Form1 : Form
    {
        Datos data = new Datos();
        public Form1()
        {
            InitializeComponent();
        }

        private void btnCargar_Click(object sender, EventArgs e)
        {
            CargarProductos();
        }

        private void CargarProductos()
        {
            string sql = @"
                SET search_path TO tienda;
                SELECT id_producto, id_categoria, nombre, precio, stock
                FROM productos
                ORDER BY id_producto;";

            dgvProductos.DataSource = data.GetDataTable(sql);
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            try
            {
                int idCat = int.Parse(txtIdCat.Text);
                string nombre = txtNombreProd.Text.Trim();
                decimal precio = decimal.Parse(txtPrecio.Text);
                int stock = int.Parse(txtStock.Text);

                string sql = $@"
                    SET search_path TO tienda;
                    INSERT INTO productos (id_categoria, nombre, precio, stock)
                    VALUES ({idCat}, '{nombre}', {precio}, {stock});";

                bool ok = data.ExecuteQuery(sql);
                MessageBox.Show(ok ? "Producto agregado" : "Error al agregar");
                if (ok) CargarProductos();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Verifica los datos. " + ex.Message);
            }


        }

        private void btnActualizar_Click(object sender, EventArgs e)
        {
            if (dgvProductos.CurrentRow == null)
            {
                MessageBox.Show("Selecciona un producto en la tabla.");
                return;
            }

            try
            {
                // Toma el id de la fila seleccionada
                int idProd = Convert.ToInt32(dgvProductos.CurrentRow.Cells["id_producto"].Value);

                int idCat = int.Parse(txtIdCat.Text);
                string nombre = txtNombreProd.Text.Trim();
                decimal precio = decimal.Parse(txtPrecio.Text);
                int stock = int.Parse(txtStock.Text);

                string sql = $@"
                    SET search_path TO tienda;
                    UPDATE productos
                    SET id_categoria = {idCat},
                        nombre = '{nombre}',
                        precio = {precio},
                        stock = {stock}
                    WHERE id_producto = {idProd};";

                bool ok = data.ExecuteQuery(sql);
                MessageBox.Show(ok ? "Producto actualizado" : "No se actualizó");
                if (ok) CargarProductos();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Verifica los datos. " + ex.Message);
            }

        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (dgvProductos.CurrentRow == null)
            {
                MessageBox.Show("Selecciona un producto en la tabla.");
                return;
            }

            int idProd = Convert.ToInt32(dgvProductos.CurrentRow.Cells["id_producto"].Value);

            var r = MessageBox.Show("¿Eliminar el producto seleccionado?", "Confirmar",
                                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (r == DialogResult.Yes)
            {
                string sql = $@"
                    SET search_path TO tienda;
                    DELETE FROM productos WHERE id_producto = {idProd};";

                bool ok = data.ExecuteQuery(sql);
                MessageBox.Show(ok ? "Producto eliminado" : "No se eliminó");
                if (ok) CargarProductos();
            }
        }

        private void dgvProductos_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvProductos.CurrentRow == null) return;

            // Carga los campos en los TextBox para editar
            txtIdCat.Text = dgvProductos.CurrentRow.Cells["id_categoria"].Value?.ToString();
            txtNombreProd.Text = dgvProductos.CurrentRow.Cells["nombre"].Value?.ToString();
            txtPrecio.Text = dgvProductos.CurrentRow.Cells["precio"].Value?.ToString();
            txtStock.Text = dgvProductos.CurrentRow.Cells["stock"].Value?.ToString();
        }
    }
}
