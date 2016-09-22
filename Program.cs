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
        
        public static void Help() {
            Console.WriteLine();
            Console.WriteLine("Usage: " + Environment.GetCommandLineArgs()[0] + " ["+paramStartsWith+"v <voice>] ["+paramStartsWith+"s <speed>] ["+paramStartsWith+"l <language>] Sentence to say");
            Console.WriteLine("   " + paramStartsWith + "v <voice>    - select voice to synthetise from installed list");
            Console.WriteLine("   " + paramStartsWith + "s <speed>    - range -10...10");
            Console.WriteLine("   " + paramStartsWith + "l <language> - try to select voice based on language code. If not found - fallback to default.");
            
            Console.WriteLine();
        }
        public static void Main(string[] args)
        {
            using (var ss = new SpeechSynthesizer()) {
                if (args.Length == 0) { Help(); }
                ReadOnlyCollection<InstalledVoice> voices = ss.GetInstalledVoices();
                Console.WriteLine("Installed voices:");
                foreach (var voice in voices) {
                    Console.Write("\"" + voice.VoiceInfo.Name + "\" : ");
                    Console.Write(" ("+ voice.VoiceInfo.Age +"");
                    Console.Write(", "+ voice.VoiceInfo.Gender +"");
                    Console.Write(", "+ voice.VoiceInfo.Culture +")");
                    Console.Write(""+ voice.VoiceInfo.Description.Replace(voice.VoiceInfo.Name,"") +"");
                    Console.WriteLine();
                }
                string sentence = "";
                int skip = 0;
                string nextparam = "";                
                bool validParam = false;
                foreach (var toSay in args) {

                    // effectively case rewritten to if's
                    if (nextparam == paramStartsWith+"v") {
                        ss.SelectVoice(toSay.Trim());
                        Console.WriteLine("Using voice \"" + toSay.Trim() + "\"");
                        validParam = true;
                    } 
                    if (nextparam == paramStartsWith+"s") {
                        ss.Rate = (Math.Max(-10,Math.Min(10,int.Parse(toSay.Trim()))));
                        Console.WriteLine("Setting speech speed to " + ss.Rate);
                        validParam = true;
                    }
                    if (nextparam == paramStartsWith+"l") {
                        bool lang_selected = false;
                        foreach(var voice in voices) {
                            if (voice.VoiceInfo.Culture.ToString().ToUpper().StartsWith(toSay.Trim().ToUpper())) {
                                Console.WriteLine("Setting voice \"" + voice.VoiceInfo.Name + "\" for language \"" + toSay.Trim() + "\"");
                                ss.SelectVoice(voice.VoiceInfo.Name);
                                lang_selected = true;
                                break;
                            }
                        }
                        if (lang_selected == false) {
                            Console.WriteLine("No voice could be selected for language \"" + toSay.Trim() + "\"");
                        }
                        validParam = true;
                    }
                    if ((!validParam) && (nextparam != "")) {
                        Console.WriteLine("Unknown option \"" + nextparam + "\"");
                    }                            
                    nextparam = "";
                    
                    
                    // manage switches
                    if (toSay.StartsWith(paramStartsWith)) {
                        skip = 2;
                        nextparam = toSay;
                    }
                    
                    // skip n params if it is parameter
                    if (skip == 0) {
                        sentence += toSay;
                    } else {
                        skip--;
                    }
                }
                ss.Speak(sentence);
            }

        }
    }
}