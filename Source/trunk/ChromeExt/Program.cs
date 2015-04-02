using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ChromeExt
{
	class Program
	{
		static string Source = "";
		static string Destination = "";

		public static void Main(string[] args)
		{
			Source = Directory.GetCurrentDirectory();
			Source = Path.GetDirectoryName(Source);
			Source = Path.GetDirectoryName(Source);
			Source = Path.Combine(Source, "src");

			Destination = Path.Combine(Directory.GetCurrentDirectory(), "ZippedExtension");

			// 1) Clean and re-create the Destination
			Console.WriteLine("Clean and create Target directory: " + Destination);
			cleanAndCreateTarget();

			// 2) Copy src\components
			Console.WriteLine("Copy components to Target");
			copy("components");

			// 3) Copy css\images
			Console.WriteLine("Copy css/images to Target");
			copy(Path.Combine("css", "images"));

			// 4) Copy font
			Console.WriteLine("Copy font to Target");
			copy("font");

			// 5) Copy images
			Console.WriteLine("Copy images to Target");
			copy("images");

			// 6) Build css
			Console.WriteLine("Builting the css with lessc");
			buildCss();

			// 7) Parse manifest.json
			Console.WriteLine("Reading a manifest file");
			JObject manifest = (JObject)JsonConvert.DeserializeObject(
				readFileContent(Path.Combine(Source, "manifest.json"))
			);

			Console.WriteLine("Coping the closure compiler");
			copyClosure();

			StringBuilder command = new StringBuilder();
			command.Append("cd builder");
			command.AppendLine();

			Console.WriteLine("Adding background builder");
			
			// 8) Add build background.js command
			addBuildBackground(manifest, command);

			// 9) Build content.js
			addBuildContent(manifest, command);

			// 10) Read index.html
			string indexContent = readFileContent(Path.Combine(Source, "index.html"));
			string newIndexContent;
			List<string> indexJs = processIndexContent(indexContent, out newIndexContent);

			// 11) Build index.js
			addBuildIndex(indexJs, command);

			// 12) Rewrite index.html
			Console.WriteLine("Creating index file");
			writeFileContent(Path.Combine(Destination, "src", "index.html"), newIndexContent);

			// 12) Rewrite manifest.json
			Console.WriteLine("Creating manifest file");
			writeFileContent(Path.Combine(Destination, "src", "manifest.json"), JsonConvert.SerializeObject(manifest));

			// 13) Creating make script
			Console.WriteLine("Creating make script");
			writeFileContent(Path.Combine(Destination, "make.bat"), command.ToString());

			// 14) Display instruction of make.bat
			Console.WriteLine("Finish");
			Console.WriteLine("Please run make.bat for building the javascript files.");
			Console.WriteLine("Press any key to exit...");
			Console.ReadKey();
		}

		protected static void cleanAndCreateTarget()
		{
			if (Directory.Exists(Destination))
				Directory.Delete(Destination, true);

			Directory.CreateDirectory(Destination);
		}

		protected static List<string> processIndexContent(string indexContent, out string newIndexContent)
		{
			List<string> result = new List<string>();
			StringBuilder content = new StringBuilder();
			string[] lines = indexContent.Split(new char[] { '\n' });
			bool isCss = false;
			bool isRemove = false;
			bool isScript = false;
			foreach (string line in lines)
			{
				bool addLine = true;
				string cmd = line.Trim();
				if (cmd == "<!-- START CSS -->")
				{
					isCss = true;
					addLine = false;
				}
				else if (cmd == "<!-- END CSS -->")
				{
					isCss = false;
					addLine = false;
				}

				if (cmd == "<!-- START REMOVE -->")
				{
					isRemove = true;
					addLine = false;
				}
				else if (cmd == "<!-- END REMOVE -->")
				{
					isRemove = false;
					addLine = false;
				}

				if (cmd == "<!-- START SCRIPT -->")
				{
					isScript = true;
					addLine = false;
				}
				else if (cmd == "<!-- END SCRIPT -->")
				{
					isScript = false;
					addLine = false;
					content.Append("<script type=\"text/javascript\" src=\"index.js\"></script>");
				}


				if (isCss && addLine)
				{
					content.Append(cmd.Replace("less", "css"));
				}
				else if (isScript && addLine)
				{
					int position = cmd.IndexOf("src=\"");
					if (position > 0)
						cmd = cmd.Substring(position + 5);
					position = cmd.IndexOf("\"");
					cmd = cmd.Substring(0, position);
					result.Add(cmd);
				}
				else if (isRemove && addLine)
				{
					continue;
				}
				else if(addLine)
				{
					content.Append(cmd);
				}
			}
			newIndexContent = content.ToString();
			return result;
		}

		protected static void addBuildIndex(List<string> array, StringBuilder command)
		{
			List<string> src = new List<string>();
			string dest = Path.Combine(Destination, "builder", "index");
			Directory.CreateDirectory(dest);

			command.Append("java -jar compiler.jar ");
			command.Append("--js_output_file=../src/index.js ");
			foreach (var item in array)
			{
				string jsFile = (string)item;
				string[] paths = jsFile.Split(new char[] { '/' });
				string path = Source;
				string name = "";
				for (int i = 0; i < paths.Length; i++)
				{
					path = Path.Combine(path, paths[i]);
					name += paths[i];
					if (i != paths.Length - 1)
						name += ".";
				}
				FileInfo fileInfo = new FileInfo(path);
				fileInfo.CopyTo(Path.Combine(dest, name));
				command.Append("index/" + name);
				command.Append(" ");
			}
			command.AppendLine();
		}

		protected static void addBuildContent(JObject manifest, StringBuilder command)
		{
			List<string> src = new List<string>();
			JArray array = (JArray)manifest["content_scripts"][0]["js"];
			string dest = Path.Combine(Destination, "builder", "content");
			Directory.CreateDirectory(dest);

			command.Append("java -jar compiler.jar ");
			command.Append("--js_output_file=../src/content.js ");
			foreach (var item in array)
			{
				string jsFile = (string)item;
				string[] paths = jsFile.Split(new char[] { '/' });
				string path = Source;
				string name = "";
				for (int i = 0; i < paths.Length; i++)
				{
					path = Path.Combine(path, paths[i]);
					name += paths[i];
					if (i != paths.Length - 1)
						name += ".";
				}
				FileInfo fileInfo = new FileInfo(path);
				fileInfo.CopyTo(Path.Combine(dest, name));
				command.Append("content/" + name);
				command.Append(" ");
			}
			command.AppendLine();

			JArray rewriteContent = new JArray();
			List<string> tmp = new List<string>();
			tmp.Add("content.js");
			rewriteContent.Add(tmp);
			manifest["content_scripts"][0]["js"] = rewriteContent;
		}

		protected static void addBuildBackground(JObject manifest, StringBuilder command)
		{
			List<string> src = new List<string>();
			JArray array = (JArray)manifest["background"]["scripts"];
			string dest = Path.Combine(Destination, "builder", "background");
			Directory.CreateDirectory(dest);

			command.Append("java -jar compiler.jar ");
			command.Append("--js_output_file=../src/background.js ");
			foreach (var item in array)
			{
				string jsFile = (string)item;
				string[] paths = jsFile.Split(new char[] { '/' });
				string path = Source;
				string name = "";
				for (int i = 0; i < paths.Length; i++)
				{
					path = Path.Combine(path, paths[i]);
					name += paths[i];
					if (i != paths.Length - 1)
						name += ".";
				}
				FileInfo fileInfo = new FileInfo(path);
				fileInfo.CopyTo(Path.Combine(dest, name));
				command.Append("background/" + name);
				command.Append(" ");
			}
			command.AppendLine();

			JArray rewriteBackground = new JArray();
			List<string> tmp = new List<string>();
			tmp.Add("background.js");
			rewriteBackground.Add(tmp);
			manifest["background"]["scripts"] = rewriteBackground;
		}

		protected static string readFileContent(string path)
		{
			FileStream stream = new FileStream(path, FileMode.Open);
			StreamReader reader = new StreamReader(stream);
			string result = reader.ReadToEnd();
			reader.Close();
			stream.Close();
			return result;
		}

		protected static void writeFileContent(string path, string content)
		{
			FileStream stream = new FileStream(path, FileMode.OpenOrCreate);
			StreamWriter writer = new StreamWriter(stream);
			writer.Write(content);
			writer.Flush();
			writer.Close();
			stream.Close();
		}

		protected static void buildCss()
		{
			string dest = Path.Combine(Destination, "src", "css");
			DirectoryInfo dir = new DirectoryInfo(Path.Combine(Source, "css"));
			FileInfo[] files = dir.GetFiles();
			foreach (var file in files)
			{
				string filename = Path.GetFileNameWithoutExtension(file.FullName);
				Console.WriteLine("  Builting " + file.Name);

				System.Diagnostics.Process process = new System.Diagnostics.Process();
				System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
				startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Maximized;
				startInfo.FileName = "lessc";
				startInfo.Arguments = "-x " + file.FullName + " " + Path.Combine(dest, filename + ".css");
				process.StartInfo = startInfo;
				process.Start();
			}
		}

		protected static void copy(string folder)
		{
			directoryCopy(Path.Combine(Source, folder), Path.Combine(Destination, "src", folder), true);
		}

		protected static void copyClosure()
		{
			directoryCopy(
				Path.Combine(Directory.GetCurrentDirectory(), "closure"),
				Path.Combine(Destination, "builder"),
				true
			);
		}

		protected static void directoryCopy(string sourceDirName, string destDirName, bool copySubDirs)
		{
			// Get the subdirectories for the specified directory.
			DirectoryInfo dir = new DirectoryInfo(sourceDirName);
			DirectoryInfo[] dirs = dir.GetDirectories();

			if (!dir.Exists)
			{
				throw new DirectoryNotFoundException(
						"Source directory does not exist or could not be found: "
						+ sourceDirName);
			}

			// If the destination directory doesn't exist, create it. 
			if (!Directory.Exists(destDirName))
			{
				Directory.CreateDirectory(destDirName);
			}

			// Get the files in the directory and copy them to the new location.
			FileInfo[] files = dir.GetFiles();
			foreach (FileInfo file in files)
			{
				string temppath = Path.Combine(destDirName, file.Name);
				file.CopyTo(temppath, false);
				Console.WriteLine("  " + temppath);
			}

			// If copying subdirectories, copy them and their contents to new location. 
			if (copySubDirs)
			{
				foreach (DirectoryInfo subdir in dirs)
				{
					string temppath = Path.Combine(destDirName, subdir.Name);
					directoryCopy(subdir.FullName, temppath, copySubDirs);
				}
			}
		}
	}
}
