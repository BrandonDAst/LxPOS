using LxPOSModels.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;


namespace LxPOS
{
	public class DBService
	{
		public enum SettingName
		{
			Currency,
			SettingSample,
			AnotherSetting,
		}
		private readonly string _ConnectionString;

		/// <summary>
		/// Main constructor
		/// </summary>
		public DBService(string connectionString)
		{
			_ConnectionString = connectionString;
		}
		/// <summary>
		/// Returns True if the input setting is configured on DB
		/// </summary>
		/// <param name="settingName"></param>
		/// <returns></returns>
		public bool CheckIfSettingIsConfigured(SettingName settingName) => !string.IsNullOrEmpty(GetSettingValue(settingName));

		/// <summary>
		/// Returns the value of the input setting
		/// </summary>
		/// <param name="settingName"></param>
		/// <returns></returns>
		public string GetSettingValue(SettingName settingName)
		{
			string settingValue = string.Empty;

			using (LxPOSContext context = new LxPOSContext(_ConnectionString))
			{
				Settings setting = context.Settings.FirstOrDefault(s => s.Name.Equals(settingName.ToString()));
				if (setting != null) settingValue = setting.Value;
				else settingValue = "";
			}

			return settingValue;
		}
		/// <summary>
		/// Return all the products 
		/// </summary>
		/// <returns></returns>
		public List<Products> GetProducts()
		{
			List<Products> products = new List<Products>();

			using (LxPOSContext context = new LxPOSContext(_ConnectionString))
				products = context.Products.ToList();

			return products;
		}
		/// <summary>
		/// Returns a list of the Currencies on the Catalog table
		/// </summary>
		/// <returns></returns>
		public List<string> GetCurrencies()
		{
			List<string> currencies = new List<string>();

			using (LxPOSContext context = new LxPOSContext(_ConnectionString))
				foreach (var cat in context.Catalog.ToList())
					if (cat.Category == SettingName.Currency.ToString())
						if (!currencies.Contains(cat.Subcat))
							currencies.Add(cat.Subcat);

			return currencies;
		}
		/// <summary>
		/// Return a list of all Denominations of a currency from Catalog table
		/// </summary>
		/// <param name="currencyValue"></param>
		/// <returns></returns>
		public List<decimal> GetMoneyDenominations(string currencyValue)
		{
			List<decimal> denominations = new List<decimal>();

			using (LxPOSContext context = new LxPOSContext(_ConnectionString))
				foreach (var cat in context.Catalog.ToList())
					if (cat.Category == SettingName.Currency.ToString() && currencyValue == cat.Subcat)
						if (!denominations.Contains(Convert.ToDecimal(cat.Value)))
							denominations.Add(Convert.ToDecimal(cat.Value));

			return denominations;
		}
		/// <summary>
		/// Save the value of the setting to not be asked each run.
		/// </summary>
		/// <param name="name"></param>
		/// <param name="value"></param>
		public void InsertSetting(string name, string value)
		{
			using (LxPOSContext context = new LxPOSContext(_ConnectionString))
			{
				var setting = new Settings() { Name = name, Value = value };
				context.Add(setting);
				context.SaveChanges();
			}
		}
	}
}