﻿using System;
using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Screenshots
{
    class TransparencyCalc
    {
        private BitmapSource _white, _black;

        public TransparencyCalc(BitmapSource white, BitmapSource black)
        {
            _white = white;
            _black = black;
        }

        public BitmapSource GetTransparent()
        {
            var blackPixels = ReadAllBytes(SourceToImageConverter(_black));
            var whitePixels = ReadAllBytes(SourceToImageConverter(_white));

            var resultBytes = CalcDiff(blackPixels, whitePixels);

            return BitmapSource.Create(_white.PixelWidth, _white.PixelHeight, _white.DpiX, _white.DpiY, PixelFormats.Bgra32, _white.Palette, resultBytes, _white.PixelWidth * (_white.Format.BitsPerPixel / 8));
        }

        private BitmapImage SourceToImageConverter(BitmapSource src)
        {
            var encoder = new PngBitmapEncoder();
            var ms = new MemoryStream();

            var image = new BitmapImage();

            encoder.Frames.Add(BitmapFrame.Create(src));
            encoder.Save(ms);

            image.BeginInit();
            image.StreamSource = ms;
            image.CreateOptions = BitmapCreateOptions.None;
            image.CacheOption = BitmapCacheOption.Default;
            image.EndInit();

            return image;
        }

        private byte[] ReadAllBytes(BitmapImage img)
        {
            var stride = img.PixelWidth * (img.Format.BitsPerPixel / 8);
            var pixels = new byte[stride * img.PixelHeight];

            img.CopyPixels(pixels, stride, 0);

            return pixels;
        }

        private byte[] CalcDiff(byte[] black, byte[] white)
        {
            byte[] result = new byte[black.Length];

            for (int i = 0; i < black.Length; i += 4)
            {
                var r0 = black[i + 2];
                var r1 = white[i + 2];

                var g0 = black[i + 1];

                var b0 = black[i];

                var AS = Math.Min(r0 - r1 + 255, 255);

                if (AS == 0)
                {
                    result[i] = result[i + 1] = result[i + 2] = result[i + 3] = 0;
                }
                else
                {
                    var ASP = AS / 255.0;
                    result[i] = Convert.ToByte(Math.Min(255, b0 / ASP));
                    result[i + 1] = Convert.ToByte(Math.Min(255, g0 / ASP));
                    result[i + 2] = Convert.ToByte(Math.Min(255, r0 / ASP));
                    result[i + 3] = Convert.ToByte(AS);
                }
            }

            return result;
        }
    }
}
