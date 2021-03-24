﻿using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Kontract.Interfaces.FileSystem;
using Kontract.Interfaces.Plugins.State;
using Kontract.Interfaces.Plugins.State.Archive;
using Kontract.Models.Archive;
using Kontract.Models.Context;
using Kontract.Models.IO;

namespace plugin_mt_framework.Archives
{
    class HfsState : IArchiveState, ILoadFiles, ISaveFiles, IReplaceFiles
    {
        private Hfs _hfs;

        public IList<IArchiveFileInfo> Files { get; private set; }
        public bool ContentChanged => true;//IsContentChanged();

        public HfsState()
        {
            _hfs = new Hfs();
        }

        public async Task Load(IFileSystem fileSystem, UPath filePath, LoadContext loadContext)
        {
            var fileStream = await fileSystem.OpenFileAsync(filePath);
            Files = _hfs.Load(fileStream);
        }

        public Task Save(IFileSystem fileSystem, UPath savePath, SaveContext saveContext)
        {
            var fileStream = fileSystem.OpenFile(savePath, FileMode.Create, FileAccess.ReadWrite);
            _hfs.Save(fileStream, Files);

            return Task.CompletedTask;
        }

        private bool IsContentChanged()
        {
            return Files.Any(x => x.ContentChanged);
        }

        public void ReplaceFile(IArchiveFileInfo afi, Stream fileData)
        {
            afi.SetFileData(fileData);
        }
    }
}
