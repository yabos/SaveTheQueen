using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using Process = System.Diagnostics.Process;
using ProcessStartInfo = System.Diagnostics.ProcessStartInfo;
using StringBuilder = System.Text.StringBuilder;
using Regex = System.Text.RegularExpressions.Regex;

public class SVNRevisionUtils
{
    public static int GetRevision(string path)
    {
        ProcessStartInfo psi = new ProcessStartInfo("svnversion", "-n -c \"" + path + "\"")
        {
            CreateNoWindow = true,
            RedirectStandardOutput = true,
            UseShellExecute = false
        };
        string output = string.Empty;
        using (Process process = Process.Start(psi))
        {
            StringBuilder standardOutput = new StringBuilder();
            // read chunk-wise while process is running.1
            while (!process.HasExited)
            {
                standardOutput.Append(process.StandardOutput.ReadToEnd());
            }
            // make sure not to miss out on any remaindings.
            standardOutput.Append(process.StandardOutput.ReadToEnd());
            output = standardOutput.ToString();
        }
        string[] strs = output.Split(':');
        int revision = 0;
        int temp = 0;
        for (int i = 0; i < strs.Length; i++)
        {
            if (!int.TryParse(Regex.Match(strs[i], @"\d+").Value, out temp))
                continue;
            if (temp > revision)
                revision = temp;
        }
        return revision;
    }

    public static int GetRevision(string[] paths)
    {
        if (paths == null)
            return 0;
        int revision = 0;
        int temp = 0;
        for (int i = 0; i < paths.Length; i++)
        {
            temp = GetRevision(paths[i]);
            if (temp > revision)
                revision = temp;
        }
        return revision;
    }

    public static int GetDependencyRevision(string[] paths)
    {
        string[] dependencies = AssetDatabase.GetDependencies(paths);
        int revision = 0;
        int temp = 0;
        for (int i = 0; i < dependencies.Length; i++)
        {
            if (System.IO.Path.GetExtension(dependencies[i]) == ".cs")
                continue;
            temp = GetRevision(dependencies[i]);
            if (temp > revision)
                revision = temp;
        }
        return revision;
    }

    public static int GetDependencyRevision(Object mainAsset, Object[] assets)
    {
        List<string> pathList = new List<string>();
        if (mainAsset != null)
        {
            string str = AssetDatabase.GetAssetPath(mainAsset);
            if (!string.IsNullOrEmpty(str))
            {
                pathList.Add(str);
            }
        }
        if (assets != null)
        {
            for (int i = 0; i < assets.Length; i++)
            {
                if (assets[i] == null)
                    continue;
                string str = AssetDatabase.GetAssetPath(assets[i]);
                if (!string.IsNullOrEmpty(str))
                {
                    pathList.Add(str);
                }
            }
        }
        string[] dependencies = AssetDatabase.GetDependencies(pathList.ToArray());
        int revision = 0;
        int temp = 0;
        for (int i = 0; i < dependencies.Length; i++)
        {
            if (System.IO.Path.GetExtension(dependencies[i]) == ".cs")
                continue;
            temp = GetRevision(dependencies[i]);
            if (temp > revision)
                revision = temp;
        }
        return revision;
    }

    public static string ExecuteCommand(string command)
    {
        ProcessStartInfo info = new ProcessStartInfo("cmd", "c/" + command)
        {
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        Process process = new Process
        {
            StartInfo = info,
        };
        process.Start();
        process.WaitForExit();
        string result = process.StandardOutput.ReadToEnd();
        string error = process.StandardError.ReadToEnd();

        return result;
    }
}
