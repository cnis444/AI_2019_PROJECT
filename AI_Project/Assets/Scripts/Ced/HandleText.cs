using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Diagnostics;
using System;

public static class HandleText
{

    public static void WriteData(string path, MapData mapData)
    {
        StreamWriter writer = new StreamWriter(path, true);
        string tmp = "";
        tmp += MatrixToString(mapData.heighMap);
        writer.Write(tmp);
        writer.Close();
    }

    public static string  MatrixToString(float[,] heightMap)
    {
        string toret = "";

        int len = heightMap.GetLength(0);
        for (int i = 0; i < len; i++)
        {
            for (int j = 0; j < len; j++)
            {
                toret += " " + heightMap[i,j] + " ";
            }
            toret += "\n";
        }

        return toret;
    }

    public static string ReadData(string path)
    {
        StreamReader reader = new StreamReader(path);
        string toret = reader.ReadToEnd();
        reader.Close();
        return toret;
        
    }

    public static void RunProcess(string arg)
    {
        try
        {
            Process myProcess = new Process();
            myProcess.StartInfo.FileName = "python.exe";
            myProcess.StartInfo.Arguments = "Assets/Ressources/prog.py " + arg;
            myProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            myProcess.EnableRaisingEvents = true;
            myProcess.Start();
            myProcess.WaitForExit();
            int ExitCode = myProcess.ExitCode;
        }
        catch (Exception e)
        {
            UnityEngine.Debug.Log(e);
        }
    }

}
    
       

