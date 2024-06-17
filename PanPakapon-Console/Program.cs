using CustomMusicCreator;

namespace PanPakapon.ConsoleApp
{
    class Program
    {
        private const string _defaultName = "BGM.DAT";
        static void Main(string[] args)
        {
            if (args.Length < 5)
            {
                Console.WriteLine("Usage:" +
                    $"\n{Path.GetFileName(Environment.GetCommandLineArgs()[0])}" +
                    " BASE_THEME_PATH LEVEL1_THEME_PATH LEVEL2_THEME_PATH LEVEL_3_THEME_PATH VOICE_THEME [VOLUME] [OUTPUT_NAME]" +
                    "\nVolume and output name are optional.");
                PrintVoices();
            }
            else if (!VoiceData.Get().HasVoice(args[4]))
            {
                Console.WriteLine("Voice name is invalid.");
                PrintVoices();
            }
            else
            {
                string filePath;
                double volume = 1;
                if (args.Length < 6) //no volume, no path
                {
                    filePath = Path.Combine(Directory.GetCurrentDirectory(), _defaultName);
                }
                else if (args.Length < 7) //path only or volume only
                {
                    if (double.TryParse(args[5], out volume))
                    {
                        filePath = Path.Combine(Directory.GetCurrentDirectory(), _defaultName);
                        Console.WriteLine($"Warning: Use {args[5]} as VOLUME, output path is still {_defaultName}");
                    }
                    else
                    {
                        volume = 1;
                        filePath = Path.GetFullPath(args[5]);
                        Console.WriteLine($"Warning: Use {args[5]} as PATH, output volume is still 100%");
                    }
                }
                else if(double.TryParse(args[5], out volume)) //both path and volume
                {
                    filePath = Path.Combine(Directory.GetCurrentDirectory(), args[6]);
                }
                else
                {
                    throw new ArgumentException($"{args[5]} is not valid volume");
                }
                var creator = new PataMusicCreator(new ConsoleLogger());
                creator.Convert(new PataMusicModel(
                    args[0], args[1], args[2], args[3], args[4], filePath, volume
                    ));
            }
        }
        static void PrintVoices()
        {
            Console.WriteLine("Available voices:\n" + string.Join('\n', VoiceData.Get().Voices) +
                "\nRemember to put voice name with number!\n"+
                "Adding Custom Voice:\n" +
                "Make *new folder* in \"resources/files/voices\", and put \"ptp_btl_bgm_voice.dat\" there. Then open the Pan Pakapon app - you'll see the custom music from the list.\n" +
                "For more info about Custom chants, check this guide (by Axus): https://docs.google.com/document/d/1rvkxxJ8OcqYgyw_RP4jU3eVAE-uf8FBdI-C-Yibi-Ac/edit");
        }
    }
}


