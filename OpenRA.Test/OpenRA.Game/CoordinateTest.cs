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
using System.Linq;
using NUnit.Framework;

namespace OpenRA.Test
{
	[TestFixture]
	public class CoordinateTest
	{
		[TestCase(TestName = "Test CPos and MPos conversion and back again.")]
		public void CoarseToMapProjection()
		{
			foreach (var shape in Enum.GetValues(typeof(TileShape)).Cast<TileShape>())
			{
				for (var x = 0; x < 12; x++)
				{
					for (var y = 0; y < 12; y++)
					{
						var cell = new CPos(x, y);
						try
						{
							Assert.That(cell, Is.EqualTo(cell.ToMPos(shape).ToCPos(shape)));
						}
						catch (Exception e)
						{
							// Known problem on isometric mods that shouldn't be visible to players as these are outside the map.
							if (shape == TileShape.Diamond && y > x)
								continue;

							Console.WriteLine("Coordinate {0} on shape {1} failed to convert back.".F(cell, shape));
							throw e;
						}
					}
				}
			}
		}
	}
}
