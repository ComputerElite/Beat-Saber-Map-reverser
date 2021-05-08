using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;


namespace BS_Map_reverser
{

    public class BeatSaberSong
    {
        [JsonPropertyName("_songName")]
        public string SongName { get; set; } = "N/A";
        [JsonPropertyName("_songSubName")]
        public string SubName { get; set; } = "N/A";
        [JsonPropertyName("_songAuthorName")]
        public string SongArtist { get; set; } = "N/A";
        [JsonPropertyName("_levelAuthorName")]
        public string Mapper { get; set; } = "N/A";
        [JsonPropertyName("_beatsPerMinute")]
        public float BPM { get; set; } = 0.0f;
        public string Hash { get; set; } = "N/A";
        public string Key { get; set; } = "N/A";
        public string hash { get; set; } = "N/A";
        public string key { get; set; } = "N/A";
        public bool cached { get; set; } = false;
        public bool RequestGood { get; set; } = false;
        [JsonPropertyName("_difficultyBeatmapSets")]
        public List<BeatSaberSongBeatMapCharacteristic> BeatMapCharacteristics { get; set; } = new List<BeatSaberSongBeatMapCharacteristic>();
        public JsonElement _customData { get; set; } = JsonSerializer.Deserialize<JsonElement>("{}");

        public string _allDirectionsEnvironmentName { get; set; } = "N/A";
        public string _environmentName { get; set; } = "N/A";
        public string _coverImageFilename { get; set; } = "N/A";
        public string _songFilename { get; set; } = "N/A";
        public float _previewDuration { get; set; } = 0.0f;
        public float _previewStartTime { get; set; } = 0.0f;
        public float _shufflePeriod { get; set; } = 0.0f;
        public float _shuffle { get; set; } = 0.0f;
        public float _songTimeOffset { get; set; } = 0.0f;
        public string _version { get; set; } = "N/A";

        public string GetJSON()
        {
            return JsonSerializer.Serialize(this);
        }
    }

    public class BeatSaberSongDifficulty
    {
        public float Duration { get; set; } = 0.0f;
        public float Length { get; set; } = 0.0f;
        [JsonPropertyName("_noteJumpMovementSpeed")]
        public double NJS { get; set; } = 0.0;
        [JsonPropertyName("_noteJumpStartBeatOffset")]
        public float NJSOffset { get; set; } = 0.0f;
        public int Bombs { get; set; } = 0;
        public int Notes { get; set; } = 0;
        public int Obstacles { get; set; } = 0;
        [JsonPropertyName("_difficulty")]
        public string DifficultyName { get; set; } = "N/A";
        [JsonPropertyName("_difficultyRank")]
        public int DifficultyID { get; set; } = 0;
        public JsonElement _customData { get; set; } = JsonSerializer.Deserialize<JsonElement>("{}");

        public string _beatmapFilename { get; set; } = "N/A";

        public bool IsEasy()
        {
            if (DifficultyName.ToLower() == "easy") return true;
            else return false;
        }

        public bool IsNormal()
        {
            if (DifficultyName.ToLower() == "normal") return true;
            else return false;
        }

        public bool IsHard()
        {
            if (DifficultyName.ToLower() == "hard") return true;
            else return false;
        }

        public bool IsExpert()
        {
            if (DifficultyName.ToLower() == "expert") return true;
            else return false;
        }

        public bool IsExpertPlus()
        {
            if (DifficultyName.ToLower() == "expertplus") return true;
            else return false;
        }
    }

    public class BeatSaberSongBeatMapCharacteristic
    {
        [JsonPropertyName("_beatmapCharacteristicName")]
        public string BeatMapCharacteristicName { get; set; } = "N/A";
        [JsonPropertyName("_difficultyBeatmaps")]
        public List<BeatSaberSongDifficulty> Difficulties { get; set; } = new List<BeatSaberSongDifficulty>();
    }


}

namespace BeatSaber
{
    //read more here https://bsmg.wiki/mapping/map-format.html
    [Serializable]
    public class DiffFile
    {
        //Only 2.0.0 supported
        public string _version { get; set; } = "2.0.0";
        public List<Event> _events { get; set; } = new List<Event>();
        public List<Note> _notes { get; set; } = new List<Note>();
        public List<Obstacle> _obstacles { get; set; } = new List<Obstacle>();
    }

