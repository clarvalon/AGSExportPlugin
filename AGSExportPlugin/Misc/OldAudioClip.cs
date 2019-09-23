using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Clarvalon.XAGE.Global
{
    /// <summary>
    /// Used to make sure old Sound/SOUND5.ogg and Music/MUSIC12.wav files are brought across as AudioClips via OldAudio.xml
    /// </summary>
    public class OldAudioClip
    {
        public string ScriptID;
        public int Index;

        public OldAudioClip()
        {

        }

        public OldAudioClip(string scriptID, int index)
        {
            ScriptID = scriptID;
            Index = index;
        }
    }
}
