#region License
/* AGSExportPlugin 
 * Copyright 2010-2018 - Dan Alexander
 *
 * Released under the MIT License.  See LICENSE for details. */
#endregion

using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text;
using System.Xml;
using System.IO;
using AGS.Types;
using CustomExportPlugin;
using Clarvalon.XAGE.Global;

namespace AGSExportPlugin
{
    /// <summary>
    /// AGS Editor component which performs the actual export.
    /// </summary>
    public class CustomExportEditorComponent : IEditorComponent
    {
        public IAGSEditor editor;

        private const string MenuID = "ExportMenu";
        private const string ExportRoomData = "ExportRoomData";
        private const string CompID = "CustomExportEditorComponent";

        private bool DebugMode = false; // Temporarily set this to true in order to diagnose bugs without attaching debugger to process
                                        // TODO:  Would probably best to plug in an actual logger
        private string PathRootDirectory;
        private string PathA2XFile;
        private string PathRoomBackgrounds; 
        private string PathRoomHotSpots; 
        private string PathRoomRegions; 
        private string PathRoomWalkAreas; 
        private string PathRoomWalkBehinds; 
        private string PathCharacterImages;
        private string PathFonts;
        private string PathAGSCode; 

        // External folders - may rework these to bring them in
        // Also need to rework these to handle new(er) AGS audio files
        private string PathExternalMP3MusicFolder;
        private string PathExternalOggMusicFolder;
        private string PathExternalWavFolder;
        private string PathExternalSpeechFolder;

        // Constructor
        public CustomExportEditorComponent(IAGSEditor owningEditor)
        {
            this.editor = owningEditor;
            editor.GUIController.AddMenu(this, MenuID, "E&xport", editor.GUIController.FileMenuID);
            MenuCommands newCommands = new MenuCommands(MenuID);
            newCommands.Commands.Add(new MenuCommand(ExportRoomData, "Prepare game for XAGE"));
            editor.GUIController.AddMenuItems(this, newCommands);
        }

        private void WriteBackgrounds(XmlTextWriter output, ILoadedRoom room, string roomName)
        {
            output.WriteStartElement("Backgrounds");
            for (int j = 0; j < room.BackgroundCount; j++)
            {
                using (Bitmap bmp = new Bitmap(room.Width, room.Height))
                using (Graphics g = Graphics.FromImage(bmp))
                {
                    // NEW - store as seperate .png file
                    editor.RoomController.DrawRoomBackground(g, 0, 0, j, 1);
                    string roomFileName = PathRoomBackgrounds + GetValidPath(roomName) + "_" + j + ".png";
                    //MessageBox.Show(roomFileName);
                    bmp.Save(roomFileName, ImageFormat.Png);
                }
            }
            output.WriteEndElement(); // Backgrounds
        }

