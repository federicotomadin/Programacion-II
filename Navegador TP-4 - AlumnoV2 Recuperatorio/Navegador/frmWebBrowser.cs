﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Threading;
using Hilo;

namespace Navegador
{
    public partial class frmWebBrowser : Form
    {
        private const string ESCRIBA_AQUI = "Escriba aquí...";
        Archivos.Texto archivos;

        /// <summary>
        /// Constructor publico. Inicializa el Formulario.
        /// </summary>
        public frmWebBrowser()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Carga el formulario.
        /// </summary>
        /// <param name="sender">objeto de tipo Object</param>
        /// <param name="e">Objeto de tipo EventArgs</param>
        private void frmWebBrowser_Load(object sender, EventArgs e)
        {
            this.txtUrl.SelectionStart = 0;  //This keeps the text
            this.txtUrl.SelectionLength = 0; //from being highlighted
            this.txtUrl.ForeColor = Color.Gray;
            this.txtUrl.Text = "http://" + frmWebBrowser.ESCRIBA_AQUI;

            archivos = new Archivos.Texto(frmHistorial.ARCHIVO_HISTORIAL);
        }

        #region "Escriba aquí..."
        private void txtUrl_MouseMove(object sender, MouseEventArgs e)
        {
            Cursor.Current = Cursors.IBeam; //Without this the mouse pointer shows busy
        }

        private void txtUrl_KeyDown(object sender, KeyEventArgs e)
        {
            if (this.txtUrl.Text.Equals(frmWebBrowser.ESCRIBA_AQUI) == true)
            {
                this.txtUrl.Text = "";
                this.txtUrl.ForeColor = Color.Black;
            }
        }

        private void txtUrl_KeyUp(object sender, KeyEventArgs e)
        {
            if (this.txtUrl.Text.Equals(null) == true || this.txtUrl.Text.Equals("") == true)
            {
                this.txtUrl.Text = frmWebBrowser.ESCRIBA_AQUI;
                this.txtUrl.ForeColor = Color.Gray;
            }
        }

        private void txtUrl_MouseDown(object sender, MouseEventArgs e)
        {
            this.txtUrl.SelectAll();
        }
        #endregion

        delegate void ProgresoDescargaCallback(int progreso);
        /// <summary>
        /// Funcion Privada que muestra el prograso de la descarga.
        /// </summary>
        /// <param name="progreso">Entero que representa el progreso</param>
        private void ProgresoDescarga(int progreso)
        {
            if (statusStrip.InvokeRequired)
            {
                ProgresoDescargaCallback d = new ProgresoDescargaCallback(ProgresoDescarga);
                this.Invoke(d, new object[] { progreso });
            }
            else
            {
                tspbProgreso.Value = progreso;
            }
        }
        delegate void FinDescargaCallback(string html);
        /// <summary>
        /// Funcion privada que carga el String html una vez que finalizo la descarga.
        /// </summary>
        /// <param name="html">String que guarda lo descargado de html</param>
        private void FinDescarga(string html)
        {
            if (rtxtHtmlCode.InvokeRequired)
            {
                FinDescargaCallback d = new FinDescargaCallback(FinDescarga);
                this.Invoke(d, new object[] { html });
            }
            else
            {
                rtxtHtmlCode.Text = html;
            }
        }
        /// <summary>
        /// Funcion privada que muestra el historial de paginas visitadas en el formulario historial.
        /// </summary>
        /// <param name="sender">objeto de tipo Object</param>
        /// <param name="e">objeto de tipo EventsArgs</param>
      
        /// <summary>
        /// Funcion privada que al apretar el boton descarga la url pasada en el textbox
        /// </summary>
        /// <param name="sender">objeto de tipo Object</param>
        /// <param name="e">objeto de tipo EventsArgs</param>
       

        private void btnIr_Click_1(object sender, EventArgs e)
        {
            try
            {

                Uri link;
                if (!(Uri.TryCreate(this.txtUrl.Text, UriKind.Absolute, out link)))
                {
                    txtUrl.Text = "http://" + txtUrl.Text;
                    link = new Uri(txtUrl.Text);
                }
                Descargador descargador = new Descargador(link);
                descargador.progresoDescarga += this.ProgresoDescarga;
                descargador.finDescarga += this.FinDescarga;

                Thread hilo = new Thread(descargador.IniciarDescarga);
                hilo.Start();

                try
                {
                    archivos.guardar(this.txtUrl.Text);
                }
                catch (Exception exc)
                {
                    MessageBox.Show(exc.Message);
                }


            }
            catch (Exception exc)
            {
                this.rtxtHtmlCode.Text = exc.Message;

            }

        }

        /// <summary>
        /// Funcion privada que muestra el historial de paginas visitadas en el formulario historial.
        /// </summary>
        /// <param name="sender">objeto de tipo Object</param>
        /// <param name="e">objeto de tipo EventsArgs</param>
      
        private void mostrarTodoElHistorialToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            frmHistorial formHistorial = new frmHistorial();
            formHistorial.ShowDialog();

        }

        private void rtxtHtmlCode_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtUrl_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
