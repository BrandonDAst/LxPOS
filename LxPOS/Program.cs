namespace LxPOS
{
	class Program
	{
		static void Main(string[] args)
		{
			DBService db = new DBService();
			string currency = db.GetSettingValue(DBService.SettingName.Currency);
			POS point = new POS(db, currency);

			if (!string.IsNullOrEmpty(currency))
			{
				point.Initialize();
			}
			else
			{
				point.ConfigureSettings();
				point.Initialize();
			}
		}
	}
}
