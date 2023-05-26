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
        static int MaxPixelOOM = 18900 * 28350,
            LittleImageSize = 150;
        static void DecideBigImageSize(int Rwidth, int Rheight)
        {
            // TODO: Make sure thats a good way to calculate that shit

            int a = (int)Math.Sqrt(MaxPixelOOM / (Rwidth * Rheight));

            int TempWidth = a * Rwidth;
            int TempHeight = a * Rheight;

            OGWidth = TempWidth / LittleImageSize;
            OGHeight = TempHeight / LittleImageSize;

            FinalWidth = OGWidth * LittleImageSize;
            FinalHeight = OGHeight * LittleImageSize;
        }

        static int RandomStringLength = 10;

        static int ColorCellSize = 32,
            FinalWidth, FinalHeight,
            OGWidth, OGHeight;

        static int PercentageMessages = -1;

        static int Process = 0, TOTAL, STEPS;

        static int SpeedImageCell = 10;

        static string RandomString()
        {
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();

            string str = "";

            for (int i = 0; i < RandomStringLength; i++)
            {
                int RandomNumber = GenerateRandomNumber(0, 61);

                if (RandomNumber < 10)
                    str += (char)('0' + RandomNumber);

                else if (RandomNumber < 36)
                    str += (char)('a' + RandomNumber - 10);

                else
                    str += (char)('A' + RandomNumber - 36);
            }
            return str;

            int GenerateRandomNumber(int minValue, int maxValue)
            {
                byte[] randomNumber = new byte[4];
                rng.GetBytes(randomNumber);

                int generatedValue = BitConverter.ToInt32(randomNumber, 0);
                return Math.Abs(generatedValue % (maxValue - minValue + 1)) + minValue;
            }
        }

        static void FindMe(double Alpha)
        {
            List<string> ImagesReady = new List<string>(Directory.GetFiles("BigImage"));

            foreach (string Image in ImagesReady)
                BigImageHandler(GetFileNameWithExtension(Image), Alpha);
        }

        static void BigImageHandler(string ImageNameWithType, double Alpha)
        {
            string BigImageURL = Path.Combine("BigImage", ImageNameWithType);

            Image BigImage = Image.FromFile(BigImageURL);

            DecideBigImageSize(BigImage.Width, BigImage.Height);

            BigImage.Dispose();

            string DirectoryPath = Path.Combine("BigImage", GetFileName(ImageNameWithType));
            Directory.CreateDirectory(DirectoryPath);

            CopyAndSaveImage(BigImageURL, Path.Combine(DirectoryPath, ImageNameWithType));

            string PixelizedImage = Path.Combine(DirectoryPath, "Pixelized Image.jpg");

            File.Move(MinImage(BigImageURL, OGWidth, OGHeight), PixelizedImage);

            TileBigImage(PixelizedImage, "SortedImages", Alpha);
        }

        static void Main(string[] args)
        {
            Console.CursorVisible = false;

            //DeleteCopies("C:\\Users\\ruper\\OneDrive\\שולחן העבודה\\New folder");
            SortImages("Trash");

            //foreach (string s in CheckFolders("SortedImages"))
            //Console.WriteLine(s);

            double Alpha = 0.4;

            FindMe(Alpha);
        }

        static void CopyAndSaveImage(string ImageURL, string NewLocation)
        {
            Image OriginalImage = Image.FromFile(ImageURL);

            OriginalImage.Save(NewLocation);

            OriginalImage.Dispose();
        }

        static void PrintPercentage()
        {
            int Pre = (int)Math.Floor((float)Process * STEPS / TOTAL);
            Process++;

            int Cur = (int)Math.Floor((float)Process * STEPS / TOTAL);

            if (Cur > Pre)
            {
                Console.SetCursorPosition(0, PercentageMessages);
                Console.WriteLine(Cur + "%");
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

        static void TileBigImage(string BigImageURL, string ImagesURL, double Alpha)
        {
            bool UsedColorCanvas = false;

            List<int> Xaxis = new List<int>();
            List<int> Yaxis = new List<int>();

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

                        Color OGPixel = BigImage.GetPixel(x, y);
                        Color CellPixel = CellRGB(OGPixel.R, OGPixel.G, OGPixel.B);
                        string FolderPath = Path.Combine(ImagesURL, ColorToString(CellPixel));

                        if (!Directory.Exists(FolderPath))
                        {
                            LittleImage = NewCanvas(LittleImageSize, LittleImageSize, OGPixel);

                            UsedColorCanvas = true;

                            Xaxis.Add(x);
                            Yaxis.Add(y);
                        }
                        else
                        {
                            string[] ImagesArr = Directory.GetFiles(FolderPath);
                            string LittleImageURL = ImagesArr[(IndexOfFolder[CellPixel.R / ColorCellSize, CellPixel.G / ColorCellSize, CellPixel.B / ColorCellSize]++) % ImagesArr.Length];

                            LittleImage = TweakImage(LittleImageURL, OGPixel, Alpha);
                        }
                        g.DrawImage(LittleImage, new Rectangle(x * LittleImageSize, y * LittleImageSize, LittleImageSize, LittleImageSize));

                        LittleImage.Dispose();
                    }
                }
                BigImage.Dispose();

                string BigImageFinalLocation = BigImageURL.Replace(GetFileNameWithExtension(BigImageURL), "Final Image.jpg");

                FinalImage.Save(BigImageFinalLocation, ImageFormat.Jpeg);

                FinalImage.Dispose();

                Console.WriteLine(UsedColorCanvas ? "We Suck" : "All Good");

                for (int i = 0; i < Xaxis.Count; i++)
                    Console.WriteLine(Xaxis[i] + ", " + Yaxis[i]);

                System.Diagnostics.Process.Start(BigImageFinalLocation);

                //Console.WriteLine("Press Any Key To Leave . . .");

                //ConsoleKeyInfo keyInfo = Console.ReadKey();
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

            string NewFileName = Path.Combine("MinimizedImages", RandomString());

            result.Save(NewFileName, ImageFormat.Jpeg);

            image.Dispose();
            File.Delete(ImageURL);

            result.Dispose();

            return NewFileName;
        }

        static void ImageToCell(string Image)
        {
            Color Cell = ImageCell(Image);

            string CellName = Path.Combine("SortedImages", ColorToString(Cell));

            if (!Directory.Exists(CellName))
            {
                // Create the new directory
                Directory.CreateDirectory(CellName);
            }

            string Destination = Path.Combine(CellName, RandomString() + ".jpg");

            File.Move(Image, Destination);
        }

        static string ColorToString(Color Color)
        {
            return $"{Color.R}_{Color.G}_{Color.B}";
        }

        static string GetFileNameWithExtension(string File)
        {
            int BackSlashIdex = File.LastIndexOf("\\");

            return File.Substring(BackSlashIdex + 1, File.Length - BackSlashIdex - 1);
        }

        static string GetFileName(string File)
        {
            return GetFileNameWithExtension(File.Substring(0, File.IndexOf('.')));
        }

        static Color ImageCell(string ImageURL)
        {
            Bitmap image = new Bitmap(ImageURL);

            int R = 0, G = 0, B = 0;

            for (int x = 0; x < image.Width; x += SpeedImageCell)
            {
                for (int y = 0; y < image.Height; y += SpeedImageCell)
                {
                    Color Pixel = image.GetPixel(x, y);

                    R += Pixel.R;
                    G += Pixel.G;
                    B += Pixel.B;
                }
            }
            R /= ((image.Height / SpeedImageCell) * (image.Width / SpeedImageCell));
            G /= ((image.Height / SpeedImageCell) * (image.Width / SpeedImageCell));
            B /= ((image.Height / SpeedImageCell) * (image.Width / SpeedImageCell));

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

        static Bitmap TweakImage(string ImageURL, Color Des, double Alpha)
        {
            Bitmap image = new Bitmap(Bitmap.FromFile(ImageURL));

            for (int x = 0; x < image.Width; x++)
            {
                for (int y = 0; y < image.Height; y++)
                {
                    Color Cur = image.GetPixel(x, y);
                    Color NewColor = TweakColor(Cur, Des, Alpha);

                    image.SetPixel(x, y, NewColor);
                }
            }
            return image;
        }

        static Color TweakColor(Color Cur, Color Des, double Alpha)
        {
            int r = TweakInt(Cur.R, Des.R, Alpha);
            int g = TweakInt(Cur.G, Des.G, Alpha);
            int b = TweakInt(Cur.B, Des.B, Alpha);

            return Color.FromArgb(r, g, b);
        }

        static int TweakInt(int Cur, int Des, double Alpha)
        {
            return (int)(Math.Round(Cur * (1 - Alpha) + Des * Alpha));
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
            InitPercentageMessages("Checking for empty folders:", (int)Math.Pow(256 / ColorCellSize, 3));

            List<string> DoesntExist = new List<string>();

            for (int R = 0; R < 256; R += ColorCellSize)
            {
                for (int G = 0; G < 256; G += ColorCellSize)
                {
                    for (int B = 0; B < 256; B += ColorCellSize)
                    {
                        PrintPercentage();

                        string FolderPath = Path.Combine(FoldersLocation, ColorToString(CellRGB(R, G, B)));

                        if ((!Directory.Exists(FolderPath)) || (Directory.GetFiles(FolderPath).Length == 0))
                        {
                            string r = CellRGB(R, G, B).R.ToString("X");
                            string g = CellRGB(R, G, B).G.ToString("X");
                            string b = CellRGB(R, G, B).B.ToString("X");

                            DoesntExist.Add($"#{r}{g}{b}");

                            //DoesntExist.Add(GetFileNameWithExtension(FolderPath));
                        }
                    }
                }
            }
            return DoesntExist;
        }

        static void SortImages(string ImagesLocation)
        {
            List<string> files = new List<string>(Directory.GetFiles(ImagesLocation));

            if (files.Count == 0)
                return;

            InitPercentageMessages("Minimizing and sorting the images:", files.Count);

            foreach (string File in files)
            {
                PrintPercentage();

                ImageToCell(MinImage(File, LittleImageSize, LittleImageSize));
            }
        }

        static void DeleteCopies(string FolderURL)
        {
            List<string> Files = new List<string>(Directory.GetFiles(FolderURL));

            foreach (string file in Files)
            {
                bool Found = false;


                string newFileName = GetFileNameWithExtension(file).IndexOf("_") == -1 ? "" : file.Replace(GetFileNameWithExtension(file), GetFileNameWithExtension(file).Substring(file.IndexOf("_") + 1));

                if (newFileName != "" && newFileName[newFileName.Length - 1] != '\\' && !File.Exists(newFileName))
                {
                    File.Move(file, newFileName);
                }

                foreach (char chr in file)
                {
                    if (chr == '(')
                    { Found = true; break; }
                }
                if (Found)
                    File.Delete(file);
            }
        }

        static void GenerateColorForTesting(string URL)
        {
            InitPercentageMessages("Generating Colorful Images for testing:", (int)Math.Pow(256 / ColorCellSize, 3) * 10);

            Random random = new Random();

            for (int R = 0; R < 256; R += ColorCellSize)
            {
                for (int G = 0; G < 256; G += ColorCellSize)
                {
                    for (int B = 0; B < 256; B += ColorCellSize)
                    {
                        for (int i = 0; i < 10; i++)
                        {
                            PrintPercentage();

                            Bitmap Canvas = NewCanvas(LittleImageSize, LittleImageSize, CellRGB(R, G, B));

                            string Location = Path.Combine(URL, RandomString());

                            Canvas.Save(Location + ".jpg", ImageFormat.Png);
                        }
                    }
                }
            }
        }
    }
}