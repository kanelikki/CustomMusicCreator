namespace CustomMusicCreator.Logics
{
    internal class VolumeAdjuster
    {
        private const short _baseVolume = 0x1000;
        internal string[] SetVolume(string[] filePaths, double scale)
        {
            foreach (var path in filePaths)
            {
               SetVolumeEach(path, checked((short)(_baseVolume * scale)));
            }
            return filePaths;
        }
        private void SetVolumeEach(string path, short volume)
        {
            using var stream = File.OpenWrite(path);
            using var writer = new BinaryWriter(stream);
            stream.Position = 0x38;
            writer.Write(volume);
            stream.Position = 0x3A;
            writer.Write(volume);
        }
    }
}
