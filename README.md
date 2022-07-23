# FfmpegVideoMerger

![01 - single file](https://user-images.githubusercontent.com/12385212/180601446-0f397e56-3e27-4241-b5fd-67e253ad4637.png)
![02 - multiple files](https://user-images.githubusercontent.com/12385212/180601447-290c0c7d-e325-4f22-814e-5e38d48d5f22.png)
![03 - settings](https://user-images.githubusercontent.com/12385212/180601448-16bac59c-28c0-4e76-97e1-396dce5318da.png)

This program allows you to combine video files with audio files both in a single to single file manner and multiple to multiple manner using regular expressions to figure out video to audio relations.

I rarely need this functionality but each time I actually need it I spend some time digging through my notes and figuring out how to apply what I've written before.
So, I've decided to create a separate app that I can install on the system and use kinda easily.
If you have the same problem as I have, you're welcome. If not, well, you're welcome too ¯\\_(ツ)_/¯.

There are two main requirements for using the app:
1. You need to have NET 6.0+ installed on your system. The installer doesn't download and install it by itself (at least at the moment of writing)
1. You need to have ffmpeg installed on your system. You can install it anywhere but then you should add the path to it to the app settings (or just `ffmpeg` if you add its directory to the `PATH` variable)

I could remove both those requirements by integrating NET 6.0 inside exe and ffmpeg in the target directory but that will make the installer around 300MB in size, so, nah, let's use shared libraries at the expense of user-friendliness
