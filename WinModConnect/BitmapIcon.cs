using System;
using System.Collections;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;

class BitmapIcon
{
    ArrayList files;

    public BitmapIcon()
    {
        files = new ArrayList();
    }

    public string CreateFile(Bitmap image)
    {
        string tempPath = System.IO.Path.GetTempFileName();
        image.Save(tempPath);
        files.Add(tempPath);
        return tempPath;
    }

    public bool CleanFiles()
    {
        foreach (string file in files)
        {
            try
            {
                File.Delete(file);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        files.Clear();
        files = null;
        return true;
    }
}

