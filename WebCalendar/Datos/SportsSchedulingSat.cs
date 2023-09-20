//====================================================================================================
//The Free Edition of C++ to C# Converter limits conversion output to 100 lines per snippet.

//To purchase the Premium Edition, visit our website:
//https://www.tangiblesoftwaresolutions.com/order/order-cplus-to-csharp.html
//====================================================================================================

using Google.OrTools.Sat;
using Google.OrTools.Util;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using WebCalendar.Datos;
using WebCalendar.Models;

// Copyright 2010-2021 Google LLC
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

// Sports scheduling problem.
//
// We want to solve the problem of scheduling of team matches in a
// double round robin tournament.  Given a number of teams, we want
// each team to encounter all other teams, twice, once at home, and
// once away. Furthermore, you cannot meet the same team twice in the
// same half-season.
//
// Finally, there are constraints on the sequence of home or aways:
//  - You cannot have 3 consecutive homes or three consecutive aways.
//  - A break is a sequence of two homes or two aways, the overall objective
//    of the optimization problem is to minimize the total number of breaks.
//  - If team A meets team B, the reverse match cannot happen less that 6 weeks
//    after.
//
// In the opponent model, we use three matrices of variables, each with
// num_teams rows and 2*(num_teams - 1) columns: the var at position [i][j]
// corresponds to the match of team #i at day #j. There are
// 2*(num_teams - 1) columns because each team meets num_teams - 1
// opponents twice.
//
// - The 'opponent' var [i][j] is the index of the opposing team.
// - The 'home_away' var [i][j] is a boolean: 1 for 'playing away',
//   0 for 'playing at home'.
// - The 'signed_opponent' var [i][j] is the 'opponent' var [i][j] +
//   num_teams * the 'home_away' var [i][j].
//
// In the fixture model, we have a cube of Boolean variables fixtures.
//   fixtures[d][i][j] is true if team i plays team j at home on day d.
// We also introduces a variable at_home[d][i] which is true if team i
// plays any opponent at home on day d.



