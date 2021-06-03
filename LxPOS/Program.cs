/*
			 * In order to run this project, first create the DB.
			 * Having SQL installed, go to 'Package Manager Console' and change de Default Project to LxPOSModels, then type 
			 * PM> update-database
			 * so Entity Framework will apply the Initial migration to create the DB and populate the Products and Catalog tables. 
			 * 
			 * Settings table is not populated, so in the first run the Currency setting is asked.
			 */
namespace LxPOS
{
	class Program
	{
		static void Main(string[] args)
		{

			DBService dbService = new DBService("Server=LAPTOP-HP;Database=LxPOS;Trusted_Connection=True;"); /*This connection should be the same as the one on EF project.*/
			string currency = dbService.GetSettingValue(DBService.SettingName.Currency);
			POS point = new POS(dbService, currency);

			if (!string.IsNullOrEmpty(currency))
				point.Initialize();
			else
			{
				point.ConfigureSettings();
				point.Initialize();
			}
		}
	}
}