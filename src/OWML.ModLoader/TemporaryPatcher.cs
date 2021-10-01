﻿using HarmonyLib;
using OWML.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

namespace OWML.ModLoader
{
	public static class Patches
	{
		public static void PermanantManager_Awake_Postfix()
		{
			ModLoader.LoadMods();
		}
	}

	public static class TemporaryPatcher
	{
		static Harmony _harmony;

		public static void TempPatch()
		{
			using (TextWriter tw = File.CreateText("TempPatch.txt"))
			{
				tw.WriteLine($"Hello! This text file was generated by Doorstop on {DateTime.Now:R}!");
				tw.WriteLine($"Command line: {Environment.CommandLine}");
				tw.Flush();
			}
			/*
			var owAssemblyDef = destination::Mono.Cecil.AssemblyDefinition.ReadAssembly(@"OuterWilds_Data\Managed\Assembly-CSharp.dll");
			var modLoaderAssemblyDef = destination::Mono.Cecil.AssemblyDefinition.ReadAssembly(@"OuterWilds_Data\Managed\OWML.ModLoader.dll");

			var permanantManagerType = owAssemblyDef.MainModule.Types.First(x => x.Name == "PermanentManager");
			var awakeMethod = permanantManagerType.Methods.First(x => x.Name == "Awake");
			var ilProcessor = awakeMethod.Body.GetILProcessor();
			var firstInstruction = ilProcessor.Body.Instructions.First();

			var modLoaderType = modLoaderAssemblyDef.MainModule.Types.First(x => x.Name == "ModLoader");
			var loadModsMethod = modLoaderType.Methods.First(x => x.Name == "LoadMods");

			//var methodReference = new destination::Mono.Cecil.MethodReference("LoadMods", awakeMethod.ReturnType, permanantManagerType);
			var methodReference = loadModsMethod;
			ilProcessor.InsertBefore(firstInstruction, ilProcessor.Create(destination::Mono.Cecil.Cil.OpCodes.Call, methodReference));
			*/

			_harmony = new Harmony("Alek.OWML.PrePatch");

			AddPostfix<PermanentManager>("Awake", typeof(Patches), nameof(Patches.PermanantManager_Awake_Postfix));
		}

		public static void AddPostfix<T>(string methodName, Type patchType, string patchMethodName) =>
			AddPostfix(GetMethod<T>(methodName), patchType, patchMethodName);

		public static void AddPostfix(MethodBase original, Type patchType, string patchMethodName)
		{
			using (TextWriter tw = File.CreateText("AddPostfix.txt"))
			{
				tw.WriteLine($"Hello! This text file was generated by Doorstop on {DateTime.Now:R}!");
				tw.WriteLine($"Command line: {Environment.CommandLine}");
				tw.Flush();
			}

			var postfix = Utils.TypeExtensions.GetAnyMethod(patchType, patchMethodName);
			if (postfix == null)
			{
				using (TextWriter tw = File.CreateText("MethodIsNull.txt"))
				{
					tw.WriteLine($"Hello! This text file was generated by Doorstop on {DateTime.Now:R}!");
					tw.WriteLine($"Command line: {Environment.CommandLine}");
					tw.Flush();
				}
				//_console.WriteLine($"Error in {nameof(AddPostfix)}: {patchType.Name}.{patchMethodName} is null.", MessageType.Error);
				return;
			}
			Patch(original, null, postfix, null);
		}

		private static void Patch(MethodBase original, MethodInfo prefix, MethodInfo postfix, MethodInfo transpiler)
		{
			using (TextWriter tw = File.CreateText("Patch.txt"))
			{
				tw.WriteLine($"Hello! This text file was generated by Doorstop on {DateTime.Now:R}!");
				tw.WriteLine($"Command line: {Environment.CommandLine}");
				tw.Flush();
			}
			if (original == null)
			{
				using (TextWriter tw = File.CreateText("Patch_ORIGINAL_NULL.txt"))
				{
					tw.WriteLine($"Hello! This text file was generated by Doorstop on {DateTime.Now:R}!");
					tw.WriteLine($"Command line: {Environment.CommandLine}");
					tw.Flush();
				}
				//_console.WriteLine($"Error in {nameof(Patch)}: original MethodInfo is null.", MessageType.Error);
				return;
			}
			var prefixMethod = prefix == null ? null : new HarmonyMethod(prefix);
			var postfixMethod = postfix == null ? null : new HarmonyMethod(postfix);
			var transpilerMethod = transpiler == null ? null : new HarmonyMethod(transpiler);
			var fullName = $"{original.DeclaringType}.{original.Name}";
try
{
				_harmony.Patch(original, prefixMethod, postfixMethod, transpilerMethod);
				//_console.WriteLine($"Patched {fullName}!", MessageType.Debug);
			}
			catch (Exception ex)
			{
				using (TextWriter tw = File.CreateText($"Patch-EXCEPTION-{fullName}.txt"))
				{
					tw.WriteLine(ex.ToString());
					tw.Flush();
				}
				//_console.WriteLine($"Exception while patching {fullName}: {ex}", MessageType.Error);
			}
		}

		private static MethodInfo GetMethod<T>(string methodName)
		{
			using (TextWriter tw = File.CreateText("GetMethod.txt"))
			{
				tw.WriteLine($"Hello! This text file was generated by Doorstop on {DateTime.Now:R}!");
				tw.WriteLine($"Command line: {Environment.CommandLine}");
				tw.Flush();
			}

			var fullName = $"{typeof(T).Name}.{methodName}";
			try
			{
				//_console.WriteLine($"Getting method {fullName}", MessageType.Debug);
				var result = Utils.TypeExtensions.GetAnyMethod(typeof(T), methodName);
				if (result == null)
				{
					using (TextWriter tw = File.CreateText("GetMethod_METHOD_NOT_FOUND.txt"))
					{
						tw.WriteLine($"Hello! This text file was generated by Doorstop on {DateTime.Now:R}!");
						tw.WriteLine($"Command line: {Environment.CommandLine}");
						tw.Flush();
					}
					//_console.WriteLine($"Error - method {fullName} not found.", MessageType.Error);
				}
				return result;
			}
			catch (Exception ex)
			{
				using (TextWriter tw = File.CreateText("GetMethod_EXCEPTION.txt"))
				{
					tw.WriteLine($"Hello! This text file was generated by Doorstop on {DateTime.Now:R}!");
					tw.WriteLine($"Command line: {Environment.CommandLine}");
					tw.Flush();
				}
				//_console.WriteLine($"Exception while getting method {fullName}: {ex}", MessageType.Error);
				return null;
			}
		}
	}
}
