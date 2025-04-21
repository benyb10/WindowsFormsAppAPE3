using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsAppAPE3
{
    public partial class REGISTRO: Form
    {
        public REGISTRO()
        {
            
            InitializeComponent();
            CargarUsuarios();
            cmbRol.Items.AddRange(new string[] { "MEDICO", "ENFERMERA", "PACIENTE" });
        }


        private void Limpiar() {
            txtCedula.Text = "";
            txtOCedula.Text = "";
            txtPrimerNombre.Text = "";
            txtSegundoNombre.Text = "";
            txtPrimerApellido.Text = "";
            txtSegundoApellido.Text = "";
            txtCorreo.Text = "";
            txtDireccion.Text = "";
            cmbRol.SelectedItem = "";
        }

        private void CargarUsuarios()
        {
            using (var db = new ValidacionesEntities1())
            {
                var usuarios = db.Usuarios
                    .Select(u => new
                    {
                        u.Cedula,
                        u.PrimerNombre,
                        u.SegundoNombre,
                        u.PrimerApellido,
                        u.SegundoApellido,
                        u.CorreoInstitucional,
                        u.Provincia,
                        u.Rol
                    })
                    .ToList();

                dgvUsuarios.DataSource = usuarios;
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            new BIENVENIDA().Show();
            this.Hide();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            // Mostrar un mensaje de confirmación antes de salir
            DialogResult resultado = MessageBox.Show("¿Está seguro de que desea salir?", "Confirmar salida", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            // Si el usuario confirma, cerrar la aplicación
            if (resultado == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private bool ValidarFormulario()
        {
            // Validar cédula: solo números y 10 dígitos
            if (!Regex.IsMatch(txtCedula.Text.Trim(), @"^\d{10}$"))
            {
                MessageBox.Show("La cédula debe tener exactamente 10 dígitos numéricos.");
                return false;
            }

            // Validar nombres y apellidos: solo letras y espacios
            string patronNombre = @"^[A-Za-zÁÉÍÓÚáéíóúÑñ\s]+$";

            if (!Regex.IsMatch(txtPrimerNombre.Text.Trim(), patronNombre))
            {
                MessageBox.Show("El primer nombre no debe contener números ni caracteres especiales.");
                return false;
            }

            if (!Regex.IsMatch(txtSegundoNombre.Text.Trim(), patronNombre))
            {
                MessageBox.Show("El segundo nombre no debe contener números ni caracteres especiales.");
                return false;
            }

            if (!Regex.IsMatch(txtPrimerApellido.Text.Trim(), patronNombre))
            {
                MessageBox.Show("El primer apellido no debe contener números ni caracteres especiales.");
                return false;
            }

            if (!Regex.IsMatch(txtSegundoApellido.Text.Trim(), patronNombre))
            {
                MessageBox.Show("El segundo apellido no debe contener números ni caracteres especiales.");
                return false;
            }

            // Validar correo institucional
            if (!Regex.IsMatch(txtCorreo.Text.Trim(), @"^[\w\.-]+@clinica\.ec$"))
            {
                MessageBox.Show("El correo debe terminar en @clinica.ec.");
                return false;
            }

            // Validar selección de rol
            if (cmbRol.SelectedItem == null)
            {
                MessageBox.Show("Debe seleccionar un rol.");
                return false;
            }

            return true;

        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            if (!ValidarFormulario())
                return;

            using (var db = new ValidacionesEntities1())
            {
                string cedulaIngresada = txtCedula.Text.Trim();

                // Verificar si ya existe un usuario con esa cédula
                var usuarioExistente = db.Usuarios.FirstOrDefault(u => u.Cedula == cedulaIngresada);
                if (usuarioExistente != null)
                {
                    MessageBox.Show("Ya existe un usuario registrado con esa cédula.", "Cédula duplicada", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var nuevoUsuario = new Usuarios
                {
                    Cedula = cedulaIngresada,
                    PrimerNombre = txtPrimerNombre.Text.Trim(),
                    SegundoNombre = txtSegundoNombre.Text.Trim(),
                    PrimerApellido = txtPrimerApellido.Text.Trim(),
                    SegundoApellido = txtSegundoApellido.Text.Trim(),
                    CorreoInstitucional = txtCorreo.Text.Trim(),
                    Provincia = txtDireccion.Text.Trim(),
                    Rol = cmbRol.SelectedItem.ToString()
                };

                db.Usuarios.Add(nuevoUsuario);
                db.SaveChanges();
                MessageBox.Show("Usuario agregado correctamente.");
                CargarUsuarios();
                Limpiar();
            }
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            string cedula = txtCedula.Text.Trim();

            if (!ValidarFormulario())
                return;

            using (var db = new ValidacionesEntities1())
            {
                var usuario = db.Usuarios.FirstOrDefault(u => u.Cedula == cedula);
                if (usuario != null)
                {
                    usuario.PrimerNombre = txtPrimerNombre.Text.Trim();
                    usuario.SegundoNombre = txtSegundoNombre.Text.Trim();
                    usuario.PrimerApellido = txtPrimerApellido.Text.Trim();
                    usuario.SegundoApellido = txtSegundoApellido.Text.Trim();
                    usuario.CorreoInstitucional = txtCorreo.Text.Trim();
                    usuario.Provincia = txtDireccion.Text.Trim();
                    usuario.Rol = cmbRol.SelectedItem.ToString();

                    db.SaveChanges();
                    MessageBox.Show("Usuario modificado correctamente.");
                    CargarUsuarios();
                    Limpiar();
                }
                else
                {
                    MessageBox.Show("Usuario no encontrado.");
                }
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            string cedula = txtCedula.Text.Trim();

            using (var db = new ValidacionesEntities1())
            {
                var usuario = db.Usuarios.FirstOrDefault(u => u.Cedula == cedula);
                if (usuario != null)
                {
                    db.Usuarios.Remove(usuario);
                    db.SaveChanges();
                    MessageBox.Show("Usuario eliminado correctamente.");
                    CargarUsuarios();
                    Limpiar();
                }
                else
                {
                    MessageBox.Show("Usuario no encontrado.");
                }
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            string cedulaActual = txtOCedula.Text.Trim();
            using (var db = new ValidacionesEntities1())
            {
                var usuarios = db.Usuarios
                    .Where(u => u.Cedula == cedulaActual)
                    .Select(u => new
                    {
                        u.Cedula,
                        u.PrimerNombre,
                        u.SegundoNombre,
                        u.PrimerApellido,
                        u.SegundoApellido,
                        u.CorreoInstitucional,
                        u.Provincia,
                        u.Rol
                    })
                    .ToList();

                dgvUsuarios.DataSource = usuarios;
                Limpiar();
            }
        }

        private void dgvUsuarios_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow fila = dgvUsuarios.Rows[e.RowIndex];

                txtCedula.Text = fila.Cells["Cedula"].Value.ToString();
                txtPrimerNombre.Text = fila.Cells["PrimerNombre"].Value.ToString().Split(' ')[0];
                txtSegundoNombre.Text = fila.Cells["SegundoNombre"].Value.ToString().Split(' ')[0];
                txtPrimerApellido.Text = fila.Cells["PrimerApellido"].Value.ToString().Split(' ')[0];
                txtSegundoApellido.Text = fila.Cells["SegundoApellido"].Value.ToString().Split(' ')[0];
                txtCorreo.Text = fila.Cells["CorreoInstitucional"].Value.ToString();
                txtDireccion.Text = fila.Cells["Provincia"].Value.ToString();
                cmbRol.SelectedItem = fila.Cells["Rol"].Value.ToString();
            }
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            Limpiar();
        }
    }
}
