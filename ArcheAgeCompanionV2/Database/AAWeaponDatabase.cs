﻿using System;
using System.Collections.Generic;
using System.Linq;
using SQLite;
using System.IO;


namespace ArcheAgeCompanionV2
{
	public class AAWeaponDatabase 
	{
		static object locker = new object ();

		SQLiteConnection database;


		string DatabasePath {
			get { 
				var sqliteFilename = "archeageDB.db";
				#if __IOS__
				string documentsPath = Environment.GetFolderPath (Environment.SpecialFolder.Personal); // Documents folder
				string libraryPath = Path.Combine (documentsPath, "..", "Library"); // Library folder
				var path = Path.Combine(libraryPath, sqliteFilename);
				#else
				#if __ANDROID__
				string documentsPath = Environment.GetFolderPath (Environment.SpecialFolder.Personal); // Documents folder
				var path = Path.Combine(documentsPath, sqliteFilename);
				#else
				// WinPhone
				var path = Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, sqliteFilename);;
				#endif
				#endif
				return path;
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Tasky.DL.TaskDatabase"/> TaskDatabase. 
		/// if the database doesn't exist, it will create the database and all the tables.
		/// </summary>
		/// <param name='path'>
		/// Path.
		/// </param>
		public AAWeaponDatabase()
		{
			database = new SQLiteConnection (DatabasePath);
			database.CreateTable<Weapon> ();
			List<SQLiteConnection.ColumnInfo> s = database.GetTableInfo ("weaponTable");
			database.BeginTransaction ();

	
			System.Diagnostics.Debug.WriteLine(database.Table<Weapon> ().Count().ToString());
		}

		public IEnumerable<Weapon> GetItems ()
		{
			lock (locker) {
				return (from i in database.Table<Weapon>() select i).ToList();
			}
		}

		public List<Weapon> GetSpecificItems (string weaponType, string weaponSubType)
		{
			lock (locker) {
				String q = "SELECT * FROM [weaponTable] WHERE type = '"+ weaponType+"' and subType = '" + weaponSubType + "'";
				return database.Query<Weapon>(q);
			}
		}
			

		public Weapon GetItem (int id) 
		{
			lock (locker) {
				return database.Table<Weapon>().FirstOrDefault(x => x.ID == id);
			}
		}
			
	}
}

