using System;
using System.Collections.Generic;
using System.Linq;

namespace Супермаркет
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Supermarket supermarket = new Supermarket();

            supermarket.ServeQueue();
        }
    }

    class Buyer
    {
        private List<Product> _products;

        private int _money;

        public Buyer(int money)
        {
            _money = money;
            _products = new List<Product>();
        }

        public void BuyProduct(List<Product> products)
        {
            foreach (Product product in products)
            {
                _money -= product.Price;
                _products.Add(product);
            }
        }

        public bool CanPay(int price)
        {
            return price <= _money;
        }
    }

    class Cart
    {
        private List<Product> _products;

        public Cart(List<Product> products)
        {
            _products = products;
        }

        public int CalculateProductsPrice()
        {
            int productsSum = 0;

            foreach (Product product in _products)
            {
                productsSum += product.Price;
            }

            return productsSum;
        }

        public void RemoveRandomProduct()
        {
            int randomProduct = Utils.GetRandomValue(_products.Count);

            Console.WriteLine($"Нехватило денег выложен Продукт {_products[randomProduct].Name} с ценой {_products[randomProduct].Price}");

            _products.RemoveAt(randomProduct);
        }

        public List<Product> GetProducts()
        {
            return _products.ToList();
        }
    }

    class Product
    {
        public Product(string name, int price)
        {
            Name = name;
            Price = price;
        }

        public string Name { get; }
        public int Price { get; }

        public override string ToString()
        {
            return $"{Name} {Price}";
        }
    }

    class Supermarket
    {
        private List<Product> _products;

        private int _money = 0;

        public Supermarket()
        {
            _products = CreateProducts();
        }


        public void ServeQueue()
        {
            int bayersNumber = 0;

            Queue<Buyer> buyers = CreateBuyers();

            while (buyers.Count > 0)
            {
                Buyer buyer = buyers.Dequeue();

                bayersNumber++;

                Console.WriteLine($"Покупатель {bayersNumber}");

                Serve(buyer);

                Console.WriteLine();
            }
        }

        private void Serve(Buyer buyer)
        {
            List<Product> products = CreateCart();

            Cart cart = new Cart(products);

            while (buyer.CanPay(cart.CalculateProductsPrice()) == false)
            {
                cart.RemoveRandomProduct();
            }

            buyer.BuyProduct(cart.GetProducts());
            _money += cart.CalculateProductsPrice();

            Console.Write("Куплено: ");

            foreach (Product product in products)
            {
                Console.Write($"{product.Name} {product.Price},");
            }

            Console.WriteLine($"\nСумма покупок {cart.CalculateProductsPrice()}");
            Console.WriteLine($"Выручка магазина {_money}"); ;
        }

        private Queue<Buyer> CreateBuyers()
        {
            int buyersNumber = 6;

            int minMoney = 150;
            int maxMoney = 450;
            int randomMoney = Utils.GetRandomValue(minMoney, maxMoney);

            Queue<Buyer> buyers = new Queue<Buyer>();

            for (int i = 0; i < buyersNumber; i++)
            {
                buyers.Enqueue(new Buyer(randomMoney));
            }

            return buyers;
        }

        private List<Product> CreateProducts()
        {
            List<Product> products = new List<Product>()
            {
                new Product("Банан", 25),
                new Product("Яблоко", 35),
                new Product("Груша", 27),
                new Product("Макароны", 68),
                new Product("Хлеб", 38),
                new Product("Молоко", 78),
                new Product("Говядина", 170),
                new Product("Чипсы", 50),
                new Product("Пиво", 97),
                new Product("Хот-Дог", 45)
            };

            return products;
        }

        private List<Product> CreateCart()
        {
            List<Product> products = new List<Product>();

            int minProductNumber = 3;
            int maxProductNumber = 10;
            int randomProductsCount = Utils.GetRandomValue(minProductNumber, maxProductNumber);

            for (int i = 0; i < randomProductsCount; i++)
            {
                products.Add(_products[Utils.GetRandomValue(_products.Count)]);
            }

            return products;
        }
    }

    public static class Utils
    {
        private static Random _random = new Random();

        public static int GetRandomValue(int minValue, int maxValue)
        {
            return _random.Next(minValue, maxValue);
        }

        public static int GetRandomValue(int maxValue)
        {
            return _random.Next(maxValue);
        }
    }
}
