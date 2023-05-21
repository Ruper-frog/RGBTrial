using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace RGBTrial
{
    internal class Program
    {
        static int ColorCellSize = 32,
            LittleImageSize = 150,
            FinalWidth = 28800, FinalHeight = 16200;
        static int PhotoName = 0;

        static int PercentageMessages = -1;

        static int Process = 0, TOTAL, STEPS;

        static void Main(string[] args)
        {
            //foreach (string s in CheckFolders(""))
            //    Console.WriteLine(s);

            //MinImage("BigImage\\R.jpg", FinalWidth / LittleImageSize, FinalHeight / LittleImageSize);

            TileBigImage("BigImage\\New Image.jpg", "");

            //GenerateColorForTesting("Trash");


            //SortImages("Trash");
        }
        static void PrintPercentage()
        {
            int Pre = (int)Math.Floor((float) Process * STEPS / TOTAL);
            Process++;

            int Cur = (int)Math.Floor((float) Process * STEPS / TOTAL);

            if (Cur > Pre)
            {
                Console.SetCursorPosition(0, PercentageMessages);
                Console.WriteLine(Cur);
            }
        }
        static void InitPercentageMessages(string Message, int Total)
        {
            TOTAL = Total;
            STEPS = 100;

            Console.WriteLine(Message);
            PercentageMessages += 2;

            Process = 0;
        }
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

            InitPercentageMessages("Tiling the big image:", BigImage.Width * BigImage.Height);

            using (Graphics g = Graphics.FromImage(FinalImage))
            {
                for (int x = 0; x < BigImage.Width; x++)
                {
                    for (int y = 0; y < BigImage.Height; y++)
                    {
                        PrintPercentage();

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

                FinalImage.Save(BigImageURL.Replace(GetImageName(BigImageURL), "Final Image.jpg"), ImageFormat.Jpeg);

                FinalImage.Dispose();
            }
        }
        static string MinImage(string ImageURL, int WidthSize, int HeightSize)
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

            string NewFileName = ImageURL.Replace(GetImageName(ImageURL), $"{RandomFileName()}.jpg");

            result.Save(NewFileName, ImageFormat.Jpeg);

            image.Dispose();
            File.Delete(ImageURL);

            result.Dispose();

            return NewFileName;
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
        static string RandomFileName()
        {
            Random random = new Random();

            string FileName = "";

            for (int i = 0; i < 10; i++)
            {
                switch (random.Next(1, 4))
                {
                    case 1:
                        FileName += (char)random.Next(48, 58);
                        break;
                    case 2:
                        FileName += (char)random.Next(65, 91);
                        break;
                    case 3:
                        FileName += (char)random.Next(97, 123);
                        break;
                }
            }
            return FileName;
        }
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
                ImageToCell(MinImage(File, LittleImageSize, LittleImageSize));
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
                        for (int i = 0; i < 10; i++)
                        {
                            Bitmap Canvas = NewCanvas(LittleImageSize, LittleImageSize, CellRGB(R, G, B));

                            string Location = Path.Combine(URL, RandomFileName());

                            Canvas.Save(Location + ".jpg", ImageFormat.Png);
                        }
                    }
                }
            }
        }
    }
}