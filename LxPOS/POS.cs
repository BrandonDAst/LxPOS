using LxPOSModels.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace LxPOS
{
	public class POS
	{
		private DBService _dBService;
		private string _currency;
		private int _productId;
		private List<decimal> _denominations;


		public POS( DBService dBService, string currency)
		{
			_currency = currency;
			_dBService = dBService;
		}

		public void Initialize()
		{
			ShowHeader();
			ShowProducts();
		}

		public void ConfigureSettings()
		{
			ShowHeader();
			ShowSettings();
		}

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
		private void GetChange(decimal price, decimal ammount)
		{
			decimal change = ammount - price;
			
			foreach (var item in _denominations)
			{
				if (item > change) continue;

				int coinBillCount = 0;

				while(coinBillCount * item <= change)
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
			Console.WriteLine("CASH Masters");
			PrintStrongLine();
		}
		private void ShowProducts()
		{
			Print(); 

			Print("PRODUCTS");
			PrintLine();

			var products = _dBService.GetProducts();
			if (products.Count < 1) Print("No products found.");
			else
				foreach (var product in products)
					Print($"{product.Id}\t{product.Name}\r\t\t\t\t"+ FormatMoney(product.Price));

			PrintLine();
			AskProduct(products);
			ShowProductPrice(products.Find(p => p.Id == _productId));
		}
		private void AskProduct(List<Products> products)
		{
			Print("Select a Product ID");
			_productId = Convert.ToInt32(Console.ReadLine());
			if (!(_productId < products.Count && _productId > 0))
			{
				Print("Invalid ID.");
				AskProduct(products);
			}
		}
		private void ShowProductPrice(Products product)
		{
			ShowHeader();
			Print();
			Print($"The product costs " + FormatMoney(product.Price));

			Print("Do you want to proceed to payment?.");
			var resp = Console.ReadLine().ToLower();

			if (resp == "yes" || resp == "y") Pay(product);
			
			Initialize();
		}
		

		private void ShowSettings()
		{
			Print(); 

			Print("SET THE CURRENCY");
			PrintLine();

			var currencies = _dBService.GetCurrencies();
			if (currencies.Count < 1) Print("No currencies found.");
			else
				foreach (var currency in currencies)
					Print($"{currency}\r");

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
