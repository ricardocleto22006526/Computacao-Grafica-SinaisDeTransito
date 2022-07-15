using System;
using System.Collections.Generic;
using System.Text;
using Emgu.CV.Structure;
using Emgu.CV;
using System.Linq;
using System.IO;
using System.Collections;
using System.Drawing;

namespace CG_OpenCV
{
    class ImageClass
    {

        /// <summary>
        /// função que metem as cores de cada pixel negativas
        /// </summary>
        /// <param name="img">parametro que ve a imagem</param>        
        public static void Negative(Image<Bgr, byte> img)
        {
            unsafe
            {
                MIplImage m = img.MIplImage;
                byte* dataPtr = (byte*)m.imageData.ToPointer(); // Pointer to the image


                int step = m.widthStep;
                int width = img.Width;
                int height = img.Height;
                int nChan = m.nChannels; // number of channels - 3
                int padding = m.widthStep - m.nChannels * m.width; // alinhament bytes (padding)
                int x, y;



                for (y = 0; y < height; y++)
                {
                    for (x = 0; x < width; x++)
                    {


                        (dataPtr + nChan * x + step * y)[0] = (byte)(255 - (dataPtr + nChan * x + step * y)[0]);
                        (dataPtr + nChan * x + step * y)[1] = (byte)(255 - (dataPtr + nChan * x + step * y)[1]);
                        (dataPtr + nChan * x + step * y)[2] = (byte)(255 - (dataPtr + nChan * x + step * y)[2]);



                    }
                }
            }
        }


        /// <summary>
        /// muda as cores para cinzento
        /// </summary>
        /// <param name="img">parametro que ve a imagem</param>
        public static void ConvertToGray(Image<Bgr, byte> img)
        {
            unsafe
            {
                // direct access to the image memory(sequencial)
                // direcion top left -> bottom right

                MIplImage m = img.MIplImage;
                byte* dataPtr = (byte*)m.imageData.ToPointer(); // Pointer to the image
                byte gray;

                int step = m.widthStep;
                int width = img.Width;
                int height = img.Height;
                int nChan = m.nChannels; // number of channels - 3
                int padding = m.widthStep - m.nChannels * m.width; // alinhament bytes (padding)
                int x, y;

                if (nChan == 3) // image in RGB
                {
                    for (y = 0; y < height; y++)
                    {
                        for (x = 0; x < width; x++)
                        {
                            //retrive 3 colour components
                            //blue = dataPtr[0];
                            //green = dataPtr[1];
                            //red = dataPtr[2];

                            // convert to gray
                            // gray = (byte)Math.Round(((int)blue + green + red) / 3.0);

                            gray = (byte)Math.Round(((dataPtr + nChan * x + step * y)[0] +
                                (dataPtr + nChan * x + step * y)[1] +
                                (dataPtr + nChan * x + step * y)[2]) / 3.0);


                            (dataPtr + nChan * x + step * y)[0] = gray;
                            (dataPtr + nChan * x + step * y)[1] = gray;
                            (dataPtr + nChan * x + step * y)[2] = gray;




                            // store in the image
                            //dataPtr[0] = gray;
                            //dataPtr[1] = gray;    
                            //dataPtr[2] = gray;

                            // advance the pointer to the next pixel
                            // dataPtr += nChan;
                        }

                        //at the end of the line advance the pointer by the aligment bytes (padding)
                        // dataPtr += padding;
                    }
                }
            }
        }

        /// <summary>
        /// trocas os valores da cor azule  verda para o valo vermelho dando assim um cinzento com puxada vermelha
        /// </summary>
        /// <param name="img"></param>
        public static void RedChannel(Image<Bgr, byte> img)
        {
            unsafe
            {
                // direct access to the image memory(sequencial)
                // direcion top left -> bottom right

                MIplImage m = img.MIplImage;
                byte* dataPtr = (byte*)m.imageData.ToPointer(); // Pointer to the image
                byte red;

                int step = m.widthStep;
                int width = img.Width;
                int height = img.Height;
                int nChan = m.nChannels; // number of channels - 3
                int padding = m.widthStep - m.nChannels * m.width; // alinhament bytes (padding)
                int x, y;

                if (nChan == 3) // image in RGB
                {
                    for (y = 0; y < height; y++)
                    {
                        for (x = 0; x < width; x++)
                        {


                            red = (byte)(dataPtr + nChan * x + step * y)[2];



                            (dataPtr + nChan * x + step * y)[0] = red;
                            (dataPtr + nChan * x + step * y)[1] = red;


                        }


                    }
                }
            }
        }

        /// <summary>
        /// função muda a imagem para o bilho e o contraste passados como parametros
        /// </summary>
        /// <param name="img"> parametro da imagem</param>
        /// <param name="bright">parametro do brilho</param>
        /// <param name="contrast">parametro do contraste</param>
        public static void BrightContrast(Image<Bgr, byte> img, int bright, double contrast)
        {

            unsafe
            {
                // direct access to the image memory(sequencial)
                // direcion top left -> bottom right

                MIplImage m = img.MIplImage;
                byte* dataPtr = (byte*)m.imageData.ToPointer(); // Pointer to the image

                int step = m.widthStep;
                int width = img.Width;
                int height = img.Height;
                int nChan = m.nChannels; // number of channels - 3
                int padding = m.widthStep - m.nChannels * m.width; // alinhament bytes (padding)
                int x, y;
                int valor;



                if (nChan == 3) // image in RGB
                {
                    for (y = 0; y < height; y++)
                    {
                        for (x = 0; x < width; x++)
                        {

                            valor = (int)Math.Round(contrast * (dataPtr + nChan * x + step * y)[0] + bright);


                            if (valor > 255)
                            {
                                (dataPtr + nChan * x + step * y)[0] = (byte)255;


                            }
                            else if (valor < 0)
                            {
                                (dataPtr + nChan * x + step * y)[0] = (byte)0;

                            }
                            else
                            {
                                (dataPtr + nChan * x + step * y)[0] = (byte)valor;

                            }

                            valor = (int)Math.Round(contrast * (dataPtr + nChan * x + step * y)[1] + bright);

                            if (valor > 255)
                            {
                                (dataPtr + nChan * x + step * y)[1] = (byte)255;


                            }
                            else if (valor < 0)
                            {
                                (dataPtr + nChan * x + step * y)[1] = (byte)0;

                            }
                            else
                            {
                                (dataPtr + nChan * x + step * y)[1] = (byte)valor;

                            }


                            valor = (int)Math.Round(contrast * (dataPtr + nChan * x + step * y)[2] + bright);

                            if (valor > 255)
                            {
                                (dataPtr + nChan * x + step * y)[2] = (byte)255;


                            }
                            else if (valor < 0)
                            {
                                (dataPtr + nChan * x + step * y)[2] = (byte)0;

                            }
                            else
                            {
                                (dataPtr + nChan * x + step * y)[2] = (byte)valor;

                            }





                        }


                    }
                }
            }
        }

        public static void Translation(Image<Bgr, byte> img, Image<Bgr, byte> imgCopy, int dx, int dy)
        {

            unsafe
            {
                // direct access to the image memory(sequencial)
                // direcion top left -> bottom right

                MIplImage m = img.MIplImage;
                MIplImage mcopy = imgCopy.MIplImage;
                byte* dataPtr = (byte*)m.imageData.ToPointer(); // Pointer to the image
                byte* dataPtrcopy = (byte*)mcopy.imageData.ToPointer(); // Pointer to the imagecopy
                byte blue, green, red;

                int step = m.widthStep;
                int stepcopy = mcopy.widthStep;
                int width = img.Width;
                int height = img.Height;
                int nChan = m.nChannels; // number of channels - 3
                int padding = m.widthStep - m.nChannels * m.width; // alinhament bytes (padding)
                int xo = 0, yo = 0;
                int x, y;

                if (nChan == 3) // image in RGB
                {
                    for (y = 0; y < height; y++)
                    {
                        for (x = 0; x < width; x++)
                        {

                            yo = y - dy;
                            xo = x - dx;

                            if (yo < 0 || yo >= height || xo < 0 || xo >= width) // pixel invalido
                            {
                                blue = red = green = 0;
                            }
                            else
                            {
                                blue = (dataPtrcopy + nChan * xo + stepcopy * yo)[0];
                                green = (dataPtrcopy + nChan * xo + stepcopy * yo)[1];
                                red = (dataPtrcopy + nChan * xo + stepcopy * yo)[2];
                            }

                            (dataPtr + nChan * x + step * y)[0] = blue;
                            (dataPtr + nChan * x + step * y)[1] = green;
                            (dataPtr + nChan * x + step * y)[2] = red;


                        }


                    }
                }
            }




        }

        public static void Rotation(Image<Bgr, byte> img, Image<Bgr, byte> imgCopy, float angle)
        {

            unsafe
            {
                // direct access to the image memory(sequencial)
                // direcion top left -> bottom right

                MIplImage m = img.MIplImage;
                MIplImage mcopy = imgCopy.MIplImage;
                byte* dataPtr = (byte*)m.imageData.ToPointer(); // Pointer to the image
                byte* dataPtrcopy = (byte*)mcopy.imageData.ToPointer(); // Pointer to the imagecopy
                byte blue, green, red;

                int step = m.widthStep;
                int stepcopy = mcopy.widthStep;
                int width = img.Width;
                int height = img.Height;
                int nChan = m.nChannels; // number of channels - 3
                int padding = m.widthStep - m.nChannels * m.width; // alinhament bytes (padding)
                int xo = 0, yo = 0;
                int x, y;

                if (nChan == 3) // image in RGB
                {
                    for (y = 0; y < height; y++)
                    {
                        for (x = 0; x < width; x++)
                        {

                            xo = (int)Math.Round((x - width / 2.0) * Math.Cos(angle) - (height / 2.0 - y) * Math.Sin(angle) + width / 2.0);
                            yo = (int)Math.Round(height / 2.0 - (x - width / 2.0) * Math.Sin(angle) - (height / 2.0 - y) * Math.Cos(angle));


                            if (yo < 0 || yo >= height || xo < 0 || xo >= width) // pixel invalido
                            {
                                blue = red = green = 0;
                            }
                            else
                            {
                                blue = (dataPtrcopy + nChan * xo + stepcopy * yo)[0];
                                green = (dataPtrcopy + nChan * xo + stepcopy * yo)[1];
                                red = (dataPtrcopy + nChan * xo + stepcopy * yo)[2];
                            }

                            (dataPtr + nChan * x + step * y)[0] = blue;
                            (dataPtr + nChan * x + step * y)[1] = green;
                            (dataPtr + nChan * x + step * y)[2] = red;


                        }


                    }
                }
            }


        }

        public static void Scale(Image<Bgr, byte> img, Image<Bgr, byte> imgCopy, float scaleFactor)
        {
            unsafe
            {
                // direct access to the image memory(sequencial)
                // direcion top left -> bottom right

                MIplImage m = img.MIplImage;
                MIplImage mcopy = imgCopy.MIplImage;
                byte* dataPtr = (byte*)m.imageData.ToPointer(); // Pointer to the image
                byte* dataPtrcopy = (byte*)mcopy.imageData.ToPointer(); // Pointer to the imagecopy
                byte blue, green, red;

                int step = m.widthStep;
                int stepcopy = mcopy.widthStep;
                int width = img.Width;
                int height = img.Height;
                int nChan = m.nChannels; // number of channels - 3
                int padding = m.widthStep - m.nChannels * m.width; // alinhament bytes (padding)
                int xo = 0, yo = 0;
                int x, y;

                if (nChan == 3) // image in RGB
                {
                    for (y = 0; y < height; y++)
                    {
                        for (x = 0; x < width; x++)
                        {
                            xo = (int)Math.Round((x - width / 2.0) / scaleFactor + width);
                            yo = (int)Math.Round((y - height / 2.0) / scaleFactor + height);


                            if (yo < 0 || yo >= height || xo < 0 || xo >= width) // pixel invalido
                            {
                                blue = red = green = 0;
                            }
                            else
                            {
                                blue = (dataPtrcopy + nChan * xo + stepcopy * yo)[0];
                                green = (dataPtrcopy + nChan * xo + stepcopy * yo)[1];
                                red = (dataPtrcopy + nChan * xo + stepcopy * yo)[2];
                            }

                            (dataPtr + nChan * x + step * y)[0] = blue;
                            (dataPtr + nChan * x + step * y)[1] = green;
                            (dataPtr + nChan * x + step * y)[2] = red;


                        }


                    }
                }
            }

        }

        public static void Scale_point_xy(Image<Bgr, byte> img, Image<Bgr, byte> imgCopy, float scaleFactor, int centerX, int centerY)
        {
            unsafe
            {
                // direct access to the image memory(sequencial)
                // direcion top left -> bottom right

                MIplImage m = img.MIplImage;
                MIplImage mcopy = imgCopy.MIplImage;
                byte* dataPtr = (byte*)m.imageData.ToPointer(); // Pointer to the image
                byte* dataPtrcopy = (byte*)mcopy.imageData.ToPointer(); // Pointer to the imagecopy
                byte blue, green, red;

                int step = m.widthStep;
                int stepcopy = mcopy.widthStep;
                int width = img.Width;
                int height = img.Height;
                int nChan = m.nChannels; // number of channels - 3
                int padding = m.widthStep - m.nChannels * m.width; // alinhament bytes (padding)
                int xo = 0, yo = 0;
                int x, y;

                if (nChan == 3) // image in RGB
                {
                    for (y = 0; y < height; y++)
                    {
                        for (x = 0; x < width; x++)
                        {
                            xo = (int)Math.Round((x - width / 2) / scaleFactor + centerX);
                            yo = (int)Math.Round((y - height / 2) / scaleFactor + centerY);


                            if (yo < 0 || yo >= height || xo < 0 || xo >= width) // pixel invalido
                            {
                                blue = red = green = 0;
                            }
                            else
                            {
                                blue = (dataPtrcopy + nChan * xo + stepcopy * yo)[0];
                                green = (dataPtrcopy + nChan * xo + stepcopy * yo)[1];
                                red = (dataPtrcopy + nChan * xo + stepcopy * yo)[2];
                            }

                            (dataPtr + nChan * x + step * y)[0] = blue;
                            (dataPtr + nChan * x + step * y)[1] = green;
                            (dataPtr + nChan * x + step * y)[2] = red;


                        }


                    }
                }
            }

        }

        public static void Mean(Image<Bgr, byte> img, Image<Bgr, byte> imgCopy)
        {
            unsafe
            {
                // direct access to the image memory(sequencial)
                // direcion top left -> bottom right

                MIplImage m = img.MIplImage;
                MIplImage mcopy = imgCopy.MIplImage;
                byte* dataPtr = (byte*)m.imageData.ToPointer(); // Pointer to the image
                byte* dataPtrcopy = (byte*)mcopy.imageData.ToPointer(); // Pointer to the imagecopy
                byte blue, green, red;

                int step = m.widthStep;
                int stepcopy = mcopy.widthStep;
                int width = img.Width;
                int height = img.Height;
                int nChan = m.nChannels; // number of channels - 3
                int padding = m.widthStep - m.nChannels * m.width; // alinhament bytes (padding)
                int x, y;

                if (nChan == 3) // image in RGB
                {       // parte de dentro da imagem
                    for (y = 1; y < height - 1; y++)
                    {
                        for (x = 1; x < width - 1; x++)
                        {

                            blue = (byte)Math.Round(((dataPtrcopy + nChan * (x - 1) + stepcopy * (y - 1))[0] + (dataPtrcopy + nChan * x + stepcopy * (y - 1))[0] + (dataPtrcopy + nChan * (x + 1) + stepcopy * (y - 1))[0] + (dataPtrcopy + nChan * (x - 1) + stepcopy * y)[0] + (dataPtrcopy + nChan * x + stepcopy * y)[0] + (dataPtrcopy + nChan * (x + 1) + stepcopy * y)[0] + (dataPtrcopy + nChan * (x - 1) + stepcopy * (y + 1))[0] + (dataPtrcopy + nChan * x + stepcopy * (y + 1))[0] + (dataPtrcopy + nChan * (x + 1) + stepcopy * (y + 1))[0]) / 9.0);

                            green = (byte)Math.Round(((dataPtrcopy + nChan * (x - 1) + stepcopy * (y - 1))[1] + (dataPtrcopy + nChan * x + stepcopy * (y - 1))[1] + (dataPtrcopy + nChan * (x + 1) + stepcopy * (y - 1))[1] + (dataPtrcopy + nChan * (x - 1) + stepcopy * y)[1] + (dataPtrcopy + nChan * x + stepcopy * y)[1] + (dataPtrcopy + nChan * (x + 1) + stepcopy * y)[1] + (dataPtrcopy + nChan * (x - 1) + stepcopy * (y + 1))[1] + (dataPtrcopy + nChan * x + stepcopy * (y + 1))[1] + (dataPtrcopy + nChan * (x + 1) + stepcopy * (y + 1))[1]) / 9.0);

                            red = (byte)Math.Round(((dataPtrcopy + nChan * (x - 1) + stepcopy * (y - 1))[2] + (dataPtrcopy + nChan * x + stepcopy * (y - 1))[2] + (dataPtrcopy + nChan * (x + 1) + stepcopy * (y - 1))[2] + (dataPtrcopy + nChan * (x - 1) + stepcopy * y)[2] + (dataPtrcopy + nChan * x + stepcopy * y)[2] + (dataPtrcopy + nChan * (x + 1) + stepcopy * y)[2] + (dataPtrcopy + nChan * (x - 1) + stepcopy * (y + 1))[2] + (dataPtrcopy + nChan * x + stepcopy * (y + 1))[2] + (dataPtrcopy + nChan * (x + 1) + stepcopy * (y + 1))[2]) / 9.0);


                            (dataPtr + nChan * x + step * y)[0] = blue;
                            (dataPtr + nChan * x + step * y)[1] = green;
                            (dataPtr + nChan * x + step * y)[2] = red;

                        }
                    }
                    for (x = 1; x < width - 1; x++)
                    {
                        y = 0;
                        blue = (byte)Math.Round(((dataPtrcopy + nChan * (x - 1) + stepcopy * y)[0] + (dataPtrcopy + nChan * x + stepcopy * y)[0] + (dataPtrcopy + nChan * (x + 1) + stepcopy * y)[0] + (dataPtrcopy + nChan * (x - 1) + stepcopy * y)[0] + (dataPtrcopy + nChan * x + stepcopy * y)[0] + (dataPtrcopy + nChan * (x + 1) + stepcopy * y)[0] + (dataPtrcopy + nChan * (x - 1) + stepcopy * (y + 1))[0] + (dataPtrcopy + nChan * x + stepcopy * (y + 1))[0] + (dataPtrcopy + nChan * (x + 1) + stepcopy * (y + 1))[0]) / 9.0);

                        green = (byte)Math.Round(((dataPtrcopy + nChan * (x - 1) + stepcopy * y)[1] + (dataPtrcopy + nChan * x + stepcopy * y)[1] + (dataPtrcopy + nChan * (x + 1) + stepcopy * y)[1] + (dataPtrcopy + nChan * (x - 1) + stepcopy * y)[1] + (dataPtrcopy + nChan * x + stepcopy * y)[1] + (dataPtrcopy + nChan * (x + 1) + stepcopy * y)[1] + (dataPtrcopy + nChan * (x - 1) + stepcopy * (y + 1))[1] + (dataPtrcopy + nChan * x + stepcopy * (y + 1))[1] + (dataPtrcopy + nChan * (x + 1) + stepcopy * (y + 1))[1]) / 9.0);

                        red = (byte)Math.Round(((dataPtrcopy + nChan * (x - 1) + stepcopy * y)[2] + (dataPtrcopy + nChan * x + stepcopy * y)[2] + (dataPtrcopy + nChan * (x + 1) + stepcopy * y)[2] + (dataPtrcopy + nChan * (x - 1) + stepcopy * y)[2] + (dataPtrcopy + nChan * x + stepcopy * y)[2] + (dataPtrcopy + nChan * (x + 1) + stepcopy * y)[2] + (dataPtrcopy + nChan * (x - 1) + stepcopy * (y + 1))[2] + (dataPtrcopy + nChan * x + stepcopy * (y + 1))[2] + (dataPtrcopy + nChan * (x + 1) + stepcopy * (y + 1))[2]) / 9.0);



                        (dataPtr + nChan * x + step * 0)[0] = blue;
                        (dataPtr + nChan * x + step * 0)[1] = green;
                        (dataPtr + nChan * x + step * 0)[2] = red;
                        y = height - 1;

                        blue = (byte)Math.Round(((dataPtrcopy + nChan * (x - 1) + stepcopy * (y - 1))[0] + (dataPtrcopy + nChan * x + stepcopy * (y - 1))[0] + (dataPtrcopy + nChan * (x + 1) + stepcopy * (y - 1))[0] + (dataPtrcopy + nChan * (x - 1) + stepcopy * y)[0] + (dataPtrcopy + nChan * x + stepcopy * y)[0] + (dataPtrcopy + nChan * (x + 1) + stepcopy * y)[0] + (dataPtrcopy + nChan * (x - 1) + stepcopy * y)[0] + (dataPtrcopy + nChan * x + stepcopy * y)[0] + (dataPtrcopy + nChan * (x + 1) + stepcopy * y)[0]) / 9.0);

                        green = (byte)Math.Round(((dataPtrcopy + nChan * (x - 1) + stepcopy * (y - 1))[1] + (dataPtrcopy + nChan * x + stepcopy * (y - 1))[1] + (dataPtrcopy + nChan * (x + 1) + stepcopy * (y - 1))[1] + (dataPtrcopy + nChan * (x - 1) + stepcopy * y)[1] + (dataPtrcopy + nChan * x + stepcopy * y)[1] + (dataPtrcopy + nChan * (x + 1) + stepcopy * y)[1] + (dataPtrcopy + nChan * (x - 1) + stepcopy * y)[1] + (dataPtrcopy + nChan * x + stepcopy * y)[1] + (dataPtrcopy + nChan * (x + 1) + stepcopy * y)[1]) / 9.0);

                        red = (byte)Math.Round(((dataPtrcopy + nChan * (x - 1) + stepcopy * (y - 1))[2] + (dataPtrcopy + nChan * x + stepcopy * (y - 1))[2] + (dataPtrcopy + nChan * (x + 1) + stepcopy * (y - 1))[2] + (dataPtrcopy + nChan * (x - 1) + stepcopy * y)[2] + (dataPtrcopy + nChan * x + stepcopy * y)[2] + (dataPtrcopy + nChan * (x + 1) + stepcopy * y)[2] + (dataPtrcopy + nChan * (x - 1) + stepcopy * y)[2] + (dataPtrcopy + nChan * x + stepcopy * y)[2] + (dataPtrcopy + nChan * (x + 1) + stepcopy * y)[2]) / 9.0);



                        (dataPtr + nChan * x + step * (height - 1))[0] = blue;
                        (dataPtr + nChan * x + step * (height - 1))[1] = green;
                        (dataPtr + nChan * x + step * (height - 1))[2] = red;

                    }
                    for (y = 1; y < height - 1; y++)
                    {
                        x = 0;

                        blue = (byte)Math.Round(((dataPtrcopy + nChan * x + stepcopy * (y - 1))[0] + (dataPtrcopy + nChan * x + stepcopy * (y - 1))[0] + (dataPtrcopy + nChan * (x + 1) + stepcopy * (y - 1))[0] + (dataPtrcopy + nChan * x + stepcopy * y)[0] + (dataPtrcopy + nChan * x + stepcopy * y)[0] + (dataPtrcopy + nChan * (x + 1) + stepcopy * y)[0] + (dataPtrcopy + nChan * x + stepcopy * (y + 1))[0] + (dataPtrcopy + nChan * x + stepcopy * (y + 1))[0] + (dataPtrcopy + nChan * (x + 1) + stepcopy * (y + 1))[0]) / 9.0);

                        green = (byte)Math.Round(((dataPtrcopy + nChan * x + stepcopy * (y - 1))[1] + (dataPtrcopy + nChan * x + stepcopy * (y - 1))[1] + (dataPtrcopy + nChan * (x + 1) + stepcopy * (y - 1))[1] + (dataPtrcopy + nChan * x + stepcopy * y)[1] + (dataPtrcopy + nChan * x + stepcopy * y)[1] + (dataPtrcopy + nChan * (x + 1) + stepcopy * y)[1] + (dataPtrcopy + nChan * x + stepcopy * (y + 1))[1] + (dataPtrcopy + nChan * x + stepcopy * (y + 1))[1] + (dataPtrcopy + nChan * (x + 1) + stepcopy * (y + 1))[1]) / 9.0);

                        red = (byte)Math.Round(((dataPtrcopy + nChan * x + stepcopy * (y - 1))[2] + (dataPtrcopy + nChan * x + stepcopy * (y - 1))[2] + (dataPtrcopy + nChan * (x + 1) + stepcopy * (y - 1))[2] + (dataPtrcopy + nChan * x + stepcopy * y)[2] + (dataPtrcopy + nChan * x + stepcopy * y)[2] + (dataPtrcopy + nChan * (x + 1) + stepcopy * y)[2] + (dataPtrcopy + nChan * x + stepcopy * (y + 1))[2] + (dataPtrcopy + nChan * x + stepcopy * (y + 1))[2] + (dataPtrcopy + nChan * (x + 1) + stepcopy * (y + 1))[2]) / 9.0);


                        (dataPtr + nChan * 0 + step * y)[0] = blue;
                        (dataPtr + nChan * 0 + step * y)[1] = green;
                        (dataPtr + nChan * 0 + step * y)[2] = red;

                        x = width - 1;

                        blue = (byte)Math.Round(((dataPtrcopy + nChan * (x - 1) + step * (y - 1))[0] + (dataPtrcopy + nChan * (x - 1) + step * (y + 1))[0] + (dataPtrcopy + nChan * (x - 1) + step * (y))[0] + (dataPtrcopy + nChan * (x) + step * (y - 1))[0] + (dataPtrcopy + nChan * (x) + step * (y))[0] + (dataPtrcopy + nChan * (x) + step * (y + 1))[0] + (dataPtrcopy + nChan * (x) + step * (y - 1))[0] + (dataPtrcopy + nChan * (x) + step * (y + 1))[0] + (dataPtrcopy + nChan * (x) + step * (y))[0]) / 9.0);

                        green = (byte)Math.Round(((dataPtrcopy + nChan * (x - 1) + step * (y - 1))[1] + (dataPtrcopy + nChan * (x - 1) + step * (y + 1))[1] + (dataPtrcopy + nChan * (x - 1) + step * (y))[1] + (dataPtrcopy + nChan * (x) + step * (y - 1))[1] + (dataPtrcopy + nChan * (x) + step * (y))[1] + (dataPtrcopy + nChan * (x) + step * (y + 1))[1] + (dataPtrcopy + nChan * (x) + step * (y - 1))[1] + (dataPtrcopy + nChan * (x) + step * (y + 1))[1] + (dataPtrcopy + nChan * (x) + step * (y))[1]) / 9.0);

                        red = (byte)Math.Round(((dataPtrcopy + nChan * (x - 1) + step * (y - 1))[2] + (dataPtrcopy + nChan * (x - 1) + step * (y + 1))[2] + (dataPtrcopy + nChan * (x - 1) + step * (y))[2] + (dataPtrcopy + nChan * (x) + step * (y - 1))[2] + (dataPtrcopy + nChan * (x) + step * (y))[2] + (dataPtrcopy + nChan * (x) + step * (y + 1))[2] + (dataPtrcopy + nChan * (x) + step * (y - 1))[2] + (dataPtrcopy + nChan * (x) + step * (y + 1))[2] + (dataPtrcopy + nChan * (x) + step * (y))[2]) / 9.0);


                        (dataPtr + nChan * (width - 1) + step * y)[0] = blue;
                        (dataPtr + nChan * (width - 1) + step * y)[1] = green;
                        (dataPtr + nChan * (width - 1) + step * y)[2] = red;
                    }

                    // canto superior esquerdo
                    x = 0;
                    y = 0;

                    blue = (byte)Math.Round(((dataPtrcopy + nChan * x + stepcopy * y)[0] + (dataPtrcopy + nChan * x + stepcopy * y)[0] + (dataPtrcopy + nChan * (x + 1) + stepcopy * y)[0] + (dataPtrcopy + nChan * x + stepcopy * y)[0] + (dataPtrcopy + nChan * x + stepcopy * y)[0] + (dataPtrcopy + nChan * (x + 1) + stepcopy * y)[0] + (dataPtrcopy + nChan * x + stepcopy * (y + 1))[0] + (dataPtrcopy + nChan * x + stepcopy * (y + 1))[0] + (dataPtrcopy + nChan * (x + 1) + stepcopy * (y + 1))[0]) / 9.0);

                    green = (byte)Math.Round(((dataPtrcopy + nChan * x + stepcopy * y)[1] + (dataPtrcopy + nChan * x + stepcopy * y)[1] + (dataPtrcopy + nChan * (x + 1) + stepcopy * y)[1] + (dataPtrcopy + nChan * x + stepcopy * y)[1] + (dataPtrcopy + nChan * x + stepcopy * y)[1] + (dataPtrcopy + nChan * (x + 1) + stepcopy * y)[1] + (dataPtrcopy + nChan * x + stepcopy * (y + 1))[1] + (dataPtrcopy + nChan * x + stepcopy * (y + 1))[1] + (dataPtrcopy + nChan * (x + 1) + stepcopy * (y + 1))[1]) / 9.0);

                    red = (byte)Math.Round(((dataPtrcopy + nChan * x + stepcopy * y)[2] + (dataPtrcopy + nChan * x + stepcopy * y)[2] + (dataPtrcopy + nChan * (x + 1) + stepcopy * y)[2] + (dataPtrcopy + nChan * x + stepcopy * y)[2] + (dataPtrcopy + nChan * x + stepcopy * y)[2] + (dataPtrcopy + nChan * (x + 1) + stepcopy * y)[2] + (dataPtrcopy + nChan * x + stepcopy * (y + 1))[2] + (dataPtrcopy + nChan * x + stepcopy * (y + 1))[2] + (dataPtrcopy + nChan * (x + 1) + stepcopy * (y + 1))[2]) / 9.0);


                    (dataPtr + nChan * x + step * y)[0] = blue;
                    (dataPtr + nChan * x + step * y)[1] = green;
                    (dataPtr + nChan * x + step * y)[2] = red;

                    // canto superior direito

                    x = width - 1; y = 0;

                    blue = (byte)Math.Round(((dataPtrcopy + nChan * (x - 1) + stepcopy * y)[0] + (dataPtrcopy + nChan * x + stepcopy * y)[0] + (dataPtrcopy + nChan * x + stepcopy * y)[0] + (dataPtrcopy + nChan * (x - 1) + stepcopy * y)[0] + (dataPtrcopy + nChan * x + stepcopy * y)[0] + (dataPtrcopy + nChan * x + stepcopy * y)[0] + (dataPtrcopy + nChan * (x - 1) + stepcopy * (y + 1))[0] + (dataPtrcopy + nChan * x + stepcopy * (y + 1))[0] + (dataPtrcopy + nChan * x + stepcopy * (y + 1))[0]) / 9.0);

                    green = (byte)Math.Round(((dataPtrcopy + nChan * (x - 1) + stepcopy * y)[1] + (dataPtrcopy + nChan * x + stepcopy * y)[1] + (dataPtrcopy + nChan * x + stepcopy * y)[1] + (dataPtrcopy + nChan * (x - 1) + stepcopy * y)[1] + (dataPtrcopy + nChan * x + stepcopy * y)[1] + (dataPtrcopy + nChan * x + stepcopy * y)[1] + (dataPtrcopy + nChan * (x - 1) + stepcopy * (y + 1))[1] + (dataPtrcopy + nChan * x + stepcopy * (y + 1))[1] + (dataPtrcopy + nChan * x + stepcopy * (y + 1))[1]) / 9.0);

                    red = (byte)Math.Round(((dataPtrcopy + nChan * (x - 1) + stepcopy * y)[2] + (dataPtrcopy + nChan * x + stepcopy * y)[2] + (dataPtrcopy + nChan * x + stepcopy * y)[2] + (dataPtrcopy + nChan * (x - 1) + stepcopy * y)[2] + (dataPtrcopy + nChan * x + stepcopy * y)[2] + (dataPtrcopy + nChan * x + stepcopy * y)[2] + (dataPtrcopy + nChan * (x - 1) + stepcopy * (y + 1))[2] + (dataPtrcopy + nChan * x + stepcopy * (y + 1))[2] + (dataPtrcopy + nChan * x + stepcopy * (y + 1))[2]) / 9.0);

                    (dataPtr + nChan * x + step * y)[0] = blue;
                    (dataPtr + nChan * x + step * y)[1] = green;
                    (dataPtr + nChan * x + step * y)[2] = red;

                    // canto inferior esquerdo

                    x = 0; y = height - 1;

                    blue = (byte)Math.Round(((dataPtrcopy + nChan * (x + 1) + step * (y - 1))[0] + (dataPtrcopy + nChan * (x + 1) + step * (y))[0] * 2 + (dataPtrcopy + nChan * (x) + step * (y - 1))[0] * 2 + (dataPtrcopy + nChan * (x) + step * (y))[0] * 4) / 9.0);

                    green = (byte)Math.Round(((dataPtrcopy + nChan * (x + 1) + step * (y - 1))[1] + (dataPtrcopy + nChan * (x + 1) + step * (y))[1] * 2 + (dataPtrcopy + nChan * (x) + step * (y - 1))[1] * 2 + (dataPtrcopy + nChan * (x) + step * (y))[1] * 4) / 9.0);

                    red = (byte)Math.Round(((dataPtrcopy + nChan * (x + 1) + step * (y - 1))[2] + (dataPtrcopy + nChan * (x + 1) + step * (y))[2] * 2 + (dataPtrcopy + nChan * (x) + step * (y - 1))[2] * 2 + (dataPtrcopy + nChan * (x) + step * (y))[2] * 4) / 9.0);


                    (dataPtr + nChan * x + step * y)[0] = blue;
                    (dataPtr + nChan * x + step * y)[1] = green;
                    (dataPtr + nChan * x + step * y)[2] = red;


                    // canto inferior direiro 

                    x = width - 1; y = height - 1;

                    blue = (byte)Math.Round(((dataPtrcopy + nChan * (x - 1) + stepcopy * (y - 1))[0] + (dataPtrcopy + nChan * x + stepcopy * (y - 1))[0] + (dataPtrcopy + nChan * x + stepcopy * (y - 1))[0] + (dataPtrcopy + nChan * (x - 1) + stepcopy * y)[0] + (dataPtrcopy + nChan * x + stepcopy * y)[0] + (dataPtrcopy + nChan * x + stepcopy * y)[0] + (dataPtrcopy + nChan * (x - 1) + stepcopy * y)[0] + (dataPtrcopy + nChan * x + stepcopy * y)[0] + (dataPtrcopy + nChan * x + stepcopy * y)[0]) / 9.0);

                    green = (byte)Math.Round(((dataPtrcopy + nChan * (x - 1) + stepcopy * (y - 1))[1] + (dataPtrcopy + nChan * x + stepcopy * (y - 1))[1] + (dataPtrcopy + nChan * x + stepcopy * (y - 1))[1] + (dataPtrcopy + nChan * (x - 1) + stepcopy * y)[1] + (dataPtrcopy + nChan * x + stepcopy * y)[1] + (dataPtrcopy + nChan * x + stepcopy * y)[1] + (dataPtrcopy + nChan * (x - 1) + stepcopy * y)[1] + (dataPtrcopy + nChan * x + stepcopy * y)[1] + (dataPtrcopy + nChan * x + stepcopy * y)[1]) / 9.0);

                    red = (byte)Math.Round(((dataPtrcopy + nChan * (x - 1) + stepcopy * (y - 1))[2] + (dataPtrcopy + nChan * x + stepcopy * (y - 1))[2] + (dataPtrcopy + nChan * x + stepcopy * (y - 1))[2] + (dataPtrcopy + nChan * (x - 1) + stepcopy * y)[2] + (dataPtrcopy + nChan * x + stepcopy * y)[2] + (dataPtrcopy + nChan * x + stepcopy * y)[2] + (dataPtrcopy + nChan * (x - 1) + stepcopy * y)[2] + (dataPtrcopy + nChan * x + stepcopy * y)[2] + (dataPtrcopy + nChan * x + stepcopy * y)[2]) / 9.0);


                    (dataPtr + nChan * x + step * y)[0] = blue;
                    (dataPtr + nChan * x + step * y)[1] = green;
                    (dataPtr + nChan * x + step * y)[2] = red;


                }
            }

        }

        public static void NonUniform(Image<Bgr, byte> img, Image<Bgr, byte> imgCopy, float[,] matrix, float matrixWeight)
        {
            unsafe
            {
                // direct access to the image memory(sequencial)
                // direcion top left -> bottom right

                MIplImage m = img.MIplImage;
                MIplImage mcopy = imgCopy.MIplImage;
                byte* dataPtr = (byte*)m.imageData.ToPointer(); // Pointer to the image
                byte* dataPtrcopy = (byte*)mcopy.imageData.ToPointer(); // Pointer to the imagecopy
                byte blue, green, red;

                int step = m.widthStep;
                int stepcopy = mcopy.widthStep;
                int width = img.Width;
                int height = img.Height;
                int nChan = m.nChannels; // number of channels - 3
                int padding = m.widthStep - m.nChannels * m.width; // alinhament bytes (padding)
                int x, y;
                int teste = 0;
                int a;

                if (nChan == 3) // image in RGB
                {       // parte de dentro da imagem
                    for (y = 1; y < height - 1; y++)
                    {
                        for (x = 1; x < width - 1; x++)
                        {
                            a = 0;
                            teste = (int)Math.Round((((dataPtrcopy + nChan * (x - 1) + stepcopy * (y - 1))[a] * matrix[0, 0]) + ((dataPtrcopy + nChan * x + stepcopy * (y - 1))[a] * matrix[0, 1]) + ((dataPtrcopy + nChan * (x + 1) + stepcopy * (y - 1))[a] * matrix[0, 2]) + ((dataPtrcopy + nChan * (x - 1) + stepcopy * y)[a] * matrix[1, 0]) + ((dataPtrcopy + nChan * x + stepcopy * y)[a] * matrix[1, 1]) + ((dataPtrcopy + nChan * (x + 1) + stepcopy * y)[a] * matrix[1, 2]) + ((dataPtrcopy + nChan * (x - 1) + stepcopy * (y + 1))[a] * matrix[2, 0]) + ((dataPtrcopy + nChan * x + stepcopy * (y + 1))[a] * matrix[2, 1]) + (((dataPtrcopy + nChan * (x + 1) + stepcopy * (y + 1))[a] * matrix[2, 2]))) / matrixWeight);

                            if (teste < 0)
                            {
                                blue = 0;
                            }
                            if (teste > 255)
                            {
                                blue = 255;
                            }
                            else
                            {
                                blue = (byte)teste;
                            }
                            a = 1;

                            teste = (int)Math.Round((((dataPtrcopy + nChan * (x - 1) + stepcopy * (y - 1))[a] * matrix[0, 0]) + ((dataPtrcopy + nChan * x + stepcopy * (y - 1))[a] * matrix[0, 1]) + ((dataPtrcopy + nChan * (x + 1) + stepcopy * (y - 1))[a] * matrix[0, 2]) + ((dataPtrcopy + nChan * (x - 1) + stepcopy * y)[a] * matrix[1, 0]) + ((dataPtrcopy + nChan * x + stepcopy * y)[a] * matrix[1, 1]) + ((dataPtrcopy + nChan * (x + 1) + stepcopy * y)[a] * matrix[1, 2]) + ((dataPtrcopy + nChan * (x - 1) + stepcopy * (y + 1))[a] * matrix[2, 0]) + ((dataPtrcopy + nChan * x + stepcopy * (y + 1))[a] * matrix[2, 1]) + (((dataPtrcopy + nChan * (x + 1) + stepcopy * (y + 1))[a] * matrix[2, 2]))) / matrixWeight);

                            if (teste < 0)
                            {
                                green = 0;
                            }
                            if (teste > 255)
                            {
                                green = 255;
                            }
                            else
                            {
                                green = (byte)teste;
                            }

                            a = 2;

                            teste = (int)Math.Round((((dataPtrcopy + nChan * (x - 1) + stepcopy * (y - 1))[a] * matrix[0, 0]) + ((dataPtrcopy + nChan * x + stepcopy * (y - 1))[a] * matrix[0, 1]) + ((dataPtrcopy + nChan * (x + 1) + stepcopy * (y - 1))[a] * matrix[0, 2]) + ((dataPtrcopy + nChan * (x - 1) + stepcopy * y)[a] * matrix[1, 0]) + ((dataPtrcopy + nChan * x + stepcopy * y)[a] * matrix[1, 1]) + ((dataPtrcopy + nChan * (x + 1) + stepcopy * y)[a] * matrix[1, 2]) + ((dataPtrcopy + nChan * (x - 1) + stepcopy * (y + 1))[a] * matrix[2, 0]) + ((dataPtrcopy + nChan * x + stepcopy * (y + 1))[a] * matrix[2, 1]) + (((dataPtrcopy + nChan * (x + 1) + stepcopy * (y + 1))[a] * matrix[2, 2]))) / matrixWeight);


                            if (teste < 0)
                            {
                                red = 0;
                            }
                            if (teste > 255)
                            {
                                red = 255;
                            }
                            else
                            {
                                red = (byte)teste;
                            }



                            (dataPtr + nChan * x + step * y)[0] = blue;
                            (dataPtr + nChan * x + step * y)[1] = green;
                            (dataPtr + nChan * x + step * y)[2] = red;

                        }
                    }
                    for (x = 1; x < width - 1; x++)
                    {
                        a = 0;
                        y = 0;
                        teste = (int)Math.Round(((dataPtrcopy + nChan * (x - 1) + stepcopy * y)[a] * matrix[0, 0] + (dataPtrcopy + nChan * x + stepcopy * y)[a] * matrix[0, 1] + (dataPtrcopy + nChan * (x + 1) + stepcopy * y)[a] * matrix[0, 2] + (dataPtrcopy + nChan * (x - 1) + stepcopy * y)[a] * matrix[1, 0] + (dataPtrcopy + nChan * x + stepcopy * y)[a] * matrix[1, 1] + (dataPtrcopy + nChan * (x + 1) + stepcopy * y)[a] * matrix[1, 2] + (dataPtrcopy + nChan * (x - 1) + stepcopy * (y + 1))[a] * matrix[2, 0] + (dataPtrcopy + nChan * x + stepcopy * (y + 1))[a] * matrix[2, 1] + (dataPtrcopy + nChan * (x + 1) + stepcopy * (y + 1))[a] * matrix[2, 2]) / matrixWeight);

                        if (teste < 0)
                        {
                            blue = 0;
                        }
                        if (teste > 255)
                        {
                            blue = 255;
                        }
                        else
                        {
                            blue = (byte)teste;
                        }

                        a = 1;

                        teste = (int)Math.Round(((dataPtrcopy + nChan * (x - 1) + stepcopy * y)[a] * matrix[0, 0] + (dataPtrcopy + nChan * x + stepcopy * y)[a] * matrix[0, 1] + (dataPtrcopy + nChan * (x + 1) + stepcopy * y)[a] * matrix[0, 2] + (dataPtrcopy + nChan * (x - 1) + stepcopy * y)[a] * matrix[1, 0] + (dataPtrcopy + nChan * x + stepcopy * y)[a] * matrix[1, 1] + (dataPtrcopy + nChan * (x + 1) + stepcopy * y)[a] * matrix[1, 2] + (dataPtrcopy + nChan * (x - 1) + stepcopy * (y + 1))[a] * matrix[2, 0] + (dataPtrcopy + nChan * x + stepcopy * (y + 1))[a] * matrix[2, 1] + (dataPtrcopy + nChan * (x + 1) + stepcopy * (y + 1))[a] * matrix[2, 2]) / matrixWeight);

                        if (teste < 0)
                        {
                            green = 0;
                        }
                        if (teste > 255)
                        {
                            green = 255;
                        }
                        else
                        {
                            green = (byte)teste;
                        }

                        a = 2;

                        teste = (int)Math.Round(((dataPtrcopy + nChan * (x - 1) + stepcopy * y)[a] * matrix[0, 0] + (dataPtrcopy + nChan * x + stepcopy * y)[a] * matrix[0, 1] + (dataPtrcopy + nChan * (x + 1) + stepcopy * y)[a] * matrix[0, 2] + (dataPtrcopy + nChan * (x - 1) + stepcopy * y)[a] * matrix[1, 0] + (dataPtrcopy + nChan * x + stepcopy * y)[a] * matrix[1, 1] + (dataPtrcopy + nChan * (x + 1) + stepcopy * y)[a] * matrix[1, 2] + (dataPtrcopy + nChan * (x - 1) + stepcopy * (y + 1))[a] * matrix[2, 0] + (dataPtrcopy + nChan * x + stepcopy * (y + 1))[a] * matrix[2, 1] + (dataPtrcopy + nChan * (x + 1) + stepcopy * (y + 1))[a] * matrix[2, 2]) / matrixWeight);


                        if (teste < 0)
                        {
                            red = 0;
                        }
                        if (teste > 255)
                        {
                            red = 255;
                        }
                        else
                        {
                            red = (byte)teste;
                        }




                        (dataPtr + nChan * x + step * 0)[0] = blue;
                        (dataPtr + nChan * x + step * 0)[1] = green;
                        (dataPtr + nChan * x + step * 0)[2] = red;

                        y = height - 1;

                        a = 0;
                        teste = (int)Math.Round(((dataPtrcopy + nChan * (x - 1) + stepcopy * (y - 1))[a] * matrix[0, 0] + (dataPtrcopy + nChan * x + stepcopy * (y - 1))[a] * matrix[0, 1] + (dataPtrcopy + nChan * (x + 1) + stepcopy * (y - 1))[a] * matrix[0, 2] + (dataPtrcopy + nChan * (x - 1) + stepcopy * y)[a] * matrix[1, 0] + (dataPtrcopy + nChan * x + stepcopy * y)[a] * matrix[1, 1] + (dataPtrcopy + nChan * (x + 1) + stepcopy * y)[a] * matrix[1, 2] + (dataPtrcopy + nChan * (x - 1) + stepcopy * y)[a] * matrix[2, 0] + (dataPtrcopy + nChan * x + stepcopy * y)[a] * matrix[2, 1] + (dataPtrcopy + nChan * (x + 1) + stepcopy * y)[a] * matrix[2, 2]) / matrixWeight);

                        if (teste < 0)
                        {
                            blue = 0;
                        }
                        if (teste > 255)
                        {
                            blue = 255;
                        }
                        else
                        {
                            blue = (byte)teste;
                        }

                        a = 1;
                        teste = (int)Math.Round(((dataPtrcopy + nChan * (x - 1) + stepcopy * (y - 1))[a] * matrix[0, 0] + (dataPtrcopy + nChan * x + stepcopy * (y - 1))[a] * matrix[0, 1] + (dataPtrcopy + nChan * (x + 1) + stepcopy * (y - 1))[a] * matrix[0, 2] + (dataPtrcopy + nChan * (x - 1) + stepcopy * y)[a] * matrix[1, 0] + (dataPtrcopy + nChan * x + stepcopy * y)[a] * matrix[1, 1] + (dataPtrcopy + nChan * (x + 1) + stepcopy * y)[a] * matrix[1, 2] + (dataPtrcopy + nChan * (x - 1) + stepcopy * y)[a] * matrix[2, 0] + (dataPtrcopy + nChan * x + stepcopy * y)[a] * matrix[2, 1] + (dataPtrcopy + nChan * (x + 1) + stepcopy * y)[a] * matrix[2, 2]) / matrixWeight);

                        if (teste < 0)
                        {
                            green = 0;
                        }
                        if (teste > 255)
                        {
                            green = 255;
                        }
                        else
                        {
                            green = (byte)teste;
                        }

                        a = 2;
                        teste = (int)Math.Round(((dataPtrcopy + nChan * (x - 1) + stepcopy * (y - 1))[a] * matrix[0, 0] + (dataPtrcopy + nChan * x + stepcopy * (y - 1))[a] * matrix[0, 1] + (dataPtrcopy + nChan * (x + 1) + stepcopy * (y - 1))[a] * matrix[0, 2] + (dataPtrcopy + nChan * (x - 1) + stepcopy * y)[a] * matrix[1, 0] + (dataPtrcopy + nChan * x + stepcopy * y)[a] * matrix[1, 1] + (dataPtrcopy + nChan * (x + 1) + stepcopy * y)[a] * matrix[1, 2] + (dataPtrcopy + nChan * (x - 1) + stepcopy * y)[a] * matrix[2, 0] + (dataPtrcopy + nChan * x + stepcopy * y)[a] * matrix[2, 1] + (dataPtrcopy + nChan * (x + 1) + stepcopy * y)[a] * matrix[2, 2]) / matrixWeight);

                        if (teste < 0)
                        {
                            red = 0;
                        }
                        if (teste > 255)
                        {
                            red = 255;
                        }
                        else
                        {
                            red = (byte)teste;
                        }


                        (dataPtr + nChan * x + step * (height - 1))[0] = blue;
                        (dataPtr + nChan * x + step * (height - 1))[1] = green;
                        (dataPtr + nChan * x + step * (height - 1))[2] = red;

                    }
                    for (y = 1; y < height - 1; y++)
                    {
                        x = 0;

                        a = 0;
                        teste = (int)Math.Round(((dataPtrcopy + nChan * x + stepcopy * (y - 1))[a] * matrix[0, 0] + (dataPtrcopy + nChan * x + stepcopy * (y - 1))[a] * matrix[0, 1] + (dataPtrcopy + nChan * (x + 1) + stepcopy * (y - 1))[a] * matrix[0, 2] + (dataPtrcopy + nChan * x + stepcopy * y)[a] * matrix[1, 0] + (dataPtrcopy + nChan * x + stepcopy * y)[a] * matrix[1, 1] + (dataPtrcopy + nChan * (x + 1) + stepcopy * y)[a] * matrix[1, 2] + (dataPtrcopy + nChan * x + stepcopy * (y + 1))[a] * matrix[2, 0] + (dataPtrcopy + nChan * x + stepcopy * (y + 1))[a] * matrix[2, 1] + (dataPtrcopy + nChan * (x + 1) + stepcopy * (y + 1))[a] * matrix[2, 2]) / matrixWeight);
                        if (teste < 0)
                        {
                            blue = 0;
                        }
                        if (teste > 255)
                        {
                            blue = 255;
                        }
                        else
                        {
                            blue = (byte)teste;
                        }


                        a = 1;
                        teste = (int)Math.Round(((dataPtrcopy + nChan * x + stepcopy * (y - 1))[a] * matrix[0, 0] + (dataPtrcopy + nChan * x + stepcopy * (y - 1))[a] * matrix[0, 1] + (dataPtrcopy + nChan * (x + 1) + stepcopy * (y - 1))[a] * matrix[0, 2] + (dataPtrcopy + nChan * x + stepcopy * y)[a] * matrix[1, 0] + (dataPtrcopy + nChan * x + stepcopy * y)[a] * matrix[1, 1] + (dataPtrcopy + nChan * (x + 1) + stepcopy * y)[a] * matrix[1, 2] + (dataPtrcopy + nChan * x + stepcopy * (y + 1))[a] * matrix[2, 0] + (dataPtrcopy + nChan * x + stepcopy * (y + 1))[a] * matrix[2, 1] + (dataPtrcopy + nChan * (x + 1) + stepcopy * (y + 1))[a] * matrix[2, 2]) / matrixWeight);
                        if (teste < 0)
                        {
                            green = 0;
                        }
                        if (teste > 255)
                        {
                            green = 255;
                        }
                        else
                        {
                            green = (byte)teste;
                        }


                        a = 2;
                        teste = (int)Math.Round(((dataPtrcopy + nChan * x + stepcopy * (y - 1))[a] * matrix[0, 0] + (dataPtrcopy + nChan * x + stepcopy * (y - 1))[a] * matrix[0, 1] + (dataPtrcopy + nChan * (x + 1) + stepcopy * (y - 1))[a] * matrix[0, 2] + (dataPtrcopy + nChan * x + stepcopy * y)[a] * matrix[1, 0] + (dataPtrcopy + nChan * x + stepcopy * y)[a] * matrix[1, 1] + (dataPtrcopy + nChan * (x + 1) + stepcopy * y)[a] * matrix[1, 2] + (dataPtrcopy + nChan * x + stepcopy * (y + 1))[a] * matrix[2, 0] + (dataPtrcopy + nChan * x + stepcopy * (y + 1))[a] * matrix[2, 1] + (dataPtrcopy + nChan * (x + 1) + stepcopy * (y + 1))[a] * matrix[2, 2]) / matrixWeight);
                        if (teste < 0)
                        {
                            red = 0;
                        }
                        if (teste > 255)
                        {
                            red = 255;
                        }
                        else
                        {
                            red = (byte)teste;
                        }

                        (dataPtr + nChan * x + step * y)[0] = blue;
                        (dataPtr + nChan * x + step * y)[1] = green;
                        (dataPtr + nChan * x + step * y)[2] = red;



                        x = width - 1;

                        a = 0;
                        teste = (int)Math.Round(((dataPtrcopy + nChan * (x - 1) + step * (y - 1))[a] * matrix[0, 0] + (dataPtrcopy + nChan * (x) + step * (y - 1))[a] * matrix[0, 1] + (dataPtrcopy + nChan * (x) + step * (y - 1))[a] * matrix[0, 2] + (dataPtrcopy + nChan * (x - 1) + step * (y))[a] * matrix[1, 0] + (dataPtrcopy + nChan * (x) + step * (y))[a] * matrix[1, 1] + (dataPtrcopy + nChan * (x) + step * (y))[a] * matrix[1, 2] + (dataPtrcopy + nChan * (x - 1) + step * (y + 1))[a] * matrix[2, 0] + (dataPtrcopy + nChan * (x) + step * (y + 1))[a] * matrix[2, 1] + (dataPtrcopy + nChan * (x) + step * (y + 1))[a] * matrix[2, 2]) / matrixWeight);

                        if (teste < 0)
                        {
                            blue = 0;
                        }
                        if (teste > 255)
                        {
                            blue = 255;
                        }
                        else
                        {
                            blue = (byte)teste;
                        }


                        a = 1;
                        teste = (int)Math.Round(((dataPtrcopy + nChan * (x - 1) + step * (y - 1))[a] * matrix[0, 0] + (dataPtrcopy + nChan * (x) + step * (y - 1))[a] * matrix[0, 1] + (dataPtrcopy + nChan * (x) + step * (y - 1))[a] * matrix[0, 2] + (dataPtrcopy + nChan * (x - 1) + step * (y))[a] * matrix[1, 0] + (dataPtrcopy + nChan * (x) + step * (y))[a] * matrix[1, 1] + (dataPtrcopy + nChan * (x) + step * (y))[a] * matrix[1, 2] + (dataPtrcopy + nChan * (x - 1) + step * (y + 1))[a] * matrix[2, 0] + (dataPtrcopy + nChan * (x) + step * (y + 1))[a] * matrix[2, 1] + (dataPtrcopy + nChan * (x) + step * (y + 1))[a] * matrix[2, 2]) / matrixWeight);

                        if (teste < 0)
                        {
                            green = 0;
                        }
                        if (teste > 255)
                        {
                            green = 255;
                        }
                        else
                        {
                            green = (byte)teste;
                        }


                        a = 2;
                        teste = (int)Math.Round(((dataPtrcopy + nChan * (x - 1) + step * (y - 1))[a] * matrix[0, 0] + (dataPtrcopy + nChan * (x) + step * (y - 1))[a] * matrix[0, 1] + (dataPtrcopy + nChan * (x) + step * (y - 1))[a] * matrix[0, 2] + (dataPtrcopy + nChan * (x - 1) + step * (y))[a] * matrix[1, 0] + (dataPtrcopy + nChan * (x) + step * (y))[a] * matrix[1, 1] + (dataPtrcopy + nChan * (x) + step * (y))[a] * matrix[1, 2] + (dataPtrcopy + nChan * (x - 1) + step * (y + 1))[a] * matrix[2, 0] + (dataPtrcopy + nChan * (x) + step * (y + 1))[a] * matrix[2, 1] + (dataPtrcopy + nChan * (x) + step * (y + 1))[a] * matrix[2, 2]) / matrixWeight);

                        if (teste < 0)
                        {
                            red = 0;
                        }
                        if (teste > 255)
                        {
                            red = 255;
                        }
                        else
                        {
                            red = (byte)teste;
                        }


                        (dataPtr + nChan * (width - 1) + step * y)[0] = blue;
                        (dataPtr + nChan * (width - 1) + step * y)[1] = green;
                        (dataPtr + nChan * (width - 1) + step * y)[2] = red;
                    }

                    // canto superior esquerdo
                    x = 0;
                    y = 0;

                    a = 0;
                    teste = (int)Math.Round(((dataPtrcopy + nChan * x + stepcopy * y)[a] * matrix[0, 0] + (dataPtrcopy + nChan * x + stepcopy * y)[a] * matrix[0, 1] + (dataPtrcopy + nChan * (x + 1) + stepcopy * y)[a] * matrix[0, 2] + (dataPtrcopy + nChan * x + stepcopy * y)[a] * matrix[1, 0] + (dataPtrcopy + nChan * x + stepcopy * y)[a] * matrix[1, 1] + (dataPtrcopy + nChan * (x + 1) + stepcopy * y)[a] * matrix[1, 2] + (dataPtrcopy + nChan * x + stepcopy * (y + 1))[a] * matrix[2, 0] + (dataPtrcopy + nChan * x + stepcopy * (y + 1))[a] * matrix[2, 1] + (dataPtrcopy + nChan * (x + 1) + stepcopy * (y + 1))[a] * matrix[2, 2]) / matrixWeight);

                    if (teste < 0)
                    {
                        blue = 0;
                    }
                    if (teste > 255)
                    {
                        blue = 255;
                    }
                    else
                    {
                        blue = (byte)teste;
                    }

                    a = 1;
                    teste = (int)Math.Round(((dataPtrcopy + nChan * x + stepcopy * y)[a] * matrix[0, 0] + (dataPtrcopy + nChan * x + stepcopy * y)[a] * matrix[0, 1] + (dataPtrcopy + nChan * (x + 1) + stepcopy * y)[a] * matrix[0, 2] + (dataPtrcopy + nChan * x + stepcopy * y)[a] * matrix[1, 0] + (dataPtrcopy + nChan * x + stepcopy * y)[a] * matrix[1, 1] + (dataPtrcopy + nChan * (x + 1) + stepcopy * y)[a] * matrix[1, 2] + (dataPtrcopy + nChan * x + stepcopy * (y + 1))[a] * matrix[2, 0] + (dataPtrcopy + nChan * x + stepcopy * (y + 1))[a] * matrix[2, 1] + (dataPtrcopy + nChan * (x + 1) + stepcopy * (y + 1))[a] * matrix[2, 2]) / matrixWeight);

                    if (teste < 0)
                    {
                        green = 0;
                    }
                    if (teste > 255)
                    {
                        green = 255;
                    }
                    else
                    {
                        green = (byte)teste;
                    }

                    a = 2;
                    teste = (int)Math.Round(((dataPtrcopy + nChan * x + stepcopy * y)[a] * matrix[0, 0] + (dataPtrcopy + nChan * x + stepcopy * y)[a] * matrix[0, 1] + (dataPtrcopy + nChan * (x + 1) + stepcopy * y)[a] * matrix[0, 2] + (dataPtrcopy + nChan * x + stepcopy * y)[a] * matrix[1, 0] + (dataPtrcopy + nChan * x + stepcopy * y)[a] * matrix[1, 1] + (dataPtrcopy + nChan * (x + 1) + stepcopy * y)[a] * matrix[1, 2] + (dataPtrcopy + nChan * x + stepcopy * (y + 1))[a] * matrix[2, 0] + (dataPtrcopy + nChan * x + stepcopy * (y + 1))[a] * matrix[2, 1] + (dataPtrcopy + nChan * (x + 1) + stepcopy * (y + 1))[a] * matrix[2, 2]) / matrixWeight);

                    if (teste < 0)
                    {
                        red = 0;
                    }
                    if (teste > 255)
                    {
                        red = 255;
                    }
                    else
                    {
                        red = (byte)teste;
                    }


                    (dataPtr + nChan * x + step * y)[0] = blue;
                    (dataPtr + nChan * x + step * y)[1] = green;
                    (dataPtr + nChan * x + step * y)[2] = red;

                    // canto superior direito

                    x = width - 1;
                    y = 0;

                    a = 0;
                    teste = (int)Math.Round(((dataPtrcopy + nChan * (x - 1) + stepcopy * y)[a] * matrix[0, 0] + (dataPtrcopy + nChan * x + stepcopy * y)[a] * matrix[0, 1] + (dataPtrcopy + nChan * x + stepcopy * y)[a] * matrix[0, 2] + (dataPtrcopy + nChan * (x - 1) + stepcopy * y)[a] * matrix[1, 0] + (dataPtrcopy + nChan * x + stepcopy * y)[a] * matrix[1, 1] + (dataPtrcopy + nChan * x + stepcopy * y)[a] * matrix[1, 2] + (dataPtrcopy + nChan * (x - 1) + stepcopy * (y + 1))[a] * matrix[2, 0] + (dataPtrcopy + nChan * x + stepcopy * (y + 1))[a] * matrix[2, 1] + (dataPtrcopy + nChan * x + stepcopy * (y + 1))[a] * matrix[2, 2]) / matrixWeight);

                    if (teste < 0)
                    {
                        blue = 0;
                    }
                    if (teste > 255)
                    {
                        blue = 255;
                    }
                    else
                    {
                        blue = (byte)teste;
                    }

                    a = 1;
                    teste = (int)Math.Round(((dataPtrcopy + nChan * (x - 1) + stepcopy * y)[a] * matrix[0, 0] + (dataPtrcopy + nChan * x + stepcopy * y)[a] * matrix[0, 1] + (dataPtrcopy + nChan * x + stepcopy * y)[a] * matrix[0, 2] + (dataPtrcopy + nChan * (x - 1) + stepcopy * y)[a] * matrix[1, 0] + (dataPtrcopy + nChan * x + stepcopy * y)[a] * matrix[1, 1] + (dataPtrcopy + nChan * x + stepcopy * y)[a] * matrix[1, 2] + (dataPtrcopy + nChan * (x - 1) + stepcopy * (y + 1))[a] * matrix[2, 0] + (dataPtrcopy + nChan * x + stepcopy * (y + 1))[a] * matrix[2, 1] + (dataPtrcopy + nChan * x + stepcopy * (y + 1))[a] * matrix[2, 2]) / matrixWeight);

                    if (teste < 0)
                    {
                        green = 0;
                    }
                    if (teste > 255)
                    {
                        green = 255;
                    }
                    else
                    {
                        green = (byte)teste;
                    }

                    a = 2;
                    teste = (int)Math.Round(((dataPtrcopy + nChan * (x - 1) + stepcopy * y)[a] * matrix[0, 0] + (dataPtrcopy + nChan * x + stepcopy * y)[a] * matrix[0, 1] + (dataPtrcopy + nChan * x + stepcopy * y)[a] * matrix[0, 2] + (dataPtrcopy + nChan * (x - 1) + stepcopy * y)[a] * matrix[1, 0] + (dataPtrcopy + nChan * x + stepcopy * y)[a] * matrix[1, 1] + (dataPtrcopy + nChan * x + stepcopy * y)[a] * matrix[1, 2] + (dataPtrcopy + nChan * (x - 1) + stepcopy * (y + 1))[a] * matrix[2, 0] + (dataPtrcopy + nChan * x + stepcopy * (y + 1))[a] * matrix[2, 1] + (dataPtrcopy + nChan * x + stepcopy * (y + 1))[a] * matrix[2, 2]) / matrixWeight);

                    if (teste < 0)
                    {
                        red = 0;
                    }
                    if (teste > 255)
                    {
                        red = 255;
                    }
                    else
                    {
                        red = (byte)teste;
                    }

                    (dataPtr + nChan * x + step * y)[0] = blue;
                    (dataPtr + nChan * x + step * y)[1] = green;
                    (dataPtr + nChan * x + step * y)[2] = red;

                    // canto inferior esquerdo

                    x = 0; y = height - 1;

                    a = 0;
                    teste = (int)Math.Round(((dataPtrcopy + nChan * (x) + stepcopy * (y - 1))[a] * matrix[0, 0] + (dataPtrcopy + nChan * x + stepcopy * (y - 1))[a] * matrix[0, 1] + (dataPtrcopy + nChan * (x + 1) + stepcopy * (y - 1))[a] * matrix[0, 2] + (dataPtrcopy + nChan * (x) + stepcopy * y)[a] * matrix[1, 0] + (dataPtrcopy + nChan * x + stepcopy * y)[a] * matrix[1, 1] + (dataPtrcopy + nChan * (x + 1) + stepcopy * (y))[a] * matrix[1, 2] + (dataPtrcopy + nChan * (x) + stepcopy * (y))[a] * matrix[2, 0] + (dataPtrcopy + nChan * x + stepcopy * (y))[a] * matrix[2, 1] + (dataPtrcopy + nChan * (x + 1) + stepcopy * (y))[a] * matrix[2, 2]) / matrixWeight);

                    if (teste < 0)
                    {
                        blue = 0;
                    }
                    if (teste > 255)
                    {
                        blue = 255;
                    }
                    else
                    {
                        blue = (byte)teste;
                    }

                    a = 1;
                    teste = (int)Math.Round(((dataPtrcopy + nChan * (x) + stepcopy * (y - 1))[a] * matrix[0, 0] + (dataPtrcopy + nChan * x + stepcopy * (y - 1))[a] * matrix[0, 1] + (dataPtrcopy + nChan * (x + 1) + stepcopy * (y - 1))[a] * matrix[0, 2] + (dataPtrcopy + nChan * (x) + stepcopy * y)[a] * matrix[1, 0] + (dataPtrcopy + nChan * x + stepcopy * y)[a] * matrix[1, 1] + (dataPtrcopy + nChan * (x + 1) + stepcopy * (y))[a] * matrix[1, 2] + (dataPtrcopy + nChan * (x) + stepcopy * (y))[a] * matrix[2, 0] + (dataPtrcopy + nChan * x + stepcopy * (y))[a] * matrix[2, 1] + (dataPtrcopy + nChan * (x + 1) + stepcopy * (y))[a] * matrix[2, 2]) / matrixWeight);

                    if (teste < 0)
                    {
                        green = 0;
                    }
                    if (teste > 255)
                    {
                        green = 255;
                    }
                    else
                    {
                        green = (byte)teste;
                    }

                    a = 2;
                    teste = (int)Math.Round(((dataPtrcopy + nChan * (x) + stepcopy * (y - 1))[a] * matrix[0, 0] + (dataPtrcopy + nChan * x + stepcopy * (y - 1))[a] * matrix[0, 1] + (dataPtrcopy + nChan * (x + 1) + stepcopy * (y - 1))[a] * matrix[0, 2] + (dataPtrcopy + nChan * (x) + stepcopy * y)[a] * matrix[1, 0] + (dataPtrcopy + nChan * x + stepcopy * y)[a] * matrix[1, 1] + (dataPtrcopy + nChan * (x + 1) + stepcopy * (y))[a] * matrix[1, 2] + (dataPtrcopy + nChan * (x) + stepcopy * (y))[a] * matrix[2, 0] + (dataPtrcopy + nChan * x + stepcopy * (y))[a] * matrix[2, 1] + (dataPtrcopy + nChan * (x + 1) + stepcopy * (y))[a] * matrix[2, 2]) / matrixWeight);

                    if (teste < 0)
                    {
                        red = 0;
                    }
                    if (teste > 255)
                    {
                        red = 255;
                    }
                    else
                    {
                        red = (byte)teste;
                    }


                    (dataPtr + nChan * x + step * y)[0] = blue;
                    (dataPtr + nChan * x + step * y)[1] = green;
                    (dataPtr + nChan * x + step * y)[2] = red;


                    // canto inferior direiro 

                    x = width - 1; y = height - 1;

                    a = 0;
                    teste = (int)Math.Round(((dataPtrcopy + nChan * (x - 1) + stepcopy * (y - 1))[a] * matrix[0, 0] + (dataPtrcopy + nChan * x + stepcopy * (y - 1))[a] * matrix[0, 1] + (dataPtrcopy + nChan * x + stepcopy * (y - 1))[a] * matrix[0, 2] + (dataPtrcopy + nChan * (x - 1) + stepcopy * y)[a] * matrix[1, 0] + (dataPtrcopy + nChan * x + stepcopy * y)[a] * matrix[1, 1] + (dataPtrcopy + nChan * x + stepcopy * y)[a] * matrix[1, 2] + (dataPtrcopy + nChan * (x - 1) + stepcopy * y)[a] * matrix[2, 0] + (dataPtrcopy + nChan * x + stepcopy * y)[a] * matrix[2, 1] + (dataPtrcopy + nChan * x + stepcopy * y)[a] * matrix[2, 2]) / matrixWeight);


                    if (teste < 0)
                    {
                        blue = 0;
                    }
                    if (teste > 255)
                    {
                        blue = 255;
                    }
                    else
                    {
                        blue = (byte)teste;
                    }

                    a = 1;
                    teste = (int)Math.Round(((dataPtrcopy + nChan * (x - 1) + stepcopy * (y - 1))[a] * matrix[0, 0] + (dataPtrcopy + nChan * x + stepcopy * (y - 1))[a] * matrix[0, 1] + (dataPtrcopy + nChan * x + stepcopy * (y - 1))[a] * matrix[0, 2] + (dataPtrcopy + nChan * (x - 1) + stepcopy * y)[a] * matrix[1, 0] + (dataPtrcopy + nChan * x + stepcopy * y)[a] * matrix[1, 1] + (dataPtrcopy + nChan * x + stepcopy * y)[a] * matrix[1, 2] + (dataPtrcopy + nChan * (x - 1) + stepcopy * y)[a] * matrix[2, 0] + (dataPtrcopy + nChan * x + stepcopy * y)[a] * matrix[2, 1] + (dataPtrcopy + nChan * x + stepcopy * y)[a] * matrix[2, 2]) / matrixWeight);

                    if (teste < 0)
                    {
                        green = 0;
                    }
                    if (teste > 255)
                    {
                        green = 255;
                    }
                    else
                    {
                        green = (byte)teste;
                    }


                    a = 2;
                    teste = (int)Math.Round(((dataPtrcopy + nChan * (x - 1) + stepcopy * (y - 1))[a] * matrix[0, 0] + (dataPtrcopy + nChan * x + stepcopy * (y - 1))[a] * matrix[0, 1] + (dataPtrcopy + nChan * x + stepcopy * (y - 1))[a] * matrix[0, 2] + (dataPtrcopy + nChan * (x - 1) + stepcopy * y)[a] * matrix[1, 0] + (dataPtrcopy + nChan * x + stepcopy * y)[a] * matrix[1, 1] + (dataPtrcopy + nChan * x + stepcopy * y)[a] * matrix[1, 2] + (dataPtrcopy + nChan * (x - 1) + stepcopy * y)[a] * matrix[2, 0] + (dataPtrcopy + nChan * x + stepcopy * y)[a] * matrix[2, 1] + (dataPtrcopy + nChan * x + stepcopy * y)[a] * matrix[2, 2]) / matrixWeight);
                    if (teste < 0)
                    {
                        red = 0;
                    }
                    if (teste > 255)
                    {
                        red = 255;
                    }
                    else
                    {
                        red = (byte)teste;
                    }


                    (dataPtr + nChan * x + step * y)[0] = blue;
                    (dataPtr + nChan * x + step * y)[1] = green;
                    (dataPtr + nChan * x + step * y)[2] = red;


                }
            }


        }

        public static void Sobel(Image<Bgr, byte> img, Image<Bgr, byte> imgCopy)
        {
            unsafe
            {
                // direct access to the image memory(sequencial)
                // direcion top left -> bottom right

                MIplImage m = img.MIplImage;
                MIplImage mcopy = imgCopy.MIplImage;
                byte* dataPtr = (byte*)m.imageData.ToPointer(); // Pointer to the image
                byte* dataPtrcopy = (byte*)mcopy.imageData.ToPointer(); // Pointer to the imagecopy
                byte blue, green, red;

                int step = m.widthStep;
                int stepcopy = mcopy.widthStep;
                int width = img.Width;
                int height = img.Height;
                int nChan = m.nChannels; // number of channels - 3
                int padding = m.widthStep - m.nChannels * m.width; // alinhament bytes (padding)
                int x, y;
                int teste = 0;
                int a;

                if (nChan == 3) // image in RGB
                {       // parte de dentro da imagem
                    for (y = 1; y < height - 1; y++)
                    {
                        for (x = 1; x < width - 1; x++)
                        {
                            //fazer dentro
                            a = 0;
                            teste = (int)(Math.Abs((dataPtrcopy + nChan * (x - 1) + stepcopy * (y - 1))[a] + 2 * (dataPtrcopy + nChan * (x - 1) + stepcopy * (y))[a] + (dataPtrcopy + nChan * (x - 1) + stepcopy * (y + 1))[a] - (dataPtrcopy + nChan * (x + 1) + stepcopy * (y - 1))[a] - 2 * (dataPtrcopy + nChan * (x + 1) + stepcopy * (y))[a] - (dataPtrcopy + nChan * (x + 1) + stepcopy * (y + 1))[a])
                                + Math.Abs((dataPtrcopy + nChan * (x - 1) + stepcopy * (y + 1))[a] + 2 * (dataPtrcopy + nChan * (x) + stepcopy * (y + 1))[a] + (dataPtrcopy + nChan * (x + 1) + stepcopy * (y + 1))[a] - (dataPtrcopy + nChan * (x - 1) + stepcopy * (y - 1))[a] - 2 * (dataPtrcopy + nChan * (x) + stepcopy * (y - 1))[a] - (dataPtrcopy + nChan * (x + 1) + stepcopy * (y - 1))[a]));

                            if (teste < 0)
                            {
                                blue = 0;
                            }
                            else if (teste > 255)
                            {
                                blue = 255;
                            }
                            else
                            {
                                blue = (byte)teste;
                            }

                            a = 1;
                            teste = (int)(Math.Abs((dataPtrcopy + nChan * (x - 1) + stepcopy * (y - 1))[a] + 2 * (dataPtrcopy + nChan * (x - 1) + stepcopy * (y))[a] + (dataPtrcopy + nChan * (x - 1) + stepcopy * (y + 1))[a] - (dataPtrcopy + nChan * (x + 1) + stepcopy * (y - 1))[a] - 2 * (dataPtrcopy + nChan * (x + 1) + stepcopy * (y))[a] - (dataPtrcopy + nChan * (x + 1) + stepcopy * (y + 1))[a])
                               + Math.Abs((dataPtrcopy + nChan * (x - 1) + stepcopy * (y + 1))[a] + 2 * (dataPtrcopy + nChan * (x) + stepcopy * (y + 1))[a] + (dataPtrcopy + nChan * (x + 1) + stepcopy * (y + 1))[a] - (dataPtrcopy + nChan * (x - 1) + stepcopy * (y - 1))[a] - 2 * (dataPtrcopy + nChan * (x) + stepcopy * (y - 1))[a] - (dataPtrcopy + nChan * (x + 1) + stepcopy * (y - 1))[a]));


                            if (teste < 0)
                            {
                                green = 0;
                            }
                            else if (teste > 255)
                            {
                                green = 255;
                            }
                            else
                            {
                                green = (byte)teste;
                            }

                            a = 2;

                            teste = (int)(Math.Abs((dataPtrcopy + nChan * (x - 1) + stepcopy * (y - 1))[a] + 2 * (dataPtrcopy + nChan * (x - 1) + stepcopy * (y))[a] + (dataPtrcopy + nChan * (x - 1) + stepcopy * (y + 1))[a] - (dataPtrcopy + nChan * (x + 1) + stepcopy * (y - 1))[a] - 2 * (dataPtrcopy + nChan * (x + 1) + stepcopy * (y))[a] - (dataPtrcopy + nChan * (x + 1) + stepcopy * (y + 1))[a])
                               + Math.Abs((dataPtrcopy + nChan * (x - 1) + stepcopy * (y + 1))[a] + 2 * (dataPtrcopy + nChan * (x) + stepcopy * (y + 1))[a] + (dataPtrcopy + nChan * (x + 1) + stepcopy * (y + 1))[a] - (dataPtrcopy + nChan * (x - 1) + stepcopy * (y - 1))[a] - 2 * (dataPtrcopy + nChan * (x) + stepcopy * (y - 1))[a] - (dataPtrcopy + nChan * (x + 1) + stepcopy * (y - 1))[a]));


                            if (teste < 0)
                            {
                                red = 0;
                            }
                            else if (teste > 255)
                            {
                                red = 255;
                            }
                            else
                            {
                                red = (byte)teste;
                            }


                            (dataPtr + nChan * x + step * y)[0] = blue;
                            (dataPtr + nChan * x + step * y)[1] = green;
                            (dataPtr + nChan * x + step * y)[2] = red;

                        }
                    }

                    for (x = 1; x < width - 1; x++)
                    {
                        y = 0;

                        a = 0;
                        teste = (int)(Math.Abs((dataPtrcopy + nChan * (x - 1) + stepcopy * (y))[a] + 2 * (dataPtrcopy + nChan * (x - 1) + stepcopy * (y))[a] + (dataPtrcopy + nChan * (x - 1) + stepcopy * (y + 1))[a] - (dataPtrcopy + nChan * (x + 1) + stepcopy * (y))[a] - 2 * (dataPtrcopy + nChan * (x + 1) + stepcopy * (y))[a] - (dataPtrcopy + nChan * (x + 1) + stepcopy * (y + 1))[a])
                            + Math.Abs((dataPtrcopy + nChan * (x - 1) + stepcopy * (y + 1))[a] + 2 * (dataPtrcopy + nChan * (x) + stepcopy * (y + 1))[a] + (dataPtrcopy + nChan * (x + 1) + stepcopy * (y + 1))[a] - (dataPtrcopy + nChan * (x - 1) + stepcopy * (y))[a] - 2 * (dataPtrcopy + nChan * (x) + stepcopy * (y))[a] - (dataPtrcopy + nChan * (x + 1) + stepcopy * (y))[a]));

                        if (teste < 0)
                        {
                            blue = 0;
                        }
                        else if (teste > 255)
                        {
                            blue = 255;
                        }
                        else
                        {
                            blue = (byte)teste;
                        }

                        a = 1;
                        teste = (int)(Math.Abs((dataPtrcopy + nChan * (x - 1) + stepcopy * (y))[a] + 2 * (dataPtrcopy + nChan * (x - 1) + stepcopy * (y))[a] + (dataPtrcopy + nChan * (x - 1) + stepcopy * (y + 1))[a] - (dataPtrcopy + nChan * (x + 1) + stepcopy * (y))[a] - 2 * (dataPtrcopy + nChan * (x + 1) + stepcopy * (y))[a] - (dataPtrcopy + nChan * (x + 1) + stepcopy * (y + 1))[a])
                            + Math.Abs((dataPtrcopy + nChan * (x - 1) + stepcopy * (y + 1))[a] + 2 * (dataPtrcopy + nChan * (x) + stepcopy * (y + 1))[a] + (dataPtrcopy + nChan * (x + 1) + stepcopy * (y + 1))[a] - (dataPtrcopy + nChan * (x - 1) + stepcopy * (y))[a] - 2 * (dataPtrcopy + nChan * (x) + stepcopy * (y))[a] - (dataPtrcopy + nChan * (x + 1) + stepcopy * (y))[a]));

                        if (teste < 0)
                        {
                            green = 0;
                        }
                        else if (teste > 255)
                        {
                            green = 255;
                        }
                        else
                        {
                            green = (byte)teste;
                        }

                        a = 2;
                        teste = (int)(Math.Abs((dataPtrcopy + nChan * (x - 1) + stepcopy * (y))[a] + 2 * (dataPtrcopy + nChan * (x - 1) + stepcopy * (y))[a] + (dataPtrcopy + nChan * (x - 1) + stepcopy * (y + 1))[a] - (dataPtrcopy + nChan * (x + 1) + stepcopy * (y))[a] - 2 * (dataPtrcopy + nChan * (x + 1) + stepcopy * (y))[a] - (dataPtrcopy + nChan * (x + 1) + stepcopy * (y + 1))[a])
                            + Math.Abs((dataPtrcopy + nChan * (x - 1) + stepcopy * (y + 1))[a] + 2 * (dataPtrcopy + nChan * (x) + stepcopy * (y + 1))[a] + (dataPtrcopy + nChan * (x + 1) + stepcopy * (y + 1))[a] - (dataPtrcopy + nChan * (x - 1) + stepcopy * (y))[a] - 2 * (dataPtrcopy + nChan * (x) + stepcopy * (y))[a] - (dataPtrcopy + nChan * (x + 1) + stepcopy * (y))[a]));

                        if (teste < 0)
                        {
                            red = 0;
                        }
                        else if (teste > 255)
                        {
                            red = 255;
                        }
                        else
                        {
                            red = (byte)teste;
                        }


                        (dataPtr + nChan * x + step * y)[0] = blue;
                        (dataPtr + nChan * x + step * y)[1] = green;
                        (dataPtr + nChan * x + step * y)[2] = red;

                        y = height - 1;

                        a = 0;
                        teste = (int)(Math.Abs((dataPtrcopy + nChan * (x - 1) + stepcopy * (y - 1))[a] + 2 * (dataPtrcopy + nChan * (x - 1) + stepcopy * (y))[a] + (dataPtrcopy + nChan * (x - 1) + stepcopy * (y))[a] - (dataPtrcopy + nChan * (x + 1) + stepcopy * (y - 1))[a] - 2 * (dataPtrcopy + nChan * (x + 1) + stepcopy * (y))[a] - (dataPtrcopy + nChan * (x + 1) + stepcopy * (y))[a])
                               + Math.Abs((dataPtrcopy + nChan * (x - 1) + stepcopy * (y))[a] + 2 * (dataPtrcopy + nChan * (x) + stepcopy * (y))[a] + (dataPtrcopy + nChan * (x + 1) + stepcopy * (y))[a] - (dataPtrcopy + nChan * (x - 1) + stepcopy * (y - 1))[a] - 2 * (dataPtrcopy + nChan * (x) + stepcopy * (y - 1))[a] - (dataPtrcopy + nChan * (x + 1) + stepcopy * (y - 1))[a]));

                        if (teste < 0)
                        {
                            blue = 0;
                        }
                        else if (teste > 255)
                        {
                            blue = 255;
                        }
                        else
                        {
                            blue = (byte)teste;
                        }

                        a = 1;
                        teste = (int)(Math.Abs((dataPtrcopy + nChan * (x - 1) + stepcopy * (y - 1))[a] + 2 * (dataPtrcopy + nChan * (x - 1) + stepcopy * (y))[a] + (dataPtrcopy + nChan * (x - 1) + stepcopy * (y))[a] - (dataPtrcopy + nChan * (x + 1) + stepcopy * (y - 1))[a] - 2 * (dataPtrcopy + nChan * (x + 1) + stepcopy * (y))[a] - (dataPtrcopy + nChan * (x + 1) + stepcopy * (y))[a])
                               + Math.Abs((dataPtrcopy + nChan * (x - 1) + stepcopy * (y))[a] + 2 * (dataPtrcopy + nChan * (x) + stepcopy * (y))[a] + (dataPtrcopy + nChan * (x + 1) + stepcopy * (y))[a] - (dataPtrcopy + nChan * (x - 1) + stepcopy * (y - 1))[a] - 2 * (dataPtrcopy + nChan * (x) + stepcopy * (y - 1))[a] - (dataPtrcopy + nChan * (x + 1) + stepcopy * (y - 1))[a]));

                        if (teste < 0)
                        {
                            green = 0;
                        }
                        else if (teste > 255)
                        {
                            green = 255;
                        }
                        else
                        {
                            green = (byte)teste;
                        }

                        a = 2;
                        teste = (int)(Math.Abs((dataPtrcopy + nChan * (x - 1) + stepcopy * (y - 1))[a] + 2 * (dataPtrcopy + nChan * (x - 1) + stepcopy * (y))[a] + (dataPtrcopy + nChan * (x - 1) + stepcopy * (y))[a] - (dataPtrcopy + nChan * (x + 1) + stepcopy * (y - 1))[a] - 2 * (dataPtrcopy + nChan * (x + 1) + stepcopy * (y))[a] - (dataPtrcopy + nChan * (x + 1) + stepcopy * (y))[a])
                               + Math.Abs((dataPtrcopy + nChan * (x - 1) + stepcopy * (y))[a] + 2 * (dataPtrcopy + nChan * (x) + stepcopy * (y))[a] + (dataPtrcopy + nChan * (x + 1) + stepcopy * (y))[a] - (dataPtrcopy + nChan * (x - 1) + stepcopy * (y - 1))[a] - 2 * (dataPtrcopy + nChan * (x) + stepcopy * (y - 1))[a] - (dataPtrcopy + nChan * (x + 1) + stepcopy * (y - 1))[a]));

                        if (teste < 0)
                        {
                            red = 0;
                        }
                        else if (teste > 255)
                        {
                            red = 255;
                        }
                        else
                        {
                            red = (byte)teste;
                        }

                        (dataPtr + nChan * x + step * y)[0] = blue;
                        (dataPtr + nChan * x + step * y)[1] = green;
                        (dataPtr + nChan * x + step * y)[2] = red;

                    }

                    for (y = 1; y < height - 1; y++)
                    {
                        x = 0;

                        a = 0;
                        teste = (int)(Math.Abs((dataPtrcopy + nChan * (x) + stepcopy * (y - 1))[a] + 2 * (dataPtrcopy + nChan * (x) + stepcopy * (y))[a] + (dataPtrcopy + nChan * (x) + stepcopy * (y + 1))[a] - (dataPtrcopy + nChan * (x + 1) + stepcopy * (y - 1))[a] - 2 * (dataPtrcopy + nChan * (x + 1) + stepcopy * (y))[a] - (dataPtrcopy + nChan * (x + 1) + stepcopy * (y + 1))[a])
                            + Math.Abs((dataPtrcopy + nChan * (x) + stepcopy * (y + 1))[a] + 2 * (dataPtrcopy + nChan * (x) + stepcopy * (y + 1))[a] + (dataPtrcopy + nChan * (x + 1) + stepcopy * (y + 1))[a] - (dataPtrcopy + nChan * (x) + stepcopy * (y - 1))[a] - 2 * (dataPtrcopy + nChan * (x) + stepcopy * (y - 1))[a] - (dataPtrcopy + nChan * (x + 1) + stepcopy * (y - 1))[a]));

                        if (teste < 0)
                        {
                            blue = 0;
                        }
                        else if (teste > 255)
                        {
                            blue = 255;
                        }
                        else
                        {
                            blue = (byte)teste;
                        }

                        a = 1;
                        teste = (int)(Math.Abs((dataPtrcopy + nChan * (x) + stepcopy * (y - 1))[a] + 2 * (dataPtrcopy + nChan * (x) + stepcopy * (y))[a] + (dataPtrcopy + nChan * (x) + stepcopy * (y + 1))[a] - (dataPtrcopy + nChan * (x + 1) + stepcopy * (y - 1))[a] - 2 * (dataPtrcopy + nChan * (x + 1) + stepcopy * (y))[a] - (dataPtrcopy + nChan * (x + 1) + stepcopy * (y + 1))[a])
                            + Math.Abs((dataPtrcopy + nChan * (x) + stepcopy * (y + 1))[a] + 2 * (dataPtrcopy + nChan * (x) + stepcopy * (y + 1))[a] + (dataPtrcopy + nChan * (x + 1) + stepcopy * (y + 1))[a] - (dataPtrcopy + nChan * (x) + stepcopy * (y - 1))[a] - 2 * (dataPtrcopy + nChan * (x) + stepcopy * (y - 1))[a] - (dataPtrcopy + nChan * (x + 1) + stepcopy * (y - 1))[a]));

                        if (teste < 0)
                        {
                            green = 0;
                        }
                        else if (teste > 255)
                        {
                            green = 255;
                        }
                        else
                        {
                            green = (byte)teste;
                        }


                        a = 2;
                        teste = (int)(Math.Abs((dataPtrcopy + nChan * (x) + stepcopy * (y - 1))[a] + 2 * (dataPtrcopy + nChan * (x) + stepcopy * (y))[a] + (dataPtrcopy + nChan * (x) + stepcopy * (y + 1))[a] - (dataPtrcopy + nChan * (x + 1) + stepcopy * (y - 1))[a] - 2 * (dataPtrcopy + nChan * (x + 1) + stepcopy * (y))[a] - (dataPtrcopy + nChan * (x + 1) + stepcopy * (y + 1))[a])
                            + Math.Abs((dataPtrcopy + nChan * (x) + stepcopy * (y + 1))[a] + 2 * (dataPtrcopy + nChan * (x) + stepcopy * (y + 1))[a] + (dataPtrcopy + nChan * (x + 1) + stepcopy * (y + 1))[a] - (dataPtrcopy + nChan * (x) + stepcopy * (y - 1))[a] - 2 * (dataPtrcopy + nChan * (x) + stepcopy * (y - 1))[a] - (dataPtrcopy + nChan * (x + 1) + stepcopy * (y - 1))[a]));

                        if (teste < 0)
                        {
                            red = 0;
                        }
                        else if (teste > 255)
                        {
                            red = 255;
                        }
                        else
                        {
                            red = (byte)teste;
                        }



                        (dataPtr + nChan * x + step * y)[0] = blue;
                        (dataPtr + nChan * x + step * y)[1] = green;
                        (dataPtr + nChan * x + step * y)[2] = red;



                        x = width - 1;

                        a = 0;
                        teste = (int)(Math.Abs((dataPtrcopy + nChan * (x - 1) + stepcopy * (y - 1))[a] + 2 * (dataPtrcopy + nChan * (x - 1) + stepcopy * (y))[a] + (dataPtrcopy + nChan * (x - 1) + stepcopy * (y + 1))[a] - (dataPtrcopy + nChan * (x) + stepcopy * (y - 1))[a] - 2 * (dataPtrcopy + nChan * (x) + stepcopy * (y))[a] - (dataPtrcopy + nChan * (x) + stepcopy * (y + 1))[a])
                            + Math.Abs((dataPtrcopy + nChan * (x - 1) + stepcopy * (y + 1))[a] + 2 * (dataPtrcopy + nChan * (x) + stepcopy * (y + 1))[a] + (dataPtrcopy + nChan * (x) + stepcopy * (y + 1))[a] - (dataPtrcopy + nChan * (x - 1) + stepcopy * (y - 1))[a] - 2 * (dataPtrcopy + nChan * (x) + stepcopy * (y - 1))[a] - (dataPtrcopy + nChan * (x) + stepcopy * (y - 1))[a]));

                        if (teste < 0)
                        {
                            blue = 0;
                        }
                        else if (teste > 255)
                        {
                            blue = 255;
                        }
                        else
                        {
                            blue = (byte)teste;
                        }

                        a = 1;
                        teste = (int)(Math.Abs((dataPtrcopy + nChan * (x - 1) + stepcopy * (y - 1))[a] + 2 * (dataPtrcopy + nChan * (x - 1) + stepcopy * (y))[a] + (dataPtrcopy + nChan * (x - 1) + stepcopy * (y + 1))[a] - (dataPtrcopy + nChan * (x) + stepcopy * (y - 1))[a] - 2 * (dataPtrcopy + nChan * (x) + stepcopy * (y))[a] - (dataPtrcopy + nChan * (x) + stepcopy * (y + 1))[a])
                            + Math.Abs((dataPtrcopy + nChan * (x - 1) + stepcopy * (y + 1))[a] + 2 * (dataPtrcopy + nChan * (x) + stepcopy * (y + 1))[a] + (dataPtrcopy + nChan * (x) + stepcopy * (y + 1))[a] - (dataPtrcopy + nChan * (x - 1) + stepcopy * (y - 1))[a] - 2 * (dataPtrcopy + nChan * (x) + stepcopy * (y - 1))[a] - (dataPtrcopy + nChan * (x) + stepcopy * (y - 1))[a]));

                        if (teste < 0)
                        {
                            green = 0;
                        }
                        else if (teste > 255)
                        {
                            green = 255;
                        }
                        else
                        {
                            green = (byte)teste;
                        }


                        a = 2;
                        teste = (int)(Math.Abs((dataPtrcopy + nChan * (x - 1) + stepcopy * (y - 1))[a] + 2 * (dataPtrcopy + nChan * (x - 1) + stepcopy * (y))[a] + (dataPtrcopy + nChan * (x - 1) + stepcopy * (y + 1))[a] - (dataPtrcopy + nChan * (x) + stepcopy * (y - 1))[a] - 2 * (dataPtrcopy + nChan * (x) + stepcopy * (y))[a] - (dataPtrcopy + nChan * (x) + stepcopy * (y + 1))[a])
                            + Math.Abs((dataPtrcopy + nChan * (x - 1) + stepcopy * (y + 1))[a] + 2 * (dataPtrcopy + nChan * (x) + stepcopy * (y + 1))[a] + (dataPtrcopy + nChan * (x) + stepcopy * (y + 1))[a] - (dataPtrcopy + nChan * (x - 1) + stepcopy * (y - 1))[a] - 2 * (dataPtrcopy + nChan * (x) + stepcopy * (y - 1))[a] - (dataPtrcopy + nChan * (x) + stepcopy * (y - 1))[a]));

                        if (teste < 0)
                        {
                            red = 0;
                        }
                        else if (teste > 255)
                        {
                            red = 255;
                        }
                        else
                        {
                            red = (byte)teste;
                        }


                        (dataPtr + nChan * x + step * y)[0] = blue;
                        (dataPtr + nChan * x + step * y)[1] = green;
                        (dataPtr + nChan * x + step * y)[2] = red;
                    }

                    // canto superior esquerdo
                    x = 0;
                    y = 0;

                    a = 0;
                    teste = (int)(Math.Abs((dataPtrcopy + nChan * (x) + stepcopy * (y))[a] + 2 * (dataPtrcopy + nChan * (x) + stepcopy * (y))[a] + (dataPtrcopy + nChan * (x) + stepcopy * (y + 1))[a] - (dataPtrcopy + nChan * (x + 1) + stepcopy * (y))[a] - 2 * (dataPtrcopy + nChan * (x + 1) + stepcopy * (y))[a] - (dataPtrcopy + nChan * (x + 1) + stepcopy * (y + 1))[a])
                        + Math.Abs((dataPtrcopy + nChan * (x) + stepcopy * (y + 1))[a] + 2 * (dataPtrcopy + nChan * (x) + stepcopy * (y + 1))[a] + (dataPtrcopy + nChan * (x + 1) + stepcopy * (y + 1))[a] - (dataPtrcopy + nChan * (x) + stepcopy * (y))[a] - 2 * (dataPtrcopy + nChan * (x) + stepcopy * (y))[a] - (dataPtrcopy + nChan * (x + 1) + stepcopy * (y))[a]));

                    if (teste < 0)
                    {
                        blue = 0;
                    }
                    else if (teste > 255)
                    {
                        blue = 255;
                    }
                    else
                    {
                        blue = (byte)teste;
                    }

                    a = 1;
                    teste = (int)(Math.Abs((dataPtrcopy + nChan * (x) + stepcopy * (y))[a] + 2 * (dataPtrcopy + nChan * (x) + stepcopy * (y))[a] + (dataPtrcopy + nChan * (x) + stepcopy * (y + 1))[a] - (dataPtrcopy + nChan * (x + 1) + stepcopy * (y))[a] - 2 * (dataPtrcopy + nChan * (x + 1) + stepcopy * (y))[a] - (dataPtrcopy + nChan * (x + 1) + stepcopy * (y + 1))[a])
                       + Math.Abs((dataPtrcopy + nChan * (x) + stepcopy * (y + 1))[a] + 2 * (dataPtrcopy + nChan * (x) + stepcopy * (y + 1))[a] + (dataPtrcopy + nChan * (x + 1) + stepcopy * (y + 1))[a] - (dataPtrcopy + nChan * (x) + stepcopy * (y))[a] - 2 * (dataPtrcopy + nChan * (x) + stepcopy * (y))[a] - (dataPtrcopy + nChan * (x + 1) + stepcopy * (y))[a]));

                    if (teste < 0)
                    {
                        green = 0;
                    }
                    else if (teste > 255)
                    {
                        green = 255;
                    }
                    else
                    {
                        green = (byte)teste;
                    }

                    a = 2;
                    teste = (int)(Math.Abs((dataPtrcopy + nChan * (x) + stepcopy * (y))[a] + 2 * (dataPtrcopy + nChan * (x) + stepcopy * (y))[a] + (dataPtrcopy + nChan * (x) + stepcopy * (y + 1))[a] - (dataPtrcopy + nChan * (x + 1) + stepcopy * (y))[a] - 2 * (dataPtrcopy + nChan * (x + 1) + stepcopy * (y))[a] - (dataPtrcopy + nChan * (x + 1) + stepcopy * (y + 1))[a])
                      + Math.Abs((dataPtrcopy + nChan * (x) + stepcopy * (y + 1))[a] + 2 * (dataPtrcopy + nChan * (x) + stepcopy * (y + 1))[a] + (dataPtrcopy + nChan * (x + 1) + stepcopy * (y + 1))[a] - (dataPtrcopy + nChan * (x) + stepcopy * (y))[a] - 2 * (dataPtrcopy + nChan * (x) + stepcopy * (y))[a] - (dataPtrcopy + nChan * (x + 1) + stepcopy * (y))[a]));

                    if (teste < 0)
                    {
                        red = 0;
                    }
                    else if (teste > 255)
                    {
                        red = 255;
                    }
                    else
                    {
                        red = (byte)teste;
                    }

                    (dataPtr + nChan * x + step * y)[0] = blue;
                    (dataPtr + nChan * x + step * y)[1] = green;
                    (dataPtr + nChan * x + step * y)[2] = red;

                    // canto superior direito

                    x = width - 1;
                    y = 0;

                    a = 0;
                    teste = (int)(Math.Abs((dataPtrcopy + nChan * (x - 1) + stepcopy * (y))[a] + 2 * (dataPtrcopy + nChan * (x - 1) + stepcopy * (y))[a] + (dataPtrcopy + nChan * (x - 1) + stepcopy * (y + 1))[a] - (dataPtrcopy + nChan * (x) + stepcopy * (y))[a] - 2 * (dataPtrcopy + nChan * (x) + stepcopy * (y))[a] - (dataPtrcopy + nChan * (x) + stepcopy * (y + 1))[a])
                        + Math.Abs((dataPtrcopy + nChan * (x - 1) + stepcopy * (y + 1))[a] + 2 * (dataPtrcopy + nChan * (x) + stepcopy * (y + 1))[a] + (dataPtrcopy + nChan * (x) + stepcopy * (y + 1))[a] - (dataPtrcopy + nChan * (x - 1) + stepcopy * (y))[a] - 2 * (dataPtrcopy + nChan * (x) + stepcopy * (y))[a] - (dataPtrcopy + nChan * (x) + stepcopy * (y))[a]));



                    if (teste < 0)
                    {
                        blue = 0;
                    }
                    else if (teste > 255)
                    {
                        blue = 255;
                    }
                    else
                    {
                        blue = (byte)teste;
                    }

                    a = 1;
                    teste = (int)(Math.Abs((dataPtrcopy + nChan * (x - 1) + stepcopy * (y))[a] + 2 * (dataPtrcopy + nChan * (x - 1) + stepcopy * (y))[a] + (dataPtrcopy + nChan * (x - 1) + stepcopy * (y + 1))[a] - (dataPtrcopy + nChan * (x) + stepcopy * (y))[a] - 2 * (dataPtrcopy + nChan * (x) + stepcopy * (y))[a] - (dataPtrcopy + nChan * (x) + stepcopy * (y + 1))[a])
                        + Math.Abs((dataPtrcopy + nChan * (x - 1) + stepcopy * (y + 1))[a] + 2 * (dataPtrcopy + nChan * (x) + stepcopy * (y + 1))[a] + (dataPtrcopy + nChan * (x) + stepcopy * (y + 1))[a] - (dataPtrcopy + nChan * (x - 1) + stepcopy * (y))[a] - 2 * (dataPtrcopy + nChan * (x) + stepcopy * (y))[a] - (dataPtrcopy + nChan * (x) + stepcopy * (y))[a]));


                    if (teste < 0)
                    {
                        green = 0;
                    }
                    else if (teste > 255)
                    {
                        green = 255;
                    }
                    else
                    {
                        green = (byte)teste;
                    }

                    a = 2;
                    teste = (int)(Math.Abs((dataPtrcopy + nChan * (x - 1) + stepcopy * (y))[a] + 2 * (dataPtrcopy + nChan * (x - 1) + stepcopy * (y))[a] + (dataPtrcopy + nChan * (x - 1) + stepcopy * (y + 1))[a] - (dataPtrcopy + nChan * (x) + stepcopy * (y))[a] - 2 * (dataPtrcopy + nChan * (x) + stepcopy * (y))[a] - (dataPtrcopy + nChan * (x) + stepcopy * (y + 1))[a])
                        + Math.Abs((dataPtrcopy + nChan * (x - 1) + stepcopy * (y + 1))[a] + 2 * (dataPtrcopy + nChan * (x) + stepcopy * (y + 1))[a] + (dataPtrcopy + nChan * (x) + stepcopy * (y + 1))[a] - (dataPtrcopy + nChan * (x - 1) + stepcopy * (y))[a] - 2 * (dataPtrcopy + nChan * (x) + stepcopy * (y))[a] - (dataPtrcopy + nChan * (x) + stepcopy * (y))[a]));


                    if (teste < 0)
                    {
                        red = 0;
                    }
                    else if (teste > 255)
                    {
                        red = 255;
                    }
                    else
                    {
                        red = (byte)teste;
                    }

                    (dataPtr + nChan * x + step * y)[0] = blue;
                    (dataPtr + nChan * x + step * y)[1] = green;
                    (dataPtr + nChan * x + step * y)[2] = red;

                    // canto inferior esquerdo

                    x = 0; y = height - 1;

                    a = 0;
                    teste = (int)(Math.Abs((dataPtrcopy + nChan * (x) + stepcopy * (y - 1))[a] + 2 * (dataPtrcopy + nChan * (x) + stepcopy * (y))[a] + (dataPtrcopy + nChan * (x) + stepcopy * (y))[a] - (dataPtrcopy + nChan * (x + 1) + stepcopy * (y - 1))[a] - 2 * (dataPtrcopy + nChan * (x + 1) + stepcopy * (y))[a] - (dataPtrcopy + nChan * (x + 1) + stepcopy * (y))[a])
                        + Math.Abs((dataPtrcopy + nChan * (x) + stepcopy * (y))[a] + 2 * (dataPtrcopy + nChan * (x) + stepcopy * (y))[a] + (dataPtrcopy + nChan * (x + 1) + stepcopy * (y))[a] - (dataPtrcopy + nChan * (x) + stepcopy * (y - 1))[a] - 2 * (dataPtrcopy + nChan * (x) + stepcopy * (y - 1))[a] - (dataPtrcopy + nChan * (x + 1) + stepcopy * (y - 1))[a]));


                    if (teste < 0)
                    {
                        blue = 0;
                    }
                    else if (teste > 255)
                    {
                        blue = 255;
                    }
                    else
                    {
                        blue = (byte)teste;
                    }

                    a = 1;
                    teste = (int)(Math.Abs((dataPtrcopy + nChan * (x) + stepcopy * (y - 1))[a] + 2 * (dataPtrcopy + nChan * (x) + stepcopy * (y))[a] + (dataPtrcopy + nChan * (x) + stepcopy * (y))[a] - (dataPtrcopy + nChan * (x + 1) + stepcopy * (y - 1))[a] - 2 * (dataPtrcopy + nChan * (x + 1) + stepcopy * (y))[a] - (dataPtrcopy + nChan * (x + 1) + stepcopy * (y))[a])
                        + Math.Abs((dataPtrcopy + nChan * (x) + stepcopy * (y))[a] + 2 * (dataPtrcopy + nChan * (x) + stepcopy * (y))[a] + (dataPtrcopy + nChan * (x + 1) + stepcopy * (y))[a] - (dataPtrcopy + nChan * (x) + stepcopy * (y - 1))[a] - 2 * (dataPtrcopy + nChan * (x) + stepcopy * (y - 1))[a] - (dataPtrcopy + nChan * (x + 1) + stepcopy * (y - 1))[a]));


                    if (teste < 0)
                    {
                        green = 0;
                    }
                    else if (teste > 255)
                    {
                        green = 255;
                    }
                    else
                    {
                        green = (byte)teste;
                    }


                    a = 2;
                    teste = (int)(Math.Abs((dataPtrcopy + nChan * (x) + stepcopy * (y - 1))[a] + 2 * (dataPtrcopy + nChan * (x) + stepcopy * (y))[a] + (dataPtrcopy + nChan * (x) + stepcopy * (y))[a] - (dataPtrcopy + nChan * (x + 1) + stepcopy * (y - 1))[a] - 2 * (dataPtrcopy + nChan * (x + 1) + stepcopy * (y))[a] - (dataPtrcopy + nChan * (x + 1) + stepcopy * (y))[a])
                        + Math.Abs((dataPtrcopy + nChan * (x) + stepcopy * (y))[a] + 2 * (dataPtrcopy + nChan * (x) + stepcopy * (y))[a] + (dataPtrcopy + nChan * (x + 1) + stepcopy * (y))[a] - (dataPtrcopy + nChan * (x) + stepcopy * (y - 1))[a] - 2 * (dataPtrcopy + nChan * (x) + stepcopy * (y - 1))[a] - (dataPtrcopy + nChan * (x + 1) + stepcopy * (y - 1))[a]));



                    if (teste < 0)
                    {
                        red = 0;
                    }
                    else if (teste > 255)
                    {
                        red = 255;
                    }
                    else
                    {
                        red = (byte)teste;
                    }






                    (dataPtr + nChan * x + step * y)[0] = blue;
                    (dataPtr + nChan * x + step * y)[1] = green;
                    (dataPtr + nChan * x + step * y)[2] = red;


                    // canto inferior direiro 

                    x = width - 1; y = height - 1;


                    a = 0;
                    teste = (int)(Math.Abs((dataPtrcopy + nChan * (x - 1) + stepcopy * (y - 1))[a] + 2 * (dataPtrcopy + nChan * (x - 1) + stepcopy * (y))[a] + (dataPtrcopy + nChan * (x - 1) + stepcopy * (y))[a] - (dataPtrcopy + nChan * (x) + stepcopy * (y - 1))[a] - 2 * (dataPtrcopy + nChan * (x) + stepcopy * (y))[a] - (dataPtrcopy + nChan * (x) + stepcopy * (y))[a])
                        + Math.Abs((dataPtrcopy + nChan * (x - 1) + stepcopy * (y))[a] + 2 * (dataPtrcopy + nChan * (x) + stepcopy * (y))[a] + (dataPtrcopy + nChan * (x) + stepcopy * (y))[a] - (dataPtrcopy + nChan * (x - 1) + stepcopy * (y - 1))[a] - 2 * (dataPtrcopy + nChan * (x) + stepcopy * (y - 1))[a] - (dataPtrcopy + nChan * (x) + stepcopy * (y - 1))[a]));

                    if (teste < 0)
                    {
                        blue = 0;
                    }
                    else if (teste > 255)
                    {
                        blue = 255;
                    }
                    else
                    {
                        blue = (byte)teste;
                    }


                    a = 1;
                    teste = (int)(Math.Abs((dataPtrcopy + nChan * (x - 1) + stepcopy * (y - 1))[a] + 2 * (dataPtrcopy + nChan * (x - 1) + stepcopy * (y))[a] + (dataPtrcopy + nChan * (x - 1) + stepcopy * (y))[a] - (dataPtrcopy + nChan * (x) + stepcopy * (y - 1))[a] - 2 * (dataPtrcopy + nChan * (x) + stepcopy * (y))[a] - (dataPtrcopy + nChan * (x) + stepcopy * (y))[a])
                        + Math.Abs((dataPtrcopy + nChan * (x - 1) + stepcopy * (y))[a] + 2 * (dataPtrcopy + nChan * (x) + stepcopy * (y))[a] + (dataPtrcopy + nChan * (x) + stepcopy * (y))[a] - (dataPtrcopy + nChan * (x - 1) + stepcopy * (y - 1))[a] - 2 * (dataPtrcopy + nChan * (x) + stepcopy * (y - 1))[a] - (dataPtrcopy + nChan * (x) + stepcopy * (y - 1))[a]));


                    if (teste < 0)
                    {
                        green = 0;
                    }
                    else if (teste > 255)
                    {
                        green = 255;
                    }
                    else
                    {
                        green = (byte)teste;
                    }

                    a = 2;
                    teste = (int)(Math.Abs((dataPtrcopy + nChan * (x - 1) + stepcopy * (y - 1))[a] + 2 * (dataPtrcopy + nChan * (x - 1) + stepcopy * (y))[a] + (dataPtrcopy + nChan * (x - 1) + stepcopy * (y))[a] - (dataPtrcopy + nChan * (x) + stepcopy * (y - 1))[a] - 2 * (dataPtrcopy + nChan * (x) + stepcopy * (y))[a] - (dataPtrcopy + nChan * (x) + stepcopy * (y))[a])
                        + Math.Abs((dataPtrcopy + nChan * (x - 1) + stepcopy * (y))[a] + 2 * (dataPtrcopy + nChan * (x) + stepcopy * (y))[a] + (dataPtrcopy + nChan * (x) + stepcopy * (y))[a] - (dataPtrcopy + nChan * (x - 1) + stepcopy * (y - 1))[a] - 2 * (dataPtrcopy + nChan * (x) + stepcopy * (y - 1))[a] - (dataPtrcopy + nChan * (x) + stepcopy * (y - 1))[a]));

                    if (teste < 0)
                    {
                        red = 0;
                    }
                    else if (teste > 255)
                    {
                        red = 255;
                    }
                    else
                    {
                        red = (byte)teste;
                    }


                    (dataPtr + nChan * x + step * y)[0] = blue;
                    (dataPtr + nChan * x + step * y)[1] = green;
                    (dataPtr + nChan * x + step * y)[2] = red;


                }
            }
        }

        public static void Diferentiation(Image<Bgr, byte> img, Image<Bgr, byte> imgCopy)
        {
            unsafe
            {
                // direct access to the image memory(sequencial)
                // direcion top left -> bottom right

                MIplImage m = img.MIplImage;
                MIplImage mcopy = imgCopy.MIplImage;
                byte* dataPtr = (byte*)m.imageData.ToPointer(); // Pointer to the image
                byte* dataPtrcopy = (byte*)mcopy.imageData.ToPointer(); // Pointer to the imagecopy
                byte blue, green, red;
                int step = m.widthStep;
                int stepcopy = mcopy.widthStep;
                int width = img.Width;
                int height = img.Height;
                int nChan = m.nChannels; // number of channels - 3
                int padding = m.widthStep - m.nChannels * m.width; // alinhament bytes (padding)
                int x, y;
                int teste = 0;
                int a;

                if (nChan == 3) // image in RGB
                {       // parte de dentro da imagem
                    for (y = 0; y < height - 1; y++)
                    {
                        for (x = 0; x < width - 1; x++)
                        {

                            a = 0;
                            teste = (int)(Math.Abs((dataPtrcopy + nChan * (x) + stepcopy * (y))[a] - (dataPtrcopy + nChan * (x + 1) + stepcopy * (y))[a])
                                + Math.Abs((dataPtrcopy + nChan * (x) + stepcopy * (y))[a] - (dataPtrcopy + nChan * (x) + stepcopy * (y + 1))[a]));

                            if (teste < 0)
                            {
                                blue = 0;
                            }
                            else if (teste > 255)
                            {
                                blue = 255;
                            }
                            else
                            {
                                blue = (byte)teste;
                            }

                            a = 1;
                            teste = (int)(Math.Abs((dataPtrcopy + nChan * (x) + stepcopy * (y))[a] - (dataPtrcopy + nChan * (x + 1) + stepcopy * (y))[a])
                                + Math.Abs((dataPtrcopy + nChan * (x) + stepcopy * (y))[a] - (dataPtrcopy + nChan * (x) + stepcopy * (y + 1))[a]));

                            if (teste < 0)
                            {
                                green = 0;
                            }
                            else if (teste > 255)
                            {
                                green = 255;
                            }
                            else
                            {
                                green = (byte)teste;
                            }

                            a = 2;
                            teste = (int)(Math.Abs((dataPtrcopy + nChan * (x) + stepcopy * (y))[a] - (dataPtrcopy + nChan * (x + 1) + stepcopy * (y))[a])
                                + Math.Abs((dataPtrcopy + nChan * (x) + stepcopy * (y))[a] - (dataPtrcopy + nChan * (x) + stepcopy * (y + 1))[a]));

                            if (teste < 0)
                            {
                                red = 0;
                            }
                            else if (teste > 255)
                            {
                                red = 255;
                            }
                            else
                            {
                                red = (byte)teste;
                            }




                            (dataPtr + nChan * x + step * y)[0] = blue;
                            (dataPtr + nChan * x + step * y)[1] = green;
                            (dataPtr + nChan * x + step * y)[2] = red;

                        }
                    }


                    for (x = 0; x < width - 1; x++)
                    {


                        y = height - 1;

                        a = 0;
                        teste = (int)(Math.Abs((dataPtrcopy + nChan * (x) + stepcopy * (y))[a] - (dataPtrcopy + nChan * (x + 1) + stepcopy * (y))[a]));


                        if (teste < 0)
                        {
                            blue = 0;
                        }
                        else if (teste > 255)
                        {
                            blue = 255;
                        }
                        else
                        {
                            blue = (byte)teste;
                        }

                        a = 1;
                        teste = (int)(Math.Abs((dataPtrcopy + nChan * (x) + stepcopy * (y))[a] - (dataPtrcopy + nChan * (x + 1) + stepcopy * (y))[a]));

                        if (teste < 0)
                        {
                            green = 0;
                        }
                        else if (teste > 255)
                        {
                            green = 255;
                        }
                        else
                        {
                            green = (byte)teste;
                        }

                        a = 2;
                        teste = (int)(Math.Abs((dataPtrcopy + nChan * (x) + stepcopy * (y))[a] - (dataPtrcopy + nChan * (x + 1) + stepcopy * (y))[a]));

                        if (teste < 0)
                        {
                            red = 0;
                        }
                        else if (teste > 255)
                        {
                            red = 255;
                        }
                        else
                        {
                            red = (byte)teste;
                        }



                        (dataPtr + nChan * x + step * y)[0] = blue;
                        (dataPtr + nChan * x + step * y)[1] = green;
                        (dataPtr + nChan * x + step * y)[2] = red;

                    }

                    for (y = 0; y < height - 1; y++)
                    {

                        x = width - 1;

                        a = 0;
                        teste = (int)Math.Abs((dataPtrcopy + nChan * (x) + stepcopy * (y))[a] - (dataPtrcopy + nChan * (x) + stepcopy * (y + 1))[a]);

                        if (teste < 0)
                        {
                            blue = 0;
                        }
                        else if (teste > 255)
                        {
                            blue = 255;
                        }
                        else
                        {
                            blue = (byte)teste;
                        }

                        a = 1;
                        teste = (int)Math.Abs((dataPtrcopy + nChan * (x) + stepcopy * (y))[a] - (dataPtrcopy + nChan * (x) + stepcopy * (y + 1))[a]);

                        if (teste < 0)
                        {
                            green = 0;
                        }
                        else if (teste > 255)
                        {
                            green = 255;
                        }
                        else
                        {
                            green = (byte)teste;
                        }

                        a = 2;
                        teste = (int)Math.Abs((dataPtrcopy + nChan * (x) + stepcopy * (y))[a] - (dataPtrcopy + nChan * (x) + stepcopy * (y + 1))[a]);

                        if (teste < 0)
                        {
                            red = 0;
                        }
                        else if (teste > 255)
                        {
                            red = 255;
                        }
                        else
                        {
                            red = (byte)teste;
                        }




                        (dataPtr + nChan * x + step * y)[0] = blue;
                        (dataPtr + nChan * x + step * y)[1] = green;
                        (dataPtr + nChan * x + step * y)[2] = red;
                    }






                    // canto inferior direiro 

                    x = width - 1; y = height - 1;




                    (dataPtr + nChan * x + step * y)[0] = 0;
                    (dataPtr + nChan * x + step * y)[1] = 0;
                    (dataPtr + nChan * x + step * y)[2] = 0;


                }
            }

        }

        public static void Roberts(Image<Bgr, byte> img, Image<Bgr, byte> imgCopy)
        {
            unsafe
            {
                // direct access to the image memory(sequencial)
                // direcion top left -> bottom right

                MIplImage m = img.MIplImage;
                MIplImage mcopy = imgCopy.MIplImage;
                byte* dataPtr = (byte*)m.imageData.ToPointer(); // Pointer to the image
                byte* dataPtrcopy = (byte*)mcopy.imageData.ToPointer(); // Pointer to the imagecopy
                byte blue, green, red;

                int step = m.widthStep;
                int stepcopy = mcopy.widthStep;
                int width = img.Width;
                int height = img.Height;
                int nChan = m.nChannels; // number of channels - 3
                int padding = m.widthStep - m.nChannels * m.width; // alinhament bytes (padding)
                int x, y;
                int teste = 0;
                int a;

                if (nChan == 3) // image in RGB
                {       // parte de dentro da imagem
                    for (y = 0; y < height - 1; y++)
                    {
                        for (x = 0; x < width - 1; x++)
                        {

                            a = 0;
                            teste = (int)(Math.Abs((dataPtrcopy + nChan * (x) + stepcopy * (y))[a] - (dataPtrcopy + nChan * (x + 1) + stepcopy * (y + 1))[a])
                                + Math.Abs((dataPtrcopy + nChan * (x + 1) + stepcopy * (y))[a] - (dataPtrcopy + nChan * (x) + stepcopy * (y + 1))[a]));

                            if (teste < 0)
                            {
                                blue = 0;
                            }
                            else if (teste > 255)
                            {
                                blue = 255;
                            }
                            else
                            {
                                blue = (byte)teste;
                            }

                            a = 1;
                            teste = (int)(Math.Abs((dataPtrcopy + nChan * (x) + stepcopy * (y))[a] - (dataPtrcopy + nChan * (x + 1) + stepcopy * (y + 1))[a])
                                + Math.Abs((dataPtrcopy + nChan * (x + 1) + stepcopy * (y))[a] - (dataPtrcopy + nChan * (x) + stepcopy * (y + 1))[a]));

                            if (teste < 0)
                            {
                                green = 0;
                            }
                            else if (teste > 255)
                            {
                                green = 255;
                            }
                            else
                            {
                                green = (byte)teste;
                            }

                            a = 2;
                            teste = (int)(Math.Abs((dataPtrcopy + nChan * (x) + stepcopy * (y))[a] - (dataPtrcopy + nChan * (x + 1) + stepcopy * (y + 1))[a])
                                + Math.Abs((dataPtrcopy + nChan * (x + 1) + stepcopy * (y))[a] - (dataPtrcopy + nChan * (x) + stepcopy * (y + 1))[a]));

                            if (teste < 0)
                            {
                                red = 0;
                            }
                            else if (teste > 255)
                            {
                                red = 255;
                            }
                            else
                            {
                                red = (byte)teste;
                            }




                            (dataPtr + nChan * x + step * y)[0] = blue;
                            (dataPtr + nChan * x + step * y)[1] = green;
                            (dataPtr + nChan * x + step * y)[2] = red;

                        }
                    }


                    for (x = 0; x < width - 1; x++)
                    {


                        y = height - 1;

                        a = 0;
                        teste = (int)(Math.Abs((dataPtrcopy + nChan * (x) + stepcopy * (y))[a] - (dataPtrcopy + nChan * (x + 1) + stepcopy * (y))[a])) * 2;


                        if (teste > 255)
                        {
                            blue = 255;
                        }
                        else
                        {
                            blue = (byte)teste;
                        }

                        a = 1;
                        teste = (int)(Math.Abs((dataPtrcopy + nChan * (x) + stepcopy * (y))[a] - (dataPtrcopy + nChan * (x + 1) + stepcopy * (y))[a])) * 2;

                        if (teste > 255)
                        {
                            green = 255;
                        }
                        else
                        {
                            green = (byte)teste;
                        }

                        a = 2;
                        teste = (int)(Math.Abs((dataPtrcopy + nChan * (x) + stepcopy * (y))[a] - (dataPtrcopy + nChan * (x + 1) + stepcopy * (y))[a])) * 2;

                        if (teste < 0)
                        {
                            red = 0;
                        }
                        else if (teste > 255)
                        {
                            red = 255;
                        }
                        else
                        {
                            red = (byte)teste;
                        }



                        (dataPtr + nChan * x + step * y)[0] = blue;
                        (dataPtr + nChan * x + step * y)[1] = green;
                        (dataPtr + nChan * x + step * y)[2] = red;

                    }

                    for (y = 0; y < height - 1; y++)
                    {

                        x = width - 1;

                        a = 0;
                        teste = (int)Math.Abs((dataPtrcopy + nChan * (x) + stepcopy * (y))[a] - (dataPtrcopy + nChan * (x) + stepcopy * (y + 1))[a]) * 2;

                        if (teste < 0)
                        {
                            blue = 0;
                        }
                        else if (teste > 255)
                        {
                            blue = 255;
                        }
                        else
                        {
                            blue = (byte)teste;
                        }

                        a = 1;
                        teste = (int)Math.Abs((dataPtrcopy + nChan * (x) + stepcopy * (y))[a] - (dataPtrcopy + nChan * (x) + stepcopy * (y + 1))[a]) * 2;

                        if (teste < 0)
                        {
                            green = 0;
                        }
                        else if (teste > 255)
                        {
                            green = 255;
                        }
                        else
                        {
                            green = (byte)teste;
                        }

                        a = 2;
                        teste = (int)Math.Abs((dataPtrcopy + nChan * (x) + stepcopy * (y))[a] - (dataPtrcopy + nChan * (x) + stepcopy * (y + 1))[a]) * 2;

                        if (teste < 0)
                        {
                            red = 0;
                        }
                        else if (teste > 255)
                        {
                            red = 255;
                        }
                        else
                        {
                            red = (byte)teste;
                        }




                        (dataPtr + nChan * x + step * y)[0] = blue;
                        (dataPtr + nChan * x + step * y)[1] = green;
                        (dataPtr + nChan * x + step * y)[2] = red;
                    }






                    // canto inferior direiro 

                    x = width - 1; y = height - 1;




                    (dataPtr + nChan * x + step * y)[0] = 0;
                    (dataPtr + nChan * x + step * y)[1] = 0;
                    (dataPtr + nChan * x + step * y)[2] = 0;


                }
            }
        }

        public static void Median(Image<Bgr, byte> img, Image<Bgr, byte> imgCopy)
        {
            unsafe
            {
                // direct access to the image memory(sequencial)
                // direcion top left -> bottom right

                MIplImage m = img.MIplImage;
                MIplImage mcopy = imgCopy.MIplImage;
                byte* dataPtr = (byte*)m.imageData.ToPointer(); // Pointer to the image
                byte* dataPtrcopy = (byte*)mcopy.imageData.ToPointer(); // Pointer to the imagecopy
                int step = m.widthStep;
                int stepcopy = mcopy.widthStep;
                int width = img.Width;
                int height = img.Height;
                int nChan = m.nChannels; // number of channels - 3
                int padding = m.widthStep - m.nChannels * m.width; // alinhament bytes (padding)
                int x, y;
                int xPicel = 0;
                int yPixel = 0;
                double[] distancias = new double[9];
                double intermedio = 0.0;
                int menorPessoacao;

                if (nChan == 3) // image in RGB
                {       // parte de dentro da imagem
                    for (y = 1; y < height - 1; y++)
                    {
                        for (x = 1; x < width - 1; x++)
                        {
                            // distancia pixei superior esquerdo
                            // guardar numa variavel o resultado e so depois é que guardar dentro o array

                            distancias[0] = 0;
                            distancias[1] = 0;
                            distancias[2] = 0;
                            distancias[3] = 0;
                            distancias[4] = 0;
                            distancias[5] = 0;
                            distancias[6] = 0;
                            distancias[7] = 0;
                            distancias[8] = 0;

                            xPicel = x - 1;
                            yPixel = y - 1;


                            intermedio = Math.Sqrt((Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[0] - (dataPtrcopy + nChan * (x - 0) + stepcopy * (y - 1))[0], 2)) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[1] - (dataPtrcopy + nChan * (x - 0) + stepcopy * (y - 1))[1], 2) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[2] - (dataPtrcopy + nChan * (x - 0) + stepcopy * (y - 1))[2], 2));
                            distancias[0] += intermedio;
                            distancias[1] += intermedio;


                            intermedio = Math.Sqrt((Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[0] - (dataPtrcopy + nChan * (x + 1) + stepcopy * (y - 1))[0], 2)) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[1] - (dataPtrcopy + nChan * (x + 1) + stepcopy * (y - 1))[1], 2) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[2] - (dataPtrcopy + nChan * (x + 1) + stepcopy * (y - 1))[2], 2));
                            distancias[0] += intermedio;
                            distancias[2] += intermedio;


                            intermedio = Math.Sqrt((Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[0] - (dataPtrcopy + nChan * (x - 1) + stepcopy * (y - 0))[0], 2)) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[1] - (dataPtrcopy + nChan * (x - 1) + stepcopy * (y - 0))[1], 2) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[2] - (dataPtrcopy + nChan * (x - 1) + stepcopy * (y - 0))[2], 2));
                            distancias[0] += intermedio;
                            distancias[3] += intermedio;


                            intermedio = Math.Sqrt((Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[0] - (dataPtrcopy + nChan * (x - 0) + stepcopy * (y - 0))[0], 2)) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[1] - (dataPtrcopy + nChan * (x - 0) + stepcopy * (y - 0))[1], 2) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[2] - (dataPtrcopy + nChan * (x - 0) + stepcopy * (y - 0))[2], 2));
                            distancias[0] += intermedio;
                            distancias[4] += intermedio;


                            intermedio = Math.Sqrt((Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[0] - (dataPtrcopy + nChan * (x + 1) + stepcopy * (y - 0))[0], 2)) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[1] - (dataPtrcopy + nChan * (x + 1) + stepcopy * (y - 0))[1], 2) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[2] - (dataPtrcopy + nChan * (x + 1) + stepcopy * (y - 0))[2], 2));
                            distancias[0] += intermedio;
                            distancias[5] += intermedio;


                            intermedio = Math.Sqrt((Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[0] - (dataPtrcopy + nChan * (x - 1) + stepcopy * (y + 1))[0], 2)) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[1] - (dataPtrcopy + nChan * (x - 1) + stepcopy * (y + 1))[1], 2) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[2] - (dataPtrcopy + nChan * (x - 1) + stepcopy * (y + 1))[2], 2));
                            distancias[0] += intermedio;
                            distancias[6] += intermedio;


                            intermedio = Math.Sqrt((Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[0] - (dataPtrcopy + nChan * (x - 0) + stepcopy * (y + 1))[0], 2)) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[1] - (dataPtrcopy + nChan * (x - 0) + stepcopy * (y + 1))[1], 2) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[2] - (dataPtrcopy + nChan * (x - 0) + stepcopy * (y + 1))[2], 2));
                            distancias[0] += intermedio;
                            distancias[7] += intermedio;


                            intermedio = Math.Sqrt((Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[0] - (dataPtrcopy + nChan * (x + 1) + stepcopy * (y + 1))[0], 2)) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[1] - (dataPtrcopy + nChan * (x + 1) + stepcopy * (y + 1))[1], 2) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[2] - (dataPtrcopy + nChan * (x + 1) + stepcopy * (y + 1))[2], 2));
                            distancias[0] += intermedio;
                            distancias[8] += intermedio;


                            // distancia pixei superior central

                            xPicel = x;
                            yPixel = y - 1;

                            intermedio = Math.Sqrt((Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[0] - (dataPtrcopy + nChan * (x + 1) + stepcopy * (y - 1))[0], 2)) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[1] - (dataPtrcopy + nChan * (x + 1) + stepcopy * (y - 1))[1], 2) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[2] - (dataPtrcopy + nChan * (x + 1) + stepcopy * (y - 1))[2], 2));
                            distancias[1] += intermedio;
                            distancias[2] += intermedio;


                            intermedio = Math.Sqrt((Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[0] - (dataPtrcopy + nChan * (x - 1) + stepcopy * (y - 0))[0], 2)) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[1] - (dataPtrcopy + nChan * (x - 1) + stepcopy * (y - 0))[1], 2) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[2] - (dataPtrcopy + nChan * (x - 1) + stepcopy * (y - 0))[2], 2));
                            distancias[1] += intermedio;
                            distancias[3] += intermedio;


                            intermedio = Math.Sqrt((Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[0] - (dataPtrcopy + nChan * (x - 0) + stepcopy * (y - 0))[0], 2)) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[1] - (dataPtrcopy + nChan * (x - 0) + stepcopy * (y - 0))[1], 2) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[2] - (dataPtrcopy + nChan * (x - 0) + stepcopy * (y - 0))[2], 2));
                            distancias[1] += intermedio;
                            distancias[4] += intermedio;


                            intermedio = Math.Sqrt((Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[0] - (dataPtrcopy + nChan * (x + 1) + stepcopy * (y - 0))[0], 2)) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[1] - (dataPtrcopy + nChan * (x + 1) + stepcopy * (y - 0))[1], 2) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[2] - (dataPtrcopy + nChan * (x + 1) + stepcopy * (y - 0))[2], 2));
                            distancias[1] += intermedio;
                            distancias[5] += intermedio;


                            intermedio = Math.Sqrt((Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[0] - (dataPtrcopy + nChan * (x - 1) + stepcopy * (y + 1))[0], 2)) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[1] - (dataPtrcopy + nChan * (x - 1) + stepcopy * (y + 1))[1], 2) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[2] - (dataPtrcopy + nChan * (x - 1) + stepcopy * (y + 1))[2], 2));
                            distancias[1] += intermedio;
                            distancias[6] += intermedio;


                            intermedio = Math.Sqrt((Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[0] - (dataPtrcopy + nChan * (x - 0) + stepcopy * (y + 1))[0], 2)) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[1] - (dataPtrcopy + nChan * (x - 0) + stepcopy * (y + 1))[1], 2) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[2] - (dataPtrcopy + nChan * (x - 0) + stepcopy * (y + 1))[2], 2));
                            distancias[1] += intermedio;
                            distancias[7] += intermedio;


                            intermedio = Math.Sqrt((Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[0] - (dataPtrcopy + nChan * (x + 1) + stepcopy * (y + 1))[0], 2)) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[1] - (dataPtrcopy + nChan * (x + 1) + stepcopy * (y + 1))[1], 2) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[2] - (dataPtrcopy + nChan * (x + 1) + stepcopy * (y + 1))[2], 2));
                            distancias[1] += intermedio;
                            distancias[8] += intermedio;


                            // distancia pixei direiro central

                            xPicel = x + 1;
                            yPixel = y - 1;

                            intermedio = Math.Sqrt((Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[0] - (dataPtrcopy + nChan * (x - 1) + stepcopy * (y - 0))[0], 2)) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[1] - (dataPtrcopy + nChan * (x - 1) + stepcopy * (y - 0))[1], 2) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[2] - (dataPtrcopy + nChan * (x - 1) + stepcopy * (y - 0))[2], 2));
                            distancias[2] += intermedio;
                            distancias[3] += intermedio;


                            intermedio = Math.Sqrt((Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[0] - (dataPtrcopy + nChan * (x - 0) + stepcopy * (y - 0))[0], 2)) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[1] - (dataPtrcopy + nChan * (x - 0) + stepcopy * (y - 0))[1], 2) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[2] - (dataPtrcopy + nChan * (x - 0) + stepcopy * (y - 0))[2], 2));
                            distancias[2] += intermedio;
                            distancias[4] += intermedio;


                            intermedio = Math.Sqrt((Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[0] - (dataPtrcopy + nChan * (x + 1) + stepcopy * (y - 0))[0], 2)) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[1] - (dataPtrcopy + nChan * (x + 1) + stepcopy * (y - 0))[1], 2) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[2] - (dataPtrcopy + nChan * (x + 1) + stepcopy * (y - 0))[2], 2));
                            distancias[2] += intermedio;
                            distancias[5] += intermedio;


                            intermedio = Math.Sqrt((Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[0] - (dataPtrcopy + nChan * (x - 1) + stepcopy * (y + 1))[0], 2)) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[1] - (dataPtrcopy + nChan * (x - 1) + stepcopy * (y + 1))[1], 2) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[2] - (dataPtrcopy + nChan * (x - 1) + stepcopy * (y + 1))[2], 2));
                            distancias[2] += intermedio;
                            distancias[6] += intermedio;


                            intermedio = Math.Sqrt((Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[0] - (dataPtrcopy + nChan * (x - 0) + stepcopy * (y + 1))[0], 2)) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[1] - (dataPtrcopy + nChan * (x - 0) + stepcopy * (y + 1))[1], 2) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[2] - (dataPtrcopy + nChan * (x - 0) + stepcopy * (y + 1))[2], 2));
                            distancias[2] += intermedio;
                            distancias[7] += intermedio;


                            intermedio = Math.Sqrt((Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[0] - (dataPtrcopy + nChan * (x + 1) + stepcopy * (y + 1))[0], 2)) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[1] - (dataPtrcopy + nChan * (x + 1) + stepcopy * (y + 1))[1], 2) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[2] - (dataPtrcopy + nChan * (x + 1) + stepcopy * (y + 1))[2], 2));
                            distancias[2] += intermedio;
                            distancias[8] += intermedio;

                            // distancia pixei centro esquerdo

                            xPicel = x - 1;
                            yPixel = y;

                            intermedio = Math.Sqrt((Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[0] - (dataPtrcopy + nChan * (x - 0) + stepcopy * (y - 0))[0], 2)) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[1] - (dataPtrcopy + nChan * (x - 0) + stepcopy * (y - 0))[1], 2) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[2] - (dataPtrcopy + nChan * (x - 0) + stepcopy * (y - 0))[2], 2));
                            distancias[3] += intermedio;
                            distancias[4] += intermedio;


                            intermedio = Math.Sqrt((Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[0] - (dataPtrcopy + nChan * (x + 1) + stepcopy * (y - 0))[0], 2)) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[1] - (dataPtrcopy + nChan * (x + 1) + stepcopy * (y - 0))[1], 2) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[2] - (dataPtrcopy + nChan * (x + 1) + stepcopy * (y - 0))[2], 2));
                            distancias[3] += intermedio;
                            distancias[5] += intermedio;


                            intermedio = Math.Sqrt((Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[0] - (dataPtrcopy + nChan * (x - 1) + stepcopy * (y + 1))[0], 2)) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[1] - (dataPtrcopy + nChan * (x - 1) + stepcopy * (y + 1))[1], 2) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[2] - (dataPtrcopy + nChan * (x - 1) + stepcopy * (y + 1))[2], 2));
                            distancias[3] += intermedio;
                            distancias[6] += intermedio;


                            intermedio = Math.Sqrt((Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[0] - (dataPtrcopy + nChan * (x - 0) + stepcopy * (y + 1))[0], 2)) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[1] - (dataPtrcopy + nChan * (x - 0) + stepcopy * (y + 1))[1], 2) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[2] - (dataPtrcopy + nChan * (x - 0) + stepcopy * (y + 1))[2], 2));
                            distancias[3] += intermedio;
                            distancias[7] += intermedio;


                            intermedio = Math.Sqrt((Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[0] - (dataPtrcopy + nChan * (x + 1) + stepcopy * (y + 1))[0], 2)) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[1] - (dataPtrcopy + nChan * (x + 1) + stepcopy * (y + 1))[1], 2) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[2] - (dataPtrcopy + nChan * (x + 1) + stepcopy * (y + 1))[2], 2));
                            distancias[3] += intermedio;
                            distancias[8] += intermedio;


                            // distancia pixei centro centro
                            // 
                            xPicel = x;
                            yPixel = y;


                            intermedio = Math.Sqrt((Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[0] - (dataPtrcopy + nChan * (x + 1) + stepcopy * (y - 0))[0], 2)) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[1] - (dataPtrcopy + nChan * (x + 1) + stepcopy * (y - 0))[1], 2) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[2] - (dataPtrcopy + nChan * (x + 1) + stepcopy * (y - 0))[2], 2));
                            distancias[4] += intermedio;
                            distancias[5] += intermedio;


                            intermedio = Math.Sqrt((Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[0] - (dataPtrcopy + nChan * (x - 1) + stepcopy * (y + 1))[0], 2)) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[1] - (dataPtrcopy + nChan * (x - 1) + stepcopy * (y + 1))[1], 2) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[2] - (dataPtrcopy + nChan * (x - 1) + stepcopy * (y + 1))[2], 2));
                            distancias[4] += intermedio;
                            distancias[6] += intermedio;


                            intermedio = Math.Sqrt((Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[0] - (dataPtrcopy + nChan * (x - 0) + stepcopy * (y + 1))[0], 2)) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[1] - (dataPtrcopy + nChan * (x - 0) + stepcopy * (y + 1))[1], 2) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[2] - (dataPtrcopy + nChan * (x - 0) + stepcopy * (y + 1))[2], 2));
                            distancias[4] += intermedio;
                            distancias[7] += intermedio;


                            intermedio = Math.Sqrt((Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[0] - (dataPtrcopy + nChan * (x + 1) + stepcopy * (y + 1))[0], 2)) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[1] - (dataPtrcopy + nChan * (x + 1) + stepcopy * (y + 1))[1], 2) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[2] - (dataPtrcopy + nChan * (x + 1) + stepcopy * (y + 1))[2], 2));
                            distancias[4] += intermedio;
                            distancias[8] += intermedio;


                            // distancia pixei centro direita
                            // 
                            xPicel = x + 1;
                            yPixel = y;

                            intermedio = Math.Sqrt((Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[0] - (dataPtrcopy + nChan * (x - 1) + stepcopy * (y + 1))[0], 2)) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[1] - (dataPtrcopy + nChan * (x - 1) + stepcopy * (y + 1))[1], 2) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[2] - (dataPtrcopy + nChan * (x - 1) + stepcopy * (y + 1))[2], 2));
                            distancias[5] += intermedio;
                            distancias[6] += intermedio;


                            intermedio = Math.Sqrt((Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[0] - (dataPtrcopy + nChan * (x - 0) + stepcopy * (y + 1))[0], 2)) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[1] - (dataPtrcopy + nChan * (x - 0) + stepcopy * (y + 1))[1], 2) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[2] - (dataPtrcopy + nChan * (x - 0) + stepcopy * (y + 1))[2], 2));
                            distancias[5] += intermedio;
                            distancias[7] += intermedio;


                            intermedio = Math.Sqrt((Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[0] - (dataPtrcopy + nChan * (x + 1) + stepcopy * (y + 1))[0], 2)) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[1] - (dataPtrcopy + nChan * (x + 1) + stepcopy * (y + 1))[1], 2) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[2] - (dataPtrcopy + nChan * (x + 1) + stepcopy * (y + 1))[2], 2));
                            distancias[5] += intermedio;
                            distancias[8] += intermedio;

                            // distancia pixei baixo esquerda

                            xPicel = x - 1;
                            yPixel = y + 1;

                            intermedio = Math.Sqrt((Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[0] - (dataPtrcopy + nChan * (x - 0) + stepcopy * (y + 1))[0], 2)) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[1] - (dataPtrcopy + nChan * (x - 0) + stepcopy * (y + 1))[1], 2) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[2] - (dataPtrcopy + nChan * (x - 0) + stepcopy * (y + 1))[2], 2));
                            distancias[6] += intermedio;
                            distancias[7] += intermedio;


                            intermedio = Math.Sqrt((Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[0] - (dataPtrcopy + nChan * (x + 1) + stepcopy * (y + 1))[0], 2)) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[1] - (dataPtrcopy + nChan * (x + 1) + stepcopy * (y + 1))[1], 2) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[2] - (dataPtrcopy + nChan * (x + 1) + stepcopy * (y + 1))[2], 2));
                            distancias[6] += intermedio;
                            distancias[8] += intermedio;


                            // distancia pixei baixo centro

                            xPicel = x;
                            yPixel = y + 1;

                            intermedio = Math.Sqrt((Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[0] - (dataPtrcopy + nChan * (x + 1) + stepcopy * (y + 1))[0], 2)) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[1] - (dataPtrcopy + nChan * (x + 1) + stepcopy * (y + 1))[1], 2) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[2] - (dataPtrcopy + nChan * (x + 1) + stepcopy * (y + 1))[2], 2));
                            distancias[7] += intermedio;
                            distancias[8] += intermedio;

                            menorPessoacao = Array.IndexOf(distancias, distancias.Min());

                            switch (menorPessoacao)
                            {
                                case 0:
                                    (dataPtr + nChan * x + step * y)[0] = (dataPtrcopy + nChan * (x - 1) + stepcopy * (y - 1))[0];
                                    (dataPtr + nChan * x + step * y)[1] = (dataPtrcopy + nChan * (x - 1) + stepcopy * (y - 1))[1];
                                    (dataPtr + nChan * x + step * y)[2] = (dataPtrcopy + nChan * (x - 1) + stepcopy * (y - 1))[2];
                                    break;
                                case 1:
                                    (dataPtr + nChan * x + step * y)[0] = (dataPtrcopy + nChan * x + stepcopy * (y - 1))[0];
                                    (dataPtr + nChan * x + step * y)[1] = (dataPtrcopy + nChan * x + stepcopy * (y - 1))[1];
                                    (dataPtr + nChan * x + step * y)[2] = (dataPtrcopy + nChan * x + stepcopy * (y - 1))[2];
                                    break;
                                case 2:
                                    (dataPtr + nChan * x + step * y)[0] = (dataPtrcopy + nChan * (x + 1) + stepcopy * (y - 1))[0];
                                    (dataPtr + nChan * x + step * y)[1] = (dataPtrcopy + nChan * (x + 1) + stepcopy * (y - 1))[1];
                                    (dataPtr + nChan * x + step * y)[2] = (dataPtrcopy + nChan * (x + 1) + stepcopy * (y - 1))[2];
                                    break;
                                case 3:
                                    (dataPtr + nChan * x + step * y)[0] = (dataPtrcopy + nChan * (x - 1) + stepcopy * y)[0];
                                    (dataPtr + nChan * x + step * y)[1] = (dataPtrcopy + nChan * (x - 1) + stepcopy * y)[1];
                                    (dataPtr + nChan * x + step * y)[2] = (dataPtrcopy + nChan * (x - 1) + stepcopy * y)[2];
                                    break;
                                case 4:
                                    (dataPtr + nChan * x + step * y)[0] = (dataPtrcopy + nChan * x + stepcopy * y)[0];
                                    (dataPtr + nChan * x + step * y)[1] = (dataPtrcopy + nChan * x + stepcopy * y)[1];
                                    (dataPtr + nChan * x + step * y)[2] = (dataPtrcopy + nChan * x + stepcopy * y)[2];
                                    break;
                                case 5:
                                    (dataPtr + nChan * x + step * y)[0] = (dataPtrcopy + nChan * (x + 1) + stepcopy * y)[0];
                                    (dataPtr + nChan * x + step * y)[1] = (dataPtrcopy + nChan * (x + 1) + stepcopy * y)[1];
                                    (dataPtr + nChan * x + step * y)[2] = (dataPtrcopy + nChan * (x + 1) + stepcopy * y)[2];
                                    break;
                                case 6:
                                    (dataPtr + nChan * x + step * y)[0] = (dataPtrcopy + nChan * (x - 1) + stepcopy * (y + 1))[0];
                                    (dataPtr + nChan * x + step * y)[1] = (dataPtrcopy + nChan * (x - 1) + stepcopy * (y + 1))[1];
                                    (dataPtr + nChan * x + step * y)[2] = (dataPtrcopy + nChan * (x - 1) + stepcopy * (y + 1))[2];
                                    break;
                                case 7:
                                    (dataPtr + nChan * x + step * y)[0] = (dataPtrcopy + nChan * x + stepcopy * (y + 1))[0];
                                    (dataPtr + nChan * x + step * y)[1] = (dataPtrcopy + nChan * x + stepcopy * (y + 1))[1];
                                    (dataPtr + nChan * x + step * y)[2] = (dataPtrcopy + nChan * x + stepcopy * (y + 1))[2];
                                    break;
                                case 8:
                                    (dataPtr + nChan * x + step * y)[0] = (dataPtrcopy + nChan * (x + 1) + stepcopy * (y + 1))[0];
                                    (dataPtr + nChan * x + step * y)[1] = (dataPtrcopy + nChan * (x + 1) + stepcopy * (y + 1))[1];
                                    (dataPtr + nChan * x + step * y)[2] = (dataPtrcopy + nChan * (x + 1) + stepcopy * (y + 1))[2];
                                    break;



                            }

                        }

                    }

                    for (x = 1; x < width - 1; x++)
                    {

                        y = 0;

                        distancias[0] = 0;
                        distancias[1] = 0;
                        distancias[2] = 0;
                        distancias[3] = 0;
                        distancias[4] = 0;
                        distancias[5] = 0;
                        distancias[6] = 0;
                        distancias[7] = 0;
                        distancias[8] = 0;

                        xPicel = x - 1;
                        yPixel = y;


                        intermedio = Math.Sqrt((Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[0] - (dataPtrcopy + nChan * (x - 0) + stepcopy * (y - 0))[0], 2)) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[1] - (dataPtrcopy + nChan * (x - 0) + stepcopy * (y - 0))[1], 2) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[2] - (dataPtrcopy + nChan * (x - 0) + stepcopy * (y - 0))[2], 2));
                        distancias[0] += intermedio;
                        distancias[1] += intermedio;
                        distancias[0] += intermedio;
                        distancias[4] += intermedio;
                        distancias[3] += intermedio;
                        distancias[4] += intermedio;


                        intermedio = Math.Sqrt((Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[0] - (dataPtrcopy + nChan * (x + 1) + stepcopy * (y - 0))[0], 2)) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[1] - (dataPtrcopy + nChan * (x + 1) + stepcopy * (y - 0))[1], 2) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[2] - (dataPtrcopy + nChan * (x + 1) + stepcopy * (y - 0))[2], 2));
                        distancias[0] += intermedio;
                        distancias[2] += intermedio;
                        distancias[0] += intermedio;
                        distancias[5] += intermedio;
                        distancias[3] += intermedio;
                        distancias[5] += intermedio;


                        intermedio = Math.Sqrt((Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[0] - (dataPtrcopy + nChan * (x - 1) + stepcopy * (y - 0))[0], 2)) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[1] - (dataPtrcopy + nChan * (x - 1) + stepcopy * (y - 0))[1], 2) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[2] - (dataPtrcopy + nChan * (x - 1) + stepcopy * (y - 0))[2], 2));
                        distancias[0] += intermedio;
                        distancias[3] += intermedio;



                        intermedio = Math.Sqrt((Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[0] - (dataPtrcopy + nChan * (x - 1) + stepcopy * (y + 1))[0], 2)) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[1] - (dataPtrcopy + nChan * (x - 1) + stepcopy * (y + 1))[1], 2) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[2] - (dataPtrcopy + nChan * (x - 1) + stepcopy * (y + 1))[2], 2));
                        distancias[0] += intermedio;
                        distancias[6] += intermedio;
                        distancias[3] += intermedio;
                        distancias[6] += intermedio;


                        intermedio = Math.Sqrt((Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[0] - (dataPtrcopy + nChan * (x - 0) + stepcopy * (y + 1))[0], 2)) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[1] - (dataPtrcopy + nChan * (x - 0) + stepcopy * (y + 1))[1], 2) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[2] - (dataPtrcopy + nChan * (x - 0) + stepcopy * (y + 1))[2], 2));
                        distancias[0] += intermedio;
                        distancias[7] += intermedio;
                        distancias[3] += intermedio;
                        distancias[7] += intermedio;


                        intermedio = Math.Sqrt((Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[0] - (dataPtrcopy + nChan * (x + 1) + stepcopy * (y + 1))[0], 2)) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[1] - (dataPtrcopy + nChan * (x + 1) + stepcopy * (y + 1))[1], 2) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[2] - (dataPtrcopy + nChan * (x + 1) + stepcopy * (y + 1))[2], 2));
                        distancias[0] += intermedio;
                        distancias[8] += intermedio;
                        distancias[3] += intermedio;
                        distancias[8] += intermedio;


                        // distancia pixei superior central

                        xPicel = x;
                        yPixel = y;

                        intermedio = Math.Sqrt((Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[0] - (dataPtrcopy + nChan * (x + 1) + stepcopy * (y - 0))[0], 2)) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[1] - (dataPtrcopy + nChan * (x + 1) + stepcopy * (y - 0))[1], 2) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[2] - (dataPtrcopy + nChan * (x + 1) + stepcopy * (y - 0))[2], 2));
                        distancias[1] += intermedio;
                        distancias[2] += intermedio;
                        distancias[1] += intermedio;
                        distancias[5] += intermedio;
                        distancias[4] += intermedio;
                        distancias[5] += intermedio;


                        intermedio = Math.Sqrt((Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[0] - (dataPtrcopy + nChan * (x - 1) + stepcopy * (y - 0))[0], 2)) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[1] - (dataPtrcopy + nChan * (x - 1) + stepcopy * (y - 0))[1], 2) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[2] - (dataPtrcopy + nChan * (x - 1) + stepcopy * (y - 0))[2], 2));
                        distancias[1] += intermedio;
                        distancias[3] += intermedio;


                        intermedio = Math.Sqrt((Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[0] - (dataPtrcopy + nChan * (x - 0) + stepcopy * (y - 0))[0], 2)) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[1] - (dataPtrcopy + nChan * (x - 0) + stepcopy * (y - 0))[1], 2) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[2] - (dataPtrcopy + nChan * (x - 0) + stepcopy * (y - 0))[2], 2));
                        distancias[1] += intermedio;
                        distancias[4] += intermedio;


                        intermedio = Math.Sqrt((Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[0] - (dataPtrcopy + nChan * (x - 1) + stepcopy * (y + 1))[0], 2)) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[1] - (dataPtrcopy + nChan * (x - 1) + stepcopy * (y + 1))[1], 2) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[2] - (dataPtrcopy + nChan * (x - 1) + stepcopy * (y + 1))[2], 2));
                        distancias[1] += intermedio;
                        distancias[6] += intermedio;
                        distancias[4] += intermedio;
                        distancias[6] += intermedio;


                        intermedio = Math.Sqrt((Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[0] - (dataPtrcopy + nChan * (x - 0) + stepcopy * (y + 1))[0], 2)) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[1] - (dataPtrcopy + nChan * (x - 0) + stepcopy * (y + 1))[1], 2) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[2] - (dataPtrcopy + nChan * (x - 0) + stepcopy * (y + 1))[2], 2));
                        distancias[1] += intermedio;
                        distancias[7] += intermedio;
                        distancias[4] += intermedio;
                        distancias[7] += intermedio;


                        intermedio = Math.Sqrt((Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[0] - (dataPtrcopy + nChan * (x + 1) + stepcopy * (y + 1))[0], 2)) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[1] - (dataPtrcopy + nChan * (x + 1) + stepcopy * (y + 1))[1], 2) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[2] - (dataPtrcopy + nChan * (x + 1) + stepcopy * (y + 1))[2], 2));
                        distancias[1] += intermedio;
                        distancias[8] += intermedio;
                        distancias[4] += intermedio;
                        distancias[8] += intermedio;


                        // distancia pixei direiro central

                        xPicel = x + 1;
                        yPixel = y;

                        intermedio = Math.Sqrt((Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[0] - (dataPtrcopy + nChan * (x - 1) + stepcopy * (y - 0))[0], 2)) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[1] - (dataPtrcopy + nChan * (x - 1) + stepcopy * (y - 0))[1], 2) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[2] - (dataPtrcopy + nChan * (x - 1) + stepcopy * (y - 0))[2], 2));
                        distancias[2] += intermedio;
                        distancias[3] += intermedio;


                        intermedio = Math.Sqrt((Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[0] - (dataPtrcopy + nChan * (x - 0) + stepcopy * (y - 0))[0], 2)) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[1] - (dataPtrcopy + nChan * (x - 0) + stepcopy * (y - 0))[1], 2) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[2] - (dataPtrcopy + nChan * (x - 0) + stepcopy * (y - 0))[2], 2));
                        distancias[2] += intermedio;
                        distancias[4] += intermedio;


                        intermedio = Math.Sqrt((Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[0] - (dataPtrcopy + nChan * (x + 1) + stepcopy * (y - 0))[0], 2)) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[1] - (dataPtrcopy + nChan * (x + 1) + stepcopy * (y - 0))[1], 2) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[2] - (dataPtrcopy + nChan * (x + 1) + stepcopy * (y - 0))[2], 2));
                        distancias[2] += intermedio;
                        distancias[5] += intermedio;


                        intermedio = Math.Sqrt((Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[0] - (dataPtrcopy + nChan * (x - 1) + stepcopy * (y + 1))[0], 2)) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[1] - (dataPtrcopy + nChan * (x - 1) + stepcopy * (y + 1))[1], 2) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[2] - (dataPtrcopy + nChan * (x - 1) + stepcopy * (y + 1))[2], 2));
                        distancias[2] += intermedio;
                        distancias[6] += intermedio;
                        distancias[5] += intermedio;
                        distancias[6] += intermedio;


                        intermedio = Math.Sqrt((Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[0] - (dataPtrcopy + nChan * (x - 0) + stepcopy * (y + 1))[0], 2)) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[1] - (dataPtrcopy + nChan * (x - 0) + stepcopy * (y + 1))[1], 2) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[2] - (dataPtrcopy + nChan * (x - 0) + stepcopy * (y + 1))[2], 2));
                        distancias[2] += intermedio;
                        distancias[7] += intermedio;
                        distancias[5] += intermedio;
                        distancias[7] += intermedio;


                        intermedio = Math.Sqrt((Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[0] - (dataPtrcopy + nChan * (x + 1) + stepcopy * (y + 1))[0], 2)) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[1] - (dataPtrcopy + nChan * (x + 1) + stepcopy * (y + 1))[1], 2) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[2] - (dataPtrcopy + nChan * (x + 1) + stepcopy * (y + 1))[2], 2));
                        distancias[2] += intermedio;
                        distancias[8] += intermedio;
                        distancias[5] += intermedio;
                        distancias[8] += intermedio;


                        // distancia pixei baixo esquerda

                        xPicel = x - 1;
                        yPixel = y + 1;

                        intermedio = Math.Sqrt((Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[0] - (dataPtrcopy + nChan * (x - 0) + stepcopy * (y + 1))[0], 2)) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[1] - (dataPtrcopy + nChan * (x - 0) + stepcopy * (y + 1))[1], 2) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[2] - (dataPtrcopy + nChan * (x - 0) + stepcopy * (y + 1))[2], 2));
                        distancias[6] += intermedio;
                        distancias[7] += intermedio;


                        intermedio = Math.Sqrt((Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[0] - (dataPtrcopy + nChan * (x + 1) + stepcopy * (y + 1))[0], 2)) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[1] - (dataPtrcopy + nChan * (x + 1) + stepcopy * (y + 1))[1], 2) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[2] - (dataPtrcopy + nChan * (x + 1) + stepcopy * (y + 1))[2], 2));
                        distancias[6] += intermedio;
                        distancias[8] += intermedio;


                        // distancia pixei baixo centro

                        xPicel = x;
                        yPixel = y + 1;

                        intermedio = Math.Sqrt((Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[0] - (dataPtrcopy + nChan * (x + 1) + stepcopy * (y + 1))[0], 2)) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[1] - (dataPtrcopy + nChan * (x + 1) + stepcopy * (y + 1))[1], 2) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[2] - (dataPtrcopy + nChan * (x + 1) + stepcopy * (y + 1))[2], 2));
                        distancias[7] += intermedio;
                        distancias[8] += intermedio;

                        menorPessoacao = Array.IndexOf(distancias, distancias.Min());

                        switch (menorPessoacao)
                        {
                            case 0:
                                (dataPtr + nChan * x + step * y)[0] = (dataPtrcopy + nChan * (x - 1) + stepcopy * y)[0];
                                (dataPtr + nChan * x + step * y)[1] = (dataPtrcopy + nChan * (x - 1) + stepcopy * y)[1];
                                (dataPtr + nChan * x + step * y)[2] = (dataPtrcopy + nChan * (x - 1) + stepcopy * y)[2];
                                break;
                            case 1:
                                (dataPtr + nChan * x + step * y)[0] = (dataPtrcopy + nChan * x + stepcopy * y)[0];
                                (dataPtr + nChan * x + step * y)[1] = (dataPtrcopy + nChan * x + stepcopy * y)[1];
                                (dataPtr + nChan * x + step * y)[2] = (dataPtrcopy + nChan * x + stepcopy * y)[2];
                                break;
                            case 2:
                                (dataPtr + nChan * x + step * y)[0] = (dataPtrcopy + nChan * (x + 1) + stepcopy * y)[0];
                                (dataPtr + nChan * x + step * y)[1] = (dataPtrcopy + nChan * (x + 1) + stepcopy * y)[1];
                                (dataPtr + nChan * x + step * y)[2] = (dataPtrcopy + nChan * (x + 1) + stepcopy * y)[2];
                                break;
                            case 3:
                                (dataPtr + nChan * x + step * y)[0] = (dataPtrcopy + nChan * (x - 1) + stepcopy * y)[0];
                                (dataPtr + nChan * x + step * y)[1] = (dataPtrcopy + nChan * (x - 1) + stepcopy * y)[1];
                                (dataPtr + nChan * x + step * y)[2] = (dataPtrcopy + nChan * (x - 1) + stepcopy * y)[2];
                                break;
                            case 4:
                                (dataPtr + nChan * x + step * y)[0] = (dataPtrcopy + nChan * x + stepcopy * y)[0];
                                (dataPtr + nChan * x + step * y)[1] = (dataPtrcopy + nChan * x + stepcopy * y)[1];
                                (dataPtr + nChan * x + step * y)[2] = (dataPtrcopy + nChan * x + stepcopy * y)[2];
                                break;
                            case 5:
                                (dataPtr + nChan * x + step * y)[0] = (dataPtrcopy + nChan * (x + 1) + stepcopy * y)[0];
                                (dataPtr + nChan * x + step * y)[1] = (dataPtrcopy + nChan * (x + 1) + stepcopy * y)[1];
                                (dataPtr + nChan * x + step * y)[2] = (dataPtrcopy + nChan * (x + 1) + stepcopy * y)[2];
                                break;
                            case 6:
                                (dataPtr + nChan * x + step * y)[0] = (dataPtrcopy + nChan * (x - 1) + stepcopy * (y + 1))[0];
                                (dataPtr + nChan * x + step * y)[1] = (dataPtrcopy + nChan * (x - 1) + stepcopy * (y + 1))[1];
                                (dataPtr + nChan * x + step * y)[2] = (dataPtrcopy + nChan * (x - 1) + stepcopy * (y + 1))[2];
                                break;
                            case 7:
                                (dataPtr + nChan * x + step * y)[0] = (dataPtrcopy + nChan * x + stepcopy * (y + 1))[0];
                                (dataPtr + nChan * x + step * y)[1] = (dataPtrcopy + nChan * x + stepcopy * (y + 1))[1];
                                (dataPtr + nChan * x + step * y)[2] = (dataPtrcopy + nChan * x + stepcopy * (y + 1))[2];
                                break;
                            case 8:
                                (dataPtr + nChan * x + step * y)[0] = (dataPtrcopy + nChan * (x + 1) + stepcopy * (y + 1))[0];
                                (dataPtr + nChan * x + step * y)[1] = (dataPtrcopy + nChan * (x + 1) + stepcopy * (y + 1))[1];
                                (dataPtr + nChan * x + step * y)[2] = (dataPtrcopy + nChan * (x + 1) + stepcopy * (y + 1))[2];
                                break;

                        }



                        y = height - 1;


                        distancias[0] = 0;
                        distancias[1] = 0;
                        distancias[2] = 0;
                        distancias[3] = 0;
                        distancias[4] = 0;
                        distancias[5] = 0;
                        distancias[6] = 0;
                        distancias[7] = 0;
                        distancias[8] = 0;

                        xPicel = x - 1;
                        yPixel = y - 1;


                        intermedio = Math.Sqrt((Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[0] - (dataPtrcopy + nChan * (x - 0) + stepcopy * (y - 1))[0], 2)) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[1] - (dataPtrcopy + nChan * (x - 0) + stepcopy * (y - 1))[1], 2) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[2] - (dataPtrcopy + nChan * (x - 0) + stepcopy * (y - 1))[2], 2));
                        distancias[0] += intermedio;
                        distancias[1] += intermedio;


                        intermedio = Math.Sqrt((Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[0] - (dataPtrcopy + nChan * (x + 1) + stepcopy * (y - 1))[0], 2)) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[1] - (dataPtrcopy + nChan * (x + 1) + stepcopy * (y - 1))[1], 2) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[2] - (dataPtrcopy + nChan * (x + 1) + stepcopy * (y - 1))[2], 2));
                        distancias[0] += intermedio;
                        distancias[2] += intermedio;


                        intermedio = Math.Sqrt((Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[0] - (dataPtrcopy + nChan * (x - 1) + stepcopy * (y - 0))[0], 2)) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[1] - (dataPtrcopy + nChan * (x - 1) + stepcopy * (y - 0))[1], 2) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[2] - (dataPtrcopy + nChan * (x - 1) + stepcopy * (y - 0))[2], 2));
                        distancias[0] += intermedio;
                        distancias[3] += intermedio;
                        distancias[0] += intermedio;
                        distancias[6] += intermedio;


                        intermedio = Math.Sqrt((Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[0] - (dataPtrcopy + nChan * (x - 0) + stepcopy * (y - 0))[0], 2)) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[1] - (dataPtrcopy + nChan * (x - 0) + stepcopy * (y - 0))[1], 2) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[2] - (dataPtrcopy + nChan * (x - 0) + stepcopy * (y - 0))[2], 2));
                        distancias[0] += intermedio;
                        distancias[4] += intermedio;
                        distancias[0] += intermedio;
                        distancias[7] += intermedio;


                        intermedio = Math.Sqrt((Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[0] - (dataPtrcopy + nChan * (x + 1) + stepcopy * (y - 0))[0], 2)) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[1] - (dataPtrcopy + nChan * (x + 1) + stepcopy * (y - 0))[1], 2) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[2] - (dataPtrcopy + nChan * (x + 1) + stepcopy * (y - 0))[2], 2));
                        distancias[0] += intermedio;
                        distancias[5] += intermedio;
                        distancias[0] += intermedio;
                        distancias[8] += intermedio;


                        // distancia pixei superior central

                        xPicel = x;
                        yPixel = y - 1;

                        intermedio = Math.Sqrt((Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[0] - (dataPtrcopy + nChan * (x + 1) + stepcopy * (y - 1))[0], 2)) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[1] - (dataPtrcopy + nChan * (x + 1) + stepcopy * (y - 1))[1], 2) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[2] - (dataPtrcopy + nChan * (x + 1) + stepcopy * (y - 1))[2], 2));
                        distancias[1] += intermedio;
                        distancias[2] += intermedio;


                        intermedio = Math.Sqrt((Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[0] - (dataPtrcopy + nChan * (x - 1) + stepcopy * (y - 0))[0], 2)) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[1] - (dataPtrcopy + nChan * (x - 1) + stepcopy * (y - 0))[1], 2) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[2] - (dataPtrcopy + nChan * (x - 1) + stepcopy * (y - 0))[2], 2));
                        distancias[1] += intermedio;
                        distancias[3] += intermedio;
                        distancias[1] += intermedio;
                        distancias[6] += intermedio;


                        intermedio = Math.Sqrt((Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[0] - (dataPtrcopy + nChan * (x - 0) + stepcopy * (y - 0))[0], 2)) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[1] - (dataPtrcopy + nChan * (x - 0) + stepcopy * (y - 0))[1], 2) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[2] - (dataPtrcopy + nChan * (x - 0) + stepcopy * (y - 0))[2], 2));
                        distancias[1] += intermedio;
                        distancias[4] += intermedio;
                        distancias[1] += intermedio;
                        distancias[7] += intermedio;


                        intermedio = Math.Sqrt((Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[0] - (dataPtrcopy + nChan * (x + 1) + stepcopy * (y - 0))[0], 2)) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[1] - (dataPtrcopy + nChan * (x + 1) + stepcopy * (y - 0))[1], 2) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[2] - (dataPtrcopy + nChan * (x + 1) + stepcopy * (y - 0))[2], 2));
                        distancias[1] += intermedio;
                        distancias[5] += intermedio;
                        distancias[1] += intermedio;
                        distancias[8] += intermedio;


                        // distancia pixei direiro central

                        xPicel = x + 1;
                        yPixel = y - 1;

                        intermedio = Math.Sqrt((Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[0] - (dataPtrcopy + nChan * (x - 1) + stepcopy * (y - 0))[0], 2)) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[1] - (dataPtrcopy + nChan * (x - 1) + stepcopy * (y - 0))[1], 2) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[2] - (dataPtrcopy + nChan * (x - 1) + stepcopy * (y - 0))[2], 2));
                        distancias[2] += intermedio;
                        distancias[3] += intermedio;
                        distancias[2] += intermedio;
                        distancias[6] += intermedio;


                        intermedio = Math.Sqrt((Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[0] - (dataPtrcopy + nChan * (x - 0) + stepcopy * (y - 0))[0], 2)) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[1] - (dataPtrcopy + nChan * (x - 0) + stepcopy * (y - 0))[1], 2) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[2] - (dataPtrcopy + nChan * (x - 0) + stepcopy * (y - 0))[2], 2));
                        distancias[2] += intermedio;
                        distancias[4] += intermedio;
                        distancias[2] += intermedio;
                        distancias[7] += intermedio;


                        intermedio = Math.Sqrt((Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[0] - (dataPtrcopy + nChan * (x + 1) + stepcopy * (y - 0))[0], 2)) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[1] - (dataPtrcopy + nChan * (x + 1) + stepcopy * (y - 0))[1], 2) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[2] - (dataPtrcopy + nChan * (x + 1) + stepcopy * (y - 0))[2], 2));
                        distancias[2] += intermedio;
                        distancias[5] += intermedio;
                        distancias[2] += intermedio;
                        distancias[8] += intermedio;

                        // distancia pixei centro esquerdo

                        xPicel = x - 1;
                        yPixel = y;

                        intermedio = Math.Sqrt((Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[0] - (dataPtrcopy + nChan * (x - 0) + stepcopy * (y - 0))[0], 2)) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[1] - (dataPtrcopy + nChan * (x - 0) + stepcopy * (y - 0))[1], 2) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[2] - (dataPtrcopy + nChan * (x - 0) + stepcopy * (y - 0))[2], 2));
                        distancias[3] += intermedio;
                        distancias[4] += intermedio;
                        distancias[3] += intermedio;
                        distancias[7] += intermedio;
                        distancias[6] += intermedio;
                        distancias[7] += intermedio;


                        intermedio = Math.Sqrt((Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[0] - (dataPtrcopy + nChan * (x + 1) + stepcopy * (y - 0))[0], 2)) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[1] - (dataPtrcopy + nChan * (x + 1) + stepcopy * (y - 0))[1], 2) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[2] - (dataPtrcopy + nChan * (x + 1) + stepcopy * (y - 0))[2], 2));
                        distancias[3] += intermedio;
                        distancias[5] += intermedio;
                        distancias[3] += intermedio;
                        distancias[8] += intermedio;
                        distancias[6] += intermedio;
                        distancias[8] += intermedio;


                        intermedio = Math.Sqrt((Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[0] - (dataPtrcopy + nChan * (x - 1) + stepcopy * (y + 0))[0], 2)) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[1] - (dataPtrcopy + nChan * (x - 1) + stepcopy * (y + 0))[1], 2) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[2] - (dataPtrcopy + nChan * (x - 1) + stepcopy * (y + 0))[2], 2));
                        distancias[3] += intermedio;
                        distancias[6] += intermedio;



                        // distancia pixei centro centro
                        // 
                        xPicel = x;
                        yPixel = y;


                        intermedio = Math.Sqrt((Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[0] - (dataPtrcopy + nChan * (x + 1) + stepcopy * (y - 0))[0], 2)) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[1] - (dataPtrcopy + nChan * (x + 1) + stepcopy * (y - 0))[1], 2) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[2] - (dataPtrcopy + nChan * (x + 1) + stepcopy * (y - 0))[2], 2));
                        distancias[4] += intermedio;
                        distancias[5] += intermedio;
                        distancias[4] += intermedio;
                        distancias[8] += intermedio;
                        distancias[7] += intermedio;
                        distancias[8] += intermedio;


                        intermedio = Math.Sqrt((Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[0] - (dataPtrcopy + nChan * (x - 1) + stepcopy * (y + 0))[0], 2)) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[1] - (dataPtrcopy + nChan * (x - 1) + stepcopy * (y + 0))[1], 2) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[2] - (dataPtrcopy + nChan * (x - 1) + stepcopy * (y + 0))[2], 2));
                        distancias[4] += intermedio;
                        distancias[6] += intermedio;


                        intermedio = Math.Sqrt((Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[0] - (dataPtrcopy + nChan * (x - 0) + stepcopy * (y + 0))[0], 2)) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[1] - (dataPtrcopy + nChan * (x - 0) + stepcopy * (y + 0))[1], 2) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[2] - (dataPtrcopy + nChan * (x - 0) + stepcopy * (y + 0))[2], 2));
                        distancias[4] += intermedio;
                        distancias[7] += intermedio;





                        // distancia pixei centro direita
                        // 
                        xPicel = x + 1;
                        yPixel = y;

                        intermedio = Math.Sqrt((Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[0] - (dataPtrcopy + nChan * (x - 1) + stepcopy * (y + 0))[0], 2)) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[1] - (dataPtrcopy + nChan * (x - 1) + stepcopy * (y + 0))[1], 2) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[2] - (dataPtrcopy + nChan * (x - 1) + stepcopy * (y + 0))[2], 2));
                        distancias[5] += intermedio;
                        distancias[6] += intermedio;


                        intermedio = Math.Sqrt((Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[0] - (dataPtrcopy + nChan * (x - 0) + stepcopy * (y + 0))[0], 2)) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[1] - (dataPtrcopy + nChan * (x - 0) + stepcopy * (y + 0))[1], 2) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[2] - (dataPtrcopy + nChan * (x - 0) + stepcopy * (y + 0))[2], 2));
                        distancias[5] += intermedio;
                        distancias[7] += intermedio;


                        intermedio = Math.Sqrt((Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[0] - (dataPtrcopy + nChan * (x + 1) + stepcopy * (y + 0))[0], 2)) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[1] - (dataPtrcopy + nChan * (x + 1) + stepcopy * (y + 0))[1], 2) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[2] - (dataPtrcopy + nChan * (x + 1) + stepcopy * (y + 0))[2], 2));
                        distancias[5] += intermedio;
                        distancias[8] += intermedio;




                        menorPessoacao = Array.IndexOf(distancias, distancias.Min());

                        switch (menorPessoacao)
                        {
                            case 0:
                                (dataPtr + nChan * x + step * y)[0] = (dataPtrcopy + nChan * (x - 1) + stepcopy * (y - 1))[0];
                                (dataPtr + nChan * x + step * y)[1] = (dataPtrcopy + nChan * (x - 1) + stepcopy * (y - 1))[1];
                                (dataPtr + nChan * x + step * y)[2] = (dataPtrcopy + nChan * (x - 1) + stepcopy * (y - 1))[2];
                                break;
                            case 1:
                                (dataPtr + nChan * x + step * y)[0] = (dataPtrcopy + nChan * x + stepcopy * (y - 1))[0];
                                (dataPtr + nChan * x + step * y)[1] = (dataPtrcopy + nChan * x + stepcopy * (y - 1))[1];
                                (dataPtr + nChan * x + step * y)[2] = (dataPtrcopy + nChan * x + stepcopy * (y - 1))[2];
                                break;
                            case 2:
                                (dataPtr + nChan * x + step * y)[0] = (dataPtrcopy + nChan * (x + 1) + stepcopy * (y - 1))[0];
                                (dataPtr + nChan * x + step * y)[1] = (dataPtrcopy + nChan * (x + 1) + stepcopy * (y - 1))[1];
                                (dataPtr + nChan * x + step * y)[2] = (dataPtrcopy + nChan * (x + 1) + stepcopy * (y - 1))[2];
                                break;
                            case 3:
                                (dataPtr + nChan * x + step * y)[0] = (dataPtrcopy + nChan * (x - 1) + stepcopy * y)[0];
                                (dataPtr + nChan * x + step * y)[1] = (dataPtrcopy + nChan * (x - 1) + stepcopy * y)[1];
                                (dataPtr + nChan * x + step * y)[2] = (dataPtrcopy + nChan * (x - 1) + stepcopy * y)[2];
                                break;
                            case 4:
                                (dataPtr + nChan * x + step * y)[0] = (dataPtrcopy + nChan * x + stepcopy * y)[0];
                                (dataPtr + nChan * x + step * y)[1] = (dataPtrcopy + nChan * x + stepcopy * y)[1];
                                (dataPtr + nChan * x + step * y)[2] = (dataPtrcopy + nChan * x + stepcopy * y)[2];
                                break;
                            case 5:
                                (dataPtr + nChan * x + step * y)[0] = (dataPtrcopy + nChan * (x + 1) + stepcopy * y)[0];
                                (dataPtr + nChan * x + step * y)[1] = (dataPtrcopy + nChan * (x + 1) + stepcopy * y)[1];
                                (dataPtr + nChan * x + step * y)[2] = (dataPtrcopy + nChan * (x + 1) + stepcopy * y)[2];
                                break;
                            case 6:
                                (dataPtr + nChan * x + step * y)[0] = (dataPtrcopy + nChan * (x - 1) + stepcopy * y)[0];
                                (dataPtr + nChan * x + step * y)[1] = (dataPtrcopy + nChan * (x - 1) + stepcopy * y)[1];
                                (dataPtr + nChan * x + step * y)[2] = (dataPtrcopy + nChan * (x - 1) + stepcopy * y)[2];
                                break;
                            case 7:
                                (dataPtr + nChan * x + step * y)[0] = (dataPtrcopy + nChan * x + stepcopy * y)[0];
                                (dataPtr + nChan * x + step * y)[1] = (dataPtrcopy + nChan * x + stepcopy * y)[1];
                                (dataPtr + nChan * x + step * y)[2] = (dataPtrcopy + nChan * x + stepcopy * y)[2];
                                break;
                            case 8:
                                (dataPtr + nChan * x + step * y)[0] = (dataPtrcopy + nChan * (x + 1) + stepcopy * y)[0];
                                (dataPtr + nChan * x + step * y)[1] = (dataPtrcopy + nChan * (x + 1) + stepcopy * y)[1];
                                (dataPtr + nChan * x + step * y)[2] = (dataPtrcopy + nChan * (x + 1) + stepcopy * y)[2];
                                break;

                        }
                    }

                    for (y = 1; y < height - 1; y++)
                    {
                        x = 0;

                        // distancia pixei superior esquerdo
                        // guardar numa variavel o resultado e so depois é que guardar dentro o array

                        distancias[0] = 0;
                        distancias[1] = 0;
                        distancias[2] = 0;
                        distancias[3] = 0;
                        distancias[4] = 0;
                        distancias[5] = 0;
                        distancias[6] = 0;
                        distancias[7] = 0;
                        distancias[8] = 0;

                        xPicel = x;
                        yPixel = y - 1;


                        intermedio = Math.Sqrt((Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[0] - (dataPtrcopy + nChan * (x - 0) + stepcopy * (y - 1))[0], 2)) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[1] - (dataPtrcopy + nChan * (x - 0) + stepcopy * (y - 1))[1], 2) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[2] - (dataPtrcopy + nChan * (x - 0) + stepcopy * (y - 1))[2], 2));
                        distancias[0] += intermedio;
                        distancias[1] += intermedio;


                        intermedio = Math.Sqrt((Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[0] - (dataPtrcopy + nChan * (x + 1) + stepcopy * (y - 1))[0], 2)) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[1] - (dataPtrcopy + nChan * (x + 1) + stepcopy * (y - 1))[1], 2) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[2] - (dataPtrcopy + nChan * (x + 1) + stepcopy * (y - 1))[2], 2));
                        distancias[0] += intermedio;
                        distancias[2] += intermedio;
                        distancias[1] += intermedio;
                        distancias[2] += intermedio;


                        intermedio = Math.Sqrt((Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[0] - (dataPtrcopy + nChan * (x - 0) + stepcopy * (y - 0))[0], 2)) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[1] - (dataPtrcopy + nChan * (x - 0) + stepcopy * (y - 0))[1], 2) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[2] - (dataPtrcopy + nChan * (x - 0) + stepcopy * (y - 0))[2], 2));
                        distancias[0] += intermedio;
                        distancias[3] += intermedio;
                        distancias[0] += intermedio;
                        distancias[4] += intermedio;
                        distancias[1] += intermedio;
                        distancias[3] += intermedio;
                        distancias[1] += intermedio;
                        distancias[4] += intermedio;


                        intermedio = Math.Sqrt((Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[0] - (dataPtrcopy + nChan * (x + 1) + stepcopy * (y - 0))[0], 2)) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[1] - (dataPtrcopy + nChan * (x + 1) + stepcopy * (y - 0))[1], 2) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[2] - (dataPtrcopy + nChan * (x + 1) + stepcopy * (y - 0))[2], 2));
                        distancias[0] += intermedio;
                        distancias[5] += intermedio;
                        distancias[1] += intermedio;
                        distancias[5] += intermedio;


                        intermedio = Math.Sqrt((Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[0] - (dataPtrcopy + nChan * (x - 0) + stepcopy * (y + 1))[0], 2)) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[1] - (dataPtrcopy + nChan * (x - 0) + stepcopy * (y + 1))[1], 2) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[2] - (dataPtrcopy + nChan * (x - 0) + stepcopy * (y + 1))[2], 2));
                        distancias[0] += intermedio;
                        distancias[6] += intermedio;
                        distancias[0] += intermedio;
                        distancias[7] += intermedio;
                        distancias[1] += intermedio;
                        distancias[6] += intermedio;
                        distancias[1] += intermedio;
                        distancias[7] += intermedio;





                        intermedio = Math.Sqrt((Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[0] - (dataPtrcopy + nChan * (x + 1) + stepcopy * (y + 1))[0], 2)) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[1] - (dataPtrcopy + nChan * (x + 1) + stepcopy * (y + 1))[1], 2) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[2] - (dataPtrcopy + nChan * (x + 1) + stepcopy * (y + 1))[2], 2));
                        distancias[0] += intermedio;
                        distancias[8] += intermedio;
                        distancias[1] += intermedio;
                        distancias[8] += intermedio;



                        // distancia pixei direiro central

                        xPicel = x + 1;
                        yPixel = y - 1;

                        intermedio = Math.Sqrt((Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[0] - (dataPtrcopy + nChan * (x - 0) + stepcopy * (y - 0))[0], 2)) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[1] - (dataPtrcopy + nChan * (x - 0) + stepcopy * (y - 0))[1], 2) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[2] - (dataPtrcopy + nChan * (x - 0) + stepcopy * (y - 0))[2], 2));
                        distancias[2] += intermedio;
                        distancias[3] += intermedio;
                        distancias[2] += intermedio;
                        distancias[4] += intermedio;


                        intermedio = Math.Sqrt((Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[0] - (dataPtrcopy + nChan * (x + 1) + stepcopy * (y - 0))[0], 2)) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[1] - (dataPtrcopy + nChan * (x + 1) + stepcopy * (y - 0))[1], 2) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[2] - (dataPtrcopy + nChan * (x + 1) + stepcopy * (y - 0))[2], 2));
                        distancias[2] += intermedio;
                        distancias[5] += intermedio;


                        intermedio = Math.Sqrt((Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[0] - (dataPtrcopy + nChan * (x - 0) + stepcopy * (y + 1))[0], 2)) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[1] - (dataPtrcopy + nChan * (x - 0) + stepcopy * (y + 1))[1], 2) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[2] - (dataPtrcopy + nChan * (x - 0) + stepcopy * (y + 1))[2], 2));
                        distancias[2] += intermedio;
                        distancias[6] += intermedio;
                        distancias[2] += intermedio;
                        distancias[7] += intermedio;



                        intermedio = Math.Sqrt((Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[0] - (dataPtrcopy + nChan * (x + 1) + stepcopy * (y + 1))[0], 2)) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[1] - (dataPtrcopy + nChan * (x + 1) + stepcopy * (y + 1))[1], 2) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[2] - (dataPtrcopy + nChan * (x + 1) + stepcopy * (y + 1))[2], 2));
                        distancias[2] += intermedio;
                        distancias[8] += intermedio;

                        // distancia pixei centro esquerdo

                        xPicel = x;
                        yPixel = y;

                        intermedio = Math.Sqrt((Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[0] - (dataPtrcopy + nChan * (x - 0) + stepcopy * (y - 0))[0], 2)) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[1] - (dataPtrcopy + nChan * (x - 0) + stepcopy * (y - 0))[1], 2) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[2] - (dataPtrcopy + nChan * (x - 0) + stepcopy * (y - 0))[2], 2));
                        distancias[3] += intermedio;
                        distancias[4] += intermedio;


                        intermedio = Math.Sqrt((Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[0] - (dataPtrcopy + nChan * (x + 1) + stepcopy * (y - 0))[0], 2)) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[1] - (dataPtrcopy + nChan * (x + 1) + stepcopy * (y - 0))[1], 2) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[2] - (dataPtrcopy + nChan * (x + 1) + stepcopy * (y - 0))[2], 2));
                        distancias[3] += intermedio;
                        distancias[5] += intermedio;
                        distancias[4] += intermedio;
                        distancias[5] += intermedio;


                        intermedio = Math.Sqrt((Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[0] - (dataPtrcopy + nChan * (x - 0) + stepcopy * (y + 1))[0], 2)) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[1] - (dataPtrcopy + nChan * (x - 0) + stepcopy * (y + 1))[1], 2) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[2] - (dataPtrcopy + nChan * (x - 0) + stepcopy * (y + 1))[2], 2));
                        distancias[3] += intermedio;
                        distancias[6] += intermedio;
                        distancias[3] += intermedio;
                        distancias[7] += intermedio;
                        distancias[4] += intermedio;
                        distancias[6] += intermedio;
                        distancias[4] += intermedio;
                        distancias[7] += intermedio;



                        intermedio = Math.Sqrt((Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[0] - (dataPtrcopy + nChan * (x + 1) + stepcopy * (y + 1))[0], 2)) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[1] - (dataPtrcopy + nChan * (x + 1) + stepcopy * (y + 1))[1], 2) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[2] - (dataPtrcopy + nChan * (x + 1) + stepcopy * (y + 1))[2], 2));
                        distancias[3] += intermedio;
                        distancias[8] += intermedio;
                        distancias[4] += intermedio;
                        distancias[8] += intermedio;



                        // distancia pixei centro direita
                        // 
                        xPicel = x + 1;
                        yPixel = y;

                        intermedio = Math.Sqrt((Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[0] - (dataPtrcopy + nChan * (x - 0) + stepcopy * (y + 1))[0], 2)) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[1] - (dataPtrcopy + nChan * (x - 0) + stepcopy * (y + 1))[1], 2) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[2] - (dataPtrcopy + nChan * (x - 0) + stepcopy * (y + 1))[2], 2));
                        distancias[5] += intermedio;
                        distancias[6] += intermedio;
                        distancias[5] += intermedio;
                        distancias[7] += intermedio;



                        intermedio = Math.Sqrt((Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[0] - (dataPtrcopy + nChan * (x + 1) + stepcopy * (y + 1))[0], 2)) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[1] - (dataPtrcopy + nChan * (x + 1) + stepcopy * (y + 1))[1], 2) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[2] - (dataPtrcopy + nChan * (x + 1) + stepcopy * (y + 1))[2], 2));
                        distancias[5] += intermedio;
                        distancias[8] += intermedio;

                        // distancia pixei baixo esquerda

                        xPicel = x;
                        yPixel = y + 1;

                        intermedio = Math.Sqrt((Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[0] - (dataPtrcopy + nChan * (x - 0) + stepcopy * (y + 1))[0], 2)) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[1] - (dataPtrcopy + nChan * (x - 0) + stepcopy * (y + 1))[1], 2) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[2] - (dataPtrcopy + nChan * (x - 0) + stepcopy * (y + 1))[2], 2));
                        distancias[6] += intermedio;
                        distancias[7] += intermedio;


                        intermedio = Math.Sqrt((Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[0] - (dataPtrcopy + nChan * (x + 1) + stepcopy * (y + 1))[0], 2)) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[1] - (dataPtrcopy + nChan * (x + 1) + stepcopy * (y + 1))[1], 2) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[2] - (dataPtrcopy + nChan * (x + 1) + stepcopy * (y + 1))[2], 2));
                        distancias[6] += intermedio;
                        distancias[8] += intermedio;
                        distancias[7] += intermedio;
                        distancias[8] += intermedio;



                        menorPessoacao = Array.IndexOf(distancias, distancias.Min());

                        switch (menorPessoacao)
                        {
                            case 0:
                                (dataPtr + nChan * x + step * y)[0] = (dataPtrcopy + nChan * x + stepcopy * (y - 1))[0];
                                (dataPtr + nChan * x + step * y)[1] = (dataPtrcopy + nChan * x + stepcopy * (y - 1))[1];
                                (dataPtr + nChan * x + step * y)[2] = (dataPtrcopy + nChan * x + stepcopy * (y - 1))[2];
                                break;
                            case 1:
                                (dataPtr + nChan * x + step * y)[0] = (dataPtrcopy + nChan * x + stepcopy * (y - 1))[0];
                                (dataPtr + nChan * x + step * y)[1] = (dataPtrcopy + nChan * x + stepcopy * (y - 1))[1];
                                (dataPtr + nChan * x + step * y)[2] = (dataPtrcopy + nChan * x + stepcopy * (y - 1))[2];
                                break;
                            case 2:
                                (dataPtr + nChan * x + step * y)[0] = (dataPtrcopy + nChan * (x + 1) + stepcopy * (y - 1))[0];
                                (dataPtr + nChan * x + step * y)[1] = (dataPtrcopy + nChan * (x + 1) + stepcopy * (y - 1))[1];
                                (dataPtr + nChan * x + step * y)[2] = (dataPtrcopy + nChan * (x + 1) + stepcopy * (y - 1))[2];
                                break;
                            case 3:
                                (dataPtr + nChan * x + step * y)[0] = (dataPtrcopy + nChan * x + stepcopy * y)[0];
                                (dataPtr + nChan * x + step * y)[1] = (dataPtrcopy + nChan * x + stepcopy * y)[1];
                                (dataPtr + nChan * x + step * y)[2] = (dataPtrcopy + nChan * x + stepcopy * y)[2];
                                break;
                            case 4:
                                (dataPtr + nChan * x + step * y)[0] = (dataPtrcopy + nChan * x + stepcopy * y)[0];
                                (dataPtr + nChan * x + step * y)[1] = (dataPtrcopy + nChan * x + stepcopy * y)[1];
                                (dataPtr + nChan * x + step * y)[2] = (dataPtrcopy + nChan * x + stepcopy * y)[2];
                                break;
                            case 5:
                                (dataPtr + nChan * x + step * y)[0] = (dataPtrcopy + nChan * (x + 1) + stepcopy * y)[0];
                                (dataPtr + nChan * x + step * y)[1] = (dataPtrcopy + nChan * (x + 1) + stepcopy * y)[1];
                                (dataPtr + nChan * x + step * y)[2] = (dataPtrcopy + nChan * (x + 1) + stepcopy * y)[2];
                                break;
                            case 6:
                                (dataPtr + nChan * x + step * y)[0] = (dataPtrcopy + nChan * x + stepcopy * (y + 1))[0];
                                (dataPtr + nChan * x + step * y)[1] = (dataPtrcopy + nChan * x + stepcopy * (y + 1))[1];
                                (dataPtr + nChan * x + step * y)[2] = (dataPtrcopy + nChan * x + stepcopy * (y + 1))[2];
                                break;
                            case 7:
                                (dataPtr + nChan * x + step * y)[0] = (dataPtrcopy + nChan * x + stepcopy * (y + 1))[0];
                                (dataPtr + nChan * x + step * y)[1] = (dataPtrcopy + nChan * x + stepcopy * (y + 1))[1];
                                (dataPtr + nChan * x + step * y)[2] = (dataPtrcopy + nChan * x + stepcopy * (y + 1))[2];
                                break;
                            case 8:
                                (dataPtr + nChan * x + step * y)[0] = (dataPtrcopy + nChan * (x + 1) + stepcopy * (y + 1))[0];
                                (dataPtr + nChan * x + step * y)[1] = (dataPtrcopy + nChan * (x + 1) + stepcopy * (y + 1))[1];
                                (dataPtr + nChan * x + step * y)[2] = (dataPtrcopy + nChan * (x + 1) + stepcopy * (y + 1))[2];
                                break;

                        }

                        x = width - 1;

                        distancias[0] = 0;
                        distancias[1] = 0;
                        distancias[2] = 0;
                        distancias[3] = 0;
                        distancias[4] = 0;
                        distancias[5] = 0;
                        distancias[6] = 0;
                        distancias[7] = 0;
                        distancias[8] = 0;

                        xPicel = x - 1;
                        yPixel = y - 1;


                        intermedio = Math.Sqrt((Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[0] - (dataPtrcopy + nChan * (x - 0) + stepcopy * (y - 1))[0], 2)) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[1] - (dataPtrcopy + nChan * (x - 0) + stepcopy * (y - 1))[1], 2) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[2] - (dataPtrcopy + nChan * (x - 0) + stepcopy * (y - 1))[2], 2));
                        distancias[0] += intermedio;
                        distancias[1] += intermedio;
                        distancias[0] += intermedio;
                        distancias[2] += intermedio;


                        intermedio = Math.Sqrt((Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[0] - (dataPtrcopy + nChan * (x - 1) + stepcopy * (y - 0))[0], 2)) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[1] - (dataPtrcopy + nChan * (x - 1) + stepcopy * (y - 0))[1], 2) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[2] - (dataPtrcopy + nChan * (x - 1) + stepcopy * (y - 0))[2], 2));
                        distancias[0] += intermedio;
                        distancias[3] += intermedio;


                        intermedio = Math.Sqrt((Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[0] - (dataPtrcopy + nChan * (x - 0) + stepcopy * (y - 0))[0], 2)) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[1] - (dataPtrcopy + nChan * (x - 0) + stepcopy * (y - 0))[1], 2) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[2] - (dataPtrcopy + nChan * (x - 0) + stepcopy * (y - 0))[2], 2));
                        distancias[0] += intermedio;
                        distancias[4] += intermedio;
                        distancias[0] += intermedio;
                        distancias[5] += intermedio;


                        intermedio = Math.Sqrt((Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[0] - (dataPtrcopy + nChan * (x - 1) + stepcopy * (y + 1))[0], 2)) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[1] - (dataPtrcopy + nChan * (x - 1) + stepcopy * (y + 1))[1], 2) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[2] - (dataPtrcopy + nChan * (x - 1) + stepcopy * (y + 1))[2], 2));
                        distancias[0] += intermedio;
                        distancias[6] += intermedio;


                        intermedio = Math.Sqrt((Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[0] - (dataPtrcopy + nChan * (x - 0) + stepcopy * (y + 1))[0], 2)) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[1] - (dataPtrcopy + nChan * (x - 0) + stepcopy * (y + 1))[1], 2) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[2] - (dataPtrcopy + nChan * (x - 0) + stepcopy * (y + 1))[2], 2));
                        distancias[0] += intermedio;
                        distancias[7] += intermedio;
                        distancias[0] += intermedio;
                        distancias[8] += intermedio;



                        // distancia pixei superior central

                        xPicel = x;
                        yPixel = y - 1;

                        intermedio = Math.Sqrt((Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[0] - (dataPtrcopy + nChan * (x + 0) + stepcopy * (y - 1))[0], 2)) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[1] - (dataPtrcopy + nChan * (x + 0) + stepcopy * (y - 1))[1], 2) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[2] - (dataPtrcopy + nChan * (x + 0) + stepcopy * (y - 1))[2], 2));
                        distancias[1] += intermedio;
                        distancias[2] += intermedio;


                        intermedio = Math.Sqrt((Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[0] - (dataPtrcopy + nChan * (x - 1) + stepcopy * (y - 0))[0], 2)) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[1] - (dataPtrcopy + nChan * (x - 1) + stepcopy * (y - 0))[1], 2) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[2] - (dataPtrcopy + nChan * (x - 1) + stepcopy * (y - 0))[2], 2));
                        distancias[1] += intermedio;
                        distancias[3] += intermedio;
                        distancias[2] += intermedio;
                        distancias[3] += intermedio;


                        intermedio = Math.Sqrt((Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[0] - (dataPtrcopy + nChan * (x - 0) + stepcopy * (y - 0))[0], 2)) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[1] - (dataPtrcopy + nChan * (x - 0) + stepcopy * (y - 0))[1], 2) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[2] - (dataPtrcopy + nChan * (x - 0) + stepcopy * (y - 0))[2], 2));
                        distancias[1] += intermedio;
                        distancias[4] += intermedio;
                        distancias[1] += intermedio;
                        distancias[5] += intermedio;
                        distancias[2] += intermedio;
                        distancias[4] += intermedio;
                        distancias[2] += intermedio;
                        distancias[5] += intermedio;


                        intermedio = Math.Sqrt((Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[0] - (dataPtrcopy + nChan * (x - 1) + stepcopy * (y + 1))[0], 2)) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[1] - (dataPtrcopy + nChan * (x - 1) + stepcopy * (y + 1))[1], 2) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[2] - (dataPtrcopy + nChan * (x - 1) + stepcopy * (y + 1))[2], 2));
                        distancias[1] += intermedio;
                        distancias[6] += intermedio;
                        distancias[2] += intermedio;
                        distancias[6] += intermedio;


                        intermedio = Math.Sqrt((Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[0] - (dataPtrcopy + nChan * (x - 0) + stepcopy * (y + 1))[0], 2)) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[1] - (dataPtrcopy + nChan * (x - 0) + stepcopy * (y + 1))[1], 2) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[2] - (dataPtrcopy + nChan * (x - 0) + stepcopy * (y + 1))[2], 2));
                        distancias[1] += intermedio;
                        distancias[7] += intermedio;
                        distancias[1] += intermedio;
                        distancias[8] += intermedio;
                        distancias[2] += intermedio;
                        distancias[7] += intermedio;
                        distancias[2] += intermedio;
                        distancias[8] += intermedio;



                        // distancia pixei centro esquerdo

                        xPicel = x - 1;
                        yPixel = y;

                        intermedio = Math.Sqrt((Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[0] - (dataPtrcopy + nChan * (x - 0) + stepcopy * (y - 0))[0], 2)) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[1] - (dataPtrcopy + nChan * (x - 0) + stepcopy * (y - 0))[1], 2) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[2] - (dataPtrcopy + nChan * (x - 0) + stepcopy * (y - 0))[2], 2));
                        distancias[3] += intermedio;
                        distancias[4] += intermedio;
                        distancias[3] += intermedio;
                        distancias[5] += intermedio;


                        intermedio = Math.Sqrt((Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[0] - (dataPtrcopy + nChan * (x - 1) + stepcopy * (y + 1))[0], 2)) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[1] - (dataPtrcopy + nChan * (x - 1) + stepcopy * (y + 1))[1], 2) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[2] - (dataPtrcopy + nChan * (x - 1) + stepcopy * (y + 1))[2], 2));
                        distancias[3] += intermedio;
                        distancias[6] += intermedio;


                        intermedio = Math.Sqrt((Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[0] - (dataPtrcopy + nChan * (x - 0) + stepcopy * (y + 1))[0], 2)) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[1] - (dataPtrcopy + nChan * (x - 0) + stepcopy * (y + 1))[1], 2) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[2] - (dataPtrcopy + nChan * (x - 0) + stepcopy * (y + 1))[2], 2));
                        distancias[3] += intermedio;
                        distancias[7] += intermedio;
                        distancias[3] += intermedio;
                        distancias[8] += intermedio;





                        // distancia pixei centro centro
                        // 
                        xPicel = x;
                        yPixel = y;


                        intermedio = Math.Sqrt((Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[0] - (dataPtrcopy + nChan * (x + 0) + stepcopy * (y - 0))[0], 2)) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[1] - (dataPtrcopy + nChan * (x + 0) + stepcopy * (y - 0))[1], 2) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[2] - (dataPtrcopy + nChan * (x + 0) + stepcopy * (y - 0))[2], 2));
                        distancias[4] += intermedio;
                        distancias[5] += intermedio;


                        intermedio = Math.Sqrt((Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[0] - (dataPtrcopy + nChan * (x - 1) + stepcopy * (y + 1))[0], 2)) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[1] - (dataPtrcopy + nChan * (x - 1) + stepcopy * (y + 1))[1], 2) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[2] - (dataPtrcopy + nChan * (x - 1) + stepcopy * (y + 1))[2], 2));
                        distancias[4] += intermedio;
                        distancias[6] += intermedio;
                        distancias[5] += intermedio;
                        distancias[6] += intermedio;


                        intermedio = Math.Sqrt((Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[0] - (dataPtrcopy + nChan * (x - 0) + stepcopy * (y + 1))[0], 2)) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[1] - (dataPtrcopy + nChan * (x - 0) + stepcopy * (y + 1))[1], 2) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[2] - (dataPtrcopy + nChan * (x - 0) + stepcopy * (y + 1))[2], 2));
                        distancias[4] += intermedio;
                        distancias[7] += intermedio;
                        distancias[4] += intermedio;
                        distancias[8] += intermedio;
                        distancias[5] += intermedio;
                        distancias[7] += intermedio;
                        distancias[5] += intermedio;
                        distancias[8] += intermedio;


                        // distancia pixei baixo esquerda

                        xPicel = x - 1;
                        yPixel = y + 1;

                        intermedio = Math.Sqrt((Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[0] - (dataPtrcopy + nChan * (x - 0) + stepcopy * (y + 1))[0], 2)) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[1] - (dataPtrcopy + nChan * (x - 0) + stepcopy * (y + 1))[1], 2) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[2] - (dataPtrcopy + nChan * (x - 0) + stepcopy * (y + 1))[2], 2));
                        distancias[6] += intermedio;
                        distancias[7] += intermedio;
                        distancias[6] += intermedio;
                        distancias[8] += intermedio;


                        // distancia pixei baixo centro

                        xPicel = x;
                        yPixel = y + 1;

                        intermedio = Math.Sqrt((Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[0] - (dataPtrcopy + nChan * (x + 0) + stepcopy * (y + 1))[0], 2)) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[1] - (dataPtrcopy + nChan * (x + 0) + stepcopy * (y + 1))[1], 2) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[2] - (dataPtrcopy + nChan * (x + 0) + stepcopy * (y + 1))[2], 2));
                        distancias[7] += intermedio;
                        distancias[8] += intermedio;





                        menorPessoacao = Array.IndexOf(distancias, distancias.Min());

                        switch (menorPessoacao)
                        {
                            case 0:
                                (dataPtr + nChan * x + step * y)[0] = (dataPtrcopy + nChan * (x - 1) + stepcopy * (y - 1))[0];
                                (dataPtr + nChan * x + step * y)[1] = (dataPtrcopy + nChan * (x - 1) + stepcopy * (y - 1))[1];
                                (dataPtr + nChan * x + step * y)[2] = (dataPtrcopy + nChan * (x - 1) + stepcopy * (y - 1))[2];
                                break;
                            case 1:
                                (dataPtr + nChan * x + step * y)[0] = (dataPtrcopy + nChan * x + stepcopy * (y - 1))[0];
                                (dataPtr + nChan * x + step * y)[1] = (dataPtrcopy + nChan * x + stepcopy * (y - 1))[1];
                                (dataPtr + nChan * x + step * y)[2] = (dataPtrcopy + nChan * x + stepcopy * (y - 1))[2];
                                break;
                            case 2:
                                (dataPtr + nChan * x + step * y)[0] = (dataPtrcopy + nChan * x + stepcopy * (y - 1))[0];
                                (dataPtr + nChan * x + step * y)[1] = (dataPtrcopy + nChan * x + stepcopy * (y - 1))[1];
                                (dataPtr + nChan * x + step * y)[2] = (dataPtrcopy + nChan * x + stepcopy * (y - 1))[2];
                                break;
                            case 3:
                                (dataPtr + nChan * x + step * y)[0] = (dataPtrcopy + nChan * (x - 1) + stepcopy * y)[0];
                                (dataPtr + nChan * x + step * y)[1] = (dataPtrcopy + nChan * (x - 1) + stepcopy * y)[1];
                                (dataPtr + nChan * x + step * y)[2] = (dataPtrcopy + nChan * (x - 1) + stepcopy * y)[2];
                                break;
                            case 4:
                                (dataPtr + nChan * x + step * y)[0] = (dataPtrcopy + nChan * x + stepcopy * y)[0];
                                (dataPtr + nChan * x + step * y)[1] = (dataPtrcopy + nChan * x + stepcopy * y)[1];
                                (dataPtr + nChan * x + step * y)[2] = (dataPtrcopy + nChan * x + stepcopy * y)[2];
                                break;
                            case 5:
                                (dataPtr + nChan * x + step * y)[0] = (dataPtrcopy + nChan * x + stepcopy * y)[0];
                                (dataPtr + nChan * x + step * y)[1] = (dataPtrcopy + nChan * x + stepcopy * y)[1];
                                (dataPtr + nChan * x + step * y)[2] = (dataPtrcopy + nChan * x + stepcopy * y)[2];
                                break;
                            case 6:
                                (dataPtr + nChan * x + step * y)[0] = (dataPtrcopy + nChan * (x - 1) + stepcopy * (y + 1))[0];
                                (dataPtr + nChan * x + step * y)[1] = (dataPtrcopy + nChan * (x - 1) + stepcopy * (y + 1))[1];
                                (dataPtr + nChan * x + step * y)[2] = (dataPtrcopy + nChan * (x - 1) + stepcopy * (y + 1))[2];
                                break;
                            case 7:
                                (dataPtr + nChan * x + step * y)[0] = (dataPtrcopy + nChan * x + stepcopy * (y + 1))[0];
                                (dataPtr + nChan * x + step * y)[1] = (dataPtrcopy + nChan * x + stepcopy * (y + 1))[1];
                                (dataPtr + nChan * x + step * y)[2] = (dataPtrcopy + nChan * x + stepcopy * (y + 1))[2];
                                break;
                            case 8:
                                (dataPtr + nChan * x + step * y)[0] = (dataPtrcopy + nChan * x + stepcopy * (y + 1))[0];
                                (dataPtr + nChan * x + step * y)[1] = (dataPtrcopy + nChan * x + stepcopy * (y + 1))[1];
                                (dataPtr + nChan * x + step * y)[2] = (dataPtrcopy + nChan * x + stepcopy * (y + 1))[2];
                                break;
                        }

                    }

                    // canto superior esquerdo

                    distancias[0] = 0;
                    distancias[1] = 0;
                    distancias[2] = 0;
                    distancias[3] = 0;
                    distancias[4] = 0;
                    distancias[5] = 0;
                    distancias[6] = 0;
                    distancias[7] = 0;
                    distancias[8] = 0;

                    x = 0;
                    y = 0;

                    // xPicel = x - 1;
                    // yPixel = y - 1;

                    xPicel = x;
                    yPixel = y;


                    intermedio = Math.Sqrt((Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[0] - (dataPtrcopy + nChan * (x - 0) + stepcopy * (y - 0))[0], 2)) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[1] - (dataPtrcopy + nChan * (x - 0) + stepcopy * (y - 0))[1], 2) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[2] - (dataPtrcopy + nChan * (x - 0) + stepcopy * (y - 0))[2], 2));
                    distancias[0] += intermedio;
                    distancias[1] += intermedio;
                    distancias[0] += intermedio;
                    distancias[3] += intermedio;
                    distancias[0] += intermedio;
                    distancias[4] += intermedio;
                    distancias[1] += intermedio;
                    distancias[3] += intermedio;
                    distancias[1] += intermedio;
                    distancias[4] += intermedio;
                    distancias[3] += intermedio;
                    distancias[4] += intermedio;


                    intermedio = Math.Sqrt((Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[0] - (dataPtrcopy + nChan * (x + 1) + stepcopy * (y - 0))[0], 2)) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[1] - (dataPtrcopy + nChan * (x + 1) + stepcopy * (y - 0))[1], 2) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[2] - (dataPtrcopy + nChan * (x + 1) + stepcopy * (y - 0))[2], 2));
                    distancias[0] += intermedio;
                    distancias[2] += intermedio;
                    distancias[0] += intermedio;
                    distancias[5] += intermedio;
                    distancias[1] += intermedio;
                    distancias[2] += intermedio;
                    distancias[1] += intermedio;
                    distancias[5] += intermedio;
                    distancias[3] += intermedio;
                    distancias[5] += intermedio;
                    distancias[4] += intermedio;
                    distancias[5] += intermedio;


                    intermedio = Math.Sqrt((Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[0] - (dataPtrcopy + nChan * (x - 0) + stepcopy * (y + 1))[0], 2)) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[1] - (dataPtrcopy + nChan * (x - 0) + stepcopy * (y + 1))[1], 2) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[2] - (dataPtrcopy + nChan * (x - 0) + stepcopy * (y + 1))[2], 2));
                    distancias[0] += intermedio;
                    distancias[6] += intermedio;
                    distancias[0] += intermedio;
                    distancias[7] += intermedio;
                    distancias[1] += intermedio;
                    distancias[6] += intermedio;
                    distancias[1] += intermedio;
                    distancias[7] += intermedio;
                    distancias[3] += intermedio;
                    distancias[6] += intermedio;
                    distancias[3] += intermedio;
                    distancias[7] += intermedio;
                    distancias[4] += intermedio;
                    distancias[6] += intermedio;
                    distancias[4] += intermedio;
                    distancias[7] += intermedio;



                    intermedio = Math.Sqrt((Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[0] - (dataPtrcopy + nChan * (x + 1) + stepcopy * (y + 1))[0], 2)) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[1] - (dataPtrcopy + nChan * (x + 1) + stepcopy * (y + 1))[1], 2) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[2] - (dataPtrcopy + nChan * (x + 1) + stepcopy * (y + 1))[2], 2));
                    distancias[0] += intermedio;
                    distancias[8] += intermedio;
                    distancias[1] += intermedio;
                    distancias[8] += intermedio;
                    distancias[3] += intermedio;
                    distancias[8] += intermedio;
                    distancias[4] += intermedio;
                    distancias[8] += intermedio;


                    // distancia pixei superior central
                    // feito no primeiro
                    // xPicel = x;  
                    // yPixel = y - 1; 





                    // distancia pixei superior direiro
                    // xPicel = x + 1;  
                    // yPixel = y - 1;

                    xPicel = x + 1;
                    yPixel = y;

                    intermedio = Math.Sqrt((Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[0] - (dataPtrcopy + nChan * (x - 0) + stepcopy * (y - 0))[0], 2)) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[1] - (dataPtrcopy + nChan * (x - 0) + stepcopy * (y - 0))[1], 2) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[2] - (dataPtrcopy + nChan * (x - 0) + stepcopy * (y - 0))[2], 2));
                    distancias[2] += intermedio;
                    distancias[3] += intermedio;
                    distancias[2] += intermedio;
                    distancias[4] += intermedio;


                    intermedio = Math.Sqrt((Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[0] - (dataPtrcopy + nChan * (x + 1) + stepcopy * (y - 0))[0], 2)) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[1] - (dataPtrcopy + nChan * (x + 1) + stepcopy * (y - 0))[1], 2) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[2] - (dataPtrcopy + nChan * (x + 1) + stepcopy * (y - 0))[2], 2));
                    distancias[2] += intermedio;
                    distancias[5] += intermedio;


                    intermedio = Math.Sqrt((Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[0] - (dataPtrcopy + nChan * (x - 0) + stepcopy * (y + 1))[0], 2)) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[1] - (dataPtrcopy + nChan * (x - 0) + stepcopy * (y + 1))[1], 2) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[2] - (dataPtrcopy + nChan * (x - 0) + stepcopy * (y + 1))[2], 2));
                    distancias[2] += intermedio;
                    distancias[6] += intermedio;
                    distancias[2] += intermedio;
                    distancias[7] += intermedio;
                    distancias[5] += intermedio;
                    distancias[6] += intermedio;
                    distancias[5] += intermedio;
                    distancias[7] += intermedio;


                    intermedio = Math.Sqrt((Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[0] - (dataPtrcopy + nChan * (x + 1) + stepcopy * (y + 1))[0], 2)) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[1] - (dataPtrcopy + nChan * (x + 1) + stepcopy * (y + 1))[1], 2) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[2] - (dataPtrcopy + nChan * (x + 1) + stepcopy * (y + 1))[2], 2));
                    distancias[2] += intermedio;
                    distancias[8] += intermedio;
                    distancias[5] += intermedio;
                    distancias[8] += intermedio;

                    // distancia pixei centro esquerdo
                    // feito no primeiro
                    // xPicel = x - 1;  
                    // yPixel = y;


                    // distancia pixei centro centro
                    // feito no primeiro
                    // xPicel = x;  
                    // yPixel = y;



                    // distancia pixei centro direita
                    // feito no segundo
                    // xPicel = x + 1;  
                    // yPixel = y;


                    // distancia pixei baixo esquerda
                    // xPicel = x - 1;  
                    // yPixel = y - 1;

                    xPicel = x;
                    yPixel = y + 1;

                    intermedio = Math.Sqrt((Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[0] - (dataPtrcopy + nChan * (x - 0) + stepcopy * (y + 1))[0], 2)) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[1] - (dataPtrcopy + nChan * (x - 0) + stepcopy * (y + 1))[1], 2) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[2] - (dataPtrcopy + nChan * (x - 0) + stepcopy * (y + 1))[2], 2));
                    distancias[6] += intermedio;
                    distancias[7] += intermedio;


                    intermedio = Math.Sqrt((Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[0] - (dataPtrcopy + nChan * (x + 1) + stepcopy * (y + 1))[0], 2)) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[1] - (dataPtrcopy + nChan * (x + 1) + stepcopy * (y + 1))[1], 2) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[2] - (dataPtrcopy + nChan * (x + 1) + stepcopy * (y + 1))[2], 2));
                    distancias[6] += intermedio;
                    distancias[8] += intermedio;
                    distancias[7] += intermedio;
                    distancias[8] += intermedio;


                    // distancia pixei baixo centro
                    // feito no terceiro
                    // xPicel = x;  
                    // yPixel = y + 1;


                    menorPessoacao = Array.IndexOf(distancias, distancias.Min());

                    switch (menorPessoacao)
                    {
                        case 0:
                        case 1:
                        case 3:
                        case 4:
                            (dataPtr + nChan * x + step * y)[0] = (dataPtrcopy + nChan * x + stepcopy * y)[0];
                            (dataPtr + nChan * x + step * y)[1] = (dataPtrcopy + nChan * x + stepcopy * y)[1];
                            (dataPtr + nChan * x + step * y)[2] = (dataPtrcopy + nChan * x + stepcopy * y)[2];
                            break;

                        case 2:
                        case 5:
                            (dataPtr + nChan * x + step * y)[0] = (dataPtrcopy + nChan * (x + 1) + stepcopy * y)[0];
                            (dataPtr + nChan * x + step * y)[1] = (dataPtrcopy + nChan * (x + 1) + stepcopy * y)[1];
                            (dataPtr + nChan * x + step * y)[2] = (dataPtrcopy + nChan * (x + 1) + stepcopy * y)[2];
                            break;

                        case 6:
                        case 7:
                            (dataPtr + nChan * x + step * y)[0] = (dataPtrcopy + nChan * x + stepcopy * (y + 1))[0];
                            (dataPtr + nChan * x + step * y)[1] = (dataPtrcopy + nChan * x + stepcopy * (y + 1))[1];
                            (dataPtr + nChan * x + step * y)[2] = (dataPtrcopy + nChan * x + stepcopy * (y + 1))[2];
                            break;

                        case 8:
                            (dataPtr + nChan * x + step * y)[0] = (dataPtrcopy + nChan * (x + 1) + stepcopy * (y + 1))[0];
                            (dataPtr + nChan * x + step * y)[1] = (dataPtrcopy + nChan * (x + 1) + stepcopy * (y + 1))[1];
                            (dataPtr + nChan * x + step * y)[2] = (dataPtrcopy + nChan * (x + 1) + stepcopy * (y + 1))[2];
                            break;



                    }

                    // canto superior direiro 

                    x = width - 1; y = 0;

                    distancias[0] = 0;
                    distancias[1] = 0;
                    distancias[2] = 0;
                    distancias[3] = 0;
                    distancias[4] = 0;
                    distancias[5] = 0;
                    distancias[6] = 0;
                    distancias[7] = 0;
                    distancias[8] = 0;

                    // xPicel = x - 1;
                    // yPixel = y - 1;

                    xPicel = x - 1;
                    yPixel = y;


                    intermedio = Math.Sqrt((Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[0] - (dataPtrcopy + nChan * (x - 0) + stepcopy * (y - 0))[0], 2)) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[1] - (dataPtrcopy + nChan * (x - 0) + stepcopy * (y - 0))[1], 2) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[2] - (dataPtrcopy + nChan * (x - 0) + stepcopy * (y - 0))[2], 2));
                    distancias[0] += intermedio;
                    distancias[1] += intermedio;
                    distancias[0] += intermedio;
                    distancias[2] += intermedio;
                    distancias[0] += intermedio;
                    distancias[4] += intermedio;
                    distancias[0] += intermedio;
                    distancias[5] += intermedio;
                    distancias[3] += intermedio;
                    distancias[4] += intermedio;
                    distancias[3] += intermedio;
                    distancias[5] += intermedio;


                    intermedio = Math.Sqrt((Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[0] - (dataPtrcopy + nChan * (x - 1) + stepcopy * (y - 0))[0], 2)) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[1] - (dataPtrcopy + nChan * (x - 1) + stepcopy * (y - 0))[1], 2) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[2] - (dataPtrcopy + nChan * (x - 1) + stepcopy * (y - 0))[2], 2));
                    distancias[0] += intermedio;
                    distancias[3] += intermedio;


                    intermedio = Math.Sqrt((Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[0] - (dataPtrcopy + nChan * (x - 1) + stepcopy * (y + 1))[0], 2)) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[1] - (dataPtrcopy + nChan * (x - 1) + stepcopy * (y + 1))[1], 2) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[2] - (dataPtrcopy + nChan * (x - 1) + stepcopy * (y + 1))[2], 2));
                    distancias[0] += intermedio;
                    distancias[6] += intermedio;
                    distancias[3] += intermedio;
                    distancias[6] += intermedio;

                    intermedio = Math.Sqrt((Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[0] - (dataPtrcopy + nChan * (x - 0) + stepcopy * (y + 1))[0], 2)) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[1] - (dataPtrcopy + nChan * (x - 0) + stepcopy * (y + 1))[1], 2) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[2] - (dataPtrcopy + nChan * (x - 0) + stepcopy * (y + 1))[2], 2));
                    distancias[0] += intermedio;
                    distancias[7] += intermedio;
                    distancias[0] += intermedio;
                    distancias[8] += intermedio;
                    distancias[3] += intermedio;
                    distancias[7] += intermedio;
                    distancias[3] += intermedio;
                    distancias[8] += intermedio;



                    // distancia pixei superior central

                    // xPicel = x;
                    // yPixel = y - 1;

                    xPicel = x;
                    yPixel = y;

                    intermedio = Math.Sqrt((Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[0] - (dataPtrcopy + nChan * (x + 0) + stepcopy * (y - 0))[0], 2)) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[1] - (dataPtrcopy + nChan * (x + 0) + stepcopy * (y - 0))[1], 2) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[2] - (dataPtrcopy + nChan * (x + 0) + stepcopy * (y - 0))[2], 2));
                    distancias[1] += intermedio;
                    distancias[2] += intermedio;
                    distancias[1] += intermedio;
                    distancias[4] += intermedio;
                    distancias[1] += intermedio;
                    distancias[5] += intermedio;
                    distancias[2] += intermedio;
                    distancias[4] += intermedio;
                    distancias[2] += intermedio;
                    distancias[5] += intermedio;
                    distancias[4] += intermedio;
                    distancias[5] += intermedio;


                    intermedio = Math.Sqrt((Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[0] - (dataPtrcopy + nChan * (x - 1) + stepcopy * (y - 0))[0], 2)) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[1] - (dataPtrcopy + nChan * (x - 1) + stepcopy * (y - 0))[1], 2) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[2] - (dataPtrcopy + nChan * (x - 1) + stepcopy * (y - 0))[2], 2));
                    distancias[1] += intermedio;
                    distancias[3] += intermedio;
                    distancias[2] += intermedio;
                    distancias[3] += intermedio;


                    intermedio = Math.Sqrt((Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[0] - (dataPtrcopy + nChan * (x - 1) + stepcopy * (y + 1))[0], 2)) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[1] - (dataPtrcopy + nChan * (x - 1) + stepcopy * (y + 1))[1], 2) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[2] - (dataPtrcopy + nChan * (x - 1) + stepcopy * (y + 1))[2], 2));
                    distancias[1] += intermedio;
                    distancias[6] += intermedio;
                    distancias[2] += intermedio;
                    distancias[6] += intermedio;
                    distancias[4] += intermedio;
                    distancias[6] += intermedio;
                    distancias[5] += intermedio;
                    distancias[6] += intermedio;

                    intermedio = Math.Sqrt((Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[0] - (dataPtrcopy + nChan * (x - 0) + stepcopy * (y + 1))[0], 2)) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[1] - (dataPtrcopy + nChan * (x - 0) + stepcopy * (y + 1))[1], 2) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[2] - (dataPtrcopy + nChan * (x - 0) + stepcopy * (y + 1))[2], 2));
                    distancias[1] += intermedio;
                    distancias[7] += intermedio;
                    distancias[1] += intermedio;
                    distancias[8] += intermedio;
                    distancias[2] += intermedio;
                    distancias[7] += intermedio;
                    distancias[2] += intermedio;
                    distancias[8] += intermedio;
                    distancias[4] += intermedio;
                    distancias[7] += intermedio;
                    distancias[4] += intermedio;
                    distancias[8] += intermedio;
                    distancias[5] += intermedio;
                    distancias[7] += intermedio;
                    distancias[5] += intermedio;
                    distancias[8] += intermedio;





                    // distancia pixei direiro central
                    // feito no segundo
                    // xPicel = x + 1;
                    // yPixel = y - 1;


                    xPicel = x;
                    yPixel = y;




                    // distancia pixei centro esquerdo
                    // feito no primeiro

                    xPicel = x - 1;
                    yPixel = y;



                    // distancia pixei centro centro
                    // feito no segundo
                    xPicel = x;
                    yPixel = y;






                    // distancia pixei centro direita
                    // feito no segundo
                    xPicel = x;
                    yPixel = y;




                    // distancia pixei baixo esquerda

                    xPicel = x - 1;
                    yPixel = y + 1;

                    intermedio = Math.Sqrt((Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[0] - (dataPtrcopy + nChan * (x - 0) + stepcopy * (y + 1))[0], 2)) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[1] - (dataPtrcopy + nChan * (x - 0) + stepcopy * (y + 1))[1], 2) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[2] - (dataPtrcopy + nChan * (x - 0) + stepcopy * (y + 1))[2], 2));
                    distancias[6] += intermedio;
                    distancias[7] += intermedio;
                    distancias[6] += intermedio;
                    distancias[8] += intermedio;


                    // distancia pixei baixo centro
                    // neste é zero o porque vai fazer a distancia com ele proprio (o da direita) e os outros já foram calculado

                    xPicel = x;
                    yPixel = y + 1;


                    menorPessoacao = Array.IndexOf(distancias, distancias.Min());

                    switch (menorPessoacao)
                    {
                        case 0:
                        case 3:
                            (dataPtr + nChan * x + step * y)[0] = (dataPtrcopy + nChan * (x - 1) + stepcopy * y)[0];
                            (dataPtr + nChan * x + step * y)[1] = (dataPtrcopy + nChan * (x - 1) + stepcopy * y)[1];
                            (dataPtr + nChan * x + step * y)[2] = (dataPtrcopy + nChan * (x - 1) + stepcopy * y)[2];
                            break;
                        case 1:
                        case 2:
                        case 4:
                        case 5:
                            (dataPtr + nChan * x + step * y)[0] = (dataPtrcopy + nChan * x + stepcopy * y)[0];
                            (dataPtr + nChan * x + step * y)[1] = (dataPtrcopy + nChan * x + stepcopy * y)[1];
                            (dataPtr + nChan * x + step * y)[2] = (dataPtrcopy + nChan * x + stepcopy * y)[2];
                            break;
                        case 6:
                            (dataPtr + nChan * x + step * y)[0] = (dataPtrcopy + nChan * (x - 1) + stepcopy * (y + 1))[0];
                            (dataPtr + nChan * x + step * y)[1] = (dataPtrcopy + nChan * (x - 1) + stepcopy * (y + 1))[1];
                            (dataPtr + nChan * x + step * y)[2] = (dataPtrcopy + nChan * (x - 1) + stepcopy * (y + 1))[2];
                            break;
                        case 7:
                        case 8:
                            (dataPtr + nChan * x + step * y)[0] = (dataPtrcopy + nChan * x + stepcopy * (y + 1))[0];
                            (dataPtr + nChan * x + step * y)[1] = (dataPtrcopy + nChan * x + stepcopy * (y + 1))[1];
                            (dataPtr + nChan * x + step * y)[2] = (dataPtrcopy + nChan * x + stepcopy * (y + 1))[2];
                            break;




                    }



                    // canto inferior esquerdo

                    x = 0; y = height - 1;


                    distancias[0] = 0;
                    distancias[1] = 0;
                    distancias[2] = 0;
                    distancias[3] = 0;
                    distancias[4] = 0;
                    distancias[5] = 0;
                    distancias[6] = 0;
                    distancias[7] = 0;
                    distancias[8] = 0;


                    // xPicel = x - 1;
                    // yPixel = y - 1;

                    xPicel = x;
                    yPixel = y - 1;


                    intermedio = Math.Sqrt((Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[0] - (dataPtrcopy + nChan * (x - 0) + stepcopy * (y - 1))[0], 2)) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[1] - (dataPtrcopy + nChan * (x - 0) + stepcopy * (y - 1))[1], 2) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[2] - (dataPtrcopy + nChan * (x - 0) + stepcopy * (y - 1))[2], 2));
                    distancias[0] += intermedio;
                    distancias[1] += intermedio;


                    intermedio = Math.Sqrt((Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[0] - (dataPtrcopy + nChan * (x + 1) + stepcopy * (y - 1))[0], 2)) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[1] - (dataPtrcopy + nChan * (x + 1) + stepcopy * (y - 1))[1], 2) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[2] - (dataPtrcopy + nChan * (x + 1) + stepcopy * (y - 1))[2], 2));
                    distancias[0] += intermedio;
                    distancias[2] += intermedio;
                    distancias[1] += intermedio;
                    distancias[2] += intermedio;


                    intermedio = Math.Sqrt((Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[0] - (dataPtrcopy + nChan * (x - 0) + stepcopy * (y - 0))[0], 2)) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[1] - (dataPtrcopy + nChan * (x - 0) + stepcopy * (y - 0))[1], 2) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[2] - (dataPtrcopy + nChan * (x - 0) + stepcopy * (y - 0))[2], 2));
                    distancias[0] += intermedio;
                    distancias[3] += intermedio;
                    distancias[0] += intermedio;
                    distancias[4] += intermedio;
                    distancias[0] += intermedio;
                    distancias[6] += intermedio;
                    distancias[0] += intermedio;
                    distancias[7] += intermedio;
                    distancias[1] += intermedio;
                    distancias[3] += intermedio;
                    distancias[1] += intermedio;
                    distancias[4] += intermedio;
                    distancias[1] += intermedio;
                    distancias[6] += intermedio;
                    distancias[1] += intermedio;
                    distancias[7] += intermedio;


                    intermedio = Math.Sqrt((Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[0] - (dataPtrcopy + nChan * (x + 1) + stepcopy * (y - 0))[0], 2)) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[1] - (dataPtrcopy + nChan * (x + 1) + stepcopy * (y - 0))[1], 2) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[2] - (dataPtrcopy + nChan * (x + 1) + stepcopy * (y - 0))[2], 2));
                    distancias[0] += intermedio;
                    distancias[5] += intermedio;
                    distancias[0] += intermedio;
                    distancias[8] += intermedio;
                    distancias[1] += intermedio;
                    distancias[5] += intermedio;
                    distancias[1] += intermedio;
                    distancias[8] += intermedio;


                    // distancia pixei superior central
                    // feito no primeiro

                    xPicel = x;
                    yPixel = y - 1;





                    // distancia pixei direiro central

                    xPicel = x + 1;
                    yPixel = y - 1;


                    intermedio = Math.Sqrt((Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[0] - (dataPtrcopy + nChan * (x - 0) + stepcopy * (y - 0))[0], 2)) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[1] - (dataPtrcopy + nChan * (x - 0) + stepcopy * (y - 0))[1], 2) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[2] - (dataPtrcopy + nChan * (x - 0) + stepcopy * (y - 0))[2], 2));
                    distancias[2] += intermedio;
                    distancias[4] += intermedio;
                    distancias[2] += intermedio;
                    distancias[3] += intermedio;
                    distancias[2] += intermedio;
                    distancias[6] += intermedio;
                    distancias[2] += intermedio;
                    distancias[7] += intermedio;


                    intermedio = Math.Sqrt((Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[0] - (dataPtrcopy + nChan * (x + 1) + stepcopy * (y - 0))[0], 2)) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[1] - (dataPtrcopy + nChan * (x + 1) + stepcopy * (y - 0))[1], 2) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[2] - (dataPtrcopy + nChan * (x + 1) + stepcopy * (y - 0))[2], 2));
                    distancias[2] += intermedio;
                    distancias[5] += intermedio;
                    distancias[2] += intermedio;
                    distancias[8] += intermedio;


                    // distancia pixei centro esquerdo

                    // xPicel = x - 1;
                    // yPixel = y;

                    xPicel = x;
                    yPixel = y;


                    intermedio = Math.Sqrt((Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[0] - (dataPtrcopy + nChan * (x + 1) + stepcopy * (y - 0))[0], 2)) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[1] - (dataPtrcopy + nChan * (x + 1) + stepcopy * (y - 0))[1], 2) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[2] - (dataPtrcopy + nChan * (x + 1) + stepcopy * (y - 0))[2], 2));
                    distancias[3] += intermedio;
                    distancias[5] += intermedio;
                    distancias[3] += intermedio;
                    distancias[8] += intermedio;
                    distancias[4] += intermedio;
                    distancias[5] += intermedio;
                    distancias[4] += intermedio;
                    distancias[8] += intermedio;
                    distancias[6] += intermedio;
                    distancias[8] += intermedio;
                    distancias[7] += intermedio;
                    distancias[8] += intermedio;



                    // distancia pixei centro centro
                    // feito no terceiro


                    // distancia pixei centro direita
                    // 
                    xPicel = x + 1;
                    yPixel = y;

                    intermedio = Math.Sqrt((Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[0] - (dataPtrcopy + nChan * (x - 0) + stepcopy * (y + 0))[0], 2)) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[1] - (dataPtrcopy + nChan * (x - 0) + stepcopy * (y + 0))[1], 2) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[2] - (dataPtrcopy + nChan * (x - 0) + stepcopy * (y + 0))[2], 2));
                    distancias[5] += intermedio;
                    distancias[6] += intermedio;
                    distancias[5] += intermedio;
                    distancias[7] += intermedio;



                    // distancia pixei baixo esquerda
                    // feito no terceiro
                    // xPicel = x - 1;
                    // yPixel = y + 1;


                    xPicel = x;
                    yPixel = y;


                    intermedio = Math.Sqrt((Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[0] - (dataPtrcopy + nChan * (x + 1) + stepcopy * (y + 0))[0], 2)) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[1] - (dataPtrcopy + nChan * (x + 1) + stepcopy * (y + 1))[1], 2) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[2] - (dataPtrcopy + nChan * (x + 1) + stepcopy * (y + 1))[2], 2));



                    // distancia pixei baixo centro
                    // feito no terceiro
                    // xPicel = x;
                    // yPixel = y + 1;

                    xPicel = x;
                    yPixel = y;


                    menorPessoacao = Array.IndexOf(distancias, distancias.Min());

                    switch (menorPessoacao)
                    {
                        case 0:
                        case 1:
                            (dataPtr + nChan * x + step * y)[0] = (dataPtrcopy + nChan * x + stepcopy * (y - 1))[0];
                            (dataPtr + nChan * x + step * y)[1] = (dataPtrcopy + nChan * x + stepcopy * (y - 1))[1];
                            (dataPtr + nChan * x + step * y)[2] = (dataPtrcopy + nChan * x + stepcopy * (y - 1))[2];
                            break;
                        case 2:
                            (dataPtr + nChan * x + step * y)[0] = (dataPtrcopy + nChan * (x + 1) + stepcopy * (y - 1))[0];
                            (dataPtr + nChan * x + step * y)[1] = (dataPtrcopy + nChan * (x + 1) + stepcopy * (y - 1))[1];
                            (dataPtr + nChan * x + step * y)[2] = (dataPtrcopy + nChan * (x + 1) + stepcopy * (y - 1))[2];
                            break;
                        case 3:
                        case 4:
                        case 6:
                        case 7:
                            (dataPtr + nChan * x + step * y)[0] = (dataPtrcopy + nChan * x + stepcopy * y)[0];
                            (dataPtr + nChan * x + step * y)[1] = (dataPtrcopy + nChan * x + stepcopy * y)[1];
                            (dataPtr + nChan * x + step * y)[2] = (dataPtrcopy + nChan * x + stepcopy * y)[2];
                            break;
                        case 5:
                        case 8:
                            (dataPtr + nChan * x + step * y)[0] = (dataPtrcopy + nChan * (x + 1) + stepcopy * y)[0];
                            (dataPtr + nChan * x + step * y)[1] = (dataPtrcopy + nChan * (x + 1) + stepcopy * y)[1];
                            (dataPtr + nChan * x + step * y)[2] = (dataPtrcopy + nChan * (x + 1) + stepcopy * y)[2];
                            break;
                    }


                    // canto inferior direito

                    x = width - 1; y = height - 1;


                    distancias[0] = 0;
                    distancias[1] = 0;
                    distancias[2] = 0;
                    distancias[3] = 0;
                    distancias[4] = 0;
                    distancias[5] = 0;
                    distancias[6] = 0;
                    distancias[7] = 0;
                    distancias[8] = 0;


                    xPicel = x - 1;
                    yPixel = y - 1;


                    intermedio = Math.Sqrt((Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[0] - (dataPtrcopy + nChan * (x - 0) + stepcopy * (y - 1))[0], 2)) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[1] - (dataPtrcopy + nChan * (x - 0) + stepcopy * (y - 1))[1], 2) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[2] - (dataPtrcopy + nChan * (x - 0) + stepcopy * (y - 1))[2], 2));
                    distancias[0] += intermedio;
                    distancias[1] += intermedio;
                    distancias[0] += intermedio;
                    distancias[2] += intermedio;
                    distancias[0] += intermedio;
                    distancias[6] += intermedio;


                    intermedio = Math.Sqrt((Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[0] - (dataPtrcopy + nChan * (x - 1) + stepcopy * (y - 0))[0], 2)) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[1] - (dataPtrcopy + nChan * (x - 1) + stepcopy * (y - 0))[1], 2) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[2] - (dataPtrcopy + nChan * (x - 1) + stepcopy * (y - 0))[2], 2));
                    distancias[0] += intermedio;
                    distancias[3] += intermedio;


                    intermedio = Math.Sqrt((Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[0] - (dataPtrcopy + nChan * (x - 0) + stepcopy * (y - 0))[0], 2)) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[1] - (dataPtrcopy + nChan * (x - 0) + stepcopy * (y - 0))[1], 2) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[2] - (dataPtrcopy + nChan * (x - 0) + stepcopy * (y - 0))[2], 2));
                    distancias[0] += intermedio;
                    distancias[4] += intermedio;
                    distancias[0] += intermedio;
                    distancias[5] += intermedio;
                    distancias[0] += intermedio;
                    distancias[7] += intermedio;
                    distancias[0] += intermedio;
                    distancias[8] += intermedio;



                    // distancia pixei superior central

                    xPicel = x;
                    yPixel = y - 1;


                    intermedio = Math.Sqrt((Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[0] - (dataPtrcopy + nChan * (x - 1) + stepcopy * (y - 0))[0], 2)) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[1] - (dataPtrcopy + nChan * (x - 1) + stepcopy * (y - 0))[1], 2) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[2] - (dataPtrcopy + nChan * (x - 1) + stepcopy * (y - 0))[2], 2));
                    distancias[1] += intermedio;
                    distancias[3] += intermedio;
                    distancias[1] += intermedio;
                    distancias[6] += intermedio;


                    intermedio = Math.Sqrt((Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[0] - (dataPtrcopy + nChan * (x - 0) + stepcopy * (y - 0))[0], 2)) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[1] - (dataPtrcopy + nChan * (x - 0) + stepcopy * (y - 0))[1], 2) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[2] - (dataPtrcopy + nChan * (x - 0) + stepcopy * (y - 0))[2], 2));
                    distancias[1] += intermedio;
                    distancias[4] += intermedio;
                    distancias[1] += intermedio;
                    distancias[5] += intermedio;
                    distancias[1] += intermedio;
                    distancias[7] += intermedio;
                    distancias[1] += intermedio;
                    distancias[8] += intermedio;



                    // distancia pixei direiro central
                    // feito no segundo
                    // xPicel = x + 1;
                    // yPixel = y - 1;
                    xPicel = x;
                    yPixel = y - 1;


                    intermedio = Math.Sqrt((Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[0] - (dataPtrcopy + nChan * (x - 1) + stepcopy * (y - 0))[0], 2)) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[1] - (dataPtrcopy + nChan * (x - 1) + stepcopy * (y - 0))[1], 2) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[2] - (dataPtrcopy + nChan * (x - 1) + stepcopy * (y - 0))[2], 2));
                    distancias[2] += intermedio;
                    distancias[3] += intermedio;
                    distancias[2] += intermedio;
                    distancias[6] += intermedio;


                    intermedio = Math.Sqrt((Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[0] - (dataPtrcopy + nChan * (x - 0) + stepcopy * (y - 0))[0], 2)) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[1] - (dataPtrcopy + nChan * (x - 0) + stepcopy * (y - 0))[1], 2) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[2] - (dataPtrcopy + nChan * (x - 0) + stepcopy * (y - 0))[2], 2));
                    distancias[2] += intermedio;
                    distancias[4] += intermedio;
                    distancias[2] += intermedio;
                    distancias[5] += intermedio;
                    distancias[2] += intermedio;
                    distancias[7] += intermedio;
                    distancias[2] += intermedio;
                    distancias[8] += intermedio;


                    // distancia pixei centro esquerdo

                    xPicel = x - 1;
                    yPixel = y;

                    intermedio = Math.Sqrt((Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[0] - (dataPtrcopy + nChan * (x - 0) + stepcopy * (y - 0))[0], 2)) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[1] - (dataPtrcopy + nChan * (x - 0) + stepcopy * (y - 0))[1], 2) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[2] - (dataPtrcopy + nChan * (x - 0) + stepcopy * (y - 0))[2], 2));
                    distancias[3] += intermedio;
                    distancias[4] += intermedio;
                    distancias[3] += intermedio;
                    distancias[5] += intermedio;
                    distancias[3] += intermedio;
                    distancias[7] += intermedio;
                    distancias[3] += intermedio;
                    distancias[8] += intermedio;
                    distancias[6] += intermedio;
                    distancias[7] += intermedio;
                    distancias[6] += intermedio;
                    distancias[8] += intermedio;


                    // distancia pixei centro centro
                    // 
                    xPicel = x;
                    yPixel = y;


                    intermedio = Math.Sqrt((Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[0] - (dataPtrcopy + nChan * (x - 1) + stepcopy * (y + 0))[0], 2)) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[1] - (dataPtrcopy + nChan * (x - 1) + stepcopy * (y + 0))[1], 2) + Math.Pow((dataPtrcopy + nChan * (xPicel) + stepcopy * (yPixel))[2] - (dataPtrcopy + nChan * (x - 1) + stepcopy * (y + 0))[2], 2));
                    distancias[4] += intermedio;
                    distancias[6] += intermedio;
                    distancias[5] += intermedio;
                    distancias[6] += intermedio;




                    // distancia pixei centro direita
                    // feito no quarto
                    // xPicel = x + 1;
                    // yPixel = y;

                    xPicel = x;
                    yPixel = y;


                    // distancia pixei baixo esquerda
                    // feito no terceiro
                    // xPicel = x - 1;
                    // yPixel = y + 1;

                    xPicel = x - 1;
                    yPixel = y;


                    // distancia pixei baixo centro
                    // feito no quarto mas este vai dar zero entao nao se faz nada aqui
                    // xPicel = x;
                    // yPixel = y + 1;

                    xPicel = x;
                    yPixel = y;



                    menorPessoacao = Array.IndexOf(distancias, distancias.Min());

                    switch (menorPessoacao)
                    {
                        case 0:
                            (dataPtr + nChan * x + step * y)[0] = (dataPtrcopy + nChan * (x - 1) + stepcopy * (y - 1))[0];
                            (dataPtr + nChan * x + step * y)[1] = (dataPtrcopy + nChan * (x - 1) + stepcopy * (y - 1))[1];
                            (dataPtr + nChan * x + step * y)[2] = (dataPtrcopy + nChan * (x - 1) + stepcopy * (y - 1))[2];
                            break;
                        case 1:
                        case 2:
                            (dataPtr + nChan * x + step * y)[0] = (dataPtrcopy + nChan * x + stepcopy * (y - 1))[0];
                            (dataPtr + nChan * x + step * y)[1] = (dataPtrcopy + nChan * x + stepcopy * (y - 1))[1];
                            (dataPtr + nChan * x + step * y)[2] = (dataPtrcopy + nChan * x + stepcopy * (y - 1))[2];
                            break;

                        case 3:
                        case 6:
                            (dataPtr + nChan * x + step * y)[0] = (dataPtrcopy + nChan * (x - 1) + stepcopy * y)[0];
                            (dataPtr + nChan * x + step * y)[1] = (dataPtrcopy + nChan * (x - 1) + stepcopy * y)[1];
                            (dataPtr + nChan * x + step * y)[2] = (dataPtrcopy + nChan * (x - 1) + stepcopy * y)[2];
                            break;
                        case 4:
                        case 5:
                        case 7:
                        case 8:
                            (dataPtr + nChan * x + step * y)[0] = (dataPtrcopy + nChan * x + stepcopy * y)[0];
                            (dataPtr + nChan * x + step * y)[1] = (dataPtrcopy + nChan * x + stepcopy * y)[1];
                            (dataPtr + nChan * x + step * y)[2] = (dataPtrcopy + nChan * x + stepcopy * y)[2];
                            break;

                    }

                }
            }

        }

        public static int[] Histogram_Gray(Emgu.CV.Image<Bgr, byte> img)
        {
            unsafe
            {
                int[] array = new int[256];
                MIplImage m = img.MIplImage;
                byte* dataPtr = (byte*)m.imageData.ToPointer(); // Pointer to the image
                int width = img.Width;
                int height = img.Height;
                int nChan = m.nChannels;
                int x, y;
                int red, blue, green, media;
                int step = m.widthStep;

                if (nChan == 3)
                {
                    //faz o interior
                    for (y = 0; y < height; y++)
                    {
                        for (x = 0; x < width; x++)
                        {
                            red = (dataPtr + nChan * (x) + step * (y))[2];
                            blue = (dataPtr + nChan * (x) + step * (y))[0];
                            green = (dataPtr + nChan * (x) + step * (y))[1];
                            media = red + blue + green;
                            media = (int)Math.Round(media / 3.0);

                            array[media]++;
                        }
                    }
                }

                return array;
            }
        }

        public static int[,] Histogram_RGB(Emgu.CV.Image<Bgr, byte> img)
        {
            unsafe
            {
                int[,] array = new int[3, 256];
                MIplImage m = img.MIplImage;
                byte* dataPtr = (byte*)m.imageData.ToPointer(); // Pointer to the image
                int width = img.Width;
                int height = img.Height;
                int nChan = m.nChannels;
                int x, y;

                int step = m.widthStep;

                if (nChan == 3)
                {
                    //faz o interior
                    for (y = 0; y < height; y++)
                    {
                        for (x = 0; x < width; x++)
                        {
                            array[2, (dataPtr + nChan * (x) + step * (y))[2]]++;
                            array[0, (dataPtr + nChan * (x) + step * (y))[0]]++;
                            array[1, (dataPtr + nChan * (x) + step * (y))[1]]++;
                        }
                    }
                }

                return array;
            }
        }

        public static int[,] Histogram_All(Emgu.CV.Image<Bgr, byte> img)
        {
            unsafe
            {
                int[,] array = new int[4, 256];
                MIplImage m = img.MIplImage;
                byte* dataPtr = (byte*)m.imageData.ToPointer(); // Pointer to the image
                int width = img.Width;
                int height = img.Height;
                int nChan = m.nChannels;
                int x, y;
                int red, blue, green, media;
                int step = m.widthStep;

                if (nChan == 3)
                {
                    //faz o interior
                    for (y = 0; y < height; y++)
                    {
                        for (x = 0; x < width; x++)
                        {
                            array[3, (dataPtr + nChan * (x) + step * (y))[2]]++;
                            array[1, (dataPtr + nChan * (x) + step * (y))[0]]++;
                            array[2, (dataPtr + nChan * (x) + step * (y))[1]]++;

                            red = (dataPtr + nChan * (x) + step * (y))[2];
                            blue = (dataPtr + nChan * (x) + step * (y))[0];
                            green = (dataPtr + nChan * (x) + step * (y))[1];
                            media = (int)Math.Round((red + blue + green) / 3.0);

                            array[0, media]++;
                        }
                    }
                }
                return array;
            }
        }

        public static void ConvertToBW(Emgu.CV.Image<Bgr, byte> img, int threshold)
        {
            int[,] resultado = new int[3, 256];
            unsafe
            {
                // direct access to the image memory(sequencial)
                // direcion top left -> bottom right

                MIplImage m = img.MIplImage;
                byte* dataPtr = (byte*)m.imageData.ToPointer(); // Pointer to the image
                int step = m.widthStep;
                int width = img.Width;
                int height = img.Height;
                int nChan = m.nChannels; // number of channels - 3
                int cinzento;
                if (nChan == 3) // image in RGB
                {       // parte de dentro da imagem
                    for (int y = 0; y < height; y++)
                    {
                        for (int x = 0; x < width; x++)
                        {
                            cinzento = (int)Math.Round(((dataPtr + nChan * x + step * y)[0] + (dataPtr + nChan * x + step * y)[1] + (dataPtr + nChan * x + step * y)[2]) / 3.0);
                            if (cinzento > threshold)
                            {
                                (dataPtr + nChan * x + step * y)[0] = 255;
                                (dataPtr + nChan * x + step * y)[1] = 255;
                                (dataPtr + nChan * x + step * y)[2] = 255;
                            }
                            else
                            {
                                (dataPtr + nChan * x + step * y)[0] = 0;
                                (dataPtr + nChan * x + step * y)[1] = 0;
                                (dataPtr + nChan * x + step * y)[2] = 0;
                            }
                        }
                    }
                }
            }


        }

        public static void ConvertToBW_Otsu(Emgu.CV.Image<Bgr, byte> img)
        {
            unsafe
            {
                MIplImage m = img.MIplImage;
                byte* dataPtr = (byte*)m.imageData.ToPointer(); // Pointer to the image
                int step = m.widthStep;
                double width = img.Width;
                double height = img.Height;
                int nChan = m.nChannels; // number of channels - 3
                int t, i, j;
                double[] q1 = new double[256];
                double[] q2 = new double[256];
                double[] G = new double[256];
                double[] u1 = new double[256];
                double[] u2 = new double[256];
                int[] hist = Histogram_Gray(img);

                if (nChan == 3) // image in RGB
                {       // parte de dentro da imagem
                    for (t = 0; t < 256; t++)
                    {
                        for (i = 0; i < t + 1; i++)
                        {
                            q1[t] += ((double)hist[i]) / (width * height);
                            u1[t] += (double)(i) * (((double)hist[i]) / (width * height));


                        }
                        u1[t] = u1[t] / q1[t];
                        for (j = t + 1; j < 256; j++)
                        {
                            q2[t] += ((double)(hist[j])) / (width * height);
                            u2[t] += (double)(j) * (((double)hist[j]) / (width * height));


                        }
                        u2[t] = u2[t] / q2[t];
                        G[t] = (q1[t] * q2[t]) * Math.Pow(u1[t] - u2[t], 2);
                    }

                }
                int index = Array.IndexOf(G, G.Max());

                ConvertToBW(img, index);

            }
        }

        public static void Mean_solutionB(Image<Bgr, byte> img, Image<Bgr, byte> imgCopy)
        {
            unsafe
            {
                MIplImage m = img.MIplImage;
                MIplImage auxm = imgCopy.MIplImage;
                byte* dataPtr = (byte*)m.imageData.ToPointer(); // Pointer to the image
                byte* auxdataPtr = (byte*)auxm.imageData.ToPointer(); // Pointer to the image
                int width = img.Width;
                int height = img.Height;
                int nChan = m.nChannels;
                int x, y;

                int step = m.widthStep;
                int auxStep = auxm.widthStep;
                double red, blue, green;
                //só queremos 3 canais rgb
                if (nChan == 3)
                {
                    //superior esquerdo
                    x = 0; y = 0;
                    blue = (auxdataPtr + nChan * x + auxStep * y)[0] * 4 + (auxdataPtr + nChan * (x + 1) + auxStep * y)[0] * 2 + (auxdataPtr + nChan * x + auxStep * (y + 1))[0] * 2 + (auxdataPtr + nChan * (x + 1) + auxStep * (y + 1))[0];
                    green = (auxdataPtr + nChan * x + auxStep * y)[1] * 4 + (auxdataPtr + nChan * (x + 1) + auxStep * y)[1] * 2 + (auxdataPtr + nChan * x + auxStep * (y + 1))[1] * 2 + (auxdataPtr + nChan * (x + 1) + auxStep * (y + 1))[1];
                    red = (auxdataPtr + nChan * x + auxStep * y)[2] * 4 + (auxdataPtr + nChan * (x + 1) + auxStep * y)[2] * 2 + (auxdataPtr + nChan * x + auxStep * (y + 1))[2] * 2 + (auxdataPtr + nChan * (x + 1) + auxStep * (y + 1))[2];

                    (dataPtr + nChan * x + step * y)[0] = (byte)Math.Round(blue / 9.0);
                    (dataPtr + nChan * x + step * y)[1] = (byte)Math.Round(green / 9.0);
                    (dataPtr + nChan * x + step * y)[2] = (byte)Math.Round(red / 9.0);
                    //inferior esquerdo
                    x = 0; y = height - 1;
                    blue = (auxdataPtr + nChan * x + auxStep * y)[0] * 4 + (auxdataPtr + nChan * (x + 1) + auxStep * y)[0] * 2 + (auxdataPtr + nChan * x + auxStep * (y - 1))[0] * 2 + (auxdataPtr + nChan * (x + 1) + auxStep * (y - 1))[0];
                    green = (auxdataPtr + nChan * x + auxStep * y)[1] * 4 + (auxdataPtr + nChan * (x + 1) + auxStep * y)[1] * 2 + (auxdataPtr + nChan * x + auxStep * (y - 1))[1] * 2 + (auxdataPtr + nChan * (x + 1) + auxStep * (y - 1))[1];
                    red = (auxdataPtr + nChan * x + auxStep * y)[2] * 4 + (auxdataPtr + nChan * (x + 1) + auxStep * y)[2] * 2 + (auxdataPtr + nChan * x + auxStep * (y - 1))[2] * 2 + (auxdataPtr + nChan * (x + 1) + auxStep * (y - 1))[2];

                    (dataPtr + nChan * x + step * y)[0] = (byte)Math.Round(blue / 9.0);
                    (dataPtr + nChan * x + step * y)[1] = (byte)Math.Round(green / 9.0);
                    (dataPtr + nChan * x + step * y)[2] = (byte)Math.Round(red / 9.0);

                    //inferior direito
                    x = width - 1; y = height - 1;
                    blue = (auxdataPtr + nChan * x + auxStep * y)[0] * 4 + (auxdataPtr + nChan * (x - 1) + auxStep * y)[0] * 2 + (auxdataPtr + nChan * x + auxStep * (y - 1))[0] * 2 + (auxdataPtr + nChan * (x - 1) + auxStep * (y - 1))[0];
                    green = (auxdataPtr + nChan * x + auxStep * y)[1] * 4 + (auxdataPtr + nChan * (x - 1) + auxStep * y)[1] * 2 + (auxdataPtr + nChan * x + auxStep * (y - 1))[1] * 2 + (auxdataPtr + nChan * (x - 1) + auxStep * (y - 1))[1];
                    red = (auxdataPtr + nChan * x + auxStep * y)[2] * 4 + (auxdataPtr + nChan * (x - 1) + auxStep * y)[2] * 2 + (auxdataPtr + nChan * x + auxStep * (y - 1))[2] * 2 + (auxdataPtr + nChan * (x - 1) + auxStep * (y - 1))[2];

                    (dataPtr + nChan * x + step * y)[0] = (byte)Math.Round(blue / 9.0);
                    (dataPtr + nChan * x + step * y)[1] = (byte)Math.Round(green / 9.0);
                    (dataPtr + nChan * x + step * y)[2] = (byte)Math.Round(red / 9.0);
                    //superior direito
                    x = width - 1; y = 0;
                    blue = (auxdataPtr + nChan * x + auxStep * y)[0] * 4 + (auxdataPtr + nChan * (x - 1) + auxStep * y)[0] * 2 + (auxdataPtr + nChan * x + auxStep * (y + 1))[0] * 2 + (auxdataPtr + nChan * (x - 1) + auxStep * (y + 1))[0];
                    green = (auxdataPtr + nChan * x + auxStep * y)[1] * 4 + (auxdataPtr + nChan * (x - 1) + auxStep * y)[1] * 2 + (auxdataPtr + nChan * x + auxStep * (y + 1))[1] * 2 + (auxdataPtr + nChan * (x - 1) + auxStep * (y + 1))[1];
                    red = (auxdataPtr + nChan * x + auxStep * y)[2] * 4 + (auxdataPtr + nChan * (x - 1) + auxStep * y)[2] * 2 + (auxdataPtr + nChan * x + auxStep * (y + 1))[2] * 2 + (auxdataPtr + nChan * (x - 1) + auxStep * (y + 1))[2];

                    (dataPtr + nChan * x + step * y)[0] = (byte)Math.Round(blue / 9.0);
                    (dataPtr + nChan * x + step * y)[1] = (byte)Math.Round(green / 9.0);
                    (dataPtr + nChan * x + step * y)[2] = (byte)Math.Round(red / 9.0);

                    //faz as colunas - 1ª e ultima
                    for (y = 1; y < height - 1; y++)
                    {
                        x = 0;
                        blue = (auxdataPtr + nChan * x + auxStep * y)[0] * 2 + (auxdataPtr + nChan * (x + 1) + auxStep * y)[0] + (auxdataPtr + nChan * x + auxStep * (y + 1))[0] * 2 + (auxdataPtr + nChan * (x + 1) + auxStep * (y + 1))[0] + (auxdataPtr + nChan * (x + 1) + auxStep * (y - 1))[0] + (auxdataPtr + nChan * (x) + auxStep * (y - 1))[0] * 2;
                        green = (auxdataPtr + nChan * x + auxStep * y)[1] * 2 + (auxdataPtr + nChan * (x + 1) + auxStep * y)[1] + (auxdataPtr + nChan * x + auxStep * (y + 1))[1] * 2 + (auxdataPtr + nChan * (x + 1) + auxStep * (y + 1))[1] + (auxdataPtr + nChan * (x + 1) + auxStep * (y - 1))[1] + (auxdataPtr + nChan * (x) + auxStep * (y - 1))[1] * 2;
                        red = (auxdataPtr + nChan * x + auxStep * y)[2] * 2 + (auxdataPtr + nChan * (x + 1) + auxStep * y)[2] + (auxdataPtr + nChan * x + auxStep * (y + 1))[2] * 2 + (auxdataPtr + nChan * (x + 1) + auxStep * (y + 1))[2] + (auxdataPtr + nChan * (x + 1) + auxStep * (y - 1))[2] + (auxdataPtr + nChan * (x) + auxStep * (y - 1))[2] * 2;

                        (dataPtr + nChan * x + step * y)[0] = (byte)Math.Round(blue / 9.0);
                        (dataPtr + nChan * x + step * y)[1] = (byte)Math.Round(green / 9.0);
                        (dataPtr + nChan * x + step * y)[2] = (byte)Math.Round(red / 9.0);

                        x = width - 1;
                        blue = (auxdataPtr + nChan * x + auxStep * y)[0] * 2 + (auxdataPtr + nChan * (x - 1) + auxStep * y)[0] + (auxdataPtr + nChan * x + auxStep * (y + 1))[0] * 2 + (auxdataPtr + nChan * (x - 1) + auxStep * (y + 1))[0] + (auxdataPtr + nChan * (x - 1) + auxStep * (y - 1))[0] + (auxdataPtr + nChan * (x) + auxStep * (y - 1))[0] * 2;
                        green = (auxdataPtr + nChan * x + auxStep * y)[1] * 2 + (auxdataPtr + nChan * (x - 1) + auxStep * y)[1] + (auxdataPtr + nChan * x + auxStep * (y + 1))[1] * 2 + (auxdataPtr + nChan * (x - 1) + auxStep * (y + 1))[1] + (auxdataPtr + nChan * (x - 1) + auxStep * (y - 1))[1] + (auxdataPtr + nChan * (x) + auxStep * (y - 1))[1] * 2;
                        red = (auxdataPtr + nChan * x + auxStep * y)[2] * 2 + (auxdataPtr + nChan * (x - 1) + auxStep * y)[2] + (auxdataPtr + nChan * x + auxStep * (y + 1))[2] * 2 + (auxdataPtr + nChan * (x - 1) + auxStep * (y + 1))[2] + (auxdataPtr + nChan * (x - 1) + auxStep * (y - 1))[2] + (auxdataPtr + nChan * (x) + auxStep * (y - 1))[2] * 2;

                        (dataPtr + nChan * x + step * y)[0] = (byte)Math.Round(blue / 9.0);
                        (dataPtr + nChan * x + step * y)[1] = (byte)Math.Round(green / 9.0);
                        (dataPtr + nChan * x + step * y)[2] = (byte)Math.Round(red / 9.0);

                    }
                    //faz as linhas - 1ª e ultima
                    for (x = 1; x < width - 1; x++)
                    {
                        y = 0;
                        blue = (auxdataPtr + nChan * x + auxStep * y)[0] * 2 + (auxdataPtr + nChan * (x + 1) + auxStep * y)[0] * 2 + (auxdataPtr + nChan * x + auxStep * (y + 1))[0] + (auxdataPtr + nChan * (x + 1) + auxStep * (y + 1))[0] + (auxdataPtr + nChan * (x - 1) + auxStep * y)[0] * 2 + (auxdataPtr + nChan * (x - 1) + auxStep * (y + 1))[0];
                        green = (auxdataPtr + nChan * x + auxStep * y)[1] * 2 + (auxdataPtr + nChan * (x + 1) + auxStep * y)[1] * 2 + (auxdataPtr + nChan * x + auxStep * (y + 1))[1] + (auxdataPtr + nChan * (x + 1) + auxStep * (y + 1))[1] + (auxdataPtr + nChan * (x - 1) + auxStep * y)[1] * 2 + (auxdataPtr + nChan * (x - 1) + auxStep * (y + 1))[1];
                        red = (auxdataPtr + nChan * x + auxStep * y)[2] * 2 + (auxdataPtr + nChan * (x + 1) + auxStep * y)[2] * 2 + (auxdataPtr + nChan * x + auxStep * (y + 1))[2] + (auxdataPtr + nChan * (x + 1) + auxStep * (y + 1))[2] + (auxdataPtr + nChan * (x - 1) + auxStep * y)[2] * 2 + (auxdataPtr + nChan * (x - 1) + auxStep * (y + 1))[2];

                        (dataPtr + nChan * x + step * y)[0] = (byte)Math.Round(blue / 9.0);
                        (dataPtr + nChan * x + step * y)[1] = (byte)Math.Round(green / 9.0);
                        (dataPtr + nChan * x + step * y)[2] = (byte)Math.Round(red / 9.0);

                        y = height - 1;
                        blue = (auxdataPtr + nChan * x + auxStep * y)[0] * 2 + (auxdataPtr + nChan * (x + 1) + auxStep * y)[0] * 2 + (auxdataPtr + nChan * x + auxStep * (y - 1))[0] + (auxdataPtr + nChan * (x + 1) + auxStep * (y - 1))[0] + (auxdataPtr + nChan * (x - 1) + auxStep * y)[0] * 2 + (auxdataPtr + nChan * (x - 1) + auxStep * (y - 1))[0];
                        green = (auxdataPtr + nChan * x + auxStep * y)[1] * 2 + (auxdataPtr + nChan * (x + 1) + auxStep * y)[1] * 2 + (auxdataPtr + nChan * x + auxStep * (y - 1))[1] + (auxdataPtr + nChan * (x + 1) + auxStep * (y - 1))[1] + (auxdataPtr + nChan * (x - 1) + auxStep * y)[1] * 2 + (auxdataPtr + nChan * (x - 1) + auxStep * (y - 1))[1];
                        red = (auxdataPtr + nChan * x + auxStep * y)[2] * 2 + (auxdataPtr + nChan * (x + 1) + auxStep * y)[2] * 2 + (auxdataPtr + nChan * x + auxStep * (y - 1))[2] + (auxdataPtr + nChan * (x + 1) + auxStep * (y - 1))[2] + (auxdataPtr + nChan * (x - 1) + auxStep * y)[2] * 2 + (auxdataPtr + nChan * (x - 1) + auxStep * (y - 1))[2];

                        (dataPtr + nChan * x + step * y)[0] = (byte)Math.Round(blue / 9.0);
                        (dataPtr + nChan * x + step * y)[1] = (byte)Math.Round(green / 9.0);
                        (dataPtr + nChan * x + step * y)[2] = (byte)Math.Round(red / 9.0);
                    }

                    //faz o interior
                    for (y = 1; y < height - 1; y++)
                    {
                        for (x = 1; x < width - 1; x++)
                        {
                            blue = (auxdataPtr + nChan * x + auxStep * y)[0] + (auxdataPtr + nChan * (x + 1) + auxStep * y)[0] + (auxdataPtr + nChan * x + auxStep * (y + 1))[0] + (auxdataPtr + nChan * (x + 1) + auxStep * (y + 1))[0] + (auxdataPtr + nChan * (x - 1) + auxStep * y)[0] + (auxdataPtr + nChan * (x - 1) + auxStep * (y + 1))[0] + (auxdataPtr + nChan * (x - 1) + auxStep * (y - 1))[0] + (auxdataPtr + nChan * (x) + auxStep * (y - 1))[0] + (auxdataPtr + nChan * (x + 1) + auxStep * (y - 1))[0];
                            green = (auxdataPtr + nChan * x + auxStep * y)[1] + (auxdataPtr + nChan * (x + 1) + auxStep * y)[1] + (auxdataPtr + nChan * x + auxStep * (y + 1))[1] + (auxdataPtr + nChan * (x + 1) + auxStep * (y + 1))[1] + (auxdataPtr + nChan * (x - 1) + auxStep * y)[1] + (auxdataPtr + nChan * (x - 1) + auxStep * (y + 1))[1] + (auxdataPtr + nChan * (x - 1) + auxStep * (y - 1))[1] + (auxdataPtr + nChan * (x) + auxStep * (y - 1))[1] + (auxdataPtr + nChan * (x + 1) + auxStep * (y - 1))[1];
                            red = (auxdataPtr + nChan * x + auxStep * y)[2] + (auxdataPtr + nChan * (x + 1) + auxStep * y)[2] + (auxdataPtr + nChan * x + auxStep * (y + 1))[2] + (auxdataPtr + nChan * (x + 1) + auxStep * (y + 1))[2] + (auxdataPtr + nChan * (x - 1) + auxStep * y)[2] + (auxdataPtr + nChan * (x - 1) + auxStep * (y + 1))[2] + (auxdataPtr + nChan * (x - 1) + auxStep * (y - 1))[2] + (auxdataPtr + nChan * (x) + auxStep * (y - 1))[2] + (auxdataPtr + nChan * (x + 1) + auxStep * (y - 1))[2];

                            (dataPtr + nChan * x + step * y)[0] = (byte)Math.Round(blue / 9.0);
                            (dataPtr + nChan * x + step * y)[1] = (byte)Math.Round(green / 9.0);
                            (dataPtr + nChan * x + step * y)[2] = (byte)Math.Round(red / 9.0);
                        }

                    }
                }
            }

        }

        public static void Mean_solutionC(Image<Bgr, byte> img, Image<Bgr, byte> imgCopy, int size)
        {
            unsafe
            {
                MIplImage m = img.MIplImage;
                MIplImage auxm = imgCopy.MIplImage;
                byte* dataPtr = (byte*)m.imageData.ToPointer(); // Pointer to the image
                byte* auxdataPtr = (byte*)auxm.imageData.ToPointer(); // Pointer to the image
                int width = img.Width;
                int height = img.Height;
                int nChan = m.nChannels;
                int x, y;

                int step = m.widthStep;
                int auxStep = auxm.widthStep;
                double red, blue, green;
                //só queremos 3 canais rgb
                if (nChan == 3)
                {
                    //superior esquerdo
                    x = 0; y = 0;
                    blue = (auxdataPtr + nChan * x + auxStep * y)[0] * 4 + (auxdataPtr + nChan * (x + 1) + auxStep * y)[0] * 2 + (auxdataPtr + nChan * x + auxStep * (y + 1))[0] * 2 + (auxdataPtr + nChan * (x + 1) + auxStep * (y + 1))[0];
                    green = (auxdataPtr + nChan * x + auxStep * y)[1] * 4 + (auxdataPtr + nChan * (x + 1) + auxStep * y)[1] * 2 + (auxdataPtr + nChan * x + auxStep * (y + 1))[1] * 2 + (auxdataPtr + nChan * (x + 1) + auxStep * (y + 1))[1];
                    red = (auxdataPtr + nChan * x + auxStep * y)[2] * 4 + (auxdataPtr + nChan * (x + 1) + auxStep * y)[2] * 2 + (auxdataPtr + nChan * x + auxStep * (y + 1))[2] * 2 + (auxdataPtr + nChan * (x + 1) + auxStep * (y + 1))[2];

                    (dataPtr + nChan * x + step * y)[0] = (byte)Math.Round(blue / 9.0);
                    (dataPtr + nChan * x + step * y)[1] = (byte)Math.Round(green / 9.0);
                    (dataPtr + nChan * x + step * y)[2] = (byte)Math.Round(red / 9.0);
                    //inferior esquerdo
                    x = 0; y = height - 1;
                    blue = (auxdataPtr + nChan * x + auxStep * y)[0] * 4 + (auxdataPtr + nChan * (x + 1) + auxStep * y)[0] * 2 + (auxdataPtr + nChan * x + auxStep * (y - 1))[0] * 2 + (auxdataPtr + nChan * (x + 1) + auxStep * (y - 1))[0];
                    green = (auxdataPtr + nChan * x + auxStep * y)[1] * 4 + (auxdataPtr + nChan * (x + 1) + auxStep * y)[1] * 2 + (auxdataPtr + nChan * x + auxStep * (y - 1))[1] * 2 + (auxdataPtr + nChan * (x + 1) + auxStep * (y - 1))[1];
                    red = (auxdataPtr + nChan * x + auxStep * y)[2] * 4 + (auxdataPtr + nChan * (x + 1) + auxStep * y)[2] * 2 + (auxdataPtr + nChan * x + auxStep * (y - 1))[2] * 2 + (auxdataPtr + nChan * (x + 1) + auxStep * (y - 1))[2];

                    (dataPtr + nChan * x + step * y)[0] = (byte)Math.Round(blue / 9.0);
                    (dataPtr + nChan * x + step * y)[1] = (byte)Math.Round(green / 9.0);
                    (dataPtr + nChan * x + step * y)[2] = (byte)Math.Round(red / 9.0);

                    //inferior direito
                    x = width - 1; y = height - 1;
                    blue = (auxdataPtr + nChan * x + auxStep * y)[0] * 4 + (auxdataPtr + nChan * (x - 1) + auxStep * y)[0] * 2 + (auxdataPtr + nChan * x + auxStep * (y - 1))[0] * 2 + (auxdataPtr + nChan * (x - 1) + auxStep * (y - 1))[0];
                    green = (auxdataPtr + nChan * x + auxStep * y)[1] * 4 + (auxdataPtr + nChan * (x - 1) + auxStep * y)[1] * 2 + (auxdataPtr + nChan * x + auxStep * (y - 1))[1] * 2 + (auxdataPtr + nChan * (x - 1) + auxStep * (y - 1))[1];
                    red = (auxdataPtr + nChan * x + auxStep * y)[2] * 4 + (auxdataPtr + nChan * (x - 1) + auxStep * y)[2] * 2 + (auxdataPtr + nChan * x + auxStep * (y - 1))[2] * 2 + (auxdataPtr + nChan * (x - 1) + auxStep * (y - 1))[2];

                    (dataPtr + nChan * x + step * y)[0] = (byte)Math.Round(blue / 9.0);
                    (dataPtr + nChan * x + step * y)[1] = (byte)Math.Round(green / 9.0);
                    (dataPtr + nChan * x + step * y)[2] = (byte)Math.Round(red / 9.0);
                    //superior direito
                    x = width - 1; y = 0;
                    blue = (auxdataPtr + nChan * x + auxStep * y)[0] * 4 + (auxdataPtr + nChan * (x - 1) + auxStep * y)[0] * 2 + (auxdataPtr + nChan * x + auxStep * (y + 1))[0] * 2 + (auxdataPtr + nChan * (x - 1) + auxStep * (y + 1))[0];
                    green = (auxdataPtr + nChan * x + auxStep * y)[1] * 4 + (auxdataPtr + nChan * (x - 1) + auxStep * y)[1] * 2 + (auxdataPtr + nChan * x + auxStep * (y + 1))[1] * 2 + (auxdataPtr + nChan * (x - 1) + auxStep * (y + 1))[1];
                    red = (auxdataPtr + nChan * x + auxStep * y)[2] * 4 + (auxdataPtr + nChan * (x - 1) + auxStep * y)[2] * 2 + (auxdataPtr + nChan * x + auxStep * (y + 1))[2] * 2 + (auxdataPtr + nChan * (x - 1) + auxStep * (y + 1))[2];

                    (dataPtr + nChan * x + step * y)[0] = (byte)Math.Round(blue / 9.0);
                    (dataPtr + nChan * x + step * y)[1] = (byte)Math.Round(green / 9.0);
                    (dataPtr + nChan * x + step * y)[2] = (byte)Math.Round(red / 9.0);

                    //faz as colunas - 1ª e ultima
                    for (y = 1; y < height - 1; y++)
                    {
                        x = 0;
                        blue = (auxdataPtr + nChan * x + auxStep * y)[0] * 2 + (auxdataPtr + nChan * (x + 1) + auxStep * y)[0] + (auxdataPtr + nChan * x + auxStep * (y + 1))[0] * 2 + (auxdataPtr + nChan * (x + 1) + auxStep * (y + 1))[0] + (auxdataPtr + nChan * (x + 1) + auxStep * (y - 1))[0] + (auxdataPtr + nChan * (x) + auxStep * (y - 1))[0] * 2;
                        green = (auxdataPtr + nChan * x + auxStep * y)[1] * 2 + (auxdataPtr + nChan * (x + 1) + auxStep * y)[1] + (auxdataPtr + nChan * x + auxStep * (y + 1))[1] * 2 + (auxdataPtr + nChan * (x + 1) + auxStep * (y + 1))[1] + (auxdataPtr + nChan * (x + 1) + auxStep * (y - 1))[1] + (auxdataPtr + nChan * (x) + auxStep * (y - 1))[1] * 2;
                        red = (auxdataPtr + nChan * x + auxStep * y)[2] * 2 + (auxdataPtr + nChan * (x + 1) + auxStep * y)[2] + (auxdataPtr + nChan * x + auxStep * (y + 1))[2] * 2 + (auxdataPtr + nChan * (x + 1) + auxStep * (y + 1))[2] + (auxdataPtr + nChan * (x + 1) + auxStep * (y - 1))[2] + (auxdataPtr + nChan * (x) + auxStep * (y - 1))[2] * 2;

                        (dataPtr + nChan * x + step * y)[0] = (byte)Math.Round(blue / 9.0);
                        (dataPtr + nChan * x + step * y)[1] = (byte)Math.Round(green / 9.0);
                        (dataPtr + nChan * x + step * y)[2] = (byte)Math.Round(red / 9.0);

                        x = width - 1;
                        blue = (auxdataPtr + nChan * x + auxStep * y)[0] * 2 + (auxdataPtr + nChan * (x - 1) + auxStep * y)[0] + (auxdataPtr + nChan * x + auxStep * (y + 1))[0] * 2 + (auxdataPtr + nChan * (x - 1) + auxStep * (y + 1))[0] + (auxdataPtr + nChan * (x - 1) + auxStep * (y - 1))[0] + (auxdataPtr + nChan * (x) + auxStep * (y - 1))[0] * 2;
                        green = (auxdataPtr + nChan * x + auxStep * y)[1] * 2 + (auxdataPtr + nChan * (x - 1) + auxStep * y)[1] + (auxdataPtr + nChan * x + auxStep * (y + 1))[1] * 2 + (auxdataPtr + nChan * (x - 1) + auxStep * (y + 1))[1] + (auxdataPtr + nChan * (x - 1) + auxStep * (y - 1))[1] + (auxdataPtr + nChan * (x) + auxStep * (y - 1))[1] * 2;
                        red = (auxdataPtr + nChan * x + auxStep * y)[2] * 2 + (auxdataPtr + nChan * (x - 1) + auxStep * y)[2] + (auxdataPtr + nChan * x + auxStep * (y + 1))[2] * 2 + (auxdataPtr + nChan * (x - 1) + auxStep * (y + 1))[2] + (auxdataPtr + nChan * (x - 1) + auxStep * (y - 1))[2] + (auxdataPtr + nChan * (x) + auxStep * (y - 1))[2] * 2;

                        (dataPtr + nChan * x + step * y)[0] = (byte)Math.Round(blue / 9.0);
                        (dataPtr + nChan * x + step * y)[1] = (byte)Math.Round(green / 9.0);
                        (dataPtr + nChan * x + step * y)[2] = (byte)Math.Round(red / 9.0);

                    }
                    //faz as linhas - 1ª e ultima
                    for (x = 1; x < width - 1; x++)
                    {
                        y = 0;
                        blue = (auxdataPtr + nChan * x + auxStep * y)[0] * 2 + (auxdataPtr + nChan * (x + 1) + auxStep * y)[0] * 2 + (auxdataPtr + nChan * x + auxStep * (y + 1))[0] + (auxdataPtr + nChan * (x + 1) + auxStep * (y + 1))[0] + (auxdataPtr + nChan * (x - 1) + auxStep * y)[0] * 2 + (auxdataPtr + nChan * (x - 1) + auxStep * (y + 1))[0];
                        green = (auxdataPtr + nChan * x + auxStep * y)[1] * 2 + (auxdataPtr + nChan * (x + 1) + auxStep * y)[1] * 2 + (auxdataPtr + nChan * x + auxStep * (y + 1))[1] + (auxdataPtr + nChan * (x + 1) + auxStep * (y + 1))[1] + (auxdataPtr + nChan * (x - 1) + auxStep * y)[1] * 2 + (auxdataPtr + nChan * (x - 1) + auxStep * (y + 1))[1];
                        red = (auxdataPtr + nChan * x + auxStep * y)[2] * 2 + (auxdataPtr + nChan * (x + 1) + auxStep * y)[2] * 2 + (auxdataPtr + nChan * x + auxStep * (y + 1))[2] + (auxdataPtr + nChan * (x + 1) + auxStep * (y + 1))[2] + (auxdataPtr + nChan * (x - 1) + auxStep * y)[2] * 2 + (auxdataPtr + nChan * (x - 1) + auxStep * (y + 1))[2];

                        (dataPtr + nChan * x + step * y)[0] = (byte)Math.Round(blue / 9.0);
                        (dataPtr + nChan * x + step * y)[1] = (byte)Math.Round(green / 9.0);
                        (dataPtr + nChan * x + step * y)[2] = (byte)Math.Round(red / 9.0);

                        y = height - 1;
                        blue = (auxdataPtr + nChan * x + auxStep * y)[0] * 2 + (auxdataPtr + nChan * (x + 1) + auxStep * y)[0] * 2 + (auxdataPtr + nChan * x + auxStep * (y - 1))[0] + (auxdataPtr + nChan * (x + 1) + auxStep * (y - 1))[0] + (auxdataPtr + nChan * (x - 1) + auxStep * y)[0] * 2 + (auxdataPtr + nChan * (x - 1) + auxStep * (y - 1))[0];
                        green = (auxdataPtr + nChan * x + auxStep * y)[1] * 2 + (auxdataPtr + nChan * (x + 1) + auxStep * y)[1] * 2 + (auxdataPtr + nChan * x + auxStep * (y - 1))[1] + (auxdataPtr + nChan * (x + 1) + auxStep * (y - 1))[1] + (auxdataPtr + nChan * (x - 1) + auxStep * y)[1] * 2 + (auxdataPtr + nChan * (x - 1) + auxStep * (y - 1))[1];
                        red = (auxdataPtr + nChan * x + auxStep * y)[2] * 2 + (auxdataPtr + nChan * (x + 1) + auxStep * y)[2] * 2 + (auxdataPtr + nChan * x + auxStep * (y - 1))[2] + (auxdataPtr + nChan * (x + 1) + auxStep * (y - 1))[2] + (auxdataPtr + nChan * (x - 1) + auxStep * y)[2] * 2 + (auxdataPtr + nChan * (x - 1) + auxStep * (y - 1))[2];

                        (dataPtr + nChan * x + step * y)[0] = (byte)Math.Round(blue / 9.0);
                        (dataPtr + nChan * x + step * y)[1] = (byte)Math.Round(green / 9.0);
                        (dataPtr + nChan * x + step * y)[2] = (byte)Math.Round(red / 9.0);
                    }

                    //faz o interior
                    for (y = 1; y < height - 1; y++)
                    {
                        for (x = 1; x < width - 1; x++)
                        {
                            blue = (auxdataPtr + nChan * x + auxStep * y)[0] + (auxdataPtr + nChan * (x + 1) + auxStep * y)[0] + (auxdataPtr + nChan * x + auxStep * (y + 1))[0] + (auxdataPtr + nChan * (x + 1) + auxStep * (y + 1))[0] + (auxdataPtr + nChan * (x - 1) + auxStep * y)[0] + (auxdataPtr + nChan * (x - 1) + auxStep * (y + 1))[0] + (auxdataPtr + nChan * (x - 1) + auxStep * (y - 1))[0] + (auxdataPtr + nChan * (x) + auxStep * (y - 1))[0] + (auxdataPtr + nChan * (x + 1) + auxStep * (y - 1))[0];
                            green = (auxdataPtr + nChan * x + auxStep * y)[1] + (auxdataPtr + nChan * (x + 1) + auxStep * y)[1] + (auxdataPtr + nChan * x + auxStep * (y + 1))[1] + (auxdataPtr + nChan * (x + 1) + auxStep * (y + 1))[1] + (auxdataPtr + nChan * (x - 1) + auxStep * y)[1] + (auxdataPtr + nChan * (x - 1) + auxStep * (y + 1))[1] + (auxdataPtr + nChan * (x - 1) + auxStep * (y - 1))[1] + (auxdataPtr + nChan * (x) + auxStep * (y - 1))[1] + (auxdataPtr + nChan * (x + 1) + auxStep * (y - 1))[1];
                            red = (auxdataPtr + nChan * x + auxStep * y)[2] + (auxdataPtr + nChan * (x + 1) + auxStep * y)[2] + (auxdataPtr + nChan * x + auxStep * (y + 1))[2] + (auxdataPtr + nChan * (x + 1) + auxStep * (y + 1))[2] + (auxdataPtr + nChan * (x - 1) + auxStep * y)[2] + (auxdataPtr + nChan * (x - 1) + auxStep * (y + 1))[2] + (auxdataPtr + nChan * (x - 1) + auxStep * (y - 1))[2] + (auxdataPtr + nChan * (x) + auxStep * (y - 1))[2] + (auxdataPtr + nChan * (x + 1) + auxStep * (y - 1))[2];

                            (dataPtr + nChan * x + step * y)[0] = (byte)Math.Round(blue / 9.0);
                            (dataPtr + nChan * x + step * y)[1] = (byte)Math.Round(green / 9.0);
                            (dataPtr + nChan * x + step * y)[2] = (byte)Math.Round(red / 9.0);
                        }

                    }
                }
            }
        }

        public static void Rotation_Bilinear(Image<Bgr, byte> img, Image<Bgr, byte> imgCopy, float angle)
        {
            unsafe
            {

                MIplImage m = img.MIplImage;
                MIplImage auxm = imgCopy.MIplImage;
                byte* dataPtr = (byte*)m.imageData.ToPointer(); // Pointer to the image
                byte* auxdataPtr = (byte*)auxm.imageData.ToPointer(); // Pointer to the image
                int width = img.Width;
                int height = img.Height;
                int nChan = m.nChannels; // number of channels - 3
                int x, y;
                double y0, x0;
                int step = m.widthStep;
                int auxStep = auxm.widthStep;
                int blue, green, red;
                if (nChan == 3)
                {
                    Console.WriteLine(angle);
                    for (y = 0; y < height; y++)
                    {
                        for (x = 0; x < width; x++)
                        {
                            x0 = (x - (width / 2.0)) * Math.Cos(angle) - ((height / 2.0) - y) * Math.Sin(angle) + width / 2.0;
                            y0 = (height / 2.0) - (x - (width / 2.0)) * Math.Sin(angle) - ((height / 2.0) - y) * Math.Cos(angle);

                            if (x0 >= 0 && x0 < width && y0 >= 0 && y0 < height)
                            {
                                double b1r = (1 - (x0 % 1)) * (auxdataPtr + nChan * (int)x0 + auxStep * (int)y0)[2] + (x0 % 1) * (auxdataPtr + nChan * (int)(x0 + 1) + auxStep * (int)y0)[2];
                                double b1g = (1 - (x0 % 1)) * (auxdataPtr + nChan * (int)x0 + auxStep * (int)y0)[1] + (x0 % 1) * (auxdataPtr + nChan * (int)(x0 + 1) + auxStep * (int)y0)[1];
                                double b1b = (1 - (x0 % 1)) * (auxdataPtr + nChan * (int)x0 + auxStep * (int)y0)[0] + (x0 % 1) * (auxdataPtr + nChan * (int)(x0 + 1) + auxStep * (int)y0)[0];

                                double b2r = (1 - (x0 % 1)) * (auxdataPtr + nChan * (int)x0 + auxStep * (int)(y0 + 1))[2] + (x0 % 1) * (auxdataPtr + nChan * (int)(x0 + 1) + auxStep * (int)(y0 + 1))[2];
                                double b2g = (1 - (x0 % 1)) * (auxdataPtr + nChan * (int)x0 + auxStep * (int)(y0 + 1))[1] + (x0 % 1) * (auxdataPtr + nChan * (int)(x0 + 1) + auxStep * (int)(y0 + 1))[1];
                                double b2b = (1 - (x0 % 1)) * (auxdataPtr + nChan * (int)x0 + auxStep * (int)(y0 + 1))[0] + (x0 % 1) * (auxdataPtr + nChan * (int)(x0 + 1) + auxStep * (int)(y0 + 1))[0];



                                red = (int)Math.Round((1 - (y0 % 1)) * b1r + (y0 % 1) * b2r);
                                green = (int)Math.Round((1 - (y0 % 1)) * b1g + (y0 % 1) * b2g);
                                blue = (int)Math.Round((1 - (y0 % 1)) * b1b + (y0 % 1) * b2b);
                            }
                            else
                            {
                                blue = green = red = 0;
                            }
                         (dataPtr + nChan * x + step * y)[2] = (byte)red;
                            (dataPtr + nChan * x + step * y)[1] = (byte)green;
                            (dataPtr + nChan * x + step * y)[0] = (byte)blue;
                        }
                    }

                }
            }

        }

        public static void Scale_Bilinear(Image<Bgr, byte> img, Image<Bgr, byte> imgCopy, float scaleFactor)
        {
            unsafe
            {
                MIplImage m = img.MIplImage;
                MIplImage auxm = imgCopy.MIplImage;
                byte* dataPtr = (byte*)m.imageData.ToPointer(); // Pointer to the image
                byte* auxdataPtr = (byte*)auxm.imageData.ToPointer(); // Pointer to the image
                int width = img.Width;
                int height = img.Height;
                int nChan = m.nChannels; // number of channels - 3      
                int x, y;
                double y0, x0;
                int step = m.widthStep;
                int auxStep = auxm.widthStep;
                int blue, green, red;
                if (nChan == 3)
                {
                    for (y = 0; y < height; y++)
                    {
                        for (x = 0; x < width; x++)
                        {
                            x0 = (x - (width / 2.0)) / scaleFactor + width;
                            y0 = (y - (height / 2.0)) / scaleFactor + height;
                            if (x0 >= 0 && x0 < width && y0 >= 0 && y0 < height)
                            {
                                double b1r = (1 - (x0 % 1)) * (auxdataPtr + nChan * (int)x0 + auxStep * (int)y0)[2] + (x0 % 1) * (auxdataPtr + nChan * (int)(x0 + 1) + auxStep * (int)y0)[2];
                                double b1g = (1 - (x0 % 1)) * (auxdataPtr + nChan * (int)x0 + auxStep * (int)y0)[1] + (x0 % 1) * (auxdataPtr + nChan * (int)(x0 + 1) + auxStep * (int)y0)[1];
                                double b1b = (1 - (x0 % 1)) * (auxdataPtr + nChan * (int)x0 + auxStep * (int)y0)[0] + (x0 % 1) * (auxdataPtr + nChan * (int)(x0 + 1) + auxStep * (int)y0)[0];

                                double b2r = (1 - (x0 % 1)) * (auxdataPtr + nChan * (int)x0 + auxStep * (int)(y0 + 1))[2] + (x0 % 1) * (auxdataPtr + nChan * (int)(x0 + 1) + auxStep * (int)(y0 + 1))[2];
                                double b2g = (1 - (x0 % 1)) * (auxdataPtr + nChan * (int)x0 + auxStep * (int)(y0 + 1))[1] + (x0 % 1) * (auxdataPtr + nChan * (int)(x0 + 1) + auxStep * (int)(y0 + 1))[1];
                                double b2b = (1 - (x0 % 1)) * (auxdataPtr + nChan * (int)x0 + auxStep * (int)(y0 + 1))[0] + (x0 % 1) * (auxdataPtr + nChan * (int)(x0 + 1) + auxStep * (int)(y0 + 1))[0];



                                red = (int)Math.Round((1 - (y0 % 1)) * b1r + (y0 % 1) * b2r);
                                green = (int)Math.Round((1 - (y0 % 1)) * b1g + (y0 % 1) * b2g);
                                blue = (int)Math.Round((1 - (y0 % 1)) * b1b + (y0 % 1) * b2b);
                            }
                            else
                            {
                                blue = green = red = 0;
                            }
                         (dataPtr + nChan * x + step * y)[2] = (byte)red;
                            (dataPtr + nChan * x + step * y)[1] = (byte)green;
                            (dataPtr + nChan * x + step * y)[0] = (byte)blue;

                        }
                    }
                }
            }
        }

        public static void Scale_point_xy_Bilinear(Image<Bgr, byte> img, Image<Bgr, byte> imgCopy, float scaleFactor, int centerX, int centerY)
        {
            unsafe
            {
                MIplImage m = img.MIplImage;
                MIplImage auxm = imgCopy.MIplImage;
                byte* dataPtr = (byte*)m.imageData.ToPointer(); // Pointer to the image
                byte* auxdataPtr = (byte*)auxm.imageData.ToPointer(); // Pointer to the image
                int width = img.Width;
                int height = img.Height;
                int nChan = m.nChannels; // number of channels - 3      
                int x, y;
                double y0, x0;
                int step = m.widthStep;
                int auxStep = auxm.widthStep;
                int blue, green, red;
                if (nChan == 3)
                {
                    for (y = 0; y < height; y++)
                    {
                        for (x = 0; x < width; x++)
                        {
                            x0 = (x - (width / 2)) / scaleFactor + centerX;
                            y0 = (y - (height / 2)) / scaleFactor + centerY;
                            if (x0 >= 0 && x0 < width && y0 >= 0 && y0 < height)
                            {
                                double b1r = (1 - (x0 % 1)) * (auxdataPtr + nChan * (int)x0 + auxStep * (int)y0)[2] + (x0 % 1) * (auxdataPtr + nChan * (int)(x0 + 1) + auxStep * (int)y0)[2];
                                double b1g = (1 - (x0 % 1)) * (auxdataPtr + nChan * (int)x0 + auxStep * (int)y0)[1] + (x0 % 1) * (auxdataPtr + nChan * (int)(x0 + 1) + auxStep * (int)y0)[1];
                                double b1b = (1 - (x0 % 1)) * (auxdataPtr + nChan * (int)x0 + auxStep * (int)y0)[0] + (x0 % 1) * (auxdataPtr + nChan * (int)(x0 + 1) + auxStep * (int)y0)[0];

                                double b2r = (1 - (x0 % 1)) * (auxdataPtr + nChan * (int)x0 + auxStep * (int)(y0 + 1))[2] + (x0 % 1) * (auxdataPtr + nChan * (int)(x0 + 1) + auxStep * (int)(y0 + 1))[2];
                                double b2g = (1 - (x0 % 1)) * (auxdataPtr + nChan * (int)x0 + auxStep * (int)(y0 + 1))[1] + (x0 % 1) * (auxdataPtr + nChan * (int)(x0 + 1) + auxStep * (int)(y0 + 1))[1];
                                double b2b = (1 - (x0 % 1)) * (auxdataPtr + nChan * (int)x0 + auxStep * (int)(y0 + 1))[0] + (x0 % 1) * (auxdataPtr + nChan * (int)(x0 + 1) + auxStep * (int)(y0 + 1))[0];



                                red = (int)Math.Round((1 - (y0 % 1)) * b1r + (y0 % 1) * b2r);
                                green = (int)Math.Round((1 - (y0 % 1)) * b1g + (y0 % 1) * b2g);
                                blue = (int)Math.Round((1 - (y0 % 1)) * b1b + (y0 % 1) * b2b);
                            }
                            else
                            {
                                blue = green = red = 0;
                            }
                            (dataPtr + nChan * x + step * y)[2] = (byte)red;
                            (dataPtr + nChan * x + step * y)[1] = (byte)green;
                            (dataPtr + nChan * x + step * y)[0] = (byte)blue;

                        }
                    }
                }
            }

        }


        /// <summary>
        /// Traffic Signs Detection
        /// </summary>
        /// <param name="img">Input image</param>
        /// <param name="imgCopy">Image Copy</param>
        /// <param name="limitSign">List of speed limit value and positions (speed limit value, Left-x,Top-y,Right-x,Bottom-y) of all detected limit signs</param>
        /// <param name="warningSign">List of value (-1) and positions (-1, Left-x,Top-y,Right-x,Bottom-y) of all detected warning signs</param>
        /// <param name="prohibitionSign">List of value (-1) and positions (-1, Left-x,Top-y,Right-x,Bottom-y) of all detected prohibition signs</param>
        /// <param name="level">Image Level</param>
        /// <returns>image with traffic signs detected</returns>
        public static Image<Bgr, byte> Signs(Image<Bgr, byte> img, Image<Bgr, byte> imgCopy, out List<string[]> limitSign, out List<string[]> warningSign, out List<string[]> prohibitionSign, int level)
        {

            // Funcao -> <SaveArrayAsCSV> reduz PERFORMANCE em 45%~65%


            limitSign = new List<string[]>();
            warningSign = new List<string[]>();
            prohibitionSign = new List<string[]>();

            unsafe
            {
                MIplImage mcopy = imgCopy.MIplImage;
                Image<Bgr, byte> imgFinal = img.Copy();
                byte* dataPtrcopy = (byte*)mcopy.imageData.ToPointer(); // Pointer to the imagecopy
                int stepcopy = mcopy.widthStep;
                int width = img.Width;
                int height = img.Height;
                double blue, red, green;
                MIplImage m = img.MIplImage;
                int step = m.widthStep;
                byte* dataPtr = (byte*)m.imageData.ToPointer(); // Pointer to the image
                int nChan = m.nChannels; // number of channels - 3
                double[] resultado = new double[3];
                int[,] Etiquetas = new int[height, width];
                List<int[,]> localizacaoEtiquetas = new List<int[,]>();
                List<int[,]> localizacaoEtiquetasSinal = new List<int[,]>();
                List<Image<Bgr, byte>> listaImagensSinais = new List<Image<Bgr, byte>>();
                List<Image<Bgr, byte>> listaNumerosSinais = new List<Image<Bgr, byte>>();
                List<Image<Bgr, byte>> numerosBaseDeDados = new List<Image<Bgr, byte>>();

                int[] maisParecido = new int[10];
                int soma = 0;
                int YNumeros = 246;
                int XNumeros = 156;


                String vetor1 = "", vetor2 = "", vetor3 = "", vetor4 = "";



                for (int imagens = 0; imagens < 10; imagens++)
                {
                    Image<Bgr, Byte> imgNumero = new Image<Bgr, Byte>(@"C:\Users\Utilizador\Desktop\Visual Studio Projetos\Projeto Computação Gráfica\Imagens-numero\" + imagens + ".png");
                    Bitmap bmpImage = new Bitmap(imgNumero.Bitmap, new Size(XNumeros, YNumeros));

                    bmpImage.SetResolution(300, 300);

                    Image<Bgr, byte> ImagemBaseDeDados = new Image<Bgr, byte>(bmpImage);

                    MIplImage mBaseDeDados = ImagemBaseDeDados.MIplImage;
                    byte* dataPtrBaseDeDados = (byte*)mBaseDeDados.imageData.ToPointer(); // Pointer to the imagecopy
                    int stepBaseDeDados = mBaseDeDados.widthStep;
                    int nChanBaseDeDados = mBaseDeDados.nChannels; // number of channels - 3

                    int valor = 90;
                    for (int y = 0; y < YNumeros - 1; y++)
                    {
                        for (int x = 0; x < XNumeros - 1; x++)
                        {

                            if ((dataPtrBaseDeDados + nChanBaseDeDados * x + stepBaseDeDados * y)[0] < valor && (dataPtrBaseDeDados + nChanBaseDeDados * x + stepBaseDeDados * y)[1] < valor && (dataPtrBaseDeDados + nChanBaseDeDados * x + stepBaseDeDados * y)[2] < valor)
                            {

                                (dataPtrBaseDeDados + nChanBaseDeDados * x + stepBaseDeDados * y)[0] = 255;
                                (dataPtrBaseDeDados + nChanBaseDeDados * x + stepBaseDeDados * y)[1] = 255;
                                (dataPtrBaseDeDados + nChanBaseDeDados * x + stepBaseDeDados * y)[2] = 255;

                            }
                            else
                            {
                                (dataPtrBaseDeDados + nChanBaseDeDados * x + stepBaseDeDados * y)[0] = 0;
                                (dataPtrBaseDeDados + nChanBaseDeDados * x + stepBaseDeDados * y)[1] = 0;
                                (dataPtrBaseDeDados + nChanBaseDeDados * x + stepBaseDeDados * y)[2] = 0;
                            }
                        }
                    }
                    numerosBaseDeDados.Add(ImagemBaseDeDados);// path can be absolute or relative.
                }



                for (int y = 0; y < height - 1; y++)
                {
                    for (int x = 0; x < width - 1; x++)
                    {
                        blue = ((double)(dataPtr + nChan * x + step * y)[0]) / 255;
                        green = ((double)(dataPtr + nChan * x + step * y)[1]) / 255;
                        red = ((double)(dataPtr + nChan * x + step * y)[2]) / 255;

                        resultado = TranformaRGBEmHSV(red, blue, green);

                        // valir de H esta no range dos vermelhos
                        // valor de s esta no range dos vermelhos
                        // valor do v esta no range dos vermelhos
                        if ((resultado[0] < 15 || resultado[0] > 310) && resultado[1] > 0.3 && resultado[2] > 0.30)
                        {

                            (dataPtrcopy + nChan * x + stepcopy * y)[0] = 255;
                            (dataPtrcopy + nChan * x + stepcopy * y)[1] = 255;
                            (dataPtrcopy + nChan * x + stepcopy * y)[2] = 255;

                        }
                        else
                        {
                            (dataPtrcopy + nChan * x + stepcopy * y)[0] = 0;
                            (dataPtrcopy + nChan * x + stepcopy * y)[1] = 0;
                            (dataPtrcopy + nChan * x + stepcopy * y)[2] = 0;
                        }
                    }
                }

                int nrCiclos = 2;
                fecho(nrCiclos, height, width, imgCopy);


                // descobrir e mapear todos os pixeis a branco marcando como etiquetas
                Etiquetas = EtiquetasFuncao(height, width, imgCopy, 0, 0, width, height);


                // Grava um CSV com as etiquetas (Reduz 25% da performance)
                // Com <SaveArrayAsCSV> demora ~750ms
                //SaveArrayAsCSV(Etiquetas, "etiquetas.csv", height, width);


                localizacaoEtiquetas = descobrirLocalizacaoEtiquetasEAreas(Etiquetas, width, height, 19.15);


                for (int i = 0; i < localizacaoEtiquetas.Count(); i++)
                {
                    int[] dimensoes = new int[2];
                    int heightSinal = localizacaoEtiquetas.ElementAt(i)[3, 0] - localizacaoEtiquetas.ElementAt(i)[0, 0];
                    int widthSinal = localizacaoEtiquetas.ElementAt(i)[2, 1] - localizacaoEtiquetas.ElementAt(i)[1, 1];

                    int yInicial = localizacaoEtiquetas.ElementAt(i)[0, 0];
                    int xInicial = localizacaoEtiquetas.ElementAt(i)[1, 1];

                    Rectangle rectBounding = new Rectangle(xInicial, yInicial, widthSinal, heightSinal);
                    Bgr color = new Bgr(0, 255, 255);
                    imgFinal.Draw(rectBounding, color, 5);
                    imgCopy.Draw(rectBounding, color, 5);
                    vetor1 = localizacaoEtiquetas.ElementAt(i)[1, 1] + ""; // Left-x
                    vetor2 = localizacaoEtiquetas.ElementAt(i)[0, 0] + ""; // Top-y
                    vetor3 = localizacaoEtiquetas.ElementAt(i)[2, 1] + ""; // Right-x
                    vetor4 = localizacaoEtiquetas.ElementAt(i)[3, 0] + ""; // Bottom-y

                    int altura = localizacaoEtiquetas.ElementAt(i)[3, 0] - localizacaoEtiquetas.ElementAt(i)[0, 0];
                    int largura = localizacaoEtiquetas.ElementAt(i)[2, 1] - localizacaoEtiquetas.ElementAt(i)[1, 1];



                    // se for um triangulo fazer
                    if ((localizacaoEtiquetas.ElementAt(i)[3, 0] < localizacaoEtiquetas.ElementAt(i)[2, 0] + 31) || (localizacaoEtiquetas.ElementAt(i)[0, 0] + 26 > localizacaoEtiquetas.ElementAt(i)[2, 0]))
                    {
                        string[] dummy_vector2 = new string[5];

                        dummy_vector2[0] = "-1";  // value -1
                        dummy_vector2[1] = vetor1;
                        dummy_vector2[2] = vetor2;
                        dummy_vector2[3] = vetor3;
                        dummy_vector2[4] = vetor4;
                        warningSign.Add(dummy_vector2);
                    }
                    else
                    {
                        bool souVelocidade = true;
                        listaImagensSinais.Add(img.Copy(rectBounding));

                        Image<Bgr, byte> NAOSEINOME = img.Copy(rectBounding);
                        Image<Bgr, byte> NAOSEINOMECopy = img.Copy(rectBounding);
                        int widthSinalCopy = NAOSEINOME.Width;
                        int heightSinalCopy = NAOSEINOME.Height;
                        widthSinal = NAOSEINOME.Width;
                        heightSinal = NAOSEINOME.Height;

                        MIplImage mSinal = NAOSEINOME.MIplImage;
                        byte* dataPtrSinal = (byte*)mSinal.imageData.ToPointer(); // Pointer to the image
                        int stepSinal = mSinal.widthStep;
                        int nChanSinal = mSinal.nChannels; // number of channels - 3

                        MIplImage mSinalCopy = NAOSEINOMECopy.MIplImage;
                        byte* dataPtrSinalCopy = (byte*)mSinalCopy.imageData.ToPointer(); // Pointer to the image
                        int stepSinalCopy = mSinalCopy.widthStep;
                        int nChanSinalCopy = mSinalCopy.nChannels; // number of channels - 3


                        // VER VALOR 150 passa galp mas estrago o resto
                        int valor = 80;

                        for (int y = 0; y < heightSinal - 1; y++)
                        {
                            for (int x = 0; x < widthSinal - 1; x++)
                            {


                                if ((dataPtrSinal + nChanSinal * x + stepSinal * y)[0] < valor && (dataPtrSinal + nChanSinal * x + stepSinal * y)[1] < valor && (dataPtrSinal + nChanSinal * x + stepSinal * y)[2] < valor)
                                {

                                    (dataPtrSinal + nChanSinal * x + stepSinal * y)[0] = 255;
                                    (dataPtrSinal + nChanSinal * x + stepSinal * y)[1] = 255;
                                    (dataPtrSinal + nChanSinal * x + stepSinal * y)[2] = 255;

                                }
                                else
                                {
                                    (dataPtrSinal + nChanSinal * x + stepSinal * y)[0] = 0;
                                    (dataPtrSinal + nChanSinal * x + stepSinal * y)[1] = 0;
                                    (dataPtrSinal + nChanSinal * x + stepSinal * y)[2] = 0;
                                }
                            }
                        }

                        int[,] EtiquetasSinal = new int[heightSinal, widthSinal];

                        EtiquetasSinal = EtiquetasFuncao(heightSinal, widthSinal, NAOSEINOME, widthSinal / 5, heightSinal / 5, 4 * widthSinal / 5, 4 * heightSinal / 5);
                        
                        // Grava um CSV com as etiquetas (Reduz 25% da performance)
                        // Com <SaveArrayAsCSV> demora ~750ms
                        //SaveArrayAsCSV(EtiquetasSinal, "etiquetasSinal.csv", heightSinal, widthSinal);


                        localizacaoEtiquetasSinal = descobrirLocalizacaoEtiquetasEAreas(EtiquetasSinal, widthSinal, heightSinal, 30.0);

                        // ver se tem vermelho no meu do sinal
                        for (int y = heightSinal / 4; y < 3 * heightSinal / 4; y++)
                        {
                            for (int x = widthSinal / 4; x < 3 * widthSinal / 4; x++)
                            {
                                blue = ((double)(dataPtrSinalCopy + nChanSinalCopy * x + stepSinalCopy * y)[0]) / 255;
                                green = ((double)(dataPtrSinalCopy + nChanSinalCopy * x + stepSinalCopy * y)[1]) / 255;
                                red = ((double)(dataPtrSinalCopy + nChanSinalCopy * x + stepSinalCopy * y)[2]) / 255;

                                resultado = TranformaRGBEmHSV(red, blue, green);

                                // valir de H esta no range dos vermelhos
                                // valor de s esta no range dos vermelhos
                                // valor do v esta no range dos vermelhos
                                if ((resultado[0] < 15 || resultado[0] > 310) && resultado[1] > 0.45 && resultado[2] > 0.30)
                                {
                                    souVelocidade = false;
                                    break;
                                }
                            }
                            if (souVelocidade == false)
                            {
                                break;
                            }
                        }


                        for (int j = 0; j < localizacaoEtiquetasSinal.Count(); j++)
                        {
                            dimensoes = new int[2];
                            heightSinal = localizacaoEtiquetasSinal.ElementAt(j)[3, 0] - localizacaoEtiquetasSinal.ElementAt(j)[0, 0];
                            widthSinal = localizacaoEtiquetasSinal.ElementAt(j)[2, 1] - localizacaoEtiquetasSinal.ElementAt(j)[1, 1];

                            yInicial = localizacaoEtiquetasSinal.ElementAt(j)[0, 0];
                            xInicial = localizacaoEtiquetasSinal.ElementAt(j)[1, 1];


                            // nao é sinal de velocidade
                            if (localizacaoEtiquetasSinal.Count() < 2 || localizacaoEtiquetasSinal.Count() > 3)
                            {

                                souVelocidade = false;
                                break;
                            }
                            else
                            {
                                if (widthSinal > 5 && heightSinal > 5)
                                {

                                    Rectangle rectBoundingNumero = new Rectangle(xInicial, yInicial, widthSinal, heightSinal);
                                    color = new Bgr(0, 255, 255);
                                    NAOSEINOMECopy.Draw(rectBoundingNumero, color, 5);
                                    listaNumerosSinais.Add(NAOSEINOME.Copy(rectBoundingNumero));
                                }
                                else
                                {
                                    souVelocidade = false;
                                    break;
                                }


                            }
                        }

                        bool valido = false;
                        if (souVelocidade)
                        {
                            List<int> numerosFinal = new List<int>();

                            // comparar numero com base de dados
                            for (int numero = 0; numero < listaNumerosSinais.Count(); numero++)
                            {

                                Bitmap bmpImage = new Bitmap(listaNumerosSinais[numero].Bitmap, new Size(XNumeros, YNumeros));


                                bmpImage.SetResolution(300, 300);

                                Image<Bgr, byte> ImagemBaseDeDados = new Image<Bgr, byte>(bmpImage);



                                MIplImage mNumero = ImagemBaseDeDados.MIplImage;
                                byte* dataPtrNumero = (byte*)mNumero.imageData.ToPointer(); // Pointer to the imagecopy
                                int stepNumero = mNumero.widthStep;
                                int nChanNumero = mNumero.nChannels; // number of channels - 3




                                for (int imagemNumero = 0; imagemNumero < numerosBaseDeDados.Count(); imagemNumero++)
                                {


                                    soma = 0;

                                    MIplImage mBaseDeDados = numerosBaseDeDados[imagemNumero].MIplImage;
                                    byte* dataPtrBaseDeDados = (byte*)mBaseDeDados.imageData.ToPointer(); // Pointer to the imagecopy
                                    int stepBaseDeDados = mBaseDeDados.widthStep;
                                    int nChanBaseDeDados = mBaseDeDados.nChannels; // number of channels - 3

                                    for (int y = 0; y < YNumeros; y++)
                                    {
                                        for (int x = 0; x < XNumeros; x++)
                                        {
                                            // COMPARAR AS IMAGENS
                                            double blueNumero = ((double)(dataPtrNumero + nChanNumero * x + stepNumero * y)[0]);
                                            double greenNumero = ((double)(dataPtrNumero + nChanNumero * x + stepNumero * y)[1]);
                                            double redNumero = ((double)(dataPtrNumero + nChanNumero * x + stepNumero * y)[2]);

                                            double blueBaseDeDados = (dataPtrBaseDeDados + nChanBaseDeDados * x + stepBaseDeDados * y)[0];
                                            double greenBaseDeDados = (dataPtrBaseDeDados + nChanBaseDeDados * x + stepBaseDeDados * y)[1];
                                            double redBaseDeDados = (dataPtrBaseDeDados + nChanBaseDeDados * x + stepBaseDeDados * y)[2];

                                            if (blueNumero == blueBaseDeDados && greenNumero == greenBaseDeDados && redNumero == redBaseDeDados)
                                            {

                                                soma++;
                                            }

                                        }
                                    }

                                    maisParecido[imagemNumero] = soma;



                                }


                                // ver qual o maior e dizer que esse é o numero

                                int max = maisParecido.Max();

                                for (int a = 0; a < maisParecido.Count(); a++)
                                {
                                    if (maisParecido[a] == max)
                                    {
                                        numerosFinal.Add(a);
                                        break;
                                    }
                                }


                                maisParecido[0] = 0;
                                maisParecido[1] = 0;
                                maisParecido[2] = 0;
                                maisParecido[3] = 0;
                                maisParecido[4] = 0;
                                maisParecido[5] = 0;
                                maisParecido[6] = 0;
                                maisParecido[7] = 0;
                                maisParecido[8] = 0;
                                maisParecido[9] = 0;
                            }


                            // escrever velocidade
                            String velocidade = "";
                            if (numerosFinal.Count() == 2)
                            {
                                numerosFinal.Sort();
                                numerosFinal.Reverse();
                                for (int naoSei = 0; naoSei < numerosFinal.Count(); naoSei++)
                                {
                                    velocidade += numerosFinal[naoSei] + "";
                                    // tem um 0 com 2 digitos é valido
                                    if (numerosFinal[naoSei] == 0)
                                    {
                                        valido = true;
                                    }
                                }
                            }

                            if (numerosFinal.Count() == 3)
                            {
                                numerosFinal.Sort();
                                numerosFinal.Reverse();
                                velocidade += "1";
                                for (int naoSei = 0; naoSei < numerosFinal.Count(); naoSei++)
                                {
                                    if (numerosFinal[naoSei] == 1)
                                    {
                                        continue;
                                    }
                                    // tem um 0 com 3 digitos é valido
                                    if (numerosFinal[naoSei] == 0)
                                    {
                                        valido = true;
                                    }
                                    velocidade += numerosFinal[naoSei] + "";

                                }
                            }


                            // preenche o vetor das velocidades e adiciona-o
                            if (valido)
                            {
                                string[] dummy_vector1 = new string[5];
                                dummy_vector1[0] = velocidade;   // Speed limit
                                dummy_vector1[1] = vetor1;
                                dummy_vector1[2] = vetor2;

                                dummy_vector1[3] = vetor3;
                                dummy_vector1[4] = vetor4;
                                limitSign.Add(dummy_vector1);

                            }
                            // preenche o vetor dos proibidos e adiciona-o
                            else
                            {
                                string[] dummy_vector3 = new string[5];
                                dummy_vector3[0] = "-1";
                                dummy_vector3[1] = vetor1;
                                dummy_vector3[2] = vetor2;

                                dummy_vector3[3] = vetor3;
                                dummy_vector3[4] = vetor4;

                                prohibitionSign.Add(dummy_vector3);
                            }

                        }

                        // preenche o vetor dos proibidos e adiciona-o
                        else
                        {
                            string[] dummy_vector3 = new string[5];
                            dummy_vector3[0] = "-1";
                            dummy_vector3[1] = vetor1;
                            dummy_vector3[2] = vetor2;

                            dummy_vector3[3] = vetor3;
                            dummy_vector3[4] = vetor4;

                            prohibitionSign.Add(dummy_vector3);
                        }
                        listaNumerosSinais.Clear();

                    }

                }
                return imgFinal;
            }
        }
        

        public static int[,] EtiquetasFuncao(int height, int width, Image<Bgr, byte> img, int xInicial, int yInicial, int xFinal, int yFinal)
        {
            unsafe
            {


                bool alteracao = true;
                MIplImage m = img.MIplImage;
                int step = m.widthStep;
                int cor;
                int[,] Etiquetas = new int[height, width];
                int nChan = m.nChannels; // number of channels - 3
                int numeroEtiqueda = 1;
                byte* dataPtr = (byte*)m.imageData.ToPointer(); // Pointer to the image

                for (int y = yInicial; y < yFinal; y++)
                {
                    for (int x = xInicial; x < xFinal; x++)
                    {
                        cor = (dataPtr + nChan * x + step * y)[0];
                        if (cor == 255)
                        {
                            Etiquetas[y, x] = numeroEtiqueda++;
                        }
                    }
                }


                do
                {
                    int maisPequeno = 0;
                    alteracao = false;
                    for (int y = 1; y < height - 1; y++)
                    {
                        for (int x = 1; x < width - 1; x++)
                        {
                            if (Etiquetas[y, x] != 0)
                            {

                                maisPequeno = Etiquetas[y, x];


                                // cima esquerda
                                if (maisPequeno > Etiquetas[y - 1, x - 1] && Etiquetas[y - 1, x - 1] != 0)
                                {
                                    alteracao = true;
                                    maisPequeno = Etiquetas[y - 1, x - 1];
                                }
                                // cima meio
                                if (maisPequeno > Etiquetas[y - 1, x] && Etiquetas[y - 1, x] != 0)
                                {
                                    alteracao = true;
                                    maisPequeno = Etiquetas[y - 1, x];
                                }
                                // cima direita
                                if (maisPequeno > Etiquetas[y - 1, x + 1] && Etiquetas[y - 1, x + 1] != 0)
                                {
                                    alteracao = true;
                                    maisPequeno = Etiquetas[y - 1, x + 1];
                                }
                                // centro esquerda
                                if (maisPequeno > Etiquetas[y, x - 1] && Etiquetas[y, x - 1] != 0)
                                {
                                    alteracao = true;
                                    maisPequeno = Etiquetas[y, x - 1];
                                }

                                // centro direito
                                if (maisPequeno > Etiquetas[y, x + 1] && Etiquetas[y, x + 1] != 0)
                                {
                                    alteracao = true;
                                    maisPequeno = Etiquetas[y, x + 1];
                                }
                                // baixo esquerda
                                if (maisPequeno > Etiquetas[y + 1, x - 1] && Etiquetas[y + 1, x - 1] != 0)
                                {
                                    alteracao = true;
                                    maisPequeno = Etiquetas[y + 1, x - 1];
                                }
                                // baixo centro
                                if (maisPequeno > Etiquetas[y + 1, x] && Etiquetas[y + 1, x] != 0)
                                {
                                    alteracao = true;
                                    maisPequeno = Etiquetas[y + 1, x];
                                }
                                // baixo direito
                                if (maisPequeno > Etiquetas[y + 1, x + 1] && Etiquetas[y + 1, x + 1] != 0)
                                {
                                    alteracao = true;
                                    maisPequeno = Etiquetas[y + 1, x + 1];
                                }

                                Etiquetas[y, x] = maisPequeno;

                            }
                        }

                    }
                    if (!alteracao)
                    {
                        break;
                    }

                    alteracao = false;
                    for (int y = height - 2; y > 0; y--)
                    {
                        for (int x = width - 2; x > 0; x--)
                        {
                            if (Etiquetas[y, x] != 0)
                            {
                                maisPequeno = Etiquetas[y, x];


                                // cima esquerda
                                if (maisPequeno > Etiquetas[y - 1, x - 1] && Etiquetas[y - 1, x - 1] != 0)
                                {
                                    alteracao = true;
                                    maisPequeno = Etiquetas[y - 1, x - 1];
                                }
                                // cima meio
                                if (maisPequeno > Etiquetas[y - 1, x] && Etiquetas[y - 1, x] != 0)
                                {
                                    alteracao = true;
                                    maisPequeno = Etiquetas[y - 1, x];
                                }
                                // cima direita
                                if (maisPequeno > Etiquetas[y - 1, x + 1] && Etiquetas[y - 1, x + 1] != 0)
                                {
                                    alteracao = true;
                                    maisPequeno = Etiquetas[y - 1, x + 1];
                                }
                                // centro esquerda
                                if (maisPequeno > Etiquetas[y, x - 1] && Etiquetas[y, x - 1] != 0)
                                {
                                    alteracao = true;
                                    maisPequeno = Etiquetas[y, x - 1];
                                }

                                // centro direito
                                if (maisPequeno > Etiquetas[y, x + 1] && Etiquetas[y, x + 1] != 0)
                                {
                                    alteracao = true;
                                    maisPequeno = Etiquetas[y, x + 1];
                                }
                                // baixo esquerda
                                if (maisPequeno > Etiquetas[y + 1, x - 1] && Etiquetas[y + 1, x - 1] != 0)
                                {
                                    alteracao = true;
                                    maisPequeno = Etiquetas[y + 1, x - 1];
                                }
                                // baixo centro
                                if (maisPequeno > Etiquetas[y + 1, x] && Etiquetas[y + 1, x] != 0)
                                {
                                    alteracao = true;
                                    maisPequeno = Etiquetas[y + 1, x];
                                }
                                // baixo direito
                                if (maisPequeno > Etiquetas[y + 1, x + 1] && Etiquetas[y + 1, x + 1] != 0)
                                {
                                    alteracao = true;
                                    maisPequeno = Etiquetas[y + 1, x + 1];
                                }

                                Etiquetas[y, x] = maisPequeno;
                            }
                        }
                    }


                } while (alteracao);

                return Etiquetas;
            }
        }

        public static double[] TranformaRGBEmHSV(double red, double blue, double green)
        {
            double h = 0, s = 0, v = 0;
            double max = 0, min = 0;
            double[] resultado = new double[3];
            if (red > blue && red > green)
            {
                max = red;
                if (green >= blue)
                {
                    min = blue;
                    h = 60 * (green - blue) / (max - min) + 0;
                }
                if (blue > green)
                {
                    min = green;
                    h = 60 * (green - blue) / (max - min) + 360;
                }
            }
            if (green > red && green > blue)
            {
                max = green;
                if (red > blue)
                {
                    min = blue;
                }
                else
                {
                    min = red;
                }
                h = 60 * (blue - red) / (max - min) + 120;
            }
            if (blue > red && blue > green)
            {
                max = blue;
                if (red > green)
                {
                    min = green;
                }
                else
                {
                    min = red;
                }
                h = 60 * (red - green) / (max - min) + 240;
            }
            if (max > 0)
            {
                s = (max - min) / max;
            }
            v = max;

            resultado[0] = h;
            resultado[1] = s;
            resultado[2] = v;
            return resultado;

        }

        public static void SaveArrayAsCSV(int[,] jaggedArrayToSave, string fileName, int height, int width)
        {
            using (StreamWriter file = new StreamWriter(fileName))
            {
                for (int y = 0; y < height - 1; y++)
                {
                    for (int x = 0; x < width - 1; x++)
                    {
                        file.Write(jaggedArrayToSave[y, x] + ";");
                    }
                    file.Write(Environment.NewLine);
                }
            }
        }

        public static List<int[,]> descobrirLocalizacaoEtiquetasEAreas(int[,] Etiquetas, int width, int height, double trashHold)
        {
            Dictionary<int, int> areas = new Dictionary<int, int>();
            HashSet<int> numeroEtiquetas = new HashSet<int>();
            List<int[,]> resultado = new List<int[,]>();
            List<int[]> listaAreas = new List<int[]>();

            int[,] localizacaoEtiquetas = new int[4, 2];

            // adicionar as etiquetas num hash set para saber quais os valores das etiquetas 
            for (int y = 0; y < height - 1; y++)
            {
                for (int x = 0; x < width - 1; x++)
                {
                    if (Etiquetas[y, x] != 0)
                    {
                        numeroEtiquetas.Add(Etiquetas[y, x]);
                    }
                }
            }


            int[] etiquetasArray = new int[numeroEtiquetas.Count()];


            numeroEtiquetas.CopyTo(etiquetasArray);


            // descobrir quantas vezes aparece uma determinada etiqueta
            for (int y = 0; y < height - 1; y++)
            {
                for (int x = 0; x < width - 1; x++)
                {

                    if (Etiquetas[y, x] != 0)
                    {
                        int key = Etiquetas[y, x];
                        int valor = 0;
                        if (areas.ContainsKey(key))
                        {
                            areas.TryGetValue(key, out valor);
                            areas.Remove(key);
                            areas.Add(key, valor + 1);
                        }
                        else
                        {
                            areas.Add(key, 1);
                        }

                    }

                }
            }
            // descobrir localiação das etiquetas
            for (int i = 0; i < etiquetasArray.Count(); i++)
            {
                localizacaoEtiquetas[0, 0] = height;
                localizacaoEtiquetas[0, 1] = height;
                localizacaoEtiquetas[1, 0] = width;
                localizacaoEtiquetas[1, 1] = width;
                localizacaoEtiquetas[2, 0] = -1;
                localizacaoEtiquetas[2, 1] = -1;
                localizacaoEtiquetas[3, 0] = -1;
                localizacaoEtiquetas[3, 1] = -1;
                int etiqueta = etiquetasArray.ElementAt(i);
                for (int y = 0; y < height - 1; y++)
                {
                    for (int x = 0; x < width - 1; x++)
                    {
                        if (Etiquetas[y, x] == etiqueta)
                        {
                            if (y < localizacaoEtiquetas[0, 0])
                            {
                                localizacaoEtiquetas[0, 0] = y;
                                localizacaoEtiquetas[0, 1] = x;
                            }
                            if (y > localizacaoEtiquetas[3, 0])
                            {
                                localizacaoEtiquetas[3, 0] = y;
                                localizacaoEtiquetas[3, 1] = x;
                            }
                            if (x < localizacaoEtiquetas[1, 1])
                            {
                                localizacaoEtiquetas[1, 0] = y;
                                localizacaoEtiquetas[1, 1] = x;
                            }
                            if (x > localizacaoEtiquetas[2, 1])
                            {
                                localizacaoEtiquetas[2, 0] = y;
                                localizacaoEtiquetas[2, 1] = x;
                            }


                        }
                    }
                }
                int altura = localizacaoEtiquetas[3, 0] - localizacaoEtiquetas[0, 0];
                int largura = localizacaoEtiquetas[2, 1] - localizacaoEtiquetas[1, 1];
                int[] adicionar = new int[2];
                int valor = 0;
                areas.TryGetValue(etiqueta, out valor);
                adicionar[0] = largura * altura;
                adicionar[1] = etiqueta;

                listaAreas.Add(adicionar);

            }

            double maxValueAreas = 0;
            for (int i = 0; i < listaAreas.Count(); i++)
            {
                if (maxValueAreas < listaAreas.ElementAt(i)[0])
                {
                    maxValueAreas = listaAreas.ElementAt(i)[0];
                }
            }

            double maiorSinal = maxValueAreas;


            for (int i = 0; i < listaAreas.Count(); i++)
            {
                double areaPossivelSinal = listaAreas.ElementAt(i)[0];


                // Não é sinal
                double teste = 100.0 * areaPossivelSinal / maiorSinal;
                if (teste < trashHold)
                {
                    listaAreas.RemoveAt(i);
                    i--;
                }

            }

            for (int i = 0; i < listaAreas.Count(); i++)
            {
                int etiqueta = listaAreas.ElementAt(i)[1];
                resultado.Add(adicionarLocalizacao(height, width, etiqueta, Etiquetas));

                // Grava um CSV com as etiquetas (Reduz 25% da performance)
                // Com <SaveArrayAsCSV> demora ~750ms
                //SaveArrayAsCSV(Etiquetas, "teste.csv", height, width);
            }

            return resultado;
        }

        public static int[,] adicionarLocalizacao(int height, int width, int etiqueta, int[,] Etiquetas)
        {
            int[,] localizacaoEtiquetas = new int[4, 2];
            localizacaoEtiquetas[0, 0] = height;
            localizacaoEtiquetas[0, 1] = width;
            localizacaoEtiquetas[1, 0] = height;
            localizacaoEtiquetas[1, 1] = width;
            localizacaoEtiquetas[2, 0] = -1;
            localizacaoEtiquetas[2, 1] = -1;
            localizacaoEtiquetas[3, 0] = -1;
            localizacaoEtiquetas[3, 1] = -1;

            for (int y = 0; y < height - 1; y++)
            {
                for (int x = 0; x < width - 1; x++)
                {
                    if (Etiquetas[y, x] == etiqueta)
                    {
                        if (y < localizacaoEtiquetas[0, 0])
                        {
                            localizacaoEtiquetas[0, 0] = y;
                            localizacaoEtiquetas[0, 1] = x;
                        }
                        if (y > localizacaoEtiquetas[3, 0])
                        {
                            localizacaoEtiquetas[3, 0] = y;
                            localizacaoEtiquetas[3, 1] = x;
                        }
                        if (x < localizacaoEtiquetas[1, 1])
                        {
                            localizacaoEtiquetas[1, 0] = y;
                            localizacaoEtiquetas[1, 1] = x;
                        }
                        if (x > localizacaoEtiquetas[2, 1])
                        {
                            localizacaoEtiquetas[2, 0] = y;
                            localizacaoEtiquetas[2, 1] = x;
                        }


                    }
                }
            }
            return localizacaoEtiquetas;

        }

        public static void dilatacao(int height, int width, Image<Bgr, byte> img)
        {
            // mascara utilizada e 1 1 1
            //                     1 1 1
            //                     1 1 1 

            // fazer dilatação
            Image<Bgr, byte> imageCopy = img.Copy();

            unsafe
            {
                bool alteracaoDilatacao = false;
                MIplImage m = img.MIplImage;

                int y, x;
                MIplImage mCopy = imageCopy.MIplImage;
                int step = m.widthStep;
                int stepCopy = mCopy.widthStep;
                int nChan = m.nChannels; // number of channels - 3
                byte* dataPtr = (byte*)m.imageData.ToPointer(); // Pointer to the image
                byte* dataPtrCopy = (byte*)mCopy.imageData.ToPointer(); // Pointer to the image




                for (y = 1; y < height - 1; y++)
                {
                    for (x = 1; x < width - 1; x++)
                    {

                        // cima esquerda
                        if ((dataPtrCopy + nChan * (x - 1) + stepCopy * (y - 1))[0] == 255)
                        {
                            alteracaoDilatacao = true;
                        }
                        // cima meio
                        if ((dataPtrCopy + nChan * x + stepCopy * (y - 1))[0] == 255)
                        {
                            alteracaoDilatacao = true;
                        }
                        // cima direita
                        if ((dataPtrCopy + nChan * (x + 1) + stepCopy * (y - 1))[0] == 255)
                        {
                            alteracaoDilatacao = true;
                        }
                        // centro esquerda
                        if ((dataPtrCopy + nChan * (x - 1) + stepCopy * y)[0] == 255)
                        {
                            alteracaoDilatacao = true;
                        }
                        // centro direita
                        if ((dataPtrCopy + nChan * (x + 1) + stepCopy * y)[0] == 255)
                        {
                            alteracaoDilatacao = true;
                        }

                        // baixo esqueda
                        if ((dataPtrCopy + nChan * (x - 1) + stepCopy * (y + 1))[0] == 255)
                        {
                            alteracaoDilatacao = true;
                        }
                        // baixo centro
                        if ((dataPtrCopy + nChan * x + stepCopy * (y + 1))[0] == 255)
                        {
                            alteracaoDilatacao = true;
                        }
                        // baixo direira
                        if ((dataPtrCopy + nChan * (x + 1) + stepCopy * (y + 1))[0] == 255)
                        {
                            alteracaoDilatacao = true;
                        }



                        if (alteracaoDilatacao)
                        {
                            (dataPtr + nChan * x + step * y)[0] = 255;
                            (dataPtr + nChan * x + step * y)[1] = 255;
                            (dataPtr + nChan * x + step * y)[2] = 255;
                        }

                        alteracaoDilatacao = false;


                    }
                }

            }




        }

        public static void erosao(int height, int width, Image<Bgr, byte> img)
        {
            // fazer erosão
            Image<Bgr, byte> imageCopy = img.Copy();

            unsafe
            {
                int alteracaoErosao = 0;
                MIplImage m = img.MIplImage;

                int y, x;
                MIplImage mCopy = imageCopy.MIplImage;
                int step = m.widthStep;
                int stepCopy = mCopy.widthStep;
                int nChan = m.nChannels; // number of channels - 3
                byte* dataPtr = (byte*)m.imageData.ToPointer(); // Pointer to the image
                byte* dataPtrCopy = (byte*)mCopy.imageData.ToPointer(); // Pointer to the image


                for (y = 1; y < height - 1; y++)
                {
                    for (x = 1; x < width - 1; x++)
                    {

                        // cima esquerda
                        if ((dataPtrCopy + nChan * (x - 1) + stepCopy * (y - 1))[0] == 255)
                        {
                            alteracaoErosao++;
                        }
                        // cima meio
                        if ((dataPtrCopy + nChan * x + stepCopy * (y - 1))[0] == 255)
                        {
                            alteracaoErosao++;
                        }
                        // cima direita
                        if ((dataPtrCopy + nChan * (x + 1) + stepCopy * (y - 1))[0] == 255)
                        {
                            alteracaoErosao++;
                        }
                        // centro esquerda
                        if ((dataPtrCopy + nChan * (x - 1) + stepCopy * y)[0] == 255)
                        {
                            alteracaoErosao++;
                        }
                        // centro meio
                        if ((dataPtrCopy + nChan * x + stepCopy * y)[0] == 255)
                        {
                            alteracaoErosao++;
                        }

                        // centro direita
                        if ((dataPtrCopy + nChan * (x + 1) + stepCopy * y)[0] == 255)
                        {
                            alteracaoErosao++;
                        }

                        // baixo esqueda
                        if ((dataPtrCopy + nChan * (x - 1) + stepCopy * (y + 1))[0] == 255)
                        {
                            alteracaoErosao++;
                        }
                        // baixo centro
                        if ((dataPtrCopy + nChan * x + stepCopy * (y + 1))[0] == 255)
                        {
                            alteracaoErosao++;
                        }
                        // baixo direira
                        if ((dataPtrCopy + nChan * (x + 1) + stepCopy * (y + 1))[0] == 255)
                        {
                            alteracaoErosao++;
                        }



                        if (alteracaoErosao == 9)
                        {
                            (dataPtr + nChan * x + step * y)[0] = 255;
                            (dataPtr + nChan * x + step * y)[1] = 255;
                            (dataPtr + nChan * x + step * y)[2] = 255;
                        }

                        alteracaoErosao = 0;


                    }
                }
            }
        }

        public static void fecho(int nrCiclos, int height, int width, Image<Bgr, byte> img)
        {

            while (nrCiclos > 0)
            {
                nrCiclos--;
                dilatacao(height, width, img);
                erosao(height, width, img);
            }
        }

        public static void abertura(int nrCiclos, int height, int width, Image<Bgr, byte> img)
        {
            while (nrCiclos > 0)
            {
                nrCiclos--;

                erosao(height, width, img);
                dilatacao(height, width, img);
            }
        }

    }
}
