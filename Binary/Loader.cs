using Newtonsoft.Json;

    namespace Binary
    {
        internal class LevelsConfig
        {
            public Dictionary<string, BoardConfig>? Levels { get; set; }
        }

        internal class BoardConfig
        {
            public int Rows { get; set; }
            public int Cols { get; set; }
            public string[][]? Board { get; set; }
            public int[,]? StoneValues { get; set; }
        }

        internal static class Loader
        {
            public static BoardConfig LoadBoardConfig(string jsonFilePath, string levelKey)
            {
                var jsonData = File.ReadAllText(jsonFilePath);
                var levelsConfig = JsonConvert.DeserializeObject<LevelsConfig>(jsonData);

                if (levelsConfig?.Levels == null || !levelsConfig.Levels.ContainsKey(levelKey))
                {
                    throw new Exception($"Level {levelKey} not found.");
                }

                return levelsConfig.Levels[levelKey];
            }
        }
    }