namespace SportsSchedulingSat
{
	public static class Globals
	{
		/// <summary>
		/// Calcula una solución de calendario y la devuelve si existe como una lista
		/// </summary>
		/// <param name="num_teams">Número de equipos en la competición</param>
		/// <param name="max_time_in_seconds">Segundos que pasará buscando una solución</param>
		/// <param name="journeys_repeat_match">Número de jornadas que tienen que pasar para repetir un enfrentamiento</param>
		/// <param name="num_journeys_home_away">Número de jornadas consecutivas que puede jugar un equipo como local o visitante</param>
		/// <param name="home_away_alternation">Indica si todos los equipos deben alternarse como local y visitante al final de cada vuelta</param>
		/// <param name="listaRestricciones">Lista de restricciones de los equipos en las jornadas que debe cumplir el calendario para ser válido</param>
		/// <returns></returns>
		public static IList<PartidosCalendario> OpponentModel(int num_teams, int max_time_in_seconds, int journeys_repeat_match, 
			int num_journeys_home_away, bool home_away_alternation, IList<RestriccionesCalendario> listaRestricciones)
		{
			int num_days = 2 * num_teams - 2;
			int kNoRematch = journeys_repeat_match;

			CpModel builder = new CpModel();

			// Calendar variables.
			List<List<IntVar>> opponents = new List<List<IntVar>>(Arrays.InitializeWithDefaultInstances<List<IntVar>>(num_teams));
			List<List<BoolVar>> home_aways = new List<List<BoolVar>>(Arrays.InitializeWithDefaultInstances<List<BoolVar>>(num_teams));
			List<List<IntVar>> signed_opponents = new List<List<IntVar>>(Arrays.InitializeWithDefaultInstances<List<IntVar>>(num_teams));

			for (int t = 0; t < num_teams; ++t)
			{
				for (int d = 0; d < num_days; ++d)
				{
					Domain opponent_domain = new Domain(0, num_teams - 1);
					Domain signed_opponent_domain = new Domain(0, 2 * num_teams - 1);
					IntVar opp = builder.NewIntVarFromDomain(opponent_domain, $"opponent_{t}_{d}");
					BoolVar home = builder.NewBoolVar($"home_aways{t}_{d}");
					IntVar signed_opp = builder.NewIntVarFromDomain(signed_opponent_domain, $"signed_opponent_{t}_{d}");

					opponents[t].Add(opp);
					home_aways[t].Add(home);
					signed_opponents[t].Add(signed_opp);

					// One team cannot meet itself.
					builder.Add(opp != t);
					builder.Add(signed_opp != t);
					builder.Add(signed_opp != t + num_teams);

					// Link opponent, home_away, and signed_opponent.
					builder.Add(opp == signed_opp).OnlyEnforceIf(home.Not());
					builder.Add(opp + num_teams == signed_opp).OnlyEnforceIf(home);
				}
			}

			// One day constraints.
			for (int d = 0; d < num_days; ++d)
			{
				List<IntVar> day_opponents = new List<IntVar>();
				List<IntVar> day_home_aways = new List<IntVar>();
				for (int t = 0; t < num_teams; ++t)
				{
					day_opponents.Add(opponents[t][d]);
					day_home_aways.Add(home_aways[t][d]);
				}

				builder.AddInverse(day_opponents, day_opponents);

				for (int first_team = 0; first_team < num_teams; ++first_team)
				{
					IntVar first_home = day_home_aways[first_team];
					IntVar second_home = builder.NewBoolVar("");
					builder.AddElement(day_opponents[first_team], day_home_aways, second_home);
					builder.Add(first_home + second_home == 1);
				}

				builder.Add(LinearExpr.Sum(day_home_aways) == num_teams / 2);
			}

			// One team constraints.
			for (int t = 0; t < num_teams; ++t)
			{
				builder.AddAllDifferent(signed_opponents[t]);
				List<IntVar> first_part = new List<IntVar>(opponents[t].GetRange(0, num_teams - 1));
				builder.AddAllDifferent(first_part);
				List<IntVar> second_part = new List<IntVar>(opponents[t].GetRange(num_teams - 1, ((opponents[t].Count) - (num_teams -1))));

				builder.AddAllDifferent(second_part);

				for (int day = num_teams - kNoRematch; day < num_teams - 1; ++day)
				{
					List<IntVar> moving = new List<IntVar>(opponents[t].GetRange(day, kNoRematch));
					builder.AddAllDifferent(moving);
				}

				builder.Add(LinearExpr.Sum(home_aways[t]) == num_teams - 1);

				// Forbid sequence of 3 homes or 3 aways.
				// Modificamos para que la secuencia sea la que llega por parametro
				//for (int start = 0; start < num_days - 2; ++start)
				for (int start = 0; start < num_days - (num_journeys_home_away - 1); ++start)
				{
					builder.AddBoolOr(new List<ILiteral>() { home_aways[t][start], home_aways[t][start + 1], home_aways[t][start + 2] });
					builder.AddBoolOr(new List<ILiteral>() { home_aways[t][start].Not(), home_aways[t][start + 1].Not(), home_aways[t][start + 2].Not() });
				}

				
				// Comprueba si los equipos tienen que alternarse como local y visitante al final de cada vuelta para aplicar la restricción
				if (home_away_alternation)
				{
                    // Force home/away alternation on last 2 rounds of first half-season
                    for (int start = num_days / 2 - 2; start < num_days / 2 - 1; ++start)
                    {
                        builder.AddBoolOr(new List<ILiteral>() { home_aways[t][start], home_aways[t][start + 1] });
                        builder.AddBoolOr(new List<ILiteral>() { home_aways[t][start].Not(), home_aways[t][start + 1].Not() });
                    }

                    // Force home/away alternation on last 2 rounds of second half-season
                    for (int start = num_days - 2; start < num_days - 1; ++start)
                    {
                        builder.AddBoolOr(new List<ILiteral>() { home_aways[t][start], home_aways[t][start + 1] });
                        builder.AddBoolOr(new List<ILiteral>() { home_aways[t][start].Not(), home_aways[t][start + 1].Not() });
                    }
                }
				

			}

			//Specific Team/Round Constraints

			//Example1: Team 10 must play Round 7 at Home
			//builder.AddBoolOr(new List<ILiteral>() { home_aways[9][6] });

			//Fixed Match: Team 3 and 8 must play each other on Round 4
			//builder.Add(opponents[2][3] == 7);

			//Fixed Match: Team 3 and 9 must play each other on Round 4
			//builder.Add(opponents[2][3] == 8);

			foreach (RestriccionesCalendario restriccion in listaRestricciones)
			{
				switch (restriccion.TipoRestriccion)
				{
					//Equipo local en jornada
					case 1: 
						builder.AddBoolOr(new List<ILiteral>() { home_aways[restriccion.Equipo][restriccion.Jornada] });
						break;
					//Equipo visitante en jornada
					case 2: 
						builder.AddBoolOr(new List<ILiteral>() { home_aways[restriccion.Equipo][restriccion.Jornada].Not() });
						break;
					//Equipos se tienen que enfrentar en jornada
					case 3:
						builder.Add(opponents[restriccion.Equipo][restriccion.Jornada] == restriccion.EquipoRival);
						break;
					//Equipos NO se tienen que enfrentar en jornada
					case 4:
						builder.Add(opponents[restriccion.Equipo][restriccion.Jornada] != restriccion.EquipoRival);
						break;
					default:
						break;
				}
			}


			// Objective.
			List<BoolVar> breaks = new List<BoolVar>();
			for (int t = 0; t < num_teams; ++t)
			{
				for (int d = 0; d < num_days - 1; ++d)
				{
					BoolVar break_var = builder.NewBoolVar($"break_{t}_{d}");
					builder.AddBoolOr(new List<ILiteral>() { home_aways[t][d].Not(), home_aways[t][d + 1].Not(), break_var });
					builder.AddBoolOr(new List<ILiteral>() { home_aways[t][d], home_aways[t][d + 1], break_var });
					breaks.Add(break_var);
				}
			}

			builder.Minimize(LinearExpr.Sum(breaks));

			// Solve model
			var solver = new CpSolver();
			solver.StringParameters = "log_search_progress:true,max_time_in_seconds:" + max_time_in_seconds;

			var status = solver.Solve(builder);

            IList<PartidosCalendario> listaPartidosCalendario = new List<PartidosCalendario>();
            // Print solution
            if (status == CpSolverStatus.Optimal || status == CpSolverStatus.Feasible)
			{
				// Recorre las jornadas
				for (int d = 0; d < num_days; ++d)
				{
					//Console.WriteLine();
					//Console.WriteLine("JORNADA " + (d + 1));

					// Recorre los equipos
					for (int t = 0; t < num_teams; ++t)
					{
						int opponent = (int)solver.Value(opponents[t][d]);
						bool home = solver.BooleanValue(home_aways[t][d]);
						if (home)
						{
							//Console.WriteLine((t+1) + " - " + (opponent+1));
							PartidosCalendario partido = new PartidosCalendario(d,t,opponent);
							listaPartidosCalendario.Add(partido);
						}
					}

					//Console.WriteLine("---------------------");
				}


				//Console.WriteLine();
				//Console.WriteLine();
				//Console.WriteLine("Statistics");
				//Console.WriteLine($"  - status          : {status}");
				//Console.WriteLine($"  - conflicts       : {solver.NumConflicts()}");
				//Console.WriteLine($"  - branches        : {solver.NumBranches()}");
				//Console.WriteLine($"  - wall time       : {solver.WallTime()}");
			}

			return listaPartidosCalendario;
		}

