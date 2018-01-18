#region License
/* AGSExportPlugin 
 * Copyright 2010-2018 - Dan Alexander
 *
 * Released under the MIT License.  See LICENSE for details. */
#endregion

using System;
using System.Collections.Generic;
using System.Text;
using AGS.Types;

namespace AGSExportPlugin
{
    [RequiredAGSVersion("3.0.0.0")]
    public class CustomExportPlugin : IAGSEditorPlugin
    {
        public CustomExportPlugin(IAGSEditor editor)
        {
            editor.AddComponent(new CustomExportEditorComponent(editor));
        }

        public void Dispose()
        {
        }
    }
}