        private void WriteCharacterIMGs(ISpriteFolder currentSpriteFolder, string parentFolder)
        {
            int numSprites = currentSpriteFolder.Sprites.Count; 
            string relativeFolder = Path.Combine(parentFolder, currentSpriteFolder.Name);
            if (relativeFolder == "Main")
                relativeFolder = string.Empty;
            string fullFolderName = Path.Combine(PathCharacterImages, relativeFolder);

            if (!Directory.Exists(fullFolderName))
                Directory.CreateDirectory(fullFolderName);

            // Save each sprite to file
            if (numSprites > 0)
            {
                for (int j = 0; j < numSprites; j++)
                {
                    int sn = currentSpriteFolder.Sprites[j].Number;
                    
                    using (Bitmap bmp =  editor.GetSpriteImage(sn)) 
                    {
                        Bitmap b2 = new Bitmap(bmp.Size.Width, bmp.Size.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                        using (Graphics g = Graphics.FromImage(b2))
                        {
                            g.DrawImage(bmp, new Point(0, 0));
                            b2.Dispose();
                            string num = AppendZerosBeginning(sn, 5);
                            
                            string spriteFileName = Path.Combine(fullFolderName, "spr" + num + ".png");
                            bmp.Save(spriteFileName, ImageFormat.Png);
                        }
                    }
                }
            }

            // Do same for subfolders
            foreach (ISpriteFolder sf in currentSpriteFolder.SubFolders)
            {
                WriteCharacterIMGs(sf, relativeFolder);
            }
        }

        private static string AppendZerosBeginning(int input, int length)
        {
            return input.ToString().PadLeft(length, Convert.ToChar("0"));
        }

        private void WriteFiddlyRoomBit(XmlTextWriter output, ILoadedRoom room, string roomName, RoomAreaMaskType getType)
        {
            string filename = "", directory = "";
            switch (getType)
            {
                case RoomAreaMaskType.Hotspots:
                    directory = PathRoomHotSpots;
                    filename = "HOTSPOT"; 
                    break;
                case RoomAreaMaskType.Regions:
                    directory = PathRoomRegions;
                    filename = "REGION"; 
                    break;
                case RoomAreaMaskType.WalkableAreas:
                    directory = PathRoomWalkAreas;
                    filename = "WALKAREA"; 
                    break;
                case RoomAreaMaskType.WalkBehinds:
                    directory = PathRoomWalkBehinds;
                    filename = "WALKBEHIND"; 
                    break;
            }

            // This function uses the GetAreaMaskPixel which doesn't deal with overlaps!
            // Get background bitmap
            using (Bitmap backBMP = new Bitmap(room.Width, room.Height))
            using (Graphics g = Graphics.FromImage(backBMP))
            {
                editor.RoomController.DrawRoomBackground(g, 0, 0, 0, 1);

                int xPos = 0, yPos = 0;
                switch (getType)
                {
                    case RoomAreaMaskType.Hotspots:
                        foreach (RoomHotspot hotspot in room.Hotspots)
                        {
                            string alphaFileName;
                            if (CreateAlphaMap(output, backBMP, room, getType, roomName, hotspot.ID, hotspot.Name, directory, filename, ref xPos, ref yPos, out alphaFileName))
                            {
                                output.WriteStartElement("Hotspot");
                                output.WriteAttributeString("ID", hotspot.ID.ToString());
                                output.WriteElementString("Description", hotspot.Description);
                                hotspot.Interactions.ToXml(output);
                                output.WriteElementString("Name", hotspot.Name);
                                hotspot.Properties.ToXml(output);
                                output.WriteElementString("WalkToPoint", hotspot.WalkToPoint.X + "," + hotspot.WalkToPoint.Y);
                                output.WriteElementString("XPos", xPos.ToString()); // add these so we know where to place
                                output.WriteElementString("YPos", yPos.ToString()); // ... anim frame in XAGE
                                output.WriteElementString("AlphaFileName", alphaFileName);
                                output.WriteEndElement(); // Hotspot
                            }
                        }
                        break;
                    case RoomAreaMaskType.Regions:
                        foreach (RoomRegion region in room.Regions)
                        {
                            string alphaFileName;
                            if (CreateAlphaMap(output, backBMP, room, getType, roomName, region.ID, region.ID.ToString(), directory, filename, ref xPos, ref yPos, out alphaFileName))
                            {
                                output.WriteStartElement("Region");
                                output.WriteAttributeString("ID", region.ID.ToString());
                                output.WriteElementString("BlueTint", region.BlueTint.ToString());
                                output.WriteElementString("GreenTint", region.GreenTint.ToString());
                                region.Interactions.ToXml(output);
                                output.WriteElementString("LightLevel", region.LightLevel.ToString());
                                output.WriteElementString("RedTint", region.RedTint.ToString());
                                output.WriteElementString("TintSaturation", region.TintSaturation.ToString());
                                output.WriteElementString("UseColourTint", region.UseColourTint.ToString());
                                output.WriteElementString("XPos", xPos.ToString()); // add these so we know where to place
                                output.WriteElementString("YPos", yPos.ToString()); // ... anim frame in XAGE
                                output.WriteElementString("AlphaFileName", alphaFileName);
                                output.WriteEndElement(); // Region
                            }
                        }
                        break;
                    case RoomAreaMaskType.WalkableAreas:
                        foreach (RoomWalkableArea walkable in room.WalkableAreas)
                        {
                            // Note:  Walkable Area bmps not cropped 
                            string alphaFileName;
                            if (CreateAlphaMap(output, backBMP, room, getType, roomName, walkable.ID, walkable.ID.ToString(), directory, filename, ref xPos, ref yPos, out alphaFileName))
                            {
                                output.WriteStartElement("WalkableArea");
                                output.WriteAttributeString("ID", walkable.ID.ToString());
                                output.WriteElementString("AreaSpecificView", walkable.AreaSpecificView.ToString());
                                output.WriteElementString("MaxScalingLevel", walkable.MaxScalingLevel.ToString());
                                output.WriteElementString("MinScalingLevel", walkable.MinScalingLevel.ToString());
                                output.WriteElementString("ScalingLevel", walkable.ScalingLevel.ToString());
                                output.WriteElementString("UseContinuousScaling", walkable.UseContinuousScaling.ToString());
                                output.WriteElementString("XPos", xPos.ToString()); // add these so we know where to place
                                output.WriteElementString("YPos", yPos.ToString()); // ... anim frame in XAGE
                                output.WriteElementString("AlphaFileName", alphaFileName);
                                output.WriteEndElement(); // WalkableArea
                            }
                        }
                        break;
                    case RoomAreaMaskType.WalkBehinds:
                        foreach (RoomWalkBehind walkbehind in room.WalkBehinds)
                        {
                            string alphaFileName;
                            if (CreateAlphaMap(output, backBMP, room, getType, roomName, walkbehind.ID, walkbehind.ID.ToString(), directory, filename, ref xPos, ref yPos, out alphaFileName))
                            {
                                output.WriteStartElement("WalkBehind");
                                output.WriteAttributeString("ID", walkbehind.ID.ToString());
                                output.WriteElementString("Baseline", walkbehind.Baseline.ToString());
                                output.WriteElementString("XPos", xPos.ToString()); // add these so we know where to place
                                output.WriteElementString("YPos", yPos.ToString()); // ... anim frame in XAGE
                                output.WriteElementString("AlphaFileName", alphaFileName);
                                output.WriteEndElement(); // WalkBehind
                            }
                        }
                        break;
                }
            }
        }

        private void WriteRoomObject(RoomObject obj, XmlTextWriter output)
        {
            output.WriteStartElement("Object");
            output.WriteAttributeString("ID", obj.ID.ToString());
            output.WriteElementString("Baseline", obj.Baseline.ToString());
            output.WriteElementString("BaselineOverridden", obj.BaselineOverridden.ToString());
            output.WriteElementString("Description", obj.Description);
            output.WriteElementString("EffectiveBaseline", obj.EffectiveBaseline.ToString());
            output.WriteElementString("Image", obj.Image.ToString());
            obj.Interactions.ToXml(output);

            output.WriteElementString("Name", obj.Name);
            obj.Properties.ToXml(output);

            output.WriteElementString("StartX", obj.StartX.ToString());
            output.WriteElementString("StartY", obj.StartY.ToString());
            output.WriteElementString("UseRoomAreaLighting", obj.UseRoomAreaLighting.ToString());
            output.WriteElementString("UseRoomAreaScaling", obj.UseRoomAreaScaling.ToString());
            output.WriteElementString("Visible", obj.Visible.ToString());
            output.WriteEndElement(); // Object
        }

        private void WriteRoom(IRoom room, ILoadedRoom roomLoaded, XmlTextWriter output)
        {
            string roomID = GetRoomID(room);
            output.WriteStartElement("AGSRoom");
            output.WriteAttributeString("Number", room.Number.ToString());

            output.WriteElementString("Description", roomID); // Todo GetRoomID?
            room.Script.ToXml(output);
            output.WriteElementString("StateSaving", room.StateSaving.ToString());

            output.WriteElementString("BackgroundAnimationDelay", roomLoaded.BackgroundAnimationDelay.ToString());
            WriteBackgrounds(output, roomLoaded, roomID);
            output.WriteElementString("BottomEdgeY", roomLoaded.BottomEdgeY.ToString());
            output.WriteElementString("ColorDepth", roomLoaded.ColorDepth.ToString());
            output.WriteElementString("GameID", roomLoaded.GameID.ToString());
            output.WriteElementString("Height", roomLoaded.Height.ToString());

            // Complicated Masks & XML

            // 1) Hotspots
            output.WriteStartElement("Hotspots");
            WriteFiddlyRoomBit(output, roomLoaded, roomID, RoomAreaMaskType.Hotspots);
            output.WriteEndElement(); // Hotspots
            
            // 2) Regions
            output.WriteStartElement("Regions");
            WriteFiddlyRoomBit(output, roomLoaded, roomID, RoomAreaMaskType.Regions);
            output.WriteEndElement(); // Regions
            
            // 3) Walkable Areas
            output.WriteStartElement("WalkableAreas");
            WriteFiddlyRoomBit(output, roomLoaded, roomID, RoomAreaMaskType.WalkableAreas);
            output.WriteEndElement(); // WalkableAreas
            
            // 4) Walk Behinds
            output.WriteStartElement("WalkBehinds");
            WriteFiddlyRoomBit(output, roomLoaded, roomID, RoomAreaMaskType.WalkBehinds);
            output.WriteEndElement(); // WalkBehinds

            roomLoaded.Interactions.ToXml(output);
            output.WriteElementString("LeftEdgeX", roomLoaded.LeftEdgeX.ToString());
            output.WriteStartElement("Messages");
            foreach (RoomMessage msg in roomLoaded.Messages)
            {
                WriteRoomMessage(msg, output);
            }
            output.WriteEndElement(); // Messages
            output.WriteElementString("MusicVolumeAdjustment", roomLoaded.MusicVolumeAdjustment.ToString());

            output.WriteStartElement("Objects");
            foreach (RoomObject obj in roomLoaded.Objects)
            {
                WriteRoomObject(obj, output);
            }
            output.WriteEndElement(); // Objects

            output.WriteElementString("PlayerCharacterView", roomLoaded.PlayerCharacterView.ToString());
            output.WriteElementString("PlayMusicOnRoomLoad", roomLoaded.PlayMusicOnRoomLoad.ToString());

            roomLoaded.Properties.ToXml(output);
            
            output.WriteElementString("Resolution", roomLoaded.Resolution.ToString());
            output.WriteElementString("RightEdgeX", roomLoaded.RightEdgeX.ToString());
            output.WriteElementString("SaveLoadEnabled", roomLoaded.SaveLoadEnabled.ToString());
            output.WriteElementString("ShowPlayerCharacter", roomLoaded.ShowPlayerCharacter.ToString());
            output.WriteElementString("TopEdgeY", roomLoaded.TopEdgeY.ToString());
            output.WriteElementString("Width", roomLoaded.Width.ToString());
            output.WriteEndElement(); // Room
        }

        private void WriteRoomMessage(RoomMessage msg, XmlTextWriter output)
        {
            output.WriteStartElement("Message");
            output.WriteAttributeString("ID", msg.ID.ToString());
            output.WriteElementString("AutoRemoveAfterTime", msg.AutoRemoveAfterTime.ToString());
            output.WriteElementString("CharacterID", msg.CharacterID.ToString());
            output.WriteElementString("DisplayNextMessageAfter", msg.DisplayNextMessageAfter.ToString());
            output.WriteElementString("ShowAsSpeech", msg.ShowAsSpeech.ToString());
            output.WriteElementString("Text", msg.Text);
            output.WriteEndElement(); 
        }

        public void CommandClick(string controlID) // Entry Point?
        {
            if (controlID == ExportRoomData)
            {
                DialogExportToXage dia = new DialogExportToXage(this);
                dia.ComponentRef = this;
                if (dia.ShowDialog() == DialogResult.OK)
                {
                    DialogSuccess d2 = new DialogSuccess();
                    d2.SetA2XFile(PathA2XFile);
                    d2.ShowDialog();
                }
            }
        }

        public void PopDebugMessage(string message)
        {
            if (DebugMode)
                MessageBox.Show(message);
        }

        public bool DoPreparation(string rootDirectory, bool generateFolderSuffix, string mp3Folder, string wavFolder, string speechFolder, string oggMusicFolder, DialogExportToXage dia)
        {
            List<string> roomIDList = new List<string>();
            string gameName = editor.CurrentGame.Settings.GameName.Replace(":", "");
            ResetPaths();
            
            if (!rootDirectory.EndsWith(@"\"))
                rootDirectory = rootDirectory + @"\";

            // Do we need to generate folder name based on gameID?
            if (generateFolderSuffix)
                rootDirectory = rootDirectory + "AGS to XAGE - " + gameName + @"\";

            PathRootDirectory = rootDirectory;
            CreateRequiredFolders();
            
            PathExternalOggMusicFolder = oggMusicFolder;
            PathExternalMP3MusicFolder = mp3Folder;
            PathExternalWavFolder = wavFolder;
            PathExternalSpeechFolder = speechFolder;
          
            string roomXmlFilename = Path.Combine(PathAGSCode, "AGS Rooms.xml");
            using (XmlTextWriter output = new XmlTextWriter(roomXmlFilename, Encoding.UTF8))
            {
                // Export Character Sprite IMGs
                dia.UpdateStatus("Exporting Sprites");
                WriteCharacterIMGs(editor.CurrentGame.Sprites, "");
    
                // Rooms
                output.Formatting = Formatting.Indented;
                output.Indentation = 2;
                output.IndentChar = ' ';
                output.WriteStartDocument();
                output.WriteStartElement("AGSRooms");
                foreach (IRoom room in editor.CurrentGame.Rooms)
                {
                    if (!editor.RoomController.LoadRoom(room))
                    {
                        MessageBox.Show("Unable to load room " + room.Number + "!", "Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                        continue;
                    }
                    string roomID = GetRoomID(room).Trim();
                    dia.UpdateStatus("Exporting Room '" + roomID + "'");
                    roomIDList.Add(roomID);        
                    WriteRoom(room, editor.RoomController.CurrentRoom, output);
                }
                output.WriteEndElement(); // Rooms
                output.WriteEndDocument();
            }

            // Copy original *.agf file and script files etc.
            dia.UpdateStatus("Exporting Code");
            CopyRequiredAgsFiles();

            // All done ok?  Then create our *.a2x file which holds our conversion info
            // We browse to this in XAGE in order to process coversion
            // First see if one exists and use that (so as not to overwrite anything important)
            AgsConversionProject proj = new AgsConversionProject();
            PathA2XFile = PathRootDirectory +  GetValidPath(gameName) + ".a2x";
            if (File.Exists(PathA2XFile))
                proj = GenericXmlSerializer.DeSerializeFromFile<AgsConversionProject>(PathA2XFile);

            // We need to make paths relative again, so reset
            ResetPaths();
            proj.PathAgsRoomIMGsFolder = PathRoomBackgrounds;
            proj.PathAgsFontsFolder = PathFonts;
            proj.PathAgsSpritesFolder = PathCharacterImages;
            proj.PathAgsRoomHotSpots = PathRoomHotSpots;
            proj.PathAgsRoomRegions = PathRoomRegions;
            proj.PathAgsRoomWalkAreas = PathRoomWalkAreas;
            proj.PathAgsRoomWalkBehinds = PathRoomWalkBehinds;
            proj.PathAgsCodeFolder = PathAGSCode;
            proj.PathAgsMusicFolder = PathExternalMP3MusicFolder;
            proj.PathAgsOggMusicFolder = PathExternalOggMusicFolder;
            proj.PathAgsSoundsFolder = PathExternalWavFolder;
            proj.PathAgsSpeechFolder = PathExternalSpeechFolder;
            proj.GameID = editor.CurrentGame.Settings.GameName;
            
            // Save it
            GenericXmlSerializer.SerializeToFile(PathA2XFile, proj);

            // Set error flag to false
            return false;
        }

        private string GetRoomID(IRoom agsRoom)
        {
            // Use room description, but if this is empty then use number
            string returnString;
            if (!string.IsNullOrEmpty(agsRoom.Description))
                returnString = agsRoom.Description;
            else
                returnString = "Room" + agsRoom.Number.ToString();
            return returnString.Trim();
        }

        private void CreateRequiredFolders()
        {
            PathFonts = CreateDir(PathFonts);
            PathRoomBackgrounds = CreateDir(PathRoomBackgrounds);
            PathRoomHotSpots = CreateDir(PathRoomHotSpots);
            PathRoomRegions = CreateDir(PathRoomRegions);
            PathRoomWalkAreas = CreateDir(PathRoomWalkAreas);
            PathRoomWalkBehinds = CreateDir(PathRoomWalkBehinds);
            PathCharacterImages = CreateDir(PathCharacterImages);
            PathAGSCode = CreateDir(PathAGSCode);
        }

        private string CreateDir(string dir)
        {
            dir = PathRootDirectory + dir;
            if (!dir.EndsWith(@"\"))
                dir = dir + @"\";
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            return dir;
        }

        private void ResetPaths()
        {
            // Have to restore here so we can run in in the same AGS session
            // Also need them relative when saved
            PathFonts = @"AGS Fonts\";
            PathRoomBackgrounds = @"AGS Room Backgrounds\";
            PathRoomHotSpots = @"AGS Room HotSpots\";
            PathRoomRegions = @"AGS Room Regions\";
            PathRoomWalkAreas = @"AGS Room WalkAreas\";
            PathRoomWalkBehinds = @"AGS Room WalkBehinds\";
            PathCharacterImages = @"AGS Sprites\";
            PathAGSCode = @"AGS Code\";
        }
        
        private void CopyRequiredAgsFiles()
        {
            string OriginalPath = editor.CurrentGame.DirectoryPath + @"\";
       
            // Copy .agf
            CopyFile(OriginalPath + "Game.agf", PathAGSCode);
            
            // Copy GlobalScript stuff
            CopyFile(OriginalPath + "GlobalScript.asc", PathAGSCode);
            CopyFile(OriginalPath + "GlobalScript.ash", PathAGSCode);

            // Copy all Room Scripts
            DirectoryInfo d = new DirectoryInfo(OriginalPath);
            foreach(FileInfo f in d.GetFiles("*.asc"))
            {
                CopyFile(f.FullName, PathAGSCode);
            }
            foreach (FileInfo f in d.GetFiles("*.ash"))
            {
                CopyFile(f.FullName, PathAGSCode);
            }

            // Copy all Fonts
            foreach (FileInfo f in d.GetFiles("*.wfn"))
            {
                CopyFile(f.FullName, PathFonts);
            }
            foreach (FileInfo f in d.GetFiles("*.sci"))
            {
                CopyFile(f.FullName, PathFonts);
            }
            foreach (FileInfo f in d.GetFiles("*.ttf"))
            {
                CopyFile(f.FullName, PathFonts);
            }

            // Copy User.ICO (if exists)
            CopyFile(OriginalPath + "USER.ico", PathRootDirectory);
        }

        private void CopyFile(string originalFile, string toDirectory)
        {
            FileInfo f = new FileInfo(originalFile);
            if (f.Exists)
                f.CopyTo(toDirectory + @"\" + f.Name, true);
        }

        private bool CreateAlphaMap(XmlTextWriter output, Bitmap backBMP, ILoadedRoom room, RoomAreaMaskType getType, string roomName, int ID, string description, string directory, string filename, ref int xPos, ref int yPos, out string alphaFileName)
        {
            alphaFileName = string.Empty;

            // Skip if hotspot 0?
            if (ID == 0)
                return false;

            // Create output bmp!
            using (Bitmap filteredBMP = new Bitmap(room.Width, room.Height))
            using (Graphics gHot = Graphics.FromImage(filteredBMP))
            {
                int minX = Int32.MaxValue, minY = Int32.MaxValue, maxX = -1, maxY = -1;
                int keptPixels = 0;

                for (int xx = 0; xx < room.Width; xx += 1)
                {
                    for (int yy = 0; yy < room.Height; yy += 1)
                    {
                        int hs = editor.RoomController.GetAreaMaskPixel(getType, xx, yy);
                        if (hs == ID)
                        {
                            keptPixels += 1;
                            if (xx < minX) minX = xx;
                            if (xx > maxX) maxX = xx;
                            if (yy < minY) minY = yy;
                            if (yy > maxY) maxY = yy;
                        }
                    }
                }

                // Done comparing bitmaps - did we find any pixels?  If not then return false
                if (keptPixels <= 0 || minX > maxX || minY > maxY)
                {
                    return false;
                }

                // Otherwise convert this into a BooleanMatrix
                // Lets add 1 to maxX and maxY to ensure we get the whole bmp
                maxX += 1;
                maxY += 1;
                int bmWidth = maxX - minX;
                int bmHeight = maxY - minY;
                BooleanMatrix bm = new BooleanMatrix(bmWidth, bmHeight);

                for (int xx = 0; xx < bmWidth; xx += 1)
                {
                    int imageX = minX + xx;
                    for (int yy = 0; yy < bmHeight; yy += 1)
                    {
                        int imageY = minY + yy;
                        int hs = editor.RoomController.GetAreaMaskPixel(getType, imageX, imageY);
                        if (hs == ID)
                        {
                            bm[xx, yy] = true;
                        }
                    }
                }

                // Save BooleanMatrix
                alphaFileName = directory + GetValidPath(roomName + "_" + filename + "_" + description + ".alpha");
                SaveBooleanMatrix(bm, alphaFileName);

                // Return alphaFileName as relative
                alphaFileName = alphaFileName.Replace(PathRootDirectory, "");

                // return XY pos
                xPos = minX;
                yPos = minY;  // to get bottom left corner
                
                // Create corresponding RoomXML data back at calling function
                return true;
            }
        }

        private void SaveBooleanMatrix(BooleanMatrix bm, string fileName)
        {
            using (FileStream fs = new FileStream(fileName, FileMode.Create))
            {
                bm.WriteToStream(fs);
            }
        }

        /// <summary>
        /// Strips out any characters that won't work in a filename
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        private static string GetValidPath(string filename)
        {
            string invalid = new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars());
            foreach (char c in invalid)
            {
                filename = filename.Replace(c.ToString(), "");
            }
            return filename;
        }

        #region Interface Gumf

        public void BeforeSaveGame()
        {
        }

        public string ComponentID
        {
            get { return CompID; }
        }

        public void EditorShutdown()
        {
        }

        public void FromXml(System.Xml.XmlNode node)
        {
        }

        public void GameSettingsChanged()
        {
        }

        public IList<MenuCommand> GetContextMenu(string controlID)
        {
            List<MenuCommand> list = new List<MenuCommand>();
            return list;
        }

        public void PropertyChanged(string propertyName, object oldValue)
        {
        }

        public void RefreshDataFromGame()
        {
        }

        public void ToXml(System.Xml.XmlTextWriter writer)
        {
        }

        #endregion
    }
}
