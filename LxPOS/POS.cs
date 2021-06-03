using LxPOSModels.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace LxPOS
{
	/// <summary>
	/// This is the POS class that manages the interaction of the user
	/// </summary>
	public class POS
	{
		private DBService _dBService;
		private string _currency;
		private int _productId;
		private List<decimal> _denominations;

		/// <summary>
		/// Create a Point of Service instance, sending the DB connection and the Settings
		/// </summary>
		/// <param name="dBService">The connection to the DB.</param>
		/// <param name="currency"></param>
		public POS(DBService dBService, string currency)
		{
			_currency = currency;
			_dBService = dBService;
		}

		/// <summary>
		/// Start the POS
		/// </summary>
		public void Initialize()
		{
			ShowHeader();
			ShowProducts();
		}

		/// <summary>
		/// Print the unconfigured settings
		/// </summary>
		public void ConfigureSettings()
		{
			ShowHeader();
			ShowSettings();
		}
		/// <summary>
		/// Iterative method that ask the client for each coin and bill available according to Currency setting
		/// </summary>
		/// <param name="product"></param>
		private void Pay(Products product)
		{
			_denominations = _dBService.GetMoneyDenominations(_currency);
			_denominations.Sort();
			_denominations.Reverse();

			decimal ammount = 0;

			foreach (var item in _denominations)
			{
				ShowHeader();
				Print($"Insert the coins and bills to pay this {product.Name} ({FormatMoney(product.Price)})");

				Print(FormatMoney(ammount));

				if (ammount >= product.Price)
				{
					PrintLine();
					Print("You've inserted enough money. Proceeding to give change.");

					PrintLine();
					Print(FormatMoney(ammount - product.Price));
					GetChange(product.Price, ammount);
					break;
				}

				PrintLine();
				Print(FormatMoney(item));

				int count = Convert.ToInt32(Console.ReadLine());
				ammount += Convert.ToDecimal(item * count);
			}
			if (ammount < product.Price)
				Print($"Not enough money to pay this product. Returning to main menu.");
			Console.ReadLine();
		}
		/// <summary>
		/// Once the client input enough money to pay the product, return as few coins and bills as posible.
		/// </summary>
		/// <param name="price"></param>
		/// <param name="ammount"></param>
		private void GetChange(decimal price, decimal ammount)
		{
			decimal change = ammount - price;

			foreach (var item in _denominations)
			{
				if (item > change) continue;

				int coinBillCount = 0;

				while (coinBillCount * item <= change)
					coinBillCount++;
				coinBillCount--;

				if (coinBillCount >= 1)
				{
					change -= coinBillCount * item;
					Print($"{coinBillCount} x {FormatMoney(item)}");
				}
			}
		}

		#region UI
		private string FormatMoney(decimal price) => $"${price} {_currency}";
		private void PrintStrongLine() => Console.Write("=========================\n");
		private void PrintLine() => Console.Write("-------------------------\n");
		private void Print(string message = "") => Console.Write(message + "\n");

		private void ShowHeader()
		{
			Console.Clear();
			PrintStrongLine();
			Console.WriteLine("C A S H - M A S T E R S");
			PrintStrongLine();
		}
		/// <summary>
		/// Retrieve the products from de DB and prints each and ask the client for one.
		/// </summary>
		private void ShowProducts()
		{
			Print();

			Print("P R O D U C T S");
			PrintLine();

			var products = _dBService.GetProducts();
			if (products.Count < 1 || products == null) Print("No products found.");
			else
				foreach (var product in products)
					Print($"{product.Id}\t{product.Name}\r\t\t\t\t" + FormatMoney(product.Price));

			PrintLine();
			AskProduct(products);
			ShowProductPrice(products.Find(p => p.Id == _productId));
		}

		private void AskProduct(List<Products> products)
		{
			Print("Select a Product ID");


			if (
				!(int.TryParse(Console.ReadLine(), out _productId))
				||
				!(_productId > 0 && _productId <= products.Count)
				)
			{
				Print("Invalid ID.");
				AskProduct(products);
			}
		}
		private void ShowProductPrice(Products product)
		{
			ShowHeader();
			Print();
			Print($"This {product.Name} costs " + FormatMoney(product.Price));

			Print("Do you want to proceed to payment?.");
			var resp = Console.ReadLine().ToLower();

			if (resp == "yes" || resp == "y") Pay(product);

			Initialize();
		}

		/// <summary>
		/// Retrieve the Currency setting to configure
		/// </summary>
		private void ShowSettings()
		{
			Print();

			Print("SET THE CURRENCY");
			PrintLine();

			var currencies = _dBService.GetCurrencies();
			if (currencies.Count < 1) 
				Print("No currencies found.");
			else
				foreach (var currency in currencies)
					Print($"{currency}");

			PrintLine();
			AskCurrency(currencies);
		}
		private void AskCurrency(List<string> currencies)
		{
			Print("Enter the currency code.");
			var c = Console.ReadLine().ToUpper();
			if (currencies.Contains(c))
			{
				_currency = c;
				_dBService.InsertSetting(DBService.SettingName.Currency.ToString(), _currency);
			}
			else
			{
				Print("Invalid code.");
				AskCurrency(currencies);
			}
		}
		#endregion
	}
}
