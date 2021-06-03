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
		}
		private readonly string _ConnectionString = "Server=LAPTOP-HP;Database=LxPOS;Trusted_Connection=True;";

		public DBService()
		{

		}

		public bool CheckIfSettingIsConfigured(SettingName settingName) => !string.IsNullOrEmpty(GetSettingValue(settingName));



		public string GetSettingValue(SettingName settingName)
		{
			string settingValue = string.Empty;

			using (LxPOSContext context = new LxPOSContext(_ConnectionString))
			{
				Settings setting = context.Settings.FirstOrDefault(s => s.Name.Equals(settingName.ToString()));
				if (setting != null)
					settingValue = setting.Value;
				else
					settingValue = "";
			}

			return settingValue;
		}
		public List<Products> GetProducts()
		{
			List<Products> products = new List<Products>();

			using(LxPOSContext context = new LxPOSContext(_ConnectionString))
			{
				products = context.Products.ToList();
			}

			return products;
		}

		public List<string> GetCurrencies()
		{
			List<string> currencies = new List<string>();

			using(LxPOSContext context = new LxPOSContext(_ConnectionString))
			{

				foreach (var cat in context.Catalog.ToList())
				{
					if(cat.Category == SettingName.Currency.ToString())
					{
						if (!currencies.Contains(cat.Subcat)) currencies.Add(cat.Subcat);
					}
				}
			}

			return currencies;
		}
		public List<decimal> GetMoneyDenominations(string currencyValue)
		{
			List<decimal> denominations = new List<decimal>();

			using (LxPOSContext context = new LxPOSContext(_ConnectionString))
			{

				foreach (var cat in context.Catalog.ToList())
				{
					if (cat.Category == SettingName.Currency.ToString() && currencyValue == cat.Subcat )
					{
						if (!denominations.Contains(Convert.ToDecimal(cat.Value))) denominations.Add(Convert.ToDecimal(cat.Value));
					}
				}
			}

			return denominations;
		}

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
