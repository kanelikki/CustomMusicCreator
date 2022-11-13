﻿using CustomMusicCreator.Utils;

namespace CustomMusicCreator
{
    /// <summary>
    /// Entry class of the app. This must be public.
    /// </summary>
    public class PataMusicCreator
    {
        private ILogger _logger;
        private MusicSplitter _splitter;
        private AtracConverter _atracConverter;
        private SgdConverter _sgdConverter;
        public PataMusicCreator(ILogger logger)
        {
            _logger = logger;
            _splitter = new MusicSplitter();
            _atracConverter = new AtracConverter(logger);
            _sgdConverter = new SgdConverter(logger);
        }
        /// <summary>
        /// Start converting music.
        /// </summary>
        /// <param name="baseMusicPath0">Intro music. played only once.</param>
        /// <param name="baseMusicPath1">Base music, before command input.</param>
        /// <param name="level1MusicPath">No fever command music phase 1 (fever worm doesn't bounce)</param>
        /// <param name="level2MusicPath">No fever command music phase 2 (fever worm bounces)</param>
        /// <param name="level3MusicPath">Fever command music</param>
        /// <note>For level 2 and level 3 music, the first 4 seconds part is intro, which means, the loop part starts from 00:04.</note>
        public void Convert(string baseMusicPath0, string baseMusicPath1,
            string level1MusicPath, string level2MusicPath, string level3MusicPath, string destinationPath)
        {
            try
            {
                if (!Directory.Exists(destinationPath))
                {
                    throw new InvalidDataException($"Directory [{destinationPath}] is invalid.");
                }
                string tempPath = Path.Combine(destinationPath, FilePathUtils.TempPath);
                if (Directory.Exists(tempPath))
                {
                    Directory.Delete(tempPath, true);
                }
                var tempDirectory = Directory.CreateDirectory(tempPath);

                ConvertEach(tempDirectory, baseMusicPath0, "base0", new TimeSpan(0,0,4));
                ConvertEach(tempDirectory,baseMusicPath1, "base1", new TimeSpan(0,0,4));
                ConvertEach(tempDirectory, level1MusicPath, "lv1", new TimeSpan(0,0,16));
                ConvertEach(tempDirectory, level2MusicPath, "lv2", new TimeSpan(0,0,20));
                ConvertEach(tempDirectory, level3MusicPath, "lv3", new TimeSpan(0,1,4));
            }
            catch(Exception e)
            {
                //Don't remove temp file here. Temp files can help for diagnosing.
                _logger.LogError(e.Message);
                throw;
            }
        }
        private void ConvertEach(DirectoryInfo directoryInfo, string filePath, string prefix, TimeSpan timeSpan)
        {
            string fileName = Path.GetFileName(filePath);
            _logger.LogMessage($"[ SPLITTER ] Started to Split --- {fileName}");
            var splitted = _splitter.ValidateAndLoadPaths(directoryInfo, filePath, prefix, timeSpan);
            _logger.LogMessage($"{fileName} successfully splitted.");
            _logger.LogMessage($"[ ATRAC CONVERTER ] Started to convert to Atrac--- {fileName}");
            var atracConverted = _atracConverter.Convert(splitted, prefix);
            _logger.LogMessage($"{fileName} successfully converted to atrac format.");
            _logger.LogMessage($"[ SGD CONVERTER ] Started to convert to Sgd--- {fileName}");
            var sgdConverted = _sgdConverter.ConvertAll(atracConverted);
            _logger.LogMessage($"{fileName} successfully converted to sgd.");
        }
    }
}
