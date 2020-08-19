using DatingCode.Basics;
using Xunit;

namespace DatingTests.Code.Basics
{
    public class DimentionTests
    {
        [Fact]
        public void CanFit_SmallerWideBox()
        {
            var dOriginal = new Dimention(width: 100, height: 50);
            var dBox = new Dimention(width: 50, height: 50);

            var scaled = dOriginal.GetSizeToFitInto(dBox);

            Assert.True(scaled.Width == 50 && scaled.Height == 25);
        }

        [Fact]
        public void CanFit_SmallerHighBox()
        {
            var dOriginal = new Dimention(width: 100, height: 200);
            var dBox = new Dimention(width: 100, height: 100);

            var scaled = dOriginal.GetSizeToFitInto(dBox);

            Assert.True(scaled.Width == 50 && scaled.Height == 100);
        }

        [Fact]
        public void CanFit_BiggerWideBox()
        {
            var dOriginal = new Dimention(width: 100, height: 100);
            var dBox = new Dimention(width: 200, height: 300);

            var scaled = dOriginal.GetSizeToFitInto(dBox);

            Assert.True(scaled.Width == 200 && scaled.Height == 200);
        }

        [Fact]
        public void CanFit_BiggerHighBox()
        {
            var dOriginal = new Dimention(width: 100, height: 100);
            var dBox = new Dimention(width: 200, height: 150);

            var scaled = dOriginal.GetSizeToFitInto(dBox);

            Assert.True(scaled.Width == 150 && scaled.Height == 150);
        }

        [Fact]
        public void CanFill_SmallerWideBox()
        {
            var dFrom = new Dimention(width: 300, height: 200);
            var dTo = new Dimention(width: 50, height: 50);

            var result = dFrom.GetCenterRectangleToFill(dTo);

            Assert.True(result.X == 50 && result.Y == 0 && result.Width == 200 && result.Height == 200);
        }

        [Fact]
        public void CanFill_SmallerHighBox()
        {
            var dFrom = new Dimention(width: 300, height: 400);
            var dTo = new Dimention(width: 50, height: 50);

            var result = dFrom.GetCenterRectangleToFill(dTo);

            Assert.True(result.X == 0 && result.Y == 50 && result.Width == 300 && result.Height == 300);
        }

        [Fact]
        public void CanFill_BiggerWideBox()
        {
            var dFrom = new Dimention(width: 300, height: 200);
            var dTo = new Dimention(width: 500, height: 500);

            var result = dFrom.GetCenterRectangleToFill(dTo);

            Assert.True(result.X == 50 && result.Y == 0 && result.Width == 200 && result.Height == 200);
        }

        [Fact]
        public void CanFill_BiggerHighBox()
        {
            var dFrom = new Dimention(width: 300, height: 400);
            var dTo = new Dimention(width: 500, height: 500);

            var result = dFrom.GetCenterRectangleToFill(dTo);

            Assert.True(result.X == 0 && result.Y == 50 && result.Width == 300 && result.Height == 300);
        }
    }
}
