﻿using CustomMusicCreator.Utils;
using System.ComponentModel.Design;

namespace CustomMusicCreator
{
    public class VoiceData
    {
        private const string _voiceRelativePath = "files/voices";
        private const string _wavDirectoryName = "voices";
        public const string SgdName = "ptp_btl_bgm_voice.sgd";
        private readonly string _voicePath;
        private static VoiceData? _voiceData;
        private readonly Dictionary<string, DirectoryInfo> _availableVoices
            = new Dictionary<string, DirectoryInfo>();
        public IEnumerable<string> Voices => _availableVoices.Keys;
        private VoiceData()
        {
            _voicePath = Path.Combine(FilePathUtils.ResourcePath, _voiceRelativePath);
            var voices = new DirectoryInfo(_voicePath).GetDirectories();
            foreach (var voice in voices)
            {
                if (File.Exists(Path.Combine(voice.FullName, SgdName)))
                {
                    _availableVoices.Add(voice.Name, voice);
                }
            }
        }
        public static VoiceData Get()
        {
            if (_voiceData == null)
            {
                _voiceData = new VoiceData();
            }
            return _voiceData;
        }
        public bool HasVoice(string voiceName) => _availableVoices.ContainsKey(voiceName);
        public DirectoryInfo? GetVoiceDirectory(string directory)
        {
            if (!_availableVoices.TryGetValue(directory, out var dirInfo))
            {
                return null;
            }
            else return dirInfo;
        }
    }
}
