using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FfmpegVideoMerger.Logic; 

public static class FfmpegCommandGenerator {

    public static string Generate(string videoPath, IEnumerable<string> audioPaths, string outputPath) {
        var audioPathsList = audioPaths.ToList();

        var result = new StringBuilder("-y");

        result.Append(" -i \"");
        result.Append(videoPath);
        result.Append('"');

        foreach (var audio in audioPathsList) {
            result.Append(" -i \"");
            result.Append(audio);
            result.Append('"');
        }

        result.Append(" -c copy -map 0:v");

        for (int i = 0; i < audioPathsList.Count; i++) {
            result.Append(" -map ");
            result.Append(i + 1);
            result.Append(":a");
        }

        result.Append(" \"");
        result.Append(outputPath);
        result.Append('"');
        
        return result.ToString();
    }
}