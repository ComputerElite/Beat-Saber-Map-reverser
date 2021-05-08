using BeatSaber;
using Microsoft.WindowsAPICodePack.Shell;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BS_Map_reverser
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Path to map (zip): ");
            try
            {
                String zip = Console.ReadLine().Replace("\"", "");
                if (Directory.Exists("tmp")) Directory.Delete("tmp", true);
                Directory.CreateDirectory("tmp");
                String unzippedPath = "tmp\\" + Path.GetFileNameWithoutExtension(zip) + "\\";
                ZipFile.ExtractToDirectory(zip, "tmp\\" + Path.GetFileNameWithoutExtension(zip));
                if (!File.Exists(unzippedPath + "info.dat"))
                {
                    throw new Exception("Info.dat is non existent");
                }
                BeatSaberSong info = JsonSerializer.Deserialize<BeatSaberSong>(File.ReadAllText(unzippedPath + "info.dat"));
                if (!File.Exists(unzippedPath + info._songFilename))
                {
                    throw new Exception("Song file is missing");
                }
                File.Move(unzippedPath + info._songFilename, unzippedPath + info._songFilename.Replace(".egg", ".ogg"));
                ShellFile song = ShellFile.FromFilePath(unzippedPath + info._songFilename.Replace(".egg", ".ogg"));
                ulong songLength = (ulong)(song.Properties.System.Media.Duration.Value / 10000000); //Convert 100 ns to second
                File.Move(unzippedPath + info._songFilename.Replace(".egg", ".ogg"), unzippedPath + info._songFilename);
                Console.WriteLine("Song length: " + songLength + " seconds");
                float songTimeInBeats = (songLength - info._songTimeOffset / 1000) * (info.BPM / 60);
                Console.WriteLine("in beats: " + songTimeInBeats);
                Directory.CreateDirectory("tmp\\new");
                String newDirPath = "tmp\\new\\";
                Console.WriteLine("Copying song");
                foreach (String s in Directory.GetFiles(unzippedPath)) File.Copy(s, newDirPath + Path.GetFileName(s), true);
                foreach(BeatSaberSongBeatMapCharacteristic c in info.BeatMapCharacteristics)
                {
                    Console.WriteLine("Beat Map Characteristic: " + c.BeatMapCharacteristicName);
                    foreach(BeatSaberSongDifficulty d in c.Difficulties)
                    {
                        Console.WriteLine("Converting difficulty: " + d.DifficultyName);
                        if(!File.Exists(unzippedPath + d._beatmapFilename))
                        {
                            Console.WriteLine("Difficulty file doesn't exist. skipping");
                            continue;
                        }
                        DiffFile orgDiff = JsonSerializer.Deserialize<DiffFile>(File.ReadAllText(unzippedPath + d._beatmapFilename));
                        DiffFile newDiff = new DiffFile();
                        foreach(Note n in orgDiff._notes)
                        {
                            Note reversed = n;
                            reversed._time = songTimeInBeats - n._time;
                            newDiff._notes.Add(reversed);
                        }
                        Console.WriteLine("Revesed " + newDiff._notes.Count + " Notes");
                        foreach (Obstacle o in orgDiff._obstacles)
                        {
                            Obstacle reversed = o;
                            reversed._time = songTimeInBeats - o._time;
                            newDiff._obstacles.Add(reversed);
                        }
                        Console.WriteLine("Revesed " + newDiff._obstacles.Count + " Obstacles");
                        foreach (Event e in orgDiff._events)
                        {
                            Event reversed = e;
                            reversed._time = songTimeInBeats - e._time;
                            newDiff._events.Add(reversed);
                        }
                        Console.WriteLine("Revesed " + newDiff._events.Count + " Events");
                        Console.WriteLine("Saving reversed " + d._beatmapFilename);
                        if (File.Exists(newDirPath + d._beatmapFilename)) File.Delete(newDirPath + d._beatmapFilename);
                        File.WriteAllText(newDirPath + d._beatmapFilename, JsonSerializer.Serialize(newDiff));
                    }
                }
                Console.WriteLine("Reversing Audio");
                if (File.Exists(newDirPath + info._songFilename)) File.Delete(newDirPath + info._songFilename);
                ProcessStartInfo i = new ProcessStartInfo();
                i.FileName = "ffmpeg.exe";
                i.Arguments = "-i \"" + unzippedPath + info._songFilename + "\" -af areverse \"" + newDirPath + info._songFilename.Replace(".egg", ".ogg") + "\"";
                Process.Start(i).WaitForExit();
                if(info._songFilename.EndsWith(".egg")) info._songFilename = info._songFilename.Replace(".egg", ".ogg");
                info.SongName += "_reversed";
                Console.WriteLine("Saving altered info.dat");
                if (File.Exists(newDirPath + "info.dat")) File.Delete(newDirPath + "info.dat");
                File.WriteAllText(newDirPath + "info.dat", JsonSerializer.Serialize(info));
                Console.WriteLine("Zipping reversed song");

                if (File.Exists(info.SongName + "_reversed.zip")) File.Delete(info.SongName + "_reversed.zip");
                ZipFile.CreateFromDirectory(newDirPath, info.SongName + "_reversed.zip");
                Console.WriteLine("Finished");
            } catch (Exception e)
            {
                Console.WriteLine("Error: \n" + e.ToString());
            }
            Console.ReadLine();
        }
    }
}
