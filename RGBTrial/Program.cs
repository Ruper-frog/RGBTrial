using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;

namespace RGBTrial
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Load the small images
            Bitmap[] images = new Bitmap[100];

            for (int i = 0; i < 100; i++)
                images[i] = new Bitmap("C:\\Users\\USER\\OneDrive\\שולחן העבודה\\Ruper\\wallpapersden.com_kali-linux-matrix_1980x1080.jpg");


            // Set the target width and height
            int targetWidth = 1920 * 10;
            int targetHeight = 1080 * 10;

            // Set the locations of the small images on the new image
            Point[] locations = new Point[100];

            int Index = 0;

            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    locations[Index++] = new Point(1920 * i, 1080 * j);
                }
            }

            // Combine the images into a larger image
            Bitmap combinedImage = CombineImages(images, targetWidth, targetHeight, locations);

            // Save the result to a file
            combinedImage.Save("C:\\Users\\USER\\OneDrive\\שולחן העבודה\\New folder (2)\\Combined Image.jpg", ImageFormat.Jpeg);

            return;
            /*
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

            FindPixle(Location);*/
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
            Image sourceImage = Image.FromFile("C:\\Users\\USER\\OneDrive\\שולחן העבודה\\New folder\\wallpapersden.com_kali-linux-matrix_1980x1080.jpg");

            Bitmap bmp = new Bitmap(sourceImage.Width, sourceImage.Height);

            Graphics g = Graphics.FromImage(bmp);

            g.DrawImage(sourceImage, new Rectangle(1000, 1000, sourceImage.Width, sourceImage.Height));

            Rectangle sourceRect = new Rectangle(0, 0, 100, 100);
            Rectangle destRect = new Rectangle(0, 0, 100, 100);
            g.DrawImage(sourceImage, destRect, sourceRect, GraphicsUnit.Pixel);

            bmp.Save("C:\\Users\\USER\\OneDrive\\שולחן העבודה\\New folder (2)\\new image.jpg", ImageFormat.Jpeg);

        }
        public static Bitmap CombineImages(Bitmap[] images, int targetWidth, int targetHeight, Point[] locations)
        {
            // Create a new Bitmap object to hold the combined image
            Bitmap result = new Bitmap(targetWidth, targetHeight);

            using (Graphics g = Graphics.FromImage(result))
            {
                // Clear the new image with a white background
                g.Clear(Color.White);

                // Draw each small image onto the new image
                for (int i = 0; i < images.Length; i++)
                {
                    int width = images[i].Width;
                    int height = images[i].Height;

                    // Shrink the image to fit into the target area
                    if (width > targetWidth || height > targetHeight)
                    {
                        float ratio = Math.Min((float)targetWidth / width, (float)targetHeight / height);
                        width = (int)(width * ratio);
                        height = (int)(height * ratio);
                    }

                    // Calculate the location of the image on the new image
                    int x = locations[i].X;
                    int y = locations[i].Y;

                    // Draw the image onto the new image
                    g.DrawImage(images[i], new Rectangle(x, y, width, height));
                }
            }
            return result;
        }
    }
}
