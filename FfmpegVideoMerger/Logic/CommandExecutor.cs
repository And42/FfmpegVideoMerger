using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FfmpegVideoMerger.Logic; 

public static class CommandExecutor {

    public delegate void GotDataHandler(string data);

    public static async Task Execute(string fileName, string arguments, GotDataHandler onGotData, CancellationToken cancellationToken = default) {
        var process = new Process {
            StartInfo = new ProcessStartInfo(fileName, arguments) {
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true,
                UseShellExecute = false,
                StandardOutputEncoding = Encoding.UTF8,
                StandardErrorEncoding = Encoding.UTF8
            }
        };

        void DataHandler(object _, DataReceivedEventArgs args) {
            string? output = args.Data;
            if (output.IsNotNullOrEmpty()) {
                onGotData(output!);
            }
        }

        process.OutputDataReceived += DataHandler;
        process.ErrorDataReceived += DataHandler;

        process.Start();

        process.BeginOutputReadLine();
        process.BeginErrorReadLine();

        try {
            await process.WaitForExitAsync(cancellationToken);
        } finally {
            process.Close();
        }
    }
}