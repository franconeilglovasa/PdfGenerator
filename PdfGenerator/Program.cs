using iText.IO.Image;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Text;

namespace PdfGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            for (var ctr=301; ctr<1000; ctr++)
            {
                var textstring = GenerateRandomArticle();
                var filename = $@"{ctr.ToString()}.pdf";
                SaveTextAndImageToPdf(textstring, filename);
            }
            Console.WriteLine("DOne");
        }

        public static string GenerateRandomArticle()
        {
            Random rand = new Random();

            string[] words = new string[] {
                "Lorem", "ipsum", "dolor", "sit", "amet", "consectetur",
                "adipiscing", "elit", "sed", "do", "eiusmod", "tempor",
                "incididunt", "ut", "labore", "et", "dolore", "magna",
                "aliqua", "Ut", "enim", "ad", "minim", "veniam", "quis",
                "nostrud", "exercitation", "ullamco", "laboris", "nisi",
                "ut", "aliquip", "ex", "ea", "commodo", "consequat",
                "Duis", "aute", "irure", "dolor", "in", "reprehenderit",
                "in", "voluptate", "velit", "esse", "cillum", "dolore",
                "eu", "fugiat", "nulla", "pariatur", "Excepteur", "sint",
                "occaecat", "cupidatat", "non", "proident", "sunt",
                "in", "culpa", "qui", "officia", "deserunt", "mollit",
                "anim", "id", "est", "laborum"
            };

            int numWords = 500;
            StringBuilder sb = new StringBuilder();

            // Generate random article with at least 500 words
            int sentenceCount = 0;
            for (int i = 1; i <= numWords; i++)
            {
                int index = rand.Next(words.Length);
                string word = words[index];

                // Capitalize first word of sentence
                if (i == 1 || i % 150 == 1)
                {
                    word = char.ToUpper(word[0]) + word.Substring(1);
                }

                sb.Append(word);

                // Add space after word unless it's the last word
                if (i != numWords)
                {
                    sb.Append(' ');
                }
                // Add period at end of sentence
                else
                {
                    sb.Append('.');
                }

                // Add newline character after every 10 words
                if (i % 150 == 0 && i != numWords)
                {
                    sb.Append("\n\n");
                }

                // Add extra newline character after every 10 sentences
                if (word.EndsWith(".") && ++sentenceCount % 150 == 0)
                {
                    sb.Append("\n\n");
                }
            }

            return sb.ToString();
        }

        public static void SaveTextAndImageToPdf(string text, string fileName)
        {
            // Generate a random image
            Random random = new Random();
            int imageWidth = 200;
            int imageHeight = 200;
            Color randomColor = Color.FromArgb(random.Next(256), random.Next(256), random.Next(256));
            using System.Drawing.Image image = new Bitmap(imageWidth, imageHeight, PixelFormat.Format32bppArgb);
            using Graphics graphics = Graphics.FromImage(image);
            graphics.SmoothingMode = SmoothingMode.AntiAlias;
            graphics.Clear(randomColor);
            Pen pen = new Pen(Color.Black, 1);
            for (int i = 0; i < 10; i++)
            {
                // Generate random points
                Point p1 = new Point(random.Next(imageWidth), random.Next(imageHeight));
                Point p2 = new Point(random.Next(imageWidth), random.Next(imageHeight));

                // Draw random lines
                graphics.DrawLine(pen, p1, p2);

                // Draw random circles
                Rectangle circleBounds = new Rectangle(random.Next(imageWidth), random.Next(imageHeight), random.Next(imageWidth / 2), random.Next(imageHeight / 2));
                graphics.DrawEllipse(pen, circleBounds);

                // Draw random dots
                Point dotPoint = new Point(random.Next(imageWidth), random.Next(imageHeight));
                graphics.FillEllipse(pen.Brush, dotPoint.X, dotPoint.Y, 1, 1);
            }

            // Save the image to a memory stream in JPEG format
            MemoryStream ms = new MemoryStream();
            image.Save(ms, ImageFormat.Jpeg);

            // Create an Image
            byte[] imageData = ms.ToArray();
            ImageData imageDataObject = ImageDataFactory.Create(imageData);

            // Create a PDF document
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(fileName));

            // Add the image to the PDF document
            iText.Layout.Element.Image pdfImage = new iText.Layout.Element.Image(imageDataObject);
            pdfImage.SetHorizontalAlignment(iText.Layout.Properties.HorizontalAlignment.CENTER);
            pdfImage.SetMargins(0, 0, 0, 20);
            Document doc = new Document(pdfDoc);
            doc.Add(pdfImage);

            // Add the text to the PDF document
            string[] paragraphs = text.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
            foreach (string paragraph in paragraphs)
            {
                doc.Add(new Paragraph(paragraph).SetMarginBottom(20));
            }

            // Close the document
            doc.Close();
        }
    }
}
