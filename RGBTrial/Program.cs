using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Security.Cryptography;

namespace RGBTrial
{
    internal class Program
    {
        static int ColorCellSize = 32,
            LittleImageSize = 100,
            FinalWidth = 19200, FinalHeight = 10800;

        static void Main(string[] args)
        {
            foreach (string s in CheckFolders(""))
                Console.WriteLine(s);

            //TileBigImage("BigImage\\New Image.jpg", "");

            //GenerateColorForTesting("Trash");

            //MinImage("BigImage\\R.jpg", 192, 108);

            //SortImages("Trash");

            return;
            /*

            // Load the small images
            Bitmap[] images = new Bitmap[100];

            for (int i = 0; i < 100; i++)
                images[i] = new Bitmap("C:\\Users\\ruper\\OneDrive\\שולחן העבודה\\Ruper\\somthing somthing 1997.jpg");


            // Set the target width and height
            int targetWidth = images[0].Width * 10 * 3;
            int targetHeight = images[0].Height * 10 * 3;

            // Set the locations of the small images on the new image
            Point[] locations = new Point[100];

            int Index = 0;

            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    locations[Index++] = new Point(images[0].Width * i, images[0].Height * j);
                }
            }

            // Combine the images into a larger image
            Bitmap combinedImage = CombineImages(images, targetWidth, targetHeight, locations);

            // Save the result to a file
            combinedImage.Save("C:\\Users\\ruper\\OneDrive\\שולחן העבודה\\Ruper\\Combined Image.jpg", ImageFormat.Jpeg);
            */

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
        /*
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
            Bitmap result = new Bitmap(20000, 20000);

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
        }*/
        static void TileBigImage(string BigImageURL, string ImagesURL)
        {
            int IndexSize = (int)Math.Ceiling((float)256 / ColorCellSize);

            int[,,] IndexOfFolder = new int[IndexSize, IndexSize, IndexSize];

            for (int i = 0; i < IndexSize; i++)
            {
                for (int j = 0; j < IndexSize; j++)
                {
                    for (int k = 0; k < IndexSize; k++)
                    {
                        IndexOfFolder[i, j, k] = 0;
                    }
                }
            }

            Bitmap BigImage = new Bitmap(Image.FromFile(BigImageURL));

            Bitmap FinalImage = new Bitmap(FinalWidth, FinalHeight);

            Image LittleImage;

            using (Graphics g = Graphics.FromImage(FinalImage))
            {
                for (int x = 0; x < BigImage.Width; x++)
                {
                    for (int y = 0; y < BigImage.Height; y++)
                    {
                        Color Pixel = BigImage.GetPixel(x, y);
                        Pixel = CellRGB(Pixel.R, Pixel.G, Pixel.B);
                        string FolderPath = Path.Combine(ImagesURL, ColorToString(Pixel));

                        string[] ImagesArr = Directory.GetFiles(FolderPath);

                        string LittleImageURL = ImagesArr[(IndexOfFolder[Pixel.R / ColorCellSize, Pixel.G / ColorCellSize, Pixel.B / ColorCellSize]++) % ImagesArr.Length];

                        LittleImage = Image.FromFile(LittleImageURL);

                        g.DrawImage(LittleImage, new Rectangle(x * LittleImageSize, y * LittleImageSize, LittleImageSize, LittleImageSize));

                        LittleImage.Dispose();
                    }
                }
                BigImage.Dispose();

                FinalImage.Save(BigImageURL.Replace(GetImageName(BigImageURL), "Final Image.jpg"), ImageFormat.Png);

                FinalImage.Dispose();
            }
        }
        static void MinImage(string ImageURL, int WidthSize, int HeightSize)
        {
            Image Photo = Image.FromFile(ImageURL);

            float ratio = Math.Max(((float)WidthSize + 2) / Photo.Width, ((float)HeightSize + 2) / Photo.Height);
            int width = (int)(Photo.Width * ratio);
            int height = (int)(Photo.Height * ratio);

            Bitmap image = new Bitmap(Photo, width, height);

            Bitmap result = new Bitmap(WidthSize, HeightSize);

            using (Graphics g = Graphics.FromImage(result))
            {
                g.Clear(Color.White);

                g.DrawImage(image, new Rectangle(-(width - WidthSize) / 2, -(height - HeightSize) / 2, width, height));
            }

            Photo.Dispose();

            result.Save(ImageURL.Replace(GetImageName(ImageURL), "New Image.jpg"), ImageFormat.Png);

            image.Dispose();
            File.Delete(ImageURL);

            result.Dispose();
        }
        static void ImageToCell(string Image)
        {
            Color Cell = ImageCell(Image);

            string CellName = ColorToString(Cell);

            if (!Directory.Exists(CellName))
            {
                // Create the new directory
                Directory.CreateDirectory(CellName);
            }

            string ImageName = GetImageName(Image);

            string Destebation = Path.Combine(CellName, ImageName);

            File.Move(Image, Destebation);
        }
        static string ColorToString(Color Color)
        {
            return $"{Color.R}_{Color.G}_{Color.B}";
        }
        static string GetImageName(string Image)
        {
            int BackSlashIdex = Image.LastIndexOf("\\");

            return Image.Substring(BackSlashIdex + 1, Image.Length - BackSlashIdex - 1);
        }
        static Color ImageCell(string ImageURL)
        {
            Bitmap image = new Bitmap(ImageURL);

            int R = 0, G = 0, B = 0;

            for (int x = 0; x < image.Width; x++)
            {
                for (int y = 0; y < image.Height; y++)
                {
                    Color Pixel = image.GetPixel(x, y);

                    R += Pixel.R;
                    G += Pixel.G;
                    B += Pixel.B;
                }
            }
            R /= (image.Height * image.Width);
            G /= (image.Height * image.Width);
            B /= (image.Height * image.Width);

            image.Dispose();

            return CellRGB(R, G, B);
        }
        static Color CellRGB(int R, int G, int B)
        {
            R /= ColorCellSize;
            G /= ColorCellSize;
            B /= ColorCellSize;

            R *= ColorCellSize;
            G *= ColorCellSize;
            B *= ColorCellSize;

            R += ColorCellSize / 2;
            G += ColorCellSize / 2;
            B += ColorCellSize / 2;

            Color RGB = Color.FromArgb(R, G, B);

            return RGB;
        }
        static Bitmap NewCanvas(int Width, int Height, Color color)
        {
            Bitmap result = new Bitmap(Width, Height);

            using (Graphics g = Graphics.FromImage(result))
            {
                // Clear the new image with a white background
                g.Clear(color);
            }
            return result;
        }
        //---------------------------------------------------------------
        static List<string> CheckFolders(string FoldersLocation)
        {
            List<string> DoesntExist = new List<string>();

            for (int R = 0; R < 256; R += ColorCellSize)
            {
                for (int G = 0; G < 256; G += ColorCellSize)
                {
                    for (int B = 0; B < 256; B += ColorCellSize)
                    {
                        string FolderPath = Path.Combine(FoldersLocation, ColorToString(CellRGB(R, G, B)));

                        if ((!Directory.Exists(FolderPath)) || (Directory.GetFiles(FolderPath).Length == 0))
                            DoesntExist.Add(GetImageName(FolderPath));
                    }
                }
            }
            return DoesntExist;
        }
        static void SortImages(string ImagesLocation)
        {
            List<string> files = new List<string>(Directory.GetFiles(ImagesLocation));

            foreach (string File in files)
            {
                MinImage(File, LittleImageSize, LittleImageSize);

                ImageToCell(File);
            }
        }
        static void GenerateColorForTesting(string URL)
        {
            Random random = new Random();

            for (int R = 0; R < 256; R += ColorCellSize)
            {
                for (int G = 0; G < 256; G += ColorCellSize)
                {
                    for (int B = 0; B < 256; B += ColorCellSize)
                    {
                        for (int i = 0; i < 90; i++)
                        {
                            Bitmap Canvas = NewCanvas(LittleImageSize, LittleImageSize, CellRGB(R, G, B));

                            string Location = Path.Combine(URL, random.Next(1, 1000000000).ToString());

                            Canvas.Save(Location + ".jpg", ImageFormat.Png);
                        }
                    }
                }
            }
        }
    }
}