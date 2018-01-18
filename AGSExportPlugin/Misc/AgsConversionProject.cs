#region License
/* AGSExportPlugin 
 * Copyright 2010-2018 - Dan Alexander
 *
 * Released under the MIT License.  See LICENSE for details. */
#endregion

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml.Serialization;
using Clarvalon.XAGE.Global;

namespace Clarvalon.XAGE.Global
{
    /// <summary>
    /// Stores details of an AGS Project that is being exported and subsequently converted to XAGE.
    /// Useful in the event the conversion is performed multiple times.
    /// </summary>
    public class AgsConversionProject
    {
        // AGS Project name
        public string GameID;

        // Ags Conversion Paths & Extras
        public string PathAgsCodeFolder;
        public string PathAgsSpritesFolder;
        public string PathAgsRoomIMGsFolder;
        public string PathAgsRoomHotSpots;
        public string PathAgsRoomRegions;
        public string PathAgsRoomWalkAreas;
        public string PathAgsRoomWalkBehinds;
        public string PathAgsSoundsFolder;
        public string PathAgsMusicFolder;
        public string PathAgsOggMusicFolder;
        public string PathAgsSpeechFolder;

        public byte SpriteAgsBackColourRed;
        public byte SpriteAgsBackColourGreen;
        public byte SpriteAgsBackColourBlue;

        public int FPS; // 40 by default
        public string Namespace;
        public string WalkableAreasToImportFile;
        public bool KeepExistingGameSettings;

        // Paths - Relative
        public string CSharpProject;
        public string BaseFolder;

        // Paths - Full
        public string PathFinalXageFolder; 

        [XmlIgnore]
        public string CSharpProjectFull
        {
            get
            {
                return Path.Combine(PathFinalXageFolder, CSharpProject);
            }
        }

        [XmlIgnore]
        public string BaseFolderFull
        {
            get
            {
                return Path.Combine(PathFinalXageFolder, BaseFolderFull);
            }
        }
        
        public AgsConversionProject()
        {
            GameID = string.Empty;

            // Ags -> Xage paths
            PathAgsCodeFolder = string.Empty;
            PathAgsSpritesFolder = string.Empty;
            PathAgsRoomIMGsFolder = string.Empty;
            PathAgsRoomHotSpots = string.Empty;
            PathAgsRoomRegions = string.Empty;
            PathAgsRoomWalkAreas = string.Empty;
            PathAgsRoomWalkBehinds = string.Empty;
            PathAgsSoundsFolder = string.Empty;
            PathAgsMusicFolder = string.Empty;
            PathAgsOggMusicFolder = string.Empty;
            PathAgsSpeechFolder = string.Empty;
            PathFinalXageFolder = string.Empty;
            WalkableAreasToImportFile = string.Empty;

            // Purple colour tends to be default in AGS
            SpriteAgsBackColourRed = 255;
            SpriteAgsBackColourGreen = 0;
            SpriteAgsBackColourBlue = 255;

            FPS = 40;
        }
    }
}
