﻿using System;
using ProjectEarthServerAPI.Models;
using ProjectEarthServerAPI.Models.Features;
using ProjectEarthServerAPI.Models.Player;

namespace ProjectEarthServerAPI.Util
{
	public class JournalUtils
	{
		public static bool UpdateEntry(string playerId, InventoryResponse.BaseItem item)
		{
			var baseJournal = ReadJournalForPlayer(playerId);
			var createEntry = !baseJournal.result.inventoryJournal.ContainsKey(item.id);

			if (createEntry)
			{
				var entry = new JournalEntry() {firstSeen = item.unlocked.on, lastSeen = item.seen.on};

				entry.amountCollected = item is InventoryResponse.StackableItem stackableItem
					? (uint)stackableItem.owned
					: (uint)((InventoryResponse.NonStackableItem)item).instances.Count;

				baseJournal.result.inventoryJournal.Add(item.id, entry);

				TokenUtils.AddItemToken(playerId, item.id);
			}
			else
			{
				var entry = baseJournal.result.inventoryJournal[item.id];
				var itemAmount = item is InventoryResponse.StackableItem stackableItem
					? (uint)stackableItem.owned
					: (uint)((InventoryResponse.NonStackableItem)item).instances.Count;

				if (entry.amountCollected < itemAmount) entry.amountCollected = itemAmount;

				entry.lastSeen = item.seen.on;

				baseJournal.result.inventoryJournal[item.id] = entry;
			}

			WriteJournalForPlayer(playerId, baseJournal);
			return true;
		}

		public static void AddActivityLogEntry(string playerId, DateTime eventTime, Scenario scenario,
		Rewards rewards = null, ChallengeDuration? duration = null, ActiveLocationType? gameplayMode = null,
		string boostId = null, Guid? referenceId = null, uint? order = null, bool? isFinalSeasonChallenge = null)
		{
			var journal = ReadJournalForPlayer(playerId);
			var activityLogEntry = new Activity
			{
				eventTime = eventTime,
				scenario = scenario,
				rewards = rewards,
				properties = new ActivityProperties {
					duration = duration,
					gameplayMode = gameplayMode,
					boostId = boostId,
					referenceId = referenceId,
					OrderUint = order,
					isFinalSeasonChallenge = isFinalSeasonChallenge
				}
			};

			journal.result.activityLog.Add(activityLogEntry);
			WriteJournalForPlayer(playerId, journal);
		}

		public static JournalResponse ReadJournalForPlayer(string playerId)
			=> GenericUtils.ParseJsonFile<JournalResponse>(playerId, "journal");

		public static void WriteJournalForPlayer(string playerId, JournalResponse data)
			=> GenericUtils.WriteJsonFile(playerId, data, "journal");
	}
}
