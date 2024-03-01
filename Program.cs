using System;
using System.Collections.Generic;

namespace Shop
{
	internal class Program
	{
		static void Main()
		{
			Supermarket supermarket = new Supermarket();
			supermarket.Work();
		}
	}
}

class Supermarket
{
	private Queue<Client> _clients;
	private List<Product> _products;
	public Supermarket()
	{
		FillProducts();
		CreateQueueAtCash();
	}

	public void Work()
	{
		while (_clients.Count > 0)
		{
			Client client = _clients.Dequeue();

			Console.WriteLine($"Клиент подошёл на кассу. Он хочет купить {client.CountProductPurchase} товаров.");
			client.ShowMoney();
			WaitForKeypress();

			for (int i = 0; i < client.CountProductPurchase; i++)
			{
				Product product = _products[UserUtils.GenerateRandomNumber(0, _products.Count)];
				client.FillFoodBasket(product);
			}
			WaitForKeypress();

			int moneyToPay = client.CalculateCostFoodBasket();

			while (client.CanBuy(moneyToPay) == true)
			{
				Console.WriteLine("Клиенту не хватило денег");
				client.RemoveProductFromFoodBasket();
				WaitForKeypress();
				moneyToPay = client.CalculateCostFoodBasket();
				WaitForKeypress();
			}

			Console.WriteLine($"Покупатель купил товаров на сумму {moneyToPay}");

			WaitForKeypress();
			Console.Clear();
		}
		Console.WriteLine("Очередь закончилась!");
		WaitForKeypress();
	}

	private void FillProducts()
	{
		_products = new List<Product>();
		int minimumProducts = 1;
		int maximumProducts = 100;
		int minimumPrice = 40;
		int maximumPrice = 500;
		int countProducts = UserUtils.GenerateRandomNumber(minimumProducts, maximumProducts);

		for (int i = 0; i < countProducts; i++)
			_products.Add(new Product((ProductType)i, UserUtils.GenerateRandomNumber(minimumPrice, maximumPrice + 1)));
	}

	private void CreateQueueAtCash()
	{
		_clients = new Queue<Client>();
		int minimumClients = 2;
		int maximumClients = 100;
		int minimumMoney = 100;
		int maximumMoney = 10000;

		int buyerCount = UserUtils.GenerateRandomNumber(minimumClients, maximumClients + 1);

		for (int i = 0; i < buyerCount; i++)
			_clients.Enqueue(new Client(UserUtils.GenerateRandomNumber(minimumMoney, maximumMoney + 1),
				UserUtils.GenerateRandomNumber(1, _products.Count)));
	}

	private void WaitForKeypress()
	{
		Console.WriteLine();
		Console.Write("Нажмите любую кнопку чтобы продолжить: ");
		Console.ReadKey();
		Console.WriteLine();
	}
}

enum ProductType
{
	Milk,
	Eggs,
	SourCream,
	Butter,
	Kefir,
	Ryazhenka,
	Curd,
	CurdCasserole,
	Cream,
	ThickYoghurt,
	DrinkingYoghurt,
	MilkShake,
	CurdCheese,
	Cheese,
	Baton,
	Bread,
	Lavash,
	Breadsticks,
	Dryers,
	Breadcrumbs,
	Crackers,
	Tomatoes,
	Cucumbers,
	Pepper,
	Mushrooms,
	Potatoes,
	Cabbage,
	Salad,
	Greens,
	Radish,
	Courgettes,
	Aubergines,
	Onions,
	Garlic,
	Avocado,
	Pickles,
	FrozenVegetables,
	Bananas,
	Berries,
	Lemons,
	Lime,
	Oranges,
	Tangerines,
	Mineola,
	Grapefruit,
	Pomelo,
	Grapes,
	Apples,
	Pears,
	Pomegranate,
	Kiwi,
	Persimmon,
	Mango,
	PassionFruit,
	FruitDesserts,
	Pistachios,
	Cashews,
	Almonds,
	Walnut,
	Hazelnut,
	Peanuts,
	PineNuts,
	Macadamia,
	DriedDates,
	Apricots,
	Prunes,
	GojiBerries,
	DriedCherries,
	PorkCarbonade,
	PorkBacon,
	PorkShoulder,
	PorkRibs,
	BeefThighFlesh,
	BeefBacon,
	BeefFlesh,
	BeefGoulash,
	BonelessBeefSteak,
	BeefClassicSteak,
	BeefSteakChuckAyRoll,
	TurkeyBreastSteak,
	MarbleBeefSteakParisienne,
	ThighBroilerChicken,
	BroilerChickenThighFillet,
	BroilerChickenShank,
	BroilerChickenBreastFillet,
	TurkeyMedallions,
	TurkeyAzu,
	GroundBeef,
	TurkeyShankStuffing,
	PorkAndBeefMince,
	ChickenMince,
	Nuggets,
	TurkeyCutlets,
	MeatbollsPork,
	ChickenMeatbolls,
	ChickenCutlets,
	DoctorSausage,
	Cervalat
}

class Product
{
	public Product(ProductType productType, int price)
	{
		ProductType = productType;
		Price = price;
	}

	public ProductType ProductType { get; private set; }

	public int Price { get; private set; }
}

class Client
{
	private List<Product> _foodBasket;
	private int _money;

	public Client(int money, int countProductPurchase)
	{
		_foodBasket = new List<Product>();
		_money = money;
		CountProductPurchase = countProductPurchase;
	}

	public int CountProductPurchase { get; private set; }

	public bool CanBuy(int moneyToPay)
	{
		return _money < moneyToPay;
	}

	public void ShowMoney()
	{
		Console.WriteLine("Денег у клиента - " + _money);
	}
	public void FillFoodBasket(Product product)
	{
		_foodBasket.Add(product);
		Console.WriteLine($"В корзину положили товар: {product.ProductType} " +
				$"стоимостью - {product.Price}");
	}

	public void RemoveProductFromFoodBasket()
	{
		Product productToRemove = _foodBasket[UserUtils.GenerateRandomNumber(1, _foodBasket.Count)];
		Console.WriteLine("Из корзины убрали товар: " + productToRemove.ProductType
			+ " стоимостью - " + productToRemove.Price);
		_foodBasket.Remove(productToRemove);
	}

	public int CalculateCostFoodBasket()
	{
		int cost = 0;

		foreach (Product product in _foodBasket)
			cost += product.Price;

		DisplayCostFoodBasket(cost);
		return cost;
	}

	private void DisplayCostFoodBasket(int cost)
	{
		Console.WriteLine("Общая стоимость товаров в продуктовой корзине - " + cost);
	}
}

class UserUtils
{
	static private Random s_random = new Random();
	public static int GenerateRandomNumber(int min, int max)
	{
		return s_random.Next(min, max);
	}
}