using DatingCode.Basics;
using DatingCode.BusinessModels.Market;
using DatingCode.Core;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;

namespace DatingCode.Media.Utils
{
    public class ImagesConversionUtil
    {
        public Result<SizedImageData> GetScaledJpegBytes(byte[] data, ResizeOption resizeOption)
        {
            if (resizeOption.ImgType == ImageType.Unknown)
                return Result<SizedImageData>.NewFailure($"{nameof(resizeOption.ImgType)} {resizeOption.ImgType} is not supported.");
            if (resizeOption.OperationType == ResizeOperationType.Unknown)
                return Result<SizedImageData>.NewFailure($"{nameof(resizeOption.OperationType)} {resizeOption.OperationType} is not supported.");


            byte[] resizedBytes;
            Dimention newSize;

            try
            {
                using (var imageReadStream = new MemoryStream(data))
                using (var sourceImg = Image.FromStream(imageReadStream))
                {
                    var sourceSize = new Dimention(height: sourceImg.Height, width: sourceImg.Width);

                    switch (resizeOption.OperationType)
                    {
                        case ResizeOperationType.Raw:
                            return Result<SizedImageData>.NewSuccess(new SizedImageData(resizeOption.ImgType, sourceSize, data));
                        case ResizeOperationType.Fit:
                            newSize = sourceSize.GetSizeToFitInto(resizeOption.Size);
                            // Resize
                            using (var outputMs = new MemoryStream())
                            {
                                using (var canvas = new Bitmap(width: newSize.Width, height: newSize.Height))
                                using (var graphics = Graphics.FromImage(canvas))
                                {
                                    graphics.CompositingQuality = CompositingQuality.HighSpeed;
                                    graphics.InterpolationMode = InterpolationMode.HighQualityBilinear;
                                    graphics.CompositingMode = CompositingMode.SourceCopy;
                                    graphics.DrawImage(sourceImg, 0, 0, width: newSize.Width, height: newSize.Height);

                                    canvas.Save(outputMs, ImageFormat.Jpeg);
                                }

                                resizedBytes = outputMs.ToArray();
                            }
                            break;
                        case ResizeOperationType.Fill:
                            newSize = resizeOption.Size;
                            var rectDst = new Rectangle(0, 0, width: resizeOption.Size.Width, height: resizeOption.Size.Height);
                            var souceSize = new Dimention(width: sourceImg.Width, height: sourceImg.Height);
                            var rectSrc = sourceSize.GetCenterRectangleToFill(resizeOption.Size);

                            // Resize
                            using (var outputMs = new MemoryStream())
                            {
                                using (var canvas = new Bitmap(width: resizeOption.Size.Width, height: resizeOption.Size.Height))
                                using (var graphics = Graphics.FromImage(canvas))
                                {
                                    graphics.CompositingQuality = CompositingQuality.HighSpeed;
                                    graphics.InterpolationMode = InterpolationMode.HighQualityBilinear;
                                    graphics.CompositingMode = CompositingMode.SourceCopy;
                                    graphics.DrawImage(image: sourceImg, destRect: rectDst, srcRect: rectSrc, srcUnit: GraphicsUnit.Pixel);

                                    canvas.Save(outputMs, ImageFormat.Jpeg);
                                }

                                resizedBytes = outputMs.ToArray();
                            }
                            break;
                        default:
                            return Result<SizedImageData>.NewFailure($"{nameof(resizeOption.OperationType)}: '{resizeOption.OperationType}' is not supported.");
                    }
                }

                var result = new SizedImageData(imgType: resizeOption.ImgType, size: newSize, bytes: resizedBytes);

                return Result<SizedImageData>.NewSuccess(result);
            }
            catch (Exception ex)
            {
                return Result<SizedImageData>.NewFailure(ex.ToString());
            }
        }
    }
}
