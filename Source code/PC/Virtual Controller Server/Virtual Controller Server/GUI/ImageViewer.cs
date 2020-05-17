using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VirtualControllerServer.GUI
{
    public partial class ImageViewer : Form
    {
        private Bitmap QR;

        public ImageViewer(Bitmap QR)
        {
            InitializeComponent();

            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.ControlBox = false;
            this.QR = QR;
        }

        private void QRCode_Load(object sender, EventArgs e)
        {
            if (QR != null)
            {
                picboxQR.Size = QR.Size;
                picboxQR.Image = QR;

                this.Size = new Size(QR.Size.Width + 40, QR.Size.Height + 62);
            }
        }
    }
}
