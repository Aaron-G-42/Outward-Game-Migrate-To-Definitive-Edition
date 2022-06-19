using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;



namespace Outward_Game_Migrate_To_Definitive_Edition
{
	public partial class Form1 : Form
	{
		List<string> old_files = new List<string>();
		List<string> new_files = new List<string>();

		public Form1()
		{
			InitializeComponent();
		}



		private void button_start_Click(object sender, EventArgs e)
		{
			richTextBox1.Text = "";
			this.Refresh();

			var conversions = new Dictionary<string, string>()
			{
				{ "envc", "deenvc" },
				{ "charc", "defedc" },
				{ "legacyc", "defedc" },
				{ "mapc", "defedc" },
				{ "worldc", "defedc" },
			};

			var dirs = Directory.GetDirectories(Application.StartupPath, "*SaveGames", SearchOption.AllDirectories).ToList();
			if (dirs.Count == 0)
			{
				richTextBox1.Text = "SaveGames folder not found";
				return;
			}

			if (dirs.Count > 1)
			{
				richTextBox1.Text = "More than one SaveGames folder found. Place this application in the correct SaveGames folder.";
				return;
			}


			old_files = new List<string>();
			new_files = new List<string>();

			conversions.ToList().ForEach(x =>
			{
				var before = x.Key;
				var after = x.Value;

				var f = Directory.GetFiles(dirs[0], "*." + before, SearchOption.AllDirectories).ToList();

				old_files.AddRange(f);
				f.ForEach(y =>
				{
					var fname = y.Replace("." + before, "." + after);
					new_files.Add(fname);
				});
			});



			if (old_files.Count == 0)
			{
				richTextBox1.Text = "No old save files found!";
				return;
			}

			var sb = new StringBuilder();
			for (int i = 0; i < old_files.Count; i++)
			{
				var o = old_files[i];
				var n = new_files[i];

				sb.AppendLine(
					o.Substring(o.IndexOf("SaveGames\\") + "SaveGames\\".Length) +
					" -> " +
					Path.GetFileName(n)
					);
			}

			richTextBox1.Text = sb.ToString();
		}



		private void button_convert_Click(object sender, EventArgs e)
		{
			try
			{
				for (int i = 0; i < old_files.Count; i++)
				{
					var o = old_files[i];
					var n = new_files[i];

					File.Move(o, n);
				}

				MessageBox.Show("all done!");
			}
			catch (Exception exc)
			{
				MessageBox.Show(exc.Message);
			}
		}



	}
}