    public class Obstacle
    {
        //in beats
        public float _time { get; set; } = 0.0f;
        public int _lineIndex { get; set; } = 0;
        //0, full height wall; 1, crouch/duck wall
        public int _type { get; set; } = 0;
        public float _duration { get; set; } = 0.0f;
        public int _width { get; set; } = 0;

    }

    public class Event
    {
        //in beats
        public float _time { get; set; } = 0.0f;
        public int _lineIndex { get; set; } = 0;
        public int _lineLayer { get; set; } = 0;
        /*
        0	Controls lights in the Back Lasers group.
        1	Controls lights in the Ring Lights group.
        2	Controls lights in the Left Rotating Lasers group.
        3	Controls lights in the Right Rotating Lasers group.
        4	Controls lights in the Center Lights group.
        5	(unused) Controls boost light colors (secondary colors).
        6	Unused.
        7	Unused.
        8	Creates one ring spin in the environment. Is not affected by _value.
        9	Controls zoom for applicable rings. Is not affected by _value.
        10	(unused) Official BPM Changes.
        11	Unused.
        12	Controls rotation speed for applicable lights in Left Rotating Lasers.
        13	Controls rotation speed for applicable lights in Right Rotating Lasers.
        14	(Previously unused) 360/90 Early rotation. Rotates future objects, while also rotating objects at the same time.
        15	(Previously unused) 360/90 Late rotation. Rotates future objects, but ignores rotating objects at the same time.*/
        public int _type { get; set; } = 0;
        /*
        ###Lights
        0	Turns the light group off.
        1	Changes the lights to blue, and turns the lights on.
        2	Changes the lights to blue, and flashes brightly before returning to normal.
        3	Changes the lights to blue, and flashes brightly before fading to black.
        4	Unused.
        5	Changes the lights to red, and turns the lights on.
        6	Changes the lights to red, and flashes brightly before returning to normal.
        7	Changes the lights to red, and flashes brightly before fading to black.*/

        public int _value { get; set; } = 0;
        //0, up; 1, down; 2, left; 3, right; 4, up left; 5, up right; 6, down left; 7 , down right; 8, any (dot note)
        public int _cutDirection { get; set; } = 0;
    }

    public class Note
    {
        //in beats
        public float _time { get; set; } = 0.0f;
        public int _lineIndex { get; set; } = 0;
        public int _lineLayer { get; set; } = 0;
        //0, left red; 1, right blue; 2, unused; 3, Bomb
        public int _type { get; set; } = 0;
        //0, up; 1, down; 2, left; 3, right; 4, up left; 5, up right; 6, down left; 7 , down right; 8, any (dot note)
        public int _cutDirection { get; set; } = 0;
    }

    public class BeatSaberSong
    {
        public string _songName;
        public string _songSubName = "N/A";
        public string _songAuthorName = "N/A";
        public string _levelAuthorName = "N/A";
        public float _beatsPerMinute;
        public bool cached = false;
        public bool RequestGood = false;
        public List<BeatSaberSongBeatMapCharacteristic> _difficultyBeatmapSets = new List<BeatSaberSongBeatMapCharacteristic>();
        //public JsonElement _customData  = JsonSerializer.Deserialize<JsonElement>("{}");

        public string _allDirectionsEnvironmentName = "N/A";
        public string _environmentName = "N/A";
        public string _coverImageFilename = "N/A";
        public string _songFilename = "N/A";
        public float _previewDuration = 0.0f;
        public float _previewStartTime = 0.0f;
        public float _shufflePeriod = 0.0f;
        public float _shuffle = 0.0f;
        public float _songTimeOffset = 0.0f;
        public string _version = "N/A";
    }

    public class BeatSaberSongDifficulty
    {
        public float Duration = 0.0f;
        public float Length = 0.0f;
        public double _noteJumpMovementSpeed = 0.0;
        public float _noteJumpStartBeatOffset = 0.0f;
        public int Bombs = 0;
        public int Notes = 0;
        public int Obstacles = 0;
        public string _difficulty = "N/A";
        public int _difficultyRank = 0;
        //public JsonElement _customData  = JsonSerializer.Deserialize<JsonElement>("{}");

        public string _beatmapFilename = "N/A";
    }

    public class BeatSaberSongBeatMapCharacteristic
    {
        public string _beatmapCharacteristicName = "N/A";
        public List<BeatSaberSongDifficulty> _difficultyBeatmaps = new List<BeatSaberSongDifficulty>();
    }
}