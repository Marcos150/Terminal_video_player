using System;
using System.Diagnostics;

public class Principal
{
    static void Main()
    {
        char opt;
        string video, duracion, text = "";
        int total, hr, min, sec, cantidadFrames;

        Console.WriteLine("Escribe el nombre del vídeo con extensión");
        video = Console.ReadLine();

        /*Process getduracion = new Process();
        getduracion.StartInfo.FileName = "ffprobe";
        getduracion.StartInfo.Argument = "-i " + video + " -show_entries format=duration -v quiet -of csv=\"p=0\"";
        getduracion.StartInfo.UseShellExecute = false;
        getduracion.StartInfo.RedirectStandardOutput = true;
        getduracion.Start();*/

        Console.WriteLine("Escribe la duración en segundos");
        total = Convert.ToInt32(Console.ReadLine());

        //Ajusta la duración del video y calcula frames totales a 30 fps
        hr = (int)total / 3600;
        min = (int)(total % 3600) / 60;
        sec = (int)(total % 3600) % 60;
        duracion = hr + ":" + min + ":" + sec;
        cantidadFrames = (int)total * 30;

        //Genera texto con todos los frames a renderizar
        for (int i = 1; i <= cantidadFrames; i++)
        {
            text = text + ("frame" + (i.ToString("D5")) + ".jpeg ");  
        }

        //Extracción de frames
        Process frames = Process.Start("ffmpeg", "-ss 00:00:00 -t " + duracion + " -i " + video + " -qscale:v 2 -r 30.0 frame%5d.jpeg");
        frames.WaitForExit();
        //Renderiza video con los frames
        Process render = Process.Start("jp2a", text + "--colors -i");
        render.WaitForExit();

        Console.WriteLine("¿Quieres borrar todos los frames generados [S/N]?");
        opt = Convert.ToChar(Console.ReadLine());

        if (opt == 'S')
        {
            Process rm = Process.Start("rm", text); //borra todos los frames generados
            rm.WaitForExit();
            Console.WriteLine("Frames eliminados");
        }
    }
}