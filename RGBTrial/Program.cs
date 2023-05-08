using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;

namespace RGBTrial
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string Location = "C:\\Users\\ruper\\OneDrive\\שולחן העבודה\\Ruper\\5688116.jpg";

            CreateAnImageFromAnotherImage();

            return;

            // Load the image file
            Image image = Image.FromFile(Location);

            // Get the pixel at position (0, 0)
            Color pixelColor = ((Bitmap)image).GetPixel(image.Width / 3, image.Height / 2 + 100);

            Console.WriteLine("Image Width: " + image.Width + " \nmage Height: " + image.Height);

            Console.WriteLine();

            // Get the RGB values
            int red = pixelColor.R;
            int green = pixelColor.G;
            int blue = pixelColor.B;

            // Display the RGB values
            Console.WriteLine("Red: {0}, Green: {1}, Blue: {2}", red, green, blue);

            FindPixle(Location);
        }
        static void FindPixle(string URL)
        {
            // Load image file
            Bitmap image = new Bitmap(URL);

            // Desired RGB color
            Color searchColor = Color.FromArgb(22, 57, 61); // Red color

            // Search for pixels with the desired color
            List<Point> matchingPixels = new List<Point>();
            for (int x = 0; x < image.Width; x++)
            {
                for (int y = 0; y < image.Height; y++)
                {
                    Color pixelColor = image.GetPixel(x, y);
                    if (pixelColor == searchColor)
                    {
                        matchingPixels.Add(new Point(x, y));
                    }
                }
            }

            // Print the coordinates of the matching pixels
            foreach (Point p in matchingPixels)
            {
                Console.WriteLine("Found matching pixel at ({0}, {1})", p.X, p.Y);
            }
            if (matchingPixels.Count == 0)
            {
                Console.WriteLine("there is non");
            }
        }
        static void CreateAnImageWithDrawings()
        {
            // Create a new bitmap with the desired size
            Bitmap image = new Bitmap(500, 500);

            // Draw something on the bitmap (optional)
            using (Graphics g = Graphics.FromImage(image))
            {
                g.Clear(Color.White);
                g.DrawLine(Pens.Red, 0, 0, image.Width, image.Height);

                g.DrawArc(Pens.Green, 0, 0, image.Width, image.Height, 45, 180);

                g.DrawArc(Pens.Red, 0, 0, image.Width, image.Height, 225, 180);
            }

            // Save the bitmap as a PNG file
            image.Save("C:\\Users\\ruper\\OneDrive\\שולחן העבודה\\Ruper\\NewImage.jpg", ImageFormat.Png);

            Console.WriteLine("your image has being created");
        }
        static void CreateAnImageFromAnotherImage()
        {
            Image sourceImage = Image.FromFile("C:\\Users\\ruper\\OneDrive\\שולחן העבודה\\Ruper\\wallpapersden.com_hacker-anonymous-evil_3840x2160.jpg");

            Bitmap bmp = new Bitmap(sourceImage.Width, sourceImage.Height);

            Graphics g = Graphics.FromImage(bmp);

            g.DrawImage(sourceImage, new Rectangle(0, 0, sourceImage.Width, sourceImage.Height));

            Rectangle sourceRect = new Rectangle(0, 0, 100, 100);
            Rectangle destRect = new Rectangle(0, 0, 100, 100);
            g.DrawImage(sourceImage, destRect, sourceRect, GraphicsUnit.Pixel);

            bmp.Save("C:\\Users\\ruper\\OneDrive\\שולחן העבודה\\Ruper\\new image.jpg", ImageFormat.Jpeg);

        }
    }
}
