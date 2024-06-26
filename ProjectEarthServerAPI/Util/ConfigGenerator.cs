﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace ProjectEarthServerAPI.Util
{
	public class ConfigGenerator
	{
		public void GenerateConfigFile(string configFilePath)
		{
			// Define default configuration
			var defaultConfig = new Dictionary<string, object>
			{
				{ "baseServerIP", "http://localhost:80" },
				{ "useBaseServerIP", false },
				{ "tileServerUrl", "http://localhost:8080" },
				{ "playfabTitleId", "20CA2" },
				{ "itemsFolderLocation", "./data/items/" },
				{ "efficiencyCategoriesFolderLocation", "./data/efficiency_categories/" },
				{ "journalCatalogFileLocation", "./data/journalCatalog.json" },
				{ "recipesFileLocation", "./data/recipes.json" },
				{ "settingsFileLocation", "./data/settings.json" },
				{ "challengeStorageFolderLocation", "./data/challenges/" },
				{ "buildplateStorageFolderLocation", "./data/buildplates/" },
				{ "sharedBuildplateStorageFolderLocation", "./data/shared_buildplates/" },
				{ "productCatalogFileLocation", "./data/productCatalog.json" },
				{ "ShopItemDictionaryFileLocation", "./data/shopItemDictionary.json" },
				{ "LevelDictionaryFileLocation", "./data/levelDictionary.json" },
				{ "EncounterLocationsFileLocation", "./data/encounterLocations.json" },
				{ "maxTappableSpawnAmount", 100 },
				{ "tappableSpawnRadius", 1 },
				{ "tappableExpirationTime", 10 }, // Minutes
                { "biomeGeneration", true }, // Custom Generation. Needs Tile Server!!
                { "publicAdventureSpawnPercentage", 5 }, // Percentage of Adventure Spawn rate
                { "publicAdventuresLimit", 5 }, // Public Adventures Limit
                { "webPanel", true },
				{ "webPanelPassword", "password" },
				{ "maxTappablesPerTile", 20 },
				{ "resourcepack", "https://github.com/andiricum2/MC-Earth-Resourcepack/releases/download/v1/vanilla.zip" },
				{ "buildplates",  "https://github.com/andiricum2/MC-Earth-Buildplates/releases/download/v1/Buildplates.zip" },
				{ "perRequestMaxTappableSpawnsInTile", 10 },
				{ "multiplayerAuthKeys", new Dictionary<string, string>
					{
						{ "YOURCLOUDBURSTSERVERPUBLICIP", "YOURKEY" }
					}
				}
			};

			// Load existing configuration or use default if it doesn't exist
			var existingConfig = LoadConfigFromFile(configFilePath) ?? defaultConfig;

			// Update existing configuration with default values where necessary
			UpdateConfigWithDefaults(existingConfig, defaultConfig);

			// Save the updated configuration
			SaveConfigToFile(existingConfig, configFilePath);
		}

		private Dictionary<string, object> LoadConfigFromFile(string configFilePath)
		{
			if (!File.Exists(configFilePath))
				return null;

			try
			{
				string existingJson = File.ReadAllText(configFilePath);
				return JsonConvert.DeserializeObject<Dictionary<string, object>>(existingJson);
			}
			catch (Exception ex)
			{
				Console.WriteLine("Error while reading existing configuration file: " + ex.Message);
				return null;
			}
		}

		private void UpdateConfigWithDefaults(Dictionary<string, object> existingConfig, Dictionary<string, object> defaultConfig)
		{
			foreach (var kvp in defaultConfig)
			{
				string propertyName = kvp.Key;

				// Update property with default value if it doesn't exist or its value is null
				if (!existingConfig.ContainsKey(propertyName) || existingConfig[propertyName] == null)
				{
					existingConfig[propertyName] = kvp.Value;
				}
			}
		}

		private void SaveConfigToFile(Dictionary<string, object> config, string configFilePath)
		{
			try
			{
				string configJson = JsonConvert.SerializeObject(config, Formatting.Indented);
				File.WriteAllText(configFilePath, configJson);
			}
			catch (Exception ex)
			{
				Console.WriteLine("Error while saving configuration file: " + ex.Message);
			}
		}
	}
}
