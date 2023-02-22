using System;
using System.Diagnostics;

public class Principal
{
    static void Main()
    {
        char opt;
        string video, duration, text = "";
        int total, hr, min, sec, frameQuantity;

        Console.WriteLine("Write the video file name");
        video = Console.ReadLine();

        Console.WriteLine("Write the length in seconds");
        total = Convert.ToInt32(Console.ReadLine());

        //Adjust the video length and calculates total frames at 30 fps
        hr = (int)total / 3600;
        min = (int)(total % 3600) / 60;
        sec = (int)(total % 3600) % 60;
        duration = hr + ":" + min + ":" + sec;
        frameQuantity = (int)total * 30;

        //Generates text with all frames to render
        for (int i = 1; i <= frameQuantity; i++)
        {
            text = text + ("frame" + (i.ToString("D5")) + ".jpeg ");  
        }

        //Extracts the frames
        Process frames = Process.Start("ffmpeg", "-ss 00:00:00 -t " + duration + " -i " + video + " -qscale:v 2 -r 30.0 frame%5d.jpeg");
        frames.WaitForExit();
        //Renders the video with all the frames extracted
        Process render = Process.Start("jp2a", text + "--colors -i");
        render.WaitForExit();

        Console.WriteLine("Do you want to remove all the files generated? [Y/N]");
        opt = Convert.ToChar(Console.ReadLine());

        if (opt == 'Y')
        {
            //Removes all the frames generated
            Process rm = Process.Start("rm", text);
            rm.WaitForExit();
            Console.WriteLine("Frames eliminados");
        }
    }
}