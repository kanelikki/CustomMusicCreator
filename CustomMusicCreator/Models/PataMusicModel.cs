using CustomMusicCreator.Utils;

namespace CustomMusicCreator
{
    /// <summary>
    /// Model for <see cref="PataMusicCreator"/>, for passing data to the creator.
    /// </summary>
    /// <remarks>This can be generated for passing data, and conversion can be done in other place.</remarks>
    public class PataMusicModel
    {
        public readonly string BaseMusicPath;
        public readonly string Level1MusicPath;
        public readonly string Level2MusicPath;
        public readonly string Level3MusicPath;
        public readonly string VoiceTheme;
        public readonly string DestinationPath;
        public readonly string DestinationDirectory;
        public readonly double VolumeScale;
        /// <summary>
        /// Creates music data for passing to <see cref="PataMusicCreator"/>.
        /// </summary>
        /// <param name="baseMusicPath">No command (intro+loop) music path.</param>
        /// <param name="level1MusicPath">Command (loop) music path.</param>
        /// <param name="level2MusicPath">Good Command (before fever, intro+loop) music path.</param>
        /// <param name="level3MusicPath">Fever (intro+loop) music path.</param>
        /// <param name="voiceTheme">Voice theme name.</param>
        /// <param name="destinationPath">The path directory for the result file. Must be writable.</param>
        /// <param name="volumeScale">The scale of the volume.</param>
        public PataMusicModel(string baseMusicPath, string level1MusicPath, string level2MusicPath, string level3MusicPath,
            string voiceTheme, string destinationPath, double volumeScale = 1)
        {
            BaseMusicPath = baseMusicPath;
            Level1MusicPath = level1MusicPath;
            Level2MusicPath = level2MusicPath;
            Level3MusicPath = level3MusicPath;
            VoiceTheme = voiceTheme;
            DestinationPath = destinationPath;
            DestinationDirectory = FilePathUtils.GetParentDirectory(destinationPath);
            VolumeScale = volumeScale;
        }
    }
}
