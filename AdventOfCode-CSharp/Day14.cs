using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode_CSharp
{
    class Day14
    {

        const string filePath = "C:\\Users\\Aaron\\source\\repos\\AdventOfCode-CSharp\\AdventOfCode-CSharp\\Resources\\Day14.txt";

        public static void Main()
        {
            var input = new List<string>(System.IO.File.ReadAllLines(filePath));
            List<Reaction> reactions = new List<Reaction>();
            Dictionary<string, long> Orders = new Dictionary<string, long>();
            ParseInput(input, reactions);

            Orders.Add("FUEL", 3279312);
            Console.WriteLine($"Ores needed : {GetOres(reactions, Orders)}");
            Orders.Clear();

            const long target = 1000000000000; 
            long result = 0;
            long previousResult = 0;
            long iterator = 1000;
            while(result < target)
            {
                Orders.Add("FUEL", iterator);
                result = GetOres(reactions, Orders);
                Orders.Clear();
                iterator+=1000;
            }

            result = 0;
            iterator -= 2000;
            while (result < target)
            {
                previousResult = result;
                Orders.Add("FUEL", iterator);
                result = GetOres(reactions, Orders);
                Orders.Clear();
                iterator ++;
            }
            Console.WriteLine($"With {target} ores you can make {iterator-=2} fuel!");
        }

        public static long GetOres(List<Reaction> reactions, Dictionary<string, long> Orders)
        {
            var ores = 0L;
            Dictionary<string, long> overSupply = new Dictionary<string, long>();

            while (Orders.Count > 0)
            {
                Dictionary<string, long> NewOrders = new Dictionary<string, long>();
                foreach (var order in Orders)
                {
                    var reaction = reactions.First(i => i.MainElement.Name == order.Key);
                    var orderAmount = order.Value;
                    if (overSupply.ContainsKey(order.Key))
                    {
                        var amountInOverSupply = overSupply[order.Key];
                        if(amountInOverSupply > order.Value && amountInOverSupply > 0)
                        {
                            overSupply[order.Key] -= orderAmount;
                            orderAmount = 0;
                        }
                        else if(amountInOverSupply <= order.Value && amountInOverSupply > 0)
                        {
                            orderAmount -= amountInOverSupply;
                            overSupply[order.Key] = 0;
                        }
                    }

                    var reactionAmount = reaction.MainElement.Amount;
                    var temp = orderAmount;
                    orderAmount = (long)(Math.Ceiling((double)orderAmount / (double)reactionAmount) * reactionAmount);
                    var overSupplyAmount = orderAmount - temp;

                    if (overSupply.ContainsKey(order.Key))
                    {
                        overSupply[order.Key] = overSupply[order.Key] + overSupplyAmount;
                    }
                    else
                    {
                        overSupply.Add(order.Key, overSupplyAmount);
                    }

                    var factorIngredients = orderAmount / reactionAmount;
                    foreach (var ingredient in reaction.Ingredients)
                    {
                        if (ingredient.Name != "ORE")
                        {
                            if (NewOrders.ContainsKey(ingredient.Name))
                            {
                                NewOrders[ingredient.Name] = NewOrders[ingredient.Name] + (ingredient.Amount * factorIngredients);
                            }
                            else
                            {
                                NewOrders.Add(ingredient.Name, ingredient.Amount * factorIngredients);
                            }
                        }
                        else
                        {
                            ores += ingredient.Amount * factorIngredients;
                        }

                    }
                }
                Orders = NewOrders;
            }
            return ores;
        }



        public static void ParseInput(List<string> inputs, List<Reaction> reactions)
        {
            foreach(var input in inputs)
            {
                var reaction = new Reaction();
                var reactionSplit = input.Trim().Split("=>");
                var mainElementSplit = reactionSplit[1].Trim().Split(" ");
                var mainElement = new Element { Amount = long.Parse(mainElementSplit[0].Trim()), Name = mainElementSplit[1].Trim() };
                reaction.MainElement = mainElement;
                var ingredients = reactionSplit[0].Trim().Split(",");
                foreach(var ingredient in ingredients)
                {
                    var ingredientSplit = ingredient.Trim().Split(" ");
                    var newIngredient = new Element { Amount = long.Parse(ingredientSplit[0].Trim()), Name = ingredientSplit[1].Trim() };
                    reaction.Ingredients.Add(newIngredient);
                }
                reactions.Add(reaction);
            }
        }
    }

    public class Element
    {
        public long Amount { get; set; }
        public string Name { get; set; }
    }

    public class Reaction
    {
        public Reaction()
        {
            Ingredients = new List<Element>();
        }
        public Element MainElement { get; set;}
        public List<Element> Ingredients { get; set; }
    }
}
