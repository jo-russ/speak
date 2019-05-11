/*
 * Created by SharpDevelop.
 * Date: 2015-04-29
 * Time: 11:52
 */
using System;
using System.Speech.Synthesis;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Speak
{
    class Program
    {
        private static string paramStartsWith = "--";

        public static void Help(ReadOnlyCollection<InstalledVoice> voices) {
            Console.WriteLine();
            Console.WriteLine("Usage: " + Environment.GetCommandLineArgs()[0] + " ["+paramStartsWith+"v <voice>] ["+paramStartsWith+"s <speed>] ["+paramStartsWith+"l <language>] Sentence to say");
            Console.WriteLine("   " + paramStartsWith + "v <voice>    - select voice to synthetise from installed list");
            Console.WriteLine("   " + paramStartsWith + "s <speed>    - range -10...10");
            Console.WriteLine("   " + paramStartsWith + "l <language> - try to select voice based on language code. If not found - fallback to default.");
            Console.WriteLine("   " + paramStartsWith + "f <file>     - write sentence as wav to file.");
            Console.WriteLine("   " + paramStartsWith + "q            - be quiet. Must be first option specified.");
            Console.WriteLine();
            Console.WriteLine("Installed voices:");
            foreach (var voice in voices) {
                Console.WriteLine("\"" + voice.VoiceInfo.Name + "\" : "
                    + " ("+ voice.VoiceInfo.Age
                    + ", "+ voice.VoiceInfo.Gender
                    + ", "+ voice.VoiceInfo.Culture +")"
                    + voice.VoiceInfo.Description.Replace(voice.VoiceInfo.Name,""));
            }

            Console.WriteLine();
        }

        public static void WriteOut(bool quiet, string message) {
            if (!quiet) {
                Console.WriteLine(message);
            }
        }

        public static void Main(string[] args)
        {
            using (var ss = new SpeechSynthesizer()) {
                ReadOnlyCollection<InstalledVoice> voices = ss.GetInstalledVoices();
                if (args.Length == 0) { 
                	Help(voices); 
                	return;
                }
                string sentence = "";
                string nextparam = "";
                string option = "";
                bool quiet = false;
                string outFile = "";
                int argIndex = 0;
                for (argIndex = 0; argIndex < args.Length; argIndex++) {
                    if (args[argIndex].StartsWith(paramStartsWith)) {
                        option = args[argIndex].Substring(2);
                        if (argIndex < args.Length-1) {
                            nextparam = (args[argIndex+1]).Trim();
                        } else {
                            nextparam = "";
                        }
                        switch (option) {
                            case "q":
                                quiet = true;
                                break;
                            case "v":
                                ss.SelectVoice(nextparam);
                                WriteOut(quiet, "Using voice \"" + nextparam + "\"");
                                argIndex++;
                                break;
                            case "f":
                                outFile = nextparam;
                                WriteOut(false, "Writing sentence to \"" + outFile + "\".");
                                argIndex++;
                                break;
                            case "s":
                                ss.Rate = (Math.Max(-10,Math.Min(10,int.Parse(nextparam))));
                                WriteOut(quiet, "Setting speech speed to " + ss.Rate);
                                argIndex++;
                                break;
                            case "l":
                                bool lang_selected = false;
                                foreach(var voice in voices) {
                                    if (voice.VoiceInfo.Culture.ToString().ToUpper().StartsWith(nextparam.ToUpper())) {
                                        WriteOut(quiet, "Selecting voice \"" + voice.VoiceInfo.Name + "\" for language \"" + nextparam + "\"");
                                        ss.SelectVoice(voice.VoiceInfo.Name);
                                        lang_selected = true;
                                        break;
                                    }
                                }
                                if (lang_selected == false) {
                                    WriteOut(false, "No voice could be selected for language \"" + nextparam + "\"");
                                }
                                argIndex++;
                                break;
                            default:
                                WriteOut(quiet, "Unknown option \"" + paramStartsWith + option + "\".");
                                break;
                        }
                    } else {
                        sentence += " " + args[argIndex];
                    }
                }

                if (quiet) {
                    WriteOut(quiet, "I'll be quiet");
                }
                if (outFile != "") {
                    ss.SetOutputToWaveFile(outFile);
                } else {
                    WriteOut(quiet, "Sentence: " + sentence);
                }
                ss.Speak(sentence);

            }

        }
    }
}