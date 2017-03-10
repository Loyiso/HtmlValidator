using System;
using System.Windows.Forms;
using System.IO;
using System.Linq;
using System.Resources;
using System.Collections;
using log4net;

public class Program
{
    private static readonly ILog Logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

    [STAThread]
    public static void Main(string[] args)
    {
        Console.Clear();

        var folderBrowser = new FolderBrowserDialog();
        var folderName = string.Empty;

        if (folderBrowser.ShowDialog() == DialogResult.OK)
        {
            folderName = folderBrowser.SelectedPath;
        }
        else
        {
            Environment.Exit(0);
        }

        var dirs = Directory.GetFiles(folderName, "*.html");

        foreach (var file in dirs)
        {
            var fileName = new FileInfo(file).Name;
            string text = System.IO.File.ReadAllText(file);
            var doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(text);

            if (doc.ParseErrors.Count() > 0)
            {
                foreach (var error in doc.ParseErrors)
                {
                    Logger.Error(
                       String.Format("File Path: {0} , File Name: {1} , Error: {2}, line: {3}",
                       file, fileName, error.Reason, error.Line)
                    );
                }
            }

        }
    }
}