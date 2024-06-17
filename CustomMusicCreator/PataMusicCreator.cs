using CustomMusicCreator.Logics;
using CustomMusicCreator.Utils;
using System.Globalization;

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
        private VolumeAdjuster _volumeAdjuster;
        public PataMusicCreator(ILogger logger)
        {
            _logger = logger;
            _splitter = new MusicSplitter(logger);
            _atracConverter = new AtracConverter(logger);
            _sgdConverter = new SgdConverter(logger);
            _volumeAdjuster = new VolumeAdjuster();
        }
        /// <summary>
        /// Start converting music.
        /// </summary>
        /// <param name="model">Patapon music model, that contains music data.</param>
        /// <note>For level 2 and level 3 music, the first 4 seconds part is intro, which means, the loop part starts from 00:04.</note>
        public void Convert(PataMusicModel model)
        {
            try
            {
                if (!Directory.Exists(model.DestinationDirectory))
                {
                    throw new InvalidDataException($"Directory [{model.DestinationDirectory}] is invalid.");
                }
                string tempPath = Path.Combine(model.DestinationDirectory, FilePathUtils.TempPath);
                if (Directory.Exists(tempPath))
                {
                    Directory.Delete(tempPath, true);
                }
                var tempDirectory = Directory.CreateDirectory(tempPath);

                var musicUnits = new PataMusicUnit[]
                {
                    new PataMusicUnit(_logger, _splitter, _atracConverter, tempDirectory)
                        .SetInfo(model.BaseMusicPath, "base", new TimeSpan(0, 0, 8)),

                    new PataMusicUnit(_logger, _splitter, _atracConverter, tempDirectory)
                        .SetInfo(model.Level1MusicPath, "level1", new TimeSpan(0, 0, 16)),

                    new PataMusicUnit(_logger, _splitter, _atracConverter, tempDirectory)
                    .SetInfo(model.Level2MusicPath, "level2", new TimeSpan(0, 0, 20)),
                    new PataMusicUnit(_logger, _splitter, _atracConverter, tempDirectory)
                        .SetInfo(model.Level3MusicPath, "level3", new TimeSpan(0, 1, 8))
                };
                foreach (var musicUnit in musicUnits)
                {
                    musicUnit.Split();
                }
                List<string> atracList = new List<string>();
                foreach (var musicUnit in musicUnits)
                {
                    atracList.AddRange(musicUnit.ConvertToAtrac());
                }
                _logger.LogMessage("[ SGD CONVERTER ] Started to convert to Sgd");
                var sgdConverted = _sgdConverter.ConvertAll(atracList.ToArray());
                _logger.LogMessage("Files are converted to SGD successfully.");
                if (model.VolumeScale != 1)
                {
                    if (model.VolumeScale < 0)
                    {
                        _logger.LogMessage("Volume value cannot be negative. Skipping...");
                    }
                    else
                    {
                        _logger.LogMessage($"Adjusting volume to {model.VolumeScale.ToString("P2", CultureInfo.InvariantCulture)}%...");
                        _volumeAdjuster.SetVolume(sgdConverted, model.VolumeScale);
                    }
                }
                _logger.LogMessage("[ BND RePacker ] Start repacking progress... (logging may not supported)");

                try
                {
                    using var repacker = new BgmRepacker();
                    repacker.ReplaceFiles(sgdConverted);
                    repacker.ReplaceFile(new VoiceRetriever().LoadSgd(model.VoiceTheme));
                    repacker.Pack(model.DestinationPath);
                    _logger.LogMessage("Packing is successfully done.");
                    _logger.LogMessage("Cleaning the build...");
                    Directory.Delete(tempPath, true);
                    _logger.LogMessage($"The music is available on {model.DestinationPath} - Enjoy!");
                }
                catch (DllNotFoundException e)
                {
                    throw new DllNotFoundException("DLL not found. If you already have bndrepacker.dll, Try downloading Visual C++: " +
                        "https://learn.microsoft.com/en-US/cpp/windows/latest-supported-vc-redist", e);
                }
            }
            catch(Exception e)
            {
                //Don't remove temp file here. Temp files can help for diagnosing.
                _logger.LogError(e.Message);
                throw;
            }
        }
    }
}
