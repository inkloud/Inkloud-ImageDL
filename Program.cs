using CommandLine;
using Microsoft.VisualBasic.FileIO;
using System;
using System.IO;
using System.Net;


namespace ConsoleApp1
{
    class Program
    {
        public class Options
        {
            [Option('p', "path", Required = true, HelpText = "Set path to download images.")]
            public string Path { get; set; }

            [Option('c', "cats", Required = true, HelpText = "Set categories [1,14,..].")]
            public string Cats { get; set; }

            [Option('u', "user", Required = true, HelpText = "Set username.")]
            public string User { get; set; }

            [Option('k', "path", Required = true, HelpText = "Set password.")]
            public string Password { get; set; }
        }
        public static string DownloadCSV(string url)
        {
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();

            StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream());
            string results = streamReader.ReadToEnd();
            streamReader.Close();

            return results;
        }
        public static void DownloadFile(string url, string filePath, string newName)
        {
            string file = filePath + newName;
            WebClient cln = new WebClient();
            cln.DownloadFile(url, file);
        }
        public static void Main(string[] args)
        {
            // Life365.exe -p "path" -c "[categories]" -u "USER" -k "pass"
            // Life365.exe -p "C:\temp\xxxooo" -c "14,1" -u "username" -k "password"

            string filePath = "";
            string categories = "";
            string user = "";
            string password = "";
            Parser.Default.ParseArguments<Options>(args)
                               .WithParsed<Options>(o =>
                               {
                                   Console.WriteLine($"Wait ... downloading ...");
                                   filePath = o.Path + @"\";
                                   categories = o.Cats;
                                   user = o.User;
                                   password = o.Password;
                               })
                               .WithNotParsed(o =>
                               {
                                   Environment.Exit(0);
                               });


            string[] catList = categories.Split(",");

            foreach (string idCat in catList)
            {
                string url = "https://www.life365.eu/api/utils/csvdata/prodlist?l=" + user + "&p=" + password + "&idcat=" + idCat;

                string myStream = DownloadCSV(url);
                File.WriteAllTextAsync("prodlistC-"+idCat + ".csv", myStream);
                Console.WriteLine($"CSV file written.");

                StringReader sr = new StringReader(myStream);
                using var parser = new TextFieldParser(sr);
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(";");
                parser.HasFieldsEnclosedInQuotes = true;
                while (!parser.EndOfData)
                {
                    Console.WriteLine("Line:");
                    var fields = parser.ReadFields();
                    Console.WriteLine(fields[2].ToString() + " " + fields[14].ToString());

                    string url_photo = fields[14].ToString();

                    if (url_photo.Contains("."))
                    {
                        // if file not exits
                        string product_code = fields[2].ToString();
                        string imgExt = Path.GetExtension(url_photo);

                        if (!File.Exists(filePath + product_code + imgExt))
                            DownloadFile(url_photo, filePath, product_code + imgExt);
                    }
                }
            }
        }

    }
}
