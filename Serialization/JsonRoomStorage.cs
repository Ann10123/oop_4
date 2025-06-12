using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Model;

namespace Serialization
{
    public static class JsonRoomStorage
    {
        private static readonly string FilePath = "rooms_data.json";

        public static void Save(List<Rooms> roomList)
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                IncludeFields = true
            };

            string json = JsonSerializer.Serialize(roomList, options);
            File.WriteAllText(FilePath, json);
        }

        public static List<Rooms> Load()
        {
            if (!File.Exists(FilePath))
                return new List<Rooms>();

            string json = File.ReadAllText(FilePath);
            return JsonSerializer.Deserialize<List<Rooms>>(json, new JsonSerializerOptions
            {
                IncludeFields = true
            }) ?? new List<Rooms>();
        }
    }
}