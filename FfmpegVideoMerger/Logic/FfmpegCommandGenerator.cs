using System.Text;

namespace FfmpegVideoMerger.Logic; 

public static class FfmpegCommandGenerator {

    public static string Generate(string videoPath, string audioPath, string outputPath) {
        var result = new StringBuilder("-y");

        result.Append(" -i \"");
        result.Append(videoPath);
        result.Append('"');

        result.Append(" -i \"");
        result.Append(audioPath);
        result.Append('"');

        result.Append(" -c copy -map 0:v -map 1:a");

        result.Append(" \"");
        result.Append(outputPath);
        result.Append('"');
        
        return result.ToString();
    }
}