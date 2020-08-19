using DatingCode.Basics;
using DatingCode.BusinessModels.Market;
using DatingCode.Core;
using DatingCode.Media.Utils;
using System;
using System.Drawing;
using System.IO;
using Xunit;

namespace DatingTests.Code.Utils
{
    public class ImageConversionUtilTests
    {
        [Fact]
        public void CanFit_to_Smaller()
        {
            var maxSize = new Dimention(width: 200, height: 400);
            var resizeSettings = new ResizeOption(ResizeOperationType.Fit, ImageType.Preview_M, maxSize);
            var result = Resize(sourceFileName: "Lama.jpg", targetDimention: resizeSettings, dstFileName: "Lama fit small.jpg");

            Assert.True(result.Success);
        }

        [Fact]
        public void CanFit_to_Bigger()
        {
            var maxSize = new Dimention(width: 1500, height: 1500);
            var resizeSettings = new ResizeOption(ResizeOperationType.Fit, ImageType.Full_L, maxSize);
            var result = Resize(sourceFileName: "Lama.jpg", targetDimention: resizeSettings, dstFileName: "Lama fit Big.jpg");

            Assert.True(result.Success);
        }

        [Fact]
        public void CanFill_to_Smaller()
        {
            var maxSize = new Dimention(width: 200, height: 400);
            var resizeSettings = new ResizeOption(ResizeOperationType.Fill, ImageType.Preview_M, maxSize);
            var result = Resize(sourceFileName: "Lama.jpg", targetDimention: resizeSettings, dstFileName: "Lama fill small.jpg");
            Assert.True(result.Success);
        }

        [Fact]
        public void CanFill_to_Bigger()
        {
            var maxSize = new Dimention(width: 1500, height: 1500);
            var resizeSettings = new ResizeOption(ResizeOperationType.Fill, ImageType.Preview_M, maxSize);
            var result = Resize(sourceFileName: "Lama.jpg", targetDimention: resizeSettings, dstFileName: "Lama fill Big.jpg");
            Assert.True(result.Success);
        }

        private Result<SizedImageData> Resize(string sourceFileName, ResizeOption targetDimention, string dstFileName)
        {
            var lamaPath = Path.Combine(Environment.CurrentDirectory, "Files", "Pics", sourceFileName);
            var lamaBytes = File.ReadAllBytes(lamaPath);
            var converter = new ImagesConversionUtil();
            var scaledBytes = converter.GetScaledJpegBytes(lamaBytes, targetDimention);
            if (!scaledBytes.Success)
                Assert.False(true);

            var tmpPath = $@"C:\Users\Andrey\Desktop\{dstFileName}";
            using (var imageReadStream = new MemoryStream(scaledBytes.Value.Bytes))
            using (var img = Image.FromStream(imageReadStream))
            {
                img.Save(tmpPath);
            }

            return scaledBytes;
        }
    }
}
