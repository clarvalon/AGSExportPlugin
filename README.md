# AGS Export Plugin

This is an Editor Plugin for Adventure Game Studio ([AGS](http://www.adventuregamestudio.co.uk)), allowing all game elements and raw assets to be exported.

The intended use for this is as the first stage of conversion of AGS games to the Cross-Platform Adventure Game Engine ([XAGE](https://clarvalon.bitbucket.io)), however is generic enough to be used for other purposes.

## Usage

Copy the following binaries to the AGS Editor executable directory:

* AGS.Plugin.ExportToXAGE.dll
* __[OPTIONAL]__ potrace.exe and mkbitmap.exe from the Potrace website.  These are not included due to a restrictive license (GPL) however can be obtained [here](http://potrace.sourceforge.net/#downloading) and will ensure walkable areas are vectorised.
* __[OPTIONAL]__ ffmpeg.exe - again not included due to a restrictive license (GPL) - can be downloaded [here](https://www.ffmpeg.org/download.html#build-windows) and will ensure all audio is converted to .ogg.

After restarting AGS, a new option should appear on the toolbar, '*Export -> Prepare game for XAGE*':

![Toolbar](/Docs/Images/AGS1.png?raw=true "Toolbar")

![Select](/Docs/Images/AGS2.png?raw=true "Select")

![Complete](/Docs/Images/AGS3.png?raw=true "Complete")

If you have explicitly specified an output directory, you'll then be able to see the exported files.

![Files](/Docs/Images/Files.png?raw=true "Files")

Converting these exported files into an XAGE project will be detailed [here](https://clarvalon.bitbucket.io/documentation.html).

## Development

Requires Visual Studio and NET 3.5.  AGS.Types.dll has been included in the repository for convenience.

## License

This project is licensed under the MIT License - see the [LICENSE.md](LICENSE.md) file for details.

## Acknowledgments

* [Denzil Quixode](http://www.adventuregamestudio.co.uk/forums/index.php?action=profile;u=511) for his initial work on the XML Export code - see [this thread](http://www.adventuregamestudio.co.uk/forums/index.php?topic=37481.msg492564#msg492564).
* Peter Selinger for [Potrace](http://potrace.sourceforge.net).
* BitSetsNet for [RoaringBitset](https://github.com/BitSetsNet/BitSetsNet/blob/master/LICENSE).
* Plus numerous people who have contributed their AGS game source code to help improve the quality of the conversion process.

## Known Issues

* Debugging not currently easy - probably needs a logger to both display and write progress to a file in order to more easily identify export issues.