		internal static class Arrays
		{
			public static T[] InitializeWithDefaultInstances<T>(int length) where T : class, new()
			{
				T[] array = new T[length];
				for (int i = 0; i < length; i++)
				{
					array[i] = new T();
				}
				return array;
			}

			public static string[] InitializeStringArrayWithDefaultInstances(int length)
			{
				string[] array = new string[length];
				for (int i = 0; i < length; i++)
				{
					array[i] = "";
				}
				return array;
			}

			public static T[] PadWithNull<T>(int length, T[] existingItems) where T : class
			{
				if (length > existingItems.Length)
				{
					T[] array = new T[length];

					for (int i = 0; i < existingItems.Length; i++)
					{
						array[i] = existingItems[i];
					}

					return array;
				}
				else
					return existingItems;
			}

			public static T[] PadValueTypeArrayWithDefaultInstances<T>(int length, T[] existingItems) where T : struct
			{
				if (length > existingItems.Length)
				{
					T[] array = new T[length];

					for (int i = 0; i < existingItems.Length; i++)
					{
						array[i] = existingItems[i];
					}

					return array;
				}
				else
					return existingItems;
			}

			public static T[] PadReferenceTypeArrayWithDefaultInstances<T>(int length, T[] existingItems) where T : class, new()
			{
				if (length > existingItems.Length)
				{
					T[] array = new T[length];

					for (int i = 0; i < existingItems.Length; i++)
					{
						array[i] = existingItems[i];
					}

					for (int i = existingItems.Length; i < length; i++)
					{
						array[i] = new T();
					}

					return array;
				}
				else
					return existingItems;
			}

			public static string[] PadStringArrayWithDefaultInstances(int length, string[] existingItems)
			{
				if (length > existingItems.Length)
				{
					string[] array = new string[length];

					for (int i = 0; i < existingItems.Length; i++)
					{
						array[i] = existingItems[i];
					}

					for (int i = existingItems.Length; i < length; i++)
					{
						array[i] = "";
					}

					return array;
				}
				else
					return existingItems;
			}

			public static void DeleteArray<T>(T[] array) where T : System.IDisposable
			{
				foreach (T element in array)
				{
					if (element != null)
						element.Dispose();
				}
			}
		}
	}

}