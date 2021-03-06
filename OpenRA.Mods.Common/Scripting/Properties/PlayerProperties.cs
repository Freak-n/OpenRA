#region Copyright & License Information
/*
 * Copyright 2007-2015 The OpenRA Developers (see AUTHORS)
 * This file is part of OpenRA, which is free software. It is made
 * available to you under the terms of the GNU General Public License
 * as published by the Free Software Foundation. For more information,
 * see COPYING.
 */
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using Eluant;
using OpenRA.Graphics;
using OpenRA.Mods.Common.Traits;
using OpenRA.Scripting;

namespace OpenRA.Mods.Common.Scripting
{
	[ScriptPropertyGroup("Player")]
	public class PlayerProperties : ScriptPlayerProperties
	{
		public PlayerProperties(ScriptContext context, Player player)
			: base(context, player) { }

		[Desc("The player's name.")]
		public string Name { get { return Player.PlayerName; } }

		[Desc("The player's color.")]
		public HSLColor Color { get { return Player.Color; } }

		[Desc("The player's race. (DEPRECATED! Use the `Faction` property.)")]
		public string Race
		{
			get
			{
				Game.Debug("The property `PlayerProperties.Race` is deprecated! Use `PlayerProperties.Faction` instead!");
				return Player.PlayerReference.Faction;
			}
		}

		[Desc("The player's faction.")]
		public string Faction { get { return Player.PlayerReference.Faction; } }

		[Desc("The player's spawnpoint ID.")]
		public int Spawn { get { return Player.SpawnPoint; } }

		[Desc("The player's team ID.")]
		public int Team { get { return Player.PlayerReference.Team; } }

		[Desc("Returns true if the player is a bot.")]
		public bool IsBot { get { return Player.IsBot; } }

		[Desc("Returns an array of actors representing all ground attack units of this player.")]
		public Actor[] GetGroundAttackers()
		{
			return Player.World.ActorsWithTrait<AttackBase>().Select(a => a.Actor)
				.Where(a => a.Owner == Player && !a.IsDead && a.IsInWorld && a.Info.HasTraitInfo<MobileInfo>())
				.ToArray();
		}

		[Desc("Returns all living actors of the specified type of this player.")]
		public Actor[] GetActorsByType(string type)
		{
			var result = new List<Actor>();

			ActorInfo ai;
			if (!Context.World.Map.Rules.Actors.TryGetValue(type, out ai))
				throw new LuaException("Unknown actor type '{0}'".F(type));

			result.AddRange(Player.World.ActorMap.ActorsInWorld()
				.Where(actor => actor.Owner == Player && !actor.IsDead && actor.IsInWorld && actor.Info.Name == ai.Name));

			return result.ToArray();
		}
	}
}
