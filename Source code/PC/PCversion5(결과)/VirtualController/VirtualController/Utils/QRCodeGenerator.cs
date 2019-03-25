using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;

using BusinessRefinery.Barcode;

namespace VirtualController.Utils
{
    public static class QRCodeGenerator
    {
        public static Bitmap generate(string code, QRCodeECL ECL)
        {
            try
            {
                QRCode QR = new QRCode();
                QR.Code = Utils.ByteConverter.stringToHex(code.Trim());

                int version, bytes = Utils.ByteConverter.EUCKR.GetByteCount(QR.Code);
                double ECLFactor;

                switch (ECL)
                {
                    case QRCodeECL.H: ECLFactor = 1.036250; break;
                    case QRCodeECL.Q: ECLFactor = 1.361495; break;
                    case QRCodeECL.M: ECLFactor = 1.902695; break;
                    case QRCodeECL.L: ECLFactor = 2.431290; break;
                    default: ECL = QRCodeECL.M; ECLFactor = 1.902695; break;
                }

                if ((bytes / ECLFactor) >= 10)
                {
                    version = (int)Math.Ceiling(Math.Sqrt(bytes / ECLFactor) - 2);
                    if (version >= 1)
                    {
                        if (version >= 40)
                            version = 40;
                    }
                    else version = 1;
                }
                else version = 1;

                QR.ECL = ECL;
                QR.ModuleSize = 4.0F;
                QR.Version = (QRCodeVersion)version;
                QR.DataMode = QRCodeDataMode.AlphaNumeric;
                QR.BottomMargin = QR.LeftMargin = QR.RightMargin = QR.TopMargin = 10;

                Bitmap image = QR.drawBarcodeOnBitmap();
                for (int y = 0; y < image.Size.Height; y++)
                {
                    for (int x = 0; x < image.Size.Width; x++)
                    {
                        if (image.GetPixel(x, y).R == 255 && image.GetPixel(x, y).G == 0 && image.GetPixel(x, y).B == 0)
                            return generate(code, ECL);
                    }
                }

                return image;
            }
            catch
            {
                return null;
            }
        }
    }
}
