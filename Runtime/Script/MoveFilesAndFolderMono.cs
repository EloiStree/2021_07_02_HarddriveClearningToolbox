using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using UnityEngine;

public class MoveFilesAndFolderMono : MonoBehaviour
{

    public string m_workingDirectoryPath;
    [TextArea(0,5)]
    public string m_commandsToSend;

    void Start()
    {

        ProcessRunnerTest(m_commandsToSend.Split('\n').ToList(), m_workingDirectoryPath);
        //RunCommands();
        // UnityEngine.Debug.Log( ProcessRunner(m_commandsToSend.Split('\n').ToList(), m_workingDirectoryPath));
    }
    private void ProcessRunnerTest(List<string> cmds, string workingDirectory = "")
    {
        ProcessStartInfo processStartInfo = new ProcessStartInfo("cmd.exe");
        processStartInfo.WorkingDirectory = workingDirectory;
        processStartInfo.WindowStyle = ProcessWindowStyle.Normal;
        processStartInfo.Arguments = "/K " + string.Join("&", m_commandsToSend.Split('\n').ToList());

        Process process = Process.Start(processStartInfo);

    }

    private string ProcessRunner(List<string> cmds, string workingDirectory = "")
    {
        ProcessStartInfo processStartInfo = new ProcessStartInfo("cmd.exe");
        processStartInfo.WorkingDirectory = workingDirectory;
        processStartInfo.RedirectStandardInput = true;
        processStartInfo.RedirectStandardOutput = true;
        processStartInfo.UseShellExecute = false;
        processStartInfo.WindowStyle = ProcessWindowStyle.Normal;

         Process process = Process.Start(processStartInfo);

        if (process != null)
        {
            for (int i = 0; i < cmds.Count; i++)
            {
                process.StandardInput.WriteLine(cmds[i]);
            }
            //process.StandardInput.WriteLine("yourCommand.exe arg1 arg2");

            process.StandardInput.Close(); // line added to stop process from hanging on ReadToEnd()

            string outputString = process.StandardOutput.ReadToEnd();
            return outputString;
        }

        return string.Empty;
    }


    static void RunCommands(List<string> cmds, string workingDirectory = "")
    {
        var process = new Process();
        var psi = new ProcessStartInfo();
        psi.FileName = "cmd.exe";
        psi.RedirectStandardInput = true;
        psi.RedirectStandardOutput = true;
        psi.RedirectStandardError = true;
        psi.UseShellExecute = false;
        psi.WorkingDirectory = workingDirectory;
        process.StartInfo = psi;
        process.Start();
        process.OutputDataReceived += (sender, e) => { UnityEngine.Debug.Log(e.Data); };
        process.ErrorDataReceived += (sender, e) => { UnityEngine.Debug.Log(e.Data); };
        process.BeginOutputReadLine();
        process.BeginErrorReadLine();
        using (StreamWriter sw = process.StandardInput)
        {
            foreach (var cmd in cmds)
            {
                sw.WriteLine(cmd);
            }
        }
        process.WaitForExit();
    }
    static void RunCommand(string command, string workingDirectory)
    {
        Process process = new Process
        {
            StartInfo = new ProcessStartInfo("cmd.exe", $"/c {command}")
            {
                WorkingDirectory = workingDirectory,
                CreateNoWindow = true,
                UseShellExecute = false,
                RedirectStandardError = true,
                RedirectStandardOutput = true
            }
        };

        process.OutputDataReceived += (object sender, DataReceivedEventArgs e) => UnityEngine.Debug.Log("output :: " + e.Data);

        process.ErrorDataReceived += (object sender, DataReceivedEventArgs e) => UnityEngine.Debug.Log("error :: " + e.Data);

        process.Start();
        process.BeginOutputReadLine();
        process.BeginErrorReadLine();
        process.WaitForExit();

        UnityEngine.Debug.Log("ExitCode: "+ process.ExitCode);
        process.Close();
    }

  
}
