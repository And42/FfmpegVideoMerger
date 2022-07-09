using System;
using System.Linq;
using System.Windows;

namespace FfmpegVideoMerger.Logic; 

public static class FileDropUtils {

    public static void EnableFileDrop(
        this UIElement self,
        Func<string, bool> fileNameValidator,
        Action<string[]> onFilesDropped
    ) {
        self.AllowDrop = true;
        
        self.PreviewDragOver += (_, args) => {
            var data = args.Data.GetData(DataFormats.FileDrop) as string[];
            if (data == null || data.IsEmpty() || data.None(fileNameValidator)) {
                return;
            }

            args.Effects = DragDropEffects.Link;
            args.Handled = true;
        };

        self.Drop += (_, args) => {
            var data = args.Data.GetData(DataFormats.FileDrop) as string[];
            if (data == null || data.IsEmpty()) {
                return;
            }

            string[] validFiles = data.Where(fileNameValidator).ToArray();
            if (validFiles.IsEmpty()) {
                return;
            }
            
            onFilesDropped(validFiles);
            args.Handled = true;
        };
    }
}